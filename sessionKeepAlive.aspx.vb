' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/sessionKeepAlive.aspx.vb $
'$$Author: Mike $
'$$Date: 6/19/19 8:41a $
'$$Modtime: 6/18/19 6:12p $
'$$Revision: 2 $
'$$Workfile: sessionKeepAlive.aspx.vb $
'
' ********************************************************************************

Partial Public Class sessionKeepAlive
  Inherits System.Web.UI.Page
  Dim aclsData_Temp As New Object
  Dim aTempTable, aTempTable2 As New DataTable 'Data Tables used

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Try
      aclsData_Temp = New clsData_Manager_SQL

      'Setting up the client, ref, history db connection strings from the application items, also declaring the data manager error property as blank.
      aclsData_Temp.JETNET_DB = Application.Item("crmJetnetDatabase")

      If Session.Item("localUser").crmEvo = False Then
        aclsData_Temp.client_DB = Application.Item("crmClientDatabase")
      Else
        aclsData_Temp.JETNET_DB = Session.Item("jetnetClientDatabase")
      End If


      aclsData_Temp.class_error = ""
      '------------------------------------------------------End Database Connection Information----------------------------------------------



      '---------------------------------------------------------------------------------------------------------------------------------------
      '-----------------------------------------------Automatic Refresh Page Load-------------------------------------------------------------
      '---------------------------------------------------------------------------------------------------------------------------------------
      '---------------------------------------------------------------------------------------------------------------------------------------
      'First we check to see if the Current Timestamp Session Variable is a date
      If IsDate(Session.Item("TimeStamp")) Then
        Dim x As New Date
        'Declaring 
        x = Session.Item("TimeStamp")
        'Next we're determining the timespan between now and the last timestamp.
        'This is only going to run on subsequent refreshes, no the first run through. Basically session timestamp - has to be a date (exist)
        Dim ts As TimeSpan = Now().Subtract(x)
        'This tests to see if the refresh time is exactly 10 minutes and 0 seconds.
        'I went through the variables that could occur with Rick and he was aware of them.
        'Variables being what could happen if someone clicks a link and there's been exactly 10 minutes since last action
        'Or what happens when the refresh takes longer than it should to load (and the seconds is off).
        If ts.Minutes = 10 And ts.Seconds <= 3 Then
          'We're incrementing an AutomaticRefreshTime variable. This is actually going to count the times that
          'The automatic refresh happened and increment the variable.

          Session.Item("AutomaticRefreshTime") = Session.Item("AutomaticRefreshTime") + 1
          Response.Write("automatic: " & ts.Minutes & " minutes! " & ts.Seconds & " seconds -  now - " & Now() & " timestamp: " & Session.Item("TimeStamp") & " Automatic # of times:" & Session.Item("AutomaticRefreshTime") & "<br />")
          If Session.Item("AutomaticRefreshTime") = 12 Then
            Dim red_url As String = ""
            If Session.Item("isMobile") = True Then
              red_url = ("<script>window.open(""default.aspx?inactive=true&mobile=1"",""_parent"");</script>")
            Else
              red_url = ("<script>window.open(""default.aspx?inactive=true"",""_parent"");</script>")
            End If

            'We're going to make two changes. But only for EVO users:
            If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO Then
              'Only if for some reason their session hasn't already expired
              If Session.Item("localUser").crmSubSubID <> 0 And Session.Item("localUser").crmUserLogin <> "" And Session.Item("localUser").crmSubSeqNo <> 0 Then
                Try
                  'First we're going to write a log about inactivity:
                  Call commonLogFunctions.Log_User_Event_Data("UserInactivityLog", "Session has been logged due to inactivity", Nothing, 0, 0, 0, 0, 0, 0, 0)
                  'Then we're going to log them out of the session.
                  Dim returned As Integer = aclsData_Temp.Update_Evo_Sub_Dates("logout", Now(), HttpContext.Current.Session.Item("localUser").crmSubSubID, HttpContext.Current.Session.Item("localUser").crmUserLogin, HttpContext.Current.Session.Item("localUser").crmSubSeqNo, HttpContext.Current.Session.Item("localUser").crmGUID)
                  If returned = 0 Then 'If that logout doesn't work for whatever reason, we're going to record an error.
                    If aclsData_Temp.class_error <> "" Then
                      Call commonLogFunctions.Log_User_Event_Data("UserError", Replace(Request.Url.AbsolutePath, "/", "") & " : Inactivity Logout was not updated (data exc): " & aclsData_Temp.class_error, Nothing, 0, 0, 0, 0, 0, 0, 0)
                    End If
                  End If
                Catch ex As Exception 'Here we're doing a try catch exception in case there's an exception on the previous two queries.
                  Call commonLogFunctions.Log_User_Event_Data("UserError", Replace(Request.Url.AbsolutePath, "/", "") & " : Inactivity Logout was not updated (exc): " & ex.Message, Nothing, 0, 0, 0, 0, 0, 0, 0)
                End Try
              End If
            End If

            Session.Contents.Clear()
            Session.Abandon()
            Session.Item("Listing") = ""
            Session.Item("Subnode") = ""
            Session.Item("ID") = ""
            If Session.Item("isMobile") = True Then
              Response.Write(red_url)
            Else
              Response.Write(red_url)
            End If

          End If
        Else
          'This means that the page load was NOT automatic, and we don't really care how many times this happened,
          'Basically - someone clicked on a link
          'Response.Write("not automatic: " & ts.Minutes & " minutes! " & ts.Seconds & " seconds -  now - " & Now() & " timestamp: " & Session.Item("TimeStamp") & "<br />")
        End If
      End If

      'I have this code in page is post back because at one time we were going to use 
      'an asp.net timer control. 
      'I haven't taken it out of postback beceause I'm leary of doing anything regarding database work in asp.net without 
      'the use of not .ispostback
      'If you wanted to use it in classic ASP - I found some links that are a workaround for classic asp's 
      'lack of postback here: http://p2p.wrox.com/asp-cdo/19638-asp-page-postback.html
      'and here: http://www.bullschmidt.com/devtip-postbackpage.asp
      If Not Page.IsPostBack Then
        Refresh_Load_Code()
      End If
      ' End If
    Catch ex As Exception
      Dim error_string As String = ""
      error_string = "SessionKeepAlive.ascx.vb - Page Load() - " & ex.Message
      Dim previousException As String = ex.Message.Trim

      Try

        If Session.Item("localUser").crmEvo = True Then
          Call commonLogFunctions.Log_User_Event_Data("UserError", error_string, Nothing, 0, 0, 0, 0, 0, 0, 0)
        Else
          aclsData_Temp.LogError(Application.Item("crmClientSiteData").crmClientHostName, error_string, DateTime.Now.ToString())
        End If

      Catch ex2 As Exception

        commonLogFunctions.forceLogError("ERROR", error_string + " : Previous Exception Thrown[" + previousException.Trim + "] : Exception Thrown[" + ex2.Message.Trim + "]")

      End Try

    End Try
  End Sub

  Public Sub Refresh_Load_Code()
    'Refresh Code that loads on every page load of the iframe. 
    'Here are the notes from Rick that I have:
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '     1.	CRM NEW – USER STATUS FRAMEWORK
    '        a.Current(Approach)
    'i.	We currently use a “session alive” approach in the header of the CRM that talks back to the server to try to keep the users session alive at a timeframe less than their session timeout.
    'ii.	We currently write to the client user table when a user logs in and logs out keeping their latest date of use as up to date as possible.
    'iii.	We currently have no real way of determining if a single user is logged in multiple times.
    '        b.New(Approach)
    'i.	Use our “session alive” approach in the header to do more than just keep the session alive, also use it to write back to the user table with the datetime stamp lettting the system know we are still connected.
    'ii.	In that same code, add code to see if the latest datetime stamp on the user record is newer than my current datetimestamp – if so, this means that this user has logged in again from another session and I must kill my current session.  In this case we should also write a record to the error log indicating that the specific user was logged out since already logged in.
    'iii.	Then each time a user logs in, we check the current users record in the client user table to determine if we think this user is still logged in elsewhere (based on how current the timedate stamp is and whether we know they were logged out. If we determine we think they are still logged in then we can ask the user if they want to login and terminate their previous login or cancel from the login process.
    'c.	Other Notes/Side Effects
    'i.	The CRM currently tracks the total number of connections the best that it can to avoid allowing clients from cheating their logins and reusing existing ones for more users.
    'ii.	If the new approach above works for ensuring that a given user in the database only uses the system once, then we may be able to abandon the overall client concurrent user checks.  The reason is that we already control the amount of users that they can put in their login table (i.e. if they have a 5 user license they can only add 5 users to their user table).  If we combine that with the ability to verify that each of those users can only login once then they will never have over 5 users connected.

    'CHANGES TO THE EXISTING SYSTEM

    '•	New Database Fields
    'o	Add the following fields to the client_user table:
    '	Client Last Logout Date – cliuser_last_logout_date – The date/time of the last user logout.
    '	Client Last Session Date – cliuser_last_session_date – The date/time of the last session refresh.  This will be the exact same as the last login date upon login and the same as the last logout date immediately after logout.
    '	Note that we may want to add a Client Last Login Date – cliuser_last_login_date as well – this would really be a replacement for the current field cliuser_last_login field but would allow us to implement without impacting code for the old field.
    '•	Login
    'o	TIMEOUTPERIOD refers to the amound of time (in seconds) that CRM will use as a means of refreshing the session to keep it active.
    'o	Check client_user table for current user login and retrieve last session date and last logout date.  
    '	If last logout date is = last session date then just login as normal. This means that the user logged out normally to close last session so user is ok to login again.
    '	If last logout date is < last session date or last logout date is NULL then there is a potential that the user is still logged in elsewhere. 
    '•	Check if last session date is within TIMEOUTPERIOD from current date/time then assume that user is still logged in.
    '•	Check if last session date is not within TIMEOUTPERIOD from current date/time then assume that the user closed his browser without logging out and is not currently online so user is ok to login again.
    '	If user is still logged in then:
    '•	Tell the user that they system has detected that the system has detected that they are currently connected to the CRM via another session and ask if they desire to terminate the previous session and login anyways or cancel to leave previous session active.  If they choose to login anyways, then follow rules for user is ok to login (below).  If they choose to cancel the login then write a note to error log “User XXXXX cancelled login due to previous session.” And do not set any variables and take user back to login page.
    '	If user is ok to login then write the current date/time to the last login date and last session date and set last logout date to NULL
    '•	Session Alive
    'o	Session Alive refers to the code that runs in a frame on the header of the site activated via javascript.  The purpose of the code is to keep the session active and keep record that the user is still connected to the system.
    'o	Process as folllows:
    '	Read current client user table last session date
    '	Check server last session date from user table against my current last session date (stored from last session alive run or login).
    '•	If server last session date > my last session date then there has to be another user logged in.  In this case we must store a record to the error log as “User XXXX session canceled due to separate session login.” and clear the users current session so they are forced to logout.  No writing to the user table is necessary.
    '•	If server last session date <= my last session date then get a new session date/time and store it back to the user last session date.
    'o	Logout
    '	On logout set the user table last session date and last logout date to date time now.
    'o	Admin Form – Login Status
    '	Based on the rules above, a user is logged in if their last session date <> last logout date and last session date is less than current TIMEOUTPERIOD
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''End Rick's Notes''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Dim kill_session As Boolean = False
    Dim old_time As New Date
    Dim old_time_database As New Date
    Dim new_time As New Date
    Dim x As New Date
    'Declaring 
    x = Session.Item("TimeStamp")

    'Response.Write("1.) First, Check the latest date time stamp on the user record. If it's newer than my current date time stamp, then the user has logged in <br />")
    'Response.Write("from somewhere else and they have to be logged out. <br />")

    'Grabbing the session date.
    If Session.Item("localUser").crmEvo = True Then
      aTempTable = aclsData_Temp.Compare_Evo_User_Dates()
      'old_time_database = Session.Item("localSubscription").crmSubinst_last_session_date
      'old_time = Session.Item("localSubscription").crmSubinst_last_login_date
    Else
      aTempTable = aclsData_Temp.CRM_Central_Compare_Client_User_Dates(CInt(Session.Item("localUser").crmLocalUserID), CLng(Session.Item("masterRecordID")))
    End If

    If Not IsNothing(aTempTable) Then
      If aTempTable.Rows.Count > 0 Then
        If Not IsDBNull(aTempTable.Rows(0).Item("cliuser_last_session_date")) Then
          old_time_database = aTempTable.Rows(0).Item("cliuser_last_session_date")
        End If
        If Not IsDBNull(aTempTable.Rows(0).Item("cliuser_last_login_date")) Then
          old_time = aTempTable.Rows(0).Item("cliuser_last_login_date")
        End If
      End If
    End If


    ''if the timestamp doesn't exist, use the cliuser_last_login_date
    If Not IsNothing(Session.Item("TimeStamp")) Then
      old_time = Session.Item("TimeStamp")
    End If

    Dim i As Integer = DateTime.Compare(old_time_database, old_time) 'Data compare function compares old times. 
    'If i >= 0 Then          'DT1 is later than DT2.
    'ElseIf i = 0 Then    'DT1 is the same as DT2.
    'ElseIf i < 0 Then    'DT1 is earlier than DT2.
    'End If

    new_time = Now()
    'comparing the session date.
    'if it's newer than current date time stamp, then user has logged in elsewhere.
    If i > 0 Then
      'Error Logged
      Dim red As String = ""
      If Session.Item("isMobile") = True Then
        red = "?mobile=1"
      Else
        red = ""
      End If

      Dim error_string As String = ""

      ''Custom error logging for this type of event. 
      'error_string = "SessionKeepAlive.ascx.vb - Current CRM session has been terminated due to login from another location."

      'If Session.Item("localUser").crmEvo <> True Then
      '    aclsData_Temp.Insert_CRM_Event("Session", Application.Item("crmClientSiteData").crmClientHostName, error_string)
      'End If

      If Session.Item("localUser").crmEvo = True Then
        Call commonLogFunctions.Log_User_Event_Data("UserLogoutForced", "Session has been terminated due to login from another location", Nothing, 0, 0, 0, 0, 0, 0, 0)
      End If

      'Session Cleared and Dies
      Session.Contents.Clear()
      Session.Abandon()
      Session.Item("Listing") = ""
      Session.Item("Subnode") = ""
      Session.Item("ID") = ""
      'Display warning to user. 

      ClientScript.RegisterStartupScript(Me.GetType(), "clear_counter", "<script language='JavaScript'>alert(""Your current session has been terminated due to login from another location."");window.top.location.href = ""default.aspx" & red & """; </script>")

    Else
      'Response.Write("record new time stamp <br />")
      'Response.Write("save timestamp in session variable <br />")

      'Just update with the current times. 
      Session.Item("TimeStamp") = Format(new_time, "yyyy-MM-dd H:mm:ss")
      If Session.Item("localUser").crmEvo = True Then
        Dim strUpdate1 As String = ""
        Dim strDate As System.DateTime
        strDate = FormatDateTime(new_time, vbGeneralDate)

        If Session.Item("localUser").crmSubSubID <> 0 And Session.Item("localUser").crmUserLogin <> "" And Session.Item("localUser").crmSubSeqNo <> 0 Then

          Session.Item("localSubscription").crmSubinst_last_session_date = strDate

          Dim returned As Integer = aclsData_Temp.Update_Evo_Sub_Dates("session", strDate, HttpContext.Current.Session.Item("localUser").crmSubSubID, HttpContext.Current.Session.Item("localUser").crmUserLogin, HttpContext.Current.Session.Item("localUser").crmSubSeqNo, HttpContext.Current.Session.Item("localUser").crmGUID)
          If returned = 1 Then
            'means this was updated.
          Else 'Data layer returned an error. 
            If aclsData_Temp.class_error <> "" Then
              Dim error_string As String = ""
              error_string = "SessionKeepAlive.ascx.vb - Refresh_Load_Code() - " & aclsData_Temp.class_error
              Call commonLogFunctions.Log_User_Event_Data("UserError", error_string, Nothing, 0, 0, 0, 0, 0, 0, 0)
            End If
          End If


        End If
      Else



        Dim returned As Integer = aclsData_Temp.CRM_Central_Update_Client_User_Dates(CInt(Session.Item("localUser").crmLocalUserID), CLng(Session.Item("masterRecordID")), "Y", "session", Session("TimeStamp"))
        If returned = 1 Then
          'means this was updated.
        Else 'Data layer returned an error. 
          If aclsData_Temp.class_error <> "" Then
            Dim error_string As String = ""
            error_string = "SessionKeepAlive.ascx.vb - Refresh_Load_Code()  - " & aclsData_Temp.class_error
            aclsData_Temp.LogError(Application.Item("crmClientSiteData").crmClientHostName, error_string, DateTime.Now.ToString())
          End If
        End If

      End If
    End If

    'Write the header - Refresh the page every 10 minutes. 
    Response.AddHeader("Refresh", 600)
  End Sub
End Class

