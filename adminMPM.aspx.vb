
' ********************************************************************************
'$$Archive: /commonWebProject/adminMPM.aspx.vb $
'$$Author: Amanda $
'$$Date: 8/21/19 1:30p $
'$$Modtime: 8/21/19 1:20p $
'$$Revision: 4 $
'$$Workfile: adminMPM.aspx.vb $

' ********************************************************************************

Partial Public Class adminMPM
  Inherits System.Web.UI.Page

  Protected reg_status As String = "N"
  Protected client_reg_Type As String = "AND (client_regCustomer_Type = 'CRM' OR client_regCustomer_Type = 'SERVERDB')"
  'client_reg_Type = "AND (client_regCustomer_Type = 'CRM' OR client_regCustomer_Type = 'SERVERDB' OR client_regCustomer_Type = 'EXPORT')"

  Protected localDatalayer As New admin_center_dataLayer

  Dim bShowAll As Boolean = False

  Dim bShowCurrentDomainUsers As Boolean = False
  Dim bShowDomainUsers As Boolean = False
  Dim bShowCurrentDomainErrors As Boolean = False
  Dim bShowDomainErrors As Boolean = False

  Dim bSendPassword As Boolean = False
  Dim bActivateUser As Boolean = False
  Dim sActivateUserFlag As String = ""
  Dim bActivate_spi_user As Boolean = False

  Dim bAddNewDomainUser As Boolean = False
  Dim bSubmitNewDomainUser As Boolean = False
  Dim bShowPasswords As Boolean = False
  Dim sHomebaseFlag As String = ""
  Dim sUsersFlag As String = ""

  Dim nClientRegID As Long = 0
  Dim nClientUserID As Long = 0

  Public Shared masterPage As New Object

  Private Sub adminMPM_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

    Dim isHomebase As Boolean = False

    If Not IsNothing(HttpContext.Current.Request("homebase")) Then
      If Not String.IsNullOrEmpty(HttpContext.Current.Request("homebase").ToString.Trim) Then
        isHomebase = IIf(HttpContext.Current.Request("homebase").ToString.Trim.Contains("Y"), True, False)
      End If
    End If

    If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER And Not isHomebase Then
      Me.MasterPageFile = "~/EvoStyles/CustomerAdminTheme.Master"
      masterPage = DirectCast(Page.Master, CustomerAdminTheme)
    ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE And Not isHomebase Then
      Me.MasterPageFile = "~/EvoStyles/HomebaseTheme.Master"
      masterPage = DirectCast(Page.Master, HomebaseTheme)
    ElseIf isHomebase Then

      Me.MasterPageFile = "~/EvoStyles/EmptyHomebaseTheme.Master"
      masterPage = DirectCast(Page.Master, EmptyHomebaseTheme)

    End If

  End Sub

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    Dim sDisplayDomainUserList As String = ""
    Dim sErrorString As String = ""

    Dim isHomebase As Boolean = False
    Me.TableCell0.Visible = False
    Dim spi_users_current As Integer = 0

    If Not IsNothing(HttpContext.Current.Request("homebase")) Then
      If Not String.IsNullOrEmpty(HttpContext.Current.Request("homebase").ToString.Trim) Then
        isHomebase = IIf(HttpContext.Current.Request("homebase").ToString.Trim.Contains("Y"), True, False)
      End If
    End If

    If Not CBool(Session.Item("crmUserLogon")) And Not isHomebase Then

      Response.Redirect("Default.aspx", True)

    ElseIf isHomebase Then


      localDatalayer.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      localDatalayer.clientConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

      If String.IsNullOrEmpty(HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim) Then
        HttpContext.Current.Session.Item("jetnetClientDatabase") = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      End If

      masterPage.SetPageTitle(" MPM Center - " + WeekdayName(Weekday(Today)).ToString + ", " + MonthName(Month(Today)).ToString + " " + Day(Today).ToString + ", " + Year(Today).ToString)

    Else

      If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
        masterPage.Set_Active_Tab(4)
        Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("MPM Admin Center - Home")
      ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Then
        masterPage.Set_Active_Tab(5)
        Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("MPM Homebase Center - Home")
      End If

      If Not Session.Item("localPreferences").loadUserSession(sErrorString, CLng(HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString), _
                                                              HttpContext.Current.Session.Item("localUser").crmUserLogin.ToString, _
                                                              CLng(HttpContext.Current.Session.Item("localUser").crmSubSeqNo.ToString), _
                                                              CLng(HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString)) Then
        Response.Redirect("Default.aspx", True)
      End If

      localDatalayer.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      localDatalayer.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
      localDatalayer.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
      localDatalayer.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
      localDatalayer.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim

    End If

    localDatalayer.crmMasterConnectStr = HttpContext.Current.Application.Item("crmMasterDatabase").ToString.Trim

    If Not IsNothing(Request.Item("show_all")) Then
      If Not String.IsNullOrEmpty(Request.Item("show_all").ToString.Trim) Then
        bShowAll = CBool(Request.Item("show_all").ToString.Trim)
      End If
    End If

    If bShowAll Then
      reg_status = "A"
      client_reg_Type = ""
    End If

    If Not IsNothing(Request.Item("id")) Then
      If Not String.IsNullOrEmpty(Request.Item("id").ToString.Trim) Then
        If IsNumeric(Request.Item("id").ToString) And CLng(Request.Item("id").ToString) Then
          nClientRegID = CLng(Request.Item("id").ToString)
        End If
      End If
    End If

    If Not IsNothing(Request.Item("user_id")) Then
      If Not String.IsNullOrEmpty(Request.Item("user_id").ToString.Trim) Then
        If IsNumeric(Request.Item("user_id").ToString) And CLng(Request.Item("user_id").ToString) Then
          nClientUserID = CLng(Request.Item("user_id").ToString)
        End If
      End If
    End If

    If Not IsNothing(Request.Item("show_current_domain_users")) Then
      If Not String.IsNullOrEmpty(Request.Item("show_current_domain_users").ToString.Trim) Then
        bShowCurrentDomainUsers = CBool(Request.Item("show_current_domain_users").ToString.Trim)
      End If
    End If

    If Not IsNothing(Request.Item("show_domain_users")) Then
      If Not String.IsNullOrEmpty(Request.Item("show_domain_users").ToString.Trim) Then
        bShowDomainUsers = CBool(Request.Item("show_domain_users").ToString.Trim)
      End If
    End If

    If Not IsNothing(Request.Item("show_pwd")) Then
      If Not String.IsNullOrEmpty(Request.Item("show_pwd").ToString.Trim) Then
        bShowPasswords = CBool(Request.Item("show_pwd").ToString.Trim)
      End If
    End If

    If Not IsNothing(Request.Item("error_display")) Then
      If Not String.IsNullOrEmpty(Request.Item("error_display").ToString.Trim) Then
        bShowCurrentDomainErrors = CBool(Request.Item("error_display").ToString.Trim)
      End If
    End If

    If Not IsNothing(Request.Item("export_display")) Then
      If Not String.IsNullOrEmpty(Request.Item("export_display").ToString.Trim) Then
        bShowDomainErrors = CBool(Request.Item("export_display").ToString.Trim)
      End If
    End If

    If Not IsNothing(Request.Item("send_password")) Then
      If Not String.IsNullOrEmpty(Request.Item("send_password").ToString.Trim) Then
        bSendPassword = CBool(Request.Item("send_password").ToString.Trim)
      End If
    End If

    If Not IsNothing(Request.Item("activate_user")) Then
      If Not String.IsNullOrEmpty(Request.Item("activate_user").ToString.Trim) Then
        bActivateUser = CBool(Request.Item("activate_user").ToString.Trim)
      End If
    End If


    If Not IsNothing(Request.Item("activate_spi_user")) Then
      If Not String.IsNullOrEmpty(Request.Item("activate_spi_user").ToString.Trim) Then
        bactivate_spi_user = CBool(Request.Item("activate_spi_user").ToString.Trim)
      End If
    End If

    If Not IsNothing(Request.Item("active")) Then
      If Not String.IsNullOrEmpty(Request.Item("active").ToString.Trim) Then
        sActivateUserFlag = Request.Item("active").ToString.ToUpper.Trim
      End If
    End If

    If Not IsNothing(Request.Item("add_client_user")) Then
      If Not String.IsNullOrEmpty(Request.Item("add_client_user").ToString.Trim) Then
        bAddNewDomainUser = CBool(Request.Item("add_client_user").ToString.Trim)
      End If
    End If

    If Not IsNothing(Request.Item("submit_client_user")) Then
      If Not String.IsNullOrEmpty(Request.Item("submit_client_user").ToString.Trim) Then
        bSubmitNewDomainUser = CBool(Request.Item("submit_client_user").ToString.Trim)
      End If
    End If

    If Not IsNothing(Request.Item("homebase")) Then
      If Not String.IsNullOrEmpty(Request.Item("homebase").ToString.Trim) Then
        sHomebaseFlag = Request.Item("homebase").ToString.ToUpper.Trim
      End If
    End If


    If Not IsNothing(Request.Item("users")) Then
      If Not String.IsNullOrEmpty(Request.Item("users").ToString.Trim) Then
        sUsersFlag = Request.Item("users").ToString.ToUpper.Trim
      End If
    End If

    If Not IsPostBack Then
      If Not IsNothing(Request.Item("type_of")) Then
        If Not String.IsNullOrEmpty(Request.Item("type_of").ToString.Trim) Then
          If Trim(Request.Item("type_of").ToString.Trim) = "All" Then
            show_type.SelectedValue = "All"
          Else
            show_type.SelectedValue = "Active"
          End If
        End If
      End If
    End If

    addNewClientUser.Visible = False  ' users
    TableRow2.Visible = False

    If (Not IsPostBack Or bShowAll) Or (bShowDomainUsers = True And nClientRegID > 0 And bShowCurrentDomainUsers = False) Then

      If bShowCurrentDomainUsers And nClientRegID > 0 Then

        localDatalayer.displayCurrentMPMDomainUsers(nClientRegID, sDisplayDomainUserList)

        mpm_data_list_display.Text = sDisplayDomainUserList.Trim

        TableRow0.Visible = False
        TableRow2.Visible = False
        TableCell1.ColumnSpan = 0

      ElseIf bShowDomainUsers And nClientRegID > 0 Then


        addNewClientUser.PostBackUrl = "adminMPM.aspx?add_client_user=true&id=" + nClientRegID.ToString

        localDatalayer.displayMPMDomainUsers(nClientRegID, sDisplayDomainUserList, addNewClientUser, sUsersFlag, show_type.SelectedValue, Me.user_info1.Text, spi_users_current, sHomebaseFlag, bShowPasswords)

        mpm_data_list_display.Text = sDisplayDomainUserList.Trim


        Me.Bottom_label.Visible = True
        Me.show_type.Visible = True

        TableRow0.Visible = False
        TableRow2.Visible = False
        TableCell1.ColumnSpan = 0

      ElseIf bSendPassword And nClientRegID > 0 And nClientUserID > 0 Then

        If localDatalayer.sendMPMDomainClientPasswordEmail(nClientRegID, nClientUserID) Then
          System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "emailSentScript", "alert(""Password Email has been sent"");", True)
        Else
          System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "emailSentScript", "alert(""Error: sending Password Email, it has NOT been sent"");", True)
        End If

        addNewClientUser.Visible = True
        addNewClientUser.PostBackUrl = "adminMPM.aspx?add_client_user=true&id=" + nClientRegID.ToString

        localDatalayer.displayMPMDomainUsers(nClientRegID, sDisplayDomainUserList, addNewClientUser, sUsersFlag, show_type.SelectedValue, user_info1.Text, "", sHomebaseFlag, bShowPasswords)

        mpm_data_list_display.Text = sDisplayDomainUserList.Trim

        TableRow0.Visible = False
        TableRow2.Visible = False
        TableCell1.ColumnSpan = 0
        Me.Bottom_label.Visible = True

      ElseIf bActivateUser And nClientRegID > 0 And nClientUserID > 0 Then

        localDatalayer.displayMPMDomainUsers(nClientRegID, sDisplayDomainUserList, addNewClientUser, sUsersFlag, show_type.SelectedValue, "", spi_users_current, sHomebaseFlag, bShowPasswords)

        If localDatalayer.activateMPMDomainClient(nClientRegID, nClientUserID, sActivateUserFlag, spi_users_current) Then
          System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "activateUserScript", "alert(""User has been " + IIf(sActivateUserFlag.Contains("Y"), "activated", "deactivated") + """);", True)
        Else
          System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "activateUserScript", "alert(""Error: User has NOT been " + IIf(sActivateUserFlag.Contains("Y"), "activated", "deactivated") + """);", True)
        End If

        addNewClientUser.Visible = True
        addNewClientUser.PostBackUrl = "adminMPM.aspx?add_client_user=true&id=" + nClientRegID.ToString

        localDatalayer.displayMPMDomainUsers(nClientRegID, sDisplayDomainUserList, addNewClientUser, sUsersFlag, show_type.SelectedValue, user_info1.Text, "", sHomebaseFlag, bShowPasswords)

        mpm_data_list_display.Text = sDisplayDomainUserList.Trim

        Me.Bottom_label.Visible = True
        TableRow0.Visible = False
        TableRow2.Visible = False
        TableCell1.ColumnSpan = 0

      ElseIf bActivate_spi_user = True And nClientRegID > 0 And nClientUserID > 0 Then


        If localDatalayer.activateMPMDomainClient_FOR_SPI(nClientRegID, nClientUserID, sActivateUserFlag) Then
          System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "activateUserScript", "alert(""User has been " + IIf(sActivateUserFlag.Contains("Y"), " turned on for Values", "turned off Values") + """);", True)
        Else
          System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "activateUserScript", "alert(""Error: User has NOT been " + IIf(sActivateUserFlag.Contains("Y"), "activated", "deactivated") + """);", True)
        End If

        addNewClientUser.PostBackUrl = "adminMPM.aspx?add_client_user=true&id=" + nClientRegID.ToString

        localDatalayer.displayMPMDomainUsers(nClientRegID, sDisplayDomainUserList, addNewClientUser, sUsersFlag, show_type.SelectedValue, "", "", sHomebaseFlag, bShowPasswords)

        mpm_data_list_display.Text = sDisplayDomainUserList.Trim
        show_type.Visible = True
        TableRow0.Visible = False
        TableRow2.Visible = False
        TableCell1.ColumnSpan = 0
        Me.Bottom_label.Visible = True

      Else

        show_type.Visible = False
        run_mpm_page("ERROR")

      End If

    ElseIf bAddNewDomainUser And nClientRegID > 0 Then

      Dim client_webHostName As String = ""
      localDatalayer.getMPMDomainDataConnection(nClientRegID, client_webHostName, "", 0)

      domainName.Text = "<b> Add New User to <a href=""http://" + client_webHostName.ToLower.Trim + """ title=""Click to GO to hosting domain"">" + client_webHostName.ToLower.Trim + "</a> Domain</b>"

      submitNewClientUser.PostBackUrl = "adminMPM.aspx?submit_client_user=true&id=" + nClientRegID.ToString

      TableRow0.Visible = False
      TableRow1.Visible = False
      TableRow2.Visible = True

      actinfo_password_mouseover_img.AlternateText = "New User Password:" + vbCrLf + vbCrLf + "New password should be a minimum of 8 characters " + vbCrLf + _
                                                     "and must contain ""at least"" one number" + vbCrLf + vbCrLf + "All characters will be stored in lower case"
      actinfo_password_mouseover_img.ToolTip = actinfo_password_mouseover_img.AlternateText

      loginString.Attributes.Add("onblur", "validatePassword();")

    ElseIf bSubmitNewDomainUser And nClientRegID > 0 Then

      If Not localDatalayer.addNewMPMDomainClient(nClientRegID, firstName.Text, lastName.Text, loginString.Text, emailAddress.Text, isAdmin.Checked) Then
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "addUserScript", "alert(""Error: User has NOT been added"");", True)
      End If

      addNewClientUser.Visible = True
      addNewClientUser.PostBackUrl = "adminMPM.aspx?add_client_user=true&id=" + nClientRegID.ToString

      localDatalayer.displayMPMDomainUsers(nClientRegID, sDisplayDomainUserList, addNewClientUser, sUsersFlag, Me.show_type.SelectedValue, "", "", sHomebaseFlag, bShowPasswords)

      mpm_data_list_display.Text = sDisplayDomainUserList.Trim

      TableRow0.Visible = False
      TableRow1.Visible = True
      TableRow2.Visible = False
      TableCell1.ColumnSpan = 0
    ElseIf IsPostBack And nClientRegID > 0 Then

      localDatalayer.displayMPMDomainUsers(nClientRegID, sDisplayDomainUserList, addNewClientUser, sUsersFlag, Me.show_type.SelectedValue, "", "", sHomebaseFlag, bShowPasswords)

      mpm_data_list_display.Text = sDisplayDomainUserList.Trim
      TableRow0.Visible = False
      TableRow1.Visible = True
      TableRow2.Visible = False
      TableCell1.ColumnSpan = 0
    End If


        Me.Bottom_label.Text = "<br clear=""all""/><div class=""Box""><p>"
        Me.Bottom_label.Text &= "<br/>To add users click on the 'Add New User' Link"
    Me.Bottom_label.Text &= "<br/>To activate or inactivate a user license click on the check mark in the 'Active' column for the user, toggling it from green (active) to red (inactive). Note that this action will only work if MPM subscription licenses are available."
    Me.Bottom_label.Text &= "<br/>To activate or inactivate a user for Values click on the check mark in the 'Active' column for the user, toggling it from green (active) to red (inactive). Note that this action will only work if values licenses are available."
        Me.Bottom_label.Text &= "<br/>To show all active and inactive users select 'Show All Users' from the upper right drop down.</p></div>"


        If nClientRegID > 0 Then
      Me.refresh.Text = "&nbsp;&nbsp;&nbsp;&nbsp;<a href='adminMPM.aspx?show_domain_users=true&id=" + nClientRegID.ToString + "&users=" + sUsersFlag + "'>Refresh</a>"
    Else
      Me.refresh.Text = ""
    End If

  End Sub

  Private Sub mpmDisplayType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles mpmDisplayType.SelectedIndexChanged

    run_mpm_page(mpmDisplayType.SelectedValue)

  End Sub

  Private Sub run_mpm_page(ByVal sDisplayType As String)

    Dim sDisplayMPMCustomerList As String = ""

    Select Case (sDisplayType.ToUpper)
      Case "CONN"
        localDatalayer.displayMPMUsers(reg_status, client_reg_Type, "CONN", sDisplayMPMCustomerList)
      Case "DATA"
        localDatalayer.displayMPMUsers(reg_status, client_reg_Type, "DATA", sDisplayMPMCustomerList)
      Case "ERROR"
        localDatalayer.displayMPMUsers(reg_status, client_reg_Type, "ERROR", sDisplayMPMCustomerList)
      Case "USERS"
        localDatalayer.displayMPMUsers(reg_status, client_reg_Type, "USERS", sDisplayMPMCustomerList)

    End Select

    mpm_data_list_display.Text = sDisplayMPMCustomerList.Trim

  End Sub

End Class