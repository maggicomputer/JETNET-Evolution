' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/Entity Classes/fractional_view_functions.vb $
'$$Author: Mike $
'$$Date: 6/19/19 8:49a $
'$$Modtime: 6/18/19 6:12p $
'$$Revision: 2 $
'$$Workfile: fractional_view_functions.vb $
'
' ********************************************************************************

<System.Serializable()> Public Class fractional_view_functions

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

#Region "fractional_functions"

  Public Function get_fractional_programs(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT DISTINCT prog_id, prog_comp_id, prog_name, (SELECT count(*) AS Expr1 FROM aircraft WITH(NOLOCK)")
      sQuery.Append(" INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id")
      sQuery.Append(" WHERE (ac_journ_id = 0) and (ac_ownership_type='F')")

      If searchCriteria.ViewCriteriaAmodID > -1 Then
        sQuery.Append(crmWebClient.Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
        sQuery.Append(crmWebClient.Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
      End If

      sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))

      sQuery.Append(" AND (ac_id IN (SELECT distinct cref_ac_id FROM aircraft_reference WITH(NOLOCK) WHERE (cref_contact_type='17') AND (cref_journ_id = 0) AND")
      sQuery.Append(" (cref_comp_id IN (SELECT DISTINCT pgref_comp_id FROM program_reference")
      sQuery.Append(" WITH(NOLOCK) WHERE pgref_prog_id = prog_id))))) AS aircraftCount")
      sQuery.Append(" FROM Aircraft_Programs WITH(NOLOCK) INNER JOIN program_reference WITH(NOLOCK) ON pgref_prog_id = prog_id")
      sQuery.Append(" WHERE prog_active_flag = 'Y'")

      If searchCriteria.ViewCriteriaFractionalProgramID > 0 Then
        sQuery.Append(crmWebClient.Constants.cAndClause + "prog_id = " + searchCriteria.ViewCriteriaFractionalProgramID.ToString)
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



      sQuery.Append(" GROUP BY prog_id, prog_name, prog_comp_id")
      sQuery.Append(" ORDER BY aircraftCount DESC, prog_name")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_fractional_programs(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
        aError = "Error in get_fractional_programs load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_fractional_programs(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

  Public Sub views_display_fractional_programs(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String)

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder
    Dim stringOut As New StringBuilder
    Dim toggleRowColor As Boolean = False

    Dim totalModelCount As Long = 0
    Dim totalProgramCount As Long = 0

    Dim bOneProg As Boolean = False
    Dim nProgramCompanyID As Long = 0
    Dim clear_program_link As String = ""

    If searchCriteria.ViewCriteriaFractionalProgramID > 0 Then
      bOneProg = True
    End If

    Try
      clear_program_link = "<a href='view_template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName + "&selectFractionalProgram=-1&amod_id=" & searchCriteria.ViewCriteriaAmodID.ToString & "'>Clear Program</a>"
      results_table = get_fractional_programs(searchCriteria)

      htmlOut.Append("<table id='fractionalProgramsOuterTable' width='100%' cellspacing='0' cellpadding='0' class='module'>")

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          stringOut.Append("<tr><td valign='top' align='left'>")
          stringOut.Append("<table id='fractionalProgramsInnerTable' width='100%' cellpadding='0' cellspacing='0'><tr>")
          stringOut.Append("<td valign='top' align='left' class='seperator' width='80%' style='padding-left:3px;'><strong>Program&nbsp;Name</strong></td>")
          stringOut.Append("<td valign='top' align='right' class='seperator' width='20%' style='padding-right:5px;'><strong>Count</strong></td></tr>")

          stringOut.Append("<tr><td colspan='2' class='rightside'>")

          If searchCriteria.ViewCriteriaFractionalProgramID > 0 Then
            stringOut.Append(clear_program_link)
            stringOut.Append("</td></tr>")
            stringOut.Append("<tr><td colspan='2' class='rightside'>")
          End If


          If results_table.Rows.Count > 15 Then
            stringOut.Append("<div valign=""top"" style='height:370px; overflow: auto;'><p>")
          End If

          stringOut.Append("<table id='fractionalProgramsDataTable' width='100%' cellpadding='4' cellspacing='0'>")

          For Each r As DataRow In results_table.Rows

            If Not IsDBNull(r("aircraftCount")) Then

              If CLng(r.Item("aircraftCount").ToString) > 0 Then

                If Not toggleRowColor Then
                  stringOut.Append("<tr class='alt_row'>")
                  toggleRowColor = True
                Else
                  stringOut.Append("<tr bgcolor='white'>")
                  toggleRowColor = False
                End If

                If bOneProg Then
                  nProgramCompanyID = CLng(r.Item("prog_comp_id").ToString)
                End If

                stringOut.Append("<td align='left' valign='top' class='seperator' width='80%'>")
                stringOut.Append("<a href='view_template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName + "&selectFractionalProgram=" + r.Item("prog_id").ToString + "&amod_id=" + searchCriteria.ViewCriteriaAmodID.ToString + "' title='Click to view " + r.Item("prog_name").ToString.Trim + " fractional program.'>")
                stringOut.Append(r.Item("prog_name").ToString.Trim)
                stringOut.Append("</a></td>")
                stringOut.Append("<td align='right' valign='top' class='seperator' width='20%' style='padding-right:5px;'>" + r.Item("aircraftCount").ToString + "</td></tr>")

                totalModelCount += CLng(r.Item("aircraftCount").ToString)
                totalProgramCount += 1

              End If
            End If

          Next

          ' if we have no models in the programs clear the stringOut
          If totalModelCount = 0 Then
            stringOut = Nothing
            stringOut = New StringBuilder
            stringOut.Append("<tr><td valign='top' align='left' class='seperator' style='padding-left:3px;'><br />No fractional programs include this model</td></tr>")
          Else

            stringOut.Append("</table>")
            If results_table.Rows.Count > 15 Then
              stringOut.Append("</p></div>")
            End If
            stringOut.Append("</td></tr></table></td></tr>")

          End If

        Else
          stringOut.Append("<tr><td valign='top' align='left' class='seperator' style='padding-left:3px;'><br />No fractional programs/aircraft match for your search criteria</td></tr>")
        End If
      Else
        stringOut.Append("<tr><td valign='top' align='left' class='seperator' style='padding-left:3px;'><br />No fractional programs/aircraft match for your search criteria</td></tr>")
      End If

      ' we have to display the header after we process the records to get the right program count
      If searchCriteria.ViewCriteriaFractionalProgramID > 0 Then
        htmlOut.Append("<tr><td valign='top' align='center' class='header' >FRACTIONAL PROGRAMS <em>(" + totalModelCount.ToString + " aircraft in program)</em></td></tr>")
      Else
        htmlOut.Append("<tr><td valign='top' align='center' class='header' >FRACTIONAL PROGRAMS <em>(" + totalProgramCount.ToString + ")</em></td></tr>")
      End If

      ' place data table after the header
      htmlOut.Append(stringOut.ToString())

      If bOneProg Then
        htmlOut.Append("<tr><td align='left' valign='top' class='seperator'><br />")
        htmlOut.Append(commonEvo.get_company_info_fromID(nProgramCompanyID, 0, True, True, "", ""))
        htmlOut.Append("<br /></td></tr>")
      End If

      htmlOut.Append("</table>")

    Catch ex As Exception

      aError = "Error in views_display_fractional_programs(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

    Finally

    End Try

    'return resulting html string
    out_htmlString = htmlOut.ToString
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Function get_fractional_shares(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT DISTINCT cref_owner_percent, count(*) AS share_count")
      sQuery.Append(" FROM aircraft WITH(NOLOCK) INNER JOIN aircraft_reference WITH(NOLOCK) ON ac_id = cref_ac_id AND ac_journ_id = cref_journ_id")
      sQuery.Append(" INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id")
      sQuery.Append(" WHERE (ac_journ_id = 0) AND (ac_ownership_type = 'F') AND (cref_contact_type = '97')")

      If searchCriteria.ViewCriteriaAmodID > -1 Then
        sQuery.Append(crmWebClient.Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
        sQuery.Append(crmWebClient.Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
      End If

      sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))

      sQuery.Append(" AND (ac_id IN (SELECT DISTINCT cref_ac_id FROM aircraft_reference WITH(NOLOCK) WHERE (cref_contact_type='17') and (cref_journ_id = 0)")
      sQuery.Append(" AND (cref_comp_id IN (SELECT DISTINCT pgref_comp_id FROM program_reference WITH(NOLOCK)")

      If searchCriteria.ViewCriteriaFractionalProgramID > 0 Then
        sQuery.Append(" WHERE pgref_prog_id = " + searchCriteria.ViewCriteriaFractionalProgramID.ToString)
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



      sQuery.Append("))))")
      sQuery.Append(" GROUP BY cref_owner_percent")
      sQuery.Append(" ORDER BY share_count DESC")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_fractional_shares(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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

  Public Sub views_display_fractional_shares(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String)

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder
    Dim toggleRowColor As Boolean = False

    Dim totalShareCount As Long = 0

    Try

      results_table = get_fractional_shares(searchCriteria)

      htmlOut.Append("<table id='fractionalSharesOuterTable' width='100%' cellspacing='0' cellpadding='0' class='module'>")
      htmlOut.Append("<tr><td valign='top' align='center' class='header'>AIRCRAFT SHARES</td></tr>")

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          htmlOut.Append("<tr><td valign='top' align='left'>")
          htmlOut.Append("<table id='fractionalSharesInnerTable' width='100%' cellpadding='1' cellspacing='0'><tr>")
          htmlOut.Append("<td valign='top' align='left' class='seperator' width='33%' style='padding-left:3px;'><strong>Share&nbsp;Size</strong></td>")
          htmlOut.Append("<td valign='top' align='left' class='seperator' width='33%' style='padding-left:3px;'><strong>#&nbsp;of&nbsp;Shares</strong></td>")
          htmlOut.Append("<td valign='top' align='left' class='seperator' width='33%' style='padding-left:3px;'><strong>%&nbsp;of&nbsp;Shares</strong></td></tr>")

          htmlOut.Append("<tr><td colspan='3' class='rightside'>")

          If results_table.Rows.Count > 15 Then
            htmlOut.Append("<div valign=""top"" style='height:370px; overflow: auto;'><p>")
          End If

          htmlOut.Append("<table id='fractionalSharesDataTable' width='100%' cellpadding='4' cellspacing='0'>")

          ' first get total share count
          For Each r As DataRow In results_table.Rows
            totalShareCount += CLng(r.Item("share_count").ToString)
          Next

          For Each r As DataRow In results_table.Rows

            If Not toggleRowColor Then
              htmlOut.Append("<tr class='alt_row'>")
              toggleRowColor = True
            Else
              htmlOut.Append("<tr bgcolor='white'>")
              toggleRowColor = False
            End If

            htmlOut.Append("<td align='left' valign='top' class='seperator' width='33%'>" + r.Item("cref_owner_percent").ToString + "</td>")
            htmlOut.Append("<td align='left' valign='top' class='seperator' width='33%'>" + r.Item("share_count").ToString + "</td>")

            If totalShareCount > 0 Then
              htmlOut.Append("<td align='left' valign='top' class='seperator' width='33%'>" + FormatNumber(CDbl((CDbl(r.Item("share_count").ToString) / totalShareCount) * 100), 1).ToString + "%</td></tr>")
            Else
              htmlOut.Append("<td align='left' valign='top' class='seperator' width='33%'>" + r.Item("share_count").ToString + "</td></tr>")
            End If

          Next

          htmlOut.Append("</table>")
          If results_table.Rows.Count > 15 Then
            htmlOut.Append("</p></div>")
          End If
          htmlOut.Append("</td></tr></table></td></tr>")

        Else
          htmlOut.Append("<tr><td valign='top' align='left' class='seperator' style='padding-left:3px;'><br />No fractional shares match for your search criteria</td></tr>")
        End If
      Else
        htmlOut.Append("<tr><td valign='top' align='left' class='seperator' style='padding-left:3px;'><br />No fractional shares match for your search criteria</td></tr>")
      End If

      htmlOut.Append("</table>")

    Catch ex As Exception

      aError = "Error in views_display_fractional_shares(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

    Finally

    End Try

    'return resulting html string
    out_htmlString = htmlOut.ToString
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Function get_fractional_fleet(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT DISTINCT amod_make_name, amod_model_name, ac_ser_no_full, ac_reg_no, ac_mfr_year, ac_id")
      sQuery.Append(" FROM aircraft WITH(NOLOCK) INNER JOIN aircraft_reference WITH(NOLOCK) ON ac_id = cref_ac_id AND ac_journ_id = cref_journ_id")
      sQuery.Append(" INNER JOIN aircraft_model WITH(NOLOCK) ON ac_amod_id = amod_id")
      sQuery.Append(" WHERE (ac_journ_id = 0) AND (ac_ownership_type = 'F')")

      sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))

      If searchCriteria.ViewCriteriaAmodID > -1 Then
        sQuery.Append(crmWebClient.Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
        sQuery.Append(crmWebClient.Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
      End If

      sQuery.Append(" AND (ac_id IN (SELECT DISTINCT cref_ac_id FROM aircraft_reference WITH(NOLOCK) WHERE (cref_contact_type = '17') AND (cref_journ_id = 0)")
      sQuery.Append(" AND (cref_comp_id IN (SELECT DISTINCT pgref_comp_id FROM program_reference WITH(NOLOCK)")

      If searchCriteria.ViewCriteriaFractionalProgramID > 0 Then
        sQuery.Append(" WHERE pgref_prog_id = " + searchCriteria.ViewCriteriaFractionalProgramID.ToString)
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



      sQuery.Append(")) ))")
      sQuery.Append(" ORDER BY amod_make_name, amod_model_name, ac_ser_no_full")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_fractional_fleet(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
        aError = "Error in get_fractional_fleet load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_fractional_fleet(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

  Public Sub views_display_fractional_fleet(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String)

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder
    Dim toggleRowColor As Boolean = False
    Dim addBreak As String = ""

    Try

      results_table = get_fractional_fleet(searchCriteria)

      htmlOut.Append("<table id='fractionalFleetOuterTable' width='100%' cellspacing='0' cellpadding='0' class='module'>")

      If Not IsNothing(results_table) Then

        htmlOut.Append("<tr><td valign='top' align='center' class='header'>FRACTIONAL FLEET <em>(" + results_table.Rows.Count.ToString + ") by make / model</em></td></tr>")

        If results_table.Rows.Count > 0 Then

          htmlOut.Append("<tr><td valign='top' align='left'>")
          htmlOut.Append("<table id='fractionalFleetInnerTable' width='100%' cellpadding='1' cellspacing='0'><tr><td valign='top' align='left'>")
          htmlOut.Append("<tr><td colspan='2' class='rightside'>")

          If results_table.Rows.Count > 20 Then
            htmlOut.Append("<div valign=""top"" style='height:1024px; overflow: auto;'><p>")
          End If

          htmlOut.Append("<table id='fractionalFleetDataTable' width='100%' cellpadding='4' cellspacing='0'>")

          For Each r As DataRow In results_table.Rows

            If Not toggleRowColor Then
              htmlOut.Append("<tr class='alt_row'>")
              toggleRowColor = True
            Else
              htmlOut.Append("<tr bgcolor='white'>")
              toggleRowColor = False
            End If

            htmlOut.Append("<td align='left' valign='top' class='seperator'><img src='images/ch_red.jpg' class='bullet' alt='acid : " + r.Item("ac_id").ToString + "' /></td>")
            htmlOut.Append("<td align='left' valign='top' class='seperator'>")

            ' dont display make/model name if amod id > 0 (view already shows make model name, redundant data)
            If searchCriteria.ViewCriteriaAmodID = -1 Then
              htmlOut.Append(r.Item("amod_make_name").ToString + "&nbsp;/&nbsp;" + r.Item("amod_model_name").ToString + crmWebClient.Constants.cSingleSpace)
              addBreak = "<br />"
            End If

            htmlOut.Append(addBreak + "<a class='underline' target='_blank' href='DisplayAircraftDetail.aspx?acid=" + r.Item("ac_id").ToString + "' title='Display Aircraft Details'><strong>Serial# : </strong> " + r.Item("ac_ser_no_full").ToString + "</a> <strong>Reg# : </strong> " + r.Item("ac_reg_no").ToString + " <strong>MFR Year : </strong> " + r.Item("ac_mfr_year").ToString + "</td></tr>")

            addBreak = ""

          Next

          htmlOut.Append("</table>")
          If results_table.Rows.Count > 20 Then
            htmlOut.Append("</p></div>")
          End If
          htmlOut.Append("</td></tr></table></td></tr>")

        Else
          htmlOut.Append("<tr><td valign='top' align='left' class='seperator' style='padding-left:3px;'><br />No aircraft in fractional fleet for your search criteria</td></tr>")
        End If
      Else
        htmlOut.Append("<tr><td valign='top' align='left' class='seperator' style='padding-left:3px;'><br />No aircraft in fractional fleet for your search criteria</td></tr>")
      End If

      htmlOut.Append("</table>")

    Catch ex As Exception

      aError = "Error in views_display_fractional_fleet(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

    Finally

    End Try

    'return resulting html string
    out_htmlString = htmlOut.ToString
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Function get_fractional_shareholders(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT DISTINCT comp_id")

      sQuery.Append(" FROM company WITH(NOLOCK) INNER JOIN aircraft_reference WITH(NOLOCK) ON comp_id = cref_comp_id AND comp_journ_id = cref_journ_id AND comp_journ_id = 0")
      sQuery.Append(" WHERE cref_contact_type = '97' AND (cref_ac_id IN (")
      sQuery.Append(" SELECT cref_ac_id FROM aircraft_reference WITH(NOLOCK) INNER JOIN program_reference WITH(NOLOCK) ON pgref_comp_id = cref_comp_id AND cref_journ_id = 0")
      sQuery.Append(" INNER JOIN aircraft WITH(NOLOCK) ON cref_ac_id = ac_id AND cref_journ_id = ac_journ_id")
      sQuery.Append(" INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id")
      sQuery.Append(" WHERE cref_contact_type = '17'")



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



      If searchCriteria.ViewCriteriaFractionalProgramID > 0 Then
        sQuery.Append(crmWebClient.Constants.cAndClause + "pgref_prog_id = " + searchCriteria.ViewCriteriaFractionalProgramID.ToString)
      End If

      If searchCriteria.ViewCriteriaAmodID > -1 Then
        sQuery.Append(crmWebClient.Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
        sQuery.Append(crmWebClient.Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
      End If

      sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))

      sQuery.Append("))")

      sQuery.Append(" ORDER BY comp_id ASC")


      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_fractional_shareholders(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
        aError = "Error in get_fractional_shareholders load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_fractional_shareholders(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

  Public Sub views_display_fractional_shareholders(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String)

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder
    Dim toggleRowColor As Boolean = False
    Dim sTmpLable As String = ""

    Try

      If searchCriteria.ViewCriteriaAmodID > -1 Or searchCriteria.ViewCriteriaMakeAmodID > -1 Then
        sTmpLable = "have this make/model"
      Else
        sTmpLable = "for this program"
      End If

      results_table = get_fractional_shareholders(searchCriteria)

      htmlOut.Append("<table id='fractionalShareHoldersOuterTable' width='100%' cellpadding='1' cellspacing='0' class='module'>")

      If Not IsNothing(results_table) Then

        htmlOut.Append("<tr><td valign='top' align='left' class='header'>FRACTIONAL SHAREHOLDERS&nbsp;<em>(" + results_table.Rows.Count.ToString + ")</em></td></tr>")

        If results_table.Rows.Count > 0 Then

          htmlOut.Append("<tr><td valign='top' align='left'>")
          htmlOut.Append("<table id='fractionalShareHoldersInnerTable' width='100%' cellpadding='1' cellspacing='0'>")
          htmlOut.Append("<tr><td valign='middle' align='left' class='seperator' width='2%'>&nbsp;</td>")
          htmlOut.Append("<td valign='middle' align='left' class='seperator' width='80%'><strong>Company&nbsp;Name</strong></td>")
          htmlOut.Append("<td valign='middle' align='left' class='seperator' style='padding-right:15px;'><strong>%&nbsp;of&nbsp;Shares</strong></td></tr>")

          htmlOut.Append("<tr><td colspan='3' class='rightside' valign='top'>")
          htmlOut.Append("<table id='fractionalShareHoldersDataTable' width='100%' cellpadding='4' cellspacing='0'>")

          htmlOut.Append("<tr><td valign='top' align='left' class='seperator'>" + results_table.Rows.Count.ToString + " Shareholders " + sTmpLable.Trim + "</td></tr>")
          htmlOut.Append("<tr><td valign='top' align='left' class='seperator'><a href='fractionalShareholderList.aspx?clearRS=true&AmodID=" + searchCriteria.ViewCriteriaAmodID.ToString + "&ProgramID=" + searchCriteria.ViewCriteriaFractionalProgramID.ToString + "' title='Click to View Fractional Shareholders' target='_blank'>Display Shareholders ...</a></td></tr>")

          'For Each r As DataRow In results_table.Rows

          'Next

          htmlOut.Append("</table></td></tr></table></td></tr>")

        Else
          htmlOut.Append("<tr><td valign='top' align='left' class='seperator' style='padding-left:3px;'><br />No fractional shareholders for your search criteria</td></tr>")
        End If
      Else
        htmlOut.Append("<tr><td valign='top' align='left' class='seperator' style='padding-left:3px;'><br />No fractional shareholders for your search criteria</td></tr>")
      End If

      htmlOut.Append("</table>")

    Catch ex As Exception

      aError = "Error in views_display_fractional_shareholders(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

    Finally

    End Try

    'return resulting html string
    out_htmlString = htmlOut.ToString
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Function get_fractional_model_shares_inuse(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT DISTINCT amod_id, amod_make_name, amod_model_name, count(*) as ac_count")
      sQuery.Append(" FROM aircraft WITH(NOLOCK) INNER JOIN aircraft_model WITH(NOLOCK) ON ac_amod_id = amod_id")
      sQuery.Append(" WHERE (ac_journ_id = 0) AND (ac_ownership_type = 'F')")

      sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))

      sQuery.Append(" AND (ac_id IN (SELECT DISTINCT cref_ac_id FROM aircraft_reference WITH(NOLOCK) WHERE (cref_contact_type='17') and (cref_journ_id = 0)")
      sQuery.Append(" AND (cref_comp_id IN (SELECT DISTINCT pgref_comp_id FROM program_reference WITH(NOLOCK)")

      If searchCriteria.ViewCriteriaFractionalProgramID > 0 Then
        sQuery.Append(" WHERE pgref_prog_id = " + searchCriteria.ViewCriteriaFractionalProgramID.ToString)
        If searchCriteria.ViewCriteriaAmodID > -1 Then
          sQuery.Append(crmWebClient.Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
        ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
          sQuery.Append(crmWebClient.Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
        End If
      ElseIf searchCriteria.ViewCriteriaAmodID > -1 Then
        sQuery.Append(" WHERE amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
        sQuery.Append(" WHERE amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
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


      sQuery.Append("))))")



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


      sQuery.Append(" GROUP BY amod_make_name, amod_model_name, amod_id")
      sQuery.Append(" ORDER BY ac_count DESC")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_fractional_model_shares_inuse(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
        aError = "Error in get_fractional_model_shares_inuse load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_fractional_model_shares_inuse(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

  Public Function get_fractional_owner_percent(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal inAmodID As Long) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT DISTINCT sum(cref_owner_percent) AS sumPercent")
      sQuery.Append(" FROM company WITH(NOLOCK) INNER JOIN aircraft_reference WITH(NOLOCK) ON comp_id = cref_comp_id AND comp_journ_id = cref_journ_id AND comp_journ_id = 0")
      sQuery.Append(" WHERE cref_contact_type = '97' AND (cref_ac_id IN (")
      sQuery.Append(" SELECT cref_ac_id FROM aircraft_reference WITH(NOLOCK) INNER JOIN program_reference WITH(NOLOCK) ON pgref_comp_id = cref_comp_id AND cref_journ_id = 0")
      sQuery.Append(" INNER JOIN aircraft WITH(NOLOCK) ON cref_ac_id = ac_id AND cref_journ_id = ac_journ_id")
      sQuery.Append(" WHERE cref_contact_type = '17'")

      If searchCriteria.ViewCriteriaFractionalProgramID > 0 Then
        sQuery.Append(" AND pgref_prog_id = " + searchCriteria.ViewCriteriaFractionalProgramID.ToString)
      End If

      If inAmodID > 0 Then
        sQuery.Append(" AND ac_amod_id = " + inAmodID.ToString)
      End If

      sQuery.Append("))")
      sQuery.Append(" ORDER BY sumPercent")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_fractional_owner_percent(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal inAmodID As Long) As DataTable</b><br />" + sQuery.ToString

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
        aError = "Error in get_fractional_owner_percent load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_fractional_owner_percent(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal inAmodID As Long) As DataTable " + ex.Message

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

  Public Sub views_display_fractional_model_shares_inuse(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String)

    Dim results_table As New DataTable
    Dim results_table2 As New DataTable

    Dim htmlOut As New StringBuilder
    Dim toggleRowColor As Boolean = False
    Dim percentage As Double = 0.0
    Dim clear_model_link As String = ""

    Try

      clear_model_link = "<a href=""view_template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName + "&selectFractionalProgram=" + searchCriteria.ViewCriteriaFractionalProgramID.ToString + "&amod_id=-1"">Clear Model</a>"

      results_table = get_fractional_model_shares_inuse(searchCriteria)

      htmlOut.Append("<table id=""fractionalModelSharesInuseOuterTable"" width=""100%"" cellpadding=""1"" cellspacing=""0"" class=""module"">")

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""header"">AIRCRAFT IN PROGRAM&nbsp;(" + results_table.Rows.Count.ToString + ")</td></tr>")
          htmlOut.Append("<tr><td valign=""top"" align=""left"">")
          If searchCriteria.ViewCriteriaAmodID > 0 Then
            htmlOut.Append(clear_model_link)
            htmlOut.Append("</td></tr><tr><td valign=""top"" align=""left"">")
          End If

          htmlOut.Append("<table id=""fractionalModelSharesInuseInnerTable"" width=""100%"" cellpadding=""2"" cellspacing=""0""><tr>")
          htmlOut.Append("<td valign=""top"" align=""left"" class=""seperator"" width=""35%""><strong>Model</strong></td>")
          htmlOut.Append("<td valign=""top"" align=""left"" class=""seperator"" width=""30%""><strong>#&nbsp;of&nbsp;AC</strong></td>")
          htmlOut.Append("<td valign=""top"" align=""left"" class=""seperator"" width=""35%""><strong>&nbsp;AC&nbsp;In&nbsp;Use</strong></td></tr>")
          htmlOut.Append("<tr><td colspan=""3"" class=""rightside"" valign=""top"">")
          htmlOut.Append("<table id=""fractionalModelSharesInuseDataTable"" width=""100%"" cellpadding=""4"" cellspacing=""0"">")

          For Each r As DataRow In results_table.Rows
            If Not toggleRowColor Then
              htmlOut.Append("<tr class=""alt_row"">")
              toggleRowColor = True
            Else
              htmlOut.Append("<tr bgcolor=""white"">")
              toggleRowColor = False
            End If

            htmlOut.Append("<td align=""left"" valign=""top"" class=""seperator"" width=""35%"" nowrap=""nowrap"">")
            htmlOut.Append("<a href=""view_template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName + "&selectFractionalProgram=" + searchCriteria.ViewCriteriaFractionalProgramID.ToString + "&amod_id=" + r.Item("amod_id").ToString + """ title=""Click to view " + r.Item("amod_make_name").ToString.Trim + "&nbsp;/&nbsp;" + r.Item("amod_model_name").ToString + " fractional programs."">")
            htmlOut.Append(r.Item("amod_make_name").ToString.Trim + "&nbsp;/&nbsp;" + r.Item("amod_model_name").ToString + "</a></td>")

            htmlOut.Append("<td align=""left"" valign=""top"" class=""seperator"" width=""30%"">" + r.Item("ac_count").ToString + "</td>")

            results_table2 = get_fractional_owner_percent(searchCriteria, CLng(r.Item("amod_id").ToString))

            If Not IsNothing(results_table2) Then
              If results_table2.Rows.Count > 0 Then
                For Each r2 As DataRow In results_table2.Rows

                  percentage = 0
                  If Not IsDBNull(r2("sumPercent")) Then
                    If CDbl(r2.Item("sumPercent").ToString) > 0 Then
                      percentage = CDbl(CDbl(r2.Item("sumPercent").ToString) / 100)
                    End If
                  End If

                  htmlOut.Append("<td align=""left"" valign=""top"" class=""seperator"" width=""35%"">" + percentage.ToString + "</td></tr>")

                Next
              Else
                htmlOut.Append("<td align=""left"" valign=""top"" class=""seperator"" width=""35%"">0</td></tr>")
              End If
            Else
              htmlOut.Append("<td align=""left"" valign=""top"" class=""seperator"" width=""35%"">0</td></tr>")
            End If

            results_table2 = Nothing
            results_table2 = New DataTable

          Next

          htmlOut.Append("</table></td></tr></table></td></tr>")

        Else
          htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""seperator"" style=""padding-left:3px;""><br />No fractional aircraft for your search criteria</td></tr>")
        End If
      Else
        htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""seperator"" style=""padding-left:3px;""><br />No fractional aircraft for your search criteria</td></tr>")
      End If

      htmlOut.Append("</table>")

    Catch ex As Exception

      aError = "Error in views_display_fractional_model_shares_inuse(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

    Finally

    End Try

    'return resulting html string
    out_htmlString = htmlOut.ToString
    htmlOut = Nothing
    results_table = Nothing
    results_table2 = Nothing

  End Sub

  Public Function get_fractional_latest_sales_graph_sold_from_provider(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal bShowSummaryData As Boolean) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      If bShowSummaryData Then
        sQuery.Append("SELECT YEAR(journ_date) AS tyear, MONTH(journ_date) AS tmonth, count(*) AS total_count")
      Else
        sQuery.Append("SELECT journ_id, journ_subcategory_code, journ_date, journ_subject, ac_ser_no_full, ac_reg_no, ac_mfr_year, cref_owner_percent, ac_id, amod_make_name")
      End If

      sQuery.Append(" FROM aircraft WITH(NOLOCK) INNER JOIN aircraft_reference WITH(NOLOCK) ON ac_id = cref_ac_id AND ac_journ_id = cref_journ_id")
      sQuery.Append(" INNER JOIN aircraft_model WITH(NOLOCK) ON ac_amod_id = amod_id")
      sQuery.Append(" INNER JOIN journal WITH(NOLOCK) ON ac_journ_id = journ_id")
      sQuery.Append(" WHERE (journ_subcategory_code LIKE 'FS%') AND (cref_contact_type = '69')")

      sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))

      sQuery.Append(crmWebClient.Constants.cAndClause + "((journ_date >= '" + Month(DateAdd("m", (-1) * searchCriteria.ViewCriteriaTimeSpan, Now())).ToString + "/01/" + Year(DateAdd("m", (-1) * searchCriteria.ViewCriteriaTimeSpan, Now())).ToString + "')")
      sQuery.Append(crmWebClient.Constants.cAndClause + "(journ_date < '" + Now.Month.ToString + "/01/" + Now.Year.ToString + "'))")

      sQuery.Append(" AND (RIGHT(journ_subcategory_code,2) NOT IN ('IT')) AND (LEFT(journ_subcategory_code,4) <> 'CORR')")
      sQuery.Append(" AND (cref_journ_id IN (SELECT DISTINCT cref_journ_id")
      sQuery.Append(" FROM aircraft_reference WITH(NOLOCK) WHERE (cref_contact_type = '69') AND (cref_journ_id > 0)")
      sQuery.Append(" AND (cref_comp_id IN (SELECT DISTINCT pgref_comp_id FROM program_reference WITH(NOLOCK)")

      If searchCriteria.ViewCriteriaFractionalProgramID > 0 Then
        sQuery.Append(" WHERE pgref_prog_id = " + searchCriteria.ViewCriteriaFractionalProgramID.ToString)
        If searchCriteria.ViewCriteriaAmodID > -1 Then
          sQuery.Append(crmWebClient.Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
        ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
          sQuery.Append(crmWebClient.Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
        End If
      ElseIf searchCriteria.ViewCriteriaAmodID > -1 Then
        sQuery.Append(" WHERE amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
        sQuery.Append(" WHERE amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
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

      sQuery.Append(")) ))")





      If bShowSummaryData Then
        sQuery.Append(" GROUP BY YEAR(journ_date), MONTH(journ_date)")
        sQuery.Append(" ORDER BY tyear, tmonth")
      Else
        sQuery.Append(" ORDER BY journ_date DESC")
      End If

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_fractional_latest_sales_graph_sold_from_provider(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
        aError = "Error in get_fractional_latest_sales_graph_sold_from_provider load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_fractional_latest_sales_graph_sold_from_provider(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

  Public Sub views_display_fractional_latest_sales_graph_sold_from_provider(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByRef SOLD_FROM_PROVIDER As DataVisualization.Charting.Chart)

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder
    Dim toggleRowColor As Boolean = False
    Dim high_number As Integer = 0
    Dim low_number As Integer = 0
    Dim starting_point As Integer = 0
    Dim interval_point As Integer = 1

    Try

      SOLD_FROM_PROVIDER.Series.Clear()
      SOLD_FROM_PROVIDER.Series.Add("SHARES")
      SOLD_FROM_PROVIDER.Series("SHARES").ChartType = UI.DataVisualization.Charting.SeriesChartType.SplineArea
      SOLD_FROM_PROVIDER.ChartAreas("ChartArea1").AxisY.Title = "Shares"
      SOLD_FROM_PROVIDER.ChartAreas("ChartArea1").AxisX.Title = "Month"

      SOLD_FROM_PROVIDER.Series("SHARES").Color = Drawing.Color.Blue
      SOLD_FROM_PROVIDER.Series("SHARES").BorderWidth = 1
      SOLD_FROM_PROVIDER.Series("SHARES").MarkerSize = 5
      SOLD_FROM_PROVIDER.Series("SHARES").MarkerStyle = UI.DataVisualization.Charting.MarkerStyle.Circle
      SOLD_FROM_PROVIDER.BorderlineWidth = 10

      SOLD_FROM_PROVIDER.Series("SHARES").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32
      SOLD_FROM_PROVIDER.Width = 260
      SOLD_FROM_PROVIDER.Height = 260

      results_table = get_fractional_latest_sales_graph_sold_from_provider(searchCriteria, True)

      htmlOut.Append("<table id=""salesByProviderGraphOuterTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"" class=""module"">")
      htmlOut.Append("<tr><td valign=""top"" align=""center"" class=""header"">SALES FROM / BY PROVIDER(S) <em>(past " + searchCriteria.ViewCriteriaTimeSpan.ToString + " months)</em></td></tr>")

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          htmlOut.Append("<tr><td valign=""top"" align=""left"">")
          htmlOut.Append("<table id=""salesByProviderGraphInnerTable"" width=""100%"" cellpadding=""4"" cellspacing=""0"">")

          For Each r As DataRow In results_table.Rows

            If Not IsDBNull(r("tyear")) Then
              If Not String.IsNullOrEmpty(r.Item("tyear").ToString.Trim) Then

                If high_number = 0 Or CDbl(r.Item("total_count").ToString) > high_number Then
                  high_number = CDbl(r.Item("total_count").ToString)
                End If

                If low_number = 0 Or CDbl(r.Item("total_count")) < low_number Then
                  low_number = CDbl(r.Item("total_count").ToString)
                End If

                SOLD_FROM_PROVIDER.Series("SHARES").Points.AddXY((r.Item("tmonth").ToString + "-" + r.Item("tyear").ToString), CDbl(r.Item("total_count").ToString))

              End If
            End If

          Next

          If high_number > 100 And high_number <= 150 Then
            high_number = 150
          ElseIf high_number > 50 And high_number <= 100 Then
            high_number = 100
          ElseIf high_number > 20 And high_number <= 50 Then
            high_number = 50
          ElseIf high_number > 10 And high_number <= 20 Then
            high_number = 20
          ElseIf high_number > 5 And high_number <= 10 Then
            high_number = 10
          ElseIf high_number < 5 Then
            high_number = 5
          End If

          SOLD_FROM_PROVIDER.ChartAreas("ChartArea1").AxisY.Maximum = high_number
          SOLD_FROM_PROVIDER.ChartAreas("ChartArea1").AxisY.Minimum = 0

          If high_number >= 10 Then
            SOLD_FROM_PROVIDER.ChartAreas("ChartArea1").AxisY.Interval = (high_number / 10)
          Else
            SOLD_FROM_PROVIDER.ChartAreas("ChartArea1").AxisY.Interval = interval_point
          End If

          SOLD_FROM_PROVIDER.Titles.Add("Shares Sold By Month")
          SOLD_FROM_PROVIDER.ImageType = DataVisualization.Charting.ChartImageType.Jpeg
          SOLD_FROM_PROVIDER.SaveImage(HttpContext.Current.Server.MapPath("TempFiles") + "\" + searchCriteria.ViewCriteriaAmodID.ToString + "_" + searchCriteria.ViewCriteriaTimeSpan.ToString + "_SOLD_FROM_PROVIDER.jpg", DataVisualization.Charting.ChartImageFormat.Jpeg)

          htmlOut.Append("<tr><td valign=""middle"" align=""center""><img src=""TempFiles\" + searchCriteria.ViewCriteriaAmodID.ToString + "_" + searchCriteria.ViewCriteriaTimeSpan.ToString + "_SOLD_FROM_PROVIDER.jpg""></td></tr>")
          htmlOut.Append("</table></td></tr>")

        Else
          htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""seperator"" style=""padding-left:3px;"">No sales by provider(s) for this model</td></tr>")
        End If
      Else
        htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""seperator"" style=""padding-left:3px;"">No sales by provider(s) for this model</td></tr>")
      End If

      htmlOut.Append("</table>")

      ' only get the supporting data table if there is data for the graph
      If results_table.Rows.Count > 0 Then

        htmlOut.Append("<br />") ' place a break between tables

        htmlOut.Append("<table id=""salesByProviderOuterTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"" class=""module"">")
        htmlOut.Append("<tr><td valign=""top"" align=""center"" class=""seperator""><strong>Sales By Provider(s)</strong></td></tr>")

        results_table = Nothing
        results_table = New DataTable
        results_table = get_fractional_latest_sales_graph_sold_from_provider(searchCriteria, False)

        If Not IsNothing(results_table) Then

          If results_table.Rows.Count > 0 Then

            htmlOut.Append("<tr><td colspan=""2"" class=""rightside"" valign=""top"">")
            htmlOut.Append("<div style=""height:300px; overflow: auto;""><p>")
            htmlOut.Append("<table id=""salesByProviderDataTable"" width=""100%"" cellpadding=""4"" cellspacing=""0"">")

            For Each r As DataRow In results_table.Rows
              If Not toggleRowColor Then
                htmlOut.Append("<tr class=""alt_row"">")
                toggleRowColor = True
              Else
                htmlOut.Append("<tr bgcolor=""white"">")
                toggleRowColor = False
              End If

              htmlOut.Append("<td align=""left"" valign=""top"" class=""seperator""><img src=""images/ch_red.jpg"" class=""bullet"" alt=""acid : " + r.Item("ac_id").ToString + """ /></td>")
              htmlOut.Append("<td align=""left"" valign=""top"" class=""seperator"">" + r.Item("journ_date").ToString.Trim + " | <b>" + r.Item("journ_subject").ToString.Trim)

              If Not IsDBNull(r.Item("cref_owner_percent")) Then
                If CDbl(r.Item("cref_owner_percent").ToString) > 0 Then
                  htmlOut.Append(crmWebClient.Constants.cSingleSpace + "(" + r.Item("cref_owner_percent").ToString + "%)</b>")
                End If
              End If

              htmlOut.Append("<br /><a class=""underline"" target=""_blank"" href=""DisplayAircraftDetail.aspx?acid=" + r.Item("ac_id").ToString + """ title=""Display Aircraft Details"">")
              htmlOut.Append("<strong>Serial# :</strong> " + r.Item("ac_ser_no_full").ToString + "</a> <strong>Reg# :</strong> " + r.Item("ac_reg_no").ToString + " <strong>MFR Year :</strong> " + r.Item("ac_mfr_year").ToString + "</td></tr>")

            Next

            htmlOut.Append("</table></p></div></td></tr>")

          Else
            htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""seperator"" style=""padding-left:3px;"">No sales by provider(s) for this model</td></tr>")
          End If
        Else
          htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""seperator"" style=""padding-left:3px;"">No sales by provider(s) for this model</td></tr>")
        End If

        htmlOut.Append("</table>")

      End If

    Catch ex As Exception

      aError = "Error in views_display_fractional_latest_sales_graph_sold_from_provider(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByRef SOLD_FROM_PROVIDER As DataVisualization.Charting.Chart) " + ex.Message

    Finally

    End Try

    'return resulting html string
    out_htmlString = htmlOut.ToString
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Function get_fractional_latest_sales_graph_sold_to_provider(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal bShowSummaryData As Boolean) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      If bShowSummaryData Then
        sQuery.Append("SELECT YEAR(journ_date) AS tyear, MONTH(journ_date) AS tmonth, count(*) AS total_count")
      Else
        sQuery.Append("SELECT journ_id, journ_subcategory_code, journ_date, journ_subject, ac_ser_no_full, ac_reg_no, ac_mfr_year, cref_owner_percent, ac_id, amod_make_name")
      End If

      sQuery.Append(" FROM aircraft WITH(NOLOCK) INNER JOIN aircraft_reference WITH(NOLOCK) ON ac_id = cref_ac_id AND ac_journ_id = cref_journ_id")
      sQuery.Append(" INNER JOIN aircraft_model WITH(NOLOCK) ON ac_amod_id = amod_id")
      sQuery.Append(" INNER JOIN journal WITH(NOLOCK) ON ac_journ_id = journ_id")
      sQuery.Append(" WHERE (journ_subcategory_code LIKE 'FS%') AND (cref_contact_type = '70')")

      sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))

      sQuery.Append(crmWebClient.Constants.cAndClause + "((journ_date >= '" + Month(DateAdd("m", (-1) * searchCriteria.ViewCriteriaTimeSpan, Now())).ToString + "/01/" + Year(DateAdd("m", (-1) * searchCriteria.ViewCriteriaTimeSpan, Now())).ToString + "')")
      sQuery.Append(crmWebClient.Constants.cAndClause + "(journ_date < '" + Now.Month.ToString + "/01/" + Now.Year.ToString + "'))")

      sQuery.Append(" AND (RIGHT(journ_subcategory_code,2) NOT IN ('IT')) AND (LEFT(journ_subcategory_code,4) <> 'CORR')")
      sQuery.Append(" AND (cref_journ_id IN (SELECT DISTINCT cref_journ_id")
      sQuery.Append(" FROM aircraft_reference WITH(NOLOCK) WHERE (cref_contact_type = '70') AND (cref_journ_id > 0)")
      sQuery.Append(" AND (cref_comp_id IN (SELECT DISTINCT pgref_comp_id FROM program_reference WITH(NOLOCK)")

      If searchCriteria.ViewCriteriaFractionalProgramID > 0 Then
        sQuery.Append(" WHERE pgref_prog_id = " + searchCriteria.ViewCriteriaFractionalProgramID.ToString)
        If searchCriteria.ViewCriteriaAmodID > -1 Then
          sQuery.Append(crmWebClient.Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
        ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
          sQuery.Append(crmWebClient.Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
        End If
      ElseIf searchCriteria.ViewCriteriaAmodID > -1 Then
        sQuery.Append(" WHERE amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
        sQuery.Append(" WHERE amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
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



      sQuery.Append(")) ))")

      If bShowSummaryData Then
        sQuery.Append(" GROUP BY YEAR(journ_date), MONTH(journ_date)")
        sQuery.Append(" ORDER BY tyear, tmonth")
      Else
        sQuery.Append(" ORDER BY journ_date DESC")
      End If

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_fractional_latest_sales_graph_sold_to_provider(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
        aError = "Error in get_fractional_latest_sales_graph_sold_to_provider load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_fractional_latest_sales_graph_sold_to_provider(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

  Public Sub views_display_fractional_latest_sales_graph_sold_to_provider(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByRef SOLD_TO_PROVIDER As DataVisualization.Charting.Chart)

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder
    Dim toggleRowColor As Boolean = False
    Dim high_number As Integer = 0
    Dim low_number As Integer = 0
    Dim starting_point As Integer = 0
    Dim interval_point As Integer = 1

    Try

      SOLD_TO_PROVIDER.Series.Clear()
      SOLD_TO_PROVIDER.Series.Add("SHARES")
      SOLD_TO_PROVIDER.Series("SHARES").ChartType = UI.DataVisualization.Charting.SeriesChartType.SplineArea
      SOLD_TO_PROVIDER.ChartAreas("ChartArea1").AxisY.Title = "Shares"
      SOLD_TO_PROVIDER.ChartAreas("ChartArea1").AxisX.Title = "Month"

      SOLD_TO_PROVIDER.Series("SHARES").Color = Drawing.Color.Blue
      SOLD_TO_PROVIDER.Series("SHARES").BorderWidth = 1
      SOLD_TO_PROVIDER.Series("SHARES").MarkerSize = 5
      SOLD_TO_PROVIDER.Series("SHARES").MarkerStyle = UI.DataVisualization.Charting.MarkerStyle.Circle
      SOLD_TO_PROVIDER.BorderlineWidth = 10

      SOLD_TO_PROVIDER.Series("SHARES").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32
      SOLD_TO_PROVIDER.Width = 260
      SOLD_TO_PROVIDER.Height = 260

      results_table = get_fractional_latest_sales_graph_sold_to_provider(searchCriteria, True)

      htmlOut.Append("<table id=""salesBackToProviderGraphOuterTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"" class=""module"">")
      htmlOut.Append("<tr><td valign=""top"" align=""center"" class=""header"">SALES BACK TO PROVIDER(S) <em>(past " + searchCriteria.ViewCriteriaTimeSpan.ToString + " months)</em></td></tr>")

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          htmlOut.Append("<tr><td valign=""top"" align=""left"">")
          htmlOut.Append("<table id=""salesBackToProviderGraphInnerTable"" width=""100%"" cellpadding=""4"" cellspacing=""0"">")

          For Each r As DataRow In results_table.Rows

            If Not IsDBNull(r("tyear")) Then
              If Not String.IsNullOrEmpty(r.Item("tyear").ToString.Trim) Then

                If high_number = 0 Or CDbl(r.Item("total_count").ToString) > high_number Then
                  high_number = CDbl(r.Item("total_count").ToString)
                End If

                If low_number = 0 Or CDbl(r.Item("total_count")) < low_number Then
                  low_number = CDbl(r.Item("total_count").ToString)
                End If

                SOLD_TO_PROVIDER.Series("SHARES").Points.AddXY((r.Item("tmonth").ToString + "-" + r.Item("tyear").ToString), CDbl(r.Item("total_count").ToString))

              End If
            End If

          Next

          If high_number > 100 And high_number <= 150 Then
            high_number = 150
          ElseIf high_number > 50 And high_number <= 100 Then
            high_number = 100
          ElseIf high_number > 20 And high_number <= 50 Then
            high_number = 50
          ElseIf high_number > 10 And high_number <= 20 Then
            high_number = 20
          ElseIf high_number > 5 And high_number <= 10 Then
            high_number = 10
          ElseIf high_number < 5 Then
            high_number = 5
          End If

          SOLD_TO_PROVIDER.ChartAreas("ChartArea1").AxisY.Maximum = high_number
          SOLD_TO_PROVIDER.ChartAreas("ChartArea1").AxisY.Minimum = 0

          If high_number >= 10 Then
            SOLD_TO_PROVIDER.ChartAreas("ChartArea1").AxisY.Interval = (high_number / 10)
          Else
            SOLD_TO_PROVIDER.ChartAreas("ChartArea1").AxisY.Interval = interval_point
          End If

          SOLD_TO_PROVIDER.Titles.Add("Sold To Provider By Month")
          SOLD_TO_PROVIDER.ImageType = DataVisualization.Charting.ChartImageType.Jpeg
          SOLD_TO_PROVIDER.SaveImage(HttpContext.Current.Server.MapPath("TempFiles") + "\" + searchCriteria.ViewCriteriaAmodID.ToString + "_" + searchCriteria.ViewCriteriaTimeSpan.ToString + "_SOLD_TO_PROVIDER.jpg", DataVisualization.Charting.ChartImageFormat.Jpeg)

          htmlOut.Append("<tr><td valign=""middle"" align=""center""><img src=""TempFiles\" + searchCriteria.ViewCriteriaAmodID.ToString + "_" + searchCriteria.ViewCriteriaTimeSpan.ToString + "_SOLD_TO_PROVIDER.jpg""></td></tr>")
          htmlOut.Append("</table></td></tr>")

        Else
          htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""seperator"" style=""padding-left:3px;"">No sales back to provider(s) for this model</td></tr>")
        End If
      Else
        htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""seperator"" style=""padding-left:3px;"">No sales back to provider(s) for this model</td></tr>")
      End If

      htmlOut.Append("</table>")

      ' only get the supporting data table if there is data for the graph
      If results_table.Rows.Count > 0 Then

        htmlOut.Append("<br />") ' place a break between tables

        htmlOut.Append("<table id=""salesByProviderOuterTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"" class=""module"">")
        htmlOut.Append("<tr><td valign=""top"" align=""center"" class=""seperator""><strong>Sales Back To Provider(s)</strong></td></tr>")

        results_table = Nothing
        results_table = New DataTable
        results_table = get_fractional_latest_sales_graph_sold_to_provider(searchCriteria, False)

        If Not IsNothing(results_table) Then

          If results_table.Rows.Count > 0 Then

            htmlOut.Append("<tr><td colspan=""2"" class=""rightside"" valign=""top"">")
            htmlOut.Append("<div style=""height:300px; overflow: auto;""><p>")
            htmlOut.Append("<table id=""salesByProviderDataTable"" width=""100%"" cellpadding=""4"" cellspacing=""0"">")

            For Each r As DataRow In results_table.Rows

              If Not toggleRowColor Then
                htmlOut.Append("<tr class=""alt_row"">")
                toggleRowColor = True
              Else
                htmlOut.Append("<tr bgcolor=""white"">")
                toggleRowColor = False
              End If

              htmlOut.Append("<td align=""left"" valign=""top"" class=""seperator""><img src=""images/ch_red.jpg"" class=""bullet"" alt=""acid : " + r.Item("ac_id").ToString + """ /></td>")
              htmlOut.Append("<td align=""left"" valign=""top"" class=""seperator"">" + r.Item("journ_date").ToString.Trim + " | <b>" + r.Item("journ_subject").ToString.Trim)

              If Not IsDBNull(r.Item("cref_owner_percent")) Then
                If CDbl(r.Item("cref_owner_percent").ToString) > 0 Then
                  htmlOut.Append(crmWebClient.Constants.cSingleSpace + "(" + r.Item("cref_owner_percent").ToString + "%)</b>")
                End If
              End If

              htmlOut.Append("<br /><a class=""underline"" target=""_blank"" href=""DisplayAircraftDetail.aspx?acid=" + r.Item("ac_id").ToString + """ title=""Display Aircraft Details"">")
              htmlOut.Append("<strong>Serial# :</strong> " + r.Item("ac_ser_no_full").ToString + "</a> <strong>Reg# :</strong> " + r.Item("ac_reg_no").ToString + " <strong>MFR Year :</strong> " + r.Item("ac_mfr_year").ToString + "</td></tr>")

            Next

            htmlOut.Append("</table></p></div></td></tr>")

          Else
            htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""seperator"" style=""padding-left:3px;"">No sales back to provider(s) for this model</td></tr>")
          End If
        Else
          htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""seperator"" style=""padding-left:3px;"">No sales back to provider(s) for this model</td></tr>")
        End If

        htmlOut.Append("</table>")

      End If

    Catch ex As Exception

      aError = "Error in views_display_fractional_latest_sales_graph_sold_to_provider(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByRef SOLD_TO_PROVIDER As DataVisualization.Charting.Chart) " + ex.Message

    Finally

    End Try

    'return resulting html string
    out_htmlString = htmlOut.ToString
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Function get_fractional_program_name(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable
    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      ' build the query to get the fractional_program_name
      sQuery.Append("SELECT DISTINCT prog_id, prog_name FROM Aircraft_Programs WITH(NOLOCK)")
      sQuery.Append(" INNER JOIN program_reference WITH(NOLOCK) ON pgref_prog_id = prog_id")
      sQuery.Append(" WHERE prog_active_flag = 'Y' AND prog_id = " + searchCriteria.ViewCriteriaFractionalProgramID.ToString)

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_fractional_program_name(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = adminConnectString
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
        aError = "Error in get_fractional_program_name load datatable" + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_fractional_program_name(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable" + ex.Message

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

  Public Function get_fractional_program_and_models(ByVal bGetProgram As Boolean) As DataTable
    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      If bGetProgram Then

        sQuery.Append("SELECT DISTINCT prog_id, prog_comp_id, prog_name")
        sQuery.Append(" FROM Aircraft_Programs WITH(NOLOCK) INNER JOIN program_reference WITH(NOLOCK) ON pgref_prog_id = prog_id")
        sQuery.Append(" WHERE prog_active_flag = 'Y'")

        sQuery.Append(" AND (EXISTS (SELECT NULL FROM aircraft WITH(NOLOCK)")
        sQuery.Append(" INNER JOIN aircraft_model WITH(NOLOCK) ON ac_amod_id = amod_id")
        sQuery.Append(" WHERE (ac_id IS NOT NULL) AND (ac_journ_id = 0) AND (ac_ownership_type='F')")

        sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))

        sQuery.Append(" AND (ac_id IN (SELECT DISTINCT cref_ac_id FROM aircraft_reference WITH(NOLOCK)")
        sQuery.Append(" WHERE (cref_contact_type='17') AND (cref_journ_id = 0) AND (cref_comp_id IN (SELECT DISTINCT pgref_comp_id FROM program_reference")
        sQuery.Append(" WITH(NOLOCK) WHERE pgref_prog_id = prog_id))))))")

        sQuery.Append(" GROUP BY prog_id, prog_name, prog_comp_id")
        sQuery.Append(" ORDER BY prog_name")

      Else

        sQuery.Append("SELECT DISTINCT amod_id, amod_make_name, amod_model_name")
        sQuery.Append(" FROM aircraft WITH(NOLOCK) INNER JOIN aircraft_model WITH(NOLOCK) ON ac_amod_id = amod_id")
        sQuery.Append(" WHERE (ac_journ_id = 0) AND (ac_ownership_type='F')")

        sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))

        sQuery.Append(" AND (ac_id IN (SELECT DISTINCT cref_ac_id FROM aircraft_reference WITH(NOLOCK)")
        sQuery.Append(" WHERE (cref_contact_type='17') AND (cref_journ_id = 0)")
        sQuery.Append(" AND (cref_comp_id IN (SELECT DISTINCT pgref_comp_id FROM program_reference WITH(NOLOCK)")
        sQuery.Append(")) ))")
        sQuery.Append(" GROUP BY amod_make_name, amod_model_name, amod_id")
        sQuery.Append(" ORDER BY amod_make_name, amod_model_name")

      End If

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_fractional_program_and_models(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal bGetProgram As Boolean) As DataTable</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = adminConnectString
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
        aError = "Error in get_fractional_program_and_models load datatable" + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_fractional_program_and_models(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal bGetProgram As Boolean) As DataTable" + ex.Message

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

  Public Sub views_fill_fractional_dropdowns(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef maxWidth As Long, ByRef fractProgram As ListBox, ByRef fractModel As ListBox)

    Dim results_table As New DataTable

    Try

      fractProgram.Items.Clear()
      fractModel.Items.Clear()

      If searchCriteria.ViewCriteriaFractionalProgramID > 0 Then

        results_table = get_fractional_program_name(searchCriteria)

        If Not IsNothing(results_table) Then

          If results_table.Rows.Count > 0 Then

            For Each r As DataRow In results_table.Rows

              If Not IsDBNull(r.Item("prog_name")) And Not String.IsNullOrEmpty(r.Item("prog_name").ToString.Trim) Then
                searchCriteria.ViewCriteriaFractionalProgramName = r.Item("prog_name").ToString.Trim
              End If

            Next
          End If
        End If

      End If

      results_table = Nothing
      results_table = New DataTable

      fractProgram.Items.Add(New ListItem("All", ""))
      fractModel.Items.Add(New ListItem("All", ""))

      results_table = get_fractional_program_and_models(True)
      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          For Each r As DataRow In results_table.Rows

            If Not IsDBNull(r.Item("prog_name")) And Not String.IsNullOrEmpty(r.Item("prog_name").ToString.Trim) Then
              If (r.Item("prog_name").ToString.Length * crmWebClient.Constants._STARTCHARWIDTH) > maxWidth Then
                maxWidth = (r.Item("prog_name").ToString.Length * crmWebClient.Constants._STARTCHARWIDTH)
              End If

              fractProgram.Items.Add(New ListItem(r.Item("prog_name").ToString, r.Item("prog_id").ToString))

              If CLng(r.Item("prog_id").ToString) = searchCriteria.ViewCriteriaFractionalProgramID Then
                fractProgram.SelectedValue = searchCriteria.ViewCriteriaFractionalProgramID.ToString
              End If

            End If

          Next
        End If
      End If

      If searchCriteria.ViewCriteriaFractionalProgramID = 0 Then
        fractProgram.SelectedValue = ""
      End If

      fractProgram.Width = (maxWidth)

      results_table = Nothing
      results_table = New DataTable
      Dim sTmpMakeModelName As String = ""
      Dim bFoundSelectedModel As Boolean = False

      results_table = get_fractional_program_and_models(False)
      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          For Each r As DataRow In results_table.Rows

            If Not IsDBNull(r.Item("amod_id")) And Not String.IsNullOrEmpty(r.Item("amod_id").ToString.Trim) Then
              sTmpMakeModelName = r.Item("amod_make_name").ToString + " / " + r.Item("amod_model_name").ToString

              If (sTmpMakeModelName.Length * crmWebClient.Constants._STARTCHARWIDTH) > maxWidth Then
                maxWidth = (sTmpMakeModelName.Length * crmWebClient.Constants._STARTCHARWIDTH)
              End If

              fractModel.Items.Add(New ListItem(sTmpMakeModelName, r.Item("amod_id").ToString))

              If CLng(r.Item("amod_id").ToString) = searchCriteria.ViewCriteriaAmodID Then
                fractModel.SelectedValue = searchCriteria.ViewCriteriaAmodID.ToString
                bFoundSelectedModel = True
              End If

            End If

          Next
        End If
      End If

      If searchCriteria.ViewCriteriaAmodID = -1 Then
        fractModel.SelectedValue = ""
      Else
        If Not bFoundSelectedModel Then
          fractModel.SelectedValue = ""
        End If
      End If

      fractModel.Width = (maxWidth)

    Catch ex As Exception

      aError = "Error in views_fill_fractional_dropdowns(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef maxWidth As Long, ByRef fractProgram As ListBox, ByRef fractModel As ListBox) " + ex.Message

    Finally

    End Try

    results_table = Nothing

  End Sub

  Public Sub views_display_fractional_models(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String)

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder
    Dim toggleRowColor As Boolean = False

    Try

      htmlOut.Append("<table id='fractionalModelsOuterTable' width='100%' cellpadding='1' cellspacing='0' class='module'>")

      results_table = get_fractional_model_shares_inuse(searchCriteria)

      If Not IsNothing(results_table) Then

        htmlOut.Append("<tr><td valign='top' align='left' class='header'>AIRCRAFT MODELS&nbsp;<em>(" + results_table.Rows.Count.ToString + ") most popular models</em></td></tr>")

        If results_table.Rows.Count > 0 Then

          htmlOut.Append("<td valign='top' align='left'>")
          htmlOut.Append("<table id='fractionalModelsInnerTable' width='100%' cellpadding='1' cellspacing='0'>")

          htmlOut.Append("<tr><td valign='top' align='left' class='seperator' width='80%'><strong>Model</strong></td>")
          htmlOut.Append("<td valign='top' align='right' class='seperator' width='20%' style='padding-right:5px;'><strong>#&nbsp;of&nbsp;AC</strong></td></tr>")
          htmlOut.Append("<tr><td colspan='2' class='rightside' valign='top'>")

          If results_table.Rows.Count > 30 Then
            htmlOut.Append("<div valign=""top"" style='height:1024px; overflow: auto;'><p>")
          End If

          htmlOut.Append("<table id='fractionalModelsDataTable' width='100%' cellpadding='4' cellspacing='0'>")

          For Each r As DataRow In results_table.Rows

            If Not toggleRowColor Then
              htmlOut.Append("<tr class='alt_row'>")
              toggleRowColor = True
            Else
              htmlOut.Append("<tr bgcolor='white'>")
              toggleRowColor = False
            End If

            htmlOut.Append("<td align='left' valign='top' class='seperator' width='80%'>")
            htmlOut.Append("<a href='view_template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName + "&selectFractionalProgram=" + searchCriteria.ViewCriteriaFractionalProgramID.ToString + "&amod_id=" + r.Item("amod_id").ToString + "'")
            htmlOut.Append(" title='Click to view fractional programs by " + r.Item("amod_make_name").ToString + " " + r.Item("amod_model_name").ToString + "'>" + r.Item("amod_make_name").ToString + "&nbsp;/&nbsp;" + r.Item("amod_model_name").ToString + "</a></td>")
            htmlOut.Append("<td align='right' valign='top' class='seperator' width='20%' style='padding-right:15px;'>" + r.Item("ac_count").ToString + "</td></tr>")
          Next

          htmlOut.Append("</table>")
          If results_table.Rows.Count > 30 Then
            htmlOut.Append("</p></div>")
          End If
          htmlOut.Append("</td></tr></table></td></tr>")

        Else
          htmlOut.Append("<tr><td valign='top' align='left' class='seperator' style='padding-left:3px;'><br />No fractional models for your search criteria</td></tr>")
        End If
      Else
        htmlOut.Append("<tr><td valign='top' align='left' class='seperator' style='padding-left:3px;'><br />No fractional models for your search criteria</td></tr>")
      End If

      htmlOut.Append("</table>")
    Catch ex As Exception

      aError = "Error in views_display_fractional_models(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

    Finally

    End Try

    'return resulting html string
    out_htmlString = htmlOut.ToString
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Function get_fractions_expired_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT DISTINCT YEAR(cref_fraction_expires_date) as yrExpired, count(*) as expired_count")
      sQuery.Append(" FROM company WITH(NOLOCK) INNER JOIN aircraft_reference WITH(NOLOCK) ON comp_id = cref_comp_id AND comp_journ_id = cref_journ_id AND comp_journ_id = 0")
      sQuery.Append(" WHERE cref_contact_type = '97' AND cref_fraction_expires_date <= GETDATE() AND comp_active_flag = 'Y' AND comp_hide_flag = 'N' AND (cref_ac_id IN (")
      sQuery.Append(" SELECT cref_ac_id FROM aircraft_reference WITH(NOLOCK) INNER JOIN program_reference WITH(NOLOCK) ON pgref_comp_id = cref_comp_id AND cref_journ_id = 0")
      sQuery.Append(" INNER JOIN aircraft WITH(NOLOCK) ON cref_ac_id = ac_id AND cref_journ_id = ac_journ_id")
      sQuery.Append(" INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id")
      sQuery.Append(" WHERE cref_contact_type = '17'")

      If searchCriteria.ViewCriteriaFractionalProgramID > 0 Then
        sQuery.Append(crmWebClient.Constants.cAndClause + "pgref_prog_id = " + searchCriteria.ViewCriteriaFractionalProgramID.ToString)
      End If

      If searchCriteria.ViewCriteriaAmodID > -1 Then
        sQuery.Append(crmWebClient.Constants.cAndClause + "ac_amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
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



      sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))

      sQuery.Append("))")
      sQuery.Append(" GROUP BY YEAR(cref_fraction_expires_date)")
      sQuery.Append(" ORDER BY YEAR(cref_fraction_expires_date)")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_fractions_expired_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
        aError = "Error in get_fractions_expired_info load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_fractions_expired_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

  Public Sub views_display_fractions_expired(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String)

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder
    Dim toggleRowColor As Boolean = False

    Try

      results_table = get_fractions_expired_info(searchCriteria)

      htmlOut.Append("<table id=""fractionsExpiredOuterTable"" width=""100%"" cellpadding=""0"" cellspacing=""0"" class=""module"">")
      htmlOut.Append("<tr><td valign=""top"" align=""center"" class=""header"">EXPIRED AGREEMENTS</td></tr>")

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          htmlOut.Append("<tr><td valign=""top"" align=""left"">")

          htmlOut.Append("<table id=""fractionsExpiredInnerTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"">")
          htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""seperator"" width=""80%""><strong>Year</strong></td>")
          htmlOut.Append("<td valign=""top"" align=""left"" class=""seperator"" width=""20%""><strong>#&nbsp;Expired</strong></td></tr>")

          htmlOut.Append("<tr><td colspan=""2"" class=""rightside"" valign=""top"">")
          htmlOut.Append("<div style=""height: 250px; overflow: auto;""><p>")

          htmlOut.Append("<table id=""fractionsExpiredDataTable"" width=""100%"" cellpadding=""4"" cellspacing=""0"">")

          For Each r As DataRow In results_table.Rows

            If Not toggleRowColor Then
              htmlOut.Append("<tr class=""alt_row"">")
              toggleRowColor = True
            Else
              htmlOut.Append("<tr bgcolor=""white"">")
              toggleRowColor = False
            End If

            htmlOut.Append("<td align=""left"" valign=""top"" class=""seperator"" width=""80%"">")
            htmlOut.Append("<a href=""fractionalShareholderList.aspx?clearRS=true&AmodID=" + searchCriteria.ViewCriteriaAmodID.ToString + "&ProgramID=" + searchCriteria.ViewCriteriaFractionalProgramID.ToString + "&expireYear=" + r.Item("yrExpired").ToString + "&expireFlag=N"" title=""Click to View Expired Fractional Shareholders for year " + r.Item("yrExpired").ToString + """ target=""_blank"">" + r.Item("yrExpired").ToString + "</a></td>")
            htmlOut.Append("<td align=""right"" valign=""top"" class=""seperator"" width=""20%"" style=""padding-right:15px;"">" + r.Item("expired_count").ToString + "</td></tr>")

          Next

          htmlOut.Append("</table></p></div></td></tr></table></td></tr>")

        Else
          htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""seperator"" style=""padding-left:3px;"">No data matches for your search criteria</td></tr>")
        End If
      Else
        htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""seperator"" style=""padding-left:3px;"">No data matches for your search criteria</td></tr>")
      End If

      htmlOut.Append("</table>")

    Catch ex As Exception

      aError = "Error in views_display_fractions_expired(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

    Finally

    End Try

    'return resulting html string
    out_htmlString = htmlOut.ToString
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Function get_fractions_to_expire_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT DISTINCT YEAR(cref_fraction_expires_date) as yrExpire, count(*) as expire_count")
      sQuery.Append(" FROM company WITH(NOLOCK) INNER JOIN aircraft_reference WITH(NOLOCK) ON comp_id = cref_comp_id AND comp_journ_id = cref_journ_id AND comp_journ_id = 0")
      sQuery.Append(" WHERE cref_contact_type = '97' AND cref_fraction_expires_date >= GETDATE() AND comp_active_flag = 'Y' AND comp_hide_flag = 'N' AND (cref_ac_id IN (")
      sQuery.Append(" SELECT cref_ac_id FROM aircraft_reference WITH(NOLOCK) INNER JOIN program_reference WITH(NOLOCK) ON pgref_comp_id = cref_comp_id AND cref_journ_id = 0")
      sQuery.Append(" INNER JOIN aircraft WITH(NOLOCK) ON cref_ac_id = ac_id AND cref_journ_id = ac_journ_id")
      sQuery.Append(" INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id")
      sQuery.Append(" WHERE cref_contact_type = '17'")

      If searchCriteria.ViewCriteriaFractionalProgramID > 0 Then
        sQuery.Append(crmWebClient.Constants.cAndClause + "pgref_prog_id = " + searchCriteria.ViewCriteriaFractionalProgramID.ToString)
      End If

      If searchCriteria.ViewCriteriaAmodID > -1 Then
        sQuery.Append(crmWebClient.Constants.cAndClause + "ac_amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
      End If

      sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))



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



      sQuery.Append("))")
      sQuery.Append(" GROUP BY YEAR(cref_fraction_expires_date)")
      sQuery.Append(" ORDER BY YEAR(cref_fraction_expires_date)")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_fractions_to_expire_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
        aError = "Error in get_fractions_to_expire_info load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_fractions_to_expire_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

  Public Sub views_display_fractions_to_expire(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String)

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder
    Dim toggleRowColor As Boolean = False

    Try

      results_table = get_fractions_to_expire_info(searchCriteria)

      htmlOut.Append("<table id=""fractionsExpiringOuterTable"" width=""100%"" cellpadding=""0"" cellspacing=""0"" class=""module"">")
      htmlOut.Append("<tr><td valign=""top"" align=""center"" class=""header"">EXPIRING AGREEMENTS</td></tr>")

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          htmlOut.Append("<tr><td valign=""top"" align=""left"">")

          htmlOut.Append("<table id=""fractionsExpiringInnerTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"">")
          htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""seperator"" width=""80%""><strong>Year</strong></td>")
          htmlOut.Append("<td valign=""top"" align=""left"" class=""seperator"" width=""20%""><strong>#&nbsp;Expiring</strong></td></tr>")

          htmlOut.Append("<tr><td colspan=""2"" class=""rightside"" valign=""top"">")
          htmlOut.Append("<div style=""height: 250px; overflow: auto;""><p>")

          htmlOut.Append("<table id=""fractionsExpiringDataTable"" width=""100%"" cellpadding=""4"" cellspacing=""0"">")

          For Each r As DataRow In results_table.Rows

            If Not toggleRowColor Then
              htmlOut.Append("<tr class=""alt_row"">")
              toggleRowColor = True
            Else
              htmlOut.Append("<tr bgcolor=""white"">")
              toggleRowColor = False
            End If

            htmlOut.Append("<td align=""left"" valign=""top"" class=""seperator"" width=""80%"">")
            htmlOut.Append("<a href=""fractionalShareholderList.aspx?clearRS=true&AmodID=" + searchCriteria.ViewCriteriaAmodID.ToString + "&ProgramID=" + searchCriteria.ViewCriteriaFractionalProgramID.ToString + "&expireYear=" + r.Item("yrExpire").ToString + "&expireFlag=Y"" title=""Click to View Expiring Fractional Shareholders for year " + r.Item("yrExpire").ToString + """ target=""_blank"">" + r.Item("yrExpire").ToString + "</a></td>")
            htmlOut.Append("<td align=""right"" valign=""top"" class=""seperator"" width=""20%"" style=""padding-right:15px;"">" + r.Item("expire_count").ToString + "</td></tr>")

          Next

          htmlOut.Append("</table></p></div></td></tr></table></td></tr>")

        Else
          htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""seperator"" style=""padding-left:3px;"">No data matches for your search criteria</td></tr>")
        End If
      Else
        htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""seperator"" style=""padding-left:3px;"">No data matches for your search criteria</td></tr>")
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

#End Region

End Class
