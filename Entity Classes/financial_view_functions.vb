' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/Entity Classes/financial_view_functions.vb $
'$$Author: Amanda $
'$$Date: 8/30/19 11:05a $
'$$Modtime: 8/30/19 10:16a $
'$$Revision: 3 $
'$$Workfile: financial_view_functions.vb $
'
' ********************************************************************************

<System.Serializable()> Public Class financial_view_functions

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


#Region "Financial Intel Display/Queries"
  '
 
 


  Public Function GetDocsByMonth(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal use_insight_roll As Boolean)
    Dim sql As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim atemptable As New DataTable
    Try
      If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("jetnetClientDatabase")) Then
        If searchCriteria.ViewCriteriaCompanyID > 0 Then

          sql = "SELECT DISTINCT YEAR(adoc_doc_date) AS tyear, MONTH(adoc_doc_date) AS tmonth, count(distinct adoc_journ_id + adoc_journ_seq_no) AS tcount "
          sql += " FROM Aircraft_Document WITH(NOLOCK) INNER JOIN Aircraft WITH(NOLOCK) ON ac_id = adoc_ac_id AND ac_journ_id = adoc_journ_id "
          sql += " INNER JOIN aircraft_model WITH(NOLOCK) ON ac_amod_id = amod_id "
          sql += " INNER JOIN Journal WITH(NOLOCK) ON ac_journ_id = journ_id "
          sql += " INNER JOIN company WITH(NOLOCK) ON adoc_infavor_comp_id = comp_id and comp_journ_id = 0 "
          sql += " LEFT OUTER JOIN Financial_Institution_Company_Reference WITH(NOLOCK) ON ficr_sub_comp_id = adoc_infavor_comp_id "
          sql += " LEFT OUTER JOIN Financial_Institution_Primary_Group WITH(NOLOCK) ON fipg_main_comp_id = ficr_main_comp_id "
          sql += " WHERE ((adoc_doc_date >= '" & Month(DateAdd(DateInterval.Month, -12, Now())) & "/" & Day(DateAdd(DateInterval.Month, -12, Now())) & "/" & Year(DateAdd(DateInterval.Month, -12, Now())) & "') AND (adoc_doc_date < '" & Month(Now()) & "/" & Day(Now()) & "/" & Year(Now()) & "')) "

          If use_insight_roll Then
            sql += " AND ficr_main_comp_id in (select distinct ficr_main_comp_id from Financial_Institution_Company_Reference with (NOLOCK)"
            sql += " where ficr_sub_comp_id = " & searchCriteria.ViewCriteriaCompanyID.ToString & ")"
          Else
            sql += " and ficr_sub_comp_id = " & searchCriteria.ViewCriteriaCompanyID.ToString
          End If

          sql += " AND journ_internal_trans_flag = 'N' AND journ_subcat_code_part2 NOT LIKE 'IT' "
          sql += Constants.cSingleSpace + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, True)

          sql += " GROUP BY YEAR(adoc_doc_date), MONTH(adoc_doc_date) "
          sql += " ORDER BY YEAR(adoc_doc_date) ASC, MONTH(adoc_doc_date) ASC"



          HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b> GetRelatedDocs(ByRef searchCriteria As viewSelectionCriteriaClass)</b><br />" & sql


          SqlConn.ConnectionString = clientConnectStr
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
      End If

      Return atemptable
    Catch ex As Exception
      GetDocsByMonth = Nothing
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

  End Function


  Public Function GetRelatedDocs(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal use_insight_roll As Boolean)
    Dim sql As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim atemptable As New DataTable
    Try
      If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("jetnetClientDatabase")) Then
        If searchCriteria.ViewCriteriaCompanyID > 0 Then
          sql = "SELECT DISTINCT comp_name, comp_city, comp_state, comp_country, comp_id, count(distinct adoc_journ_id + adoc_journ_seq_no) AS tcount "
          sql += " FROM Aircraft_Document WITH(NOLOCK) INNER JOIN Aircraft WITH(NOLOCK) ON ac_id = adoc_ac_id AND ac_journ_id = adoc_journ_id "
          sql += " INNER JOIN aircraft_model WITH(NOLOCK) ON ac_amod_id = amod_id "
          sql += " INNER JOIN Journal WITH(NOLOCK) ON ac_journ_id = journ_id "
          sql += " INNER JOIN company WITH(NOLOCK) ON adoc_infavor_comp_id = comp_id and comp_journ_id = 0 "
          sql += " LEFT OUTER JOIN Financial_Institution_Company_Reference WITH(NOLOCK) ON ficr_sub_comp_id = adoc_infavor_comp_id "
          sql += " LEFT OUTER JOIN Financial_Institution_Primary_Group WITH(NOLOCK) ON fipg_main_comp_id = ficr_main_comp_id "
          sql += " WHERE ((adoc_doc_date >= '" & Month(DateAdd(DateInterval.Month, -12, Now())) & "/" & Day(DateAdd(DateInterval.Month, -12, Now())) & "/" & Year(DateAdd(DateInterval.Month, -12, Now())) & "') AND (adoc_doc_date < '" & Month(Now()) & "/" & Day(Now()) & "/" & Year(Now()) & "')) "

          If use_insight_roll Then
            sql += " AND ficr_main_comp_id in (select distinct ficr_main_comp_id from Financial_Institution_Company_Reference with (NOLOCK)"
            sql += " where ficr_sub_comp_id = " & searchCriteria.ViewCriteriaCompanyID.ToString & ")"
          Else
            sql += " and ficr_sub_comp_id =" & searchCriteria.ViewCriteriaCompanyID.ToString
          End If
          sql += " AND journ_internal_trans_flag = 'N' AND journ_subcat_code_part2 NOT LIKE 'IT' "
          sql += Constants.cSingleSpace + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, True)

          sql += " GROUP BY comp_name, comp_city, comp_state, comp_country, comp_id"
          sql += " ORDER BY comp_name, comp_city, comp_state, comp_country, comp_id"


          HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b> GetRelatedDocs(ByRef searchCriteria As viewSelectionCriteriaClass)</b><br />" & sql


          SqlConn.ConnectionString = clientConnectStr
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
      End If

      Return atemptable
    Catch ex As Exception
      GetRelatedDocs = Nothing
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

  End Function
  Public Function GetModelDocuments(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal use_insight_roll As Boolean)
    Dim sql As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim atemptable As New DataTable
    Try
      If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("jetnetClientDatabase")) Then
        If searchCriteria.ViewCriteriaCompanyID > 0 Then
          sql = "SELECT amod_make_name, amod_model_name, amod_id, count(distinct adoc_journ_id + adoc_journ_seq_no) AS tcount "
          sql += " FROM Aircraft_Document WITH(NOLOCK) INNER JOIN Aircraft WITH(NOLOCK) ON ac_id = adoc_ac_id AND ac_journ_id = adoc_journ_id "
          sql += " INNER JOIN aircraft_model WITH(NOLOCK) ON ac_amod_id = amod_id "
          sql += " INNER JOIN Journal WITH(NOLOCK) ON ac_journ_id = journ_id "
          sql += " INNER JOIN company WITH(NOLOCK) ON adoc_infavor_comp_id = comp_id and comp_journ_id = 0 "
          sql += " LEFT OUTER JOIN Financial_Institution_Company_Reference WITH(NOLOCK) ON ficr_sub_comp_id = adoc_infavor_comp_id "
          sql += " LEFT OUTER JOIN Financial_Institution_Primary_Group WITH(NOLOCK) ON fipg_main_comp_id = ficr_main_comp_id "
          sql += " WHERE ((adoc_doc_date >= '" & Month(DateAdd(DateInterval.Month, -12, Now())) & "/" & Day(DateAdd(DateInterval.Month, -12, Now())) & "/" & Year(DateAdd(DateInterval.Month, -12, Now())) & "') AND (adoc_doc_date < '" & Month(Now()) & "/" & Day(Now()) & "/" & Year(Now()) & "')) "

          If use_insight_roll Then
            sql += " AND ficr_main_comp_id in (select distinct ficr_main_comp_id from Financial_Institution_Company_Reference with (NOLOCK)"
            sql += " where ficr_sub_comp_id = " & searchCriteria.ViewCriteriaCompanyID.ToString & ")"
          Else
            sql += " and ficr_sub_comp_id = " & searchCriteria.ViewCriteriaCompanyID.ToString
          End If

          sql += " AND journ_internal_trans_flag = 'N' AND journ_subcat_code_part2 NOT LIKE 'IT' "
          sql += Constants.cSingleSpace + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, True)
          sql += " GROUP BY amod_make_name, amod_model_name, amod_id "
          sql += " ORDER BY tcount DESC"



          HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>GetModelDocuments(ByRef searchCriteria As viewSelectionCriteriaClass)</b><br />" & sql


          SqlConn.ConnectionString = clientConnectStr
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
      End If

      Return atemptable
    Catch ex As Exception
      GetModelDocuments = Nothing
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

  End Function
  Public Function GetTypeDocs(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal use_insight_roll As Boolean)
    Dim sql As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim atemptable As New DataTable
    Try
      If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("jetnetClientDatabase")) Then
        If searchCriteria.ViewCriteriaCompanyID > 0 Then

          sql = " SELECT adoc_doc_type, count(distinct adoc_journ_id + adoc_journ_seq_no) AS tcount "
          sql += " FROM Aircraft_Document WITH(NOLOCK) "
          sql += " INNER JOIN Aircraft WITH(NOLOCK) ON ac_id = adoc_ac_id AND ac_journ_id = adoc_journ_id "
          sql += " INNER JOIN aircraft_model WITH(NOLOCK) ON ac_amod_id = amod_id INNER JOIN Journal WITH(NOLOCK) ON ac_journ_id = journ_id "
          sql += " INNER JOIN company WITH(NOLOCK) ON adoc_infavor_comp_id = comp_id and comp_journ_id = 0 "
          sql += " LEFT OUTER JOIN Financial_Institution_Company_Reference WITH(NOLOCK) ON ficr_sub_comp_id = adoc_infavor_comp_id "
          sql += " LEFT OUTER JOIN Financial_Institution_Primary_Group WITH(NOLOCK) ON fipg_main_comp_id = ficr_main_comp_id "
          sql += " WHERE ((adoc_doc_date >= '" & Month(DateAdd(DateInterval.Month, -12, Now())) & "/" & Day(DateAdd(DateInterval.Month, -12, Now())) & "/" & Year(DateAdd(DateInterval.Month, -12, Now())) & "') AND (adoc_doc_date < '" & Month(Now()) & "/" & Day(Now()) & "/" & Year(Now()) & "'))"

          If use_insight_roll Then
            sql += " AND ficr_main_comp_id in (select distinct ficr_main_comp_id from Financial_Institution_Company_Reference with (NOLOCK)"
            sql += " where ficr_sub_comp_id =  " & searchCriteria.ViewCriteriaCompanyID.ToString & ")"
          Else
            sql += " and ficr_sub_comp_id = " & searchCriteria.ViewCriteriaCompanyID.ToString
          End If

          sql += " AND journ_internal_trans_flag = 'N' AND journ_subcat_code_part2 NOT LIKE 'IT' "
          sql += Constants.cSingleSpace + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, True)
          sql += " GROUP BY adoc_doc_type ORDER BY tcount DESC"

          HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>GetModelDocuments(ByRef searchCriteria As viewSelectionCriteriaClass)</b><br />" & sql


          SqlConn.ConnectionString = clientConnectStr
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
      End If

      Return atemptable
    Catch ex As Exception
      GetTypeDocs = Nothing
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

  End Function

