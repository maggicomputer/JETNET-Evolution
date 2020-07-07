' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/Entity Classes/manufacturer_view_functions.vb $
'$$Author: Matt $
'$$Date: 3/29/20 10:03a $
'$$Modtime: 3/29/20 9:50a $
'$$Revision: 6 $
'$$Workfile: manufacturer_view_functions.vb $
'
' ********************************************************************************

<System.Serializable()> Public Class manufacturer_view_functions
  Private aError As String
  Private clientConnectString As String
  Private adminConnectString As String

  Private starConnectString As String
  Private cloudConnectString As String
  Private serverConnectString As String
  Dim aclsData_Manager_SQL As New clsData_Manager_SQL

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

#Region "manufacturer_view_functions"

  Public Function get_manufacturer_companies_info(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal use_roll As String) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      If searchCriteria.ViewCriteriaAmodID > -1 Then
        sQuery.Append("SELECT DISTINCT comp_id, comp_country, comp_name As MfrCompany,")
      Else
        sQuery.Append("SELECT DISTINCT top 100 comp_id, comp_country, comp_name As MfrCompany,")
      End If


      sQuery.Append("sum(case when ac_lifecycle_stage = 1 then 1 else 0 end) as InProduction, ")
      sQuery.Append("sum(case when ac_lifecycle_stage = 2 then 1 else 0 end) as WithMfr, ")
      sQuery.Append("sum(case when ac_lifecycle_stage = 3 then 1 else 0 end) as InOperation, ")
      sQuery.Append("sum(case when ac_lifecycle_stage = 4 AND (ac_status NOT LIKE '%Stored%') then 1 else 0 end) as WrittenOff, ")
      sQuery.Append("sum(case when ac_lifecycle_stage = 4 AND (ac_status LIKE '%Stored%') then 1 else 0 end) as Stored, ")
      sQuery.Append("sum(case when ac_forsale_flag='Y' AND (ac_status LIKE 'For Sale%') then 1 else 0 end) as ForSale, ")
      sQuery.Append("COUNT(*) as TCOUNT ")

      'sQuery.Append(" (")
      'sQuery.Append("SELECT COUNT(ac_id) FROM Aircraft WITH (NOLOCK)")
      'sQuery.Append(" INNER JOIN Aircraft_Model WITH (NOLOCK) ON ac_amod_id = amod_id")
      'sQuery.Append(" WHERE (ac_journ_id = 0)")
      'sQuery.Append(" AND (amod_manufacturer_comp_id = comp_id)")
      'sQuery.Append(" AND (amod_customer_flag = 'Y')")
      'sQuery.Append(" AND (ac_lifecycle_stage = 1)")

      'Select Case CInt(searchCriteria.ViewCriteriaAirframeType)
      '  Case Constants.VIEW_EXECUTIVE
      '    sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'E')")
      '  Case Constants.VIEW_JETS
      '    sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'J')")
      '  Case Constants.VIEW_TURBOPROPS
      '    sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'T')")
      '  Case Constants.VIEW_PISTONS
      '    sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'P')")
      '  Case Constants.VIEW_HELICOPTERS
      '    sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='R' AND amod_type_code in ('T','P'))")
      'End Select

      'If searchCriteria.ViewCriteriaAmodID > -1 Then
      '  sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
      'ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
      '  sQuery.Append(Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
      'End If

      'If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
      '  sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))
      'Else
      '  sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True))
      'End If

      'sQuery.Append(" ) As InProduction,")
      'sQuery.Append(" (")
      'sQuery.Append("SELECT COUNT(ac_id) FROM Aircraft WITH (NOLOCK)")
      'sQuery.Append(" INNER JOIN Aircraft_Model WITH (NOLOCK) ON ac_amod_id = amod_id")
      'sQuery.Append(" WHERE (ac_journ_id = 0)")
      'sQuery.Append(" AND (amod_manufacturer_comp_id = comp_id)")
      'sQuery.Append(" AND (amod_customer_flag = 'Y')")
      'sQuery.Append(" AND (ac_lifecycle_stage = 2)")

      'Select Case CInt(searchCriteria.ViewCriteriaAirframeType)
      '  Case Constants.VIEW_EXECUTIVE
      '    sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'E')")
      '  Case Constants.VIEW_JETS
      '    sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'J')")
      '  Case Constants.VIEW_TURBOPROPS
      '    sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'T')")
      '  Case Constants.VIEW_PISTONS
      '    sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'P')")
      '  Case Constants.VIEW_HELICOPTERS
      '    sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='R' AND amod_type_code in ('T','P'))")
      'End Select

      'If searchCriteria.ViewCriteriaAmodID > -1 Then
      '  sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
      'ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
      '  sQuery.Append(Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
      'End If

      'If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
      '  sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))
      'Else
      '  sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True))
      'End If

      'sQuery.Append(" ) As WithMfr,")
      'sQuery.Append(" (")
      'sQuery.Append("SELECT COUNT(ac_id) FROM Aircraft WITH (NOLOCK)")
      'sQuery.Append(" INNER JOIN Aircraft_Model WITH (NOLOCK) ON ac_amod_id = amod_id")
      'sQuery.Append(" WHERE (ac_journ_id = 0)")
      'sQuery.Append(" AND (amod_manufacturer_comp_id = comp_id)")
      'sQuery.Append(" AND (amod_customer_flag = 'Y')")
      'sQuery.Append(" AND (ac_lifecycle_stage = 3)")

      'Select Case CInt(searchCriteria.ViewCriteriaAirframeType)
      '  Case Constants.VIEW_EXECUTIVE
      '    sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'E')")
      '  Case Constants.VIEW_JETS
      '    sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'J')")
      '  Case Constants.VIEW_TURBOPROPS
      '    sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'T')")
      '  Case Constants.VIEW_PISTONS
      '    sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'P')")
      '  Case Constants.VIEW_HELICOPTERS
      '    sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='R' AND amod_type_code in ('T','P'))")
      'End Select

      'If searchCriteria.ViewCriteriaAmodID > -1 Then
      '  sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
      'ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
      '  sQuery.Append(Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
      'End If

      'If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
      '  sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))
      'Else
      '  sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True))
      'End If

      'sQuery.Append(" ) As InOperation,")
      'sQuery.Append(" (")
      'sQuery.Append("SELECT COUNT(ac_id) FROM Aircraft WITH (NOLOCK)")
      'sQuery.Append(" INNER JOIN Aircraft_Model WITH (NOLOCK) ON ac_amod_id = amod_id")
      'sQuery.Append(" WHERE (ac_journ_id = 0)")
      'sQuery.Append(" AND (amod_manufacturer_comp_id = comp_id)")
      'sQuery.Append(" AND (amod_customer_flag = 'Y')")
      'sQuery.Append(" AND (ac_lifecycle_stage = 4)")
      'sQuery.Append(" AND (ac_status NOT LIKE '%Stored%')")

      'Select Case CInt(searchCriteria.ViewCriteriaAirframeType)
      '  Case Constants.VIEW_EXECUTIVE
      '    sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'E')")
      '  Case Constants.VIEW_JETS
      '    sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'J')")
      '  Case Constants.VIEW_TURBOPROPS
      '    sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'T')")
      '  Case Constants.VIEW_PISTONS
      '    sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'P')")
      '  Case Constants.VIEW_HELICOPTERS
      '    sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='R' AND amod_type_code in ('T','P'))")
      'End Select

      'If searchCriteria.ViewCriteriaAmodID > -1 Then
      '  sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
      'ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
      '  sQuery.Append(Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
      'End If

      'If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
      '  sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))
      'Else
      '  sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True))
      'End If

      'sQuery.Append(" ) As WrittenOff,")
      'sQuery.Append(" (")
      'sQuery.Append("SELECT COUNT(ac_id) FROM Aircraft WITH (NOLOCK)")
      'sQuery.Append(" INNER JOIN Aircraft_Model WITH (NOLOCK) ON ac_amod_id = amod_id")
      'sQuery.Append(" WHERE (ac_journ_id = 0)")
      'sQuery.Append(" AND (amod_manufacturer_comp_id = comp_id)")
      'sQuery.Append(" AND (amod_customer_flag = 'Y')")
      'sQuery.Append(" AND (ac_lifecycle_stage = 4)")
      'sQuery.Append(" AND (ac_status LIKE '%Stored%')")

      'Select Case CInt(searchCriteria.ViewCriteriaAirframeType)
      '  Case Constants.VIEW_EXECUTIVE
      '    sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'E')")
      '  Case Constants.VIEW_JETS
      '    sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'J')")
      '  Case Constants.VIEW_TURBOPROPS
      '    sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'T')")
      '  Case Constants.VIEW_PISTONS
      '    sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'P')")
      '  Case Constants.VIEW_HELICOPTERS
      '    sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='R' AND amod_type_code in ('T','P'))")
      'End Select

      'If searchCriteria.ViewCriteriaAmodID > -1 Then
      '  sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
      'ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
      '  sQuery.Append(Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
      'End If

      'If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
      '  sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))
      'Else
      '  sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True))
      'End If

      'sQuery.Append(" ) As Stored,")
      'sQuery.Append(" (")
      'sQuery.Append("SELECT COUNT(ac_id) FROM Aircraft WITH (NOLOCK)")
      'sQuery.Append(" INNER JOIN Aircraft_Model WITH (NOLOCK) ON ac_amod_id = amod_id")
      'sQuery.Append(" WHERE (ac_journ_id = 0)")
      'sQuery.Append(" AND (amod_manufacturer_comp_id = comp_id)")
      'sQuery.Append(" AND (amod_customer_flag = 'Y')")
      'sQuery.Append(" AND (ac_forsale_flag = 'Y')")
      'sQuery.Append(" AND (ac_status LIKE 'For Sale%')")

      'Select Case CInt(searchCriteria.ViewCriteriaAirframeType)
      '  Case Constants.VIEW_EXECUTIVE
      '    sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'E')")
      '  Case Constants.VIEW_JETS
      '    sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'J')")
      '  Case Constants.VIEW_TURBOPROPS
      '    sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'T')")
      '  Case Constants.VIEW_PISTONS
      '    sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'P')")
      '  Case Constants.VIEW_HELICOPTERS
      '    sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='R' AND amod_type_code in ('T','P'))")
      'End Select

      'If searchCriteria.ViewCriteriaAmodID > -1 Then
      '  sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
      'ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
      '  sQuery.Append(Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
      'End If

      'If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
      '  sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))
      'Else
      '  sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True))
      'End If

      'sQuery.Append(" ) As Forsale")

      sQuery.Append(" FROM Company WITH (NOLOCK)")
      sQuery.Append(" INNER JOIN Aircraft_Model WITH (NOLOCK) ON comp_id = amod_manufacturer_comp_id AND comp_journ_id = 0")
      sQuery.Append(" inner join aircraft with (NOLOCK) on ac_journ_id = 0 and ac_amod_id = amod_id ")

      Select Case CInt(searchCriteria.ViewCriteriaAirframeType)
        Case Constants.VIEW_EXECUTIVE
          sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'E')")
        Case Constants.VIEW_JETS
          sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'J')")
        Case Constants.VIEW_TURBOPROPS
          sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'T')")
        Case Constants.VIEW_PISTONS
          sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'P')")
        Case Constants.VIEW_HELICOPTERS
          sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='R' AND amod_type_code in ('T','P'))")
      End Select

      sQuery.Append(" where (amod_manufacturer_comp_id > 0)")
      If searchCriteria.ViewCriteriaCompanyID > 0 Then
        If use_roll = "Y" Then 
          sQuery.Append(" and comp_id in (select distinct RelCompID from ReturnAllCompanyLocationsByCompId(" & searchCriteria.ViewCriteriaCompanyID.ToString & "))")  
        Else
          sQuery.Append(Constants.cAndClause + "comp_id = " + searchCriteria.ViewCriteriaCompanyID.ToString) 
        End If
      End If

      If searchCriteria.ViewCriteriaAmodID > -1 Then
        sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
        sQuery.Append(Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
      End If

      If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
        sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, True))
      Else
        sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True))
      End If

      sQuery.Append("  group by comp_id, comp_country, comp_name ")

      sQuery.Append(" ORDER BY InOperation desc")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_manufacturer_companies_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
        aError = "Error in get_manufacturer_companies_info load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_manufacturer_companies_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

  Public Function get_manufacturer_by_year(ByVal comp_id As Long, ByVal amod_id As Long, ByVal lifecycle_stage As String, ByVal searchCriteria As viewSelectionCriteriaClass, ByVal use_roll As String)

    Dim aTempTable As New DataTable
    Dim sql As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Try

      sql &= " SELECT DISTINCT ac_mfr_year,COUNT(distinct ac_id) as tcount"
      sql &= " from Aircraft with (NOLOCK)"
      sql &= " inner join Aircraft_Model with (NOLOCK) on amod_id = ac_amod_id"
      If Trim(use_roll) = "Y" Then
        sql &= " where amod_manufacturer_comp_id in (select distinct RelCompID from ReturnAllCompanyLocationsByCompId(" & comp_id & ")) And ac_journ_id = 0 "
      Else
        sql &= " where amod_manufacturer_comp_id = " & comp_id & " And ac_journ_id = 0 "
      End If


      If amod_id > 0 Then
        sql &= " AND ac_amod_id = " & amod_id
      End If

      If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
        sql &= " " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False)
      Else
        sql &= " " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True)
      End If

      If Trim(lifecycle_stage) <> "" Then
        sql &= " AND ac_lifecycle_stage = '" & lifecycle_stage & "' "
      End If


      sql &= " and ac_mfr_year is NOT NULL"
      sql &= " group by ac_mfr_year"
      sql &= " order by ac_mfr_year"

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>DisplayYachtForGivenCompanyByCompanyID(ByVal companyID As Long) As DataTable</b><br />" & sql


      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn

      SqlCommand.CommandText = sql
      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      Try
        aTempTable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = aTempTable.GetErrors()
      End Try

    Catch ex As Exception
      Return Nothing
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing

    End Try

    Return aTempTable

  End Function

  Public Sub views_display_manufacturer_companies(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, Optional ByVal bHasMaster As Boolean = False, Optional ByVal from_spot As String = "", Optional ByVal use_roll As String = "N")

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder
    Dim strOut As New StringBuilder
    Dim toggleRowColor As Boolean = False

    Dim sTmpTitle As String = ""
    Dim sTitle As String = ""
    Dim sTmpCompanyName As String = ""

    Dim sLinkString As String = ""
    Dim sReportString As String = ""

    Dim nTotal As Integer = 0

    Dim nInProduction As Integer = 0
    Dim nWithMfr As Integer = 0
    Dim nInOperation As Integer = 0
    Dim nWrittenOff As Integer = 0
    Dim nStored As Integer = 0
    Dim nForsale As Integer = 0

    Dim sTempCompanyHtml As String = ""
    Dim sColSpan As String = ""
    Dim font_text_start As String = ""
    Dim font_text_end As String = ""

    Dim bAerodexFlag As Boolean = HttpContext.Current.Session.Item("localPreferences").AerodexFlag.ToString.ToLower()
    Dim bgcolor As String = ""
    Dim font_text_title As String = ""
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


      If Trim(from_spot) = "company" Then
        font_text_start = "<font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>"
        font_text_end = "</font>"
      Else
        font_text_start = ""
        font_text_end = ""
      End If

      If Not bAerodexFlag Then
        sColSpan = "'8'"
      Else
        sColSpan = "'7'"
      End If

      If Trim(use_roll) = "Y" Then
        results_table = get_manufacturer_companies_info(searchCriteria, "Y")
      Else
        results_table = get_manufacturer_companies_info(searchCriteria, "N")
      End If



      If searchCriteria.ViewCriteriaCompanyID > 0 Then
        sTmpCompanyName = commonEvo.get_company_name_fromID(searchCriteria.ViewCriteriaCompanyID, 0, False, True, "")
        sTmpTitle += " : " + sTmpCompanyName.Trim
      End If

      If searchCriteria.ViewCriteriaAmodID > -1 Then
        sTmpTitle += " - " + commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaAmodID, False, "")
      ElseIf searchCriteria.ViewCriteriaMakeAmodID > -1 Then
        sTmpTitle += " - " + commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaMakeAmodID, True, "")
      End If

      If String.IsNullOrEmpty(sTmpTitle) Then
        sTmpTitle = " : Top 100 COMPANIES"
      End If

      If Not IsNothing(results_table) Then

        sTitle = "MANUFACTURER SUMMARY" + sTmpTitle

        If Trim(from_spot) = "pdf" Then
          htmlOut.Append("<tr class='header_row'>")
        ElseIf Trim(from_spot) = "company" Then
          htmlOut.Append("<table id='manufacturerCompaniesInnerTable' cellspacing='0' cellpadding='2' border='0' width='100%' class='data_aircraft_grid'>")
          htmlOut.Append("<tr class='header_row'>")
        Else

          If Not searchCriteria.ViewCriteriaIsReport Then
            If results_table.Rows.Count > 15 Then
              htmlOut.Append("<div valign=""top"" style='height:400px; overflow: auto;'><p>")
            End If
          End If

                    If HttpContext.Current.Session.Item("localUser").crmDemoUserFlag = True Then
                    Else
                        If Not searchCriteria.ViewCriteriaIsReport And searchCriteria.ViewID > 2 And searchCriteria.ViewCriteriaCompanyID = 0 And searchCriteria.ViewCriteriaAmodID = -1 Then
                            sReportString = "<div align=""left"" style=""padding-left:8px; color:""><strong><a class=""White"" href=""View_Template.aspx?" + IIf(Not bHasMaster, "noMaster=false&", "") + "ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName + "&viewCompany=0&amod_id=-1&bIsReport=Y&bIsMfrModel=N"">Export to Excel</a></strong></div>"
                        End If
                    End If


                    htmlOut.Append("<table id='manufacturerCompaniesInnerTable' cellspacing='0' cellpadding='2' border='0' width='100%' class='module'>")
          htmlOut.Append("<tr><td align=""center"" valign=""middle"" class=""header"" colspan=" + sColSpan + ">" + sTitle + sReportString + "</td></tr>")

          If searchCriteria.ViewCriteriaCompanyID > 0 And searchCriteria.ViewCriteriaAmodID > -1 Then

            sLinkString = "<tr><td valign='top' align='left' class='seperator' colspan=" + sColSpan + "><br />Click <a href='view_template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName + "&viewCompany=0&amod_id=" + searchCriteria.ViewCriteriaAmodID.ToString + "'>Here</a> to Clear Company Data and Search for "
            sLinkString += searchCriteria.ViewCriteriaAircraftModel + " under " + sTmpTitle.Trim + " Models</td></tr>"

            If searchCriteria.ViewCriteriaAmodID > -1 Then
              sLinkString = "<tr><td valign='top' align='left' class='seperator' colspan=" + sColSpan + "><br />Click <a href='view_template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName + "&viewCompany=" + searchCriteria.ViewCriteriaCompanyID.ToString + "&amod_id=-1'>Here</a> to Clear Model Data and Search for "
              sLinkString += sTmpCompanyName.Trim + " under " + sTmpTitle.Trim + " Manufacturers</td></tr>"
            End If

          ElseIf searchCriteria.ViewCriteriaAmodID > -1 Then

            sLinkString = "<tr><td valign='top' align='left' class='seperator' colspan=" + sColSpan + "><br />Click <a href='view_template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName + "&viewCompany=" + searchCriteria.ViewCriteriaCompanyID.ToString + "&amod_id=-1'>Here</a> to Clear Model and Search for All Models"
            sLinkString += "</td></tr>"

          ElseIf searchCriteria.ViewCriteriaCompanyID > 0 Then

            sLinkString = "<tr><td valign='top' align='left' class='seperator' colspan=" + sColSpan + "><br />Click <a href='view_template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName + "&viewCompany=0&amod_id=-1'>Here</a> to Clear Company Data and Search for All Manufacturers"
            sLinkString += "</td></tr>"

          End If

          If Not searchCriteria.ViewCriteriaIsReport Then
            If Not String.IsNullOrEmpty(sLinkString.Trim) And searchCriteria.ViewID > 2 Then
              htmlOut.Append(sLinkString)
            End If
          End If
          htmlOut.Append("<tr>")
        End If


        strOut.Append("<td valign='top' align='left' width='15%' class='seperator'><strong>" & font_text_start & "Company&nbsp;Name<br /><em>(Country)</em>" & font_text_end & "</strong></td>")
        strOut.Append("<td valign='top' align='center' class='seperator'><strong>" & font_text_start & "In<br />Prod" & font_text_end & "</strong></td>")
        strOut.Append("<td valign='top' align='center' class='seperator'><strong>" & font_text_start & "With<br />Mfr" & font_text_end & "</strong></td>")
        strOut.Append("<td valign='top' align='center' class='seperator'><strong>" & font_text_start & "In<br />Operation" & font_text_end & "</strong></td>")
        strOut.Append("<td valign='top' align='center' class='seperator'><strong>" & font_text_start & "Written<br />Off" & font_text_end & "</strong></td>")
        strOut.Append("<td valign='top' align='center' class='seperator'><strong>" & font_text_start & "In<br />Storage" & font_text_end & "</strong></td>")

        If Not bAerodexFlag Then
          strOut.Append("<td valign='top' align='center' class='seperator'><strong>" & font_text_start & "For<br />Sale" & font_text_end & "</strong></td>")
        End If

        strOut.Append("<td valign='top' align='center' class='seperator'><strong>" & font_text_start & "&nbsp;<br />Total" & font_text_end & "</strong></td>")
        strOut.Append("</tr>")

        If results_table.Rows.Count > 0 Then

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
                strOut.Append("<tr class='alt_row'>")
                toggleRowColor = True
              Else
                strOut.Append("<tr bgcolor='white'>")
                toggleRowColor = False
              End If
            End If

            nTotal = 0
            nInProduction = 0
            nWithMfr = 0
            nInOperation = 0
            nWrittenOff = 0
            nStored = 0
            nForsale = 0

            If Not IsDBNull(r.Item("InProduction")) Then
              If Not String.IsNullOrEmpty(r.Item("InProduction").ToString.Trim) Then
                nInProduction = CLng(r.Item("InProduction").ToString)
              End If
            End If

            If Not IsDBNull(r.Item("WithMfr")) Then
              If Not String.IsNullOrEmpty(r.Item("WithMfr").ToString.Trim) Then
                nWithMfr = CLng(r.Item("WithMfr").ToString)
              End If
            End If

            If Not IsDBNull(r.Item("InOperation")) Then
              If Not String.IsNullOrEmpty(r.Item("InOperation").ToString.Trim) Then
                nInOperation = CLng(r.Item("InOperation").ToString)
              End If
            End If

            If Not IsDBNull(r.Item("WrittenOff")) Then
              If Not String.IsNullOrEmpty(r.Item("WrittenOff").ToString.Trim) Then
                nWrittenOff = CLng(r.Item("WrittenOff").ToString)
              End If
            End If

            If Not IsDBNull(r.Item("Stored")) Then
              If Not String.IsNullOrEmpty(r.Item("Stored").ToString.Trim) Then
                nStored = CLng(r.Item("Stored").ToString)
              End If
            End If

            If Not bAerodexFlag Then
              If Not IsDBNull(r.Item("Forsale")) Then
                If Not String.IsNullOrEmpty(r.Item("Forsale").ToString.Trim) Then
                  nForsale = CLng(r.Item("Forsale").ToString)
                End If
              End If
            End If

            nTotal = nInProduction + nWithMfr + nInOperation + nWrittenOff + nStored

            If nTotal > 0 Then

              If Not searchCriteria.ViewCriteriaIsReport Then
                strOut.Append("<td valign='top' align='right' class='border_bottom_right'>" & font_text_start)

                If aclsData_Manager_SQL.is_aerodex_insight() = True Then
                                    strOut.Append("<a class='underline' onclick=""javascript:load('DisplayCompanyDetail.aspx?compid=" & r.Item("comp_id").ToString & "&journid=0&amod_id=" & searchCriteria.ViewCriteriaAmodID & "&use_insight_manu=Y&use_insight_roll=Y','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');"">")
                                Else
                  strOut.Append("<a href='view_template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName + "&viewCompany=" + r.Item("comp_id").ToString + "'>")
                End If

                strOut.Append(r.Item("MfrCompany").ToString.Trim + "</a><br /><em>(" + Replace(r.Item("comp_country").ToString.Trim, Constants.cSingleSpace, Constants.cHTMLnbsp) + ")</em>")
                strOut.Append(font_text_end & "</td>")
              Else
                strOut.Append("<td valign='top' align='left' class='border_bottom_right'>" & font_text_start)
                strOut.Append(r.Item("MfrCompany").ToString.Trim + "<br /><em>(" + Replace(r.Item("comp_country").ToString.Trim, Constants.cSingleSpace, Constants.cHTMLnbsp) + ")</em>")
                strOut.Append(font_text_end & "</td>")
              End If

              If nInProduction > 0 Then
                strOut.Append("<td valign='top' align='right' class='border_bottom_right'>" & font_text_start & "" + FormatNumber(nInProduction, TriState.False, TriState.True, TriState.False, TriState.True).ToString + "&nbsp;" & font_text_end & "</td>")
              Else
                strOut.Append("<td valign='top' align='right' class='border_bottom_right'>" & font_text_start & "N/A" & font_text_end & "</td>")
              End If

              If nWithMfr > 0 Then
                strOut.Append("<td valign='top' align='right' class='border_bottom_right'>" & font_text_start & "" + FormatNumber(nWithMfr, TriState.False, TriState.True, TriState.False, TriState.True).ToString + "&nbsp;" & font_text_end & "</td>")
              Else
                strOut.Append("<td valign='top' align='right' class='border_bottom_right'>" & font_text_start & "N/A" & font_text_end & "</td>")
              End If

              If nInOperation > 0 Then
                strOut.Append("<td valign='top' align='right' class='border_bottom_right'>" & font_text_start & "" + FormatNumber(nInOperation, TriState.False, TriState.True, TriState.False, TriState.True).ToString + "&nbsp;" & font_text_end & "</td>")
              Else
                strOut.Append("<td valign='top' align='right' class='border_bottom_right'>" & font_text_start & "N/A" & font_text_end & "</td>")
              End If

              If nWrittenOff > 0 Then
                strOut.Append("<td valign='top' align='right' class='border_bottom_right'>" & font_text_start & "" + FormatNumber(nWrittenOff, TriState.False, TriState.True, TriState.False, TriState.True).ToString + "&nbsp;" & font_text_end & "</td>")
              Else
                strOut.Append("<td valign='top' align='right' class='border_bottom_right'>" & font_text_start & "N/A" & font_text_end & "</td>")
              End If

              If nStored > 0 Then
                strOut.Append("<td valign='top' align='right' class='border_bottom_right'>" & font_text_start & "" + FormatNumber(nStored, TriState.False, TriState.True, TriState.False, TriState.True).ToString + "&nbsp;" & font_text_end & "</td>")
              Else
                strOut.Append("<td valign='top' align='right' class='border_bottom_right'>" & font_text_start & "N/A" & font_text_end & "</td>")
              End If

              If Not bAerodexFlag Then
                If nForsale > 0 Then
                  strOut.Append("<td valign='top' align='right' class='border_bottom_right'>" & font_text_start & "" + FormatNumber(nForsale, TriState.False, TriState.True, TriState.False, TriState.True).ToString + "&nbsp;" & font_text_end & "</td>")
                Else
                  strOut.Append("<td valign='top' align='right' class='border_bottom_right'>" & font_text_start & "N/A" & font_text_end & "</td>")
                End If
              End If

              strOut.Append("<td valign='top' align='right' class='border_bottom_right'>" & font_text_start & "" + FormatNumber(nTotal, TriState.False, TriState.True, TriState.False, TriState.True).ToString + "&nbsp;" & font_text_end & "</td>")

              strOut.Append("</tr>")

            End If

          Next
        Else
          strOut.Append("<tr><td valign=""top"" align=""left"" class=""border_bottom_right"" colspan=" + sColSpan + "><br/>" & font_text_start & "No Data Available " + sTitle.Trim + "" & font_text_end & "</td></tr>")
        End If
      Else
        strOut.Append("<tr><td valign=""top"" align=""left"" class=""border_bottom_right"" colspan=" + sColSpan + "><br/>" & font_text_start & "No Data Available " + sTitle.Trim + "" & font_text_end & "</td></tr>")
      End If

      strOut.Append("</table>")

      If Not searchCriteria.ViewCriteriaIsReport Then

        If results_table.Rows.Count > 15 Then
          strOut.Append("</p></div>")
        End If

        If Trim(from_spot) = "company" Then
        Else
          If searchCriteria.ViewCriteriaCompanyID > 0 Then

            Dim sExtraCompanyData As String = ""
            Dim sCompanyName As String = commonEvo.get_company_name_fromID(searchCriteria.ViewCriteriaCompanyID, 0, True, True, sExtraCompanyData)

            Dim aCompanyInfo = Split(sExtraCompanyData, ":")
            Dim sTmpCity As String = ""
            Dim sTmpCountry As String = ""

            For x As Integer = 0 To UBound(aCompanyInfo)

              If x = 0 Then sTmpCity = aCompanyInfo(0).Trim
              If x = 1 Then sTmpCountry = aCompanyInfo(1).Trim

            Next

            If Not String.IsNullOrEmpty(sCompanyName) Then
              sTempCompanyHtml = "<br /><table width='100%' cellspacing='0' cellpadding='4' class='module'>"
              sTempCompanyHtml += "<tr><td align='left' valign='middle' class='header'>MANUFACTURER DETAILS" + sTmpTitle.Trim + "</td></tr>"
              sTempCompanyHtml += "<tr><td valign='middle' align='left' class='seperator'>Name : " + sCompanyName.Trim + "</td></tr>"
              sTempCompanyHtml += "<tr class='alt_row'><td valign='middle' align='left' class='seperator'>City : " + sTmpCity + "</td></tr>"
              sTempCompanyHtml += "<tr><td valign='middle' align='left' class='seperator'>Country : " + sTmpCountry + "</td></tr>"
              sTempCompanyHtml += "</table>"
            End If

          End If
        End If
        htmlOut.Append(strOut.ToString())

        If Not String.IsNullOrEmpty(sTempCompanyHtml) Then
          If searchCriteria.ViewCriteriaCompanyID > 0 And searchCriteria.ViewCriteriaAmodID > -1 Then
            htmlOut.Append(sTempCompanyHtml)
          End If
        End If

      Else
        htmlOut.Append(strOut.ToString())
      End If

    Catch ex As Exception

      aError = "Error in views_display_manufacturer_companies(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

    Finally

    End Try

    'return resulting html string
    out_htmlString = htmlOut.ToString
    htmlOut = Nothing
    strOut = Nothing
    results_table = Nothing

  End Sub

  Public Sub views_display_manufacturer_summary_block(ByVal nTotal, ByVal nInProduction, ByVal nWithMfr, ByVal nInOperation, ByVal nWrittenOff, ByVal nStored, ByVal nForsale, ByVal type_display, ByVal is_bottom)

    'Dim strBuilder, size_of
    'size_of = 185
    'strBuilder = New StringBuilder

    'strBuilder.Append("<table width='100%' cellspacing='0' cellpadding='1' class='module'><tr>")

    'If is_bottom = 0 Then
    '  strBuilder.Append("<tr><td align='left' colspan='8' valign='middle' class='header' style='padding-left:3px;'>DETAILS :</td></tr>")
    'Else
    '  strBuilder.Append("<tr><td align='left' colspan='8' valign='middle' class='header' style='padding-left:3px;'>Totals :</td></tr>")
    'End If

    'strBuilder.Append("<tr>")

    'If CLng(Session("manufacturerViewProductView")) = PRODUCT_CODE_COMMERCIAL Then
    '  size_of = 185
    'Else
    '  size_of = 247
    'End If

    'If is_bottom = 0 Then
    '  strBuilder.Append("<td width='205' class='border_bottom_right'>Totals")
    'Else
    '  strBuilder.Append("<td width='" & size_of & "' class='border_bottom_right'>&nbsp;")
    'End If

    'strBuilder.Append("</td>")

    'strBuilder.Append("<td valign='top' width='40' align='right' class='border_bottom_right'>&nbsp;" & CStr(nInProduction) & "&nbsp;</td>")
    'strBuilder.Append("<td valign='top' width='40' align='right' class='border_bottom_right'>&nbsp;" & CStr(nWithMfr) & "&nbsp;</td>")
    'strBuilder.Append("<td valign='top' width='35' align='right' class='border_bottom_right'>&nbsp;" & CStr(nInOperation) & "&nbsp;</td>")
    'strBuilder.Append("<td valign='top' width='40' align='right' class='border_bottom_right'>&nbsp;" & CStr(nWrittenOff) & "&nbsp;</td>")
    'strBuilder.Append("<td valign='top' width='40' align='right' class='border_bottom_right'>&nbsp;" & CStr(nStored) & "&nbsp;</td>")
    'strBuilder.Append("<td valign='top' width='40' align='right' class='border_bottom_right'>&nbsp;" & CStr(nForsale) & "&nbsp;</td>")
    'strBuilder.Append("<td valign='top' align='right' width='35' class='border_bottom_right'>&nbsp;" & CStr(nTotal) & "&nbsp;</td>")
    'strBuilder.Append("</tr><tr>")

    'strBuilder.Append("<td width='185' class='border_bottom_right'>&nbsp;</td>")
    'strBuilder.Append("<td valign='top' align='center'><strong>In<br />Prod</strong></td>")
    'strBuilder.Append("<td valign='top' align='center'><strong>With<br />Mfr</strong></td>")
    'strBuilder.Append("<td valign='top' align='center'><strong>In<br />Operation</strong></td>")
    'strBuilder.Append("<td valign='top' align='center'><strong>Written<br />Off</strong></td>")
    'strBuilder.Append("<td valign='top' align='center'><strong>In<br />Storage</strong></td>")
    'strBuilder.Append("<td valign='top' align='center'><strong>For<br />Sale</strong></td>")
    'strBuilder.Append("<td valign='top' width='35' align='right' bgcolor='#eeeeee' class='border_bottom_right'><strong>Total</strong></td>")
    'strBuilder.Append("</tr><tr>")

    'strBuilder.Append("<tr><td colspan='7' valign='top' align='center' class='seperator'>")
    'If type_display = "op" Then
    '  strBuilder.Append("<br><br>Click the Company Name above to view additional details regarding Manufacturer.")
    'Else
    '  strBuilder.Append("<br><br>Click the Model Name above to view additional details regarding the model.")
    'End If
    'strBuilder.Append("</td>")
    'strBuilder.Append("</tr><tr>")

    'strBuilder.Append("</table>")

    'displayManufacturerSummaryBlock = strBuilder.ToString()

    'strBuilder = Nothing

  End Sub

  Public Function get_manufacturer_aircraft_models_info(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal use_roll As String) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      If searchCriteria.ViewCriteriaAmodID > -1 Or searchCriteria.ViewCriteriaCompanyID > 0 Then
        sQuery.Append("SELECT DISTINCT amod_make_name, amod_model_name, amod_id, comp_id, ")
      Else
        sQuery.Append("SELECT DISTINCT top 100 amod_make_name, amod_model_name, amod_id, comp_id, ")
      End If

      sQuery.Append("sum(case when ac_lifecycle_stage = 1 then 1 else 0 end) as InProduction, ")
      sQuery.Append("sum(case when ac_lifecycle_stage = 2 then 1 else 0 end) as WithMfr, ")
      sQuery.Append("sum(case when ac_lifecycle_stage = 3 then 1 else 0 end) as InOperation, ")
      sQuery.Append("sum(case when ac_lifecycle_stage = 4 AND (ac_status NOT LIKE '%Stored%') then 1 else 0 end) as WrittenOff, ")
      sQuery.Append("sum(case when ac_lifecycle_stage = 4 AND (ac_status LIKE '%Stored%') then 1 else 0 end) as Stored, ")
      sQuery.Append("sum(case when ac_forsale_flag='Y' AND (ac_status LIKE 'For Sale%') then 1 else 0 end) as ForSale, ")
      sQuery.Append("COUNT(*) as TCOUNT ")

      'sQuery.Append(" (")
      'sQuery.Append("   SELECT COUNT(ac_id) FROM Aircraft WITH (NOLOCK)")
      'sQuery.Append("   INNER JOIN Aircraft_Model WITH (NOLOCK) ON ac_amod_id = amod_id")
      'sQuery.Append("   WHERE (ac_journ_id = 0)")
      'sQuery.Append("   AND (amod_id = am.amod_id)")
      'sQuery.Append("   AND (amod_manufacturer_comp_id = comp_id)")
      'sQuery.Append("   AND (amod_customer_flag = 'Y')")
      'sQuery.Append("   AND (ac_lifecycle_stage = 1)")

      'Select Case CInt(searchCriteria.ViewCriteriaAirframeType)
      '  Case Constants.VIEW_EXECUTIVE
      '    sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'E')")
      '  Case Constants.VIEW_JETS
      '    sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'J')")
      '  Case Constants.VIEW_TURBOPROPS
      '    sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'T')")
      '  Case Constants.VIEW_PISTONS
      '    sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'P')")
      '  Case Constants.VIEW_HELICOPTERS
      '    sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='R' AND amod_type_code in ('T','P'))")
      'End Select

      'If searchCriteria.ViewCriteriaAmodID > -1 Then
      '  sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
      'ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
      '  sQuery.Append(Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
      'End If

      'If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
      '  sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))
      'Else
      '  sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True))
      'End If

      'sQuery.Append(" ) As InProduction,")
      'sQuery.Append(" (")
      'sQuery.Append("   SELECT COUNT(ac_id) FROM Aircraft WITH (NOLOCK)")
      'sQuery.Append("   INNER JOIN Aircraft_Model WITH (NOLOCK) ON ac_amod_id = amod_id")
      'sQuery.Append("   WHERE (ac_journ_id = 0)")
      'sQuery.Append("   AND (amod_id = am.amod_id)")
      'sQuery.Append("   AND (amod_manufacturer_comp_id = comp_id)")
      'sQuery.Append("   AND (amod_customer_flag = 'Y')")
      'sQuery.Append("   AND (ac_lifecycle_stage = 2)")

      'Select Case CInt(searchCriteria.ViewCriteriaAirframeType)
      '  Case Constants.VIEW_EXECUTIVE
      '    sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'E')")
      '  Case Constants.VIEW_JETS
      '    sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'J')")
      '  Case Constants.VIEW_TURBOPROPS
      '    sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'T')")
      '  Case Constants.VIEW_PISTONS
      '    sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'P')")
      '  Case Constants.VIEW_HELICOPTERS
      '    sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='R' AND amod_type_code in ('T','P'))")
      'End Select

      'If searchCriteria.ViewCriteriaAmodID > -1 Then
      '  sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
      'ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
      '  sQuery.Append(Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
      'End If

      'If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
      '  sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))
      'Else
      '  sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True))
      'End If

      'sQuery.Append(" ) As WithMfr,")
      'sQuery.Append(" (")
      'sQuery.Append("   SELECT COUNT(ac_id) FROM Aircraft WITH (NOLOCK)")
      'sQuery.Append("   INNER JOIN Aircraft_Model WITH (NOLOCK) ON ac_amod_id = amod_id")
      'sQuery.Append("   WHERE (ac_journ_id = 0)")
      'sQuery.Append("   AND (amod_id = am.amod_id)")
      'sQuery.Append("   AND (amod_manufacturer_comp_id = comp_id)")
      'sQuery.Append("   AND (amod_customer_flag = 'Y')")
      'sQuery.Append("   AND (ac_lifecycle_stage = 3)")

      'Select Case CInt(searchCriteria.ViewCriteriaAirframeType)
      '  Case Constants.VIEW_EXECUTIVE
      '    sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'E')")
      '  Case Constants.VIEW_JETS
      '    sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'J')")
      '  Case Constants.VIEW_TURBOPROPS
      '    sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'T')")
      '  Case Constants.VIEW_PISTONS
      '    sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'P')")
      '  Case Constants.VIEW_HELICOPTERS
      '    sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='R' AND amod_type_code in ('T','P'))")
      'End Select

      'If searchCriteria.ViewCriteriaAmodID > -1 Then
      '  sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
      'ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
      '  sQuery.Append(Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
      'End If

      'If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
      '  sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))
      'Else
      '  sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True))
      'End If

      'sQuery.Append(" ) As InOperation,")
      'sQuery.Append(" (")
      'sQuery.Append("   SELECT COUNT(ac_id) FROM Aircraft WITH (NOLOCK)")
      'sQuery.Append("   INNER JOIN Aircraft_Model WITH (NOLOCK) ON ac_amod_id = amod_id")
      'sQuery.Append("   WHERE (ac_journ_id = 0)")
      'sQuery.Append("   AND (amod_id = am.amod_id)")
      'sQuery.Append("   AND (amod_manufacturer_comp_id = comp_id)")
      'sQuery.Append("   AND (amod_customer_flag = 'Y')")
      'sQuery.Append("   AND (ac_lifecycle_stage = 4)")
      'sQuery.Append("   AND (ac_status NOT LIKE '%Stored%')")

      'Select Case CInt(searchCriteria.ViewCriteriaAirframeType)
      '  Case Constants.VIEW_EXECUTIVE
      '    sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'E')")
      '  Case Constants.VIEW_JETS
      '    sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'J')")
      '  Case Constants.VIEW_TURBOPROPS
      '    sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'T')")
      '  Case Constants.VIEW_PISTONS
      '    sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'P')")
      '  Case Constants.VIEW_HELICOPTERS
      '    sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='R' AND amod_type_code in ('T','P'))")
      'End Select

      'If searchCriteria.ViewCriteriaAmodID > -1 Then
      '  sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
      'ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
      '  sQuery.Append(Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
      'End If

      'If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
      '  sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))
      'Else
      '  sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True))
      'End If

      'sQuery.Append(" ) As WrittenOff,")
      'sQuery.Append(" (")
      'sQuery.Append("   SELECT COUNT(ac_id) FROM Aircraft WITH (NOLOCK)")
      'sQuery.Append("   INNER JOIN Aircraft_Model WITH (NOLOCK) ON ac_amod_id = amod_id")
      'sQuery.Append("   WHERE (ac_journ_id = 0)")
      'sQuery.Append("   AND (amod_id = am.amod_id)")
      'sQuery.Append("   AND (amod_manufacturer_comp_id = comp_id)")
      'sQuery.Append("   AND (amod_customer_flag = 'Y')")
      'sQuery.Append("   AND (ac_lifecycle_stage = 4)")
      'sQuery.Append("   AND (ac_status LIKE '%Stored%')")

      'Select Case CInt(searchCriteria.ViewCriteriaAirframeType)
      '  Case Constants.VIEW_EXECUTIVE
      '    sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'E')")
      '  Case Constants.VIEW_JETS
      '    sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'J')")
      '  Case Constants.VIEW_TURBOPROPS
      '    sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'T')")
      '  Case Constants.VIEW_PISTONS
      '    sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'P')")
      '  Case Constants.VIEW_HELICOPTERS
      '    sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='R' AND amod_type_code in ('T','P'))")
      'End Select

      'If searchCriteria.ViewCriteriaAmodID > -1 Then
      '  sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
      'ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
      '  sQuery.Append(Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
      'End If

      'If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
      '  sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))
      'Else
      '  sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True))
      'End If

      'sQuery.Append(" ) As Stored,")
      'sQuery.Append(" (")
      'sQuery.Append("   SELECT COUNT(ac_id) FROM Aircraft WITH (NOLOCK)")
      'sQuery.Append("   INNER JOIN Aircraft_Model WITH (NOLOCK) ON ac_amod_id = amod_id")
      'sQuery.Append("   WHERE (ac_journ_id = 0)")
      'sQuery.Append("   AND (amod_id = am.amod_id)")
      'sQuery.Append("   AND (amod_manufacturer_comp_id = comp_id)")
      'sQuery.Append("   AND (amod_customer_flag = 'Y')")
      'sQuery.Append("   AND (ac_forsale_flag = 'Y')")
      'sQuery.Append("   AND (ac_status LIKE 'For Sale%')")

      'Select Case CInt(searchCriteria.ViewCriteriaAirframeType)
      '  Case Constants.VIEW_EXECUTIVE
      '    sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'E')")
      '  Case Constants.VIEW_JETS
      '    sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'J')")
      '  Case Constants.VIEW_TURBOPROPS
      '    sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'T')")
      '  Case Constants.VIEW_PISTONS
      '    sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'P')")
      '  Case Constants.VIEW_HELICOPTERS
      '    sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='R' AND amod_type_code in ('T','P'))")
      'End Select

      'If searchCriteria.ViewCriteriaAmodID > -1 Then
      '  sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
      'ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
      '  sQuery.Append(Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
      'End If

      'If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
      '  sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))
      'Else
      '  sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True))
      'End If

      'sQuery.Append(" ) As Forsale")

      sQuery.Append(" FROM Aircraft_Model AS am WITH (NOLOCK)")
      sQuery.Append(" inner join aircraft with (NOLOCK) on ac_journ_id = 0 and ac_amod_id = amod_id ")
      sQuery.Append(" INNER JOIN Company WITH (NOLOCK) ON comp_id = amod_manufacturer_comp_id AND comp_journ_id = 0")

      sQuery.Append(" WHERE comp_active_flag = 'Y'")

      If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
        sQuery.Append(" " + commonEvo.MakeCompanyProductCodeClause(HttpContext.Current.Session.Item("localPreferences"), False))
      Else
        sQuery.Append(" " + commonEvo.BuildCompanyProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False))
      End If

      Select Case CInt(searchCriteria.ViewCriteriaAirframeType)
        Case Constants.VIEW_EXECUTIVE
          sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'E')")
        Case Constants.VIEW_JETS
          sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'J')")
        Case Constants.VIEW_TURBOPROPS
          sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'T')")
        Case Constants.VIEW_PISTONS
          sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'P')")
        Case Constants.VIEW_HELICOPTERS
          sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='R' AND amod_type_code in ('T','P'))")
      End Select

      sQuery.Append(" AND (amod_manufacturer_comp_id > 0)")

      If searchCriteria.ViewCriteriaCompanyID > 0 Then
        If use_roll = "Y" Then
          sQuery.Append(" and comp_id in (select distinct RelCompID from ReturnAllCompanyLocationsByCompId(" & searchCriteria.ViewCriteriaCompanyID.ToString & "))")
        Else
          sQuery.Append(Constants.cAndClause + "comp_id = " + searchCriteria.ViewCriteriaCompanyID.ToString)
        End If
      End If

      If searchCriteria.ViewCriteriaAmodID > -1 Then
        sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
        sQuery.Append(Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
      End If

      If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
        sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, True))
      Else
        sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True))
      End If

      sQuery.Append(" GROUP BY amod_make_name, amod_model_name, amod_id, comp_id")
      sQuery.Append(" ORDER BY inOperation DESC, amod_make_name, amod_model_name, amod_id, comp_id")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_manufacturer_aircraft_models_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
        aError = "Error in get_manufacturer_aircraft_models_info load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_manufacturer_aircraft_models_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

  Public Sub views_display_manufacturer_aircraft_models(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, Optional ByVal bHasMaster As Boolean = False, Optional ByVal from_spot As String = "", Optional ByVal use_roll As String = "N")

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder
    Dim toggleRowColor As Boolean = False

    Dim sTmpTitle As String = ""
    Dim sTitle As String = ""
    Dim sTmpCompanyName As String = ""
    Dim sLinkString As String = ""
    Dim sReportString As String = ""

    Dim nTotal As Integer = 0

    Dim nInProduction As Integer = 0
    Dim nWithMfr As Integer = 0
    Dim nInOperation As Integer = 0
    Dim nWrittenOff As Integer = 0
    Dim nStored As Integer = 0
    Dim nForsale As Integer = 0
    Dim sColSpan As String = ""
    Dim bgcolor As String = ""
    Dim font_text_title As String = ""
    Dim font_text_start As String = ""
    Dim font_text_end As String = ""
    Dim temp_dir As String = "right"



    Dim bAerodexFlag As Boolean = HttpContext.Current.Session.Item("localPreferences").AerodexFlag.ToString.ToLower()

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


      If Not bAerodexFlag Then
        sColSpan = "'8'"
      Else
        sColSpan = "'7'"
      End If

      If use_roll = "Y" Then
        results_table = get_manufacturer_aircraft_models_info(searchCriteria, "Y")
      Else
        results_table = get_manufacturer_aircraft_models_info(searchCriteria, "N")
      End If 

      If searchCriteria.ViewCriteriaCompanyID > 0 Then
        sTmpCompanyName = commonEvo.get_company_name_fromID(searchCriteria.ViewCriteriaCompanyID, 0, False, True, "")
        sTmpTitle += " : " + sTmpCompanyName.Trim
      End If

      If searchCriteria.ViewCriteriaAmodID > -1 Then
        sTmpTitle += " - " + commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaAmodID, False, "")
      ElseIf searchCriteria.ViewCriteriaMakeAmodID > -1 Then
        sTmpTitle += " - " + commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaMakeAmodID, True, "")
      End If

      If String.IsNullOrEmpty(sTmpTitle) Then
        sTmpTitle = " : Top 100 MODELS"
      End If

      If Not IsNothing(results_table) Then

        sTitle = "MODEL SUMMARY" + sTmpTitle

        If Trim(from_spot) = "pdf" Then
          htmlOut.Append("<tr class='header_row'>")
        ElseIf Trim(from_spot) = "company" Then
          htmlOut.Append("<div valign=""top"" style='height:400px; overflow: auto;'>")
          htmlOut.Append("<table id='manufacturerModelInnerTable' cellspacing='0' cellpadding='2' border='0' width='100%' class='data_aircraft_grid'>")
          htmlOut.Append("<tr class='header_row'>")
        Else

          If Not searchCriteria.ViewCriteriaIsReport Then
            If results_table.Rows.Count > 15 Then
              htmlOut.Append("<div valign=""top"" style='height:400px; overflow: auto;'><p>")
            End If
          End If

                    If HttpContext.Current.Session.Item("localUser").crmDemoUserFlag = True Then
                    Else
                        If Not searchCriteria.ViewCriteriaIsReport And searchCriteria.ViewID > 2 And searchCriteria.ViewCriteriaCompanyID = 0 And searchCriteria.ViewCriteriaAmodID = -1 Then
                            sReportString = "<div align=""left"" style=""padding-left:8px;""><strong><a class=""White"" href=""View_Template.aspx?" + IIf(Not bHasMaster, "noMaster=false&", "") + "ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName + "&viewCompany=0&amod_id=-1&bIsReport=Y&bIsMfrModel=Y"">Export to Excel</a></strong></div>"
                        End If
                    End If

                    htmlOut.Append("<table id='manufacturerModelInnerTable' cellspacing='0' cellpadding='2' border='0' width='100%' class='module'>")
          htmlOut.Append("<tr><td align=""center"" valign=""middle"" class=""header"" colspan=" + sColSpan + ">" + sTitle + sReportString + "</td></tr>")

          If searchCriteria.ViewCriteriaCompanyID > 0 And searchCriteria.ViewCriteriaAmodID > -1 Then

            sLinkString = "<tr><td valign='top' align='left' class='seperator' colspan=" + sColSpan + "><br />Click <a href='view_template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName + "&viewCompany=0&amod_id=" + searchCriteria.ViewCriteriaAmodID.ToString + "'>Here</a> to Clear Company Data and Search for "
            sLinkString += searchCriteria.ViewCriteriaAircraftModel + " under " + sTmpTitle.Trim + " Models</td></tr>"

            If searchCriteria.ViewCriteriaAmodID > -1 Then
              sLinkString = "<tr><td valign='top' align='left' class='seperator' colspan=" + sColSpan + "><br />Click <a href='view_template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName + "&viewCompany=" + searchCriteria.ViewCriteriaCompanyID.ToString + "&amod_id=-1'>Here</a> to Clear Model Data and Search for "
              sLinkString += sTmpCompanyName.Trim + " under " + sTmpTitle.Trim + " Manufacturers</td></tr>"
            End If

          ElseIf searchCriteria.ViewCriteriaAmodID > -1 Then

            sLinkString = "<tr><td valign='top' align='left' class='seperator' colspan=" + sColSpan + "><br />Click <a href='view_template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName + "&viewCompany=" + searchCriteria.ViewCriteriaCompanyID.ToString + "&amod_id=-1'>Here</a> to Clear Model and Search for All Models"
            sLinkString += "</td></tr>"

          ElseIf searchCriteria.ViewCriteriaCompanyID > 0 Then

            sLinkString = "<tr><td valign='top' align='left' class='seperator' colspan=" + sColSpan + "><br />Click <a href='view_template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName + "&viewCompany=0&amod_id=-1'>Here</a> to Clear Company Data and Search for All Manufacturers"
            sLinkString += "</td></tr>"

          End If

          If Not searchCriteria.ViewCriteriaIsReport Then
            If Not String.IsNullOrEmpty(sLinkString.Trim) And searchCriteria.ViewID > 2 Then
              htmlOut.Append(sLinkString)
            End If
          End If

          htmlOut.Append("<tr>")

        End If

        htmlOut.Append("<td valign='top' align='left' width='15%' class='seperator'><strong>" & font_text_title & "Model&nbsp;Name" & font_text_end & "</td>")
        htmlOut.Append("<td valign='top' align='center' class='seperator'><strong>" & font_text_title & "In<br />Prod" & font_text_end & "</strong></td>")
        htmlOut.Append("<td valign='top' align='center' class='seperator'><strong>" & font_text_title & "With<br />Mfr" & font_text_end & "</strong></td>")
        htmlOut.Append("<td valign='top' align='center' class='seperator'><strong>" & font_text_title & "In<br />Operation" & font_text_end & "</strong></td>")
        htmlOut.Append("<td valign='top' align='center' class='seperator'><strong>" & font_text_title & "Written<br />Off" & font_text_end & "</strong></td>")
        htmlOut.Append("<td valign='top' align='center' class='seperator'><strong>" & font_text_title & "In<br />Storage" & font_text_end & "</strong></td>")

        If Not bAerodexFlag Then
          htmlOut.Append("<td valign='top' align='center' class='seperator'><strong>" & font_text_title & "For<br />Sale" & font_text_end & "</strong></td>")
        End If

        htmlOut.Append("<td valign='top' align='center' class='seperator'><strong>" & font_text_title & "&nbsp;<br />Total" & font_text_end & "</strong></td>")
        htmlOut.Append("</tr>")

        If results_table.Rows.Count > 0 Then

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


            nTotal = 0
            nInProduction = 0
            nWithMfr = 0
            nInOperation = 0
            nWrittenOff = 0
            nStored = 0
            nForsale = 0

            If Not IsDBNull(r.Item("InProduction")) Then
              If Not String.IsNullOrEmpty(r.Item("InProduction").ToString.Trim) Then
                nInProduction = CLng(r.Item("InProduction").ToString)
              End If
            End If

            If Not IsDBNull(r.Item("WithMfr")) Then
              If Not String.IsNullOrEmpty(r.Item("WithMfr").ToString.Trim) Then
                nWithMfr = CLng(r.Item("WithMfr").ToString)
              End If
            End If

            If Not IsDBNull(r.Item("InOperation")) Then
              If Not String.IsNullOrEmpty(r.Item("InOperation").ToString.Trim) Then
                nInOperation = CLng(r.Item("InOperation").ToString)
              End If
            End If

            If Not IsDBNull(r.Item("WrittenOff")) Then
              If Not String.IsNullOrEmpty(r.Item("WrittenOff").ToString.Trim) Then
                nWrittenOff = CLng(r.Item("WrittenOff").ToString)
              End If
            End If

            If Not IsDBNull(r.Item("Stored")) Then
              If Not String.IsNullOrEmpty(r.Item("Stored").ToString.Trim) Then
                nStored = CLng(r.Item("Stored").ToString)
              End If
            End If

            If Not bAerodexFlag Then
              If Not IsDBNull(r.Item("Forsale")) Then
                If Not String.IsNullOrEmpty(r.Item("Forsale").ToString.Trim) Then
                  nForsale = CLng(r.Item("Forsale").ToString)
                End If
              End If
            End If

            nTotal = nInProduction + nWithMfr + nInOperation + nWrittenOff + nStored

            If nTotal > 0 Then



              If Trim(from_spot) = "pdf" Then
                htmlOut.Append("<td valign='top' align='left' class='border_bottom_right' nowrap=""nowrap"">" & font_text_start & "")
                htmlOut.Append(r.Item("amod_make_name").ToString.Trim + " " + r.Item("amod_model_name").ToString.Trim + "" & font_text_end & "")
                htmlOut.Append("</td>")
              ElseIf Trim(from_spot) = "company" Then
                htmlOut.Append("<td valign='top' align='left' class='border_bottom_right' nowrap=""nowrap"">")
                htmlOut.Append("<a href='DisplayCompanyDetail.aspx?compid=" & searchCriteria.ViewCriteriaCompanyID.ToString & "&amod_id=" & r.Item("amod_id") & "&use_insight_manu=Y&use_insight_roll=" & use_roll & "&viewCompany=" + searchCriteria.ViewCriteriaCompanyID.ToString + "'>")
                htmlOut.Append(r.Item("amod_make_name").ToString.Trim + " " + r.Item("amod_model_name").ToString.Trim + "</a>")
                htmlOut.Append("</td>")
              ElseIf Not searchCriteria.ViewCriteriaIsReport Then
                htmlOut.Append("<td valign='top' align='left' class='border_bottom_right' nowrap=""nowrap"">")
                htmlOut.Append("<a href='view_template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName + "&viewCompany=" + searchCriteria.ViewCriteriaCompanyID.ToString + "&amod_id=" + r.Item("amod_id").ToString + "'>")
                htmlOut.Append(r.Item("amod_make_name").ToString.Trim + " " + r.Item("amod_model_name").ToString.Trim + "</a>")
                htmlOut.Append("</td>")
              Else
                htmlOut.Append("<td valign='top' align='left' class='border_bottom_right' nowrap=""nowrap"">")
                htmlOut.Append(r.Item("amod_make_name").ToString.Trim + " " + r.Item("amod_model_name").ToString.Trim)
                htmlOut.Append("</td>")
              End If

              If nInProduction > 0 Then
                htmlOut.Append("<td valign='top' align='right' class='border_bottom_right'>" & font_text_start & "" + FormatNumber(nInProduction, TriState.False, TriState.True, TriState.False, TriState.True).ToString + "&nbsp;" & font_text_end & "</td>")
              Else
                htmlOut.Append("<td valign='top' align='right' class='border_bottom_right'>" & font_text_start & "N/A" & font_text_end & "</td>")
              End If

              If nWithMfr > 0 Then
                htmlOut.Append("<td valign='top' align='right' class='border_bottom_right'>" & font_text_start & "" + FormatNumber(nWithMfr, TriState.False, TriState.True, TriState.False, TriState.True).ToString + "&nbsp;" & font_text_end & "</td>")
              Else
                htmlOut.Append("<td valign='top' align='right' class='border_bottom_right'>" & font_text_start & "N/A" & font_text_end & "</td>")
              End If

              If nInOperation > 0 Then
                htmlOut.Append("<td valign='top' align='right' class='border_bottom_right'>" & font_text_start & "" + FormatNumber(nInOperation, TriState.False, TriState.True, TriState.False, TriState.True).ToString + "&nbsp;" & font_text_end & "</td>")
              Else
                htmlOut.Append("<td valign='top' align='right' class='border_bottom_right'>" & font_text_start & "N/A" & font_text_end & "</td>")
              End If

              If nWrittenOff > 0 Then
                htmlOut.Append("<td valign='top' align='right' class='border_bottom_right'>" & font_text_start & "" + FormatNumber(nWrittenOff, TriState.False, TriState.True, TriState.False, TriState.True).ToString + "&nbsp;" & font_text_end & "</td>")
              Else
                htmlOut.Append("<td valign='top' align='right' class='border_bottom_right'>" & font_text_start & "N/A" & font_text_end & "</td>")
              End If

              If nStored > 0 Then
                htmlOut.Append("<td valign='top' align='right' class='border_bottom_right'>" & font_text_start & "" + FormatNumber(nStored, TriState.False, TriState.True, TriState.False, TriState.True).ToString + "&nbsp;" & font_text_end & "</td>")
              Else
                htmlOut.Append("<td valign='top' align='right' class='border_bottom_right'>" & font_text_start & "N/A" & font_text_end & "</td>")
              End If

              If Not bAerodexFlag Then
                If nForsale > 0 Then
                  htmlOut.Append("<td valign='top' align='right' class='border_bottom_right'>" & font_text_start & "" + FormatNumber(nForsale, TriState.False, TriState.True, TriState.False, TriState.True).ToString + "&nbsp;" & font_text_end & "</td>")
                Else
                  htmlOut.Append("<td valign='top' align='right' class='border_bottom_right'>" & font_text_start & "N/A" & font_text_end & "</td>")
                End If
              End If

              htmlOut.Append("<td valign='top' align='right' class='border_bottom_right'>" & font_text_start & "" + FormatNumber(nTotal, TriState.False, TriState.True, TriState.False, TriState.True).ToString + "&nbsp;" & font_text_end & "</td>")

              htmlOut.Append("</tr>")

            End If

          Next
        Else
          htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""border_bottom_right"" colspan=" + sColSpan + "><br/>No Data Available " + sTitle.Trim + "" & font_text_end & "</td></tr>")
        End If
      Else
        htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""border_bottom_right"" colspan=" + sColSpan + "><br/>No Data Available " + sTitle.Trim + "" & font_text_end & "</td></tr>")
      End If

      htmlOut.Append("</table>")

      If Trim(from_spot) = "company" Then
        htmlOut.Append("</div>")
      Else
        If Not searchCriteria.ViewCriteriaIsReport Then
          If results_table.Rows.Count > 15 Then
            htmlOut.Append("</p></div>")
          End If
        End If
      End If


    Catch ex As Exception

      aError = "Error in views_display_manufacturer_aircraft_models(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

    Finally

    End Try

    'return resulting html string
    out_htmlString = htmlOut.ToString
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Function get_manufacturer_aircraft_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT DISTINCT ac_id, ac_ser_no_full, ac_reg_no, amod_type_code, amod_make_name, amod_model_name, comp_name, comp_country, comp_id")
      sQuery.Append(" FROM Aircraft WITH(NOLOCK)")
      sQuery.Append(" INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id")
      sQuery.Append(" LEFT OUTER JOIN company ON comp_id = amod_manufacturer_comp_id AND comp_journ_id = 0")
      sQuery.Append(" WHERE ac_journ_id = 0")

      Select Case CInt(searchCriteria.ViewCriteriaAirframeType)
        Case Constants.VIEW_EXECUTIVE
          sQuery.Append(Constants.cAndClause + "(amod_customer_flag = 'Y' AND amod_airframe_type_code='F' AND amod_type_code = 'E')")
        Case Constants.VIEW_JETS
          sQuery.Append(Constants.cAndClause + "(amod_customer_flag = 'Y' AND amod_airframe_type_code='F' AND amod_type_code = 'J')")
        Case Constants.VIEW_TURBOPROPS
          sQuery.Append(Constants.cAndClause + "(amod_customer_flag = 'Y' AND amod_airframe_type_code='F' AND amod_type_code = 'T')")
        Case Constants.VIEW_PISTONS
          sQuery.Append(Constants.cAndClause + "(amod_customer_flag = 'Y' AND amod_airframe_type_code='F' AND amod_type_code = 'P')")
        Case Constants.VIEW_HELICOPTERS
          sQuery.Append(Constants.cAndClause + "(amod_customer_flag = 'Y' AND amod_airframe_type_code='R' AND amod_type_code in ('T','P'))")
      End Select

      If searchCriteria.ViewCriteriaCompanyID > 0 Then
        sQuery.Append(Constants.cAndClause + "comp_id = " + searchCriteria.ViewCriteriaCompanyID.ToString)
      End If

      If searchCriteria.ViewCriteriaAmodID > -1 Then
        sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
        sQuery.Append(Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
      End If

      If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
        sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))
      Else
        sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True))
      End If

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_manufacturer_aircraft_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
        aError = "Error in get_manufacturer_aircraft_info load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_manufacturer_aircraft_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

  Public Sub views_display_manufacturer_aircraft(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String)

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder
    Dim strOut As New StringBuilder
    Dim toggleRowColor As Boolean = False

    Dim sTmpTitle As String = ""
    Dim sTitle As String = ""
    Dim sTmpCompanyName As String = ""

    Try

      results_table = get_manufacturer_aircraft_info(searchCriteria)

      If searchCriteria.ViewCriteriaCompanyID > 0 Then
        sTmpCompanyName = commonEvo.get_company_name_fromID(searchCriteria.ViewCriteriaCompanyID, 0, False, True, "")
        sTmpTitle += " : with " + sTmpCompanyName.Trim
      End If

      If searchCriteria.ViewCriteriaAmodID > -1 Then
        sTmpTitle += " - " + commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaAmodID, False, "")
      ElseIf searchCriteria.ViewCriteriaMakeAmodID > -1 Then
        sTmpTitle += " - " + commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaMakeAmodID, True, "")
      End If

      If Not IsNothing(results_table) Then

        sTitle = "AIRCRAFT" + sTmpTitle + "&nbsp;<em>(" + results_table.Rows.Count.ToString + ")</em>"

        If results_table.Rows.Count > 15 Then
          If searchCriteria.ViewCriteriaAmodID = -1 And searchCriteria.ViewCriteriaCompanyID > 0 Then
            htmlOut.Append("<div valign=""top"" style='height:440px; overflow: auto;'><p>")
          Else
            htmlOut.Append("<div valign=""top"" style='height:386px; overflow: auto;'><p>")
          End If
        End If

        htmlOut.Append("<table id='manufacturerAircraftInnerTable' cellspacing='0' cellpadding='2' border='0' width='100%' class='module'>")
        htmlOut.Append("<tr><td align=""center"" valign=""middle"" class=""header"" colspan=""2"">" + sTitle + "</td></tr>")

        If results_table.Rows.Count > 0 Then

          For Each r As DataRow In results_table.Rows
            If Not toggleRowColor Then
              htmlOut.Append("<tr class='alt_row'>")
              toggleRowColor = True
            Else
              htmlOut.Append("<tr bgcolor='white'>")
              toggleRowColor = False
            End If

            htmlOut.Append("<td align='left' valign='top' class='seperator'><img src='images/ch_red.jpg' class='bullet' alt='acid : " + r.Item("ac_id").ToString + "' /></td>")
                        htmlOut.Append("<td align='left' valign='middle' class='seperator'> Serial# <a class='underline' onclick=""javascript:load('DisplayAircraftDetail.aspx?acid=" + r.Item("ac_id").ToString + "&jid=0','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');"" title='Display Aircraft Details'>")
                        htmlOut.Append(r.Item("ac_ser_no_full").ToString + "</a>")

            htmlOut.Append(", Reg# " + r.Item("ac_reg_no").ToString)
            htmlOut.Append(" " + r.Item("amod_make_name").ToString + " / " + r.Item("amod_model_name").ToString + ", ")

                        htmlOut.Append("<br /><a class='underline' onclick=""javascript:load('DisplayCompanyDetail.aspx?compid=" + r.Item("comp_id").ToString + "&journid=0','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');"" title='Display Company Details'>")
                        htmlOut.Append(Replace(r.Item("comp_name").ToString, Constants.cSingleSpace, Constants.cHTMLnbsp) + "</a> <em>(" + r.Item("comp_country").ToString.Trim + ")</em>")

            htmlOut.Append("</td></tr>")

          Next
        Else
          htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""border_bottom_right""><br/>No Data Available " + sTitle.Trim + "</td></tr>")
        End If
      Else
        htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""border_bottom_right""><br/>No Data Available " + sTitle.Trim + "</td></tr>")
      End If

      htmlOut.Append("</table>")

      If results_table.Rows.Count > 15 Then
        htmlOut.Append("</p></div>")
      End If

    Catch ex As Exception

      aError = "Error in views_display_manufacturer_aircraft(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

    Finally

    End Try

    'return resulting html string
    out_htmlString = htmlOut.ToString
    htmlOut = Nothing
    strOut = Nothing
    results_table = Nothing

  End Sub

  Public Function get_manufacturer_aircraft_bar_chart_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      If searchCriteria.ViewCriteriaAmodID > -1 Or searchCriteria.ViewCriteriaCompanyID > 0 Then
        sQuery.Append("SELECT DISTINCT amod_make_name, amod_model_name, amod_id, comp_id, ")
      Else
        sQuery.Append("SELECT DISTINCT top 100 amod_make_name, amod_model_name, amod_id, comp_id, ")
      End If

      sQuery.Append(" (")
      sQuery.Append("SELECT COUNT(ac_id) FROM Aircraft WITH (NOLOCK)")
      sQuery.Append(" INNER JOIN Aircraft_Model WITH (NOLOCK) ON ac_amod_id = amod_id")
      sQuery.Append(" WHERE (ac_journ_id = 0)")
      sQuery.Append(" AND (amod_id = am.amod_id)")
      sQuery.Append(" AND (amod_manufacturer_comp_id = comp_id)")
      sQuery.Append(" AND (amod_customer_flag = 'Y')")
      sQuery.Append(" AND (ac_lifecycle_stage = 1)")

      Select Case CInt(searchCriteria.ViewCriteriaAirframeType)
        Case Constants.VIEW_EXECUTIVE
          sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'E')")
        Case Constants.VIEW_JETS
          sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'J')")
        Case Constants.VIEW_TURBOPROPS
          sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'T')")
        Case Constants.VIEW_PISTONS
          sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'P')")
        Case Constants.VIEW_HELICOPTERS
          sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='R' AND amod_type_code in ('T','P'))")
      End Select

      If searchCriteria.ViewCriteriaAmodID > -1 Then
        sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
        sQuery.Append(Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
      End If

      If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
        sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))
      Else
        sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True))
      End If

      sQuery.Append(" ) As InProduction,")
      sQuery.Append(" (")
      sQuery.Append("SELECT COUNT(ac_id) FROM Aircraft WITH (NOLOCK)")
      sQuery.Append(" INNER JOIN Aircraft_Model WITH (NOLOCK) ON ac_amod_id = amod_id")
      sQuery.Append(" WHERE (ac_journ_id = 0)")
      sQuery.Append(" AND (amod_id = am.amod_id)")
      sQuery.Append(" AND (amod_manufacturer_comp_id = comp_id)")
      sQuery.Append(" AND (amod_customer_flag = 'Y')")
      sQuery.Append(" AND (ac_lifecycle_stage = 2)")

      Select Case CInt(searchCriteria.ViewCriteriaAirframeType)
        Case Constants.VIEW_EXECUTIVE
          sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'E')")
        Case Constants.VIEW_JETS
          sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'J')")
        Case Constants.VIEW_TURBOPROPS
          sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'T')")
        Case Constants.VIEW_PISTONS
          sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'P')")
        Case Constants.VIEW_HELICOPTERS
          sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='R' AND amod_type_code in ('T','P'))")
      End Select

      If searchCriteria.ViewCriteriaAmodID > -1 Then
        sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
        sQuery.Append(Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
      End If

      If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
        sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))
      Else
        sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True))
      End If

      sQuery.Append(" ) As WithMfr,")
      sQuery.Append(" (")
      sQuery.Append("SELECT COUNT(ac_id) FROM Aircraft WITH (NOLOCK)")
      sQuery.Append(" INNER JOIN Aircraft_Model WITH (NOLOCK) ON ac_amod_id = amod_id")
      sQuery.Append(" WHERE (ac_journ_id = 0)")
      sQuery.Append(" AND (amod_id = am.amod_id)")
      sQuery.Append(" AND (amod_manufacturer_comp_id = comp_id)")
      sQuery.Append(" AND (amod_customer_flag = 'Y')")
      sQuery.Append(" AND (ac_lifecycle_stage = 3)")

      Select Case CInt(searchCriteria.ViewCriteriaAirframeType)
        Case Constants.VIEW_EXECUTIVE
          sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'E')")
        Case Constants.VIEW_JETS
          sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'J')")
        Case Constants.VIEW_TURBOPROPS
          sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'T')")
        Case Constants.VIEW_PISTONS
          sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'P')")
        Case Constants.VIEW_HELICOPTERS
          sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='R' AND amod_type_code in ('T','P'))")
      End Select

      If searchCriteria.ViewCriteriaAmodID > -1 Then
        sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
        sQuery.Append(Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
      End If

      If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
        sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))
      Else
        sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True))
      End If

      sQuery.Append(" ) As InOperation,")
      sQuery.Append(" (")
      sQuery.Append("SELECT COUNT(ac_id) FROM Aircraft WITH (NOLOCK)")
      sQuery.Append(" INNER JOIN Aircraft_Model WITH (NOLOCK) ON ac_amod_id = amod_id")
      sQuery.Append(" WHERE (ac_journ_id = 0)")
      sQuery.Append(" AND (amod_id = am.amod_id)")
      sQuery.Append(" AND (amod_manufacturer_comp_id = comp_id)")
      sQuery.Append(" AND (amod_customer_flag = 'Y')")
      sQuery.Append(" AND (ac_lifecycle_stage = 4)")
      sQuery.Append(" AND (ac_status NOT LIKE '%Stored%')")

      Select Case CInt(searchCriteria.ViewCriteriaAirframeType)
        Case Constants.VIEW_EXECUTIVE
          sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'E')")
        Case Constants.VIEW_JETS
          sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'J')")
        Case Constants.VIEW_TURBOPROPS
          sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'T')")
        Case Constants.VIEW_PISTONS
          sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'P')")
        Case Constants.VIEW_HELICOPTERS
          sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='R' AND amod_type_code in ('T','P'))")
      End Select

      If searchCriteria.ViewCriteriaAmodID > -1 Then
        sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
        sQuery.Append(Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
      End If

      If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
        sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))
      Else
        sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True))
      End If

      sQuery.Append(" ) As WrittenOff,")
      sQuery.Append(" (")
      sQuery.Append("SELECT COUNT(ac_id) FROM Aircraft WITH (NOLOCK)")
      sQuery.Append(" INNER JOIN Aircraft_Model WITH (NOLOCK) ON ac_amod_id = amod_id")
      sQuery.Append(" WHERE (ac_journ_id = 0)")
      sQuery.Append(" AND (amod_id = am.amod_id)")
      sQuery.Append(" AND (amod_manufacturer_comp_id = comp_id)")
      sQuery.Append(" AND (amod_customer_flag = 'Y')")
      sQuery.Append(" AND (ac_lifecycle_stage = 4)")
      sQuery.Append(" AND (ac_status LIKE '%Stored%')")

      Select Case CInt(searchCriteria.ViewCriteriaAirframeType)
        Case Constants.VIEW_EXECUTIVE
          sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'E')")
        Case Constants.VIEW_JETS
          sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'J')")
        Case Constants.VIEW_TURBOPROPS
          sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'T')")
        Case Constants.VIEW_PISTONS
          sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'P')")
        Case Constants.VIEW_HELICOPTERS
          sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='R' AND amod_type_code in ('T','P'))")
      End Select

      If searchCriteria.ViewCriteriaAmodID > -1 Then
        sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
        sQuery.Append(Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
      End If

      If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
        sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))
      Else
        sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True))
      End If

      sQuery.Append(" ) As Stored,")
      sQuery.Append(" (")
      sQuery.Append("SELECT COUNT(ac_id) FROM Aircraft WITH (NOLOCK)")
      sQuery.Append(" INNER JOIN Aircraft_Model WITH (NOLOCK) ON ac_amod_id = amod_id")
      sQuery.Append(" WHERE (ac_journ_id = 0)")
      sQuery.Append(" AND (amod_id = am.amod_id)")
      sQuery.Append(" AND (amod_manufacturer_comp_id = comp_id)")
      sQuery.Append(" AND (amod_customer_flag = 'Y')")
      sQuery.Append(" AND (ac_forsale_flag = 'Y')")
      sQuery.Append(" AND (ac_status LIKE 'For Sale%')")

      Select Case CInt(searchCriteria.ViewCriteriaAirframeType)
        Case Constants.VIEW_EXECUTIVE
          sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'E')")
        Case Constants.VIEW_JETS
          sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'J')")
        Case Constants.VIEW_TURBOPROPS
          sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'T')")
        Case Constants.VIEW_PISTONS
          sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'P')")
        Case Constants.VIEW_HELICOPTERS
          sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='R' AND amod_type_code in ('T','P'))")
      End Select

      If searchCriteria.ViewCriteriaAmodID > -1 Then
        sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
        sQuery.Append(Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
      End If

      If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
        sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))
      Else
        sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True))
      End If

      sQuery.Append(" ) As Forsale")

      sQuery.Append(" FROM Aircraft_Model AS am WITH (NOLOCK)")
      sQuery.Append(" INNER JOIN Company WITH (NOLOCK) ON comp_id = amod_manufacturer_comp_id AND comp_journ_id = 0")

      sQuery.Append(" WHERE comp_active_flag = 'Y'")

      If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
        sQuery.Append(" " + commonEvo.MakeCompanyProductCodeClause(HttpContext.Current.Session.Item("localPreferences"), False))
      Else
        sQuery.Append(" " + commonEvo.BuildCompanyProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False))
      End If

      Select Case CInt(searchCriteria.ViewCriteriaAirframeType)
        Case Constants.VIEW_EXECUTIVE
          sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'E')")
        Case Constants.VIEW_JETS
          sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'J')")
        Case Constants.VIEW_TURBOPROPS
          sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'T')")
        Case Constants.VIEW_PISTONS
          sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='F' AND amod_type_code = 'P')")
        Case Constants.VIEW_HELICOPTERS
          sQuery.Append(Constants.cAndClause + "(amod_airframe_type_code='R' AND amod_type_code in ('T','P'))")
      End Select

      sQuery.Append(" AND (amod_manufacturer_comp_id > 0)")

      If searchCriteria.ViewCriteriaCompanyID > 0 Then
        sQuery.Append(Constants.cAndClause + "comp_id = " + searchCriteria.ViewCriteriaCompanyID.ToString)
      End If

      If searchCriteria.ViewCriteriaAmodID > -1 Then
        sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
        sQuery.Append(Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
      End If

      If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
        sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, True))
      Else
        sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True))
      End If

      sQuery.Append(" GROUP BY amod_make_name, amod_model_name, amod_id, comp_id")
      sQuery.Append(" ORDER BY amod_make_name, amod_model_name, amod_id, comp_id, inOperation DESC")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_manufacturer_aircraft_bar_chart_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
        aError = "Error in get_manufacturer_aircraft_bar_chart_info load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_manufacturer_aircraft_bar_chart_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

  Public Sub views_display_manufacturer_aircraft_bar_chart(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByVal graphID As Integer)

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder

    Dim x As Integer = 0

    Dim bAerodexFlag As Boolean = HttpContext.Current.Session.Item("localPreferences").AerodexFlag.ToString.ToLower()

    Try

      results_table = get_manufacturer_aircraft_bar_chart_info(searchCriteria)

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          htmlOut.Append(vbCrLf + "<script type='text/javascript'>" + vbCrLf)
          htmlOut.Append("google.load('visualization', '1', {'packages':['corechart']});" + vbCrLf)
          htmlOut.Append("google.setOnLoadCallback(drawVisualization" + graphID.ToString + ");" + vbCrLf)
          htmlOut.Append("function drawVisualization" + graphID.ToString + "() {" + vbCrLf)
          htmlOut.Append("var data = new google.visualization.DataTable();" + vbCrLf)
          htmlOut.Append("data.addColumn('string', 'Label');" + vbCrLf)
          htmlOut.Append("data.addColumn('number', 'Number of Aircraft');" + vbCrLf)

          If Not bAerodexFlag Then
            htmlOut.Append("data.addRows(" + CStr(results_table.Rows.Count * 6) + ");" + vbCrLf)
          Else
            htmlOut.Append("data.addRows(" + CStr(results_table.Rows.Count * 5) + ");" + vbCrLf)
          End If

          For Each r As DataRow In results_table.Rows

            If Not IsDBNull(r.Item("InProduction")) Then
              If Not String.IsNullOrEmpty(r.Item("InProduction").ToString.Trim) Then
                htmlOut.Append("data.setCell(" + x.ToString + ", 0, 'In Production');" + vbCrLf)
                htmlOut.Append("data.setCell(" + x.ToString + ", 1, " + r.Item("InProduction").ToString + ");" + vbCrLf)
                x += 1
              Else
                htmlOut.Append("data.setCell(" + x.ToString + ", 0, 'In Production');" + vbCrLf)
                htmlOut.Append("data.setCell(" + x.ToString + ", 1, " + ("0").ToString + ");" + vbCrLf)
                x += 1
              End If
            End If

            If Not IsDBNull(r.Item("WithMfr")) Then
              If Not String.IsNullOrEmpty(r.Item("WithMfr").ToString.Trim) Then
                htmlOut.Append("data.setCell(" + x.ToString + ", 0, 'With Manufacturer');" + vbCrLf)
                htmlOut.Append("data.setCell(" + x.ToString + ", 1, " + r.Item("WithMfr").ToString + ");" + vbCrLf)
                x += 1
              Else
                htmlOut.Append("data.setCell(" + x.ToString + ", 0, 'With Manufacturer');" + vbCrLf)
                htmlOut.Append("data.setCell(" + x.ToString + ", 1, " + ("0").ToString + ");" + vbCrLf)
                x += 1
              End If
            End If

            If Not IsDBNull(r.Item("InOperation")) Then
              If Not String.IsNullOrEmpty(r.Item("InOperation").ToString.Trim) Then
                htmlOut.Append("data.setCell(" + x.ToString + ", 0, 'In Operation');" + vbCrLf)
                htmlOut.Append("data.setCell(" + x.ToString + ", 1, " + r.Item("InOperation").ToString + ");" + vbCrLf)
                x += 1
              Else
                htmlOut.Append("data.setCell(" + x.ToString + ", 0, 'In Operation');" + vbCrLf)
                htmlOut.Append("data.setCell(" + x.ToString + ", 1, " + ("0").ToString + ");" + vbCrLf)
                x += 1
              End If
            End If

            If Not IsDBNull(r.Item("WrittenOff")) Then
              If Not String.IsNullOrEmpty(r.Item("WrittenOff").ToString.Trim) Then
                htmlOut.Append("data.setCell(" + x.ToString + ", 0, 'Written Off');" + vbCrLf)
                htmlOut.Append("data.setCell(" + x.ToString + ", 1, " + r.Item("WrittenOff").ToString + ");" + vbCrLf)
                x += 1
              Else
                htmlOut.Append("data.setCell(" + x.ToString + ", 0, 'Written Off');" + vbCrLf)
                htmlOut.Append("data.setCell(" + x.ToString + ", 1, " + ("0").ToString + ");" + vbCrLf)
                x += 1
              End If
            End If

            If Not IsDBNull(r.Item("Stored")) Then
              If Not String.IsNullOrEmpty(r.Item("Stored").ToString.Trim) Then
                htmlOut.Append("data.setCell(" + x.ToString + ", 0, 'Stored');" + vbCrLf)
                htmlOut.Append("data.setCell(" + x.ToString + ", 1, " + r.Item("Stored").ToString + ");" + vbCrLf)
                x += 1
              Else
                htmlOut.Append("data.setCell(" + x.ToString + ", 0, 'Stored');" + vbCrLf)
                htmlOut.Append("data.setCell(" + x.ToString + ", 1, " + ("0").ToString + ");" + vbCrLf)
                x += 1
              End If
            End If

            If Not bAerodexFlag Then

              If Not IsDBNull(r.Item("Forsale")) Then
                If Not String.IsNullOrEmpty(r.Item("Forsale").ToString.Trim) Then
                  htmlOut.Append("data.setCell(" + x.ToString + ", 0, 'For Sale');" + vbCrLf)
                  htmlOut.Append("data.setCell(" + x.ToString + ", 1, " + r.Item("Forsale").ToString + ");" + vbCrLf)
                  x += 1
                Else
                  htmlOut.Append("data.setCell(" + x.ToString + ", 0, 'For Sale');" + vbCrLf)
                  htmlOut.Append("data.setCell(" + x.ToString + ", 1, " + ("0").ToString + ");" + vbCrLf)
                  x += 1
                End If
              End If

            End If

          Next

          htmlOut.Append("var chart = new google.visualization.ColumnChart(document.getElementById('visualization" + graphID.ToString + "'));" + vbCrLf)
          htmlOut.Append("chart.draw(data, {chartArea:{width:'95%',height:'85%'}, title:'', slantedText:'true', slantedTextAngle:60, legend:'top', legendFontSize:12, tooltipFontSize:9});" + vbCrLf)

          htmlOut.Append("}" + vbCrLf)
          htmlOut.Append("</script>" + vbCrLf)

        End If

      End If

      If Not String.IsNullOrEmpty(htmlOut.ToString.Trim) Then
        htmlOut.Append("<table width='100%' height='400' cellpadding='2' cellspacing='0' class='module'>")
        htmlOut.Append("<tr><td valign='middle' align='center' class='header'>MODEL STATUS SUMMARY : " + commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaAmodID, False, "") + "</td></tr>")
        htmlOut.Append("<tr><td valign='top' align='left' class='border_bottom_right'><div id='visualization" + graphID.ToString + "' style='text-align:center; width:100%; height:400px;'></div></td></tr></table>")
      Else
        htmlOut.Append("<table width='100%' height='400' cellpadding='2' cellspacing='0' class='module'>")
        htmlOut.Append("<tr><td valign='middle' align='center' class='header'>MODEL STATUS SUMMARY : " + commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaAmodID, False, "") + "</td></tr>")
        htmlOut.Append("<tr><td valign='top' align='left' class='border_bottom_right'><div style='text-align:center; width:100%; height:400px;'>No Data to display</div></td></tr></table>")
      End If

    Catch ex As Exception

      aError = "Error in get_manufacturer_aircraft_bar_chart_info(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByVal graphID As Integer) " + ex.Message

    Finally

    End Try

    'return resulting html string
    out_htmlString = htmlOut.ToString
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Function get_manufacturer_model_pie_chart_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT DISTINCT amod_make_name, amod_model_name, amod_id, count(*) AS modelCount")
      sQuery.Append(" FROM Aircraft_Model WITH (NOLOCK)")
      sQuery.Append(" INNER JOIN Aircraft WITH (NOLOCK) ON ac_amod_id = amod_id AND ac_journ_id = 0")
      sQuery.Append(" INNER JOIN Company WITH (NOLOCK) ON comp_id = amod_manufacturer_comp_id AND comp_journ_id = 0")
      sQuery.Append(" WHERE comp_active_flag = 'Y'")

      If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
        sQuery.Append(" " + commonEvo.MakeCompanyProductCodeClause(HttpContext.Current.Session.Item("localPreferences"), False))
      Else
        sQuery.Append(" " + commonEvo.BuildCompanyProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False))
      End If

      Select Case CInt(searchCriteria.ViewCriteriaAirframeType)
        Case Constants.VIEW_EXECUTIVE
          sQuery.Append(Constants.cAndClause + "(amod_customer_flag = 'Y' AND amod_airframe_type_code='F' AND amod_type_code = 'E')")
        Case Constants.VIEW_JETS
          sQuery.Append(Constants.cAndClause + "(amod_customer_flag = 'Y' AND amod_airframe_type_code='F' AND amod_type_code = 'J')")
        Case Constants.VIEW_TURBOPROPS
          sQuery.Append(Constants.cAndClause + "(amod_customer_flag = 'Y' AND amod_airframe_type_code='F' AND amod_type_code = 'T')")
        Case Constants.VIEW_PISTONS
          sQuery.Append(Constants.cAndClause + "(amod_customer_flag = 'Y' AND amod_airframe_type_code='F' AND amod_type_code = 'P')")
        Case Constants.VIEW_HELICOPTERS
          sQuery.Append(Constants.cAndClause + "(amod_customer_flag = 'Y' AND amod_airframe_type_code='R' AND amod_type_code in ('T','P'))")
      End Select

      sQuery.Append(" AND (amod_manufacturer_comp_id > 0)")

      If searchCriteria.ViewCriteriaCompanyID > 0 Then
        sQuery.Append(Constants.cAndClause + "comp_id = " + searchCriteria.ViewCriteriaCompanyID.ToString)
      End If

      If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
        sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, True))
      Else
        sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True))
      End If

      sQuery.Append(" GROUP BY amod_make_name, amod_model_name, amod_id")
      sQuery.Append(" ORDER BY modelCount DESC, amod_make_name, amod_model_name, amod_id")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_manufacturer_model_pie_chart_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
        aError = "Error in get_manufacturer_model_pie_chart_info load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_manufacturer_model_pie_chart_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

  Public Sub views_display_manufacturer_model_pie_chart(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByVal graphID As Integer)

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder

    Dim x As Integer = 0

    Try

      results_table = get_manufacturer_model_pie_chart_info(searchCriteria)

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          htmlOut.Append(vbCrLf + "<script type='text/javascript'>" + vbCrLf)
          htmlOut.Append("google.load('visualization', '1', {'packages':['corechart']});" + vbCrLf)
          htmlOut.Append("google.setOnLoadCallback(drawVisualization" + graphID.ToString + ");" + vbCrLf)
          htmlOut.Append("function drawVisualization" + graphID.ToString + "() {" + vbCrLf)
          htmlOut.Append("var data = new google.visualization.DataTable();" + vbCrLf)
          htmlOut.Append("data.addColumn('string', 'Label');" + vbCrLf)
          htmlOut.Append("data.addColumn('number', 'Value');" + vbCrLf)
          htmlOut.Append("data.addRows(" + results_table.Rows.Count.ToString + ");" + vbCrLf)

          For Each r As DataRow In results_table.Rows

            If Not IsDBNull(r.Item("modelCount")) Then
              If Not String.IsNullOrEmpty(r.Item("modelCount").ToString.Trim) Then
                If CLng(r.Item("modelCount").ToString.Trim) > 0 Then
                  htmlOut.Append("data.setCell(" + x.ToString + ", 0, '" + r.Item("amod_make_name").ToString + " / " + r.Item("amod_model_name").ToString + "');" + vbCrLf)
                  htmlOut.Append("data.setCell(" + x.ToString + ", 1, " + r.Item("modelCount").ToString + ");" + vbCrLf)
                Else
                  htmlOut.Append("data.setCell(" + x.ToString + ", 0, '" + r.Item("amod_make_name").ToString + " / " + r.Item("amod_model_name").ToString + "');" + vbCrLf)
                  htmlOut.Append("data.setCell(" + x.ToString + ", 1, 0);" + vbCrLf)
                End If
                x += 1
              End If
            End If

          Next

          htmlOut.Append("var chart = new google.visualization.PieChart(document.getElementById('visualization" + graphID.ToString + "'));" + vbCrLf)

          If results_table.Rows.Count > 35 Then  ' 1/720 slice visibility threshold
            htmlOut.Append("chart.draw(data, {chartArea:{width:'95%',height:'85%'}, sliceVisibilityThreshold:'0', pieResidueSliceLabel:'Other', legend:'left', legendFontSize:12 });" + vbCrLf)
          Else
            htmlOut.Append("chart.draw(data, {chartArea:{width:'95%',height:'85%'}, legend:'left', legendFontSize:12 });" + vbCrLf)
          End If

          htmlOut.Append("}" + vbCrLf)
          htmlOut.Append("</script>" + vbCrLf)

        End If

      End If

      If Not String.IsNullOrEmpty(htmlOut.ToString.Trim) Then
        htmlOut.Append("<table width='100%' height='400' cellpadding='2' cellspacing='0' class='module'>")
        htmlOut.Append("<tr><td valign='middle' align='center' class='header'>MANUFACTURER MODEL SUMMARY</td></tr>")
        htmlOut.Append("<tr><td valign='top' align='left' class='border_bottom_right'><div id='visualization" + graphID.ToString + "' style='text-align:center; width:100%; height:400px;'></div></td></tr></table>")
      Else
        htmlOut.Append("<table width='100%' height='400' cellpadding='2' cellspacing='0' class='module'>")
        htmlOut.Append("<tr><td valign='middle' align='center' class='header'>MANUFACTURER MODEL SUMMARY</td></tr>")
        htmlOut.Append("<tr><td valign='top' align='left' class='border_bottom_right'><div style='text-align:center; width:100%; height:400px;'>No Data to display</div></td></tr></table>")
      End If

    Catch ex As Exception

      aError = "Error in views_display_manufacturer_model_pie_chart(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByVal graphID As Integer) " + ex.Message

    Finally

    End Try

    'return resulting html string
    out_htmlString = htmlOut.ToString
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Function write_manufacturerReport_string_to_file(ByVal sOutoutString As String, ByVal sReportname As String) As Boolean

    Try

      Dim f As System.IO.StreamWriter

      f = System.IO.File.CreateText(HttpContext.Current.Server.MapPath(HttpContext.Current.Session.Item("MarketSummaryFolderVirtualPath").ToString) + "\" + sReportname.Trim)

      ' write to the file
      f.WriteLine(sOutoutString)

      'close the streamwriter
      f.Close()
      f.Dispose()
      f = Nothing

    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in write_notesReport_string_to_file(ByVal sOutoutString As String, ByVal sReportname As String) As Boolean " + ex.Message
      Return False
    End Try

    Return True

  End Function

  Public Function include_excel_report_style() As String

    Dim htmlOut = New StringBuilder()

    htmlOut.Append("<style type='text/css'>")
    htmlOut.Append("  td.textformat {mso-number-format:'\@'}")
    htmlOut.Append("  td.textdate {mso-number-format:'Short Date'}")
    htmlOut.Append("</style>")

    Return htmlOut.ToString
    htmlOut = Nothing

  End Function

#End Region

End Class
