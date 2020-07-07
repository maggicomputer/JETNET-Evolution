' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/Entity Classes/airport_fbo_functions.vb $
'$$Author: Amanda $
'$$Date: 6/24/20 4:23p $
'$$Modtime: 6/24/20 3:33p $
'$$Revision: 16 $
'$$Workfile: airport_fbo_functions.vb $
'
' ********************************************************************************

<System.Serializable()> Public Class airport_fbo_view_functions

  Private aError As String
  Private clientConnectString As String
  Private adminConnectString As String

  Private starConnectString As String
  Private cloudConnectString As String
  Private serverConnectString As String
  Public Airport_ID_OVERALL As Integer

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

#Region "airport_fbo_functions"

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
      End If

      sQuery.Append(" FROM FAA_Flight_Data WITH(NOLOCK)  ")
      sQuery.Append(" INNER JOIN aircraft WITH(NOLOCK) ON (ac_id = ffd_ac_id) AND ac_journ_id = 0  ")
      sQuery.Append(" INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id  ")

      If Trim(type_of) = "Month" Then

      ElseIf Trim(type_of) = "BWeight" Or Trim(type_of) = "TWeight" Or Trim(type_of) = "HWeight" Then
        sQuery.Append(" inner join Aircraft_Weight_Class WITH(NOLOCK)on amod_type_code=acwgtcls_maketype and amod_weight_class=acwgtcls_code ")
      ElseIf Trim(type_of) = "Type" Then
        sQuery.Append(" INNER JOIN Aircraft_Type WITH(NOLOCK) on amod_type_code=atype_code ")
      End If

      sQuery.Append(" WHERE ffd_hide_flag= 'N' ")

      If Trim(use_faa_date) = "" Then
        temp_date = DateAdd(DateInterval.Year, 0, Date.Now)
      Else
        temp_date = DateAdd(DateInterval.Year, 0, CDate(use_faa_date))
      End If

      Call get_past_dates(temp_date, start_month, start_month_back, end_month, end_month_back)


      If go_back_farther = True Then
        sQuery.Append(" and ffd_date >= ('" & start_month_back & "') ")
        sQuery.Append(" and ffd_date <= ('" & end_month_back & "') ")
      Else
        sQuery.Append(" and ffd_date >= ('" & start_month & "') ")
        sQuery.Append(" and ffd_date <= ('" & end_month & "') ")
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


      If Trim(product_code_selection) <> "" Then
        sQuery.Append(product_code_selection)
      End If

      sQuery.Append(" and ffd_origin_aport_id > 0")
      ' sQuery.Append(" AND (ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportIATA & "' or ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportICAO & "')")
      sQuery.Append(" AND (ffd_dest_aport_id = '" & Airport_ID_OVERALL & "')")


      If Trim(type_of) = "BWeight" Then
        sQuery.Append(" AND amod_type_code ='J'and amod_airframe_type_code = 'F'  ")
      ElseIf Trim(type_of) = "TWeight" Then
        sQuery.Append(" AND amod_type_code ='T'and amod_airframe_type_code = 'F' ")
      ElseIf Trim(type_of) = "HWeight" Then
        sQuery.Append(" AND amod_airframe_type_code = 'R' ")
      End If

      If Trim(product_code_selection) <> "" Then
        sQuery.Append(product_code_selection)
      End If




      If Trim(type_of) = "Month" Then
        sQuery.Append(" group by year(ffd_date), month(ffd_date) ")
        sQuery.Append(" ORDER BY year(ffd_date), month(ffd_date) ")
      ElseIf Trim(type_of) = "BWeight" Or Trim(type_of) = "TWeight" Or Trim(type_of) = "HWeight" Then
        sQuery.Append(" group by acwgtcls_name ")
        sQuery.Append(" order by COUNT(distinct ffd_unique_flight_id) desc ")
      ElseIf Trim(type_of) = "Type" Then
        sQuery.Append(" group by amod_airframe_type_code, atype_name ")
        sQuery.Append(" order by COUNT(distinct ffd_unique_flight_id) desc ")
      End If




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
      sQuery.Append(" SELECT DISTINCT count(*) AS tflights ")
      sQuery.Append(" FROM FAA_Flight_Data WITH(NOLOCK)  ")
      sQuery.Append(" INNER JOIN aircraft WITH(NOLOCK) ON (ac_id = ffd_ac_id) AND ac_journ_id = 0  ")
      sQuery.Append(" INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id  ")
      sQuery.Append(" INNER JOIN Airport WITH(NOLOCK) ON ffd_origin_aport_id = aport_id  ")


      temp_date = DateAdd(DateInterval.Year, -1, Date.Now)
      temp_date = Month(temp_date) & "/01/" & Year(temp_date)

      sQuery.Append(" WHERE ffd_date >= ('" & temp_date & "')   and ffd_hide_flag= 'N'  ")

      ' sQuery.Append(" AND (ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportIATA & "' or ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportICAO & "') ")
      sQuery.Append(" AND (ffd_dest_aport_id = '" & Airport_ID_OVERALL & "') ")

      If Trim(product_code_selection) <> "" Then
        sQuery.Append(product_code_selection)
      End If

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

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />" & Date.Now & " - get_flight_activity_overall(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />" & Date.Now & " - get_flight_activity_overall(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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


      ElseIf Trim(get_by) = "Radius" Then


        sQuery.Append(" select distinct zmap_latitude, zmap_longitude ")
        sQuery.Append(" from Company with (NOLOCK)  ")
        sQuery.Append(" inner join Zip_Mapping with (NOLOCK) on left(comp_zip_code,5) = zmap_zip_code  ")
        sQuery.Append(" where comp_id = 135887 And comp_journ_id = 0 ")


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
      sQuery.Append(" aport_name, aport_country, aport_city,aport_id,  aport_state, count(*) AS tflights ")
      sQuery.Append(" FROM FAA_Flight_Data WITH(NOLOCK) ")

      'sQuery.Append(" INNER JOIN aircraft WITH(NOLOCK) ON (ac_id = ffd_ac_id) AND ac_journ_id = 0  ")
      ' sQuery.Append(" INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id  ")

      sQuery.Append(" INNER JOIN aircraft_flat WITH(NOLOCK) ON (ac_id = ffd_ac_id) AND ac_journ_id = 0 ")
      sQuery.Append(" INNER JOIN Airport WITH(NOLOCK) ON ffd_dest_aport_id = aport_id ")
      sQuery.Append(" WHERE ffd_date >= (getdate()-" & (searchCriteria.ViewCriteriaTimeSpan * 30) & ")  ")


      sQuery.Append(" and ffd_hide_flag= 'N' and ffd_dest_aport <> '' ")
      'sQuery.Append(" AND (ffd_origin_aport = '" & searchCriteria.ViewCriteriaAirportIATA & "' or ffd_origin_aport = '" & searchCriteria.ViewCriteriaAirportICAO & "') ")
      sQuery.Append(" AND (ffd_origin_aport_id = '" & Airport_ID_OVERALL & "') ")

      If Trim(product_code_selection) <> "" Then
        sQuery.Append(product_code_selection)
      End If

      sQuery.Append(" AND ((amod_type_code IN ('T','P') AND amod_customer_flag = 'Y'  ")
      sQuery.Append(" AND amod_airframe_type_code = 'R' AND amod_product_helicopter_flag = 'Y')  ")
      sQuery.Append(" OR (amod_customer_flag = 'Y' AND amod_airframe_type_code = 'F'  ")
      sQuery.Append(" AND amod_product_business_flag = 'Y') OR (amod_customer_flag = 'Y'  ")
      sQuery.Append(" AND amod_airframe_type_code = 'F' AND amod_product_commercial_flag = 'Y')) ")


      If searchCriteria.ViewCriteriaAmodID > -1 Then
        sQuery.Append(crmWebClient.Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
        sQuery.Append(crmWebClient.Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
      End If

      sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))


      sQuery.Append(" group by aport_iata_code, aport_icao_code, ffd_origin_aport_id, ")
      sQuery.Append(" aport_name, aport_country,aport_id,  aport_city, aport_state ")
      sQuery.Append(" order by COUNT(*) desc ")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />" & Date.Now & " - get_most_common_destinations(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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

  Public Function get_most_common_origins(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal product_code_selection As String) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      '-- ***************  UPPER RIGHT TAB 2 - TOP ORIGINS/DESTINATIONS ************************
      '-- # FLIGHT ACTIVITY -MOST COMMON ORIGINS - WE WOULD ALSO DO SAME FOR DESTINATIONS
      sQuery.Append(" select DISTINCT  top 25  aport_id, aport_iata_code as IATA, aport_icao_code as ICAO,ffd_origin_aport_id, ")
      sQuery.Append(" aport_name, aport_country, aport_city, aport_state, count(*) AS tflights ")
      sQuery.Append(" FROM FAA_Flight_Data WITH(NOLOCK) ")

      sQuery.Append(" INNER JOIN aircraft_flat WITH(NOLOCK) ON (ac_id = ffd_ac_id) AND ac_journ_id = 0 ")
      'sQuery.Append(" INNER JOIN aircraft WITH(NOLOCK) ON (ac_id = ffd_ac_id) AND ac_journ_id = 0  ")
      ' sQuery.Append(" INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id  ")

      sQuery.Append(" INNER JOIN Airport WITH(NOLOCK) ON ffd_origin_aport_id = aport_id ")
      sQuery.Append(" WHERE ffd_date >= (getdate()-" & (searchCriteria.ViewCriteriaTimeSpan * 30) & ")  ")
      sQuery.Append(" and ffd_origin_aport_id > 0  and ffd_hide_flag= 'N' ")


      ' sQuery.Append(" AND (ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportIATA & "' or ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportICAO & "') ")
      sQuery.Append(" AND (ffd_dest_aport_id = '" & Airport_ID_OVERALL & "') ")

      If Trim(product_code_selection) <> "" Then
        sQuery.Append(product_code_selection)
      End If

      sQuery.Append(" AND ((amod_type_code IN ('T','P') AND amod_customer_flag = 'Y'  ")
      sQuery.Append(" AND amod_airframe_type_code = 'R' AND amod_product_helicopter_flag = 'Y')  ")
      sQuery.Append(" OR (amod_customer_flag = 'Y' AND amod_airframe_type_code = 'F'  ")
      sQuery.Append(" AND amod_product_business_flag = 'Y') OR (amod_customer_flag = 'Y'  ")
      sQuery.Append(" AND amod_airframe_type_code = 'F' AND amod_product_commercial_flag = 'Y')) ")


      If searchCriteria.ViewCriteriaAmodID > -1 Then
        sQuery.Append(crmWebClient.Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
        sQuery.Append(crmWebClient.Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
      End If

      sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))


      sQuery.Append(" group by aport_iata_code, aport_icao_code, ffd_origin_aport_id, ")
      sQuery.Append(" aport_name, aport_country,aport_id,  aport_city, aport_state ")
      sQuery.Append(" order by COUNT(*) desc ")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />" & Date.Now & " - get_most_common_origins(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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

    Public Function get_nearby_airports(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal temp_distance As Integer, ByVal org_latitude As Double, ByVal org_longitude As Double, ByVal use_controlled As Boolean, Optional ByVal order_by As String = "") As DataTable

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

            If Trim(order_by) = "distance" Then
                sQuery.Append(",  (cast( case when aport_latitude_decimal > " & org_latitude & " then aport_latitude_decimal - " & org_latitude & " else " & org_latitude & " - aport_latitude_decimal end  as float) +  ")
                sQuery.Append("cast( case when aport_longitude_decimal > " & org_longitude & " then aport_longitude_decimal - " & org_longitude & " else " & org_longitude & " - aport_longitude_decimal end as float)) As lat_long_difference  ")
            End If

            sQuery.Append("from Airport with (NOLOCK) ")
            sQuery.Append("where aport_active_flag='Y' ")
            sQuery.Append("and aport_latitude_full <> '' ")
            sQuery.Append("and (aport_iata_code <> '' or aport_icao_code <> '') ")

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
            sQuery.Append("order by ")

            If Trim(order_by) = "distance" Then
                sQuery.Append("   (cast( case when aport_latitude_decimal > " & org_latitude & " then aport_latitude_decimal - " & org_latitude & " else " & org_latitude & " - aport_latitude_decimal end  as float) +  ")
                sQuery.Append("cast( case when aport_longitude_decimal > " & org_longitude & " then aport_longitude_decimal - " & org_longitude & " else " & org_longitude & " - aport_longitude_decimal end as float)) ,  ")
            End If

            sQuery.Append("  aport_name Asc, aport_city, aport_state, aport_country ")

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

    Public Function get_flight_activity_by_ac(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal show_not_based As Boolean, ByVal product_code_selection As String) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      '-- ***************  UPPER RIGHT TAB 3 - TOP MODELS ************************
      '-- # FLIGHT ACTIVITY BY MODEL
      sQuery.Append(" SELECT DISTINCT top 50 amod_make_name, amod_model_name, ac_ser_no_full, ac_reg_no, ac_aport_name, ac_id, count(*) AS tflights  ")
      sQuery.Append(" FROM View_FAA_Flight_Data_Clean WITH(NOLOCK)  ")
      ' sQuery.Append(" INNER JOIN aircraft WITH(NOLOCK) ON (ac_id = ffd_ac_id) AND ac_journ_id = 0  ")
      ' sQuery.Append(" INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id  ")
      sQuery.Append(" INNER JOIN aircraft_flat WITH(NOLOCK) ON (ac_id = ffd_ac_id) AND ac_journ_id = 0 ")
      sQuery.Append(" WHERE ffd_date >= (getdate()-" & (searchCriteria.ViewCriteriaTimeSpan * 30) & ")  ")
      'sQuery.Append(" AND (ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportIATA & "' or ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportICAO & "') ")
      sQuery.Append(" AND (ffd_dest_aport_id = '" & Airport_ID_OVERALL & "') ")


      sQuery.Append("  and ffd_hide_flag= 'N' AND ((amod_type_code IN ('T','P') AND amod_customer_flag = 'Y'  ")
      sQuery.Append(" AND amod_airframe_type_code = 'R' AND amod_product_helicopter_flag = 'Y')  ")
      sQuery.Append(" OR (amod_customer_flag = 'Y' AND amod_airframe_type_code = 'F'  ")
      sQuery.Append(" AND amod_product_business_flag = 'Y') OR (amod_customer_flag = 'Y'  ")
      sQuery.Append(" AND amod_airframe_type_code = 'F' AND amod_product_commercial_flag = 'Y')) ")

      If Trim(product_code_selection) <> "" Then
        sQuery.Append(product_code_selection)
      End If

      '-- SORT # 2 IS JUST BY MAKE AND MODEL NOT BY NUMBER OF FLIGHTS
      '' sQuery.Append(" order by amod_make_name, amod_model_name, amod_id ")

      If searchCriteria.ViewCriteriaAmodID > -1 Then
        sQuery.Append(crmWebClient.Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
        sQuery.Append(crmWebClient.Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
      End If

      sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))

      If show_not_based = True Then
        sQuery.Append(" and ac_id not in ( ")
        sQuery.Append(" select distinct ac_id from View_Aircraft_Flat with (NOLOCK)  ")
        sQuery.Append(" where (ac_aport_iata_code = '" & searchCriteria.ViewCriteriaAirportIATA & "' or ac_aport_icao_code = '" & searchCriteria.ViewCriteriaAirportICAO & "') and ac_journ_id = 0) ")
      End If

      sQuery.Append(" group by amod_make_name, amod_model_name, ac_ser_no_full, ac_aport_name, ac_reg_no, ac_id ")
      sQuery.Append(" order by COUNT(*) desc ")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />" & Date.Now & " - get_flight_activity_by_model(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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

  Public Function GET_USER_AIRPORTS(ByVal aport_ids As String, ByVal selected_value As String, ByVal latest_faa_date As String, Optional ByVal orderByTopFlights As Boolean = False) As DataTable

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
          old_date = FormatDateTime(DateAdd(DateInterval.Day, -730, CDate(latest_faa_date)), DateFormat.ShortDate)
          old_date = FormatDateTime(DateAdd(DateInterval.Day, 1, CDate(old_date)), DateFormat.ShortDate)
          mid_date = FormatDateTime(DateAdd(DateInterval.Year, -1, CDate(latest_faa_date)), DateFormat.ShortDate)
        Else
          old_date = FormatDateTime(DateAdd(DateInterval.Year, -2, Now()), DateFormat.ShortDate)
          old_date = FormatDateTime(DateAdd(DateInterval.Day, 1, CDate(old_date)), DateFormat.ShortDate)
          mid_date = FormatDateTime(DateAdd(DateInterval.Year, -1, Now()), DateFormat.ShortDate)
        End If
      ElseIf Trim(selected_value) = "90" Then
        If Trim(latest_faa_date) <> "" Then
          old_date = FormatDateTime(DateAdd(DateInterval.Day, -180, CDate(latest_faa_date)), DateFormat.ShortDate)
          old_date = FormatDateTime(DateAdd(DateInterval.Day, 1, CDate(old_date)), DateFormat.ShortDate)
          mid_date = FormatDateTime(DateAdd(DateInterval.Day, -90, CDate(latest_faa_date)), DateFormat.ShortDate)
        Else
          old_date = FormatDateTime(DateAdd(DateInterval.Day, -180, Now()), DateFormat.ShortDate)
          old_date = FormatDateTime(DateAdd(DateInterval.Day, 1, CDate(old_date)), DateFormat.ShortDate)
          mid_date = FormatDateTime(DateAdd(DateInterval.Day, -90, Now()), DateFormat.ShortDate)
        End If
      Else
        If Trim(latest_faa_date) <> "" Then
          mid_date = FormatDateTime(DateAdd(DateInterval.Year, -1, CDate(latest_faa_date)), DateFormat.ShortDate)
          old_date = CDate("1/1/" + Year(mid_date).ToString)
        Else
          mid_date = FormatDateTime(DateAdd(DateInterval.Year, -1, Now()), DateFormat.ShortDate)
          old_date = CDate("1/1/" + Year(mid_date).ToString)
        End If
      End If

      sql = sql & " SELECT aport_id As APortId, "
      sql = sql & " COALESCE(aport_iata_code,'') As IATACode, "
      sql = sql & " COALESCE(aport_icao_code,'') As ICAOCode,"
      sql = sql & " COALESCE(aport_faaid_code,'') As FAAIdCode,"
      sql = sql & " COALESCE(aport_name,'') As APortName,"
      sql = sql & " COALESCE(aport_city,'') As APortCity,"
      sql = sql & " COALESCE(aport_state,'') As APortState,"
      sql = sql & " COALESCE(aport_country,'') As APortCountry,"

      'Edits: 10/29/15: Amanda.
      'The multiple groups of two subqueries down below have been changed per instruction 
      'to use the aircraft flat table instead of the join to the aircraft and aircraft model.

      If selected_value.Contains("365") Or String.IsNullOrEmpty(selected_value) Then
        sql = sql & " (SELECT COUNT(ffd_unique_flight_id)"
        sql = sql & " FROM FAA_Flight_Data WITH (NOLOCK)"
        sql = sql & " INNER JOIN Aircraft_Flat WITH (NOLOCK) ON ac_id = ffd_ac_id AND ac_journ_id = 0"

        sql = sql & " WHERE(ffd_dest_aport_id = aport_id)"
        sql = sql & " AND (ffd_hide_flag = 'N')"
        sql = sql & " AND (ffd_date BETWEEN '" + old_date.Trim + "' AND '" + mid_date.Trim + "')"
        sql = sql & commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False)
        sql = sql & " ) As previousperiod,"

        mid_date = DateAdd(DateInterval.Day, 1, CDate(mid_date))

        sql = sql & " (SELECT COUNT(ffd_unique_flight_id)"
        sql = sql & " FROM FAA_Flight_Data WITH (NOLOCK)"
        sql = sql & " INNER JOIN Aircraft_Flat WITH (NOLOCK) ON ac_id = ffd_ac_id AND ac_journ_id = 0"

        sql = sql & " WHERE(ffd_dest_aport_id = aport_id)"
        sql = sql & " AND (ffd_hide_flag = 'N')"
        sql = sql & " AND (ffd_date BETWEEN '" + mid_date.Trim + "' AND '" + latest_faa_date.Trim + "')"
        sql = sql & commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False)
        sql = sql & " ) As currentperiod"

      ElseIf selected_value.Contains("90") Then

        sql = sql & " (SELECT COUNT(ffd_unique_flight_id)"
        sql = sql & " FROM FAA_Flight_Data WITH (NOLOCK)"
        sql = sql & " INNER JOIN Aircraft_Flat WITH (NOLOCK) ON ac_id = ffd_ac_id AND ac_journ_id = 0"

        sql = sql & " WHERE(ffd_dest_aport_id = aport_id)"
        sql = sql & " AND (ffd_hide_flag = 'N')"
        sql = sql & " AND (ffd_date BETWEEN '" + old_date.Trim + "' AND '" + mid_date.Trim + "')"
        sql = sql & commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False)
        sql = sql & " ) As previousperiod,"

        mid_date = DateAdd(DateInterval.Day, 1, CDate(mid_date))

        sql = sql & " (SELECT COUNT(ffd_unique_flight_id)"
        sql = sql & " FROM FAA_Flight_Data WITH (NOLOCK)"
        sql = sql & " INNER JOIN Aircraft_Flat WITH (NOLOCK) ON ac_id = ffd_ac_id AND ac_journ_id = 0 "

        sql = sql & " WHERE(ffd_dest_aport_id = aport_id)"
        sql = sql & " AND (ffd_hide_flag = 'N')"
        sql = sql & " AND (ffd_date BETWEEN '" + mid_date.Trim + "' AND '" + latest_faa_date.Trim + "')"
        sql = sql & commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False)
        sql = sql & " ) As currentperiod"

      Else

        sql = sql & " (SELECT COUNT(ffd_unique_flight_id)"
        sql = sql & " FROM FAA_Flight_Data WITH (NOLOCK)"
        sql = sql & " INNER JOIN Aircraft_Flat WITH (NOLOCK) ON ac_id = ffd_ac_id AND ac_journ_id = 0"

        sql = sql & " WHERE(ffd_dest_aport_id = aport_id)"
        sql = sql & " AND (ffd_hide_flag = 'N')"
        sql = sql & " AND (ffd_date BETWEEN '" + old_date.Trim + "' AND '" + mid_date.Trim + "')"
        sql = sql & commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False)
        sql = sql & " ) As previousperiod,"

        old_date = DateAdd(DateInterval.Year, 1, CDate(old_date)) ' now will be current year

        sql = sql & " (SELECT COUNT(ffd_unique_flight_id)"
        sql = sql & " FROM FAA_Flight_Data WITH (NOLOCK)"
        sql = sql & " INNER JOIN Aircraft_Flat WITH (NOLOCK) ON ac_id = ffd_ac_id AND ac_journ_id = 0"

        sql = sql & " WHERE(ffd_dest_aport_id = aport_id)"
        sql = sql & " AND (ffd_hide_flag = 'N')"
        sql = sql & " AND (ffd_date BETWEEN '" + old_date.Trim + "' AND '" + latest_faa_date.Trim + "')"
        sql = sql & commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False)
        sql = sql & " ) As currentperiod"

      End If

      sql = sql & " FROM Airport WITH(NOLOCK)"
      sql = sql & " WHERE aport_id IN (" + aport_ids.Trim + ") AND (aport_active_flag = 'Y')"

      If Not orderByTopFlights Then
        sql = sql & " ORDER BY aport_name ASC"
      Else
        sql = sql & " ORDER BY currentperiod DESC"
      End If

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
  Public Function GET_USER_AIRPORTS_NEW_RANGES(ByVal aport_ids As String, ByVal selected_value As String, ByVal latest_faa_date As String, Optional ByVal orderByTopFlights As Boolean = False) As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader : SqlReader = Nothing
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim sql As String = ""
    Dim old_date As String = ""
    Dim mid_date As String = ""

    Try


      sql = sql & " SELECT aport_id As APortId, "
      sql = sql & " COALESCE(aport_iata_code,'') As IATACode, "
      sql = sql & " COALESCE(aport_icao_code,'') As ICAOCode,"
      sql = sql & " COALESCE(aport_faaid_code,'') As FAAIdCode,"
      sql = sql & " COALESCE(aport_name,'') As APortName,"
      sql = sql & " COALESCE(aport_city,'') As APortCity,"
      sql = sql & " COALESCE(aport_state,'') As APortState,"
      sql = sql & " COALESCE(aport_country,'') As APortCountry,"
      sql = sql & " '' as previousperiod, "
      'Edits: 10/29/15: Amanda.
      'The multiple groups of two subqueries down below have been changed per instruction 
      'to use the aircraft flat table instead of the join to the aircraft and aircraft model.

      If IsNumeric(Trim(selected_value)) = True Then

        mid_date = FormatDateTime(DateAdd(DateInterval.Month, -CInt(selected_value), Date.Now()), DateFormat.ShortDate)

        sql = sql & " (SELECT COUNT(ffd_unique_flight_id)"
        sql = sql & " FROM FAA_Flight_Data WITH (NOLOCK)"
        sql = sql & " INNER JOIN Aircraft_Flat WITH (NOLOCK) ON ac_id = ffd_ac_id AND ac_journ_id = 0"

        sql = sql & " WHERE(ffd_dest_aport_id = aport_id)"
        sql = sql & " AND (ffd_hide_flag = 'N')"
        sql = sql & " AND (ffd_date >= '" & mid_date.ToString.Trim & "')"
        sql = sql & commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False)
        sql = sql & " ) As currentperiod"

      Else

        If Trim(Trim(selected_value)) = "YTD" Then
          If Trim(latest_faa_date) <> "" Then
            old_date = CDate("1/1/" + Year(Date.Now).ToString)
          Else
            old_date = CDate("1/1/" + Year(Date.Now).ToString)
          End If
        ElseIf Trim(selected_value) = "MTD" Then
          If Trim(latest_faa_date) <> "" Then
            old_date = CDate(Month(Date.Now) & "/1/" + Year(Date.Now).ToString)
          Else
            old_date = CDate(Month(Date.Now) & "/1/" + Year(Date.Now).ToString)
          End If
        End If


        sql = sql & " (SELECT COUNT(ffd_unique_flight_id)"
        sql = sql & " FROM FAA_Flight_Data WITH (NOLOCK)"
        sql = sql & " INNER JOIN Aircraft_Flat WITH (NOLOCK) ON ac_id = ffd_ac_id AND ac_journ_id = 0"

        sql = sql & " WHERE(ffd_dest_aport_id = aport_id)"
        sql = sql & " AND (ffd_hide_flag = 'N')"
        sql = sql & " AND (ffd_date >= '" + old_date.Trim + "' )"
        sql = sql & commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False)
        sql = sql & " ) As currentperiod"
      End If




      sql = sql & " FROM Airport WITH(NOLOCK)"
      sql = sql & " WHERE aport_id IN (" + aport_ids.Trim + ") AND (aport_active_flag = 'Y')"

      If Not orderByTopFlights Then
        sql = sql & " ORDER BY aport_name ASC"
      Else
        sql = sql & " ORDER BY currentperiod DESC"
      End If

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

  Public Function get_flight_activity_by_model(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal product_code_selection As String) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      '-- ***************  UPPER RIGHT TAB 3 - TOP MODELS ************************
      '-- # FLIGHT ACTIVITY BY MODEL
      sQuery.Append(" SELECT DISTINCT top 25 amod_make_name, amod_model_name, amod_id, count(*) AS tflights ")
      sQuery.Append(" FROM FAA_Flight_Data WITH(NOLOCK)  ")

      '  sQuery.Append(" INNER JOIN aircraft WITH(NOLOCK) ON (ac_id = ffd_ac_id) AND ac_journ_id = 0  ")
      '   sQuery.Append(" INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id  ")
      sQuery.Append(" INNER JOIN aircraft_flat WITH(NOLOCK) ON (ac_id = ffd_ac_id) AND ac_journ_id = 0 ")
      sQuery.Append(" WHERE ffd_date >= (getdate()-" & (searchCriteria.ViewCriteriaTimeSpan * 30) & ")  ")
      'sQuery.Append(" AND (ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportIATA & "' or ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportICAO & "') ")
      '
      sQuery.Append(" AND (ffd_dest_aport_id = '" & Airport_ID_OVERALL & "') ")

      sQuery.Append("  and ffd_hide_flag= 'N' AND ((amod_type_code IN ('T','P') AND amod_customer_flag = 'Y'  ")
      sQuery.Append(" AND amod_airframe_type_code = 'R' AND amod_product_helicopter_flag = 'Y')  ")
      sQuery.Append(" OR (amod_customer_flag = 'Y' AND amod_airframe_type_code = 'F'  ")
      sQuery.Append(" AND amod_product_business_flag = 'Y') OR (amod_customer_flag = 'Y'  ")
      sQuery.Append(" AND amod_airframe_type_code = 'F' AND amod_product_commercial_flag = 'Y')) ")


      If Trim(product_code_selection) <> "" Then
        sQuery.Append(product_code_selection)
      End If



      '-- SORT # 2 IS JUST BY MAKE AND MODEL NOT BY NUMBER OF FLIGHTS
      '' sQuery.Append(" order by amod_make_name, amod_model_name, amod_id ")

      If searchCriteria.ViewCriteriaAmodID > -1 Then
        sQuery.Append(crmWebClient.Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
        sQuery.Append(crmWebClient.Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
      End If

      sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))

      sQuery.Append(" group by amod_make_name, amod_model_name, amod_id ")
      sQuery.Append(" order by COUNT(*) desc  ")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />" & Date.Now & " - get_flight_activity_by_model(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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

      If Trim(run_export) <> "" Then
        sQuery.Append(", comp_zip_code  as ZIP_CODE ")
        sQuery.Append(", comp_email_address  as COMP_EMAIL, (select top 1 pnum_number_full from Phone_Numbers with (NOLOCK) where pnum_comp_id = comp_id and pnum_journ_id = 0 and pnum_contact_id = 0 and pnum_type = 'Office' and pnum_hide_customer = 'N') as OFFICE_PHONE ")
        sQuery.Append(", (contact_first_name + ' ' + contact_last_name) as CONTACT_NAME, contact_email_address as CONTACT_EMAIL, ")
        sQuery.Append("(select top 1 pnum_number_full  from  Phone_Numbers with (NOLOCK) where pnum_contact_id = contact_id and pnum_hide_customer = 'N' and pnum_type in ('Mobile','Office')) as CONTACT_PHONE ")
      End If

      If use_ac = True Then
        sQuery.Append(", ac_id As ACId, amod_make_name As Make, amod_model_name As Model")
        sQuery.Append(", ac_ser_no_full As SerNbr, ac_reg_no As RegNbr ")
      End If

      sQuery.Append(" , SUM(ffd_flight_time) as FLIGHT_TIME , COUNT(*) as TOTAL_COUNT ")
      sQuery.Append(" FROM View_FAA_Flight_Data_Clean WITH(NOLOCK) ")

      sQuery.Append(" INNER JOIN aircraft_flat WITH(NOLOCK) ON (ac_id = ffd_ac_id) AND ac_journ_id = 0 ")

      If Trim(selected_value) = "" Or Trim(selected_value) = "D" Then
        sQuery.Append(" left outer JOIN Airport WITH(NOLOCK) ON ffd_origin_aport_id = aport_id ")
        sQuery.Append(" left outer JOIN Airport a2 WITH(NOLOCK) ON ffd_dest_aport_id = a2.aport_id ")
      ElseIf Trim(selected_value) = "A" Then
        sQuery.Append(" INNER JOIN  Airport WITH(NOLOCK) ON ffd_origin_aport_id = aport_id ")
      End If

      If Trim(contact_type) = "36" Then
        sQuery.Append(" inner join Aircraft_Reference on cref_ac_id = ac_id and cref_journ_id = ac_journ_id and (cref_contact_type = '" & contact_type & "' or cref_operator_flag  in ('Y', 'O')) ")
      Else
        sQuery.Append(" inner join Aircraft_Reference on cref_ac_id = ac_id and cref_journ_id = ac_journ_id and cref_contact_type = '" & contact_type & "' ")
      End If



      sQuery.Append(" inner join Company with (NOLOCK) on comp_id = cref_comp_id and comp_journ_id = ac_journ_id  ")

      If Trim(run_export) <> "" Then
        sQuery.Append("  inner join Contact with (NOLOCK) on contact_comp_id = comp_id and contact_journ_id = 0 and contact_acpros_seq_no = 1  ")
      End If

      ' If use_date_range = True Then
      sQuery.Append(" WHERE ffd_date >= '" & start_date & "' and  ffd_date <= '" & end_date & "' and ffd_hide_flag= 'N'  ")
      ' ElseIf recent_flight_months = 0 Then
      ' sQuery.Append(" WHERE ffd_date >= (getdate()-90) and ffd_hide_flag= 'N'  ")
      '  Else
      '  sQuery.Append(" WHERE ffd_date >= (getdate()-" & (recent_flight_months * 30) & ")   and ffd_hide_flag= 'N'  ")
      ' End If

      If Trim(selected_value) = "" Or Trim(selected_value) = "A" Then
        sQuery.Append(" AND (ffd_dest_aport_id = '" & Airport_ID_OVERALL & "') ")
      ElseIf Trim(selected_value) = "D" Then
        sQuery.Append(" AND (ffd_origin_aport_id = '" & Airport_ID_OVERALL & "') ")
      End If


      If Trim(product_code_selection) <> "" Then
        sQuery.Append(product_code_selection)
      End If

      sQuery.Append(" AND ((amod_type_code IN ('T','P') AND amod_customer_flag = 'Y'  ")
      sQuery.Append(" AND amod_airframe_type_code = 'R' AND amod_product_helicopter_flag = 'Y') ")
      sQuery.Append(" OR (amod_customer_flag = 'Y' AND amod_airframe_type_code = 'F'  ")
      sQuery.Append(" AND amod_product_business_flag = 'Y') OR (amod_customer_flag = 'Y'  ")
      sQuery.Append(" AND amod_airframe_type_code = 'F' AND amod_product_commercial_flag = 'Y')) ")

      sQuery.Append(" and ffd_hide_flag= 'N' ")



      ' ADDED MSW - 3/10/20 TO BUILD IN DROP DOWN FOR IN OPERATION 
      If Not IsNothing(searchCriteria.viewCriteriaInOperation) Then
        If Trim(searchCriteria.viewCriteriaInOperation) <> "" Then
          Dim util_functions As New utilization_functions
          sQuery.Append(util_functions.Build_In_Operation_String(searchCriteria))
        End If
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

      If Trim(run_export) <> "" Then
        sQuery.Append(" , comp_email_address,  (contact_first_name + ' ' + contact_last_name) , contact_email_address, contact_id ")
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

  Public Function get_most_recent_flight_activity(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal run_export As String, ByVal selected_value As String, ByVal recent_flight_months As Integer, ByVal start_date As String, ByVal end_date As String, ByVal product_code_selection As String) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      '-- ***************  LOWER TAB 2 - RECENT FLIGHT ACTIVITY ************************
      '-- # FLIGHT ACTIVITY MOST RECENT

      If Trim(run_export) = "A" Then
        sQuery.Append(" select top 500 amod_make_name As Make, amod_model_name As Model, ")
        sQuery.Append(" ac_ser_no_full As SerNbr, ac_reg_no As RegNbr,  ffd_dest_date, ffd_origin_date, ")
        sQuery.Append(" ffd_date As FlightDate, ffd_origin_aport As OriginAPort,")
        sQuery.Append(" aport_name, aport_country, aport_city, aport_state,  ")
        sQuery.Append(" ffd_flight_time As FlightTime, ffd_distance As Distance ")
      Else
        sQuery.Append(" select top 500 ac_id As ACId, amod_make_name As Make, amod_model_name As Model, ")
        sQuery.Append(" ac_ser_no_full As SerNbr, ac_reg_no As RegNbr,  ffd_dest_date, ffd_origin_date,  ")
        sQuery.Append(" ffd_date As FlightDate, ffd_origin_aport As OriginAPort, ffd_dest_aport As DestinAPort,ffd_origin_aport_id,ffd_dest_aport_id, ")
        sQuery.Append(" airport.aport_name, airport.aport_country, airport.aport_city, airport.aport_state, airport.aport_id, ")
        If Trim(selected_value) = "" Or Trim(selected_value) = "D" Then
          sQuery.Append(" a2.aport_name as aport_name2, a2.aport_country as aport_country2, a2.aport_city as aport_city2, a2.aport_state as aport_state2, a2.aport_id as aport_id2, ")
        End If

        sQuery.Append(" ffd_flight_time As FlightTime, ffd_distance As Distance ")
      End If




      sQuery.Append(" FROM View_FAA_Flight_Data_Clean WITH(NOLOCK) ")
      sQuery.Append(" INNER JOIN aircraft_flat WITH(NOLOCK) ON (ac_id = ffd_ac_id) AND ac_journ_id = 0 ")
      ' sQuery.Append(" INNER JOIN aircraft WITH(NOLOCK) ON (ac_id = ffd_ac_id) AND ac_journ_id = 0  ")
      ' sQuery.Append(" INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id  ")

      If Trim(selected_value) = "" Or Trim(selected_value) = "D" Then
        sQuery.Append(" left outer JOIN Airport WITH(NOLOCK) ON ffd_origin_aport_id = aport_id ")
        sQuery.Append(" left outer JOIN Airport a2 WITH(NOLOCK) ON ffd_dest_aport_id = a2.aport_id ")
      ElseIf Trim(selected_value) = "A" Then
        sQuery.Append(" INNER JOIN  Airport WITH(NOLOCK) ON ffd_origin_aport_id = aport_id ")
      End If

      If Trim(start_date) <> "" And Trim(end_date) <> "" Then
        sQuery.Append(" WHERE  convert(date, ffd_date, 0) >= '" & start_date & "' and  convert(date, ffd_date, 0) <= '" & end_date & "' and ffd_hide_flag= 'N'  ")
        ' ElseIf recent_flight_months = 0 Then
      Else
        sQuery.Append(" WHERE ffd_date >= (getdate()-90) and ffd_hide_flag= 'N'  ")
        'sQuery.Append(" WHERE ffd_date >= (getdate()-" & (recent_flight_months * 30) & ")   and ffd_hide_flag= 'N'  ")
      End If

      If Trim(product_code_selection) <> "" Then
        sQuery.Append(product_code_selection)
      End If

      If Trim(selected_value) = "" Or Trim(selected_value) = "A" Then
        '   sQuery.Append(" AND ( ")
        '   sQuery.Append(" (ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportIATA & "' or ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportICAO & "') ")
        '   sQuery.Append(" or ")
        '   sQuery.Append(" (ffd_origin_aport = '" & searchCriteria.ViewCriteriaAirportIATA & "' or ffd_origin_aport = '" & searchCriteria.ViewCriteriaAirportICAO & "') ")
        '   sQuery.Append(" ) ")


        sQuery.Append(" AND (ffd_dest_aport_id = '" & Airport_ID_OVERALL & "') ")
        ' sQuery.Append(" AND (ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportIATA & "' or ffd_dest_aport = '" & searchCriteria.ViewCriteriaAirportICAO & "') ")
      ElseIf Trim(selected_value) = "D" Then
        'sQuery.Append(" AND (ffd_origin_aport = '" & searchCriteria.ViewCriteriaAirportIATA & "' or ffd_origin_aport = '" & searchCriteria.ViewCriteriaAirportICAO & "') ")
        sQuery.Append(" AND (ffd_origin_aport_id = '" & Airport_ID_OVERALL & "') ")
      End If


      sQuery.Append(" AND ((amod_type_code IN ('T','P') AND amod_customer_flag = 'Y'  ")
      sQuery.Append(" AND amod_airframe_type_code = 'R' AND amod_product_helicopter_flag = 'Y') ")
      sQuery.Append(" OR (amod_customer_flag = 'Y' AND amod_airframe_type_code = 'F'  ")
      sQuery.Append(" AND amod_product_business_flag = 'Y') OR (amod_customer_flag = 'Y'  ")
      sQuery.Append(" AND amod_airframe_type_code = 'F' AND amod_product_commercial_flag = 'Y')) ")

      sQuery.Append(" and ffd_hide_flag= 'N' ")

      If searchCriteria.ViewCriteriaAmodID > -1 Then
        sQuery.Append(crmWebClient.Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
        sQuery.Append(crmWebClient.Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
      End If

      sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))

      If Trim(selected_value) = "" Or Trim(selected_value) = "A" Then
        sQuery.Append(" order by ffd_origin_date desc ")
      Else
        sQuery.Append(" order by ffd_dest_date desc ")
      End If


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



      HttpContext.Current.Session.Item("Selection_Listing_Fields") = ""
      HttpContext.Current.Session.Item("Selection_Listing_Table") = ""
      HttpContext.Current.Session.Item("Selection_Listing_Where") = ""
      HttpContext.Current.Session.Item("Selection_Listing_Group") = ""
      HttpContext.Current.Session.Item("Selection_Listing_Order") = ""





      '-- ***************  LOWER TAB 3 - COMPANY DIRECTORY ************************
      '-- SELECT A LIST OF COMPANIES LOCATED AT SPECIFIC AIRPORT OR SAME CITY
      If Trim(run_export) = "A" Then
        sQuery.Append(" select comp_name as CompanyName, comp_address1 as Address, comp_city as City, comp_state as State, comp_web_address as WebAddress, comp_email_address  as EmailAddress")
        HttpContext.Current.Session.Item("Selection_Listing_Fields") = "comp_name as 'CompanyName', comp_address1 as 'Address', comp_city as 'City', comp_state as 'State', comp_web_address as 'WebAddress', comp_email_address  as 'EmailAddress'"
      Else
        sQuery.Append(" select comp_id, comp_name, comp_address1, comp_city, comp_state, comp_web_address, comp_email_address ")
        HttpContext.Current.Session.Item("Selection_Listing_Fields") = "comp_id as 'CompID', comp_name as 'CompanyName', comp_address1 as 'Address', comp_city as 'City', comp_state as 'State',  comp_web_address as 'WebAddress', comp_email_address  as 'EmailAddress'"
      End If


      sQuery.Append(" from Company with (NOLOCK) ")
      HttpContext.Current.Session.Item("Selection_Listing_Table") = "Company"

      If searchCriteria.ViewCriteriaCountry = "United States" Then
        sQuery.Append(" inner join Zip_Mapping with (NOLOCK) on left(comp_zip_code,5) = zmap_zip_code ")
        HttpContext.Current.Session.Item("Selection_Listing_Table") &= (" inner join Zip_Mapping with (NOLOCK) on left(comp_zip_code,5) = zmap_zip_code ")
      Else
        sQuery.Append(" left outer JOIN Airport WITH(NOLOCK)on comp_country = aport_country and comp_city=aport_city ")
        HttpContext.Current.Session.Item("Selection_Listing_Table") &= (" left outer JOIN Airport WITH(NOLOCK)on comp_country = aport_country and comp_city=aport_city ")
      End If


      If Trim(bus_type) <> "" Then
        sQuery.Append(" inner JOIN Business_Type_Reference WITH(NOLOCK)on comp_id = bustypref_comp_id and comp_journ_id = bustypref_journ_id  ")
        sQuery.Append(" inner JOIN Company_Business_Type WITH(NOLOCK)on bustypref_type= cbus_type ")

        HttpContext.Current.Session.Item("Selection_Listing_Table") &= (" inner JOIN Business_Type_Reference WITH(NOLOCK)on comp_id = bustypref_comp_id and comp_journ_id = bustypref_journ_id  ")
        HttpContext.Current.Session.Item("Selection_Listing_Table") &= (" inner JOIN Company_Business_Type WITH(NOLOCK)on bustypref_type= cbus_type ")
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

        HttpContext.Current.Session.Item("Selection_Listing_Where") = ("where (zmap_longitude <= " & max_WEST & " AND zmap_longitude >= " & max_EAST & ")  AND (zmap_latitude <= " & max_NORTH & " AND zmap_latitude >= " & max_SOUTH & ")  ")
        sQuery.Append(HttpContext.Current.Session.Item("Selection_Listing_Where"))
      Else

        HttpContext.Current.Session.Item("Selection_Listing_Where") = (" where (aport_id= '" & Airport_ID_OVERALL & "') ") ' or ")


        'If Trim(city_name) <> "" And Trim(country_name) <> "" Then
        '  sQuery.Append(" ( comp_city = '" & Trim(city_name) & "' and comp_country = '" & Trim(country_name) & "' ) )")
        '  HttpContext.Current.Session.Item("Selection_Listing_Where") &= (" ( comp_city = '" & Trim(city_name) & "' and comp_country = '" & Trim(country_name) & "' ) )")
        'ElseIf Trim(city_name) <> "" Then
        '  sQuery.Append("  comp_city = '" & Trim(city_name) & "'  )")
        '  HttpContext.Current.Session.Item("Selection_Listing_Where") &= ("  comp_city = '" & Trim(city_name) & "'  )")
        'ElseIf Trim(country_name) <> "" Then
        '  sQuery.Append("  comp_country = '" & Trim(country_name) & "' )")
        '  HttpContext.Current.Session.Item("Selection_Listing_Where") &= ("  comp_country = '" & Trim(country_name) & "' )")
        'End If


        sQuery.Append(HttpContext.Current.Session.Item("Selection_Listing_Where"))
      End If



      If searchCriteria.ViewCriteriaCountry = "United States" Then
        HttpContext.Current.Session.Item("Selection_Listing_Where") &= (" and comp_country='United States' ")
        sQuery.Append(" and comp_country='United States' ")
      End If


      ' ADDED MSW - 3/10/20 TO BUILD IN DROP DOWN FOR IN OPERATION 
      If Not IsNothing(searchCriteria.viewCriteriaInOperation) Then
        If Trim(searchCriteria.viewCriteriaInOperation) <> "" Then
          Dim util_functions As New utilization_functions
          sQuery.Append(util_functions.Build_In_Operation_String(searchCriteria))
        End If
      End If

      HttpContext.Current.Session.Item("Selection_Listing_Where") &= (" and comp_journ_id = 0 ")
      HttpContext.Current.Session.Item("Selection_Listing_Where") &= (" and comp_active_flag = 'Y' ")

      sQuery.Append(" and comp_journ_id = 0 ")
      sQuery.Append(" and comp_active_flag = 'Y' ")

      If Trim(bus_type) <> "" Then
        sQuery.Append(" and cbus_type in ('" & bus_type & "')")
        HttpContext.Current.Session.Item("Selection_Listing_Where") &= (" and cbus_type in ('" & bus_type & "')")
      End If

      sQuery.Append(" order by comp_name ")
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />" & Date.Now & " - get_companies_in_city(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString



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

  Public Function get_companies_from_airport(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal company_type As String, ByVal run_export As String, ByVal aport_id As Long, ByVal use_ac As Boolean) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try
      '-- ***************  LOWER TAB 4 - OWNERS ************************
      '-- SELECT A LIST OF COMPANIES OWNING AIRCRAFT AT SPECIFIC AIRPORT 

      HttpContext.Current.Session.Item("Selection_Listing_Fields") = ""
      HttpContext.Current.Session.Item("Selection_Listing_Table") = ""
      HttpContext.Current.Session.Item("Selection_Listing_Where") = ""
      HttpContext.Current.Session.Item("Selection_Listing_Group") = ""
      HttpContext.Current.Session.Item("Selection_Listing_Order") = ""




      If Trim(run_export) = "A" Then
        HttpContext.Current.Session.Item("Selection_Listing_Fields") &= (" select distinct comp_name as 'CompanyName', comp_address1 as 'Address', comp_city as 'City', comp_state as 'State', comp_web_address as 'WebAddress' ")

        If use_ac = True Then
          HttpContext.Current.Session.Item("Selection_Listing_Fields") &= (", ac_list_date as 'ListDate', amod_make_name as 'Make', amod_model_name as 'Model', ac_mfr_year as 'MFRYear', ac_forsale_flag as 'ForSale', ac_year as 'Year', ")
          HttpContext.Current.Session.Item("Selection_Listing_Fields") &= (" ac_ser_no_full as 'SerNbr', ac_reg_no as 'RegNbr'")
        End If

        If Trim(run_export) <> "" Then
          HttpContext.Current.Session.Item("Selection_Listing_Fields") &= (", comp_email_address  as 'COMP_EMAIL', (select top 1 pnum_number_full from Phone_Numbers with (NOLOCK) where pnum_comp_id = comp_id and pnum_journ_id = 0 and pnum_contact_id = 0 and pnum_type = 'Office' and pnum_hide_customer = 'N') as 'OFFICE_PHONE' ")
          HttpContext.Current.Session.Item("Selection_Listing_Fields") &= (", (contact_first_name + ' ' + contact_last_name) as 'CONTACT_NAME', contact_email_address as 'CONTACT_EMAIL', ")
          HttpContext.Current.Session.Item("Selection_Listing_Fields") &= ("(select top 1 pnum_number_full  from  Phone_Numbers with (NOLOCK) where pnum_contact_id = contact_id and pnum_hide_customer = 'N' and pnum_type in ('Mobile','Office')) as 'CONTACT_PHONE' ")
        End If

      ElseIf Trim(run_export) = "C" Then
        HttpContext.Current.Session.Item("Selection_Listing_Fields") &= (" select distinct comp_name as 'CompanyName', comp_address1 as 'Address', comp_city as 'City', comp_state as 'State', comp_web_address as 'WebAddress' ")
      Else
        HttpContext.Current.Session.Item("Selection_Listing_Fields") &= (" select distinct comp_id, comp_name, comp_address1, comp_city, comp_state, comp_zip_code, comp_web_address")

        If use_ac = True Then
          HttpContext.Current.Session.Item("Selection_Listing_Fields") &= (", ac_id as 'ACId', ac_list_date, amod_make_name as 'Make', amod_model_name as 'Model',amod_id, ac_mfr_year, ac_forsale_flag, ac_year, ")
          HttpContext.Current.Session.Item("Selection_Listing_Fields") &= (" ac_ser_no_full as 'SerNbr' ,ac_ser_no_sort, ac_reg_no as 'RegNbr' ")
        End If

      End If

      sQuery.Append(HttpContext.Current.Session.Item("Selection_Listing_Fields"))

      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "comp_id", "comp_id as 'COMPID'")
      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "comp_name", "comp_name as 'COMPNAME'")
      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "comp_address1", "comp_address1 as 'ADDRESS'")
      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "comp_city", "comp_city as 'CITY'")
      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "comp_state", "comp_state as 'STATE'")
      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "comp_web_address", "comp_web_address as 'WEBADDRESS'")
      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "ac_list_date", "ac_list_date as 'LSITDATE'")
      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "amod_id", "amod_id as 'MODELID'")
      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "ac_mfr_year", "ac_mfr_year as 'MFRYEAR'")
      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "ac_forsale_flag", "ac_forsale_flag as 'FORSALE'")
      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "ac_year", "ac_year as 'YEAR'")
      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "ac_ser_no_sort", "ac_ser_no_sort as 'SerNoSort'")
      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "comp_zip_code", "comp_zip_code as 'Zip'")





      HttpContext.Current.Session.Item("Selection_Listing_Table") = (" from View_Aircraft_Company_Flat with (NOLOCK)  ")
      sQuery.Append(HttpContext.Current.Session.Item("Selection_Listing_Table"))


      If aport_id > 0 Then
        HttpContext.Current.Session.Item("Selection_Listing_Where") = (" where ac_aport_id= '" & aport_id & "'  ")
      ElseIf Trim(searchCriteria.ViewCriteriaAirportIATA) <> "" Then
        HttpContext.Current.Session.Item("Selection_Listing_Where") = (" where ac_aport_iata_code = '" & searchCriteria.ViewCriteriaAirportIATA & "'  ")
      ElseIf Trim(searchCriteria.ViewCriteriaAirportICAO) <> "" Then
        HttpContext.Current.Session.Item("Selection_Listing_Where") = (" where ac_aport_icao_code = '" & searchCriteria.ViewCriteriaAirportICAO & "'  ")
      End If

      sQuery.Append(HttpContext.Current.Session.Item("Selection_Listing_Where"))

      If Trim(company_type) = "Owner" Then
        sQuery.Append(" and ( ( cref_contact_type in ('00','97','17','08','16') ) )  ")
        HttpContext.Current.Session.Item("Selection_Listing_Where") &= (" and ( ( cref_contact_type in ('00','97','17','08','16') ) )  ")
      ElseIf Trim(company_type) = "Operator" Then
        sQuery.Append(" and ( cref_operator_flag  in ('Y', 'O') )  ")
        sQuery.Append(" and exists (select ffd_origin_date from View_FAA_Flight_Data_Clean with (NOLOCK) where ffd_ac_id = ac_id and ffd_origin_date >= ac_purchase_date) ")

        HttpContext.Current.Session.Item("Selection_Listing_Where") &= (" and ( cref_operator_flag  in ('Y', 'O') )  ")
        HttpContext.Current.Session.Item("Selection_Listing_Where") &= (" and exists (select ffd_origin_date from View_FAA_Flight_Data_Clean with (NOLOCK) where ffd_ac_id = ac_id and ffd_origin_date >= ac_purchase_date) ")
      Else
        sQuery.Append(" and ( ( cref_contact_type in ('00','97','17','08','16') ) )  ")
        HttpContext.Current.Session.Item("Selection_Listing_Where") &= (" and ( ( cref_contact_type in ('00','97','17','08','16') ) )  ")
      End If



      sQuery.Append(" AND amod_customer_flag = 'Y' AND (( amod_product_business_flag = 'Y')  ")
      sQuery.Append(" OR ( amod_product_commercial_flag = 'Y') OR (amod_product_helicopter_flag = 'Y'))  ")
      sQuery.Append(" AND ( ac_product_business_flag = 'Y' OR ac_product_commercial_flag = 'Y'  ")
      sQuery.Append(" OR ac_product_helicopter_flag = 'Y')  ")


      ' ADDED MSW - 3/10/20 TO BUILD IN DROP DOWN FOR IN OPERATION 
      If Not IsNothing(searchCriteria.viewCriteriaInOperation) Then
        If Trim(searchCriteria.viewCriteriaInOperation) <> "" Then
          Dim util_functions As New utilization_functions
          sQuery.Append(util_functions.Build_In_Operation_String(searchCriteria))
        End If
      End If



      HttpContext.Current.Session.Item("Selection_Listing_Where") &= (" AND amod_customer_flag = 'Y' AND (( amod_product_business_flag = 'Y')  ")
      HttpContext.Current.Session.Item("Selection_Listing_Where") &= (" OR ( amod_product_commercial_flag = 'Y') OR (amod_product_helicopter_flag = 'Y'))  ")
      HttpContext.Current.Session.Item("Selection_Listing_Where") &= (" AND ( ac_product_business_flag = 'Y' OR ac_product_commercial_flag = 'Y'  ")
      HttpContext.Current.Session.Item("Selection_Listing_Where") &= (" OR ac_product_helicopter_flag = 'Y')  ")



      If searchCriteria.ViewCriteriaAmodID > -1 Then
        sQuery.Append(crmWebClient.Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
        HttpContext.Current.Session.Item("Selection_Listing_Where") &= (crmWebClient.Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
        sQuery.Append(crmWebClient.Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
        HttpContext.Current.Session.Item("Selection_Listing_Where") &= (crmWebClient.Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
      End If

      sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))
      HttpContext.Current.Session.Item("Selection_Listing_Where") &= (" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))

      If use_ac = False Then
        HttpContext.Current.Session.Item("Selection_Listing_Group") = (" group by ")

        If Trim(run_export) <> "A" And Trim(run_export) <> "C" Then
          HttpContext.Current.Session.Item("Selection_Listing_Group") &= (" comp_id, ")
        End If

        HttpContext.Current.Session.Item("Selection_Listing_Group") &= ("comp_name, comp_address1, comp_city, comp_state, comp_web_address ")

        If Trim(run_export) = "A" Then
          HttpContext.Current.Session.Item("Selection_Listing_Group") &= (" , comp_email_address,  (contact_first_name + ' ' + contact_last_name) , contact_email_address, contact_id, comp_id ")
        End If

        sQuery.Append(HttpContext.Current.Session.Item("Selection_Listing_Group"))
      End If

      HttpContext.Current.Session.Item("Selection_Listing_Order") = (" order by comp_name ")
      sQuery.Append(HttpContext.Current.Session.Item("Selection_Listing_Order"))


      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />" & Date.Now & " - get_companies_from_airport(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
      sQuery.Append(" and comp_state=aport_state  ")
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


      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />" & Date.Now & " - get_bus_type_from_companies_from_airport(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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

  Public Sub get_flight_profile_top_function(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, Optional ByVal type_of As String = "Month", Optional ByVal use_faa_date As String = "", Optional ByVal product_code_selection As String = "")

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
    Try

      If IsNothing(use_faa_date) Then
        use_faa_date = ""
      End If
      'use_faa_date
      results_table = get_flight_profile(searchCriteria, type_of, False, use_faa_date, product_code_selection)
      results_table2 = get_flight_profile(searchCriteria, type_of, True, use_faa_date, product_code_selection)



      htmlOut.Append("<table id=""fractionsExpiringOuterTable"" width=""100%"" cellpadding=""0"" cellspacing=""0"" class=""module"">")
      htmlOut.Append("<tr><td valign=""top"" align=""center"" class=""header"">Flight Profile</td></tr>")

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          htmlOut.Append("<tr><td valign=""top"" align=""left"">")

          htmlOut.Append("<table id=""fractionsExpiringInnerTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"">")

          htmlOut.Append("<tr><td colspan=""2"" class=""rightside"" valign=""top"">")
          htmlOut.Append("<div style=""height: 250px; overflow: auto;"">")

          htmlOut.Append("<table id=""fractionsExpiringDataTable"" width=""100%"" cellpadding=""4"" cellspacing=""0"">")
          htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""seperator"" width=""80%""><strong>Month/Year</strong></td>")
          htmlOut.Append("<td valign=""top"" align=""right"" class=""seperator"" width=""20%""><strong>#&nbsp;Flights</strong></td></tr>")

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

          Call get_past_dates(use_faa_date, start_date, mid_date2, mid_date, end_date)

          If Trim(type_of) = "Month" Then
            start_temp_string = " data1.addColumn('string', 'Month/Year'); "
            start_temp_string &= " data1.addColumn('number', '# Arrivals " & mid_date2 & " to " & end_date & "'); "
            start_temp_string &= " data1.addColumn('number', '# Arrivals " & start_date & " to " & mid_date & "'); "
            start_temp_string &= " data1.addColumn('number', 'Est/Sold Value'); "
            start_temp_string &= " data1.addColumn('number', 'My AC Asking'); "
            start_temp_string &= " data1.addColumn('number', 'My AC Take'); "
            start_temp_string &= " data1.addColumn('number', 'My AC Est Value'); "
          ElseIf Trim(type_of) = "BWeight" Or Trim(type_of) = "TWeight" Or Trim(type_of) = "HWeight" Then
            start_temp_string = " data1.addColumn('string', 'Weight'); "
            start_temp_string &= " data1.addColumn('number', '# Arrivals " & mid_date2 & " to " & end_date & "'); "
            start_temp_string &= " data1.addColumn('number', '# Arrivals " & start_date & " to " & mid_date & "'); "
          ElseIf Trim(type_of) = "Type" Then
            start_temp_string = " data1.addColumn('string', 'Type'); "
            start_temp_string &= " data1.addColumn('number', '# Arrivals " & mid_date2 & " to " & end_date & "'); "
            start_temp_string &= " data1.addColumn('number', '# Arrivals " & start_date & " to " & mid_date & "'); "
          End If



          start_temp_string &= "data1.addRows(["


          For Each r As DataRow In results_table.Rows

            If Not toggleRowColor Then
              htmlOut.Append("<tr class=""alt_row"">")
              toggleRowColor = True
            Else
              htmlOut.Append("<tr bgcolor=""white"">")
              toggleRowColor = False
            End If



            If Trim(type_of) = "Month" Then
              tcompare1 = r.Item("tmonth").ToString
              htmlOut.Append("<td align=""left"" valign=""top"" class=""seperator"" width=""80%"">")

              htmlOut.Append("" & r.Item("tmonth").ToString & "/" & r.Item("tyear").ToString & "</td>")
              htmlOut.Append("<td align=""right"" valign=""top"" class=""seperator"" width=""20%"" style=""padding-right:15px;"">" + r.Item("tcount").ToString + "</td></tr>")



              If Not IsDBNull(r.Item("tcount")) Then
                If IsNumeric(r.Item("tcount")) Then
                  If CInt(r.Item("tmonth").ToString) = Now.Month Then
                    If CDbl(r.Item("tcount")) = 0 Then
                      ' if its this month, and its 0, do nothing 
                    Else
                      If Trim(temp_string) <> "" Then
                        temp_string &= ","
                      End If
                      temp_string &= "['" & r.Item("tmonth").ToString + "-" + r.Item("tyear").ToString & "', XXXXX, " & Replace(r.Item("tcount").ToString, ",", "") & ", null,  null, null, null]"
                    End If
                  Else
                    If CDbl(r.Item("tcount")) = 0 Then
                      If Trim(temp_string) <> "" Then
                        temp_string &= ", "
                      End If
                      temp_string &= "['" & r.Item("tmonth").ToString + "-" + r.Item("tyear").ToString & "',XXXXX,0, null,  null, null, null]"
                    Else
                      If Trim(temp_string) <> "" Then
                        temp_string &= ", "
                      End If
                      temp_string &= "['" & r.Item("tmonth").ToString + "-" + r.Item("tyear").ToString & "',XXXXX, " & Replace(r.Item("tcount").ToString, ",", "") & ", null,  null, null, null]"
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
                      temp_string &= "['" & type_temp & "',XXXXX, " & Replace(r.Item("tflights").ToString, ",", "") & "]"
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
                    temp_string &= "['" & r.Item("type_name").ToString & "', XXXXX," & Replace(r.Item("tflights").ToString, ",", "") & "]"
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
    out_htmlString = start_temp_string & temp_string



    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Sub get_flight_activity_overall_top_function(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString_count As Long, ByVal product_code_selection As String)

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

  Public Sub get_most_common_origins_top_function(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByVal product_code_selection As String)

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder
    Dim toggleRowColor As Boolean = False

    Try


      results_table = get_most_common_origins(searchCriteria, product_code_selection)


      htmlOut.Append("<table id=""fractionsExpiringOuterTable"" width=""100%"" cellpadding=""0"" cellspacing=""0"" class=""module"">")
      htmlOut.Append("<tr><td valign=""top"" align=""center"" class=""header"">Top 25 Origins (Last Year)</td></tr>")

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          htmlOut.Append("<tr><td valign=""top"" align=""left"">")

          htmlOut.Append("<table id=""fractionsExpiringInnerTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"">")


          htmlOut.Append("<tr><td colspan=""7"" class=""rightside"" valign=""top"">")
          htmlOut.Append("<div style=""height: 250px; overflow: auto;"">")

          htmlOut.Append("<table id=""fractionsExpiringDataTable"" width=""100%"" cellpadding=""3"" cellspacing=""0"">")
          htmlOut.Append("<tr>")
          'htmlOut.Append("<td valign=""top"" align=""left"" class=""seperator""><strong>Original Airport</strong></td>")
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

            ' htmlOut.Append("<td align=""left"" valign=""top"" class=""seperator"">")
            ' htmlOut.Append("" & r.Item("OriginAPort").ToString & "</td>")
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

  Public Sub get_nearby_airports_top_function(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByVal temp_distance As Integer, ByVal org_longitude As Double, ByVal org_latitude As Double, ByVal bus_type As String, ByVal aport_id As Long, ByVal use_controlled As Boolean, ByVal UpdateProgressPanel As UpdateProgress, Optional ByVal location_of As String = "")

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder
    Dim toggleRowColor As Boolean = False
    Dim range_text As String = ""
    Dim view_id As Integer = 24
    Dim ViewName As String = "Airport FBO View"
    Dim id_list As New StringBuilder

    Try


            results_table = get_nearby_airports(searchCriteria, temp_distance, org_longitude, org_latitude, use_controlled, "distance")

            If Trim(location_of) = "util_view" Then
        view_id = 28
        ViewName = "Operator/Airport Utilization"
        ' range_text = "Change Range Miles from Currently Selected Airport to "
        '  range_text &= "<a href='view_template.aspx?ViewID=" & view_id & "&ViewName=" & ViewName & "&aport_id=" & aport_id & "&distance=25&activetab=19&bus_type=" & bus_type & "' onclick=""document.body.style.cursor='wait';$get('" & UpdateProgressPanel.ClientID & "').style.display = 'block';""><font color='white'><u>25</u></font></a>"
        '          range_text &= ", <a href='view_template.aspx?ViewID=" & view_id & "&ViewName=" & ViewName & "&aport_id=" & aport_id & "&distance=50&activetab=19&bus_type=" & bus_type & "' onclick=""document.body.style.cursor='wait';$get('" & UpdateProgressPanel.ClientID & "').style.display = 'block';""><font color='white'><u>50</u></font></a>"
        '          range_text &= ", <a href='view_template.aspx?ViewID=" & view_id & "&ViewName=" & ViewName & "&aport_id=" & aport_id & "&distance=75&activetab=19&bus_type=" & bus_type & "' onclick=""document.body.style.cursor='wait';$get('" & UpdateProgressPanel.ClientID & "').style.display = 'block';""><font color='white'><u>75</u></font></a>"
        '          range_text &= ", <a href='view_template.aspx?ViewID=" & view_id & "&ViewName=" & ViewName & "&aport_id=" & aport_id & "&distance=100&activetab=19&bus_type=" & bus_type & "' onclick=""document.body.style.cursor='wait';$get('" & UpdateProgressPanel.ClientID & "').style.display = 'block';""><font color='white'><u>100</u></font></a>"
        '          range_text &= ", <a href='view_template.aspx?ViewID=" & view_id & "&ViewName=" & ViewName & "&aport_id=" & aport_id & "&distance=150&activetab=19&bus_type=" & bus_type & "' onclick=""document.body.style.cursor='wait';$get('" & UpdateProgressPanel.ClientID & "').style.display = 'block';""><font color='white'><u>150</u></font></a>"
        '          range_text &= ", <a href='view_template.aspx?ViewID=" & view_id & "&ViewName=" & ViewName & "&aport_id=" & aport_id & "&distance=200&activetab=19&bus_type=" & bus_type & "' onclick=""document.body.style.cursor='wait';$get('" & UpdateProgressPanel.ClientID & "').style.display = 'block';""><font color='white'><u>200</u></font></a>&nbsp;"
      Else
        range_text = "Change Range Miles from Currently Selected Airport to "
        range_text &= "<a href='view_template.aspx?ViewID=24&ViewName=Airport FBO View&aport_id=" & aport_id & "&distance=25&top_active_tab=4&bus_type=" & bus_type & "' onclick=""document.body.style.cursor='wait';$get('" & UpdateProgressPanel.ClientID & "').style.display = 'block';""><font color='white'><u>25</u></font></a>"
        range_text &= ", <a href='view_template.aspx?ViewID=24&ViewName=Airport FBO View&aport_id=" & aport_id & "&distance=50&top_active_tab=4&bus_type=" & bus_type & "' onclick=""document.body.style.cursor='wait';$get('" & UpdateProgressPanel.ClientID & "').style.display = 'block';""><font color='white'><u>50</u></font></a>"
        range_text &= ", <a href='view_template.aspx?ViewID=24&ViewName=Airport FBO View&aport_id=" & aport_id & "&distance=75&top_active_tab=4&bus_type=" & bus_type & "' onclick=""document.body.style.cursor='wait';$get('" & UpdateProgressPanel.ClientID & "').style.display = 'block';""><font color='white'><u>75</u></font></a>"
        range_text &= ", <a href='view_template.aspx?ViewID=24&ViewName=Airport FBO View&aport_id=" & aport_id & "&distance=100&top_active_tab=4&bus_type=" & bus_type & "' onclick=""document.body.style.cursor='wait';$get('" & UpdateProgressPanel.ClientID & "').style.display = 'block';""><font color='white'><u>100</u></font></a>"
        range_text &= ", <a href='view_template.aspx?ViewID=24&ViewName=Airport FBO View&aport_id=" & aport_id & "&distance=150&top_active_tab=4&bus_type=" & bus_type & "' onclick=""document.body.style.cursor='wait';$get('" & UpdateProgressPanel.ClientID & "').style.display = 'block';""><font color='white'><u>150</u></font></a>"
        range_text &= ", <a href='view_template.aspx?ViewID=24&ViewName=Airport FBO View&aport_id=" & aport_id & "&distance=200&top_active_tab=4&bus_type=" & bus_type & "' onclick=""document.body.style.cursor='wait';$get('" & UpdateProgressPanel.ClientID & "').style.display = 'block';""><font color='white'><u>200</u></font></a>&nbsp;"
      End If


      If Not IsNothing(results_table) Then

        htmlOut.Append("<table id=""fractionsExpiringOuterTable"" width=""100%"" cellpadding=""0"" cellspacing=""0"" class=""module"">")
        If temp_distance = 0 Then
          htmlOut.Append("<tr><td valign=""top"" align=""center"" class=""header"">" & results_table.Rows.Count & " Nearby Airports (Within 150 Miles)")
          'this is what it defaults to - 150
        Else
          htmlOut.Append("<tr><td valign=""top"" align=""center"" class=""header"">" & results_table.Rows.Count & " Nearby Airports (Within " & temp_distance & " Miles)")
        End If



        If Trim(location_of) = "util_view" Then
          htmlOut.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a class=""underline"" title='Expand' ' onclick=""javascript:load('FolderMaintenance.aspx?t=17&newStaticFolder=true&id_list=XXXXX','','scrollbars=yes,menubar=no,height=700,width=1250,resizable=yes,toolbar=no,location=no,status=no');""/><font color='white'>Create Nearby Airports Folder</font></a></strong>")
        End If

        htmlOut.Append("</td></tr>")

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


            If id_list.ToString.Trim <> "" Then
              id_list.Append("," & r.Item("aport_id").ToString.Trim)
            Else
              id_list.Append(r.Item("aport_id").ToString.Trim)
            End If


            htmlOut.Append("<td align=""left"" valign=""top"" class=""seperator"">")

            If Trim(location_of) = "util_view" Then
              htmlOut.Append("<a href='view_template.aspx?ViewID=" & view_id & "&ViewName=" & ViewName & "&aport_id=" & r.Item("aport_id").ToString & "&activetab=18'  onclick=""document.body.style.cursor='wait';$get('" & UpdateProgressPanel.ClientID & "').style.display = 'block';"">")
            Else
              htmlOut.Append("<a href='view_template.aspx?ViewID=" & view_id & "&ViewName=" & ViewName & "&aport_id=" & r.Item("aport_id").ToString & "'  onclick=""document.body.style.cursor='wait';$get('" & UpdateProgressPanel.ClientID & "').style.display = 'block';"">")
            End If




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

    If Trim(location_of) = "util_view" Then
      out_htmlString = Replace(out_htmlString, "XXXXX", id_list.ToString)
    End If


    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Sub get_flight_activity_by_ac_top_function(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByVal show_not_based As Boolean, ByVal product_code_selection As String)

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder
    Dim toggleRowColor As Boolean = False

    Try


      results_table = get_flight_activity_by_ac(searchCriteria, show_not_based, product_code_selection)

      htmlOut.Append("<table id=""fractionsExpiringOuterTable"" width=""100%"" cellpadding=""0"" cellspacing=""0"" class=""module"">")
      htmlOut.Append("<tr><td valign=""top"" align=""center"" class=""header"">Top 50 Aircraft Flight Activity (Last " & searchCriteria.ViewCriteriaTimeSpan & " Months, Based on Arrivals)</td></tr>")

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          htmlOut.Append("<tr><td valign=""top"" align=""left"">")

          htmlOut.Append("<table id=""fractionsExpiringInnerTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"">")

          htmlOut.Append("<tr><td colspan=""2"" class=""rightside"" valign=""top"">")
          '  htmlOut.Append("<div style=""height: 250px; overflow: auto;"">")

          htmlOut.Append("<table id=""fractionsExpiringDataTable"" width=""100%"" cellpadding=""4"" cellspacing=""0"">")
          htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""seperator"" width=""50%""><strong>Aircraft</strong></td>")
          htmlOut.Append("<td valign=""top"" align=""left"" class=""seperator"" width=""30%""><strong>Aircraft Based At</strong></td>")
          htmlOut.Append("<td valign=""top"" align=""right"" class=""seperator"" width=""20%""><strong>#&nbsp;Flights</strong></td></tr>")



          For Each r As DataRow In results_table.Rows

            If Not toggleRowColor Then
              htmlOut.Append("<tr class=""alt_row"">")
              toggleRowColor = True
            Else
              htmlOut.Append("<tr bgcolor=""white"">")
              toggleRowColor = False
            End If

            htmlOut.Append("<td align=""left"" valign=""top"" class=""seperator"">")
            htmlOut.Append("" & r.Item("amod_make_name").ToString & " ")
            htmlOut.Append("" & r.Item("amod_model_name").ToString & "")
            htmlOut.Append(" S#: ")
            htmlOut.Append("<a class='underline' onclick=""javascript:load('DisplayAircraftDetail.aspx?acid=" + r.Item("ac_id").ToString + "&jid=0','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');"" title='Display Aircraft Details'>")
            htmlOut.Append("" & r.Item("ac_ser_no_full").ToString & "</a> ")
            htmlOut.Append("R#: " & r.Item("ac_reg_no").ToString & " ")
            htmlOut.Append("</td>")
            htmlOut.Append("<td align=""left"" valign=""top"" class=""seperator"" width=""20%"" style=""padding-right:15px;"">" + r.Item("ac_aport_name").ToString + "</td>")
            htmlOut.Append("<td align=""right"" valign=""top"" class=""seperator"" width=""20%"" style=""padding-right:15px;"">" + r.Item("tflights").ToString + "</td>")

            htmlOut.Append("</tr>")

          Next

          'htmlOut.Append("</table></div></td></tr></table></td></tr>")
          htmlOut.Append("</table></td></tr></table></td></tr>")
        Else
          htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""seperator"" style=""padding-left:3px;"">No FAA Flight Activity Found.</td></tr>")
        End If
      Else
        htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""seperator"" style=""padding-left:3px;"">No FAA Flight Activity Found.</td></tr>")
      End If

      htmlOut.Append("</table>")

    Catch ex As Exception

      aError = "Error in get_flight_activity_by_ac_top_function(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

    Finally

    End Try

    'return resulting html string
    out_htmlString = htmlOut.ToString
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Sub get_flight_activity_by_model_top_function(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByVal product_code_selection As String)

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder
    Dim toggleRowColor As Boolean = False

    Try
      '


      results_table = get_flight_activity_by_model(searchCriteria, product_code_selection)

      htmlOut.Append("<table id=""fractionsExpiringOuterTable"" width=""100%"" cellpadding=""0"" cellspacing=""0"" class=""module"">")
      htmlOut.Append("<tr><td valign=""top"" align=""center"" class=""header"">Top 25 Models Flight Activity (Last " & searchCriteria.ViewCriteriaTimeSpan & " Months, Based on Arrivals)</td></tr>")

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          htmlOut.Append("<tr><td valign=""top"" align=""left"">")

          htmlOut.Append("<table id=""fractionsExpiringInnerTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"">")

          htmlOut.Append("<tr><td colspan=""2"" class=""rightside"" valign=""top"">")
          '  htmlOut.Append("<div style=""height: 250px; overflow: auto;"">")

          htmlOut.Append("<table id=""fractionsExpiringDataTable"" width=""100%"" cellpadding=""4"" cellspacing=""0"">")
          htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""seperator"" width=""80%""><strong>Make/Model</strong></td>")
          htmlOut.Append("<td valign=""top"" align=""right"" class=""seperator"" width=""20%""><strong>#&nbsp;Flights</strong></td></tr>")



          For Each r As DataRow In results_table.Rows

            If Not toggleRowColor Then
              htmlOut.Append("<tr class=""alt_row"">")
              toggleRowColor = True
            Else
              htmlOut.Append("<tr bgcolor=""white"">")
              toggleRowColor = False
            End If

            htmlOut.Append("<td align=""left"" valign=""top"" class=""seperator"">")
            htmlOut.Append("" & r.Item("amod_make_name").ToString & " " & r.Item("amod_model_name").ToString)
            htmlOut.Append("</td>")
            htmlOut.Append("<td align=""right"" valign=""top"" class=""seperator"" width=""20%"" style=""padding-right:15px;"">" + r.Item("tflights").ToString + "</td></tr>")

          Next

          'htmlOut.Append("</table></div></td></tr></table></td></tr>")
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



    Public Sub GET_USER_AIRPORTS_top_function(ByVal user_airports_string As String, ByRef out_htmlString As String, ByVal UpdateProgressPanel As UpdateProgress, ByVal from_location As String, Optional ByVal selected_value As String = "3", Optional ByVal faa_Date As String = "", Optional ByVal usejQueryTable As Boolean = False, Optional linkNewWindow As Boolean = False)

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False
        Dim old_date As String = ""
        Dim mid_date As String = ""

        Dim last_period_diff As Double = 0.0
        Dim last_period_percentage As Double = 0.0

        Try

            ' results_table = GET_USER_AIRPORTS(user_airports_string, selected_value, faa_Date, usejQueryTable)

            results_table = GET_USER_AIRPORTS_NEW_RANGES(user_airports_string, selected_value, faa_Date, usejQueryTable)

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    If Not usejQueryTable Then

                        htmlOut.Append("<tr><td valign=""top"" align=""left"">")

                        htmlOut.Append("<table id=""fractionsExpiringInnerTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"" class=""data_aircraft_grid darker_blue_border"">")

                        htmlOut.Append("<tr><td colspan=""2"" class=""rightside"" valign=""top"">")

                        htmlOut.Append("<table id=""fractionsExpiringDataTable"" width=""100%"" cellpadding=""4"" cellspacing=""0"">")

                        If Not String.IsNullOrEmpty(faa_Date) Then

                            If selected_value.Contains("365") Or String.IsNullOrEmpty(selected_value) Then
                                old_date = FormatDateTime(DateAdd(DateInterval.Day, -730, CDate(faa_Date)), DateFormat.ShortDate)
                                old_date = FormatDateTime(DateAdd(DateInterval.Day, 1, CDate(old_date)), DateFormat.ShortDate)
                                mid_date = FormatDateTime(DateAdd(DateInterval.Year, -1, CDate(faa_Date)), DateFormat.ShortDate)
                            ElseIf selected_value.Contains("90") Then
                                old_date = FormatDateTime(DateAdd(DateInterval.Day, -180, CDate(faa_Date)), DateFormat.ShortDate)
                                old_date = FormatDateTime(DateAdd(DateInterval.Day, 1, CDate(old_date)), DateFormat.ShortDate)
                                mid_date = FormatDateTime(DateAdd(DateInterval.Day, -90, CDate(faa_Date)), DateFormat.ShortDate)
                            Else 'its year to date 
                                old_date = Year(CDate(faa_Date))
                                old_date = "01/01/" + old_date ' first of the year this year
                                old_date = FormatDateTime(DateAdd(DateInterval.Year, -1, CDate(old_date)), DateFormat.ShortDate)
                                mid_date = FormatDateTime(DateAdd(DateInterval.Year, -1, CDate(faa_Date)), DateFormat.ShortDate)
                            End If

                            htmlOut.Append("<tr><th valign=""middle"" align=""center"" class=""header_row cell_border_top"" width=""70%""><strong>Airports</strong></th>")
                            If Trim(LCase(from_location)) = "home" Then
                            Else
                                htmlOut.Append("<th valign=""middle"" align=""center"" class=""header_row cell_border_top"" width=""13%"" nowrap=""nowrap""><strong>#Arrivals</strong><br/><strong>" + old_date + "  -" + mid_date + "</strong></th>")
                            End If
                            If selected_value.Contains("365") Or String.IsNullOrEmpty(selected_value) Then
                                mid_date = FormatDateTime(DateAdd(DateInterval.Day, 1, CDate(mid_date)), DateFormat.ShortDate)
                            ElseIf selected_value.Contains("90") Then
                                mid_date = FormatDateTime(DateAdd(DateInterval.Day, 1, CDate(mid_date)), DateFormat.ShortDate)
                            Else
                                mid_date = FormatDateTime(DateAdd(DateInterval.Year, 1, CDate(old_date)), DateFormat.ShortDate)
                            End If


                            faa_Date = FormatDateTime(CDate(faa_Date), DateFormat.ShortDate)

                            htmlOut.Append("<th valign=""middle"" align=""center"" class=""header_row cell_border_top"" width=""13%"" nowrap=""nowrap""><strong>#Arrivals</strong><br/><strong>" + mid_date + " - " + faa_Date + "</strong></th>")
                            If Trim(LCase(from_location)) = "home" Then
                            Else
                                htmlOut.Append("<th valign=""middle"" align=""center"" class=""header_row cell_border_top"" width=""2%""><strong>Net Change<br/><em>+/-</em></strong></th>")
                            End If

                            htmlOut.Append("</tr>")

                        Else

                            htmlOut.Append("<tr><th valign=""middle"" align=""center"" class=""header_row cell_border_top"" width=""74%""><strong>Airports</strong></th>")
                            If Trim(LCase(from_location)) = "home" Then
                            Else
                                htmlOut.Append("<th valign=""middle"" align=""center"" class=""header_row cell_border_top"" width=""13%"" nowrap='nowrap'><strong>Previous Year</strong></th>")
                            End If
                            htmlOut.Append("<th valign=""middle"" align=""center"" class=""header_row cell_border_top"" width=""13%"" nowrap='nowrap'><strong>Current Year</strong></th>")
                            If Trim(LCase(from_location)) = "home" Then
                            Else
                                htmlOut.Append("<th valign=""middle"" align=""center"" class=""header_row cell_border_top"" width=""2%""><strong>Net Change<br/><em>+/-</em></strong></th>")
                            End If
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


                            htmlOut.Append("<td class=""text_align_left"">")

                            If Trim(LCase(from_location)) = "view" Or Trim(LCase(from_location)) = "home" Or HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME").ToString.ToUpper.Trim.Contains("TESTJETNETEVOLUTION.COM") Then
                                If Not IsNothing(UpdateProgressPanel) Then
                                    htmlOut.Append("<a " & IIf(linkNewWindow, "target=""_blank""", "") & " href='view_template.aspx?ViewID=24&ViewName=Airport FBO View&aport_id=" + r.Item("APortId").ToString + "'  onclick=""document.body.style.cursor='wait';$get('" + UpdateProgressPanel.ClientID + "').style.display = 'block';"">")
                                Else
                                    htmlOut.Append("<a " & IIf(linkNewWindow, "target=""_blank""", "") & " href='view_template.aspx?ViewID=24&ViewName=Airport FBO View&aport_id=" + r.Item("APortId").ToString + "'  onclick=""document.body.style.cursor='wait';$get('').style.display = 'block';"">")
                                End If
                            End If


                            If Not IsDBNull(r.Item("APortName")) Then
                                htmlOut.Append("<b>" + r.Item("APortName").ToString + "</b>")
                            End If

                            If Trim(LCase(from_location)) = "view" Or Trim(LCase(from_location)) = "home" Or HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME").ToString.ToUpper.Trim.Contains("TESTJETNETEVOLUTION.COM") Then
                                htmlOut.Append("</a>")
                            End If

                            If Not IsDBNull(r.Item("IATACode")) And Not IsDBNull(r.Item("ICAOCode")) Then
                                If Trim(LCase(from_location)) = "view" Or Trim(LCase(from_location)) = "home" Or HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME").ToString.ToUpper.Trim.Contains("TESTJETNETEVOLUTION.COM") Then
                                    htmlOut.Append("<br/> ")
                                Else
                                    htmlOut.Append(", ")
                                End If
                            End If

                            If Not IsDBNull(r.Item("IATACode")) Then
                                htmlOut.Append("" + r.Item("IATACode").ToString + "")
                            End If

                            If Not IsDBNull(r.Item("ICAOCode")) Then
                                If Not String.IsNullOrEmpty(r.Item("ICAOCode").ToString.Trim) Then

                                    If Not IsDBNull(r.Item("IATACode")) Then
                                        If Not String.IsNullOrEmpty(r.Item("IATACode").ToString.Trim) Then
                                            htmlOut.Append("/")
                                        End If
                                    End If

                                    htmlOut.Append("" + r.Item("ICAOCode").ToString + "")

                                End If
                            End If

                            If Not IsDBNull(r.Item("APortCountry")) Or Not IsDBNull(r.Item("APortCity")) Or Not IsDBNull(r.Item("APortState")) Then
                                If Not String.IsNullOrEmpty(r.Item("APortCountry").ToString.Trim) Or Not String.IsNullOrEmpty(r.Item("APortCity").ToString.Trim) Or Not String.IsNullOrEmpty(r.Item("APortState").ToString.Trim) Then
                                    htmlOut.Append(" ( ")
                                End If
                            End If

                            If Not IsDBNull(r.Item("APortCity")) Then
                                htmlOut.Append(r.Item("APortCity").ToString)
                            End If

                            If Not IsDBNull(r.Item("APortState")) Then
                                If Not String.IsNullOrEmpty(r.Item("APortState").ToString.Trim) Then
                                    htmlOut.Append(", " + r.Item("APortState").ToString)
                                End If
                            End If

                            If Not IsDBNull(r.Item("APortCountry")) Then
                                If Not String.IsNullOrEmpty(r.Item("APortCountry").ToString.Trim) Then
                                    htmlOut.Append(" " + r.Item("APortCountry").ToString.Trim.Replace("United States", "U.S.").Trim)
                                End If
                            End If

                            If Not IsDBNull(r.Item("APortCountry")) Or Not IsDBNull(r.Item("APortCity")) Or Not IsDBNull(r.Item("APortState")) Then
                                If Not String.IsNullOrEmpty(r.Item("APortCountry").ToString.Trim) Or Not String.IsNullOrEmpty(r.Item("APortCity").ToString.Trim) Or Not String.IsNullOrEmpty(r.Item("APortState").ToString.Trim) Then
                                    htmlOut.Append(" )")
                                End If
                            End If

                            htmlOut.Append("</td>")

                            If Trim(LCase(from_location)) = "home" Then
                            Else
                                If Not IsDBNull(r.Item("previousperiod")) Then
                                    htmlOut.Append("<td class=""text_align_right"" width=""13%"" style=""padding-right:3px;"">" + FormatNumber(r.Item("previousperiod").ToString, 0) + "</td>")
                                Else
                                    htmlOut.Append("<td class=""text_align_right"" width=""13%"" style=""padding-right:3px;"">&nbsp;</td>")
                                End If
                            End If


                            If Not IsDBNull(r.Item("currentperiod")) Then
                                htmlOut.Append("<td class=""text_align_right"" width=""13%"" style=""padding-right:3px;"">" + FormatNumber(r.Item("currentperiod").ToString, 0) + "</td>")
                            Else
                                htmlOut.Append("<td class=""text_align_right"" width=""13%"" style=""padding-right:3px;"">&nbsp;</td>")
                            End If

                            If Trim(LCase(from_location)) = "home" Then
                            Else

                                last_period_diff = 0.0
                                last_period_percentage = 0.0

                                If Not IsDBNull(r.Item("previousperiod")) And Not IsDBNull(r.Item("currentperiod")) Then

                                    last_period_diff = CInt(CDbl(r.Item("currentperiod").ToString) - CDbl(r.Item("previousperiod").ToString))
                                    last_period_percentage = last_period_diff / CDbl(r.Item("previousperiod").ToString)

                                    htmlOut.Append("<td class=""text_align_right"" width=""13%"" style=""padding-right:3px;"" nowrap=""nowrap"">")

                                    If last_period_diff = 0 Then
                                        htmlOut.Append("<img src=""images/gain_loss_none.jpg"" alt=""No Change"" class=""image_padding"" />")
                                        htmlOut.Append(last_period_diff.ToString + " (" + FormatPercent(last_period_percentage, 2, TriState.False, TriState.False, TriState.True).ToString + ")")
                                    ElseIf last_period_diff < 0 Then
                                        htmlOut.Append("<img src=""images/gain_loss_down.jpg"" alt=""Net Loss""/>")
                                        htmlOut.Append(last_period_diff.ToString + " (" + FormatPercent(last_period_percentage, 2, TriState.False, TriState.False, TriState.True).ToString + ")")
                                    Else
                                        htmlOut.Append("<img src=""images/gain_loss_up.jpg"" alt=""Net Gain""/>")
                                        htmlOut.Append(last_period_diff.ToString + " (" + FormatPercent(last_period_percentage, 2, TriState.False, TriState.False, TriState.True).ToString + ")")
                                    End If

                                    htmlOut.Append("</td>")

                                Else
                                    htmlOut.Append("<td class=""text_align_right"" width=""13%"" style=""padding-right:3px;"" nowrap=""nowrap"">N/A</td>")
                                End If
                            End If


                            htmlOut.Append("</tr>")

                        Next

                        htmlOut.Append("</table></td></tr></table></td></tr>")

                    Else

                        htmlOut.Append("<table id=""aPortDataTable"" cellpadding=""0"" cellspacing=""0"" border=""0"" width=""100%"">")
                        htmlOut.Append("<thead><tr>")
                        htmlOut.Append("<th width=""25""><span class=""help_cursor"" title=""Used to select and remove airports from the list"">SEL</span></th>")

                        htmlOut.Append("<th></th>")

                        '  If Not String.IsNullOrEmpty(faa_Date) Then

                        'If selected_value.Contains("365") Or String.IsNullOrEmpty(selected_value) Then
                        '  old_date = FormatDateTime(DateAdd(DateInterval.Day, -730, CDate(faa_Date)), DateFormat.ShortDate)
                        '  old_date = FormatDateTime(DateAdd(DateInterval.Day, 1, CDate(old_date)), DateFormat.ShortDate)
                        '  mid_date = FormatDateTime(DateAdd(DateInterval.Year, -1, CDate(faa_Date)), DateFormat.ShortDate)
                        'ElseIf selected_value.Contains("90") Then
                        '  old_date = FormatDateTime(DateAdd(DateInterval.Day, -180, CDate(faa_Date)), DateFormat.ShortDate)
                        '  old_date = FormatDateTime(DateAdd(DateInterval.Day, 1, CDate(old_date)), DateFormat.ShortDate)
                        '  mid_date = FormatDateTime(DateAdd(DateInterval.Day, -90, CDate(faa_Date)), DateFormat.ShortDate)
                        'Else 'its year to date 
                        '  old_date = Year(CDate(faa_Date))
                        '  old_date = "01/01/" + old_date ' first of the year this year
                        '  old_date = FormatDateTime(DateAdd(DateInterval.Year, -1, CDate(old_date)), DateFormat.ShortDate)
                        '  mid_date = FormatDateTime(DateAdd(DateInterval.Year, -1, CDate(faa_Date)), DateFormat.ShortDate)
                        'End If
                        If IsNumeric(Trim(selected_value)) = True Then
                            If CInt(Trim(selected_value)) = 1 Then
                                mid_date = "Last Month"
                            Else
                                mid_date = "Last " & Trim(selected_value) & " Months"
                            End If


                            ' mid_date = DateAdd(DateInterval.Month, -CInt(selected_value), Date.Now())

                        ElseIf Trim(selected_value) = "YTD" Then
                            '  mid_date = "1/1/" & Year(Date.Now())
                            mid_date = "Year to Date"
                        ElseIf Trim(selected_value) = "MTD" Then
                            ' mid_date = Month(Date.Now) & "/1/" & Year(Date.Now())
                            mid_date = "Month to Date"
                        End If

                        htmlOut.Append("<th>AIRPORT</th>")
                        htmlOut.Append("<th data-priority=""1""># ARRIVALS<br/>" & mid_date & "</th>")

                        'If selected_value.Contains("365") Or String.IsNullOrEmpty(selected_value) Then
                        '  mid_date = FormatDateTime(DateAdd(DateInterval.Day, 1, CDate(mid_date)), DateFormat.ShortDate)
                        'ElseIf selected_value.Contains("90") Then
                        '  mid_date = FormatDateTime(DateAdd(DateInterval.Day, 1, CDate(mid_date)), DateFormat.ShortDate)
                        'Else
                        '  mid_date = FormatDateTime(DateAdd(DateInterval.Year, 1, CDate(old_date)), DateFormat.ShortDate)
                        'End If

                        'faa_Date = FormatDateTime(CDate(faa_Date), DateFormat.ShortDate)



                        '  Else

                        '  htmlOut.Append("<th>AIRPORT</th>")
                        'htmlOut.Append("<th># ARRIVALS<br/>PREVIOUS YEAR</th>")
                        '   htmlOut.Append("<th data-priority=""1""># ARRIVALS<br/>CURRENT YEAR</th>")
                        'htmlOut.Append("<th>NET CHANGE<br/><em>+/-</em></th>")

                        ' End If

                        htmlOut.Append("</tr></thead><tbody>")

                        For Each r As DataRow In results_table.Rows

                            htmlOut.Append("<tr>")
                            htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">&nbsp;</td>")
                            htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">" + r.Item("APortId").ToString + "</td>")

                            htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

                            If Not IsDBNull(r.Item("APortName")) Then

                                If Not String.IsNullOrEmpty(r.Item("APortName").ToString.Trim) Then

                                    htmlOut.Append("<a " & IIf(linkNewWindow, "target=""_blank""", "") & " href=""view_template.aspx?ViewID=28&ViewName=Flight%20Activity%20(Operator/Airport)&aport_id=" + r.Item("APortId").ToString + """ onclick=""document.body.style.cursor='wait';$get('').style.display = 'block';"">")

                                    htmlOut.Append(r.Item("APortName").ToString.Trim)

                                    If Not IsDBNull(r.Item("IATACode")) Or Not IsDBNull(r.Item("ICAOCode")) Then
                                        htmlOut.Append(", ")
                                    End If

                                    If Not IsDBNull(r.Item("IATACode")) Then
                                        If Not String.IsNullOrEmpty(r.Item("IATACode").ToString.Trim) Then
                                            htmlOut.Append(r.Item("IATACode").ToString.Trim)
                                        End If
                                    End If

                                    If Not IsDBNull(r.Item("ICAOCode")) Then
                                        If Not String.IsNullOrEmpty(r.Item("ICAOCode").ToString.Trim) Then

                                            If Not IsDBNull(r.Item("IATACode")) Then
                                                If Not String.IsNullOrEmpty(r.Item("IATACode").ToString.Trim) Then
                                                    htmlOut.Append(" / ")
                                                End If
                                            End If

                                            htmlOut.Append(r.Item("ICAOCode").ToString.Trim)

                                        End If
                                    End If
                                    htmlOut.Append("</a>")
                                Else
                                    htmlOut.Append("<em>Unknown</em>")
                                End If
                            Else
                                htmlOut.Append("<em>Unknown</em>")
                            End If

                            If Not IsDBNull(r.Item("APortCountry")) Or Not IsDBNull(r.Item("APortCity")) Or Not IsDBNull(r.Item("APortState")) Then
                                If Not String.IsNullOrEmpty(r.Item("APortCountry").ToString.Trim) Or Not String.IsNullOrEmpty(r.Item("APortCity").ToString.Trim) Or Not String.IsNullOrEmpty(r.Item("APortState").ToString.Trim) Then

                                    If Not IsDBNull(r.Item("APortName")) Then
                                        If Not String.IsNullOrEmpty(r.Item("APortName").ToString.Trim) Then
                                            htmlOut.Append("<br /> ")
                                        End If
                                    End If

                                    htmlOut.Append("<em>")

                                    If Not IsDBNull(r.Item("APortCity")) Then
                                        If Not String.IsNullOrEmpty(r.Item("APortCity").ToString.Trim) Then
                                            htmlOut.Append(r.Item("APortCity").ToString)
                                        End If
                                    End If

                                    If Not IsDBNull(r.Item("APortState")) Then
                                        If Not String.IsNullOrEmpty(r.Item("APortState").ToString.Trim) Then

                                            If Not IsDBNull(r.Item("APortCity")) Then
                                                If Not String.IsNullOrEmpty(r.Item("APortCity").ToString.Trim) Then
                                                    htmlOut.Append(", ")
                                                End If
                                            End If

                                            htmlOut.Append(r.Item("APortState").ToString)

                                        End If
                                    End If

                                    If Not IsDBNull(r.Item("APortCountry")) Then
                                        If Not String.IsNullOrEmpty(r.Item("APortCountry").ToString.Trim) Then
                                            htmlOut.Append(" " + r.Item("APortCountry").ToString.Trim.Replace("United States", "U.S.").Trim)
                                        End If
                                    End If

                                    htmlOut.Append("</em>")

                                End If
                            End If

                            htmlOut.Append("</td>")

                            'If Not IsDBNull(r.Item("previousperiod")) Then
                            '  htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">" + FormatNumber(r.Item("previousperiod").ToString, 0) + "</td>")
                            'Else
                            '  htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">&nbsp;</td>")
                            'End If

                            If Not IsDBNull(r.Item("currentperiod")) Then
                                htmlOut.Append("<td align=""right"" valign=""middle"" nowrap=""nowrap"" data-sort=""" + FormatNumber(r.Item("currentperiod").ToString, 0) + """>" + FormatNumber(r.Item("currentperiod").ToString, 0) + "&nbsp;</td>")
                            Else
                                htmlOut.Append("<td align=""right"" valign=""middle"" nowrap=""nowrap"" data-sort=""" + FormatNumber(-1, 0) + """>&nbsp;</td>")
                            End If


                            'last_period_diff = 0.0
                            'last_period_percentage = 0.0

                            'If Not IsDBNull(r.Item("previousperiod")) And Not IsDBNull(r.Item("currentperiod")) Then

                            '  last_period_diff = CInt(CDbl(r.Item("currentperiod").ToString) - CDbl(r.Item("previousperiod").ToString))
                            '  last_period_percentage = last_period_diff / CDbl(r.Item("previousperiod").ToString)

                            '  htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

                            '  If last_period_diff = 0 Then
                            '    htmlOut.Append("<img src=""images/gain_loss_none.jpg"" alt=""No Change"" class=""image_padding"" /> ")
                            '    htmlOut.Append(last_period_diff.ToString + " (" + FormatPercent(last_period_percentage, 2, TriState.False, TriState.False, TriState.True).ToString + ")")
                            '  ElseIf last_period_diff < 0 Then
                            '    htmlOut.Append("<img src=""images/gain_loss_down.jpg"" alt=""Net Loss""/> ")
                            '    htmlOut.Append(last_period_diff.ToString + " (" + FormatPercent(last_period_percentage, 2, TriState.False, TriState.False, TriState.True).ToString + ")")
                            '  Else
                            '    htmlOut.Append("<img src=""images/gain_loss_up.jpg"" alt=""Net Gain""/> ")
                            '    htmlOut.Append(last_period_diff.ToString + " (" + FormatPercent(last_period_percentage, 2, TriState.False, TriState.False, TriState.True).ToString + ")")
                            '  End If

                            '  htmlOut.Append("</td>")

                            'Else
                            '  htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">N/A</td>")
                            'End If


                            htmlOut.Append("</tr>")

                        Next

                        htmlOut.Append("</tbody></table>")
                        htmlOut.Append("<div id=""aPortLabel"" class="""" style=""padding:2px;""><strong>" + results_table.Rows.Count.ToString + " Airport(s)</strong></div>")
                        htmlOut.Append("<div id=""aPortInnerTable"" align=""left"" valign=""middle"" style=""max-height:670px; overflow: auto;""></div>")

                    End If ' not usejQueryTable

                Else
                    If Not usejQueryTable Then
                        htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""seperator"" style=""padding-left:3px;"">You have Not selected any Airports</td></tr>")
                    End If
                End If
            Else
                If Not usejQueryTable Then
                    htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""seperator"" style=""padding-left:3px;"">You have Not selected any Airports</td></tr>")
                End If
            End If

            If Not usejQueryTable Then
                htmlOut.Append("</table>")
            End If


        Catch ex As Exception

            aError = "Error in GET_USER_AIRPORTS_top_function(ByVal user_airports_string As String, ByRef out_htmlString As String, ByVal UpdateProgressPanel As UpdateProgress, ByVal from_location As String, Optional ByVal selected_value As String = '90', Optional ByVal faa_Date As String = "", Optional ByVal usejQueryTable As Boolean = False) " + ex.Message

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
              htmlOut.Append("<td align='right'>Export:&nbsp;&nbsp;<a href='view_template.aspx?ViewID=24&ViewName=Airport FBO View&aport_id=" & Airport_ID_OVERALL & "&activetab=4&export=OP' target='_blank'><font color='white'><u>Full List</u></font></a>&nbsp;&nbsp;")
            Else
              htmlOut.Append("&nbsp;Owners Flight Activity at " & aport_name & " (" & start_date & " - " & end_date & ") - " & results_table.Rows.Count & " Owners") ' , " & results_table.Rows.Count & " Aircraft
              If use_ac = True Then
                htmlOut.Append("/Aircraft")
              End If
              htmlOut.Append("</td>")
              htmlOut.Append("<td align='right'>Export:&nbsp;&nbsp;<a href='view_template.aspx?ViewID=24&ViewName=Airport FBO View&aport_id=" & Airport_ID_OVERALL & "&activetab=3&export=OW' target='_blank'><font color='white'><u>Full List</u></font></a>&nbsp;&nbsp;")
            End If

            If HttpContext.Current.Session.Item("localUser").crmDemoUserFlag = True Then  ' ADDED IN MSW - 3/26/20
            Else
              htmlOut.Append("&nbsp;&nbsp;&nbsp;&nbsp;<a class=""text_underline cursor"" title='View/Export' ' onclick=""javascript:load('WebSource.aspx?viewType=dynamic&display=table&PageTitle=Owners','','scrollbars=yes,menubar=no,height=700,width=1250,resizable=yes,toolbar=no,location=no,status=no');"" >VIEW IN GRID</a>")
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
                htmlOut.Append("<a class='underline' onclick=""javascript:load('DisplayAircraftDetail.aspx?acid=" + r.Item("ACId").ToString + "&jid=0','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');' title='Display Aircraft Details'>")
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

  Public Sub get_most_recent_flight_activity_top_function(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByVal aport_name As String, ByVal run_export As String, ByRef title_text As String, ByRef title_text2 As String, ByVal selected_value As String, ByVal recent_flight_months As Integer, ByVal start_date As String, ByVal end_date As String, ByVal product_code_selection As String, ByVal product_link As String)

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder
    Dim toggleRowColor As Boolean = False
    Dim same_country As Boolean = False

    Try

      results_table = get_most_recent_flight_activity(searchCriteria, run_export, selected_value, recent_flight_months, start_date, end_date, product_code_selection)

      If Trim(run_export) <> "" Then
        crmViewDataLayer.ExportTableData(results_table)
      Else
        htmlOut.Append("<table id=""fractionsExpiringOuterTable"" width=""100%"" cellpadding=""0"" cellspacing=""0"" class=""module"">")
        'htmlOut.Append("<tr><td valign=""top"" align=""center"" class=""header"">Recent Flight Activity at " & aport_name & " (" & results_table.Rows.Count & " in Last " & searchCriteria.ViewCriteriaTimeSpan & " Months)</td></tr>")

        title_text = " at " & aport_name & " "

        title_text2 = " - " & results_table.Rows.Count & " Flights Displayed - Display <a href='view_template.aspx?ViewID=24&ViewName=Airport Activity View&activetab=3&start_date=" & start_date & "&end_date=" & end_date & product_link & "'>Owners</a>, <a href='view_template.aspx?ViewID=24&ViewName=Airport Activity View&activetab=4&start_date=" & start_date & "&end_date=" & end_date & product_link & "'>Operators</a> for the Flights Below"

        If Not IsNothing(results_table) Then

          If results_table.Rows.Count > 0 Then

            htmlOut.Append("<tr><td valign=""top"" align=""left"">")

            htmlOut.Append("<table id=""fractionsExpiringInnerTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"">")

            htmlOut.Append("<tr><td colspan=""2"" class=""rightside"" valign=""top"">")
            htmlOut.Append("<div style=""height: 250px; overflow: auto;"">")

            htmlOut.Append("<table id=""fractionsExpiringDataTable"" width=""100%"" cellpadding=""4"" cellspacing=""0"">")
            htmlOut.Append("<tr>")
            htmlOut.Append("<td valign=""top"" align=""left"" class=""seperator""><strong>Aircraft</strong></td>")


            If Trim(selected_value) = "" Or Trim(selected_value) = "A" Then
              ' htmlOut.Append("<td valign=""top"" align=""left"" class=""seperator""><strong>Origin Airport</strong></td>")
              ' htmlOut.Append("<td valign=""top"" align=""left"" class=""seperator""><strong>Destin Airport</strong></td>")
              htmlOut.Append("<td valign=""top"" align=""left"" class=""seperator""><strong>Date</strong></td>")
              htmlOut.Append("<td valign=""top"" align=""left"" class=""seperator""><strong>Origin Airport</strong></td>")
            ElseIf Trim(selected_value) = "D" Then
              htmlOut.Append("<td valign=""top"" align=""left"" class=""seperator""><strong>Date</strong></td>")
              htmlOut.Append("<td valign=""top"" align=""left"" class=""seperator""><strong>Destination Airport</strong></td>")
            End If


            htmlOut.Append("<td valign=""top"" align=""left"" class=""seperator"" nowrap='nowrap'><strong>Flight Time/Dist(sm)</strong></td>")
            htmlOut.Append("</tr>")


            For Each r As DataRow In results_table.Rows

              If Not toggleRowColor Then
                htmlOut.Append("<tr class=""alt_row"">")
                toggleRowColor = True
              Else
                htmlOut.Append("<tr bgcolor=""white"">")
                toggleRowColor = False
              End If


              '       ac_id As ACId, amod_make_name As Make, amod_model_name As Model, ")
              '      sQuery.Append(" ac_ser_no_full As SerNbr, ac_reg_no As RegNbr,  ")
              '      sQuery.Append(" ffd_date As FlightDate, ffd_origin_aport As OriginAPort,ffd_origin_aport_id, ")
              '      sQuery.Append(" aport_name, aport_country, aport_city, aport_state, ")
              'sQuery.Append(" ffd_flight_time As FlightTime, ffd_distance As Distance


              htmlOut.Append("<td align=""left"" valign=""top"" class=""seperator"">")
              htmlOut.Append("" & r.Item("Make").ToString & " ")
              htmlOut.Append("" & r.Item("Model").ToString & "")
              htmlOut.Append(" S#: ")
              htmlOut.Append("<a class='underline' onclick=""javascript:load('DisplayAircraftDetail.aspx?acid=" + r.Item("ACId").ToString + "&jid=0','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');"" title='Display Aircraft Details'>")
              htmlOut.Append("" & r.Item("SerNbr").ToString & "</a> ")
              htmlOut.Append("R#: " & r.Item("RegNbr").ToString & " ")
              htmlOut.Append("</td>")

              htmlOut.Append("<td align=""left"" valign=""top"" class=""seperator"" nowrap='nowrap'>")
              ' If Not IsDBNull(r.Item("FlightDate")) Then
              '   htmlOut.Append("" & Month(r.Item("FlightDate").ToString) & "/" & Day(r.Item("FlightDate").ToString) & "/" & Right(Year(r.Item("FlightDate").ToString), 2) & "")
              ' End If



              same_country = False

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
                If Not IsDBNull(r.Item("ffd_origin_date")) Then
                  htmlOut.Append(r.Item("ffd_origin_date"))
                End If
                'ffd_dest_date, , 

                htmlOut.Append("</td>")
                htmlOut.Append("<td align=""left"" valign=""top"" class=""seperator"">")
                htmlOut.Append("<a href='view_template.aspx?ViewID=24&ViewName=Airport FBO View&aport_id=" & r.Item("aport_id").ToString & "'>")
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
              ElseIf Trim(selected_value) = "D" Then

                If Not IsDBNull(r.Item("ffd_dest_date")) Then
                  htmlOut.Append(r.Item("ffd_dest_date"))
                End If
                htmlOut.Append("</td>")

                htmlOut.Append("<td align=""left"" valign=""top"" class=""seperator"">")
                htmlOut.Append("<a href='view_template.aspx?ViewID=24&ViewName=Airport FBO View&aport_id=" & r.Item("aport_id2").ToString & "'>")
                htmlOut.Append("" & r.Item("DestinAPort").ToString & "</a> ")

                If Not IsDBNull(r.Item("aport_name2")) Then
                  htmlOut.Append(" - " & r.Item("aport_name2").ToString & " (")
                End If

                If Not IsDBNull(r.Item("aport_country2")) Then
                  htmlOut.Append("" & r.Item("aport_country2").ToString & " ")
                End If

                If Not IsDBNull(r.Item("aport_city2")) Then
                  htmlOut.Append("" & r.Item("aport_city2").ToString & " ")
                End If

                If Not IsDBNull(r.Item("aport_state2")) Then
                  htmlOut.Append(", " & r.Item("aport_state2").ToString)
                End If

                If Not IsDBNull(r.Item("aport_name2")) Or Not IsDBNull(r.Item("aport_country2")) Or Not IsDBNull(r.Item("aport_city2")) Or Not IsDBNull(r.Item("aport_state2")) Then
                  htmlOut.Append(")")
                End If

                htmlOut.Append("&nbsp;</td>")
              End If






              htmlOut.Append("<td align=""right"" valign=""top"" class=""seperator"">")
              If Not IsDBNull(r.Item("FlightTime").ToString) Then
                htmlOut.Append("" & r.Item("FlightTime").ToString & " ")
              End If
              htmlOut.Append("&nbsp;/&nbsp;")

              If Not IsDBNull(r.Item("Distance").ToString) Then
                htmlOut.Append("" & r.Item("Distance").ToString & " ")
              End If
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

  Public Sub get_companies_from_airport_top_function(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByVal city_name As String, ByVal company_type As String, ByVal run_export As String, ByVal aport_id As Long, ByVal use_ac As Boolean)

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder
    Dim toggleRowColor As Boolean = False
    Dim last_comp_id As Long = 0
    Dim comp_count As Long = 0


    Try


      results_table = get_companies_from_airport(searchCriteria, company_type, run_export, aport_id, use_ac)

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

        If HttpContext.Current.Session.Item("localUser").crmDemoUserFlag = True Then  ' ADDED IN MSW - 3/26/20
          htmlOut.Append("</td></tr>")
        ElseIf Trim(company_type) = "Owner" Then
          htmlOut.Append("<table id=""fractionsExpiringOuterTable"" width=""100%"" cellpadding=""0"" cellspacing=""0"" class=""module"">")
          htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""header"">")
          htmlOut.Append("<table cellspacing='0' cellpadding='0' width='100%'><tr><td align='left' width='80%'>")
          htmlOut.Append("&nbsp;Companies Owning Aircraft at " & city_name & " - " & comp_count & " Owners</td>")    ', " & results_table.Rows.Count & " Aircraft
          htmlOut.Append("<td align='right'>Export:&nbsp;&nbsp;<a href='view_template.aspx?ViewID=24&ViewName=Airport FBO View&aport_id=" & Airport_ID_OVERALL & "&activetab=3&export=A' target='_blank'><font color='white'><u>Full List</u></font></a>&nbsp;&nbsp;")
          htmlOut.Append("<a href='view_template.aspx?ViewID=24&ViewName=Airport FBO View&aport_id=" & Airport_ID_OVERALL & "&activetab=3&export=C' target='_blank'><font color='white'><u>Owners</u></font></a>")
          If HttpContext.Current.Session.Item("localUser").crmDemoUserFlag = True Then  ' ADDED IN MSW - 3/26/20
            htmlOut.Append("</td><td nowrap='nowrap'>&nbsp;")
          Else
            htmlOut.Append("</td><td nowrap='nowrap'>&nbsp;&nbsp;&nbsp;&nbsp;<a class=""text_underline cursor"" title='View/Export' ' onclick=""javascript:load('WebSource.aspx?viewType=dynamic&display=table&PageTitle=Owners','','scrollbars=yes,menubar=no,height=700,width=1250,resizable=yes,toolbar=no,location=no,status=no');"" ><font color='white'>VIEW IN GRID</font></a>")
          End If
          htmlOut.Append("&nbsp;</td></tr></table>")
          htmlOut.Append("</td></tr>")
        ElseIf Trim(company_type) = "Operator" Then
          htmlOut.Append("<table id=""fractionsExpiringOuterTable"" width=""100%"" cellpadding=""0"" cellspacing=""0"" class=""module"">")
          htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""header"">")
          htmlOut.Append("<table cellspacing='0' cellpadding='0' width='100%'><tr><td align='left' width='80%'>")
          htmlOut.Append("&nbsp;Companies Operating Aircraft at " & city_name & " - " & comp_count & " Operators</td>") ' , " & results_table.Rows.Count & " Aircraft
          htmlOut.Append("<td align='right'>Export:&nbsp;&nbsp;<a href='view_template.aspx?ViewID=24&ViewName=Airport FBO View&aport_id=" & Airport_ID_OVERALL & "&activetab=4&export=A' target='_blank'><font color='white'><u>Full List</u></font></a>&nbsp;&nbsp;")
          htmlOut.Append("<a href='view_template.aspx?ViewID=24&ViewName=Airport FBO View&aport_id=" & Airport_ID_OVERALL & "&activetab=4&export=C' target='_blank'><font color='white'><u>Operators</u></font></a>")
          If HttpContext.Current.Session.Item("localUser").crmDemoUserFlag = True Then  ' ADDED IN MSW - 3/26/20
            htmlOut.Append("</td><td nowrap='nowrap'>&nbsp;")
          Else
            htmlOut.Append("</td><td nowrap='nowrap'>&nbsp;&nbsp;&nbsp;&nbsp;<a class=""text_underline cursor"" title='View/Export' ' onclick=""javascript:load('WebSource.aspx?viewType=dynamic&display=table&PageTitle=Owners','','scrollbars=yes,menubar=no,height=700,width=1250,resizable=yes,toolbar=no,location=no,status=no');"" ><font color='white'>VIEW IN GRID</font></a>")
          End If
          htmlOut.Append("&nbsp;</td></tr></table>")
          htmlOut.Append("</td></tr>")
        Else

          htmlOut.Append("<table id=""fractionsExpiringOuterTable"" width=""100%"" cellpadding=""0"" cellspacing=""0"" class=""module"">")
          htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""header"">")
          htmlOut.Append("<table cellspacing='0' cellpadding='0' width='100%'><tr><td align='left' width='80%'>")
          htmlOut.Append("&nbsp;Companies Owning Aircraft at " & city_name & " - " & comp_count & " Owners</td>") ' , " & results_table.Rows.Count & " Aircraft
          htmlOut.Append("<td align='right'>Export:&nbsp;&nbsp;<a href='view_template.aspx?ViewID=24&ViewName=Airport FBO View&aport_id=" & Airport_ID_OVERALL & "&activetab=3&export=A' target='_blank'><font color='white'><u>Full List</u></font></a>&nbsp;&nbsp;")
          htmlOut.Append("<a href='view_template.aspx?ViewID=24&ViewName=Airport FBO View&aport_id=" & Airport_ID_OVERALL & "&activetab=3&export=C' target='_blank'><font color='white'><u>Owners</u></font></a>")

          htmlOut.Append("</td><td nowrap='nowrap'>&nbsp;&nbsp;&nbsp;&nbsp;<a class=""text_underline cursor"" title='View/Export' ' onclick=""javascript:load('WebSource.aspx?viewType=dynamic&display=table&PageTitle=Owners','','scrollbars=yes,menubar=no,height=700,width=1250,resizable=yes,toolbar=no,location=no,status=no');"" ><font color='white'>VIEW IN GRID</font></a>")

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


        If HttpContext.Current.Session.Item("localUser").crmDemoUserFlag = True Then  ' ADDED IN MSW - 3/26/20
        Else
          compare_view_sold_label &= "&nbsp;-&nbsp;&nbsp;<a href='view_template.aspx?ViewID=24&ViewName=Airport FBO View&aport_id=" & aport_id & "&activetab=2&export=A' target='_blank'>Export Companies</a>"
          'compare_view_sold_label &= "&nbsp;&nbsp;&nbsp;Change Miles To "
          'compare_view_sold_label &= "<a href='view_template.aspx?ViewID=24&ViewName=Airport FBO View&aport_id=" & aport_id & "&activetab=2&cdistance=25&bus_type=" & bus_type & "'>25</a>&nbsp;"
          ' compare_view_sold_label &= "<a href='view_template.aspx?ViewID=24&ViewName=Airport FBO View&aport_id=" & aport_id & "&activetab=2&cdistance=50&bus_type=" & bus_type & "'>50</a>&nbsp;"
          compare_view_sold_label &= "&nbsp;"

          compare_view_sold_label &= "&nbsp;&nbsp;&nbsp;&nbsp;<a class=""text_underline cursor"" title='View/Export' ' onclick=""javascript:load('WebSource.aspx?viewType=dynamic&display=table&PageTitle=Company Directory','','scrollbars=yes,menubar=no,height=700,width=1250,resizable=yes,toolbar=no,location=no,status=no');"" >VIEW IN GRID</a>"
        End If



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

  Public Sub get_bus_type_from_companies_from_airport_top_function(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef drop_down_list As DropDownList, ByVal selected_bus_type As String, ByVal company_distance As Integer, ByVal orig_lat As Double, ByVal orig_long As Double)

    Dim results_table As New DataTable

    Try
      drop_down_list.Items.Add(New System.Web.UI.WebControls.ListItem("All", ""))

      results_table = get_bus_type_from_companies_from_airport(searchCriteria, company_distance, orig_lat, orig_long)

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then
          For Each r As DataRow In results_table.Rows
            drop_down_list.Items.Add(New System.Web.UI.WebControls.ListItem(r.Item("cbus_name").ToString, r.Item("cbus_type").ToString))
          Next
        Else
        End If

        drop_down_list.SelectedValue = Trim(selected_bus_type)
      Else
      End If

    Catch ex As Exception

      aError = "Error in get_bus_type_from_companies_from_airport_top_function(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

    Finally

    End Try
    results_table = Nothing

  End Sub

#End Region

End Class