#End Region

#Region "financial_documents_functions"

  Public Function get_top_financial_institutions_info(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal bGetNamesOnly As Boolean) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sSeperator As String = ""

    Try

      If Not bGetNamesOnly Then
        sQuery.Append("SELECT case ISNULL(fipg_generic_name,'') WHEN '' THEN 'Misc. Institutions' ELSE fipg_generic_name END AS fipg_generic_name, fipg_main_comp_id, count(distinct adoc_journ_id + adoc_journ_seq_no) as tcount")

        sQuery.Append(" FROM Aircraft_Document WITH(NOLOCK)")
        sQuery.Append(" INNER JOIN Aircraft WITH(NOLOCK) ON ac_id = adoc_ac_id AND ac_journ_id = adoc_journ_id")
        sQuery.Append(" INNER JOIN aircraft_model WITH(NOLOCK) ON ac_amod_id = amod_id")
        sQuery.Append(" INNER JOIN Journal WITH(NOLOCK) ON ac_journ_id = journ_id")

        sQuery.Append(" INNER JOIN company WITH(NOLOCK) ON adoc_infavor_comp_id = comp_id and comp_journ_id = 0")

        ' "check" "company location information"
        If searchCriteria.ViewCriteriaHasCompanyLocationInfo Then

          sQuery.Append(" LEFT OUTER JOIN State WITH(NOLOCK) ON state_code = comp_state")

          sQuery.Append(commonEvo.check_selected_geography_from_dropdowns(IIf(searchCriteria.ViewCriteriaUseContinent, "continent", "region"), searchCriteria.ViewCriteriaContinent, searchCriteria.ViewCriteriaCountry, searchCriteria.ViewCriteriaState, False))

          If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaTimeZone) Then
            sQuery.Append(Constants.cAndClause + "comp_timezone IN ('" + commonEvo.TranslateTimeZone(searchCriteria.ViewCriteriaTimeZone).Replace(Constants.cCommaDelim, Constants.cValueSeperator) + "')")
          End If

        End If

        sQuery.Append(" LEFT OUTER JOIN Financial_Institution_Company_Reference WITH(NOLOCK) ON ficr_sub_comp_id = adoc_infavor_comp_id")
        sQuery.Append(" LEFT OUTER JOIN Financial_Institution_Primary_Group WITH(NOLOCK) ON fipg_main_comp_id = ficr_main_comp_id")

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentsStartDate) And Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentsEndDate) Then
          sQuery.Append(" WHERE ((adoc_doc_date >= '" + searchCriteria.ViewCriteriaDocumentsStartDate + "') AND (adoc_doc_date < '" + searchCriteria.ViewCriteriaDocumentsEndDate + "'))")
        Else
          sQuery.Append(" WHERE ((adoc_doc_date >= '" + Month(DateAdd("m", (-1) * searchCriteria.ViewCriteriaTimeSpan, Now)).ToString + "/01/" + Year(DateAdd("m", (-1) * searchCriteria.ViewCriteriaTimeSpan, Now)).ToString + "')")
          sQuery.Append(Constants.cAndClause + "(adoc_doc_date < '" + Now.Month.ToString + "/01/" + Now.Year.ToString + "'))")
        End If

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentType) Then
          sQuery.Append(Constants.cAndClause + "adoc_doc_type IN ('" + searchCriteria.ViewCriteriaDocumentType.Replace(Constants.cCommaDelim, Constants.cValueSeperator) + "')")
        End If

        If searchCriteria.ViewCriteriaCompanyID > 0 Then
          sQuery.Append(Constants.cAndClause + "ficr_main_comp_id = " + searchCriteria.ViewCriteriaCompanyID.ToString)
        End If

        If Not searchCriteria.ViewCriteriaShowInternal Then
          sQuery.Append(Constants.cAndClause + "journ_internal_trans_flag = 'N' AND journ_subcat_code_part2 NOT LIKE 'IT'")
        End If

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentsTxType) Then
          sQuery.Append(Constants.cAndClause + "journ_subcat_code_part1 IN ('" + searchCriteria.ViewCriteriaDocumentsTxType.Replace(Constants.cCommaDelim, Constants.cValueSeperator) + "')")
        End If

        If searchCriteria.ViewCriteriaAmodID > -1 Then
          sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
        End If

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
          sQuery.Append(Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
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



        sQuery.Append(" GROUP BY fipg_generic_name, fipg_main_comp_id")
        sQuery.Append(" ORDER BY tcount DESC")

      Else

        sQuery.Append("SELECT fipg_generic_name, fipg_main_comp_id FROM Financial_Institution_Primary_Group WITH(NOLOCK)")
        sQuery.Append(" INNER JOIN company WITH(NOLOCK) ON fipg_main_comp_id = comp_id AND comp_journ_id = 0 ")

        sQuery.Append(" WHERE fipg_generic_name IS NOT NULL AND fipg_generic_name <> ''")
        sQuery.Append(Constants.cAndClause + "(comp_active_flag = 'Y' AND comp_hide_flag = 'N')")

        sQuery.Append(" GROUP BY fipg_generic_name, fipg_main_comp_id")
        sQuery.Append(" ORDER BY fipg_generic_name ASC")

      End If

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_top_financial_institutions_info(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal bGetNamesOnly As Boolean) As DataTable</b><br />" + sQuery.ToString

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
        aError = "Error in get_top_financial_institutions_info load datatable" + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_top_financial_institutions_info(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal bGetNamesOnly As Boolean) As DataTable" + ex.Message

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

  Public Sub views_display_top_financial_institutions(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String)

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder
    Dim toggleRowColor As Boolean = False

    Dim sRefLink As String = ""
    Dim sRefTitle As String = ""

    Try

      results_table = get_top_financial_institutions_info(searchCriteria, False)

      htmlOut.Append("<table id=""financialInstitutionsOuterTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"">")

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          htmlOut.Append("<tr><td align=""center"" valign=""middle"" class=""header"" colspan=""2"">FINANCIAL INSTITUTIONS WITH DOCUMENTS <em>(" + results_table.Rows.Count.ToString + ")</em></td></tr>")
          htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""seperator"" width=""70%"" style=""padding-left:5px;"" valign=""top""><strong>&nbsp;Name</strong></td>")
          htmlOut.Append("<td valign=""top"" align=""right"" class=""seperator"" style=""padding-right:5px;"" width=""30%""><strong># of Docs&nbsp;&nbsp;&nbsp;</strong></td></tr>")

          htmlOut.Append("<tr><td colspan=""2"" class=""rightside"">")

          If results_table.Rows.Count > 15 Then
            htmlOut.Append("<div valign=""top"" align=""left"" style=""height:642px; overflow: auto;""><p>")
          End If

          htmlOut.Append("<table id=""financialInstitutionsDataTable"" width=""100%"" cellpadding=""4"" cellspacing=""0"">")

          For Each r As DataRow In results_table.Rows

            If Not IsDBNull(r.Item("tcount")) Then
              If Not String.IsNullOrEmpty(r.Item("tcount").ToString.Trim) Then

                If CLng(r.Item("tcount").ToString) > 0 Then

                  If Not toggleRowColor Then
                    htmlOut.Append("<tr class=""alt_row"">")
                    toggleRowColor = True
                  Else
                    htmlOut.Append("<tr bgcolor=""white"">")
                    toggleRowColor = False
                  End If

                  If Not IsDBNull(r.Item("fipg_generic_name")) Then
                    If Not String.IsNullOrEmpty(r.Item("fipg_generic_name").ToString.Trim) Then

                      If r.Item("fipg_generic_name").ToString.Trim.ToLower.Contains("misc. institutions") Then
                        htmlOut.Append("<td align=""left"" valign=""top"" class=""seperator"">" + r.Item("fipg_generic_name").ToString.Trim + "</td>")
                      Else

                        If searchCriteria.ViewCriteriaAmodID > -1 Then

                          sRefLink = "view_template.aspx?ViewID=4&ViewName=" + HttpContext.Current.Server.UrlEncode("Financial & Transaction Documents")
                          sRefLink += "&viewCompany=" + r.Item("fipg_main_comp_id").ToString + "&amod_id=" + searchCriteria.ViewCriteriaAmodID.ToString

                          sRefTitle = IIf(CBool(HttpContext.Current.Application.Item("DebugFlag").ToString), " title=""" + sRefLink.Trim + """", " title=""Click to view company documents""")
                          htmlOut.Append("<td align=""left"" valign=""middle"" class=""seperator""><a class=""underline cursor"" href=""" + sRefLink + """" + sRefTitle + ">" + r.Item("fipg_generic_name").ToString + "</a></td>")

                        Else

                          sRefLink = "view_template.aspx?ViewID=4&ViewName=" + HttpContext.Current.Server.UrlEncode("Financial & Transaction Documents")
                          sRefLink += "&viewCompany=" + r.Item("fipg_main_comp_id").ToString

                          sRefTitle = IIf(CBool(HttpContext.Current.Application.Item("DebugFlag").ToString), " title=""" + sRefLink.Trim + """", " title=""Click to view company documents""")
                          htmlOut.Append("<td align=""left"" valign=""middle"" class=""seperator""><a class=""underline cursor"" href=""" + sRefLink + """" + sRefTitle + ">" + r.Item("fipg_generic_name").ToString + "</a></td>")

                        End If

                      End If
                    Else
                      htmlOut.Append("<td align=""left"" valign=""middle"" class=""seperator""></td>")
                    End If
                  Else
                    htmlOut.Append("<td align=""left"" valign=""middle"" class=""seperator""></td>")
                  End If

                  If r.Item("fipg_generic_name").ToString.Trim.ToLower.Contains("misc. institutions") Then
                    htmlOut.Append("<td align=""right"" valign=""top"" class=""seperator"" style=""padding-right:5px;"">" + r.Item("tcount").ToString + "</td></tr>")
                  Else
                    htmlOut.Append("<td align=""right"" valign=""top"" class=""seperator"" style=""padding-right:5px;""><a class=""underline cursor"" href=""" + sRefLink + """" + sRefTitle + ">" + r.Item("tcount").ToString + "</a></td></tr>")
                  End If

                End If

              End If

            End If

          Next

          htmlOut.Append("</table>")

          If results_table.Rows.Count > 15 Then
            htmlOut.Append("</p></div>")
          End If

          htmlOut.Append("</td></tr>")

        Else
          htmlOut.Append("<tr><td align=""left valign=""middle"">No Financial Institutions matches for your search criteria</td></tr>")
        End If
      Else
        htmlOut.Append("<tr><td align=""left"" valign=""middle"">No Financial Institutions matches for your search criteria</td></tr>")
      End If

      htmlOut.Append("</table>")

    Catch ex As Exception

      aError = "Error in views_display_top_financial_institutions(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

    Finally

    End Try

    'return resulting html string
    out_htmlString = htmlOut.ToString
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Function get_ac_transaction_docs(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

    Dim atemptable As New DataTable
    Dim rptTable As New DataTable

    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      HttpContext.Current.Session.Item("Selection_Listing_Fields") = ""
      HttpContext.Current.Session.Item("Selection_Listing_Table") = ""
      HttpContext.Current.Session.Item("Selection_Listing_Where") = ""
      HttpContext.Current.Session.Item("Selection_Listing_Group") = ""
      HttpContext.Current.Session.Item("Selection_Listing_Order") = ""

      If searchCriteria.ViewCriteriaAmodID > -1 Or searchCriteria.ViewCriteriaCompanyID > 0 Or _
         Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaContinent) Or _
         Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaCountry) Or _
         Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaState) Then
        sQuery.Append("SELECT DISTINCT adoc_doc_date, adoc_doc_type, adoc_journ_id AS journ_id, adoc_journ_seq_no, journ_subject, journ_date, amod_make_name, amod_model_name, ac_id, ac_ser_no_full, ac_reg_no,")
        sQuery.Append(" comp_id, comp_name , comp_name_alt_type, comp_name_alt , comp_email_address, comp_web_address, comp_address1, comp_address2, comp_city, comp_state, comp_country, comp_zip_code")
      Else
        sQuery.Append("SELECT DISTINCT TOP 50 adoc_doc_date, adoc_doc_type, adoc_journ_id AS journ_id, adoc_journ_seq_no, journ_subject, journ_date, amod_make_name, amod_model_name, ac_id, ac_ser_no_full, ac_reg_no,")
        sQuery.Append(" comp_id, comp_name , comp_name_alt_type, comp_name_alt , comp_email_address, comp_web_address, comp_address1, comp_address2, comp_city, comp_state, comp_country, comp_zip_code")
      End If

      HttpContext.Current.Session.Item("Selection_Listing_Fields") = sQuery.ToString

      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "TOP 50 ", "")
      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "comp_id", "comp_id as 'COMPID'")
      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "comp_name ", "comp_name as 'COMPNAME'")
      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "comp_address1", "comp_address1 as 'ADDRESS'")
      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "comp_city", "comp_city as 'CITY'")
      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "comp_state", "comp_state as 'STATE'")
      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "comp_web_address", "comp_web_address as 'WEBADDRESS'")
      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "ac_list_date", "ac_list_date as 'LSITDATE'")


      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "adoc_doc_date", "adoc_doc_date as 'DOCDATE'")
      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "adoc_doc_type", "adoc_doc_type as 'DOCTYPE'")  
      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "adoc_journ_seq_no", "adoc_journ_seq_no as 'SEQNO'")
      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "journ_subject", "journ_subject as 'SUBJECT'")
      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "journ_date", "journ_date as 'DATE'")
      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "amod_make_name", "amod_make_name as 'MAKENAME'")
      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "amod_model_name", "amod_model_name as 'MODELNAME'")
      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "ac_id", "ac_id as 'ACID'")
      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "ac_ser_no_full", "ac_ser_no_full as 'SERNO'")
      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "ac_reg_no", "ac_reg_no as 'REGNO'")
      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "comp_name_alt_type", "comp_name_alt_type as 'COMPALTTYPE'")
      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "comp_name_alt ", "comp_name_alt as 'COMPALTNAME'")
      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "comp_email_address", "comp_email_address as 'COMPEMAIL'")
      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "comp_address2", "comp_address2 as 'COMPADDRESS2'")
      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "comp_country", "comp_country as 'COMPCOUNTRY'")
      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "comp_zip_code", "comp_zip_code as 'ZIPCODE'")
  

      HttpContext.Current.Session.Item("Selection_Listing_Table") = (" FROM Aircraft_Document WITH(NOLOCK)")
      HttpContext.Current.Session.Item("Selection_Listing_Table") &= (" INNER JOIN Aircraft WITH(NOLOCK) ON ac_id = adoc_ac_id AND ac_journ_id = adoc_journ_id")
      HttpContext.Current.Session.Item("Selection_Listing_Table") &= (" INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id")
      HttpContext.Current.Session.Item("Selection_Listing_Table") &= (" INNER JOIN Journal WITH(NOLOCK) ON ac_journ_id = journ_id")

      HttpContext.Current.Session.Item("Selection_Listing_Table") &= (" LEFT OUTER JOIN Financial_Institution_Company_Reference WITH(NOLOCK) ON ficr_sub_comp_id = adoc_infavor_comp_id")
      HttpContext.Current.Session.Item("Selection_Listing_Table") &= (" LEFT OUTER JOIN Financial_Institution_Primary_Group WITH(NOLOCK) ON fipg_main_comp_id = ficr_main_comp_id")

      HttpContext.Current.Session.Item("Selection_Listing_Table") &= (" INNER JOIN company WITH(NOLOCK) ON adoc_infavor_comp_id = comp_id and comp_journ_id = 0")




      ' "check" "company location information"
      If searchCriteria.ViewCriteriaHasCompanyLocationInfo Then

        HttpContext.Current.Session.Item("Selection_Listing_Table") &= (" LEFT OUTER JOIN State WITH(NOLOCK) ON state_code = comp_state")

        HttpContext.Current.Session.Item("Selection_Listing_Table") &= (commonEvo.check_selected_geography_from_dropdowns(IIf(searchCriteria.ViewCriteriaUseContinent, "continent", "region"), searchCriteria.ViewCriteriaContinent, searchCriteria.ViewCriteriaCountry, searchCriteria.ViewCriteriaState, False))

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaTimeZone) Then
          HttpContext.Current.Session.Item("Selection_Listing_Table") &= (Constants.cAndClause + "comp_timezone IN ('" + commonEvo.TranslateTimeZone(searchCriteria.ViewCriteriaTimeZone).Replace(Constants.cCommaDelim, Constants.cValueSeperator) + "')")
        End If

      End If 

      sQuery.Append(HttpContext.Current.Session.Item("Selection_Listing_Table"))
 
      HttpContext.Current.Session.Item("Selection_Listing_Where") = ""

      If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentsStartDate) And Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentsEndDate) Then
        HttpContext.Current.Session.Item("Selection_Listing_Where") &= (" WHERE ((adoc_doc_date >= '" + searchCriteria.ViewCriteriaDocumentsStartDate + "') AND (adoc_doc_date < '" + searchCriteria.ViewCriteriaDocumentsEndDate + "'))")
      Else
        HttpContext.Current.Session.Item("Selection_Listing_Where") &= (" WHERE ((adoc_doc_date >= '" + Month(DateAdd("m", (-1) * searchCriteria.ViewCriteriaTimeSpan, Now)).ToString + "/01/" + Year(DateAdd("m", (-1) * searchCriteria.ViewCriteriaTimeSpan, Now)).ToString + "')")
        HttpContext.Current.Session.Item("Selection_Listing_Where") &= (Constants.cAndClause + "(adoc_doc_date < '" + Now.Month.ToString + "/01/" + Now.Year.ToString + "'))")
      End If

      If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentType) Then
        HttpContext.Current.Session.Item("Selection_Listing_Where") &= (Constants.cAndClause + "adoc_doc_type IN ('" + searchCriteria.ViewCriteriaDocumentType.Replace(Constants.cCommaDelim, Constants.cValueSeperator) + "')")
      End If

      If searchCriteria.ViewCriteriaCompanyID > 0 Then
        HttpContext.Current.Session.Item("Selection_Listing_Where") &= (Constants.cAndClause + "ficr_main_comp_id = " + searchCriteria.ViewCriteriaCompanyID.ToString)
      End If

      If Not searchCriteria.ViewCriteriaShowInternal Then
        HttpContext.Current.Session.Item("Selection_Listing_Where") &= (Constants.cAndClause + "journ_internal_trans_flag = 'N' AND journ_subcat_code_part2 NOT LIKE 'IT'")
      End If

      If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentsTxType) Then
        HttpContext.Current.Session.Item("Selection_Listing_Where") &= (Constants.cAndClause + "journ_subcat_code_part1 IN ('" + searchCriteria.ViewCriteriaDocumentsTxType.Replace(Constants.cCommaDelim, Constants.cValueSeperator) + "')")
      End If

      If searchCriteria.ViewCriteriaAmodID > -1 Then
        HttpContext.Current.Session.Item("Selection_Listing_Where") &= (Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
      End If

      If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
        HttpContext.Current.Session.Item("Selection_Listing_Where") &= (Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
      End If

      Select Case CInt(searchCriteria.ViewCriteriaAirframeType)
        Case Constants.VIEW_EXECUTIVE
          HttpContext.Current.Session.Item("Selection_Listing_Where") &= (Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'E'")
        Case Constants.VIEW_JETS
          HttpContext.Current.Session.Item("Selection_Listing_Where") &= (Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'J'")
        Case Constants.VIEW_TURBOPROPS
          HttpContext.Current.Session.Item("Selection_Listing_Where") &= (Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'T'")
        Case Constants.VIEW_PISTONS
          HttpContext.Current.Session.Item("Selection_Listing_Where") &= (Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'P'")
        Case Constants.VIEW_HELICOPTERS
          HttpContext.Current.Session.Item("Selection_Listing_Where") &= (Constants.cAndClause + "amod_airframe_type_code='R' AND amod_type_code in ('T','P')")
      End Select

      If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
        HttpContext.Current.Session.Item("Selection_Listing_Where") &= (" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))
      Else
        HttpContext.Current.Session.Item("Selection_Listing_Where") &= (" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True))
      End If
 

      sQuery.Append(HttpContext.Current.Session.Item("Selection_Listing_Where"))

      HttpContext.Current.Session.Item("Selection_Listing_Group") = ""
      HttpContext.Current.Session.Item("Selection_Listing_Order") = (" ORDER BY adoc_doc_date desc")

      sQuery.Append(HttpContext.Current.Session.Item("Selection_Listing_Order"))

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_ac_transaction_docs(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlCommand.CommandText = sQuery.ToString
      SqlReader = SqlCommand.ExecuteReader()

      Try

        atemptable.Load(SqlReader)

      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
        aError = "Error in get_ac_transaction_docs load datatable " + constrExc.Message
      End Try

      SqlReader = Nothing
      SqlCommand.CommandText = sQuery.ToString.Replace(" TOP 50 ", Constants.cSingleSpace)
      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try

        rptTable.Load(SqlReader)

        ' save the query for the report
        HttpContext.Current.Session.Item("MasterDocumentList") = SqlCommand.CommandText

        ' save the datatable for the report
        HttpContext.Current.Session.Item("documentsDataTable") = rptTable

      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
        aError = "Error in get_ac_transaction_docs load datatable " + constrExc.Message
      End Try



    Catch ex As Exception
      Return Nothing

      aError = "Error in get_ac_transaction_docs(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

  Public Sub views_display_transaction_documents(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String)

    Dim htmlOut As New StringBuilder
    Dim results_table As New DataTable
    Dim toggleRowColor As Boolean = False

    Try

      If Not searchCriteria.ViewCriteriaIsReport Then
        htmlOut.Append("<table id=""displayTransactionDocumentsOuterTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"">")
      End If

      results_table = get_ac_transaction_docs(searchCriteria)

      If Not IsNothing(results_table) Then

        If Not searchCriteria.ViewCriteriaIsReport Then
          htmlOut.Append("<tr><td valign=""top"" align=""center"" class=""header"" colspan=""2"">LATEST FINANCIAL DOCUMENTS <em>(" + results_table.Rows.Count.ToString + ")</em></td></tr>")
        End If

        If results_table.Rows.Count > 0 Then

          If searchCriteria.ViewCriteriaIsReport Then
            htmlOut.Append("<table id=""displayTransactionDocumentsExcelTable"" width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""1"">")
            htmlOut.Append("<tr class=""alt_row""><th valign=""top"" align=""left"">DocumentDate</th><th>Type</th><th>Company</th><th>Make</th><th>Model</th><th>Serial</th><th>Registration</th><th>Subject</th><th>Transaction Date</th></tr>")
          End If

          If Not searchCriteria.ViewCriteriaIsReport Then

            htmlOut.Append("<tr><td colspan=""2"" valign=""top"" align=""left"" class=""rightside"">")

            If results_table.Rows.Count > 20 Then
              htmlOut.Append("<div valign=""top"" align=""left"" style=""height:935px; overflow: auto;""><p>")
            End If

            htmlOut.Append("<table id=""displayTransactionDocumentsDataTable"" width=""100%"" cellpadding=""4"" cellspacing=""0"">")

          End If

          Dim sTransdocHtml As String = ""
          Dim sTmpStr As String = ""

          For Each r As DataRow In results_table.Rows

            If Not searchCriteria.ViewCriteriaIsReport Then

              If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaContinent) Or Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaCountry) Or Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaState) Then
                sTmpStr = ", on behalf of "
              Else
                sTmpStr = ", Lender : "

                If r.Item("adoc_doc_type").ToString.ToLower.Contains("lease agreement") Then
                  sTmpStr = ", Lessor : "
                ElseIf r.Item("adoc_doc_type").ToString.ToLower.Contains("lien release") Then
                  sTmpStr = ", Released by "
                End If
              End If

              commonEvo.displayTransactionDocuments(CLng(r.Item("ac_id").ToString), CLng(r.Item("journ_id").ToString), CLng(r.Item("adoc_journ_seq_no").ToString), False, False, False, True, sTransdocHtml)

              If Not toggleRowColor Then
                htmlOut.Append("<tr class=""alt_row"">")
                toggleRowColor = True
              Else
                htmlOut.Append("<tr bgcolor=""white"">")
                toggleRowColor = False
              End If

              htmlOut.Append("<td valign=""top"" align=""left"" class=""seperator"">" + sTransdocHtml + "</td>")
                            htmlOut.Append("<td valign=""top"" align=""left"" class=""seperator""><em>" + FormatDateTime(r.Item("adoc_doc_date").ToString, DateFormat.ShortDate).ToString + "</em>, " + r.Item("adoc_doc_type").ToString + sTmpStr + "<a class=""underline pointer"" onclick=""javascript:load('DisplayCompanyDetail.aspx?compid=" + r.Item("comp_id").ToString + "&journid=" + r.Item("journ_id").ToString + "','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');"" title=""Display Company Details"">" + r.Item("comp_name").ToString.Trim + "</a>, " + r.Item("amod_make_name").ToString + "&nbsp;" + r.Item("amod_model_name").ToString + " Ser# " & r.Item("ac_ser_no_full").ToString + " Reg# " + r.Item("ac_reg_no").ToString)
                            htmlOut.Append(", <a class=""underline pointer"" onclick=""javascript:load('DisplayAircraftDetail.aspx?acid=" + r.Item("ac_id").ToString + "&jid=" + r.Item("journ_id").ToString + "','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');"" title=""Display Aircraft Details"">" + r.Item("journ_subject").ToString + " on " + FormatDateTime(r.Item("journ_date").ToString, DateFormat.ShortDate).ToString + "</a></td></tr>")
                        Else
              htmlOut.Append("<tr><td>" + FormatDateTime(r.Item("adoc_doc_date").ToString, DateFormat.ShortDate).ToString + "</td><td>" + r.Item("adoc_doc_type").ToString + "</td><td>" + r.Item("comp_name").ToString + "</td><td>" + r.Item("amod_make_name").ToString + "</td><td>" + r.Item("amod_model_name").ToString & "</td><td>" + r.Item("ac_ser_no_full").ToString + "</td><td>" + r.Item("ac_reg_no").ToString)
              htmlOut.Append("</td><td>" + r.Item("journ_subject").ToString + "</td><td>" + FormatDateTime(r.Item("journ_date").ToString, DateFormat.ShortDate).ToString + "</td></tr>")
            End If

            sTransdocHtml = ""
            sTmpStr = ""

          Next

          htmlOut.Append("</table>")
          If results_table.Rows.Count > 15 Then
            htmlOut.Append("</p></div>")
          End If

          htmlOut.Append("</td></tr>")

        Else
          htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""seperator""><br />No data matches for your search criteria.</td></tr>")
        End If

      Else
        htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""seperator""><br />No data matches for your search criteria.</td></tr>")
      End If

      htmlOut.Append("</table>")

    Catch ex As Exception

      aError = "Error in views_display_transaction_documents(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

    Finally

    End Try

    'return resulting html string
    out_htmlString = htmlOut.ToString
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Function views_build_documents_header(ByRef searchCriteria As viewSelectionCriteriaClass) As String

    Dim query As String = ""
    Dim tHeaderTextString As New StringBuilder
    Dim tHeaderString As New StringBuilder

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentsStartDate) And Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentsEndDate) Then
        tHeaderTextString.Append("From: " + FormatDateTime(CDate(searchCriteria.ViewCriteriaDocumentsStartDate), DateFormat.ShortDate))
        tHeaderTextString.Append(Constants.cSingleSpace + "Up to: " + FormatDateTime(CDate(searchCriteria.ViewCriteriaDocumentsEndDate), DateFormat.ShortDate) + "<br />")
      End If

      If searchCriteria.ViewCriteriaAmodID > -1 Then

        query = "SELECT DISTINCT amod_make_name, amod_model_name, amod_id FROM Aircraft_model WITH(NOLOCK) WHERE amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString
        SqlCommand.CommandText = query

        SqlReader = SqlCommand.ExecuteReader()

        If SqlReader.HasRows Then
          SqlReader.Read()
          tHeaderTextString.Append(SqlReader.Item("amod_make_name").ToString.Trim + Constants.cSingleSpace + SqlReader.Item("amod_model_name").ToString.Trim + "<br />")
        End If

        SqlReader.Close()

      Else
        tHeaderTextString.Append("All Models<br />")
      End If

      If searchCriteria.ViewCriteriaCompanyID > 0 Then

        query = "SELECT DISTINCT fipg_generic_name, fipg_main_comp_id FROM Financial_Institution_Primary_Group WITH(NOLOCK) WHERE fipg_main_comp_id = " + searchCriteria.ViewCriteriaCompanyID.ToString
        SqlCommand.CommandText = query.Trim
        SqlReader = SqlCommand.ExecuteReader()

        If SqlReader.HasRows Then
          SqlReader.Read()
          tHeaderTextString.Append(SqlReader.Item("fipg_generic_name").ToString.Trim + "<br />")
        End If

        SqlReader.Close()

      End If

      If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaContinent.Trim) Or Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaCountry.Trim) Or Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaState.Trim) Then

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaContinent.Trim) Then

          If Split(searchCriteria.ViewCriteriaContinent, Constants.cCommaDelim).Length = 1 Then
            tHeaderString.Append(searchCriteria.ViewCriteriaContinent.Trim)
          Else
            tHeaderString.Append("Multiple Continents/Regions")
          End If

        End If

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaCountry.Trim) Then

          If Split(searchCriteria.ViewCriteriaCountry, Constants.cCommaDelim).Length = 1 Then
            tHeaderString.Append(Constants.cSingleSpace + searchCriteria.ViewCriteriaCountry.Trim)
          Else
            tHeaderString.Append("Multiple Countries")
          End If

        End If

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaState.Trim) Then

          If Split(searchCriteria.ViewCriteriaState, Constants.cCommaDelim).Length = 1 Then
            tHeaderString.Append(Constants.cSingleSpace + searchCriteria.ViewCriteriaState.Trim)
          Else
            tHeaderString.Append("Multiple States")
          End If

        End If

        tHeaderTextString.Append(Constants.cSingleSpace + tHeaderString.ToString)

      End If

    Catch ex As Exception
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing

    End Try

    Return tHeaderTextString.ToString

    tHeaderTextString = Nothing
    tHeaderString = Nothing

  End Function

  Public Function get_top_document_types_info(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal bGetNamesOnly As Boolean) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sSeperator As String = ""

    Try

      If Not bGetNamesOnly Then

        sQuery.Append("SELECT adoc_doc_type, count(distinct adoc_journ_id + adoc_journ_seq_no) AS tcount")

        sQuery.Append(" FROM Aircraft_Document WITH(NOLOCK)")
        sQuery.Append(" INNER JOIN Aircraft WITH(NOLOCK) ON ac_id = adoc_ac_id AND ac_journ_id = adoc_journ_id")
        sQuery.Append(" INNER JOIN aircraft_model WITH(NOLOCK) ON ac_amod_id = amod_id")
        sQuery.Append(" INNER JOIN Journal WITH(NOLOCK) ON ac_journ_id = journ_id")

        sQuery.Append(" INNER JOIN company WITH(NOLOCK) ON adoc_infavor_comp_id = comp_id and comp_journ_id = 0")

        ' "check" "company location information"
        If searchCriteria.ViewCriteriaHasCompanyLocationInfo Then

          sQuery.Append(" LEFT OUTER JOIN State WITH(NOLOCK) ON state_code = comp_state")

          sQuery.Append(commonEvo.check_selected_geography_from_dropdowns(IIf(searchCriteria.ViewCriteriaUseContinent, "continent", "region"), searchCriteria.ViewCriteriaContinent, searchCriteria.ViewCriteriaCountry, searchCriteria.ViewCriteriaState, False))

          If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaTimeZone) Then
            sQuery.Append(Constants.cAndClause + "comp_timezone IN ('" + commonEvo.TranslateTimeZone(searchCriteria.ViewCriteriaTimeZone).Replace(Constants.cCommaDelim, Constants.cValueSeperator) + "')")
          End If

        End If

        If searchCriteria.ViewCriteriaCompanyID > 0 Then
          sQuery.Append(" LEFT OUTER JOIN Financial_Institution_Company_Reference WITH(NOLOCK) ON ficr_sub_comp_id = adoc_infavor_comp_id")
          sQuery.Append(" LEFT OUTER JOIN Financial_Institution_Primary_Group WITH(NOLOCK) ON fipg_main_comp_id = ficr_main_comp_id")
        End If

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentsStartDate) And Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentsEndDate) Then
          sQuery.Append(" WHERE ((adoc_doc_date >= '" + searchCriteria.ViewCriteriaDocumentsStartDate + "') AND (adoc_doc_date < '" + searchCriteria.ViewCriteriaDocumentsEndDate + "'))")
        Else
          sQuery.Append(" WHERE ((adoc_doc_date >= '" + Month(DateAdd("m", (-1) * searchCriteria.ViewCriteriaTimeSpan, Now)).ToString + "/01/" + Year(DateAdd("m", (-1) * searchCriteria.ViewCriteriaTimeSpan, Now)).ToString + "')")
          sQuery.Append(Constants.cAndClause + "(adoc_doc_date < '" + Now.Month.ToString + "/01/" + Now.Year.ToString + "'))")
        End If

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentType) Then
          sQuery.Append(Constants.cAndClause + "adoc_doc_type IN ('" + searchCriteria.ViewCriteriaDocumentType.Replace(Constants.cCommaDelim, Constants.cValueSeperator) + "')")
        End If

        If searchCriteria.ViewCriteriaCompanyID > 0 Then
          sQuery.Append(Constants.cAndClause + "ficr_main_comp_id = " + searchCriteria.ViewCriteriaCompanyID.ToString)
        End If

        If Not searchCriteria.ViewCriteriaShowInternal Then
          sQuery.Append(Constants.cAndClause + "journ_internal_trans_flag = 'N' AND journ_subcat_code_part2 NOT LIKE 'IT'")
        End If

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentsTxType) Then
          sQuery.Append(Constants.cAndClause + "journ_subcat_code_part1 IN ('" + searchCriteria.ViewCriteriaDocumentsTxType.Replace(Constants.cCommaDelim, Constants.cValueSeperator) + "')")
        End If

        If searchCriteria.ViewCriteriaAmodID > -1 Then
          sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
        End If

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
          sQuery.Append(Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
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

        sQuery.Append(" GROUP BY adoc_doc_type")
        sQuery.Append(" ORDER BY tcount DESC")

      Else
        'The query this was replaced with needed to be removed for speed issues.
        'It was performing a select distinct on over 650,000 records. 
        'This was replaced on 4/5/2016.
        sQuery.Append(" select distinct doctype_description as adoc_doc_type from Document_Type with (NOLOCK) ")
        sQuery.Append(" where doctype_send_to_evol='Y'")
        sQuery.Append(" order by doctype_description")

      End If

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_top_document_types_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
        aError = "Error in get_top_document_types_info load datatable" + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_top_document_types_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable" + ex.Message

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

  Public Sub views_display_top_document_types(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String)

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder
    Dim toggleRowColor As Boolean = False

    Dim sRefLink As String = ""
    Dim sRefTitle As String = ""

    Try

      results_table = get_top_document_types_info(searchCriteria, False)

      htmlOut.Append("<table id=""financialDocumentsTypesOuterTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"">")

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          htmlOut.Append("<tr><td align=""center"" valign=""middle"" class=""header"" colspan=""2"">TYPES OF FINANCIAL DOCUMENTS</td></tr>")
          htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""seperator"" width=""70%"" style=""padding-left:5px;"" valign=""top""><strong>&nbsp;Name</strong></td>")
          htmlOut.Append("<td valign=""top"" align=""right"" class=""seperator"" style=""padding-right:5px;"" width=""30%""><strong># of Docs&nbsp;&nbsp;&nbsp;</strong></td></tr>")

          htmlOut.Append("<tr><td colspan=""2"" class=""rightside"">")
          If results_table.Rows.Count > 15 Then
            htmlOut.Append("<div valign=""top"" align=""left"" style=""height:642px; overflow: auto;""><p>")
          End If
          htmlOut.Append("<table id=""financialDocumentsTypesDataTable"" width=""100%"" cellpadding=""4"" cellspacing=""0"">")

          For Each r As DataRow In results_table.Rows

            If Not toggleRowColor Then
              htmlOut.Append("<tr class=""alt_row"">")
              toggleRowColor = True
            Else
              htmlOut.Append("<tr bgcolor=""white"">")
              toggleRowColor = False
            End If

            If Not IsDBNull(r.Item("tcount")) Then
              If Not String.IsNullOrEmpty(r.Item("tcount").ToString.Trim) Then   'If searchCriteria.ViewCriteriaCompanyID > 0 Then

                If CLng(r.Item("tcount").ToString) > 0 Then

                  If searchCriteria.ViewCriteriaAmodID > -1 Then

                    sRefLink = "view_template.aspx?ViewID=4&ViewName=" + HttpContext.Current.Server.UrlEncode("Financial & Transaction Documents")
                    sRefLink += "&viewCompany=" + searchCriteria.ViewCriteriaCompanyID.ToString + "&amod_id=" + searchCriteria.ViewCriteriaAmodID.ToString + "&viewDocType=" + HttpContext.Current.Server.UrlEncode(r.Item("adoc_doc_type").ToString.Trim)

                    sRefTitle = IIf(CBool(HttpContext.Current.Application.Item("DebugFlag").ToString), " title=""" + sRefLink.Trim + """", " title=""Click to view top document types""")
                    htmlOut.Append("<td align=""left"" valign=""top"" class=""seperator"">" + r.Item("adoc_doc_type").ToString + "</td>")
                    htmlOut.Append("<td align=""left"" valign=""right"" class=""seperator"" style=""padding-right:5px;""><a class=""underline cursor"" href=""" + sRefLink + """" + sRefTitle + ">" + FormatNumber(r.Item("tcount").ToString, 0, False, False, True).ToString + "</a></td></tr>")

                  Else

                    sRefLink = "view_template.aspx?ViewID=4&ViewName=" + HttpContext.Current.Server.UrlEncode("Financial & Transaction Documents")
                    sRefLink += "&viewCompany=" + searchCriteria.ViewCriteriaCompanyID.ToString + "&viewDocType=" + HttpContext.Current.Server.UrlEncode(r.Item("adoc_doc_type").ToString.Trim)

                    sRefTitle = IIf(CBool(HttpContext.Current.Application.Item("DebugFlag").ToString), " title=""" + sRefLink.Trim + """", " title=""Click to view top document types""")
                    htmlOut.Append("<td align=""left"" valign=""top"" class=""seperator"">" + r.Item("adoc_doc_type").ToString + "</td>")
                    htmlOut.Append("<td align=""left"" valign=""right"" class=""seperator"" style=""padding-right:5px;""><a class=""underline cursor"" href=""" + sRefLink + """" + sRefTitle + ">" + FormatNumber(r.Item("tcount").ToString, 0, False, False, True).ToString + "</a></td></tr>")

                  End If

                End If

              End If

            End If

          Next

          htmlOut.Append("</table>")
          If results_table.Rows.Count > 15 Then
            htmlOut.Append("</p></div>")
          End If

          htmlOut.Append("</td></tr>")

        Else
          htmlOut.Append("<tr><td align=""left valign=""middle"">No types of financial documents match for your search criteria</td></tr>")
        End If
      Else
        htmlOut.Append("<tr><td align=""left"" valign=""middle"">No types of financial documents match for your search criteria</td></tr>")
      End If

      htmlOut.Append("</table>")

    Catch ex As Exception

      aError = "Error in views_display_top_document_types(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

    Finally

    End Try

    'return resulting html string
    out_htmlString = htmlOut.ToString
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Function get_documents_by_month_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sSeperator As String = ""

    Try

      sQuery.Append("SELECT DISTINCT YEAR(adoc_doc_date) AS tyear, MONTH(adoc_doc_date) AS tmonth, count(distinct adoc_journ_id + adoc_journ_seq_no) AS tcount")

      sQuery.Append(" FROM Aircraft_Document WITH(NOLOCK)")
      sQuery.Append(" INNER JOIN Aircraft WITH(NOLOCK) ON ac_id = adoc_ac_id AND ac_journ_id = adoc_journ_id")
      sQuery.Append(" INNER JOIN aircraft_model WITH(NOLOCK) ON ac_amod_id = amod_id")
      sQuery.Append(" INNER JOIN Journal WITH(NOLOCK) ON ac_journ_id = journ_id")

      sQuery.Append(" INNER JOIN company WITH(NOLOCK) ON adoc_infavor_comp_id = comp_id and comp_journ_id = 0")

      ' "check" "company location information"
      If searchCriteria.ViewCriteriaHasCompanyLocationInfo Then

        sQuery.Append(" LEFT OUTER JOIN State WITH(NOLOCK) ON state_code = comp_state")

        sQuery.Append(commonEvo.check_selected_geography_from_dropdowns(IIf(searchCriteria.ViewCriteriaUseContinent, "continent", "region"), searchCriteria.ViewCriteriaContinent, searchCriteria.ViewCriteriaCountry, searchCriteria.ViewCriteriaState, False))

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaTimeZone) Then
          sQuery.Append(Constants.cAndClause + "comp_timezone IN ('" + commonEvo.TranslateTimeZone(searchCriteria.ViewCriteriaTimeZone).Replace(Constants.cCommaDelim, Constants.cValueSeperator) + "')")
        End If

      End If

      If searchCriteria.ViewCriteriaCompanyID > 0 Then
        sQuery.Append(" LEFT OUTER JOIN Financial_Institution_Company_Reference WITH(NOLOCK) ON ficr_sub_comp_id = adoc_infavor_comp_id")
        sQuery.Append(" LEFT OUTER JOIN Financial_Institution_Primary_Group WITH(NOLOCK) ON fipg_main_comp_id = ficr_main_comp_id")
      End If

      If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentsStartDate) And Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentsEndDate) Then
        sQuery.Append(" WHERE ((adoc_doc_date >= '" + searchCriteria.ViewCriteriaDocumentsStartDate + "') AND (adoc_doc_date < '" + searchCriteria.ViewCriteriaDocumentsEndDate + "'))")
      Else
        sQuery.Append(" WHERE ((adoc_doc_date >= '" + Month(DateAdd("m", (-1) * searchCriteria.ViewCriteriaTimeSpan, Now)).ToString + "/01/" + Year(DateAdd("m", (-1) * searchCriteria.ViewCriteriaTimeSpan, Now)).ToString + "')")
        sQuery.Append(Constants.cAndClause + "(adoc_doc_date < '" + Now.Month.ToString + "/01/" + Now.Year.ToString + "'))")
      End If

      If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentType) Then
        sQuery.Append(Constants.cAndClause + "adoc_doc_type IN ('" + searchCriteria.ViewCriteriaDocumentType.Replace(Constants.cCommaDelim, Constants.cValueSeperator) + "')")
      End If

      If searchCriteria.ViewCriteriaCompanyID > 0 Then
        sQuery.Append(Constants.cAndClause + "ficr_main_comp_id = " + searchCriteria.ViewCriteriaCompanyID.ToString)
      End If

      If Not searchCriteria.ViewCriteriaShowInternal Then
        sQuery.Append(Constants.cAndClause + "journ_internal_trans_flag = 'N' AND journ_subcat_code_part2 NOT LIKE 'IT'")
      End If

      If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentsTxType) Then
        sQuery.Append(Constants.cAndClause + "journ_subcat_code_part1 IN ('" + searchCriteria.ViewCriteriaDocumentsTxType.Replace(Constants.cCommaDelim, Constants.cValueSeperator) + "')")
      End If

      If searchCriteria.ViewCriteriaAmodID > -1 Then
        sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
      End If

      If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
        sQuery.Append(Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
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

      sQuery.Append(" GROUP BY YEAR(adoc_doc_date), MONTH(adoc_doc_date)")
      sQuery.Append(" ORDER BY YEAR(adoc_doc_date) ASC, MONTH(adoc_doc_date) ASC")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_top_document_types_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
        aError = "Error in get_top_document_types_info load datatable" + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_top_document_types_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable" + ex.Message

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

  Public Sub views_display_documents_by_month(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByRef DOCS_BY_MONTH_GRAPH As DataVisualization.Charting.Chart)

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder

    Dim high_number As Integer = 0
    Dim low_number As Integer = 0
    Dim starting_point As Integer = 0
    Dim interval_point As Integer = 1

    Dim imgCnt As Integer = 0

    Dim sImageMapPath As String = ""
    Dim sImageSrc As String = ""
    Dim sImageName As String = ""

    Dim subscriptionInfo As String = HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString + "_" + HttpContext.Current.Session.Item("localUser").crmUserLogin.ToString.Trim + "_" + HttpContext.Current.Session.Item("localUser").crmSubSeqNo.ToString + "_"
    Dim displayFolder As String = HttpContext.Current.Application.Item("crmClientSiteData").ClientFullHostName + HttpContext.Current.Session.Item("MarketSummaryFolderVirtualPath")

    Try

      DOCS_BY_MONTH_GRAPH.Series.Clear()
      DOCS_BY_MONTH_GRAPH.Series.Add("DOCUMENTS")
      DOCS_BY_MONTH_GRAPH.Series("DOCUMENTS").ChartType = UI.DataVisualization.Charting.SeriesChartType.Column
      DOCS_BY_MONTH_GRAPH.Series("DOCUMENTS").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32
      DOCS_BY_MONTH_GRAPH.Series("DOCUMENTS").LabelForeColor = Drawing.Color.Blue
      DOCS_BY_MONTH_GRAPH.Series("DOCUMENTS").Color = Drawing.Color.Blue
      DOCS_BY_MONTH_GRAPH.Series("DOCUMENTS").BorderWidth = 1
      DOCS_BY_MONTH_GRAPH.Series("DOCUMENTS").MarkerSize = 5
      DOCS_BY_MONTH_GRAPH.Series("DOCUMENTS").MarkerStyle = UI.DataVisualization.Charting.MarkerStyle.Circle

      DOCS_BY_MONTH_GRAPH.ChartAreas("ChartArea1").AxisY.Title = "Documents"
      DOCS_BY_MONTH_GRAPH.ChartAreas("ChartArea1").AxisX.Title = "Month"

      If searchCriteria.ViewID = 4 Then
        DOCS_BY_MONTH_GRAPH.Width = 400
        DOCS_BY_MONTH_GRAPH.Height = 350
      Else
        DOCS_BY_MONTH_GRAPH.Width = 260
        DOCS_BY_MONTH_GRAPH.Height = 260
      End If

      results_table = get_documents_by_month_info(searchCriteria)

      htmlOut.Append("<table id=""financialDocumentsGraphOuterTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"">")
      htmlOut.Append("<tr><td align=""center"" valign=""middle"" class=""header"">FINANCIAL DOCUMENTS BY MONTH</td></tr>")

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          For Each r As DataRow In results_table.Rows

            If Not IsDBNull(r("tyear")) Then
              If Not String.IsNullOrEmpty(r.Item("tyear").ToString.Trim) Then

                If high_number = 0 Or CDbl(r.Item("tcount").ToString) > high_number Then
                  high_number = CDbl(r.Item("tcount").ToString)
                End If

                If low_number = 0 Or CDbl(r.Item("tcount")) < low_number Then
                  low_number = CDbl(r.Item("tcount").ToString)
                End If

                DOCS_BY_MONTH_GRAPH.Series("DOCUMENTS").Points.AddXY((r.Item("tmonth").ToString + "-" + r.Item("tyear").ToString), CDbl(r.Item("tcount").ToString))

              End If
            End If

          Next

          DOCS_BY_MONTH_GRAPH.ChartAreas("ChartArea1").AxisY.Maximum = high_number + Math.Round((high_number / 10), 0)
          DOCS_BY_MONTH_GRAPH.ChartAreas("ChartArea1").AxisY.Minimum = 0

          If high_number >= 100 Then
            DOCS_BY_MONTH_GRAPH.ChartAreas("ChartArea1").AxisY.Interval = Math.Round((high_number / 10), 0)
          ElseIf high_number < 5 Then
            DOCS_BY_MONTH_GRAPH.ChartAreas("ChartArea1").AxisY.Interval = 1
          ElseIf high_number < 10 Then
            DOCS_BY_MONTH_GRAPH.ChartAreas("ChartArea1").AxisY.Interval = 2
          Else
            DOCS_BY_MONTH_GRAPH.ChartAreas("ChartArea1").AxisY.Interval = 10
          End If

          DOCS_BY_MONTH_GRAPH.Titles.Clear()
          DOCS_BY_MONTH_GRAPH.Titles.Add("Documents per Month")

          imgCnt += 1
          sImageName = subscriptionInfo + commonEvo.GenerateFileName("image_" + imgCnt.ToString, ".jpg", False)
          sImageMapPath = HttpContext.Current.Server.MapPath(HttpContext.Current.Session.Item("MarketSummaryFolderVirtualPath")) + "\" + sImageName
          sImageSrc = displayFolder + "/" + sImageName

          DOCS_BY_MONTH_GRAPH.ImageType = DataVisualization.Charting.ChartImageType.Jpeg
          DOCS_BY_MONTH_GRAPH.SaveImage(sImageMapPath, DataVisualization.Charting.ChartImageFormat.Jpeg)

          htmlOut.Append("<tr><td valign=""middle"" align=""center""><img src=""" + sImageSrc + """ title=""Documents Per Month""></td></tr>")

        Else
          htmlOut.Append("<tr><td valign=""middle"" align=""left"" class=""seperator"" style=""padding-left:3px;""><br />No documents match your search criteria</td></tr>")
        End If

      Else
        htmlOut.Append("<tr><td valign=""middle"" align=""left"" class=""seperator"" style=""padding-left:3px;""><br />No documents match your search criteria</td></tr>")

      End If

      htmlOut.Append("</table>")

    Catch ex As Exception

      aError = "Error in views_display_documents_by_month(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByRef DOCS_BY_MONTH_GRAPH As DataVisualization.Charting.Chart) " + ex.Message

    Finally

    End Try

    'return resulting html string
    out_htmlString = htmlOut.ToString
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Sub views_display_documents_by_month_pie_chart(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByVal graphID As Integer)

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder

    Dim x As Integer = 0

    Try

      results_table = get_documents_by_month_info(searchCriteria)

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          htmlOut.Append(vbCrLf + "<script type=""text/javascript"" language=""javascript"">" + vbCrLf)
          htmlOut.Append("google.load('visualization', '1', {'packages':['corechart']});" + vbCrLf)
          htmlOut.Append("google.setOnLoadCallback(drawVisualization" + graphID.ToString + ");" + vbCrLf)
          htmlOut.Append("function drawVisualization" + graphID.ToString + "() {" + vbCrLf)
          htmlOut.Append("  var data" + graphID.ToString + " = new google.visualization.DataTable();" + vbCrLf)
          htmlOut.Append("  data" + graphID.ToString + ".addColumn('string', 'Label');" + vbCrLf)
          htmlOut.Append("  data" + graphID.ToString + ".addColumn('number', 'Value');" + vbCrLf)
          htmlOut.Append("  data" + graphID.ToString + ".addRows(" + results_table.Rows.Count.ToString + ");" + vbCrLf)

          For Each r As DataRow In results_table.Rows

            If Not IsDBNull(r.Item("tcount")) Then
              If Not String.IsNullOrEmpty(r.Item("tcount").ToString.Trim) Then
                If CLng(r.Item("tcount").ToString.Trim) > 0 Then
                  htmlOut.Append("  data" + graphID.ToString + ".setCell(" + x.ToString + ", 0, '" + r.Item("tmonth").ToString + "-" + r.Item("tyear").ToString + "');" + vbCrLf)
                  htmlOut.Append("  data" + graphID.ToString + ".setCell(" + x.ToString + ", 1, " + r.Item("tcount").ToString + ");" + vbCrLf)
                Else
                  htmlOut.Append("  data" + graphID.ToString + ".setCell(" + x.ToString + ", 0, '" + r.Item("tmonth").ToString + "-" + r.Item("tyear").ToString + "');" + vbCrLf)
                  htmlOut.Append("  data" + graphID.ToString + ".setCell(" + x.ToString + ", 1, 0);" + vbCrLf)
                End If
                x += 1
              End If
            End If

          Next

          htmlOut.Append("  var chart = new google.visualization.PieChart(document.getElementById(""Visualization" + graphID.ToString + """));" + vbCrLf)

          If results_table.Rows.Count > 35 Then  ' 1/720 slice visibility threshold
            htmlOut.Append("  chart.draw(data" + graphID.ToString + ", {chartArea:{width:'95%',height:'85%'}, sliceVisibilityThreshold:'0', pieResidueSliceLabel:'Other', legend:'left', legendFontSize:12 });" + vbCrLf)
          Else
            htmlOut.Append("  chart.draw(data" + graphID.ToString + ", {chartArea:{width:'95%',height:'85%'}, legend:'left', legendFontSize:12 });" + vbCrLf)
          End If

          htmlOut.Append("}" + vbCrLf)
          htmlOut.Append("</script>" + vbCrLf)

        End If

      End If

      If Not String.IsNullOrEmpty(htmlOut.ToString.Trim) Then
        htmlOut.Append("<table width=""100%"" height=""400"" cellpadding=""1"" cellspacing=""0"" class=""module"">")
        htmlOut.Append("<tr><td valign=""middle"" align=""center"" class=""header"">FINANCIAL DOCUMENTS BY MONTH</td></tr>")
        htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""border_bottom_right"">")
        htmlOut.Append("<div id=""Visualization" + graphID.ToString + """ style=""text-align:center; width:100%; height:400px;""></div>")
        htmlOut.Append("</td></tr></table>")
      Else
        htmlOut.Append("<table width=""100%"" height=""400"" cellpadding=""1"" cellspacing=""0"" class=""module"">")
        htmlOut.Append("<tr><td valign=""middle"" align=""center"" class=""header"">FINANCIAL DOCUMENTS BY MONTH</td></tr>")
        htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""border_bottom_right""><div style=""text-align:center; width:100%; height:400px;"">No Data to display</div></td></tr></table>")
      End If

    Catch ex As Exception

      aError = "Error in views_display_documents_by_month_pie_chart(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByVal graphID As Integer) " + ex.Message

    Finally

    End Try

    'return resulting html string
    out_htmlString = htmlOut.ToString
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Function get_top_document_models_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sSeperator As String = ""

    Try

      sQuery.Append("SELECT amod_make_name, amod_model_name, amod_id, count(distinct adoc_journ_id + adoc_journ_seq_no) AS tcount")

      sQuery.Append(" FROM Aircraft_Document WITH(NOLOCK)")
      sQuery.Append(" INNER JOIN Aircraft WITH(NOLOCK) ON ac_id = adoc_ac_id AND ac_journ_id = adoc_journ_id")
      sQuery.Append(" INNER JOIN aircraft_model WITH(NOLOCK) ON ac_amod_id = amod_id")
      sQuery.Append(" INNER JOIN Journal WITH(NOLOCK) ON ac_journ_id = journ_id")

      sQuery.Append(" INNER JOIN company WITH(NOLOCK) ON adoc_infavor_comp_id = comp_id and comp_journ_id = 0")

      ' "check" "company location information"
      If searchCriteria.ViewCriteriaHasCompanyLocationInfo Then

        sQuery.Append(" LEFT OUTER JOIN State WITH(NOLOCK) ON state_code = comp_state")

        sQuery.Append(commonEvo.check_selected_geography_from_dropdowns(IIf(searchCriteria.ViewCriteriaUseContinent, "continent", "region"), searchCriteria.ViewCriteriaContinent, searchCriteria.ViewCriteriaCountry, searchCriteria.ViewCriteriaState, False))

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaTimeZone) Then
          sQuery.Append(Constants.cAndClause + "comp_timezone IN ('" + commonEvo.TranslateTimeZone(searchCriteria.ViewCriteriaTimeZone).Replace(Constants.cCommaDelim, Constants.cValueSeperator) + "')")
        End If

      End If

      If searchCriteria.ViewCriteriaCompanyID > 0 Then
        sQuery.Append(" LEFT OUTER JOIN Financial_Institution_Company_Reference WITH(NOLOCK) ON ficr_sub_comp_id = adoc_infavor_comp_id")
        sQuery.Append(" LEFT OUTER JOIN Financial_Institution_Primary_Group WITH(NOLOCK) ON fipg_main_comp_id = ficr_main_comp_id")
      End If

      If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentsStartDate) And Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentsEndDate) Then
        sQuery.Append(" WHERE ((adoc_doc_date >= '" + searchCriteria.ViewCriteriaDocumentsStartDate + "') AND (adoc_doc_date < '" + searchCriteria.ViewCriteriaDocumentsEndDate + "'))")
      Else
        sQuery.Append(" WHERE ((adoc_doc_date >= '" + Month(DateAdd("m", (-1) * searchCriteria.ViewCriteriaTimeSpan, Now)).ToString + "/01/" + Year(DateAdd("m", (-1) * searchCriteria.ViewCriteriaTimeSpan, Now)).ToString + "')")
        sQuery.Append(Constants.cAndClause + "(adoc_doc_date < '" + Now.Month.ToString + "/01/" + Now.Year.ToString + "'))")
      End If

      If searchCriteria.ViewCriteriaCompanyID > 0 Then
        sQuery.Append(Constants.cAndClause + "ficr_main_comp_id = " + searchCriteria.ViewCriteriaCompanyID.ToString)
      End If

      If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentType) Then
        sQuery.Append(Constants.cAndClause + "adoc_doc_type IN ('" + searchCriteria.ViewCriteriaDocumentType.Replace(Constants.cCommaDelim, Constants.cValueSeperator) + "')")
      End If

      If Not searchCriteria.ViewCriteriaShowInternal Then
        sQuery.Append(Constants.cAndClause + "journ_internal_trans_flag = 'N' AND journ_subcat_code_part2 NOT LIKE 'IT'")
      End If

      If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentsTxType) Then
        sQuery.Append(Constants.cAndClause + "journ_subcat_code_part1 IN ('" + searchCriteria.ViewCriteriaDocumentsTxType.Replace(Constants.cCommaDelim, Constants.cValueSeperator) + "')")
      End If

      If searchCriteria.ViewCriteriaAmodID > -1 Then
        sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
      End If

      If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
        sQuery.Append(Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
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

      sQuery.Append(" GROUP BY amod_make_name, amod_model_name, amod_id")
      sQuery.Append(" ORDER BY tcount DESC")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_top_document_models_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
        aError = "Error in get_top_document_models_info load datatable" + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_top_document_models_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable" + ex.Message

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

  Public Sub views_display_top_document_models(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String)

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder
    Dim toggleRowColor As Boolean = False

    Dim sRefLink As String = ""
    Dim sRefTitle As String = ""

    Try

      results_table = get_top_document_models_info(searchCriteria)

      htmlOut.Append("<table id=""financialModelsOuterTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"">")

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          htmlOut.Append("<tr><td align=""center"" valign=""middle"" class=""header"" colspan=""2"">AIRCRAFT MODELS FINANCED <em>(" + results_table.Rows.Count.ToString + ")</em></td></tr>")
          htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""seperator"" width=""70%"" style=""padding-left:5px;"" valign=""top""><strong>&nbsp;Name</strong></td>")
          htmlOut.Append("<td valign=""top"" align=""right"" class=""seperator"" style=""padding-right:5px;"" width=""30%""><strong># of Docs&nbsp;&nbsp;&nbsp;</strong></td></tr>")

          htmlOut.Append("<tr><td colspan=""2"" class=""rightside"">")
          If results_table.Rows.Count > 15 Then
            htmlOut.Append("<div valign=""top"" align=""left"" style=""height:642px; overflow: auto;""><p>")
          End If
          htmlOut.Append("<table id=""financialModelsDataTable"" width=""100%"" cellpadding=""4"" cellspacing=""0"">")


          For Each r As DataRow In results_table.Rows

            If Not toggleRowColor Then
              htmlOut.Append("<tr class=""alt_row"">")
              toggleRowColor = True
            Else
              htmlOut.Append("<tr bgcolor=""white"">")
              toggleRowColor = False
            End If

            If Not IsDBNull(r.Item("tcount")) Then
              If Not String.IsNullOrEmpty(r.Item("tcount").ToString.Trim) Then

                If CLng(r.Item("tcount").ToString) > 0 Then

                  sRefLink = "view_template.aspx?ViewID=4&ViewName=" + HttpContext.Current.Server.UrlEncode("Financial & Transaction Documents") + "&viewCompany=" + searchCriteria.ViewCriteriaCompanyID.ToString + "&amod_id=" + r.Item("amod_id").ToString
                  sRefLink += "&viewCity=" + searchCriteria.ViewCriteriaCity.Trim + "&viewState=" + searchCriteria.ViewCriteriaState.Trim + "&viewCountry=" + searchCriteria.ViewCriteriaCountry.Trim + "&viewContinent=" + searchCriteria.ViewCriteriaContinent.Trim + "&viewAirframe=" + searchCriteria.ViewCriteriaAirframeTypeStr

                  sRefTitle = IIf(CBool(HttpContext.Current.Application.Item("DebugFlag").ToString), " title=""" + sRefLink.Trim + """", " title=""Click to view aircraft documents""")

                  htmlOut.Append("<td align=""left"" valign=""top"" class=""seperator""><a class=""underline cursor"" href=""" + sRefLink + """" + sRefTitle + ">" + r.Item("amod_make_name").ToString + "&nbsp;/&nbsp;" + r.Item("amod_model_name").ToString + "</a></td>")
                  htmlOut.Append("<td align=""right"" valign=""top"" class=""seperator"" style=""padding-right:5px;""><a class=""underline cursor"" href=""" + sRefLink + """" + sRefTitle + ">" + FormatNumber(r.Item("tcount").ToString, 0, False, False, True).ToString + "</a></td></tr>")

                End If

              End If

            End If

          Next

          htmlOut.Append("</table>")
          If results_table.Rows.Count > 15 Then
            htmlOut.Append("</p></div>")
          End If

          htmlOut.Append("</td></tr>")

        Else
          htmlOut.Append("<tr><td align=""left valign=""middle"">No Top Models matches for your search criteria</td></tr>")
        End If
      Else
        htmlOut.Append("<tr><td align=""left"" valign=""middle"">No Top Models matches for your search criteria</td></tr>")
      End If

      htmlOut.Append("</table>")

    Catch ex As Exception

      aError = "Error in views_display_top_document_models(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

    Finally

    End Try

    'return resulting html string
    out_htmlString = htmlOut.ToString
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Sub views_fill_financial_institutions(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef maxWidth As Long, ByRef lbFinanceInstitutions As ListBox)

    Dim results_table As New DataTable

    Try

      lbFinanceInstitutions.Items.Clear()
      lbFinanceInstitutions.Items.Add(New ListItem("All", ""))

      results_table = get_top_financial_institutions_info(searchCriteria, True)
      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          For Each r As DataRow In results_table.Rows

            If Not IsDBNull(r.Item("fipg_generic_name")) And Not String.IsNullOrEmpty(r.Item("fipg_generic_name").ToString.Trim) Then

              If (r.Item("fipg_generic_name").ToString.Length * Constants._STARTCHARWIDTH) > maxWidth Then
                maxWidth = (r.Item("fipg_generic_name").ToString.Length * Constants._STARTCHARWIDTH)
              End If

              lbFinanceInstitutions.Items.Add(New ListItem(r.Item("fipg_generic_name").ToString, r.Item("fipg_main_comp_id").ToString))

              If Not String.IsNullOrEmpty(r.Item("fipg_main_comp_id").ToString.Trim) Then
                If IsNumeric(r.Item("fipg_main_comp_id").ToString) Then
                  If CLng(r.Item("fipg_main_comp_id").ToString) = searchCriteria.ViewCriteriaCompanyID Then
                    lbFinanceInstitutions.SelectedValue = searchCriteria.ViewCriteriaCompanyID.ToString
                  End If
                End If
              End If

            End If

          Next
        End If
      End If

      If searchCriteria.ViewCriteriaCompanyID = 0 Then
        lbFinanceInstitutions.SelectedValue = ""
      End If

      lbFinanceInstitutions.Width = (maxWidth)

    Catch ex As Exception

      aError = "Error in views_fill_financial_institutions(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef maxWidth As Long, ByRef lbFinanceInstitutions As ListBox) " + ex.Message

    Finally

    End Try

    results_table = Nothing

  End Sub

  Public Sub views_fill_financial_doc_types(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef maxWidth As Long, ByRef lbFinancialDocType As ListBox)

    Dim results_table As New DataTable
    Dim tmpDocArr() As String = Nothing

    Try

      lbFinancialDocType.Items.Clear()
      lbFinancialDocType.Items.Add(New ListItem("All", ""))

      results_table = get_top_document_types_info(searchCriteria, True)

      tmpDocArr = searchCriteria.ViewCriteriaDocumentType.Split(Constants.cCommaDelim)

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          For Each r As DataRow In results_table.Rows

            If Not IsDBNull(r.Item("adoc_doc_type")) And Not String.IsNullOrEmpty(r.Item("adoc_doc_type").ToString.Trim) Then

              If (r.Item("adoc_doc_type").ToString.Length * Constants._STARTCHARWIDTH) > maxWidth Then
                maxWidth = (r.Item("adoc_doc_type").ToString.Length * Constants._STARTCHARWIDTH)
              End If

              lbFinancialDocType.Items.Add(New ListItem(r.Item("adoc_doc_type").ToString, r.Item("adoc_doc_type").ToString))

            End If

          Next
        End If
      End If

      If String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentType.Trim) Then
        lbFinancialDocType.SelectedValue = ""
      Else
        For i As Integer = 0 To lbFinancialDocType.Items.Count - 1

          If commonEvo.inMyArray(tmpDocArr, lbFinancialDocType.Items(i).Value.ToUpper) Then
            lbFinancialDocType.Items(i).Selected = True
          End If

        Next

      End If

      lbFinancialDocType.Width = (maxWidth)

    Catch ex As Exception

      aError = "Error in views_fill_financial_doc_types(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef maxWidth As Long, ByRef lbFinancialDocType As ListBox) " + ex.Message

    Finally

    End Try

    results_table = Nothing

  End Sub

#End Region

End Class
