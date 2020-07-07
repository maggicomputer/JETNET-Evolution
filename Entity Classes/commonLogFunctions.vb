' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/Entity Classes/commonLogFunctions.vb $
'$$Author: Mike $
'$$Date: 6/30/20 3:45p $
'$$Modtime: 6/30/20 3:40p $
'$$Revision: 12 $
'$$Workfile: commonLogFunctions.vb $
'
' ********************************************************************************

<System.Serializable()> Public Class commonLogFunctions

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

    LogStatusCode = eObjStatusCode.NULL
    LogDetailError = eObjDetailErrorCode.NULL
    LogSessionGUID = ""

  End Sub

  Public Property class_error() As String
    Get
      class_error = aError
    End Get
    Set(ByVal value As String)
      aError = value
    End Set
  End Property

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

  Public Property LogStatusCode() As eObjStatusCode

  Public Property LogDetailError() As eObjDetailErrorCode

  Public Property LogSessionGUID() As String



  Public Shared Sub Log_User_Event_Data(ByVal type_of_insert As String, ByVal message As String,
                                        Optional ByRef sqlcommand2 As SqlClient.SqlCommand = Nothing,
                                        Optional ByVal view_id As Long = 0, Optional ByVal journ_id As Long = 0,
                                        Optional ByVal wanted_id As Long = 0, Optional ByVal comp_id As Long = 0,
                                        Optional ByVal contact_id As Long = 0, Optional ByVal ac_id As Long = 0,
                                        Optional ByVal amod_id As Long = 0, Optional ByVal yacht_id As Long = 0,
                                        Optional ByVal ActionDatePassed As String = "")

    Dim insert_string As String = ""
    Dim string_fields As String = ""
    Dim string_values As String = ""
    Dim UserIPAddress As String = ""

    Dim WebSiteTypeURL As String = ""

    WebSiteTypeURL = HttpContext.Current.Session.Item("jetnetFullHostName").ToString.ToLower.Replace("http://", "").Replace("https://", "").Replace("www.", "").Replace("/", "")

    'Then we go ahead and put it back in uppercase:
    WebSiteTypeURL = WebSiteTypeURL.ToUpper


    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim sqlcommand3 As New System.Data.SqlClient.SqlCommand
    Dim Query As String : Query = ""
    Dim close_conn As Boolean = False
    Dim useBackupSQL As Boolean = CBool(My.Settings.useBackupSQL_SRV.ToString)


    Try
      ' if there is no type of event passed in or it is passed in blank, dont do the insert or anything
      If Trim(type_of_insert) <> "" Then

        ' if you dont pass in a sqlcommand, which means that you dont have an open connection to use, then create one
        If IsNothing(sqlcommand2) Then

          SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase")

          SqlConn.Open()


          sqlcommand3.Connection = SqlConn
          sqlcommand3.CommandType = System.Data.CommandType.Text
          sqlcommand3.CommandTimeout = 60
          close_conn = True
        End If




        ' add the variables for the user
        string_fields += "subislog_subid, subislog_login,subislog_seq_no,subislog_email_address,subislog_subins_contact_id,subislog_mobile_flag,"

        If HttpContext.Current.Session.Item("jetnetWebHostType") = eWebHostTypes.CRM Then
          string_values += "'" & HttpContext.Current.Session.Item("localSubscription").crmSubscriptionID.ToString.Trim & "',"                '   o	subislog_subid
        Else ' use HttpContext.Current.Session.Item("localUser").crmSubSubID when on EVO
          string_values += "'" & HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString.Trim & "',"                '   o	subislog_subid
        End If


        If HttpContext.Current.Session.Item("jetnetWebHostType") = eWebHostTypes.CRM Then
          string_values += "'" & HttpContext.Current.Session.Item("CRMJetnetUserName").ToString.Trim & "',"               '	o	subislog_login
        Else ' use HttpContext.Current.Session.Item("localUser").crmUserLogin when on EVO
          string_values += "'" & HttpContext.Current.Session.Item("localUser").crmUserLogin.ToString.Trim & "',"               '	o	subislog_login
        End If

        string_values += "'" & HttpContext.Current.Session.Item("localUser").crmSubSeqNo.ToString.Trim & "',"                '	o	subislog_seq_no
        string_values += "'" & HttpContext.Current.Session.Item("localUser").crmLocalUserEmailAddress.ToString.Trim & "',"   '	o	subislog_email_address
        string_values += "'" & HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString.Trim & "',"           '	o	subislog_subins_contact_id

        If HttpContext.Current.Session.Item("localUser").crmMobileFlag.ToString.Trim = "True" Then
          string_values += "'Y',"              '	o	subislog_mobile_flag
        Else
          string_values += "'N',"              '	o	subislog_mobile_flag
        End If


        ' add the event type into the msg type field, will be passed in 
        If Trim(type_of_insert) <> "" Then
          string_fields += "subislog_msg_type, "
          string_values += "'" & Trim(type_of_insert) & "',"
        End If

        ' add the host name so that you know what server they are connected from 
        string_fields += "subislog_host_name, "
        'string_values += "'" & HttpContext.Current.Application.Item("crmClientSiteData").crmClientHostName.ToString & "',"
        'This is the line where I swap out the app variable for the session variable that's been parsed up above.
        string_values += "'" & WebSiteTypeURL & "',"


        ' add the application name so that we know what site they are coming in from 
        string_fields += "subislog_app_name, "
        string_values += "'" & HttpContext.Current.Session.Item("localPreferences").AppUserName.ToString & "',"

        'add the message in that was passed from the page 
        string_fields += "subislog_message, "

        'clean up any double quotes any " + " and change single tick to double tick or query will bomb out on insert.
        message = message.Replace(Chr(34), "").Replace(" + ", "").Replace("'", "''")
        string_values += "'" & Left(message, 2000) & "',"



        string_fields += "subislog_ac_id, subislog_amod_id, subislog_comp_id, subislog_contact_id, subislog_wanted_id, subislog_journ_id, subislog_view_id, subislog_yt_id, "
        string_values += "'" & ac_id & "',"         'o	subislog_ac_id – Aircraft ID clicked on (aircraft model id will be stored as well for each aircraft click)
        string_values += "'" & amod_id & "',"       'o	subislog_amod_id – Aircraft Model ID clicked on 
        string_values += "'" & comp_id & "',"       'o	subislog_comp_id – Company ID clicked on
        string_values += "'" & contact_id & "',"    'o	subislog_contact_id – Contact ID clicked on (company id will be stored as well for each contact click)
        string_values += "'" & wanted_id & "',"     'o	subislog_wanted_id – Aircraft ID that was clicked on (model id will be stored as well for each wanted click)
        string_values += "'" & journ_id & "',"      'o	subislog_journ_id – Journal ID that was clicked on (company or aircraft id will be stored as well for each journal click)
        string_values += "'" & view_id & "',"       'o	subislog_view_id – View ID clicked on (model id will be stored as well if appropriate when the view is clicked on)
        string_values += "'" & yacht_id & "',"       'o	subislog_yt_id – View ID clicked on (model id will be stored as well if appropriate when the view is clicked on)


        ' set the date time of the current insert
        string_fields += "subislog_date, "
        string_values += "'" & DateTime.Now & "',"

        ' set the date time webaction date, but only if it's passed.
        'subislog_webaction_date - This gets passed on the Notify.ascx control. 
        If Not String.IsNullOrEmpty(ActionDatePassed) Then
          string_fields += "subislog_webaction_date, "
          string_values += "'" & ActionDatePassed & "',"
        End If

        ' insert the unique guid
        string_fields += "subislog_session_guid, "
        string_values += "'" & HttpContext.Current.Session.Item("localUser").crmGUID.ToString() & "',"

        string_fields += "subislog_password, "
        string_values += "'" & HttpContext.Current.Session.Item("localUser").crmLocalUserPswd.ToString() & "',"

        ' not sure of fields
        string_fields += "subislog_tcpip "

        If Not IsNothing(HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR")) Then
          If Not String.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR").ToString.Trim) Then
            UserIPAddress = HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR").ToString.Trim
          End If
        End If

        If String.IsNullOrEmpty(UserIPAddress.Trim) Then
          UserIPAddress = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR").ToString.Trim
        End If

        string_values += "'" & UserIPAddress & "'"

        insert_string = " insert into Subscription_Install_Log ( "
        insert_string += string_fields
        insert_string += ") VALUES ("
        insert_string += string_values
        insert_string += ") "


        If close_conn Then
          sqlcommand3.CommandText = insert_string
          sqlcommand3.ExecuteNonQuery()
          sqlcommand3.Dispose()
          sqlcommand3 = Nothing
        Else
          sqlcommand2.CommandText = insert_string
          sqlcommand2.ExecuteNonQuery()
        End If


        clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "commonLogFunctions.vb", insert_string.ToString)
      End If

    Catch ex As Exception
      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "commonLogFunctions.vb", ex.Message)
    Finally
      ' this means that you had to open the connection, so you now have to clean up and close it.
      If close_conn Then
        SqlConn.Close()
        SqlConn.Dispose()
      End If
    End Try

  End Sub

  Public Shared Function forceLogError(Optional ByVal message_type As String = "", Optional ByVal message As String = "",
                                       Optional ByVal view_id As Long = 0, Optional ByVal journ_id As Long = 0,
                                       Optional ByVal wanted_id As Long = 0, Optional ByVal comp_id As Long = 0,
                                       Optional ByVal contact_id As Long = 0, Optional ByVal ac_id As Long = 0,
                                       Optional ByVal amod_id As Long = 0, Optional ByVal yacht_id As Long = 0,
                                       Optional ByVal action_date As String = "") As Boolean

    Dim WebSiteTypeURL As String = ""

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim sQuery As New StringBuilder

    Dim bResult As Boolean = False

    Dim sSubscriptionID As String = ""
    Dim sUserID As String = ""
    Dim sSequenceNo As String = ""
    Dim sEmailAddress As String = ""
    Dim sPassword As String = ""
    Dim sUserContactID As String = ""
    Dim sMobileFlag As String = ""
    Dim sConnectionStr As String = ""

    If Not IsNothing(HttpContext.Current.Session.Item("localUser")) Then

      If Not IsNothing(HttpContext.Current.Session.Item("localUser").crmSubSubID) Then
        If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString.Trim) Then
          sSubscriptionID = HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString.Trim
        End If
      End If

      If Not IsNothing(HttpContext.Current.Session.Item("localUser").crmUserLogin) Then
        If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("localUser").crmUserLogin.ToString.Trim) Then
          sUserID = HttpContext.Current.Session.Item("localUser").crmUserLogin.ToString.Trim
        End If
      End If

      If Not IsNothing(HttpContext.Current.Session.Item("localUser").crmSubSeqNo) Then
        If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("localUser").crmSubSeqNo.ToString.Trim) Then
          sSequenceNo = HttpContext.Current.Session.Item("localUser").crmSubSeqNo.ToString.Trim
        End If
      End If

      If Not IsNothing(HttpContext.Current.Session.Item("localUser").crmLocalUserEmailAddress) Then
        If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("localUser").crmLocalUserEmailAddress.ToString.Trim) Then
          sEmailAddress = HttpContext.Current.Session.Item("localUser").crmLocalUserEmailAddress.ToString.Trim
        End If
      End If

      If Not IsNothing(HttpContext.Current.Session.Item("localUser").crmLocalUserPswd) Then
        If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("localUser").crmLocalUserPswd.ToString.Trim) Then
          sPassword = HttpContext.Current.Session.Item("localUser").crmLocalUserPswd.ToString.Trim
        End If
      End If

      If Not IsNothing(HttpContext.Current.Session.Item("localUser").crmUserContactID) Then
        If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString.Trim) Then
          sUserContactID = HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString.Trim
        End If
      End If

      If Not IsNothing(HttpContext.Current.Session.Item("localUser").crmMobileFlag) Then
        If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("localUser").crmMobileFlag.ToString.Trim) Then
          sMobileFlag = IIf(CBool(HttpContext.Current.Session.Item("localUser").crmMobileFlag), "Y", "N")
        End If
      End If

    End If

    Dim sMessageType As String = message_type.Trim
    Dim sWebHostName As String = ""

    Dim sAppName As String = ""

    If Not IsNothing(HttpContext.Current.Session.Item("localPreferences")) Then

      If Not IsNothing(HttpContext.Current.Session.Item("localPreferences").AppUserName) Then
        If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("localPreferences").AppUserName.ToString.Trim) Then
          sAppName = HttpContext.Current.Session.Item("localPreferences").AppUserName.ToString.Trim
        End If
      End If

    End If

    Dim sMessage As String = Left(message.Replace("'", "''"), 2000)
    Dim sActionDate As String = action_date.Trim

    Dim sUserGUID As String = ""

    If Not IsNothing(HttpContext.Current.Session.Item("localUser")) Then

      If Not IsNothing(HttpContext.Current.Session.Item("localUser").crmGUID) Then
        If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("localUser").crmGUID.ToString.Trim) Then
          sUserGUID = HttpContext.Current.Session.Item("localUser").crmGUID.ToString.Trim
        End If
      End If

    End If

    Dim sLocalIPAddress As String = ""

    If Not IsNothing(HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR")) Then
      If Not String.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR").ToString.Trim) Then
        sLocalIPAddress = HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR").ToString.Trim
      End If
    End If

    If String.IsNullOrEmpty(sLocalIPAddress.Trim) Then
      If Not IsNothing(HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")) Then
        If Not String.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables("REMOTE_ADDR").ToString.Trim) Then
          sLocalIPAddress = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR").ToString.Trim
        End If
      End If
    End If

    Dim sAircraftID As String = ac_id.ToString
    Dim sModelID As String = amod_id.ToString
    Dim sCompanyID As String = comp_id.ToString
    Dim sContactID As String = contact_id.ToString
    Dim sWantedID As String = wanted_id.ToString
    Dim sJournalID As String = journ_id.ToString
    Dim sViewID As String = view_id.ToString
    Dim sYachtID As String = yacht_id.ToString

    Try

      If Not IsNothing(HttpContext.Current.Session.Item("jetnetFullHostName")) Then
        If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("jetnetFullHostName").ToString.Trim) Then
          WebSiteTypeURL = HttpContext.Current.Session.Item("jetnetFullHostName").ToString.Trim
        End If
      End If

      WebSiteTypeURL = WebSiteTypeURL.ToLower.Replace("http://", "").Replace("https://", "").Replace("www.", "").Replace("/", "").ToUpper

      sWebHostName = WebSiteTypeURL.ToUpper

      If Not String.IsNullOrEmpty(message_type.Trim) Then

        sQuery.Append("INSERT INTO Subscription_Install_Log (subislog_subid, subislog_login, subislog_seq_no, subislog_email_address, subislog_subins_contact_id, subislog_mobile_flag,")
        sQuery.Append(" subislog_msg_type, subislog_host_name, subislog_app_name, subislog_message, subislog_date, subislog_webaction_date, subislog_session_guid, subislog_tcpip,")
        sQuery.Append(" subislog_ac_id, subislog_amod_id, subislog_comp_id, subislog_contact_id, subislog_wanted_id, subislog_journ_id, subislog_view_id, subislog_yt_id, subislog_password")
        sQuery.Append(") VALUES (")

        sQuery.Append(IIf(Not String.IsNullOrEmpty(sSubscriptionID.Trim), sSubscriptionID.Trim, "NULL") + ", ")
        sQuery.Append(IIf(Not String.IsNullOrEmpty(sUserID.Trim), "'" + sUserID.Trim + "'", "NULL") + ", ")
        sQuery.Append(IIf(Not String.IsNullOrEmpty(sSequenceNo.Trim), sSequenceNo.Trim, "NULL") + ", ")
        sQuery.Append(IIf(Not String.IsNullOrEmpty(sEmailAddress.Trim), "'" + sEmailAddress.Trim + "'", "NULL") + ", ")
        sQuery.Append(IIf(Not String.IsNullOrEmpty(sUserContactID.Trim), sUserContactID.Trim, "-1") + ", ")
        sQuery.Append(IIf(Not String.IsNullOrEmpty(sMobileFlag.Trim), "'" + sMobileFlag.Trim + "'", "NULL") + ", ")

        sQuery.Append(IIf(Not String.IsNullOrEmpty(sMessageType.Trim), "'" + sMessageType.Trim + "'", "NULL") + ", ")
        sQuery.Append(IIf(Not String.IsNullOrEmpty(sWebHostName.Trim), "'" + sWebHostName.Trim + "'", "NULL") + ", ")
        sQuery.Append(IIf(Not String.IsNullOrEmpty(sAppName.Trim), "'" + sAppName.Trim + "'", "NULL") + ", ")
        sQuery.Append(IIf(Not String.IsNullOrEmpty(sMessage.Trim), "'" + sMessage.Trim + "'", "NULL") + ", ")
        sQuery.Append("'" + DateTime.Now + "', ")
        sQuery.Append(IIf(Not String.IsNullOrEmpty(sActionDate.Trim), "'" + sActionDate.Trim + "'", "NULL") + ", ")
        sQuery.Append(IIf(Not String.IsNullOrEmpty(sUserGUID.Trim), "'" + sUserGUID.Trim + "'", "NULL") + ", ")
        sQuery.Append(IIf(Not String.IsNullOrEmpty(sLocalIPAddress.Trim), "'" + sLocalIPAddress.Trim + "'", "NULL") + ", ")

        sQuery.Append(IIf(Not String.IsNullOrEmpty(sAircraftID.Trim), sAircraftID.Trim, "-1") + ", ")
        sQuery.Append(IIf(Not String.IsNullOrEmpty(sModelID.Trim), sModelID.Trim, "-1") + ", ")
        sQuery.Append(IIf(Not String.IsNullOrEmpty(sCompanyID.Trim), sCompanyID.Trim, "-1") + ", ")
        sQuery.Append(IIf(Not String.IsNullOrEmpty(sContactID.Trim), sContactID.Trim, "-1") + ", ")
        sQuery.Append(IIf(Not String.IsNullOrEmpty(sWantedID.Trim), sWantedID.Trim, "-1") + ", ")
        sQuery.Append(IIf(Not String.IsNullOrEmpty(sJournalID.Trim), sJournalID.Trim, "-1") + ", ")
        sQuery.Append(IIf(Not String.IsNullOrEmpty(sViewID.Trim), sViewID.Trim, "-1") + ", ")
        sQuery.Append(IIf(Not String.IsNullOrEmpty(sYachtID.Trim), sYachtID.Trim, "-1") + ", ")

        sQuery.Append(IIf(Not String.IsNullOrEmpty(sPassword.Trim), "'" + sPassword.Trim + "'", "NULL") + ")")

        'failed_password
        clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "commonLogFunctions.vb", sQuery.ToString)

        Try

          Dim useBackupSQL As Boolean = CBool(My.Settings.useBackupSQL_SRV.ToString)

          If Not IsNothing(HttpContext.Current.Application.Item("crmClientSiteData").AdminDatabaseConn) Then

            If Not String.IsNullOrEmpty(HttpContext.Current.Application.Item("crmClientSiteData").AdminDatabaseConn.ToString.Trim) Then
              sConnectionStr = HttpContext.Current.Application.Item("crmClientSiteData").AdminDatabaseConn.ToString.Trim
            Else

              If HttpContext.Current.Application.Item("crmClientSiteData").WebSiteType <> eWebSiteTypes.LOCAL Then
                If Not useBackupSQL Then
                  ' if doesnt have local sql server use live default sql connection
                  sConnectionStr = My.Settings.DEFAULT_LIVE_MSSQL
                Else
                  ' if doesnt have local sql server use backup default sql connection
                  sConnectionStr = My.Settings.DEFAULT_LIVE_MSSQL_BK
                End If
              Else
                If Not useBackupSQL Then
                  ' if doesnt have local sql server use live default sql connection
                  sConnectionStr = My.Settings.TEST_LOCAL_MSSQL
                Else
                  ' if doesnt have local sql server use backup default sql connection
                  sConnectionStr = My.Settings.TEST_LOCAL_MSSQL_BK
                End If
              End If

            End If

          Else

            If HttpContext.Current.Application.Item("crmClientSiteData").WebSiteType <> eWebSiteTypes.LOCAL Then
              If Not useBackupSQL Then
                ' if doesnt have local sql server use live default sql connection
                sConnectionStr = My.Settings.DEFAULT_LIVE_MSSQL
              Else
                ' if doesnt have local sql server use backup default sql connection
                sConnectionStr = My.Settings.DEFAULT_LIVE_MSSQL_BK
              End If
            Else
              If Not useBackupSQL Then
                ' if doesnt have local sql server use live default sql connection
                sConnectionStr = My.Settings.TEST_LOCAL_MSSQL
              Else
                ' if doesnt have local sql server use backup default sql connection
                sConnectionStr = My.Settings.TEST_LOCAL_MSSQL_BK
              End If
            End If

          End If

          SqlConn.ConnectionString = sConnectionStr
          SqlConn.Open()

          SqlCommand.Connection = SqlConn
          SqlCommand.CommandTimeout = 1000
          SqlCommand.CommandText = sQuery.ToString

          SqlCommand.ExecuteNonQuery()
          bResult = True

        Catch SqlException

          If Not IsNothing(HttpContext.Current.Session.Item("localUser")) Then

            If Not IsNothing(HttpContext.Current.Session.Item("localUser").crmUser_DebugText) Then
              If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("localUser").crmUser_DebugText.ToString.Trim) Then
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />SQLError in forceLogError(...) ExecuteNonQuery<br />" + SqlException.Message
              Else
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText = "<br /><br />SQLError in forceLogError(...) ExecuteNonQuery<br />" + SqlException.Message
              End If
            End If

          End If

        Finally

          SqlConn.Dispose()
          SqlConn.Close()
          SqlConn = Nothing

          SqlCommand.Dispose()
          SqlCommand = Nothing

        End Try

      End If

    Catch ex As Exception

      If Not IsNothing(HttpContext.Current.Session.Item("localUser")) Then

        If Not IsNothing(HttpContext.Current.Session.Item("localUser").crmUser_DebugText) Then
          If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("localUser").crmUser_DebugText.ToString.Trim) Then
            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />SQLError in forceLogError(...) ExecuteNonQuery<br />" + SqlException.Message
          Else
            HttpContext.Current.Session.Item("localUser").crmUser_DebugText = "<br /><br />SQLError in forceLogError(...) ExecuteNonQuery<br />" + SqlException.Message
          End If
        End If

      End If

    Finally

    End Try

    Return bResult

  End Function


  Public Shared Function InsertAPILog(ByVal apiact_call_token As String, ByVal apiact_sub_id As Long, ByVal apiact_comp_id As Long, ByVal apiact_contact_id As Long, ByVal apiact_email_address As String, ByVal apiact_password As String, ByVal apiact_type As String, ByVal apiact_request_comp_id As Long, ByVal apiact_request_ac_id As Long, ByVal apiact_request_contact_id As Long, ByVal apiact_request_amod_id As Long, ByVal apiact_notes As String) As Boolean
    Dim QueryFields As String = ""
    Dim QueryValues As String = ""
    Dim Query As String = ""

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim ResponseCode As Boolean = False
    Try
      '[apiact_request_date] [datetime] NULL,
      QueryFields = "insert into API_Activity_Log(apiact_request_date, "
      QueryValues = " values (@apiact_request_date,"

      '[apiact_ip_address] [varchar](70) NULL,
      QueryFields += "apiact_ip_address, "
      QueryValues += "@apiact_ip_address,"

      '[apiact_call_token] [varchar](50) NULL,
      QueryFields += "apiact_call_token, "
      QueryValues += "@apiact_call_token,"

      '[apiact_sub_id] [int] NULL,
      QueryFields += "apiact_sub_id, "
      QueryValues += "@apiact_sub_id,"

      '[apiact_comp_id] [int] NULL,
      QueryFields += "apiact_comp_id, "
      QueryValues += "@apiact_comp_id,"

      '[apiact_contact_id] [int] NULL,
      QueryFields += "apiact_contact_id, "
      QueryValues += "@apiact_contact_id,"

      '[apiact_email_address] [varchar](150) NULL,
      QueryFields += "apiact_email_address, "
      QueryValues += "@apiact_email_address,"

      '[apiact_password] [varchar](50) NULL,
      QueryFields += "apiact_password, "
      QueryValues += "@apiact_password,"

      '[apiact_type] [varchar](50) NULL,
      QueryFields += "apiact_type, "
      QueryValues += "@apiact_type,"

      '[apiact_request_comp_id] [int] NULL,
      QueryFields += "apiact_request_comp_id, "
      QueryValues += "@apiact_request_comp_id,"

      '[apiact_request_ac_id] [int] NULL,
      QueryFields += "apiact_request_ac_id, "
      QueryValues += "@apiact_request_ac_id,"

      '[apiact_request_contact_id] [int] NULL,
      QueryFields += "apiact_request_contact_id, "
      QueryValues += "@apiact_request_contact_id,"

      '[apiact_request_amod_id] [int] NULL,
      QueryFields += "apiact_request_amod_id, "
      QueryValues += "@apiact_request_amod_id,"

      '[apiact_notes] [varchar](1000) NULL,
      QueryFields += "apiact_notes) "
      QueryValues += "@apiact_notes)"


      Query = QueryFields & QueryValues

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      SqlConn.Open()


      Dim SqlCommand As New SqlClient.SqlCommand(Query, SqlConn)
      Dim apiact_ip_address As String = ""

      If Not IsNothing(HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR")) Then
        If Not String.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR").ToString.Trim) Then
          apiact_ip_address = HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR").ToString.Trim
        End If
      End If

      If String.IsNullOrEmpty(apiact_ip_address.Trim) Then
        apiact_ip_address = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR").ToString.Trim
      End If

      SqlCommand.Parameters.AddWithValue("@apiact_request_date", FormatDateTime(Now(), vbGeneralDate))
      SqlCommand.Parameters.AddWithValue("@apiact_ip_address", apiact_ip_address)
      SqlCommand.Parameters.AddWithValue("@apiact_call_token", Left(apiact_call_token, 50))
      SqlCommand.Parameters.AddWithValue("@apiact_sub_id", apiact_sub_id)
      SqlCommand.Parameters.AddWithValue("@apiact_comp_id", apiact_comp_id)
      SqlCommand.Parameters.AddWithValue("@apiact_contact_id", apiact_contact_id)
      SqlCommand.Parameters.AddWithValue("@apiact_email_address", Left(apiact_email_address, 150))
      SqlCommand.Parameters.AddWithValue("@apiact_password", Left(apiact_password, 50))
      SqlCommand.Parameters.AddWithValue("@apiact_type", Left(apiact_type, 50))
      SqlCommand.Parameters.AddWithValue("@apiact_request_comp_id", apiact_request_comp_id)
      SqlCommand.Parameters.AddWithValue("@apiact_request_ac_id", apiact_request_ac_id)
      SqlCommand.Parameters.AddWithValue("@apiact_request_contact_id", apiact_request_contact_id)
      SqlCommand.Parameters.AddWithValue("@apiact_request_amod_id", apiact_request_amod_id)


      SqlCommand.Parameters.AddWithValue("@apiact_notes", Left(apiact_notes, 1000))


      SqlCommand.ExecuteNonQuery()

      ResponseCode = True

      SqlCommand.Dispose()
      SqlCommand = Nothing

      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "commonLogFunctions.vb", Query.ToString)

    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in " & System.Reflection.MethodBase.GetCurrentMethod().Name.ToString & ": " & ex.Message & "<br />"
      Return Nothing
    Finally
      'kill everything
      SqlConn.Close()
      SqlConn.Dispose()
      SqlConn = Nothing

    End Try
    Return ResponseCode
  End Function



End Class
