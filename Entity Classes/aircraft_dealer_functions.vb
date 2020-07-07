' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/Entity Classes/aircraft_dealer_functions.vb $
'$$Author: Matt $
'$$Date: 3/25/20 11:50a $
'$$Modtime: 3/25/20 8:57a $
'$$Revision: 4 $
'$$Workfile: aircraft_dealer_functions.vb $
'
' ********************************************************************************

<System.Serializable()> Public Class aircraft_dealer_functions

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



  Public Function ac_dealer_function_ac_sales(ByVal country_name As String, ByVal amod_id As Long, ByVal make_name As String, ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try


      sQuery.Append(" SELECT TOP 500 broker_main_comp_id, Company.comp_name, COUNT(distinct journ_id) as numtrans")
      sQuery.Append(" FROM Aircraft_Broker with (NOLOCK)")
      sQuery.Append(" INNER JOIN Company with (NOLOCK) on broker_main_comp_id = comp_id and comp_journ_id = 0")
      sQuery.Append(" INNER JOIN View_Aircraft_Company_History_Flat with (NOLOCK) on broker_comp_id = View_Aircraft_Company_History_Flat.comp_id and cref_journ_id = ac_journ_id")

      If Not IsNothing(searchCriteria) Then
        sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, False))
      End If

      sQuery.Append(" WHERE journ_subcat_code_part1 = 'WS'")
      sQuery.Append(" AND journ_internal_trans_flag = 'N'")
      sQuery.Append(" AND cref_contact_type in ('99','93','38','95','96','IV', '2P', '2X')")
      sQuery.Append(" AND journ_date >= (GETDATE() -365)")

      If Not String.IsNullOrEmpty(make_name.Trim) Then
        sQuery.Append(" AND amod_make_name = '" + make_name.Trim + "'")
      End If

      If amod_id > 0 Then
        sQuery.Append(" AND amod_id = " + amod_id.ToString)
      End If

      If Not String.IsNullOrEmpty(country_name.Trim) Then
        sQuery.Append(" AND company.comp_country = '" + country_name.Trim + "'")
      End If

      If Not IsNothing(searchCriteria) Then
        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftType.Trim) Then
          sQuery.Append(" AND amod_type_code in ('" + searchCriteria.ViewCriteriaAircraftType.Trim + "')")
        End If
      End If

      sQuery.Append(" GROUP BY broker_main_comp_id, Company.comp_name")
      sQuery.Append(" ORDER BY COUNT(distinct journ_id) desc")


      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn


      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      Return Nothing
      aError = "Error in ac_dealer_function_ac_sales() As DataTable: " + ex.Message

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

  Public Function ac_dealer_function_ac_count(ByVal main_comp_id As Long, ByVal amod_id As Long, ByVal country_name As String, ByVal make_name As String, ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try

      sQuery.Append(" SELECT DISTINCT TOP 500 broker_main_comp_id, Company.comp_name, count(distinct broker_comp_id) as NUMLOCATIONS,")
      sQuery.Append(" COUNT(distinct ac_id) as ACCOUNT")
      sQuery.Append(" FROM Aircraft_Broker with (NOLOCK)")
      sQuery.Append(" INNER JOIN Company with (NOLOCK) ON broker_main_comp_id = comp_id and comp_journ_id = 0")
      sQuery.Append(" INNER JOIN View_Aircraft_Company_Flat with (NOLOCK) ON broker_comp_id = View_Aircraft_Company_Flat.comp_id AND cref_journ_id = 0")

      If amod_id > 0 Then
        sQuery.Append(" AND View_Aircraft_Company_Flat.amod_id = " + amod_id.ToString)
      End If

      If Not IsNothing(searchCriteria) Then
        sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, False))
      End If

      sQuery.Append(" WHERE ac_journ_id = 0")

      If main_comp_id > 0 Then
        sQuery.Append(" AND broker_main_comp_id = " + main_comp_id.ToString)
      End If

      If Not String.IsNullOrEmpty(make_name.Trim) Then
        sQuery.Append(" AND amod_make_name = '" + make_name.Trim + "'")
      End If

      If Not String.IsNullOrEmpty(country_name.Trim) Then
        sQuery.Append(" AND company.comp_country = '" + country_name.Trim + "'")
      End If

      If Not IsNothing(searchCriteria) Then
        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftType.Trim) Then
          sQuery.Append(" AND amod_type_code in ('" + searchCriteria.ViewCriteriaAircraftType.Trim + "')")
        End If
      End If

      sQuery.Append(" AND cref_contact_type IN ('99','93','38','2X')")
      sQuery.Append(" GROUP BY broker_main_comp_id, Company.comp_name")
      sQuery.Append(" ORDER BY count(distinct ac_id) desc")


      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn


      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      Return Nothing
      aError = "Error in ac_dealer_function_ac_count() As DataTable: " + ex.Message

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

  Public Function ac_dealer_function_by_model(ByVal amod_id As Long, ByVal country_name As String, ByVal make_name As String, ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try


      sQuery.Append(" select distinct top 500 amod_make_name, amod_model_name, amod_id, COUNT(distinct broker_main_comp_id) as numlocations  ")
      sQuery.Append(" from Aircraft_Broker with (NOLOCK) ")
      sQuery.Append(" inner join Company with (NOLOCK) on broker_comp_id = comp_id and comp_journ_id =0 ")
      sQuery.Append(" inner join View_Aircraft_Company_Flat with (NOLOCK) on broker_comp_id=View_Aircraft_Company_Flat.comp_id and cref_journ_id = 0 ")
      sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, False))


      sQuery.Append("  where ac_journ_id = 0 ")
      sQuery.Append(" and cref_contact_type in ('99','93','38','2X') ")

      If Trim(make_name) <> "" Then
        sQuery.Append(" and amod_make_name = '" & make_name & "'  ")
      End If

      If amod_id > 0 Then
        sQuery.Append(" and amod_id = " & amod_id & " ")
      End If


      If Trim(searchCriteria.ViewCriteriaAircraftType) <> "" Then
        sQuery.Append(" and amod_type_code in ('" & Trim(searchCriteria.ViewCriteriaAircraftType) & "') ")
      End If



      If Trim(country_name) <> "" Then
        sQuery.Append(" and company.comp_country = '" & country_name & "' ")
      End If

      sQuery.Append(" group by amod_make_name, amod_model_name, amod_id ")
      sQuery.Append(" order by COUNT(distinct broker_main_comp_id) desc ")
      ' sQuery.Append(" order by amod_make_name, amod_model_name ")


      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn


      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      Return Nothing
      aError = "Error in ac_dealer_function_by_model() As DataTable: " + ex.Message

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

  Public Function ac_dealer_function_by_country(ByVal amod_id As Long, ByVal country_name As String, ByVal make_name As String, ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try


      sQuery.Append(" select top 500 company.comp_country, COUNT(distinct broker_comp_id) as numdealers  ")
      sQuery.Append(" from Aircraft_Broker with (NOLOCK)  ")
      sQuery.Append(" inner join Company with (NOLOCK) on broker_comp_id = comp_id and comp_journ_id =0 ")

 
      sQuery.Append(" inner join View_Aircraft_Company_Flat with (NOLOCK) on broker_comp_id=View_Aircraft_Company_Flat.comp_id and cref_journ_id = 0 ")
      sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, False))


      sQuery.Append(" where company.comp_country is not NULL and company.comp_country <> '' ")



      If Trim(searchCriteria.ViewCriteriaAircraftType) <> "" Then
        sQuery.Append(" and amod_type_code in ('" & Trim(searchCriteria.ViewCriteriaAircraftType) & "') ")
      End If



      If amod_id > 0 Then
        sQuery.Append(" and ac_journ_id = 0 ")
        sQuery.Append(" and cref_contact_type in ('99','93','38','2X') ")
        sQuery.Append(" and amod_id = " & amod_id & " ")
      ElseIf Trim(make_name) <> "" Then
        sQuery.Append(" and ac_journ_id = 0 ")
        sQuery.Append(" and cref_contact_type in ('99','93','38','2X') ")
        sQuery.Append(" and amod_make_name = '" & make_name & "'  ")
      End If

      If Trim(country_name) <> "" Then
        sQuery.Append(" and company.comp_country = '" & country_name & "' ")
      End If

      sQuery.Append(" group by company.comp_country ")
      sQuery.Append(" order by  COUNT(distinct broker_comp_id) desc, company.comp_country ")



      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn


      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      Return Nothing
      aError = "Error in ac_dealer_function_by_country() As DataTable: " + ex.Message

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

  Public Function ac_dealer_get_main_comp_id(ByVal comp_id As Long) As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try


      sQuery.Append(" select broker_comp_id, comp_name, comp_city,comp_state, comp_country ")
      sQuery.Append(" from Aircraft_Broker with (NOLOCK) ")
      sQuery.Append(" inner join Company with (NOLOCK) on broker_comp_id = comp_id and comp_journ_id =0 ")
      sQuery.Append(" where broker_main_comp_id = " & comp_id & " And comp_journ_id = 0  ")
      sQuery.Append(" order by comp_name, comp_city, comp_state ")
 


      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn


      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      Return Nothing
      aError = "Error in ac_dealer_get_main_comp_id() As DataTable: " + ex.Message

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

  Public Function ac_dealer_get_models_for_main_comp_id(ByVal comp_id As Long, ByVal make_name As String, ByVal amod_id As Long, ByRef searchCriteria As viewSelectionCriteriaClass, Optional ByVal company_string As String = "", Optional ByVal from_spot As String = "") As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try


      If comp_id > 0 And amod_id > 0 Then
        sQuery.Append(" select distinct  ac_id, ac_ser_no_full, amod_make_name, amod_model_name, amod_id ")
      Else
        sQuery.Append(" select distinct amod_make_name, amod_model_name, COUNT(distinct ac_id) as num_ac , amod_id ")
      End If


      sQuery.Append(" from Aircraft_Broker with (NOLOCK) ")
      sQuery.Append(" inner join Company with (NOLOCK) on broker_comp_id = comp_id and comp_journ_id =0 ") 
      sQuery.Append(" inner join View_Aircraft_Company_Flat with (NOLOCK) on broker_comp_id=View_Aircraft_Company_Flat.comp_id and cref_journ_id = 0 ")

      sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, False))

      sQuery.Append(" where comp_journ_id = 0 ")

      If Trim(company_string) <> "" Then
        sQuery.Append(Replace(company_string, " comp_id", " broker_comp_id"))
      ElseIf Trim(from_spot) = "company" Then
        sQuery.Append(" and broker_comp_id = " & comp_id & "  ")
      Else
        sQuery.Append(" and broker_main_comp_id = " & comp_id & "  ")
      End If

      If Trim(make_name) <> "" Then
        sQuery.Append(" and amod_make_name = '" & make_name & "'  ")
      End If

      If Trim(amod_id) > 0 Then
        sQuery.Append(" and amod_id = " & amod_id & "  ")
      End If

      If Trim(searchCriteria.ViewCriteriaAircraftType) <> "" Then
        sQuery.Append(" and amod_type_code in ('" & Trim(searchCriteria.ViewCriteriaAircraftType) & "') ")
      End If

      sQuery.Append(" and cref_contact_type in ('99','93','38','2X') ")

      If comp_id > 0 And amod_id > 0 Then
        sQuery.Append(" order by ac_ser_no_full asc ")
      Else
        sQuery.Append(" group by amod_make_name, amod_model_name, amod_id ")
        sQuery.Append(" order by COUNT(distinct ac_id) desc,amod_make_name, amod_model_name, amod_id ")
      End If


      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn


      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      Return Nothing
      aError = "Error in ac_dealer_get_models_for_main_comp_id() As DataTable: " + ex.Message

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

  Public Function ac_dealer_get_sales_by_year_main_comp_id(ByVal comp_id As Long, ByVal make_name As String, ByVal amod_id As Long, ByRef searchCriteria As viewSelectionCriteriaClass, Optional ByVal company_string As String = "", Optional ByVal from_spot As String = "") As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try


      sQuery.Append(" select distinct year(journ_date) as TYEAR, COUNT(distinct journ_id) as numtrans  ")
      sQuery.Append(" from Aircraft_Broker with (NOLOCK) ")
      sQuery.Append(" inner join Company with (NOLOCK) on broker_comp_id = comp_id and comp_journ_id =0 ")
      sQuery.Append(" inner join View_Aircraft_Company_History_Flat with (NOLOCK) on broker_comp_id=View_Aircraft_Company_History_Flat.comp_id and cref_journ_id = ac_journ_id ")


      sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, False))


      sQuery.Append(" where journ_subcat_code_part1='WS' ")

      If Trim(company_string) <> "" Then
        sQuery.Append(Replace(company_string, " comp_id", " broker_comp_id"))
      ElseIf Trim(from_spot) = "company" Then
        sQuery.Append(" and broker_comp_id = " & comp_id & "  ")
      Else
        sQuery.Append(" and broker_main_comp_id = " & comp_id & "  ")
      End If


      If amod_id > 0 Then
        sQuery.Append(" and amod_id = " & amod_id & "  ")
      End If

      If Trim(make_name) <> "" Then
        sQuery.Append(" and amod_make_name = '" & make_name & "'  ")
      End If


      If Trim(searchCriteria.ViewCriteriaAircraftType) <> "" Then
        sQuery.Append(" and amod_type_code in ('" & Trim(searchCriteria.ViewCriteriaAircraftType) & "') ")
      End If


      sQuery.Append(" and journ_internal_trans_flag='N' ")
      sQuery.Append(" and cref_contact_type in ('99','93','38','95','96','2P','IV','2X') ")
      sQuery.Append(" and year(journ_date) >= YEAR(GETDATE()) - 5 ")
      sQuery.Append(" group by year(journ_date) ")
      sQuery.Append(" order by year(journ_date) asc ")


      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn


      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      Return Nothing
      aError = "Error in ac_dealer_get_sales_by_year_main_comp_id() As DataTable: " + ex.Message

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

    Public Function ac_dealer_get_relationship_sales_main_comp_id(ByVal comp_id As Long, ByVal make_name As String, ByVal amod_id As Long, ByRef searchCriteria As viewSelectionCriteriaClass, ByVal company_string As String) As DataTable

        Dim temptable As New DataTable
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Dim sQuery = New StringBuilder()

        Try


            sQuery.Append(" select distinct case cref_Contact_type when '93' then 'EXBROKER' when '99' then 'EXBROKER' when '38' then 'SALESREP' when '2X' then 'SALESREP' when '95' then 'SELLER' when '96' then 'PURCHASER' when '2P' then 'ACQUISITION' when 'IV' then 'EXBROKER' else 'OTHER' end as RELTYPE, COUNT(distinct journ_id) as numtrans  ")
            sQuery.Append(" from Aircraft_Broker with (NOLOCK) ")
            sQuery.Append(" inner join Company with (NOLOCK) on broker_comp_id = comp_id and comp_journ_id =0 ")
            sQuery.Append(" inner join View_Aircraft_Company_History_Flat with (NOLOCK) on broker_comp_id=View_Aircraft_Company_History_Flat.comp_id and cref_journ_id = ac_journ_id ")

            sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, False))

            sQuery.Append("  where journ_subcat_code_part1='WS' ")

            If Trim(company_string) <> "" Then
                sQuery.Append(Replace(company_string, " comp_id", " broker_comp_id"))
            Else
                sQuery.Append(" and broker_comp_id = " & comp_id & " ")
            End If

            If amod_id > 0 Then
                sQuery.Append(" and amod_id = " & amod_id & "  ")
            End If


            If Trim(make_name) <> "" Then
                sQuery.Append(" and amod_make_name = '" & make_name & "'  ")
            End If


            If Trim(searchCriteria.ViewCriteriaAircraftType) <> "" Then
                sQuery.Append(" and amod_type_code in ('" & Trim(searchCriteria.ViewCriteriaAircraftType) & "') ")
            End If



            sQuery.Append(" and journ_internal_trans_flag='N' ")
            sQuery.Append(" and cref_contact_type in ('99','93','38','95','96', '2X', 'IV', '2P') ")
            sQuery.Append(" and year(journ_date) >= year(GETDATE()) - 1 ")
            sQuery.Append(" group by case cref_Contact_type when '93' then 'EXBROKER' when '99' then 'EXBROKER' when '38' then 'SALESREP'  when '2X' then 'SALESREP' when '95' then 'SELLER' when '96' then 'PURCHASER' when '2P' then 'ACQUISITION' when 'IV' then 'EXBROKER' else 'OTHER' end ")
            sQuery.Append(" order by RELTYPE ")



            SqlConn.ConnectionString = clientConnectString
            SqlConn.Open()
            SqlCommand.Connection = SqlConn


            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

            SqlCommand.CommandText = sQuery.ToString
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 60

            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                temptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
            End Try

        Catch ex As Exception

            Return Nothing
            aError = "Error in ac_dealer_get_relationship_sales_main_comp_id() As DataTable: " + ex.Message

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

    Public Function get_country_continent_totals_Graphs(ByVal aclist As String, ByVal field_to_sum As String) As DataTable

        Dim temptable As New DataTable
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Dim sQuery = New StringBuilder()

        Try


            sQuery.Append(" select distinct " & field_to_sum & ", count(*) as tcount  ")
            sQuery.Append(" From View_Aircraft_Company_Flat WITH(NOLOCK) ")
            sQuery.Append(" where ac_journ_id = 0 ")

            If Trim(aclist) <> "" Then
                sQuery.Append(" and ac_id in (" & Trim(aclist) & ")")
            End If

            sQuery.Append(" group by " & field_to_sum & "  ")
            sQuery.Append(" order by count(*) desc  ")




            SqlConn.ConnectionString = clientConnectString
            SqlConn.Open()
            SqlCommand.Connection = SqlConn


            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

            SqlCommand.CommandText = sQuery.ToString
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 600

            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                temptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
            End Try

        Catch ex As Exception

            Return Nothing
            aError = "Error in ac_dealer_get_relationship_sales_main_comp_id() As DataTable: " + ex.Message

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



    Public Function ac_get_trans_by_year(ByVal comp_id As Long, ByVal make_name As String, ByVal amod_id As Long, ByRef searchCriteria As viewSelectionCriteriaClass, ByVal company_string As String, ByVal get_roles As String, ByVal show_type As String, ByVal ac_list As String) As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try

      sQuery.Append(" select distinct year(journ_date) as Year_Of , COUNT(distinct journ_id) as numtrans  ")

      sQuery.Append(" from Company with (NOLOCK) ")
      sQuery.Append(" inner join View_Aircraft_Company_History_Flat with (NOLOCK) on Company.comp_id =View_Aircraft_Company_History_Flat.comp_id and cref_journ_id = ac_journ_id ")
 
      sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, False))

      sQuery.Append("  where journ_internal_trans_flag='N' ")

      If Trim(company_string) <> "" Then
        If InStr(company_string, "comp_id") > 0 Then
          sQuery.Append(Replace(company_string, " comp_id", " Company.comp_id "))
        Else
          sQuery.Append(" and Company.comp_id  in (" & company_string & ") ")
        End If
      ElseIf comp_id > 0 Then
        sQuery.Append(" and Company.comp_id  = " & comp_id & " ")
      ElseIf Trim(ac_list) <> "" Then
        sQuery.Append(" and View_Aircraft_Company_History_Flat.ac_id in (" & Trim(ac_list) & ") ")
      End If

      If amod_id > 0 Then
        sQuery.Append(" and amod_id = " & amod_id & "  ")
      End If


      If Trim(make_name) <> "" Then
        sQuery.Append(" and amod_make_name = '" & make_name & "'  ")
      End If


      If Trim(searchCriteria.ViewCriteriaAircraftType) <> "" Then
        sQuery.Append(" and amod_type_code in ('" & Trim(searchCriteria.ViewCriteriaAircraftType) & "') ")
      End If


      If Trim(show_type) = "operated" Then
        sQuery.Append(" AND cref_operator_flag IN('Y','O')  ")
      ElseIf Trim(show_type) = "own_operated" Then
        sQuery.Append(" AND (cref_contact_type IN('95','96') or cref_operator_flag IN('Y','O')) ")
      ElseIf Trim(show_type) = "brokered" Then
        sQuery.Append(" AND cref_contact_type IN('99','2P','IV') ")
      ElseIf Trim(show_type) = "managed" Then
        sQuery.Append(" AND cref_contact_type IN('31') ")
      End If

      'sQuery.Append(" and cref_contact_type in ('99','93','38','95','96', '2X', 'IV', '2P') ")


      sQuery.Append(" and  journ_subcat_code_part1 not in ('OM','MA','MS') ")
      '   sQuery.Append(" AND journ_date >= (getdate() - 365) ")


      sQuery.Append(" group by year(journ_date) ")







      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn


      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 600

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      Return Nothing
      aError = "Error in ac_dealer_get_relationship_sales_main_comp_id() As DataTable: " + ex.Message

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
  Public Function ac_dealer_get_relationship_all_trans_main_comp_id(ByVal comp_id As Long, ByVal make_name As String, ByVal amod_id As Long, ByRef searchCriteria As viewSelectionCriteriaClass, ByVal company_string As String, ByVal get_roles As String, ByVal show_type As String, ByVal ac_list As String) As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try

      If Trim(get_roles) = "Y" Then
        sQuery.Append(" select distinct Journal_Category.jcat_subcategory_name, COUNT(distinct journ_id) as numtrans  ")
      Else
        sQuery.Append(" select distinct case cref_Contact_type when '93' then 'EXBROKER' when '99' then 'EXBROKER' when '38' then 'SALESREP' when '2X' then 'SALESREP' when '95' then 'SELLER' when '96' then 'PURCHASER' when '2P' then 'ACQUISITION' when 'IV' then 'EXBROKER' else 'OTHER' end as RELTYPE, COUNT(distinct journ_id) as numtrans  ")
      End If

      sQuery.Append(" from Company with (NOLOCK) ")
      sQuery.Append(" inner join View_Aircraft_Company_History_Flat with (NOLOCK) on Company.comp_id =View_Aircraft_Company_History_Flat.comp_id and cref_journ_id = ac_journ_id ")

      If Trim(get_roles) = "Y" Then
        sQuery.Append(" inner join Journal_Category with (NOLOCK) on Journal_Category.jcat_subcategory_code = journ_subcat_code_part1 ")
      End If

      sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, False))

      sQuery.Append("  where journ_internal_trans_flag='N' ")

      If Trim(company_string) <> "" Then
        If InStr(company_string, "comp_id") > 0 Then
          sQuery.Append(Replace(company_string, " comp_id", " Company.comp_id "))
        Else
          sQuery.Append(" and Company.comp_id  in (" & company_string & ") ")
        End If
      ElseIf comp_id > 0 Then
        sQuery.Append(" and Company.comp_id  = " & comp_id & " ")
      ElseIf Trim(ac_list) <> "" Then
        sQuery.Append(" and View_Aircraft_Company_History_Flat.ac_id in (" & Trim(ac_list) & ") ")
      End If

      If amod_id > 0 Then
        sQuery.Append(" and amod_id = " & amod_id & "  ")
      End If


      If Trim(make_name) <> "" Then
        sQuery.Append(" and amod_make_name = '" & make_name & "'  ")
      End If


      If Trim(searchCriteria.ViewCriteriaAircraftType) <> "" Then
        sQuery.Append(" and amod_type_code in ('" & Trim(searchCriteria.ViewCriteriaAircraftType) & "') ")
      End If


      If Trim(show_type) = "operated" Then
        sQuery.Append(" AND cref_operator_flag IN('Y','O')  ")
      ElseIf Trim(show_type) = "own_operated" Then
        sQuery.Append(" AND (cref_contact_type IN('95','96') or cref_operator_flag IN('Y','O')) ")
      ElseIf Trim(show_type) = "brokered" Then
        sQuery.Append(" AND cref_contact_type IN('99','2P','IV') ")
      ElseIf Trim(show_type) = "managed" Then
        sQuery.Append(" AND cref_contact_type IN('31') ")
      End If

      'sQuery.Append(" and cref_contact_type in ('99','93','38','95','96', '2X', 'IV', '2P') ")


      sQuery.Append(" and  journ_subcat_code_part1 not in ('OM','MA','MS') ")
      '   sQuery.Append(" AND journ_date >= (getdate() - 365) ")

      If Trim(get_roles) = "Y" Then
        sQuery.Append(" group by Journal_Category.jcat_subcategory_name ")
      Else
        sQuery.Append(" group by case cref_Contact_type when '93' then 'EXBROKER' when '99' then 'EXBROKER' when '38' then 'SALESREP'  when '2X' then 'SALESREP' when '95' then 'SELLER' when '96' then 'PURCHASER' when '2P' then 'ACQUISITION' when 'IV' then 'EXBROKER' else 'OTHER' end ")
        sQuery.Append(" order by RELTYPE ")
      End If





      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn


      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 600

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      Return Nothing
      aError = "Error in ac_dealer_get_relationship_sales_main_comp_id() As DataTable: " + ex.Message

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

    Public Function ac_dealer_sales_by_model(ByVal comp_id As Long, ByVal make_name As String, ByVal amod_id As Long, ByRef searchCriteria As viewSelectionCriteriaClass, ByVal company_string As String, Optional ByVal display_ac As Boolean = False) As DataTable

        Dim temptable As New DataTable
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Dim sQuery = New StringBuilder()

        Try


            sQuery.Append(" select distinct ")
            If amod_id > 0 And comp_id > 0 Then
                sQuery.Append(" year(journ_date) as year_of, ")
            End If
            sQuery.Append(" amod_make_name, amod_model_name, ")

            If display_ac = True Then
                sQuery.Append(" ac_ser_no_full ,  ")
            End If

            sQuery.Append(" count(distinct journ_id) As TCOUNT ")
            sQuery.Append(" from Aircraft_Broker With (NOLOCK)  ")
            sQuery.Append(" inner join Company With (NOLOCK) On broker_comp_id = comp_id And comp_journ_id = 0  ")
            sQuery.Append(" inner join View_Aircraft_Company_History_Flat With (NOLOCK) On broker_comp_id=View_Aircraft_Company_History_Flat.comp_id And cref_journ_id = ac_journ_id  ")

            sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, False))

            sQuery.Append("  where journ_subcat_code_part1='WS'  ")

            If Trim(company_string) <> "" Then
                sQuery.Append(Replace(company_string, " comp_id", " broker_comp_id"))
            Else
                sQuery.Append(" and broker_comp_id = " & comp_id & " ")
            End If



            sQuery.Append(" and journ_internal_trans_flag='N'  ")

            If amod_id > 0 Then
                sQuery.Append(" and amod_id = " & amod_id & "  ")
            End If

            If Trim(make_name) <> "" Then
                sQuery.Append(" and amod_make_name = '" & make_name & "'  ")
            End If


            If Trim(searchCriteria.ViewCriteriaAircraftType) <> "" Then
                sQuery.Append(" and amod_type_code in ('" & Trim(searchCriteria.ViewCriteriaAircraftType) & "') ")
            End If


            sQuery.Append(" and cref_contact_type in ('99','93','38','95','96','IV', '2P','2X') ")

            'get last 5 years, grouped by year
            If amod_id > 0 And comp_id > 0 Then
                sQuery.Append(" and year(journ_date) >= YEAR(GETDATE()) - 5 ")
                sQuery.Append(" group by YEAR(journ_date) , amod_make_name, amod_model_name ")

                If display_ac = True Then
                    sQuery.Append(", ac_ser_no_full  ")
                End If
                sQuery.Append(" order by YEAR(journ_date) desc ")
            Else
                sQuery.Append(" and year(journ_date) >= YEAR(GETDATE()) - 1 ")
                sQuery.Append(" group by amod_make_name, amod_model_name ")

                If display_ac = True Then
                    sQuery.Append(", ac_ser_no_full  ")
                End If
                sQuery.Append(" order by TCOUNT desc ")
            End If






            SqlConn.ConnectionString = clientConnectString
            SqlConn.Open()
            SqlCommand.Connection = SqlConn


            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

            SqlCommand.CommandText = sQuery.ToString
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 60

            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                temptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
            End Try

        Catch ex As Exception

            Return Nothing
            aError = "Error in ac_dealer_get_relationship_sales_main_comp_id() As DataTable: " + ex.Message

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


End Class
