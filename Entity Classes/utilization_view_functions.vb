Imports Microsoft.VisualBasic
Imports System.ComponentModel

' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/Entity Classes/utilization_view_functions.vb $
'$$Author: Amanda $
'$$Date: 7/01/20 4:19p $
'$$Modtime: 7/01/20 1:49p $
'$$Revision: 14 $ 
'$$Workfile: utilization_view_functions.vb $
' 
' ********************************************************************************

<System.Serializable()> Public Class utilization_view_functions

  Private aError As String
  Private clientConnectString As String
  Private adminConnectString As String

  Private starConnectString As String
  Private cloudConnectString As String
  Private serverConnectString As String
  Private value_label As String = "eValue"
  Private value_color As String = "#078fd7"
  Private grey_color As String = "#B7B7B7"


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

#Region "utilization_view_functions"

  Public Function get_flight_activity(ByRef searchCriteria As viewSelectionCriteriaClass, Optional ByVal use_faa_data As Boolean = False, Optional ByVal faa_date As String = "", Optional ByVal filterDate As Boolean = False) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim temp_faa_date As String = ""

    Try
      If use_faa_data = True Then

        sQuery.Append(" SELECT DISTINCT count(*) AS tflights, (sum(ffd_distance)/count(*)) AS avgdistance,")
        sQuery.Append(" sum(ffd_distance) AS tdistance, (sum(ffd_flight_time)/count(*)) AS avgflighttime,")
        sQuery.Append(" sum(ffd_flight_time) AS tflighttime")
        sQuery.Append(" FROM FAA_Flight_Data WITH(NOLOCK)")
        sQuery.Append(" INNER JOIN aircraft WITH(NOLOCK) ON (ac_id = ffd_ac_id AND ac_journ_id = 0)")
        sQuery.Append(" INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id")

        If Trim(faa_date) <> "" Then
          If filterDate Then
            temp_faa_date = DateAdd(DateInterval.Month, -searchCriteria.ViewCriteriaTimeSpan, CDate(faa_date))
          Else
            temp_faa_date = DateAdd(DateInterval.Day, -365, CDate(faa_date))
          End If

          sQuery.Append(" WHERE (ffd_date >= '" & Trim(temp_faa_date) & "') ")
        Else
          sQuery.Append(" WHERE (ffd_date >= (getdate() - 365))")
        End If

        sQuery.Append("  AND ffd_hide_flag = 'N' ")



        If searchCriteria.ViewCriteriaAFTTStart > 0 Then
          sQuery.Append(Constants.cAndClause + " ((ac_airframe_tot_hrs >= " & searchCriteria.ViewCriteriaAFTTStart & ") or (ac_airframe_tot_hrs IS NULL))")
        End If

        If searchCriteria.ViewCriteriaAFTTEnd > 0 Then
          sQuery.Append(Constants.cAndClause + " ((ac_airframe_tot_hrs <= " & searchCriteria.ViewCriteriaAFTTEnd & ") or (ac_airframe_tot_hrs IS NULL))")
        End If


        If searchCriteria.ViewCriteriaYearStart > 0 Then
          sQuery.Append(Constants.cAndClause + " ac_year >= " & searchCriteria.ViewCriteriaYearStart)
        End If


        If searchCriteria.ViewCriteriaYearEnd > 0 Then
          sQuery.Append(Constants.cAndClause + " ac_year <=  " & searchCriteria.ViewCriteriaYearEnd)
        End If



        If searchCriteria.ViewCriteriaAmodID > -1 Then
          sQuery.Append(Constants.cAndClause + "ac_amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
        End If

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
          sQuery.Append(Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
        End If

        sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, True))

      Else
        sQuery.Append("SELECT DISTINCT count(*) AS tflights, (sum(aractivity_distance)/count(*)) AS avgdistance, sum(aractivity_distance) AS tdistance,")
        sQuery.Append(" (sum(aractivity_flight_time)/count(*)) AS avgflighttime, sum(aractivity_flight_time) AS tflighttime")
        sQuery.Append(" FROM ARGUS_Activity_Data WITH(NOLOCK) INNER JOIN aircraft WITH(NOLOCK) ON (aractivity_reg_no = ac_reg_no_search) AND ac_journ_id = 0")
        sQuery.Append(" INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id")
        sQuery.Append(" WHERE aractivity_date_depart >= (getdate()-90)")

        If searchCriteria.ViewCriteriaAmodID > -1 Then
          sQuery.Append(Constants.cAndClause + "ac_amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
        End If

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
          sQuery.Append(Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
        End If



        If searchCriteria.ViewCriteriaAFTTStart > 0 Then
          sQuery.Append(Constants.cAndClause + " ((ac_airframe_tot_hrs >= " & searchCriteria.ViewCriteriaAFTTStart & ") or (ac_airframe_tot_hrs IS NULL))")
        End If

        If searchCriteria.ViewCriteriaAFTTEnd > 0 Then
          sQuery.Append(Constants.cAndClause + " ((ac_airframe_tot_hrs <= " & searchCriteria.ViewCriteriaAFTTEnd & ") or (ac_airframe_tot_hrs IS NULL))")
        End If


        If searchCriteria.ViewCriteriaYearStart > 0 Then
          sQuery.Append(Constants.cAndClause + " ac_year >= " & searchCriteria.ViewCriteriaYearStart)
        End If


        If searchCriteria.ViewCriteriaYearEnd > 0 Then
          sQuery.Append(Constants.cAndClause + " ac_year <=  " & searchCriteria.ViewCriteriaYearEnd)
        End If



        sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, True))

      End If



      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

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
        aError = "Error in get_flight_activity load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_flight_activity(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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


    Public Function get_flight_utilization(ByRef searchCriteria As viewSelectionCriteriaClass, Optional ByVal faa_date As String = "", Optional ByVal bFromUtilizationTab As Boolean = False, Optional ByVal NonURLTimespan As String = "") As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim sTimeSpan As String = ""
        Dim sTimeSpanQuery As String = ""

        Try

            If (searchCriteria.ViewID < 2) Then

                If bFromUtilizationTab Then
                    If Not IsNothing(HttpContext.Current.Request.Item("utilizationGraphRange")) Then
                        If Not String.IsNullOrEmpty(HttpContext.Current.Request.Item("utilizationGraphRange").ToString.Trim) Then
                            sTimeSpan = CInt(HttpContext.Current.Request.Item("utilizationGraphRange").ToString.Trim)
                        End If
                    End If
                End If

                If Not String.IsNullOrEmpty(NonURLTimespan) Then
                    sTimeSpan = NonURLTimespan
                End If

                If String.IsNullOrEmpty(sTimeSpan.Trim) Then
                    sTimeSpan = "2"
                End If

                If Month(Date.Now()) = 1 And Trim(sTimeSpan) = "2" Then
                    sTimeSpanQuery = " AND (DATEDIFF(month, ffd_date, GETDATE()) < 25) "
                Else
                    sTimeSpanQuery = " AND (DATEDIFF(year, ffd_date, GETDATE()) < " + CInt(sTimeSpan).ToString + ") "
                End If


            End If


            '-- GET SUMMARY OF FLIGHT ACTIVITY
            sQuery.Append(" SELECT distinct year(ffd_date) as tyear ,month(ffd_date) as tmonth ,COUNT(DISTINCT ac_id) as NUMAC, count(*) as FLIGHTS, ")
            sQuery.Append(" (COUNT(*)/COUNT(DISTINCT ac_id)) AS AVGFLTS, (sum(ffd_flight_time)/60) AS FLIGHTTIMEH, ")
            sQuery.Append(" sum(ffd_flight_time) AS FLIGHTTIMEM, ")
            sQuery.Append(" sum(ffd_flight_time)/count(*) AS AVGFLIGHTTIME, ")
            sQuery.Append(" sum(ffd_distance) AS STATMILES,")
            sQuery.Append(" sum(ffd_distance)/count(*) AS AVGSTATMILES")
            sQuery.Append(" FROM Aircraft_Flat WITH (NOLOCK) ")
            sQuery.Append(" INNER JOIN FAA_Flight_Data WITH (NOLOCK) ON ffd_ac_id = ac_id AND ffd_journ_id = ac_journ_id ")
            sQuery.Append(" LEFT OUTER JOIN airport o on o.aport_id = ffd_origin_aport_id LEFT OUTER JOIN airport d on d.aport_id = ffd_dest_aport_id ")
            sQuery.Append(" WHERE ffd_journ_id = 0 ")

            If Not String.IsNullOrEmpty(faa_date.Trim) Then
                sQuery.Append(" AND  ffd_date >= '" + faa_date.Trim + "'")
            ElseIf (searchCriteria.ViewID < 2) And Not String.IsNullOrEmpty(sTimeSpanQuery.Trim) Then
                sQuery.Append(sTimeSpanQuery)
            Else
                sQuery.Append(" AND (DATEDIFF(m, ffd_date, GETDATE()) < 24)")
            End If




            If searchCriteria.ViewCriteriaAFTTStart > 0 Then
                sQuery.Append(Constants.cAndClause + " ((ac_airframe_tot_hrs >= " & searchCriteria.ViewCriteriaAFTTStart & ") or (ac_airframe_tot_hrs IS NULL))")
            End If

            If searchCriteria.ViewCriteriaAFTTEnd > 0 Then
                sQuery.Append(Constants.cAndClause + " ((ac_airframe_tot_hrs <= " & searchCriteria.ViewCriteriaAFTTEnd & ") or (ac_airframe_tot_hrs IS NULL))")
            End If


            If searchCriteria.ViewCriteriaYearStart > 0 Then
                sQuery.Append(Constants.cAndClause + " ac_year >= " & searchCriteria.ViewCriteriaYearStart)
            End If


            If searchCriteria.ViewCriteriaYearEnd > 0 Then
                sQuery.Append(Constants.cAndClause + " ac_year <=  " & searchCriteria.ViewCriteriaYearEnd)
            End If

            sQuery.Append(" AND amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
            sQuery.Append(" GROUP BY YEAR(ffd_date), MONTH(ffd_date)")
            sQuery.Append(" ORDER BY YEAR(ffd_date), MONTH(ffd_date)")


            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

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
                aError = "Error in get_flight_utilization load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_flight_utilization(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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
    Public Shared Function setup_where_clause_client(ByVal bad_year_ac_id As String, ByVal variantListString As String, ByVal forsaleFlag As String, ByVal amod_id As Long, ByVal regType As String, ByVal afttStart As Long, ByVal from_spot As String) As String
        Dim where_clause As String = ""
    Try

      setup_where_clause_client = ""


      where_clause = " journ_subcat_code_part1='WS' AND journ_internal_trans_flag='N' "
      where_clause &= " and journ_subcat_code_part3 NOT IN ('DB','DS','FI','FY','IT','MF','RE','CC','LS', 'RM') "


      'query += " and ac_amod_id = @amodID" 

      If Trim(from_spot) = "Vintage" Then
        If Not String.IsNullOrEmpty(variantListString) Then
          where_clause &= " and amod_id in (" & amod_id & "," & variantListString & ")"
        Else
          where_clause &= " and amod_id = @amodID"
        End If
      Else
        If Not String.IsNullOrEmpty(variantListString) Then
          where_clause &= " and ac_amod_id in (" & amod_id & "," & variantListString & ")"
        Else
          where_clause &= " and ac_amod_id = @amodID"
        End If
      End If



      If forsaleFlag = "Y" Then
        where_clause &= " and ac_forsale_flag = 'Y' "
      End If

      If Trim(from_spot) = "Vintage" Then
      Else
        '-- ADD TRANSACTION DATE RANGE
        where_clause &= " and journ_date between @StartDate and @EndDate"
      End If




      '-- YEAR RANGE
      where_clause &= " and ac_year between @yearOne and @yearTwo"
      '-- WITH OR WITHOUT SALE PRICES

      'reg Type
      If regType = "N" Then
        where_clause &= " and ac_reg_no like 'N%' "
      ElseIf regType = "I" Then
        where_clause &= " and ac_reg_no not like 'N%' "
      End If

      '-- AFTT
      If afttStart = 0 Then
        where_clause &= " and ((ac_airframe_tot_hrs between @startAFTT and @endAFTT) or (ac_airframe_tot_hrs is NULL))"
      Else
        where_clause &= " and ac_airframe_tot_hrs between @startAFTT and @endAFTT"
      End If


      ' seems redundent, but not sure of a better way to include the criteria and get all the ac 
      setup_where_clause_client += " and ("

      '----------------------------
      setup_where_clause_client += " ("
      setup_where_clause_client += where_clause
      setup_where_clause_client += add_client_ac_string(False) ' where it is one of these ac and its not client 
      setup_where_clause_client += " ) "
      '----------------------------

      ' this can only be here, if there is client 
      If Trim(bad_year_ac_id) <> "" Then
        setup_where_clause_client += " or "

        '---------------------------- 
        setup_where_clause_client += " ("
        setup_where_clause_client += where_clause
        setup_where_clause_client += " and ac_id in (" & bad_year_ac_id & ") "
        setup_where_clause_client += " ) "
        '----------------------------
      End If

      setup_where_clause_client += " ) "

    Catch ex As Exception
      Return ""
    End Try

        Return setup_where_clause_client

    End Function

    Public Shared Function add_client_ac_string(ByVal include As Boolean) As String
        add_client_ac_string = ""

        Dim temp_query As String = ""

        Try

            If HttpContext.Current.Session.Item("localUser").crmUser_Evo_MPM_Flag = True And valueControl.has_client_data = True Then

                If include = False Then
                    If valueControl.array_count > 0 Then

                        For i = 0 To valueControl.array_count - 1
                            If Trim(temp_query) <> "" Then
                                temp_query &= ", "
                            End If

                            temp_query &= valueControl.ac_id_array(i)
                        Next

                        temp_query = " and ac_id not in (" & temp_query & ") "

                        add_client_ac_string = temp_query
                    End If
                Else
                    If valueControl.array_count > 0 Then

                        For i = 0 To valueControl.array_count - 1
                            If Trim(temp_query) <> "" Then
                                temp_query &= ", "
                            End If

                            temp_query &= valueControl.ac_id_array(i)
                        Next

                        temp_query = " or ac_id in (" & temp_query & ") "

                        add_client_ac_string = temp_query
                    End If
                End If



            End If

        Catch ex As Exception

        End Try
    End Function
    Public Function get_assett_summary(ByRef searchCriteria As viewSelectionCriteriaClass, Optional ByVal faa_date As String = "", Optional ByVal bFromUtilizationTab As Boolean = False, Optional ByVal YearOne As String = "", Optional ByVal YearTwo As String = "", Optional ByVal forsaleFlag As String = "", Optional ByVal regType As String = "", Optional ByVal afttStart As String = "", Optional ByVal afttEnd As String = "0", Optional ByVal VariantList As String = "", Optional ByVal bad_year_ac_id As String = "") As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        'Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim sTimeSpan As String = ""
        Dim sTimeSpanQuery As String = ""
        Dim afttStart_int As Integer = 0

        Try
            If Trim(afttStart) = "" Then
                afttStart_int = 0
            Else
                afttStart_int = CInt(afttStart)
            End If

            sQuery.Append(" select distinct amod_make_name, amod_model_name, amod_id, ac_year, min(afmv_value) AS LOWVALUE, AVG(afmv_value) AS AVGVALUE, MAX(afmv_value) AS HIGHVALUE,COUNT(*) as TOTVALUES, ")

            sQuery.Append(" (select avg(ac_sale_price) from Aircraft_Flat a2 with (NOLOCK) inner join Journal with (NOLOCK) on journ_id = ac_journ_id where ac_sale_price > 0 and a2.amod_id  = Aircraft_Flat.amod_id and a2.ac_year = Aircraft_Flat.ac_year and (journ_date > GETDATE() -365) ")


            sQuery.Append(setup_where_clause_client(bad_year_ac_id, VariantList, forsaleFlag, searchCriteria.ViewCriteriaAmodID.ToString, regType, afttStart_int, "Vintage"))


            '-- YEAR RANGE 
            If Not String.IsNullOrEmpty(YearOne) And Not String.IsNullOrEmpty(YearTwo) Then
                If YearOne > 0 And YearTwo > 0 Then
                    sQuery.Append(" and a2.ac_year between @yearOne and @yearTwo ")
                End If
            End If


            If Not String.IsNullOrEmpty(afttStart) And Not String.IsNullOrEmpty(afttEnd) Then
                '-- AFTT
                sQuery.Append(" and a2.ac_est_airframe_hrs between @startAFTT and @endAFTT")  ' changed from ac_airframe_tot_hrs to ac_est_airframe_hrs - MSW - 11/5/18
            End If

            sQuery.Append(" ) as avg_sale, ")
            sQuery.Append(" (select avg(ac_asking_price) from Aircraft a2 with (NOLOCK) where ac_asking_price > 0  and a2.ac_amod_id  = Aircraft_Flat.amod_id and a2.ac_year = Aircraft_Flat.ac_year and ac_journ_id = 0 ")


            '-- YEAR RANGE
            If Not String.IsNullOrEmpty(YearOne) And Not String.IsNullOrEmpty(YearTwo) Then
                If YearOne > 0 And YearTwo > 0 Then
                    sQuery.Append(" and a2.ac_year between @yearOne and @yearTwo ")
                End If
            End If

            If Not String.IsNullOrEmpty(afttStart) And Not String.IsNullOrEmpty(afttEnd) Then
                '-- AFTT
                sQuery.Append(" and a2.ac_airframe_tot_hrs between @startAFTT and @endAFTT")
            End If

            sQuery.Append(" ) as avg_asking ")



            ' ADDED MSW - ----------------------
            sQuery.Append(", (select sum(ac_asking_price) from Aircraft a2 with (NOLOCK) where ac_asking_price > 0  and a2.ac_amod_id  = Aircraft_Flat.amod_id and a2.ac_year = Aircraft_Flat.ac_year and ac_journ_id = 0 ")


            '-- YEAR RANGE
            If Not String.IsNullOrEmpty(YearOne) And Not String.IsNullOrEmpty(YearTwo) Then
                If YearOne > 0 And YearTwo > 0 Then
                    sQuery.Append(" and a2.ac_year between @yearOne and @yearTwo ")
                End If
            End If

            If Not String.IsNullOrEmpty(afttStart) And Not String.IsNullOrEmpty(afttEnd) Then
                '-- AFTT
                sQuery.Append(" and a2.ac_airframe_tot_hrs between @startAFTT and @endAFTT")
            End If

            sQuery.Append(" ) as SUMASKING ")

            sQuery.Append(", (select count(ac_asking_price) from Aircraft a2 with (NOLOCK) where ac_asking_price > 0  and a2.ac_amod_id  = Aircraft_Flat.amod_id and a2.ac_year = Aircraft_Flat.ac_year and ac_journ_id = 0 ")


            '-- YEAR RANGE
            If Not String.IsNullOrEmpty(YearOne) And Not String.IsNullOrEmpty(YearTwo) Then
                If YearOne > 0 And YearTwo > 0 Then
                    sQuery.Append(" and a2.ac_year between @yearOne and @yearTwo ")
                End If
            End If

            If Not String.IsNullOrEmpty(afttStart) And Not String.IsNullOrEmpty(afttEnd) Then
                '-- AFTT
                sQuery.Append(" and a2.ac_airframe_tot_hrs between @startAFTT and @endAFTT")
            End If

            sQuery.Append(" ) as COUNTASKING, ")

            sQuery.Append(" (select sum(ac_sale_price) from Aircraft_Flat a2 with (NOLOCK) inner join Journal with (NOLOCK) on journ_id = ac_journ_id where ac_sale_price > 0 and a2.amod_id  = Aircraft_Flat.amod_id and a2.ac_year = Aircraft_Flat.ac_year and (journ_date > GETDATE() -365) ")

            sQuery.Append(setup_where_clause_client(bad_year_ac_id, VariantList, forsaleFlag, searchCriteria.ViewCriteriaAmodID.ToString, regType, afttStart_int, "Vintage"))

            '-- YEAR RANGE 
            If Not String.IsNullOrEmpty(YearOne) And Not String.IsNullOrEmpty(YearTwo) Then
                If YearOne > 0 And YearTwo > 0 Then
                    sQuery.Append(" and a2.ac_year between @yearOne and @yearTwo ")
                End If
            End If


            If Not String.IsNullOrEmpty(afttStart) And Not String.IsNullOrEmpty(afttEnd) Then
                '-- AFTT
                sQuery.Append(" and a2.ac_est_airframe_hrs between @startAFTT and @endAFTT")  ' changed from ac_airframe_tot_hrs to ac_est_airframe_hrs - MSW - 11/5/18
            End If

            sQuery.Append(" ) as SUMSALE, ")

            sQuery.Append(" (select count(ac_sale_price) from Aircraft_Flat a2 with (NOLOCK) inner join Journal with (NOLOCK) on journ_id = ac_journ_id where ac_sale_price > 0 and a2.amod_id  = Aircraft_Flat.amod_id and a2.ac_year = Aircraft_Flat.ac_year and (journ_date > GETDATE() -365) ")

            sQuery.Append(setup_where_clause_client(bad_year_ac_id, VariantList, forsaleFlag, searchCriteria.ViewCriteriaAmodID.ToString, regType, afttStart_int, "Vintage"))

            '-- YEAR RANGE 
            If Not String.IsNullOrEmpty(YearOne) And Not String.IsNullOrEmpty(YearTwo) Then
                If YearOne > 0 And YearTwo > 0 Then
                    sQuery.Append(" and a2.ac_year between @yearOne and @yearTwo ")
                End If
            End If


            If Not String.IsNullOrEmpty(afttStart) And Not String.IsNullOrEmpty(afttEnd) Then
                '-- AFTT
                sQuery.Append(" and a2.ac_est_airframe_hrs between @startAFTT and @endAFTT")  ' changed from ac_airframe_tot_hrs to ac_est_airframe_hrs - MSW - 11/5/18
            End If

            sQuery.Append(" ) as COUNTSALE  ")
            ' ADDED MSW - ----------------------


            sQuery.Append(" from Aircraft_FMV with (NOLOCK) ")
            sQuery.Append(" inner join Aircraft_Flat with (NOLOCK) on afmv_ac_id = ac_id and ac_journ_id = 0 ")





            sQuery.Append(" where afmv_status='Y' and afmv_latest_flag='Y'  and afmv_value > 0 ")
            'sQuery.Append(" and amod_id = 33 ")
            '--and amod_id in (272,278,110)
            '--and ac_forsale_flag='Y'
            '--and not ac_asking_price is NULL

            'If searchCriteria.ViewCriteriaAmodID > 0 Then
            '  sQuery.Append(Constants.cAndClause + " amod_id = " & searchCriteria.ViewCriteriaAmodID & " ")
            'End If

            If Not String.IsNullOrEmpty(VariantList) Then
                sQuery.Append(" and amod_id in (" & searchCriteria.ViewCriteriaAmodID.ToString & "," & VariantList & ")")
            Else
                sQuery.Append(" and amod_id = @amodID")
            End If


            '-- YEAR RANGE
            If Not String.IsNullOrEmpty(YearOne) And Not String.IsNullOrEmpty(YearTwo) Then
                If YearOne > 0 And YearTwo > 0 Then
                    sQuery.Append(" and ac_year between @yearOne and @yearTwo")
                End If
            End If

            If forsaleFlag = "Y" Then
                sQuery.Append(" and ac_forsale_flag = 'Y' ")
            End If

            'reg Type
            If Not String.IsNullOrEmpty(regType) Then
                If regType = "N" Then
                    sQuery.Append(" and ac_reg_no like 'N%' ")
                ElseIf regType = "I" Then
                    sQuery.Append(" and ac_reg_no not like 'N%' ")
                End If
            End If


            If Not String.IsNullOrEmpty(afttStart) And Not String.IsNullOrEmpty(afttEnd) Then
                '-- AFTT
                sQuery.Append(" and afmv_airframe_hrs between @startAFTT and @endAFTT")
            End If

            sQuery.Append(" group by amod_make_name, amod_model_name, amod_id, ac_year ")

            If Not String.IsNullOrEmpty(VariantList) Then
                sQuery.Append(" order by ac_year,  amod_make_name, amod_model_name ")
            Else
                sQuery.Append(" order by amod_make_name, amod_model_name, ac_year ")
            End If

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

            SqlConn.ConnectionString = clientConnectString   '  "server=www.jetnetsql2.com;initial catalog=jetnet_ra_backup;Persist Security Info=False;User Id=sa;Password=krw32n89;"   '


            SqlConn.Open()

            Dim SqlCommand As New SqlClient.SqlCommand(sQuery.ToString, SqlConn)
            SqlCommand.Parameters.AddWithValue("amodID", searchCriteria.ViewCriteriaAmodID)

            SqlCommand.Parameters.AddWithValue("yearOne", YearOne)
            SqlCommand.Parameters.AddWithValue("yearTwo", YearTwo)
            SqlCommand.Parameters.AddWithValue("startAFTT", afttStart)
            SqlCommand.Parameters.AddWithValue("endAFTT", afttEnd)
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
    Public Function get_assett_summary_US_vs_Foreign(ByRef searchCriteria As viewSelectionCriteriaClass, Optional ByVal faa_date As String = "", Optional ByVal bFromUtilizationTab As Boolean = False) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim sTimeSpan As String = ""
        Dim sTimeSpanQuery As String = ""

        Try

            sQuery.Append(" select distinct amod_make_name, amod_model_name, ac_year,  case when left(ac_reg_no, 1) = 'N' then 1 else 0 end as IS_US_REG, AVG(afmv_value) AS AVGVALUE  ")
            sQuery.Append(" from Aircraft_FMV with (NOLOCK) ")
            sQuery.Append(" inner join Aircraft_Flat with (NOLOCK) on afmv_ac_id = ac_id and ac_journ_id = 0 ")
            sQuery.Append(" where afmv_status='Y' and afmv_latest_flag='Y' ")

            If searchCriteria.ViewCriteriaAmodID > 0 Then
                sQuery.Append(Constants.cAndClause + " amod_id = " & searchCriteria.ViewCriteriaAmodID & " ")
            End If

            sQuery.Append(" group by amod_make_name, amod_model_name, ac_year, case when left(ac_reg_no, 1) = 'N' then 1 else 0 end ")
            sQuery.Append(" order by amod_make_name, amod_model_name, ac_year ")




            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

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
                aError = "Error in get_assett_summary load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_assett_summary(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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
    Public Shared Sub clear_asking_sold_arrays()
        Dim f As Integer = 0

        valueControl.has_client_data = False
        For i = 0 To valueControl.array_count - 1
            valueControl.ac_id_array(f) = 0
            valueControl.ac_asking_array(f) = 0
            valueControl.ac_sold_array(f) = 0
            valueControl.ac_dlv_year_array(f) = 0
            valueControl.ac_sold_aftt_array(f) = 0
        Next
        valueControl.array_count = 0


        valueControl.has_current_client_data = False
        For i = 0 To valueControl.array_count_current - 1
            valueControl.ac_id_array_current(f) = 0
            valueControl.ac_asking_array_current(f) = 0
            valueControl.ac_dlv_year_array_current(f) = 0
            valueControl.ac_asking_aftt_array(f) = 0
        Next
        valueControl.array_count_current = 0

    End Sub
    Public Shared Sub Get_Client_AC_Models(ByVal amod_id As Integer, ByVal start_date As String, ByVal end_date As String)

        Try

            Call clear_asking_sold_arrays()

            ' added in MSW 
            If HttpContext.Current.Session.Item("localUser").crmUser_Evo_MPM_Flag = True Then
                Dim localDataLayer As New viewsDataLayer
                localDataLayer.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
                localDataLayer.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
                localDataLayer.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
                localDataLayer.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
                localDataLayer.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim


                Dim client_datatable As New DataTable
                client_datatable = localDataLayer.check_client_model_transactions(amod_id, start_date, end_date)

                If Not IsNothing(client_datatable) Then
                    If client_datatable.Rows.Count > 0 Then
                        For Each r As DataRow In client_datatable.Rows
                            valueControl.has_client_data = True

                            valueControl.ac_id_array(valueControl.array_count) = r("CLITRANS_jetnet_ac_id")
                            valueControl.ac_asking_array(valueControl.array_count) = r("clitrans_asking_price")
                            valueControl.ac_sold_array(valueControl.array_count) = r("clitrans_sold_price")

                            If Not IsDBNull(r("cliaircraft_year_dlv")) Then
                                valueControl.ac_dlv_year_array(valueControl.array_count) = r("cliaircraft_year_dlv")
                            Else
                                valueControl.ac_dlv_year_array(valueControl.array_count) = get_ac_year_dlv(valueControl.ac_id_array(valueControl.array_count))
                            End If

                            If Not IsDBNull(r("cliaircraft_airframe_total_hours")) Then
                                valueControl.ac_sold_aftt_array(valueControl.array_count) = r("cliaircraft_airframe_total_hours")
                            Else
                                valueControl.ac_sold_aftt_array(valueControl.array_count) = get_ac_aftt(valueControl.ac_id_array(valueControl.array_count))
                            End If


                            If Trim(valueControl.ac_id_array(valueControl.array_count)) = 10288 Then
                                valueControl.client_record_found = valueControl.client_record_found
                            End If

                            valueControl.array_count = valueControl.array_count + 1
                        Next

                    End If
                End If


                Dim client_datatable2 As New DataTable
                client_datatable2 = localDataLayer.check_client_model_current_market_all(amod_id)

                If Not IsNothing(client_datatable2) Then
                    If client_datatable2.Rows.Count > 0 Then
                        For Each r As DataRow In client_datatable2.Rows
                            valueControl.has_current_client_data = True

                            valueControl.ac_id_array_current(valueControl.array_count_current) = r("cliaircraft_jetnet_ac_id")
                            valueControl.ac_asking_array_current(valueControl.array_count_current) = r("cliaircraft_asking_price")
                            valueControl.ac_dlv_year_array_current(valueControl.array_count_current) = r("cliaircraft_year_dlv")

                            If Not IsDBNull(r("cliaircraft_airframe_total_hours")) Then
                                valueControl.ac_asking_aftt_array(valueControl.array_count_current) = r("cliaircraft_airframe_total_hours")
                            Else
                                valueControl.ac_asking_aftt_array(valueControl.array_count_current) = 0
                            End If

                            valueControl.array_count_current = valueControl.array_count_current + 1
                        Next

                    End If
                End If

            End If

        Catch ex As Exception

        End Try
    End Sub

    Public Shared Function get_ac_aftt(ByVal ac_id As Long) As Long
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim TempTable As New DataTable
        Dim query As String
        get_ac_aftt = 0

        Try
            'Opening Connection
            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString
            SqlConn.Open()


            query = "SELECT ac_est_airframe_hrs "
            query += " From Aircraft_Flat with (NOLOCK)"
            query += " where ac_id = " & ac_id & " "
            query += " and ac_journ_id = 0 "

            'clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, query.ToString)
            Dim SqlCommand As New SqlClient.SqlCommand(query, SqlConn)

            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                TempTable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = TempTable.GetErrors()
            End Try

            SqlCommand.Dispose()
            SqlCommand = Nothing

            If Not IsNothing(TempTable) Then
                If TempTable.Rows.Count > 0 Then
                    For Each r As DataRow In TempTable.Rows
                        get_ac_aftt = r("ac_est_airframe_hrs")
                    Next
                End If
            End If


        Catch ex As Exception
            get_ac_aftt = Nothing
            'Me.class_error = "Error in GetAircraftStartingTable(ByVal amod_id As Long) As DataTable SQL VERSION: " & ex.Message
        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing
        End Try

    End Function

    Public Shared Function get_ac_year_dlv(ByVal ac_id As Long) As Long
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim TempTable As New DataTable
        Dim query As String
        get_ac_year_dlv = 0

        Try
            'Opening Connection
            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString
            SqlConn.Open()


            query = "SELECT ac_year "
            query += " From Aircraft_Flat with (NOLOCK)"
            query += " where ac_id = " & ac_id & " "
            query += " and ac_journ_id = 0 "

            'clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, query.ToString)
            Dim SqlCommand As New SqlClient.SqlCommand(query, SqlConn)

            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                TempTable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = TempTable.GetErrors()
            End Try

            SqlCommand.Dispose()
            SqlCommand = Nothing

            If Not IsNothing(TempTable) Then
                If TempTable.Rows.Count > 0 Then
                    For Each r As DataRow In TempTable.Rows
                        get_ac_year_dlv = r("ac_year")
                    Next
                End If
            End If


        Catch ex As Exception
            get_ac_year_dlv = Nothing
            'Me.class_error = "Error in GetAircraftStartingTable(ByVal amod_id As Long) As DataTable SQL VERSION: " & ex.Message
        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing
        End Try

    End Function
    Public Function get_current_month_assett_summary(ByRef searchCriteria As viewSelectionCriteriaClass, Optional ByVal temp_ac_dlv_year As Integer = 0, Optional ByVal YearOne As String = "", Optional ByVal YearTwo As String = "", Optional ByVal forsaleFlag As String = "", Optional ByVal regType As String = "", Optional ByVal afttStart As String = "", Optional ByVal afttEnd As String = "", Optional ByVal VariantList As String = "", Optional ByVal force_current_month As String = "N") As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    'Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim sTimeSpan As String = ""
    Dim sTimeSpanQuery As String = ""

    Try

      If temp_ac_dlv_year > 0 Then
        sQuery.Append(" select distinct 'current_month' as type_of,   ")    ' amod_make_name, amod_model_name,
        sQuery.Append(" AVG(afmv_value) AS AVGVALUE ")    ',COUNT(*) as TOTVALUES
      Else
        sQuery.Append(" select distinct 'current_month' as type_of, 0.0 as asking_price, 0.0 as take_price, 0.0 as sale_price, min(afmv_value) AS LOWVALUE,  ")    ' amod_make_name, amod_model_name,
        If Trim(force_current_month) = "Y" Then
          sQuery.Append(" AVG(afmv_value) AS AVGVALUE, MAX(afmv_value) AS HIGHVALUE, month(GETDATE()) as month1, year(GETDATE()) as year1 ")    ',COUNT(*) as TOTVALUES
        Else
          sQuery.Append(" AVG(afmv_value) AS AVGVALUE, MAX(afmv_value) AS HIGHVALUE, month(afmv_date) as month1, year(afmv_date) as year1 ")    ',COUNT(*) as TOTVALUES
        End If 
      End If

      If temp_ac_dlv_year > 0 Then
      Else
        If searchCriteria.ViewCriteriaAircraftID > 0 And searchCriteria.ViewCriteriaAmodID = 0 Then
          sQuery.Append(", afmv_date as date1, 'Current eValue Estimate' as descrip ")
        End If
      End If

      If searchCriteria.ViewCriteriaAircraftID > 0 And searchCriteria.ViewCriteriaAmodID = 0 Then
        sQuery.Append(", afmv_source_id ")
      End If

      sQuery.Append(" from Aircraft_FMV with (NOLOCK) ")
      sQuery.Append(" inner join Aircraft_Flat with (NOLOCK) on afmv_ac_id = ac_id and ac_journ_id = 0 ")
      sQuery.Append(" where afmv_status='Y' and afmv_latest_flag='Y' and afmv_value > 0  ")


      '-- YEAR RANGE 
      If Not String.IsNullOrEmpty(YearOne) And Not String.IsNullOrEmpty(YearTwo) Then
        If YearOne > 0 And YearTwo > 0 Then
          sQuery.Append(" and ac_year between @yearOne and @yearTwo")
        End If
      End If

      If forsaleFlag = "Y" Then
        sQuery.Append(" and ac_forsale_flag = 'Y' ")
      End If

      'reg Type
      If Not String.IsNullOrEmpty(regType) Then
        If regType = "N" Then
          sQuery.Append(" and ac_reg_no like 'N%' ")
        ElseIf regType = "I" Then
          sQuery.Append(" and ac_reg_no not like 'N%' ")
        End If
      End If

      If Not String.IsNullOrEmpty(afttStart) And Not String.IsNullOrEmpty(afttEnd) Then
        '-- AFTT
        sQuery.Append(" and afmv_airframe_hrs  between @startAFTT and @endAFTT")
      End If


      'If searchCriteria.ViewCriteriaAmodID > 0 Then
      '  sQuery.Append(Constants.cAndClause + " amod_id = " & searchCriteria.ViewCriteriaAmodID & " ")
      'End If

      If Not String.IsNullOrEmpty(VariantList) Then
        sQuery.Append(" and amod_id in (" & searchCriteria.ViewCriteriaAmodID.ToString & "," & VariantList & ")")
      Else
        If searchCriteria.ViewCriteriaAmodID > 0 Then
          sQuery.Append(" and amod_id = @amodID")
        End If
      End If


      'if we have an mft year then skip the ac (so it has a dual function) 
      If temp_ac_dlv_year > 0 Then
        sQuery.Append(Constants.cAndClause + " ac_year = " & temp_ac_dlv_year & " ")
      Else
        If searchCriteria.ViewCriteriaAircraftID > 0 Then
          sQuery.Append(Constants.cAndClause + " ac_id = " & searchCriteria.ViewCriteriaAircraftID & " ")
        End If

        If Trim(force_current_month) = "Y" Then
        Else
          sQuery.Append(" group by year(afmv_date),  month(afmv_date)  ")
        End If

        If searchCriteria.ViewCriteriaAircraftID > 0 And searchCriteria.ViewCriteriaAmodID = 0 Then
          sQuery.Append(" , afmv_date ") 
          sQuery.Append(", afmv_source_id ") 
        End If
      End If




      ' sQuery.Append(" order by amod_make_name, amod_model_name ")

      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)
      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()

      Dim SqlCommand As New SqlClient.SqlCommand(sQuery.ToString, SqlConn)
      SqlCommand.Parameters.AddWithValue("amodID", searchCriteria.ViewCriteriaAmodID)
      SqlCommand.Parameters.AddWithValue("yearOne", YearOne)
      SqlCommand.Parameters.AddWithValue("yearTwo", YearTwo)
      SqlCommand.Parameters.AddWithValue("startAFTT", afttStart)
      SqlCommand.Parameters.AddWithValue("endAFTT", afttEnd)
      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
        aError = "Error in get_current_month_assett_summary load datatable " + constrExc.Message
      End Try


      SqlReader.Close()
      SqlReader = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_current_month_assett_summary(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

    End Try

    Return atemptable

  End Function
  Public Function get_residual_assett_summary_by_Year_MFR(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal get_acs_dlv_year As String, ByVal ac_dlv_year As Integer, Optional ByVal YearOne As String = "", Optional ByVal YearTwo As String = "", Optional ByVal forsaleFlag As String = "", Optional ByVal regType As String = "", Optional ByVal afttStart As String = "", Optional ByVal afttEnd As String = "", Optional ByVal VariantList As String = "") As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    'Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim sTimeSpan As String = ""
    Dim sTimeSpanQuery As String = ""

    Try


      If searchCriteria.ViewCriteriaAircraftID > 0 Then
        If Trim(get_acs_dlv_year) = "Y" Then
          sQuery.Append(" select distinct 'AVG " & ac_dlv_year & " " & searchCriteria.ViewCriteriaAircraftMake & " " & searchCriteria.ViewCriteriaAircraftModel & "'  as type_of, ")   '  'AVG MFR YEAR MODEL'  as type_of2,  ' 'AVG " & ac_mf_year & " " & searchCriteria.ViewCriteriaAircraftMake & " " & searchCriteria.ViewCriteriaAircraftModel & "'  as type_of2,
        Else
          sQuery.Append(" select distinct  'MY AC'  as type_of,  ")  ' 'MY AC'  as type_of2,
        End If
      Else
        sQuery.Append(" select distinct 'future_month'  as type_of, ")
      End If

      sQuery.Append(" YEAR(aires_date) as year1, MONTH(aires_date) as month1, ac_year, ")
      sQuery.Append(" min(aires_residual) AS LOWVALUE, AVG(aires_residual) AS AVGVALUE, MAX(aires_residual) AS HIGHVALUE,COUNT(*) as TOTVALUES, 0.0 as asking_price, 0.0 as take_price, 0.0 as sale_price ")

      sQuery.Append(" from Asset_Insight_Residual with (NOLOCK) ")
      sQuery.Append(" inner join Aircraft_Flat with (NOLOCK) on aires_ac_id = ac_id and ac_journ_id = 0 ")

      sQuery.Append(" where ")
      If Date.Now.Month = 12 Then
        sQuery.Append(" aires_date >= cast('1/1/' + cast(" & (Date.Now.Year + 1) & " as varchar(30)) as varchar(30)) ")
      Else
        sQuery.Append(" aires_date >= cast(" & (Date.Now.Month + 1) & " as varchar(30)) + '/1/' + cast(" & Date.Now.Year & " as varchar(30)) ")
      End If


      '-- YEAR RANGE 
      If Not String.IsNullOrEmpty(YearOne) And Not String.IsNullOrEmpty(YearTwo) Then
        If YearOne > 0 And YearTwo > 0 Then
          sQuery.Append(" and ac_year between @yearOne and @yearTwo")
        End If
      End If


      If forsaleFlag = "Y" Then
        sQuery.Append(" and ac_forsale_flag = 'Y' ")
      End If

      'reg Type
      If Not String.IsNullOrEmpty(regType) Then
        If regType = "N" Then
          sQuery.Append(" and ac_reg_no like 'N%' ")
        ElseIf regType = "I" Then
          sQuery.Append(" and ac_reg_no not like 'N%' ")
        End If
      End If


      If Not String.IsNullOrEmpty(afttStart) And Not String.IsNullOrEmpty(afttEnd) Then
        '-- AFTT
        sQuery.Append(" and ac_airframe_tot_hrs between @startAFTT and @endAFTT")
      End If


      If searchCriteria.ViewCriteriaAircraftID > 0 Then
        If Trim(get_acs_dlv_year) = "Y" Then
          sQuery.Append(Constants.cAndClause + " amod_id = " & searchCriteria.ViewCriteriaAmodID & " ")
          sQuery.Append(Constants.cAndClause + " ac_year = " & ac_dlv_year & " ")
        Else
          sQuery.Append(Constants.cAndClause + " ac_id = " & searchCriteria.ViewCriteriaAircraftID & " ")
        End If


      Else
        'If searchCriteria.ViewCriteriaAmodID > 0 Then
        '  sQuery.Append(Constants.cAndClause + " amod_id = " & searchCriteria.ViewCriteriaAmodID & " ")
        'End If

        If Not String.IsNullOrEmpty(VariantList) Then
          sQuery.Append(" and amod_id in (" & searchCriteria.ViewCriteriaAmodID.ToString & "," & VariantList & ")")
        Else
          sQuery.Append(" and amod_id = @amodID")
        End If

      End If




      '    sQuery.Append(" and YEAR(aires_date) < 2019 ")
      sQuery.Append(" group by  YEAR(aires_date), MONTH(aires_date) ,  ac_year ")
      sQuery.Append(" order by YEAR(aires_date) asc, MONTH(aires_date) asc , ac_year  ")

      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)


      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString
      SqlConn.Open()

      Dim SqlCommand As New SqlClient.SqlCommand(sQuery.ToString, SqlConn)
      SqlCommand.Parameters.AddWithValue("amodID", searchCriteria.ViewCriteriaAmodID)
      SqlCommand.Parameters.AddWithValue("yearOne", YearOne)
      SqlCommand.Parameters.AddWithValue("yearTwo", YearTwo)
      SqlCommand.Parameters.AddWithValue("startAFTT", afttStart)
      SqlCommand.Parameters.AddWithValue("endAFTT", afttEnd)
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
  Public Function get_residual_assett_summary(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim sTimeSpan As String = ""
    Dim sTimeSpanQuery As String = ""

    Try

      sQuery.Append(" select distinct top 6 'future_month' as type_of, YEAR(aires_date) as year1, MONTH(aires_date) as month1, ")
      sQuery.Append(" min(aires_residual) AS LOWVALUE, AVG(aires_residual) AS AVGVALUE, MAX(aires_residual) AS HIGHVALUE,COUNT(*) as TOTVALUES, 0.0 as asking_price, 0.0 as take_price, 0.0 as sale_price ")

      If searchCriteria.ViewCriteriaAircraftID > 0 Then
        sQuery.Append(" , aires_date as date1 ")
      End If

      sQuery.Append(" from Asset_Insight_Residual with (NOLOCK) ")
      sQuery.Append(" inner join Aircraft_Flat with (NOLOCK) on aires_ac_id = ac_id and ac_journ_id = 0 ")

      sQuery.Append(" where ")
      If Date.Now.Month = 12 Then
        sQuery.Append(" aires_date >= cast('1/1/' + cast(" & (Date.Now.Year + 1) & " as varchar(30)) ")
      Else
        sQuery.Append(" aires_date >= cast(" & (Date.Now.Month + 1) & " as varchar(30)) + '/1/' + cast(" & Date.Now.Year & " as varchar(30)) ")
      End If

      If searchCriteria.ViewCriteriaAmodID > 0 Then
        sQuery.Append(Constants.cAndClause + " amod_id = " & searchCriteria.ViewCriteriaAmodID & " ")
      End If

      If searchCriteria.ViewCriteriaAircraftID > 0 Then
        sQuery.Append(Constants.cAndClause + " ac_id = " & searchCriteria.ViewCriteriaAircraftID & " ")
      End If



      '--and ac_forsale_flag='Y'
      '--and not ac_asking_price is NULL
      sQuery.Append(" group by YEAR(aires_date), MONTH(aires_date) ")
      If searchCriteria.ViewCriteriaAircraftID > 0 Then
        sQuery.Append(", aires_date ")
      End If
      sQuery.Append(" order by YEAR(aires_date) asc, MONTH(aires_date) asc ")

      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

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
        aError = "Error in get_residual_assett_summary load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_residual_assett_summary(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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
  Public Function get_past_month_assett_summary(ByRef searchCriteria As viewSelectionCriteriaClass, Optional ByVal YearOne As String = "", Optional ByVal YearTwo As String = "", Optional ByVal forsaleFlag As String = "", Optional ByVal regType As String = "", Optional ByVal afttStart As String = "", Optional ByVal afttEnd As String = "", Optional ByVal VariantList As String = "") As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    'Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim sTimeSpan As String = ""
    Dim sTimeSpanQuery As String = ""

    Try

      sQuery.Append(" select distinct 'past_month' as type_of,0 as asking_price, 0.0 as take_price, 0 as sale_price, ")  'amod_make_name, amod_model_name,
      sQuery.Append(" YEAR(afmv_date) as year1, MONTH(afmv_date) as month1, ")
      sQuery.Append("  min(afmv_value) AS LOWVALUE, AVG(afmv_value) AS AVGVALUE, MAX(afmv_value) AS HIGHVALUE ")    ',COUNT(*) as TOTVALUES

      If searchCriteria.ViewCriteriaAircraftID > 0 And searchCriteria.ViewCriteriaAmodID = 0 Then
        sQuery.Append(" ,  afmv_date as date1, 'eValue Estimate' as descrip ")
      End If


      sQuery.Append(" from Aircraft_FMV with (NOLOCK) ")
      sQuery.Append(" inner join Aircraft_Flat with (NOLOCK) on afmv_ac_id = ac_id and ac_journ_id = 0 ")
      sQuery.Append(" where afmv_status='Y'  and afmv_value > 0 and afmv_latest_flag = 'N' ")



      '-- YEAR RANGE


      If Not String.IsNullOrEmpty(YearOne) And Not String.IsNullOrEmpty(YearTwo) Then
        If YearOne > 0 And YearTwo > 0 Then
          sQuery.Append(" and ac_year between @yearOne and @yearTwo")
        End If
      End If


      If forsaleFlag = "Y" Then
        sQuery.Append(" and ac_forsale_flag = 'Y' ")
      End If

      'reg Type
      If Not String.IsNullOrEmpty(regType) Then
        If regType = "N" Then
          sQuery.Append(" and ac_reg_no like 'N%' ")
        ElseIf regType = "I" Then
          sQuery.Append(" and ac_reg_no not like 'N%' ")
        End If
      End If


      If Not String.IsNullOrEmpty(afttStart) And Not String.IsNullOrEmpty(afttEnd) Then
        '-- AFTT
        sQuery.Append(" and afmv_airframe_hrs between @startAFTT and @endAFTT")
      End If


      'If searchCriteria.ViewCriteriaAmodID > 0 Then
      '  sQuery.Append(Constants.cAndClause + " amod_id = " & searchCriteria.ViewCriteriaAmodID & " ")
      'End If

      If Not String.IsNullOrEmpty(VariantList) Then
        sQuery.Append(" and amod_id in (" & searchCriteria.ViewCriteriaAmodID.ToString & "," & VariantList & ")")
      ElseIf searchCriteria.ViewCriteriaAmodID > 0 Then 
        sQuery.Append(" and amod_id = @amodID") 
      End If


      If searchCriteria.ViewCriteriaAircraftID > 0 Then
        sQuery.Append(Constants.cAndClause + " ac_id = " & searchCriteria.ViewCriteriaAircraftID & " ")
      End If

      ' If searchCriteria.ViewCriteriaAircraftID > 0 Then
      sQuery.Append(" and afmv_date <  getdate() ")
      ' Else
      '   sQuery.Append(" and afmv_date < cast((month(getdate()) -0) as varchar(30)) + '/1/' + cast(year(getdate()) as varchar(30)) ")
      ' End If


      'If searchCriteria.ViewCriteriaAircraftID > 0 Then
      '   Else

      sQuery.Append("  and ( ")


      Dim i As Integer = 0
      Dim year_to_get As Integer = 0
      Dim month_to_get As Integer = 0

      month_to_get = Date.Now.Month
      year_to_get = Date.Now.Year

      For i = 0 To 11
        If i > 0 Then
          sQuery.Append("  OR ")
        End If
        sQuery.Append("  afmv_id in ( ")
        sQuery.Append("  select top 1 afmv_id from Aircraft_FMV afmv2 with (NOLOCK)  ")
        sQuery.Append("  inner join Aircraft_Flat af2 with (NOLOCK) on afmv_ac_id = af2.ac_id and af2.ac_journ_id = 0 ")
        sQuery.Append("  where  afmv_status='Y'")

        'If searchCriteria.ViewCriteriaAmodID > 0 Then
        '  sQuery.Append(Constants.cAndClause + " af2.amod_id = " & searchCriteria.ViewCriteriaAmodID & " ")
        'End If

        If Not String.IsNullOrEmpty(VariantList) Then
          sQuery.Append(" and af2.amod_id in (" & searchCriteria.ViewCriteriaAmodID.ToString & "," & VariantList & ")")
        ElseIf searchCriteria.ViewCriteriaAmodID > 0 Then
          sQuery.Append(" and af2.amod_id = @amodID")
        End If

        '-- YEAR RANGE  
        If Not String.IsNullOrEmpty(YearOne) And Not String.IsNullOrEmpty(YearTwo) Then
          If YearOne > 0 And YearTwo > 0 Then
            sQuery.Append(" and af2.ac_year between @yearOne and @yearTwo ")
          End If
        End If


        If forsaleFlag = "Y" Then
          sQuery.Append(" and af2.ac_forsale_flag = 'Y' ")
        End If

        'reg Type
        If Not String.IsNullOrEmpty(regType) Then
          If regType = "N" Then
            sQuery.Append(" and af2.ac_reg_no like 'N%' ")
          ElseIf regType = "I" Then
            sQuery.Append(" and af2.ac_reg_no not like 'N%' ")
          End If
        End If


        If Not String.IsNullOrEmpty(afttStart) And Not String.IsNullOrEmpty(afttEnd) Then
          '-- AFTT
          sQuery.Append(" and afmv2.afmv_airframe_hrs between @startAFTT and @endAFTT")
        End If


        If month_to_get = 1 Then
          'greater than the first of last month
          sQuery.Append("   and afmv2.afmv_date>= cast(12 as varchar(30)) + '/1/' + cast(" & year_to_get - 1 & " as varchar(30)) ")
          ' and less than the first of this month
          sQuery.Append("   and afmv2.afmv_date < cast(" & month_to_get & " as varchar(30)) + '/1/' + cast(" & year_to_get & " as varchar(30)) ")

        ElseIf month_to_get = 0 Then
          month_to_get = 12
          year_to_get = year_to_get - 1

          'greater than the first of last month
          sQuery.Append("   and afmv2.afmv_date>= cast(" & (month_to_get - 1) & " as varchar(30)) + '/1/' + cast(" & year_to_get & " as varchar(30)) ")
          ' and less than the first of this month
          sQuery.Append("   and afmv2.afmv_date < cast(" & month_to_get & " as varchar(30)) + '/1/' + cast(" & year_to_get & " as varchar(30)) ")

        Else
          'greater than the first of last month
          sQuery.Append("   and afmv2.afmv_date>= cast(" & (month_to_get - 1) & " as varchar(30)) + '/1/' + cast(" & year_to_get & " as varchar(30)) ")
          ' and less than the first of this month
          sQuery.Append("   and afmv2.afmv_date < cast(" & month_to_get & " as varchar(30)) + '/1/' + cast(" & year_to_get & " as varchar(30)) ")
        End If


        month_to_get = month_to_get - 1


        sQuery.Append("   and afmv2.afmv_ac_id  = Aircraft_FMV.afmv_ac_id  ")
        sQuery.Append("   group by afmv2.afmv_ac_id, afmv2.afmv_id, year(afmv2.afmv_date), month(afmv2.afmv_date), afmv2.afmv_date ")
        sQuery.Append("  order by afmv2.afmv_date desc ")
        sQuery.Append("  ) ")
      Next

      sQuery.Append("  ) ")
      '  End If

      '--and amod_id in (272,278,110)
      '--and ac_forsale_flag='Y'
      '--and not ac_asking_price is NULL
      sQuery.Append(" group by  YEAR(afmv_date), MONTH(afmv_date) ")

      If searchCriteria.ViewCriteriaAircraftID > 0 And searchCriteria.ViewCriteriaAmodID = 0 Then
        sQuery.Append(" ,  afmv_date")
      End If

      sQuery.Append(" order by YEAR(afmv_date) asc, MONTH(afmv_date) asc ")

      If searchCriteria.ViewCriteriaAircraftID > 0 And searchCriteria.ViewCriteriaAmodID = 0 Then
        sQuery.Append(" , afmv_date asc ")
      End If

      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()

      Dim SqlCommand As New SqlClient.SqlCommand(sQuery.ToString, SqlConn)
      SqlCommand.Parameters.AddWithValue("amodID", searchCriteria.ViewCriteriaAmodID)
      SqlCommand.Parameters.AddWithValue("yearOne", YearOne)
      SqlCommand.Parameters.AddWithValue("yearTwo", YearTwo)
      SqlCommand.Parameters.AddWithValue("startAFTT", afttStart)
      SqlCommand.Parameters.AddWithValue("endAFTT", afttEnd)
      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)


      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
        aError = "Error in get_past_month_assett_summary load datatable " + constrExc.Message
      End Try

      SqlReader.Close()
      SqlReader = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_past_month_assett_summary(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

    End Try

    Return atemptable

  End Function
  Public Function get_jetnet_asking_w_sold(ByRef searchCriteria As viewSelectionCriteriaClass, Optional ByVal YearOne As String = "", Optional ByVal YearTwo As String = "", Optional ByVal forsaleFlag As String = "", Optional ByVal regType As String = "", Optional ByVal afttStart As String = "", Optional ByVal afttEnd As String = "", Optional ByVal VariantList As String = "") As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    'Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim sTimeSpan As String = ""
    Dim sTimeSpanQuery As String = ""

    Try

      sQuery.Append(" select 'asking_w_sold' as type_of, case when avg(ac_asking_price) IS null then 0 else avg(ac_asking_price) end   as asking_price, 0.0 as take_price, case when avg(ac_sale_price) IS null then 0 else avg(ac_sale_price) end   as sale_price, year(journ_date) as year1, MONTH(journ_date) as month1, 0 as LOWVALUE, 0 as AVGVALUE, 0 as HIGHVALUE ")
      If searchCriteria.ViewCriteriaAircraftID > 0 Then
        sQuery.Append(", journ_date as date1, journ_subject as descrip ")
      End If
      sQuery.Append(" from Aircraft_Flat with (NOLOCK)  ")
      sQuery.Append(" inner join Journal with (NOLOCK) on journ_id = ac_journ_id  ")
      sQuery.Append(" where journ_date >= '' + cast(month(getdate() -365) as varchar(30)) + '/1/' + cast(year(getdate() -365) as varchar(30)) ")

      'sQuery.Append(" and ac_asking_price > 0 And ac_sale_price > 0 ")
      sQuery.Append(" and (ac_asking_price > 0 or ac_sale_price > 0) ")

      ' ADDED IN MSW - FOR MAKING SURE WE ARE JUST GETTING SOLDS

      Dim aclsData_Temp2 As New clsData_Manager_SQL
      sQuery.Append(aclsData_Temp2.add_in_wholesale_non_internal_retail_string())

      'If searchCriteria.ViewCriteriaAmodID > 0 Then
      '  sQuery.Append(Constants.cAndClause + " ac_amod_id = " & searchCriteria.ViewCriteriaAmodID & " ")
      'End If

      If Not String.IsNullOrEmpty(VariantList) Then
        sQuery.Append(" and amod_id in (" & searchCriteria.ViewCriteriaAmodID.ToString & "," & VariantList & ")")
      Else
        sQuery.Append(" and amod_id = @amodID")
      End If

      If searchCriteria.ViewCriteriaAircraftID > 0 Then
        sQuery.Append(Constants.cAndClause + " ac_id = " & searchCriteria.ViewCriteriaAircraftID & " ")
      End If

      '-- YEAR RANGE
      If Not String.IsNullOrEmpty(YearOne) And Not String.IsNullOrEmpty(YearTwo) Then
        If YearOne > 0 And YearTwo > 0 Then
          sQuery.Append(" and ac_year between @yearOne and @yearTwo")
        End If
      End If


      If forsaleFlag = "Y" Then
        sQuery.Append(" and ac_forsale_flag = 'Y' ")
      End If

      'reg Type
      If Not String.IsNullOrEmpty(regType) Then
        If regType = "N" Then
          sQuery.Append(" and ac_reg_no like 'N%' ")
        ElseIf regType = "I" Then
          sQuery.Append(" and ac_reg_no not like 'N%' ")
        End If
      End If


      If Not String.IsNullOrEmpty(afttStart) And Not String.IsNullOrEmpty(afttEnd) Then
        '-- AFTT
        sQuery.Append(" and ac_est_airframe_hrs between @startAFTT and @endAFTT")     '  changed from ac_airframe_tot_hrs to ac_est_airframe_hrs - MSW - 11/5/18
      End If



      sQuery.Append(" group by year(journ_date), MONTH(journ_date) ")
      If searchCriteria.ViewCriteriaAircraftID > 0 Then
        sQuery.Append(", journ_date, journ_subject ")
      End If
      sQuery.Append(" order by year(journ_date) desc, MONTH(journ_date) desc ")


      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()

      Dim SqlCommand As New SqlClient.SqlCommand(sQuery.ToString, SqlConn)
      SqlCommand.Parameters.AddWithValue("amodID", searchCriteria.ViewCriteriaAmodID)
      SqlCommand.Parameters.AddWithValue("yearOne", YearOne)
      SqlCommand.Parameters.AddWithValue("yearTwo", YearTwo)
      SqlCommand.Parameters.AddWithValue("startAFTT", afttStart)
      SqlCommand.Parameters.AddWithValue("endAFTT", afttEnd)
      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
        aError = "Error in get_jetnet_asking_w_sold load datatable " + constrExc.Message
      End Try



      SqlReader.Close()
      SqlReader = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_jetnet_asking_w_sold(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

    Finally


      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

    End Try

    Return atemptable

  End Function

  Public Sub views_display_flight_utilization(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, Optional ByVal faa_date As String = "")

    Dim htmlOut As New StringBuilder
    Dim results_table As New DataTable

    Try

      htmlOut.Append("<br clear=""all"" /><br /><div class=""aircraft_list display_block""><span class=""tabheader padding_table""><strong>Flight&nbsp;Utilization</strong></span></div><div class=""clearfix""></div>")
      htmlOut.Append("<div class=""resizeCW""><table id='flightUtilTable' width='100%' cellspacing='0' cellpadding='4'>")


      results_table = get_flight_utilization(searchCriteria, faa_date)

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          'htmlOut.Append("<tr class='aircraft_list'>")
          'htmlOut.Append("<td valign='middle' align='left' class='tabheader'><strong>Flight&nbsp;Utilization</strong></td>")
          'htmlOut.Append("</tr>")

          htmlOut.Append("<thead>")
          htmlOut.Append("<tr>")
          htmlOut.Append("<th>Year</th>")
          htmlOut.Append("<th>Month</th>")
          htmlOut.Append("<th>#AC</strong></th>")
          htmlOut.Append("<th>#Flights</th>")
          htmlOut.Append("<th>AVG Flights</th>")
          htmlOut.Append("<th>Flight Time HRS</th>")
          htmlOut.Append("<th>Flight Time Min</th>")
          htmlOut.Append("<th>Avg Flight Time</th>")
          htmlOut.Append("<th>Nautical Miles</th>")
          htmlOut.Append("<th>Avg Nautical Miles</th>")
          htmlOut.Append("</tr>")
          htmlOut.Append("</thead>")

          htmlOut.Append("<tbody>")
          For Each r As DataRow In results_table.Rows

            htmlOut.Append("<tr>")

            If Not IsDBNull(r("tyear")) Then
              htmlOut.Append("<td>" & r.Item("tyear") & "</td>")
            Else
              htmlOut.Append("<td>0</td>")
            End If

            If Not IsDBNull(r("tmonth")) Then
              htmlOut.Append("<td>" & r.Item("tmonth") & "</td>")
            Else
              htmlOut.Append("<td>0</td>")
            End If

            If Not IsDBNull(r("NUMAC")) Then
              htmlOut.Append("<td>" & r.Item("NUMAC") & "</td>")
            Else
              htmlOut.Append("<td>0</td>")
            End If

            If Not IsDBNull(r("FLIGHTS")) Then
              htmlOut.Append("<td >" & r.Item("FLIGHTS") & "</td>")
            Else
              htmlOut.Append("<td>0</td>")
            End If

            If Not IsDBNull(r("AVGFLTS")) Then
              htmlOut.Append("<td>" & r.Item("AVGFLTS") & "</td>")
            Else
              htmlOut.Append("<td>0</td>")
            End If

            If Not IsDBNull(r("FLIGHTTIMEH")) Then
              htmlOut.Append("<td>" & r.Item("FLIGHTTIMEH") & "</td>")
            Else
              htmlOut.Append("<td>0</td>")
            End If

            If Not IsDBNull(r("FLIGHTTIMEM")) Then
              htmlOut.Append("<td>" & r.Item("FLIGHTTIMEM") & "</td>")
            Else
              htmlOut.Append("<td>0</td>")
            End If

            If Not IsDBNull(r("AVGFLIGHTTIME")) Then
              htmlOut.Append("<td>" & r.Item("AVGFLIGHTTIME") & "</td>")
            Else
              htmlOut.Append("<td>0</td>")
            End If

            If Not IsDBNull(r("STATMILES")) Then
              htmlOut.Append("<td>" & FormatNumber(flightDataFunctions.ConvertStatuteMileToNauticalMile(r.Item("STATMILES")), 0) & "</td>")
            Else
              htmlOut.Append("<td>0</td>")
            End If

            If Not IsDBNull(r("AVGSTATMILES")) Then
              htmlOut.Append("<td>" & FormatNumber(flightDataFunctions.ConvertStatuteMileToNauticalMile(r.Item("AVGSTATMILES")), 0) & "</td>")
            Else
              htmlOut.Append("<td>0</td>")
            End If

            htmlOut.Append("</tr>")
          Next
          htmlOut.Append("</tbody>")
        Else
          htmlOut.Append("<tr><td valign='middle' align='left' class='tabheader'><strong>Flight&nbsp;Utilization</strong>&nbsp;<em>(last&nbsp;90&nbsp;days)</em></td><td width='40%' class='border_bottom'>&nbsp;</td></tr>")
          htmlOut.Append("<tr><td valign='middle' align='center' class='border_bottom_right' colspan='2'>No Flight Utilization Data at this time, for this Make/Model ...</td></tr>")
        End If

      Else
        htmlOut.Append("<tr><td valign='middle' align='left' class='tabheader'><strong>Flight&nbsp;Utilization</strong>&nbsp;<em>(last&nbsp;90&nbsp;days)</em></td><td width='40%' class='border_bottom'>&nbsp;</td></tr>")
        htmlOut.Append("<tr><td valign='middle' align='center' class='border_bottom_right' colspan='2'>No Flight Utilization Data at this time, for this Make/Model ...</td></tr>")
      End If

      htmlOut.Append("</table></div>" + vbCrLf)


    Catch ex As Exception

      aError = "Error in views_display_flight_utilization(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String,, Optional ByVal faa_date As String = "") " + ex.Message

    Finally

    End Try

    'return resulting html string
    out_htmlString = htmlOut.ToString
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Sub views_display_flight_activity(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, Optional ByVal use_faa_data As Boolean = False, Optional ByVal faa_date As String = "", Optional ByVal filterDate As Boolean = False)

    Dim htmlOut As New StringBuilder
    Dim results_table As New DataTable

    Try


      htmlOut.Append("<table id='flightActivityTable' width='100%' cellspacing='0' cellpadding='4'>")


      results_table = get_flight_activity(searchCriteria, use_faa_data, faa_date, filterDate)

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          For Each r As DataRow In results_table.Rows
            htmlOut.Append("<thead>")
            If use_faa_data Then
              htmlOut.Append("<tr class='aircraft_list'><th valign='middle' align='left' class='tabheader' colspan=""2""><strong>Flight&nbsp;Activity</strong>&nbsp;<em>(last&nbsp;365&nbsp;days)</em></th></tr>")
            Else
              htmlOut.Append("<tr class='aircraft_list'><th valign='middle' align='left' class='tabheader' colspan=""2""><strong>Flight&nbsp;Activity</strong>&nbsp;<em>(last&nbsp;90&nbsp;days)</em></th></tr>")
            End If
            htmlOut.Append("</thead>")
            htmlOut.Append("<tbody>")
            If Not IsDBNull(r("tflights")) Then
              htmlOut.Append("<tr><td valign='middle' align='left' class='seperator'>Number of Flights</td><td valign='top' align='left' class='rightside'>" + FormatNumber(CDbl(r.Item("tflights").ToString), 0) + "</td></tr>")
            Else
              htmlOut.Append("<tr><td valign='middle' align='left' class='seperator'>Number of Flights</td><td valign='top' align='left' class='rightside'>0</td></tr>")
            End If

            If Not IsDBNull(r("avgdistance")) Then
              htmlOut.Append("<tr><td valign='middle' align='left' class='seperator'>Average Distance</td><td valign='top' align='left' class='rightside'>" & FormatNumber(flightDataFunctions.ConvertStatuteMileToNauticalMile(CDbl(r.Item("avgdistance").ToString)), 0) & " <em>(nm)</em></td></tr>")
            Else
              htmlOut.Append("<tr><td valign='middle' align='left' class='seperator'>Average Distance</td><td valign='top' align='left' class='rightside'>0 <em>(nm)</em></td></tr>")
            End If

            If Not IsDBNull(r("tdistance")) Then
              htmlOut.Append("<tr><td valign='middle' align='left' class='seperator'>Total Distance</td><td valign='top' align='left' class='rightside'>" & FormatNumber(flightDataFunctions.ConvertStatuteMileToNauticalMile(CDbl(r.Item("tdistance").ToString)), 0) & " <em>(nm)</em></td></tr>")
            Else
              htmlOut.Append("<tr><td valign='middle' align='left' class='seperator'>Total Distance</td><td valign='top' align='left' class='rightside'>0 <em>(nm)</em></td></tr>")
            End If

            If Not IsDBNull(r("avgflighttime")) Then
              htmlOut.Append("<tr><td valign='middle' align='left' class='seperator'>Average Flight Time</td><td valign='top' align='left' class='rightside'>" + FormatNumber((CDbl(r.Item("avgflighttime").ToString) / 60), 1) + " <em>(hrs)</em></td></tr>")
            Else
              htmlOut.Append("<tr><td valign='middle' align='left' class='seperator'>Average Flight Time</td><td valign='top' align='left' class='rightside'0 <em>(hrs)</em></td></tr>")
            End If

            If Not IsDBNull(r("tflighttime")) Then
              htmlOut.Append("<tr><td valign='middle' align='left' class='seperator'>Total Flight Time</td><td valign='top' align='left' class='rightside'>" + FormatNumber((CDbl(r.Item("tflighttime").ToString) / 60), 1) + " <em>(hrs)</em></td></tr>")
            Else
              htmlOut.Append("<tr><td valign='middle' align='left' class='seperator'>Total Flight Time</td><td valign='top' align='left' class='rightside'>0 <em>(hrs)</em></td></tr>")
            End If

            If Not use_faa_data Then
              htmlOut.Append("<tr><td valign='middle' align='center' class='border_bottom_right' colspan='2' width='100%'><b>Powered by ARGUS/TRAQPak</b></td></tr>")
            End If

          Next

        Else
          htmlOut.Append("<thead><tr><th valign='middle' align='left' class='tabheader'><strong>Flight&nbsp;Activity</strong>&nbsp;<em>(last&nbsp;90&nbsp;days)</em></th><th width='40%' class='border_bottom'>&nbsp;</th></tr></thead><tbody>")
          htmlOut.Append("<tr><td valign='middle' align='center' class='border_bottom_right' colspan='2'>No Flight Activity at this time, for this Make/Model ...</td></tr>")
        End If

      Else
        htmlOut.Append("<thead><tr><th valign='middle' align='left' class='tabheader'><strong>Flight&nbsp;Activity</strong>&nbsp;<em>(last&nbsp;90&nbsp;days)</em></th><th width='40%' class='border_bottom'>&nbsp;</th></tr></thead><tbody>")
        htmlOut.Append("<tr><td valign='middle' align='center' class='border_bottom_right' colspan='2'>No Flight Activity at this time, for this Make/Model ...</td></tr>")
      End If
      htmlOut.Append("</tbody>")
      htmlOut.Append("</table>" + vbCrLf)

      If use_faa_data Then

        htmlOut.Append("<br /><div style=""text-align:left; padding-left:3px;"" class=""hideValuePDF""><strong>Flight Data</strong> as of : ")
        'htmlOut.Append("<a class=""underline cursor"" onclick=""javascript:openSmallWindowJS('help/documents/589.pdf','HelpWindow');"">")
        htmlOut.Append("<a class=""underline cursor"" href=""/help/documents/589.pdf"">")
        htmlOut.Append(FormatDateTime(HttpContext.Current.Session.Item("localSubscription").crmSubinst_FAA_data_date.ToString, DateFormat.ShortDate))
        htmlOut.Append("</a></div>" + vbCrLf)
      End If

    Catch ex As Exception

      aError = "Error in views_display_flight_activity(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

    Finally

    End Try

    'return resulting html string
    out_htmlString = htmlOut.ToString
    htmlOut = Nothing
    results_table = Nothing

  End Sub

    Public Sub views_display_flight_utilization_graph(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_scriptString As String, ByRef out_htmlString As String, ByVal graphID As Integer, Optional ByVal faa_date As String = "", Optional ByVal bFromUtilizationTab As Boolean = False, Optional ByVal ValuePDF As Boolean = False, Optional ByVal nonUrlTimespan As String = "")

        Dim htmlOut As New StringBuilder
        Dim scriptOut As New StringBuilder
        Dim results_table As New DataTable

        Dim x As Integer = 0

        Dim sYear As String = ""

        Dim sMonthArray() As String = Split("1,2,3,4,5,6,7,8,9,10,11,12", ",")
        Dim sYearArray() As String = Nothing

        Dim afiltered_Rows As DataRow() = Nothing

        Try

            results_table = get_flight_utilization(searchCriteria, faa_date, bFromUtilizationTab, nonUrlTimespan)

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    If graphID = 32 Then
                        'scriptOut.Append(" alert('drawVisualization" + graphID.ToString + "');" + vbCrLf)  
                        ' scriptOut.Append(" var data32 = new google.visualization.DataTable();" + vbCrLf)

                        scriptOut.Append(" data32.addColumn('number', 'Month');" + vbCrLf)
                    Else
                        scriptOut.Append("function drawVisualization" + graphID.ToString + "() {" + vbCrLf)
                        'scriptOut.Append(" alert('drawVisualization" + graphID.ToString + "');" + vbCrLf)  
                        scriptOut.Append(" var data = new google.visualization.DataTable();" + vbCrLf)

                        scriptOut.Append(" data.addColumn('number', 'Month');" + vbCrLf)
                    End If




                    For Each r As DataRow In results_table.Rows

                        If Not commonEvo.inMyArray(sYear.Split(","), r.Item("tyear").ToString) Then

                            If Month(Date.Now()) = 1 And Trim(r.Item("tyear")) = Trim(Year(Date.Now())) Then  ' then do special stuff
                                ' if we are in january, of this year then skip jan and do tha last 2 years
                            Else
                                If String.IsNullOrEmpty(sYear.Trim) Then
                                    sYear = r.Item("tyear").ToString
                                Else
                                    sYear += "," + r.Item("tyear").ToString
                                End If

                                If graphID = 32 Then
                                    scriptOut.Append(" data32.addColumn('number', 'Flights " + r.Item("tyear").ToString.Trim + "');" + vbCrLf)
                                Else
                                    scriptOut.Append(" data.addColumn('number', 'Flights " + r.Item("tyear").ToString.Trim + "');" + vbCrLf)
                                End If

                            End If


                        End If



                    Next

                    sYearArray = sYear.Split(",")

                    If graphID = 32 Then
                        scriptOut.Append(" data32.addRows([")
                    Else
                        scriptOut.Append(" data.addRows([")
                    End If


                    For Each strMO As String In sMonthArray

                        scriptOut.Append(IIf(CInt(strMO.Trim) > 1, ", [" + strMO.Trim, " [" + strMO.Trim))

                        For Each strYR As String In sYearArray

                            afiltered_Rows = results_table.Select("tmonth = " + strMO.Trim + " AND tyear = " + strYR.Trim, "")

                            If afiltered_Rows.Count > 0 Then

                                For Each r As DataRow In afiltered_Rows

                                    If Not IsDBNull(r.Item("FLIGHTS")) Then
                                        If Not String.IsNullOrEmpty(r.Item("FLIGHTS").ToString.Trim) Then

                                            If CLng(r.Item("FLIGHTS").ToString) > 0 Then
                                                scriptOut.Append("," + r.Item("FLIGHTS").ToString)
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

                        x += 1

                        scriptOut.Append("]")

                    Next


                    If graphID = 32 Then
                    Else


                        scriptOut.Append("]);" + vbCrLf)

                        If graphID = 32 Then
                            scriptOut.Append("var options32 = { " + vbCrLf)
                        Else
                            scriptOut.Append("var options = { " + vbCrLf)
                        End If


                        scriptOut.Append("  chartArea:{width:'80%',height:'75%'}," + vbCrLf)
                        scriptOut.Append("  hAxis: { title: 'Month'," + vbCrLf)
                        scriptOut.Append("           textStyle: { color: '#01579b', fontSize: 14, fontName:  'Arial', bold: true, italic: true }, " + vbCrLf)
                        scriptOut.Append("           titleTextStyle: { color: '#01579b', fontSize: 14, fontName:  'Arial', bold: false, italic: true }" + vbCrLf)
                        scriptOut.Append("         }," + vbCrLf)
                        scriptOut.Append("  vAxis: { title: 'Flights'," + vbCrLf)
                        scriptOut.Append("           textStyle: { color: '#1a237e', fontSize: 14, bold: true }," + vbCrLf)
                        scriptOut.Append("           titleTextStyle: { color: '#1a237e', fontSize: 16, bold: true }" + vbCrLf)
                        scriptOut.Append("        }," + vbCrLf)
                        scriptOut.Append("  smoothLine:true," + vbCrLf)
                        scriptOut.Append("  legend:'top'," + vbCrLf)
                        scriptOut.Append("  colors: ['black','red', 'blue', 'green', 'orange']" + vbCrLf)
                        scriptOut.Append("};" + vbCrLf)


                        If graphID = 32 Then
                            scriptOut.Append(" var chartVis = new google.visualization.LineChart(document.getElementById('visualization" + graphID.ToString + "'));" + vbCrLf)
                            scriptOut.Append(" chartVis.draw(data32, options32);" + vbCrLf)
                        Else
                            scriptOut.Append(" var chartVis = new google.visualization.LineChart(document.getElementById('visualization" + graphID.ToString + "'));" + vbCrLf)
                            scriptOut.Append(" chartVis.draw(data, options);" + vbCrLf)
                        End If


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
                htmlOut.Append("<table id=""flightActivityTable"" width=""100%"" cellspacing=""0"" cellpadding=""4"">")
                htmlOut.Append("<tr><td valign=""top"" align=""left""><div id='visualization" + graphID.ToString + "' style=""height:295px;""></div></td></tr>")
                htmlOut.Append("</table>" + vbCrLf)
            Else
                htmlOut.Append("<table id=""flightActivityTable"" width=""100%"" cellspacing=""0"" cellpadding=""4"">")
                htmlOut.Append("<tr><td valign=""middle"" align=""center"">No Flight Utilization Data at this time, for this Make/Model ...</td></tr>")
                htmlOut.Append("</table>" + vbCrLf)
            End If

        Catch ex As Exception

            aError = "Error in views_display_flight_utilization_graph(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, Optional ByVal faa_date As String = """") " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_scriptString = scriptOut.ToString
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub
    Public Shared Function CombineAssettTables_AFTT(ByRef sold_table As DataTable, ByRef asking_table As DataTable, ByRef est_val_table As DataTable, ByVal AFTTQuery As String) As DataTable

    Try
      CombineAssettTables_AFTT = Nothing

      'Dim column As New DataColumn 'Column to Add Source to jetnet data.
      'Dim column2 As New DataColumn 'Column to Add id to jetnet data. To match client side datatable 
      'Dim column3 As New DataColumn 'Column to add take price to jetnet data (null)
      'Dim column4 As New DataColumn 'Column to add sold price
      'Dim column5 As New DataColumn 'Column to add sold price type
      'Dim column6 As New DataColumn 'Column to add sold price type
      'Dim column7 As New DataColumn 'Column to add sold price type
      'Dim column8 As New DataColumn
      'Dim IDsToExclude As String = ""
      Dim ReturnTable As DataTable

      ReturnTable = est_val_table.Clone
      ReturnTable.AcceptChanges()
      ReturnTable.Constraints.Clear()

      ''Going ahead to add the Source to the Jetnet Datatable, let's you know what type of data you're displaying. 
      'column.DataType = System.Type.GetType("System.String")
      'column.DefaultValue = ""
      'column.Unique = False
      'column.ColumnName = "month_year"
      'ReturnTable.Columns.Add(column)

      'column2.DataType = System.Type.GetType("System.String")
      'column2.DefaultValue = 0
      'column2.Unique = False
      'column2.ColumnName = "type_of"
      'ReturnTable.Columns.Add(column2)

      'column3.DataType = System.Type.GetType("System.Int64")
      'column3.AllowDBNull = True
      'column3.Unique = False
      'column3.ColumnName = "asking_price"
      'ReturnTable.Columns.Add(column3)

      'column4.DataType = System.Type.GetType("System.Int64")
      'column4.DefaultValue = ""
      'column4.Unique = False
      'column4.ColumnName = "sale_price"
      'ReturnTable.Columns.Add(column4)

      If Not IsNothing(sold_table) Then
        If sold_table.Rows.Count > 0 Then
          ReturnTable.Merge(sold_table, True, MissingSchemaAction.Ignore)
        End If
      End If


      If Not IsNothing(asking_table) Then
        If asking_table.Rows.Count > 0 Then
          ReturnTable.Merge(asking_table, True, MissingSchemaAction.Ignore)
        End If
      End If

      If Not IsNothing(est_val_table) Then
        If est_val_table.Rows.Count > 0 Then
          ReturnTable.Merge(est_val_table, True, MissingSchemaAction.Ignore)
        End If
      End If


      ReturnTable.Constraints.Clear()

      For i = 0 To ReturnTable.Columns.Count - 1
        If ReturnTable.Columns(i).DataType.ToString.ToLower = "system.string" Then
          ReturnTable.Columns(i).MaxLength = 1000
        End If
      Next


      'Let's reorder this:
      'Sorting
      Dim Filtered_DV As New DataView(ReturnTable)

      Filtered_DV.Sort = " AFTT desc "

      ReturnTable = Filtered_DV.ToTable

      CombineAssettTables_AFTT = ReturnTable

    Catch ex As Exception
      Return Nothing
    Finally
    End Try

    Return CombineAssettTables_AFTT

  End Function
  Public Shared Function CombineAssettTables_res(ByRef ac_table As DataTable, ByRef ac_dlv_year_table As DataTable) As DataTable

    Try
      CombineAssettTables_res = Nothing

      Dim ReturnTable As DataTable

      ReturnTable = ac_dlv_year_table.Clone
      ReturnTable.AcceptChanges()
      ReturnTable.Constraints.Clear()

      If Not IsNothing(ac_table) Then
        If ac_table.Rows.Count > 0 Then
          ReturnTable.Merge(ac_table, True, MissingSchemaAction.Ignore)
        End If
      End If

      If Not IsNothing(ac_dlv_year_table) Then
        If ac_dlv_year_table.Rows.Count > 0 Then
          ReturnTable.Merge(ac_dlv_year_table, True, MissingSchemaAction.Ignore)
        End If
      End If

      ReturnTable.Constraints.Clear()

      For i = 0 To ReturnTable.Columns.Count - 1
        If ReturnTable.Columns(i).DataType.ToString.ToLower = "system.string" Then
          ReturnTable.Columns(i).MaxLength = 1000
        End If
      Next


      'Let's reorder this:
      'Sorting
      Dim Filtered_DV As New DataView(ReturnTable)

      Filtered_DV.Sort = " year1 asc, month1 asc , ac_year "

      ReturnTable = Filtered_DV.ToTable

      CombineAssettTables_res = ReturnTable


    Catch ex As Exception
      Return Nothing
    Finally
    End Try

    Return CombineAssettTables_res
  End Function
  Public Shared Function CombineAssettTables(ByRef Current_Values As DataTable, ByRef Past_Values As DataTable, ByRef Future_Values As DataTable, ByRef asking_w_sold_table As DataTable, ByVal client_trans_table As DataTable, ByVal jetnet_trans_table As DataTable, ByVal comparable_table As DataTable, ByVal est_value_table As DataTable, ByVal current_ac_values As DataTable) As DataTable

    Try
      CombineAssettTables = Nothing

      'Dim column As New DataColumn 'Column to Add Source to jetnet data.
      'Dim column2 As New DataColumn 'Column to Add id to jetnet data. To match client side datatable 
      'Dim column3 As New DataColumn 'Column to add take price to jetnet data (null)
      'Dim column4 As New DataColumn 'Column to add sold price
      'Dim column5 As New DataColumn 'Column to add sold price type
      'Dim column6 As New DataColumn 'Column to add sold price type
      'Dim column7 As New DataColumn 'Column to add sold price type
      'Dim column8 As New DataColumn
      'Dim IDsToExclude As String = ""
      Dim ReturnTable As DataTable

      ReturnTable = Current_Values.Clone
      ReturnTable.AcceptChanges()
      ReturnTable.Constraints.Clear()

      For i = 0 To ReturnTable.Columns.Count - 1
        If ReturnTable.Columns(i).DataType.ToString.ToLower = "system.string" Then
          ReturnTable.Columns(i).MaxLength = 1000
        End If
      Next


      ''Going ahead to add the Source to the Jetnet Datatable, let's you know what type of data you're displaying. 
      'column.DataType = System.Type.GetType("System.String")
      'column.DefaultValue = ""
      'column.Unique = False
      'column.ColumnName = "month_year"
      'ReturnTable.Columns.Add(column)

      'column2.DataType = System.Type.GetType("System.String")
      'column2.DefaultValue = 0
      'column2.Unique = False
      'column2.ColumnName = "type_of"
      'ReturnTable.Columns.Add(column2)

      'column3.DataType = System.Type.GetType("System.Int64")
      'column3.AllowDBNull = True
      'column3.Unique = False
      'column3.ColumnName = "asking_price"
      'ReturnTable.Columns.Add(column3)

      'column4.DataType = System.Type.GetType("System.Int64")
      'column4.DefaultValue = ""
      'column4.Unique = False
      'column4.ColumnName = "sale_price"
      'ReturnTable.Columns.Add(column4)

      If Not IsNothing(Past_Values) Then
        If Past_Values.Rows.Count > 0 Then
          ReturnTable.Merge(Past_Values, True, MissingSchemaAction.Ignore)
        End If
      End If


      If Not IsNothing(Current_Values) Then
        If Current_Values.Rows.Count > 0 Then
          ReturnTable.Merge(Current_Values, True, MissingSchemaAction.Ignore)
        End If
      End If

      If Not IsNothing(Future_Values) Then
        If Future_Values.Rows.Count > 0 Then
          ReturnTable.Merge(Future_Values, True, MissingSchemaAction.Ignore)
        End If
      End If


      If Not IsNothing(asking_w_sold_table) Then
        If asking_w_sold_table.Rows.Count > 0 Then
          ReturnTable.Merge(asking_w_sold_table, True, MissingSchemaAction.Ignore)
        End If
      End If

      If Not IsNothing(client_trans_table) Then
        If client_trans_table.Rows.Count > 0 Then
          ReturnTable.Merge(client_trans_table, True, MissingSchemaAction.Ignore)
        End If
      End If


      If Not IsNothing(jetnet_trans_table) Then
        If jetnet_trans_table.Rows.Count > 0 Then
          ReturnTable.Merge(jetnet_trans_table, True, MissingSchemaAction.Ignore)
        End If
      End If



      If Not IsNothing(comparable_table) Then
        If comparable_table.Rows.Count > 0 Then
          ReturnTable.Merge(comparable_table, True, MissingSchemaAction.Ignore)
        End If
      End If


      If Not IsNothing(est_value_table) Then
        If est_value_table.Rows.Count > 0 Then
          ReturnTable.Merge(est_value_table, True, MissingSchemaAction.Ignore)
        End If
      End If

      If Not IsNothing(current_ac_values) Then
        If current_ac_values.Rows.Count > 0 Then
          ReturnTable.Merge(current_ac_values, True, MissingSchemaAction.Ignore)
        End If
      End If

      'If Not IsNothing(Past_Values) Then
      '  For Each drJetnet In Past_Values.Rows
      '    ReturnTable.ImportRow(drJetnet)
      '  Next
      'End If


      'If Not IsNothing(Current_Values) Then
      '  For Each drJetnet In Current_Values.Rows
      '    ReturnTable.ImportRow(drJetnet)
      '  Next
      'End If

      'If Not IsNothing(Future_Values) Then
      '  For Each drJetnet In Future_Values.Rows
      '    ReturnTable.ImportRow(drJetnet)
      '  Next
      'End If

      'If Not IsNothing(asking_w_sold_table) Then
      '  For Each drJetnet In asking_w_sold_table.Rows
      '    ReturnTable.ImportRow(drJetnet)
      '  Next
      'End If

      'If Not IsNothing(client_trans_table) Then
      '  For Each drJetnet In client_trans_table.Rows
      '    ReturnTable.ImportRow(drJetnet)
      '  Next
      'End If


      'If Not IsNothing(jetnet_trans_table) Then
      '  For Each drJetnet In jetnet_trans_table.Rows
      '    ReturnTable.ImportRow(drJetnet)
      '  Next
      'End If

      'If Not IsNothing(comparable_table) Then
      '  For Each drJetnet In comparable_table.Rows
      '    ReturnTable.ImportRow(drJetnet)
      '  Next
      'End If

      'If Not IsNothing(est_value_table) Then
      '  For Each drJetnet In est_value_table.Rows
      '    ReturnTable.ImportRow(drJetnet)
      '  Next
      'End If


      ReturnTable.Constraints.Clear()

      For i = 0 To ReturnTable.Columns.Count - 1
        If ReturnTable.Columns(i).DataType.ToString.ToLower = "system.string" Then
          ReturnTable.Columns(i).MaxLength = 1000
        End If
      Next


      'Let's reorder this:
      'Sorting
      Dim Filtered_DV As New DataView(ReturnTable)

      Filtered_DV.Sort = "year1 asc, month1 asc "

      ReturnTable = Filtered_DV.ToTable

      CombineAssettTables = ReturnTable


    Catch ex As Exception
      Return Nothing
    Finally
    End Try
    Return CombineAssettTables

  End Function

  Public Function GetAircraftCurrentMarket(ByVal amod_id As Long, ByVal idList As String, ByVal order_by_string As String, Optional ByVal YearOne As String = "", Optional ByVal YearTwo As String = "", Optional ByVal forsaleFlag As String = "", Optional ByVal regType As String = "", Optional ByVal afttStart As String = "", Optional ByVal afttEnd As String = "", Optional ByRef variantList As String = "") As DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim TempTable As New DataTable
    Dim query As String
    Try

      If amod_id > 0 Then


        'Opening Connection
        SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString
        SqlConn.Open()

        query = "SELECT ac_id,ac_ser_no, ac_ser_no_full, ac_ser_no_sort, ac_reg_no, ac_year, ac_mfr_year, ac_forsale_flag,"
        query += " ac_asking, ac_asking_price,"
        query += "   (select top 1 afmv_value from Aircraft_FMV with (NOLOCK) where afmv_ac_id = ac_id and afmv_status='Y' and afmv_latest_flag='Y') as eValue  "

        query += " , (select top 1 avg(afmv_value) from Aircraft_FMV with (NOLOCK)"
        query += "   inner join Aircraft a2 with (NOLOCK) on a2.ac_id = afmv_ac_id and a2.ac_amod_id = " & amod_id & " and a2.ac_mfr_year = Aircraft_Flat.ac_mfr_year "
        query += "   where   afmv_status='Y' and afmv_latest_flag='Y') as eValue_ModelAVG  "

        'If FeaturesList <> "" Then
        '  query += FeaturesList & ","
        'End If

        'If HttpContext.Current.Session.Item("localPreferences").UserSPIViewFlag Then
        '  query += " (select top 1 ac_sale_price From Aircraft b with (NOLOCK)"
        '  query += " inner join Journal with (NOLOCK) on journ_id = ac_journ_id and ac_id = journ_ac_id"
        '  query += " where(a.ac_id = b.ac_id)"
        '  query += " and ac_sale_price > 0  and ac_sale_price_display_flag = 'Y' and journ_subcat_code_part1='WS' AND journ_internal_trans_flag='N' "
        '  query += " and journ_subcat_code_part3 NOT IN ('DB','DS','FI','FY','IT','MF','RE','CC','LS', 'RM')"
        '  query += " order by journ_date desc) as  LASTSALEPRICE,"
        'Else
        '  query += " NULL as LASTSALEPRICE, "
        'End If

        'If HttpContext.Current.Session.Item("localPreferences").UserSPIViewFlag Then
        '  query += " (select top 1 journ_date From Aircraft b with (NOLOCK)"
        '  query += " inner join Journal with (NOLOCK) on journ_id = ac_journ_id and ac_id = journ_ac_id"
        '  query += " where a.ac_id = b.ac_id and ac_sale_price > 0 and ac_sale_price_display_flag = 'Y' and journ_subcat_code_part1='WS'"
        '  query += " AND journ_internal_trans_flag='N' "
        '  query += " and journ_subcat_code_part3 NOT IN ('DB','DS','FI','FY','IT','MF','RE','CC','LS', 'RM')"
        '  query += " order by journ_date desc) as  LASTSALEPRICEDATE,"
        'Else
        '  query += " NULL as LASTSALEPRICEDATE, "
        'End If

        'query += " (select top 1 comp_name from Company with (NOLOCK)  inner join Aircraft_Reference with (NOLOCK) on cref_comp_id = comp_id and cref_journ_id = comp_journ_id where cref_ac_id = a.ac_id and a.ac_journ_id = cref_journ_id and cref_contact_type in ('00','08','17')) as ACOwner,"
        'query += " case amp_program_name when 'Unknown' then '' when 'Confirmed to be on a maintenance program' then 'Confirmed' when 'Confirmed not on any maintenance program' then 'Confirmed Not' else amp_program_name end as APROG,"
        'query += " case emp_program_name when 'Unknown' then '' when 'Confirmed to be on a maintenance program' then 'Confirmed' when 'Confirmed not on any maintenance program' then 'Confirmed Not' else emp_program_name end as EPROG, "
        'query += " ac_airframe_tot_hrs, ac_est_airframe_hrs, amod_airframe_type_code, amod_type_code, amod_weight_class, amod_model_name, amod_make_name, "
        'query += " ac_engine_1_tot_hrs, "
        'query += " ac_engine_2_tot_hrs, ac_engine_3_tot_hrs, ac_engine_4_tot_hrs, ac_interior_moyear, ac_exterior_moyear, ac_list_date, ac_status, "
        'query += " ac_passenger_count, ac_journ_id,"
        'query += " ac_previously_owned_flag, ac_lease_flag, ac_maintained"

        query += " from Aircraft_Flat  with (NOLOCK)"
        ' query += " LEFT outer join Aircraft_Features_Flat on a.ac_id = afeat_ac_id and a.ac_journ_id = afeat_journ_id"

        query += " WHERE ac_journ_id = 0 "

        '-- YEAR RANGE
        If Not String.IsNullOrEmpty(YearOne) And Not String.IsNullOrEmpty(YearTwo) Then
          If YearOne > 0 And YearTwo > 0 Then
            query += " and ac_mfr_year between @yearOne and @yearTwo "
          End If
        End If

        If forsaleFlag = "Y" Then
          query += " and ac_forsale_flag = 'Y' "
        End If

        'reg Type
        If Not String.IsNullOrEmpty(regType) Then
          If regType = "N" Then
            query += " and ac_reg_no like 'N%' "
          ElseIf regType = "I" Then
            query += " and ac_reg_no not like 'N%' "
          End If
        End If


        If Not String.IsNullOrEmpty(afttStart) And Not String.IsNullOrEmpty(afttEnd) Then
          '-- AFTT
          query += " and ac_airframe_tot_hrs between @startAFTT and @endAFTT "
        End If


                'If idList <> "" Then
                '  query += " and ac_id in (" & idList & ") "
                '     'End If

                query += clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(HttpContext.Current.Session.Item("localSubscription"), False, True)

        ' If loadAll = False Then
        query += " and ac_forsale_flag ='Y' "
        'End If

        query += " and ac_lifecycle_stage=3"

        If Not String.IsNullOrEmpty(variantList) Then
          query += " and amod_id in (" & amod_id.ToString & "," & variantList & ")"
        Else
          query += " and amod_id = @amodID"
        End If

        If Trim(order_by_string) = "" Or Trim(order_by_string) = "Serial Number" Then
          query += " ORDER BY ac_ser_no_sort"
        ElseIf Trim(order_by_string) = "Year" Then
          query += " ORDER BY ac_year asc, ac_ser_no_sort "
        ElseIf Trim(order_by_string) = "Serial Number" Then
          query += " ORDER BY ac_ser_no_sort"
        Else
          query += " ORDER BY ac_ser_no_sort"
        End If


        clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, query.ToString)

        Dim SqlCommand As New SqlClient.SqlCommand(query, SqlConn)
        SqlCommand.Parameters.AddWithValue("amodID", amod_id)
        SqlCommand.Parameters.AddWithValue("yearOne", YearOne)
        SqlCommand.Parameters.AddWithValue("yearTwo", YearTwo)
        SqlCommand.Parameters.AddWithValue("startAFTT", afttStart)
        SqlCommand.Parameters.AddWithValue("endAFTT", afttEnd)
        SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

        Try
          TempTable.Load(SqlReader)
        Catch constrExc As System.Data.ConstraintException
          Dim rowsErr As System.Data.DataRow() = TempTable.GetErrors()
        End Try

        SqlCommand.Dispose()
        SqlCommand = Nothing
      End If



      Return TempTable
    Catch ex As Exception
      GetAircraftCurrentMarket = Nothing
      'Me.class_error = "Error in GetAircraftStartingTable(ByVal amod_id As Long) As DataTable SQL VERSION: " & ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing


    End Try

  End Function

    Public Sub FillAssettInsightGraphs(ByVal graph_type As String, ByVal ModelID As Long, ByRef label_string As String, ByVal parentContainer As Object, ByVal graphID As Integer, ByVal ac_id As Long, ByVal client_ac_id As Long, ByVal div_height As Long, ByVal ac_dlv_year As Long, Optional ByVal show_asking As Boolean = True, Optional ByVal show_sale As Boolean = True, Optional ByVal show_evalues As Boolean = True, Optional ByVal order_by_text As String = "", Optional ByVal from_pdf As String = "N", Optional ByRef table_string As String = "", Optional ByRef count_of_records_visible As String = "", Optional ByRef ticks_string_to_return As String = "", Optional ByVal YearOne As String = "", Optional ByVal YearTwo As String = "", Optional ByVal forsaleFlag As String = "", Optional ByVal regType As String = "", Optional ByVal afttStart As String = "", Optional ByVal afttEnd As String = "", Optional ByRef google_map_array_list As String = "", Optional ByVal VariantList As String = "", Optional ByRef miniGraph As Boolean = False, Optional ByVal CheckForDOMLoad As Boolean = True, Optional ByRef has_info As Boolean = False)

        Dim htmlUtilizationGraph As String = ""
        Dim htmlUtilizationGraphScript As String = ""
        Dim htmlUtilizationFunctionScript As String = ""

        Dim htmlOut As New StringBuilder

        Dim utilization_functions As New utilization_view_functions
        Dim searchCriteria As New viewSelectionCriteriaClass

        searchCriteria.ViewCriteriaAmodID = ModelID

        'utilization_functions.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
        'utilization_functions.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
        'utilization_functions.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
        'utilization_functions.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
        'utilization_functions.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim 



        If Trim(graph_type) = "CURRENTMARKET" Then
            views_display_current_market_and_assett_prices_graph(searchCriteria, htmlUtilizationFunctionScript, htmlUtilizationGraph, graphID, "", False, False, div_height, ac_dlv_year, show_asking, show_sale, show_evalues, order_by_text, from_pdf, table_string, ticks_string_to_return, VariantList, YearOne, YearTwo, forsaleFlag, regType, afttStart, afttEnd, miniGraph)
        ElseIf Trim(graph_type) = "DLVYEAR" Then
            If Not IsNothing(HttpContext.Current.Request.Item("USA")) Then
                If HttpContext.Current.Request.Item("USA") = "Y" Then
                    views_display_assett_prices_graph_US_Foreign(searchCriteria, htmlUtilizationFunctionScript, htmlUtilizationGraph, graphID, "", False, False, div_height, ac_dlv_year, show_asking, show_sale, show_evalues, from_pdf, ticks_string_to_return)
                End If
            Else
                views_display_assett_prices_graph(searchCriteria, htmlUtilizationFunctionScript, htmlUtilizationGraph, graphID, "", False, False, div_height, ac_dlv_year, show_asking, show_sale, show_evalues, from_pdf, ticks_string_to_return, YearOne, YearTwo, forsaleFlag, regType, afttStart, afttEnd, VariantList, miniGraph)
            End If
        ElseIf Trim(graph_type) = "ASKSOLD" Then
            searchCriteria.ViewCriteriaAircraftID = ac_id
            views_display_asking_with_sold_over_assett_prices_graph(searchCriteria, htmlUtilizationFunctionScript, htmlUtilizationGraph, graphID, "", False, False, client_ac_id, div_height, from_pdf, ticks_string_to_return, YearOne, YearTwo, forsaleFlag, regType, afttStart, afttEnd, VariantList, miniGraph, has_info)
            If Trim(table_string) = "MODEL" Then
                htmlUtilizationFunctionScript = Replace(htmlUtilizationFunctionScript, "chartArea:{width:'85", "chartArea:{width:'78")
                htmlUtilizationFunctionScript = Replace(htmlUtilizationFunctionScript, "height:'78", "height:'68")
            End If

        ElseIf Trim(graph_type) = "RESIDUAL" Then
                views_display_assett_residual_graph(searchCriteria, htmlUtilizationFunctionScript, htmlUtilizationGraph, graphID, "", False, False, div_height, ac_dlv_year, show_asking, show_sale, show_evalues, from_pdf, count_of_records_visible, ticks_string_to_return, Trim(graph_type), YearOne, YearTwo, forsaleFlag, regType, afttStart, afttEnd, VariantList, miniGraph)
            ElseIf Trim(graph_type) = "RESIDUALAC" Then
                searchCriteria.ViewCriteriaAircraftID = ac_id


                Dim aclsData_Temp As New clsData_Manager_SQL
                aclsData_Temp.JETNET_DB = HttpContext.Current.Session.Item("jetnetClientDatabase")
                aclsData_Temp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase")

                Dim ModelTable2 As New DataTable
                If client_ac_id > 0 Then
                    ModelTable2 = CommonAircraftFunctions.BuildReusableTable(client_ac_id, 0, "CLIENT", "", aclsData_Temp, True, 0, "CLIENT")
                Else
                    ModelTable2 = aclsData_Temp.Get_JETNET_Aircraft_Model_amodID(ModelID)
                End If

                If Not IsNothing(ModelTable2) Then
                    If ModelTable2.Rows.Count > 0 Then
                        searchCriteria.ViewCriteriaAircraftMake = ModelTable2.Rows(0).Item("amod_make_name")
                        searchCriteria.ViewCriteriaAircraftModel = ModelTable2.Rows(0).Item("amod_model_name")
                    End If
                End If

                views_display_assett_residual_graph(searchCriteria, htmlUtilizationFunctionScript, htmlUtilizationGraph, graphID, "", False, False, div_height, ac_dlv_year, show_asking, show_sale, show_evalues, from_pdf, count_of_records_visible, ticks_string_to_return, Trim(graph_type), YearOne, YearTwo, forsaleFlag, regType, afttStart, afttEnd, "", False, has_info)
            ElseIf Trim(graph_type) = "AFTT" Then
                DisplayAFTTTable(searchCriteria, htmlUtilizationFunctionScript, htmlUtilizationGraph, graphID, "", False, False, div_height, ac_dlv_year, show_asking, show_sale, show_evalues, from_pdf, count_of_records_visible, ticks_string_to_return, ac_id, YearOne, YearTwo, forsaleFlag, regType, afttStart, afttEnd, miniGraph)
        End If

        If Trim(from_pdf) = "Y" Then
            label_string += Trim(htmlUtilizationFunctionScript)
        ElseIf Trim(from_pdf) = "A" Then
            label_string += Trim(htmlUtilizationGraph)
        Else
            If Not IsNothing(htmlUtilizationFunctionScript) Then
                If Not String.IsNullOrEmpty(htmlUtilizationFunctionScript.Trim) Then

                    htmlUtilizationGraphScript = vbCrLf + "<script type=""text/javascript"">" + vbCrLf
                    If CheckForDOMLoad Then
                        htmlUtilizationGraphScript += "$(document).ready(function(){" + vbCrLf
                    End If

                    htmlUtilizationGraphScript += " google.charts.setOnLoadCallback(function() { drawVisualization" + graphID.ToString + "();});" + vbCrLf
                    If CheckForDOMLoad Then
                        htmlUtilizationGraphScript += "});" + vbCrLf
                    End If
                    htmlUtilizationGraphScript += htmlUtilizationFunctionScript.Trim
                    htmlUtilizationGraphScript += "</script>" + vbCrLf


                    System.Web.UI.ScriptManager.RegisterStartupScript(parentContainer, parentContainer.GetType(), "showUtilizationGraph" + graphID.ToString, htmlUtilizationGraphScript, False)

                End If
            End If


            label_string += htmlUtilizationGraph.ToString
        End If


        google_map_array_list = htmlUtilizationFunctionScript
        ' assett_label.Text = htmlOut.ToString

        If Trim(graph_type) = "ASKSOLD" Then
            If Right(Trim(google_map_array_list), 84) = ", null, null, null,null, null, null, null, null, null, null, null, null, null, null]" Then
                google_map_array_list = ""
            End If
        End If

        ' utilization.Visible = True

    End Sub

    Public Sub views_display_asking_with_sold_over_assett_prices_graph(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_scriptString As String, ByRef out_htmlString As String, ByVal graphID As Integer, Optional ByVal faa_date As String = "", Optional ByVal bFromUtilizationTab As Boolean = False, Optional ByVal ValuePDF As Boolean = False, Optional ByVal client_ac_id As Long = 0, Optional ByVal div_height As Integer = 295, Optional ByVal from_pdf As String = "", Optional ByRef ticks_string_to_return As String = "", Optional ByVal YearOne As String = "", Optional ByVal YearTwo As String = "", Optional ByVal forsaleFlag As String = "", Optional ByVal regType As String = "", Optional ByVal afttStart As String = "", Optional ByVal afttEnd As String = "", Optional ByVal VariantList As String = "", Optional ByVal MiniGraph As Boolean = False, Optional ByRef has_info As Boolean = False)

    Dim htmlOut As New StringBuilder
    Dim scriptOut As New StringBuilder
    Dim asking_w_sold_table As New DataTable
    Dim current_month_table As New DataTable
    Dim past_months_table As New DataTable
    Dim projections_table As New DataTable

    Dim x As Integer = 0
    Dim color As String = ""
    Dim sYear As String = ""
    Dim row_added As Boolean = False


    Dim afiltered_Rows As DataRow() = Nothing
    Dim temp_low As String = ""
    Dim temp_avg As String = ""
    Dim temp_high As String = ""
    Dim temp_low_proj As String = ""
    Dim temp_avg_proj As String = ""
    Dim temp_high_proj As String = ""
    Dim temp_data As String = ""
    Dim avg_asking As String = ""
    Dim avg_sale As String = ""
    Dim avg_take As String = ""
    Dim high_number As Long = 0
    Dim low_number As Long = 1000000
    Dim first_date As String = ""
    Dim last_date As String = ""
    Dim horizontal_tick_string As String = ""
    Dim starting_point As Integer = 0
    Dim interval_point As Integer = 1
    Dim ending_point As Integer = 0
    Dim ticks_string As String = ""
    Dim results_table As New DataTable
    Dim temp_type As String = ""
    Dim is_current As Boolean = False
    Dim has_current_sold As Boolean = False
    Dim has_current_val As Boolean = False
    Dim last_asking As String = "null"
    Dim last_sale As String = "null"
    Dim last_take As String = "null"
    Dim localDataLayer As New viewsDataLayer
    Dim temp_trans_ids As String = ""
    Dim transaction_table As New DataTable
    Dim client_transaction_table As New DataTable
    Dim comparables_table As New DataTable
    Dim estimated_values_table As New DataTable
    Dim date_of As String = ""
    Dim has_data As Boolean = False
    Dim date_of_orig As String = ""
    Dim html_table As New StringBuilder
    Dim inner_string As New StringBuilder
    Dim ac_results_table As New DataTable
    Dim date_of_string As String = ""
    Dim style_text As String = ""
    Dim info_found As Boolean = False


    Try

      asking_w_sold_table = get_jetnet_asking_w_sold(searchCriteria, YearOne, YearTwo, forsaleFlag, regType, afttStart, afttEnd, VariantList)

      current_month_table = get_current_month_assett_summary(searchCriteria, 0, YearOne, YearTwo, forsaleFlag, regType, afttStart, afttEnd, VariantList)

      If Not IsNothing(current_month_table) Then
        If current_month_table.Rows.Count > 1 Then
          current_month_table.Clear()
          current_month_table = get_current_month_assett_summary(searchCriteria, 0, YearOne, YearTwo, forsaleFlag, regType, afttStart, afttEnd, VariantList, "Y")
        End If
      End If

      past_months_table = get_past_month_assett_summary(searchCriteria, YearOne, YearTwo, forsaleFlag, regType, afttStart, afttEnd, VariantList)

      'projections_table = get_residual_assett_summary(searchCriteria)

      If searchCriteria.ViewCriteriaAircraftID > 0 Then
        localDataLayer.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
        If client_ac_id = 0 And searchCriteria.ViewCriteriaAircraftID > 0 Then
          ac_results_table = localDataLayer.get_ac_details_jetnet_record(searchCriteria.ViewCriteriaAircraftID, "Y")
        Else
          ac_results_table = localDataLayer.get_my_ac_value_history_comparables(client_ac_id, "Current", True, 0, 0, "O", True, 0, "", "Y")      ' get current ac primary info
        End If

        client_transaction_table = localDataLayer.get_my_ac_value_history_comparables(client_ac_id, "Trans", True, searchCriteria.ViewCriteriaAircraftID, 0, "O", True, searchCriteria.ViewCriteriaAmodID, "", "Y")        ' get transactions for current ac
        'comparables_table = localDataLayer.get_my_ac_value_history_comparables(client_ac_id, "Comparable", True, searchCriteria.ViewCriteriaAircraftID, 0, "O", True, searchCriteria.ViewCriteriaAmodID, "", "Y")        ' get other sold ac that were comparables
        estimated_values_table = localDataLayer.get_my_ac_value_history_comparables(client_ac_id, "est_value", True, searchCriteria.ViewCriteriaAircraftID, 0, "O", True, searchCriteria.ViewCriteriaAmodID, "", "Y")

        temp_trans_ids = localDataLayer.get_journ_ids_from_client_trans(searchCriteria.ViewCriteriaAircraftID, client_ac_id)
        transaction_table = localDataLayer.get_ac_trans_not_in_client_trans(searchCriteria.ViewCriteriaAircraftID, temp_trans_ids, "Y")
      End If

      results_table = CombineAssettTables(current_month_table, past_months_table, projections_table, asking_w_sold_table, client_transaction_table, transaction_table, comparables_table, estimated_values_table, ac_results_table)



      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          If Trim(from_pdf) = "Y" Or Trim(from_pdf) = "A" Then
          Else
            scriptOut.Append("function drawVisualization" + graphID.ToString + "() {" + vbCrLf)
            'scriptOut.Append(" alert('drawVisualization" + graphID.ToString + "');" + vbCrLf)  
            scriptOut.Append(" var data" + graphID.ToString + " = new google.visualization.DataTable();" + vbCrLf)
          End If

          '  If searchCriteria.ViewCriteriaAircraftID > 0 Then
          'scriptOut.Append("data" + graphID.ToString + ".addColumn('date', 'Month/Year'); ")
          '  Else
          scriptOut.Append("data" + graphID.ToString + ".addColumn('string', 'Month/Year'); ")
          '  End If

          If searchCriteria.ViewCriteriaAircraftID > 0 Then
            scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', '" & value_label & "'); ")
            scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', '" & value_label & "'); ")
            scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', '" & value_label & "'); ")
            scriptOut.Append(" data" + graphID.ToString + ".addColumn({""type"":""string"", role:""style""}); ")
            scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', 'Residual " & value_label & "'); ")
            scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', 'Residual " & value_label & "'); ")
            scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', 'Residual " & value_label & "'); ")
            scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', 'Asking'); ")
            scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', 'Take'); ")
            scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', 'Sale'); ")
            scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', 'Current Asking'); ")
            scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', 'Current Take'); ")
            scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', 'Current Est'); ")
            scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', 'Client Estimates'); ")
          Else
            scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', 'Low " & value_label & "'); ")
            scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', 'Avg " & value_label & "'); ")
            scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', 'High " & value_label & "'); ")
            scriptOut.Append(" data" + graphID.ToString + ".addColumn({""type"":""string"", role:""style""}); ")
            scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', 'Low Residual " & value_label & "'); ")
            scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', 'Avg Residual " & value_label & "'); ")
            scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', 'High Residual " & value_label & "'); ")
            'scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', 'ACAskingValues'); ")
            'scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', 'ACTakeValues'); ")
            'scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', 'ACEstValues'); ")
            scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', 'Avg Asking'); ")
            scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', 'Avg Take'); ")
            scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', 'Avg Sale'); ")
            scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', 'Asking'); ")
            scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', 'Take'); ")
            scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', 'Est Value'); ")
            scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', 'Client Estimates'); ")
          End If


          scriptOut.Append(" data" + graphID.ToString + ".addRows([")



          For Each r As DataRow In results_table.Rows

            temp_data = "null"
            temp_low = "null"
            temp_avg = "null"
            temp_high = "null"
            temp_low_proj = "null"
            temp_avg_proj = "null"
            temp_high_proj = "null"
            avg_asking = "null"
            avg_sale = "null"
            avg_take = "null"
            temp_type = ""
            is_current = False
            has_data = False
            style_text = "null"

            ' If Not IsDBNull(r("amod_make_name")) And Not IsDBNull(r("amod_model_name")) Then 
            '   temp_data = r.Item("amod_make_name") & " " & r("amod_model_name")  
            ' Else
            '   temp_data = ""
            ' End If
            If Not IsDBNull(r("type_of")) Then
              temp_type = r.Item("type_of")
            Else
              temp_type = ""
            End If



            If Not IsDBNull(r("month1")) Then
              temp_data = r.Item("month1") & "/" & r.Item("year1")

              If Trim(r.Item("month1")) = Trim(Date.Now.Month) And Trim(r.Item("year1")) = Trim(Date.Now.Year) Then
                is_current = True

                If Trim(temp_type) = "asking_w_sold" Then
                  has_current_sold = True
                ElseIf Trim(temp_type) = "current_month" Then
                  has_current_val = True
                End If
              End If
            Else
              temp_data = "null"
            End If

            If searchCriteria.ViewCriteriaAircraftID > 0 And searchCriteria.ViewCriteriaAmodID = 0 Then
              If Not IsDBNull(r("date1")) Then
                date_of = r.Item("date1")
                date_of = FormatDateTime(date_of, DateFormat.ShortDate)
                date_of = Replace(date_of, Year(CDate(date_of)), Right(Trim(Year(CDate(date_of))), 2))
                date_of = "'" & date_of & "'"

                'commented out for now 
                ' date_of_orig = date_of
                ' date_of = "new Date(" & Year(CDate(date_of)) & ", " & (Month(CDate(date_of)) - 1) & ", " & Day(CDate(date_of)) & ")"
              Else
                date_of = ""
              End If
            Else
              date_of = "'" & temp_data & "'"
            End If




            If Trim(temp_type) = "future_month" Then

              If Not IsDBNull(r("LOWVALUE")) Then
                temp_low_proj = r.Item("LOWVALUE")
                If CLng(temp_low_proj) > 0 Then
                  temp_low_proj = CLng(temp_low_proj / 1000)
                  Call check_high_low(temp_low_proj, high_number, low_number)
                  has_data = True
                Else
                  temp_low_proj = "null"
                End If
              Else
                temp_low_proj = "null"
              End If

              If Not IsDBNull(r("AVGVALUE")) Then
                temp_avg_proj = r.Item("AVGVALUE")
                If CLng(temp_avg_proj) > 0 Then
                  temp_avg_proj = CLng(temp_avg_proj / 1000)
                  Call check_high_low(temp_avg_proj, high_number, low_number)
                  has_data = True
                Else
                  temp_avg_proj = "null"
                End If
              Else
                temp_avg_proj = "null"
              End If

              If Not IsDBNull(r("HIGHVALUE")) Then
                temp_high_proj = r.Item("HIGHVALUE")
                If CLng(temp_high_proj) > 0 Then
                  temp_high_proj = CLng(temp_high_proj / 1000)
                  Call check_high_low(temp_high_proj, high_number, low_number)
                  has_data = True
                Else
                  temp_high_proj = "null"
                End If
              Else
                temp_high_proj = "null"
              End If

            Else

              If Not IsDBNull(r("LOWVALUE")) Then
                temp_low = r.Item("LOWVALUE")
                If CLng(temp_low) > 0 Then
                  temp_low = CLng(temp_low / 1000)
                  Call check_high_low(temp_low, high_number, low_number)
                  has_data = True
                Else
                  temp_low = "null"
                End If
              Else
                temp_low = "null"
              End If

              If Not IsDBNull(r("AVGVALUE")) Then
                temp_avg = r.Item("AVGVALUE")
                If CLng(temp_avg) > 0 Then
                  temp_avg = CLng(temp_avg / 1000)
                  Call check_high_low(temp_avg, high_number, low_number)
                  has_data = True
                Else
                  temp_avg = "null"
                End If
              Else
                temp_avg = "null"
              End If

              If Not IsDBNull(r("HIGHVALUE")) Then
                temp_high = r.Item("HIGHVALUE")
                If CLng(temp_high) > 0 Then
                  temp_high = CLng(temp_high / 1000)
                  Call check_high_low(temp_high, high_number, low_number)
                  has_data = True
                Else
                  temp_high = "null"
                End If
              Else
                temp_high = "null"
              End If

            End If



            If Not IsDBNull(r("asking_price")) Then
              avg_asking = r.Item("asking_price")
              If CLng(avg_asking) > 0 Then
                avg_asking = CLng(avg_asking / 1000)
                Call check_high_low(avg_asking, high_number, low_number)
                has_data = True
                last_asking = avg_asking
              Else
                avg_asking = "null"
              End If
            Else
              avg_asking = "null"
            End If


            If Not IsDBNull(r("take_price")) Then
              avg_take = r.Item("take_price")
              If CLng(avg_take) > 0 Then
                avg_take = CLng(avg_take / 1000)
                Call check_high_low(avg_take, high_number, low_number)
                has_data = True
                last_take = avg_take
              Else
                avg_take = "null"
              End If
            Else
              avg_take = "null"
            End If

            If Not IsDBNull(r("sale_price")) Then
              avg_sale = r.Item("sale_price")
              If CLng(avg_sale) > 0 Then
                avg_sale = CLng(avg_sale / 1000)
                Call check_high_low(avg_sale, high_number, low_number)
                has_data = True
                last_sale = avg_sale
              Else
                avg_sale = "null"
              End If
            Else
              avg_sale = "null"
            End If


            ' added in so it doesnt count blank comparables or snapshots that were added 
            If searchCriteria.ViewCriteriaAircraftID > 0 Then
              If Trim(date_of_orig) <> "" And has_data = True Then
                Call commonEvo.set_ranges_dates(date_of_orig, first_date, last_date)
              End If
            End If


            inner_string.Append("<tr class='" & color & "' valign='top'><td align='left'><font size='-2' style='font-family: Arial'>")
            inner_string.Append(Replace(date_of, "'", ""))
            inner_string.Append("&nbsp;</font></td><td align='left'><font size='-2' style='font-family: Arial'>")

            If searchCriteria.ViewCriteriaAircraftID > 0 Then
              If Not IsDBNull(r("descrip")) Then
                If Trim(r("descrip")) <> "" Then
                  If Trim(temp_type) = "CLIENTESTVAL" And Len(Trim(r("descrip"))) = 1 Then
                    If Trim(r("descrip")) = "B" Then
                      inner_string.Append("Blue Book (")
                    ElseIf Trim(r("descrip")) = "V" Then
                      inner_string.Append("VREF (")
                    ElseIf Trim(r("descrip")) = "H" Then
                      inner_string.Append("HeliValue$ (")
                    ElseIf Trim(r("descrip")) = "F" Then
                      inner_string.Append("Full Appraisal (")
                    ElseIf Trim(r("descrip")) = "D" Then
                      inner_string.Append("Desktop Appraisal (")
                    End If
                  Else
                    inner_string.Append(Trim(r("descrip")) & " (")
                  End If
                Else
                  inner_string.Append(temp_type & " (")
                End If
              Else
                inner_string.Append(temp_type & " (")
              End If
              info_found = False
              If Trim(temp_type) = "asking_w_sold" Then
                If Trim(temp_low) <> "null" Then
                  inner_string.Append("Asking: $" & FormatNumber(temp_low, 0) & "k")
                  info_found = True
                End If
                If Trim(temp_avg) <> "null" Then
                  If Trim(temp_avg) <> "null" Then
                    inner_string.Append(", ")
                  End If
                  inner_string.Append("Take: $" & FormatNumber(temp_avg, 0) & "k")
                  info_found = True
                End If
                If Trim(temp_high) <> "null" Then
                  If Trim(temp_low) <> "null" Or Trim(temp_avg) <> "null" Then
                    inner_string.Append(", ")
                  End If
                  inner_string.Append("Sale: $" & FormatNumber(temp_high, 0) & "k")
                  info_found = True
                End If
              ElseIf Trim(temp_type) = "future_month" Then
                If Trim(temp_low) <> "null" Then
                  inner_string.Append("Asking: $" & FormatNumber(temp_low_proj, 0) & "k")
                  info_found = True
                End If
                If Trim(temp_avg_proj) <> "null" Then
                  If Trim(temp_avg_proj) <> "null" Then
                    inner_string.Append(", ")
                  End If
                  inner_string.Append("Take: $" & FormatNumber(temp_avg_proj, 0) & "k")
                  info_found = True
                End If
                If Trim(temp_high_proj) <> "null" Then
                  If Trim(temp_avg_proj) <> "null" Or Trim(temp_high_proj) <> "null" Then
                    inner_string.Append(", ")
                  End If
                  inner_string.Append("Sale: $" & FormatNumber(temp_high_proj, 0) & "k")
                End If
              ElseIf Trim(temp_type) = "current_month" Then
                If Trim(temp_low) <> "null" Then
                  inner_string.Append("$" & FormatNumber(temp_low, 0) & "k")
                  info_found = True
                End If


              ElseIf Trim(temp_type) = "past_month" Then
                If Trim(temp_low) <> "null" Then
                  inner_string.Append("$" & FormatNumber(temp_low, 0) & "k")
                  info_found = True
                End If

              ElseIf Trim(temp_type) = "AC_VALUES" Then
                If Trim(avg_asking) <> "null" Then
                  inner_string.Append("Asking: $" & FormatNumber(avg_asking, 0) & "k")
                  info_found = True
                End If
                If Trim(avg_take) <> "null" Then
                  If Trim(avg_asking) <> "null" Then
                    inner_string.Append(", ")
                  End If
                  inner_string.Append("Take: $" & FormatNumber(avg_take, 0) & "k")
                  info_found = True
                End If
                If Trim(avg_sale) <> "null" Then
                  If Trim(avg_take) <> "null" Or Trim(avg_asking) <> "null" Then
                    inner_string.Append(", ")
                  End If
                  inner_string.Append("Est Value: $" & FormatNumber(avg_sale, 0) & "k")
                  info_found = True
                End If
              ElseIf Trim(temp_type) = "CLIENTTRANS" Then
                If Trim(temp_low) <> "null" Then
                  inner_string.Append("Asking: $" & FormatNumber(temp_low, 0) & "k")
                  info_found = True
                End If
                If Trim(temp_avg) <> "null" Then
                  If Trim(temp_low) <> "null" Then
                    inner_string.Append(", ")
                  End If
                  inner_string.Append("Take: $" & FormatNumber(temp_avg, 0) & "k")
                  info_found = True
                End If
                If Trim(temp_high) <> "null" Then
                  If Trim(temp_low) <> "null" Or Trim(temp_avg) <> "null" Then
                    inner_string.Append(", ")
                  End If
                  inner_string.Append("Sale: $" & FormatNumber(temp_high, 0) & "k")
                  info_found = True
                End If
              ElseIf Trim(temp_type) = "CCOMPARE" Then
                If Trim(avg_asking) <> "null" Then
                  inner_string.Append("Asking: $" & FormatNumber(avg_asking, 0) & "k")
                  info_found = True
                End If
                If Trim(avg_take) <> "null" Then
                  If Trim(avg_asking) <> "null" Then
                    inner_string.Append(", ")
                  End If
                  inner_string.Append("Take: $" & FormatNumber(avg_take, 0) & "k")
                  info_found = True
                End If
                If Trim(avg_sale) <> "null" Then
                  If Trim(avg_take) <> "null" Or Trim(avg_asking) <> "null" Then
                    inner_string.Append(", ")
                  End If
                  inner_string.Append("Sale: $" & FormatNumber(avg_sale, 0) & "k")
                  info_found = True
                End If
              ElseIf Trim(temp_type) = "CLIENTESTVAL" Then
                If Trim(avg_sale) <> "null" Then
                  inner_string.Append("Estimated Value: $" & FormatNumber(avg_sale, 0) & "k")
                  info_found = True
                End If
              ElseIf Trim(temp_type) = "JETNETTRANS" Then
                If Trim(avg_asking) <> "null" Then
                  inner_string.Append("Asking: $" & FormatNumber(avg_asking, 0) & "k")
                  info_found = True
                End If
                If Trim(avg_take) <> "null" Then
                  If Trim(avg_asking) <> "null" Then
                    inner_string.Append(", ")
                  End If
                  inner_string.Append("Take: $" & FormatNumber(avg_take, 0) & "k")
                  info_found = True
                End If
                If Trim(avg_sale) <> "null" Then
                  If Trim(avg_take) <> "null" Or Trim(avg_asking) <> "null" Then
                    inner_string.Append(", ")
                  End If
                  inner_string.Append("Sale: $" & FormatNumber(avg_sale, 0) & "k")
                  info_found = True
                End If
              Else
                temp_type = temp_type
              End If

              If info_found = False Then
                inner_string.Append("No Values Found")
              End If

              inner_string.Append(")</font></td>")
            End If
            inner_string.Append("</tr>")



            If info_found = True Then 
              If html_table.Length < 1 Then
                html_table.Append("")
                html_table.Append("<table border='1' cellpadding='3' cellspacing='0' class='engine'>")
                html_table.Append("<tr class='dark_blue'><td align='left'><font size='-2' style='font-family: Arial'><b>Date</b></td><td align='left'><font size='-2' style='font-family: Arial'><b>Description</b></td></tr>")
              End If

              html_table.Append(inner_string.ToString)   ' else dont add it 
            End If
            inner_string.Length = 0

            'html_table.Append("<tr><td align='left'>" & temp_type & "</td>")
            'html_table.Append("<td align='left'>" & date_of_orig & "</td>")
            'html_table.Append("<td align='left'>" & temp_low & "</td>")
            'html_table.Append("<td align='left'>" & temp_avg & "</td>")
            'html_table.Append("<td align='left'>" & temp_high & "</td>")
            'html_table.Append("<td align='left'>" & temp_low_proj & "</td>")
            'html_table.Append("<td align='left'>" & temp_avg_proj & "</td>")
            'html_table.Append("<td align='left'>" & temp_high_proj & "</td>")
            'html_table.Append("<td align='left'>" & avg_asking & "</td>")
            'html_table.Append("<td align='left'>" & avg_take & "</td>")
            'html_table.Append("<td align='left'>" & avg_sale & "</td>")
            'html_table.Append("<td align='left'>" & temp_avg & "</td>")
            'html_table.Append("</tr>")




            If searchCriteria.ViewCriteriaAircraftID > 0 Then
              If Trim(temp_type) = "current_month" Then
                style_text = ""
                style_text = " 'point {size: 5;}'"
                'style_text = " point {Size: 10;}"
                'style_text = " 'point {stroke-width: 10;}'"
                'style_text = " point {stroke-width: 10;}"
                'style_text = " 'point {color: green;}'"
                'style_text = " point {color: green;}"
              End If
            End If

            ' AC CHART CURRENTLY, POSSIBLY LATER OTHERS, WILL NOT WORRY AS WELL 
            If searchCriteria.ViewCriteriaAircraftID > 0 Then

              If Trim(temp_type) = "AC_VALUES" Then
                If row_added Then
                  scriptOut.Append(",[" & date_of & ", " & temp_low & ", " & temp_avg & ", " & temp_high & "," & style_text & ", " & temp_low_proj & ", " & temp_avg_proj & ", " & temp_high_proj & ", null, null, null, " & avg_asking & ", " & avg_take & ", " & avg_sale & ", null]")
                Else
                  scriptOut.Append("[" & date_of & ", " & temp_low & ", " & temp_avg & ", " & temp_high & "," & style_text & ", " & temp_low_proj & ", " & temp_avg_proj & ", " & temp_high_proj & ", null, null, null, " & avg_asking & ", " & avg_take & ", " & avg_sale & ", null]")
                End If
                'ElseIf Trim(temp_type) = "current_month" Then
                '  If row_added Then
                '    scriptOut.Append(",[" & date_of & ", null, null, null, " & temp_low_proj & ", " & temp_avg_proj & ", " & temp_high_proj & ", null,null, null, " & temp_high & ", null, null, null," & style_text & "]")
                '  Else
                '    scriptOut.Append("[" & date_of & ", null, null, null, " & temp_low_proj & ", " & temp_avg_proj & ", " & temp_high_proj & ",null, null, null, " & temp_high & ", null, null, null," & style_text & "]")
                '  End If
              ElseIf Trim(temp_type) = "CLIENTESTVAL" Then
                If row_added Then
                  scriptOut.Append(",[" & date_of & ", null, null, null,null,null, null,null, null,null, null, null, null, null, " & avg_sale & "]")
                Else
                  scriptOut.Append("[" & date_of & ", null, null, null,null,null, null,null, null,null, null, null, null, null, " & avg_sale & "]")
                End If
              Else
                If row_added Then
                  scriptOut.Append(",[" & date_of & ", " & temp_low & ", " & temp_avg & ", " & temp_high & "," & style_text & ", " & temp_low_proj & ", " & temp_avg_proj & ", " & temp_high_proj & ", " & avg_asking & ", " & avg_take & ", " & avg_sale & ", null, null, null, null]")
                Else
                  scriptOut.Append("[" & date_of & ", " & temp_low & ", " & temp_avg & ", " & temp_high & "," & style_text & ", " & temp_low_proj & ", " & temp_avg_proj & ", " & temp_high_proj & ", " & avg_asking & ", " & avg_take & ", " & avg_sale & ", null, null, null, null]")
                End If
              End If
              row_added = True
              ' commented out all of this for now .... 
            ElseIf has_current_sold = True And has_current_val = True Then
              If row_added Then
                scriptOut.Append(",[" & date_of & ", null, null, null," & style_text & ", " & temp_low_proj & ", " & temp_avg_proj & ", " & temp_high_proj & ", " & last_asking & ", " & last_take & ", " & last_sale & "," & temp_avg & ", null, null, null]")
              Else
                scriptOut.Append("[" & date_of & ", null, null, null," & style_text & ", " & temp_low_proj & ", " & temp_avg_proj & ", " & temp_high_proj & ", " & last_asking & ", " & last_take & ", " & last_sale & "," & temp_avg & ", null, null, null]")
              End If
              has_current_sold = False
              has_current_val = False
              row_added = True
            ElseIf has_current_sold = False And has_current_val = True Then   'we are no longer seperating past and current - 
              'If row_added Then
              '  scriptOut.Append(",[" & date_of & ", null, null, null," & style_text & ", " & temp_low_proj & ", " & temp_avg_proj & ", " & temp_high_proj & ", null, null, null, null, null, null, null]")
              'Else
              '  scriptOut.Append("[" & date_of & ", null, null, null," & style_text & ", " & temp_low_proj & ", " & temp_avg_proj & ", " & temp_high_proj & ", null, null, null, null, null, null, null]")  ' was 3 from end " & temp_avg & "
              'End If
              If row_added Then
                scriptOut.Append(",[" & date_of & ", " & temp_low & ", " & temp_avg & ", " & temp_high & "," & style_text & ", " & temp_low_proj & ", " & temp_avg_proj & ", " & temp_high_proj & ", " & avg_asking & ", " & avg_take & ", " & avg_sale & ", null, null, null, null]")
              Else
                scriptOut.Append("[" & date_of & ", " & temp_low & ", " & temp_avg & ", " & temp_high & "," & style_text & ", " & temp_low_proj & ", " & temp_avg_proj & ", " & temp_high_proj & ", " & avg_asking & ", " & avg_take & ", " & avg_sale & ", null, null, null, null]")
              End If
              has_current_sold = False
              has_current_val = False
              row_added = True
            ElseIf has_current_sold = True Then
              has_current_sold = has_current_sold
              ' then dont add it this time, see if there is another record 
              'if has_current_val = True then we it will just call into the else statement 
            ElseIf row_added Then
              scriptOut.Append(",[" & date_of & ", " & temp_low & ", " & temp_avg & ", " & temp_high & "," & style_text & ", " & temp_low_proj & ", " & temp_avg_proj & ", " & temp_high_proj & ", " & avg_asking & ", " & avg_take & ", " & avg_sale & ", null, null, null, null]")
              has_current_val = False
              has_current_sold = False
              row_added = True
            Else
              scriptOut.Append("[" & date_of & ", " & temp_low & ", " & temp_avg & ", " & temp_high & "," & style_text & ", " & temp_low_proj & ", " & temp_avg_proj & ", " & temp_high_proj & ", " & avg_asking & ", " & avg_take & ", " & avg_sale & ", null, null, null, null]")
              has_current_val = False
              has_current_sold = False
              row_added = True
            End If

            If Trim(date_of_string) <> "" Then
              date_of_string &= ", '" & date_of_orig & "'"
            Else
              date_of_string &= "'" & date_of_orig & "'"
            End If

          Next

          If info_found = True Then
            If html_table.Length < 1 Then
              html_table.Append("")
              html_table.Append("<table border='1' cellpadding='3' cellspacing='0' class='engine'>")
              html_table.Append("<tr class='dark_blue'><td align='left'><font size='-2' style='font-family: Arial'><b>Date</b></td><td align='left'><font size='-2' style='font-family: Arial'><b>Description</b></td></tr>")
            End If
          End If

          html_table.Append("</table>")

          ticks_string = "Y" ' so that is does the extra build
          commonEvo.set_ranges_for_vsCharts(low_number, high_number, interval_point, starting_point, ending_point, ticks_string)

          horizontal_tick_string = ""
          '  If searchCriteria.ViewCriteriaAircraftID > 0 And Trim(first_date) <> "" Then
          ' commonEvo.make_ticks_string(first_date, last_date, horizontal_tick_string)
          '' commonEvo.make_ticks_string2(first_date, last_date, horizontal_tick_string, date_of_string)   ' added tikcs back in above to date_of_string
          ' End If

          If Trim(from_pdf) = "Y" Or Trim(from_pdf) = "A" Then
          Else


            scriptOut.Append("]);" + vbCrLf)

            scriptOut.Append("var options = { " + vbCrLf)
            scriptOut.Append("  chartArea:{width:'" & IIf(MiniGraph, "80", "85") & "%',height:'" & IIf(MiniGraph, "68", "78") & "%'}," + vbCrLf)
            scriptOut.Append("series: { ")

            'scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', '" & value_label & "'); ")
            'scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', '" & value_label & "'); ")
            'scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', '" & value_label & "'); ")
            'scriptOut.Append(" data" + graphID.ToString + ".addColumn({""type"":""string"", role:""style""}); ")
            'scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', 'Residual " & value_label & "'); ")
            'scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', 'Residual " & value_label & "'); ")
            'scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', 'Residual " & value_label & "'); ")
            'scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', 'Avg Asking'); ")
            'scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', 'Avg Take'); ")
            'scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', 'Avg Sale'); ")
            'scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', 'Asking'); ")
            'scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', 'Take'); ")
            'scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', 'Est'); ")

            If searchCriteria.ViewCriteriaAircraftID > 0 Then
              scriptOut.Append("    0: { lineWidth: 3, pointSize: 3 , lineDashStyle: [4, 4] } ")
              scriptOut.Append(" ,  1: { lineWidth: 3, pointSize: 3 , lineDashStyle: [4, 4], visibleInLegend: false } ")
              scriptOut.Append(" ,  2: { lineWidth: 3, pointSize: 3 , lineDashStyle: [4, 4], visibleInLegend: false } ")
              scriptOut.Append(" ,  3: { lineWidth: 5, pointSize: 5 , visibleInLegend: false  } ")
              scriptOut.Append(" ,  4: { lineWidth: 3, pointSize: 3, lineDashStyle: [4, 4], visibleInLegend: false } ")
              scriptOut.Append(" ,  5: { lineWidth: 3, pointSize: 3 , lineDashStyle: [4, 4], visibleInLegend: false } ")
              scriptOut.Append(" ,  6: { lineWidth: 3, pointSize: 3 , lineDashStyle: [4, 4]} ") 

              If HttpContext.Current.Session.Item("localUser").crmUser_Evo_MPM_Flag = True Then
                scriptOut.Append(" ,  7: { lineWidth: 3, pointSize: 3  } ")
                scriptOut.Append(" ,  8: { lineWidth: 3, pointSize: 3  } ")
                scriptOut.Append(" ,  9: { lineWidth: 3, pointSize: 3  } ")
                scriptOut.Append(" ,  10: { lineWidth: 5, pointSize: 5  } ")
                scriptOut.Append(" ,  11: { lineWidth: 5, pointSize: 5  } ")
                scriptOut.Append(" ,  12: { lineWidth: 5, pointSize: 5  } ")
                scriptOut.Append(" ,  13: { lineWidth: 5, pointSize: 5  } ")
              Else
                scriptOut.Append(" ,  7: { lineWidth: 3, pointSize: 3  } ")
                scriptOut.Append(" ,  8: { lineWidth: 3, pointSize: 3 , visibleInLegend: false } ")
                scriptOut.Append(" ,  9: { lineWidth: 3, pointSize: 3  } ")
                scriptOut.Append(" ,  10: { lineWidth: 5, pointSize: 5  } ")
                scriptOut.Append(" ,  11: { lineWidth: 5, pointSize: 5 , visibleInLegend: false } ")
                scriptOut.Append(" ,  12: { lineWidth: 5, pointSize: 5  } ")
                scriptOut.Append(" ,  13: { lineWidth: 5, pointSize: 5 , visibleInLegend: false } ")
              End If
 


            Else
              scriptOut.Append("    0: { lineWidth: 2, pointSize: 2 , lineDashStyle: [4, 4] } ")
              scriptOut.Append(" ,  1: { lineWidth: 3, pointSize: 3 , lineDashStyle: [4, 4] } ")
              scriptOut.Append(" ,  2: { lineWidth: 2, pointSize: 2 , lineDashStyle: [4, 4] } ")
              scriptOut.Append(" ,  3: { lineWidth: 5, pointSize: 5, visibleInLegend: false  } ")
              scriptOut.Append(" ,  4: { lineWidth: 2, pointSize: 2, lineDashStyle: [4, 4], visibleInLegend: false } ")
              scriptOut.Append(" ,  5: { lineWidth: 3, pointSize: 3 , lineDashStyle: [4, 4], visibleInLegend: false } ")
              scriptOut.Append(" ,  6: { lineWidth: 2, pointSize: 2 , lineDashStyle: [4, 4] } ")
              scriptOut.Append(" ,  7: { lineWidth: 2, pointSize: 2  } ")
              scriptOut.Append(" ,  8: { lineWidth: 2, pointSize: 2  } ")
              scriptOut.Append(" ,  9: { lineWidth: 2, pointSize: 2  } ")
              scriptOut.Append(" ,  10: { lineWidth: 5, pointSize: 5, visibleInLegend: false  } ")
              scriptOut.Append(" ,  11: { lineWidth: 5, pointSize: 5, visibleInLegend: false  } ")
            End If

            scriptOut.Append("  }  , ")

            If div_height > 295 Then
              scriptOut.Append("  hAxis: { title: 'Month/Year'," + vbCrLf)
              scriptOut.Append("           textStyle: { fontSize: " & IIf(MiniGraph, "8", "10") & ", italic: false}, ")
              scriptOut.Append("           titleTextStyle: { fontSize: " & IIf(MiniGraph, "8", "15") & ", italic: false} ")
              '  scriptOut.Append("           textStyle: { color: '#01579b', fontSize: 10, fontName:  'Arial', bold: true, italic: true }, " + vbCrLf)
              '  scriptOut.Append("           titleTextStyle: { color: '#01579b', fontSize: 15, fontName:  'Arial', bold: false, italic: true }" + vbCrLf)
              If Trim(horizontal_tick_string) <> "" Then
                scriptOut.Append(", ticks: [ " & horizontal_tick_string & "] ")
              End If
              scriptOut.Append("         }," + vbCrLf)
              scriptOut.Append("  vAxis: { title: 'Price ($k)'," + vbCrLf)
              scriptOut.Append("           textStyle: { fontSize: " & IIf(MiniGraph, "8", "10") & ", italic: false}, ")
              scriptOut.Append("           titleTextStyle: { fontSize: " & IIf(MiniGraph, "8", "15") & ", italic: false} ")
              'scriptOut.Append("           textStyle: { color: '#1a237e', fontSize: 10, bold: true }," + vbCrLf)
              ' scriptOut.Append("           titleTextStyle: { color: '#1a237e', fontSize: 15, bold: true }" + vbCrLf)
              If Trim(ticks_string) <> "" Then
                scriptOut.Append(", ticks: [ " & ticks_string & "] ")
              End If
              scriptOut.Append("        }," + vbCrLf)
              scriptOut.Append("  smoothLine:true," + vbCrLf)
              scriptOut.Append(" legend: { position: 'top', textStyle:{fontSize:" & IIf(MiniGraph, "8", "9") & "}}, " + vbCrLf)
              scriptOut.Append("  colors: ['" & value_color & "','" & value_color & "', '" & value_color & "', '#B7B7B7', '#B7B7B7', '#B7B7B7', '#a3c28d', '#eba059', '#a84543', '#a3c28d', '#eba059', '#a84543', 'purple']" + vbCrLf)    ' , '" & value_color & "'
              scriptOut.Append("};" + vbCrLf)
            Else
              scriptOut.Append("  hAxis: { title: 'Month/Year'," + vbCrLf)
              scriptOut.Append("           textStyle: {  fontSize: 8, bold: false, italic: false}, " + vbCrLf)
              scriptOut.Append("           titleTextStyle: { fontSize: " & IIf(MiniGraph, "8", "15") & ", bold: false , italic: false}" + vbCrLf)
              If Trim(horizontal_tick_string) <> "" Then
                scriptOut.Append(", ticks: [ " & horizontal_tick_string & "] ")
              End If
              scriptOut.Append("         }," + vbCrLf)
              scriptOut.Append("  vAxis: { title: 'Price ($k)'," + vbCrLf)
              scriptOut.Append("           textStyle: {  fontSize: " & IIf(MiniGraph, "8", "10") & ", bold: false , italic: false}," + vbCrLf)
              scriptOut.Append("           titleTextStyle: { fontSize: " & IIf(MiniGraph, "8", "15") & ", bold: false , italic: false }" + vbCrLf)
              If Trim(ticks_string) <> "" Then
                scriptOut.Append(", ticks: [ " & ticks_string & "] ")
              End If
              scriptOut.Append("        }," + vbCrLf)
              scriptOut.Append("  smoothLine:true," + vbCrLf)
              scriptOut.Append(" legend: { position: 'top', textStyle:{fontSize:8}}, " + vbCrLf)
              scriptOut.Append("  colors: ['" & value_color & "','" & value_color & "', '" & value_color & "', '#B7B7B7', '#B7B7B7', '#B7B7B7', '#a3c28d', '#eba059', '#a84543', '#a3c28d', '#eba059', '#a84543']" + vbCrLf)    ', '" & value_color & "'
              scriptOut.Append("};" + vbCrLf)
            End If


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
        htmlOut.Append("<table id=""flightActivityTable"" width=""100%"" cellspacing=""0"" cellpadding=""4"">")
        htmlOut.Append("<tr><td valign=""top"" align=""left""><div id='visualization" + graphID.ToString + "' style=""height:" & div_height.ToString & "px;""></div></td></tr>")
        '  htmlOut.Append("<tr><td>" & html_table.ToString & "</td></tr>")
        htmlOut.Append("</table>" + vbCrLf)
      Else
        htmlOut.Append("<table id=""flightActivityTable"" width=""100%"" cellspacing=""0"" cellpadding=""4"">")
        htmlOut.Append("<tr><td valign=""middle"" align=""center"">No eValue estimates at this time for this make and model....</td></tr>")
        htmlOut.Append("</table>" + vbCrLf)
      End If

    Catch ex As Exception

      aError = "Error in views_display_flight_utilization_graph(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, Optional ByVal faa_date As String = """") " + ex.Message

    Finally

    End Try

    'return resulting html string
    out_scriptString = scriptOut.ToString
    If Trim(from_pdf) = "A" Then
      out_htmlString = html_table.ToString
    Else
      out_htmlString = htmlOut.ToString
    End If

    has_info = info_found

    ticks_string_to_return = ticks_string
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Sub views_display_assett_residual_graph(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_scriptString As String, ByRef out_htmlString As String, ByVal graphID As Integer, Optional ByVal faa_date As String = "", Optional ByVal bFromUtilizationTab As Boolean = False, Optional ByVal ValuePDF As Boolean = False, Optional ByVal div_height As Integer = 295, Optional ByVal ac_dlv_year As Integer = 0, Optional ByVal show_asking As Boolean = True, Optional ByVal show_sale As Boolean = True, Optional ByVal show_evaules As Boolean = True, Optional ByVal from_pdf As String = "", Optional ByRef count_of_records_visible As String = "", Optional ByRef ticks_string_to_return As String = "", Optional ByVal graph_type As String = "", Optional ByVal YearOne As String = "", Optional ByVal YearTwo As String = "", Optional ByVal forsaleFlag As String = "", Optional ByVal regType As String = "", Optional ByVal afttStart As String = "", Optional ByVal afttEnd As String = "", Optional ByVal VariantList As String = "", Optional ByVal miniGraph As Boolean = False, Optional ByRef has_info As Boolean = False)

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


    Try

      If Trim(graph_type) = "RESIDUAL" Then
        results_table = get_residual_assett_summary_by_Year_MFR(searchCriteria, "N", 0, YearOne, YearTwo, forsaleFlag, regType, afttStart, afttEnd, VariantList)
      ElseIf Trim(graph_type) = "RESIDUALAC" Then
        results_table = get_residual_assett_summary_by_Year_MFR(searchCriteria, "N", 0)
        results_table2 = get_residual_assett_summary_by_Year_MFR(searchCriteria, "Y", ac_dlv_year)

        results_table = CombineAssettTables_res(results_table, results_table2)
      End If




      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

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
              If Not IsDBNull(r("ac_year")) Then
                temp_data = r.Item("ac_year")
              Else
                temp_data = "null"
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
                finish_script(scriptOut, current_point, null_max)
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
                Call check_high_low(temp_avg, high_number, low_number)
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


            ' 50 minus (0+1) = 49 
            ' 50 minus (1+1) = 48
            '  post_count_nulls = ending_point - (current_point + 1)

            ' pre_count_nulls = current_point

            'For i = 0 To current_point
            'scriptOut.Append(", null ")
            'Next

            'For i = post_count_nulls To ending_point
            '
            '   Next


            scriptOut.Append("," & temp_avg & "")
            '  scriptOut.Append(", " & temp_low & ", " & temp_avg & ", " & temp_high & "")


            row_added = True
          Next

          count_of_records_visible = current_rows

          horizontal_tick_string = ""
          ' commonEvo.make_ticks_string(first_date, last_date, horizontal_tick_string) 

          finish_script(scriptOut, current_point, null_max) ' also ends ]

          current_rows = current_rows + 1
          finish_column_script(column_scriptOut, current_rows, null_max, graphID)




          ticks_string = "Y" ' so that is does the extra build
          commonEvo.set_ranges_for_vsCharts(low_number, high_number, interval_point, starting_point, ending_point, ticks_string)


          If Trim(from_pdf) = "Y" Then
          Else
            scriptOut.Append("]);" + vbCrLf)

            scriptOut.Append("var options = { " + vbCrLf)
                        scriptOut.Append("  chartArea:{width:'" & IIf(miniGraph, "80", "85") & "%',height:'" & IIf(miniGraph, "58", "80") & "%'}," + vbCrLf)


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

            If Trim(graph_type) = "RESIDUALAC" Then

              scriptOut.Append("  colors: ['" & value_color & "','" & grey_color & "'],")
            Else

            End If
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
        htmlOut.Append("<table id=""flightActivityTable"" width=""100%"" cellspacing=""0"" cellpadding=""4"">")
        htmlOut.Append("<tr><td valign=""top"" align=""left""><div id='visualization" + graphID.ToString + "' style=""height:" & div_height.ToString & "px;""></div></td></tr>")
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
  Public Function GetValuesTrendsByAFTT(ByVal amod_id As Long, ByVal forsaleFlag As String, ByVal yearOne As String, ByVal yearTwo As String, ByVal afttStart As String, ByVal afttEnd As String, ByVal regType As String, ByVal Startdate As String, ByVal EndDate As String, ByVal variantListString As String, ByVal AFTTQuery As String) As DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim TempTable As New DataTable
    Dim query As String
    Try

      If amod_id <> 0 Then

        TempTable.Columns.Add("AFTT")
        TempTable.Columns.Add("AFTTSORT", System.Type.GetType("System.Int64"))
        TempTable.Columns.Add("TYPE_OF")
        TempTable.Columns.Add("SALECOUNT")
        TempTable.Columns.Add("LOWASKING")
        TempTable.Columns.Add("AVGASKING")
        TempTable.Columns.Add("HIGHASKING")
        TempTable.Columns.Add("LOWSALE")
        TempTable.Columns.Add("AVGSALE")
        TempTable.Columns.Add("HIGHSALE")
        TempTable.Columns.Add("COUNTASKING")
        TempTable.Columns.Add("SUMASKING")
                TempTable.Columns.Add("COUNTSALE")
                TempTable.Columns.Add("SUMSALE")


        'Opening Connection
        SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString
        SqlConn.Open()

        query = "SELECT "


        AFTTQuery = Replace(AFTTQuery, " ac_airframe_tot_hrs ", " ac_est_airframe_hrs ")

        query += AFTTQuery & " as AFTT"
        query += " , 'Sold' as type_of, "
        query += " count(distinct journ_id) as SALECOUNT,"
        query += " 0 as LOWASKING, "
        query += " 0 AS AVGASKING,"
        query += " 0 as HIGHASKING,"
        query += " MIN(ac_sale_price) AS LOWSALE,"
        query += " AVG(ac_sale_price) AS AVGSALE,"
        query += " max(ac_sale_price) AS HIGHSALE,"
        query += " 0 as COUNTASKING, "
        query += " 0 as SUMASKING, "
        query += " SUM(case when ac_sale_price > 0 then 1 else 0 end) as COUNTSALE, "
        query += " SUM(ac_sale_price) as SUMSALE "
        query += ", '' as  AC_HRS "

        If HttpContext.Current.Session.Item("localPreferences").UserSPIViewFlag = True Then
          query += ", 0 as LOWVALUE "
          query += ", 0 as AVGVALUE "
          query += ", 0 as HIGHVALUE "
        End If

        query += " From Aircraft_Flat a with (NOLOCK) "
        query += " inner join Journal with (NOLOCK) on journ_id = ac_journ_id and ac_id = journ_ac_id "
        query += " where journ_subcat_code_part1='WS' AND journ_internal_trans_flag='N' "
        query += " and journ_subcat_code_part3 NOT IN ('DB','DS','FI','FY','IT','MF','RE','CC','LS', 'RM') "
        query += " and ac_est_airframe_hrs > 0  and journ_date >= (GETDATE() - 365) "

        If Not String.IsNullOrEmpty(variantListString) Then
          query += " and amod_id in (" & amod_id & "," & variantListString & ")"
        Else
          query += " and amod_id = @amodID"
        End If


        '-- YEAR RANGE
        If Not String.IsNullOrEmpty(yearOne) And Not String.IsNullOrEmpty(yearTwo) Then
          If yearOne > 0 And yearTwo > 0 Then
            query += (" and ac_year between @yearOne and @yearTwo")
          End If
        End If


        If forsaleFlag = "Y" Then
          query += "  and ac_forsale_flag = 'Y' "
        End If

        'reg Type
        If Not String.IsNullOrEmpty(regType) Then
          If regType = "N" Then
            query += "  and ac_reg_no like 'N%' "
          ElseIf regType = "I" Then
            query += "  and ac_reg_no not like 'N%' "
          End If
        End If


        If Not String.IsNullOrEmpty(afttStart) And Not String.IsNullOrEmpty(afttEnd) Then
          '-- AFTT
          query += "  and ac_est_airframe_hrs between @startAFTT and @endAFTT"
        End If

                query += add_client_ac_string(False)

                query += " group by amod_id, "
        query += "( " & AFTTQuery & " )"
        query += " order by "
        query += " cast( replace( ( " & AFTTQuery & " ), ' - ', '') as float) desc "


        clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, query.ToString)

        Dim SqlCommand As New SqlClient.SqlCommand(query, SqlConn)
        SqlCommand.Parameters.AddWithValue("amodID", amod_id)
        SqlCommand.Parameters.AddWithValue("yearOne", yearOne)
        SqlCommand.Parameters.AddWithValue("yearTwo", yearTwo)
        ' SqlCommand.Parameters.AddWithValue("StartDate", Startdate)
        '  SqlCommand.Parameters.AddWithValue("EndDate", EndDate)
        SqlCommand.Parameters.AddWithValue("startAFTT", afttStart)
        SqlCommand.Parameters.AddWithValue("endAFTT", afttEnd)
        SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

        Try
          TempTable.Load(SqlReader)
        Catch constrExc As System.Data.ConstraintException
          Dim rowsErr As System.Data.DataRow() = TempTable.GetErrors()
        End Try

        SqlCommand.Dispose()
        SqlCommand = Nothing
        '  While SqlReader.Read()

        '    Dim newRow As DataRow = TempTable.NewRow()
        '    newRow("AFTT") = SqlReader.Item("AFTT")

        '    Dim SortAFTT As Long = 0
        '    If Not IsDBNull(SqlReader.Item("AFTT")) Then
        '      Dim SortStringAFTTArray As Array = Split(SqlReader.Item("AFTT"), "-")
        '      If UBound(SortStringAFTTArray) = 1 Then
        '        If IsNumeric(Trim(SortStringAFTTArray(0))) Then
        '          SortAFTT = Trim(SortStringAFTTArray(0))
        '        End If
        '      End If
        '    End If

        '    newRow("AFTTSORT") = SortAFTT
        '    newRow("SALECOUNT") = SqlReader.Item("SALECOUNT")
        '    newRow("LOWASKING") = SqlReader.Item("LOWASKING")
        '    newRow("AVGASKING") = SqlReader.Item("AVGASKING")
        '    newRow("HIGHASKING") = SqlReader.Item("HIGHASKING")
        '    newRow("LOWSALE") = SqlReader.Item("LOWSALE")
        '    newRow("AVGSALE") = SqlReader.Item("AVGSALE")
        '    newRow("HIGHSALE") = SqlReader.Item("HIGHSALE")
        '    newRow("COUNTASKING") = SqlReader.Item("COUNTASKING")
        '    newRow("SUMASKING") = SqlReader.Item("SUMASKING")
        '    newRow("COUNTSALE") = SqlReader.Item("COUNTSALE")
        '    newRow("SUMSALE") = SqlReader.Item("SUMSALE")

        '    TempTable.Rows.Add(newRow)
        '    TempTable.AcceptChanges()
        '  End While

        '  SqlCommand.Dispose()
        '  SqlCommand = Nothing
      End If

      'Dim SortView As New DataView(TempTable)
      'SortView.Sort = "AFTTSORT desc"
      ' TempTable = SortView.ToTable


    Catch ex As Exception
      GetValuesTrendsByAFTT = Nothing
      'Me.class_error = "Error in GetAircraftStartingTable(ByVal amod_id As Long) As DataTable SQL VERSION: " & ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

    End Try
    Return TempTable
  End Function
  Public Function GetValuesTrendsByAFTT_Asknig(ByVal amod_id As Long, ByVal forsaleFlag As String, ByVal yearOne As String, ByVal yearTwo As String, ByVal afttStart As String, ByVal afttEnd As String, ByVal regType As String, ByVal Startdate As String, ByVal EndDate As String, ByVal variantListString As String, ByVal AFTTQuery As String) As DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim TempTable As New DataTable
    Dim query As String
    Try

      If amod_id <> 0 Then

        TempTable.Columns.Add("AFTT")
        TempTable.Columns.Add("AFTTSORT", System.Type.GetType("System.Int64"))
        TempTable.Columns.Add("type_of")
        TempTable.Columns.Add("SALECOUNT")
        TempTable.Columns.Add("LOWASKING")
        TempTable.Columns.Add("AVGASKING")
        TempTable.Columns.Add("HIGHASKING")
        TempTable.Columns.Add("LOWSALE")
        TempTable.Columns.Add("AVGSALE")
        TempTable.Columns.Add("HIGHSALE")
        TempTable.Columns.Add("COUNTASKING")
        TempTable.Columns.Add("SUMASKING")
        TempTable.Columns.Add("COUNTSALE")
        TempTable.Columns.Add("SUMSALE")


        'Opening Connection
        SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString
        SqlConn.Open()

        query = "SELECT "


        query += AFTTQuery & " as AFTT"
        query += " , 'Asking' as type_of, "
        query += " 0 as SALECOUNT,"
        query += " avg(ac_asking_price) as LOWASKING, "
        query += " avg(ac_asking_price) AS AVGASKING,"
        query += " avg(ac_asking_price) as HIGHASKING,"
        query += " 0 AS LOWSALE,"
        query += " 0 AS AVGSALE,"
                query += " 0 AS HIGHSALE,"

                query += " sum(ac_asking_price) as SUMASKING , "
                query += " sum(case when ac_asking_price > 0 then 1 else 0 end) as COUNTASKING, "
                ' replaced these 
                ' query += " count(*) as COUNTASKING, "
                ' query += " sum(case when ac_asking_price > 0 then 1 else 0 end) as SUMASKING, "
                query += " 0 as COUNTSALE, "
        query += " 0 as SUMSALE "
        query += ", '' as  AC_HRS "

        If HttpContext.Current.Session.Item("localPreferences").UserSPIViewFlag = True Then
          query += ", 0 as LOWVALUE "
          query += ", 0 as AVGVALUE "
          query += ", 0 as HIGHVALUE "
        End If

        query += "  From Aircraft_Flat with (NOLOCK) "
        query += " where ac_journ_id = 0 and ac_forsale_flag='Y'"

        If Not String.IsNullOrEmpty(variantListString) Then
          query += " and amod_id in (" & amod_id & "," & variantListString & ")"
        Else
          query += " and amod_id = @amodID"
        End If


        '-- YEAR RANGE
        If Not String.IsNullOrEmpty(yearOne) And Not String.IsNullOrEmpty(yearTwo) Then
          If yearOne > 0 And yearTwo > 0 Then
            query += (" and ac_year between @yearOne and @yearTwo")
          End If
        End If


        If forsaleFlag = "Y" Then
          query += " and ac_forsale_flag = 'Y' "
        End If

        'reg Type
        If Not String.IsNullOrEmpty(regType) Then
          If regType = "N" Then
            query += " and ac_reg_no like 'N%' "
          ElseIf regType = "I" Then
            query += " and ac_reg_no not like 'N%' "
          End If
        End If


                If Not String.IsNullOrEmpty(afttStart) And Not String.IsNullOrEmpty(afttEnd) Then
                    '-- AFTT
                    query += " and ac_airframe_tot_hrs between @startAFTT and @endAFTT"
                End If

                ' add in MSW -------------
                query += add_client_ac_string(False)


                query += " group by   "
        query += "( " & AFTTQuery & " )"
        query += " order by "
        query += " cast( replace( ( " & AFTTQuery & " ), ' - ', '') as float) desc "


        clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, query.ToString)

        Dim SqlCommand As New SqlClient.SqlCommand(query, SqlConn)
        SqlCommand.Parameters.AddWithValue("amodID", amod_id)
        SqlCommand.Parameters.AddWithValue("yearOne", yearOne)
        SqlCommand.Parameters.AddWithValue("yearTwo", yearTwo)
        SqlCommand.Parameters.AddWithValue("startAFTT", afttStart)
        SqlCommand.Parameters.AddWithValue("endAFTT", afttEnd)


        SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
 
        Try
          TempTable.Load(SqlReader)
        Catch constrExc As System.Data.ConstraintException
          Dim rowsErr As System.Data.DataRow() = TempTable.GetErrors()
        End Try

        SqlCommand.Dispose()
        SqlCommand = Nothing

      End If


    Catch ex As Exception
      GetValuesTrendsByAFTT_Asknig = Nothing
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

    End Try
    Return TempTable
  End Function
  Public Function GetValuesTrendsByAFTT_Est(ByVal amod_id As Long, ByVal forsaleFlag As String, ByVal yearOne As String, ByVal yearTwo As String, ByVal afttStart As String, ByVal afttEnd As String, ByVal regType As String, ByVal Startdate As String, ByVal EndDate As String, ByVal variantListString As String, ByVal AFTTQuery As String, Optional ByVal ac_id As Long = 0) As DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim TempTable As New DataTable
    Dim query As String
    Try

      If amod_id <> 0 Then

        TempTable.Columns.Add("AFTT")
        TempTable.Columns.Add("AFTTSORT", System.Type.GetType("System.Int64"))
        TempTable.Columns.Add("type_of")
        TempTable.Columns.Add("SALECOUNT")
        TempTable.Columns.Add("LOWASKING")
        TempTable.Columns.Add("AVGASKING")
        TempTable.Columns.Add("HIGHASKING")
        TempTable.Columns.Add("LOWSALE")
        TempTable.Columns.Add("AVGSALE")
        TempTable.Columns.Add("HIGHSALE")
        TempTable.Columns.Add("COUNTASKING")
        TempTable.Columns.Add("SUMASKING")
        TempTable.Columns.Add("COUNTSALE")
        TempTable.Columns.Add("SUMSALE")


        'Opening Connection
        SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString
        SqlConn.Open()

        query = "SELECT "


        query += AFTTQuery & " as AFTT"
        query += " , 'Est' as type_of, "
        query += " 0 as SALECOUNT,"
        query += " 0 as LOWASKING, "
        query += " 0 AS AVGASKING,"
        query += " 0 as HIGHASKING,"
        query += " 0 AS LOWSALE,"
        query += " 0 AS AVGSALE,"
        query += " 0 AS HIGHSALE,"
        query += " 0 as COUNTASKING, "
        query += " sum(case when ac_asking_price > 0 then 1 else 0 end) as SUMASKING, "
        query += " 0 as COUNTSALE, "
        query += " 0 as SUMSALE "

        If ac_id > 0 Then
          query += ", (select " & AFTTQuery & " from Aircraft_FMV fmv2 with (NOLOCK) where afmv_ac_id = " & ac_id & " and afmv_latest_flag='Y' and afmv_value > 0 and afmv_status='Y' ) as AC_HRS "
        Else
          query += ", '' as  AC_HRS "
        End If

        If HttpContext.Current.Session.Item("localPreferences").UserSPIViewFlag = True Then
          query += ", min(afmv_value) as LOWVALUE "
          query += ", avg(afmv_value) as AVGVALUE "
          query += ", max(afmv_value) as HIGHVALUE "
        End If

        query += "  From Aircraft_Flat with (NOLOCK) "
        query += " inner join Aircraft_FMV with (NOLOCK) on ac_id = afmv_ac_id and afmv_latest_flag='Y' and afmv_value > 0 and afmv_status='Y' and afmv_airframe_hrs > 0 "
        query += " where ac_journ_id = 0 and afmv_airframe_hrs > 0 "
        If Not String.IsNullOrEmpty(variantListString) Then
          query += " and amod_id in (" & amod_id & "," & variantListString & ")"
        Else
          query += " and amod_id = @amodID"
        End If


        '-- YEAR RANGE
        If Not String.IsNullOrEmpty(yearOne) And Not String.IsNullOrEmpty(yearTwo) Then
          If yearOne > 0 And yearTwo > 0 Then
            query += (" and ac_year between @yearOne and @yearTwo")
          End If
        End If


        If forsaleFlag = "Y" Then
          query += "  and ac_forsale_flag = 'Y' "
        End If

        'reg Type
        If Not String.IsNullOrEmpty(regType) Then
          If regType = "N" Then
            query += "  and ac_reg_no like 'N%' "
          ElseIf regType = "I" Then
            query += "  and ac_reg_no not like 'N%' "
          End If
        End If

        ' If ac_id > 0 Then
        query += " and  ( " & AFTTQuery & " is not null ) "
        'End If

        If Not String.IsNullOrEmpty(afttStart) And Not String.IsNullOrEmpty(afttEnd) Then
          '-- AFTT
          query += "  and afmv_airframe_hrs between @startAFTT and @endAFTT"
        End If

        query += " group by   "
        query += "( " & AFTTQuery & " )"
        query += " order by "
        query += " cast( replace( ( " & AFTTQuery & " ), ' - ', '') as float) desc "


        clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, query.ToString)

        Dim SqlCommand As New SqlClient.SqlCommand(query, SqlConn)
        SqlCommand.Parameters.AddWithValue("amodID", amod_id)
        SqlCommand.Parameters.AddWithValue("yearOne", yearOne)
        SqlCommand.Parameters.AddWithValue("yearTwo", yearTwo)
        SqlCommand.Parameters.AddWithValue("startAFTT", afttStart)
        SqlCommand.Parameters.AddWithValue("endAFTT", afttEnd)
        SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

        Try
          TempTable.Load(SqlReader)
        Catch constrExc As System.Data.ConstraintException
          Dim rowsErr As System.Data.DataRow() = TempTable.GetErrors()
        End Try

        SqlCommand.Dispose()
        SqlCommand = Nothing

      End If


    Catch ex As Exception
      GetValuesTrendsByAFTT_Est = Nothing
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

    End Try
    Return TempTable
  End Function
  Public Function GetAircraftSliderValues(ByVal amod_id As Long, ByVal variantIDs As String) As DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim TempTable As New DataTable
    Dim query As String
    Try

      If amod_id <> 0 Then


        'Opening Connection
        SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString
        SqlConn.Open()


        query = "select MIN(ac_year) as MINYEAR, MAX(ac_year) AS MAXYEAR, "
        query += " MAX(ac_est_airframe_hrs) AS MAXAFTT"
        query += " from Aircraft_Flat with (NOLOCK)"

        If Not String.IsNullOrEmpty(variantIDs) Then
          query += " where amod_id in (" & amod_id & "," & variantIDs & ") "
        Else
          query += " where amod_id = @amodID "
        End If


        query += clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(HttpContext.Current.Session.Item("localSubscription"), False, True)
        query += " and ac_journ_id = 0"



        clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, query.ToString)
        Dim SqlCommand As New SqlClient.SqlCommand(query, SqlConn)


        SqlCommand.Parameters.AddWithValue("amodID", amod_id)


        SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

        Try
          TempTable.Load(SqlReader)
        Catch constrExc As System.Data.ConstraintException
          Dim rowsErr As System.Data.DataRow() = TempTable.GetErrors()
        End Try

        SqlCommand.Dispose()
        SqlCommand = Nothing

      End If
      Return TempTable
    Catch ex As Exception
      GetAircraftSliderValues = Nothing
      'Me.class_error = "Error in GetAircraftStartingTable(ByVal amod_id As Long) As DataTable SQL VERSION: " & ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing



    End Try

  End Function
  Public Sub DisplayAFTTTable(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_scriptString As String, ByRef out_htmlString As String, ByVal graphID As Integer, Optional ByVal faa_date As String = "", Optional ByVal bFromUtilizationTab As Boolean = False, Optional ByVal ValuePDF As Boolean = False, Optional ByVal div_height As Integer = 295, Optional ByVal ac_dlv_year As Integer = 0, Optional ByVal show_asking As Boolean = True, Optional ByVal show_sale As Boolean = True, Optional ByVal show_evaules As Boolean = True, Optional ByVal from_pdf As String = "", Optional ByRef count_of_records_visible As String = "", Optional ByRef ticks_string_to_return As String = "", Optional ByVal ac_id As Long = 0, Optional ByVal YearOne As String = "", Optional ByVal YearTwo As String = "", Optional ByVal forsaleFlag As String = "", Optional ByVal regType As String = "", Optional ByVal afttStart As String = "", Optional ByVal afttEnd As String = "", Optional ByVal miniGraph As Boolean = False)

    Dim htmlOut As New StringBuilder
    Dim scriptOut As New StringBuilder
    Dim column_scriptOut As New StringBuilder
    Dim results_table As New DataTable
    Dim sold_table As New DataTable
    Dim asking_table As New DataTable
    Dim est_val_table As New DataTable

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
    Dim results As String = ""
    Dim sumAsking As Long = 0
    Dim sumSale As Long = 0
    Dim countAsking As Long = 0
    Dim CountSale As Long = 0
    Dim AircraftYear As String = ""
    Dim last_AircraftYear As String = ""
    Dim AvgAsking As String = ""
    Dim AvgSale As String = ""
    Dim AVGVALUE As String = ""
    Dim HIGHVALUE As String = ""
    Dim LOWVALUE As String = ""
    Dim found_aftt_cat As Boolean = False
    Dim aftt_spot As Integer = 0
    Dim aftt_spot_used As Integer = 0
    Dim highlight_this_ac As Boolean = False
    Dim low_highlight As String = "null"
    Dim avg_highlight As String = "null"
    Dim high_highlight As String = "null"
    Dim total_spots As Integer = 0
        Dim hours_string As String = "null"
        Dim low_aftt As Long = 4000
        Dim high_aftt As Long = 4999
        Dim client_asking_added As Boolean = False
        Dim client_sale_added As Boolean = False

        Try
      results_table = GetAircraftSliderValues(searchCriteria.ViewCriteriaAmodID, "")

      If Not IsNothing(results_table) Then
        If results_table.Rows.Count > 0 Then
          If Not IsDBNull(results_table.Rows(0).Item("MAXAFTT")) Then
            searchCriteria.ViewCriteriaAFTTEnd = results_table.Rows(0).Item("MAXAFTT")
          End If
        End If
      End If

      Dim AFTTQuery As String = "case "
      Dim spacer_space As String = ""

      Dim CeilingAFTT As Long = (Math.Ceiling(searchCriteria.ViewCriteriaAFTTEnd / 1000) * 1000)
      horizontal_tick_string = ""
      For x = 0 To CeilingAFTT Step 1000

        If Len(Trim(x)) = 4 Then
          spacer_space = "  "
        ElseIf Len(Trim(x)) = 5 Then
          spacer_space = ""
        End If

        If x = CeilingAFTT Then
          AFTTQuery += " when ac_airframe_tot_hrs >= " & x & " and ac_airframe_tot_hrs <= " & x + 1000 & " then '" & spacer_space & x & " - " & (x + 1000) & "' "
        Else
          If x = 0 Then
            AFTTQuery += " when ac_airframe_tot_hrs >= 1 and ac_airframe_tot_hrs < 1000 then '   1 - 999' "
          Else
            AFTTQuery += " when ac_airframe_tot_hrs >= " & x & " and ac_airframe_tot_hrs < " & x + 1000 & " then '" & spacer_space & x & " - " & (x + 1000) - 1 & "' "
          End If
        End If
      Next
      AFTTQuery += " end "

      results_table.Clear()
      sold_table = GetValuesTrendsByAFTT(searchCriteria.ViewCriteriaAmodID, forsaleFlag, YearOne, YearTwo, afttStart, afttEnd, regType, "", "", "", AFTTQuery)
      asking_table = GetValuesTrendsByAFTT_Asknig(searchCriteria.ViewCriteriaAmodID, forsaleFlag, YearOne, YearTwo, afttStart, afttEnd, regType, "", "", "", AFTTQuery)

      AFTTQuery = Replace(AFTTQuery, " ac_airframe_tot_hrs ", " afmv_airframe_hrs ")

      est_val_table = GetValuesTrendsByAFTT_Est(searchCriteria.ViewCriteriaAmodID, forsaleFlag, YearOne, YearTwo, afttStart, afttEnd, regType, "", "", "", AFTTQuery, ac_id)

      results_table = CombineAssettTables_AFTT(sold_table, asking_table, est_val_table, AFTTQuery)

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          ' then its from the AC PAGE --- 
          If Trim(from_pdf) = "Y" Or Trim(from_pdf) = "A" Then
            ' then skip this section
          Else
            scriptOut.Append("function drawVisualization" + graphID.ToString + "() {" + vbCrLf)
            scriptOut.Append(" var data" + graphID.ToString + " = new google.visualization.DataTable();" + vbCrLf)
          End If


          scriptOut.Append("data" + graphID.ToString + ".addColumn('string', 'AFTT'); ")
          scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', 'Avg Asking'); ")
          scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', 'Avg Sale'); ")
          scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', 'Low " & value_label & "'); ")
          scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', 'Avg " & value_label & "'); ")
          scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', 'High " & value_label & "'); ")
          scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', 'ACLOW'); ")
          scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', 'ACAVG'); ")
          scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', 'ACHIGH'); ")
          scriptOut.Append(" data" + graphID.ToString + ".addRows([")

          'results = "<table id=""afttTable"" width=""100%"" border=""1"">"
          'results += "<thead>"
          'results += "<tr>"
          'If HttpContext.Current.Session.Item("isMobile") Then
          '  results += "<th></th>"
          'End If
          'results += "<th>AFTT</th>"
          'results += "<th># Sold</th>"
          'results += "<th>Low Asking ($k)</th>"
          'results += "<th>Avg Asking ($k)</th>"
          'results += "<th>High Asking ($k)</th>"
          'results += "<th>Low Sale ($k)</th>"
          'results += "<th>Avg Sale ($k)</th>"
          'results += "<th>High Sale ($k)</th>"
          'results += "</tr>"
          'results += "</thead>"
          'results += "<tbody>"
          AircraftYear = "null"
          AvgAsking = "null"
          AvgSale = "null"
          AVGVALUE = "null"
          low_highlight = "null"
          avg_highlight = "null"
          high_highlight = "null"




          For Each r As DataRow In results_table.Rows

                        If Not IsDBNull(r("AFTT")) Then
                            ' results += "<td data-sort=""" & r("AFTTSORT") & """>" 
                            ' results += r("AFTT").ToString
                            AircraftYear = r("AFTT").ToString

                            If InStr(AircraftYear, "-") > 0 Then
                                low_aftt = Left(Trim(AircraftYear), InStr(Trim(AircraftYear), "-") - 2)
                                high_aftt = Right(Trim(AircraftYear), Len(Trim(AircraftYear)) - InStr(Trim(AircraftYear), "-") - 1)
                            End If
                        Else
                            AircraftYear = "null"
                            ' results += "<td>"
                        End If

                        If ac_id > 0 Then
              ' if its changed 
              'If Trim(last_AircraftYear) <> Trim(AircraftYear) Then
              '  aftt_spot_used = aftt_spot_used + 1
              'End If

              If Not IsDBNull(r("AC_HRS")) Then
                If Trim(r("AC_HRS")) <> "" Then
                  hours_string = r("AC_HRS")
                End If
              End If


              If Trim(hours_string) = Trim(last_AircraftYear) Then
                low_highlight = LOWVALUE
                avg_highlight = AVGVALUE
                high_highlight = HIGHVALUE
              End If

              'If Not IsDBNull(r("AC_HRS")) And found_aftt_cat = False Then
              '  If CInt(r("AC_HRS")) > 0 Then
              '    aftt_spot = 0
              '    For x = 0 To CeilingAFTT Step 1000
              '      total_spots = total_spots + 1
              '      If found_aftt_cat = False Then
              '        aftt_spot = aftt_spot + 1
              '      End If

              '      If CInt(r("AC_HRS")) > x And CInt(r("AC_HRS")) <= (x + 1000) Then
              '        found_aftt_cat = True
              '      End If
              '    Next
              '  End If
              'End If 

              '' if something says 7 .. and there is 10 total spots, it should go in the 3rd spot
              ''so if 10, minus 7 .. =3 
              'If (total_spots - aftt_spot) = aftt_spot_used Then
              '  low_highlight = LOWVALUE
              '  avg_highlight = AVGVALUE
              '  high_highlight = HIGHVALUE
              '  aftt_spot = 0
              'End If
            End If

            If Trim(AircraftYear) <> Trim(last_AircraftYear) And Trim(last_AircraftYear) <> "" Then
              If HttpContext.Current.Session.Item("localPreferences").UserSPIViewFlag = True Then
                If Trim(last_AircraftYear) = "null" Then
                  scriptOut.Append("['0', " & Replace(AvgAsking, ",", "") & ", " & Replace(AvgSale, ",", "") & ", " & LOWVALUE & ", " & AVGVALUE & ", " & HIGHVALUE & ", " & low_highlight & ", " & avg_highlight & ", " & high_highlight & "]")
                Else
                  scriptOut.Append("['" & last_AircraftYear & "', " & Replace(AvgAsking, ",", "") & ", " & Replace(AvgSale, ",", "") & ", " & LOWVALUE & ", " & AVGVALUE & ", " & HIGHVALUE & ", " & low_highlight & ", " & avg_highlight & ", " & high_highlight & "]")
                End If
              Else
                If Trim(AircraftYear) = "null" Then
                  scriptOut.Append("['0', " & Replace(AvgAsking, ",", "") & ", null, null, null, null, null, null, null]")
                Else
                  scriptOut.Append("['" & last_AircraftYear & "', " & Replace(AvgAsking, ",", "") & ", null, null, null, null, null, null, null]")
                End If
              End If

              row_added = True
              AvgAsking = "null"
              AvgSale = "null"
              LOWVALUE = "null"
              AVGVALUE = "null"
              HIGHVALUE = "null"
              low_highlight = "null"
              avg_highlight = "null"
                            high_highlight = "null"
                            client_sale_added = False
                            client_asking_added = False

                            If row_added = True Then
                scriptOut.Append(", ")
              End If
            End If


                        If Not IsDBNull(r("COUNTASKING")) Then
                            countAsking = r("COUNTASKING")
                        End If

                        If Not IsDBNull(r("COUNTSALE")) Then
                            CountSale = r("COUNTSALE")
                        End If

                        If Not IsDBNull(r("SUMASKING")) Then
                            sumAsking = r("SUMASKING")
                        End If

                        If Not IsDBNull(r("SUMSALE")) Then
                            sumSale = r("SUMSALE")
                        End If



                        'results += "<tr>"
                        'If HttpContext.Current.Session.Item("isMobile") Then
                        '  results += "<td></td>"
                        'End If


                        If Trim(r("type_of")) = "Sold" Then

                            If client_sale_added = True Then

                            Else
                                If Trim(temp_data) = "" Then
                                    Call Add_in_client_sale_prices("AFTT", countAsking, sumAsking, CountSale, sumSale, 0, 0, 0, 0, 0, 0, 0, low_aftt, high_aftt)
                                Else
                                    Call Add_in_client_sale_prices("AFTT", countAsking, sumAsking, CountSale, sumSale, CLng(temp_data), 0, 0, 0, 0, 0, 0, low_aftt, high_aftt)
                                End If


                                If HttpContext.Current.Session.Item("localPreferences").UserSPIViewFlag = True Then
                                    If sumSale > 0 And CountSale > 0 Then
                                        ' results += "$" & FormatNumber((r("AVGSALE") / 1000), 0) '& "k" 
                                        AvgSale = Replace(FormatNumber(((sumSale / CountSale) / 1000), 0), ",", "")
                                        Call check_high_low(AvgSale, high_number, low_number)
                                    Else
                                        AvgSale = "null"
                                    End If
                                Else
                                    AvgSale = "null"
                                End If
                                client_sale_added = True
                            End If


                        ElseIf Trim(r("type_of")) = "Asking" Then

                            If client_asking_added = True Then

                            Else
                                Call Add_in_client_asking("AFTT", sumAsking, countAsking, 0, 0, low_aftt, high_aftt)

                                If sumAsking > 0 And countAsking > 0 Then
                                    'results += "$" & FormatNumber((r("AVGASKING") / 1000), 0) '& "k"
                                    AvgAsking = Replace(FormatNumber(((sumAsking / countAsking) / 1000), 0), ",", "")

                                    Call check_high_low(AvgAsking, high_number, low_number)
                                Else
                                    AvgAsking = "null"
                                End If
                                client_asking_added = True
                            End If


                        ElseIf Trim(r("type_of")) = "Est" Then
                            If Not IsDBNull(r("LOWVALUE")) Then
                                LOWVALUE = r("LOWVALUE")
                                LOWVALUE = FormatNumber((LOWVALUE / 1000), 0).ToString
                                Call check_high_low(LOWVALUE, high_number, low_number)
                                LOWVALUE = Replace(LOWVALUE, ",", "")
                            Else
                                LOWVALUE = "null"
                            End If

                            If Not IsDBNull(r("AVGVALUE")) Then
                                AVGVALUE = r("AVGVALUE")
                                AVGVALUE = FormatNumber((AVGVALUE / 1000), 0).ToString
                                Call check_high_low(AVGVALUE, high_number, low_number)
                                AVGVALUE = Replace(AVGVALUE, ",", "")
                            Else
                                AVGVALUE = "null"
                            End If

                            If Not IsDBNull(r("HIGHVALUE")) Then
                                HIGHVALUE = r("HIGHVALUE")
                                HIGHVALUE = FormatNumber((HIGHVALUE / 1000), 0).ToString
                                Call check_high_low(HIGHVALUE, high_number, low_number)
                                HIGHVALUE = Replace(HIGHVALUE, ",", "")
                            Else
                                HIGHVALUE = "null"
                            End If

                        End If




                        ' if we changed, then add the item to the horizontal string 
                        If (Trim(last_AircraftYear) <> Trim(AircraftYear)) Then
                            If row_added = True Then
                                horizontal_tick_string &= ", "
                            End If

                            horizontal_tick_string &= "'" & AircraftYear & "'"
                        End If

                        last_AircraftYear = AircraftYear

            ' results += "</td>"
            ' results += "</tr>"
          Next
          ' results += "</tbody>"

          ' results += "</table>"

          If HttpContext.Current.Session.Item("localPreferences").UserSPIViewFlag = True Then
            If Trim(AircraftYear) = "null" Then
              scriptOut.Append("['0', " & Replace(AvgAsking, ",", "") & ", " & Replace(AvgSale, ",", "") & ", " & LOWVALUE & ", " & AVGVALUE & ", " & HIGHVALUE & ", " & low_highlight & ", " & avg_highlight & ", " & high_highlight & "]")
            Else
              scriptOut.Append("['" & AircraftYear & "', " & Replace(AvgAsking, ",", "") & ", " & Replace(AvgSale, ",", "") & ", " & LOWVALUE & ", " & AVGVALUE & ", " & HIGHVALUE & ", " & low_highlight & ", " & avg_highlight & ", " & high_highlight & "]")
            End If
          Else
            If Trim(AircraftYear) = "null" Then
              scriptOut.Append("['0', " & Replace(AvgAsking, ",", "") & ", null, null, null, null, null, null, null]")
            Else
              scriptOut.Append("['" & AircraftYear & "', " & Replace(AvgAsking, ",", "") & ", null, null, null, null, null, null, null]")
            End If
          End If

          row_added = True



          ' If Not IsDBNull(r("amod_make_name")) And Not IsDBNull(r("amod_model_name")) Then 
          '   temp_data = r.Item("amod_make_name") & " " & r("amod_model_name")  
          ' Else
          '   temp_data = ""
          ' End If



          'count_of_records_visible = current_rows

          'horizontal_tick_string = ""
          'commonEvo.make_ticks_string(first_date, last_date, horizontal_tick_string)

          'finish_script(scriptOut, current_point, null_max) ' also ends ]

          'current_rows = current_rows + 1
          'finish_column_script(column_scriptOut, current_rows, null_max, graphID)




          ticks_string = "Y" ' so that is does the extra build
          commonEvo.set_ranges_for_vsCharts(low_number, high_number, interval_point, starting_point, ending_point, ticks_string)


          If Trim(from_pdf) = "Y" Or Trim(from_pdf) = "A" Then

          Else
            scriptOut.Append("]);" + vbCrLf)

            scriptOut.Append("var options = { " + vbCrLf)
            scriptOut.Append("  chartArea:{width:'" & IIf(miniGraph, "80", "85") & "%',height:'" & IIf(miniGraph, "68", "80") & "%'}," + vbCrLf)


            scriptOut.Append("series: { ")

            scriptOut.Append(" 0: { lineWidth: 0, pointSize: 3  } ")   ' , lineDashStyle: [4, 4]
            If HttpContext.Current.Session.Item("localPreferences").UserSPIViewFlag = True Then
              scriptOut.Append(", 1: { lineWidth: 0, pointSize: 3  } ")
            Else
              scriptOut.Append(", 1: { lineWidth: 0, pointSize: 3 , visibleInLegend: false } ")
            End If
            scriptOut.Append(", 2: { lineWidth: 2, pointSize: 2 , lineDashStyle: [4, 4] } ")
            scriptOut.Append(", 3: { lineWidth: 3, pointSize: 3 , lineDashStyle: [4, 4] } ")
            scriptOut.Append(", 4: { lineWidth: 2, pointSize: 2 , lineDashStyle: [4, 4] } ")

            scriptOut.Append(" ,  5: { lineWidth: 0, pointSize: 7, visibleInLegend: false  } ")
            scriptOut.Append(" ,  6: { lineWidth: 0, pointSize: 7, visibleInLegend: false  } ")
            scriptOut.Append(" ,  7: { lineWidth: 0, pointSize: 7, visibleInLegend: false  } ")

            'scriptOut.Append("    0: { lineWidth: 1, pointSize: 1  , lineDashStyle: [4, 4] } ")
            'scriptOut.Append(" ,  1: { lineWidth: 2, pointSize: 2  , lineDashStyle: [4, 4] } ")
            'scriptOut.Append(" ,  2: { lineWidth: 1, pointSize: 1  , lineDashStyle: [4, 4] } ")


            scriptOut.Append("  }  , ")

            scriptOut.Append("  hAxis: { title: 'AFTT'," + vbCrLf)
            scriptOut.Append("           textStyle: {fontSize: " & IIf(miniGraph, "8", "10") & ", bold: false, italic: false }, " + vbCrLf) ' , fontName:  'Arial'    color: '#01579b', 
            scriptOut.Append("           titleTextStyle: {  fontSize: " & IIf(miniGraph, "8", "15") & ", fontName:  'Arial', bold: false, italic: false }" + vbCrLf)  'color: '#01579b',
            If Trim(horizontal_tick_string) <> "" Then
              scriptOut.Append(", ticks: [ " & horizontal_tick_string & "] ")
            End If
            scriptOut.Append("         }," + vbCrLf)
            scriptOut.Append("  vAxis: { title: 'Price ($k)'," + vbCrLf)
            scriptOut.Append("           textStyle: { fontSize: " & IIf(miniGraph, "8", "10") & ", bold: false }," + vbCrLf)   '  color: '#1a237e',
            scriptOut.Append("           titleTextStyle: { fontSize: " & IIf(miniGraph, "8", "15") & ", bold: false }" + vbCrLf)  '  color: '#1a237e',
            If Trim(ticks_string) <> "" Then
              scriptOut.Append(", ticks: [ " & ticks_string & "] ")
            End If
            scriptOut.Append("        }," + vbCrLf)
            scriptOut.Append("  smoothLine:true," + vbCrLf)
            scriptOut.Append(" legend: { position: 'top', textStyle:{fontSize:" & IIf(miniGraph, "8", "9") & "}}, " + vbCrLf)

            '#B7DCF6  - blue             
            '#a3c28d  - green
            '#a84543 - red
            scriptOut.Append("  colors: ['#a3c28d','#a84543', '" & value_color & "', '" & value_color & "', '" & value_color & "', '" & value_color & "', '" & value_color & "', '" & value_color & "', '#a84543', '" & value_color & "', '#a3c28d', '#eba059', '#a84543']" + vbCrLf)
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
        htmlOut.Append("<table id=""flightActivityTable"" width=""100%"" cellspacing=""0"" cellpadding=""4"">")
        htmlOut.Append("<tr><td valign=""top"" align=""left""><div id='visualization" + graphID.ToString + "' style=""height:" & div_height.ToString & "px;""></div></td></tr>")
        htmlOut.Append("</table>" + vbCrLf)
      Else
        htmlOut.Append("<table id=""flightActivityTable"" width=""100%"" cellspacing=""0"" cellpadding=""4"">")
        htmlOut.Append("<tr><td valign=""middle"" align=""center"">No eValue estimates at this time for this make and model. ...</td></tr>")
        htmlOut.Append("</table>" + vbCrLf)
      End If

    Catch ex As Exception

      aError = "Error in views_display_flight_utilization_graph(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, Optional ByVal faa_date As String = """") " + ex.Message

    Finally

    End Try

    'return resulting html string
    out_scriptString = scriptOut.ToString

    out_htmlString = htmlOut.ToString
    ticks_string_to_return = ticks_string


    htmlOut = Nothing
    results_table = Nothing

  End Sub
  Public Sub finish_column_script(ByRef scriptOut As StringBuilder, ByVal current_point As Integer, ByVal point_max As Integer, ByVal graphID As String)

    ' each point has 3 items .. so if we have added 10 points, there will be 30 total columns. 
    ' 50 as point max, means there will be 150 total columns

    Dim i As Integer = 0

    For i = current_point To point_max
      ' scriptOut.Append("data" + graphID.ToString + ".addColumn('number', '" & i & " Low Residual'); ")
      scriptOut.Append("data" + graphID.ToString + ".addColumn('number', '" & i & " Avg Residual'); ")
      ' scriptOut.Append("data" + graphID.ToString + ".addColumn('number', '" & i & " High Residual'); ")
    Next

  End Sub
  Public Sub finish_script(ByRef scriptOut As StringBuilder, ByVal current_point As Integer, ByVal point_max As Integer)

    ' each point has 3 items .. so if we have added 10 points, there will be 30 total columns. 
    ' 50 as point max, means there will be 150 total columns

    current_point = current_point + 1
    Dim i As Integer = 0

    For i = current_point To point_max
      ' scriptOut.Append(",null") ' for low 
      scriptOut.Append(",null") ' for avg 
      ' scriptOut.Append(",null") ' for high 
    Next


    scriptOut.Append("]")  ' make sure u end the script

  End Sub



  Public Sub views_display_assett_prices_graph_US_Foreign(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_scriptString As String, ByRef out_htmlString As String, ByVal graphID As Integer, Optional ByVal faa_date As String = "", Optional ByVal bFromUtilizationTab As Boolean = False, Optional ByVal ValuePDF As Boolean = False, Optional ByVal div_height As Integer = 295, Optional ByVal ac_dlv_year As Integer = 0, Optional ByVal show_asking As Boolean = True, Optional ByVal show_sale As Boolean = True, Optional ByVal show_evaules As Boolean = True, Optional ByVal from_pdf As String = "", Optional ByRef ticks_string_to_return As String = "")

    Dim htmlOut As New StringBuilder
    Dim scriptOut As New StringBuilder
    Dim results_table As New DataTable

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
    Dim IS_US_REG As Integer = 0
    Dim last_mfr As String = ""
    Dim foreign_val As String = ""
    Dim foreign_val_year As String = ""


    Try

      results_table = get_assett_summary_US_vs_Foreign(searchCriteria, faa_date, bFromUtilizationTab)

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          If Trim(from_pdf) = "Y" Then
          Else
            scriptOut.Append("function drawVisualization" + graphID.ToString + "() {" + vbCrLf)
            'scriptOut.Append(" alert('drawVisualization" + graphID.ToString + "');" + vbCrLf)  
            scriptOut.Append(" var data" + graphID.ToString + " = new google.visualization.DataTable();" + vbCrLf)
          End If

          scriptOut.Append("data" + graphID.ToString + ".addColumn('string', 'DLV Year'); ")
          scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', 'Foreign Avg " & value_label & "'); ")
          scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', 'US Avg " & value_label & "'); ")
          scriptOut.Append(" data" + graphID.ToString + ".addRows([")

          For Each r As DataRow In results_table.Rows

            temp_data = "null"
            temp_low = "null"
            temp_avg = "null"
            temp_high = "null"
            avg_asking = "null"
            avg_sale = "null"

            If Not IsDBNull(r("ac_year")) Then
              temp_data = r.Item("ac_year")
            Else
              temp_data = "null"
            End If

            If Not IsDBNull(r("IS_US_REG")) Then
              IS_US_REG = r.Item("IS_US_REG")
            Else
              IS_US_REG = 0
            End If

            If Not IsDBNull(r("AVGVALUE")) And show_evaules = True Then
              temp_avg = r.Item("AVGVALUE")
              If CLng(temp_avg) > 0 Then
                temp_avg = CLng(temp_avg / 1000)
                Call check_high_low(temp_avg, high_number, low_number)
              End If
            Else
              temp_avg = "null"
            End If


            If IS_US_REG = 1 Then
              If row_added Then
                scriptOut.Append(",")
              End If

              'if there is a foreign value and it is for this year
              If Trim(foreign_val) <> "" And Trim(foreign_val_year) = Trim(temp_data) Then
                scriptOut.Append("['" & temp_data & "', " & foreign_val & ", " & temp_avg & "]")
              ElseIf Trim(foreign_val) <> "" And Trim(foreign_val_year) = Trim(temp_data) Then
                'if there is a foreign value and it is for the year (most likely) before this one
                scriptOut.Append("['" & temp_data & "', " & foreign_val & ",null]")
              Else
                scriptOut.Append("['" & temp_data & "', null, " & temp_avg & " ]")
              End If

              row_added = True
              foreign_val = ""
              foreign_val_year = ""
            Else
              foreign_val = temp_avg
              foreign_val_year = temp_data
            End If

            last_mfr = temp_data

          Next

          ' if the last one was foreign
          If Trim(foreign_val) <> "" And Trim(foreign_val_year) <> "" Then
            scriptOut.Append("['" & temp_data & "', " & foreign_val & ", null]")
          End If

          ticks_string = "Y" ' so that is does the extra build
          commonEvo.set_ranges_for_vsCharts(low_number, high_number, interval_point, starting_point, ending_point, ticks_string)


          If Trim(from_pdf) = "Y" Then
          Else
            scriptOut.Append("]);" + vbCrLf)

            scriptOut.Append("var options = { " + vbCrLf)
            scriptOut.Append("  chartArea:{width:'76%',height:'72%'}," + vbCrLf)
            scriptOut.Append("series: { ")

            scriptOut.Append("    0: { lineWidth: 3, pointSize: 3  , lineDashStyle: [4, 4] } ")
            scriptOut.Append(" ,  1: { lineWidth: 3, pointSize: 3  , lineDashStyle: [4, 4] } ")
            scriptOut.Append(" ,  2: { lineWidth: 3, pointSize: 3  , lineDashStyle: [4, 4] } ")

            scriptOut.Append("  }  , ")
            scriptOut.Append("  hAxis: { title: 'DLV Year'," + vbCrLf)
            scriptOut.Append("           textStyle: { color: '#01579b', fontSize: 10, fontName:  'Arial', bold: true, italic: true }, " + vbCrLf)
            scriptOut.Append("           titleTextStyle: { color: '#01579b', fontSize: 15, fontName:  'Arial', bold: false, italic: true }" + vbCrLf)
            scriptOut.Append("         }," + vbCrLf)
            scriptOut.Append("  vAxis: { title: 'Price ($k)'," + vbCrLf)
            scriptOut.Append("           textStyle: { color: '#1a237e', fontSize: 10, bold: true }," + vbCrLf)
            scriptOut.Append("           titleTextStyle: { color: '#1a237e', fontSize: 15, bold: true }" + vbCrLf)
            If Trim(ticks_string) <> "" Then
              scriptOut.Append(", ticks: [ " & ticks_string & "] ")
            End If
            scriptOut.Append("        }," + vbCrLf)
            scriptOut.Append("  smoothLine:true," + vbCrLf)
            scriptOut.Append(" legend: { position: 'top', textStyle:{fontSize:8}}, " + vbCrLf)
            scriptOut.Append("  colors: ['" & value_color & "','#a3c28d', '" & value_color & "', '#a3c28d', '#a84543']")
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
        htmlOut.Append("<table id=""flightActivityTable"" width=""100%"" cellspacing=""0"" cellpadding=""4"">")
        htmlOut.Append("<tr><td valign=""top"" align=""left""><div id='visualization" + graphID.ToString + "' style=""height:" & div_height.ToString & "px;""></div></td></tr>")
        htmlOut.Append("</table>" + vbCrLf)
      Else
        htmlOut.Append("<table id=""flightActivityTable"" width=""100%"" cellspacing=""0"" cellpadding=""4"">")
        htmlOut.Append("<tr><td valign=""middle"" align=""center"">No Flight Utilization Data at this time, for this Make/Model ...</td></tr>")
        htmlOut.Append("</table>" + vbCrLf)
      End If

    Catch ex As Exception

      aError = "Error in views_display_flight_utilization_graph(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, Optional ByVal faa_date As String = """") " + ex.Message

    Finally

    End Try

    'return resulting html string
    out_scriptString = scriptOut.ToString
    out_htmlString = htmlOut.ToString
    ticks_string_to_return = ticks_string
    htmlOut = Nothing
    results_table = Nothing

  End Sub
  Public Sub views_display_evalues_in_status_block(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal journalID As Long, ByRef estimator_width As System.Web.UI.WebControls.Button, ByRef status_label As String, ByRef found_eval As Boolean, ByVal temp_ac_dlv_year As String, ByVal temp_make_model As String, ByVal AircraftModel_JETNET As Long, ByVal temp_jetnet_ac_id As Long, ByVal aircraftID As Long)
    Dim current_month_table As New DataTable
    Dim comp_functions As New CompanyFunctions
    Dim temp_est As String = ""

    Try

      If journalID = 0 Then
        current_month_table = get_current_month_assett_summary(searchCriteria)
        If Not IsNothing(current_month_table) Then
          If current_month_table.Rows.Count > 0 Then
            For Each r As DataRow In current_month_table.Rows
              If Not IsDBNull(r("AVGVALUE")) Then
                found_eval = True
                If Not IsNothing(estimator_width) Then
                  estimator_width.Width = 200
                End If

                If Not status_label.ToUpper.Contains(value_label.ToUpper) Then
                  status_label = Replace(status_label, "</table></div>", comp_functions.create_value_with_label("<a href=""#"" onclick=""javascript:load('/help/documents/809.pdf','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');return false;""><font color='" & value_color & "'>" & value_label & "</font></a>", "<font color='" & value_color & "'>$" & FormatNumber((r("AVGVALUE") / 1000), 0) & "k</font>", True, False, 0, "") & "</table></div>")
                End If
              End If

            Next
          End If
        End If

        ' set it for the 2nd selection 
        searchCriteria.ViewCriteriaAmodID = AircraftModel_JETNET
        current_month_table.Clear()
        ' get rid of the ac id so it just does model 
        current_month_table = get_current_month_assett_summary(searchCriteria, temp_ac_dlv_year)
        If Not IsNothing(current_month_table) Then
          If current_month_table.Rows.Count > 0 Then
            For Each r As DataRow In current_month_table.Rows
              If Not IsDBNull(r("AVGVALUE")) Then
                If found_eval = False Then
                  status_label = Replace(status_label, "</table></div>", comp_functions.create_value_with_label("<font color='" & value_color & "'><a href='' title='Avg " & temp_ac_dlv_year & " " & temp_make_model & " " & value_label & "' alt='Avg " & temp_ac_dlv_year & " " & temp_make_model & " " & value_label & "'><font color='" & value_color & "'>Avg/Year</font></a>", "<font color='" & value_color & "'>$" & FormatNumber((r("AVGVALUE") / 1000), 0) & "k</font>", True, False, 0, "") & "</table></div>")
                Else

                  If Not status_label.ToUpper.Contains("Avg/Year".ToUpper) Then
                    status_label = Replace(status_label, "</td></tr></table></div>", "&nbsp;&nbsp;&nbsp;&nbsp;<font color='" & value_color & "'>(<a href='' title='Avg " & temp_ac_dlv_year & " " & temp_make_model & " " & value_label & "' alt='Avg " & temp_ac_dlv_year & " " & temp_make_model & " " & value_label & "'><font color='" & value_color & "'>Avg/Year:</font></a> $" & FormatNumber((r("AVGVALUE") / 1000), 0) & "k)</font></td></tr></table></div>")
                  End If

                End If
              End If
            Next
          End If
        End If

        If Not status_label.ToUpper.Contains("Airframe total time was estimated".ToUpper) Then
          ' if the value is based off of the average usage, then we should explain
          If temp_jetnet_ac_id > 0 And aircraftID > 0 Then
            temp_est = Get_Hours_Based_Usage(temp_jetnet_ac_id, "", "")   ' so its jetnet not client
          Else
            temp_est = Get_Hours_Based_Usage(aircraftID, "", "")
          End If

          If temp_est > 0 Then
            status_label = Replace(status_label, "</table></div>", "<tr class='" & HttpContext.Current.Session("ROW_CLASS_BOTTOM") & "' valign='top'><td align='left' colspan='2'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT_SMALL") & "'><font color='" & value_color & "' size='-2'>Airframe total time was estimated at " & FormatNumber(temp_est, 0) & " hours based on average hours/month.</font></font></td></tr></table></div>")
          End If
        End If

        'clear it again 
        searchCriteria.ViewCriteriaAmodID = 0
      End If

    Catch ex As Exception

    End Try

  End Sub
  Public Function Get_Hours_Based_Usage(ByVal ac_id As Long, ByRef afmv_date As String, ByRef afmv_landings As String) As Long
    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlDataReader As System.Data.SqlClient.SqlDataReader : SqlDataReader = Nothing
    Get_Hours_Based_Usage = 0
    Dim tmpStr As String : tmpStr = ""

    Dim Query As String : Query = ""
    Query = "select afmv_airframe_hrs, afmv_date, "
    Query &= " ac_airframe_flights_based_avg "
    Query &= " from Aircraft_FMV with (NOLOCK) "
    Query &= " inner join View_Aircraft_Usage_Estimates with (NOLOCK) on afmv_ac_id = ac_id   "
    Query &= " where afmv_ac_id = " & ac_id & " and afmv_jetnet_assumptions='HRS BASED ON USAGE PER MONTH' "
    Query &= " and afmv_status='Y' and afmv_latest_flag='Y' "


    '    Query &= " " + commonEvo.GenerateProductCodeSelectionQuery(Session.Item("localSubscription"), False, True)
    '  Query = Query & clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(Session.Item("localSubscription"), False, False)

    Try

      'Select Case Application.Item("webHostObject").evoWebHostType
      'Case eWebSiteTypes.LOCAL
      'SqlConn.ConnectionString = My.Settings.DEFAULT_LOCAL_MSSQL
      '  Case Else
      SqlConn.ConnectionString = clientConnectString
      'End Select

      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandTimeout = 1000
      SqlCommand.CommandText = Query.ToString

      SqlDataReader = SqlCommand.ExecuteReader()

      If SqlDataReader.HasRows Then
        SqlDataReader.Read()
        If Not IsDBNull(SqlDataReader("afmv_airframe_hrs")) Then
          If IsNumeric(SqlDataReader("afmv_airframe_hrs")) Then
            Get_Hours_Based_Usage = SqlDataReader("afmv_airframe_hrs")
          End If
        End If

        If Not IsDBNull(SqlDataReader("afmv_date")) Then
          afmv_date = SqlDataReader("afmv_date")
        End If

        If Not IsDBNull(SqlDataReader("ac_airframe_flights_based_avg")) Then
          If IsNumeric(SqlDataReader("ac_airframe_flights_based_avg")) Then
            afmv_landings = SqlDataReader("ac_airframe_flights_based_avg")
          End If
        End If
      End If
      SqlDataReader.Close()
      SqlDataReader = Nothing
    Catch SqlException

      SqlConn.Dispose()
      SqlCommand.Dispose()

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in Get_Hours_Based_Usage: " & SqlException.Message

    Finally

      SqlCommand.Dispose()
      SqlConn.Close()
      SqlConn.Dispose()

    End Try


  End Function
    Public Shared Function check_missing_client_years(ByRef dt As DataTable, ByRef bad_years As String, ByRef bad_year_ac_id As String, ByVal year_field_name As String) As Boolean

        check_missing_client_years = False

        Dim all_years As String = ""
        Dim AircraftYear As String = ""

        If HttpContext.Current.Session.Item("localUser").crmUser_Evo_MPM_Flag = True And valueControl.has_client_data = True Then
            If valueControl.array_count > 0 Then

                ' go through all of the client records, and write years into all years string 
                For i = 0 To valueControl.array_count - 1
                    If Trim(all_years) <> "" Then
                        all_years &= " , "
                    End If

                    all_years &= " " & valueControl.ac_dlv_year_array(i) & " "
                Next

                ' go thro all of the jetnet records, and remove all of the years from the string that exist in jetnet 
                For Each r As DataRow In dt.Rows
                    AircraftYear = ""
                    If Not IsDBNull(r("COUNTASKING")) Then
                        AircraftYear = r("" & year_field_name & "").ToString
                    End If

                    all_years = Replace(all_years, " " & AircraftYear & " ,", "")
                    all_years = Replace(all_years, " " & AircraftYear & " ", "")
                Next

                ' if there are years leftover, then these years are in client but not in jetnet 
                If Trim(all_years) <> "" Then
                    ' go back through, and get those ac_ids 
                    For i = 0 To valueControl.array_count - 1

                        'if the ac_id's delivery year is bad, then put it into a string 
                        If InStr(all_years, valueControl.ac_dlv_year_array(i)) > 0 Then
                            If Trim(bad_year_ac_id) <> "" Then
                                bad_year_ac_id &= " , "
                            End If

                            bad_year_ac_id &= " " & valueControl.ac_id_array(i) & " "
                        End If
                    Next

                    bad_years = all_years
                    dt.Rows.Clear()

                    check_missing_client_years = True

                End If
            End If
        End If

    End Function
    Public Sub views_display_assett_prices_graph(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_scriptString As String, ByRef out_htmlString As String, ByVal graphID As Integer, Optional ByVal faa_date As String = "", Optional ByVal bFromUtilizationTab As Boolean = False, Optional ByVal ValuePDF As Boolean = False, Optional ByVal div_height As Integer = 295, Optional ByVal ac_dlv_year As Integer = 0, Optional ByVal show_asking As Boolean = True, Optional ByVal show_sale As Boolean = True, Optional ByVal show_evaules As Boolean = True, Optional ByVal from_pdf As String = "", Optional ByRef ticks_string_to_return As String = "", Optional ByVal YearOne As String = "", Optional ByVal YearTwo As String = "", Optional ByVal forsaleFlag As String = "", Optional ByVal regType As String = "", Optional ByVal afttStart As String = "", Optional ByVal afttEnd As String = "", Optional ByVal variantList As String = "", Optional ByVal miniGraph As Boolean = False)

        Dim htmlOut As New StringBuilder
        Dim scriptOut As New StringBuilder
        Dim results_table As New DataTable

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
        Dim bad_years As String = ""
        Dim bad_year_ac_id As String = ""
        Dim total_sales As Integer = 0
        Dim total_asking As Integer = 0

        Dim countAsking As Integer = 0
        Dim sumAsking As Integer = 0
        Dim CountSale As Integer = 0
        Dim sumSale As Integer = 0
        Dim SALECOUNT As Integer = 0
        Dim LOWASKING As Integer = 0
        Dim HIGHASKING As Integer = 0
        Dim AvgAsking1 As Integer = 0
        Dim LOWSALE As Integer = 0
        Dim AVGSALE1 As Integer = 0
        Dim HIGHSALE As Integer = 0


        Try

            results_table = get_assett_summary(searchCriteria, faa_date, bFromUtilizationTab, YearOne, YearTwo, forsaleFlag, regType, afttStart, afttEnd, variantList)

            ' this will clear results_table
            If check_missing_client_years(results_table, bad_years, bad_year_ac_id, "ac_year") = True Then
                results_table = get_assett_summary(searchCriteria, faa_date, bFromUtilizationTab, YearOne, YearTwo, forsaleFlag, regType, afttStart, afttEnd, variantList, bad_year_ac_id)
            End If



            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    If Trim(from_pdf) = "Y" Then
                    Else
                        scriptOut.Append("function drawVisualization" + graphID.ToString + "() {" + vbCrLf)
                        'scriptOut.Append(" alert('drawVisualization" + graphID.ToString + "');" + vbCrLf)  
                        scriptOut.Append(" var data" + graphID.ToString + " = new google.visualization.DataTable();" + vbCrLf)
                    End If

                    scriptOut.Append("data" + graphID.ToString + ".addColumn('string', 'DLV Year'); ")
                    scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', 'Low " & value_label & "'); ")
                    scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', 'Avg " & value_label & "'); ")
                    scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', 'High " & value_label & "'); ")
                    scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', 'Avg Asking'); ")
                    scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', 'Avg Sale'); ")
                    scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', '" & ac_dlv_year & " Low " & value_label & "'); ")
                    scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', '" & ac_dlv_year & " Avg " & value_label & "'); ")
                    scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', '" & ac_dlv_year & " High " & value_label & "'); ")
                    scriptOut.Append(" data" + graphID.ToString + ".addRows([")

                    For Each r As DataRow In results_table.Rows

                        temp_data = "null"
                        temp_low = "null"
                        temp_avg = "null"
                        temp_high = "null"
                        avg_asking = "null"
                        avg_sale = "null"

                        ' If Not IsDBNull(r("amod_make_name")) And Not IsDBNull(r("amod_model_name")) Then 
                        '   temp_data = r.Item("amod_make_name") & " " & r("amod_model_name")  
                        ' Else
                        '   temp_data = ""
                        ' End If

                        If Not IsDBNull(r("ac_year")) Then
                            temp_data = r.Item("ac_year")
                        Else
                            temp_data = "null"
                        End If

                        If Not IsDBNull(r("LOWVALUE")) And show_evaules = True Then
                            temp_low = r.Item("LOWVALUE")
                            If CLng(temp_low) > 0 Then
                                temp_low = CLng(temp_low / 1000)
                                Call check_high_low(temp_low, high_number, low_number)
                            End If
                        Else
                            temp_low = "null"
                        End If

                        If Not IsDBNull(r("AVGVALUE")) And show_evaules = True Then
                            temp_avg = r.Item("AVGVALUE")
                            If CLng(temp_avg) > 0 Then
                                temp_avg = CLng(temp_avg / 1000)
                                Call check_high_low(temp_avg, high_number, low_number)
                            End If
                        Else
                            temp_avg = "null"
                        End If

                        If Not IsDBNull(r("HIGHVALUE")) And show_evaules = True Then
                            temp_high = r.Item("HIGHVALUE")
                            If CLng(temp_high) > 0 Then
                                temp_high = CLng(temp_high / 1000)
                                Call check_high_low(temp_high, high_number, low_number)
                            End If
                        Else
                            temp_high = "null"
                        End If


                        If Not IsDBNull(r("avg_asking")) And show_asking = True Then
                            avg_asking = r.Item("avg_asking")
                            If CLng(avg_asking) > 0 Then
                                avg_asking = CLng(avg_asking / 1000)
                                Call check_high_low(avg_asking, high_number, low_number)
                            End If
                        Else
                            avg_asking = "null"
                        End If


                        If Not IsDBNull(r("avg_sale")) And show_sale = True Then
                            avg_sale = r.Item("avg_sale")
                            If CLng(avg_sale) > 0 Then
                                avg_sale = CLng(avg_sale / 1000)
                                Call check_high_low(avg_sale, high_number, low_number)
                            End If
                        Else
                            avg_sale = "null"
                        End If



                        If HttpContext.Current.Session.Item("localUser").crmUser_Evo_MPM_Flag = True And valueControl.has_client_data = True Then

                            countAsking = 0
                            sumAsking = 0
                            CountSale = 0
                            sumSale = 0
                            SALECOUNT = 0
                            LOWASKING = 0
                            HIGHASKING = 0
                            AvgAsking1 = 0
                            LOWSALE = 0
                            AVGSALE1 = 0
                            HIGHSALE = 0

                            If Not IsDBNull(r("SUMSALE")) Then
                                sumSale = r("SUMSALE")
                            End If

                            If Not IsDBNull(r("COUNTSALE")) Then
                                SALECOUNT = r("COUNTSALE")
                            End If

                            If Not IsDBNull(r("SUMASKING")) Then
                                sumAsking = r("SUMASKING")
                            End If

                            If Not IsDBNull(r("COUNTASKING")) Then
                                countAsking = r("COUNTASKING")
                            End If

                            Call Add_in_client_sale_prices("DLV", countAsking, sumAsking, CountSale, sumSale, temp_data, LOWASKING, HIGHASKING, AvgAsking1, LOWSALE, AVGSALE1, HIGHSALE, 0, 0)

                            If sumAsking > 0 And countAsking > 0 Then
                                avg_asking = Replace(FormatNumber(((sumAsking / countAsking) / 1000), 0), ",", "")
                                Call check_high_low(avg_asking, high_number, low_number)
                            Else
                                avg_asking = "null"
                            End If

                            'added MSW - to throw in client ones and sale ones together 
                            SALECOUNT += CountSale

                            If sumSale > 0 And SALECOUNT > 0 Then  ' changed from CountSale to SALECOUNT
                                avg_sale = Replace(FormatNumber(((sumSale / SALECOUNT) / 1000), 0), ",", "")
                                Call check_high_low(avg_sale, high_number, low_number)
                            Else
                                avg_sale = "null"
                            End If




                        ElseIf HttpContext.Current.Session.Item("localUser").crmUser_Evo_MPM_Flag = True Then
                            countAsking = 0
                            sumAsking = 0
                            CountSale = 0
                            sumSale = 0
                            SALECOUNT = 0
                            LOWASKING = 0
                            HIGHASKING = 0
                            AvgAsking1 = 0
                            LOWSALE = 0
                            AVGSALE1 = 0
                            HIGHSALE = 0


                            If Not IsDBNull(r("SUMASKING")) Then
                                sumAsking = r("SUMASKING")
                            End If

                            If Not IsDBNull(r("COUNTASKING")) Then
                                countAsking = r("COUNTASKING")
                            End If

                            Call Add_in_client_sale_prices("DLV", countAsking, sumAsking, CountSale, sumSale, temp_data, LOWASKING, HIGHASKING, AvgAsking1, LOWSALE, AVGSALE1, HIGHSALE, 0, 0)

                            If sumAsking > 0 And countAsking > 0 Then
                                avg_asking = Replace(FormatNumber(((sumAsking / countAsking) / 1000), 0), ",", "")
                                Call check_high_low(avg_asking, high_number, low_number)
                            Else
                                avg_asking = "null"
                            End If

                        Else

                        End If

                        If Trim(temp_data) = Trim(ac_dlv_year) Then
                            If row_added Then
                                scriptOut.Append(",['" & temp_data & "', " & temp_low & ", " & temp_avg & ", " & temp_high & ", " & avg_asking & ", " & avg_sale & ", " & temp_low & ", " & temp_avg & ", " & temp_high & "]")
                            Else
                                scriptOut.Append("['" & temp_data & "', " & temp_low & ", " & temp_avg & ", " & temp_high & ", " & avg_asking & ", " & avg_sale & ", " & temp_low & ", " & temp_avg & ", " & temp_high & "]")
                            End If
                        Else
                            If row_added Then
                                scriptOut.Append(",['" & temp_data & "', " & temp_low & ", " & temp_avg & ", " & temp_high & ", " & avg_asking & ", " & avg_sale & ", null, null, null]")
                            Else
                                scriptOut.Append("['" & temp_data & "', " & temp_low & ", " & temp_avg & ", " & temp_high & ", " & avg_asking & ", " & avg_sale & ", null, null, null]")
                            End If
                        End If


                        row_added = True


                    Next


                    ticks_string = "Y" ' so that is does the extra build
                    commonEvo.set_ranges_for_vsCharts(low_number, high_number, interval_point, starting_point, ending_point, ticks_string)


                    If Trim(from_pdf) = "Y" Then
                    Else
                        scriptOut.Append("]);" + vbCrLf)

                        scriptOut.Append("var options = { " + vbCrLf)

                        scriptOut.Append("  chartArea:{width:'" & IIf(miniGraph, "80", "76") & "%',height:'" & IIf(miniGraph, "68", "72") & "%'}," + vbCrLf)
                        scriptOut.Append("series: { ")

                        If show_evaules = True Then
                            scriptOut.Append("    0: { lineWidth: 1, pointSize: 1  , lineDashStyle: [4, 4] } ")
                            scriptOut.Append(" ,  1: { lineWidth: 2, pointSize: 2  , lineDashStyle: [4, 4] } ")
                            scriptOut.Append(" ,  2: { lineWidth: 1, pointSize: 1  , lineDashStyle: [4, 4] } ")
                        Else
                            scriptOut.Append("    0: { lineWidth: 1, pointSize: 1  , lineDashStyle: [4, 4], visibleInLegend: false } ")
                            scriptOut.Append(" ,  1: { lineWidth: 2, pointSize: 2  , lineDashStyle: [4, 4], visibleInLegend: false } ")
                            scriptOut.Append(" ,  2: { lineWidth: 1, pointSize: 1  , lineDashStyle: [4, 4], visibleInLegend: false } ")
                        End If


                        If show_asking = True Then
                            scriptOut.Append(" ,  3: { lineWidth: 0, pointSize: 3  } ")
                        Else
                            scriptOut.Append(" ,  3: { lineWidth: 0, pointSize: 3, visibleInLegend: false   } ")
                        End If

                        If show_sale = True Then
                            scriptOut.Append(" ,  4: { lineWidth: 0, pointSize: 3  } ")
                        Else
                            scriptOut.Append(" ,  4: { lineWidth: 0, pointSize: 3, visibleInLegend: false   } ")
                        End If


                        'If ac_mfr_year > 0 Then
                        '  scriptOut.Append(" ,  5: { lineWidth: 0, pointSize: 7, visibleInLegend: true  } ")
                        '  scriptOut.Append(" ,  6: { lineWidth: 0, pointSize: 7, visibleInLegend: true  } ")
                        '  scriptOut.Append(" ,  7: { lineWidth: 0, pointSize: 7, visibleInLegend: true  } ")
                        'Else
                        scriptOut.Append(" ,  5: { lineWidth: 0, pointSize: 7, visibleInLegend: false  } ")
                        scriptOut.Append(" ,  6: { lineWidth: 0, pointSize: 7, visibleInLegend: false  } ")
                        scriptOut.Append(" ,  7: { lineWidth: 0, pointSize: 7, visibleInLegend: false  } ")
                        ' End If

                        scriptOut.Append("  }  , ")
                        scriptOut.Append("  hAxis: { title: 'DLV Year'," + vbCrLf)
                        scriptOut.Append("           textStyle: { fontSize: " & IIf(miniGraph, "8", "10") & ", bold: false, italic: false }, " + vbCrLf)   'color: '#01579b',   , fontName:  'Arial'
                        scriptOut.Append("           titleTextStyle: {   fontSize: " & IIf(miniGraph, "8", "15") & ", bold: false, italic: false }" + vbCrLf)
                        scriptOut.Append("         }," + vbCrLf)
                        scriptOut.Append("  vAxis: { title: 'Price ($k)'," + vbCrLf)
                        scriptOut.Append("           textStyle: {fontSize: " & IIf(miniGraph, "8", "10") & ", bold: false, italic: false }," + vbCrLf)   ' color: '#1a237e', 
                        scriptOut.Append("           titleTextStyle: {fontSize: " & IIf(miniGraph, "8", "15") & ", bold: false, italic: false }" + vbCrLf)
                        If Trim(ticks_string) <> "" Then
                            scriptOut.Append(", ticks: [ " & ticks_string & "] ")
                        End If
                        scriptOut.Append("        }," + vbCrLf)
                        scriptOut.Append("  smoothLine:true," + vbCrLf)
                        scriptOut.Append(" legend: { position: 'top', textStyle:{fontSize:8}}, " + vbCrLf)
                        scriptOut.Append("  colors: ['" & value_color & "','" & value_color & "', '" & value_color & "', '#a3c28d', '#a84543']")
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
                htmlOut.Append("<table id=""flightActivityTable"" width=""100%"" cellspacing=""0"" cellpadding=""4"">")
                htmlOut.Append("<tr><td valign=""top"" align=""left""><div id='visualization" + graphID.ToString + "' style=""height:" & div_height.ToString & "px;""></div></td></tr>")
                htmlOut.Append("</table>" + vbCrLf)
            Else
                htmlOut.Append("<table id=""flightActivityTable"" width=""100%"" cellspacing=""0"" cellpadding=""4"">")
                htmlOut.Append("<tr><td valign=""middle"" align=""center"">No eValue estimates at this time for this make and model. ...</td></tr>")
                htmlOut.Append("</table>" + vbCrLf)
            End If

        Catch ex As Exception

            aError = "Error in views_display_flight_utilization_graph(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, Optional ByVal faa_date As String = """") " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_scriptString = scriptOut.ToString
        out_htmlString = htmlOut.ToString
        ticks_string_to_return = ticks_string
        htmlOut = Nothing
        results_table = Nothing

    End Sub
    Public Shared Sub Add_in_client_asking(ByVal type_of As String, ByRef avgCurrentAskingTotal As Long, ByRef avgCurrentAskingTotalCount As Long, ByRef minCurrentAsking As Long, ByRef maxCurrentAsking As Long, ByVal low_aftt As Long, ByVal high_aftt As Long)
        ' the 2 is the "current market select" 


        If HttpContext.Current.Session.Item("localUser").crmUser_Evo_MPM_Flag = True And valueControl.has_client_data = True Then

            If Trim(type_of) = "ASKING" Then
                If valueControl.array_count_current > 0 Then
                    For i = 0 To valueControl.array_count_current - 1
                        If valueControl.ac_asking_array_current(i) > 0 Then
                            avgCurrentAskingTotal += CLng(valueControl.ac_asking_array_current(i))
                            avgCurrentAskingTotalCount = avgCurrentAskingTotalCount + 1

                            If CLng(valueControl.ac_asking_array_current(i)) < minCurrentAsking Then
                                minCurrentAsking = CLng(valueControl.ac_asking_array_current(i))
                            End If

                            If CLng(valueControl.ac_asking_array_current(i)) > maxCurrentAsking Then
                                maxCurrentAsking = CLng(valueControl.ac_asking_array_current(i))
                            End If

                        End If
                    Next
                End If
            ElseIf Trim(type_of) = "AFTT" Then
                If valueControl.array_count_current > 0 Then
                    For i = 0 To valueControl.array_count_current - 1
                        If valueControl.ac_asking_array_current(i) > 0 Then
                            If valueControl.ac_asking_aftt_array(i) > low_aftt And valueControl.ac_asking_aftt_array(i) <= high_aftt Then
                                avgCurrentAskingTotal += CLng(valueControl.ac_asking_array_current(i))
                                avgCurrentAskingTotalCount = avgCurrentAskingTotalCount + 1

                                If CLng(valueControl.ac_asking_array_current(i)) < minCurrentAsking Then
                                    minCurrentAsking = CLng(valueControl.ac_asking_array_current(i))
                                End If

                                If CLng(valueControl.ac_asking_array_current(i)) > maxCurrentAsking Then
                                    maxCurrentAsking = CLng(valueControl.ac_asking_array_current(i))
                                End If
                            End If
                        End If
                    Next
                End If
            End If






        End If

    End Sub
    Public Shared Sub Add_in_client_sale_prices(ByVal to_do_by As String, ByRef countAsking As Long, ByRef sumAsking As Long, ByRef CountSale As Long, ByRef sumSale As Long, ByVal dlv_year As Long, ByRef LOWASKING As Long, ByRef HIGHASKING As Long, ByRef AvgAsking1 As Long, ByRef LOWSALE As Long, ByRef AVGSALE1 As Long, ByRef HIGHSALE As Long, ByVal aftt_low As Long, ByVal aftt_high As Long)

        If HttpContext.Current.Session.Item("localUser").crmUser_Evo_MPM_Flag = True And valueControl.has_client_data = True Then

            If Trim(to_do_by) = "DLV" Then        '-----------------------------------------------------------------------
                If valueControl.array_count > 0 Then
                    For i = 0 To valueControl.array_count - 1
                        If valueControl.ac_asking_array(i) > 0 And valueControl.ac_dlv_year_array(i) = dlv_year Then
                            sumAsking += CLng(valueControl.ac_asking_array(i))
                            countAsking = countAsking + 1

                            If CLng(valueControl.ac_asking_array(i)) < LOWASKING Then
                                LOWASKING = CLng(valueControl.ac_asking_array(i))
                            End If

                            If CLng(valueControl.ac_asking_array(i)) > HIGHASKING Then
                                HIGHASKING = CLng(valueControl.ac_asking_array(i))
                            End If
                        End If

                        If valueControl.ac_sold_array(i) > 0 And valueControl.ac_dlv_year_array(i) = dlv_year Then
                            sumSale += CLng(valueControl.ac_sold_array(i))
                            CountSale = CountSale + 1

                            If CLng(valueControl.ac_sold_array(i)) < LOWSALE Then
                                LOWSALE = CLng(valueControl.ac_sold_array(i))
                            End If

                            If CLng(valueControl.ac_sold_array(i)) > HIGHSALE Then
                                HIGHSALE = CLng(valueControl.ac_sold_array(i))
                            End If
                        End If

                    Next
                End If
            ElseIf Trim(to_do_by) = "AFTT" Or Trim(to_do_by) = "AFTT2" Then       '-----------------------------------------------------------------

                If valueControl.array_count > 0 Then
                    For i = 0 To valueControl.array_count - 1

                        If Trim(to_do_by) = "AFTT2" Then   ' only do in certain AFTT 
                            If valueControl.ac_asking_aftt_array(i) > aftt_low And valueControl.ac_asking_aftt_array(i) <= aftt_high Then
                                sumAsking += CLng(valueControl.ac_asking_array(i))
                                countAsking = countAsking + 1

                                If CLng(valueControl.ac_asking_array(i)) < LOWASKING Then
                                    LOWASKING = CLng(valueControl.ac_asking_array(i))
                                End If

                                If CLng(valueControl.ac_asking_array(i)) > HIGHASKING Then
                                    HIGHASKING = CLng(valueControl.ac_asking_array(i))
                                End If
                            End If
                        End If
                        If valueControl.ac_sold_aftt_array(i) > aftt_low And valueControl.ac_sold_aftt_array(i) <= aftt_high Then
                            sumSale += CLng(valueControl.ac_sold_array(i))
                            CountSale = CountSale + 1

                            If CLng(valueControl.ac_sold_array(i)) < LOWSALE Then
                                LOWSALE = CLng(valueControl.ac_sold_array(i))
                            End If

                            If CLng(valueControl.ac_sold_array(i)) > HIGHSALE Then
                                HIGHSALE = CLng(valueControl.ac_sold_array(i))
                            End If
                        End If

                    Next
                End If
            End If



        End If

    End Sub

    Public Function get_model_forsale_info_w_assett_summary(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()
    Dim AclsData_Temp As New clsData_Manager_SQL
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try


      sQuery.Append("SELECT ac_id, ac_ser_no_full, ac_delivery_date, ac_delivery, ac_aport_city, ac_aport_country, ac_ser_no_sort, ac_reg_no, ac_year, ac_mfr_year, ac_airframe_tot_hrs, ac_engine_1_tot_hrs, ac_engine_2_tot_hrs, ac_engine_3_tot_hrs, ac_engine_4_tot_hrs, ac_interior_moyear, ac_exterior_moyear,")
      sQuery.Append(" ac_list_date, ac_status, ac_asking, ac_asking_price, ac_passenger_count, ac_journ_id, amod_make_name, amod_model_name")


      sQuery.Append(", case when ac_est_airframe_hrs >  (((datediff(m, ac_times_as_of_date, getdate())+1) *  (ac_airframe_tot_hrs/case when datediff(m, ('6/1/' + ac_year), ac_times_as_of_date)=0 then 1 else datediff(m, ('6/1/' + ac_year), ac_times_as_of_date) end)) + ac_airframe_tot_hrs) then ac_est_airframe_hrs ")
      sQuery.Append(" else (((datediff(m, ac_times_as_of_date, getdate())+1) *  (ac_airframe_tot_hrs/case when datediff(m, ('6/1/' + ac_year), ac_times_as_of_date)=0 then 1 else datediff(m, ('6/1/' + ac_year), ac_times_as_of_date) end)) + ac_airframe_tot_hrs) end as ac_est_airframe_hrs  ")


      If HttpContext.Current.Session.Item("isMobile") = True Then
        sQuery.Append(" ,ac_picture_id,ac_aport_icao_code,ac_aport_iata_code, amod_airframe_type_code, amod_id, ac_forsale_flag, ac_delivery, ac_times_as_of_date, ")
        sQuery.Append(" ac_engine_1_soh_hrs, ac_engine_2_soh_hrs,ac_engine_3_soh_hrs,ac_engine_4_soh_hrs, ")
        sQuery.Append(" ac_last_event")
      End If

      sQuery.Append(" ,  (select top 1 afmv_value from Aircraft_FMV with (NOLOCK) where afmv_ac_id = ac_id and afmv_status='Y' and afmv_latest_flag='Y') as eValue  ")

      sQuery.Append(" , (select top 1 avg(afmv_value) from Aircraft_FMV with (NOLOCK)")
      sQuery.Append("   inner join Aircraft a2 with (NOLOCK) on a2.ac_id = afmv_ac_id and a2.ac_amod_id = " & searchCriteria.ViewCriteriaAmodID.ToString & " and a2.ac_year = Aircraft_Flat.ac_year ")
      sQuery.Append("   where   afmv_status='Y' and afmv_latest_flag='Y') as eValue_ModelAVG  ")


      If HttpContext.Current.Session.Item("localPreferences").UserSPIViewFlag = True Then
        sQuery.Append(", (select top 1 ac_sale_price From Aircraft b with (NOLOCK) ")
        sQuery.Append(" inner join Journal with (NOLOCK) on journ_id = ac_journ_id and ac_id = journ_ac_id ")
        sQuery.Append(" where(Aircraft_Flat.ac_id = b.ac_id) ")
        sQuery.Append(" and ac_sale_price > 0 and ac_sale_price_display_flag='Y' ")
        sQuery.Append(AclsData_Temp.add_in_wholesale_non_internal_retail_string())
        sQuery.Append(" order by journ_date desc) as  LASTSALEPRICE ")

        sQuery.Append(", (select top 1 journ_date From Aircraft b with (NOLOCK) ")
        sQuery.Append(" inner join Journal with (NOLOCK) on journ_id = ac_journ_id and ac_id = journ_ac_id ")
        sQuery.Append(" where Aircraft_Flat.ac_id = b.ac_id and ac_sale_price > 0 and ac_sale_price_display_flag='Y'  ")
        sQuery.Append(AclsData_Temp.add_in_wholesale_non_internal_retail_string())

        sQuery.Append(" order by journ_date desc) as  LASTSALEPRICEDATE ")

      End If


      ' changed msw = 7/28/16 per request
      'sQuery.Append(" FROM Aircraft WITH(NOLOCK) INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id")
      sQuery.Append(" FROM Aircraft_Flat WITH(NOLOCK) ")

      If searchCriteria.ViewCriteriaAmodID > -1 Then
        sQuery.Append(" WHERE amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString + " AND ac_journ_id = 0")
      ElseIf searchCriteria.ViewCriteriaMakeAmodID > -1 Then
        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
          sQuery.Append(" WHERE amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "' AND ac_journ_id = 0")
        End If
      End If
      If searchCriteria.ViewCriteriaAFTTStart > 0 Then
        sQuery.Append(Constants.cAndClause + " ((ac_airframe_tot_hrs >= " & searchCriteria.ViewCriteriaAFTTStart & ") or (ac_airframe_tot_hrs IS NULL))")
      End If

      If searchCriteria.ViewCriteriaAFTTEnd > 0 Then
        sQuery.Append(Constants.cAndClause + " ((ac_airframe_tot_hrs <= " & searchCriteria.ViewCriteriaAFTTEnd & ") or (ac_airframe_tot_hrs IS NULL))")
      End If


      If searchCriteria.ViewCriteriaYearStart > 0 Then
        sQuery.Append(Constants.cAndClause + " ac_year >= " & searchCriteria.ViewCriteriaYearStart)
      End If


      If searchCriteria.ViewCriteriaYearEnd > 0 Then
        sQuery.Append(Constants.cAndClause + " ac_year <=  " & searchCriteria.ViewCriteriaYearEnd)
      End If
      sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))

      sQuery.Append(" AND ac_forsale_flag = 'Y'")

      Select Case (searchCriteria.ViewCriteriaSortBy.ToLower)
        Case "serno"
          sQuery.Append(" ORDER BY ac_ser_no_sort, ac_list_date, ac_airframe_tot_hrs, ac_mfr_year, ac_year, ac_asking_price desc, ac_asking asc")

        Case "aftt"
          sQuery.Append(" ORDER BY ac_airframe_tot_hrs, ac_ser_no_sort, ac_list_date, ac_mfr_year, ac_year, ac_asking_price desc, ac_asking asc")

        Case "mfryear"
          sQuery.Append(" ORDER BY ac_mfr_year, ac_ser_no_sort, ac_list_date, ac_airframe_tot_hrs, ac_year, ac_asking_price desc, ac_asking asc")

        Case "acyear"
          sQuery.Append(" ORDER BY ac_year, ac_ser_no_sort, ac_list_date, ac_airframe_tot_hrs, ac_mfr_year, ac_asking_price desc, ac_asking asc")

        Case "listdate"
          sQuery.Append(" ORDER BY ac_list_date, ac_ser_no_sort, ac_airframe_tot_hrs, ac_mfr_year, ac_year, ac_asking_price desc, ac_asking asc")

        Case "asking"
          sQuery.Append(" ORDER BY ac_asking_price desc, ac_asking asc, ac_list_date, ac_ser_no_sort, ac_airframe_tot_hrs, ac_mfr_year, ac_year")

        Case Else
          sQuery.Append(" ORDER BY ac_ser_no_sort, ac_list_date, ac_airframe_tot_hrs, ac_mfr_year, ac_year, ac_asking_price desc, ac_asking asc")

      End Select

      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

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
        aError = "Error in get_model_forsale_info load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_model_forsale_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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
  Public Sub views_display_market_for_Sale_assett_prices_graph(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef table_string As String, ByVal temp_header As String)

    Dim htmlOut As New StringBuilder
    Dim scriptOut As New StringBuilder
    Dim results As New StringBuilder
    Dim results_table As New DataTable

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
    Dim ReturnString As String = ""
    Dim GraphStr As String = ""
    Dim eValue As String = ""
    Dim Asking As String = ""
    Dim SerNo As String = ""
    Dim eValue_ModelAVG As String = ""
    Dim bHadStatus As Boolean = False
    Dim localDataLayer As New viewsDataLayer
    Dim temp_counter As Integer = 0

    Try

      results_table = get_model_forsale_info_w_assett_summary(searchCriteria)

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          Dim table_color As String = "blue"
          results.Append("<tr><td valign='top'><div class=""Box""><table id=""weightTable"" cellpadding='3'  cellspacing='0' width='100%' class='formatTable " & table_color & "'><thead>")
          results.Append("<tr class=""noBorder"">")
          results.Append("<th class='left'>SER NO</th>")
          results.Append("<th class='right'>REG NO</th>")
          results.Append("<th class='right'>DLV YEAR</th>")
          results.Append("<th class='right'>ASKING</th>")
          results.Append("<th class='right'>" & value_label & " ($k)</th>")
          results.Append("<th class='right'>AVG MODEL<br/>YEAR " & value_label & " ($k)</th>")
          results.Append("<th class='right'>EST AFTT</th>")
          results.Append("</tr>")

          results.Append("</thead>")
          results.Append("<tbody>")

          For Each r As DataRow In results_table.Rows

            temp_counter += 1

            If temp_counter > 40 Then

              results.Append("</tbody></table></div></td></tr>")

              results.Append(temp_header)

              results.Append("<tr><td valign='top'><div class=""Box""><table id=""weightTable"" cellpadding='3'  cellspacing='0' width='100%' class='formatTable " & table_color & "'><thead>")
              results.Append("<tr class=""noBorder"">")
              results.Append("<th class='left'>SER NO</th>")
              results.Append("<th class='right'>REG NO</th>")
              results.Append("<th class='right'>DLV YEAR</th>")
              results.Append("<th class='right'>STATUS</th>")
              results.Append("<th class='right'>" & value_label & " ($k)</th>")
              results.Append("<th class='right'>AVG MODEL MFR<br/>YEAR " & value_label & " ($k)</th>")
              results.Append("<th class='right'>EST AFTT</th>")
              results.Append("</tr>")

              results.Append("</thead>")
              results.Append("<tbody>")
              temp_counter = 0
            End If


            If Not IsDBNull(r("ac_ser_no_full")) Then
              SerNo = r("ac_ser_no_full").ToString
            End If

            If Not IsDBNull(r("ac_asking_price")) Then
              Asking = FormatNumber((r("ac_asking_price") / 1000), 0).ToString
            Else
              Asking = "null"
            End If

            If Not IsDBNull(r("eValue")) Then
              eValue = FormatNumber((r("eValue") / 1000), 0).ToString
            Else
              eValue = "null"
            End If

            If Not IsDBNull(r("eValue_ModelAVG")) Then
              eValue_ModelAVG = FormatNumber((r("eValue_ModelAVG") / 1000), 0).ToString
            Else
              eValue_ModelAVG = "null"
            End If



            If Not String.IsNullOrEmpty(Asking) Then

              results.Append("<tr>")
              results.Append("<td align=""right""><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>")
              results.Append(SerNo)
              results.Append("</font></td>")

              results.Append("<td align=""right""><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>")
              If Not IsDBNull(r("ac_reg_no")) Then
                results.Append(r("ac_reg_no"))
              Else
                results.Append("&nbsp;")
              End If
              results.Append("</font></td>")

              results.Append("<td align=""right""><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>")
              If Not IsDBNull(r("ac_year")) Then
                results.Append(r("ac_year"))
              Else
                results.Append("&nbsp;")
              End If
              results.Append("</font></td>")

              results.Append("<td align=""right""><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>")

              bHadStatus = False
              If Not IsDBNull(r("ac_Status")) Then
                If Not String.IsNullOrEmpty(r.Item("ac_Status").ToString) Then
                  If r.Item("ac_Status").ToString.ToLower.Trim.Contains("for sale") Then
                    results.Append(localDataLayer.forsale_status(r.Item("ac_Status").ToString.Trim))
                    bHadStatus = True
                  End If
                End If
              End If

              If bHadStatus Then
                results.Append("&nbsp;")
              End If

              If Not IsDBNull(r("ac_asking")) Then
                If r.Item("ac_asking").ToString.ToLower.Trim.Trim.Contains("price") Then
                  If Not IsDBNull(r("ac_asking_price")) Then
                    If CDbl(r.Item("ac_asking_price").ToString) > 0 Then
                      results.Append("$" + FormatNumber((CDbl(r.Item("ac_asking_price").ToString) / 1000), 0).ToString + "")
                    End If
                  End If
                Else
                  results.Append(localDataLayer.forsale_status(r.Item("ac_asking").ToString.Trim))
                End If
              End If


              results.Append("</font></td>")
              results.Append("<td align=""right""><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>")
              If Trim(eValue) = "null" Then
                results.Append("&nbsp;")
              Else
                results.Append("$" & eValue)
              End If
              results.Append("</font></td>")
              results.Append("<td align=""right""><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>")
              If Trim(eValue_ModelAVG) = "null" Then
                results.Append("&nbsp;")
              Else
                results.Append("$" & eValue_ModelAVG)
              End If
              results.Append("</font></td>")


              results.Append("<td align=""right""><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>")
              If Not IsDBNull(r("ac_est_airframe_hrs")) Then
                results.Append(FormatNumber(r("ac_est_airframe_hrs"), 0))
              Else
                results.Append("&nbsp;")
              End If
              results.Append("</font></td>")

              results.Append("</tr>")
            End If

          Next

          results.Append("</tbody></table></div></td></tr>")

        End If
      End If

      table_string = results.ToString


    Catch ex As Exception

      aError = "Error in views_display_flight_utilization_graph(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, Optional ByVal faa_date As String = """") " + ex.Message

    Finally

    End Try

  End Sub
  Public Sub views_display_current_market_and_assett_prices_graph(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_scriptString As String, ByRef out_htmlString As String, ByVal graphID As Integer, Optional ByVal faa_date As String = "", Optional ByVal bFromUtilizationTab As Boolean = False, Optional ByVal ValuePDF As Boolean = False, Optional ByVal div_height As Integer = 295, Optional ByVal ac_dlv_year As Integer = 0, Optional ByVal show_asking As Boolean = True, Optional ByVal show_sale As Boolean = True, Optional ByVal show_evaules As Boolean = True, Optional ByVal order_by_string As String = "", Optional ByVal from_pdf As String = "N", Optional ByRef table_string As String = "", Optional ByRef ticks_string_to_return As String = "", Optional ByVal VariantList As String = "", Optional ByVal YearOne As String = "", Optional ByVal YearTwo As String = "", Optional ByVal forsaleFlag As String = "", Optional ByVal regType As String = "", Optional ByVal afttStart As String = "", Optional ByVal afttEnd As String = "", Optional ByVal miniGraph As Boolean = False)

    Dim htmlOut As New StringBuilder
    Dim scriptOut As New StringBuilder
    Dim results As New StringBuilder
    Dim results_table As New DataTable

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
    Dim ReturnString As String = ""
    Dim GraphStr As String = ""
    Dim eValue As String = ""
    Dim Asking As String = ""
    Dim SerNo As String = ""
    Dim eValue_ModelAVG As String = ""
        Dim ac_year As Long = 0
        Dim client_asking As Integer = 0
        Dim k As Integer = 0

        Try

      results_table = GetAircraftCurrentMarket(searchCriteria.ViewCriteriaAmodID, "", order_by_string, YearOne, YearTwo, forsaleFlag, regType, afttStart, afttEnd, VariantList)



      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          Dim table_color As String = "blue"
          results.Append("<tr><td valign='top'><div class=""Box""><table id=""weightTable"" cellpadding='3'  cellspacing='0' width='100%' class='formatTable " & table_color & "'><thead>")
          results.Append("<tr class=""noBorder"">")
          results.Append("<th class='left'>SERIAL NUMBER</th>")
          results.Append("<th class='right'>ASKING PRICE ($k)</th>")
          results.Append("<th class='right'>" & value_label & " ($k)</th>")
          results.Append("<th class='right'>AVG MODEL YEAR " & value_label & " ($k)</th>")
          results.Append("</tr>")

          results.Append("</thead>")
          results.Append("<tbody>")



          If Trim(from_pdf) = "Y" Then
          Else
            scriptOut.Append("function drawVisualization" + graphID.ToString + "() {" + vbCrLf)
            'scriptOut.Append(" alert('drawVisualization" + graphID.ToString + "');" + vbCrLf)  
            scriptOut.Append(" var data" + graphID.ToString + " = new google.visualization.DataTable();" + vbCrLf)
          End If

          scriptOut.Append("data" + graphID.ToString + ".addColumn('string', 'Serial Number'); ")
          scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', 'Asking Price'); ")
          scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', '" & value_label & "'); ")
          scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', 'Avg Model Year " & value_label & "'); ")
          scriptOut.Append(" data" + graphID.ToString + ".addRows([")

          For Each r As DataRow In results_table.Rows
            If Not IsDBNull(r("ac_ser_no")) Then
              SerNo = r("ac_ser_no").ToString
            End If

            If Not IsDBNull(r("ac_year")) Then
              ac_year = CLng(r("ac_year").ToString)
            End If


                        client_asking = 0
                        If HttpContext.Current.Session.Item("localUser").crmUser_Evo_MPM_Flag = True Then
                            If valueControl.array_count_current > 0 Then
                                For k = 0 To valueControl.array_count_current - 1
                                    If valueControl.ac_asking_array_current(k) > 0 And valueControl.ac_id_array_current(k) = r("ac_id") Then
                                        client_asking = valueControl.ac_asking_array_current(k)
                                    End If
                                    k = valueControl.array_count_current
                                Next
                            End If
                        End If

                        If client_asking > 0 Then
                            Asking = FormatNumber((client_asking / 1000), 0).ToString
                            Call check_high_low(Asking, high_number, low_number)
                        Else
                            If Not IsDBNull(r("ac_asking_price")) Then
                                Asking = FormatNumber((r("ac_asking_price") / 1000), 0).ToString
                                Call check_high_low(Asking, high_number, low_number)
                            Else
                                Asking = "null"
                            End If
                        End If

                        If Not IsDBNull(r("eValue")) Then
              If CLng(r("eValue")) > 0 Then
                eValue = FormatNumber((r("eValue") / 1000), 0).ToString
                Call check_high_low(eValue, high_number, low_number)
              Else
                eValue = "null"
              End If
            Else
              eValue = "null"
            End If

              If Not IsDBNull(r("eValue_ModelAVG")) Then
                eValue_ModelAVG = FormatNumber((r("eValue_ModelAVG") / 1000), 0).ToString
                Call check_high_low(eValue_ModelAVG, high_number, low_number)
              Else
                eValue_ModelAVG = "null"
              End If

              '   If Trim(order_by_string) = "" Or Trim(order_by_string) = "Serial Number" Then
              '    temp_data = SerNo & "  (Year: " & ac_year & ") "
              '   ElseIf Trim(order_by_string) = "Year" Then
              '    temp_data = ac_year
              '   Else
              temp_data = SerNo
              '   End If


              If Not String.IsNullOrEmpty(Asking) Then

                If row_added = True Then
                  horizontal_tick_string &= ", "
                End If

                horizontal_tick_string &= "'" & temp_data & "'"

                results.Append("<tr>")
                results.Append("<td align=""right""><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>")
                results.Append(temp_data)
                results.Append("</font></td>")
                results.Append("<td align=""right""><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>")
                If Trim(temp_data) = "null" Then
                  results.Append("&nbsp;")
                Else
                  results.Append(Asking)
                End If
                results.Append("</font></td>")
                results.Append("<td align=""right""><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>")
                If Trim(eValue) = "null" Then
                  results.Append("&nbsp;")
                Else
                  results.Append(eValue)
                End If
                results.Append("</font></td>")
                results.Append("<td align=""right""><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>")
                If Trim(eValue_ModelAVG) = "null" Then
                  results.Append("&nbsp;")
                Else
                  results.Append(eValue_ModelAVG)
                End If
                results.Append("</font></td>")
                results.Append("</tr>")


                If ReturnString <> "" Then
                  scriptOut.Append(", ")
                End If
                scriptOut.Append("['" & temp_data & "', " & Replace(Asking, ",", "") & ", " & Replace(eValue, ",", "") & ", " & Replace(eValue_ModelAVG, ",", "") & "]")
                ReturnString = "-"  ' easy way just to add comma 

                row_added = True
              End If

          Next

          results.Append("</tbody></table></div></td></tr>")

        End If
      End If

      table_string = results.ToString

      '  results_table = get_assett_summary(searchCriteria, faa_date, bFromUtilizationTab)

      'If Not IsNothing(results_table) Then

      '  If results_table.Rows.Count > 0 Then

      '    scriptOut.Append("function drawVisualization" + graphID.ToString + "() {" + vbCrLf)
      '    'scriptOut.Append(" alert('drawVisualization" + graphID.ToString + "');" + vbCrLf)  
      '    scriptOut.Append(" var data" + graphID.ToString + " = new google.visualization.DataTable();" + vbCrLf)


      '    scriptOut.Append("data" + graphID.ToString + ".addColumn('string', 'MFR Year'); ")
      '    scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', 'Low eValue'); ")
      '    scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', 'Avg eValue'); ")
      '    scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', 'High eValue'); ")
      '    scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', 'Avg Asking'); ")
      '    scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', 'Avg Sale'); ")
      '    scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', '" & ac_mfr_year & " Low eValue'); ")
      '    scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', '" & ac_mfr_year & " Avg eValue'); ")
      '    scriptOut.Append(" data" + graphID.ToString + ".addColumn('number', '" & ac_mfr_year & " High eValue'); ")
      '    scriptOut.Append(" data" + graphID.ToString + ".addRows([")

      '    For Each r As DataRow In results_table.Rows

      '      temp_data = "null"
      '      temp_low = "null"
      '      temp_avg = "null"
      '      temp_high = "null"
      '      avg_asking = "null"
      '      avg_sale = "null"

      '      ' If Not IsDBNull(r("amod_make_name")) And Not IsDBNull(r("amod_model_name")) Then 
      '      '   temp_data = r.Item("amod_make_name") & " " & r("amod_model_name")  
      '      ' Else
      '      '   temp_data = ""
      '      ' End If

      '      If Not IsDBNull(r("ac_mfr_year")) Then
      '        temp_data = r.Item("ac_mfr_year")
      '      Else
      '        temp_data = "null"
      '      End If

      '      If Not IsDBNull(r("LOWVALUE")) And show_evaules = True Then
      '        temp_low = r.Item("LOWVALUE")
      '        If CLng(temp_low) > 0 Then
      '          temp_low = CLng(temp_low / 1000)
      '          Call check_high_low(temp_low, high_number, low_number)
      '        End If
      '      Else
      '        temp_low = "null"
      '      End If

      '      If Not IsDBNull(r("AVGVALUE")) And show_evaules = True Then
      '        temp_avg = r.Item("AVGVALUE")
      '        If CLng(temp_avg) > 0 Then
      '          temp_avg = CLng(temp_avg / 1000)
      '          Call check_high_low(temp_avg, high_number, low_number)
      '        End If
      '      Else
      '        temp_avg = "null"
      '      End If

      '      If Not IsDBNull(r("HIGHVALUE")) And show_evaules = True Then
      '        temp_high = r.Item("HIGHVALUE")
      '        If CLng(temp_high) > 0 Then
      '          temp_high = CLng(temp_high / 1000)
      '          Call check_high_low(temp_high, high_number, low_number)
      '        End If
      '      Else
      '        temp_high = "null"
      '      End If


      '      If Not IsDBNull(r("avg_asking")) And show_asking = True Then
      '        avg_asking = r.Item("avg_asking")
      '        If CLng(avg_asking) > 0 Then
      '          avg_asking = CLng(avg_asking / 1000)
      '          Call check_high_low(avg_asking, high_number, low_number)
      '        End If
      '      Else
      '        avg_asking = "null"
      '      End If


      '      If Not IsDBNull(r("avg_sale")) And show_sale = True Then
      '        avg_sale = r.Item("avg_sale")
      '        If CLng(avg_sale) > 0 Then
      '          avg_sale = CLng(avg_sale / 1000)
      '          Call check_high_low(avg_sale, high_number, low_number)
      '        End If
      '      Else
      '        avg_sale = "null"
      '      End If

      '      If Trim(temp_data) = Trim(ac_mfr_year) Then
      '        If row_added Then
      '          scriptOut.Append(",['" & temp_data & "', " & temp_low & ", " & temp_avg & ", " & temp_high & ", " & avg_asking & ", " & avg_sale & ", " & temp_low & ", " & temp_avg & ", " & temp_high & "]")
      '        Else
      '          scriptOut.Append("['" & temp_data & "', " & temp_low & ", " & temp_avg & ", " & temp_high & ", " & avg_asking & ", " & avg_sale & ", " & temp_low & ", " & temp_avg & ", " & temp_high & "]")
      '        End If
      '      Else
      '        If row_added Then
      '          scriptOut.Append(",['" & temp_data & "', " & temp_low & ", " & temp_avg & ", " & temp_high & ", " & avg_asking & ", " & avg_sale & ", null, null, null]")
      '        Else
      '          scriptOut.Append("['" & temp_data & "', " & temp_low & ", " & temp_avg & ", " & temp_high & ", " & avg_asking & ", " & avg_sale & ", null, null, null]")
      '        End If
      '      End If


      '      row_added = True


      '    Next


      ticks_string = "Y" ' so that is does the extra build
      commonEvo.set_ranges_for_vsCharts(low_number, high_number, interval_point, starting_point, ending_point, ticks_string)

      If Trim(from_pdf) = "Y" Then
      Else

        scriptOut.Append("]);" + vbCrLf)

                scriptOut.Append("var options = { " + vbCrLf)
                scriptOut.Append("  chartArea:{width:'" & IIf(miniGraph, "80", "76") & "%',height:'" & IIf(miniGraph, "58", "72") & "%'}," + vbCrLf)
                scriptOut.Append("series: { ")

        ' If show_evaules = True Then
        scriptOut.Append("    0: { lineWidth: 0, pointSize: 4  , lineDashStyle: [4, 4] } ")
        scriptOut.Append(" ,  1: { lineWidth: 0, pointSize: 4  , lineDashStyle: [4, 4] } ")
        scriptOut.Append(" ,  2: { lineWidth: 1, pointSize: 3  , lineDashStyle: [4, 4] } ")
        '  Else
        '  scriptOut.Append("    0: { lineWidth: 1, pointSize: 1  , lineDashStyle: [4, 4], visibleInLegend: false } ")
        '  scriptOut.Append(" ,  1: { lineWidth: 2, pointSize: 2  , lineDashStyle: [4, 4], visibleInLegend: false } ")
        '  scriptOut.Append(" ,  2: { lineWidth: 1, pointSize: 1  , lineDashStyle: [4, 4], visibleInLegend: false } ")
        ' End If


        'If show_asking = True Then
        '  scriptOut.Append(" ,  3: { lineWidth: 0, pointSize: 3  } ")
        'Else
        '  scriptOut.Append(" ,  3: { lineWidth: 0, pointSize: 3, visibleInLegend: false   } ")
        'End If

        'If show_sale = True Then
        '  scriptOut.Append(" ,  4: { lineWidth: 0, pointSize: 3  } ")
        'Else
        '  scriptOut.Append(" ,  4: { lineWidth: 0, pointSize: 3, visibleInLegend: false   } ")
        'End If


        'scriptOut.Append(" ,  5: { lineWidth: 0, pointSize: 7, visibleInLegend: false  } ")
        'scriptOut.Append(" ,  6: { lineWidth: 0, pointSize: 7, visibleInLegend: false  } ")
        'scriptOut.Append(" ,  7: { lineWidth: 0, pointSize: 7, visibleInLegend: false  } ")


        scriptOut.Append("  }  , ")
        scriptOut.Append("  hAxis: { title: 'Serial Number'," + vbCrLf) 

        '   scriptOut.Append("           textStyle: {  fontSize: " & IIf(miniGraph, "8", "10") & ", bold: false, italic: false}, slantedText:true, slantedTextAngle:10, " + vbCrLf)    'color: '#01579b', fontName:  'Arial'
        '   scriptOut.Append("           textStyle: {  fontSize: " & IIf(miniGraph, "8", "10") & ", bold: false, italic: false}, slantedText:true, slantedTextAngle:30, " + vbCrLf)    'color: '#01579b', fontName:  'Arial'
        scriptOut.Append("           textStyle: {  fontSize: " & IIf(miniGraph, "8", "10") & ", bold: false, italic: false}, slantedText:true, slantedTextAngle:70, " + vbCrLf)    'color: '#01579b', fontName:  'Arial'
        '   scriptOut.Append("           textStyle: {  fontSize: " & IIf(miniGraph, "8", "10") & ", bold: false, italic: false} , slantedText:true, slantedTextAngle:90 , " + vbCrLf)    'color: '#01579b', fontName:  'Arial'


        scriptOut.Append("           titleTextStyle: {   fontSize: " & IIf(miniGraph, "8", "15") & ", bold: false, italic: false }" + vbCrLf)  ' color: '#01579b', fontName:  'Arial'
        ' If Trim(horizontal_tick_string) <> "" Then
        ' scriptOut.Append(", ticks: [ " & horizontal_tick_string & "] ")
        ' End If
        scriptOut.Append("         }," + vbCrLf)
        scriptOut.Append("  vAxis: { title: 'Price ($k)'," + vbCrLf)
        scriptOut.Append("           textStyle: { fontSize: " & IIf(miniGraph, "8", "10") & ", bold: false, italic: false  }," + vbCrLf)   ' color: '#1a237e',
        scriptOut.Append("           titleTextStyle: {  fontSize: " & IIf(miniGraph, "8", "15") & ", bold: false, italic: false  }" + vbCrLf)  'color: '#1a237e',
        If Trim(ticks_string) <> "" Then
          scriptOut.Append(", ticks: [ " & ticks_string & "] ")
        End If
        scriptOut.Append("        }," + vbCrLf)
        scriptOut.Append("  smoothLine:true," + vbCrLf)
        scriptOut.Append(" legend: { position: 'top', textStyle:{fontSize:8}}, " + vbCrLf)
        scriptOut.Append("  colors: ['#a3c28d','" & value_color & "', '#B7B7B7', '#a3c28d', '#a84543']")

        'for referance
        ' scriptOut.Append("  colors: ['#B7DCF6','#B7DCF6', '#B7DCF6', '#a3c28d', '#a84543']")
        'scriptOut.Append("  colors: ['#blue','#blue', '#blue', '#green', '#red']")
        'eba059 orange 
        ' B7B7B7 grey 
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

      If Not String.IsNullOrEmpty(scriptOut.ToString.Trim) Then
        htmlOut.Append("<table id=""flightActivityTable"" width=""100%"" cellspacing=""0"" cellpadding=""4"">")
        htmlOut.Append("<tr><td valign=""top"" align=""left""><div id='visualization" + graphID.ToString + "' style=""height:" & div_height.ToString & "px;""></div></td></tr>")
        htmlOut.Append("</table>" + vbCrLf)
      Else
        htmlOut.Append("<table id=""flightActivityTable"" width=""100%"" cellspacing=""0"" cellpadding=""4"">")
        htmlOut.Append("<tr><td valign=""middle"" align=""center"">No eValue estimates at this time for this make and model. ...</td></tr>")
        htmlOut.Append("</table>" + vbCrLf)
      End If

    Catch ex As Exception

      aError = "Error in views_display_flight_utilization_graph(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, Optional ByVal faa_date As String = """") " + ex.Message

    Finally

    End Try

    'return resulting html string
    out_scriptString = scriptOut.ToString
    out_htmlString = htmlOut.ToString
    ticks_string_to_return = ticks_string
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Sub check_high_low(ByVal temp_num As Long, ByRef high_number As Long, ByRef low_number As Long)

    If temp_num > high_number Then
      high_number = temp_num
    End If

    If temp_num < low_number Then
      low_number = temp_num
    End If

  End Sub

  Public Sub check_first_last(ByVal first_date As Long, ByRef high_number As Long, ByRef low_number As Long)




  End Sub


#End Region

End Class
