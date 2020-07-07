Imports Microsoft.VisualBasic
Imports System.ComponentModel

' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/Entity Classes/admin_center_dataLayer.vb $
'$$Author: Amanda $
'$$Date: 7/06/20 4:24p $
'$$Modtime: 7/06/20 4:24p $
'$$Revision: 61 $
'$$Workfile: admin_center_dataLayer.vb $
'
' ********************************************************************************
<System.Serializable()> Public Class developerSelectionCriteriaClass
    Sub New()

        DeveloperCriteriaStatusCode = eObjStatusCode.NULL
        DeveloperCriteriaDetailError = eObjDetailErrorCode.NULL
        DeveloperCriteriaProjectKey = -1
        DeveloperCriteriaProjectTitle = ""
        DeveloperCriteriaProjectPriority = ""
        DeveloperCriteriaProjectStaffName = ""
        DeveloperCriteriaProjectEntryStaffName = ""
        DeveloperCriteriaProjectTask = ""
        DeveloperCriteriaProjectTaskTitle = ""
        DeveloperCriteriaProjectTaskDiscription = ""
        DeveloperCriteriaProjectStatus = ""
        DeveloperCriteriaProjectFollowUp = ""
        DeveloperCriteriaToggleDisplay = False

    End Sub

    Public Property DeveloperCriteriaStatusCode() As eObjStatusCode

    Public Property DeveloperCriteriaDetailError() As eObjDetailErrorCode

    Public Property DeveloperCriteriaProjectKey() As Integer

    Public Property DeveloperCriteriaProjectTitle() As String

    Public Property DeveloperCriteriaProjectPriority() As String

    Public Property DeveloperCriteriaProjectEntryStaffName() As String

    Public Property DeveloperCriteriaProjectStaffName() As String

    Public Property DeveloperCriteriaProjectTask() As String

    Public Property DeveloperCriteriaProjectTaskTitle() As String

    Public Property DeveloperCriteriaProjectTaskDiscription() As String

    Public Property DeveloperCriteriaProjectStatus() As String

    Public Property DeveloperCriteriaProjectFollowUp() As String

    Public Property DeveloperCriteriaToggleDisplay() As Boolean

End Class

<System.Serializable()> Public Class helpAdminSelectionCriteriaClass

    Sub New()

        HelpCriteriaStatusCode = eObjStatusCode.NULL
        HelpCriteriaDetailError = eObjDetailErrorCode.NULL

        HelpCriteriaItemID = -1
        HelpCriteriaItemStatus = False

        HelpCriteriaModelID = -1
        HelpCriteriaSubID = -1
        HelpCriteriaCompanyID = -1
        HelpCriteriaAdminOnly = False

        HelpCriteriaItemReleaseDate = ""
        HelpCriteriaItemReleaseType = ""
        HelpCriteriaItemTitle = ""
        HelpCriteriaItemDiscription = ""

        HelpCriteriaItemVideoLink = ""
        HelpCriteriaItemDocumentLink = ""
        HelpCriteriaItemTopicList = ""

        HelpCriteriaItemViewNumber = ""
        HelpCriteriaItemTabName = ""
        HelpCriteriaBusinessFlag = False
        HelpCriteriaHelicopterFlag = False
        HelpCriteriaCommercialFlag = False
        HelpCriteriaYachtFlag = False
        HelpCriteriaNewEvoFlag = False
        HelpCriteriaNewEvoOnlyFlag = False
        HelpCriteriaOldEvoFlag = False
        HelpCriteriaCRMFlag = False

    End Sub

    Public Property HelpCriteriaStatusCode() As eObjStatusCode

    Public Property HelpCriteriaDetailError() As eObjDetailErrorCode

    Public Property HelpCriteriaItemID() As Integer

    Public Property HelpCriteriaModelID() As Long

    Public Property HelpCriteriaSubID() As Long

    Public Property HelpCriteriaCompanyID() As Long

    Public Property HelpCriteriaAdminOnly() As Boolean

    Public Property HelpCriteriaItemStatus() As Boolean

    Public Property HelpCriteriaItemReleaseDate() As String

    Public Property HelpCriteriaItemReleaseType() As String

    Public Property HelpCriteriaItemTitle() As String

    Public Property HelpCriteriaItemDiscription() As String

    Public Property HelpCriteriaItemVideoLink() As String

    Public Property HelpCriteriaItemDocumentLink() As String

    Public Property HelpCriteriaItemTopicList() As String

    Public Property HelpCriteriaBusinessFlag() As Boolean

    Public Property HelpCriteriaHelicopterFlag() As Boolean

    Public Property HelpCriteriaCommercialFlag() As Boolean

    Public Property HelpCriteriaYachtFlag() As Boolean

    Public Property HelpCriteriaNewEvoFlag() As Boolean

    Public Property HelpCriteriaNewEvoOnlyFlag() As Boolean

    Public Property HelpCriteriaOldEvoFlag() As Boolean

    Public Property HelpCriteriaCRMFlag() As Boolean

    Public Property HelpCriteriaItemViewNumber() As String

    Public Property HelpCriteriaItemTabName() As String

End Class  ' 

<System.Serializable()> Public Class onLineUsersSelectionCriteriaClass

    Sub New()

        OnLineCriteriaStatusCode = eObjStatusCode.NULL
        OnLineCriteriaDetailError = eObjDetailErrorCode.NULL

        OnLineCriteriaCompanyID = -1
        OnLineCriteriaContactID = -1

        OnLineCriteriaNumberToShow = 0

        OnLineCriteriaByBrowser = False
        OnLineCriteriaByNew = False

        OnLineCriteriaSearchItem = ""
        OnLineCriteriaBusType = ""
        OnLineCriteriaProductCode = ""
        OnLineCriteriaService = ""

        OnLineCriteriaSelectedItem = ""
        OnLineCriteriaFrequency = ""
        OnLineCriteriaOrderBy = ""
        OnLineCriteriaInfo = ""
        OnLineCriteriaServer = ""
        OnLineCriteriaPlatformType = ""

    End Sub

    Public Property OnLineCriteriaStatusCode() As eObjStatusCode

    Public Property OnLineCriteriaDetailError() As eObjDetailErrorCode

    Public Property OnLineCriteriaNumberToShow() As Integer

    Public Property OnLineCriteriaCompanyID() As Long

    Public Property OnLineCriteriaContactID() As Long

    Public Property OnLineCriteriaByBrowser() As Boolean

    Public Property OnLineCriteriaByNew() As Boolean

    Public Property OnLineCriteriaSearchItem() As String

    Public Property OnLineCriteriaBusType() As String

    Public Property OnLineCriteriaProductCode() As String

    Public Property OnLineCriteriaService() As String

    Public Property OnLineCriteriaSelectedItem() As String

    Public Property OnLineCriteriaFrequency() As String

    Public Property OnLineCriteriaOrderBy() As String

    Public Property OnLineCriteriaInfo() As String

    Public Property OnLineCriteriaServer() As String

    Public Property OnLineCriteriaPlatformType() As String

End Class  ' 

<System.Serializable()> Public Class adminBackgroundCriteriaClass
    Sub New()

        BkndCriteriaStatusCode = eObjStatusCode.NULL
        BkndCriteriaDetailError = eObjDetailErrorCode.NULL
        BkndCriteriaItemID = -1
        BkndCriteriaItemNew = False
        BkndCriteriaItemTitle = ""
        BkndCriteriaItemLink = ""
        BkndCriteriaBusinessFlag = False
        BkndCriteriaHelicopterFlag = False
        BkndCriteriaCommercialFlag = False
        BkndCriteriaYachtFlag = False
        BkndCriteriaAerodexFlag = False
        BkndCriteriaFeatureFlag = False
        BkndCriteriaItemStatus = False

    End Sub

    Public Property BkndCriteriaStatusCode() As eObjStatusCode

    Public Property BkndCriteriaDetailError() As eObjDetailErrorCode

    Public Property BkndCriteriaItemID() As Integer

    Public Property BkndCriteriaItemNew() As Boolean

    Public Property BkndCriteriaItemStatus() As Boolean

    Public Property BkndCriteriaItemTitle() As String

    Public Property BkndCriteriaItemLink() As String

    Public Property BkndCriteriaBusinessFlag() As Boolean

    Public Property BkndCriteriaHelicopterFlag() As Boolean

    Public Property BkndCriteriaCommercialFlag() As Boolean

    Public Property BkndCriteriaYachtFlag() As Boolean

    Public Property BkndCriteriaAerodexFlag() As Boolean

    Public Property BkndCriteriaFeatureFlag() As Boolean

End Class

<System.Serializable()> Public Class admin_center_dataLayer

    Private aError As String
    Private clientConnectString As String
    Private adminConnectString As String

    Private starConnectString As String
    Private cloudConnectString As String
    Private serverConnectString As String

    Private taskerConnectString As String
    Private crmMasterConnectString As String

    Sub New()
        aError = ""
        clientConnectString = ""
        adminConnectString = ""

        starConnectString = ""
        cloudConnectString = ""
        serverConnectString = ""
        taskerConnectString = ""
        crmMasterConnectString = ""

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

    Public Property taskerConnectStr() As String
        Get
            taskerConnectStr = taskerConnectString
        End Get
        Set(ByVal value As String)
            taskerConnectString = value
        End Set
    End Property

    Public Property crmMasterConnectStr() As String
        Get
            crmMasterConnectStr = crmMasterConnectString
        End Get
        Set(ByVal value As String)
            crmMasterConnectString = value
        End Set
    End Property

#End Region

#Region "admin_online_now_page_functions"


    Public Sub ticker_selects(ByRef total_licenses As Integer, ByRef up_down_licenses As Integer, ByRef total_marketplace As Integer, ByRef up_down_marketplace As Integer, ByRef total_aerodex As Integer, ByRef up_down_aerodex As Integer, ByRef total_last As Integer, ByRef marketplace_last As Integer, ByRef aerodex_last As Integer, ByVal go_back_year As Boolean)

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader = Nothing
        Dim SqlReader2 As SqlClient.SqlDataReader = Nothing
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim temp_query As String = ""
        Dim atemptable As New DataTable
        Dim atemptable2 As New DataTable
        Dim atemptable3 As New DataTable


        Try
            SqlConn.ConnectionString = adminConnectStr

            SqlConn.Open()

            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = System.Data.CommandType.Text
            SqlCommand.CommandTimeout = 240


            ' -- ****************************************************************************************************************
            '-- CUSTOMER - VALUES CUSTOMER GROWTH
            ' -- GET NUMBER OF TOTAL LICENSES TODAY VS LAST MONTH
            temp_query = " select top 1 cstat_year, cstat_month,  cstat_total, "
            temp_query &= " (select COUNT(*) "
            temp_query &= " from View_JETNET_Customers wtih (NOLOCK) "
            temp_query &= " where (sub_comp_id <> 135887) AND (sublogin_demo_flag = 'N')  and sub_serv_code not Like '%FS' ) AS CURRENTTOTAL  "
            temp_query &= " from Customer_Statistics with (NOLOCK) "
            temp_query &= " where  cstat_type='Licenses' "
            If go_back_year = True Then
                temp_query &= "  And cstat_month = 12 And cstat_year = (year(getdate()) - 1) "
            End If
            temp_query &= " order by cstat_year desc, cstat_month desc "

            SqlCommand.CommandText = temp_query
            SqlReader2 = SqlCommand.ExecuteReader()

            Try
                atemptable3.Load(SqlReader2)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />Error in current_subscriber_summary load datatable " + constrExc.Message
            End Try

            If Not IsNothing(atemptable3) Then
                If atemptable3.Rows.Count > 0 Then
                    For Each es As DataRow In atemptable3.Rows
                        total_licenses = es.Item("CURRENTTOTAL")
                        total_last = es.Item("cstat_total")

                        up_down_licenses = (es.Item("CURRENTTOTAL") - es.Item("cstat_total"))
                    Next
                End If
            End If
            SqlReader2.Close()




            ' -- GET NUMBER OF TOTAL LICENSES TODAY VS LAST MONTH
            temp_query = " Select top 1 cstat_year, cstat_month,  cstat_marketplace, "
            temp_query &= " (select SUM(CASE WHEN sub_aerodex_flag='N' THEN 1 ELSE 0 END)  "
            temp_query &= "  From View_JETNET_Customers wtih (NOLOCK) "
            temp_query &= " Where (sub_comp_id <> 135887) And (sublogin_demo_flag = 'N') and sub_serv_code not Like '%FS' ) AS CURRMARKETPLACE  "
            temp_query &= " From Customer_Statistics with (NOLOCK) "
            temp_query &= " Where cstat_type ='Licenses' "
            If go_back_year = True Then
                temp_query &= "  And cstat_month = 12 And cstat_year = (year(getdate()) - 1) "
            End If
            temp_query &= "  order by cstat_year desc, cstat_month desc"


            SqlCommand.CommandText = temp_query
            SqlReader2 = SqlCommand.ExecuteReader()

            Try
                atemptable2.Load(SqlReader2)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />Error in current_subscriber_summary load datatable " + constrExc.Message
            End Try

            If Not IsNothing(atemptable2) Then
                If atemptable2.Rows.Count > 0 Then
                    For Each es As DataRow In atemptable2.Rows
                        total_marketplace = es.Item("CURRMARKETPLACE")
                        marketplace_last = es.Item("cstat_marketplace")

                        up_down_marketplace = (es.Item("CURRMARKETPLACE") - es.Item("cstat_marketplace"))
                    Next
                End If
            End If
            SqlReader2.Close()


            '  -- GET NUMBER OF AERODEX LICENSES TODAY VS LAST MONTH 
            temp_query = "Select top 1 cstat_year, cstat_month,  cstat_aerodex, "
            temp_query &= " (select SUM(CASE WHEN sub_aerodex_flag='Y' THEN 1 ELSE 0 END)   "
            temp_query &= " From View_JETNET_Customers wtih (NOLOCK) "
            temp_query &= "  Where (sub_comp_id <> 135887) And (sublogin_demo_flag = 'N') and sub_serv_code not Like '%FS' ) AS CURRAERODEX  "
            temp_query &= "  From Customer_Statistics with (NOLOCK)"
            temp_query &= "  Where cstat_type ='Licenses' "
            If go_back_year = True Then
                temp_query &= "  And cstat_month =12 And cstat_year = (year(getdate()) - 1) "
            End If
            temp_query &= " order by cstat_year desc, cstat_month desc"

            SqlCommand.CommandText = temp_query
            SqlReader2 = SqlCommand.ExecuteReader()

            Try
                atemptable.Load(SqlReader2)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />Error in current_subscriber_summary load datatable " + constrExc.Message
            End Try

            If Not IsNothing(atemptable) Then
                If atemptable.Rows.Count > 0 Then
                    For Each es As DataRow In atemptable.Rows
                        total_aerodex = es.Item("CURRAERODEX")
                        aerodex_last = es.Item("cstat_aerodex")

                        up_down_aerodex = (es.Item("CURRAERODEX") - es.Item("cstat_aerodex"))
                    Next
                End If
            End If
            SqlReader2.Close()


            SqlReader2 = Nothing


        Catch ex As Exception
            SqlCommand.Dispose()
            SqlCommand = Nothing

            SqlConn.Close()
            SqlConn.Dispose()
            SqlConn = Nothing
        End Try



    End Sub

    Public Function displayAdminOnLine(ByVal productCode As String, ByVal type_to_show As String, ByVal companyID As Long, ByVal bShowHostNames As Boolean, ByVal errors As Boolean, ByVal error_count As Integer, ByRef total_users_on As Integer, ByRef AerodexCount As Integer, ByRef live_sub_counter As Integer, ByRef weekly_sub_counter As Integer, ByRef monthly_sub_counter As Integer, ByRef biweekly_sub_counter As Integer, ByRef TotalBusinessCounter As Integer, ByRef TotalCommCounter As Integer, ByRef TotalHeliCounter As Integer, ByRef TotalYachtCounter As Integer, ByVal complete_summary As String, ByVal growth_summary As String, ByRef growth_graph1 As String, ByRef growth_graph2 As String, ByVal location_text As String) As String

        ' Dim live_sub_counter As Integer = 0

        Dim live_bus As Integer = 0
        Dim live_hel As Integer = 0
        Dim live_comm As Integer = 0
        Dim live_aero As Integer = 0
        Dim live_yacht As Integer = 0
        Dim live_t1 As Integer = 0
        Dim live_t2 As Integer = 0
        Dim live_t3 As Integer = 0
        Dim live_spi As Integer = 0
        Dim live_crm As Integer = 0

        'Dim weekly_sub_counter As Integer = 0

        Dim week_bus As Integer = 0
        Dim week_hel As Integer = 0
        Dim week_comm As Integer = 0
        Dim week_aero As Integer = 0
        Dim week_yacht As Integer = 0
        Dim week_t1 As Integer = 0
        Dim week_t2 As Integer = 0
        Dim week_t3 As Integer = 0
        Dim week_spi As Integer = 0
        Dim week_crm As Integer = 0

        'Dim monthly_sub_counter As Integer = 0

        Dim month_bus As Integer = 0
        Dim month_hel As Integer = 0
        Dim month_comm As Integer = 0
        Dim month_aero As Integer = 0
        Dim month_yacht As Integer = 0
        Dim month_t1 As Integer = 0
        Dim month_t2 As Integer = 0
        Dim month_t3 As Integer = 0
        Dim month_spi As Integer = 0
        Dim month_crm As Integer = 0

        'Dim biweekly_sub_counter As Integer = 0

        Dim biweek_bus As Integer = 0
        Dim biweek_hel As Integer = 0
        Dim biweek_comm As Integer = 0
        Dim biweek_aero As Integer = 0
        Dim biweek_yacht As Integer = 0
        Dim biweek_t1 As Integer = 0
        Dim biweek_t2 As Integer = 0
        Dim biweek_t3 As Integer = 0
        Dim biweek_spi As Integer = 0
        Dim biweek_crm As Integer = 0

        Dim total_user_counter As Integer = 0
        ' Dim total_business_counter As Integer = 0
        'Dim total_heli_counter As Integer = 0
        'Dim total_comm_counter As Integer = 0

        Dim total_counted_browser As Integer = 0
        Dim total_counted_browser_percent As Double = 0
        Dim total_counted_platform As Integer = 0
        Dim total_counted_platform_percent As Double = 0

        Dim notes_plus_count As Integer = 0
        Dim server_notes_count As Integer = 0

        Dim new_evo_count As Integer = 0
        Dim new_evo_test_count As Integer = 0
        Dim yacht_spot_count As Integer = 0
        Dim yacht_spot_test_count As Integer = 0
        Dim local_count As Integer = 0

        Dim evo_admin_count As Integer = 0 '
        Dim evo_admin_test_count As Integer = 0 '
        Dim jetnet_admin_count As Integer = 0
        Dim jetnet_admin_test_count As Integer = 0

        Dim old_evo_count As Integer = 0

        Dim evo_mobile_on_count As Integer = 0
        Dim jetnet_mobile_on_count As Integer = 0
        Dim evo_mobile_test_on_count As Integer = 0

        Dim test_count As Integer = 0

        Dim homebase_count As Integer = 0

        Dim sQuery = New StringBuilder()
        Dim htmlOut As New StringBuilder
        Dim tmpHtmlOut As New StringBuilder

        Dim text_for_type As String = ""

        Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
        Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
        Dim my1 As MySql.Data.MySqlClient.MySqlDataReader

        Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing

        'Dim total_users_on As Integer = 0
        Dim master_monthly As Integer = 0
        Dim master_weekly As Integer = 0
        Dim master_live As Integer = 0
        Dim master_biweekly As Integer = 0

        Dim salesforce_live As Integer = 0
        Dim salesforce_monthly As Integer = 0
        Dim salesforce_weekly As Integer = 0
        Dim salesforce_total As Integer = 0


        Dim count_of_current_crm_users As Integer = 0

        Dim strSQL As String = ""
        Dim string_master As String = ""

        Dim counter1_market As Integer = 0
        Dim counter1_aero As Integer = 0

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader = Nothing
        Dim SqlReader2 As SqlClient.SqlDataReader = Nothing
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim sSeperator As String = ""
        Dim atemptable As New DataTable
        Dim atemptable2 As New DataTable
        Dim atemptable3 As New DataTable
        Dim atemptable4 As New DataTable
        Dim temp_query As String = ""


        Try



            ' GET COUNT OF NUMBER OF USERS FOR EACH COMPANY CURRENTLY LOGGED INTO SYSTEM

            If type_to_show.Trim.ToUpper.Contains("ALL") Then

                sQuery.Append("SELECT distinct sub_id, sub_comp_id, sub_frequency, sub_aerodex_flag, sub_busair_tier_level, sub_sale_price_flag, sub_serv_code,")
                sQuery.Append(" sub_business_aircraft_flag, sub_helicopters_flag, sub_commerical_flag, sub_yacht_flag, sub_server_side_notes_flag, sub_cloud_notes_flag, sub_business_aircraft_flag ")

                sQuery.Append(" FROM View_JETNET_Customers")

                If Trim(location_text) <> "All" Then
                    sQuery.Append(" inner Join country with (NOLOCK) on country_name = comp_country ")
                End If

                sQuery.Append(" WHERE sub_start_date <= '" + Now().ToShortDateString + "' AND (sub_end_date IS NULL OR sub_end_date >= '" + Now().ToShortDateString + "')")

                sQuery.Append(" AND sub_comp_id <> 135887")


                If Trim(location_text) <> "All" Then
                    If Trim(location_text) = "EMEA" Then
                        sQuery.Append(" and (country_continent_name in ('Europe','Africa') or comp_country in ('Armenia', 'Azerbaijan', 'Bahrain', 'Georgia', 'Iran', 'Iraq', 'Israel', 'Jordan', 'Kuwait', 'Lebanon', 'Oman', 'Palestine', 'Qatar', 'Saudi Arabia', 'Syria', 'Turkey', 'United Arab Emirates', 'Yemen')) ")
                    ElseIf Trim(location_text) = "Non EMEA" Then
                        sQuery.Append(" and not (country_continent_name in ('Europe','Africa') or comp_country in ('Armenia', 'Azerbaijan', 'Bahrain', 'Georgia', 'Iran', 'Iraq', 'Israel', 'Jordan', 'Kuwait', 'Lebanon', 'Oman', 'Palestine', 'Qatar', 'Saudi Arabia', 'Syria', 'Turkey', 'United Arab Emirates', 'Yemen')) ")
                    End If
                End If


                If Not String.IsNullOrEmpty(productCode.Trim) Then

                    sQuery.Append(" And (")

                    If productCode.ToUpper.Trim.Contains("B") Then

                        sQuery.Append("sub_business_aircraft_flag = 'Y'")

                        If String.IsNullOrEmpty(text_for_type) Then
                            text_for_type = "Business"
                        Else
                            text_for_type += ", Business"
                        End If

                        sSeperator = Constants.cOrClause

                    End If

                    If productCode.ToUpper.Trim.Contains("H") Then

                        sQuery.Append(sSeperator + "sub_helicopters_flag = 'Y'")

                        If String.IsNullOrEmpty(text_for_type) Then
                            text_for_type = "Helicopter"
                        Else
                            text_for_type += ", Helicopter"
                        End If

                        sSeperator = Constants.cOrClause

                    End If

                    If productCode.ToUpper.Trim.Contains("C") Then

                        sQuery.Append(sSeperator + "sub_commerical_flag = 'Y'")

                        If String.IsNullOrEmpty(text_for_type) Then
                            text_for_type = "Commercial"
                        Else
                            text_for_type += ", Commercial"
                        End If

                        sSeperator = Constants.cOrClause

                    End If

                    If productCode.ToUpper.Trim.Contains("Y") Then

                        sQuery.Append(sSeperator + "sub_yacht_flag = 'Y'")

                        If String.IsNullOrEmpty(text_for_type) Then
                            text_for_type = "Yacht"
                        Else
                            text_for_type += ", Yacht"
                        End If

                    End If

                    sQuery.Append(")")

                Else
                    text_for_type = "All "
                End If

                If companyID > 0 Then
                    sQuery.Append(" AND comp_id = " + companyID.ToString)
                End If

            Else
                sQuery.Append("SELECT sub_comp_id, sub_frequency, sub_aerodex_flag, sub_busair_tier_level, sub_sale_price_flag, sub_serv_code,")
                sQuery.Append(" sublogin_sub_id, contact_last_name, sublogin_password, contact_first_name, contact_email_address,")
                sQuery.Append(" comp_name, comp_city, comp_state, sub_business_aircraft_flag, sub_helicopters_flag, sub_commerical_flag, sub_yacht_flag,")
                sQuery.Append(" subins_last_login_date, subins_last_session_date, subins_last_logout_date,")
                sQuery.Append(" subins_activex_flag, subins_local_db_flag, subins_evo_mobile_flag, sub_server_side_notes_flag, sub_cloud_notes_flag")

                If bShowHostNames Then
                    sQuery.Append(", (SELECT TOP 1 subislog_host_name FROM Subscription_Install_Log WITH (NOLOCK) WHERE subislog_subid = sub_id")
                    sQuery.Append(" AND subislog_login = subins_login AND subislog_seq_no = subins_seq_no AND subislog_message")
                    sQuery.Append(" LIKE '%User Has Logged In%' AND subislog_date >= GETDATE() - 1 ORDER BY subislog_id DESC) AS website_host_name")
                End If

                sQuery.Append(" FROM View_JETNET_Customers")

                If Trim(location_text) <> "All" Then
                    sQuery.Append(" inner Join country with (NOLOCK) on country_name = comp_country ")
                End If

                If Trim(complete_summary) = "Y" Then
                    sQuery.Append(" where  (sub_comp_id <> 135887) AND (sublogin_demo_flag = 'N')  ")
                Else
                    sQuery.Append(" WHERE subins_last_session_date >= '" + DateAdd("n", -10, Now()).ToString.Trim + "'")
                    sQuery.Append(" AND (subins_last_session_date <> subins_last_logout_date OR subins_last_logout_date IS NULL)")
                End If

                'added MSW - 11/29/19
                sQuery.Append(" and sub_serv_code not Like '%FS' ")


                If Trim(location_text) <> "All" Then
                    If Trim(location_text) = "EMEA" Then
                        sQuery.Append(" and (country_continent_name in ('Europe','Africa') or comp_country in ('Armenia', 'Azerbaijan', 'Bahrain', 'Georgia', 'Iran', 'Iraq', 'Israel', 'Jordan', 'Kuwait', 'Lebanon', 'Oman', 'Palestine', 'Qatar', 'Saudi Arabia', 'Syria', 'Turkey', 'United Arab Emirates', 'Yemen')) ")
                    ElseIf Trim(location_text) = "Non EMEA" Then
                        sQuery.Append(" and not (country_continent_name in ('Europe','Africa') or comp_country in ('Armenia', 'Azerbaijan', 'Bahrain', 'Georgia', 'Iran', 'Iraq', 'Israel', 'Jordan', 'Kuwait', 'Lebanon', 'Oman', 'Palestine', 'Qatar', 'Saudi Arabia', 'Syria', 'Turkey', 'United Arab Emirates', 'Yemen')) ")
                    End If
                End If

                If companyID > 0 Then
                    sQuery.Append(" AND comp_id = " + companyID.ToString)
                End If

                sQuery.Append(" ORDER BY comp_name, comp_city, comp_state ASC")
            End If

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>current_subscriber_summary(ByVal productCode As String, ByVal type_to_show As String, ByVal companyID As Long, ByVal errors As Boolean, ByVal error_count As Integer) As String</b><br />" + sQuery.ToString

            SqlConn.ConnectionString = adminConnectStr

            SqlConn.Open()

            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = System.Data.CommandType.Text
            SqlCommand.CommandTimeout = 240

            If Not errors Then

                SqlCommand.CommandText = sQuery.ToString
                SqlReader = SqlCommand.ExecuteReader()

                Try
                    atemptable.Load(SqlReader)
                Catch constrExc As System.Data.ConstraintException
                    Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />Error in current_subscriber_summary load datatable " + constrExc.Message
                End Try

                SqlReader.Close()
                SqlReader = Nothing

                htmlOut.Append("<div class=""Box""><table width=""95%""><tr><td width=""25%""><div class=""subHeader"">")

                If type_to_show.Trim.ToUpper.Contains("ALL") Then
                    htmlOut.Append("Evolution Subscriber Summary")
                ElseIf Trim(complete_summary) = "Y" Then
                    htmlOut.Append("User Summary")
                Else
                    htmlOut.Append("User Summary")
                End If

                htmlOut.Append("</div></td><td width=""50%"">")

                If type_to_show.Trim.ToUpper.Contains("ALL") Then

                    htmlOut.Append("<table align=""center"" width=""95%"" class=""formatTable blue""><tr><td align=""center"">")

                    htmlOut.Append("<input type=""hidden"" name=""type_to_show"" value=""all"">")

                    If String.IsNullOrEmpty(productCode.Trim) Then
                        htmlOut.Append("Business <input type=""checkbox"" name=""productCode"" value=""B"" checked=""checked"">&nbsp;&nbsp;")
                        htmlOut.Append("Commercial <input type=""checkbox"" name=""productCode"" value=""C"" checked=""checked"">&nbsp;&nbsp;")
                        htmlOut.Append("Helicopters <input type=""checkbox"" name=""productCode"" value=""H"" checked=""checked"">&nbsp;&nbsp;")
                        htmlOut.Append("Yachts <input type=""checkbox"" name=""productCode"" value=""Y"" checked=""checked"">&nbsp;&nbsp;")
                    Else
                        If productCode.ToUpper.Trim.Contains("B") Then
                            htmlOut.Append("Business <input type=""checkbox"" name=""productCode"" value=""B"" checked=""checked"">&nbsp;&nbsp;")
                        Else
                            htmlOut.Append("Business <input type=""checkbox"" name=""productCode"" value=""B"">&nbsp;&nbsp;")
                        End If

                        If productCode.ToUpper.Trim.Contains("C") Then
                            htmlOut.Append("Commercial <input type=""checkbox"" name=""productCode"" value=""C"" checked=""checked"">&nbsp;&nbsp;")
                        Else
                            htmlOut.Append("Commercial <input type=""checkbox"" name=""productCode"" value=""C"">&nbsp;&nbsp;")
                        End If

                        If productCode.ToUpper.Trim.Contains("H") Then
                            htmlOut.Append("Helicopters <input type=""checkbox"" name=""productCode"" value=""H"" checked=""checked"">&nbsp;&nbsp;")
                        Else
                            htmlOut.Append("Helicopters <input type=""checkbox"" name=""productCode"" value=""H"">&nbsp;&nbsp;")
                        End If

                        If productCode.ToUpper.Trim.Contains("Y") Then
                            htmlOut.Append("Yachts <input type=""checkbox"" name=""productCode"" value=""Y"" checked=""checked"">")
                        Else
                            htmlOut.Append("Yachts <input type=""checkbox"" name=""productCode"" value=""Y"">")
                        End If

                    End If

                    htmlOut.Append("<input type=""submit"" value=""Refresh"">")
                    htmlOut.Append("</td></tr></table>")

                    htmlOut.Append("</td><td width=""25%"">")
                    htmlOut.Append("<table align=""right"" cellspacing=""2"" cellpadding=""2""><tr><td><a class=""underline pointer"" href=""adminOnline.aspx"">Online Now<a/></td></tr></table>")
                Else
                    htmlOut.Append("</td><td width=""25%"">")
                    '  htmlOut.Append("<table align=""right"" cellspacing=""2"" cellpadding=""2""><tr><td><a class=""underline pointer"" href=""adminOnline.aspx?complete=Y"">Complete User Summary</a></td><td><a class=""underline pointer"" href=""adminOnline.aspx?type_to_show=all"">Evolution Subscriber Overview</a></td></tr></table>")
                End If

                htmlOut.Append("</td></tr></table></div>")

                htmlOut.Append("<div class=""row""><div class=""Box""><table valign=top width=""95%"" border=""0"" cellspacing=""2"" cellpadding=""2"" class=""formatTable blue""><thead><tr>")
                htmlOut.Append("<th>FREQUENCY</th>")
                htmlOut.Append("<th class=""text_align_right"">USERS</th>")
                htmlOut.Append("<th class=""text_align_right"">BUSINESS</th>")
                htmlOut.Append("<th class=""text_align_right"">JETS</th>")
                htmlOut.Append("<th class=""text_align_right"">T/P</th>")
                htmlOut.Append("<th class=""text_align_right"">ALL BUS</th>")
                htmlOut.Append("<th class=""text_align_right"">HELICOPTER</th>")
                htmlOut.Append("<th class=""text_align_right"">COMMERCIAL</th>")
                htmlOut.Append("<th class=""text_align_right"">YACHT</th>")
                htmlOut.Append("<th class=""text_align_right"">AERODEX</th>")
                htmlOut.Append("<th class=""text_align_right"">VALUES</th>")
                htmlOut.Append("<th class=""text_align_right"">MPM</th>")
                htmlOut.Append("<th class=""text_align_right"">SF</th>")
                If Trim(complete_summary) = "Y" Then
                Else
                    htmlOut.Append("<th class=""text_align_right"">MPM + EVO</th>")
                End If


                If type_to_show.Trim.ToUpper.Contains("ALL") Then
                    htmlOut.Append("<th class=""text_align_right"">SVC CODE CRM</th>")
                End If

                htmlOut.Append("</tr></thead><tbody>")

                ' collect subscriber data
                If atemptable.Rows.Count > 0 Then

                    For Each es As DataRow In atemptable.Rows

                        total_user_counter += 1

                        If Not type_to_show.Trim.ToUpper.Contains("ALL") Then


                            If Not IsDBNull(es.Item("sub_cloud_notes_flag")) Then
                                If es.Item("sub_cloud_notes_flag").ToString.ToUpper.Contains("Y") Then
                                    notes_plus_count += 1
                                End If
                            End If

                            If Not IsDBNull(es.Item("sub_server_side_notes_flag")) Then
                                If es.Item("sub_server_side_notes_flag").ToString.ToUpper.Contains("Y") Then
                                    server_notes_count += 1
                                End If
                            End If

                        End If

                        If Not IsDBNull(es.Item("sub_frequency")) Then

                            If es.Item("sub_frequency").ToString.ToUpper.Trim.Contains("LIVE") Then

                                live_sub_counter += 1

                                If Not IsDBNull(es.Item("sub_business_aircraft_flag")) Then
                                    If es.Item("sub_business_aircraft_flag").ToString.ToUpper.Contains("Y") Then
                                        live_bus += 1
                                    End If
                                End If

                                If Not IsDBNull(es.Item("sub_helicopters_flag")) Then
                                    If es.Item("sub_helicopters_flag").ToString.ToUpper.Contains("Y") Then
                                        live_hel += 1
                                    End If
                                End If

                                If Not IsDBNull(es.Item("sub_commerical_flag")) Then
                                    If es.Item("sub_commerical_flag").ToString.ToUpper.Contains("Y") Then
                                        live_comm += 1
                                    End If
                                End If

                                If Not IsDBNull(es.Item("sub_aerodex_flag")) Then
                                    If es.Item("sub_aerodex_flag").ToString.ToUpper.Contains("Y") Then
                                        live_aero += 1
                                    End If
                                End If

                                If Not IsDBNull(es.Item("sub_yacht_flag")) Then
                                    If es.Item("sub_yacht_flag").ToString.ToUpper.Contains("Y") Then
                                        live_yacht += 1
                                    End If
                                End If


                                If Not IsDBNull(es.Item("sub_business_aircraft_flag")) Then
                                    If es.Item("sub_business_aircraft_flag").ToString.ToUpper.Contains("Y") Then
                                        If Not IsDBNull(es.Item("sub_busair_tier_level")) Then
                                            If Not String.IsNullOrEmpty(es.Item("sub_busair_tier_level").ToString.Trim) Then
                                                If CInt(es.Item("sub_busair_tier_level").ToString) = 1 Then
                                                    live_t1 += 1
                                                ElseIf CInt(es.Item("sub_busair_tier_level").ToString) = 2 Then
                                                    live_t2 += 1
                                                Else
                                                    live_t3 += 1
                                                End If
                                            End If
                                        End If
                                    End If
                                End If



                                If Not IsDBNull(es.Item("sub_sale_price_flag")) Then
                                    If es.Item("sub_sale_price_flag").ToString.ToUpper.Contains("Y") Then
                                        live_spi += 1
                                    End If
                                End If

                                If Not IsDBNull(es.Item("sub_serv_code")) Then
                                    If Not String.IsNullOrEmpty(es.Item("sub_serv_code").ToString.Trim) Then

                                        If es.Item("sub_serv_code").ToString.ToUpper.Contains("CRM") Then
                                            live_crm += 1
                                        End If

                                    End If
                                End If

                            ElseIf es.Item("sub_frequency").ToString.ToUpper.Trim.Contains("WEEKLY") Then

                                weekly_sub_counter += 1

                                If Not IsDBNull(es.Item("sub_business_aircraft_flag")) Then
                                    If es.Item("sub_business_aircraft_flag").ToString.ToUpper.Contains("Y") Then
                                        week_bus += 1
                                    End If
                                End If

                                If Not IsDBNull(es.Item("sub_helicopters_flag")) Then
                                    If es.Item("sub_helicopters_flag").ToString.ToUpper.Contains("Y") Then
                                        week_hel += 1
                                    End If
                                End If

                                If Not IsDBNull(es.Item("sub_commerical_flag")) Then
                                    If es.Item("sub_commerical_flag").ToString.ToUpper.Contains("Y") Then
                                        week_comm += 1
                                    End If
                                End If

                                If Not IsDBNull(es.Item("sub_aerodex_flag")) Then
                                    If es.Item("sub_aerodex_flag").ToString.ToUpper.Contains("Y") Then
                                        week_aero += 1
                                    End If
                                End If

                                If Not IsDBNull(es.Item("sub_yacht_flag")) Then
                                    If es.Item("sub_yacht_flag").ToString.ToUpper.Contains("Y") Then
                                        week_yacht += 1
                                    End If
                                End If

                                If Not IsDBNull(es.Item("sub_business_aircraft_flag")) Then
                                    If es.Item("sub_business_aircraft_flag").ToString.ToUpper.Contains("Y") Then
                                        If Not IsDBNull(es.Item("sub_busair_tier_level")) Then
                                            If Not String.IsNullOrEmpty(es.Item("sub_busair_tier_level").ToString.Trim) Then
                                                If CInt(es.Item("sub_busair_tier_level").ToString) = 1 Then
                                                    week_t1 += 1
                                                ElseIf CInt(es.Item("sub_busair_tier_level").ToString) = 2 Then
                                                    week_t2 += 1
                                                Else
                                                    week_t3 += 1
                                                End If
                                            End If
                                        End If
                                    End If
                                End If


                                If Not IsDBNull(es.Item("sub_sale_price_flag")) Then
                                    If es.Item("sub_sale_price_flag").ToString.ToUpper.Contains("Y") Then
                                        week_spi += 1
                                    End If
                                End If

                                If Not IsDBNull(es.Item("sub_serv_code")) Then
                                    If Not String.IsNullOrEmpty(es.Item("sub_serv_code").ToString.Trim) Then

                                        If es.Item("sub_serv_code").ToString.ToUpper.Contains("CRM") Then
                                            week_crm += 1
                                        End If

                                    End If
                                End If

                            ElseIf es.Item("sub_frequency").ToString.ToUpper.Trim.Contains("MONTHLY") Then

                                monthly_sub_counter += 1

                                If Not IsDBNull(es.Item("sub_business_aircraft_flag")) Then
                                    If es.Item("sub_business_aircraft_flag").ToString.ToUpper.Contains("Y") Then
                                        month_bus += 1
                                    End If
                                End If

                                If Not IsDBNull(es.Item("sub_helicopters_flag")) Then
                                    If es.Item("sub_helicopters_flag").ToString.ToUpper.Contains("Y") Then
                                        month_hel += 1
                                    End If
                                End If

                                If Not IsDBNull(es.Item("sub_commerical_flag")) Then
                                    If es.Item("sub_commerical_flag").ToString.ToUpper.Contains("Y") Then
                                        month_comm += 1
                                    End If
                                End If

                                If Not IsDBNull(es.Item("sub_aerodex_flag")) Then
                                    If es.Item("sub_aerodex_flag").ToString.ToUpper.Contains("Y") Then
                                        month_aero += 1
                                    End If
                                End If

                                If Not IsDBNull(es.Item("sub_yacht_flag")) Then
                                    If es.Item("sub_yacht_flag").ToString.ToUpper.Contains("Y") Then
                                        month_yacht += 1
                                    End If
                                End If

                                If Not IsDBNull(es.Item("sub_business_aircraft_flag")) Then
                                    If es.Item("sub_business_aircraft_flag").ToString.ToUpper.Contains("Y") Then
                                        If Not IsDBNull(es.Item("sub_busair_tier_level")) Then
                                            If Not String.IsNullOrEmpty(es.Item("sub_busair_tier_level").ToString.Trim) Then
                                                If CInt(es.Item("sub_busair_tier_level").ToString) = 1 Then
                                                    month_t1 += 1
                                                ElseIf CInt(es.Item("sub_busair_tier_level").ToString) = 2 Then
                                                    month_t2 += 1
                                                Else
                                                    month_t3 += 1
                                                End If
                                            End If
                                        End If
                                    End If
                                End If


                                If Not IsDBNull(es.Item("sub_sale_price_flag")) Then
                                    If es.Item("sub_sale_price_flag").ToString.ToUpper.Contains("Y") Then
                                        month_spi += 1
                                    End If
                                End If

                                If Not IsDBNull(es.Item("sub_serv_code")) Then
                                    If Not String.IsNullOrEmpty(es.Item("sub_serv_code").ToString.Trim) Then

                                        If es.Item("sub_serv_code").ToString.ToUpper.Contains("CRM") Then
                                            month_crm += 1
                                        End If

                                    End If
                                End If

                            ElseIf es.Item("sub_frequency").ToString.ToUpper.Trim.Contains("BIWEEKLY") Then

                                biweekly_sub_counter += 1

                                If Not IsDBNull(es.Item("sub_business_aircraft_flag")) Then
                                    If es.Item("sub_business_aircraft_flag").ToString.ToUpper.Contains("Y") Then
                                        biweek_bus += 1
                                    End If
                                End If

                                If Not IsDBNull(es.Item("sub_helicopters_flag")) Then
                                    If es.Item("sub_helicopters_flag").ToString.ToUpper.Contains("Y") Then
                                        biweek_hel += 1
                                    End If
                                End If

                                If Not IsDBNull(es.Item("sub_commerical_flag")) Then
                                    If es.Item("sub_commerical_flag").ToString.ToUpper.Contains("Y") Then
                                        biweek_comm += 1
                                    End If
                                End If

                                If Not IsDBNull(es.Item("sub_aerodex_flag")) Then
                                    If es.Item("sub_aerodex_flag").ToString.ToUpper.Contains("Y") Then
                                        biweek_aero += 1
                                    End If
                                End If

                                If Not IsDBNull(es.Item("sub_yacht_flag")) Then
                                    If es.Item("sub_yacht_flag").ToString.ToUpper.Contains("Y") Then
                                        biweek_yacht += 1
                                    End If
                                End If

                                If Not IsDBNull(es.Item("sub_business_aircraft_flag")) Then
                                    If es.Item("sub_business_aircraft_flag").ToString.ToUpper.Contains("Y") Then
                                        If Not IsDBNull(es.Item("sub_busair_tier_level")) Then
                                            If Not String.IsNullOrEmpty(es.Item("sub_busair_tier_level").ToString.Trim) Then
                                                If CInt(es.Item("sub_busair_tier_level").ToString) = 1 Then
                                                    biweek_t1 += 1
                                                ElseIf CInt(es.Item("sub_busair_tier_level").ToString) = 2 Then
                                                    biweek_t2 += 1
                                                Else
                                                    biweek_t3 += 1
                                                End If
                                            End If
                                        End If
                                    End If
                                End If


                                If Not IsDBNull(es.Item("sub_sale_price_flag")) Then
                                    If es.Item("sub_sale_price_flag").ToString.ToUpper.Contains("Y") Then
                                        biweek_spi += 1
                                    End If
                                End If

                                If Not IsDBNull(es.Item("sub_serv_code")) Then
                                    If Not String.IsNullOrEmpty(es.Item("sub_serv_code").ToString.Trim) Then

                                        If es.Item("sub_serv_code").ToString.ToUpper.Contains("CRM") Then
                                            biweek_crm += 1
                                        End If

                                    End If
                                End If

                            End If

                        End If

                        If Not type_to_show.Trim.ToUpper.Contains("ALL") And bShowHostNames Then

                            If Not IsDBNull(es.Item("website_host_name")) Then

                                If es.Item("website_host_name").ToString.ToUpper.Trim.Contains("JETNETEVOLUTION.COM") Then
                                    new_evo_count += 1
                                ElseIf es.Item("website_host_name").ToString.ToUpper.Trim.Contains("TESTJETNETEVOLUTION.COM") Then
                                    new_evo_test_count += 1
                                ElseIf es.Item("website_host_name").ToString.ToUpper.Trim.Contains("YACHT-SPOTONLINE.COM") Then
                                    yacht_spot_count += 1
                                ElseIf es.Item("website_host_name").ToString.ToUpper.Trim.Contains("YACHT-SPOTTEST.COM") Then
                                    yacht_spot_test_count += 1
                                ElseIf es.Item("website_host_name").ToString.ToUpper.Trim.Contains("LOCALHOST") Then
                                    local_count += 1
                                ElseIf es.Item("website_host_name").ToString.ToUpper.Trim.Contains("NEWEVONET") Then
                                    local_count += 1
                                ElseIf es.Item("website_host_name").ToString.ToUpper.Trim.Contains("YACHTSITE") Then
                                    local_count += 1
                                ElseIf es.Item("website_host_name").ToString.ToUpper.Trim.Contains("LOCALGLOBAL") Then
                                    local_count += 1
                                ElseIf es.Item("website_host_name").ToString.ToUpper.Trim.Contains("LOCALHOMEBASE") Then
                                    local_count += 1
                                ElseIf es.Item("website_host_name").ToString.ToUpper.Trim.Contains("LOCALEVOLUTION") Then
                                    local_count += 1
                                ElseIf es.Item("website_host_name").ToString.ToUpper.Trim.Contains("LOCALEVOMOBILE") Then
                                    local_count += 1
                                ElseIf es.Item("website_host_name").ToString.ToUpper.Trim.Contains("EVOADMIN") Then
                                    local_count += 1
                                ElseIf es.Item("website_host_name").ToString.ToUpper.Trim.Contains("EVOLUTIONADMIN.COM") Then
                                    evo_admin_count += 1
                                ElseIf es.Item("website_host_name").ToString.ToUpper.Trim.Contains("EVOLUTIONADMINTEST.COM") Then
                                    evo_admin_test_count += 1
                                ElseIf es.Item("website_host_name").ToString.ToUpper.Trim.Contains("ADMIN.JETNET.COM") Then
                                    jetnet_admin_count += 1
                                ElseIf es.Item("website_host_name").ToString.ToUpper.Trim.Contains("ADMINTEST.JETNET.COM") Then
                                    jetnet_admin_test_count += 1
                                ElseIf es.Item("website_host_name").ToString.ToUpper.Trim.Contains("JETNETEVO.COM") Then
                                    old_evo_count += 1
                                ElseIf es.Item("website_host_name").ToString.ToUpper.Trim.Contains("JETNETEVOMOBILE.COM") Then
                                    evo_mobile_on_count += 1
                                ElseIf es.Item("website_host_name").ToString.ToUpper.Trim.Contains("MOBILE.JETNET.COM") Then
                                    jetnet_mobile_on_count += 1
                                ElseIf es.Item("website_host_name").ToString.ToUpper.Trim.Contains("TESTEVOLUTIONMOBILE.COM") Then
                                    evo_mobile_test_on_count += 1
                                ElseIf es.Item("website_host_name").ToString.ToUpper.Trim.Contains("JETNETTEST.COM") Then
                                    test_count += 1
                                ElseIf es.Item("website_host_name").ToString.ToUpper.Trim.Contains("HOMEBASE.COM") Then
                                    homebase_count += 1
                                ElseIf es.Item("website_host_name").ToString.ToUpper.Trim.Contains("HOMEBASETEST.COM") Then
                                    homebase_count += 1
                                End If

                            End If

                        End If

                    Next

                Else
                    htmlOut.Append("<tr><td colspan=""13"">NO USERS FOUND</td>")
                End If

                atemptable = Nothing

                ' summ total users
                total_users_on = live_sub_counter + weekly_sub_counter + monthly_sub_counter + biweekly_sub_counter

                ' connect back to "CRM" and get users
                master_monthly = 0
                master_weekly = 0
                master_live = 0
                master_biweekly = 0

                strSQL = "SELECT DISTINCT cliuser_login, client_regFrequency FROM client_user"
                strSQL += " INNER JOIN client_register_master ON client_regid = cliuser_client_regid"

                If Trim(complete_summary) = "Y" Then
                    strSQL += "  where client_regStatus = 'Y' and  cliuser_client_regid not in (18,8,14,7,47,21,13,40,76,30,1,2,69) "
                Else
                    strSQL += " WHERE cliuser_last_session_date > '" + Format(DateAdd("n", -10, Now()), "yyyy-MM-dd H:mm:ss").Trim + "'"
                    strSQL += " AND ((cliuser_last_session_date <> cliuser_last_logout_date) OR (cliuser_last_logout_date is NULL))"
                End If



                Try
                    MySqlConn.ConnectionString = crmMasterConnectStr

                    MySqlConn.Open()

                    MySqlCommand.Connection = MySqlConn
                    MySqlCommand.CommandType = System.Data.CommandType.Text
                    MySqlCommand.CommandTimeout = 240

                    MySqlCommand.CommandText = strSQL
                    my1 = MySqlCommand.ExecuteReader()

                    If my1.HasRows Then
                        Do While my1.Read

                            If my1.Item("client_regFrequency").ToString.ToUpper.Trim.Contains("LIVE") Then
                                master_live += 1
                            ElseIf my1.Item("client_regFrequency").ToString.ToUpper.Trim.Contains("WEEKLY") Then
                                master_weekly += 1
                            ElseIf my1.Item("client_regFrequency").ToString.ToUpper.Trim.Contains("MONTHLY") Then
                                master_monthly += 1
                            ElseIf my1.Item("client_regFrequency").ToString.ToUpper.Trim.Contains("BIWEEKLY") Then
                                master_biweekly += 1
                            End If

                            count_of_current_crm_users += 1

                        Loop

                    End If

                    my1.Close()
                    my1 = Nothing

                Catch MySqlException
                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />" + MySqlException.Message
                End Try



                If Trim(growth_summary) = "Y" Then

                    '  -- ****************************************************************************************************************
                    '-- CUSTOMER - CUSTOMER GROWTH MARKETPLACE VS AERODEX LINE CHART


                    temp_query = "   Select cstat_year, cstat_month, cstat_total, cstat_marketplace, cstat_aerodex from Customer_Statistics With (NOLOCK) "
                    temp_query &= " where cstat_year >= 2017 "
                    temp_query &= "order by cstat_year, cstat_month "
                    SqlCommand.CommandText = temp_query
                    SqlReader2 = SqlCommand.ExecuteReader()

                    Try
                        atemptable2.Load(SqlReader2)
                    Catch constrExc As System.Data.ConstraintException
                        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />Error in current_subscriber_summary load datatable " + constrExc.Message
                    End Try

                    If Not IsNothing(atemptable2) Then
                        If atemptable2.Rows.Count > 0 Then
                            For Each es As DataRow In atemptable2.Rows
                                If Trim(growth_graph1) <> "" Then
                                    growth_graph1 &= ", "
                                End If
                                growth_graph1 &= "['" & es.Item("cstat_month") & "-" & es.Item("cstat_year") & "', " & es.Item("cstat_total") & ", " & es.Item("cstat_marketplace") & ", " & es.Item("cstat_aerodex") & "]"
                            Next
                        End If
                    End If




                    SqlReader2.Close()

                    ' -- ****************************************************************************************************************
                    '-- CUSTOMER - VALUES CUSTOMER GROWTH
                    temp_query = "   Select cstat_year, cstat_month, cstat_values from Customer_Statistics With (NOLOCK) "
                    temp_query &= "where cstat_year >= 2017 "
                    temp_query &= "order by cstat_year, cstat_month "

                    SqlCommand.CommandText = temp_query
                    SqlReader2 = SqlCommand.ExecuteReader()

                    Try
                        atemptable3.Load(SqlReader2)
                    Catch constrExc As System.Data.ConstraintException
                        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />Error in current_subscriber_summary load datatable " + constrExc.Message
                    End Try

                    If Not IsNothing(atemptable3) Then
                        If atemptable3.Rows.Count > 0 Then
                            For Each es As DataRow In atemptable3.Rows
                                If Trim(growth_graph2) <> "" Then
                                    growth_graph2 &= ", "
                                End If
                                growth_graph2 &= "['" & es.Item("cstat_month") & "-" & es.Item("cstat_year") & "', " & es.Item("cstat_values") & "]"
                            Next
                        End If
                    End If


                    SqlReader2.Close()

                    SqlReader2 = Nothing

                End If


                ' GET SALESFORCE COUNTS
                Try
                    temp_query = " Select sub_frequency, COUNT(distinct contact_email_address) As TCOUNT "
                    temp_query &= " From View_JETNET_Customers with (NOLOCK) "

                    If Trim(complete_summary) = "Y" Then
                    Else
                        temp_query &= " inner Join API_Activity_Log with (NOLOCK) on sublogin_sub_id = apiact_sub_id  and contact_email_address=apiact_email_address "
                    End If

                    temp_query &= " WHERE sub_serv_code Like '%FS' "

                    If Trim(complete_summary) = "Y" Then
                        temp_query &= " and  (sub_comp_id <> 135887) AND (sublogin_demo_flag = 'N')  "
                    Else
                        temp_query &= " And apiact_request_date >= DateAdd(Minute, -60, GETDATE())  "
                    End If
                    temp_query &= " Group BY sub_frequency "

                    SqlCommand.CommandText = temp_query
                    SqlReader2 = SqlCommand.ExecuteReader()

                    Try
                        atemptable4.Load(SqlReader2)
                    Catch constrExc As System.Data.ConstraintException
                        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />Error in current_subscriber_summary load datatable " + constrExc.Message
                    End Try

                    If Not IsNothing(atemptable4) Then
                        If atemptable4.Rows.Count > 0 Then
                            For Each es As DataRow In atemptable4.Rows

                                If Not IsDBNull(es.Item("sub_frequency")) And Not IsDBNull(es.Item("TCOUNT")) Then
                                    If Trim(es.Item("sub_frequency")) = "Live" Then
                                        salesforce_live = salesforce_live + CLng(es.Item("TCOUNT"))
                                    ElseIf Trim(es.Item("sub_frequency")) = "Monthly" Then
                                        salesforce_monthly = salesforce_monthly + CLng(es.Item("TCOUNT"))
                                    ElseIf Trim(es.Item("sub_frequency")) = "Weekly" Then
                                        salesforce_weekly = salesforce_weekly + CLng(es.Item("TCOUNT"))
                                    End If
                                End If

                            Next


                            salesforce_total = salesforce_live + salesforce_monthly + salesforce_weekly

                        End If
                    End If

                    SqlReader2.Close()
                Catch ex As Exception

                End Try





                htmlOut.Append("<tr><td>LIVE</td>")
                htmlOut.Append("<td Class=""text_align_right""><a Class=""underline pointer"" href=""adminCurrentUsers.aspx?freq=Live"">" + FormatNumber(live_sub_counter, 0).ToString + "</a></td>")
                htmlOut.Append("<td Class=""text_align_right""><a Class=""underline pointer"" href=""adminCurrentUsers.aspx?freq=Live&col=bus"">" + FormatNumber(live_bus, 0).ToString + "</a></td>")
                htmlOut.Append("<td Class=""text_align_right""><a Class=""underline pointer"" href=""adminCurrentUsers.aspx?freq=Live&col=t1"">" + FormatNumber(live_t1, 0).ToString + "</a></td>")
                htmlOut.Append("<td Class=""text_align_right""><a Class=""underline pointer"" href=""adminCurrentUsers.aspx?freq=Live&col=t2"">" + FormatNumber(live_t2, 0).ToString + "</a></td>")
                htmlOut.Append("<td Class=""text_align_right""><a Class=""underline pointer"" href=""adminCurrentUsers.aspx?freq=Live&col=t3"">" + FormatNumber(live_t3, 0).ToString + "</a></td>")
                htmlOut.Append("<td Class=""text_align_right""><a Class=""underline pointer"" href=""adminCurrentUsers.aspx?freq=Live&col=hel"">" + FormatNumber(live_hel, 0).ToString + "</a></td>")
                htmlOut.Append("<td Class=""text_align_right""><a Class=""underline pointer"" href=""adminCurrentUsers.aspx?freq=Live&col=comm"">" + FormatNumber(live_comm, 0).ToString + "</a></td>")
                htmlOut.Append("<td Class=""text_align_right""><a Class=""underline pointer"" href=""adminCurrentUsers.aspx?freq=Live&col=yacht"">" + FormatNumber(live_yacht, 0).ToString + "</a></td>")
                htmlOut.Append("<td Class=""text_align_right""><a Class=""underline pointer"" href=""adminCurrentUsers.aspx?freq=Live&col=aero"">" + FormatNumber(live_aero, 0).ToString + "</a></td>")
                htmlOut.Append("<td Class=""text_align_right""><a Class=""underline pointer"" href=""adminCurrentUsers.aspx?freq=Live&col=spi"">" + FormatNumber(live_spi, 0).ToString + "</a></td>")

                htmlOut.Append("<td Class=""text_align_right""><a Class=""underline pointer"" href=""adminCurrentUsers.aspx?New=Y&freq=live"">" + FormatNumber(master_live, 0).ToString + "</a></td>")
                htmlOut.Append("<td Class=""text_align_right""><a Class=""underline pointer"" href=""adminCurrentUsers.aspx?freq=Live&col=sf"">" + FormatNumber(salesforce_live, 0).ToString + "</a></td>")

                If Trim(complete_summary) = "Y" Then
                Else
                    htmlOut.Append("<td Class=""text_align_right""><a Class=""underline pointer"" href=""adminCurrentUsers.aspx?freq=Live"">" + FormatNumber(live_sub_counter + master_live, 0).ToString + "</td>")
                End If

                If type_to_show.Trim.ToUpper.Contains("ALL") Then
                    htmlOut.Append("<td Class=""text_align_right""><a Class=""underline pointer"" href=""adminCurrentUsers.aspx?freq=Live&col=crm"">" + FormatNumber(live_crm, 0).ToString + "</a></td>")
                End If

                htmlOut.Append("</tr>")

                htmlOut.Append("<tr><td>WEEKLY</td>")
                htmlOut.Append("<td Class=""text_align_right""><a Class=""underline pointer"" href=""adminCurrentUsers.aspx?freq=Weekly"">" + FormatNumber(weekly_sub_counter, 0).ToString + "</a></td>")
                htmlOut.Append("<td Class=""text_align_right""><a Class=""underline pointer"" href=""adminCurrentUsers.aspx?freq=Weekly&col=bus"">" + FormatNumber(week_bus, 0).ToString + "</a></td>")
                htmlOut.Append("<td Class=""text_align_right""><a Class=""underline pointer"" href=""adminCurrentUsers.aspx?freq=Weekly&col=t1"">" + FormatNumber(week_t1, 0).ToString + "</a></td>")
                htmlOut.Append("<td Class=""text_align_right""><a Class=""underline pointer"" href=""adminCurrentUsers.aspx?freq=Weekly&col=t2"">" + FormatNumber(week_t2, 0).ToString + "</a></td>")
                htmlOut.Append("<td Class=""text_align_right""><a Class=""underline pointer"" href=""adminCurrentUsers.aspx?freq=Weekly&col=t3"">" + FormatNumber(week_t3, 0).ToString + "</a></td>")
                htmlOut.Append("<td Class=""text_align_right""><a Class=""underline pointer"" href=""adminCurrentUsers.aspx?freq=Weekly&col=hel"">" + FormatNumber(week_hel, 0).ToString + "</a></td>")
                htmlOut.Append("<td Class=""text_align_right""><a Class=""underline pointer"" href=""adminCurrentUsers.aspx?freq=Weekly&col=comm"">" + FormatNumber(week_comm, 0).ToString + "</a></td>")
                htmlOut.Append("<td Class=""text_align_right""><a Class=""underline pointer"" href=""adminCurrentUsers.aspx?freq=Weekly&col=yacht"">" + FormatNumber(week_yacht, 0).ToString + "</a></td>")
                htmlOut.Append("<td Class=""text_align_right""><a Class=""underline pointer"" href=""adminCurrentUsers.aspx?freq=Weekly&col=aero"">" + FormatNumber(week_aero, 0).ToString + "</a></td>")
                htmlOut.Append("<td Class=""text_align_right""><a Class=""underline pointer"" href=""adminCurrentUsers.aspx?freq=Weekly&col=spi"">" + FormatNumber(week_spi, 0).ToString + "</a></td>")

                htmlOut.Append("<td Class=""text_align_right""><a Class=""underline pointer"" href=""adminCurrentUsers.aspx?New=Y&freq=Weekly"">" + FormatNumber(master_weekly, 0).ToString + "</a></td>")
                htmlOut.Append("<td Class=""text_align_right""><a Class=""underline pointer"" href=""adminCurrentUsers.aspx?freq=Weekly&col=sf"">" + FormatNumber(salesforce_weekly, 0).ToString + "</a></td>")

                If Trim(complete_summary) = "Y" Then
                Else
                    htmlOut.Append("<td Class=""text_align_right""><a Class=""underline pointer"" href=""adminCurrentUsers.aspx?freq=Weekly"">" + FormatNumber(weekly_sub_counter + master_weekly, 0).ToString + "</td>")
                End If

                If type_to_show.Trim.ToUpper.Contains("ALL") Then
                    htmlOut.Append("<td Class=""text_align_right""><a Class=""underline pointer"" href=""adminCurrentUsers.aspx?freq=Weekly&col=crm"">" + FormatNumber(week_crm, 0).ToString + "</a></td>")
                End If

                htmlOut.Append("</tr>")

                htmlOut.Append("<tr><td>BIWEEKLY</td>")
                htmlOut.Append("<td Class=""text_align_right""><a Class=""underline pointer"" href=""adminCurrentUsers.aspx?freq=Biweekly"">" + FormatNumber(biweekly_sub_counter, 0).ToString + "</a></td>")
                htmlOut.Append("<td Class=""text_align_right""><a Class=""underline pointer"" href=""adminCurrentUsers.aspx?freq=Biweekly&col=bus"">" + FormatNumber(biweek_bus, 0).ToString + "</a></td>")
                htmlOut.Append("<td Class=""text_align_right""><a Class=""underline pointer"" href=""adminCurrentUsers.aspx?freq=Biweekly&col=t1"">" + FormatNumber(biweek_t1, 0).ToString + "</a></td>")
                htmlOut.Append("<td Class=""text_align_right""><a Class=""underline pointer"" href=""adminCurrentUsers.aspx?freq=Biweekly&col=t2"">" + FormatNumber(biweek_t2, 0).ToString + "</a></td>")
                htmlOut.Append("<td Class=""text_align_right""><a Class=""underline pointer"" href=""adminCurrentUsers.aspx?freq=Biweekly&col=t3"">" + FormatNumber(biweek_t3, 0).ToString + "</a></td>")
                htmlOut.Append("<td Class=""text_align_right""><a Class=""underline pointer"" href=""adminCurrentUsers.aspx?freq=Biweekly&col=hel"">" + FormatNumber(biweek_hel, 0).ToString + "</a></td>")
                htmlOut.Append("<td Class=""text_align_right""><a Class=""underline pointer"" href=""adminCurrentUsers.aspx?freq=Biweekly&col=comm"">" + FormatNumber(biweek_comm, 0).ToString + "</a></td>")
                htmlOut.Append("<td Class=""text_align_right""><a Class=""underline pointer"" href=""adminCurrentUsers.aspx?freq=Biweekly&col=yacht"">" + FormatNumber(biweek_yacht, 0).ToString + "</a></td>")
                htmlOut.Append("<td Class=""text_align_right""><a Class=""underline pointer"" href=""adminCurrentUsers.aspx?freq=Biweekly&col=aero"">" + FormatNumber(biweek_aero, 0).ToString + "</a></td>")
                htmlOut.Append("<td Class=""text_align_right""><a Class=""underline pointer"" href=""adminCurrentUsers.aspx?freq=Biweekly&col=spi"">" + FormatNumber(biweek_spi, 0).ToString + "</a></td>")

                htmlOut.Append("<td Class=""text_align_right""><a Class=""underline pointer"" href=""adminCurrentUsers.aspx?New=Y&freq=Biweekly"">" + FormatNumber(master_biweekly, 0).ToString + "</a></td>")
                htmlOut.Append("<td Class=""text_align_right""><a Class=""underline pointer"" href=""adminCurrentUsers.aspx?freq=Biweekly&col=sf"">0</a></td>")

                If Trim(complete_summary) = "Y" Then
                Else
                    htmlOut.Append("<td Class=""text_align_right""><a Class=""underline pointer"" href=""adminCurrentUsers.aspx?freq=Biweekly"">" + FormatNumber(biweekly_sub_counter + master_biweekly, 0).ToString + "</td>")
                End If

                If type_to_show.Trim.ToUpper.Contains("ALL") Then
                    htmlOut.Append("<td Class=""text_align_right""><a Class=""underline pointer"" href=""adminCurrentUsers.aspx?freq=Biweekly&col=crm"">" + FormatNumber(biweek_crm, 0).ToString + "</a></td>")
                End If

                htmlOut.Append("</tr>")

                htmlOut.Append("<tr><td>MONTHLY</td>")
                htmlOut.Append("<td Class=""text_align_right""><a Class=""underline pointer"" href=""adminCurrentUsers.aspx?freq=Monthly"">" + FormatNumber(monthly_sub_counter, 0).ToString + "</a></td>")
                htmlOut.Append("<td Class=""text_align_right""><a Class=""underline pointer"" href=""adminCurrentUsers.aspx?freq=Monthly&col=bus"">" + FormatNumber(month_bus, 0).ToString + "</a></td>")
                htmlOut.Append("<td Class=""text_align_right""><a Class=""underline pointer"" href=""adminCurrentUsers.aspx?freq=Monthly&col=t1"">" + FormatNumber(month_t1, 0).ToString + "</a></td>")
                htmlOut.Append("<td Class=""text_align_right""><a Class=""underline pointer"" href=""adminCurrentUsers.aspx?freq=Monthly&col=t2"">" + FormatNumber(month_t2, 0).ToString + "</a></td>")
                htmlOut.Append("<td Class=""text_align_right""><a Class=""underline pointer"" href=""adminCurrentUsers.aspx?freq=Monthly&col=t3"">" + FormatNumber(month_t3, 0).ToString + "</a></td>")
                htmlOut.Append("<td Class=""text_align_right""><a Class=""underline pointer"" href=""adminCurrentUsers.aspx?freq=Monthly&col=hel"">" + FormatNumber(month_hel, 0).ToString + "</a></td>")
                htmlOut.Append("<td Class=""text_align_right""><a Class=""underline pointer"" href=""adminCurrentUsers.aspx?freq=Monthly&col=comm"">" + FormatNumber(month_comm, 0).ToString + "</a></td>")
                htmlOut.Append("<td Class=""text_align_right""><a Class=""underline pointer"" href=""adminCurrentUsers.aspx?freq=Monthly&col=yacht"">" + FormatNumber(month_yacht, 0).ToString + "</a></td>")
                htmlOut.Append("<td Class=""text_align_right""><a Class=""underline pointer"" href=""adminCurrentUsers.aspx?freq=Monthly&col=aero"">" + FormatNumber(month_aero, 0).ToString + "</a></td>")
                htmlOut.Append("<td Class=""text_align_right""><a Class=""underline pointer"" href=""adminCurrentUsers.aspx?freq=Monthly&col=spi"">" + FormatNumber(month_spi, 0).ToString + "</a></td>")

                htmlOut.Append("<td Class=""text_align_right""><a Class=""underline pointer"" href=""adminCurrentUsers.aspx?New=Y&freq=Monthly"">" + FormatNumber(master_monthly, 0).ToString + "</a></td>")
                htmlOut.Append("<td Class=""text_align_right""><a Class=""underline pointer"" href=""adminCurrentUsers.aspx?freq=Monthly&col=sf"">" + FormatNumber(salesforce_monthly, 0).ToString + "</a></td>")

                If Trim(complete_summary) = "Y" Then
                Else
                    htmlOut.Append("<td Class=""text_align_right""><a Class=""underline pointer"" href=""adminCurrentUsers.aspx?freq=Monthly"">" + FormatNumber(monthly_sub_counter + master_monthly, 0).ToString + "</td>")
                End If

                If type_to_show.Trim.ToUpper.Contains("ALL") Then
                    htmlOut.Append("<td Class=""text_align_right""><a Class=""underline pointer"" href=""adminCurrentUsers.aspx?freq=Monthly&col=crm"">" + FormatNumber(month_crm, 0).ToString + "</a></td>")
                End If

                htmlOut.Append("</tr>")
                AerodexCount = FormatNumber(live_aero + week_aero + month_aero + biweek_aero, 0)
                TotalBusinessCounter = FormatNumber(live_bus + week_bus + month_bus + biweek_bus, 0)
                TotalHeliCounter = FormatNumber(live_hel + week_hel + month_hel + biweek_hel, 0)
                TotalCommCounter = FormatNumber(live_comm + week_comm + month_comm + biweek_comm, 0)
                TotalYachtCounter = FormatNumber(live_yacht + week_yacht + month_yacht + biweek_yacht, 0)
                htmlOut.Append("<tr><td Class=""text_align_right""><strong>TOTALS</strong></td><td align=""right"">" + FormatNumber(total_users_on, 0).ToString + "</a></td>")
                htmlOut.Append("<td Class=""text_align_right"">" + FormatNumber(live_bus + week_bus + month_bus + biweek_bus, 0).ToString + "</a></td>")
                htmlOut.Append("<td Class=""text_align_right"">" + FormatNumber(live_t1 + week_t1 + month_t1 + biweek_t1, 0).ToString + "</a></td>")
                htmlOut.Append("<td Class=""text_align_right"">" + FormatNumber(live_t2 + week_t2 + month_t2 + biweek_t2, 0).ToString + "</a></td>")
                htmlOut.Append("<td Class=""text_align_right"">" + FormatNumber(live_t3 + week_t3 + month_t3 + biweek_t3, 0).ToString + "</a></td>")
                htmlOut.Append("<td Class=""text_align_right"">" + FormatNumber(live_hel + week_hel + month_hel + biweek_hel, 0).ToString + "</a></td>")
                htmlOut.Append("<td Class=""text_align_right"">" + FormatNumber(live_comm + week_comm + month_comm + biweek_comm, 0).ToString + "</a></td>")
                htmlOut.Append("<td Class=""text_align_right"">" + FormatNumber(live_yacht + week_yacht + month_yacht + biweek_yacht, 0).ToString + "</a></td>")
                htmlOut.Append("<td Class=""text_align_right"">" + FormatNumber(live_aero + week_aero + month_aero + biweek_aero, 0).ToString + "</a></td>")
                htmlOut.Append("<td Class=""text_align_right"">" + FormatNumber(live_spi + week_spi + month_spi + biweek_spi, 0).ToString + "</a></td>")
                htmlOut.Append("<td Class=""text_align_right""><a Class=""underline pointer"" href=""adminCurrentUsers.aspx?New=Y"">" + FormatNumber(master_monthly + master_weekly + master_live + master_biweekly, 0).ToString + "</a></td>")

                'added MSW - 11/29/19
                htmlOut.Append("<td Class=""text_align_right""><a href='/adminSummary.aspx?rid=154' target='_blank'>" + FormatNumber(salesforce_total, 0).ToString + "</a></td>")


                If Trim(complete_summary) = "Y" Then
                Else
                    htmlOut.Append("<td Class=""text_align_right""><a Class=""underline pointer"" href=""adminCurrentUsers.aspx?freq=Live"">" + FormatNumber(live_sub_counter + weekly_sub_counter + biweekly_sub_counter + monthly_sub_counter + count_of_current_crm_users, 0).ToString + "</a></td>")
                End If

                If type_to_show.Trim.ToUpper.Contains("ALL") Then
                    htmlOut.Append("<td Class=""text_align_right"">" + FormatNumber(live_crm + week_crm + month_crm + biweek_crm, 0).ToString + "</a></td>")
                End If

                htmlOut.Append("</tr></tbody>")
                htmlOut.Append("</table></div></div>")




                If Trim(growth_summary) = "Y" Then
                    htmlOut.Append("YYYYY")
                Else
                End If

                counter1_market = 0
                counter1_aero = 0

                If type_to_show.Trim.ToUpper.Contains("ALL") Then

                    tmpHtmlOut.Append("<table><tr><td>")

                    ' FOR EVOLUTION MARKETPLACE -------------------------
                    sQuery = New StringBuilder
                    sQuery.Append("Select cbus_name, cbus_type, count(distinct sub_id) As tcount")
                    sQuery.Append(" FROM subscription")
                    sQuery.Append(" INNER JOIN company With(NOLOCK) On sub_comp_id = comp_id And comp_journ_id = 0")
                    sQuery.Append(" INNER JOIN company_business_type With(NOLOCK) On comp_business_type = cbus_type")
                    sQuery.Append(" INNER JOIN Service_Frequency With(NOLOCK) On sub_frequency = serfreq_frequency")
                    sQuery.Append(" WHERE sub_start_date <= '" + Now().ToShortDateString + "' AND (sub_end_date is NULL OR sub_end_date >= '" + Now().ToShortDateString + "')")
                    sQuery.Append(" AND sub_aerodex_flag='N' AND sub_comp_id <> 135887")

                    If Not String.IsNullOrEmpty(productCode.Trim) Then

                        sQuery.Append(" AND (")

                        If productCode.ToUpper.Trim.Contains("B") Then

                            sQuery.Append("sub_business_aircraft_flag = 'Y'")

                            sSeperator = Constants.cOrClause

                        End If

                        If productCode.ToUpper.Trim.Contains("H") Then

                            sQuery.Append(sSeperator + "sub_helicopters_flag = 'Y'")

                            sSeperator = Constants.cOrClause

                        End If

                        If productCode.ToUpper.Trim.Contains("C") Then

                            sQuery.Append(sSeperator + "sub_commerical_flag = 'Y'")

                            sSeperator = Constants.cOrClause

                        End If

                        If productCode.ToUpper.Trim.Contains("Y") Then

                            sQuery.Append(sSeperator + "sub_yacht_flag = 'Y'")

                        End If

                        sQuery.Append(")")

                    End If

                    sQuery.Append(" GROUP BY cbus_name ")
                    sQuery.Append(" ORDER BY cbus_name ")

                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>current_subscriber_summary(ByVal productCode As String, ByVal type_to_show As String, ByVal companyID As Long, ByVal errors As Boolean, ByVal error_count As Integer) As String</b><br />" + sQuery.ToString

                    SqlCommand.CommandText = sQuery.ToString
                    SqlReader = SqlCommand.ExecuteReader()

                    ' clear previous query results
                    atemptable = New DataTable

                    Try
                        atemptable.Load(SqlReader)
                    Catch constrExc As System.Data.ConstraintException
                        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                        aError = "Error in current_subscriber_summary load datatable " + constrExc.Message
                    End Try

                    SqlReader.Close()
                    SqlReader = Nothing

                    If atemptable.Rows.Count > 0 Then

                        Dim tmpCount As Integer = 0

                        tmpHtmlOut.Append("<div class=""Box"">Subscribers by Business Type<br/><b>Evolution Marketplace <br/>" + text_for_type.Trim + "Subscribers</b><br/>")
                        tmpHtmlOut.Append("<table border=""0"" class=""formatTable blue"">")
                        tmpHtmlOut.Append("<thead><tr><th>COMPANY TYPE</th><th>SUBSCRIBERS</th></tr></thead><tbody>")
                        For Each es As DataRow In atemptable.Rows

                            If Not IsDBNull(es.Item("tcount")) Then
                                If Not String.IsNullOrEmpty(es.Item("tcount").ToString.Trim) Then
                                    counter1_market += CInt(es.Item("tcount").ToString)
                                    tmpCount = CInt(es.Item("tcount").ToString)
                                End If
                            End If

                            htmlOut.Append("<tr><td align=""left"">" + es.Item("cbus_name").ToString.Trim + "</td><td align=""right"">" + FormatNumber(tmpCount, 0).ToString + "</td></tr>")

                        Next

                        tmpHtmlOut.Append("<tr><td  class=""text_align_right""><strong>TOTAL</strong></td><td class=""text_align_right"">" + FormatNumber(counter1_market, 0).ToString + "</td></tr>")
                        tmpHtmlOut.Append("</tbody></table></div>")

                    Else
                        tmpHtmlOut.Append("NO Evolution Marketplace " + text_for_type.Trim + "Subscribers FOUND")
                    End If

                    atemptable = Nothing

                    tmpHtmlOut.Append("</td><td>")

                    ' aerodex subscribers
                    sQuery = New StringBuilder
                    sQuery.Append("SELECT cbus_name, cbus_type, count(distinct sub_id) AS tcount")
                    sQuery.Append(" FROM subscription")
                    sQuery.Append(" INNER JOIN company WITH(NOLOCK) ON sub_comp_id = comp_id AND comp_journ_id = 0")
                    sQuery.Append(" INNER JOIN company_business_type WITH(NOLOCK) ON comp_business_type = cbus_type")
                    sQuery.Append(" INNER JOIN Service_Frequency WITH(NOLOCK) ON sub_frequency = serfreq_frequency")
                    sQuery.Append(" WHERE sub_start_date <= '" + Now().ToShortDateString + "' AND (sub_end_date is NULL OR sub_end_date >= '" + Now().ToShortDateString + "')")
                    sQuery.Append(" AND sub_aerodex_flag='Y' AND sub_comp_id <> 135887")

                    If Not String.IsNullOrEmpty(productCode.Trim) Then

                        sQuery.Append(" AND (")

                        If productCode.ToUpper.Trim.Contains("B") Then

                            sQuery.Append("sub_business_aircraft_flag = 'Y'")

                            sSeperator = Constants.cOrClause

                        End If

                        If productCode.ToUpper.Trim.Contains("H") Then

                            sQuery.Append(sSeperator + "sub_helicopters_flag = 'Y'")

                            sSeperator = Constants.cOrClause

                        End If

                        If productCode.ToUpper.Trim.Contains("C") Then

                            sQuery.Append(sSeperator + "sub_commerical_flag = 'Y'")

                            sSeperator = Constants.cOrClause

                        End If

                        If productCode.ToUpper.Trim.Contains("Y") Then

                            sQuery.Append(sSeperator + "sub_yacht_flag = 'Y'")

                        End If

                        sQuery.Append(")")

                    End If

                    sQuery.Append(" GROUP BY cbus_name ")
                    sQuery.Append(" ORDER BY cbus_name ")

                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>current_subscriber_summary(ByVal productCode As String, ByVal type_to_show As String, ByVal companyID As Long, ByVal errors As Boolean, ByVal error_count As Integer) As String</b><br />" + sQuery.ToString

                    SqlCommand.CommandText = sQuery.ToString
                    SqlReader = SqlCommand.ExecuteReader()

                    ' clear previous query results
                    atemptable = New DataTable

                    Try
                        atemptable.Load(SqlReader)
                    Catch constrExc As System.Data.ConstraintException
                        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                        aError = "Error in current_subscriber_summary load datatable " + constrExc.Message
                    End Try

                    SqlReader.Close()
                    SqlReader = Nothing

                    If atemptable.Rows.Count > 0 Then

                        Dim tmpCount As Integer = 0

                        tmpHtmlOut.Append("<div class=""Box"">Subscribers by Business Type<br /><b>Evolution Aerodex<br />" + text_for_type.Trim + "Subscribers</b>")
                        tmpHtmlOut.Append("<br />")
                        tmpHtmlOut.Append("<table border=""0"" class=""formatTable blue"">")
                        tmpHtmlOut.Append("<tr><td bgcolor=""#D9D8D8"">COMPANY TYPE</td><td bgcolor=""#D9D8D8"">SUBSCRIBERS</td></tr>")

                        For Each es As DataRow In atemptable.Rows

                            If Not IsDBNull(es.Item("tcount")) Then
                                If Not String.IsNullOrEmpty(es.Item("tcount").ToString.Trim) Then
                                    counter1_aero += CInt(es.Item("tcount").ToString)
                                    tmpCount = CInt(es.Item("tcount").ToString)
                                End If
                            End If

                            htmlOut.Append("<tr><td align=""left"">" + es.Item("cbus_name").ToString.Trim + "</td><td align=""right"">" + FormatNumber(tmpCount, 0).ToString + "</td></tr>")

                        Next

                        tmpHtmlOut.Append("<tr><td  class=""text_align_right""><strong>TOTALS</strong></td><td align=""right"">" + FormatNumber(counter1_aero, 0).ToString + "</td></tr>")
                        tmpHtmlOut.Append("</table></div>")

                    Else
                        tmpHtmlOut.Append("NO Evolution Aerodex<br />" + text_for_type.Trim + "Subscribers FOUND")
                    End If

                    atemptable = Nothing

                    tmpHtmlOut.Append("</td></tr></table>")

                End If
                ' platform and browser selection starting 

                If String.IsNullOrEmpty(type_to_show.Trim) Then


                    If Trim(growth_summary) = "Y" Then
                        tmpHtmlOut.Append("<div class=""row""><div class=""six columns""><div class=""Box""><div class=""subHeader"">CUSTOMER GROWTH - MARKETPLACE VS AERODEX</div><div id=""piechartMarket""></div></div></div>")
                        tmpHtmlOut.Append("<div class=""six columns""><div class=""Box""><div class=""subHeader"">CUSTOMER GROWTH - VALUES</div><div id=""frequencychartMarket""></div></div></div>")
                        tmpHtmlOut.Append("</div>ZZZZ")
                    Else
                        tmpHtmlOut.Append("<div class=""row""><div class=""four columns""><div class=""Box""><div class=""subHeader"">MARKETPLACE VS AERODEX</div><div id=""piechartMarket""></div></div></div>")
                        tmpHtmlOut.Append("<div class=""four columns""><div class=""Box""><div class=""subHeader"">FREQUENCY</div><div id=""frequencychartMarket""></div></div></div>")
                        tmpHtmlOut.Append("<div class=""four columns""><div class=""Box""><div class=""subHeader"">AIRCRAFT TYPES</div><div id=""acTypechartMarket""></div></div></div>")
                        tmpHtmlOut.Append("</div>")
                    End If





                    If Trim(growth_summary) = "Y" Then
                    Else

                    End If



                    tmpHtmlOut.Append("<div class=""row""><div class=""six columns""><div class=""Box"">") ' first row (browser)

                    tmpHtmlOut.Append("<div class=""subHeader"">Current User Browser</div>")

                    tmpHtmlOut.Append("<table width=""100%"" border=""0"" cellspacing=""2"" cellpadding=""2"" class=""formatTable blue""><thead><tr>")
                    tmpHtmlOut.Append("<th>BROWSER</th>")
                    tmpHtmlOut.Append("<th class=""text_align_right"">USERS</th>")
                    tmpHtmlOut.Append("<th class=""text_align_right"">PERCENTAGE</th></tr></thead><tbody>")

                    sQuery = New StringBuilder


                    '                    Select Case DISTINCT replace(ltrim(substring(subins_platform_os,7,25)),'n ','') AS browser, count(*) AS tcount 
                    'From view_jetnet_Customers
                    'Where (sub_comp_id <> 135887) And (sublogin_demo_flag = 'N') 
                    'Group BY replace(ltrim(substring(subins_platform_os,7,25)),'n ','') ORDER BY browser

                    sQuery.Append("SELECT DISTINCT replace(ltrim(substring(subins_platform_os,7,25)),'n ','') AS browser, count(*) AS tcount ")
                    sQuery.Append(" from view_jetnet_Customers with (NOLOCK) ")
                    'sQuery.Append(" FROM Subscription WITH(NOLOCK)")
                    'sQuery.Append(" INNER JOIN Subscription_Login WITH(NOLOCK) ON (sublogin_sub_id = sub_id)")
                    'sQuery.Append(" INNER JOIN Subscription_Install WITH(NOLOCK) ON (subins_sub_id = sub_id) AND sublogin_login = subins_login")
                    'sQuery.Append(" INNER JOIN Company WITH(NOLOCK) ON (comp_id = sub_comp_id) AND comp_journ_id = 0")
                    'sQuery.Append(" INNER JOIN Contact WITH(NOLOCK) ON (contact_comp_id = sub_comp_id) AND subins_contact_id = contact_id AND contact_journ_id = 0")
                    If Trim(complete_summary) = "Y" Then
                        sQuery.Append(" where (sub_comp_id <> 135887) AND (sublogin_demo_flag = 'N')   ")
                    Else
                        sQuery.Append(" WHERE subins_last_session_date >= '" + DateAdd("n", -10, Now()).ToString.Trim + "'")
                        sQuery.Append(" AND (subins_last_session_date <> subins_last_logout_date OR subins_last_logout_date IS NULL)")
                    End If

                    sQuery.Append(" GROUP BY replace(ltrim(substring(subins_platform_os,7,25)),'n ','') ORDER BY browser ")

                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>current_subscriber_summary(ByVal productCode As String, ByVal type_to_show As String, ByVal companyID As Long, ByVal errors As Boolean, ByVal error_count As Integer) As String</b><br />" + sQuery.ToString

                    SqlCommand.CommandText = sQuery.ToString
                    SqlReader = SqlCommand.ExecuteReader()

                    ' clear previous query results
                    atemptable = New DataTable

                    Try
                        atemptable.Load(SqlReader)
                    Catch constrExc As System.Data.ConstraintException
                        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                        aError = "Error in current_subscriber_summary load datatable " + constrExc.Message
                    End Try

                    SqlReader.Close()
                    SqlReader = Nothing

                    If atemptable.Rows.Count > 0 Then

                        Dim tCount As Integer = 0

                        For Each es As DataRow In atemptable.Rows

                            If Not IsDBNull(es.Item("browser")) Then
                                If Not String.IsNullOrEmpty(es.Item("browser").ToString.Trim) Then

                                    tCount = 0

                                    If es.Item("browser").ToString.Trim.ToUpper.Contains("MSIE") Then
                                        tmpHtmlOut.Append("<tr><td>INTERNET EXPLORER(" + es.Item("browser").ToString.ToUpper.Trim + ")</td>")
                                    Else
                                        tmpHtmlOut.Append("<tr><td>" + es.Item("browser").ToString.ToUpper.Trim + "</td>")
                                    End If

                                    If Not IsDBNull(es.Item("tcount")) Then
                                        If Not String.IsNullOrEmpty(es.Item("tcount").ToString.Trim) Then
                                            If CInt(es.Item("tcount").ToString.Trim) > 0 Then
                                                tCount = CInt(es.Item("tcount").ToString.Trim)
                                            End If
                                        End If
                                    End If

                                    tmpHtmlOut.Append("<td class=""text_align_right""><a class=""underline pointer"" href=""adminCurrentUsers.aspx?browser=Y&type=B&info=" + es.Item("browser").ToString.ToUpper.Trim + """>" + tCount.ToString + "</a></td>")
                                    tmpHtmlOut.Append("<td class=""text_align_right"">" + CInt((tCount / total_users_on) * 100).ToString + "%</td>")

                                    total_counted_browser_percent += CInt((tCount / total_users_on) * 100)

                                    tmpHtmlOut.Append("</tr>")

                                    total_counted_browser += CInt(es.Item("tcount").ToString)

                                Else

                                    tmpHtmlOut.Append("<tr><td>BLANKS</td>")

                                    If Not IsDBNull(es.Item("tcount")) Then
                                        If Not String.IsNullOrEmpty(es.Item("tcount").ToString.Trim) Then
                                            If CInt(es.Item("tcount").ToString.Trim) > 0 Then
                                                tCount = CInt(es.Item("tcount").ToString.Trim)
                                            End If
                                        End If
                                    End If

                                    tmpHtmlOut.Append("<td class=""text_align_right""><a class=""underline pointer"" href=""adminCurrentUsers.aspx?browser=Y&type=B&info="">" + tCount.ToString + "</a></td>")
                                    tmpHtmlOut.Append("<td class=""text_align_right"">" + CInt((tCount / total_users_on) * 100).ToString + "%</td>")

                                    total_counted_browser_percent += CInt((tCount / total_users_on) * 100)

                                    tmpHtmlOut.Append("</tr>")
                                    total_counted_browser += CInt(es.Item("tcount").ToString)

                                End If
                            End If

                        Next

                        tmpHtmlOut.Append("</tr>")
                        tmpHtmlOut.Append("<tr><td class=""text_align_right""><strong>TOTALS</strong></td>")
                        tmpHtmlOut.Append("<td class=""text_align_right"">" + total_users_on.ToString + "</td>")
                        tmpHtmlOut.Append("<td class=""text_align_right"">" + total_counted_browser_percent.ToString + "%</td>")
                        tmpHtmlOut.Append("</td></tr></tbody></table>")

                    Else
                        tmpHtmlOut.Append("NO BROWSER INFO FOUND")
                    End If

                    tmpHtmlOut.Append("</div></div><div class=""six columns""><div class=""Box"">") ' second row (platform)

                    tmpHtmlOut.Append("<div class=""subHeader"">Current User Platform</div>")

                    tmpHtmlOut.Append("<table width=""100%"" cellspacing=""2"" cellpadding=""2""  class=""formatTable blue""><tr>")
                    tmpHtmlOut.Append("<th>PLATFORM</th>")
                    tmpHtmlOut.Append("<th class=""text_align_right"">USERS</th>")
                    tmpHtmlOut.Append("<th class=""text_align_right"">PERCENTAGE</th></tr></thead><tbody>")

                    sQuery = New StringBuilder
                    sQuery.Append("SELECT DISTINCT substring(subins_platform_os,1,7) AS platform, count(*) AS tcount")
                    sQuery.Append(" from view_jetnet_Customers with (NOLOCK)")
                    'sQuery.Append(" FROM Subscription WITH(NOLOCK)")
                    'sQuery.Append(" INNER JOIN Subscription_Login WITH(NOLOCK) ON (sublogin_sub_id = sub_id)")
                    'sQuery.Append(" INNER JOIN Subscription_Install WITH(NOLOCK) ON (subins_sub_id = sub_id) AND sublogin_login = subins_login")
                    'sQuery.Append(" INNER JOIN Company WITH(NOLOCK) ON (comp_id = sub_comp_id) AND comp_journ_id = 0")
                    'sQuery.Append(" INNER JOIN Contact WITH(NOLOCK) ON (contact_comp_id = sub_comp_id) AND subins_contact_id = contact_id AND contact_journ_id = 0")

                    If Trim(complete_summary) = "Y" Then
                        sQuery.Append(" where  (sub_comp_id <> 135887) AND (sublogin_demo_flag = 'N')  ")
                    Else
                        sQuery.Append(" WHERE subins_last_session_date >= '" + DateAdd("n", -10, Now()).ToString.Trim + "'")
                        sQuery.Append(" AND (subins_last_session_date <> subins_last_logout_date OR subins_last_logout_date IS NULL)")
                    End If


                    sQuery.Append(" GROUP BY substring(subins_platform_os,1,7)")
                    sQuery.Append(" ORDER BY platform")

                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>current_subscriber_summary(ByVal productCode As String, ByVal type_to_show As String, ByVal companyID As Long, ByVal errors As Boolean, ByVal error_count As Integer) As String</b><br />" + sQuery.ToString

                    SqlCommand.CommandText = sQuery.ToString
                    SqlReader = SqlCommand.ExecuteReader()

                    ' clear previous query results
                    atemptable = New DataTable

                    Try
                        atemptable.Load(SqlReader)
                    Catch constrExc As System.Data.ConstraintException
                        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                        aError = "Error in current_subscriber_summary load datatable " + constrExc.Message
                    End Try

                    SqlReader.Close()
                    SqlReader = Nothing

                    If atemptable.Rows.Count > 0 Then

                        Dim tCount As Integer = 0

                        For Each es As DataRow In atemptable.Rows

                            If Not IsDBNull(es.Item("platform")) Then
                                If Not String.IsNullOrEmpty(es.Item("platform").ToString.Trim) Then

                                    tCount = 0

                                    If es.Item("platform").ToString.Trim.ToUpper.Contains("WIN") Then
                                        tmpHtmlOut.Append("<tr><td>WINDOWS (" + es.Item("platform").ToString.ToUpper.Trim + ")</td>")
                                    Else
                                        tmpHtmlOut.Append("<tr><td>" + es.Item("platform").ToString.ToUpper.Trim + "</td>")
                                    End If

                                    If Not IsDBNull(es.Item("tcount")) Then
                                        If Not String.IsNullOrEmpty(es.Item("tcount").ToString.Trim) Then
                                            If CInt(es.Item("tcount").ToString.Trim) > 0 Then
                                                tCount = CInt(es.Item("tcount").ToString.Trim)
                                            End If
                                        End If
                                    End If

                                    tmpHtmlOut.Append("<td class=""text_align_right""><a class=""underline pointer"" href=""adminCurrentUsers.aspx?browser=Y&type=P&info=" + es.Item("platform").ToString.ToUpper.Trim + """>" + tCount.ToString + "</a></td>")
                                    tmpHtmlOut.Append("<td class=""text_align_right"">" + CInt((tCount / total_users_on) * 100).ToString + "%</td>")

                                    total_counted_platform_percent += CInt((tCount / total_users_on) * 100)

                                    tmpHtmlOut.Append("</tr>")

                                    total_counted_platform += CInt(es.Item("tcount").ToString)

                                Else

                                    tmpHtmlOut.Append("<tr><td>BLANKS</td>")

                                    If Not IsDBNull(es.Item("tcount")) Then
                                        If Not String.IsNullOrEmpty(es.Item("tcount").ToString.Trim) Then
                                            If CInt(es.Item("tcount").ToString.Trim) > 0 Then
                                                tCount = CInt(es.Item("tcount").ToString.Trim)
                                            End If
                                        End If
                                    End If

                                    tmpHtmlOut.Append("<td class=""text_align_right""><a class=""underline pointer"" href=""adminCurrentUsers.aspx?browser=Y&type=P&info="">" + tCount.ToString + "</a></td>")
                                    tmpHtmlOut.Append("<td class=""text_align_right"">" + CInt((tCount / total_users_on) * 100).ToString + "%</td>")

                                    total_counted_platform_percent += CInt((tCount / total_users_on) * 100)

                                    tmpHtmlOut.Append("</tr>")
                                    total_counted_platform += CInt(es.Item("tcount").ToString)

                                End If
                            End If

                        Next

                    Else
                        tmpHtmlOut.Append("NO PLATFORM INFO FOUND")
                    End If


                    tmpHtmlOut.Append("</tr>")
                    tmpHtmlOut.Append("<tr><td class=""text_align_right""><strong>TOTALS</strong></td>")
                    tmpHtmlOut.Append("<td class=""text_align_right"">" + total_users_on.ToString + "</td>")
                    tmpHtmlOut.Append("<td class=""text_align_right"">" + total_counted_platform_percent.ToString + "%</td>")
                    tmpHtmlOut.Append("</td></tr></tbody></table></div></div>")
                    tmpHtmlOut.Append("</div>")



                    tmpHtmlOut.Append("<div class=""row""><div class=""six columns""><div class=""Box""><table width=""50%"" border=""0"" cellspacing=""2"" cellpadding=""2"" class=""formatTable blue""><thead><tr>")
                    tmpHtmlOut.Append("<th>CLOUD NOTES USERS</th>")
                    tmpHtmlOut.Append("<th>CLOUD NOTES+ USERS</th>")
                    tmpHtmlOut.Append("</tr></thead><tbody><tr>")
                    tmpHtmlOut.Append("<td>" + notes_plus_count.ToString + "</td>")
                    tmpHtmlOut.Append("<td>" + server_notes_count.ToString + "</td>")
                    tmpHtmlOut.Append("</td></tr></table></div></div>")

                End If

                sQuery = New StringBuilder

                sQuery.Append(" SELECT COUNT(DISTINCT subislog_id) AS tcount")
                sQuery.Append(" FROM Subscription_Install_Log")
                sQuery.Append(" INNER JOIN Subscription WITH(NOLOCK) on sub_id = subislog_subid")
                sQuery.Append(" INNER JOIN Subscription_Login WITH(NOLOCK) ON (sublogin_sub_id = sub_id)")
                sQuery.Append(" INNER JOIN Subscription_Install WITH(NOLOCK) ON (subins_sub_id = sub_id) and sublogin_login = subins_login")
                sQuery.Append(" LEFT OUTER JOIN Company WITH(NOLOCK) ON subislog_comp_id = comp_id and comp_journ_id = 0")
                sQuery.Append(" LEFT OUTER JOIN Contact WITH(NOLOCK) ON subislog_contact_id = contact_id and contact_journ_id = 0")
                sQuery.Append(" LEFT OUTER JOIN aircraft_model WITH(NOLOCK) ON subislog_amod_id = amod_id")
                sQuery.Append(" LEFT OUTER JOIN aircraft WITH(NOLOCK) ON subislog_ac_id = ac_id and ac_journ_id = 0")
                sQuery.Append(" LEFT OUTER JOIN Evolution_Views WITH(NOLOCK) ON subislog_view_id = evoview_id and evoview_id > 0")
                sQuery.Append(" WHERE subislog_date >= '" + Now().ToShortDateString + "'")
                sQuery.Append(" AND subislog_msg_type = 'UserError'")

                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>current_subscriber_summary(ByVal productCode As String, ByVal type_to_show As String, ByVal companyID As Long, ByVal errors As Boolean, ByVal error_count As Integer) As String</b><br />" + sQuery.ToString

                tmpHtmlOut.Append("<div class=""six columns""><div class=""Box""><div class=""subHeader"">ERRORS REPORTED TODAY: <strong>")


                SqlCommand.CommandText = sQuery.ToString
                SqlReader = SqlCommand.ExecuteReader()

                ' clear previous query results
                atemptable = New DataTable

                Try
                    atemptable.Load(SqlReader)
                Catch constrExc As System.Data.ConstraintException
                    Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                    aError = "Error in current_subscriber_summary load datatable " + constrExc.Message
                End Try

                SqlReader.Close()
                SqlReader = Nothing

                If atemptable.Rows.Count > 0 Then

                    For Each es As DataRow In atemptable.Rows

                        If Not IsDBNull(es.Item("tcount")) Then
                            If Not String.IsNullOrEmpty(es.Item("tcount").ToString.Trim) Then
                                If CInt(es.Item("tcount").ToString.Trim) > 0 Then
                                    tmpHtmlOut.Append("<a class=""text_underline pointer"" href=""adminOnline.aspx?errors=Y&error_count=" + es.Item("tcount").ToString + """>" + es.Item("tcount").ToString.Trim + "</a>")
                                End If
                            End If
                        End If

                    Next
                Else
                    tmpHtmlOut.Append("0")
                End If

                tmpHtmlOut.Append("</strong></div></div></div>")

                If String.IsNullOrEmpty(type_to_show.Trim) Then
                    tmpHtmlOut.Append("</div>")
                End If

                If bShowHostNames Then


                    tmpHtmlOut.Append("<div class=""row""><div class=""twelve columns""><div class=""Box""><table width=""95%"" cellspacing=""2"" cellpadding=""2"" class=""formatTable blue""><thead><tr>")
                    tmpHtmlOut.Append("<th>WEBSERVER</th><th class=""text_align_right"">COUNT</th></tr></thead>")

                    tmpHtmlOut.Append("<tbody><tr><td>JETNETEVOLUTION</td>")
                    tmpHtmlOut.Append("<td class=""text_align_right""><a class=""underline pointer"" href=""adminCurrentUsers.aspx?server=JETNETEVOLUTION"">" + new_evo_count.ToString + "</a></td></tr><tr>")
                    tmpHtmlOut.Append("<td>TESTJETNETEVOLUTION</td>")
                    tmpHtmlOut.Append("<td class=""text_align_right""><a class=""underline pointer"" href=""adminCurrentUsers.aspx?server=TESTJETNETEVOLUTION"">" + new_evo_test_count.ToString + "</a></td></tr><tr>")

                    tmpHtmlOut.Append("<td>YACHT-SPOTONLINE</td>")
                    tmpHtmlOut.Append("<td class=""text_align_right""><a class=""underline pointer"" href=""adminCurrentUsers.aspx?server=YACHT-SPOTONLINE"">" + yacht_spot_count.ToString + "</a></td></tr><tr>")
                    tmpHtmlOut.Append("<td>YACHT-SPOTTEST</td>")
                    tmpHtmlOut.Append("<td class=""text_align_right""><a class=""underline pointer"" href=""adminCurrentUsers.aspx?server=YACHT-SPOTONLINE"">" + yacht_spot_test_count.ToString + "</a></td></tr><tr>")

                    tmpHtmlOut.Append("<td>LOCALHOST</td>")
                    tmpHtmlOut.Append("<td class=""text_align_right""><a class=""underline pointer"" href=""adminCurrentUsers.aspx?server=NEWEVONET,LOCALHOST,YACHTSITE,EVOADMIN"">" + local_count.ToString + "</a></td></tr><tr>")

                    tmpHtmlOut.Append("<td>EVOLUTIONADMIN</td>")
                    tmpHtmlOut.Append("<td class=""text_align_right""><a class=""underline pointer"" href=""adminCurrentUsers.aspx?server=EVOLUTIONADMIN"">" + evo_admin_count.ToString + "</a></td></tr><tr>")
                    tmpHtmlOut.Append("<td>EVOLUTIONADMINTEST</td>")
                    tmpHtmlOut.Append("<td class=""text_align_right""><a class=""underline pointer"" href=""adminCurrentUsers.aspx?server=EVOLUTIONADMINTEST"">" + evo_admin_test_count.ToString + "</a></td></tr><tr>")

                    tmpHtmlOut.Append("<td>ADMIN.JETNET.COM</td>")
                    tmpHtmlOut.Append("<td class=""text_align_right""><a class=""underline pointer"" href=""adminCurrentUsers.aspx?server=ADMIN.JETNET"">" + jetnet_admin_count.ToString + "</a></td></tr><tr>")
                    tmpHtmlOut.Append("<td>ADMINTEST.JETNET.COM</td>")
                    tmpHtmlOut.Append("<td class=""text_align_right""><a class=""underline pointer"" href=""adminCurrentUsers.aspx?server=ADMINTEST.JETNET"">" + jetnet_admin_test_count.ToString + "</a></td></tr><tr>")

                    tmpHtmlOut.Append("<td>JETNETEVO</td>")
                    tmpHtmlOut.Append("<td class=""text_align_right""><a class=""underline pointer"" href=""adminCurrentUsers.aspx?server=JETNETEVO"">" + old_evo_count.ToString + "</a></td></tr><tr>")

                    tmpHtmlOut.Append("<td>MOBILE.JETNET.COM</td>")
                    tmpHtmlOut.Append("<td class=""text_align_right""><a class=""underline pointer"" href=""adminCurrentUsers.aspx?server=MOBILE.JETNET"">" + jetnet_mobile_on_count.ToString + "</a></td></tr><tr>")
                    tmpHtmlOut.Append("<td>JETNETEVOMOBILE</td>")
                    tmpHtmlOut.Append("<td class=""text_align_right""><a class=""underline pointer"" href=""adminCurrentUsers.aspx?server=JETNETEVOMOBILE"">" + evo_mobile_on_count.ToString + "</a></td></tr><tr>")
                    tmpHtmlOut.Append("<td>TESTEVOLUTIONMOBILE</td>")
                    tmpHtmlOut.Append("<td class=""text_align_right""><a class=""underline pointer"" href=""adminCurrentUsers.aspx?server=TESTEVOLUTIONMOBILE"">" + evo_mobile_test_on_count.ToString + "</a></td></tr><tr>")

                    tmpHtmlOut.Append("<td>JETNETTEST</td>")
                    tmpHtmlOut.Append("<td class=""text_align_right""><a class=""underline pointer"" href=""adminCurrentUsers.aspx?server=JETNETTEST"">" + test_count.ToString + "</a></td></tr><tr>")

                    tmpHtmlOut.Append("<td>HOMEBASE</td>")
                    tmpHtmlOut.Append("<td class=""text_align_right""><a class=""underline pointer"" href=""adminCurrentUsers.aspx?server=HOMEBASE"">" + homebase_count.ToString + "</a></td></tr><tr>")

                    Dim total_crm As Integer = master_monthly + master_weekly + master_live + master_biweekly
                    Dim total_other As Integer = new_evo_count + new_evo_test_count + yacht_spot_count + yacht_spot_test_count + local_count + evo_admin_count + evo_admin_test_count + jetnet_admin_count + jetnet_admin_test_count + old_evo_count + jetnet_mobile_on_count + evo_mobile_on_count + evo_mobile_test_on_count + test_count + homebase_count

                    tmpHtmlOut.Append("<td>CRM</td>")
                    tmpHtmlOut.Append("<td class=""text_align_right"">" + total_crm.ToString + "</td></tr><tr>")

                    tmpHtmlOut.Append("<td>TOTAL</td>")
                    tmpHtmlOut.Append("<td class=""text_align_right"">" + (total_crm + total_other).ToString + "</td></tr>")

                    tmpHtmlOut.Append("</table></div>")
                Else
                    tmpHtmlOut.Append("<div class=""Box""><a class=""underline pointer"" href=""adminOnline.aspx?showHostnames=Y"">Get User Host Name Counts</a></div>")

                End If

                htmlOut.Append(tmpHtmlOut.ToString())

            Else ' show the error list

                htmlOut.Append("<div class=""Box""><div class=""subHeader"">" + error_count.ToString + " ERRORS REPORTED TODAY</div><table width=""95%"" border=""0"" cellspacing=""2"" cellpadding=""2"" class=""formatTable blue"">")

                sQuery = New StringBuilder

                sQuery.Append("SELECT DISTINCT Subscription_Install_Log.*, comp_name, comp_city,comp_state, comp_id,")
                sQuery.Append(" amod_make_name, amod_model_name, amod_id, ac_ser_no_full, ac_id, evoview_title, evoview_id,")
                sQuery.Append(" contact_first_name, contact_last_name, contact_id")
                sQuery.Append(" FROM Subscription_Install_Log")
                sQuery.Append(" INNER JOIN Subscription WITH(NOLOCK) ON sub_id = subislog_subid")
                sQuery.Append(" INNER JOIN Subscription_Login WITH(NOLOCK) ON (sublogin_sub_id = sub_id)")
                sQuery.Append(" INNER JOIN Subscription_Install WITH(NOLOCK) ON (subins_sub_id = sub_id  AND  sublogin_login = subins_login)")
                sQuery.Append(" LEFT OUTER JOIN Company ON subislog_comp_id = comp_id AND comp_journ_id = 0")
                sQuery.Append(" LEFT OUTER JOIN Contact ON subislog_contact_id = contact_id AND contact_journ_id = 0")
                sQuery.Append(" LEFT OUTER JOIN aircraft_model ON subislog_amod_id = amod_id")
                sQuery.Append(" LEFT OUTER JOIN aircraft ON subislog_ac_id = ac_id AND ac_journ_id = 0")
                sQuery.Append(" LEFT OUTER JOIN Evolution_Views ON subislog_view_id = evoview_id AND evoview_id > 0")
                sQuery.Append(" WHERE subislog_date >= '" + Now().ToShortDateString + "' AND subislog_msg_type = 'UserError'")
                sQuery.Append(" ORDER BY subislog_date DESC")

                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>current_subscriber_summary(ByVal productCode As String, ByVal type_to_show As String, ByVal companyID As Long, ByVal errors As Boolean, ByVal error_count As Integer) As String</b><br />" + sQuery.ToString

                SqlCommand.CommandText = sQuery.ToString
                SqlReader = SqlCommand.ExecuteReader()

                ' clear previous query results
                atemptable = New DataTable

                Try
                    atemptable.Load(SqlReader)
                Catch constrExc As System.Data.ConstraintException
                    Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                    aError = "Error in current_subscriber_summary load datatable " + constrExc.Message
                End Try

                SqlReader.Close()
                SqlReader = Nothing

                If atemptable.Rows.Count > 0 Then

                    Dim tmpMessage As String = ""

                    htmlOut.Append("<thead><tr>")
                    htmlOut.Append("<th>Time</th>")
                    htmlOut.Append("<th>Email</th>")
                    htmlOut.Append("<th>Type</th>")
                    htmlOut.Append("<th>Action</th>")
                    htmlOut.Append("<th>Site</th>")
                    htmlOut.Append("</tr></thead><tbody>" + vbCrLf)

                    For Each es As DataRow In atemptable.Rows

                        tmpMessage = ""

                        If Not IsDBNull(es.Item("subislog_email_address")) Then
                            If Not String.IsNullOrEmpty(es.Item("subislog_email_address").ToString.Trim) Then

                                tmpHtmlOut.Append("<tr>")

                                tmpHtmlOut.Append("<td align=""left"" nowrap=""nowrap"">")

                                If Not IsDBNull(es.Item("subislog_date")) Then
                                    If Not String.IsNullOrEmpty(es.Item("subislog_date").ToString.Trim) Then
                                        tmpHtmlOut.Append(FormatDateTime(es.Item("subislog_date").ToString.Trim, DateFormat.GeneralDate).ToString)
                                    End If
                                End If

                                tmpHtmlOut.Append("</td>")

                                tmpHtmlOut.Append("<td align=""left"" nowrap=""nowrap"">")

                                If Not IsDBNull(es.Item("subislog_email_address")) Then
                                    If Not String.IsNullOrEmpty(es.Item("subislog_email_address").ToString.Trim) Then
                                        tmpHtmlOut.Append("<a href=""javascript:void(0);"" onclick=""javascript:load('adminSubErrors.aspx?email=" + HttpContext.Current.Server.UrlEncode(es.Item("subislog_email_address").ToString.Trim) + "','','scrollbars=yes,menubar=no,height=600,width=1250,resizable=yes,toolbar=no,location=no,status=no');""  title=""Click to see errors for this user"">" + es.Item("subislog_email_address").ToString.Trim + "</a>")
                                    End If
                                End If

                                tmpHtmlOut.Append("</td>")


                                tmpHtmlOut.Append("<td align=""left"">")

                                If Not IsDBNull(es.Item("subislog_msg_type")) Then
                                    If Not String.IsNullOrEmpty(es.Item("subislog_msg_type").ToString.Trim) Then
                                        tmpHtmlOut.Append(es.Item("subislog_msg_type").ToString.Trim)
                                    End If
                                End If

                                tmpHtmlOut.Append("</td>")

                                tmpHtmlOut.Append("<td align=""left"">")

                                If Not IsDBNull(es.Item("subislog_message")) Then
                                    If Not String.IsNullOrEmpty(es.Item("subislog_message").ToString.Trim) Then

                                        If es.Item("subislog_message").ToString.ToLower.Trim.Contains("displayaircraftdetail") Then
                                            tmpMessage = "Aircraft:"

                                            If Not IsDBNull(es.Item("amod_make_name")) Then
                                                If Not String.IsNullOrEmpty(es.Item("amod_make_name").ToString.Trim) Then
                                                    tmpMessage += Constants.cSingleSpace + es.Item("amod_make_name").ToString.Trim
                                                End If
                                            End If

                                            If Not IsDBNull(es.Item("amod_model_name")) Then
                                                If Not String.IsNullOrEmpty(es.Item("amod_model_name").ToString.Trim) Then
                                                    tmpMessage += Constants.cSingleSpace + es.Item("amod_model_name").ToString.Trim
                                                End If
                                            End If

                                            If Not IsDBNull(es.Item("amod_id")) Then
                                                If Not String.IsNullOrEmpty(es.Item("amod_id").ToString.Trim) Then
                                                    tmpMessage += Constants.cSingleSpace + es.Item("amod_id").ToString.Trim
                                                End If
                                            End If

                                            If Not IsDBNull(es.Item("ac_ser_no_full")) Then
                                                If Not String.IsNullOrEmpty(es.Item("ac_ser_no_full").ToString.Trim) Then
                                                    tmpMessage += Constants.cSingleSpace + es.Item("ac_ser_no_full").ToString.Trim
                                                End If
                                            End If

                                            If Not IsDBNull(es.Item("ac_id")) Then
                                                If Not String.IsNullOrEmpty(es.Item("ac_id").ToString.Trim) Then
                                                    tmpMessage += Constants.cSingleSpace + es.Item("ac_id").ToString.Trim
                                                End If
                                            End If

                                        ElseIf es.Item("subislog_message").ToString.ToLower.Trim.Contains("displaycompanydetail") Then

                                            tmpMessage = "Company:"

                                            If Not IsDBNull(es.Item("comp_name")) Then
                                                If Not String.IsNullOrEmpty(es.Item("comp_name").ToString.Trim) Then
                                                    tmpMessage += Constants.cSingleSpace + es.Item("comp_name").ToString.Trim
                                                End If
                                            End If

                                            If Not IsDBNull(es.Item("comp_city")) Then
                                                If Not String.IsNullOrEmpty(es.Item("comp_city").ToString.Trim) Then
                                                    tmpMessage += Constants.cSingleSpace + es.Item("comp_city").ToString.Trim + IIf(Not IsDBNull(es.Item("comp_state")), Constants.cColonDelim, "")
                                                End If
                                            End If

                                            If Not IsDBNull(es.Item("comp_state")) Then
                                                If Not String.IsNullOrEmpty(es.Item("comp_state").ToString.Trim) Then
                                                    tmpMessage += Constants.cSingleSpace + es.Item("comp_state").ToString.Trim
                                                End If
                                            End If

                                            If Not IsDBNull(es.Item("comp_id")) Then
                                                If Not String.IsNullOrEmpty(es.Item("comp_id").ToString.Trim) Then
                                                    tmpMessage += Constants.cSingleSpace + es.Item("comp_id").ToString.Trim
                                                End If
                                            End If

                                        ElseIf es.Item("subislog_message").ToString.ToLower.Trim.Contains("displaycontactdetails") Then

                                            tmpMessage = "Company:"

                                            If Not IsDBNull(es.Item("comp_name")) Then
                                                If Not String.IsNullOrEmpty(es.Item("comp_name").ToString.Trim) Then
                                                    tmpMessage += Constants.cSingleSpace + es.Item("comp_name").ToString.Trim
                                                End If
                                            End If

                                            If Not IsDBNull(es.Item("comp_city")) Then
                                                If Not String.IsNullOrEmpty(es.Item("comp_city").ToString.Trim) Then
                                                    tmpMessage += Constants.cSingleSpace + es.Item("comp_city").ToString.Trim + IIf(Not IsDBNull(es.Item("comp_state")), Constants.cColonDelim, "")
                                                End If
                                            End If

                                            If Not IsDBNull(es.Item("comp_state")) Then
                                                If Not String.IsNullOrEmpty(es.Item("comp_state").ToString.Trim) Then
                                                    tmpMessage += Constants.cSingleSpace + es.Item("comp_state").ToString.Trim
                                                End If
                                            End If

                                            If Not IsDBNull(es.Item("comp_id")) Then
                                                If Not String.IsNullOrEmpty(es.Item("comp_id").ToString.Trim) Then
                                                    tmpMessage += Constants.cSingleSpace + es.Item("comp_id").ToString.Trim
                                                End If
                                            End If

                                            tmpMessage += " Contact:"

                                            If Not IsDBNull(es.Item("contact_first_name")) Then
                                                If Not String.IsNullOrEmpty(es.Item("contact_first_name").ToString.Trim) Then
                                                    tmpMessage += Constants.cSingleSpace + es.Item("contact_first_name").ToString.Trim
                                                End If
                                            End If

                                            If Not IsDBNull(es.Item("contact_last_name")) Then
                                                If Not String.IsNullOrEmpty(es.Item("contact_last_name").ToString.Trim) Then
                                                    tmpMessage += Constants.cSingleSpace + es.Item("contact_last_name").ToString.Trim
                                                End If
                                            End If

                                            If Not IsDBNull(es.Item("contact_id")) Then
                                                If Not String.IsNullOrEmpty(es.Item("contact_id").ToString.Trim) Then
                                                    tmpMessage += Constants.cSingleSpace + es.Item("contact_id").ToString.Trim
                                                End If
                                            End If

                                            tmpHtmlOut.Append(HttpContext.Current.Server.HtmlEncode(tmpMessage))

                                        Else

                                            Dim nCount As Integer = 0

                                            If Not IsDBNull(es.Item("subislog_message")) Then
                                                If Not String.IsNullOrEmpty(es.Item("subislog_message").ToString.Trim) Then

                                                    nCount = 0
                                                    For Each ch As Char In es.Item("subislog_message").ToString

                                                        If Not Char.IsControl(ch) Then
                                                            If Char.IsWhiteSpace(ch) Then
                                                                tmpHtmlOut.Append(Constants.cSingleSpace)
                                                            Else
                                                                tmpHtmlOut.Append(ch)
                                                            End If
                                                        End If

                                                        nCount += 1

                                                        If nCount Mod 50 = 0 Then
                                                            tmpHtmlOut.Append("<br/>")
                                                        End If

                                                    Next
                                                End If
                                            End If

                                        End If

                                    End If
                                End If

                                tmpHtmlOut.Append("</td>")

                                tmpHtmlOut.Append("<td align=""left"">")

                                If Not IsDBNull(es("subislog_host_name")) Then
                                    If Not String.IsNullOrEmpty(es("subislog_host_name").ToString.Trim) Then
                                        tmpHtmlOut.Append(es.Item("subislog_host_name").ToString.ToUpper.Replace(".COM", "").Trim)
                                    End If
                                End If

                                tmpHtmlOut.Append("</td>")

                                tmpHtmlOut.Append("</tr>" + vbCrLf)

                            End If
                        End If

                    Next
                    tmpHtmlOut.Append("</tbody>")
                    htmlOut.Append(tmpHtmlOut.ToString())

                Else

                    htmlOut.Append("<tr><td width=""100"" align=""center"" colspan=""5"">NO ERRORS REPORTED TODAY</td></tr>")

                End If

                htmlOut.Append("</table></div>")

            End If ' show errors


        Catch ex As Exception

            aError = "Error in current_subscriber_summary(ByVal type As String, ByVal type_to_show As String, ByVal companyID As Long, ByVal errors As Boolean, ByVal error_count As Integer) As String" + ex.Message

        Finally

            SqlCommand.Dispose()
            SqlCommand = Nothing

            SqlConn.Close()
            SqlConn.Dispose()
            SqlConn = Nothing

            MySqlConn.Close()
            MySqlConn.Dispose()
            MySqlCommand.Dispose()

            MySqlCommand = Nothing
            MySqlConn = Nothing

        End Try

        Return htmlOut.ToString
        htmlOut = Nothing

    End Function

    'Public Function getAdminPlatformTable(ByVal from_where_string As String, ByRef total_out As Integer, ByVal sub_freq As String, ByVal search_clause As String) As String

    '  Dim htmlOut As New StringBuilder
    '  Dim sQuery = New StringBuilder()

    '  Dim SqlConn As New SqlClient.SqlConnection
    '  Dim SqlCommand As New SqlClient.SqlCommand
    '  Dim SqlReader As SqlClient.SqlDataReader = Nothing

    '  Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    '  Dim sSeperator As String = ""
    '  Dim atemptable As New DataTable

    '  Try

    '    sQuery.Append("SELECT DISTINCT sub_frequency As SubFreq, COUNT(DISTINCT sub_parent_sub_id) As NbrClients") ' -- NUMBER OF PARENT SUBS = # CLIENTS" 

    '    If Not String.IsNullOrEmpty(from_where_string.Trim) Then
    '      sQuery.Append(Constants.cSingleSpace + from_where_string.Trim)
    '    End If

    '    If Not String.IsNullOrEmpty(sub_freq.Trim) Then
    '      sQuery.Append(Constants.cAndClause + "sub_frequency = '" + sub_freq.Trim + "'")
    '    End If

    '    sQuery.Append(Constants.cSingleSpace + search_clause.Trim)

    '    sQuery.Append(" AND (sub_comp_id <> 135887)")
    '    sQuery.Append(" GROUP BY sub_frequency")
    '    sQuery.Append(" ORDER BY sub_frequency")

    '    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>getAdminOnLineSummaryTotal(ByVal from_where_string As String, ByRef total_out As Integer, ByVal sub_freq As String, ByVal search_clause As String) As String</b><br />" + sQuery.ToString

    '    SqlConn.ConnectionString = adminConnectStr

    '    SqlConn.Open()

    '    SqlCommand.Connection = SqlConn
    '    SqlCommand.CommandType = System.Data.CommandType.Text
    '    SqlCommand.CommandTimeout = 240

    '    SqlCommand.CommandText = sQuery.ToString
    '    SqlReader = SqlCommand.ExecuteReader()

    '    Try
    '      atemptable.Load(SqlReader)
    '    Catch constrExc As System.Data.ConstraintException
    '      Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
    '      aError = "Error in getAdminOnLineSummaryTotal load datatable " + constrExc.Message
    '    End Try

    '    SqlReader.Close()
    '    SqlReader = Nothing

    '    If atemptable.Rows.Count > 0 Then

    '      For Each es As DataRow In atemptable.Rows
    '        htmlOut.Append("<td align=""right"">" + es.Item("NbrClients").ToString + "</td>")
    '        total_out += CInt(es.Item("NbrClients").ToString)
    '      Next

    '    Else
    '      htmlOut.Append("<td align=""right"">0</td>")
    '    End If

    '  Catch ex As Exception

    '    aError = "Error in getAdminOnLineSummaryTotal(ByVal from_where_string As String, ByRef total_out As Integer, ByVal sub_freq As String, ByVal search_clause As String) As String " + ex.Message

    '  Finally

    '    SqlCommand.Dispose()
    '    SqlCommand = Nothing

    '    SqlConn.Close()
    '    SqlConn.Dispose()
    '    SqlConn = Nothing

    '  End Try

    '  Return htmlOut.ToString
    '  htmlOut = Nothing

    'End Function

    Public Function displayAdminOnLineOverview(ByVal displayType As String, ByVal displayFreq As String, ByVal cbus_name As String, ByVal productCode As String, ByVal service As String, ByVal orderByClause As String) As String

        Dim htmlOut As New StringBuilder
        Dim sQuery = New StringBuilder()
        Dim sSearchClause = New StringBuilder()

        Dim text_for_type As String = ""

        Dim total_clients As Integer = 0
        Dim total_sub As Integer = 0
        Dim total_locations As Integer = 0
        Dim total_installs As Integer = 0
        Dim total_comp As Integer = 0

        Dim total_spi As Integer = 0
        Dim total_cloud As Integer = 0
        Dim total_server As Integer = 0
        Dim total_crm As Integer = 0

        Dim saveOrderBy As String = ""

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader = Nothing

        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim sSeperator As String = ""
        Dim atemptable As New DataTable

        Try

            sQuery.Append("SELECT DISTINCT")

            If displayType.Trim.ToLower.Contains("freq") Then
                sQuery.Append(" sub_frequency AS SubFreq,")
            ElseIf displayType.Trim.ToLower.Contains("types") Then
                sQuery.Append(" comp_business_type, cbus_name,")
            End If

            sQuery.Append(" COUNT(DISTINCT sub_comp_id) AS NbrLocations,")     '-- NUMBER OF LOCATIONS = # COMPANY IDS" 
            sQuery.Append(" COUNT(DISTINCT sub_parent_sub_id) AS NbrClients,") '-- NUMBER OF PARENT SUBS = # CLIENTS"
            sQuery.Append(" COUNT(DISTINCT sub_id) AS NbrSubscriptions,")     ' -- NUMBER OF SUBSCRIPTIONS"

            If displayType.Trim.ToLower.Contains("freq") Then
                sQuery.Append(" COUNT(*) AS NbrInstalls") '-- NUMBER OF VALID USERS"
            ElseIf displayType.Trim.ToLower.Contains("types") Then
                sQuery.Append(" COUNT(*) AS NbrInstalls,") '-- NUMBER OF VALID USERS"
                sQuery.Append(" (SELECT COUNT(*) FROM COMPANY WITH(NOLOCK) WHERE comp_journ_id = 0 AND comp_active_flag='Y' AND comp_business_type = View_JETNET_Customers.comp_business_type) AS NbrCompanies") '-- NUMBER OF VALID COMPANIES"
            End If

            sQuery.Append(" FROM View_JETNET_Customers WITH(NOLOCK)")
            sQuery.Append(" WHERE (sub_comp_id <> 135887) AND (sublogin_demo_flag = 'N')")

            If Not String.IsNullOrEmpty(cbus_name.Trim) Then
                sQuery.Append(Constants.cAndClause + "comp_business_type = '" + cbus_name.Trim + "'")
            End If

            If Not String.IsNullOrEmpty(productCode.Trim) Then

                sSearchClause.Append(" AND (")

                If productCode.ToUpper.Trim.Contains("B") Then

                    sSearchClause.Append("sub_business_aircraft_flag = 'Y'")

                    If String.IsNullOrEmpty(text_for_type) Then
                        text_for_type = "Business"
                    Else
                        text_for_type += ", Business"
                    End If

                    sSeperator = Constants.cOrClause

                End If

                If productCode.ToUpper.Trim.Contains("H") Then

                    sSearchClause.Append(sSeperator + "sub_helicopters_flag = 'Y'")

                    If String.IsNullOrEmpty(text_for_type) Then
                        text_for_type = "Helicopter"
                    Else
                        text_for_type += ", Helicopter"
                    End If

                    sSeperator = Constants.cOrClause

                End If

                If productCode.ToUpper.Trim.Contains("C") Then

                    sSearchClause.Append(sSeperator + "sub_commerical_flag = 'Y'")

                    If String.IsNullOrEmpty(text_for_type) Then
                        text_for_type = "Commercial"
                    Else
                        text_for_type += ", Commercial"
                    End If

                    sSeperator = Constants.cOrClause

                End If

                If productCode.ToUpper.Trim.Contains("Y") Then

                    sSearchClause.Append(sSeperator + "sub_yacht_flag = 'Y'")

                    If String.IsNullOrEmpty(text_for_type) Then
                        text_for_type = "Yacht"
                    Else
                        text_for_type += ", Yacht"
                    End If

                End If

                sSearchClause.Append(")")

            Else
                text_for_type = "All "
            End If

            If Not String.IsNullOrEmpty(service.Trim) Then
                If service.ToUpper.Trim.Contains("A") Then
                    sSearchClause.Append(Constants.cAndClause + "sub_aerodex_flag = 'Y'")
                    text_for_type += " - Aerodex"
                Else
                    sSearchClause.Append(Constants.cAndClause + "sub_aerodex_flag = 'N'")
                    text_for_type += " - Marketplace"
                End If
            End If

            sQuery.Append(sSearchClause.ToString)

            If Not String.IsNullOrEmpty(displayFreq.Trim) Then
                sQuery.Append(Constants.cAndClause + "sub_frequency = '" + displayFreq.Trim + "'")
                text_for_type += Constants.cSingleSpace + displayFreq.Trim
            End If

            saveOrderBy = orderByClause

            If Not String.IsNullOrEmpty(orderByClause.Trim) Then
                Select Case (orderByClause.ToUpper.Trim)
                    Case "LOC"
                        orderByClause = " COUNT(DISTINCT sub_comp_id)"
                    Case "CLIENTS"
                        orderByClause = " COUNT(DISTINCT sub_parent_sub_id)"
                    Case "SUB"
                        orderByClause = " COUNT(DISTINCT sub_id)"
                    Case "INSTALL"
                        orderByClause = " COUNT(*)"
                End Select
            End If

            If displayType.Trim.ToLower.Contains("freq") Then 'sOrderByClause
                sQuery.Append(" GROUP BY sub_frequency")
                If Not String.IsNullOrEmpty(orderByClause.Trim) Then
                    sQuery.Append(" ORDER BY " + orderByClause.Trim + " DESC")
                Else
                    sQuery.Append(" ORDER BY sub_frequency")
                End If
            ElseIf displayType.Trim.ToLower.Contains("types") Then
                sQuery.Append(" GROUP BY comp_business_type, cbus_name")
                If Not String.IsNullOrEmpty(orderByClause.Trim) Then
                    sQuery.Append(" ORDER BY " + orderByClause.Trim + " DESC")
                Else
                    sQuery.Append(" ORDER BY comp_business_type")
                End If
            End If

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>all_subscriber_summary(ByVal displayType As String, ByVal displayFreq As String, ByVal cbus_name As String, ByVal productCode As String, ByVal service As String, ByVal orderByClause As String) As String</b><br />" + sQuery.ToString

            SqlConn.ConnectionString = adminConnectStr

            SqlConn.Open()

            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = System.Data.CommandType.Text
            SqlCommand.CommandTimeout = 240
            htmlOut.Append("<div class=""row""><div class=""twelve columns""><div class=""Box""><div class=""subHeader"">")
            If displayType.Trim.ToLower.Contains("freq") Then
                htmlOut.Append("" + text_for_type.Trim + " Summary")
            ElseIf displayType.Trim.ToLower.Contains("types") Then
                htmlOut.Append("" + text_for_type.Trim + " Business Types")
            End If
            htmlOut.Append("</div>")
            If displayType.Trim.ToLower.Contains("freq") Then
                htmlOut.Append("<table width=""100%""  border=""0"" cellspacing=""2"" cellpadding=""2"" class=""formatTable blue""><thead><tr>")
                htmlOut.Append("<th align=""left"" width=""200"">FREQUENCY</th>")
            ElseIf displayType.Trim.ToLower.Contains("types") Then
                htmlOut.Append("<table width=""100%""  border=""0"" cellspacing=""2"" cellpadding=""2"" class=""formatTable blue""><thead><tr>")
                htmlOut.Append("<th align=""left"" width=""200"">BUSINESS&nbsp;TYPE</th>")
            End If

            htmlOut.Append("<th class=""text_align_right""><a class=""underline pointer"" href=""adminOnline.aspx?overView=Y&order=clients&service=" + service.Trim + "&productCode=" + productCode.Trim + "&freq=" + displayFreq.Trim + """>CLIENTS</a></th>")
            htmlOut.Append("<th class=""text_align_right""><a class=""underline pointer"" href=""adminOnline.aspx?overView=Y&order=loc&service=" + service.Trim + "&productCode=" + productCode.Trim + "&freq=" + displayFreq.Trim + """>CLIENT&nbsp;LOCATIONS</a></th>")
            htmlOut.Append("<th class=""text_align_right""><a class=""underline pointer"" href=""adminOnline.aspx?overView=Y&order=sub&service=" + service.Trim + "&productCode=" + productCode.Trim + "&freq=" + displayFreq.Trim + """>SUBSCRIPTIONS</a></th>")
            htmlOut.Append("<th class=""text_align_right""><a class=""underline pointer"" href=""adminOnline.aspx?overView=Y&order=install&service=" + service.Trim + "&productCode=" + productCode.Trim + "&freq=" + displayFreq.Trim + """>LICENSES</a></th>")

            If displayType.Trim.ToLower.Contains("freq") Then
                htmlOut.Append("<th class=""text_align_right"">VALUES</td>")
                htmlOut.Append("<th class=""text_align_right"">CLOUD&nbsp;NOTES</th>")
                htmlOut.Append("<th class=""text_align_right"">CLOUD&nbsp;NOTES+</th>")
                htmlOut.Append("<th class=""text_align_right"">CRM</td>")
            ElseIf displayType.Trim.ToLower.Contains("types") Then
                htmlOut.Append("<th class=""text_align_right"">ALL&nbsp;LOCATIONS</th>")
                htmlOut.Append("<th class=""text_align_right"">%&nbsp;CLIENTS</th>")
            End If

            htmlOut.Append("</tr></thead><tbody>")

            SqlCommand.CommandText = sQuery.ToString
            SqlReader = SqlCommand.ExecuteReader()

            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                aError = "Error in current_subscriber_summary load datatable " + constrExc.Message
            End Try

            SqlReader.Close()
            SqlReader = Nothing

            If atemptable.Rows.Count > 0 Then

                For Each es As DataRow In atemptable.Rows

                    htmlOut.Append("<tr>")

                    If displayType.Trim.ToLower.Contains("freq") Then
                        htmlOut.Append("<td align=""left""><a class=""underline pointer"" href=""adminOnline.aspx?overView=Y&freq=" + es.Item("SubFreq").ToString.Trim + "&service=" + service.Trim + "&productCode=" + productCode.Trim + "&order=" + saveOrderBy.Trim + """>" + es.Item("SubFreq").ToString.Trim + "</a></td>")
                    ElseIf displayType.Trim.ToLower.Contains("types") Then
                        htmlOut.Append("<td align=""left""><a class=""underline pointer"" href=""adminCurrentUsers.aspx?bus_type=" + es.Item("comp_business_type").ToString.Trim + "&service=" + service.Trim + "&productCode=" + productCode.Trim + "&freq=" + displayFreq.Trim + """>" + es.Item("cbus_name").ToString.Trim + "</a></td>")
                    End If

                    htmlOut.Append("<td align=""right"">" + es.Item("NbrClients").ToString.Trim + "</td>")
                    total_clients += CInt(es.Item("NbrClients").ToString)

                    htmlOut.Append("<td align=""right"">" + es.Item("NbrLocations").ToString.Trim + "</td>")
                    total_locations += CInt(es.Item("NbrLocations").ToString)

                    htmlOut.Append("<td align=""right"">" + es.Item("NbrSubscriptions").ToString.Trim + "</td>")
                    total_sub += CInt(es.Item("NbrSubscriptions").ToString)

                    htmlOut.Append("<td align=""right"">" + es.Item("NbrInstalls").ToString.Trim + "</td>")
                    total_installs += CInt(es.Item("NbrInstalls").ToString)


                    If displayType.Trim.ToLower.Contains("freq") Then

                        ' -- COUNT OF NUMBER OF CLIENTS FOR VALUES
                        htmlOut.Append(getAdminOnLineSummaryTotal("FROM View_JETNET_Customers WHERE sub_sale_price_flag='Y' AND sub_aerodex_flag = 'N'", total_spi, es.Item("SubFreq").ToString.Trim, sSearchClause.ToString))

                        ' -- COUNT OF NUMBER OF CLIENTS FOR CLOUD NOTES
                        htmlOut.Append(getAdminOnLineSummaryTotal("FROM View_JETNET_Customers WHERE sub_server_side_notes_flag='Y' AND sub_aerodex_flag = 'N' AND sub_parent_sub_id NOT IN (select distinct sub_parent_sub_id FROM View_JETNET_CRM_Customers)", total_cloud, es.Item("SubFreq").ToString.Trim, sSearchClause.ToString))

                        ' -- COUNT OF NUMBER OF CLIENTS FOR SERVER NOTES
                        htmlOut.Append(getAdminOnLineSummaryTotal("FROM View_JETNET_Customers WHERE sub_cloud_notes_flag='Y' AND sub_aerodex_flag = 'N' AND sub_parent_sub_id NOT IN (select distinct sub_parent_sub_id FROM View_JETNET_CRM_Customers)", total_server, es.Item("SubFreq").ToString.Trim, sSearchClause.ToString))

                        ' -- COUNT OF CLIENTS AND INSTALLS FOR CRM   
                        htmlOut.Append(getAdminOnLineSummaryTotal("FROM View_JETNET_CRM_Customers WHERE sub_aerodex_flag = 'N'", total_crm, es.Item("SubFreq").ToString.Trim, sSearchClause.ToString)) ' -- COUNT OF CLIENTS AND INSTALLS FOR CRM

                    ElseIf displayType.Trim.ToLower.Contains("types") Then

                        htmlOut.Append("<td align=""right"">" + es.Item("NbrCompanies").ToString.Trim + "</td>")
                        total_comp += CInt(es.Item("NbrCompanies").ToString)

                        If CInt(es.Item("NbrCompanies").ToString) > 0 Then
                            htmlOut.Append("<td align=""right"">" + FormatNumber(System.Math.Round(CDbl(CInt(es.Item("NbrLocations").ToString) / CInt(es.Item("NbrCompanies").ToString) * 100), 2), 1, False, False, False) + "%</td>")
                        Else
                            htmlOut.Append("<td align=""right"">N/A</td>")
                        End If

                    End If

                    htmlOut.Append("</tr>")

                Next

            End If

            htmlOut.Append("<tr><td class=""text_align_right""><strong>TOTALS</strong></td>")
            htmlOut.Append("<td class=""text_align_right"">" + total_clients.ToString + "</td>")
            htmlOut.Append("<td class=""text_align_right"">" + total_locations.ToString + "</td>")
            htmlOut.Append("<td class=""text_align_right"">" + total_sub.ToString + "</td>")
            htmlOut.Append("<td class=""text_align_right"">" + total_installs.ToString + "</td>")

            If displayType.Trim.ToLower.Contains("freq") Then
                htmlOut.Append("<td class=""text_align_right"">" + total_spi.ToString + "</td>")
                htmlOut.Append("<td class=""text_align_right"">" + total_cloud.ToString + "</td>")
                htmlOut.Append("<td class=""text_align_right"">" + total_server.ToString + "</td>")
                htmlOut.Append("<td class=""text_align_right"">" + total_crm.ToString + "</td>")
            ElseIf displayType.Trim.ToLower.Contains("types") Then
                htmlOut.Append("<td class=""text_align_right"">" + total_comp.ToString + "</td>")
                htmlOut.Append("<td class=""text_align_right"">&nbsp;</td>")
            End If

            htmlOut.Append("</tr></tbody></table></div></div></div>")

        Catch ex As Exception

            aError = "Error in all_subscriber_summary(ByVal displayType As String, ByVal displayFreq As String, ByVal cbus_name As String, ByVal productCode As String, ByVal service As String, ByVal orderByClause As String) As String" + ex.Message

        Finally

            SqlCommand.Dispose()
            SqlCommand = Nothing

            SqlConn.Close()
            SqlConn.Dispose()
            SqlConn = Nothing

        End Try

        Return htmlOut.ToString
        htmlOut = Nothing

    End Function

    Public Function getAdminOnLineSummaryTotal(ByVal from_where_string As String, ByRef total_out As Integer, ByVal sub_freq As String, ByVal search_clause As String) As String

        Dim htmlOut As New StringBuilder
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader = Nothing

        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim sSeperator As String = ""
        Dim atemptable As New DataTable

        Try

            sQuery.Append("SELECT DISTINCT sub_frequency As SubFreq, COUNT(DISTINCT sub_parent_sub_id) As NbrClients") ' -- NUMBER OF PARENT SUBS = # CLIENTS" 

            If Not String.IsNullOrEmpty(from_where_string.Trim) Then
                sQuery.Append(Constants.cSingleSpace + from_where_string.Trim)
            End If

            If Not String.IsNullOrEmpty(sub_freq.Trim) Then
                sQuery.Append(Constants.cAndClause + "sub_frequency = '" + sub_freq.Trim + "'")
            End If

            sQuery.Append(Constants.cSingleSpace + search_clause.Trim)

            sQuery.Append(" AND (sub_comp_id <> 135887)")
            sQuery.Append(" GROUP BY sub_frequency")
            sQuery.Append(" ORDER BY sub_frequency")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>getAdminOnLineSummaryTotal(ByVal from_where_string As String, ByRef total_out As Integer, ByVal sub_freq As String, ByVal search_clause As String) As String</b><br />" + sQuery.ToString

            SqlConn.ConnectionString = adminConnectStr

            SqlConn.Open()

            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = System.Data.CommandType.Text
            SqlCommand.CommandTimeout = 240

            SqlCommand.CommandText = sQuery.ToString
            SqlReader = SqlCommand.ExecuteReader()

            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                aError = "Error in getAdminOnLineSummaryTotal load datatable " + constrExc.Message
            End Try

            SqlReader.Close()
            SqlReader = Nothing

            If atemptable.Rows.Count > 0 Then

                For Each es As DataRow In atemptable.Rows
                    htmlOut.Append("<td align=""right"">" + es.Item("NbrClients").ToString + "</td>")
                    total_out += CInt(es.Item("NbrClients").ToString)
                Next

            Else
                htmlOut.Append("<td align=""right"">0</td>")
            End If

        Catch ex As Exception

            aError = "Error in getAdminOnLineSummaryTotal(ByVal from_where_string As String, ByRef total_out As Integer, ByVal sub_freq As String, ByVal search_clause As String) As String " + ex.Message

        Finally

            SqlCommand.Dispose()
            SqlCommand = Nothing

            SqlConn.Close()
            SqlConn.Dispose()
            SqlConn = Nothing

        End Try

        Return htmlOut.ToString
        htmlOut = Nothing

    End Function

    Public Function getAdminCompanyUsersDataTable(ByVal searchCriteria As onLineUsersSelectionCriteriaClass) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            sQuery.Append("SELECT sublogin_sub_id, contact_last_name, contact_first_name, comp_name, comp_city, comp_state, contact_email_address, sublogin_password,")
            sQuery.Append(" subins_last_login_date, subins_last_session_date, subins_last_logout_date, sub_comp_id, subins_platform_name, subins_contact_id")
            sQuery.Append(" FROM Subscription WITH(NOLOCK)")
            sQuery.Append(" INNER JOIN Subscription_Login WITH(NOLOCK) ON (sublogin_sub_id = sub_id)")
            sQuery.Append(" INNER JOIN Subscription_Install WITH(NOLOCK) ON (subins_sub_id = sub_id) AND sublogin_login = subins_login")
            sQuery.Append(" INNER JOIN Company WITH(NOLOCK) ON (comp_id = sub_comp_id) AND comp_journ_id = 0")
            sQuery.Append(" INNER JOIN Contact WITH(NOLOCK) ON (contact_comp_id = sub_comp_id) AND subins_contact_id = contact_id AND contact_journ_id = 0")
            sQuery.Append(" WHERE comp_id =" + searchCriteria.OnLineCriteriaCompanyID.ToString + " AND contact_journ_id = 0 AND comp_journ_id = 0")

            If Not String.IsNullOrEmpty(searchCriteria.OnLineCriteriaOrderBy.Trim) Then

                Select Case (searchCriteria.OnLineCriteriaOrderBy.ToUpper.Trim)
                    Case "COMP1"
                        sQuery.Append(" ORDER BY comp_name DESC, comp_city, comp_state, contact_last_name, contact_first_name")
                    Case "COMP2"
                        sQuery.Append(" ORDER BY comp_name ASC, comp_city, comp_state, contact_last_name, contact_first_name")
                    Case "NAME1"
                        sQuery.Append(" ORDER BY contact_last_name DESC, contact_first_name, comp_name, comp_city, comp_state")
                    Case "NAME2"
                        sQuery.Append(" ORDER BY contact_last_name ASC, contact_first_name, comp_name, comp_city, comp_state")

                End Select
            Else
                sQuery.Append(" ORDER BY contact_last_name ASC, contact_first_name, comp_name, comp_city, comp_state")
            End If

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>getAdminCompanyUsersDataTable(ByVal searchCriteria As onLineUsersSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

            SqlConn.ConnectionString = adminConnectStr

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
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getAdminCompanyUsersDataTable load datatable</b><br /> " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getAdminCompanyUsersDataTable(ByVal searchCriteria As onLineUsersSelectionCriteriaClass) As DataTable</b><br />" + ex.Message

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

    Public Sub displayAdminCompanyUsers(ByVal searchCriteria As onLineUsersSelectionCriteriaClass, ByRef out_tabTitle As String, ByRef out_htmlString As String)

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False

        Try

            out_tabTitle = "Users Logged In"

            results_table = getAdminCompanyUsersDataTable(searchCriteria)

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    htmlOut.Append("<table id=""companyUsersDataTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"">")
                    htmlOut.Append("<tr bgcolor=""#D9D8D8""><td valign=""middle"" align=""left""><b>SUB&nbsp;ID</b></td>")

                    If searchCriteria.OnLineCriteriaOrderBy.ToUpper.Contains("NAME1") Then
                        htmlOut.Append("<td valign=""middle"" align=""left""><b><a class=""underline pointer"" href=""adminCurrentUsers.aspx?order=name2"" title=""Order by contact Last name"">USER NAME</a></td>")
                    Else
                        htmlOut.Append("<td valign=""middle"" align=""left""><b><a class=""underline pointer"" href=""adminCurrentUsers.aspx?order=name1"" title=""Order by contact Last name"">USER NAME</a></td>")
                    End If

                    htmlOut.Append("<td valign=""middle"" align=""left""><b>EMAIL ADDRESS</b></td>")
                    htmlOut.Append("<td valign=""middle"" align=""left""><b>PASSWORD</b></td>")

                    If searchCriteria.OnLineCriteriaOrderBy.ToUpper.Contains("COMP1") Then
                        htmlOut.Append("<td valign=""middle"" align=""left""><b><a class=""underline pointer"" href=""adminCurrentUsers.aspx?order=comp2"" title=""Order by company name"">PLATFORM NAME</a></td>")
                    Else
                        htmlOut.Append("<td valign=""middle"" align=""left""><b><a class=""underline pointer"" href=""adminCurrentUsers.aspx?order=comp1"" title=""Order by company name"">PLATFORM NAME</a></td>")
                    End If

                    htmlOut.Append("<td valign=""middle"" align=""left""><b>LOGIN</b></td></tr>")

                    For Each r As DataRow In results_table.Rows

                        If Not toggleRowColor Then
                            htmlOut.Append("<tr class=""alt_row"">")
                            toggleRowColor = True
                        Else
                            htmlOut.Append("<tr bgcolor=""white"">")
                            toggleRowColor = False
                        End If

                        If Not IsDBNull(r.Item("sublogin_sub_id")) Then
                            If Not String.IsNullOrEmpty(r.Item("sublogin_sub_id").ToString.Trim) Then
                                htmlOut.Append("<td valign=""middle"" align=""left"">" + r.Item("sublogin_sub_id").ToString.Trim + "</td>")
                            Else
                                htmlOut.Append("<td valign=""middle"" align=""left""></td>")
                            End If
                        Else
                            htmlOut.Append("<td valign=""middle"" align=""left""></td>")
                        End If

                        htmlOut.Append("<td valign=""middle"" align=""left"" wrap=""nowrap"">")
                        htmlOut.Append("<a class=""underline pointer"" href=""adminCurrentUsers.aspx?user_id=" + r.Item("subins_contact_id").ToString.Trim + "&id=" + r.Item("sub_comp_id").ToString.Trim + """>" + r.Item("contact_last_name").ToString.Trim + ", " + r.Item("contact_first_name").ToString.Trim + "</a>")
                        htmlOut.Append("</td>")

                        If Not IsDBNull(r.Item("contact_email_address")) Then
                            If Not String.IsNullOrEmpty(r.Item("contact_email_address").ToString.Trim) Then
                                htmlOut.Append("<td valign=""middle"" align=""left"">" + r.Item("contact_email_address").ToString.Trim + "</td>")
                            Else
                                htmlOut.Append("<td valign=""middle"" align=""left""></td>")
                            End If
                        Else
                            htmlOut.Append("<td valign=""middle"" align=""left""></td>")
                        End If

                        If Not IsDBNull(r.Item("sublogin_password")) Then
                            If Not String.IsNullOrEmpty(r.Item("sublogin_password").ToString.Trim) Then
                                htmlOut.Append("<td valign=""middle"" align=""left"">" + r.Item("sublogin_password").ToString.Trim + "</td>")
                            Else
                                htmlOut.Append("<td valign=""middle"" align=""left""></td>")
                            End If
                        Else
                            htmlOut.Append("<td valign=""middle"" align=""left""></td>")
                        End If

                        htmlOut.Append("<td valign=""middle"" align=""left"">")

                        If Not IsDBNull(r.Item("contact_email_address")) Then
                            If Not String.IsNullOrEmpty(r.Item("contact_email_address").ToString.Trim) Then
                                If r.Item("contact_email_address").ToString.ToLower.Contains("demo@jetnet.com") Then

                                    htmlOut.Append(r.Item("subins_platform_name").ToString.Trim)

                                End If
                            End If
                        End If

                        htmlOut.Append("</td>")

                        If Not IsDBNull(r.Item("subins_last_login_date")) Then
                            If Not String.IsNullOrEmpty(r.Item("subins_last_login_date").ToString.Trim) Then
                                htmlOut.Append("<td align=""left"" valign=""middle"">" + FormatDateTime(CDate(r.Item("subins_last_login_date").ToString.Trim), DateFormat.GeneralDate) + "</td>")
                            Else
                                htmlOut.Append("<td align=""left"" valign=""middle""></td>")
                            End If
                        Else
                            htmlOut.Append("<td align=""left"" valign=""middle""></td>")
                        End If

                        htmlOut.Append("</tr>")

                    Next

                    htmlOut.Append("</table>")

                Else
                    htmlOut.Append("<table id=""companyUsersDataTable"" width=""100%"" cellpadding=""2"" cellspacing=""0""><tr><td valign=""top"" align=""left""><br/>No Users Found</td></tr></table>")
                End If
            Else
                htmlOut.Append("<table id=""companyUsersDataTable"" width=""100%"" cellpadding=""2"" cellspacing=""0""><tr><td valign=""top"" align=""left""><br/>No Users Found</td></tr></table>")
            End If

        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />Error in displayAdminCompanyUsers(ByVal searchCriteria As onLineUsersSelectionCriteriaClass, ByRef out_tabTitle As String, ByRef out_htmlString As String)<br />" + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub

    Public Function getAdminCurrentUsersDataTable(ByVal searchCriteria As onLineUsersSelectionCriteriaClass) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            sQuery.Append("SELECT sub_comp_id, sub_frequency, sub_aerodex_flag, sub_busair_tier_level, sub_sale_price_flag, sub_serv_code,")
            sQuery.Append(" sublogin_sub_id, subins_contact_id, contact_last_name, sublogin_password, contact_first_name, contact_email_address,")
            sQuery.Append(" comp_name, comp_city, comp_state, sub_business_aircraft_flag, sub_helicopters_flag, sub_commerical_flag, sub_yacht_flag,")
            sQuery.Append(" subins_last_login_date, subins_last_session_date, subins_last_logout_date, subins_platform_name,")
            sQuery.Append(" subins_activex_flag, subins_local_db_flag, subins_evo_mobile_flag, sub_server_side_notes_flag, sub_cloud_notes_flag")

            sQuery.Append(", (SELECT TOP 1 subislog_host_name FROM Subscription_Install_Log WITH (NOLOCK) WHERE subislog_subid = sub_id")
            sQuery.Append(" AND subislog_login = subins_login AND subislog_seq_no = subins_seq_no AND subislog_msg_type LIKE 'UserLog%' ORDER BY subislog_id DESC) AS website_host_name")

            sQuery.Append(" FROM View_JETNET_Customers")
            sQuery.Append(" WHERE subins_last_session_date >= '" + DateAdd("n", -10, Now()).ToString.Trim + "'")
            sQuery.Append(" AND (subins_last_session_date <> subins_last_logout_date OR subins_last_logout_date IS NULL)")

            If Not String.IsNullOrEmpty(searchCriteria.OnLineCriteriaFrequency.Trim) Then
                sQuery.Append(Constants.cAndClause + "sub_frequency = '" + searchCriteria.OnLineCriteriaFrequency.ToUpper.Trim + "'")
            End If

            If Not String.IsNullOrEmpty(searchCriteria.OnLineCriteriaSelectedItem.Trim) Then

                Select Case (searchCriteria.OnLineCriteriaSelectedItem.ToUpper.Trim)
                    Case "BUS"
                        sQuery.Append(Constants.cAndClause + "sub_business_aircraft_flag = 'Y'")
                    Case "T1"
                        sQuery.Append(Constants.cAndClause + "sub_busair_tier_level = 1")
                    Case "T2"
                        sQuery.Append(Constants.cAndClause + "sub_busair_tier_level = 2")
                    Case "T3"
                        sQuery.Append(Constants.cAndClause + "sub_busair_tier_level > 2")
                    Case "HEL"
                        sQuery.Append(Constants.cAndClause + "sub_helicopters_flag = 'Y'")
                    Case "COMM"
                        sQuery.Append(Constants.cAndClause + "sub_commerical_flag = 'Y'")
                    Case "AERO"
                        sQuery.Append(Constants.cAndClause + "sub_aerodex_flag = 'Y'")
                    Case "SPI"
                        sQuery.Append(Constants.cAndClause + "sub_sale_price_flag = 'Y'")
                    Case "YACHT"
                        sQuery.Append(Constants.cAndClause + "sub_yacht_flag = 'Y'")
                    Case "CRM"
                        sQuery.Append(Constants.cAndClause + "sub_serv_code LIKE 'CRM%'")

                End Select

            End If

            If searchCriteria.OnLineCriteriaContactID > 0 Then
                sQuery.Append(Constants.cAndClause + "subins_contact_id = " + searchCriteria.OnLineCriteriaContactID.ToString)
            End If

            If searchCriteria.OnLineCriteriaCompanyID > 0 Then
                sQuery.Append(Constants.cAndClause + "sub_comp_id = " + searchCriteria.OnLineCriteriaCompanyID.ToString)
            End If

            If Not String.IsNullOrEmpty(searchCriteria.OnLineCriteriaSearchItem.Trim) Then
                sQuery.Append(Constants.cAndClause + "(comp_name LIKE '" + searchCriteria.OnLineCriteriaSearchItem.Trim + "%'")
                sQuery.Append(Constants.cOrClause + "contact_first_name LIKE '" + searchCriteria.OnLineCriteriaSearchItem.Trim + "%'")
                sQuery.Append(Constants.cOrClause + "contact_last_name LIKE '" + searchCriteria.OnLineCriteriaSearchItem.Trim + "%')")
            End If

            If Not String.IsNullOrEmpty(searchCriteria.OnLineCriteriaOrderBy.Trim) Then

                Select Case (searchCriteria.OnLineCriteriaOrderBy.ToUpper.Trim)
                    Case "COMP1"
                        sQuery.Append(" ORDER BY comp_name DESC, comp_city, comp_state, contact_last_name, contact_first_name")
                    Case "COMP2"
                        sQuery.Append(" ORDER BY comp_name ASC, comp_city, comp_state, contact_last_name, contact_first_name")
                    Case "NAME1"
                        sQuery.Append(" ORDER BY contact_last_name DESC, contact_first_name, comp_name, comp_city, comp_state")
                    Case "NAME2"
                        sQuery.Append(" ORDER BY contact_last_name ASC, contact_first_name, comp_name, comp_city, comp_state")

                End Select
            Else
                sQuery.Append(" ORDER BY contact_last_name ASC, contact_first_name, comp_name, comp_city, comp_state")
            End If

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>getAdminCurrentUsersDataTable(ByVal searchCriteria As onLineUsersSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

            SqlConn.ConnectionString = adminConnectStr

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
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getAdminCurrentUsersDataTable load datatable</b><br /> " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getAdminCurrentUsersDataTable(ByVal searchCriteria As onLineUsersSelectionCriteriaClass) As DataTable</b><br />" + ex.Message

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

    Public Sub displayAdminCurrentUsers(ByVal searchCriteria As onLineUsersSelectionCriteriaClass, ByRef out_tabTitle As String, ByRef out_htmlString As String)

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder
        Dim tmpHtmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False

        Dim customer_total As Long = 0
        Dim new_total_count As Long = 0
        Dim jetnet_count As Long = 0
        Dim demo_count As Long = 0

        Try

            out_tabTitle = "Users Logged In "

            If Not String.IsNullOrEmpty(searchCriteria.OnLineCriteriaFrequency.Trim) Then
                out_tabTitle += ": " + searchCriteria.OnLineCriteriaFrequency.ToUpper.Trim
            End If

            If Not String.IsNullOrEmpty(searchCriteria.OnLineCriteriaSelectedItem.Trim) Then

                Select Case (searchCriteria.OnLineCriteriaSelectedItem.ToUpper.Trim)
                    Case "BUS"
                        out_tabTitle += "/Business"
                    Case "T1"
                        out_tabTitle += "/Tier 1"
                    Case "T2"
                        out_tabTitle += "/Tier 2"
                    Case "T3"
                        out_tabTitle += "/Tier 3"
                    Case "HEL"
                        out_tabTitle += "/Helicopters"
                    Case "COMM"
                        out_tabTitle += "/Commercial"
                    Case "AERO"
                        out_tabTitle += "/Aerodex"
                    Case "SPI"
                        out_tabTitle += "/VALUES"
                    Case "YACHT"
                        out_tabTitle += "/Yacht"
                    Case "CRM"
                        out_tabTitle += "/MPM"

                End Select

            End If

            results_table = getAdminCurrentUsersDataTable(searchCriteria)

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    htmlOut.Append("<table id=""currentUsersDataTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"">")
                    htmlOut.Append("<tr bgcolor=""#D9D8D8""><td valign=""middle"" align=""left""><b>SUB&nbsp;ID</b></td>")

                    If searchCriteria.OnLineCriteriaOrderBy.ToUpper.Contains("NAME1") Then
                        htmlOut.Append("<td valign=""middle"" align=""left""><b><a class=""underline pointer"" href=""adminCurrentUsers.aspx?order=name2"" title=""Order by contact Last name"">USER NAME</a></td>")
                    Else
                        htmlOut.Append("<td valign=""middle"" align=""left""><b><a class=""underline pointer"" href=""adminCurrentUsers.aspx?order=name1"" title=""Order by contact Last name"">USER NAME</a></td>")
                    End If

                    htmlOut.Append("<td valign=""middle"" align=""left""><b>EMAIL ADDRESS</b></td>")
                    htmlOut.Append("<td valign=""middle"" align=""left""><b>PASSWORD</b></td>")

                    If searchCriteria.OnLineCriteriaOrderBy.ToUpper.Contains("COMP1") Then
                        htmlOut.Append("<td valign=""middle"" align=""left""><b><a class=""underline pointer"" href=""adminCurrentUsers.aspx?order=comp2"" title=""Order by company name"">COMPANY NAME</a></td>")
                    Else
                        htmlOut.Append("<td valign=""middle"" align=""left""><b><a class=""underline pointer"" href=""adminCurrentUsers.aspx?order=comp1"" title=""Order by company name"">COMPANY NAME</a></td>")
                    End If

                    htmlOut.Append("<td valign=""middle"" align=""left""><b>COMP&nbsp;ID</b></td>")
                    htmlOut.Append("<td valign=""middle"" align=""left""><b>LOGIN</b></td></tr>")

                    For Each r As DataRow In results_table.Rows

                        tmpHtmlOut = New StringBuilder

                        If Not toggleRowColor Then
                            tmpHtmlOut.Append("<tr class=""alt_row"">")
                            toggleRowColor = True
                        Else
                            tmpHtmlOut.Append("<tr bgcolor=""white"">")
                            toggleRowColor = False
                        End If

                        If Not IsDBNull(r.Item("sublogin_sub_id")) Then
                            If Not String.IsNullOrEmpty(r.Item("sublogin_sub_id").ToString.Trim) Then
                                tmpHtmlOut.Append("<td valign=""middle"" align=""left"">" + r.Item("sublogin_sub_id").ToString.Trim + "</td>")
                            Else
                                tmpHtmlOut.Append("<td valign=""middle"" align=""left""></td>")
                            End If
                        Else
                            tmpHtmlOut.Append("<td valign=""middle"" align=""left""></td>")
                        End If

                        tmpHtmlOut.Append("<td valign=""middle"" align=""left"" wrap=""nowrap"">")
                        tmpHtmlOut.Append("<a class=""underline pointer"" href=""adminCurrentUsers.aspx?user_id=" + r.Item("subins_contact_id").ToString.Trim + "&id=" + r.Item("sub_comp_id").ToString.Trim + """>" + r.Item("contact_last_name").ToString.Trim + ", " + r.Item("contact_first_name").ToString.Trim + "</a>")
                        tmpHtmlOut.Append("</td>")

                        If Not IsDBNull(r.Item("contact_email_address")) Then
                            If Not String.IsNullOrEmpty(r.Item("contact_email_address").ToString.Trim) Then
                                tmpHtmlOut.Append("<td valign=""middle"" align=""left"">" + r.Item("contact_email_address").ToString.Trim + "</td>")
                            Else
                                tmpHtmlOut.Append("<td valign=""middle"" align=""left""></td>")
                            End If
                        Else
                            tmpHtmlOut.Append("<td valign=""middle"" align=""left""></td>")
                        End If

                        If Not IsDBNull(r.Item("sublogin_password")) Then
                            If Not String.IsNullOrEmpty(r.Item("sublogin_password").ToString.Trim) Then
                                tmpHtmlOut.Append("<td valign=""middle"" align=""left"">" + r.Item("sublogin_password").ToString.Trim + "</td>")
                            Else
                                tmpHtmlOut.Append("<td valign=""middle"" align=""left""></td>")
                            End If
                        Else
                            tmpHtmlOut.Append("<td valign=""middle"" align=""left""></td>")
                        End If

                        tmpHtmlOut.Append("<td valign=""middle"" align=""left"">")
                        tmpHtmlOut.Append("<a class=""underline pointer"" href=""adminCurrentUsers.aspx?id=" + r.Item("sub_comp_id").ToString.Trim + """>")

                        If Not IsDBNull(r.Item("comp_name")) Then
                            If Not String.IsNullOrEmpty(r.Item("comp_name").ToString.Trim) Then
                                tmpHtmlOut.Append(r.Item("comp_name").ToString.Trim)
                            End If
                        End If

                        tmpHtmlOut.Append(" (")

                        If Not IsDBNull(r.Item("comp_city")) Then
                            If Not String.IsNullOrEmpty(r.Item("comp_city").ToString.Trim) Then
                                tmpHtmlOut.Append(r.Item("comp_city").ToString.Trim)
                            End If
                        End If

                        If Not IsDBNull(r.Item("comp_state")) Then
                            If Not String.IsNullOrEmpty(r.Item("comp_state").ToString.Trim) Then
                                tmpHtmlOut.Append(", " + r.Item("comp_state").ToString.Trim)
                            End If
                        End If

                        tmpHtmlOut.Append(")")

                        If Not IsDBNull(r.Item("contact_email_address")) Then
                            If Not String.IsNullOrEmpty(r.Item("contact_email_address").ToString.Trim) Then
                                If r.Item("contact_email_address").ToString.ToLower.Contains("demo@jetnet.com") Then

                                    tmpHtmlOut.Append(" - " + r.Item("subins_platform_name").ToString.Trim)

                                End If
                            End If
                        End If

                        tmpHtmlOut.Append("</a></td>")

                        tmpHtmlOut.Append("<td valign=""middle"" align=""left"">")
                        tmpHtmlOut.Append("<a class=""underline pointer"" href=""DisplayCompanyDetail.aspx?compid=" + r.Item("sub_comp_id").ToString.Trim + """ target=""_new"" title=""Click to view company details"">" + r.Item("sub_comp_id").ToString.Trim + "</a>")
                        tmpHtmlOut.Append("</td>")

                        If Not IsDBNull(r.Item("website_host_name")) Then
                            If Not String.IsNullOrEmpty(r.Item("website_host_name").ToString.Trim) Then
                                tmpHtmlOut.Append("<td align=""left"" valign=""middle"">" + r.Item("website_host_name").ToString.ToUpper.Replace(".COM", "").Trim + "</td>")
                            Else
                                tmpHtmlOut.Append("<td align=""left"" valign=""middle"">UNKNOWN</td>")
                            End If
                        Else
                            tmpHtmlOut.Append("<td align=""left"" valign=""middle"">UNKNOWN</td>")
                        End If

                        tmpHtmlOut.Append("</tr>")

                        If Not String.IsNullOrEmpty(searchCriteria.OnLineCriteriaServer.Trim) Then

                            If Not IsDBNull(r.Item("website_host_name")) Then

                                Dim temp_web_name = r.Item("website_host_name").ToString.ToUpper.Replace(".COM", "").Trim

                                If ((temp_web_name.ToUpper.Contains(searchCriteria.OnLineCriteriaServer.ToUpper)) Or (String.IsNullOrEmpty(temp_web_name) And searchCriteria.OnLineCriteriaServer.Trim.ToUpper.Contains("JETNETEVOLUTION"))) Then
                                    htmlOut.Append(tmpHtmlOut.ToString)
                                End If

                            Else

                                'if its null and we are loooking for JETNETEVOLUTION.com then add it 
                                If searchCriteria.OnLineCriteriaServer.Trim.ToUpper.Contains("JETNETEVOLUTION") Then
                                    htmlOut.Append(tmpHtmlOut.ToString)
                                End If

                            End If

                        Else
                            htmlOut.Append(tmpHtmlOut.ToString)
                        End If

                    Next

                    htmlOut.Append("</table>")

                    If results_table.Rows.Count > 1 Then

                        For Each r As DataRow In results_table.Rows

                            If Not String.IsNullOrEmpty(searchCriteria.OnLineCriteriaServer.Trim) Then

                                If Not IsDBNull(r.Item("website_host_name")) Then

                                    Dim temp_web_name = r.Item("website_host_name").ToString.ToUpper.Replace(".COM", "").Trim

                                    If ((temp_web_name.ToUpper.Contains(searchCriteria.OnLineCriteriaServer.ToUpper)) Or (String.IsNullOrEmpty(temp_web_name) And searchCriteria.OnLineCriteriaServer.Trim.ToUpper.Contains("JETNETEVOLUTION"))) Then

                                        If Not IsDBNull(r.Item("contact_email_address")) Then
                                            If Not String.IsNullOrEmpty(r.Item("contact_email_address").ToString.Trim) Then

                                                If r.Item("contact_email_address").ToString.ToLower.Contains("demo@jetnet.com") Then
                                                    demo_count += 1
                                                ElseIf r.Item("contact_email_address").ToString.ToLower.Contains("@jetnet.com") Then
                                                    jetnet_count += 1
                                                End If

                                            End If
                                        End If

                                        new_total_count += 1

                                    End If

                                Else

                                    ' if its null and we are loooking for jetnetevo.com then add it 
                                    If searchCriteria.OnLineCriteriaServer.Trim.ToUpper.Contains("JETNETEVOLUTION") Then

                                        If Not IsDBNull(r.Item("contact_email_address")) Then
                                            If Not String.IsNullOrEmpty(r.Item("contact_email_address").ToString.Trim) Then

                                                If r.Item("contact_email_address").ToString.ToLower.Contains("demo@jetnet.com") Then
                                                    demo_count += 1
                                                ElseIf r.Item("contact_email_address").ToString.ToLower.Contains("@jetnet.com") Then
                                                    jetnet_count += 1
                                                End If

                                            End If
                                        End If

                                        new_total_count += 1

                                    End If

                                End If
                            End If

                        Next

                        customer_total = (new_total_count - jetnet_count - demo_count)

                        If customer_total > 0 Then
                            htmlOut.Append("<br/><br />")
                            htmlOut.Append("<table id=""currentUsersTotalDataTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"">")
                            htmlOut.Append("<tr><td align=""left"" valign=""middle"">TOTAL USERS</td><td valign=""middle"" align=""right"">" + new_total_count.ToString + "</td></tr>")
                            htmlOut.Append("<tr><td align=""left"" valign=""middle"">CUSTOMERS</td><td valign=""middle"" align=""right"">" + customer_total.ToString + "</td></tr>")
                            htmlOut.Append("<tr><td align=""left"" valign=""middle"">JETNET EMPLOYEES</td><td valign=""middle"" align=""right"">" + jetnet_count.ToString + "</td></tr>")
                            htmlOut.Append("<tr><td align=""left"" valign=""middle"">DEMO USERS</td><td valign=""middle"" align=""right"">" + demo_count.ToString + "</td></tr>")
                            htmlOut.Append("</table>")
                        End If

                    End If

                Else
                    htmlOut.Append("<table id=""currentUsersDataTable"" width=""100%"" cellpadding=""2"" cellspacing=""0""><tr><td valign=""top"" align=""left""><br/>No OnLine Users Found</td></tr></table>")
                End If
            Else
                htmlOut.Append("<table id=""currentUsersDataTable"" width=""100%"" cellpadding=""2"" cellspacing=""0""><tr><td valign=""top"" align=""left""><br/>No OnLine Users Found</td></tr></table>")
            End If

        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />Error in displayAdminCurrentUsers(ByVal searchCriteria As onLineUsersSelectionCriteriaClass, ByRef out_tabTitle As String, ByRef out_htmlString As String)<br />" + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub

    Public Function getAdminUsersByBrowserDataTable(ByVal searchCriteria As onLineUsersSelectionCriteriaClass) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            If searchCriteria.OnLineCriteriaPlatformType.ToUpper.Contains("B") Then
                sQuery.Append("SELECT substring(subins_platform_os,8,25) AS platform, count(*) AS tcount,")
            Else
                sQuery.Append("SELECT substring(subins_platform_os,1,7) AS platform, count(*) AS tcount,")
            End If

            sQuery.Append(" sublogin_sub_id, contact_last_name, contact_first_name, contact_email_address, sublogin_password, sub_comp_id, comp_name, comp_city, comp_state,")
            sQuery.Append(" contact_email_address, subins_platform_name, sub_comp_id, subins_last_login_date, subins_last_session_date, subins_last_logout_date")

            sQuery.Append(" FROM Subscription WITH(NOLOCK)")
            sQuery.Append(" INNER JOIN Subscription_Login WITH(NOLOCK) ON (sublogin_sub_id = sub_id)")
            sQuery.Append(" INNER JOIN Subscription_Install WITH(NOLOCK) ON (subins_sub_id = sub_id) AND sublogin_login = subins_login")
            sQuery.Append(" INNER JOIN Company WITH(NOLOCK) ON (comp_id = sub_comp_id) AND comp_journ_id = 0")
            sQuery.Append(" INNER JOIN Contact WITH(NOLOCK) ON (contact_comp_id = sub_comp_id) AND subins_contact_id = contact_id AND contact_journ_id = 0")
            sQuery.Append(" WHERE subins_last_session_date >= '" + DateAdd("n", -10, Now()).ToString.Trim + "'")
            sQuery.Append(" AND (subins_last_session_date <> subins_last_logout_date OR subins_last_logout_date IS NULL)")

            If Not String.IsNullOrEmpty(searchCriteria.OnLineCriteriaSearchItem.Trim) Then
                sQuery.Append(Constants.cAndClause + "(comp_name LIKE '" + searchCriteria.OnLineCriteriaSearchItem.Trim + "%'")
                sQuery.Append(Constants.cOrClause + "contact_first_name LIKE '" + searchCriteria.OnLineCriteriaSearchItem.Trim + "%'")
                sQuery.Append(Constants.cOrClause + "contact_last_name LIKE '" + searchCriteria.OnLineCriteriaSearchItem.Trim + "%')")
            End If

            If searchCriteria.OnLineCriteriaPlatformType.ToUpper.Contains("B") Then
                sQuery.Append(Constants.cAndClause + "lower(subins_platform_os) LIKE '%" + searchCriteria.OnLineCriteriaInfo.ToLower.Trim + "'")
                sQuery.Append(" GROUP BY substring(subins_platform_os,8,25),")
            Else
                Dim sPad As Char = Constants.cSingleSpace
                sQuery.Append(Constants.cAndClause + "lower(subins_platform_os) LIKE '" + searchCriteria.OnLineCriteriaInfo.ToLower.Trim.PadRight(7, sPad) + "%'")
                sQuery.Append(" GROUP BY substring(subins_platform_os,1,7),")
            End If

            sQuery.Append(" sublogin_sub_id, contact_last_name, contact_first_name, contact_email_address, sublogin_password, sub_comp_id, comp_name, comp_city, comp_state,")
            sQuery.Append(" contact_email_address, subins_platform_name, sub_comp_id, subins_last_login_date, subins_last_session_date, subins_last_logout_date")

            sQuery.Append(" ORDER BY platform")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>getAdminUsersByBrowserDataTable(ByVal searchCriteria As onLineUsersSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

            SqlConn.ConnectionString = adminConnectStr

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
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getAdminUsersByBrowserDataTable load datatable</b><br /> " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getAdminUsersByBrowserDataTable(ByVal searchCriteria As onLineUsersSelectionCriteriaClass) As DataTable</b><br />" + ex.Message

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

    Public Sub displayAdminUsersByBrowser(ByVal searchCriteria As onLineUsersSelectionCriteriaClass, ByRef out_tabTitle As String, ByRef out_htmlString As String)

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False

        Try

            out_tabTitle = "Users Logged In"

            results_table = getAdminUsersByBrowserDataTable(searchCriteria)

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    htmlOut.Append("<table id=""usersByBrowserDataTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"">")
                    htmlOut.Append("<tr bgcolor=""#D9D8D8"">")

                    htmlOut.Append("<td valign=""middle"" align=""left""><b>SUB&nbsp;ID</b></td>")

                    htmlOut.Append("<td valign=""middle"" align=""left""><b>USER NAME</td>")

                    htmlOut.Append("<td valign=""middle"" align=""left""><b>EMAIL ADDRESS</b></td>")
                    htmlOut.Append("<td valign=""middle"" align=""left""><b>PASSWORD</b></td>")

                    htmlOut.Append("<td valign=""middle"" align=""left""><b>COMPANY NAME</td>")

                    htmlOut.Append("<td valign=""middle"" align=""left""><b>COMP&nbsp;ID</b></td>")
                    htmlOut.Append("<td valign=""middle"" align=""left""><b>LOGIN</b></td>")
                    htmlOut.Append("<td valign=""middle"" align=""left""><b>SESSION</b></td>")

                    If searchCriteria.OnLineCriteriaPlatformType.ToUpper.Contains("B") Then
                        htmlOut.Append("<td valign=""middle"" align=""left""><b>BROWSER</b></td></tr>")
                    Else
                        htmlOut.Append("<td valign=""middle"" align=""left""><b>PLATFORM</b></td></tr>")
                    End If

                    For Each r As DataRow In results_table.Rows

                        If Not toggleRowColor Then
                            htmlOut.Append("<tr class=""alt_row"">")
                            toggleRowColor = True
                        Else
                            htmlOut.Append("<tr bgcolor=""white"">")
                            toggleRowColor = False
                        End If

                        If Not IsDBNull(r.Item("sublogin_sub_id")) Then
                            If Not String.IsNullOrEmpty(r.Item("sublogin_sub_id").ToString.Trim) Then
                                htmlOut.Append("<td valign=""middle"" align=""left"">" + r.Item("sublogin_sub_id").ToString.Trim + "</td>")
                            Else
                                htmlOut.Append("<td valign=""middle"" align=""left""></td>")
                            End If
                        Else
                            htmlOut.Append("<td valign=""middle"" align=""left""></td>")
                        End If

                        htmlOut.Append("<td valign=""middle"" align=""left"" wrap=""nowrap"">")
                        htmlOut.Append(r.Item("contact_last_name").ToString.Trim + ", " + r.Item("contact_first_name").ToString.Trim)
                        htmlOut.Append("</td>")

                        If Not IsDBNull(r.Item("contact_email_address")) Then
                            If Not String.IsNullOrEmpty(r.Item("contact_email_address").ToString.Trim) Then
                                htmlOut.Append("<td valign=""middle"" align=""left"">" + r.Item("contact_email_address").ToString.Trim + "</td>")
                            Else
                                htmlOut.Append("<td valign=""middle"" align=""left""></td>")
                            End If
                        Else
                            htmlOut.Append("<td valign=""middle"" align=""left""></td>")
                        End If

                        If Not IsDBNull(r.Item("sublogin_password")) Then
                            If Not String.IsNullOrEmpty(r.Item("sublogin_password").ToString.Trim) Then
                                htmlOut.Append("<td valign=""middle"" align=""left"">" + r.Item("sublogin_password").ToString.Trim + "</td>")
                            Else
                                htmlOut.Append("<td valign=""middle"" align=""left""></td>")
                            End If
                        Else
                            htmlOut.Append("<td valign=""middle"" align=""left""></td>")
                        End If

                        htmlOut.Append("<td valign=""middle"" align=""left"">")
                        htmlOut.Append("<a class=""underline pointer"" href=""adminCurrentUsers.aspx?id=" + r.Item("sub_comp_id").ToString.Trim + """>")

                        If Not IsDBNull(r.Item("comp_name")) Then
                            If Not String.IsNullOrEmpty(r.Item("comp_name").ToString.Trim) Then
                                htmlOut.Append(r.Item("comp_name").ToString.Trim)
                            End If
                        End If

                        htmlOut.Append(" (")

                        If Not IsDBNull(r.Item("comp_city")) Then
                            If Not String.IsNullOrEmpty(r.Item("comp_city").ToString.Trim) Then
                                htmlOut.Append(r.Item("comp_city").ToString.Trim)
                            End If
                        End If

                        If Not IsDBNull(r.Item("comp_state")) Then
                            If Not String.IsNullOrEmpty(r.Item("comp_state").ToString.Trim) Then
                                htmlOut.Append(", " + r.Item("comp_state").ToString.Trim)
                            End If
                        End If

                        htmlOut.Append(")")

                        If Not IsDBNull(r.Item("contact_email_address")) Then
                            If Not String.IsNullOrEmpty(r.Item("contact_email_address").ToString.Trim) Then
                                If r.Item("contact_email_address").ToString.ToLower.Contains("demo@jetnet.com") Then

                                    htmlOut.Append(" - " + r.Item("subins_platform_name").ToString.Trim)

                                End If
                            End If
                        End If

                        htmlOut.Append("</a></td>")

                        If Not IsDBNull(r.Item("sub_comp_id")) Then
                            If Not String.IsNullOrEmpty(r.Item("sub_comp_id").ToString.Trim) Then
                                htmlOut.Append("<td valign=""middle"" align=""left"">" + r.Item("sub_comp_id").ToString.Trim + "</td>")
                            Else
                                htmlOut.Append("<td valign=""middle"" align=""left""></td>")
                            End If
                        Else
                            htmlOut.Append("<td valign=""middle"" align=""left""></td>")
                        End If

                        If Not IsDBNull(r.Item("subins_last_login_date")) Then
                            If Not String.IsNullOrEmpty(r.Item("subins_last_login_date").ToString.Trim) Then
                                htmlOut.Append("<td align=""left"" valign=""middle"">" + FormatDateTime(CDate(r.Item("subins_last_login_date").ToString.Trim), DateFormat.GeneralDate) + "</td>")
                            Else
                                htmlOut.Append("<td align=""left"" valign=""middle""></td>")
                            End If
                        Else
                            htmlOut.Append("<td align=""left"" valign=""middle""></td>")
                        End If

                        If Not IsDBNull(r.Item("subins_last_session_date")) Then
                            If Not String.IsNullOrEmpty(r.Item("subins_last_session_date").ToString.Trim) Then
                                htmlOut.Append("<td align=""left"" valign=""middle"">" + FormatDateTime(CDate(r.Item("subins_last_session_date").ToString.Trim), DateFormat.GeneralDate) + "</td>")
                            Else
                                htmlOut.Append("<td align=""left"" valign=""middle""></td>")
                            End If
                        Else
                            htmlOut.Append("<td align=""left"" valign=""middle""></td>")
                        End If

                        If Not IsDBNull(r.Item("platform")) Then
                            If Not String.IsNullOrEmpty(r.Item("platform").ToString.Trim) Then
                                htmlOut.Append("<td valign=""middle"" align=""left"">" + r.Item("platform").ToString.Trim + "</td>")
                            Else
                                htmlOut.Append("<td valign=""middle"" align=""left""></td>")
                            End If
                        Else
                            htmlOut.Append("<td valign=""middle"" align=""left""></td>")
                        End If

                        htmlOut.Append("</tr>")

                    Next

                    htmlOut.Append("</table>")

                    Dim customer_total As Long = 0
                    Dim new_total_count As Long = 0
                    Dim jetnet_count As Long = 0
                    Dim demo_count As Long = 0

                    If results_table.Rows.Count > 1 Then

                        For Each r As DataRow In results_table.Rows

                            If Not IsDBNull(r.Item("contact_email_address")) Then
                                If Not String.IsNullOrEmpty(r.Item("contact_email_address").ToString.Trim) Then

                                    If r.Item("contact_email_address").ToString.ToLower.Contains("demo@jetnet.com") Then
                                        demo_count += 1
                                    ElseIf r.Item("contact_email_address").ToString.ToLower.Contains("@jetnet.com") Then
                                        jetnet_count += 1
                                    End If

                                End If
                            End If

                            new_total_count += 1

                        Next

                        customer_total = (new_total_count - jetnet_count - demo_count)

                        If customer_total > 0 Then
                            htmlOut.Append("<br/><br />")
                            htmlOut.Append("<table id=""currentUsersTotalDataTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"">")
                            htmlOut.Append("<tr><td align=""left"" valign=""middle"">TOTAL USERS</td><td valign=""middle"" align=""right"">" + new_total_count.ToString + "</td></tr>")
                            htmlOut.Append("<tr><td align=""left"" valign=""middle"">CUSTOMERS</td><td valign=""middle"" align=""right"">" + customer_total.ToString + "</td></tr>")
                            htmlOut.Append("<tr><td align=""left"" valign=""middle"">JETNET EMPLOYEES</td><td valign=""middle"" align=""right"">" + jetnet_count.ToString + "</td></tr>")
                            htmlOut.Append("<tr><td align=""left"" valign=""middle"">DEMO USERS</td><td valign=""middle"" align=""right"">" + demo_count.ToString + "</td></tr>")
                            htmlOut.Append("</table>")
                        End If

                    End If

                Else
                    htmlOut.Append("<table id=""usersByBrowserDataTable"" width=""100%"" cellpadding=""2"" cellspacing=""0""><tr><td valign=""top"" align=""left""><br/>No Users Found</td></tr></table>")
                End If
            Else
                htmlOut.Append("<table id=""usersByBrowserDataTable"" width=""100%"" cellpadding=""2"" cellspacing=""0""><tr><td valign=""top"" align=""left""><br/>No Users Found</td></tr></table>")
            End If

        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />Error in displayAdminUsersByBrowser(ByVal searchCriteria As onLineUsersSelectionCriteriaClass, ByRef out_tabTitle As String, ByRef out_htmlString As String)<br />" + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub

    Public Function getAdminUserLogDataTable(ByVal searchCriteria As onLineUsersSelectionCriteriaClass, ByRef in_SelectedTxt As String) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            sQuery.Append("SELECT distinct Subscription_Install_Log.*,")
            sQuery.Append(" comp_name, comp_city,comp_state, comp_id, amod_make_name, amod_model_name, amod_id, ac_ser_no_full, ac_id,")
            sQuery.Append(" evoview_title, evoview_id, contact_first_name, contact_last_name, contact_id")
            sQuery.Append(" FROM Subscription_Install_Log")
            sQuery.Append(" INNER JOIN Subscription WITH(NOLOCK) ON sub_id = subislog_subid")
            sQuery.Append(" INNER JOIN Subscription_Login WITH(NOLOCK) ON sublogin_sub_id = sub_id")
            sQuery.Append(" INNER JOIN Subscription_Install WITH(NOLOCK) ON (subins_sub_id = sub_id AND sublogin_login = subins_login)")
            sQuery.Append(" LEFT OUTER JOIN Company ON subislog_comp_id = comp_id AND comp_journ_id = 0")
            sQuery.Append(" LEFT OUTER JOIN Contact ON (subislog_contact_id = contact_id AND contact_journ_id = 0)")
            sQuery.Append(" LEFT OUTER JOIN aircraft_model ON subislog_amod_id = amod_id")
            sQuery.Append(" LEFT OUTER JOIN aircraft ON (subislog_ac_id = ac_id AND ac_journ_id = 0)")
            sQuery.Append(" LEFT OUTER JOIN Evolution_Views ON (subislog_view_id = evoview_id AND evoview_id > 0)")

            If searchCriteria.OnLineCriteriaContactID > 0 And searchCriteria.OnLineCriteriaCompanyID > 0 Then
                sQuery.Append(" WHERE sub_comp_id = " + searchCriteria.OnLineCriteriaCompanyID.ToString)
                sQuery.Append(Constants.cAndClause + "subislog_contact_id = " + searchCriteria.OnLineCriteriaContactID.ToString)
            ElseIf searchCriteria.OnLineCriteriaContactID > 0 Then
                sQuery.Append(" WHERE subislog_contact_id = " + searchCriteria.OnLineCriteriaContactID.ToString)
            ElseIf searchCriteria.OnLineCriteriaCompanyID > 0 Then
                sQuery.Append(" WHERE sub_comp_id = " + searchCriteria.OnLineCriteriaCompanyID.ToString)
            End If

            If Not String.IsNullOrEmpty(searchCriteria.OnLineCriteriaSearchItem.Trim) Then
                sQuery.Append(Constants.cAndClause + "(subislog_email_address LIKE '" + searchCriteria.OnLineCriteriaSearchItem.Trim + "%'")
                sQuery.Append(Constants.cOrClause + "comp_name LIKE '" + searchCriteria.OnLineCriteriaSearchItem.Trim + "%')")
            End If

            If searchCriteria.OnLineCriteriaNumberToShow > 0 Then
                sQuery.Append(Constants.cAndClause + " subislog_date >= '" + DateAdd("d", (-1 * searchCriteria.OnLineCriteriaNumberToShow), Now()).ToString + "'")
            Else
                sQuery.Append(Constants.cAndClause + "subislog_date >= '" + Now().ToShortDateString.Trim + "'")
            End If

            If Not String.IsNullOrEmpty(in_SelectedTxt.Trim) Then
                sQuery.Append(Constants.cAndClause + "subislog_msg_type = '" + in_SelectedTxt.Trim + "%'")
            End If

            sQuery.Append(" ORDER BY subislog_date DESC")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>getAdminUserLogDataTable(ByVal searchCriteria As onLineUsersSelectionCriteriaClass, ByRef in_SelectedTxt As String) As DataTable</b><br />" + sQuery.ToString

            SqlConn.ConnectionString = adminConnectStr

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
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getAdminUserLogDataTable load datatable</b><br /> " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getAdminUserLogDataTable(ByVal searchCriteria As onLineUsersSelectionCriteriaClass, ByRef in_SelectedTxt As String) As DataTable</b><br />" + ex.Message

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

    Public Sub displayAdminUserLog(ByVal searchCriteria As onLineUsersSelectionCriteriaClass, ByRef in_SelectedTxt As String, ByRef out_htmlString As String)

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False
        Dim foreColor As String = ""
        Dim tmpMessage As String = ""
        Dim nCount As Integer = 0

        Try

            results_table = getAdminUserLogDataTable(searchCriteria, in_SelectedTxt)

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    htmlOut.Append("<table id=""currentUserLogDataTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"">")
                    htmlOut.Append("<tr bgcolor=""#D9D8D8"">")

                    htmlOut.Append("<td valign=""middle"" align=""left""><b>TIME</b></td>")
                    htmlOut.Append("<td valign=""middle"" align=""left""><b>EMAIL</b></td>")
                    htmlOut.Append("<td valign=""middle"" align=""left""><b>TYPE</b></td>")
                    htmlOut.Append("<td valign=""middle"" align=""left""><b>ACTION</b></td>")
                    htmlOut.Append("<td valign=""middle"" align=""left""><b>SITE</b></td>")

                    htmlOut.Append("</tr>")

                    For Each r As DataRow In results_table.Rows

                        If Not toggleRowColor Then
                            htmlOut.Append("<tr class=""alt_row"">")
                            toggleRowColor = True
                        Else
                            htmlOut.Append("<tr bgcolor=""white"">")
                            toggleRowColor = False
                        End If

                        If Not IsDBNull(r.Item("subislog_email_address")) Then
                            If Not String.IsNullOrEmpty(r.Item("subislog_email_address").ToString.Trim) Then

                                If Not IsDBNull(r("subislog_msg_type")) Then
                                    If Not String.IsNullOrEmpty(r.Item("subislog_msg_type").ToString.Trim) Then
                                        foreColor = IIf(r.Item("subislog_msg_type").ToString.ToLower.Trim.Contains("usererror"), " style=""color: red;""", "")
                                    End If
                                End If

                                If Not IsDBNull(r.Item("subislog_date")) Then
                                    If Not String.IsNullOrEmpty(r.Item("subislog_date").ToString.Trim) Then
                                        htmlOut.Append("<td align=""left"" valign=""middle""" + foreColor.Trim + ">" + FormatDateTime(CDate(r.Item("subislog_date").ToString.Trim), DateFormat.GeneralDate) + "</td>")
                                    Else
                                        htmlOut.Append("<td align=""left"" valign=""middle""></td>")
                                    End If
                                Else
                                    htmlOut.Append("<td align=""left"" valign=""middle""></td>")
                                End If

                                If Not IsDBNull(r.Item("subislog_email_address")) Then
                                    If Not String.IsNullOrEmpty(r.Item("subislog_email_address").ToString.Trim) Then
                                        htmlOut.Append("<td valign=""middle"" align=""left""" + foreColor.Trim + ">" + r.Item("subislog_email_address").ToString.Trim + "</td>")
                                    Else
                                        htmlOut.Append("<td valign=""middle"" align=""left""></td>")
                                    End If
                                Else
                                    htmlOut.Append("<td valign=""middle"" align=""left""></td>")
                                End If

                                If Not IsDBNull(r.Item("subislog_msg_type")) Then
                                    If Not String.IsNullOrEmpty(r.Item("subislog_msg_type").ToString.Trim) Then
                                        htmlOut.Append("<td valign=""middle"" align=""left""" + foreColor.Trim + ">" + r.Item("subislog_msg_type").ToString.Trim + "</td>")
                                    Else
                                        htmlOut.Append("<td valign=""middle"" align=""left""></td>")
                                    End If
                                Else
                                    htmlOut.Append("<td valign=""middle"" align=""left""></td>")
                                End If

                                htmlOut.Append("<td valign=""middle"" align=""left""" + foreColor.Trim + ">")

                                If Not IsDBNull(r.Item("subislog_message")) Then
                                    If Not String.IsNullOrEmpty(r.Item("subislog_message").ToString.Trim) Then

                                        If r.Item("subislog_message").ToString.ToLower.Trim.Contains("displayaircraftdetail") Then
                                            tmpMessage = "Aircraft:"

                                            If Not IsDBNull(r.Item("amod_make_name")) Then
                                                If Not String.IsNullOrEmpty(r.Item("amod_make_name").ToString.Trim) Then
                                                    tmpMessage += Constants.cSingleSpace + r.Item("amod_make_name").ToString.Trim
                                                End If
                                            End If

                                            If Not IsDBNull(r.Item("amod_model_name")) Then
                                                If Not String.IsNullOrEmpty(r.Item("amod_model_name").ToString.Trim) Then
                                                    tmpMessage += Constants.cSingleSpace + r.Item("amod_model_name").ToString.Trim
                                                End If
                                            End If

                                            If Not IsDBNull(r.Item("amod_id")) Then
                                                If Not String.IsNullOrEmpty(r.Item("amod_id").ToString.Trim) Then
                                                    tmpMessage += Constants.cSingleSpace + r.Item("amod_id").ToString.Trim
                                                End If
                                            End If

                                            If Not IsDBNull(r.Item("ac_ser_no_full")) Then
                                                If Not String.IsNullOrEmpty(r.Item("ac_ser_no_full").ToString.Trim) Then
                                                    tmpMessage += Constants.cSingleSpace + r.Item("ac_ser_no_full").ToString.Trim
                                                End If
                                            End If

                                            If Not IsDBNull(r.Item("ac_id")) Then
                                                If Not String.IsNullOrEmpty(r.Item("ac_id").ToString.Trim) Then
                                                    tmpMessage += Constants.cSingleSpace + r.Item("ac_id").ToString.Trim
                                                End If
                                            End If

                                        ElseIf r.Item("subislog_message").ToString.ToLower.Trim.Contains("displaycompanydetail") Then

                                            tmpMessage = "Company:"

                                            If Not IsDBNull(r.Item("comp_name")) Then
                                                If Not String.IsNullOrEmpty(r.Item("comp_name").ToString.Trim) Then
                                                    tmpMessage += Constants.cSingleSpace + r.Item("comp_name").ToString.Trim
                                                End If
                                            End If

                                            If Not IsDBNull(r.Item("comp_city")) Then
                                                If Not String.IsNullOrEmpty(r.Item("comp_city").ToString.Trim) Then
                                                    tmpMessage += Constants.cSingleSpace + r.Item("comp_city").ToString.Trim + IIf(Not IsDBNull(r.Item("comp_state")), Constants.cColonDelim, "")
                                                End If
                                            End If

                                            If Not IsDBNull(r.Item("comp_state")) Then
                                                If Not String.IsNullOrEmpty(r.Item("comp_state").ToString.Trim) Then
                                                    tmpMessage += Constants.cSingleSpace + r.Item("comp_state").ToString.Trim
                                                End If
                                            End If

                                            If Not IsDBNull(r.Item("comp_id")) Then
                                                If Not String.IsNullOrEmpty(r.Item("comp_id").ToString.Trim) Then
                                                    tmpMessage += Constants.cSingleSpace + r.Item("comp_id").ToString.Trim
                                                End If
                                            End If

                                        ElseIf r.Item("subislog_message").ToString.ToLower.Trim.Contains("displaycontactdetails") Then

                                            tmpMessage = "Company:"

                                            If Not IsDBNull(r.Item("comp_name")) Then
                                                If Not String.IsNullOrEmpty(r.Item("comp_name").ToString.Trim) Then
                                                    tmpMessage += Constants.cSingleSpace + r.Item("comp_name").ToString.Trim
                                                End If
                                            End If

                                            If Not IsDBNull(r.Item("comp_city")) Then
                                                If Not String.IsNullOrEmpty(r.Item("comp_city").ToString.Trim) Then
                                                    tmpMessage += Constants.cSingleSpace + r.Item("comp_city").ToString.Trim + IIf(Not IsDBNull(r.Item("comp_state")), Constants.cColonDelim, "")
                                                End If
                                            End If

                                            If Not IsDBNull(r.Item("comp_state")) Then
                                                If Not String.IsNullOrEmpty(r.Item("comp_state").ToString.Trim) Then
                                                    tmpMessage += Constants.cSingleSpace + r.Item("comp_state").ToString.Trim
                                                End If
                                            End If

                                            If Not IsDBNull(r.Item("comp_id")) Then
                                                If Not String.IsNullOrEmpty(r.Item("comp_id").ToString.Trim) Then
                                                    tmpMessage += Constants.cSingleSpace + r.Item("comp_id").ToString.Trim
                                                End If
                                            End If

                                            tmpMessage += " Contact:"

                                            If Not IsDBNull(r.Item("contact_first_name")) Then
                                                If Not String.IsNullOrEmpty(r.Item("contact_first_name").ToString.Trim) Then
                                                    tmpMessage += Constants.cSingleSpace + r.Item("contact_first_name").ToString.Trim
                                                End If
                                            End If

                                            If Not IsDBNull(r.Item("contact_last_name")) Then
                                                If Not String.IsNullOrEmpty(r.Item("contact_last_name").ToString.Trim) Then
                                                    tmpMessage += Constants.cSingleSpace + r.Item("contact_last_name").ToString.Trim
                                                End If
                                            End If

                                            If Not IsDBNull(r.Item("contact_id")) Then
                                                If Not String.IsNullOrEmpty(r.Item("contact_id").ToString.Trim) Then
                                                    tmpMessage += Constants.cSingleSpace + r.Item("contact_id").ToString.Trim
                                                End If
                                            End If

                                            htmlOut.Append(HttpContext.Current.Server.HtmlEncode(tmpMessage))

                                        Else

                                            nCount = 0
                                            For Each ch As Char In r.Item("subislog_message").ToString

                                                If Not Char.IsControl(ch) Then
                                                    If Char.IsWhiteSpace(ch) Then
                                                        htmlOut.Append(Constants.cSingleSpace)
                                                    Else
                                                        htmlOut.Append(ch)
                                                    End If
                                                End If

                                                nCount += 1

                                                If nCount Mod 50 = 0 Then
                                                    htmlOut.Append("<br/>")
                                                End If

                                            Next

                                        End If

                                    End If
                                End If

                                htmlOut.Append("</td>")

                                If Not IsDBNull(r.Item("subislog_host_name")) Then
                                    If Not String.IsNullOrEmpty(r.Item("subislog_host_name").ToString.Trim) Then
                                        htmlOut.Append("<td align=""left"" valign=""middle""" + foreColor.Trim + ">" + r.Item("subislog_host_name").ToString.ToUpper.Replace(".COM", "").Trim + "</td>")
                                    Else
                                        htmlOut.Append("<td align=""left"" valign=""middle""></td>")
                                    End If
                                Else
                                    htmlOut.Append("<td align=""left"" valign=""middle""></td>")
                                End If

                                htmlOut.Append("</tr>")

                            End If
                        End If

                    Next

                    htmlOut.Append("</table>")

                Else
                    htmlOut.Append("<table id=""currentUserLogDataTable"" width=""100%"" cellpadding=""2"" cellspacing=""0""><tr><td valign=""top"" align=""left""><br/>No User Log Items Found</td></tr></table>")
                End If
            Else
                htmlOut.Append("<table id=""currentUserLogDataTable"" width=""100%"" cellpadding=""2"" cellspacing=""0""><tr><td valign=""top"" align=""left""><br/>No User Log Items Found</td></tr></table>")
            End If

        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />Error in displayAdminUserLog(ByVal searchCriteria As onLineUsersSelectionCriteriaClass, ByRef in_SelectedTxt As String, ByRef out_htmlString As String)<br />" + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub

    Public Function getAdminCompanyByBusinessTypeDataTable(ByVal searchCriteria As onLineUsersSelectionCriteriaClass) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim sSeperator As String = ""

        Try

            sQuery.Append("SELECT DISTINCT comp_name, sub_comp_id, comp_city, comp_state, comp_country, COUNT(*) AS usercount")
            sQuery.Append(" FROM View_JETNET_Customers")
            sQuery.Append(" WHERE comp_journ_id = 0 AND comp_business_type = '" + searchCriteria.OnLineCriteriaBusType.Trim + "'")

            If Not String.IsNullOrEmpty(searchCriteria.OnLineCriteriaProductCode.Trim) Then

                sQuery.Append(" AND (")

                If searchCriteria.OnLineCriteriaProductCode.ToUpper.Trim.Contains("B") Then

                    sQuery.Append("sub_business_aircraft_flag = 'Y'")

                    sSeperator = Constants.cOrClause

                End If

                If searchCriteria.OnLineCriteriaProductCode.ToUpper.Trim.Contains("H") Then

                    sQuery.Append(sSeperator + "sub_helicopters_flag = 'Y'")

                    sSeperator = Constants.cOrClause

                End If

                If searchCriteria.OnLineCriteriaProductCode.ToUpper.Trim.Contains("C") Then

                    sQuery.Append(sSeperator + "sub_commerical_flag = 'Y'")

                    sSeperator = Constants.cOrClause

                End If

                If searchCriteria.OnLineCriteriaProductCode.ToUpper.Trim.Contains("Y") Then

                    sQuery.Append(sSeperator + "sub_yacht_flag = 'Y'")

                End If

                sQuery.Append(")")

            End If

            If Not String.IsNullOrEmpty(searchCriteria.OnLineCriteriaFrequency.Trim) Then
                sQuery.Append(Constants.cAndClause + "sub_frequency = '" + searchCriteria.OnLineCriteriaFrequency.ToUpper.Trim + "'")
            End If

            If Not String.IsNullOrEmpty(searchCriteria.OnLineCriteriaService.Trim) Then
                If searchCriteria.OnLineCriteriaService.ToUpper.Trim.Contains("A") Then
                    sQuery.Append(Constants.cAndClause + "sub_aerodex_flag = 'Y'")
                Else
                    sQuery.Append(Constants.cAndClause + "sub_aerodex_flag = 'N'")
                End If
            End If

            sQuery.Append(" GROUP BY comp_name, comp_country, comp_city, comp_state, sub_comp_id")
            sQuery.Append(" ORDER BY comp_name, comp_country, comp_city, comp_state ASC")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>getAdminCompanyByBusinessTypeDataTable(ByVal searchCriteria As onLineUsersSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

            SqlConn.ConnectionString = adminConnectStr

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
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getAdminCompanyByBusinessTypeDataTable load datatable</b><br /> " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getAdminCompanyByBusinessTypeDataTable(ByVal searchCriteria As onLineUsersSelectionCriteriaClass) As DataTable</b><br />" + ex.Message

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

    Public Sub displayAdminCompanyByBusinessType(ByVal searchCriteria As onLineUsersSelectionCriteriaClass, ByRef out_htmlString As String)

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False

        Try

            results_table = getAdminCompanyByBusinessTypeDataTable(searchCriteria)

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    htmlOut.Append("<table id=""currentUserLogDataTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"">")
                    htmlOut.Append("<tr bgcolor=""#D9D8D8"">")

                    If searchCriteria.OnLineCriteriaOrderBy.ToUpper.Contains("NAME1") Then
                        htmlOut.Append("<td valign=""middle"" align=""left""><b><a class=""underline pointer"" href=""adminCurrentUsers.aspx?bus_type=" + searchCriteria.OnLineCriteriaBusType.Trim + "&order=name2"" title=""Order by Company name"">COMPANY</a></td>")
                    Else
                        htmlOut.Append("<td valign=""middle"" align=""left""><b><a class=""underline pointer"" href=""adminCurrentUsers.aspx?bus_type=" + searchCriteria.OnLineCriteriaBusType.Trim + "&order=name1"" title=""Order by Company name"">COMPANY</a></td>")
                    End If

                    htmlOut.Append("<td valign=""middle"" align=""left""><b>COMP&nbsp;ID</b></td>")

                    If searchCriteria.OnLineCriteriaOrderBy.ToUpper.Contains("USERS1") Then
                        htmlOut.Append("<td valign=""middle"" align=""left""><b><a class=""underline pointer"" href=""adminCurrentUsers.aspx?bus_type=" + searchCriteria.OnLineCriteriaBusType.Trim + "&order=users2"" title=""Show users""># USERS</a></td>")
                    Else
                        htmlOut.Append("<td valign=""middle"" align=""left""><b><a class=""underline pointer"" href=""adminCurrentUsers.aspx?bus_type=" + searchCriteria.OnLineCriteriaBusType.Trim + "&order=users1"" title=""Show users""># USERS</a></td>")
                    End If

                    htmlOut.Append("</tr>")

                    For Each r As DataRow In results_table.Rows

                        If Not toggleRowColor Then
                            htmlOut.Append("<tr class=""alt_row"">")
                            toggleRowColor = True
                        Else
                            htmlOut.Append("<tr bgcolor=""white"">")
                            toggleRowColor = False
                        End If

                        htmlOut.Append("<td valign=""middle"" align=""left"">")
                        htmlOut.Append("<a class=""underline pointer"" href=""adminCurrentUsers.aspx?id=" + r.Item("sub_comp_id").ToString.Trim + """>")

                        If Not IsDBNull(r.Item("comp_name")) Then
                            If Not String.IsNullOrEmpty(r.Item("comp_name").ToString.Trim) Then
                                htmlOut.Append(r.Item("comp_name").ToString.Trim)
                            End If
                        End If

                        htmlOut.Append(" (")

                        If Not IsDBNull(r.Item("comp_city")) Then
                            If Not String.IsNullOrEmpty(r.Item("comp_city").ToString.Trim) Then
                                htmlOut.Append(r.Item("comp_city").ToString.Trim)
                            End If
                        End If

                        If Not IsDBNull(r.Item("comp_state")) Then
                            If Not String.IsNullOrEmpty(r.Item("comp_state").ToString.Trim) Then
                                htmlOut.Append(", " + r.Item("comp_state").ToString.Trim)
                            End If
                        End If

                        htmlOut.Append(")")
                        htmlOut.Append("</a></td>")

                        htmlOut.Append("<td valign=""middle"" align=""left"">")
                        htmlOut.Append("<a class=""underline pointer"" href=""DisplayCompanyDetail.aspx?compid=" + r.Item("sub_comp_id").ToString.Trim + """ target=""_new"" title=""Click to view company details"">" + r.Item("sub_comp_id").ToString.Trim + "</a>")
                        htmlOut.Append("</td>")

                        htmlOut.Append("<td valign=""middle"" align=""left"">")
                        htmlOut.Append("<a class=""underline pointer"" href=""adminCurrentUsers.aspx?id=" + r.Item("sub_comp_id").ToString.Trim + """>")
                        htmlOut.Append("</a>" + r.Item("usercount").ToString.Trim + "</td>")

                        htmlOut.Append("</tr>")

                    Next

                    htmlOut.Append("</table>")

                Else
                    htmlOut.Append("<table id=""currentUserLogDataTable"" width=""100%"" cellpadding=""2"" cellspacing=""0""><tr><td valign=""top"" align=""left""><br/>No User Log Items Found</td></tr></table>")
                End If
            Else
                htmlOut.Append("<table id=""currentUserLogDataTable"" width=""100%"" cellpadding=""2"" cellspacing=""0""><tr><td valign=""top"" align=""left""><br/>No User Log Items Found</td></tr></table>")
            End If

        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />Error in displayAdminCompanyByBusinessType(ByVal searchCriteria As onLineUsersSelectionCriteriaClass, ByRef out_htmlString As String)<br />" + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub

    Public Function getAdminCompanyLogDataTable(ByVal searchCriteria As onLineUsersSelectionCriteriaClass) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim sSeperator As String = ""

        Try

            sQuery.Append("SELECT DISTINCT comp_name, sub_comp_id, comp_city, comp_state, comp_country, COUNT(*) AS usercount")
            sQuery.Append(" FROM Subscription WITH(NOLOCK)")
            sQuery.Append(" INNER JOIN Subscription_Login WITH(NOLOCK) ON (sublogin_sub_id = sub_id)")
            sQuery.Append(" INNER JOIN Subscription_Install WITH(NOLOCK) ON (subins_sub_id = sub_id AND sublogin_login = subins_login)")
            sQuery.Append(" INNER JOIN Company WITH(NOLOCK) ON (comp_id = sub_comp_id AND comp_journ_id = 0)")
            sQuery.Append(" INNER JOIN Contact WITH(NOLOCK) ON (contact_comp_id = sub_comp_id AND subins_contact_id = contact_id AND contact_journ_id = 0)")
            sQuery.Append(" WHERE subins_last_session_date >= '" + DateAdd("n", -10, Now()).ToString.Trim + "'")
            sQuery.Append(" AND (subins_last_session_date <> subins_last_logout_date OR subins_last_logout_date IS NULL)")

            If searchCriteria.OnLineCriteriaCompanyID > 0 Then
                sQuery.Append(Constants.cAndClause + "sub_comp_id = " + searchCriteria.OnLineCriteriaCompanyID.ToString)
            End If

            If Not String.IsNullOrEmpty(searchCriteria.OnLineCriteriaSearchItem.Trim) Then
                sQuery.Append(Constants.cAndClause + "(comp_name LIKE '" + searchCriteria.OnLineCriteriaSearchItem.Trim + "%')")
            End If

            sQuery.Append(" GROUP BY comp_name, comp_country, comp_city, comp_state, sub_comp_id")

            If Not String.IsNullOrEmpty(searchCriteria.OnLineCriteriaOrderBy.Trim) Then

                Select Case (searchCriteria.OnLineCriteriaOrderBy.ToUpper.Trim)
                    Case "USERS1"
                        sQuery.Append(" ORDER BY usercount DESC")
                    Case "USERS2"
                        sQuery.Append(" ORDER BY usercount ASC")
                    Case "NAME1"
                        sQuery.Append(" ORDER BY comp_name, comp_city, comp_state, comp_id, comp_country DESC")
                    Case "NAME2"
                        sQuery.Append(" ORDER BY comp_name, comp_city, comp_state, comp_id, comp_country ASC")

                End Select
            Else
                sQuery.Append(" ORDER BY comp_name, comp_city, comp_state, comp_id, comp_country ASC")
            End If

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>getAdminCompanyLogDataTable(ByVal searchCriteria As onLineUsersSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

            SqlConn.ConnectionString = adminConnectStr

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
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getAdminCompanyLogDataTable load datatable</b><br /> " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getAdminCompanyLogDataTable(ByVal searchCriteria As onLineUsersSelectionCriteriaClass) As DataTable</b><br />" + ex.Message

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

    Public Function getAdminCompanyCountryLogDataTable() As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim sSeperator As String = ""

        Try

            sQuery.Append("SELECT comp_country, COUNT(*) AS usercount")
            sQuery.Append(" FROM Subscription WITH(NOLOCK)")
            sQuery.Append(" INNER JOIN Subscription_Install WITH(NOLOCK) ON (subins_sub_id = sub_id AND sublogin_login = subins_login)")
            sQuery.Append(" INNER JOIN Company WITH(NOLOCK) ON (comp_id = sub_comp_id AND comp_journ_id = 0)")
            sQuery.Append(" INNER JOIN Contact WITH(NOLOCK) ON (contact_comp_id = sub_comp_id AND subins_contact_id = contact_id AND contact_journ_id = 0)")
            sQuery.Append(" WHERE subins_last_session_date >= '" + DateAdd("n", -10, Now()).ToString.Trim + "'")
            sQuery.Append(" GROUP BY comp_country ORDER BY  comp_country")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>getAdminCompanyCountryLogDataTable() As DataTable</b><br />" + sQuery.ToString

            SqlConn.ConnectionString = adminConnectStr

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
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getAdminCompanyCountryLogDataTable load datatable</b><br /> " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getAdminCompanyCountryLogDataTable() As DataTable</b><br />" + ex.Message

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

    Public Sub displayAdminCompanyLog(ByVal searchCriteria As onLineUsersSelectionCriteriaClass, ByRef out_htmlString As String)

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False

        Dim user_counter As Long = 0
        Dim company_counter As Long = 0

        Try

            results_table = getAdminCompanyLogDataTable(searchCriteria)

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    htmlOut.Append("<table id=""currentCompanyLogDataTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"">")
                    htmlOut.Append("<tr bgcolor=""#D9D8D8"">")

                    If searchCriteria.OnLineCriteriaOrderBy.ToUpper.Contains("NAME1") Then
                        htmlOut.Append("<td valign=""middle"" align=""left""><b><a class=""underline pointer"" href=""adminCurrentUsers.aspx?order=name2"" title=""Order by Company name"">COMPANY</a></td>")
                    Else
                        htmlOut.Append("<td valign=""middle"" align=""left""><b><a class=""underline pointer"" href=""adminCurrentUsers.aspx?order=name1"" title=""Order by Company name"">COMPANY</a></td>")
                    End If

                    htmlOut.Append("<td valign=""middle"" align=""left""><b>COMP&nbsp;ID</b></td>")

                    If searchCriteria.OnLineCriteriaOrderBy.ToUpper.Contains("USERS1") Then
                        htmlOut.Append("<td valign=""middle"" align=""left""><b><a class=""underline pointer"" href=""adminCurrentUsers.aspx?order=users2"" title=""Show users""># USERS</a></td>")
                    Else
                        htmlOut.Append("<td valign=""middle"" align=""left""><b><a class=""underline pointer"" href=""adminCurrentUsers.aspx?order=users1"" title=""Show users""># USERS</a></td>")
                    End If

                    htmlOut.Append("</tr>")

                    For Each r As DataRow In results_table.Rows

                        If Not toggleRowColor Then
                            htmlOut.Append("<tr class=""alt_row"">")
                            toggleRowColor = True
                        Else
                            htmlOut.Append("<tr bgcolor=""white"">")
                            toggleRowColor = False
                        End If

                        htmlOut.Append("<td valign=""middle"" align=""left"">")
                        htmlOut.Append("<a class=""underline pointer"" href=""adminCurrentUsers.aspx?id=" + r.Item("sub_comp_id").ToString.Trim + """>")

                        If Not IsDBNull(r.Item("comp_name")) Then
                            If Not String.IsNullOrEmpty(r.Item("comp_name").ToString.Trim) Then
                                htmlOut.Append(r.Item("comp_name").ToString.Trim)
                            End If
                        End If

                        htmlOut.Append(" (")

                        If Not IsDBNull(r.Item("comp_city")) Then
                            If Not String.IsNullOrEmpty(r.Item("comp_city").ToString.Trim) Then
                                htmlOut.Append(r.Item("comp_city").ToString.Trim)
                            End If
                        End If

                        If Not IsDBNull(r.Item("comp_state")) Then
                            If Not String.IsNullOrEmpty(r.Item("comp_state").ToString.Trim) Then
                                htmlOut.Append(", " + r.Item("comp_state").ToString.Trim)
                            End If
                        End If

                        If Not IsDBNull(r.Item("comp_country")) Then
                            If Not String.IsNullOrEmpty(r.Item("comp_country").ToString.Trim) Then
                                htmlOut.Append(" - " + r.Item("comp_country").ToString.Trim)
                            End If
                        End If

                        htmlOut.Append(")")
                        htmlOut.Append("</a></td>")

                        htmlOut.Append("<td valign=""middle"" align=""left"">")
                        htmlOut.Append("<a class=""underline pointer"" href=""DisplayCompanyDetail.aspx?compid=" + r.Item("sub_comp_id").ToString.Trim + """ target=""_new"" title=""Click to view company details"">" + r.Item("sub_comp_id").ToString.Trim + "</a>")
                        htmlOut.Append("</td>")

                        htmlOut.Append("<td valign=""middle"" align=""left"">")
                        htmlOut.Append("<a class=""underline pointer"" href=""adminCurrentUsers.aspx?id=" + r.Item("sub_comp_id").ToString.Trim + """>")
                        htmlOut.Append("</a>" + r.Item("usercount").ToString.Trim + "</td>")

                        htmlOut.Append("</tr>")

                        user_counter += CLng(r.Item("usercount").ToString)
                        company_counter += 1

                    Next

                    htmlOut.Append("</table>")

                Else
                    htmlOut.Append("<table id=""currentCompanyLogDataTable"" width=""100%"" cellpadding=""2"" cellspacing=""0""><tr><td valign=""top"" align=""left""><br/>No Company Log Items Found</td></tr></table>")
                End If
            Else
                htmlOut.Append("<table id=""currentCompanyLogDataTable"" width=""100%"" cellpadding=""2"" cellspacing=""0""><tr><td valign=""top"" align=""left""><br/>No Company Log Items Found</td></tr></table>")
            End If


            results_table = Nothing

            results_table = getAdminCompanyCountryLogDataTable()

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    htmlOut.Append("<table id=""currentCompanyCountryDataTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"">")
                    htmlOut.Append("<tr bgcolor=""#D9D8D8"">")

                    htmlOut.Append("<td valign=""middle"" align=""left""><b>COUNTRY&nbsp;NAME</b></td>")
                    htmlOut.Append("<td valign=""middle"" align=""left""><b>USER&nbsp;COUNT</b></td>")

                    htmlOut.Append("</tr>")

                    For Each r As DataRow In results_table.Rows

                        If Not IsDBNull(r.Item("comp_country")) Then
                            If Not String.IsNullOrEmpty(r.Item("comp_country").ToString.Trim) Then

                                If Not toggleRowColor Then
                                    htmlOut.Append("<tr class=""alt_row"">")
                                    toggleRowColor = True
                                Else
                                    htmlOut.Append("<tr bgcolor=""white"">")
                                    toggleRowColor = False
                                End If

                                htmlOut.Append("<td valign=""middle"" align=""left"">" + r.Item("comp_country").ToString.Trim + "</td>")
                                htmlOut.Append("<td valign=""middle"" align=""right"">" + r.Item("usercount").ToString.Trim + "</td>")

                                htmlOut.Append("</tr>")

                            End If

                        End If

                    Next

                    htmlOut.Append("</table>")

                Else
                    htmlOut.Append("<table id=""currentCompanyCountryDataTable"" width=""100%"" cellpadding=""2"" cellspacing=""0""><tr><td valign=""top"" align=""left""><br/>No Company Log Items Found</td></tr></table>")
                End If
            Else
                htmlOut.Append("<table id=""currentCompanyCountryDataTable"" width=""100%"" cellpadding=""2"" cellspacing=""0""><tr><td valign=""top"" align=""left""><br/>No Company Log Items Found</td></tr></table>")
            End If

            If company_counter > 0 Then

                htmlOut.Append("<table id=""currentCompanyCountryDataTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"">")
                htmlOut.Append("<tr bgcolor=""#D9D8D8"">")

                htmlOut.Append("<td valign=""middle"" align=""left""><b>UNIQUE&nbsp;COMPANIES</b></td><td valign=""middle"" align=""right"">" + company_counter.ToString.Trim + "</td>")
                htmlOut.Append("<td valign=""middle"" align=""left""><b>USER&nbsp;COUNT</b></td><td valign=""middle"" align=""right"">" + user_counter.ToString.Trim + "</td>")

                htmlOut.Append("</tr>")
                htmlOut.Append("</table>")

            End If

        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />Error in displayAdminCompanyLog(ByVal searchCriteria As onLineUsersSelectionCriteriaClass, ByRef out_htmlString As String)<br />" + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub

    Public Function getAdminCRMUsersDataTable(ByVal searchCriteria As onLineUsersSelectionCriteriaClass) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
        Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
        Dim MySqlReader As MySql.Data.MySqlClient.MySqlDataReader = Nothing
        Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing

        Try

            sQuery.Append("SELECT * FROM client_user")
            sQuery.Append(" INNER JOIN client_register_master ON client_regID = cliuser_client_regid")
            sQuery.Append(" WHERE cliuser_last_session_date > '" + Format(DateAdd("n", -10, Now()), "yyyy-MM-dd H:mm:ss").Trim + "'")
            sQuery.Append(" AND ((cliuser_last_session_date <> cliuser_last_logout_date) OR (cliuser_last_logout_date is NULL))")

            If Not String.IsNullOrEmpty(searchCriteria.OnLineCriteriaFrequency.Trim) Then
                sQuery.Append(Constants.cAndClause + "client_regFrequency = '" + searchCriteria.OnLineCriteriaFrequency.ToUpper.Trim + "'")
            End If

            If Not String.IsNullOrEmpty(searchCriteria.OnLineCriteriaSearchItem.Trim) Then
                sQuery.Append(Constants.cAndClause + "(client_webhostname LIKE '" + searchCriteria.OnLineCriteriaSearchItem.Trim + "%'")
                sQuery.Append(Constants.cOrClause + "cliuser_email_address LIKE '" + searchCriteria.OnLineCriteriaSearchItem.Trim + "%'")
                sQuery.Append(Constants.cOrClause + "cliuser_first_name LIKE '" + searchCriteria.OnLineCriteriaSearchItem.Trim + "%'")
                sQuery.Append(Constants.cOrClause + "cliuser_last_name LIKE '" + searchCriteria.OnLineCriteriaSearchItem.Trim + "%')")
            End If

            If Not String.IsNullOrEmpty(searchCriteria.OnLineCriteriaOrderBy.Trim) Then

                Select Case (searchCriteria.OnLineCriteriaOrderBy.ToUpper.Trim)
                    Case "NAME1"
                        sQuery.Append(" ORDER BY client_webhostname, cliuser_last_name DESC")
                    Case "NAME2"
                        sQuery.Append(" ORDER BY client_webhostname, cliuser_last_name ASC")

                End Select
            Else
                sQuery.Append(" ORDER BY client_webhostname, cliuser_last_name ASC")
            End If

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>getAdminCRMUsersDataTable(ByVal searchCriteria As onLineUsersSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

            MySqlConn.ConnectionString = crmMasterConnectStr

            MySqlConn.Open()
            MySqlCommand.Connection = MySqlConn
            MySqlCommand.CommandType = CommandType.Text
            MySqlCommand.CommandTimeout = 240

            MySqlCommand.CommandText = sQuery.ToString

            MySqlReader = MySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                atemptable.Load(MySqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getAdminCRMUsersDataTable load datatable</b><br /> " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getAdminCRMUsersDataTable(ByVal searchCriteria As onLineUsersSelectionCriteriaClass) As DataTable</b><br />" + ex.Message

        Finally
            MySqlReader = Nothing

            MySqlConn.Dispose()
            MySqlConn.Close()
            MySqlConn = Nothing

            MySqlCommand.Dispose()
            MySqlCommand = Nothing
        End Try

        Return atemptable

    End Function

    Public Sub displayAdminCRMUsers(ByVal searchCriteria As onLineUsersSelectionCriteriaClass, ByRef out_htmlString As String)

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False

        Try

            results_table = getAdminCRMUsersDataTable(searchCriteria)

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    htmlOut.Append("<table id=""currentMPMUsersDataTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"">")
                    htmlOut.Append("<tr bgcolor=""#D9D8D8"">")

                    htmlOut.Append("<td valign=""middle"" align=""left""><b>LOGIN</b></td>")
                    htmlOut.Append("<td valign=""middle"" align=""left""><b>HOST&nbsp;NAME</b></td>")
                    htmlOut.Append("<td valign=""middle"" align=""left""><b>FREQUENCY</b></td>")

                    If searchCriteria.OnLineCriteriaOrderBy.ToUpper.Contains("NAME1") Then
                        htmlOut.Append("<td valign=""middle"" align=""left""><b><a class=""underline pointer"" href=""adminCurrentUsers.aspx?new=Y&freq=" + searchCriteria.OnLineCriteriaFrequency.Trim + "&order=name2"" title=""Order by contact Last name"">USER NAME</a></td>")
                    Else
                        htmlOut.Append("<td valign=""middle"" align=""left""><b><a class=""underline pointer"" href=""adminCurrentUsers.aspx?new=Y&freq=" + searchCriteria.OnLineCriteriaFrequency.Trim + "&order=name1"" title=""Order by contact Last name"">USER NAME</a></td>")
                    End If

                    htmlOut.Append("<td valign=""middle"" align=""left""><b>EMAIL</b></td>")
                    htmlOut.Append("<td valign=""middle"" align=""left""><b>LAST SESSION DATE</b></td>")


                    htmlOut.Append("</tr>")

                    For Each r As DataRow In results_table.Rows

                        If Not toggleRowColor Then
                            htmlOut.Append("<tr class=""alt_row"">")
                            toggleRowColor = True
                        Else
                            htmlOut.Append("<tr bgcolor=""white"">")
                            toggleRowColor = False
                        End If

                        htmlOut.Append("<td valign=""middle"" align=""left"">")

                        If Not IsDBNull(r.Item("cliuser_login")) Then
                            If Not String.IsNullOrEmpty(r.Item("cliuser_login").ToString.Trim) Then
                                htmlOut.Append(r.Item("cliuser_login").ToString.Trim)
                            End If
                        End If

                        htmlOut.Append("</td>")
                        htmlOut.Append("<td valign=""middle"" align=""left"">")

                        If Not IsDBNull(r.Item("client_webhostname")) Then
                            If Not String.IsNullOrEmpty(r.Item("client_webhostname").ToString.Trim) Then
                                htmlOut.Append(r.Item("client_webhostname").ToString.Trim)
                            End If
                        End If

                        htmlOut.Append("</td>")
                        htmlOut.Append("<td valign=""middle"" align=""left"">")

                        If Not IsDBNull(r.Item("client_regFrequency")) Then
                            If Not String.IsNullOrEmpty(r.Item("client_regFrequency").ToString.Trim) Then
                                htmlOut.Append(r.Item("client_regFrequency").ToString.Trim)
                            End If
                        End If

                        htmlOut.Append("</td>")
                        htmlOut.Append("<td valign=""middle"" align=""left"">")

                        If Not IsDBNull(r.Item("cliuser_first_name")) Then
                            If Not String.IsNullOrEmpty(r.Item("cliuser_first_name").ToString.Trim) Then
                                htmlOut.Append(r.Item("cliuser_first_name").ToString.Trim)
                            End If
                        End If

                        If Not IsDBNull(r.Item("cliuser_last_name")) Then
                            If Not String.IsNullOrEmpty(r.Item("cliuser_last_name").ToString.Trim) Then
                                htmlOut.Append(Constants.cSingleSpace + r.Item("cliuser_last_name").ToString.Trim)
                            End If
                        End If

                        htmlOut.Append("</td>")
                        htmlOut.Append("<td valign=""middle"" align=""left"">")

                        If Not IsDBNull(r.Item("cliuser_email_address")) Then
                            If Not String.IsNullOrEmpty(r.Item("cliuser_email_address").ToString.Trim) Then
                                htmlOut.Append(r.Item("cliuser_email_address").ToString.Trim)
                            End If
                        End If

                        htmlOut.Append("</td>")
                        htmlOut.Append("<td valign=""middle"" align=""left"">")

                        If Not IsDBNull(r.Item("cliuser_last_session_date")) Then
                            If Not String.IsNullOrEmpty(r.Item("cliuser_last_session_date").ToString.Trim) Then
                                htmlOut.Append(FormatDateTime(r.Item("cliuser_last_session_date").ToString.Trim, DateFormat.GeneralDate))
                            End If
                        End If

                        htmlOut.Append("</td>")

                        htmlOut.Append("</tr>")

                    Next

                    htmlOut.Append("</table>")

                Else
                    htmlOut.Append("<table id=""currentMPMUsersDataTable"" width=""100%"" cellpadding=""2"" cellspacing=""0""><tr><td valign=""top"" align=""left""><br/>No Company Log Items Found</td></tr></table>")
                End If
            Else
                htmlOut.Append("<table id=""currentMPMUsersDataTable"" width=""100%"" cellpadding=""2"" cellspacing=""0""><tr><td valign=""top"" align=""left""><br/>No Company Log Items Found</td></tr></table>")
            End If

        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />Error in displayAdminCompanyLog(ByVal searchCriteria As onLineUsersSelectionCriteriaClass, ByRef out_htmlString As String)<br />" + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub

    'Public Function display_top_models() As String
    '  display_top_models = ""


    '  Dim es As SqlClient.SqlDataReader
    '  Dim strSQL As String = ""
    '  Dim temp_string As String = ""
    '  Dim SqlConn As New SqlClient.SqlConnection
    '  Dim SqlCommand As New SqlClient.SqlCommand
    '  Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    '  Try
    '    SqlConn.ConnectionString = adminConnectStr
    '    SqlConn.Open()
    '    SqlCommand.Connection = SqlConn


    '    Dim temp_datetime_query As String = ""
    '    Dim temp_datetime As String = ""

    '    temp_datetime = DateAdd(DateInterval.Month, -12, Now())

    '    temp_datetime_query = Year(temp_datetime) & "-"

    '    If Month(temp_datetime) < 10 Then
    '      temp_datetime_query = temp_datetime_query & "0"
    '    End If

    '    temp_datetime_query = temp_datetime_query & Month(temp_datetime) & "-"

    '    If Day(temp_datetime) < 10 Then
    '      temp_datetime_query = temp_datetime_query & "0"
    '    End If
    '    temp_datetime_query = temp_datetime_query & Day(temp_datetime) & " " & Hour(temp_datetime) & ":" & Minute(temp_datetime) & ":" & Second(temp_datetime)

    '    strSQL = "select top 100 amod_make_name, amod_model_name, amod_id, "
    '    strSQL = strSQL & " COUNT(*) as tcount "
    '    strSQL = strSQL & " from Subscription_Install_Log a  with (nolock)"
    '    strSQL = strSQL & " inner join Aircraft b  with (nolock) on subislog_ac_id = ac_id and ac_journ_id = 0"
    '    strSQL = strSQL & " inner join aircraft_model c  with (nolock) on ac_amod_id = amod_id"
    '    strSQL = strSQL & " where subislog_amod_id > 0 "
    '    strSQL = strSQL & " and subislog_msg_type='UserStatistics'"
    '    strSQL = strSQL & " and subislog_subid not in (select distinct sub_id from Subscription where sub_comp_id = 135887)"
    '    strSQL = strSQL & " and subislog_date >= '" & temp_datetime_query & "'"
    '    strSQL = strSQL & " group by amod_make_name, amod_model_name, amod_id"
    '    strSQL = strSQL & " order by COUNT(*) desc"


    '    HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>display_crm_new_user</b><br />" & strSQL


    '    SqlCommand.CommandText = strSQL
    '    es = SqlCommand.ExecuteReader()
    '    'response.Write Application("objAdminConn")


    '    If es.HasRows Then

    '      temp_string = temp_string & "<b>Top Models Display (Last 12 Months)</b><br><br>"
    '      temp_string = temp_string & "<table width='100%' border='1px' cellspacing='2' cellpadding='2'>"
    '      temp_string = temp_string & "<tr bgcolor='gray'>"
    '      temp_string = temp_string & "<td><b>Make/Model:</b></td>"
    '      temp_string = temp_string & "<td><b>Count:</b></td>"
    '      temp_string = temp_string & "</tr>"


    '      Do While es.Read

    '        temp_string = temp_string & "<tr>"

    '        temp_string = temp_string & "<td>" & es("amod_make_name") & " " & es("amod_model_name") & "</td>"
    '        temp_string = temp_string & "<td align='right'>" & FormatNumber(es("tcount"), 0) & "</td>"

    '        temp_string = temp_string & "</tr>"
    '      Loop

    '      temp_string = temp_string & "</table>"
    '    Else
    '      temp_string = temp_string & ""
    '    End If
    '    es.Close()
    '    es = Nothing


    '    display_top_models = temp_string

    '  Catch ex As Exception
    '  Finally
    '    SqlConn.Close()
    '    SqlConn.Dispose()
    '    SqlCommand.Dispose()

    '    SqlCommand = Nothing
    '    SqlConn = Nothing
    '  End Try
    'End Function

    'Public Function display_top_usage() As String
    '  display_top_usage = ""


    '  Dim es As SqlClient.SqlDataReader
    '  Dim strSQL As String = ""
    '  Dim temp_string As String = ""
    '  Dim SqlConn As New SqlClient.SqlConnection
    '  Dim SqlCommand As New SqlClient.SqlCommand
    '  Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    '  Try
    '    SqlConn.ConnectionString = adminConnectStr
    '    SqlConn.Open()
    '    SqlCommand.Connection = SqlConn


    '    Dim temp_datetime_query As String = ""
    '    Dim temp_datetime As String = ""

    '    temp_datetime = DateAdd(DateInterval.Month, -12, Now())

    '    temp_datetime_query = Year(temp_datetime) & "-"

    '    If Month(temp_datetime) < 10 Then
    '      temp_datetime_query = temp_datetime_query & "0"
    '    End If

    '    temp_datetime_query = temp_datetime_query & Month(temp_datetime) & "-"

    '    If Day(temp_datetime) < 10 Then
    '      temp_datetime_query = temp_datetime_query & "0"
    '    End If
    '    temp_datetime_query = temp_datetime_query & Day(temp_datetime) & " " & Hour(temp_datetime) & ":" & Minute(temp_datetime) & ":" & Second(temp_datetime)

    '    strSQL = "select evoview_title, COUNT(*) as tcount"
    '    strSQL = strSQL & " from subscription_install_log  with (nolock) "
    '    strSQL = strSQL & " inner join Evolution_Views  with (nolock) on subislog_view_id = evoview_id"
    '    strSQL = strSQL & " where subislog_msg_type ='UserDisplayView'"
    '    strSQL = strSQL & " and subislog_date >= '" & temp_datetime_query & "'"
    '    strSQL = strSQL & " and subislog_view_id not in (0,5) "
    '    strSQL = strSQL & " group by evoview_title "
    '    strSQL = strSQL & " order by COUNT(*) desc"

    '    HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>display_crm_new_user</b><br />" & strSQL


    '    SqlCommand.CommandText = strSQL
    '    es = SqlCommand.ExecuteReader()
    '    'response.Write Application("objAdminConn")


    '    If es.HasRows Then

    '      temp_string = temp_string & "<b>Top Views Display (Last 12 Months)</b><br><br>"
    '      temp_string = temp_string & "<table width='100%' border='1px' cellspacing='2' cellpadding='2'>"
    '      temp_string = temp_string & "<tr bgcolor='gray'>"
    '      temp_string = temp_string & "<td><b>Evo View Title:</b></td>"
    '      ' temp_string = temp_string & "<td><b>Year:</b></td>"
    '      temp_string = temp_string & "<td><b>Count:</b></td>"
    '      temp_string = temp_string & "</tr>"


    '      Do While es.Read

    '        temp_string = temp_string & "<tr>"

    '        temp_string = temp_string & "<td>" & es("evoview_title") & "</td>"
    '        '  temp_string = temp_string & "<td align='right'>" & es("tyear") & "</td>"
    '        temp_string = temp_string & "<td align='right'>" & FormatNumber(es("tcount"), 0) & "</td>"

    '        temp_string = temp_string & "</tr>"
    '      Loop

    '      temp_string = temp_string & "</table>"
    '    Else
    '      temp_string = temp_string & ""
    '    End If
    '    es.Close()
    '    es = Nothing


    '    display_top_usage = temp_string

    '  Catch ex As Exception
    '  Finally
    '    SqlConn.Close()
    '    SqlConn.Dispose()
    '    SqlCommand.Dispose()

    '    SqlCommand = Nothing
    '    SqlConn = Nothing
    '  End Try
    'End Function

#End Region

#Region "admin_subscriber_search_functions"

    Public Function getSubscriberSearchDataTable(ByVal searchCriteria As SearchSelectionCriteria, ByVal bBySubscription As Boolean, Optional ByVal bHistorical As Boolean = False) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            If bBySubscription Then
                sQuery.Append("SELECT DISTINCT sub_id, sub_parent_sub_id, comp_id, comp_name, comp_city, comp_state, comp_country, serv_code, sub_helicopters_flag, sub_business_aircraft_flag, sub_commerical_flag, sub_max_allowed_custom_export, sub_share_by_comp_id_flag,")
                sQuery.Append(" sub_yacht_flag, sub_busair_tier_level, sub_aerodex_flag, sub_sale_price_flag, sub_cloud_notes_flag, sub_cloud_notes_database, sub_server_side_notes_flag, sub_server_side_dbase_name, sub_nbr_of_installs, sub_share_by_parent_sub_id_flag,")
                sQuery.Append(" sub_start_date, sub_end_date FROM " + IIf(Not bHistorical, "View_JETNET_Customers", "View_JETNET_Customers_History"))
            Else
                sQuery.Append("SELECT DISTINCT sub_id, sub_parent_sub_id, comp_id, comp_name, comp_city, comp_state, comp_country, subins_admin_flag,")
                sQuery.Append(" contact_id, contact_email_address, contact_sirname, contact_first_name, contact_middle_initial, contact_last_name, contact_title,")
                sQuery.Append(" subins_evo_mobile_flag, subins_mobile_active_date, sublogin_password, subins_last_login_date, LastHostName FROM " + IIf(Not bHistorical, "View_JETNET_Customers", "View_JETNET_Customers_History"))
            End If

            sQuery.Append(" LEFT OUTER JOIN State WITH(NOLOCK) ON state_code = comp_state")

            sQuery.Append(" WHERE")

            sQuery.Append(commonEvo.BuildCompanyProductCodeCheckWhereClause(searchCriteria.SearchCriteriaHelicopterFlag, searchCriteria.SearchCriteriaBusinessFlag, searchCriteria.SearchCriteriaCommercialFlag, False, searchCriteria.SearchCriteriaYachtFlag, True))

            If searchCriteria.SearchCriteriaHasCompanyLocationInfo Then

                sQuery.Append(commonEvo.check_selected_geography_from_dropdowns(IIf(searchCriteria.SearchCriteriaUseContinent, "continent", "region"), searchCriteria.SearchCriteriaCompanyContinent, searchCriteria.SearchCriteriaCompanyRegion, searchCriteria.SearchCriteriaCompanyStateProvince, False))

                If Not String.IsNullOrEmpty(searchCriteria.SearchCriteriaCompanyTimezone) Then
                    sQuery.Append(Constants.cAndClause + "comp_timezone IN ('" + commonEvo.TranslateTimeZone(searchCriteria.SearchCriteriaCompanyTimezone).Replace(Constants.cCommaDelim, Constants.cValueSeperator) + "')")
                End If

            End If

            If Not String.IsNullOrEmpty(searchCriteria.SearchCriteriaServices.Trim) Then

                sQuery.Append(Constants.cAndClause + "comp_id IN (select distinct csu_comp_id")
                sQuery.Append(" FROM Company_Services_Used with (NOLOCK)")
                sQuery.Append(" WHERE csu_svud_id IN (" + searchCriteria.SearchCriteriaServices + "))")

            End If

            If Not String.IsNullOrEmpty(searchCriteria.SearchCriteriaCompanyBusinessType.Trim) Then
                sQuery.Append(Constants.cAndClause + "comp_business_type IN (" + searchCriteria.SearchCriteriaCompanyBusinessType + ")")
            End If

            If Not String.IsNullOrEmpty(searchCriteria.SearchCriteriaCompanyNameQueryString.Trim) Then
                sQuery.Append(Constants.cAndClause + searchCriteria.SearchCriteriaCompanyNameQueryString.Trim)
            End If

            If Not String.IsNullOrEmpty(searchCriteria.SearchCriteriaCompanyEmail.Trim) Then
                sQuery.Append(Constants.cAndClause + "comp_email_address = '" + searchCriteria.SearchCriteriaCompanyEmail.ToString + "'")
            End If

            If Not String.IsNullOrEmpty(searchCriteria.SearchCriteriaCompanyPostalCode.Trim) Then
                sQuery.Append(Constants.cAndClause + "comp_zip_code = '" + searchCriteria.SearchCriteriaCompanyPostalCode.ToString + "'")
            End If

            If Not String.IsNullOrEmpty(searchCriteria.SearchCriteriaCompanyAddress.Trim) Then
                sQuery.Append(Constants.cAndClause + "comp_address = '" + searchCriteria.SearchCriteriaCompanyAddress.ToString + "'")
            End If

            If Not String.IsNullOrEmpty(searchCriteria.SearchCriteriaCompanyCity.Trim) Then
                sQuery.Append(Constants.cAndClause + "comp_city = '" + searchCriteria.SearchCriteriaCompanyCity.ToString + "'")
            End If

            If Not String.IsNullOrEmpty(searchCriteria.SearchCriteriaQueryString.Trim) Then
                sQuery.Append(Constants.cAndClause + searchCriteria.SearchCriteriaQueryString.Trim)
            End If

            'Contact info localCriteria.SearchCriteriaCompanyContactTitle
            If Not String.IsNullOrEmpty(searchCriteria.SearchCriteriaCompanyContactFirstName.Trim) Then
                sQuery.Append(Constants.cAndClause + "contact_first_name = '" + searchCriteria.SearchCriteriaCompanyContactFirstName.ToString + "'")
            End If

            If Not String.IsNullOrEmpty(searchCriteria.SearchCriteriaCompanyContactLastName.Trim) Then
                sQuery.Append(Constants.cAndClause + "contact_last_name = '" + searchCriteria.SearchCriteriaCompanyContactLastName.ToString + "'")
            End If

            If Not String.IsNullOrEmpty(searchCriteria.SearchCriteriaCompanyContactEmail.Trim) Then
                sQuery.Append(Constants.cAndClause + "contact_email_address = '" + searchCriteria.SearchCriteriaCompanyContactEmail.ToString + "'")
            End If

            If Not String.IsNullOrEmpty(searchCriteria.SearchCriteriaCompanyContactTitle.Trim) Then
                sQuery.Append(Constants.cAndClause + searchCriteria.SearchCriteriaCompanyContactTitle)
            End If

            If searchCriteria.SearchCriteriaCompanyID > 0 Then
                sQuery.Append(Constants.cAndClause + "comp_id = " + searchCriteria.SearchCriteriaCompanyID.ToString)
            End If

            If searchCriteria.SearchCriteriaCompanyContactID > 0 Then
                sQuery.Append(Constants.cAndClause + "contact_id = " + searchCriteria.SearchCriteriaCompanyContactID.ToString)
            End If

            If Not String.IsNullOrEmpty(searchCriteria.SearchCriteriaSub_user_id.Trim) Then
                sQuery.Append(Constants.cAndClause + "sub_tech_id = '" + searchCriteria.SearchCriteriaSub_user_id.Trim + "'")
            End If

            If Not String.IsNullOrEmpty(searchCriteria.SearchCriteriaSub_login.Trim) Then
                sQuery.Append(Constants.cAndClause + "sublogin_login = '" + searchCriteria.SearchCriteriaSub_login.Trim + "'")
            End If

            If searchCriteria.SearchCriteriaSub_id > 0 Then
                sQuery.Append(Constants.cAndClause + "sub_id = " + searchCriteria.SearchCriteriaSub_id.ToString)
            End If

            If searchCriteria.SearchCriteriaSequence_number > 0 Then
                sQuery.Append(Constants.cAndClause + "subins_seq_no = " + searchCriteria.SearchCriteriaSequence_number.ToString)
            End If

            If Not String.IsNullOrEmpty(searchCriteria.SearchCriteriaService_code.Trim) Then
                sQuery.Append(Constants.cAndClause + "sub_serv_code IN (" + searchCriteria.SearchCriteriaService_code.Trim + ")")
            End If

            If Not String.IsNullOrEmpty(searchCriteria.SearchCriteriaLast_login_date.Trim) Then
                sQuery.Append(Constants.cAndClause + "subins_last_login_date = '" + CDate(searchCriteria.SearchCriteriaLast_login_date.Trim).ToShortDateString.Trim + "'")
            End If

            If Not String.IsNullOrEmpty(searchCriteria.SearchCriteriaStart_date.Trim) Then
                sQuery.Append(Constants.cAndClause + "sub_start_date = '" + CDate(searchCriteria.SearchCriteriaStart_date.Trim).ToShortDateString.Trim + "'")
            End If

            If Not String.IsNullOrEmpty(searchCriteria.SearchCriteriaEnd_date.Trim) Then
                sQuery.Append(Constants.cAndClause + "sub_end_date = '" + CDate(searchCriteria.SearchCriteriaEnd_date.Trim).ToShortDateString.Trim + "'")
            End If

            If searchCriteria.SearchCriteriaAerodexFlag Then
                sQuery.Append(Constants.cAndClause + "sub_aerodex_flag = 'Y'")
            End If

            If searchCriteria.SearchCriteriaDemoFlag Then
                sQuery.Append(Constants.cAndClause + "sublogin_demo_flag = 'Y'")
            End If

            If searchCriteria.SearchCriteriaMarketingFlag Then
                sQuery.Append(Constants.cAndClause + "sub_marketing_flag = 'Y'")
            End If

            If searchCriteria.SearchCriteriaCRMFlag Then
                sQuery.Append(Constants.cAndClause + "sub_serv_code LIKE 'CRM'")
            End If

            If searchCriteria.SearchCriteriaSPIFlag Then
                sQuery.Append(Constants.cAndClause + "sub_sale_price_flag = 'Y'")
            End If

            If searchCriteria.SearchCriteriaMobileFlag Then
                sQuery.Append(Constants.cAndClause + "subins_evo_mobile_flag = 'Y'")
            End If

            If searchCriteria.SearchCriteriaLocalNotesFlag Then
                sQuery.Append(Constants.cAndClause + "subins_local_db_flag = 'Y'")
            End If

            If searchCriteria.SearchCriteriaCloudNotesFlag Then
                sQuery.Append(Constants.cAndClause + "sub_cloud_notes_flag = 'Y'")
            End If

            If searchCriteria.SearchCriteriaNotesPlusFlag Then
                sQuery.Append(Constants.cAndClause + "sub_server_side_notes_flag = 'Y'")
            End If

            If searchCriteria.SearchCriteriaActiveFlag Then
                sQuery.Append(Constants.cAndClause + "((sublogin_active_flag = 'Y') AND (subins_active_flag = 'Y') AND (contact_active_flag = 'Y'))")
            End If

            If searchCriteria.SearchCriteriaExpiredFlag Then
                sQuery.Append(Constants.cAndClause + "sublogin_active_flag = 'N'")
            End If

            If searchCriteria.SearchCriteriaAdminFlag Then
                sQuery.Append(Constants.cAndClause + "subins_admin_flag = 'Y'")
            End If

            If searchCriteria.SearchCriteriaParentSub Then
                sQuery.Append(Constants.cAndClause + "sub_id = sub_parent_sub_id")
            End If

            sQuery.Append(" ORDER BY sub_id, comp_name")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + sQuery.ToString

            SqlConn.ConnectionString = clientConnectStr

            SqlConn.Open()
            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 240

            SqlCommand.CommandText = sQuery.ToString
            HttpContext.Current.Session.Item("MasterSubscriber") = sQuery.ToString

            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

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

    Public Function getSubscriberErrorDataTable(ByVal sEmailAddress As String, ByVal sub_id As Long, ByVal login As String, ByVal records As Long, Optional ByVal sum_by As String = "", Optional ByVal days_back As Integer = 0) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            If Trim(sum_by) = "" Then
                If records > 0 Then
                    sQuery.Append("SELECT TOP " + records.ToString + " subislog_date, subislog_msg_type, subislog_message, subislog_host_name, subislog_tcpip, iploc_region, iploc_country_name,")
                Else
                    sQuery.Append("SELECT TOP 250 subislog_date, subislog_msg_type, subislog_message, subislog_host_name, subislog_tcpip, iploc_region, iploc_country_name,")
                End If

                sQuery.Append(" (SELECT DISTINCT amod_make_name + ' ' + amod_model_name FROM Aircraft_Model WITH(NOLOCK) WHERE subislog_amod_id = amod_id) AS AcModel,")
                sQuery.Append(" (SELECT DISTINCT ' Ser#:' + ac_ser_no_full FROM Aircraft_Flat WITH(NOLOCK) WHERE subislog_ac_id = ac_id AND ac_journ_id = 0) AS AcName,")
                sQuery.Append(" (SELECT DISTINCT comp_name FROM company WITH(NOLOCK) WHERE subislog_comp_id = comp_id AND comp_journ_id = 0) AS CompName")
            Else
                If Trim(sum_by) = "export_counts" Then
                    sQuery.Append("SELECT subislog_msg_type, subislog_message as Sum_By ")

                ElseIf Trim(sum_by) = "export_details" Then
                    sQuery.Append("SELECT subislog_msg_type, subislog_message as Sum_By ")

                    sQuery.Append(" , (Select distinct s3.subislog_message from Subscription_Install_Log s3 With(NOLOCK)   where subislog_id =  ")
                    sQuery.Append("(select distinct top 1 s2.subislog_id from  Subscription_Install_Log s2 With(NOLOCK)   ")
                    sQuery.Append(" where s2.subislog_email_address = 'PAT.MCHAFFEY@SPARFELL-PARTNERS.COM' and s2.subislog_id < Subscription_Install_Log.subislog_id ")
                    sQuery.Append(" And subislog_msg_type in('UserSearch')  order by subislog_id desc)) as Searched ")

                ElseIf Trim(sum_by) = "AcModel" Then
                    sQuery.Append(" SELECT DISTINCT amod_make_name + ' ' + amod_model_name FROM Aircraft_Model WITH(NOLOCK) WHERE subislog_amod_id = amod_id  AS Sum_By ")
                ElseIf Trim(sum_by) = "AcName" Then
                    sQuery.Append(" SELECT DISTINCT ' Ser#:' + ac_ser_no_full FROM Aircraft_Flat WITH(NOLOCK) WHERE subislog_ac_id = ac_id AND ac_journ_id = 0  AS Sum_By ")
                ElseIf Trim(sum_by) = "CompName" Then
                    sQuery.Append(" SELECT DISTINCT comp_name FROM company WITH(NOLOCK) WHERE subislog_comp_id = comp_id AND comp_journ_id = 0  AS Sum_By ")
                ElseIf Trim(sum_by) = "subislog_message" Then
                    ' sQuery.Append("SELECT  subislog_msg_type, left(replace(subislog_message, 'Phrase:', 'Phrase Searches:'), 16) as Sum_By  ")
                    sQuery.Append("SELECT  subislog_msg_type, case when charindex(':', subislog_message) > 0 then left(subislog_message, charindex(':', subislog_message) ) else subislog_message end as Sum_By  ")
                Else
                    sQuery.Append("SELECT    " & Trim(sum_by) & " as Sum_By ")
                End If

                If Trim(sum_by) = "export_details" Then
                Else
                    sQuery.Append(", count(*) As tcount ")
                End If

            End If

            sQuery.Append(" FROM Subscription_Install_Log With(NOLOCK)")
            sQuery.Append(" LEFT OUTER JOIN IP_Location With(NOLOCK) On subislog_tcpip = iploc_ip")


            If sub_id > 0 Then
                sQuery.Append(" WHERE subislog_subid = " + sub_id.ToString)
                sQuery.Append(" And subislog_login = '" + login.Trim + "' AND subislog_seq_no = 1")
            Else
                sQuery.Append(" WHERE subislog_email_address = '" + sEmailAddress.Trim + "'")
            End If

            If days_back > 0 Then
                sQuery.Append(" and subislog_date >= dateadd(D,-" & (days_back - 1) & ", CAST(GETDATE() AS DATE)) ")
            ElseIf Trim(sum_by) = "" Then

            Else
                sQuery.Append(" and subislog_date >= dateadd(D,-365, CAST(GETDATE() AS DATE)) ")
            End If


            If Trim(sum_by) = "export_details" Then
                sQuery.Append(" and subislog_msg_type in ('UserJetnetReport','UserExport') ")
                sQuery.Append(" And subislog_message Not in ('Aircraft Full Spec - Standard', 'Aircraft Condensed Spec', 'Aircraft Full Spec - Classic', 'Aircraft Single Page Spec') ")
                sQuery.Append(" ORDER BY subislog_date DESC")
            ElseIf Trim(sum_by) = "export_counts" Then
                sQuery.Append(" and subislog_msg_type in ('UserJetnetReport','UserExport') ")
                sQuery.Append(" GROUP BY subislog_msg_type, subislog_message ")
                sQuery.Append(" ORDER BY tcount DESC")
            ElseIf Trim(sum_by) = "subislog_message" Then

                ' sQuery.Append(" And subislog_msg_type Not in ('UserSearch', 'UserStatistics') ")

                sQuery.Append(" GROUP BY subislog_msg_type, case when charindex(':', subislog_message) > 0 then left(subislog_message, charindex(':', subislog_message) ) else subislog_message end ")
                'sQuery.Append(" GROUP BY subislog_msg_type, left(replace(subislog_message, 'Phrase:', 'Phrase Searches:'), 16) ")
                sQuery.Append(" ORDER BY tcount DESC")
            Else
                If Trim(sum_by) = "" Then
                    sQuery.Append(" ORDER BY subislog_id DESC")
                Else
                    sQuery.Append(" GROUP BY " & Trim(sum_by))
                    sQuery.Append(" ORDER BY tcount DESC")
                End If
            End If







            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, [GetType]().FullName, sQuery.ToString)

            SqlConn.ConnectionString = adminConnectStr

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
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodInfo.GetCurrentMethod().ToString + "</b><br />" + constrExc.Message.ToString.Trim
            End Try

        Catch ex As Exception
            Return Nothing

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodInfo.GetCurrentMethod().ToString + "</b><br />" + ex.Message.ToString.Trim

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

    Public Sub displaySubscriberErrorList(ByVal sEmailAddress As String, ByRef out_htmlString As String, Optional ByVal sub_id As Long = 0, Optional ByVal user_login As String = "", Optional ByVal records As Long = 0, Optional ByVal sum_by As String = "", Optional ByVal days_back As Integer = 0)

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False

        Dim nCount As Integer = 0

        Dim hadAircraft As Boolean = False

        Try

            If Trim(sum_by) = "" Then
                results_table = getSubscriberErrorDataTable(sEmailAddress, sub_id, user_login, records, "", days_back)
            Else
                results_table = getSubscriberErrorDataTable(sEmailAddress, sub_id, user_login, records, sum_by, days_back)
            End If


            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then
                    htmlOut.Append("<div class=""Box"">")
                    htmlOut.Append("<table id=""subscriberErrorDataTable"" width=""100%"" cellpadding=""4"" cellspacing=""2"" class=""formatTable blue"">")

                    If Trim(sum_by) = "" Then
                        htmlOut.Append("<thead><tr><th align=""left""><b>DATE<br />TIME</b></th>")
                        htmlOut.Append("<th align=""left""><b>MSG TYPE</b></th>")
                        htmlOut.Append("<th align=""left"" style=""width : 80px;""><b>MESSAGE</b></th>")
                        htmlOut.Append("<th align=""left""><b>AIRCRAFT</b></th>")
                        htmlOut.Append("<th align=""left""><b>COMPANY</b></th>")
                        htmlOut.Append("<th align=""left""><b>WEB HOST</b></th>")
                        htmlOut.Append("<th align=""left""><b>CLIENT IP</b></th>")
                        htmlOut.Append("<th align=""left""><b>CLIENT<br />LOCATION</b></th>")
                        htmlOut.Append("</tr></thead><tbody>")
                    ElseIf Trim(sum_by) = "export_details" Then
                        htmlOut.Append("<thead><tr><th align=""left"" style=""width : 400px;""><b>CATEGORY</b></th>")
                        htmlOut.Append("<th align=""left"" style=""width : 400px;""><b>SEARCHED TERM</b></th>")
                        htmlOut.Append("</tr></thead><tbody>")
                    Else
                        htmlOut.Append("<thead><tr><th align=""left""><b>CATEGORY</b></th>")
                        htmlOut.Append("<th align=""left""><b>COUNT</b></th>")
                        htmlOut.Append("</tr></thead><tbody>")
                    End If


                    For Each r As DataRow In results_table.Rows

                        If Not toggleRowColor Then
                            htmlOut.Append("<tr class=""alt_row"">")
                            toggleRowColor = True
                        Else
                            htmlOut.Append("<tr bgcolor=""white"">")
                            toggleRowColor = False
                        End If


                        If Trim(sum_by) = "" Then
                            htmlOut.Append("<td valign=""middle"" align=""left"">")

                            If Not IsDBNull(r.Item("subislog_date")) Then
                                If Not String.IsNullOrEmpty(r.Item("subislog_date").ToString.Trim) Then
                                    htmlOut.Append(FormatDateTime(r.Item("subislog_date").ToString, DateFormat.ShortDate).Trim)
                                    htmlOut.Append("<br />" + FormatDateTime(r.Item("subislog_date").ToString, DateFormat.ShortTime).Trim)
                                End If
                            End If

                            htmlOut.Append("</td>")
                            htmlOut.Append("<td valign=""middle"" align=""left"">")

                            If Not IsDBNull(r.Item("subislog_msg_type")) Then
                                If Not String.IsNullOrEmpty(r.Item("subislog_msg_type").ToString.Trim) Then
                                    htmlOut.Append(r.Item("subislog_msg_type").ToString.Trim)
                                End If
                            End If

                            htmlOut.Append("</td>")
                            htmlOut.Append("<td valign=""middle"" align=""left"">")
                            If Not IsDBNull(r.Item("subislog_message")) Then
                                If Not String.IsNullOrEmpty(r.Item("subislog_message").ToString.Trim) Then

                                    nCount = 0
                                    For Each ch As Char In r.Item("subislog_message").ToString.ToLower

                                        If Not Char.IsControl(ch) Then
                                            If Char.IsWhiteSpace(ch) Then
                                                htmlOut.Append(Constants.cSingleSpace)
                                            Else
                                                htmlOut.Append(ch)
                                            End If
                                        End If

                                        nCount += 1

                                        If nCount Mod 65 = 0 Then
                                            htmlOut.Append("<br/>")
                                        End If

                                    Next
                                End If
                            End If

                            htmlOut.Append("</td>")
                            htmlOut.Append("<td valign=""middle"" align=""left"">")

                            If Not IsDBNull(r.Item("AcName")) Then
                                If Not String.IsNullOrEmpty(r.Item("AcName").ToString.Trim) Then
                                    htmlOut.Append("AC : " + r.Item("AcName").ToString.Trim)
                                    hadAircraft = True
                                End If
                            End If

                            If Not IsDBNull(r.Item("AcModel")) Then
                                If Not String.IsNullOrEmpty(r.Item("AcModel").ToString.Trim) Then
                                    htmlOut.Append(IIf(hadAircraft, "<br />MOD : ", "MOD : ") + r.Item("AcModel").ToString.Trim)
                                End If
                            End If

                            htmlOut.Append("</td>")
                            htmlOut.Append("<td valign=""middle"" align=""left"">")

                            If Not IsDBNull(r.Item("CompName")) Then
                                If Not String.IsNullOrEmpty(r.Item("CompName").ToString.Trim) Then
                                    htmlOut.Append(r.Item("CompName").ToString.Trim)
                                End If
                            End If

                            htmlOut.Append("</td>")
                            htmlOut.Append("<td valign=""middle"" align=""left"">")

                            If Not IsDBNull(r.Item("subislog_host_name")) Then
                                If Not String.IsNullOrEmpty(r.Item("subislog_host_name").ToString.Trim) Then
                                    htmlOut.Append(r.Item("subislog_host_name").ToString.ToUpper.Replace(".COM", "").Trim)
                                End If
                            End If

                            htmlOut.Append("</td>")
                            htmlOut.Append("<td valign=""middle"" align=""left"">")

                            If Not IsDBNull(r.Item("subislog_tcpip")) Then
                                If Not String.IsNullOrEmpty(r.Item("subislog_tcpip").ToString.Trim) Then
                                    htmlOut.Append(r.Item("subislog_tcpip").ToString.Trim)
                                End If
                            End If

                            htmlOut.Append("</td>")
                            htmlOut.Append("<td valign=""middle"" align=""left"">")

                            If Not IsDBNull(r.Item("iploc_region")) Then
                                If Not String.IsNullOrEmpty(r.Item("iploc_region").ToString.Trim) Then
                                    htmlOut.Append(r.Item("iploc_region").ToString.Trim)
                                End If
                            End If

                            If Not IsDBNull(r.Item("iploc_country_name")) Then
                                If Not String.IsNullOrEmpty(r.Item("iploc_country_name").ToString.Trim) Then
                                    htmlOut.Append("<br />" + r.Item("iploc_country_name").ToString.Trim)
                                End If
                            End If

                            htmlOut.Append("</td>")

                            htmlOut.Append("</tr>")
                        ElseIf Trim(sum_by) = "export_details" Then
                            htmlOut.Append("<td valign=""middle"" align=""left"">")

                            If Not IsDBNull(r.Item("Sum_By")) Then
                                If Not String.IsNullOrEmpty(r.Item("Sum_By").ToString.Trim) Then

                                    If Trim(sum_by) = "subislog_message" Then
                                        htmlOut.Append(r.Item("subislog_msg_type").ToString & " - ")
                                    Else

                                    End If

                                    htmlOut.Append(r.Item("Sum_By").ToString)
                                End If
                            End If

                            htmlOut.Append("</td>")
                            htmlOut.Append("<td valign=""middle"" align=""left"">")

                            If Not IsDBNull(r.Item("Searched")) Then
                                If Not String.IsNullOrEmpty(r.Item("Searched").ToString.Trim) Then
                                    htmlOut.Append(r.Item("Searched").ToString.Trim)
                                End If
                            End If

                            htmlOut.Append("</td>")
                            htmlOut.Append("</tr>")

                        Else

                            htmlOut.Append("<td valign=""middle"" align=""left"">")


                            If Not IsDBNull(r.Item("Sum_By")) Then
                                If Not String.IsNullOrEmpty(r.Item("Sum_By").ToString.Trim) Then

                                    If Trim(sum_by) = "subislog_message" Then
                                        htmlOut.Append(r.Item("subislog_msg_type").ToString & " - ")
                                    Else

                                    End If

                                    htmlOut.Append(r.Item("Sum_By").ToString)

                                    If Hot_Words(r.Item("Sum_By"), r.Item("tcount")) = True Then
                                        htmlOut.Append("  <font color='red'>*****</font>")
                                    End If
                                End If
                            End If


                            htmlOut.Append("</td>")
                            htmlOut.Append("<td valign=""middle"" align=""left"">")

                            If Not IsDBNull(r.Item("tcount")) Then
                                If Not String.IsNullOrEmpty(r.Item("tcount").ToString.Trim) Then
                                    htmlOut.Append(r.Item("tcount").ToString.Trim)
                                End If
                            End If

                            htmlOut.Append("</td>")
                            htmlOut.Append("</tr>")
                        End If



                        hadAircraft = False

                    Next

                    htmlOut.Append("</tbody></table></div>")

                Else
                    htmlOut.Append("<div class=""Box""><table id=""subscriberErrorDataTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"" class=""formatTable blue""><tr><td valign=""top"" align=""left""><br/>No Subscriber Errors Found</td></tr></table></div>")
                End If
            Else
                htmlOut.Append("<div class=""Box""><table id=""subscriberErrorDataTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"" class=""formatTable blue""><tr><td valign=""top"" align=""left""><br/>No Subscriber Errors Found</td></tr></table></div>")
            End If

        Catch ex As Exception

            aError = "Error in displaySubscriberErrorList(ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub
    Public Function Hot_Words(ByVal hot_words_temp As String, ByVal hot_words_count As Integer) As Boolean

        Hot_Words = False

        If InStr(hot_words_temp, "ession has been logged due to inactivity") > 0 And hot_words_count > 100 Then
            Hot_Words = True
        ElseIf InStr(hot_words_temp, "ession has been terminated due to login from another location") > 0 And hot_words_count > 100 Then
            Hot_Words = True
        Else

        End If

    End Function

#End Region

#Region "admin_mpm_page_functions"

    Public Function getMPMUsersDataTable_For_User(ByVal reg_id As Long, ByVal user_id As Long) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
        Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
        Dim MySqlReader As MySql.Data.MySqlClient.MySqlDataReader = Nothing
        Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing

        Try



            sQuery.Append("SELECT cliuser_last_login_date, cliuser_loggedin_flag, cliuser_last_session_date ")
            sQuery.Append(" FROM Client_User WHERE cliuser_user_id = " & user_id.ToString & "")
            sQuery.Append(" And cliuser_client_regid = " & reg_id & "  ")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>getMPMUsersDataTable(ByVal reg_status As String, ByVal client_reg_Type As String, ByVal select_type As String) As DataTable</b><br />" + sQuery.ToString

            MySqlConn.ConnectionString = crmMasterConnectStr

            MySqlConn.Open()
            MySqlCommand.Connection = MySqlConn
            MySqlCommand.CommandType = CommandType.Text
            MySqlCommand.CommandTimeout = 240

            MySqlCommand.CommandText = sQuery.ToString

            MySqlReader = MySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                atemptable.Load(MySqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getMPMUsersDataTable load datatable</b><br /> " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getMPMUsersDataTable(ByVal reg_status As String, ByVal client_reg_Type As String, ByVal select_type As String) As DataTable</b><br />" + ex.Message

        Finally
            MySqlReader = Nothing

            MySqlConn.Dispose()
            MySqlConn.Close()
            MySqlConn = Nothing

            MySqlCommand.Dispose()
            MySqlCommand = Nothing
        End Try

        Return atemptable

    End Function

    Public Function getMPMUsersDataTable(ByVal reg_status As String, ByVal client_reg_Type As String, ByVal select_type As String) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
        Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
        Dim MySqlReader As MySql.Data.MySqlClient.MySqlDataReader = Nothing
        Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing

        Try

            If select_type.ToUpper.Trim.Contains("CONN") Then

                sQuery.Append("SELECT client_regName, client_webHostName, client_regCustomer_Type,")
                sQuery.Append(" client_regAerodexFlag, client_regID, client_regStatus, client_regFrequency,")
                sQuery.Append(" client_webUserLimit, client_regProductCode, client_regType,")
                sQuery.Append(" client_dbHost, client_dbUID, client_dbPWD, client_dbDatabase,")
                sQuery.Append(" client_dbBackHost, client_dbBackUID, client_dbBackPWD, client_dbBackDatabase, client_regSub_ID")
                sQuery.Append(" FROM client_register_master WHERE client_regStatus != '" + reg_status.Trim + "' " + client_reg_Type.Trim + " ORDER BY client_regName ASC")

            ElseIf select_type.ToUpper.Trim.Contains("ERROR") Or select_type.ToUpper.Trim.Contains("USER") Then

                sQuery.Append("SELECT client_regName, client_webHostName, client_regCustomer_Type, client_regSub_ID,")
                sQuery.Append(" client_regAerodexFlag, client_regID, client_regStatus, client_regFrequency, client_regDocumentsFlag,")
                sQuery.Append(" client_webUserLimit, client_regProductCode, client_regType, client_webCurrentUsers,")
                sQuery.Append(" (SELECT count(*) FROM client_error_log WHERE clierror_location = client_webHostName) AS errorcount,")
                sQuery.Append(" client_dbDatabase, client_dbHost, client_dbPWD, client_dbUID, client_reg_sale_price_flag, client_reg_sale_price_limit")
                sQuery.Append(" FROM client_register_master WHERE client_regStatus != '" + reg_status.Trim + "' " + client_reg_Type.Trim + " ORDER BY client_regName ASC")

            ElseIf select_type.ToUpper.Trim.Contains("DATA") Then

                sQuery.Append("SELECT client_regName, client_webHostName, client_regCustomer_Type,")
                sQuery.Append(" client_regAerodexFlag, client_regID, client_regStatus, client_regFrequency,")
                sQuery.Append(" client_webUserLimit, client_regProductCode, client_regType,")
                sQuery.Append(" client_dbHost, client_dbUID, client_dbPWD, client_dbDatabase,")
                sQuery.Append(" client_dbBackHost, client_dbBackUID, client_dbBackPWD, client_dbBackDatabase")
                sQuery.Append(" FROM client_register_master WHERE client_regStatus != '" + reg_status.Trim + "' " + client_reg_Type.Trim + " ORDER BY client_regName ASC")

            End If

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>getMPMUsersDataTable(ByVal reg_status As String, ByVal client_reg_Type As String, ByVal select_type As String) As DataTable</b><br />" + sQuery.ToString

            MySqlConn.ConnectionString = crmMasterConnectStr

            MySqlConn.Open()
            MySqlCommand.Connection = MySqlConn
            MySqlCommand.CommandType = CommandType.Text
            MySqlCommand.CommandTimeout = 240

            MySqlCommand.CommandText = sQuery.ToString

            MySqlReader = MySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                atemptable.Load(MySqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getMPMUsersDataTable load datatable</b><br /> " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getMPMUsersDataTable(ByVal reg_status As String, ByVal client_reg_Type As String, ByVal select_type As String) As DataTable</b><br />" + ex.Message

        Finally
            MySqlReader = Nothing

            MySqlConn.Dispose()
            MySqlConn.Close()
            MySqlConn = Nothing

            MySqlCommand.Dispose()
            MySqlCommand = Nothing
        End Try

        Return atemptable

    End Function

    Public Sub displayMPMUsers(ByVal reg_status As String, ByVal client_reg_Type As String, ByVal select_type As String, ByRef out_htmlString As String)

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False

        Dim nTotalClients As Integer = 0
        Dim nTotalUsers As Integer = 0
        Dim nTotalCurrentUsers As Integer = 0
        Dim nTotalMPMUsers As Integer = 0
        Dim nTotalDomainUsers As Integer = 0
        Dim nTotalErrors As Integer = 0
        Dim nTotalClientConn As Integer = 0
        Dim nTotalBackupConn As Integer = 0

        Dim db_host As String = ""
        Dim db_id As String = ""
        Dim db_pass As String = ""
        Dim db_db As String = ""

        Dim bu_host As String = ""
        Dim bu_id As String = ""
        Dim bu_pass As String = ""
        Dim bu_db As String = ""

        Dim nErrorCount As Long = 0

        Dim nDomainErrorCount As Long = 0
        Dim nDomainUsersCount As Long = 0


        Dim client_regName As String = ""
        Dim client_webHostName As String = ""
        Dim client_regCustomer_Type As String = ""
        Dim client_regAerodexFlag As String = ""
        Dim client_regStatus As String = ""
        Dim client_regID As Long = 0
        Dim client_reg_SubID As Long = 0

        Dim client_regFrequency As String = ""
        Dim client_webUserLimit As Long = 0
        Dim client_regProductCode As String = ""
        Dim client_regType As String = ""
        Dim client_regDocumentsFlag As String = ""

        Dim conn_string As String = ""
        Dim clientInfoString As String = ""

        Dim last_updated_time As Date = Nothing
        Dim last_updated_time_display As String = ""

        Dim last_updated_time2 As Date = Nothing
        Dim last_updated_time2_display As String = ""

        Dim currentUsers_display As String = ""
        Dim errorCount_display As String = ""

        Dim bHadDB_lastUpdate As Boolean = False
        Dim bHadBU_lastUpdate As Boolean = False

        Dim has_values As String = "N"
        Dim values_limit As Integer = 0
        Dim values_overall_limit As Integer = 0

        Try

            results_table = getMPMUsersDataTable(reg_status, client_reg_Type, select_type)


            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    htmlOut.Append("<div class=""Box""><table id=""mpmListingDataTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"" border=""0"" class=""formatTable blue""><thead>")

                    If select_type.ToUpper.Trim.Contains("CONN") Then

                        htmlOut.Append("<div class=""subHeader"">MPM - Client Connection List</div>")
                        htmlOut.Append("<table id=""mpmListingDataTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"" border=""0"" class=""formatTable blue""><thead>")
                        htmlOut.Append("<tr><th width=""3%""><b>ID</b></th>")
                        htmlOut.Append("<th width=""3%""><b>Reg</b></th>")
                        htmlOut.Append("<th width=""3%""><b>T</b></th>")
                        htmlOut.Append("<th width=""15%""><b>Client Name</b></th>")
                        htmlOut.Append("<th width=""20%""><b>Client Web Address</b></th>")
                        htmlOut.Append("<th width=""7%""><b># Users</b></th>")
                        htmlOut.Append("<th width=""5%""><b>Freq</b></th>")
                        htmlOut.Append("<th width=""7%""><b>Products</b></th>")
                        htmlOut.Append("<th width=""7%""><b>Aerodex</b></th>")
                        htmlOut.Append("<th width=""4%""><b>Type</b></th>")
                        htmlOut.Append("<th width=""12%""><b>DB Connn</b></th>")
                        htmlOut.Append("<th width=""18%""><b>DB Backup Conn</b></th></tr>")

                    ElseIf select_type.ToUpper.Trim.Contains("ERROR") Or select_type.ToUpper.Trim.Contains("USER") Then

                        If select_type.ToUpper.Trim.Contains("USER") Then
                            htmlOut.Append("<div class=""subHeader"">MPM - Master Client List</div>")
                        Else
                            htmlOut.Append("<div class=""subHeader"">MPM - Master Client List</div>")
                        End If
                        htmlOut.Append("<table id=""mpmListingDataTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"" border=""0"" class=""formatTable blue""><thead>")
                        htmlOut.Append("<tr><th width=""3%""><b>ID</b></th>")
                        ' htmlOut.Append("<td align=""center"" width=""3%""><b>Reg</b></td>")
                        ' htmlOut.Append("<td align=""center"" width=""3%""><b>T</b></td>")
                        htmlOut.Append("<th width=""16%""><b>Client Name</b></th>")
                        htmlOut.Append("<th width=""20%""><b>Client Web Address</b></th>")
                        htmlOut.Append("<th width=""7%""><b># Users</b></th>")

                        If select_type.ToUpper.Trim.Contains("USER") Then
                            htmlOut.Append("</th><th width='6%' align='center'><b># Conn</b>")
                        End If

                        htmlOut.Append("<th width=""5%""><b>Freq</b></th>")
                        htmlOut.Append("<th width=""7%""><b>Products</b></th>")
                        htmlOut.Append("<th width=""7%""><b>Aerodex</b></th>")
                        htmlOut.Append("<th width=""5%""><b>Doc</b></th>")
                        htmlOut.Append("<th width=""8%""><b>Client Type</b></th>")
                        htmlOut.Append("<th width=""8%""><b>Values</b></th>")
                        ' htmlOut.Append("<td align=""right"" width=""8%""><b>#&nbsp;Errors</b></td>")
                        'htmlOut.Append("<td align=""right"" width=""10%""><b>#&nbsp;Events</b></td></tr>")

                    ElseIf select_type.ToUpper.Trim.Contains("DATA") Then

                        htmlOut.Append("<div class=""subHeader"">MPM - Client Statistics</div>")
                        htmlOut.Append("<table id=""mpmListingDataTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"" border=""0"" class=""formatTable blue""><thead>")
                        htmlOut.Append("<tr><th width=""3%""><b>ID</b></th>")
                        htmlOut.Append("<th width=""3%""><b>Reg</b></th>")
                        htmlOut.Append("<th width=""3%""><b>T</b></th>")
                        htmlOut.Append("<th width=""15%""><b>Client Name</b></th>")
                        htmlOut.Append("<th width=""19%""><b>Client Web Address</b></th>")
                        htmlOut.Append("<th width=""5%""><b># Users</b></th>")
                        htmlOut.Append("<th width=""10%""><b># Client Companies</b></th>")
                        htmlOut.Append("<th width=""7%""><b># Contacts</b></th>")
                        htmlOut.Append("<th width=""5%""><b># Notes</b></th>")
                        htmlOut.Append("<th width=""6%""><b># Aircraft</b></th>")
                        htmlOut.Append("<th width=""8%""><b># Transactions</b></th></tr>")

                    End If
                    htmlOut.Append("</thead><tbody>")

                    For Each r As DataRow In results_table.Rows
                        If Not toggleRowColor Then
                            htmlOut.Append("<tr class=""alt_row"">")
                            toggleRowColor = True
                        Else
                            htmlOut.Append("<tr bgcolor=""white"">")
                            toggleRowColor = False
                        End If

                        ' registration info
                        If Not IsDBNull(r.Item("client_regName")) Then
                            If Not String.IsNullOrEmpty(r.Item("client_regName").ToString.Trim) Then
                                client_regName = r.Item("client_regName").ToString.Trim
                            End If
                        End If

                        If Not IsDBNull(r.Item("client_webHostName")) Then
                            If Not String.IsNullOrEmpty(r.Item("client_webHostName").ToString.Trim) Then
                                client_webHostName = r.Item("client_webHostName").ToString.ToUpper.Trim
                            End If
                        End If

                        If Not IsDBNull(r.Item("client_regCustomer_Type")) Then
                            If Not String.IsNullOrEmpty(r.Item("client_regCustomer_Type").ToString.Trim) Then
                                client_regCustomer_Type = r.Item("client_regCustomer_Type").ToString.Trim
                            End If
                        End If

                        If Not IsDBNull(r.Item("client_regID")) Then
                            If Not String.IsNullOrEmpty(r.Item("client_regID").ToString.Trim) Then
                                client_regID = CLng(r.Item("client_regID").ToString.Trim)
                            End If
                        End If

                        client_regAerodexFlag = IIf(r.Item("client_regAerodexFlag").ToString.Trim.ToUpper.Contains("Y"), "<img width=""20"" height=""20"" alt=""Yes"" src=""images/evo_green_check.png"">", "<img width=""20"" height=""20"" alt=""No"" src=""images/evo_red_check.png"">")

                        If Not IsDBNull(r.Item("client_regFrequency")) Then
                            If Not String.IsNullOrEmpty(r.Item("client_regFrequency").ToString.Trim) Then
                                client_regFrequency = r.Item("client_regFrequency").ToString.Trim
                            End If
                        End If

                        If Not IsDBNull(r.Item("client_webUserLimit")) Then
                            If Not String.IsNullOrEmpty(r.Item("client_webUserLimit").ToString.Trim) Then
                                client_webUserLimit = CLng(r.Item("client_webUserLimit").ToString.Trim)
                            End If
                        End If

                        If Not IsDBNull(r.Item("client_regProductCode")) Then
                            If Not String.IsNullOrEmpty(r.Item("client_regProductCode").ToString.Trim) Then
                                client_regProductCode = r.Item("client_regProductCode").ToString.Trim
                            End If
                        End If

                        If Not IsDBNull(r.Item("client_regType")) Then
                            If Not String.IsNullOrEmpty(r.Item("client_regType").ToString.Trim) Then
                                client_regType = r.Item("client_regType").ToString.Trim
                            End If
                        End If

                        client_regStatus = IIf(r.Item("client_regStatus").ToString.Trim.ToUpper.Contains("Y"), "<img width=""20"" height=""20"" alt=""Yes"" src=""images/evo_green_check.png"">", "<img width=""20"" height=""20"" alt=""No"" src=""images/evo_red_check.png"">")

                        ' client db
                        If Not IsDBNull(r.Item("client_dbHost")) Then
                            If Not String.IsNullOrEmpty(r.Item("client_dbHost").ToString.Trim) Then
                                db_host = r.Item("client_dbHost").ToString.Trim
                            End If
                        End If

                        If Not IsDBNull(r.Item("client_dbUID")) Then
                            If Not String.IsNullOrEmpty(r.Item("client_dbUID").ToString.Trim) Then
                                db_id = r.Item("client_dbUID").ToString.Trim
                            End If
                        End If

                        If Not IsDBNull(r.Item("client_dbPWD")) Then
                            If Not String.IsNullOrEmpty(r.Item("client_dbPWD").ToString.Trim) Then
                                db_pass = r.Item("client_dbPWD").ToString.Trim
                            End If
                        End If

                        If Not IsDBNull(r.Item("client_dbDatabase")) Then
                            If Not String.IsNullOrEmpty(r.Item("client_dbDatabase").ToString.Trim) Then
                                db_db = r.Item("client_dbDatabase").ToString.Trim
                            End If
                        End If


                        If Not IsDBNull(r.Item("client_reg_sale_price_flag")) Then
                            If Not String.IsNullOrEmpty(r.Item("client_reg_sale_price_flag").ToString.Trim) Then
                                has_values = r.Item("client_reg_sale_price_flag").ToString.Trim
                            End If
                        End If

                        If Not IsDBNull(r.Item("client_reg_sale_price_limit")) Then
                            If Not String.IsNullOrEmpty(r.Item("client_reg_sale_price_limit").ToString.Trim) Then
                                values_limit = r.Item("client_reg_sale_price_limit").ToString.Trim
                                values_overall_limit = values_overall_limit + values_limit
                            End If
                        End If


                        If select_type.ToUpper.Trim.Contains("CONN") Then

                            bHadDB_lastUpdate = False
                            bHadBU_lastUpdate = False

                            'backup db
                            If Not IsDBNull(r.Item("client_dbBackHost")) Then
                                If Not String.IsNullOrEmpty(r.Item("client_dbBackHost").ToString.Trim) Then
                                    bu_host = r.Item("client_dbBackHost").ToString.Trim
                                End If
                            End If

                            If Not IsDBNull(r.Item("client_dbBackUID")) Then
                                If Not String.IsNullOrEmpty(r.Item("client_dbBackUID").ToString.Trim) Then
                                    bu_id = r.Item("client_dbBackUID").ToString.Trim
                                End If
                            End If

                            If Not IsDBNull(r.Item("client_dbBackPWD")) Then
                                If Not String.IsNullOrEmpty(r.Item("client_dbBackPWD").ToString.Trim) Then
                                    bu_pass = r.Item("client_dbBackPWD").ToString.Trim
                                End If
                            End If

                            If Not IsDBNull(r.Item("client_dbBackDatabase")) Then
                                If Not String.IsNullOrEmpty(r.Item("client_dbBackDatabase").ToString.Trim) Then
                                    bu_db = r.Item("client_dbBackDatabase").ToString.Trim
                                End If
                            End If

                            If Not IsDBNull(r.Item("client_regSub_ID")) Then
                                If Not String.IsNullOrEmpty(r.Item("client_regSub_ID").ToString.Trim) Then
                                    client_reg_SubID = CLng(r.Item("client_regSub_ID").ToString.Trim)
                                End If
                            End If

                            If HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.LOCAL Then
                                If db_host.Trim.Contains("172.30.5.47") Then
                                    db_host = "jetnetcrm2.jetnet.com" ' or 192.69.4.165
                                Else
                                    db_host = "jetnetcrm.jetnet.com" ' or 192.69.4.159
                                End If
                            End If




                            conn_string = ""
                            conn_string = crmWebHostClass.generateMYSQLConnectionString(db_host, db_db, db_id, db_pass)

                            Dim tmpTimeStr = getMaxNotesDate(conn_string)

                            If IsDate(tmpTimeStr) Then
                                last_updated_time = CDate(tmpTimeStr)
                                nTotalClientConn += 1
                                last_updated_time_display = "<font color=""green"">" + tmpTimeStr.Trim + "</font>"
                                bHadDB_lastUpdate = True
                            Else
                                last_updated_time_display = "<font color=""red"">No Connection Found</font>"
                            End If

                            If HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.LOCAL Then
                                If bu_host.Trim.Contains("172.30.5.47") Then
                                    bu_host = "jetnetcrm2.jetnet.com" ' or 192.69.4.165
                                Else
                                    bu_host = "jetnetcrm.jetnet.com" ' or 192.69.4.159
                                End If
                            End If

                            conn_string = ""
                            conn_string = crmWebHostClass.generateMYSQLConnectionString(bu_host, bu_db, bu_id, bu_pass)

                            tmpTimeStr = ""
                            tmpTimeStr = getMaxNotesDate(conn_string)

                            If IsDate(tmpTimeStr) Then
                                last_updated_time2 = CDate(tmpTimeStr)
                                nTotalBackupConn += 1
                                last_updated_time2_display = "<font color=""green"">" + tmpTimeStr.Trim + "</font>"
                                bHadBU_lastUpdate = True
                            Else
                                last_updated_time2_display = "<font color=""red"">No Connection Found</font>"
                            End If

                            If bHadDB_lastUpdate And bHadBU_lastUpdate Then
                                If DateDiff("h", last_updated_time2, last_updated_time) > 24 Then
                                    last_updated_time2_display = "<font color=""red"">" + FormatDateTime(last_updated_time2, DateFormat.ShortDate) + "</font>"
                                    last_updated_time_display = "<font color=""red"">" + FormatDateTime(last_updated_time, DateFormat.ShortDate) + "</font>"
                                End If
                            End If

                            htmlOut.Append("<td align=""left"">" + client_regID.ToString.Trim + "</td>")

                            htmlOut.Append("<td align=""left"">" + client_regStatus.Trim + "</td>")
                            htmlOut.Append("<td align=""left"">" + client_regType.Trim + "</td>")

                            htmlOut.Append("<td align=""left"">" + client_regName.Trim + IIf(client_reg_SubID > 0, " (" + client_reg_SubID.ToString.Trim + ")", "") + "</td>")

                            htmlOut.Append("<td align=""left""><a class=""underline pointer"" href=""http://" + client_webHostName.Trim + """ title=""Click to go to hosting domain"">" + client_webHostName.Trim + "</a></td>")

                            htmlOut.Append("<td align=""left"">" + client_webUserLimit.ToString.Trim + "</td>")
                            htmlOut.Append("<td align=""left"">" + client_regFrequency.Trim + "</td>")
                            htmlOut.Append("<td align=""left"">" + client_regProductCode.Trim + "</td>")
                            htmlOut.Append("<td align=""left"">" + client_regAerodexFlag.Trim + "</td>")
                            htmlOut.Append("<td align=""left"">" + client_regCustomer_Type.Trim + "</td>")
                            htmlOut.Append("<td align=""left"">" + last_updated_time_display.Trim + "</td>")
                            htmlOut.Append("<td align=""left"">" + last_updated_time2_display.Trim + "</td>")

                            htmlOut.Append("</tr>")

                            nTotalUsers += client_webUserLimit
                            nTotalClients += 1

                        ElseIf select_type.ToUpper.Trim.Contains("ERROR") Or select_type.ToUpper.Trim.Contains("USER") Then

                            If Not IsDBNull(r.Item("client_regSub_ID")) Then
                                If Not String.IsNullOrEmpty(r.Item("client_regSub_ID").ToString.Trim) Then
                                    client_reg_SubID = CLng(r.Item("client_regSub_ID").ToString.Trim)
                                End If
                            End If

                            If Not IsDBNull(r.Item("errorcount")) Then
                                If Not String.IsNullOrEmpty(r.Item("errorcount").ToString.Trim) Then
                                    nErrorCount = CLng(r.Item("errorcount").ToString.Trim)
                                End If
                            End If

                            If nErrorCount > 0 Then
                                errorCount_display = "<font color=""red"">" + nErrorCount.ToString.Trim + "</font>"
                            Else
                                errorCount_display = "0"
                            End If

                            client_regDocumentsFlag = IIf(r.Item("client_regDocumentsFlag").ToString.Trim.ToUpper.Contains("Y"), "<img width=""20"" height=""20"" alt=""Yes"" src=""images/evo_green_check.png"">", "<img width=""20"" height=""20"" alt=""No"" src=""images/evo_red_check.png"">")

                            If select_type.ToUpper.Trim.Contains("USER") Then

                                If HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.LOCAL Then
                                    If db_host.Trim.Contains("172.30.5.47") Then
                                        db_host = "jetnetcrm2.jetnet.com" ' or 192.69.4.165
                                    Else
                                        db_host = "jetnetcrm.jetnet.com" ' or 192.69.4.159
                                    End If
                                End If

                                conn_string = ""
                                conn_string = crmWebHostClass.generateMYSQLConnectionString(db_host, db_db, db_id, db_pass)

                                nTotalCurrentUsers = getCurrentMPMUsers(conn_string)

                                If nTotalCurrentUsers > 0 Then
                                    currentUsers_display = "<font color=""green""><b>" + nTotalCurrentUsers.ToString.Trim + "</b></font>"
                                Else
                                    currentUsers_display = "0"
                                End If

                            End If

                            nDomainErrorCount = getCurrentMPMErrorCountForHost(client_webHostName)
                            nDomainUsersCount = getCurrentMPMUsersFromMaster(client_regID)

                            htmlOut.Append("<td align=""left"">" + client_regID.ToString.Trim + "</td>")

                            '  htmlOut.Append("<td align=""left"">" + client_regStatus.Trim + "</td>")
                            '   htmlOut.Append("<td align=""left"">" + client_regType.Trim + "</td>")

                            htmlOut.Append("<td align=""left"">" + client_regName.Trim + IIf(client_reg_SubID > 0, " (" + client_reg_SubID.ToString.Trim + ")", "") + "</td>")

                            htmlOut.Append("<td align=""left""><a class=""underline pointer"" href=""http://" + client_webHostName.Trim + """ title=""Click to go to hosting domain"">" + client_webHostName.Trim + "</a></td>")

                            htmlOut.Append("<td align=""left"">" + IIf(nDomainUsersCount > 0, "<a class=""underline pointer"" href=""adminMPM.aspx?show_current_domain_users=true&id=" + client_regID.ToString + """ title=""Click to see ONLINE domain users"">" + nDomainUsersCount.ToString.Trim + "</a>", "0"))
                            htmlOut.Append("&nbsp;/&nbsp;<a class=""underline pointer"" href=""adminMPM.aspx?show_domain_users=true&id=" + client_regID.ToString + "&users=" + client_webUserLimit.ToString.Trim + """ title=""Click to see ALL domain users"">" + client_webUserLimit.ToString.Trim + "</a></td>")

                            If select_type.ToUpper.Trim.Contains("USER") Then
                                htmlOut.Append("<td align=""left"">" + IIf(nTotalCurrentUsers > 0, "<a class=""underline pointer"" href=""adminMPM.aspx?show_current_domain_users=true&id=" + client_regID.ToString + """ title=""Click to see ALL domain users"">" + currentUsers_display.Trim + "</a>", currentUsers_display.Trim) + "</td>")
                            End If

                            htmlOut.Append("<td align=""left"">" + client_regFrequency.Trim + "</td>")
                            htmlOut.Append("<td align=""left"">" + client_regProductCode.Trim + "</td>")
                            htmlOut.Append("<td align=""left"">" + client_regAerodexFlag.Trim + "</td>")
                            htmlOut.Append("<td align=""left"">" + client_regDocumentsFlag.Trim + "</td>")
                            htmlOut.Append("<td align=""left"">" + client_regCustomer_Type.Trim + "</td>")


                            If has_values = "Y" Then
                                htmlOut.Append("<td align=""right"">" + values_limit.ToString + "</td>")
                            Else
                                htmlOut.Append("<td align=""right"">-</td>")
                            End If

                            ' htmlOut.Append("<td align=""right"">" + IIf(nErrorCount > 0, "<a class=""underline pointer"" href=""adminMPM.aspx?error_display=true&id=" + client_regID.ToString + "&domain=" + client_webHostName.Trim + """ title=""Click to see CURRENT domain errors"">" + errorCount_display.Trim + "</a>", errorCount_display.Trim) + "</td>")
                            '  htmlOut.Append("<td align=""right"">" + IIf(nDomainErrorCount > 0, "<a class=""underline pointer"" href=""adminMPM.aspx?export_display=true&domain=" + client_webHostName.Trim + """ title=""Click to see ALL domain errors"">" + nDomainErrorCount.ToString.Trim + "</a>", "0") + "</td>")

                            htmlOut.Append("</tr>")

                            nTotalErrors += nErrorCount
                            nTotalMPMUsers += nTotalCurrentUsers
                            nTotalDomainUsers += nDomainUsersCount
                            nTotalUsers += client_webUserLimit
                            nTotalClients += 1

                        ElseIf select_type.ToUpper.Trim.Contains("DATA") Then

                            If HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.LOCAL Then
                                If db_host.Trim.Contains("172.30.5.47") Then
                                    db_host = "jetnetcrm2.jetnet.com" ' or 192.69.4.165
                                Else
                                    db_host = "jetnetcrm.jetnet.com" ' or 192.69.4.159
                                End If
                            End If

                            conn_string = ""
                            conn_string = crmWebHostClass.generateMYSQLConnectionString(db_host, db_db, db_id, db_pass)

                            clientInfoString = ""
                            clientInfoString = getMPMClientDisplayData(conn_string)

                            htmlOut.Append("<td align=""left"">" + client_regID.ToString.Trim + "</td>")

                            htmlOut.Append("<td align=""left"">" + client_regStatus.Trim + "</td>")
                            htmlOut.Append("<td align=""left"">" + client_regType.Trim + "</td>")

                            htmlOut.Append("<td align=""left"">" + client_regName.Trim + IIf(client_reg_SubID > 0, " (" + client_reg_SubID.ToString.Trim + ")", "") + "</td>")

                            htmlOut.Append("<td align=""left""><a class=""underline pointer"" href=""http://" + client_webHostName.Trim + """ title=""Click to go to hosting domain"">" + client_webHostName.Trim + "</a></td>")

                            htmlOut.Append("<td align=""left"">" + client_webUserLimit.ToString.Trim + "</td>")

                            htmlOut.Append(clientInfoString)

                            htmlOut.Append("</tr>")

                            nTotalUsers += client_webUserLimit
                            nTotalClients += 1

                        End If

                    Next

                    If Not toggleRowColor Then
                        htmlOut.Append("<tr class=""alt_row"">")
                        toggleRowColor = True
                    Else
                        htmlOut.Append("<tr bgcolor=""white"">")
                        toggleRowColor = False
                    End If

                    If select_type.ToUpper.Trim.Contains("CONN") Then

                        htmlOut.Append("<td colspan=""3"" align=""left""><b>Total Clients: </b></td>")
                        htmlOut.Append("<td align=""left"">" + IIf(nTotalClients > 0, nTotalClients.ToString.Trim, "0") + "</td>")
                        htmlOut.Append("<td align=""left""><b>Total Users: </b></td>")
                        htmlOut.Append("<td align=""left"">" + IIf(nTotalUsers > 0, nTotalUsers.ToString.Trim, "0") + "</td>")
                        htmlOut.Append("<td colspan=""4"" align=""left""><b>Total Connections: </b></td>")
                        htmlOut.Append("<td align=""left"">" + IIf(nTotalClientConn > 0, nTotalClientConn.ToString.Trim, "0") + "</td>")
                        htmlOut.Append("<td align=""left"">" + IIf(nTotalBackupConn > 0, nTotalBackupConn.ToString.Trim, "0") + "</td>")

                    ElseIf select_type.ToUpper.Trim.Contains("ERROR") Or select_type.ToUpper.Trim.Contains("USER") Then

                        htmlOut.Append("<td colspan=""2"" align=""left""><b>Total Clients: </b>" & IIf(nTotalClients > 0, nTotalClients.ToString.Trim, "0") & "</td>")
                        htmlOut.Append("<td align=""left""><b>Users: Total/Active </b></td>")
                        htmlOut.Append("<td align=""left"">" + IIf(nTotalDomainUsers > 0, "<a class=""underline pointer"" href=""adminMPM.aspx?show_current_domain_users=true"">" + nTotalDomainUsers.ToString.Trim + "</a>", "0"))
                        htmlOut.Append("&nbsp;/&nbsp;" + IIf(nTotalUsers > 0, nTotalUsers.ToString.Trim, "0") + "</td>")

                        If select_type.ToUpper.Trim.Contains("USER") Then
                            htmlOut.Append("<td align=""left"">" + IIf(nTotalMPMUsers > 0, nTotalMPMUsers.ToString.Trim, "0") + "</td>")
                        End If

                        htmlOut.Append("<td colspan=""2"" align=""left""></td>")

                        htmlOut.Append("<td align=""left""><b>Total Errors: </b></td>")
                        htmlOut.Append("<td align=""center"">" + IIf(nTotalErrors > 0, nTotalErrors.ToString.Trim, "0") + "</td>")
                        htmlOut.Append("<td align=""left""><b>Total Users: </b></td>")
                        htmlOut.Append("<td align=""center"">" & values_overall_limit & "</td>")

                    ElseIf select_type.ToUpper.Trim.Contains("DATA") Then

                        htmlOut.Append("<td colspan=""3"" align=""left""><b>Total Clients: </b></td>")
                        htmlOut.Append("<td align=""left"">" + IIf(nTotalClients > 0, nTotalClients.ToString.Trim, "0") + "</td>")
                        htmlOut.Append("<td align=""left""><b>Total of Client Data: </b></td>")
                        htmlOut.Append("<td align=""left"">" + IIf(nTotalUsers > 0, nTotalUsers.ToString.Trim, "0") + "</td>")
                        htmlOut.Append("<td colspan=""5"" align=""left""></td>")

                    End If

                    htmlOut.Append("</tr>")

                    htmlOut.Append("</tbody></table></div>")

                Else
                    htmlOut.Append("<div class=""Box""><table id=""mpmListingDataTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"" class=""formatTable blue""><tr><td valign=""top"" align=""left""><br/>No MPM Data Found</td></tr></table></div>")
                End If
            Else
                htmlOut.Append("<div class=""Box""><table id=""mpmListingDataTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"" class=""formatTable blue""><tr><td valign=""top"" align=""left""><br/>No MPM Data Found</td></tr></table></div>")
            End If

        Catch ex As Exception

            aError = "Error in displayMPMUsers(ByVal reg_status As String, ByVal client_reg_Type As String, ByVal select_type As String, ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub

    Public Function getMaxNotesDate(ByVal inConnectionStr As String) As String

        Dim sQuery = New StringBuilder()

        Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
        Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
        Dim MySqlReader As MySql.Data.MySqlClient.MySqlDataReader = Nothing
        Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing

        Dim sMaxNotesDate As String = ""

        Try

            sQuery.Append("SELECT MAX(lnote_entry_date) AS maxDate FROM local_notes WHERE lnote_status = 'A'")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />getMaxNotesDate(ByVal inConnectionStr As String) As String</b><br />" + sQuery.ToString

            MySqlConn.ConnectionString = inConnectionStr
            MySqlConn.Open()
            MySqlCommand.Connection = MySqlConn
            MySqlCommand.CommandType = CommandType.Text
            MySqlCommand.CommandTimeout = 240

            MySqlCommand.CommandText = sQuery.ToString

            MySqlReader = MySqlCommand.ExecuteReader()

            If MySqlReader.HasRows Then

                MySqlReader.Read()

                If Not (IsDBNull(MySqlReader("maxDate"))) Then
                    If Not String.IsNullOrEmpty(MySqlReader.Item("maxDate").ToString.Trim) Then
                        sMaxNotesDate = FormatDateTime(MySqlReader.Item("maxDate").ToString.Trim, DateFormat.ShortDate)
                    End If

                End If

            End If

            MySqlReader.Close()

        Catch ex As Exception
            aError = "Error in getMaxNotesDate(ByVal inConnectionStr As String) As String" + ex.Message
        Finally
            MySqlReader = Nothing

            MySqlConn.Dispose()
            MySqlConn.Close()
            MySqlConn = Nothing

            MySqlCommand.Dispose()
            MySqlCommand = Nothing
        End Try

        Return sMaxNotesDate

    End Function

    Public Function getMPMDomainDataConnection(ByVal nDomainID As Long, ByRef sHostName As String, Optional ByRef has_values As String = "", Optional ByRef values_limit As Integer = 0, Optional ByRef web_user_limit As Integer = 0) As String

        Dim sQuery = New StringBuilder()

        Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
        Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
        Dim MySqlReader As MySql.Data.MySqlClient.MySqlDataReader = Nothing
        Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing

        Dim db_host As String = ""
        Dim db_id As String = ""
        Dim db_pass As String = ""
        Dim db_db As String = ""

        Dim conn_string As String = ""

        Try

            sQuery.Append("SELECT client_dbHost, client_dbUID, client_dbPWD, client_dbDatabase, client_webHostName ,client_reg_sale_price_flag, client_reg_sale_price_limit, client_webUserLimit")
            sQuery.Append(" FROM client_register_master WHERE client_regID = " + nDomainID.ToString)


            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />getDomainDataConnection(ByVal nDomainID As Long, ByRef sHostName As String) As String</b><br />" + sQuery.ToString

            MySqlConn.ConnectionString = crmMasterConnectStr
            MySqlConn.Open()
            MySqlCommand.Connection = MySqlConn
            MySqlCommand.CommandType = CommandType.Text
            MySqlCommand.CommandTimeout = 240

            MySqlCommand.CommandText = sQuery.ToString

            MySqlReader = MySqlCommand.ExecuteReader()

            If MySqlReader.HasRows Then

                MySqlReader.Read()

                If Not IsDBNull(MySqlReader.Item("client_webHostName")) Then
                    If Not String.IsNullOrEmpty(MySqlReader.Item("client_webHostName").ToString.Trim) Then
                        sHostName = MySqlReader.Item("client_webHostName").ToString.Trim
                    End If
                End If

                ' client db
                If Not IsDBNull(MySqlReader.Item("client_dbHost")) Then
                    If Not String.IsNullOrEmpty(MySqlReader.Item("client_dbHost").ToString.Trim) Then
                        db_host = MySqlReader.Item("client_dbHost").ToString.Trim
                    End If
                End If

                If Not IsDBNull(MySqlReader.Item("client_dbUID")) Then
                    If Not String.IsNullOrEmpty(MySqlReader.Item("client_dbUID").ToString.Trim) Then
                        db_id = MySqlReader.Item("client_dbUID").ToString.Trim
                    End If
                End If

                If Not IsDBNull(MySqlReader.Item("client_dbPWD")) Then
                    If Not String.IsNullOrEmpty(MySqlReader.Item("client_dbPWD").ToString.Trim) Then
                        db_pass = MySqlReader.Item("client_dbPWD").ToString.Trim
                    End If
                End If

                If Not IsDBNull(MySqlReader.Item("client_dbDatabase")) Then
                    If Not String.IsNullOrEmpty(MySqlReader.Item("client_dbDatabase").ToString.Trim) Then
                        db_db = MySqlReader.Item("client_dbDatabase").ToString.Trim
                    End If
                End If

                If Not IsDBNull(MySqlReader.Item("client_reg_sale_price_flag")) Then
                    If Not String.IsNullOrEmpty(MySqlReader.Item("client_reg_sale_price_flag").ToString.Trim) Then
                        has_values = MySqlReader.Item("client_reg_sale_price_flag").ToString.Trim
                    End If
                End If

                If Not IsDBNull(MySqlReader.Item("client_reg_sale_price_limit")) Then
                    If Not String.IsNullOrEmpty(MySqlReader.Item("client_reg_sale_price_limit").ToString.Trim) Then
                        values_limit = MySqlReader.Item("client_reg_sale_price_limit").ToString.Trim
                    End If
                End If

                If Not IsDBNull(MySqlReader.Item("client_webUserLimit")) Then
                    If Not String.IsNullOrEmpty(MySqlReader.Item("client_webUserLimit").ToString.Trim) Then
                        web_user_limit = MySqlReader.Item("client_webUserLimit").ToString.Trim
                    End If
                End If

                If HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.LOCAL Then
                    If db_host.Trim.Contains("172.30.5.47") Then
                        db_host = "jetnetcrm2.jetnet.com" ' or 192.69.4.165
                    Else
                        db_host = "jetnetcrm.jetnet.com" ' or 192.69.4.159
                    End If
                End If

                conn_string = crmWebHostClass.generateMYSQLConnectionString(db_host, db_db, db_id, db_pass)

            End If

            MySqlReader.Close()

        Catch ex As Exception
            aError = "Error in getDomainDataConnection(ByVal nDomainID As Long, ByRef sHostName As String) As String" + ex.Message
        Finally
            MySqlReader = Nothing

            MySqlConn.Dispose()
            MySqlConn.Close()
            MySqlConn = Nothing

            MySqlCommand.Dispose()
            MySqlCommand = Nothing
        End Try

        Return conn_string

    End Function

    Public Function getCurrentMPMUsers(ByVal inConnectionStr As String) As Long

        Dim sQuery = New StringBuilder()

        Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
        Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
        Dim MySqlReader As MySql.Data.MySqlClient.MySqlDataReader = Nothing
        Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing

        Dim nUserCount As Long = 0

        Try

            sQuery.Append("SELECT DISTINCT count(*) AS tcount FROM client_user")
            sQuery.Append(" WHERE cliuser_last_session_date > '" + Format(DateAdd("n", -10, Now()), "yyyy-MM-dd H:mm:ss").Trim + "'")
            sQuery.Append(" AND ((cliuser_last_session_date <> cliuser_last_logout_date) OR (cliuser_last_logout_date is NULL))")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />getCurrentMPMUsers(ByVal inConnectionStr As String) As String</b><br />" + sQuery.ToString

            MySqlConn.ConnectionString = inConnectionStr
            MySqlConn.Open()
            MySqlCommand.Connection = MySqlConn
            MySqlCommand.CommandType = CommandType.Text
            MySqlCommand.CommandTimeout = 240

            MySqlCommand.CommandText = sQuery.ToString

            MySqlReader = MySqlCommand.ExecuteReader()

            If MySqlReader.HasRows Then

                MySqlReader.Read()

                If Not (IsDBNull(MySqlReader("tcount"))) Then
                    If Not String.IsNullOrEmpty(MySqlReader.Item("tcount").ToString.Trim) Then
                        nUserCount = CLng(MySqlReader.Item("tcount").ToString.Trim)
                    End If

                End If

            End If

            MySqlReader.Close()

        Catch ex As Exception
            aError = "Error in getCurrentMPMUsers(ByVal inConnectionStr As String) As String" + ex.Message
        Finally
            MySqlReader = Nothing

            MySqlConn.Dispose()
            MySqlConn.Close()
            MySqlConn = Nothing

            MySqlCommand.Dispose()
            MySqlCommand = Nothing
        End Try

        Return nUserCount

    End Function

    Public Function getCurrentMPMErrorCountForHost(ByVal inHostName As String) As Long

        Dim sQuery = New StringBuilder()

        Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
        Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
        Dim MySqlReader As MySql.Data.MySqlClient.MySqlDataReader = Nothing
        Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing

        Dim nErrorCount As Long = 0

        Try

            sQuery.Append("SELECT DISTINCT count(*) AS tcount FROM client_event_log")
            sQuery.Append(" WHERE clievent_location  = '" + inHostName.Trim + "'")
            sQuery.Append(" AND clievent_type <> 'LOGIN'")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />getCurrentMPMErrorCountForHost(ByVal inHostName As String) As Long</b><br />" + sQuery.ToString

            MySqlConn.ConnectionString = crmMasterConnectStr
            MySqlConn.Open()
            MySqlCommand.Connection = MySqlConn
            MySqlCommand.CommandType = CommandType.Text
            MySqlCommand.CommandTimeout = 240

            MySqlCommand.CommandText = sQuery.ToString

            MySqlReader = MySqlCommand.ExecuteReader()

            If MySqlReader.HasRows Then

                MySqlReader.Read()

                If Not (IsDBNull(MySqlReader("tcount"))) Then
                    If Not String.IsNullOrEmpty(MySqlReader.Item("tcount").ToString.Trim) Then
                        nErrorCount = CLng(MySqlReader.Item("tcount").ToString.Trim)
                    End If

                End If

            End If

            MySqlReader.Close()

        Catch ex As Exception
            aError = "Error in getCurrentMPMErrorCountForHost(ByVal inHostName As String) As Long" + ex.Message
        Finally
            MySqlReader = Nothing

            MySqlConn.Dispose()
            MySqlConn.Close()
            MySqlConn = Nothing

            MySqlCommand.Dispose()
            MySqlCommand = Nothing
        End Try

        Return nErrorCount

    End Function

    Public Function getCurrentMPMUsersFromMaster(ByVal inClient_RegID As Long) As Long

        Dim sQuery = New StringBuilder()

        Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
        Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
        Dim MySqlReader As MySql.Data.MySqlClient.MySqlDataReader = Nothing
        Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing

        Dim nUserCount As Long = 0

        Try

            sQuery.Append("SELECT DISTINCT count(*) AS tcount FROM client_user WHERE cliuser_client_regid = " + inClient_RegID.ToString)
            sQuery.Append(" AND cliuser_last_session_date > '" + Format(DateAdd("n", -10, Now()), "yyyy-MM-dd H:mm:ss").Trim + "'")
            sQuery.Append(" AND ((cliuser_last_session_date <> cliuser_last_logout_date) OR (cliuser_last_logout_date is NULL))")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />getCurrentMPMUsersFromMaster(ByVal inClient_RegID As Long) As Long</b><br />" + sQuery.ToString

            MySqlConn.ConnectionString = crmMasterConnectStr
            MySqlConn.Open()
            MySqlCommand.Connection = MySqlConn
            MySqlCommand.CommandType = CommandType.Text
            MySqlCommand.CommandTimeout = 240

            MySqlCommand.CommandText = sQuery.ToString

            MySqlReader = MySqlCommand.ExecuteReader()

            If MySqlReader.HasRows Then

                MySqlReader.Read()

                If Not (IsDBNull(MySqlReader("tcount"))) Then
                    If Not String.IsNullOrEmpty(MySqlReader.Item("tcount").ToString.Trim) Then
                        nUserCount = CLng(MySqlReader.Item("tcount").ToString.Trim)
                    End If

                End If

            End If

            MySqlReader.Close()

        Catch ex As Exception
            aError = "Error in getCurrentMPMUsersFromMaster(ByVal inClient_RegID As Long) As Long" + ex.Message
        Finally
            MySqlReader = Nothing

            MySqlConn.Dispose()
            MySqlConn.Close()
            MySqlConn = Nothing

            MySqlCommand.Dispose()
            MySqlCommand = Nothing
        End Try

        Return nUserCount

    End Function

    Public Function getMPMClientDisplayData(ByVal inConnectionStr As String) As String

        Dim sQuery = New StringBuilder()

        Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
        Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
        Dim MySqlReader As MySql.Data.MySqlClient.MySqlDataReader = Nothing
        Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing

        Dim htmlOut As New StringBuilder

        Try

            MySqlConn.ConnectionString = inConnectionStr
            MySqlConn.Open()
            MySqlCommand.Connection = MySqlConn
            MySqlCommand.CommandType = CommandType.Text
            MySqlCommand.CommandTimeout = 240

            sQuery.Append("SELECT count(*) AS tcount FROM client_company")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />getClientDisplayData(ByVal inConnectionStr As String) As String</b><br />" + sQuery.ToString

            MySqlCommand.CommandText = sQuery.ToString
            MySqlReader = MySqlCommand.ExecuteReader()

            If MySqlReader.HasRows Then

                MySqlReader.Read()

                If Not (IsDBNull(MySqlReader("tcount"))) Then
                    If Not String.IsNullOrEmpty(MySqlReader.Item("tcount").ToString.Trim) Then
                        htmlOut.Append("<td align=""right"">" + MySqlReader.Item("tcount").ToString.Trim + "</td>")
                    Else
                        htmlOut.Append("<td align=""right"">0</td>")
                    End If
                Else
                    htmlOut.Append("<td align=""right"">0</td>")
                End If
            Else
                htmlOut.Append("<td align=""right"">0</td>")
            End If

            MySqlReader.Close()

            sQuery = New StringBuilder
            sQuery.Append("SELECT count(*) AS tcount FROM client_contact")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />getClientDisplayData(ByVal inConnectionStr As String) As String</b><br />" + sQuery.ToString

            MySqlCommand.CommandText = sQuery.ToString
            MySqlReader = MySqlCommand.ExecuteReader()

            If MySqlReader.HasRows Then

                MySqlReader.Read()

                If Not (IsDBNull(MySqlReader("tcount"))) Then
                    If Not String.IsNullOrEmpty(MySqlReader.Item("tcount").ToString.Trim) Then
                        htmlOut.Append("<td align=""right"">" + MySqlReader.Item("tcount").ToString.Trim + "</td>")
                    Else
                        htmlOut.Append("<td align=""right"">0</td>")
                    End If
                Else
                    htmlOut.Append("<td align=""right"">0</td>")
                End If
            Else
                htmlOut.Append("<td align=""right"">0</td>")
            End If

            MySqlReader.Close()

            sQuery = New StringBuilder
            sQuery.Append("SELECT count(*) AS tcount FROM local_notes")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />getClientDisplayData(ByVal inConnectionStr As String) As String</b><br />" + sQuery.ToString

            MySqlCommand.CommandText = sQuery.ToString
            MySqlReader = MySqlCommand.ExecuteReader()

            If MySqlReader.HasRows Then

                MySqlReader.Read()

                If Not (IsDBNull(MySqlReader("tcount"))) Then
                    If Not String.IsNullOrEmpty(MySqlReader.Item("tcount").ToString.Trim) Then
                        htmlOut.Append("<td align=""right"">" + MySqlReader.Item("tcount").ToString.Trim + "</td>")
                    Else
                        htmlOut.Append("<td align=""right"">0</td>")
                    End If
                Else
                    htmlOut.Append("<td align=""right"">0</td>")
                End If
            Else
                htmlOut.Append("<td align=""right"">0</td>")
            End If

            MySqlReader.Close()

            sQuery = New StringBuilder
            sQuery.Append("SELECT count(*) AS tcount FROM client_aircraft")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />getClientDisplayData(ByVal inConnectionStr As String) As String</b><br />" + sQuery.ToString

            MySqlCommand.CommandText = sQuery.ToString
            MySqlReader = MySqlCommand.ExecuteReader()

            If MySqlReader.HasRows Then

                MySqlReader.Read()

                If Not (IsDBNull(MySqlReader("tcount"))) Then
                    If Not String.IsNullOrEmpty(MySqlReader.Item("tcount").ToString.Trim) Then
                        htmlOut.Append("<td align=""right"">" + MySqlReader.Item("tcount").ToString.Trim + "</td>")
                    Else
                        htmlOut.Append("<td align=""right"">0</td>")
                    End If
                Else
                    htmlOut.Append("<td align=""right"">0</td>")
                End If
            Else
                htmlOut.Append("<td align=""right"">0</td>")
            End If

            MySqlReader.Close()

            sQuery = New StringBuilder
            sQuery.Append("SELECT count(*) AS tcount FROM client_transactions")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />getClientDisplayData(ByVal inConnectionStr As String) As String</b><br />" + sQuery.ToString

            MySqlCommand.CommandText = sQuery.ToString
            MySqlReader = MySqlCommand.ExecuteReader()

            If MySqlReader.HasRows Then

                MySqlReader.Read()

                If Not (IsDBNull(MySqlReader("tcount"))) Then
                    If Not String.IsNullOrEmpty(MySqlReader.Item("tcount").ToString.Trim) Then
                        htmlOut.Append("<td align=""right"">" + MySqlReader.Item("tcount").ToString.Trim + "</td>")
                    Else
                        htmlOut.Append("<td align=""right"">0</td>")
                    End If
                Else
                    htmlOut.Append("<td align=""right"">0</td>")
                End If
            Else
                htmlOut.Append("<td align=""right"">0</td>")
            End If

            MySqlReader.Close()

        Catch ex As Exception
            aError = "Error in getClientDisplayData(ByVal inConnectionStr As String) As String" + ex.Message
        Finally
            MySqlReader = Nothing

            MySqlConn.Dispose()
            MySqlConn.Close()
            MySqlConn = Nothing

            MySqlCommand.Dispose()
            MySqlCommand = Nothing
        End Try

        Return htmlOut.ToString

        htmlOut = Nothing

    End Function

    Public Function getCurrentMPMDomainUsersDataTable(ByVal nDomainID As Long) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
        Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
        Dim MySqlReader As MySql.Data.MySqlClient.MySqlDataReader = Nothing
        Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing

        Try


            sQuery.Append("SELECT DISTINCT client_user.*, client_regName FROM client_user")
            sQuery.Append(" INNER JOIN client_register_master ON client_regID = cliuser_client_regid")
            sQuery.Append(" WHERE cliuser_last_session_date > '" + Format(DateAdd(DateInterval.Minute, -10, Now()), "yyyy-MM-dd H:mm:ss").Trim + "'")
            sQuery.Append(" AND ((cliuser_last_session_date <> cliuser_last_logout_date) OR (cliuser_last_logout_date is NULL))")

            If nDomainID > 0 Then
                sQuery.Append(Constants.cAndClause + "cliuser_client_regid = " + nDomainID.ToString)
            End If

            sQuery.Append(" ORDER BY client_regName, cliuser_last_name ASC")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>getCurrentDomainUsersDataTable(ByVal nDomainID As Long) As DataTable</b><br />" + sQuery.ToString

            MySqlConn.ConnectionString = crmMasterConnectStr

            MySqlConn.Open()
            MySqlCommand.Connection = MySqlConn
            MySqlCommand.CommandType = CommandType.Text
            MySqlCommand.CommandTimeout = 240

            MySqlCommand.CommandText = sQuery.ToString

            MySqlReader = MySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                atemptable.Load(MySqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getCurrentDomainUsersDataTable load datatable</b><br /> " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getCurrentDomainUsersDataTable(ByVal nDomainID As Long) As DataTable</b><br />" + ex.Message

        Finally
            MySqlReader = Nothing

            MySqlConn.Dispose()
            MySqlConn.Close()
            MySqlConn = Nothing

            MySqlCommand.Dispose()
            MySqlCommand = Nothing
        End Try

        Return atemptable

    End Function

    Public Sub displayCurrentMPMDomainUsers(ByVal nDomainID As Long, ByRef out_htmlString As String)

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False

        Dim client_regName As String = ""
        Dim client_userId As Long = 0
        Dim client_sessionGuid As String = ""
        Dim client_firstName As String = ""
        Dim client_lastName As String = ""
        Dim client_login As String = ""
        Dim client_email As String = ""
        Dim client_lastSessionDate As String = ""

        Try

            results_table = getCurrentMPMDomainUsersDataTable(nDomainID)

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    htmlOut.Append("<table id=""mpmCurrentDomainUserListingDataTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"" border=""1"">")

                    htmlOut.Append("<tr><td colspan=""8"" align=""center""><b>Domain - Current User List</b></td></tr>")
                    htmlOut.Append("<tr><td align=""left""><b>Company Name</b></td>")
                    htmlOut.Append("<td align=""center""><b>ID</b></td>")
                    htmlOut.Append("<td align=""center""><b>GUID</b></td>")
                    htmlOut.Append("<td align=""center""><b>First Name</b></td>")
                    htmlOut.Append("<td align=""center""><b>Last Name</b></td>")
                    htmlOut.Append("<td align=""center""><b>Login</b></td>")
                    htmlOut.Append("<td align=""center""><b>Email</b></td>")
                    htmlOut.Append("<td align=""center""><b>Last Session Date</b></td></tr>")

                    For Each r As DataRow In results_table.Rows

                        If Not toggleRowColor Then
                            htmlOut.Append("<tr class=""alt_row"">")
                            toggleRowColor = True
                        Else
                            htmlOut.Append("<tr bgcolor=""white"">")
                            toggleRowColor = False
                        End If

                        If Not IsDBNull(r.Item("client_regName")) Then
                            If Not String.IsNullOrEmpty(r.Item("client_regName").ToString.Trim) Then
                                client_regName = r.Item("client_regName").ToString.Trim
                            End If
                        End If

                        If Not IsDBNull(r.Item("cliuser_id")) Then
                            If Not String.IsNullOrEmpty(r.Item("cliuser_id").ToString.Trim) Then
                                client_userId = CLng(r.Item("cliuser_id").ToString.Trim)
                            End If
                        End If

                        If Not IsDBNull(r.Item("cliuser_session_guid")) Then
                            If Not String.IsNullOrEmpty(r.Item("cliuser_session_guid").ToString.Trim) Then
                                client_sessionGuid = r.Item("cliuser_session_guid").ToString.Trim
                            End If
                        End If

                        If Not IsDBNull(r.Item("cliuser_first_name")) Then
                            If Not String.IsNullOrEmpty(r.Item("cliuser_first_name").ToString.Trim) Then
                                client_firstName = r.Item("cliuser_first_name").ToString.Trim
                            End If
                        End If

                        If Not IsDBNull(r.Item("cliuser_last_name")) Then
                            If Not String.IsNullOrEmpty(r.Item("cliuser_last_name").ToString.Trim) Then
                                client_lastName = r.Item("cliuser_last_name").ToString.Trim
                            End If
                        End If

                        If Not IsDBNull(r.Item("cliuser_login")) Then
                            If Not String.IsNullOrEmpty(r.Item("cliuser_login").ToString.Trim) Then
                                client_login = r.Item("cliuser_login").ToString.Trim
                            End If
                        End If

                        If Not IsDBNull(r.Item("cliuser_email_address")) Then
                            If Not String.IsNullOrEmpty(r.Item("cliuser_email_address").ToString.Trim) Then
                                client_email = r.Item("cliuser_email_address").ToString.Trim
                            End If
                        End If

                        If Not IsDBNull(r.Item("cliuser_last_session_date")) Then
                            If Not String.IsNullOrEmpty(r.Item("cliuser_last_session_date").ToString.Trim) Then
                                client_lastSessionDate = FormatDateTime(r.Item("cliuser_last_session_date").ToString.Trim, DateFormat.ShortDate)
                            End If
                        End If

                        htmlOut.Append("<td align=""left"">" + client_regName.Trim + "</td>")
                        htmlOut.Append("<td align=""left"">" + client_userId.ToString.Trim + "</td>")
                        htmlOut.Append("<td align=""left"">" + client_sessionGuid.Trim + "</td>")
                        htmlOut.Append("<td align=""left"">" + client_firstName.Trim + "</td>")
                        htmlOut.Append("<td align=""left"">" + client_lastName.Trim + "</td>")
                        htmlOut.Append("<td align=""left"">" + client_login.Trim + "</td>")
                        htmlOut.Append("<td align=""left"">" + client_email.Trim + "</td>")
                        htmlOut.Append("<td align=""center"">" + client_lastSessionDate.Trim + "</td>")

                        htmlOut.Append("</tr>")

                    Next

                    htmlOut.Append("</table>")

                Else
                    htmlOut.Append("<table id=""mpmCurrentDomainUserListingDataTable"" width=""100%"" cellpadding=""2"" cellspacing=""0""><tr><td valign=""top"" align=""left""><br/>No Domain Users Found</td></tr></table>")
                End If
            Else
                htmlOut.Append("<table id=""mpmCurrentDomainUserListingDataTable"" width=""100%"" cellpadding=""2"" cellspacing=""0""><tr><td valign=""top"" align=""left""><br/>No Domain Users Found</td></tr></table>")
            End If

        Catch ex As Exception

            aError = "Error in displayCurrentDomainUsers(ByVal nDomainID As Long, ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub

    Public Function getMPMDomainUsersDataTable(ByVal inConnectionStr As String, Optional ByVal nUserID As Long = 0, Optional ByVal user_types_to_show As String = "") As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
        Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
        Dim MySqlReader As MySql.Data.MySqlClient.MySqlDataReader = Nothing
        Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing

        Try

            sQuery.Append("SELECT * FROM Client_User")

            If nUserID > 0 Then
                sQuery.Append(" WHERE cliuser_id = " + nUserID.ToString)
                If Trim(user_types_to_show) = "All" Then

                ElseIf Trim(user_types_to_show) = "Active" Then
                    sQuery.Append(" and cliuser_active_flag = 'Y' and cliuser_password <> '' and cliuser_password is not null ")
                Else
                    sQuery.Append(" and cliuser_active_flag = 'Y' and cliuser_password <> '' and cliuser_password is not null ")
                End If
            Else
                If Trim(user_types_to_show) = "All" Then

                ElseIf Trim(user_types_to_show) = "Active" Then
                    sQuery.Append(" Where cliuser_active_flag = 'Y' and cliuser_password <> '' and cliuser_password is not null ")
                Else
                    sQuery.Append(" Where cliuser_active_flag = 'Y' and cliuser_password <> '' and cliuser_password is not null ")
                End If
            End If





            sQuery.Append(" ORDER BY cliuser_active_flag DESC, cliuser_last_name ASC, cliuser_first_name ASC")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>getDomainUsersDataTable(ByVal inConnectionStr As String, Optional ByVal nUserID As Long = 0) As DataTable</b><br />" + sQuery.ToString

            MySqlConn.ConnectionString = inConnectionStr

            MySqlConn.Open()
            MySqlCommand.Connection = MySqlConn
            MySqlCommand.CommandType = CommandType.Text
            MySqlCommand.CommandTimeout = 240

            MySqlCommand.CommandText = sQuery.ToString

            MySqlReader = MySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                atemptable.Load(MySqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getDomainUsersDataTable load datatable</b><br /> " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getDomainUsersDataTable(ByVal inConnectionStr As String, Optional ByVal nUserID As Long = 0) As DataTable</b><br />" + ex.Message

        Finally
            MySqlReader = Nothing

            MySqlConn.Dispose()
            MySqlConn.Close()
            MySqlConn = Nothing

            MySqlCommand.Dispose()
            MySqlCommand = Nothing
        End Try

        Return atemptable

    End Function

    Public Sub displayMPMDomainUsers(ByVal nDomainID As Long, ByRef out_htmlString As String, ByRef addNewClientUser As LinkButton, ByVal users As String, Optional ByVal user_types_to_show As String = "", Optional ByRef user_info As String = "", Optional ByRef current_users_count As String = "", Optional ByRef is_homebase As String = "N", Optional ByVal bShowPasswords As Boolean = False)  '

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder
        Dim tmpOut As New StringBuilder

        Dim with_passwords As String = ""
        Dim without_passwords As String = ""

        Dim toggleRowColor As Boolean = False

        Dim client_webHostName As String = ""

        Dim client_userId As Long = 0
        Dim client_firstName As String = ""
        Dim client_lastName As String = ""
        Dim client_login As String = ""

        Dim client_adminFlag As String = ""
        Dim client_email As String = ""
        Dim client_activeFlag As String = ""

        Dim conn_string As String = ""

        Dim first_inactive As Boolean = False
        Dim temp_table As New DataTable
        Dim user_logged_in As String = "N"
        Dim last_login As String = ""
        Dim counter1 As Integer = 0
        Dim temp_string As String = ""
        Dim client_spi_flag As String = ""
        Dim values_count As Integer = 0
        Dim current_user_count As Integer = 0
        Dim temp_link_text As String = ""
        Dim has_values As String = "N"
        Dim values_limit As Integer = 0
        Dim all_string As String = ""
        Dim users_total_active As Long = 0
        Dim cliuser_jetnet_contact_id As Long = 0

        Try

            conn_string = getMPMDomainDataConnection(nDomainID, client_webHostName, has_values, values_limit, users_total_active)

            If String.IsNullOrEmpty(users.Trim) Then
                users = users_total_active.ToString
            End If

            results_table = getMPMDomainUsersDataTable(conn_string, 0, user_types_to_show)
            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then


                    For Each r As DataRow In results_table.Rows
                        If Not IsDBNull(r.Item("cliuser_spi_flag")) Then
                            If Trim(r.Item("cliuser_spi_flag")) = "Y" Then
                                values_count = values_count + 1
                            End If
                        End If

                        If Not IsDBNull(r.Item("cliuser_active_flag")) Then
                            If Trim(r.Item("cliuser_active_flag")) = "Y" Then
                                current_user_count = current_user_count + 1
                            End If
                        End If
                    Next

                End If
            End If


            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then


                    htmlOut.Append("<div class=""Box""><div class=""subHeader"">Users for <a class=""underline pointer"" href=""http://" + client_webHostName.ToLower.Trim + """ title=""Click to GO to hosting domain"">" + client_webHostName.ToLower.Trim + "</a> domain</div>")
                    htmlOut.Append("<table id=""mpmDomainUserListTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"" border=""0"" class=""formatTable blue"">")
                    htmlOut.Append("<thead><tr><td align=""left""><b>Evo</b></td>")
                    htmlOut.Append("<td align=""left""><b>ID</b></td>")
                    htmlOut.Append("<td align=""left""><b>First Name</b></td>")
                    htmlOut.Append("<td align=""left""><b>Last Name</b></td>")
                    htmlOut.Append("<td align=""left""><b>Login</b></td>")
                    htmlOut.Append("<td align=""left""><a class=""pointer"" href=""adminMPM.aspx?&id=" + nDomainID.ToString + "&users=" + users.Trim + "&type_of=" + user_types_to_show + "&show_domain_users=true&show_pwd=true&homebase=" + is_homebase + "&hostname=" + client_webHostName.Trim + """ title=""Show User Passwords""><b>Password</b></a></td>")
                    htmlOut.Append("<td align=""left""><b>Administrator</b></td>")
                    htmlOut.Append("<td align=""left""><b>Email</b></td>")
                    htmlOut.Append("<td align=""left""><b>Active</b></td>")
                    htmlOut.Append("<td align=""left""><b>Values</b></td>")
                    htmlOut.Append("<td align=""right""><b>Last Login</b></td></tr></thead><tbody>")

                    For Each r As DataRow In results_table.Rows



                        If Not IsDBNull(r.Item("cliuser_id")) Then
                            If Not String.IsNullOrEmpty(r.Item("cliuser_id").ToString.Trim) Then
                                client_userId = CLng(r.Item("cliuser_id").ToString.Trim)
                            End If
                        End If

                        If Not IsDBNull(r.Item("cliuser_first_name")) Then
                            If Not String.IsNullOrEmpty(r.Item("cliuser_first_name").ToString.Trim) Then
                                client_firstName = r.Item("cliuser_first_name").ToString.Trim
                            End If
                        End If

                        If Not IsDBNull(r.Item("cliuser_last_name")) Then
                            If Not String.IsNullOrEmpty(r.Item("cliuser_last_name").ToString.Trim) Then
                                client_lastName = r.Item("cliuser_last_name").ToString.Trim
                            End If
                        End If

                        If Not IsDBNull(r.Item("cliuser_login")) Then
                            If Not String.IsNullOrEmpty(r.Item("cliuser_login").ToString.Trim) Then
                                client_login = r.Item("cliuser_login").ToString.Trim
                            End If
                        End If

                        If Not IsDBNull(r.Item("cliuser_jetnet_contact_id")) Then
                            If Not String.IsNullOrEmpty(r.Item("cliuser_jetnet_contact_id").ToString.Trim) Then
                                cliuser_jetnet_contact_id = r.Item("cliuser_jetnet_contact_id").ToString.Trim
                            End If
                        End If

                        client_adminFlag = IIf(r.Item("cliuser_admin_flag").ToString.Trim.ToUpper.Contains("Y"), "<img width=""20"" height=""20"" alt=""Yes"" src=""images/evo_green_check.png"">", "<img width=""20"" height=""20"" alt=""No"" src=""images/evo_red_check.png"">")

                        If Not IsDBNull(r.Item("cliuser_email_address")) Then
                            If Not String.IsNullOrEmpty(r.Item("cliuser_email_address").ToString.Trim) Then
                                client_email = r.Item("cliuser_email_address").ToString.Trim
                            End If
                        End If

                        client_activeFlag = IIf(r.Item("cliuser_active_flag").ToString.Trim.ToUpper.Contains("Y"), "<img width=""20"" height=""20"" alt=""Yes"" src=""images/evo_green_check.png"">", "<img width=""20"" height=""20"" alt=""No"" src=""images/evo_red_check.png"">")


                        client_spi_flag = IIf(r.Item("cliuser_spi_flag").ToString.Trim.ToUpper.Contains("Y"), "<img width=""20"" height=""20"" alt=""Yes"" src=""images/evo_green_check.png"">", "<img width=""20"" height=""20"" alt=""No"" src=""images/evo_red_check.png"">")


                        If Not r.Item("cliuser_active_flag").ToString.Trim.ToUpper.Contains("Y") Then

                            If Not first_inactive Then
                                If Not IsDBNull(r.Item("cliuser_password")) Then
                                    If Not String.IsNullOrEmpty(r.Item("cliuser_password").ToString.Trim) Then
                                        tmpOut.Append("<tr class=""alt_row""><td colspan=""11"" align=""left"" height=""18""><b>Inactive MPM Users</b></td></tr>")
                                        first_inactive = True
                                    End If
                                End If
                            End If

                        End If

                        'if active, check to see last login, and if current logged in 
                        user_logged_in = "N"
                        last_login = ""
                        If InStr(client_activeFlag, "evo_green_check", CompareMethod.Text) > 0 Then
                            temp_table.Clear()
                            temp_table = getMPMUsersDataTable_For_User(nDomainID, client_userId.ToString)

                            If Not IsNothing(temp_table) Then
                                If temp_table.Rows.Count > 0 Then
                                    For Each z As DataRow In temp_table.Rows
                                        If Not IsDBNull(z.Item("cliuser_last_login_date")) Then
                                            last_login = z.Item("cliuser_last_login_date")
                                        End If
                                        'If Not IsDBNull(z.Item("cliuser_loggedin_flag")) Then
                                        '   user_logged_in = z.Item("cliuser_loggedin_flag")
                                        ' End If
                                        If Not IsDBNull(z.Item("cliuser_last_session_date")) Then
                                            If DateDiff(DateInterval.Minute, Date.Now(), CDate(z.Item("cliuser_last_session_date"))) > -12 Then
                                                user_logged_in = "Y"
                                            Else
                                                user_logged_in = "N"
                                            End If
                                        End If
                                    Next
                                End If
                            End If
                            counter1 = counter1 + 1 ' counter for active users
                        End If



                        If Trim(user_logged_in) = "Y" Then
                            tmpOut.Append("<tr bgcolor=""#90EE90"">")
                        Else
                            tmpOut.Append("<tr bgcolor=""white"">")
                        End If

                        If cliuser_jetnet_contact_id > 0 Then
                            tmpOut.Append("<td align=""left""><img src=""images/chain.png"" title=""Linked Evo Account"" alt=""Linked Evo Account""></td>")
                        Else
                            tmpOut.Append("<td align=""left"">&nbsp;</td>")
                        End If


                        tmpOut.Append("<td align=""left"">" + client_userId.ToString.Trim + "</td>")
                        tmpOut.Append("<td align=""left"">" + client_firstName.Trim + "</td>")
                        tmpOut.Append("<td align=""left""><a class=""underline pointer"" href=""adminMPM.aspx?export_display=true&hostname=" + client_webHostName.Trim + "&showlog=Y&login=" + client_login.Trim + """ title=""Show User Events"">" + client_lastName.Trim + "</a></td>")
                        tmpOut.Append("<td align=""left"">" + client_login.Trim + "</td>")

                        If Not IsDBNull(r.Item("cliuser_password")) And InStr(client_activeFlag, "evo_green_check", CompareMethod.Text) > 0 Then
                            If Not String.IsNullOrEmpty(r.Item("cliuser_password").ToString.Trim) Then
                                tmpOut.Append("<td align=""left"">" + IIf(bShowPasswords, r.Item("cliuser_password").ToString.Trim + "<br/>", "") + "<a class=""underline pointer"" href=""adminMPM.aspx?send_password=true&id=" + nDomainID.ToString.Trim + "&user_id=" + client_userId.ToString.Trim + """ title=""Send Password to User""><b>Send password</b></a></td>")
                            Else
                                tmpOut.Append("<td align=""left"">&nbsp;</td>")
                            End If
                        Else
                            tmpOut.Append("<td align=""left"">&nbsp;</td>")
                        End If

                        tmpOut.Append("<td align=""center"">" + client_adminFlag + "</td>")
                        tmpOut.Append("<td align=""left""><a href=""mailto:" + client_email.Trim + """>" + client_email.Trim + "</a></td>")

                        temp_link_text = ""
                        temp_link_text = ("<td align=""center""><a class=""underline pointer"" href=""adminMPM.aspx?activate_user=true&users=" + users + "&homebase=" + is_homebase + "&type_of=" + user_types_to_show + "&id=" + nDomainID.ToString.Trim + "&user_id=" + client_userId.ToString.Trim + IIf(r.Item("cliuser_active_flag").ToString.Trim.ToUpper.Contains("Y"), "&active=N""", "&active=Y""") + IIf(r.Item("cliuser_active_flag").ToString.Trim.ToUpper.Contains("Y"), " title=""Deactivate User""", " title=""Activate User""") + ">" + client_activeFlag.Trim + "</a></td>")


                        ' if there is too many, then we only put the checks for the yes,
                        If current_user_count >= users Then
                            If Not IsDBNull(r.Item("cliuser_active_flag")) Then
                                If Trim(r.Item("cliuser_active_flag")) = "Y" Then
                                    tmpOut.Append(temp_link_text) ' which will put in the green check
                                Else
                                    tmpOut.Append("<td align=""center"">-</td>")
                                End If
                            Else
                                tmpOut.Append("<td align=""center"">-</td>")
                            End If
                        Else
                            tmpOut.Append(temp_link_text) ' which will put in the green check 
                        End If

                        temp_link_text = ""
                        temp_link_text = "<td align=""center""><a class=""underline pointer"" href=""adminMPM.aspx?activate_spi_user=true&users=" & users & "&homebase=" & is_homebase & "&type_of=" & user_types_to_show & "&id=" + nDomainID.ToString.Trim + "&user_id=" + client_userId.ToString.Trim + IIf(r.Item("cliuser_spi_flag").ToString.Trim.ToUpper.Contains("Y"), "&active=N""", "&active=Y""") + IIf(r.Item("cliuser_spi_flag").ToString.Trim.ToUpper.Contains("Y"), " title=""Deactivate Values""", " title=""Activate Values""") + ">" + client_spi_flag.Trim + "</a></td>"

                        ' if there is too many, then we only put the checks for the yes,
                        If values_count >= values_limit Then
                            If Not IsDBNull(r.Item("cliuser_spi_flag")) Then
                                If Trim(r.Item("cliuser_spi_flag")) = "Y" Then
                                    tmpOut.Append(temp_link_text) ' which will put in the green check
                                Else
                                    tmpOut.Append("<td align=""center"">-</td>")
                                End If
                            Else
                                tmpOut.Append("<td align=""center"">-</td>")
                            End If
                        Else
                            tmpOut.Append(temp_link_text) ' which will put in the green check 
                        End If



                        tmpOut.Append("<td align=""right"">" + last_login.Trim.ToString + "</td>")

                        tmpOut.Append("</tr>")

                        If Not IsDBNull(r.Item("cliuser_password")) Then
                            If Not String.IsNullOrEmpty(r.Item("cliuser_password").ToString.Trim) Then
                                with_passwords += tmpOut.ToString
                                tmpOut = New StringBuilder
                            Else
                                without_passwords += tmpOut.ToString
                                tmpOut = New StringBuilder
                            End If
                        Else
                            without_passwords += tmpOut.ToString
                            tmpOut = New StringBuilder
                        End If

                    Next


                    htmlOut.Append(with_passwords)

                    If Trim(without_passwords) <> "" Then
                        If Not String.IsNullOrEmpty(without_passwords.Trim) Then
                            htmlOut.Append("<tr class=""alt_row""><td colspan=""10"" align=""left"" height=""18""><b>Evolution Notes Users</b></td></tr>")
                            htmlOut.Append(without_passwords)
                        End If
                    End If

                    htmlOut.Append("</tbody></table></div>")

                Else
                    htmlOut.Append("<div class=""Box""><table id=""mpmDomainUserListTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"" class=""formatTable blue""><tr><td valign=""top"" align=""left""><br/>No Domain Users Found</td></tr></table></div>")
                End If
            Else
                htmlOut.Append("<div class=""Box""><table id=""mpmDomainUserListTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"" class=""formatTable blue""><tr><td valign=""top"" align=""left""><br/>No Domain Users Found</td></tr></table></div>")
            End If

            If Trim(users) <> "" Then
                If counter1 < CInt(Trim(users)) Then
                    If Not IsNothing(addNewClientUser) Then
                        addNewClientUser.Visible = True
                    End If
                Else
                    temp_string = Replace(htmlOut.ToString, "adminMPM.aspx?activate_user=true&id=" + nDomainID.ToString.Trim + "", "")
                    temp_string = Replace(temp_string, "Activate User", "Unable to Activate User - Active Users Limit Reached")
                    htmlOut.Length = 0
                    htmlOut.Append(temp_string)
                End If
            End If

            current_users_count = values_count

            If values_count = 0 And values_limit = 0 Then
                user_info = "Licences: (" + users + " Available/" + current_user_count.ToString + " Used), Values: None"
            Else
                user_info = "Licences: (" + users + " Available/" + current_user_count.ToString + " Used), Values: (" + values_limit.ToString + " Available/ " + values_count.ToString + " Used)"
            End If



        Catch ex As Exception

            aError = "Error in displayCurrentDomainUsers(ByVal nDomainID As Long, ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub

    Public Function sendMPMDomainClientPasswordEmail(ByVal nDomainID As Long, ByVal nUserID As Long) As Boolean

        Dim sQuery = New StringBuilder()
        Dim results_table As New DataTable

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Dim client_webHostName As String = ""
        Dim client_firstName As String = ""
        Dim client_lastName As String = ""
        Dim client_login As String = ""
        Dim client_password As String = ""

        Dim conn_string As String = ""
        Dim email_body_string As String = ""

        Dim bReturnValue As Boolean = False

        Try

            conn_string = getMPMDomainDataConnection(nDomainID, client_webHostName)

            results_table = getMPMDomainUsersDataTable(conn_string, nUserID)

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    For Each r As DataRow In results_table.Rows

                        If Not IsDBNull(r.Item("cliuser_first_name")) Then
                            If Not String.IsNullOrEmpty(r.Item("cliuser_first_name").ToString.Trim) Then
                                client_firstName = r.Item("cliuser_first_name").ToString.Trim
                            End If
                        End If

                        If Not IsDBNull(r.Item("cliuser_last_name")) Then
                            If Not String.IsNullOrEmpty(r.Item("cliuser_last_name").ToString.Trim) Then
                                client_lastName = r.Item("cliuser_last_name").ToString.Trim
                            End If
                        End If

                        If Not IsDBNull(r.Item("cliuser_login")) Then
                            If Not String.IsNullOrEmpty(r.Item("cliuser_login").ToString.Trim) Then
                                client_login = r.Item("cliuser_login").ToString.Trim
                            End If
                        End If

                        If Not IsDBNull(r.Item("cliuser_password")) Then
                            If Not String.IsNullOrEmpty(r.Item("cliuser_password").ToString.Trim) Then
                                client_password = r.Item("cliuser_password").ToString.Trim
                            End If
                        End If

                    Next

                    ' ok we have this users info send password email
                    sQuery.Append("INSERT INTO EMail_Queue (emailq_service, emailq_replyname, emailq_replyemail, emailq_smtp_server,")
                    sQuery.Append(" emailq_smtp_username, emailq_smtp_password, emailq_status, emailq_onhold_flag, emailq_html_flag,")
                    sQuery.Append(" emailq_comp_id, emailq_contact_id, emailq_sub_id, emailq_to, emailq_subject, emailq_body)")
                    sQuery.Append(" VALUES ('Evolution','Customer Service','customerservice@jetnet.com','smtp.jetnet.com',")
                    sQuery.Append("'customerservice@jetnet.com','cservice123','Open','N','Y',")

                    If CLng(HttpContext.Current.Session.Item("localUser").crmUserCompanyID.ToString) > 0 Then
                        sQuery.Append(HttpContext.Current.Session.Item("localUser").crmUserCompanyID.ToString + ",")
                    Else
                        sQuery.Append("0,")
                    End If

                    If CLng(HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString) > 0 Then
                        sQuery.Append(HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString + ",")
                    Else
                        sQuery.Append("0,")
                    End If

                    If CLng(HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString) > 0 Then
                        sQuery.Append(HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString + ",")
                    Else
                        sQuery.Append("0,")
                    End If

                    sQuery.Append("'" + client_login.Trim + "',") ' mail to
                    sQuery.Append("'Marketplace Manager Login Information',") ' email subject

                    email_body_string = "<hr />Below is your login information to the JETNET Marketplace Manager."
                    email_body_string += " Should you have any questions or issues contact customerservice@jetnet.com"
                    email_body_string += "<br />Site Address : " + client_webHostName.Trim
                    email_body_string += "<br />User Name : " + client_firstName.Trim + " " + client_lastName.Trim
                    email_body_string += "<br />Login : " + client_login.Trim
                    email_body_string += "<br />Password : " + client_password.Trim

                    email_body_string += "<br /><br /><b>JETNET LLC</b>"
                    email_body_string += "<br />101 First Street, 2nd Floor"
                    email_body_string += "<br />Utica, New York 13501"
                    email_body_string += "<br />Tel 800.553.8638"
                    email_body_string += "<br />Europe +41 (0) 43.243.7056"
                    email_body_string += "<br /><hr />"

                    sQuery.Append("'" + email_body_string.Trim + "'") ' email body
                    sQuery.Append(")")

                    SqlConn.ConnectionString = adminConnectStr

                    SqlConn.Open()
                    SqlCommand.Connection = SqlConn
                    SqlCommand.CommandType = CommandType.Text
                    SqlCommand.CommandTimeout = 240

                    SqlCommand.CommandText = sQuery.ToString

                    SqlCommand.ExecuteNonQuery()

                    bReturnValue = True

                End If

            End If


        Catch ex As Exception

            aError = "Error in sendMPMDomainClientPasswordEmail(ByVal nDomainID As Long, ByVal nUserID As Long) As Boolean " + ex.Message

        Finally

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing

            SqlCommand.Dispose()
            SqlCommand = Nothing

        End Try

        Return bReturnValue

        results_table = Nothing

    End Function

    Public Function activateMPMDomainClient_FOR_SPI(ByVal nDomainID As Long, ByVal nUserID As Long, ByVal activateFlag As String) As Boolean

        Dim sQuery = New StringBuilder()

        Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
        Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
        Dim MySqlReader As MySql.Data.MySqlClient.MySqlDataReader = Nothing
        Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing

        Dim conn_string As String = ""

        Dim bReturnValue As Boolean = False

        Try

            conn_string = getMPMDomainDataConnection(nDomainID, "")

            MySqlConn.ConnectionString = conn_string

            MySqlConn.Open()
            MySqlCommand.Connection = MySqlConn
            MySqlCommand.CommandType = CommandType.Text
            MySqlCommand.CommandTimeout = 240

            sQuery.Append("UPDATE Client_User SET cliuser_spi_flag = '" + activateFlag.ToUpper.Trim + "'")
            sQuery.Append(" WHERE cliuser_id = " + nUserID.ToString)

            MySqlCommand.CommandText = sQuery.ToString
            MySqlCommand.ExecuteNonQuery()

            bReturnValue = True

        Catch ex As Exception

            aError = "Error in activateMPMDomainClient(ByVal nDomainID As Long, ByVal nUserID As Long) As Boolean " + ex.Message

        Finally

            MySqlReader = Nothing

            MySqlConn.Dispose()
            MySqlConn.Close()
            MySqlConn = Nothing

            MySqlCommand.Dispose()
            MySqlCommand = Nothing

        End Try

        Return bReturnValue

    End Function

    Public Function activateMPMDomainClient(ByVal nDomainID As Long, ByVal nUserID As Long, ByVal activateFlag As String, ByVal current_values_count As Integer) As Boolean

        Dim sQuery = New StringBuilder()

        Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
        Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
        Dim MySqlReader As MySql.Data.MySqlClient.MySqlDataReader = Nothing
        Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing

        Dim conn_string As String = ""

        Dim bReturnValue As Boolean = False
        Dim has_values As String = "N"
        Dim values_count As Integer = 0

        Try

            conn_string = getMPMDomainDataConnection(nDomainID, "", has_values, values_count)

            MySqlConn.ConnectionString = conn_string

            MySqlConn.Open()
            MySqlCommand.Connection = MySqlConn
            MySqlCommand.CommandType = CommandType.Text
            MySqlCommand.CommandTimeout = 240

            sQuery.Append("UPDATE Client_User SET cliuser_active_flag = '" + activateFlag.ToUpper.Trim + "'")

            If Trim(activateFlag.ToUpper.Trim) = "Y" Then
                If has_values = "Y" And values_count > 0 Then
                    If current_values_count < values_count Then
                        sQuery.Append(", cliuser_spi_flag = 'Y' ")
                    End If
                End If
            Else
                sQuery.Append(", cliuser_spi_flag = 'N' ")
            End If

            sQuery.Append(" WHERE cliuser_id = " + nUserID.ToString)

            MySqlCommand.CommandText = sQuery.ToString
            MySqlCommand.ExecuteNonQuery()

            bReturnValue = True

        Catch ex As Exception

            aError = "Error in activateMPMDomainClient(ByVal nDomainID As Long, ByVal nUserID As Long) As Boolean " + ex.Message

        Finally

            MySqlReader = Nothing

            MySqlConn.Dispose()
            MySqlConn.Close()
            MySqlConn = Nothing

            MySqlCommand.Dispose()
            MySqlCommand = Nothing

        End Try

        Return bReturnValue

    End Function

    Public Function addNewMPMDomainClient(ByVal nDomainID As Long,
                                          ByVal sFirstName As String,
                                          ByVal sLastName As String,
                                          ByVal sLogin As String,
                                          ByVal sEmail As String,
                                          ByVal bAdminFlag As Boolean) As Boolean

        Dim sQuery = New StringBuilder()

        Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
        Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
        Dim MySqlReader As MySql.Data.MySqlClient.MySqlDataReader = Nothing
        Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing

        Dim conn_string As String = ""

        Dim bReturnValue As Boolean = False

        Try

            conn_string = getMPMDomainDataConnection(nDomainID, "")

            MySqlConn.ConnectionString = conn_string

            MySqlConn.Open()
            MySqlCommand.Connection = MySqlConn
            MySqlCommand.CommandType = CommandType.Text
            MySqlCommand.CommandTimeout = 240

            Dim sPassword As String = sFirstName.Trim + "@"
            Dim nRandomNum As Integer = (Rnd() * (9 - 1)) + 1

            sPassword += nRandomNum.ToString

            nRandomNum = (Rnd() * (9 - 1)) + 1
            sPassword += nRandomNum.ToString

            nRandomNum = (Rnd() * (9 - 1)) + 1
            sPassword += nRandomNum.ToString

            nRandomNum = (Rnd() * (9 - 1)) + 1
            sPassword += nRandomNum.ToString

            nRandomNum = (Rnd() * (9 - 1)) + 1
            sPassword += nRandomNum.ToString

            sQuery.Append("INSERT INTO Client_User (")
            sQuery.Append("cliuser_first_name,")
            sQuery.Append(" cliuser_last_name,")
            sQuery.Append(" cliuser_login,")
            sQuery.Append(" cliuser_email_address,")
            sQuery.Append(" cliuser_admin_flag,")
            sQuery.Append(" cliuser_password,")
            sQuery.Append(" cliuser_active_flag")
            sQuery.Append(") VALUES (")

            sQuery.Append("'" + sFirstName.Trim + "',")
            sQuery.Append("'" + sLastName.Trim + "',")
            sQuery.Append("'" + sLogin.Trim + "',")
            sQuery.Append("'" + sEmail.Trim + "',")

            sQuery.Append("'" + IIf(bAdminFlag, "Y", "N") + "',")

            sQuery.Append("'" + sPassword.Trim + "',")

            sQuery.Append("'Y')")

            MySqlCommand.CommandText = sQuery.ToString
            MySqlCommand.ExecuteNonQuery()

            bReturnValue = True

        Catch ex As Exception

            aError = "Error in addNewMPMDomainClient(ByVal nDomainID As Long, ...) As Boolean " + ex.Message

        Finally

            MySqlReader = Nothing

            MySqlConn.Dispose()
            MySqlConn.Close()
            MySqlConn = Nothing

            MySqlCommand.Dispose()
            MySqlCommand = Nothing

        End Try

        Return bReturnValue

    End Function

#End Region

#Region "admin_dev_page_functions"

    Public Function getDevelopmentPriorityDataTable() As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
        Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
        Dim MySqlReader As MySql.Data.MySqlClient.MySqlDataReader = Nothing
        Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing

        Try

            sQuery.Append("SELECT DISTINCT project_title, project_key,")
            sQuery.Append(" (SELECT COUNT(*) FROM tasks WHERE task_project_key = project_key AND task_priority = 1 AND task_status IN ('N','O','R','P','I')) AS HIGH,")
            sQuery.Append(" (SELECT COUNT(*) FROM tasks WHERE task_project_key = project_key AND task_priority = 2 AND task_status IN ('N','O','R','P','I')) AS MED,")
            sQuery.Append(" (SELECT COUNT(*) FROM tasks WHERE task_project_key = project_key AND task_priority = 3 AND task_status IN ('N','O','R','P','I')) AS LOW,")
            sQuery.Append(" (SELECT COUNT(*) FROM tasks WHERE task_project_key = project_key AND task_priority = 4 AND task_status IN ('N','O','R','P','I')) AS REVIEW,")
            sQuery.Append(" (SELECT COUNT(*) FROM tasks WHERE task_project_key = project_key AND task_priority = 5 AND task_status IN ('N','O','R','P','I')) AS HOLD,")
            sQuery.Append(" COUNT(*) AS totaltasks")
            sQuery.Append(" FROM projects INNER JOIN tasks ON task_project_key = project_key")
            sQuery.Append(" AND task_status IN ('N','O','R','P','I')")
            sQuery.Append(" GROUP BY project_title")
            sQuery.Append(" ORDER BY project_title")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>getDevelopmentPriorityDataTable() As DataTable</b><br />" + sQuery.ToString

            MySqlConn.ConnectionString = taskerConnectStr

            MySqlConn.Open()
            MySqlCommand.Connection = MySqlConn
            MySqlCommand.CommandType = CommandType.Text
            MySqlCommand.CommandTimeout = 240

            MySqlCommand.CommandText = sQuery.ToString

            MySqlReader = MySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                atemptable.Load(MySqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getDevelopmentPriorityDataTable load datatable</b><br /> " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getDevelopmentPriorityDataTable() As DataTable</b><br />" + ex.Message

        Finally
            MySqlReader = Nothing

            MySqlConn.Dispose()
            MySqlConn.Close()
            MySqlConn = Nothing

            MySqlCommand.Dispose()
            MySqlCommand = Nothing
        End Try

        Return atemptable

    End Function

    Public Sub displayDevelopmentPriority(ByRef out_htmlString As String)

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False

        Dim nTotalHigh As Integer = 0
        Dim nTotalMedium As Integer = 0
        Dim nTotalLow As Integer = 0
        Dim nTotalReview As Integer = 0
        Dim nTotalHold As Integer = 0
        Dim nTotalTotal As Integer = 0

        Try

            results_table = getDevelopmentPriorityDataTable()

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    htmlOut.Append("<table id=""devPriorityDataTable"" width=""98%"" cellpadding=""2"" cellspacing=""0"">")
                    htmlOut.Append("<tr><td colspan=""7"" align=""center"" style=""background-color: #eeeeee; height: 18px;""><b>JETNET Development Tasking Summary - By Priority</b></td></tr>")
                    htmlOut.Append("<tr><td align=""left"" width=""60%""><b>Project</b></td>")
                    htmlOut.Append("<td align=""center""><b>High</b></td>")
                    htmlOut.Append("<td align=""center""><b>Medium</b></td>")
                    htmlOut.Append("<td align=""center""><b>Low</b></td>")
                    htmlOut.Append("<td align=""center""><b>Review</b></td>")
                    htmlOut.Append("<td align=""center""><b>Hold</b></td>")
                    htmlOut.Append("<td align=""center""><b>Total</b></td></tr>")

                    For Each r As DataRow In results_table.Rows
                        If Not toggleRowColor Then
                            htmlOut.Append("<tr class=""alt_row"">")
                            toggleRowColor = True
                        Else
                            htmlOut.Append("<tr bgcolor=""white"">")
                            toggleRowColor = False
                        End If

                        htmlOut.Append("<td align=""left"" width=""60%"">" + HttpContext.Current.Server.HtmlEncode(r.Item("project_title").ToString.Trim) + "</td>")

                        If CInt(r.Item("HIGH").ToString.Trim) > 0 Then
                            htmlOut.Append("<td align=""center""><a class=""underline pointer"" href=""admindeveloper.aspx?project=" + r.Item("project_key").ToString.Trim + "&priority=HIGH"">" + r.Item("HIGH").ToString.Trim + "</a></td>")
                        Else
                            htmlOut.Append("<td align=""center"">0</td>")
                        End If

                        If CInt(r.Item("MED").ToString.Trim) > 0 Then
                            htmlOut.Append("<td align=""center""><a class=""underline pointer"" href=""admindeveloper.aspx?project=" + r.Item("project_key").ToString.Trim + "&priority=MED"">" + r.Item("MED").ToString.Trim + "</a></td>")
                        Else
                            htmlOut.Append("<td align=""center"">0</td>")
                        End If

                        If CInt(r.Item("LOW").ToString.Trim) > 0 Then
                            htmlOut.Append("<td align=""center""><a class=""underline pointer"" href=""admindeveloper.aspx?project=" + r.Item("project_key").ToString.Trim + "&priority=LOW"">" + r.Item("LOW").ToString.Trim + "</a></td>")
                        Else
                            htmlOut.Append("<td align=""center"">0</td>")
                        End If

                        If CInt(r.Item("REVIEW").ToString.Trim) > 0 Then
                            htmlOut.Append("<td align=""center""><a class=""underline pointer"" href=""admindeveloper.aspx?project=" + r.Item("project_key").ToString.Trim + "&priority=REVIEW"">" + r.Item("REVIEW").ToString.Trim + "</a></td>")
                        Else
                            htmlOut.Append("<td align=""center"">0</td>")
                        End If

                        If CInt(r.Item("HOLD").ToString.Trim) > 0 Then
                            htmlOut.Append("<td align=""center""><a class=""underline pointer"" href=""admindeveloper.aspx?project=" + r.Item("project_key").ToString.Trim + "&priority=HOLD"">" + r.Item("HOLD").ToString.Trim + "</a></td>")
                        Else
                            htmlOut.Append("<td align=""center"">0</td>")
                        End If

                        If CInt(r.Item("totaltasks").ToString.Trim) > 0 Then
                            htmlOut.Append("<td align=""center""><a class=""underline pointer"" href=""admindeveloper.aspx?project=" + r.Item("project_key").ToString.Trim + "&priority=ALL"">" + r.Item("totaltasks").ToString.Trim + "</a></td>")
                        Else
                            htmlOut.Append("<td align=""center"">0</td>")
                        End If

                        htmlOut.Append("</tr>")

                        nTotalHigh += CInt(r.Item("HIGH").ToString.Trim)
                        nTotalMedium += CInt(r.Item("MED").ToString.Trim)
                        nTotalLow += CInt(r.Item("LOW").ToString.Trim)
                        nTotalReview += CInt(r.Item("REVIEW").ToString.Trim)
                        nTotalHold += CInt(r.Item("HOLD").ToString.Trim)
                        nTotalTotal += CInt(r.Item("totaltasks").ToString.Trim)

                    Next

                    If Not toggleRowColor Then
                        htmlOut.Append("<tr class=""alt_row"">")
                        toggleRowColor = True
                    Else
                        htmlOut.Append("<tr bgcolor=""white"">")
                        toggleRowColor = False
                    End If

                    htmlOut.Append("<td align=""left"">Totals</td>")
                    htmlOut.Append("<td align=""center"">" + IIf(nTotalHigh > 0, "<a class=""underline pointer"" href=""admindeveloper.aspx?priority=HIGH"">" + nTotalHigh.ToString.Trim + "</a>", "0") + "</td>")
                    htmlOut.Append("<td align=""center"">" + IIf(nTotalMedium > 0, "<a class=""underline pointer"" href=""admindeveloper.aspx?priority=MED"">" + nTotalMedium.ToString.Trim + "</a>", "0") + "</td>")
                    htmlOut.Append("<td align=""center"">" + IIf(nTotalLow > 0, "<a class=""underline pointer"" href=""admindeveloper.aspx?priority=LOW"">" + nTotalLow.ToString.Trim + "</a>", "0") + "</td>")
                    htmlOut.Append("<td align=""center"">" + IIf(nTotalReview > 0, "<a class=""underline pointer"" href=""admindeveloper.aspx?priority=REVIEW"">" + nTotalReview.ToString.Trim + "</a>", "0") + "</td>")
                    htmlOut.Append("<td align=""center"">" + IIf(nTotalHold > 0, "<a class=""underline pointer"" href=""admindeveloper.aspx?priority=HOLD"">" + nTotalHold.ToString.Trim + "</a>", "0") + "</td>")
                    htmlOut.Append("<td align=""center"">" + IIf(nTotalTotal > 0, "<a class=""underline pointer"" href=""admindeveloper.aspx?priority=ALL"">" + nTotalTotal.ToString.Trim + "</a>", "0") + "</td>")

                    htmlOut.Append("</tr>")

                    htmlOut.Append("</table>")

                Else
                    htmlOut.Append("<table id=""devPriorityDataTable"" width=""100%"" cellpadding=""2"" cellspacing=""0""><tr><td valign=""top"" align=""left""><br/>No Development Priority Found</td></tr></table>")
                End If
            Else
                htmlOut.Append("<table id=""devPriorityDataTable"" width=""100%"" cellpadding=""2"" cellspacing=""0""><tr><td valign=""top"" align=""left""><br/>No Development Priority Found</td></tr></table>")
            End If

        Catch ex As Exception

            aError = "Error in displayDevelopmentPriority(ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub

    Public Function getDevelopmentStaffDataTable() As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
        Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
        Dim MySqlReader As MySql.Data.MySqlClient.MySqlDataReader = Nothing
        Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing

        Try

            sQuery.Append("SELECT DISTINCT task_assigned_staff AS staffname,")
            sQuery.Append(" (SELECT COUNT(*) FROM tasks WHERE task_assigned_staff = staffname AND task_priority = 1 AND task_status IN ('N','O','R','P','I')) as HIGH,")
            sQuery.Append(" (SELECT COUNT(*) FROM tasks WHERE task_assigned_staff = staffname AND task_priority = 2 AND task_status IN ('N','O','R','P','I')) as MED,")
            sQuery.Append(" (SELECT COUNT(*) FROM tasks WHERE task_assigned_staff = staffname AND task_priority = 3 AND task_status IN ('N','O','R','P','I')) as LOW,")
            sQuery.Append(" (SELECT COUNT(*) FROM tasks WHERE task_assigned_staff = staffname AND task_priority = 4 AND task_status IN ('N','O','R','P','I')) as REVIEW,")
            sQuery.Append(" (SELECT COUNT(*) FROM tasks WHERE task_assigned_staff = staffname AND task_priority = 5 AND task_status IN ('N','O','R','P','I')) as HOLD,")
            sQuery.Append(" COUNT(*) AS totaltasks")
            sQuery.Append(" FROM tasks WHERE task_status IN ('N','O','R','P','I')")
            sQuery.Append(" GROUP BY task_assigned_staff")
            sQuery.Append(" ORDER BY task_assigned_staff")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>getDevelopmentStaffDataTable() As DataTable</b><br />" + sQuery.ToString

            MySqlConn.ConnectionString = taskerConnectStr

            MySqlConn.Open()
            MySqlCommand.Connection = MySqlConn
            MySqlCommand.CommandType = CommandType.Text
            MySqlCommand.CommandTimeout = 240

            MySqlCommand.CommandText = sQuery.ToString

            MySqlReader = MySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                atemptable.Load(MySqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getDevelopmentStaffDataTable load datatable</b><br /> " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getDevelopmentStaffDataTable() As DataTable</b><br />" + ex.Message

        Finally
            MySqlReader = Nothing

            MySqlConn.Dispose()
            MySqlConn.Close()
            MySqlConn = Nothing

            MySqlCommand.Dispose()
            MySqlCommand = Nothing
        End Try

        Return atemptable

    End Function

    Public Sub displayDevelopmentStaff(ByRef out_htmlString As String)

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False

        Try

            results_table = getDevelopmentStaffDataTable()

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    htmlOut.Append("<table id=""devStaffDataTable"" width=""98%"" cellpadding=""2"" cellspacing=""0"">")
                    htmlOut.Append("<tr><td colspan=""7"" align=""center"" style=""background-color: #eeeeee; height: 18px;""><b>JETNET Development Tasking Summary - By Staff</b></td></tr>")
                    htmlOut.Append("<tr><td align=""left"" width=""60%""><b>Name</b></td>")
                    htmlOut.Append("<td align=""center""><b>High</b></td>")
                    htmlOut.Append("<td align=""center""><b>Medium</b></td>")
                    htmlOut.Append("<td align=""center""><b>Low</b></td>")
                    htmlOut.Append("<td align=""center""><b>Review</b></td>")
                    htmlOut.Append("<td align=""center""><b>Hold</b></td>")
                    htmlOut.Append("<td align=""center""><b>Total</b></td></tr>")

                    For Each r As DataRow In results_table.Rows
                        If Not toggleRowColor Then
                            htmlOut.Append("<tr class=""alt_row"">")
                            toggleRowColor = True
                        Else
                            htmlOut.Append("<tr bgcolor=""white"">")
                            toggleRowColor = False
                        End If

                        htmlOut.Append("<td align=""left"" width=""60%"">" + HttpContext.Current.Server.HtmlEncode(r.Item("staffname").ToString.Trim) + "</td>")

                        If CInt(r.Item("HIGH").ToString.Trim) > 0 Then
                            htmlOut.Append("<td align=""center""><a class=""underline pointer"" href=""admindeveloper.aspx?staffname=" + HttpContext.Current.Server.UrlEncode(r.Item("staffname").ToString.Trim) + "&priority=HIGH"">" + r.Item("HIGH").ToString.Trim + "</a></td>")
                        Else
                            htmlOut.Append("<td align=""center"">0</td>")
                        End If

                        If CInt(r.Item("MED").ToString.Trim) > 0 Then
                            htmlOut.Append("<td align=""center""><a class=""underline pointer"" href=""admindeveloper.aspx?staffname=" + HttpContext.Current.Server.UrlEncode(r.Item("staffname").ToString.Trim) + "&priority=MED"">" + r.Item("MED").ToString.Trim + "</a></td>")
                        Else
                            htmlOut.Append("<td align=""center"">0</td>")
                        End If

                        If CInt(r.Item("LOW").ToString.Trim) > 0 Then
                            htmlOut.Append("<td align=""center""><a class=""underline pointer"" href=""admindeveloper.aspx?staffname=" + HttpContext.Current.Server.UrlEncode(r.Item("staffname").ToString.Trim) + "&priority=LOW"">" + r.Item("LOW").ToString.Trim + "</a></td>")
                        Else
                            htmlOut.Append("<td align=""center"">0</td>")
                        End If

                        If CInt(r.Item("REVIEW").ToString.Trim) > 0 Then
                            htmlOut.Append("<td align=""center""><a class=""underline pointer"" href=""admindeveloper.aspx?staffname=" + HttpContext.Current.Server.UrlEncode(r.Item("staffname").ToString.Trim) + "&priority=REVIEW"">" + r.Item("REVIEW").ToString.Trim + "</a></td>")
                        Else
                            htmlOut.Append("<td align=""center"">0</td>")
                        End If

                        If CInt(r.Item("HOLD").ToString.Trim) > 0 Then
                            htmlOut.Append("<td align=""center""><a class=""underline pointer"" href=""admindeveloper.aspx?staffname=" + HttpContext.Current.Server.UrlEncode(r.Item("staffname").ToString.Trim) + "&priority=HOLD"">" + r.Item("HOLD").ToString.Trim + "</a></td>")
                        Else
                            htmlOut.Append("<td align=""center"">0</td>")
                        End If

                        If CInt(r.Item("totaltasks").ToString.Trim) > 0 Then
                            htmlOut.Append("<td align=""center""><a class=""underline pointer"" href=""admindeveloper.aspx?staffname=" + HttpContext.Current.Server.UrlEncode(r.Item("staffname").ToString.Trim) + "&priority=ALL"">" + r.Item("totaltasks").ToString.Trim + "</a></td>")
                        Else
                            htmlOut.Append("<td align=""center"">0</td>")
                        End If

                        htmlOut.Append("</tr>")

                    Next

                    htmlOut.Append("</table>")

                Else
                    htmlOut.Append("<table id=""devStaffDataTable"" width=""100%"" cellpadding=""2"" cellspacing=""0""><tr><td valign=""top"" align=""left""><br/>No Staff Development Tasks Found</td></tr></table>")
                End If
            Else
                htmlOut.Append("<table id=""devStaffDataTable"" width=""100%"" cellpadding=""2"" cellspacing=""0""><tr><td valign=""top"" align=""left""><br/>No Staff Development Tasks Found</td></tr></table>")
            End If

        Catch ex As Exception

            aError = "Error in displayDevelopmentStaff(ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub

    Public Function getDevelopmentTaskSummaryDataTable() As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
        Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
        Dim MySqlReader As MySql.Data.MySqlClient.MySqlDataReader = Nothing
        Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing

        Try

            sQuery.Append("SELECT project_title, project_key,")
            sQuery.Append(" (SELECT COUNT(*) FROM tasks WHERE task_project_key = project_key AND task_status = 'C' AND task_status_date >= '" + Format(DateAdd(DateInterval.Day, -60, Now()), "yyyy-MM-dd H:mm:ss").Trim + "') AS closecount,")
            sQuery.Append(" (SELECT COUNT(*) FROM tasks WHERE task_project_key = project_key AND task_status = 'O') AS opencount,")
            sQuery.Append(" (SELECT COUNT(*) FROM tasks WHERE task_project_key = project_key AND task_status = 'R') AS reviewcount,")
            sQuery.Append(" (SELECT COUNT(*) FROM tasks WHERE task_project_key = project_key AND task_status = 'P') AS releasecount,")
            sQuery.Append(" (SELECT COUNT(*) FROM tasks WHERE task_project_key = project_key AND task_status = 'N') AS unassignedcount,")
            sQuery.Append(" (SELECT COUNT(*) FROM tasks WHERE task_project_key = project_key AND task_status = 'I') AS inprogresscount")
            sQuery.Append(" FROM projects WHERE project_status = 'A'")
            sQuery.Append(" ORDER BY project_title")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>getDevelopmentTaskSummaryDataTable() As DataTable</b><br />" + sQuery.ToString

            MySqlConn.ConnectionString = taskerConnectStr

            MySqlConn.Open()
            MySqlCommand.Connection = MySqlConn
            MySqlCommand.CommandType = CommandType.Text
            MySqlCommand.CommandTimeout = 240

            MySqlCommand.CommandText = sQuery.ToString

            MySqlReader = MySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                atemptable.Load(MySqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getDevelopmentTaskSummaryDataTable load datatable</b><br /> " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getDevelopmentTaskSummaryDataTable() As DataTable</b><br />" + ex.Message

        Finally
            MySqlReader = Nothing

            MySqlConn.Dispose()
            MySqlConn.Close()
            MySqlConn = Nothing

            MySqlCommand.Dispose()
            MySqlCommand = Nothing
        End Try

        Return atemptable

    End Function

    Public Sub displayDevelopmentSummary(ByRef out_htmlString As String)

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder
        ' Dim toggleRowColor As Boolean = False

        Dim nTotalUnAssigned As Integer = 0
        Dim nTotalOpen As Integer = 0
        Dim nTotalReview As Integer = 0
        Dim nTotalReady As Integer = 0
        Dim nTotalClosed As Integer = 0
        Dim nTotalProgress As Integer = 0
        Dim nTotalTotal As Integer = 0

        Dim nRunningTotalUnAssigned As Integer = 0
        Dim nRunningTotalOpen As Integer = 0
        Dim nRunningTotalReview As Integer = 0
        Dim nRunningTotalReady As Integer = 0
        Dim nRunningTotalClosed As Integer = 0
        Dim nRunningTotalProgress As Integer = 0
        Dim nRunningTotal As Integer = 0

        Try

            results_table = getDevelopmentTaskSummaryDataTable()

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then
                    htmlOut.Append("<div class=""subHeader"">JETNET Development Tasking Summary</div>")
                    htmlOut.Append("<div class=""Box""><table id=""devSummaryDataTable"" width=""98%"" cellpadding=""2"" cellspacing=""0"" class=""formatTable blue""><thead>")

                    htmlOut.Append("<tr><th width=""40%""><b>Project</b></th>")
                    htmlOut.Append("<th><b>Closed</b></th>")
                    htmlOut.Append("<th><b>Unassigned</b></td>")
                    htmlOut.Append("<th><b>Open Items</b></td>")
                    htmlOut.Append("<th><b>In Progress</b></td>")
                    htmlOut.Append("<th><b>Review/Test</b></td>")
                    htmlOut.Append("<th><b>Ready for Release</b></td>")
                    htmlOut.Append("<th><b>All</b></td></tr></thead><tbody>")

                    For Each r As DataRow In results_table.Rows

                        nTotalTotal = 0
                        nTotalUnAssigned = 0
                        nTotalOpen = 0
                        nTotalReview = 0
                        nTotalReady = 0
                        nTotalClosed = 0
                        nTotalProgress = 0

                        'If Not toggleRowColor Then
                        '    htmlOut.Append("<tr class=""alt_row"">")
                        '    toggleRowColor = True
                        'Else
                        htmlOut.Append("<tr>")
                        '    toggleRowColor = False
                        'End If

                        htmlOut.Append("<td align=""left"" width=""40%"">" + HttpContext.Current.Server.HtmlEncode(r.Item("project_title").ToString.Trim) + "</td>")

                        If CInt(r.Item("closecount").ToString.Trim) > 0 Then
                            htmlOut.Append("<td align=""center""><a class=""underline pointer"" href=""admindeveloper.aspx?project=" + r.Item("project_key").ToString.Trim + "&status=C"">" + r.Item("closecount").ToString.Trim + "</a></td>")
                        Else
                            htmlOut.Append("<td align=""center"">0</td>")
                        End If

                        If CInt(r.Item("unassignedcount").ToString.Trim) > 0 Then
                            htmlOut.Append("<td align=""center""><a class=""underline pointer"" href=""admindeveloper.aspx?project=" + r.Item("project_key").ToString.Trim + "&status=N"">" + r.Item("unassignedcount").ToString.Trim + "</a></td>")
                        Else
                            htmlOut.Append("<td align=""center"">0</td>")
                        End If

                        If CInt(r.Item("opencount").ToString.Trim) > 0 Then
                            htmlOut.Append("<td align=""center""><a class=""underline pointer"" href=""admindeveloper.aspx?project=" + r.Item("project_key").ToString.Trim + "&status=O"">" + r.Item("opencount").ToString.Trim + "</a></td>")
                        Else
                            htmlOut.Append("<td align=""center"">0</td>")
                        End If

                        If CInt(r.Item("inprogresscount").ToString.Trim) > 0 Then
                            htmlOut.Append("<td align=""center""><a class=""underline pointer"" href=""admindeveloper.aspx?project=" + r.Item("project_key").ToString.Trim + "&status=I"">" + r.Item("inprogresscount").ToString.Trim + "</a></td>")
                        Else
                            htmlOut.Append("<td align=""center"">0</td>")
                        End If

                        If CInt(r.Item("reviewcount").ToString.Trim) > 0 Then
                            htmlOut.Append("<td align=""center""><a class=""underline pointer"" href=""admindeveloper.aspx?project=" + r.Item("project_key").ToString.Trim + "&status=R"">" + r.Item("reviewcount").ToString.Trim + "</a></td>")
                        Else
                            htmlOut.Append("<td align=""center"">0</td>")
                        End If

                        If CInt(r.Item("releasecount").ToString.Trim) > 0 Then
                            htmlOut.Append("<td align=""center""><a class=""underline pointer"" href=""admindeveloper.aspx?project=" + r.Item("project_key").ToString.Trim + "&status=P"">" + r.Item("releasecount").ToString.Trim + "</a></td>")
                        Else
                            htmlOut.Append("<td align=""center"">0</td>")
                        End If

                        nTotalClosed += CInt(r.Item("closecount").ToString.Trim)
                        nTotalProgress += CInt(r.Item("inprogresscount").ToString.Trim)
                        nTotalUnAssigned += CInt(r.Item("unassignedcount").ToString.Trim)
                        nTotalOpen += CInt(r.Item("opencount").ToString.Trim)
                        nTotalReview += CInt(r.Item("reviewcount").ToString.Trim)
                        nTotalReady += CInt(r.Item("releasecount").ToString.Trim)

                        nTotalTotal = (nTotalClosed + nTotalProgress + nTotalUnAssigned + nTotalOpen + nTotalReview + nTotalReady)

                        htmlOut.Append("<td align=""center""><a class=""underline pointer"" href=""admindeveloper.aspx?project=" + r.Item("project_key").ToString.Trim + "&status=A"">" + nTotalTotal.ToString.Trim + "</a></td>")

                        htmlOut.Append("</tr>")

                        nRunningTotalClosed += nTotalClosed
                        nRunningTotalProgress += nTotalProgress
                        nRunningTotalUnAssigned += nTotalUnAssigned
                        nRunningTotalOpen += nTotalOpen
                        nRunningTotalReview += nTotalReview
                        nRunningTotalReady += nTotalReady
                        nRunningTotal += nTotalTotal

                    Next

                    'If Not toggleRowColor Then
                    '    htmlOut.Append("<tr class=""alt_row"">")
                    '    toggleRowColor = True
                    'Else
                    htmlOut.Append("<tr>")
                    '    toggleRowColor = False
                    'End If

                    htmlOut.Append("<td align=""left"">Totals</td>")
                    htmlOut.Append("<td align=""center"">" + nRunningTotalClosed.ToString.Trim + "</td>")
                    htmlOut.Append("<td align=""center"">" + nRunningTotalUnAssigned.ToString.Trim + "</td>")
                    htmlOut.Append("<td align=""center"">" + nRunningTotalOpen.ToString.Trim + "</td>")
                    htmlOut.Append("<td align=""center"">" + nRunningTotalProgress.ToString.Trim + "</td>")
                    htmlOut.Append("<td align=""center"">" + nRunningTotalReview.ToString.Trim + "</td>")
                    htmlOut.Append("<td align=""center"">" + nRunningTotalReady.ToString.Trim + "</td>")
                    htmlOut.Append("<td align=""center"">" + nRunningTotal.ToString.Trim + "</td>")

                    htmlOut.Append("</tr>")

                    htmlOut.Append("</tbody></table></div>")

                Else
                    htmlOut.Append("<div class=""Box""><table id=""devSummaryDataTable"" width=""100%"" cellpadding=""2"" cellspacing=""0""><tr><td valign=""top"" align=""left""><br/>No Development Summary Tasks Found</td></tr></table></div>")
                End If
            Else
                htmlOut.Append("<div class=""Box""><table id=""devSummaryDataTable"" width=""100%"" cellpadding=""2"" cellspacing=""0""><tr><td valign=""top"" align=""left""><br/>No Development Summary Tasks Found</td></tr></table></div>")
            End If

        Catch ex As Exception

            aError = "Error in displayDevelopmentSummary(ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub

    Public Function getDevelopmentDetailDataTable(ByVal searchCriteria As developerSelectionCriteriaClass) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
        Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
        Dim MySqlReader As MySql.Data.MySqlClient.MySqlDataReader = Nothing
        Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing

        Try

            sQuery.Append("SELECT task_title, task_assigned_staff, task_description, task_entry_date, status_name, priority_title, task_follow_up, project_title, task_key, task_customer")
            sQuery.Append(" FROM tasks")
            sQuery.Append(" INNER JOIN projects ON project_key = task_project_key")
            sQuery.Append(" INNER JOIN status ON status_value = task_status")
            sQuery.Append(" INNER JOIN priority ON priority_key = task_priority")

            If searchCriteria.DeveloperCriteriaProjectKey > 0 Then
                sQuery.Append(" WHERE task_project_key = " + searchCriteria.DeveloperCriteriaProjectKey.ToString.Trim)
            Else
                sQuery.Append(" WHERE task_project_key > 0")
            End If

            If Not String.IsNullOrEmpty(searchCriteria.DeveloperCriteriaProjectStatus.Trim) Then
                If Not searchCriteria.DeveloperCriteriaProjectStatus.Trim.ToUpper.Contains("A") Then
                    sQuery.Append(Constants.cAndClause + "task_status = '" + searchCriteria.DeveloperCriteriaProjectStatus.Trim + "'")

                    If searchCriteria.DeveloperCriteriaProjectStatus.Trim.ToUpper.Contains("C") Then
                        sQuery.Append(Constants.cAndClause + "task_status_date >= '" + Format(DateAdd(DateInterval.Day, -60, Now()), "yyyy-MM-dd H:mm:ss").Trim + "'")
                    End If

                End If
            End If

            If Not String.IsNullOrEmpty(searchCriteria.DeveloperCriteriaProjectTitle.Trim) Then
                sQuery.Append(Constants.cAndClause + "project_title = '" + searchCriteria.DeveloperCriteriaProjectTitle.Trim + "'")
            End If

            If Not String.IsNullOrEmpty(searchCriteria.DeveloperCriteriaProjectStaffName.Trim) Then
                sQuery.Append(Constants.cAndClause + "task_assigned_staff = '" + searchCriteria.DeveloperCriteriaProjectStaffName.Trim + "'")
            End If

            If Not String.IsNullOrEmpty(searchCriteria.DeveloperCriteriaProjectPriority.Trim) Then
                If Not searchCriteria.DeveloperCriteriaProjectPriority.Trim.ToUpper.Contains("ALL") Then
                    sQuery.Append(Constants.cAndClause + "priority_title LIKE '" + searchCriteria.DeveloperCriteriaProjectPriority.Trim + "%'")
                End If
            End If

            If searchCriteria.DeveloperCriteriaProjectStatus.Trim.ToUpper.Contains("C") Then
                sQuery.Append(Constants.cAndClause + "task_status = 'C'")
            Else
                sQuery.Append(Constants.cAndClause + "task_status IN ('N','O','R','P','I')")
            End If

            sQuery.Append("ORDER BY task_entry_date DESC, status_name DESC")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>getDevelopmentDataTable(ByVal searchCriteria As developerSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

            MySqlConn.ConnectionString = taskerConnectStr

            MySqlConn.Open()
            MySqlCommand.Connection = MySqlConn
            MySqlCommand.CommandType = CommandType.Text
            MySqlCommand.CommandTimeout = 240

            MySqlCommand.CommandText = sQuery.ToString

            MySqlReader = MySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                atemptable.Load(MySqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getDevelopmentDataTable load datatable</b><br /> " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getDevelopmentDataTable(ByVal searchCriteria As developerSelectionCriteriaClass) As DataTable</b><br />" + ex.Message

        Finally
            MySqlReader = Nothing

            MySqlConn.Dispose()
            MySqlConn.Close()
            MySqlConn = Nothing

            MySqlCommand.Dispose()
            MySqlCommand = Nothing
        End Try

        Return atemptable

    End Function

    Public Sub displayTaskDetails(ByVal searchCriteria As developerSelectionCriteriaClass, ByRef out_htmlString As String)

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False
        Dim last_project As String = ""

        Try

            results_table = getDevelopmentDetailDataTable(searchCriteria)

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    If searchCriteria.DeveloperCriteriaToggleDisplay Then

                        htmlOut.Append("<table id=""devDetailsDataTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"">")
                        htmlOut.Append("<tr><td colspan=""7"" valign=""middle"" align=""center""><font size=""+1""><b>" + results_table.Rows(0).Item("project_title").ToString)

                        If ((Not String.IsNullOrEmpty(searchCriteria.DeveloperCriteriaProjectStatus.Trim)) And (Not searchCriteria.DeveloperCriteriaProjectStatus.Trim.ToUpper.Contains("A"))) Then
                            htmlOut.Append(" - " + results_table.Rows(0).Item("status_name").ToString)
                        End If

                        htmlOut.Append("</b></font></td></tr>")

                        htmlOut.Append("<tr><td valign=""middle"" align=""left""><b>ID</b></td>")
                        htmlOut.Append("<td valign=""middle"" align=""right""><b>Priority</b></td>")
                        htmlOut.Append("<td valign=""middle"" align=""right""><b>Title</b></td>")
                        htmlOut.Append("<td valign=""middle"" align=""right""><b>Assigned Staff</b></td>")
                        htmlOut.Append("<td valign=""middle"" align=""right""><b>Description</b></td>")
                        htmlOut.Append("<td valign=""middle"" align=""right""><b>Entry Date</b></td>")
                        htmlOut.Append("<td valign=""middle"" align=""right""><b>Status</b></td></tr>")

                    Else

                        htmlOut.Append("<table id=""devDetailsDataTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"">")

                        If Not String.IsNullOrEmpty(searchCriteria.DeveloperCriteriaProjectStaffName.Trim) Then
                            htmlOut.Append("<tr><td colspan=""2"" valign=""middle"" align=""center""><font size=""+1""><b>" + searchCriteria.DeveloperCriteriaProjectStaffName.Trim)
                        Else
                            htmlOut.Append("<tr><td colspan=""2"" valign=""middle"" align=""center""><font size=""+1""><b>" + results_table.Rows(0).Item("project_title").ToString)
                        End If

                        If ((Not String.IsNullOrEmpty(searchCriteria.DeveloperCriteriaProjectStatus.Trim)) And (Not searchCriteria.DeveloperCriteriaProjectStatus.Trim.ToUpper.Contains("A"))) Then
                            htmlOut.Append(" - " + results_table.Rows(0).Item("status_name").ToString)
                        End If

                        If Not String.IsNullOrEmpty(searchCriteria.DeveloperCriteriaProjectPriority.Trim) Then
                            htmlOut.Append(" - " + searchCriteria.DeveloperCriteriaProjectPriority.Trim)
                        End If

                        If Not String.IsNullOrEmpty(searchCriteria.DeveloperCriteriaProjectStaffName.Trim) Then
                            htmlOut.Append("&nbsp;&nbsp;(<a class=""underline pointer"" href=""admindeveloper.aspx?staffname=" + HttpContext.Current.Server.UrlEncode(searchCriteria.DeveloperCriteriaProjectStaffName.Trim) + "&priority=" + searchCriteria.DeveloperCriteriaProjectPriority.Trim + "&status=O"">OPEN</a>")
                            htmlOut.Append("&nbsp;&nbsp;<a class=""underline pointer"" href=""admindeveloper.aspx?staffname=" + HttpContext.Current.Server.UrlEncode(searchCriteria.DeveloperCriteriaProjectStaffName.Trim) + "&priority=" + searchCriteria.DeveloperCriteriaProjectPriority.Trim + "&status=R"">REVIEW</a>")
                            htmlOut.Append("&nbsp;&nbsp;<a class=""underline pointer"" href=""admindeveloper.aspx?staffname=" + HttpContext.Current.Server.UrlEncode(searchCriteria.DeveloperCriteriaProjectStaffName.Trim) + "&priority=" + searchCriteria.DeveloperCriteriaProjectPriority.Trim + "&status=P"">RELEASE</a>")
                            htmlOut.Append("&nbsp;&nbsp;<a class=""underline pointer"" href=""admindeveloper.aspx?staffname=" + HttpContext.Current.Server.UrlEncode(searchCriteria.DeveloperCriteriaProjectStaffName.Trim) + "&priority=" + searchCriteria.DeveloperCriteriaProjectPriority.Trim + "&status=H"">HOLD</a>")
                            htmlOut.Append("&nbsp;&nbsp;<a class=""underline pointer"" href=""admindeveloper.aspx?staffname=" + HttpContext.Current.Server.UrlEncode(searchCriteria.DeveloperCriteriaProjectStaffName.Trim) + "&priority=" + searchCriteria.DeveloperCriteriaProjectPriority.Trim + "&status=I"">IN PROGRESS</a>")
                            htmlOut.Append("&nbsp;&nbsp;<a class=""underline pointer"" href=""admindeveloper.aspx?staffname=" + HttpContext.Current.Server.UrlEncode(searchCriteria.DeveloperCriteriaProjectStaffName.Trim) + "&priority=" + searchCriteria.DeveloperCriteriaProjectPriority.Trim + "&status=C"">CLOSED</a>)")
                        End If

                        htmlOut.Append("</b></font></td></tr>")

                    End If

                    For Each r As DataRow In results_table.Rows

                        If searchCriteria.DeveloperCriteriaToggleDisplay Then

                            If Not toggleRowColor Then
                                htmlOut.Append("<tr class=""alt_row"">")
                                toggleRowColor = True
                            Else
                                htmlOut.Append("<tr bgcolor=""white"">")
                                toggleRowColor = False
                            End If

                            htmlOut.Append("<td valign=""middle"" align=""left"" width=""25"">" + r.Item("task_key").ToString.Trim + "</td>")
                            htmlOut.Append("<td valign=""middle"" align=""left"" width=""100"">" + HttpContext.Current.Server.HtmlEncode(r.Item("priority_title").ToString.Trim) + "</td>")
                            htmlOut.Append("<td valign=""middle"" align=""left"" width=""300""><b>" + HttpContext.Current.Server.HtmlEncode(r.Item("task_title").ToString.Trim) + "</b></td>")
                            htmlOut.Append("<td valign=""middle"" align=""left"" width=""125"">" + HttpContext.Current.Server.HtmlEncode(r.Item("task_assigned_staff").ToString.Trim) + "</td>")
                            htmlOut.Append("<td valign=""middle"" align=""left"" width=""550"">" + HttpContext.Current.Server.HtmlEncode(r.Item("task_description").ToString.Replace(vbCrLf, "<br />").Trim) + "</td>")

                            If Not IsDBNull(r.Item("task_follow_up")) Then
                                If Not String.IsNullOrEmpty(r.Item("task_follow_up").ToString.Trim) Then
                                    htmlOut.Append("<br /><br /><em>Follow Up</em> : <font color=""#FA5858"">" + r.Item("task_follow_up").ToString.Replace(vbCrLf, "<br />").Trim + "</font>")
                                End If
                            End If

                            htmlOut.Append("</td>")
                            htmlOut.Append("<td width=""100"" valign=""middle"" align=""right"">" + FormatDateTime(CDate(r.Item("task_entry_date").ToString.Trim), DateFormat.ShortDate) + "</td>")
                            htmlOut.Append("<td width=""50"" valign=""middle"" align=""left"">" + r.Item("status_name").ToString.Trim + "</td>")

                            htmlOut.Append("</tr>")

                        Else

                            If Not last_project.ToLower.Trim.Contains(r.Item("project_title").ToString.ToLower.Trim) Then
                                htmlOut.Append("<tr class=""alt_row""><td align=""left"" valign=""middle"" colspan=""2""><b>" + r.Item("project_title").ToString.Trim + "</b></td></tr>")
                            End If

                            htmlOut.Append("<tr><td align=""right"" valign=""top"" width=""10"">&#8226;</td><td align=""left"" valign=""top"" width=""1000"">")

                            htmlOut.Append("<b>" + HttpContext.Current.Server.HtmlEncode(r.Item("task_title").ToString.Trim))

                            If Not IsDBNull(r.Item("task_key")) Then
                                htmlOut.Append(" (#" & r.Item("task_key") & ")")
                            End If

                            htmlOut.Append("</b> - ")
                            htmlOut.Append(r.Item("task_description").ToString.Replace(vbCrLf, "").Replace("<br>", "").Trim)

                            If Not IsDBNull(r.Item("task_follow_up")) Then
                                If Not String.IsNullOrEmpty(r.Item("task_follow_up").ToString.Trim) Then
                                    htmlOut.Append("<br /><br /><em>Follow Up</em> : <font color=""#FA5858"">" + r.Item("task_follow_up").ToString.Replace(vbCrLf, "<br />").Trim + "</font>")
                                End If
                            End If

                            If Not IsDBNull(r.Item("task_customer")) Then
                                If Not String.IsNullOrEmpty(r.Item("task_customer").ToString.Trim) Then
                                    htmlOut.Append("<br /><br /><em>Customer</em> : <font color=""#FA5858"">" + r.Item("task_customer").ToString.Replace(vbCrLf, "<br />").Trim + "</font>")
                                End If
                            End If

                            htmlOut.Append("<br /><br />[Entered: " + FormatDateTime(CDate(r.Item("task_entry_date").ToString.Trim), DateFormat.ShortDate))
                            htmlOut.Append(", Assigned: " + r.Item("task_assigned_staff").ToString.Trim)
                            htmlOut.Append(", Priority:" + r.Item("priority_title").ToString.Replace("Priority", ""))
                            htmlOut.Append(", Status:" + r.Item("status_name").ToString.Trim + "]")

                            htmlOut.Append("<br /><br /><br /></td></tr>")

                            last_project = r.Item("project_title").ToString.Trim

                        End If

                    Next

                    htmlOut.Append("</table>")

                Else
                    htmlOut.Append("<table id=""devDetailsDataTable"" width=""100%"" cellpadding=""2"" cellspacing=""0""><tr><td valign=""top"" align=""left""><br/>No Task Details Found</td></tr></table>")
                End If
            Else
                htmlOut.Append("<table id=""devDetailsDataTable"" width=""100%"" cellpadding=""2"" cellspacing=""0""><tr><td valign=""top"" align=""left""><br/>No Task Details Found</td></tr></table>")
            End If

        Catch ex As Exception

            aError = "Error in displayTaskDetails(ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub

    Public Sub insertAddNewTask(ByVal searchCriteria As developerSelectionCriteriaClass)

        Dim sQuery = New StringBuilder()

        Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
        Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
        Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            sQuery.Append("INSERT INTO tasks (")
            sQuery.Append("task_entry_date,")
            sQuery.Append(" task_status_date,")
            sQuery.Append(" task_project_key,")
            sQuery.Append(" task_assigned_staff,")
            sQuery.Append(" task_status,")
            sQuery.Append(" task_priority,")
            sQuery.Append(" task_title,")
            sQuery.Append(" task_description,")
            sQuery.Append(" task_follow_up,")
            sQuery.Append(" task_custview_flag,")
            sQuery.Append(" task_entry_staff")
            sQuery.Append(") VALUES (")

            sQuery.Append("'" + Format(Now(), "yyyy-MM-dd H:mm:ss").Trim + "',")
            sQuery.Append("'" + Format(Now(), "yyyy-MM-dd H:mm:ss").Trim + "',")
            sQuery.Append("'" + searchCriteria.DeveloperCriteriaProjectKey.ToString.Trim + "',")
            sQuery.Append("'" + searchCriteria.DeveloperCriteriaProjectStaffName.Trim + "',")
            sQuery.Append("'" + searchCriteria.DeveloperCriteriaProjectStatus.Trim + "',")
            sQuery.Append("'" + searchCriteria.DeveloperCriteriaProjectPriority.Trim + "',")
            sQuery.Append(IIf(String.IsNullOrEmpty(searchCriteria.DeveloperCriteriaProjectTaskTitle), "''", "'" + searchCriteria.DeveloperCriteriaProjectTaskTitle.Replace(Constants.cSingleQuote, Constants.cDoubleSingleQuote).Trim + "'") + ",")
            sQuery.Append(IIf(String.IsNullOrEmpty(searchCriteria.DeveloperCriteriaProjectTaskDiscription.Trim), "''", "'" + searchCriteria.DeveloperCriteriaProjectTaskDiscription.Replace(Constants.cSingleQuote, Constants.cDoubleSingleQuote).Trim + "'") + ",")
            sQuery.Append(IIf(String.IsNullOrEmpty(searchCriteria.DeveloperCriteriaProjectFollowUp.Trim), "''", "'" + searchCriteria.DeveloperCriteriaProjectFollowUp.Replace(Constants.cSingleQuote, Constants.cDoubleSingleQuote).Trim + "'") + ",")
            sQuery.Append("'N',")
            sQuery.Append("'" + searchCriteria.DeveloperCriteriaProjectEntryStaffName.Trim + "'")

            sQuery.Append(")")

            MySqlConn.ConnectionString = taskerConnectStr

            MySqlConn.Open()
            MySqlCommand.Connection = MySqlConn
            MySqlCommand.CommandType = CommandType.Text
            MySqlCommand.CommandTimeout = 240

            MySqlCommand.CommandText = sQuery.ToString
            MySqlCommand.ExecuteNonQuery()

            sQuery = New StringBuilder

            sQuery.Append("INSERT INTO EMail_Queue (emailq_service, emailq_replyname, emailq_replyemail, emailq_smtp_server,")
            sQuery.Append(" emailq_smtp_username, emailq_smtp_password, emailq_status, emailq_onhold_flag, emailq_html_flag,")
            sQuery.Append(" emailq_comp_id, emailq_contact_id, emailq_sub_id, emailq_to, emailq_cc, emailq_subject, emailq_body)")
            sQuery.Append(" VALUES ('Evolution','Customer Service','customerservice@jetnet.com','smtp.jetnet.com',")
            sQuery.Append("'customerservice@jetnet.com','cservice123','Open','N','Y',")

            If CLng(HttpContext.Current.Session.Item("localUser").crmUserCompanyID.ToString) > 0 Then
                sQuery.Append(HttpContext.Current.Session.Item("localUser").crmUserCompanyID.ToString + ",")
            Else
                sQuery.Append("0,")
            End If

            If CLng(HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString) > 0 Then
                sQuery.Append(HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString + ",")
            Else
                sQuery.Append("0,")
            End If

            If CLng(HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString) > 0 Then
                sQuery.Append(HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString + ",")
            Else
                sQuery.Append("0,")
            End If

            sQuery.Append("'rick@mvintech.com',")
            sQuery.Append("'matt@mvintech.com',")
            sQuery.Append("'JETNET LLC - New Task" + IIf(Not String.IsNullOrEmpty(searchCriteria.DeveloperCriteriaProjectTaskTitle), " - " + searchCriteria.DeveloperCriteriaProjectTaskTitle.Replace(Constants.cSingleQuote, Constants.cDoubleSingleQuote).Trim, "") + "',")
            sQuery.Append("'Task Description : " + searchCriteria.DeveloperCriteriaProjectTaskDiscription.Replace(Constants.cSingleQuote, Constants.cDoubleSingleQuote).Trim)
            sQuery.Append(IIf(Not String.IsNullOrEmpty(searchCriteria.DeveloperCriteriaProjectFollowUp), vbCrLf + "Follow Up : " + searchCriteria.DeveloperCriteriaProjectFollowUp.Replace(Constants.cSingleQuote, Constants.cDoubleSingleQuote).Trim, "") + "'")
            sQuery.Append(")")

            SqlConn.ConnectionString = adminConnectStr

            SqlConn.Open()
            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 240

            SqlCommand.CommandText = sQuery.ToString

            SqlCommand.ExecuteNonQuery()

        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in insertAddNewTask(ByVal searchCriteria As developerSelectionCriteriaClass)</b><br />" + ex.Message

        Finally

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing

            SqlCommand.Dispose()
            SqlCommand = Nothing

            MySqlConn.Dispose()
            MySqlConn.Close()
            MySqlConn = Nothing

            MySqlCommand.Dispose()
            MySqlCommand = Nothing

        End Try

    End Sub

    Public Function getDevelopmentDropdownData(ByVal bGetProjectkey As Boolean) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
        Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
        Dim MySqlReader As MySql.Data.MySqlClient.MySqlDataReader = Nothing
        Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing

        Try
            If bGetProjectkey Then

                sQuery.Append("SELECT customer_abbrev, project_key, project_title FROM customer")
                sQuery.Append(" INNER JOIN projects ON customer_key = project_org_id")
                sQuery.Append(" ORDER BY project_title")

            Else

                sQuery.Append("SELECT priority_value, priority_title FROM priority")

            End If

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>getDevelopmentStaffDataTable() As DataTable</b><br />" + sQuery.ToString

            MySqlConn.ConnectionString = taskerConnectStr

            MySqlConn.Open()
            MySqlCommand.Connection = MySqlConn
            MySqlCommand.CommandType = CommandType.Text
            MySqlCommand.CommandTimeout = 240

            MySqlCommand.CommandText = sQuery.ToString

            MySqlReader = MySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                atemptable.Load(MySqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getDevelopmentStaffDataTable load datatable</b><br /> " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getDevelopmentStaffDataTable() As DataTable</b><br />" + ex.Message

        Finally
            MySqlReader = Nothing

            MySqlConn.Dispose()
            MySqlConn.Close()
            MySqlConn = Nothing

            MySqlCommand.Dispose()
            MySqlCommand = Nothing
        End Try

        Return atemptable

    End Function

    Public Sub fill_project_key_dropdown(ByRef searchCriteria As developerSelectionCriteriaClass, ByRef maxWidth As Long, ByRef ddlProjectkey As DropDownList)

        Dim results_table As New DataTable

        Try

            ddlProjectkey.Items.Clear()
            results_table = getDevelopmentDropdownData(True)

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    For Each r As DataRow In results_table.Rows

                        If Not IsDBNull(r.Item("project_title")) And Not String.IsNullOrEmpty(r.Item("project_title").ToString.Trim) Then

                            If (r.Item("project_title").ToString.Length * Constants._STARTCHARWIDTH) > maxWidth Then
                                maxWidth = (r.Item("project_title").ToString.Length * Constants._STARTCHARWIDTH)
                            End If

                            ddlProjectkey.Items.Add(New ListItem(r.Item("project_title").ToString, r.Item("project_key").ToString))

                            If Not String.IsNullOrEmpty(r.Item("project_key").ToString.Trim) Then
                                If IsNumeric(r.Item("project_key").ToString) Then
                                    If CLng(r.Item("project_key").ToString) = searchCriteria.DeveloperCriteriaProjectKey Then
                                        ddlProjectkey.SelectedValue = searchCriteria.DeveloperCriteriaProjectKey.ToString
                                    End If
                                End If
                            End If

                        End If

                    Next
                End If
            End If

            If searchCriteria.DeveloperCriteriaProjectKey = 0 Then
                ddlProjectkey.SelectedValue = ""
            End If

            ddlProjectkey.Width = (maxWidth)

        Catch ex As Exception

            aError = "Error in fill_project_key_dropdown(ByRef searchCriteria As developerSelectionCriteriaClass, ByRef maxWidth As Long, ByRef ddlProjectkey As DropDownList) " + ex.Message

        Finally

        End Try

        results_table = Nothing

    End Sub

    Public Sub fill_project_priority_dropdown(ByRef searchCriteria As developerSelectionCriteriaClass, ByRef maxWidth As Long, ByRef ddlProjectpriority As DropDownList)

        Dim results_table As New DataTable

        Try

            ddlProjectpriority.Items.Clear()

            results_table = getDevelopmentDropdownData(False)

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    For Each r As DataRow In results_table.Rows

                        If Not IsDBNull(r.Item("priority_title")) And Not String.IsNullOrEmpty(r.Item("priority_title").ToString.Trim) Then

                            If (r.Item("priority_title").ToString.Length * Constants._STARTCHARWIDTH) > maxWidth Then
                                maxWidth = (r.Item("priority_title").ToString.Length * Constants._STARTCHARWIDTH)
                            End If

                            ddlProjectpriority.Items.Add(New ListItem(r.Item("priority_title").ToString, r.Item("priority_value").ToString))

                            If Not String.IsNullOrEmpty(r.Item("priority_value").ToString.Trim) Then
                                If r.Item("priority_value").ToString = searchCriteria.DeveloperCriteriaProjectPriority Then
                                    ddlProjectpriority.SelectedValue = searchCriteria.DeveloperCriteriaProjectPriority.ToString
                                End If
                            End If

                        End If

                    Next
                End If
            End If

            If String.IsNullOrEmpty(searchCriteria.DeveloperCriteriaProjectPriority) Then
                ddlProjectpriority.SelectedValue = ""
            End If

            ddlProjectpriority.Width = (maxWidth)

        Catch ex As Exception

            aError = "Error in fill_project_priority_dropdown(ByRef searchCriteria As developerSelectionCriteriaClass, ByRef maxWidth As Long, ByRef ddlProjectpriority As ListBox) " + ex.Message

        Finally

        End Try

        results_table = Nothing

    End Sub

#End Region

#Region "admin_help_page_functions"

    Public Function getHelpListDataTable(Optional ByVal sNoteType As String = "") As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            sQuery.Append("SELECT evonotype_type, evonotype_name FROM evolution_notifications_type WITH(NOLOCK)")
            sQuery.Append(IIf(Not String.IsNullOrEmpty(sNoteType.Trim), " WHERE evonotype_type = '" + sNoteType.ToUpper.Trim + "'", ""))
            sQuery.Append(" ORDER BY evonotype_name ASC")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>getHelpListDataTable(Optional ByVal sNoteType As String = "") As DataTable</b><br />" + sQuery.ToString

            SqlConn.ConnectionString = adminConnectStr

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
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getHelpListDataTable load datatable</b><br /> " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getHelpListDataTable(Optional ByVal sNoteType As String = "") As DataTable</b><br />" + ex.Message

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

    Public Function getHelpTopicDataTable() As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            sQuery.Append("SELECT * FROM evolution_topics WITH(NOLOCK) ORDER BY evotop_name ASC")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>getHelpTopicDataTable() As DataTable</b><br />" + sQuery.ToString

            SqlConn.ConnectionString = adminConnectStr

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
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getHelpTopicDataTable load datatable</b><br /> " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getHelpTopicDataTable() As DataTable</b><br />" + ex.Message

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

    Public Function getHelpTopicIndexDataTable(ByVal sTopicID As Integer, ByVal sHelpItemID As Integer) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            sQuery.Append("SELECT * FROM evolution_topic_index WITH(NOLOCK) WHERE evotopind_evotop_id = " + sTopicID.ToString)
            sQuery.Append(" AND evotopind_evonot_id = " + sHelpItemID.ToString)

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>getHelpTopicIndexDataTable(ByVal sTopicID As Integer, ByVal sHelpItemID As Integer) As DataTable</b><br />" + sQuery.ToString

            SqlConn.ConnectionString = adminConnectStr

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
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getHelpTopicIndexDataTable load datatable</b><br /> " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getHelpTopicIndexDataTable(ByVal sTopicID As Integer, ByVal sHelpItemID As Integer) As DataTable</b><br />" + ex.Message

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

    Public Sub displayAdminHelpList(ByRef bIsAdd As Boolean, ByRef out_htmlString As String)

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder

        Try

            results_table = getHelpListDataTable()

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    htmlOut.Append("<table id=""adminHelpListTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"">")
                    htmlOut.Append("<tr><td align=""left"" valign=""middle""><strong>Help Areas</strong></td></tr>")

                    For Each r As DataRow In results_table.Rows

                        htmlOut.Append("<tr bgcolor=""white"">")
                        htmlOut.Append("<td align=""left"" valign=""middle"" style=""padding-left:4px;"">&#8226;&nbsp;<a class=""underline pointer"" href=""adminHelp.aspx?helpArea=" + r.Item("evonotype_type").ToString.Trim + """><strong>" + HttpContext.Current.Server.HtmlEncode(r.Item("evonotype_name").ToString.Trim) + "</strong></a></td>")
                        htmlOut.Append("</tr>")

                    Next

                    If Not bIsAdd Then
                        htmlOut.Append("<tr bgcolor=""white"">")
                        htmlOut.Append("<td align=""left"" valign=""middle"" style=""padding-left:4px;"">&#8226;&nbsp;<a class=""underline pointer"" href=""adminHelp.aspx?add=true""><strong><font color=""blue"">Add New Help Item</font></strong></a></td>")
                        htmlOut.Append("</tr>")
                        htmlOut.Append("<tr bgcolor=""white"">")
                        htmlOut.Append("<td align=""left"" valign=""middle"" style=""padding-left:4px;"">&#8226;&nbsp;<a class=""underline pointer"" href=""adminHelp.aspx?add=true&release=true""><strong><font color=""blue"">Add New Evo Release Image Link (Required)</font></strong></a></td>")
                        htmlOut.Append("</tr>")
                    End If

                    htmlOut.Append("</table>")

                Else
                    htmlOut.Append("<table id=""adminHelpListTable"" width=""100%"" cellpadding=""2"" cellspacing=""0""><tr><td valign=""top"" align=""left""><br/>No Help Areas Found</td></tr></table>")
                End If
            Else
                htmlOut.Append("<table id=""adminHelpListTable"" width=""100%"" cellpadding=""2"" cellspacing=""0""><tr><td valign=""top"" align=""left""><br/>No Help Areas Found</td></tr></table>")
            End If

        Catch ex As Exception

            aError = "Error in displayAdminHelpList(ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub

    Public Function getHelpListDetailDisplayDataTable(ByVal sHelpArea As String, ByVal sListOrder As String, ByVal sSortOrder As String) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            sQuery.Append("SELECT * FROM evolution_notifications")

            If Not String.IsNullOrEmpty(sHelpArea.Trim) Then
                sQuery.Append(" WHERE evonot_release_type = '" + sHelpArea.Trim + "'")

                If Not sHelpArea.ToUpper.Contains("Y") Then

                    '   If sHelpArea.ToUpper.Contains("G") Then
                    'sQuery.Append(" OR evonot_release_type ='R'")
                    '  End If

                    If sHelpArea.ToUpper.Contains("B") Then
                        sQuery.Append(" OR evonot_release_type ='J'")
                    End If

                End If

            End If

            If sListOrder.ToUpper.Contains("DATE") Then
                sQuery.Append(" ORDER BY evonot_release_date " + sSortOrder.Trim)
            ElseIf sListOrder.ToUpper.Contains("TITLE") Then
                sQuery.Append(" ORDER BY evonot_title " + sSortOrder.Trim)
            ElseIf sListOrder.ToUpper.Contains("TYPE") Then
                sQuery.Append(" ORDER BY evonot_release_type " + sSortOrder.Trim)
            Else
                sQuery.Append(" ORDER BY evonot_release_date DESC")
            End If

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>getHelpListDetailDisplayDataTable(ByVal sHelpArea As String, ByVal sListOrder As String, ByVal sSortOrder As String) As DataTable</b><br />" + sQuery.ToString

            SqlConn.ConnectionString = adminConnectStr

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
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getHelpListDetailDisplayDataTable load datatable</b><br /> " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getHelpListDetailDisplayDataTable(ByVal sHelpArea As String, ByVal sListOrder As String, ByVal sSortOrder As String) As DataTable</b><br />" + ex.Message

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

    Public Sub displayAdminDetailHelpList(ByVal sHelpArea As String, ByVal sListOrder As String, ByVal sSortOrder As String, ByRef out_htmlString As String)

        Dim results_table As New DataTable
        Dim toggleRowColor As Boolean = False
        Dim htmlOut As New StringBuilder

        Dim bIsRelease As Boolean = False

        Dim sHelpAreaTitle As String = "All Items"

        Try

            results_table = getHelpListDetailDisplayDataTable(sHelpArea, sListOrder, sSortOrder)

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    Dim areaTitle_table As DataTable = getHelpListDataTable(sHelpArea)

                    If Not IsNothing(areaTitle_table) Then
                        If areaTitle_table.Rows.Count > 0 Then
                            sHelpAreaTitle = "Help Items - " + areaTitle_table.Rows(0).Item("evonotype_name").ToString.Trim
                        End If
                    End If

                    htmlOut.Append("<table id=""adminHelpListDetailTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"" border=""0"">")

                    htmlOut.Append("<tr><td align=""center"" valign=""middle"" colspan=""16""><b>" + sHelpAreaTitle.Trim + "</b></td></tr>")

                    htmlOut.Append("<tr><td align=""center"" valign=""middle"">ACTIVE</td>")
                    htmlOut.Append("<td align=""center"" valign=""middle"">ADMIN ONLY</td>")
                    htmlOut.Append("<td align=""center"" valign=""middle""><a class=""underline pointer"" href=""adminHelp.aspx?displayOrder=date&helpArea=" + sHelpArea.Trim + "&sortOrder=" + sSortOrder.Trim + """>RELEASE DATE</a></td>")
                    htmlOut.Append("<td align=""center"" valign=""middle""><a class=""underline pointer"" href=""adminHelp.aspx?displayOrder=title&helpArea=" + sHelpArea.Trim + "&sortOrder=" + sSortOrder.Trim + """>TITLE</a></td>")
                    htmlOut.Append("<td align=""center"" valign=""middle"">SUBID</td>")
                    htmlOut.Append("<td align=""center"" valign=""middle"">COMPID</td>")
                    htmlOut.Append("<td align=""center"" valign=""middle"">BUS</td>")
                    htmlOut.Append("<td align=""center"" valign=""middle"">HELI</td>")
                    htmlOut.Append("<td align=""center"" valign=""middle"">COM</td>")
                    htmlOut.Append("<td align=""center"" valign=""middle"">CRM</td>")
                    htmlOut.Append("<td align=""center"" valign=""middle"">DOC URL</td>")
                    htmlOut.Append("<td align=""center"" valign=""middle"">VIDEO</td>")
                    htmlOut.Append("<td align=""center"" valign=""middle"">NEW EVO</td>")
                    htmlOut.Append("<td align=""center"" valign=""middle"">OLD EVO</td>")
                    htmlOut.Append("<td align=""center"" valign=""middle""><a class=""underline pointer"" href=""adminHelp.aspx?displayOrder=type&helpArea=" + sHelpArea.Trim + "&sortOrder=" + sSortOrder.Trim + """>TYPE</a></td>")
                    htmlOut.Append("</tr>")

                    For Each r As DataRow In results_table.Rows

                        If Not toggleRowColor Then
                            htmlOut.Append("<tr class=""alt_row"">")
                            toggleRowColor = True
                        Else
                            htmlOut.Append("<tr bgcolor=""white"">")
                            toggleRowColor = False
                        End If

                        If Not IsDBNull(r.Item("evonot_active_flag")) Then
                            If Not String.IsNullOrEmpty(r.Item("evonot_active_flag").ToString.Trim) Then

                                If r.Item("evonot_active_flag").ToString.Trim.Contains("Y") Then
                                    htmlOut.Append("<td align=""center"" valign=""top""><img width=""20"" height=""20"" alt=""Yes"" src=""images/evo_green_check.png""></td>")
                                Else
                                    htmlOut.Append("<td align=""center"" valign=""top""><img width=""20"" height=""20"" alt=""No"" src=""images/evo_red_check.png""></td>")
                                End If

                            Else
                                htmlOut.Append("<td align=""center"" valign=""top""><img width=""20"" height=""20"" alt=""No"" src=""images/evo_red_check.png""></td>")
                            End If
                        Else
                            htmlOut.Append("<td align=""center"" valign=""top""><img width=""20"" height=""20"" alt=""No"" src=""images/evo_red_check.png""></td>")
                        End If

                        If Not IsDBNull(r.Item("evonot_admin_flag")) Then
                            If Not String.IsNullOrEmpty(r.Item("evonot_admin_flag").ToString.Trim) Then

                                If r.Item("evonot_admin_flag").ToString.Trim.Contains("Y") Then
                                    htmlOut.Append("<td align=""center"" valign=""top""><img width=""20"" height=""20"" alt=""Yes"" src=""images/evo_green_check.png""></td>")
                                Else
                                    htmlOut.Append("<td align=""center"" valign=""top""><img width=""20"" height=""20"" alt=""No"" src=""images/evo_red_check.png""></td>")
                                End If

                            Else
                                htmlOut.Append("<td align=""center"" valign=""top""><img width=""20"" height=""20"" alt=""No"" src=""images/evo_red_check.png""></td>")
                            End If
                        Else
                            htmlOut.Append("<td align=""center"" valign=""top""><img width=""20"" height=""20"" alt=""No"" src=""images/evo_red_check.png""></td>")
                        End If

                        If Not IsDBNull(r.Item("evonot_release_date")) Then
                            If Not String.IsNullOrEmpty(r.Item("evonot_release_date").ToString.Trim) Then
                                htmlOut.Append("<td align=""left"" valign=""top"">" + FormatDateTime(CDate(r.Item("evonot_release_date").ToString.Trim), DateFormat.ShortDate) + "</td>")
                            Else
                                htmlOut.Append("<td align=""left""></td>")
                            End If
                        Else
                            htmlOut.Append("<td align=""left""></td>")
                        End If

                        bIsRelease = False

                        If Not IsDBNull(r.Item("evonot_release_type")) Then
                            If Not String.IsNullOrEmpty(r.Item("evonot_release_type").ToString.Trim) Then
                                If r.Item("evonot_release_type").ToString.Trim.ToUpper.Contains("R") Then
                                    bIsRelease = True
                                End If
                            End If
                        End If

                        If Not IsDBNull(r.Item("evonot_title")) Then
                            If Not String.IsNullOrEmpty(r.Item("evonot_title").ToString.Trim) Then
                                htmlOut.Append("<td align=""left"" valign=""top"" height=""48""><a class=""underline pointer"" href=""adminHelp.aspx?helpId=" + r.Item("evonot_id").ToString.Trim + "&release=" + IIf(bIsRelease, "true", "false") + "&edit=true"">" + r.Item("evonot_title").ToString.Trim + "</a></td>")
                            Else
                                htmlOut.Append("<td align=""left""></td>")
                            End If
                        Else
                            htmlOut.Append("<td align=""left""></td>")
                        End If

                        If Not IsDBNull(r.Item("evonot_sub_id")) Then
                            If Not String.IsNullOrEmpty(r.Item("evonot_sub_id").ToString.Trim) Then
                                htmlOut.Append("<td align=""left"" valign=""top"">" + r.Item("evonot_sub_id").ToString.Trim + "</td>")
                            Else
                                htmlOut.Append("<td align=""left""></td>")
                            End If
                        Else
                            htmlOut.Append("<td align=""left""></td>")
                        End If

                        If Not IsDBNull(r.Item("evonot_comp_id")) Then
                            If Not String.IsNullOrEmpty(r.Item("evonot_comp_id").ToString.Trim) Then
                                htmlOut.Append("<td align=""left"" valign=""top"">" + r.Item("evonot_comp_id").ToString.Trim + "</td>")
                            Else
                                htmlOut.Append("<td align=""left""></td>")
                            End If
                        Else
                            htmlOut.Append("<td align=""left""></td>")
                        End If

                        If Not IsDBNull(r.Item("evonot_product_business_flag")) Then
                            If Not String.IsNullOrEmpty(r.Item("evonot_product_business_flag").ToString.Trim) Then

                                If r.Item("evonot_product_business_flag").ToString.Trim.Contains("Y") Then
                                    htmlOut.Append("<td align=""center"" valign=""top""><img width=""20"" height=""20"" alt=""Yes"" src=""images/evo_green_check.png""></td>")
                                Else
                                    htmlOut.Append("<td align=""center"" valign=""top""><img width=""20"" height=""20"" alt=""No"" src=""images/evo_red_check.png""></td>")
                                End If

                            Else
                                htmlOut.Append("<td align=""center"" valign=""top""><img width=""20"" height=""20"" alt=""No"" src=""images/evo_red_check.png""></td>")
                            End If
                        Else
                            htmlOut.Append("<td align=""center"" valign=""top""><img width=""20"" height=""20"" alt=""No"" src=""images/evo_red_check.png""></td>")
                        End If

                        If Not IsDBNull(r.Item("evonot_product_helicopter_flag")) Then
                            If Not String.IsNullOrEmpty(r.Item("evonot_product_helicopter_flag").ToString.Trim) Then

                                If r.Item("evonot_product_helicopter_flag").ToString.Trim.Contains("Y") Then
                                    htmlOut.Append("<td align=""center"" valign=""top""><img width=""20"" height=""20"" alt=""Yes"" src=""images/evo_green_check.png""></td>")
                                Else
                                    htmlOut.Append("<td align=""center"" valign=""top""><img width=""20"" height=""20"" alt=""No"" src=""images/evo_red_check.png""></td>")
                                End If

                            Else
                                htmlOut.Append("<td align=""center"" valign=""top""><img width=""20"" height=""20"" alt=""No"" src=""images/evo_red_check.png""></td>")
                            End If
                        Else
                            htmlOut.Append("<td align=""center"" valign=""top""><img width=""20"" height=""20"" alt=""No"" src=""images/evo_red_check.png""></td>")
                        End If

                        If Not IsDBNull(r.Item("evonot_product_commercial_flag")) Then
                            If Not String.IsNullOrEmpty(r.Item("evonot_product_commercial_flag").ToString.Trim) Then

                                If r.Item("evonot_product_commercial_flag").ToString.Trim.Contains("Y") Then
                                    htmlOut.Append("<td align=""center"" valign=""top""><img width=""20"" height=""20"" alt=""Yes"" src=""images/evo_green_check.png""></td>")
                                Else
                                    htmlOut.Append("<td align=""center"" valign=""top""><img width=""20"" height=""20"" alt=""No"" src=""images/evo_red_check.png""></td>")
                                End If

                            Else
                                htmlOut.Append("<td align=""center"" valign=""top""><img width=""20"" height=""20"" alt=""No"" src=""images/evo_red_check.png""></td>")
                            End If
                        Else
                            htmlOut.Append("<td align=""center"" valign=""top""><img width=""20"" height=""20"" alt=""No"" src=""images/evo_red_check.png""></td>")
                        End If

                        If Not IsDBNull(r.Item("evonot_product_crm_flag")) Then
                            If Not String.IsNullOrEmpty(r.Item("evonot_product_crm_flag").ToString.Trim) Then

                                If r.Item("evonot_product_crm_flag").ToString.Trim.Contains("Y") Then
                                    htmlOut.Append("<td align=""center"" valign=""top""><img width=""20"" height=""20"" alt=""Yes"" src=""images/evo_green_check.png""></td>")
                                Else
                                    htmlOut.Append("<td align=""center"" valign=""top""><img width=""20"" height=""20"" alt=""No"" src=""images/evo_red_check.png""></td>")
                                End If

                            Else
                                htmlOut.Append("<td align=""center"" valign=""top""><img width=""20"" height=""20"" alt=""No"" src=""images/evo_red_check.png""></td>")
                            End If
                        Else
                            htmlOut.Append("<td align=""center"" valign=""top""><img width=""20"" height=""20"" alt=""No"" src=""images/evo_red_check.png""></td>")
                        End If

                        If Not IsDBNull(r.Item("evonot_doc_link")) Then
                            If Not String.IsNullOrEmpty(r.Item("evonot_doc_link").ToString.Trim) Then

                                htmlOut.Append("<td align=""center"" valign=""top""><img width=""20"" height=""20"" alt=""Yes"" src=""images/evo_green_check.png""></td>")

                            Else
                                htmlOut.Append("<td align=""center"" valign=""top""><img width=""20"" height=""20"" alt=""No"" src=""images/evo_red_check.png""></td>")
                            End If
                        Else
                            htmlOut.Append("<td align=""center"" valign=""top""><img width=""20"" height=""20"" alt=""No"" src=""images/evo_red_check.png""></td>")
                        End If

                        If Not IsDBNull(r.Item("evonot_video")) Then
                            If Not String.IsNullOrEmpty(r.Item("evonot_video").ToString.Trim) Then

                                htmlOut.Append("<td align=""center"" valign=""top""><img width=""20"" height=""20"" alt=""Yes"" src=""images/evo_green_check.png""></td>")

                            Else
                                htmlOut.Append("<td align=""center"" valign=""top""><img width=""20"" height=""20"" alt=""No"" src=""images/evo_red_check.png""></td>")
                            End If
                        Else
                            htmlOut.Append("<td align=""center"" valign=""top""><img width=""20"" height=""20"" alt=""No"" src=""images/evo_red_check.png""></td>")
                        End If

                        If Not IsDBNull(r.Item("evonot_evo_dotnet_flag")) Then
                            If Not String.IsNullOrEmpty(r.Item("evonot_evo_dotnet_flag").ToString.Trim) Then

                                If r.Item("evonot_evo_dotnet_flag").ToString.Trim.Contains("Y") Then
                                    htmlOut.Append("<td align=""center"" valign=""top""><img width=""20"" height=""20"" alt=""Yes"" src=""images/evo_green_check.png""></td>")
                                Else
                                    htmlOut.Append("<td align=""center"" valign=""top""><img width=""20"" height=""20"" alt=""No"" src=""images/evo_red_check.png""></td>")
                                End If

                            Else
                                htmlOut.Append("<td align=""center"" valign=""top""><img width=""20"" height=""20"" alt=""No"" src=""images/evo_red_check.png""></td>")
                            End If
                        Else
                            htmlOut.Append("<td align=""center"" valign=""top""><img width=""20"" height=""20"" alt=""No"" src=""images/evo_red_check.png""></td>")
                        End If

                        If Not IsDBNull(r.Item("evonot_evo_asp_flag")) Then
                            If Not String.IsNullOrEmpty(r.Item("evonot_evo_asp_flag").ToString.Trim) Then

                                If r.Item("evonot_evo_asp_flag").ToString.Trim.Contains("Y") Then
                                    htmlOut.Append("<td align=""center"" valign=""top""><img width=""20"" height=""20"" alt=""Yes"" src=""images/evo_green_check.png""></td>")
                                Else
                                    htmlOut.Append("<td align=""center"" valign=""top""><img width=""20"" height=""20"" alt=""No"" src=""images/evo_red_check.png""></td>")
                                End If

                            Else
                                htmlOut.Append("<td align=""center"" valign=""top""><img width=""20"" height=""20"" alt=""No"" src=""images/evo_red_check.png""></td>")
                            End If
                        Else
                            htmlOut.Append("<td align=""center"" valign=""top""><img width=""20"" height=""20"" alt=""No"" src=""images/evo_red_check.png""></td>")
                        End If

                        If Not IsDBNull(r.Item("evonot_release_type")) Then
                            If Not String.IsNullOrEmpty(r.Item("evonot_release_type").ToString.Trim) Then

                                Select Case (r.Item("evonot_release_type").ToString.Trim.ToUpper)

                                    Case "G"
                                        htmlOut.Append("<td align=""center"" valign=""top""><img width=""20"" height=""20"" src=""images/document.png"" title=""Release Note"" alt=""Release Note""></td>")
                                    Case "B"
                                        htmlOut.Append("<td align=""center"" valign=""top""><img width=""20"" height=""20"" src=""images/papers.png"" title=""Bulletin Board"" alt=""Bulletin Board""></td>")
                                    Case "J"
                                        htmlOut.Append("<td align=""center"" valign=""top""><img width=""20"" height=""20"" src=""images/jbul.png"" title=""Bulletin Board"" alt=""Bulletin Board""></td>")
                                    Case "R"
                                        htmlOut.Append("<td align=""center"" valign=""top""><img width=""20"" height=""20"" src=""images/red_pin.png"" title=""Subscriber Required"" alt=""Subscriber Required""></td>")
                                    Case "JC"
                                        htmlOut.Append("<td align=""center"" valign=""top""><img width=""20"" height=""20"" src=""images/final.png"" title=""Calendar"" alt=""Calendar""></td>")
                                    Case "H"
                                        htmlOut.Append("<td align=""center"" valign=""top""><img width=""20"" height=""20"" src=""images/info.png"" title=""Help Note"" alt=""Help Note""></td>")
                                    Case "L"
                                        htmlOut.Append("<td align=""center"" valign=""top""><img width=""20"" height=""20"" src=""images/base.png"" title=""Help Note"" alt=""Help Note""></td>")
                                    Case "N"
                                        htmlOut.Append("<td align=""center"" valign=""top""><img width=""20"" height=""20"" src=""images/binoculars.png"" title=""News"" alt=""News""></td>")
                                    Case "ML"
                                        htmlOut.Append("<td align=""center"" valign=""top""><img width=""20"" height=""20"" src=""images/evoPlane.png"" title=""Model"" alt=""Model""></td>")
                                    Case "EH"
                                        htmlOut.Append("<td align=""center"" valign=""top""><img width=""20"" height=""20"" src=""images/light.png"" title=""Hint"" alt=""Hint""></td>")
                                    Case "MG"
                                        htmlOut.Append("<td align=""center"" valign=""top""><img width=""20"" height=""20"" src=""images/myCRM_small.png"" title=""MPM"" alt=""MPM""></td>")
                                    Case "MH"
                                        htmlOut.Append("<td align=""center"" valign=""top""><img width=""20"" height=""20"" src=""images/myCRM_small.png"" title=""MPM"" alt=""MPM""></td>")
                                    Case Else
                                        htmlOut.Append("<td align=""center"" valign=""top""><img width=""20"" height=""20"" src=""images/delete_icon.png"" title=""Unknown"" alt=""Unknown""></td>")

                                End Select

                            Else
                                htmlOut.Append("<td align=""center"" valign=""top""></td>")
                            End If
                        Else
                            htmlOut.Append("<td align=""center"" valign=""top""></td>")
                        End If

                        htmlOut.Append("</tr>")

                    Next

                    htmlOut.Append("</table>")

                Else
                    htmlOut.Append("<table id=""adminHelpListDetailTable"" width=""100%"" cellpadding=""2"" cellspacing=""0""><tr><td valign=""top"" align=""left""><br/>No Help Items Found</td></tr></table>")
                End If
            Else
                htmlOut.Append("<table id=""adminHelpListDetailTable"" width=""100%"" cellpadding=""2"" cellspacing=""0""><tr><td valign=""top"" align=""left""><br/>No Help Items Found</td></tr></table>")
            End If

        Catch ex As Exception

            aError = "Error in displayAdminDetailHelpList(ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub

    Public Function getHelpDetailsDisplayDataTable(ByVal nItemID As Integer) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            sQuery.Append("SELECT * FROM evolution_notifications")
            sQuery.Append(" WHERE evonot_id = " + nItemID.ToString.Trim)

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>getHelpDetailsDisplayDataTable(ByVal nItemID As Integer) As DataTable</b><br />" + sQuery.ToString

            SqlConn.ConnectionString = adminConnectStr

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
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getHelpDetailsDisplayDataTable load datatable</b><br /> " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getHelpDetailsDisplayDataTable(ByVal nItemID As Integer) As DataTable</b><br />" + ex.Message

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

    Public Sub fill_help_release_type_dropdown(ByVal sHelpType As String, ByRef maxWidth As Long, ByRef ddlReleaseType As DropDownList, Optional ByVal bIsReleaseNote As Boolean = False)

        Dim results_table As New DataTable

        Try

            ddlReleaseType.Items.Clear()
            results_table = getHelpListDataTable()

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    For Each r As DataRow In results_table.Rows

                        If Not IsDBNull(r.Item("evonotype_name")) And Not String.IsNullOrEmpty(r.Item("evonotype_name").ToString.Trim) Then

                            If bIsReleaseNote And r.Item("evonotype_type").ToString.ToUpper.Contains("R") Then ' show only release item

                                If (r.Item("evonotype_name").ToString.Length * Constants._STARTCHARWIDTH) > maxWidth Then
                                    maxWidth = (r.Item("evonotype_name").ToString.Length * Constants._STARTCHARWIDTH)
                                End If

                                ddlReleaseType.Items.Add(New ListItem(r.Item("evonotype_name").ToString, r.Item("evonotype_type").ToString))

                                If Not String.IsNullOrEmpty(r.Item("evonotype_type").ToString.Trim) Then
                                    If r.Item("evonotype_type").ToString.ToUpper.Contains(sHelpType) Then
                                        ddlReleaseType.SelectedValue = sHelpType
                                    End If
                                End If

                                Exit For

                            ElseIf Not bIsReleaseNote Then

                                If (r.Item("evonotype_name").ToString.Length * Constants._STARTCHARWIDTH) > maxWidth Then
                                    maxWidth = (r.Item("evonotype_name").ToString.Length * Constants._STARTCHARWIDTH)
                                End If

                                ddlReleaseType.Items.Add(New ListItem(r.Item("evonotype_name").ToString, r.Item("evonotype_type").ToString))

                                If Not String.IsNullOrEmpty(r.Item("evonotype_type").ToString.Trim) Then
                                    If r.Item("evonotype_type").ToString.ToUpper.Contains(sHelpType) Then
                                        ddlReleaseType.SelectedValue = sHelpType
                                    End If
                                End If

                            End If

                        End If

                    Next
                End If
            End If

            If String.IsNullOrEmpty(sHelpType.Trim) Then
                ddlReleaseType.SelectedValue = ""
            End If

            ddlReleaseType.Width = (maxWidth)

        Catch ex As Exception

            aError = "Error in fill_help_release_type_dropdown(ByVal sHelpType as String, ByRef maxWidth As Long, ByRef ddlReleaseType As DropDownList) " + ex.Message

        Finally

        End Try

        results_table = Nothing

    End Sub

    Public Sub fill_help_release_topic_checkbox_dropdown(ByVal nItemID As Integer, ByRef maxWidth As Long, ByRef ddlReleaseTopic As CheckBoxList)

        Dim results_table As New DataTable
        Dim topic_results_table As New DataTable

        Try

            ddlReleaseTopic.Items.Clear()
            results_table = getHelpTopicDataTable()

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    For Each r As DataRow In results_table.Rows

                        If Not IsDBNull(r.Item("evotop_name")) And Not String.IsNullOrEmpty(r.Item("evotop_name").ToString.Trim) Then

                            If (r.Item("evotop_name").ToString.Length * Constants._STARTCHARWIDTH) > maxWidth Then
                                maxWidth = (r.Item("evotop_name").ToString.Length * Constants._STARTCHARWIDTH)
                            End If

                            ddlReleaseTopic.Items.Add(New ListItem(r.Item("evotop_name").ToString, r.Item("evotop_id").ToString))

                            If nItemID > 0 Then

                                ' check to see if this topic ID has been entered into the "index" table for this Item
                                topic_results_table = getHelpTopicIndexDataTable(CLng(r.Item("evotop_id").ToString), nItemID)

                                If Not IsNothing(topic_results_table) Then
                                    If topic_results_table.Rows.Count > 0 Then
                                        Dim currentCheckBox As ListItem = ddlReleaseTopic.Items.FindByValue(r.Item("evotop_id").ToString)
                                        currentCheckBox.Selected = True
                                    End If
                                End If

                                topic_results_table = Nothing

                            End If

                        End If

                    Next

                End If
            End If

        Catch ex As Exception

            aError = "Error in fill_help_release_topic_checkbox_dropdown(ByVal nItemID As Integer, ByRef maxWidth As Long, ByRef ddlReleaseTopic As CheckBoxList) " + ex.Message

        Finally

        End Try

        results_table = Nothing
        topic_results_table = Nothing

    End Sub

    Public Sub getAdminDetailHelpItem(ByVal nHelpItemID As Integer, ByRef helpCriteria As helpAdminSelectionCriteriaClass)

        Dim results_table As New DataTable
        Dim toggleRowColor As Boolean = False
        Dim htmlOut As New StringBuilder

        Dim sHelpAreaTitle As String = "All Items"

        Try

            results_table = getHelpDetailsDisplayDataTable(nHelpItemID)

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    For Each r As DataRow In results_table.Rows

                        If Not IsDBNull(r.Item("evonot_amod_id")) Then
                            If Not String.IsNullOrEmpty(r.Item("evonot_amod_id").ToString.Trim) Then
                                helpCriteria.HelpCriteriaModelID = CLng(r.Item("evonot_amod_id").ToString.Trim)
                            End If
                        End If

                        If Not IsDBNull(r.Item("evonot_active_flag")) Then
                            If Not String.IsNullOrEmpty(r.Item("evonot_active_flag").ToString.Trim) Then
                                helpCriteria.HelpCriteriaItemStatus = IIf(r.Item("evonot_active_flag").ToString.Trim.Contains("Y"), True, False)
                            End If
                        End If

                        If Not IsDBNull(r.Item("evonot_release_date")) Then
                            If Not String.IsNullOrEmpty(r.Item("evonot_release_date").ToString.Trim) Then
                                helpCriteria.HelpCriteriaItemReleaseDate = FormatDateTime(CDate(r.Item("evonot_release_date").ToString.Trim), DateFormat.ShortDate)
                            End If
                        End If

                        If Not IsDBNull(r.Item("evonot_title")) Then
                            If Not String.IsNullOrEmpty(r.Item("evonot_title").ToString.Trim) Then
                                helpCriteria.HelpCriteriaItemTitle = r.Item("evonot_title").ToString.Trim.Replace("&#39;", Constants.cSingleQuote)
                            End If
                        End If

                        If Not IsDBNull(r.Item("evonot_release_type")) Then
                            If Not String.IsNullOrEmpty(r.Item("evonot_release_type").ToString.Trim) Then
                                helpCriteria.HelpCriteriaItemReleaseType = r.Item("evonot_release_type").ToString.Trim
                            End If
                        End If

                        If Not IsDBNull(r.Item("evonot_announcement")) Then
                            If Not String.IsNullOrEmpty(r.Item("evonot_announcement").ToString.Trim) Then
                                helpCriteria.HelpCriteriaItemDiscription = r.Item("evonot_announcement").ToString.Trim
                            End If
                        End If

                        If Not IsDBNull(r.Item("evonot_description")) Then
                            If Not String.IsNullOrEmpty(r.Item("evonot_description").ToString.Trim) Then
                                helpCriteria.HelpCriteriaItemViewNumber = r.Item("evonot_description").ToString.Trim
                            End If
                        End If

                        If Not IsDBNull(r.Item("evonot_tabs")) Then
                            If Not String.IsNullOrEmpty(r.Item("evonot_tabs").ToString.Trim) Then
                                helpCriteria.HelpCriteriaItemTabName = r.Item("evonot_tabs").ToString.Trim
                            End If
                        End If

                        If Not IsDBNull(r.Item("evonot_video")) Then
                            If Not String.IsNullOrEmpty(r.Item("evonot_video").ToString.Trim) Then
                                helpCriteria.HelpCriteriaItemVideoLink = r.Item("evonot_video").ToString.Trim
                            End If
                        End If
                        ' jetnet product "check boxes"                  

                        If Not IsDBNull(r.Item("evonot_product_business_flag")) Then
                            If Not String.IsNullOrEmpty(r.Item("evonot_product_business_flag").ToString.Trim) Then
                                helpCriteria.HelpCriteriaBusinessFlag = IIf(r.Item("evonot_product_business_flag").ToString.Trim.Contains("Y"), True, False)
                            End If
                        End If

                        If Not IsDBNull(r.Item("evonot_product_helicopter_flag")) Then
                            If Not String.IsNullOrEmpty(r.Item("evonot_product_helicopter_flag").ToString.Trim) Then
                                helpCriteria.HelpCriteriaHelicopterFlag = IIf(r.Item("evonot_product_helicopter_flag").ToString.Trim.Contains("Y"), True, False)
                            End If
                        End If

                        If Not IsDBNull(r.Item("evonot_product_commercial_flag")) Then
                            If Not String.IsNullOrEmpty(r.Item("evonot_product_commercial_flag").ToString.Trim) Then
                                helpCriteria.HelpCriteriaCommercialFlag = IIf(r.Item("evonot_product_commercial_flag").ToString.Trim.Contains("Y"), True, False)
                            End If
                        End If

                        'If Not IsDBNull(r.Item("evonot_product_yacht_flag")) Then
                        '  If Not String.IsNullOrEmpty(r.Item("evonot_product_yacht_flag").ToString.Trim) Then
                        '    helpCriteria.HelpCriteriaYachtFlag = IIf(r.Item("evonot_product_yacht_flag").ToString.Trim.Contains("Y"), True, False)
                        '  End If
                        'End If

                        If Not IsDBNull(r.Item("evonot_evo_dotnet_only_flag")) Then
                            If Not String.IsNullOrEmpty(r.Item("evonot_evo_dotnet_only_flag").ToString.Trim) Then
                                helpCriteria.HelpCriteriaNewEvoOnlyFlag = IIf(r.Item("evonot_evo_dotnet_only_flag").ToString.Trim.Contains("Y"), True, False)
                            End If
                        End If

                        If Not IsDBNull(r.Item("evonot_evo_dotnet_flag")) Then
                            If Not String.IsNullOrEmpty(r.Item("evonot_evo_dotnet_flag").ToString.Trim) Then
                                helpCriteria.HelpCriteriaNewEvoFlag = IIf(r.Item("evonot_evo_dotnet_flag").ToString.Trim.Contains("Y"), True, False)
                            End If
                        End If

                        If Not IsDBNull(r.Item("evonot_evo_asp_flag")) Then
                            If Not String.IsNullOrEmpty(r.Item("evonot_evo_asp_flag").ToString.Trim) Then
                                helpCriteria.HelpCriteriaOldEvoFlag = IIf(r.Item("evonot_evo_asp_flag").ToString.Trim.Contains("Y"), True, False)
                            End If
                        End If

                        If Not IsDBNull(r.Item("evonot_product_crm_flag")) Then
                            If Not String.IsNullOrEmpty(r.Item("evonot_product_crm_flag").ToString.Trim) Then
                                helpCriteria.HelpCriteriaCRMFlag = IIf(r.Item("evonot_product_crm_flag").ToString.Trim.Contains("Y"), True, False)
                            End If
                        End If

                        If Not IsDBNull(r.Item("evonot_doc_link")) Then
                            If Not String.IsNullOrEmpty(r.Item("evonot_doc_link").ToString.Trim) Then
                                helpCriteria.HelpCriteriaItemDocumentLink = r.Item("evonot_doc_link").ToString.Trim
                            End If
                        End If

                        If Not IsDBNull(r.Item("evonot_sub_id")) Then
                            If Not String.IsNullOrEmpty(r.Item("evonot_sub_id").ToString.Trim) Then
                                helpCriteria.HelpCriteriaSubID = CLng(r.Item("evonot_sub_id").ToString.Trim)
                            End If
                        End If

                        If Not IsDBNull(r.Item("evonot_comp_id")) Then
                            If Not String.IsNullOrEmpty(r.Item("evonot_comp_id").ToString.Trim) Then
                                helpCriteria.HelpCriteriaCompanyID = CLng(r.Item("evonot_comp_id").ToString.Trim)
                            End If
                        End If

                        If Not IsDBNull(r.Item("evonot_admin_flag")) Then
                            If Not String.IsNullOrEmpty(r.Item("evonot_admin_flag").ToString.Trim) Then
                                helpCriteria.HelpCriteriaAdminOnly = IIf(r.Item("evonot_admin_flag").ToString.Trim.Contains("Y"), True, False)
                            End If
                        End If

                    Next

                End If
            End If


        Catch ex As Exception

            aError = "Error in getAdminDetailHelpItem(ByVal nHelpItemID As Integer, ByRef helpCriteria As helpAdminSelectionCriteriaClass) " + ex.Message

        Finally

        End Try

        htmlOut = Nothing
        results_table = Nothing

    End Sub

    Public Function insertOrUpdateHelpItem(ByVal bIsUpdateItem As Boolean, ByVal helpCriteria As helpAdminSelectionCriteriaClass, ByRef oUploadFile As FileUpload) As Boolean

        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader = Nothing
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing '

        Dim bReturnStatus As Boolean = False

        Try

            SqlConn.ConnectionString = adminConnectStr

            SqlConn.Open()
            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 240

            If bIsUpdateItem Then

                sQuery.Append("UPDATE Evolution_Notifications SET")

                sQuery.Append(" evonot_active_flag = " + IIf(helpCriteria.HelpCriteriaItemStatus, "'Y'", "'N'") + ",")
                sQuery.Append(" evonot_release_date = '" + helpCriteria.HelpCriteriaItemReleaseDate + "',")
                sQuery.Append(" evonot_title = " + IIf(String.IsNullOrEmpty(helpCriteria.HelpCriteriaItemTitle.Trim), "null", "'" + helpCriteria.HelpCriteriaItemTitle.Trim + "'") + ",")
                sQuery.Append(" evonot_release_type = '" + helpCriteria.HelpCriteriaItemReleaseType + "',")

                If helpCriteria.HelpCriteriaModelID > -1 Then
                    sQuery.Append(" evonot_amod_id = " + helpCriteria.HelpCriteriaModelID.ToString + ",")
                End If

                If helpCriteria.HelpCriteriaSubID > -1 Then
                    sQuery.Append(" evonot_sub_id = " + helpCriteria.HelpCriteriaSubID.ToString + ",")
                End If

                If helpCriteria.HelpCriteriaCompanyID > -1 Then
                    sQuery.Append(" evonot_comp_id = " + helpCriteria.HelpCriteriaCompanyID.ToString + ",")
                End If

                sQuery.Append(" evonot_admin_flag = " + IIf(helpCriteria.HelpCriteriaAdminOnly, "'Y'", "'N'") + ",")

                sQuery.Append(" evonot_description = " + IIf(String.IsNullOrEmpty(helpCriteria.HelpCriteriaItemViewNumber.Trim), "null", "'" + helpCriteria.HelpCriteriaItemViewNumber.Trim + "'") + ",")
                sQuery.Append(" evonot_tabs = " + IIf(String.IsNullOrEmpty(helpCriteria.HelpCriteriaItemTabName.Trim), "null", "'" + helpCriteria.HelpCriteriaItemTabName.Trim + "'") + ",")

                sQuery.Append(" evonot_announcement = " + IIf(String.IsNullOrEmpty(helpCriteria.HelpCriteriaItemDiscription.Trim), "null", "'" + helpCriteria.HelpCriteriaItemDiscription.Trim + "'") + ",")
                sQuery.Append(" evonot_video = " + IIf(String.IsNullOrEmpty(helpCriteria.HelpCriteriaItemVideoLink.Trim), "null", "'" + helpCriteria.HelpCriteriaItemVideoLink.Trim + "'") + ",")      '

                sQuery.Append(" evonot_product_business_flag = " + IIf(helpCriteria.HelpCriteriaBusinessFlag, "'Y'", "'N'") + ",")
                sQuery.Append(" evonot_product_helicopter_flag = " + IIf(helpCriteria.HelpCriteriaHelicopterFlag, "'Y'", "'N'") + ",")
                sQuery.Append(" evonot_product_commercial_flag = " + IIf(helpCriteria.HelpCriteriaCommercialFlag, "'Y'", "'N'") + ",")

                'sQuery.Append(" evonot_product_yacht_flag = " + IIf(helpCriteria.HelpCriteriaYachtFlag, "'Y'", "'N'") + ",")

                sQuery.Append(" evonot_evo_dotnet_only_flag = " + IIf(helpCriteria.HelpCriteriaNewEvoOnlyFlag, "'Y'", "'N'") + ",")
                sQuery.Append(" evonot_evo_dotnet_flag = " + IIf(helpCriteria.HelpCriteriaNewEvoFlag, "'Y'", "'N'") + ",")
                sQuery.Append(" evonot_evo_asp_flag = " + IIf(helpCriteria.HelpCriteriaOldEvoFlag, "'Y'", "'N'") + ",")
                sQuery.Append(" evonot_product_crm_flag = " + IIf(helpCriteria.HelpCriteriaCRMFlag, "'Y'", "'N'") + ",")

                sQuery.Append(" evonot_doc_link = " + IIf(String.IsNullOrEmpty(helpCriteria.HelpCriteriaItemDocumentLink.Trim), "null", "'" + helpCriteria.HelpCriteriaItemDocumentLink.Trim + "'") + ",")

                sQuery.Append(" evonot_web_action_date = null")

                sQuery.Append(" WHERE evonot_id = " + helpCriteria.HelpCriteriaItemID.ToString)

            Else

                sQuery.Append("INSERT INTO Evolution_Notifications (evonot_active_flag, evonot_admin_flag, evonot_sub_id, evonot_comp_id, evonot_release_date, evonot_title, evonot_release_type,")
                sQuery.Append(" evonot_description, evonot_tabs, evonot_announcement, evonot_video, evonot_product_commercial_flag, evonot_product_business_flag, evonot_product_helicopter_flag,")
                sQuery.Append(" evonot_evo_dotnet_only_flag, evonot_evo_dotnet_flag, evonot_evo_asp_flag, evonot_product_crm_flag, evonot_web_action_date")

                ' If helpCriteria.HelpCriteriaModelID > -1 And helpCriteria.HelpCriteriaItemReleaseType.ToUpper.Contains("ML") Then
                sQuery.Append(", evonot_amod_id, evonot_doc_link")
                ' End If

                sQuery.Append(") VALUES (")
                sQuery.Append(IIf(helpCriteria.HelpCriteriaItemStatus, "'Y'", "'N'") + ",")
                sQuery.Append(IIf(helpCriteria.HelpCriteriaAdminOnly, "'Y'", "'N'") + ",")
                If Trim(helpCriteria.HelpCriteriaSubID.ToString) <> "" Then
                    sQuery.Append("" + helpCriteria.HelpCriteriaSubID.ToString + ",")
                Else
                    sQuery.Append("0,")
                End If

                If Trim(helpCriteria.HelpCriteriaCompanyID.ToString) <> "" Then
                    sQuery.Append("" + helpCriteria.HelpCriteriaCompanyID.ToString + ",")
                Else
                    sQuery.Append("0,")
                End If

                sQuery.Append("'" + helpCriteria.HelpCriteriaItemReleaseDate + "',")
                sQuery.Append(IIf(String.IsNullOrEmpty(helpCriteria.HelpCriteriaItemTitle.Trim), "null", "'" + helpCriteria.HelpCriteriaItemTitle.Trim + "'") + ",")
                sQuery.Append("'" + helpCriteria.HelpCriteriaItemReleaseType + "',")

                sQuery.Append(IIf(String.IsNullOrEmpty(helpCriteria.HelpCriteriaItemViewNumber.Trim), "null", "'" + helpCriteria.HelpCriteriaItemViewNumber.Trim + "'") + ",")
                sQuery.Append(IIf(String.IsNullOrEmpty(helpCriteria.HelpCriteriaItemTabName.Trim), "null", "'" + helpCriteria.HelpCriteriaItemTabName.Trim + "'") + ",")

                sQuery.Append(IIf(String.IsNullOrEmpty(helpCriteria.HelpCriteriaItemDiscription.Trim), "null", "'" + helpCriteria.HelpCriteriaItemDiscription.Trim + "'") + ",")
                sQuery.Append(IIf(String.IsNullOrEmpty(helpCriteria.HelpCriteriaItemVideoLink.Trim), "null", "'" + helpCriteria.HelpCriteriaItemVideoLink.Trim + "'") + ",")

                sQuery.Append(IIf(helpCriteria.HelpCriteriaBusinessFlag, "'Y'", "'N'") + ",")
                sQuery.Append(IIf(helpCriteria.HelpCriteriaHelicopterFlag, "'Y'", "'N'") + ",")
                sQuery.Append(IIf(helpCriteria.HelpCriteriaCommercialFlag, "'Y'", "'N'") + ",")

                'sQuery.Append(IIf(helpCriteria.HelpCriteriaYachtFlag, "'Y'", "'N'") + ",")

                sQuery.Append(IIf(helpCriteria.HelpCriteriaNewEvoOnlyFlag, "'Y'", "'N'") + ",")
                sQuery.Append(IIf(helpCriteria.HelpCriteriaNewEvoFlag, "'Y'", "'N'") + ",")
                sQuery.Append(IIf(helpCriteria.HelpCriteriaOldEvoFlag, "'Y'", "'N'") + ",")
                sQuery.Append(IIf(helpCriteria.HelpCriteriaCRMFlag, "'Y'", "'N'") + ",")
                sQuery.Append("null")

                '  If helpCriteria.HelpCriteriaModelID > -1 And helpCriteria.HelpCriteriaItemReleaseType.ToUpper.Contains("ML") Then
                sQuery.Append("," + helpCriteria.HelpCriteriaModelID.ToString + ",")
                sQuery.Append(IIf(String.IsNullOrEmpty(helpCriteria.HelpCriteriaItemDocumentLink.Trim), "null", "'" + helpCriteria.HelpCriteriaItemDocumentLink.Trim + "'"))
                '  End If

                sQuery.Append(")")

            End If

            ' first Insert new or update old record ...
            SqlCommand.CommandText = sQuery.ToString
            SqlCommand.ExecuteNonQuery()

            If bIsUpdateItem Then

                ' delete all "help topics" connected to this "help" item (only applicable to update)

                sQuery = New StringBuilder()
                sQuery.Append("DELETE FROM evolution_topic_index WHERE evotopind_evonot_id = " + helpCriteria.HelpCriteriaItemID.ToString)

                SqlCommand.CommandText = sQuery.ToString
                SqlCommand.ExecuteNonQuery()

            Else

                'get the inserted "ID" of this new item so we can use it to insert the "help topics" associated to this help item

                sQuery = New StringBuilder()
                sQuery.Append("SELECT MAX(evonot_id) AS lastHelpID FROM Evolution_Notifications")

                SqlCommand.CommandText = sQuery.ToString
                SqlReader = SqlCommand.ExecuteReader()

                If SqlReader.HasRows Then

                    SqlReader.Read()

                    If Not (IsDBNull(SqlReader("lastHelpID"))) Then
                        helpCriteria.HelpCriteriaItemID = CInt(SqlReader.Item("lastHelpID").ToString)
                    End If

                End If

                SqlReader.Close()
                SqlReader.Dispose()

                ' only insert the document file if we have a filename and its not a "model help item"
                If Not String.IsNullOrEmpty(oUploadFile.FileName.ToString.Trim) And Not helpCriteria.HelpCriteriaItemReleaseType.ToUpper.Contains("ML") Then

                    ' ok here is where the fun begins
                    Dim pos As Integer = oUploadFile.FileName.IndexOf(".")
                    Dim extension As String = oUploadFile.FileName.Substring(pos, (oUploadFile.FileName.Length - pos))
                    Dim sDestinationPath = HttpContext.Current.Server.MapPath(HttpContext.Current.Session.Item("HelpDocumentFolderVirtualPath").ToString.Trim + Constants.cSingleForwardSlash + helpCriteria.HelpCriteriaItemID.ToString + extension)

                    ' save the file
                    oUploadFile.SaveAs(sDestinationPath)

                    helpCriteria.HelpCriteriaItemDocumentLink = HttpContext.Current.Session.Item("jetnetFullHostName").ToString.Trim + "/help/documents" + Constants.cSingleForwardSlash + helpCriteria.HelpCriteriaItemID.ToString + extension

                    sQuery = New StringBuilder()

                    ' set the doc link to match the "generated" file name
                    sQuery.Append("UPDATE Evolution_Notifications SET")
                    sQuery.Append(" evonot_doc_link = '" + helpCriteria.HelpCriteriaItemDocumentLink + "',")
                    sQuery.Append(" evonot_web_action_date = null")
                    sQuery.Append(" WHERE evonot_id = " + helpCriteria.HelpCriteriaItemID.ToString)

                    SqlCommand.CommandText = sQuery.ToString
                    SqlCommand.ExecuteNonQuery()

                End If

            End If

            ' insert the "help topics" into the topic index table
            If Not String.IsNullOrEmpty(helpCriteria.HelpCriteriaItemTopicList.Trim) Then

                sQuery = New StringBuilder()

                sQuery.Append("INSERT INTO evolution_topic_index (evotopind_evonot_id, evotopind_evotop_id) VALUES ")

                Dim sHelpTopicArray = Split(helpCriteria.HelpCriteriaItemTopicList, Constants.cCommaDelim)
                Dim x As Integer = 0

                For Each ts As String In sHelpTopicArray

                    If x < sHelpTopicArray.Length - 1 Then
                        sQuery.Append("(" + helpCriteria.HelpCriteriaItemID.ToString + ", " + ts.Trim + "),")
                    Else
                        sQuery.Append("(" + helpCriteria.HelpCriteriaItemID.ToString + ", " + ts.Trim + ");")
                    End If

                    x += 1

                Next

                SqlCommand.CommandText = sQuery.ToString
                SqlCommand.ExecuteNonQuery()

            End If

            bReturnStatus = True

        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in insertOrUpdateHelpItem(ByVal bIsUpdateItem As Boolean, ByVal helpCriteria As helpAdminSelectionCriteriaClass)</b><br />" + ex.Message

        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing

            SqlCommand.Dispose()
            SqlCommand = Nothing
        End Try

        Return bReturnStatus

    End Function

    Public Function deleteHelpItem(ByVal nItemId As Long) As Boolean

        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing '

        Dim bReturnStatus As Boolean = False

        Try

            SqlConn.ConnectionString = adminConnectStr

            SqlConn.Open()
            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 240

            'sQuery.Append("UPDATE Evolution_Notifications SET")
            'sQuery.Append(" evonot_active_flag = 'N',")
            'sQuery.Append(" evonot_web_action_date = null")
            'sQuery.Append(" WHERE evonot_id = " + nItemId.ToString)

            'SqlCommand.CommandText = sQuery.ToString
            'SqlCommand.ExecuteNonQuery()

            ' delete all "help topics" connected to this "help" item (only applicable to update)
            sQuery.Append("DELETE FROM evolution_topic_index WHERE evotopind_evonot_id = " + nItemId.ToString)

            SqlCommand.CommandText = sQuery.ToString
            SqlCommand.ExecuteNonQuery()

            sQuery = New StringBuilder()
            ' delete "help item"  
            sQuery.Append("DELETE FROM Evolution_Notifications WHERE evonot_id = " + nItemId.ToString)

            SqlCommand.CommandText = sQuery.ToString
            SqlCommand.ExecuteNonQuery()

            ' now delete any file "associated" with this help item
            Dim sFileNames As String = HttpContext.Current.Server.MapPath(HttpContext.Current.Session.Item("HelpDocumentFolderVirtualPath"))
            Dim fileList As String() = System.IO.Directory.GetFiles(sFileNames, nItemId.ToString + "*.*")

            For Each f As String In fileList
                System.IO.File.Delete(f)
            Next

            bReturnStatus = True

        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in deleteHelpItem(ByVal nItemId As Long) as boolean</b><br />" + ex.Message

        Finally

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing

            SqlCommand.Dispose()
            SqlCommand = Nothing
        End Try

        Return bReturnStatus

    End Function

#End Region

#Region "admin_report_page_functions"

    Public Function getAdminReportListDataTable(ByVal nReportID As Integer, ByVal nSubID As Long, Optional ByVal sRptType As String = "", Optional ByVal bIsAdminReport As Boolean = False, Optional ByVal is_aerodex_limited As Boolean = False, Optional ByVal type_of As String = "") As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            sQuery.Append("SELECT * FROM [Homebase].jetnet_ra.dbo.SQL_Report WITH(NOLOCK)")

            If bIsAdminReport Then

                If nReportID > 0 Then
                    sQuery.Append(" WHERE sqlrep_level = 'JETNET' AND sqlrep_sub_id = 0 AND sqlrep_id = " + nReportID.ToString)
                Else
                    sQuery.Append(" WHERE sqlrep_level = 'JETNET' AND sqlrep_sub_id = 0")
                End If

            Else

                If nReportID > 0 Then
                    ' added in second line with or, for if its a 0 all 
                    sQuery.Append(" WHERE (sqlrep_sub_id = " + IIf(nSubID > 0, nSubID.ToString, "0") + " AND sqlrep_level IN ('" + sRptType.Replace(Constants.cCommaDelim, Constants.cValueSeperator).Trim + "') AND sqlrep_id = " + nReportID.ToString + ")")
                    sQuery.Append(" OR (sqlrep_sub_id = 0 AND sqlrep_level IN ('" + sRptType.Replace(Constants.cCommaDelim, Constants.cValueSeperator).Trim + "') AND sqlrep_id = " + nReportID.ToString + ")")
                Else
                    sQuery.Append(" WHERE (sqlrep_sub_id = " + IIf(nSubID > 0, nSubID.ToString, "0") + " AND sqlrep_level IN ('" + sRptType.Replace(Constants.cCommaDelim, Constants.cValueSeperator).Trim + "'))")
                End If

            End If

            'MSW 11/12/15 - true would mean the person is aerodex, so they can only see ones where aerodex is Y
            If is_aerodex_limited Then
                sQuery.Append(" AND sqlrep_aerodex_flag = 'Y'")
            End If

            If Not String.IsNullOrEmpty(type_of.Trim) Then
                sQuery.Append(" AND sqlrep_type = '" + type_of.Trim + "'")
            End If

            sQuery.Append(" ORDER BY sqlrep_type, sqlrep_title")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + sQuery.ToString

            SqlConn.ConnectionString = adminConnectStr

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
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

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

    Public Sub displayAdminReportList(ByRef out_htmlString As String, Optional ByVal nSubID As Long = 0, Optional ByVal sRptType As String = "", Optional ByVal bIsAdminReport As Boolean = False, Optional ByVal type_of As String = "")

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False

        Dim lastRptType As String = ""

        Try

            results_table = getAdminReportListDataTable(0, nSubID, sRptType, bIsAdminReport, False, type_of)

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    htmlOut.Append("<div class=""Box""><table id=""adminReportListTable"" width=""100%"" cellpadding=""4"" cellspacing=""0"" class=""formatTable blue"">")

                    For Each r As DataRow In results_table.Rows

                        If Not lastRptType.ToLower.Trim.Contains(r.Item("sqlrep_type").ToString.ToLower.Trim) Then
                            lastRptType = r.Item("sqlrep_type").ToString.Trim
                            htmlOut.Append("<tr class=""noBorder""><td align=""left"" valign=""middle"" height=""24""><div class=""subHeader"">" + lastRptType + "</div></td></tr>")
                        End If


                        htmlOut.Append("<tr class=""noBorder"">")


                        htmlOut.Append("<td align=""left"" valign=""top""><p><a class=""underline pointer"" href=""adminSummary.aspx?rid=" + r.Item("sqlrep_id").ToString.Trim + """><strong>" + UCase(HttpContext.Current.Server.HtmlEncode(r.Item("sqlrep_title").ToString.Trim)) + "</strong></a>: ")

                        If Not String.IsNullOrEmpty(r.Item("sqlrep_description").ToString.Trim) Then
                            htmlOut.Append(r.Item("sqlrep_description").ToString.Replace(vbCrLf, "").Replace("<br>", "").Trim)
                        End If

                        htmlOut.Append("</p></td></tr>")

                    Next

                    htmlOut.Append("</table></div>")

                Else
                    htmlOut.Append("<div class=""Box""><div class=""subHeader"">No Reports Found</div></div>")
                End If
            Else
                htmlOut.Append("<div class=""Box""><div class=""subHeader"">No Reports Found</div></div>")
            End If

        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub

    Public Sub generateAdminReport(ByVal nReportID As Integer, ByRef out_ReportString As String, Optional ByVal nSubID As Long = 0, Optional ByVal sRptType As String = "",
                                   Optional ByVal bIsAdminReport As Boolean = False, Optional ByVal is_aerodex_limited As Boolean = False)

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder

        Dim sRptQuery As String = ""

        Dim sTmpQuery As String = ""
        Dim sProductString As String = ""

        Dim sRptTitle As String = ""
        Dim sRptReportType As String = ""

        Dim sRptConnection As String = ""

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader = Nothing
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            results_table = getAdminReportListDataTable(nReportID, nSubID, sRptType, bIsAdminReport, is_aerodex_limited)

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    sRptQuery = results_table.Rows(0).Item("sqlrep_query").ToString.Trim
                    sRptTitle = results_table.Rows(0).Item("sqlrep_title").ToString.Trim

                    If Not IsDBNull(results_table.Rows(0).Item("sqlrep_type")) Then
                        sRptReportType = results_table.Rows(0).Item("sqlrep_type").ToString.Trim
                    End If

                    If Not IsDBNull(results_table.Rows(0).Item("sqlrep_connetion_string")) Then
                        If Not String.IsNullOrEmpty(results_table.Rows(0).Item("sqlrep_connetion_string").ToString.Trim) Then
                            sRptConnection = results_table.Rows(0).Item("sqlrep_connetion_string").ToString.Trim
                        End If
                    End If

                End If

            End If

            If sRptReportType.ToLower.Contains("aircraft") Or sRptReportType.ToLower.Contains("company") Then

                sTmpQuery = sRptQuery.Trim

                If sRptReportType.ToLower.Contains("aircraft") Then
                    sProductString = commonEvo.BuildProductCodeCheckWhereClause(HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag, HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag, HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag, False, HttpContext.Current.Session.Item("localSubscription").crmYacht_Flag, False, True)
                ElseIf sRptReportType.ToLower.Contains("company") Then
                    sProductString = commonEvo.BuildCompanyProductCodeCheckWhereClause(HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag, HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag, HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag, False, HttpContext.Current.Session.Item("localSubscription").crmYacht_Flag, False)
                End If

                sRptQuery = sTmpQuery.Replace("/* INSERT SUBSCRIPTION */", sProductString)

            End If

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + sRptQuery.ToString

            If String.IsNullOrEmpty(sRptConnection.Trim) Then
                SqlConn.ConnectionString = adminConnectStr
            Else
                SqlConn.ConnectionString = sRptConnection
            End If

            SqlConn.Open()
            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 240

            SqlCommand.CommandText = sRptQuery.Trim

            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                results_table = New DataTable
                results_table.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = results_table.GetErrors()
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + constrExc.Message
            End Try

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    htmlOut.Append(include_excel_admin_report_style())
                    htmlOut.Append("<table border=""1"" cellpadding=""2"" cellspacing=""0"">")

                    ' first add the report title
                    htmlOut.Append("<tr><td align=""left"" valign=""middle"" colspan=""" + results_table.Columns.Count.ToString + """><b>" + sRptTitle.Trim + "</b></td></tr>")

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

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message

        Finally

        End Try

        'return resulting html string
        out_ReportString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub

    Public Function include_excel_admin_report_style() As String

        Dim htmlOut = New StringBuilder()

        htmlOut.Append("<style type='text/css'>")
        htmlOut.Append("  td.textformat {mso-number-format:'\@'}")
        htmlOut.Append("  td.textdate {mso-number-format:'Short Date'}")
        htmlOut.Append("</style>")

        Return htmlOut.ToString
        htmlOut = Nothing

    End Function

#End Region

#Region "admin_background_page_functions"

    Public Function getBackgroundList(ByVal sBackgroundProduct As String, ByVal bActive As Boolean) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            sQuery.Append("SELECT *, ")
            sQuery.Append(" (SELECT COUNT(*) FROM Evolution_Backgrounds WHERE evoback_aerodex_flag = 'Y'" + IIf(bActive, " AND evoback_active_flag = 'Y'", "") + ") AS AERODEX,")
            sQuery.Append(" (SELECT COUNT(*) FROM Evolution_Backgrounds WHERE evoback_product_business_flag = 'Y'" + IIf(bActive, " AND evoback_active_flag = 'Y'", "") + ") AS BUSINESS,")
            sQuery.Append(" (SELECT COUNT(*) FROM Evolution_Backgrounds WHERE evoback_product_helicopter_flag = 'Y'" + IIf(bActive, " AND evoback_active_flag = 'Y'", "") + ") AS HELICOPTER,")
            sQuery.Append(" (SELECT COUNT(*) FROM Evolution_Backgrounds WHERE evoback_product_commercial_flag = 'Y'" + IIf(bActive, " AND evoback_active_flag = 'Y'", "") + ") AS COMMERCIAL,")
            sQuery.Append(" (SELECT COUNT(*) FROM Evolution_Backgrounds WHERE evoback_product_yacht_flag = 'Y'" + IIf(bActive, " AND evoback_active_flag = 'Y'", "") + ") AS YACHT,")
            sQuery.Append(" (SELECT COUNT(*) FROM Evolution_Backgrounds WHERE evoback_feature_flag = 'Y'" + IIf(bActive, " AND evoback_active_flag = 'Y'", "") + ") AS FEATURE,")

            If bActive Then
                sQuery.Append(" (SELECT COUNT(*) FROM Evolution_Backgrounds WHERE evoback_active_flag = 'Y') AS TOTALBACKGROUNDS")
            Else
                sQuery.Append(" (SELECT COUNT(*) FROM Evolution_Backgrounds) AS TOTALBACKGROUNDS")
            End If

            sQuery.Append(" FROM Evolution_Backgrounds WITH(NOLOCK)")

            If bActive Then
                sQuery.Append(" WHERE evoback_active_flag = 'Y'")
            End If

            Select Case sBackgroundProduct.ToUpper.Trim
                Case "A"
                    sQuery.Append(IIf(bActive, Constants.cAndClause, Constants.cWhereClause) + "evoback_aerodex_flag = 'Y'")
                Case "B"
                    sQuery.Append(IIf(bActive, Constants.cAndClause, Constants.cWhereClause) + "evoback_product_business_flag = 'Y'")
                Case "H"
                    sQuery.Append(IIf(bActive, Constants.cAndClause, Constants.cWhereClause) + "evoback_product_helicopter_flag = 'Y'")
                Case "C"
                    sQuery.Append(IIf(bActive, Constants.cAndClause, Constants.cWhereClause) + "evoback_product_commercial_flag = 'Y'")
                Case "Y"
                    sQuery.Append(IIf(bActive, Constants.cAndClause, Constants.cWhereClause) + "evoback_product_yacht_flag = 'Y'")
                Case "F"
                    sQuery.Append(IIf(bActive, Constants.cAndClause, Constants.cWhereClause) + "evoback_feature_flag = 'Y'")
            End Select

            sQuery.Append(" GROUP BY evoback_title, evoback_id, evoback_active_flag, evoback_aerodex_flag, evoback_product_business_flag, evoback_product_helicopter_flag,")
            sQuery.Append("evoback_product_commercial_flag, evoback_product_yacht_flag, evoback_feature_flag")

            sQuery.Append(" ORDER BY evoback_title ASC")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>getBackgroundList(ByVal sBackgroundProduct As String, ByVal bActive As Boolean) As DataTable</b><br />" + sQuery.ToString

            SqlConn.ConnectionString = adminConnectStr

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
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getBackgroundList load datatable</b><br /> " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getBackgroundList(ByVal sBackgroundProduct As String, ByVal bActive As Boolean) As DataTable</b><br />" + ex.Message

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

    Public Sub displayBackgroundList(ByVal sBackgroundProduct As String, ByVal bActive As Boolean, ByRef out_htmlString As String)

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder
        ' Dim toggleRowColor As Boolean = False

        Dim nTotalAerodex As Integer = 0
        Dim nTotalBusiness As Integer = 0
        Dim nTotalCommercial As Integer = 0
        Dim nTotalHelicopters As Integer = 0
        Dim nTotalFeatured As Integer = 0
        Dim nTotalYachts As Integer = 0
        Dim nTotalTotal As Integer = 0

        Try

            results_table = getBackgroundList(sBackgroundProduct, bActive)

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then


                    htmlOut.Append("<div class=""subHeader"">JETNET Background List" + IIf(Not String.IsNullOrEmpty(sBackgroundProduct.Trim), " by Product", "") + IIf(bActive, " (<em>Active Backgrounds</em>)", " (<em>All Backgrounds</em>)") + "</div>")
                    htmlOut.Append("<table id=""backgroundDataTable"" width=""100%"" cellpadding=""2"" cellspacing=""2"" border=""0"" class=""formatTable blue"">")
                    htmlOut.Append("<thead><tr><td align=""left"" width=""40%""><b>Image / Title</b></td>")
                    htmlOut.Append("<td align=""center""><b>Active</b></td>")
                    htmlOut.Append("<td align=""center""><b>Aerodex</b></td>")
                    htmlOut.Append("<td align=""center""><b>Business</b></td>")
                    htmlOut.Append("<td align=""center""><b>Commercial</b></td>")
                    htmlOut.Append("<td align=""center""><b>Helicopters</b></td>")
                    htmlOut.Append("<td align=""center""><b>Featured</b></td>")
                    htmlOut.Append("<td align=""center""><b>Yachts</b></td></tr></thead><tbody>")

                    For Each r As DataRow In results_table.Rows
                        'If Not toggleRowColor Then
                        '    htmlOut.Append("<tr class=""alt_row"">")
                        '    toggleRowColor = True
                        'Else
                        htmlOut.Append("<tr>")
                        '    toggleRowColor = False
                        'End If

                        htmlOut.Append("<td align=""left"" valign=""middle"" width=""50%""><a class=""underline pointer"" href=""adminbackground.aspx?backID=" + r.Item("evoback_id").ToString + "&task=details"" title=""Show Background Details"">" + HttpContext.Current.Server.HtmlEncode(r.Item("evoback_title").ToString.Trim) + "</a><br/><img width=""190"" height=""120"" alt=""../images/background/" + r.Item("evoback_id").ToString + ".jpg"" title=""../images/background/" + r.Item("evoback_id").ToString + ".jpg"" src=""../images/background/" + r.Item("evoback_id").ToString + ".jpg"" border=""1""/></td>")

                        If Not IsDBNull(r.Item("evoback_active_flag")) Then
                            If Not String.IsNullOrEmpty(r.Item("evoback_active_flag").ToString.Trim) Then

                                If r.Item("evoback_active_flag").ToString.Trim.Contains("Y") Then
                                    htmlOut.Append("<td align=""center"" valign=""middle""><a class=""underline pointer"" href=""adminbackground.aspx?backID=" + r.Item("evoback_id").ToString + "&active=false&task=update"" title=""Disable Background""><img width=""20"" height=""20"" alt=""Yes"" src=""images/evo_green_check.png""/></a></td>")
                                Else
                                    htmlOut.Append("<td align=""center"" valign=""middle""><a class=""underline pointer"" href=""adminbackground.aspx?backID=" + r.Item("evoback_id").ToString + "&active=true&task=update"" title=""Enable Background""><img width=""20"" height=""20"" alt=""No"" src=""images/evo_red_check.png""/></a></td>")
                                End If

                            Else
                                htmlOut.Append("<td align=""center"" valign=""middle""><img width=""20"" height=""20"" alt=""No"" src=""images/evo_red_check.png""></td>")
                            End If
                        Else
                            htmlOut.Append("<td align=""center"" valign=""middle""><img width=""20"" height=""20"" alt=""No"" src=""images/evo_red_check.png""></td>")
                        End If

                        If Not IsDBNull(r.Item("evoback_aerodex_flag")) Then
                            If Not String.IsNullOrEmpty(r.Item("evoback_aerodex_flag").ToString.Trim) Then

                                If r.Item("evoback_aerodex_flag").ToString.Trim.Contains("Y") Then
                                    htmlOut.Append("<td align=""center"" valign=""middle""><a class=""underline pointer"" href=""adminbackground.aspx?backID=" + r.Item("evoback_id").ToString + "&product=A&prodvalue=false&task=update"" title=""Disable for Aerodex""><img width=""20"" height=""20"" alt=""Yes"" src=""images/evo_green_check.png""/></a></td>")
                                Else
                                    htmlOut.Append("<td align=""center"" valign=""middle""><a class=""underline pointer"" href=""adminbackground.aspx?backID=" + r.Item("evoback_id").ToString + "&product=A&prodvalue=true&task=update"" title=""Enable for Aerodex""><img width=""20"" height=""20"" alt=""No"" src=""images/evo_red_check.png""/></a></td>")
                                End If

                            Else
                                htmlOut.Append("<td align=""center"" valign=""middle""><img width=""20"" height=""20"" alt=""No"" src=""images/evo_red_check.png""></td>")
                            End If
                        Else
                            htmlOut.Append("<td align=""center"" valign=""middle""><img width=""20"" height=""20"" alt=""No"" src=""images/evo_red_check.png""></td>")
                        End If

                        If Not IsDBNull(r.Item("evoback_product_business_flag")) Then
                            If Not String.IsNullOrEmpty(r.Item("evoback_product_business_flag").ToString.Trim) Then

                                If r.Item("evoback_product_business_flag").ToString.Trim.Contains("Y") Then
                                    htmlOut.Append("<td align=""center"" valign=""middle""><a class=""underline pointer"" href=""adminbackground.aspx?backID=" + r.Item("evoback_id").ToString + "&product=B&prodvalue=false&task=update"" title=""Disable for Business""><img width=""20"" height=""20"" alt=""Yes"" src=""images/evo_green_check.png""/></a></td>")
                                Else
                                    htmlOut.Append("<td align=""center"" valign=""middle""><a class=""underline pointer"" href=""adminbackground.aspx?backID=" + r.Item("evoback_id").ToString + "&product=B&prodvalue=true&task=update"" title=""Enable for Business""><img width=""20"" height=""20"" alt=""No"" src=""images/evo_red_check.png""/></a></td>")
                                End If

                            Else
                                htmlOut.Append("<td align=""center"" valign=""middle""><img width=""20"" height=""20"" alt=""No"" src=""images/evo_red_check.png""></td>")
                            End If
                        Else
                            htmlOut.Append("<td align=""center"" valign=""middle""><img width=""20"" height=""20"" alt=""No"" src=""images/evo_red_check.png""></td>")
                        End If

                        If Not IsDBNull(r.Item("evoback_product_commercial_flag")) Then
                            If Not String.IsNullOrEmpty(r.Item("evoback_product_commercial_flag").ToString.Trim) Then

                                If r.Item("evoback_product_commercial_flag").ToString.Trim.Contains("Y") Then
                                    htmlOut.Append("<td align=""center"" valign=""middle""><a class=""underline pointer"" href=""adminbackground.aspx?backID=" + r.Item("evoback_id").ToString + "&product=C&prodvalue=false&task=update"" title=""Disable for Commercial""><img width=""20"" height=""20"" alt=""Yes"" src=""images/evo_green_check.png""/></a></td>")
                                Else
                                    htmlOut.Append("<td align=""center"" valign=""middle""><a class=""underline pointer"" href=""adminbackground.aspx?backID=" + r.Item("evoback_id").ToString + "&product=C&prodvalue=true&task=update"" title=""Enable for Commercial""><img width=""20"" height=""20"" alt=""No"" src=""images/evo_red_check.png""/></a></td>")
                                End If

                            Else
                                htmlOut.Append("<td align=""center"" valign=""middle""><img width=""20"" height=""20"" alt=""No"" src=""images/evo_red_check.png""></td>")
                            End If
                        Else
                            htmlOut.Append("<td align=""center"" valign=""middle""><img width=""20"" height=""20"" alt=""No"" src=""images/evo_red_check.png""></td>")
                        End If

                        If Not IsDBNull(r.Item("evoback_product_helicopter_flag")) Then
                            If Not String.IsNullOrEmpty(r.Item("evoback_product_helicopter_flag").ToString.Trim) Then

                                If r.Item("evoback_product_helicopter_flag").ToString.Trim.Contains("Y") Then
                                    htmlOut.Append("<td align=""center"" valign=""middle""><a class=""underline pointer"" href=""adminbackground.aspx?backID=" + r.Item("evoback_id").ToString + "&product=H&prodvalue=false&task=update"" title=""Disable for Helicopters""><img width=""20"" height=""20"" alt=""Yes"" src=""images/evo_green_check.png""/></a></td>")
                                Else
                                    htmlOut.Append("<td align=""center"" valign=""middle""><a class=""underline pointer"" href=""adminbackground.aspx?backID=" + r.Item("evoback_id").ToString + "&product=H&prodvalue=true&task=update"" title=""Enable for Helicopters""><img width=""20"" height=""20"" alt=""No"" src=""images/evo_red_check.png""/></a></td>")
                                End If

                            Else
                                htmlOut.Append("<td align=""center"" valign=""middle""><img width=""20"" height=""20"" alt=""No"" src=""images/evo_red_check.png""></td>")
                            End If
                        Else
                            htmlOut.Append("<td align=""center"" valign=""middle""><img width=""20"" height=""20"" alt=""No"" src=""images/evo_red_check.png""></td>")
                        End If

                        If Not IsDBNull(r.Item("evoback_feature_flag")) Then
                            If Not String.IsNullOrEmpty(r.Item("evoback_feature_flag").ToString.Trim) Then

                                If r.Item("evoback_feature_flag").ToString.Trim.Contains("Y") Then
                                    htmlOut.Append("<td align=""center"" valign=""middle""><a class=""underline pointer"" href=""adminbackground.aspx?backID=" + r.Item("evoback_id").ToString + "&product=F&prodvalue=false&task=update"" title=""Disable for Featured""><img width=""20"" height=""20"" alt=""Yes"" src=""images/evo_green_check.png""/></a></td>")
                                Else
                                    htmlOut.Append("<td align=""center"" valign=""middle""><a class=""underline pointer"" href=""adminbackground.aspx?backID=" + r.Item("evoback_id").ToString + "&product=F&prodvalue=true&task=update"" title=""Enable for Featured""><img width=""20"" height=""20"" alt=""No"" src=""images/evo_red_check.png""/></a></td>")
                                End If

                            Else
                                htmlOut.Append("<td align=""center"" valign=""middle""><img width=""20"" height=""20"" alt=""No"" src=""images/evo_red_check.png""></td>")
                            End If
                        Else
                            htmlOut.Append("<td align=""center"" valign=""middle""><img width=""20"" height=""20"" alt=""No"" src=""images/evo_red_check.png""></td>")
                        End If

                        If Not IsDBNull(r.Item("evoback_product_yacht_flag")) Then
                            If Not String.IsNullOrEmpty(r.Item("evoback_product_yacht_flag").ToString.Trim) Then

                                If r.Item("evoback_product_yacht_flag").ToString.Trim.Contains("Y") Then
                                    htmlOut.Append("<td align=""center"" valign=""middle""><a class=""underline pointer"" href=""adminbackground.aspx?backID=" + r.Item("evoback_id").ToString + "&product=Y&prodvalue=false&task=update"" title=""Disable for Yacht""><img width=""20"" height=""20"" alt=""Yes"" src=""images/evo_green_check.png""/></a></td>")
                                Else
                                    htmlOut.Append("<td align=""center"" valign=""middle""><a class=""underline pointer"" href=""adminbackground.aspx?backID=" + r.Item("evoback_id").ToString + "&product=Y&prodvalue=true&task=update"" title=""Enable for Yacht""><img width=""20"" height=""20"" alt=""No"" src=""images/evo_red_check.png""/></a></td>")
                                End If

                            Else
                                htmlOut.Append("<td align=""center"" valign=""middle""><img width=""20"" height=""20"" alt=""No"" src=""images/evo_red_check.png""></td>")
                            End If
                        Else
                            htmlOut.Append("<td align=""center"" valign=""middle""><img width=""20"" height=""20"" alt=""No"" src=""images/evo_red_check.png""></td>")
                        End If

                        htmlOut.Append("</tr>")

                    Next

                    nTotalAerodex = CInt(results_table.Rows(0).Item("AERODEX").ToString.Trim)
                    nTotalBusiness = CInt(results_table.Rows(0).Item("BUSINESS").ToString.Trim)
                    nTotalCommercial = CInt(results_table.Rows(0).Item("COMMERCIAL").ToString.Trim)
                    nTotalHelicopters = CInt(results_table.Rows(0).Item("HELICOPTER").ToString.Trim)
                    nTotalFeatured = CInt(results_table.Rows(0).Item("FEATURE").ToString.Trim)
                    nTotalYachts = CInt(results_table.Rows(0).Item("YACHT").ToString.Trim)
                    nTotalTotal = CInt(results_table.Rows(0).Item("TOTALBACKGROUNDS").ToString.Trim)

                    'If Not toggleRowColor Then
                    '    htmlOut.Append("<tr class=""alt_row"">")
                    '    toggleRowColor = True
                    'Else
                    htmlOut.Append("<tr>")
                    '    toggleRowColor = False
                    'End If

                    htmlOut.Append("<td align=""left"">Totals</td>")
                    htmlOut.Append("<td align=""center"">" + IIf(nTotalTotal > 0, "<a class=""underline pointer"" href=""adminBackground.aspx?" + IIf(bActive, "active=true&", "active=&") + "product="">" + nTotalTotal.ToString.Trim + "</a>", "0") + "</td>")
                    htmlOut.Append("<td align=""center"">" + IIf(nTotalAerodex > 0, "<a class=""underline pointer"" href=""adminBackground.aspx?" + IIf(bActive, "active=true&", "active=&") + "product=A"">" + nTotalAerodex.ToString.Trim + "</a>", "0") + "</td>")
                    htmlOut.Append("<td align=""center"">" + IIf(nTotalBusiness > 0, "<a class=""underline pointer"" href=""adminBackground.aspx?" + IIf(bActive, "active=true&", "active=&") + "product=B"">" + nTotalBusiness.ToString.Trim + "</a>", "0") + "</td>")
                    htmlOut.Append("<td align=""center"">" + IIf(nTotalCommercial > 0, "<a class=""underline pointer"" href=""adminBackground.aspx?" + IIf(bActive, "active=true&", "active=&") + "product=C"">" + nTotalCommercial.ToString.Trim + "</a>", "0") + "</td>")
                    htmlOut.Append("<td align=""center"">" + IIf(nTotalHelicopters > 0, "<a class=""underline pointer"" href=""adminBackground.aspx?" + IIf(bActive, "active=true&", "active=&") + "product=H"">" + nTotalHelicopters.ToString.Trim + "</a>", "0") + "</td>")
                    htmlOut.Append("<td align=""center"">" + IIf(nTotalFeatured > 0, "<a class=""underline pointer"" href=""adminBackground.aspx?" + IIf(bActive, "active=true&", "active=&") + "product=F"">" + nTotalFeatured.ToString.Trim + "</a>", "0") + "</td>")
                    htmlOut.Append("<td align=""center"">" + IIf(nTotalYachts > 0, "<a class=""underline pointer"" href=""adminBackground.aspx?" + IIf(bActive, "active=true&", "active=&") + "product=Y"">" + nTotalYachts.ToString.Trim + "</a>", "0") + "</td>")


                    htmlOut.Append("</tr></tbody>")

                    htmlOut.Append("</table>")

                Else
                    htmlOut.Append("<table id=""backgroundDataTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"" class=""formatTable blue""><tr><td valign=""top"" align=""left""><br/>No Backgrounds Found</td></tr></table>")
                End If
            Else
                htmlOut.Append("<table id=""backgroundDataTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"" class=""formatTable blue""><tr><td valign=""top"" align=""left""><br/>No Backgrounds Found</td></tr></table>")
            End If

        Catch ex As Exception

            aError = "Error in displayBackgroundList(ByVal sBackgroundProduct As String, ByVal bActive As Boolean, ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub

    Public Function insertOrUpdateBackground(ByVal nBackID As Integer, ByVal sSetActive As String, ByVal sSetProduct As String, ByVal sSetProductValue As Boolean, Optional ByRef oBackground As adminBackgroundCriteriaClass = Nothing) As Boolean

        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader = Nothing
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing '

        Dim bReturnStatus As Boolean = False

        Try

            SqlConn.ConnectionString = adminConnectStr

            SqlConn.Open()
            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 240

            If IsNothing(oBackground) Then

                sQuery.Append("UPDATE Evolution_Backgrounds Set")

                If Not String.IsNullOrEmpty(sSetActive.Trim) Then
                    sQuery.Append(" evoback_active_flag = " + IIf(sSetActive.ToLower.Contains("True"), "'Y'", "'N'"))
                End If

                If Not String.IsNullOrEmpty(sSetProduct.Trim) Then

                    Select Case sSetProduct.ToUpper.Trim
                        Case "A"
                            sQuery.Append(" evoback_aerodex_flag = " + IIf(sSetProductValue, "'Y'", "'N'"))
                        Case "B"
                            sQuery.Append(" evoback_product_business_flag = " + IIf(sSetProductValue, "'Y'", "'N'"))
                        Case "H"
                            sQuery.Append(" evoback_product_helicopter_flag = " + IIf(sSetProductValue, "'Y'", "'N'"))
                        Case "C"
                            sQuery.Append(" evoback_product_commercial_flag = " + IIf(sSetProductValue, "'Y'", "'N'"))
                        Case "Y"
                            sQuery.Append(" evoback_product_yacht_flag = " + IIf(sSetProductValue, "'Y'", "'N'"))
                        Case "F"
                            sQuery.Append(" evoback_feature_flag = " + IIf(sSetProductValue, "'Y'", "'N'"))

                    End Select
                End If

                sQuery.Append(" WHERE evoback_id = " + nBackID.ToString) ' evoback_title

            Else

                If oBackground.BkndCriteriaItemNew Then

                    sQuery.Append("INSERT INTO Evolution_Backgrounds (evoback_id, evoback_title, evoback_active_flag, evoback_aerodex_flag, evoback_product_business_flag, evoback_product_helicopter_flag,")
                    sQuery.Append(" evoback_product_commercial_flag, evoback_product_yacht_flag, evoback_feature_flag")
                    sQuery.Append(") VALUES (" + oBackground.BkndCriteriaItemID.ToString + ",")

                    If Not String.IsNullOrEmpty(oBackground.BkndCriteriaItemTitle.Trim) Then
                        sQuery.Append("'" + oBackground.BkndCriteriaItemTitle.Trim + "',")
                    Else
                        sQuery.Append("'',")
                    End If

                    sQuery.Append(IIf(oBackground.BkndCriteriaItemStatus, "'Y'", "'N'") + ",")
                    sQuery.Append(IIf(oBackground.BkndCriteriaAerodexFlag, "'Y'", "'N'") + ",")
                    sQuery.Append(IIf(oBackground.BkndCriteriaBusinessFlag, "'Y'", "'N'") + ",")
                    sQuery.Append(IIf(oBackground.BkndCriteriaHelicopterFlag, "'Y'", "'N'") + ",")
                    sQuery.Append(IIf(oBackground.BkndCriteriaCommercialFlag, "'Y'", "'N'") + ",")
                    sQuery.Append(IIf(oBackground.BkndCriteriaYachtFlag, "'Y'", "'N'") + ",")
                    sQuery.Append(IIf(oBackground.BkndCriteriaFeatureFlag, "'Y'", "'N'"))
                    sQuery.Append(")")

                Else
                    sQuery.Append("UPDATE Evolution_Backgrounds SET")

                    sQuery.Append(" evoback_title = " + IIf(Not String.IsNullOrEmpty(oBackground.BkndCriteriaItemTitle.Trim), "'" + oBackground.BkndCriteriaItemTitle.Trim + "'", "''") + ",")

                    sQuery.Append(" evoback_active_flag = " + IIf(oBackground.BkndCriteriaItemStatus, "'Y'", "'N'") + ",")
                    sQuery.Append(" evoback_aerodex_flag = " + IIf(oBackground.BkndCriteriaAerodexFlag, "'Y'", "'N'") + ",")
                    sQuery.Append(" evoback_product_business_flag = " + IIf(oBackground.BkndCriteriaBusinessFlag, "'Y'", "'N'") + ",")
                    sQuery.Append(" evoback_product_helicopter_flag = " + IIf(oBackground.BkndCriteriaHelicopterFlag, "'Y'", "'N'") + ",")
                    sQuery.Append(" evoback_product_commercial_flag = " + IIf(oBackground.BkndCriteriaCommercialFlag, "'Y'", "'N'") + ",")
                    sQuery.Append(" evoback_product_yacht_flag = " + IIf(oBackground.BkndCriteriaYachtFlag, "'Y'", "'N'") + ",")
                    sQuery.Append(" evoback_feature_flag = " + IIf(oBackground.BkndCriteriaFeatureFlag, "'Y'", "'N'"))

                    sQuery.Append(" WHERE evoback_id = " + oBackground.BkndCriteriaItemID.ToString)

                End If

            End If

            ' first Insert new or update old record ...
            SqlCommand.CommandText = sQuery.ToString
            SqlCommand.ExecuteNonQuery()

            bReturnStatus = True

        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in insertOrUpdateBackground(ByVal bIsUpdateItem As Boolean, ByVal nBackID As Integer, ByVal sSetActive As String, ByVal sSetProduct As String)</b><br />" + ex.Message

        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing

            SqlCommand.Dispose()
            SqlCommand = Nothing
        End Try

        Return bReturnStatus

    End Function

    Public Function getBackground(ByVal nBackID As Integer) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader = Nothing
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing '

        Try

            SqlConn.ConnectionString = adminConnectStr

            SqlConn.Open()
            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 240

            sQuery.Append("SELECT * FROM Evolution_Backgrounds WHERE evoback_id = " + nBackID.ToString)

            SqlCommand.CommandText = sQuery.ToString
            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getBackground load datatable</b><br /> " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getBackground(ByVal nBackID As Integer) As DataTable</b><br />" + ex.Message

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

    Public Function getMaxBackgroundID() As Integer

        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Dim nBackgroundID As Integer = 0

        Try

            SqlConn.ConnectionString = adminConnectStr
            SqlConn.Open()
            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 60

            sQuery.Append("SELECT MAX(evoback_id) AS MaxBackgroundID FROM Evolution_Backgrounds WITH(NOLOCK)")

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "getMaxBackgroundID() As Integer", sQuery.ToString)

            SqlCommand.CommandText = sQuery.ToString
            SqlReader = SqlCommand.ExecuteReader()

            If SqlReader.HasRows Then
                SqlReader.Read()
                If Not IsDBNull(SqlReader.Item("MaxBackgroundID")) Then
                    If Not String.IsNullOrEmpty(SqlReader.Item("MaxBackgroundID").ToString.Trim) Then
                        nBackgroundID = CInt(SqlReader.Item("MaxBackgroundID").ToString)
                    End If
                End If
                SqlReader.Close()
            End If

        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in getMaxBackgroundID() As Integer " + ex.Message

        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing

            SqlCommand.Dispose()
            SqlCommand = Nothing
        End Try

        Return nBackgroundID

    End Function

#End Region

#Region "admin_asset_insight_functions"

    Public Function getEvaluesCount() As Long

        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Dim _dataTable As New DataTable
        Dim _recordSet As System.Data.SqlClient.SqlDataReader : _recordSet = Nothing

        Dim nReturnCount As Long = 0

        Try

            sQuery.Append("SELECT COUNT(*) AS TOTALEVALUES")
            sQuery.Append(" FROM Aircraft_FMV WITH (NOLOCK)")
            sQuery.Append(" WHERE (afmv_latest_flag = 'Y' AND afmv_value > 0 AND afmv_status = 'Y')")

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "admin_center_dataLayer.vb", sQuery.ToString)

            SqlConn.ConnectionString = adminConnectString
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
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in getEvaluesCount load datatable " + constrExc.Message
            End Try

            _recordSet.Close()
            _recordSet = Nothing

            If _dataTable.Rows.Count > 0 Then

                For Each r As DataRow In _dataTable.Rows

                    If Not IsDBNull(r.Item("TOTALEVALUES")) Then
                        If Not String.IsNullOrEmpty(r.Item("TOTALEVALUES").ToString.Trim) Then
                            If IsNumeric(r.Item("TOTALEVALUES").ToString.Trim) Then
                                nReturnCount = CLng(r.Item("TOTALEVALUES").ToString)
                            End If
                        End If
                    End If

                Next

            End If ' _dataTable.Rows.Count > 0 Then
        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in getEvaluesCount() As long " + ex.Message

        Finally

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing

            SqlCommand.Dispose()
            SqlCommand = Nothing
        End Try

        Return nReturnCount

    End Function


    Public Function getJetsTurbopropsCount() As Long

        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Dim _dataTable As New DataTable
        Dim _recordSet As System.Data.SqlClient.SqlDataReader : _recordSet = Nothing

        Dim nReturnCount As Long = 0

        Try

            sQuery.Append("SELECT DISTINCT COUNT(*) AS TOTALJETTURBO")
            sQuery.Append(" FROM Aircraft_Flat WITH (NOLOCK)")
            sQuery.Append(" WHERE (Aircraft_Flat.ac_lifecycle_stage = 3)")
            sQuery.Append(" AND (Aircraft_Flat.amod_airframe_type_code = 'F') AND (Aircraft_Flat.amod_product_business_flag = 'Y') AND ")
            sQuery.Append(" (Aircraft_Flat.ac_product_business_flag = 'Y') AND (Aircraft_Flat.amod_type_code IN ('J', 'E', 'T'))")
            sQuery.Append(" AND (Aircraft_Flat.ac_journ_id = 0)")

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "admin_center_dataLayer.vb", sQuery.ToString)

            SqlConn.ConnectionString = adminConnectString
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
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in getJetsTurbopropsCount load datatable " + constrExc.Message
            End Try

            _recordSet.Close()
            _recordSet = Nothing

            If _dataTable.Rows.Count > 0 Then

                For Each r As DataRow In _dataTable.Rows

                    If Not IsDBNull(r.Item("TOTALJETTURBO")) Then
                        If Not String.IsNullOrEmpty(r.Item("TOTALJETTURBO").ToString.Trim) Then
                            If IsNumeric(r.Item("TOTALJETTURBO").ToString.Trim) Then
                                nReturnCount = CLng(r.Item("TOTALJETTURBO").ToString)
                            End If
                        End If
                    End If

                Next

            End If ' _dataTable.Rows.Count > 0 Then
        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in getJetsTurbopropsCount() As Long " + ex.Message

        Finally

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing

            SqlCommand.Dispose()
            SqlCommand = Nothing
        End Try

        Return nReturnCount

    End Function

    Public Function getEstimatesInQueueCount() As Long

        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Dim _dataTable As New DataTable
        Dim _recordSet As System.Data.SqlClient.SqlDataReader : _recordSet = Nothing

        Dim nReturnCount As Long = 0

        Try

            sQuery.Append("SELECT COUNT(*) AS TOTALESTIMATES")
            sQuery.Append(" FROM View_Asset_Insight_Aircraft_To_Estimate WITH (NOLOCK)")
            sQuery.Append(" WHERE (PROCESS_ORDER < 7)")

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "admin_center_dataLayer.vb", sQuery.ToString)

            SqlConn.ConnectionString = adminConnectString
            SqlConn.Open()
            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 120

            SqlCommand.CommandText = sQuery.ToString
            _recordSet = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                _dataTable.Load(_recordSet)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = _dataTable.GetErrors()
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in getEstimatesInQueueCount load datatable " + constrExc.Message
            End Try

            _recordSet.Close()
            _recordSet = Nothing

            If _dataTable.Rows.Count > 0 Then

                For Each r As DataRow In _dataTable.Rows

                    If Not IsDBNull(r.Item("TOTALESTIMATES")) Then
                        If Not String.IsNullOrEmpty(r.Item("TOTALESTIMATES").ToString.Trim) Then
                            If IsNumeric(r.Item("TOTALESTIMATES").ToString.Trim) Then
                                nReturnCount = CLng(r.Item("TOTALESTIMATES").ToString)
                            End If
                        End If
                    End If

                Next

            End If ' _dataTable.Rows.Count > 0 Then
        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in getEstimatesInQueueCount() As Long " + ex.Message

        Finally

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing

            SqlCommand.Dispose()
            SqlCommand = Nothing
        End Try

        Return nReturnCount

    End Function

    Public Sub getModelsAndAircraftNotMappedCount(ByRef modelsNotMapped As Long, ByRef acNotMapped As Long)

        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Dim _dataTable As New DataTable
        Dim _recordSet As System.Data.SqlClient.SqlDataReader : _recordSet = Nothing

        modelsNotMapped = 0
        acNotMapped = 0

        Try

            sQuery.Append("SELECT DISTINCT COUNT(distinct amod_id) as NUMMODELS, COUNT(distinct ac_id) as NUMAIRCRAFT")
            sQuery.Append(" FROM Aircraft_Flat WITH (NOLOCK)")
            sQuery.Append(" WHERE (Aircraft_Flat.ac_lifecycle_stage = 3)")
            sQuery.Append(" AND (Aircraft_Flat.amod_airframe_type_code = 'F') AND (Aircraft_Flat.amod_product_business_flag = 'Y') AND ")
            sQuery.Append(" (Aircraft_Flat.ac_product_business_flag = 'Y') AND (Aircraft_Flat.amod_type_code IN ('J', 'E', 'T'))")
            sQuery.Append(" AND (Aircraft_Flat.ac_journ_id = 0)")
            sQuery.Append(" AND (amod_id NOT IN (SELECT DISTINCT aimodel_jetnet_amod_id FROM Asset_Insight_Model WITH (NOLOCK) WHERE aimodel_jetnet_amod_id > 0))")

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "admin_center_dataLayer.vb", sQuery.ToString)

            SqlConn.ConnectionString = adminConnectString
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
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in getModelsNotMappedCount load datatable " + constrExc.Message
            End Try

            _recordSet.Close()
            _recordSet = Nothing

            If _dataTable.Rows.Count > 0 Then

                For Each r As DataRow In _dataTable.Rows

                    If Not IsDBNull(r.Item("NUMMODELS")) Then
                        If Not String.IsNullOrEmpty(r.Item("NUMMODELS").ToString.Trim) Then
                            If IsNumeric(r.Item("NUMMODELS").ToString.Trim) Then
                                modelsNotMapped = CLng(r.Item("NUMMODELS").ToString)
                            End If
                        End If
                    End If

                    If Not IsDBNull(r.Item("NUMAIRCRAFT")) Then
                        If Not String.IsNullOrEmpty(r.Item("NUMAIRCRAFT").ToString.Trim) Then
                            If IsNumeric(r.Item("NUMAIRCRAFT").ToString.Trim) Then
                                acNotMapped = CLng(r.Item("NUMAIRCRAFT").ToString)
                            End If
                        End If
                    End If

                Next

            End If ' _dataTable.Rows.Count > 0 Then
        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in getModelsAndAircraftNotMappedCount(ByRef modelsNotMapped As Long, ByRef acNotMapped As Long) " + ex.Message

        Finally

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing

            SqlCommand.Dispose()
            SqlCommand = Nothing
        End Try

    End Sub

    Public Function getLatestEstimatesCount() As Long

        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Dim _dataTable As New DataTable
        Dim _recordSet As System.Data.SqlClient.SqlDataReader : _recordSet = Nothing

        Dim nReturnCount As Long = 0

        Try

            sQuery.Append("SELECT COUNT(*) AS LATESTESTIMATES")
            sQuery.Append(" FROM Aircraft_FMV WITH (NOLOCK)")
            sQuery.Append(" WHERE (afmv_latest_flag = 'Y' AND afmv_date > GETDATE()-1)")

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "admin_center_dataLayer.vb", sQuery.ToString)

            SqlConn.ConnectionString = adminConnectString
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
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in getLatestEstimatesCount load datatable " + constrExc.Message
            End Try

            _recordSet.Close()
            _recordSet = Nothing

            If _dataTable.Rows.Count > 0 Then

                For Each r As DataRow In _dataTable.Rows

                    If Not IsDBNull(r.Item("LATESTESTIMATES")) Then
                        If Not String.IsNullOrEmpty(r.Item("LATESTESTIMATES").ToString.Trim) Then
                            If IsNumeric(r.Item("LATESTESTIMATES").ToString.Trim) Then
                                nReturnCount = CLng(r.Item("LATESTESTIMATES").ToString)
                            End If
                        End If
                    End If

                Next

            End If ' _dataTable.Rows.Count > 0 Then
        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in getLatestEstimatesCount() As Long " + ex.Message

        Finally

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing

            SqlCommand.Dispose()
            SqlCommand = Nothing
        End Try

        Return nReturnCount

    End Function

    Public Function getAircraftOnProbationCount() As Long

        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Dim _dataTable As New DataTable
        Dim _recordSet As System.Data.SqlClient.SqlDataReader : _recordSet = Nothing

        Dim nReturnCount As Long = 0

        Try

            sQuery.Append("SELECT DISTINCT COUNT(*) AS ACONPROBATION")
            sQuery.Append(" FROM Asset_Insight_Do_Not_Process WITH (NOLOCK)")
            sQuery.Append(" WHERE (aidonot_process_status = 'N')")

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "admin_center_dataLayer.vb", sQuery.ToString)

            SqlConn.ConnectionString = adminConnectString
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
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in getAircraftOnProbationCount load datatable " + constrExc.Message
            End Try

            _recordSet.Close()
            _recordSet = Nothing

            If _dataTable.Rows.Count > 0 Then

                For Each r As DataRow In _dataTable.Rows

                    If Not IsDBNull(r.Item("ACONPROBATION")) Then
                        If Not String.IsNullOrEmpty(r.Item("ACONPROBATION").ToString.Trim) Then
                            If IsNumeric(r.Item("ACONPROBATION").ToString.Trim) Then
                                nReturnCount = CLng(r.Item("ACONPROBATION").ToString)
                            End If
                        End If
                    End If

                Next

            End If ' _dataTable.Rows.Count > 0 Then
        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in getAircraftOnProbationCount() As Long " + ex.Message

        Finally

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing

            SqlCommand.Dispose()
            SqlCommand = Nothing
        End Try

        Return nReturnCount

    End Function

    Public Sub display_asset_insight_summary_table(ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False)

        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False

        Try

            out_htmlString = ""

            Dim eValuesTotal As Long = getEvaluesCount()
            Dim aircraftTotal As Long = getJetsTurbopropsCount()

            htmlOut.Append("<table id=""eValueSummaryTable"" width=""90%"" cellpadding=""2"" cellspacing=""0"">")

            htmlOut.Append("<tr><td valign=""top"" align=""center"" colspan=""2""><strong>eValue Summary</strong></td></tr>")

            htmlOut.Append("<tr><td valign=""top"" align=""left"" nowrap=""nowrap"">Total Jets & TurboProps:&nbsp;</td><td align=""right"">")
            'htmlOut.Append("<a class=""underline"" href=""adminAssetInsight.aspx?task=results&item=totaljets"" title=""Show Resuts"">")
            htmlOut.Append(aircraftTotal.ToString)
            'htmlOut.Append("</a>")

            htmlOut.Append("</td></tr>")

            htmlOut.Append("<tr><td valign=""top"" align=""left"">Total eValue Estimates</td><td align=""right"">")
            'htmlOut.Append("<a class=""underline"" href=""adminAssetInsight.aspx?task=results&item=totalevalue"" title=""Show Resuts"">")
            htmlOut.Append(eValuesTotal.ToString)
            'htmlOut.Append("</a>")

            htmlOut.Append("</td></tr>")

            htmlOut.Append("<tr><td valign=""top"" align=""left"">Percent Estimated</td><td align=""right"">")

            htmlOut.Append(CInt((eValuesTotal / aircraftTotal) * 100).ToString + "%")

            htmlOut.Append("</td></tr>")

            htmlOut.Append("</table>")

            out_htmlString = htmlOut.ToString

        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in display_asset_insight_summary_table(ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False) " + ex.Message

        Finally

            htmlOut = Nothing

        End Try

    End Sub

    Public Sub display_asset_insight_processing_table(ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False)

        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False

        Try

            out_htmlString = ""

            Dim latestEstimates As Long = getLatestEstimatesCount()

            Dim estimatesInQueue As Long = getEstimatesInQueueCount()

            Dim modelsNotMapped As Long = 0
            Dim aircraftNotMapped As Long = 0

            getModelsAndAircraftNotMappedCount(modelsNotMapped, aircraftNotMapped)

            Dim aircraftOnProbation As Long = getAircraftOnProbationCount()

            htmlOut.Append("<table id=""eValueProcessingTable"" width=""90%"" cellpadding=""2"" cellspacing=""0"">")

            htmlOut.Append("<tr><td valign=""top"" align=""center"" colspan=""2""><strong>eValue Processing</strong></td></tr>")

            htmlOut.Append("<tr><td valign=""top"" align=""left"" nowrap=""nowrap"">Latest eValue Estimates (Today):&nbsp;</td><td align=""right"">")
            htmlOut.Append("<a class=""underline"" href=""adminAssetInsight.aspx?task=results&item=latestEstimates"" onclick=""ShowLoadingMessage('DivLoadingMessage', 'Loading Aircraft', 'Loading Results ... Please Wait ...');"" title=""Show Resuts"">")
            htmlOut.Append(latestEstimates.ToString)
            htmlOut.Append("</a>")

            htmlOut.Append("</td></tr>")

            htmlOut.Append("<tr><td valign=""top"" align=""left"" nowrap=""nowrap"">eValue Estimates in Queue:&nbsp;</td><td align=""right"">")
            htmlOut.Append("<a class=""underline"" href=""adminAssetInsight.aspx?task=results&item=estimatesInQueue"" onclick=""ShowLoadingMessage('DivLoadingMessage', 'Loading Aircraft', 'Loading Results ... Please Wait ...');"" title=""Show Resuts"">")
            htmlOut.Append(estimatesInQueue.ToString)
            htmlOut.Append("</a>")

            htmlOut.Append("</td></tr>")

            htmlOut.Append("<tr><td valign=""top"" align=""left"">Models Not Mapped to Assets:&nbsp;</td><td align=""right"">")
            htmlOut.Append("<a class=""underline"" href=""adminAssetInsight.aspx?task=results&item=modelsNotMapped"" onclick=""ShowLoadingMessage('DivLoadingMessage', 'Loading Aircraft', 'Loading Results ... Please Wait ...');"" title=""Show Resuts"">")
            htmlOut.Append(modelsNotMapped.ToString)
            htmlOut.Append("</a>")

            htmlOut.Append("</td></tr>")

            htmlOut.Append("<tr><td valign=""top"" align=""left"">Aircraft Not Mapped to Assets:&nbsp;</td><td align=""right"">")
            htmlOut.Append("<a class=""underline"" href=""adminAssetInsight.aspx?task=results&item=aircraftNotMapped"" onclick=""ShowLoadingMessage('DivLoadingMessage', 'Loading Aircraft', 'Loading Results ... Please Wait ...');"" title=""Show Resuts"">")
            htmlOut.Append(aircraftNotMapped.ToString)
            htmlOut.Append("</a>")

            htmlOut.Append("</td></tr>")

            htmlOut.Append("<tr><td valign=""top"" align=""left"">Aircraft on Probation:&nbsp;</td><td align=""right"">")
            htmlOut.Append("<a class=""underline"" href=""adminAssetInsight.aspx?task=results&item=aircraftOnProbation"" onclick=""ShowLoadingMessage('DivLoadingMessage', 'Loading Aircraft', 'Loading Results ... Please Wait ...');"" title=""Show Resuts"">")
            htmlOut.Append(aircraftOnProbation.ToString)
            htmlOut.Append("</a>")

            htmlOut.Append("</td></tr>")

            htmlOut.Append("</table>")

            out_htmlString = htmlOut.ToString

        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in display_asset_insight_processing_table(ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False) " + ex.Message

        Finally

            htmlOut = Nothing

        End Try

    End Sub

    Public Sub display_latest_estimates_table(ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False)

        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False

        Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
        Dim SqlConn As New System.Data.SqlClient.SqlConnection
        Dim SqlCommand As New System.Data.SqlClient.SqlCommand

        Dim _dataTable As New DataTable
        Dim _recordSet As System.Data.SqlClient.SqlDataReader : _recordSet = Nothing

        Dim sQuery As New StringBuilder()

        Try

            out_htmlString = ""


            SqlConn.ConnectionString = adminConnectString

            SqlConn.Open()

            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = System.Data.CommandType.Text
            SqlCommand.CommandTimeout = 90

            sQuery.Append("SELECT TOP 500 afmv_source_id AS ANALYSISID, afmv_date AS EDATE, amod_make_name AS MAKE, amod_model_name AS MODEL,")
            sQuery.Append(" ac_mfr_year AS MFRYEAR, ac_year AS DLVYEAR, ac_ser_no_sort,")
            sQuery.Append(" ac_ser_no_full AS SERNO, afmv_ac_id ACID, afmv_value AS EVALUE, afmv_airframe_hrs AS ESTHOURS, ac_est_airframe_hrs AS ESTHRSFLIGHTS, ")
            sQuery.Append(" amod_id AS MODID, afmv_jetnet_assumptions AS JETNETASSUMPTIONS,")
            sQuery.Append(" afmv_detail_note AS ASSETASSUMPTIONS, afmv_latest_flag AS ELATEST, afmv_status AS ESTATUS, afmv_results AS EVALUERESULTS")
            sQuery.Append(" FROM Aircraft_FMV WITH (NOLOCK)")
            sQuery.Append(" LEFT OUTER JOIN Aircraft_Flat WITH (NOLOCK) ON ac_id = afmv_ac_id AND ac_journ_id = 0")
            sQuery.Append(" WHERE afmv_date > GETDATE()-1 AND afmv_latest_flag='Y'")
            sQuery.Append(" ORDER BY afmv_id DESC")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />display_latest_estimates_table(ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False)<br />" + sQuery.ToString

            SqlCommand.CommandText = sQuery.ToString
            _recordSet = SqlCommand.ExecuteReader()

            Try
                _dataTable.Load(_recordSet)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = _dataTable.GetErrors()
            End Try

            _recordSet.Close()
            _recordSet = Nothing

            If _dataTable.Rows.Count > 0 Then

                htmlOut.Append("<table id=""tabPanel0_DataTable"" cellpadding=""0"" cellspacing=""0"" border=""0"" width=""100%"">")
                htmlOut.Append("<thead><tr>")
                htmlOut.Append("<th><span class=""help_cursor"" title=""Used to select and remove aircraft from the list"">SEL</span></th>")

                If isMobileDisplay Then
                    htmlOut.Append("<th></th>")
                    htmlOut.Append("<th></th>")
                Else
                    htmlOut.Append("<th></th>")
                    htmlOut.Append("<th>ANALYSISID</th>")
                    htmlOut.Append("<th>EDATE</th>")
                    htmlOut.Append("<th>MAKE</th>")
                    htmlOut.Append("<th>MODEL</th>")
                    htmlOut.Append("<th>MFRYEAR</th>")
                    htmlOut.Append("<th>DLVYEAR</th>")
                    htmlOut.Append("<th data-priority=""1"">SERIAL <br />NUMBER</th>")
                    htmlOut.Append("<th>EVALUE</th>")
                    htmlOut.Append("<th>ESTHOURS</th>")
                    htmlOut.Append("<th>ESTHRSFLIGHTS</th>")
                    htmlOut.Append("<th>MODID</th>")
                    htmlOut.Append("<th>JETNETASSUMPTIONS</th>")
                    htmlOut.Append("<th>ASSETASSUMPTIONS</th>")
                    htmlOut.Append("<th>ELATEST</th>")
                    htmlOut.Append("<th>ESTATUS</th>")
                    htmlOut.Append("<th>EVALUERESULTS</th>")

                End If

                htmlOut.Append("</tr></thead><tbody>")

                Dim sSeparator As String = ""

                For Each r As DataRow In _dataTable.Rows

                    htmlOut.Append("<tr>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">&nbsp;</td>")
                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">" + r.Item("ACID").ToString.Trim + "</td>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

                    If Not IsDBNull(r.Item("ANALYSISID")) Then
                        If Not String.IsNullOrEmpty(r.Item("ANALYSISID").ToString.Trim) Then
                            htmlOut.Append(r.Item("ANALYSISID").ToString.Trim)
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

                    If Not IsDBNull(r.Item("EDATE")) Then
                        If Not String.IsNullOrEmpty(r.Item("EDATE").ToString.Trim) Then
                            htmlOut.Append(r.Item("EDATE").ToString.Trim)
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

                    If Not IsDBNull(r.Item("MAKE")) Then
                        If Not String.IsNullOrEmpty(r.Item("MAKE").ToString.Trim) Then
                            htmlOut.Append(r.Item("MAKE").ToString.Trim)
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

                    If Not IsDBNull(r.Item("MODEL")) Then
                        If Not String.IsNullOrEmpty(r.Item("MODEL").ToString.Trim) Then
                            htmlOut.Append(r.Item("MODEL").ToString.Trim)
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

                    If Not IsDBNull(r.Item("MFRYEAR")) Then
                        If Not String.IsNullOrEmpty(r.Item("MFRYEAR").ToString.Trim) Then
                            htmlOut.Append(r.Item("MFRYEAR").ToString.Trim)
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

                    If Not IsDBNull(r.Item("DLVYEAR")) Then
                        If Not String.IsNullOrEmpty(r.Item("DLVYEAR").ToString.Trim) Then
                            htmlOut.Append(r.Item("DLVYEAR").ToString.Trim)
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"" data-sort=""" + IIf(Not IsDBNull(r.Item("ac_ser_no_sort")), r.Item("ac_ser_no_sort").ToString, "") + """>")

                    If Not IsDBNull(r.Item("SERNO")) Then
                        If Not String.IsNullOrEmpty(r.Item("SERNO").ToString.Trim) Then
                            htmlOut.Append("<a class=""underline"" onclick='javascript:openSmallWindowJS(""DisplayAircraftDetail.aspx?acid=" + r.Item("ACID").ToString + "&jid=0"",""AircraftDetails"");' title=""Display Aircraft Details"">")
                            htmlOut.Append(r.Item("SERNO").ToString.Trim)
                            htmlOut.Append("</a>")
                        End If
                    End If

                    htmlOut.Append("</td>")


                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

                    If Not IsDBNull(r.Item("EVALUE")) Then
                        If Not String.IsNullOrEmpty(r.Item("EVALUE").ToString.Trim) Then

                            If IsNumeric(r.Item("EVALUE").ToString.Trim) Then
                                If CLng(r.Item("EVALUE").ToString.Trim) > 0 Then
                                    htmlOut.Append(FormatNumber(CLng(r.Item("EVALUE").ToString.Trim) / 1000, 0, TriState.False, TriState.False, TriState.True) + "k")
                                Else
                                    htmlOut.Append("0")
                                End If
                            End If

                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

                    If Not IsDBNull(r.Item("ESTHOURS")) Then
                        If Not String.IsNullOrEmpty(r.Item("ESTHOURS").ToString.Trim) Then
                            htmlOut.Append(r.Item("ESTHOURS").ToString.Trim)
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

                    If Not IsDBNull(r.Item("ESTHRSFLIGHTS")) Then
                        If Not String.IsNullOrEmpty(r.Item("ESTHRSFLIGHTS").ToString.Trim) Then
                            htmlOut.Append(r.Item("ESTHRSFLIGHTS").ToString.Trim)
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

                    If Not IsDBNull(r.Item("MODID")) Then
                        If Not String.IsNullOrEmpty(r.Item("MODID").ToString.Trim) Then
                            htmlOut.Append(r.Item("MODID").ToString.Trim)
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("<td align=""left"" valign=""middle"">")

                    If Not IsDBNull(r.Item("JETNETASSUMPTIONS")) Then
                        If Not String.IsNullOrEmpty(r.Item("JETNETASSUMPTIONS").ToString.Trim) Then
                            htmlOut.Append(r.Item("JETNETASSUMPTIONS").ToString.Trim)
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""wrap"">")

                    If Not IsDBNull(r.Item("ASSETASSUMPTIONS")) Then
                        If Not String.IsNullOrEmpty(r.Item("ASSETASSUMPTIONS").ToString.Trim) Then
                            htmlOut.Append(r.Item("ASSETASSUMPTIONS").ToString.Trim)
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

                    If Not IsDBNull(r.Item("ELATEST")) Then
                        If Not String.IsNullOrEmpty(r.Item("ELATEST").ToString.Trim) Then
                            htmlOut.Append(r.Item("ELATEST").ToString.Trim)
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

                    If Not IsDBNull(r.Item("ESTATUS")) Then
                        If Not String.IsNullOrEmpty(r.Item("ESTATUS").ToString.Trim) Then
                            htmlOut.Append(r.Item("ESTATUS").ToString.Trim)
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""wrap"">")

                    If Not IsDBNull(r.Item("EVALUERESULTS")) Then
                        If Not String.IsNullOrEmpty(r.Item("EVALUERESULTS").ToString.Trim) Then
                            htmlOut.Append(r.Item("EVALUERESULTS").ToString.Trim)
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("</tr>")

                Next

            End If ' _dataTable.Rows.Count > 0 Then

            htmlOut.Append("</tbody></table>")
            htmlOut.Append("<div id=""tabPanel0_Label"" class="""" style=""padding:2px;""><strong>" + _dataTable.Rows.Count.ToString + " Aircraft</strong></div>")
            htmlOut.Append("<div id=""tabPanel0_InnerTable"" align=""left"" valign=""middle"" style=""max-height:510px; overflow: auto;""></div>")

            out_htmlString = htmlOut.ToString

        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in display_latest_estimates_table(ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False) " + ex.Message

        Finally

            htmlOut = Nothing

        End Try

    End Sub

    Public Sub display_estimates_in_queue_table(ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False)

        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False

        Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
        Dim SqlConn As New System.Data.SqlClient.SqlConnection
        Dim SqlCommand As New System.Data.SqlClient.SqlCommand

        Dim _dataTable As New DataTable
        Dim _recordSet As System.Data.SqlClient.SqlDataReader : _recordSet = Nothing

        Dim sQuery As New StringBuilder()

        Try

            out_htmlString = ""


            SqlConn.ConnectionString = adminConnectString

            SqlConn.Open()

            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = System.Data.CommandType.Text
            SqlCommand.CommandTimeout = 90

            sQuery.Append("SELECT * ")
            sQuery.Append(" FROM View_Asset_Insight_Aircraft_To_Estimate WITH (NOLOCK)")
            sQuery.Append(" WHERE (PROCESS_ORDER < 7)")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />display_estimates_in_queue_table(ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False)<br />" + sQuery.ToString

            SqlCommand.CommandText = sQuery.ToString
            _recordSet = SqlCommand.ExecuteReader()

            Try
                _dataTable.Load(_recordSet)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = _dataTable.GetErrors()
            End Try

            _recordSet.Close()
            _recordSet = Nothing

            If _dataTable.Rows.Count > 0 Then

                htmlOut.Append("<table id=""tabPanel0_DataTable"" cellpadding=""0"" cellspacing=""0"" border=""0"" width=""100%"">")
                htmlOut.Append("<thead><tr>")
                htmlOut.Append("<th><span class=""help_cursor"" title=""Used to select and remove aircraft from the list"">SEL</span></th>")

                If isMobileDisplay Then
                    htmlOut.Append("<th></th>")
                    htmlOut.Append("<th></th>")
                Else
                    htmlOut.Append("<th></th>")
                    htmlOut.Append("<th>TYPE</th>")
                    htmlOut.Append("<th>MODEL</th>")
                    htmlOut.Append("<th>VERSION</th>")
                    htmlOut.Append("<th>MANUFACTURER</th>")
                    htmlOut.Append("<th>MAKE</th>")
                    htmlOut.Append("<th>MODEL</th>")
                    htmlOut.Append("<th data-priority=""1"">SERIAL<br />NUMBER</th>")
                    htmlOut.Append("<th data-priority=""2"">REG<br />NUMBER</th>")
                    htmlOut.Append("<th>ESTHOURS</th>")
                    htmlOut.Append("<th>HRSBASEDUSAGE</th>")
                    htmlOut.Append("<th>JETNETASSUMPTIONS</th>")
                    htmlOut.Append("<th>ASOFDATE</th>")
                    htmlOut.Append("<th>AIRFRAMECYCLES</th>")
                    htmlOut.Append("<th>MANUFACTURE</th>")
                    htmlOut.Append("<th>COVERAGE<br />ENGINES</th>")
                    htmlOut.Append("<th>COVERAGE<br />AIRFRAME</th>")
                    htmlOut.Append("<th>FORSALE</th>")
                    htmlOut.Append("<th>RANK</th>")
                    htmlOut.Append("<th>EDATE</th>")
                    htmlOut.Append("<th>ELATEST</th>")
                    htmlOut.Append("<th>ESTATUS</th>")
                    htmlOut.Append("<th>LIST<br />DATE</th>")
                    htmlOut.Append("<th>EVALUE</th>")
                    htmlOut.Append("<th>PROCESS<br />ORDER</th>")
                    htmlOut.Append("<th>ACTION<br />DATE</th>")
                    htmlOut.Append("<th>INSERVICEDATE</th>")
                    htmlOut.Append("<th>MFRYEAR</th>")
                    htmlOut.Append("<th>ENGINE<br />NAME</th>")
                    htmlOut.Append("<th>DLVYEAR</th>")
                    htmlOut.Append("<th>ESTHRSFLIGHTS</th>")

                End If

                htmlOut.Append("</tr></thead><tbody>")

                Dim sSeparator As String = ""

                For Each r As DataRow In _dataTable.Rows

                    htmlOut.Append("<tr>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">&nbsp;</td>")
                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">" + r.Item("ac_id").ToString.Trim + "</td>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

                    If Not IsDBNull(r.Item("type")) Then
                        If Not String.IsNullOrEmpty(r.Item("type").ToString.Trim) Then
                            htmlOut.Append(r.Item("type").ToString.Trim)
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

                    If Not IsDBNull(r.Item("model")) Then
                        If Not String.IsNullOrEmpty(r.Item("model").ToString.Trim) Then
                            htmlOut.Append(r.Item("model").ToString.Trim)
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

                    If Not IsDBNull(r.Item("version")) Then
                        If Not String.IsNullOrEmpty(r.Item("version").ToString.Trim) Then
                            htmlOut.Append(r.Item("version").ToString.Trim)
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

                    If Not IsDBNull(r.Item("manufacturer")) Then
                        If Not String.IsNullOrEmpty(r.Item("manufacturer").ToString.Trim) Then
                            htmlOut.Append(r.Item("manufacturer").ToString.Trim)
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

                    If Not IsDBNull(r.Item("amod_make_name")) Then
                        If Not String.IsNullOrEmpty(r.Item("amod_make_name").ToString.Trim) Then
                            htmlOut.Append(r.Item("amod_make_name").ToString.Trim)
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

                    If Not IsDBNull(r.Item("amod_model_name")) Then
                        If Not String.IsNullOrEmpty(r.Item("amod_model_name").ToString.Trim) Then
                            htmlOut.Append(r.Item("amod_model_name").ToString.Trim)
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"" data-sort=""" + IIf(Not IsDBNull(r.Item("serial")), r.Item("serial").ToString, "") + """>")

                    If Not IsDBNull(r.Item("serial")) Then
                        If Not String.IsNullOrEmpty(r.Item("serial").ToString.Trim) Then
                            htmlOut.Append("<a class=""underline"" onclick='javascript:openSmallWindowJS(""DisplayAircraftDetail.aspx?acid=" + r.Item("ac_id").ToString + "&jid=0"",""AircraftDetails"");' title=""Display Aircraft Details"">")
                            htmlOut.Append(r.Item("serial").ToString.Trim)
                            htmlOut.Append("</a>")
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

                    If Not IsDBNull(r.Item("tail")) Then
                        If Not String.IsNullOrEmpty(r.Item("tail").ToString.Trim) Then
                            htmlOut.Append(r.Item("tail").ToString.Trim)
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

                    If Not IsDBNull(r.Item("airframehrs")) Then
                        If Not String.IsNullOrEmpty(r.Item("airframehrs").ToString.Trim) Then
                            htmlOut.Append(r.Item("airframehrs").ToString.Trim)
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

                    If Not IsDBNull(r.Item("HRSBASEDUSAGE")) Then
                        If Not String.IsNullOrEmpty(r.Item("HRSBASEDUSAGE").ToString.Trim) Then
                            htmlOut.Append(r.Item("HRSBASEDUSAGE").ToString.Trim)
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

                    If Not IsDBNull(r.Item("JETNETASSUMPTIONS")) Then
                        If Not String.IsNullOrEmpty(r.Item("JETNETASSUMPTIONS").ToString.Trim) Then
                            htmlOut.Append(r.Item("JETNETASSUMPTIONS").ToString.Trim)
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

                    If Not IsDBNull(r.Item("ASOFDATE")) Then
                        If Not String.IsNullOrEmpty(r.Item("ASOFDATE").ToString.Trim) Then
                            htmlOut.Append(r.Item("ASOFDATE").ToString.Trim)
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

                    If Not IsDBNull(r.Item("airframecycles")) Then
                        If Not String.IsNullOrEmpty(r.Item("airframecycles").ToString.Trim) Then
                            htmlOut.Append(r.Item("airframecycles").ToString.Trim)
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

                    If Not IsDBNull(r.Item("manufacture")) Then
                        If Not String.IsNullOrEmpty(r.Item("manufacture").ToString.Trim) Then
                            htmlOut.Append(r.Item("manufacture").ToString.Trim)
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

                    If Not IsDBNull(r.Item("coverage_engines")) Then
                        If Not String.IsNullOrEmpty(r.Item("coverage_engines").ToString.Trim) Then
                            htmlOut.Append(r.Item("coverage_engines").ToString.Trim)
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

                    If Not IsDBNull(r.Item("coverage_airframe")) Then
                        If Not String.IsNullOrEmpty(r.Item("coverage_airframe").ToString.Trim) Then
                            htmlOut.Append(r.Item("coverage_airframe").ToString.Trim)
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

                    If Not IsDBNull(r.Item("ac_forsale_flag")) Then
                        If Not String.IsNullOrEmpty(r.Item("ac_forsale_flag").ToString.Trim) Then
                            htmlOut.Append(r.Item("ac_forsale_flag").ToString.Trim)
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

                    If Not IsDBNull(r.Item("amodrank_rank")) Then
                        If Not String.IsNullOrEmpty(r.Item("amodrank_rank").ToString.Trim) Then
                            htmlOut.Append(r.Item("amodrank_rank").ToString.Trim)
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

                    If Not IsDBNull(r.Item("afmv_date")) Then
                        If Not String.IsNullOrEmpty(r.Item("afmv_date").ToString.Trim) Then
                            htmlOut.Append(r.Item("afmv_date").ToString.Trim)
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

                    If Not IsDBNull(r.Item("afmv_latest_flag")) Then
                        If Not String.IsNullOrEmpty(r.Item("afmv_latest_flag").ToString.Trim) Then
                            htmlOut.Append(r.Item("afmv_latest_flag").ToString.Trim)
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

                    If Not IsDBNull(r.Item("afmv_status")) Then
                        If Not String.IsNullOrEmpty(r.Item("afmv_status").ToString.Trim) Then
                            htmlOut.Append(r.Item("afmv_status").ToString.Trim)
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

                    If Not IsDBNull(r.Item("ac_list_date")) Then
                        If Not String.IsNullOrEmpty(r.Item("ac_list_date").ToString.Trim) Then
                            htmlOut.Append(FormatDateTime(r.Item("ac_list_date").ToString.Trim, DateFormat.ShortDate))
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

                    If Not IsDBNull(r.Item("afmv_value")) Then
                        If Not String.IsNullOrEmpty(r.Item("afmv_value").ToString.Trim) Then
                            If IsNumeric(r.Item("afmv_value").ToString.Trim) Then
                                If CLng(r.Item("afmv_value").ToString.Trim) > 0 Then
                                    htmlOut.Append(FormatNumber(CLng(r.Item("afmv_value").ToString.Trim) / 1000, 0, TriState.False, TriState.False, TriState.True) + "k")
                                Else
                                    htmlOut.Append("0")
                                End If
                            End If
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

                    If Not IsDBNull(r.Item("PROCESS_ORDER")) Then
                        If Not String.IsNullOrEmpty(r.Item("PROCESS_ORDER").ToString.Trim) Then
                            htmlOut.Append(r.Item("PROCESS_ORDER").ToString.Trim)
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

                    If Not IsDBNull(r.Item("ac_action_date")) Then
                        If Not String.IsNullOrEmpty(r.Item("ac_action_date").ToString.Trim) Then
                            htmlOut.Append(FormatDateTime(r.Item("ac_action_date").ToString.Trim, DateFormat.ShortDate))
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

                    If Not IsDBNull(r.Item("inservicedate")) Then
                        If Not String.IsNullOrEmpty(r.Item("inservicedate").ToString.Trim) Then
                            htmlOut.Append(FormatDateTime(r.Item("inservicedate").ToString.Trim, DateFormat.ShortDate))
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

                    If Not IsDBNull(r.Item("ac_mfr_year")) Then
                        If Not String.IsNullOrEmpty(r.Item("ac_mfr_year").ToString.Trim) Then
                            htmlOut.Append(r.Item("ac_mfr_year").ToString.Trim)
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

                    If Not IsDBNull(r.Item("ac_engine_name")) Then
                        If Not String.IsNullOrEmpty(r.Item("ac_engine_name").ToString.Trim) Then
                            htmlOut.Append(r.Item("ac_engine_name").ToString.Trim)
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

                    If Not IsDBNull(r.Item("ac_year")) Then
                        If Not String.IsNullOrEmpty(r.Item("ac_year").ToString.Trim) Then
                            htmlOut.Append(r.Item("ac_year").ToString.Trim)
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

                    If Not IsDBNull(r.Item("ac_est_airframe_hrs")) Then
                        If Not String.IsNullOrEmpty(r.Item("ac_est_airframe_hrs").ToString.Trim) Then
                            htmlOut.Append(r.Item("ac_est_airframe_hrs").ToString.Trim)
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("</tr>")

                Next

            End If ' _dataTable.Rows.Count > 0 Then

            htmlOut.Append("</tbody></table>")
            htmlOut.Append("<div id=""tabPanel0_Label"" class="""" style=""padding:2px;""><strong>" + _dataTable.Rows.Count.ToString + " Aircraft</strong></div>")
            htmlOut.Append("<div id=""tabPanel0_InnerTable"" align=""left"" valign=""middle"" style=""max-height:510px; overflow: auto;""></div>")

            out_htmlString = htmlOut.ToString

        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in display_estimates_in_queue_table(ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False) " + ex.Message

        Finally

            htmlOut = Nothing

        End Try

    End Sub

    Public Sub display_models_not_mapped_table(ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False)

        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False

        Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
        Dim SqlConn As New System.Data.SqlClient.SqlConnection
        Dim SqlCommand As New System.Data.SqlClient.SqlCommand

        Dim _dataTable As New DataTable
        Dim _recordSet As System.Data.SqlClient.SqlDataReader : _recordSet = Nothing

        Dim sQuery As New StringBuilder()

        Try

            out_htmlString = ""


            SqlConn.ConnectionString = adminConnectString

            SqlConn.Open()

            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = System.Data.CommandType.Text
            SqlCommand.CommandTimeout = 90

            sQuery.Append("SELECT distinct amod_make_name AS MAKE, amod_model_name AS MODEL, amod_id AS AMODID, COUNT(distinct ac_id) AS NUMAIRCRAFT")
            sQuery.Append(" FROM Aircraft_Flat WITH (NOLOCK)")
            sQuery.Append(" WHERE (ac_lifecycle_stage = 3) AND (amod_airframe_type_code = 'F') AND (amod_product_business_flag = 'Y') AND")
            sQuery.Append(" (ac_product_business_flag = 'Y') AND (amod_type_code IN ('J', 'E', 'T')) AND (ac_journ_id = 0)")
            sQuery.Append(" AND amod_id not in (SELECT DISTINCT aimodel_jetnet_amod_id FROM Asset_Insight_Model WITH (NOLOCK) WHERE aimodel_jetnet_amod_id > 0)")
            sQuery.Append(" GROUP BY amod_make_name, amod_model_name, amod_id")
            sQuery.Append(" ORDER BY amod_make_name, amod_model_name")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />display_models_not_mapped_table(ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False)<br />" + sQuery.ToString

            SqlCommand.CommandText = sQuery.ToString
            _recordSet = SqlCommand.ExecuteReader()

            Try
                _dataTable.Load(_recordSet)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = _dataTable.GetErrors()
            End Try

            _recordSet.Close()
            _recordSet = Nothing

            If _dataTable.Rows.Count > 0 Then

                htmlOut.Append("<table id=""tabPanel0_DataTable"" cellpadding=""0"" cellspacing=""0"" border=""0"" width=""100%"">")
                htmlOut.Append("<thead><tr>")
                htmlOut.Append("<th><span class=""help_cursor"" title=""Used to select and remove aircraft from the list"">SEL</span></th>")

                If isMobileDisplay Then
                    htmlOut.Append("<th></th>")
                    htmlOut.Append("<th></th>")
                Else
                    htmlOut.Append("<th></th>")
                    htmlOut.Append("<th>MAKE</th>")
                    htmlOut.Append("<th>MODEL</th>")
                    htmlOut.Append("<th>MODELID</th>")
                    htmlOut.Append("<th>NUMAIRCRAFT</th>")

                End If

                htmlOut.Append("</tr></thead><tbody>")

                Dim sSeparator As String = ""

                For Each r As DataRow In _dataTable.Rows

                    htmlOut.Append("<tr>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">&nbsp;</td>")
                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">" + r.Item("AMODID").ToString.Trim + "</td>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

                    If Not IsDBNull(r.Item("MAKE")) Then
                        If Not String.IsNullOrEmpty(r.Item("MAKE").ToString.Trim) Then
                            htmlOut.Append(r.Item("MAKE").ToString.Trim)
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

                    If Not IsDBNull(r.Item("MODEL")) Then
                        If Not String.IsNullOrEmpty(r.Item("MODEL").ToString.Trim) Then
                            htmlOut.Append(r.Item("MODEL").ToString.Trim)
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">" + r.Item("AMODID").ToString.Trim + "</td>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

                    If Not IsDBNull(r.Item("NUMAIRCRAFT")) Then
                        If Not String.IsNullOrEmpty(r.Item("NUMAIRCRAFT").ToString.Trim) Then
                            htmlOut.Append(r.Item("NUMAIRCRAFT").ToString.Trim)
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("</tr>")

                Next

            End If ' _dataTable.Rows.Count > 0 Then

            htmlOut.Append("</tbody></table>")
            htmlOut.Append("<div id=""tabPanel0_Label"" class="""" style=""padding:2px;""><strong>" + _dataTable.Rows.Count.ToString + " Models</strong></div>")
            htmlOut.Append("<div id=""tabPanel0_InnerTable"" align=""left"" valign=""middle"" style=""max-height:510px; overflow: auto;""></div>")

            out_htmlString = htmlOut.ToString

        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in display_models_not_mapped_table(ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False) " + ex.Message

        Finally

            htmlOut = Nothing

        End Try

    End Sub

    Public Sub display_aircraft_not_mapped_table(ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False)

        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False

        Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
        Dim SqlConn As New System.Data.SqlClient.SqlConnection
        Dim SqlCommand As New System.Data.SqlClient.SqlCommand

        Dim _dataTable As New DataTable
        Dim _recordSet As System.Data.SqlClient.SqlDataReader : _recordSet = Nothing

        Dim sQuery As New StringBuilder()

        Try

            out_htmlString = ""


            SqlConn.ConnectionString = adminConnectString

            SqlConn.Open()

            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = System.Data.CommandType.Text
            SqlCommand.CommandTimeout = 90

            sQuery.Append("SELECT distinct ac_id, amod_make_name AS MAKE, amod_model_name AS MODEL,")
            sQuery.Append(" ac_mfr_year as MFRYEAR, ac_year as DLVYEAR,")
            sQuery.Append(" ac_ser_no_sort, ac_ser_no_full AS SERNO, ac_reg_no AS REGNO")
            sQuery.Append(" FROM Aircraft_Flat WITH (NOLOCK)")
            sQuery.Append(" WHERE (ac_lifecycle_stage = 3) AND (amod_airframe_type_code = 'F') AND (amod_product_business_flag = 'Y') AND")
            sQuery.Append(" (ac_product_business_flag = 'Y') AND (amod_type_code IN ('J', 'E', 'T')) AND (ac_journ_id = 0)")
            sQuery.Append(" AND amod_id NOT IN (SELECT DISTINCT aimodel_jetnet_amod_id FROM Asset_Insight_Model WITH (NOLOCK) WHERE aimodel_jetnet_amod_id > 0)")
            sQuery.Append(" ORDER BY amod_make_name, amod_model_name")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />display_aircraft_not_mapped_table(ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False)<br />" + sQuery.ToString

            SqlCommand.CommandText = sQuery.ToString
            _recordSet = SqlCommand.ExecuteReader()

            Try
                _dataTable.Load(_recordSet)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = _dataTable.GetErrors()
            End Try

            _recordSet.Close()
            _recordSet = Nothing

            If _dataTable.Rows.Count > 0 Then

                htmlOut.Append("<table id=""tabPanel0_DataTable"" cellpadding=""0"" cellspacing=""0"" border=""0"" width=""100%"">")
                htmlOut.Append("<thead><tr>")
                htmlOut.Append("<th><span class=""help_cursor"" title=""Used to select and remove aircraft from the list"">SEL</span></th>")

                If isMobileDisplay Then
                    htmlOut.Append("<th></th>")
                    htmlOut.Append("<th></th>")
                Else
                    htmlOut.Append("<th></th>")
                    htmlOut.Append("<th>MAKE</th>")
                    htmlOut.Append("<th>MODEL</th>")
                    htmlOut.Append("<th data-priority=""1"">SERIAL <br />NUMBER</th>")
                    htmlOut.Append("<th data-priority=""2"">REG <br />NUMBER</th>")
                    htmlOut.Append("<th>MFRYEAR</th>")
                    htmlOut.Append("<th>DLVYEAR</th>")
                End If

                htmlOut.Append("</tr></thead><tbody>")

                Dim sSeparator As String = ""

                For Each r As DataRow In _dataTable.Rows

                    htmlOut.Append("<tr>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">&nbsp;</td>")
                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">" + r.Item("ac_id").ToString.Trim + "</td>")


                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

                    If Not IsDBNull(r.Item("MAKE")) Then
                        If Not String.IsNullOrEmpty(r.Item("MAKE").ToString.Trim) Then
                            htmlOut.Append(r.Item("MAKE").ToString.Trim)
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

                    If Not IsDBNull(r.Item("MODEL")) Then
                        If Not String.IsNullOrEmpty(r.Item("MODEL").ToString.Trim) Then
                            htmlOut.Append(r.Item("MODEL").ToString.Trim)
                        End If
                    End If

                    htmlOut.Append("</td>")


                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"" data-sort=""" + IIf(Not IsDBNull(r.Item("ac_ser_no_sort")), r.Item("ac_ser_no_sort").ToString, "") + """>")

                    If Not IsDBNull(r.Item("SERNO")) Then
                        If Not String.IsNullOrEmpty(r.Item("SERNO").ToString.Trim) Then
                            htmlOut.Append("<a class=""underline"" onclick='javascript:openSmallWindowJS(""DisplayAircraftDetail.aspx?acid=" + r.Item("ac_id").ToString + "&jid=0"",""AircraftDetails"");' title=""Display Aircraft Details"">")
                            htmlOut.Append(r.Item("SERNO").ToString.Trim)
                            htmlOut.Append("</a>")
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

                    If Not IsDBNull(r.Item("REGNO")) Then
                        If Not String.IsNullOrEmpty(r.Item("REGNO").ToString.Trim) Then
                            htmlOut.Append(r.Item("REGNO").ToString.Trim)
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

                    If Not IsDBNull(r.Item("MFRYEAR")) Then
                        If Not String.IsNullOrEmpty(r.Item("MFRYEAR").ToString.Trim) Then
                            htmlOut.Append(r.Item("MFRYEAR").ToString.Trim)
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

                    If Not IsDBNull(r.Item("DLVYEAR")) Then
                        If Not String.IsNullOrEmpty(r.Item("DLVYEAR").ToString.Trim) Then
                            htmlOut.Append(r.Item("DLVYEAR").ToString.Trim)
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("</tr>")

                Next

            End If ' _dataTable.Rows.Count > 0 Then

            htmlOut.Append("</tbody></table>")
            htmlOut.Append("<div id=""tabPanel0_Label"" class="""" style=""padding:2px;""><strong>" + _dataTable.Rows.Count.ToString + " Aircraft</strong></div>")
            htmlOut.Append("<div id=""tabPanel0_InnerTable"" align=""left"" valign=""middle"" style=""max-height:510px; overflow: auto;""></div>")

            out_htmlString = htmlOut.ToString

        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in display_aircraft_not_mapped_table(ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False) " + ex.Message

        Finally

            htmlOut = Nothing

        End Try

    End Sub

    Public Sub display_aircraft_on_probation_table(ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False)

        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False

        Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
        Dim SqlConn As New System.Data.SqlClient.SqlConnection
        Dim SqlCommand As New System.Data.SqlClient.SqlCommand

        Dim _dataTable As New DataTable
        Dim _recordSet As System.Data.SqlClient.SqlDataReader : _recordSet = Nothing

        Dim sQuery As New StringBuilder()

        Try

            out_htmlString = ""


            SqlConn.ConnectionString = adminConnectString

            SqlConn.Open()

            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = System.Data.CommandType.Text
            SqlCommand.CommandTimeout = 90

            sQuery.Append("SELECT aidonot_make AS MAKE, aidonot_model AS MODEL, aidonot_ser_no AS SERNO, aidonot_ac_id AS ACID, aidonot_notes AS NOTES")
            sQuery.Append(" FROM Asset_Insight_Do_Not_Process WITH (NOLOCK)")
            sQuery.Append(" WHERE aidonot_process_status = 'N'")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />display_aircraft_on_probation_table(ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False)<br />" + sQuery.ToString

            SqlCommand.CommandText = sQuery.ToString
            _recordSet = SqlCommand.ExecuteReader()

            Try
                _dataTable.Load(_recordSet)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = _dataTable.GetErrors()
            End Try

            _recordSet.Close()
            _recordSet = Nothing

            If _dataTable.Rows.Count > 0 Then

                htmlOut.Append("<table id=""tabPanel0_DataTable"" cellpadding=""0"" cellspacing=""0"" border=""0"" width=""100%"">")
                htmlOut.Append("<thead><tr>")
                htmlOut.Append("<th><span class=""help_cursor"" title=""Used to select and remove aircraft from the list"">SEL</span></th>")

                If isMobileDisplay Then
                    htmlOut.Append("<th></th>")
                    htmlOut.Append("<th></th>")
                Else
                    htmlOut.Append("<th></th>")
                    htmlOut.Append("<th>MAKE</th>")
                    htmlOut.Append("<th>MODEL</th>")
                    htmlOut.Append("<th data-priority=""1"">SERIAL <br />NUMBER</th>")
                    htmlOut.Append("<th>ACID</th>")
                    htmlOut.Append("<th>NOTES</th>")
                End If

                htmlOut.Append("</tr></thead><tbody>")

                Dim sSeparator As String = ""

                For Each r As DataRow In _dataTable.Rows

                    htmlOut.Append("<tr>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">&nbsp;</td>")
                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">" + r.Item("ACID").ToString.Trim + "</td>")


                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

                    If Not IsDBNull(r.Item("MAKE")) Then
                        If Not String.IsNullOrEmpty(r.Item("MAKE").ToString.Trim) Then
                            htmlOut.Append(r.Item("MAKE").ToString.Trim)
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

                    If Not IsDBNull(r.Item("MODEL")) Then
                        If Not String.IsNullOrEmpty(r.Item("MODEL").ToString.Trim) Then
                            htmlOut.Append(r.Item("MODEL").ToString.Trim)
                        End If
                    End If

                    htmlOut.Append("</td>")


                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"" data-sort=""" + IIf(Not IsDBNull(r.Item("SERNO")), r.Item("SERNO").ToString, "") + """>")

                    If Not IsDBNull(r.Item("SERNO")) Then
                        If Not String.IsNullOrEmpty(r.Item("SERNO").ToString.Trim) Then
                            htmlOut.Append("<a class=""underline"" onclick='javascript:openSmallWindowJS(""DisplayAircraftDetail.aspx?acid=" + r.Item("ACID").ToString + "&jid=0"",""AircraftDetails"");' title=""Display Aircraft Details"">")
                            htmlOut.Append(r.Item("SERNO").ToString.Trim)
                            htmlOut.Append("</a>")
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">" + r.Item("ACID").ToString.Trim + "</td>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

                    If Not IsDBNull(r.Item("NOTES")) Then
                        If Not String.IsNullOrEmpty(r.Item("NOTES").ToString.Trim) Then
                            htmlOut.Append(r.Item("NOTES").ToString.Trim)
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("</tr>")

                Next

            End If ' _dataTable.Rows.Count > 0 Then

            htmlOut.Append("</tbody></table>")
            htmlOut.Append("<div id=""tabPanel0_Label"" class="""" style=""padding:2px;""><strong>" + _dataTable.Rows.Count.ToString + " Aircraft</strong></div>")
            htmlOut.Append("<div id=""tabPanel0_InnerTable"" align=""left"" valign=""middle"" style=""max-height:510px; overflow: auto;""></div>")

            out_htmlString = htmlOut.ToString

        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in display_aircraft_on_probation_table(ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False) " + ex.Message

        Finally

            htmlOut = Nothing

        End Try

    End Sub

#End Region

#Region "admin_menutree_functions"


    Public Function UniquePage() As DataTable

        Dim sQuery = New StringBuilder()
        Dim atemptable As New DataTable
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing


        Try

            SqlConn.ConnectionString = adminConnectStr
            SqlConn.Open()
            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 60

            sQuery.Append("SELECT distinct menutree_item_name as 'menutree_page_name' from " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[HomeBase].jetnet_ra.dbo.", "") + "menu_tree order by menutree_page_name asc")

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

            SqlCommand.CommandText = sQuery.ToString
            SqlReader = SqlCommand.ExecuteReader()

            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in " & Me.GetType().FullName & "</b><br /> " + constrExc.Message
            End Try


        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in " & Me.GetType().FullName & " " + ex.Message

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
    Public Function MenuFilter(ByVal menuName As String, ByVal level As String, displayURL As String, Optional domain As String = "") As DataTable
        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try
            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
            SqlConn.Open()


            sQuery.Append("Select menutree_id, menutree_target, menutree_status,  menutree_display_name, menutree_description, menutree_page_name, menutree_item_name, menutree_display_url, menutree_order,")
            sQuery.Append(" (Select count(*) FROM " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[HomeBase].jetnet_ra.dbo.", "") + "menu_tree WITH(NOLOCK) WHERE menutree_page_name = a.menutree_item_name) As itemSubCount,")

            sQuery.Append(" (Select max(menutree_order) FROM " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[HomeBase].jetnet_ra.dbo.", "") + "menu_tree WITH(NOLOCK) WHERE menutree_page_name = a.menutree_item_name) As maxCount")

            sQuery.Append(" FROM " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[HomeBase].jetnet_ra.dbo.", "") + "menu_tree a WHERE menutree_display_name <> ''")

            If displayURL <> "" Then
                sQuery.Append(" and lower(menutree_display_url) = @displayURL")
            End If

            If menuName <> "" Then
                sQuery.Append(" and menutree_page_name = @menuName")
            End If

            If level <> "" Then
                sQuery.Append(" and menutree_item_name = @itemName")
            End If

            If domain <> "" Then
                sQuery.Append(" and menutree_home_domain LIKE(@domain)")
            End If

            sQuery.Append(" order by menutree_order")

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

            Dim SqlCommand As New SqlClient.SqlCommand(sQuery.ToString, SqlConn)

            If displayURL <> "" Then
                SqlCommand.Parameters.AddWithValue("@displayURL", displayURL.ToLower)
            End If

            If menuName <> "" Then
                SqlCommand.Parameters.AddWithValue("@menuName", menuName)
            End If

            If level <> "" Then
                SqlCommand.Parameters.AddWithValue("@itemName", level)
            End If

            If domain <> "" Then
                SqlCommand.Parameters.AddWithValue("@domain", "%" + domain + "%")
            End If

            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
            End Try

            SqlCommand.Dispose()
            SqlCommand = Nothing
        Catch ex As Exception
            Return Nothing

        Finally

            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing


        End Try

        Return atemptable

    End Function
#End Region
#Region "Dashboard Functions"
    Public Function DashboardModuleList(ByVal subID As Long, ByVal userLogin As String, ByVal seqNO As Long) As DataTable
        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try
            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
            SqlConn.Open()


            sQuery.Append(" select * FROM " & IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") & "Subscription_Install_Dashboard WITH(NOLOCK) INNER JOIN " & IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") & "Dashboard_Menu WITH(NOLOCK) on sidash_dashb_id = dashb_id where ")

            sQuery.Append(" sidash_sub_id = @subID ")
            sQuery.Append(" and sidash_login = @userLogin")
            sQuery.Append(" and sidash_seq_no = @seqNO ")

            If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
                sQuery.Append(" and dashb_system='HOMEBASE' ")
            Else
                sQuery.Append(" and dashb_system = 'EVOLUTION' ")
            End If

            sQuery.Append(" order by sidash_order ")

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

            Dim SqlCommand As New SqlClient.SqlCommand(sQuery.ToString, SqlConn)


            SqlCommand.Parameters.AddWithValue("@subID", subID)
            SqlCommand.Parameters.AddWithValue("@userLogin", userLogin)
            SqlCommand.Parameters.AddWithValue("@seqNo", seqNO)


            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
            End Try

            SqlCommand.Dispose()
            SqlCommand = Nothing
        Catch ex As Exception
            Return Nothing

        Finally

            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing


        End Try

        Return atemptable

    End Function

    Public Function DashboardSelectionList(ByRef Optional ChosenIDs As String = "", Optional SetSort As Boolean = False) As DataTable
        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try
            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
            SqlConn.Open()


            sQuery.Append("select dashb_id, dashb_area, dashb_display_title ")

            If SetSort Then
                If HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag = False Then
                    'Marketplace
                    sQuery.Append(", case dashb_id when 26 then 1 when 27 then 2 when 34 then 3 when 28 then 4 when 30 then 5 when 29 then 6 when 43 then 100 else 99 end as SORTORDER ")
                Else
                    'Aerodex
                    sQuery.Append(", case dashb_id when 35 then 1 when 27 then 2 when 34 then 3 when 28 then 4 when 30 then 5 when 29 then 6 when 43 then 100 else 99 end as SORTORDER ")
                End If
            End If


            sQuery.Append(" from " & IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") & "Dashboard_Menu with (NOLOCK) ")


            If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
                sQuery.Append(" where dashb_system='HOMEBASE' ")
            Else
                sQuery.Append(" where dashb_system = 'EVOLUTION' ")
            End If

            sQuery.Append(" and dashb_id <> 43 ")

            If ChosenIDs <> "" Then
                sQuery.Append(" and dashb_id not in (" & ChosenIDs & ") ")
            End If


            If HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag = True Then
                sQuery.Append(" and dashb_aerodex_flag='Y' ")
            End If

            If SetSort Then
                sQuery.Append(" order by SORTORDER, dashb_display_title")
            Else
                sQuery.Append(" order by dashb_area, dashb_display_title")
            End If

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

            Dim SqlCommand As New SqlClient.SqlCommand(sQuery.ToString, SqlConn)

            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
            End Try

            SqlCommand.Dispose()
            SqlCommand = Nothing
        Catch ex As Exception
            Return Nothing

        Finally

            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing


        End Try

        Return atemptable

    End Function
    Public Function DashboardDefaultSelectionList() As DataTable
        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()


        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try
            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
            SqlConn.Open()

            '-- MARKETPLACE
            'select dashb_id, dashb_area, dashb_display_title ,
            'case dashb_id when 26 then 1 when 27 then 2 when 34 then 3 when 28 then 4 when 30 then 5 when 29 then '6 else 99 end as SORTORDER
            'from Dashboard_Menu with (NOLOCK) 
            'where dashb_system = 'EVOLUTION' and dashb_id in (26, 27, 34, 28,29,30, 33,31, 32, 36)
            'order by SORTORDER, dashb_display_title

            '-- AERODEX
            'select dashb_id, dashb_area, dashb_display_title ,
            'case dashb_id when 35 then 1 when 27 then 2 when 34 then 3 when 28 then 4 when 30 then 5 when 29 then '6 else 99 end as SORTORDER
            'from Dashboard_Menu with (NOLOCK) 
            'where dashb_system = 'EVOLUTION' and dashb_id in (35, 27, 34, 28,29,30, 33,31, 32, 36)
            'order by SORTORDER, dashb_display_title

            sQuery.Append("select dashb_id, dashb_area, dashb_display_title, ")
            '
            If HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag = False Then
                'Marketplace
                sQuery.Append("case dashb_id when 26 then 1 when 27 then 2 when 34 then 3 when 28 then 4 when 30 then 5 when 29 then 6 else 99 end as SORTORDER ")
            Else
                'Aerodex
                sQuery.Append("case dashb_id when 35 then 1 when 27 then 2 when 34 then 3 when 28 then 4 when 30 then 5 when 29 then 6 else 99 end as SORTORDER ")
            End If


            sQuery.Append(" from " & IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") & "Dashboard_Menu with (NOLOCK) ")


            If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
                sQuery.Append(" where dashb_system='HOMEBASE' ")
            Else
                sQuery.Append(" where dashb_system = 'EVOLUTION' ")
            End If

            If HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag = False Then
                'Marketplace
                sQuery.Append(" and dashb_id in (26, 27, 34, 28,29,30, 33,31, 32)  ")
            Else
                'Aerodex
                sQuery.Append(" and dashb_id in (35, 27, 34, 28,29,30, 33, 31, 32) ")
            End If


            sQuery.Append(" order by SORTORDER, dashb_display_title")


            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

            Dim SqlCommand As New SqlClient.SqlCommand(sQuery.ToString, SqlConn)

            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
            End Try

            SqlCommand.Dispose()
            SqlCommand = Nothing
        Catch ex As Exception
            Return Nothing

        Finally

            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing


        End Try

        Return atemptable

    End Function
#Region "Dashboard Module Queries"

    Public Function getModule1() As DataTable

        Dim sQuery = New StringBuilder()
        Dim atemptable As New DataTable
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Dim nBackgroundID As Integer = 0

        Try

            SqlConn.ConnectionString = adminConnectStr
            SqlConn.Open()
            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 60

            sQuery.Append(" select COUNT(*) AS TOTLICENSES,")
            sQuery.Append(" SUM(CASE WHEN sub_aerodex_flag='Y' THEN 1 ELSE 0 END) AS AERODEX,")
            sQuery.Append(" SUM(CASE WHEN sub_aerodex_flag='N' THEN 1 ELSE 0 END) AS MARKETPLACE")
            sQuery.Append(" from View_JETNET_Customers with (NOLOCK)")
            sQuery.Append(" where (sub_comp_id <> 135887) AND (sublogin_demo_flag = 'N')")

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

            SqlCommand.CommandText = sQuery.ToString
            SqlReader = SqlCommand.ExecuteReader()

            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in " & Me.GetType().FullName & "</b><br /> " + constrExc.Message
            End Try


        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in " & Me.GetType().FullName & " " + ex.Message

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



    Public Function getModule2() As DataTable

        Dim sQuery = New StringBuilder()
        Dim atemptable As New DataTable
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Dim nBackgroundID As Integer = 0

        Try

            SqlConn.ConnectionString = adminConnectStr
            SqlConn.Open()
            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 60

            sQuery.Append(" select COUNT(*) AS TOTLICENSES, ")
            sQuery.Append(" SUM(CASE WHEN sub_frequency='Live' THEN 1 ELSE 0 END) AS LIVE,")
            sQuery.Append(" SUM(CASE WHEN sub_frequency='Weekly' THEN 1 ELSE 0 END) AS WEEKLY,")
            sQuery.Append(" SUM(CASE WHEN sub_frequency='Monthly' THEN 1 ELSE 0 END) AS MONTHLY ")
            sQuery.Append(" from View_JETNET_Customers with (NOLOCK)")
            sQuery.Append(" where (sub_comp_id <> 135887) And (sublogin_demo_flag = 'N') ")

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

            SqlCommand.CommandText = sQuery.ToString
            SqlReader = SqlCommand.ExecuteReader()

            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in " & Me.GetType().FullName & "</b><br /> " + constrExc.Message
            End Try


        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in " & Me.GetType().FullName & " " + ex.Message

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

    Public Function getModule3() As DataTable

        Dim sQuery = New StringBuilder()
        Dim atemptable As New DataTable
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Dim nBackgroundID As Integer = 0

        Try

            SqlConn.ConnectionString = adminConnectStr
            SqlConn.Open()
            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 60

            sQuery.Append(" Select SUM(CASE WHEN sub_business_aircraft_flag='Y' THEN 1 ELSE 0 END) AS BUSINESS,")
            sQuery.Append(" SUM(CASE WHEN sub_helicopters_flag ='Y' THEN 1 ELSE 0 END) AS HELICOPTER,")
            sQuery.Append(" SUM(CASE WHEN sub_commerical_flag ='Y' THEN 1 ELSE 0 END) AS COMMERCIAL,")
            sQuery.Append(" SUM(CASE WHEN sub_yacht_flag ='Y' THEN 1 ELSE 0 END) AS YACHT")
            sQuery.Append(" from View_JETNET_Customers with (NOLOCK)")
            sQuery.Append(" where (sub_comp_id <> 135887) And (sublogin_demo_flag = 'N')  ")



            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

            SqlCommand.CommandText = sQuery.ToString
            SqlReader = SqlCommand.ExecuteReader()

            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in " & Me.GetType().FullName & "</b><br /> " + constrExc.Message
            End Try


        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in " & Me.GetType().FullName & " " + ex.Message

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
    Public Function getModule22_2020_sales_customer_support_activity(ByVal user_or_all As String, ByVal sum_by As String) As DataTable

        Dim sQuery = New StringBuilder()
        Dim atemptable As New DataTable
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            SqlConn.ConnectionString = adminConnectStr
            SqlConn.Open()
            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 60


            ' -- 2020 SALES/CUSTOMER SUPPORT ACTIVITY SUMMARY
            sQuery.Append("  Select   Replace(Replace(jcat_subcategory_name,'Customer',''),'Marketing','') as 'TYPE',  ")
            sQuery.Append(" COUNT(*) as 'VALUE' ")

            If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE = True Then
                sQuery.Append(" From view_customer_notes with (NOLOCK) ")
            ElseIf HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER = True Then
                sQuery.Append(" From [Homebase].jetnet_ra.dbo.view_customer_notes with (NOLOCK) ")
            Else
                sQuery.Append(" From view_customer_notes with (NOLOCK) ")
            End If

            sQuery.Append(" inner Join Journal_Category with (NOLOCK) on jcat_subcategory_code=journ_subcategory_code ")
            sQuery.Append(" where jcat_category_code in ('CS','MR') ")
            sQuery.Append(" And year(journ_date) = YEAR(getdate()) ")

            If Trim(user_or_all) = "All" Then
            Else
                If Not IsNothing(HttpContext.Current.Session.Item("homebaseUserClass").home_user_id) Then
                    If Trim(HttpContext.Current.Session.Item("homebaseUserClass").home_user_id) <> "" Then
                        sQuery.Append("  AND journ_user_id = '" & HttpContext.Current.Session.Item("homebaseUserClass").home_user_id & "' ")
                    End If
                End If
            End If

            sQuery.Append(" Group by replace(replace(jcat_subcategory_name,'Customer',''),'Marketing','') ")






            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

            SqlCommand.CommandText = sQuery.ToString
            SqlReader = SqlCommand.ExecuteReader()

            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in " & Me.GetType().FullName & "</b><br /> " + constrExc.Message
            End Try


        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in " & Me.GetType().FullName & " " + ex.Message

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

    Public Function getModule21_closed_prospects(ByVal user_or_all As String, ByVal sum_by As String) As DataTable

        Dim sQuery = New StringBuilder()
        Dim atemptable As New DataTable
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            SqlConn.ConnectionString = adminConnectStr
            SqlConn.Open()
            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 60


            If Trim(sum_by) = "price" Then
                sQuery.Append(" Select Year(cprospect_target_date) As YEAR, SUM(cprospect_value) As 'VALUE' ")
            Else
                sQuery.Append(" Select Year(cprospect_target_date) As YEAR, COUNT(*) As 'VALUE' ")
            End If


            If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE = True Then
                sQuery.Append(" From View_Company_Prospects with (NOLOCK) ")
            ElseIf HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER = True Then
                sQuery.Append(" From [Homebase].jetnet_ra.dbo.View_Company_Prospects with (NOLOCK) ")
            Else
                sQuery.Append(" From View_Company_Prospects with (NOLOCK) ")
            End If

            sQuery.Append(" Where cprospect_status ='Closed' and cprospect_type='Contract' ")

            If Trim(user_or_all) = "All" Then
            Else
                If Not IsNothing(HttpContext.Current.Session.Item("homebaseUserClass").home_user_id) Then
                    If Trim(HttpContext.Current.Session.Item("homebaseUserClass").home_user_id) <> "" Then
                        sQuery.Append("  AND cprospect_user_id = '" & HttpContext.Current.Session.Item("homebaseUserClass").home_user_id & "' ")
                    End If
                End If
            End If
            sQuery.Append(" Group by YEAR(cprospect_target_date) ")
            sQuery.Append(" Order By Year(cprospect_target_date) ")


            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

            SqlCommand.CommandText = sQuery.ToString
            SqlReader = SqlCommand.ExecuteReader()

            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in " & Me.GetType().FullName & "</b><br /> " + constrExc.Message
            End Try


        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in " & Me.GetType().FullName & " " + ex.Message

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


    Public Function getModule7() As DataTable

        Dim sQuery = New StringBuilder()
        Dim atemptable As New DataTable
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            SqlConn.ConnectionString = adminConnectStr
            SqlConn.Open()
            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 60

            sQuery.Append(" Select cstat_year, cstat_month, cstat_total, cstat_marketplace, cstat_aerodex from Customer_Statistics With (NOLOCK) where cstat_year >= 2017  order by cstat_year, cstat_month ")

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

            SqlCommand.CommandText = sQuery.ToString
            SqlReader = SqlCommand.ExecuteReader()

            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in " & Me.GetType().FullName & "</b><br /> " + constrExc.Message
            End Try


        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in " & Me.GetType().FullName & " " + ex.Message

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

    Public Function getModule8() As DataTable

        Dim sQuery = New StringBuilder()
        Dim atemptable As New DataTable
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            SqlConn.ConnectionString = adminConnectStr
            SqlConn.Open()
            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 60

            sQuery.Append(" select cstat_year, cstat_month, cstat_values from Customer_Statistics with (NOLOCK) where cstat_year >= 2017 And cstat_type='Licenses' order by cstat_year, cstat_month")

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

            SqlCommand.CommandText = sQuery.ToString
            SqlReader = SqlCommand.ExecuteReader()

            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in " & Me.GetType().FullName & "</b><br /> " + constrExc.Message
            End Try


        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in " & Me.GetType().FullName & " " + ex.Message

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
#End Region
#End Region

End Class
