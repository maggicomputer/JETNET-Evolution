' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/controls/LogonUser.ascx.vb $
'$$Author: Mike $
'$$Date: 3/27/20 3:30p $
'$$Modtime: 3/27/20 3:22p $
'$$Revision: 15 $
'$$Workfile: LogonUser.ascx.vb $
'
' ********************************************************************************

Partial Public Class _LogonUser
  Inherits System.Web.UI.UserControl

  Public WithEvents UserName As Global.System.Web.UI.WebControls.TextBox
  Public WithEvents Password As Global.System.Web.UI.WebControls.TextBox
  Public WithEvents RememberMe As Global.System.Web.UI.WebControls.CheckBox
  Public WithEvents AutoLogin As Global.System.Web.UI.WebControls.CheckBox

  Public Event UserLogonStatus As EventHandler
  Public Event UserLogonFailed As EventHandler
  Public Event LogonShadowText(ByVal text As String)
  Dim aclsData_Temp As New clsData_Manager_SQL

  Protected Overridable Sub OnUserLogonStatus(ByVal e As EventArgs)
    RaiseEvent UserLogonStatus(Me, e)

  End Sub

  Protected Overridable Sub OnUserLogonFailed(ByVal e As EventArgs)
    RaiseEvent UserLogonFailed(Me, e)
  End Sub

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    Me.UserName.Focus()
    'declare and find label on default.aspx
    'Dim logon_add_info_lbl As Label = FindControl("logon_add_info_lbl")
    If Not IsNothing(logonlbl) Then
      If Session.Item("jetnetWebSiteType") = eWebSiteTypes.TEST Then
        logonlbl.Text = "<em class='tiny'>Test Site</em> - Please Login Below<br />"
      ElseIf Session.Item("jetnetWebSiteType") = eWebSiteTypes.BETA Then
        logonlbl.Text = "<em class='tiny'>Beta Site</em> - Please Login Below<br />"
      ElseIf Session.Item("jetnetWebSiteType") = eWebSiteTypes.LOCAL Then
        logonlbl.Text = "<em class='tiny'>Local Site</em> - Please Login Below<br />"
      Else
        logonlbl.Text = "Please Login Below<br /> "
      End If
    End If

    If Session.Item("localUser").crmEvo = True Then
      AutoLogin.Visible = True
      ForgotPassword.Visible = True
      regexpName.Enabled = True

      If Not String.IsNullOrEmpty(UserName.Text) Then
        emailAddress.Text = UserName.Text
      End If

    End If

  End Sub

  ''' <summary>
  ''' This routine runs when the forgotten password is checked.
  ''' </summary>
  ''' <remarks></remarks>
  Protected Sub ForgotPasswordRoutine()
    'Pseudo Code:
    '1.) First check if you're Evo or CRM.
    'If Evo:
    '2.) Check for email address on a contact.
    '3.) Check for Demo Flag, Marketing Flag
    '4.) If set, do not send password. 
    '5.) Check for 1 row, if 1 row, send email.
    '6.) If more than 1 row, cannot send email.
    '7.) Notify User.
    'If CRM:
    'Nothing for now.

    'Dim sQuery As String = ""
    Dim temporaryTable As New DataTable
    Dim sub_marketing_flag As String = ""
    Dim sublogin_demo_flag As String = ""
    Dim sublogon_password As String = ""
    Dim sublogon_logon As String = ""
    Dim sublogon_subid As Long = 0
    Dim sub_comp_id As Long = 0
    Dim subins_contact_id As Long = 0

    Try

      If ((Not String.IsNullOrEmpty(emailAddress.Text)) And (Not emailAddress.Text.ToLower.Contains("username@email.com"))) Then

        If Session.Item("localUser").crmEvo = True Then

          'Let's clean the USERNAME variable for web input.
          If clsGeneral.clsGeneral.String_Special_BlackList_Words(emailAddress.Text.Trim, True) = True Then

            temporaryTable = aclsData_Temp.ForgottenPassword(emailAddress.Text.Replace("'", "").ToLower)

            If Not IsNothing(temporaryTable) Then

              If temporaryTable.Rows.Count > 0 Then

                If temporaryTable.Rows.Count = 1 Then

                  HttpContext.Current.Session.Item("localUser").crmLocalUserEmailAddress = emailAddress.Text.Replace("'", "")

                  'Setting up all the important flags. 
                  If Not (IsDBNull(temporaryTable.Rows(0).Item("sub_marketing_flag"))) Then
                    sub_marketing_flag = Trim(temporaryTable.Rows(0).Item("sub_marketing_flag").ToString)
                  End If

                  If Not (IsDBNull(temporaryTable.Rows(0).Item("sublogin_demo_flag"))) Then
                    sublogin_demo_flag = Trim(temporaryTable.Rows(0).Item("sublogin_demo_flag").ToString)
                  End If

                  If Not (IsDBNull(temporaryTable.Rows(0).Item("sublogin_password"))) Then
                    sublogon_password = Trim(temporaryTable.Rows(0).Item("sublogin_password").ToString)
                  End If

                  If Not (IsDBNull(temporaryTable.Rows(0).Item("sublogin_sub_id"))) Then
                    sublogon_subid = temporaryTable.Rows(0).Item("sublogin_sub_id")
                  End If

                  If Not (IsDBNull(temporaryTable.Rows(0).Item("sub_comp_id"))) Then
                    sub_comp_id = temporaryTable.Rows(0).Item("sub_comp_id")
                  End If

                  If Not (IsDBNull(temporaryTable.Rows(0).Item("subins_contact_id"))) Then
                    subins_contact_id = temporaryTable.Rows(0).Item("subins_contact_id")
                  End If

                  If Not (IsDBNull(temporaryTable.Rows(0).Item("sublogin_login"))) Then
                    sublogon_logon = temporaryTable.Rows(0).Item("sublogin_login")
                  End If


                  'Checking to see if elligible.
                  If Not (sub_marketing_flag = "Y" Or sublogin_demo_flag = "Y") Then

                    Dim timestamp As String = Now.ToString.Trim

                    Dim tmpGUID As String = Guid.NewGuid().ToString

                    ' guid, timestamp, sub_comp_id, subins_contact_id, sublogin_sub_id, email, sublogon_logon
                    Dim tokenstring As String = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(tmpGUID + "," + timestamp + "," + sub_comp_id.ToString.Trim + "," + subins_contact_id.ToString.Trim + "," + sublogon_subid.ToString.Trim + "," + emailAddress.Text.Trim + "," + sublogon_logon.Trim))

                    ' save the token to the Subscription_Install table 
                    Dim SqlConn As New SqlClient.SqlConnection
                    Dim SqlCommand As New SqlClient.SqlCommand
                    Dim sQuery = New StringBuilder()

                    Try

                      sQuery.Append("UPDATE Subscription_Login SET sublogin_forgot_password_token = '" + tmpGUID + "', sublogin_forgot_password_token_date = '" + timestamp + "'")
                      sQuery.Append(" WHERE sublogin_sub_id = " + sublogon_subid.ToString.Trim + " AND sublogin_login = '" + sublogon_logon.Trim + "'")

                      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />Update Forgot Password Token</b><br />" + sQuery.ToString

                      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
                      SqlConn.Open()
                      SqlCommand.Connection = SqlConn
                      SqlCommand.CommandType = CommandType.Text
                      SqlCommand.CommandTimeout = 60

                      SqlCommand.CommandText = sQuery.ToString
                      SqlCommand.ExecuteNonQuery()

                    Catch ex As Exception

                      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in Update Forgot Password Token" + ex.Message

                    Finally

                      SqlConn.Dispose()
                      SqlConn.Close()
                      SqlConn = Nothing

                      SqlCommand.Dispose()
                      SqlCommand = Nothing
                    End Try

                    Dim HTML_Body As String = ""
                    HTML_Body = "<html><head>"
                    HTML_Body += "</head><body>"
                    HTML_Body += "<img src=""" + clsData_Manager_SQL.get_site_name + "/images/JN_EvolutionMarketplace_Logo2.png""><br /><br />"
                    HTML_Body += "<font face=""Arial"" size=""3"">" + FormatDateTime(Now, vbGeneralDate).ToString + "<br /><br />"
                    HTML_Body += "JETNET LLC<br />Utica, NY  United States<br /><br />"
                    HTML_Body += "Per your request, Please Click the link below to get Forgotten Password.<br /><br />"
                    HTML_Body += "<table border=""1"" cellspacing=""0"" cellpadding=""8"">"
                    HTML_Body += "<tr><th align=""left""><b><a target=""_blank"" href=""" + Application.Item("crmClientSiteData").ClientFullHostName + "forgotPasswordVerify.aspx?forgotToken=" + tokenstring + """>Forgot Password</a></b></th></tr>"
                    HTML_Body += "</table>"
                    HTML_Body += "<br /><br />"
                    HTML_Body += "Click the following link To view the Evolution user guide (PDF) <a target=""_blank"" title=""Evolution User Guide"" href=""" + Application.Item("crmClientSiteData").ClientFullHostName + "help/evolution_user_guide.pdf"">User Guide</a><br /><br />"
                    HTML_Body += "If the technical staff of JETNET can assist you in any way, please do not hesitate to call 800-553-8638, Ext 1, and we will be happy to assist you.<br /><br />Best regards<br /><br />"
                    HTML_Body += "<span style=""font-size:10.5pt; font-family Arial; color:#616E7D"">"
                    HTML_Body += "<em><b>Customer Technical Support</b></em><br />"
                    HTML_Body += "<a href=""mailto:customerservice@jetnet.com?Subject=Customer Technical Support"">customerservice@jetnet.com</a><br />"
                    HTML_Body += "<em><b>JETNET LLC</b></em><br />"
                    HTML_Body += "<em>Worldwide leader in aviation market intelligence.</em><br />"
                    HTML_Body += "101 First St. | Utica, NY 13501 USA |<br />"
                    HTML_Body += "Main Office: 800.553.8638 >> N.Y. Office: 315.797.4420<br />"
                    HTML_Body += "<span style=""font-size:9.0pt; color:#616E7D"">"
                    HTML_Body += "<a target=""_blank"" href=""https://www.jetnet.com/"" title=""http://www.jetnet.com/"">website</a> |"
                    HTML_Body += "<a target=""_blank"" href=""http://www.jetstreamblog.com/"" title=""http://www.jetstreamblog.com/"">blog</a> |"
                    HTML_Body += "<a target=""_blank"" href=""http://www.twitter.com/jetnetllc"" title=""http://www.twitter.com/jetnetllc"">twitter</a> |"
                    HTML_Body += "<a target=""_blank"" href=""http://www.jetnetglobal.com/"" title=""http://www.jetnetglobal.com/"">ABI</a>"
                    HTML_Body += "</span></span>"
                    HTML_Body += "</body></html>"

                    aclsData_Temp.InsertMailQueue(sub_comp_id, subins_contact_id, sublogon_subid, emailAddress.Text.Trim, HTML_Body, True)

                    Me.FailureText.Text = "Forgot Password has been sent to " + emailAddress.Text.Trim + ". Please check your email in a few minutes."

                    commonLogFunctions.Log_User_Event_Data("UserPreferences", "User Requested Forgotten Password", Nothing, , , , CLng(HttpContext.Current.Session.Item("localUser").crmUserCompanyID.ToString), CLng(HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString))

                  Else
                    Me.FailureText.Text = "** Not available under Marketing/Demo account **"
                  End If
                Else
                  'multiple accounts.  
                  Me.FailureText.Text = "This email address results in multiple installs. The forgot password feature does not support this setup."
                End If
              Else
                'username doesn't exist.
                Me.FailureText.Text = "Username does not exist. " + emailAddress.Text.Trim
              End If

            End If

          End If

        End If

      ElseIf Session.Item("localUser").crmEvo = False Then
        'If CRM:
        'Nothing for now.
      Else
        forgotPasswordPopUp.CssClass = "modalPopup"

        btnOk.Focus()

        MPE2.Show() 'this shows the ajax modal popupcontrol and kills command connection

      End If

    Catch ex As Exception
      Me.FailureText.Text = "LogonUser.ascx > LoginButton_Click Error : " & ex.Message
    End Try

  End Sub

  Protected Sub Remember_Cookies()

    If RememberMe.Checked Then
      Response.Cookies.Item("crmUserName").Item(Session.Item("localUser").crmSubSubID.ToString) = Session.Item("localUser").crmLocalUserName
      Response.Cookies.Item("crmUserName").Expires = DateTime.Now.AddDays(300)

      Response.Cookies.Item("crmUserPassword").Item(Session.Item("localUser").crmSubSubID.ToString) = Session.Item("localUser").EncodeBase64(Session.Item("localUser").crmLocalUserPswd)
      Response.Cookies.Item("crmUserPassword").Expires = DateTime.Now.AddDays(300)

      If AutoLogin.Checked Then
        Response.Cookies.Item(Application.Item("crmClientSiteData").AutoLogonCookie).Item(Session.Item("localUser").crmSubSubID.ToString) = True
        Response.Cookies.Item(Application.Item("crmClientSiteData").AutoLogonCookie).Expires = DateTime.Now.AddDays(300)
      Else
        Response.Cookies.Item(Application.Item("crmClientSiteData").AutoLogonCookie).Item(Session.Item("localUser").crmSubSubID.ToString) = False
        Response.Cookies.Item(Application.Item("crmClientSiteData").AutoLogonCookie).Expires = DateTime.Now.AddDays(0)
      End If

    ElseIf AutoLogin.Checked Then

      Response.Cookies.Item("crmUserName").Item(Session.Item("localUser").crmSubSubID.ToString) = Session.Item("localUser").crmLocalUserName
      Response.Cookies.Item("crmUserName").Expires = DateTime.Now.AddDays(300)

      Response.Cookies.Item("crmUserPassword").Item(Session.Item("localUser").crmSubSubID.ToString) = Session.Item("localUser").EncodeBase64(Session.Item("localUser").crmLocalUserPswd)
      Response.Cookies.Item("crmUserPassword").Expires = DateTime.Now.AddDays(300)

      Response.Cookies.Item(Application.Item("crmClientSiteData").AutoLogonCookie).Item(Session.Item("localUser").crmSubSubID.ToString) = True
      Response.Cookies.Item(Application.Item("crmClientSiteData").AutoLogonCookie).Expires = DateTime.Now.AddDays(300)

    Else

      Response.Cookies.Item("crmUserName").Item(Session.Item("localUser").crmSubSubID.ToString) = ""
      Response.Cookies.Item("crmUserName").Expires = DateTime.Now.AddDays(0)

      Response.Cookies.Item("crmUserPassword").Item(Session.Item("localUser").crmSubSubID.ToString) = ""
      Response.Cookies.Item("crmUserPassword").Expires = DateTime.Now.AddDays(0)

      Response.Cookies.Item(Application.Item("crmClientSiteData").AutoLogonCookie).Item(Session.Item("localUser").crmSubSubID.ToString) = False
      Response.Cookies.Item(Application.Item("crmClientSiteData").AutoLogonCookie).Expires = DateTime.Now.AddDays(0)
    End If

  End Sub

  Protected Sub LoginButton_Click(ByVal sender As Object, ByVal e As EventArgs) 'Handles LoginButton.Click
    Try
      If Page.IsValid Then

        Trace.Write("Start LoginButton_Click LogonUser.ascx" + Now.ToString)

        If Session.Item("localUser").crmEvo Then
          'This is going to check to see if forgotten password is checked before login is performed.

          If Logon_Client() Then
            Remember_Cookies()
            Call OnUserLogonStatus(New EventArgs())
          Else



            'fail because of spam catch
            If Me.FailureText.Text = "Please Contact Our Offices Regarding Your Subscription" Then
              Response.Redirect("errorPages/scriptError.aspx")
            End If

            Call OnUserLogonFailed(New EventArgs())

          End If


        ElseIf Not Session.Item("localUser").crmEvo Then

          If Logon_Client() Then

            Session.Item("localUserID") = Session.Item("localUser").crmLocalUserID

            Session.Item("localSubscription").crmSubStatusCode = eObjStatusCode.SUCCESS
            Session.Item("localUser").crmUserStatusCode = eObjStatusCode.SUCCESS

            Remember_Cookies()

            Call OnUserLogonStatus(New EventArgs())

          Else

            Response.Cookies.Item("crmUserName").Item(Session.Item("localUser").crmSubSubID.ToString) = ""
            Response.Cookies.Item("crmUserName").Expires = DateTime.Now.AddDays(-300)

            Response.Cookies.Item("crmUserPassword").Item(Session.Item("localUser").crmSubSubID.ToString) = ""
            Response.Cookies.Item("crmUserPassword").Expires = DateTime.Now.AddDays(-300)

            Response.Cookies.Item(Application.Item("crmClientSiteData").AutoLogonCookie).Item(Session.Item("localUser").crmSubSubID.ToString) = ""
            Response.Cookies.Item(Application.Item("crmClientSiteData").AutoLogonCookie).Expires = DateTime.Now.AddDays(-300)

            If Me.FailureText.Text = "" Then
              Me.FailureText.Text = "Logon Failed. Please try Again!"
            End If

            'fail because of spam catch
            If Me.FailureText.Text = "Please Contact Our Offices Regarding Your Subscription" Then
              Response.Redirect("errorPages/scriptError.aspx")
            End If

            Session.Item("localSubscription").crmSubStatusCode = eObjStatusCode.FAILURE
            Session.Item("localUser").crmUserStatusCode = eObjStatusCode.FAILURE

            Call OnUserLogonFailed(New EventArgs())

          End If
        End If
      End If

      Trace.Write("End LoginButton_Click LogonUser.ascx" + Now.ToString)

    Catch ex As Exception
      Me.FailureText.Text = "LogonUser.ascx > LoginButton_Click Error : " & ex.Message

    End Try

  End Sub

  Public Sub AutoLoginClick()
    'Runs the validation group for the login control manually because if you force a button click, it does not
    'do it automatically. This ensures the page does not error because inside of LoginButton_Click, there is a test
    'for Page.IsValid. Without running this, it would return an error.
    Page.Validate("Login1")

    Me.Password.Text = Me.Password.Attributes.Item("value")
    LoginButton_Click(Nothing, EventArgs.Empty)

  End Sub

  Public Sub AutoLoginFromOtherApplication()
    Dim PasswordDecoded As String = ""
    Dim UsernameDecoded As String = ""
    If Not IsNothing(Trim(Request("1"))) And Not IsNothing(Trim(Request("2"))) Then
      If Not String.IsNullOrEmpty(Trim(Request("1"))) And Not String.IsNullOrEmpty(Trim(Request("2"))) Then
        PasswordDecoded = Trim(Request("1"))
        UsernameDecoded = Trim(Request("2"))

        PasswordDecoded = clsGeneral.clsGeneral.DecodeBase64(PasswordDecoded)
        UsernameDecoded = clsGeneral.clsGeneral.DecodeBase64(UsernameDecoded)

        Me.UserName.Text = UsernameDecoded
        Me.Password.Attributes.Item("value") = PasswordDecoded
        Me.Password.Text = PasswordDecoded

        Page.Validate("Login1")
        LoginButton_Click(Nothing, EventArgs.Empty)
      End If
    End If
  End Sub

  Private Function Logon_Client() As Boolean

    Dim sQuery As String = ""
    Dim answer As Boolean = False
    Dim sub_yacht_flag As Boolean = False
    Dim sub_business_aircraft_flag As Boolean = False
    Dim sub_helicopters_flag As Boolean = False
    Dim sub_commerical_flag As Boolean = False

    Dim sub_marketing_flag As Boolean = False
    Dim sub_demo_flag As Boolean = False
    Dim sub_admin_flag As Boolean = False

    Const JETNET_DOMAIN As String = "JETNET.COM"
    Const MVINTECH_DOMAIN As String = "MVINTECH.COM"

    Try

      ' log local crm user if client all ready registered

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br/>logon get user subscription info start : " + Now.ToString + "<br />"
      Trace.Write("Start Logon_Client LogonUser.ascx" + Now.ToString)

      If Not (String.IsNullOrEmpty(Me.UserName.Text.Trim)) And Not (String.IsNullOrEmpty(Me.Password.Text.Trim)) Then



        If clsGeneral.clsGeneral.String_Special_BlackList_Words(UserName.Text.Trim, True) = True And clsGeneral.clsGeneral.String_Special_BlackList_Words(Password.Text.Trim, False) = True Then
          Session.Item("localUser").crmLocalUserName = Replace(UserName.Text.Trim, "'", "")
          Session.Item("localUser").crmLocalUserPswd = Replace(Password.Text.Trim, "'", "")

          'this means that the username and password have gotten past the bad words check + single quotes removed

          '' check the client local database for a list of users that match the username and password

          If Session.Item("localUser").crmEvo Then

            Trace.Write("Start Logon_Client Evo Section LogonUser.ascx" + Now.ToString)

            Dim atemptable As New DataTable
            Dim ContinueLogin As Boolean = True 'we're going to default the continue login to true. This is a variable that's set to stop the login process - only if they're demo/marketing/blank install date.

            Try

              atemptable = aclsData_Temp.EvoLoginVerificationCheck(Session.Item("localUser").crmLocalUserName.ToString.Trim, Session.Item("localUser").crmLocalUserPswd.ToString.Trim, IIf(Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT, True, False), Session.Item("localUser").crmUser_API_Login)


              Dim product_code As String = ""

              If atemptable.Rows.Count = 0 Then
                Me.FailureText.Text = "Username and/or Password Combination Doesn't Exist. Please try again."

                HttpContext.Current.Session.Item("localUser").crmLocalUserEmailAddress = Replace(UserName.Text.Trim, "'", "")
                HttpContext.Current.Session.Item("localUser").crmLocalUserPswd = Replace(Password.Text.Trim, "'", "")

                commonLogFunctions.forceLogError("UserError", "User Failed Login")

                'This fails with this email address, but I am saving it to session to log it. After that, it can just clear
                'They're going back to the homepage anyhow. If it doesn't clear, it still doesn't matter. They will lose their session and have to start again.
                HttpContext.Current.Session.Item("localUser").crmLocalUserEmailAddress = ""
                HttpContext.Current.Session.Item("localUser").crmLocalUserPswd = ""

                Call OnUserLogonFailed(New EventArgs())

                Return False
              ElseIf atemptable.Rows.Count > 1 Then
                Me.FailureText.Text = "Login is tied to multiple subscriptions. Please contact JETNET for more information."
                Call OnUserLogonFailed(New EventArgs())
                Return False
              End If

              If Not IsNothing(atemptable.Rows(0).Item("sub_yacht_flag")) Then
                If Not IsDBNull(atemptable.Rows(0).Item("sub_yacht_flag")) Then
                  If Not String.IsNullOrEmpty(atemptable.Rows(0).Item("sub_yacht_flag").ToString) Then
                    sub_yacht_flag = IIf(atemptable.Rows(0).Item("sub_yacht_flag").ToString.ToUpper.Trim.Contains("Y"), True, False)
                  End If
                End If
              End If

              If Not IsNothing(atemptable.Rows(0).Item("sub_business_aircraft_flag")) Then
                If Not IsDBNull(atemptable.Rows(0).Item("sub_business_aircraft_flag")) Then
                  If Not String.IsNullOrEmpty(atemptable.Rows(0).Item("sub_business_aircraft_flag").ToString) Then
                    sub_business_aircraft_flag = IIf(atemptable.Rows(0).Item("sub_business_aircraft_flag").ToString.ToUpper.Trim.Contains("Y"), True, False)
                  End If
                End If
              End If

              If Not IsNothing(atemptable.Rows(0).Item("sub_helicopters_flag")) Then
                If Not IsDBNull(atemptable.Rows(0).Item("sub_helicopters_flag")) Then
                  If Not String.IsNullOrEmpty(atemptable.Rows(0).Item("sub_helicopters_flag").ToString) Then
                    sub_helicopters_flag = IIf(atemptable.Rows(0).Item("sub_helicopters_flag").ToString.ToUpper.Trim.Contains("Y"), True, False)
                  End If
                End If
              End If

              If Not IsNothing(atemptable.Rows(0).Item("sub_commerical_flag")) Then
                If Not IsDBNull(atemptable.Rows(0).Item("sub_commerical_flag")) Then
                  If Not String.IsNullOrEmpty(atemptable.Rows(0).Item("sub_commerical_flag").ToString) Then
                    sub_commerical_flag = IIf(atemptable.Rows(0).Item("sub_commerical_flag").ToString.ToUpper.Trim.Contains("Y"), True, False)
                  End If
                End If
              End If

              If Not IsNothing(atemptable.Rows(0).Item("sub_marketing_flag")) Then
                If Not IsDBNull(atemptable.Rows(0).Item("sub_marketing_flag")) Then
                  If Not String.IsNullOrEmpty(atemptable.Rows(0).Item("sub_marketing_flag").ToString) Then
                    sub_marketing_flag = IIf(atemptable.Rows(0).Item("sub_marketing_flag").ToString.ToUpper.Trim.Contains("Y"), True, False)
                  End If
                End If
              End If

              If Not IsNothing(atemptable.Rows(0).Item("sublogin_demo_flag")) Then
                If Not IsDBNull(atemptable.Rows(0).Item("sublogin_demo_flag")) Then
                  If Not String.IsNullOrEmpty(atemptable.Rows(0).Item("sublogin_demo_flag").ToString) Then
                    sub_demo_flag = IIf(atemptable.Rows(0).Item("sublogin_demo_flag").ToString.ToUpper.Trim.Contains("Y"), True, False)
                  End If
                End If
              End If

              If Not IsNothing(atemptable.Rows(0).Item("subins_admin_flag")) Then
                If Not IsDBNull(atemptable.Rows(0).Item("subins_admin_flag")) Then
                  If Not String.IsNullOrEmpty(atemptable.Rows(0).Item("subins_admin_flag").ToString) Then
                    sub_admin_flag = IIf(atemptable.Rows(0).Item("subins_admin_flag").ToString.ToUpper.Trim.Contains("Y"), True, False)
                  End If
                End If
              End If

              If CType(Session.Item("jetnetWebHostType"), crmWebClient.eWebHostTypes) = eWebHostTypes.ADMIN Then

                ' is this a "jetnet" or "mvintech" domain user
                If Session.Item("localUser").crmLocalUserName.ToString.ToUpper.Contains(JETNET_DOMAIN) Or Session.Item("localUser").crmLocalUserName.ToString.ToUpper.Contains(MVINTECH_DOMAIN) Then

                  If Not sub_admin_flag Or sub_marketing_flag Or sub_demo_flag Then

                    Me.FailureText.Text = "Subscription logon is NOT VALID for www.evolutionadmin.com. Please contact JETNET for more information."
                    Call OnUserLogonFailed(New EventArgs())
                    Return False

                  End If

                Else

                  Me.FailureText.Text = "Subscription logon is NOT VALID for www.evolutionadmin.com. Please contact JETNET for more information."
                  Call OnUserLogonFailed(New EventArgs())
                  Return False

                End If

              End If

              If CType(Session.Item("jetnetWebHostType"), crmWebClient.eWebHostTypes) = eWebHostTypes.EVOLUTION And Not (sub_business_aircraft_flag Or sub_helicopters_flag Or sub_commerical_flag) Then

                Me.FailureText.Text = "Subscription logon is only for www.jetnetevolution.com. Please contact JETNET for more information."
                Call OnUserLogonFailed(New EventArgs())
                Return False

              End If

              If CType(Session.Item("jetnetWebHostType"), crmWebClient.eWebHostTypes) = eWebHostTypes.YACHT And Not sub_yacht_flag Then

                Me.FailureText.Text = "Subscription logon is only for www.yacht-spotonline.com. Please contact JETNET for more information."
                Call OnUserLogonFailed(New EventArgs())
                Return False

              End If


              ''Added a check to determine if the account is an expired demo.
              'if sub_marketing_flag =’Y’ AND ((Clng(datediff("d", subins_install_date, now)) > sub_nbr_days_expire) AND (sub_nbr_days_expire <> 0))) then
              If Not IsDBNull(atemptable.Rows(0).Item("sub_marketing_flag")) Then
                If UCase(atemptable.Rows(0).Item("sub_marketing_flag").ToString) = "Y" Then 'This is a marketing flag = Yes.
                  If Not IsDBNull(atemptable.Rows(0).Item("subins_install_date")) Then 'The date is not null
                    If Not IsDBNull(atemptable.Rows(0).Item("sub_nbr_days_expire")) Then 'If the nbr of days expire isn't null.
                      If (CLng(DateDiff("d", atemptable.Rows(0).Item("subins_install_date"), Now) > CLng(atemptable.Rows(0).Item("sub_nbr_days_expire")))) And (CLng(atemptable.Rows(0).Item("sub_nbr_days_expire")) <> 0) Then 'testing the date installed against the number of days for expiration.
                        Me.FailureText.Text = "This Login is expired. Please contact JETNET for more information."
                        Call OnUserLogonFailed(New EventArgs())
                        Return False
                      End If
                    End If
                  Else
                    'We need to check if this is a demo account or a marketing account. If it is, we need to popup something first.
                    'Let's default this text first. It's the basic text. Down below, an if then statement will run, but only if we're aerodex.
                    demoWarningTextSwap.Text = " JETNET's EVOLUTION service is offered only to bona fide aircraft"
                    demoWarningTextSwap.Text += " sales professionals, financiers and lessors who currently and routinely engage"
                    demoWarningTextSwap.Text += " in the business of selling, buying or brokering aircraft, and institutions"
                    demoWarningTextSwap.Text += " engaged in the financing or leasing of aircraft. In proceeding with this"
                    demoWarningTextSwap.Text += " demonstration you represent to JETNET LLC that at all times during the"
                    demoWarningTextSwap.Text += " trial period you and your organization are eligible for the"
                    demoWarningTextSwap.Text += " JETNET's EVOLUTION service and you acknowledge that JETNET LLC has"
                    demoWarningTextSwap.Text += " the right, at any time, to deny service. You agree that this demonstration is"
                    demoWarningTextSwap.Text += " to be used for EVALUATION PURPOSES ONLY! <br /><br />"
                    demoWarningTextSwap.Text += " You agree that you shall not use information revealed in the demonstration to in any way facilitate the"
                    demoWarningTextSwap.Text += " transaction or lease of aircraft. You agree that you will not utilize the"
                    demoWarningTextSwap.Text += " demonstration for purposes of gaining intelligence to facilitate a future"
                    demoWarningTextSwap.Text += " transaction or lease of aircraft. You agree that you will not in any way"
                    demoWarningTextSwap.Text += " collect, record, export or sell information revealed in the demonstration."

                    If Not IsDBNull(atemptable.Rows(0).Item("sub_aerodex_flag")) Then
                      If UCase(atemptable.Rows(0).Item("sub_aerodex_flag")) = "Y" Then
                        demoWarningTextSwap.Text = " JETNET's AERODEX service is offered only to bona fide aircraft"
                        demoWarningTextSwap.Text += " product and service professionals. In proceeding with this demonstration you"
                        demoWarningTextSwap.Text += " represent to JETNET LLC that at all times during the trial period you"
                        demoWarningTextSwap.Text += " and your organization are eligible for JETNET's AERODEX service"
                        demoWarningTextSwap.Text += " and you acknowledge that JETNET LLC has the right, at any time, to deny"
                        demoWarningTextSwap.Text += " service. You agree that this demonstration is to be used for EVALUATION PURPOSES ONLY!<br /><br />"
                        demoWarningTextSwap.Text += " You agree that you shall not use information revealed in"
                        demoWarningTextSwap.Text += " the demonstration to in any way facilitate the transaction or lease of aircraft"
                        demoWarningTextSwap.Text += " or to promote your own product or service. You agree that you will not utilize"
                        demoWarningTextSwap.Text += " the demonstration for purposes of gaining intelligence to facilitate a future"
                        demoWarningTextSwap.Text += " transaction or lease of aircraft. You agree that you will not in any way"
                        demoWarningTextSwap.Text += " collect, record, export or sell information revealed in the demonstration."
                      End If
                    End If

                    If Not IsNothing(atemptable.Rows(0).Item("sub_yacht_flag")) Then
                      If Not IsDBNull(atemptable.Rows(0).Item("sub_yacht_flag")) Then
                        If UCase(atemptable.Rows(0).Item("sub_yacht_flag")) = "Y" Then
                          If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT Then
                            demoWarningTextSwap.Text = " YachtSpot, a product of JETNET LLC, is a service is offered only to bona fide"
                            demoWarningTextSwap.Text += " professionals who engage in the business of buying, selling, brokering, chartering yachts,"
                            demoWarningTextSwap.Text += " and those institutions engaged in financing and leasing as well as additional product and"
                            demoWarningTextSwap.Text += " service providers. "
                            demoWarningTextSwap.Text += " In proceeding with this demonstration you represent to JETNET LLC that at all times "
                            demoWarningTextSwap.Text += " during the trial period you and your organization are eligible for the YachSpot service."
                            demoWarningTextSwap.Text += " You agree that this demonstration is to be used for EVALUATION PURPOSES ONLY!"
                            demoWarningTextSwap.Text += " You agree that you will not in any way"
                            demoWarningTextSwap.Text += " collect, record, export or sell information revealed in the demonstration."
                          End If
                        End If
                      End If
                    End If
                    ContinueLogin = False

                  End If
                End If
              End If

              answer = clsGeneral.clsGeneral.Reload_Evolution_Subscription(aclsData_Temp, atemptable)

              If (Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER) Or (Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE) Then
                '•	Home_user_id – initials of user used to put on all types of records indicating who Is responsible. 
                '•	Home_account_id – 4 char code of the account rep
                '•	Home_user_type – String indicating if user Is a research manager, administrator, etc.
                'Query to Get Homebase User Data
                'To get this select top 1 from the Homebase [User] table using basic query below noting that it will need a slightly different syntax when launched from Evolution admin with the purple added. 
                'From Homebase.com
                'Select top 1 user_id As home_user_id, user_default_account_id As home_account_id, user_type As home_user_type
                'From [User] Where user_email_address = 'laurie@jetnet.com' 
                'From EvolutionAdmin.com
                'Select top 1 user_id As home_user_id, user_default_account_id As home_account_id, user_type As home_user_type
                'From [Homebase].[jetnet_ra].[dbo].[User] Where user_email_address = 'laurie@jetnet.com'
                Dim HomeBaseInfo As DataTable = getHomeBaseAdminDataForClass(Session.Item("localUser").crmLocalUserName)
                If Not IsNothing(HomeBaseInfo) Then
                  If HomeBaseInfo.Rows.Count > 0 Then
                    If Not IsDBNull(HomeBaseInfo.Rows(0).Item("home_user_id")) Then
                      Session.Item("homebaseUserClass").home_user_id = HomeBaseInfo.Rows(0).Item("home_user_id")
                    End If
                    If Not IsDBNull(HomeBaseInfo.Rows(0).Item("home_account_id")) Then
                      Session.Item("homebaseUserClass").home_account_id = HomeBaseInfo.Rows(0).Item("home_account_id")
                    End If
                    If Not IsDBNull(HomeBaseInfo.Rows(0).Item("home_user_type")) Then
                      Session.Item("homebaseUserClass").home_user_type = HomeBaseInfo.Rows(0).Item("home_user_type")
                    End If
                  End If
                End If

              End If

              If ContinueLogin = False Then
                If Session.Item("isMobile") = False Then
                  DemoWarningText.CssClass = "modalPopup"
                Else
                  DemoWarningText.CssClass = "modalDemoPopup"
                  PopupDemoWarning.RepositionMode = AjaxControlToolkit.ModalPopupRepositionMode.RepositionOnWindowResize
                End If

                PopupDemoWarning.Show()
                answer = False
              Else

                'If the answer is returned and subscription is filled up, then the user is logged in.
                If answer = True Then
                  Session.Item("crmUserLogon") = True
                End If

                'Adding a check to see if the install date is filled in
                If atemptable.Rows.Count = 1 Then
                  If IsDBNull(atemptable.Rows(0).Item("subins_install_date")) Then
                    'This needs to be set to not be null anymore.
                    If Not IsDBNull(atemptable.Rows(0).Item("sublogin_sub_id")) Then
                      If Not IsDBNull(atemptable.Rows(0).Item("subins_login")) Then
                        If Not IsDBNull(atemptable.Rows(0).Item("subins_seq_no")) Then
                          aclsData_Temp.Update_Evo_Sub_Install_Date(CLng(atemptable.Rows(0).Item("sublogin_sub_id")), atemptable.Rows(0).Item("subins_login").ToString, CLng(atemptable.Rows(0).Item("subins_seq_no")))
                        End If
                      End If
                    End If
                  End If
                End If

              End If
              Return answer

            Catch ex As Exception
              Return False
              Session.Item("crmUserLogon") = False
            End Try

            Trace.Write("End Logon_Client Evo Section LogonUser.ascx" + Now.ToString)

          Else 'not evo

            Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
            Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
            Dim MySqlReader As MySql.Data.MySqlClient.MySqlDataReader
            Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing

            MySqlConn.ConnectionString = Application.Item("crmClientDatabase")

            Try

              MySqlConn.Open()

              MySqlCommand.Connection = MySqlConn
              MySqlCommand.CommandType = CommandType.Text
              MySqlCommand.CommandTimeout = 60
              'added Amanda: and ((cliuser_admin_flag = 'D' and cliuser_end_date > NOW()) or (cliuser_admin_flag <> 'D')) 10/19/2011 for  demo account verification
              sQuery = "SELECT * FROM Client_User WHERE cliuser_active_flag = 'Y' AND cliuser_login = '" + Session.Item("localUser").crmLocalUserName.ToString.Trim + "' and cliuser_password = '" + Session.Item("localUser").crmLocalUserPswd.ToString.Trim + "' and ((cliuser_admin_flag = 'D' and cliuser_end_date > NOW()) or (cliuser_admin_flag <> 'D')) LIMIT 1"
              HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>This is on LogonUser.ascx.vb, Logon_Client()</b><br />" & sQuery

              MySqlCommand.CommandText = sQuery
              MySqlReader = MySqlCommand.ExecuteReader()

              If MySqlReader.HasRows Then

                Do While MySqlReader.Read()

                  If Not (IsDBNull(MySqlReader("cliuser_id"))) Then
                    Session.Item("localUser").crmLocalUserID = MySqlReader.Item("cliuser_id")
                  End If

                  If Not (IsDBNull(MySqlReader("cliuser_autolog_flag"))) Then
                    Session.Item("localUser").crmUser_Autolog_Flag = IIf(MySqlReader.Item("cliuser_autolog_flag") = "Y", True, False)
                  End If

                  If Not IsDBNull(MySqlReader("cliuser_spi_flag")) Then
                    'storing separate value for debugging purposes:
                    Session.Item("localUser").crmUser_SPI_Flag = IIf(MySqlReader.Item("cliuser_spi_flag") = "Y", True, False)
                    'Only bothering if this is set as true.
                    If Session.Item("localSubscription").crmSalesPriceIndex_Flag Then
                      Session.Item("localSubscription").crmSalesPriceIndex_Flag = IIf(MySqlReader.Item("cliuser_spi_flag") = "Y", True, False)
                    End If
                  End If

                  If Not (IsDBNull(MySqlReader("cliuser_first_name"))) Then
                    Session.Item("localUser").crmLocalUserFirstName = MySqlReader.Item("cliuser_first_name")
                  End If

                  If Not (IsDBNull(MySqlReader("cliuser_last_name"))) Then
                    Session.Item("localUser").crmLocalUserLastName = MySqlReader.Item("cliuser_last_name")
                  End If

                  If Not (IsDBNull(MySqlReader("cliuser_recs_per_page"))) Then
                    If IsNumeric(MySqlReader("cliuser_recs_per_page")) Then
                      Session.Item("localUser").crmUserRecsPerPage = MySqlReader("cliuser_recs_per_page")
                    End If
                  End If

                  If Not IsDBNull(MySqlReader("cliuser_default_models")) Then
                    Session.Item("localUser").crmUserDefaultModels = MySqlReader.Item("cliuser_default_models")

                    Dim SplitModelsForJetnetID As Array = Split(MySqlReader.Item("cliuser_default_models"), ",")
                    For counting As Integer = 0 To UBound(SplitModelsForJetnetID)
                      Dim splitFinalModelID As Array = Split(SplitModelsForJetnetID(counting), "|")
                      If UBound(splitFinalModelID) > 0 Then
                        If Session.Item("localUser").crmSelectedModels <> "" Then
                          Session.Item("localUser").crmSelectedModels += ","
                        End If
                        Session.Item("localUser").crmSelectedModels += splitFinalModelID(0)
                      End If
                    Next
                  End If

                  If Not (IsDBNull(MySqlReader("cliuser_timezone"))) Then
                    If IsNumeric(MySqlReader("cliuser_timezone")) Then
                      Session("timezone") = MySqlReader("cliuser_timezone")
                    End If
                  End If

                  If Not (IsDBNull(MySqlReader("cliuser_aircraft_relationship"))) Then
                    Session.Item("localUser").crmUserAircraftRelationship = MySqlReader("cliuser_aircraft_relationship")
                  End If


                  If Not (IsDBNull(MySqlReader("cliuser_admin_flag"))) Then

                    Dim bisAdminUser As String = UCase(MySqlReader.Item("cliuser_admin_flag").ToString)

                    If bisAdminUser = "Y" Then
                      Session.Item("localUser").crmUserType = eUserTypes.ADMINISTRATOR
                      'added this for demo ACCOUNTS 10/19/2011 Amanda
                    ElseIf bisAdminUser = "D" Then
                      Session.Item("localUser").crmUserType = eUserTypes.GUEST
                    ElseIf bisAdminUser = "R" Then 'research only
                      Session.Item("localUser").crmUserType = eUserTypes.RESEARCH
                    ElseIf bisAdminUser = "M" Then 'My personal notes only
                      Session.Item("localUser").crmUserType = eUserTypes.MyNotesOnly
                    Else
                      Session.Item("localUser").crmUserType = eUserTypes.USER
                    End If

                  End If

                  If Not (IsDBNull(MySqlReader("cliuser_email_address"))) Then
                    Session.Item("localUser").crmLocalUserEmailAddress = MySqlReader.Item("cliuser_email_address")
                  End If

                  If Not (IsDBNull(MySqlReader("cliuser_login"))) Then
                    Session.Item("localUser").crmUserLogin = MySqlReader.Item("cliuser_login")
                  End If

                  Session.Item("localUser").crmAllowExport_Flag = True


                  Session.Item("localUser").crmUserTemporaryFilePrefix = Session.Item("masterRecordID").ToString & "_" & Session.Item("localUser").crmLocalUserID.ToString & "_"
                Loop ' while MySqlReader.HasRows

                MySqlReader.Close()

              Else

                MySqlConn.Close()
                MySqlCommand.Dispose()
                MySqlConn.Dispose()

                Me.FailureText.Text = "User Doesn't exist please check logon and password"
                Return False

              End If 'MySqlReader.HasRows 

              MySqlReader.Dispose()

            Catch MySqlException

              MySqlConn.Dispose()
              MySqlCommand.Dispose()

              Me.FailureText.Text = "LogonUser.ascx > Logon_Client Error: " + MySqlException.Message
              Return False

            Finally

              MySqlConn.Close()
              MySqlCommand.Dispose()
              MySqlConn.Dispose()

            End Try


            sQuery = Nothing


          End If 'if not evo
        Else

          Me.FailureText.Text = "Please Contact Our Offices Regarding Your Subscription"
          Return False
          '  Response.Redirect("errorPages/genericError.aspx")
        End If
      Else
        Me.FailureText.Text = "Please enter a password"
        Return False
      End If 'if not blank username password
    Catch ex As Exception
      Me.FailureText.Text = "LogonUser.ascx > Logon_Client Error: " + ex.Message
      Return False
    End Try

    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br/>logon get user subscription info end : " + Now.ToString + "<br />"
    Trace.Write("End Logon_Client LogonUser.ascx" + Now.ToString)

    Return True

  End Function

  Private Sub OkButtonDemo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles OkButtonDemo.Click
    'They clicked okay. 
    'This means we go ahead and log them in.
    Session.Item("crmUserLogon") = True

    'This needs to be set to not be null anymore.

    aclsData_Temp.Update_Evo_Sub_Install_Date(CLng(Session.Item("localUser").crmSubSubID), Session.Item("localUser").crmUserLogin.ToString, CLng(Session.Item("localUser").crmSubSeqNo))

    'This means that they said okay, carry on with the login process.
    Remember_Cookies()
    Call OnUserLogonStatus(New EventArgs())
  End Sub

  Private Sub CancelButtonDemo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CancelButtonDemo.Click

    Me.FailureText.Text = "You must agree in order to continue."
    Session.Item("crmUserLogon") = False
    Call OnUserLogonFailed(New EventArgs())

  End Sub

  Public Sub Ok_Button_ModalPopup(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click

    'If they say okay, log the login.
    If Session.Item("localUser").crmEvo Then

      ForgotPasswordRoutine()

      Call OnUserLogonFailed(New EventArgs())

    End If

  End Sub

  Private Sub ForgotPassword_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ForgotPassword.Click

    If Session.Item("localUser").crmEvo Then

      forgotPasswordPopUp.CssClass = "modalPopup"

      btnOk.Focus()

      MPE2.Show() 'this shows the ajax modal popupcontrol and kills command connection

    End If

  End Sub

  Public Function getHomeBaseAdminDataForClass(ByVal emailAddress As String) As DataTable

    Dim atemptable As New DataTable
    Dim subQuery As String = ""

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      subQuery = "select top 1 user_id as home_user_id, user_default_account_id as home_account_id, user_type as home_user_type "

      If (HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER) Then
        subQuery += " from [Homebase].[jetnet_ra].[dbo].[User] "
      Else
        subQuery += "  from [User] "
      End If

      subQuery += "  where user_email_address = @emailAddress "

      SqlCommand.Parameters.AddWithValue("@emailAddress", emailAddress.ToString.Trim)

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

End Class