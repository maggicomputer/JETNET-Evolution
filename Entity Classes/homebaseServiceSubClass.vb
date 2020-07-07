' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/Entity Classes/homebaseServiceSubClass.vb $
'$$Author: Mike $
'$$Date: 3/04/20 10:13a $
'$$Modtime: 3/04/20 9:48a $
'$$Revision: 17 $
'$$Workfile: homebaseServiceSubClass.vb $
'
' ********************************************************************************

<System.Serializable()> Public Class homebaseServiceClass
  Public Property serv_name() As String
  Public Property serv_database_name() As String
  Public Property serv_active_flag() As String

  Public Property serfreqan_sqlserver_name() As String
  Public Property serfreqan_database_name() As String
  Public Property serfreqan_user_id() As String
  Public Property serfreqan_password() As String
  Public Property serfreqan_appname() As String

  Public Sub New()

    Try

      serv_name = ""
      serv_database_name = ""
      serv_active_flag = ""

      serfreqan_sqlserver_name = ""
      serfreqan_database_name = ""
      serfreqan_user_id = ""
      serfreqan_password = ""
      serfreqan_appname = ""

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message

    End Try

  End Sub

  Public Sub New(ByVal servName As String)

    Try

      serv_name = servName
      serv_database_name = ""
      serv_active_flag = ""

      serfreqan_sqlserver_name = ""
      serfreqan_database_name = ""
      serfreqan_user_id = ""
      serfreqan_password = ""
      serfreqan_appname = ""

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message

    End Try

  End Sub

  Public Function getServiceDataTable(ByVal inSubID As Long, ByVal inCompID As Long) As DataTable

    Dim atemptable As New DataTable
    Dim subQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      subQuery.Append("SELECT * FROM " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Subscription WITH(NOLOCK)")
      subQuery.Append(" WHERE sub_id = @sub_id AND sub_comp_id = @sub_comp_id")

      SqlCommand.Parameters.AddWithValue("@sub_id", inSubID.ToString.Trim)
      SqlCommand.Parameters.AddWithValue("@sub_comp_id", inCompID.ToString.Trim)

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 90

      SqlCommand.CommandText = subQuery.ToString
      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()

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

  Public Sub fillServiceClass()

    Dim resultsTable As New DataTable

    Try

      'If sublogin_sub_id = 0 And String.IsNullOrEmpty(sublogin_login) And sublogin_contact_id = 0 Then
      '  Exit Sub
      'End If

      'resultsTable = getSubscriptionLoginDataTable(sublogin_sub_id, sublogin_login, sublogin_contact_id)

      If Not IsNothing(resultsTable) Then

        If resultsTable.Rows.Count > 0 Then

          For Each r As DataRow In resultsTable.Rows

            'If Not (IsDBNull(r("sublogin_password"))) Then
            '  sublogin_password = r.Item("sublogin_password").ToString.Trim
            'End If

            'If Not (IsDBNull(r("sublogin_active_flag"))) Then
            '  sublogin_active_flag = IIf(r.Item("sublogin_active_flag").ToString.ToUpper.Contains("Y"), True, False)
            'End If

            'If Not (IsDBNull(r("sublogin_demo_flag"))) Then
            '  sublogin_demo_flag = IIf(r.Item("sublogin_demo_flag").ToString.ToUpper.Contains("Y"), True, False)
            'End If

            'If Not (IsDBNull(r("sublogin_nbr_of_installs"))) Then
            '  If Not String.IsNullOrEmpty(r.Item("sublogin_nbr_of_installs").ToString.Trim) Then
            '    If IsNumeric(r.Item("sublogin_nbr_of_installs").ToString.Trim) Then
            '      sublogin_nbr_of_installs = CInt(r.Item("sublogin_nbr_of_installs").ToString.Trim)
            '    End If
            '  End If
            'End If

            'If Not (IsDBNull(r("sublogin_allow_export_flag"))) Then
            '  sublogin_allow_export_flag = IIf(r.Item("sublogin_allow_export_flag").ToString.ToUpper.Contains("Y"), True, False)
            'End If

            'If Not (IsDBNull(r("sublogin_allow_local_notes_flag"))) Then
            '  sublogin_allow_local_notes_flag = IIf(r.Item("sublogin_allow_local_notes_flag").ToString.ToUpper.Contains("Y"), True, False)
            'End If

            'If Not (IsDBNull(r("sublogin_allow_projects_flag"))) Then
            '  sublogin_allow_projects_flag = IIf(r.Item("sublogin_allow_projects_flag").ToString.ToUpper.Contains("Y"), True, False)
            'End If

            'If Not (IsDBNull(r("sublogin_allow_email_request_flag"))) Then
            '  sublogin_allow_email_request_flag = IIf(r.Item("sublogin_allow_email_request_flag").ToString.ToUpper.Contains("Y"), True, False)
            'End If

            'If Not (IsDBNull(r("sublogin_allow_event_request_flag"))) Then
            '  sublogin_allow_event_request_flag = IIf(r.Item("sublogin_allow_event_request_flag").ToString.ToUpper.Contains("Y"), True, False)
            'End If

            'If Not (IsDBNull(r("sublogin_bypass_active_x_registry_flag"))) Then
            '  sublogin_bypass_active_x_registry_flag = IIf(r.Item("sublogin_bypass_active_x_registry_flag").ToString.ToUpper.Contains("Y"), True, False)
            'End If

            'If Not (IsDBNull(r("sublogin_allow_text_message_flag"))) Then
            '  sublogin_allow_text_message_flag = IIf(r.Item("sublogin_allow_text_message_flag").ToString.ToUpper.Contains("Y"), True, False)
            'End If

          Next

        End If

      End If

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message

    End Try

  End Sub

  Public Sub updateServiceClass()
    Dim subLoginQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand

    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sSeperator = ""
    Try

      'If sublogin_sub_id = 0 And String.IsNullOrEmpty(sublogin_login) And sublogin_contact_id = 0 Then
      '  Exit Sub
      'End If

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 90

      subLoginQuery.Append("UPDATE Subscription_Install SET")

      'If Not String.IsNullOrEmpty(sublogin_password.Trim) Then

      '  subLoginQuery.Append(sSeperator + " sublogin_password = @sublogin_password")
      '  SqlCommand.Parameters.AddWithValue("@sublogin_password", sublogin_password.Trim)
      '  sSeperator = ","

      'End If

      '' set web action date to null for any changes
      'subLoginQuery.Append(sSeperator + " sublogin_web_action_date = @sublogin_web_action_date")
      'SqlCommand.Parameters.AddWithValue("@sublogin_web_action_date", DBNull.Value)

      'subLoginQuery.Append(" WHERE sublogin_sub_id = @sublogin_sub_id AND sublogin_login = @sublogin_login AND sublogin_contact_id = @sublogin_contact_id")

      'SqlCommand.Parameters.AddWithValue("@sublogin_sub_id", sublogin_sub_id.ToString.Trim)
      'SqlCommand.Parameters.AddWithValue("@sublogin_login", sublogin_login.Trim)
      'SqlCommand.Parameters.AddWithValue("@sublogin_contact_id", sublogin_contact_id.ToString.Trim)

      SqlCommand.CommandText = subLoginQuery.ToString

      SqlCommand.ExecuteNonQuery()

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message

    Finally

      SqlConn.Dispose()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

  End Sub

  Public Sub insertServiceClass()

    Dim subLoginQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand

    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      'If sublogin_sub_id = 0 And String.IsNullOrEmpty(sublogin_login) And sublogin_contact_id = 0 Then
      '  Exit Sub
      'End If

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 90

      subLoginQuery.Append("INSERT INTO Subscription (sublogin_password, sublogin_active_flag, sublogin_demo_flag, sublogin_nbr_of_installs, sublogin_allow_export_flag, sublogin_allow_local_notes_flag,")
      subLoginQuery.Append(" sublogin_allow_projects_flag, sublogin_allow_email_request_flag, sublogin_allow_event_request_flag, sublogin_bypass_active_x_registry_flag, sublogin_allow_text_message_flag, sublogin_web_action_date")
      subLoginQuery.Append(") VALUES (@sublogin_password, @sublogin_active_flag, @sublogin_demo_flag, @sublogin_nbr_of_installs, @sublogin_allow_export_flag, @sublogin_allow_local_notes_flag,")
      subLoginQuery.Append(" @sublogin_allow_projects_flag, @sublogin_allow_email_request_flag, @sublogin_allow_event_request_flag, @sublogin_bypass_active_x_registry_flag, @sublogin_allow_text_message_flag, @sublogin_web_action_date")
      subLoginQuery.Append(") WHERE sublogin_sub_id = @sublogin_sub_id AND sublogin_login = @sublogin_login AND sublogin_contact_id = @sublogin_contact_id")

      'If Not String.IsNullOrEmpty(sublogin_password.Trim) Then
      '  SqlCommand.Parameters.AddWithValue("@sublogin_password", sublogin_password.Trim)
      'End If

      '' set web action date to null for any changes
      'SqlCommand.Parameters.AddWithValue("@sublogin_web_action_date", DBNull.Value)


      'SqlCommand.Parameters.AddWithValue("@sublogin_sub_id", sublogin_sub_id.ToString.Trim)
      'SqlCommand.Parameters.AddWithValue("@sublogin_login", sublogin_login.Trim)
      'SqlCommand.Parameters.AddWithValue("@sublogin_contact_id", sublogin_contact_id.ToString.Trim)

      SqlCommand.CommandText = subLoginQuery.ToString

      SqlCommand.ExecuteNonQuery()

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
    Dim [class] = TryCast(obj, homebaseServiceClass)
    Return [class] IsNot Nothing AndAlso
           serv_name = [class].serv_name AndAlso
           serv_database_name = [class].serv_database_name AndAlso
           serv_active_flag = [class].serv_active_flag AndAlso
           serfreqan_sqlserver_name = [class].serfreqan_sqlserver_name AndAlso
           serfreqan_database_name = [class].serfreqan_database_name AndAlso
           serfreqan_user_id = [class].serfreqan_user_id AndAlso
           serfreqan_password = [class].serfreqan_password AndAlso
           serfreqan_appname = [class].serfreqan_appname
  End Function

  Public Shared Operator =(class1 As homebaseServiceClass, class2 As homebaseServiceClass) As Boolean
    Return EqualityComparer(Of homebaseServiceClass).Default.Equals(class1, class2)
  End Operator

  Public Shared Operator <>(class1 As homebaseServiceClass, class2 As homebaseServiceClass) As Boolean
    Return Not class1 = class2
  End Operator

End Class

<System.Serializable()> Public Class homebaseSubscriptionClass

  Public Property sub_id() As Long
  Public Property sub_comp_id() As Long
  Public Property sub_contact_id() As Long
  Public Property sub_serv_code() As String
  Public Property sub_start_date() As String
  Public Property sub_end_date() As String
  Public Property sub_tech_id() As String
  Public Property sub_marketing_flag() As Boolean
  Public Property sub_nbr_days_expire() As Integer
  Public Property sub_business_aircraft_flag() As Boolean
  Public Property sub_busair_tier_level() As String
  Public Property sub_helicopters_flag() As Boolean
  Public Property sub_commerical_flag() As Boolean
  Public Property sub_regional_flag() As Boolean
  Public Property sub_aerodex_flag() As Boolean
  Public Property sub_frequency() As String
  Public Property sub_nbr_of_installs() As Integer
  Public Property sub_contract_amount() As Double
  Public Property sub_abi_flag() As Boolean
  Public Property sub_starreports_flag() As Boolean
  Public Property sub_server_side_notes_flag() As Boolean
  Public Property sub_yacht_flag() As Boolean
  Public Property sub_history_flag() As Boolean
  Public Property sub_server_side_dbase_name() As String
  Public Property sub_server_side_crm_regid() As Long
  Public Property sub_sale_price_flag() As Boolean
  Public Property sub_nbr_of_spi_installs() As Integer
  Public Property sub_cloud_notes_flag() As Boolean
  Public Property sub_cloud_notes_database() As String
  Public Property sub_parent_sub_id() As Long
  Public Property sub_share_by_parent_sub_id_flag() As Boolean
  Public Property sub_share_by_comp_id_flag() As Boolean
  Public Property sub_max_allowed_custom_export() As Long

  Public Property sub_callback_date() As String
  Public Property sub_callback_status() As String
  Public Property sub_callback_comment() As String

  Public Property sub_serv_english_desc() As String

  Public Sub New()

    Try

      sub_id = 0
      sub_comp_id = 0

      sub_contact_id = 0
      sub_serv_code = ""
      sub_start_date = ""
      sub_end_date = ""
      sub_tech_id = ""
      sub_marketing_flag = False
      sub_nbr_days_expire = 0
      sub_business_aircraft_flag = False
      sub_busair_tier_level = ""
      sub_helicopters_flag = False
      sub_commerical_flag = False
      sub_regional_flag = False
      sub_aerodex_flag = False
      sub_frequency = ""
      sub_nbr_of_installs = 0
      sub_contract_amount = 0
      sub_abi_flag = False
      sub_starreports_flag = False
      sub_server_side_notes_flag = False
      sub_yacht_flag = False
      sub_history_flag = False
      sub_server_side_dbase_name = ""
      sub_server_side_crm_regid = 0
      sub_sale_price_flag = False
      sub_nbr_of_spi_installs = 0
      sub_cloud_notes_flag = False
      sub_cloud_notes_database = ""
      sub_parent_sub_id = 0
      sub_share_by_parent_sub_id_flag = False
      sub_share_by_comp_id_flag = False
      sub_max_allowed_custom_export = 0

      sub_callback_date = ""
      sub_callback_status = ""
      sub_callback_comment = ""

      sub_serv_english_desc = "" ' reference only

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message

    End Try

  End Sub

  Public Sub New(ByVal subId As Long, ByVal subCompId As Long)

    Try

      sub_id = subId
      sub_comp_id = subCompId

      sub_contact_id = 0
      sub_serv_code = ""
      sub_start_date = ""
      sub_end_date = ""
      sub_tech_id = ""
      sub_marketing_flag = False
      sub_nbr_days_expire = 0
      sub_business_aircraft_flag = False
      sub_busair_tier_level = ""
      sub_helicopters_flag = False
      sub_commerical_flag = False
      sub_regional_flag = False
      sub_aerodex_flag = False
      sub_frequency = ""
      sub_nbr_of_installs = 0
      sub_contract_amount = 0
      sub_abi_flag = False
      sub_starreports_flag = False
      sub_server_side_notes_flag = False
      sub_yacht_flag = False
      sub_history_flag = False
      sub_server_side_dbase_name = ""
      sub_server_side_crm_regid = 0
      sub_sale_price_flag = False
      sub_nbr_of_spi_installs = 0
      sub_cloud_notes_flag = False
      sub_cloud_notes_database = ""
      sub_parent_sub_id = 0
      sub_share_by_parent_sub_id_flag = False
      sub_share_by_comp_id_flag = False
      sub_max_allowed_custom_export = 0

      sub_callback_date = ""
      sub_callback_status = ""
      sub_callback_comment = ""

      sub_serv_english_desc = "" ' reference only

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message

    End Try

  End Sub

  Public Function getSubscriptionDataTable(ByVal inSubID As Long, ByVal inCompID As Long) As DataTable

    Dim atemptable As New DataTable
    Dim subQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      subQuery.Append("SELECT * FROM " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Subscription WITH(NOLOCK)")
      subQuery.Append(" WHERE sub_id = @sub_id AND sub_comp_id = @sub_comp_id")

      SqlCommand.Parameters.AddWithValue("@sub_id", inSubID.ToString.Trim)
      SqlCommand.Parameters.AddWithValue("@sub_comp_id", inCompID.ToString.Trim)

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 90

      SqlCommand.CommandText = subQuery.ToString
      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()

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

  Public Sub fillSubscriptionClass()

    Dim resultsTable As New DataTable

    Try

      If sub_id = 0 And sub_comp_id = 0 Then
        Exit Sub
      End If

      resultsTable = getSubscriptionDataTable(sub_id, sub_comp_id)

      If Not IsNothing(resultsTable) Then

        If resultsTable.Rows.Count > 0 Then

          For Each r As DataRow In resultsTable.Rows

            If Not (IsDBNull(r("sub_contact_id"))) Then
              If Not String.IsNullOrEmpty(r.Item("sub_contact_id").ToString.Trim) Then
                If IsNumeric(r.Item("sub_contact_id").ToString.Trim) Then
                  sub_contact_id = CLng(r.Item("sub_contact_id").ToString.Trim)
                End If
              End If
            End If

            If Not (IsDBNull(r("sub_serv_code"))) Then
              sub_serv_code = r.Item("sub_serv_code").ToString.Trim
            End If

            If Not (IsDBNull(r("sub_service_name"))) Then
              sub_serv_english_desc = r.Item("sub_service_name").ToString.Trim
            End If

            If Not (IsDBNull(r("sub_start_date"))) Then
              sub_start_date = r.Item("sub_start_date").ToString.Trim
            End If

            If Not (IsDBNull(r("sub_end_date"))) Then
              sub_end_date = r.Item("sub_end_date").ToString.Trim
            End If

            If Not (IsDBNull(r("sub_tech_id"))) Then
              sub_tech_id = r.Item("sub_tech_id").ToString.Trim
            End If

            If Not (IsDBNull(r("sub_marketing_flag"))) Then
              sub_marketing_flag = IIf(r.Item("sub_marketing_flag").ToString.ToUpper.Contains("Y"), True, False)
            End If

            If Not (IsDBNull(r("sub_nbr_days_expire"))) Then
              If Not String.IsNullOrEmpty(r.Item("sub_nbr_days_expire").ToString.Trim) Then
                If IsNumeric(r.Item("sub_nbr_days_expire").ToString.Trim) Then
                  sub_nbr_days_expire = CInt(r.Item("sub_nbr_days_expire").ToString.Trim)
                End If
              End If
            End If

            If Not (IsDBNull(r("sub_business_aircraft_flag"))) Then
              sub_business_aircraft_flag = IIf(r.Item("sub_business_aircraft_flag").ToString.ToUpper.Contains("Y"), True, False)
            End If

            If Not (IsDBNull(r("sub_busair_tier_level"))) Then
              sub_busair_tier_level = r.Item("sub_busair_tier_level").ToString.Trim
            End If

            If Not (IsDBNull(r("sub_helicopters_flag"))) Then
              sub_helicopters_flag = IIf(r.Item("sub_helicopters_flag").ToString.ToUpper.Contains("Y"), True, False)
            End If

            If Not (IsDBNull(r("sub_commerical_flag"))) Then
              sub_commerical_flag = IIf(r.Item("sub_commerical_flag").ToString.ToUpper.Contains("Y"), True, False)
            End If

            If Not (IsDBNull(r("sub_regional_flag"))) Then
              sub_regional_flag = IIf(r.Item("sub_regional_flag").ToString.ToUpper.Contains("Y"), True, False)
            End If

            If Not (IsDBNull(r("sub_aerodex_flag"))) Then
              sub_aerodex_flag = IIf(r.Item("sub_aerodex_flag").ToString.ToUpper.Contains("Y"), True, False)
            End If

            If Not (IsDBNull(r("sub_frequency"))) Then
              sub_frequency = r.Item("sub_frequency").ToString.Trim
            End If

            If Not (IsDBNull(r("sub_nbr_of_installs"))) Then
              If Not String.IsNullOrEmpty(r.Item("sub_nbr_of_installs").ToString.Trim) Then
                If IsNumeric(r.Item("sub_nbr_of_installs").ToString.Trim) Then
                  sub_nbr_of_installs = CInt(r.Item("sub_nbr_of_installs").ToString.Trim)
                End If
              End If
            End If

            If Not (IsDBNull(r("sub_contract_amount"))) Then
              If Not String.IsNullOrEmpty(r.Item("sub_contract_amount").ToString.Trim) Then
                If IsNumeric(r.Item("sub_contract_amount").ToString.Trim) Then
                  sub_contract_amount = CDbl(r.Item("sub_contract_amount").ToString.Trim)
                End If
              End If
            End If

            If Not (IsDBNull(r("sub_abi_flag"))) Then
              sub_abi_flag = IIf(r.Item("sub_abi_flag").ToString.ToUpper.Contains("Y"), True, False)
            End If

            If Not (IsDBNull(r("sub_starreports_flag"))) Then
              sub_starreports_flag = IIf(r.Item("sub_starreports_flag").ToString.ToUpper.Contains("Y"), True, False)
            End If

            If Not (IsDBNull(r("sub_server_side_notes_flag"))) Then
              sub_server_side_notes_flag = IIf(r.Item("sub_server_side_notes_flag").ToString.ToUpper.Contains("Y"), True, False)
            End If

            If Not (IsDBNull(r("sub_yacht_flag"))) Then
              sub_yacht_flag = IIf(r.Item("sub_yacht_flag").ToString.ToUpper.Contains("Y"), True, False)
            End If

            If Not (IsDBNull(r("sub_history_flag"))) Then
              sub_history_flag = IIf(r.Item("sub_history_flag").ToString.ToUpper.Contains("Y"), True, False)
            End If

            If Not (IsDBNull(r("sub_server_side_dbase_name"))) Then
              sub_server_side_dbase_name = r.Item("sub_server_side_dbase_name").ToString.Trim
            End If

            If Not (IsDBNull(r("sub_server_side_crm_regid"))) Then
              If Not String.IsNullOrEmpty(r.Item("sub_server_side_crm_regid").ToString.Trim) Then
                If IsNumeric(r.Item("sub_server_side_crm_regid").ToString.Trim) Then
                  sub_server_side_crm_regid = CLng(r.Item("sub_server_side_crm_regid").ToString.Trim)
                End If
              End If
            End If

            If Not (IsDBNull(r("sub_sale_price_flag"))) Then
              sub_sale_price_flag = IIf(r.Item("sub_sale_price_flag").ToString.ToUpper.Contains("Y"), True, False)
            End If

            If Not (IsDBNull(r("sub_nbr_of_spi_installs"))) Then
              If Not String.IsNullOrEmpty(r.Item("sub_nbr_of_spi_installs").ToString.Trim) Then
                If IsNumeric(r.Item("sub_nbr_of_spi_installs").ToString.Trim) Then
                  sub_nbr_of_spi_installs = CInt(r.Item("sub_nbr_of_spi_installs").ToString.Trim)
                End If
              End If
            End If

            If Not (IsDBNull(r("sub_cloud_notes_flag"))) Then
              sub_cloud_notes_flag = IIf(r.Item("sub_cloud_notes_flag").ToString.ToUpper.Contains("Y"), True, False)
            End If

            If Not (IsDBNull(r("sub_cloud_notes_database"))) Then
              sub_cloud_notes_database = r.Item("sub_cloud_notes_database").ToString.Trim
            End If

            If Not (IsDBNull(r("sub_parent_sub_id"))) Then
              If Not String.IsNullOrEmpty(r.Item("sub_parent_sub_id").ToString.Trim) Then
                If IsNumeric(r.Item("sub_parent_sub_id").ToString.Trim) Then
                  sub_parent_sub_id = CLng(r.Item("sub_parent_sub_id").ToString.Trim)
                End If
              End If
            End If

            If Not (IsDBNull(r("sub_share_by_parent_sub_id_flag"))) Then
              sub_share_by_parent_sub_id_flag = IIf(r.Item("sub_share_by_parent_sub_id_flag").ToString.ToUpper.Contains("Y"), True, False)
            End If

            If Not (IsDBNull(r("sub_share_by_comp_id_flag"))) Then
              sub_share_by_comp_id_flag = IIf(r.Item("sub_share_by_comp_id_flag").ToString.ToUpper.Contains("Y"), True, False)
            End If

            If Not (IsDBNull(r("sub_max_allowed_custom_export"))) Then
              If Not String.IsNullOrEmpty(r.Item("sub_max_allowed_custom_export").ToString.Trim) Then
                If IsNumeric(r.Item("sub_max_allowed_custom_export").ToString.Trim) Then
                  sub_max_allowed_custom_export = CLng(r.Item("sub_max_allowed_custom_export").ToString.Trim)
                End If
              End If
            End If

            If Not (IsDBNull(r("sub_callback_date"))) Then
              sub_callback_date = r.Item("sub_callback_date").ToString.Trim
            End If

            If Not (IsDBNull(r("sub_callback_status"))) Then
              sub_callback_status = r.Item("sub_callback_status").ToString.Trim
            End If

            If Not (IsDBNull(r("sub_callback_comment"))) Then
              sub_callback_comment = r.Item("sub_callback_comment").ToString.Trim
            End If

          Next

        End If

      End If

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message

    End Try

  End Sub

  Public Sub updateSubscriptionClass()
    Dim subQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand

    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sSeperator = ""
    Try

      If sub_id = 0 And sub_comp_id = 0 Then
        Exit Sub
      End If

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 90

      subQuery.Append("UPDATE Subscription SET")

      If sub_contact_id >= 0 Then

        subQuery.Append(" sub_contact_id = @sub_contact_id")
        SqlCommand.Parameters.AddWithValue("@sub_contact_id", sub_contact_id.ToString)
        sSeperator = ","

      End If

      If Not String.IsNullOrEmpty(sub_serv_code.Trim) Then

        subQuery.Append(sSeperator + " sub_serv_code = @sub_serv_code")
        SqlCommand.Parameters.AddWithValue("@sub_serv_code", sub_serv_code.Trim)
        sSeperator = ","

      End If

      If Not String.IsNullOrEmpty(sub_start_date.Trim) Then

        subQuery.Append(sSeperator + " sub_start_date = @sub_start_date")
        SqlCommand.Parameters.AddWithValue("@sub_start_date", sub_start_date.Trim)
        sSeperator = ","

      Else

        subQuery.Append(sSeperator + " sub_start_date = @sub_start_date")
        SqlCommand.Parameters.AddWithValue("@sub_start_date", DBNull.Value)
        sSeperator = ","

      End If

      If Not String.IsNullOrEmpty(sub_end_date.Trim) Then

        subQuery.Append(sSeperator + " sub_end_date = @sub_end_date")
        SqlCommand.Parameters.AddWithValue("@sub_end_date", sub_end_date.Trim)
        sSeperator = ","

      Else

        subQuery.Append(sSeperator + " sub_end_date = @sub_end_date")
        SqlCommand.Parameters.AddWithValue("@sub_end_date", DBNull.Value)
        sSeperator = ","

      End If

      If Not String.IsNullOrEmpty(sub_tech_id.Trim) Then

        subQuery.Append(sSeperator + " sub_tech_id = @sub_tech_id")
        SqlCommand.Parameters.AddWithValue("@sub_tech_id", sub_tech_id.Trim)
        sSeperator = ","

      End If

      subQuery.Append(sSeperator + " sub_marketing_flag = @sub_marketing_flag")
      SqlCommand.Parameters.AddWithValue("@sub_marketing_flag", IIf(sub_marketing_flag, "Y", "N"))
      sSeperator = ","

      If sub_nbr_days_expire >= 0 Then

        subQuery.Append(sSeperator + " sub_nbr_days_expire = @sub_nbr_days_expire")
        SqlCommand.Parameters.AddWithValue("@sub_nbr_days_expire", sub_nbr_days_expire.ToString)
        sSeperator = ","

      End If

      subQuery.Append(sSeperator + " sub_business_aircraft_flag = @sub_business_aircraft_flag")
      SqlCommand.Parameters.AddWithValue("@sub_business_aircraft_flag", IIf(sub_business_aircraft_flag, "Y", "N"))
      sSeperator = ","

      If Not String.IsNullOrEmpty(sub_busair_tier_level.Trim) Then

        subQuery.Append(sSeperator + " sub_busair_tier_level = @sub_busair_tier_level")
        SqlCommand.Parameters.AddWithValue("@sub_busair_tier_level", sub_busair_tier_level.Trim)
        sSeperator = ","

      End If

      subQuery.Append(sSeperator + " sub_helicopters_flag = @sub_helicopters_flag")
      SqlCommand.Parameters.AddWithValue("@sub_helicopters_flag", IIf(sub_helicopters_flag, "Y", "N"))
      sSeperator = ","

      subQuery.Append(sSeperator + " sub_commerical_flag = @sub_commerical_flag")
      SqlCommand.Parameters.AddWithValue("@sub_commerical_flag", IIf(sub_commerical_flag, "Y", "N"))
      sSeperator = ","

      subQuery.Append(sSeperator + " sub_regional_flag = @sub_regional_flag")
      SqlCommand.Parameters.AddWithValue("@sub_regional_flag", IIf(sub_regional_flag, "Y", "N"))
      sSeperator = ","

      subQuery.Append(sSeperator + " sub_aerodex_flag = @sub_aerodex_flag")
      SqlCommand.Parameters.AddWithValue("@sub_aerodex_flag", IIf(sub_aerodex_flag, "Y", "N"))
      sSeperator = ","

      If Not String.IsNullOrEmpty(sub_frequency.Trim) Then

        subQuery.Append(sSeperator + " sub_frequency = @sub_frequency")
        SqlCommand.Parameters.AddWithValue("@sub_frequency", sub_frequency.Trim)
        sSeperator = ","

      End If

      If sub_nbr_of_installs >= 0 Then

        subQuery.Append(sSeperator + " sub_nbr_of_installs = @sub_nbr_of_installs")
        SqlCommand.Parameters.AddWithValue("@sub_nbr_of_installs", sub_nbr_of_installs.ToString)
        sSeperator = ","

      End If

      If sub_contract_amount >= 0 Then

        subQuery.Append(sSeperator + " sub_contract_amount = @sub_contract_amount")
        SqlCommand.Parameters.AddWithValue("@sub_contract_amount", sub_contract_amount.ToString)
        sSeperator = ","

      End If

      subQuery.Append(sSeperator + " sub_abi_flag = @sub_abi_flag")
      SqlCommand.Parameters.AddWithValue("@sub_abi_flag", IIf(sub_abi_flag, "Y", "N"))
      sSeperator = ","

      subQuery.Append(sSeperator + " sub_starreports_flag = @sub_starreports_flag")
      SqlCommand.Parameters.AddWithValue("@sub_starreports_flag", IIf(sub_starreports_flag, "Y", "N"))
      sSeperator = ","

      subQuery.Append(sSeperator + " sub_server_side_notes_flag = @sub_server_side_notes_flag")
      SqlCommand.Parameters.AddWithValue("@sub_server_side_notes_flag", IIf(sub_server_side_notes_flag, "Y", "N"))
      sSeperator = ","

      subQuery.Append(sSeperator + " sub_yacht_flag = @sub_yacht_flag")
      SqlCommand.Parameters.AddWithValue("@sub_yacht_flag", IIf(sub_yacht_flag, "Y", "N"))
      sSeperator = ","

      subQuery.Append(sSeperator + " sub_history_flag = @sub_history_flag")
      SqlCommand.Parameters.AddWithValue("@sub_history_flag", IIf(sub_history_flag, "Y", "N"))
      sSeperator = ","

      If Not String.IsNullOrEmpty(sub_server_side_dbase_name.Trim) Then

        subQuery.Append(sSeperator + " sub_server_side_dbase_name = @sub_server_side_dbase_name")
        SqlCommand.Parameters.AddWithValue("@sub_server_side_dbase_name", sub_server_side_dbase_name.Trim)
        sSeperator = ","

      End If

      If sub_server_side_crm_regid >= 0 Then

        subQuery.Append(sSeperator + " sub_server_side_crm_regid = @sub_server_side_crm_regid")
        SqlCommand.Parameters.AddWithValue("@sub_server_side_crm_regid", sub_server_side_crm_regid.ToString)
        sSeperator = ","

      End If

      subQuery.Append(sSeperator + " sub_sale_price_flag = @sub_sale_price_flag")
      SqlCommand.Parameters.AddWithValue("@sub_sale_price_flag", IIf(sub_sale_price_flag, "Y", "N"))
      sSeperator = ","

      If sub_nbr_of_spi_installs >= 0 Then

        subQuery.Append(sSeperator + " sub_nbr_of_spi_installs = @sub_nbr_of_spi_installs")
        SqlCommand.Parameters.AddWithValue("@sub_nbr_of_spi_installs", sub_nbr_of_spi_installs.ToString)
        sSeperator = ","

      End If

      subQuery.Append(sSeperator + " sub_cloud_notes_flag = @sub_cloud_notes_flag")
      SqlCommand.Parameters.AddWithValue("@sub_cloud_notes_flag", IIf(sub_cloud_notes_flag, "Y", "N"))
      sSeperator = ","

      If Not String.IsNullOrEmpty(sub_cloud_notes_database.Trim) Then

        subQuery.Append(sSeperator + " sub_cloud_notes_database = @sub_cloud_notes_database")
        SqlCommand.Parameters.AddWithValue("@sub_cloud_notes_database", sub_cloud_notes_database.Trim)
        sSeperator = ","

      End If

      If sub_parent_sub_id >= 0 Then

        subQuery.Append(sSeperator + " sub_parent_sub_id = @sub_parent_sub_id")
        SqlCommand.Parameters.AddWithValue("@sub_parent_sub_id", sub_parent_sub_id.ToString)
        sSeperator = ","

      End If

      subQuery.Append(sSeperator + " sub_share_by_parent_sub_id_flag = @sub_share_by_parent_sub_id_flag")
      SqlCommand.Parameters.AddWithValue("@sub_share_by_parent_sub_id_flag", IIf(sub_share_by_parent_sub_id_flag, "Y", "N"))
      sSeperator = ","

      subQuery.Append(sSeperator + " sub_share_by_comp_id_flag = @sub_share_by_comp_id_flag")
      SqlCommand.Parameters.AddWithValue("@sub_share_by_comp_id_flag", IIf(sub_share_by_comp_id_flag, "Y", "N"))
      sSeperator = ","

      If sub_max_allowed_custom_export >= 0 Then

        subQuery.Append(sSeperator + " sub_max_allowed_custom_export = @sub_max_allowed_custom_export")
        SqlCommand.Parameters.AddWithValue("@sub_max_allowed_custom_export", sub_max_allowed_custom_export.ToString)
        sSeperator = ","

      End If

      If Not String.IsNullOrEmpty(sub_callback_date.Trim) Then

        subQuery.Append(sSeperator + " sub_callback_date = @sub_callback_date")
        SqlCommand.Parameters.AddWithValue("@sub_callback_date", sub_callback_date.Trim)
        sSeperator = ","

      Else

        subQuery.Append(sSeperator + " sub_callback_date = @sub_callback_date")
        SqlCommand.Parameters.AddWithValue("@sub_callback_date", DBNull.Value)
        sSeperator = ","

      End If

      If Not String.IsNullOrEmpty(sub_callback_status.Trim) Then

        subQuery.Append(sSeperator + " sub_callback_status = @sub_callback_status")
        SqlCommand.Parameters.AddWithValue("@sub_callback_status", sub_callback_status.Trim)
        sSeperator = ","

      End If

      If Not String.IsNullOrEmpty(sub_callback_comment.Trim) Then

        subQuery.Append(sSeperator + " sub_callback_comment = @sub_callback_comment")
        SqlCommand.Parameters.AddWithValue("@sub_callback_comment", sub_tech_id.Trim)
        sSeperator = ","

      End If

      ' set web action date to null for any changes
      subQuery.Append(sSeperator + " sub_web_action_date = @sub_web_action_date")
      SqlCommand.Parameters.AddWithValue("@sub_web_action_date", DBNull.Value)

      subQuery.Append(" WHERE sub_id = @sub_id AND sub_comp_id = @sub_comp_id")

      SqlCommand.Parameters.AddWithValue("@sub_id", sub_id.ToString.Trim)
      SqlCommand.Parameters.AddWithValue("@sub_comp_id", sub_comp_id.ToString.Trim)

      SqlCommand.CommandText = subQuery.ToString

      SqlCommand.ExecuteNonQuery()

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message

    Finally

      SqlConn.Dispose()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

  End Sub

  Public Sub insertSubscriptionClass()

    Dim subQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand

    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      If sub_id = 0 And sub_comp_id = 0 Then
        Exit Sub
      End If

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 90

      subQuery.Append("INSERT INTO Subscription (sub_contact_id, sub_serv_code, sub_start_date, sub_end_date, sub_tech_id, sub_max_allowed_custom_export, sub_callback_date,")
      subQuery.Append(" sub_marketing_flag, sub_nbr_days_expire, sub_business_aircraft_flag, sub_busair_tier_level, sub_helicopters_flag, sub_commerical_flag, sub_callback_comment,")
      subQuery.Append(" sub_regional_flag, sub_aerodex_flag, sub_frequency, sub_nbr_of_installs, sub_contract_amount, sub_abi_flag, sub_history_flag, sub_callback_status,")
      subQuery.Append(" sub_starreports_flag, sub_server_side_notes_flag, sub_server_side_dbase_name, sub_yacht_flag, sub_server_side_crm_regid, sub_sale_price_flag, sub_nbr_of_spi_installs,")
      subQuery.Append(" sub_cloud_notes_flag, sub_cloud_notes_database, sub_parent_sub_id, sub_web_action_date, sub_share_by_parent_sub_id_flag, sub_share_by_comp_id_flag")
      subQuery.Append(") VALUES (@sub_contact_id, @sub_serv_code, @sub_start_date, @sub_end_date, @sub_tech_id, @sub_max_allowed_custom_export, @sub_callback_date,")
      subQuery.Append(" @sub_marketing_flag, @sub_nbr_days_expire, @sub_business_aircraft_flag, @sub_busair_tier_level, @sub_helicopters_flag, @sub_commerical_flag, @sub_callback_comment,")
      subQuery.Append(" @sub_regional_flag, @sub_aerodex_flag, @sub_frequency, @sub_nbr_of_installs, @sub_contract_amount, @sub_abi_flag, @sub_history_flag, @sub_callback_status,")
      subQuery.Append(" @sub_starreports_flag, @sub_server_side_notes_flag, @sub_server_side_dbase_name, @sub_yacht_flag, @sub_server_side_crm_regid, @sub_sale_price_flag, @sub_nbr_of_spi_installs,")
      subQuery.Append(" @sub_cloud_notes_flag, @sub_cloud_notes_database, @sub_parent_sub_id, @sub_web_action_date, @sub_share_by_parent_sub_id_flag, @sub_share_by_comp_id_flag")
      subQuery.Append(") WHERE sub_id = @sub_id AND sub_comp_id = @sub_comp_id")

      If sub_contact_id >= 0 Then
        SqlCommand.Parameters.AddWithValue("@sub_contact_id", sub_contact_id.ToString)
      End If

      If Not String.IsNullOrEmpty(sub_serv_code.Trim) Then
        SqlCommand.Parameters.AddWithValue("@sub_serv_code", sub_serv_code.Trim)
      End If

      If Not String.IsNullOrEmpty(sub_start_date.Trim) Then
        SqlCommand.Parameters.AddWithValue("@sub_start_date", sub_start_date.Trim)
      End If

      If Not String.IsNullOrEmpty(sub_end_date.Trim) Then
        SqlCommand.Parameters.AddWithValue("@sub_end_date", sub_end_date.Trim)
      End If

      If Not String.IsNullOrEmpty(sub_tech_id.Trim) Then
        SqlCommand.Parameters.AddWithValue("@sub_tech_id", sub_tech_id.Trim)
      End If

      SqlCommand.Parameters.AddWithValue("@sub_marketing_flag", IIf(sub_marketing_flag, "Y", "N"))

      If sub_nbr_days_expire >= 0 Then
        SqlCommand.Parameters.AddWithValue("@sub_nbr_days_expire", sub_nbr_days_expire.ToString)
      End If

      SqlCommand.Parameters.AddWithValue("@sub_business_aircraft_flag", IIf(sub_business_aircraft_flag, "Y", "N"))

      If Not String.IsNullOrEmpty(sub_busair_tier_level.Trim) Then
        SqlCommand.Parameters.AddWithValue("@sub_busair_tier_level", sub_busair_tier_level.Trim)
      End If

      SqlCommand.Parameters.AddWithValue("@sub_helicopters_flag", IIf(sub_helicopters_flag, "Y", "N"))

      SqlCommand.Parameters.AddWithValue("@sub_commerical_flag", IIf(sub_commerical_flag, "Y", "N"))

      SqlCommand.Parameters.AddWithValue("@sub_regional_flag", IIf(sub_regional_flag, "Y", "N"))

      SqlCommand.Parameters.AddWithValue("@sub_aerodex_flag", IIf(sub_aerodex_flag, "Y", "N"))

      If Not String.IsNullOrEmpty(sub_frequency.Trim) Then
        SqlCommand.Parameters.AddWithValue("@sub_frequency", sub_frequency.Trim)
      End If

      If sub_nbr_of_installs >= 0 Then
        SqlCommand.Parameters.AddWithValue("@sub_nbr_of_installs", sub_nbr_of_installs.ToString)
      End If

      If sub_contract_amount >= 0 Then
        SqlCommand.Parameters.AddWithValue("@sub_contract_amount", sub_contract_amount.ToString)
      End If

      SqlCommand.Parameters.AddWithValue("@sub_abi_flag", IIf(sub_abi_flag, "Y", "N"))

      SqlCommand.Parameters.AddWithValue("@sub_starreports_flag", IIf(sub_starreports_flag, "Y", "N"))

      SqlCommand.Parameters.AddWithValue("@sub_server_side_notes_flag", IIf(sub_server_side_notes_flag, "Y", "N"))

      SqlCommand.Parameters.AddWithValue("@sub_yacht_flag", IIf(sub_yacht_flag, "Y", "N"))

      SqlCommand.Parameters.AddWithValue("@sub_history_flag", IIf(sub_history_flag, "Y", "N"))

      If Not String.IsNullOrEmpty(sub_server_side_dbase_name.Trim) Then
        SqlCommand.Parameters.AddWithValue("@sub_server_side_dbase_name", sub_server_side_dbase_name.Trim)
      End If

      If sub_server_side_crm_regid >= 0 Then
        SqlCommand.Parameters.AddWithValue("@sub_server_side_crm_regid", sub_server_side_crm_regid.ToString)
      End If

      SqlCommand.Parameters.AddWithValue("@sub_sale_price_flag", IIf(sub_sale_price_flag, "Y", "N"))

      If sub_nbr_of_spi_installs >= 0 Then
        SqlCommand.Parameters.AddWithValue("@sub_nbr_of_spi_installs", sub_nbr_of_spi_installs.ToString)
      End If

      SqlCommand.Parameters.AddWithValue("@sub_cloud_notes_flag", IIf(sub_cloud_notes_flag, "Y", "N"))

      If Not String.IsNullOrEmpty(sub_cloud_notes_database.Trim) Then
        SqlCommand.Parameters.AddWithValue("@sub_cloud_notes_database", sub_cloud_notes_database.Trim)
      End If

      If sub_parent_sub_id >= 0 Then
        SqlCommand.Parameters.AddWithValue("@sub_parent_sub_id", sub_parent_sub_id.ToString)
      End If

      SqlCommand.Parameters.AddWithValue("@sub_share_by_parent_sub_id_flag", IIf(sub_share_by_parent_sub_id_flag, "Y", "N"))

      SqlCommand.Parameters.AddWithValue("@sub_share_by_comp_id_flag", IIf(sub_share_by_comp_id_flag, "Y", "N"))

      If sub_max_allowed_custom_export >= 0 Then
        SqlCommand.Parameters.AddWithValue("@sub_max_allowed_custom_export", sub_max_allowed_custom_export.ToString)
      End If

      If Not String.IsNullOrEmpty(sub_callback_date.Trim) Then
        SqlCommand.Parameters.AddWithValue("@sub_callback_date", sub_callback_date.Trim)
      End If

      If Not String.IsNullOrEmpty(sub_callback_status.Trim) Then
        SqlCommand.Parameters.AddWithValue("@sub_callback_status", sub_callback_status.Trim)
      End If

      If Not String.IsNullOrEmpty(sub_callback_comment.Trim) Then
        SqlCommand.Parameters.AddWithValue("@sub_callback_comment", sub_tech_id.Trim)
      End If

      ' set web action date to null for any changes
      SqlCommand.Parameters.AddWithValue("@sub_web_action_date", DBNull.Value)

      SqlCommand.Parameters.AddWithValue("@sub_id", sub_id.ToString.Trim)
      SqlCommand.Parameters.AddWithValue("@sub_comp_id", sub_comp_id.ToString.Trim)

      SqlCommand.CommandText = subQuery.ToString

      SqlCommand.ExecuteNonQuery()

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
    Dim [class] = TryCast(obj, homebaseSubscriptionClass)
    Return [class] IsNot Nothing AndAlso
           sub_id = [class].sub_id AndAlso
           sub_comp_id = [class].sub_comp_id AndAlso
           sub_contact_id = [class].sub_contact_id AndAlso
           sub_serv_code = [class].sub_serv_code AndAlso
           sub_start_date = [class].sub_start_date AndAlso
           sub_end_date = [class].sub_end_date AndAlso
           sub_tech_id = [class].sub_tech_id AndAlso
           sub_marketing_flag = [class].sub_marketing_flag AndAlso
           sub_nbr_days_expire = [class].sub_nbr_days_expire AndAlso
           sub_business_aircraft_flag = [class].sub_business_aircraft_flag AndAlso
           sub_busair_tier_level = [class].sub_busair_tier_level AndAlso
           sub_helicopters_flag = [class].sub_helicopters_flag AndAlso
           sub_commerical_flag = [class].sub_commerical_flag AndAlso
           sub_regional_flag = [class].sub_regional_flag AndAlso
           sub_aerodex_flag = [class].sub_aerodex_flag AndAlso
           sub_frequency = [class].sub_frequency AndAlso
           sub_nbr_of_installs = [class].sub_nbr_of_installs AndAlso
           sub_contract_amount = [class].sub_contract_amount AndAlso
           sub_abi_flag = [class].sub_abi_flag AndAlso
           sub_starreports_flag = [class].sub_starreports_flag AndAlso
           sub_server_side_notes_flag = [class].sub_server_side_notes_flag AndAlso
           sub_yacht_flag = [class].sub_yacht_flag AndAlso
           sub_history_flag = [class].sub_history_flag AndAlso
           sub_server_side_dbase_name = [class].sub_server_side_dbase_name AndAlso
           sub_server_side_crm_regid = [class].sub_server_side_crm_regid AndAlso
           sub_sale_price_flag = [class].sub_sale_price_flag AndAlso
           sub_nbr_of_spi_installs = [class].sub_nbr_of_spi_installs AndAlso
           sub_cloud_notes_flag = [class].sub_cloud_notes_flag AndAlso
           sub_cloud_notes_database = [class].sub_cloud_notes_database AndAlso
           sub_parent_sub_id = [class].sub_parent_sub_id AndAlso
           sub_share_by_parent_sub_id_flag = [class].sub_share_by_parent_sub_id_flag AndAlso
           sub_share_by_comp_id_flag = [class].sub_share_by_comp_id_flag AndAlso
           sub_max_allowed_custom_export = [class].sub_max_allowed_custom_export AndAlso
           sub_callback_date = [class].sub_callback_date AndAlso
           sub_callback_status = [class].sub_callback_status AndAlso
           sub_callback_comment = [class].sub_callback_comment

  End Function

  Public Shared Operator =(class1 As homebaseSubscriptionClass, class2 As homebaseSubscriptionClass) As Boolean
    Return EqualityComparer(Of homebaseSubscriptionClass).Default.Equals(class1, class2)
  End Operator

  Public Shared Operator <>(class1 As homebaseSubscriptionClass, class2 As homebaseSubscriptionClass) As Boolean
    Return Not class1 = class2
  End Operator

End Class

<System.Serializable()> Public Class homebaseInstallClass

  Public Property subins_sub_id() As Long
  Public Property subins_login() As String
  Public Property subins_seq_no() As Integer
  Public Property subins_platform_name() As String
  Public Property subins_platform_os() As String
  Public Property subins_install_date() As String
  Public Property subins_access_date() As String
  Public Property subins_active_flag() As Boolean
  Public Property subins_local_db_flag() As Boolean
  Public Property subins_local_db_file() As String
  Public Property subins_webpage_timeout() As Integer
  Public Property subins_activex_flag() As Boolean
  Public Property subins_autocheck_tservice() As Boolean
  Public Property subins_terminal_service() As Boolean
  Public Property subins_email_replyname() As String
  Public Property subins_email_replyaddress() As String
  Public Property subins_email_default_format() As String
  Public Property subins_default_airports() As String
  Public Property subins_aircraft_tab_relationship_to_ac_default() As String
  Public Property subins_contract_amount() As Double
  Public Property subins_use_cookie_flag() As Boolean
  Public Property subins_display_note_tag_on_aclist_flag() As Boolean
  Public Property subins_evoview_id() As Integer
  Public Property subins_cell_number() As String
  Public Property subins_cell_service() As String
  Public Property subins_smstxt_models() As String
  Public Property subins_cell_carrier_id() As Integer
  Public Property subins_smstxt_active_flag() As String
  Public Property subins_mobile_active_date() As String
  Public Property subins_default_amod_id() As Integer
  Public Property subins_default_analysis_months() As Integer
  Public Property subins_evo_mobile_flag() As Boolean
  Public Property subins_sms_events() As String
  Public Property subins_contact_id() As Long
  Public Property subins_business_type_code() As String
  Public Property subins_last_login_date() As String
  Public Property subins_last_logout_date() As String
  Public Property subins_last_session_date() As String
  Public Property subins_background_image_id() As Integer
  Public Property subins_nbr_rec_per_page() As Integer
  Public Property subins_session_guid() As String
  Public Property subins_default_models() As String
  Public Property subins_admin_flag() As Boolean
  Public Property subins_chat_flag() As Boolean

  Public Sub New()

    Try

      subins_sub_id = 0
      subins_login = ""
      subins_seq_no = 0
      subins_platform_name = ""
      subins_platform_os = ""
      subins_install_date = ""
      subins_access_date = ""
      subins_active_flag = False
      subins_local_db_flag = False
      subins_local_db_file = ""
      subins_webpage_timeout = 0
      subins_activex_flag = False
      subins_autocheck_tservice = False
      subins_terminal_service = False
      subins_email_replyname = ""
      subins_email_replyaddress = ""
      subins_email_default_format = ""
      subins_default_airports = ""
      subins_aircraft_tab_relationship_to_ac_default = ""
      subins_contract_amount = 0
      subins_use_cookie_flag = False
      subins_display_note_tag_on_aclist_flag = False
      subins_evoview_id = 0
      subins_cell_number = ""
      subins_cell_service = ""
      subins_smstxt_models = ""
      subins_cell_carrier_id = 0
      subins_smstxt_active_flag = ""
      subins_mobile_active_date = ""
      subins_default_amod_id = 0
      subins_default_analysis_months = 0
      subins_evo_mobile_flag = False
      subins_sms_events = ""
      subins_contact_id = 0
      subins_business_type_code = ""
      subins_last_login_date = ""
      subins_last_logout_date = ""
      subins_last_session_date = ""
      subins_background_image_id = 0
      subins_nbr_rec_per_page = 0
      subins_session_guid = ""
      subins_default_models = ""
      subins_admin_flag = False
      subins_chat_flag = False

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message

    End Try

  End Sub

  Public Sub New(ByVal subinsSubId As Long, ByVal subinsLogin As String)

    Try

      subins_sub_id = subinsSubId
      subins_login = subinsLogin
      subins_seq_no = 0
      subins_platform_name = ""
      subins_platform_os = ""
      subins_install_date = ""
      subins_access_date = ""
      subins_active_flag = False
      subins_local_db_flag = False
      subins_local_db_file = ""
      subins_webpage_timeout = 0
      subins_activex_flag = False
      subins_autocheck_tservice = False
      subins_terminal_service = False
      subins_email_replyname = ""
      subins_email_replyaddress = ""
      subins_email_default_format = ""
      subins_default_airports = ""
      subins_aircraft_tab_relationship_to_ac_default = ""
      subins_contract_amount = 0
      subins_use_cookie_flag = False
      subins_display_note_tag_on_aclist_flag = False
      subins_evoview_id = 0
      subins_cell_number = ""
      subins_cell_service = ""
      subins_smstxt_models = ""
      subins_cell_carrier_id = 0
      subins_smstxt_active_flag = ""
      subins_mobile_active_date = ""
      subins_default_amod_id = 0
      subins_default_analysis_months = 0
      subins_evo_mobile_flag = False
      subins_sms_events = ""
      subins_contact_id = 0
      subins_business_type_code = ""
      subins_last_login_date = ""
      subins_last_logout_date = ""
      subins_last_session_date = ""
      subins_background_image_id = 0
      subins_nbr_rec_per_page = 0
      subins_session_guid = ""
      subins_default_models = ""
      subins_admin_flag = False
      subins_chat_flag = False

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message

    End Try

  End Sub

  Public Function getSubscriptionInstallDataTable(ByVal inSubID As Long, ByVal inLogin As String) As DataTable

    Dim atemptable As New DataTable
    Dim subQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      subQuery.Append("SELECT * FROM " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Subscription_Install WITH(NOLOCK)")
      subQuery.Append(" WHERE subins_sub_id = @subins_sub_id AND subins_login = @subins_login")

      SqlCommand.Parameters.AddWithValue("@subins_sub_id", inSubID.ToString.Trim)
      SqlCommand.Parameters.AddWithValue("@subins_login", inLogin.Trim)

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 90

      SqlCommand.CommandText = subQuery.ToString
      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()

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

  Public Sub fillSubscriptionInstallClass()

    Dim resultsTable As New DataTable

    Try

      If subins_sub_id = 0 And String.IsNullOrEmpty(subins_login) Then
        Exit Sub
      End If

      resultsTable = getSubscriptionInstallDataTable(subins_sub_id, subins_login)

      If Not IsNothing(resultsTable) Then

        If resultsTable.Rows.Count > 0 Then

          For Each r As DataRow In resultsTable.Rows

            If Not (IsDBNull(r("subins_seq_no"))) Then
              If Not String.IsNullOrEmpty(r.Item("subins_seq_no").ToString.Trim) Then
                If IsNumeric(r.Item("subins_seq_no").ToString.Trim) Then
                  subins_seq_no = CInt(r.Item("subins_seq_no").ToString.Trim)
                End If
              End If
            End If

            If Not (IsDBNull(r("subins_platform_name"))) Then
              subins_platform_name = r.Item("subins_platform_name").ToString.Trim
            End If

            If Not (IsDBNull(r("subins_platform_os"))) Then
              subins_platform_os = r.Item("subins_platform_os").ToString.Trim
            End If

            If Not (IsDBNull(r("subins_install_date"))) Then
              subins_install_date = r.Item("subins_install_date").ToString.Trim
            End If

            If Not (IsDBNull(r("subins_access_date"))) Then
              subins_access_date = r.Item("subins_access_date").ToString.Trim
            End If

            If Not (IsDBNull(r("subins_active_flag"))) Then
              subins_active_flag = IIf(r.Item("subins_active_flag").ToString.ToUpper.Contains("Y"), True, False)
            End If

            If Not (IsDBNull(r("subins_local_db_flag"))) Then
              subins_local_db_flag = IIf(r.Item("subins_local_db_flag").ToString.ToUpper.Contains("Y"), True, False)
            End If

            If Not (IsDBNull(r("subins_local_db_file"))) Then
              subins_local_db_file = r.Item("subins_local_db_file").ToString.Trim
            End If

            If Not (IsDBNull(r("subins_webpage_timeout"))) Then
              If Not String.IsNullOrEmpty(r.Item("subins_webpage_timeout").ToString.Trim) Then
                If IsNumeric(r.Item("subins_webpage_timeout").ToString.Trim) Then
                  subins_webpage_timeout = CInt(r.Item("subins_webpage_timeout").ToString.Trim)
                End If
              End If
            End If

            If Not (IsDBNull(r("subins_activex_flag"))) Then
              subins_activex_flag = IIf(r.Item("subins_activex_flag").ToString.ToUpper.Contains("Y"), True, False)
            End If

            If Not (IsDBNull(r("subins_autocheck_tservice"))) Then
              subins_autocheck_tservice = IIf(r.Item("subins_autocheck_tservice").ToString.ToUpper.Contains("Y"), True, False)
            End If

            If Not (IsDBNull(r("subins_terminal_service"))) Then
              subins_terminal_service = IIf(r.Item("subins_terminal_service").ToString.ToUpper.Contains("Y"), True, False)
            End If

            If Not (IsDBNull(r("subins_email_replyname"))) Then
              subins_email_replyname = r.Item("subins_email_replyname").ToString.Trim
            End If

            If Not (IsDBNull(r("subins_email_replyaddress"))) Then
              subins_email_replyaddress = r.Item("subins_email_replyaddress").ToString.Trim
            End If

            If Not (IsDBNull(r("subins_email_default_format"))) Then
              subins_email_default_format = r.Item("subins_email_default_format").ToString.Trim
            End If

            If Not (IsDBNull(r("subins_default_airports"))) Then
              subins_default_airports = r.Item("subins_default_airports").ToString.Trim
            End If

            If Not (IsDBNull(r("subins_aircraft_tab_relationship_to_ac_default"))) Then
              subins_aircraft_tab_relationship_to_ac_default = r.Item("subins_aircraft_tab_relationship_to_ac_default").ToString.Trim
            End If

            If Not (IsDBNull(r("subins_contract_amount"))) Then
              If Not String.IsNullOrEmpty(r.Item("subins_contract_amount").ToString.Trim) Then
                If IsNumeric(r.Item("subins_contract_amount").ToString.Trim) Then
                  subins_contract_amount = CDbl(r.Item("subins_contract_amount").ToString.Trim)
                End If
              End If
            End If

            If Not (IsDBNull(r("subins_use_cookie_flag"))) Then
              subins_use_cookie_flag = IIf(r.Item("subins_use_cookie_flag").ToString.ToUpper.Contains("Y"), True, False)
            End If

            If Not (IsDBNull(r("subins_display_note_tag_on_aclist_flag"))) Then
              subins_display_note_tag_on_aclist_flag = IIf(r.Item("subins_display_note_tag_on_aclist_flag").ToString.ToUpper.Contains("Y"), True, False)
            End If

            If Not (IsDBNull(r("subins_evoview_id"))) Then
              If Not String.IsNullOrEmpty(r.Item("subins_evoview_id").ToString.Trim) Then
                If IsNumeric(r.Item("subins_evoview_id").ToString.Trim) Then
                  subins_evoview_id = CInt(r.Item("subins_evoview_id").ToString.Trim)
                End If
              End If
            End If

            If Not (IsDBNull(r("subins_cell_number"))) Then
              subins_cell_number = r.Item("subins_cell_number").ToString.Trim
            End If

            If Not (IsDBNull(r("subins_cell_service"))) Then
              subins_cell_service = r.Item("subins_cell_service").ToString.Trim
            End If

            If Not (IsDBNull(r("subins_smstxt_models"))) Then
              subins_smstxt_models = r.Item("subins_smstxt_models").ToString.Trim
            End If

            If Not (IsDBNull(r("subins_cell_carrier_id"))) Then
              If Not String.IsNullOrEmpty(r.Item("subins_cell_carrier_id").ToString.Trim) Then
                If IsNumeric(r.Item("subins_cell_carrier_id").ToString.Trim) Then
                  subins_cell_carrier_id = CInt(r.Item("subins_cell_carrier_id").ToString.Trim)
                End If
              End If
            End If

            If Not (IsDBNull(r("subins_smstxt_active_flag"))) Then
              subins_smstxt_active_flag = r.Item("subins_smstxt_active_flag").ToString.Trim
            End If

            If Not (IsDBNull(r("subins_mobile_active_date"))) Then
              subins_mobile_active_date = r.Item("subins_mobile_active_date").ToString.Trim
            End If

            If Not (IsDBNull(r("subins_default_amod_id"))) Then
              If Not String.IsNullOrEmpty(r.Item("subins_default_amod_id").ToString.Trim) Then
                If IsNumeric(r.Item("subins_default_amod_id").ToString.Trim) Then
                  subins_default_amod_id = CInt(r.Item("subins_default_amod_id").ToString.Trim)
                End If
              End If
            End If

            If Not (IsDBNull(r("subins_default_analysis_months"))) Then
              If Not String.IsNullOrEmpty(r.Item("subins_default_analysis_months").ToString.Trim) Then
                If IsNumeric(r.Item("subins_default_analysis_months").ToString.Trim) Then
                  subins_default_analysis_months = CInt(r.Item("subins_default_analysis_months").ToString.Trim)
                End If
              End If
            End If

            If Not (IsDBNull(r("subins_evo_mobile_flag"))) Then
              subins_evo_mobile_flag = IIf(r.Item("subins_evo_mobile_flag").ToString.ToUpper.Contains("Y"), True, False)
            End If

            If Not (IsDBNull(r("subins_sms_events"))) Then
              subins_sms_events = r.Item("subins_sms_events").ToString.Trim
            End If

            If Not (IsDBNull(r("subins_contact_id"))) Then
              If Not String.IsNullOrEmpty(r.Item("subins_contact_id").ToString.Trim) Then
                If IsNumeric(r.Item("subins_contact_id").ToString.Trim) Then
                  subins_contact_id = CLng(r.Item("subins_contact_id").ToString.Trim)
                End If
              End If
            End If

            If Not (IsDBNull(r("subins_business_type_code"))) Then
              subins_business_type_code = r.Item("subins_business_type_code").ToString.Trim
            End If

            If Not (IsDBNull(r("subins_last_login_date"))) Then
              subins_last_login_date = r.Item("subins_last_login_date").ToString.Trim
            End If

            If Not (IsDBNull(r("subins_last_logout_date"))) Then
              subins_last_logout_date = r.Item("subins_last_logout_date").ToString.Trim
            End If

            If Not (IsDBNull(r("subins_last_session_date"))) Then
              subins_last_session_date = r.Item("subins_last_session_date").ToString.Trim
            End If

            If Not (IsDBNull(r("subins_background_image_id"))) Then
              If Not String.IsNullOrEmpty(r.Item("subins_background_image_id").ToString.Trim) Then
                If IsNumeric(r.Item("subins_background_image_id").ToString.Trim) Then
                  subins_background_image_id = CInt(r.Item("subins_background_image_id").ToString.Trim)
                End If
              End If
            End If

            If Not (IsDBNull(r("subins_nbr_rec_per_page"))) Then
              If Not String.IsNullOrEmpty(r.Item("subins_nbr_rec_per_page").ToString.Trim) Then
                If IsNumeric(r.Item("subins_nbr_rec_per_page").ToString.Trim) Then
                  subins_nbr_rec_per_page = CInt(r.Item("subins_nbr_rec_per_page").ToString.Trim)
                End If
              End If
            End If

            If Not (IsDBNull(r("subins_session_guid"))) Then
              subins_session_guid = r.Item("subins_session_guid").ToString.Trim
            End If

            If Not (IsDBNull(r("subins_default_models"))) Then
              subins_default_models = r.Item("subins_default_models").ToString.Trim
            End If

            If Not (IsDBNull(r("subins_admin_flag"))) Then
              subins_admin_flag = IIf(r.Item("subins_admin_flag").ToString.ToUpper.Contains("Y"), True, False)
            End If

            If Not (IsDBNull(r("subins_chat_flag"))) Then
              subins_chat_flag = IIf(r.Item("subins_chat_flag").ToString.ToUpper.Contains("Y"), True, False)
            End If

          Next

        End If

      End If

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message

    End Try

  End Sub

  Public Sub updateSubscriptionInstallClass()
    Dim subInstallQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand

    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sSeperator = ""
    Try

      If subins_sub_id = 0 And String.IsNullOrEmpty(subins_login) And subins_seq_no = 0 Then
        Exit Sub
      End If

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 90

      subInstallQuery.Append("UPDATE Subscription_Install SET")

      If Not String.IsNullOrEmpty(subins_platform_name.Trim) Then

        subInstallQuery.Append(sSeperator + " subins_platform_name = @subins_platform_name")
        SqlCommand.Parameters.AddWithValue("@subins_platform_name", subins_platform_name.Trim)
        sSeperator = ","

      End If

      If Not String.IsNullOrEmpty(subins_platform_os.Trim) Then

        subInstallQuery.Append(sSeperator + " subins_platform_os = @subins_platform_os")
        SqlCommand.Parameters.AddWithValue("@subins_platform_os", subins_platform_os.Trim)
        sSeperator = ","

      End If

      If Not String.IsNullOrEmpty(subins_install_date.Trim) Then

        subInstallQuery.Append(sSeperator + " subins_install_date = @subins_install_date")
        SqlCommand.Parameters.AddWithValue("@subins_install_date", subins_install_date.Trim)
        sSeperator = ","

      Else

        subInstallQuery.Append(sSeperator + " subins_install_date = @subins_install_date")
        SqlCommand.Parameters.AddWithValue("@subins_install_date", DBNull.Value)
        sSeperator = ","

      End If

      If Not String.IsNullOrEmpty(subins_access_date.Trim) Then

        subInstallQuery.Append(sSeperator + " subins_access_date = @subins_access_date")
        SqlCommand.Parameters.AddWithValue("@subins_access_date", subins_access_date.Trim)
        sSeperator = ","

      Else

        subInstallQuery.Append(sSeperator + " subins_access_date = @subins_access_date")
        SqlCommand.Parameters.AddWithValue("@subins_access_date", DBNull.Value)
        sSeperator = ","

      End If

      subInstallQuery.Append(sSeperator + " subins_active_flag = @subins_active_flag")
      SqlCommand.Parameters.AddWithValue("@subins_active_flag", IIf(subins_active_flag, "Y", "N"))
      sSeperator = ","

      subInstallQuery.Append(sSeperator + " subins_local_db_flag = @subins_local_db_flag")
      SqlCommand.Parameters.AddWithValue("@subins_local_db_flag", IIf(subins_local_db_flag, "Y", "N"))
      sSeperator = ","

      If Not String.IsNullOrEmpty(subins_local_db_file.Trim) Then

        subInstallQuery.Append(sSeperator + " subins_local_db_file = @subins_local_db_file")
        SqlCommand.Parameters.AddWithValue("@subins_local_db_file", subins_local_db_file.Trim)
        sSeperator = ","

      End If

      If subins_webpage_timeout >= 0 Then

        subInstallQuery.Append(sSeperator + " subins_webpage_timeout = @subins_webpage_timeout")
        SqlCommand.Parameters.AddWithValue("@subins_webpage_timeout", subins_webpage_timeout.ToString)
        sSeperator = ","

      End If

      subInstallQuery.Append(sSeperator + " subins_activex_flag = @subins_activex_flag")
      SqlCommand.Parameters.AddWithValue("@subins_activex_flag", IIf(subins_activex_flag, "Y", "N"))
      sSeperator = ","

      subInstallQuery.Append(sSeperator + " subins_autocheck_tservice = @subins_autocheck_tservice")
      SqlCommand.Parameters.AddWithValue("@subins_autocheck_tservice", IIf(subins_autocheck_tservice, "Y", "N"))
      sSeperator = ","

      subInstallQuery.Append(sSeperator + " subins_terminal_service = @subins_terminal_service")
      SqlCommand.Parameters.AddWithValue("@subins_terminal_service", IIf(subins_terminal_service, "Y", "N"))
      sSeperator = ","

      If Not String.IsNullOrEmpty(subins_email_replyname.Trim) Then

        subInstallQuery.Append(sSeperator + " subins_email_replyname = @subins_email_replyname")
        SqlCommand.Parameters.AddWithValue("@subins_email_replyname", subins_email_replyname.Trim)
        sSeperator = ","

      End If

      If Not String.IsNullOrEmpty(subins_email_replyaddress.Trim) Then

        subInstallQuery.Append(sSeperator + " subins_email_replyaddress = @subins_email_replyaddress")
        SqlCommand.Parameters.AddWithValue("@subins_email_replyaddress", subins_email_replyaddress.Trim)
        sSeperator = ","

      End If

      If Not String.IsNullOrEmpty(subins_email_default_format.Trim) Then

        subInstallQuery.Append(sSeperator + " subins_email_default_format = @subins_email_default_format")
        SqlCommand.Parameters.AddWithValue("@subins_email_default_format", subins_email_default_format.Trim)
        sSeperator = ","

      End If

      If Not String.IsNullOrEmpty(subins_default_airports.Trim) Then

        subInstallQuery.Append(sSeperator + " subins_default_airports = @subins_default_airports")
        SqlCommand.Parameters.AddWithValue("@subins_default_airports", subins_default_airports.Trim)
        sSeperator = ","

      End If

      If Not String.IsNullOrEmpty(subins_aircraft_tab_relationship_to_ac_default.Trim) Then

        subInstallQuery.Append(sSeperator + " subins_aircraft_tab_relationship_to_ac_default = @subins_aircraft_tab_relationship_to_ac_default")
        SqlCommand.Parameters.AddWithValue("@subins_aircraft_tab_relationship_to_ac_default", subins_aircraft_tab_relationship_to_ac_default.Trim)
        sSeperator = ","

      End If

      If subins_contract_amount >= 0 Then

        subInstallQuery.Append(sSeperator + " subins_contract_amount = @subins_contract_amount")
        SqlCommand.Parameters.AddWithValue("@subins_contract_amount", subins_contract_amount.ToString)
        sSeperator = ","

      End If

      subInstallQuery.Append(sSeperator + " subins_use_cookie_flag = @subins_use_cookie_flag")
      SqlCommand.Parameters.AddWithValue("@subins_use_cookie_flag", IIf(subins_use_cookie_flag, "Y", "N"))
      sSeperator = ","

      subInstallQuery.Append(sSeperator + " subins_display_note_tag_on_aclist_flag = @subins_display_note_tag_on_aclist_flag")
      SqlCommand.Parameters.AddWithValue("@subins_display_note_tag_on_aclist_flag", IIf(subins_display_note_tag_on_aclist_flag, "Y", "N"))
      sSeperator = ","

      If subins_evoview_id >= 0 Then

        subInstallQuery.Append(sSeperator + " subins_evoview_id = @subins_evoview_id")
        SqlCommand.Parameters.AddWithValue("@subins_evoview_id", subins_evoview_id.ToString)
        sSeperator = ","

      End If

      If Not String.IsNullOrEmpty(subins_cell_number.Trim) Then

        subInstallQuery.Append(sSeperator + " subins_cell_number = @subins_cell_number")
        SqlCommand.Parameters.AddWithValue("@subins_cell_number", subins_cell_number.Trim)
        sSeperator = ","

      End If

      If Not String.IsNullOrEmpty(subins_cell_service.Trim) Then

        subInstallQuery.Append(sSeperator + " subins_cell_service = @subins_cell_service")
        SqlCommand.Parameters.AddWithValue("@subins_cell_service", subins_cell_service.Trim)
        sSeperator = ","

      End If

      If Not String.IsNullOrEmpty(subins_smstxt_models.Trim) Then

        subInstallQuery.Append(sSeperator + " subins_smstxt_models = @subins_smstxt_models")
        SqlCommand.Parameters.AddWithValue("@subins_smstxt_models", subins_smstxt_models.Trim)
        sSeperator = ","

      End If

      If subins_cell_carrier_id >= 0 Then

        subInstallQuery.Append(sSeperator + " subins_cell_carrier_id = @subins_cell_carrier_id")
        SqlCommand.Parameters.AddWithValue("@subins_cell_carrier_id", subins_cell_carrier_id.ToString)
        sSeperator = ","

      End If

      subInstallQuery.Append(sSeperator + " subins_smstxt_active_flag = @subins_smstxt_active_flag")
      SqlCommand.Parameters.AddWithValue("@subins_smstxt_active_flag", IIf(subins_smstxt_active_flag, "Y", "N"))
      sSeperator = ","

      If Not String.IsNullOrEmpty(subins_mobile_active_date.Trim) Then

        subInstallQuery.Append(sSeperator + " subins_mobile_active_date = @subins_mobile_active_date")
        SqlCommand.Parameters.AddWithValue("@subins_mobile_active_date", subins_mobile_active_date.Trim)
        sSeperator = ","

      Else

        subInstallQuery.Append(sSeperator + " subins_mobile_active_date = @subins_mobile_active_date")
        SqlCommand.Parameters.AddWithValue("@subins_mobile_active_date", DBNull.Value)
        sSeperator = ","

      End If

      If subins_default_amod_id >= 0 Then

        subInstallQuery.Append(sSeperator + " subins_default_amod_id = @subins_default_amod_id")
        SqlCommand.Parameters.AddWithValue("@subins_default_amod_id", subins_default_amod_id.ToString)
        sSeperator = ","

      End If

      If subins_default_analysis_months >= 0 Then

        subInstallQuery.Append(sSeperator + " subins_default_analysis_months = @subins_default_analysis_months")
        SqlCommand.Parameters.AddWithValue("@subins_default_analysis_months", subins_default_analysis_months.ToString)
        sSeperator = ","

      End If

      subInstallQuery.Append(sSeperator + " subins_evo_mobile_flag = @subins_evo_mobile_flag")
      SqlCommand.Parameters.AddWithValue("@subins_evo_mobile_flag", IIf(subins_evo_mobile_flag, "Y", "N"))
      sSeperator = ","

      If Not String.IsNullOrEmpty(subins_sms_events.Trim) Then

        subInstallQuery.Append(sSeperator + " subins_sms_events = @subins_sms_events")
        SqlCommand.Parameters.AddWithValue("@subins_sms_events", subins_sms_events.Trim)
        sSeperator = ","

      End If

      If subins_contact_id >= 0 Then

        subInstallQuery.Append(sSeperator + " subins_contact_id = @subins_contact_id")
        SqlCommand.Parameters.AddWithValue("@subins_contact_id", subins_contact_id.ToString)
        sSeperator = ","

      End If

      If Not String.IsNullOrEmpty(subins_business_type_code.Trim) Then

        subInstallQuery.Append(sSeperator + " subins_business_type_code = @subins_business_type_code")
        SqlCommand.Parameters.AddWithValue("@subins_business_type_code", subins_business_type_code.Trim)
        sSeperator = ","

      End If

      If Not String.IsNullOrEmpty(subins_last_login_date.Trim) Then

        subInstallQuery.Append(sSeperator + " subins_last_login_date = @subins_last_login_date")
        SqlCommand.Parameters.AddWithValue("@subins_last_login_date", subins_last_login_date.Trim)
        sSeperator = ","

      Else

        subInstallQuery.Append(sSeperator + " subins_last_login_date = @subins_last_login_date")
        SqlCommand.Parameters.AddWithValue("@subins_last_login_date", DBNull.Value)
        sSeperator = ","

      End If

      If Not String.IsNullOrEmpty(subins_last_logout_date.Trim) Then

        subInstallQuery.Append(sSeperator + " subins_last_logout_date = @subins_last_logout_date")
        SqlCommand.Parameters.AddWithValue("@subins_last_logout_date", subins_last_logout_date.Trim)
        sSeperator = ","

      Else

        subInstallQuery.Append(sSeperator + " subins_last_logout_date = @subins_last_logout_date")
        SqlCommand.Parameters.AddWithValue("@subins_last_logout_date", DBNull.Value)
        sSeperator = ","

      End If

      If Not String.IsNullOrEmpty(subins_last_session_date.Trim) Then

        subInstallQuery.Append(sSeperator + " subins_last_session_date = @subins_last_session_date")
        SqlCommand.Parameters.AddWithValue("@subins_last_session_date", subins_last_session_date.Trim)
        sSeperator = ","

      Else

        subInstallQuery.Append(sSeperator + " subins_last_session_date = @subins_last_session_date")
        SqlCommand.Parameters.AddWithValue("@subins_last_session_date", DBNull.Value)
        sSeperator = ","

      End If

      If subins_background_image_id >= 0 Then

        subInstallQuery.Append(sSeperator + " subins_background_image_id = @subins_background_image_id")
        SqlCommand.Parameters.AddWithValue("@subins_background_image_id", subins_background_image_id.ToString)
        sSeperator = ","

      End If

      If subins_nbr_rec_per_page >= 0 Then

        subInstallQuery.Append(sSeperator + " subins_nbr_rec_per_page = @subins_nbr_rec_per_page")
        SqlCommand.Parameters.AddWithValue("@subins_nbr_rec_per_page", subins_background_image_id.ToString)
        sSeperator = ","

      End If

      If Not String.IsNullOrEmpty(subins_session_guid.Trim) Then

        subInstallQuery.Append(sSeperator + " subins_session_guid = @subins_session_guid")
        SqlCommand.Parameters.AddWithValue("@subins_session_guid", subins_session_guid.Trim)
        sSeperator = ","

      End If

      If Not String.IsNullOrEmpty(subins_default_models.Trim) Then

        subInstallQuery.Append(sSeperator + " subins_default_models = @subins_default_models")
        SqlCommand.Parameters.AddWithValue("@subins_default_models", subins_default_models.Trim)
        sSeperator = ","

      End If

      subInstallQuery.Append(sSeperator + " subins_admin_flag = @subins_admin_flag")
      SqlCommand.Parameters.AddWithValue("@subins_admin_flag", IIf(subins_admin_flag, "Y", "N"))
      sSeperator = ","

      subInstallQuery.Append(sSeperator + " subins_chat_flag = @subins_chat_flag")
      SqlCommand.Parameters.AddWithValue("@subins_chat_flag", IIf(subins_chat_flag, "Y", "N"))
      sSeperator = ","

      ' set web action date to null for any changes
      subInstallQuery.Append(sSeperator + " subins_web_action_date = @subins_web_action_date")
      SqlCommand.Parameters.AddWithValue("@subins_web_action_date", DBNull.Value)

      subInstallQuery.Append(" WHERE subins_sub_id = @subins_sub_id AND subins_login = @subins_login AND subins_seq_no = @subins_seq_no")

      SqlCommand.Parameters.AddWithValue("@subins_sub_id", subins_sub_id.ToString.Trim)
      SqlCommand.Parameters.AddWithValue("@subins_login", subins_login.Trim)
      SqlCommand.Parameters.AddWithValue("@subins_seq_no", subins_seq_no.ToString.Trim)

      SqlCommand.CommandText = subInstallQuery.ToString

      SqlCommand.ExecuteNonQuery()

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message

    Finally

      SqlConn.Dispose()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

  End Sub

  Public Sub insertSubscriptionInstallClass()

    Dim subInstallQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand

    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      If subins_sub_id = 0 And String.IsNullOrEmpty(subins_login) And subins_seq_no = 0 Then
        Exit Sub
      End If

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 90

      subInstallQuery.Append("INSERT INTO Subscription (subins_platform_name, subins_platform_os, subins_install_date, subins_access_date, subins_active_flag, subins_local_db_flag, subins_local_db_file,")
      subInstallQuery.Append(" subins_webpage_timeout, subins_activex_flag, subins_autocheck_tservice, subins_terminal_service, subins_email_replyname, subins_email_replyaddress, subins_email_default_format,")
      subInstallQuery.Append(" subins_default_airports, subins_aircraft_tab_relationship_to_ac_default, subins_contract_amount, subins_use_cookie_flag, subins_display_note_tag_on_aclist_flag,")
      subInstallQuery.Append(" subins_use_cookie_flag, subins_display_note_tag_on_aclist_flag, subins_evoview_id, subins_cell_number, subins_cell_service, subins_smstxt_models, subins_cell_carrier_id,")
      subInstallQuery.Append(" subins_smstxt_active_flag, subins_mobile_active_date, subins_default_amod_id, subins_default_analysis_months, subins_evo_mobile_flag, subins_sms_events,")
      subInstallQuery.Append(" subins_contact_id, subins_business_type_code, subins_last_login_date, subins_last_logout_date, subins_last_session_date, subins_background_image_id, subins_nbr_rec_per_page,")
      subInstallQuery.Append(" subins_session_guid, subins_default_models, subins_admin_flag, subins_chat_flag, subins_web_action_date")
      subInstallQuery.Append(") VALUES (@subins_platform_name, @subins_platform_os, @subins_install_date, @subins_access_date, @subins_active_flag, @subins_local_db_flag, @subins_local_db_file,")
      subInstallQuery.Append(" @subins_webpage_timeout, @subins_activex_flag, @subins_autocheck_tservice, @subins_terminal_service, @subins_email_replyname, @subins_email_replyaddress, @subins_email_default_format,")
      subInstallQuery.Append(" @subins_default_airports, @subins_aircraft_tab_relationship_to_ac_default, @subins_contract_amount, @subins_use_cookie_flag, @subins_display_note_tag_on_aclist_flag,")
      subInstallQuery.Append(" @subins_use_cookie_flag, @subins_display_note_tag_on_aclist_flag, @subins_evoview_id, @subins_cell_number, @subins_cell_service, @subins_smstxt_models, @subins_cell_carrier_id,")
      subInstallQuery.Append(" @subins_smstxt_active_flag, @subins_mobile_active_date, @subins_default_amod_id, @subins_default_analysis_months, @subins_evo_mobile_flag, @subins_sms_events,")
      subInstallQuery.Append(" @subins_contact_id, @subins_business_type_code, @subins_last_login_date, @subins_last_logout_date, @subins_last_session_date, @subins_background_image_id, @subins_nbr_rec_per_page,")
      subInstallQuery.Append(" @subins_session_guid, @subins_default_models, @subins_admin_flag, @subins_chat_flag, @subins_web_action_date")
      subInstallQuery.Append(") WHERE subins_sub_id = @subins_sub_id AND subins_login = @subins_login AND subins_seq_no = @subins_seq_no")

      If Not String.IsNullOrEmpty(subins_platform_name.Trim) Then
        SqlCommand.Parameters.AddWithValue("@subins_platform_name", subins_platform_name.Trim)
      End If

      If Not String.IsNullOrEmpty(subins_platform_os.Trim) Then
        SqlCommand.Parameters.AddWithValue("@subins_platform_os", subins_platform_os.Trim)
      End If

      If Not String.IsNullOrEmpty(subins_install_date.Trim) Then
        SqlCommand.Parameters.AddWithValue("@subins_install_date", subins_install_date.Trim)
      Else
        SqlCommand.Parameters.AddWithValue("@subins_install_date", DBNull.Value)
      End If

      If Not String.IsNullOrEmpty(subins_access_date.Trim) Then
        SqlCommand.Parameters.AddWithValue("@subins_access_date", subins_access_date.Trim)
      Else
        SqlCommand.Parameters.AddWithValue("@subins_access_date", DBNull.Value)
      End If

      SqlCommand.Parameters.AddWithValue("@subins_active_flag", IIf(subins_active_flag, "Y", "N"))

      SqlCommand.Parameters.AddWithValue("@subins_local_db_flag", IIf(subins_local_db_flag, "Y", "N"))

      If Not String.IsNullOrEmpty(subins_local_db_file.Trim) Then
        SqlCommand.Parameters.AddWithValue("@subins_local_db_file", subins_local_db_file.Trim)
      End If

      If subins_webpage_timeout >= 0 Then
        SqlCommand.Parameters.AddWithValue("@subins_webpage_timeout", subins_webpage_timeout.ToString)
      End If

      SqlCommand.Parameters.AddWithValue("@subins_activex_flag", IIf(subins_activex_flag, "Y", "N"))

      SqlCommand.Parameters.AddWithValue("@subins_autocheck_tservice", IIf(subins_autocheck_tservice, "Y", "N"))

      SqlCommand.Parameters.AddWithValue("@subins_terminal_service", IIf(subins_terminal_service, "Y", "N"))

      If Not String.IsNullOrEmpty(subins_email_replyname.Trim) Then
        SqlCommand.Parameters.AddWithValue("@subins_email_replyname", subins_email_replyname.Trim)
      End If

      If Not String.IsNullOrEmpty(subins_email_replyaddress.Trim) Then
        SqlCommand.Parameters.AddWithValue("@subins_email_replyaddress", subins_email_replyaddress.Trim)
      End If

      If Not String.IsNullOrEmpty(subins_email_default_format.Trim) Then
        SqlCommand.Parameters.AddWithValue("@subins_email_default_format", subins_email_default_format.Trim)
      End If

      If Not String.IsNullOrEmpty(subins_default_airports.Trim) Then
        SqlCommand.Parameters.AddWithValue("@subins_default_airports", subins_default_airports.Trim)
      End If

      If Not String.IsNullOrEmpty(subins_aircraft_tab_relationship_to_ac_default.Trim) Then
        SqlCommand.Parameters.AddWithValue("@subins_aircraft_tab_relationship_to_ac_default", subins_aircraft_tab_relationship_to_ac_default.Trim)
      End If

      If subins_contract_amount >= 0 Then
        SqlCommand.Parameters.AddWithValue("@subins_contract_amount", subins_contract_amount.ToString)
      End If

      SqlCommand.Parameters.AddWithValue("@subins_use_cookie_flag", IIf(subins_use_cookie_flag, "Y", "N"))

      SqlCommand.Parameters.AddWithValue("@subins_display_note_tag_on_aclist_flag", IIf(subins_display_note_tag_on_aclist_flag, "Y", "N"))

      If subins_evoview_id >= 0 Then
        SqlCommand.Parameters.AddWithValue("@subins_evoview_id", subins_evoview_id.ToString)
      End If

      If Not String.IsNullOrEmpty(subins_cell_number.Trim) Then
        SqlCommand.Parameters.AddWithValue("@subins_cell_number", subins_cell_number.Trim)
      End If

      If Not String.IsNullOrEmpty(subins_cell_service.Trim) Then
        SqlCommand.Parameters.AddWithValue("@subins_cell_service", subins_cell_service.Trim)
      End If

      If Not String.IsNullOrEmpty(subins_smstxt_models.Trim) Then
        SqlCommand.Parameters.AddWithValue("@subins_smstxt_models", subins_smstxt_models.Trim)
      End If

      If subins_cell_carrier_id >= 0 Then
        SqlCommand.Parameters.AddWithValue("@subins_cell_carrier_id", subins_cell_carrier_id.ToString)
      End If

      SqlCommand.Parameters.AddWithValue("@subins_smstxt_active_flag", IIf(subins_smstxt_active_flag, "Y", "N"))

      If Not String.IsNullOrEmpty(subins_mobile_active_date.Trim) Then
        SqlCommand.Parameters.AddWithValue("@subins_mobile_active_date", subins_mobile_active_date.Trim)
      Else
        SqlCommand.Parameters.AddWithValue("@subins_mobile_active_date", DBNull.Value)
      End If

      If subins_default_amod_id >= 0 Then
        SqlCommand.Parameters.AddWithValue("@subins_default_amod_id", subins_default_amod_id.ToString)
      End If

      If subins_default_analysis_months >= 0 Then
        SqlCommand.Parameters.AddWithValue("@subins_default_analysis_months", subins_default_analysis_months.ToString)
      End If

      SqlCommand.Parameters.AddWithValue("@subins_evo_mobile_flag", IIf(subins_evo_mobile_flag, "Y", "N"))

      If Not String.IsNullOrEmpty(subins_sms_events.Trim) Then
        SqlCommand.Parameters.AddWithValue("@subins_sms_events", subins_sms_events.Trim)
      End If

      If subins_contact_id >= 0 Then
        SqlCommand.Parameters.AddWithValue("@subins_contact_id", subins_contact_id.ToString)
      End If

      If Not String.IsNullOrEmpty(subins_business_type_code.Trim) Then
        SqlCommand.Parameters.AddWithValue("@subins_business_type_code", subins_business_type_code.Trim)
      End If

      If Not String.IsNullOrEmpty(subins_last_login_date.Trim) Then
        SqlCommand.Parameters.AddWithValue("@subins_last_login_date", subins_last_login_date.Trim)
      Else
        SqlCommand.Parameters.AddWithValue("@subins_last_login_date", DBNull.Value)
      End If

      If Not String.IsNullOrEmpty(subins_last_logout_date.Trim) Then
        SqlCommand.Parameters.AddWithValue("@subins_last_logout_date", subins_last_logout_date.Trim)
      Else
        SqlCommand.Parameters.AddWithValue("@subins_last_logout_date", DBNull.Value)
      End If

      If Not String.IsNullOrEmpty(subins_last_session_date.Trim) Then
        SqlCommand.Parameters.AddWithValue("@subins_last_session_date", subins_last_session_date.Trim)
      Else
        SqlCommand.Parameters.AddWithValue("@subins_last_session_date", DBNull.Value)
      End If

      If subins_background_image_id >= 0 Then
        SqlCommand.Parameters.AddWithValue("@subins_background_image_id", subins_background_image_id.ToString)
      End If

      If subins_nbr_rec_per_page >= 0 Then
        SqlCommand.Parameters.AddWithValue("@subins_nbr_rec_per_page", subins_background_image_id.ToString)
      End If

      If Not String.IsNullOrEmpty(subins_session_guid.Trim) Then
        SqlCommand.Parameters.AddWithValue("@subins_session_guid", subins_session_guid.Trim)
      End If

      If Not String.IsNullOrEmpty(subins_default_models.Trim) Then
        SqlCommand.Parameters.AddWithValue("@subins_default_models", subins_default_models.Trim)
      End If

      SqlCommand.Parameters.AddWithValue("@subins_admin_flag", IIf(subins_admin_flag, "Y", "N"))

      SqlCommand.Parameters.AddWithValue("@subins_chat_flag", IIf(subins_chat_flag, "Y", "N"))

      ' set web action date to null for any changes
      SqlCommand.Parameters.AddWithValue("@subins_web_action_date", DBNull.Value)

      SqlCommand.Parameters.AddWithValue("@subins_sub_id", subins_sub_id.ToString.Trim)
      SqlCommand.Parameters.AddWithValue("@subins_login", subins_login.Trim)
      SqlCommand.Parameters.AddWithValue("@subins_seq_no", subins_seq_no.ToString.Trim)

      SqlCommand.CommandText = subInstallQuery.ToString

      SqlCommand.ExecuteNonQuery()

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
    Dim [class] = TryCast(obj, homebaseInstallClass)
    Return [class] IsNot Nothing AndAlso
           subins_sub_id = [class].subins_sub_id AndAlso
           subins_login = [class].subins_login AndAlso
           subins_seq_no = [class].subins_seq_no AndAlso
           subins_platform_name = [class].subins_platform_name AndAlso
           subins_platform_os = [class].subins_platform_os AndAlso
           subins_install_date = [class].subins_install_date AndAlso
           subins_access_date = [class].subins_access_date AndAlso
           subins_active_flag = [class].subins_active_flag AndAlso
           subins_local_db_flag = [class].subins_local_db_flag AndAlso
           subins_local_db_file = [class].subins_local_db_file AndAlso
           subins_webpage_timeout = [class].subins_webpage_timeout AndAlso
           subins_activex_flag = [class].subins_activex_flag AndAlso
           subins_autocheck_tservice = [class].subins_autocheck_tservice AndAlso
           subins_terminal_service = [class].subins_terminal_service AndAlso
           subins_email_replyname = [class].subins_email_replyname AndAlso
           subins_email_replyaddress = [class].subins_email_replyaddress AndAlso
           subins_email_default_format = [class].subins_email_default_format AndAlso
           subins_default_airports = [class].subins_default_airports AndAlso
           subins_aircraft_tab_relationship_to_ac_default = [class].subins_aircraft_tab_relationship_to_ac_default AndAlso
           subins_contract_amount = [class].subins_contract_amount AndAlso
           subins_use_cookie_flag = [class].subins_use_cookie_flag AndAlso
           subins_display_note_tag_on_aclist_flag = [class].subins_display_note_tag_on_aclist_flag AndAlso
           subins_evoview_id = [class].subins_evoview_id AndAlso
           subins_cell_number = [class].subins_cell_number AndAlso
           subins_cell_service = [class].subins_cell_service AndAlso
           subins_smstxt_models = [class].subins_smstxt_models AndAlso
           subins_cell_carrier_id = [class].subins_cell_carrier_id AndAlso
           subins_smstxt_active_flag = [class].subins_smstxt_active_flag AndAlso
           subins_mobile_active_date = [class].subins_mobile_active_date AndAlso
           subins_default_amod_id = [class].subins_default_amod_id AndAlso
           subins_default_analysis_months = [class].subins_default_analysis_months AndAlso
           subins_evo_mobile_flag = [class].subins_evo_mobile_flag AndAlso
           subins_sms_events = [class].subins_sms_events AndAlso
           subins_contact_id = [class].subins_contact_id AndAlso
           subins_business_type_code = [class].subins_business_type_code AndAlso
           subins_last_login_date = [class].subins_last_login_date AndAlso
           subins_last_logout_date = [class].subins_last_logout_date AndAlso
           subins_last_session_date = [class].subins_last_session_date AndAlso
           subins_background_image_id = [class].subins_background_image_id AndAlso
           subins_nbr_rec_per_page = [class].subins_nbr_rec_per_page AndAlso
           subins_session_guid = [class].subins_session_guid AndAlso
           subins_default_models = [class].subins_default_models AndAlso
           subins_admin_flag = [class].subins_admin_flag AndAlso
           subins_chat_flag = [class].subins_chat_flag

  End Function

  Public Shared Operator =(class1 As homebaseInstallClass, class2 As homebaseInstallClass) As Boolean
    Return EqualityComparer(Of homebaseInstallClass).Default.Equals(class1, class2)
  End Operator

  Public Shared Operator <>(class1 As homebaseInstallClass, class2 As homebaseInstallClass) As Boolean
    Return Not class1 = class2
  End Operator

End Class

<System.Serializable()> Public Class homebaseLoginClass

  Public Property sublogin_sub_id() As Long
  Public Property sublogin_login() As String
  Public Property sublogin_password() As String
  Public Property sublogin_contact_id() As Long
  Public Property sublogin_active_flag() As Boolean
  Public Property sublogin_demo_flag() As Boolean
  Public Property sublogin_nbr_of_installs() As Integer
  Public Property sublogin_allow_export_flag() As Boolean
  Public Property sublogin_allow_local_notes_flag() As Boolean
  Public Property sublogin_allow_projects_flag() As Boolean
  Public Property sublogin_allow_email_request_flag() As Boolean
  Public Property sublogin_allow_event_request_flag() As Boolean
  Public Property sublogin_bypass_active_x_registry_flag() As Boolean
  Public Property sublogin_allow_text_message_flag() As Boolean

  Public Property sublogin_contract_amount() As Decimal
  Public Property sublogin_mpm_flag() As Boolean
  Public Property sublogin_values_flag() As Boolean
  Public Property sublogin_forgot_password_token() As String
  Public Property sublogin_forgot_password_token_date() As String
  Public Property sublogin_entry_date() As String


  Public Sub New()

    Try

      sublogin_sub_id = 0
      sublogin_login = ""
      sublogin_password = ""
      sublogin_contact_id = 0

      sublogin_active_flag = False
      sublogin_demo_flag = False
      sublogin_nbr_of_installs = 0
      sublogin_allow_export_flag = False
      sublogin_allow_local_notes_flag = False
      sublogin_allow_projects_flag = False
      sublogin_allow_email_request_flag = False
      sublogin_allow_event_request_flag = False
      sublogin_bypass_active_x_registry_flag = False
      sublogin_allow_text_message_flag = False

      sublogin_contract_amount = 0.0
      sublogin_mpm_flag = False
      sublogin_values_flag = False
      sublogin_forgot_password_token = ""
      sublogin_forgot_password_token_date = ""
      sublogin_entry_date = ""

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message

    End Try

  End Sub

  Public Sub New(ByVal inSubId As Long, ByVal inLogin As String)

    Try

      sublogin_sub_id = inSubId
      sublogin_login = inLogin
      sublogin_contact_id = 0

      sublogin_password = ""
      sublogin_active_flag = False
      sublogin_demo_flag = False
      sublogin_nbr_of_installs = 0
      sublogin_allow_export_flag = False
      sublogin_allow_local_notes_flag = False
      sublogin_allow_projects_flag = False
      sublogin_allow_email_request_flag = False
      sublogin_allow_event_request_flag = False
      sublogin_bypass_active_x_registry_flag = False
      sublogin_allow_text_message_flag = False

      sublogin_contract_amount = 0.0
      sublogin_mpm_flag = False
      sublogin_values_flag = False
      sublogin_forgot_password_token = ""
      sublogin_forgot_password_token_date = ""
      sublogin_entry_date = ""

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message

    End Try

  End Sub

  Public Function getSubscriptionLoginDataTable(ByVal inSubId As Long, ByVal inLogin As String) As DataTable

    Dim atemptable As New DataTable
    Dim subLoginQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      subLoginQuery.Append("SELECT * FROM " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Subscription_login WITH(NOLOCK)")
      subLoginQuery.Append(" WHERE sublogin_sub_id = @sublogin_sub_id AND sublogin_login = @sublogin_login")

      SqlCommand.Parameters.AddWithValue("@sublogin_sub_id", inSubId.ToString.Trim)
      SqlCommand.Parameters.AddWithValue("@sublogin_login", inLogin.Trim)

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 90

      SqlCommand.CommandText = subLoginQuery.ToString
      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()

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

  Public Sub fillSubscriptionLoginClass()

    Dim resultsTable As New DataTable

    Try

      If sublogin_sub_id = 0 And String.IsNullOrEmpty(sublogin_login) Then
        Exit Sub
      End If

      resultsTable = getSubscriptionLoginDataTable(sublogin_sub_id, sublogin_login)

      If Not IsNothing(resultsTable) Then

        If resultsTable.Rows.Count > 0 Then

          For Each r As DataRow In resultsTable.Rows

            If Not (IsDBNull(r("sublogin_password"))) Then
              sublogin_password = r.Item("sublogin_password").ToString.Trim
            End If

            If Not (IsDBNull(r("sublogin_active_flag"))) Then
              sublogin_active_flag = IIf(r.Item("sublogin_active_flag").ToString.ToUpper.Contains("Y"), True, False)
            End If

            If Not (IsDBNull(r("sublogin_demo_flag"))) Then
              sublogin_demo_flag = IIf(r.Item("sublogin_demo_flag").ToString.ToUpper.Contains("Y"), True, False)
            End If

            If Not (IsDBNull(r("sublogin_nbr_of_installs"))) Then
              If Not String.IsNullOrEmpty(r.Item("sublogin_nbr_of_installs").ToString.Trim) Then
                If IsNumeric(r.Item("sublogin_nbr_of_installs").ToString.Trim) Then
                  sublogin_nbr_of_installs = CInt(r.Item("sublogin_nbr_of_installs").ToString.Trim)
                End If
              End If
            End If

            If Not (IsDBNull(r("sublogin_allow_export_flag"))) Then
              sublogin_allow_export_flag = IIf(r.Item("sublogin_allow_export_flag").ToString.ToUpper.Contains("Y"), True, False)
            End If

            If Not (IsDBNull(r("sublogin_allow_local_notes_flag"))) Then
              sublogin_allow_local_notes_flag = IIf(r.Item("sublogin_allow_local_notes_flag").ToString.ToUpper.Contains("Y"), True, False)
            End If

            If Not (IsDBNull(r("sublogin_allow_projects_flag"))) Then
              sublogin_allow_projects_flag = IIf(r.Item("sublogin_allow_projects_flag").ToString.ToUpper.Contains("Y"), True, False)
            End If

            If Not (IsDBNull(r("sublogin_allow_email_request_flag"))) Then
              sublogin_allow_email_request_flag = IIf(r.Item("sublogin_allow_email_request_flag").ToString.ToUpper.Contains("Y"), True, False)
            End If

            If Not (IsDBNull(r("sublogin_allow_event_request_flag"))) Then
              sublogin_allow_event_request_flag = IIf(r.Item("sublogin_allow_event_request_flag").ToString.ToUpper.Contains("Y"), True, False)
            End If

            If Not (IsDBNull(r("sublogin_bypass_active_x_registry_flag"))) Then
              sublogin_bypass_active_x_registry_flag = IIf(r.Item("sublogin_bypass_active_x_registry_flag").ToString.ToUpper.Contains("Y"), True, False)
            End If

            If Not (IsDBNull(r("sublogin_allow_text_message_flag"))) Then
              sublogin_allow_text_message_flag = IIf(r.Item("sublogin_allow_text_message_flag").ToString.ToUpper.Contains("Y"), True, False)
            End If

            'sublogin_contract_amount = 0.0
            'sublogin_mpm_flag = False
            'sublogin_values_flag = False
            'sublogin_forgot_password_token = ""
            'sublogin_forgot_password_token_date = ""
            'sublogin_entry_date = ""

            If Not (IsDBNull(r("sublogin_contract_amount"))) Then
              If Not String.IsNullOrEmpty(r.Item("sublogin_contract_amount").ToString.Trim) Then
                If IsNumeric(r.Item("sublogin_contract_amount").ToString.Trim) Then
                  sublogin_contract_amount = CDec(r.Item("sublogin_contract_amount").ToString.Trim)
                End If
              End If
            End If

            If Not (IsDBNull(r("sublogin_mpm_flag"))) Then
              sublogin_mpm_flag = IIf(r.Item("sublogin_mpm_flag").ToString.ToUpper.Contains("Y"), True, False)
            End If

            If Not (IsDBNull(r("sublogin_values_flag"))) Then
              sublogin_values_flag = IIf(r.Item("sublogin_values_flag").ToString.ToUpper.Contains("Y"), True, False)
            End If

            If Not (IsDBNull(r("sublogin_forgot_password_token"))) Then
              sublogin_forgot_password_token = r.Item("sublogin_forgot_password_token").ToString.Trim
            End If

            If Not (IsDBNull(r("sublogin_forgot_password_token_date"))) Then
              sublogin_forgot_password_token_date = r.Item("sublogin_forgot_password_token_date").ToString.Trim
            End If

            If Not (IsDBNull(r("sublogin_entry_date"))) Then
              sublogin_entry_date = r.Item("sublogin_entry_date").ToString.Trim
            End If

          Next

        End If

      End If

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message

    End Try

  End Sub

  Public Sub updateSubscriptionLoginClass()
    Dim subLoginQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand

    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sSeperator = ""
    Try

      If sublogin_sub_id = 0 And String.IsNullOrEmpty(sublogin_login) And sublogin_contact_id = 0 Then
        Exit Sub
      End If

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 90

      subLoginQuery.Append("UPDATE Subscription_Install SET")

      If Not String.IsNullOrEmpty(sublogin_password.Trim) Then

        subLoginQuery.Append(sSeperator + " sublogin_password = @sublogin_password")
        SqlCommand.Parameters.AddWithValue("@sublogin_password", sublogin_password.Trim)
        sSeperator = ","

      End If

      subLoginQuery.Append(sSeperator + " sublogin_active_flag = @sublogin_active_flag")
      SqlCommand.Parameters.AddWithValue("@sublogin_active_flag", IIf(sublogin_active_flag, "Y", "N"))
      sSeperator = ","

      subLoginQuery.Append(sSeperator + " sublogin_demo_flag = @sublogin_demo_flag")
      SqlCommand.Parameters.AddWithValue("@sublogin_demo_flag", IIf(sublogin_demo_flag, "Y", "N"))
      sSeperator = ","

      If sublogin_nbr_of_installs >= 0 Then

        subLoginQuery.Append(sSeperator + " sublogin_nbr_of_installs = @sublogin_nbr_of_installs")
        SqlCommand.Parameters.AddWithValue("@sublogin_nbr_of_installs", sublogin_nbr_of_installs.ToString)
        sSeperator = ","

      End If

      subLoginQuery.Append(sSeperator + " sublogin_allow_export_flag = @sublogin_allow_export_flag")
      SqlCommand.Parameters.AddWithValue("@sublogin_allow_export_flag", IIf(sublogin_allow_export_flag, "Y", "N"))
      sSeperator = ","

      subLoginQuery.Append(sSeperator + " sublogin_allow_local_notes_flag = @sublogin_allow_local_notes_flag")
      SqlCommand.Parameters.AddWithValue("@sublogin_allow_local_notes_flag", IIf(sublogin_allow_local_notes_flag, "Y", "N"))
      sSeperator = ","

      subLoginQuery.Append(sSeperator + " sublogin_allow_projects_flag = @sublogin_allow_projects_flag")
      SqlCommand.Parameters.AddWithValue("@sublogin_allow_projects_flag", IIf(sublogin_allow_projects_flag, "Y", "N"))
      sSeperator = ","

      subLoginQuery.Append(sSeperator + " sublogin_allow_email_request_flag = @sublogin_allow_email_request_flag")
      SqlCommand.Parameters.AddWithValue("@sublogin_allow_email_request_flag", IIf(sublogin_allow_email_request_flag, "Y", "N"))
      sSeperator = ","

      subLoginQuery.Append(sSeperator + " sublogin_allow_event_request_flag = @sublogin_allow_event_request_flag")
      SqlCommand.Parameters.AddWithValue("@sublogin_allow_event_request_flag", IIf(sublogin_allow_event_request_flag, "Y", "N"))
      sSeperator = ","

      subLoginQuery.Append(sSeperator + " sublogin_bypass_active_x_registry_flag = @sublogin_bypass_active_x_registry_flag")
      SqlCommand.Parameters.AddWithValue("@sublogin_bypass_active_x_registry_flag", IIf(sublogin_bypass_active_x_registry_flag, "Y", "N"))
      sSeperator = ","

      subLoginQuery.Append(sSeperator + " sublogin_allow_text_message_flag = @sublogin_allow_text_message_flag")
      SqlCommand.Parameters.AddWithValue("@sublogin_allow_text_message_flag", IIf(sublogin_allow_text_message_flag, "Y", "N"))
      sSeperator = ","

      ' set web action date to null for any changes
      subLoginQuery.Append(sSeperator + " sublogin_web_action_date = @sublogin_web_action_date")
      SqlCommand.Parameters.AddWithValue("@sublogin_web_action_date", DBNull.Value)

      subLoginQuery.Append(" WHERE sublogin_sub_id = @sublogin_sub_id AND sublogin_login = @sublogin_login AND sublogin_contact_id = @sublogin_contact_id")

      SqlCommand.Parameters.AddWithValue("@sublogin_sub_id", sublogin_sub_id.ToString.Trim)
      SqlCommand.Parameters.AddWithValue("@sublogin_login", sublogin_login.Trim)
      SqlCommand.Parameters.AddWithValue("@sublogin_contact_id", sublogin_contact_id.ToString.Trim)

      SqlCommand.CommandText = subLoginQuery.ToString

      SqlCommand.ExecuteNonQuery()

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message

    Finally

      SqlConn.Dispose()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

  End Sub

  Public Sub insertSubscriptionLoginClass()

    Dim subLoginQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand

    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      If sublogin_sub_id = 0 And String.IsNullOrEmpty(sublogin_login) And sublogin_contact_id = 0 Then
        Exit Sub
      End If

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 90

      subLoginQuery.Append("INSERT INTO Subscription (sublogin_password, sublogin_active_flag, sublogin_demo_flag, sublogin_nbr_of_installs, sublogin_allow_export_flag, sublogin_allow_local_notes_flag,")
      subLoginQuery.Append(" sublogin_allow_projects_flag, sublogin_allow_email_request_flag, sublogin_allow_event_request_flag, sublogin_bypass_active_x_registry_flag, sublogin_allow_text_message_flag, sublogin_web_action_date")
      subLoginQuery.Append(") VALUES (@sublogin_password, @sublogin_active_flag, @sublogin_demo_flag, @sublogin_nbr_of_installs, @sublogin_allow_export_flag, @sublogin_allow_local_notes_flag,")
      subLoginQuery.Append(" @sublogin_allow_projects_flag, @sublogin_allow_email_request_flag, @sublogin_allow_event_request_flag, @sublogin_bypass_active_x_registry_flag, @sublogin_allow_text_message_flag, @sublogin_web_action_date")
      subLoginQuery.Append(") WHERE sublogin_sub_id = @sublogin_sub_id AND sublogin_login = @sublogin_login AND sublogin_contact_id = @sublogin_contact_id")

      If Not String.IsNullOrEmpty(sublogin_password.Trim) Then
        SqlCommand.Parameters.AddWithValue("@sublogin_password", sublogin_password.Trim)
      End If

      SqlCommand.Parameters.AddWithValue("@sublogin_active_flag", IIf(sublogin_active_flag, "Y", "N"))

      SqlCommand.Parameters.AddWithValue("@sublogin_demo_flag", IIf(sublogin_demo_flag, "Y", "N"))

      If sublogin_nbr_of_installs >= 0 Then
        SqlCommand.Parameters.AddWithValue("@sublogin_nbr_of_installs", sublogin_nbr_of_installs.ToString)
      End If

      SqlCommand.Parameters.AddWithValue("@sublogin_allow_export_flag", IIf(sublogin_allow_export_flag, "Y", "N"))

      SqlCommand.Parameters.AddWithValue("@sublogin_allow_local_notes_flag", IIf(sublogin_allow_local_notes_flag, "Y", "N"))

      SqlCommand.Parameters.AddWithValue("@sublogin_allow_projects_flag", IIf(sublogin_allow_projects_flag, "Y", "N"))

      SqlCommand.Parameters.AddWithValue("@sublogin_allow_email_request_flag", IIf(sublogin_allow_email_request_flag, "Y", "N"))

      SqlCommand.Parameters.AddWithValue("@sublogin_allow_event_request_flag", IIf(sublogin_allow_event_request_flag, "Y", "N"))

      SqlCommand.Parameters.AddWithValue("@sublogin_bypass_active_x_registry_flag", IIf(sublogin_bypass_active_x_registry_flag, "Y", "N"))

      SqlCommand.Parameters.AddWithValue("@sublogin_allow_text_message_flag", IIf(sublogin_allow_text_message_flag, "Y", "N"))

      ' set web action date to null for any changes
      SqlCommand.Parameters.AddWithValue("@sublogin_web_action_date", DBNull.Value)


      SqlCommand.Parameters.AddWithValue("@sublogin_sub_id", sublogin_sub_id.ToString.Trim)
      SqlCommand.Parameters.AddWithValue("@sublogin_login", sublogin_login.Trim)
      SqlCommand.Parameters.AddWithValue("@sublogin_contact_id", sublogin_contact_id.ToString.Trim)

      SqlCommand.CommandText = subLoginQuery.ToString

      SqlCommand.ExecuteNonQuery()

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
    Dim [class] = TryCast(obj, homebaseLoginClass)
    Return [class] IsNot Nothing AndAlso
           sublogin_sub_id = [class].sublogin_sub_id AndAlso
           sublogin_login = [class].sublogin_login AndAlso
           sublogin_password = [class].sublogin_password AndAlso
           sublogin_contact_id = [class].sublogin_contact_id AndAlso
           sublogin_active_flag = [class].sublogin_active_flag AndAlso
           sublogin_demo_flag = [class].sublogin_demo_flag AndAlso
           sublogin_nbr_of_installs = [class].sublogin_nbr_of_installs AndAlso
           sublogin_allow_export_flag = [class].sublogin_allow_export_flag AndAlso
           sublogin_allow_local_notes_flag = [class].sublogin_allow_local_notes_flag AndAlso
           sublogin_allow_projects_flag = [class].sublogin_allow_projects_flag AndAlso
           sublogin_allow_email_request_flag = [class].sublogin_allow_email_request_flag AndAlso
           sublogin_allow_event_request_flag = [class].sublogin_allow_event_request_flag AndAlso
           sublogin_bypass_active_x_registry_flag = [class].sublogin_bypass_active_x_registry_flag AndAlso
           sublogin_allow_text_message_flag = [class].sublogin_allow_text_message_flag
  End Function

  Public Shared Operator =(class1 As homebaseLoginClass, class2 As homebaseLoginClass) As Boolean
    Return EqualityComparer(Of homebaseLoginClass).Default.Equals(class1, class2)
  End Operator

  Public Shared Operator <>(class1 As homebaseLoginClass, class2 As homebaseLoginClass) As Boolean
    Return Not class1 = class2
  End Operator

End Class
