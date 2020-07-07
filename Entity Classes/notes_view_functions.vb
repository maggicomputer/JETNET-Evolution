' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/Entity Classes/notes_view_functions.vb $
'$$Author: Matt $
'$$Date: 7/24/19 4:31p $
'$$Modtime: 7/24/19 1:22p $
'$$Revision: 3 $
'$$Workfile: notes_view_functions.vb $
'
' ********************************************************************************

<System.Serializable()> Public Class notes_view_functions

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

  Public Function get_notes_view_all_aircraft_server_notes(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
    Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
    Dim MySqlReader As MySql.Data.MySqlClient.MySqlDataReader = Nothing
    Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing

    Dim sSeperator As String = ""

    Try

      sQuery.Append("SELECT lnote_id, lnote_jetnet_ac_id, lnote_jetnet_comp_id, lnote_client_ac_id, lnote_client_comp_id, lnote_jetnet_contact_id, lnote_client_contact_id, lnote_note,")
      sQuery.Append(" lnote_entry_date, lnote_action_date, lnote_user_login, lnote_user_name, lnote_notecat_key, lnote_status, lnote_schedule_start_date, lnote_schedule_end_date,")
      sQuery.Append(" lnote_user_id, lnote_clipri_ID, lnote_document_flag, lnote_jetnet_amod_id, lnote_client_amod_id")
      sQuery.Append(" FROM local_notes")
      sQuery.Append(" WHERE (lnote_status = 'A')")

      If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaNoteTextValue.Trim) Then
        sQuery.Append(Constants.cAndClause + "(lnote_note LIKE '%" + searchCriteria.ViewCriteriaNoteTextValue.Trim + "%')")
      End If

      If searchCriteria.ViewCriteriaNoteUserID > 0 Then
        sQuery.Append(Constants.cAndClause + "(lnote_user_id = " + searchCriteria.ViewCriteriaNoteUserID.ToString + ")")
      End If

      If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaNoteStartDate) And Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaNoteEndDate) Then
        sQuery.Append(Constants.cAndClause + "(lnote_entry_date > '" + Format(CDate(searchCriteria.ViewCriteriaNoteStartDate), "yyyy-MM-dd H:mm:ss").Trim + "' AND lnote_entry_date <= '" + Format(CDate(searchCriteria.ViewCriteriaNoteEndDate), "yyyy-MM-dd H:mm:ss").Trim + "')")
      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaNoteStartDate) And String.IsNullOrEmpty(searchCriteria.ViewCriteriaNoteEndDate) Then
        sQuery.Append(Constants.cAndClause + "(lnote_entry_date > '" + Format(CDate(searchCriteria.ViewCriteriaNoteStartDate), "yyyy-MM-dd H:mm:ss").Trim + "')")
      ElseIf String.IsNullOrEmpty(searchCriteria.ViewCriteriaNoteStartDate) And Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaNoteEndDate) Then
        sQuery.Append(Constants.cAndClause + "(lnote_entry_date <= '" + Format(CDate(searchCriteria.ViewCriteriaNoteEndDate), "yyyy-MM-dd H:mm:ss").Trim + "')")
      Else

        If String.IsNullOrEmpty(searchCriteria.ViewCriteriaNoteACSearchTextValue.Trim) Then  ' get previous 90 days of notes data
          sQuery.Append(Constants.cAndClause + "(lnote_entry_date > '" + Format(DateAdd(DateInterval.Month, -3, Now()), "yyyy-MM-dd H:mm:ss").Trim + "')")
          searchCriteria.ViewCriteriaNoteStartDate = DateAdd(DateInterval.Month, -3, Now()).ToShortDateString
        End If

      End If

      Select Case searchCriteria.ViewCriteriaNoteACSearchField
        Case eNotesACSearchTypes.AIRCRAFT_ID

          If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaNoteACSearchTextValue.Trim) Then
            If IsNumeric(searchCriteria.ViewCriteriaNoteACSearchTextValue.Trim) Then
              sQuery.Append(Constants.cAndClause + "(lnote_jetnet_ac_id = " + searchCriteria.ViewCriteriaNoteACSearchTextValue.Trim + ")")

              searchCriteria.ViewCriteriaAircraftID = CLng(searchCriteria.ViewCriteriaNoteACSearchTextValue.Trim)
            End If
          End If

      End Select

      If searchCriteria.ViewCriteriaAircraftID = 0 Then

        Dim tmpStr As String = ""

        If Not IsNothing(searchCriteria.ViewCriteriaAmodIDArray) Then

          ' flatten out amodID array ...
          For x As Integer = 0 To UBound(searchCriteria.ViewCriteriaAmodIDArray)
            If String.IsNullOrEmpty(tmpStr) Then
              tmpStr = searchCriteria.ViewCriteriaAmodIDArray(x)
            Else
              tmpStr += Constants.cCommaDelim + searchCriteria.ViewCriteriaAmodIDArray(x)
            End If
          Next

          sQuery.Append(Constants.cAndClause + "lnote_jetnet_amod_id IN (" + tmpStr.Trim + ")")
        ElseIf searchCriteria.ViewCriteriaAmodID > -1 Then
          sQuery.Append(Constants.cAndClause + "lnote_jetnet_amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
          ' change the "make names" into list (of model ids of each make)
        ElseIf Not IsNothing(searchCriteria.ViewCriteriaMakeIDArray) Or Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then

          ' get "list" of models for make
          Dim resultsTable As DataTable = commonEvo.get_view_model_info(searchCriteria, True)

          If Not IsNothing(resultsTable) Then

            If resultsTable.Rows.Count > 0 Then
              For Each r As DataRow In resultsTable.Rows

                If Not IsDBNull(r.Item("amod_id")) Then
                  If Not String.IsNullOrEmpty(r.Item("amod_id").ToString.Trim) Then

                    If String.IsNullOrEmpty(tmpStr) Then
                      tmpStr = r.Item("amod_id").ToString.Trim
                    Else
                      tmpStr += Constants.cCommaDelim + r.Item("amod_id").ToString.Trim
                    End If

                  End If
                End If

              Next
            End If

          End If

          sQuery.Append(Constants.cAndClause + "lnote_jetnet_amod_id IN (" + tmpStr.Trim + ")")

        End If

      End If

      If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaNoteOrderBy.Trim) Then
        If searchCriteria.ViewCriteriaNoteOrderBy.ToLower.Contains("note") And Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaNoteTextValue.Trim) Then
          sQuery.Append(" ORDER BY lnote_note ASC")
        Else
          sQuery.Append(" ORDER BY lnote_entry_date DESC")
        End If
      End If

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>get_notes_view_all_server_notes(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

      MySqlConn.ConnectionString = serverConnectStr

      MySqlConn.Open()
      MySqlCommand.Connection = MySqlConn
      MySqlCommand.CommandType = CommandType.Text
      MySqlCommand.CommandTimeout = 240

      MySqlCommand.CommandText = sQuery.ToString
      MySqlReader = MySqlCommand.ExecuteReader()

      Try
        atemptable.Load(MySqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in get_notes_view_all_server_notes load datatable</b><br /> " + constrExc.Message
      End Try

      ' if "ac search " values are filled in then check and see if any of the "aircraft" match any of the "ac search values"
      If Not (searchCriteria.ViewCriteriaNoteACSearchField = eNotesACSearchTypes.AIRCRAFT_ID Or searchCriteria.ViewCriteriaNoteACSearchField = eNotesACSearchTypes.NULL) Then

        Dim tmpACID As String = ""

        If atemptable.Rows.Count > 0 Then

          For Each ac As DataRow In atemptable.Rows

            If Not IsDBNull(ac.Item("lnote_jetnet_ac_id")) Then
              If Not String.IsNullOrEmpty(ac.Item("lnote_jetnet_ac_id").ToString.Trim) Then
                If String.IsNullOrEmpty(tmpACID.Trim) Then
                  tmpACID = ac.Item("lnote_jetnet_ac_id").ToString.Trim
                Else
                  tmpACID += Constants.cCommaDelim + ac.Item("lnote_jetnet_ac_id").ToString.Trim
                End If
              End If
            End If

          Next

        End If
        ' ok now we have a list of "aircraft ID's" from the "notes" search

        atemptable = New DataTable
        sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Select Case searchCriteria.ViewCriteriaNoteACSearchField

          Case eNotesACSearchTypes.SERIAL_OR_REGNO

            Select Case searchCriteria.ViewCriteriaNoteACSearchOperator

              Case eNotesACSearchOperator.BEGINS
                sQuery.Append("SELECT * FROM Aircraft WITH(NOLOCK) AS a WHERE a.ac_id IN (" + tmpACID.Trim + ") AND a.ac_journ_id = 0 AND (a.ac_ser_no_full LIKE '" + searchCriteria.ViewCriteriaNoteACSearchTextValue.Trim + "%' OR a.ac_reg_no LIKE '" + searchCriteria.ViewCriteriaNoteACSearchTextValue.Trim + "%')")
              Case eNotesACSearchOperator.ANYWHERE
                sQuery.Append("SELECT * FROM Aircraft WITH(NOLOCK) AS a WHERE a.ac_id IN (" + tmpACID.Trim + ") AND a.ac_journ_id = 0 AND (a.ac_ser_no_full LIKE '%" + searchCriteria.ViewCriteriaNoteACSearchTextValue.Trim + "%' OR a.ac_reg_no LIKE '%" + searchCriteria.ViewCriteriaNoteACSearchTextValue.Trim + "%')")
              Case eNotesACSearchOperator.EQUALS
                sQuery.Append("SELECT * FROM Aircraft WITH(NOLOCK) AS a WHERE a.ac_id IN (" + tmpACID.Trim + ") AND a.ac_journ_id = 0 AND (a.ac_ser_no_full = '" + searchCriteria.ViewCriteriaNoteACSearchTextValue.Trim + "' OR a.ac_reg_no = '" + searchCriteria.ViewCriteriaNoteACSearchTextValue.Trim + "')")
            End Select

          Case eNotesACSearchTypes.SERIAL_ONLY

            Select Case searchCriteria.ViewCriteriaNoteACSearchOperator

              Case eNotesACSearchOperator.BEGINS
                sQuery.Append("SELECT * FROM Aircraft WITH(NOLOCK) AS a WHERE a.ac_id IN (" + tmpACID.Trim + ") AND a.ac_journ_id = 0 AND a.ac_ser_no_full LIKE '" + searchCriteria.ViewCriteriaNoteACSearchTextValue.Trim + "%'")
              Case eNotesACSearchOperator.ANYWHERE
                sQuery.Append("SELECT * FROM Aircraft WITH(NOLOCK) AS a WHERE a.ac_id IN (" + tmpACID.Trim + ") AND a.ac_journ_id = 0 AND a.ac_ser_no_full LIKE '%" + searchCriteria.ViewCriteriaNoteACSearchTextValue.Trim + "%'")
              Case eNotesACSearchOperator.EQUALS
                sQuery.Append("SELECT * FROM Aircraft WITH(NOLOCK) AS a WHERE a.ac_id IN (" + tmpACID.Trim + ") AND a.ac_journ_id = 0 AND a.ac_ser_no_full = '" + searchCriteria.ViewCriteriaNoteACSearchTextValue.Trim + "'")
            End Select

          Case eNotesACSearchTypes.REGNO_ONLY

            Select Case searchCriteria.ViewCriteriaNoteACSearchOperator

              Case eNotesACSearchOperator.BEGINS
                sQuery.Append("SELECT * FROM Aircraft WITH(NOLOCK) AS a WHERE a.ac_id IN (" + tmpACID.Trim + ") AND a.ac_journ_id = 0 AND a.ac_reg_no LIKE '" + searchCriteria.ViewCriteriaNoteACSearchTextValue.Trim + "%'")
              Case eNotesACSearchOperator.ANYWHERE
                sQuery.Append("SELECT * FROM Aircraft WITH(NOLOCK) AS a WHERE a.ac_id IN (" + tmpACID.Trim + ") AND a.ac_journ_id = 0 AND a.ac_reg_no LIKE '%" + searchCriteria.ViewCriteriaNoteACSearchTextValue.Trim + "%'")
              Case eNotesACSearchOperator.EQUALS
                sQuery.Append("SELECT * FROM Aircraft WITH(NOLOCK) AS a WHERE a.ac_id IN (" + tmpACID.Trim + ") AND a.ac_journ_id = 0 AND a.ac_reg_no = '" + searchCriteria.ViewCriteriaNoteACSearchTextValue.Trim + "'")
            End Select

        End Select

        If Not searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then

          sQuery.Append(Constants.cAndClause + "(")

          If searchCriteria.ViewCriteriaHasHelicopterFlag Then
            sQuery.Append("a.ac_product_helicopter_flag = 'Y'")
            sSeperator = Constants.cOrClause
          End If

          If searchCriteria.ViewCriteriaHasBusinessFlag Then
            sQuery.Append(sSeperator + "a.ac_product_business_flag = 'Y'")
            sSeperator = Constants.cOrClause
          End If

          If searchCriteria.ViewCriteriaHasCommercialFlag Then
            sQuery.Append(sSeperator + "a.ac_product_commercial_flag = 'Y'")
          End If

          sQuery.Append(")")

        End If

        SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
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
          HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in get_notes_view_all_server_notes load ac datatable</b><br /> " + constrExc.Message
          Return Nothing

        End Try

        tmpACID = ""
        If atemptable.Rows.Count > 0 Then

          For Each ac As DataRow In atemptable.Rows

            If Not IsDBNull(ac.Item("ac_id")) Then
              If Not String.IsNullOrEmpty(ac.Item("ac_id").ToString.Trim) Then
                If String.IsNullOrEmpty(tmpACID.Trim) Then
                  tmpACID = ac.Item("ac_id").ToString.Trim
                Else
                  tmpACID += Constants.cCommaDelim + ac.Item("ac_id").ToString.Trim
                End If
              End If
            End If

          Next

        End If

        If Not String.IsNullOrEmpty(tmpACID.Trim) Then
          ' now "re-query" origional query with "ac from list"
          atemptable = New DataTable
          sQuery = New StringBuilder()

          sQuery.Append("SELECT lnote_id, lnote_jetnet_ac_id, lnote_jetnet_comp_id, lnote_client_ac_id, lnote_client_comp_id, lnote_jetnet_contact_id, lnote_client_contact_id, lnote_note,")
          sQuery.Append(" lnote_entry_date, lnote_action_date, lnote_user_login, lnote_user_name, lnote_notecat_key, lnote_status, lnote_schedule_start_date, lnote_schedule_end_date,")
          sQuery.Append(" lnote_user_id, lnote_clipri_ID, lnote_document_flag, lnote_jetnet_amod_id, lnote_client_amod_id")
          sQuery.Append(" FROM local_notes")
          sQuery.Append(" WHERE (lnote_status = 'A')" + Constants.cAndClause + "(lnote_jetnet_ac_id IN (" + tmpACID.Trim + ")")

          If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaNoteOrderBy.Trim) Then
            If searchCriteria.ViewCriteriaNoteOrderBy.ToLower.Contains("note") And Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaNoteTextValue.Trim) Then
              sQuery.Append(" ORDER BY lnote_note ASC")
            Else
              sQuery.Append(" ORDER BY lnote_entry_date DESC")
            End If
          End If

          MySqlCommand.CommandText = sQuery.ToString

          MySqlReader = MySqlCommand.ExecuteReader()

          Try
            atemptable.Load(MySqlReader)
          Catch constrExc As System.Data.ConstraintException
            Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in get_notes_view_all_server_notes load datatable</b><br /> " + constrExc.Message
            Return Nothing
          End Try

        End If

      End If

    Catch ex As Exception
      Return Nothing

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in get_notes_view_all_server_notes(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + ex.Message

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

  Public Function get_notes_view_all_aircraft_cloud_notes(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sSeperator As String = ""

    Try

      sQuery.Append("SELECT cn_id AS lnote_id, cn_ac_id AS lnote_jetnet_ac_id, cn_comp_id AS lnote_jetnet_comp_id, '0' AS lnote_client_ac_id, '0' AS lnote_client_comp_id,")
      sQuery.Append(" cn_contact_id AS lnote_jetnet_contact_id, '0' AS lnote_client_contact_id, cn_notes AS lnote_note, cn_entry_date AS lnote_entry_date,")
      sQuery.Append(" cn_action_date AS lnote_action_date, cn_user_login AS lnote_user_login, cn_user_name AS lnote_user_name, '25' AS lnote_notecat_key, cn_status AS lnote_status,")
      sQuery.Append(" cn_schedule_start_date AS lnote_schedule_start_date, cn_schedule_end_date AS lnote_schedule_end_date,")
      sQuery.Append(" cn_user_contact_id AS lnote_user_id, '0' AS lnote_clipri_ID, '' AS clipri_name,")
      sQuery.Append(" '0' AS clipri_sort_order, 'N' AS lnote_document_flag, cn_amod_id AS lnote_jetnet_amod_id, '0' AS lnote_client_amod_id,")
      sQuery.Append(" '' AS lnote_document_name, '' AS lnote_opportunity_status, '0' AS lnote_cASh_value, '0' AS lnote_capture_percentage,")
      sQuery.Append(" '' AS lnote_wanted_start_year, '' AS lnote_wanted_end_year, '0' AS lnote_wanted_max_price, '0' AS lnote_wanted_max_aftt,")
      sQuery.Append(" '' AS lnote_wanted_damage_hist, '' AS lnote_wanted_damage_cur")
      sQuery.Append(" FROM " + HttpContext.Current.Session.Item("localPreferences").CloudNotesDatabaseName.ToString.Trim)

      sQuery.Append(" WHERE cn_status = 'A' AND cn_ac_id IS NOT NULL AND cn_ac_id > 0 AND (cn_yt_id = 0 OR cn_yt_id IS NULL)")

      If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaNoteTextValue.Trim) Then
        sQuery.Append(Constants.cAndClause + "(cn_notes LIKE '%" + searchCriteria.ViewCriteriaNoteTextValue.Trim + "%')")
      End If

      If searchCriteria.ViewCriteriaNoteUserID > 0 Then
        sQuery.Append(Constants.cAndClause + "(cn_user_contact_id = " + searchCriteria.ViewCriteriaNoteUserID.ToString + ")")
      End If

      If Not searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then

        Dim tmpProductQuery As New StringBuilder

        If searchCriteria.ViewCriteriaHasHelicopterFlag Then
          tmpProductQuery.Append("EXISTS (SELECT * FROM [jetnet_ra].[dbo].[Aircraft] AS a WHERE a.ac_id = cn_ac_id AND a.ac_journ_id = 0 AND a.ac_product_helicopter_flag = 'Y')")
          sSeperator = Constants.cOrClause
        End If

        If searchCriteria.ViewCriteriaHasBusinessFlag Then
          tmpProductQuery.Append(sSeperator + "EXISTS (SELECT * FROM [jetnet_ra].[dbo].[Aircraft] AS a WHERE a.ac_id = cn_ac_id AND a.ac_journ_id = 0 AND a.ac_product_business_flag = 'Y')")
          sSeperator = Constants.cOrClause
        End If

        If searchCriteria.ViewCriteriaHasCommercialFlag Then
          tmpProductQuery.Append(sSeperator + "EXISTS (SELECT * FROM [jetnet_ra].[dbo].[Aircraft] AS a WHERE a.ac_id = cn_ac_id AND a.ac_journ_id = 0 AND a.ac_product_commercial_flag = 'Y')")
        End If

        If Not String.IsNullOrEmpty(tmpProductQuery.ToString.Trim) Then
          sQuery.Append(Constants.cAndClause + "(" + tmpProductQuery.ToString + ")")
        End If

      End If

      If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaNoteACSearchTextValue.Trim) Then

        Select Case searchCriteria.ViewCriteriaNoteACSearchField

          Case eNotesACSearchTypes.SERIAL_OR_REGNO

            Select Case searchCriteria.ViewCriteriaNoteACSearchOperator

              Case eNotesACSearchOperator.BEGINS
                sQuery.Append(Constants.cAndClause + "EXISTS (SELECT * FROM [jetnet_ra].[dbo].[Aircraft] AS a WHERE a.ac_id = cn_ac_id AND a.ac_journ_id = 0 AND (a.ac_ser_no_full LIKE '" + searchCriteria.ViewCriteriaNoteACSearchTextValue.Trim + "%' OR a.ac_reg_no LIKE '" + searchCriteria.ViewCriteriaNoteACSearchTextValue.Trim + "%'))")
              Case eNotesACSearchOperator.ANYWHERE
                sQuery.Append(Constants.cAndClause + "EXISTS (SELECT * FROM [jetnet_ra].[dbo].[Aircraft] AS a WHERE a.ac_id = cn_ac_id AND a.ac_journ_id = 0 AND (a.ac_ser_no_full LIKE '%" + searchCriteria.ViewCriteriaNoteACSearchTextValue.Trim + "%' OR a.ac_reg_no LIKE '%" + searchCriteria.ViewCriteriaNoteACSearchTextValue.Trim + "%'))")
              Case eNotesACSearchOperator.EQUALS
                sQuery.Append(Constants.cAndClause + "EXISTS (SELECT * FROM [jetnet_ra].[dbo].[Aircraft] AS a WHERE a.ac_id = cn_ac_id AND a.ac_journ_id = 0 AND (a.ac_ser_no_full = '" + searchCriteria.ViewCriteriaNoteACSearchTextValue.Trim + "' OR a.ac_reg_no = '" + searchCriteria.ViewCriteriaNoteACSearchTextValue.Trim + "'))")
            End Select

          Case eNotesACSearchTypes.SERIAL_ONLY

            Select Case searchCriteria.ViewCriteriaNoteACSearchOperator

              Case eNotesACSearchOperator.BEGINS
                sQuery.Append(Constants.cAndClause + "EXISTS (SELECT * FROM [jetnet_ra].[dbo].[Aircraft] AS a WHERE a.ac_id = cn_ac_id AND a.ac_journ_id = 0 AND a.ac_ser_no_full LIKE '" + searchCriteria.ViewCriteriaNoteACSearchTextValue.Trim + "%')")
              Case eNotesACSearchOperator.ANYWHERE
                sQuery.Append(Constants.cAndClause + "EXISTS (SELECT * FROM [jetnet_ra].[dbo].[Aircraft] AS a WHERE a.ac_id = cn_ac_id AND a.ac_journ_id = 0 AND a.ac_ser_no_full LIKE '%" + searchCriteria.ViewCriteriaNoteACSearchTextValue.Trim + "%')")
              Case eNotesACSearchOperator.EQUALS
                sQuery.Append(Constants.cAndClause + "EXISTS (SELECT * FROM [jetnet_ra].[dbo].[Aircraft] AS a WHERE a.ac_id = cn_ac_id AND a.ac_journ_id = 0 AND a.ac_ser_no_full = '" + searchCriteria.ViewCriteriaNoteACSearchTextValue.Trim + "')")
            End Select

          Case eNotesACSearchTypes.REGNO_ONLY

            Select Case searchCriteria.ViewCriteriaNoteACSearchOperator

              Case eNotesACSearchOperator.BEGINS
                sQuery.Append(Constants.cAndClause + "EXISTS (SELECT * FROM [jetnet_ra].[dbo].[Aircraft] AS a WHERE a.ac_id = cn_ac_id AND a.ac_journ_id = 0 AND a.ac_reg_no LIKE '" + searchCriteria.ViewCriteriaNoteACSearchTextValue.Trim + "%')")
              Case eNotesACSearchOperator.ANYWHERE
                sQuery.Append(Constants.cAndClause + "EXISTS (SELECT * FROM [jetnet_ra].[dbo].[Aircraft] AS a WHERE a.ac_id = cn_ac_id AND a.ac_journ_id = 0 AND a.ac_reg_no LIKE '%" + searchCriteria.ViewCriteriaNoteACSearchTextValue.Trim + "%')")
              Case eNotesACSearchOperator.EQUALS
                sQuery.Append(Constants.cAndClause + "EXISTS (SELECT * FROM [jetnet_ra].[dbo].[Aircraft] AS a WHERE a.ac_id = cn_ac_id AND a.ac_journ_id = 0 AND a.ac_reg_no = '" + searchCriteria.ViewCriteriaNoteACSearchTextValue.Trim + "')")
            End Select

          Case eNotesACSearchTypes.AIRCRAFT_ID

            If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaNoteACSearchTextValue.Trim) Then
              If IsNumeric(searchCriteria.ViewCriteriaNoteACSearchTextValue.Trim) Then

                sQuery.Append(Constants.cAndClause + "(cn_ac_id = " + searchCriteria.ViewCriteriaNoteACSearchTextValue.Trim + ")")
                searchCriteria.ViewCriteriaAircraftID = CLng(searchCriteria.ViewCriteriaNoteACSearchTextValue.Trim)

              End If
            End If

        End Select

      End If

      If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaNoteStartDate) And Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaNoteEndDate) Then
        sQuery.Append(Constants.cAndClause + "(cn_entry_date > '" + searchCriteria.ViewCriteriaNoteStartDate.Trim + "' AND cn_entry_date <= '" + searchCriteria.ViewCriteriaNoteEndDate.Trim + "')")
      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaNoteStartDate) And String.IsNullOrEmpty(searchCriteria.ViewCriteriaNoteEndDate) Then
        sQuery.Append(Constants.cAndClause + "(cn_entry_date > '" + searchCriteria.ViewCriteriaNoteStartDate.Trim + "')")
      ElseIf String.IsNullOrEmpty(searchCriteria.ViewCriteriaNoteStartDate) And Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaNoteEndDate) Then
        sQuery.Append(Constants.cAndClause + "(cn_entry_date <= '" + searchCriteria.ViewCriteriaNoteEndDate.Trim + "')")
      Else

        If String.IsNullOrEmpty(searchCriteria.ViewCriteriaNoteACSearchTextValue.Trim) Then  ' get previous 90 days of notes data
          sQuery.Append(Constants.cAndClause + "(cn_entry_date > '" + DateAdd(DateInterval.Month, -3, Now()).ToShortDateString + "')")
          searchCriteria.ViewCriteriaNoteStartDate = DateAdd(DateInterval.Month, -3, Now()).ToShortDateString
        End If

      End If

      If searchCriteria.ViewCriteriaAircraftID = 0 Then

        Dim tmpStr As String = ""

        If Not IsNothing(searchCriteria.ViewCriteriaAmodIDArray) Then

          ' flatten out amodID array ...
          For x As Integer = 0 To UBound(searchCriteria.ViewCriteriaAmodIDArray)
            If String.IsNullOrEmpty(tmpStr) Then
              tmpStr = searchCriteria.ViewCriteriaAmodIDArray(x)
            Else
              tmpStr += Constants.cCommaDelim + searchCriteria.ViewCriteriaAmodIDArray(x)
            End If
          Next

          sQuery.Append(Constants.cAndClause + "cn_amod_id IN (" + tmpStr.Trim + ")")
        ElseIf searchCriteria.ViewCriteriaAmodID > -1 Then
          sQuery.Append(Constants.cAndClause + "cn_amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
        ElseIf Not IsNothing(searchCriteria.ViewCriteriaMakeIDArray) Or Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then

          ' get "list" of models for make
          Dim resultsTable As DataTable = commonEvo.get_view_model_info(searchCriteria, True)

          If Not IsNothing(resultsTable) Then

            If resultsTable.Rows.Count > 0 Then
              For Each r As DataRow In resultsTable.Rows

                If Not IsDBNull(r.Item("amod_id")) Then
                  If Not String.IsNullOrEmpty(r.Item("amod_id").ToString.Trim) Then

                    If String.IsNullOrEmpty(tmpStr) Then
                      tmpStr = r.Item("amod_id").ToString.Trim
                    Else
                      tmpStr += Constants.cCommaDelim + r.Item("amod_id").ToString.Trim
                    End If

                  End If
                End If

              Next
            End If

          End If

          sQuery.Append(Constants.cAndClause + "cn_amod_id IN (" + tmpStr.Trim + ")")
        End If

      End If

      If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaNoteOrderBy.Trim) Then
        If searchCriteria.ViewCriteriaNoteOrderBy.ToLower.Contains("note") And Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaNoteTextValue.Trim) Then
          sQuery.Append(" ORDER BY cn_notes ASC")
        Else
          sQuery.Append(" ORDER BY cn_entry_date DESC")
        End If
      End If

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>get_notes_view_all_aircraft_cloud_notes(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = cloudConnectStr

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
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in get_notes_view_all_aircraft_cloud_notes load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in get_notes_view_all_aircraft_cloud_notes(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

  Public Function get_notes_view_all_yacht_cloud_notes(ByRef searchCriteria As yachtViewSelectionCriteria) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sSeperator As String = ""

    Try

      sQuery.Append("SELECT cn_id AS lnote_id, cn_yt_id AS lnote_jetnet_yt_id, cn_comp_id AS lnote_jetnet_comp_id, '0' AS lnote_client_ac_id, '0' AS lnote_client_comp_id,")
      sQuery.Append(" cn_contact_id AS lnote_jetnet_contact_id, '0' AS lnote_client_contact_id, cn_notes AS lnote_note, cn_entry_date AS lnote_entry_date,")
      sQuery.Append(" cn_action_date AS lnote_action_date, cn_user_login AS lnote_user_login, cn_user_name AS lnote_user_name, '25' AS lnote_notecat_key, cn_status AS lnote_status,")
      sQuery.Append(" cn_schedule_start_date AS lnote_schedule_start_date, cn_schedule_end_date AS lnote_schedule_end_date,")
      sQuery.Append(" cn_user_contact_id AS lnote_user_id, '0' AS lnote_clipri_ID, '' AS clipri_name,")
      sQuery.Append(" '0' AS clipri_sort_order, 'N' AS lnote_document_flag, cn_ym_model_id AS lnote_jetnet_ymod_id, '0' AS lnote_client_amod_id,")
      sQuery.Append(" '' AS lnote_document_name, '' AS lnote_opportunity_status, '0' AS lnote_cash_value, '0' AS lnote_capture_percentage,")
      sQuery.Append(" '' AS lnote_wanted_start_year, '' AS lnote_wanted_end_year, '0' AS lnote_wanted_max_price, '0' AS lnote_wanted_max_aftt,")
      sQuery.Append(" '' AS lnote_wanted_damage_hist, '' AS lnote_wanted_damage_cur")
      sQuery.Append(" FROM " + HttpContext.Current.Session.Item("localPreferences").CloudNotesDatabaseName.ToString.Trim)

      sQuery.Append(" WHERE cn_status = 'A'  AND cn_yt_id IS NOT NULL AND cn_yt_id > 0 AND (cn_ac_id = 0 OR cn_ac_id IS NULL)")

      If Not String.IsNullOrEmpty(searchCriteria.YachtViewCriteriaNoteTextValue.Trim) Then
        sQuery.Append(Constants.cAndClause + "(cn_notes LIKE '%" + searchCriteria.YachtViewCriteriaNoteTextValue.Trim + "%')")
      End If

      If searchCriteria.YachtViewCriteriaNoteUserID > 0 Then
        sQuery.Append(Constants.cAndClause + "(cn_user_contact_id = " + searchCriteria.YachtViewCriteriaNoteUserID.ToString + ")")
      End If

      If Not String.IsNullOrEmpty(searchCriteria.YachtViewCriteriaNoteYTSearchTextValue.Trim) Then

        Select Case searchCriteria.YachtViewCriteriaNoteYTSearchField

          Case eNotesYTSearchTypes.NAME_OR_CALLSIGN

            Select Case searchCriteria.YachtViewCriteriaNoteYTSearchOperator

              Case eNotesACSearchOperator.BEGINS
                sQuery.Append(Constants.cAndClause + "EXISTS (SELECT * FROM [jetnet_ra].[dbo].[Yacht] AS y WHERE y.yt_id = cn_yt_id AND y.yt_journ_id = 0 AND (y.yt_radio_call_sign LIKE '" + searchCriteria.YachtViewCriteriaNoteYTSearchTextValue.Trim + "%' OR a.yt_yacht_name_search LIKE '" + searchCriteria.YachtViewCriteriaNoteYTSearchTextValue.Trim + "%'))")
              Case eNotesACSearchOperator.ANYWHERE
                sQuery.Append(Constants.cAndClause + "EXISTS (SELECT * FROM [jetnet_ra].[dbo].[Yacht] AS y WHERE y.yt_id = cn_yt_id AND y.yt_journ_id = 0 AND (y.yt_radio_call_sign LIKE '%" + searchCriteria.YachtViewCriteriaNoteYTSearchTextValue.Trim + "%' OR a.yt_yacht_name_search LIKE '%" + searchCriteria.YachtViewCriteriaNoteYTSearchTextValue.Trim + "%'))")
              Case eNotesACSearchOperator.EQUALS
                sQuery.Append(Constants.cAndClause + "EXISTS (SELECT * FROM [jetnet_ra].[dbo].[Yacht] AS y WHERE y.yt_id = cn_yt_id AND y.yt_journ_id = 0 AND (y.yt_radio_call_sign = '" + searchCriteria.YachtViewCriteriaNoteYTSearchTextValue.Trim + "' OR a.yt_yacht_name_search = '" + searchCriteria.YachtViewCriteriaNoteYTSearchTextValue.Trim + "'))")
            End Select

          Case eNotesYTSearchTypes.CALLSIGN_ONLY

            Select Case searchCriteria.YachtViewCriteriaNoteYTSearchOperator

              Case eNotesACSearchOperator.BEGINS
                sQuery.Append(Constants.cAndClause + "EXISTS (SELECT * FROM [jetnet_ra].[dbo].[Yacht] AS y WHERE y.yt_id = cn_yt_id AND y.yt_journ_id = 0 AND y.yt_radio_call_sign LIKE '" + searchCriteria.YachtViewCriteriaNoteYTSearchTextValue.Trim + "%')")
              Case eNotesACSearchOperator.ANYWHERE
                sQuery.Append(Constants.cAndClause + "EXISTS (SELECT * FROM [jetnet_ra].[dbo].[Yacht] AS y WHERE y.yt_id = cn_yt_id AND y.yt_journ_id = 0 AND y.yt_radio_call_sign LIKE '%" + searchCriteria.YachtViewCriteriaNoteYTSearchTextValue.Trim + "%')")
              Case eNotesACSearchOperator.EQUALS
                sQuery.Append(Constants.cAndClause + "EXISTS (SELECT * FROM [jetnet_ra].[dbo].[Yacht] AS y WHERE y.yt_id = cn_yt_id AND y.yt_journ_id = 0 AND y.yt_radio_call_sign = '" + searchCriteria.YachtViewCriteriaNoteYTSearchTextValue.Trim + "')")
            End Select

          Case eNotesYTSearchTypes.NAME_ONLY

            Select Case searchCriteria.YachtViewCriteriaNoteYTSearchOperator

              Case eNotesACSearchOperator.BEGINS
                sQuery.Append(Constants.cAndClause + "EXISTS (SELECT * FROM [jetnet_ra].[dbo].[Yacht] AS y WHERE y.yt_id = cn_yt_id AND y.yt_journ_id = 0 AND y.yt_yacht_name_search LIKE '" + searchCriteria.YachtViewCriteriaNoteYTSearchTextValue.Trim + "%')")
              Case eNotesACSearchOperator.ANYWHERE
                sQuery.Append(Constants.cAndClause + "EXISTS (SELECT * FROM [jetnet_ra].[dbo].[Yacht] AS y WHERE y.yt_id = cn_yt_id AND y.yt_journ_id = 0 AND y.yt_yacht_name_search LIKE '%" + searchCriteria.YachtViewCriteriaNoteYTSearchTextValue.Trim + "%')")
              Case eNotesACSearchOperator.EQUALS
                sQuery.Append(Constants.cAndClause + "EXISTS (SELECT * FROM [jetnet_ra].[dbo].[Yacht] AS y WHERE y.yt_id = cn_yt_id AND y.yt_journ_id = 0 AND y.yt_yacht_name_search = '" + searchCriteria.YachtViewCriteriaNoteYTSearchTextValue.Trim + "')")
            End Select

          Case eNotesYTSearchTypes.YACHT_ID

            If Not String.IsNullOrEmpty(searchCriteria.YachtViewCriteriaNoteYTSearchTextValue.Trim) Then
              If IsNumeric(searchCriteria.YachtViewCriteriaNoteYTSearchTextValue.Trim) Then

                sQuery.Append(Constants.cAndClause + "(cn_yt_id = " + searchCriteria.YachtViewCriteriaNoteYTSearchTextValue.Trim + ")")
                searchCriteria.YachtViewCriteriaYachtID = CLng(searchCriteria.YachtViewCriteriaNoteYTSearchTextValue.Trim)

              End If
            End If

        End Select

      End If

      If Not String.IsNullOrEmpty(searchCriteria.YachtViewCriteriaNoteStartDate) And Not String.IsNullOrEmpty(searchCriteria.YachtViewCriteriaNoteEndDate) Then
        sQuery.Append(Constants.cAndClause + "(cn_entry_date > '" + searchCriteria.YachtViewCriteriaNoteStartDate.Trim + "' AND cn_entry_date <= '" + searchCriteria.YachtViewCriteriaNoteEndDate.Trim + "')")
      ElseIf Not String.IsNullOrEmpty(searchCriteria.YachtViewCriteriaNoteStartDate) And String.IsNullOrEmpty(searchCriteria.YachtViewCriteriaNoteEndDate) Then
        sQuery.Append(Constants.cAndClause + "(cn_entry_date > '" + searchCriteria.YachtViewCriteriaNoteStartDate.Trim + "')")
      ElseIf String.IsNullOrEmpty(searchCriteria.YachtViewCriteriaNoteStartDate) And Not String.IsNullOrEmpty(searchCriteria.YachtViewCriteriaNoteEndDate) Then
        sQuery.Append(Constants.cAndClause + "(cn_entry_date <= '" + searchCriteria.YachtViewCriteriaNoteEndDate.Trim + "')")
      Else

        If String.IsNullOrEmpty(searchCriteria.YachtViewCriteriaNoteYTSearchTextValue.Trim) Then  ' get previous 90 days of notes data
          sQuery.Append(Constants.cAndClause + "(cn_entry_date > '" + DateAdd(DateInterval.Month, -3, Now()).ToShortDateString + "')")
          searchCriteria.YachtViewCriteriaNoteStartDate = DateAdd(DateInterval.Month, -3, Now()).ToShortDateString
        End If

      End If

      If searchCriteria.YachtViewCriteriaYachtID = 0 Then

        Dim tmpStr As String = ""

        If Not IsNothing(searchCriteria.YachtViewCriteriaYmodIDArray) Then

          ' flatten out amodID array ...
          For x As Integer = 0 To UBound(searchCriteria.YachtViewCriteriaYmodIDArray)
            If String.IsNullOrEmpty(tmpStr) Then
              tmpStr = searchCriteria.YachtViewCriteriaYmodIDArray(x)
            Else
              tmpStr += Constants.cCommaDelim + searchCriteria.YachtViewCriteriaYmodIDArray(x)
            End If
          Next

          sQuery.Append(Constants.cAndClause + "cn_ym_model_id IN (" + tmpStr.Trim + ")")
        ElseIf searchCriteria.YachtViewCriteriaYmodID > -1 Then
          sQuery.Append(Constants.cAndClause + "cn_ym_model_id = " + searchCriteria.YachtViewCriteriaYmodID.ToString)
        ElseIf Not IsNothing(searchCriteria.YachtViewCriteriaBrandIDArray) Or Not String.IsNullOrEmpty(searchCriteria.YachtViewCriteriaYachtBrand.Trim) Then

          ' get "list" of models for make
          Dim resultsTable As DataTable = commonEvo.get_yacht_view_model_info(searchCriteria, True)

          If Not IsNothing(resultsTable) Then

            If resultsTable.Rows.Count > 0 Then
              For Each r As DataRow In resultsTable.Rows

                If Not IsDBNull(r.Item("ym_model_id")) Then
                  If Not String.IsNullOrEmpty(r.Item("ym_model_id").ToString.Trim) Then

                    If String.IsNullOrEmpty(tmpStr) Then
                      tmpStr = r.Item("ym_model_id").ToString.Trim
                    Else
                      tmpStr += Constants.cCommaDelim + r.Item("ym_model_id").ToString.Trim
                    End If

                  End If
                End If

              Next
            End If

          End If

          sQuery.Append(Constants.cAndClause + "cn_ym_model_id IN (" + tmpStr.Trim + ")")
        End If

      End If

      If Not String.IsNullOrEmpty(searchCriteria.YachtViewCriteriaNoteOrderBy.Trim) Then
        If searchCriteria.YachtViewCriteriaNoteOrderBy.ToLower.Contains("note") And Not String.IsNullOrEmpty(searchCriteria.YachtViewCriteriaNoteTextValue.Trim) Then
          sQuery.Append(" ORDER BY cn_notes ASC")
        Else
          sQuery.Append(" ORDER BY cn_entry_date DESC")
        End If
      End If

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>get_notes_view_all_yacht_cloud_notes(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = cloudConnectStr

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
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in get_notes_view_all_yacht_cloud_notes load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in get_notes_view_all_yacht_cloud_notes(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

    Public Sub display_notes_view_listTable(ByVal bHasMaster As Boolean, ByVal bIsAdmin As Boolean, ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, Optional ByRef ytSearchCriteria As yachtViewSelectionCriteria = Nothing, Optional ByRef table_count As Integer = 0)

        Dim results_table As New DataTable   '
        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False

        Dim tableTitle As String = ""
        Dim dateRangeTitle As String = ""

        Dim sNoteID As String = ""

        Dim nAircraftID As Long = 0
        Dim nYachtID As Long = 0

        Dim acInfoStr As String = ""
        Dim ytInfoStr As String = ""

        Dim nCompanyID As Long = 0
        Dim sUserLogin As String = ""
        Dim sUserName As String = ""
        Dim sNoteText As String = ""
        Dim sEntryDate As String = ""
        Dim sCategory As String = ""
        Dim sNoteStatus As String = ""
        Dim sStartDate As String = ""
        Dim sEndDate As String = ""

        Dim bIsReport As Boolean = False

        Try

            Dim bHasStandardCloudNotes As Boolean = HttpContext.Current.Session.Item("localPreferences").HasCloudNotes
            Dim bHasServerNotes As Boolean = HttpContext.Current.Session.Item("localPreferences").HasServerNotes

            If bHasServerNotes Then 'If they're plus + notes users. 
                results_table = get_notes_view_all_aircraft_server_notes(searchCriteria)
                tableTitle = " Cloud Notes Plus database "
            ElseIf bHasStandardCloudNotes Then 'if they're standard cloud users 

                tableTitle = " Cloud Notes database "

                Select Case CType(HttpContext.Current.Session.Item("jetnetWebHostType"), crmWebClient.eWebHostTypes)

                    Case eWebHostTypes.YACHT
                        results_table = get_notes_view_all_yacht_cloud_notes(ytSearchCriteria)
                        bIsReport = ytSearchCriteria.YachtViewCriteriaIsReport
                    Case eWebHostTypes.EVOLUTION
                        results_table = get_notes_view_all_aircraft_cloud_notes(searchCriteria)
                        bIsReport = searchCriteria.ViewCriteriaIsReport
                End Select

            End If

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    table_count = results_table.Rows.Count

                    Select Case CType(HttpContext.Current.Session.Item("jetnetWebHostType"), crmWebClient.eWebHostTypes)

                        Case eWebHostTypes.YACHT
                            If Not String.IsNullOrEmpty(ytSearchCriteria.YachtViewCriteriaNoteStartDate.Trim) Then
                                sStartDate = "starting&nbsp;" + FormatDateTime(CDate(ytSearchCriteria.YachtViewCriteriaNoteStartDate), DateFormat.ShortDate).ToString
                            End If

                            If Not String.IsNullOrEmpty(ytSearchCriteria.YachtViewCriteriaNoteEndDate.Trim) Then
                                sEndDate = IIf(String.IsNullOrEmpty(sStartDate.Trim), "", "&nbsp;") + "up&nbsp;to&nbsp;" + FormatDateTime(CDate(ytSearchCriteria.YachtViewCriteriaNoteEndDate), DateFormat.ShortDate).ToString
                            End If

                        Case eWebHostTypes.EVOLUTION
                            If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaNoteStartDate.Trim) Then
                                sStartDate = "starting&nbsp;" + FormatDateTime(CDate(searchCriteria.ViewCriteriaNoteStartDate), DateFormat.ShortDate).ToString
                            End If

                            If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaNoteEndDate.Trim) Then
                                sEndDate = IIf(String.IsNullOrEmpty(sStartDate.Trim), "", "&nbsp;") + "up&nbsp;to&nbsp;" + FormatDateTime(CDate(searchCriteria.ViewCriteriaNoteEndDate), DateFormat.ShortDate).ToString
                            End If

                    End Select

                    If (Not String.IsNullOrEmpty(sStartDate.Trim)) Or (Not String.IsNullOrEmpty(sEndDate.Trim)) Then
                        dateRangeTitle = "&nbsp;<em>" + sStartDate + sEndDate + "</em>"
                    End If

                    If Not bIsReport Then
                        If results_table.Rows.Count > 15 Then
                            htmlOut.Append("<div align=""left"" valign=""top"" style=""height:470px; overflow: auto;"">")
                        End If

                        If bIsAdmin Then
                            Select Case CType(HttpContext.Current.Session.Item("jetnetWebHostType"), crmWebClient.eWebHostTypes)

                                Case eWebHostTypes.YACHT
                                    htmlOut.Append("<div align=""right"" style=""padding-right:8px;""><strong><a class=""underline pointer"" href=""Yacht_View_Template.aspx?" + IIf(Not bHasMaster, "noMaster=false&", "") + "ViewID=" + ytSearchCriteria.YachtViewID.ToString + "&ViewName=" + ytSearchCriteria.YachtViewName + "&bIsReport=Y"">Export to Excel</a></strong></div>")

                                Case eWebHostTypes.EVOLUTION
                                    htmlOut.Append("<div align=""right"" style=""padding-right:8px;""><strong><a class=""underline pointer"" href=""View_Template.aspx?" + IIf(Not bHasMaster, "noMaster=false&", "") + "ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName + "&bIsReport=Y"">Export to Excel</a></strong></div>")

                            End Select
                        End If

                    End If

                    If Not bIsReport Then
                        htmlOut.Append("<table id=""notesViewListDataTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"" border=""0"">")
                        htmlOut.Append("<tr><td colspan=""" + IIf(bHasServerNotes, "5", "4") + """ align=""center""><b>" + results_table.Rows.Count.ToString + " User notes in " + tableTitle.Trim + dateRangeTitle.Trim + "</b></td></tr>")
                        htmlOut.Append("<tr><td align=""left"" width=""5%""><b>&nbsp;</b></td>")
                    Else
                        htmlOut.Append(include_excel_report_style())
                        htmlOut.Append("<table id=""notesViewListDataTable"" border=""1"" cellpadding=""2"" cellspacing=""0"">")
                        htmlOut.Append("<tr><td colspan=""" + IIf(bHasServerNotes, "5", "4") + """ align=""center""><b>" + results_table.Rows.Count.ToString + " User notes in " + tableTitle.Trim + dateRangeTitle.Trim + "</b></td></tr>")
                        htmlOut.Append("<tr><td align=""left""><b>NoteID</b></td>")
                    End If

                    htmlOut.Append("<td align=""left"" width=""15%""><b>Entry Date</b></td>")
                    htmlOut.Append("<td align=""center"" width=""50%""><b>Note</b></td>")

                    Select Case CType(HttpContext.Current.Session.Item("jetnetWebHostType"), crmWebClient.eWebHostTypes)

                        Case eWebHostTypes.YACHT
                            htmlOut.Append("<td align=""left""><b>Yacht</b></td>")

                        Case eWebHostTypes.EVOLUTION
                            htmlOut.Append("<td align=""left""><b>Aircraft</b></td>")

                    End Select

                    If bHasServerNotes Then
                        htmlOut.Append("<td align=""left""><b>Company</b></td>")
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

                        If Not IsDBNull(r.Item("lnote_id")) Then
                            If Not String.IsNullOrEmpty(r.Item("lnote_id").ToString.Trim) Then
                                sNoteID = r.Item("lnote_id").ToString.Trim
                            End If
                        End If

                        If Not IsDBNull(r.Item("lnote_status")) Then
                            If Not String.IsNullOrEmpty(r.Item("lnote_status").ToString.Trim) Then
                                sNoteStatus = r.Item("lnote_status").ToString.Trim
                            End If
                        End If

                        If Not IsDBNull(r.Item("lnote_entry_date")) Then
                            If Not String.IsNullOrEmpty(r.Item("lnote_entry_date").ToString.Trim) Then
                                sEntryDate = FormatDateTime(r.Item("lnote_entry_date").ToString.Trim, DateFormat.ShortDate)
                            End If
                        End If

                        If Not IsDBNull(r.Item("lnote_user_name")) Then
                            If Not String.IsNullOrEmpty(r.Item("lnote_user_name").ToString.Trim) Then
                                sUserLogin = r.Item("lnote_user_name").ToString.Replace(Constants.cSingleSpace, Constants.cHTMLnbsp).Trim
                            End If
                        End If

                        If Not String.IsNullOrEmpty(sUserLogin.Trim) Then
                            sEntryDate += "<br />By: <em>" + sUserLogin + "</em>"
                        End If

                        Select Case CType(HttpContext.Current.Session.Item("jetnetWebHostType"), crmWebClient.eWebHostTypes)

                            Case eWebHostTypes.YACHT
                                If Not IsDBNull(r.Item("lnote_jetnet_yt_id")) Then
                                    If Not String.IsNullOrEmpty(r.Item("lnote_jetnet_yt_id").ToString.Trim) Then
                                        nYachtID = CLng(r.Item("lnote_jetnet_yt_id").ToString.Trim)
                                    End If
                                End If

                            Case eWebHostTypes.EVOLUTION
                                If Not IsDBNull(r.Item("lnote_jetnet_ac_id")) Then
                                    If Not String.IsNullOrEmpty(r.Item("lnote_jetnet_ac_id").ToString.Trim) Then
                                        nAircraftID = CLng(r.Item("lnote_jetnet_ac_id").ToString.Trim)
                                    End If
                                End If

                        End Select


                        If Not IsDBNull(r.Item("lnote_jetnet_comp_id")) Then
                            If Not String.IsNullOrEmpty(r.Item("lnote_jetnet_comp_id").ToString.Trim) Then
                                nCompanyID = CLng(r.Item("lnote_jetnet_comp_id").ToString.Trim)
                            End If
                        End If

                        If Not IsDBNull(r.Item("lnote_note")) Then
                            If Not String.IsNullOrEmpty(r.Item("lnote_note").ToString.Trim) Then
                                sNoteText = r.Item("lnote_note").ToString.Trim.Replace(vbCrLf, "<br />")
                            End If
                        End If

                        If Not bIsReport Then
                            htmlOut.Append("<td align=""left"" valign=""middle""><img src=""images/note_view_pin.PNG"" height=""48"" width=""48"" class=""bullet"" alt=""Note ID : " + sNoteID.Trim + """ title=""Note ID : " + sNoteID.Trim + """/></td>")
                        Else
                            htmlOut.Append("<td align=""left"">" + sNoteID.Trim + "</td>")
                        End If
                        htmlOut.Append("<td align=""left"">" + sEntryDate.Trim + "</td>")
                        htmlOut.Append("<td align=""left"">" + sNoteText.Trim + "</td>")

                        If nAircraftID > 0 Then

                            acInfoStr = ""

                            Dim acInfoArray() As String = Split(commonEvo.GetAircraftInfo(nAircraftID, False), Constants.cSvrDataSeperator)

                            If Not String.IsNullOrEmpty(acInfoArray(0).ToString) Then
                                acInfoStr = acInfoArray(0).ToString
                            End If

                            If Not String.IsNullOrEmpty(acInfoArray(1).ToString) Then
                                acInfoStr += Constants.cSingleSpace + acInfoArray(1).ToString
                            End If

                            If Not String.IsNullOrEmpty(acInfoArray(2).ToString) Then
                                If Not bIsReport Then
                                    acInfoStr += "<br />Serial# <a class=""underline pointer"" onclick='javascript:openSmallWindowJS(""DisplayAircraftDetail.aspx?acid=" + nAircraftID.ToString + "&jid=0"",""AircraftDetails"");' title=""Display Aircraft Details"">"
                                    acInfoStr += acInfoArray(2).Trim + "</a>"
                                Else
                                    acInfoStr += "<br />Serial# " + acInfoArray(2).Trim
                                End If

                            End If

                            If Not String.IsNullOrEmpty(acInfoArray(3).ToString) Then
                                acInfoStr += "<br />Reg# " + acInfoArray(3).ToString
                            End If

                            htmlOut.Append("<td align=""left"" nowrap=""nowrap"">" + IIf(Not String.IsNullOrEmpty(acInfoStr.Trim), acInfoStr.Trim, "") + "</td>")

                        ElseIf nYachtID > 0 Then

                            ytInfoStr = ""

                            Dim ytInfoArray() As String = Split(commonEvo.GetYachtInfo(nYachtID, False), Constants.cSvrDataSeperator)

                            If Not String.IsNullOrEmpty(ytInfoArray(0).ToString) Then
                                ytInfoStr = ytInfoArray(0).ToString
                            End If

                            If Not String.IsNullOrEmpty(ytInfoArray(1).ToString) Then
                                ytInfoStr += Constants.cSingleSpace + ytInfoArray(1).ToString
                            End If

                            If Not String.IsNullOrEmpty(ytInfoArray(4).ToString) Then
                                If Not bIsReport Then
                                    ytInfoStr += "<br /><a class=""underline pointer"" onclick='javascript:openSmallWindowJS(""DisplayYachtDetail.aspx?yid=" + nYachtID.ToString + "&jid=0"",""YachtDetails"");' title=""Display Yacht Details"">"
                                    ytInfoStr += ytInfoArray(4).Trim + "</a>"
                                Else
                                    ytInfoStr += "<br />" + ytInfoArray(4).Trim
                                End If

                                If Not String.IsNullOrEmpty(ytInfoArray(3).ToString) Then
                                    ytInfoStr += " Hull# " + ytInfoArray(3).ToString
                                End If

                            End If

                            htmlOut.Append("<td align=""left"" nowrap=""nowrap"">" + IIf(Not String.IsNullOrEmpty(ytInfoStr.Trim), ytInfoStr.Trim, "") + "</td>")

                        Else
                            htmlOut.Append("<td align=""left"" nowrap=""nowrap""> NA </td>")
                        End If


                        If bHasServerNotes Then
                            htmlOut.Append("<td align=""left"">" + IIf(nCompanyID > 0, commonEvo.get_company_info_fromID(nCompanyID, 0, True, (Not bIsReport), "", ""), " NA ") + "</td>")
                        End If

                        htmlOut.Append("</tr>")

                    Next

                    htmlOut.Append("</table>")

                    If Not bIsReport Then
                        If results_table.Rows.Count > 15 Then
                            htmlOut.Append("</div>")
                        End If
                    End If

                Else
                    htmlOut.Append("<table id=""notesViewListDataTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"">")
                    htmlOut.Append("<tr><td valign=""middle"" align=""left""><img src=""images/alert.png"" class=""bullet"" alt=""Search Error"" title=""Search Error""/>")
                    htmlOut.Append("&nbsp;No " + tableTitle.Replace("database", Constants.cEmptyString).Trim + " Found ... Please check your selections and try again ...</td></tr></table>")
                End If
            Else
                htmlOut.Append("<table id=""notesViewListDataTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"">")
                htmlOut.Append("<tr><td valign=""middle"" align=""left""><img src=""images/alert.png"" class=""bullet"" alt=""Search Error"" title=""Search Error""/>")
                htmlOut.Append("&nbsp;No " + tableTitle.Replace("database", Constants.cEmptyString).Trim + " Found ... Please check your selections and try again ...</td></tr></table>")
            End If

        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in display_notes_view_listTable(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub

    Public Function get_notes_view_notesPlus_summaryDataTable(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
    Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
    Dim MySqlReader As MySql.Data.MySqlClient.MySqlDataReader = Nothing
    Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing

    Try

      ' will have to adjust for date selections
      Dim startDate As Date = CDate(Now().Month.ToString + "/01/" + Now().Year.ToString)

      Dim month_one As String = Format(DateAdd(DateInterval.Month, -1, startDate), "yyyy-MM-dd H:mm:ss").Trim
      Dim month_two As String = Format(DateAdd(DateInterval.Month, -2, startDate), "yyyy-MM-dd H:mm:ss").Trim
      Dim month_three As String = Format(DateAdd(DateInterval.Month, -3, startDate), "yyyy-MM-dd H:mm:ss").Trim
      Dim month_four As String = Format(DateAdd(DateInterval.Month, -4, startDate), "yyyy-MM-dd H:mm:ss").Trim
      Dim month_five As String = Format(DateAdd(DateInterval.Month, -5, startDate), "yyyy-MM-dd H:mm:ss").Trim
      Dim month_six As String = Format(DateAdd(DateInterval.Month, -6, startDate), "yyyy-MM-dd H:mm:ss").Trim


      sQuery.Append("SELECT cliuser_id, cliuser_first_name, cliuser_last_name, cliuser_email_address,")
      sQuery.Append(" (SELECT COUNT(lnote_id) FROM local_notes WHERE lnote_status='A' AND cliuser_id = lnote_user_id AND lnote_entry_date >= '" + month_one + "' AND lnote_entry_date < '" + Format(startDate, "yyyy-MM-dd H:mm:ss").Trim + "') AS month_one_count,")
      sQuery.Append(" (SELECT COUNT(lnote_id) FROM local_notes WHERE lnote_status='A' AND cliuser_id = lnote_user_id AND lnote_entry_date >= '" + month_two + "' AND lnote_entry_date < '" + month_one + "') AS month_two_count, ")
      sQuery.Append(" (SELECT COUNT(lnote_id) FROM local_notes WHERE lnote_status='A' AND cliuser_id = lnote_user_id AND lnote_entry_date >= '" + month_three + "' AND lnote_entry_date < '" + month_two + "') AS month_three_count, ")
      sQuery.Append(" (SELECT COUNT(lnote_id) FROM local_notes WHERE lnote_status='A' AND cliuser_id = lnote_user_id AND lnote_entry_date >= '" + month_four + "' AND lnote_entry_date < '" + month_three + "') AS month_four_count, ")
      sQuery.Append(" (SELECT COUNT(lnote_id) FROM local_notes WHERE lnote_status='A' AND cliuser_id = lnote_user_id AND lnote_entry_date >= '" + month_five + "' AND lnote_entry_date < '" + month_four + "') AS month_five_count, ")
      sQuery.Append(" (SELECT COUNT(lnote_id) FROM local_notes WHERE lnote_status='A' AND cliuser_id = lnote_user_id AND lnote_entry_date >= '" + month_six + "' AND lnote_entry_date < '" + month_five + "') AS month_six_count, ")
      sQuery.Append(" (SELECT COUNT(lnote_id) FROM local_notes WHERE lnote_status='A' AND cliuser_id = lnote_user_id) AS total_note_count, ")
      sQuery.Append(" (SELECT COUNT(lnote_id) FROM local_notes WHERE lnote_status='P' AND cliuser_id = lnote_user_id) AS total_action_count")
      sQuery.Append(" FROM client_user")
      sQuery.Append(" WHERE cliuser_active_flag = 'Y'")

      sQuery.Append(" ORDER BY cliuser_last_name asc")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>get_notes_view_notesPlus_summaryDataTable(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

      MySqlConn.ConnectionString = serverConnectStr

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
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in get_notes_view_notesPlus_summaryDataTable load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in get_notes_view_notesPlus_summaryDataTable(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

  Public Function get_notes_view_cloudNotes_summaryDataTable(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try
      ' will have to adjust for date selections

      Dim startDate As Date = CDate(Now().Month.ToString + "/01/" + Now().Year.ToString)

      Dim month_one As String = DateAdd(DateInterval.Month, -1, startDate).ToShortDateString
      Dim month_two As String = DateAdd(DateInterval.Month, -2, startDate).ToShortDateString
      Dim month_three As String = DateAdd(DateInterval.Month, -3, startDate).ToShortDateString
      Dim month_four As String = DateAdd(DateInterval.Month, -4, startDate).ToShortDateString
      Dim month_five As String = DateAdd(DateInterval.Month, -5, startDate).ToShortDateString
      Dim month_six As String = DateAdd(DateInterval.Month, -6, startDate).ToShortDateString

      Dim cloudNotes_databaseTable As String = HttpContext.Current.Session.Item("localPreferences").CloudNotesDatabaseName.ToString.Trim

      sQuery.Append("SELECT c.contact_id AS cliuser_id, c.contact_first_name AS cliuser_first_name, c.contact_last_name AS cliuser_last_name, c.contact_email_address AS cliuser_email_address,")
      sQuery.Append(" (SELECT COUNT(cn_id) FROM " + cloudNotes_databaseTable.Trim + " WHERE cn_status='A' AND c.contact_id = cn_user_contact_id AND cn_entry_date >= '" + month_one + "' AND cn_entry_date < '" + startDate.ToShortDateString + "') AS month_one_count,")
      sQuery.Append(" (SELECT COUNT(cn_id) FROM " + cloudNotes_databaseTable.Trim + " WHERE cn_status='A' AND c.contact_id = cn_user_contact_id AND cn_entry_date >= '" + month_two + "' AND cn_entry_date < '" + month_one + "') AS month_two_count,")
      sQuery.Append(" (SELECT COUNT(cn_id) FROM " + cloudNotes_databaseTable.Trim + " WHERE cn_status='A' AND c.contact_id = cn_user_contact_id AND cn_entry_date >= '" + month_three + "' AND cn_entry_date < '" + month_two + "') AS month_three_count,")
      sQuery.Append(" (SELECT COUNT(cn_id) FROM " + cloudNotes_databaseTable.Trim + " WHERE cn_status='A' AND c.contact_id = cn_user_contact_id AND cn_entry_date >= '" + month_four + "' AND cn_entry_date < '" + month_three + "') AS month_four_count,")
      sQuery.Append(" (SELECT COUNT(cn_id) FROM " + cloudNotes_databaseTable.Trim + " WHERE cn_status='A' AND c.contact_id = cn_user_contact_id AND cn_entry_date >= '" + month_five + "' AND cn_entry_date < '" + month_four + "') AS month_five_count,")
      sQuery.Append(" (SELECT COUNT(cn_id) FROM " + cloudNotes_databaseTable.Trim + " WHERE cn_status='A' AND c.contact_id = cn_user_contact_id AND cn_entry_date >= '" + month_six + "' AND cn_entry_date < '" + month_five + "') AS month_six_count,")
      sQuery.Append(" (SELECT COUNT(cn_id) FROM " + cloudNotes_databaseTable.Trim + " WHERE cn_status='A' AND c.contact_id = cn_user_contact_id) AS total_note_count,")
      sQuery.Append(" (SELECT COUNT(cn_id) FROM " + cloudNotes_databaseTable.Trim + " WHERE cn_status='P' AND c.contact_id = cn_user_contact_id) AS total_action_count")
      sQuery.Append(" FROM [jetnet_ra].[dbo].[Contact] AS c WHERE c.contact_comp_id = " + HttpContext.Current.Session.Item("localUser").crmUserCompanyID.ToString.Trim)
      sQuery.Append(Constants.cAndClause + "c.contact_journ_id = 0" + Constants.cAndClause + "c.contact_active_flag = 'Y'")
      sQuery.Append(" ORDER BY c.contact_last_name asc")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>get_notes_view_cloudNotes_summaryDataTable(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = cloudConnectStr

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
        aError = "Error in get_notes_view_cloudNotes_summaryDataTable load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in get_notes_view_cloudNotes_summaryDataTable(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

  Public Sub display_notes_view_summaryTable(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String)

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder
    Dim toggleRowColor As Boolean = False

    Dim cliuser_id As String = ""
    Dim client_firstName As String = ""
    Dim client_lastName As String = ""
    Dim month_one_count As Long = 0
    Dim month_two_count As Long = 0
    Dim month_three_count As Long = 0
    Dim month_four_count As Long = 0
    Dim month_five_count As Long = 0
    Dim month_six_count As Long = 0

    Dim total_notes As Long = 0
    Dim total_actions As Long = 0

    Dim tableTitle As String = ""

    Dim bHasStandardCloudNotes As Boolean = HttpContext.Current.Session.Item("localPreferences").HasCloudNotes
    Dim bHasServerNotes As Boolean = HttpContext.Current.Session.Item("localPreferences").HasServerNotes

    Try

      If bHasServerNotes Then 'If they're plus + notes users. 
        results_table = get_notes_view_notesPlus_summaryDataTable(searchCriteria)
        tableTitle = " Cloud Notes Plus "
      ElseIf bHasStandardCloudNotes Then 'if they're standard cloud users
        results_table = get_notes_view_cloudNotes_summaryDataTable(searchCriteria)
        tableTitle = " Cloud Notes "
      End If

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          htmlOut.Append("<table id=""notesViewSummaryDataTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"" border=""0"">")

          htmlOut.Append("<tr><td colspan=""9"" align=""center""><b>" + tableTitle.Trim + " Summary List <em>(users that have AT LEAST one note)</em></b></td></tr>")

          htmlOut.Append("<tr><td align=""left""><b>User</b></td>")
          For Counter = 6 To 1 Step -1
            htmlOut.Append("<td align=""left""><b>" + MonthName(Month(DateAdd(DateInterval.Month, -Counter, Now()))) + "</b></td>")
          Next
          htmlOut.Append("<td align=""left""><b>Total Notes</b></td>")
          htmlOut.Append("<td align=""left""><b>Action Items</b></td>")
          htmlOut.Append("</tr>")

          For Each r As DataRow In results_table.Rows   'cliuser_id

            If Not IsDBNull(r.Item("cliuser_id")) Then
              If Not String.IsNullOrEmpty(r.Item("cliuser_id").ToString.Trim) Then
                cliuser_id = r.Item("cliuser_id").ToString.Trim
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

            If Not IsDBNull(r.Item("month_one_count")) Then
              If Not String.IsNullOrEmpty(r.Item("month_one_count").ToString.Trim) Then
                month_one_count = CLng(r.Item("month_one_count").ToString.Trim)
              End If
            End If

            If Not IsDBNull(r.Item("month_two_count")) Then
              If Not String.IsNullOrEmpty(r.Item("month_two_count").ToString.Trim) Then
                month_two_count = CLng(r.Item("month_two_count").ToString.Trim)
              End If
            End If

            If Not IsDBNull(r.Item("month_three_count")) Then
              If Not String.IsNullOrEmpty(r.Item("month_three_count").ToString.Trim) Then
                month_three_count = CLng(r.Item("month_three_count").ToString.Trim)
              End If
            End If

            If Not IsDBNull(r.Item("month_four_count")) Then
              If Not String.IsNullOrEmpty(r.Item("month_four_count").ToString.Trim) Then
                month_four_count = CLng(r.Item("month_four_count").ToString.Trim)
              End If
            End If

            If Not IsDBNull(r.Item("month_five_count")) Then
              If Not String.IsNullOrEmpty(r.Item("month_five_count").ToString.Trim) Then
                month_five_count = CLng(r.Item("month_five_count").ToString.Trim)
              End If
            End If

            If Not IsDBNull(r.Item("month_six_count")) Then
              If Not String.IsNullOrEmpty(r.Item("month_six_count").ToString.Trim) Then
                month_six_count = CLng(r.Item("month_six_count").ToString.Trim)
              End If
            End If

            If Not IsDBNull(r.Item("total_note_count")) Then
              If Not String.IsNullOrEmpty(r.Item("total_note_count").ToString.Trim) Then
                total_notes = CLng(r.Item("total_note_count").ToString.Trim)
              End If
            End If

            If Not IsDBNull(r.Item("total_action_count")) Then
              If Not String.IsNullOrEmpty(r.Item("total_action_count").ToString.Trim) Then
                total_actions = CLng(r.Item("total_action_count").ToString.Trim)
              End If
            End If

            If total_notes + total_actions > 0 Then

              If Not toggleRowColor Then
                htmlOut.Append("<tr class=""alt_row"">")
                toggleRowColor = True
              Else
                htmlOut.Append("<tr bgcolor=""white"">")
                toggleRowColor = False
              End If

              htmlOut.Append("<td align=""left"" valign=""middle""><img src=""/images/user_male_gray.png"" width=""16"" title=""UID : " + cliuser_id.Trim + """ alt=""UID : " + cliuser_id.Trim + """ style=""padding-top:2px;"" />&nbsp;" + client_firstName.Trim + Constants.cHTMLnbsp + client_lastName.Trim + "</td>")
              htmlOut.Append("<td align=""left"">" + month_one_count.ToString.Trim + "</td>")
              htmlOut.Append("<td align=""left"">" + month_two_count.ToString.Trim + "</td>")
              htmlOut.Append("<td align=""left"">" + month_three_count.ToString.Trim + "</td>")
              htmlOut.Append("<td align=""left"">" + month_four_count.ToString.Trim + "</td>")
              htmlOut.Append("<td align=""left"">" + month_five_count.ToString.Trim + "</td>")
              htmlOut.Append("<td align=""left"">" + month_six_count.ToString.Trim + "</td>")
              htmlOut.Append("<td align=""left"">" + total_notes.ToString.Trim + "</td>")
              htmlOut.Append("<td align=""left"">" + total_actions.ToString.Trim + "</td>")

              htmlOut.Append("</tr>")

            End If

          Next

          htmlOut.Append("</table>")

        Else
          htmlOut.Append("<table id=""notesViewSummaryDataTable"" width=""100%"" cellpadding=""2"" cellspacing=""0""><tr><td valign=""top"" align=""left""><br/>No " + tableTitle.Trim + " Summary Found</td></tr></table>")
        End If
      Else
        htmlOut.Append("<table id=""notesViewSummaryDataTable"" width=""100%"" cellpadding=""2"" cellspacing=""0""><tr><td valign=""top"" align=""left""><br/>No " + tableTitle.Trim + " Summary Found</td></tr></table>")
      End If

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in display_notes_view_summaryTable(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

    Finally

    End Try

    'return resulting html string
    out_htmlString = htmlOut.ToString
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Function get_notes_view_notesPlus_UserList(ByRef searchCriteria As viewSelectionCriteriaClass, Optional ByRef ytSearchCriteria As yachtViewSelectionCriteria = Nothing) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
    Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
    Dim MySqlReader As MySql.Data.MySqlClient.MySqlDataReader = Nothing
    Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing

    Try

      Select Case CType(HttpContext.Current.Session.Item("jetnetWebHostType"), crmWebClient.eWebHostTypes)

        Case eWebHostTypes.YACHT
          sQuery.Append("SELECT cliuser_id, cliuser_first_name, cliuser_last_name, cliuser_email_address FROM client_user")
          sQuery.Append(" WHERE cliuser_active_flag = 'Y' ORDER BY cliuser_last_name ASC")

          ' only get users that have at least one "note"
          'sQuery.Append(" WHERE cliuser_active_flag = 'Y' AND (SELECT COUNT(lnote_id) FROM local_notes WHERE lnote_status='A' AND cliuser_id = lnote_user_id) > 1 ORDER BY cliuser_last_name ASC")

        Case eWebHostTypes.EVOLUTION
          sQuery.Append("SELECT cliuser_id, cliuser_first_name, cliuser_last_name, cliuser_email_address FROM client_user")
          sQuery.Append(" WHERE cliuser_active_flag = 'Y' ORDER BY cliuser_last_name ASC")

          ' only get users that have at least one "note"
          'sQuery.Append(" WHERE cliuser_active_flag = 'Y' AND (SELECT COUNT(lnote_id) FROM local_notes WHERE lnote_status='A' AND cliuser_id = lnote_user_id) > 1 ORDER BY cliuser_last_name ASC")

      End Select

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>get_notes_view_notesPlus_UserList(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

      MySqlConn.ConnectionString = serverConnectStr

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
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in get_notes_view_notesPlus_UserList load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in get_notes_view_notesPlus_UserList(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

  Public Function get_notes_view_cloudNotes_UserList(ByRef searchCriteria As viewSelectionCriteriaClass, Optional ByRef ytSearchCriteria As yachtViewSelectionCriteria = Nothing) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try


      Dim cloudNotes_databaseTable As String = HttpContext.Current.Session.Item("localPreferences").CloudNotesDatabaseName.ToString.Trim

      Select Case CType(HttpContext.Current.Session.Item("jetnetWebHostType"), crmWebClient.eWebHostTypes)

        Case eWebHostTypes.YACHT
          sQuery.Append("SELECT c.contact_id AS cliuser_id, c.contact_first_name AS cliuser_first_name, c.contact_last_name AS cliuser_last_name, c.contact_email_address AS cliuser_email_address")
          sQuery.Append(" FROM [jetnet_ra].[dbo].[Contact] AS c WHERE c.contact_comp_id = " + HttpContext.Current.Session.Item("localUser").crmUserCompanyID.ToString.Trim)
          sQuery.Append(Constants.cAndClause + "c.contact_journ_id = 0" + Constants.cAndClause + "c.contact_active_flag = 'Y'")
          'sQuery.Append(Constants.cAndClause + "(SELECT COUNT(cn_id) FROM " + cloudNotes_databaseTable.Trim + " WHERE cn_status='A' AND c.contact_id = cn_user_contact_id) > 1")
          sQuery.Append(" ORDER BY c.contact_last_name ASC")

        Case eWebHostTypes.EVOLUTION
          sQuery.Append("SELECT c.contact_id AS cliuser_id, c.contact_first_name AS cliuser_first_name, c.contact_last_name AS cliuser_last_name, c.contact_email_address AS cliuser_email_address")
          sQuery.Append(" FROM [jetnet_ra].[dbo].[Contact] AS c WHERE c.contact_comp_id = " + HttpContext.Current.Session.Item("localUser").crmUserCompanyID.ToString.Trim)
          sQuery.Append(Constants.cAndClause + "c.contact_journ_id = 0" + Constants.cAndClause + "c.contact_active_flag = 'Y'")
          'sQuery.Append(Constants.cAndClause + "(SELECT COUNT(cn_id) FROM " + cloudNotes_databaseTable.Trim + " WHERE cn_status='A' AND c.contact_id = cn_user_contact_id) > 1")
          sQuery.Append(" ORDER BY c.contact_last_name ASC")

      End Select

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>get_notes_view_cloudNotes_UserList(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = cloudConnectStr

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
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in get_notes_view_cloudNotes_UserList load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in get_notes_view_cloudNotes_UserList(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

  Public Sub views_fill_notesUserDropdown(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef maxWidth As Long, ByRef notesSearchWho As DropDownList, Optional ByRef ytSearchCriteria As yachtViewSelectionCriteria = Nothing)

    Dim results_table As New DataTable
    Dim userName As String = ""
    Dim userID As Long = 0

    Dim firstName As String = ""
    Dim lastName As String = ""

    Dim bHasStandardCloudNotes As Boolean = HttpContext.Current.Session.Item("localPreferences").HasCloudNotes
    Dim bHasServerNotes As Boolean = HttpContext.Current.Session.Item("localPreferences").HasServerNotes

    Try

      notesSearchWho.Items.Clear()
      notesSearchWho.Items.Add(New ListItem("All", "-1"))

      If bHasServerNotes Then 'If they're plus + notes users. 
        results_table = get_notes_view_notesPlus_UserList(searchCriteria, ytSearchCriteria)
      ElseIf bHasStandardCloudNotes Then 'if they're standard cloud users
        results_table = get_notes_view_cloudNotes_UserList(searchCriteria, ytSearchCriteria)
      End If

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          For Each r As DataRow In results_table.Rows

            If Not IsDBNull(r.Item("cliuser_id")) Then
              If Not String.IsNullOrEmpty(r.Item("cliuser_id").ToString.Trim) Then
                If IsNumeric(r.Item("cliuser_id").ToString.Trim) Then
                  userID = CLng(r.Item("cliuser_id").ToString.Trim)
                End If
              End If
            End If

            If Not IsDBNull(r.Item("cliuser_first_name")) Then
              If Not String.IsNullOrEmpty(r.Item("cliuser_first_name").ToString.Trim) Then
                firstName = r.Item("cliuser_first_name").ToString.Trim
              End If
            End If

            If Not IsDBNull(r.Item("cliuser_last_name")) Then
              If Not String.IsNullOrEmpty(r.Item("cliuser_last_name").ToString.Trim) Then
                lastName = r.Item("cliuser_last_name").ToString.Trim
              End If
            End If

            userName = firstName.Trim + Constants.cSingleSpace + lastName.Trim

            If (userName.Length * Constants._STARTCHARWIDTH) > maxWidth Then
              maxWidth = (userName.Length * Constants._STARTCHARWIDTH)
            End If

            notesSearchWho.Items.Add(New ListItem(userName, userID.ToString))

            If Not IsNothing(searchCriteria) Then
              If searchCriteria.ViewCriteriaNoteUserID = userID Then
                notesSearchWho.SelectedValue = userID.ToString
              End If
            ElseIf Not IsNothing(ytSearchCriteria) Then
              If ytSearchCriteria.YachtViewCriteriaNoteUserID = userID Then
                notesSearchWho.SelectedValue = userID.ToString
              End If
            End If

          Next
        End If
      End If


      results_table = Nothing

      notesSearchWho.Width = (maxWidth)

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in views_fill_notesUserDropdown(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef maxWidth As Long, ByRef notesSearchWho As DropDownList) " + ex.Message

    Finally

    End Try

    results_table = Nothing

  End Sub

  Public Function write_notesReport_string_to_file(ByVal sOutoutString As String, ByVal sReportname As String) As Boolean

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

End Class
