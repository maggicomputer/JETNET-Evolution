
' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/forgotPasswordVerify.aspx.vb $
'$$Author: Mike $
'$$Date: 6/17/20 1:40p $
'$$Modtime: 6/17/20 1:38p $
'$$Revision: 19 $
'$$Workfile: forgotPasswordVerify.aspx.vb $
'
' ********************************************************************************

Public Class forgotPasswordVerify
  Inherits System.Web.UI.Page
  Private localDatalayer As preferencesDataLayer
  Private oldPasswordID As String = ""

  Private sGUID As String = ""
  Private sTimeStamp As String = ""
  Private sSub_Comp_ID As String = ""
  Private sSubins_Contact_ID As String = ""
  Private sSubID As String = ""
  Private sUser_EmailAddress As String = ""
  Private sSub_Logon_ID As String = ""

  Private changeClicked As Boolean = False
  Private tryAgainClicked As Boolean = False
  Private expiredToken As Boolean = False
  Private newUser As Boolean = False

  Public Shared masterPage As New Object

  Private Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

    If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO Then
      Me.MasterPageFile = "~/EvoStyles/EmptyEvoTheme.Master"
      masterPage = DirectCast(Page.Master, EmptyEvoTheme)
    ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Then
      Me.MasterPageFile = "~/EvoStyles/EmptyHomebaseTheme.Master"
      masterPage = DirectCast(Page.Master, EmptyHomebaseTheme)
    ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
      Me.MasterPageFile = "~/EvoStyles/EmptyCustomerAdminTheme.Master"
      masterPage = DirectCast(Page.Master, EmptyCustomerAdminTheme)
    End If

  End Sub

  Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender

    If tryAgainClicked Then
      System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "PasswordTryAgain", "$(document).ready(function(){window.close();});", True)
    ElseIf changeClicked Then
      System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "PasswordChange", "$(document).ready(function(){CloseLoadingMessage(""DivLoadingMessage"");});", True)
    Else
      If changeBtn.Visible Then
        System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "PasswordRender", "$(document).ready(function(){showChangeButton();});", True)
      End If
    End If

  End Sub

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    Try

      localDatalayer = New preferencesDataLayer
      localDatalayer.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim '=True

      If Not IsNothing(Request.Item("newUser")) Then
        If Not String.IsNullOrEmpty(Request.Item("newUser").ToString.Trim) Then
          If Request.Item("newUser").ToString.ToLower.Contains("true") Then

            newUser = True

          End If
        End If
      End If

      If Not IsNothing(Request.Item("fromChange")) Then
        If Not String.IsNullOrEmpty(Request.Item("fromChange").ToString.Trim) Then
          If Request.Item("fromChange").ToString.ToLower.Contains("true") Then

            changeClicked = True

          End If
        End If
      End If

      If Not IsNothing(Request.Item("fromTryAgain")) Then
        If Not String.IsNullOrEmpty(Request.Item("fromTryAgain").ToString.Trim) Then
          If Request.Item("fromTryAgain").ToString.ToLower.Contains("true") Then

            tryAgainClicked = True

          End If
        End If
      End If

      If Not IsNothing(Request.Item("forgotToken")) Then
        If Not String.IsNullOrEmpty(Request.Item("forgotToken").ToString.Trim) Then


          Dim data() As Byte = Convert.FromBase64String(Request.Item("forgotToken").ToString)
          Dim base64Decoded As String = Encoding.ASCII.GetString(data)

          Dim passwordArray() As String = base64Decoded.Split(",")

          ' guid, timestamp, sub_comp_id, subins_contact_id, sublogin_sub_id, email, sublogon_logon         

          If Not IsNothing(passwordArray(0)) Then
            If Not String.IsNullOrEmpty(passwordArray(0).Trim) Then
              sGUID = passwordArray(0)
            End If
          End If

          If Not IsNothing(passwordArray(1)) Then
            If Not String.IsNullOrEmpty(passwordArray(1).Trim) Then
              sTimeStamp = passwordArray(1)
            End If
          End If

          If Not IsNothing(passwordArray(2)) Then
            If Not String.IsNullOrEmpty(passwordArray(2).Trim) Then
              sSub_Comp_ID = passwordArray(2)
            End If
          End If

          If Not IsNothing(passwordArray(3)) Then
            If Not String.IsNullOrEmpty(passwordArray(3).Trim) Then
              sSubins_Contact_ID = passwordArray(3)
            End If
          End If

          If Not IsNothing(passwordArray(4)) Then
            If Not String.IsNullOrEmpty(passwordArray(4).Trim) Then
              sSubID = passwordArray(4)
            End If
          End If

          If Not IsNothing(passwordArray(5)) Then
            If Not String.IsNullOrEmpty(passwordArray(5).Trim) Then
              sUser_EmailAddress = passwordArray(5)
            End If
          End If

          If Not IsNothing(passwordArray(6)) Then
            If Not String.IsNullOrEmpty(passwordArray(6).Trim) Then
              sSub_Logon_ID = passwordArray(6)
            End If
          End If

          ' verify "token" hasn't expired

          HttpContext.Current.Session.Item("localUser").crmLocalUserEmailAddress = sUser_EmailAddress

          Dim tempTable As DataTable
          tempTable = VerifyGUID()

          Dim sTmpGUID As String = ""
          Dim sTmpTimeStamp As DateTime

          If Not IsNothing(tempTable) Then
            If tempTable.Rows.Count > 0 Then

              For Each r As DataRow In tempTable.Rows

                If Not (IsDBNull(r.Item("sublogin_forgot_password_token"))) Then
                  If Not String.IsNullOrEmpty(r.Item("sublogin_forgot_password_token").ToString) Then
                    sTmpGUID = r.Item("sublogin_forgot_password_token").ToString.Trim
                  End If
                End If

                If Not (IsDBNull(r.Item("sublogin_forgot_password_token_date"))) Then
                  If Not String.IsNullOrEmpty(r.Item("sublogin_forgot_password_token_date").ToString) Then
                    sTmpTimeStamp = CDate(r.Item("sublogin_forgot_password_token_date").ToString.Trim)
                  End If
                End If

                If Not (IsDBNull(r.Item("sublogin_password"))) Then
                  If Not String.IsNullOrEmpty(r.Item("sublogin_password").ToString) Then
                    oldPasswordID = r.Item("sublogin_password").ToString.Trim
                  End If
                End If

              Next

            End If
          End If

          If sGUID.Contains(sTmpGUID) Then ' verify "GUID" matches saved guid and get old password

            If Not changeClicked Then

              If newUser Then

                If DateDiff(DateInterval.Day, sTmpTimeStamp, Now()) > 3 Then ' check token date and verify its within a 4 day window for new users ...
                  expiredToken = True
                End If

              Else

                If DateDiff(DateInterval.Minute, sTmpTimeStamp, Now()) > 10 Then ' check token date and verify its within a 10 minute window for current users ...
                  expiredToken = True
                End If

              End If

            End If


          End If

          If Not tryAgainClicked Then

            If changeClicked Then

              changeBtn.Visible = False
              tryAgainBtn.Visible = False

            End If

            If newUser Then

              user_email_text.Text = "Password Change for New User " + sUser_EmailAddress

              changeBtn.PostBackUrl = "~/forgotPasswordVerify.aspx?fromChange=True&newUser=True&forgotToken=" + Request.Item("forgotToken").ToString
              tryAgainBtn.Visible = False

              If expiredToken Then ' else show "expired token warning"

                forgot_email_response.Text = "<strong>Change New User Password Token has expired. Please Try Again.</strong>"
                changeBtn.Visible = False

                tryAgainBtn.PostBackUrl = "~/forgotPasswordVerify.aspx?fromTryAgain=True&newUser=True&forgotToken=" + Request.Item("forgotToken").ToString

                tryAgainBtn.Visible = True
                newPasswordID.Enabled = False
                confirmPasswordID.Enabled = False

              End If

              Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("Change New User Password")
              masterPage.SetPageText("Change New User Password")

              newPasswordID.Attributes.Add("onblur", "validatePassword();")
              confirmPasswordID.Attributes.Add("onblur", "showChangeButton();")

              actinfo_password_mouseover_img.AlternateText = "Change Subscriber Password:" + vbCrLf + vbCrLf + "New password should be a minimum of 8 characters " + vbCrLf +
                                                         "and must contain *at least*" + vbCrLf + vbCrLf + "one number, one LOWER case and one UPPER case, and one SPECIAL character ( !@#$%^&*()_+=- )"
              actinfo_password_mouseover_img.ToolTip = actinfo_password_mouseover_img.AlternateText

            Else

              user_email_text.Text = "Password Change for User " + sUser_EmailAddress

              changeBtn.PostBackUrl = "~/forgotPasswordVerify.aspx?fromChange=True&forgotToken=" + Request.Item("forgotToken").ToString
              tryAgainBtn.Visible = False

              If expiredToken Then ' else show "expired token warning"

                forgot_email_response.Text = "<strong>Forgot Password Token has expired. Please Try Again.</strong>"
                changeBtn.Visible = False

                tryAgainBtn.PostBackUrl = "~/forgotPasswordVerify.aspx?fromTryAgain=True&forgotToken=" + Request.Item("forgotToken").ToString

                tryAgainBtn.Visible = True
                newPasswordID.Enabled = False
                confirmPasswordID.Enabled = False

              End If

              Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("Change Forgotten Password")
              masterPage.SetPageText("Change Forgotten Password")

              newPasswordID.Attributes.Add("onblur", "validatePassword();")
              confirmPasswordID.Attributes.Add("onblur", "showChangeButton();")

              actinfo_password_mouseover_img.AlternateText = "Change Subscriber Password:" + vbCrLf + vbCrLf + "New password should be a minimum of 8 characters " + vbCrLf +
                                                         "and must contain *at least*" + vbCrLf + vbCrLf + "one number, one LOWER case and one UPPER case, and one SPECIAL character ( !@#$%^&*()_+=- )"
              actinfo_password_mouseover_img.ToolTip = actinfo_password_mouseover_img.AlternateText

            End If

          End If

        Else

          Response.Redirect("Default.aspx", True)

        End If

      Else

        Response.Redirect("Default.aspx", True)

      End If

    Catch ex As Exception

      commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " : Exception Thrown[" + ex.Message.Trim + "]")

      Response.Redirect("Default.aspx", True)

    End Try

  End Sub

  Private Sub send_password_change_email()

    Dim aclsData_Temp As New clsData_Manager_SQL
    aclsData_Temp.JETNET_DB = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

    Dim EmailString As New StringBuilder

    'Let's build the EMAIL
    EmailString.Append("<html><head>")
    EmailString.Append("<title>Evolution JETNET, Jets and Turboprops, Helicopters, Commercial Setup License Information</title>")
    EmailString.Append("</head><body>")
    EmailString.Append("<img src=""" + Application.Item("crmClientSiteData").ClientFullHostName + "/images/JN_EvolutionMarketplace_Logo2.png""><br /><br />")
    EmailString.Append("<font face=""Arial"" size=""3"">" + FormatDateTime(Now, vbGeneralDate).ToString + "<br /><br />")
    EmailString.Append("JETNET LLC<br />Utica, NY  United States<br /><br />")
    EmailString.Append("Per your request, listed below is the license information for your access to the Evolution program.<br /><br />")
    EmailString.Append("<b><a target = ""_blank"" href=""" + Application.Item("crmClientSiteData").ClientFullHostName + """>" + Application.Item("crmClientSiteData").crmClientHostName + "</a></b><br /><br />")
    EmailString.Append("<table border=""1"" cellspacing=""0"" cellpadding=""5"">")
    EmailString.Append("<tr><th align=""left"">Subscription ID  </th>")
    EmailString.Append("<th align=""right"">" + sSubID.ToString.Trim + "</th></tr>")
    EmailString.Append("<tr><th align=""left"">EMail Address  </th>")
    EmailString.Append("<th align=""right"">" + sUser_EmailAddress.Trim + "</th></tr>")
    EmailString.Append("</table>")
    EmailString.Append("<br /><b><font color=""red"">Per your request, your password to <a target=""_blank"" href=""" + Application.Item("crmClientSiteData").ClientFullHostName + """>" + Application.Item("crmClientSiteData").crmClientHostName + "</a> has been changed. If you did Not make a change to your password please contact JETNET immediately at the customer support number below.</font></b><br /><br />")
    EmailString.Append("Click the following link to view the Evolution user guide (PDF) <a target=""_blank"" title=""Evolution User Guide"" href=""" + Application.Item("crmClientSiteData").ClientFullHostName + "/help/evolution_user_guide.pdf"">User Guide</a><br /><br />")
    EmailString.Append("If the technical staff of JETNET can assist you in any way, please do Not hesitate to call 800-553-8638, Ext 1, And we will be happy to assist you.<br /><br />Best regards<br /><br />")
    EmailString.Append("<span style=""font-size:10.5pt; font-family Arial; color:#616E7D"">")
    EmailString.Append("<em><b>Customer Technical Support</b></em><br />")
    EmailString.Append("<a href=""mailto:customerservice@jetnet.com?Subject=Customer Technical Support"">customerservice@jetnet.com</a><br />")
    EmailString.Append("<em><b>JETNET LLC</b></em><br />")
    EmailString.Append("<em>Worldwide leader in aviation market intelligence.</em><br />")
    EmailString.Append("101 First St. | Utica, NY 13501 USA |<br />")
    EmailString.Append("Main Office: 800.553.8638 >> N.Y. Office: 315.797.4420<br />")
    EmailString.Append("<span style=""font-size:9.0pt; color:#616E7D"">")
    EmailString.Append("<a target=""_blank"" href=""https://www.jetnet.com/"" title=""https://www.jetnet.com/"">website</a> |")
    EmailString.Append("<a target=""_blank"" href=""http://www.jetstreamblog.com/"" title=""http://www.jetstreamblog.com/"">blog</a> |")
    EmailString.Append("<a target=""_blank"" href=""http://www.twitter.com/jetnetllc"" title=""http://www.twitter.com/jetnetllc"">twitter</a> |")
    EmailString.Append("<a target=""_blank"" href=""http://www.jetnetGlobal.com/"" title=""http://www.jetnetGlobal.com/"">ABI</a>")
    EmailString.Append("</span></span>")
    EmailString.Append("</body></html>")

    aclsData_Temp.InsertMailQueue(sSub_Comp_ID, sSubins_Contact_ID, sSubID, sUser_EmailAddress, EmailString.ToString, False, True)

  End Sub

  Private Sub send_password_forgotten_email(subcompid As Long, subinscontactid As Long, subid As Long, forgottoken As String)

    Dim aclsData_Temp As New clsData_Manager_SQL
    aclsData_Temp.JETNET_DB = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

    Dim EmailString As New StringBuilder

    'Let's build the EMAIL
    EmailString.Append("<html><head>")
    EmailString.Append("</head><body>")
    EmailString.Append("<img src=""" + Application.Item("crmClientSiteData").ClientFullHostName + "/images/JN_EvolutionMarketplace_Logo2.png""><br /><br />")
    EmailString.Append("<font face=""Arial"" size=""3"">" + FormatDateTime(Now, vbGeneralDate).ToString + "<br /><br />")
    EmailString.Append("JETNET LLC<br />Utica, NY  United States<br /><br />")
    EmailString.Append("Per your request, Please Click the link below to get Forgotten Password.<br /><br />")
    EmailString.Append("<table border=""1"" cellspacing=""0"" cellpadding=""8"">")
    EmailString.Append("<tr><th align=""left""><b><a target=""_blank"" href=""" + Application.Item("crmClientSiteData").ClientFullHostName + "forgotPasswordVerify.aspx?forgotToken=" + forgottoken + """>Forgot Password</a></b></th></tr>")
    EmailString.Append("</table>")
    EmailString.Append("<br /><br />")
    EmailString.Append("Click the following link To view the Evolution user guide (PDF) <a target=""_blank"" title=""Evolution User Guide"" href=""" + Application.Item("crmClientSiteData").ClientFullHostName + "help/evolution_user_guide.pdf"">User Guide</a><br /><br />")
    EmailString.Append("If the technical staff of JETNET can assist you in any way, please do not hesitate to call 800-553-8638, Ext 1, and we will be happy to assist you.<br /><br />Best regards<br /><br />")
    EmailString.Append("<span style=""font-size:10.5pt; font-family Arial; color:#616E7D"">")
    EmailString.Append("<em><b>Customer Technical Support</b></em><br />")
    EmailString.Append("<a href=""mailto:customerservice@jetnet.com?Subject=Customer Technical Support"">customerservice@jetnet.com</a><br />")
    EmailString.Append("<em><b>JETNET LLC</b></em><br />")
    EmailString.Append("<em>Worldwide leader in aviation market intelligence.</em><br />")
    EmailString.Append("101 First St. | Utica, NY 13501 USA |<br />")
    EmailString.Append("Main Office: 800.553.8638 >> N.Y. Office: 315.797.4420<br />")
    EmailString.Append("<span style=""font-size:9.0pt; color:#616E7D"">")
    EmailString.Append("<a target=""_blank"" href=""https://www.jetnet.com/"" title=""http://www.jetnet.com/"">website</a> |")
    EmailString.Append("<a target=""_blank"" href=""http://www.jetstreamblog.com/"" title=""http://www.jetstreamblog.com/"">blog</a> |")
    EmailString.Append("<a target=""_blank"" href=""http://www.twitter.com/jetnetllc"" title=""http://www.twitter.com/jetnetllc"">twitter</a> |")
    EmailString.Append("<a target=""_blank"" href=""http://www.jetnetglobal.com/"" title=""http://www.jetnetglobal.com/"">ABI</a>")
    EmailString.Append("</span></span>")
    EmailString.Append("</body></html>")

    aclsData_Temp.InsertMailQueue(subcompid, subinscontactid, subid, sUser_EmailAddress.Trim, EmailString.ToString, True)

    commonLogFunctions.Log_User_Event_Data("UserPreferences", "User Requested Forgotten Password", Nothing, , , , subcompid, subinscontactid)

  End Sub

  Private Sub tryAgainBtn_Click(sender As Object, e As EventArgs) Handles tryAgainBtn.Click

    Dim tempTable As DataTable
    Dim temporaryTable As New DataTable

    Dim sub_marketing_flag As String = ""
    Dim sublogin_demo_flag As String = ""
    Dim sublogon_password As String = ""
    Dim sublogon_logon As String = ""
    Dim sublogon_subid As Long = 0
    Dim sub_comp_id As Long = 0
    Dim subins_contact_id As Long = 0

    Dim EmailString As New StringBuilder

    'Let's clean the USERNAME variable for web input.
    If clsGeneral.clsGeneral.String_Special_BlackList_Words(sUser_EmailAddress.Trim, True) = True Then

      tempTable = ForgottenPassword(Replace(sUser_EmailAddress.Trim, "'", "").ToLower, sSubID, sSub_Logon_ID)

      If Not IsNothing(tempTable) Then

        If tempTable.Rows.Count > 0 Then

          If tempTable.Rows.Count = 1 Then

            'Setting up all the important flags. 
            If Not (IsDBNull(tempTable.Rows(0).Item("sub_marketing_flag"))) Then
              sub_marketing_flag = Trim(tempTable.Rows(0).Item("sub_marketing_flag").ToString)
            End If

            If Not (IsDBNull(tempTable.Rows(0).Item("sublogin_demo_flag"))) Then
              sublogin_demo_flag = Trim(tempTable.Rows(0).Item("sublogin_demo_flag").ToString)
            End If

            If Not (IsDBNull(tempTable.Rows(0).Item("sublogin_password"))) Then
              sublogon_password = Trim(tempTable.Rows(0).Item("sublogin_password").ToString)
            End If

            If Not (IsDBNull(tempTable.Rows(0).Item("sublogin_sub_id"))) Then
              sublogon_subid = tempTable.Rows(0).Item("sublogin_sub_id")
            End If

            If Not (IsDBNull(tempTable.Rows(0).Item("sub_comp_id"))) Then
              sub_comp_id = tempTable.Rows(0).Item("sub_comp_id")
            End If

            If Not (IsDBNull(tempTable.Rows(0).Item("subins_contact_id"))) Then
              subins_contact_id = tempTable.Rows(0).Item("subins_contact_id")
            End If

            If Not (IsDBNull(tempTable.Rows(0).Item("sublogin_login"))) Then
              sublogon_logon = tempTable.Rows(0).Item("sublogin_login")
            End If

            Dim timestamp As String = Now.ToString.Trim

            Dim tmpGUID As String = Guid.NewGuid().ToString

            ' guid, timestamp, sub_comp_id, subins_contact_id, sublogin_sub_id, email, sublogon_logon
            Dim tokenstring As String = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(tmpGUID + "," + timestamp + "," + sub_comp_id.ToString.Trim + "," + subins_contact_id.ToString.Trim + "," + sublogon_subid.ToString.Trim + "," + sUser_EmailAddress.Trim + "," + sublogon_logon.Trim))

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

              ' re-send email for forgotten password
              send_password_forgotten_email(sub_comp_id, subins_contact_id, sublogon_subid, tokenstring)

              forgot_email_response.Text = "Forgot Password has been sent to " + sUser_EmailAddress.Trim + ". Please check your email in a few minutes."

            Catch ex As Exception

              HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in Update Forgot Password Token" + ex.Message

            Finally

              SqlConn.Dispose()
              SqlConn.Close()
              SqlConn = Nothing

              SqlCommand.Dispose()
              SqlCommand = Nothing
            End Try

          End If

        Else
          'username doesn't exist.
          forgot_email_response.Text = "Username does not exist. " + sUser_EmailAddress.Trim
        End If

      End If

    End If

  End Sub

  Private Sub changeBtn_Click(sender As Object, e As EventArgs) Handles changeBtn.Click

    Dim tempTable As DataTable
    Dim sSavedPassword As String = ""

    If Not String.IsNullOrEmpty(oldPasswordID.Trim) And Not String.IsNullOrEmpty(newPasswordID.Text.Trim) And Not String.IsNullOrEmpty(confirmPasswordID.Text.Trim) Then

      Try

        tempTable = localDatalayer.VerifyPassword(sSub_Logon_ID, sSubID, oldPasswordID)

        If Not IsNothing(tempTable) Then
          If tempTable.Rows.Count > 0 Then

            For Each r As DataRow In tempTable.Rows
              If Not (IsDBNull(r.Item("sublogin_password"))) And Not String.IsNullOrEmpty(r.Item("sublogin_password").ToString) Then
                sSavedPassword = r.Item("sublogin_password").ToString.Trim
              End If
            Next

          End If
        End If

        tempTable = Nothing

        If String.Compare(oldPasswordID.ToLower.Trim, sSavedPassword, True) <> 0 Then
          forgot_email_response.Text = "Your old password doesn't match current password"
        Else

          If String.Compare(newPasswordID.Text.ToString.ToLower.Trim, confirmPasswordID.Text.ToString.ToLower.Trim, True) <> 0 Then
            forgot_email_response.Text = "Your new password doesn't match confirm password, please Correct and try again."
          Else

            If localDatalayer.UpdatePassword(sSub_Logon_ID, sSubID, oldPasswordID, newPasswordID.Text) Then
              forgot_email_response.Text = "Your password has been changed successfully, please use at next logon."
            End If

            newPasswordID.Enabled = False
            confirmPasswordID.Enabled = False

            login_link.Visible = True
            login_link.Text = "Please Click <a target=""_blank"" href=""" + Application.Item("crmClientSiteData").ClientFullHostName + "/Default.aspx"">Here</a> to login."

            ' send email of the password change
            send_password_change_email()

            ' if user has autologon selected then change users saved cookie of the password
            If commonEvo.getUserAutoLogonCookies(Application.Item("crmClientSiteData").AutoLogonCookie, False) Then
              Response.Cookies.Item("crmUserPassword").Item(sSubID) = Session.Item("localUser").EncodeBase64(newPasswordID.Text.ToString.ToLower.Trim)
              Response.Cookies.Item("crmUserPassword").Expires = DateTime.Now.AddDays(300)
            End If

            Dim SqlConn As New SqlClient.SqlConnection
            Dim SqlCommand As New SqlClient.SqlCommand
            Dim sQuery = New StringBuilder()

            Try

              sQuery.Append("UPDATE Subscription_Login SET sublogin_forgot_password_token = NULL, sublogin_forgot_password_token_date = NULL")
              sQuery.Append(" WHERE sublogin_sub_id = " + sSubID.Trim + " AND sublogin_login = '" + sSub_Logon_ID.Trim + "'")

              HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />Clear Forgot Password Token</b><br />" + sQuery.ToString

              SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
              SqlConn.Open()
              SqlCommand.Connection = SqlConn
              SqlCommand.CommandType = CommandType.Text
              SqlCommand.CommandTimeout = 60

              SqlCommand.CommandText = sQuery.ToString
              SqlCommand.ExecuteNonQuery()

            Catch ex As Exception

              HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in Clear Forgot Password Token" + ex.Message

            Finally

              SqlConn.Dispose()
              SqlConn.Close()
              SqlConn = Nothing

              SqlCommand.Dispose()
              SqlCommand = Nothing
            End Try
          End If

        End If

      Catch ex As Exception

        forgot_email_response.Text = "There was an error with changing your password. Your password has NOT been changed!"

      End Try

    End If

  End Sub

  Private Function VerifyGUID() As DataTable
    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT sublogin_forgot_password_token, sublogin_forgot_password_token_date, sublogin_password FROM Subscription_Login WITH(NOLOCK)")
      sQuery.Append(" WHERE (sublogin_sub_id = " + sSubID.Trim + " AND sublogin_login = '" + sSub_Logon_ID.Trim + "'")
      sQuery.Append(" AND sublogin_forgot_password_token = '" + sGUID + "')")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
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
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message

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

  Private Function ForgottenPassword(ByVal username As String, ByVal usersubid As String, ByVal userlogon As String) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT DISTINCT sublogin_sub_id, sublogin_login, sublogin_password, sublogin_active_flag, sub_marketing_flag, sublogin_demo_flag, sub_comp_id, subins_contact_id, contact_email_address")
      sQuery.Append(" FROM Subscription WITH(NOLOCK)")
      sQuery.Append(" INNER JOIN Subscription_Login WITH(NOLOCK) ON (sublogin_sub_id = sub_id)")
      sQuery.Append(" INNER JOIN Subscription_Install WITH(NOLOCK) ON ((subins_sub_id = sub_id) AND (sublogin_login = subins_login))")
      sQuery.Append(" INNER JOIN Contact WITH(NOLOCK) ON ((contact_comp_id = sub_comp_id) AND (subins_contact_id = contact_id))")

      sQuery.Append(" WHERE lower(contact_email_address) = '" + username.Trim + "' AND sublogin_sub_id = " + usersubid.Trim + " AND sublogin_login = '" + userlogon.Trim + "' AND contact_journ_id = 0")
      sQuery.Append(" AND (sub_start_date <= GetDate()) AND (sub_end_date IS NULL OR sub_end_date > GetDate())")
      sQuery.Append(" AND (sublogin_active_flag = 'Y') AND (subins_active_flag = 'Y') AND (contact_active_flag = 'Y') AND")

      sQuery.Append(" ORDER BY sublogin_sub_id")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandText = sQuery.ToString.Trim
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message

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

End Class