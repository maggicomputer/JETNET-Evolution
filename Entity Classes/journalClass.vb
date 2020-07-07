' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/Entity Classes/journalClass.vb $
'$$Author: Mike $
'$$Date: 3/19/20 1:47p $
'$$Modtime: 3/19/20 1:03p $
'$$Revision: 22 $
'$$Workfile: journalClass.vb $
'
' ********************************************************************************

<System.Serializable()> Public Class journalClass

  Public Property journ_id() As Long
  Public Property journ_date() As String
  Public Property journ_end_date() As String
  Public Property journ_subcategory_code() As String
  Public Property journ_subject() As String
  Public Property journ_description() As String
  Public Property journ_ac_id() As Long
  Public Property journ_contact_id() As Long
  Public Property journ_comp_id() As Long
  Public Property journ_user_id() As String
  Public Property journ_entry_date() As String
  Public Property journ_entry_time() As String
  Public Property journ_account_id() As String
  Public Property journ_prior_account_id() As String
  Public Property journ_status() As String
  Public Property journ_customer_note() As String
  Public Property journ_action_date() As String
  Public Property journ_pcreckey() As Integer
  Public Property journ_fractowr_id() As Long
  Public Property journ_fractsld_id() As Long
  Public Property journ_newac_flag() As Boolean
  Public Property journ_internal_trans_flag() As Boolean
  Public Property journ_subcat_code_part1() As String
  Public Property journ_subcat_code_part2() As String
  Public Property journ_subcat_code_part3() As String
  Public Property journ_yacht_id() As Long
  Public Property journ_amod_id() As Long

  Public Property resultList As New List(Of journalClass)
  Public Property resultsTable As New DataTable

  Public Sub New()
    journ_id = 0
    journ_date = ""
    journ_end_date = ""
    journ_subcategory_code = ""
    journ_subject = ""
    journ_description = ""
    journ_ac_id = 0
    journ_contact_id = 0
    journ_comp_id = 0
    journ_user_id = ""
    journ_entry_date = ""
    journ_entry_time = ""
    journ_account_id = ""
    journ_prior_account_id = ""
    journ_status = ""
    journ_customer_note = ""
    journ_action_date = ""
    journ_pcreckey = 0
    journ_fractowr_id = 0
    journ_fractsld_id = 0
    journ_newac_flag = False
    journ_internal_trans_flag = False
    journ_subcat_code_part1 = ""
    journ_subcat_code_part2 = ""
    journ_subcat_code_part3 = ""
    journ_yacht_id = 0
    journ_amod_id = 0
  End Sub

  Public Sub New(journid As Long, journdate As String, journenddate As String, journsubcategorycode As String,
                 journsubject As String, journdescription As String, journacid As Long,
                 journcontactid As Long, journcompid As Long, journuserid As String,
                 journentrydate As String, journentrytime As String, journaccountid As String,
                 journprioraccountid As String, journstatus As String, journcustomernote As String,
                 journactiondate As String, journpcreckey As Integer, journfractowrid As Long,
                 journfractsldid As Long, journnewacflag As Boolean, journinternaltransflag As Boolean,
                 journsubcatcodepart1 As String, journsubcatcodepart2 As String, journsubcatcodepart3 As String,
                 journyachtid As Long, journamodid As Long)
    journ_id = journid
    journ_date = journdate
    journ_end_date = journenddate
    journ_subcategory_code = journsubcategorycode
    journ_subject = journsubject
    journ_description = journdescription
    journ_ac_id = journacid
    journ_contact_id = journcontactid
    journ_comp_id = journcompid
    journ_user_id = journuserid
    journ_entry_date = journentrydate
    journ_entry_time = journentrytime
    journ_account_id = journaccountid
    journ_prior_account_id = journprioraccountid
    journ_status = journstatus
    journ_customer_note = journcustomernote
    journ_action_date = journactiondate
    journ_pcreckey = journpcreckey
    journ_fractowr_id = journfractowrid
    journ_fractsld_id = journfractsldid
    journ_newac_flag = journnewacflag
    journ_internal_trans_flag = journinternaltransflag
    journ_subcat_code_part1 = journsubcatcodepart1
    journ_subcat_code_part2 = journsubcatcodepart2
    journ_subcat_code_part3 = journsubcatcodepart3
    journ_yacht_id = journyachtid
    journ_amod_id = journamodid
  End Sub

  Public Function getJournalDataTable() As DataTable

    Dim atemptable As New DataTable
    Dim journalQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sSeperator = ""

    Try

      journalQuery.Append("SELECT * FROM " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Journal WITH(NOLOCK) ")
      journalQuery.Append("LEFT OUTER JOIN " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Contact WITH(NOLOCK) ON contact_id = journ_contact_id AND contact_journ_id = 0 AND contact_hide_flag = 'N' ")
      journalQuery.Append("LEFT OUTER JOIN " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Company WITH(NOLOCK) ON comp_id = journ_comp_id AND comp_journ_id = 0 AND comp_hide_flag = 'N' ")
      journalQuery.Append("LEFT OUTER JOIN " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "[user] WITH(NOLOCK) ON [user_id] = journ_user_id AND user_email_address <> '' AND user_password <> 'inactive' ")
      journalQuery.Append("WHERE ")

      If journ_id > 0 Then
        journalQuery.Append("journ_id = @journ_id")
        SqlCommand.Parameters.AddWithValue("@journ_id", journ_id.ToString.Trim)
        sSeperator = " AND "
      End If

      If Not String.IsNullOrEmpty(journ_date.Trim) Then
        journalQuery.Append(sSeperator + "journ_date >= @journ_date")
        SqlCommand.Parameters.AddWithValue("@journ_date", journ_date.Trim)
        sSeperator = " AND "
      End If

      If Not String.IsNullOrEmpty(journ_end_date.Trim) Then
        journalQuery.Append(sSeperator + "journ_date <= @journ_end_date")
        SqlCommand.Parameters.AddWithValue("@journ_end_date", journ_end_date.Trim)
        sSeperator = " AND "
      End If

      If Not String.IsNullOrEmpty(journ_subcategory_code.Trim) Then
        journalQuery.Append(sSeperator + "journ_subcategory_code IN (@journ_subcategory_code)")
        SqlCommand.Parameters.AddWithValue("@journ_subcategory_code", journ_subcategory_code.Trim)
        sSeperator = " AND "
      End If

      If Not String.IsNullOrEmpty(journ_subject.Trim) Then
        journalQuery.Append(sSeperator + "journ_subject LIKE(@journ_subject)")
        SqlCommand.Parameters.AddWithValue("@journ_subject", "%" + journ_subject.Trim + "%")
        sSeperator = " AND "
      End If

      If Not String.IsNullOrEmpty(journ_description.Trim) Then
        journalQuery.Append(sSeperator + "journ_description LIKE(@journ_description)")
        SqlCommand.Parameters.AddWithValue("@journ_description", "%" + journ_description.Trim + "%")
        sSeperator = " AND "
      End If

      If journ_ac_id > 0 Then
        journalQuery.Append(sSeperator + "journ_ac_id = @journ_ac_id")
        SqlCommand.Parameters.AddWithValue("@journ_ac_id", journ_ac_id.ToString.Trim)
        sSeperator = " AND "
      End If

      If journ_contact_id > 0 Then
        journalQuery.Append(sSeperator + "journ_contact_id = @journ_contact_id")
        SqlCommand.Parameters.AddWithValue("@journ_contact_id", journ_contact_id.ToString.Trim)
        sSeperator = " AND "
      End If

      If journ_comp_id > 0 Then
        journalQuery.Append(sSeperator + "journ_comp_id = @journ_comp_id")
        SqlCommand.Parameters.AddWithValue("@journ_comp_id", journ_comp_id.ToString.Trim)
        sSeperator = " AND "
      End If

      If Not String.IsNullOrEmpty(journ_user_id.Trim) Then
        journalQuery.Append(sSeperator + "journ_user_id LIKE(@journ_user_id)")
        SqlCommand.Parameters.AddWithValue("@journ_user_id", journ_user_id.Trim)
        sSeperator = " AND "
      End If

      If Not String.IsNullOrEmpty(journ_entry_date.Trim) Then
        journalQuery.Append(sSeperator + "journ_entry_date >= @journ_entry_date")
        SqlCommand.Parameters.AddWithValue("@journ_entry_date", journ_entry_date.Trim)
        sSeperator = " AND "
      End If

      If Not String.IsNullOrEmpty(journ_entry_time.Trim) Then
        journalQuery.Append(sSeperator + "journ_entry_time >= @journ_entry_time")
        SqlCommand.Parameters.AddWithValue("@journ_entry_time", journ_entry_time.Trim)
        sSeperator = " AND "
      End If

      If Not String.IsNullOrEmpty(journ_account_id.Trim) Then
        journalQuery.Append(sSeperator + "journ_account_id LIKE(@journ_account_id)")
        SqlCommand.Parameters.AddWithValue("@journ_account_id", "%" + journ_account_id.Trim + "%")
        sSeperator = " AND "
      End If

      If Not String.IsNullOrEmpty(journ_prior_account_id.Trim) Then
        journalQuery.Append(sSeperator + "journ_prior_account_id LIKE(@journ_prior_account_id)")
        SqlCommand.Parameters.AddWithValue("@journ_prior_account_id", "%" + journ_prior_account_id.Trim + "%")
        sSeperator = " AND "
      End If

      If Not String.IsNullOrEmpty(journ_status.Trim) Then
        journalQuery.Append(sSeperator + "journ_status LIKE(@journ_status)")
        SqlCommand.Parameters.AddWithValue("@journ_status", "%" + journ_status.Trim + "%")
        sSeperator = " AND "
      End If

      If Not String.IsNullOrEmpty(journ_customer_note.Trim) Then
        journalQuery.Append(sSeperator + "journ_customer_note LIKE(@journ_customer_note)")
        SqlCommand.Parameters.AddWithValue("@journ_customer_note", "%" + journ_customer_note.Trim + "%")
        sSeperator = " AND "
      End If

      If Not String.IsNullOrEmpty(journ_action_date.Trim) Then
        journalQuery.Append(sSeperator + "journ_action_date >= @journ_action_date")
        SqlCommand.Parameters.AddWithValue("@journ_action_date", journ_action_date.Trim)
        sSeperator = " AND "
      End If

      If journ_pcreckey > 0 Then
        journalQuery.Append(sSeperator + "journ_pcreckey = @journ_pcreckey")
        SqlCommand.Parameters.AddWithValue("@journ_pcreckey", journ_pcreckey.ToString.Trim)
        sSeperator = " AND "
      End If

      If journ_fractowr_id > 0 Then
        journalQuery.Append(sSeperator + "journ_fractowr_id = @journ_fractowr_id")
        SqlCommand.Parameters.AddWithValue("@journ_fractowr_id", journ_fractowr_id.ToString.Trim)
        sSeperator = " AND "
      End If

      If journ_fractsld_id > 0 Then
        journalQuery.Append(sSeperator + "journ_fractsld_id = @journ_fractsld_id")
        SqlCommand.Parameters.AddWithValue("@journ_fractsld_id", journ_fractsld_id.ToString.Trim)
        sSeperator = " AND "
      End If

      journalQuery.Append(sSeperator + "journ_newac_flag = @journ_newac_flag")
      SqlCommand.Parameters.AddWithValue("@journ_newac_flag", IIf(journ_newac_flag, "Y", "N"))
      sSeperator = " AND "

      journalQuery.Append(sSeperator + "journ_internal_trans_flag = @journ_internal_trans_flag")
      SqlCommand.Parameters.AddWithValue("@journ_internal_trans_flag", IIf(journ_internal_trans_flag, "Y", "N"))
      sSeperator = " AND "

      If Not String.IsNullOrEmpty(journ_subcat_code_part1.Trim) Then
        journalQuery.Append(sSeperator + "journ_subcat_code_part1 IN (@journ_subcat_code_part1)")
        SqlCommand.Parameters.AddWithValue("@journ_subcat_code_part1", journ_subcat_code_part1.Trim)
        sSeperator = " AND "
      End If

      If Not String.IsNullOrEmpty(journ_subcat_code_part2.Trim) Then
        journalQuery.Append(sSeperator + "journ_subcat_code_part2 IN (@journ_subcat_code_part2)")
        SqlCommand.Parameters.AddWithValue("@journ_subcat_code_part2", journ_subcat_code_part2.Trim)
        sSeperator = " AND "
      End If

      If Not String.IsNullOrEmpty(journ_subcat_code_part3.Trim) Then
        journalQuery.Append(sSeperator + "journ_subcat_code_part3 IN (@journ_subcat_code_part3)")
        SqlCommand.Parameters.AddWithValue("@journ_subcat_code_part3", journ_subcat_code_part3.Trim)
      End If

      If journ_yacht_id > 0 Then
        journalQuery.Append(sSeperator + "journ_yacht_id = @journ_yacht_id")
        SqlCommand.Parameters.AddWithValue("@journ_yacht_id", journ_yacht_id.ToString.Trim)
        sSeperator = " AND "
      End If

      If journ_amod_id > 0 Then
        journalQuery.Append(sSeperator + "journ_amod_id = @journ_amod_id")
        SqlCommand.Parameters.AddWithValue("@journ_amod_id", journ_amod_id.ToString.Trim)
      End If

      journalQuery.Append(" ORDER BY journ_date ASC, journ_id ASC")

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 90

      SqlCommand.CommandText = journalQuery.ToString

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + journalQuery.ToString

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()

        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + constrExc.Message

        Return Nothing

      End Try

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message
      Return Nothing

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return atemptable

  End Function

  Public Sub fillJournalClass(Optional bDataTableOnly As Boolean = False)

    Try

      resultsTable = getJournalDataTable()

      If Not IsNothing(resultsTable) And Not bDataTableOnly Then

        If resultsTable.Rows.Count > 0 Then

          For Each r As DataRow In resultsTable.Rows

            Dim journalRec As New journalClass

            If Not (IsDBNull(r("journ_id"))) Then
              If Not String.IsNullOrEmpty(r.Item("journ_id").ToString.Trim) Then
                If IsNumeric(r.Item("journ_id").ToString.Trim) Then
                  journalRec.journ_id = CLng(r.Item("journ_id").ToString.Trim)
                End If
              End If
            End If

            If Not (IsDBNull(r("journ_date"))) Then
              journalRec.journ_date = r.Item("journ_date").ToString.Trim
            End If

            If Not (IsDBNull(r("journ_subcategory_code"))) Then
              journalRec.journ_subcategory_code = r.Item("journ_subcategory_code").ToString.Trim
            End If

            If Not (IsDBNull(r("journ_subject"))) Then
              journalRec.journ_subject = r.Item("journ_subject").ToString.Trim
            End If

            If Not (IsDBNull(r("journ_description"))) Then
              journalRec.journ_description = r.Item("journ_description").ToString.Trim
            End If

            If Not (IsDBNull(r("journ_ac_id"))) Then
              If Not String.IsNullOrEmpty(r.Item("journ_ac_id").ToString.Trim) Then
                If IsNumeric(r.Item("journ_ac_id").ToString.Trim) Then
                  journalRec.journ_ac_id = CLng(r.Item("journ_ac_id").ToString.Trim)
                End If
              End If
            End If

            If Not (IsDBNull(r("journ_contact_id"))) Then
              If Not String.IsNullOrEmpty(r.Item("journ_contact_id").ToString.Trim) Then
                If IsNumeric(r.Item("journ_contact_id").ToString.Trim) Then
                  journalRec.journ_contact_id = CLng(r.Item("journ_contact_id").ToString.Trim)
                End If
              End If
            End If

            If Not (IsDBNull(r("journ_comp_id"))) Then
              If Not String.IsNullOrEmpty(r.Item("journ_comp_id").ToString.Trim) Then
                If IsNumeric(r.Item("journ_comp_id").ToString.Trim) Then
                  journalRec.journ_comp_id = CLng(r.Item("journ_comp_id").ToString.Trim)
                End If
              End If
            End If

            If Not (IsDBNull(r("journ_user_id"))) Then
              journalRec.journ_user_id = r.Item("journ_user_id").ToString.Trim
            End If

            If Not (IsDBNull(r("journ_entry_date"))) Then
              journalRec.journ_entry_date = r.Item("journ_entry_date").ToString.Trim
            End If

            If Not (IsDBNull(r("journ_entry_time"))) Then
              journalRec.journ_entry_time = r.Item("journ_entry_time").ToString.Trim
            End If

            If Not (IsDBNull(r("journ_account_id"))) Then
              journalRec.journ_account_id = r.Item("journ_account_id").ToString.Trim
            End If

            If Not (IsDBNull(r("journ_prior_account_id"))) Then
              journalRec.journ_prior_account_id = r.Item("journ_prior_account_id").ToString.Trim
            End If

            If Not (IsDBNull(r("journ_status"))) Then
              journalRec.journ_status = r.Item("journ_status").ToString.Trim
            End If

            If Not (IsDBNull(r("journ_customer_note"))) Then
              journalRec.journ_customer_note = r.Item("journ_customer_note").ToString.Trim
            End If

            If Not (IsDBNull(r("journ_action_date"))) Then
              journalRec.journ_action_date = r.Item("journ_action_date").ToString.Trim
            End If

            If Not (IsDBNull(r("journ_pcreckey"))) Then
              If Not String.IsNullOrEmpty(r.Item("journ_pcreckey").ToString.Trim) Then
                If IsNumeric(r.Item("journ_pcreckey").ToString.Trim) Then
                  journalRec.journ_pcreckey = CInt(r.Item("journ_pcreckey").ToString.Trim)
                End If
              End If
            End If

            If Not (IsDBNull(r("journ_fractowr_id"))) Then
              If Not String.IsNullOrEmpty(r.Item("journ_fractowr_id").ToString.Trim) Then
                If IsNumeric(r.Item("journ_fractowr_id").ToString.Trim) Then
                  journalRec.journ_fractowr_id = CLng(r.Item("journ_fractowr_id").ToString.Trim)
                End If
              End If
            End If

            If Not (IsDBNull(r("journ_fractsld_id"))) Then
              If Not String.IsNullOrEmpty(r.Item("journ_fractsld_id").ToString.Trim) Then
                If IsNumeric(r.Item("journ_fractsld_id").ToString.Trim) Then
                  journalRec.journ_fractsld_id = CLng(r.Item("journ_fractsld_id").ToString.Trim)
                End If
              End If
            End If

            If Not (IsDBNull(r("journ_newac_flag"))) Then
              journalRec.journ_newac_flag = IIf(r.Item("journ_newac_flag").ToString.ToUpper.Contains("Y"), True, False)
            End If

            If Not (IsDBNull(r("journ_internal_trans_flag"))) Then
              journalRec.journ_internal_trans_flag = IIf(r.Item("journ_internal_trans_flag").ToString.ToUpper.Contains("Y"), True, False)
            End If

            If Not (IsDBNull(r("journ_subcat_code_part1"))) Then
              journalRec.journ_subcat_code_part1 = r.Item("journ_subcat_code_part1").ToString.Trim
            End If

            If Not (IsDBNull(r("journ_subcat_code_part2"))) Then
              journalRec.journ_subcat_code_part2 = r.Item("journ_subcat_code_part2").ToString.Trim
            End If

            If Not (IsDBNull(r("journ_subcat_code_part3"))) Then
              journalRec.journ_subcat_code_part3 = r.Item("journ_subcat_code_part3").ToString.Trim
            End If

            If Not (IsDBNull(r("journ_yacht_id"))) Then
              If Not String.IsNullOrEmpty(r.Item("journ_yacht_id").ToString.Trim) Then
                If IsNumeric(r.Item("journ_yacht_id").ToString.Trim) Then
                  journalRec.journ_yacht_id = CLng(r.Item("journ_yacht_id").ToString.Trim)
                End If
              End If
            End If

            If Not (IsDBNull(r("journ_amod_id"))) Then
              If Not String.IsNullOrEmpty(r.Item("journ_amod_id").ToString.Trim) Then
                If IsNumeric(r.Item("journ_amod_id").ToString.Trim) Then
                  journalRec.journ_amod_id = CLng(r.Item("journ_amod_id").ToString.Trim)
                End If
              End If
            End If

            resultList.Add(journalRec)

          Next

        End If

      End If

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message

    End Try

  End Sub

  Public Sub updateJournalRecord(Optional bUseStringQuery As Boolean = False)
    Dim journalQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand

    Dim sSeperator = ""
    Try

      If journ_id = 0 Then
        Exit Sub
      End If

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 90

      If Not bUseStringQuery Then

        journalQuery.Append("UPDATE " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[HomeBase].jetnet_ra.dbo.", "") + "Journal SET")

        If Not String.IsNullOrEmpty(journ_date.Trim) Then
          journalQuery.Append(sSeperator + " journ_date = @journ_date")
          SqlCommand.Parameters.AddWithValue("@journ_date", FormatDateTime(journ_date.Trim, DateFormat.GeneralDate))
          sSeperator = ","
        ElseIf String.IsNullOrEmpty(journ_date.Trim) Then
          journalQuery.Append(sSeperator + " journ_date = @journ_date")
          SqlCommand.Parameters.AddWithValue("@journ_date", FormatDateTime(Now, DateFormat.GeneralDate))
          sSeperator = ","
        End If

        If Not String.IsNullOrEmpty(journ_subcategory_code.Trim) Then
          journalQuery.Append(sSeperator + " journ_subcategory_code = @journ_subcategory_code")
          SqlCommand.Parameters.AddWithValue("@journ_subcategory_code", journ_subcategory_code.Trim)
          sSeperator = ","
        End If

        If Not String.IsNullOrEmpty(journ_subject.Trim) Then
          journalQuery.Append(sSeperator + " journ_subject = @journ_subject")
          SqlCommand.Parameters.AddWithValue("@journ_subject", journ_subject.Trim)
          sSeperator = ","
        End If

        If Not String.IsNullOrEmpty(journ_description.Trim) Then
          journalQuery.Append(sSeperator + " journ_description = @journ_description")
          SqlCommand.Parameters.AddWithValue("@journ_description", journ_description.Trim)
          sSeperator = ","
        ElseIf String.IsNullOrEmpty(journ_description.Trim) Then
          journalQuery.Append(sSeperator + " journ_description = @journ_description")
          SqlCommand.Parameters.AddWithValue("@journ_description", DBNull.Value)
          sSeperator = ","
        End If

        If journ_ac_id > 0 Then
          journalQuery.Append(sSeperator + " journ_ac_id = @journ_ac_id")
          SqlCommand.Parameters.AddWithValue("@journ_ac_id", journ_ac_id.ToString.Trim)
          sSeperator = ","
        ElseIf journ_ac_id = 0 Then
          journalQuery.Append(sSeperator + " journ_ac_id = 0")
          sSeperator = ","
        End If

        If journ_contact_id > 0 Then
          journalQuery.Append(sSeperator + " journ_contact_id = @journ_contact_id")
          SqlCommand.Parameters.AddWithValue("@journ_contact_id", journ_contact_id.ToString.Trim)
          sSeperator = ","
        ElseIf journ_contact_id = 0 Then
          journalQuery.Append(sSeperator + " journ_contact_id = 0")
          sSeperator = ","
        End If

        If journ_comp_id > 0 Then
          journalQuery.Append(sSeperator + " journ_comp_id = @journ_comp_id")
          SqlCommand.Parameters.AddWithValue("@journ_comp_id", journ_comp_id.ToString.Trim)
          sSeperator = ","
        ElseIf journ_comp_id = 0 Then
          journalQuery.Append(sSeperator + " journ_comp_id = 0")
          sSeperator = ","
        End If

        If Not String.IsNullOrEmpty(journ_user_id.Trim) Then
          journalQuery.Append(sSeperator + " journ_user_id = @journ_user_id")
          SqlCommand.Parameters.AddWithValue("@journ_user_id", journ_user_id.Trim)
          sSeperator = ","
        ElseIf String.IsNullOrEmpty(journ_user_id.Trim) Then
          journalQuery.Append(sSeperator + " journ_user_id = @journ_user_id")
          SqlCommand.Parameters.AddWithValue("@journ_user_id", DBNull.Value)
          sSeperator = ","
        End If

        If Not String.IsNullOrEmpty(journ_entry_date.Trim) Then
          journalQuery.Append(sSeperator + " journ_entry_date = @journ_entry_date")
          SqlCommand.Parameters.AddWithValue("@journ_entry_date", FormatDateTime(journ_entry_date.Trim, DateFormat.ShortDate))
          sSeperator = ","
        ElseIf String.IsNullOrEmpty(journ_entry_date.Trim) Then
          journalQuery.Append(sSeperator + " journ_entry_date = @journ_entry_date")
          SqlCommand.Parameters.AddWithValue("@journ_entry_date", FormatDateTime(Now, DateFormat.ShortDate))
          sSeperator = ","
        End If

        If Not String.IsNullOrEmpty(journ_entry_time.Trim) Then
          journalQuery.Append(sSeperator + " journ_entry_time = @journ_entry_time")
          SqlCommand.Parameters.AddWithValue("@journ_entry_time", FormatDateTime(journ_entry_time.Trim, DateFormat.LongTime))
          sSeperator = ","
        ElseIf String.IsNullOrEmpty(journ_entry_time.Trim) Then
          journalQuery.Append(sSeperator + " journ_entry_time = @journ_entry_time")
          SqlCommand.Parameters.AddWithValue("@journ_entry_time", FormatDateTime(Now, DateFormat.LongTime))
          sSeperator = ","
        End If

        If Not String.IsNullOrEmpty(journ_account_id.Trim) Then
          journalQuery.Append(sSeperator + " journ_account_id = @journ_account_id")
          SqlCommand.Parameters.AddWithValue("@journ_account_id", journ_account_id.Trim)
          sSeperator = ","
        End If

        If Not String.IsNullOrEmpty(journ_prior_account_id.Trim) Then
          journalQuery.Append(sSeperator + " journ_prior_account_id = @journ_prior_account_id")
          SqlCommand.Parameters.AddWithValue("@journ_prior_account_id", journ_prior_account_id.Trim)
          sSeperator = ","
        End If

        If Not String.IsNullOrEmpty(journ_status.Trim) Then
          journalQuery.Append(sSeperator + " journ_status = @journ_status")
          SqlCommand.Parameters.AddWithValue("@journ_status", journ_status.Trim)
          sSeperator = ","
        End If

        If Not String.IsNullOrEmpty(journ_customer_note.Trim) Then
          journalQuery.Append(sSeperator + " journ_customer_note = @journ_customer_note")
          SqlCommand.Parameters.AddWithValue("@journ_customer_note", journ_customer_note.Trim)
          sSeperator = ","
        End If

        If journ_pcreckey > 0 Then
          journalQuery.Append(sSeperator + " journ_pcreckey = @journ_pcreckey")
          SqlCommand.Parameters.AddWithValue("@journ_pcreckey", journ_pcreckey.ToString.Trim)
          sSeperator = ","
        End If

        If journ_fractowr_id > 0 Then
          journalQuery.Append(sSeperator + " journ_fractowr_id = @journ_fractowr_id")
          SqlCommand.Parameters.AddWithValue("@journ_fractowr_id", journ_fractowr_id.ToString.Trim)
          sSeperator = ","
        End If

        If journ_fractsld_id > 0 Then
          journalQuery.Append(sSeperator + " journ_fractsld_id = @journ_fractsld_id")
          SqlCommand.Parameters.AddWithValue("@journ_fractsld_id", journ_fractsld_id.ToString.Trim)
          sSeperator = ","
        End If

        journalQuery.Append(sSeperator + " journ_newac_flag = @journ_newac_flag")
        SqlCommand.Parameters.AddWithValue("@journ_newac_flag", IIf(journ_newac_flag, "Y", "N"))
        sSeperator = ","

        journalQuery.Append(sSeperator + " journ_internal_trans_flag = @journ_internal_trans_flag")
        SqlCommand.Parameters.AddWithValue("@journ_internal_trans_flag", IIf(journ_internal_trans_flag, "Y", "N"))
        sSeperator = ","

        If Not String.IsNullOrEmpty(journ_subcat_code_part1.Trim) Then
          journalQuery.Append(sSeperator + " journ_subcat_code_part1 = @journ_subcat_code_part1")
          SqlCommand.Parameters.AddWithValue("@journ_subcat_code_part1", journ_subcat_code_part1.Trim)
          sSeperator = ","
        End If

        If Not String.IsNullOrEmpty(journ_subcat_code_part2.Trim) Then
          journalQuery.Append(sSeperator + " journ_subcat_code_part2 = @journ_subcat_code_part2")
          SqlCommand.Parameters.AddWithValue("@journ_subcat_code_part2", journ_subcat_code_part2.Trim)
          sSeperator = ","
        End If

        If Not String.IsNullOrEmpty(journ_subcat_code_part3.Trim) Then
          journalQuery.Append(sSeperator + " journ_subcat_code_part3 = @journ_subcat_code_part3")
          SqlCommand.Parameters.AddWithValue("@journ_subcat_code_part3", journ_subcat_code_part3.Trim)
          sSeperator = ","
        End If

        If journ_yacht_id > 0 Then
          journalQuery.Append(sSeperator + " journ_yacht_id = @journ_yacht_id")
          SqlCommand.Parameters.AddWithValue("@journ_yacht_id", journ_yacht_id.ToString.Trim)
          sSeperator = ","
        End If

        If journ_amod_id > 0 Then
          journalQuery.Append(sSeperator + " journ_amod_id = @journ_amod_id")
          SqlCommand.Parameters.AddWithValue("@journ_amod_id", journ_amod_id.ToString.Trim)
          sSeperator = ","
        End If

        ' set action date when updating record 

        journalQuery.Append(sSeperator + " journ_action_date = @journ_action_date")
        SqlCommand.Parameters.AddWithValue("@journ_action_date", IIf(Not String.IsNullOrEmpty(journ_action_date.Trim), FormatDateTime(journ_action_date, DateFormat.GeneralDate).Trim, Now.ToString))

        journalQuery.Append(" WHERE journ_id = @journ_id")

        SqlCommand.Parameters.AddWithValue("@journ_id", journ_id.ToString.Trim)

      Else

        journalQuery.Append("UPDATE " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[HomeBase].jetnet_ra.dbo.", "") + "Journal SET")

        If Not String.IsNullOrEmpty(journ_date.Trim) Then
          journalQuery.Append(" journ_date = '" + FormatDateTime(journ_date.Trim, DateFormat.GeneralDate).Trim + "',")
        Else
          journalQuery.Append(" journ_date = '" + FormatDateTime(Now, DateFormat.GeneralDate).Trim + "',")
        End If

        If Not String.IsNullOrEmpty(journ_subcategory_code.Trim) Then
          journalQuery.Append(" journ_subcategory_code = '" + journ_subcategory_code.Trim + "',")
        Else
          journalQuery.Append(" journ_subcategory_code = '',")
        End If

        If Not String.IsNullOrEmpty(journ_subject.Trim) Then
          journalQuery.Append(" journ_subject = '" + journ_subject.Replace("'", "''").Trim + "',")
        Else
          journalQuery.Append(" journ_subject = '',")
        End If

        If Not String.IsNullOrEmpty(journ_description.Trim) Then
          journalQuery.Append(" journ_description = '" + journ_description.Replace("'", "''").Trim + "',")
        Else
          journalQuery.Append(" journ_description = '',")
        End If

        If journ_ac_id > 0 Then
          journalQuery.Append(" journ_ac_id = " + journ_ac_id.ToString + ",")
        ElseIf journ_ac_id = 0 Then
          journalQuery.Append(" journ_ac_id = 0,")
        End If

        If journ_contact_id > 0 Then
          journalQuery.Append(" journ_contact_id = " + journ_contact_id.ToString + ",")
        ElseIf journ_contact_id = 0 Then
          journalQuery.Append(" journ_contact_id = 0,")
        End If

        If journ_comp_id > 0 Then
          journalQuery.Append(" journ_comp_id = " + journ_comp_id.ToString + ",")
        ElseIf journ_comp_id = 0 Then
          journalQuery.Append(" journ_comp_id = 0,")
        End If

        If Not String.IsNullOrEmpty(journ_user_id.Trim) Then
          journalQuery.Append(" journ_user_id = '" + journ_user_id.Trim + "',")
        Else
          journalQuery.Append(" journ_user_id = '',")
        End If

        If Not String.IsNullOrEmpty(journ_entry_date.Trim) Then
          journalQuery.Append(" journ_entry_date = '" + FormatDateTime(journ_entry_date.Trim, DateFormat.ShortDate).Trim + "',")
        Else
          journalQuery.Append(" journ_entry_date = '" + FormatDateTime(Now, DateFormat.ShortDate).Trim + "',")
        End If

        If Not String.IsNullOrEmpty(journ_entry_time.Trim) Then
          journalQuery.Append(" journ_entry_time = '" + FormatDateTime(journ_entry_time.Trim, DateFormat.LongTime) + "',")
        Else
          journalQuery.Append(" journ_entry_time = '" + FormatDateTime(Now, DateFormat.LongTime) + "',")
        End If

        If Not String.IsNullOrEmpty(journ_account_id.Trim) Then
          journalQuery.Append(" journ_account_id = '" + journ_account_id.Trim + "',")
        End If

        If Not String.IsNullOrEmpty(journ_prior_account_id.Trim) Then
          journalQuery.Append(" journ_prior_account_id = '" + journ_prior_account_id.Trim + "',")
        End If

        If Not String.IsNullOrEmpty(journ_status.Trim) Then
          journalQuery.Append(" journ_status = '" + journ_status.Trim + "',")
        End If

        If Not String.IsNullOrEmpty(journ_customer_note.Trim) Then
          journalQuery.Append(" journ_customer_note = '" + journ_customer_note.Trim + "',")
        End If

        If journ_pcreckey > 0 Then
          journalQuery.Append(" journ_pcreckey = " + journ_pcreckey.ToString + ",")
        ElseIf journ_pcreckey = 0 Then
          journalQuery.Append(" journ_pcreckey = 0,")
        End If

        If journ_fractowr_id > 0 Then
          journalQuery.Append(" journ_fractowr_id = " + journ_fractowr_id.ToString + ",")
        ElseIf journ_fractowr_id = 0 Then
          journalQuery.Append(" journ_fractowr_id = 0,")
        End If

        If journ_fractsld_id > 0 Then
          journalQuery.Append(" journ_fractsld_id = " + journ_fractsld_id.ToString + ",")
        ElseIf journ_fractsld_id = 0 Then
          journalQuery.Append(" journ_fractsld_id = 0,")
        End If

        journalQuery.Append(" journ_newac_flag = '" + IIf(journ_newac_flag, "Y", "N") + "',")

        journalQuery.Append(" journ_internal_trans_flag = '" + IIf(journ_internal_trans_flag, "Y", "N") + "',")

        If Not String.IsNullOrEmpty(journ_subcat_code_part1.Trim) Then
          journalQuery.Append(" journ_subcat_code_part1 = '" + journ_subcat_code_part1.Trim + "',")
        Else
          journalQuery.Append(" journ_subcat_code_part1 = '',")
        End If

        If Not String.IsNullOrEmpty(journ_subcat_code_part2.Trim) Then
          journalQuery.Append(" journ_subcat_code_part2 = '" + journ_subcat_code_part2.Trim + "',")
        Else
          journalQuery.Append(" journ_subcat_code_part2 = '',")
        End If

        If Not String.IsNullOrEmpty(journ_subcat_code_part3.Trim) Then
          journalQuery.Append(" journ_subcat_code_part3 = '" + journ_subcat_code_part3.Trim + "',")
        Else
          journalQuery.Append(" journ_subcat_code_part3 = '',")
        End If

        If journ_yacht_id > 0 Then
          journalQuery.Append(" journ_yacht_id = " + journ_yacht_id.ToString + ",")
        ElseIf journ_yacht_id = 0 Then
          journalQuery.Append(" journ_yacht_id = 0,")
        End If

        If journ_amod_id > 0 Then
          journalQuery.Append(" journ_amod_id = " + journ_amod_id.ToString + ",")
        ElseIf journ_amod_id = 0 Then
          journalQuery.Append(" journ_amod_id = 0,")
        End If

        journalQuery.Append(" journ_action_date = '" + IIf(Not String.IsNullOrEmpty(journ_action_date.Trim), FormatDateTime(journ_action_date, DateFormat.GeneralDate).Trim, Now.ToString) + "'")

        journalQuery.Append(" WHERE journ_id = " + journ_id.ToString)

      End If

      SqlCommand.CommandText = journalQuery.ToString

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + journalQuery.ToString

      Try
        SqlCommand.ExecuteNonQuery()
      Catch exSql As SqlClient.SqlException
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + exSql.Message
      End Try

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message

    Finally

      SqlConn.Dispose()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

  End Sub

  Public Function insertJournalRecord(Optional bUseStringQuery As Boolean = False) As Long

    Dim journalQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand

    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim newJournal_rowID As Long = 0

    Try

      If String.IsNullOrEmpty(journ_subcategory_code.Trim) Then
        Return -1
      End If

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 90

      If Not bUseStringQuery Then

        journalQuery.Append("INSERT INTO " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Journal (journ_date, journ_subcategory_code, journ_subject, journ_description, journ_ac_id, journ_contact_id,")
        journalQuery.Append(" journ_comp_id, journ_user_id, journ_entry_date, journ_entry_time, journ_account_id, journ_prior_account_id, journ_action_date,")
        journalQuery.Append(" journ_status, journ_customer_note, journ_pcreckey, journ_fractowr_id, journ_fractsld_id, journ_newac_flag,")
        journalQuery.Append(" journ_internal_trans_flag, journ_subcat_code_part1, journ_subcat_code_part2, journ_subcat_code_part3, journ_yacht_id, journ_amod_id")
        journalQuery.Append(") VALUES (@journ_date, @journ_subcategory_code, @journ_subject, @journ_description, @journ_ac_id, @journ_contact_id,")
        journalQuery.Append(" @journ_comp_id, @journ_user_id, @journ_entry_date, @journ_entry_time, @journ_account_id, @journ_prior_account_id, @journ_action_date,")
        journalQuery.Append(" @journ_status, @journ_customer_note, @journ_pcreckey, @journ_fractowr_id, @journ_fractsld_id, @journ_newac_flag,")
        journalQuery.Append(" @journ_internal_trans_flag, @journ_subcat_code_part1, @journ_subcat_code_part2, @journ_subcat_code_part3, @journ_yacht_id, @journ_amod_id")
        journalQuery.Append("); SELECT CAST(scope_identity() AS int);")

        SqlCommand.Parameters.AddWithValue("@journ_date", FormatDateTime(journ_date.Trim, DateFormat.GeneralDate).Trim)

        If Not String.IsNullOrEmpty(journ_subcategory_code.Trim) Then
          SqlCommand.Parameters.AddWithValue("@journ_subcategory_code", journ_subcategory_code.Trim)
        End If

        If Not String.IsNullOrEmpty(journ_subject.Trim) Then
          SqlCommand.Parameters.AddWithValue("@journ_subject", journ_subject.Trim)
        End If

        If Not String.IsNullOrEmpty(journ_description.Trim) Then
          SqlCommand.Parameters.AddWithValue("@journ_description", journ_description.Replace("'", "''").Trim)
        End If

        If journ_ac_id > 0 Then
          SqlCommand.Parameters.AddWithValue("@journ_ac_id", journ_ac_id.ToString.Trim)
        End If

        If journ_contact_id > 0 Then
          SqlCommand.Parameters.AddWithValue("@journ_contact_id", journ_contact_id.ToString.Trim)
        End If

        If journ_comp_id > 0 Then
          SqlCommand.Parameters.AddWithValue("@journ_comp_id", journ_comp_id.ToString.Trim)
        End If

        If Not String.IsNullOrEmpty(journ_user_id.Trim) Then
          SqlCommand.Parameters.AddWithValue("@journ_user_id", journ_user_id.Trim)
        End If

        If Not String.IsNullOrEmpty(journ_entry_date.Trim) Then
          SqlCommand.Parameters.AddWithValue("@journ_entry_date", journ_entry_date.Trim)
        End If

        If Not String.IsNullOrEmpty(journ_entry_time.Trim) Then
          SqlCommand.Parameters.AddWithValue("@journ_entry_time", journ_entry_time.Trim)
        End If

        If Not String.IsNullOrEmpty(journ_account_id.Trim) Then
          SqlCommand.Parameters.AddWithValue("@journ_account_id", journ_account_id.Trim)
        End If

        If Not String.IsNullOrEmpty(journ_prior_account_id.Trim) Then
          SqlCommand.Parameters.AddWithValue("@journ_prior_account_id", journ_prior_account_id.Trim)
        End If

        If Not String.IsNullOrEmpty(journ_status.Trim) Then
          SqlCommand.Parameters.AddWithValue("@journ_status", journ_status.Trim)
        End If

        If Not String.IsNullOrEmpty(journ_customer_note.Trim) Then
          SqlCommand.Parameters.AddWithValue("@journ_customer_note", journ_customer_note.Trim)
        End If

        If journ_pcreckey > 0 Then
          SqlCommand.Parameters.AddWithValue("@journ_pcreckey", journ_pcreckey.ToString.Trim)
        End If

        If journ_fractowr_id > 0 Then
          SqlCommand.Parameters.AddWithValue("@journ_fractowr_id", journ_fractowr_id.ToString.Trim)
        End If

        If journ_fractsld_id > 0 Then
          SqlCommand.Parameters.AddWithValue("@journ_fractsld_id", journ_fractsld_id.ToString.Trim)
        End If

        SqlCommand.Parameters.AddWithValue("@journ_newac_flag", IIf(journ_newac_flag, "Y", "N"))

        SqlCommand.Parameters.AddWithValue("@journ_internal_trans_flag", IIf(journ_internal_trans_flag, "Y", "N"))

        If Not String.IsNullOrEmpty(journ_subcat_code_part1.Trim) Then
          SqlCommand.Parameters.AddWithValue("@journ_subcat_code_part1", journ_subcat_code_part1.Trim)
        End If

        If Not String.IsNullOrEmpty(journ_subcat_code_part2.Trim) Then
          SqlCommand.Parameters.AddWithValue("@journ_subcat_code_part2", journ_subcat_code_part2.Trim)
        End If

        If Not String.IsNullOrEmpty(journ_subcat_code_part3.Trim) Then
          SqlCommand.Parameters.AddWithValue("@journ_subcat_code_part3", journ_subcat_code_part3.Trim)
        End If

        If journ_yacht_id > 0 Then
          SqlCommand.Parameters.AddWithValue("@journ_yacht_id", journ_yacht_id.ToString.Trim)
        End If

        If journ_amod_id > 0 Then
          SqlCommand.Parameters.AddWithValue("@journ_amod_id", journ_amod_id.ToString.Trim)
        End If


        ' set action date when updating record
        SqlCommand.Parameters.AddWithValue("@journ_action_date", IIf(Not String.IsNullOrEmpty(journ_action_date.Trim), FormatDateTime(journ_action_date, DateFormat.GeneralDate).Trim, Now.ToString))

        SqlCommand.CommandText = journalQuery.ToString

      Else

        journalQuery.Append("INSERT INTO " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Journal (journ_date, journ_subcategory_code, journ_subject, journ_description, journ_ac_id, journ_contact_id,")
        journalQuery.Append(" journ_comp_id, journ_user_id, journ_entry_date, journ_entry_time, journ_account_id, journ_prior_account_id, journ_action_date,")
        journalQuery.Append(" journ_status, journ_customer_note, journ_pcreckey, journ_fractowr_id, journ_fractsld_id, journ_newac_flag,")
        journalQuery.Append(" journ_internal_trans_flag, journ_subcat_code_part1, journ_subcat_code_part2, journ_subcat_code_part3, journ_yacht_id, journ_amod_id")
        journalQuery.Append(") VALUES ('" + FormatDateTime(journ_date.Trim, DateFormat.GeneralDate).Trim + "'," + IIf(Not String.IsNullOrEmpty(journ_subcategory_code.Trim), "'" + journ_subcategory_code.Trim + "'", "''") + ", ")
        journalQuery.Append(IIf(Not String.IsNullOrEmpty(journ_subject.Trim), "'" + journ_subject.Trim + "'", "''") + ", " + IIf(Not String.IsNullOrEmpty(journ_description.Trim), "'" + journ_description.Replace("'", "''").Trim + "'", "''") + ", ")
        journalQuery.Append(IIf(journ_ac_id > 0, journ_ac_id.ToString, "0") + ", " + IIf(journ_contact_id > 0, journ_contact_id.ToString, "0") + ",")
        journalQuery.Append(" " + IIf(journ_comp_id > 0, journ_comp_id.ToString, "0") + ", " + IIf(Not String.IsNullOrEmpty(journ_user_id.Trim), "'" + journ_user_id.Trim + "'", "''") + ", '")
        journalQuery.Append(FormatDateTime(journ_entry_date.Trim, DateFormat.ShortDate).Trim + "', '" + FormatDateTime(journ_entry_time.Trim, DateFormat.LongTime) + "', '', '','")
        journalQuery.Append(IIf(Not String.IsNullOrEmpty(journ_action_date.Trim), FormatDateTime(journ_action_date, DateFormat.GeneralDate).Trim, Now.ToString) + "',")
        journalQuery.Append(" " + IIf(Not String.IsNullOrEmpty(journ_status.Trim), "'" + journ_status.Trim + "'", "''") + ", " + IIf(Not String.IsNullOrEmpty(journ_customer_note.Trim), "'" + journ_customer_note.Trim + "'", "''") + ", ")
        journalQuery.Append(IIf(journ_pcreckey > 0, journ_pcreckey.ToString, "0") + ", " + IIf(journ_fractowr_id > 0, journ_fractowr_id.ToString, "0") + ", " + IIf(journ_fractsld_id > 0, journ_fractsld_id.ToString, "0") + ", '")
        journalQuery.Append(IIf(journ_newac_flag, "Y", "N") + "', '" + IIf(journ_internal_trans_flag, "Y", "N") + "', ")
        journalQuery.Append(IIf(Not String.IsNullOrEmpty(journ_subcat_code_part1.Trim), "'" + journ_subcat_code_part1.Trim + "'", "''") + ", ")
        journalQuery.Append(IIf(Not String.IsNullOrEmpty(journ_subcat_code_part2.Trim), "'" + journ_subcat_code_part2.Trim + "'", "''") + ", ")
        journalQuery.Append(IIf(Not String.IsNullOrEmpty(journ_subcat_code_part3.Trim), "'" + journ_subcat_code_part3.Trim + "'", "''") + ", ")
        journalQuery.Append(IIf(journ_yacht_id > 0, journ_yacht_id.ToString, "0") + ", " + IIf(journ_amod_id > 0, journ_amod_id.ToString, "0") + ")")
        journalQuery.Append("; SELECT MAX(journ_id) AS newrowid FROM " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Journal;")

        SqlCommand.CommandText = journalQuery.ToString

      End If

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + journalQuery.ToString

      Try
        newJournal_rowID = Convert.ToInt32(SqlCommand.ExecuteScalar())
      Catch exSql As SqlClient.SqlException
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + exSql.Message
      End Try

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message
      Return -1

    Finally

      SqlConn.Dispose()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return newJournal_rowID

  End Function

  Public Sub deleteJournalRecord(Optional bUseStringQuery As Boolean = False)
    Dim journalQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand

    Dim sSeperator = ""
    Try

      If journ_id = 0 Then
        Exit Sub
      End If

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 90

      If Not bUseStringQuery Then

        journalQuery.Append("DELETE FROM " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[HomeBase].jetnet_ra.dbo.", "") + "Journal")
        journalQuery.Append(" WHERE journ_id = @journ_id")

        SqlCommand.Parameters.AddWithValue("@journ_id", journ_id.ToString.Trim)

      Else

        journalQuery.Append("DELETE FROM " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[HomeBase].jetnet_ra.dbo.", "") + "Journal WHERE journ_id = " + journ_id.ToString)

      End If

      SqlCommand.CommandText = journalQuery.ToString

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + journalQuery.ToString

      Try
        SqlCommand.ExecuteNonQuery()
      Catch exSql As SqlClient.SqlException
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + exSql.Message
      End Try

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message

    Finally

      SqlConn.Dispose()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

  End Sub

  Public Sub setJournalActionToMarketingNote(Optional bUseStringQuery As Boolean = False)
    Dim journalQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand

    Dim sSeperator = ""
    Try

      If journ_id = 0 Then
        Exit Sub
      End If

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 90

      If Not bUseStringQuery Then

        journalQuery.Append("UPDATE " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[HomeBase].jetnet_ra.dbo.", "") + "Journal SET")

        If Not String.IsNullOrEmpty(journ_date.Trim) Then
          journalQuery.Append(sSeperator + " journ_date = @journ_date")
          SqlCommand.Parameters.AddWithValue("@journ_date", FormatDateTime(journ_date.Trim, DateFormat.GeneralDate))
          sSeperator = ","
        ElseIf String.IsNullOrEmpty(journ_date.Trim) Then
          journalQuery.Append(sSeperator + " journ_date = @journ_date")
          SqlCommand.Parameters.AddWithValue("@journ_date", FormatDateTime(Now, DateFormat.GeneralDate))
          sSeperator = ","
        End If

        If Not String.IsNullOrEmpty(journ_subcategory_code.Trim) Then
          journalQuery.Append(sSeperator + " journ_subcategory_code = @journ_subcategory_code")
          SqlCommand.Parameters.AddWithValue("@journ_subcategory_code", journ_subcategory_code.Trim)
          sSeperator = ","
        End If

        If Not String.IsNullOrEmpty(journ_subject.Trim) Then
          journalQuery.Append(sSeperator + " journ_subject = @journ_subject")
          SqlCommand.Parameters.AddWithValue("@journ_subject", journ_subject.Trim)
          sSeperator = ","
        End If

        If Not String.IsNullOrEmpty(journ_description.Trim) Then
          journalQuery.Append(sSeperator + " journ_description = @journ_description")
          SqlCommand.Parameters.AddWithValue("@journ_description", journ_description.Replace("'", "''").Trim)
          sSeperator = ","
        ElseIf String.IsNullOrEmpty(journ_description.Trim) Then
          journalQuery.Append(sSeperator + " journ_description = @journ_description")
          SqlCommand.Parameters.AddWithValue("@journ_description", DBNull.Value)
          sSeperator = ","
        End If

        If Not String.IsNullOrEmpty(journ_status.Trim) Then
          journalQuery.Append(sSeperator + " journ_status = @journ_status")
          SqlCommand.Parameters.AddWithValue("@journ_status", journ_status.Trim)
          sSeperator = ","
        End If

        If Not String.IsNullOrEmpty(journ_subcat_code_part1.Trim) Then
          journalQuery.Append(sSeperator + " journ_subcat_code_part1 = @journ_subcat_code_part1")
          SqlCommand.Parameters.AddWithValue("@journ_subcat_code_part1", journ_subcat_code_part1.Trim)
          sSeperator = ","
        End If

        If Not String.IsNullOrEmpty(journ_subcat_code_part2.Trim) Then
          journalQuery.Append(sSeperator + " journ_subcat_code_part2 = @journ_subcat_code_part2")
          SqlCommand.Parameters.AddWithValue("@journ_subcat_code_part2", journ_subcat_code_part2.Trim)
          sSeperator = ","
        End If

        If Not String.IsNullOrEmpty(journ_subcat_code_part3.Trim) Then
          journalQuery.Append(sSeperator + " journ_subcat_code_part3 = @journ_subcat_code_part3")
          SqlCommand.Parameters.AddWithValue("@journ_subcat_code_part3", journ_subcat_code_part3.Trim)
          sSeperator = ","
        End If

        journalQuery.Append(sSeperator + " journ_action_date = @journ_action_date")
        SqlCommand.Parameters.AddWithValue("@journ_action_date", IIf(Not String.IsNullOrEmpty(journ_action_date.Trim), FormatDateTime(journ_action_date, DateFormat.GeneralDate).Trim, Now.ToString))

        journalQuery.Append(" WHERE journ_id = @journ_id")

        SqlCommand.Parameters.AddWithValue("@journ_id", journ_id.ToString.Trim)

      Else

        journalQuery.Append("UPDATE " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[HomeBase].jetnet_ra.dbo.", "") + "Journal SET")
        journalQuery.Append(" journ_date = '" + FormatDateTime(journ_date.Trim, DateFormat.GeneralDate).Trim + "',")
        journalQuery.Append(" journ_subcategory_code = " + IIf(Not String.IsNullOrEmpty(journ_subcategory_code.Trim), "'" + journ_subcategory_code.Trim + "'", "''") + ",")
        journalQuery.Append(" journ_subject = " + IIf(Not String.IsNullOrEmpty(journ_subject.Trim), "'" + journ_subject.Trim + "'", "''") + ",")
        journalQuery.Append(" journ_description = " + IIf(Not String.IsNullOrEmpty(journ_description.Trim), "'" + journ_description.Replace("'", "''").Trim + "'", "''") + ",")
        journalQuery.Append(" journ_status = " + IIf(Not String.IsNullOrEmpty(journ_status.Trim), "'" + journ_status.Trim + "'", "''") + ",")
        journalQuery.Append(" journ_subcat_code_part1 = " + IIf(Not String.IsNullOrEmpty(journ_subcat_code_part1.Trim), "'" + journ_subcat_code_part1.Trim + "'", "''") + ",")
        journalQuery.Append(" journ_subcat_code_part2 = " + IIf(Not String.IsNullOrEmpty(journ_subcat_code_part2.Trim), "'" + journ_subcat_code_part2.Trim + "'", "''") + ",")
        journalQuery.Append(" journ_subcat_code_part3 = " + IIf(Not String.IsNullOrEmpty(journ_subcat_code_part3.Trim), "'" + journ_subcat_code_part3.Trim + "'", "''") + ",")
        journalQuery.Append(" journ_action_date = '" + IIf(Not String.IsNullOrEmpty(journ_action_date.Trim), FormatDateTime(journ_action_date, DateFormat.GeneralDate).Trim, Now.ToString) + "'")
        journalQuery.Append(" WHERE journ_id = " + journ_id.ToString)

      End If

      SqlCommand.CommandText = journalQuery.ToString

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + journalQuery.ToString

      Try
        SqlCommand.ExecuteNonQuery()
      Catch exSql As SqlClient.SqlException
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + exSql.Message
      End Try

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message

    Finally

      SqlConn.Dispose()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

  End Sub

  Public Overrides Function Equals(obj As Object) As Boolean
    Dim [class] = TryCast(obj, journalClass)
    Return [class] IsNot Nothing AndAlso
           journ_id = [class].journ_id AndAlso
           journ_date = [class].journ_date AndAlso
           journ_subcategory_code = [class].journ_subcategory_code AndAlso
           journ_subject = [class].journ_subject AndAlso
           journ_description = [class].journ_description AndAlso
           journ_ac_id = [class].journ_ac_id AndAlso
           journ_contact_id = [class].journ_contact_id AndAlso
           journ_comp_id = [class].journ_comp_id AndAlso
           journ_user_id = [class].journ_user_id AndAlso
           journ_entry_date = [class].journ_entry_date AndAlso
           journ_entry_time = [class].journ_entry_time AndAlso
           journ_account_id = [class].journ_account_id AndAlso
           journ_prior_account_id = [class].journ_prior_account_id AndAlso
           journ_status = [class].journ_status AndAlso
           journ_customer_note = [class].journ_customer_note AndAlso
           journ_action_date = [class].journ_action_date AndAlso
           journ_pcreckey = [class].journ_pcreckey AndAlso
           journ_fractowr_id = [class].journ_fractowr_id AndAlso
           journ_fractsld_id = [class].journ_fractsld_id AndAlso
           journ_newac_flag = [class].journ_newac_flag AndAlso
           journ_internal_trans_flag = [class].journ_internal_trans_flag AndAlso
           journ_subcat_code_part1 = [class].journ_subcat_code_part1 AndAlso
           journ_subcat_code_part2 = [class].journ_subcat_code_part2 AndAlso
           journ_subcat_code_part3 = [class].journ_subcat_code_part3 AndAlso
           journ_yacht_id = [class].journ_yacht_id AndAlso
           journ_amod_id = [class].journ_amod_id
  End Function

  Public Shared Operator =(class1 As journalClass, class2 As journalClass) As Boolean
    Return EqualityComparer(Of journalClass).Default.Equals(class1, class2)
  End Operator

  Public Shared Operator <>(class1 As journalClass, class2 As journalClass) As Boolean
    Return Not class1 = class2
  End Operator

End Class
