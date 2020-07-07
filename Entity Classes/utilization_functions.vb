Imports Microsoft.VisualBasic
Imports System.ComponentModel


' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved. 
'
'$$Archive: /commonWebProject/Entity Classes/utilization_functions.vb $
'$$Author: Matt $
'$$Date: 7/06/20 4:14p $

'$$Modtime: 7/06/20 3:59p $
'$$Revision: 37 $

'$$Workfile: utilization_functions.vb $
'
' ********************************************************************************




<System.Serializable()> Public Class utilization_functions

    Private aError As String
    Private clientConnectString As String

    Private adminConnectString As String

    Private starConnectString As String
    Private cloudConnectString As String
    Private serverConnectString As String

    Private taskerConnectString As String
    Private crmMasterConnectString As String
    Public Aircraft_IDS_String As String = ""
    Public exclude_Aircraft As Boolean = False
    Public Airport_ID_OVERALL As Integer
    Public Airport_IDS_String As String
    Public Operator_IDS_String As String
    Public rollup_text As String = ""
    Public use_operator As Boolean = False
    Public use_owner As Boolean = False
    Public use_insight_manu As Boolean = False
    Public use_insight_dealer As Boolean = False
    Public exclude_check As Boolean = False

    Public exclude_airport_check As Boolean = False
    Public airport_direction As String = "D"   ' d is for destination, which means origin will be O
    Public distance_string As String = "D"
    Dim comp_functions As New CompanyFunctions
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



#Region "util_functions"

    Public Function GetAircraft_Ownership_Function(ByVal ac_id As Long) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim temp_date As String = ""
        Dim temp_date2 As String = ""
        Dim end_month As String = ""
        Dim start_month As String = ""
        Dim start_month_back As String = ""
        Dim end_month_back As String = ""

        Try


            sQuery.Append(" select * from ReturnAircraftOwnershipbyAircraft(" & ac_id & ") ")
            sQuery.Append(" order by purchased_date desc, journ_id desc ")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />" & Date.Now & " - get_flight_profile(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
                aError = "Error in GetAircraft_Ownership_Function load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in GetAircraft_Ownership_Function As DataTable " + ex.Message

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

    Public Function get_company_purchase_history(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal roll_up As String) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim temp_date As String = ""
        Dim temp_date2 As String = ""
        Dim end_month As String = ""
        Dim start_month As String = ""
        Dim start_month_back As String = ""
        Dim end_month_back As String = ""

        Try


            sQuery.Append("	select distinct YEAR(purchased_date) as tyear, COUNT(*) as tcount  ")
            sQuery.Append("	from ReturnAircraftOwnershipbyCompany(" & searchCriteria.ViewCriteriaCompanyID & ",'" & roll_up & "') ")

            sQuery.Append(" where purchased_date is not null ") ' added in so there is a where

            If searchCriteria.ViewCriteriaAmodID > 0 Then
                sQuery.Append("	and amod_id = " & searchCriteria.ViewCriteriaAmodID & "")
            End If

            If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
                sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))
            Else
                sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True))
            End If



            sQuery.Append("	group by YEAR(purchased_date) ")
            sQuery.Append("	order by YEAR(purchased_date) ")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />" & Date.Now & " - get_flight_profile(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
                aError = "Error in get_flight_profile load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_flight_profile(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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
    Public Function get_flight_profile(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal type_of As String, ByVal go_back_farther As Boolean, ByVal use_faa_date As String, ByVal product_code_selection As String) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim temp_date As String = ""
        Dim temp_date2 As String = ""
        Dim end_month As String = ""
        Dim start_month As String = ""
        Dim start_month_back As String = ""
        Dim end_month_back As String = ""
        Dim sqlWhere As String = ""
        Try

            '-- ***********************************************************************
            '-- BY AIRPORT

            '-- *******************  UPPER RIGHT TAB 1 - GENERAL ************************
            '-- AIRPORT FLIGHT PROFILE - DISPLAY THE NUMBER OF FLIGHTS PER MONTH FOR THE AIRPORT
            If Trim(type_of) = "Month" Then
                sQuery.Append(" SELECT distinct year(ffd_date) as tyear, month(ffd_date) as tmonth, count(*) as tcount  ")
            ElseIf Trim(type_of) = "BWeight" Or Trim(type_of) = "TWeight" Or Trim(type_of) = "HWeight" Then
                sQuery.Append(" SELECT DISTINCT acwgtcls_name as type_name, count(distinct ffd_unique_flight_id) AS tflights ")
            ElseIf Trim(type_of) = "Type" Then
                'sQuery.Append(" SELECT DISTINCT atype_name as type_name, count(distinct ffd_unique_flight_id) AS tflights ")
                sQuery.Append("SELECT DISTINCT (case when amod_airframe_type_code = 'F' then atype_name else 'Helicopter ' + atype_name end) as type_name, count(distinct ffd_unique_flight_id) AS tflights ")
            ElseIf Trim(type_of) = "Hours" Then
                sQuery.Append(" SELECT distinct year(ffd_date) as tyear, month(ffd_date) as tmonth,   (SUM(ffd_flight_time)/60) as tcount ")
            End If

            sQuery.Append(" FROM VIEW_FLIGHTS WITH(NOLOCK)    ")
            'sQuery.Append(" FROM FAA_Flight_Data WITH(NOLOCK)  ")

            'If searchCriteria.ViewCriteriaCompanyID > 0 Or Trim(Operator_IDS_String) <> "" Then
            '  sQuery.Append(" INNER JOIN view_aircraft_company_flat WITH(NOLOCK) ON (ac_id = ffd_ac_id) AND ac_journ_id = 0 and  cref_operator_flag in ('Y', 'O') ")
            'Else
            '  sQuery.Append(" INNER JOIN aircraft WITH(NOLOCK) ON (ac_id = ffd_ac_id) AND ac_journ_id = 0 ")
            '  sQuery.Append(" INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id  ")
            'End If




            If Trim(type_of) = "Month" Then

            ElseIf Trim(type_of) = "BWeight" Or Trim(type_of) = "TWeight" Or Trim(type_of) = "HWeight" Then
                sQuery.Append(" inner join Aircraft_Weight_Class WITH(NOLOCK)on amod_type_code=acwgtcls_maketype and amod_weight_class=acwgtcls_code ")
            ElseIf searchCriteria.ViewCriteriaCompanyID > 0 Or Trim(Operator_IDS_String) <> "" Then
                ' dont join type 
            ElseIf Trim(type_of) = "Type" Then
                sQuery.Append(" INNER JOIN Aircraft_Type WITH(NOLOCK) on amod_type_code=atype_code ")
            End If

            sQuery.Append(" WHERE ") ' ffd_hide_flag= 'N' ")

            If String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentsStartDate) And String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentsEndDate) Then
                If Trim(use_faa_date) = "" Then
                    temp_date = DateAdd(DateInterval.Year, 0, Date.Now)
                Else
                    temp_date = DateAdd(DateInterval.Year, 0, CDate(use_faa_date))
                End If

                Call get_past_dates(temp_date, start_month, start_month_back, end_month, end_month_back)

                If go_back_farther = True Then
                    sQuery.Append(" convert(date, ffd_date, 0) >= ('" & start_month_back & "') ")
                    sQuery.Append(" and convert(date, ffd_date, 0) <= ('" & DateAdd(DateInterval.Day, 1, CDate(end_month_back)) & "') ")
                Else
                    sQuery.Append(" convert(date, ffd_date, 0) >= ('" & start_month & "') ")
                    sQuery.Append(" and convert(date, ffd_date, 0) <= ('" & end_month & "') ")
                End If
            Else
                sQuery.Append(" convert(date, ffd_date, 0) >= ('" & searchCriteria.ViewCriteriaDocumentsStartDate & "') ")
                sQuery.Append(" and convert(date, ffd_date, 0) <= ('" & DateAdd(DateInterval.Day, 0, CDate(searchCriteria.ViewCriteriaDocumentsEndDate)) & "') ")
            End If

            If Trim(Operator_IDS_String) <> "" Then
                If exclude_check = True Then
                    sQuery.Append(" and comp_id not ")
                Else
                    sQuery.Append(" and comp_id ")
                End If
                sQuery.Append(" in (" & Operator_IDS_String & ") ")
            End If


            If Trim(searchCriteria.ViewCriteriaContinent) = "International" Then
                sQuery.Append(" and (origin_aport_country not in ('United States', 'U.S.') or dest_aport_country not in ('United States', 'U.S.'))  ")
            End If

            If searchCriteria.ViewCriteriaCompanyID > 0 And searchCriteria.ViewCriteriaAmodID > 0 Then
                sQuery.Append(" AND amod_id = " & searchCriteria.ViewCriteriaAmodID & "")
            ElseIf Not IsNothing(searchCriteria.ViewCriteriaAmodIDArray) Then
                If searchCriteria.ViewCriteriaCompanyID > 0 Then
                    sQuery.Append(SetUpModelString(searchCriteria))
                Else
                    sQuery.Append(SetUpModelString(searchCriteria))
                End If
            ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
                sQuery.Append(SetUpMakeString(searchCriteria))
            End If

            If Not IsNothing(searchCriteria.ViewCriteriaTypeIDArray) Then
                If searchCriteria.ViewCriteriaCompanyID > 0 Then
                    sQuery.Append(SetUpTypeString(searchCriteria))
                Else
                    sQuery.Append(SetUpTypeString(searchCriteria))
                End If
            End If

            If Trim(rollup_text) <> "" Then
                sQuery.Append(rollup_text)
            ElseIf searchCriteria.ViewCriteriaCompanyID > 0 Then
                sQuery.Append(" AND comp_id = " & searchCriteria.ViewCriteriaCompanyID & "")
            End If

            If searchCriteria.ViewCriteriaAmodID > 0 Then
                sQuery.Append(" AND amod_id = " & searchCriteria.ViewCriteriaAmodID & "")
            End If


            If Trim(distance_string) <> "" And Len(Trim(distance_string)) > 2 Then
                sQuery.Append(distance_string)
            End If

            If Not String.IsNullOrEmpty(Trim(Aircraft_IDS_String)) Then
                If exclude_Aircraft = True Then
                    sQuery.Append(" and ac_id not ")
                Else
                    sQuery.Append(" and ac_id ")
                End If
                sQuery.Append(" in (" & Aircraft_IDS_String & ") ")
            End If

            'If go_back_farther = True Then
            '  ' go a year back from temp date 
            '  If Month(CDate(temp_date)) = 12 Then 
            '    sQuery.Append(" and ffd_date <= ('" & "1/01/" & (Year(CDate(temp_date)) - 1) & "') ")
            '  Else
            '    start_month = Month(CDate(temp_date)) & "/01/" & Year(CDate(temp_date))
            '  End If

            '  end_month = Month(CDate(temp_date)) & "/01/" & Year(CDate(temp_date))



            '  If Month(temp_date) = 12 Then
            '    temp_date = "01/01/" & Year(temp_date) 'take first of 1/1/ year and no need to go back
            '    sQuery.Append(" and ffd_date >= ('" & temp_date & "') ")
            '  Else
            '    temp_date = (Month(temp_date) + 1) & "/01/" & Year(temp_date) 'take first of last month
            '    temp_date2 = DateAdd(DateInterval.Year, -1, CDate(temp_date))
            '    sQuery.Append(" and ffd_date >= ('" & temp_date2 & "') ")
            '  End If 

            'Else
            '  If Month(temp_date) = 12 Then
            '    temp_date = "12/01/" & Year(temp_date) 'take first of 1/1/ year and no need to go back 
            '    sQuery.Append(" and ffd_date >= ('" & temp_date & "') ")
            '  Else
            '    temp_date = (Month(temp_date) + 1) & "/01/" & Year(temp_date)
            '    sQuery.Append(" and ffd_date >= ('" & temp_date & "') ")
            '  End If
            'End If


            sQuery.Append(" " & commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))


            Dim temp_use As String = " "
            If searchCriteria.ViewCriteriaHasBusinessFlag = True Or searchCriteria.ViewCriteriaHasCommercialFlag = True Or searchCriteria.ViewCriteriaHasHelicopterFlag = True Then
                sQuery.Append(" and ( ")
            End If

            If searchCriteria.ViewCriteriaHasBusinessFlag = True Then
                If Trim(temp_use) = "" Then
                    temp_use = " or "
                Else
                    sQuery.Append(temp_use)
                End If
                sQuery.Append(" ac_product_business_flag = 'Y' ")
            End If

            If searchCriteria.ViewCriteriaHasCommercialFlag = True Then
                If Trim(temp_use) = "" Then
                    temp_use = " or "
                Else
                    sQuery.Append(temp_use)
                End If
                sQuery.Append(" ac_product_commercial_flag = 'Y' ")
            End If

            If searchCriteria.ViewCriteriaHasHelicopterFlag = True Then
                If Trim(temp_use) = "" Then
                    temp_use = " or "
                Else
                    sQuery.Append(temp_use)
                End If
                sQuery.Append(" ac_product_helicopter_flag = 'Y' ")
            End If

            If searchCriteria.ViewCriteriaHasBusinessFlag = True Or searchCriteria.ViewCriteriaHasCommercialFlag = True Or searchCriteria.ViewCriteriaHasHelicopterFlag = True Then
                sQuery.Append(" ) ")
            End If


            If Trim(product_code_selection) <> "" Then
                sQuery.Append(product_code_selection)
            End If



            If Trim(airport_direction) = "D" Then
                sQuery.Append(" and ffd_origin_aport_id > 0")
                ' sQuery.Append(" AND (ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportIATA & "' or ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportICAO & "')")

                If Airport_ID_OVERALL > 1 Then
                    sQuery.Append(" AND (ffd_dest_aport_id = '" & Airport_ID_OVERALL & "')")
                ElseIf Not String.IsNullOrEmpty(Airport_IDS_String) Then
                    If exclude_airport_check Then
                        sQuery.Append(" AND (ffd_dest_aport_id not in (" & Airport_IDS_String & "))")
                    Else
                        sQuery.Append(" AND (ffd_dest_aport_id in (" & Airport_IDS_String & "))")
                    End If

                End If
            ElseIf Trim(airport_direction) = "O" Then
                sQuery.Append(" and ffd_dest_aport_id > 0")
                ' sQuery.Append(" AND (ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportIATA & "' or ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportICAO & "')")

                If Airport_ID_OVERALL > 1 Then
                    sQuery.Append(" AND (ffd_origin_aport_id = '" & Airport_ID_OVERALL & "')")
                ElseIf Not String.IsNullOrEmpty(Airport_IDS_String) Then
                    If exclude_airport_check Then
                        sQuery.Append(" AND (ffd_origin_aport_id not in (" & Airport_IDS_String & "))")
                    Else
                        sQuery.Append(" AND (ffd_origin_aport_id in (" & Airport_IDS_String & "))")
                    End If

                End If
            ElseIf Trim(airport_direction) = "X" Then
                '  sQuery.Append(" and ffd_dest_aport_id > 0")
                ' sQuery.Append(" AND (ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportIATA & "' or ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportICAO & "')")

                If Airport_ID_OVERALL > 1 Then
                    sQuery.Append(" AND ((ffd_origin_aport_id = '" & Airport_ID_OVERALL & "') or (ffd_dest_aport_id = '" & Airport_ID_OVERALL & "')) ")
                ElseIf Not String.IsNullOrEmpty(Airport_IDS_String) Then
                    If exclude_airport_check Then
                        sQuery.Append(" AND (ffd_origin_aport_id not in (" & Airport_IDS_String & ")) AND (ffd_dest_aport_id not in (" & Airport_IDS_String & "))   ")
                    Else
                        sQuery.Append(" AND (   (ffd_origin_aport_id in (" & Airport_IDS_String & "))  or (ffd_dest_aport_id in (" & Airport_IDS_String & "))   )")
                    End If

                End If

            Else
                sQuery.Append(" and ffd_origin_aport_id > 0")
                ' sQuery.Append(" AND (ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportIATA & "' or ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportICAO & "')")

                If Airport_ID_OVERALL > 1 Then
                    sQuery.Append(" AND (ffd_dest_aport_id = '" & Airport_ID_OVERALL & "')")
                ElseIf Not String.IsNullOrEmpty(Airport_IDS_String) Then
                    If exclude_airport_check Then
                        sQuery.Append(" AND (ffd_dest_aport_id not in (" & Airport_IDS_String & "))")
                    Else
                        sQuery.Append(" AND (ffd_dest_aport_id in (" & Airport_IDS_String & "))")
                    End If

                End If
            End If


            ' ADDED MSW - 3/10/20 TO BUILD IN DROP DOWN FOR IN OPERATION 
            If Not IsNothing(searchCriteria.viewCriteriaInOperation) Then
                If Trim(searchCriteria.viewCriteriaInOperation) <> "" Then
                    sQuery.Append(Build_In_Operation_String(searchCriteria))
                End If
            End If


            If Trim(type_of) = "BWeight" Then
                sQuery.Append(" AND amod_type_code ='J'and amod_airframe_type_code = 'F'  ")
            ElseIf Trim(type_of) = "TWeight" Then
                sQuery.Append(" AND amod_type_code ='T'and amod_airframe_type_code = 'F' ")
            ElseIf Trim(type_of) = "HWeight" Then
                sQuery.Append(" AND amod_airframe_type_code = 'R' ")
            End If

            'If Trim(product_code_selection) <> "" Then
            '  sQuery.Append(product_code_selection)
            'End If




            If Trim(type_of) = "Month" Then
                sQuery.Append(" group by year(ffd_date), month(ffd_date) ")
                sQuery.Append(" ORDER BY year(ffd_date), month(ffd_date) ")
            ElseIf Trim(type_of) = "BWeight" Or Trim(type_of) = "TWeight" Or Trim(type_of) = "HWeight" Then
                sQuery.Append(" group by acwgtcls_name ")
                sQuery.Append(" order by COUNT(distinct ffd_unique_flight_id) desc ")
            ElseIf Trim(type_of) = "Type" Then
                sQuery.Append(" group by amod_airframe_type_code, atype_name ")
                sQuery.Append(" order by COUNT(distinct ffd_unique_flight_id) desc ")
            ElseIf Trim(type_of) = "Hours" Then
                sQuery.Append(" group by year(ffd_date), month(ffd_date) ")
                sQuery.Append(" ORDER BY year(ffd_date), month(ffd_date) ")
            End If



            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "utilization_functions.vb", sQuery.ToString.ToString)

            'HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />" & Date.Now & " - get_flight_profile(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

            SqlConn.ConnectionString = clientConnectString
            SqlConn.Open()
            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 120

            SqlCommand.CommandText = sQuery.ToString
            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                aError = "Error in get_flight_profile load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_flight_profile(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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
    Public Function SetUpModelString(ByRef searchCriteria As viewSelectionCriteriaClass, Optional ByVal use_ac_model_table As Boolean = False) As String
        Dim TempModelList As String = ""
        For Each model In searchCriteria.ViewCriteriaAmodIDArray
            If TempModelList <> "" Then
                TempModelList += ", "
            End If
            TempModelList += model.ToString
        Next

        If TempModelList <> "" Then
            If use_ac_model_table = True Then
                TempModelList = " and aircraft_model.amod_id in (" & TempModelList & ") "
            Else
                TempModelList = " and amod_id in (" & TempModelList & ") "
            End If
        End If
        Return TempModelList
    End Function

    Public Function SetUpMakeString(ByRef searchCriteria As viewSelectionCriteriaClass, Optional ByVal use_ac_model_table As Boolean = False) As String
        Dim TempModelList As String = ""
        Dim splitMake As String() = Split(searchCriteria.ViewCriteriaAircraftMake, ",")
        For Each model In splitMake
            If Not String.IsNullOrEmpty(model) Then
                If TempModelList <> "" Then
                    TempModelList += ", "
                End If
                TempModelList += "'" & model.ToString & "'"
            End If
        Next

        If TempModelList <> "" Then
            If use_ac_model_table = True Then
                TempModelList = " and aircraft_model.amod_make_name in (" & TempModelList & ") "
            Else
                TempModelList = " and amod_make_name in (" & TempModelList & ") "
            End If

        End If
        Return TempModelList
    End Function

    Public Function Build_In_Operation_String(ByRef searchCriteria As viewSelectionCriteriaClass) As String

        Build_In_Operation_String = ""

        If searchCriteria.viewCriteriaInOperation <> "" Then
            If searchCriteria.viewCriteriaInOperation = "Y" Then
                Build_In_Operation_String = " and ac_lifecycle_stage = 3 "
            ElseIf searchCriteria.viewCriteriaInOperation = "N" Then
                Build_In_Operation_String = " and ac_lifecycle_stage <> 3 "
            End If
        End If


    End Function

    Public Function SetUpTypeString(ByRef searchCriteria As viewSelectionCriteriaClass, Optional ByVal use_ac_model_table As Boolean = False) As String
        Dim TempModelList As String = ""
        'localCriteria.ViewCriteriaAircraftType & "|" & localCriteria.ViewCriteriaAirframeTypeStr
        If Not IsNothing(searchCriteria.ViewCriteriaTypeIDArray) Then
            For Each ty In searchCriteria.ViewCriteriaTypeIDArray
                Dim tempSpl As String() = Split(ty, "|")
                If UBound(tempSpl) = 1 Then
                    If TempModelList <> "" Then
                        TempModelList += " or "
                    End If
                    'amod_type_code in ('P') and amod_airframe_type_code
                    If use_ac_model_table = True Then
                        TempModelList += " ( aircraft_model.amod_type_code = '" & tempSpl(0).ToString & "' and aircraft_model.amod_airframe_type_code ='" & tempSpl(1).ToString & "' ) "
                    Else
                        TempModelList += " ( amod_type_code = '" & tempSpl(0).ToString & "' and amod_airframe_type_code ='" & tempSpl(1).ToString & "' ) "
                    End If
                End If
            Next

            If TempModelList <> "" Then
                TempModelList = " and (" & TempModelList & ") "
            End If
        End If

        Return TempModelList
    End Function

    Public Sub get_past_dates(ByVal temp_date As String, ByRef start_month As String, ByRef start_month_back As String, ByRef end_month As String, ByRef end_month_back As String)
        ' 11/24/16 -- 12/5/2016
        temp_date = Month(CDate(temp_date)) & "/01/" & Year(CDate(temp_date))  ' 11/1/2016--12/1/2016
        If Month(CDate(temp_date)) = 12 Then
            '--start with 12/1/2016
            start_month = DateAdd(DateInterval.Month, 1, CDate(temp_date)) ' -- 1/1/2017
            start_month = DateAdd(DateInterval.Year, -1, CDate(start_month)) ' -- 1/1/2016
            start_month_back = DateAdd(DateInterval.Year, -1, CDate(start_month)) '--1/1/2015

            end_month = DateAdd(DateInterval.Month, 1, CDate(temp_date)) '--1/1/2017 
            end_month = DateAdd(DateInterval.Day, -1, CDate(end_month)) '--12/31/2016
            end_month_back = DateAdd(DateInterval.Year, -1, CDate(end_month)) '--12/31/2015
        Else
            '--start with 11/1/2016
            start_month = DateAdd(DateInterval.Month, 1, CDate(temp_date))  ' 12/1/2016--
            start_month = DateAdd(DateInterval.Year, -1, CDate(start_month)) ' 12/1/2015--
            start_month_back = DateAdd(DateInterval.Year, -1, CDate(start_month)) '12/1/2014--

            end_month = DateAdd(DateInterval.Month, 1, CDate(temp_date)) '12/1/2016
            end_month = DateAdd(DateInterval.Day, -1, CDate(end_month)) ' 11/31/2016
            end_month_back = DateAdd(DateInterval.Year, -1, CDate(end_month)) ' 11/31/2015
        End If
    End Sub
    Public Function get_flight_activity_overall(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal product_code_selection As String) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim temp_date As String = ""

        Try

            '-- # FLIGHT ACTIVITY OVERALL

            sQuery.Append(" SELECT DISTINCT count(*) as tflights, SUM(convert(decimal(18,4),ffd_flight_time))/60 as TotalFlightTimeHrs, SUM((ffd_flight_time* amod_fuel_burn_rate)/60) as TotalFuelBurn ")
            'sQuery.Append(" FROM FAA_Flight_Data WITH(NOLOCK)  ")

            Call build_flight_data_from(sQuery)
            'If searchCriteria.ViewCriteriaCompanyID > 0 Or Trim(Operator_IDS_String) <> "" Then
            '  sQuery.Append(" INNER JOIN view_aircraft_company_flat WITH(NOLOCK) ON (ac_id = ffd_ac_id) AND ac_journ_id = 0 and  cref_operator_flag  in ('Y', 'O') ")
            'Else
            '  sQuery.Append(" INNER JOIN aircraft WITH(NOLOCK) ON (ac_id = ffd_ac_id) AND ac_journ_id = 0 ")
            '  sQuery.Append(" INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id  ")
            'End If

            'If Trim(airport_direction) = "D" Then
            '  sQuery.Append(" INNER JOIN Airport WITH(NOLOCK) ON ffd_dest_aport_id = aport_id where ")
            'ElseIf Trim(airport_direction) = "O" Then
            '  sQuery.Append(" INNER JOIN Airport WITH(NOLOCK) ON ffd_origin_aport_id = aport_id where ")
            'Else
            '  sQuery.Append(" INNER JOIN Airport WITH(NOLOCK) ON ffd_origin_aport_id = aport_id where ")
            'End If


            sQuery.Append(" where ")

            temp_date = DateAdd(DateInterval.Month, -searchCriteria.ViewCriteriaTimeSpan, Date.Now)
            temp_date = Month(temp_date) & "/01/" & Year(temp_date)

            If String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentsStartDate) And String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentsEndDate) Then
                sQuery.Append("  ffd_date >= ('" & temp_date & "')  ")
            Else
                sQuery.Append(" ( convert(date, ffd_date, 0) >= '" & Month(searchCriteria.ViewCriteriaDocumentsStartDate) & "/" & Day(searchCriteria.ViewCriteriaDocumentsStartDate) & "/" & Year(searchCriteria.ViewCriteriaDocumentsStartDate) & "') ")
                sQuery.Append(" AND (convert(date, ffd_date, 0) <= '" & Month(searchCriteria.ViewCriteriaDocumentsEndDate) & "/" & Day(searchCriteria.ViewCriteriaDocumentsEndDate) & "/" & Year(searchCriteria.ViewCriteriaDocumentsEndDate) & "') ")
            End If

            'sQuery.Append("  and ffd_hide_flag= 'N'  ")


            If searchCriteria.ViewCriteriaCompanyID > 0 Then
                sQuery.Append(" AND comp_id = " & searchCriteria.ViewCriteriaCompanyID & "")
            End If

            If searchCriteria.ViewCriteriaAmodID >= 1 Then
                sQuery.Append(" and amod_id = " & searchCriteria.ViewCriteriaAmodID.ToString)
            ElseIf Not IsNothing(searchCriteria.ViewCriteriaAmodIDArray) Then
                sQuery.Append(SetUpModelString(searchCriteria))
            ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
                sQuery.Append(SetUpMakeString(searchCriteria))
            End If

            If Not IsNothing(searchCriteria.ViewCriteriaTypeIDArray) Then
                sQuery.Append(SetUpTypeString(searchCriteria))
            End If

            If Trim(Operator_IDS_String) <> "" Then
                If exclude_check = True Then
                    sQuery.Append(" and comp_id not ")
                Else
                    sQuery.Append(" and comp_id ")
                End If
                sQuery.Append(" in (" & Operator_IDS_String & ") ")
            End If

            If Not String.IsNullOrEmpty(Trim(Aircraft_IDS_String)) Then
                If exclude_Aircraft = True Then
                    sQuery.Append(" and ac_id not ")
                Else
                    sQuery.Append(" and ac_id ")
                End If
                sQuery.Append(" in (" & Aircraft_IDS_String & ") ")
            End If


            If Trim(airport_direction) = "D" Then
                ' sQuery.Append(" AND (ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportIATA & "' or ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportICAO & "') ")
                If Airport_ID_OVERALL > 1 Then
                    sQuery.Append(" AND (ffd_dest_aport_id = '" & Airport_ID_OVERALL & "') ")
                ElseIf Not String.IsNullOrEmpty(Airport_IDS_String) Then
                    If exclude_airport_check Then
                        sQuery.Append(" AND (ffd_dest_aport_id not in (" & Airport_IDS_String & ")) ")
                    Else
                        sQuery.Append(" AND (ffd_dest_aport_id in (" & Airport_IDS_String & ")) ")
                    End If

                End If
            ElseIf Trim(airport_direction) = "O" Then
                ' sQuery.Append(" AND (ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportIATA & "' or ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportICAO & "') ")
                If Airport_ID_OVERALL > 1 Then
                    sQuery.Append(" AND (ffd_origin_aport_id = '" & Airport_ID_OVERALL & "') ")
                ElseIf Not String.IsNullOrEmpty(Airport_IDS_String) Then
                    If exclude_airport_check Then
                        sQuery.Append(" AND (ffd_origin_aport_id not in (" & Airport_IDS_String & ")) ")
                    Else
                        sQuery.Append(" AND (ffd_origin_aport_id in (" & Airport_IDS_String & ")) ")
                    End If

                End If

            ElseIf Trim(airport_direction) = "X" Then
                '  sQuery.Append(" and ffd_dest_aport_id > 0")
                ' sQuery.Append(" AND (ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportIATA & "' or ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportICAO & "')")

                If Airport_ID_OVERALL > 1 Then
                    sQuery.Append(" AND ((ffd_origin_aport_id = '" & Airport_ID_OVERALL & "') or (ffd_dest_aport_id = '" & Airport_ID_OVERALL & "')) ")
                ElseIf Not String.IsNullOrEmpty(Airport_IDS_String) Then
                    If exclude_airport_check Then
                        sQuery.Append(" AND (ffd_origin_aport_id not in (" & Airport_IDS_String & ")) AND (ffd_dest_aport_id not in (" & Airport_IDS_String & "))   ")
                    Else
                        sQuery.Append(" AND (   (ffd_origin_aport_id in (" & Airport_IDS_String & "))  or (ffd_dest_aport_id in (" & Airport_IDS_String & "))   )")
                    End If

                End If


            Else
                ' sQuery.Append(" AND (ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportIATA & "' or ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportICAO & "') ")
                If Airport_ID_OVERALL > 1 Then
                    sQuery.Append(" AND (ffd_dest_aport_id = '" & Airport_ID_OVERALL & "') ")
                ElseIf Not String.IsNullOrEmpty(Airport_IDS_String) Then
                    If exclude_airport_check Then
                        sQuery.Append(" AND (ffd_dest_aport_id not in (" & Airport_IDS_String & ")) ")
                    Else
                        sQuery.Append(" AND (ffd_dest_aport_id in (" & Airport_IDS_String & ")) ")
                    End If

                End If
            End If

            ' ADDED MSW - 3/10/20 TO BUILD IN DROP DOWN FOR IN OPERATION 
            If Not IsNothing(searchCriteria.viewCriteriaInOperation) Then
                If Trim(searchCriteria.viewCriteriaInOperation) <> "" Then
                    sQuery.Append(Build_In_Operation_String(searchCriteria))
                End If
            End If



            If Trim(distance_string) <> "" And Len(Trim(distance_string)) > 2 Then
                sQuery.Append(distance_string)
            End If


            If searchCriteria.ViewCriteriaCompanyID > 0 Or Trim(Operator_IDS_String) <> "" Then
                sQuery.Append(" " & commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))
            Else
                sQuery.Append(" " & commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))
            End If

            If Trim(product_code_selection) <> "" Then
                sQuery.Append(product_code_selection)
            End If

            ' sQuery.Append(" having(COUNT(ffd_unique_flight_id) > 1) ")
            '  sQuery.Append(" AND ((amod_type_code IN ('T','P') AND amod_customer_flag = 'Y'  ")
            ' sQuery.Append(" AND amod_airframe_type_code = 'R' AND amod_product_helicopter_flag = 'Y')  ")
            ' sQuery.Append(" OR (amod_customer_flag = 'Y' AND amod_airframe_type_code = 'F'  ")
            ' sQuery.Append(" AND amod_product_business_flag = 'Y') OR (amod_customer_flag = 'Y'  ")
            ' sQuery.Append(" AND amod_airframe_type_code = 'F' AND amod_product_commercial_flag = 'Y')) ")


            ' If searchCriteria.ViewCriteriaAmodID > -1 Then
            'sQuery.Append(crmWebClient.Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
            '  ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
            '  sQuery.Append(crmWebClient.Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
            '  End If

            '  sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "utilization_functions.vb", sQuery.ToString.ToString)

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
                aError = "Error in get_flight_activity_overall load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_flight_activity_overall(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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
    Public Function get_flight_activity_last(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim temp_date As String = ""

        Try

            '-- # FLIGHT ACTIVITY OVERALL
            sQuery.Append(" SELECT top 1 ffd_date ")
            sQuery.Append(" FROM View_FAA_Flight_Data_Clean WITH(NOLOCK)  ")
            sQuery.Append(" INNER JOIN Airport WITH(NOLOCK) ON ffd_origin_aport_id = aport_id  ")
            sQuery.Append(" WHERE  ffd_hide_flag= 'N' order by ffd_date desc ")

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "utilization_functions.vb", sQuery.ToString.ToString)

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
                aError = "Error in get_flight_activity_last load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_flight_activity_last(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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
    Public Function RunActiveFolderCompanyID(ByVal queryToRun As String) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim temp_date As String = ""

        Try

            If Not String.IsNullOrEmpty(queryToRun) Then
                sQuery.Append(queryToRun)

                clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "utilization_functions.vb", sQuery.ToString)


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
                    aError = "Error in RunActiveFolderCompanyID(ByVal queryToRun As String) load datatable " + constrExc.Message
                End Try
            End If
        Catch ex As Exception
            Return Nothing

            aError = "Error in RunActiveFolderCompanyID(ByVal queryToRun As String) " + ex.Message

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

    Public Function get_data_from_client_folder(ByVal client_folder_id As Long, ByVal cfolder_field_name As String) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim temp_date As String = ""

        Try

            '-- # FLIGHT ACTIVITY OVERALL
            sQuery.Append(" select distinct " & cfolder_field_name & " from Client_Folder_Index with (NOLOCK) ")
            sQuery.Append(" where cfoldind_cfolder_id = " & client_folder_id & " ")

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "utilization_functions.vb", sQuery.ToString.ToString)

            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
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
                aError = "Error in get_data_from_client_folder load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_data_from_client_folder(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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
    Public Function get_all_client_folder(ByVal cfolder_field_type As String) As DataTable



        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim temp_date As String = ""

        Try

            '-- # FLIGHT ACTIVITY OVERALL
            sQuery.Append(" select distinct cfolder_name, cfolder_id, cfolder_operator_flag, cfolder_method ")
            sQuery.Append(" from Client_Folder with (NOLOCK)  ")
            sQuery.Append(" inner join Client_Folder_Type with (NOLOCK) on cfolder_cftype_id = cftype_id  ")
            sQuery.Append(" LEFT OUTER JOIN Subscription_Install with (NOLOCK) on subins_sub_id = cfolder_sub_id and subins_login=cfolder_login and subins_seq_no = cfolder_seq_no  ")
            sQuery.Append(" inner join Subscription with (NOLOCK) on cfolder_sub_id=sub_id  ")

            ' sQuery.Append(" where (( cfolder_sub_id = 777 and cfolder_login = 'mvintech' and cfolder_seq_no = 1) or (sub_comp_id = 135887 and cfolder_share='Y'  ")
            ' sQuery.Append(" and sub_share_by_comp_id_flag = 'Y'))  ")

            ' added MSW - 9/19/18 so that demo accounts dont see shared folders 
            If HttpContext.Current.Session.Item("localUser").crmDemoUserFlag = True Then
                sQuery.Append(" where ( cfolder_sub_id = " & HttpContext.Current.Session.Item("localUser").crmSubSubID & " and cfolder_login = '" & HttpContext.Current.Session.Item("localUser").crmUserLogin & "' and cfolder_seq_no = " & HttpContext.Current.Session.Item("localUser").crmSubSeqNo & ")  ")
            Else
                sQuery.Append(" where (( cfolder_sub_id = " & HttpContext.Current.Session.Item("localUser").crmSubSubID & " and cfolder_login = '" & HttpContext.Current.Session.Item("localUser").crmUserLogin & "' and cfolder_seq_no = " & HttpContext.Current.Session.Item("localUser").crmSubSeqNo & ") or (sub_comp_id = " & HttpContext.Current.Session.Item("localUser").crmUserCompanyID & " and cfolder_share='Y'  ")
                sQuery.Append(" and sub_share_by_comp_id_flag = 'Y') or (cfolder_sub_id = " & HttpContext.Current.Session.Item("localUser").crmSubSubID & " and cfolder_share='Y')     )  ")
                ' ADDED IN THE LAST "OR" WITH CFOLDER AND SHARE, SO THAT IF ONLY ONE SUB IT SHARES AS IT SHOULD
            End If

            sQuery.Append(" and cfolder_hide_flag ='N' and not (cfolder_cftype_id = 3 and cfolder_jetnet_run_flag='Y') ")
            sQuery.Append(" and cfttpe_name = '" & cfolder_field_type & "' ")

            If LCase(cfolder_field_type) = "company" Then
                sQuery.Append(" and cfolder_operator_flag= 'Y' ")
            ElseIf LCase(cfolder_field_type) = "airport" Then
                sQuery.Append(" and cfolder_method='S' ")
            End If

            sQuery.Append(" order by  cfolder_name ")

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "utilization_functions.vb", sQuery.ToString.ToString)

            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
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
                aError = "Error in get_data_from_client_folder load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_data_from_client_folder(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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
    Public Function get_default_airport_id(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal comp_id As Long, ByVal get_by As String, ByVal temp_distance As Integer) As Long
        get_default_airport_id = 0

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim max_NORTH As Double = 0.0
        Dim max_SOUTH As Double = 0.0
        Dim max_WEST As Double = 0.0
        Dim max_EAST As Double = 0.0
        Dim query_distance As String = ""
        Dim orig_lat As Double = 0.0
        Dim orig_long As Double = 0.0


        Try

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />" & Date.Now & " - get_default_airport_id(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

            SqlConn.ConnectionString = clientConnectString
            SqlConn.Open()
            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 60




            If Trim(get_by) = "State" Then

                sQuery.Append(" select top 1 * from airport WITH(NOLOCK) ")
                sQuery.Append(" where aport_state in (select distinct comp_state from Company with (NOLOCK) ")
                sQuery.Append(" where comp_id = " & comp_id & " and comp_journ_id = 0 ")
                sQuery.Append("  and comp_country = aport_country ")
                sQuery.Append("  and ((comp_state = aport_state) or comp_state is null) ")
                sQuery.Append(" ) ")
                sQuery.Append(" and aport_active_flag='Y' ")
                sQuery.Append(" and aport_max_runway_length > 0 ")

            ElseIf Trim(get_by) = "Country" Then

                sQuery.Append(" select top 1 * from airport WITH(NOLOCK) ")
                sQuery.Append(" where aport_country in (select distinct comp_country from Company with (NOLOCK) ")
                sQuery.Append(" where comp_id = " & comp_id & " and comp_journ_id = 0 ")
                sQuery.Append("  and comp_country = aport_country ")
                sQuery.Append(" ) ")
                sQuery.Append(" and aport_active_flag='Y' ")
                sQuery.Append(" and aport_max_runway_length > 0 ")

            ElseIf Trim(get_by) = "Radius" Then


                sQuery.Append(" select distinct zmap_latitude, zmap_longitude ")
                sQuery.Append(" from Company with (NOLOCK)  ")
                sQuery.Append(" inner join Zip_Mapping with (NOLOCK) on left(comp_zip_code,5) = zmap_zip_code  ")
                sQuery.Append(" where comp_id = " & comp_id & " And comp_journ_id = 0 ")


                SqlCommand.CommandText = sQuery.ToString
                SqlReader = SqlCommand.ExecuteReader()

                Try
                    atemptable.Load(SqlReader)
                Catch constrExc As System.Data.ConstraintException
                    Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                    aError = "Error in get_fractional_shares load datatable " + constrExc.Message
                End Try
                SqlReader.Close()

                If Not IsNothing(atemptable) Then
                    If atemptable.Rows.Count > 0 Then
                        For Each r As DataRow In atemptable.Rows
                            orig_lat = r.Item("zmap_latitude")
                            orig_long = r.Item("zmap_longitude")
                        Next
                    End If
                End If

                ' then re-select using zmap
                sQuery.Length = 0 ' clear query 
                atemptable.Clear()
                atemptable.Constraints.Clear()
                sQuery.Append(" select top 1 * from airport WITH(NOLOCK) ")
                sQuery.Append(" where aport_latitude_decimal in (select distinct aport_latitude_decimal from Airport with (NOLOCK) ")
                sQuery.Append(" inner join Zip_Mapping with (NOLOCK) on aport_city = zmap_city and aport_country = zmap_country and ((aport_state = zmap_state) or aport_state is null) ")

                If temp_distance > 0 Then
                    query_distance = CDbl(temp_distance / 69).ToString 'http://www.distancebetweencities.net/ helped us 
                Else
                    query_distance = "2.1739"
                End If

                max_WEST = FormatNumber(orig_long + query_distance, 6)
                max_EAST = FormatNumber(orig_long - query_distance, 6)
                max_NORTH = FormatNumber(orig_lat + query_distance, 6)
                max_SOUTH = FormatNumber(orig_lat - query_distance, 6)


                sQuery.Append(" and (zmap_longitude <= " & max_WEST & " AND zmap_longitude >= " & max_EAST & ")  ")
                sQuery.Append(" AND (zmap_latitude <= " & max_NORTH & " AND zmap_latitude >= " & max_SOUTH & ")  ")
                sQuery.Append(" where aport_active_flag='Y' ")
                sQuery.Append(" and aport_max_runway_length > 0 ")
                sQuery.Append(" and CHARINDEX('0',aport_iata_code) = 0 ")
                sQuery.Append("and CHARINDEX('1',aport_iata_code) = 0 ")
                sQuery.Append("and CHARINDEX('2',aport_iata_code) = 0 ")
                sQuery.Append("and CHARINDEX('3',aport_iata_code) = 0 ")
                sQuery.Append("and CHARINDEX('4',aport_iata_code) = 0 ")
                sQuery.Append("and CHARINDEX('5',aport_iata_code) = 0 ")
                sQuery.Append("and CHARINDEX('6',aport_iata_code) = 0 ")
                sQuery.Append("and CHARINDEX('7',aport_iata_code) = 0 ")
                sQuery.Append("and CHARINDEX('8',aport_iata_code) = 0 ")
                sQuery.Append("and CHARINDEX('9',aport_iata_code) = 0 ")
                sQuery.Append(" ) ")

                sQuery.Append(" and aport_active_flag='Y' ")
                sQuery.Append(" and aport_max_runway_length > 0 ")
                sQuery.Append(" and CHARINDEX('0',aport_iata_code) = 0 ")
                sQuery.Append("and CHARINDEX('1',aport_iata_code) = 0 ")
                sQuery.Append("and CHARINDEX('2',aport_iata_code) = 0 ")
                sQuery.Append("and CHARINDEX('3',aport_iata_code) = 0 ")
                sQuery.Append("and CHARINDEX('4',aport_iata_code) = 0 ")
                sQuery.Append("and CHARINDEX('5',aport_iata_code) = 0 ")
                sQuery.Append("and CHARINDEX('6',aport_iata_code) = 0 ")
                sQuery.Append("and CHARINDEX('7',aport_iata_code) = 0 ")
                sQuery.Append("and CHARINDEX('8',aport_iata_code) = 0 ")
                sQuery.Append("and CHARINDEX('9',aport_iata_code) = 0 ")


            Else
                sQuery.Append(" select top 1 * from airport WITH(NOLOCK) ")
                sQuery.Append(" where aport_city in (select distinct comp_city from Company with (NOLOCK) ")
                sQuery.Append(" where comp_id = " & comp_id & " and comp_journ_id = 0 ")
                sQuery.Append("  and comp_country = aport_country ")
                sQuery.Append("  and ((comp_state = aport_state) or comp_state is null) ")
                sQuery.Append(" ) ")
                sQuery.Append(" and aport_active_flag='Y' ")
                sQuery.Append(" and aport_max_runway_length > 0 ")
            End If

            sQuery.Append(" and aport_iata_code <> '' and aport_icao_code <> ''")


            SqlCommand.CommandText = sQuery.ToString
            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)


            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                aError = "Error in get_fractional_shares load datatable " + constrExc.Message
            End Try

            If Not IsNothing(atemptable) Then
                If atemptable.Rows.Count > 0 Then
                    For Each r As DataRow In atemptable.Rows
                        get_default_airport_id = r.Item("aport_id")
                    Next
                End If
            End If


        Catch ex As Exception
            Return Nothing

            aError = "Error in get_fractional_shares(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing

            SqlCommand.Dispose()
            SqlCommand = Nothing
        End Try

    End Function

    ''' <summary>
    ''' Default Folder List queried by folder type
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DefaultFolderByType(ByVal type As String, ByVal subscriptionID As Long, ByVal login As String, ByVal seqNo As Long) As DataTable

        Dim sqlQuery As String = ""
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim atemptable As New DataTable

        Try
            'Opening Connection
            SqlConn.ConnectionString = clientConnectString
            SqlConn.Open()

            sqlQuery = " select top 1 * from Client_Folder with (NOLOCK) "
            sqlQuery += " inner join Client_Folder_Type with (NOLOCK) on cfolder_cftype_id = cftype_id "
            sqlQuery += " where cfttpe_name = @folderType "
            sqlQuery += " and cfolder_sub_id = @subID "
            sqlQuery += " and cfolder_login= @subLogin "
            sqlQuery += " and cfolder_seq_no= @seqNo "
            sqlQuery += " and cfolder_default_flag='Y' "

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "utilization_functions.vb", sqlQuery.ToString.ToString)

            Dim SqlCommand As New SqlClient.SqlCommand(sqlQuery, SqlConn)

            SqlCommand.Parameters.AddWithValue("folderType", type)
            SqlCommand.Parameters.AddWithValue("subID", subscriptionID)
            SqlCommand.Parameters.AddWithValue("subLogin", login)
            SqlCommand.Parameters.AddWithValue("seqNo", seqNo)

            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
            End Try

            DefaultFolderByType = atemptable

            SqlCommand.Dispose()
            SqlCommand = Nothing

        Catch ex As Exception
            DefaultFolderByType = Nothing
            Me.class_error = "Error in DefaultFolderByType(ByVal type As String, ByVal subscriptionID As Long, ByVal login As String, ByVal seqNo As Long) As DataTable: " & ex.Message
        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing


        End Try

    End Function
    Public Function get_most_common_destinations(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal product_code_selection As String) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            '-- ***************  UPPER RIGHT TAB 2 - TOP ORIGINS/DESTINATIONS ************************
            '-- # FLIGHT ACTIVITY -MOST COMMON ORIGINS - WE WOULD ALSO DO SAME FOR DESTINATIONS
            sQuery.Append(" select  DISTINCT  top 25 aport_iata_code as IATA, aport_icao_code as ICAO,ffd_origin_aport_id, ")
            sQuery.Append(" aport_name, aport_country, aport_city,aport_id, comp_name,  aport_state, count(*) AS tflights ")
            sQuery.Append(" FROM FAA_Flight_Data WITH(NOLOCK) ")

            'sQuery.Append(" INNER JOIN aircraft WITH(NOLOCK) ON (ac_id = ffd_ac_id) AND ac_journ_id = 0  ")
            ' sQuery.Append(" INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id  ") 
            If searchCriteria.ViewCriteriaCompanyID > 0 Then
                sQuery.Append(" INNER JOIN view_aircraft_company_flat WITH(NOLOCK) ON (ac_id = ffd_ac_id) AND ac_journ_id = 0 and  cref_operator_flag  in ('Y', 'O') ")
            Else
                sQuery.Append(" INNER JOIN aircraft_flat WITH(NOLOCK) ON (ac_id = ffd_ac_id) AND ac_journ_id = 0 ")
            End If

            sQuery.Append(" INNER JOIN Airport WITH(NOLOCK) ON ffd_dest_aport_id = aport_id ")
            sQuery.Append(" WHERE ffd_date >= (getdate()-" & (searchCriteria.ViewCriteriaTimeSpan * 30) & ")  ")


            sQuery.Append(" and ffd_hide_flag= 'N' and ffd_dest_aport <> '' ")
            'sQuery.Append(" AND (ffd_origin_aport = '" & searchCriteria.ViewCriteriaAirportIATA & "' or ffd_origin_aport = '" & searchCriteria.ViewCriteriaAirportICAO & "') ")


            If Airport_ID_OVERALL > 0 Then
                sQuery.Append(" AND (ffd_origin_aport_id = '" & Airport_ID_OVERALL & "') ")
            End If

            If Trim(product_code_selection) <> "" Then
                sQuery.Append(product_code_selection)
            End If

            sQuery.Append(" AND ((amod_type_code IN ('T','P') AND amod_customer_flag = 'Y'  ")
            sQuery.Append(" AND amod_airframe_type_code = 'R' AND amod_product_helicopter_flag = 'Y')  ")
            sQuery.Append(" OR (amod_customer_flag = 'Y' AND amod_airframe_type_code = 'F'  ")
            sQuery.Append(" AND amod_product_business_flag = 'Y') OR (amod_customer_flag = 'Y'  ")
            sQuery.Append(" AND amod_airframe_type_code = 'F' AND amod_product_commercial_flag = 'Y')) ")

            If Trim(distance_string) <> "" And Len(Trim(distance_string)) > 2 Then
                sQuery.Append(distance_string)
            End If

            If searchCriteria.ViewCriteriaCompanyID > 0 Then
                sQuery.Append(" AND comp_id = " & searchCriteria.ViewCriteriaCompanyID & "")
            End If

            If searchCriteria.ViewCriteriaAmodID > -1 Then
                sQuery.Append(crmWebClient.Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
            ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
                sQuery.Append(crmWebClient.Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
            End If

            sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))


            sQuery.Append(" group by aport_iata_code, aport_icao_code, ffd_origin_aport_id, ")
            sQuery.Append(" aport_name, aport_country,aport_id,  aport_city, aport_state ")
            sQuery.Append(" order by COUNT(*) desc ")

            'HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />" & Date.Now & " - get_most_common_destinations(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString
            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "utilization_functions.vb", sQuery.ToString.ToString)

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
                aError = "Error in get_most_common_destinations load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_most_common_destinations(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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
    Public Function util_get_routes_locations(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal location_field As String, ByVal product_code_selection As String, ByVal just_airports As Boolean, ByVal AirportTab As Boolean, ByRef FlightTotals As Long, Optional ByVal LimitThisQuery As Long = 0, Optional ByVal selected_product_codes As String = "", Optional ByVal from_header_string As String = "") As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()
        Dim sQuery_temp = New StringBuilder()
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim temp_string As String = ""


        Try

            '-- ***************  UPPER RIGHT TAB 2 - TOP ORIGINS/DESTINATIONS ************************
            '-- # FLIGHT ACTIVITY -MOST COMMON ORIGINS - WE WOULD ALSO DO SAME FOR DESTINATIONS
            sQuery.Append(" select DISTINCT top 15 ")


            If Trim(airport_direction) = "D" Then
                sQuery.Append(" " & Trim(location_field) & ", ")
            ElseIf Trim(airport_direction) = "O" Then
                sQuery.Append(" " & Trim(location_field) & ", ")
            Else
                sQuery.Append(" " & Trim(location_field) & ", ")
            End If

            If Trim(airport_direction) = "D" Then
                sQuery.Append(" count(*) AS tflights ")
            ElseIf Trim(airport_direction) = "O" Then
                sQuery.Append(" count(*) AS tflights ")
            Else
                sQuery.Append(" count(*) AS tflights ")
            End If

            sQuery.Append(" FROM view_flights WITH(NOLOCK) ")

            If Trim(airport_direction) = "D" Then
                sQuery.Append(" inner join Airport with (NOLOCK) on ffd_origin_aport_id = aport_id ")
                sQuery.Append(" inner join Country with (NOLOCK) on country_name = aport_country  ")
            ElseIf Trim(airport_direction) = "O" Then
                sQuery.Append(" inner join Airport with (NOLOCK) on ffd_dest_aport_id = aport_id ")
                sQuery.Append(" inner join Country with (NOLOCK) on country_name = aport_country  ")
            Else
                sQuery.Append(" inner join Airport with (NOLOCK) on ffd_origin_aport_id = aport_id ")
                sQuery.Append(" inner join Country with (NOLOCK) on country_name = aport_country  ")
            End If

            If Trim(location_field) = "state_name" Then
                sQuery.Append(" inner join State with (NOLOCK) on state_code = aport_state and state_country = replace(aport_country, 'U.S.', 'United States')  ")
            End If


            If String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentsStartDate) And String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentsEndDate) Then
                sQuery.Append(" WHERE convert(date, ffd_date, 0) >= (getdate()-182)  ")
            Else
                sQuery.Append(" where (convert(date, ffd_date, 0) >= '" & Month(searchCriteria.ViewCriteriaDocumentsStartDate) & "/" & Day(searchCriteria.ViewCriteriaDocumentsStartDate) & "/" & Year(searchCriteria.ViewCriteriaDocumentsStartDate) & "') ")
                sQuery.Append(" AND (convert(date, ffd_date, 0) <= '" & Month(searchCriteria.ViewCriteriaDocumentsEndDate) & "/" & Day(searchCriteria.ViewCriteriaDocumentsEndDate) & "/" & Year(searchCriteria.ViewCriteriaDocumentsEndDate) & "') ")
            End If


            sQuery.Append(" and ffd_dest_aport_id  > 0  ")  'and ffd_hide_flag= 'N' 

            If Trim(searchCriteria.ViewCriteriaContinent) = "International" Then
                sQuery.Append(" and (origin_aport_country not in ('United States', 'U.S.') or dest_aport_country not in ('United States', 'U.S.'))  ")
            End If

            If Trim(distance_string) <> "" And Len(Trim(distance_string)) > 2 Then
                sQuery.Append(distance_string)
            End If


            ' if there is an operator and there is no airport or airport list selected


            ' Call build_flight_data_from(sQuery)
            ' Call build_flight_data_where(sQuery, aport_id)

            If Trim(product_code_selection) <> "" Then
                ' sQuery.Append(Replace(product_code_selection, "amod", "Aircraft_Model.amod"))
                sQuery.Append(product_code_selection)
            Else
                If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
                    sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))
                Else
                    sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True))
                End If
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


            Dim temp_use As String = " "
            If searchCriteria.ViewCriteriaHasBusinessFlag = True Or searchCriteria.ViewCriteriaHasCommercialFlag = True Or searchCriteria.ViewCriteriaHasHelicopterFlag = True Then
                sQuery.Append(" and ( ")
            End If

            If searchCriteria.ViewCriteriaHasBusinessFlag = True Then
                If Trim(temp_use) = "" Then
                    temp_use = " or "
                Else
                    sQuery.Append(temp_use)
                End If
                sQuery.Append(" ac_product_business_flag = 'Y' ")
            End If

            If searchCriteria.ViewCriteriaHasCommercialFlag = True Then
                If Trim(temp_use) = "" Then
                    temp_use = " or "
                Else
                    sQuery.Append(temp_use)
                End If
                sQuery.Append(" ac_product_commercial_flag = 'Y' ")
            End If

            If searchCriteria.ViewCriteriaHasHelicopterFlag = True Then
                If Trim(temp_use) = "" Then
                    temp_use = " or "
                Else
                    sQuery.Append(temp_use)
                End If
                sQuery.Append(" ac_product_helicopter_flag = 'Y' ")
            End If

            If searchCriteria.ViewCriteriaHasBusinessFlag = True Or searchCriteria.ViewCriteriaHasCommercialFlag = True Or searchCriteria.ViewCriteriaHasHelicopterFlag = True Then
                sQuery.Append(" ) ")
            End If


            ' sQuery.Append(" AND (ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportIATA & "' or ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportICAO & "') ")

            If Not String.IsNullOrEmpty(Trim(Aircraft_IDS_String)) Then
                If exclude_Aircraft = True Then
                    sQuery.Append(" and ac_id not ")
                Else
                    sQuery.Append(" and ac_id ")
                End If
                sQuery.Append(" in (" & Aircraft_IDS_String & ") ")
            End If





            If Trim(airport_direction) = "D" Then
                If Airport_ID_OVERALL > 1 Then
                    sQuery.Append(" AND (ffd_dest_aport_id = '" & Airport_ID_OVERALL & "') ")
                ElseIf Not String.IsNullOrEmpty(Airport_IDS_String) Then
                    'Airport strings mean that a folder has been passed which means we need to display the airports in the folder
                    If AirportTab Then
                        If exclude_airport_check Then
                            sQuery.Append(" AND (ffd_dest_aport_id not in (" & Airport_IDS_String & ")) ")
                        Else
                            sQuery.Append(" AND (ffd_dest_aport_id in (" & Airport_IDS_String & ")) ")
                        End If
                    Else
                        'need routes
                        If exclude_airport_check Then
                            sQuery.Append(" AND (ffd_dest_aport_id not in (" & Airport_IDS_String & ")) ")
                        Else
                            sQuery.Append(" AND (ffd_dest_aport_id in (" & Airport_IDS_String & ")) ")
                        End If
                    End If
                End If
            ElseIf Trim(airport_direction) = "O" Then
                If Airport_ID_OVERALL > 1 Then
                    sQuery.Append(" AND (ffd_origin_aport_id = '" & Airport_ID_OVERALL & "') ")
                ElseIf Not String.IsNullOrEmpty(Airport_IDS_String) Then
                    'Airport strings mean that a folder has been passed which means we need to display the airports in the folder
                    If AirportTab Then
                        If exclude_airport_check Then
                            sQuery.Append(" AND (ffd_origin_aport_id not in (" & Airport_IDS_String & ")) ")
                        Else
                            sQuery.Append(" AND (ffd_origin_aport_id in (" & Airport_IDS_String & ")) ")
                        End If
                    Else
                        'need routes
                        If exclude_airport_check Then
                            sQuery.Append(" AND (ffd_origin_aport_id not in (" & Airport_IDS_String & ")) ")
                        Else
                            sQuery.Append(" AND (ffd_origin_aport_id in (" & Airport_IDS_String & ")) ")
                        End If
                    End If
                End If
            ElseIf Trim(airport_direction) = "X" Then
                '  sQuery.Append(" and ffd_dest_aport_id > 0")
                ' sQuery.Append(" AND (ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportIATA & "' or ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportICAO & "')")

                If Airport_ID_OVERALL > 1 Then
                    sQuery.Append(" AND ((ffd_origin_aport_id = '" & Airport_ID_OVERALL & "') or (ffd_dest_aport_id = '" & Airport_ID_OVERALL & "')) ")
                ElseIf Not String.IsNullOrEmpty(Airport_IDS_String) Then
                    If exclude_airport_check Then
                        sQuery.Append(" AND (ffd_origin_aport_id not in (" & Airport_IDS_String & ")) AND (ffd_dest_aport_id not in (" & Airport_IDS_String & "))   ")
                    Else
                        sQuery.Append(" AND (   (ffd_origin_aport_id in (" & Airport_IDS_String & "))  or (ffd_dest_aport_id in (" & Airport_IDS_String & "))   )")
                    End If

                End If

            Else
                If Airport_ID_OVERALL > 1 Then
                    sQuery.Append(" AND (ffd_dest_aport_id = '" & Airport_ID_OVERALL & "') ")
                ElseIf Not String.IsNullOrEmpty(Airport_IDS_String) Then
                    'Airport strings mean that a folder has been passed which means we need to display the airports in the folder
                    If AirportTab Then
                        If exclude_airport_check Then
                            sQuery.Append(" AND (ffd_dest_aport_id not in (" & Airport_IDS_String & ")) ")
                        Else
                            sQuery.Append(" AND (ffd_dest_aport_id in (" & Airport_IDS_String & ")) ")
                        End If
                    Else
                        'need routes
                        If exclude_airport_check Then
                            sQuery.Append(" AND (ffd_dest_aport_id not in (" & Airport_IDS_String & ")) ")
                        Else
                            sQuery.Append(" AND (ffd_dest_aport_id in (" & Airport_IDS_String & ")) ")
                        End If
                    End If
                End If
            End If






            If Trim(Operator_IDS_String) <> "" Then
                If exclude_check = True Then
                    sQuery.Append(" and comp_id not ")
                Else
                    sQuery.Append(" and comp_id ")
                End If
                sQuery.Append(" in (" & Operator_IDS_String & ") ")
            End If


            If searchCriteria.ViewCriteriaCompanyID > 0 And searchCriteria.ViewCriteriaAmodID > 0 Then
                sQuery.Append(" AND amod_id = " & searchCriteria.ViewCriteriaAmodID & "")
            ElseIf Not IsNothing(searchCriteria.ViewCriteriaAmodIDArray) Then
                If searchCriteria.ViewCriteriaCompanyID > 0 Then
                    sQuery.Append(SetUpModelString(searchCriteria))
                Else
                    sQuery.Append(SetUpModelString(searchCriteria))
                End If
            ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake) Then
                If searchCriteria.ViewCriteriaCompanyID > 0 Then
                    sQuery.Append(SetUpMakeString(searchCriteria))
                Else
                    sQuery.Append(SetUpMakeString(searchCriteria))
                End If
            End If

            If Not IsNothing(searchCriteria.ViewCriteriaTypeIDArray) Then
                If searchCriteria.ViewCriteriaCompanyID > 0 Then
                    sQuery.Append(SetUpTypeString(searchCriteria))
                Else
                    sQuery.Append(SetUpTypeString(searchCriteria))
                End If
            End If


            If Trim(rollup_text) <> "" Then
                sQuery.Append(rollup_text)
            ElseIf searchCriteria.ViewCriteriaCompanyID > 0 Then
                sQuery.Append(" AND comp_id = " & searchCriteria.ViewCriteriaCompanyID & "")
            End If



            'If searchCriteria.ViewCriteriaAmodID > -1 Then ' changed to 0 - MSW 
            If searchCriteria.ViewCriteriaAmodID > 0 Then
                sQuery.Append(crmWebClient.Constants.cAndClause + " amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
            ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
                sQuery.Append(SetUpMakeString(searchCriteria))
                'sQuery.Append(crmWebClient.Constants.cAndClause + "Aircraft_Model.amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
            End If


            sQuery.Append(" group by ")
            sQuery.Append(" " & Trim(location_field) & " ")

            sQuery.Append(" order by COUNT(*) desc ")

            'HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />" & Date.Now & " - get_most_common_origins(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString
            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "utilization_functions.vb", sQuery.ToString.ToString)

            SqlConn.ConnectionString = clientConnectString
            SqlConn.Open()
            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 600

            SqlCommand.CommandText = sQuery.ToString
            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                aError = "Error in get_fractional_shares load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_fractional_shares(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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
    Public Function get_most_common_origins(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal product_code_selection As String, ByVal just_airports As Boolean, ByVal AirportTab As Boolean, ByRef FlightTotals As Long, Optional ByVal LimitThisQuery As Long = 0, Optional ByVal selected_product_codes As String = "", Optional ByVal from_header_string As String = "") As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()
        Dim sQuery_temp = New StringBuilder()
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim temp_string As String = ""


        Try

            '-- ***************  UPPER RIGHT TAB 2 - TOP ORIGINS/DESTINATIONS ************************
            '-- # FLIGHT ACTIVITY -MOST COMMON ORIGINS - WE WOULD ALSO DO SAME FOR DESTINATIONS
            sQuery.Append(" select DISTINCT  ")

            If LimitThisQuery > 0 Then
                sQuery.Append(" top " + LimitThisQuery.ToString + " ")
            Else
                ' If FlightTotals > 50000 Then
                'sQuery.Append(" top 500 ")
                '  End If
            End If


            If Trim(from_header_string) = "Airports" And Airport_ID_OVERALL > 1 Then
                If Trim(airport_direction) = "X" Then
                    sQuery.Append("  ffd_origin_aport_id, origin_aport_iata_code, origin_aport_icao_code, origin_aport_name, origin_aport_country, origin_aport_city, origin_aport_state, origin_continent, ")
                    sQuery.Append("  ffd_dest_aport_id, dest_aport_iata_code, dest_aport_icao_code, dest_aport_name, dest_aport_country, dest_aport_city, dest_aport_state, dest_continent,  ")
                Else
                    If Trim(airport_direction) = "D" Then
                        sQuery.Append("   ffd_dest_aport_id, dest_aport_iata_code, dest_aport_icao_code , dest_aport_name, dest_aport_country, dest_aport_city, dest_aport_state, dest_continent, ")
                    ElseIf Trim(airport_direction) = "O" Then
                        sQuery.Append("   ffd_origin_aport_id, origin_aport_iata_code, origin_aport_icao_code, origin_aport_name, origin_aport_country, origin_aport_city, origin_aport_state, origin_continent, ")
                    Else
                        sQuery.Append("   ffd_dest_aport_id, dest_aport_iata_code, dest_aport_icao_code, dest_aport_name, dest_aport_country, dest_aport_city, dest_aport_state, dest_continent, ")
                    End If
                End If
            ElseIf Trim(airport_direction) = "X" Or Trim(from_header_string) = "Routes" Or ((searchCriteria.ViewCriteriaCompanyID > 0 Or Operator_IDS_String <> "") And Airport_ID_OVERALL <= 1 And Trim(Airport_IDS_String) = "" And just_airports = False) Or (Airport_IDS_String <> "" And just_airports = False) Or (Airport_ID_OVERALL > 1) Or (LimitThisQuery = 10) Then
                sQuery.Append("  ffd_origin_aport_id, origin_aport_iata_code, origin_aport_icao_code, origin_aport_name, origin_aport_country, origin_aport_city, origin_aport_state, origin_continent, ")
                sQuery.Append("  ffd_dest_aport_id, dest_aport_iata_code, dest_aport_icao_code, dest_aport_name, dest_aport_country, dest_aport_city, dest_aport_state, dest_continent,  ")
            Else
                If Trim(airport_direction) = "D" Then
                    sQuery.Append("   ffd_dest_aport_id, dest_aport_iata_code, dest_aport_icao_code , dest_aport_name, dest_aport_country, dest_aport_city, dest_aport_state, dest_continent, ")
                ElseIf Trim(airport_direction) = "O" Then
                    sQuery.Append("   ffd_origin_aport_id, origin_aport_iata_code, origin_aport_icao_code, origin_aport_name, origin_aport_country, origin_aport_city, origin_aport_state, origin_continent, ")
                Else
                    sQuery.Append("   ffd_dest_aport_id, dest_aport_iata_code, dest_aport_icao_code, dest_aport_name, dest_aport_country, dest_aport_city, dest_aport_state, dest_continent, ")
                End If
            End If

            If Trim(from_header_string) <> "Airports" And Trim(from_header_string) <> "Routes" And Trim(from_header_string) <> "company" And InStr(Trim(from_header_string), "TOP ROUTES") = 0 Then
                sQuery.Append(" amjiqs_cat_desc, ")
            End If

            '  sQuery.Append(" COMMERCIAL_FIELD, ")  

            If Trim(airport_direction) = "D" Then
                sQuery.Append(" ffd_dest_aport_id, count(*) AS tflights ")
            ElseIf Trim(airport_direction) = "O" Then
                sQuery.Append(" ffd_origin_aport_id , count(*) AS tflights ")
            Else
                sQuery.Append(" ffd_dest_aport_id, count(*) AS tflights ")
            End If

            If Trim(from_header_string) = "Routes" Then
                sQuery.Append(" , (SUM(ffd_flight_time)/count(*)) as AvgMinPerFlights , ")
                sQuery.Append("  (  SUM((ffd_flight_time* amod_fuel_burn_rate)/60)/count(*)) as TotalFuelBurnPerFlight , ")
                sQuery.Append("   (SUM(ffd_distance)/count(*)) as RouteDistance ")
            ElseIf Trim(from_header_string) = "Airports" Then
                sQuery.Append(" , (SUM(ffd_flight_time)/count(*)) as AvgMinPerFlights")
            End If

            Call build_flight_data_subselects(sQuery_temp)
            temp_string = sQuery_temp.ToString
            temp_string = Replace(Trim(temp_string), "AM2.", "")

            sQuery.Append(Trim(temp_string))
            ' sQuery.Append(" FROM FAA_Flight_Data FFD2 WITH(NOLOCK) ")


            If Trim(from_header_string) = "Airports" Then
                sQuery.Append(" FROM View_Flights_New WITH(NOLOCK) ")
            Else
                sQuery.Append(" FROM view_flights WITH(NOLOCK) ")
            End If


            'If (searchCriteria.ViewCriteriaCompanyID > 0 Or Trim(Operator_IDS_String) <> "") Then
            '  sQuery.Append(" INNER JOIN view_aircraft_company_flat WITH(NOLOCK) ON (ac_id = FFD2.ffd_ac_id) AND ac_journ_id = 0 and  cref_operator_flag  in ('Y', 'O') ")
            '  sQuery.Append(" INNER JOIN Aircraft_Model WITH (NOLOCK) ON Aircraft_Model.amod_id = view_aircraft_company_flat.amod_id ")
            'Else
            '  sQuery.Append(" INNER JOIN aircraft WITH(NOLOCK) ON (ac_id = FFD2.ffd_ac_id) AND ac_journ_id = 0 ")
            '  sQuery.Append(" INNER JOIN Aircraft_Model WITH (NOLOCK) ON Aircraft_Model.amod_id = aircraft.ac_amod_id ")
            'End If

            '  If (searchCriteria.ViewCriteriaCompanyID > 0 Or Trim(Operator_IDS_String) <> "") Then
            '  sQuery.Append(" INNER JOIN view_aircraft_company_flat WITH(NOLOCK) ON (ac_id = FFD2.ffd_ac_id) AND ac_journ_id = 0 and  cref_operator_flag  in ('Y', 'O') ")
            ' sQuery.Append(" INNER JOIN Aircraft_Model WITH (NOLOCK) ON Aircraft_Model.amod_id = view_aircraft_company_flat.amod_id ")
            '  Else
            '  sQuery.Append(" INNER JOIN aircraft WITH(NOLOCK) ON (ac_id = FFD2.ffd_ac_id) AND ac_journ_id = 0 ")
            '  sQuery.Append(" INNER JOIN Aircraft_Model WITH (NOLOCK) ON Aircraft_Model.amod_id = aircraft.ac_amod_id ")
            '  End If


            'sQuery.Append(" INNER JOIN aircraft WITH(NOLOCK) ON (ac_id = ffd_ac_id) AND ac_journ_id = 0  ")
            ' sQuery.Append(" INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id  ")



            'If Trim(airport_direction) = "D" Then
            '  sQuery.Append(" INNER JOIN Airport AA WITH(NOLOCK) ON FFD2.ffd_dest_aport_id  = AA.aport_id ")

            '  If ((searchCriteria.ViewCriteriaCompanyID > 0 Or Operator_IDS_String <> "") And Airport_ID_OVERALL <= 1 And Trim(Airport_IDS_String) = "" And just_airports = False) Or (Airport_IDS_String <> "" And just_airports = False) Or (Airport_ID_OVERALL > 1) Or (LimitThisQuery = 10) Then
            '    sQuery.Append(" INNER JOIN Airport AD WITH(NOLOCK) ON FFD2.ffd_origin_aport_id = AD.aport_id ")
            '  End If
            'ElseIf Trim(airport_direction) = "O" Then
            '  ' switched these two labels 
            '  sQuery.Append(" INNER JOIN Airport AA WITH(NOLOCK) ON FFD2.ffd_origin_aport_id  = AA.aport_id ")

            '  If ((searchCriteria.ViewCriteriaCompanyID > 0 Or Operator_IDS_String <> "") And Airport_ID_OVERALL <= 1 And Trim(Airport_IDS_String) = "" And just_airports = False) Or (Airport_IDS_String <> "" And just_airports = False) Or (Airport_ID_OVERALL > 1) Or (LimitThisQuery = 10) Then
            '    sQuery.Append(" INNER JOIN Airport AD WITH(NOLOCK) ON FFD2.ffd_dest_aport_id = AD.aport_id ")
            '  End If
            'Else
            '  sQuery.Append(" INNER JOIN Airport AA WITH(NOLOCK) ON FFD2.ffd_dest_aport_id  = AA.aport_id ")

            '  If ((searchCriteria.ViewCriteriaCompanyID > 0 Or Operator_IDS_String <> "") And Airport_ID_OVERALL <= 1 And Trim(Airport_IDS_String) = "" And just_airports = False) Or (Airport_IDS_String <> "" And just_airports = False) Or (Airport_ID_OVERALL > 1) Or (LimitThisQuery = 10) Then
            '    sQuery.Append(" INNER JOIN Airport AD WITH(NOLOCK) ON FFD2.ffd_origin_aport_id = AD.aport_id ")
            '  End If
            'End If






            If String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentsStartDate) And String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentsEndDate) Then
                sQuery.Append(" WHERE convert(date, ffd_date, 0) >= (getdate()-182)  ")
            Else
                sQuery.Append(" where (convert(date, ffd_date, 0) >= '" & Month(searchCriteria.ViewCriteriaDocumentsStartDate) & "/" & Day(searchCriteria.ViewCriteriaDocumentsStartDate) & "/" & Year(searchCriteria.ViewCriteriaDocumentsStartDate) & "') ")
                sQuery.Append(" AND (convert(date, ffd_date, 0) <= '" & Month(searchCriteria.ViewCriteriaDocumentsEndDate) & "/" & Day(searchCriteria.ViewCriteriaDocumentsEndDate) & "/" & Year(searchCriteria.ViewCriteriaDocumentsEndDate) & "') ")
            End If


            sQuery.Append(" and ffd_dest_aport_id  > 0  ")  'and ffd_hide_flag= 'N' 

            If Trim(searchCriteria.ViewCriteriaContinent) = "International" Then
                sQuery.Append(" and (origin_aport_country not in ('United States', 'U.S.') or dest_aport_country not in ('United States', 'U.S.'))  ")
            End If

            If Trim(distance_string) <> "" And Len(Trim(distance_string)) > 2 Then
                sQuery.Append(distance_string)
            End If


            ' if there is an operator and there is no airport or airport list selected


            ' Call build_flight_data_from(sQuery)
            ' Call build_flight_data_where(sQuery, aport_id)

            If Trim(product_code_selection) <> "" Then
                ' sQuery.Append(Replace(product_code_selection, "amod", "Aircraft_Model.amod"))
                sQuery.Append(product_code_selection)
            Else
                If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
                    sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))
                Else
                    sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True))
                End If
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


            Dim temp_use As String = " "
            If searchCriteria.ViewCriteriaHasBusinessFlag = True Or searchCriteria.ViewCriteriaHasCommercialFlag = True Or searchCriteria.ViewCriteriaHasHelicopterFlag = True Then
                sQuery.Append(" and ( ")
            End If

            If searchCriteria.ViewCriteriaHasBusinessFlag = True Then
                If Trim(temp_use) = "" Then
                    temp_use = " or "
                Else
                    sQuery.Append(temp_use)
                End If
                sQuery.Append(" ac_product_business_flag = 'Y' ")
            End If

            If searchCriteria.ViewCriteriaHasCommercialFlag = True Then
                If Trim(temp_use) = "" Then
                    temp_use = " or "
                Else
                    sQuery.Append(temp_use)
                End If
                sQuery.Append(" ac_product_commercial_flag = 'Y' ")
            End If

            If searchCriteria.ViewCriteriaHasHelicopterFlag = True Then
                If Trim(temp_use) = "" Then
                    temp_use = " or "
                Else
                    sQuery.Append(temp_use)
                End If
                sQuery.Append(" ac_product_helicopter_flag = 'Y' ")
            End If

            If searchCriteria.ViewCriteriaHasBusinessFlag = True Or searchCriteria.ViewCriteriaHasCommercialFlag = True Or searchCriteria.ViewCriteriaHasHelicopterFlag = True Then
                sQuery.Append(" ) ")
            End If


            ' sQuery.Append(" AND (ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportIATA & "' or ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportICAO & "') ")

            If Not String.IsNullOrEmpty(Trim(Aircraft_IDS_String)) Then
                If exclude_Aircraft = True Then
                    sQuery.Append(" and ac_id not ")
                Else
                    sQuery.Append(" and ac_id ")
                End If
                sQuery.Append(" in (" & Aircraft_IDS_String & ") ")
            End If





            If Trim(airport_direction) = "D" Then
                If Airport_ID_OVERALL > 1 Then
                    sQuery.Append(" AND (ffd_dest_aport_id = '" & Airport_ID_OVERALL & "') ")
                ElseIf Not String.IsNullOrEmpty(Airport_IDS_String) Then
                    'Airport strings mean that a folder has been passed which means we need to display the airports in the folder
                    If AirportTab Then
                        If exclude_airport_check Then
                            sQuery.Append(" AND (ffd_dest_aport_id not in (" & Airport_IDS_String & ")) ")
                        Else
                            sQuery.Append(" AND (ffd_dest_aport_id in (" & Airport_IDS_String & ")) ")
                        End If
                    Else
                        'need routes
                        If exclude_airport_check Then
                            sQuery.Append(" AND (ffd_dest_aport_id not in (" & Airport_IDS_String & ")) ")
                        Else
                            sQuery.Append(" AND (ffd_dest_aport_id in (" & Airport_IDS_String & ")) ")
                        End If
                    End If
                End If
            ElseIf Trim(airport_direction) = "O" Then
                If Airport_ID_OVERALL > 1 Then
                    sQuery.Append(" AND (ffd_origin_aport_id = '" & Airport_ID_OVERALL & "') ")
                ElseIf Not String.IsNullOrEmpty(Airport_IDS_String) Then
                    'Airport strings mean that a folder has been passed which means we need to display the airports in the folder
                    If AirportTab Then
                        If exclude_airport_check Then
                            sQuery.Append(" AND (ffd_origin_aport_id not in (" & Airport_IDS_String & ")) ")
                        Else
                            sQuery.Append(" AND (ffd_origin_aport_id in (" & Airport_IDS_String & ")) ")
                        End If
                    Else
                        'need routes
                        If exclude_airport_check Then
                            sQuery.Append(" AND (ffd_origin_aport_id not in (" & Airport_IDS_String & ")) ")
                        Else
                            sQuery.Append(" AND (ffd_origin_aport_id in (" & Airport_IDS_String & ")) ")
                        End If
                    End If
                End If
            ElseIf Trim(airport_direction) = "X" Then
                '  sQuery.Append(" and ffd_dest_aport_id > 0")
                ' sQuery.Append(" AND (ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportIATA & "' or ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportICAO & "')")

                If Airport_ID_OVERALL > 1 Then
                    sQuery.Append(" AND ((ffd_origin_aport_id = '" & Airport_ID_OVERALL & "') or (ffd_dest_aport_id = '" & Airport_ID_OVERALL & "')) ")
                ElseIf Not String.IsNullOrEmpty(Airport_IDS_String) Then
                    If exclude_airport_check Then
                        sQuery.Append(" AND (ffd_origin_aport_id not in (" & Airport_IDS_String & ")) AND (ffd_dest_aport_id not in (" & Airport_IDS_String & "))   ")
                    Else
                        sQuery.Append(" AND (   (ffd_origin_aport_id in (" & Airport_IDS_String & "))  or (ffd_dest_aport_id in (" & Airport_IDS_String & "))   )")
                    End If

                End If

            Else
                If Airport_ID_OVERALL > 1 Then
                    sQuery.Append(" AND (ffd_dest_aport_id = '" & Airport_ID_OVERALL & "') ")
                ElseIf Not String.IsNullOrEmpty(Airport_IDS_String) Then
                    'Airport strings mean that a folder has been passed which means we need to display the airports in the folder
                    If AirportTab Then
                        If exclude_airport_check Then
                            sQuery.Append(" AND (ffd_dest_aport_id not in (" & Airport_IDS_String & ")) ")
                        Else
                            sQuery.Append(" AND (ffd_dest_aport_id in (" & Airport_IDS_String & ")) ")
                        End If
                    Else
                        'need routes
                        If exclude_airport_check Then
                            sQuery.Append(" AND (ffd_dest_aport_id not in (" & Airport_IDS_String & ")) ")
                        Else
                            sQuery.Append(" AND (ffd_dest_aport_id in (" & Airport_IDS_String & ")) ")
                        End If
                    End If
                End If
            End If






            If Trim(Operator_IDS_String) <> "" Then
                If exclude_check = True Then
                    sQuery.Append(" and comp_id not ")
                Else
                    sQuery.Append(" and comp_id ")

                End If
                sQuery.Append(" in (" & Operator_IDS_String & ") ")
            End If


            If searchCriteria.ViewCriteriaCompanyID > 0 And searchCriteria.ViewCriteriaAmodID > 0 Then
                sQuery.Append(" AND amod_id = " & searchCriteria.ViewCriteriaAmodID & "")
            ElseIf Not IsNothing(searchCriteria.ViewCriteriaAmodIDArray) Then
                If searchCriteria.ViewCriteriaCompanyID > 0 Then
                    sQuery.Append(SetUpModelString(searchCriteria))
                Else
                    sQuery.Append(SetUpModelString(searchCriteria))
                End If
            ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake) Then
                If searchCriteria.ViewCriteriaCompanyID > 0 Then
                    sQuery.Append(SetUpMakeString(searchCriteria))
                Else
                    sQuery.Append(SetUpMakeString(searchCriteria))
                End If
            End If

            If Not IsNothing(searchCriteria.ViewCriteriaTypeIDArray) Then
                If searchCriteria.ViewCriteriaCompanyID > 0 Then
                    sQuery.Append(SetUpTypeString(searchCriteria))
                Else
                    sQuery.Append(SetUpTypeString(searchCriteria))
                End If
            End If

            ' ADDED MSW - 3/10/20 TO BUILD IN DROP DOWN FOR IN OPERATION 
            If Not IsNothing(searchCriteria.viewCriteriaInOperation) Then
                If Trim(searchCriteria.viewCriteriaInOperation) <> "" Then
                    sQuery.Append(Build_In_Operation_String(searchCriteria))
                End If
            End If




            If Trim(rollup_text) <> "" Then
                sQuery.Append(rollup_text)
            ElseIf searchCriteria.ViewCriteriaCompanyID > 0 Then
                sQuery.Append(" AND comp_id = " & searchCriteria.ViewCriteriaCompanyID & "")
            End If



            'If searchCriteria.ViewCriteriaAmodID > -1 Then ' changed to 0 - MSW 
            If searchCriteria.ViewCriteriaAmodID > 0 Then
                sQuery.Append(crmWebClient.Constants.cAndClause + " amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
            ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
                sQuery.Append(SetUpMakeString(searchCriteria))
                'sQuery.Append(crmWebClient.Constants.cAndClause + "Aircraft_Model.amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
            End If


            sQuery.Append(" group by ")

            If Trim(from_header_string) = "Airports" And Airport_ID_OVERALL > 1 Then
                If Trim(airport_direction) = "X" Then
                    sQuery.Append("  origin_aport_iata_code, origin_aport_icao_code, origin_aport_name, origin_aport_country, ffd_origin_aport_id, origin_aport_city, origin_aport_state, origin_continent, ")
                    sQuery.Append(" dest_aport_iata_code, dest_aport_icao_code, dest_aport_name, dest_aport_country, ffd_dest_aport_id,  dest_aport_city, dest_aport_state, dest_continent  ")
                Else
                    If Trim(airport_direction) = "D" Then
                        sQuery.Append(" dest_aport_iata_code, dest_aport_icao_code, dest_aport_name, dest_aport_country, ffd_dest_aport_id,  dest_aport_city, dest_aport_state, dest_continent  ")
                    ElseIf Trim(airport_direction) = "O" Then
                        sQuery.Append(" origin_aport_iata_code, origin_aport_icao_code, origin_aport_name, origin_aport_country, ffd_origin_aport_id, origin_aport_city, origin_aport_state, origin_continent ")
                    Else
                        sQuery.Append(" dest_aport_iata_code, dest_aport_icao_code, dest_aport_name, dest_aport_country, ffd_dest_aport_id,  dest_aport_city, dest_aport_state, dest_continent  ")
                    End If
                End If
            ElseIf Trim(airport_direction) = "X" Or Trim(from_header_string) = "Routes" Or ((searchCriteria.ViewCriteriaCompanyID > 0 Or Operator_IDS_String <> "") And Airport_ID_OVERALL <= 1 And Trim(Airport_IDS_String) = "" And just_airports = False) Or (Airport_IDS_String <> "" And just_airports = False) Or (Airport_ID_OVERALL > 1) Or (LimitThisQuery = 10) Then
                sQuery.Append("  origin_aport_iata_code, origin_aport_icao_code, origin_aport_name, origin_aport_country, ffd_origin_aport_id, origin_aport_city, origin_aport_state, origin_continent, ")
                sQuery.Append(" dest_aport_iata_code, dest_aport_icao_code, dest_aport_name, dest_aport_country, ffd_dest_aport_id,  dest_aport_city, dest_aport_state, dest_continent  ")
            Else
                If Trim(airport_direction) = "D" Then
                    sQuery.Append(" dest_aport_iata_code, dest_aport_icao_code, dest_aport_name, dest_aport_country, ffd_dest_aport_id,  dest_aport_city, dest_aport_state, dest_continent  ")
                ElseIf Trim(airport_direction) = "O" Then
                    sQuery.Append(" origin_aport_iata_code, origin_aport_icao_code, origin_aport_name, origin_aport_country, ffd_origin_aport_id, origin_aport_city, origin_aport_state, origin_continent ")
                Else
                    sQuery.Append(" dest_aport_iata_code, dest_aport_icao_code, dest_aport_name, dest_aport_country, ffd_dest_aport_id,  dest_aport_city, dest_aport_state, dest_continent  ")
                End If
            End If

            If Trim(airport_direction) = "D" Then
                sQuery.Append(" , ffd_dest_aport_id  ")
            ElseIf Trim(airport_direction) = "O" Then
                sQuery.Append(" , ffd_origin_aport_id  ")
            Else
                sQuery.Append(" , ffd_dest_aport_id  ")
            End If

            If Trim(from_header_string) <> "Airports" And Trim(from_header_string) <> "Routes" And Trim(from_header_string) <> "company" And InStr(Trim(from_header_string), "TOP ROUTES") = 0 Then
                sQuery.Append(" , amjiqs_cat_desc ")
            End If
            '  sQuery.Append(" , COMMERCIAL_FIELD ")  

            ''If flights are 0 meaning a total hasn't been passed here, or if they're more than 1000
            'If FlightTotals = 0 Or FlightTotals > 1000 Then
            '  If (Trim(searchCriteria.ViewCriteriaCompanyID) = 0 And Trim(Operator_IDS_String) = "" And just_airports = False) Or (Airport_IDS_String <> "" And just_airports = False) Or (LimitThisQuery = 10) Then
            '    sQuery.Append(" having(COUNT(ffd_unique_flight_id) > 1) ")
            '  ElseIf FlightTotals > 0 Then
            '    If FlightTotals > 10000 Then
            '      sQuery.Append(" having(COUNT(ffd_unique_flight_id) > 4) ")
            '    Else
            '      sQuery.Append(" having(COUNT(ffd_unique_flight_id) > 1) ")
            '    End If
            '  End If
            'End If


            sQuery.Append(" order by COUNT(*) desc ")


            If Trim(from_header_string) = "Airports" Then
                If Trim(airport_direction) = "D" Then
                    sQuery.Append(" , View_Flights_New.ffd_dest_aport_id asc ")
                ElseIf Trim(airport_direction) = "O" Then
                    sQuery.Append(" , View_Flights_New.ffd_origin_aport_id asc ")
                Else
                    sQuery.Append(" , View_Flights_New.ffd_dest_aport_id asc ")
                End If
            Else
                If Trim(airport_direction) = "D" Then
                    sQuery.Append(" , view_flights.ffd_dest_aport_id asc ")
                ElseIf Trim(airport_direction) = "O" Then
                    sQuery.Append(" , view_flights.ffd_origin_aport_id asc ")
                Else
                    sQuery.Append(" , view_flights.ffd_dest_aport_id asc ")
                End If
            End If





            'HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />" & Date.Now & " - get_most_common_origins(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString
            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "utilization_functions.vb", sQuery.ToString.ToString)

            SqlConn.ConnectionString = clientConnectString
            SqlConn.Open()
            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 600

            SqlCommand.CommandText = sQuery.ToString
            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                aError = "Error in get_fractional_shares load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_fractional_shares(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

    Public Function get_ac_reg_searched(ByVal reg_num_search As String, ByVal is_exact As String, ByVal dont_search_prev As String) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim query_distance As String = ""
        Dim max_NORTH As Double = 0.0
        Dim max_SOUTH As Double = 0.0
        Dim max_WEST As Double = 0.0
        Dim max_EAST As Double = 0.0


        Try

            reg_num_search = Trim(reg_num_search)

            sQuery.Append("select ac_id as ACId, amod_make_name As Make, amod_model_name As Model, ")
            sQuery.Append(" ac_ser_no_full As SerNbr, ac_reg_no As RegNbr  ")
            sQuery.Append(" FROM Aircraft_Flat WITH(NOLOCK) ")

            'Modified 10/29/15: Amanda. Task:
            'Speed issues were reported in the search on registration numbers. Investigation shows that we were not searching the 
            '"search" field which is indexed but the formatted registration number field which was not indexed. We need to 
            'modify the code to use the ac_reg_no_search field in all locations.
            'This does not change or affect the previous reg field.
            'ALSO: Changed the debug text to use the correct name of the function.
            sQuery.Append(" WHERE ( ac_reg_no_search ")

            If Trim(is_exact) = "Y" Then
                sQuery.Append(" = '" & Replace(reg_num_search, "-", "") & "' ")
            Else
                sQuery.Append(" like '" & Replace(reg_num_search, "-", "") & "%' ")
            End If

            If Trim(dont_search_prev) = "Y" Then

            Else
                If Trim(is_exact) = "Y" Then
                    sQuery.Append(" or ac_prev_reg_no = '" & reg_num_search & "' ")
                Else
                    sQuery.Append(" or ac_prev_reg_no like '" & reg_num_search & "%' ")
                End If
            End If

            sQuery.Append(" ) and ac_journ_id = 0 ")


            sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))


            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />" & Date.Now & " - get_ac_reg_searched(ByVal reg_num_search As String, ByVal is_exact As String, ByVal dont_search_prev As String) As DataTable</b><br />" + sQuery.ToString

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
                aError = "Error in get_ac_searched load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_ac_searched(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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
    Public Function get_nearby_airports(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal temp_distance As Integer, ByVal org_latitude As Double, ByVal org_longitude As Double, ByVal use_controlled As Boolean) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim query_distance As String = ""
        Dim max_NORTH As Double = 0.0
        Dim max_SOUTH As Double = 0.0
        Dim max_WEST As Double = 0.0
        Dim max_EAST As Double = 0.0


        Try


            '-- 1. get aport_latitude_decimal and aport_longitude_decimal for my airport (use aport id)
            '-- 2. set the distance for radius to find nearby airports - set to 150 miles as default
            '-- 3. convert the miles value into a lat-long adjustment value - query distance below
            '-- 4. add/subtract the query distance to/from the lat and long

            sQuery.Append("select distinct aport_city, aport_state, aport_country,aport_id, aport_name,  ")
            sQuery.Append("aport_iata_code, aport_icao_code, aport_longitude_decimal, aport_latitude_decimal ")
            sQuery.Append("from Airport with (NOLOCK) ")
            sQuery.Append("where aport_active_flag='Y' ")
            sQuery.Append("and aport_latitude_full <> '' ")
            sQuery.Append("and aport_iata_code <> '' and aport_icao_code <> '' ")

            sQuery.Append(" AND aport_max_runway_length IS NOT NULL ")
            sQuery.Append(" AND aport_max_runway_length >= 0  ")

            '  sQuery.Append("and aport_iata_code <> '" & searchCriteria.ViewCriteriaAirportIATA & "' ")
            sQuery.Append("and aport_id <> '" & Airport_ID_OVERALL & "' ")

            If Trim(use_controlled) = True Then
                sQuery.Append(" and CHARINDEX('0',aport_iata_code) = 0 ")
                sQuery.Append("and CHARINDEX('1',aport_iata_code) = 0 ")
                sQuery.Append("and CHARINDEX('2',aport_iata_code) = 0 ")
                sQuery.Append("and CHARINDEX('3',aport_iata_code) = 0 ")
                sQuery.Append("and CHARINDEX('4',aport_iata_code) = 0 ")
                sQuery.Append("and CHARINDEX('5',aport_iata_code) = 0 ")
                sQuery.Append("and CHARINDEX('6',aport_iata_code) = 0 ")
                sQuery.Append("and CHARINDEX('7',aport_iata_code) = 0 ")
                sQuery.Append("and CHARINDEX('8',aport_iata_code) = 0 ")
                sQuery.Append("and CHARINDEX('9',aport_iata_code) = 0 ")
            End If




            If temp_distance > 0 Then
                query_distance = CDbl(temp_distance / 69).ToString 'http://www.distancebetweencities.net/ helped us 
            Else
                query_distance = "2.1739"
            End If

            'Select Case temp_distance
            '  Case 25
            '    query_distance = ".255"
            '  Case 50
            '    query_distance = ".4"
            '  Case 75
            '    query_distance = ".67"
            '  Case 100
            '    query_distance = ".7546"
            '  Case 150
            '    query_distance = "1.34"
            '  Case 200
            '    query_distance = "1.55"
            '  Case Else
            '    'query_distance = "1.34"
            '    query_distance = "2.1739"
            'End Select

            'max_NORTH = FormatNumber(org_longitude + query_distance, 6)
            'max_SOUTH = FormatNumber(org_longitude - query_distance, 6)
            'max_WEST = FormatNumber(org_latitude + query_distance, 6)
            'max_EAST = FormatNumber(org_latitude - query_distance, 6)

            max_WEST = FormatNumber(org_longitude + query_distance, 6)
            max_EAST = FormatNumber(org_longitude - query_distance, 6)
            max_NORTH = FormatNumber(org_latitude + query_distance, 6)
            max_SOUTH = FormatNumber(org_latitude - query_distance, 6)

            sQuery.Append("AND (aport_longitude_decimal <= " & max_WEST & " AND aport_longitude_decimal >= " & max_EAST & ")  ")
            sQuery.Append("AND (aport_latitude_decimal <= " & max_NORTH & " AND aport_latitude_decimal >= " & max_SOUTH & ")  ")
            sQuery.Append("order by aport_name asc, aport_city, aport_state, aport_country ")


            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />" & Date.Now & " - get_nearby_airports(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
                aError = "Error in get_fractional_shares load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_fractional_shares(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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
    Public Function get_flight_activity_by_ac(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal show_not_based As Boolean, ByVal product_code_selection As String, ByRef TotalFlights As Long, Optional ByVal from_spot As String = "", Optional ByRef BasedAtAirport As Boolean = False, Optional ByVal limitthisQuery As Integer = 0, Optional ByVal include_weight As String = "") As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim temp_string As String = ""
        Dim sQuery_temp As New StringBuilder


        Try

            '-- ***************  UPPER RIGHT TAB 3 - TOP MODELS ************************
            '-- # FLIGHT ACTIVITY BY MODEL

            sQuery.Append(" SELECT DISTINCT ")

            If limitthisQuery > 0 Then
                sQuery.Append(" top " & limitthisQuery & " ")
            End If

            If BasedAtAirport And (Airport_ID_OVERALL > 1 Or Trim(Airport_IDS_String) <> "") Then
                sQuery.Append(" Aircraft_Model.amod_make_name, Aircraft_Model.amod_model_name, ")
                sQuery.Append(" aircraft.ac_ser_no_full as SERNO_NONDISPLAY, aircraft.ac_ser_no_sort as SERNOSORT_NONDISPLAY,  ")
                sQuery.Append(" aircraft.ac_reg_no as REGNO_NONDISPLAY, faablk_reg_no,  ")
                sQuery.Append(" aircraft.ac_ser_no_full, aircraft.ac_reg_no, base_aport_name, aircraft.ac_id,  ")
            Else
                sQuery.Append(" amod_make_name, amod_model_name, ")
                sQuery.Append(" ac_ser_no_full as SERNO_NONDISPLAY, ac_ser_no_sort as SERNOSORT_NONDISPLAY,  ")
                sQuery.Append(" ac_reg_no as REGNO_NONDISPLAY, faablk_reg_no,  ")
                sQuery.Append(" ac_ser_no_full, ac_reg_no, base_aport_name, ac_id,  ")
            End If

            ' sQuery.Append(" case when ac_reg_no=faablk_reg_no then 'BLOCKED' else ac_ser_no_full end as ac_ser_no_full, ")
            ' sQuery.Append(" case when ac_reg_no=faablk_reg_no then 'BLOCKED' else ac_reg_no end as ac_reg_no, ")
            ' If searchCriteria.ViewCriteriaCompanyID > 0 Then
            If Trim(from_spot) = "pdf2" And searchCriteria.ViewCriteriaCompanyID > 0 Then
                sQuery.Append(" comp_name, ")
            ElseIf Trim(from_spot) = "pdf2" Then
                sQuery.Append(" Company.comp_name, ")
            End If

            If BasedAtAirport And (Airport_ID_OVERALL > 1 Or Trim(Airport_IDS_String) <> "") Then
                If include_weight = "Y" Then
                    sQuery.Append(" amjiqs_cat_desc , ")
                End If
            Else
                sQuery.Append(" amjiqs_cat_desc , ")
            End If


            '  sQuery.Append(" COMMERCIAL_FIELD, ")  

            sQuery.Append(" count(*) AS tflights  ")




            If BasedAtAirport And (Airport_ID_OVERALL > 1 Or Trim(Airport_IDS_String) <> "") Then
                Call build_flight_data_subselects(sQuery_temp, 1)
            Else
                Call build_flight_data_subselects(sQuery_temp, 0)
            End If



            temp_string = sQuery_temp.ToString
            temp_string = Replace(Trim(temp_string), "AM2.", "")

            sQuery.Append(Trim(temp_string))

            sQuery.Append(", (SUM(ffd_distance)/count(*)) as AvgDistance ")
            sQuery.Append(" , (SUM(ffd_flight_time)/count(*)) as AvgMinPerFlights ")

            If Trim(from_spot) = "pdf2" Then
                sQuery.Append(", SUM(ffd_distance) as distance ")
            Else

                'Phone_Numbers.pnum_number_full  AS 'OFFICE PHONE', 
                If BasedAtAirport And (Airport_ID_OVERALL > 1 Or Trim(Airport_IDS_String) <> "") Then
                    sQuery.Append(", company.comp_name AS 'OPERATOR', company.comp_address1 AS 'ADDRESS', company.comp_city AS 'CITY', company.comp_state AS 'STATE', company.comp_country AS 'COUNTRY', ")
                    sQuery.Append("company.comp_web_address AS 'WEB ADDRESS', company.comp_email_address AS 'EMAIL',Phone_Numbers.pnum_number_full  AS 'OFFICE PHONE',  company.comp_id ")
                Else
                    sQuery.Append(", comp_name AS 'OPERATOR', comp_address1 AS 'ADDRESS', comp_city AS 'CITY', comp_state AS 'STATE', comp_country AS 'COUNTRY', ")
                    sQuery.Append("comp_web_address AS 'WEB ADDRESS', comp_email_address AS 'EMAIL',comp_off_phone AS 'OFFICE PHONE', comp_id ")
                End If

            End If

            ' sQuery.Append(" FROM FAA_Flight_Data FFD2 WITH(NOLOCK)  ")


            sQuery.Append(", cbus_name ")


            ' if we have picked an airport and are saying where the ac is best, then get them all
            If BasedAtAirport And (Airport_ID_OVERALL > 1 Or Trim(Airport_IDS_String) <> "") Then
                sQuery.Append(" from Aircraft with (NOLOCK) ")
                sQuery.Append(" inner join Aircraft_Model with (NOLOCK) on ac_amod_id = amod_id  ")
                ' sQuery.Append(" left outer join view_flights on view_flights.ac_id = aircraft.ac_id ")  ' changed on 5/15/20
                sQuery.Append(" left outer join View_Flights_Current_Operator on View_Flights_Current_Operator.ac_id = aircraft.ac_id ")


                If String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentsStartDate) And String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentsEndDate) Then
                    sQuery.Append(" and (convert(date, ffd_date, 0) >= (getdate()-182)  ")
                Else
                    sQuery.Append(" and (convert(date, ffd_date, 0) >= '" & Month(searchCriteria.ViewCriteriaDocumentsStartDate) & "/" & Day(searchCriteria.ViewCriteriaDocumentsStartDate) & "/" & Year(searchCriteria.ViewCriteriaDocumentsStartDate) & "') ")
                    sQuery.Append(" AND (convert(date, ffd_date, 0) <= '" & Month(searchCriteria.ViewCriteriaDocumentsEndDate) & "/" & Day(searchCriteria.ViewCriteriaDocumentsEndDate) & "/" & Year(searchCriteria.ViewCriteriaDocumentsEndDate) & "') ")
                End If



                If Trim(airport_direction) = "O" Then
                    If Airport_ID_OVERALL > 1 Then
                        sQuery.Append(" AND (ffd_origin_aport_id = '" & Airport_ID_OVERALL & "') ")
                    ElseIf Not String.IsNullOrEmpty(Airport_IDS_String) Then
                        If exclude_airport_check Then
                            sQuery.Append(" AND (ffd_origin_aport_id not in (" & Airport_IDS_String & ")) ")
                        Else
                            sQuery.Append(" AND (ffd_origin_aport_id in (" & Airport_IDS_String & ")) ")
                        End If
                    End If
                ElseIf Trim(airport_direction) = "D" Then
                    If Airport_ID_OVERALL > 1 Then
                        sQuery.Append(" AND (ffd_dest_aport_id = '" & Airport_ID_OVERALL & "') ")
                    ElseIf Not String.IsNullOrEmpty(Airport_IDS_String) Then
                        If exclude_airport_check Then
                            sQuery.Append(" AND (ffd_dest_aport_id not in (" & Airport_IDS_String & ")) ")
                        Else
                            sQuery.Append(" AND (ffd_dest_aport_id in (" & Airport_IDS_String & ")) ")
                        End If
                    End If
                ElseIf Trim(airport_direction) = "X" Then
                    '  sQuery.Append(" and ffd_dest_aport_id > 0")
                    ' sQuery.Append(" AND (ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportIATA & "' or ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportICAO & "')")

                    If Airport_ID_OVERALL > 1 Then
                        sQuery.Append(" AND ((ffd_origin_aport_id = '" & Airport_ID_OVERALL & "') or (ffd_dest_aport_id = '" & Airport_ID_OVERALL & "')) ")
                    ElseIf Not String.IsNullOrEmpty(Airport_IDS_String) Then
                        If exclude_airport_check Then
                            sQuery.Append(" AND (ffd_origin_aport_id not in (" & Airport_IDS_String & ")) AND (ffd_dest_aport_id not in (" & Airport_IDS_String & "))   ")
                        Else
                            sQuery.Append(" AND (   (ffd_origin_aport_id in (" & Airport_IDS_String & "))  or (ffd_dest_aport_id in (" & Airport_IDS_String & "))   )")
                        End If

                    End If


                Else
                    If Airport_ID_OVERALL > 1 Then
                        sQuery.Append(" AND (ffd_dest_aport_id = '" & Airport_ID_OVERALL & "') ")
                    ElseIf Not String.IsNullOrEmpty(Airport_IDS_String) Then
                        If exclude_airport_check Then
                            sQuery.Append(" AND (ffd_dest_aport_id not in (" & Airport_IDS_String & ")) ")
                        Else
                            sQuery.Append(" AND (ffd_dest_aport_id in (" & Airport_IDS_String & ")) ")
                        End If
                    End If
                End If

                sQuery.Append(" left outer  JOIN aircraft_reference WITH (NOLOCK) ON aircraft_reference.cref_ac_id = aircraft.ac_id and cref_journ_id = ac_journ_id and cref_operator_flag = 'Y' ")
                sQuery.Append(" left outer JOIN company with (NOLOCK) on  aircraft_reference.cref_comp_id =  company.comp_id and aircraft_reference.cref_journ_id = company.comp_journ_id  ")
                sQuery.Append(" left outer JOIN Phone_Numbers  with (NOLOCK)  ON pnum_comp_id = Company.comp_id and pnum_journ_id = comp_journ_id and pnum_type = 'Office' and pnum_contact_id = 0  ")
                sQuery.Append(" left outer JOIN Business_Type_Reference WITH(NOLOCK)on company.comp_id = bustypref_comp_id and  bustypref_journ_id = 0 ")

            Else
                sQuery.Append(" FROM View_Flights_Current_Operator WITH(NOLOCK) ")
                'sQuery.Append(" left outer JOIN Business_Type_Reference WITH(NOLOCK)on comp_id = bustypref_comp_id and  bustypref_journ_id = 0 ")
            End If



            ' sQuery.Append(" INNER JOIN aircraft WITH(NOLOCK) ON (ac_id = ffd_ac_id) AND ac_journ_id = 0  ")
            ' sQuery.Append(" INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id  ") 
            'If searchCriteria.ViewCriteriaCompanyID > 0 Or Trim(Operator_IDS_String) <> "" Then
            '  sQuery.Append(" INNER JOIN view_aircraft_company_flat WITH(NOLOCK) ON (ac_id = FFD2.ffd_ac_id) AND ac_journ_id = 0 and  cref_operator_flag in ('Y', 'O') ")
            '  sQuery.Append(" INNER JOIN Aircraft_Model WITH (NOLOCK) ON Aircraft_Model.amod_id = view_aircraft_company_flat.amod_id ")

            'Else
            '  sQuery.Append(" INNER JOIN aircraft_flat WITH(NOLOCK) ON (ac_id = FFD2.ffd_ac_id) AND ac_journ_id = 0 ")
            '  sQuery.Append(" INNER JOIN Aircraft_Model WITH (NOLOCK) ON Aircraft_Model.amod_id = aircraft_flat.amod_id ") 

            '  If Trim(from_spot) = "pdf2" Then
            '    sQuery.Append(" inner join Aircraft_Reference with (NOLOCK) on cref_ac_id = ac_id and cref_journ_id = ac_journ_id and cref_operator_flag IN ('Y', 'O')  ")
            '    sQuery.Append(" inner join Company with (NOLOCK) on comp_id = cref_comp_id and comp_journ_id = 0  ")
            '  End If
            'End If


            If searchCriteria.ViewCriteriaCompanyID > 0 Or Trim(Operator_IDS_String) <> "" Then
                'sQuery.Append(" INNER JOIN view_aircraft_company_flat WITH(NOLOCK) ON (ac_id = FFD2.ffd_ac_id) AND ac_journ_id = 0 and  cref_operator_flag in ('Y', 'O') ")
                ' sQuery.Append(" INNER JOIN Aircraft_Model WITH (NOLOCK) ON Aircraft_Model.amod_id = view_aircraft_company_flat.amod_id ")
            Else
                If Trim(from_spot) = "pdf2" Then
                    sQuery.Append(" inner join Aircraft_Reference with (NOLOCK) on cref_ac_id = ac_id and cref_operator_flag IN ('Y', 'O') and cref_journ_id = 0  ")   ' and cref_journ_id = ac_journ_id 
                    sQuery.Append(" inner join Company with (NOLOCK) on Company.comp_id = cref_comp_id and comp_journ_id = 0  ")
                End If
            End If


            '    sQuery.Append(" left outer join FAA_Blocked_Registration_Numbers with (NOLOCK) on ac_reg_no=faablk_reg_no ")

            If BasedAtAirport And (Airport_ID_OVERALL > 1 Or Trim(Airport_IDS_String) <> "") Then
                ' add it above 
                sQuery.Append(" WHERE aircraft.ac_journ_id = 0  ")   ' just to start the where clause 
            ElseIf String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentsStartDate) And String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentsEndDate) Then
                sQuery.Append(" WHERE (convert(date, ffd_date, 0) >= (getdate()-182)  ")
            Else
                sQuery.Append(" where (convert(date, ffd_date, 0) >= '" & Month(searchCriteria.ViewCriteriaDocumentsStartDate) & "/" & Day(searchCriteria.ViewCriteriaDocumentsStartDate) & "/" & Year(searchCriteria.ViewCriteriaDocumentsStartDate) & "') ")
                sQuery.Append(" AND (convert(date, ffd_date, 0) <= '" & Month(searchCriteria.ViewCriteriaDocumentsEndDate) & "/" & Day(searchCriteria.ViewCriteriaDocumentsEndDate) & "/" & Year(searchCriteria.ViewCriteriaDocumentsEndDate) & "') ")
            End If


            If BasedAtAirport And (Airport_ID_OVERALL > 1 Or Trim(Airport_IDS_String) <> "") Then
                If Not String.IsNullOrEmpty(Trim(Aircraft_IDS_String)) Then
                    If exclude_Aircraft = True Then
                        sQuery.Append(" and aircraft.ac_id not ")
                    Else
                        sQuery.Append(" and aircraft.ac_id ")
                    End If
                    sQuery.Append(" in (" & Aircraft_IDS_String & ") ")
                End If

            ElseIf Not String.IsNullOrEmpty(Trim(Aircraft_IDS_String)) Then
                If exclude_Aircraft = True Then
                    sQuery.Append(" and ac_id not ")
                Else
                    sQuery.Append(" and ac_id ")
                End If
                sQuery.Append(" in (" & Aircraft_IDS_String & ") ")
            End If

            'sQuery.Append(" AND (ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportIATA & "' or ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportICAO & "') ")


            If BasedAtAirport And (Airport_ID_OVERALL > 1 Or Trim(Airport_IDS_String) <> "") Then
                ' done in left outer join 
            ElseIf Trim(airport_direction) = "O" Then
                If Airport_ID_OVERALL > 1 Then
                    sQuery.Append(" AND (ffd_origin_aport_id = '" & Airport_ID_OVERALL & "') ")
                ElseIf Not String.IsNullOrEmpty(Airport_IDS_String) Then
                    If exclude_airport_check Then
                        sQuery.Append(" AND (ffd_origin_aport_id not in (" & Airport_IDS_String & ")) ")
                    Else
                        sQuery.Append(" AND (ffd_origin_aport_id in (" & Airport_IDS_String & ")) ")
                    End If
                End If
            ElseIf Trim(airport_direction) = "D" Then
                If Airport_ID_OVERALL > 1 Then
                    sQuery.Append(" AND (ffd_dest_aport_id = '" & Airport_ID_OVERALL & "') ")
                ElseIf Not String.IsNullOrEmpty(Airport_IDS_String) Then
                    If exclude_airport_check Then
                        sQuery.Append(" AND (ffd_dest_aport_id not in (" & Airport_IDS_String & ")) ")
                    Else
                        sQuery.Append(" AND (ffd_dest_aport_id in (" & Airport_IDS_String & ")) ")
                    End If
                End If
            ElseIf Trim(airport_direction) = "X" Then
                '  sQuery.Append(" and ffd_dest_aport_id > 0")
                ' sQuery.Append(" AND (ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportIATA & "' or ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportICAO & "')")

                If Airport_ID_OVERALL > 1 Then
                    sQuery.Append(" AND ((ffd_origin_aport_id = '" & Airport_ID_OVERALL & "') or (ffd_dest_aport_id = '" & Airport_ID_OVERALL & "')) ")
                ElseIf Not String.IsNullOrEmpty(Airport_IDS_String) Then
                    If exclude_airport_check Then
                        sQuery.Append(" AND (ffd_origin_aport_id not in (" & Airport_IDS_String & ")) AND (ffd_dest_aport_id not in (" & Airport_IDS_String & "))   ")
                    Else
                        sQuery.Append(" AND (   (ffd_origin_aport_id in (" & Airport_IDS_String & "))  or (ffd_dest_aport_id in (" & Airport_IDS_String & "))   )")
                    End If

                End If


            Else
                If Airport_ID_OVERALL > 1 Then
                    sQuery.Append(" AND (ffd_dest_aport_id = '" & Airport_ID_OVERALL & "') ")
                ElseIf Not String.IsNullOrEmpty(Airport_IDS_String) Then
                    If exclude_airport_check Then
                        sQuery.Append(" AND (ffd_dest_aport_id not in (" & Airport_IDS_String & ")) ")
                    Else
                        sQuery.Append(" AND (ffd_dest_aport_id in (" & Airport_IDS_String & ")) ")
                    End If
                End If
            End If

            If BasedAtAirport And (Airport_ID_OVERALL > 1 Or Trim(Airport_IDS_String) <> "") Then
                ' dont think this one matters 
            ElseIf Trim(searchCriteria.ViewCriteriaContinent) = "International" Then
                sQuery.Append(" and (origin_aport_country not in ('United States', 'U.S.') or dest_aport_country not in ('United States', 'U.S.'))  ")
            End If


            If BasedAtAirport And Airport_ID_OVERALL > 1 Then
                sQuery.Append(" AND (ac_aport_id = '" & Airport_ID_OVERALL & "') ")
            End If

            If BasedAtAirport And Trim(Airport_IDS_String) <> "" Then
                sQuery.Append(" AND ac_aport_id in (" & Trim(Airport_IDS_String) & ") ")
            End If


            If searchCriteria.ViewCriteriaCompanyID > 0 Then
                sQuery.Append(" AND comp_id = " & searchCriteria.ViewCriteriaCompanyID & "")
            End If

            Dim temp_use As String = " "
            If searchCriteria.ViewCriteriaHasBusinessFlag = True Or searchCriteria.ViewCriteriaHasCommercialFlag = True Or searchCriteria.ViewCriteriaHasHelicopterFlag = True Then
                sQuery.Append(" and ( ")
            End If


            If BasedAtAirport And (Airport_ID_OVERALL > 1 Or Trim(Airport_IDS_String) <> "") Then

                If searchCriteria.ViewCriteriaHasBusinessFlag = True Then
                    If Trim(temp_use) = "" Then
                        temp_use = " or "
                    Else
                        sQuery.Append(temp_use)
                    End If
                    sQuery.Append(" aircraft.ac_product_business_flag = 'Y' ")
                End If

                If searchCriteria.ViewCriteriaHasCommercialFlag = True Then
                    If Trim(temp_use) = "" Then
                        temp_use = " or "
                    Else
                        sQuery.Append(temp_use)
                    End If
                    sQuery.Append(" aircraft.ac_product_commercial_flag = 'Y' ")
                End If

                If searchCriteria.ViewCriteriaHasHelicopterFlag = True Then
                    If Trim(temp_use) = "" Then
                        temp_use = " or "
                    Else
                        sQuery.Append(temp_use)
                    End If
                    sQuery.Append(" aircraft.ac_product_helicopter_flag = 'Y' ")
                End If

                If searchCriteria.ViewCriteriaHasBusinessFlag = True Or searchCriteria.ViewCriteriaHasCommercialFlag = True Or searchCriteria.ViewCriteriaHasHelicopterFlag = True Then
                    sQuery.Append(" ) ")
                End If

            Else

                If searchCriteria.ViewCriteriaHasBusinessFlag = True Then
                    If Trim(temp_use) = "" Then
                        temp_use = " or "
                    Else
                        sQuery.Append(temp_use)
                    End If
                    sQuery.Append(" ac_product_business_flag = 'Y' ")
                End If

                If searchCriteria.ViewCriteriaHasCommercialFlag = True Then
                    If Trim(temp_use) = "" Then
                        temp_use = " or "
                    Else
                        sQuery.Append(temp_use)
                    End If
                    sQuery.Append(" ac_product_commercial_flag = 'Y' ")
                End If

                If searchCriteria.ViewCriteriaHasHelicopterFlag = True Then
                    If Trim(temp_use) = "" Then
                        temp_use = " or "
                    Else
                        sQuery.Append(temp_use)
                    End If
                    sQuery.Append(" ac_product_helicopter_flag = 'Y' ")
                End If

                If searchCriteria.ViewCriteriaHasBusinessFlag = True Or searchCriteria.ViewCriteriaHasCommercialFlag = True Or searchCriteria.ViewCriteriaHasHelicopterFlag = True Then
                    sQuery.Append(" ) ")
                End If

            End If


            ' ADDED MSW - 3/10/20 TO BUILD IN DROP DOWN FOR IN OPERATION 
            If Not IsNothing(searchCriteria.viewCriteriaInOperation) Then
                If Trim(searchCriteria.viewCriteriaInOperation) <> "" Then
                    sQuery.Append(Build_In_Operation_String(searchCriteria))
                End If
            End If


            If Trim(distance_string) <> "" And Len(Trim(distance_string)) > 2 Then
                sQuery.Append(distance_string)
            End If


            If Trim(Operator_IDS_String) <> "" Then
                If exclude_check = True Then
                    sQuery.Append(" and comp_id not ")
                Else
                    sQuery.Append(" and comp_id ")
                End If
                sQuery.Append(" in (" & Operator_IDS_String & ") ")
            End If

            ' sQuery.Append(" and ffd_hide_flag= 'N'")


            If Trim(product_code_selection) <> "" Then
                If BasedAtAirport And (Airport_ID_OVERALL > 1 Or Trim(Airport_IDS_String) <> "") Then
                    sQuery.Append(Replace(Replace(Replace(Replace(product_code_selection, " amod_", " Aircraft_Model.amod_"), " ac_", " Aircraft.ac_"), "(amod_", "(Aircraft_Model.amod_"), "(ac_", "(Aircraft.ac_"))
                Else
                    sQuery.Append(product_code_selection)
                End If

            End If


            If BasedAtAirport And (Airport_ID_OVERALL > 1 Or Trim(Airport_IDS_String) <> "") Then
                sQuery.Append(" " & Replace(Replace(Replace(Replace(commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False), " amod_", " Aircraft_Model.amod_"), " ac_", " Aircraft.ac_"), "(amod_", "(Aircraft_Model.amod_"), "(ac_", "(Aircraft.ac_"))
            Else
                sQuery.Append(" " & commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))
            End If




            '-- SORT # 2 IS JUST BY MAKE AND MODEL NOT BY NUMBER OF FLIGHTS
            '' sQuery.Append(" order by amod_make_name, amod_model_name, amod_id ")


            If searchCriteria.ViewCriteriaAmodID > -1 Then
                If Trim(searchCriteria.ViewCriteriaAmodID.ToString) = "0" Then
                Else
                    sQuery.Append(crmWebClient.Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
                End If
            ElseIf Not IsNothing(searchCriteria.ViewCriteriaAmodIDArray) Then
                If BasedAtAirport And (Airport_ID_OVERALL > 1 Or Trim(Airport_IDS_String) <> "") Then
                    sQuery.Append(Replace(Replace(Replace(Replace(SetUpModelString(searchCriteria), " amod_", " Aircraft_Model.amod_"), " ac_", " Aircraft.ac_"), "(amod_", "(Aircraft_Model.amod_"), "(ac_", "(Aircraft.ac_"))
                Else
                    sQuery.Append(SetUpModelString(searchCriteria))
                End If


            ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
                If BasedAtAirport And (Airport_ID_OVERALL > 1 Or Trim(Airport_IDS_String) <> "") Then
                    sQuery.Append(Replace(Replace(Replace(Replace(SetUpMakeString(searchCriteria), " amod_", " Aircraft_Model.amod_"), " ac_", " Aircraft.ac_"), "(amod_", "(Aircraft_Model.amod_"), "(ac_", "(Aircraft.ac_"))
                Else
                    sQuery.Append(SetUpMakeString(searchCriteria))
                End If

            End If


            If Not IsNothing(searchCriteria.ViewCriteriaTypeIDArray) Then

                If BasedAtAirport And (Airport_ID_OVERALL > 1 Or Trim(Airport_IDS_String) <> "") Then
                    sQuery.Append(Replace(Replace(Replace(Replace(SetUpTypeString(searchCriteria), " amod_", " Aircraft_Model.amod_"), " ac_", " Aircraft.ac_"), "(amod_", "(Aircraft_Model.amod_"), "(ac_", "(Aircraft.ac_"))
                Else
                    sQuery.Append(SetUpTypeString(searchCriteria))
                End If

            End If


            If show_not_based = True Then
                sQuery.Append(" and ac_id not in ( ")
                sQuery.Append(" select distinct ac_id from View_Aircraft_Flat with (NOLOCK)  ")
                sQuery.Append(" where (ac_aport_iata_code = '" & searchCriteria.ViewCriteriaAirportIATA & "' or ac_aport_icao_code = '" & searchCriteria.ViewCriteriaAirportICAO & "') and ac_journ_id = 0) ")
            End If


            If BasedAtAirport And (Airport_ID_OVERALL > 1 Or Trim(Airport_IDS_String) <> "") Then
                sQuery.Append(" group by Aircraft_Model.amod_make_name, Aircraft_Model.amod_model_name, aircraft.ac_ser_no_full, base_aport_name, aircraft.ac_reg_no, faablk_reg_no, aircraft.ac_ser_no_sort, aircraft.ac_id ")
                '  If Trim(from_spot) = "pdf2" Then
                'sQuery.Append(", Company.comp_name")
                ' Else 
                sQuery.Append(", company.comp_name, company.comp_address1, company.comp_city,company.comp_state, company.comp_country, ")
                sQuery.Append(" company.comp_web_address, company.comp_email_address, Phone_Numbers.pnum_number_full , company.comp_id ")


                '   End If
            Else
                sQuery.Append(" group by amod_make_name, amod_model_name, ac_ser_no_full, base_aport_name, ac_reg_no, faablk_reg_no,ac_ser_no_sort, ac_id ")
                If Trim(from_spot) = "pdf2" And searchCriteria.ViewCriteriaCompanyID > 0 Then
                    sQuery.Append(", comp_name")
                ElseIf Trim(from_spot) = "pdf2" Then
                    sQuery.Append(", Company.comp_name")
                Else
                    sQuery.Append(", comp_name, comp_address1, comp_city, comp_state, comp_country, ")
                    sQuery.Append(" comp_web_address, comp_email_address, comp_off_phone, comp_id ")
                End If
            End If

            If BasedAtAirport And (Airport_ID_OVERALL > 1 Or Trim(Airport_IDS_String) <> "") Then
                If include_weight = "Y" Then
                    sQuery.Append(" , amjiqs_cat_desc ")
                End If
            Else
                sQuery.Append(" , amjiqs_cat_desc ")
            End If

            sQuery.Append(", cbus_name ")


            '  sQuery.Append(" , COMMERCIAL_FIELD ")  

            'If Trim(searchCriteria.ViewCriteriaCompanyID) = 0 And Trim(Operator_IDS_String) = "" Then
            '  If TotalFlights = 0 Or TotalFlights > 1000 Then
            '    sQuery.Append(" having(COUNT(ffd_unique_flight_id) > 1) ")
            '  End If
            'End If
            sQuery.Append(" order by COUNT(*) desc ")
            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "utilization_functions.vb", sQuery.ToString)


            ' HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />" & Date.Now & " - get_flight_activity_by_model(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
                aError = "Error in get_flight_activity_by_model load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_flight_activity_by_model(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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
    Public Function get_flight_activity_by_ac_pie(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal field_name As String, ByVal show_not_based As Boolean, ByVal product_code_selection As String, ByRef TotalFlights As Long, Optional ByVal from_spot As String = "", Optional ByRef BasedAtAirport As Boolean = False, Optional ByVal limitthisQuery As Integer = 0) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim temp_string As String = ""
        Dim sQuery_temp As New StringBuilder


        Try

            '-- ***************  UPPER RIGHT TAB 3 - TOP MODELS ************************
            '-- # FLIGHT ACTIVITY BY MODEL

            sQuery.Append(" SELECT DISTINCT ")
            If Trim(field_name) = "amod_number_of_passengers" Then
                sQuery.Append(" case when amod_number_of_passengers >= 50 then 50 else amod_number_of_passengers end as amod_number_of_passengers, ")
            ElseIf Trim(field_name) = "ac_reg_no" Then
                sQuery.Append(" top 15 " & field_name & ", ")
            Else
                sQuery.Append(" " & field_name & ", ")
            End If


            sQuery.Append(" count(*) AS tflights  ")



            ' if we have picked an airport and are saying where the ac is best, then get them all
            If BasedAtAirport And (Airport_ID_OVERALL > 1 Or Trim(Airport_IDS_String) <> "") Then
                sQuery.Append(" from Aircraft with (NOLOCK) ")
                sQuery.Append(" inner join Aircraft_Model with (NOLOCK) on ac_amod_id = amod_id  ")
                sQuery.Append(" left outer join view_flights on view_flights.ac_id = aircraft.ac_id ")

                If String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentsStartDate) And String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentsEndDate) Then
                    sQuery.Append(" and (convert(date, ffd_date, 0) >= (getdate()-182)  ")
                Else
                    sQuery.Append(" and (convert(date, ffd_date, 0) >= '" & Month(searchCriteria.ViewCriteriaDocumentsStartDate) & "/" & Day(searchCriteria.ViewCriteriaDocumentsStartDate) & "/" & Year(searchCriteria.ViewCriteriaDocumentsStartDate) & "') ")
                    sQuery.Append(" AND (convert(date, ffd_date, 0) <= '" & Month(searchCriteria.ViewCriteriaDocumentsEndDate) & "/" & Day(searchCriteria.ViewCriteriaDocumentsEndDate) & "/" & Year(searchCriteria.ViewCriteriaDocumentsEndDate) & "') ")
                End If



                If Trim(airport_direction) = "O" Then
                    If Airport_ID_OVERALL > 1 Then
                        sQuery.Append(" AND (ffd_origin_aport_id = '" & Airport_ID_OVERALL & "') ")
                    ElseIf Not String.IsNullOrEmpty(Airport_IDS_String) Then
                        If exclude_airport_check Then
                            sQuery.Append(" AND (ffd_origin_aport_id not in (" & Airport_IDS_String & ")) ")
                        Else
                            sQuery.Append(" AND (ffd_origin_aport_id in (" & Airport_IDS_String & ")) ")
                        End If
                    End If
                ElseIf Trim(airport_direction) = "D" Then
                    If Airport_ID_OVERALL > 1 Then
                        sQuery.Append(" AND (ffd_dest_aport_id = '" & Airport_ID_OVERALL & "') ")
                    ElseIf Not String.IsNullOrEmpty(Airport_IDS_String) Then
                        If exclude_airport_check Then
                            sQuery.Append(" AND (ffd_dest_aport_id not in (" & Airport_IDS_String & ")) ")
                        Else
                            sQuery.Append(" AND (ffd_dest_aport_id in (" & Airport_IDS_String & ")) ")
                        End If
                    End If
                ElseIf Trim(airport_direction) = "X" Then
                    '  sQuery.Append(" and ffd_dest_aport_id > 0")
                    ' sQuery.Append(" AND (ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportIATA & "' or ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportICAO & "')")

                    If Airport_ID_OVERALL > 1 Then
                        sQuery.Append(" AND ((ffd_origin_aport_id = '" & Airport_ID_OVERALL & "') or (ffd_dest_aport_id = '" & Airport_ID_OVERALL & "')) ")
                    ElseIf Not String.IsNullOrEmpty(Airport_IDS_String) Then
                        If exclude_airport_check Then
                            sQuery.Append(" AND (ffd_origin_aport_id not in (" & Airport_IDS_String & ")) AND (ffd_dest_aport_id not in (" & Airport_IDS_String & "))   ")
                        Else
                            sQuery.Append(" AND (   (ffd_origin_aport_id in (" & Airport_IDS_String & "))  or (ffd_dest_aport_id in (" & Airport_IDS_String & "))   )")
                        End If

                    End If


                Else
                    If Airport_ID_OVERALL > 1 Then
                        sQuery.Append(" AND (ffd_dest_aport_id = '" & Airport_ID_OVERALL & "') ")
                    ElseIf Not String.IsNullOrEmpty(Airport_IDS_String) Then
                        If exclude_airport_check Then
                            sQuery.Append(" AND (ffd_dest_aport_id not in (" & Airport_IDS_String & ")) ")
                        Else
                            sQuery.Append(" AND (ffd_dest_aport_id in (" & Airport_IDS_String & ")) ")
                        End If
                    End If
                End If

                sQuery.Append(" left outer  JOIN aircraft_reference WITH (NOLOCK) ON aircraft_reference.cref_ac_id = aircraft.ac_id and cref_journ_id = ac_journ_id and cref_operator_flag = 'Y' ")
                sQuery.Append(" left outer JOIN company with (NOLOCK) on  aircraft_reference.cref_comp_id =  company.comp_id and aircraft_reference.cref_journ_id = company.comp_journ_id  ")
                sQuery.Append(" left outer JOIN Phone_Numbers  with (NOLOCK)  ON pnum_comp_id = Company.comp_id and pnum_journ_id = comp_journ_id and pnum_type = 'Office' and pnum_contact_id = 0  ")


            Else
                sQuery.Append(" FROM view_flights WITH(NOLOCK) ")
                sQuery.Append(" inner join Aircraft_Model with (NOLOCK) on view_flights.amod_id = Aircraft_Model.amod_id ")

            End If


            If searchCriteria.ViewCriteriaCompanyID > 0 Or Trim(Operator_IDS_String) <> "" Then
                'sQuery.Append(" INNER JOIN view_aircraft_company_flat WITH(NOLOCK) ON (ac_id = FFD2.ffd_ac_id) AND ac_journ_id = 0 and  cref_operator_flag in ('Y', 'O') ")
                ' sQuery.Append(" INNER JOIN Aircraft_Model WITH (NOLOCK) ON Aircraft_Model.amod_id = view_aircraft_company_flat.amod_id ")
            Else
                If Trim(from_spot) = "pdf2" Then
                    sQuery.Append(" inner join Aircraft_Reference with (NOLOCK) on cref_ac_id = ac_id and cref_operator_flag IN ('Y', 'O') and cref_journ_id = 0  ")   ' and cref_journ_id = ac_journ_id 
                    sQuery.Append(" inner join Company with (NOLOCK) on Company.comp_id = cref_comp_id and comp_journ_id = 0  ")
                End If
            End If


            '    sQuery.Append(" left outer join FAA_Blocked_Registration_Numbers with (NOLOCK) on ac_reg_no=faablk_reg_no ")

            If BasedAtAirport And (Airport_ID_OVERALL > 1 Or Trim(Airport_IDS_String) <> "") Then
                ' add it above 
                sQuery.Append(" WHERE aircraft.ac_journ_id = 0  ")   ' just to start the where clause 
            ElseIf String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentsStartDate) And String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentsEndDate) Then
                sQuery.Append(" WHERE (convert(date, ffd_date, 0) >= (getdate()-182)  ")
            Else
                sQuery.Append(" where (convert(date, ffd_date, 0) >= '" & Month(searchCriteria.ViewCriteriaDocumentsStartDate) & "/" & Day(searchCriteria.ViewCriteriaDocumentsStartDate) & "/" & Year(searchCriteria.ViewCriteriaDocumentsStartDate) & "') ")
                sQuery.Append(" AND (convert(date, ffd_date, 0) <= '" & Month(searchCriteria.ViewCriteriaDocumentsEndDate) & "/" & Day(searchCriteria.ViewCriteriaDocumentsEndDate) & "/" & Year(searchCriteria.ViewCriteriaDocumentsEndDate) & "') ")
            End If


            If BasedAtAirport And (Airport_ID_OVERALL > 1 Or Trim(Airport_IDS_String) <> "") Then
                If Not String.IsNullOrEmpty(Trim(Aircraft_IDS_String)) Then
                    If exclude_Aircraft = True Then
                        sQuery.Append(" and aircraft.ac_id not ")
                    Else
                        sQuery.Append(" and aircraft.ac_id ")
                    End If
                    sQuery.Append(" in (" & Aircraft_IDS_String & ") ")
                End If

            ElseIf Not String.IsNullOrEmpty(Trim(Aircraft_IDS_String)) Then
                If exclude_Aircraft = True Then
                    sQuery.Append(" and ac_id not ")
                Else
                    sQuery.Append(" and ac_id ")
                End If
                sQuery.Append(" in (" & Aircraft_IDS_String & ") ")
            End If

            'sQuery.Append(" AND (ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportIATA & "' or ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportICAO & "') ")


            If BasedAtAirport And (Airport_ID_OVERALL > 1 Or Trim(Airport_IDS_String) <> "") Then
                ' done in left outer join 
            ElseIf Trim(airport_direction) = "O" Then
                If Airport_ID_OVERALL > 1 Then
                    sQuery.Append(" AND (ffd_origin_aport_id = '" & Airport_ID_OVERALL & "') ")
                ElseIf Not String.IsNullOrEmpty(Airport_IDS_String) Then
                    If exclude_airport_check Then
                        sQuery.Append(" AND (ffd_origin_aport_id not in (" & Airport_IDS_String & ")) ")
                    Else
                        sQuery.Append(" AND (ffd_origin_aport_id in (" & Airport_IDS_String & ")) ")
                    End If
                End If
            ElseIf Trim(airport_direction) = "D" Then
                If Airport_ID_OVERALL > 1 Then
                    sQuery.Append(" AND (ffd_dest_aport_id = '" & Airport_ID_OVERALL & "') ")
                ElseIf Not String.IsNullOrEmpty(Airport_IDS_String) Then
                    If exclude_airport_check Then
                        sQuery.Append(" AND (ffd_dest_aport_id not in (" & Airport_IDS_String & ")) ")
                    Else
                        sQuery.Append(" AND (ffd_dest_aport_id in (" & Airport_IDS_String & ")) ")
                    End If
                End If
            ElseIf Trim(airport_direction) = "X" Then
                '  sQuery.Append(" and ffd_dest_aport_id > 0")
                ' sQuery.Append(" AND (ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportIATA & "' or ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportICAO & "')")

                If Airport_ID_OVERALL > 1 Then
                    sQuery.Append(" AND ((ffd_origin_aport_id = '" & Airport_ID_OVERALL & "') or (ffd_dest_aport_id = '" & Airport_ID_OVERALL & "')) ")
                ElseIf Not String.IsNullOrEmpty(Airport_IDS_String) Then
                    If exclude_airport_check Then
                        sQuery.Append(" AND (ffd_origin_aport_id not in (" & Airport_IDS_String & ")) AND (ffd_dest_aport_id not in (" & Airport_IDS_String & "))   ")
                    Else
                        sQuery.Append(" AND (   (ffd_origin_aport_id in (" & Airport_IDS_String & "))  or (ffd_dest_aport_id in (" & Airport_IDS_String & "))   )")
                    End If

                End If


            Else
                If Airport_ID_OVERALL > 1 Then
                    sQuery.Append(" AND (ffd_dest_aport_id = '" & Airport_ID_OVERALL & "') ")
                ElseIf Not String.IsNullOrEmpty(Airport_IDS_String) Then
                    If exclude_airport_check Then
                        sQuery.Append(" AND (ffd_dest_aport_id not in (" & Airport_IDS_String & ")) ")
                    Else
                        sQuery.Append(" AND (ffd_dest_aport_id in (" & Airport_IDS_String & ")) ")
                    End If
                End If
            End If

            If BasedAtAirport And (Airport_ID_OVERALL > 1 Or Trim(Airport_IDS_String) <> "") Then
                ' dont think this one matters 
            ElseIf Trim(searchCriteria.ViewCriteriaContinent) = "International" Then
                sQuery.Append(" and (origin_aport_country not in ('United States', 'U.S.') or dest_aport_country not in ('United States', 'U.S.'))  ")
            End If


            If BasedAtAirport And Airport_ID_OVERALL > 1 Then
                sQuery.Append(" AND (ac_aport_id = '" & Airport_ID_OVERALL & "') ")
            End If

            If BasedAtAirport And Trim(Airport_IDS_String) <> "" Then
                sQuery.Append(" AND ac_aport_id in (" & Trim(Airport_IDS_String) & ") ")
            End If


            If searchCriteria.ViewCriteriaCompanyID > 0 Then
                sQuery.Append(" AND comp_id = " & searchCriteria.ViewCriteriaCompanyID & "")
            End If

            Dim temp_use As String = " "
            If searchCriteria.ViewCriteriaHasBusinessFlag = True Or searchCriteria.ViewCriteriaHasCommercialFlag = True Or searchCriteria.ViewCriteriaHasHelicopterFlag = True Then
                sQuery.Append(" and ( ")
            End If


            If BasedAtAirport And (Airport_ID_OVERALL > 1 Or Trim(Airport_IDS_String) <> "") Then

                If searchCriteria.ViewCriteriaHasBusinessFlag = True Then
                    If Trim(temp_use) = "" Then
                        temp_use = " or "
                    Else
                        sQuery.Append(temp_use)
                    End If
                    sQuery.Append(" aircraft.ac_product_business_flag = 'Y' ")
                End If

                If searchCriteria.ViewCriteriaHasCommercialFlag = True Then
                    If Trim(temp_use) = "" Then
                        temp_use = " or "
                    Else
                        sQuery.Append(temp_use)
                    End If
                    sQuery.Append(" aircraft.ac_product_commercial_flag = 'Y' ")
                End If

                If searchCriteria.ViewCriteriaHasHelicopterFlag = True Then
                    If Trim(temp_use) = "" Then
                        temp_use = " or "
                    Else
                        sQuery.Append(temp_use)
                    End If
                    sQuery.Append(" aircraft.ac_product_helicopter_flag = 'Y' ")
                End If

                If searchCriteria.ViewCriteriaHasBusinessFlag = True Or searchCriteria.ViewCriteriaHasCommercialFlag = True Or searchCriteria.ViewCriteriaHasHelicopterFlag = True Then
                    sQuery.Append(" ) ")
                End If

            Else

                If searchCriteria.ViewCriteriaHasBusinessFlag = True Then
                    If Trim(temp_use) = "" Then
                        temp_use = " or "
                    Else
                        sQuery.Append(temp_use)
                    End If
                    sQuery.Append(" ac_product_business_flag = 'Y' ")
                End If

                If searchCriteria.ViewCriteriaHasCommercialFlag = True Then
                    If Trim(temp_use) = "" Then
                        temp_use = " or "
                    Else
                        sQuery.Append(temp_use)
                    End If
                    sQuery.Append(" ac_product_commercial_flag = 'Y' ")
                End If

                If searchCriteria.ViewCriteriaHasHelicopterFlag = True Then
                    If Trim(temp_use) = "" Then
                        temp_use = " or "
                    Else
                        sQuery.Append(temp_use)
                    End If
                    sQuery.Append(" ac_product_helicopter_flag = 'Y' ")
                End If

                If searchCriteria.ViewCriteriaHasBusinessFlag = True Or searchCriteria.ViewCriteriaHasCommercialFlag = True Or searchCriteria.ViewCriteriaHasHelicopterFlag = True Then
                    sQuery.Append(" ) ")
                End If

            End If

            'added MSW -5/6/19
            If searchCriteria.ViewCriteriaCompanyID > 0 And searchCriteria.ViewCriteriaAmodID > 0 Then
                sQuery.Append(" AND aircraft_model.amod_id = " & searchCriteria.ViewCriteriaAmodID & "")
            ElseIf Not IsNothing(searchCriteria.ViewCriteriaAmodIDArray) Then
                If searchCriteria.ViewCriteriaCompanyID > 0 Then
                    sQuery.Append(SetUpModelString(searchCriteria, True))
                Else
                    sQuery.Append(SetUpModelString(searchCriteria, True))
                End If
            ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake) Then
                If searchCriteria.ViewCriteriaCompanyID > 0 Then
                    sQuery.Append(SetUpMakeString(searchCriteria, True))
                Else
                    sQuery.Append(SetUpMakeString(searchCriteria, True))
                End If
            End If

            If Not IsNothing(searchCriteria.ViewCriteriaTypeIDArray) Then
                sQuery.Append(SetUpTypeString(searchCriteria, True))
            End If

            If Trim(distance_string) <> "" And Len(Trim(distance_string)) > 2 Then
                sQuery.Append(distance_string)
            End If


            If Trim(Operator_IDS_String) <> "" Then
                If exclude_check = True Then
                    sQuery.Append(" and comp_id not ")
                Else
                    sQuery.Append(" and comp_id ")
                End If
                sQuery.Append(" in (" & Operator_IDS_String & ") ")
            End If

            ' sQuery.Append(" and ffd_hide_flag= 'N'")


            If Trim(product_code_selection) <> "" Then
                If BasedAtAirport And (Airport_ID_OVERALL > 1 Or Trim(Airport_IDS_String) <> "") Then
                    sQuery.Append(Replace(Replace(Replace(Replace(product_code_selection, " amod_", " Aircraft_Model.amod_"), " ac_", " Aircraft.ac_"), "(amod_", "(Aircraft_Model.amod_"), "(ac_", "(Aircraft.ac_"))
                Else
                    sQuery.Append(product_code_selection)
                End If

            End If


            If BasedAtAirport And (Airport_ID_OVERALL > 1 Or Trim(Airport_IDS_String) <> "") Then
                sQuery.Append(" " & Replace(Replace(Replace(Replace(commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False), " amod_", " Aircraft_Model.amod_"), " ac_", " Aircraft.ac_"), "(amod_", "(Aircraft_Model.amod_"), "(ac_", "(Aircraft.ac_"))
            Else
                sQuery.Append(" " & Replace(commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False), "amod_", "Aircraft_Model.amod_"))
            End If




            '-- SORT # 2 IS JUST BY MAKE AND MODEL NOT BY NUMBER OF FLIGHTS
            '' sQuery.Append(" order by amod_make_name, amod_model_name, amod_id ")

            If searchCriteria.ViewCriteriaAmodID > -1 Then
                If Trim(searchCriteria.ViewCriteriaAmodID.ToString) = "0" Then
                Else
                    sQuery.Append(crmWebClient.Constants.cAndClause + " aircraft_model.amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
                End If
            ElseIf Not IsNothing(searchCriteria.ViewCriteriaAmodIDArray) Then
                sQuery.Append(SetUpModelString(searchCriteria, True))
            ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
                sQuery.Append(SetUpMakeString(searchCriteria, True))
            End If


            If Not IsNothing(searchCriteria.ViewCriteriaTypeIDArray) Then
                sQuery.Append(SetUpTypeString(searchCriteria, True))
            End If


            If show_not_based = True Then
                sQuery.Append(" and ac_id not in ( ")
                sQuery.Append(" select distinct ac_id from View_Aircraft_Flat with (NOLOCK)  ")
                sQuery.Append(" where (ac_aport_iata_code = '" & searchCriteria.ViewCriteriaAirportIATA & "' or ac_aport_icao_code = '" & searchCriteria.ViewCriteriaAirportICAO & "') and ac_journ_id = 0) ")
            End If



            If Trim(field_name) = "amod_number_of_passengers" Then
                sQuery.Append(" group by case when amod_number_of_passengers >= 50 then 50 else amod_number_of_passengers end ")
            Else
                sQuery.Append(" group by " & field_name & " ")
            End If

            If Trim(field_name) = "ac_reg_no" Then
                sQuery.Append(" order by  count(*) desc ")
            Else
                sQuery.Append(" order by " & field_name & " asc ")
            End If

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "utilization_functions.vb", sQuery.ToString)


            ' HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />" & Date.Now & " - get_flight_activity_by_model(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
                aError = "Error in get_flight_activity_by_model load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_flight_activity_by_model(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

    Public Function GET_USER_AIRPORTS(ByVal aport_ids As String, ByVal selected_value As String, ByVal latest_faa_date As String) As DataTable

        Dim temptable As New DataTable
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader : SqlReader = Nothing
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim sql As String = ""
        Dim old_date As String = ""
        Dim mid_date As String = ""

        Try


            If Trim(selected_value) = "365" Or Trim(selected_value) = "" Then
                If Trim(latest_faa_date) <> "" Then
                    old_date = DateAdd(DateInterval.Year, -2, CDate(latest_faa_date))
                    old_date = DateAdd(DateInterval.Day, 1, CDate(old_date))
                    mid_date = DateAdd(DateInterval.Year, -1, CDate(latest_faa_date))
                Else
                    old_date = DateAdd(DateInterval.Year, -2, CDate(Date.Now.Date))
                    old_date = DateAdd(DateInterval.Day, 1, CDate(old_date))
                    mid_date = DateAdd(DateInterval.Year, -1, CDate(Date.Now.Date))
                End If
            Else
                If Trim(latest_faa_date) <> "" Then
                    mid_date = DateAdd(DateInterval.Year, -1, CDate(latest_faa_date))
                    old_date = CDate("1/1/" & Year(mid_date))
                Else
                    mid_date = DateAdd(DateInterval.Year, -1, CDate(Date.Now.Date))
                    old_date = CDate("1/1/" & Year(mid_date))
                End If
            End If


            'view_reports_label
            ' sql = " select DISTINCT top 100 aport_id, "
            ' sql = sql & " aport_iata_code as IATA, aport_icao_code as ICAO,"
            ' sql = sql & " aport_name, aport_country, aport_city, aport_state,"

            sql = sql & " SELECT aport_id As APortId, "
            sql = sql & " COALESCE(aport_iata_code,'') As IATACode, "
            sql = sql & " COALESCE(aport_icao_code,'') As ICAOCode,"
            sql = sql & " COALESCE(aport_faaid_code,'') As FAAIdCode,"
            sql = sql & " COALESCE(aport_name,'') As APortName,"
            sql = sql & " COALESCE(aport_city,'') As APortCity,"
            sql = sql & " COALESCE(aport_state,'') As APortState,"
            sql = sql & " COALESCE(aport_country,'') As APortCountry,"



            'If Trim(selected_value) = "365" Or Trim(selected_value) = "" Then
            '  sql = sql & " (select count(*) from FAA_Flight_Data WITH(NOLOCK) "
            '  sql = sql & " where ffd_dest_aport_id=aport_id and ffd_hide_flag='N' and ffd_date > ('" & old_date & "') and ffd_date <= ('" & mid_date & "') ) as previousperiod, "
            '  sql = sql & " (select count(*) from FAA_Flight_Data WITH(NOLOCK) "
            '  sql = sql & " where ffd_dest_aport_id=aport_id and ffd_hide_flag='N' and ffd_date > ('" & mid_date & "')) as currentperiod "
            'Else
            '  sql = sql & " (select count(*) from FAA_Flight_Data WITH(NOLOCK) "
            '  sql = sql & " where ffd_dest_aport_id=aport_id and ffd_hide_flag='N' and month(ffd_date) <=  month('" & latest_faa_date & "') and year(ffd_date) = (YEAR('" & latest_faa_date & "') - 1) ) as previousperiod, "
            '  sql = sql & " (select count(*) from FAA_Flight_Data WITH(NOLOCK) "
            '  sql = sql & " where ffd_dest_aport_id=aport_id and ffd_hide_flag='N' and month(ffd_date) <=  month('" & latest_faa_date & "') and year(ffd_date) = YEAR('" & latest_faa_date & "')) as currentperiod "
            'End If

            'Edits: 10/29/15: Amanda.
            'The multiple groups of two subqueries down below have been changed per instruction 
            'to use the aircraft flat table instead of the join to the aircraft and aircraft model.
            If Trim(selected_value) = "365" Or Trim(selected_value) = "" Then
                sql = sql & " (SELECT COUNT(ffd_unique_flight_id) "
                sql = sql & " FROM FAA_Flight_Data WITH (NOLOCK)"

                'sql = sql & " INNER JOIN Aircraft WITH (NOLOCK) ON ac_id = ffd_ac_id AND ac_journ_id = 0"
                'sql = sql & " INNER JOIN Aircraft_Model WITH (NOLOCK) ON amod_id = ac_amod_id"
                sql = sql & " INNER JOIN Aircraft_Flat WITH (NOLOCK) ON ac_id = ffd_ac_id AND ac_journ_id = 0 "

                sql = sql & " WHERE(ffd_dest_aport_id = aport_id)"
                sql = sql & " AND (ffd_hide_flag = 'N')"
                sql = sql & " AND (ffd_date BETWEEN '" & old_date & "' AND '" & mid_date & "')"
                sql = sql & commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False)
                sql = sql & " ) As previousperiod,"

                mid_date = DateAdd(DateInterval.Day, 1, CDate(mid_date))

                sql = sql & " (SELECT COUNT(ffd_unique_flight_id) "
                sql = sql & " FROM FAA_Flight_Data WITH (NOLOCK)"

                'sql = sql & " INNER JOIN Aircraft WITH (NOLOCK) ON ac_id = ffd_ac_id AND ac_journ_id = 0"
                'sql = sql & " INNER JOIN Aircraft_Model WITH (NOLOCK) ON amod_id = ac_amod_id"
                sql = sql & " INNER JOIN Aircraft_Flat WITH (NOLOCK) ON ac_id = ffd_ac_id AND ac_journ_id = 0 "

                sql = sql & " WHERE(ffd_dest_aport_id = aport_id)"
                sql = sql & " AND (ffd_hide_flag = 'N')"
                sql = sql & " AND (ffd_date BETWEEN '" & mid_date & "' AND '" & latest_faa_date & "')"
                sql = sql & commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False)
                sql = sql & " ) As currentperiod "
            Else
                sql = sql & " (SELECT COUNT(ffd_unique_flight_id) "
                sql = sql & " FROM FAA_Flight_Data WITH (NOLOCK)"

                'sql = sql & " INNER JOIN Aircraft WITH (NOLOCK) ON ac_id = ffd_ac_id AND ac_journ_id = 0"
                'sql = sql & " INNER JOIN Aircraft_Model WITH (NOLOCK) ON amod_id = ac_amod_id"
                sql = sql & " INNER JOIN Aircraft_Flat WITH (NOLOCK) ON ac_id = ffd_ac_id AND ac_journ_id = 0 "

                sql = sql & " WHERE(ffd_dest_aport_id = aport_id)"
                sql = sql & " AND (ffd_hide_flag = 'N')"
                sql = sql & " AND (ffd_date BETWEEN '" & old_date & "' AND '" & mid_date & "')"
                sql = sql & commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False)
                sql = sql & " ) As previousperiod,"

                old_date = DateAdd(DateInterval.Year, 1, CDate(old_date)) ' now will be current year

                sql = sql & " (SELECT COUNT(ffd_unique_flight_id) "
                sql = sql & " FROM FAA_Flight_Data WITH (NOLOCK)"

                'sql = sql & " INNER JOIN Aircraft WITH (NOLOCK) ON ac_id = ffd_ac_id AND ac_journ_id = 0"
                'sql = sql & " INNER JOIN Aircraft_Model WITH (NOLOCK) ON amod_id = ac_amod_id"
                sql = sql & " INNER JOIN Aircraft_Flat WITH (NOLOCK) ON ac_id = ffd_ac_id AND ac_journ_id = 0 "

                sql = sql & " WHERE(ffd_dest_aport_id = aport_id)"
                sql = sql & " AND (ffd_hide_flag = 'N')"
                sql = sql & " AND (ffd_date BETWEEN '" & old_date & "' AND '" & latest_faa_date & "')"
                sql = sql & commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False)
                sql = sql & " ) As currentperiod "
            End If





            sql = sql & " FROM Airport WITH(NOLOCK) "
            sql = sql & " WHERE aport_id in (" & aport_ids & ") AND (aport_active_flag = 'Y') "
            'sql = sql & " group by aport_id, aport_iata_code, aport_icao_code,"
            ' sql = sql & " aport_name, aport_country, aport_city, aport_state "
            ' sql = sql & " order by aport_name asc "


            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>GET_USER_AIRPORTS() As DataTable: </b><br />" + sql

            SqlConn.ConnectionString = clientConnectString
            SqlConn.Open()
            SqlCommand.Connection = SqlConn

            SqlCommand.CommandText = sql
            SqlCommand.CommandTimeout = 60
            SqlCommand.CommandType = CommandType.Text
            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                temptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
            End Try

        Catch ex As Exception
            Return Nothing
            Me.class_error = "Error in ListOfActiveAirportsControlled() As DataTable: " + ex.Message
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
    Public Function GetRefuel(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal product_code_selection As String, ByRef TotalFlights As Long, ByVal percentage As Integer, ByVal minutes As Integer, Optional ByVal is_from As String = "", Optional ByVal limitthisquery As Integer = 0) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim temp_string As String = ""
        Dim sQuery_temp As New StringBuilder
        Dim tstart As String = ""
        Dim tend As String = ""


        Try

            If Trim(searchCriteria.ViewCriteriaDocumentsStartDate) <> "" Then
                tstart = DateAdd(DateInterval.Day, -1, CDate(searchCriteria.ViewCriteriaDocumentsStartDate))
            End If

            If Trim(searchCriteria.ViewCriteriaDocumentsEndDate) <> "" Then
                tend = DateAdd(DateInterval.Day, 1, CDate(searchCriteria.ViewCriteriaDocumentsEndDate))
            End If

            sQuery.Append(" select distinct ")

            If limitthisquery > 0 Then
                sQuery.Append(" top " & limitthisquery & " ")
            End If

            sQuery.Append(" ac_id As ACId, amod_make_name As Make, amod_model_name As Model, ac_ser_no_full as SERNO_NONDISPLAY, ")
            sQuery.Append(" ac_ser_no_sort as SERNOSORT_NONDISPLAY, ac_reg_no as REGNO_NONDISPLAY, faablk_reg_no, ")
            sQuery.Append("  ac_ser_no_full as SerNbr, ")
            sQuery.Append("  ac_reg_no  as RegNbr,  ")
            sQuery.Append(" base_aport_name AS BaseAirport ,  base_aport_iata_code as BaseIATA , ")
            '-- DEPARTED
            sQuery.Append("FFD2.ffd_origin_date AS L1DEPARTED, ffd_origin_aport_id AS L1DEPARTID,ffd_origin_aport As L1DEPARTIATA, ")
            sQuery.Append("origin_aport_name AS L1DEPARTAPPORT, origin_aport_country AS L1DEPARTCOUNTRY, origin_aport_city AS L1DEPARTCITY,  ")
            sQuery.Append("origin_aport_state AS L1DEPARTSTATE, origin_aport_icao_code as ORIGINICAO, origin_aport_iata_code as ORIGINIATA, ")
            sQuery.Append("FFD2.ffd_dest_date AS L1ARRIVED, ffd_dest_aport_id AS L1ARRIVEDID,ffd_dest_aport As L1ARRIVEDIATA,  ")
            sQuery.Append("dest_aport_name AS L1ARRIVEDAPORT,  ")
            sQuery.Append("dest_aport_country as L1ARRIVEDCOUNTRY, dest_aport_city as L1ARRIVEDCITY, dest_aport_state as L1ARRIVEDSTATE, dest_aport_icao_code as DESTICAO, dest_aport_iata_code as DESTIATA,  ")
            sQuery.Append("ffd_flight_time As L1FLIGHTTIME, ffd_distance As L1DISTANCE, ")

            sQuery.Append(" ffd_unique_flight_id as Flight_Id1, ")


            If Trim(is_from) = "pdf" Then
                sQuery.Append(" ((ffd_flight_time * FFD2.amod_fuel_burn_rate)/60) as TotalFuelBurn1, ")
                sQuery.Append(" comp_name, comp_country, ")
            End If

            '-- GET STRING OF DISTANCE AND AIRPORT OF SECOND FLIGHT WITH TIME ON GROUND
            sQuery.Append(" (select top 1 ('ONGROUND:' + cast(DATEDIFF(mi, FFD2.ffd_dest_date, f2.ffd_origin_date) as varchar(100)) +',L2DEPARTED:'+CAST(case when cast(f2.ffd_origin_date as  varchar(100)) is null then '' else  cast(f2.ffd_origin_date as varchar(100))  end   AS VARCHAR(100))  + ',L2DISTANCE:'+convert(varchar,f2.ffd_distance)+',L2ARRIVED:'+CAST(case when cast(f2.ffd_dest_date as  varchar(100)) is null then '' else  cast(f2.ffd_dest_date as varchar(100))  end   AS VARCHAR(100))  + ',L2ARRIVEAPORT:'+f2.ffd_dest_aport+'-'+ replace(d.aport_name, ',', '')   ")

            'these 3 added in by MSW - 9/25/18
            sQuery.Append(" + ',L2ARRIVECITY:'+  case when d.aport_city is null then '' else replace(d.aport_city, ',', '') end  + ")
            sQuery.Append(" ',L2ARRIVESTATE:'+  case when d.aport_state is null then '' else replace(d.aport_state, ',', '')  end   +")
            sQuery.Append(" ',L2ARRIVCOUNTRY:'+   case when d.aport_country is null then '' else  replace(d.aport_country, ',', '')  end  + ")
            sQuery.Append(" ',L2ARRIVEIATA:'+   case when d.aport_iata_code is null then '' else d.aport_iata_code end    + ")
            sQuery.Append(" ',L2ARRIVEICAO:'+    case when d.aport_icao_code is null then '' else d.aport_icao_code end + ")

            sQuery.Append(" ', L2ARRIVEFLIGHTID:'+   case when ffd_unique_flight_id is null then '' else  replace(ffd_unique_flight_id, ',', '')  end + ")
            sQuery.Append("  ',L2ARRIVEDESTID:'+   case when ffd_dest_aport_id is null then '' else  replace(ffd_dest_aport_id, ',', '')  end ")


            sQuery.Append(" )  from FAA_Flight_Data f2 with (NOLOCK)   ")
            sQuery.Append(" inner join Airport d with (NOLOCK) on f2.ffd_dest_aport_id=d.aport_id  ")
            sQuery.Append(" where(f2.ffd_ac_id = ffd2.ac_id And f2.ffd_unique_flight_id <> ffd2.ffd_unique_flight_id)  ")
            '  -- dest date of original/landing and origin date of second
            '-- and the difference in time
            sQuery.Append(" and (DATEDIFF(mi, FFD2.ffd_dest_date, f2.ffd_origin_date)) < " & minutes & " ")
            sQuery.Append(" and (DATEDIFF(mi, FFD2.ffd_dest_date, f2.ffd_origin_date)) > 0   ")
            sQuery.Append(" and   (f2.ffd_origin_aport_id = ffd2.ffd_dest_aport_id) ")
            sQuery.Append(" )")
            sQuery.Append(" as SECONDFLIGHT, ")


            If Trim(is_from) = "pdf" Then
                sQuery.Append(" (select ((sum(f2.ffd_flight_time) * FFD2.amod_fuel_burn_rate)/60) from FAA_Flight_Data f2 with (NOLOCK)   ")
                sQuery.Append(" inner join Airport d with (NOLOCK) on f2.ffd_dest_aport_id=d.aport_id  ")
                sQuery.Append(" where(f2.ffd_ac_id = ffd2.ac_id And f2.ffd_unique_flight_id <> ffd2.ffd_unique_flight_id)  ")
                '  -- dest date of original/landing and origin date of second
                '-- and the difference in time
                sQuery.Append(" and (DATEDIFF(mi, FFD2.ffd_dest_date, f2.ffd_origin_date)) < " & minutes & " ")
                sQuery.Append(" and (DATEDIFF(mi, FFD2.ffd_dest_date, f2.ffd_origin_date)) > 0   ")
                sQuery.Append(" and   (f2.ffd_origin_aport_id = ffd2.ffd_dest_aport_id) ")
                sQuery.Append(" )")
                sQuery.Append(" as SECONDFUELBURN, ")
            End If

            sQuery.Append(" amod_max_range_miles AS MODELRANGE ")

            sQuery.Append(", origin_continent, dest_continent, amjiqs_cat_desc  ")

            sQuery.Append("  FROM view_flights FFD2 WITH(NOLOCK)  ")
            '   sQuery.Append(" INNER JOIN aircraft  WITH(NOLOCK) ON (ac_id = ffd_ac_id) AND ac_journ_id = 0  ")
            '   sQuery.Append(" left outer JOIN Airport WITH(NOLOCK) ON ffd_origin_aport_id = aport_id  ")
            '   sQuery.Append(" left outer JOIN Airport a2 WITH(NOLOCK) ON ffd_dest_aport_id = a2.aport_id  ")
            '   sQuery.Append(" left outer join FAA_Blocked_Registration_Numbers with (NOLOCK) on ac_reg_no=faablk_reg_no ")
            '   sQuery.Append(" inner join Aircraft_Model with (NOLOCK) on Aircraft_Model.amod_id =  ac_amod_id ")

            '  If Operator_IDS_String <> "" Or rollup_text <> "" Or searchCriteria.ViewCriteriaCompanyID > 0 Then
            '    sQuery.Append(" inner join aircraft_reference on ac_id = cref_ac_id and ac_journ_id = cref_journ_id ")
            '  End If

            '-- *************** MAIN WHERE CLAUSE ******************************
            sQuery.Append(" WHERE ")

            If String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentsStartDate) And String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentsEndDate) Then
                sQuery.Append("  convert(date, ffd_date, 0) >= (getdate()-10)  ")
            Else
                sQuery.Append("  ((convert(date, ffd_date, 0) >= '" & Month(searchCriteria.ViewCriteriaDocumentsStartDate) & "/" & Day(searchCriteria.ViewCriteriaDocumentsStartDate) & "/" & Year(searchCriteria.ViewCriteriaDocumentsStartDate) & "') ")
                sQuery.Append(" AND ( convert(date, ffd_date, 0) <= '" & Month(searchCriteria.ViewCriteriaDocumentsEndDate) & "/" & Day(searchCriteria.ViewCriteriaDocumentsEndDate) & "/" & Year(searchCriteria.ViewCriteriaDocumentsEndDate) & "')) ")
            End If


            ' -- AIRPORT SELECTION OR FOLDER
            If Airport_ID_OVERALL > 1 Then
                sQuery.Append(" AND (ffd_dest_aport_id = '" & Airport_ID_OVERALL & "') ")
            ElseIf Not String.IsNullOrEmpty(Airport_IDS_String) Then
                If exclude_airport_check Then
                    sQuery.Append(" AND (ffd_dest_aport_id not in (" & Airport_IDS_String & ")) ")
                Else
                    sQuery.Append(" AND (ffd_dest_aport_id in (" & Airport_IDS_String & ")) ")
                End If
            End If

            Dim temp_use As String = " "
            If searchCriteria.ViewCriteriaHasBusinessFlag = True Or searchCriteria.ViewCriteriaHasCommercialFlag = True Or searchCriteria.ViewCriteriaHasHelicopterFlag = True Then
                sQuery.Append(" and ( ")
            End If

            If searchCriteria.ViewCriteriaHasBusinessFlag = True Then
                If Trim(temp_use) = "" Then
                    temp_use = " or "
                Else
                    sQuery.Append(temp_use)
                End If
                sQuery.Append(" ac_product_business_flag = 'Y' ")
            End If

            If searchCriteria.ViewCriteriaHasCommercialFlag = True Then
                If Trim(temp_use) = "" Then
                    temp_use = " or "
                Else
                    sQuery.Append(temp_use)
                End If
                sQuery.Append(" ac_product_commercial_flag = 'Y' ")
            End If

            If searchCriteria.ViewCriteriaHasHelicopterFlag = True Then
                If Trim(temp_use) = "" Then
                    temp_use = " or "
                Else
                    sQuery.Append(temp_use)
                End If
                sQuery.Append(" ac_product_helicopter_flag = 'Y' ")
            End If

            If searchCriteria.ViewCriteriaHasBusinessFlag = True Or searchCriteria.ViewCriteriaHasCommercialFlag = True Or searchCriteria.ViewCriteriaHasHelicopterFlag = True Then
                sQuery.Append(" ) ")
            End If


            ' ADDED MSW - 3/10/20 TO BUILD IN DROP DOWN FOR IN OPERATION 
            If Not IsNothing(searchCriteria.viewCriteriaInOperation) Then
                If Trim(searchCriteria.viewCriteriaInOperation) <> "" Then
                    sQuery.Append(Build_In_Operation_String(searchCriteria))
                End If
            End If


            '-- AIRCRAFT SELECTION
            If Trim(product_code_selection) <> "" Then
                sQuery.Append(product_code_selection)
            Else
            End If

            If Trim(distance_string) <> "" And Len(Trim(distance_string)) > 2 Then
                sQuery.Append(distance_string)
            End If


            sQuery.Append(" " & commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))


            ' -- ***************** ADDITIONAL CRITERIA FOR REFUEL ************************************
            sQuery.Append(" and ")
            sQuery.Append(" ( ")
            '-- AND MY ARRIVAL FLIGHT WAS A DISTANCE OF AT LEAST 60% OF MY MAX RANGE FOR THE AIRCRAFT 
            sQuery.Append(" (ffd_distance > 0 and ffd_distance > (amod_max_range_miles * ." & percentage & ") ")
            sQuery.Append(" and  ")
            '-- AND MY AIRCRAFT WAS ON THE GROUND FOR LESS THAN 90 MINUTES BEFORE TAKING OFF
            sQuery.Append(" exists( ")
            sQuery.Append(" (select top 1 f2.ffd_distance from FAA_Flight_Data f2 with (NOLOCK)  ")
            sQuery.Append(" INNER JOIN aircraft  a2 WITH(NOLOCK) ON (a2.ac_id = ffd_ac_id) AND a2.ac_journ_id = 0  ")
            sQuery.Append(" inner join Aircraft_Model am2 with (NOLOCK) on am2.amod_id =  a2.ac_amod_id ")
            sQuery.Append("  where(f2.ffd_ac_id = ffd2.ac_id And f2.ffd_unique_flight_id <> ffd2.ffd_unique_flight_id) ")

            ' ADDED IN MSW 
            If String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentsStartDate) And String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentsEndDate) Then
                sQuery.Append("  AND convert(date, ffd_date, 0) >= (getdate()-10)  ")
            Else
                sQuery.Append(" AND ((convert(date, ffd_date, 0) >= '" & Month(tstart) & "/" & Day(tstart) & "/" & Year(tstart) & "') ")
                sQuery.Append(" AND ( convert(date, ffd_date, 0) <= '" & Month(tend) & "/" & Day(tend) & "/" & Year(tend) & "')) ")
            End If


            '-- dest date of original/landing and origin date of second
            '-- and the difference in time is less than 90 minutes
            sQuery.Append(" and (DATEDIFF(mi, FFD2.ffd_dest_date, f2.ffd_origin_date)) < " & minutes & " ")
            sQuery.Append(" and (DATEDIFF(mi, FFD2.ffd_dest_date, f2.ffd_origin_date)) > 0  ")
            '  -- AND MY AIRPORT  
            sQuery.Append("  and   (f2.ffd_origin_aport_id = ffd2.ffd_dest_aport_id) ")
            sQuery.Append(" ) ")
            sQuery.Append(" ) ")

            sQuery.Append("OR  ")

            sQuery.Append(" (  ")
            ' -- MY DEPARTURE FLIGHT WAS OVER 60% OF MY MAX RANGE FOR MY AIRCRAFT AND 
            ' -- OCCURED WITHIN 90 MINUTES
            sQuery.Append(" exists( ")
            sQuery.Append(" (select  top 1 f2.ffd_distance from FAA_Flight_Data f2 with (NOLOCK) ")
            sQuery.Append(" INNER JOIN aircraft  a2 WITH(NOLOCK) ON (a2.ac_id = ffd_ac_id) AND a2.ac_journ_id = 0  ")
            sQuery.Append(" inner join Aircraft_Model am2 with (NOLOCK) on am2.amod_id =  a2.ac_amod_id ")
            sQuery.Append(" where(f2.ffd_ac_id = ffd2.ac_id And f2.ffd_unique_flight_id <> ffd2.ffd_unique_flight_id) ")

            ' ADDED IN MSW 
            If String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentsStartDate) And String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentsEndDate) Then
                sQuery.Append("  AND convert(date, ffd_date, 0) >= (getdate()-10)  ")
            Else
                sQuery.Append(" AND ((convert(date, ffd_date, 0) >= '" & Month(tstart) & "/" & Day(tstart) & "/" & Year(tstart) & "') ")
                sQuery.Append(" AND ( convert(date, ffd_date, 0) <= '" & Month(tend) & "/" & Day(tend) & "/" & Year(tend) & "')) ")
            End If

            ' -- dest date of original/landing and origin date of second
            '-- and the difference in time
            sQuery.Append(" and (DATEDIFF(mi, FFD2.ffd_dest_date, f2.ffd_origin_date)) < " & minutes & "  ")
            sQuery.Append("  and (DATEDIFF(mi, FFD2.ffd_dest_date, f2.ffd_origin_date)) > 0   ")
            '  -- AND MY AIRPORT  
            sQuery.Append("  and   (f2.ffd_origin_aport_id = ffd2.ffd_dest_aport_id) ")
            sQuery.Append(" and f2.ffd_distance > 0 and ffd_distance > (am2.amod_max_range_miles * ." & percentage & ")) ")
            sQuery.Append(" ) ")
            sQuery.Append(" ) ")
            sQuery.Append(" ) ")
            sQuery.Append(" ) ")


            If searchCriteria.ViewCriteriaAmodID > 0 Then  ' changed from -1 -- MSW 
                sQuery.Append(crmWebClient.Constants.cAndClause + " amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
            ElseIf Not IsNothing(searchCriteria.ViewCriteriaAmodIDArray) Then
                sQuery.Append(SetUpModelString(searchCriteria))
            ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
                sQuery.Append(SetUpMakeString(searchCriteria))
            End If


            If Not IsNothing(searchCriteria.ViewCriteriaTypeIDArray) Then
                sQuery.Append(SetUpTypeString(searchCriteria))
            End If


            If Not String.IsNullOrEmpty(Trim(Aircraft_IDS_String)) Then
                If exclude_Aircraft = True Then
                    sQuery.Append(" and ac_id not ")
                Else
                    sQuery.Append(" and ac_id ")
                End If
                sQuery.Append(" in (" & Aircraft_IDS_String & ") ")
            End If

            If Trim(Operator_IDS_String) <> "" Then
                If exclude_check = True Then
                    sQuery.Append(" and comp_id not ")
                Else
                    sQuery.Append(" and comp_id ")
                End If
                sQuery.Append(" in (" & Operator_IDS_String & ") ")
            End If


            If Trim(rollup_text) <> "" Then
                sQuery.Append(rollup_text)
            ElseIf searchCriteria.ViewCriteriaCompanyID > 0 Then
                sQuery.Append(" AND comp_id = " & searchCriteria.ViewCriteriaCompanyID & "")
            End If


            sQuery.Append(" order by ffd_origin_date desc ")

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "utilization_functions.vb", sQuery.ToString)

            SqlConn.ConnectionString = clientConnectString
            SqlConn.Open()
            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 600

            SqlCommand.CommandText = sQuery.ToString
            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                aError = "Error in getRefuel load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing
            aError = "Error in GetRefuel(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal product_code_selection As String,ByRef TotalFlights As Long, ByVal ForSummary As Boolean) As DataTable " + ex.Message

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

    Public Function get_flight_activity_by_model(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal product_code_selection As String, ByVal from_spot As String, ByRef TotalFlights As Long, ByVal ForSummary As Boolean) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim temp_string As String = ""
        Dim sQuery_temp As New StringBuilder

        Try

            '-- ***************  UPPER RIGHT TAB 3 - TOP MODELS ************************
            '-- # FLIGHT ACTIVITY BY MODEL
            If Trim(from_spot) = "company" Or Trim(from_spot) = "pdf" Then
                sQuery.Append(" SELECT DISTINCT comp_id, amod_make_name, amod_model_name, amod_id, ")
                sQuery.Append("  amjiqs_cat_desc, ")
                sQuery.Append(" count(*) AS tflights ")
            Else
                sQuery.Append(" SELECT DISTINCT ")
                If ForSummary = True Then
                    sQuery.Append(" top 10 amod_manufacturer_common_name as 'TOP 10 MANUFACTURERS' ")
                Else
                    sQuery.Append(" amod_make_name, amod_model_name, amod_id, ")
                    sQuery.Append(" amjiqs_cat_desc, ")
                    sQuery.Append(" count(*) AS tflights ")
                End If
            End If




            Call build_flight_data_subselects(sQuery_temp)
            temp_string = sQuery_temp.ToString
            temp_string = Replace(Trim(temp_string), "AM2.", "")

            If ForSummary = True Then
                temp_string = Replace(temp_string, "TotalFlightTimeHrs", "'TOTAL FLIGHT HOURS'")
                temp_string = Replace(temp_string, "TotalFuelBurn", "'EST FUEL BURN'")
                temp_string = Replace(temp_string, "NbrFlights", "'NBR FLIGHTS'")
            End If

            sQuery.Append(Trim(temp_string))

            sQuery.Append(", (SUM(ffd_distance)/count(*)) as AvgDistance ")
            sQuery.Append(" , (SUM(ffd_flight_time)/count(*)) as AvgMinPerFlights  ")

            If Trim(from_spot) = "company" Or Trim(from_spot) = "pdf" Then
                sQuery.Append(" , (select count(distinct a.ac_id) FROM Aircraft_Summary a WITH(NOLOCK) WHERE a.ac_lifecycle_stage = 3 AND a.cref_operator_flag IN ('Y', 'O') and a.amod_id = view_flights.amod_id and a.comp_id = view_flights.comp_id) as opcount ")
                sQuery.Append(" , (select count(distinct b.ac_id) FROM Aircraft_Summary b WITH(NOLOCK) WHERE  b.ac_lease_flag = 'Y' and b.cref_operator_flag IN ('Y', 'O') and b.amod_id = view_flights.amod_id and b.comp_id = view_flights.comp_id) as leasecount ")
            End If



            sQuery.Append(" FROM view_flights WITH(NOLOCK) ")
            '  sQuery.Append(" FROM FAA_Flight_Data FFD2 WITH(NOLOCK)  ")

            '  sQuery.Append(" INNER JOIN aircraft WITH(NOLOCK) ON (ac_id = ffd_ac_id) AND ac_journ_id = 0  ")
            '   sQuery.Append(" INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id  ")

            'If searchCriteria.ViewCriteriaCompanyID > 0 Or Trim(Operator_IDS_String) <> "" Then
            '  sQuery.Append(" INNER JOIN view_aircraft_company_flat WITH(NOLOCK) ON (ac_id = FFD2.ffd_ac_id) AND ac_journ_id = 0 and  cref_operator_flag in ('Y', 'O') ")
            '  sQuery.Append(" INNER JOIN Aircraft_Model WITH (NOLOCK) ON Aircraft_Model.amod_id = view_aircraft_company_flat.amod_id ")
            'Else
            '  sQuery.Append(" INNER JOIN aircraft_flat WITH(NOLOCK) ON (ac_id = FFD2.ffd_ac_id) AND ac_journ_id = 0 ")
            '  sQuery.Append(" INNER JOIN Aircraft_Model WITH (NOLOCK) ON Aircraft_Model.amod_id = aircraft_flat.amod_id ")
            'End If

            'sQuery.Append(" AND (ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportIATA & "' or ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportICAO & "') ")

            If String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentsStartDate) And String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentsEndDate) Then
                sQuery.Append(" WHERE convert(date, ffd_date, 0) >= (getdate()-182)  ")
            Else
                sQuery.Append(" where (convert(date, ffd_date, 0) >= '" & Month(searchCriteria.ViewCriteriaDocumentsStartDate) & "/" & Day(searchCriteria.ViewCriteriaDocumentsStartDate) & "/" & Year(searchCriteria.ViewCriteriaDocumentsStartDate) & "') ")
                sQuery.Append(" AND (convert(date, ffd_date, 0) <= '" & Month(searchCriteria.ViewCriteriaDocumentsEndDate) & "/" & Day(searchCriteria.ViewCriteriaDocumentsEndDate) & "/" & Year(searchCriteria.ViewCriteriaDocumentsEndDate) & "') ")
            End If

            If Not String.IsNullOrEmpty(Trim(Aircraft_IDS_String)) Then
                If exclude_Aircraft = True Then
                    sQuery.Append(" and ac_id not ")
                Else
                    sQuery.Append(" and ac_id ")
                End If
                sQuery.Append(" in (" & Aircraft_IDS_String & ") ")
            End If


            ' ADDED MSW - 3/10/20 TO BUILD IN DROP DOWN FOR IN OPERATION 
            If Not IsNothing(searchCriteria.viewCriteriaInOperation) Then
                If Trim(searchCriteria.viewCriteriaInOperation) <> "" Then
                    sQuery.Append(Build_In_Operation_String(searchCriteria))
                End If
            End If

            If Trim(airport_direction) = "O" Then
                If Airport_ID_OVERALL > 1 Then
                    sQuery.Append(" AND (ffd_origin_aport_id = '" & Airport_ID_OVERALL & "') ")
                ElseIf Not String.IsNullOrEmpty(Airport_IDS_String) Then
                    If exclude_airport_check Then
                        sQuery.Append(" AND (ffd_origin_aport_id not in (" & Airport_IDS_String & ")) ")
                    Else
                        sQuery.Append(" AND (ffd_origin_aport_id in (" & Airport_IDS_String & ")) ")
                    End If
                End If
            ElseIf Trim(airport_direction) = "D" Then
                If Airport_ID_OVERALL > 1 Then
                    sQuery.Append(" AND (ffd_dest_aport_id = '" & Airport_ID_OVERALL & "') ")
                ElseIf Not String.IsNullOrEmpty(Airport_IDS_String) Then
                    If exclude_airport_check Then
                        sQuery.Append(" AND (ffd_dest_aport_id not in (" & Airport_IDS_String & ")) ")
                    Else
                        sQuery.Append(" AND (ffd_dest_aport_id in (" & Airport_IDS_String & ")) ")
                    End If
                End If
            ElseIf Trim(airport_direction) = "X" Then
                '  sQuery.Append(" and ffd_dest_aport_id > 0")
                ' sQuery.Append(" AND (ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportIATA & "' or ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportICAO & "')")

                If Airport_ID_OVERALL > 1 Then
                    sQuery.Append(" AND ((ffd_origin_aport_id = '" & Airport_ID_OVERALL & "') or (ffd_dest_aport_id = '" & Airport_ID_OVERALL & "')) ")
                ElseIf Not String.IsNullOrEmpty(Airport_IDS_String) Then
                    If exclude_airport_check Then
                        sQuery.Append(" AND (ffd_origin_aport_id not in (" & Airport_IDS_String & ")) AND (ffd_dest_aport_id not in (" & Airport_IDS_String & "))   ")
                    Else
                        sQuery.Append(" AND (   (ffd_origin_aport_id in (" & Airport_IDS_String & "))  or (ffd_dest_aport_id in (" & Airport_IDS_String & "))   )")
                    End If

                End If

            Else
                If Airport_ID_OVERALL > 1 Then
                    sQuery.Append(" AND (ffd_dest_aport_id = '" & Airport_ID_OVERALL & "') ")
                ElseIf Not String.IsNullOrEmpty(Airport_IDS_String) Then
                    If exclude_airport_check Then
                        sQuery.Append(" AND (ffd_dest_aport_id not in (" & Airport_IDS_String & ")) ")
                    Else
                        sQuery.Append(" AND (ffd_dest_aport_id in (" & Airport_IDS_String & ")) ")
                    End If
                End If
            End If


            If Trim(searchCriteria.ViewCriteriaContinent) = "International" Then
                sQuery.Append(" and (origin_aport_country not in ('United States', 'U.S.') or dest_aport_country not in ('United States', 'U.S.'))  ")
            End If

            If Trim(distance_string) <> "" And Len(Trim(distance_string)) > 2 Then
                sQuery.Append(distance_string)
            End If


            If Trim(Operator_IDS_String) <> "" Then
                If exclude_check = True Then
                    sQuery.Append(" and comp_id not ")
                Else
                    sQuery.Append(" and comp_id ")
                End If
                sQuery.Append(" in (" & Operator_IDS_String & ") ")
            End If

            ' sQuery.Append("  and ffd_hide_flag= 'N' ")


            Dim temp_use As String = " "
            If searchCriteria.ViewCriteriaHasBusinessFlag = True Or searchCriteria.ViewCriteriaHasCommercialFlag = True Or searchCriteria.ViewCriteriaHasHelicopterFlag = True Then
                sQuery.Append(" and ( ")
            End If

            If searchCriteria.ViewCriteriaHasBusinessFlag = True Then
                If Trim(temp_use) = "" Then
                    temp_use = " or "
                Else
                    sQuery.Append(temp_use)
                End If
                sQuery.Append(" ac_product_business_flag = 'Y' ")
            End If

            If searchCriteria.ViewCriteriaHasCommercialFlag = True Then
                If Trim(temp_use) = "" Then
                    temp_use = " or "
                Else
                    sQuery.Append(temp_use)
                End If
                sQuery.Append(" ac_product_commercial_flag = 'Y' ")
            End If

            If searchCriteria.ViewCriteriaHasHelicopterFlag = True Then
                If Trim(temp_use) = "" Then
                    temp_use = " or "
                Else
                    sQuery.Append(temp_use)
                End If
                sQuery.Append(" ac_product_helicopter_flag = 'Y' ")
            End If

            If searchCriteria.ViewCriteriaHasBusinessFlag = True Or searchCriteria.ViewCriteriaHasCommercialFlag = True Or searchCriteria.ViewCriteriaHasHelicopterFlag = True Then
                sQuery.Append(" ) ")
            End If



            If Trim(rollup_text) <> "" Then
                sQuery.Append(rollup_text)
            ElseIf searchCriteria.ViewCriteriaCompanyID > 0 Then
                sQuery.Append(" AND comp_id = " & searchCriteria.ViewCriteriaCompanyID & "")
            End If


            If Trim(product_code_selection) <> "" Then
                sQuery.Append(product_code_selection)
            Else
            End If

            sQuery.Append(" " & commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))



            ' ADDED MSW - 3/10/20 TO BUILD IN DROP DOWN FOR IN OPERATION 
            If Not IsNothing(searchCriteria.viewCriteriaInOperation) Then
                If Trim(searchCriteria.viewCriteriaInOperation) <> "" Then
                    sQuery.Append(Build_In_Operation_String(searchCriteria))
                End If
            End If

            '-- SORT # 2 IS JUST BY MAKE AND MODEL NOT BY NUMBER OF FLIGHTS
            '' sQuery.Append(" order by amod_make_name, amod_model_name, amod_id ")

            If searchCriteria.ViewCriteriaAmodID > 0 Then ' changed from > -1
                sQuery.Append(crmWebClient.Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
            ElseIf Not IsNothing(searchCriteria.ViewCriteriaAmodIDArray) Then
                sQuery.Append(SetUpModelString(searchCriteria))
            ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
                sQuery.Append(SetUpMakeString(searchCriteria))
            End If

            If Not IsNothing(searchCriteria.ViewCriteriaTypeIDArray) Then
                sQuery.Append(SetUpTypeString(searchCriteria))
            End If



            If Trim(from_spot) = "company" Or Trim(from_spot) = "pdf" Then
                sQuery.Append(" group by comp_id, amod_make_name, amod_model_name, amod_id ")
            Else
                If ForSummary = True Then
                    sQuery.Append(" group by amod_manufacturer_common_name ")
                Else
                    sQuery.Append(" group by amod_make_name, amod_model_name, amod_id ")
                End If
            End If

            sQuery.Append(" , amjiqs_cat_desc ")
            'If Trim(searchCriteria.ViewCriteriaCompanyID) = 0 And Trim(Operator_IDS_String) = "" Then
            '  If TotalFlights = 0 Or TotalFlights > 1000 Then
            '    sQuery.Append(" having(COUNT(ffd_unique_flight_id) > 1) ")
            '  End If
            'End If

            If ForSummary = True Then
                sQuery.Append(" order by 'NBR FLIGHTS' desc  ")
            Else
                sQuery.Append(" order by COUNT(*) desc  ")
            End If
            'HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />" & Date.Now & " - get_flight_activity_by_model(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString
            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "utilization_functions.vb", sQuery.ToString)

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
                aError = "Error in get_flight_activity_by_model load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_flight_activity_by_model(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

    Public Function get_flight_activity_pie_by_model(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal field_name As String, ByVal product_code_selection As String, ByVal from_spot As String, ByRef TotalFlights As Long, ByVal ForSummary As Boolean) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim temp_string As String = ""
        Dim sQuery_temp As New StringBuilder

        Try

            '-- ***************  UPPER RIGHT TAB 3 - TOP MODELS ************************ 

            If Trim(field_name) = "amod_model_name" Then
                sQuery.Append(" SELECT DISTINCT top 20 aircraft_model.amod_make_name, aircraft_model.amod_model_name, count(*) AS tflights ")
            Else
                sQuery.Append(" SELECT DISTINCT " & field_name & ", count(*) AS tflights ")
            End If


            sQuery.Append(" FROM view_flights WITH(NOLOCK) ")
            sQuery.Append(" inner join Aircraft_Model with (NOLOCK) on view_flights.amod_id = Aircraft_Model.amod_id   ")

            If Trim(field_name) = "acwgtcls_name" Then
                sQuery.Append("  inner join Aircraft_Weight_Class with (NOLOCK) on acwgtcls_code = Aircraft_Model.amod_weight_class ")
            End If

            '   If Trim(field_name) = "amjiqs_cat_desc" Then
            'sQuery.Append("  inner join Aircraft_Model_JIQ_Size with (NOLOCK) on amjiqs_cat_code = amod_jniq_size  ")
            '   End If 

            If String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentsStartDate) And String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentsEndDate) Then
                sQuery.Append(" WHERE convert(date, ffd_date, 0) >= (getdate()-182)  ")
            Else
                sQuery.Append(" where (convert(date, ffd_date, 0) >= '" & Month(searchCriteria.ViewCriteriaDocumentsStartDate) & "/" & Day(searchCriteria.ViewCriteriaDocumentsStartDate) & "/" & Year(searchCriteria.ViewCriteriaDocumentsStartDate) & "') ")
                sQuery.Append(" AND (convert(date, ffd_date, 0) <= '" & Month(searchCriteria.ViewCriteriaDocumentsEndDate) & "/" & Day(searchCriteria.ViewCriteriaDocumentsEndDate) & "/" & Year(searchCriteria.ViewCriteriaDocumentsEndDate) & "') ")
            End If

            If Not String.IsNullOrEmpty(Trim(Aircraft_IDS_String)) Then
                If exclude_Aircraft = True Then
                    sQuery.Append(" and ac_id not ")
                Else
                    sQuery.Append(" and ac_id ")
                End If
                sQuery.Append(" in (" & Aircraft_IDS_String & ") ")
            End If


            If Trim(airport_direction) = "O" Then
                If Airport_ID_OVERALL > 1 Then
                    sQuery.Append(" AND (ffd_origin_aport_id = '" & Airport_ID_OVERALL & "') ")
                ElseIf Not String.IsNullOrEmpty(Airport_IDS_String) Then
                    If exclude_airport_check Then
                        sQuery.Append(" AND (ffd_origin_aport_id not in (" & Airport_IDS_String & ")) ")
                    Else
                        sQuery.Append(" AND (ffd_origin_aport_id in (" & Airport_IDS_String & ")) ")
                    End If
                End If
            ElseIf Trim(airport_direction) = "D" Then
                If Airport_ID_OVERALL > 1 Then
                    sQuery.Append(" AND (ffd_dest_aport_id = '" & Airport_ID_OVERALL & "') ")
                ElseIf Not String.IsNullOrEmpty(Airport_IDS_String) Then
                    If exclude_airport_check Then
                        sQuery.Append(" AND (ffd_dest_aport_id not in (" & Airport_IDS_String & ")) ")
                    Else
                        sQuery.Append(" AND (ffd_dest_aport_id in (" & Airport_IDS_String & ")) ")
                    End If
                End If
            ElseIf Trim(airport_direction) = "X" Then
                '  sQuery.Append(" and ffd_dest_aport_id > 0")
                ' sQuery.Append(" AND (ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportIATA & "' or ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportICAO & "')")

                If Airport_ID_OVERALL > 1 Then
                    sQuery.Append(" AND ((ffd_origin_aport_id = '" & Airport_ID_OVERALL & "') or (ffd_dest_aport_id = '" & Airport_ID_OVERALL & "')) ")
                ElseIf Not String.IsNullOrEmpty(Airport_IDS_String) Then
                    If exclude_airport_check Then
                        sQuery.Append(" AND (ffd_origin_aport_id not in (" & Airport_IDS_String & ")) AND (ffd_dest_aport_id not in (" & Airport_IDS_String & "))   ")
                    Else
                        sQuery.Append(" AND (   (ffd_origin_aport_id in (" & Airport_IDS_String & "))  or (ffd_dest_aport_id in (" & Airport_IDS_String & "))   )")
                    End If

                End If

            Else
                If Airport_ID_OVERALL > 1 Then
                    sQuery.Append(" AND (ffd_dest_aport_id = '" & Airport_ID_OVERALL & "') ")
                ElseIf Not String.IsNullOrEmpty(Airport_IDS_String) Then
                    If exclude_airport_check Then
                        sQuery.Append(" AND (ffd_dest_aport_id not in (" & Airport_IDS_String & ")) ")
                    Else
                        sQuery.Append(" AND (ffd_dest_aport_id in (" & Airport_IDS_String & ")) ")
                    End If
                End If
            End If


            If Trim(searchCriteria.ViewCriteriaContinent) = "International" Then
                sQuery.Append(" and (origin_aport_country not in ('United States', 'U.S.') or dest_aport_country not in ('United States', 'U.S.'))  ")
            End If

            If Trim(distance_string) <> "" And Len(Trim(distance_string)) > 2 Then
                sQuery.Append(distance_string)
            End If


            If Trim(Operator_IDS_String) <> "" Then
                If exclude_check = True Then
                    sQuery.Append(" and comp_id not ")
                Else
                    sQuery.Append(" and comp_id ")
                End If
                sQuery.Append(" in (" & Operator_IDS_String & ") ")
            End If

            ' sQuery.Append("  and ffd_hide_flag= 'N' ")


            Dim temp_use As String = " "
            If searchCriteria.ViewCriteriaHasBusinessFlag = True Or searchCriteria.ViewCriteriaHasCommercialFlag = True Or searchCriteria.ViewCriteriaHasHelicopterFlag = True Then
                sQuery.Append(" and ( ")
            End If

            If searchCriteria.ViewCriteriaHasBusinessFlag = True Then
                If Trim(temp_use) = "" Then
                    temp_use = " or "
                Else
                    sQuery.Append(temp_use)
                End If
                sQuery.Append(" ac_product_business_flag = 'Y' ")
            End If

            If searchCriteria.ViewCriteriaHasCommercialFlag = True Then
                If Trim(temp_use) = "" Then
                    temp_use = " or "
                Else
                    sQuery.Append(temp_use)
                End If
                sQuery.Append(" ac_product_commercial_flag = 'Y' ")
            End If

            If searchCriteria.ViewCriteriaHasHelicopterFlag = True Then
                If Trim(temp_use) = "" Then
                    temp_use = " or "
                Else
                    sQuery.Append(temp_use)
                End If
                sQuery.Append(" ac_product_helicopter_flag = 'Y' ")
            End If

            If searchCriteria.ViewCriteriaHasBusinessFlag = True Or searchCriteria.ViewCriteriaHasCommercialFlag = True Or searchCriteria.ViewCriteriaHasHelicopterFlag = True Then
                sQuery.Append(" ) ")
            End If

            If Trim(field_name) = "amod_model_name" Then
                sQuery.Append(" and aircraft_model.amod_model_name <> '' and aircraft_model.amod_model_name is not null ")
            Else
                sQuery.Append(" and " & field_name & " <> '' and " & field_name & " is not null ")
            End If

            'added MSW -5/6/19
            If searchCriteria.ViewCriteriaCompanyID > 0 And searchCriteria.ViewCriteriaAmodID > 0 Then
                sQuery.Append(" AND aircraft_model.amod_id = " & searchCriteria.ViewCriteriaAmodID & "")
            ElseIf Not IsNothing(searchCriteria.ViewCriteriaAmodIDArray) Then
                If searchCriteria.ViewCriteriaCompanyID > 0 Then
                    sQuery.Append(SetUpModelString(searchCriteria, True))
                Else
                    sQuery.Append(SetUpModelString(searchCriteria, True))
                End If
            ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake) Then
                If searchCriteria.ViewCriteriaCompanyID > 0 Then
                    sQuery.Append(SetUpMakeString(searchCriteria, True))
                Else
                    sQuery.Append(SetUpMakeString(searchCriteria, True))
                End If
            End If


            If Not IsNothing(searchCriteria.ViewCriteriaTypeIDArray) Then
                sQuery.Append(SetUpTypeString(searchCriteria, True))
            End If

            If Trim(rollup_text) <> "" Then
                sQuery.Append(rollup_text)
            ElseIf searchCriteria.ViewCriteriaCompanyID > 0 Then
                sQuery.Append(" AND comp_id = " & searchCriteria.ViewCriteriaCompanyID & "")
            End If


            If Trim(product_code_selection) <> "" Then
                sQuery.Append(product_code_selection)
            Else
            End If

            sQuery.Append(" " & Replace(commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False), "amod_", "aircraft_model.amod_"))



            '-- SORT # 2 IS JUST BY MAKE AND MODEL NOT BY NUMBER OF FLIGHTS
            '' sQuery.Append(" order by amod_make_name, amod_model_name, amod_id ")



            If Trim(field_name) = "amod_model_name" Then
                sQuery.Append(" group by aircraft_model.amod_make_name, aircraft_model.amod_model_name ")
            Else
                sQuery.Append(" group by " & field_name & " ")
            End If


            sQuery.Append(" order by COUNT(*) desc  ")

            'HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />" & Date.Now & " - get_flight_activity_by_model(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString
            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "utilization_functions.vb", sQuery.ToString)

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
                aError = "Error in get_flight_activity_by_model load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_flight_activity_by_model(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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
    Public Function get_company_profile(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal from_spot As String, ByVal rollup As String) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim temp_string As String = ""
        Dim sQuery_temp As New StringBuilder

        Try


            sQuery.Append(" exec EvolutionGetCompanyProfile " & searchCriteria.ViewCriteriaCompanyID & ",'" & rollup & "' ")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />" & Date.Now & " - get_company_ownership(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
                aError = "Error in get_company_profile load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_company_profile(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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
    Public Function get_company_ownership(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal product_code_selection As String, ByVal from_spot As String) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim temp_string As String = ""
        Dim sQuery_temp As New StringBuilder

        Try


            'select distinct RelCompID from ReturnAllCompanyLocationsByCompId(" & CompanyID & ")
            If Trim(rollup_text) <> "" Then
                sQuery.Append(" select * from ReturnAircraftOwnershipbyCompany(" & searchCriteria.ViewCriteriaCompanyID.ToString & ", 'Y')")
            Else
                sQuery.Append(" select * from ReturnAircraftOwnershipbyCompany(" & searchCriteria.ViewCriteriaCompanyID.ToString & ", 'N') ")
            End If

            '  sQuery.Append(" inner join Aircraft_Model with (NOLOCK) on  Aircraft_Model.amod_make_name  = ReturnAircraftOwnershipbyCompany.amod_make_name and  Aircraft_Model.amod_model_name  = ReturnAircraftOwnershipbyCompany.amod_model_name ")
            sQuery.Append(" where ac_id > 0 ")
            sQuery.Append(" " & commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))

            If searchCriteria.ViewCriteriaAmodID > 0 Then
                sQuery.Append(" and amod_id = " & searchCriteria.ViewCriteriaAmodID & " ")
            End If

            sQuery.Append(" order by purchased_date desc ")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />" & Date.Now & " - get_company_ownership(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
                aError = "Error in get_company_ownership load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_company_ownership(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

    Public Function get_normal_ac_for_location(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal product_code_selection As String) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try


            '-- ***************  LOWER TAB 1 - AIRCDRAFT BASED ************************
            '-- SAME BASIC LIST AS NORMAL FOR AIRCRAFT AT THAT LOCATION
            sQuery.Append(" select distinct ac_id, amod_airframe_type_code, amod_type_code, ac_last_aerodex_event,  ")
            sQuery.Append(" ac_picture_id,ac_aport_icao_code,ac_aport_iata_code,aport_latitude_decimal,aport_longitude_decimal,  ")
            sQuery.Append(" ac_list_date, amod_make_name, amod_model_name,amod_id, ac_mfr_year, ac_forsale_flag, ac_year,  ")
            sQuery.Append(" ac_ser_no_full,ac_ser_no_sort, ac_reg_no, ac_flights_id, ac_airframe_tot_hrs, ac_engine_1_tot_hrs, ")
            sQuery.Append(" ac_engine_2_tot_hrs, ac_engine_3_tot_hrs, ac_engine_4_tot_hrs, ac_status, ac_asking,  ")
            sQuery.Append(" ac_asking_price, ac_delivery,ac_reg_no_search, ac_exclusive_flag, ac_lease_flag,  ")
            sQuery.Append(" ac_engine_1_soh_hrs, ac_engine_2_soh_hrs,ac_engine_3_soh_hrs,ac_engine_4_soh_hrs,  ")
            sQuery.Append(" ac_last_event, ac_passenger_count, ac_interior_moyear, ac_exterior_moyear ")
            sQuery.Append(" from View_Aircraft_Flat with (NOLOCK)  ")
            'sQuery.Append(" where (ac_aport_iata_code = '" & searchCriteria.ViewCriteriaAirportIATA & "' or ac_aport_icao_code = '" & searchCriteria.ViewCriteriaAirportICAO & "') ")
            sQuery.Append(" where (ac_aport_id = '" & Airport_ID_OVERALL & "') ")
            sQuery.Append(" and ac_lifecycle_stage = 3 ")
            sQuery.Append(" AND amod_customer_flag = 'Y' AND (( amod_product_business_flag = 'Y')  ")
            sQuery.Append(" OR ( amod_product_commercial_flag = 'Y') OR (amod_product_helicopter_flag = 'Y'))  ")
            sQuery.Append(" AND ( ac_product_business_flag = 'Y' OR ac_product_commercial_flag = 'Y'  ")
            sQuery.Append(" OR ac_product_helicopter_flag = 'Y')  ")

            If Trim(product_code_selection) <> "" Then
                sQuery.Append(product_code_selection)
            End If

            If searchCriteria.ViewCriteriaAmodID > -1 Then
                sQuery.Append(crmWebClient.Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
            ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
                sQuery.Append(crmWebClient.Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
            End If

            sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))

            sQuery.Append(" order by amod_make_name, amod_airframe_type_code, amod_type_code,  ")
            sQuery.Append(" amod_id, amod_model_name, ac_ser_no_sort  ")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />" & Date.Now & "get_normal_ac_for_location(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
                aError = "Error in get_fractional_shares load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_fractional_shares(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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
    Public Function get_most_recent_flight_activity_companies(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal run_export As String, ByVal selected_value As String, ByVal recent_flight_months As Integer, ByVal contact_type As String, ByVal use_ac As Boolean, ByVal start_date As String, ByVal end_date As String, ByVal product_code_selection As String) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            '-- ***************  LOWER TAB 2 - RECENT FLIGHT ACTIVITY ************************
            '-- # FLIGHT ACTIVITY MOST RECENT 
            sQuery.Append(" select comp_id as COMPID, comp_name as COMPANY, comp_address1 as COMP_ADDRESS, comp_city as CITY, comp_state as STATE ")

            If use_ac = True Then
                sQuery.Append(", ac_id As ACId, amod_make_name As Make, amod_model_name As Model")
                sQuery.Append(", ac_ser_no_full As SerNbr, ac_reg_no As RegNbr ")
            End If

            sQuery.Append(" , SUM(ffd_flight_time) as FLIGHT_TIME , COUNT(*) as TOTAL_COUNT ")
            sQuery.Append(" FROM View_FAA_Flight_Data_Clean WITH(NOLOCK) ")

            sQuery.Append(" INNER JOIN aircraft_flat WITH(NOLOCK) ON (ac_id = ffd_ac_id) AND ac_journ_id = 0 ")

            ' U is for utilization, B is for based 
            If Trim(selected_value) = "" Or Trim(selected_value) = "U" Then
                sQuery.Append(" INNER JOIN Airport WITH(NOLOCK) ON ffd_dest_aport_id = aport_id and ffd_dest_aport_id = " & Airport_ID_OVERALL & " ")
            ElseIf Trim(selected_value) = "B" Then

            End If


            ' only do arrivals - for now 
            ' If Trim(selected_value) = "" Or Trim(selected_value) = "F" Then
            '   sQuery.Append(" left outer JOIN Airport WITH(NOLOCK) ON ffd_origin_aport_id = aport_id ")
            '   sQuery.Append(" left outer JOIN Airport a2 WITH(NOLOCK) ON ffd_dest_aport_id = a2.aport_id ")
            ' ElseIf Trim(selected_value) = "A" Then
            '  sQuery.Append(" INNER JOIN  Airport WITH(NOLOCK) ON ffd_origin_aport_id = aport_id ")
            ' End If

            If Trim(contact_type) = "36" Then
                sQuery.Append(" inner join Aircraft_Reference on cref_ac_id = ac_id and cref_journ_id = ac_journ_id and (cref_contact_type = '" & contact_type & "' or cref_operator_flag  in ('Y', 'O')) ")
            Else
                sQuery.Append(" inner join Aircraft_Reference on cref_ac_id = ac_id and cref_journ_id = ac_journ_id and cref_contact_type = '" & contact_type & "' ")
            End If



            sQuery.Append(" inner join Company with (NOLOCK) on comp_id = cref_comp_id and comp_journ_id = ac_journ_id  ")

            ' If use_date_range = True Then
            sQuery.Append(" WHERE ffd_date >= '" & start_date & "' and  ffd_date <= '" & DateAdd(DateInterval.Day, 1, CDate(end_date)) & "' and ffd_hide_flag= 'N'  ")
            ' ElseIf recent_flight_months = 0 Then
            ' sQuery.Append(" WHERE ffd_date >= (getdate()-90) and ffd_hide_flag= 'N'  ")
            '  Else
            '  sQuery.Append(" WHERE ffd_date >= (getdate()-" & (recent_flight_months * 30) & ")   and ffd_hide_flag= 'N'  ")
            ' End If






            '  If Trim(selected_value) = "" Or Trim(selected_value) = "A" Then
            '   sQuery.Append(" AND (ffd_dest_aport_id = '" & Airport_ID_OVERALL & "') ")
            ' ElseIf Trim(selected_value) = "D" Then
            '    sQuery.Append(" AND (ffd_origin_aport_id = '" & Airport_ID_OVERALL & "') ")
            ' End If


            If Trim(product_code_selection) <> "" Then
                sQuery.Append(product_code_selection)
            End If

            sQuery.Append(" AND ((amod_type_code IN ('T','P') AND amod_customer_flag = 'Y'  ")
            sQuery.Append(" AND amod_airframe_type_code = 'R' AND amod_product_helicopter_flag = 'Y') ")
            sQuery.Append(" OR (amod_customer_flag = 'Y' AND amod_airframe_type_code = 'F'  ")
            sQuery.Append(" AND amod_product_business_flag = 'Y') OR (amod_customer_flag = 'Y'  ")
            sQuery.Append(" AND amod_airframe_type_code = 'F' AND amod_product_commercial_flag = 'Y')) ")

            sQuery.Append(" and ffd_hide_flag= 'N' ")

            If Trim(distance_string) <> "" And Len(Trim(distance_string)) > 2 Then
                sQuery.Append(distance_string)
            End If


            If searchCriteria.ViewCriteriaAmodID > -1 Then
                sQuery.Append(crmWebClient.Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
            ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
                sQuery.Append(crmWebClient.Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
            End If

            sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))
            If use_ac = True Then
                sQuery.Append(" group by comp_id, comp_name , comp_address1, comp_city, comp_state, ac_id, amod_make_name, amod_model_name, ac_ser_no_full, ac_reg_no ")
            Else
                sQuery.Append(" group by comp_id, comp_name , comp_address1, comp_city, comp_state ")
            End If

            sQuery.Append(" order by comp_name asc, COUNT(*) desc ")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />" & Date.Now & " - get_most_recent_flight_activity(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
                aError = "Error in get_most_recent_flight_activity_companies load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_most_recent_flight_activity_companies(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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
    Public Function get_most_recent_flight_activity(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal run_export As String, ByVal selected_value As String, ByVal recent_flight_months As Integer, ByVal start_date As String, ByVal end_date As String, ByVal product_code_selection As String, ByVal LimitExport As Boolean) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            '-- ***************  LOWER TAB 2 - RECENT FLIGHT ACTIVITY ************************
            '-- # FLIGHT ACTIVITY MOST RECENT

            HttpContext.Current.Session.Item("Selection_Listing_Fields") = ""
            HttpContext.Current.Session.Item("Selection_Listing_Table") = ""
            HttpContext.Current.Session.Item("Selection_Listing_Where") = ""
            HttpContext.Current.Session.Item("Selection_Listing_Group") = ""
            HttpContext.Current.Session.Item("Selection_Listing_Order") = ""


            'If Trim(run_export) = "A" Then

            '  HttpContext.Current.Session.Item("Selection_Listing_Fields") &= (" select TOP 1000 amod_make_name As 'Make', amod_model_name As 'Model', ")
            '  HttpContext.Current.Session.Item("Selection_Listing_Fields") &= ("ac_ser_no_full as 'SerNbr', ")
            '  HttpContext.Current.Session.Item("Selection_Listing_Fields") &= ("ac_reg_no as 'RegNbr', ")
            '  HttpContext.Current.Session.Item("Selection_Listing_Fields") &= (" ffd_dest_date, ffd_origin_date, ")
            '  HttpContext.Current.Session.Item("Selection_Listing_Fields") &= (" ffd_date As 'FlightDate', ffd_origin_aport As 'OriginAPort',")
            '  HttpContext.Current.Session.Item("Selection_Listing_Fields") &= (" origin_aport_latitude AS 'ORIGIN LAT', origin_aport_longitude AS 'ORIGIN LONG',dest_aport_latitude AS 'DEST LAT', dest_aport_longitude AS 'DEST LONG', ")

            '  HttpContext.Current.Session.Item("Selection_Listing_Fields") &= (" origin_aport_name as 'origin_aport_name', origin_aport_country as 'origin_aport_country', origin_aport_city as 'origin_aport_city', origin_aport_state as 'origin_aport_state', ")
            '  ' HttpContext.Current.Session.Item("Selection_Listing_Fields") &= (" origin_aport_name, origin_aport_country, origin_aport_city, origin_aport_state,  ")
            '  '   HttpContext.Current.Session.Item("Selection_Listing_Fields") &= (" dest_aport_name, dest_aport_country, dest_aport_city, dest_aport_state,  ")
            '  HttpContext.Current.Session.Item("Selection_Listing_Fields") &= (" comp_id as 'COMPID', comp_name AS 'OPERATOR', comp_address1 AS 'ADDRESS', comp_city AS 'CITY', comp_state AS 'STATE', comp_country AS 'COUNTRY', comp_web_address AS 'WEB ADDRESS', comp_email_address AS 'EMAIL',comp_off_phone AS 'OFFICE PHONE', contact_first_name AS 'FIRST NAME', contact_last_name AS 'LAST NAME', contact_title AS 'TITLE', contact_email_address AS 'CONTACT EMAIL',contact_off_phone AS 'CONTACT OFFICE PHONE',contact_mob_phone AS 'CONTACT MOBILE PHONE',  ac_id as ACID,contact_id AS 'CONTACTID', ")

            '  HttpContext.Current.Session.Item("Selection_Listing_Fields") &= (" ffd_flight_time As 'FlightTime', ffd_distance As 'Distance', ")
            '  HttpContext.Current.Session.Item("Selection_Listing_Fields") &= (" ((ffd_flight_time * amod_fuel_burn_rate)/60) as 'FuelBurn' ")

            '  sQuery.Append(HttpContext.Current.Session.Item("Selection_Listing_Fields"))

            '  ' HttpContext.Current.Session.Item("Selection_Listing_Table") &= (" FROM View_FAA_Flight_Data_Clean WITH(NOLOCK) ")
            '  HttpContext.Current.Session.Item("Selection_Listing_Table") &= (" FROM view_flights WITH(NOLOCK) ")
            'Else
            '  HttpContext.Current.Session.Item("Selection_Listing_Fields") &= (" select TOP 1000 ac_id As 'ACId', amod_make_name As 'Make', amod_model_name As 'Model', ")
            '  HttpContext.Current.Session.Item("Selection_Listing_Fields") &= ("ac_ser_no_full as 'SERNO_NONDISPLAY', ac_ser_no_sort as 'SERNOSORT_NONDISPLAY', ")
            '  HttpContext.Current.Session.Item("Selection_Listing_Fields") &= ("ac_reg_no as 'REGNO_NONDISPLAY', ")
            '  HttpContext.Current.Session.Item("Selection_Listing_Fields") &= (" ac_ser_no_full as 'SerNbr',")
            '  HttpContext.Current.Session.Item("Selection_Listing_Fields") &= (" ac_reg_no as 'RegNbr',")
            '  HttpContext.Current.Session.Item("Selection_Listing_Fields") &= (" ffd_dest_date, ffd_origin_date,  ")
            '  HttpContext.Current.Session.Item("Selection_Listing_Fields") &= (" ffd_date As 'FlightDate', ffd_origin_aport As 'OriginAPort', ffd_dest_aport As 'DestinAPort',ffd_origin_aport_id,ffd_dest_aport_id, ")
            '  HttpContext.Current.Session.Item("Selection_Listing_Fields") &= (" origin_aport_latitude AS 'ORIGIN LAT', origin_aport_longitude AS 'ORIGIN LONG',dest_aport_latitude AS 'DEST LAT', dest_aport_longitude AS 'DEST LONG', ")
            '  HttpContext.Current.Session.Item("Selection_Listing_Fields") &= (" comp_id as 'COMPID', comp_name AS 'OPERATOR', comp_address1 AS 'ADDRESS', comp_city AS 'CITY', comp_state AS 'STATE', comp_country AS 'COUNTRY', comp_web_address AS 'WEB ADDRESS', comp_email_address AS 'EMAIL',comp_off_phone AS 'OFFICE PHONE', contact_first_name AS 'FIRST NAME', contact_last_name AS 'LAST NAME', contact_title AS 'TITLE', contact_email_address AS 'CONTACT EMAIL',contact_off_phone AS 'CONTACT OFFICE PHONE',contact_mob_phone AS 'CONTACT MOBILE PHONE',  ac_id as ACID,contact_id AS 'CONTACTID', ")

            '  HttpContext.Current.Session.Item("Selection_Listing_Fields") &= (" origin_aport_name as 'origin_aport_name', origin_aport_country as 'origin_aport_country', origin_aport_city as 'origin_aport_city', origin_aport_state as 'origin_aport_state', ")

            '  If Trim(selected_value) = "" Or Trim(selected_value) = "D" Then
            '    HttpContext.Current.Session.Item("Selection_Listing_Fields") &= (" dest_aport_name as 'dest_aport_name', dest_aport_country as 'dest_aport_country', dest_aport_city as 'dest_aport_city', dest_aport_state as 'dest_aport_state',  ")
            '  End If

            '  HttpContext.Current.Session.Item("Selection_Listing_Fields") &= (" ffd_flight_time As 'FlightTime', ffd_distance As 'Distance', ")
            '  HttpContext.Current.Session.Item("Selection_Listing_Fields") &= (" ((ffd_flight_time * amod_fuel_burn_rate)/60) as 'FuelBurn' ")

            '  '   HttpContext.Current.Session.Item("Selection_Listing_Table") &= (" FROM FAA_Flight_Data FFD2 WITH(NOLOCK) ")
            '  HttpContext.Current.Session.Item("Selection_Listing_Table") &= (" FROM view_flights WITH(NOLOCK) ")
            'End If



            HttpContext.Current.Session.Item("Selection_Listing_Fields") = "SELECT "

            If LimitExport = True Then
                HttpContext.Current.Session.Item("Selection_Listing_Fields") += " TOP 1000"
            End If

            HttpContext.Current.Session.Item("Selection_Listing_Fields") += " amod_make_name as MAKE, amod_model_name as MODEL, base_aport_name, base_aport_city, base_aport_state, base_aport_country, base_aport_iata_code, base_aport_icao_code,  ac_mfr_year as 'MFR YEAR', SERNBR, REGNBR,"
            HttpContext.Current.Session.Item("Selection_Listing_Fields") += "ffd_date as 'FLIGHT DATE', ffd_flight_time as 'FLIGHT TIME', ffd_distance as 'DISTANCE', ESTFUELBURN, "
            HttpContext.Current.Session.Item("Selection_Listing_Fields") += "ffd_origin_aport as 'ORIGIN CODE', origin_aport_name AS 'ORIGIN NAME', origin_aport_city AS 'ORIGIN CITY', origin_aport_state AS 'ORIGIN STATE', "
            HttpContext.Current.Session.Item("Selection_Listing_Fields") += "origin_aport_country as 'ORIGIN COUNTRY',  origin_aport_latitude AS 'ORIGIN LAT', origin_aport_longitude AS 'ORIGIN LONG',"
            HttpContext.Current.Session.Item("Selection_Listing_Fields") += "ffd_dest_aport AS 'DEST CODE',dest_aport_name AS 'DEST NAME',dest_aport_city AS 'DEST CITY',  dest_aport_state AS 'DEST STATE',  dest_aport_country AS 'DEST COUNTRY', "
            HttpContext.Current.Session.Item("Selection_Listing_Fields") += "dest_aport_latitude AS 'DEST LAT', dest_aport_longitude AS 'DEST LONG',  comp_name AS 'OPERATOR', comp_address1 AS 'ADDRESS', "
            HttpContext.Current.Session.Item("Selection_Listing_Fields") += "comp_city AS 'CITY', comp_state AS 'STATE', comp_country AS 'COUNTRY', comp_web_address AS 'WEB ADDRESS', comp_email_address AS 'EMAIL',"
            HttpContext.Current.Session.Item("Selection_Listing_Fields") += "comp_off_phone AS 'OFFICE PHONE', "
            HttpContext.Current.Session.Item("Selection_Listing_Fields") += "contact_first_name AS 'FIRST NAME', contact_last_name AS 'LAST NAME', contact_title AS 'TITLE', contact_email_address AS 'CONTACT EMAIL',"
            HttpContext.Current.Session.Item("Selection_Listing_Fields") += "contact_off_phone AS 'CONTACT OFFICE PHONE',"
            HttpContext.Current.Session.Item("Selection_Listing_Fields") += "contact_mob_phone AS 'CONTACT MOBILE PHONE', ac_id as AC_ID, comp_id AS 'COMP_ID',contact_ID AS 'CONTACT_ID',ffd_origin_aport_ID AS 'ORIGIN_ID',ffd_dest_aport_id AS 'DEST_ID' "

            HttpContext.Current.Session.Item("Selection_Listing_Fields") += ", origin_continent, dest_continent, amjiqs_cat_desc  "
            HttpContext.Current.Session.Item("Selection_Listing_Fields") += ", cbus_name "


            HttpContext.Current.Session.Item("Selection_Listing_Fields") += ", Convert(varchar(10), ffd_origin_date, 101) as 'DepartureDate',  convert (varchar(15), ffd_origin_date,8) as 'DepartureTime' "
            HttpContext.Current.Session.Item("Selection_Listing_Fields") += ", Convert(varchar(10), ffd_dest_date, 101) as 'ArrivalDate' , convert (varchar(15), ffd_dest_date,8) as 'ArrivalTime' "


            '  HttpContext.Current.Session.Item("Selection_Listing_Fields") += ",  COMMERCIAL_FIELD"

            '   HttpContext.Current.Session.Item("Selection_Listing_Table") &= (" FROM view_flights WITH(NOLOCK) ")

            HttpContext.Current.Session.Item("Selection_Listing_Table") &= (" FROM View_Flights_New WITH(NOLOCK) ")


            sQuery.Append(HttpContext.Current.Session.Item("Selection_Listing_Fields"))


            '  HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "a2.aport_name as 'aport_name2'", "a2.aport_name as 'AIRPORT2'")
            '  HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "a2.aport_country as 'aport_country2'", "a2.aport_country as 'AIRPORT2 COUNTRY'")
            '  HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "a2.aport_city as 'aport_city2'", "a2.aport_city as 'AIRPORT2 CITY'")
            '  HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "a2.aport_state as 'aport_state2'", "a2.aport_state as 'AIRPORT2 STATE'")
            '  HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "a2.aport_id as 'aport_id2'", "a2.aport_id as 'AIRPORT2 ID'")


            'HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "ffd_dest_date", "ffd_dest_date as 'DESTDATE'")
            'HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "ffd_origin_date", "ffd_origin_date as 'ORIGINDATE'")
            '' HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "airport.aport_name", "airport.aport_name as 'APORTNAME'")
            '' HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "airport.aport_country", "airport.aport_country as 'COUNTRY'")
            '' HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "airport.aport_city", "airport.aport_city as 'CITY'")
            '' HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "airport.aport_state", "airport.aport_state as 'STATE'")
            'HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "ffd_dest_aport_id", "ffd_dest_aport_id as 'DEST AIRPORT ID'")
            '' HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "airport.aport_id", "airport.aport_id as 'SerNoSort'")
            'HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "ffd_origin_aport_id", "ffd_origin_aport_id as 'ORIG AIRPORT ID'")


            '  HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "ac_ser_no_full as 'SERNO_NONDISPLAY',", "")
            '   HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "ac_ser_no_sort as 'SERNOSORT_NONDISPLAY',", "")
            '     HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "faablk_reg_no, ", "")




            'If searchCriteria.ViewCriteriaCompanyID > 0 Or Trim(Operator_IDS_String) <> "" Then
            '  HttpContext.Current.Session.Item("Selection_Listing_Table") &= (" INNER JOIN view_aircraft_company_flat WITH(NOLOCK) ON (ac_id = ffd_ac_id) AND ac_journ_id = 0 and  cref_operator_flag in ('Y', 'O') ")
            '  HttpContext.Current.Session.Item("Selection_Listing_Table") &= ("  inner join aircraft_model with (NOLOCK) on view_aircraft_company_flat.amod_id = aircraft_model.amod_id  ")
            'Else
            '  HttpContext.Current.Session.Item("Selection_Listing_Table") &= (" INNER JOIN aircraft WITH(NOLOCK) ON (ac_id = ffd_ac_id) AND ac_journ_id = 0 ")
            '  HttpContext.Current.Session.Item("Selection_Listing_Table") &= ("  inner join aircraft_model with (NOLOCK) on ac_amod_id = aircraft_model.amod_id  ")
            'End If



            ' sQuery.Append(" INNER JOIN aircraft WITH(NOLOCK) ON (ac_id = ffd_ac_id) AND ac_journ_id = 0  ")
            ' sQuery.Append(" INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id  ")

            ' NO CHANGE HERE YET 
            'If Trim(airport_direction) = "D" Then
            '  If Trim(selected_value) = "" Or Trim(selected_value) = "D" Then
            '    HttpContext.Current.Session.Item("Selection_Listing_Table") &= (" left outer JOIN Airport WITH(NOLOCK) ON ffd_origin_aport_id = aport_id ")
            '    HttpContext.Current.Session.Item("Selection_Listing_Table") &= (" left outer JOIN Airport a2 WITH(NOLOCK) ON ffd_dest_aport_id = a2.aport_id ")
            '  ElseIf Trim(selected_value) = "A" Then
            '    HttpContext.Current.Session.Item("Selection_Listing_Table") &= (" INNER JOIN  Airport WITH(NOLOCK) ON ffd_origin_aport_id = aport_id ")
            '  End If
            'ElseIf Trim(airport_direction) = "O" Then
            '  If Trim(selected_value) = "" Or Trim(selected_value) = "D" Then
            '    HttpContext.Current.Session.Item("Selection_Listing_Table") &= (" left outer JOIN Airport WITH(NOLOCK) ON ffd_origin_aport_id = aport_id ")
            '    HttpContext.Current.Session.Item("Selection_Listing_Table") &= (" left outer JOIN Airport a2 WITH(NOLOCK) ON ffd_dest_aport_id = a2.aport_id ")
            '  ElseIf Trim(selected_value) = "A" Then
            '    HttpContext.Current.Session.Item("Selection_Listing_Table") &= (" INNER JOIN  Airport WITH(NOLOCK) ON ffd_origin_aport_id = aport_id ")
            '  End If
            'Else
            '  If Trim(selected_value) = "" Or Trim(selected_value) = "D" Then
            '    HttpContext.Current.Session.Item("Selection_Listing_Table") &= (" left outer JOIN Airport WITH(NOLOCK) ON ffd_origin_aport_id = aport_id ")
            '    HttpContext.Current.Session.Item("Selection_Listing_Table") &= (" left outer JOIN Airport a2 WITH(NOLOCK) ON ffd_dest_aport_id = a2.aport_id ")
            '  ElseIf Trim(selected_value) = "A" Then
            '    HttpContext.Current.Session.Item("Selection_Listing_Table") &= (" INNER JOIN  Airport WITH(NOLOCK) ON ffd_origin_aport_id = aport_id ")
            '  End If
            'End If







            '  If Trim(run_export) <> "A" Then
            '    HttpContext.Current.Session.Item("Selection_Listing_Table") &= (" left outer join FAA_Blocked_Registration_Numbers with (NOLOCK) on ac_reg_no=faablk_reg_no ")
            '  End If


            sQuery.Append(HttpContext.Current.Session.Item("Selection_Listing_Table"))


            If Trim(start_date) <> "" And Trim(end_date) <> "" Then
                HttpContext.Current.Session.Item("Selection_Listing_Where") &= (" WHERE  convert(date, ffd_date, 0) >= '" & start_date & "' and  convert(date, ffd_date, 0) <= '" & end_date & "' ")
                ' ElseIf recent_flight_months = 0 Then
            ElseIf searchCriteria.ViewCriteriaCompanyID > 0 Then
                HttpContext.Current.Session.Item("Selection_Listing_Where") &= (" WHERE convert(date, ffd_date, 0) >= (getdate()-182) ")
            Else
                HttpContext.Current.Session.Item("Selection_Listing_Where") &= (" WHERE convert(date, ffd_date, 0) >= (getdate()-30) ")
                'sQuery.Append(" WHERE ffd_date >= (getdate()-" & (recent_flight_months * 30) & ")   and ffd_hide_flag= 'N'  ")
            End If


            If Trim(Operator_IDS_String) <> "" Then
                If exclude_check = True Then
                    HttpContext.Current.Session.Item("Selection_Listing_Where") &= (" and comp_id not ")
                Else
                    HttpContext.Current.Session.Item("Selection_Listing_Where") &= (" and comp_id ")
                End If
                HttpContext.Current.Session.Item("Selection_Listing_Where") &= (" in (" & Operator_IDS_String & ") ")
            End If

            If searchCriteria.ViewCriteriaCompanyID > 0 Then
                HttpContext.Current.Session.Item("Selection_Listing_Where") &= (" AND comp_id = " & searchCriteria.ViewCriteriaCompanyID & "")
            End If

            If Not String.IsNullOrEmpty(Trim(Aircraft_IDS_String)) Then
                If exclude_Aircraft = True Then
                    HttpContext.Current.Session.Item("Selection_Listing_Where") &= (" and ac_id not ")
                Else
                    HttpContext.Current.Session.Item("Selection_Listing_Where") &= (" and ac_id ")
                End If
                HttpContext.Current.Session.Item("Selection_Listing_Where") &= (" in (" & Aircraft_IDS_String & ") ")
            End If

            If Trim(airport_direction) = "O" Then
                If Airport_ID_OVERALL > 1 Then
                    HttpContext.Current.Session.Item("Selection_Listing_Where") &= (" AND (ffd_origin_aport_id = '" & Airport_ID_OVERALL & "') ")
                ElseIf Not String.IsNullOrEmpty(Airport_IDS_String) Then
                    If exclude_airport_check Then
                        HttpContext.Current.Session.Item("Selection_Listing_Where") &= (" AND (ffd_origin_aport_id not in (" & Airport_IDS_String & ")) ")
                    Else
                        HttpContext.Current.Session.Item("Selection_Listing_Where") &= (" AND (ffd_origin_aport_id in (" & Airport_IDS_String & ")) ")
                    End If
                End If

            ElseIf Trim(airport_direction) = "O" Then
                If Airport_ID_OVERALL > 1 Then
                    HttpContext.Current.Session.Item("Selection_Listing_Where") &= (" AND (ffd_dest_aport_id = '" & Airport_ID_OVERALL & "') ")
                ElseIf Not String.IsNullOrEmpty(Airport_IDS_String) Then
                    If exclude_airport_check Then
                        HttpContext.Current.Session.Item("Selection_Listing_Where") &= (" AND (ffd_dest_aport_id not in (" & Airport_IDS_String & ")) ")
                    Else
                        HttpContext.Current.Session.Item("Selection_Listing_Where") &= (" AND (ffd_dest_aport_id in (" & Airport_IDS_String & ")) ")
                    End If
                End If

            ElseIf Trim(airport_direction) = "X" Then
                '  sQuery.Append(" and ffd_dest_aport_id > 0")
                ' sQuery.Append(" AND (ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportIATA & "' or ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportICAO & "')")

                If Airport_ID_OVERALL > 1 Then
                    HttpContext.Current.Session.Item("Selection_Listing_Where") &= (" AND ((ffd_origin_aport_id = '" & Airport_ID_OVERALL & "') or (ffd_dest_aport_id = '" & Airport_ID_OVERALL & "')) ")
                ElseIf Not String.IsNullOrEmpty(Airport_IDS_String) Then
                    If exclude_airport_check Then
                        HttpContext.Current.Session.Item("Selection_Listing_Where") &= (" AND (ffd_origin_aport_id not in (" & Airport_IDS_String & ")) AND (ffd_dest_aport_id not in (" & Airport_IDS_String & "))   ")
                    Else
                        HttpContext.Current.Session.Item("Selection_Listing_Where") &= (" AND (   (ffd_origin_aport_id in (" & Airport_IDS_String & "))  or (ffd_dest_aport_id in (" & Airport_IDS_String & "))   )")
                    End If

                End If



            Else 'If Trim(airport_direction) = "D" Then
                If Airport_ID_OVERALL > 1 Then
                    HttpContext.Current.Session.Item("Selection_Listing_Where") &= (" AND (ffd_dest_aport_id = '" & Airport_ID_OVERALL & "') ")
                ElseIf Not String.IsNullOrEmpty(Airport_IDS_String) Then
                    If exclude_airport_check Then
                        HttpContext.Current.Session.Item("Selection_Listing_Where") &= (" AND (ffd_dest_aport_id not in (" & Airport_IDS_String & ")) ")
                    Else
                        HttpContext.Current.Session.Item("Selection_Listing_Where") &= (" AND (ffd_dest_aport_id in (" & Airport_IDS_String & ")) ")
                    End If
                End If
            End If
            'sQuery.Append(" AND ((amod_type_code IN ('T','P') AND amod_customer_flag = 'Y'  ")
            'sQuery.Append(" AND amod_airframe_type_code = 'R' AND amod_product_helicopter_flag = 'Y') ")
            'sQuery.Append(" OR (amod_customer_flag = 'Y' AND amod_airframe_type_code = 'F'  ")
            'sQuery.Append(" AND amod_product_business_flag = 'Y') OR (amod_customer_flag = 'Y'  ")
            'sQuery.Append(" AND amod_airframe_type_code = 'F' AND amod_product_commercial_flag = 'Y')) ")

            ' HttpContext.Current.Session.Item("Selection_Listing_Where") &= (" and ffd_hide_flag= 'N' ")


            If Trim(distance_string) <> "" And Len(Trim(distance_string)) > 2 Then
                HttpContext.Current.Session.Item("Selection_Listing_Where") &= (distance_string)
            End If


            ' ADDED MSW - 3/10/20 TO BUILD IN DROP DOWN FOR IN OPERATION 
            If Not IsNothing(searchCriteria.viewCriteriaInOperation) Then
                If Trim(searchCriteria.viewCriteriaInOperation) <> "" Then
                    HttpContext.Current.Session.Item("Selection_Listing_Where") &= Build_In_Operation_String(searchCriteria)
                End If
            End If



            If searchCriteria.ViewCriteriaAmodID > -1 Then
                HttpContext.Current.Session.Item("Selection_Listing_Where") &= (crmWebClient.Constants.cAndClause + " amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
            ElseIf Not IsNothing(searchCriteria.ViewCriteriaAmodIDArray) Then
                HttpContext.Current.Session.Item("Selection_Listing_Where") &= (SetUpModelString(searchCriteria))
            ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
                HttpContext.Current.Session.Item("Selection_Listing_Where") &= (SetUpMakeString(searchCriteria))
            End If

            If Not IsNothing(searchCriteria.ViewCriteriaTypeIDArray) Then
                HttpContext.Current.Session.Item("Selection_Listing_Where") &= (SetUpTypeString(searchCriteria))
            End If

            If Trim(product_code_selection) <> "" Then
                HttpContext.Current.Session.Item("Selection_Listing_Where") &= (product_code_selection)
            Else

            End If
            HttpContext.Current.Session.Item("Selection_Listing_Where") &= (" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))



            sQuery.Append(HttpContext.Current.Session.Item("Selection_Listing_Where"))

            If Trim(selected_value) = "" Or Trim(selected_value) = "A" Then
                HttpContext.Current.Session.Item("Selection_Listing_Order") &= (" order by ffd_origin_date desc ")
            Else
                HttpContext.Current.Session.Item("Selection_Listing_Order") &= (" order by ffd_dest_date desc ")
            End If

            sQuery.Append(HttpContext.Current.Session.Item("Selection_Listing_Order"))

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "utilization_functions.vb", sQuery.ToString)

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
                aError = "Error in get_most_recent_flight_activity load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_most_recent_flight_activity(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

    Public Function get_companies_in_city(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal bus_type As String, ByVal run_export As String, ByVal temp_distance As Integer, ByVal orig_lat As Double, ByVal orig_long As Double, ByVal city_name As String, ByVal country_name As String) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim query_distance As String = ""
        Dim max_NORTH As Double = 0.0
        Dim max_SOUTH As Double = 0.0
        Dim max_WEST As Double = 0.0
        Dim max_EAST As Double = 0.0



        Try

            '-- ***************  LOWER TAB 3 - COMPANY DIRECTORY ************************
            '-- SELECT A LIST OF COMPANIES LOCATED AT SPECIFIC AIRPORT OR SAME CITY
            If Trim(run_export) = "A" Then
                sQuery.Append(" select comp_name as CompanyName, comp_address1 as Address, comp_city as City, comp_state as State, comp_web_address as WebAddress, comp_email_address  as EmailAddress")
            Else
                sQuery.Append(" select comp_id, comp_name, comp_address1, comp_city, comp_state, comp_web_address, comp_email_address ")
            End If


            sQuery.Append(" from Company ")


            If searchCriteria.ViewCriteriaCountry = "United States" Then
                sQuery.Append(" inner join Zip_Mapping with (NOLOCK) on left(comp_zip_code,5) = zmap_zip_code ")
            Else
                sQuery.Append(" left outer JOIN Airport WITH(NOLOCK)on comp_country = aport_country  ")
                sQuery.Append(" and comp_state=aport_state and comp_city=aport_city ")
            End If


            If Trim(bus_type) <> "" Then
                sQuery.Append(" inner JOIN Business_Type_Reference WITH(NOLOCK)on comp_id = bustypref_comp_id and comp_journ_id = bustypref_journ_id  ")
                sQuery.Append(" inner JOIN Company_Business_Type WITH(NOLOCK)on bustypref_type= cbus_type ")
            End If





            'If Trim(searchCriteria.ViewCriteriaAirportICAO) <> "" Then
            'sQuery.Append(" or aport_iata_code = '" & searchCriteria.ViewCriteriaAirportICAO & "') ")
            '  Else
            '  sQuery.Append(" ) ")
            '  End If
            If searchCriteria.ViewCriteriaCountry = "United States" Then


                If temp_distance > 0 Then
                    query_distance = CDbl(temp_distance / 69).ToString 'http://www.distancebetweencities.net/ helped us 
                Else
                    query_distance = "2.1739"
                End If

                max_WEST = FormatNumber(orig_long + query_distance, 6)
                max_EAST = FormatNumber(orig_long - query_distance, 6)
                max_NORTH = FormatNumber(orig_lat + query_distance, 6)
                max_SOUTH = FormatNumber(orig_lat - query_distance, 6)

                sQuery.Append("where (zmap_longitude <= " & max_WEST & " AND zmap_longitude >= " & max_EAST & ")  ")
                sQuery.Append("AND (zmap_latitude <= " & max_NORTH & " AND zmap_latitude >= " & max_SOUTH & ")  ")
            Else
                sQuery.Append(" where (aport_id= '" & Airport_ID_OVERALL & "' or ")

                If Trim(city_name) <> "" And Trim(country_name) <> "" Then
                    sQuery.Append(" ( comp_city = '" & Trim(city_name) & "' and comp_country = '" & Trim(country_name) & "' ) )")
                ElseIf Trim(city_name) <> "" Then
                    sQuery.Append("  comp_city = '" & Trim(city_name) & "'  )")
                ElseIf Trim(country_name) <> "" Then
                    sQuery.Append("  comp_country = '" & Trim(country_name) & "' )")
                End If


            End If



            If searchCriteria.ViewCriteriaCountry = "United States" Then
                sQuery.Append(" and comp_country='United States' ")
            End If

            sQuery.Append(" and comp_journ_id = 0 ")
            sQuery.Append(" and comp_active_flag = 'Y' ")

            If Trim(bus_type) <> "" Then
                sQuery.Append(" and cbus_type in ('" & bus_type & "')")
            End If


            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />" & Date.Now & " - get_companies_in_city(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

            sQuery.Append(" order by comp_name ")

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
                aError = "Error in get_companies_in_city load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_companies_in_city(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

    Public Function util_get_opearators_rollup(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal aport_id As Long) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try
            '-- ***************  LOWER TAB 4 - OWNERS ************************
            '-- SELECT A LIST OF COMPANIES OWNING AIRCRAFT AT SPECIFIC AIRPORT 

            sQuery.Append("  	select comp_name, comp_city,comp_state, comp_country, comp_id, ")
            sQuery.Append(" 	sum(case when cref_contact_type in ('00','08','97') and ac_product_business_flag='Y' and amod_type_code in ('E','J') then 1 else 0 end) as JetOwner, ")
            sQuery.Append(" 	sum(case when cref_contact_type in ('00','08','97') and ac_product_business_flag='Y' and amod_type_code in ('T') then 1 else 0 end) as TurboOwner, ")
            sQuery.Append(" 	sum(case when cref_contact_type in ('00','08','97') and ac_product_business_flag='Y' and amod_type_code in ('P') then 1 else 0 end) as PistonOwner, ")
            sQuery.Append(" 	sum(case when cref_contact_type in ('00','08','97') and ac_product_commercial_flag='Y' then 1 else 0 end) as CommercialOwner, ")
            sQuery.Append(" 	sum(case when cref_contact_type in ('00','08','97') and ac_product_helicopter_flag='Y' then 1 else 0 end) as HeloOwner, ")
            sQuery.Append(" 	sum(case when cref_operator_flag='Y' then 1 else 0 end) as Operator, ")
            sQuery.Append(" 	sum(case when cref_contact_type in ('99','98') then 1 else 0 end) as Broker ")
            sQuery.Append(" 	from Company with (NOLOCK)  ")

            If use_owner = True Or searchCriteria.ViewCriteriaAmodID > 0 Then
                sQuery.Append(" 	inner join Aircraft_Reference with (NOLOCK) on cref_comp_id = comp_id and cref_journ_id = comp_journ_id ")
                sQuery.Append(" 	inner join Aircraft_Flat with (NOLOCK) on cref_ac_id = ac_id and cref_journ_id = ac_journ_id ")
                'sQuery.Append(" 	inner join Aircraft_Model with (NOLOCK) on ac_amod_id = amod_id ")
            Else
                sQuery.Append(" 	left outer join Aircraft_Reference with (NOLOCK) on cref_comp_id = comp_id and cref_journ_id = comp_journ_id ")
                sQuery.Append(" 	left outer join Aircraft_Flat with (NOLOCK) on cref_ac_id = ac_id and cref_journ_id = ac_journ_id ")
                ' sQuery.Append(" 	left outer join Aircraft_Model with (NOLOCK) on ac_amod_id = amod_id ")
            End If

            ' moved these into where clause 
            If searchCriteria.ViewCriteriaAmodID > 0 Then
                sQuery.Append(" and Aircraft_Model.amod_id = " & searchCriteria.ViewCriteriaAmodID & " ")
            End If

            If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
                sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))
            Else
                sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True))
            End If


            sQuery.Append("      where comp_journ_id = 0 ")
            sQuery.Append(" 	and comp_id in (select distinct RelCompID from ReturnAllCompanyLocationsByCompId(" & searchCriteria.ViewCriteriaCompanyID & ")) ")

            sQuery.Append(" " + commonEvo.MakeCompanyProductCodeClause(HttpContext.Current.Session.Item("localPreferences"), False))


            sQuery.Append(" 	group by comp_name, comp_city,comp_state, comp_country, comp_id ")
            sQuery.Append(" 	order by comp_name, comp_city,comp_state, comp_country, comp_id ")


            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />" & Date.Now & " - util_get_opearators_rollup(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
                aError = "Error in util_get_opearators_rollup load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in util_get_opearators_rollup(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

    Public Function GetTopManufacturers(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal aport_id As Long, ByRef TotalFlights As Long, ByVal from_spot As String, Optional ByRef LimitThisQuery As Long = 0) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery As String = ""
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            sQuery = "SELECT DISTINCT top 10 Aircraft_Model.amod_manufacturer_common_name as 'TOP 10 MANUFACTURERS' , "
            sQuery += " COUNT(FFD2.ffd_unique_flight_id) as 'NBR FLIGHTS', SUM(convert(decimal(18,4),FFD2.ffd_flight_time))/60 as 'TOTAL FLIGHT HOURS', "
            sQuery += " SUM((FFD2.ffd_flight_time* amod_fuel_burn_rate)/60) as 'EST FUEL BURN' "
            sQuery += " FROM FAA_Flight_Data FFD2 WITH(NOLOCK) "
            sQuery += " INNER JOIN aircraft_flat WITH(NOLOCK) ON (ac_id = FFD2.ffd_ac_id) AND ac_journ_id = 0 "
            sQuery += " INNER JOIN Aircraft_Model WITH (NOLOCK) ON Aircraft_Model.amod_id = aircraft_flat.amod_id "

            If searchCriteria.ViewCriteriaCompanyID > 0 Then
                sQuery += " inner join Aircraft_Reference with (NOLOCK) on cref_ac_id = ac_id and cref_operator_flag IN ('Y', 'O') and cref_journ_id = 0"
            End If


            If Trim(searchCriteria.ViewCriteriaContinent) = "International" Then
                sQuery += ("  INNER JOIN Airport ad WITH(NOLOCK) ON ffd_dest_aport_id = ad.aport_id   ")
                sQuery += (" INNER JOIN Airport ao WITH(NOLOCK) ON ffd_origin_aport_id = ao.aport_id  ")
            End If

            sQuery += " where  "

            If String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentsStartDate) And String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentsStartDate) Then
                Dim TempDate As String = DateAdd(DateInterval.Month, IIf(searchCriteria.ViewCriteriaTimeSpan > 0, -searchCriteria.ViewCriteriaTimeSpan, -6), Now())
                sQuery += "  (convert(date, FFD2.ffd_date, 0) >= '" & Month(TempDate) & "/" & Day(TempDate) & "/" & Year(TempDate) & "') AND (convert(date, FFD2.ffd_date, 0) <= '" & Month(Now()) & "/" & Day(Now()) & "/" & Year(Now()) & "') "
            Else
                sQuery += "  ((convert(date, FFD2.ffd_date, 0) >= '" & Month(searchCriteria.ViewCriteriaDocumentsStartDate) & "/" & Day(searchCriteria.ViewCriteriaDocumentsStartDate) & "/" & Year(searchCriteria.ViewCriteriaDocumentsStartDate) & "') "
                sQuery += " AND (convert(date, FFD2.ffd_date, 0) <= '" & Month(searchCriteria.ViewCriteriaDocumentsEndDate) & "/" & Day(searchCriteria.ViewCriteriaDocumentsEndDate) & "/" & Year(searchCriteria.ViewCriteriaDocumentsEndDate) & "')) "
            End If



            If Trim(airport_direction) = "D" Then
                If aport_id > 0 Then
                    sQuery += (" and FFD2.ffd_dest_aport_id  = " & aport_id & " ")
                ElseIf Not String.IsNullOrEmpty(Airport_IDS_String) Then
                    If exclude_airport_check Then
                        sQuery += (" and FFD2.ffd_dest_aport_id not in (" & Airport_IDS_String & ") ")
                    Else
                        sQuery += (" and FFD2.ffd_dest_aport_id  in (" & Airport_IDS_String & ") ")
                    End If
                End If
            ElseIf Trim(airport_direction) = "O" Then
                If aport_id > 0 Then
                    sQuery += (" and FFD2.ffd_origin_aport_id  = " & aport_id & " ")
                ElseIf Not String.IsNullOrEmpty(Airport_IDS_String) Then
                    If exclude_airport_check Then
                        sQuery += (" and FFD2.ffd_origin_aport_id not in (" & Airport_IDS_String & ") ")
                    Else
                        sQuery += (" and FFD2.ffd_origin_aport_id  in (" & Airport_IDS_String & ") ")
                    End If
                End If
            ElseIf Trim(airport_direction) = "X" Then
                '  sQuery.Append(" and ffd_dest_aport_id > 0")
                ' sQuery.Append(" AND (ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportIATA & "' or ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportICAO & "')")

                If Airport_ID_OVERALL > 1 Then
                    sQuery += (" AND ((ffd_origin_aport_id = '" & Airport_ID_OVERALL & "') or (ffd_dest_aport_id = '" & Airport_ID_OVERALL & "')) ")
                ElseIf Not String.IsNullOrEmpty(Airport_IDS_String) Then
                    If exclude_airport_check Then
                        sQuery += (" AND (ffd_origin_aport_id not in (" & Airport_IDS_String & ")) AND (ffd_dest_aport_id not in (" & Airport_IDS_String & "))   ")
                    Else
                        sQuery += (" AND (   (ffd_origin_aport_id in (" & Airport_IDS_String & "))  or (ffd_dest_aport_id in (" & Airport_IDS_String & "))   )")
                    End If

                End If

            Else
                If aport_id > 0 Then
                    sQuery += (" and FFD2.ffd_dest_aport_id  = " & aport_id & " ")
                ElseIf Not String.IsNullOrEmpty(Airport_IDS_String) Then
                    If exclude_airport_check Then
                        sQuery += (" and FFD2.ffd_dest_aport_id not in (" & Airport_IDS_String & ") ")
                    Else
                        sQuery += (" and FFD2.ffd_dest_aport_id  in (" & Airport_IDS_String & ") ")
                    End If
                End If
            End If



            If Trim(distance_string) <> "" And Len(Trim(distance_string)) > 2 Then
                sQuery += (distance_string)
            End If

            If searchCriteria.ViewCriteriaCompanyID > 0 Then
                sQuery += (" AND cref_comp_id = " & searchCriteria.ViewCriteriaCompanyID & "")
            End If

            'added MSW -5/6/19
            If searchCriteria.ViewCriteriaCompanyID > 0 And searchCriteria.ViewCriteriaAmodID > 0 Then
                sQuery += (" AND aircraft_model.amod_id = " & searchCriteria.ViewCriteriaAmodID & "")
            ElseIf Not IsNothing(searchCriteria.ViewCriteriaAmodIDArray) Then
                If searchCriteria.ViewCriteriaCompanyID > 0 Then
                    sQuery += (SetUpModelString(searchCriteria, True))
                Else
                    sQuery += (SetUpModelString(searchCriteria, True))
                End If
            ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake) Then
                If searchCriteria.ViewCriteriaCompanyID > 0 Then
                    sQuery += (SetUpMakeString(searchCriteria, True))
                Else
                    sQuery += (SetUpMakeString(searchCriteria, True))
                End If
            End If


            If Not IsNothing(searchCriteria.ViewCriteriaTypeIDArray) Then
                sQuery &= (SetUpTypeString(searchCriteria, True))
            End If

            If Trim(searchCriteria.ViewCriteriaContinent) = "International" Then
                sQuery &= (" and ( ao.aport_country not in ('United States', 'U.S.') or ad.aport_country not in ('United States', 'U.S.'))  ")
            End If

            sQuery += " and FFD2.ffd_hide_flag= 'N' "
            'sQuery += " and ( ( ac_product_business_flag = 'Y' and Aircraft_Model.amod_type_code in ('E','J') ) "
            'sQuery += " or ( ac_product_business_flag = 'Y' and Aircraft_Model.amod_type_code in ('T','P') ) or ( ac_product_helicopter_flag = 'Y' ) "
            'sQuery += " or ( ac_product_commercial_flag = 'Y' ) ) "

            Dim HoldClsSubscription As New crmSubscriptionClass

            HoldClsSubscription.crmAerodexFlag = HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag
            HoldClsSubscription.crmBusiness_Flag = HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag
            HoldClsSubscription.crmCommercial_Flag = HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag
            HoldClsSubscription.crmHelicopter_Flag = HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag
            HoldClsSubscription.crmJets_Flag = HttpContext.Current.Session.Item("localSubscription").crmJets_Flag
            HoldClsSubscription.crmTurboprops = HttpContext.Current.Session.Item("localSubscription").crmTurboprops
            HoldClsSubscription.crmExecutive_Flag = HttpContext.Current.Session.Item("localSubscription").crmExecutive_Flag

            sQuery += " " & Replace(clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(HoldClsSubscription, False, True), "amod_", "Aircraft_Model.amod_")

            Dim temp_use As String = " "
            If searchCriteria.ViewCriteriaHasBusinessFlag = True Or searchCriteria.ViewCriteriaHasCommercialFlag = True Or searchCriteria.ViewCriteriaHasHelicopterFlag = True Then
                sQuery += (" and ( ")
            End If

            If searchCriteria.ViewCriteriaHasBusinessFlag = True Then
                If Trim(temp_use) = "" Then
                    temp_use = " or "
                Else
                    sQuery += (temp_use)
                End If
                sQuery += (" ac_product_business_flag = 'Y' ")
            End If

            If searchCriteria.ViewCriteriaHasCommercialFlag = True Then
                If Trim(temp_use) = "" Then
                    temp_use = " or "
                Else
                    sQuery += (temp_use)
                End If
                sQuery += (" ac_product_commercial_flag = 'Y' ")
            End If

            If searchCriteria.ViewCriteriaHasHelicopterFlag = True Then
                If Trim(temp_use) = "" Then
                    temp_use = " or "
                Else
                    sQuery += (temp_use)
                End If
                sQuery += (" ac_product_helicopter_flag = 'Y' ")
            End If

            If searchCriteria.ViewCriteriaHasBusinessFlag = True Or searchCriteria.ViewCriteriaHasCommercialFlag = True Or searchCriteria.ViewCriteriaHasHelicopterFlag = True Then
                sQuery += (" ) ")
            End If

            sQuery += " group by Aircraft_Model.amod_manufacturer_common_name "
            sQuery += " having(COUNT(FFD2.ffd_unique_flight_id) > 1)"
            sQuery += " order by 'NBR FLIGHTS' desc"



            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "utilization_functions.vb", sQuery.ToString)

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
                aError = "Error in GetTopManufacturers load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in GetTopManufacturers As DataTable " + ex.Message

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
    Public Function util_get_opearators(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal aport_id As Long, ByRef TotalFlights As Long, ByVal from_spot As String, Optional ByRef LimitThisQuery As Long = 0) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try
            '-- ***************  LOWER TAB 4 - OWNERS ************************
            '-- SELECT A LIST OF COMPANIES OWNING AIRCRAFT AT SPECIFIC AIRPORT 
            sQuery.Append(" SELECT distinct ")

            If LimitThisQuery > 0 Then
                sQuery.Append(" top " + LimitThisQuery.ToString + " ")
            Else
                If Trim(from_spot) = "pdf" Then
                    sQuery.Append(" top 40")
                ElseIf TotalFlights > 50000 Then
                    '    sQuery.Append(" top 500")
                End If
            End If


            ' ADDED MSW - 5/05/2020

            '     sQuery.Append(" company.comp_id, company.comp_name, Company_Business_Type.cbus_name, company.comp_city, company.comp_state, company.comp_country, company.comp_email_address,  pnum_number_full as  comp_off_phone, company.comp_address1 ")
            '     sQuery.Append(", country_continent_name ")




            sQuery.Append(" comp_id, comp_name, cbus_name, comp_city, comp_state, comp_country, comp_email_address,comp_off_phone, comp_address1 ")
            sQuery.Append(", country_continent_name ")



            'sQuery.Append(" contact_first_name , contact_last_name , contact_title , contact_email_address,contact_off_phone ,contact_mob_phone, ")

            Call build_flight_data_subselects(sQuery)

            sQuery.Append(", (SUM(ffd_distance)/count(*)) as AvgDistance ")
            sQuery.Append(" , (SUM(ffd_flight_time)/count(*)) as AvgMinPerFlights ")

            Call build_flight_data_from(sQuery)




            ' changed to a left outer join so that unknown operator showed - msw - 3/15/20

            ' ADDED MSW - 5/05/2020
            '  sQuery.Append("  Left outer join aircraft_company_role with (NOLOCK) on View_Flights.ac_id = acomprole_ac_id  ")
            ' sQuery.Append(" And (convert(date,View_Flights.ffd_date) >= acomprole_start_date And (convert(date,View_Flights.ffd_date) <= acomprole_end_date Or acomprole_end_date Is NULL)) ")
            '  sQuery.Append(" Left outer join company on Company.comp_id = acomprole_comp_id And comp_journ_id = 0 ")

            '    sQuery.Append(" left outer join country with (NOLOCK) on  country_name = Replace(comp_country, 'U.S.', 'United States')   ")
            sQuery.Append(" left outer join country with (NOLOCK) on  country_name = Replace(comp_country, 'U.S.', 'United States')   ")

            '   sQuery.Append(" Left outer join Phone_Numbers with (NOLOCK) on pnum_comp_id = company.comp_id And pnum_journ_id = comp_journ_id And pnum_type = 'Office'  ")
            '   sQuery.Append(" inner Join Company_Business_Type with (NOLOCK) on cbus_type = comp_business_type ")


            Call build_flight_data_where(sQuery, aport_id, searchCriteria)


            If Trim(Operator_IDS_String) <> "" Then
                If exclude_check = True Then
                    sQuery.Append(" and comp_id not ")
                Else
                    sQuery.Append(" and comp_id ")
                End If
                sQuery.Append(" in (" & Operator_IDS_String & ") ")
            End If

            If Not String.IsNullOrEmpty(Trim(Aircraft_IDS_String)) Then
                If exclude_Aircraft = True Then
                    sQuery.Append(" and ac_id not ")
                Else
                    sQuery.Append(" and ac_id ")
                End If
                sQuery.Append(" in (" & Aircraft_IDS_String & ") ")
            End If


            If Trim(rollup_text) <> "" Then
                sQuery.Append(Trim(rollup_text))
            End If

            If searchCriteria.ViewCriteriaAFTTStart > 0 Then
                sQuery.Append(Constants.cAndClause + " ((ac_airframe_tot_hrs >= " & searchCriteria.ViewCriteriaAFTTStart & ") or (ac_airframe_tot_hrs IS NULL))")
            End If

            If searchCriteria.ViewCriteriaAFTTEnd > 0 Then
                sQuery.Append(Constants.cAndClause + " ((ac_airframe_tot_hrs <= " & searchCriteria.ViewCriteriaAFTTEnd & ") or (ac_airframe_tot_hrs IS NULL))")
            End If



            If Trim(searchCriteria.ViewCriteriaContinent) = "International" Then
                sQuery.Append(" and (origin_aport_country not in ('United States', 'U.S.') or dest_aport_country not in ('United States', 'U.S.'))  ")
            End If


            If Trim(distance_string) <> "" And Len(Trim(distance_string)) > 2 Then
                sQuery.Append(distance_string)
            End If


            If searchCriteria.ViewCriteriaYearStart > 0 Then
                sQuery.Append(Constants.cAndClause + " ac_mfr_year >= " & searchCriteria.ViewCriteriaYearStart)
            End If

            If searchCriteria.ViewCriteriaYearEnd > 0 Then
                sQuery.Append(Constants.cAndClause + " ac_mfr_year <=  " & searchCriteria.ViewCriteriaYearEnd)
            End If

            Dim temp_use As String = " "
            If searchCriteria.ViewCriteriaHasBusinessFlag = True Or searchCriteria.ViewCriteriaHasCommercialFlag = True Or searchCriteria.ViewCriteriaHasHelicopterFlag = True Then
                sQuery.Append(" and ( ")
            End If

            If searchCriteria.ViewCriteriaHasBusinessFlag = True Then
                If Trim(temp_use) = "" Then
                    temp_use = " or "
                Else
                    sQuery.Append(temp_use)
                End If
                sQuery.Append(" ac_product_business_flag = 'Y' ")
            End If

            If searchCriteria.ViewCriteriaHasCommercialFlag = True Then
                If Trim(temp_use) = "" Then
                    temp_use = " or "
                Else
                    sQuery.Append(temp_use)
                End If
                sQuery.Append(" ac_product_commercial_flag = 'Y' ")
            End If

            If searchCriteria.ViewCriteriaHasHelicopterFlag = True Then
                If Trim(temp_use) = "" Then
                    temp_use = " or "
                Else
                    sQuery.Append(temp_use)
                End If
                sQuery.Append(" ac_product_helicopter_flag = 'Y' ")
            End If

            If searchCriteria.ViewCriteriaHasBusinessFlag = True Or searchCriteria.ViewCriteriaHasCommercialFlag = True Or searchCriteria.ViewCriteriaHasHelicopterFlag = True Then
                sQuery.Append(" ) ")
            End If


            ' ADDED MSW - 3/10/20 TO BUILD IN DROP DOWN FOR IN OPERATION 
            If Not IsNothing(searchCriteria.viewCriteriaInOperation) Then
                If Trim(searchCriteria.viewCriteriaInOperation) <> "" Then
                    sQuery.Append(Build_In_Operation_String(searchCriteria))
                End If
            End If


            If searchCriteria.ViewCriteriaAmodID > 0 Then
                sQuery.Append(" and amod_id = " & searchCriteria.ViewCriteriaAmodID & "")
            ElseIf Not IsNothing(searchCriteria.ViewCriteriaAmodIDArray) Then
                sQuery.Append(SetUpModelString(searchCriteria)) ', "amod_id", "ac_amod_id"))
            ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
                sQuery.Append(Replace(SetUpMakeString(searchCriteria), "amod_make_name", "amod_make_name"))
            End If

            If Not IsNothing(searchCriteria.ViewCriteriaTypeIDArray) Then
                sQuery.Append(Replace(Replace(SetUpTypeString(searchCriteria), "amod_airframe_type_code", "amod_airframe_type_code"), "amod_type_code", "amod_type_code"))
            End If

            If searchCriteria.ViewCriteriaCompanyID > 0 Then
                sQuery.Append(" and comp_id = " & searchCriteria.ViewCriteriaCompanyID & " ")
            End If

            sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))


            '      sQuery.Append(" group by company.comp_id, company.comp_name, Company_Business_Type.cbus_name, company.comp_city, company.comp_state, company.comp_country, company.comp_email_address,  pnum_number_full,  company.comp_address1 ")
            '       sQuery.Append(", country_continent_name ")
            sQuery.Append(" group by comp_id, comp_name, cbus_name, comp_city, comp_state, comp_country, comp_email_address,  comp_off_phone,  comp_address1 ")
            sQuery.Append(", country_continent_name ")


            '  sQuery.Append(" contact_first_name , contact_last_name , contact_title , contact_email_address,contact_off_phone, contact_mob_phone,")

            'If Trim(Operator_IDS_String) = "" Then
            '  If TotalFlights > 10000 Then
            '    sQuery.Append(" having(COUNT(ffd_unique_flight_id) > 4) ")
            '  ElseIf TotalFlights = 0 Or TotalFlights > 1000 Then
            '    sQuery.Append(" having(COUNT(ffd_unique_flight_id) > 1) ")
            '  End If
            'End If

            '   If LimitThisQuery > 0 Or Trim(from_spot) = "pdf" Then
            sQuery.Append(" order by NbrFlights desc ")
            '   Else
            'If Trim(from_spot) <> "pdf" And TotalFlights > 50000 Then
            '   sQuery.Append(" order by NbrFlights desc ")
            '  Else 
            '   sQuery.Append(" order by comp_name ")
            '    End If

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "utilization_functions.vb", sQuery.ToString)

            'HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />" & Date.Now & " - get_companies_from_airport(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

            SqlConn.ConnectionString = clientConnectString
            SqlConn.Open()
            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 600

            SqlCommand.CommandText = sQuery.ToString
            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                aError = "Error in util_get_opearators load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in util_get_opearators(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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


    Public Function util_get_operator_pie_charts(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal field_name As String, ByVal aport_id As Long, ByRef TotalFlights As Long, ByVal from_spot As String, Optional ByRef LimitThisQuery As Long = 0) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try
            '-- ***************  LOWER TAB 4 - OWNERS ************************
            '-- SELECT A LIST OF COMPANIES OWNING AIRCRAFT AT SPECIFIC AIRPORT 
            sQuery.Append(" SELECT distinct top 15 ")

            If Trim(field_name) = "country_continent_name" Then
                sQuery.Append("  case when country_continent_name is null then 'Unknown' else country_continent_name end as country_continent_name ")
            Else
                sQuery.Append("  " & field_name & " ")
            End If


            sQuery.Append(", COUNT(ffd_unique_flight_id) as NbrFlights ")

            Call build_flight_data_from(sQuery)

            If Trim(field_name) = "country_continent_name" Then
                sQuery.Append(" left outer join Country with (NOLOCK) on country_name =  replace(comp_country, 'U.S.', 'United States')   ")
            End If


            If Trim(field_name) = "state_name" Then
                sQuery.Append(" inner join State with (NOLOCK) on state_code = comp_state and state_country = replace(comp_country, 'U.S.', 'United States') ")
            End If

            Call build_flight_data_where(sQuery, aport_id, searchCriteria)




            If Trim(Operator_IDS_String) <> "" Then
                If exclude_check = True Then
                    sQuery.Append(" and comp_id not ")
                Else
                    sQuery.Append(" and comp_id ")
                End If
                sQuery.Append(" in (" & Operator_IDS_String & ") ")
            End If

            If Not String.IsNullOrEmpty(Trim(Aircraft_IDS_String)) Then
                If exclude_Aircraft = True Then
                    sQuery.Append(" and ac_id not ")
                Else
                    sQuery.Append(" and ac_id ")
                End If
                sQuery.Append(" in (" & Aircraft_IDS_String & ") ")
            End If

            If Trim(field_name) = "country_continent_name" Then
                ' we want to include the unknowns for the count 
            Else
                sQuery.Append("  and " & field_name & " <> '' and " & field_name & " is not null ")
            End If



            If Trim(rollup_text) <> "" Then
                sQuery.Append(Trim(rollup_text))
            End If

            If searchCriteria.ViewCriteriaAFTTStart > 0 Then
                sQuery.Append(Constants.cAndClause + " ((ac_airframe_tot_hrs >= " & searchCriteria.ViewCriteriaAFTTStart & ") or (ac_airframe_tot_hrs IS NULL))")
            End If

            If searchCriteria.ViewCriteriaAFTTEnd > 0 Then
                sQuery.Append(Constants.cAndClause + " ((ac_airframe_tot_hrs <= " & searchCriteria.ViewCriteriaAFTTEnd & ") or (ac_airframe_tot_hrs IS NULL))")
            End If



            If Trim(searchCriteria.ViewCriteriaContinent) = "International" Then
                sQuery.Append(" and (origin_aport_country not in ('United States', 'U.S.') or dest_aport_country not in ('United States', 'U.S.'))  ")
            End If


            If Trim(distance_string) <> "" And Len(Trim(distance_string)) > 2 Then
                sQuery.Append(distance_string)
            End If


            If searchCriteria.ViewCriteriaYearStart > 0 Then
                sQuery.Append(Constants.cAndClause + " ac_mfr_year >= " & searchCriteria.ViewCriteriaYearStart)
            End If

            If searchCriteria.ViewCriteriaYearEnd > 0 Then
                sQuery.Append(Constants.cAndClause + " ac_mfr_year <=  " & searchCriteria.ViewCriteriaYearEnd)
            End If

            Dim temp_use As String = " "
            If searchCriteria.ViewCriteriaHasBusinessFlag = True Or searchCriteria.ViewCriteriaHasCommercialFlag = True Or searchCriteria.ViewCriteriaHasHelicopterFlag = True Then
                sQuery.Append(" and ( ")
            End If

            If searchCriteria.ViewCriteriaHasBusinessFlag = True Then
                If Trim(temp_use) = "" Then
                    temp_use = " or "
                Else
                    sQuery.Append(temp_use)
                End If
                sQuery.Append(" ac_product_business_flag = 'Y' ")
            End If

            If searchCriteria.ViewCriteriaHasCommercialFlag = True Then
                If Trim(temp_use) = "" Then
                    temp_use = " or "
                Else
                    sQuery.Append(temp_use)
                End If
                sQuery.Append(" ac_product_commercial_flag = 'Y' ")
            End If

            If searchCriteria.ViewCriteriaHasHelicopterFlag = True Then
                If Trim(temp_use) = "" Then
                    temp_use = " or "
                Else
                    sQuery.Append(temp_use)
                End If
                sQuery.Append(" ac_product_helicopter_flag = 'Y' ")
            End If

            If searchCriteria.ViewCriteriaHasBusinessFlag = True Or searchCriteria.ViewCriteriaHasCommercialFlag = True Or searchCriteria.ViewCriteriaHasHelicopterFlag = True Then
                sQuery.Append(" ) ")
            End If



            If searchCriteria.ViewCriteriaAmodID > 0 Then
                sQuery.Append(" and amod_id = " & searchCriteria.ViewCriteriaAmodID & "")
            ElseIf Not IsNothing(searchCriteria.ViewCriteriaAmodIDArray) Then
                sQuery.Append(SetUpModelString(searchCriteria)) ', "amod_id", "ac_amod_id"))
            ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
                sQuery.Append(Replace(SetUpMakeString(searchCriteria), "amod_make_name", "amod_make_name"))
            End If

            If Not IsNothing(searchCriteria.ViewCriteriaTypeIDArray) Then
                sQuery.Append(Replace(Replace(SetUpTypeString(searchCriteria), "amod_airframe_type_code", "amod_airframe_type_code"), "amod_type_code", "amod_type_code"))
            End If

            If searchCriteria.ViewCriteriaCompanyID > 0 Then
                sQuery.Append(" and comp_id = " & searchCriteria.ViewCriteriaCompanyID & " ")
            End If

            sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))


            sQuery.Append(" group by  " & field_name & " ")

            sQuery.Append(" order by NbrFlights desc ")

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "utilization_functions.vb", sQuery.ToString)

            'HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />" & Date.Now & " - get_companies_from_airport(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

            SqlConn.ConnectionString = clientConnectString
            SqlConn.Open()
            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 600

            SqlCommand.CommandText = sQuery.ToString
            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                aError = "Error in util_get_opearators load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in util_get_opearators(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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
    Public Sub build_flight_data_subselects(ByRef sQuery As StringBuilder, Optional ByVal do_Replaces As Integer = 0)


        If do_Replaces = 1 Then
            sQuery.Append(", COUNT(ffd_unique_flight_id) as NbrFlights,")
            sQuery.Append(" SUM(convert(decimal(18,4),ffd_flight_time))/60 as TotalFlightTimeHrs, ")
            sQuery.Append(" SUM((ffd_flight_time* Aircraft_Model.amod_fuel_burn_rate)/60) as TotalFuelBurn ")
        Else
            sQuery.Append(", COUNT(ffd_unique_flight_id) as NbrFlights,")
            sQuery.Append(" SUM(convert(decimal(18,4),ffd_flight_time))/60 as TotalFlightTimeHrs, ")
            sQuery.Append(" SUM((ffd_flight_time* amod_fuel_burn_rate)/60) as TotalFuelBurn ")
        End If


    End Sub

    Public Sub build_flight_data_from(ByRef sQuery As StringBuilder)


        'sQuery.Append(" FROM view_flights WITH (NOLOCK)  ")


        sQuery.Append(" FROM View_Flights_New WITH (NOLOCK)  ")

        '  sQuery.Append(" FROM FAA_Flight_Data AS FFD2 WITH (NOLOCK) ")
        '  sQuery.Append(" INNER JOIN Aircraft AS A2 WITH (NOLOCK) ON A2.ac_id = FFD2.ffd_ac_id AND A2.ac_journ_id = FFD2.ffd_journ_id ")
        '  sQuery.Append(" INNER JOIN Aircraft_Reference AS AR2 WITH (NOLOCK) ON AR2.cref_ac_id = A2.ac_id AND AR2.cref_journ_id = AR2.cref_journ_id and AR2.cref_operator_flag  in ('Y', 'O') ")
        '  sQuery.Append(" INNER JOIN Aircraft_Model AS AM2 WITH (NOLOCK) ON AM2.amod_id = A2.ac_amod_id ")
        '  sQuery.Append(" inner join Company as C1 with (NOLOCK) on C1.comp_id = AR2.cref_comp_id and C1.comp_journ_id = AR2.cref_journ_id ")






        'sQuery.Append(" inner join Airport as AP1 with (NOLOCK) on AP1.aport_id=FFD2.ffd_dest_aport_id ")





    End Sub

    'Public Sub build_flight_data_where(ByRef sQuery As StringBuilder, ByVal aport_id As Long, ByRef ViewCriteria As viewSelectionCriteriaClass)

    '  'If InStr(LCase(sQuery.ToString), "where") > 0 Then
    '  '  sQuery.Append(" AND (ffd_hide_flag = 'N') ")
    '  'Else
    '  '  sQuery.Append(" WHERE (ffd_hide_flag = 'N') ")
    '  'End If


    '  If InStr(LCase(sQuery.ToString), "where") > 0 Then
    '    sQuery.Append(" AND (ffd_distance IS NOT NULL) ")
    '  Else
    '    sQuery.Append(" WHERE (ffd_distance IS NOT NULL) ")
    '  End If


    '  sQuery.Append(" AND (ffd_distance > 0) ")
    '  sQuery.Append(" AND (ffd_flight_time IS NOT NULL) ")
    '  sQuery.Append(" AND (ffd_flight_time > 0) ")


    '  If String.IsNullOrEmpty(ViewCriteria.ViewCriteriaDocumentsStartDate) And String.IsNullOrEmpty(ViewCriteria.ViewCriteriaDocumentsStartDate) Then
    '    sQuery.Append(" AND (CAST(ffd_origin_date AS DATE) >= DATEADD(month,-6,GETDATE())) ")
    '    sQuery.Append(" AND (CAST(ffd_origin_date AS DATE) >= A2.ac_purchase_date) ")
    '  Else
    '    sQuery.Append(" AND (CAST(ffd_origin_date AS DATE) >= '" & Month(ViewCriteria.ViewCriteriaDocumentsStartDate) & "/" & Day(ViewCriteria.ViewCriteriaDocumentsStartDate) & "/" & Year(ViewCriteria.ViewCriteriaDocumentsStartDate) & "') ")
    '    sQuery.Append(" AND (CAST(ffd_origin_date AS DATE) <= '" & Month(ViewCriteria.ViewCriteriaDocumentsEndDate) & "/" & Day(ViewCriteria.ViewCriteriaDocumentsEndDate) & "/" & Year(ViewCriteria.ViewCriteriaDocumentsEndDate) & "') ")

    '    'MAY NEED TO PUT BACK PURCAHSE DATE 
    '    ' sQuery.Append(" AND (CAST(ffd_origin_date AS DATE) >= A2.ac_purchase_date) ")
    '  End If


    '  ' sQuery.Append(" AND comp_journ_id = 0  ")


    '  If Trim(airport_direction) = "D" Then
    '    If aport_id > 0 Then
    '      sQuery.Append(" and ffd_dest_aport_id  = " & aport_id & " ")
    '    ElseIf Not String.IsNullOrEmpty(Airport_IDS_String) Then
    '      If exclude_airport_check Then
    '        sQuery.Append(" and ffd_dest_aport_id not in (" & Airport_IDS_String & ") ")
    '      Else
    '        sQuery.Append(" and ffd_dest_aport_id  in (" & Airport_IDS_String & ") ")
    '      End If 
    '    End If
    '  ElseIf Trim(airport_direction) = "O" Then
    '    If aport_id > 0 Then
    '      sQuery.Append(" and ffd_origin_aport_id  = " & aport_id & " ")
    '    ElseIf Not String.IsNullOrEmpty(Airport_IDS_String) Then
    '      If exclude_airport_check Then
    '        sQuery.Append(" and ffd_origin_aport_id not in (" & Airport_IDS_String & ") ")
    '      Else
    '        sQuery.Append(" and ffd_origin_aport_id  in (" & Airport_IDS_String & ") ")
    '      End If 
    '    End If
    '  Else
    '    If aport_id > 0 Then
    '      sQuery.Append(" and ffd_dest_aport_id  = " & aport_id & " ")
    '    ElseIf Not String.IsNullOrEmpty(Airport_IDS_String) Then
    '      If exclude_airport_check Then
    '        sQuery.Append(" and ffd_dest_aport_id not in (" & Airport_IDS_String & ") ")
    '      Else
    '        sQuery.Append(" and ffd_dest_aport_id  in (" & Airport_IDS_String & ") ")
    '      End If 
    '    End If
    '  End If




    '  Dim HoldClsSubscription As New crmSubscriptionClass

    '  HoldClsSubscription.crmAerodexFlag = HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag
    '  HoldClsSubscription.crmBusiness_Flag = ViewCriteria.ViewCriteriaHasBusinessFlag
    '  HoldClsSubscription.crmCommercial_Flag = ViewCriteria.ViewCriteriaHasCommercialFlag
    '  HoldClsSubscription.crmHelicopter_Flag = ViewCriteria.ViewCriteriaHasHelicopterFlag
    '  HoldClsSubscription.crmJets_Flag = HttpContext.Current.Session.Item("localSubscription").crmJets_Flag
    '  HoldClsSubscription.crmTurboprops = HttpContext.Current.Session.Item("localSubscription").crmTurboprops
    '  HoldClsSubscription.crmExecutive_Flag = HttpContext.Current.Session.Item("localSubscription").crmExecutive_Flag

    '  sQuery.Append(" " & clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(HoldClsSubscription, False, True))

    'End Sub

    Public Sub build_flight_data_where(ByRef sQuery As StringBuilder, ByVal aport_id As Long, ByRef ViewCriteria As viewSelectionCriteriaClass)
        Dim seperator As String = ""

        If InStr(LCase(sQuery.ToString), "where") > 0 Then
            seperator = " and "
        Else
            seperator = " WHERE "
        End If


        If String.IsNullOrEmpty(ViewCriteria.ViewCriteriaDocumentsStartDate) And String.IsNullOrEmpty(ViewCriteria.ViewCriteriaDocumentsStartDate) Then
            sQuery.Append(seperator & " (convert(date, ffd_date, 0) >= DATEADD(month,-6,GETDATE())) ")
            sQuery.Append(" and (convert(date, ffd_date, 0) >= A2.ac_purchase_date) ")
        Else
            sQuery.Append(seperator & " (convert(date, ffd_date, 0) >= '" & Month(ViewCriteria.ViewCriteriaDocumentsStartDate) & "/" & Day(ViewCriteria.ViewCriteriaDocumentsStartDate) & "/" & Year(ViewCriteria.ViewCriteriaDocumentsStartDate) & "') ")
            sQuery.Append(" and (convert(date, ffd_date, 0) <= '" & Month(ViewCriteria.ViewCriteriaDocumentsEndDate) & "/" & Day(ViewCriteria.ViewCriteriaDocumentsEndDate) & "/" & Year(ViewCriteria.ViewCriteriaDocumentsEndDate) & "') ")

            'MAY NEED TO PUT BACK PURCAHSE DATE 
            ' sQuery.Append(" AND (CAST(ffd_origin_date AS DATE) >= A2.ac_purchase_date) ")
        End If


        ' sQuery.Append(" AND comp_journ_id = 0  ")


        If Trim(airport_direction) = "D" Then
            If aport_id > 0 Then
                sQuery.Append(" and ffd_dest_aport_id  = " & aport_id & " ")
            ElseIf Not String.IsNullOrEmpty(Airport_IDS_String) Then
                If exclude_airport_check Then
                    sQuery.Append(" and ffd_dest_aport_id not in (" & Airport_IDS_String & ") ")
                Else
                    sQuery.Append(" and ffd_dest_aport_id  in (" & Airport_IDS_String & ") ")
                End If
            End If
        ElseIf Trim(airport_direction) = "O" Then
            If aport_id > 0 Then
                sQuery.Append(" and ffd_origin_aport_id  = " & aport_id & " ")
            ElseIf Not String.IsNullOrEmpty(Airport_IDS_String) Then
                If exclude_airport_check Then
                    sQuery.Append(" and ffd_origin_aport_id not in (" & Airport_IDS_String & ") ")
                Else
                    sQuery.Append(" and ffd_origin_aport_id  in (" & Airport_IDS_String & ") ")
                End If
            End If
        ElseIf Trim(airport_direction) = "X" Then
            '  sQuery.Append(" and ffd_dest_aport_id > 0")
            ' sQuery.Append(" AND (ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportIATA & "' or ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportICAO & "')")

            If Airport_ID_OVERALL > 1 Then
                sQuery.Append(" AND ((ffd_origin_aport_id = '" & Airport_ID_OVERALL & "') or (ffd_dest_aport_id = '" & Airport_ID_OVERALL & "')) ")
            ElseIf Not String.IsNullOrEmpty(Airport_IDS_String) Then
                If exclude_airport_check Then
                    sQuery.Append(" AND (ffd_origin_aport_id not in (" & Airport_IDS_String & ")) AND (ffd_dest_aport_id not in (" & Airport_IDS_String & "))   ")
                Else
                    sQuery.Append(" AND (   (ffd_origin_aport_id in (" & Airport_IDS_String & "))  or (ffd_dest_aport_id in (" & Airport_IDS_String & "))   )")
                End If

            End If

        Else
            If aport_id > 0 Then
                sQuery.Append(" and ffd_dest_aport_id  = " & aport_id & " ")
            ElseIf Not String.IsNullOrEmpty(Airport_IDS_String) Then
                If exclude_airport_check Then
                    sQuery.Append(" and ffd_dest_aport_id not in (" & Airport_IDS_String & ") ")
                Else
                    sQuery.Append(" and ffd_dest_aport_id  in (" & Airport_IDS_String & ") ")
                End If
            End If
        End If




        Dim HoldClsSubscription As New crmSubscriptionClass

        HoldClsSubscription.crmAerodexFlag = HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag
        HoldClsSubscription.crmBusiness_Flag = ViewCriteria.ViewCriteriaHasBusinessFlag
        HoldClsSubscription.crmCommercial_Flag = ViewCriteria.ViewCriteriaHasCommercialFlag
        HoldClsSubscription.crmHelicopter_Flag = ViewCriteria.ViewCriteriaHasHelicopterFlag
        HoldClsSubscription.crmJets_Flag = HttpContext.Current.Session.Item("localSubscription").crmJets_Flag
        HoldClsSubscription.crmTurboprops = HttpContext.Current.Session.Item("localSubscription").crmTurboprops
        HoldClsSubscription.crmExecutive_Flag = HttpContext.Current.Session.Item("localSubscription").crmExecutive_Flag

        sQuery.Append(" " & clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(HoldClsSubscription, False, True))

    End Sub

    Public Function get_companies_from_airport(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal company_type As String, ByVal run_export As String, ByVal aport_id As Long, ByVal use_ac As Boolean, ByRef TotalFlightsLimit As Long, Optional ByRef LimitThisQuery As Long = 0, Optional ByVal exclude_based_operators As Boolean = False) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try
            '-- ***************  LOWER TAB 4 - OWNERS ************************
            '-- SELECT A LIST OF COMPANIES OWNING AIRCRAFT AT SPECIFIC AIRPORT 

            'If Trim(run_export) = "A" Then
            '  sQuery.Append(" select distinct comp_name as CompanyName, comp_address1 as Address, comp_city as City, comp_state as State, comp_web_address as WebAddress ")

            '  If use_ac = True Then
            '    sQuery.Append(", ac_list_date as ListDate, amod_make_name as Make, amod_model_name as Model, ac_mfr_year as MFRYear, ac_forsale_flag as ForSale, ac_year as Year, ")
            '    sQuery.Append(" ac_ser_no_full as SerNbr, ac_reg_no as RegNbr")
            '  End If

            'ElseIf Trim(run_export) = "C" Then
            '  sQuery.Append(" select distinct comp_name as CompanyName, comp_address1 as Address, comp_city as City, comp_state as State, comp_web_address as WebAddress ")
            'Else
            sQuery.Append(" select distinct ")
            If LimitThisQuery > 0 Then
                sQuery.Append(" top " + LimitThisQuery.ToString + " ")
            Else
                If TotalFlightsLimit > 50000 Then
                    sQuery.Append(" top 500 ")
                End If
            End If
            sQuery.Append(" comp_id, comp_name, cbus_name, comp_city, comp_state, comp_country, comp_email_address,comp_off_phone, comp_address1")
            sQuery.Append(", country_continent_name ")

            '  sQuery.Append(" company.comp_id, company.comp_name, Company_Business_Type.cbus_name, company.comp_city, company.comp_state, company.comp_country, company.comp_email_address,  pnum_number_full as  comp_off_phone, company.comp_address1 ")
            '   sQuery.Append(", country_continent_name ")

            ' sQuery.Append(" contact_first_name , contact_last_name , contact_title , contact_email_address,contact_off_phone ,contact_mob_phone, ")

            Call build_flight_data_subselects(sQuery)

            sQuery.Append(", (SUM(ffd_distance)/count(*)) as AvgDistance ")
            sQuery.Append(" , (SUM(ffd_flight_time)/count(*)) as AvgMinPerFlights  ")

            Call build_flight_data_from(sQuery)

            ' ADDED MSW - 5/05/2020
            '    sQuery.Append("  Left outer join aircraft_company_role with (NOLOCK) on View_Flights.ac_id = acomprole_ac_id  ")
            '    sQuery.Append(" And (convert(date,View_Flights.ffd_date) >= acomprole_start_date And (convert(date,View_Flights.ffd_date) <= acomprole_end_date Or acomprole_end_date Is NULL)) ")
            '    sQuery.Append(" Left outer join company on Company.comp_id = acomprole_comp_id And comp_journ_id = 0 ")

            '    sQuery.Append(" left outer join country with (NOLOCK) on  country_name = Replace(comp_country, 'U.S.', 'United States')   ")
            sQuery.Append(" left outer join country with (NOLOCK) on  country_name = Replace(comp_country, 'U.S.', 'United States')   ")

            '   sQuery.Append(" Left outer join Phone_Numbers with (NOLOCK) on pnum_comp_id = company.comp_id And pnum_journ_id = comp_journ_id And pnum_type = 'Office'  ")
            '   sQuery.Append(" inner Join Company_Business_Type with (NOLOCK) on cbus_type = comp_business_type ")



            Call build_flight_data_where(sQuery, aport_id, searchCriteria)

            If searchCriteria.ViewCriteriaAmodID > -1 Then
                sQuery.Append(crmWebClient.Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
            ElseIf Not IsNothing(searchCriteria.ViewCriteriaAmodIDArray) Then
                sQuery.Append(SetUpModelString(searchCriteria))
            ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
                sQuery.Append(SetUpMakeString(searchCriteria))
                'sQuery.Append(crmWebClient.Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'") 
            End If

            If Not IsNothing(searchCriteria.ViewCriteriaTypeIDArray) Then
                sQuery.Append(SetUpTypeString(searchCriteria))
            End If

            If Trim(searchCriteria.ViewCriteriaContinent) = "International" Then
                sQuery.Append(" and (origin_aport_country not in ('United States', 'U.S.') or dest_aport_country not in ('United States', 'U.S.'))  ")
            End If

            If exclude_based_operators = True Then
                If aport_id > 0 Then
                    sQuery.Append(" and base_aport_id <> " & aport_id & " ")
                ElseIf Not String.IsNullOrEmpty(Airport_IDS_String) Then
                    sQuery.Append(" and base_aport_id  not in (" & Airport_IDS_String & ") ")
                End If
            Else
                If aport_id > 0 Then
                    sQuery.Append(" and base_aport_id  = " & aport_id & " ")
                ElseIf Not String.IsNullOrEmpty(Airport_IDS_String) Then
                    If exclude_airport_check Then
                        sQuery.Append(" and base_aport_id  not in (" & Airport_IDS_String & ") ")
                    Else
                        sQuery.Append(" and base_aport_id  in (" & Airport_IDS_String & ") ")
                    End If

                End If
            End If


            If Not String.IsNullOrEmpty(Trim(Aircraft_IDS_String)) Then
                If exclude_Aircraft = True Then
                    sQuery.Append(" and ac_id not ")
                Else
                    sQuery.Append(" and ac_id ")
                End If
                sQuery.Append(" in (" & Aircraft_IDS_String & ") ")
            End If


            If Trim(distance_string) <> "" And Len(Trim(distance_string)) > 2 Then
                sQuery.Append(distance_string)
            End If

            If Trim(Operator_IDS_String) <> "" Then
                If exclude_check = True Then
                    sQuery.Append(" and comp_id not ")
                Else
                    sQuery.Append(" and comp_id ")
                End If
                sQuery.Append(" in (" & Operator_IDS_String & ") ")
            End If

            Dim temp_use As String = " "
            If searchCriteria.ViewCriteriaHasBusinessFlag = True Or searchCriteria.ViewCriteriaHasCommercialFlag = True Or searchCriteria.ViewCriteriaHasHelicopterFlag = True Then
                sQuery.Append(" and ( ")
            End If

            If searchCriteria.ViewCriteriaHasBusinessFlag = True Then
                If Trim(temp_use) = "" Then
                    temp_use = " or "
                Else
                    sQuery.Append(temp_use)
                End If
                sQuery.Append(" ac_product_business_flag = 'Y' ")
            End If

            If searchCriteria.ViewCriteriaHasCommercialFlag = True Then
                If Trim(temp_use) = "" Then
                    temp_use = " or "
                Else
                    sQuery.Append(temp_use)
                End If
                sQuery.Append(" ac_product_commercial_flag = 'Y' ")
            End If

            If searchCriteria.ViewCriteriaHasHelicopterFlag = True Then
                If Trim(temp_use) = "" Then
                    temp_use = " or "
                Else
                    sQuery.Append(temp_use)
                End If
                sQuery.Append(" ac_product_helicopter_flag = 'Y' ")
            End If

            If searchCriteria.ViewCriteriaHasBusinessFlag = True Or searchCriteria.ViewCriteriaHasCommercialFlag = True Or searchCriteria.ViewCriteriaHasHelicopterFlag = True Then
                sQuery.Append(" ) ")
            End If


            ' ADDED MSW - 3/10/20 TO BUILD IN DROP DOWN FOR IN OPERATION 
            If Not IsNothing(searchCriteria.viewCriteriaInOperation) Then
                If Trim(searchCriteria.viewCriteriaInOperation) <> "" Then
                    sQuery.Append(Build_In_Operation_String(searchCriteria))
                End If
            End If



            If searchCriteria.ViewCriteriaCompanyID > 0 Then
                sQuery.Append(" and comp_id = " & searchCriteria.ViewCriteriaCompanyID & " ")
            End If

            sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))



            sQuery.Append(" group by comp_id, comp_name, cbus_name, comp_city, comp_state, comp_country,comp_email_address,comp_off_phone, comp_address1 ")
            sQuery.Append(", country_continent_name ")

            'sQuery.Append(" group by company.comp_id, company.comp_name, Company_Business_Type.cbus_name, company.comp_city, company.comp_state, company.comp_country, company.comp_email_address,  pnum_number_full,  company.comp_address1 ")
            'sQuery.Append(", country_continent_name ")

            '  sQuery.Append(" contact_first_name , contact_last_name , contact_title , contact_email_address,contact_off_phone ,contact_mob_phone,")

            'If TotalFlightsLimit > 10000 Then
            '  sQuery.Append(" having(COUNT(FFD2.ffd_unique_flight_id) > 4) ")
            'ElseIf TotalFlightsLimit = 0 Or TotalFlightsLimit > 1000 Then
            '  sQuery.Append(" having(COUNT(FFD2.ffd_unique_flight_id) > 1) ")
            'End If



            sQuery.Append(" order by comp_name asc ")


            'HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />" & Date.Now & " - get_companies_from_airport(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString
            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "utilization_functions.vb", sQuery.ToString.ToString)

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
                aError = "Error in get_fractional_shares load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_fractional_shares(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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


    Public Function get_bus_type_from_companies_from_airport(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal company_distance As Integer, ByVal orig_lat As Double, ByVal orig_long As Double) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim max_NORTH As Double = 0.0
        Dim max_SOUTH As Double = 0.0
        Dim max_WEST As Double = 0.0
        Dim max_EAST As Double = 0.0
        Dim query_distance As String = ""



        Try
            '-- ***************  TAB UNKNOWN - LIST OF BUSINESS TYPES AT AIRPORT ************************
            '-- SELECT A SUMMARY OF COMPANIES LOCATED AT SPECIFIC AIRPORT OR SAME CITY
            sQuery.Append(" select cbus_name, cbus_type, COUNT(*) as tcount ")
            sQuery.Append(" from Company ")
            sQuery.Append(" inner JOIN Airport WITH(NOLOCK)on comp_country=aport_country  ")
            If searchCriteria.ViewCriteriaCountry = "United States" Then ' MSW - 2/27/20 - FOREIGN ONES DONT HAVE A STATE, SO DONT JOIN HERE 
                sQuery.Append(" and comp_state=aport_state  ")
            End If
            sQuery.Append(" and comp_city=aport_city ")
            sQuery.Append(" inner JOIN Business_Type_Reference WITH(NOLOCK)on comp_id = bustypref_comp_id and comp_journ_id = bustypref_journ_id  ")
            sQuery.Append(" inner JOIN Company_Business_Type WITH(NOLOCK)on bustypref_type= cbus_type ")

            If searchCriteria.ViewCriteriaCountry = "United States" Then
                sQuery.Append(" inner join Zip_Mapping with (NOLOCK) on left(comp_zip_code,5) = zmap_zip_code ")
            Else

            End If

            If searchCriteria.ViewCriteriaCountry = "United States" Then

                If company_distance > 0 Then
                    query_distance = CDbl(company_distance / 69).ToString 'http://www.distancebetweencities.net/ helped us 
                Else
                    query_distance = "2.1739"
                End If

                max_WEST = FormatNumber(orig_long + query_distance, 6)
                max_EAST = FormatNumber(orig_long - query_distance, 6)
                max_NORTH = FormatNumber(orig_lat + query_distance, 6)
                max_SOUTH = FormatNumber(orig_lat - query_distance, 6)

                sQuery.Append("where (zmap_longitude <= " & max_WEST & " AND zmap_longitude >= " & max_EAST & ")  ")
                sQuery.Append("AND (zmap_latitude <= " & max_NORTH & " AND zmap_latitude >= " & max_SOUTH & ")  ")
            ElseIf searchCriteria.ViewCriteriaAirportName = "" Then
                sQuery.Append(" ")
            Else
                sQuery.Append(" where (aport_iata_code = '" & searchCriteria.ViewCriteriaAirportIATA & "' ")

                If Trim(searchCriteria.ViewCriteriaAirportICAO) <> "" Then
                    sQuery.Append(" or aport_iata_code = '" & searchCriteria.ViewCriteriaAirportICAO & "') ")
                Else
                    sQuery.Append(" ) ")
                End If
            End If

            If searchCriteria.ViewCriteriaCountry = "United States" Then
                sQuery.Append(" and comp_country='United States' ")
            End If


            sQuery.Append(" and comp_journ_id = 0 ")
            sQuery.Append(" group by cbus_name, cbus_type ")
            sQuery.Append(" order by cbus_name ")


            'HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />" & Date.Now & " - get_bus_type_from_companies_from_airport(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString
            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "utilization_functions.vb", sQuery.ToString.ToString)

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
                aError = "Error in get_fractional_shares load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_fractional_shares(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

    Public Sub get_company_purchase_history_top_function(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByVal roll_up As String, ByVal from_spot As String, Optional ByRef graph1 As System.Web.UI.DataVisualization.Charting.Chart = Nothing)

        Dim results_table As New DataTable
        Dim results_table2 As New DataTable
        Dim htmlOut As New StringBuilder
        Dim htmlOut_graph As New StringBuilder
        Dim toggleRowColor As Boolean = False
        Dim graphID As Integer = 1
        Dim temp_string As String = ""
        Dim start_temp_string As String = ""
        Dim type_temp As String = ""
        Dim tcompare1 As String = ""
        Dim tcompare2 As String = ""
        Dim start_date As String = ""
        Dim end_date As String = ""
        Dim mid_date As String = ""
        Dim mid_date2 As String = ""
        Dim high_number As Long = 0
        Dim low_number As Long = 100000
        Dim starting_point As Integer = 0
        Dim interval_point As Integer = 1
        Dim ending_point As Integer = 0


        Try

            'use_faa_date
            results_table = get_company_purchase_history(searchCriteria, roll_up)



            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    If Trim(from_spot) = "pdf" Then
                        graph1.Series.Clear()
                        graph1.Series.Add("AVG_PRICE").ChartType = UI.DataVisualization.Charting.SeriesChartType.Spline
                        graph1.Series("AVG_PRICE").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32
                        graph1.Series("AVG_PRICE").LabelForeColor = Drawing.Color.Blue
                        graph1.ChartAreas("ChartArea1").AxisY.Title = "Average Asking Price - US $"
                        graph1.Series("AVG_PRICE").Color = Drawing.Color.Blue
                        graph1.Series("AVG_PRICE").BorderWidth = 2
                        graph1.Series("AVG_PRICE").MarkerSize = 6
                        graph1.Series("AVG_PRICE").MarkerStyle = UI.DataVisualization.Charting.MarkerStyle.Circle
                        '  Me.AVG_PRICE_MONTH.BorderlineWidth = 10
                        graph1.Series("AVG_PRICE").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32
                        'Me.AVG_PRICE_MONTH.Series("AVG_PRICE").SmartLabelStyle.Enabled = False
                        'Me.AVG_PRICE_MONTH.Series("AVG_PRICE").LabelAngle = 45
                        ' If Me.WD.SelectedValue = "Word" or Me.WD.SelectedValue = "WordX" Then
                        graph1.Width = 500
                        graph1.Height = 500
                    Else
                        start_temp_string = " data1.addColumn('string', 'YearSolds'); "
                        start_temp_string &= " data1.addColumn('number', 'xxxx'); "
                        start_temp_string &= " data1.addColumn('number', 'Purchased Per Year'); "
                        start_temp_string &= " data1.addColumn('number', 'Est/Sold Value'); "
                        start_temp_string &= " data1.addColumn('number', 'My AC Asking'); "
                        start_temp_string &= " data1.addColumn('number', 'My AC Take'); "
                        start_temp_string &= " data1.addColumn('number', 'My AC Est Value'); "

                        start_temp_string &= "data1.addRows(["
                    End If



                    For Each r As DataRow In results_table.Rows

                        If Not IsDBNull(r.Item("tcount")) Then
                            If IsNumeric(r.Item("tcount")) Then

                                If Trim(from_spot) = "pdf" Then
                                    graph1.Series("AVG_PRICE").Points.AddXY(r.Item("tyear").ToString, r.Item("tcount").ToString)

                                    If CDbl(r("tcount").ToString) > high_number Then
                                        high_number = CDbl(r("tcount").ToString)
                                    End If
                                    If CDbl(r("tcount").ToString) < low_number Then
                                        low_number = CDbl(r("tcount").ToString)
                                    End If
                                End If


                                If CDbl(r.Item("tcount")) = 0 Then
                                    If Trim(temp_string) <> "" Then
                                        temp_string &= ", "
                                    End If
                                    temp_string &= "['" & r.Item("tyear").ToString & "',null, 0, null,  null, null, null]"
                                Else
                                    If Trim(temp_string) <> "" Then
                                        temp_string &= ", "
                                    End If
                                    temp_string &= "['" & r.Item("tyear").ToString & "', null," & Replace(r.Item("tcount").ToString, ",", "") & ", null,  null, null, null]"
                                End If


                            End If
                        End If

                    Next



                    commonEvo.set_ranges_for_vsCharts(low_number, high_number, interval_point, starting_point, ending_point)

                    graph1.ChartAreas("ChartArea1").AxisY.Maximum = ending_point
                    graph1.ChartAreas("ChartArea1").AxisY.Minimum = IIf(starting_point > 0, starting_point, 0)
                    graph1.ChartAreas("ChartArea1").AxisY.Interval = interval_point

                Else
                    'htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""seperator"" style=""padding-left:3px;"">No FAA Flight Activity Found.</td></tr>")
                End If
            Else
                ' htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""seperator"" style=""padding-left:3px;"">No FAA Flight Activity Found.</td></tr>")
            End If


        Catch ex As Exception

            aError = "Error in views_display_fractions_to_expire(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        'return resulting html string


        If Trim(from_spot) = "pdf" Then

            htmlOut.Length = 0   ' clear it, then go add in the pictre 
            graph1.Titles.Clear()
            graph1.ImageType = DataVisualization.Charting.ChartImageType.Jpeg
            graph1.SaveImage(HttpContext.Current.Server.MapPath(HttpContext.Current.Session.Item("MarketSummaryFolderVirtualPath").ToString) + "\" & HttpContext.Current.Session.Item("localUser").crmUserTemporaryFilePrefix & searchCriteria.ViewCriteriaCompanyID.ToString + "_OP_GRAPH1.jpg", DataVisualization.Charting.ChartImageFormat.Jpeg)
            htmlOut.Append("<table width='80%' align='center'>")
            htmlOut.Append("<tr class='" & HttpContext.Current.Session.Item("ROW_CLASS_BOTTOM") & "'><td colspan='2' align='center'><font class='" & HttpContext.Current.Session.Item("FONT_CLASS_HEADER") & "'>Purchase History</font></td></tr>")
            htmlOut.Append("<tr><td align='center'><img src='" & HttpContext.Current.Session.Item("localUser").crmUserTemporaryFilePrefix & searchCriteria.ViewCriteriaCompanyID.ToString + "_OP_GRAPH1.jpg'><br></td></tr>")
            htmlOut.Append("</table>")

            out_htmlString = htmlOut.ToString
        Else
            out_htmlString = start_temp_string & temp_string
        End If

        htmlOut = Nothing
        results_table = Nothing

    End Sub

    Public Sub get_flight_profile_top_function(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, Optional ByVal type_of As String = "Month", Optional ByVal use_faa_date As String = "", Optional ByVal product_code_selection As String = "", Optional ByVal from_spot As String = "", Optional ByRef graph1 As System.Web.UI.DataVisualization.Charting.Chart = Nothing, Optional ByRef table_string As String = "", Optional ByRef temp_ticks_string As String = "")

        Dim results_table As New DataTable
        Dim results_table2 As New DataTable
        Dim htmlOut As New StringBuilder
        Dim htmlOut_graph As New StringBuilder
        Dim toggleRowColor As Boolean = False
        Dim graphID As Integer = 1
        Dim temp_string As String = ""
        Dim start_temp_string As String = ""
        Dim type_temp As String = ""
        Dim tcompare1 As String = ""
        Dim tcompare2 As String = ""
        Dim start_date As String = ""
        Dim end_date As String = ""
        Dim mid_date As String = ""
        Dim mid_date2 As String = ""
        Dim bgcolor As String = ""
        Dim font_text_title As String = ""
        Dim font_text_start As String = ""
        Dim font_text_end As String = ""
        Dim temp_dir As String = "right"
        Dim high_number As Long = 0
        Dim low_number As Long = 100000
        Dim starting_point As Integer = 0
        Dim interval_point As Integer = 1
        Dim ending_point As Integer = 0
        Dim ticks_string As String = ""

        Try

            '        type_of = "Hours"

            If Trim(from_spot) = "pdf" Or Trim(from_spot) = "pdf2" Or Trim(from_spot) = "pdf3" Then
                font_text_start = "<font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>"
                font_text_title = "<font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT_TITLE") & "'>"
                font_text_end = "</font>"
                temp_dir = "left"
            Else
                font_text_start = ""
                font_text_title = ""
                font_text_end = ""
            End If

            If IsNothing(use_faa_date) Then
                use_faa_date = ""
            End If
            'use_faa_date
            results_table = get_flight_profile(searchCriteria, type_of, False, use_faa_date, product_code_selection)
            'results_table2 = get_flight_profile(searchCriteria, type_of, True, use_faa_date, product_code_selection)





            If Trim(from_spot) = "pdf" Then
                htmlOut.Append("<tr><td valign=""top"" align=""center"" class=""header""><font class='" & HttpContext.Current.Session("FONT_CLASS_HEADER") & "'>Flight Profile</font></td></tr>")
            ElseIf Trim(from_spot) = "pdf2" Or Trim(from_spot) = "pdf3" Then
                ' htmlOut.Append("<tr><th valign=""top"" align=""center"" class=""header""><font class='" & HttpContext.Current.Session("FONT_CLASS_HEADER_NOALIGN") & "'>Flight Summary</font></th></tr>")
            Else
                htmlOut.Append("<table id=""fractionsExpiringOuterTable"" width=""100%"" cellpadding=""0"" cellspacing=""0"" class=""module"">")
                htmlOut.Append("<tr><td valign=""top"" align=""center"" class=""header"">Flight Profile</td></tr>")
            End If



            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    htmlOut.Append("<tr><td valign=""top"" align=""left"">")

                    htmlOut.Append("<table id=""fractionsExpiringInnerTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"">")

                    htmlOut.Append("<tr><td colspan=""2"" class=""rightside"" valign=""top"">")

                    If Trim(from_spot) = "pdf" Or Trim(from_spot) = "pdf2" Or Trim(from_spot) = "pdf3" Then
                    Else
                        htmlOut.Append("<div style=""height: 250px; overflow: auto;"">")
                    End If

                    htmlOut.Append("<table id=""fractionsExpiringDataTable"" width=""100%"" cellpadding=""4"" cellspacing=""0"">")

                    If Trim(type_of) = "Hours" Then
                        htmlOut.Append("<thead><tr><th valign=""top"" align=""left"" class=""seperator"" width=""80%"">" & font_text_title & "<strong>Month/Year</strong>" & font_text_end & "</th>")
                        htmlOut.Append("<th valign=""top"" align=""right"" class=""seperator"" width=""20%"">" & font_text_title & "<strong>#&nbsp;Hours</strong>" & font_text_end & "</th></tr></thead><tbody>")
                    ElseIf Trim(from_spot) = "pdf2" Or Trim(from_spot) = "pdf3" Then
                        htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""seperator"" width=""80%"">" & font_text_title & "<strong>Month/Year</strong>" & font_text_end & "</td>")
                        htmlOut.Append("<td valign=""top"" align=""right"" class=""seperator"" width=""20%"">" & font_text_title & "<strong>#&nbsp;Flights</strong>" & font_text_end & "</td></tr>")
                    Else
                        htmlOut.Append("<thead><tr><th valign=""top"" align=""left"" class=""seperator"" width=""80%"">" & font_text_title & "<strong>Month/Year</strong>" & font_text_end & "</th>")
                        htmlOut.Append("<th valign=""top"" align=""right"" class=""seperator"" width=""20%"">" & font_text_title & "<strong>#&nbsp;Flights</strong>" & font_text_end & "</th></tr></thead><tbody>")
                    End If


                    ''set dates, to today, a year ago, and 2 years ago using faa date or today as start
                    'If Trim(use_faa_date) = "" Then
                    '  start_date = DateAdd(DateInterval.Day, 1, DateAdd(DateInterval.Year, -2, Date.Now.Date)) ' go forward one day, so we dont get same date last year 
                    '  mid_date = DateAdd(DateInterval.Year, -1, Date.Now.Date)
                    '  mid_date2 = DateAdd(DateInterval.Day, 1, CDate(mid_date))
                    '  end_date = Date.Now.Date
                    'Else
                    '  start_date = DateAdd(DateInterval.Day, 1, DateAdd(DateInterval.Year, -2, CDate(use_faa_date)))
                    '  start_date = CDate(Month(start_date) & "/01/" & Year(start_date))

                    '  mid_date = DateAdd(DateInterval.Year, -1, CDate(use_faa_date))
                    '  '  mid_date = DateAdd(DateInterval.Year, 1, CDate(start_date))
                    '  ' mid_date = DateAdd(DateInterval.Day, -1, CDate(mid_date))

                    '  mid_date2 = DateAdd(DateInterval.Year, 1, CDate(start_date))
                    '  end_date = CDate(use_faa_date)
                    '  ' 
                    '  '  mid_date2 = DateAdd(DateInterval.Day, 1, CDate(mid_date))
                    '  '  end_date = CDate(use_faa_date)
                    'End If



                    If Trim(from_spot) = "pdf" Or Trim(from_spot) = "pdf2" Or Trim(from_spot) = "pdf3" Then

                        If Trim(from_spot) = "pdf" Then
                            graph1.Series.Clear()
                            graph1.Series.Add("AVG_PRICE").ChartType = UI.DataVisualization.Charting.SeriesChartType.Spline
                            graph1.Series("AVG_PRICE").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32
                            graph1.Series("AVG_PRICE").LabelForeColor = Drawing.Color.Blue
                            graph1.ChartAreas("ChartArea1").AxisY.Title = "Average Asking Price - US $"
                            graph1.Series("AVG_PRICE").Color = Drawing.Color.Blue
                            graph1.Series("AVG_PRICE").BorderWidth = 2
                            graph1.Series("AVG_PRICE").MarkerSize = 6
                            graph1.Series("AVG_PRICE").MarkerStyle = UI.DataVisualization.Charting.MarkerStyle.Circle
                            '  Me.AVG_PRICE_MONTH.BorderlineWidth = 10
                            graph1.Series("AVG_PRICE").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32
                            'Me.AVG_PRICE_MONTH.Series("AVG_PRICE").SmartLabelStyle.Enabled = False
                            'Me.AVG_PRICE_MONTH.Series("AVG_PRICE").LabelAngle = 45
                            ' If Me.WD.SelectedValue = "Word" or Me.WD.SelectedValue = "WordX" Then
                            graph1.Width = 500
                            graph1.Height = 500
                        End If
                        '  End If

                    ElseIf Trim(from_spot) = "company" Then
                        Call get_past_dates(use_faa_date, start_date, mid_date2, mid_date, end_date)
                        start_temp_string = " data1.addColumn('string', 'Month/Year'); "
                        start_temp_string &= " data1.addColumn('number', '# Arrivals " & mid_date2 & " to " & end_date & "'); "
                        start_temp_string &= " data1.addColumn('number', '# Flights " & start_date & " to " & mid_date & "'); "
                        start_temp_string &= " data1.addColumn('number', 'Est/Sold Value'); "
                        start_temp_string &= " data1.addColumn('number', 'My AC Asking'); "
                        start_temp_string &= " data1.addColumn('number', 'My AC Take'); "
                        start_temp_string &= " data1.addColumn('number', 'My AC Est Value'); "
                    Else
                        Call get_past_dates(use_faa_date, start_date, mid_date2, mid_date, end_date)
                        If Trim(from_spot) = "pdf4" Then
                            start_temp_string = " data1.addColumn('string', 'Month/Year'); "
                            start_temp_string &= " data1.addColumn('number', '# Arrivals " & searchCriteria.ViewCriteriaDocumentsStartDate & " to " & searchCriteria.ViewCriteriaDocumentsEndDate & "'); "
                        ElseIf Trim(type_of) = "Month" Then
                            start_temp_string = " data1.addColumn('string', 'Month/Year'); "
                            start_temp_string &= " data1.addColumn('number', '# Arrivals " & searchCriteria.ViewCriteriaDocumentsStartDate & " to " & searchCriteria.ViewCriteriaDocumentsEndDate & "'); "
                            ' start_temp_string &= " data1.addColumn('number', '# Arrivals " & searchCriteria.ViewCriteriaDocumentsStartDate & " to " & searchCriteria.ViewCriteriaDocumentsEndDate & "'); "
                            ' start_temp_string &= " data1.addColumn('number', 'Est/Sold Value'); "
                            ' start_temp_string &= " data1.addColumn('number', 'My AC Asking'); "
                            ' start_temp_string &= " data1.addColumn('number', 'My AC Take'); "
                            ' start_temp_string &= " data1.addColumn('number', 'My AC Est Value'); "
                        ElseIf Trim(type_of) = "BWeight" Or Trim(type_of) = "TWeight" Or Trim(type_of) = "HWeight" Then
                            start_temp_string = " data1.addColumn('string', 'Weight'); "
                            start_temp_string &= " data1.addColumn('number', '# Arrivals " & searchCriteria.ViewCriteriaDocumentsStartDate & " to " & searchCriteria.ViewCriteriaDocumentsEndDate & "'); "
                            ' start_temp_string &= " data1.addColumn('number', '# Arrivals " & searchCriteria.ViewCriteriaDocumentsStartDate & " to " & searchCriteria.ViewCriteriaDocumentsEndDate & "'); "
                        ElseIf Trim(type_of) = "Type" Then
                            start_temp_string = " data1.addColumn('string', 'Type'); "
                            start_temp_string &= " data1.addColumn('number', '# Arrivals " & searchCriteria.ViewCriteriaDocumentsStartDate & " to " & searchCriteria.ViewCriteriaDocumentsEndDate & "'); "
                            ' start_temp_string &= " data1.addColumn('number', '# Arrivals " & searchCriteria.ViewCriteriaDocumentsStartDate & " to " & searchCriteria.ViewCriteriaDocumentsEndDate & "'); "
                        ElseIf Trim(type_of) = "Hours" Then
                            start_temp_string = " data1.addColumn('string', 'Type'); "
                            start_temp_string &= " data1.addColumn('number', '# Hours " & searchCriteria.ViewCriteriaDocumentsStartDate & " to " & searchCriteria.ViewCriteriaDocumentsEndDate & "'); "
                        End If
                    End If








                    start_temp_string &= "data1.addRows(["


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
                        ElseIf Trim(from_spot) = "pdf2" Or Trim(from_spot) = "pdf3" Then
                            htmlOut.Append("<tr>")
                        Else
                            If Not toggleRowColor Then
                                htmlOut.Append("<tr class=""alt_row"">")
                                toggleRowColor = True
                            Else
                                htmlOut.Append("<tr bgcolor=""white"">")
                                toggleRowColor = False
                            End If
                        End If


                        If Trim(type_of) = "Month" Or Trim(type_of) = "Hours" Then

                            If Trim(from_spot) = "pdf" Or Trim(from_spot) = "pdf2" Or Trim(from_spot) = "pdf3" Then
                                If Trim(from_spot) = "pdf" Then
                                    graph1.Series("AVG_PRICE").Points.AddXY((r.Item("tmonth").ToString + "-" & Right(r.Item("tyear").ToString, 2)), r.Item("tcount").ToString)
                                End If

                                If CDbl(r("tcount").ToString) > high_number Then
                                    high_number = CDbl(r("tcount").ToString)
                                End If
                                If CDbl(r("tcount").ToString) < low_number Then
                                    low_number = CDbl(r("tcount").ToString)
                                End If
                            ElseIf Trim(from_spot) = "pdf4" Then
                                If CDbl(r("tcount").ToString) > high_number Then
                                    high_number = CDbl(r("tcount").ToString)
                                End If
                                If CDbl(r("tcount").ToString) < low_number Then
                                    low_number = CDbl(r("tcount").ToString)
                                End If
                            End If


                            tcompare1 = r.Item("tmonth").ToString
                            htmlOut.Append("<td align=""left"" valign=""top"" class=""seperator"" width=""80%"">" & font_text_start & "")

                            htmlOut.Append("" & r.Item("tmonth").ToString & "/" & r.Item("tyear").ToString & "" & font_text_end & "</td>")


                            If Not IsDBNull(r.Item("tcount")) Then
                                If IsNumeric(r.Item("tcount")) Then
                                    htmlOut.Append("<td align=""right"" valign=""top"" class=""seperator"" width=""20%"" style=""padding-right:15px;"">" & font_text_start & "" & FormatNumber(r.Item("tcount"), 0) & "" & font_text_end & "</td></tr>")
                                Else
                                    htmlOut.Append("<td align=""right"" valign=""top"" class=""seperator"" width=""20%"" style=""padding-right:15px;"">" & font_text_start & "" + r.Item("tcount").ToString + "" & font_text_end & "</td></tr>")
                                End If
                            Else
                                htmlOut.Append("<td align=""right"" valign=""top"" class=""seperator"" width=""20%"" style=""padding-right:15px;"">" & font_text_start & "" + r.Item("tcount").ToString + "" & font_text_end & "</td></tr>")
                            End If



                            If Not IsDBNull(r.Item("tcount")) Then
                                If IsNumeric(r.Item("tcount")) Then
                                    If CInt(r.Item("tmonth").ToString) = Now.Month Then
                                        If CDbl(r.Item("tcount")) = 0 Then
                                            ' if its this month, and its 0, do nothing 
                                        Else
                                            If Trim(temp_string) <> "" Then
                                                temp_string &= ","
                                            End If
                                            If Trim(from_spot) = "pdf4" Then
                                                temp_string &= "['" & r.Item("tmonth").ToString + "-" & Right(r.Item("tyear").ToString, 2) & "', " & Replace(r.Item("tcount").ToString, ",", "") & "]"
                                            Else
                                                If Trim(type_of) = "Month" Or Trim(type_of) = "BWeight" Or Trim(type_of) = "TWeight" Or Trim(type_of) = "HWeight" Or Trim(type_of) = "Type" Or Trim(type_of) = "Hours" Then
                                                    temp_string &= "['" & r.Item("tmonth").ToString + "-" & Right(r.Item("tyear").ToString, 2) & "', " & Replace(r.Item("tcount").ToString, ",", "") & "]"
                                                Else
                                                    temp_string &= "['" & r.Item("tmonth").ToString + "-" & Right(r.Item("tyear").ToString, 2) & "', XXXXX, " & Replace(r.Item("tcount").ToString, ",", "") & ", null,  null, null, null]"
                                                End If
                                            End If
                                        End If
                                    Else
                                        If CDbl(r.Item("tcount")) = 0 Then
                                            If Trim(temp_string) <> "" Then
                                                temp_string &= ", "
                                            End If
                                            If Trim(from_spot) = "pdf4" Then
                                                temp_string &= "['" & r.Item("tmonth").ToString + "-" & Right(r.Item("tyear").ToString, 2) & "',0]"
                                            Else
                                                temp_string &= "['" & r.Item("tmonth").ToString + "-" & Right(r.Item("tyear").ToString, 2) & "',XXXXX,0, null,  null, null, null]"
                                            End If

                                        Else
                                            If Trim(temp_string) <> "" Then
                                                temp_string &= ", "
                                            End If
                                            If Trim(from_spot) = "pdf4" Then
                                                temp_string &= "['" & r.Item("tmonth").ToString + "-" & Right(r.Item("tyear").ToString, 2) & "', " & Replace(r.Item("tcount").ToString, ",", "") & "]"
                                            Else
                                                'added MSW - 5/10/19
                                                If Trim(type_of) = "Month" Or Trim(type_of) = "BWeight" Or Trim(type_of) = "TWeight" Or Trim(type_of) = "HWeight" Or Trim(type_of) = "Type" Or Trim(type_of) = "Hours" Then
                                                    temp_string &= "['" & r.Item("tmonth").ToString + "-" & Right(r.Item("tyear").ToString, 2) & "', " & Replace(r.Item("tcount").ToString, ",", "") & "]"
                                                Else
                                                    temp_string &= "['" & r.Item("tmonth").ToString + "-" & Right(r.Item("tyear").ToString, 2) & "',XXXXX, " & Replace(r.Item("tcount").ToString, ",", "") & ", null,  null, null, null]"
                                                End If
                                            End If
                                        End If
                                    End If

                                End If

                            End If



                        ElseIf Trim(type_of) = "Type" Then

                            If Not IsDBNull(r.Item("type_name")) Then
                                type_temp = r.Item("type_name").ToString
                                tcompare1 = type_temp
                                If InStr(type_temp, "Helicopter") > 0 Then
                                    type_temp = Replace(type_temp, "Turboprop", "Turbine")
                                End If

                                If Not IsDBNull(r.Item("tflights")) Then
                                    If IsNumeric(r.Item("tflights")) Then

                                        If CDbl(r.Item("tflights")) = 0 Then
                                            If Trim(temp_string) <> "" Then
                                                temp_string &= ", "
                                            End If
                                            temp_string &= "['" & type_temp & "',XXXXX, 0]"
                                        Else
                                            If Trim(temp_string) <> "" Then
                                                temp_string &= ", "
                                            End If
                                            temp_string &= "['" & type_temp & "', " & Replace(r.Item("tflights").ToString, ",", "") & "]"
                                        End If

                                    End If
                                End If
                            End If
                        ElseIf Trim(type_of) = "BWeight" Or Trim(type_of) = "TWeight" Or Trim(type_of) = "HWeight" Then
                            If Not IsDBNull(r.Item("tflights")) Then
                                If IsNumeric(r.Item("tflights")) Then
                                    tcompare1 = r.Item("type_name").ToString
                                    If CDbl(r.Item("tflights")) = 0 Then
                                        If Trim(temp_string) <> "" Then
                                            temp_string &= ", "
                                        End If
                                        temp_string &= "['" & r.Item("type_name").ToString & "',XXXXX,0]"
                                    Else
                                        If Trim(temp_string) <> "" Then
                                            temp_string &= ", "
                                        End If
                                        temp_string &= "['" & r.Item("type_name").ToString & "'," & Replace(r.Item("tflights").ToString, ",", "") & "]"
                                    End If

                                End If
                            End If

                        End If


                        If Not IsNothing(results_table2) Then
                            If results_table2.Rows.Count > 0 Then
                                For Each k As DataRow In results_table2.Rows

                                    ' assign what we will be comparing
                                    If Trim(type_of) = "Month" Then
                                        tcompare2 = k.Item("tmonth").ToString
                                    ElseIf Trim(type_of) = "BWeight" Or Trim(type_of) = "TWeight" Or Trim(type_of) = "HWeight" Then
                                        tcompare2 = k.Item("type_name").ToString
                                    ElseIf Trim(type_of) = "Type" Then
                                        tcompare2 = k.Item("type_name").ToString
                                    End If

                                    ' find one for the same month or type
                                    If Trim(tcompare1) = Trim(tcompare2) Then

                                        If Trim(type_of) = "Month" Then
                                            temp_string = Replace(temp_string, "XXXXX", Replace(k.Item("tcount").ToString, ",", ""))
                                        ElseIf Trim(type_of) = "BWeight" Or Trim(type_of) = "TWeight" Or Trim(type_of) = "HWeight" Then
                                            temp_string = Replace(temp_string, "XXXXX", Replace(k.Item("tflights").ToString, ",", ""))
                                        ElseIf Trim(type_of) = "Type" Then
                                            temp_string = Replace(temp_string, "XXXXX", Replace(k.Item("tflights").ToString, ",", ""))
                                        End If

                                    End If
                                Next
                            End If
                        End If

                        ' if for some reason we didnt have it, then replace it with null
                        If InStr(Trim(temp_string), "XXXXX") > 0 Then
                            temp_string = Replace(temp_string, "XXXXX", "null")
                        End If

                    Next


                    If Trim(from_spot) = "pdf" Or Trim(from_spot) = "pdf2" Or Trim(from_spot) = "pdf3" Then
                        commonEvo.set_ranges_for_vsCharts(low_number, high_number, interval_point, starting_point, ending_point)
                        If Trim(from_spot) = "pdf" Then
                            graph1.ChartAreas("ChartArea1").AxisY.Maximum = ending_point
                            graph1.ChartAreas("ChartArea1").AxisY.Minimum = IIf(starting_point > 0, starting_point, 0)
                            graph1.ChartAreas("ChartArea1").AxisY.Interval = interval_point
                        End If
                        htmlOut.Append("</table></td></tr></table></td></tr>")
                    Else
                        htmlOut.Append("</table></div></td></tr></table></td></tr>")
                    End If


                Else
                    htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""seperator"" style=""padding-left:3px;"">No FAA Flight Activity Found.</td></tr>")
                End If
            Else
                htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""seperator"" style=""padding-left:3px;"">No FAA Flight Activity Found.</td></tr>")
            End If

            htmlOut.Append("</table>")


            If Trim(from_spot) = "pdf4" Then
                ticks_string = "Y"
                commonEvo.set_ranges_for_vsCharts(low_number, high_number, interval_point, starting_point, ending_point, ticks_string)

                temp_ticks_string = ticks_string
            End If

        Catch ex As Exception

            aError = "Error in views_display_fractions_to_expire(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        If Trim(from_spot) = "pdf" Or Trim(from_spot) = "pdf2" Or Trim(from_spot) = "pdf3" Then


            If Trim(from_spot) = "pdf" Then
                htmlOut.Length = 0   ' clear it, then go add in the pictre 
                graph1.Titles.Clear()
                graph1.ImageType = DataVisualization.Charting.ChartImageType.Jpeg
                graph1.SaveImage(HttpContext.Current.Server.MapPath(HttpContext.Current.Session.Item("MarketSummaryFolderVirtualPath").ToString) + "\" & HttpContext.Current.Session.Item("localUser").crmUserTemporaryFilePrefix & searchCriteria.ViewCriteriaCompanyID.ToString + "_OP_GRAPH1.jpg", DataVisualization.Charting.ChartImageFormat.Jpeg)
            End If
            If Trim(from_spot) = "pdf" Then
                htmlOut.Append("<table width='80%' align='center'>")
                htmlOut.Append("<tr class='" & HttpContext.Current.Session.Item("ROW_CLASS_BOTTOM") & "'><td colspan='2' align='center'><font class='" & HttpContext.Current.Session.Item("FONT_CLASS_HEADER") & "'>Aircraft Utilization Summary</font></td></tr>")
                htmlOut.Append("<tr><td align='center'><img src='" & HttpContext.Current.Session.Item("localUser").crmUserTemporaryFilePrefix & searchCriteria.ViewCriteriaCompanyID.ToString + "_OP_GRAPH1.jpg'><br></td></tr>")
                htmlOut.Append("</table>")
            End If
            out_htmlString = htmlOut.ToString
        Else
            out_htmlString = start_temp_string & temp_string
            table_string = htmlOut.ToString
        End If
        'return resulting html string




        htmlOut = Nothing
        results_table = Nothing

    End Sub

    Public Function get_flight_activity_overall_top_function(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString_count As Long, ByVal product_code_selection As String) As DataTable

        Dim results_table As New DataTable
        Dim toggleRowColor As Boolean = False

        Try

            results_table = get_flight_activity_overall(searchCriteria, product_code_selection)

            If Not IsNothing(results_table) Then
                If results_table.Rows.Count > 0 Then
                    For Each r As DataRow In results_table.Rows
                        out_htmlString_count = CDbl(r.Item("tflights"))
                    Next
                End If
            End If


        Catch ex As Exception

            aError = "Error in get_flight_activity_overall_top_function(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        Return results_table
        'return resulting html string  
        results_table = Nothing

    End Function
    Public Sub get_company_profile_top_function(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal from_spot As String, ByRef count_current_ownser As Long, ByRef count_past_owner As Long, ByRef count_operator As Long, ByRef count_manu As Long, ByRef count_dealer As Long, ByRef count_locations As Long, ByRef count_lease As Long, ByRef count_finance As Long, ByVal rollup As String)

        Dim results_table As New DataTable
        Dim toggleRowColor As Boolean = False

        Try

            results_table = get_company_profile(searchCriteria, from_spot, rollup)

            If Not IsNothing(results_table) Then
                If results_table.Rows.Count > 0 Then
                    For Each r As DataRow In results_table.Rows

                        count_current_ownser = CLng(r.Item("currentowner"))
                        count_past_owner = CLng(r.Item("pastowner"))
                        count_operator = CLng(r.Item("operator"))
                        count_manu = CLng(r.Item("manufacturer"))
                        count_dealer = CLng(r.Item("dealer"))
                        count_locations = CLng(r.Item("locations"))
                        count_lease = CLng(r.Item("leasing"))
                        count_finance = CLng(r.Item("financial"))

                    Next
                End If
            End If


        Catch ex As Exception

            aError = "Error in get_company_profile_top_function(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        'return resulting html string  
        results_table = Nothing

    End Sub
    Public Sub get_flight_activity_last_function(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef last_date As String)

        Dim results_table As New DataTable
        Dim toggleRowColor As Boolean = False

        Try

            results_table = get_flight_activity_last(searchCriteria)

            If Not IsNothing(results_table) Then
                If results_table.Rows.Count > 0 Then
                    For Each r As DataRow In results_table.Rows
                        last_date = Trim(r.Item("ffd_date"))
                    Next
                End If
            End If


        Catch ex As Exception

            aError = "Error in get_flight_activity_last_function(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        'return resulting html string  
        results_table = Nothing

    End Sub

    Public Function get_arriavals_departures(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal product_code_selection As String, ByVal just_airports As Boolean, ByVal AirportTab As Boolean, ByRef FlightTotals As Long, Optional ByVal LimitThisQuery As Long = 0, Optional ByVal selected_product_codes As String = "", Optional ByVal arrivals_departures As String = "D") As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()
        Dim sQuery_temp = New StringBuilder()
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim temp_string As String = ""


        Try

            'passed in from 
            airport_direction = arrivals_departures

            '-- ***************  UPPER RIGHT TAB 2 - TOP ORIGINS/DESTINATIONS ************************
            '-- # FLIGHT ACTIVITY -MOST COMMON ORIGINS - WE WOULD ALSO DO SAME FOR DESTINATIONS
            sQuery.Append(" select DISTINCT  ")

            If LimitThisQuery > 0 Then
                sQuery.Append(" top " + LimitThisQuery.ToString + " ")
            Else
                ' If FlightTotals > 50000 Then
                'sQuery.Append(" top 500 ")
                '  End If
            End If

            'If ((searchCriteria.ViewCriteriaCompanyID > 0 Or Operator_IDS_String <> "") And Airport_ID_OVERALL <= 1 And Trim(Airport_IDS_String) = "" And just_airports = False) Or (Airport_IDS_String <> "" And just_airports = False) Or (Airport_ID_OVERALL > 1) Or (LimitThisQuery = 10) Then
            '  sQuery.Append("  ffd_origin_aport_id, origin_aport_iata_code, origin_aport_icao_code, origin_aport_name, origin_aport_country, origin_aport_city, origin_aport_state, ")
            '  sQuery.Append("  ffd_dest_aport_id, dest_aport_iata_code, dest_aport_icao_code, dest_aport_name, dest_aport_country, dest_aport_city, dest_aport_state  ")
            'Else
            If Trim(airport_direction) = "X" Then
                sQuery.Append("  ffd_origin_aport_id, origin_aport_iata_code, origin_aport_icao_code, origin_aport_name, origin_aport_country, origin_aport_city, origin_aport_state, ")
                sQuery.Append("  ffd_dest_aport_id, dest_aport_iata_code, dest_aport_icao_code, dest_aport_name, dest_aport_country, dest_aport_city, dest_aport_state  ")
            ElseIf Trim(airport_direction) = "D" Then
                sQuery.Append("   ffd_dest_aport_id as aport_id, dest_aport_iata_code as aport_iata, dest_aport_icao_code as aport_icao, dest_aport_name as aport_name, dest_aport_country as aport_country, dest_aport_city as aport_city, dest_aport_state as aport_state, ffd_origin_date as flight_date  ")
            ElseIf Trim(airport_direction) = "A" Then
                sQuery.Append("   ffd_origin_aport_id as aport_id, origin_aport_iata_code as aport_iata, origin_aport_icao_code as aport_icao, origin_aport_name as aport_name, origin_aport_country as aport_country, origin_aport_city as aport_city, origin_aport_state as aport_state, ffd_dest_date as flight_date  ")
            End If
            '  End If 

            sQuery.Append(", amod_make_name, amod_model_name, ac_ser_no_full, ac_reg_no ")

            sQuery.Append(" FROM view_flights WITH(NOLOCK) ")



            If String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentsStartDate) And String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentsEndDate) Then
                sQuery.Append(" WHERE convert(date, ffd_date, 0) >= (getdate()-182)  ")
            Else
                sQuery.Append(" where (convert(date, ffd_date, 0) >= '" & Month(searchCriteria.ViewCriteriaDocumentsStartDate) & "/" & Day(searchCriteria.ViewCriteriaDocumentsStartDate) & "/" & Year(searchCriteria.ViewCriteriaDocumentsStartDate) & "') ")
                sQuery.Append(" AND (convert(date, ffd_date, 0) <= '" & Month(searchCriteria.ViewCriteriaDocumentsEndDate) & "/" & Day(searchCriteria.ViewCriteriaDocumentsEndDate) & "/" & Year(searchCriteria.ViewCriteriaDocumentsEndDate) & "') ")
            End If


            sQuery.Append(" and ffd_dest_aport_id  > 0  ")  'and ffd_hide_flag= 'N' 

            If Trim(searchCriteria.ViewCriteriaContinent) = "International" Then
                sQuery.Append(" and (origin_aport_country not in ('United States', 'U.S.') or dest_aport_country not in ('United States', 'U.S.'))  ")
            End If



            ' if there is an operator and there is no airport or airport list selected


            If Trim(distance_string) <> "" And Len(Trim(distance_string)) > 2 Then
                sQuery.Append(distance_string)
            End If
            ' Call build_flight_data_from(sQuery)
            ' Call build_flight_data_where(sQuery, aport_id)

            If Trim(product_code_selection) <> "" Then
                ' sQuery.Append(Replace(product_code_selection, "amod", "Aircraft_Model.amod"))
                sQuery.Append(product_code_selection)
            Else
                If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
                    sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))
                Else
                    sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True))
                End If
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


            Dim temp_use As String = " "
            If searchCriteria.ViewCriteriaHasBusinessFlag = True Or searchCriteria.ViewCriteriaHasCommercialFlag = True Or searchCriteria.ViewCriteriaHasHelicopterFlag = True Then
                sQuery.Append(" and ( ")
            End If

            If searchCriteria.ViewCriteriaHasBusinessFlag = True Then
                If Trim(temp_use) = "" Then
                    temp_use = " or "
                Else
                    sQuery.Append(temp_use)
                End If
                sQuery.Append(" ac_product_business_flag = 'Y' ")
            End If

            If searchCriteria.ViewCriteriaHasCommercialFlag = True Then
                If Trim(temp_use) = "" Then
                    temp_use = " or "
                Else
                    sQuery.Append(temp_use)
                End If
                sQuery.Append(" ac_product_commercial_flag = 'Y' ")
            End If

            If searchCriteria.ViewCriteriaHasHelicopterFlag = True Then
                If Trim(temp_use) = "" Then
                    temp_use = " or "
                Else
                    sQuery.Append(temp_use)
                End If
                sQuery.Append(" ac_product_helicopter_flag = 'Y' ")
            End If

            If searchCriteria.ViewCriteriaHasBusinessFlag = True Or searchCriteria.ViewCriteriaHasCommercialFlag = True Or searchCriteria.ViewCriteriaHasHelicopterFlag = True Then
                sQuery.Append(" ) ")
            End If


            ' sQuery.Append(" AND (ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportIATA & "' or ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportICAO & "') ")

            If Not String.IsNullOrEmpty(Trim(Aircraft_IDS_String)) Then
                If exclude_Aircraft = True Then
                    sQuery.Append(" and ac_id not ")
                Else
                    sQuery.Append(" and ac_id ")
                End If
                sQuery.Append(" in (" & Aircraft_IDS_String & ") ")
            End If





            If Trim(airport_direction) = "D" Then
                If Airport_ID_OVERALL > 1 Then
                    sQuery.Append(" AND (ffd_origin_aport_id = '" & Airport_ID_OVERALL & "') ")
                ElseIf Not String.IsNullOrEmpty(Airport_IDS_String) Then
                    'Airport strings mean that a folder has been passed which means we need to display the airports in the folder
                    If AirportTab Then
                        If exclude_airport_check Then
                            sQuery.Append(" AND (ffd_origin_aport_id not in (" & Airport_IDS_String & ")) ")
                        Else
                            sQuery.Append(" AND (ffd_origin_aport_id in (" & Airport_IDS_String & ")) ")
                        End If
                    Else
                        'need routes
                        If exclude_airport_check Then
                            sQuery.Append(" AND (ffd_origin_aport_id not in (" & Airport_IDS_String & ")) ")
                        Else
                            sQuery.Append(" AND (ffd_origin_aport_id in (" & Airport_IDS_String & ")) ")
                        End If
                    End If
                End If
            ElseIf Trim(airport_direction) = "A" Then
                If Airport_ID_OVERALL > 1 Then
                    sQuery.Append(" AND (ffd_dest_aport_id = '" & Airport_ID_OVERALL & "') ")
                ElseIf Not String.IsNullOrEmpty(Airport_IDS_String) Then
                    'Airport strings mean that a folder has been passed which means we need to display the airports in the folder
                    If AirportTab Then
                        If exclude_airport_check Then
                            sQuery.Append(" AND (ffd_dest_aport_id not in (" & Airport_IDS_String & ")) ")
                        Else
                            sQuery.Append(" AND (ffd_dest_aport_id in (" & Airport_IDS_String & ")) ")
                        End If
                    Else
                        'need routes
                        If exclude_airport_check Then
                            sQuery.Append(" AND (ffd_dest_aport_id not in (" & Airport_IDS_String & ")) ")
                        Else
                            sQuery.Append(" AND (ffd_dest_aport_id in (" & Airport_IDS_String & ")) ")
                        End If
                    End If
                End If
            ElseIf Trim(airport_direction) = "X" Then
                '  sQuery.Append(" and ffd_dest_aport_id > 0")
                ' sQuery.Append(" AND (ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportIATA & "' or ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportICAO & "')")

                If Airport_ID_OVERALL > 1 Then
                    sQuery.Append(" AND ((ffd_origin_aport_id = '" & Airport_ID_OVERALL & "') or (ffd_dest_aport_id = '" & Airport_ID_OVERALL & "')) ")
                ElseIf Not String.IsNullOrEmpty(Airport_IDS_String) Then
                    If exclude_airport_check Then
                        sQuery.Append(" AND (ffd_origin_aport_id not in (" & Airport_IDS_String & ")) AND (ffd_dest_aport_id not in (" & Airport_IDS_String & "))   ")
                    Else
                        sQuery.Append(" AND (   (ffd_origin_aport_id in (" & Airport_IDS_String & "))  or (ffd_dest_aport_id in (" & Airport_IDS_String & "))   )")
                    End If

                End If

            End If






            If Trim(Operator_IDS_String) <> "" Then
                If exclude_check = True Then
                    sQuery.Append(" and comp_id not ")
                Else
                    sQuery.Append(" and comp_id ")
                End If
                sQuery.Append(" in (" & Operator_IDS_String & ") ")
            End If


            If searchCriteria.ViewCriteriaCompanyID > 0 And searchCriteria.ViewCriteriaAmodID > 0 Then
                sQuery.Append(" AND amod_id = " & searchCriteria.ViewCriteriaAmodID & "")
            ElseIf Not IsNothing(searchCriteria.ViewCriteriaAmodIDArray) Then
                If searchCriteria.ViewCriteriaCompanyID > 0 Then
                    sQuery.Append(SetUpModelString(searchCriteria))
                Else
                    sQuery.Append(SetUpModelString(searchCriteria))
                End If
            ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake) Then
                If searchCriteria.ViewCriteriaCompanyID > 0 Then
                    sQuery.Append(SetUpMakeString(searchCriteria))
                Else
                    sQuery.Append(SetUpMakeString(searchCriteria))
                End If
            End If

            If Not IsNothing(searchCriteria.ViewCriteriaTypeIDArray) Then
                If searchCriteria.ViewCriteriaCompanyID > 0 Then
                    sQuery.Append(SetUpTypeString(searchCriteria))
                Else
                    sQuery.Append(SetUpTypeString(searchCriteria))
                End If
            End If


            If Trim(rollup_text) <> "" Then
                sQuery.Append(rollup_text)
            ElseIf searchCriteria.ViewCriteriaCompanyID > 0 Then
                sQuery.Append(" AND comp_id = " & searchCriteria.ViewCriteriaCompanyID & "")
            End If



            'If searchCriteria.ViewCriteriaAmodID > -1 Then ' changed to 0 - MSW 
            If searchCriteria.ViewCriteriaAmodID > 0 Then
                sQuery.Append(crmWebClient.Constants.cAndClause + " amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
            ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
                sQuery.Append(SetUpMakeString(searchCriteria))
                'sQuery.Append(crmWebClient.Constants.cAndClause + "Aircraft_Model.amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
            End If

            If Trim(airport_direction) = "D" Then
                sQuery.Append(" order by ffd_origin_date desc ")
            Else
                sQuery.Append(" order by ffd_dest_date desc ")
            End If





            'sQuery.Append(" group by ")
            'If ((searchCriteria.ViewCriteriaCompanyID > 0 Or Operator_IDS_String <> "") And Airport_ID_OVERALL <= 1 And Trim(Airport_IDS_String) = "" And just_airports = False) Or (Airport_IDS_String <> "" And just_airports = False) Or (Airport_ID_OVERALL > 1) Or (LimitThisQuery = 10) Then
            '  sQuery.Append("  origin_aport_iata_code, origin_aport_icao_code, origin_aport_name, origin_aport_country, ffd_origin_aport_id, origin_aport_city, origin_aport_state, ")
            '  sQuery.Append(" dest_aport_iata_code, dest_aport_icao_code, dest_aport_name, dest_aport_country, ffd_dest_aport_id,  dest_aport_city, dest_aport_state  ")
            'Else
            '  If Trim(airport_direction) = "D" Then
            '    sQuery.Append(" dest_aport_iata_code, dest_aport_icao_code, dest_aport_name, dest_aport_country, ffd_dest_aport_id,  dest_aport_city, dest_aport_state  ")
            '  ElseIf Trim(airport_direction) = "O" Then
            '    sQuery.Append(" origin_aport_iata_code, origin_aport_icao_code, origin_aport_name, origin_aport_country, ffd_origin_aport_id, origin_aport_city, origin_aport_state ")
            '  Else
            '    sQuery.Append(" dest_aport_iata_code, dest_aport_icao_code, dest_aport_name, dest_aport_country, ffd_dest_aport_id,  dest_aport_city, dest_aport_state  ")
            '  End If
            'End If

            'If Trim(airport_direction) = "D" Then
            '  sQuery.Append(" , ffd_dest_aport_id  ")
            'ElseIf Trim(airport_direction) = "O" Then
            '  sQuery.Append(" , ffd_origin_aport_id  ")
            'Else
            '  sQuery.Append(" , ffd_dest_aport_id  ")
            'End If

            'HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />" & Date.Now & " - get_most_common_origins(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString
            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "utilization_functions.vb", sQuery.ToString.ToString)

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
                aError = "Error in get_fractional_shares load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_fractional_shares(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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
    Public Sub MostCommonOriginsJSArray(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByVal product_code_selection As String, ByRef table_count As Long, ByVal initial As Boolean, ByRef AirportTab As Boolean, ByRef FlightTotals As Long, ByVal from_spot As String, ByVal table_color As String, ByVal temp_pdf_header As String, Optional ByRef LimitThisQuery As Long = 0, Optional ByRef SubHeaderString As String = "", Optional ByVal sSelectedProductCode As String = "", Optional ByRef Selection_Count As Long = 0, Optional ByVal show_fuel_burn_in_liters As Boolean = False, Optional ByRef only_one As Boolean = False)

        Dim ResultsTable As New DataTable
        Dim toggleRowColor As Boolean = False
        Dim Results As String = ""
        Dim htmlOut As New StringBuilder
        Dim tcount As Long = 0
        Dim temp_gal_lit As Integer = 0
        Dim temp_String As String = ""

        Try

            If InStr(Trim(from_spot), "pdf") > 0 And Trim(from_spot) <> "valpdf" Then
                LimitThisQuery = 41
            End If

            If initial = False Then
                ResultsTable = get_most_common_origins(searchCriteria, product_code_selection, False, AirportTab, FlightTotals, LimitThisQuery, sSelectedProductCode, SubHeaderString)
            Else
                ResultsTable = get_most_common_origins(searchCriteria, product_code_selection, True, AirportTab, FlightTotals, LimitThisQuery, sSelectedProductCode, SubHeaderString)
            End If




            If Not IsNothing(ResultsTable) Then
                If ResultsTable.Rows.Count = 1 Then
                    only_one = True
                End If

                If ResultsTable.Rows.Count > 10000 Then  ' if greater than 50,000 then limit to 500
                    Selection_Count = ResultsTable.Rows.Count
                    ResultsTable.Clear()
                    LimitThisQuery = 500
                    If initial = False Then
                        ResultsTable = get_most_common_origins(searchCriteria, product_code_selection, False, AirportTab, FlightTotals, LimitThisQuery, sSelectedProductCode, SubHeaderString)
                    Else
                        ResultsTable = get_most_common_origins(searchCriteria, product_code_selection, True, AirportTab, FlightTotals, LimitThisQuery, sSelectedProductCode, SubHeaderString)
                    End If
                End If
            End If

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "Utilization_functions.vb", "start fill array")
            If Not IsNothing(ResultsTable) Then

                If ResultsTable.Rows.Count > 0 Then
                    table_count = ResultsTable.Rows.Count

                    If InStr(Trim(from_spot), "pdf") > 0 Then 'Or Trim(from_spot) = "valpdf" Then
                        htmlOut.Append("<tr><td valign='top' align='center'><font class='" & HttpContext.Current.Session("FONT_CLASS_HEADER") & " mainHeading'>" & SubHeaderString & "</font></td></tr>")

                        htmlOut.Append("<tr><td valign='top'><div class=""Box""><table id=""weightTable"" cellpadding='3'  cellspacing='0' width='100%' class='formatTable " & table_color & " " & IIf(from_spot = "valpdf", "large", "") & "'><thead>")
                        htmlOut.Append("<tr class=""noBorder"">")

                        If Trim(airport_direction) = "X" Then
                            htmlOut.Append("<th>ORIGIN</th>")
                            htmlOut.Append("<th>DESTINATION</th>")
                        ElseIf Trim(airport_direction) = "D" Then
                            htmlOut.Append("<th>ORIGIN</th>")

                            ' will need to be changed 
                            If from_spot = "valpdf" Then
                                htmlOut.Append("<th>DESTINATION</th>")
                            End If
                        ElseIf Trim(airport_direction) = "O" Then
                            htmlOut.Append("<th>DESTINATION</th>")

                            ' will need to be changed 
                            If from_spot = "valpdf" Then
                                htmlOut.Append("<th>ORIGIN</th>")
                            End If
                        Else
                            htmlOut.Append("<th>ORIGIN</th>")

                            ' will need to be changed 
                            If from_spot = "valpdf" Then
                                htmlOut.Append("<th>DESTINATION</th>")
                            End If
                        End If


                        htmlOut.Append("<th class='right'>NBR" & IIf(from_spot = "pdf", "<br/>", " ") & "FLTS</th>")


                        If from_spot = "pdf" Then
                            htmlOut.Append("<th class='right'>TOTAL FLT<br />HRS</th>")
                            If show_fuel_burn_in_liters = True Then
                                htmlOut.Append("<th class='right'><font size='-1'>EST FUEL<br/>BURN (L)</font></th>")
                            Else
                                htmlOut.Append("<th class='right'><font size='-1'>EST FUEL<br/>BURN (GAL)</font></th>")
                            End If
                        Else
                            htmlOut.Append("<th class='right'>FLIGHT HOURS</th>")
                        End If

                        htmlOut.Append("</tr>")
                        htmlOut.Append("</thead>")
                        htmlOut.Append("<tbody>")
                    End If

                    For Each r As DataRow In ResultsTable.Rows

                        If InStr(Trim(from_spot), "pdf") > 0 Then


                            If tcount > 39 Or (Trim(airport_direction) = "X" And tcount > 24) Then
                                'tcount = 0
                                'htmlOut.Append("</table></td></tr></table>")
                                'htmlOut.Append(comp_functions.NEW_Insert_Page_Break_PDF(0, "pdf"))
                                'htmlOut.Append(temp_pdf_header)
                                ''  If bWordReport = True Then
                                ''  htmlOut.Append("<table width='" & word_width & "' align='center' cellpadding='3'>")
                                ''Else
                                'htmlOut.Append("<table width='95%' align='center' cellpadding='3'>")
                                ''  End If

                                'htmlOut.Append("<tr><td valign='top' align='center'><font class='" & HttpContext.Current.Session("FONT_CLASS_HEADER") & " mainHeading'><strong>" & searchCriteria.ViewCriteriaAirportName & "</strong> Routes</font></td></tr>")

                                'htmlOut.Append("<tr><td valign='top'><div class=""Box""><table id=""weightTable"" cellpadding='3'  cellspacing='0' width='100%' class='formatTable " & table_color & "'><thead>")
                                'htmlOut.Append("<tr class=""noBorder"">")
                                'htmlOut.Append("<th>ORIGIN</th>")
                                'If Airport_ID_OVERALL = 0 Then
                                '  htmlOut.Append("<th>DESTINATION</th>")
                                'End If
                                'htmlOut.Append("<th>NBR<br/>FLIGHTS</th>")
                                'htmlOut.Append("<th>TOTAL<br/>FLIGHT HOURS</th>")
                                'htmlOut.Append("<th>EST FUEL<br/>BURN</th>")
                                'htmlOut.Append("</tr>")
                                'htmlOut.Append("</thead>")
                                'htmlOut.Append("<tbody>")
                            Else

                                htmlOut.Append("<tr>")

                                If (searchCriteria.ViewCriteriaCompanyID > 0 And Airport_ID_OVERALL = 1 And Trim(Airport_IDS_String) = "" And initial = False) Or (Airport_IDS_String <> "" And initial = False) Or (Airport_ID_OVERALL > 1 And initial = False) Or (from_spot = "valpdf") Then
                                    htmlOut.Append("<td><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>")

                                    If Not from_spot = "valpdf" Then

                                        If Trim(airport_direction) = "X" Then

                                            htmlOut.Append("" & Replace(Replace(clsGeneral.clsGeneral.StripChars(Replace(r.Item("origin_aport_name").ToString, " Airport", " "), False), "é", "e"), "è", "e"))
                                            htmlOut.Append(" (" & Replace(Replace(r.Item("origin_aport_country").ToString, "United States", "U.S."), "United Kingdom", "UK") & " - " & clsGeneral.clsGeneral.StripChars(r.Item("origin_aport_city").ToString, False) & " " & clsGeneral.clsGeneral.StripChars(r.Item("origin_aport_state").ToString, False) & ") ")
                                            htmlOut.Append("<font size='-1'>" & r.Item("origin_aport_iata_code").ToString & "</a> / ")
                                            htmlOut.Append("" & r.Item("origin_aport_icao_code").ToString & "")
                                            htmlOut.Append("</font></font></td>")

                                            htmlOut.Append("<td><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>")
                                            htmlOut.Append("" & Replace(Replace(clsGeneral.clsGeneral.StripChars(r.Item("dest_aport_name").ToString, False), "é", "e"), "è", "e"))
                                            htmlOut.Append("(" & Replace(Replace(r.Item("dest_aport_country").ToString, "United States", "U.S."), "United Kingdom", "UK") & " - " & clsGeneral.clsGeneral.StripChars(r.Item("dest_aport_city").ToString, False) & " " & clsGeneral.clsGeneral.StripChars(r.Item("dest_aport_state").ToString, False) & ") ")
                                            htmlOut.Append("<font size='-1'>" & r.Item("dest_aport_iata_code").ToString & "</a> / ")
                                            htmlOut.Append(r.Item("dest_aport_icao_code").ToString)
                                            htmlOut.Append("</font>")
                                            ' localCriteria.ViewCriteriaAirportName = Replace(localCriteria.ViewCriteriaAirportName, " International", " Intl.")
                                            ' localCriteria.ViewCriteriaAirportName = Replace(localCriteria.ViewCriteriaAirportName, "é", "e")
                                            '   localCriteria.ViewCriteriaAirportName = Replace(localCriteria.ViewCriteriaAirportName, "è", "e")

                                        ElseIf Trim(airport_direction) = "O" Then
                                            htmlOut.Append("" & Replace(Replace(clsGeneral.clsGeneral.StripChars(r.Item("dest_aport_name").ToString, False), "é", "e"), "è", "e"))
                                            htmlOut.Append("(" & Replace(Replace(r.Item("dest_aport_country").ToString, "United States", "U.S."), "United Kingdom", "UK") & " - " & clsGeneral.clsGeneral.StripChars(r.Item("dest_aport_city").ToString, False) & " " & clsGeneral.clsGeneral.StripChars(r.Item("dest_aport_state").ToString, False) & ") ")
                                            htmlOut.Append(r.Item("dest_aport_iata_code").ToString & "</a> / ")
                                            htmlOut.Append(r.Item("dest_aport_icao_code").ToString)
                                        Else
                                            htmlOut.Append("" & Replace(Replace(clsGeneral.clsGeneral.StripChars(Replace(r.Item("origin_aport_name").ToString, " Airport", " "), False), "é", "e"), "è", "e"))
                                            htmlOut.Append(" (" & Replace(Replace(r.Item("origin_aport_country").ToString, "United States", "U.S."), "United Kingdom", "UK") & " - " & clsGeneral.clsGeneral.StripChars(r.Item("origin_aport_city").ToString, False) & " " & clsGeneral.clsGeneral.StripChars(r.Item("origin_aport_state").ToString, False) & ") ")
                                            htmlOut.Append("" & r.Item("origin_aport_iata_code").ToString & "</a> / ")
                                            htmlOut.Append("" & r.Item("origin_aport_icao_code").ToString & "")
                                        End If



                                    Else
                                        htmlOut.Append("" & Replace(clsGeneral.clsGeneral.StripChars(r.Item("origin_aport_name").ToString, False), "Airport", ""))
                                    End If


                                    htmlOut.Append("</font></td>")

                                    If Airport_ID_OVERALL = 0 Then
                                        htmlOut.Append("<td><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>")

                                        If Not from_spot = "valpdf" Then
                                            htmlOut.Append("" & clsGeneral.clsGeneral.StripChars(r.Item("dest_aport_name").ToString, False))
                                            htmlOut.Append("<br />(" & Replace(r.Item("dest_aport_country").ToString, "United States", "U.S.") & " - " & clsGeneral.clsGeneral.StripChars(r.Item("dest_aport_city").ToString, False) & " " & clsGeneral.clsGeneral.StripChars(r.Item("dest_aport_state").ToString, False) & ") ")
                                            htmlOut.Append(r.Item("dest_aport_iata_code").ToString & "</a> / ")
                                            htmlOut.Append(r.Item("dest_aport_icao_code").ToString)
                                        Else
                                            htmlOut.Append("" & Replace(clsGeneral.clsGeneral.StripChars(Replace(Replace(r.Item("dest_aport_name").ToString, "Airport", ""), "International", ""), False), "Airport", ""))
                                        End If


                                        htmlOut.Append("</font></td>")
                                    End If
                                Else
                                    htmlOut.Append("<td><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>")

                                    If Not from_spot = "valpdf" Then
                                        htmlOut.Append("" & clsGeneral.clsGeneral.StripChars(Replace(r.Item("origin_aport_name").ToString, " Airport", " "), False))
                                        htmlOut.Append(" (" & Replace(r.Item("origin_aport_country").ToString, "United States", "U.S.") & " - " & clsGeneral.clsGeneral.StripChars(r.Item("origin_aport_city").ToString, False) & " " & clsGeneral.clsGeneral.StripChars(r.Item("origin_aport_state").ToString, False) & ") ")
                                        htmlOut.Append("" & r.Item("origin_aport_iata_code").ToString & "</a> / ")
                                        htmlOut.Append("" & r.Item("origin_aport_icao_code").ToString & "")
                                    Else
                                        htmlOut.Append("" & Replace(clsGeneral.clsGeneral.StripChars(Replace(Replace(r.Item("origin_aport_name").ToString, "Airport", ""), "International", ""), False), "Airport", ""))
                                    End If


                                    htmlOut.Append("</font></td>")
                                End If

                                htmlOut.Append("<td align='right'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>")
                                If from_spot = "valpdf" Or from_spot = "pdf" Then
                                Else
                                    htmlOut.Append("<A href='FAAFlightData.aspx?acid=0&aport_id1=" & r("ffd_origin_aport_id") & "&aport_id2=" & r("ffd_dest_aport_id") & "&pc=" & sSelectedProductCode & "&start_date=" & searchCriteria.ViewCriteriaDocumentsStartDate & "&end_date=" & searchCriteria.ViewCriteriaDocumentsEndDate & "&comp_id=" & searchCriteria.ViewCriteriaCompanyID & "' target='_blank'>")
                                End If
                                htmlOut.Append(FormatNumber(r("NbrFlights"), 0))

                                If from_spot = "valpdf" Then
                                Else
                                    htmlOut.Append("</a>")
                                End If
                                htmlOut.Append("</font></td>")


                                htmlOut.Append("<td align='right'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>")
                                htmlOut.Append(FormatNumber(r("TotalFlightTimeHrs"), 0))
                                htmlOut.Append("</font></td>")

                                If from_spot = "pdf" Then
                                    htmlOut.Append("<td align='right'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>")


                                    temp_gal_lit = FormatNumber(r("TotalFuelBurn"), 0)

                                    If show_fuel_burn_in_liters = True Then
                                        temp_gal_lit = FormatNumber((temp_gal_lit * 3.78541), 0)
                                    End If

                                    htmlOut.Append(FormatNumber(temp_gal_lit, 0))

                                    htmlOut.Append("</font></td>")
                                End If

                                htmlOut.Append("</tr>")
                            End If

                        Else
                            If Trim(htmlOut.ToString.Trim) <> "" Then
                                htmlOut.Append(",")
                            End If
                            htmlOut.Append("{")
                            htmlOut.Append("""check"": """",") 'Checkbox row.

                            ' the first one is origin, second is destination


                            If Trim(airport_direction) = "X" Or Trim(SubHeaderString) = "Routes" Or ((searchCriteria.ViewCriteriaCompanyID > 0 Or Operator_IDS_String <> "") And Airport_ID_OVERALL <= 1 And Trim(Airport_IDS_String) = "" And initial = False) Or (Airport_IDS_String <> "" And initial = False) Or (Airport_ID_OVERALL > 1 And initial = False) Then


                                htmlOut.Append("""origin"":""")
                                htmlOut.Append("<a href='view_template.aspx?ViewID=28&ViewName=Operator/Airport Utilization&aport_id=" & r.Item("ffd_origin_aport_id").ToString & "&comp_id=" & searchCriteria.ViewCriteriaCompanyID & "'>")

                                htmlOut.Append("" & clsGeneral.clsGeneral.StripChars(Replace(Replace(r.Item("origin_aport_name").ToString, "Airport", ""), "International", ""), False) & "</a></br>")
                                htmlOut.Append("" & r.Item("origin_aport_iata_code").ToString & " / ")
                                htmlOut.Append("" & r.Item("origin_aport_icao_code").ToString & "")
                                htmlOut.Append(""",")

                                ' added the 2nd or statement - MSW - 10/4/2018
                                If Trim(airport_direction) = "X " Or Trim(SubHeaderString) = "Routes" Or (Airport_ID_OVERALL > 0 Or (Trim(Airport_IDS_String) = "" And Airport_ID_OVERALL = 0)) Then
                                    htmlOut.Append("""city2"":""" & r.Item("origin_aport_city").ToString & """,")
                                    htmlOut.Append("""state2"":""" & r.Item("origin_aport_state").ToString & """,")
                                    htmlOut.Append("""country2"":""" & r.Item("origin_aport_country").ToString & """,")
                                    htmlOut.Append("""continent2"":""" & r.Item("origin_continent").ToString & """,")
                                End If

                                If Trim(airport_direction) = "X" Or Trim(SubHeaderString) = "Routes" Or (Airport_ID_OVERALL <= 1 And initial = False) Or (Airport_ID_OVERALL > 1 And airport_direction = "O" And initial = False) Then
                                    If Trim(airport_direction) = "X" Or Trim(SubHeaderString) = "Routes" Or (Airport_IDS_String <> "" Or (Airport_ID_OVERALL > 1 And airport_direction = "O" And initial = False) Or (Operator_IDS_String <> "" And initial = False) Or (Airport_ID_OVERALL <= 1 And searchCriteria.ViewCriteriaCompanyID > 0 And initial = False)) Then

                                        htmlOut.Append("""destination"":""")

                                        htmlOut.Append("<a href='view_template.aspx?ViewID=28&ViewName=Operator/Airport Utilization&aport_id=" & r.Item("ffd_dest_aport_id").ToString & "&comp_id=" & searchCriteria.ViewCriteriaCompanyID & "'>")

                                        htmlOut.Append("" & clsGeneral.clsGeneral.StripChars(Replace(Replace(r.Item("dest_aport_name").ToString, "Airport", ""), "International", ""), False) & "</a><br />")
                                        htmlOut.Append(r.Item("dest_aport_iata_code").ToString & " / ")
                                        htmlOut.Append(r.Item("dest_aport_icao_code").ToString)
                                        htmlOut.Append(""",")
                                        If Trim(airport_direction) = "X" Or (Airport_ID_OVERALL > 0 Or (Trim(Airport_IDS_String) = "" And Airport_ID_OVERALL = 0)) Then
                                            htmlOut.Append("""city"":""" & r.Item("dest_aport_city").ToString & """,")
                                            htmlOut.Append("""state"":""" & r.Item("dest_aport_state").ToString & """,")
                                            htmlOut.Append("""country"":""" & r.Item("dest_aport_country").ToString & """,")
                                            htmlOut.Append("""continent"":""" & r.Item("dest_continent").ToString & """,")
                                        End If
                                    End If
                                End If


                                ' i dont beleive it matters where this goes
                                If Trim(SubHeaderString) = "Routes" Then
                                    If Not IsDBNull(r.Item("AvgMinPerFlights")) Then
                                        htmlOut.Append("""AvgMinPerFlights"":""" & r.Item("AvgMinPerFlights").ToString & """,")
                                    Else
                                        htmlOut.Append("""AvgMinPerFlights"":""0"",")
                                    End If

                                    If Not IsDBNull(r.Item("TotalFuelBurnPerFlight")) Then
                                        htmlOut.Append("""TotalFuelBurnPerFlight"":""" & FormatNumber(r.Item("TotalFuelBurnPerFlight").ToString, 0) & """,")
                                    Else
                                        htmlOut.Append("""TotalFuelBurnPerFlight"":""0"",")
                                    End If

                                    If Not IsDBNull(r.Item("RouteDistance")) Then
                                        htmlOut.Append("""RouteDistance"":""" & r.Item("RouteDistance").ToString & """,")
                                    Else
                                        htmlOut.Append("""RouteDistance"":""0"",")
                                    End If
                                End If


                                htmlOut.Append("""flights"":""<A href='FAAFlightData.aspx?acid=0&aport_id1=" & r("ffd_origin_aport_id") & "&aport_id2=" & r("ffd_dest_aport_id") & "&orig_direction=1&pc=" & sSelectedProductCode & "&start_date=" & searchCriteria.ViewCriteriaDocumentsStartDate & "&end_date=" & searchCriteria.ViewCriteriaDocumentsEndDate & "&comp_id=" & searchCriteria.ViewCriteriaCompanyID & "' target='_blank' alt='Route Analysis' name='Route Analysis' title='Route Analysis'>" & FormatNumber(r("NbrFlights"), 0) & "</a>"",")

                            Else
                                If Trim(airport_direction) = "D" Then
                                    htmlOut.Append("""iata"":""<a href='view_template.aspx?ViewID=28&ViewName=Operator/Airport Utilization&aport_id=" & r.Item("ffd_dest_aport_id").ToString & "&comp_id=" & searchCriteria.ViewCriteriaCompanyID & "'>")
                                    htmlOut.Append(r.Item("dest_aport_iata_code").ToString & "</a>"",")
                                    htmlOut.Append("""icao"":""<a href='view_template.aspx?ViewID=28&ViewName=Operator/Airport Utilization&aport_id=" & r.Item("ffd_dest_aport_id").ToString & "&comp_id=" & searchCriteria.ViewCriteriaCompanyID & "'>" & r.Item("dest_aport_icao_code").ToString & "</a>"",")
                                    htmlOut.Append("""airport"":""" & Replace(Replace(r.Item("dest_aport_name").ToString, "Airport", ""), "International", "") & """,")
                                    htmlOut.Append("""city"":""" & r.Item("dest_aport_city").ToString & """,")
                                    htmlOut.Append("""state"":""" & r.Item("dest_aport_state").ToString & """,")
                                    htmlOut.Append("""country"":""" & r.Item("dest_aport_country").ToString & """,")
                                    htmlOut.Append("""continent"":""" & r.Item("dest_continent").ToString & """,")
                                    htmlOut.Append("""flights"":""" & FormatNumber(r("NbrFlights"), 0) & """,")
                                ElseIf Trim(airport_direction) = "O" Then
                                    htmlOut.Append("""iata"":""<a href='view_template.aspx?ViewID=28&ViewName=Operator/Airport Utilization&aport_id=" & r.Item("ffd_origin_aport_id").ToString & "&comp_id=" & searchCriteria.ViewCriteriaCompanyID & "'>")
                                    htmlOut.Append(r.Item("origin_aport_iata_code").ToString & "</a>"",")
                                    htmlOut.Append("""icao"":""<a href='view_template.aspx?ViewID=28&ViewName=Operator/Airport Utilization&aport_id=" & r.Item("ffd_origin_aport_id").ToString & "&comp_id=" & searchCriteria.ViewCriteriaCompanyID & "'>" & r.Item("origin_aport_icao_code").ToString & "</a>"",")
                                    htmlOut.Append("""airport"":""" & Replace(Replace(r.Item("origin_aport_name").ToString, "Airport", ""), "International", "") & """,")
                                    htmlOut.Append("""city"":""" & r.Item("origin_aport_city").ToString & """,")
                                    htmlOut.Append("""state"":""" & r.Item("origin_aport_state").ToString & """,")
                                    htmlOut.Append("""country"":""" & r.Item("origin_aport_country").ToString & """,")
                                    htmlOut.Append("""continent"":""" & r.Item("origin_continent").ToString & """,")
                                    htmlOut.Append("""flights"":""" & FormatNumber(r("NbrFlights"), 0) & """,")
                                Else
                                    htmlOut.Append("""iata"":""<a href='view_template.aspx?ViewID=28&ViewName=Operator/Airport Utilization&aport_id=" & r.Item("ffd_dest_aport_id").ToString & "&comp_id=" & searchCriteria.ViewCriteriaCompanyID & "'>")
                                    htmlOut.Append(r.Item("dest_aport_iata_code").ToString & "</a>"",")
                                    htmlOut.Append("""icao"":""<a href='view_template.aspx?ViewID=28&ViewName=Operator/Airport Utilization&aport_id=" & r.Item("ffd_dest_aport_id").ToString & "&comp_id=" & searchCriteria.ViewCriteriaCompanyID & "'>" & r.Item("dest_aport_icao_code").ToString & "</a>"",")
                                    htmlOut.Append("""airport"":""" & Replace(Replace(r.Item("dest_aport_name").ToString, "Airport", ""), "International", "") & """,")
                                    htmlOut.Append("""city"":""" & r.Item("dest_aport_city").ToString & """,")
                                    htmlOut.Append("""state"":""" & r.Item("dest_aport_state").ToString & """,")
                                    htmlOut.Append("""country"":""" & r.Item("dest_aport_country").ToString & """,")
                                    htmlOut.Append("""continent"":""" & r.Item("dest_continent").ToString & """,")
                                    htmlOut.Append("""flights"":""" & FormatNumber(r("NbrFlights"), 0) & """,")
                                End If
                            End If

                            If Trim(SubHeaderString) = "Airports" Then
                                If Not IsDBNull(r.Item("AvgMinPerFlights")) Then
                                    htmlOut.Append("""AvgMinPerFlights"":""" & r.Item("AvgMinPerFlights").ToString & """,")
                                Else
                                    htmlOut.Append("""AvgMinPerFlights"":""0"",")
                                End If
                            End If



                            htmlOut.Append("""hours"":""" & FormatNumber(r("TotalFlightTimeHrs"), 1) & """,")
                            htmlOut.Append("""fuel"":""" & FormatNumber(r("TotalFuelBurn"), 0) & """")

                            ' added the Trim(airport_direction) = "X" and since that is already included above, so dont need to double include 
                            If Trim(airport_direction) = "X" Or Trim(SubHeaderString) = "Routes" And ((searchCriteria.ViewCriteriaCompanyID > 0 Or Operator_IDS_String <> "") And Airport_ID_OVERALL <= 1 And Trim(Airport_IDS_String) = "" And initial = False) Or (Airport_IDS_String <> "" And initial = False) Or (Airport_ID_OVERALL > 1 And initial = False) Then
                                ' if there is no airport idea, then show the moved columns
                                If Trim(Airport_IDS_String) <> "" Then
                                    htmlOut.Append(", ""city2"":""" & r.Item("origin_aport_city").ToString & """,")
                                    htmlOut.Append("""state2"":""" & r.Item("origin_aport_state").ToString & """,")
                                    htmlOut.Append("""country2"":""" & r.Item("origin_aport_country").ToString & """,")
                                    htmlOut.Append("""continent2"":""" & r.Item("origin_continent").ToString & """")

                                    If (Airport_ID_OVERALL <= 1 And initial = False) Or (Airport_ID_OVERALL > 1 And airport_direction = "O" And initial = False) Then
                                        If Airport_IDS_String <> "" Or (Airport_ID_OVERALL > 1 And airport_direction = "O" And initial = False) Or (Operator_IDS_String <> "" And initial = False) Or (Airport_ID_OVERALL <= 1 And searchCriteria.ViewCriteriaCompanyID > 0 And initial = False) Then
                                            htmlOut.Append(", ""city"":""" & r.Item("dest_aport_city").ToString & """,")
                                            htmlOut.Append("""state"":""" & r.Item("dest_aport_state").ToString & """,")
                                            htmlOut.Append("""country"":""" & r.Item("dest_aport_country").ToString & """,")
                                            htmlOut.Append("""continent"":""" & r.Item("dest_continent").ToString & """")
                                        End If
                                    End If
                                End If
                            End If


                            htmlOut.Append("}")

                        End If
                        tcount += 1
                    Next
                End If
            End If
            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "Utilization_functions.vb", "end fill array")

            'If SubHeaderString = "Routes" Then
            '    If only_one = True Then
            '        'temp_String = htmlOut.ToString
            '        'temp_String = Replace(temp_String, """,", "<br/><br/>"",")
            '        'htmlOut.Length = 0
            '        'htmlOut.Append(temp_String)
            '        '  htmlOut.Append("," & htmlOut.ToString)

            '        '---------------------- BLANK ROW SECTION -----------------------------------
            '        htmlOut.Append(", {")
            '        htmlOut.Append("""check"": """",") 'Checkbox row. 
            '        ' the first one is origin, second is destination  
            '        If Trim(airport_direction) = "X" Or Trim(SubHeaderString) = "Routes" Or ((searchCriteria.ViewCriteriaCompanyID > 0 Or Operator_IDS_String <> "") And Airport_ID_OVERALL <= 1 And Trim(Airport_IDS_String) = "" And initial = False) Or (Airport_IDS_String <> "" And initial = False) Or (Airport_ID_OVERALL > 1 And initial = False) Then
            '            htmlOut.Append("""origin"":""Click on Airport to Analyze This Route"",")
            '            ' added the 2nd or statement - MSW - 10/4/2018
            '            If Trim(airport_direction) = "X " Or Trim(SubHeaderString) = "Routes" Or (Airport_ID_OVERALL > 0 Or (Trim(Airport_IDS_String) = "" And Airport_ID_OVERALL = 0)) Then
            '                htmlOut.Append("""city2"":"""",")
            '                htmlOut.Append("""state2"":"""",")
            '                htmlOut.Append("""country2"":"""",")
            '                htmlOut.Append("""continent2"":"""",")
            '            End If

            '            If Trim(airport_direction) = "X" Or Trim(SubHeaderString) = "Routes" Or (Airport_ID_OVERALL <= 1 And initial = False) Or (Airport_ID_OVERALL > 1 And airport_direction = "O" And initial = False) Then
            '                If Trim(airport_direction) = "X" Or Trim(SubHeaderString) = "Routes" Or (Airport_IDS_String <> "" Or (Airport_ID_OVERALL > 1 And airport_direction = "O" And initial = False) Or (Operator_IDS_String <> "" And initial = False) Or (Airport_ID_OVERALL <= 1 And searchCriteria.ViewCriteriaCompanyID > 0 And initial = False)) Then
            '                    htmlOut.Append("""destination"":"""",")
            '                    If Trim(airport_direction) = "X" Or (Airport_ID_OVERALL > 0 Or (Trim(Airport_IDS_String) = "" And Airport_ID_OVERALL = 0)) Then
            '                        htmlOut.Append("""city"":"""",")
            '                        htmlOut.Append("""state"":"""",")
            '                        htmlOut.Append("""country"":"""",")
            '                        htmlOut.Append("""continent"":"""",")
            '                    End If
            '                End If
            '            End If
            '            ' i dont beleive it matters where this goes
            '            If Trim(SubHeaderString) = "Routes" Then
            '                htmlOut.Append("""AvgMinPerFlights"":"""",")
            '                htmlOut.Append("""TotalFuelBurnPerFlight"":"""",")
            '                htmlOut.Append("""RouteDistance"":"""",")
            '            End If
            '            htmlOut.Append("""flights"":"""",")
            '        Else
            '            If Trim(airport_direction) = "D" Then
            '                htmlOut.Append("""iata"":"""",")
            '                htmlOut.Append("""icao"":"""",")
            '                htmlOut.Append("""airport"":"""",")
            '                htmlOut.Append("""city"":"""",")
            '                htmlOut.Append("""state"":"""",")
            '                htmlOut.Append("""country"":"""",")
            '                htmlOut.Append("""continent"":"""",")
            '                htmlOut.Append("""flights"":"""",")
            '            ElseIf Trim(airport_direction) = "O" Then
            '                htmlOut.Append("""iata"":"""",")
            '                htmlOut.Append("""icao"":"""",")
            '                htmlOut.Append("""airport"":"""",")
            '                htmlOut.Append("""city"":"""",")
            '                htmlOut.Append("""state"":"""",")
            '                htmlOut.Append("""country"":"""",")
            '                htmlOut.Append("""continent"":"""",")
            '                htmlOut.Append("""flights"":"""",")
            '            Else
            '                htmlOut.Append("""iata"":"""",")
            '                htmlOut.Append("""icao"":"""",")
            '                htmlOut.Append("""airport"":"""",")
            '                htmlOut.Append("""city"":"""",")
            '                htmlOut.Append("""state"":"""",")
            '                htmlOut.Append("""country"":"""",")
            '                htmlOut.Append("""continent"":"""",")
            '                htmlOut.Append("""flights"":"""",")
            '            End If
            '        End If

            '        If Trim(SubHeaderString) = "Airports" Then
            '            htmlOut.Append("""AvgMinPerFlights"":"""",")
            '        End If

            '        htmlOut.Append("""hours"":""  "",")
            '        htmlOut.Append("""fuel"":""  """)

            '        ' added the Trim(airport_direction) = "X" and since that is already included above, so dont need to double include 
            '        If Trim(airport_direction) = "X" Or Trim(SubHeaderString) = "Routes" And ((searchCriteria.ViewCriteriaCompanyID > 0 Or Operator_IDS_String <> "") And Airport_ID_OVERALL <= 1 And Trim(Airport_IDS_String) = "" And initial = False) Or (Airport_IDS_String <> "" And initial = False) Or (Airport_ID_OVERALL > 1 And initial = False) Then
            '            ' if there is no airport idea, then show the moved columns
            '            If Trim(Airport_IDS_String) <> "" Then
            '                htmlOut.Append(", ""city2"":""  "",")
            '                htmlOut.Append("""state2"":"""",")
            '                htmlOut.Append("""country2"":"""",")
            '                htmlOut.Append("""continent2"":""""")

            '                If (Airport_ID_OVERALL <= 1 And initial = False) Or (Airport_ID_OVERALL > 1 And airport_direction = "O" And initial = False) Then
            '                    If Airport_IDS_String <> "" Or (Airport_ID_OVERALL > 1 And airport_direction = "O" And initial = False) Or (Operator_IDS_String <> "" And initial = False) Or (Airport_ID_OVERALL <= 1 And searchCriteria.ViewCriteriaCompanyID > 0 And initial = False) Then
            '                        htmlOut.Append(", ""city"":"""",")
            '                        htmlOut.Append("""state"":"""",")
            '                        htmlOut.Append("""country"":"""",")
            '                        htmlOut.Append("""continent"":""""")
            '                    End If
            '                End If
            '            End If
            '        End If


            '        htmlOut.Append("}")
            '    End If
            'End If
            ''---------------------- BLANK ROW SECTION -----------------------------------

            If InStr(Trim(from_spot), "pdf") > 0 Then
                htmlOut.Append("</tbody></table></div></td></tr>")
            End If

        Catch ex As Exception

            aError = "Error in MostCommonOriginsJSArray(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByVal product_code_selection As String, ByRef table_count As Long)" + ex.Message

        Finally
            If InStr(Trim(from_spot), "pdf") > 0 Then
                Results = htmlOut.ToString.Trim
                out_htmlString = Results
            Else
                Results = " var mostCommonOriginsDataset = [ " & htmlOut.ToString.Trim & " ]; "
                out_htmlString = Results
            End If


        End Try

    End Sub
    Public Sub Refuel_Tech_Stops(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByVal product_code_selection As String, ByRef table_count As Long, ByVal initial As Boolean, ByRef AirportTab As Boolean, ByRef FlightTotals As Long, ByVal from_spot As String, ByVal table_color As String, ByVal temp_pdf_header As String, Optional ByRef LimitThisQuery As Long = 0, Optional ByRef SubHeaderString As String = "", Optional ByVal sSelectedProductCode As String = "", Optional ByVal percentage_drop As Long = 0, Optional ByVal minutes_drop As Long = 0, Optional ByVal show_fuel_burn_in_liters As Boolean = False)

        Dim ResultsTable As New DataTable
        Dim toggleRowColor As Boolean = False
        Dim Results As String = ""
        Dim htmlOut As New StringBuilder
        Dim tcount As Long = 0
        Dim temp_date As New Date
        Dim temp_string As String = ""
        Dim flight1 As Long = 0
        Dim flight2 As Long = 0
        Dim temp_gal_lit As Integer = 0


        Try

            If InStr(Trim(from_spot), "pdf") > 0 Then
                LimitThisQuery = 41
            End If

            ResultsTable = GetRefuel(searchCriteria, product_code_selection, 0, percentage_drop, minutes_drop, from_spot, LimitThisQuery)

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "Utilization_functions.vb", "start fill array")
            If Not IsNothing(ResultsTable) Then

                If InStr(Trim(from_spot), "pdf") > 0 Then 'Or Trim(from_spot) = "valpdf" Then
                    htmlOut.Append("<tr><td valign='top' align='center'><font class='" & HttpContext.Current.Session("FONT_CLASS_HEADER") & " mainHeading'>" & SubHeaderString & "</font></td></tr>")
                End If


                If ResultsTable.Rows.Count > 0 Then
                    table_count = ResultsTable.Rows.Count

                    If InStr(Trim(from_spot), "pdf") > 0 Then 'Or Trim(from_spot) = "valpdf" Then

                        htmlOut.Append("<tr><td valign='top'><div class=""Box""><table id=""weightTable"" cellpadding='3'  cellspacing='0' width='100%' class='formatTable " & table_color & " " & IIf(from_spot = "valpdf", "large", "") & "'><thead>")
                        htmlOut.Append("<tr class=""noBorder"">")
                        'Departed	Aircraft	Ser#	Reg#	Based at	MOG	Operator	Country	Fuel Burn
                        htmlOut.Append("<th>DATE</th>")
                        htmlOut.Append("<th>AIRCRAFT</th>")
                        htmlOut.Append("<th>SER NO</th>")
                        htmlOut.Append("<th>REG NO</th>")
                        '   htmlOut.Append("<th>BASED AT</th>")
                        htmlOut.Append("<th>Min On<br/>Ground</th>")
                        htmlOut.Append("<th>OPERATOR</th>")
                        htmlOut.Append("<th>COUNTRY</th>")

                        If show_fuel_burn_in_liters = True Then
                            htmlOut.Append("<th class='right'>FUEL<br/>BURN (L)</th>")
                        Else
                            htmlOut.Append("<th class='right'>FUEL<br/>BURN (GAL)</th>")
                        End If

                        htmlOut.Append("</tr>")
                        htmlOut.Append("</thead>")
                        htmlOut.Append("<tbody>")
                    End If

                    For Each r As DataRow In ResultsTable.Rows

                        If InStr(Trim(from_spot), "pdf") > 0 Then


                            If tcount > 40 Then

                            Else

                                htmlOut.Append("<tr valign='top'>")


                                '  comp_id
                                htmlOut.Append("<td><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'><font size='-1'>")
                                If Not IsDBNull(r.Item("L1DEPARTED")) Then
                                    htmlOut.Append("" & FormatDateTime(r.Item("L1DEPARTED"), DateFormat.ShortDate))
                                End If
                                htmlOut.Append("&nbsp;</font></font></td>")


                                htmlOut.Append("<td><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'><font size='-1'>")
                                htmlOut.Append("" & r.Item("Make").ToString & " " & r.Item("Model").ToString)
                                htmlOut.Append("</font></font></td>")

                                htmlOut.Append("<td><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'><font size='-1'>")
                                htmlOut.Append("" & r.Item("SerNbr").ToString)
                                htmlOut.Append("</font></font></td>")

                                htmlOut.Append("<td><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'><font size='-1'>")
                                htmlOut.Append("" & r.Item("RegNbr").ToString)
                                htmlOut.Append("</font></font></td>")


                                'htmlOut.Append("<td><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'><font size='-1'>")
                                'If Not IsDBNull(r.Item("BaseAirport")) Then
                                '  htmlOut.Append("" & Replace(r.Item("BaseAirport").ToString, "International", "Intl."))
                                'End If
                                'htmlOut.Append("&nbsp;</font></font></td>")

                                Dim secondFlight As String() = Split("", "")
                                If Not IsDBNull(r("SECONDFLIGHT")) Then
                                    secondFlight = Split(r("SECONDFLIGHT"), ",")
                                End If

                                htmlOut.Append("<td align='right'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'><font size='-1'>")
                                If UBound(secondFlight) >= 0 Then
                                    htmlOut.Append(Replace(secondFlight(0), "ONGROUND:", ""))
                                End If
                                htmlOut.Append("</font></font></td>")


                                htmlOut.Append("<td><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'><font size='-1'>")
                                If Not IsDBNull(r.Item("comp_name")) Then
                                    htmlOut.Append("" & Replace(r.Item("comp_name").ToString, "International", "Intl."))
                                End If
                                htmlOut.Append("&nbsp;</font></font></td>")


                                htmlOut.Append("<td><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'><font size='-1'>")
                                If Not IsDBNull(r("comp_country")) Then
                                    htmlOut.Append("" & r.Item("comp_country").ToString)
                                End If
                                htmlOut.Append("</font></font></td>")


                                htmlOut.Append("<td align='right'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'><font size='-1'>")

                                flight1 = 0
                                flight2 = 0
                                If Not IsDBNull(r("TotalFuelBurn1")) Then
                                    flight1 = r.Item("TotalFuelBurn1")
                                End If
                                If Not IsDBNull(r("SECONDFUELBURN")) Then
                                    flight2 = r.Item("SECONDFUELBURN")
                                End If

                                If flight1 >= flight2 Then
                                    temp_gal_lit = flight1
                                Else
                                    temp_gal_lit = flight2
                                End If

                                If show_fuel_burn_in_liters = True Then
                                    temp_gal_lit = FormatNumber((temp_gal_lit * 3.78541), 1)
                                End If

                                htmlOut.Append(FormatNumber(temp_gal_lit, 0))

                                htmlOut.Append("</font></font></td>")


                                htmlOut.Append("</tr>")
                            End If

                        Else


                        End If
                        tcount += 1
                    Next
                Else
                    If InStr(Trim(from_spot), "pdf") > 0 Then
                        htmlOut.Append("<tr><td valign='top' align='center'><font class='" & HttpContext.Current.Session("FONT_CLASS_HEADER") & " mainHeading'>No Results Found</font></td></tr>")
                    End If
                End If
            Else
                If InStr(Trim(from_spot), "pdf") > 0 Then
                    htmlOut.Append("<tr><td valign='top' align='center'><font class='" & HttpContext.Current.Session("FONT_CLASS_HEADER") & " mainHeading'>No Results Found</font></td></tr>")
                End If
            End If
            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "Utilization_functions.vb", "end fill array")


            If InStr(Trim(from_spot), "pdf") > 0 Then
                htmlOut.Append("</tbody></table></div></td></tr>")
            End If

        Catch ex As Exception

            aError = "Error in MostCommonOriginsJSArray(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByVal product_code_selection As String, ByRef table_count As Long)" + ex.Message

        Finally
            If InStr(Trim(from_spot), "pdf") > 0 Then
                Results = htmlOut.ToString.Trim
                out_htmlString = Results
            Else
                Results = " var mostCommonOriginsDataset = [ " & htmlOut.ToString.Trim & " ]; "
                out_htmlString = Results
            End If


        End Try

    End Sub
    Public Sub ArrivalsDeparturesJSArray(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByVal product_code_selection As String, ByRef table_count As Long, ByVal initial As Boolean, ByRef AirportTab As Boolean, ByRef FlightTotals As Long, ByVal from_spot As String, ByVal table_color As String, ByVal temp_pdf_header As String, Optional ByRef LimitThisQuery As Long = 0, Optional ByRef SubHeaderString As String = "", Optional ByVal sSelectedProductCode As String = "", Optional ByVal arrivals_departures As String = "D")

        Dim ResultsTable As New DataTable
        Dim toggleRowColor As Boolean = False
        Dim Results As String = ""
        Dim htmlOut As New StringBuilder
        Dim tcount As Long = 0
        Dim temp_date As New Date
        Dim temp_string As String = ""

        Try

            ResultsTable = get_arriavals_departures(searchCriteria, product_code_selection, False, AirportTab, FlightTotals, LimitThisQuery, sSelectedProductCode, arrivals_departures)

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "Utilization_functions.vb", "start fill array")
            If Not IsNothing(ResultsTable) Then

                If ResultsTable.Rows.Count > 0 Then
                    table_count = ResultsTable.Rows.Count

                    If InStr(Trim(from_spot), "pdf") > 0 Then 'Or Trim(from_spot) = "valpdf" Then
                        htmlOut.Append("<tr><td valign='top' align='center'><font class='" & HttpContext.Current.Session("FONT_CLASS_HEADER") & " mainHeading'>" & SubHeaderString & "</font></td></tr>")

                        htmlOut.Append("<tr><td valign='top'><div class=""Box""><table id=""weightTable"" cellpadding='3'  cellspacing='0' width='100%' class='formatTable " & table_color & " " & IIf(from_spot = "valpdf", "large", "") & "'><thead>")
                        htmlOut.Append("<tr class=""noBorder"">")

                        htmlOut.Append("<th>MAKE/MODEL</th>")
                        htmlOut.Append("<th>SER NO</th>")
                        htmlOut.Append("<th>REG NO</th>")

                        If Trim(arrivals_departures) = "D" Then
                            htmlOut.Append("<th>DEPARTURE</th>")
                            htmlOut.Append("<th>DAY</th>")
                            htmlOut.Append("<th>DESTINATION</th>")
                        Else
                            htmlOut.Append("<th>ARRIVAL</th>")
                            htmlOut.Append("<th>DAY</th>")
                            htmlOut.Append("<th>ORIGIN</th>")
                        End If

                        htmlOut.Append("</tr>")
                        htmlOut.Append("</thead>")
                        htmlOut.Append("<tbody>")
                    End If

                    For Each r As DataRow In ResultsTable.Rows

                        If InStr(Trim(from_spot), "pdf") > 0 Then


                            If tcount > 40 Then
                                'tcount = 0
                                'htmlOut.Append("</table></td></tr></table>")
                                'htmlOut.Append(comp_functions.NEW_Insert_Page_Break_PDF(0, "pdf"))
                                'htmlOut.Append(temp_pdf_header)
                                ''  If bWordReport = True Then
                                ''  htmlOut.Append("<table width='" & word_width & "' align='center' cellpadding='3'>")
                                ''Else
                                'htmlOut.Append("<table width='95%' align='center' cellpadding='3'>")
                                ''  End If

                                'htmlOut.Append("<tr><td valign='top' align='center'><font class='" & HttpContext.Current.Session("FONT_CLASS_HEADER") & " mainHeading'><strong>" & searchCriteria.ViewCriteriaAirportName & "</strong> Routes</font></td></tr>")

                                'htmlOut.Append("<tr><td valign='top'><div class=""Box""><table id=""weightTable"" cellpadding='3'  cellspacing='0' width='100%' class='formatTable " & table_color & "'><thead>")
                                'htmlOut.Append("<tr class=""noBorder"">")
                                'htmlOut.Append("<th>ORIGIN</th>")
                                'If Airport_ID_OVERALL = 0 Then
                                '  htmlOut.Append("<th>DESTINATION</th>")
                                'End If
                                'htmlOut.Append("<th>NBR<br/>FLIGHTS</th>")
                                'htmlOut.Append("<th>TOTAL<br/>FLIGHT HOURS</th>")
                                'htmlOut.Append("<th>EST FUEL<br/>BURN</th>")
                                'htmlOut.Append("</tr>")
                                'htmlOut.Append("</thead>")
                                'htmlOut.Append("<tbody>")
                            Else

                                htmlOut.Append("<tr>")

                                'If (searchCriteria.ViewCriteriaCompanyID > 0 And Airport_ID_OVERALL = 1 And Trim(Airport_IDS_String) = "" And initial = False) Or (Airport_IDS_String <> "" And initial = False) Or (Airport_ID_OVERALL > 1 And initial = False) Or (from_spot = "valpdf") Then
                                '  htmlOut.Append("<td><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>")


                                '  If Not from_spot = "valpdf" Then
                                '    htmlOut.Append("" & clsGeneral.clsGeneral.StripChars(Replace(r.Item("origin_aport_name").ToString, " Airport", " "), False))
                                '    htmlOut.Append(" (" & Replace(r.Item("origin_aport_country").ToString, "United States", "U.S.") & " - " & clsGeneral.clsGeneral.StripChars(r.Item("origin_aport_city").ToString, False) & " " & clsGeneral.clsGeneral.StripChars(r.Item("origin_aport_state").ToString, False) & ") ")
                                '    htmlOut.Append("" & r.Item("origin_aport_iata_code").ToString & "</a> / ")
                                '    htmlOut.Append("" & r.Item("origin_aport_icao_code").ToString & "")
                                '  Else
                                '    htmlOut.Append("" & Replace(clsGeneral.clsGeneral.StripChars(r.Item("origin_aport_name").ToString, False), "Airport", ""))
                                '  End If


                                '  htmlOut.Append("</font></td>")

                                '  If Airport_ID_OVERALL = 0 Then
                                '    htmlOut.Append("<td><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>")

                                '    If Not from_spot = "valpdf" Then
                                '      htmlOut.Append("" & clsGeneral.clsGeneral.StripChars(r.Item("dest_aport_name").ToString, False))
                                '      htmlOut.Append("<br />(" & Replace(r.Item("dest_aport_country").ToString, "United States", "U.S.") & " - " & clsGeneral.clsGeneral.StripChars(r.Item("dest_aport_city").ToString, False) & " " & clsGeneral.clsGeneral.StripChars(r.Item("dest_aport_state").ToString, False) & ") ")
                                '      htmlOut.Append(r.Item("dest_aport_iata_code").ToString & "</a> / ")
                                '      htmlOut.Append(r.Item("dest_aport_icao_code").ToString)
                                '    Else
                                '      htmlOut.Append("" & Replace(clsGeneral.clsGeneral.StripChars(Replace(Replace(r.Item("dest_aport_name").ToString, "Airport", ""), "International", ""), False), "Airport", ""))
                                '    End If


                                '    htmlOut.Append("</font></td>")
                                '  End If
                                'Else


                                htmlOut.Append("<td><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'><font size='-1'>")
                                htmlOut.Append("" & r.Item("amod_make_name").ToString & " " & r.Item("amod_model_name").ToString)
                                htmlOut.Append("</font></font></td>")

                                htmlOut.Append("<td><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'><font size='-1'>")
                                htmlOut.Append("" & r.Item("ac_ser_no_full").ToString)
                                htmlOut.Append("</font></font></td>")

                                htmlOut.Append("<td><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'><font size='-1'>")
                                htmlOut.Append("" & r.Item("ac_reg_no").ToString)
                                htmlOut.Append("</font></font></td>")

                                htmlOut.Append("<td><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'><font size='-1'>")
                                If Not IsDBNull(r.Item("flight_date")) Then
                                    temp_string = r.Item("flight_date")

                                    For i = 2000 To Year(Date.Now)
                                        temp_string = Replace(Trim(temp_string), "/" & i, "/" & Right(Trim(i), 2))
                                    Next
                                    temp_string = Replace(temp_string, ":00 PM", " <font size='-1'>PM</font>")
                                    temp_string = Replace(temp_string, ":00 AM", " <font size='-1'>AM</font>")
                                    htmlOut.Append("" & temp_string)

                                End If
                                htmlOut.Append("</font></td>")

                                htmlOut.Append("<td><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>")
                                If Not IsDBNull(r.Item("flight_date")) Then
                                    temp_date = r.Item("flight_date").ToString
                                    If temp_date.DayOfWeek = DayOfWeek.Monday Then
                                        htmlOut.Append("<font size='-1'>Monday</font>")
                                    ElseIf temp_date.DayOfWeek = DayOfWeek.Tuesday Then
                                        htmlOut.Append("<font size='-1'>Tuesday</font>")
                                    ElseIf temp_date.DayOfWeek = DayOfWeek.Wednesday Then
                                        htmlOut.Append("<font size='-1'>Wed.</font>")
                                    ElseIf temp_date.DayOfWeek = DayOfWeek.Thursday Then
                                        htmlOut.Append("<font size='-1'>Thursday</font>")
                                    ElseIf temp_date.DayOfWeek = DayOfWeek.Friday Then
                                        htmlOut.Append("<font size='-1'>Friday</font>")
                                    ElseIf temp_date.DayOfWeek = DayOfWeek.Saturday Then
                                        htmlOut.Append("<font size='-1'>Saturday</font>")
                                    ElseIf temp_date.DayOfWeek = DayOfWeek.Sunday Then
                                        htmlOut.Append("<font size='-1'>Sunday</font>")
                                    End If
                                End If
                                htmlOut.Append("</font></td>")


                                htmlOut.Append("<td><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'><font size='-1'>")

                                '  If Not from_spot = "valpdf" Then
                                htmlOut.Append("" & clsGeneral.clsGeneral.StripChars(Replace(Replace(Replace(r.Item("aport_name").ToString, " Airport", " "), "Internacional", "Intl."), "Aeropuerto ", "Aero. "), False))
                                htmlOut.Append(" (" & Replace(Replace(r.Item("aport_country").ToString, "United States", "U.S."), "Internacional", "Intl.") & " - " & clsGeneral.clsGeneral.StripChars(r.Item("aport_city").ToString, False) & " " & clsGeneral.clsGeneral.StripChars(r.Item("aport_state").ToString, False) & ")</font> ")
                                htmlOut.Append("" & r.Item("aport_iata").ToString & "</a></font><font size='-2'> / </font><font size='-1'>")
                                htmlOut.Append("" & r.Item("aport_icao").ToString & "")
                                '  Else
                                '    htmlOut.Append("" & Replace(clsGeneral.clsGeneral.StripChars(Replace(Replace(r.Item("origin_aport_name").ToString, "Airport", ""), "International", ""), False), "Airport", ""))
                                '   End If


                                htmlOut.Append("</font></td>")
                                ' End If

                                htmlOut.Append("</tr>")
                            End If

                        Else
                            'If Trim(htmlOut.ToString.Trim) <> "" Then
                            '  htmlOut.Append(",")
                            'End If
                            'htmlOut.Append("{")
                            'htmlOut.Append("""check"": """",") 'Checkbox row.

                            '' the first one is origin, second is destination


                            'If ((searchCriteria.ViewCriteriaCompanyID > 0 Or Operator_IDS_String <> "") And Airport_ID_OVERALL <= 1 And Trim(Airport_IDS_String) = "" And initial = False) Or (Airport_IDS_String <> "" And initial = False) Or (Airport_ID_OVERALL > 1 And initial = False) Then


                            '  htmlOut.Append("""origin"":""")
                            '  htmlOut.Append("<a href='view_template.aspx?ViewID=28&ViewName=Operator/Airport Utilization&aport_id=" & r.Item("ffd_origin_aport_id").ToString & "&comp_id=" & searchCriteria.ViewCriteriaCompanyID & "'>")

                            '  htmlOut.Append("" & clsGeneral.clsGeneral.StripChars(Replace(Replace(r.Item("origin_aport_name").ToString, "Airport", ""), "International", ""), False) & "</a></br>")
                            '  htmlOut.Append("" & r.Item("origin_aport_iata_code").ToString & " / ")
                            '  htmlOut.Append("" & r.Item("origin_aport_icao_code").ToString & "")
                            '  htmlOut.Append(""",")
                            '  htmlOut.Append("""city2"":""" & r.Item("origin_aport_city").ToString & """,")
                            '  htmlOut.Append("""state2"":""" & r.Item("origin_aport_state").ToString & """,")
                            '  htmlOut.Append("""country2"":""" & r.Item("origin_aport_country").ToString & """,")

                            '  If (Airport_ID_OVERALL <= 1 And initial = False) Or (Airport_ID_OVERALL > 1 And airport_direction = "O" And initial = False) Then
                            '    If Airport_IDS_String <> "" Or (Airport_ID_OVERALL > 1 And airport_direction = "O" And initial = False) Or (Operator_IDS_String <> "" And initial = False) Or (Airport_ID_OVERALL <= 1 And searchCriteria.ViewCriteriaCompanyID > 0 And initial = False) Then

                            '      htmlOut.Append("""destination"":""")

                            '      htmlOut.Append("<a href='view_template.aspx?ViewID=28&ViewName=Operator/Airport Utilization&aport_id=" & r.Item("ffd_dest_aport_id").ToString & "&comp_id=" & searchCriteria.ViewCriteriaCompanyID & "'>")

                            '      htmlOut.Append("" & clsGeneral.clsGeneral.StripChars(Replace(Replace(r.Item("dest_aport_name").ToString, "Airport", ""), "International", ""), False) & "</a><br />")
                            '      htmlOut.Append(r.Item("dest_aport_iata_code").ToString & " / ")
                            '      htmlOut.Append(r.Item("dest_aport_icao_code").ToString)
                            '      htmlOut.Append(""",")
                            '      htmlOut.Append("""city"":""" & r.Item("dest_aport_city").ToString & """,")
                            '      htmlOut.Append("""state"":""" & r.Item("dest_aport_state").ToString & """,")
                            '      htmlOut.Append("""country"":""" & r.Item("dest_aport_country").ToString & """,")

                            '    End If
                            '  End If
                            '  htmlOut.Append("""flights"":""<A href='FAAFlightData.aspx?acid=0&aport_id1=" & r("ffd_origin_aport_id") & "&aport_id2=" & r("ffd_dest_aport_id") & "&orig_direction=1&pc=" & sSelectedProductCode & "&start_date=" & searchCriteria.ViewCriteriaDocumentsStartDate & "&end_date=" & searchCriteria.ViewCriteriaDocumentsEndDate & "' target='_blank' alt='Route Analysis' name='Route Analysis' title='Route Analysis'>" & FormatNumber(r("NbrFlights"), 0) & "</a>"",")

                            'Else
                            '  If Trim(airport_direction) = "D" Then
                            '    htmlOut.Append("""iata"":""<a href='view_template.aspx?ViewID=28&ViewName=Operator/Airport Utilization&aport_id=" & r.Item("ffd_dest_aport_id").ToString & "&comp_id=" & searchCriteria.ViewCriteriaCompanyID & "'>")
                            '    htmlOut.Append(r.Item("dest_aport_iata_code").ToString & "</a>"",")
                            '    htmlOut.Append("""icao"":""<a href='view_template.aspx?ViewID=28&ViewName=Operator/Airport Utilization&aport_id=" & r.Item("ffd_dest_aport_id").ToString & "&comp_id=" & searchCriteria.ViewCriteriaCompanyID & "'>" & r.Item("dest_aport_icao_code").ToString & "</a>"",")
                            '    htmlOut.Append("""airport"":""" & Replace(Replace(r.Item("dest_aport_name").ToString, "Airport", ""), "International", "") & """,")
                            '    htmlOut.Append("""city"":""" & r.Item("dest_aport_city").ToString & """,")
                            '    htmlOut.Append("""state"":""" & r.Item("dest_aport_state").ToString & """,")
                            '    htmlOut.Append("""country"":""" & r.Item("dest_aport_country").ToString & """,")
                            '    htmlOut.Append("""flights"":""" & FormatNumber(r("NbrFlights"), 0) & """,")
                            '  ElseIf Trim(airport_direction) = "O" Then
                            '    htmlOut.Append("""iata"":""<a href='view_template.aspx?ViewID=28&ViewName=Operator/Airport Utilization&aport_id=" & r.Item("ffd_origin_aport_id").ToString & "&comp_id=" & searchCriteria.ViewCriteriaCompanyID & "'>")
                            '    htmlOut.Append(r.Item("origin_aport_iata_code").ToString & "</a>"",")
                            '    htmlOut.Append("""icao"":""<a href='view_template.aspx?ViewID=28&ViewName=Operator/Airport Utilization&aport_id=" & r.Item("ffd_origin_aport_id").ToString & "&comp_id=" & searchCriteria.ViewCriteriaCompanyID & "'>" & r.Item("origin_aport_icao_code").ToString & "</a>"",")
                            '    htmlOut.Append("""airport"":""" & Replace(Replace(r.Item("origin_aport_name").ToString, "Airport", ""), "International", "") & """,")
                            '    htmlOut.Append("""city"":""" & r.Item("origin_aport_city").ToString & """,")
                            '    htmlOut.Append("""state"":""" & r.Item("origin_aport_state").ToString & """,")
                            '    htmlOut.Append("""country"":""" & r.Item("origin_aport_country").ToString & """,")
                            '    htmlOut.Append("""flights"":""" & FormatNumber(r("NbrFlights"), 0) & """,")
                            '  Else
                            '    htmlOut.Append("""iata"":""<a href='view_template.aspx?ViewID=28&ViewName=Operator/Airport Utilization&aport_id=" & r.Item("ffd_dest_aport_id").ToString & "&comp_id=" & searchCriteria.ViewCriteriaCompanyID & "'>")
                            '    htmlOut.Append(r.Item("dest_aport_iata_code").ToString & "</a>"",")
                            '    htmlOut.Append("""icao"":""<a href='view_template.aspx?ViewID=28&ViewName=Operator/Airport Utilization&aport_id=" & r.Item("ffd_dest_aport_id").ToString & "&comp_id=" & searchCriteria.ViewCriteriaCompanyID & "'>" & r.Item("dest_aport_icao_code").ToString & "</a>"",")
                            '    htmlOut.Append("""airport"":""" & Replace(Replace(r.Item("dest_aport_name").ToString, "Airport", ""), "International", "") & """,")
                            '    htmlOut.Append("""city"":""" & r.Item("dest_aport_city").ToString & """,")
                            '    htmlOut.Append("""state"":""" & r.Item("dest_aport_state").ToString & """,")
                            '    htmlOut.Append("""country"":""" & r.Item("dest_aport_country").ToString & """,")
                            '    htmlOut.Append("""flights"":""" & FormatNumber(r("NbrFlights"), 0) & """,")
                            '  End If
                            'End If





                            'htmlOut.Append("""hours"":""" & FormatNumber(r("TotalFlightTimeHrs"), 1) & """,")
                            'htmlOut.Append("""fuel"":""" & FormatNumber(r("TotalFuelBurn"), 0) & """")
                            'htmlOut.Append("}")

                        End If
                        tcount += 1
                    Next
                End If
            End If
            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "Utilization_functions.vb", "end fill array")


            If InStr(Trim(from_spot), "pdf") > 0 Then
                htmlOut.Append("</tbody></table></div></td></tr>")
            End If

        Catch ex As Exception

            aError = "Error in MostCommonOriginsJSArray(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByVal product_code_selection As String, ByRef table_count As Long)" + ex.Message

        Finally
            If InStr(Trim(from_spot), "pdf") > 0 Then
                Results = htmlOut.ToString.Trim
                out_htmlString = Results
            Else
                Results = " var mostCommonOriginsDataset = [ " & htmlOut.ToString.Trim & " ]; "
                out_htmlString = Results
            End If


        End Try

    End Sub


    Public Sub get_most_common_origins_top_function(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByVal product_code_selection As String, ByRef table_count As Long, ByVal airportTab As Boolean)

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False

        Try

            results_table = get_most_common_origins(searchCriteria, product_code_selection, False, airportTab, 0)


            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then
                    table_count = results_table.Rows.Count

                    htmlOut.Append("<table id='modelForsaleViewOuterTable' width=""100%"" cellpadding=""0"" cellspacing=""0"" class=""module"">")

                    If Trim(searchCriteria.ViewCriteriaCompanyID) = 0 And Trim(Operator_IDS_String) = "" Then
                        htmlOut.Append("<tr valign='top'><td valign='top' class='header' align='center'>Arrival Routes with Greater Than 2 Flights in the Last Year</td></tr>")
                    Else
                        htmlOut.Append("<tr valign='top'><td valign='top' class='header' align='center'>Operator Arrival Routes in the Last Year</td></tr>")
                    End If

                    htmlOut.Append("<tr><td align=""left"" valign=""top"">")

                    htmlOut.Append("<table id='tableCopy' cellpadding='0' cellspacing='0' border='0' align='left'><thead>")
                    htmlOut.Append(" <th>SEL</th>")

                    ' the first one is origin, second is destination
                    If searchCriteria.ViewCriteriaCompanyID > 0 And Airport_ID_OVERALL = 1 And Trim(Airport_IDS_String) = "" Then
                        htmlOut.Append("<th width='250'>Origin</th>")
                        ' htmlOut.Append("<th>IATA</th>")
                        '   htmlOut.Append("<th>ICAO</th>")
                        htmlOut.Append("<th width='250'>Destination</th>")
                        '  htmlOut.Append("<th>IATA</th>")
                        '  htmlOut.Append("<th>ICAO</th>")
                    Else
                        htmlOut.Append("<th>IATA</th>")
                        htmlOut.Append("<th>ICAO</th>")
                        htmlOut.Append("<th width='400'>Airport Name</th>")
                    End If

                    htmlOut.Append("<th>Nbr Flights</th>")
                    htmlOut.Append("<th>Total Flight Hrs</th>")
                    htmlOut.Append("<th><a href='#' title='Estimated Gallons of Fuel Burned' alt='Estimated Gallons of Fuel Burned'> Est. Fuel Burn (Gal)</a></th>")
                    htmlOut.Append("</thead><tbody>")


                    For Each r As DataRow In results_table.Rows

                        htmlOut.Append("<td></td>")



                        If searchCriteria.ViewCriteriaCompanyID > 0 And Airport_ID_OVERALL = 1 And Trim(Airport_IDS_String) = "" Then
                            htmlOut.Append("<td class=""text_align_left"" valign=""top"" width='250'>")
                            htmlOut.Append("" & r.Item("aport_name").ToString & " (" & Replace(r.Item("aport_country").ToString, "United States", "U.S.") & " - " & r.Item("aport_city").ToString & " " & r.Item("aport_state").ToString & ")")
                            ' htmlOut.Append("</td>")
                            'htmlOut.Append("<td class=""text_align_left"" valign=""top"" >")
                            htmlOut.Append(" - <a href='view_template.aspx?ViewID=28&ViewName=Operator/Airport Utilization&aport_id=" & r.Item("aport_id").ToString & "&comp_id=" & searchCriteria.ViewCriteriaCompanyID & "'>")
                            htmlOut.Append("" & r.Item("IATA").ToString & "</a> / ")
                            ' htmlOut.Append("<td class=""text_align_left"" valign=""top"" >")
                            htmlOut.Append("" & r.Item("ICAO").ToString & "")
                            htmlOut.Append("</td>")
                            htmlOut.Append("<td class=""text_align_left"" valign=""top"" width='250'>")
                            htmlOut.Append("" & r.Item("aport_name2").ToString & " (" & Replace(r.Item("aport_country2").ToString, "United States", "U.S.") & " - " & r.Item("aport_city2").ToString & " " & r.Item("aport_state2").ToString & ")")

                            ' htmlOut.Append("<td class=""text_align_left"" valign=""top"" >")
                            htmlOut.Append(" - <a href='view_template.aspx?ViewID=28&ViewName=Operator/Airport Utilization&aport_id=" & r.Item("aport_id2").ToString & "&comp_id=" & searchCriteria.ViewCriteriaCompanyID & "'>")
                            htmlOut.Append("" & r.Item("IATA2").ToString & "</a> / ")
                            ' htmlOut.Append("<td class=""text_align_left"" valign=""top"" >")
                            htmlOut.Append("" & r.Item("ICAO2").ToString & "")
                            htmlOut.Append("</td>")

                        Else
                            htmlOut.Append("<td class=""text_align_left"" valign=""top"" >")
                            htmlOut.Append("<a href='view_template.aspx?ViewID=28&ViewName=Operator/Airport Utilization&aport_id=" & r.Item("aport_id").ToString & "&comp_id=" & searchCriteria.ViewCriteriaCompanyID & "'>")
                            htmlOut.Append("" & r.Item("IATA").ToString & "</a></td>")
                            htmlOut.Append("<td class=""text_align_left"" valign=""top"" >")
                            htmlOut.Append("" & r.Item("ICAO").ToString & "</td>")

                            htmlOut.Append("<td class=""text_align_left"" valign=""top"" width='400'>")
                            htmlOut.Append("" & r.Item("aport_name").ToString & " (" & Replace(r.Item("aport_country").ToString, "United States", "U.S.") & " - " & r.Item("aport_city").ToString & " " & r.Item("aport_state").ToString & ")</td>")
                        End If

                        htmlOut.Append("<td class=""text_align_right"">" & FormatNumber(r("NbrFlights"), 0) & "</td>")
                        htmlOut.Append("<td class=""text_align_right"">" & FormatNumber(r("TotalFlightTimeHrs"), 1) & "</td>")
                        htmlOut.Append("<td class=""text_align_right"">" & FormatNumber(r("TotalFuelBurn"), 0) & "</td>")

                        htmlOut.Append("</tr>")

                    Next


                    htmlOut.Append("</tbody></table>")
                    htmlOut.Append("<div id=""forSaleInnerTable"" style=""width:930px;""></div>")
                    htmlOut.Append("</td></tr></table>")

                Else
                    htmlOut.Append("<table><tr><td valign=""top"" align=""left"" class=""seperator"" style=""padding-left:3px;"">No FAA Flight Activity Found.</td></tr></table>")
                End If
            Else
                htmlOut.Append("<table><tr><td valign=""top"" align=""left"" class=""seperator"" style=""padding-left:3px;"">No FAA Flight Activity Found.</td></tr></table>")
            End If



        Catch ex As Exception

            aError = "Error in views_display_fractions_to_expire(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub

    Public Sub get_most_common_destinations_top_function(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByVal product_code_selection As String)

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False

        Try
            '


            results_table = get_most_common_destinations(searchCriteria, product_code_selection)


            htmlOut.Append("<table id=""fractionsExpiringOuterTable"" width=""100%"" cellpadding=""0"" cellspacing=""0"" class=""module"">")
            htmlOut.Append("<tr><td valign=""top"" align=""center"" class=""header"">Top 25 Destinations (Last Year)</td></tr>")

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    htmlOut.Append("<tr><td valign=""top"" align=""left"">")

                    htmlOut.Append("<table id=""fractionsExpiringInnerTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"">")


                    htmlOut.Append("<tr><td colspan=""7"" class=""rightside"" valign=""top"">")
                    htmlOut.Append("<div style=""height: 250px; overflow: auto;"">")

                    htmlOut.Append("<table id=""fractionsExpiringDataTable"" width=""100%"" cellpadding=""3"" cellspacing=""0"">")
                    htmlOut.Append("<tr>")
                    'htmlOut.Append("<td valign=""top"" align=""left"" class=""seperator""><strong>Destin Airport</strong></td>")
                    htmlOut.Append("<td valign=""top"" align=""left"" class=""seperator""><strong>IATA</strong></td>")
                    htmlOut.Append("<td valign=""top"" align=""left"" class=""seperator""><strong>ICAO</strong></td>")
                    htmlOut.Append("<td valign=""top"" align=""left"" class=""seperator"" width=""55%""><strong>Airport Name</strong></td>")
                    htmlOut.Append("<td valign=""top"" align=""right"" class=""seperator""><strong>#&nbsp;Flights</strong></td></tr>")

                    For Each r As DataRow In results_table.Rows

                        If Not toggleRowColor Then
                            htmlOut.Append("<tr class=""alt_row"">")
                            toggleRowColor = True
                        Else
                            htmlOut.Append("<tr bgcolor=""white"">")
                            toggleRowColor = False
                        End If

                        '   htmlOut.Append("<td align=""left"" valign=""top"" class=""seperator"">")
                        '   htmlOut.Append("" & r.Item("DestinAPort").ToString & "</td>")
                        htmlOut.Append("<td align=""left"" valign=""top"" class=""seperator"">")
                        htmlOut.Append("<a href='view_template.aspx?ViewID=24&ViewName=Airport FBO View&aport_id=" & r.Item("aport_id").ToString & "'>")
                        htmlOut.Append("" & r.Item("IATA").ToString & "</a></td>")
                        htmlOut.Append("<td align=""left"" valign=""top"" class=""seperator"">")
                        htmlOut.Append("" & r.Item("ICAO").ToString & "</td>")
                        htmlOut.Append("<td align=""left"" valign=""top"" class=""seperator"">")
                        htmlOut.Append("" & r.Item("aport_name").ToString & " (" & r.Item("aport_country").ToString & " - " & r.Item("aport_city").ToString & " " & r.Item("aport_state").ToString & ")</td>")

                        htmlOut.Append("<td align=""right"" valign=""top"" class=""seperator"" width=""20%"" style=""padding-right:15px;"">" + r.Item("tflights").ToString + "</td></tr>")

                    Next

                    htmlOut.Append("</table></div></td></tr></table></td></tr>")
                Else
                    htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""seperator"" style=""padding-left:3px;"">No FAA Flight Activity Found.</td></tr>")
                End If
            Else
                htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""seperator"" style=""padding-left:3px;"">No FAA Flight Activity Found.</td></tr>")
            End If

            htmlOut.Append("</table>")

        Catch ex As Exception

            aError = "Error in views_display_fractions_to_expire(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub

    Public Sub get_nearby_airports_top_function(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByVal temp_distance As Integer, ByVal org_longitude As Double, ByVal org_latitude As Double, ByVal bus_type As String, ByVal aport_id As Long, ByVal use_controlled As Boolean, ByVal UpdateProgressPanel As UpdateProgress)

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False
        Dim range_text As String = ""

        Try


            results_table = get_nearby_airports(searchCriteria, temp_distance, org_longitude, org_latitude, use_controlled)

            range_text = "Change Range Miles from Currently Selected Airport to "
            range_text &= "<a href='view_template.aspx?ViewID=24&ViewName=Airport FBO View&aport_id=" & aport_id & "&distance=25&top_active_tab=4&bus_type=" & bus_type & "' onclick=""document.body.style.cursor='wait';$get('" & UpdateProgressPanel.ClientID & "').style.display = 'block';""><font color='white'><u>25</u></font></a>"
            range_text &= ", <a href='view_template.aspx?ViewID=24&ViewName=Airport FBO View&aport_id=" & aport_id & "&distance=50&top_active_tab=4&bus_type=" & bus_type & "' onclick=""document.body.style.cursor='wait';$get('" & UpdateProgressPanel.ClientID & "').style.display = 'block';""><font color='white'><u>50</u></font></a>"
            range_text &= ", <a href='view_template.aspx?ViewID=24&ViewName=Airport FBO View&aport_id=" & aport_id & "&distance=75&top_active_tab=4&bus_type=" & bus_type & "' onclick=""document.body.style.cursor='wait';$get('" & UpdateProgressPanel.ClientID & "').style.display = 'block';""><font color='white'><u>75</u></font></a>"
            range_text &= ", <a href='view_template.aspx?ViewID=24&ViewName=Airport FBO View&aport_id=" & aport_id & "&distance=100&top_active_tab=4&bus_type=" & bus_type & "' onclick=""document.body.style.cursor='wait';$get('" & UpdateProgressPanel.ClientID & "').style.display = 'block';""><font color='white'><u>100</u></font></a>"
            range_text &= ", <a href='view_template.aspx?ViewID=24&ViewName=Airport FBO View&aport_id=" & aport_id & "&distance=150&top_active_tab=4&bus_type=" & bus_type & "' onclick=""document.body.style.cursor='wait';$get('" & UpdateProgressPanel.ClientID & "').style.display = 'block';""><font color='white'><u>150</u></font></a>"
            range_text &= ", <a href='view_template.aspx?ViewID=24&ViewName=Airport FBO View&aport_id=" & aport_id & "&distance=200&top_active_tab=4&bus_type=" & bus_type & "' onclick=""document.body.style.cursor='wait';$get('" & UpdateProgressPanel.ClientID & "').style.display = 'block';""><font color='white'><u>200</u></font></a>&nbsp;"


            If Not IsNothing(results_table) Then

                htmlOut.Append("<table id=""fractionsExpiringOuterTable"" width=""100%"" cellpadding=""0"" cellspacing=""0"" class=""module"">")
                htmlOut.Append("<tr><td valign=""top"" align=""center"" class=""header"">" & results_table.Rows.Count & " Nearby Airports (Within " & temp_distance & " Miles)</td></tr>")

                htmlOut.Append("<tr><td valign=""top"" align=""right"" class=""header"">" & range_text & "</td></tr>")




                If results_table.Rows.Count > 0 Then

                    htmlOut.Append("<tr><td valign=""top"" align=""left"">")

                    htmlOut.Append("<table id=""fractionsExpiringInnerTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"">")

                    htmlOut.Append("<tr><td colspan=""2"" class=""rightside"" valign=""top"">")
                    '  htmlOut.Append("<div style=""height: 250px; overflow: auto;"">")

                    htmlOut.Append("<table id=""fractionsExpiringDataTable"" width=""100%"" cellpadding=""4"" cellspacing=""0"">")


                    For Each r As DataRow In results_table.Rows

                        If Not toggleRowColor Then
                            htmlOut.Append("<tr class=""alt_row"">")
                            toggleRowColor = True
                        Else
                            htmlOut.Append("<tr bgcolor=""white"">")
                            toggleRowColor = False
                        End If

                        htmlOut.Append("<td align=""left"" valign=""top"" class=""seperator"">")
                        htmlOut.Append("<a href='view_template.aspx?ViewID=24&ViewName=Airport FBO View&aport_id=" & r.Item("aport_id").ToString & "'  onclick=""document.body.style.cursor='wait';$get('" & UpdateProgressPanel.ClientID & "').style.display = 'block';"">")

                        htmlOut.Append("<b>" & r.Item("aport_name").ToString & "</b></a>")

                        If Not IsDBNull(r.Item("aport_iata_code")) Then
                            htmlOut.Append(", IATA:<i>" & r.Item("aport_iata_code").ToString & "</i>")
                        End If

                        If Not IsDBNull(r.Item("aport_icao_code")) Then
                            htmlOut.Append(", ICAO: <i>" & r.Item("aport_icao_code").ToString & "</i>")
                        End If

                        htmlOut.Append(" (" & r.Item("aport_country").ToString & " - " & r.Item("aport_city").ToString & " " & r.Item("aport_state").ToString & ")")

                        htmlOut.Append("</td>")
                        htmlOut.Append("</tr>")

                    Next

                    htmlOut.Append("</table></td></tr></table></td></tr>")
                Else
                    htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""seperator"" style=""padding-left:3px;"">No FAA Flight Activity Found.</td></tr>")
                End If
            Else
                htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""seperator"" style=""padding-left:3px;"">No FAA Flight Activity Found.</td></tr>")
            End If

            htmlOut.Append("</table>")

        Catch ex As Exception

            aError = "Error in views_display_fractions_to_expire(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub
    Public Sub FlightActivityJSArray(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByVal show_not_based As Boolean, ByVal product_code_selection As String, ByRef TotalFlights As Long, Optional ByVal from_spot As String = "", Optional ByVal table_color As String = "", Optional ByVal extra_title_text As String = "", Optional ByRef basedAtAirport As Boolean = False, Optional ByVal aport_id As Long = 0, Optional ByVal include_weight As String = "", Optional ByVal show_fuel_burn_in_liters As Boolean = False)

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False
        Dim Results As String = ""
        Dim tcount As Long = 1
        Dim limit_count As Integer = 0
        Dim temp_gal_lit As Integer = 0


        Try

            If Trim(from_spot) = "pdf2" Then
                limit_count = 41
            End If

            results_table = get_flight_activity_by_ac(searchCriteria, show_not_based, product_code_selection, TotalFlights, from_spot, basedAtAirport, limit_count, include_weight)

            If Not IsNothing(results_table) Then
                If results_table.Rows.Count > 0 Then

                    If Trim(from_spot) = "pdf2" Then
                        htmlOut.Append("<tr><td valign='top' align='left'><font class='" & HttpContext.Current.Session("FONT_CLASS_HEADER") & " mainHeading'><strong>" & Replace(searchCriteria.ViewCriteriaAirportName.ToString.Trim, " Airport", " ") & "</strong> TOP Aircraft" & extra_title_text & "</font></td></tr>")


                        htmlOut.Append("<tr><td valign='top'><div class=""Box""><table id=""weightTable"" cellpadding='3'  cellspacing='0' width='100%' class='formatTable " & table_color & "'><thead>")
                        htmlOut.Append("<tr class=""noBorder"">")
                        htmlOut.Append("<th>Aircraft</th>")
                        htmlOut.Append("<th>Ser#</th>")
                        htmlOut.Append("<th>Reg#</th>")
                        htmlOut.Append("<th>Current Operator</th>")

                        htmlOut.Append("<th class='right'><font size='-1'>NBR<br/>FLIGHTS</font></th>")
                        htmlOut.Append("<th class='right'>Dist(NM)</th>")
                        htmlOut.Append("<th class='right'><font size='-1'>Flight<br/>Time(Min)</font></th>")
                        If show_fuel_burn_in_liters = True Then
                            htmlOut.Append("<th class='right'><font size='-1'>EST FUEL<br/>BURN (L)</font></th>")
                        Else
                            htmlOut.Append("<th class='right'><font size='-1'>EST FUEL<br/>BURN (GAL)</font></th>")
                        End If

                        htmlOut.Append("</tr>")
                        htmlOut.Append("</thead>")
                        htmlOut.Append("<tbody>")
                    End If




                    For Each r As DataRow In results_table.Rows

                        If Trim(from_spot) = "pdf2" Then

                            If tcount > 39 Then
                                ' only show top 40
                            Else
                                htmlOut.Append("<tr>")
                                htmlOut.Append("<td><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>")
                                htmlOut.Append(r.Item("amod_make_name").ToString & " " & r.Item("amod_model_name").ToString)
                                htmlOut.Append("</font></td>")

                                htmlOut.Append("<td><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>")
                                htmlOut.Append(r.Item("ac_ser_no_full").ToString)
                                htmlOut.Append("</font></td>")

                                htmlOut.Append("<td><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>")
                                htmlOut.Append(r.Item("ac_reg_no").ToString)
                                htmlOut.Append("</font></td>")

                                htmlOut.Append("<td><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>")
                                If Not IsDBNull(r.Item("comp_name")) Then
                                    If InStr(r.Item("comp_name"), ",") > 0 And Len(Trim(r.Item("comp_name"))) > 45 Then
                                        htmlOut.Append(Replace(r.Item("comp_name").ToString, ",", "<br/>"))
                                    Else
                                        htmlOut.Append(r.Item("comp_name").ToString)
                                    End If
                                End If

                                htmlOut.Append("</font></td>")

                                htmlOut.Append("<td align='right'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>")
                                htmlOut.Append(FormatNumber(r("NbrFlights"), 0))
                                htmlOut.Append("</font></td>")

                                htmlOut.Append("<td align='right'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>")
                                htmlOut.Append(FormatNumber(flightDataFunctions.ConvertStatuteMileToNauticalMile(r.Item("Distance")), 0))
                                htmlOut.Append("</font></td>")

                                htmlOut.Append("<td align='right'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>")
                                htmlOut.Append(FormatNumber(r("TotalFlightTimeHrs"), 0))
                                htmlOut.Append("</font></td>")

                                htmlOut.Append("<td align='right'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>")

                                temp_gal_lit = FormatNumber(r("TotalFuelBurn"), 0)

                                If show_fuel_burn_in_liters = True Then
                                    temp_gal_lit = FormatNumber((temp_gal_lit * 3.78541), 0)
                                End If
                                htmlOut.Append(FormatNumber(temp_gal_lit, 0))

                                htmlOut.Append("</font></td>")

                                htmlOut.Append("</tr>")
                            End If

                            tcount = tcount + 1

                        Else


                            If Trim(htmlOut.ToString.Trim) <> "" Then
                                htmlOut.Append(",")
                            End If
                            htmlOut.Append("{")
                            htmlOut.Append("""check"": """",") 'Checkbox row.
                            htmlOut.Append("""ac"": """ & r.Item("amod_make_name").ToString & " " & r.Item("amod_model_name").ToString & """,")
                            If Not IsDBNull(r.Item("ac_ser_no_full")) Then
                                ' If r.Item("ac_ser_no_full") = "BLOCKED" Then
                                '   htmlOut.Append( """ser"": [""<span class='help_cursor' title='IDENTITY OF THIS AIRCRAFT IS BLOCKED BASED ON REQUEST OF OWNER/OPERATOR'>" & r.Item("ac_ser_no_full").ToString & "</span>"",""0""],")
                                '   htmlOut.Append( """reg"": ""<span class='help_cursor' title='IDENTITY OF THIS AIRCRAFT IS BLOCKED BASED ON REQUEST OF OWNER/OPERATOR'>" & r.Item("ac_reg_no").ToString & "</span>"",")
                                ' Else
                                htmlOut.Append((buildSerNoMenu(r.Item("ac_ser_no_full"), r.Item("ac_id"), r.Item("SERNOSORT_NONDISPLAY"), searchCriteria)))



                                htmlOut.Append("""reg"": """ & r.Item("ac_reg_no").ToString & """,")
                                '  End If 
                            End If


                            If Not IsDBNull(r.Item("base_aport_name")) Then
                                htmlOut.Append("""airport"": """ & r.Item("base_aport_name").ToString & """,")
                            Else
                                htmlOut.Append("""airport"": """",")
                            End If

                            If Not IsDBNull(r("NbrFlights")) Then
                                htmlOut.Append("""flights"": """ & FormatNumber(r("NbrFlights"), 0) & """,")
                            Else
                                htmlOut.Append("""flights"": ""0"",")
                            End If

                            If Not IsDBNull(r("TotalFlightTimeHrs")) Then
                                htmlOut.Append("""hours"": """ & FormatNumber(r("TotalFlightTimeHrs"), 1) & """,")
                            Else
                                htmlOut.Append("""hours"": ""0"",")
                            End If

                            If Not IsDBNull(r("TotalFuelBurn")) Then
                                htmlOut.Append("""fuel"": """ & FormatNumber(r("TotalFuelBurn"), 0) & """,")
                            Else
                                htmlOut.Append("""fuel"": ""0"",")
                            End If

                            If Not IsDBNull(r("AvgDistance")) Then
                                htmlOut.Append("""AvgDistance"": """ & FormatNumber(r("AvgDistance"), 0) & """,")
                            Else
                                htmlOut.Append("""AvgDistance"": ""0"",")
                            End If

                            If Not IsDBNull(r.Item("AvgMinPerFlights")) Then
                                htmlOut.Append("""AvgMinPerFlights"":""" & r.Item("AvgMinPerFlights").ToString & """,")
                            Else
                                htmlOut.Append("""AvgMinPerFlights"":""0"",")
                            End If

                            htmlOut.Append("""operator"": [""")

                            htmlOut.Append("<ul class='cssMenu'><li><a href='#' class='expand_more'>" & Replace(r("OPERATOR").ToString, "'", "") & "</a><ul>")
                            htmlOut.Append("<li><a class='underline' href='view_template.aspx?ViewID=28&ViewName=Fuel Utilization View&" & "aport_id=" & IIf(aport_id < 2, "0", aport_id) & "&" & "comp_id=" & r("comp_id") & "' title='Select Operator'>Select Operator</a></li>")
                            htmlOut.Append("<li><a href='#' onclick=\""javascript:load('DisplayCompanyDetail.aspx?compid=" & r("COMP_ID") & "','','scrollbars=yes,menubar=no,height=900,width=1090,resizable=yes,toolbar=no,location=no,status=no');return false;\"">View Operator Profile</a></li>")
                            htmlOut.Append("</ul></li></ul>")

                            If Not IsDBNull(r("OPERATOR")) Then
                                htmlOut.Append(""", """ & Replace(r("OPERATOR"), "'", "") & """], ")
                            Else
                                htmlOut.Append(""", """"], ")
                            End If


                            If Not IsDBNull(r("ADDRESS")) Then
                                htmlOut.Append("""address"": """ & Replace(r("ADDRESS"), "'", "") & """,")
                            Else
                                htmlOut.Append("""address"": ""&nbsp;"",")
                            End If
                            If Not IsDBNull(r("CITY")) Then
                                htmlOut.Append("""city"": """ & Replace(r("CITY"), "'", "") & """,")
                            Else
                                htmlOut.Append("""city"": ""&nbsp;"",")
                            End If

                            If Not IsDBNull(r("STATE")) Then
                                htmlOut.Append("""state"": """ & Replace(r("STATE"), "'", "") & """,")
                            Else
                                htmlOut.Append("""state"": ""&nbsp;"",")
                            End If

                            If Not IsDBNull(r("COUNTRY")) Then
                                htmlOut.Append("""country"": """ & Replace(r("COUNTRY"), "United States", "U.S.") & """,")
                            Else
                                htmlOut.Append("""country"": ""&nbsp;"",")
                            End If
                            If Not IsDBNull(r("EMAIL")) Then
                                htmlOut.Append("""email"": [""<a href='mailto:" & r("EMAIL") & "'>" & r("EMAIL") & "</a>"", """ & r("EMAIL") & """],")
                            Else
                                htmlOut.Append("""email"": ["""",""""],")
                            End If
                            If Not IsDBNull(r("WEB ADDRESS")) Then
                                htmlOut.Append("""web"": """ & r("WEB ADDRESS") & """,")
                            Else
                                htmlOut.Append("""web"": ""&nbsp;"",")
                            End If


                            If Not IsDBNull(r("OFFICE PHONE")) Then
                                htmlOut.Append("""office"": """ & r("OFFICE PHONE") & """,")
                            Else
                                htmlOut.Append("""office"": ""&nbsp;"",")
                            End If

                            If Not IsDBNull(r("amjiqs_cat_desc")) Then
                                htmlOut.Append("""SizeCategory"": """ & r("amjiqs_cat_desc") & """,")
                            Else
                                htmlOut.Append("""SizeCategory"": ""&nbsp;"",")
                            End If

                            If Not IsDBNull(r("cbus_name")) Then
                                htmlOut.Append("""BusinessType"": """ & r("cbus_name") & """")
                            Else
                                htmlOut.Append("""BusinessType"": ""&nbsp;""")
                            End If

                            htmlOut.Append("}")

                        End If
                    Next

                End If
            End If

            If Trim(from_spot) = "pdf2" Then
                htmlOut.Append("</tbody></table></div></td></tr>")
                Results = htmlOut.ToString.Trim
            Else
                Results = (" var acDataSet = [ " & htmlOut.ToString.Trim & " ]; ")
            End If


        Catch ex As Exception

            aError = "Error in get_flight_activity_by_ac_top_function(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

        Finally
            out_htmlString = Results
        End Try

    End Sub

    Public Function get_operater_piechart_info(ByRef searchCriteria As viewSelectionCriteriaClass, Optional ByVal use_faa_date As String = "") As DataTable
        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim start_date As String = ""
        Dim end_date As String = ""
        Dim mid_date As String = ""
        Dim mid_date2 As String = ""

        Dim temp_string As String = ""
        Dim start_temp_string As String = ""
        Dim type_temp As String = ""
        Dim tcompare1 As String = ""
        Dim tcompare2 As String = ""
        Dim bgcolor As String = ""

        Dim font_text_title As String = ""
        Dim font_text_start As String = ""
        Dim font_text_end As String = ""
        Dim temp_dir As String = "right"
        Dim high_number As Long = 0
        Dim low_number As Long = 100000
        Dim starting_point As Integer = 0
        Dim interval_point As Integer = 1
        Dim ending_point As Integer = 0

        Dim temp_date As String = ""
        Dim temp_date2 As String = ""
        Dim end_month As String = ""
        Dim start_month As String = ""
        Dim start_month_back As String = ""
        Dim end_month_back As String = ""
        Dim sqlWhere As String = ""


        Try

            If searchCriteria.ViewCriteriaAmodID > -1 Then

                sQuery.Append("SELECT DISTINCT comp_country AS ac_aport_country, count(*) AS modelCount")
                sQuery.Append(" FROM aircraft_summary WITH(NOLOCK)")
                sQuery.Append(" WHERE ac_lifecycle_stage = 3")
                sQuery.Append(" and comp_country is not null ")

                sQuery.Append(Constants.cAndClause + "(cref_operator_flag IN ('Y', 'O'))")

                If searchCriteria.ViewCriteriaCompanyID > 0 Then
                    sQuery.Append(Constants.cAndClause + "comp_id  = " + searchCriteria.ViewCriteriaCompanyID.ToString)
                End If

                If searchCriteria.ViewCriteriaAmodID > 0 Then
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

                sQuery.Append(" GROUP BY comp_country ORDER BY modelCount DESC")

            Else

                sQuery.Append("SELECT Case ISNULL(ac_aport_country,'') When '' then 'unknown' ELSE ac_aport_country END AS ac_aport_country, COUNT(*) AS modelCount")
                sQuery.Append(" FROM Aircraft WITH(NOLOCK) INNER JOIN Company WITH(NOLOCK) INNER JOIN Aircraft_Reference WITH(NOLOCK) ON comp_id = cref_comp_id AND")
                sQuery.Append(" comp_journ_id = cref_journ_id ON ac_journ_id = cref_journ_id AND ac_id = cref_ac_id INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id")
                sQuery.Append(" WHERE ac_journ_id = 0 AND ac_lifecycle_stage = 3 AND comp_active_flag = 'Y'")

                sQuery.Append(Constants.cAndClause + "(cref_operator_flag IN ('Y', 'O'))")

                If searchCriteria.ViewCriteriaCompanyID > 0 Then
                    sQuery.Append(Constants.cAndClause + "cref_comp_id  = " + searchCriteria.ViewCriteriaCompanyID.ToString)
                End If

                If searchCriteria.ViewCriteriaAmodID > -1 Then
                    sQuery.Append(Constants.cAndClause + "ac_amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
                ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
                    sQuery.Append(Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
                ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftType.Trim) Then
                    sQuery.Append(Constants.cAndClause + "amod_type_code = '" + searchCriteria.ViewCriteriaAircraftType.Trim + "'")
                End If


                If String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentsStartDate) And String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentsEndDate) Then
                    If Trim(use_faa_date) = "" Then
                        temp_date = DateAdd(DateInterval.Year, 0, Date.Now)
                    Else
                        temp_date = DateAdd(DateInterval.Year, 0, CDate(use_faa_date))
                    End If

                    Call get_past_dates(temp_date, start_month, start_month_back, end_month, end_month_back)

                    ' If go_back_farther = True Then
                    'sQuery.Append(" ffd_date >= ('" & start_month_back & "') ")
                    '  sQuery.Append(" and ffd_date <= ('" & DateAdd(DateInterval.Day, 1, CDate(end_month_back)) & "') ")
                    ' Else
                    sQuery.Append(" ffd_date >= ('" & start_month & "') ")
                    sQuery.Append(" and ffd_date <= ('" & end_month & "') ")
                    ' End If
                Else
                    sQuery.Append(" ffd_date >= ('" & searchCriteria.ViewCriteriaDocumentsStartDate & "') ")
                    sQuery.Append(" and ffd_date <= ('" & DateAdd(DateInterval.Day, 1, CDate(searchCriteria.ViewCriteriaDocumentsEndDate)) & "') ")
                End If

                If Trim(Operator_IDS_String) <> "" Then
                    If exclude_check = True Then
                        sQuery.Append(" and comp_id not ")
                    Else
                        sQuery.Append(" and comp_id ")
                    End If
                    sQuery.Append(" in (" & Operator_IDS_String & ") ")
                End If

                If Trim(searchCriteria.ViewCriteriaContinent) = "International" Then
                    sQuery.Append(" and (origin_aport_country not in ('United States', 'U.S.') or dest_aport_country not in ('United States', 'U.S.'))  ")
                End If

                If Not IsNothing(searchCriteria.ViewCriteriaTypeIDArray) Then
                    If searchCriteria.ViewCriteriaCompanyID > 0 Then
                        sQuery.Append(SetUpTypeString(searchCriteria))
                    Else
                        sQuery.Append(SetUpTypeString(searchCriteria))
                    End If
                End If

                If Trim(rollup_text) <> "" Then
                    sQuery.Append(rollup_text)
                ElseIf searchCriteria.ViewCriteriaCompanyID > 0 Then
                    sQuery.Append(" AND comp_id = " & searchCriteria.ViewCriteriaCompanyID & "")
                End If


                If Trim(distance_string) <> "" And Len(Trim(distance_string)) > 2 Then
                    sQuery.Append(distance_string)
                End If

                If Not String.IsNullOrEmpty(Trim(Aircraft_IDS_String)) Then
                    If exclude_Aircraft = True Then
                        sQuery.Append(" and ac_id not ")
                    Else
                        sQuery.Append(" and ac_id ")
                    End If
                    sQuery.Append(" in (" & Aircraft_IDS_String & ") ")
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

                sQuery.Append(" GROUP BY ac_aport_country ORDER BY modelCount DESC")

            End If

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_operater_piechart_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
                aError = "Error in get_operater_piechart_info load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_operater_piechart_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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


    Public Sub views_display_operator_piechart(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal field_name As String, ByRef out_htmlString As String, ByVal graphID As Integer, ByRef charting_string As String, ByVal use_faa_date As String, ByVal aport_id As Long, ByRef TotalFlights As Long, ByVal from_spot As String, Optional ByRef LimitThisQuery As Long = 0, Optional ByRef total_Flights As Long = 0)

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder
        Dim temp_label As String = ""
        Dim GoogleChart1TabScript As StringBuilder = New StringBuilder()

        Dim temp_string As String = ""
        Dim label_script As New Label
        Dim chart_label As New Label
        Dim string_from_charts As String = ""
        Dim row_added As Boolean = False
        Dim total_up_flights As Boolean = False

        If total_Flights = 1 Then
            total_up_flights = True
        End If


        Dim x As Integer = 0

        Try
            results_table = util_get_operator_pie_charts(searchCriteria, field_name, aport_id, TotalFlights, from_spot, LimitThisQuery)


            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    If Trim(field_name) = "cbus_name" Then
                        string_from_charts = string_from_charts & (" data" & graphID & ".addColumn('string', 'Business Type'); ")
                    ElseIf Trim(field_name) = "country_continent_name" Then
                        string_from_charts = string_from_charts & (" data" & graphID & ".addColumn('string', 'Continent'); ")
                    ElseIf Trim(field_name) = "comp_country" Then
                        string_from_charts = string_from_charts & (" data" & graphID & ".addColumn('string', 'Country'); ")
                    ElseIf Trim(field_name) = "comp_name" Then
                        string_from_charts = string_from_charts & (" data" & graphID & ".addColumn('string', 'Operator'); ")
                    ElseIf Trim(field_name) = "state_name" Then
                        string_from_charts = string_from_charts & (" data" & graphID & ".addColumn('string', 'State'); ")
                    Else
                        string_from_charts = string_from_charts & (" data" & graphID & ".addColumn('string', 'Country Name'); ")
                    End If

                    string_from_charts = string_from_charts & (" data" & graphID & ".addColumn('number', 'Value'); ")
                    string_from_charts = string_from_charts & (" data" & graphID & ".addRows([")


                    For Each r As DataRow In results_table.Rows

                        If Not IsDBNull(r.Item("NbrFlights")) Then
                            If Not String.IsNullOrEmpty(r.Item("NbrFlights").ToString.Trim) Then

                                temp_label = r.Item("" & field_name & "").ToString.Trim
                                temp_label = Replace(temp_label, "'", "")

                                If total_Flights > 1 And total_up_flights = False Then
                                    If row_added Then
                                        string_from_charts &= (",['" & temp_label & " (" & FormatNumber((r.Item("NbrFlights") / total_Flights) * 100, 0) & "%)'," & r.Item("NbrFlights").ToString & "]")
                                    Else
                                        string_from_charts &= ("['" & temp_label & " (" & FormatNumber((r.Item("NbrFlights") / total_Flights) * 100, 0) & "%)'," & r.Item("NbrFlights").ToString & "]")
                                    End If
                                    row_added = True
                                Else
                                    If row_added Then
                                        string_from_charts &= (",['" & temp_label & "'," & r.Item("NbrFlights").ToString & "]")
                                    Else
                                        string_from_charts &= ("['" & temp_label & "'," & r.Item("NbrFlights").ToString & "]")
                                    End If
                                    row_added = True
                                End If


                                If Trim(field_name) = "country_continent_name" And total_up_flights = True Then
                                    total_Flights = total_Flights + r.Item("NbrFlights")
                                End If
                                x += 1
                            End If
                        End If

                    Next

                    'string_from_charts &= ("]);")

                End If

            End If




            charting_string = string_from_charts



        Catch ex As Exception

            aError = "Error in vieews_display_operator_piechart(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByVal graphID As Integer) " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub
    Public Sub views_display_top_ac_piechart(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal field_name As String, ByVal graphID As Integer, ByRef htmlOut_return As String, ByVal aport_id As Long, ByVal selected_value As String, ByRef table_count As Long, ByVal product_code_selection As String, ByVal from_spot As String, ByRef airportTab As Boolean, Optional ByVal page_break_plus_header As String = "")

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder
        Dim temp_label As String = ""
        Dim GoogleChart1TabScript As StringBuilder = New StringBuilder()

        Dim temp_string As String = ""
        Dim label_script As New Label
        Dim chart_label As New Label
        Dim string_from_charts As String = ""
        Dim row_added As Boolean = False


        Dim x As Integer = 0

        Try

            results_table = get_flight_activity_by_ac_pie(searchCriteria, field_name, False, product_code_selection, 0, from_spot)

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    If Trim(field_name) = "amod_number_of_passengers" Then
                        string_from_charts = string_from_charts & (" data" & graphID & ".addColumn('string', 'Passengers'); ")
                    ElseIf Trim(field_name) = "amod_number_of_crew" Then
                        string_from_charts = string_from_charts & (" data" & graphID & ".addColumn('string', 'Crew'); ")
                    ElseIf Trim(field_name) = "ac_reg_no" Then
                        string_from_charts = string_from_charts & (" data" & graphID & ".addColumn('string', 'Aircraft'); ")
                    End If


                    string_from_charts = string_from_charts & (" data" & graphID & ".addColumn('number', 'Value'); ")
                    string_from_charts = string_from_charts & (" data" & graphID & ".addRows([")


                    For Each r As DataRow In results_table.Rows

                        If Not IsDBNull(r.Item("tflights")) Then
                            If Not String.IsNullOrEmpty(r.Item("tflights").ToString.Trim) Then

                                temp_label = r.Item("" & field_name & "").ToString.Trim
                                temp_label = Replace(temp_label, "'", "")

                                If Trim(temp_label) = "50" Then
                                    temp_label = "50+"
                                End If

                                If row_added Then
                                    string_from_charts &= (",['" & temp_label & "'," & r.Item("tflights").ToString & "]")
                                Else
                                    string_from_charts &= ("['" & temp_label & "'," & r.Item("tflights").ToString & "]")
                                End If
                                row_added = True

                                x += 1
                            End If
                        End If

                    Next

                End If

            End If




            htmlOut_return = string_from_charts



        Catch ex As Exception

            aError = "Error in vieews_display_operator_piechart(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByVal graphID As Integer) " + ex.Message

        Finally

        End Try

        'return resulting html string
        '  out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub
    Public Sub views_display_top_model_piechart(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal field_name As String, ByVal graphID As Integer, ByRef htmlOut_return As String, ByVal aport_id As Long, ByVal selected_value As String, ByRef table_count As Long, ByVal product_code_selection As String, ByVal from_spot As String, ByRef airportTab As Boolean, Optional ByVal page_break_plus_header As String = "")

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder
        Dim temp_label As String = ""
        Dim GoogleChart1TabScript As StringBuilder = New StringBuilder()

        Dim temp_string As String = ""
        Dim label_script As New Label
        Dim chart_label As New Label
        Dim string_from_charts As String = ""
        Dim row_added As Boolean = False


        Dim x As Integer = 0

        Try


            results_table = get_flight_activity_pie_by_model(searchCriteria, field_name, product_code_selection, from_spot, 0, False)

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    If Trim(field_name) = "acwgtcls_name" Then
                        string_from_charts = string_from_charts & (" data" & graphID & ".addColumn('string', 'Weight Class'); ")
                    ElseIf Trim(field_name) = "amjiqs_cat_desc" Then
                        string_from_charts = string_from_charts & (" data" & graphID & ".addColumn('string', 'Size Category'); ")
                    ElseIf Trim(field_name) = "amod_model_name" Then
                        string_from_charts = string_from_charts & (" data" & graphID & ".addColumn('string', 'Aircraft Model'); ")
                    End If


                    string_from_charts = string_from_charts & (" data" & graphID & ".addColumn('number', 'Value'); ")
                    string_from_charts = string_from_charts & (" data" & graphID & ".addRows([")


                    For Each r As DataRow In results_table.Rows

                        If Not IsDBNull(r.Item("tflights")) Then
                            If Not String.IsNullOrEmpty(r.Item("tflights").ToString.Trim) Then

                                temp_label = r.Item("" & field_name & "").ToString.Trim
                                temp_label = Replace(temp_label, "'", "")

                                If Trim(field_name) = "amod_model_name" Then
                                    temp_label = r.Item("amod_make_name") & " " & temp_label
                                End If


                                If row_added Then
                                    string_from_charts &= (",['" & temp_label & "'," & r.Item("tflights").ToString & "]")
                                Else
                                    string_from_charts &= ("['" & temp_label & "'," & r.Item("tflights").ToString & "]")
                                End If
                                row_added = True

                                x += 1
                            End If
                        End If

                    Next

                End If

            End If




            htmlOut_return = string_from_charts



        Catch ex As Exception

            aError = "Error in vieews_display_operator_piechart(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByVal graphID As Integer) " + ex.Message

        Finally

        End Try

        'return resulting html string
        '  out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub
    Public Sub views_display_routes_piechart(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal field_name As String, ByVal graphID As Integer, ByRef htmlOut_return As String, ByVal aport_id As Long, ByVal selected_value As String, ByRef table_count As Long, ByVal product_code_selection As String, ByVal from_spot As String, ByRef airportTab As Boolean, Optional ByVal page_break_plus_header As String = "")

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder
        Dim temp_label As String = ""
        Dim GoogleChart1TabScript As StringBuilder = New StringBuilder()

        Dim temp_string As String = ""
        Dim label_script As New Label
        Dim chart_label As New Label
        Dim string_from_charts As String = ""
        Dim row_added As Boolean = False


        Dim x As Integer = 0

        Try


            results_table = util_get_routes_locations(searchCriteria, field_name, product_code_selection, True, airportTab, 0, 0, product_code_selection, from_spot)

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    If Trim(field_name) = "aport_country" Then
                        string_from_charts = string_from_charts & (" data" & graphID & ".addColumn('string', 'Country Name'); ")
                    ElseIf Trim(field_name) = "country_continent_name" Then
                        string_from_charts = string_from_charts & (" data" & graphID & ".addColumn('string', 'Continent'); ")
                    ElseIf Trim(field_name) = "aport_icao_code" Then
                        string_from_charts = string_from_charts & (" data" & graphID & ".addColumn('string', 'Airport'); ")
                    ElseIf Trim(field_name) = "state_name" Then
                        string_from_charts = string_from_charts & (" data" & graphID & ".addColumn('string', 'State'); ")
                    End If


                    string_from_charts = string_from_charts & (" data" & graphID & ".addColumn('number', 'Value'); ")
                    string_from_charts = string_from_charts & (" data" & graphID & ".addRows([")


                    For Each r As DataRow In results_table.Rows

                        If Not IsDBNull(r.Item("tflights")) Then
                            If Not String.IsNullOrEmpty(r.Item("tflights").ToString.Trim) Then

                                temp_label = r.Item("" & field_name & "").ToString.Trim
                                temp_label = Replace(temp_label, "'", "")

                                If row_added Then
                                    string_from_charts &= (",['" & temp_label & "'," & r.Item("tflights").ToString & "]")
                                Else
                                    string_from_charts &= ("['" & temp_label & "'," & r.Item("tflights").ToString & "]")
                                End If
                                row_added = True


                                x += 1
                            End If
                        End If

                    Next

                End If

            End If




            htmlOut_return = string_from_charts




        Catch ex As Exception

            aError = "Error in vieews_display_operator_piechart(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByVal graphID As Integer) " + ex.Message

        Finally

        End Try

        'return resulting html string
        '  out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub

    Public Function buildSerNoMenu(ByVal serNoFull As String, ByVal acID As Long, ByVal serNoNonDisplay As String, ByVal searchCriteria As viewSelectionCriteriaClass, Optional ByVal Flight_Id1 As String = "", Optional ByVal Flight_Id2 As String = "", Optional ByVal Origin_Aport_id As Long = 0, Optional ByVal Dest_Aport_id As Long = 0) As String
        Dim returnString As String = ""
        returnString = """ser"": [""<ul class='cssMenu'><li><a href='#' class='expand_more'>" & serNoFull.ToString & "</a><ul>"
        returnString += "<li><a class='underline' onclick=\""javascript:load('DisplayAircraftDetail.aspx?acid=" + acID.ToString + "&jid=0','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');\"" title='Display Aircraft Details'><img src='/images/aircraftDetailsDropdown.jpg' width='100%' /></a></li>"
        returnString += "<li><a class='underline' onclick=\""javascript:load('FAAFlightData.aspx?acid=" + acID.ToString + "&jid=0"

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentsStartDate) And Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentsEndDate) Then
            returnString += "&start_date=" & Month(searchCriteria.ViewCriteriaDocumentsStartDate) & "/" & Day(searchCriteria.ViewCriteriaDocumentsStartDate) & "/" & Year(searchCriteria.ViewCriteriaDocumentsStartDate)
            returnString += "&end_date=" & Month(searchCriteria.ViewCriteriaDocumentsEndDate) & "/" & Day(searchCriteria.ViewCriteriaDocumentsEndDate) & "/" & Year(searchCriteria.ViewCriteriaDocumentsEndDate)
        End If


        returnString += "','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');\"" title='Display Aircraft Flight Activity'><img src='/images/flightActivityDropdown.jpg'  width='100%'/></a></li>"

        returnString += "<li><a class='underline' onclick=\""javascript:load('FAAFlightData.aspx?acid=" + acID.ToString + "&jid=0&Flight_Id1=" & Flight_Id1 & "&Flight_Id2=" & Flight_Id2 & "&Origin_Aport_id=" & Origin_Aport_id & "&Dest_Aport_id=" & Dest_Aport_id & "&activetab=10','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');\"" title='Display Route Map'><img src='/images/routeMapDropdown.jpg' width='100%' /></a></li>"

        returnString += "</ul></li></ul>"",""" & serNoNonDisplay.ToString & "</a>""],"
        Return returnString
    End Function

    Public Sub get_flight_activity_by_ac_top_function(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByVal show_not_based As Boolean, ByVal product_code_selection As String)

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False

        Try


            results_table = get_flight_activity_by_ac(searchCriteria, show_not_based, product_code_selection, 0)




            If Not IsNothing(results_table) Then
                If results_table.Rows.Count > 0 Then

                    htmlOut.Append("<table id='modelForsaleViewOuterTable' width=""100%"" cellpadding=""0"" cellspacing=""0"" class=""module"">")
                    If Trim(searchCriteria.ViewCriteriaCompanyID) = 0 And Trim(Operator_IDS_String) = "" Then
                        htmlOut.Append("<tr><td valign=""top"" align=""center"" class=""header"">Operator Aircraft with Greater Than 2 Flights in the Last Year</td></tr>")
                    Else
                        htmlOut.Append("<tr><td valign=""top"" align=""center"" class=""header"">Aircraft with Greater Than 2 Flights in the Last Year</td></tr>")
                    End If

                    htmlOut.Append("<tr><td align=""left"" valign=""top"">")

                    htmlOut.Append("<table id='tableCopy' width='100%' cellpadding='0' cellspacing='0' border='0' align='left'><thead>")

                    htmlOut.Append(" <th>SEL</th>")
                    htmlOut.Append("<th>Aircraft</th>")
                    htmlOut.Append("<th>Ser#</th>")
                    htmlOut.Append("<th>Reg#</th>")
                    htmlOut.Append("<th>Aircraft Based At</th>")
                    htmlOut.Append("<th>Nbr Flights</th>")
                    htmlOut.Append("<th>Total Flight Hrs</th>")
                    htmlOut.Append("<th><a href='' title='Estimated Gallons of Fuel Burned' alt='Estimated Gallons of Fuel Burned'> Est. Fuel Burn (Gal)</a></th>")

                    htmlOut.Append("</thead><tbody>")

                    For Each r As DataRow In results_table.Rows

                        htmlOut.Append("<td class=""text_align_center""></td>") ' for sel

                        htmlOut.Append("<td class=""text_align_left"">")
                        htmlOut.Append("" & r.Item("amod_make_name").ToString & " ")
                        htmlOut.Append("" & r.Item("amod_model_name").ToString & "")
                        htmlOut.Append("</td>")

                        htmlOut.Append("<td class=""text_align_left"">")
                        htmlOut.Append("<a class='underline' onclick=""javascript:load('DisplayAircraftDetail.aspx?acid=" + r.Item("ac_id").ToString + "&jid=0','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');"" title='Display Aircraft Details'>")
                        htmlOut.Append("" & r.Item("ac_ser_no_full").ToString & "</a> ")
                        htmlOut.Append("</td>")

                        htmlOut.Append("<td class=""text_align_left"">")
                        htmlOut.Append("" & r.Item("ac_reg_no").ToString & " ")
                        htmlOut.Append("</td>")


                        htmlOut.Append("<td class=""text_align_left"">" & r.Item("ac_aport_name").ToString & "</td>")

                        htmlOut.Append("<td class=""text_align_right"">" & FormatNumber(r("NbrFlights"), 0) & "</td>")
                        htmlOut.Append("<td class=""text_align_right"">" & FormatNumber(r("TotalFlightTimeHrs"), 1) & "</td>")
                        htmlOut.Append("<td class=""text_align_right"">" & FormatNumber(r("TotalFuelBurn"), 0) & "</td>")

                        htmlOut.Append("</tr>")

                    Next


                    htmlOut.Append("</tbody></table>")
                    htmlOut.Append("<div id=""forSaleInnerTable"" style=""width: 930px;""></div>")
                    htmlOut.Append("</td></tr></table>")
                Else
                    htmlOut.Append("<table><tr><td valign=""top"" align=""left"" class=""seperator"" style=""padding-left:3px;"">No FAA Flight Activity Found.</td></tr></table>")
                End If
            Else
                htmlOut.Append("<table><tr><td valign=""top"" align=""left"" class=""seperator"" style=""padding-left:3px;"">No FAA Flight Activity Found.</td></tr></table>")
            End If



        Catch ex As Exception

            aError = "Error in get_flight_activity_by_ac_top_function(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub
    Public Sub get_company_ownership_top_function(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByVal product_code_selection As String, ByVal from_spot As String, ByVal page_break_code As String)

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False
        Dim total As Integer = 0
        Dim total_op As Long = 0
        Dim total_lease As Long = 0
        Dim total_flights As Long = 0
        Dim total_hours As Long = 0
        Dim total_burn As Long = 0
        Dim util_link As String = ""
        Dim make_model_name As String = ""
        Dim temp_date As String = ""
        Dim temp_date2 As String = ""
        Dim font_text_title As String = ""
        Dim font_text_start As String = ""
        Dim font_text_end As String = ""
        Dim temp_dir As String = "right"
        Dim htmlOut_start As New StringBuilder
        Dim temp_count As Integer = 0
        Dim bgcolor As String = ""
        Dim temp_percent As Double = 0.0

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

            If Trim(from_spot) = "company" Then
                If use_operator = True Then
                    util_link = "&use_insight_op=Y"
                End If

                If use_owner = True Then
                    util_link = "&use_insight_own=Y"
                End If

                If Trim(rollup_text) <> "" Then
                    util_link = "&use_insight_roll=Y"
                End If
            End If


            results_table = get_company_ownership(searchCriteria, product_code_selection, from_spot)

            If Not IsNothing(results_table) Then
                If results_table.Rows.Count > 0 Then

                    If Trim(from_spot) = "pdf" Then
                        htmlOut_start.Append("<tr><td valign=""top"" align=""center"" class=""header"" colspan='12'><font class='" & HttpContext.Current.Session("FONT_CLASS_HEADER") & "'>Aircraft Ownership History</font></td></tr>")
                    Else
                        htmlOut_start.Append("<div class=""Box""><div class=""subHeader"">Aircraft Ownership History</div><br /><table id='modelForsaleViewOuterTable' width=""100%"" cellpadding=""2"" cellspacing=""0""  class='formatTable small blue'>")
                    End If


                    htmlOut_start.Append("<tr class='header_row noBorder'>")
                    htmlOut_start.Append("<th align='left'>" & font_text_title & "PURCHASED" & font_text_end & "</th>")
                    htmlOut_start.Append("<th align='left'>" & font_text_title & "PURCHASED&nbsp;FROM" & font_text_end & "</th>")
                    htmlOut_start.Append("<th align='left'>" & font_text_title & "MAKE&nbsp;MODEL" & font_text_end & "</th>")
                    htmlOut_start.Append("<th align='left'>" & font_text_title & "SERNO" & font_text_end & "</th>")
                    htmlOut_start.Append("<th align='left'>" & font_text_title & "SOLD&nbsp;ON" & font_text_end & "</th>")
                    htmlOut_start.Append("<th align='left'>" & font_text_title & "SOLD&nbsp;TO" & font_text_end & "</th>")

                    htmlOut_start.Append("</tr>")

                    htmlOut.Append(htmlOut_start.ToString)

                    ' htmlOut.Append("</thead><tbody>")

                    For Each r As DataRow In results_table.Rows


                        If Trim(from_spot) = "pdf" Then
                            If temp_count > 50 Then
                                htmlOut.Append("</table>")
                                htmlOut.Append(page_break_code)
                                htmlOut.Append(htmlOut_start.ToString)
                                temp_count = 0
                            End If
                            temp_count = temp_count + 1
                            If Not toggleRowColor Then
                                toggleRowColor = True
                                bgcolor = ""
                            Else
                                toggleRowColor = False
                                bgcolor = "#f0f0f0"
                            End If
                            htmlOut.Append("<tr bgcolor='" & bgcolor & "' valign='top'>")
                        Else
                            If Not toggleRowColor Then
                                htmlOut.Append("<tr class=""alt_row"" valign='top'>")
                                toggleRowColor = True
                            Else
                                htmlOut.Append("<tr bgcolor=""white"" valign='top'>")
                                toggleRowColor = False
                            End If
                        End If

                        If Not IsDBNull(r("purchased_date")) Then
                            temp_date = clsGeneral.clsGeneral.TwoPlaceYear(r("purchased_date"))
                            'temp_date2 = Left(Trim(temp_date), Len(Trim(temp_date)) - 4)
                            'temp_date = Right(Trim(temp_date), 4)
                            'temp_date = Right(Trim(temp_date), 2)
                            htmlOut.Append("<td align='" & temp_dir & "' class=""text_align_center"">" & font_text_start & "")
                            If Trim(from_spot) = "pdf" Then
                                htmlOut.Append("" & temp_date & "</td>")
                            Else
                                htmlOut.Append(DisplayFunctions.WriteDetailsLink(r("ac_id"), 0, 0, r("journ_id"), True, temp_date, "", ""))
                                htmlOut.Append("</td>")
                            End If


                        Else
                            htmlOut.Append("<td class=""text_align_left"">&nbsp;</td>")
                        End If

                        htmlOut.Append("<td align='" & temp_dir & "' class=""text_align_left"">" & font_text_start & "" & r("purchased_from") & "" & font_text_end & "</td>")

                        If Not IsDBNull(r.Item("cref_owner_percent")) Then
                            If Trim(r.Item("cref_owner_percent")) <> "" Then
                                If CDbl(r.Item("cref_owner_percent")) > 0 And CDbl(r.Item("cref_owner_percent")) < 99 Then
                                    temp_percent = r.Item("cref_owner_percent")
                                End If
                            End If
                        End If

                        If Trim(from_spot) = "pdf" Then
                            htmlOut.Append("<td align='" & temp_dir & "' class=""text_align_left"">" & font_text_start & "")
                            htmlOut.Append("" & r("amod_make_name") & " " & r.Item("amod_model_name").ToString)
                            If temp_percent > 0 Then
                                htmlOut.Append(" (" & CDbl(temp_percent) & "%)")
                            End If
                            htmlOut.Append("" & font_text_end & "</td>")
                        Else
                            htmlOut.Append("<td class=""text_align_left"">")
                            '  htmlOut.Append("<a class=""underline cursor"" href='DisplayCompanyDetail.aspx?compid=" + searchCriteria.ViewCriteriaCompanyID.ToString + "&amod_id=" + r.Item("amod_id").ToString & util_link & "' title='Show operator details for this make/model' onclick=""ChangeTheMouseCursorOnItemParentDocument('cursor_wait')"">")
                            htmlOut.Append("" & r("amod_make_name") & " " & r.Item("amod_model_name").ToString)
                            '   htmlOut.Append("</a>")
                            If temp_percent > 0 Then
                                htmlOut.Append(" (" & CDbl(temp_percent) & "%)")
                            End If
                            htmlOut.Append("</td>")
                        End If



                        htmlOut.Append("<td align='" & temp_dir & "' class=""text_align_left"">" & font_text_start & "" & r("ac_ser_no_full") & "" & font_text_end & "</td>")

                        If Not IsDBNull(r("Sold_on")) Then
                            temp_date = clsGeneral.clsGeneral.TwoPlaceYear(r("Sold_on"))
                            'temp_date2 = Left(Trim(temp_date), Len(Trim(temp_date)) - 4)
                            'temp_date = Right(Trim(temp_date), 4)
                            'temp_date = Right(Trim(temp_date), 2)
                            htmlOut.Append("<td align='" & temp_dir & "' class=""text_align_center"">" & font_text_start & "" & temp_date & "" & font_text_end & "</td>")
                        Else
                            htmlOut.Append("<td class=""text_align_center"">&nbsp;</td>")
                        End If
                        htmlOut.Append("<td class=""text_align_left"">" & font_text_start & "" & r("sold_to") & "" & font_text_end & "</td>")

                        htmlOut.Append("</tr>")

                    Next



                    htmlOut.Append("</table>")
                    If Trim(from_spot) <> "pdf" Then
                        htmlOut.Append("</div>")
                    End If

                Else
                    If Trim(from_spot) <> "pdf" Then
                        htmlOut.Append("<div class=""Box""><div class=""subHeader"">Aircraft Ownership History</div><br />")
                    End If
                    htmlOut.Append("<table><tr><td valign=""top"" align=""left"" class=""seperator"" style=""padding-left:3px;"">No FAA Flight Activity Found.</td></tr></table>")
                    If Trim(from_spot) <> "pdf" Then
                        htmlOut.Append("</div>")
                    End If
                End If
            Else
                If Trim(from_spot) <> "pdf" Then
                    htmlOut.Append("<div class=""Box""><div class=""subHeader"">Aircraft Ownership History</div>")
                End If
                htmlOut.Append("<table><tr><td valign=""top"" align=""left"" class=""seperator"" style=""padding-left:3px;"">No FAA Flight Activity Found.</td></tr></table>")
                If Trim(from_spot) <> "pdf" Then
                    htmlOut.Append("</div>")
                End If
            End If



        Catch ex As Exception

            aError = "Error in views_display_fractions_to_expire(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub
    Public Sub FlightActivityArrayByModel(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByVal product_code_selection As String, ByVal from_spot As String, ByRef TotalFlights As Long, ByVal temp_pdf_header As String, ByVal table_color As String, Optional ByVal extra_title_text As String = "", Optional ByVal show_fuel_burn_in_liters As Boolean = False)

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False
        Dim total As Integer = 0
        Dim total_op As Long = 0
        Dim total_lease As Long = 0
        Dim total_flights As Long = 0
        Dim total_hours As Long = 0
        Dim total_burn As Long = 0
        Dim util_link As String = ""
        Dim make_model_name As String = ""
        Dim Results As String = ""
        Dim tcount As Integer = 0
        Dim temp_gal_lit As Integer = 0

        Try


            results_table = get_flight_activity_by_model(searchCriteria, product_code_selection, from_spot, TotalFlights, False)

            If Not IsNothing(results_table) Then
                If results_table.Rows.Count > 0 Then


                    If Trim(from_spot) = "pdf2" Then
                        htmlOut.Append("<tr><td valign='top' align='left'><font class='" & HttpContext.Current.Session("FONT_CLASS_HEADER") & " mainHeading'><strong>" & Replace(searchCriteria.ViewCriteriaAirportName.ToString.Trim, " Airport", " ") & "</strong> TOP Models" & extra_title_text & "</font></td></tr>")

                        htmlOut.Append("<tr><td valign='top'><div class=""Box""><table id=""weightTable"" cellpadding='3'  cellspacing='0' width='100%' class='formatTable " & table_color & "'><thead>")
                        htmlOut.Append("<tr class=""noBorder"">")
                        htmlOut.Append("<th>MODEL</th>")
                        htmlOut.Append("<th class='right'>NBR<br/>FLIGHTS</th>")
                        htmlOut.Append("<th class='right'>TOTAL<br/>FLIGHT HOURS</th>")

                        If show_fuel_burn_in_liters = True Then
                            htmlOut.Append("<th class='right'>EST FUEL<br/>BURN (L)</th>")
                        Else
                            htmlOut.Append("<th class='right'>EST FUEL<br/>BURN (GAL)</th>")
                        End If

                        htmlOut.Append("</tr>")
                        htmlOut.Append("</thead>")
                        htmlOut.Append("<tbody>")
                    End If




                    For Each r As DataRow In results_table.Rows

                        If Trim(from_spot) = "pdf2" Then

                            If tcount > 38 Then
                                'tcount = 0
                                'htmlOut.Append("</table></td></tr></table>")
                                'htmlOut.Append(comp_functions.NEW_Insert_Page_Break_PDF(0, "pdf"))
                                'htmlOut.Append(temp_pdf_header)
                                ''  If bWordReport = True Then
                                ''  htmlOut.Append("<table width='" & word_width & "' align='center' cellpadding='3'>")
                                ''Else
                                'htmlOut.Append("<table width='95%' align='center' cellpadding='3'>")
                                ''  End If

                                'htmlOut.Append("<tr><td valign='top' align='center'><font class='" & HttpContext.Current.Session("FONT_CLASS_HEADER") & " mainHeading'><strong>" & searchCriteria.ViewCriteriaAirportName & "</strong> Models</font></td></tr>")

                                'htmlOut.Append("<tr><td valign='top'><div class=""Box""><table id=""weightTable"" cellpadding='3'  cellspacing='0' width='100%' class='formatTable " & table_color & "'><thead>")
                                'htmlOut.Append("<tr class=""noBorder"">")
                                'htmlOut.Append("<th>MODEL</th>")
                                'htmlOut.Append("<th>NBR FLIGHTS</th>")
                                'htmlOut.Append("<th>TOTAL FLIGHT HOURS</th>")
                                'htmlOut.Append("<th>EST FUEL BURN</th>")
                                'htmlOut.Append("</tr>")
                                'htmlOut.Append("</thead>")
                                'htmlOut.Append("<tbody>")
                            Else
                                htmlOut.Append("<tr>")
                                htmlOut.Append("<td><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>")
                                htmlOut.Append(r.Item("amod_make_name").ToString & " " & r.Item("amod_model_name").ToString)
                                htmlOut.Append("</font></td>")

                                htmlOut.Append("<td align='right'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>")
                                htmlOut.Append(FormatNumber(r("NbrFlights"), 0))
                                htmlOut.Append("</font></td>")

                                htmlOut.Append("<td align='right'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>")
                                htmlOut.Append(FormatNumber(r("TotalFlightTimeHrs"), 0))
                                htmlOut.Append("</font></td>")

                                htmlOut.Append("<td align='right'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>")

                                temp_gal_lit = FormatNumber(r("TotalFuelBurn"), 0)
                                If show_fuel_burn_in_liters = True Then
                                    temp_gal_lit = FormatNumber((temp_gal_lit * 3.78541), 0)
                                End If
                                htmlOut.Append(FormatNumber(temp_gal_lit, 0))


                                htmlOut.Append("</font></td>")

                                htmlOut.Append("</tr>")
                            End If



                        Else

                            If Results <> "" Then
                                Results += ","
                            End If
                            Results += "{"
                            Results += """check"": """"," 'Checkbox row.
                            Results += """model"": """ & r.Item("amod_make_name").ToString & " " & r.Item("amod_model_name").ToString & ""","
                            Results += vbNewLine
                            Results += """flights"":""" & FormatNumber(r("NbrFlights"), 0).ToString & ""","
                            Results += vbNewLine
                            Results += """hours"":""" & FormatNumber(r("TotalFlightTimeHrs"), 1).ToString & ""","
                            Results += vbNewLine
                            Results += """fuel"":""" & FormatNumber(r("TotalFuelBurn"), 0).ToString & ""","
                            Results += vbNewLine
                            Results += """AvgDistance"":""" & FormatNumber(r("AvgDistance"), 0).ToString & ""","
                            Results += vbNewLine

                            If Not IsDBNull(r("AvgMinPerFlights")) Then
                                Results += """AvgMinPerFlights"":""" & FormatNumber(r("AvgMinPerFlights"), 0).ToString & ""","
                            Else
                                Results += """AvgMinPerFlights"":""0"","
                            End If

                            If Not IsDBNull(r("amjiqs_cat_desc")) Then
                                Results += """SizeCategory"":""" & r("amjiqs_cat_desc") & ""","
                            Else
                                Results += """SizeCategory"":""0"","
                            End If

                            Results += vbNewLine
                            Results += "}"

                            If Not IsDBNull(r("NbrFlights")) Then
                                If IsNumeric(r("NbrFlights")) Then
                                    total_flights += r("NbrFlights")
                                End If
                            End If

                            If Not IsDBNull(r("TotalFlightTimeHrs")) Then
                                If IsNumeric(r("TotalFlightTimeHrs")) Then
                                    total_hours += r("TotalFlightTimeHrs")
                                End If
                            End If
                            If Not IsDBNull(r("TotalFuelBurn")) Then
                                If IsNumeric(r("TotalFuelBurn")) Then
                                    total_burn += r("TotalFuelBurn")
                                End If
                            End If

                        End If

                        tcount += 1
                    Next

                    'We're going to add a final total line to our array:
                    'Results += ",{"
                    'Results += """check"": """","
                    'Results += """model"": [""<strong>TOTALS</strong>""],"
                    'Results += vbNewLine
                    'Results += """flights"": """ & FormatNumber(total_flights, 0).ToString & ""","
                    'Results += vbNewLine
                    'Results += """hours"": """ & FormatNumber(total_hours, 0).ToString & ""","
                    'Results += vbNewLine
                    'Results += """fuel"": """ & FormatNumber(total_burn, 0).ToString & ""","
                    'Results += vbNewLine
                    'Results += "}"



                    'If searchCriteria.ViewCriteriaAmodID > 0 Then
                    '  htmlOut.Append("<tr class=""alt_row""><td>")
                    '  htmlOut.Append("<a class=""underline cursor"" href='DisplayCompanyDetail.aspx?compid=" + searchCriteria.ViewCriteriaCompanyID.ToString & util_link & "' title='Show operator details for this make/model' onclick=""ChangeTheMouseCursorOnItemParentDocument('cursor_wait')"">")
                    '  htmlOut.Append("Clear Model - " & Trim(make_model_name) & "")
                    '  htmlOut.Append("</a></td></tr>")
                    'Else
                    '  htmlOut.Append("<tr class=""alt_row"">")
                    '  htmlOut.Append("<td valign='top' align='right' class='border_bottom_right'><strong>Totals</strong></td>")
                    '  htmlOut.Append("<td align='right' class='border_bottom_right'><strong>" & FormatNumber(total_op, 0) & "</strong></td>")
                    '  htmlOut.Append("<td align='right' class='border_bottom_right'><strong>" & FormatNumber(total_lease, 0) & "</strong></td>")
                    '  htmlOut.Append("<td align='right' class='border_bottom_right'><strong>" & FormatNumber(total_flights, 0) & "</strong></td>")
                    '  htmlOut.Append("<td align='right' class='border_bottom_right'><strong>" & FormatNumber(total_hours, 0) & "</strong></td>")
                    '  htmlOut.Append("<td align='right' class='border_bottom_right'><strong>" & FormatNumber(total_burn, 0) & "</strong></td>")
                    '  htmlOut.Append("</tr>")
                    'End If

                    'If Trim(from_spot) = "company" Then
                    '  htmlOut.Append("</table>")
                    'Else
                    '  htmlOut.Append("</tbody></table>")
                    '  htmlOut.Append("<div id=""forSaleInnerTable"" style=""width: 930px;""></div>")
                    '  htmlOut.Append("</td></tr></table>")
                    'End If

                End If
            End If


            If Trim(from_spot) = "pdf2" Then
                htmlOut.Append("</tbody></table></div></td></tr>")
                Results = htmlOut.ToString.Trim
            Else

                Results = " var modelDataSet = [ " & Results & " ]; "
            End If




        Catch ex As Exception

            aError = "Error in FlightActivityArrayByModel(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByVal product_code_selection As String, ByVal from_spot As String) " + ex.Message

        Finally
            out_htmlString = Results
        End Try


    End Sub
    Public Sub get_flight_activity_by_model_top_function(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByVal product_code_selection As String, ByVal from_spot As String)

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False
        Dim total As Integer = 0
        Dim total_op As Long = 0
        Dim total_lease As Long = 0
        Dim total_flights As Long = 0
        Dim total_hours As Long = 0
        Dim total_burn As Long = 0
        Dim util_link As String = ""
        Dim make_model_name As String = ""
        Dim bgcolor As String = ""
        Dim font_text_title As String = ""
        Dim font_text_start As String = ""
        Dim font_text_end As String = ""
        Dim temp_dir As String = "right"

        If Trim(from_spot) = "pdf" Then
            font_text_start = "<font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>"
            font_text_title = "<font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT_TITLE") & "'>"
            font_text_end = "</font>"
            temp_dir = "right"
        Else
            font_text_start = ""
            font_text_title = ""
            font_text_end = ""
        End If

        Try


            If Trim(from_spot) = "company" Then
                If use_operator = True Then
                    util_link = "&use_insight_op=Y"
                End If

                If use_owner = True Then
                    util_link = "&use_insight_own=Y"
                End If

                If Trim(rollup_text) <> "" Then
                    util_link = "&use_insight_roll=Y"
                End If
            End If


            results_table = get_flight_activity_by_model(searchCriteria, product_code_selection, from_spot, 0, False)

            If Not IsNothing(results_table) Then
                If results_table.Rows.Count > 0 Then


                    If Trim(from_spot) = "pdf" Then
                        htmlOut.Append("<tr><td valign=""top"" align=""center"" class=""header"" colspan='12'><font class='" & HttpContext.Current.Session("FONT_CLASS_HEADER") & "'>MODELS OPERATED</font></td></tr>")
                        htmlOut.Append("<tr class='header_row'>")
                        htmlOut.Append("<th align='left'>" & font_text_title & "Make/Model" & font_text_end & "</th>")
                        htmlOut.Append("<th align='" & temp_dir & "'>" & font_text_title & "Operation" & font_text_end & "</th>")
                        htmlOut.Append("<th align='" & temp_dir & "'>" & font_text_title & "Leased" & font_text_end & "</th>")
                        htmlOut.Append("<th align='" & temp_dir & "'>" & font_text_title & "Nbr Flights" & font_text_end & "</th>")
                        htmlOut.Append("<th align='" & temp_dir & "'>" & font_text_title & "Total Flight Hrs" & font_text_end & "</th>")
                    ElseIf Trim(from_spot) = "company" Then
                        htmlOut.Append("<div class=""Box""><table id='modelForsaleViewOuterTable' width=""100%"" cellpadding=""1"" cellspacing=""0""  class='formatTable blue'>")
                        htmlOut.Append("<tr class='header_row'>")
                        htmlOut.Append("<th align='left'>" & font_text_title & "Make/Model" & font_text_end & "</th>")
                        htmlOut.Append("<th align='" & temp_dir & "'>" & font_text_title & "Operation" & font_text_end & "</th>")
                        htmlOut.Append("<th align='" & temp_dir & "'>" & font_text_title & "Leased" & font_text_end & "</th>")
                        htmlOut.Append("<th align='" & temp_dir & "'>" & font_text_title & "Nbr Flights" & font_text_end & "</th>")
                        htmlOut.Append("<th align='" & temp_dir & "'>" & font_text_title & "Total Flight Hrs" & font_text_end & "</th>")
                    Else
                        htmlOut.Append("<table id='modelForsaleViewOuterTable' width=""100%"" cellpadding=""0"" cellspacing=""0"" class=""module"">")
                        If Trim(searchCriteria.ViewCriteriaCompanyID) = 0 And Trim(Operator_IDS_String) = "" Then
                            htmlOut.Append("<tr><td valign=""top"" align=""center"" class=""header"">Flight Activity (Last " & searchCriteria.ViewCriteriaTimeSpan & " Months, Based on Arrivals)</td></tr>")
                        Else
                            htmlOut.Append("<tr><td valign=""top"" align=""center"" class=""header"">Models with Greater Than 2 Flights in the Last Year</td></tr>")
                        End If
                        htmlOut.Append("<tr><td align=""left"" valign=""top"">")
                        htmlOut.Append("<table id='tableCopy' cellpadding='0' cellspacing='0' border='0' align='left'><thead>")
                        htmlOut.Append(" <th>SEL</th>")
                        htmlOut.Append("<th>Make/Model</th>")
                        htmlOut.Append("<th>Nbr Flights</th>")
                        htmlOut.Append("<th>Total Flight Hrs</th>")
                    End If


                    If Trim(from_spot) = "company" Or Trim(from_spot) = "pdf" Then
                        htmlOut.Append("<th align='" & temp_dir & "'>" & font_text_title & "Est. Fuel<br/>Burn (Gal)" & font_text_end & "</th>")
                    Else
                        htmlOut.Append("<th><a href='' title='Estimated Gallons of Fuel Burned' alt='Estimated Gallons of Fuel Burned'> Est. Fuel Burn (Gal)</a></th>")
                    End If


                    htmlOut.Append("</thead><tbody>")

                    For Each r As DataRow In results_table.Rows

                        If Trim(from_spot) = "company" Or Trim(from_spot) = "pdf" Then

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
                                    htmlOut.Append("<tr class=""alt_row"">")
                                    toggleRowColor = True
                                Else
                                    htmlOut.Append("<tr bgcolor=""white"">")
                                    toggleRowColor = False
                                End If
                            End If


                            htmlOut.Append("<td align='left' class=""text_align_left"">" & font_text_start & "")
                            If Trim(from_spot) = "pdf" Then
                            Else
                                htmlOut.Append("<a class=""underline cursor"" href='DisplayCompanyDetail.aspx?compid=" + searchCriteria.ViewCriteriaCompanyID.ToString + "&amod_id=" + r.Item("amod_id").ToString & util_link & "' title='Show operator details for this make/model' onclick=""ChangeTheMouseCursorOnItemParentDocument('cursor_wait')"">")
                            End If
                            htmlOut.Append("" & r.Item("amod_make_name").ToString & " " & r.Item("amod_model_name").ToString)

                            If Trim(from_spot) = "pdf" Then
                                htmlOut.Append("" & font_text_end & "</td>")
                            Else
                                htmlOut.Append("</a></td>")
                            End If
                            htmlOut.Append("<td align='" & temp_dir & "' class=""text_align_right"">" & font_text_start & "" & FormatNumber(r("opcount"), 0) & "" & font_text_end & "</td>")
                            total_op = total_op + r("opcount")
                            htmlOut.Append("<td align='" & temp_dir & "' class=""text_align_right"">" & font_text_start & "" & FormatNumber(r("leasecount"), 0) & "" & font_text_end & "</td>")
                            total_lease = total_lease + r("leasecount")

                            If searchCriteria.ViewCriteriaAmodID > 0 Then
                                If Trim(make_model_name) = "" Then
                                    make_model_name = r.Item("amod_make_name").ToString & " " & r.Item("amod_model_name").ToString
                                End If
                            End If
                        Else
                            htmlOut.Append("<td class=""text_align_center""></td>")
                            htmlOut.Append("<td class=""text_align_center"">")
                            htmlOut.Append("" & r.Item("amod_make_name").ToString & " " & r.Item("amod_model_name").ToString)
                            htmlOut.Append("</td>")
                        End If




                        htmlOut.Append("<td align='" & temp_dir & "' class=""text_align_right"">" & font_text_start & "" & FormatNumber(r("NbrFlights"), 0) & "" & font_text_end & "</td>")
                        total_flights = total_flights + r("NbrFlights")
                        htmlOut.Append("<td align='" & temp_dir & "' class=""text_align_right"">" & font_text_start & "" & FormatNumber(r("TotalFlightTimeHrs"), 1) & "" & font_text_end & "</td>")
                        total_hours = total_hours + r("TotalFlightTimeHrs")
                        htmlOut.Append("<td align='" & temp_dir & "' class=""text_align_right"">" & font_text_start & "" & FormatNumber(r("TotalFuelBurn"), 0) & "" & font_text_end & "</td>")
                        total_burn = total_burn + r("TotalFuelBurn")
                        htmlOut.Append("</tr>")

                    Next

                    If searchCriteria.ViewCriteriaAmodID > 0 Then
                        htmlOut.Append("<tr class=""alt_row""><td>")
                        htmlOut.Append("<a class=""underline cursor"" href='DisplayCompanyDetail.aspx?compid=" + searchCriteria.ViewCriteriaCompanyID.ToString & util_link & "' title='Show operator details for this make/model' onclick=""ChangeTheMouseCursorOnItemParentDocument('cursor_wait')"">")
                        htmlOut.Append("Clear Model - " & Trim(make_model_name) & "")
                        htmlOut.Append("</a></td></tr>")
                    Else
                        htmlOut.Append("<tr class=""alt_row"">")
                        If Trim(from_spot) = "company" Then
                            htmlOut.Append("<td valign='top' align='" & temp_dir & "' class='border_bottom_right' nowrap='nowrap'><strong>" & font_text_title & "Totals (Last 6 Months)" & font_text_end & "</strong></td>")
                        Else
                            htmlOut.Append("<td valign='top' align='" & temp_dir & "' class='border_bottom_right'><strong>" & font_text_title & "Totals" & font_text_end & "</strong></td>")
                        End If

                        htmlOut.Append("<td align='" & temp_dir & "' class='border_bottom_" & temp_dir & "'><strong>" & font_text_title & "" & FormatNumber(total_op, 0) & "" & font_text_end & "</strong></td>")
                        htmlOut.Append("<td align='" & temp_dir & "' class='border_bottom_" & temp_dir & "'><strong>" & font_text_title & "" & FormatNumber(total_lease, 0) & "" & font_text_end & "</strong></td>")
                        htmlOut.Append("<td align='" & temp_dir & "' class='border_bottom_" & temp_dir & "'><strong>" & font_text_title & "" & FormatNumber(total_flights, 0) & "" & font_text_end & "</strong></td>")
                        htmlOut.Append("<td align='" & temp_dir & "' class='border_bottom_" & temp_dir & "'><strong>" & font_text_title & "" & FormatNumber(total_hours, 0) & "" & font_text_end & "</strong></td>")
                        htmlOut.Append("<td align='" & temp_dir & "' class='border_bottom_" & temp_dir & "'><strong>" & font_text_title & "" & FormatNumber(total_burn, 0) & "" & font_text_end & "</strong></td>")
                        htmlOut.Append("</tr>")
                    End If

                    If Trim(from_spot) = "company" Then
                        htmlOut.Append("</table></div>")
                    Else
                        htmlOut.Append("</tbody></table>")
                        htmlOut.Append("<div id=""forSaleInnerTable"" style=""width: 930px;""></div>")
                        htmlOut.Append("</td></tr></table>")
                    End If



                Else
                    htmlOut.Append("<table><tr><td valign=""top"" align=""left"" class=""seperator"" style=""padding-left:3px;"">No FAA Flight Activity Found.</td></tr></table>")
                End If
            Else
                htmlOut.Append("<table><tr><td valign=""top"" align=""left"" class=""seperator"" style=""padding-left:3px;"">No FAA Flight Activity Found.</td></tr></table>")
            End If



        Catch ex As Exception

            aError = "Error in views_display_fractions_to_expire(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub
    Public Sub GET_USER_AIRPORTS_top_function(ByVal user_airports_string As String, ByRef out_htmlString As String, ByVal UpdateProgressPanel As UpdateProgress, ByVal from_location As String, Optional ByVal selected_value As String = "365", Optional ByVal faa_Date As String = "")

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False
        Dim old_date As String = ""
        Dim mid_date As String = ""
        Dim temp_calc As Integer = 0
        Dim temp_percentage As Double = 0.0
        Dim temp_string As String = ""

        Try

            results_table = GET_USER_AIRPORTS(user_airports_string, selected_value, faa_Date)


            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    htmlOut.Append("<tr><td valign=""top"" align=""left"">")

                    htmlOut.Append("<table id=""fractionsExpiringInnerTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"">")

                    htmlOut.Append("<tr><td colspan=""2"" class=""rightside"" valign=""top"">")
                    '  htmlOut.Append("<div style=""height: 250px; overflow: auto;"">")

                    htmlOut.Append("<table id=""fractionsExpiringDataTable"" width=""100%"" cellpadding=""4"" cellspacing=""0"">")

                    htmlOut.Append("<tr><td valign=""top"" align=""left"" width=""74%"">&nbsp;</td>")
                    htmlOut.Append("<td valign=""top"" align=""center"" width=""26%"" colspan='2'><strong>#Arrivals</strong></td>")
                    htmlOut.Append("</tr>")

                    If Trim(faa_Date) <> "" Then

                        If Trim(selected_value) = "365" Or Trim(selected_value) = "" Then
                            old_date = DateAdd(DateInterval.Day, -730, CDate(faa_Date))
                            old_date = DateAdd(DateInterval.Day, 1, CDate(old_date))
                            mid_date = DateAdd(DateInterval.Year, -1, CDate(faa_Date))
                        Else 'its year to date 
                            old_date = Year(CDate(faa_Date))
                            old_date = "1/1/" & old_date ' first of the year this year
                            old_date = DateAdd(DateInterval.Year, -1, CDate(old_date))
                            mid_date = DateAdd(DateInterval.Year, -1, CDate(faa_Date))
                        End If

                        ' replace 2015 with 15, and so on 
                        old_date = Replace(old_date, Year(Date.Now), Right(Year(Date.Now), 2))
                        old_date = Replace(old_date, (Year(Date.Now) - 1), Right((Year(Date.Now) - 1), 2))
                        old_date = Replace(old_date, (Year(Date.Now) - 2), Right((Year(Date.Now) - 2), 2))

                        mid_date = Replace(mid_date, Year(Date.Now), Right(Year(Date.Now), 2))
                        mid_date = Replace(mid_date, (Year(Date.Now) - 1), Right((Year(Date.Now) - 1), 2))
                        mid_date = Replace(mid_date, (Year(Date.Now) - 2), Right((Year(Date.Now) - 2), 2))

                        htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""seperator"" width=""72%""><strong>My Airports</strong></td>")
                        htmlOut.Append("<td valign=""top"" align=""right"" class=""seperator"" width=""13%"" nowrap='nowrap'><strong>" & old_date & "-" & mid_date & "</strong></td>")

                        If Trim(selected_value) = "365" Or Trim(selected_value) = "" Then
                            mid_date = DateAdd(DateInterval.Day, 1, CDate(mid_date))
                        Else
                            mid_date = DateAdd(DateInterval.Year, 1, CDate(old_date))
                        End If

                        ' replace 2015 with 15, and so on 
                        mid_date = Replace(mid_date, Year(Date.Now), Right(Year(Date.Now), 2))
                        mid_date = Replace(mid_date, (Year(Date.Now) - 1), Right((Year(Date.Now) - 1), 2))
                        mid_date = Replace(mid_date, (Year(Date.Now) - 2), Right((Year(Date.Now) - 2), 2))

                        faa_Date = Replace(faa_Date, Year(Date.Now), Right(Year(Date.Now), 2))
                        faa_Date = Replace(faa_Date, (Year(Date.Now) - 1), Right((Year(Date.Now) - 1), 2))
                        faa_Date = Replace(faa_Date, (Year(Date.Now) - 2), Right((Year(Date.Now) - 2), 2))


                        htmlOut.Append("<td valign=""top"" align=""right"" class=""seperator"" width=""13%"" nowrap='nowrap'><strong>" & mid_date & "-" & faa_Date & "</strong></td>")
                        htmlOut.Append("<td valign=""top"" align=""right"" class=""seperator"" width=""2%""><strong>+/-</strong></td>")
                        htmlOut.Append("</tr>")
                    Else

                        htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""seperator"" width=""74%""><strong>My Airports</strong></td>")
                        htmlOut.Append("<td valign=""top"" align=""right"" class=""seperator"" width=""13%"" nowrap='nowrap'><strong>Previous Year</strong></td>")
                        htmlOut.Append("<td valign=""top"" align=""right"" class=""seperator"" width=""13%"" nowrap='nowrap'><strong>Current Year</strong></td>")
                        htmlOut.Append("<td valign=""top"" align=""right"" class=""seperator"" width=""2%""><strong>+/-</strong></td>")
                        htmlOut.Append("</tr>")
                    End If

                    For Each r As DataRow In results_table.Rows

                        If Not toggleRowColor Then
                            htmlOut.Append("<tr class=""alt_row"">")
                            toggleRowColor = True
                        Else
                            htmlOut.Append("<tr bgcolor=""white"">")
                            toggleRowColor = False
                        End If


                        htmlOut.Append("<td align=""left"" valign=""top"" class=""seperator"">")

                        If Trim(LCase(from_location)) = "view" Or HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME").ToString.ToUpper.Trim.Contains("JETNETTEST.COM") Then
                            If Not IsNothing(UpdateProgressPanel) Then
                                htmlOut.Append("<a href='view_template.aspx?ViewID=24&ViewName=Airport FBO View&aport_id=" & r.Item("APortId").ToString & "'  onclick=""document.body.style.cursor='wait';$get('" & UpdateProgressPanel.ClientID & "').style.display = 'block';"">")
                            Else
                                htmlOut.Append("<a href='view_template.aspx?ViewID=24&ViewName=Airport FBO View&aport_id=" & r.Item("APortId").ToString & "'  onclick=""document.body.style.cursor='wait';$get('').style.display = 'block';"">")
                            End If
                        End If


                        If Not IsDBNull(r.Item("APortName")) Then
                            htmlOut.Append("<b>" & r.Item("APortName").ToString & "</b>")
                        End If

                        If Trim(LCase(from_location)) = "view" Or HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME").ToString.ToUpper.Trim.Contains("JETNETTEST.COM") Then
                            htmlOut.Append("</a>")
                        End If

                        If Not IsDBNull(r.Item("IATACode")) And Not IsDBNull(r.Item("ICAOCode")) Then
                            htmlOut.Append(", ")
                        End If

                        If Not IsDBNull(r.Item("IATACode")) Then
                            htmlOut.Append("" & r.Item("IATACode").ToString & "")
                        End If

                        If Not IsDBNull(r.Item("IATACode")) And Not IsDBNull(r.Item("ICAOCode")) Then
                            htmlOut.Append("/")
                        End If

                        If Not IsDBNull(r.Item("ICAOCode")) Then
                            htmlOut.Append("" & r.Item("ICAOCode").ToString & "")
                        End If

                        If Not IsDBNull(r.Item("APortCountry")) Or Not IsDBNull(r.Item("APortCity")) Or Not IsDBNull(r.Item("APortState")) Then
                            htmlOut.Append(" (")
                        End If

                        If Not IsDBNull(r.Item("APortCity")) Then
                            htmlOut.Append("" & r.Item("APortCity").ToString & "")
                        End If

                        If Not IsDBNull(r.Item("APortState")) Then
                            htmlOut.Append(", " & r.Item("APortState").ToString)
                        End If

                        If Not IsDBNull(r.Item("APortCountry")) Then
                            temp_string = r.Item("APortCountry").ToString
                            temp_string = Replace(temp_string, "United States", "U.S.")
                            htmlOut.Append(" " & temp_string & "")
                        End If

                        If Not IsDBNull(r.Item("APortCountry")) Or Not IsDBNull(r.Item("APortCity")) Or Not IsDBNull(r.Item("APortState")) Then
                            htmlOut.Append(")")
                        End If
                        htmlOut.Append("</td>")

                        temp_calc = 0.0

                        If Not IsDBNull(r.Item("previousperiod")) Then
                            htmlOut.Append("<td align=""right"" valign=""top"" class=""seperator"" width=""13%"" style=""padding-right:2px;"">" & FormatNumber(r.Item("previousperiod").ToString, 0) & "</td>")
                        Else
                            htmlOut.Append("<td align=""right"" valign=""top"" class=""seperator"" width=""13%"" style=""padding-right:2px;"">&nbsp;</td>")
                        End If

                        If Not IsDBNull(r.Item("currentperiod")) Then
                            htmlOut.Append("<td align=""right"" valign=""top"" class=""seperator"" width=""13%"" style=""padding-right:2px;"">" & FormatNumber(r.Item("currentperiod").ToString, 0) & "</td>")
                        Else
                            htmlOut.Append("<td align=""right"" valign=""top"" class=""seperator"" width=""13%"" style=""padding-right:2px;"">&nbsp;</td>")
                        End If

                        If Not IsDBNull(r.Item("previousperiod")) And Not IsDBNull(r.Item("currentperiod")) Then

                            temp_calc = CInt(r.Item("currentperiod") - r.Item("previousperiod"))

                            htmlOut.Append("<td align=""right"" valign=""top"" class=""seperator"" width=""13%"" style=""padding-right:2px;"" nowrap='nowrap'>")


                            temp_percentage = 0.0

                            If temp_calc = 0 Then
                                htmlOut.Append("<img src='images/gain_loss_none.jpg' alt='' class='image_padding' />")
                            ElseIf temp_calc > 0 Then
                                htmlOut.Append("<img src='images/gain_loss_up.jpg' alt=''/>")
                                temp_percentage = CDbl((temp_calc / r.Item("currentperiod")) * 100)
                            ElseIf temp_calc < 0 Then
                                htmlOut.Append("<img src='images/gain_loss_down.jpg' alt='' />")
                                temp_percentage = CDbl((temp_calc / r.Item("currentperiod")) * 100)
                            End If

                            htmlOut.Append(temp_calc)
                            htmlOut.Append(" (" & FormatNumber(temp_percentage, 2) & "%)")
                            htmlOut.Append("</td>")
                        Else
                            htmlOut.Append("<td align=""right"" valign=""top"" class=""seperator"" width=""13%"" style=""padding-right:2px;"">N/A</td>")
                        End If

                        htmlOut.Append("</tr>")

                    Next

                    'htmlOut.Append("</table></div></td></tr></table></td></tr>")
                    htmlOut.Append("</table></td></tr></table></td></tr>")
                Else
                    htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""seperator"" style=""padding-left:3px;"">You have Not selected any Airports</td></tr>")
                End If
            Else
                htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""seperator"" style=""padding-left:3px;"">You have Not selected any Airports</td></tr>")
            End If

            htmlOut.Append("</table>")

        Catch ex As Exception

            aError = "Error in views_display_fractions_to_expire(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub


    Public Sub get_normal_ac_for_location_top_function(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByVal aclsData_Temp As clsData_Manager_SQL, ByVal city_name As String, ByRef AircraftSearchDataGrid As DataGrid, ByVal product_code_selection As String)

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder
        Dim htmlOut_Export As New StringBuilder
        Dim strOut As New StringBuilder
        Dim strOut_Export As New StringBuilder
        Dim toggleRowColor As Boolean = False
        Dim DisplayLink As Boolean = True
        Dim CRMViewActive As Boolean = False
        Dim JetnetViewData As New viewsDataLayer
        Dim font_shrink As String = ""
        Dim cellWidth As Integer = 20
        Dim sCompanyPhone As String = ""
        Dim arrFeatCodes() As String = Nothing
        Dim arrStdFeatCodes(,) As String = Nothing
        Dim is_word As Boolean = False
        Dim ActiveTabIndex As Integer = 0
        Dim start_text As String = ""
        Dim start_text_export As String = ""
        Dim page_break_after As Integer = 0


        Try


            JetnetViewData.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim

            results_table = get_normal_ac_for_location(searchCriteria, product_code_selection)

            Call fill_airport_view_data_grid(results_table, aclsData_Temp, AircraftSearchDataGrid)

            htmlOut.Append("<table id=""fractionsExpiringOuterTable"" width=""100%"" cellpadding=""5"" cellspacing=""0"" class=""module"">")
            htmlOut.Append("<tr><td valign=""top"" align=""center"" class=""header"">" & results_table.Rows.Count & " Aircraft Located at " & city_name & "</td></tr>")



            'If Not IsNothing(results_table) Then

            '  If results_table.Rows.Count > 0 Then

            '    htmlOut.Append("<tr><td valign=""top"" align=""left"">")

            '    htmlOut.Append("<table id=""fractionsExpiringInnerTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"">")


            '    htmlOut.Append("<tr><td colspan=""2"" class=""rightside"" valign=""top"">")
            '    htmlOut.Append("<div style=""height: 250px; overflow: auto;"">")



            '    If Not searchCriteria.ViewCriteriaIsReport Then
            '      strOut.Append("" & font_shrink & "&nbsp;&nbsp;&nbsp;&nbsp;AIRCRAFT FOR SALE&nbsp;<em>(" + results_table.Rows.Count.ToString + ")</em></td></tr>")
            '      strOut_Export.Append("" & font_shrink & "&nbsp;&nbsp;&nbsp;&nbsp;AIRCRAFT FOR SALE&nbsp;<em>(" + results_table.Rows.Count.ToString + ")</em></td></tr>")
            '    Else
            '      strOut.Append("" & font_shrink & "AIRCRAFT FOR SALE&nbsp;<em>(" + results_table.Rows.Count.ToString + ")</em></strong></td></tr>")
            '      strOut_Export.Append("" & font_shrink & "AIRCRAFT FOR SALE&nbsp;<em>(" + results_table.Rows.Count.ToString + ")</em></strong></td></tr>")
            '    End If

            '    If Not searchCriteria.ViewCriteriaIsReport Then

            '      If is_word Then
            '        If page_break_after > 0 Then
            '          htmlOut.Append("<table id='forSaleInnerTable' cellpadding='1' cellspacing='0' border='1' valign='top'><tr valign='top'>")
            '          htmlOut_Export.Append("<table id='forSaleInnerTable' cellpadding='1' cellspacing='0' border='1' valign='top'><tr valign='top'>")
            '        Else
            '          htmlOut.Append("<table id='forSaleInnerTable' cellpadding='1' cellspacing='0' border='0' valign='top'><tr valign='top'>")
            '          htmlOut_Export.Append("<table id='forSaleInnerTable' cellpadding='1' cellspacing='0' border='0' valign='top'><tr valign='top'>")
            '        End If
            '      Else
            '        If page_break_after > 0 Then
            '          htmlOut.Append("<table id='forSaleInnerTable' cellpadding='2' cellspacing='0' border='1' valign='top'><tr valign='top'>")
            '          htmlOut_Export.Append("<table id='forSaleInnerTable' cellpadding='2' cellspacing='0' border='1' valign='top'><tr valign='top'>")
            '        Else
            '          htmlOut.Append("<table id='forSaleInnerTable' cellpadding='2' cellspacing='0' border='0' valign='top'><tr valign='top'>")
            '          htmlOut_Export.Append("<table id='forSaleInnerTable' cellpadding='2' cellspacing='0' border='0' valign='top'><tr valign='top'>")
            '        End If
            '      End If


            '      'If DisplayLink Then
            '      '  htmlOut.Append("<td align='center' valign='middle' height='30px' class='forSaleCellBorder'>&nbsp;</td><td>&nbsp;</td>")
            '      'End If

            '      If DisplayLink Then
            '        If CRMViewActive Then
            '          htmlOut.Append("<td>&nbsp;</td>")
            '        End If

            '        If ((HttpContext.Current.Session.Item("localPreferences").HasServerNotes And Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("localPreferences").ServerNotesDatabaseName) Or CRMViewActive = True)) Then
            '          htmlOut.Append("<td>&nbsp;</td>") ' blue plus 
            '        End If
            '      End If

            '      If is_word Then
            '        htmlOut.Append("<td align='center' valign='middle' class='forSaleCellBorder'><strong>" & font_shrink & "SER<br />NUM</font></strong></td>")
            '        htmlOut_Export.Append("<td align='center' valign='middle' class='forSaleCellBorder'><strong>" & font_shrink & "SER<br />NUM</font></strong></td>")
            '      Else
            '        htmlOut.Append("<td align='center' valign='middle' class='forSaleCellBorder'><strong>" & font_shrink & "SERIAL<br />NUMBER</font></strong></td>")
            '        htmlOut_Export.Append("<td align='center' valign='middle' class='forSaleCellBorder'><strong>" & font_shrink & "SERIAL<br />NUMBER</font></strong></td>")
            '      End If


            '    Else

            '      If is_word Then
            '        If page_break_after > 0 Then
            '          htmlOut.Append("<table id='forSaleInnerTable' cellpadding='1' cellspacing='0' border='1'><tr>")
            '          htmlOut_Export.Append("<table id='forSaleInnerTable' cellpadding='1' cellspacing='0' border='1'><tr>")
            '        Else
            '          htmlOut.Append("<table id='forSaleInnerTable' cellpadding='1' cellspacing='0' border='0'><tr>")
            '          htmlOut_Export.Append("<table id='forSaleInnerTable' cellpadding='1' cellspacing='0' border='0'><tr>")
            '        End If
            '      Else
            '        If page_break_after > 0 Then
            '          htmlOut.Append("<table id='forSaleInnerTable' cellpadding='2' cellspacing='0' border='1'><tr>")
            '          htmlOut_Export.Append("<table id='forSaleInnerTable' cellpadding='2' cellspacing='0' border='1'><tr>")
            '        Else
            '          htmlOut.Append("<table id='forSaleInnerTable' cellpadding='2' cellspacing='0' border='0'><tr>")
            '          htmlOut_Export.Append("<table id='forSaleInnerTable' cellpadding='2' cellspacing='0' border='0'><tr>")
            '        End If
            '      End If



            '      htmlOut.Append("<tr>")
            '      htmlOut_Export.Append("<tr>")

            '      ' If DisplayLink Then
            '      '   htmlOut.Append("<td align='center' valign='middle' height='30px' class='forSaleCellBorder'>&nbsp;</td><td>&nbsp;</td>")
            '      'End If


            '      If is_word Then
            '        htmlOut.Append("<td align='center' valign='middle' height='30px' class='forSaleCellBorder'><strong>SER<br />NUM</strong></td>")
            '        htmlOut_Export.Append("<td align='center' valign='middle' height='30px' class='forSaleCellBorder'><strong>SER<br />NUM</strong></td>")
            '      Else
            '        htmlOut.Append("<td align='center' valign='middle' height='30px' class='forSaleCellBorder'><strong>SERIAL<br />NUMBER</strong></td>")
            '        htmlOut_Export.Append("<td align='center' valign='middle' height='30px' class='forSaleCellBorder'><strong>SERIAL<br />NUMBER</strong></td>")
            '      End If

            '    End If



            '    htmlOut.Append("<td align='center' valign='middle' class='forSaleCellBorder'><strong>" & font_shrink & "YEAR MFR</font></strong></td>")
            '    htmlOut.Append("<td align='center' valign='middle' class='forSaleCellBorder'><strong>" & font_shrink & "YEAR DLV</font></strong></td>")


            '    htmlOut_Export.Append("<td align='center' valign='middle' class='forSaleCellBorder'><strong>" & font_shrink & "YEAR MFR</font></strong></td>")
            '    htmlOut_Export.Append("<td align='center' valign='middle' class='forSaleCellBorder'><strong>" & font_shrink & "YEAR DLV</font></strong></td>")



            '    If Not searchCriteria.ViewCriteriaIsReport Then
            '      htmlOut.Append("<td align='center' valign='middle' class='forSaleCellBorder'><strong>" & font_shrink & "OWNER</font></strong></td>")
            '      htmlOut_Export.Append("<td align='center' valign='middle' class='forSaleCellBorder'><strong>" & font_shrink & "OWNER</font></strong></td>")

            '      If DisplayLink Then
            '        htmlOut.Append("<td align='center' valign='middle' class='forSaleCellBorder'><strong>BROKER</strong></td>")
            '        htmlOut_Export.Append("<td align='center' valign='middle' class='forSaleCellBorder'><strong>BROKER</strong></td>")
            '      End If
            '    Else
            '      htmlOut.Append("<td align='center' valign='middle' class='forSaleCellBorder'><strong>OWNER</strong></td>")
            '      htmlOut.Append("<td align='center' valign='middle' class='forSaleCellBorder'><strong>OWNERPHONE</strong></td>")
            '      htmlOut.Append("<td align='center' valign='middle' class='forSaleCellBorder'><strong>OPERATOR</strong></td>")
            '      htmlOut.Append("<td align='center' valign='middle' class='forSaleCellBorder'><strong>OPERATORPHONE</strong></td>")
            '      htmlOut.Append("<td align='center' valign='middle' class='forSaleCellBorder'><strong>BROKER</strong></td>")
            '      htmlOut.Append("<td align='center' valign='middle' class='forSaleCellBorder'><strong>BROKERPHONE</strong></td>")

            '      htmlOut_Export.Append("<td align='center' valign='middle' class='forSaleCellBorder'><strong>OWNER</strong></td>")
            '      htmlOut_Export.Append("<td align='center' valign='middle' class='forSaleCellBorder'><strong>OWNERPHONE</strong></td>")
            '      htmlOut_Export.Append("<td align='center' valign='middle' class='forSaleCellBorder'><strong>OPERATOR</strong></td>")
            '      htmlOut_Export.Append("<td align='center' valign='middle' class='forSaleCellBorder'><strong>OPERATORPHONE</strong></td>")
            '      htmlOut_Export.Append("<td align='center' valign='middle' class='forSaleCellBorder'><strong>BROKER</strong></td>")
            '      htmlOut_Export.Append("<td align='center' valign='middle' class='forSaleCellBorder'><strong>BROKERPHONE</strong></td>")
            '    End If

            '    htmlOut.Append("<td align='center' valign='middle' class='forSaleCellBorder'><strong>" & font_shrink & "ASKING</font></strong></td>")

            '    htmlOut_Export.Append("<td align='center' valign='middle' class='forSaleCellBorder'><strong>" & font_shrink & "ASKING</font></strong></td>")


            '    'Take Price Added
            '    If CRMViewActive Then
            '      htmlOut.Append("<td align='center' valign='middle' class='forSaleCellBorder'><strong>" & font_shrink & "TAKE PRICE</font></strong></td>")
            '      htmlOut.Append("<td align='center' valign='middle' class='forSaleCellBorder'><strong>" & font_shrink & "EST VALUE</font></strong></td>")

            '      htmlOut_Export.Append("<td align='center' valign='middle' class='forSaleCellBorder'><strong>" & font_shrink & "TAKE PRICE</font></strong></td>")
            '      htmlOut_Export.Append("<td align='center' valign='middle' class='forSaleCellBorder'><strong>" & font_shrink & "EST VALUE</font></strong></td>")
            '    End If

            '    htmlOut.Append("<td align='center' valign='middle' class='forSaleCellBorder'><strong>" & font_shrink & "DATE LISTED</font></strong></td>")
            '    htmlOut.Append("<td align='center' valign='middle' class='forSaleCellBorder'><strong>" & font_shrink & "AFTT</font></strong></td>")
            '    htmlOut.Append("<td align='center' valign='middle' class='forSaleCellBorder'><strong>" & font_shrink & "ENGINE&nbsp;TT</font></strong></td>")

            '    htmlOut_Export.Append("<td align='center' valign='middle' class='forSaleCellBorder'><strong>" & font_shrink & "DATE LISTED</font></strong></td>")
            '    htmlOut_Export.Append("<td align='center' valign='middle' class='forSaleCellBorder'><strong>" & font_shrink & "AFTT</font></strong></td>")
            '    htmlOut_Export.Append("<td align='center' valign='middle' class='forSaleCellBorder'><strong>" & font_shrink & "ENGINE&nbsp;TT</font></strong></td>")



            '    'If DisplayLink Then
            '    '  htmlOut.Append("<td align='center' valign='middle' class='forSaleCellBorder'><strong>FEATURES</strong><br />")
            '    '  htmlOut_Export.Append("<td align='center' valign='middle' class='forSaleCellBorder'><strong>FEATURES</strong><br />")


            '    '  htmlOut.Append("<table id='featureHeadingTable' width='100%' cellpadding='1' cellspacing='0' border='0'><tr>")
            '    '  htmlOut_Export.Append("<table id='featureHeadingTable' width='100%' cellpadding='1' cellspacing='0' border='0'><tr>")

            '    '  JetnetViewData.load_standard_ac_features(searchCriteria, arrStdFeatCodes)

            '    '  Dim sNonStandardAcFeature As String = ""
            '    '  JetnetViewData.display_nonstandard_feature_code_headings(searchCriteria, arrFeatCodes, arrStdFeatCodes, cellWidth, sNonStandardAcFeature)

            '    '  htmlOut.Append(sNonStandardAcFeature + "</tr></table>")
            '    '  htmlOut_Export.Append(sNonStandardAcFeature + "</tr></table>")

            '    '  htmlOut.Append("</td>")
            '    '  htmlOut_Export.Append("</td>")
            '    'End If


            '    htmlOut.Append("<td align='center' valign='middle' class='forSaleCellBorder' title='Number Of Passengers'><strong>" & font_shrink & "PAX</font></strong></td>")
            '    htmlOut.Append("<td align='center' valign='middle' class='forSaleCellBorder'><strong>" & font_shrink & "INT<br />YEAR</font></strong></td>")

            '    htmlOut_Export.Append("<td align='center' valign='middle' class='forSaleCellBorder' title='Number Of Passengers'><strong>" & font_shrink & "PAX</font></strong></td>")
            '    htmlOut_Export.Append("<td align='center' valign='middle' class='forSaleCellBorder'><strong>" & font_shrink & "INT<br />YEAR</font></strong></td>")


            '    If (HttpContext.Current.Session.Item("localPreferences").HasServerNotes And Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("localPreferences").ServerNotesDatabaseName)) Then
            '      htmlOut.Append("<td align='center' valign='middle' class='forSaleCellBorder'><strong>" & font_shrink & "EXT<br />YEAR</font></strong></td>")
            '      htmlOut_Export.Append("<td align='center' valign='middle' class='forSaleCellBorder'><strong>" & font_shrink & "EXT<br />YEAR</font></strong></td>")
            '    Else
            '      htmlOut.Append("<td align='center' valign='middle' class='forSaleCellBorderNoNotes'><strong>" & font_shrink & "EXT<br />YEAR</font></strong></td>")
            '      htmlOut_Export.Append("<td align='center' valign='middle' class='forSaleCellBorderNoNotes'><strong>" & font_shrink & "EXT<br />YEAR</font></strong></td>")
            '    End If

            '    If DisplayLink Then
            '      If Not searchCriteria.ViewCriteriaIsReport Then
            '        If ((HttpContext.Current.Session.Item("localPreferences").HasServerNotes And Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("localPreferences").ServerNotesDatabaseName)) Or CRMViewActive = True) Then
            '          htmlOut.Append("<td align='center' valign='middle' class='forSaleCellBorder'><strong>NOTES</strong>")
            '          htmlOut.Append("</td>")
            '        End If
            '      Else
            '        htmlOut.Append("<td align='center' valign='middle' class='forSaleCellBorder'><strong>NOTES</strong>")
            '        htmlOut.Append("</td>")
            '      End If
            '    End If



            '    htmlOut.Append("</tr>")
            '    htmlOut_Export.Append("</tr>")

            '    start_text = htmlOut.ToString
            '    start_text_export = htmlOut_Export.ToString





            '    For Each r As DataRow In results_table.Rows


            '      '---------------------------- TAKEN FROM FOR SALE ITEMS -------------------
            '      If Not toggleRowColor Then
            '        htmlOut.Append("<tr class='alt_row'>")
            '        htmlOut_Export.Append("<tr class='alt_row'>")
            '        toggleRowColor = True
            '      Else
            '        htmlOut.Append("<tr bgcolor='white'>")
            '        htmlOut_Export.Append("<tr bgcolor='white'>")
            '        toggleRowColor = False
            '      End If

            '      If DisplayLink Then
            '        '  htmlOut.Append("<td align='center' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>" + IIf(r.Item("source").ToString = "JETNET", "<img src='images/evo.png' alt='JETNET RECORD' width='15' />", "<img src='images/client.png' alt='CLIENT RECORD' width='15' />") + "</td>")
            '        '  htmlOut.Append("<td align='center' valign='middle' class='forSaleCellBorder' nowrap='nowrap'><img src='images/evo.png' alt='JETNET RECORD' width='15' /></td>")


            '        If (searchCriteria.ViewCriteriaNoLocalNotes = False And Not searchCriteria.ViewCriteriaIsReport) Then

            '          htmlOut.Append("<td align='center' valign='middle' class='forSaleCellBorder'>")  ' Note ICON
            '          htmlOut.Append("<div style='visibility: hidden;' id='bHasNotesGif" + r.Item("ac_id").ToString + "ID'><a href='javascript:displayLocalAircraftNoteJS(" + r.Item("ac_id").ToString + ",0,0);'><img src='images/Notes.gif' border='0'></a></div>")
            '          htmlOut.Append("</td>")

            '        ElseIf (HttpContext.Current.Session.Item("localPreferences").HasServerNotes And Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("localPreferences").ServerNotesDatabaseName) And Not searchCriteria.ViewCriteriaIsReport) Then

            '          htmlOut.Append("<td align='center' valign='middle' class='forSaleCellBorder'>")  ' Note ICON
            '          htmlOut.Append("<div style='visibility: hidden;' id='bHasNotesGif" + r.Item("ac_id").ToString + "ID'><a class='underline' onclick='javascript:callNoteViewImg" + r.Item("ac_id").ToString + "();'><img src='images/Notes.gif' border='0'></a></div>")
            '          htmlOut.Append("</td>")

            '        Else

            '          ' If Not searchCriteria.ViewCriteriaIsReport Then
            '          'htmlOut.Append("<td align='center' valign='middle' class='forSaleCellBorder'>&nbsp;")  ' NO NOTES No Note ICON
            '          '  htmlOut.Append("</td>")
            '          ' End If

            '        End If
            '      End If

            '      If DisplayLink Then
            '        If CRMViewActive Then
            '          htmlOut.Append("<td>")


            '          ' htmlOut.Append("<a href='#' onclick=""window.open('/edit.aspx?action=edit&type=aircraft&ac_ID=" & r.Item("ac_id") & "&source=" & r.Item("source").ToString & "&from=view&viewNOTEID=0&activetab=1','viewAnalysis','scrollbars=yes,menubar=no,height=900,width=1030,resizable=yes,toolbar=no,location=no,status=no');return false;"">")
            '          htmlOut.Append("<a href='#' onclick=""window.open('/edit.aspx?action=edit&type=aircraft&ac_ID=" & r.Item("ac_id") & "&source=JETNET&from=view&viewNOTEID=0&activetab=1','viewAnalysis','scrollbars=yes,menubar=no,height=900,width=1030,resizable=yes,toolbar=no,location=no,status=no');return false;"">")


            '          htmlOut.Append("<img src='images/edit_icon.png' alt='Edit Aircraft' title='Edit Aircraft'>")
            '          htmlOut.Append("</a>")
            '          htmlOut.Append("</td>")
            '        End If
            '      End If

            '      ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '      ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '      'OWNER LOOKUP MOVED TO BEFORE NOTES ICON SO QUERY HAD TO BE DONE ONLY ONCE.
            '      searchCriteria.ViewCriteriaGetExclusive = False
            '      searchCriteria.ViewCriteriaGetOperator = False

            '      Dim ownerDataTable As New DataTable

            '      'Select Case UCase(r("source").ToString)
            '      '   Case "JETNET"
            '      searchCriteria.ViewCriteriaAircraftID = r.Item("ac_id")
            '      ownerDataTable = JetnetViewData.get_owner_info(searchCriteria)
            '      '   Case "CLIENT"
            '      ' ownerDataTable = crmViewDataLayer.Get_Client_Owner_Info(searchCriteria)
            '      '  End Select


            '      If DisplayLink Then

            '        If ((HttpContext.Current.Session.Item("localPreferences").HasServerNotes And Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("localPreferences").ServerNotesDatabaseName) Or CRMViewActive = True)) Then

            '          htmlOut.Append("<td align='center' valign='middle' class='forSaleCellBorder'>") ' NOTE ADD 
            '          If Not IsNothing(ownerDataTable) Then
            '            If ownerDataTable.Rows.Count > 0 Then
            '              Dim TemporaryCompanyID As Long = 0
            '              Dim CheckNoteTable As New DataTable

            '              htmlOut.Append("<a href='#' class='no_text_underline' onclick=""javascript:load('edit.aspx?prospectACID=" & IIf(r("client_jetnet_ac_id") > 0, r("client_jetnet_ac_id"), r("ac_id")) & "&comp_ID=")

            '              'Need to send jetnet company ID
            '              ' If UCase(r("source")) = "JETNET" Then
            '              htmlOut.Append(ownerDataTable.Rows(0).Item("comp_id"))
            '              TemporaryCompanyID = ownerDataTable.Rows(0).Item("comp_id")
            '              'Else
            '              '  htmlOut.Append(ownerDataTable.Rows(0).Item("clicomp_jetnet_comp_id"))
            '              '   TemporaryCompanyID = ownerDataTable.Rows(0).Item("clicomp_jetnet_comp_id")
            '              '  End If

            '              htmlOut.Append("&source=JETNET&type=company&action=checkforcreation&note_type=A&from=view&rememberTab=" & ActiveTabIndex & "&returnView=" & searchCriteria.ViewID & "','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');"">")


            '              CheckNoteTable = crmViewDataLayer.Check_For_Applicable_Notes_LIMIT_CRM("COMP_AC", TemporaryCompanyID, IIf(r("client_jetnet_ac_id") > 0, r("client_jetnet_ac_id"), r("ac_id")), "JETNET", "", 1, HttpContext.Current.Application.Item("crmClientDatabase"))
            '              If Not IsNothing(CheckNoteTable) Then
            '                If CheckNoteTable.Rows.Count > 0 Then
            '                  If CheckNoteTable.Rows(0).Item("lnote_status") = "A" Then
            '                    htmlOut.Append("<img src='images/note_pin_add.png' width='16' title='" & CheckNoteTable.Rows(0).Item("lnote_entry_date") & " - " & CheckNoteTable.Rows(0).Item("lnote_note") & "'>")
            '                  Else
            '                    htmlOut.Append("<img src='images/blue_plus_sign.png' width='16'>")
            '                  End If

            '                Else
            '                  htmlOut.Append("<img src='images/blue_plus_sign.png' width='16'>")
            '                End If
            '              End If

            '              htmlOut.Append("</a>")
            '            Else
            '              Dim CheckNoteTable As New DataTable

            '              htmlOut.Append("<a href='#' class='no_text_underline' onclick=""javascript:load('edit_note.aspx?source=JETNET&from=view&ac_ID=" & IIf(r("client_jetnet_ac_id") > 0, r("client_jetnet_ac_id"), r("ac_id")) & "&type=note&action=new&ViewID=19&refreshing=prospect&rememberTab=" & ActiveTabIndex & "&NoteID=0');"">")

            '              CheckNoteTable = crmViewDataLayer.Check_For_Applicable_Notes_LIMIT_CRM("AC", 0, IIf(r("client_jetnet_ac_id") > 0, r("client_jetnet_ac_id"), r("ac_id")), "JETNET", "", 1, HttpContext.Current.Application.Item("crmClientDatabase"))
            '              If Not IsNothing(CheckNoteTable) Then
            '                If CheckNoteTable.Rows.Count > 0 Then
            '                  If CheckNoteTable.Rows(0).Item("lnote_status") = "A" Then
            '                    htmlOut.Append("<img src='images/note_pin_add.png' width='16' title='" & CheckNoteTable.Rows(0).Item("lnote_entry_date") & " - " & CheckNoteTable.Rows(0).Item("lnote_note") & "'>")
            '                  Else
            '                    htmlOut.Append("<img src='images/blue_plus_sign.png' width='16'>")
            '                  End If
            '                Else
            '                  htmlOut.Append("<img src='images/blue_plus_sign.png' width='16'>")
            '                End If
            '              End If

            '              htmlOut.Append("</a>")

            '            End If
            '          End If
            '          htmlOut.Append("</td>")

            '        End If
            '        ' End If
            '      End If



            '      htmlOut.Append("<td align='center' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>")  ' SERIAL NUMBER

            '      htmlOut_Export.Append("<td align='center' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>")  ' SERIAL NUMBER


            '      If (Not searchCriteria.ViewCriteriaIsReport And DisplayLink) Or DisplayLink Then
            '        If Not IsDBNull(r("ac_ser_no_full")) Then

            '          '    If r.Item("source").ToString = "JETNET" Then
            '          htmlOut.Append("<a class='underline' onclick='javascript:openSmallWindowJS(""DisplayAircraftDetail.aspx?acid=" + r.Item("ac_id").ToString + "&jid=0"",""AircraftDetails"");' title='Display Aircraft Details'>")
            '          'Else
            '          '  Dim JetnetForSaleCheck As New DataTable
            '          '  Dim NotForSaleJetnetSide As Boolean = False
            '          '  'This is where we need to add a check for client off market aircraft. 
            '          '  'On both the market summary view and the value view need to have a way of showing that an aircraft is an off market.
            '          '  'Recommend the following: on display of every client record in the listing check to see if there is a 
            '          '  'corresponding jetnet for sale record 
            '          '  '(select count(*) from aircraft where ac_id = #### and ac_journ_id = 0 and ac_forsale_flag=’Y’), 
            '          '  'if not then color the serial number red and bold it and modify the alt tag/mouseover to read as 
            '          '  '“Display Aircraft Details: JETNET shows this aircraft as off market.
            '          '  JetnetForSaleCheck = JetnetViewData.Check_Jetnet_Off_Market_Aircraft(r.Item("client_jetnet_ac_id"))
            '          '  If Not IsNothing(JetnetForSaleCheck) Then
            '          '    If JetnetForSaleCheck.Rows.Count > 0 Then
            '          '      If JetnetForSaleCheck.Rows(0).Item(0) = 0 Then
            '          '        NotForSaleJetnetSide = True
            '          '      End If
            '          '    End If
            '          '  End If

            '          '  htmlOut.Append("<a onclick='javascript:openSmallWindowJS(""DisplayAircraftDetail.aspx?acid=" + r.Item("client_jetnet_ac_id").ToString + "&jid=0"",""AircraftDetails"");'")

            '          '  If NotForSaleJetnetSide Then
            '          '    htmlOut.Append(" class='underline error_text' title='Display Aircraft Details: JETNET shows this aircraft as off market.'>")
            '          '  Else
            '          '    htmlOut.Append(" class='underline' title='Display Aircraft Details'>")
            '          '  End If

            '          'End If



            '          htmlOut.Append(r.Item("ac_ser_no_full").ToString + "</a>")

            '          htmlOut_Export.Append(r.Item("ac_ser_no_full").ToString + "</a>")

            '        Else
            '          htmlOut.Append("&nbsp;")
            '        End If
            '      Else

            '        If Not IsDBNull(r("ac_ser_no_full")) Then
            '          htmlOut.Append(font_shrink & "" & r.Item("ac_ser_no_full").ToString & "</font>")
            '          htmlOut_Export.Append(font_shrink & "" & r.Item("ac_ser_no_full").ToString & "</font>")
            '        Else
            '          htmlOut.Append("&nbsp;")
            '          htmlOut_Export.Append("&nbsp;")
            '        End If


            '      End If





            '      htmlOut.Append("</font></td><td align='center' valign='middle' class='forSaleCellBorder'>" & font_shrink) ' YR MFG
            '      htmlOut_Export.Append("</font></td><td align='center' valign='middle' class='forSaleCellBorder'>" & font_shrink) ' YR MFG

            '      If Not IsDBNull(r("ac_mfr_year")) Then
            '        If Not String.IsNullOrEmpty(r.Item("ac_mfr_year")) Then
            '          If CDbl(r.Item("ac_mfr_year").ToString) = 0 Then
            '            htmlOut.Append("0")
            '            htmlOut_Export.Append("0")
            '          Else
            '            htmlOut.Append(r.Item("ac_mfr_year").ToString)
            '            htmlOut_Export.Append(r.Item("ac_mfr_year").ToString)
            '          End If
            '        End If
            '      Else
            '        htmlOut.Append("U")
            '        htmlOut_Export.Append("U")
            '      End If

            '      htmlOut.Append("</font></td><td align='center' valign='middle' class='forSaleCellBorder'>" & font_shrink) ' YR DLV
            '      htmlOut_Export.Append("</font></td><td align='center' valign='middle' class='forSaleCellBorder'>" & font_shrink) ' YR DLV

            '      If Not IsDBNull(r("ac_year")) Then
            '        If Not String.IsNullOrEmpty(r.Item("ac_year")) Then
            '          If CDbl(r.Item("ac_year").ToString) = 0 Then
            '            htmlOut.Append("0")
            '            htmlOut_Export.Append("0")
            '          Else
            '            htmlOut.Append(r.Item("ac_year").ToString)
            '            htmlOut_Export.Append(r.Item("ac_year").ToString)
            '          End If
            '        End If
            '      Else
            '        htmlOut.Append("U")
            '        htmlOut_Export.Append("U")
            '      End If

            '      If DisplayLink Then
            '        htmlOut.Append("</font></td><td align='left' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>") ' OWNER

            '        htmlOut_Export.Append("</font></td><td align='left' valign='middle' class='forSaleCellBorder' >" & font_shrink) ' OWNER
            '      Else
            '        htmlOut.Append("</font></td><td align='left' valign='middle' class='forSaleCellBorder' >" & font_shrink) ' OWNER
            '      End If

            '      'Owner table has been moved up above the notes icon. So it doesn't have to be ran twice.
            '      If Not IsNothing(ownerDataTable) Then

            '        If ownerDataTable.Rows.Count > 0 Then
            '          For Each vr_owner As DataRow In ownerDataTable.Rows

            '            '  Select Case UCase(r("source").ToString)
            '            '   Case "JETNET"
            '            sCompanyPhone = commonEvo.get_company_phone(CLng(vr_owner.Item("comp_id").ToString), True)
            '            '   Case "CLIENT"
            '            ' sCompanyPhone = crmViewDataLayer.Get_Client_Company_Phone(CLng(vr_owner.Item("comp_id").ToString), True)
            '            ' End Select

            '            If String.IsNullOrEmpty(sCompanyPhone) Then
            '              sCompanyPhone = "Not listed"
            '            End If

            '            If Not searchCriteria.ViewCriteriaIsReport And DisplayLink Then
            '              ' If r.Item("source").ToString = "JETNET" Then
            '              htmlOut.Append("<strong><a class='underline' onclick='javascript:openSmallWindowJS(""DisplayCompanyDetail.aspx?compid=" + vr_owner.Item("comp_id").ToString + "&journid=0"",""CompanyDetails"");'")
            '              'Else
            '              ' htmlOut.Append("<strong><a class='underline' onclick='javascript:openSmallWindowJS(""DisplayCompanyDetail.aspx?compid=" + vr_owner.Item("clicomp_jetnet_comp_id").ToString + "&journid=0"",""CompanyDetails"");'")
            '              ' End If

            '              htmlOut.Append(" title='PH : " + sCompanyPhone + "'>" + vr_owner.Item("comp_name").ToString.Trim + "</a></strong>")
            '              htmlOut_Export.Append("" + font_shrink + vr_owner.Item("comp_name").ToString.Trim + "</font>") ' OWNER
            '            Else

            '              If is_word Then
            '                htmlOut.Append("" + font_shrink + vr_owner.Item("comp_name").ToString.Trim + "</font>") ' OWNER 
            '              Else
            '                htmlOut.Append("<strong>" + font_shrink + vr_owner.Item("comp_name").ToString.Trim + "</font></strong>") ' OWNER 
            '              End If


            '              If DisplayLink Then
            '                htmlOut.Append("<td align='left' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>" + sCompanyPhone) ' OWNERPHONE  
            '              End If
            '            End If
            '          Next
            '        Else
            '          If Not searchCriteria.ViewCriteriaIsReport Then
            '            htmlOut.Append("<strong>None</strong>")
            '            htmlOut_Export.Append("<strong>None</strong>")
            '          Else
            '            htmlOut.Append("<strong>None</strong></td>") ' OWNER
            '            htmlOut.Append("<td align='left' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>&nbsp;") ' OWNERPHONE

            '            htmlOut_Export.Append("<strong>None</strong></td>") ' OWNER
            '            htmlOut_Export.Append("<td align='left' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>&nbsp;") ' OWNERPHONE
            '          End If
            '        End If
            '      Else
            '        If Not searchCriteria.ViewCriteriaIsReport Then
            '          htmlOut.Append("<strong>None</strong>")
            '          htmlOut_Export.Append("<strong>None</strong>")
            '        Else
            '          htmlOut.Append("<strong>None</strong></td>") ' OWNER
            '          htmlOut.Append("<td align='left' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>&nbsp;") ' OWNERPHONE  

            '          htmlOut_Export.Append("<strong>None</strong></td>") ' OWNER
            '          htmlOut_Export.Append("<td align='left' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>&nbsp;") ' OWNERPHONE  
            '        End If
            '      End If

            '      ownerDataTable = Nothing

            '      If searchCriteria.ViewCriteriaIsReport Then

            '        searchCriteria.ViewCriteriaGetExclusive = False
            '        searchCriteria.ViewCriteriaGetOperator = True

            '        Dim operatorDataTable As New DataTable

            '        '  Select Case UCase(r("source").ToString)
            '        '  Case "JETNET"
            '        operatorDataTable = JetnetViewData.get_owner_info(searchCriteria)
            '        '   Case "CLIENT"
            '        ' operatorDataTable = crmViewDataLayer.Get_Client_Owner_Info(searchCriteria)
            '        ' End Select


            '        If Not IsNothing(operatorDataTable) Then

            '          If operatorDataTable.Rows.Count > 0 Then
            '            For Each r_operator As DataRow In operatorDataTable.Rows
            '              sCompanyPhone = ""
            '              htmlOut.Append("</td><td align='left' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>") ' OPERATOR
            '              htmlOut.Append("<strong>" + r_operator.Item("comp_name").ToString.Trim + "</strong></td>")
            '              htmlOut.Append("<td align='left' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>")

            '              htmlOut_Export.Append("</td><td align='left' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>") ' OPERATOR
            '              htmlOut_Export.Append("<strong>" + r_operator.Item("comp_name").ToString.Trim + "</strong></td>")
            '              htmlOut_Export.Append("<td align='left' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>")

            '              '   Select Case UCase(r("source").ToString)
            '              '  Case "JETNET"
            '              sCompanyPhone = commonEvo.get_company_phone(CLng(r_operator.Item("comp_id").ToString), True) ' OPERATORPHONE  
            '              '    Case "CLIENT"
            '              '  sCompanyPhone = crmViewDataLayer.Get_Client_Company_Phone(CLng(r_operator.Item("comp_id").ToString), True)
            '              '  End Select

            '              htmlOut.Append(sCompanyPhone)
            '              htmlOut_Export.Append(sCompanyPhone)
            '              '+ 
            '            Next
            '          Else
            '            htmlOut.Append("</td><td align='left' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>") ' OPERATOR
            '            htmlOut.Append("<strong>None</strong></td>")
            '            htmlOut.Append("<td align='left' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>&nbsp;") ' OPERATORPHONE  

            '            htmlOut_Export.Append("</td><td align='left' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>") ' OPERATOR
            '            htmlOut_Export.Append("<strong>None</strong></td>")
            '            htmlOut_Export.Append("<td align='left' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>&nbsp;") ' OPERATORPHONE   
            '          End If
            '        Else
            '          htmlOut.Append("</td><td align='left' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>") ' OPERATOR
            '          htmlOut.Append("<strong>None</strong></td>")
            '          htmlOut.Append("<td align='left' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>&nbsp;") ' OPERATORPHONE 

            '          htmlOut_Export.Append("</td><td align='left' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>") ' OPERATOR
            '          htmlOut_Export.Append("<strong>None</strong></td>")
            '          htmlOut_Export.Append("<td align='left' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>&nbsp;") ' OPERATORPHONE 
            '        End If

            '        operatorDataTable = Nothing

            '      End If



            '      If DisplayLink Then
            '        htmlOut.Append("</td><td align='left' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>") ' BROKER
            '        htmlOut_Export.Append("</td><td align='left' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>") ' BROKER

            '        searchCriteria.ViewCriteriaGetExclusive = True
            '        searchCriteria.ViewCriteriaGetOperator = False

            '        Dim exclusiveDataTable As New DataTable

            '        '  Select Case UCase(r("source").ToString)
            '        '   Case "JETNET"
            '        exclusiveDataTable = JetnetViewData.get_owner_info(searchCriteria)
            '        '   Case "CLIENT"
            '        '  exclusiveDataTable = crmViewDataLayer.Get_Client_Owner_Info(searchCriteria)
            '        '  End Select


            '        If Not IsNothing(exclusiveDataTable) Then

            '          If exclusiveDataTable.Rows.Count > 0 Then
            '            For Each vr_exclusive As DataRow In exclusiveDataTable.Rows

            '              '  Select Case UCase(r("source").ToString)
            '              '    Case "JETNET"
            '              sCompanyPhone = commonEvo.get_company_phone(CLng(vr_exclusive.Item("comp_id").ToString), True) ' OPERATORPHONE  
            '              '     Case "CLIENT"
            '              '   sCompanyPhone = crmViewDataLayer.Get_Client_Company_Phone(CLng(vr_exclusive.Item("comp_id").ToString), True)
            '              '   End Select


            '              If String.IsNullOrEmpty(sCompanyPhone) Then
            '                sCompanyPhone = "Not listed"
            '              End If

            '              If Not searchCriteria.ViewCriteriaIsReport Then
            '                '  If r.Item("source").ToString = "JETNET" Then
            '                htmlOut.Append("<strong><a class='underline' onclick='javascript:openSmallWindowJS(""DisplayCompanyDetail.aspx?compid=" + vr_exclusive.Item("comp_id").ToString + "&journid=0"",""CompanyDetails"");'")
            '                htmlOut_Export.Append("<strong>")
            '                'Else
            '                '    htmlOut.Append("<strong><a class='underline' onclick='javascript:openSmallWindowJS(""DisplayCompanyDetail.aspx?compid=" + vr_exclusive.Item("clicomp_jetnet_comp_id").ToString + "&journid=0"",""CompanyDetails"");'")
            '                '    htmlOut_Export.Append("<strong>")
            '                '   End If

            '                ' htmlOut.Append("<strong><a class='underline' onclick='javascript:openSmallWindowJS(""DisplayCompanyDetail.aspx?compid=" + vr_exclusive.Item("comp_id").ToString + "&journid=0"",""CompanyDetails"");'")
            '                htmlOut.Append(" title='PH : " + sCompanyPhone + "'><font style='color:purple;'>" + vr_exclusive.Item("comp_name").ToString.Trim + "</font></a></strong>")
            '                htmlOut_Export.Append("" + vr_exclusive.Item("comp_name").ToString.Trim + "</strong>")
            '              Else
            '                htmlOut.Append("<strong><font style='color:purple;'>" + vr_exclusive.Item("comp_name").ToString.Trim + "</font></strong></td>")
            '                htmlOut.Append("<td align='left' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>" + sCompanyPhone) ' BROKERPHONE  
            '                htmlOut_Export.Append("<strong><font style='color:purple;'>" + vr_exclusive.Item("comp_name").ToString.Trim + "</font></strong></td>")
            '                htmlOut_Export.Append("<td align='left' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>" + sCompanyPhone) ' BROKERPHONE  
            '              End If
            '            Next
            '          Else
            '            If Not searchCriteria.ViewCriteriaIsReport Then
            '              htmlOut.Append("<strong>None</strong>")
            '              htmlOut_Export.Append("<strong>None</strong>")
            '            Else
            '              htmlOut.Append("<strong>None</strong></td>")
            '              htmlOut.Append("<td align='left' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>&nbsp;") ' BROKERPHONE  
            '              htmlOut_Export.Append("<strong>None</strong></td>")
            '              htmlOut_Export.Append("<td align='left' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>&nbsp;") ' BROKERPHONE  
            '            End If
            '          End If
            '        Else
            '          If Not searchCriteria.ViewCriteriaIsReport Then
            '            htmlOut.Append("<strong>None</strong>")
            '            htmlOut_Export.Append("<strong>None</strong>")
            '          Else
            '            htmlOut.Append("<strong>None</strong></td>")
            '            htmlOut.Append("<td align='left' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>&nbsp;") ' BROKERPHONE  
            '            htmlOut_Export.Append("<strong>None</strong></td>")
            '            htmlOut_Export.Append("<td align='left' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>&nbsp;") ' BROKERPHONE  
            '          End If
            '        End If

            '        exclusiveDataTable = Nothing
            '      End If



            '      htmlOut.Append("</font></td><td align='left' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>" & font_shrink) ' ASKING
            '      htmlOut_Export.Append("</font></td><td align='left' valign='middle' class='forSaleCellBorder' nowrap='nowrap'>" & font_shrink) ' ASKING

            '      'bHadStatus = False
            '      'If Not IsDBNull(r("ac_Status")) Then
            '      '    If Not String.IsNullOrEmpty(r.Item("ac_Status").ToString) Then
            '      '        If r.Item("ac_Status").ToString.ToLower.Trim.Contains("for sale") Then
            '      '            'htmlOut.Append(JetnetViewData.forsale_status(r.Item("ac_Status").ToString.Trim))
            '      '            ' bHadStatus = True
            '      '        End If
            '      '    End If
            '      'End If

            '      'If bHadStatus Then
            '      '    htmlOut.Append("&nbsp;")
            '      'End If




            '      If Not IsDBNull(r("ac_asking")) Then
            '        If r.Item("ac_asking").ToString.ToLower.Trim.Trim.Contains("price") Then
            '          If Not IsDBNull(r("ac_asking_price")) Then
            '            If CDbl(r.Item("ac_asking_price").ToString) > 0 Then
            '              htmlOut.Append("$" + (CDbl(r.Item("ac_asking_price").ToString) / 1000).ToString + "k")
            '              htmlOut_Export.Append("$" + (CDbl(r.Item("ac_asking_price").ToString) / 1000).ToString + "k")
            '            End If
            '          End If
            '        Else
            '          htmlOut.Append(JetnetViewData.forsale_status(r.Item("ac_asking").ToString.Trim))
            '          htmlOut_Export.Append(JetnetViewData.forsale_status(r.Item("ac_asking").ToString.Trim))
            '        End If
            '      End If

            '      htmlOut.Append("&nbsp;</td>")
            '      htmlOut_Export.Append("&nbsp;</td>")



            '      'Take Price Added 
            '      If CRMViewActive Then
            '        htmlOut.Append("<td align='center' valign='middle' class='forSaleCellBorder'>" & font_shrink)
            '        htmlOut_Export.Append("<td align='center' valign='middle' class='forSaleCellBorder'>" & font_shrink)
            '        If Not IsDBNull(r("ac_take_price")) Then
            '          If CDbl(r.Item("ac_take_price").ToString) > 0 Then
            '            htmlOut.Append("$" + (CDbl(r.Item("ac_take_price").ToString) / 1000).ToString + "k")
            '            htmlOut_Export.Append("$" + (CDbl(r.Item("ac_take_price").ToString) / 1000).ToString + "k")
            '          End If
            '        End If
            '        htmlOut.Append("</font></td>")
            '        htmlOut_Export.Append("</font></td>")
            '      End If


            '      'sold_price  Added 
            '      If CRMViewActive Then
            '        htmlOut.Append("<td align='center' valign='middle' class='forSaleCellBorder'>" & font_shrink)
            '        htmlOut_Export.Append("<td align='center' valign='middle' class='forSaleCellBorder'>" & font_shrink)
            '        If Not IsDBNull(r("sold_price")) Then
            '          If CDbl(r.Item("sold_price").ToString) > 0 Then
            '            htmlOut.Append("$" + (CDbl(r.Item("sold_price").ToString) / 1000).ToString + "k")
            '            htmlOut_Export.Append("$" + (CDbl(r.Item("sold_price").ToString) / 1000).ToString + "k")
            '          End If
            '        End If
            '        htmlOut.Append("</font></td>")
            '        htmlOut_Export.Append("</font></td>")
            '      End If

            '      htmlOut.Append("<td align='center' valign='middle' class='forSaleCellBorder'>" & font_shrink) ' AC LIST DATE
            '      htmlOut_Export.Append("<td align='center' valign='middle' class='forSaleCellBorder'>" & font_shrink) ' AC LIST DATE

            '      If Not IsDBNull(r.Item("ac_list_date")) Then
            '        If IsDate(r.Item("ac_list_date").ToString) Then
            '          htmlOut.Append(FormatDateTime(r.Item("ac_list_date").ToString, vbShortDate))
            '          htmlOut_Export.Append(FormatDateTime(r.Item("ac_list_date").ToString, vbShortDate))
            '        Else
            '          htmlOut.Append("&nbsp;")
            '          htmlOut_Export.Append("&nbsp;")
            '        End If
            '      Else
            '        htmlOut.Append("&nbsp;")
            '        htmlOut_Export.Append("&nbsp;")
            '      End If

            '      htmlOut.Append("</font></td><td align='right' valign='middle' class='forSaleCellBorder'>" & font_shrink) ' AFTT
            '      htmlOut_Export.Append("</font></td><td align='right' valign='middle' class='forSaleCellBorder'>" & font_shrink) ' AFTT


            '      If Not IsDBNull(r("ac_airframe_tot_hrs")) Then
            '        If CDbl(r.Item("ac_airframe_tot_hrs").ToString) = 0 Then
            '          htmlOut.Append("0")
            '          htmlOut_Export.Append("0")
            '        Else
            '          htmlOut.Append(r.Item("ac_airframe_tot_hrs").ToString)
            '          htmlOut_Export.Append(r.Item("ac_airframe_tot_hrs").ToString)
            '        End If
            '      Else
            '        htmlOut.Append("U")
            '        htmlOut_Export.Append("U")
            '      End If

            '      htmlOut.Append("</font></td><td align='right' valign='middle' class='forSaleCellBorder'>" & font_shrink) ' Engine Times
            '      htmlOut_Export.Append("</font></td><td align='right' valign='middle' class='forSaleCellBorder'>" & font_shrink) ' Engine Times

            '      If Not IsDBNull(r("ac_engine_1_tot_hrs")) Then
            '        If CDbl(r.Item("ac_engine_1_tot_hrs").ToString) = 0 Then
            '          htmlOut.Append("[0]&nbsp;")
            '          htmlOut_Export.Append("[0]&nbsp;")
            '        Else
            '          htmlOut.Append("[" + r.Item("ac_engine_1_tot_hrs").ToString + "]&nbsp;")
            '          htmlOut_Export.Append("[" + r.Item("ac_engine_1_tot_hrs").ToString + "]&nbsp;")
            '        End If
            '      Else
            '        htmlOut.Append("[U]&nbsp;")
            '        htmlOut_Export.Append("[U]&nbsp;")
            '      End If

            '      If Not IsDBNull(r("ac_engine_2_tot_hrs")) Then
            '        If CDbl(r.Item("ac_engine_2_tot_hrs").ToString) = 0 Then
            '          htmlOut.Append("[0]&nbsp;")
            '          htmlOut_Export.Append("[0]&nbsp;")
            '        Else
            '          htmlOut.Append("[" + r.Item("ac_engine_2_tot_hrs").ToString + "]&nbsp;")
            '          htmlOut_Export.Append("[" + r.Item("ac_engine_2_tot_hrs").ToString + "]&nbsp;")
            '        End If
            '      Else
            '        htmlOut.Append("[U]&nbsp;")
            '        htmlOut_Export.Append("[U]&nbsp;")
            '      End If

            '      If Not IsDBNull(r("ac_engine_3_tot_hrs")) Then
            '        If CDbl(r.Item("ac_engine_3_tot_hrs").ToString) = 0 Then
            '          htmlOut.Append("[0]&nbsp;")
            '          htmlOut_Export.Append("[0]&nbsp;")
            '        Else
            '          htmlOut.Append("[" + r.Item("ac_engine_3_tot_hrs").ToString + "]&nbsp;")
            '          htmlOut_Export.Append("[" + r.Item("ac_engine_3_tot_hrs").ToString + "]&nbsp;")
            '        End If
            '      End If

            '      If Not IsDBNull(r("ac_engine_4_tot_hrs")) Then
            '        If CDbl(r.Item("ac_engine_4_tot_hrs").ToString) = 0 Then
            '          htmlOut.Append("[0]&nbsp;")
            '          htmlOut_Export.Append("[0]&nbsp;")
            '        Else
            '          htmlOut.Append("[" + r.Item("ac_engine_4_tot_hrs").ToString + "]&nbsp;")
            '          htmlOut_Export.Append("[" + r.Item("ac_engine_4_tot_hrs").ToString + "]&nbsp;")
            '        End If
            '      End If



            '      'If DisplayLink Then
            '      '  htmlOut.Append("</font></td><td align='center' valign='middle' class='forSaleCellBorder'>") ' Feature Codes
            '      '  htmlOut_Export.Append("</font></td><td align='center' valign='middle' class='forSaleCellBorder'>") ' Feature Codes

            '      '  Dim sAcFeatureCodes As String = ""
            '      '  '''''''''''''''''''''''''''''''''''''''''''

            '      '  ' If Not IsDBNull(r.Item("source").ToString) Then
            '      '  'If Trim(r.Item("source").ToString) = "CLIENT" Then
            '      '  '   JetnetViewData.display_client_ac_feature_codes(searchCriteria, arrFeatCodes, (cellWidth * 2.1), sAcFeatureCodes)
            '      '  ' Else
            '      '  JetnetViewData.display_ac_feature_codes(searchCriteria, arrFeatCodes, (cellWidth * 2.1), sAcFeatureCodes)
            '      '  '  End If
            '      '  ' Else
            '      '  ' JetnetViewData.display_ac_feature_codes(searchCriteria, arrFeatCodes, (cellWidth * 2.1), sAcFeatureCodes)
            '      '  '  End If


            '      '  htmlOut.Append(sAcFeatureCodes)

            '      '  sAcFeatureCodes = Replace(Trim(sAcFeatureCodes), "height='15'", "")
            '      '  sAcFeatureCodes = Replace(Trim(sAcFeatureCodes), "vertical-align: middle;", "")
            '      '  sAcFeatureCodes = Replace(Trim(sAcFeatureCodes), "'>No features", "' colspan='4'>No features")


            '      '  htmlOut_Export.Append(sAcFeatureCodes)
            '      'End If



            '      htmlOut.Append("</td><td align='center' valign='middle' class='forSaleCellBorder'>" & font_shrink) ' PASSENGERS
            '      htmlOut_Export.Append("</td><td align='center' valign='middle' class='forSaleCellBorder'>" & font_shrink)

            '      If Not IsDBNull(r("ac_passenger_count")) Then
            '        If CDbl(r.Item("ac_passenger_count").ToString) = 0 Then
            '          htmlOut.Append("0&nbsp;")
            '          htmlOut_Export.Append("0&nbsp;")
            '        Else
            '          htmlOut.Append(r.Item("ac_passenger_count").ToString + "&nbsp;")
            '          htmlOut_Export.Append(r.Item("ac_passenger_count").ToString + "&nbsp;")
            '        End If
            '      Else
            '        htmlOut.Append("U&nbsp;")
            '        htmlOut_Export.Append("U&nbsp;")
            '      End If

            '      htmlOut.Append("</font></td><td align='center' valign='middle' class='forSaleCellBorder'>" & font_shrink) ' INT YEAR
            '      htmlOut_Export.Append("</font></td><td align='center' valign='middle' class='forSaleCellBorder'>" & font_shrink)

            '      If Not String.IsNullOrEmpty(r.Item("ac_interior_moyear").ToString) Then
            '        htmlOut.Append(Left(r.Item("ac_interior_moyear").ToString, 2).Trim)
            '        htmlOut_Export.Append(Left(r.Item("ac_interior_moyear").ToString, 2).Trim)

            '        If Not String.IsNullOrEmpty(Left(r.Item("ac_interior_moyear").ToString, 2).Trim) Then
            '          htmlOut.Append("/")
            '          htmlOut_Export.Append("/")
            '        End If
            '        htmlOut.Append(Right(r.Item("ac_interior_moyear").ToString, 4).Trim + "&nbsp;")
            '        htmlOut_Export.Append(Right(r.Item("ac_interior_moyear").ToString, 4).Trim + "&nbsp;")
            '      Else
            '        htmlOut.Append("&nbsp;")
            '        htmlOut_Export.Append("&nbsp;")
            '      End If



            '      'If HttpContext.Current.Session.Item("localPreferences").HasLocalNotes And Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("localPreferences").NotesDatabaseName) And searchCriteria.ViewCriteriaNoLocalNotes = False Then
            '      ' htmlOut.Append("</td><td align='center' valign='middle' class='forSaleCellBorder'>") ' EXT YEAR
            '      '  Else
            '      htmlOut.Append("</font></td><td align='center' valign='middle' class='forSaleCellBorderNoNotes'>" & font_shrink) ' EXT YEAR
            '      htmlOut_Export.Append("</font></td><td align='center' valign='middle' class='forSaleCellBorderNoNotes'>" & font_shrink) ' EXT YEAR

            '      '   End If

            '      If Not String.IsNullOrEmpty(r.Item("ac_exterior_moyear").ToString) Then
            '        htmlOut.Append(Left(r.Item("ac_exterior_moyear").ToString, 2).Trim)
            '        htmlOut_Export.Append(Left(r.Item("ac_exterior_moyear").ToString, 2).Trim)
            '        If Not String.IsNullOrEmpty(Left(r.Item("ac_exterior_moyear").ToString, 2).Trim) Then
            '          htmlOut.Append("/")
            '          htmlOut_Export.Append("/")
            '        End If
            '        htmlOut.Append(Right(r.Item("ac_exterior_moyear").ToString, 4).Trim + "&nbsp;")
            '        htmlOut_Export.Append(Right(r.Item("ac_exterior_moyear").ToString, 4).Trim + "&nbsp;")
            '      Else
            '        htmlOut.Append("&nbsp;")
            '        htmlOut_Export.Append("&nbsp;")
            '      End If

            '      If DisplayLink Then
            '        If ((HttpContext.Current.Session.Item("localPreferences").HasServerNotes And Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("localPreferences").ServerNotesDatabaseName) Or CRMViewActive = True)) Then

            '          htmlOut.Append("</td><td align='center' valign='middle' class='forSaleCellBorder' title='Most Recent Local Note'>") ' NOTES

            '          'This appends the notes on the table.
            '          ' htmlOut.Append(crmViewDataLayer.CheckForNotesForSaleTab(CRMViewActive, r.Item("source").ToString, r.Item("ac_id"), aclsData_Temp))
            '          htmlOut.Append(crmViewDataLayer.CheckForNotesForSaleTab(CRMViewActive, "JETNET", r.Item("ac_id"), aclsData_Temp))


            '        End If
            '      End If

            '      htmlOut.Append("</font></td></tr>")
            '      htmlOut_Export.Append("</font></td></tr>")
            '      '---------------------------- TAKEN FROM FOR SALE ITEMS -------------------

            '    Next

            '    htmlOut.Append("</table></div></td></tr></table></td></tr>")

            '  Else
            '    htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""seperator"" style=""padding-left:3px;"">No data matches for your search criteria</td></tr>")
            '  End If
            'Else
            '  htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""seperator"" style=""padding-left:3px;"">No data matches for your search criteria</td></tr>")
            'End If

            htmlOut.Append("</table>")

        Catch ex As Exception

            aError = "Error in views_display_fractions_to_expire(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub
    Public Sub fill_airport_view_data_grid(ByVal Results_Table As DataTable, ByVal aclsData_Temp As clsData_Manager_SQL, ByRef AircraftSearchDataGrid As DataGrid)
        Dim Counter As Integer = 0
        Dim Dynamically_Configured_Datagrid As New DataGrid
        Dim RecordsPerPage As Integer = 1000
        Dim Paging_Table As New DataTable

        Try


            Dynamically_Configured_Datagrid = AircraftSearchDataGrid

            If Not IsNothing(Results_Table) Then

                If Results_Table.Rows.Count > 0 Then

                    If HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag = False Then
                        Dynamically_Configured_Datagrid.Columns(5).Visible = True
                        Dynamically_Configured_Datagrid.Columns(6).Visible = True
                    End If

                    'This is basically saying that if the datagrid isn't visible, don't fill it
                    ' If Dynamically_Configured_Datagrid.Visible = True Then
                    Dynamically_Configured_Datagrid.DataSource = Results_Table
                    Dynamically_Configured_Datagrid.PageSize = RecordsPerPage
                    'Added this on 07/01/2015 - This is going to reset the current page index whenever the datagrid listing is active
                    'and a new search occurs.
                    Dynamically_Configured_Datagrid.CurrentPageIndex = 0 'PageNumber - 1
                    Dynamically_Configured_Datagrid.DataBind()
                    'End If


                    ''This is basically saying that if the datagrid isn't visible, don't fill it
                    'If Dynamically_Configured_DataList.Visible = True Then
                    '  'We need to add the paging to this for now since the datalist doesn't natively support paging. 
                    '  'For right now, we clone the results table (getting the schema) then filter based on the ac_count field (added during query)
                    '  'This will allow us to bind based on the paging table.
                    '  Paging_Table = Results_Table.Clone
                    '  Dim afiltered_Client As DataRow() = Results_Table.Select("ac_id > 0 ", "")
                    '  For Each atmpDataRow_Client In afiltered_Client
                    '    Paging_Table.ImportRow(atmpDataRow_Client)
                    '  Next

                    '  Dynamically_Configured_DataList.DataSource = Paging_Table
                    '  Dynamically_Configured_DataList.DataBind()
                    'End If

                    ''criteria_results.Text = Results_Table.Rows.Count & " Results"

                    'record_count.Text = "Showing 1 - " & IIf(Results_Table.Rows.Count <= RecordsPerPage, Results_Table.Rows.Count, RecordsPerPage)
                    'bottom_record_count.Text = "Showing 1 - " & IIf(Results_Table.Rows.Count <= RecordsPerPage, Results_Table.Rows.Count, RecordsPerPage)

                    ''This will fill up the dropdown bar with however many pages.
                    'If Results_Table.Rows.Count > RecordsPerPage Then
                    '  Fill_Page_To_To_Dropdown(Math.Ceiling(Results_Table.Rows.Count / RecordsPerPage))
                    '  'Criteria_Bar2.Fill_Page_To_To_Dropdown(Math.Ceiling(Results_Table.Rows.Count / RecordsPerPage))
                    '  SetPagingButtons(False, True)
                    '  'Criteria_Bar2.SetPagingButtons(False, True)
                    'Else
                    '  Fill_Page_To_To_Dropdown(1)
                    '  SetPagingButtons(False, False)
                    '  'Criteria_Bar2.SetPagingButtons(False, False)
                    'End If


                    'PanelCollapseEx.Collapsed = True
                    'Paging_Table = Nothing
                    Results_Table = Nothing

                Else
                    Dynamically_Configured_Datagrid.CurrentPageIndex = 0

                    Dynamically_Configured_Datagrid.DataSource = New DataTable
                    Dynamically_Configured_Datagrid.DataBind()
                    'Dynamically_Configured_DataList.DataSource = New DataTable
                    'Dynamically_Configured_DataList.DataBind()
                End If
            Else 'this means that the datatable equals nothing

                Dynamically_Configured_Datagrid.CurrentPageIndex = 0

                Dynamically_Configured_Datagrid.DataSource = New DataTable
                Dynamically_Configured_Datagrid.DataBind()
                'Dynamically_Configured_DataList.DataSource = New DataTable
                'Dynamically_Configured_DataList.DataBind()

            End If

        Catch ex As Exception

        End Try
    End Sub
    Public Sub get_most_recent_flight_activity_companies_top_function(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByVal aport_name As String, ByVal run_export As String, ByRef title_text As String, ByRef title_text2 As String, ByVal selected_value As String, ByVal recent_flight_months As Integer, ByVal contact_type As String, ByVal use_ac As Boolean, ByVal start_date As String, ByVal end_date As String, ByVal product_code_selection As String)

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False
        Dim same_country As Boolean = False

        Try

            results_table = get_most_recent_flight_activity_companies(searchCriteria, run_export, selected_value, recent_flight_months, contact_type, use_ac, start_date, end_date, product_code_selection)

            If Trim(run_export) <> "" Then
                crmViewDataLayer.ExportTableData(results_table)
            Else
                htmlOut.Append("<table id=""fractionsExpiringOuterTable"" width=""100%"" cellpadding=""0"" cellspacing=""0"" class=""module"">")

                title_text = " at " & aport_name & " Last "

                title_text2 = " - " & results_table.Rows.Count & " Flights Displayed"


                If Not IsNothing(results_table) Then

                    If results_table.Rows.Count > 0 Then


                        htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""header"">")
                        htmlOut.Append("<table cellspacing='0' cellpadding='0' width='100%'><tr><td align='left' width='80%'>")

                        If Trim(contact_type) = "36" Then
                            htmlOut.Append("&nbsp;Operators Flight Activity at " & aport_name & " (" & start_date & " - " & end_date & ") - " & results_table.Rows.Count & " Operators")
                            If use_ac = True Then
                                htmlOut.Append("/Aircraft")
                            End If
                            htmlOut.Append("</td>")
                            If searchCriteria.ViewID = 28 Then
                                htmlOut.Append("<td align='right'>Export:&nbsp;&nbsp;<a href='view_template.aspx?ViewID=28&ViewName=Operator/Airport Utilization&aport_id=" & Airport_ID_OVERALL & "&activetab=19&export=OP' target='_blank'><font color='white'><u>Full List</u></font></a>&nbsp;&nbsp;")
                            Else
                                htmlOut.Append("<td align='right'>Export:&nbsp;&nbsp;<a href='view_template.aspx?ViewID=24&ViewName=Airport FBO View&aport_id=" & Airport_ID_OVERALL & "&activetab=4&export=OP' target='_blank'><font color='white'><u>Full List</u></font></a>&nbsp;&nbsp;")
                            End If

                        Else
                            htmlOut.Append("&nbsp;Owners Flight Activity at " & aport_name & " (" & start_date & " - " & end_date & ") - " & results_table.Rows.Count & " Owners") ' , " & results_table.Rows.Count & " Aircraft
                            If use_ac = True Then
                                htmlOut.Append("/Aircraft")
                            End If
                            htmlOut.Append("</td>")
                            If searchCriteria.ViewID = 28 Then
                                htmlOut.Append("<td align='right'>Export:&nbsp;&nbsp;<a href='view_template.aspx?ViewID=28&ViewName=Operator/Airport Utilization&aport_id=" & Airport_ID_OVERALL & "&activetab=20&export=OW' target='_blank'><font color='white'><u>Full List</u></font></a>&nbsp;&nbsp;")
                            Else
                                htmlOut.Append("<td align='right'>Export:&nbsp;&nbsp;<a href='view_template.aspx?ViewID=24&ViewName=Airport FBO View&aport_id=" & Airport_ID_OVERALL & "&activetab=3&export=OW' target='_blank'><font color='white'><u>Full List</u></font></a>&nbsp;&nbsp;")
                            End If

                        End If

                        htmlOut.Append("&nbsp;</td></tr></table>")
                        htmlOut.Append("</td></tr>")


                        htmlOut.Append("<tr><td valign=""top"" align=""left"">")

                        htmlOut.Append("<table id=""fractionsExpiringInnerTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"">")

                        htmlOut.Append("<tr><td colspan=""2"" class=""rightside"" valign=""top"">")
                        htmlOut.Append("<div style=""height: 250px; overflow: auto;"">")

                        htmlOut.Append("<table id=""fractionsExpiringDataTable"" width=""100%"" cellpadding=""4"" cellspacing=""0"">")
                        htmlOut.Append("<tr>")
                        htmlOut.Append("<td valign=""top"" align=""left"" class=""seperator""><strong>Company</strong></td>")

                        If use_ac = True Then
                            htmlOut.Append("<td valign=""top"" align=""left"" class=""seperator""><strong>Aircraft</strong></td>")
                        End If

                        htmlOut.Append("<td valign=""top"" align=""left"" class=""seperator"" nowrap='nowrap'><strong>Flight Time (min)</strong></td>")

                        htmlOut.Append("<td valign=""top"" align=""right"" class=""seperator"" nowrap='nowrap'><strong>Flights</strong></td>")
                        htmlOut.Append("</tr>")


                        For Each r As DataRow In results_table.Rows

                            If Not toggleRowColor Then
                                htmlOut.Append("<tr class=""alt_row"">")
                                toggleRowColor = True
                            Else
                                htmlOut.Append("<tr bgcolor=""white"">")
                                toggleRowColor = False
                            End If


                            htmlOut.Append("<td , align=""left"" valign=""top"" class=""seperator"">")
                            htmlOut.Append("<strong><a class='underline' onclick=""javascript:load('DisplayCompanyDetail.aspx?compid=" + r.Item("COMPID").ToString + "&journid=0','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');"">")
                            htmlOut.Append("" & r.Item("COMPANY").ToString & "</a></strong> (")
                            htmlOut.Append("" & r.Item("COMP_ADDRESS").ToString & ", " & r.Item("CITY").ToString & ", " & r.Item("STATE").ToString)
                            htmlOut.Append(")</td>")

                            If use_ac = True Then
                                htmlOut.Append("<td align=""left"" valign=""top"" class=""seperator"">")
                                htmlOut.Append("" & r.Item("Make").ToString & " ")
                                htmlOut.Append("" & r.Item("Model").ToString & "")
                                htmlOut.Append(" S#: ")
                                htmlOut.Append("<a class='underline' onclick=""javascript:load('DisplayAircraftDetail.aspx?acid=" + r.Item("ACId").ToString + "&jid=0','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');"" title='Display Aircraft Details'>")
                                htmlOut.Append("" & r.Item("SerNbr").ToString & "</a> ")
                                htmlOut.Append("R#: " & r.Item("RegNbr").ToString & " ")
                                htmlOut.Append("</td>")
                            End If

                            htmlOut.Append("<td align=""right"" valign=""top"" class=""seperator"">")
                            If Not IsDBNull(r.Item("FLIGHT_TIME")) Then
                                htmlOut.Append("" & FormatNumber(r.Item("FLIGHT_TIME").ToString, 0) & "")
                            End If

                            htmlOut.Append("&nbsp;</td>")

                            htmlOut.Append("<td align=""right"" valign=""top"" class=""seperator"">")
                            htmlOut.Append("" & r.Item("TOTAL_COUNT").ToString & "")
                            htmlOut.Append("&nbsp;</td>")


                            htmlOut.Append("</tr>")
                        Next

                        htmlOut.Append("</table></div></td></tr></table></td></tr>")

                    Else
                        htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""seperator"" style=""padding-left:3px;"">No FAA Flight Activity Found.</td></tr>")
                    End If
                Else
                    htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""seperator"" style=""padding-left:3px;"">No FAA Flight Activity Found.</td></tr>")
                End If

                htmlOut.Append("</table>")
            End If

        Catch ex As Exception

            aError = "Error in views_display_fractions_to_expire(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub

    Public Sub FlightJSArray(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByVal aport_name As String, ByVal run_export As String, ByRef title_text As String, ByRef title_text2 As String, ByVal selected_value As String, ByVal recent_flight_months As Integer, ByVal start_date As String, ByVal end_date As String, ByVal product_code_selection As String, ByVal product_link As String, ByRef table_count As Long, ByVal from_spot As String, ByVal table_color As String, Optional ByRef ColumnList As String = "")

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False
        Dim same_country As Boolean = False
        Dim temp_date As String = ""
        Dim i As Integer = 0
        Dim Results As String = ""
        Try

            results_table = get_most_recent_flight_activity(searchCriteria, run_export, selected_value, recent_flight_months, start_date, end_date, product_code_selection, True)

            If Not IsNothing(results_table) Then
                If results_table.Rows.Count > 0 Then
                    table_count = results_table.Rows.Count

                    If Trim(from_spot) = "pdf" Then
                        htmlOut.Append("<tr><td valign='top' align='center'><font class='" & HttpContext.Current.Session("FONT_CLASS_HEADER") & " mainHeading'><strong>" & searchCriteria.ViewCriteriaAircraftMake & " " & searchCriteria.ViewCriteriaAircraftModel & "</strong> Flights</font></td></tr>")

                        htmlOut.Append("<tr><td valign='top'><div class=""Box""><table id=""weightTable"" cellpadding='3'  cellspacing='0' width='100%' class='formatTable " & table_color & "'><thead>")
                        htmlOut.Append("<tr class=""noBorder"">")
                        htmlOut.Append("<th>AIRCRAFT</th>")
                        htmlOut.Append("<th>SER#</th>")
                        htmlOut.Append("<th>REG#</th>")
                        htmlOut.Append("<th>DATE</th>")

                        htmlOut.Append("<th>ORIGIN AIRPORT</th>")
                        htmlOut.Append("<th>COUNTRY</th>")
                        htmlOut.Append("<th>CITY</th>")
                        htmlOut.Append("<th>STATE</th>")
                        htmlOut.Append("<th>DESTINATION AIRPORT</th>")
                        htmlOut.Append("<th>COUNTRY</th>")
                        htmlOut.Append("<th>CITY</th>")
                        htmlOut.Append("<th>STATE</th>")

                        htmlOut.Append("<th>FLIGHT TIME</th>")
                        htmlOut.Append("<th>DIST(NM)</th>")
                        htmlOut.Append("<th>EST FUEL</br>BURN (GAL)</th>")
                        htmlOut.Append("</tr>")
                        htmlOut.Append("</thead>")
                        htmlOut.Append("<tbody>")


                    End If


                    If Trim(from_spot) = "view" Then


                        htmlOut.Append(DisplayFunctions.ConvertDataTableToArrayCombinedFields(results_table, ColumnList, searchCriteria, True, Airport_ID_OVERALL, False))

                        '  htmlOut.Append("""check"": """",") 'Checkbox row.


                        '  htmlOut.Append("""ac"": """ & r.Item("Make").ToString & " " & r.Item("Model").ToString & """,")


                        '  If UCase(r.Item("SerNbr")) = "BLOCKED" Then
                        '    htmlOut.Append("""ser"": [""<span title='IDENTITY OF THIS AIRCRAFT IS BLOCKED BASED ON REQUEST OF OWNER/OPERATOR'>" & r.Item("SerNbr").ToString & "</span>"",""0""],")
                        '    htmlOut.Append("""reg"": ""<span title='IDENTITY OF THIS AIRCRAFT IS BLOCKED BASED ON REQUEST OF OWNER/OPERATOR'>" & r.Item("RegNbr").ToString & "</span>"",")
                        '  Else

                        '    htmlOut.Append((buildSerNoMenu(r.Item("SerNbr"), r.Item("acID"), r.Item("SERNOSORT_NONDISPLAY"), searchCriteria)))
                        '    'htmlOut.Append("""ser"": [""<a class='underline' onclick='javascript:openSmallWindowJS(\""DisplayAircraftDetail.aspx?acid=" + r.Item("ACId").ToString + "&jid=0\"",\""AircraftDetails\"");' title='Display Aircraft Details'>" & r.Item("SerNbr").ToString & "</a>"",""" & r.Item("SERNOSORT_NONDISPLAY").ToString & """],")
                        '    htmlOut.Append("""reg"": """ & r.Item("RegNbr").ToString & """,")
                        '  End If

                        '  If Trim(selected_value) = "" Or Trim(selected_value) = "A" Then

                        '  ElseIf Trim(selected_value) = "D" Then
                        '    htmlOut.Append("""date"": """",")
                        '    htmlOut.Append("""destin"": """",")
                        '  End If



                        '  If Trim(selected_value) = "" Or Trim(selected_value) = "A" Then
                        '    htmlOut.Append("""date"": [""")

                        '    temp_date = ""
                        '    If Not IsDBNull(r.Item("FlightDate")) Then
                        '      temp_date = r.Item("FlightDate")
                        '      htmlOut.Append(Format(CDate(temp_date), "MM/dd/yy hh:mm tt"))
                        '      htmlOut.Append(""", """ & Format(r.Item("FlightDate"), "yyyy/MM/dd") & """],")
                        '    Else
                        '      htmlOut.Append(""", """"],")
                        '    End If
                        '  End If
                        '  htmlOut.Append("""origin"": """)

                        '  htmlOut.Append("<a href='view_template.aspx?ViewID=28&ViewName=Operator/Airport Utilization&aport_id=" & r.Item("ffd_origin_aport_id").ToString & "&comp_id=" & searchCriteria.ViewCriteriaCompanyID & "'>")
                        '  htmlOut.Append("" & r.Item("OriginAPort").ToString & "</a> ")

                        '  If Not IsDBNull(r.Item("origin_aport_name")) Then
                        '    htmlOut.Append(" " & Replace(Replace(r.Item("origin_aport_name").ToString, "Airport", ""), "International", "") & " (")
                        '  End If

                        '  If Not IsDBNull(r.Item("origin_aport_country")) Then
                        '    htmlOut.Append("" & r.Item("origin_aport_country").ToString & " ")
                        '  End If

                        '  If Not IsDBNull(r.Item("origin_aport_city")) Then
                        '    htmlOut.Append("" & r.Item("origin_aport_city").ToString & " ")
                        '  End If

                        '  If Not IsDBNull(r.Item("origin_aport_state")) Then
                        '    htmlOut.Append(", " & r.Item("origin_aport_state").ToString)
                        '  End If

                        '  If Not IsDBNull(r.Item("origin_aport_name")) Or Not IsDBNull(r.Item("origin_aport_country")) Or Not IsDBNull(r.Item("origin_aport_city")) Or Not IsDBNull(r.Item("origin_aport_state")) Then
                        '    htmlOut.Append(")")
                        '  End If
                        '  htmlOut.Append(""",")

                        '  htmlOut.Append("""origlat"": """)

                        '  If Not IsDBNull(r.Item("ORIGIN LAT").ToString) Then
                        '    htmlOut.Append("" & r.Item("ORIGIN LAT").ToString & " ")
                        '  End If

                        '  htmlOut.Append(""",")

                        '  htmlOut.Append("""origlong"": """)

                        '  If Not IsDBNull(r.Item("ORIGIN LONG").ToString) Then
                        '    htmlOut.Append("" & r.Item("ORIGIN LONG").ToString & " ")
                        '  End If

                        '  htmlOut.Append(""",")


                        '  htmlOut.Append("""time"": """)

                        '  If Not IsDBNull(r.Item("FlightTime").ToString) Then
                        '    htmlOut.Append("" & r.Item("FlightTime").ToString & " ")
                        '  End If

                        '  htmlOut.Append(""",")

                        '  htmlOut.Append("""distance"": """)

                        '  If Not IsDBNull(r.Item("Distance").ToString) Then
                        '    htmlOut.Append(FormatNumber(flightDataFunctions.ConvertStatuteMileToNauticalMile(r.Item("Distance").ToString), 0) & " ")
                        '  End If

                        '  htmlOut.Append(""",")


                        '  htmlOut.Append("""FuelBurn"": """)

                        '  If Not IsDBNull(r.Item("FuelBurn").ToString) Then
                        '    htmlOut.Append(FormatNumber(r.Item("FuelBurn").ToString, 0) & " ")
                        '  End If

                        '  htmlOut.Append(""",")



                        '  htmlOut.Append("""destination"": """)

                        '  htmlOut.Append("<a href='view_template.aspx?ViewID=28&ViewName=Operator/Airport Utilization&aport_id=" & r.Item("ffd_dest_aport_id").ToString & "&comp_id=" & searchCriteria.ViewCriteriaCompanyID & "'>")

                        '  If Not IsDBNull(r.Item("dest_aport_name")) Then
                        '    htmlOut.Append(" - " & Replace(Replace(r.Item("dest_aport_name").ToString, "Airport", ""), "International", "") & " ")
                        '  End If
                        '  htmlOut.Append("</a>")

                        '  htmlOut.Append(""",")

                        '  htmlOut.Append("""destaport"": """)
                        '  htmlOut.Append("<a href='view_template.aspx?ViewID=28&ViewName=Operator/Airport Utilization&aport_id=" & r.Item("ffd_dest_aport_id").ToString & "&comp_id=" & searchCriteria.ViewCriteriaCompanyID & "'>")

                        '  htmlOut.Append("" & r.Item("DestinAPort").ToString & "</a> ")
                        '  htmlOut.Append(""",")


                        '  htmlOut.Append("""destcountry"": """)
                        '  If Not IsDBNull(r.Item("dest_aport_country")) Then
                        '    htmlOut.Append("" & r.Item("dest_aport_country").ToString & " ")
                        '  End If
                        '  htmlOut.Append(""",")


                        '  htmlOut.Append("""destcity"": """)
                        '  If Not IsDBNull(r.Item("dest_aport_city")) Then
                        '    htmlOut.Append("" & r.Item("dest_aport_city").ToString & " ")
                        '  End If
                        '  htmlOut.Append(""",")

                        '  htmlOut.Append("""deststate"": """)
                        '  If Not IsDBNull(r.Item("dest_aport_state")) Then
                        '    htmlOut.Append("" & r.Item("dest_aport_state").ToString & " ")
                        '  End If
                        '  htmlOut.Append(""",")


                        '  htmlOut.Append("""destlat"": """)
                        '  If Not IsDBNull(r.Item("DEST LAT")) Then
                        '    htmlOut.Append("" & r.Item("DEST LAT").ToString & " ")
                        '  End If
                        '  htmlOut.Append(""",")

                        '  htmlOut.Append("""destlong"": """)
                        '  If Not IsDBNull(r.Item("DEST LONG")) Then
                        '    htmlOut.Append("" & r.Item("DEST LONG").ToString & " ")
                        '  End If
                        '  htmlOut.Append(""",")


                        '  htmlOut.Append("""comp"": """)
                        '  If Not IsDBNull(r.Item("OPERATOR")) Then
                        '    htmlOut.Append("" & r.Item("OPERATOR").ToString & " ")
                        '  End If
                        '  htmlOut.Append(""",")


                        '  htmlOut.Append("""compaddress"": """)
                        '  If Not IsDBNull(r.Item("ADDRESS")) Then
                        '    htmlOut.Append("" & r.Item("ADDRESS").ToString & " ")
                        '  End If
                        '  htmlOut.Append(""",")


                        '  htmlOut.Append("""compcity"": """)
                        '  If Not IsDBNull(r.Item("CITY")) Then
                        '    htmlOut.Append("" & r.Item("CITY").ToString & " ")
                        '  End If
                        '  htmlOut.Append(""",")


                        '  htmlOut.Append("""compstate"": """)
                        '  If Not IsDBNull(r.Item("STATE")) Then
                        '    htmlOut.Append("" & r.Item("STATE").ToString & " ")
                        '  End If
                        '  htmlOut.Append(""",")


                        '  htmlOut.Append("""compcountry"": """)
                        '  If Not IsDBNull(r.Item("COUNTRY")) Then
                        '    htmlOut.Append("" & r.Item("COUNTRY").ToString & " ")
                        '  End If
                        '  htmlOut.Append(""",")


                        '  htmlOut.Append("""compweb"": """)
                        '  If Not IsDBNull(r.Item("WEB ADDRESS")) Then
                        '    htmlOut.Append("" & r.Item("WEB ADDRESS").ToString & " ")
                        '  End If
                        '  htmlOut.Append(""",")

                        '  htmlOut.Append("""compemail"": """)
                        '  If Not IsDBNull(r.Item("EMAIL")) Then
                        '    htmlOut.Append("" & r.Item("EMAIL").ToString & " ")
                        '  End If
                        '  htmlOut.Append(""",")

                        '  htmlOut.Append("""compoffice"": """)
                        '  If Not IsDBNull(r.Item("OFFICE PHONE")) Then
                        '    htmlOut.Append("" & r.Item("OFFICE PHONE").ToString & " ")
                        '  End If
                        '  htmlOut.Append(""",")

                        '  htmlOut.Append("""comp_id"": """)
                        '  If Not IsDBNull(r.Item("COMPID")) Then
                        '    htmlOut.Append("" & r.Item("COMPID").ToString & " ")
                        '  End If
                        '  htmlOut.Append(""",")


                        '  htmlOut.Append("""contactfirst"": """)
                        '  If Not IsDBNull(r.Item("FIRST NAME")) Then
                        '    htmlOut.Append("" & r.Item("FIRST NAME").ToString & " ")
                        '  End If
                        '  htmlOut.Append(""",")

                        '  htmlOut.Append("""contactlast"": """)
                        '  If Not IsDBNull(r.Item("LAST NAME")) Then
                        '    htmlOut.Append("" & r.Item("LAST NAME").ToString & " ")
                        '  End If
                        '  htmlOut.Append(""",")

                        '  htmlOut.Append("""contacttitle"": """)
                        '  If Not IsDBNull(r.Item("TITLE")) Then
                        '    htmlOut.Append("" & r.Item("TITLE").ToString & " ")
                        '  End If
                        '  htmlOut.Append(""",")

                        '  htmlOut.Append("""contactemail"": """)
                        '  If Not IsDBNull(r.Item("CONTACT EMAIL")) Then
                        '    htmlOut.Append("" & r.Item("CONTACT EMAIL").ToString & " ")
                        '  End If
                        '  htmlOut.Append(""",")

                        '  htmlOut.Append("""contactoffice"": """)
                        '  If Not IsDBNull(r.Item("CONTACT OFFICE PHONE")) Then
                        '    htmlOut.Append("" & r.Item("CONTACT OFFICE PHONE").ToString & " ")
                        '  End If
                        '  htmlOut.Append(""",")

                        '  htmlOut.Append("""contactmobile"": """)
                        '  If Not IsDBNull(r.Item("CONTACT MOBILE PHONE")) Then
                        '    htmlOut.Append("" & r.Item("CONTACT MOBILE PHONE").ToString & " ")
                        '  End If
                        '  htmlOut.Append(""",")

                        '  htmlOut.Append("""contact_id"": """)
                        '  If Not IsDBNull(r.Item("CONTACTID")) Then
                        '    htmlOut.Append("" & r.Item("CONTACTID").ToString & " ")
                        '  End If
                        '  htmlOut.Append(""",")


                        '  htmlOut.Append("""ac_id"": """)
                        '  If Not IsDBNull(r.Item("ACID")) Then
                        '    htmlOut.Append("" & r.Item("ACID").ToString & " ")
                        '  End If
                        '  'htmlOut.Append(""",")

                        '  htmlOut.Append("""")
                        '  htmlOut.Append("}")

                    ElseIf Trim(from_spot) = "pdf" Then
                        For Each r As DataRow In results_table.Rows

                            htmlOut.Append("<tr>")
                            htmlOut.Append("<td><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>")
                            htmlOut.Append(r.Item("Make").ToString & " " & r.Item("Model").ToString)
                            htmlOut.Append("</font></td>")

                            htmlOut.Append("<td><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>")
                            If UCase(r.Item("SerNbr")) = "BLOCKED" Then
                                htmlOut.Append("<span title='IDENTITY OF THIS AIRCRAFT IS BLOCKED BASED ON REQUEST OF OWNER/OPERATOR'>" & r.Item("SerNbr").ToString & "</span>")
                            Else
                                htmlOut.Append(r.Item("SerNbr").ToString)
                            End If
                            htmlOut.Append("</font></td>")

                            htmlOut.Append("<td><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>")
                            If UCase(r.Item("SerNbr")) = "BLOCKED" Then
                                htmlOut.Append("<span title='IDENTITY OF THIS AIRCRAFT IS BLOCKED BASED ON REQUEST OF OWNER/OPERATOR'>" & r.Item("SerNbr").ToString & "</span>")
                            Else
                                htmlOut.Append(r.Item("RegNbr").ToString)
                            End If
                            htmlOut.Append("</font></td>")

                            htmlOut.Append("<td><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>")

                            If Not IsDBNull(r.Item("ffd_origin_date")) Then
                                temp_date = r.Item("ffd_origin_date")
                                htmlOut.Append(Format(CDate(temp_date), "MM/dd/yy hh:mm tt"))
                            Else
                                temp_date = ""
                                htmlOut.Append("")
                            End If

                            htmlOut.Append("</font></td>")

                            htmlOut.Append("<td><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>")
                            '  htmlOut.Append("<a href='view_template.aspx?ViewID=28&ViewName=Operator/Airport Utilization&aport_id=" & r.Item("aport_id").ToString & "&comp_id=" & searchCriteria.ViewCriteriaCompanyID & "'>")
                            htmlOut.Append("" & r.Item("OriginAPort").ToString & " ")   ' </a>

                            If Not IsDBNull(r.Item("origin_aport_name")) Then
                                htmlOut.Append(" - " & Replace(r.Item("origin_aport_name").ToString, "International", "Intl.") & " (")
                            End If

                            If Not IsDBNull(r.Item("origin_aport_country")) Then
                                htmlOut.Append("" & Replace(r.Item("origin_aport_country").ToString, "United States", "U.S.") & " ")
                            End If

                            If Not IsDBNull(r.Item("origin_aport_city")) Then
                                htmlOut.Append("" & r.Item("origin_aport_city").ToString & " ")
                            End If

                            If Not IsDBNull(r.Item("origin_aport_state")) Then
                                htmlOut.Append(", " & r.Item("origin_aport_state").ToString)
                            End If

                            If Not IsDBNull(r.Item("origin_aport_name")) Or Not IsDBNull(r.Item("origin_aport_country")) Or Not IsDBNull(r.Item("origin_aport_city")) Or Not IsDBNull(r.Item("origin_aport_state")) Then
                                htmlOut.Append(")")
                            End If
                            htmlOut.Append("</td>")


                            htmlOut.Append("<td><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>")
                            If Not IsDBNull(r.Item("FlightTime").ToString) Then
                                htmlOut.Append("" & r.Item("FlightTime").ToString & " ")
                            End If
                            htmlOut.Append("</td>")

                            htmlOut.Append("<td><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>")
                            If Not IsDBNull(r.Item("Distance").ToString) Then
                                htmlOut.Append(r.Item("Distance").ToString & " ")
                            End If
                            htmlOut.Append("</td>")


                            htmlOut.Append("<td><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>")
                            If Not IsDBNull(r.Item("FuelBurn").ToString) Then
                                htmlOut.Append(r.Item("FuelBurn").ToString & " ")
                            End If
                            htmlOut.Append("</td>")


                            'htmlOut.Append("<td><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>")
                            'If Not IsDBNull(r.Item("COMMERCIAL_FIELD").ToString) Then
                            '  htmlOut.Append(r.Item("COMMERCIAL_FIELD").ToString & " ")
                            'End If
                            'htmlOut.Append("</td>") 


                            htmlOut.Append("</tr>")
                        Next
                    End If



                End If
            End If
            If Trim(from_spot) = "view" Then
                Results = htmlOut.ToString '(" var flightsDataSet = [ " & htmlOut.ToString & " ]; ")
            ElseIf Trim(from_spot) = "pdf" Then
                htmlOut.Append("</tbody></table></div></td></tr>")
                Results = htmlOut.ToString
            End If

        Catch ex As Exception

            aError = "Error in views_display_fractions_to_expire(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

        Finally
            out_htmlString = Results
        End Try

    End Sub
    Public Function RefuelJSArray(ByVal results_table As DataTable, ByVal searchCriteria As viewSelectionCriteriaClass) As String
        Dim table_count As Long = 0
        Dim htmlOut As New StringBuilder
        Dim results As String = ""
        Dim L2ARRIVEFLIGHTID As String = ""
        Dim L2ARRIVEDESTID As Long = 0

        Try
            If Not IsNothing(results_table) Then
                If results_table.Rows.Count > 0 Then
                    table_count = results_table.Rows.Count

                    For Each r As DataRow In results_table.Rows
                        L2ARRIVEFLIGHTID = ""
                        L2ARRIVEDESTID = 0


                        Dim secondFlight As String() = Split("", "")
                        If Not IsDBNull(r("SECONDFLIGHT")) Then
                            secondFlight = Split(r("SECONDFLIGHT"), ",")
                        End If



                        If (htmlOut.ToString.Trim) <> "" Then
                            htmlOut.Append(",")
                        End If
                        htmlOut.Append("{")
                        'htmlOut.Append("{ title: ""SEL"", width: ""20px"", data: ""check""}, ")
                        htmlOut.Append("""check"": """",")

                        htmlOut.Append("""ac"": """ & r.Item("Make").ToString & " " & r.Item("Model").ToString & """,")
                        If UCase(r.Item("SerNbr")) = "BLOCKED" Then
                            htmlOut.Append("""ser"": [""<span title='IDENTITY OF THIS AIRCRAFT IS BLOCKED BASED ON REQUEST OF OWNER/OPERATOR'>" & r.Item("SerNbr").ToString & "</span>"",""0""],")
                            htmlOut.Append("""reg"": ""<span title='IDENTITY OF THIS AIRCRAFT IS BLOCKED BASED ON REQUEST OF OWNER/OPERATOR'>" & r.Item("RegNbr").ToString & "</span>"",")
                        Else


                            If UBound(secondFlight) >= 10 Then
                                L2ARRIVEFLIGHTID = (Replace(secondFlight(10), "L2ARRIVEFLIGHTID:", ""))
                            End If

                            If UBound(secondFlight) >= 11 Then
                                L2ARRIVEDESTID = (Replace(secondFlight(11), "L2ARRIVEDESTID:", ""))
                            End If

                            htmlOut.Append((buildSerNoMenu(r.Item("SerNbr"), r.Item("acID"), r.Item("SERNOSORT_NONDISPLAY"), searchCriteria, r.Item("Flight_Id1"), L2ARRIVEFLIGHTID, r.Item("L1DEPARTID"), L2ARRIVEDESTID)))

                            'htmlOut.Append("""ser"": [""<a class='underline' onclick='javascript:openSmallWindowJS(\""DisplayAircraftDetail.aspx?acid=" + r.Item("ACId").ToString + "&jid=0\"",\""AircraftDetails\"");' title='Display Aircraft Details'>" & r.Item("SerNbr").ToString & "</a>"",""" & r.Item("SERNOSORT_NONDISPLAY").ToString & """],")
                            htmlOut.Append("""reg"": """ & r.Item("RegNbr").ToString & """,")
                        End If


                        htmlOut.Append("""based"": """)
                        If Not IsDBNull(r.Item("BaseAirport")) Then
                            htmlOut.Append(r.Item("BaseAirport").ToString)
                        End If
                        If Not IsDBNull(r.Item("BaseIATA")) Then
                            If Not IsDBNull(r.Item("BaseAirport")) Then
                                htmlOut.Append(" (")
                            End If
                            htmlOut.Append(r.Item("BaseIATA").ToString)
                            If Not IsDBNull(r.Item("BaseAirport")) Then
                                htmlOut.Append(")")
                            End If
                        End If

                        htmlOut.Append(""",")
                        htmlOut.Append("""depart1"": [""" & Format(CDate(r("L1DEPARTED")), "MM/dd/yy hh:mm tt") & """,""" & Format(CDate(r("L1DEPARTED")), "yyyy/MM/dd hh:mm tt") & """],")

                        htmlOut.Append("""departaport1"": """)

                        htmlOut.Append("<a href='view_template.aspx?ViewID=28&ViewName=Operator/Airport Utilization&aport_id=" & r.Item("L1DEPARTID").ToString & "&comp_id=" & searchCriteria.ViewCriteriaCompanyID.ToString & "'>")
                        htmlOut.Append(r.Item("L1DEPARTAPPORT").ToString & "</a> ")
                        htmlOut.Append(""",")

                        htmlOut.Append("""city1"": """)

                        If Not IsDBNull(r.Item("L1DEPARTCITY")) Then
                            htmlOut.Append(r.Item("L1DEPARTCITY").ToString & " ")
                        End If
                        htmlOut.Append(""",")

                        htmlOut.Append("""state1"": """)

                        If Not IsDBNull(r.Item("L1DEPARTSTATE")) Then
                            htmlOut.Append("" & r.Item("L1DEPARTSTATE").ToString)
                        End If
                        htmlOut.Append(""",")



                        htmlOut.Append("""originiata"": """)

                        If Not IsDBNull(r.Item("ORIGINIATA")) Then
                            htmlOut.Append("" & r.Item("ORIGINIATA").ToString)
                        End If
                        htmlOut.Append(""",")

                        htmlOut.Append("""originicao"": """)

                        If Not IsDBNull(r.Item("ORIGINICAO")) Then
                            htmlOut.Append(r.Item("ORIGINICAO").ToString)
                        End If
                        htmlOut.Append(""",")



                        htmlOut.Append("""country1"": """)
                        If Not IsDBNull(r.Item("L1DEPARTCOUNTRY")) Then
                            htmlOut.Append(Replace(r.Item("L1DEPARTCOUNTRY"), "United States", "USA").ToString & " ")
                        End If
                        htmlOut.Append(""",")

                        htmlOut.Append("""continent1"": """)
                        If Not IsDBNull(r.Item("origin_continent")) Then
                            htmlOut.Append(Replace(r.Item("origin_continent"), "United States", "USA").ToString & " ")
                        End If
                        htmlOut.Append(""",")


                        htmlOut.Append("""distance1"": """ & Replace(FormatNumber(flightDataFunctions.ConvertStatuteMileToNauticalMile(r("L1DISTANCE").ToString), 0), ",", "") & """,")

                        htmlOut.Append(vbNewLine)
                        htmlOut.Append("""arrived1"": [")
                        If Not IsDBNull(r("L1ARRIVED")) Then
                            htmlOut.Append("""" & Format(CDate(r("L1ARRIVED")), "MM/dd/yy hh:mm tt") & """, """ & Format(CDate(r("L1ARRIVED")), "yyyy/MM/dd hh:mm tt") & """")
                        Else
                            htmlOut.Append(""""",""""")
                        End If
                        htmlOut.Append("],")

                        htmlOut.Append(vbNewLine)
                        htmlOut.Append("""arrivedaport1"": """)
                        htmlOut.Append("<a href='view_template.aspx?ViewID=28&ViewName=Operator/Airport Utilization&aport_id=" & r.Item("L1ARRIVEDID").ToString & "&comp_id=" & searchCriteria.ViewCriteriaCompanyID.ToString & "'>")
                        htmlOut.Append(r.Item("L1ARRIVEDAPORT").ToString & "</a> ")
                        htmlOut.Append(""",")


                        htmlOut.Append("""city2"": """)

                        If Not IsDBNull(r.Item("L1ARRIVEDCITY")) Then
                            htmlOut.Append(r.Item("L1ARRIVEDCITY").ToString & " ")
                        End If
                        htmlOut.Append(""",")

                        htmlOut.Append("""state2"": """)
                        If Not IsDBNull(r.Item("L1ARRIVEDSTATE")) Then
                            htmlOut.Append("" & r.Item("L1ARRIVEDSTATE").ToString)
                        End If
                        htmlOut.Append(""",")

                        htmlOut.Append("""country2"": """)
                        If Not IsDBNull(r.Item("L1ARRIVEDCOUNTRY")) Then
                            htmlOut.Append(Replace(r.Item("L1ARRIVEDCOUNTRY"), "United States", "USA").ToString & " ")
                        End If
                        htmlOut.Append(""",")

                        htmlOut.Append("""continent2"": """)
                        If Not IsDBNull(r.Item("dest_continent")) Then
                            htmlOut.Append(Replace(r.Item("dest_continent"), "United States", "USA").ToString & " ")
                        End If
                        htmlOut.Append(""",")


                        htmlOut.Append("""destiata"": """)

                        If Not IsDBNull(r.Item("DESTIATA")) Then
                            htmlOut.Append("" & r.Item("DESTIATA").ToString)
                        End If
                        htmlOut.Append(""",")


                        htmlOut.Append("""desticao"": """)

                        If Not IsDBNull(r.Item("DESTICAO")) Then
                            htmlOut.Append("" & r.Item("DESTICAO").ToString)
                        End If
                        htmlOut.Append(""",")


                        'On Ground
                        htmlOut.Append("""onground"": """)
                        If UBound(secondFlight) >= 0 Then
                            htmlOut.Append(Replace(secondFlight(0), "ONGROUND:", ""))
                        End If
                        htmlOut.Append(""",")

                        htmlOut.Append(vbNewLine)
                        htmlOut.Append("""departed2"": [")

                        If UBound(secondFlight) >= 1 Then
                            If Not IsDBNull(Replace(secondFlight(1), "L2DEPARTED:", "")) Then
                                If Trim(Replace(secondFlight(1), "L2DEPARTED:", "")) <> "" Then
                                    htmlOut.Append("""" & Format(CDate(Replace(secondFlight(1), "L2DEPARTED:", "")), "MM/dd/yy hh:mm tt") & """, """ & Format(CDate(Replace(secondFlight(1), "L2DEPARTED:", "")), "yyyy/MM/dd hh:mm tt") & """")
                                Else
                                    htmlOut.Append(""""",""""")
                                End If
                            Else
                                htmlOut.Append(""""",""""")
                            End If
                        Else
                            htmlOut.Append(""""",""""")
                        End If

                        htmlOut.Append("],")

                        'Distance
                        htmlOut.Append("""distance2"": """)
                        If UBound(secondFlight) >= 2 Then
                            htmlOut.Append(Replace(FormatNumber(flightDataFunctions.ConvertStatuteMileToNauticalMile(Replace(secondFlight(2), "L2DISTANCE:", "")), 0), ",", ""))
                        End If
                        htmlOut.Append(""",")
                        'Arrived 

                        htmlOut.Append(vbNewLine)
                        htmlOut.Append("""arrived2"": [")
                        If UBound(secondFlight) >= 3 Then
                            If Not IsDBNull(Replace(secondFlight(3), "L2ARRIVED:", "")) Then
                                If Trim(Replace(secondFlight(3), "L2ARRIVED:", "")) <> "" Then
                                    htmlOut.Append("""" & Format(CDate(Replace(secondFlight(3), "L2ARRIVED:", "")), "MM/dd/yy hh:mm tt") & """, """ & Format(CDate(Replace(secondFlight(3), "L2ARRIVED:", "")), "yyyy/MM/dd hh:mm tt") & """")
                                Else
                                    htmlOut.Append(""""",""""")
                                End If
                            Else
                                htmlOut.Append(""""",""""")
                            End If
                        Else
                            htmlOut.Append(""""",""""")
                        End If
                        htmlOut.Append("],")


                        htmlOut.Append("""arrivedaport2"": """)
                        If UBound(secondFlight) >= 4 Then
                            htmlOut.Append(Replace(secondFlight(4), "L2ARRIVEAPORT:", ""))
                        End If
                        htmlOut.Append("""")
                        htmlOut.Append(",")

                        htmlOut.Append("""arrivediata"": """)
                        If UBound(secondFlight) >= 8 Then
                            htmlOut.Append(Replace(secondFlight(8), "L2ARRIVEIATA:", ""))
                        End If
                        htmlOut.Append("""")
                        htmlOut.Append(",")

                        htmlOut.Append("""arrivedicao"": """)
                        If UBound(secondFlight) >= 9 Then
                            htmlOut.Append(Replace(secondFlight(9), "L2ARRIVEICAO:", ""))
                        End If
                        htmlOut.Append("""")
                        htmlOut.Append(",")

                        htmlOut.Append("""arrivedcity"": """)
                        If UBound(secondFlight) >= 5 Then
                            htmlOut.Append(Replace(secondFlight(5), "L2ARRIVECITY:", ""))
                        End If
                        htmlOut.Append("""")
                        htmlOut.Append(",")

                        htmlOut.Append("""arrivedstate"": """)
                        If UBound(secondFlight) >= 6 Then
                            htmlOut.Append(Replace(secondFlight(6), "L2ARRIVESTATE:", ""))
                        End If
                        htmlOut.Append("""")
                        htmlOut.Append(",")

                        htmlOut.Append("""arrivedcountry"": """)
                        If UBound(secondFlight) >= 7 Then
                            htmlOut.Append(Replace(secondFlight(7), "L2ARRIVCOUNTRY:", ""))
                        End If
                        htmlOut.Append("""")


                        htmlOut.Append("}")
                    Next

                End If
            End If



            results = (" var refuelDataSet = [ " & htmlOut.ToString & " ]; ")


        Catch ex As Exception

            aError = "Error in utilFunctions RefuelJSArray " + ex.Message

        Finally

        End Try
        Return results
    End Function
    Public Sub get_most_recent_flight_activity_top_function(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByVal aport_name As String, ByVal run_export As String, ByRef title_text As String, ByRef title_text2 As String, ByVal selected_value As String, ByVal recent_flight_months As Integer, ByVal start_date As String, ByVal end_date As String, ByVal product_code_selection As String, ByVal product_link As String, ByRef table_count As Long)

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False
        Dim same_country As Boolean = False
        Dim temp_date As String = ""
        Dim i As Integer = 0

        Try

            results_table = get_most_recent_flight_activity(searchCriteria, run_export, selected_value, recent_flight_months, start_date, end_date, product_code_selection, True)


            If Trim(run_export) <> "" Then
                crmViewDataLayer.ExportTableData(results_table)
            Else



                If Not IsNothing(results_table) Then
                    If results_table.Rows.Count > 0 Then
                        table_count = results_table.Rows.Count
                        htmlOut.Append("<table id='modelForsaleViewOuterTable' width=""100%"" cellpadding=""0"" cellspacing=""0"" class=""module"">")
                        htmlOut.Append("<tr valign='top'><td align='center' valign='top' class='header'>")


                        If Trim(start_date) <> "" And Trim(end_date) <> "" Then
                            htmlOut.Append("&nbsp;Operators Flight Activity at " & aport_name & " (" & start_date & " - " & end_date & ")")
                        ElseIf searchCriteria.ViewCriteriaCompanyID > 0 Then
                            htmlOut.Append("&nbsp;Operator Flight Activity at " & aport_name & " in the Last Year")
                        Else
                            htmlOut.Append("&nbsp;Operators Flight Activity at " & aport_name & " (Last 30 Days) ")
                        End If

                        htmlOut.Append("</td></tr>")


                        htmlOut.Append("<tr><td align=""left"" valign=""top"">")

                        htmlOut.Append("<table id='tableCopy' width='100%' cellpadding='0' cellspacing='0' border='0' align='left'><thead>")

                        htmlOut.Append(" <th>SEL</th>")
                        htmlOut.Append("<th>Aircraft</th>")
                        htmlOut.Append("<th>Ser#</th>")
                        htmlOut.Append("<th>Reg#</th>")

                        If Trim(airport_direction) = "D" Then
                            If Trim(selected_value) = "" Or Trim(selected_value) = "A" Then
                                htmlOut.Append("<th valign=""top"" align=""left"" class=""seperator""><strong>Date</strong></th>")
                                htmlOut.Append("<th valign=""top"" align=""left"" class=""seperator""><strong>Origin Airport</strong></th>")
                            ElseIf Trim(selected_value) = "D" Then
                                htmlOut.Append("<th valign=""top"" align=""left"" class=""seperator""><strong>Date</strong></th>")
                                htmlOut.Append("<th valign=""top"" align=""left"" class=""seperator""><strong>Destin Airport</strong></th>")
                            End If
                        ElseIf Trim(airport_direction) = "O" Then
                            If Trim(selected_value) = "" Or Trim(selected_value) = "A" Then
                                htmlOut.Append("<th valign=""top"" align=""left"" class=""seperator""><strong>Date</strong></th>")
                                htmlOut.Append("<th valign=""top"" align=""left"" class=""seperator""><strong>Destin Airport</strong></th>")  ' switched these
                            ElseIf Trim(selected_value) = "D" Then
                                htmlOut.Append("<th valign=""top"" align=""left"" class=""seperator""><strong>Date</strong></th>")
                                htmlOut.Append("<th valign=""top"" align=""left"" class=""seperator""><strong>Origin Airport</strong></th>") ' switched these
                            End If
                        Else
                            If Trim(selected_value) = "" Or Trim(selected_value) = "A" Then
                                htmlOut.Append("<th valign=""top"" align=""left"" class=""seperator""><strong>Date</strong></th>")
                                htmlOut.Append("<th valign=""top"" align=""left"" class=""seperator""><strong>Origin Airport</strong></th>")
                            ElseIf Trim(selected_value) = "D" Then
                                htmlOut.Append("<th valign=""top"" align=""left"" class=""seperator""><strong>Date</strong></th>")
                                htmlOut.Append("<th valign=""top"" align=""left"" class=""seperator""><strong>Destin Airport</strong></th>")
                            End If
                        End If









                        htmlOut.Append("<th>Flight Time/<br/>Dist(nm)</th>")

                        htmlOut.Append("</thead><tbody>")

                        For Each r As DataRow In results_table.Rows

                            htmlOut.Append("<td class=""text_align_center""></td>") ' for sel 

                            htmlOut.Append("<td class=""text_align_left"">")
                            htmlOut.Append("" & r.Item("Make").ToString & " ")
                            htmlOut.Append("" & r.Item("Model").ToString & "")
                            htmlOut.Append("</td>")

                            htmlOut.Append("<td class=""text_align_left"">")
                            htmlOut.Append("<a class='underline' onclick=""javascript:load('DisplayAircraftDetail.aspx?acid=" + r.Item("AC_ID").ToString + "&jid=0','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');"" title='Display Aircraft Details'>")
                            htmlOut.Append("" & r.Item("SerNbr").ToString & "</a> ")
                            htmlOut.Append("</td>")

                            htmlOut.Append("<td class=""text_align_left"">")
                            htmlOut.Append("" & r.Item("RegNbr").ToString & " ")
                            htmlOut.Append("</td>")


                            htmlOut.Append("<td align='left'>")

                            If Trim(selected_value) = "" Or Trim(selected_value) = "A" Then
                                'If Not IsDBNull(r.Item("aport_country")) And Not IsDBNull(r.Item("aport_country2")) Then
                                '  If Trim(r.Item("aport_country")) = Trim(r.Item("aport_country2")) Then
                                '    same_country = True
                                '  End If
                                'End If

                                'htmlOut.Append("<td align=""left"" valign=""top"" class=""seperator"">")
                                'htmlOut.Append("<a href='view_template.aspx?ViewID=24&ViewName=Airport FBO View&aport_id=" & r.Item("aport_id").ToString & "'>")
                                'htmlOut.Append("" & r.Item("OriginAPort").ToString & "</a>")

                                'If Not IsDBNull(r.Item("aport_name")) Then
                                '  htmlOut.Append(" - " & r.Item("aport_name").ToString & " (")
                                'End If

                                'If same_country = False Then
                                '  If Not IsDBNull(r.Item("aport_country")) Then
                                '    htmlOut.Append("" & r.Item("aport_country").ToString & " ")
                                '  End If
                                'End If


                                'If Not IsDBNull(r.Item("aport_city")) Then
                                '  htmlOut.Append("" & r.Item("aport_city").ToString & " ")
                                'End If

                                'If Not IsDBNull(r.Item("aport_state")) Then
                                '  htmlOut.Append(", " & r.Item("aport_state").ToString)
                                'End If

                                'If Not IsDBNull(r.Item("aport_name")) Or Not IsDBNull(r.Item("aport_country")) Or Not IsDBNull(r.Item("aport_city")) Or Not IsDBNull(r.Item("aport_state")) Then
                                '  htmlOut.Append(")")
                                'End If


                                'htmlOut.Append("&nbsp;</td>")

                                'htmlOut.Append("<td align=""left"" valign=""top"" class=""seperator"">")
                                'htmlOut.Append("<a href='view_template.aspx?ViewID=24&ViewName=Airport FBO View&aport_id=" & r.Item("aport_id2").ToString & "'>")
                                'htmlOut.Append("" & r.Item("DestinAPort").ToString & "</a> ")

                                'If Not IsDBNull(r.Item("aport_name2")) Then
                                '  htmlOut.Append(" - " & r.Item("aport_name2").ToString & " (")
                                'End If

                                'If same_country = False Then
                                '  If Not IsDBNull(r.Item("aport_country2")) Then
                                '    htmlOut.Append("" & r.Item("aport_country2").ToString & " ")
                                '  End If
                                'End If


                                'If Not IsDBNull(r.Item("aport_city2")) Then
                                '  htmlOut.Append("" & r.Item("aport_city2").ToString & " ")
                                'End If

                                'If Not IsDBNull(r.Item("aport_state2")) Then
                                '  htmlOut.Append(", " & r.Item("aport_state2").ToString)
                                'End If


                                'If Not IsDBNull(r.Item("aport_name2")) Or Not IsDBNull(r.Item("aport_country2")) Or Not IsDBNull(r.Item("aport_city2")) Or Not IsDBNull(r.Item("aport_state2")) Then
                                '  htmlOut.Append(")")
                                'End If

                                'htmlOut.Append("&nbsp;</td>")
                                temp_date = ""
                                If Not IsDBNull(r.Item("ffd_origin_date")) Then
                                    temp_date = r.Item("ffd_origin_date")
                                    temp_date = Replace(Trim(temp_date), ":00 PM", " PM")
                                    temp_date = Replace(Trim(temp_date), ":00 AM", " AM")

                                    For i = 2012 To 2025
                                        temp_date = Replace(Trim(temp_date), "/" & Trim(i), "/" & Right(Trim(i), 2))
                                    Next

                                    htmlOut.Append(temp_date)
                                End If
                                'ffd_dest_date, , 

                                htmlOut.Append("&nbsp;</td>")
                                htmlOut.Append("<td align=""left"" valign=""top"">")
                                htmlOut.Append("<a href='view_template.aspx?ViewID=28&ViewName=Operator/Airport Utilization&aport_id=" & r.Item("aport_id").ToString & "&comp_id=" & searchCriteria.ViewCriteriaCompanyID & "'>")
                                htmlOut.Append("" & r.Item("OriginAPort").ToString & "</a> ")

                                If Not IsDBNull(r.Item("aport_name")) Then
                                    htmlOut.Append(" - " & r.Item("aport_name").ToString & " (")
                                End If


                                If Not IsDBNull(r.Item("aport_country")) Then
                                    htmlOut.Append("" & r.Item("aport_country").ToString & " ")
                                End If

                                If Not IsDBNull(r.Item("aport_city")) Then
                                    htmlOut.Append("" & r.Item("aport_city").ToString & " ")
                                End If

                                If Not IsDBNull(r.Item("aport_state")) Then
                                    htmlOut.Append(", " & r.Item("aport_state").ToString)
                                End If

                                If Not IsDBNull(r.Item("aport_name")) Or Not IsDBNull(r.Item("aport_country")) Or Not IsDBNull(r.Item("aport_city")) Or Not IsDBNull(r.Item("aport_state")) Then
                                    htmlOut.Append(")")
                                End If
                                htmlOut.Append("&nbsp;</td>")
                                'ElseIf Trim(selected_value) = "D" Then

                                '  If Not IsDBNull(r.Item("ffd_dest_date")) Then
                                '    htmlOut.Append(r.Item("ffd_dest_date"))
                                '  End If
                                '  htmlOut.Append("</td>")

                                '  htmlOut.Append("<td align=""left"" valign=""top"" class=""seperator"">")
                                '  htmlOut.Append("<a href='view_template.aspx?ViewID=24&ViewName=Airport FBO View&aport_id=" & r.Item("aport_id2").ToString & "'>")
                                '  htmlOut.Append("" & r.Item("DestinAPort").ToString & "</a> ")

                                '  If Not IsDBNull(r.Item("aport_name2")) Then
                                '    htmlOut.Append(" - " & r.Item("aport_name2").ToString & " (")
                                '  End If

                                '  If Not IsDBNull(r.Item("aport_country2")) Then
                                '    htmlOut.Append("" & r.Item("aport_country2").ToString & " ")
                                '  End If

                                '  If Not IsDBNull(r.Item("aport_city2")) Then
                                '    htmlOut.Append("" & r.Item("aport_city2").ToString & " ")
                                '  End If

                                '  If Not IsDBNull(r.Item("aport_state2")) Then
                                '    htmlOut.Append(", " & r.Item("aport_state2").ToString)
                                '  End If

                                '  If Not IsDBNull(r.Item("aport_name2")) Or Not IsDBNull(r.Item("aport_country2")) Or Not IsDBNull(r.Item("aport_city2")) Or Not IsDBNull(r.Item("aport_state2")) Then
                                '    htmlOut.Append(")")
                                '  End If

                                '  htmlOut.Append("&nbsp;</td>")
                            End If



                            htmlOut.Append("<td align=""right"" valign=""top"">")
                            If Not IsDBNull(r.Item("FlightTime").ToString) Then
                                htmlOut.Append("" & r.Item("FlightTime").ToString & " ")
                            End If
                            htmlOut.Append("&nbsp;/&nbsp;")

                            If Not IsDBNull(r.Item("Distance").ToString) Then
                                htmlOut.Append("" & flightDataFunctions.ConvertStatuteMileToNauticalMile(r.Item("Distance").ToString) & " ")
                            End If
                            htmlOut.Append("&nbsp;</td>")

                            htmlOut.Append("</tr>")

                        Next


                        htmlOut.Append("</tbody></table>")
                        htmlOut.Append("<div id=""forSaleInnerTable"" style=""width: 930px;""></div>")
                        htmlOut.Append("</td></tr></table>")
                    Else
                        htmlOut.Append("<table><tr><td valign=""top"" align=""left"" class=""seperator"" style=""padding-left:3px;"">No FAA Flight Activity Found.</td></tr></table>")
                    End If
                Else
                    htmlOut.Append("<table><tr><td valign=""top"" align=""left"" class=""seperator"" style=""padding-left:3px;"">No FAA Flight Activity Found.</td></tr></table>")
                End If
            End If



        Catch ex As Exception

            aError = "Error in views_display_fractions_to_expire(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub
    Public Sub util_get_operator_airports_top_function(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef htmlOut_return As String, ByVal aport_id As Long, ByVal selected_value As String, ByRef table_count As Long, ByVal product_code_selection As String, ByVal from_spot As String, ByRef airportTab As Boolean, Optional ByVal page_break_plus_header As String = "")
        Dim results_table As New DataTable
        Dim toggleRowColor As Boolean = False
        Dim last_comp_id As Long = 0
        Dim comp_count As Long = 0
        Dim htmlOut As New StringBuilder
        Dim htmlOut_header As New StringBuilder
        Dim htmlout_java As New StringBuilder

        Dim font_text_title As String = ""
        Dim font_text_start As String = ""
        Dim font_text_end As String = ""
        Dim temp_dir As String = "right"
        Dim bgcolor As String = ""
        Dim page_break_after As Integer = 55


        Try
            If Trim(from_spot) = "pdf" Then
                font_text_start = "<font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>"
                font_text_title = "<font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT_TITLE") & "'>"
                font_text_end = "</font>"
                temp_dir = "right"
            Else
                font_text_start = ""
                font_text_title = ""
                font_text_end = ""
            End If


            results_table = get_most_common_origins(searchCriteria, product_code_selection, True, airportTab, 0, 0, "", from_spot)

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then
                    table_count = results_table.Rows.Count


                    If Trim(from_spot) = "company" Then
                        htmlOut_header.Append("<div class=""Box""><div class=""subHeader"">Airport Utilization (Arrivals - 6 Months)</div><br /><table id='modelForsaleViewOuterTable' width=""100%"" cellpadding=""1"" cellspacing=""0""  class='formatTable blue small'>")
                        htmlOut_header.Append("<tr class='header_row'>")
                    ElseIf Trim(from_spot) = "pdf" Then
                        ' htmlOut_header.Append("<table id='modelForsaleViewOuterTable' width=""100%"" cellpadding=""1"" cellspacing=""0""  class='data_aircraft_grid'>")
                        htmlOut_header.Append("<tr><td valign=""top"" align=""center"" class=""header"" colspan='12'><font class='" & HttpContext.Current.Session("FONT_CLASS_HEADER") & "'>AIRPORT UTILIZATION</font></td></tr>")
                        htmlOut_header.Append("<tr class='header_row'>")
                    Else
                        htmlOut_header.Append("<table id='modelForsaleViewOuterTable' width=""100%"" cellpadding=""0"" cellspacing=""0"" class=""module"">")
                        htmlOut_header.Append("<tr valign='top'><td valign='top' class='header' align='center'>Operator Airports Arrived from in the Last Year</td></tr>")
                        htmlOut_header.Append("<tr><td align=""left"" valign=""top"">")
                    End If


                    If Trim(from_spot) = "pdf" Or Trim(from_spot) = "company" Then
                    Else
                        htmlOut_header.Append("<table id='tableCopy' cellpadding='0' cellspacing='0' border='0' align='left'><thead>")
                    End If

                    If Trim(from_spot) = "pdf" Then
                        htmlOut_header.Append("<td width='400' align='left'>" & font_text_title & "IATA/ICAO - Airport Name" & font_text_end & "</th>")
                        htmlOut_header.Append("<td width='50' align='" & temp_dir & "'>" & font_text_title & "#<br/>Flts" & font_text_end & "</th>")
                        htmlOut_header.Append("<td width='50' align='" & temp_dir & "'>" & font_text_title & "Total Flight Hrs" & font_text_end & "</th>")
                        htmlOut_header.Append("<td width='50' align='" & temp_dir & "'>" & font_text_title & "Est. Fuel Burn (Gal)" & font_text_end & "</th>")
                    ElseIf Trim(from_spot) = "company" Then
                        htmlOut_header.Append("<th width='390' align='left'>" & font_text_title & "IATA/ICAO<br />Airport Name" & font_text_end & "</th>")
                        htmlOut_header.Append("<th width='50' align='" & temp_dir & "'>" & font_text_title & "# Flts" & font_text_end & "</th>")
                        htmlOut_header.Append("<th width='50' align='" & temp_dir & "'>" & font_text_title & "Total Flight Hrs" & font_text_end & "</th>")
                        htmlOut_header.Append("<th width='50' align='" & temp_dir & "'>" & font_text_title & "Est. Fuel Burn (Gal)" & font_text_end & "</th>")
                    Else
                        htmlOut_header.Append(" <th>SEL</th>")
                        htmlOut_header.Append("<th>IATA</th>")
                        htmlOut_header.Append("<th>ICAO</th>")
                        htmlOut_header.Append("<th width='400'>Airport Name</th>")
                        htmlOut_header.Append("<th>Nbr Flights</th>")
                        htmlOut_header.Append("<th>Total Flight Hrs</th>")
                        htmlOut_header.Append("<th><a href='#' title='Estimated Gallons of Fuel Burned' alt='Estimated Gallons of Fuel Burned'> Est. Fuel Burn (Gal)</a></th>")
                    End If



                    htmlOut_header.Append("</thead><tbody>")

                    htmlOut.Append(htmlOut_header.ToString)


                    For Each r As DataRow In results_table.Rows

                        If Trim(from_spot) = "pdf" Then

                            comp_count = comp_count + 1

                            If comp_count > page_break_after Then
                                htmlOut.Append("</tbody></table>")
                                htmlOut.Append("</td></tr></table>")
                                htmlOut.Append(page_break_plus_header)
                                htmlOut.Append(htmlOut_header.ToString)
                                comp_count = 0
                            End If


                            If bgcolor = "" Then
                                bgcolor = "#f0f0f0"
                            Else
                                bgcolor = ""
                            End If

                            htmlOut.Append("<tr bgcolor='" & bgcolor & "'  valign='top'>")
                        ElseIf Trim(from_spot) = "company" Then
                            If Not toggleRowColor Then
                                htmlOut.Append("<tr class=""alt_row"">")
                                toggleRowColor = True
                            Else
                                htmlOut.Append("<tr bgcolor=""white"">")
                                toggleRowColor = False
                            End If
                        Else
                            htmlOut.Append("<td></td>")
                        End If


                        If Trim(from_spot) = "pdf" Then

                        ElseIf Trim(from_spot) = "company" Then
                            htmlOut.Append("<a href='DisplayCompanyDetail.aspx?amod_id=" & searchCriteria.ViewCriteriaAmodID & "&use_insight_op=Y&aport_id" & r.Item("ffd_dest_aport_id").ToString & "&compid=" & searchCriteria.ViewCriteriaCompanyID & "'>")
                        Else
                            htmlOut.Append("<a href='view_template.aspx?ViewID=28&ViewName=Operator/Airport Utilization&aport_id=" & r.Item("ffd_dest_aport_id").ToString & "&comp_id=" & searchCriteria.ViewCriteriaCompanyID & "'>")
                        End If

                        If Trim(from_spot) = "company" Or Trim(from_spot) = "pdf" Then
                            htmlOut.Append("<td class=""text_align_left"" valign=""top"" width='400'>")
                            If Trim(from_spot) = "company" Then
                                htmlOut.Append("<A class='underline text_underline' onclick=""javascript:load('view_template.aspx?ViewID=28&ViewName=Operator/Airport Utilization&comp_id=" & searchCriteria.ViewCriteriaCompanyID & "&aport_id=" & r.Item("ffd_dest_aport_id").ToString & "&noMaster=false&display_flight=Y','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');"">")
                            End If

                            htmlOut.Append(font_text_start & "")
                            htmlOut.Append("" & r.Item("dest_aport_iata_code").ToString & " / ")
                            htmlOut.Append("" & r.Item("dest_aport_icao_code").ToString & "</a> - ")

                            htmlOut.Append("" & r.Item("dest_aport_name").ToString & " (" & Replace(r.Item("dest_aport_country").ToString, "United States", "U.S.") & " - " & r.Item("dest_aport_city").ToString & " " & r.Item("dest_aport_state").ToString & ")" & font_text_end & "</td>")

                        Else
                            htmlOut.Append("<td class=""text_align_left"" valign=""top"" >")
                            htmlOut.Append("" & r.Item("dest_aport_iata_code").ToString & "</a></td>")
                            htmlOut.Append("<td class=""text_align_left"" valign=""top"" >")
                            htmlOut.Append("" & r.Item("dest_aport_icao_code").ToString & "</td>")

                            htmlOut.Append("<td class=""text_align_left"" valign=""top"" width='400'>")
                            htmlOut.Append("" & r.Item("dest_aport_name").ToString & " (" & Replace(r.Item("dest_aport_country").ToString, "United States", "U.S.") & " - " & r.Item("dest_aport_city").ToString & " " & r.Item("dest_aport_state").ToString & ")</td>")
                        End If

                        If Trim(from_spot) = "company" Then
                            htmlOut.Append("<td width='50'  align='" & temp_dir & "' class=""text_align_right""  valign=""top""><A class='underline text_underline' onclick=""javascript:load('view_template.aspx?ViewID=28&ViewName=Operator/Airport Utilization&comp_id=" & searchCriteria.ViewCriteriaCompanyID & "&aport_id=" & r.Item("ffd_dest_aport_id").ToString & "&noMaster=false&display_flight=Y','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');"">" & font_text_start & "" & FormatNumber(r("NbrFlights"), 0) & "" & font_text_end & "</a></td>")
                        Else
                            htmlOut.Append("<td width='50'  align='" & temp_dir & "' class=""text_align_right"">" & font_text_start & "" & FormatNumber(r("NbrFlights"), 0) & "" & font_text_end & "</td>")
                        End If

                        htmlOut.Append("<td  width='50'  align='" & temp_dir & "' class=""text_align_right"" " & IIf(Trim(from_spot) = "company", " valign=""top""", "") & ">" & font_text_start & "" & FormatNumber(r("TotalFlightTimeHrs"), 1) & "" & font_text_end & "</td>")
                        htmlOut.Append("<td align='right'  align='" & temp_dir & "' width='50'  class=""text_align_right"" " & IIf(Trim(from_spot) = "company", " valign=""top""", "") & ">" & font_text_start & "" & FormatNumber(r("TotalFuelBurn"), 0) & "" & font_text_end & "</td>")

                        htmlOut.Append("</tr>")

                    Next

                    htmlOut.Append("</tbody></table>")

                    If Trim(from_spot) = "company" Then
                        htmlOut.Append("</div>")
                    Else
                        htmlOut.Append("<div id=""forSaleInnerTable"" style=""width:930px;""></div>")
                        htmlOut.Append("</td></tr></table>")
                    End If



                Else
                    htmlOut.Append("<table><tr><td valign=""top"" align=""left"" class=""seperator"" style=""padding-left:3px;"">No FAA Flight Activity Found.</td></tr></table>")
                End If
            Else
                htmlOut.Append("<table><tr><td valign=""top"" align=""left"" class=""seperator"" style=""padding-left:3px;"">No FAA Flight Activity Found.</td></tr></table>")
            End If



        Catch ex As Exception

            aError = "Error in views_display_fractions_to_expire(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        'return resulting html string
        htmlOut_return = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing


    End Sub
    Public Sub util_get_operators_rollup_top_function(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef htmlOut_return As String, ByVal aport_id As Long, ByVal selected_value As String, ByRef table_count As Long, ByVal from_spot As String)

        Dim results_table As New DataTable
        Dim toggleRowColor As Boolean = False
        Dim last_comp_id As Long = 0
        Dim comp_count As Long = 0
        Dim htmlOut As New StringBuilder
        Dim htmlout_java As New StringBuilder
        Dim tot_jet As Long = 0
        Dim tot_turbo As Long = 0
        Dim tot_piston As Long = 0
        Dim tot_comm As Long = 0
        Dim tot_helo As Long = 0
        Dim tot_op As Long = 0
        Dim tot_broker As Long = 0

        Dim count_current_owner As Long = 0
        Dim count_past_owner As Long = 0
        Dim count_operator As Long = 0
        Dim count_manu As Long = 0
        Dim count_dealer As Long = 0
        Dim count_locations As Long = 0
        Dim count_lease As Long = 0
        Dim count_finanace As Long = 0

        Dim col1 As Long = 0
        Dim font_start As String = ""
        Dim font_start_title As String = ""
        Dim font_end As String = ""
        Dim bgcolor As String = ""
        Dim talign As String = "right"


        Try

            If Trim(from_spot) = "pdf" Then
                font_start = "<font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>"
                font_start_title = "<font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT_TITLE") & "'>"
                font_end = "</font>"
                talign = "right"
            Else
                font_start = ""
                font_start_title = ""
                font_end = ""
            End If

            results_table = util_get_opearators_rollup(searchCriteria, aport_id)


            Call get_company_profile_top_function(searchCriteria, "company", count_current_owner, count_past_owner, count_operator, count_manu, count_dealer, count_locations, count_lease, count_finanace, "Y")


            If Not IsNothing(results_table) Then
                If results_table.Rows.Count > 0 Then
                    table_count = results_table.Rows.Count

                    If Trim(from_spot) = "pdf" Then
                        htmlOut.Append("<tr><td valign=""top"" align=""center"" class=""header""><font class='" & HttpContext.Current.Session("FONT_CLASS_HEADER") & "'>Operating Location</font></td></tr>")
                    Else
                        htmlOut.Append("<table id='modelForsaleViewOuterTable' width=""100%"" cellpadding=""0"" cellspacing=""0""class='formatTable blue'>")
                    End If


                    htmlOut.Append("<tr class='header_row noBorder'>")
                    htmlOut.Append("<th colspan='1' align='right'>&nbsp;</th>")

                    'colspan will change 
                    If count_current_owner > 0 Then
                        If HttpContext.Current.Session.Item("localSubscription").crmTierlevel = "ALL" And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True Then
                            htmlOut.Append("<th colspan='5' align='center'><table border='1' cellpadding='2' cellspacing='0' width='100%' align='center'><tr><td align='center'>" & font_start_title & "Ownership" & font_end & "</td></tr></table></th>")
                        Else
                            If HttpContext.Current.Session.Item("localSubscription").crmJets_Flag = True Then
                                col1 = col1 + 1
                            End If
                            If HttpContext.Current.Session.Item("localSubscription").crmTurboprops = True Then
                                col1 = col1 + 1
                            End If
                            col1 = col1 + 1 ' piston
                            If HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
                                col1 = col1 + 1
                            End If
                            If HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True Then
                                col1 = col1 + 1
                            End If
                            htmlOut.Append("<th colspan='" & col1 & "' align='center'><table border='1' cellpadding='2' cellspacing='0' width='100%' align='center'><tr><td align='center'>" & font_start_title & "Ownership" & font_end & "</td></tr></table></th>")
                        End If
                    Else

                    End If

                    htmlOut.Append("<th colspan='2' align='right'>&nbsp;</th>")
                    htmlOut.Append("</tr>")

                    htmlOut.Append("<tr class='header_row noBorder'>")

                    If use_owner = True Then
                        If count_current_owner = 0 Then ' addded for companies like freesteram who are just brokers
                            htmlOut.Append("<th width='370' align='left'>" & font_start_title & "Owner (Location)" & font_end & "</th>")
                        Else
                            htmlOut.Append("<th width='250' align='left'>" & font_start_title & "Owner (Location)" & font_end & "</th>")
                        End If
                    Else
                        If count_current_owner = 0 Then ' addded for companies like freesteram who are just brokers
                            htmlOut.Append("<th width='370' align='left' class=""subHeader"">" & font_start_title & "Company (Location)" & font_end & "</th>")
                        Else
                            htmlOut.Append("<th width='250' align='left' class=""subHeader"">" & font_start_title & "Company (Location)" & font_end & "</th>")
                        End If
                    End If




                    If count_current_owner > 0 Then
                        If HttpContext.Current.Session.Item("localSubscription").crmTierlevel = "ALL" And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True Then
                            htmlOut.Append("<th align='" & talign & "'>" & font_start_title & "Jet&nbsp;" & font_end & "</th>")
                            htmlOut.Append("<th align='" & talign & "'>" & font_start_title & "Turbo&nbsp;" & font_end & "</th>")
                            htmlOut.Append("<th align='" & talign & "'>" & font_start_title & "Pist&nbsp;" & font_end & "</th>")
                            htmlOut.Append("<th align='" & talign & "'>" & font_start_title & "Comm&nbsp;" & font_end & "</th>")
                            htmlOut.Append("<th align='" & talign & "'>" & font_start_title & "Helo&nbsp;" & font_end & "</th>")
                        Else
                            If HttpContext.Current.Session.Item("localSubscription").crmJets_Flag = True Then
                                htmlOut.Append("<th align='" & talign & "'>" & font_start_title & "Jet&nbsp;" & font_end & "</th>")
                            End If
                            If HttpContext.Current.Session.Item("localSubscription").crmTurboprops = True Then
                                htmlOut.Append("<th align='" & talign & "'>" & font_start_title & "Turbo&nbsp;" & font_end & "</th>")
                            End If
                            htmlOut.Append("<th align='" & talign & "'>Pist&nbsp;</th>")

                            If HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
                                htmlOut.Append("<th align='" & talign & "'>" & font_start_title & "Comm&nbsp;" & font_end & "</th>")
                            End If
                            If HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True Then
                                htmlOut.Append("<th align='" & talign & "'>" & font_start_title & "Helo&nbsp;" & font_end & "</th>")
                            End If
                        End If


                    End If


                    If count_operator > 0 Then
                        htmlOut.Append("<th align='" & talign & "'>" & font_start_title & "#Operator&nbsp;" & font_end & "</th>")
                    End If

                    If count_dealer > 0 Then
                        htmlOut.Append("<th align='" & talign & "'>" & font_start_title & "#Broker&nbsp;" & font_end & "</th>")
                    End If

                    htmlOut.Append("</tr>")


                    For Each r As DataRow In results_table.Rows


                        If Trim(from_spot) = "pdf" Then
                            If bgcolor = "" Then
                                bgcolor = "#f0f0f0"
                            Else
                                bgcolor = ""
                            End If

                            htmlOut.Append("<tr bgcolor='" & bgcolor & "'  valign='top'>")
                        Else
                            If Not toggleRowColor Then
                                htmlOut.Append("<tr class=""alt_row"" valign='top'>")
                                toggleRowColor = True
                            Else
                                htmlOut.Append("<tr bgcolor=""white"" valign='top'>")
                                toggleRowColor = False
                            End If
                        End If





                        htmlOut.Append("<td class=""text_align_left"" width='250'><a class='underline' onclick=""javascript:load('DisplayCompanyDetail.aspx?compid=" + r("comp_id").ToString + "&journid=0','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');"">")
                        htmlOut.Append("" & font_start & "" & r("comp_name") & "</a> " & font_start & "")

                        If Not IsDBNull(r("comp_city")) Then
                            htmlOut.Append("(" & r("comp_city") & "")
                        Else
                            htmlOut.Append("(&nbsp;")
                        End If

                        If Not IsDBNull(r("comp_state")) Then
                            If Not IsDBNull(r("comp_city")) Then
                                htmlOut.Append(", ")
                            End If
                            htmlOut.Append("" & r("comp_state") & "")
                        Else
                            htmlOut.Append("&nbsp;")
                        End If

                        If Not IsDBNull(r("comp_country")) Then
                            htmlOut.Append("" & Replace(r("comp_country"), "United States", "U.S.") & ")" & font_end & "</td>")
                        Else
                            htmlOut.Append("&nbsp;)" & font_end & "</td>")
                        End If

                        If count_current_owner > 0 Then
                            If HttpContext.Current.Session.Item("localSubscription").crmTierlevel = "ALL" And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True Then
                                htmlOut.Append("<td align='" & talign & "' class=""text_align_right"">" & font_start & "" & FormatNumber(r("jetowner"), 0) & "&nbsp;" & font_end & "</td>")
                                tot_jet = tot_jet + r("jetowner")
                                htmlOut.Append("<td align='" & talign & "' class=""text_align_right"">" & font_start & "" & FormatNumber(r("turboowner"), 0) & "&nbsp;" & font_end & "</td>")
                                tot_turbo = tot_turbo + r("turboowner")
                                htmlOut.Append("<td align='" & talign & "' class=""text_align_right"">" & font_start & "" & FormatNumber(r("pistonowner"), 0) & "&nbsp;" & font_end & "</td>")
                                tot_piston = tot_piston + r("pistonowner")
                                htmlOut.Append("<td align='" & talign & "' class=""text_align_right"">" & font_start & "" & FormatNumber(r("commercialowner"), 0) & "&nbsp;" & font_end & "</td>")
                                tot_comm = tot_comm + r("commercialowner")
                                htmlOut.Append("<td align='" & talign & "' class=""text_align_right"">" & font_start & "" & FormatNumber(r("heloowner"), 0) & "&nbsp;" & font_end & "</td>")
                                tot_helo = tot_helo + r("heloowner")
                            Else
                                If HttpContext.Current.Session.Item("localSubscription").crmJets_Flag = True Then
                                    htmlOut.Append("<td align='" & talign & "' class=""text_align_right"">" & font_start & "" & FormatNumber(r("jetowner"), 0) & "&nbsp;" & font_end & "</td>")
                                    tot_jet = tot_jet + r("jetowner")
                                End If
                                If HttpContext.Current.Session.Item("localSubscription").crmTurboprops = True Then
                                    htmlOut.Append("<td align='" & talign & "' class=""text_align_right"">" & font_start & "" & FormatNumber(r("turboowner"), 0) & "&nbsp;" & font_end & "</td>")
                                    tot_turbo = tot_turbo + r("turboowner")
                                End If

                                htmlOut.Append("<td align='" & talign & "' class=""text_align_right"">" & font_start & "" & FormatNumber(r("pistonowner"), 0) & "&nbsp;" & font_end & "</td>")
                                tot_piston = tot_piston + r("pistonowner")

                                If HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
                                    htmlOut.Append("<td align='" & talign & "' class=""text_align_right"">" & font_start & "" & FormatNumber(r("commercialowner"), 0) & "&nbsp;" & font_end & "</td>")
                                    tot_comm = tot_comm + r("commercialowner")
                                End If
                                If HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True Then
                                    htmlOut.Append("<td align='" & talign & "' class=""text_align_right"">" & font_start & "" & FormatNumber(r("heloowner"), 0) & "&nbsp;" & font_end & "</td>")
                                    tot_helo = tot_helo + r("heloowner")
                                End If
                            End If

                        End If

                        If count_operator > 0 Then
                            htmlOut.Append("<td align='" & talign & "' class=""text_align_right"">" & font_start & "" & FormatNumber(r("operator"), 0) & "&nbsp;" & font_end & "</td>")
                            tot_op = tot_op + r("operator")
                        End If

                        If count_dealer > 0 Then
                            htmlOut.Append("<td align='" & talign & "' class=""text_align_right"">" & font_start & "" & FormatNumber(r("broker"), 0) & "&nbsp;" & font_end & "</td>")
                            tot_broker = tot_broker + r("broker")
                        End If


                        htmlOut.Append("</tr>")

                    Next

                    If count_current_owner > 0 Or count_operator > 0 Or count_dealer > 0 Then
                        htmlOut.Append("<tr><td colspan='1' align='right'><strong>" & font_start & "Totals" & font_end & "</strong></td>")
                    End If


                    If count_current_owner > 0 Then
                        If HttpContext.Current.Session.Item("localSubscription").crmTierlevel = "ALL" Then
                            htmlOut.Append("<td align='" & talign & "' class=""text_align_right"">" & font_start & "<strong>" & FormatNumber(tot_jet, 0) & "&nbsp;</strong>" & font_end & "</td>")
                            htmlOut.Append("<td align='" & talign & "' class=""text_align_right"">" & font_start & "<strong>" & FormatNumber(tot_turbo, 0) & "&nbsp;</strong>" & font_end & "</td>")
                            htmlOut.Append("<td align='" & talign & "' class=""text_align_right"">" & font_start & "<strong>" & FormatNumber(tot_piston, 0) & "&nbsp;</strong>" & font_end & "</td>")

                            If HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
                                htmlOut.Append("<td align='" & talign & "' class=""text_align_right"">" & font_start & "<strong>" & FormatNumber(tot_comm, 0) & "&nbsp;</strong>" & font_end & "</td>")
                            End If
                            If HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True Then
                                htmlOut.Append("<td align='" & talign & "' class=""text_align_right"">" & font_start & "<strong>" & FormatNumber(tot_helo, 0) & "&nbsp;</strong>" & font_end & "</td>")
                            End If
                        End If
                    End If
                    If count_operator > 0 Then
                        htmlOut.Append("<td align='" & talign & "' class=""text_align_right"">" & font_start & "<strong>" & FormatNumber(tot_op, 0) & "&nbsp;</strong>" & font_end & "</td>")
                    End If
                    If count_dealer > 0 Then
                        htmlOut.Append("<td align='" & talign & "' class=""text_align_right"">" & font_start & "<strong>" & FormatNumber(tot_broker, 0) & "&nbsp;</strong>" & font_end & "</td>")
                    End If

                    If count_current_owner > 0 Or count_operator > 0 Or count_dealer > 0 Then
                        htmlOut.Append("</tr>")
                    End If

                    htmlOut.Append("</table>")



                End If
            End If



        Catch ex As Exception

            aError = "Error in util_get_operaotrs_top_function(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        htmlOut_return = htmlOut.ToString
        results_table = Nothing

    End Sub





    Public Sub Build_Operators_Array(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal from_spot As String, ByVal aport_id As Long, ByRef htmlOut_return As String, ByVal selected_value As String, ByRef table_count As Long, ByRef FlightTotalLimit As Long, ByVal table_color As String, ByVal temp_pdf_header As String, Optional ByRef LimitThisQuery As Long = 0, Optional ByRef SubHeaderString As String = "", Optional ByVal show_fuel_burn_in_liters As Boolean = False)
        Dim html As New StringBuilder
        Dim dt As New DataTable
        Dim htmlOut As StringBuilder = New StringBuilder()
        Dim tcount As Integer = 0
        Dim temp_gal_lit As Integer = 0

        'Column 1 - Checkbox:
        'Column 2 - Operator:
        'Column 3 - City
        'Column 4 - State
        'Column 5 - Country
        'Column 6 - Nbr Flights
        'Column 7 - Total Flight Hours
        'Column 8 - Est Fuel Burn
        Try


            If InStr(from_spot, "pdf") > 0 And Trim(from_spot) <> "valpdf" Then
                LimitThisQuery = 43
            End If

            If Trim(selected_value) = "N" Then
                dt = get_companies_from_airport(searchCriteria, "36", "", aport_id, False, FlightTotalLimit, LimitThisQuery, True)
            ElseIf Trim(selected_value) = "B" Then
                dt = get_companies_from_airport(searchCriteria, "36", "", aport_id, False, FlightTotalLimit, LimitThisQuery, False)
            Else
                dt = util_get_opearators(searchCriteria, aport_id, FlightTotalLimit, from_spot, LimitThisQuery)
            End If


            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "Utilization_functions.vb", "start fill array")


            If Not IsNothing(dt) Then
                If dt.Rows.Count > 0 Then
                    table_count = dt.Rows.Count

                    If InStr(from_spot, "pdf") > 0 Then

                        htmlOut.Append("<tr><td valign='top'><div class=""Box""><table id=""weightTable"" cellpadding='3'  cellspacing='0' width='100%' class='formatTable " & table_color & " " & IIf(from_spot = "valpdf", "large", "") & "'><thead>")

                        htmlOut.Append("<tr><td valign='top' align='center' colspan='20'><font class='" & HttpContext.Current.Session("FONT_CLASS_HEADER") & " mainHeading'>" & SubHeaderString & "</font></td></tr>")


                        htmlOut.Append("<tr class=""noBorder"">")
                        htmlOut.Append("<th>OPERATOR</th>")

                        If from_spot = "pdf" Then
                            htmlOut.Append("<th>BUSINESS<br/>TYPE</th>")
                        Else
                            htmlOut.Append("<th>BUS TYPE</th>")
                        End If

                        htmlOut.Append("<th>CITY</th>")
                        htmlOut.Append("<th>STATE</th>")
                        htmlOut.Append("<th>COUNTRY</th>")

                        If from_spot = "pdf" Then
                            htmlOut.Append("<th class='right'><font size='-1'>NBR</br>FLTS</font></th>")
                            htmlOut.Append("<th class='right'><font size='-1'>TOTAL<br />FLIGHT<br />HOURS</font></th>")
                            If show_fuel_burn_in_liters = True Then
                                htmlOut.Append("<th class='right'><font size='-1'>EST FUEL<br />BURN (L)</font></th>")
                            Else
                                htmlOut.Append("<th class='right'><font size='-1'>EST FUEL<br />BURN (GAL)</font></th>")
                            End If
                        ElseIf InStr(from_spot, "pdf") > 0 Then
                            htmlOut.Append("<th class='right' width='15'>NBR FLTS</th>")
                            htmlOut.Append("<th class='right' width='25'>FLIGHT HOURS</th>")
                        Else
                            htmlOut.Append("<th class='right'>NBR FLIGHTS</th>")
                            htmlOut.Append("<th class='right'>FLIGHT HOURS</th>")
                        End If


                        htmlOut.Append("</tr>")
                        htmlOut.Append("</thead>")
                        htmlOut.Append("<tbody>")
                    End If


                    For Each r As DataRow In dt.Rows

                        If InStr(from_spot, "pdf") > 0 Then

                            If tcount > 38 Then
                                'tcount = 0
                                'htmlOut.Append("</table></td></tr></table>")
                                'htmlOut.Append(comp_functions.NEW_Insert_Page_Break_PDF(0, "pdf"))
                                'htmlOut.Append(temp_pdf_header)
                                ''  If bWordReport = True Then
                                ''  htmlOut.Append("<table width='" & word_width & "' align='center' cellpadding='3'>")
                                ''Else
                                'htmlOut.Append("<table width='95%' align='center' cellpadding='3'>")
                                ''  End If

                                'htmlOut.Append("<tr><td valign='top' align='center'><font class='" & HttpContext.Current.Session("FONT_CLASS_HEADER") & " mainHeading'><strong>" & searchCriteria.ViewCriteriaAirportName & "</strong> OPERATORS</font></td></tr>")

                                'htmlOut.Append("<tr><td valign='top'><div class=""Box""><table id=""weightTable"" cellpadding='3'  cellspacing='0' width='100%' class='formatTable " & table_color & "'><thead>")
                                'htmlOut.Append("<tr class=""noBorder"">")
                                'htmlOut.Append("<th>OPERATOR</th>")
                                'htmlOut.Append("<th>CITY</th>")
                                'htmlOut.Append("<th>STATE</th>")
                                'htmlOut.Append("<th>COUNTRY</th>")
                                'htmlOut.Append("<th align='right'>NBR</br>FLIGHTS</th>")
                                'htmlOut.Append("<th align='right'>TOTAL FLIGHTS<br/>HOURS</th>")
                                'htmlOut.Append("<th align='right'>EST FUEL BURN</th>")
                                'htmlOut.Append("</tr>")
                                'htmlOut.Append("</thead>")
                                'htmlOut.Append("<tbody>")

                            Else
                                htmlOut.Append("<tr>")


                                htmlOut.Append("<td><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>")
                                If Not IsDBNull(r("comp_name")) Then
                                    htmlOut.Append(Replace(Replace(r("comp_name").ToString, "'", ""), " International", " Intl."))
                                Else
                                    htmlOut.Append("&nbsp;")
                                End If
                                htmlOut.Append("</font></td>")

                                htmlOut.Append("<td><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>")
                                If Not IsDBNull(r("cbus_name")) Then
                                    ' added in replace "company" for charter company, seems redundant
                                    ' 
                                    htmlOut.Append(Replace(Replace(Replace(r("cbus_name").ToString, "'", ""), "Company", ""), "Aviation Related Business", "Aviation Related"))
                                Else
                                    htmlOut.Append("&nbsp;")
                                End If
                                htmlOut.Append("</font></td>")

                                htmlOut.Append("<td><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>")
                                If Not IsDBNull(r("comp_city")) Then
                                    If Len(Trim(r("comp_city"))) > 20 And InStr(Trim(r("comp_city")), ", ") > 0 Then
                                        htmlOut.Append(Replace(Replace(r("comp_city").ToString, "'", ""), ", ", ",<br/>"))
                                    Else
                                        htmlOut.Append(Replace(r("comp_city").ToString, "'", ""))
                                    End If
                                Else
                                    htmlOut.Append("&nbsp;")
                                End If
                                htmlOut.Append("</font></td>")


                                htmlOut.Append("<td><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>")
                                If Not IsDBNull(r("comp_state")) Then
                                    htmlOut.Append(r("comp_state").ToString)
                                Else
                                    htmlOut.Append("&nbsp;")
                                End If
                                htmlOut.Append("</font></td>")


                                htmlOut.Append("<td><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>")
                                If Not IsDBNull(r("comp_country")) Then
                                    htmlOut.Append(Replace(Replace(r("comp_country").ToString, "United States", "U.S."), "United Kingdom", "UK"))  ' added in UK - 1/16/18 MSW 
                                Else
                                    htmlOut.Append("&nbsp;")
                                End If
                                htmlOut.Append("</font></td>")


                                htmlOut.Append("<td align='right'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>")
                                If Not IsDBNull(r("NbrFlights")) Then
                                    htmlOut.Append(FormatNumber(r("NbrFlights"), 0))
                                Else
                                    htmlOut.Append("&nbsp;")
                                End If
                                htmlOut.Append("</font></td>")

                                htmlOut.Append("<td align='right'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>")
                                If from_spot = "pdf" Then
                                    If Not IsDBNull(r("TotalFlightTimeHrs")) Then
                                        htmlOut.Append(FormatNumber(r("TotalFlightTimeHrs"), 0))
                                    Else
                                        htmlOut.Append("&nbsp;")
                                    End If
                                Else
                                    If Not IsDBNull(r("TotalFlightTimeHrs")) Then
                                        htmlOut.Append(FormatNumber(r("TotalFlightTimeHrs"), 1))
                                    Else
                                        htmlOut.Append("&nbsp;")
                                    End If
                                End If

                                htmlOut.Append("</font></td>")

                                If from_spot = "pdf" Then
                                    htmlOut.Append("<td align='right'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>")
                                    If Not IsDBNull(r("TotalFuelBurn")) Then

                                        temp_gal_lit = FormatNumber(r("TotalFuelBurn"), 0)
                                        If show_fuel_burn_in_liters = True Then
                                            temp_gal_lit = FormatNumber((temp_gal_lit * 3.78541), 0)
                                        End If

                                        htmlOut.Append(FormatNumber(temp_gal_lit, 0)) ' changed to 0 .. msw - 1/16/19 
                                    Else
                                        htmlOut.Append("&nbsp;")
                                    End If
                                    htmlOut.Append("</font></td>")
                                End If

                                htmlOut.Append("</tr>")
                            End If

                        Else

                            If html.ToString <> "" Then
                                html.Append(",")
                            End If
                            html.Append("{")
                            html.Append("""check"": """",") 'Checkbox row.



                            If Not IsDBNull(r("comp_name")) Then
                                html.Append("""operator"": [""")

                                If UCase(r("comp_name")) = "OPERATOR UNKNOWN" Then
                                    html.Append(Replace(r("comp_name"), "'", ""))
                                Else
                                    html.Append("<ul class='cssMenu'><li><a href='#' class='expand_more'>" & Replace(r("comp_name").ToString, "'", "") & "</a><ul>")
                                    html.Append("<li><a class='underline' href='view_template.aspx?ViewID=28&ViewName=Fuel Utilization View&" & "aport_id=" & IIf(aport_id < 2, "0", aport_id) & "&" & "comp_id=" & r("comp_id") & "' title='Select Operator'>Select Operator</a></li>")
                                    html.Append("<li><a title='View Operator Profile' href='#' onclick=\""javascript:load('DisplayCompanyDetail.aspx?compid=" & r("comp_id") & "','','scrollbars=yes,menubar=no,height=900,width=1090,resizable=yes,toolbar=no,location=no,status=no');return false;\"">View Operator Profile</a></li>")
                                    html.Append("</ul></li></ul>")
                                End If

                                html.Append(""", """ & Replace(r("comp_name"), "'", "") & """], ")
                            Else
                                html.Append("""operator"": ["""",""""],")
                            End If

                            If Not IsDBNull(r("cbus_name")) Then
                                html.Append("""cbus_name"": """ & Replace(r("cbus_name"), "'", "") & """,")
                            Else
                                html.Append("""cbus_name"": ""&nbsp;"",")
                            End If

                            If Not IsDBNull(r("comp_city")) Then
                                html.Append("""city"": """ & Replace(r("comp_city"), "'", "") & """,")
                            Else
                                html.Append("""city"": ""&nbsp;"",")
                            End If

                            If Not IsDBNull(r("comp_state")) Then
                                html.Append("""state"": """ & Replace(r("comp_state"), "'", "") & """,")
                            Else
                                html.Append("""state"": ""&nbsp;"",")
                            End If

                            If Not IsDBNull(r("comp_country")) Then
                                html.Append("""country"": """ & Replace(r("comp_country"), "United States", "U.S.") & """,")
                            Else
                                html.Append("""country"": ""&nbsp;"",")
                            End If

                            If Not IsDBNull(r("country_continent_name")) Then
                                html.Append("""continent"": """ & r("country_continent_name") & """,")
                            Else
                                html.Append("""continent"": ""&nbsp;"",")
                            End If
                            'End If

                            html.Append("""flights"":""" & FormatNumber(r("NbrFlights"), 0) & """,")
                            html.Append("""hours"":""" & FormatNumber(r("TotalFlightTimeHrs"), 1) & """,")
                            html.Append("""fuel"":""" & FormatNumber(r("TotalFuelBurn"), 0) & """,")
                            html.Append("""AvgDistance"":""" & FormatNumber(r("AvgDistance"), 0) & """,")
                            If Not IsDBNull(r("AvgMinPerFlights")) Then
                                html.Append("""AvgMinPerFlights"":""" & FormatNumber(r("AvgMinPerFlights"), 0) & """,")
                            Else
                                html.Append("""AvgMinPerFlights"":""0"",")
                            End If



                            If Not IsDBNull(r("comp_email_address")) Then
                                html.Append("""email"": [""<a href='mailto:" & r("comp_email_address") & "'>" & r("comp_email_address") & "</a>"", """ & r("comp_email_address") & """],")
                            Else
                                html.Append("""email"": ["""",""""],")
                            End If

                            If Not IsDBNull(r("comp_off_phone")) Then
                                html.Append("""office"": """ & r("comp_off_phone") & """,")
                            Else
                                html.Append("""office"": ""&nbsp;"",")
                            End If


                            'If Not IsDBNull(r("contact_first_name")) Then
                            '  html.append("""first_name"": """ & r("contact_first_name") & """,")
                            'Else
                            '  html.append("""first_name"": ""&nbsp;"",")
                            'End If

                            'If Not IsDBNull(r("contact_first_name")) Then
                            '  html.append("""last_name"": """ & r("contact_first_name") & """,")
                            'Else
                            '  html.append("""last_name"": ""&nbsp;"",")
                            'End If

                            'If Not IsDBNull(r("contact_title")) Then
                            '  html.append("""title"": """ & r("contact_title") & """,")
                            'Else
                            '  html.append("""title"": ""&nbsp;"",")
                            'End If

                            'If Not IsDBNull(r("contact_email_address")) Then
                            '  html.append("""contact_email"": """ & r("contact_email_address") & """,")
                            'Else
                            '  html.append("""contact_email"": ""&nbsp;"",")
                            'End If

                            'If Not IsDBNull(r("contact_off_phone")) Then
                            '  html.append("""contact_off_phone"": """ & r("contact_off_phone") & """,")
                            'Else
                            '  html.append("""contact_off_phone"": ""&nbsp;"",")
                            'End If

                            'If Not IsDBNull(r("contact_mob_phone")) Then
                            '  html.append("""contact_mob_phone"": """ & r("contact_mob_phone") & """,")
                            'Else
                            '  html.append("""contact_mob_phone"": ""&nbsp;"",")
                            'End If

                            If Not IsDBNull(r("comp_address1")) Then
                                html.Append("""address"": """ & Replace(r("comp_address1"), "'", "") & """")
                            Else
                                html.Append("""address"": ""&nbsp;""")
                            End If

                            html.Append("}")

                        End If

                        tcount += 1

                    Next
                End If
            End If

            htmlOut.Append("</tbody></table></div></td></tr>")

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "Utilization_functions.vb", "end fill array")



            If InStr(from_spot, "pdf") > 0 Then
                htmlOut_return = htmlOut.ToString
            Else
                htmlOut_return = " var currentDataSet = [ " & html.ToString & " ]; "
            End If


        Catch ex As Exception

        End Try
    End Sub
    Public Sub util_get_operators_top_function(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef htmlOut_return As String, ByVal aport_id As Long, ByVal selected_value As String, ByRef table_count As Long, ByVal from_spot As String)

        Dim results_table As New DataTable
        Dim toggleRowColor As Boolean = False
        Dim last_comp_id As Long = 0
        Dim comp_count As Long = 0
        Dim htmlOut As New StringBuilder
        Dim htmlout_java As New StringBuilder
        Dim tot_flight As Long = 0
        Dim tot_hours As Long = 0
        Dim tot_burn As Long = 0


        Try

            If Trim(selected_value) = "B" Then
                results_table = get_companies_from_airport(searchCriteria, "36", "", aport_id, False, 0)
            Else
                results_table = util_get_opearators(searchCriteria, aport_id, 0, from_spot)
            End If


            If Not IsNothing(results_table) Then
                If results_table.Rows.Count > 0 Then
                    table_count = results_table.Rows.Count

                    If Trim(from_spot) = "company" Then
                        htmlOut.Append("<div class=""Box""><table id='modelForsaleViewOuterTable' width=""100%"" cellpadding=""0"" cellspacing=""0""class='formatTable blue'>")
                        htmlOut.Append("<tr class='header_row'>")
                        htmlOut.Append("<th width='280' align='left'>Operator (Location)</th>")
                        htmlOut.Append("<th align='right'>#Flights</th>")
                        htmlOut.Append("<th align='right'>Total<br/>Flight<br/>Hrs</th>")
                        htmlOut.Append("<th align='right'><a href='#' title='Estimated Gallons of Fuel Burned' alt='Estimated Gallons of Fuel Burned'> Est. Fuel<br/>Burn (Gal)</a></th>")
                    Else
                        htmlOut.Append("<table id='modelForsaleViewOuterTable' width=""100%"" cellpadding=""0"" cellspacing=""0"" class=""module"">")
                        If searchCriteria.ViewCriteriaCompanyID > 0 Then
                            htmlOut.Append("<tr valign='top'><td align='center' valign='top' class='header' width='100%'>Operator Flights in the Last Year</td></tr>")
                        Else
                            htmlOut.Append("<tr valign='top'><td align='center' valign='top' class='header' width='100%'>Operators with Greater Than 2 Flights in the Last Year</td></tr>")
                        End If
                        htmlOut.Append("<tr><td align=""left"" valign=""top"" width='100%'>")

                        htmlOut.Append("<table id='tableCopy' width='100%' cellpadding='0' cellspacing='0' border='0' align='left'><thead>")

                        htmlOut.Append(" <th>SEL</th>")
                        htmlOut.Append("<th width='250'>Operator</th>")
                        htmlOut.Append("<th>City</th>")
                        htmlOut.Append("<th>State</th>")
                        htmlOut.Append("<th>Country</th>")
                        htmlOut.Append("<th>Nbr Flights&nbsp;</th>")
                        htmlOut.Append("<th>Total Flight Hrs&nbsp;</th>")
                        htmlOut.Append("<th><a href='#' title='Estimated Gallons of Fuel Burned' alt='Estimated Gallons of Fuel Burned'> Est. Fuel<br/>Burn (Gal)&nbsp;</a></th>")
                    End If



                    If Trim(from_spot) = "company" Then
                        htmlOut.Append("</tr>")
                    Else
                        htmlOut.Append("</thead><tbody>")
                    End If


                    For Each r As DataRow In results_table.Rows

                        If Trim(from_spot) = "company" Then
                            If Not toggleRowColor Then
                                htmlOut.Append("<tr class=""alt_row"">")
                                toggleRowColor = True
                            Else
                                htmlOut.Append("<tr bgcolor=""white"">")
                                toggleRowColor = False
                            End If
                            htmlOut.Append("<td class=""text_align_left"" width='280'><a class='underline' onclick=""javascript:load('DisplayCompanyDetail.aspx?compid=" + r("comp_id").ToString + "&journid=0','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');"">")
                            htmlOut.Append("" & r("comp_name") & "</a> ")
                        Else
                            htmlOut.Append("<td class=""text_align_center""></td>")
                            htmlOut.Append("<td class=""text_align_center"" width='220'><a href='view_template.aspx?ViewID=28&ViewName=Fuel Utilization View&aport_id=" & aport_id & "&comp_id=" & r("comp_id") & "'>")
                            htmlOut.Append("" & r("comp_name") & "</a></td>")
                        End If


                        If Trim(from_spot) = "company" Then
                            If Not IsDBNull(r("comp_city")) Then
                                htmlOut.Append("(" & r("comp_city") & "")
                            Else
                                htmlOut.Append("(&nbsp;")
                            End If

                            If Not IsDBNull(r("comp_state")) Then
                                If Not IsDBNull(r("comp_city")) Then
                                    htmlOut.Append(", ")
                                End If
                                htmlOut.Append("" & r("comp_state") & "&nbsp;")
                            Else
                                htmlOut.Append("&nbsp;")
                            End If

                            If Not IsDBNull(r("comp_country")) Then
                                htmlOut.Append("" & Replace(r("comp_country"), "United States", "U.S.") & ")</td>")
                            Else
                                htmlOut.Append("&nbsp;)</td>")
                            End If
                        Else
                            If Not IsDBNull(r("comp_city")) Then
                                htmlOut.Append("<td class=""text_align_left"">" & r("comp_city") & "</td>")
                            Else
                                htmlOut.Append("<td class=""text_align_left"">&nbsp;</td>")
                            End If

                            If Not IsDBNull(r("comp_state")) Then
                                htmlOut.Append("<td class=""text_align_center"">" & r("comp_state") & "</td>")
                            Else
                                htmlOut.Append("<td class=""text_align_center"">&nbsp;</td>")
                            End If

                            If Not IsDBNull(r("comp_country")) Then
                                htmlOut.Append("<td class=""text_align_center"">" & Replace(r("comp_country"), "United States", "U.S.") & "</td>")
                            Else
                                htmlOut.Append("<td class=""text_align_center"">&nbsp;</td>")
                            End If
                        End If


                        htmlOut.Append("<td class=""text_align_right"">" & FormatNumber(r("NbrFlights"), 0) & "&nbsp;</td>")
                        tot_flight = tot_flight + r("NbrFlights")
                        htmlOut.Append("<td class=""text_align_right"">" & FormatNumber(r("TotalFlightTimeHrs"), 1) & "&nbsp;</td>")
                        tot_hours = tot_hours + r("TotalFlightTimeHrs")
                        htmlOut.Append("<td class=""text_align_right"">" & FormatNumber(r("TotalFuelBurn"), 0) & "&nbsp;</td>")
                        tot_burn = tot_burn + r("TotalFuelBurn")
                        htmlOut.Append("</tr>")

                    Next

                    If Trim(from_spot) = "company" Then
                        htmlOut.Append("<tr><td colspan='1' align='right'><strong>Totals</strong></td>")
                        htmlOut.Append("<td class=""text_align_right""><strong>" & FormatNumber(tot_flight, 0) & "&nbsp;</strong></td>")
                        htmlOut.Append("<td class=""text_align_right""><strong>" & FormatNumber(tot_hours, 0) & "&nbsp;</strong></td>")
                        htmlOut.Append("<td class=""text_align_right""><strong>" & FormatNumber(tot_burn, 0) & "&nbsp;</strong></td>")
                        htmlOut.Append("</tr>")
                        htmlOut.Append("</table></div>")
                    Else
                        htmlOut.Append("</tbody></table>")
                        htmlOut.Append("<div id=""forSaleInnerTable"" style=""width:930px;""></div>")
                        htmlOut.Append("</td></tr></table>")
                    End If


                End If
            End If



        Catch ex As Exception

            aError = "Error in util_get_operaotrs_top_function(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        htmlOut_return = htmlOut.ToString
        results_table = Nothing

    End Sub

    Public Sub EMPTY_TABLE_EXAMPLE(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef htmlOut_return As String, ByVal aport_id As Long)

        Dim results_table As New DataTable
        Dim toggleRowColor As Boolean = False
        Dim last_comp_id As Long = 0
        Dim comp_count As Long = 0
        Dim htmlOut As New StringBuilder


        Try


            results_table = util_get_opearators(searchCriteria, aport_id, 0, "")


            If Not IsNothing(results_table) Then
                If results_table.Rows.Count > 0 Then

                    htmlOut.Append("<table id='modelForsaleViewOuterTable' width=""100%"" cellpadding=""0"" cellspacing=""0"" class=""module"">")
                    htmlOut.Append("<tr valign='top'><td align='center' valign='top' class='header'>Top</td></tr>")
                    htmlOut.Append("<tr><td align=""left"" valign=""top"">")

                    htmlOut.Append("<table id='tableCopy' width='100%' cellpadding='0' cellspacing='0' border='0' align='left'><thead>")

                    htmlOut.Append(" <th>SEL</th>")
                    htmlOut.Append("<th>comp</th>")
                    htmlOut.Append("<th>NbrFlights</th>")

                    htmlOut.Append("</thead><tbody>")

                    For Each r As DataRow In results_table.Rows

                        htmlOut.Append("<td class=""text_align_center""></td>") ' for sel
                        htmlOut.Append("<td class=""text_align_center"">cc</td>")
                        htmlOut.Append("<td class=""text_align_center"">ccnumber</td>")
                        htmlOut.Append("</tr>")

                    Next


                    htmlOut.Append("</tbody></table>")
                    htmlOut.Append("<div id=""forSaleInnerTable"" style=""width: 930px;""></div>")
                    htmlOut.Append("</td></tr></table>")
                Else
                    htmlOut.Append("<table><tr><td valign=""top"" align=""left"" class=""seperator"" style=""padding-left:3px;"">No FAA Flight Activity Found.</td></tr></table>")
                End If
            Else
                htmlOut.Append("<table><tr><td valign=""top"" align=""left"" class=""seperator"" style=""padding-left:3px;"">No FAA Flight Activity Found.</td></tr></table>")
            End If


            ' CheckAndJSForDatatable()


        Catch ex As Exception

            aError = "Error in util_get_operaotrs_top_function(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        htmlOut_return = htmlOut.ToString
        results_table = Nothing

    End Sub

    Public Sub get_companies_from_airport_top_function(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByVal city_name As String, ByVal company_type As String, ByVal run_export As String, ByVal aport_id As Long, ByVal use_ac As Boolean)

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False
        Dim last_comp_id As Long = 0
        Dim comp_count As Long = 0


        Try


            results_table = get_companies_from_airport(searchCriteria, company_type, run_export, aport_id, use_ac, 0)

            If Trim(run_export) <> "" Then
                crmViewDataLayer.ExportTableData(results_table)
            Else


                If Not IsNothing(results_table) Then
                    If results_table.Rows.Count > 0 Then
                        For Each r As DataRow In results_table.Rows

                            If CLng(r.Item("comp_id")) <> CLng(last_comp_id) Then
                                comp_count = comp_count + 1
                            End If

                            last_comp_id = CLng(r.Item("comp_id"))

                        Next
                    End If
                End If


                If Trim(company_type) = "Owner" Then
                    htmlOut.Append("<table id=""fractionsExpiringOuterTable"" width=""100%"" cellpadding=""0"" cellspacing=""0"" class=""module"">")
                    htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""header"">")
                    htmlOut.Append("<table cellspacing='0' cellpadding='0' width='100%'><tr><td align='left' width='80%'>")
                    htmlOut.Append("&nbsp;Companies Owning Aircraft at " & city_name & " - " & comp_count & " Owners</td>")    ', " & results_table.Rows.Count & " Aircraft
                    htmlOut.Append("<td align='right'>Export:&nbsp;&nbsp;<a href='view_template.aspx?ViewID=24&ViewName=Airport FBO View&aport_id=" & Airport_ID_OVERALL & "&activetab=3&export=A' target='_blank'><font color='white'><u>Full List</u></font></a>&nbsp;&nbsp;")
                    htmlOut.Append("<a href='view_template.aspx?ViewID=24&ViewName=Airport FBO View&aport_id=" & Airport_ID_OVERALL & "&activetab=3&export=C' target='_blank'><font color='white'><u>Owners</u></font></a>")
                    htmlOut.Append("&nbsp;</td></tr></table>")
                    htmlOut.Append("</td></tr>")
                ElseIf Trim(company_type) = "Operator" Then
                    htmlOut.Append("<table id=""fractionsExpiringOuterTable"" width=""100%"" cellpadding=""0"" cellspacing=""0"" class=""module"">")
                    htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""header"">")
                    htmlOut.Append("<table cellspacing='0' cellpadding='0' width='100%'><tr><td align='left' width='80%'>")
                    htmlOut.Append("&nbsp;Companies Operating Aircraft at " & city_name & " - " & comp_count & " Operators</td>") ' , " & results_table.Rows.Count & " Aircraft
                    htmlOut.Append("<td align='right'>Export:&nbsp;&nbsp;<a href='view_template.aspx?ViewID=24&ViewName=Airport FBO View&aport_id=" & Airport_ID_OVERALL & "&activetab=4&export=A' target='_blank'><font color='white'><u>Full List</u></font></a>&nbsp;&nbsp;")
                    htmlOut.Append("<a href='view_template.aspx?ViewID=24&ViewName=Airport FBO View&aport_id=" & Airport_ID_OVERALL & "&activetab=4&export=C' target='_blank'><font color='white'><u>Operators</u></font></a>")
                    htmlOut.Append("&nbsp;</td></tr></table>")
                    htmlOut.Append("</td></tr>")
                Else

                    htmlOut.Append("<table id=""fractionsExpiringOuterTable"" width=""100%"" cellpadding=""0"" cellspacing=""0"" class=""module"">")
                    htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""header"">")
                    htmlOut.Append("<table cellspacing='0' cellpadding='0' width='100%'><tr><td align='left' width='80%'>")
                    htmlOut.Append("&nbsp;Companies Owning Aircraft at " & city_name & " - " & comp_count & " Owners</td>") ' , " & results_table.Rows.Count & " Aircraft
                    htmlOut.Append("<td align='right'>Export:&nbsp;&nbsp;<a href='view_template.aspx?ViewID=24&ViewName=Airport FBO View&aport_id=" & Airport_ID_OVERALL & "&activetab=3&export=A' target='_blank'><font color='white'><u>Full List</u></font></a>&nbsp;&nbsp;")
                    htmlOut.Append("<a href='view_template.aspx?ViewID=24&ViewName=Airport FBO View&aport_id=" & Airport_ID_OVERALL & "&activetab=3&export=C' target='_blank'><font color='white'><u>Owners</u></font></a>")
                    htmlOut.Append("&nbsp;</td></tr></table>")
                    htmlOut.Append("</td></tr>")
                End If




                If Not IsNothing(results_table) Then

                    If results_table.Rows.Count > 0 Then

                        htmlOut.Append("<tr><td valign=""top"" align=""left"">")

                        htmlOut.Append("<table id=""fractionsExpiringInnerTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"">")

                        htmlOut.Append("<tr><td colspan=""2"" class=""rightside"" valign=""top"">")
                        htmlOut.Append("<div style=""height: 250px; overflow: auto;"">")

                        htmlOut.Append("<table id=""fractionsExpiringDataTable"" width=""100%"" cellpadding=""4"" cellspacing=""0"">")
                        htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""seperator""><strong>Company</strong></td>")
                        If use_ac = True Then
                            htmlOut.Append("<td class=""seperator""><strong>Aircraft</strong></td>")
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

                            htmlOut.Append("<td align=""left"" valign=""top"" class=""seperator"">")
                            htmlOut.Append("<strong><a class='underline' onclick=""javascript:load('DisplayCompanyDetail.aspx?compid=" + r.Item("comp_id").ToString + "&journid=0','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');"">")
                            htmlOut.Append("" & r.Item("comp_name").ToString & "</a></strong> (")
                            htmlOut.Append("" & r.Item("comp_address1").ToString & ", " & r.Item("comp_city").ToString & ", " & r.Item("comp_state").ToString)
                            htmlOut.Append(")</td>")

                            If use_ac = True Then
                                htmlOut.Append("<td align=""left"" valign=""top"" class=""seperator"">")
                                htmlOut.Append("" & r.Item("Make").ToString & " ")
                                htmlOut.Append("" & r.Item("Model").ToString & " ")
                                htmlOut.Append(", S#: ")
                                htmlOut.Append("<a class='underline' onclick=""javascript:load('DisplayAircraftDetail.aspx?acid=" + r.Item("ACId").ToString + "&jid=0','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');"" title='Display Aircraft Details'>")
                                htmlOut.Append("" & r.Item("SerNbr").ToString & "</a> ")
                                htmlOut.Append(", R#: " & r.Item("RegNbr").ToString & " ")
                                htmlOut.Append(" </td>")
                            End If


                            htmlOut.Append("</tr>")
                        Next

                        htmlOut.Append("</table></div></td></tr></table></td></tr>")

                    Else
                        htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""seperator"" style=""padding-left:3px;"">No FAA Flight Activity Found.</td></tr>")
                    End If
                Else
                    htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""seperator"" style=""padding-left:3px;"">No FAA Flight Activity Found.</td></tr>")
                End If

                htmlOut.Append("</table>")
            End If

        Catch ex As Exception

            aError = "Error in views_display_fractions_to_expire(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub
    Public Sub get_companies_in_city_top_function(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByVal city_name As String, ByVal bus_type As String, ByRef compare_view_sold_label As String, ByVal run_export As String, ByVal aport_id As Long, ByVal temp_distance As Integer, ByVal orig_lat As Double, ByVal orig_long As Double, ByRef inbetween_text As String)

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False

        Try

            results_table = get_companies_in_city(searchCriteria, bus_type, run_export, temp_distance, orig_lat, orig_long, searchCriteria.ViewCriteriaCity, searchCriteria.ViewCriteriaCountry)

            If Trim(run_export) <> "" Then
                crmViewDataLayer.ExportTableData(results_table)
            Else
                'compare_view_sold_label = "<table cellpadding='0' cellspacing='0' width='85%' align='right'><tr><td align='left'>"
                'compare_view_sold_label &= "&nbsp;Companies In " & city_name & " - " & results_table.Rows.Count & " Companies"
                'compare_view_sold_label &= "</td><td align='right'><a href='view_template.aspx?ViewID=24&ViewName=Airport FBO View&aport_id=" & aport_id & "&aport_iata=" & searchCriteria.ViewCriteriaAirportIATA & "&activetab=2&export=A' target='_blank'>Export Companies</a>"
                'compare_view_sold_label &= "&nbsp;</td></tr></table>"

                inbetween_text = "&nbsp;Companies Within"

                compare_view_sold_label = " Miles "
                compare_view_sold_label &= " of " & city_name & " - " & results_table.Rows.Count & " Companies"
                compare_view_sold_label &= "&nbsp;-&nbsp;&nbsp;<a href='view_template.aspx?ViewID=24&ViewName=Airport FBO View&aport_id=" & aport_id & "&activetab=2&export=A' target='_blank'>Export Companies</a>"
                'compare_view_sold_label &= "&nbsp;&nbsp;&nbsp;Change Miles To "
                'compare_view_sold_label &= "<a href='view_template.aspx?ViewID=24&ViewName=Airport FBO View&aport_id=" & aport_id & "&activetab=2&cdistance=25&bus_type=" & bus_type & "'>25</a>&nbsp;"
                ' compare_view_sold_label &= "<a href='view_template.aspx?ViewID=24&ViewName=Airport FBO View&aport_id=" & aport_id & "&activetab=2&cdistance=50&bus_type=" & bus_type & "'>50</a>&nbsp;"
                compare_view_sold_label &= "&nbsp;"

                htmlOut.Append("<table id=""fractionsExpiringOuterTable"" width=""100%"" cellpadding=""0"" cellspacing=""0"" class=""module"">")

                If Not IsNothing(results_table) Then

                    If results_table.Rows.Count > 0 Then

                        htmlOut.Append("<tr><td valign=""top"" align=""left"">")

                        htmlOut.Append("<table id=""fractionsExpiringInnerTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"">")

                        htmlOut.Append("<tr><td colspan=""2"" class=""rightside"" valign=""top"">")
                        htmlOut.Append("<div style=""height: 250px; overflow: auto;"">")

                        htmlOut.Append("<table id=""fractionsExpiringDataTable"" width=""100%"" cellpadding=""4"" cellspacing=""0"">")
                        'htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""seperator"" width=""80%""><strong>Company</strong></td></tr>")

                        For Each r As DataRow In results_table.Rows

                            If Not toggleRowColor Then
                                htmlOut.Append("<tr class=""alt_row"">")
                                toggleRowColor = True
                            Else
                                htmlOut.Append("<tr bgcolor=""white"">")
                                toggleRowColor = False
                            End If

                            htmlOut.Append("<td align=""left"" valign=""top"" class=""seperator"">")
                            htmlOut.Append("<strong><a class='underline' onclick=""javascript:load('DisplayCompanyDetail.aspx?compid=" + r.Item("comp_id").ToString + "&journid=0','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');"">")
                            htmlOut.Append("" & r.Item("comp_name").ToString & "</a></strong> (")


                            If Not IsDBNull(r.Item("comp_address1")) Then
                                htmlOut.Append("" & r.Item("comp_address1").ToString & " ")
                            End If

                            If Not IsDBNull(r.Item("comp_city")) Then
                                htmlOut.Append("" & r.Item("comp_city").ToString)
                                If Not IsDBNull(r.Item("comp_state")) Then
                                    htmlOut.Append(", " & r.Item("comp_state").ToString)
                                End If
                            ElseIf Not IsDBNull(r.Item("comp_state")) Then
                                htmlOut.Append("" & r.Item("comp_state").ToString)
                            End If
                            If Not IsDBNull(r.Item("comp_email_address")) Then
                                If r.Item("comp_web_address").ToString.Trim.ToLower.Contains("www") Then
                                    htmlOut.Append(" - <a href=""http://" + r.Item("comp_web_address").ToString.Trim + """ target=""new"">" + r.Item("comp_web_address").ToString.Trim + "</a>")
                                Else
                                    htmlOut.Append(" - <a href=""" + r.Item("comp_web_address").ToString.Trim + """ target=""new"">" + r.Item("comp_web_address").ToString.Trim + "</a>")
                                End If
                            End If


                            If Not IsDBNull(r.Item("comp_email_address")) Then
                                htmlOut.Append(" - <a href='mailto:" + r.Item("comp_email_address").ToString.Trim + "' title='Send Email to Company'>" & r.Item("comp_email_address").ToString & "</a>")
                            End If
                            htmlOut.Append(")</td></tr>")
                        Next

                        htmlOut.Append("</table></div></td></tr></table></td></tr>")

                    Else
                        htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""seperator"" style=""padding-left:3px;"">No data matches for your search criteria</td></tr>")
                    End If
                Else
                    htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""seperator"" style=""padding-left:3px;"">No data matches for your search criteria</td></tr>")
                End If

                htmlOut.Append("</table>")
            End If

        Catch ex As Exception

            aError = "Error in views_display_fractions_to_expire(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub
    Public Sub get_bus_type_from_companies_from_airport_top_function(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef drop_down_list As DropDownList, ByVal selected_bus_type As String, ByVal company_distance As Integer, ByVal orig_lat As Double, ByVal orig_long As Double, ByRef drop_down_list_19 As DropDownList)

        Dim results_table As New DataTable
        Try
            drop_down_list.Items.Add(New System.Web.UI.WebControls.ListItem("All", ""))
            drop_down_list_19.Items.Add(New System.Web.UI.WebControls.ListItem("All", ""))

            results_table = get_bus_type_from_companies_from_airport(searchCriteria, company_distance, orig_lat, orig_long)

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then
                    For Each r As DataRow In results_table.Rows
                        drop_down_list.Items.Add(New System.Web.UI.WebControls.ListItem(r.Item("cbus_name").ToString, r.Item("cbus_type").ToString))
                        drop_down_list_19.Items.Add(New System.Web.UI.WebControls.ListItem(r.Item("cbus_name").ToString, r.Item("cbus_type").ToString))
                    Next
                Else
                End If

                drop_down_list.SelectedValue = Trim(selected_bus_type)
                drop_down_list_19.SelectedValue = Trim(selected_bus_type)
            Else
            End If

        Catch ex As Exception

            aError = "Error in get_bus_type_from_companies_from_airport_top_function(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try
        results_table = Nothing

    End Sub
#End Region




    'Public Sub make_util_active_tab(ByVal active_tab As Integer, ByRef searchCriteria As viewSelectionCriteriaClass, ByRef this_section_string As String, ByVal product_code_selection As String, ByVal aport_id As Long, ByVal operator_list As String, ByRef acStartTableColspan As HtmlTableCell, ByRef TotalFlights As Long, ByRef cfolder_id As Long, ByRef airport_list As Long)

    '  Dim table_count As Long = 0

    '  Try


    '    If active_tab = 0 Then   ' changed from tab 4 
    '      '----------- OPERATOR SECTION---------------------------------------------------

    '      this_section_string = ""

    '      'If a company is picked and an airport ID.
    '      'If an airport folder isn't picked.
    '      If (aport_id < 2) Then
    '        Call MostCommonOriginsJSArray(searchCriteria, this_section_string, product_code_selection, table_count, True, True, TotalFlights)
    '        ' Me.operator_drop2.Visible = False
    '        acStartTableColspan.Attributes.Remove("colspan")
    '        acStartTableColspan.Attributes.Add("colspan", "4")
    '        BuildOnLoadJavascript(this_section_string, BuildOperatorDataTableJS("createOriginTable", "startTableOper", "mostCommonOriginsDataset", 2, False, TotalFlights, IIf(cfolder_id > 0 Or aport_id > 0, True, False), operator_list))
    '      Else
    '        Call Build_Operators_Array(searchCriteria, "view", IIf(Not String.IsNullOrEmpty(airport_list), 0, aport_id), this_section_string, Me.operator_drop2.SelectedValue, table_count, TotalFlights)
    '        BuildOnLoadJavascript(this_section_string, BuildOperatorDataTableJS("createOperatorTable", "startTableOper", "currentDataSet", 1, False, TotalFlights, IIf(cfolder_id > 0 Or aport_id > 0, True, False), operator_list))
    '      End If

    '      Session("Last_FBO_Bottom") = 0

    '    ElseIf active_tab = 1 Then

    '      '-- ***************  UPPER RIGHT TAB 2 - TOP ORIGINS/DESTINATIONS ************************
    '      '-- # FLIGHT ACTIVITY -MOST COMMON ORIGINS - WE WOULD ALSO DO SAME FOR DESTINATIONS 
    '      this_section_string = ""
    '      'Call util_functions.get_most_common_origins_top_function(searchCriteria, this_section_string, product_code_selection, table_count)
    '      MostCommonOriginsJSArray(searchCriteria, this_section_string, product_code_selection, table_count, False, False, TotalFlights)

    '      If InStr(this_section_string, """iata"":") > 0 Then
    '        BuildOnLoadJavascript(this_section_string, BuildOperatorDataTableJS("createOriginTable", "mostCommonOrigins", "mostCommonOriginsDataset", 2, False, TotalFlights, IIf(cfolder_id > 0 Or aport_id > 0, True, False), operator_list))
    '      Else
    '        originTableColspan.Attributes.Remove("colspan")
    '        originTableColspan.Attributes.Add("colspan", "3")
    '        BuildOnLoadJavascript(this_section_string, BuildOperatorDataTableJS("createOriginTable", "mostCommonOrigins", "mostCommonOriginsDataset", 2, True, TotalFlights, IIf(cfolder_id > 0 Or aport_id > 0, True, False), operator_list))
    '      End If

    '    ElseIf active_tab = 2 Then

    '      this_section_string = ""
    '      Call FlightActivityArrayByModel(searchCriteria, this_section_string, product_code_selection, "view", TotalFlights)
    '      BuildOnLoadJavascript(this_section_string, BuildOperatorDataTableJS("createJsModelTable", "modelData", "modelDataSet", 3, False, TotalFlights, IIf(cfolder_id > 0 Or aport_id > 0, True, False), operator_list))
    '      HttpContext.Current.Session.Item("Last_FBO_Bottom") = 2

    '    ElseIf active_tab = 3 Then
    '      view_events_label.Text = ""
    '      view_events_label.Visible = True
    '      If NoAirportNoOperator Then
    '        'This can't run.
    '        view_events_label.Text = "Please pick an operator, airport or folders to run this tab."
    '      Else
    '        'test


    '        Call FlightActivityJSArray(searchCriteria, this_section_string, is_located_here, product_code_selection, TotalFlights)
    '        'Me.view_events_label.Text = this_section_string
    '        BuildOnLoadJavascript(this_section_string, BuildOperatorDataTableJS("createJsACTable", "acData", "acDataSet", 4, False, TotalFlights, IIf(cfolder_id > 0 Or aport_id > 0, True, False), operator_list))
    '      End If
    '      Session("Last_FBO_Bottom") = 3
    '    ElseIf active_tab = 4 Then
    '      view_news_label1.Text = ""
    '      view_news_label1.Visible = True
    '      If NoAirportNoOperator Then
    '        view_news_label1.Text = "Please pick an operator, airport or folders to run this tab."
    '      Else
    '        this_section_string = ""
    '        Call FlightJSArray(searchCriteria, this_section_string, location_name, Trim(Request("export")), Me.compare_view_current_label.Text, Me.compare_view_current_label2.Text, Me.ac_projects_ddl.SelectedValue, recent_flight_months, IIf(String.IsNullOrEmpty(localCriteria.ViewCriteriaDocumentsStartDate), start_date.Text, localCriteria.ViewCriteriaDocumentsStartDate), IIf(String.IsNullOrEmpty(localCriteria.ViewCriteriaDocumentsEndDate), end_date.Text, localCriteria.ViewCriteriaDocumentsEndDate), product_code_selection, product_link, table_count)  ' TONS OF FIELDS
    '        BuildOnLoadJavascript(this_section_string, BuildOperatorDataTableJS("createJsFlightTable", "flightData", "flightsDataSet", 5, False, TotalFlights, IIf(cfolder_id > 0 Or aport_id > 0, True, False), operator_list))
    '      End If
    '      Session("Last_FBO_Bottom") = 4
    '    ElseIf active_tab = 5 Then
    '      Call Build_Operators_Array(searchCriteria, "view", IIf(Not String.IsNullOrEmpty(airport_list), 0, aport_id), this_section_string, Me.operator_drop2.SelectedValue, table_count, TotalFlights)
    '      BuildOnLoadJavascript(this_section_string, BuildOperatorDataTableJS("createOperatorTable", "startTableOper2", "currentDataSet", 1, False, TotalFlights, IIf(cfolder_id > 0 Or aport_id > 0, True, False), operator_list))
    '    ElseIf active_tab = 17 Then
    '      Utilization_Build_Summaries(util_functions, searchCriteria, product_code_selection, TotalFlights)
    '    End If


    '    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />" & Date.Now & " - FBO View After Functions<br />"

    '  Catch ex As Exception

    '  End Try

    'End Sub

    'Public Sub Utilization_Build_Summaries(ByRef util_functions As utilization_functions, ByVal searchCriteria As viewSelectionCriteriaClass, ByVal product_code_selection As String, ByVal TotalFlights As Long, ByRef utilization_summaries_label As Label, ByVal utilization_summary_type As DropDownList)
    '  Dim tempTable As New DataTable
    '  Dim googleText As String = "<p><!--Chart goes here.--></p>"
    '  utilization_summaries_label.Text = ""
    '  'Model Report

    '  Select Case utilization_summary_type.SelectedValue
    '    Case "refuel"
    '      tempTable = util_functions.GetRefuel(searchCriteria, product_code_selection, TotalFlights)

    '      'write js to screen
    '      System.Web.UI.ScriptManager.RegisterClientScriptBlock(top_tab_update_panel, Me.GetType(), "WriteRefuelArray", util_functions.RefuelJSArray(tempTable, searchCriteria), True)

    '      System.Web.UI.ScriptManager.RegisterClientScriptBlock(top_tab_update_panel, Me.GetType(), "CreateRefuelDT", BuildRefuelOperatorDataTableJS(), True)
    '      'System.Web.UI.ScriptManager.RegisterClientScriptBlock(top_tab_update_panel, Me.GetType(), "CreateRefuelDT", crea, True)ui

    '      'utilization_summaries_label.Text = "<div class=""row""><div class=""twelve columns""><div style=""width:940px"">" & ReportRefuel(tempTable, searchCriteria) & "</div></div></div>"
    '    Case Else
    '      tempTable = util_functions.get_flight_activity_by_model(searchCriteria, product_code_selection, "view", TotalFlights, True)
    '      utilization_summaries_label.Text = "<div class=""row""><div class=""twelve columns""><div style=""width:940px"">" & ConvertDataTableToHTML(tempTable) & "</div></div></div>"

    '      UtilizationReportTable()
    '  End Select

    'End Sub


    'Private Function BuildRefuelOperatorDataTableJS() As String
    '  Dim TableBuild As New StringBuilder
    '  Dim TotalSorting As New StringBuilder
    '  Dim TopNumber As Integer = 0
    '  Dim BottomNumber As Integer = 0
    '  Dim EntriesText As String = "entries"
    '  Dim StartType As String = ""

    '  TableBuild.Append("function refuelSummaryTable() {" & vbNewLine)
    '  TableBuild.Append("jQuery('#refuelSummary').removeClass();")
    '  TableBuild.Append("var table = jQuery('#refuelSummary').DataTable({" & vbNewLine)
    '  TableBuild.Append("destroy:true,dom: 'Bilrtfp', paging: true, pageLength: 100, " & vbNewLine)

    '  TableBuild.Append("data: refuelDataSet, " & vbNewLine)
    '  TableBuild.Append("scrollY: 430," & vbNewLine)
    '  TableBuild.Append("scrollX: 960," & vbNewLine)

    '  TableBuild.Append("dom: 'Bfitrp'," & vbNewLine)

    '  TableBuild.Append("scrollCollapse:true," & vbNewLine)
    '  TableBuild.Append("scroller:true," & vbNewLine)
    '  TableBuild.Append("deferRender: true, " & vbNewLine)

    '  TableBuild.Append("processing: true, autoWidth: false," & vbNewLine)

    '  TableBuild.Append("columns: [ " & vbNewLine)

    '  TableBuild.Append("{ title: ""Aircraft"", data: ""ac"", width: ""60px"" }, " & vbNewLine)
    '  TableBuild.Append("{ title: ""Ser#"", width: ""50px"",className: """", data:{  ")
    '  TableBuild.Append("_:    ""ser.0"",")
    '  TableBuild.Append("sort: ""ser.1"",")
    '  TableBuild.Append("} }, ")
    '  TableBuild.Append("{ title: ""Reg#"", width: ""50px"", className: """",data:""reg"" }, " & vbNewLine)
    '  TableBuild.Append("{ title: ""Based at"", width: ""50px"", className: """", data:""based"" }, " & vbNewLine)
    '  TableBuild.Append("{ title: ""Departed"", width: ""40px"", className: """", data:""depart1"" }, " & vbNewLine)
    '  TableBuild.Append("{ title: ""From"", width: ""100px"", className: """", data:""departaport1"" }, " & vbNewLine)
    '  TableBuild.Append("{ title: ""Distance (sm)"", width: ""60px"", className: ""text_align_right"", data:""distance1"" }, " & vbNewLine)
    '  TableBuild.Append("{ title: ""Arrived"", width: ""60px"", className: """", data:""arrived1"" }, ")
    '  TableBuild.Append("{ title: ""To"", width: ""100px"", className: """", data:""arrivedaport1"" }, ")
    '  TableBuild.Append("{ title: ""Minutes On Ground"", width: ""100px"", className: ""text_align_right"", data:""onground"" }, " & vbNewLine)
    '  TableBuild.Append("{ title: ""Departed"", width: ""100px"", className: """", data:""departed2"" }, ")
    '  TableBuild.Append("{ title: ""Distance (sm)"", width: ""100px"", className: ""text_align_right"",  data:""distance2"" }, " & vbNewLine)
    '  TableBuild.Append("{ title: ""Arrived"", width: ""100px"", className: """", data:""arrived2"" }, " & vbNewLine)
    '  TableBuild.Append("{ title: ""To"", width: ""100px"", className: """", data:""arrivedaport2"" } " & vbNewLine)

    '  TableBuild.Append("]," & vbNewLine)

    '  TableBuild.Append("buttons: [ " & vbNewLine)

    '  Dim excelButton As String = ""
    '  'PDF Button
    '  TableBuild.Append(" {extend: 'pdf', orientation: 'landscape', pageSize: 'A2', exportOptions : {columns: ':visible'}}, ")
    '  TableBuild.Append("{extend: 'csv', exportOptions : {columns: ':visible'}}, ")
    '  'Excel Button
    '  CreateExcelButton(excelButton, "refuelSummary")

    '  TableBuild.Append(excelButton & vbNewLine)
    '  TableBuild.Append("]")
    '  TableBuild.Append("});}refuelSummaryTable();")


    '  TableBuild.Append("setTimeout(function(){$($.fn.dataTable.tables(true)).DataTable().columns.adjust();")
    '  TableBuild.Append("$($.fn.dataTable.tables(true)).DataTable().scroller.measure();}, 1000);")

    '  Return TableBuild.ToString
    'End Function

    'Private Sub UtilizationReportTable()
    '  'Let's turn this generic table into a js datatable.
    '  Dim jsScript As String = ""
    '  Dim excelButton As String = ""
    '  jsScript = "function createSummaryTable() { $('#summary_table').DataTable({"
    '  jsScript += " destroy: true,"
    '  jsScript += "scrollX: 940,"
    '  jsScript += "autoWidth: true, "
    '  jsScript += "scrollY: 430,"
    '  jsScript += " fixedHeader: true, "
    '  jsScript += " scrollCollapse: true,"
    '  jsScript += " stateSave: true,"
    '  jsScript += "paging: false, "
    '  jsScript += "dom: 'Bfitrp',"
    '  jsScript += "order: [[ 0, 'asc' ]],"

    '  If utilization_summary_type.SelectedValue = "refuel" Then
    '    jsScript += "columnDefs: ["
    '    jsScript += " {"
    '    jsScript += "targets: [ 3,5,8,13 ],"
    '    jsScript += " width: '100px',"
    '    jsScript += "},"
    '    jsScript += " {"
    '    jsScript += "targets: [ 0,1,2,4,6,7,9,10,11,12 ],"
    '    jsScript += " width: '54px',"
    '    jsScript += "}"
    '    jsScript += "],"
    '  End If

    '  jsScript += "buttons: [ "

    '  'PDF Button
    '  jsScript += " {extend: 'pdf', orientation: 'landscape', pageSize: 'A2', exportOptions : {columns: ':visible'}}, "
    '  jsScript += " {extend: 'csv', exportOptions : {columns: ':visible'}}, "
    '  'Excel Button
    '  CreateExcelButton(excelButton, "summaryTable")

    '  jsScript += excelButton
    '  jsScript += "]}); "
    '  jsScript += "$($.fn.dataTable.tables(true)).DataTable().columns.adjust();"
    '  jsScript += "$($.fn.dataTable.tables(true)).DataTable().scroller.measure();"
    '  jsScript += "}; createSummaryTable();"

    '  'Written to page:
    '  System.Web.UI.ScriptManager.RegisterClientScriptBlock(top_tab_update_panel, Me.GetType(), "CreateUtilDatatableFunction", jsScript.ToString, True)


    'End Sub


    'Public Sub CreateExcelButton(ByRef ExcelButton As String, ByVal PanelName As String)
    '  Dim PlaceholderString As String = ""

    '  ExcelButton = "var panel = document.getElementById(""" & PanelName & """);"
    '  ExcelButton += "my_form = document.createElement('FORM');"
    '  ExcelButton += "my_form.name = 'myForm';"
    '  ExcelButton += "my_form.method = 'POST';"
    '  ExcelButton += "my_form.action = 'MacShell.aspx';"
    '  ExcelButton += "my_form.target = '_new';"
    '  ExcelButton += " my_tb = document.createElement('INPUT');"
    '  ExcelButton += "my_tb.type = 'HIDDEN';"
    '  ExcelButton += "my_tb.name = 'MacExport';"
    '  ExcelButton += "my_tb.value = true;"
    '  ExcelButton += "my_form.appendChild(my_tb);"

    '  ExcelButton += " my_tb = document.createElement('INPUT');"
    '  ExcelButton += "my_tb.type = 'HIDDEN';"
    '  ExcelButton += "my_tb.name = 'data';"
    '  ExcelButton += "my_tb.value = panel.innerHTML;"
    '  ExcelButton += "my_form.appendChild(my_tb);"
    '  ExcelButton += " document.body.appendChild(my_form);"
    '  ExcelButton += "  my_form.submit();"



    '  If Not IsNothing(Session.Item("localUser").crmPlatformOS) Then
    '    If Not String.IsNullOrEmpty(Session.Item("localUser").crmPlatformOS) Then
    '      If InStr(Session.Item("localUser").crmPlatformOS, "mac") > 0 Then
    '        PlaceholderString += ", { text:'Excel', "
    '        PlaceholderString += " action: function( e, dt, node, config) {" & ExcelButton & "}},"
    '      Else
    '        PlaceholderString += " {extend: 'excel', exportOptions : {columns: ':visible'}}, "
    '      End If
    '    Else
    '      PlaceholderString += " {extend: 'excel', exportOptions : {columns: ':visible'}}, "
    '    End If
    '  Else
    '    PlaceholderString += " {extend: 'excel', exportOptions : {columns: ':visible'}}, "
    '  End If
    '  ExcelButton = PlaceholderString
    'End Sub

    'Private Function BuildOperatorDataTableJS(ByVal jsFunctionName As String, ByVal tableName As String, ByVal jsArrayName As String, ByVal columnSet As Integer, ByVal ToggleDisplay As Boolean, ByVal TotalFlights As Long, ByVal cfolderSearch As Boolean, ByVal operator_List As String) As String
    '  Dim TableBuild As New StringBuilder
    '  Dim TotalSorting As New StringBuilder
    '  Dim TopNumber As Integer = 0
    '  Dim BottomNumber As Integer = 0
    '  Dim EntriesText As String = "entries"
    '  Dim StartType As String = ""

    '  TableBuild.Append("function " & jsFunctionName & "() {jQuery('#" & tableName & "').removeClass();var table = jQuery('#" & tableName & "').DataTable({destroy:true,dom: 'Bilrtfp', paging: true, pageLength: 100, ")

    '  If Session.Item("isMobile") = True Then
    '    TableBuild.Append("responsive:true,")
    '    TableBuild.Append("  responsive: {")
    '    TableBuild.Append("details: { ")
    '    TableBuild.Append("type:  'column', ")
    '    TableBuild.Append("target: -1 ")
    '    TableBuild.Append("} ")
    '    TableBuild.Append("},")
    '  End If

    '  TableBuild.Append("data: " & jsArrayName & ", ")
    '  TableBuild.Append("scrollY: 430,")
    '  TableBuild.Append("scrollX: 960,")

    '  TableBuild.Append("dom: 'Bfitrp',")



    '  TableBuild.Append("scrollCollapse:true,")
    '  TableBuild.Append("scroller:true,")
    '  TableBuild.Append("deferRender: true, ")

    '  TableBuild.Append("processing: true, autoWidth: false,")

    '  TableBuild.Append(" ""language"": {")


    '  If columnSet = 1 Or columnSet = 2 Then
    '    'We need to replace the entries text depending on lots of things:
    '    If columnSet = 1 Then
    '      StartType = "Operators"
    '    ElseIf columnSet = 2 Then
    '      If ToggleDisplay Then
    '        StartType = "Aircraft"
    '      Else
    '        StartType = "Airports"
    '      End If
    '    End If


    '    If TotalFlights > 10000 And operator_List = "" Then
    '      EntriesText = StartType & " with at least 5 flights"
    '    ElseIf TotalFlights > 50000 And operator_List = "" Then
    '      EntriesText = StartType & " with at least 5 flights (top 500)"
    '    ElseIf TotalFlights > 1000 And operator_List = "" Then
    '      EntriesText = StartType & " with at least 2 flights"
    '    Else
    '      EntriesText = StartType
    '    End If
    '  End If


    '  If Trim(tableName) = "flightData" Then
    '    TableBuild.Append("""emptyTable"": ""No Flights available for " & IIf(cfolderSearch = False, "these selections.", "these folder selections.") & """,")
    '    TableBuild.Append("""info"": ""Latest _TOTAL_ Flights"",")
    '    TableBuild.Append("""infoEmpty"": ""Latest 0 to 0 of 0 " & EntriesText & """")
    '  ElseIf Trim(jsFunctionName) = "createOriginTable" Then
    '    TableBuild.Append("""emptyTable"": ""No Airports available for " & IIf(cfolderSearch = False, "these selections.", "these folder selections.") & """,")
    '    TableBuild.Append("""info"": ""_TOTAL_ Airports"",")
    '    TableBuild.Append("""infoEmpty"": ""0 Airports""")
    '  ElseIf Trim(jsFunctionName) = "createJsModelTable" Then
    '    TableBuild.Append("""emptyTable"": ""No Models available for " & IIf(cfolderSearch = False, "these selections.", "these folder selections.") & """,")
    '    TableBuild.Append("""info"": ""_TOTAL_ Models"",")
    '    TableBuild.Append("""infoEmpty"": ""0 Models""")
    '  ElseIf Trim(jsFunctionName) = "createJsACTable" Then
    '    TableBuild.Append("""emptyTable"": ""No Aircraft available for " & IIf(cfolderSearch = False, "these selections.", "these folder selections.") & """,")
    '    TableBuild.Append("""info"": ""_TOTAL_ Aircraft"",")
    '    TableBuild.Append("""infoEmpty"": ""0 Aircraft""")
    '  Else
    '    TableBuild.Append("""emptyTable"": ""No " & EntriesText & " available for " & IIf(cfolderSearch = False, "these selections.", "these folder selections.") & """,")
    '    TableBuild.Append("""info"": ""Showing _START_ to _END_ of _TOTAL_ " & EntriesText & """,")
    '    TableBuild.Append("""infoEmpty"": ""Showing 0 to 0 of 0 " & EntriesText & """")
    '  End If


    '  TableBuild.Append("},")

    '  TableBuild.Append("columns: [ ")


    '  TableBuild.Append("{ title: ""SEL"", width: ""20px"", data: ""check""}, ")
    '  If columnSet = 1 Then
    '    TableBuild.Append("{ title: ""Operator"", data: ""operator"", width: ""60px"" }, ")
    '    TableBuild.Append("{ title: ""City"", width: ""50px"",className: """", data:""city"" }, ")
    '    TableBuild.Append("{ title: ""State"", width: ""50px"", className: """",data:""state"" }, ")
    '    TableBuild.Append("{ title: ""Country"", width: ""50px"", className: """", data:""country"" }, ")
    '    TableBuild.Append("{ title: ""Nbr Flights"", width: ""50px"", className: ""text_align_right"", data:""flights"" }, ")
    '    TableBuild.Append("{ title: ""Total Flight Hours"", width: ""50px"", className: ""text_align_right"", data:""hours"" }, ")
    '    TableBuild.Append("{ title: ""Est Fuel Burn"", width: ""100px"",className: ""text_align_right"", data:""fuel"" } ")

    '    'What needs to be totalled here? Column 5,6,7
    '    TopNumber = 7
    '    BottomNumber = 5
    '  ElseIf columnSet = 2 Then
    '    If ToggleDisplay Then
    '      TableBuild.Append("{ title: ""Origin"", width: ""150px"",className: """", data:""origin"" }, ")
    '      TableBuild.Append("{ title: ""Destination"", width: ""150px"",className: """", data:""destination"" }, ")

    '      'What needs to be totalled here? Column 3,4,5
    '      TopNumber = 5
    '      BottomNumber = 3
    '    Else
    '      TableBuild.Append("{ title: ""IATA"", width: ""50px"",className: """", data:""iata"" }, ")
    '      TableBuild.Append("{ title: ""ICAO"", width: ""50px"",className: """", data:""icao"" }, ")
    '      TableBuild.Append("{ title: ""Destination Airport"", width: ""50px"",className: """", data:""airport"" }, ")

    '      'What needs to be totalled here? Column 4,5,6
    '      TopNumber = 6
    '      BottomNumber = 4
    '    End If

    '    TableBuild.Append("{ title: ""Nbr Flights"", width: ""50px"", className: ""text_align_right"", data:""flights"" }, ")
    '    TableBuild.Append("{ title: ""Total Flight Hours"", width: ""50px"", className: ""text_align_right"", data:""hours"" }, ")
    '    TableBuild.Append("{ title: ""Est Fuel Burn"", width: ""100px"",className: ""text_align_right"", data:""fuel"" } ")

    '  ElseIf columnSet = 3 Then
    '    TableBuild.Append("{ title: ""Model"", width: ""50px"",className: """", data:""model"" }, ")
    '    TableBuild.Append("{ title: ""Nbr Flights"", width: ""50px"", className: ""text_align_right"", data:""flights"" }, ")
    '    TableBuild.Append("{ title: ""Total Flight Hours"", width: ""50px"", className: ""text_align_right"", data:""hours"" }, ")
    '    TableBuild.Append("{ title: ""Est Fuel Burn"", width: ""100px"",className: ""text_align_right"", data:""fuel"" } ")

    '    'What needs to be totalled here? Column 2,3,4
    '    TopNumber = 4
    '    BottomNumber = 2
    '  ElseIf columnSet = 4 Then
    '    TableBuild.Append("{ title: ""Aircraft"", width: ""50px"",className: """", data:""ac"" }, ")
    '    TableBuild.Append("{ title: ""Ser#"", width: ""50px"",className: """", data:{  ")
    '    TableBuild.Append("_:    ""ser.0"",")
    '    TableBuild.Append("sort: ""ser.1"",")
    '    TableBuild.Append("} }, ")
    '    TableBuild.Append("{ title: ""Reg#"", width: ""50px"",className: """", data:""reg"" }, ")
    '    TableBuild.Append("{ title: ""Nbr Flights"", width: ""50px"", className: ""text_align_right"", data:""flights"" }, ")
    '    TableBuild.Append("{ title: ""Total Flight Hours"", width: ""50px"", className: ""text_align_right"", data:""hours"" }, ")
    '    TableBuild.Append("{ title: ""Est Fuel Burn"", width: ""100px"",className: ""text_align_right"", data:""fuel"" } ")

    '    'What needs to be totalled here? Column 4,5,6
    '    TopNumber = 6
    '    BottomNumber = 4
    '  ElseIf columnSet = 5 Then
    '    TableBuild.Append("{ title: ""Aircraft"", width: ""50px"",className: """", data:""ac"" }, ")
    '    TableBuild.Append("{ title: ""Ser#"", width: ""50px"",className: """", data:{  ")
    '    TableBuild.Append("_:    ""ser.0"",")
    '    TableBuild.Append("sort: ""ser.1"",")
    '    TableBuild.Append("} }, ")
    '    TableBuild.Append("{ title: ""Reg#"", width: ""50px"",className: """", data:""reg"" }, ")
    '    TableBuild.Append("{ title: ""Date"", width: ""50px"", className: """", data:""date"" }, ")
    '    TableBuild.Append("{ title: ""Origin Airport"", width: ""50px"", className: """", data:""origin"" }, ")
    '    TableBuild.Append("{ title: ""Flight Time"", width: ""50px"",className: ""text_align_right"", data:""time"" }, ")
    '    TableBuild.Append("{ title: ""Dist(sm)"", width: ""50px"",className: ""text_align_right"", data:""distance"" } ")

    '    'What needs to be totalled here? None
    '    TopNumber = 0
    '    BottomNumber = 0
    '  End If

    '  TableBuild.Append("],")

    '  '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '  'Building the footer
    '  If BottomNumber > 0 And TopNumber > 0 Then
    '    TableBuild.Append("""footerCallback"": function ( row, data, start, end, display ) {")
    '    TableBuild.Append("var api = this.api(), data;")

    '    '// Remove the formatting to get integer data for summation
    '    TableBuild.Append("var intVal = function ( i ) {")
    '    TableBuild.Append("return typeof i === 'string' ?")
    '    TableBuild.Append("i.replace(/[\$,]/g, '')*1 :")
    '    TableBuild.Append("typeof i === 'number' ?")
    '    TableBuild.Append("i : 0;")
    '    TableBuild.Append("};")

    '    'Let's build the total string:

    '    '// Total over all pages
    '    TotalSorting = New StringBuilder

    '    For x = BottomNumber To TopNumber
    '      TotalSorting.Append("total = api")
    '      TotalSorting.Append(".column(" & x & ")")
    '      TotalSorting.Append(".data()")
    '      TotalSorting.Append(".reduce( function (a, b) {")
    '      TotalSorting.Append("return intVal(a) + intVal(b);")
    '      TotalSorting.Append("}, 0 );")

    '      '// Update footer
    '      TotalSorting.Append("if (Math.round(total) !== total) {")
    '      TotalSorting.Append("total = total.toFixed(2);")
    '      TotalSorting.Append("}")

    '      TotalSorting.Append("$( api.column(" & x & ").footer() ).html('<span>' + ")
    '      TotalSorting.Append("total.toLocaleString('en')")

    '      TotalSorting.Append("+ '</span>');")
    '    Next

    '    TotalSorting.Append("$( api.column(" & BottomNumber - 1 & ").footer() ).html(")
    '    TotalSorting.Append("'Totals:'")
    '    TotalSorting.Append(");")

    '    TableBuild.Append(TotalSorting.ToString)
    '    TableBuild.Append("},")
    '  End If
    '  '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


    '  TableBuild.Append("""columnDefs"": [ ")
    '  TableBuild.Append(" {")
    '  TableBuild.Append("orderable: false,")
    '  TableBuild.Append("className:  'select-checkbox',")
    '  TableBuild.Append(" width: '10px',")
    '  TableBuild.Append("targets:   0")
    '  TableBuild.Append(" }")
    '  TableBuild.Append(" ],")
    '  TableBuild.Append("select: {")
    '  TableBuild.Append("style:    'multi',")
    '  TableBuild.Append("selector: 'td:first-child'")
    '  TableBuild.Append("},")
    '  TableBuild.Append("buttons: [ ")
    '  TableBuild.Append(BuildButtonString(jsFunctionName))
    '  TableBuild.Append("]")
    '  TableBuild.Append("});};" & jsFunctionName & "();")


    '  TableBuild.Append("setTimeout(function(){$($.fn.dataTable.tables(true)).DataTable().columns.adjust();")
    '  TableBuild.Append("$($.fn.dataTable.tables(true)).DataTable().scroller.measure();}, 1000);")

    '  Return TableBuild.ToString
    'End Function


    'Private Sub BuildOnLoadJavascript(ByVal currentTableArray As String, ByVal TableBuild As String)

    '  TableBuild = "var hideFromExport = [0];" & TableBuild

    '  Dim JavascriptPostback As String = ""
    '  If Page.IsPostBack Then
    '    JavascriptPostback += currentTableArray.ToString
    '    JavascriptPostback += TableBuild.ToString
    '    System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me.bottom_tab_update_panel, Me.GetType(), "resetTable", JavascriptPostback, True)
    '  Else
    '    If Not Page.ClientScript.IsClientScriptBlockRegistered("operatorArrayLoad") Then
    '      Dim JavascriptOnLoad As String = ""
    '      JavascriptOnLoad = vbCrLf & "if (window.addEventListener) {"
    '      JavascriptOnLoad += vbCrLf & " window.addEventListener(""load"", "
    '      JavascriptOnLoad += vbCrLf & "function () {"


    '      'function goes here.
    '      JavascriptOnLoad += currentTableArray.ToString
    '      JavascriptOnLoad += TableBuild.ToString
    '      JavascriptOnLoad += vbCrLf & ";$(""body"").removeClass(""loading"");}, false); "
    '      JavascriptOnLoad += vbCrLf & "}" 'Else 
    '      JavascriptOnLoad += vbCrLf & "else {"

    '      JavascriptOnLoad += vbCrLf & " window.attachEvent(""load"","
    '      JavascriptOnLoad += vbCrLf & "function () {"
    '      JavascriptOnLoad += currentTableArray.ToString
    '      JavascriptOnLoad += TableBuild.ToString
    '      JavascriptOnLoad += vbCrLf & ";$(""body"").removeClass(""loading"");});"

    '      JavascriptOnLoad += vbCrLf & "}" 'End if

    '      If Not Page.ClientScript.IsClientScriptBlockRegistered("onLoadCode") Then
    '        System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "onLoadCode", JavascriptOnLoad.ToString, True)
    '      End If
    '    End If
    '  End If

    'End Sub

    'Private Function BuildButtonString(ByRef JSFunctionName As String) As String
    '  Dim ButtonsString As New StringBuilder
    '  Dim exportOptions As String = ""
    '  Dim ExcelButton As String = ""
    '  exportOptions += "columns: [function ( idx, data, node ) {"
    '  exportOptions += "var isVisible = table.column( idx ).visible();"
    '  exportOptions += "if ((typeof hideFromExport === 'undefined') || (hideFromExport === null))"
    '  exportOptions += " { var isNotForExport = false;} "
    '  exportOptions += " else {"
    '  exportOptions += "  var isNotForExport = $.inArray( idx, hideFromExport ) !== -1;"
    '  exportOptions += " };"
    '  exportOptions += "return isVisible && !isNotForExport ? true : false; "
    '  'ExportOptions += "}"
    '  exportOptions += "}, 'colvis']"

    '  'CreateExcelButton(excelButton, "refuelSummary")

    '  'CSV Button:
    '  ButtonsString.Append("{")
    '  ButtonsString.Append("extend:  'csv',")
    '  ButtonsString.Append("exportOptions: {")
    '  ButtonsString.Append(exportOptions)
    '  ButtonsString.Append("}")
    '  ButtonsString.Append("}, ")
    '  'Excel Button
    '  ButtonsString.Append("{extend: 'excel', ")
    '  ButtonsString.Append("exportOptions: {")
    '  ButtonsString.Append(exportOptions)
    '  ButtonsString.Append("}")
    '  ButtonsString.Append("},")
    '  'PDF Button
    '  ButtonsString.Append(" {extend: 'pdf', orientation: 'landscape', pageSize: 'A2', ")
    '  ButtonsString.Append("exportOptions: {")
    '  ButtonsString.Append(exportOptions)
    '  ButtonsString.Append("}")
    '  ButtonsString.Append("}, ")
    '  'Column Visibility Button
    '  ButtonsString.Append("{")
    '  ButtonsString.Append("extend: 'colvis', text: 'Columns',")
    '  ButtonsString.Append("collectionLayout:  'fixed two-column',")
    '  ButtonsString.Append("postfixButtons: [ 'colvisRestore' ]")
    '  ButtonsString.Append("},")


    '  'ButtonsString.Append("{")
    '  'ButtonsString.Append("extend: 'colvis',")
    '  'ButtonsString.Append(" text: 'Columns',")
    '  'ButtonsString.Append("collectionLayout:  'fixed two-column',")
    '  'ButtonsString.Append("postfixButtons: [ 'colvisRestore' ]")
    '  'ButtonsString.Append("},")

    '  'Remove Selected Button:

    '  ButtonsString.Append("{ text:'Remove Selected', ")
    '  ButtonsString.Append(" action: function( e, dt, node, config) { dt.rows( { selected: true } ).remove().draw( false );}},")
    '  ButtonsString.Append("{ text:'Keep Selected', ")
    '  ButtonsString.Append(" action: function( e, dt, node, config) { dt.rows( { selected: false } ).remove().draw( false );dt.draw();dt.rows('.selected').deselect();}},")

    '  ButtonsString.Append("{ text:'Reload Table', action: function( e, dt, node, config) { $('#startTable').empty();" & JSFunctionName & "();}}")


    '  Return ButtonsString.ToString

    'End Function
End Class
