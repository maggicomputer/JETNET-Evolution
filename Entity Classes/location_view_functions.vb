' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/Entity Classes/location_view_functions.vb $
'$$Author: Mike $
'$$Date: 5/15/20 3:33p $
'$$Modtime: 5/15/20 3:28p $
'$$Revision: 4 $
'$$Workfile: location_view_functions.vb $
'
' ********************************************************************************

<System.Serializable()> Public Class location_view_functions

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

#Region "location_functions"

  Public Function get_state_code_search(ByVal inState As String) As String

    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sStateCode As String = ""

    Try

      sQuery.Append("SELECT state_code FROM State WITH(NOLOCK) WHERE state_name = '" + inState.Trim + "'")

      'HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_state_code_search(ByVal inState As String) As String</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlCommand.CommandText = sQuery.ToString

      SqlReader = SqlCommand.ExecuteReader()

      If SqlReader.HasRows Then

        SqlReader.Read()

        If Not IsDBNull(SqlReader("state_code")) Then
          If Not String.IsNullOrEmpty(SqlReader.Item("state_code").ToString.Trim) Then
            sStateCode = SqlReader.Item("state_code").ToString.Trim
          End If
        End If

      End If 'SqlReader.HasRows

      SqlReader.Close()

    Catch ex As Exception
      aError = "Error in get_state_code_search(ByVal inState As String) As String" + ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return sStateCode

  End Function

  Public Function get_location_map_center(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaState.Trim) And String.IsNullOrEmpty(searchCriteria.ViewCriteriaCity.Trim) Then
        sQuery.Append("SELECT Top 1 map_country, map_state, map_city, map_latitude, map_longitude")
        sQuery.Append(" FROM Mapping WITH(NOLOCK) INNER JOIN State WITH(NOLOCK) ON state_code = map_state AND state_country = map_country")
        sQuery.Append(" WHERE lower(map_country) = '" + Replace(searchCriteria.ViewCriteriaCountry.Trim, Constants.cSingleQuote, Constants.cDoubleSingleQuote).ToLower + "'")
        sQuery.Append(" AND lower(map_state) = '" + get_state_code_search(searchCriteria.ViewCriteriaState.Trim.ToLower) + "'")
        sQuery.Append(" AND map_latitude <> 0 AND map_latitude <> 0 AND map_city = ''")
        sQuery.Append(" ORDER BY map_country, map_state, map_city ASC")
      Else
        sQuery.Append("SELECT Top 1 map_country, map_city, map_latitude, map_longitude FROM Mapping WITH(NOLOCK)")
        sQuery.Append(" WHERE lower(map_country) = '" + Replace(searchCriteria.ViewCriteriaCountry.Trim, Constants.cSingleQuote, Constants.cDoubleSingleQuote).ToLower + "'")
        sQuery.Append(" AND map_latitude <> 0 AND map_latitude <> 0")

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaCity.Trim) Then
          sQuery.Append(" AND lower(map_city) = '" + Replace(searchCriteria.ViewCriteriaCity.Trim, Constants.cSingleQuote, Constants.cDoubleSingleQuote).ToLower + "'")
        Else
          sQuery.Append(" AND map_city = ''")
        End If

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaState.Trim) Then
          sQuery.Append(" AND lower(map_state) = '" + get_state_code_search(searchCriteria.ViewCriteriaState.Trim.ToLower) + "'")
        Else
          sQuery.Append(" AND map_state = ''")
        End If

        sQuery.Append(" GROUP BY map_country, map_city, map_latitude, map_longitude")
        sQuery.Append(" ORDER BY map_country, map_city ASC")
      End If

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_location_map_center(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
        aError = "Error in get_location_map_center load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_location_map_center(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

  Public Function get_location_continent(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      If searchCriteria.ViewCriteriaLocationViewType <> Constants.LOCATION_VIEW_BASE Then

        sQuery.Append("SELECT Case ISNULL(country_continent_name, '') WHEN '' THEN 'unknown' ELSE country_continent_name END country_continent_name, COUNT(distinct ac_id) AS tcount")

        sQuery.Append(" FROM Aircraft_Summary WITH(NOLOCK) ")
        sQuery.Append(" INNER JOIN Country WITH(NOLOCK) ON ac_aport_country = country_name")
        sQuery.Append(" WHERE ac_lifecycle_stage = 3")

        If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
          sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))
        Else
          sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True))
        End If

        If searchCriteria.ViewCriteriaLocationViewType = Constants.LOCATION_VIEW_OWNER Then
          sQuery.Append(" AND cref_contact_type in ('00','08','17')")
        Else
          sQuery.Append(" AND cref_operator_flag IN ('Y', 'O')")
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



        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaContinent.Trim) Then
          sQuery.Append(Constants.cAndClause + "lower(country_continent_name) = '" + searchCriteria.ViewCriteriaContinent.ToLower.Trim + "'")
        End If

        sQuery.Append(" GROUP BY country_continent_name")

        If searchCriteria.ViewCriteriaLocationViewSort = Constants.LOCATION_SORT_CONTINENT Then
          sQuery.Append(" ORDER BY country_continent_name ASC")
        Else
          sQuery.Append(" ORDER BY tcount DESC")
        End If

      Else

        sQuery.Append("SELECT Case ISNULL(country_continent_name, '') WHEN '' THEN 'unknown' ELSE country_continent_name END country_continent_name, COUNT(distinct ac_id) AS tcount")

        sQuery.Append(" FROM Aircraft_Summary WITH(NOLOCK)")
        sQuery.Append(" INNER JOIN Country WITH(NOLOCK) ON ac_aport_country = country_name")
        sQuery.Append(" WHERE ac_lifecycle_stage = 3")

        If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
          sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))
        Else
          sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True))
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

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaContinent.Trim) Then
          sQuery.Append(Constants.cAndClause + "lower(country_continent_name) = '" + searchCriteria.ViewCriteriaContinent.ToLower.Trim + "'")
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



        sQuery.Append(" GROUP BY country_continent_name")

        If searchCriteria.ViewCriteriaLocationViewSort = Constants.LOCATION_SORT_CONTINENT Then
          sQuery.Append(" ORDER BY country_continent_name ASC")
        Else
          sQuery.Append(" ORDER BY tcount DESC")
        End If

      End If

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_location_continent(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
        aError = "Error in get_location_continent load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_location_continent(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

  Public Sub views_display_location_continent(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByRef locationTable As DataTable, ByVal bHasMaster As Boolean)

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder
    Dim toggleRowColor As Boolean = False

    Dim sRefLink As String = ""
    Dim sRefTitle As String = ""

    Try

      locationTable = Nothing
      results_table = get_location_continent(searchCriteria)

      htmlOut.Append("<div class=""Box""><table id='displayContinentOuterTable' width='100%' cellspacing='0' cellpadding='0' class='" & IIf(bHasMaster = False, "formatTable blue", "module") & "'>")

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          locationTable = results_table

          Select Case (searchCriteria.ViewCriteriaLocationViewType)
            Case Constants.LOCATION_VIEW_BASE
              htmlOut.Append("<tr><td align='center' valign='middle' class='header'>CONTINENTS <em>(" + results_table.Rows.Count.ToString + ") by Aircraft Base</em></td></tr>")
            Case Constants.LOCATION_VIEW_OWNER
              htmlOut.Append("<tr><td align='center' valign='middle' class='header'>CONTINENTS <em>(" + results_table.Rows.Count.ToString + ") by Aircraft Owners</em></td></tr>")
            Case Constants.LOCATION_VIEW_OPERATOR
              htmlOut.Append("<tr><td align='center' valign='middle' class='header'>CONTINENTS <em>(" + results_table.Rows.Count.ToString + ") by Aircraft Operators</em></td></tr>")
          End Select

          htmlOut.Append("<tr><td valign='top' align='left'><table id='displayContinentInnerTable' width='100%' cellpadding='0' cellspacing='0'>")
          htmlOut.Append("<tr><td valign='top' align='left' class='seperator' width='80%' style='padding-left:5px;' valign='top'><strong>Continent</strong></td>")
          htmlOut.Append("<td valign='top' align='right' class='seperator' style='padding-right:5px;'><strong>Count</strong></td></tr>")

          htmlOut.Append("<tr><td class='rightside' colspan='2'>")

          If results_table.Rows.Count > 15 Then
            htmlOut.Append("<div valign=""top"" style=""height:370px; overflow: auto;""><p>")
          End If

          htmlOut.Append("<table id='displayContinentDataTable' width='100%' cellpadding='4' cellspacing='0'>")

          For Each r As DataRow In results_table.Rows

            If Not toggleRowColor Then
              htmlOut.Append("<tr class='alt_row'>")
              toggleRowColor = True
            Else
              htmlOut.Append("<tr bgcolor='white'>")
              toggleRowColor = False
            End If

            If r.Item("country_continent_name").ToString.Trim.ToLower.Contains("unknown") Then
              htmlOut.Append("<td align='left' valign='top' class='seperator' width='80%'>Unknown Continent</td>")
              htmlOut.Append("<td align='right' valign='top' class='seperator' style='padding-right:5px;'>" + FormatNumber(r.Item("tcount").ToString, 0) + "</td></tr>")
            Else

              htmlOut.Append("<td align='left' valign='top' class='seperator' width='80%'>")

              sRefLink = "view_template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName.Trim
              sRefLink += "&onLocationTab=" + IIf(searchCriteria.ViewID < 2, "true", "false") & IIf(bHasMaster = False, "&noMaster=false", "")
              sRefLink += "&viewCountry="
              sRefLink += "&viewCity="
              sRefLink += "&viewContinent=" + HttpContext.Current.Server.UrlEncode(r.Item("country_continent_name").ToString.Trim)
              sRefLink += "&viewState="
              sRefLink += "&amod_id=" + searchCriteria.ViewCriteriaAmodID.ToString

              sRefTitle = IIf(CBool(HttpContext.Current.Application.Item("DebugFlag").ToString), " title=""" + sRefLink.Trim + """", " title=""Click to view aircraft at location""")

              htmlOut.Append("<a href=""" + sRefLink + """" + sRefTitle + ">" + r.Item("country_continent_name").ToString.Trim + "</a></td>")

              htmlOut.Append("<td align='right' valign='top' class='seperator' style='padding-right:5px;'>" + FormatNumber(r.Item("tcount").ToString, 0) + "</td></tr>")
            End If

          Next

          htmlOut.Append("</table>")

          If results_table.Rows.Count > 15 Then
            htmlOut.Append("</p></div>")
          End If

          htmlOut.Append("</td></tr></table></td></tr>")

        Else
          htmlOut.Append("<tr><td align='left' colspan='2' valign='middle'>No aircraft continents for your search criteria.</td></tr>")
        End If
      Else
        htmlOut.Append("<tr><td align='left' colspan='2' valign='middle'>No aircraft continents for your search criteria.</td></tr>")
      End If

      htmlOut.Append("</table>")

    Catch ex As Exception

      aError = "Error in views_display_location_continent(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

    Finally

    End Try

    'return resulting html string
    out_htmlString = htmlOut.ToString
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Function get_location_country(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      If searchCriteria.ViewCriteriaLocationViewType <> Constants.LOCATION_VIEW_BASE Then

        If searchCriteria.ViewCriteriaCountryHasStates Then
          sQuery.Append("SELECT Case ISNULL(comp_country,'') WHEN '' THEN 'unknown' ELSE comp_country END AS comp_country, COUNT(distinct ac_id) AS tcount,")
          sQuery.Append(" (SELECT TOP 1 map_latitude FROM mapping WITH(NOLOCK) WHERE lower(comp_country) = lower(map_country) AND map_state IS NOT NULL AND map_latitude <> 0) AS tlatitude,")
          sQuery.Append(" (SELECT TOP 1 map_longitude FROM mapping WITH(NOLOCK) WHERE lower(comp_country) = lower(map_country) AND map_state IS NOT NULL AND map_longitude <> 0) AS tlongitude")
        Else
          sQuery.Append("SELECT Case ISNULL(comp_country,'') WHEN '' THEN 'unknown' ELSE comp_country END AS comp_country, COUNT(distinct ac_id) AS tcount,")
          sQuery.Append(" (SELECT TOP 1 map_latitude FROM mapping WITH(NOLOCK) WHERE lower(comp_country) = lower(map_country) AND map_city IS NOT NULL AND map_latitude <> 0) AS tlatitude,")
          sQuery.Append(" (SELECT TOP 1 map_longitude FROM mapping WITH(NOLOCK) WHERE lower(comp_country) = lower(map_country) AND map_city IS NOT NULL AND map_longitude <> 0) AS tlongitude")
        End If

        sQuery.Append(" FROM Aircraft_Summary WITH(NOLOCK) ")

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaContinent.Trim) Then
          sQuery.Append(" INNER JOIN Country WITH(NOLOCK) ON ac_aport_country = country_name")
        End If

        sQuery.Append(" WHERE ac_lifecycle_stage = 3")

        If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
          sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))
        Else
          sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True))
        End If

        If searchCriteria.ViewCriteriaLocationViewType = Constants.LOCATION_VIEW_OWNER Then
          sQuery.Append(" AND cref_contact_type in ('00','08','17')")
        Else
          sQuery.Append(" AND cref_operator_flag IN ('Y', 'O')")
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



        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaContinent.Trim) Then
          sQuery.Append(Constants.cAndClause + "lower(country_continent_name) = '" + searchCriteria.ViewCriteriaContinent.ToLower.Trim + "'")
        End If

        sQuery.Append(" GROUP BY comp_country")

        If searchCriteria.ViewCriteriaLocationViewSort = Constants.LOCATION_SORT_COUNTRY Then
          sQuery.Append(" ORDER BY comp_country ASC")
        Else
          sQuery.Append(" ORDER BY tcount DESC")
        End If

      Else

        If searchCriteria.ViewCriteriaCountryHasStates Then
          sQuery.Append("SELECT Case ISNULL(ac_aport_country,'') When '' then 'unknown' ELSE ac_aport_country END AS ac_aport_country, COUNT(distinct ac_id) AS tcount,")
          sQuery.Append(" (SELECT TOP 1 map_latitude FROM mapping WITH(NOLOCK) WHERE lower(ac_aport_country) = lower(map_country) AND map_state IS NOT NULL AND map_latitude <> 0) AS tlatitude,")
          sQuery.Append(" (SELECT TOP 1 map_longitude FROM mapping WITH(NOLOCK) WHERE lower(ac_aport_country) = lower(map_country) AND map_state IS NOT NULL AND map_longitude <> 0) AS tlongitude")
        Else
          sQuery.Append("SELECT Case ISNULL(ac_aport_country,'') When '' then 'unknown' ELSE ac_aport_country END AS ac_aport_country, COUNT(distinct ac_id) AS tcount,")
          sQuery.Append(" (SELECT TOP 1 map_latitude FROM mapping WITH(NOLOCK) WHERE lower(ac_aport_country) = lower(map_country) AND map_city IS NOT NULL AND map_latitude <> 0) AS tlatitude,")
          sQuery.Append(" (SELECT TOP 1 map_longitude FROM mapping WITH(NOLOCK) WHERE lower(ac_aport_country) = lower(map_country) AND map_city IS NOT NULL AND map_longitude <> 0) AS tlongitude")
        End If

        sQuery.Append(" FROM Aircraft_Summary WITH(NOLOCK)")

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaContinent.Trim) Then
          sQuery.Append(" INNER JOIN Country WITH(NOLOCK) ON ac_aport_country = country_name")
        End If

        sQuery.Append(" WHERE ac_lifecycle_stage = 3")

        If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
          sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))
        Else
          sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True))
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



        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaContinent.Trim) Then
          sQuery.Append(Constants.cAndClause + "lower(country_continent_name) = '" + searchCriteria.ViewCriteriaContinent.ToLower.Trim + "'")
        End If

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAirportIATA.Trim) Then
          sQuery.Append(Constants.cAndClause + "ac_aport_iata_code = '" + searchCriteria.ViewCriteriaAirportIATA.Trim + "'")
        End If

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAirportICAO.Trim) Then
          sQuery.Append(Constants.cAndClause + "ac_aport_icao_code = '" + searchCriteria.ViewCriteriaAirportICAO.Trim + "'")
        End If

        sQuery.Append(" GROUP BY ac_aport_country")

        If searchCriteria.ViewCriteriaLocationViewSort = Constants.LOCATION_SORT_COUNTRY Then
          sQuery.Append(" ORDER BY ac_aport_country ASC")
        Else
          sQuery.Append(" ORDER BY tcount DESC")
        End If

      End If

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_location_country(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
        aError = "Error in get_location_country load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_location_country(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

  Public Sub views_display_location_country(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByRef locationTable As DataTable, ByVal bHasMaster As Boolean)

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder
    Dim toggleRowColor As Boolean = False
    Dim total_count_unknown As Long = 0

    Dim sRefLink As String = ""
    Dim sRefTitle As String = ""

    Try

      locationTable = Nothing

      results_table = get_location_country(searchCriteria)

      htmlOut.Append("<div class=""Box""><table id='displayCountryOuterTable' width='100%' cellspacing='0' cellpadding='0' class='" & IIf(bHasMaster = False, "formatTable blue", "module") & "'>")

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          locationTable = results_table

          Select Case (searchCriteria.ViewCriteriaLocationViewType)
            Case Constants.LOCATION_VIEW_BASE
              htmlOut.Append("<tr><td align='center' valign='middle' class='header'>COUNTRIES <em>(" + results_table.Rows.Count.ToString + ") by Aircraft Base</em></td></tr>")
            Case Constants.LOCATION_VIEW_OWNER
              htmlOut.Append("<tr><td align='center' valign='middle' class='header'>COUNTRIES <em>(" + results_table.Rows.Count.ToString + ") by Aircraft Owners</em></td></tr>")
            Case Constants.LOCATION_VIEW_OPERATOR
              htmlOut.Append("<tr><td align='center' valign='middle' class='header'>COUNTRIES <em>(" + results_table.Rows.Count.ToString + ") by Aircraft Operators</em></td></tr>")
          End Select

          htmlOut.Append("<tr><td valign='top' align='left'><table id='displayCountryInnerTable' width='100%' cellpadding='0' cellspacing='0'>")
          htmlOut.Append("<tr><td valign='top' align='left' class='seperator' width='80%' style='padding-left:5px;' valign='top'><strong>Country</strong></td>")
          htmlOut.Append("<td valign='top' align='right' class='seperator' style='padding-right:5px;'><strong>Count</strong></td></tr>")

          htmlOut.Append("<tr><td class='rightside' colspan='2'>")

          If results_table.Rows.Count > 15 Then
            htmlOut.Append("<div valign=""top"" style=""height:370px; overflow: auto;""><p>")
          End If

          htmlOut.Append("<table id='displayCountryDataTable' width='100%' cellpadding='4' cellspacing='0'>")

          For Each r As DataRow In results_table.Rows

            If CLng(r.Item("tcount").ToString) > 0 Then

              If searchCriteria.ViewCriteriaLocationViewType <> Constants.LOCATION_VIEW_BASE Then
                If r.Item("comp_country").ToString.Trim.ToLower.Contains("unknown") Then
                  total_count_unknown += CLng(r.Item("tcount").ToString)
                Else
                  If Not toggleRowColor Then
                    htmlOut.Append("<tr class='alt_row'>")
                    toggleRowColor = True
                  Else
                    htmlOut.Append("<tr bgcolor='white'>")
                    toggleRowColor = False
                  End If

                  htmlOut.Append("<td align='left' valign='top' class='seperator' width='80%'>")

                  sRefLink = "view_template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName.Trim
                  sRefLink += "&onLocationTab=" + IIf(searchCriteria.ViewID < 2, "true", "false") & IIf(bHasMaster = False, "&noMaster=false", "")
                  sRefLink += "&viewCountry=" + HttpContext.Current.Server.UrlEncode(r.Item("comp_country").ToString.Trim)
                  sRefLink += "&viewCity="
                  sRefLink += "&viewContinent="
                  sRefLink += "&viewState="
                  sRefLink += "&amod_id=" + searchCriteria.ViewCriteriaAmodID.ToString

                  sRefTitle = IIf(CBool(HttpContext.Current.Application.Item("DebugFlag").ToString), " title=""" + sRefLink.Trim + """", " title=""Click to view aircraft at location""")

                  htmlOut.Append("<a href=""" + sRefLink + """" + sRefTitle + ">" + r.Item("comp_country").ToString.Trim + "</a></td>")
                  htmlOut.Append("<td align='right' valign='top' class='seperator' style='padding-right:5px;'>" + FormatNumber(r.Item("tcount").ToString, 0) + "</td></tr>")
                End If
              Else
                If r.Item("ac_aport_country").ToString.Trim.ToLower.Contains("unknown") Then
                  total_count_unknown += CLng(r.Item("tcount").ToString)
                Else
                  If Not toggleRowColor Then
                    htmlOut.Append("<tr class='alt_row'>")
                    toggleRowColor = True
                  Else
                    htmlOut.Append("<tr bgcolor='white'>")
                    toggleRowColor = False
                  End If

                  htmlOut.Append("<td align='left' valign='top' class='seperator' width='80%'>")

                  sRefLink = "view_template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName.Trim
                  sRefLink += "&onLocationTab=" + IIf(searchCriteria.ViewID < 2, "true", "false") & IIf(bHasMaster = False, "&noMaster=false", "")
                  sRefLink += "&viewCountry=" + HttpContext.Current.Server.UrlEncode(r.Item("ac_aport_country").ToString.Trim)
                  sRefLink += "&viewCity="
                  sRefLink += "&viewContinent="
                  sRefLink += "&viewState="
                  sRefLink += "&amod_id=" + searchCriteria.ViewCriteriaAmodID.ToString

                  sRefTitle = IIf(CBool(HttpContext.Current.Application.Item("DebugFlag").ToString), " title=""" + sRefLink.Trim + """", " title=""Click to view aircraft at location""")

                  htmlOut.Append("<a href=""" + sRefLink + """" + sRefTitle + ">" + r.Item("ac_aport_country").ToString.Trim + "</a></td>")

                  htmlOut.Append("<td align='right' valign='top' class='seperator' style='padding-right:5px;'>" + FormatNumber(r.Item("tcount").ToString, 0) + "</td></tr>")
                End If
              End If

            End If

          Next

          If total_count_unknown > 0 Then
            If Not toggleRowColor Then
              htmlOut.Append("<tr class='alt_row'>")
              toggleRowColor = True
            Else
              htmlOut.Append("<tr bgcolor='white'>")
              toggleRowColor = False
            End If
            htmlOut.Append("<td align='left' valign='top' class='seperator' width='80%'>Unknown Country</td>")
            htmlOut.Append("<td align='right' valign='top' class='seperator' style='padding-right:5px;'>" + total_count_unknown.ToString + "</td></tr>")
          End If

          htmlOut.Append("</table>")

          If results_table.Rows.Count > 15 Then
            htmlOut.Append("</p></div>")
          End If

          htmlOut.Append("</td></tr></table></td></tr>")

        Else
          htmlOut.Append("<tr><td align='left' colspan='2' valign='middle'>No aircraft countries for your search criteria.</td></tr>")
        End If
      Else
        htmlOut.Append("<tr><td align='left' colspan='2' valign='middle'>No aircraft countries for your search criteria.</td></tr>")
      End If

      htmlOut.Append("</table></div>")

    Catch ex As Exception

      aError = "Error in views_display_location_country(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByRef locationTable As DataTable) " + ex.Message

    Finally

    End Try

    'return resulting html string
    out_htmlString = htmlOut.ToString
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Function get_location_state_prov(ByRef searchCriteria As viewSelectionCriteriaClass, Optional ByVal from_spot As String = "") As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      If searchCriteria.ViewCriteriaLocationViewType <> Constants.LOCATION_VIEW_BASE Then

        If searchCriteria.ViewCriteriaCountryHasStates And searchCriteria.ViewCriteriaLocationViewSort = Constants.LOCATION_SORT_STATE Then
          sQuery.Append("SELECT Case ISNULL(comp_state,'') When '' then 'unknown' ELSE comp_state END AS comp_state, COUNT(distinct ac_id) AS tcount, state_name, state_code, comp_country, comp_city,")
          If searchCriteria.ViewCriteriaLocationViewSort = Constants.LOCATION_SORT_CITY Then
            sQuery.Append(" (SELECT TOP 1 map_latitude FROM mapping WITH(NOLOCK) WHERE lower(comp_country) = lower(map_country) AND lower(comp_state) = lower(map_state) AND lower(comp_city) = lower(map_city) AND map_latitude <> 0) AS tlatitude,")
            sQuery.Append(" (SELECT TOP 1 map_longitude FROM mapping WITH(NOLOCK) WHERE lower(comp_country) = lower(map_country) AND lower(comp_state) = lower(map_state) AND lower(comp_city) = lower(map_city) AND map_longitude <> 0) AS tlongitude")
          Else
            sQuery.Append(" (SELECT TOP 1 map_latitude FROM mapping WITH(NOLOCK) WHERE lower(comp_country) = lower(map_country) AND lower(comp_state) = lower(map_state) AND map_latitude <> 0) AS tlatitude,")
            sQuery.Append(" (SELECT TOP 1 map_longitude FROM mapping WITH(NOLOCK) WHERE lower(comp_country) = lower(map_country) AND lower(comp_state) = lower(map_state) AND map_longitude <> 0) AS tlongitude")
          End If
        Else
          sQuery.Append("SELECT Case ISNULL(comp_city,'') When '' then 'unknown' ELSE comp_city END AS comp_city,   COUNT(distinct ac_id) AS tcount, comp_country,")
          sQuery.Append(" (SELECT TOP 1 map_latitude FROM mapping WITH(NOLOCK) WHERE lower(comp_country) = lower(map_country) AND lower(comp_city) = lower(map_city) AND map_latitude <> 0) AS tlatitude,")
          sQuery.Append(" (SELECT TOP 1 map_longitude FROM mapping WITH(NOLOCK) WHERE lower(comp_country) = lower(map_country) AND lower(comp_city) = lower(map_city) AND map_longitude <> 0) AS tlongitude")
        End If

        sQuery.Append(" FROM Aircraft_Summary WITH(NOLOCK)")

        If searchCriteria.ViewCriteriaCountryHasStates Then
          sQuery.Append(" INNER JOIN State WITH(NOLOCK) ON state_code = comp_state AND state_country = comp_country")
        End If

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaContinent.Trim) Then
          sQuery.Append(" INNER JOIN Country WITH(NOLOCK) ON ac_aport_country = country_name")
        End If

        sQuery.Append(" WHERE ac_lifecycle_stage = 3")

        If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
          sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))
        Else
          sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True))
        End If

        If searchCriteria.ViewCriteriaLocationViewType = Constants.LOCATION_VIEW_OWNER Then
          sQuery.Append(" AND cref_contact_type in ('00','08','17')")
        Else
          sQuery.Append(" AND cref_operator_flag IN ('Y', 'O')")
        End If

        If searchCriteria.ViewCriteriaAmodID > -1 Then
          sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
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
          sQuery.Append(Constants.cAndClause + " ac_mfr_year >= " & searchCriteria.ViewCriteriaYearStart)
        End If


        If searchCriteria.ViewCriteriaYearEnd > 0 Then
          sQuery.Append(Constants.cAndClause + " ac_mfr_year <=  " & searchCriteria.ViewCriteriaYearEnd)
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

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaContinent.Trim) Then
          sQuery.Append(Constants.cAndClause + "lower(country_continent_name) = '" + searchCriteria.ViewCriteriaContinent.ToLower.Trim + "'")
        End If

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaCountry.Trim) Then
          sQuery.Append(Constants.cAndClause + "lower(comp_country) = '" + Replace(searchCriteria.ViewCriteriaCountry.Trim, Constants.cSingleQuote, Constants.cDoubleSingleQuote).ToLower + "'")
        End If

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaState.Trim) Then
          sQuery.Append(Constants.cAndClause + "lower(state_name) = '" + Replace(searchCriteria.ViewCriteriaState.Trim, Constants.cSingleQuote, Constants.cDoubleSingleQuote).ToLower + "'")
        End If

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaCity.Trim) Then
          sQuery.Append(Constants.cAndClause + "lower(comp_city) = '" + Replace(searchCriteria.ViewCriteriaCity.Trim, Constants.cSingleQuote, Constants.cDoubleSingleQuote).ToLower + "'")
        End If

        ' If searchCriteria.ViewCriteriaCountryHasStates Or searchCriteria.ViewCriteriaLocationViewSort = Constants.LOCATION_SORT_STATE Then
        ' sQuery.Append(Constants.cAndClause + "comp_state IS NOT NULL AND comp_state <> ''")
        ' If searchCriteria.ViewCriteriaLocationViewSort <> Constants.LOCATION_SORT_CITY Then
        ' sQuery.Append(Constants.cAndClause + "ac_aport_city IS NULL")
        '  End If
        '  Else
        ' sQuery.Append(Constants.cAndClause + "comp_city IS NOT NULL AND comp_city <> ''")
        ' sQuery.Append(Constants.cAndClause + "comp_state IS NULL")
        ' End If


        If searchCriteria.ViewCriteriaCountryHasStates Or searchCriteria.ViewCriteriaLocationViewSort = Constants.LOCATION_SORT_STATE Then
          sQuery.Append(" GROUP BY comp_country, comp_state, state_name, state_code, comp_city")
          sQuery.Append(" ORDER BY comp_country, comp_state, state_name ASC, comp_city ASC")
        Else
          sQuery.Append(" GROUP BY comp_country, comp_city")
          sQuery.Append(" ORDER BY tcount DESC, comp_country ASC, comp_city ASC")
        End If

      Else

        If searchCriteria.ViewCriteriaCountryHasStates Or searchCriteria.ViewCriteriaLocationViewSort = Constants.LOCATION_SORT_STATE Then
          sQuery.Append("SELECT CASE ISNULL(ac_aport_state,'') WHEN '' THEN 'unknown' ELSE ac_aport_state END AS ac_aport_state, COUNT(distinct ac_id) AS tcount, state_name, state_code, ac_aport_country,")

          If Trim(from_spot) = "City" Then
            sQuery.Append(" ac_aport_city, ")
          End If

          If searchCriteria.ViewCriteriaLocationViewSort = Constants.LOCATION_SORT_CITY Then
            sQuery.Append(" (SELECT TOP 1 map_latitude FROM mapping WITH(NOLOCK) WHERE lower(ac_aport_country) = lower(map_country) AND lower(ac_aport_state) = lower(map_state) AND lower(ac_aport_city) = lower(map_city) AND map_latitude <> 0) AS tlatitude,")
            sQuery.Append(" (SELECT TOP 1 map_longitude FROM mapping WITH(NOLOCK) WHERE lower(ac_aport_country) = lower(map_country) AND lower(ac_aport_state) = lower(map_state) AND lower(ac_aport_city) = lower(map_city) AND map_longitude <> 0) AS tlongitude")
          Else
            sQuery.Append(" (SELECT TOP 1 map_latitude FROM mapping WITH(NOLOCK) WHERE lower(ac_aport_country) = lower(map_country) AND lower(ac_aport_state) = lower(map_state) AND map_latitude <> 0) AS tlatitude,")
            sQuery.Append(" (SELECT TOP 1 map_longitude FROM mapping WITH(NOLOCK) WHERE lower(ac_aport_country) = lower(map_country) AND lower(ac_aport_state) = lower(map_state) AND map_longitude <> 0) AS tlongitude")
          End If
        Else
          sQuery.Append("SELECT CASE ISNULL(ac_aport_city,'') WHEN '' THEN 'unknown' ELSE ac_aport_city END AS ac_aport_city, '' as state_name, COUNT(distinct ac_id) AS tcount, ac_aport_country,")
          sQuery.Append(" (SELECT TOP 1 map_latitude FROM mapping WITH(NOLOCK) WHERE lower(ac_aport_country) = lower(map_country) AND lower(ac_aport_city) = lower(map_city) AND map_latitude <> 0) AS tlatitude,")
          sQuery.Append(" (SELECT TOP 1 map_longitude FROM mapping WITH(NOLOCK) WHERE lower(ac_aport_country) = lower(map_country) AND lower(ac_aport_city) = lower(map_city) AND map_longitude <> 0) AS tlongitude")
        End If

        sQuery.Append(" FROM Aircraft_Summary WITH(NOLOCK)")

        If searchCriteria.ViewCriteriaCountryHasStates Then
          sQuery.Append(" INNER JOIN State WITH(NOLOCK) ON state_code = ac_aport_state AND state_country = ac_aport_country")
        End If

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaContinent.Trim) Then
          sQuery.Append(" INNER JOIN Country WITH(NOLOCK) ON ac_aport_country = country_name")
        End If

        sQuery.Append(" WHERE ac_lifecycle_stage = 3")

        If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
          sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))
        Else
          sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True))
        End If

        If searchCriteria.ViewCriteriaAmodID > -1 Then
          sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
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
          sQuery.Append(Constants.cAndClause + " ac_mfr_year >= " & searchCriteria.ViewCriteriaYearStart)
        End If


        If searchCriteria.ViewCriteriaYearEnd > 0 Then
          sQuery.Append(Constants.cAndClause + " ac_mfr_year <=  " & searchCriteria.ViewCriteriaYearEnd)
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

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaContinent.Trim) Then
          sQuery.Append(Constants.cAndClause + "lower(country_continent_name) = '" + searchCriteria.ViewCriteriaContinent.ToLower.Trim + "'")
        End If

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaCountry.Trim) Then
          sQuery.Append(Constants.cAndClause + "lower(ac_aport_country) = '" + Replace(searchCriteria.ViewCriteriaCountry.Trim, Constants.cSingleQuote, Constants.cDoubleSingleQuote).ToLower + "'")
        End If

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaState.Trim) Then
          sQuery.Append(Constants.cAndClause + "lower(state_name) = '" + Replace(searchCriteria.ViewCriteriaState.Trim, Constants.cSingleQuote, Constants.cDoubleSingleQuote).ToLower + "'")
        End If

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaCity.Trim) Then
          sQuery.Append(Constants.cAndClause + "lower(ac_aport_city) = '" + Replace(searchCriteria.ViewCriteriaCity.Trim, Constants.cSingleQuote, Constants.cDoubleSingleQuote).ToLower + "'")
        End If

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAirportIATA.Trim) Then
          sQuery.Append(Constants.cAndClause + "ac_aport_iata_code = '" + searchCriteria.ViewCriteriaAirportIATA.Trim + "'")
        End If


        If Trim(from_spot) = "City" Then
          If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAirportICAO.Trim) Then
            sQuery.Append(Constants.cAndClause + "ac_aport_icao_code = '" + searchCriteria.ViewCriteriaAirportICAO.Trim + "'")
          End If

          'commented out MSW - to show the unknowns - 3/31/19
          'If searchCriteria.ViewCriteriaCountryHasStates Or searchCriteria.ViewCriteriaLocationViewSort = Constants.LOCATION_SORT_STATE Then
          '  sQuery.Append(Constants.cAndClause + "ac_aport_state IS NOT NULL AND ac_aport_state <> ''")
          '  ' If searchCriteria.ViewCriteriaLocationViewSort <> Constants.LOCATION_SORT_CITY Then
          '  ' sQuery.Append(Constants.cAndClause + "ac_aport_city IS NULL")
          '  'End If
          'Else
          '  sQuery.Append(Constants.cAndClause + "ac_aport_city IS NOT NULL AND ac_aport_city <> ''")
          '  ' sQuery.Append(Constants.cAndClause + "ac_aport_state IS NULL")
          'End If
        Else
          If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAirportICAO.Trim) Then
            sQuery.Append(Constants.cAndClause + "ac_aport_icao_code = '" + searchCriteria.ViewCriteriaAirportICAO.Trim + "'")
          End If

          If searchCriteria.ViewCriteriaCountryHasStates Or searchCriteria.ViewCriteriaLocationViewSort = Constants.LOCATION_SORT_STATE Then
            sQuery.Append(Constants.cAndClause + "ac_aport_state IS NOT NULL AND ac_aport_state <> ''")
            ' If searchCriteria.ViewCriteriaLocationViewSort <> Constants.LOCATION_SORT_CITY Then
            ' sQuery.Append(Constants.cAndClause + "ac_aport_city IS NULL")
            'End If
          Else
            sQuery.Append(Constants.cAndClause + "ac_aport_city IS NOT NULL AND ac_aport_city <> ''")
            ' sQuery.Append(Constants.cAndClause + "ac_aport_state IS NULL")
          End If
        End If


        If searchCriteria.ViewCriteriaCountryHasStates Or searchCriteria.ViewCriteriaLocationViewSort = Constants.LOCATION_SORT_STATE Then

          If Trim(from_spot) = "City" Then
            sQuery.Append(" GROUP BY ac_aport_country, ac_aport_state, state_name, state_code, ac_aport_city")
            sQuery.Append(" ORDER BY ac_aport_country, ac_aport_state, state_name ASC, ac_aport_city ASC")
          Else
            sQuery.Append(" GROUP BY ac_aport_country, ac_aport_state, state_name, state_code ")
            sQuery.Append(" ORDER BY ac_aport_country, ac_aport_state, state_name ASC ")
          End If

        Else

          If Trim(from_spot) = "City" Then
            sQuery.Append(" GROUP BY ac_aport_country, ac_aport_city")
            sQuery.Append(" ORDER BY tcount DESC, ac_aport_country ASC, ac_aport_city ASC")
          Else
            sQuery.Append(" GROUP BY ac_aport_country ")
            sQuery.Append(" ORDER BY tcount DESC, ac_aport_country ASC ")
          End If

        End If

      End If

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_location_state_prov(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
        aError = "Error in get_location_state_prov load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_location_state_prov(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

  Public Sub views_display_location_state_prov(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByRef locationTable As DataTable, Optional ByVal from_spot As String = "", Optional ByVal bHasMaster As Boolean = True)

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder
    Dim toggleRowColor As Boolean = False
    Dim total_count_unknown As Long = 0

    Dim sRefLink As String = ""
    Dim sRefTitle As String = ""

    Try

      locationTable = Nothing


      results_table = get_location_state_prov(searchCriteria, from_spot)

      htmlOut.Append("<div class=""Box""><table id='displayStateOuterTable' width='100%' cellspacing='0' cellpadding='0' class='" & IIf(bHasMaster = False, "formatTable blue", "module") & "'>")

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          locationTable = results_table

          Select Case (searchCriteria.ViewCriteriaLocationViewType)
            Case Constants.LOCATION_VIEW_BASE
              htmlOut.Append("<tr><td align='center' valign='middle' class='header'>STATES/PROVINCES <em>(" + results_table.Rows.Count.ToString + ") by Aircraft Base</em></td></tr>")
            Case Constants.LOCATION_VIEW_OWNER
              htmlOut.Append("<tr><td align='center' valign='middle' class='header'>STATES/PROVINCES <em>(" + results_table.Rows.Count.ToString + ") by Aircraft Owners</em></td></tr>")
            Case Constants.LOCATION_VIEW_OPERATOR
              htmlOut.Append("<tr><td align='center' valign='middle' class='header'>STATES/PROVINCES <em>(" + results_table.Rows.Count.ToString + ") by Aircraft Operators</em></td></tr>")
          End Select

          htmlOut.Append("<tr><td valign='top' align='left'><table id='displayStateInnerTable' width='100%' cellpadding='0' cellspacing='0'>")
          htmlOut.Append("<tr><td valign='top' align='left' class='seperator' width='80%' style='padding-left:5px;' valign='top'><strong>State/Province</strong></td>")
          htmlOut.Append("<td valign='top' align='right' class='seperator' style='padding-right:5px;'><strong>Count</strong></td></tr>")

          htmlOut.Append("<tr><td class='rightside' colspan='2'>")

          If results_table.Rows.Count > 15 Then
            htmlOut.Append("<div valign=""top"" style=""height:520px; overflow: auto;""><p>")
          End If

          htmlOut.Append("<table id='displayStateDataTable' width='100%' cellpadding='4' cellspacing='0'>")

          For Each r As DataRow In results_table.Rows

            If CLng(r.Item("tcount").ToString) Then

              If searchCriteria.ViewCriteriaLocationViewType <> Constants.LOCATION_VIEW_BASE Then

                If r.Item("comp_state").ToString.Trim.ToLower.Contains("unknown") Then
                  total_count_unknown += CLng(r.Item("tcount").ToString)
                Else
                  If Not toggleRowColor Then
                    htmlOut.Append("<tr class='alt_row'>")
                    toggleRowColor = True
                  Else
                    htmlOut.Append("<tr bgcolor='white'>")
                    toggleRowColor = False
                  End If

                  htmlOut.Append("<td align='left' valign='top' class='seperator' width='80%'>")

                  sRefLink = "view_template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName.Trim
                  sRefLink += "&onLocationTab=" + IIf(searchCriteria.ViewID < 2, "true", "false") & IIf(bHasMaster = False, "&noMaster=false", "")
                  sRefLink += "&viewCountry=" + HttpContext.Current.Server.UrlEncode(searchCriteria.ViewCriteriaCountry.Trim)
                  sRefLink += "&viewCity=" + HttpContext.Current.Server.UrlEncode(searchCriteria.ViewCriteriaCity.Trim)
                  sRefLink += "&viewContinent=" + HttpContext.Current.Server.UrlEncode(r.Item("country_continent_name").ToString.Trim)
                  sRefLink += "&viewState=" + HttpContext.Current.Server.UrlEncode(r.Item("state_name").ToString.Trim)
                  sRefLink += "&amod_id=" + searchCriteria.ViewCriteriaAmodID.ToString

                  sRefTitle = IIf(CBool(HttpContext.Current.Application.Item("DebugFlag").ToString), " title=""" + sRefLink.Trim + """", " title=""Click to view aircraft at location""")

                  htmlOut.Append("<a href=""" + sRefLink + """" + sRefTitle + ">" + r.Item("state_name").ToString.Trim + "</a></td>")

                  htmlOut.Append("<td align='right' valign='top' class='seperator' style='padding-right:5px;'>" + r.Item("tcount").ToString + "</td></tr>")

                End If

              Else

                If r.Item("ac_aport_state").ToString.Trim.ToLower.Contains("unknown") Then
                  total_count_unknown += CLng(r.Item("tcount").ToString)
                Else
                  If Not toggleRowColor Then
                    htmlOut.Append("<tr class='alt_row'>")
                    toggleRowColor = True
                  Else
                    htmlOut.Append("<tr bgcolor='white'>")
                    toggleRowColor = False
                  End If

                  htmlOut.Append("<td align='left' valign='top' class='seperator' width='80%'>")

                  sRefLink = "view_template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName.Trim
                  sRefLink += "&onLocationTab=" + IIf(searchCriteria.ViewID < 2, "true", "false") & IIf(bHasMaster = False, "&noMaster=false", "")
                  sRefLink += "&viewCountry=" + HttpContext.Current.Server.UrlEncode(searchCriteria.ViewCriteriaCountry.Trim)
                  sRefLink += "&viewCity=" + HttpContext.Current.Server.UrlEncode(searchCriteria.ViewCriteriaCity.Trim)
                  sRefLink += "&viewContinent=" + HttpContext.Current.Server.UrlEncode(searchCriteria.ViewCriteriaContinent.Trim)
                  sRefLink += "&viewState=" + HttpContext.Current.Server.UrlEncode(r.Item("state_name").ToString.Trim)
                  sRefLink += "&amod_id=" + searchCriteria.ViewCriteriaAmodID.ToString
                  ' sRefLink += "&amod_id=" + searchCriteria.

                  sRefTitle = IIf(CBool(HttpContext.Current.Application.Item("DebugFlag").ToString), " title=""" + sRefLink.Trim + """", " title=""Click to view aircraft at location""")

                  htmlOut.Append("<a href=""" + sRefLink + """" + sRefTitle + ">" + r.Item("state_name").ToString.Trim + "</a></td>")

                  htmlOut.Append("<td align='right' valign='top' class='seperator' style='padding-right:5px;'>" + FormatNumber(r.Item("tcount").ToString, 0) + "</td></tr>")

                End If
              End If

            End If

          Next

          If total_count_unknown > 0 Then
            If Not toggleRowColor Then
              htmlOut.Append("<tr class='alt_row'>")
              toggleRowColor = True
            Else
              htmlOut.Append("<tr bgcolor='white'>")
              toggleRowColor = False
            End If

            htmlOut.Append("<td align='left' valign='top' class='seperator' width='80%'>Unknown State/Province</td>")
            htmlOut.Append("<td align='right' valign='top' class='seperator' style='padding-right:5px;'>" + total_count_unknown.ToString + "</td></tr>")
          End If

          htmlOut.Append("</table>")

          If results_table.Rows.Count > 15 Then
            htmlOut.Append("</p></div>")
          End If

          htmlOut.Append("</td></tr></table></td></tr>")

        Else
          htmlOut.Append("<tr><td align='left' colspan='2' valign='middle'>No aircraft states/provinces for your search criteria.</td></tr>")
        End If
      Else
        htmlOut.Append("<tr><td align='left' colspan='2' valign='middle'>No aircraft states/provinces for your search criteria.</td></tr>")
      End If

      htmlOut.Append("</table></div>")

    Catch ex As Exception

      aError = "Error in views_display_location_state_prov(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

    Finally

    End Try

    'return resulting html string
    out_htmlString = htmlOut.ToString
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Sub views_display_location_city(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByRef locationTable As DataTable, ByVal bHasMaster As Boolean)

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder
    Dim toggleRowColor As Boolean = False
    Dim total_count_unknown As Long = 0

    Dim sRefLink As String = ""
    Dim sRefTitle As String = ""

    Try

      locationTable = Nothing

      results_table = get_location_state_prov(searchCriteria, "City")

      htmlOut.Append("<div class=""Box""><table id='displayCityOuterTable' width='100%' cellspacing='0' cellpadding='0' class='" & IIf(bHasMaster = False, "formatTable blue", "module") & "'>")

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          locationTable = results_table

          Select Case (searchCriteria.ViewCriteriaLocationViewType)
            Case Constants.LOCATION_VIEW_BASE
              htmlOut.Append("<tr><td align='center' valign='middle' class='header'>CITIES <em>(" + results_table.Rows.Count.ToString + ") by Aircraft Base</em></td></tr>")
            Case Constants.LOCATION_VIEW_OWNER
              htmlOut.Append("<tr><td align='center' valign='middle' class='header'>CITIES <em>(" + results_table.Rows.Count.ToString + ") by Aircraft Owners</em></td></tr>")
            Case Constants.LOCATION_VIEW_OPERATOR
              htmlOut.Append("<tr><td align='center' valign='middle' class='header'>CITIES <em>(" + results_table.Rows.Count.ToString + ") by Aircraft Operators</em></td></tr>")
          End Select

          htmlOut.Append("<tr><td valign='top' align='left'><table id='displayCityInnerTable' width='100%' cellpadding='0' cellspacing='0'>")
          htmlOut.Append("<tr><td class='rightside' colspan='2'>")

          If results_table.Rows.Count > 15 Then
            htmlOut.Append("<div valign=""top"" style=""height:370px; overflow: auto;""><p>")
          End If

          htmlOut.Append("<table id='displayCityDataTable' width='100%' cellpadding='4' cellspacing='0'>")

          htmlOut.Append("<tr><td valign='top' align='left' class='seperator' width='80%' style='padding-left:5px;' valign='top'><strong>City</strong></td>")
          htmlOut.Append("<td valign='top' align='right' class='seperator' style='padding-right:5px;'><strong>Count</strong></td></tr>")

          For Each r As DataRow In results_table.Rows

            If CLng(r.Item("tcount").ToString) Then

              If searchCriteria.ViewCriteriaLocationViewType <> Constants.LOCATION_VIEW_BASE Then
                If r.Item("comp_city").ToString.Trim.ToLower.Contains("unknown") Then
                  total_count_unknown += CLng(r.Item("tcount").ToString)
                Else
                  If Not toggleRowColor Then
                    htmlOut.Append("<tr class='alt_row'>")
                    toggleRowColor = True
                  Else
                    htmlOut.Append("<tr bgcolor='white'>")
                    toggleRowColor = False
                  End If

                  htmlOut.Append("<td align='left' valign='top' class='seperator' width='80%'>")

                  sRefLink = "view_template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName.Trim
                  sRefLink += "&onLocationTab=" + IIf(searchCriteria.ViewID < 2, "true", "false") & IIf(bHasMaster = False, "&noMaster=false", "")
                  sRefLink += "&viewCountry=" + HttpContext.Current.Server.UrlEncode(searchCriteria.ViewCriteriaCountry.Trim)
                  sRefLink += "&viewCity=" + HttpContext.Current.Server.UrlEncode(r.Item("comp_city").ToString.Trim)
                  sRefLink += "&viewContinent="
                  If Not IsDBNull(r.Item("state_name")) Then
                    sRefLink += "&viewState=" & HttpContext.Current.Server.UrlEncode(r.Item("state_name").ToString.Trim)
                  Else
                    sRefLink += "&viewState="
                  End If
                  sRefLink += "&amod_id=" + searchCriteria.ViewCriteriaAmodID.ToString

                  sRefTitle = IIf(CBool(HttpContext.Current.Application.Item("DebugFlag").ToString), " title=""" + sRefLink.Trim + """", " title=""Click to view aircraft at location""")

                  htmlOut.Append("<a href=""" + sRefLink + """" + sRefTitle + ">" + r.Item("comp_city").ToString.Trim + "</a></td>")

                  htmlOut.Append("<td align='right' valign='top' class='seperator' style='padding-right:5px;'>" + r.Item("tcount").ToString + "</td></tr>")

                End If

              Else
                If r.Item("ac_aport_city").ToString.Trim.ToLower.Contains("unknown") Then
                  total_count_unknown += CLng(r.Item("tcount").ToString)
                Else
                  If Not toggleRowColor Then
                    htmlOut.Append("<tr class='alt_row'>")
                    toggleRowColor = True
                  Else
                    htmlOut.Append("<tr bgcolor='white'>")
                    toggleRowColor = False
                  End If

                  htmlOut.Append("<td align='left' valign='top' class='seperator' width='80%'>")

                  sRefLink = "view_template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName.Trim
                  sRefLink += "&onLocationTab=" + IIf(searchCriteria.ViewID < 2, "true", "false") & IIf(bHasMaster = False, "&noMaster=false", "")
                  sRefLink += "&viewCountry=" + HttpContext.Current.Server.UrlEncode(searchCriteria.ViewCriteriaCountry.Trim)
                  sRefLink += "&viewCity=" + HttpContext.Current.Server.UrlEncode(r.Item("ac_aport_city").ToString.Trim)
                  sRefLink += "&viewContinent="
                  If Not IsDBNull(r.Item("state_name")) Then
                    sRefLink += "&viewState=" & HttpContext.Current.Server.UrlEncode(r.Item("state_name").ToString.Trim)
                  Else
                    sRefLink += "&viewState="
                  End If
                  sRefLink += "&amod_id=" + searchCriteria.ViewCriteriaAmodID.ToString

                  sRefTitle = IIf(CBool(HttpContext.Current.Application.Item("DebugFlag").ToString), " title=""" + sRefLink.Trim + """", " title=""Click to view aircraft at location""")

                  htmlOut.Append("<a href=""" + sRefLink + """" + sRefTitle + ">" + r.Item("ac_aport_city").ToString.Trim + "</a></td>")

                  htmlOut.Append("<td align='right' valign='top' class='seperator' style='padding-right:5px;'>" + r.Item("tcount").ToString + "</td></tr>")

                End If

              End If

            End If

          Next

          If total_count_unknown > 0 Then
            If Not toggleRowColor Then
              htmlOut.Append("<tr class='alt_row'>")
              toggleRowColor = True
            Else
              htmlOut.Append("<tr bgcolor='white'>")
              toggleRowColor = False
            End If

            htmlOut.Append("<td align='left' valign='top' class='seperator' width='80%'>Unknown City</td>")
            htmlOut.Append("<td align='right' valign='top' class='seperator' style='padding-right:5px;'>" + total_count_unknown.ToString + "</td></tr>")
          End If

          htmlOut.Append("</table>")

          If results_table.Rows.Count > 15 Then
            htmlOut.Append("</p></div>")
          End If

          htmlOut.Append("</td></tr></table></td></tr>")

        Else
          htmlOut.Append("<tr><td align='left' colspan='2' valign='middle'>No aircraft cities for your search criteria.</td></tr>")
        End If
      Else
        htmlOut.Append("<tr><td align='left' colspan='2' valign='middle'>No aircraft cities for your search criteria.</td></tr>")
      End If

      htmlOut.Append("</table></div>")

    Catch ex As Exception

      aError = "Error in views_display_location_city(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

    Finally

    End Try

    'return resulting html string
    out_htmlString = htmlOut.ToString
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Function get_location_aircraft_info(ByRef searchCriteria As viewSelectionCriteriaClass, Optional ByVal from_spot As String = "") As DataTable
    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      If searchCriteria.ViewCriteriaLocationViewType <> Constants.LOCATION_VIEW_BASE Then

        If searchCriteria.ViewCriteriaCountryHasStates Then
          sQuery.Append("SELECT amod_make_name, amod_model_name, amod_id, ac_ser_no_full, ac_reg_no, ac_mfr_year, ac_id, comp_name, comp_city, comp_state, state_name, comp_country, comp_id,")
          sQuery.Append(" (SELECT TOP 1 map_latitude FROM mapping WITH(NOLOCK) WHERE lower(comp_country) = lower(map_country) AND lower(comp_state) = lower(map_state) AND lower(comp_city) = lower(map_city) AND map_latitude <> 0) AS tlatitude,")
          sQuery.Append(" (SELECT TOP 1 map_longitude FROM mapping WITH(NOLOCK) WHERE lower(comp_country) = lower(map_country) AND lower(comp_state) = lower(map_state) AND lower(comp_city) = lower(map_city) AND map_longitude <> 0) AS tlongitude")
        Else
          sQuery.Append("SELECT amod_make_name, amod_model_name, amod_id, ac_ser_no_full, ac_reg_no, ac_mfr_year, ac_id, comp_name, comp_city, comp_country, comp_id,")
          sQuery.Append(" (SELECT TOP 1 map_latitude FROM mapping WITH(NOLOCK) WHERE lower(comp_country) = lower(map_country) AND lower(comp_city) = lower(map_city) AND map_latitude <> 0) AS tlatitude,")
          sQuery.Append(" (SELECT TOP 1 map_longitude FROM mapping WITH(NOLOCK) WHERE lower(comp_country) = lower(map_country) AND lower(comp_city) = lower(map_city) AND map_longitude <> 0) AS tlongitude")
        End If

        sQuery.Append(" FROM Aircraft_Summary WITH(NOLOCK)")

        If searchCriteria.ViewCriteriaCountryHasStates Then
          sQuery.Append(" INNER JOIN State WITH(NOLOCK) ON state_code = comp_state AND state_country = comp_country")
        End If

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaContinent.Trim) Then
          sQuery.Append(" INNER JOIN Country WITH(NOLOCK) ON ac_aport_country = country_name")
        End If

        sQuery.Append(" WHERE ac_lifecycle_stage = 3")

        If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
          sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))
        Else
          sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True))
        End If

        If searchCriteria.ViewCriteriaLocationViewType = Constants.LOCATION_VIEW_OWNER Then
          sQuery.Append(" AND cref_contact_type in ('00','08','17')")
        Else
          sQuery.Append(" AND cref_operator_flag IN ('Y', 'O')")
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

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaContinent.Trim) Then
          sQuery.Append(Constants.cAndClause + "lower(country_continent_name) = '" + searchCriteria.ViewCriteriaContinent.ToLower.Trim + "'")
        End If

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaCountry.Trim) Then
          sQuery.Append(Constants.cAndClause + "lower(comp_country) = '" + Replace(searchCriteria.ViewCriteriaCountry.Trim, Constants.cSingleQuote, Constants.cDoubleSingleQuote).ToLower + "'")
        End If

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaState.Trim) Then
          sQuery.Append(Constants.cAndClause + "lower(state_name) = '" + Replace(searchCriteria.ViewCriteriaState.Trim, Constants.cSingleQuote, Constants.cDoubleSingleQuote).ToLower + "'")
        End If

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaCity.Trim) Then
          sQuery.Append(Constants.cAndClause + "lower(comp_city) = '" + Replace(searchCriteria.ViewCriteriaCity.Trim, Constants.cSingleQuote, Constants.cDoubleSingleQuote).ToLower + "'")
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



        If searchCriteria.ViewCriteriaCountryHasStates Then
          sQuery.Append(Constants.cAndClause + "comp_state IS NOT NULL AND comp_state <> ''")
          sQuery.Append(Constants.cAndClause + "comp_city IS NOT NULL AND comp_city <> ''")
        Else
          sQuery.Append(Constants.cAndClause + "comp_city IS NOT NULL AND comp_city <> ''")
        End If

        If searchCriteria.ViewCriteriaCountryHasStates Then
          sQuery.Append(" GROUP BY amod_make_name, amod_model_name, amod_id, ac_ser_no_full, ac_reg_no, ac_mfr_year, ac_id, comp_name, comp_city, comp_state, state_name, comp_country, comp_id")
          sQuery.Append(" ORDER BY amod_make_name, amod_model_name, ac_ser_no_full, ac_reg_no, ac_mfr_year, ac_id ASC")
        Else
          sQuery.Append(" GROUP BY amod_make_name, amod_model_name, amod_id, ac_ser_no_full, ac_reg_no, ac_mfr_year, ac_id, comp_name, comp_city, comp_country, comp_id")
          sQuery.Append(" ORDER BY amod_make_name, amod_model_name, ac_ser_no_full, ac_reg_no, ac_mfr_year, ac_id ASC")
        End If

      Else

        If searchCriteria.ViewCriteriaCountryHasStates Then
          sQuery.Append("SELECT amod_make_name, amod_model_name, amod_id, ac_ser_no_full, ac_reg_no, ac_mfr_year, ac_id, ac_aport_city, ac_aport_state, state_name, ac_aport_country, comp_name, comp_id,")
          sQuery.Append(" (SELECT TOP 1 map_latitude FROM mapping WITH(NOLOCK) WHERE lower(ac_aport_country) = lower(map_country) AND lower(ac_aport_state) = lower(map_state) AND lower(ac_aport_city) = lower(map_city) AND map_latitude <> 0) AS tlatitude,")
          sQuery.Append(" (SELECT TOP 1 map_longitude FROM mapping WITH(NOLOCK) WHERE lower(ac_aport_country) = lower(map_country) AND lower(ac_aport_state) = lower(map_state) AND lower(ac_aport_city) = lower(map_city) AND map_longitude <> 0) AS tlongitude")
        Else
          sQuery.Append("SELECT amod_make_name, amod_model_name, amod_id, ac_ser_no_full, ac_reg_no, ac_mfr_year, ac_id, ac_aport_city, ac_aport_country, comp_name, comp_id,")
          sQuery.Append(" (SELECT TOP 1 map_latitude FROM mapping WITH(NOLOCK) WHERE lower(ac_aport_country) = lower(map_country) AND lower(ac_aport_city) = lower(map_city) AND map_latitude <> 0) AS tlatitude,")
          sQuery.Append(" (SELECT TOP 1 map_longitude FROM mapping WITH(NOLOCK) WHERE lower(ac_aport_country) = lower(map_country) AND lower(ac_aport_city) = lower(map_city) AND map_longitude <> 0) AS tlongitude")
        End If

        sQuery.Append(" FROM Aircraft_Summary WITH(NOLOCK)")

        If searchCriteria.ViewCriteriaCountryHasStates Then
          sQuery.Append(" INNER JOIN State WITH(NOLOCK) ON state_code = ac_aport_state AND state_country = ac_aport_country")
        End If

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaContinent.Trim) Then
          sQuery.Append(" INNER JOIN Country WITH(NOLOCK) ON ac_aport_country = country_name")
        End If

        sQuery.Append(" WHERE ac_lifecycle_stage = 3")

        If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
          sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))
        Else
          sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True))
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

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaContinent.Trim) Then
          sQuery.Append(Constants.cAndClause + "lower(country_continent_name) = '" + searchCriteria.ViewCriteriaContinent.ToLower.Trim + "'")
        End If

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaCountry.Trim) Then
          sQuery.Append(Constants.cAndClause + "lower(ac_aport_country) = '" + Replace(searchCriteria.ViewCriteriaCountry.Trim, Constants.cSingleQuote, Constants.cDoubleSingleQuote).ToLower + "'")
        End If

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaState.Trim) Then
          sQuery.Append(Constants.cAndClause + "lower(state_name) = '" + Replace(searchCriteria.ViewCriteriaState.Trim, Constants.cSingleQuote, Constants.cDoubleSingleQuote).ToLower + "'")
        End If

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaCity.Trim) Then
          sQuery.Append(Constants.cAndClause + "lower(ac_aport_city) = '" + Replace(searchCriteria.ViewCriteriaCity.Trim, Constants.cSingleQuote, Constants.cDoubleSingleQuote).ToLower + "'")
        End If

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAirportIATA.Trim) Then
          sQuery.Append(Constants.cAndClause + "ac_aport_iata_code = '" + searchCriteria.ViewCriteriaAirportIATA.Trim + "'")
        End If

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAirportICAO.Trim) Then
          sQuery.Append(Constants.cAndClause + "ac_aport_icao_code = '" + searchCriteria.ViewCriteriaAirportICAO.Trim + "'")
        End If


        If Trim(from_spot) = "City" Then
          If searchCriteria.ViewCriteriaCountryHasStates Then
            sQuery.Append(Constants.cAndClause + "ac_aport_state IS NOT NULL AND ac_aport_state <> ''")
            sQuery.Append(Constants.cAndClause + "ac_aport_city IS NOT NULL AND ac_aport_city <> ''")
          Else
            sQuery.Append(Constants.cAndClause + "ac_aport_city IS NOT NULL AND ac_aport_city <> ''")
          End If
        Else
          If searchCriteria.ViewCriteriaCountryHasStates Then
            sQuery.Append(Constants.cAndClause + "ac_aport_state IS NOT NULL AND ac_aport_state <> ''")
            ' sQuery.Append(Constants.cAndClause + "ac_aport_city IS NOT NULL AND ac_aport_city <> ''")
          Else
            ' sQuery.Append(Constants.cAndClause + "ac_aport_city IS NOT NULL AND ac_aport_city <> ''")
          End If
        End If


        If searchCriteria.ViewCriteriaCountryHasStates Then
          sQuery.Append(" GROUP BY amod_make_name, amod_model_name, amod_id, ac_ser_no_full, ac_reg_no, ac_mfr_year, ac_id, comp_name, ac_aport_city, ac_aport_state, state_name, ac_aport_country, comp_id")
          sQuery.Append(" ORDER BY amod_make_name, amod_model_name, ac_ser_no_full, ac_reg_no, ac_mfr_year")
        Else
          sQuery.Append(" GROUP BY amod_make_name, amod_model_name, amod_id, ac_ser_no_full, ac_reg_no, ac_mfr_year, ac_id, comp_name, ac_aport_city, ac_aport_country, comp_id")
          sQuery.Append(" ORDER BY amod_make_name, amod_model_name, ac_ser_no_full, ac_reg_no, ac_mfr_year")
        End If

      End If

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_location_aircraft_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
        aError = "Error in get_location_aircraft_info load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_location_aircraft_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

  Public Sub views_display_location_aircraft(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByRef locationTable As DataTable, Optional ByVal from_spot As String = "", Optional ByVal bHasMaster As Boolean = True)

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder

    Dim toggleRowColor As Boolean = False

    Dim previous_acid As String = ""
    Dim ac_count As Integer = 0

    Dim sRefLink As String = ""
    Dim sRefTitle As String = ""

    Try

      locationTable = Nothing

      results_table = get_location_aircraft_info(searchCriteria, from_spot)

      Dim tmpAcDetails As String = make_linkback_aircraftInfo(searchCriteria)

      htmlOut.Append("<div class=""Box""><table id='displayAircraftOuterTable' width='100%' cellspacing='0' cellpadding='0' class='" & IIf(bHasMaster = False, "formatTable blue", "module") & "'>")

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          locationTable = results_table

          For Each ac As DataRow In results_table.Rows
            If Not previous_acid.Trim.ToLower.Contains(ac.Item("ac_id").ToString.Trim.ToLower) Then
              previous_acid = ac.Item("ac_id").ToString.Trim
              ac_count += 1
            End If
          Next

          Select Case (searchCriteria.ViewCriteriaLocationViewType)
            Case Constants.LOCATION_VIEW_BASE
              htmlOut.Append("<tr><td align='center' valign='middle' class='header'>AIRCRAFT&nbsp;<em>(" + ac_count.ToString + ") by Aircraft Base</em></td></tr>")
            Case Constants.LOCATION_VIEW_OWNER
              htmlOut.Append("<tr><td align='center' valign='middle' class='header'>AIRCRAFT&nbsp;<em>(" + ac_count.ToString + ") by Aircraft Owners</em></td></tr>")
            Case Constants.LOCATION_VIEW_OPERATOR
              htmlOut.Append("<tr><td align='center' valign='middle' class='header'>AIRCRAFT&nbsp;<em>(" + ac_count.ToString + ") by Aircraft Operators</em></td></tr>")
          End Select

          sRefLink = "javascript:ParseForm('0',false,false,false,false,false,'"

          If Not String.IsNullOrEmpty(tmpAcDetails.Trim) Then
            sRefLink += tmpAcDetails.Trim + "!~!"
          End If

          sRefLink += "clearSelection=true');"

          sRefTitle = IIf(CBool(HttpContext.Current.Application.Item("DebugFlag").ToString), " title=""" + sRefLink.Trim + """", " title=""Click to view aircraft list""")


          htmlOut.Append("<tr bgcolor='white'><td align='center' valign='middle'><a class=""underline cursor"" onclick=""" + sRefLink.Trim + """" + sRefTitle + "><strong>Click to view aircraft list</strong></a></td></tr>")

          htmlOut.Append("<tr><td valign='top' align='left'><table id='displayAircraftInnerTable' width='100%' cellpadding='0' cellspacing='0'>")
          htmlOut.Append("<tr><td class='rightside' colspan='2'>")

          If results_table.Rows.Count > 15 Then
            htmlOut.Append("<div valign=""top"" style=""height:370px; overflow: auto;""><p>")
          End If

          htmlOut.Append("<table id='displayAircraftDataTable' width='100%' cellpadding='4' cellspacing='0'>")

          For Each r As DataRow In results_table.Rows

            If Not previous_acid.Trim.ToLower.Contains(r.Item("ac_id").ToString.Trim.ToLower) Then

              If Not toggleRowColor Then
                htmlOut.Append("<tr class='alt_row'>")
                toggleRowColor = True
              Else
                htmlOut.Append("<tr bgcolor='white'>")
                toggleRowColor = False
              End If


              htmlOut.Append("<td align='left' valign='top' class='seperator' width='5%'><img src='images/ch_red.jpg' class='bullet' alt='acid : " + r.Item("ac_id").ToString + "' /></td>")
              htmlOut.Append("<td align='left' valign='middle' class='seperator'> Serial# <a class='underline' onclick=""javascript:load('DisplayAircraftDetail.aspx?acid=" + r.Item("ac_id").ToString + "&jid=0','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');"" title='Display Aircraft Details'>")
              htmlOut.Append(r.Item("ac_ser_no_full").ToString + "</a>")

              htmlOut.Append(", Reg# " + r.Item("ac_reg_no").ToString)
              htmlOut.Append(" " + r.Item("amod_make_name").ToString + " / " + r.Item("amod_model_name").ToString)

              If searchCriteria.ViewCriteriaLocationViewType <> Constants.LOCATION_VIEW_BASE Then
                htmlOut.Append("<br /><a class='underline' onclick=""javascript:load('DisplayCompanyDetail.aspx?compid=" + r.Item("comp_id").ToString + "&journid=0','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');"" title='Display Company Details'>")
                htmlOut.Append(Replace(r.Item("comp_name").ToString, Constants.cSingleSpace, Constants.cHTMLnbsp) + "</a> <em>(" + r.Item("comp_country").ToString.Trim + ")</em>")
              Else
                htmlOut.Append("<br /><a class='underline' onclick=""javascript:load('DisplayCompanyDetail.aspx?compid=" + r.Item("comp_id").ToString + "&journid=0','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');"" title='Display Company Details'>")
                htmlOut.Append(Replace(r.Item("comp_name").ToString, Constants.cSingleSpace, Constants.cHTMLnbsp) + "</a> <em>(" + r.Item("ac_aport_country").ToString.Trim + ")</em>")
              End If

              htmlOut.Append("</td></tr>")

              previous_acid = r.Item("ac_id").ToString.Trim

            End If

          Next

          htmlOut.Append("</table>")

          If results_table.Rows.Count > 15 Then
            htmlOut.Append("</p></div>")
          End If

          htmlOut.Append("</td></tr></table></td></tr>")

        Else
          htmlOut.Append("<tr><td align='left' colspan='2' valign='middle'>No Aircraft for your search criteria.</td></tr>")
        End If
      Else
        htmlOut.Append("<tr><td align='left' colspan='2' valign='middle'>No Aircraft for your search criteria.</td></tr>")
      End If

      htmlOut.Append("</table></div>")

    Catch ex As Exception

      aError = "Error in views_display_location_aircraft(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

    Finally

    End Try

    out_htmlString = htmlOut.ToString()
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Function make_linkback_aircraftInfo(ByRef searchCriteria As viewSelectionCriteriaClass) As String

    Dim nTmpIndex As Long = -1
    Dim nTmpModelID As Long = -1

    Dim sAirFrame As String = ""
    Dim sAirType As String = ""
    Dim sMake As String = ""
    Dim sModel As String = ""
    Dim sUsage As String = ""

    Dim linkback_info As String = ""

    ' add product filters
    linkback_info = "chkHelicopterFilterID=" + searchCriteria.ViewCriteriaHasHelicopterFlag.ToString.ToLower
    linkback_info += "!~!chkBusinessFilterID=" + searchCriteria.ViewCriteriaHasBusinessFlag.ToString.ToLower
    linkback_info += "!~!chkCommercialFilterID=" + searchCriteria.ViewCriteriaHasCommercialFlag.ToString.ToLower


    If (searchCriteria.ViewCriteriaHasHelicopterFlag Or searchCriteria.ViewCriteriaHasBusinessFlag Or searchCriteria.ViewCriteriaHasCommercialFlag) Then
      linkback_info += "!~!hasModelFilterID=True"
    End If

    ' check for model id first
    If searchCriteria.ViewCriteriaAmodID > -1 Then

      nTmpModelID = searchCriteria.ViewCriteriaAmodID

      If nTmpModelID > -1 Then
        nTmpIndex = commonEvo.FindIndexForItemByAmodID(nTmpModelID)
        commonEvo.ReturnModelDataFromIndex(nTmpIndex, sAirFrame, sAirType, sMake, sModel, sUsage)
      End If

      linkback_info += "!~!cboAircraftTypeID=" + sAirType + Constants.cSvrDataSeperator + sAirFrame
      linkback_info += "!~!cboAircraftMakeID=" + sMake.ToUpper.Trim
      linkback_info += "!~!cboAircraftModelID=" + nTmpModelID.ToString

      If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaCountry.Trim) Then
        linkback_info += "!~!cboCompanyCountryID=" + Replace(searchCriteria.ViewCriteriaCountry.Trim, Constants.cSingleQuote, Constants.cDoubleSingleQuote).Trim
      End If

      If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaState.Trim) Then
        linkback_info += "!~!cboCompanyStateID=" + Replace(searchCriteria.ViewCriteriaState.Trim, Constants.cSingleQuote, Constants.cDoubleSingleQuote).Trim
      End If

      Return linkback_info

    End If

    ' clean out any "ticks" from the "type/make/model" selections

    If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftType.Trim) Then

      linkback_info += "!~!cboAircraftTypeID=" + searchCriteria.ViewCriteriaAircraftType.Trim

      If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAirframeTypeStr.Trim) Then
        linkback_info += Constants.cSvrDataSeperator + searchCriteria.ViewCriteriaAirframeTypeStr.Trim
      End If

    Else

      If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAirframeTypeStr.Trim) Then
        linkback_info += "!~!cboAircraftTypeID=" + searchCriteria.ViewCriteriaAirframeTypeStr.ToUpper.Trim
      End If

    End If

    If Not IsNothing(searchCriteria.ViewCriteriaMakeIDArray) Then
      Dim tmpStr As String = ""

      ' flatten out amodID array ...
      For x As Integer = 0 To UBound(searchCriteria.ViewCriteriaMakeIDArray)
        If String.IsNullOrEmpty(tmpStr) Then
          tmpStr = searchCriteria.ViewCriteriaMakeIDArray(x)
        Else
          tmpStr += Constants.cCommaDelim + searchCriteria.ViewCriteriaMakeIDArray(x)
        End If
      Next

      linkback_info += "!~!cboAircraftMakeID=" + tmpStr.Trim

    End If

    If Not IsNothing(searchCriteria.ViewCriteriaAmodIDArray) Then
      Dim tmpStr As String = ""

      ' flatten out amodID array ...
      For x As Integer = 0 To UBound(searchCriteria.ViewCriteriaAmodIDArray)
        If String.IsNullOrEmpty(tmpStr) Then
          tmpStr = searchCriteria.ViewCriteriaAmodIDArray(x)
        Else
          tmpStr += Constants.cCommaDelim + searchCriteria.ViewCriteriaAmodIDArray(x)
        End If
      Next

      linkback_info += "!~!cboAircraftModelID=" + tmpStr.Trim

    End If

    If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAirframeTypeStr.Trim) Then
      linkback_info += "!~!radContinentRegionID=" + IIf(searchCriteria.ViewCriteriaUseContinent, "true", "false")
    End If

    If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaContinent.Trim) Then
      linkback_info += "!~!cboCompanyRegionID=" + searchCriteria.ViewCriteriaContinent.Trim
    End If

    If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaCountry.Trim) Then
      linkback_info += "!~!cboCompanyCountryID=" + Replace(searchCriteria.ViewCriteriaCountry.Trim, Constants.cSingleQuote, Constants.cDoubleSingleQuote).Trim
    End If

    If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaState.Trim) Then
      linkback_info += "!~!cboCompanyStateID=" + Replace(searchCriteria.ViewCriteriaState.Trim, Constants.cSingleQuote, Constants.cDoubleSingleQuote).Trim
    End If

    If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaTimeZone.Trim) Then
      linkback_info += "!~!cboCompanyTimeZoneID=" + searchCriteria.ViewCriteriaTimeZone.Trim
    End If

    Return linkback_info

  End Function

#End Region

End Class
