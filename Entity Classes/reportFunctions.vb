Imports Microsoft.VisualBasic
Imports System.ComponentModel

' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/Entity Classes/reportFunctions.vb $
'$$Author: Matt $
'$$Date: 11/21/19 2:44p $
'$$Modtime: 11/21/19 10:46a $
'$$Revision: 5 $
'$$Workfile: reportFunctions.vb $
'
' ********************************************************************************

<System.Serializable()> Public Class reportFunctions
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

  Public Function get_first_contact_email_address_report(ByVal inCompanyID As Long, ByVal inJournalID As Long) As String

    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sFirstEmailAddress As String = ""

    Try

      sQuery.Append("SELECT TOP 1 contact_email_address FROM Contact WITH(NOLOCK)")
      sQuery.Append(" WHERE contact_comp_id = " + inCompanyID.ToString)
      sQuery.Append(" AND contact_journ_id = " + inJournalID.ToString)
      sQuery.Append(" AND contact_email_address <> '' AND contact_email_address IS NOT NULL")
      sQuery.Append(" AND contact_active_flag = 'Y' AND contact_hide_flag = 'N'")
      sQuery.Append(" ORDER BY contact_acpros_seq_no")

      'HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />GetFirstContactEmailAddress(ByVal inCompanyID As String, ByVal inJournalID As Long) As String</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlCommand.CommandText = sQuery.ToString

      SqlReader = SqlCommand.ExecuteReader()

      If SqlReader.HasRows Then

        SqlReader.Read()

        If Not IsDBNull(SqlReader("contact_email_address")) Then
          If Not String.IsNullOrEmpty(SqlReader.Item("contact_email_address").ToString.Trim) Then
            sFirstEmailAddress = SqlReader.Item("contact_email_address").ToString.Trim
          End If
        End If

      End If 'SqlReader.HasRows

      SqlReader.Close()

    Catch ex As Exception
      aError = "Error in GetFirstContactEmailAddress(ByVal inCompanyID As String, ByVal inJournalID As Long) As String" + ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return sFirstEmailAddress

  End Function

  Public Function get_contact_email_address_report(ByVal inCompanyID As Long, ByVal inContactID As Long) As String

    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sEmailAddress As String = ""

    Try

      sQuery.Append("SELECT TOP 1 contact_email_address FROM Contact WITH(NOLOCK)")
      sQuery.Append(" WHERE contact_comp_id = " + inCompanyID.ToString)
      sQuery.Append(" AND contact_journ_id = 0")
      sQuery.Append(" AND contact_id = " + inContactID.ToString)

      'HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_contact_email_address_report(ByVal inCompanyID As String, ByVal inContactID As Long) As String</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlCommand.CommandText = sQuery.ToString

      SqlReader = SqlCommand.ExecuteReader()

      If SqlReader.HasRows Then

        SqlReader.Read()

        If Not IsDBNull(SqlReader("contact_email_address")) Then
          If Not String.IsNullOrEmpty(SqlReader.Item("contact_email_address").ToString.Trim) Then
            sEmailAddress = SqlReader.Item("contact_email_address").ToString.Trim
          End If
        End If

      End If 'SqlReader.HasRows

      SqlReader.Close()

    Catch ex As Exception
      aError = "Error in GetFirstContactEmailAddress(ByVal inCompanyID As String, ByVal inJournalID As Long) As String" + ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return sEmailAddress

  End Function

  Public Function get_state_name_report(ByVal inState As String, ByVal inCountry As String) As String

    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sStateName As String = ""

    Try

      sQuery.Append("SELECT state_name FROM State WITH(NOLOCK) WHERE state_code = '" + inState.Trim + "' AND state_country = '" + inCountry.Trim + "'")

      'HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_state_name_report(ByVal inState As String, ByVal inCountry As String) As String</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlCommand.CommandText = sQuery.ToString

      SqlReader = SqlCommand.ExecuteReader()

      If SqlReader.HasRows Then

        SqlReader.Read()

        If Not IsDBNull(SqlReader("state_name")) Then
          If Not String.IsNullOrEmpty(SqlReader.Item("state_name").ToString.Trim) Then
            sStateName = SqlReader.Item("state_name").ToString.Trim
          End If
        End If

      End If 'SqlReader.HasRows

      SqlReader.Close()

    Catch ex As Exception
      aError = "Error in get_state_name_report(ByVal inState As String, ByVal inCountry As String) As String" + ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return sStateName

  End Function

  Public Sub get_fraction_percent_and_expire_date(ByVal nAircraftJournalID As String, ByVal inReport As Boolean, ByRef sFractionPercent As String, ByRef sFractionExpiresDate As String)
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      If Not inReport Then
        sFractionPercent = "&nbsp;"
        sFractionExpiresDate = "&nbsp;"
      End If

      sQuery.Append("SELECT DISTINCT cref_owner_percent, cref_fraction_expires_date FROM Aircraft_Reference WITH(NOLOCK)")
      sQuery.Append(" WHERE cref_contact_type = '70' AND cref_journ_id = " + nAircraftJournalID.ToString)

      'HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_fraction_percent_and_expire_date(ByVal nAircraftJournalID As String, ByVal inReport As Boolean, ByRef sFractionPercent As String, ByRef sFractionExpiresDate As String)</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlCommand.CommandText = sQuery.ToString

      SqlReader = SqlCommand.ExecuteReader()

      If SqlReader.HasRows Then

        SqlReader.Read()

        If Not (IsDBNull(SqlReader("cref_owner_percent"))) Then

          If Not String.IsNullOrEmpty(SqlReader.Item("cref_owner_percent").ToString.Trim) Then
            If inReport Then
              sFractionPercent = SqlReader.Item("cref_owner_percent").ToString.Trim + "%"
            Else
              sFractionPercent = "[" + SqlReader.Item("cref_owner_percent").ToString.Trim + "%]"
            End If
          End If

        End If

        If Not (IsDBNull(SqlReader("cref_fraction_expires_date"))) Then

          If Not String.IsNullOrEmpty(SqlReader.Item("cref_fraction_expires_date").ToString.Trim) Then
            sFractionExpiresDate = SqlReader.Item("cref_fraction_expires_date").ToString.Trim
          End If

        End If

      End If

      SqlReader.Close()

    Catch ex As Exception
      aError = "Error in get_fraction_percent_and_expire_date(ByVal nAircraftJournalID As String, ByVal inReport As Boolean, ByRef sFractionPercent As String, ByRef sFractionExpiresDate As String)" + ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

  End Sub

  Public Sub get_lease_expire_and_confirm_date(ByVal inJournalID As String, ByVal inReport As Boolean, ByRef sLeaseExpireDate As String, ByRef sLeaseExpConfirmDate As String)

    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      If Not inReport Then
        sLeaseExpireDate = "&nbsp;"
        sLeaseExpConfirmDate = "&nbsp;"
      End If

      sQuery.Append("SELECT DISTINCT aclease_expiration_date, aclease_exp_confirm_date FROM Aircraft_Lease WITH(NOLOCK) WHERE aclease_journ_id = " + inJournalID.ToString)

      'HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_lease_expire_and_confirm_date(ByVal inJournalID As String, ByVal inReport As Boolean, ByRef sLeaseExpireDate As String, ByRef sLeaseExpConfirmDate As String)</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlCommand.CommandText = sQuery.ToString

      SqlReader = SqlCommand.ExecuteReader()

      If SqlReader.HasRows Then

        SqlReader.Read()

        If Not (IsDBNull(SqlReader("aclease_expiration_date"))) Then

          If Not String.IsNullOrEmpty(SqlReader.Item("aclease_expiration_date").ToString.Trim) Then
            sLeaseExpireDate = SqlReader.Item("aclease_expiration_date").ToString.Trim
          End If

        End If

        If Not (IsDBNull(SqlReader("aclease_exp_confirm_date"))) Then

          If Not String.IsNullOrEmpty(SqlReader.Item("aclease_exp_confirm_date").ToString.Trim) Then
            sLeaseExpConfirmDate = SqlReader.Item("aclease_exp_confirm_date").ToString.Trim
          End If

        End If

      End If

      SqlReader.Close()

    Catch ex As Exception
      aError = "Error in get_lease_expire_and_confirm_date(ByVal inJournalID As String, ByVal inReport As Boolean, ByRef sLeaseExpireDate As String, ByRef sLeaseExpConfirmDate As String)" + ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

  End Sub

  Public Sub get_in_favor_of_info(ByVal lACId As Long, ByVal lCompOwrId As Long, ByRef strInFavorOf As String, ByRef strDocDate As String, ByRef strDocType As String, ByRef strDocAmount As String)

    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      strInFavorOf = ""
      strDocDate = ""
      strDocType = ""
      strDocAmount = ""

      If (lACId > 0) Then

        sQuery.Append("SELECT TOP 1 comp_name, adoc_doc_date, adoc_doc_type, adoc_doc_amount")
        sQuery.Append(" FROM Company WITH (NOLOCK)")
        sQuery.Append(" INNER JOIN Aircraft_Document WITH (NOLOCK) ON comp_id = adoc_infavor_comp_id AND comp_journ_id = adoc_journ_id")
        sQuery.Append(" WHERE (adoc_ac_id = " + lACId.ToString + ")")
        sQuery.Append(" AND (adoc_infavor_comp_id IS NOT NULL)")
        sQuery.Append(" AND (adoc_onbehalf_comp_id = " + lCompOwrId.ToString + ")")
        sQuery.Append(" ORDER BY adoc_doc_date DESC ") ' Newest First  

        'HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_in_favor_of_info(ByVal lACId As Long, ByVal lJournId As Long, ByVal lCompOwrId As Long, ByRef strInFavorOf As String, ByRef strDocDate As String, ByRef strDocType As String, ByRef strDocAmount As String)</b><br />" + sQuery.ToString

        SqlConn.ConnectionString = clientConnectString
        SqlConn.Open()
        SqlCommand.Connection = SqlConn
        SqlCommand.CommandType = CommandType.Text
        SqlCommand.CommandTimeout = 60

        SqlCommand.CommandText = sQuery.ToString

        SqlReader = SqlCommand.ExecuteReader()

        If SqlReader.HasRows Then

          SqlReader.Read()

          If Not IsDBNull(SqlReader("comp_name")) Then
            If Not String.IsNullOrEmpty(SqlReader.Item("comp_name").ToString.Trim) Then
              strInFavorOf = SqlReader.Item("comp_name").ToString.Trim
            End If
          End If

          If Not IsDBNull(SqlReader("adoc_doc_date")) Then
            If Not String.IsNullOrEmpty(SqlReader.Item("adoc_doc_date").ToString.Trim) Then
              strDocDate = FormatDateTime(SqlReader.Item("adoc_doc_date").ToString, vbShortDate).Trim
            End If
          End If

          If Not IsDBNull(SqlReader("adoc_doc_type")) Then
            If Not String.IsNullOrEmpty(SqlReader.Item("adoc_doc_type").ToString.Trim) Then
              strDocType = SqlReader.Item("adoc_doc_type").ToString.Trim
            End If
          End If

          If Not IsDBNull(SqlReader("adoc_doc_amount")) Then
            If Not String.IsNullOrEmpty(SqlReader.Item("adoc_doc_amount").ToString.Trim) Then
              strDocAmount = FormatNumber(SqlReader.Item("adoc_doc_amount").ToString, 0, True, False, False).Trim
            End If
          End If

        End If

        SqlReader.Close()

      End If ' If (lACId > 0) Then

    Catch ex As Exception
      aError = "Error in get_in_favor_of_info(ByVal lACId As Long, ByVal lJournId As Long, ByVal lCompOwrId As Long, ByRef strInFavorOf As String, ByRef strDocDate As String, ByRef strDocType As String, ByRef strDocAmount As String)" + ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

  End Sub

  Public Function get_lifecycle_stage_report(ByVal nAircraftLifeCycle As Integer, ByVal inReport As Boolean) As String

    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sLifeCycleStage As String = ""

    Try

      If Not inReport Then
        sLifeCycleStage = "&nbsp;"
      End If

      sQuery.Append("SELECT acs_name FROM Aircraft_Stage WITH(NOLOCK) WHERE acs_code = " + nAircraftLifeCycle.ToString)

      'HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_lifecycle_stage_report(ByVal nAircraftLifeCycle As Integer, ByVal inReport As Boolean) As String</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlCommand.CommandText = sQuery.ToString

      SqlReader = SqlCommand.ExecuteReader()

      If SqlReader.HasRows Then

        SqlReader.Read()

        If Not (IsDBNull(SqlReader("acs_name"))) Then

          If Not String.IsNullOrEmpty(SqlReader.Item("acs_name").ToString.Trim) Then
            sLifeCycleStage = SqlReader.Item("acs_name").ToString.Trim
          End If

        End If

      End If

      SqlReader.Close()

    Catch ex As Exception
      aError = "Error in get_lifecycle_stage_report(ByVal nAircraftLifeCycle As Integer, ByVal inReport As Boolean) As String" + ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return sLifeCycleStage

  End Function

  Public Function get_ownership_type_report(ByVal sAircraftOwnerType As String) As String

    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sOwnerType As String = "&lt;Unknown&gt;"

    Try

      sQuery.Append("SELECT acot_name FROM Aircraft_Owner_Type WITH(NOLOCK) WHERE lower(acot_code) = '" + sAircraftOwnerType.ToLower.Trim + "'")

      'HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_ownership_type_report(ByVal sAircraftOwnerType As String, ByVal inReport As Boolean) As String</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlCommand.CommandText = sQuery.ToString

      SqlReader = SqlCommand.ExecuteReader()

      If SqlReader.HasRows Then

        SqlReader.Read()

        If Not (IsDBNull(SqlReader("acot_name"))) Then

          If Not String.IsNullOrEmpty(SqlReader.Item("acot_name").ToString.Trim) Then
            sOwnerType = SqlReader.Item("acot_name").ToString.Trim
          End If

        End If

      End If

      SqlReader.Close()

    Catch ex As Exception
      aError = "Error in get_ownership_type_report(ByVal sAircraftOwnerType As String, ByVal inReport As Boolean) As String" + ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return sOwnerType

  End Function

  Public Function get_reg_expire_date(ByVal nAircraftID As Long, ByVal nJournalID As Long) As String

    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sRegExpireDate As String = ""

    Try

      sQuery.Append("SELECT DISTINCT ac_reg_no_expiration_date FROM Aircraft WITH(NOLOCK) WHERE ac_id = " + nAircraftID.ToString + " AND ac_journ_id = " + nJournalID.ToString)

      'HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_reg_expire_date(ByVal nAircraftID As Long, ByVal nJournalID As Long) As String</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlCommand.CommandText = sQuery.ToString

      SqlReader = SqlCommand.ExecuteReader()

      If SqlReader.HasRows Then

        SqlReader.Read()

        If Not (IsDBNull(SqlReader("ac_reg_no_expiration_date"))) Then
          If Not String.IsNullOrEmpty(SqlReader.Item("ac_reg_no_expiration_date").ToString.Trim) Then
            sRegExpireDate = "<br />[Expires: " + FormatDateTime(SqlReader.Item("ac_reg_no_expiration_date").ToString.Trim, DateFormat.ShortDate) + "]"
          End If

        End If

      End If

      SqlReader.Close()

    Catch ex As Exception
      aError = "Error in get_reg_expire_date(ByVal nAircraftID As Long, ByVal nJournalID As Long) As String" + ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return sRegExpireDate

  End Function

  Public Function get_exclusive_date_report(ByVal nAircraftID As Long, ByVal nJournalID As Long) As String

    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim nGetExclusiveCompID As Long = 0
    Dim sExclusiveDate As String = ""

    Try

      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      If nAircraftID = 0 Then

        sQuery.Append("SELECT TOP 1 journ_date FROM Journal WITH(NOLOCK) WHERE (journ_ac_id = " + nAircraftID.ToString)
        sQuery.Append(" AND journ_subcategory_code = 'EXON') ORDER BY journ_date DESC")

        SqlCommand.CommandText = sQuery.ToString

        SqlReader = SqlCommand.ExecuteReader()

        If SqlReader.HasRows Then

          SqlReader.Read()

          If Not IsDBNull(SqlReader.Item("journ_date")) Then
            If Not String.IsNullOrEmpty(SqlReader.Item("journ_date").ToString) Then
              sExclusiveDate = FormatDateTime(SqlReader.Item("journ_date").ToString, DateFormat.ShortDate).Trim
            End If
          End If

        End If

        SqlReader.Close()

      Else

        sQuery.Append("SELECT DISTINCT comp_id, cref_transmit_seq_no FROM Company WITH(NOLOCK)")
        sQuery.Append(" INNER JOIN Aircraft_Reference WITH(NOLOCK) ON (comp_id = cref_comp_id AND comp_journ_id = cref_journ_id)")
        sQuery.Append(" WHERE (cref_ac_id = " + nAircraftID.ToString + " AND cref_journ_id = " + nJournalID.ToString)
        sQuery.Append(" AND cref_contact_type IN ('93','98','99')")

        If nJournalID = 0 Then
          sQuery.Append(" AND comp_active_flag = 'Y'")
        End If

        sQuery.Append(" AND comp_hide_flag = 'N')")
        sQuery.Append(" ORDER BY cref_transmit_seq_no")

        SqlCommand.CommandText = sQuery.ToString

        SqlReader = SqlCommand.ExecuteReader()

        If SqlReader.HasRows Then

          SqlReader.Read()

          If Not IsDBNull(SqlReader.Item("comp_id")) Then
            If Not String.IsNullOrEmpty(SqlReader.Item("comp_id").ToString) Then
              nGetExclusiveCompID = CLng(SqlReader.Item("comp_id").ToString)
            End If
          End If

        End If

        SqlReader.Close()

        If nGetExclusiveCompID > 0 Then

          sQuery = New StringBuilder()

          sQuery.Append("SELECT TOP 1 journ_date FROM Journal WITH(NOLOCK) WHERE (journ_ac_id = " + nAircraftID.ToString)
          sQuery.Append(" AND journ_comp_id = " + nGetExclusiveCompID.ToString + " AND journ_subcategory_code = 'EXON') ORDER BY journ_date DESC")

          SqlCommand.CommandText = sQuery.ToString
          SqlReader = SqlCommand.ExecuteReader()

          If SqlReader.HasRows Then

            SqlReader.Read()

            If Not IsDBNull(SqlReader.Item("journ_date")) Then
              If Not String.IsNullOrEmpty(SqlReader.Item("journ_date").ToString) Then
                sExclusiveDate = FormatDateTime(SqlReader.Item("journ_date").ToString, DateFormat.ShortDate).Trim
              End If
            End If

          End If

          SqlReader.Close()

        End If '  nGetExclusiveCompID > 0 Then

      End If

    Catch ex As Exception
      aError = "Error in get_exclusive_date_report(ByVal nAircraftID As Long, ByVal nJournalID As Long) As String" + ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return sExclusiveDate

  End Function

  Public Function get_fraction_purchase_date(ByVal nAircraftID As Long, ByVal nCompanyID As Long, ByVal inReport As Boolean) As String

    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sFractionPurchaseDate As String = ""

    Try

      If Not inReport Then
        sFractionPurchaseDate = "&nbsp;"
      End If

      sQuery.Append("SELECT TOP 1 journ_date FROM Aircraft_Reference WITH(NOLOCK)")
      sQuery.Append(" INNER JOIN Journal WITH(NOLOCK) ON (cref_journ_id = journ_id AND cref_ac_id = journ_ac_id)")
      sQuery.Append(" WHERE (cref_contact_type = '70')")                 ' Purchaser
      sQuery.Append(" AND (cref_ac_id = " + nAircraftID.ToString + ") AND (cref_comp_id = " + nCompanyID.ToString + ")")
      sQuery.Append(" AND (cref_journ_id <> 0)")

      'HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />GetFractionPurchaseDate(ByVal nAircraftID As Long, ByVal nCompanyID As Long, ByVal inReport As Boolean) As String</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlCommand.CommandText = sQuery.ToString

      SqlReader = SqlCommand.ExecuteReader()

      If SqlReader.HasRows Then

        SqlReader.Read()

        If Not (IsDBNull(SqlReader("journ_date"))) Then

          If Not String.IsNullOrEmpty(SqlReader.Item("journ_date").ToString.Trim) Then
            sFractionPurchaseDate = FormatDateTime(SqlReader.Item("journ_date").ToString.Trim, DateFormat.ShortDate)
          End If

        End If

      End If

      SqlReader.Close()

    Catch ex As Exception
      aError = "Error in GetFractionPurchaseDate(ByVal nAircraftID As Long, ByVal nCompanyID As Long, ByVal inReport As Boolean) As String" + ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return sFractionPurchaseDate

  End Function

  Public Function get_reference_type_report(ByVal inRefCode As String, ByVal inReport As Boolean) As String

    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sReferenceType As String = ""

    Try

      If Not inReport Then
        sReferenceType = "&nbsp;"
      End If

      sQuery.Append("SELECT actype_name FROM Aircraft_Contact_Type WITH(NOLOCK) WHERE actype_code = '" + inRefCode + "'")

      'HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_reference_type_report(ByVal inRefCode As String, ByVal inReport As Boolean) As String</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlCommand.CommandText = sQuery.ToString

      SqlReader = SqlCommand.ExecuteReader()

      If SqlReader.HasRows Then

        SqlReader.Read()

        If Not IsDBNull(SqlReader("actype_name")) Then

          If Not String.IsNullOrEmpty(SqlReader.Item("actype_name").ToString.Trim) Then
            sReferenceType = SqlReader.Item("actype_name").ToString.Trim
          End If

        End If

      End If

      SqlReader.Close()

    Catch ex As Exception
      aError = "Error in get_reference_type_report(ByVal inRefCode As String, ByVal inReport As Boolean) As String" + ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return sReferenceType

  End Function

  Public Function get_reference_info_report(ByVal inCompanyRefID As String, ByVal inReport As Boolean) As String

    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sReferenceInfo As String = ""

    Try

      If Not inReport Then
        sReferenceInfo = "&nbsp;"
      End If

      sQuery.Append("SELECT actype_name, cref_owner_percent FROM Aircraft_Reference WITH(NOLOCK), Aircraft_Contact_Type WITH(NOLOCK)")
      sQuery.Append(" WHERE cref_id = " + inCompanyRefID.ToString + " AND cref_contact_type = actype_code")

      'HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_reference_info_report(ByVal inCompanyRefID As String, ByVal inReport As Boolean) As String</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlCommand.CommandText = sQuery.ToString

      SqlReader = SqlCommand.ExecuteReader()

      If SqlReader.HasRows Then

        SqlReader.Read()

        If Not (inReport) Then

          If Not (IsDBNull(SqlReader("actype_name"))) Then
            If Not String.IsNullOrEmpty(SqlReader.Item("actype_name").ToString.Trim) Then
              sReferenceInfo = SqlReader.Item("actype_name").ToString.Trim
            End If
          End If

          If Not IsDBNull(SqlReader.Item("cref_owner_percent")) Then
            If CLng(SqlReader.Item("cref_owner_percent").ToString) > 0 And CLng(SqlReader.Item("cref_owner_percent").ToString) < 100 Then
              sReferenceInfo += " [" + SqlReader.Item("cref_owner_percent").ToString + "%]"
            End If
          End If

        Else

          If Not IsDBNull(SqlReader.Item("cref_owner_percent")) Then
            If CLng(SqlReader.Item("cref_owner_percent").ToString) > 0 And CLng(SqlReader.Item("cref_owner_percent").ToString) < 100 Then
              sReferenceInfo = SqlReader.Item("cref_owner_percent").ToString + "%"
            End If
          End If

        End If

      End If

      SqlReader.Close()

    Catch ex As Exception
      aError = "Error in get_reference_info_report(ByVal inCompanyRefID As String, ByVal inReport As Boolean) As String" + ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return sReferenceInfo

  End Function
  Public Function get_standard_ac_features_report_multiple_models(ByVal ac_amod_id As String) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT ")

      If Trim(ac_amod_id) <> "" Then
        sQuery.Append(" distinct amfeat_feature_code, kfeat_name   ")
      Else
        sQuery.Append(" amfeat_feature_code, kfeat_name, amfeat_seq_no  ")
      End If

      sQuery.Append(" FROM Aircraft_Model_Key_Feature WITH(NOLOCK), Key_Feature WITH(NOLOCK) WHERE amfeat_standard_equip = 'Y' AND")
      sQuery.Append(" ((amfeat_stdeq_start_ser_no_value IS NULL AND amfeat_stdeq_end_ser_no_value IS NULL) OR")
      sQuery.Append(" (amfeat_stdeq_start_ser_no_value = 0 AND amfeat_stdeq_end_ser_no_value = 0)) AND ")

      If Trim(ac_amod_id) <> "" Then
        sQuery.Append(" amfeat_amod_id in (" & ac_amod_id.ToString & ") ")
      End If

      sQuery.Append(" AND amfeat_feature_code = kfeat_code and kfeat_code <> 'DAM' ")

      If Trim(ac_amod_id) <> "" Then
        sQuery.Append("  ORDER BY amfeat_feature_code")
      Else
        sQuery.Append("  ORDER BY amfeat_seq_no, amfeat_feature_code")
      End If

      'HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_standard_ac_features_report(ByVal ac_amod_id As Long) As DataTable</b><br />" + sQuery.ToString

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
        aError = "Error in get_standard_ac_features_report load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_standard_ac_features_report(ByVal ac_amod_id As Long) As DataTable " + ex.Message

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
  Public Function get_standard_ac_features_report(ByVal ac_amod_id As Long) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT amfeat_feature_code, kfeat_name, amfeat_seq_no FROM Aircraft_Model_Key_Feature WITH(NOLOCK), Key_Feature WITH(NOLOCK) WHERE amfeat_standard_equip = 'Y' AND")
      sQuery.Append(" ((amfeat_stdeq_start_ser_no_value IS NULL AND amfeat_stdeq_end_ser_no_value IS NULL) OR")
      sQuery.Append(" (amfeat_stdeq_start_ser_no_value = 0 AND amfeat_stdeq_end_ser_no_value = 0)) AND")

      sQuery.Append(" amfeat_amod_id = " + ac_amod_id.ToString)

      sQuery.Append(" AND amfeat_feature_code = kfeat_code ORDER BY amfeat_seq_no, amfeat_feature_code")

      'HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_standard_ac_features_report(ByVal ac_amod_id As Long) As DataTable</b><br />" + sQuery.ToString

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
        aError = "Error in get_standard_ac_features_report load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_standard_ac_features_report(ByVal ac_amod_id As Long) As DataTable " + ex.Message

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

  Public Sub load_standard_ac_features_report(ByVal ac_amod_id As Long, ByRef inStdFeatCodes(,) As String)

    Dim results_table As New DataTable
    Dim nCounter As Integer = 0

    Try

      results_table = get_standard_ac_features_report(ac_amod_id)

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          ReDim inStdFeatCodes(results_table.Rows.Count - 1, 1)

          For Each r As DataRow In results_table.Rows
            inStdFeatCodes(nCounter, 0) = r.Item("amfeat_feature_code").ToString.Trim.ToUpper
            inStdFeatCodes(nCounter, 1) = r.Item("kfeat_name").ToString.Trim
            nCounter += 1
          Next

        Else
          ReDim inStdFeatCodes(0, 0)
          inStdFeatCodes(0, 0) = ""
        End If

      Else
        ReDim inStdFeatCodes(0, 0)
        inStdFeatCodes(0, 0) = ""
      End If

    Catch ex As Exception

      aError = "Error in load_standard_ac_features_report(ByVal ac_amod_id As Long, ByRef inStdFeatCodes(,) As String) " + ex.Message

    Finally

    End Try

    results_table = Nothing

  End Sub

    Public Sub load_standard_ac_features_report_multiple(ByVal ac_amod_id As String, ByRef inStdFeatCodes(,) As String, ByRef counter_temp As Integer)

        Dim results_table As New DataTable

        Try

            results_table = get_standard_ac_features_report_multiple_models(ac_amod_id)

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    For Each r As DataRow In results_table.Rows
                        '  ReDim Preserve inStdFeatCodes(counter_temp, 1)
                        inStdFeatCodes(counter_temp, 0) = r.Item("amfeat_feature_code").ToString.Trim.ToUpper
                        inStdFeatCodes(counter_temp, 1) = r.Item("kfeat_name").ToString.Trim
                        counter_temp += 1
                    Next

                Else
                    '  ReDim inStdFeatCodes(0, 0)
                    '             inStdFeatCodes(0, 0) = ""
                End If

            Else
                '   ReDim inStdFeatCodes(0, 0)
                '           inStdFeatCodes(0, 0) = ""
            End If


        Catch ex As Exception

            aError = "Error in load_standard_ac_features_report(ByVal ac_amod_id As Long, ByRef inStdFeatCodes(,) As String) " + ex.Message

        Finally

        End Try

        results_table = Nothing

    End Sub

    Public Function get_nonstandard_ac_features_report(ByVal ac_amod_id As Long, ByVal inStdFeatCodes(,) As String) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT amfeat_feature_code, kfeat_name FROM Aircraft_Model_Key_Feature WITH(NOLOCK), Key_Feature WITH(NOLOCK)")

      sQuery.Append(" WHERE amfeat_amod_id = " + ac_amod_id.ToString + " AND amfeat_feature_code = kfeat_code")

      If Not IsNothing(inStdFeatCodes) And IsArray(inStdFeatCodes) Then
        If inStdFeatCodes(0, 0) <> "" Then

          sQuery.Append(" AND amfeat_feature_code NOT IN(")
          For x As Integer = 0 To UBound(inStdFeatCodes)
            If x = 0 Then
              sQuery.Append("'" + inStdFeatCodes(x, 0) + "'")
            Else
              sQuery.Append(",'" + inStdFeatCodes(x, 0) + "'")
            End If
          Next
          sQuery.Append(") ORDER BY amfeat_seq_no")

        Else
          sQuery.Append(" ORDER BY amfeat_seq_no")
        End If
      Else
        sQuery.Append(" ORDER BY amfeat_seq_no")
      End If

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_nonstandard_ac_features_report(ByVal ac_amod_id As Long, ByRef inStdFeatCodes As String) As DataTable</b><br />" + sQuery.ToString

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
        aError = "Error in get_nonstandard_ac_features_report load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_nonstandard_ac_features_report(ByVal ac_amod_id As Long, ByRef inStdFeatCodes As String) As DataTable " + ex.Message

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

  Public Sub display_nonstandard_feature_code_headings_report(ByVal ac_amod_id As Long, ByRef inFeatCodes() As String, ByRef inStdFeatCodes(,) As String, ByRef out_htmlString As String)

    Dim htmlOut As New StringBuilder
    Dim results_table As New DataTable
    Dim nCounter As Integer = 0

    Try

      results_table = get_nonstandard_ac_features_report(ac_amod_id, inStdFeatCodes)

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          ReDim inFeatCodes(results_table.Rows.Count - 1)

          For Each r As DataRow In results_table.Rows

            htmlOut.Append("<td style='text-align:center; vertical-align: middle;' title='" + r.Item("kfeat_name").ToString.Trim + "'><strong>&nbsp;" + r.Item("amfeat_feature_code").ToString.Trim.ToUpper + "&nbsp;</strong></td>")

            inFeatCodes(nCounter) = r.Item("amfeat_feature_code").ToString.Trim.ToUpper
            nCounter += 1

          Next

        Else
          ReDim inFeatCodes(0)
          inFeatCodes(0) = ""
        End If

      Else
        ReDim inFeatCodes(0)
        inFeatCodes(0) = ""
      End If

    Catch ex As Exception

      aError = "Error in display_nonstandard_feature_code_headings_report(ByVal ac_amod_id As Long, ByRef inFeatCodes() As String, ByRef inStdFeatCodes(,) As String, ByRef out_htmlString As String) " + ex.Message

    Finally

    End Try

    'return resulting html string
    out_htmlString = htmlOut.ToString
    htmlOut = Nothing
    results_table = Nothing

  End Sub
  Public Function get_ac_features_report_multiple(ByVal amod_id_list As String, ByVal nAircraftJournalID As Long, ByRef inFeatCodes(,) As String) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT distinct kfeat_code, kfeat_name FROM Aircraft_Key_Feature WITH(NOLOCK) INNER JOIN")
      sQuery.Append(" Key_Feature WITH(NOLOCK) ON (afeat_feature_code = kfeat_code) ")
      sQuery.Append(" inner join aircraft with (NOLOCK) on  afeat_ac_id = ac_id and ac_journ_id = 0 ")

      sQuery.Append(" WHERE kfeat_inactive_date IS NULL")

      If Not IsNothing(inFeatCodes) And IsArray(inFeatCodes) Then

        If inFeatCodes(0, 0) <> "" Then

          sQuery.Append(crmWebClient.Constants.cAndClause + "kfeat_code NOT IN(")

          For x As Integer = 0 To UBound(inFeatCodes)
            If x = 0 Then
              sQuery.Append("'" + inFeatCodes(x, 0) + "'")
            Else
              sQuery.Append(",'" + inFeatCodes(x, 0) + "'")
            End If
          Next

          sQuery.Append(")")

        End If

      End If

      sQuery.Append(crmWebClient.Constants.cAndClause + " ac_amod_id in (" & amod_id_list.ToString & ") and kfeat_code <> 'DAM' AND afeat_journ_id = " + nAircraftJournalID.ToString + " ORDER BY kfeat_code, kfeat_name ")

      'HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_ac_features_report(ByVal ac_id As Long, ByVal j_id As Long, ByRef inFeatCodes() As String) As DataTable</b><br />" + sQuery.ToString

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
        aError = "Error in get_ac_features_report load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_ac_features_report(ByVal ac_id As Long, ByVal j_id As Long, ByRef inFeatCodes() As String) As DataTable " + ex.Message

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
  Public Function get_ac_features_report(ByVal nAircraftID As Long, ByVal nAircraftJournalID As Long, ByRef inFeatCodes() As String, ByVal getTopFeatures As Integer) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT" + IIf(getTopFeatures > 0, " TOP " + getTopFeatures.ToString, "") + " afeat_status_flag, kfeat_code, kfeat_name FROM Aircraft_Key_Feature WITH(NOLOCK) INNER JOIN")
      sQuery.Append(" Key_Feature WITH(NOLOCK) ON (afeat_feature_code = kfeat_code) WHERE kfeat_inactive_date IS NULL")

      If Not IsNothing(inFeatCodes) And IsArray(inFeatCodes) Then

        If inFeatCodes(0) <> "" Then

          sQuery.Append(crmWebClient.Constants.cAndClause + "kfeat_code IN(")

          For x As Integer = 0 To UBound(inFeatCodes)
            If x = 0 Then
              sQuery.Append("'" + inFeatCodes(x) + "'")
            Else
              sQuery.Append(",'" + inFeatCodes(x) + "'")
            End If
          Next

          sQuery.Append(")")

        End If

      End If

      sQuery.Append(crmWebClient.Constants.cAndClause + "afeat_ac_id = " + nAircraftID.ToString + " AND afeat_journ_id = " + nAircraftJournalID.ToString + " ORDER BY afeat_seq_no")

      'HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_ac_features_report(ByVal ac_id As Long, ByVal j_id As Long, ByRef inFeatCodes() As String) As DataTable</b><br />" + sQuery.ToString

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
        aError = "Error in get_ac_features_report load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_ac_features_report(ByVal ac_id As Long, ByVal j_id As Long, ByRef inFeatCodes() As String) As DataTable " + ex.Message

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

  Public Sub display_ac_feature_codes_report(ByVal nAircraftID As Long, ByVal nAircraftJournalID As Long, ByRef inFeatCodes() As String, ByRef out_htmlString As String)

    Dim htmlOut As New StringBuilder
    Dim results_table As New DataTable

    Try

      htmlOut.Append("<table id=""featureDataTable"" cellpadding=""2"" cellspacing=""0"" border=""0"" width=""100%""><tr>")

      If Not IsNothing(inFeatCodes) And IsArray(inFeatCodes) Then

        results_table = get_ac_features_report(nAircraftID, nAircraftJournalID, inFeatCodes, 0)

        If Not IsNothing(results_table) Then

          If results_table.Rows.Count > 0 Then

            For Each r As DataRow In results_table.Rows

              If Not IsDBNull(r("afeat_status_flag")) Then
                If Not String.IsNullOrEmpty(r.Item("afeat_status_flag").ToString) Then
                  htmlOut.Append("<td style=""text-align:center; vertical-align: middle;"" title=""" + r.Item("kfeat_code").ToString.Trim.ToUpper + """>")
                  htmlOut.Append("<strong>&nbsp;" + r.Item("afeat_status_flag").ToString.Trim.ToUpper + "&nbsp;</strong></td>")
                Else
                  htmlOut.Append("<td style=""text-align:center; vertical-align: middle;"" title=""Unknown""><strong>&nbsp;U&nbsp;</strong></td>")
                End If
              Else
                htmlOut.Append("<td style=""text-align:center; vertical-align: middle;"" title=""Unknown""><strong>&nbsp;U&nbsp;</strong></td>")
              End If

            Next

          Else
            htmlOut.Append("<td style=""text-align:center; vertical-align: middle;"">No features available for this Make / Model ...</td>")
          End If

        Else
          htmlOut.Append("<td style=""text-align:center; vertical-align: middle;"">No features available for this Make / Model ...</td>")
        End If

      End If

      htmlOut.Append("</tr></table>")

    Catch ex As Exception

      aError = "Error in display_ac_feature_codes_report(ByVal nAircraftID As Long, ByVal nAircraftJournalID As Long, ByRef inFeatCodes() As String, ByRef out_htmlString As String) " + ex.Message

    Finally

    End Try

    'return resulting html string
    out_htmlString = htmlOut.ToString
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Sub get_all_ac_feature_codes(ByVal nAircraftID As Long, ByVal nAircraftJournalID As Long, ByRef out_htmlString As String, Optional ByVal threeColumn As Boolean = False)

    Dim htmlOut As New StringBuilder
    Dim results_table As New DataTable

    Dim nCounter As Integer = 0
    Dim bFirstOne As Boolean = False

    Dim nColumns As Integer = IIf(threeColumn, 3, 4)

    Try

      htmlOut.Append("<table id=""featureDataTable"" cellpadding=""2"" cellspacing=""0"" border=""0"" width=""100%""><tr>")

      results_table = get_ac_features_report(nAircraftID, nAircraftJournalID, Nothing, 0)

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          For Each r As DataRow In results_table.Rows

            If Not IsDBNull(r("afeat_status_flag")) Then
              If Not String.IsNullOrEmpty(r.Item("afeat_status_flag").ToString) Then

                If Not bFirstOne Then
                  htmlOut.Append("<tr>")
                  bFirstOne = True
                End If

                If nCounter = nColumns Then
                  htmlOut.Append("</tr><tr>")
                  nCounter = 0
                End If

                htmlOut.Append("<td nowrap=""nowrap"" style=""text-align:left; vertical-align: middle;"" title=""" + r.Item("kfeat_code").ToString.Trim.ToUpper + """>")
                htmlOut.Append("<strong>" + r.Item("kfeat_code").ToString.Trim.ToUpper + "</strong>&nbsp;:&nbsp;" + r.Item("afeat_status_flag").ToString.Trim.ToUpper + "</td>")

                nCounter += 1

              Else
                htmlOut.Append("<td style=""text-align:left; vertical-align: middle;"" title=""Unknown""><strong>UNK</strong>&nbsp;:&nbsp;U</td>")
              End If
            Else
              htmlOut.Append("<td style=""text-align:left; vertical-align: middle;"" title=""Unknown""><strong>UNK</strong>&nbsp;:&nbsp;U</td>")
            End If

          Next

        Else
          htmlOut.Append("<td style=""text-align:center; vertical-align: middle;"">No features available for this Make / Model</td>")
        End If

      End If

      htmlOut.Append("</tr></table>")

    Catch ex As Exception

      aError = "Error in get_all_feature_codes(ByVal nAircraftID As Long, ByVal nAircraftJournalID As Long, ByRef inFeatCodes() As String, ByRef out_htmlString As String) " + ex.Message

    Finally

    End Try

    'return resulting html string
    out_htmlString = htmlOut.ToString
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Function get_owner_info_report(ByVal nAircraftID As Long, ByVal nAircraftJournalID As Long, ByVal bGetExclusive As Boolean, ByVal bGetOperator As Boolean) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT TOP 1 * FROM Company WITH(NOLOCK)")
      sQuery.Append(" INNER JOIN Aircraft_Reference WITH(NOLOCK) ON (comp_id = cref_comp_id AND comp_journ_id = cref_journ_id)")
      sQuery.Append(" LEFT OUTER JOIN Contact WITH(NOLOCK) ON (cref_contact_id = contact_id AND cref_journ_id = contact_journ_id)")
      sQuery.Append(" WHERE (cref_ac_id = " + nAircraftID.ToString + " AND cref_journ_id = " + nAircraftJournalID.ToString)

      If bGetExclusive Then
        sQuery.Append(" AND ((cref_contact_type = '99') OR (cref_contact_type = '93') OR (cref_transmit_seq_no = 4))")
      ElseIf bGetOperator Then
        sQuery.Append(" AND (cref_operator_flag in ('Y','O'))")
      Else
        sQuery.Append(" AND (cref_transmit_seq_no = 1 AND cref_contact_type <> '71')")
      End If

      If nAircraftJournalID = 0 Then
        sQuery.Append(" AND comp_active_flag = 'Y'")
      End If

      sQuery.Append(" AND comp_hide_flag = 'N')")
      sQuery.Append(" " + commonEvo.MakeCompanyProductCodeClause(HttpContext.Current.Session.Item("localPreferences"), False))

      'HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_owner_info_report(ByVal nAircraftID As Long, ByVal nAircraftJournalID As Long, ByVal bGetExclusive As Boolean, ByVal bGetOperator As Boolean) As DataTable</b><br />" + sQuery.ToString

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
        aError = "Error in get_owner_info_report load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_owner_info_report(ByVal nAircraftID As Long, ByVal nAircraftJournalID As Long, ByVal bGetExclusive As Boolean, ByVal bGetOperator As Boolean) As DataTable " + ex.Message

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

  Public Function get_avionics_package_report(ByVal nAircraftID As Long, ByVal nAircraftJournalID As Long, ByVal inReport As Boolean) As String

    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sAvionicsPackage As String = ""

    Try

      If Not inReport Then
        sAvionicsPackage = "&nbsp;"
      End If

      sQuery.Append("SELECT av_description FROM Aircraft_Avionics WITH(NOLOCK) WHERE av_ac_id = " + nAircraftID.ToString)
      sQuery.Append(" AND av_ac_journ_id = " + nAircraftJournalID.ToString + " AND lower(av_name) = 'avionics package'")

      'HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_avionics_package_report(ByVal nAircraftID As Long, ByVal nAircraftJournalID As Long, ByVal inReport As Boolean) As String</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlCommand.CommandText = sQuery.ToString

      SqlReader = SqlCommand.ExecuteReader()

      If SqlReader.HasRows Then

        SqlReader.Read()

        If Not IsDBNull(SqlReader("av_description")) Then
          If Not String.IsNullOrEmpty(SqlReader.Item("av_description").ToString.Trim) Then
            sAvionicsPackage = HttpContext.Current.Server.HtmlEncode(SqlReader.Item("av_description").ToString.Trim)
          End If
        End If

      End If 'SqlReader.HasRows

      SqlReader.Close()

    Catch ex As Exception
      aError = "Error in get_avionics_package_report(ByVal nAircraftID As Long, ByVal nAircraftJournalID As Long, ByVal inReport As Boolean) As String" + ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return sAvionicsPackage

  End Function

  Public Function get_EMP_name(ByVal lACId As Long, ByVal lJournId As Long) As String

    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sEMPName As String = ""

    Try

      sQuery.Append("SELECT emp_provider_name, emp_program_name FROM Aircraft WITH(NOLOCK)")
      sQuery.Append(" INNER JOIN Engine_Maintenance_Program WITH(NOLOCK) ON ac_engine_maintenance_prog_EMP = emp_id")
      sQuery.Append(" WHERE (ac_id = " + lACId.ToString + " AND ac_journ_id = " + lJournId.ToString + ")")

      'HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_EMP_name(ByVal lACId As Long, ByVal lJournId As Long) As String</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlCommand.CommandText = sQuery.ToString

      SqlReader = SqlCommand.ExecuteReader()

      If SqlReader.HasRows Then

        SqlReader.Read()

        If Not IsDBNull(SqlReader("emp_provider_name")) Then

          If Not String.IsNullOrEmpty(SqlReader.Item("emp_provider_name").ToString.Trim) Then

            If Not SqlReader.Item("emp_provider_name").ToString.Trim.Contains("unknown") Then
              sEMPName = SqlReader.Item("emp_provider_name").ToString.Trim + " " + SqlReader.Item("emp_program_name").ToString.Trim
            End If

          End If

        End If

      End If

      SqlReader.Close()

    Catch ex As Exception
      aError = "Error in get_EMP_name(ByVal lACId As Long, ByVal lJournId As Long) As String" + ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return sEMPName

  End Function

  Public Function get_AMP_name(ByVal lACId As Long, ByVal lJournId As Long) As String

    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sAMPName As String = ""

    Try

      sQuery.Append("SELECT amp_provider_name, amp_program_name FROM Aircraft WITH(NOLOCK)")
      sQuery.Append(" INNER JOIN Airframe_Maintenance_Program WITH(NOLOCK) ON ac_airframe_maintenance_prog_AMP = amp_id")
      sQuery.Append(" WHERE (ac_id = " + lACId.ToString + " AND ac_journ_id = " + lJournId.ToString + ")")

      'HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_AMP_name(ByVal lACId As Long, ByVal lJournId As Long) As String</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlCommand.CommandText = sQuery.ToString

      SqlReader = SqlCommand.ExecuteReader()

      If SqlReader.HasRows Then

        SqlReader.Read()

        If Not IsDBNull(SqlReader("amp_provider_name")) Then

          If Not String.IsNullOrEmpty(SqlReader.Item("amp_provider_name").ToString.Trim) Then

            If Not SqlReader.Item("amp_provider_name").ToString.Trim.Contains("unknown") Then
              sAMPName = SqlReader.Item("amp_provider_name").ToString.Trim + " " + SqlReader.Item("amp_program_name").ToString.Trim
            End If

          End If

        End If

      End If

      SqlReader.Close()

    Catch ex As Exception
      aError = "Error in get_AMP_name(ByVal lACId As Long, ByVal lJournId As Long) As String" + ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return sAMPName

  End Function

  Public Function get_contact_name_title(ByVal inContactID As String, ByVal bNoContactTitle As Boolean) As String

    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sContactNameTitle As String = ""

    Try

      sQuery.Append("SELECT contact_first_name, contact_last_name, contact_title, contact_suffix FROM Contact WITH(NOLOCK)")
      sQuery.Append(" WHERE (contact_id = " + inContactID.ToString + ") AND (contact_journ_id = 0)")

      'HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_contact_name_title(ByVal inContactID As String) As String</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlCommand.CommandText = sQuery.ToString

      SqlReader = SqlCommand.ExecuteReader()

      If SqlReader.HasRows Then

        SqlReader.Read()

        If Not IsDBNull(SqlReader("contact_first_name")) Then

          If Not String.IsNullOrEmpty(SqlReader.Item("contact_first_name").ToString.Trim) Then
            If bNoContactTitle Then
              sContactNameTitle = SqlReader.Item("contact_first_name").ToString.Trim + " " + SqlReader.Item("contact_last_name").ToString.Trim + " " + SqlReader.Item("contact_suffix").ToString.Trim + "<br />"
            Else
              sContactNameTitle = SqlReader.Item("contact_first_name").ToString.Trim + " " + SqlReader.Item("contact_last_name").ToString.Trim + " " + SqlReader.Item("contact_suffix").ToString.Trim + "<br />" + SqlReader.Item("contact_title").ToString.Trim + "<br />"
            End If

          End If

        End If

      End If

      SqlReader.Close()

    Catch ex As Exception
      aError = "Error in get_contact_name_title(ByVal inContactID As String) As String" + ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return sContactNameTitle

  End Function

  Public Function get_contact_type_for_contact_id(ByVal inContactType As String) As String

    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sContactType As String = ""

    Try

      sQuery.Append("SELECT actype_name FROM Aircraft_Contact_Type WITH(NOLOCK)")
      sQuery.Append(" WHERE (actype_code = '" + inContactType.Trim + "'")

      ' Hide Exclusive Brokers and Representatives and Dealers from Aerodex users
      If HttpContext.Current.Session.Item("localPreferences").AerodexFlag Then
        sQuery.Append(" AND actype_code NOT IN ('93','98','99','67','68','02'))")
      Else
        sQuery.Append(" AND actype_code NOT IN ('67','68','02'))")
      End If

      'HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_contact_type_for_contact_id(ByVal inContactType As String) As String</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlCommand.CommandText = sQuery.ToString

      SqlReader = SqlCommand.ExecuteReader()

      If SqlReader.HasRows Then

        SqlReader.Read()

        If Not IsDBNull(SqlReader("actype_name")) Then

          If Not String.IsNullOrEmpty(SqlReader.Item("actype_name").ToString.Trim) Then

            sContactType = SqlReader.Item("actype_name").ToString.Trim.Replace("Additional Contact1", "Additional Company")

          End If

        End If

      End If

      SqlReader.Close()

    Catch ex As Exception
      aError = "Error in get_contact_type_for_contact_id(ByVal inContactType As String) As String" + ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return sContactType

  End Function

  Public Function get_phone_info(ByVal inCompanyId As Long, ByVal inJournalId As Long, ByVal inContactId As Long) As String
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sPhoneNumbers As String = ""

    Try

      If (inCompanyId >= 0) And (inJournalId >= 0) And (inContactId >= 0) Then

        sQuery.Append("SELECT pnum_type, pnum_number_full FROM Phone_Numbers WITH(NOLOCK)")
        sQuery.Append(" INNER JOIN Phone_Type WITH(NOLOCK) ON pnum_type = ptype_name")
        sQuery.Append(" WHERE (pnum_comp_id = " + inCompanyId.ToString + ")")
        sQuery.Append(" AND (pnum_contact_id = " + inContactId.ToString + ")")
        sQuery.Append(" AND (pnum_journ_id = " + inJournalId.ToString + ")")
        sQuery.Append(" AND (pnum_hide_customer = 'N')")
        sQuery.Append(" ORDER BY ptype_seq_no")

        'HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_phone_info(ByVal inCompanyId As Long, ByVal inJournalId As Long, ByVal inContactId As Long) As String</b><br />" + sQuery.ToString

        SqlConn.ConnectionString = clientConnectStr
        SqlConn.Open()
        SqlCommand.Connection = SqlConn
        SqlCommand.CommandType = CommandType.Text
        SqlCommand.CommandTimeout = 60

        SqlCommand.CommandText = sQuery.ToString

        SqlReader = SqlCommand.ExecuteReader()

        If SqlReader.HasRows Then

          Do While SqlReader.Read

            If Not IsDBNull(SqlReader("pnum_type")) And Not IsDBNull(SqlReader("pnum_number_full")) Then
              sPhoneNumbers += SqlReader.Item("pnum_type").ToString.Trim + " : " + SqlReader.Item("pnum_number_full").ToString.Trim + "<br />"
            End If

          Loop

        End If

        SqlReader.Close()

      End If ' (inCompanyId >= 0) AND (inJournalId >= 0) AND (inContactId >= 0) 

    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in get_phone_info(ByVal inCompanyId As Long, ByVal inJournalId As Long, ByVal inContactId As Long) As String" + ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return sPhoneNumbers

  End Function

  Public Function include_excel_report_style() As String

    Dim htmlOut = New StringBuilder()

    htmlOut.Append("<style type=""text/css"">" + vbCrLf)
    htmlOut.Append("  td.textformat {mso-number-format:'\@'}" + vbCrLf)
    htmlOut.Append("  td.textdate {mso-number-format:'Short Date'}" + vbCrLf)
    htmlOut.Append("</style>")

    Return htmlOut.ToString
    htmlOut = Nothing

  End Function

  Public Function get_fractional_program_name(ByVal inProgramID As Long, ByRef outProgramComanyID As Long) As String

    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sProgramName As String = "UNKNOWN"

    Try

      sQuery.Append("SELECT DISTINCT prog_comp_id, prog_name")
      sQuery.Append(" FROM Aircraft_Programs WITH(NOLOCK) INNER JOIN program_reference WITH(NOLOCK) ON pgref_prog_id = prog_id")
      sQuery.Append(" WHERE prog_active_flag = 'Y' AND prog_id = " + inProgramID.ToString)

      'HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_fractional_program_name(ByVal inProgramID As Long) As String<br />" + sQuery.ToString

      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlCommand.CommandText = sQuery.ToString

      SqlReader = SqlCommand.ExecuteReader()

      If SqlReader.HasRows Then

        SqlReader.Read()

        If Not IsDBNull(SqlReader("prog_name")) Then
          If Not String.IsNullOrEmpty(SqlReader.Item("prog_name").ToString.Trim) Then
            sProgramName = SqlReader.Item("prog_name").ToString.Trim
          End If
        End If

        If Not IsDBNull(SqlReader("prog_comp_id")) Then
          If Not String.IsNullOrEmpty(SqlReader.Item("prog_comp_id").ToString.Trim) Then
            outProgramComanyID = CLng(SqlReader.Item("prog_comp_id").ToString)
          End If
        End If

      End If

      SqlReader.Close()

    Catch ex As Exception
      aError = "Error in get_fractional_program_name(ByVal inProgramID As Long) As String" + ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return sProgramName

  End Function

  Public Function get_fractional_program_name_for_model(ByVal inAircraftID As Long, ByVal inModelID As Long) As String

    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sProgramName As String = "UNKNOWN"

    Try

      sQuery.Append("SELECT DISTINCT prog_name, ac_id FROM Aircraft_Programs WITH(NOLOCK)")
      sQuery.Append(" INNER JOIN program_reference WITH(NOLOCK) ON pgref_prog_id = prog_id,")
      sQuery.Append(" Aircraft WITH(NOLOCK) INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id")
      sQuery.Append(" WHERE (prog_active_flag = 'Y') AND (ac_journ_id = 0) and (ac_ownership_type='F') and (ac_id = " + inAircraftID.ToString + ")")

            If inModelID > 0 Then
                sQuery.Append(" AND (ac_amod_id = " & inModelID & ")")
            End If

            sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))

      sQuery.Append(" AND (ac_id IN (SELECT distinct cref_ac_id FROM aircraft_reference")
      sQuery.Append(" WITH(NOLOCK) WHERE (cref_contact_type='17') AND (cref_journ_id = 0) AND (cref_comp_id IN")
      sQuery.Append(" (SELECT DISTINCT pgref_comp_id FROM program_reference WITH(NOLOCK) WHERE pgref_prog_id = prog_id))))")

      'HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_fractional_program_name_for_model(ByVal inAircraftID As Long, ByVal inModelID As Long) As String<br />" + sQuery.ToString

      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlCommand.CommandText = sQuery.ToString

      SqlReader = SqlCommand.ExecuteReader()

      If SqlReader.HasRows Then

        SqlReader.Read()

        If CLng(SqlReader.Item("ac_id").ToString) = inAircraftID Then
          If Not IsDBNull(SqlReader("prog_name")) Then
            If Not String.IsNullOrEmpty(SqlReader.Item("prog_name").ToString.Trim) Then
              sProgramName = SqlReader.Item("prog_name").ToString.Trim
            End If
          End If
        End If

      End If

      SqlReader.Close()

    Catch ex As Exception
      aError = "Error in get_fractional_program_name_for_model(ByVal inAircraftID As Long, ByVal inModelID As Long) As String" + ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return sProgramName

  End Function

  Public Function get_fractional_program_holder_manager(ByVal inAircraftID As Long, ByVal inJournalID As Long, ByVal bGetManager As Boolean) As String

    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sProgramName As String = "UNKNOWN"

    Try

      If bGetManager Then
        sQuery.Append("SELECT TOP 1 comp_name FROM Company WITH (NOLOCK)")
        sQuery.Append(" INNER JOIN Aircraft_Reference WITH (NOLOCK) ON comp_id = cref_comp_id AND comp_journ_id = cref_journ_id")
        sQuery.Append(" WHERE cref_ac_id = " + inAircraftID.ToString + " AND cref_journ_id = " + inJournalID.ToString + " AND (cref_contact_type = '18' OR cref_business_type = 'PM')")
      Else
        sQuery.Append("SELECT TOP 1 comp_name FROM Company WITH (NOLOCK)")
        sQuery.Append(" INNER JOIN Aircraft_Reference WITH (NOLOCK) ON comp_id = cref_comp_id AND comp_journ_id = cref_journ_id")
        sQuery.Append(" WHERE cref_ac_id = " + inAircraftID.ToString + " AND cref_journ_id = " + inJournalID.ToString + " AND (cref_contact_type = '17' OR cref_business_type = 'PH')")
      End If

      'HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_fractional_program_holder_manager(ByVal inAircraftID As Long, ByVal inJournalID As Long, ByVal bGetManager As Boolean) As String<br />" + sQuery.ToString

      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlCommand.CommandText = sQuery.ToString

      SqlReader = SqlCommand.ExecuteReader()

      If SqlReader.HasRows Then

        SqlReader.Read()

        If Not IsDBNull(SqlReader("comp_name")) Then
          If Not String.IsNullOrEmpty(SqlReader.Item("comp_name").ToString.Trim) Then
            sProgramName = SqlReader.Item("comp_name").ToString.Trim
          End If
        End If

      End If

      SqlReader.Close()

    Catch ex As Exception
      aError = "Error in get_fractional_program_holder_manager(ByVal inAircraftID As Long, ByVal inJournalID As Long, ByVal bGetManager As Boolean) As String" + ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return sProgramName

  End Function

  Public Function get_all_cloud_notes(ByVal nAircraftID As Long, ByVal nYachtID As Long) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT ")
      sQuery.Append(" cn_id AS lnote_id, cn_ac_id AS lnote_jetnet_ac_id, cn_comp_id AS lnote_jetnet_comp_id, '0' AS lnote_client_ac_id, '0' AS lnote_client_comp_id,")
      sQuery.Append(" cn_contact_id AS lnote_jetnet_contact_id, '0' AS lnote_client_contact_id, cn_notes AS lnote_note, cn_entry_date AS lnote_entry_date,")
      sQuery.Append(" cn_action_date AS lnote_action_date, cn_user_login AS lnote_user_login, cn_user_name AS lnote_user_name, '25' AS lnote_notecat_key, cn_status AS lnote_status,")
      sQuery.Append(" cn_schedule_start_date AS lnote_schedule_start_date, cn_schedule_end_date AS lnote_schedule_end_date,")
      sQuery.Append(" cn_user_contact_id AS lnote_user_id, '0' AS lnote_clipri_ID, '' AS clipri_name,")
      sQuery.Append(" '0' AS clipri_sort_order, 'N' AS lnote_document_flag, cn_amod_id AS lnote_jetnet_amod_id, '0' AS lnote_client_amod_id,")
      sQuery.Append(" '' AS lnote_document_name, '' AS lnote_opportunity_status, '0' AS lnote_cASh_value, '0' AS lnote_capture_percentage,")
      sQuery.Append(" '' AS lnote_wanted_start_year, '' AS lnote_wanted_end_year, '0' AS lnote_wanted_max_price, '0' AS lnote_wanted_max_aftt,")
      sQuery.Append(" '' AS lnote_wanted_damage_hist, '' AS lnote_wanted_damage_cur")
      sQuery.Append(" from " + HttpContext.Current.Session.Item("localSubscription").crmCloudNotesDBName.ToString)

      sQuery.Append(" WHERE ")
      sQuery.Append(" cn_status = 'A' ")

      ' If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString.Trim) Then
      '   sQuery.Append(" and (cn_user_sub_id = " + HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString + ")")
      ' End If
      ' If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("localUser").crmUserLogin.ToString.Trim) Then
      '   sQuery.Append(" AND (cn_user_login = '" + HttpContext.Current.Session.Item("localUser").crmUserLogin.trim + "')")
      ' End If

      If nAircraftID > 0 Then
        sQuery.Append(" and (cn_ac_id = " + nAircraftID.ToString + ")")
      ElseIf nYachtID > 0 Then
        sQuery.Append(" and (cn_yt_id = " + nYachtID.ToString + ")")
      End If



      sQuery.Append(" ORDER BY cn_schedule_start_date")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>get_all_cloud_notes(ByVal nAircraftID As Long, ByVal nYachtID As Long) As DataTable</b><br />" + sQuery.ToString

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
        aError = "Error in get_all_cloud_notes load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_all_cloud_notes(ByVal nAircraftID As Long, ByVal nYachtID As Long) As DataTable " + ex.Message

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

  Public Function get_all_server_notes(ByVal nAircraftID As Long, ByVal nYachtID As Long) As DataTable

    Dim atemptable As New DataTable

    Dim dsServerNotes As New viewSelectionCriteriaClass
    Dim notesFunctions As New notes_view_functions

    notesFunctions.adminConnectStr = adminConnectString.Trim
    notesFunctions.clientConnectStr = clientConnectString.Trim
    notesFunctions.starConnectStr = starConnectString.Trim
    notesFunctions.serverConnectStr = serverConnectString.Trim
    notesFunctions.cloudConnectStr = cloudConnectString.Trim

    Try

      If nAircraftID > 0 Then
        dsServerNotes.ViewCriteriaNoteACSearchField = eNotesACSearchTypes.AIRCRAFT_ID
        dsServerNotes.ViewCriteriaNoteACSearchTextValue = nAircraftID.ToString
        dsServerNotes.ViewCriteriaNoteOrderBy = "DATE"
      ElseIf nYachtID > 0 Then
        dsServerNotes.ViewCriteriaNoteACSearchField = eNotesACSearchTypes.AIRCRAFT_ID
        dsServerNotes.ViewCriteriaNoteACSearchTextValue = nYachtID.ToString
        dsServerNotes.ViewCriteriaNoteOrderBy = "DATE"
      End If

      atemptable = notesFunctions.get_notes_view_all_aircraft_server_notes(dsServerNotes)

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_all_server_notes(ByVal nAircraftID As Long, ByVal nYachtID As Long) As DataTable " + ex.Message
    End Try

    Return atemptable

  End Function

  Public Function get_max_feature_codes(ByVal aircraftID As Long) As Integer

    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim nFeatureCount As Integer = 0

    Try

      sQuery.Append("SELECT COUNT(afeat_ac_id) AS TotCnt FROM Aircraft_Key_Feature WITH(NOLOCK)")
      sQuery.Append(" WHERE (afeat_ac_id = " + aircraftID.ToString + ") AND (afeat_journ_id = 0)")

      'HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_max_feature_codes(ByVal aircraftID As Long) As Integer<br />" + sQuery.ToString

      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlCommand.CommandText = sQuery.ToString

      SqlReader = SqlCommand.ExecuteReader()

      If SqlReader.HasRows Then

        SqlReader.Read()

        If Not IsDBNull(SqlReader("TotCnt")) Then
          If Not String.IsNullOrEmpty(SqlReader.Item("TotCnt").ToString.Trim) Then
            nFeatureCount = CInt(SqlReader.Item("TotCnt").ToString)
          End If
        End If

      End If

      SqlReader.Close()

    Catch ex As Exception
      aError = "Error in get_max_feature_codes(ByVal aircraftID As Long) As Integer" + ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return nFeatureCount

  End Function

  Public Function get_avionics_description_report(ByVal lACid As Long, ByVal lJournID As Long, ByVal inAVstring As String, ByVal bJustDescription As Boolean) As String

    Dim sQuery = New StringBuilder()
    Dim htmlOut = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT TOP 1 av_description FROM Aircraft_Avionics WITH(NOLOCK) WHERE av_ac_id = " + lACid.ToString)
      sQuery.Append(" AND (av_ac_journ_id = " + lJournID.ToString + ")")
      sQuery.Append(" AND (av_name = '" + inAVstring.Trim + "')")

      'aError = "<br /><br />get_avionics_description_report(ByVal lACid As Long, ByVal lJournID As Long, ByVal inAVstring As String, ByVal bJustDescription As Boolean) As String<br />" + sQuery.ToString

      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlCommand.CommandText = sQuery.ToString

      SqlReader = SqlCommand.ExecuteReader()

      If SqlReader.HasRows Then

        Do While SqlReader.Read

          If Not IsDBNull(SqlReader.Item("av_description")) Then

            If Not String.IsNullOrEmpty(SqlReader.Item("av_description").ToString.Trim) Then
              If Not bJustDescription Then
                htmlOut.Append("<tr><td align=""left"" valign=""top"">" + SqlReader.Item("av_description").ToString.Trim + "</td></tr>")
              Else
                htmlOut.Append(SqlReader.Item("av_description").ToString.Trim)
              End If
            End If

          End If

        Loop

      End If

      SqlReader.Close()

    Catch ex As Exception
      aError = "Error in get_avionics_description_report(ByVal lACid As Long, ByVal lJournID As Long, ByVal inAVstring As String, ByVal bJustDescription As Boolean) As String" + ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return htmlOut.ToString

  End Function

  Public Function get_tbo_oncondition(ByVal aircraftID As Long) As String

    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sTboOnCondition As String = ""

    Try

      sQuery.Append("SELECT ac_engine_tbo_oc_flag FROM Aircraft WITH(NOLOCK)")
      sQuery.Append(" WHERE (ac_id = " + aircraftID.ToString + ") AND (ac_journ_id = 0)")

      'HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_tbo_oncondition(ByVal aircraftID As Long) As String<br />" + sQuery.ToString

      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlCommand.CommandText = sQuery.ToString

      SqlReader = SqlCommand.ExecuteReader()

      If SqlReader.HasRows Then

        SqlReader.Read()

        If Not IsDBNull(SqlReader("ac_engine_tbo_oc_flag")) Then
          If Not String.IsNullOrEmpty(SqlReader.Item("ac_engine_tbo_oc_flag").ToString.Trim) Then
            sTboOnCondition = IIf(SqlReader.Item("ac_engine_tbo_oc_flag").ToString.Trim.ToLower.Contains("y"), "Yes", "No")
          End If
        End If

      End If

      SqlReader.Close()

    Catch ex As Exception
      aError = "Error in get_tbo_oncondition(ByVal aircraftID As Long) As String" + ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return sTboOnCondition

  End Function

  Public Function get_exclusive_expiration_flag(ByVal aircraftID As Long, ByVal journalID As Long) As String

    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sExclusiveExpirationFlag As String = ""

    Try

      sQuery.Append("SELECT ac_exclusive_expiration_flag FROM Aircraft WITH(NOLOCK)")
      sQuery.Append(" WHERE (ac_id = " + aircraftID.ToString + ") AND (ac_journ_id = " + journalID.ToString + ")")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_exclusive_expiration_flag(ByVal aircraftID As Long, ByVal journalID As Long) As String<br />" + sQuery.ToString

      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlCommand.CommandText = sQuery.ToString

      SqlReader = SqlCommand.ExecuteReader()

      If SqlReader.HasRows Then

        SqlReader.Read()

        If Not IsDBNull(SqlReader("ac_exclusive_expiration_flag")) Then
          If Not String.IsNullOrEmpty(SqlReader.Item("ac_exclusive_expiration_flag").ToString.Trim) Then
            sExclusiveExpirationFlag = IIf(SqlReader.Item("ac_exclusive_expiration_flag").ToString.Trim.ToLower.Contains("y"), "Y", "N")
          End If
        End If

      End If

      SqlReader.Close()

    Catch ex As Exception
      aError = "Error inget_exclusive_expiration_flag(ByVal aircraftID As Long, ByVal journalID As Long) As String" + ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return sExclusiveExpirationFlag

  End Function

  Public Function get_contact_count_report(ByVal nCompanyID As Long, ByVal nAircraftID As Long, ByVal nAircraftJournalID As Long, ByVal inStrCompanyTypeNbr As String) As Integer

    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim recCount As Long = 0

    sQuery.Append("SELECT DISTINCT cref_contact_id, cref_transmit_seq_no FROM Aircraft_Reference WITH(NOLOCK)")
    sQuery.Append(" WHERE cref_ac_id = " + nAircraftID.ToString)
    sQuery.Append(" AND cref_journ_id = " + nAircraftJournalID.ToString)
    sQuery.Append(" AND cref_comp_id = " + nCompanyID.ToString)
    sQuery.Append(" AND cref_contact_type IN (" + inStrCompanyTypeNbr + ")")
    sQuery.Append(" ORDER BY cref_transmit_seq_no")

    Try

      'HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_contact_count_report(ByVal nCompanyID As Long, ByVal nAircraftID As Long, ByVal nAircraftJournalID As Long, ByVal inStrCompanyTypeNbr As String) As Integer<br />" + sQuery.ToString

      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60
      SqlCommand.CommandText = sQuery.ToString

      SqlReader = SqlCommand.ExecuteReader()

      If SqlReader.HasRows Then

        Do While SqlReader.Read()
          recCount += 1
        Loop

      End If

      SqlReader.Close()

    Catch ex As Exception
      aError = "Error in get_contact_count_report(ByVal nCompanyID As Long, ByVal nAircraftID As Long, ByVal nAircraftJournalID As Long, ByVal inStrCompanyTypeNbr As String) As Integer" + ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return recCount

  End Function

  Public Function get_journal_date(ByVal nJournalID As Long) As String

    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sJournalDate As String = ""

    Try

      sQuery.Append("SELECT journ_date FROM Journal WHERE journ_id = " + nJournalID.ToString)

      'HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_journal_date(ByVal nJournalID As Long) As String</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlCommand.CommandText = sQuery.ToString

      SqlReader = SqlCommand.ExecuteReader()

      If SqlReader.HasRows Then

        SqlReader.Read()

        If Not (IsDBNull(SqlReader("journ_date"))) Then
          If Not String.IsNullOrEmpty(SqlReader.Item("journ_date").ToString.Trim) Then
            sJournalDate = FormatDateTime(SqlReader.Item("journ_date").ToString.Trim, DateFormat.ShortDate)
          End If

        End If

      End If

      SqlReader.Close()

    Catch ex As Exception
      aError = "Error in get_journal_date(ByVal nJournalID As Long) As String" + ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return sJournalDate

  End Function

  Public Function get_lease_expire_confirm_date(ByVal nJournalID As Long, ByVal bIsLeaseExpired As Boolean) As String

    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sLeaseExpConfirmDate As String = ""

    Try

      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      If bIsLeaseExpired Then

        sQuery.Append("SELECT journ_subject FROM Journal WHERE journ_id = " + nJournalID.ToString)

        'HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_lease_expire_confirm_date(ByVal nJournalID As Long, ByVal bIsLeaseExpired As Boolean) As String</b><br />" + sQuery.ToString

        SqlCommand.CommandText = sQuery.ToString

        SqlReader = SqlCommand.ExecuteReader()

        If SqlReader.HasRows Then

          SqlReader.Read()

          If Not (IsDBNull(SqlReader("journ_subject"))) Then
            If Not String.IsNullOrEmpty(SqlReader.Item("journ_subject").ToString.Trim) Then

              If SqlReader.Item("journ_subject").ToString.Substring(0, 16).ToLower.Contains("lease expired on") Then

                If IsDate(SqlReader.Item("journ_subject").ToString.Substring(18, 10)) Then
                  sLeaseExpConfirmDate = FormatDateTime(SqlReader.Item("journ_subject").ToString.Substring(18, 10), DateFormat.ShortDate)
                End If

              End If

            End If

          End If

        End If

      Else

        sQuery.Append("SELECT aclease_exp_confirm_date FROM Aircraft_Lease WHERE aclease_journ_id = " + nJournalID.ToString)

        'HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_lease_expire_confirm_date(ByVal nJournalID As Long, ByVal bIsLeaseExpired As Boolean) As String</b><br />" + sQuery.ToString

        SqlCommand.CommandText = sQuery.ToString

        SqlReader = SqlCommand.ExecuteReader()

        If SqlReader.HasRows Then

          SqlReader.Read()

          If Not (IsDBNull(SqlReader("aclease_exp_confirm_date"))) Then
            If Not String.IsNullOrEmpty(SqlReader.Item("aclease_exp_confirm_date").ToString.Trim) Then
              sLeaseExpConfirmDate = FormatDateTime(SqlReader.Item("aclease_exp_confirm_date").ToString.Trim, DateFormat.ShortDate)
            End If

          End If

        End If

      End If

      SqlReader.Close()

    Catch ex As Exception
      aError = "Error in get_lease_expire_confirm_date(ByVal nJournalID As Long, ByVal bIsLeaseExpired As Boolean) As String" + ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return sLeaseExpConfirmDate

  End Function

  Public Function get_lease_expire_date(ByVal nAircraftID As Long, ByVal nJournalID As Long, ByVal inExpConfirmDate As String) As String

    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sLeaseExpireDate As String = ""

    Try

      If nAircraftID = 0 Then
        sQuery.Append("SELECT aclease_expiration_date FROM Aircraft_Lease WHERE aclease_journ_id = " + nJournalID.ToString)
      Else
        sQuery.Append("SELECT aclease_expiration_date FROM Aircraft_Lease, Journal")
        sQuery.Append(" WHERE aclease_journ_id = journ_id AND aclease_exp_confirm_date = '" + inExpConfirmDate.Trim + "'")
        sQuery.Append(" AND aclease_ac_id =  " + nAircraftID.ToString + " AND journ_subcategory_code LIKE 'L%'")
      End If

      'HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_lease_expire_date(ByVal nAircraftID As Long, ByVal nJournalID As Long, ByVal inExpConfirmDate As String) As String</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlCommand.CommandText = sQuery.ToString

      SqlReader = SqlCommand.ExecuteReader()

      If SqlReader.HasRows Then

        SqlReader.Read()

        If Not (IsDBNull(SqlReader("aclease_expiration_date"))) Then
          If Not String.IsNullOrEmpty(SqlReader.Item("aclease_expiration_date").ToString.Trim) Then
            sLeaseExpireDate = FormatDateTime(SqlReader.Item("aclease_expiration_date").ToString.Trim, DateFormat.ShortDate)
          End If

        End If

      End If

      SqlReader.Close()

    Catch ex As Exception
      aError = "Error in get_lease_expire_date(ByVal nAircraftID As Long, ByVal nJournalID As Long, ByVal inExpConfirmDate As String) As String" + ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return sLeaseExpireDate

  End Function

  Public Function display_equipment_details_report(ByRef currentRow As DataRow, ByVal nAircraftID As Long, ByVal nAircraftJournalID As Long) As String

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim SqlConnection As New System.Data.SqlClient.SqlConnection
    Dim lDataReader As System.Data.SqlClient.SqlDataReader = Nothing
    Dim htmlOut As StringBuilder = New StringBuilder()
    Dim sQuery As String = ""

    sQuery = "SELECT adet_data_name, adet_data_description FROM Aircraft_Details WITH(NOLOCK) WHERE adet_ac_id = " + nAircraftID.ToString
    sQuery &= " AND adet_journ_id = " + nAircraftJournalID.ToString + " AND lower(adet_data_type) = 'equipment'"

    Try

      SqlConnection.ConnectionString = clientConnectStr

      SqlConnection.Open()

      SqlCommand.Connection = SqlConnection
      SqlCommand.CommandTimeout = 1000
      SqlCommand.CommandText = sQuery.Trim

      lDataReader = SqlCommand.ExecuteReader()

      htmlOut.Append("<table class=""centerTable"" cellpadding=""2"" cellspacing=""0"" width=""100%"">")
      htmlOut.Append("<tr><td class=""bottom"" colspan=""2"" align=""left""><b>Equipment</b></td></tr>")

      If lDataReader.HasRows Then

        Do While lDataReader.Read()

          If Not IsDBNull(lDataReader.Item("adet_data_name")) And Not String.IsNullOrEmpty(lDataReader.Item("adet_data_name").ToString) Then
            htmlOut.Append("<tr><td valign=""middle"" align=""left""nowrap=""nowrap"" width=""10%"" style=""padding-left:5px;""><b>" + lDataReader.Item("adet_data_name").ToString.Trim + "</b></td>")

            If Not IsDBNull(lDataReader.Item("adet_data_description")) And Not String.IsNullOrEmpty(lDataReader.Item("adet_data_description").ToString) Then
              htmlOut.Append("<td valign=""middle"" align=""left"">" + lDataReader.Item("adet_data_description").ToString.Trim + "</td></tr>")
            Else
              htmlOut.Append("<td valign=""middle"" align=""left""></td></tr>")
            End If

          Else
            htmlOut.Append("<tr><td valign=""middle"" align=""left""></td><td valign=""middle"" align=""left""></td></tr>")
          End If

        Loop

      Else
        htmlOut.Append("<tr><td valign=""middle"" align=""left"" colspan=""2"">No Equipment Details</td></tr>")
      End If

      htmlOut.Append("</table>")

      lDataReader.Close()

    Catch SqlException

      SqlConnection.Dispose()
      SqlCommand.Dispose()

    Finally

      SqlCommand.Dispose()
      SqlConnection.Close()
      SqlConnection.Dispose()

    End Try

    lDataReader = Nothing
    SqlCommand = Nothing

    Return htmlOut.ToString.Trim

  End Function

  Public Function display_interior_details_report(ByRef currentRow As DataRow, ByVal nAircraftID As Long, ByVal nAircraftJournalID As Long) As String

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim SqlConnection As New System.Data.SqlClient.SqlConnection
    Dim lDataReader As System.Data.SqlClient.SqlDataReader = Nothing
    Dim htmlOut As StringBuilder = New StringBuilder()
    Dim sQuery As String = ""

    htmlOut.Append("<table class=""centerTable"" cellpadding=""2"" cellspacing=""0"" width=""100%"">")
    htmlOut.Append("<tr><td class=""bottom"" colspan=""2"" align=""left""><b>Interior</b></td></tr>")

    'htmlOut.Append("<tr><td valign=""middle"" align=""right"" width=""30%"">Rating</td>")
    'If Not IsDBNull(currentRow.Item("ac_interior_rating")) And Not String.IsNullOrEmpty(currentRow.Item("ac_interior_rating").ToString) Then
    '  htmlOut.Append("<td valign=""middle"" align=""left"">" + currentRow.Item("ac_interior_rating").ToString.Trim + "</td></tr>")
    'Else
    '  htmlOut.Append("<td valign=""middle"" align=""left"">&nbsp;</td></tr>")
    'End If

    'htmlOut.Append("<tr><td valign=""middle"" align=""right"">Done By</td>")
    'If Not IsDBNull(currentRow.Item("ac_interior_doneby_name")) And Not String.IsNullOrEmpty(currentRow.Item("ac_interior_doneby_name").ToString) Then
    '  htmlOut.Append("<td valign=""middle"" align=""left"">" + currentRow.Item("ac_interior_doneby_name").ToString.Trim + "</td></tr>")
    'Else
    '  htmlOut.Append("<td valign=""middle"" align=""left"">&nbsp;</td></tr>")
    'End If

    'htmlOut.Append("<tr><td valign=""middle"" align=""right"">MM/YYYY</td>")
    'If Not IsDBNull(currentRow.Item("ac_interior_moyear")) And Not String.IsNullOrEmpty(currentRow.Item("ac_interior_moyear").ToString) Then
    '  htmlOut.Append("<td valign=""middle"" align=""left"">" + currentRow.Item("ac_interior_moyear").ToString.Substring(0, 2) + "/" + currentRow.Item("ac_interior_moyear").ToString.Substring(2, 4) + "</td></tr>")
    'Else
    '  htmlOut.Append("<td valign=""middle"" align=""left"">&nbsp;</td></tr>")
    'End If

    'htmlOut.Append("<td valign=""middle"" align=""right"">Passengers</td>")
    'If Not IsDBNull(currentRow.Item("ac_passenger_count")) And Not String.IsNullOrEmpty(currentRow.Item("ac_passenger_count").ToString) Then
    '  htmlOut.Append("<td valign=""middle"" align=""left"">" + currentRow.Item("ac_passenger_count").ToString.Trim + "</td></tr>")
    'Else
    '  htmlOut.Append("<td valign=""middle"" align=""left"">&nbsp;</td></tr>")
    'End If

    'htmlOut.Append("<td valign=""middle"" align=""right"">Configuration</td>")
    'If Not IsDBNull(currentRow.Item("ac_interior_config_name")) And Not String.IsNullOrEmpty(currentRow.Item("ac_interior_config_name").ToString) Then
    '  htmlOut.Append("<td valign=""middle"" align=""left"">" + currentRow.Item("ac_interior_config_name").ToString.Trim + "</td></tr>")
    'Else
    '  htmlOut.Append("<td valign=""middle"" align=""left"">&nbsp;</td></tr>")
    'End If

    sQuery = "SELECT adet_data_name, adet_data_description FROM Aircraft_Details WITH(NOLOCK) WHERE adet_ac_id = " + nAircraftID.ToString
    sQuery &= " AND adet_journ_id = " + nAircraftJournalID.ToString + " AND lower(adet_data_type) = 'interior'"

    Try

      SqlConnection.ConnectionString = clientConnectStr

      SqlConnection.Open()

      SqlCommand.Connection = SqlConnection
      SqlCommand.CommandTimeout = 1000
      SqlCommand.CommandText = sQuery.Trim

      lDataReader = SqlCommand.ExecuteReader()

      If lDataReader.HasRows Then

        Do While lDataReader.Read()

          If Not IsDBNull(lDataReader.Item("adet_data_name")) And Not String.IsNullOrEmpty(lDataReader.Item("adet_data_name").ToString) Then
            htmlOut.Append("<tr><td valign=""middle"" align=""left"" nowrap=""nowrap"" width=""10%"" style=""padding-left:5px;""><b>" + lDataReader.Item("adet_data_name").ToString.Trim + "</b></td>")

            If Not IsDBNull(lDataReader.Item("adet_data_description")) And Not String.IsNullOrEmpty(lDataReader.Item("adet_data_description").ToString) Then
              htmlOut.Append("<td valign=""middle"" align=""left"">" + lDataReader.Item("adet_data_description").ToString.Trim + "</td></tr>")
            Else
              htmlOut.Append("<td valign=""middle"" align=""left""></td></tr>")
            End If

          Else
            htmlOut.Append("<tr><td valign=""middle"" align=""left""></td><td valign=""middle"" align=""left""></td></tr>")
          End If

        Loop

      Else
        htmlOut.Append("<tr><td valign=""middle"" align=""left"" colspan=""2"">No Interior Details</td></tr>")
      End If

      htmlOut.Append("</table>")

      lDataReader.Close()

    Catch SqlException

      SqlConnection.Dispose()
      SqlCommand.Dispose()

    Finally

      SqlCommand.Dispose()
      SqlConnection.Close()
      SqlConnection.Dispose()

    End Try

    lDataReader = Nothing
    SqlCommand = Nothing

    Return htmlOut.ToString.Trim

  End Function

  Public Function display_exterior_details_report(ByRef currentRow As DataRow, ByVal nAircraftID As Long, ByVal nAircraftJournalID As Long) As String

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim SqlConnection As New System.Data.SqlClient.SqlConnection
    Dim lDataReader As System.Data.SqlClient.SqlDataReader = Nothing
    Dim htmlOut As StringBuilder = New StringBuilder()
    Dim sQuery As String = ""

    htmlOut.Append("<table class=""centerTable"" cellpadding=""2"" cellspacing=""0"" width=""100%"">")
    htmlOut.Append("<tr><td class=""bottom"" colspan=""2"" align=""left""><b>Exterior</b></td></tr>")

    'htmlOut.Append("<tr><td valign='middle' align='right' width='30%'>Rating</td>")
    'If Not IsDBNull(currentRow.Item("ac_exterior_rating")) And Not String.IsNullOrEmpty(currentRow.Item("ac_exterior_rating").ToString) Then
    '  htmlOut.Append("<td valign='middle' align='left'>" + currentRow.Item("ac_exterior_rating").ToString.Trim + "</td></tr>")
    'Else
    '  htmlOut.Append("<td valign='middle' align='left'>&nbsp;</td></tr>")
    'End If

    'htmlOut.Append("<tr><td valign='middle' align='right'>Done By</td>")
    'If Not IsDBNull(currentRow.Item("ac_exterior_doneby_name")) And Not String.IsNullOrEmpty(currentRow.Item("ac_exterior_doneby_name").ToString) Then
    '  htmlOut.Append("<td valign='middle' align='left'>" + currentRow.Item("ac_exterior_doneby_name").ToString.Trim + "</td></tr>")
    'Else
    '  htmlOut.Append("<td valign='middle' align='left'>&nbsp;</td></tr>")
    'End If

    'htmlOut.Append("<tr><td valign='middle' align='right'>MM/YYYY</td>")
    'If Not IsDBNull(currentRow.Item("ac_exterior_moyear")) And Not String.IsNullOrEmpty(currentRow.Item("ac_exterior_moyear").ToString) Then
    '  htmlOut.Append("<td valign='middle' align='left'>" + currentRow.Item("ac_exterior_moyear").ToString.Substring(0, 2) + "/" + currentRow.Item("ac_exterior_moyear").ToString.Substring(2, 4) + "</td></tr>")
    'Else
    '  htmlOut.Append("<td valign='middle' align='left'>&nbsp;</td></tr>")
    'End If

    sQuery = "SELECT adet_data_name, adet_data_description FROM Aircraft_Details WITH(NOLOCK) WHERE adet_ac_id = " + nAircraftID.ToString
    sQuery &= " AND adet_journ_id = " + nAircraftJournalID.ToString + " AND lower(adet_data_type) = 'exterior'"

    Try

      SqlConnection.ConnectionString = clientConnectStr

      SqlConnection.Open()

      SqlCommand.Connection = SqlConnection
      SqlCommand.CommandTimeout = 1000
      SqlCommand.CommandText = sQuery.Trim

      lDataReader = SqlCommand.ExecuteReader()

      If lDataReader.HasRows Then

        Do While lDataReader.Read()

          If Not IsDBNull(lDataReader.Item("adet_data_name")) And Not String.IsNullOrEmpty(lDataReader.Item("adet_data_name").ToString) Then
            htmlOut.Append("<tr><td valign=""middle"" align=""left"" nowrap=""nowrap"" width=""10%"" style=""padding-left:5px;""><b>" + lDataReader.Item("adet_data_name").ToString.Trim + "</b></td>")

            If Not IsDBNull(lDataReader.Item("adet_data_description")) And Not String.IsNullOrEmpty(lDataReader.Item("adet_data_description").ToString) Then
              htmlOut.Append("<td valign=""middle"" align=""left"">" + lDataReader.Item("adet_data_description").ToString.Trim + "</td></tr>")
            Else
              htmlOut.Append("<td valign=""middle"" align=""left""></td></tr>")
            End If

          Else
            htmlOut.Append("<tr><td valign=""middle"" align=""left""></td><td valign=""middle"" align=""left""></td></tr>")
          End If
        Loop

      Else
        htmlOut.Append("<tr><td valign=""middle"" align=""left"" colspan=""2"">No Exterior Details</td></tr>")
      End If

      htmlOut.Append("</table>")

      lDataReader.Close()

    Catch SqlException

      SqlConnection.Dispose()
      SqlCommand.Dispose()

    Finally

      SqlCommand.Dispose()
      SqlConnection.Close()
      SqlConnection.Dispose()

    End Try

    lDataReader = Nothing
    SqlCommand = Nothing

    Return htmlOut.ToString.Trim

  End Function

  Public Function display_avionics_details_report(ByVal nAircraftID As Long, ByVal nAircraftJournalID As Long) As String

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim SqlConnection As New System.Data.SqlClient.SqlConnection
    Dim lDataReader As System.Data.SqlClient.SqlDataReader = Nothing
    Dim htmlOut As StringBuilder = New StringBuilder()
    Dim sQuery As String = ""

    sQuery = "SELECT av_name, av_description FROM Aircraft_Avionics WITH(NOLOCK) WHERE av_ac_id = " + nAircraftID.ToString + " AND av_ac_journ_id = " + nAircraftJournalID.ToString

    Try

      SqlConnection.ConnectionString = clientConnectStr

      SqlConnection.Open()

      SqlCommand.Connection = SqlConnection
      SqlCommand.CommandTimeout = 1000
      SqlCommand.CommandText = sQuery.Trim

      lDataReader = SqlCommand.ExecuteReader()

      htmlOut.Append("<table class=""centerTable"" cellpadding=""2"" cellspacing=""0"" width=""100%"">")

      If lDataReader.HasRows Then

        Do While lDataReader.Read()

          If Not IsDBNull(lDataReader.Item("av_name")) And Not String.IsNullOrEmpty(lDataReader.Item("av_name").ToString) Then
            htmlOut.Append("<tr><td valign=""middle"" align=""left"" width=""10%"" style=""padding-left:5px;""><b>" + lDataReader.Item("av_name").ToString.Trim + "</b></td>")

            If Not IsDBNull(lDataReader.Item("av_description")) And Not String.IsNullOrEmpty(lDataReader.Item("av_description").ToString) Then
              htmlOut.Append("<td valign=""middle"" align=""left"">" + lDataReader.Item("av_description").ToString.Trim + "</td>")
            Else
              htmlOut.Append("<td valign=""middle"" align=""left""></td>")
            End If

          Else
            htmlOut.Append("<tr><td valign=""middle"" align=""left"" colspan=""2""></td>")
          End If

          If lDataReader.Read() Then
            If Not IsDBNull(lDataReader.Item("av_name")) And Not String.IsNullOrEmpty(lDataReader.Item("av_name").ToString) Then
              htmlOut.Append("<td valign=""middle"" align=""left"" width=""10%""><b>" + lDataReader.Item("av_name").ToString.Trim + "</b></td>")

              If Not IsDBNull(lDataReader.Item("av_description")) And Not String.IsNullOrEmpty(lDataReader.Item("av_description").ToString) Then
                htmlOut.Append("<td valign=""middle"" align=""left"">" + lDataReader.Item("av_description").ToString.Trim + "</td></tr>")
              Else
                htmlOut.Append("<td valign=""middle"" align=""left""></td></tr>")
              End If

            Else
              htmlOut.Append("<td valign=""middle"" align=""left"" width=""10%""></td><td valign=""middle"" align=""left""></td></tr>")
            End If
          Else
            htmlOut.Append("<td valign=""middle"" align=""left"" colspan=""2""></td></tr>")
          End If

        Loop

      Else
        htmlOut.Append("<tr><td valign=""middle"" align=""left"">No Avionics Found</td></tr>")
      End If

      htmlOut.Append("</table>")

      lDataReader.Close()

    Catch SqlException

      SqlConnection.Dispose()
      SqlCommand.Dispose()

    Finally

      SqlCommand.Dispose()
      SqlConnection.Close()
      SqlConnection.Dispose()

    End Try

    lDataReader = Nothing
    SqlCommand = Nothing

    Return htmlOut.ToString.Trim


  End Function

  Public Function display_cockpit_details_report(ByVal nAircraftID As Long, ByVal nAircraftJournalID As Long) As String

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim SqlConnection As New System.Data.SqlClient.SqlConnection
    Dim lDataReader As System.Data.SqlClient.SqlDataReader = Nothing
    Dim htmlOut As StringBuilder = New StringBuilder()
    Dim sQuery As String = ""


    sQuery = "SELECT * FROM Aircraft_Details WITH(NOLOCK) WHERE adet_ac_id = " + nAircraftID.ToString
    sQuery &= " AND adet_journ_id = " + nAircraftJournalID.ToString + " AND lower(adet_data_type) = 'addl cockpit equipment'"

    Try

      SqlConnection.ConnectionString = clientConnectStr

      SqlConnection.Open()

      SqlCommand.Connection = SqlConnection
      SqlCommand.CommandTimeout = 1000
      SqlCommand.CommandText = sQuery.Trim

      lDataReader = SqlCommand.ExecuteReader()

      htmlOut.Append("<table class=""centerTable"" cellpadding=""2"" cellspacing=""0"" width=""100%"">")
      htmlOut.Append("<tr><td colspan=""2"" align=""left""><b>Addl Cockpit Equipment</b></td></tr>")

      If lDataReader.HasRows Then

        Do While lDataReader.Read()

          If Not IsDBNull(lDataReader.Item("adet_data_name")) And Not String.IsNullOrEmpty(lDataReader.Item("adet_data_name").ToString) Then
            htmlOut.Append("<tr><td valign=""middle"" align=""left"" nowrap=""nowrap"" width=""10%"" style=""padding-left:5px;""><b>" + lDataReader.Item("adet_data_name").ToString.Trim + "</b>:</td>")

            If Not IsDBNull(lDataReader.Item("adet_data_description")) And Not String.IsNullOrEmpty(lDataReader.Item("adet_data_description").ToString) Then
              htmlOut.Append("<td valign=""middle"" align=""left"">" + lDataReader.Item("adet_data_description").ToString.Trim + "</td></tr>")
            Else
              htmlOut.Append("<td valign=""middle"" align=""left""></td></tr>")
            End If

          Else
            htmlOut.Append("<tr><td valign=""middle"" align=""left""></td><td valign=""middle"" align=""left""></td></tr>")
          End If
        Loop

      Else
        htmlOut.Append("<tr><td valign=""middle"" align=""left"" colspan=""2"">No Cockpit Details</td></tr>")
      End If

      htmlOut.Append("</table>")

      lDataReader.Close()

    Catch SqlException

      SqlConnection.Dispose()
      SqlCommand.Dispose()

    Finally

      SqlCommand.Dispose()
      SqlConnection.Close()
      SqlConnection.Dispose()

    End Try

    lDataReader = Nothing
    SqlCommand = Nothing

    Return htmlOut.ToString.Trim

  End Function

  Public Function display_maintenance_details_report(ByVal nAircraftID As Long, ByVal nAircraftJournalID As Long) As String

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim SqlConnection As New System.Data.SqlClient.SqlConnection
    Dim lDataReader As System.Data.SqlClient.SqlDataReader = Nothing
    Dim htmlOut As StringBuilder = New StringBuilder()
    Dim sQuery As String = ""


    sQuery = "SELECT adet_data_name, adet_data_description FROM Aircraft_Details WITH(NOLOCK) WHERE adet_ac_id = " + nAircraftID.ToString
    sQuery &= " AND adet_journ_id = " + nAircraftJournalID.ToString + " AND lower(adet_data_type) = 'maintenance'"

    Try

      SqlConnection.ConnectionString = clientConnectStr

      SqlConnection.Open()

      SqlCommand.Connection = SqlConnection
      SqlCommand.CommandTimeout = 1000
      SqlCommand.CommandText = sQuery.Trim

      lDataReader = SqlCommand.ExecuteReader()

      htmlOut.Append("<table class=""centerTable"" cellpadding=""2"" cellspacing=""0"" width=""100%"">")
      htmlOut.Append("<tr><td colspan=""2"" align=""left""><b>Maintenance</b></td></tr>")

      If lDataReader.HasRows Then

        Do While lDataReader.Read()

          If Not IsDBNull(lDataReader.Item("adet_data_name")) And Not String.IsNullOrEmpty(lDataReader.Item("adet_data_name").ToString) Then
            htmlOut.Append("<tr><td valign=""middle"" align=""left"" nowrap=""nowrap"" width=""10%"" style=""padding-left:5px;""><b>" + lDataReader.Item("adet_data_name").ToString.Trim + "</b>:</td>")

            If Not IsDBNull(lDataReader.Item("adet_data_description")) And Not String.IsNullOrEmpty(lDataReader.Item("adet_data_description").ToString) Then
              htmlOut.Append("<td valign=""middle"" align=""left"">" + lDataReader.Item("adet_data_description").ToString.Trim + "</td></tr>")
            Else
              htmlOut.Append("<td valign=""middle"" align=""left""></td></tr>")
            End If

          Else
            htmlOut.Append("<tr><td valign=""middle"" align=""left""></td><td valign=""middle"" align=""left""></td></tr>")
          End If
        Loop

      Else
        htmlOut.Append("<tr><td valign=""middle"" align=""left"" colspan=""2"">No Maintenance Details</td></tr>")
      End If

      htmlOut.Append("</table>")

      lDataReader.Close()

    Catch SqlException

      SqlConnection.Dispose()
      SqlCommand.Dispose()

    Finally

      SqlCommand.Dispose()
      SqlConnection.Close()
      SqlConnection.Dispose()

    End Try

    lDataReader = Nothing
    SqlCommand = Nothing

    Return htmlOut.ToString.Trim

  End Function

  Public Sub get_company_info_by_reference(ByVal in_AircraftID As Long, ByVal in_AircraftJournalID As Long, ByVal cref_contact_type As String, ByRef compInfo() As String)

    Dim SqlException As System.Data.SqlClient.SqlException = Nothing
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim SqlConnection As New System.Data.SqlClient.SqlConnection
    Dim lDataReader As System.Data.SqlClient.SqlDataReader = Nothing
    Dim sQuery As New StringBuilder

    sQuery.Append("SELECT DISTINCT comp_name, comp_city, comp_state, comp_country, comp_id, cref_transmit_seq_no FROM Company WITH(NOLOCK)")
    sQuery.Append(" INNER JOIN Aircraft_Reference WITH(NOLOCK) ON (comp_id = cref_comp_id AND comp_journ_id = cref_journ_id)")
    sQuery.Append(" WHERE (cref_ac_id = " + in_AircraftID.ToString + " AND cref_journ_id = " + in_AircraftJournalID.ToString)
    sQuery.Append(" AND cref_contact_type IN (" + cref_contact_type + ")")

    If in_AircraftJournalID = 0 Then
      sQuery.Append(" AND comp_active_flag = 'Y'")
    End If

    sQuery.Append(" AND comp_hide_flag = 'N')")
    sQuery.Append(" ORDER BY cref_transmit_seq_no")

    compInfo(0) = ""
    compInfo(1) = ""
    compInfo(2) = ""
    compInfo(3) = ""

    Try

      SqlConnection.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim

      SqlConnection.Open()

      SqlCommand.Connection = SqlConnection
      SqlCommand.CommandTimeout = 1000
      SqlCommand.CommandText = sQuery.ToString

      lDataReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      If lDataReader.HasRows Then

        lDataReader.Read()

        If Not (IsDBNull(lDataReader("comp_name"))) And Not String.IsNullOrEmpty(lDataReader.Item("comp_name").ToString.Trim) Then
          compInfo(0) = Replace(lDataReader.Item("comp_name").ToString, Constants.cSingleSpace, Constants.cHTMLnbsp)
        End If

        If Not (IsDBNull(lDataReader("comp_city"))) And Not String.IsNullOrEmpty(lDataReader.Item("comp_city").ToString.Trim) Then
          compInfo(1) = lDataReader.Item("comp_city").ToString.Trim
        End If

        If Not (IsDBNull(lDataReader("comp_state"))) And Not String.IsNullOrEmpty(lDataReader.Item("comp_state").ToString.Trim) Then
          compInfo(2) = lDataReader.Item("comp_state").ToString.Trim
        End If

        If Not (IsDBNull(lDataReader("comp_country"))) And Not String.IsNullOrEmpty(lDataReader.Item("comp_country").ToString.Trim) Then
          If (lDataReader.Item("comp_country").ToString.ToUpper <> "UNITED STATES") Then
            compInfo(3) = lDataReader.Item("comp_country").ToString.Trim
          Else
            compInfo(3) = "US"
          End If
        End If

      End If

    Catch SqlException

      SqlConnection.Dispose()
      SqlCommand.Dispose()

    Finally

      SqlCommand.Dispose()
      SqlConnection.Close()
      SqlConnection.Dispose()

    End Try

    lDataReader = Nothing
    SqlCommand = Nothing
    SqlConnection = Nothing

  End Sub

#Region "support_functions"

#End Region

End Class
