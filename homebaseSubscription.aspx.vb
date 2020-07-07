
' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/homebaseSubscription.aspx.vb $
'$$Author: Amanda $
'$$Date: 3/11/20 4:18p $
'$$Modtime: 3/10/20 4:11p $
'$$Revision: 21 $
'$$Workfile: homebaseSubscription.aspx.vb $
'
' ********************************************************************************

Partial Public Class homebaseSubscription
    Inherits System.Web.UI.Page

    Private sSubTask As String = ""
    Private inSubscriptionID As Long = 0
    Private inComapnyID As Long = 0
    Private inContactID As Long = 0
    Private inSubLogin As String = ""

    Private bAddNewSubscription As Boolean = False
    Private bSaveSubscription As Boolean = False
    Private bEditSubLoginInstall As Boolean = False
    Private bSaveSubLoginInstall As Boolean = False

    Private nMaxWidth As Long = 0

    Public Shared masterPage As New Object

    Private Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

        Try
            If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
                Me.MasterPageFile = "~/EvoStyles/EmptyCustomerAdminTheme.master"
                masterPage = DirectCast(Page.Master, EmptyCustomerAdminTheme)
            ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Then
                Me.MasterPageFile = "~/EvoStyles/EmptyHomebaseTheme.Master"
                masterPage = DirectCast(Page.Master, EmptyHomebaseTheme)
            ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO Then
                Me.MasterPageFile = "~/EvoStyles/EmptyEvoTheme.master"
                masterPage = DirectCast(Page.Master, EmptyEvoTheme)
            ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CRM Then
                Me.MasterPageFile = "~/EvoStyles/EmptyEvoTheme.master"
                masterPage = DirectCast(Page.Master, EmptyEvoTheme)
            End If

        Catch ex As Exception
            If Not IsNothing(masterPage) Then
                masterPage.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString + " : " + ex.Message.ToString.Trim)
            Else
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodInfo.GetCurrentMethod().ToString + "</b><br />" + ex.Message.ToString.Trim
            End If
        End Try

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim sErrorString As String = ""

        Try

            ' get request variable 
            If Session.Item("crmUserLogon") <> True Then
                Response.Redirect("Default.aspx", False)
            Else

                Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("Edit Subscription")
                masterPage.SetPageTitle("Edit Subscription")
                masterPage.SetContainerClass("container MaxWidthRemove") 'set full width page

                If Not Session.Item("localPreferences").loadUserSession(sErrorString, CLng(HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString),
                                                                      HttpContext.Current.Session.Item("localUser").crmUserLogin.ToString,
                                                                      CLng(HttpContext.Current.Session.Item("localUser").crmSubSeqNo.ToString),
                                                                      CLng(HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString)) Then
                    Response.Redirect("Default.aspx", True)
                End If

            End If

            If Not IsNothing(Request.Item("subid")) Then
                If Not String.IsNullOrEmpty(Request.Item("subid").Trim) Then
                    If IsNumeric(Request.Item("subid")) Then
                        inSubscriptionID = CLng(Request.Item("subid"))
                    End If
                End If
            End If

            If Not IsNothing(Request.Item("compid")) Then
                If Not String.IsNullOrEmpty(Request.Item("compid").Trim) Then
                    If IsNumeric(Request.Item("compid")) Then
                        inComapnyID = CLng(Request.Item("compid"))
                    End If
                End If
            End If

            If Not IsNothing(Request.Item("contactid")) Then
                If Not String.IsNullOrEmpty(Request.Item("contactid").Trim) Then
                    If IsNumeric(Request.Item("contactid")) Then
                        inContactID = CLng(Request.Item("contactid"))
                    End If
                End If
            End If

            If Not IsNothing(Request.Item("login")) Then
                If Not String.IsNullOrEmpty(Request.Item("login").Trim) Then
                    inSubLogin = Request.Item("login").Trim
                End If
            End If

            If Not IsNothing(Request.Item("task")) Then ' 
                If Not String.IsNullOrEmpty(Request.Item("task").ToString.Trim) Then
                    sSubTask = Request.Item("task").ToString.ToUpper.Trim

                    If sSubTask.ToLower.Contains("add") Then
                        bAddNewSubscription = True
                        Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("ADD Subscription")
                        masterPage.SetPageTitle("ADD Subscription")
                    End If

                    If sSubTask.ToLower.Contains("save") Then
                        bSaveSubscription = True
                    End If

                    If sSubTask.ToLower.Contains("savelogin") Then
                        bSaveSubLoginInstall = True
                    End If

                    If sSubTask.ToLower.Contains("edit") Then
                        bEditSubLoginInstall = True
                        Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("Edit License")
                        masterPage.SetPageTitle("Edit License")
                    End If

                End If '
            End If

            If Not bSaveSubscription And Not bAddNewSubscription And Not bSaveSubLoginInstall And Not bEditSubLoginInstall Then

                fillServiceList(115, sub_serv_code)

                displaySubscription()

                CompanyFunctions.Fill_Information_Tab_ChatBox(company_name, company_information_label, inComapnyID, 0, "", company_address)

            ElseIf bSaveSubscription Then ' save current subscription

            ElseIf bAddNewSubscription Then ' add new subscription

            ElseIf bSaveSubLoginInstall Then ' save current login / install

            ElseIf bEditSubLoginInstall Then

                CompanyFunctions.Fill_Information_Tab_ChatBox(company_name, company_information_label, inComapnyID, 0, "", company_address)

                Dim ContactTable As New DataTable
                Dim aclsData_Temp As New clsData_Manager_SQL
                aclsData_Temp.JETNET_DB = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

                ContactTable = aclsData_Temp.ReturnContactInformationACDetails(0, inContactID, False)
                ContactFunctions.Display_Contact_Details(ContactTable, contact_information_label, inComapnyID, 0, masterPage, False, True, False, contactNameText.Text, False, "JETNET", True)
                If Not IsNothing(ContactTable) Then
                    ContactTable.Dispose()
                End If
                ContactTable = Nothing
                aclsData_Temp = Nothing

                ContactVisibilityBox.Visible = True
                subscription_login_install.Visible = True
                subscription_panel.Visible = False

                displayLoginInstall()

            End If

        Catch ex As Exception
            If Not IsNothing(masterPage) Then
                masterPage.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString + " : " + ex.Message.ToString.Trim)
            Else
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodInfo.GetCurrentMethod().ToString + "</b><br />" + ex.Message.ToString.Trim
            End If
        End Try

    End Sub

    Private Sub saveLicense_Click(sender As Object, e As EventArgs) Handles saveLicense.Click

        saveSubscriptionChanges()

    End Sub

    Private Sub displayLoginInstall()

        Dim loginInfo As New homebaseLoginClass(inSubscriptionID, inSubLogin)

        loginInfo.fillSubscriptionLoginClass()

        Dim installInfo As New homebaseInstallClass(inSubscriptionID, inSubLogin)

        installInfo.fillSubscriptionInstallClass()

        Dim subscriptionInfo As New homebaseSubscriptionClass(inSubscriptionID, inComapnyID)

        subscriptionInfo.fillSubscriptionClass()

        sub_serv_english_desc.Text = subscriptionInfo.sub_serv_english_desc.ToUpper ' service code english translation

        sub_marketing_flag2.Checked = subscriptionInfo.sub_marketing_flag

        subins_platform_name.Items.Clear()
        subins_platform_name.Items.Add(New ListItem(installInfo.subins_platform_name.Trim, installInfo.subins_seq_no.ToString))

        If Not loginInfo.sublogin_active_flag Then
            sublogin_active_flag.Text = "Inactive"
            sublogin_active_flag.ForeColor = Drawing.Color.Red
        Else
            sublogin_active_flag.Text = "Active"
            sublogin_active_flag.ForeColor = Drawing.Color.Green
        End If

        sublogin_login_subins_seq_no.Text = loginInfo.sublogin_login + " (" + installInfo.subins_seq_no.ToString + ")"

        sublogin_password.Text = loginInfo.sublogin_password
        sublogin_contract_amount.Text = FormatCurrency(loginInfo.sublogin_contract_amount, 2, TriState.False, TriState.True, TriState.True)

        subins_admin_flag.Checked = installInfo.subins_admin_flag

        subins_install_date.Text = FormatDateTime(installInfo.subins_install_date, DateFormat.ShortDate)
        subins_access_date.Text = FormatDateTime(installInfo.subins_access_date, DateFormat.ShortDate)
        sublogin_entry_date.Text = FormatDateTime(loginInfo.sublogin_entry_date, DateFormat.ShortDate)
        subins_platform_os.Text = installInfo.subins_platform_os.Trim

        Select Case installInfo.subins_business_type_code.Trim.ToUpper

            Case "DB"
                subins_business_type_code.Text = "Dealer/Broker"
            Case "FB"
                subins_business_type_code.Text = "Fixed Base Operator"
            Case "UI"
                subins_business_type_code.Text = "Unidentified"

        End Select

        subins_session_guid.Text = installInfo.subins_session_guid.Trim
        subins_session_guid.ToolTip = installInfo.subins_session_guid.Trim

        sublogin_demo_flag.Checked = loginInfo.sublogin_demo_flag
        sublogin_values_flag.Checked = loginInfo.sublogin_values_flag
        sublogin_mpm_flag.Checked = loginInfo.sublogin_mpm_flag
        sublogin_allow_export_flag.Checked = loginInfo.sublogin_allow_export_flag
        sublogin_allow_event_request_flag.Checked = loginInfo.sublogin_allow_event_request_flag
        sublogin_allow_local_notes_flag.Checked = loginInfo.sublogin_allow_local_notes_flag
        sublogin_allow_text_message_flag.Checked = loginInfo.sublogin_allow_text_message_flag
        subins_evo_mobile_flag.Checked = installInfo.subins_evo_mobile_flag


        sublogin_allow_email_request_flag.Checked = loginInfo.sublogin_allow_email_request_flag

        If installInfo.subins_email_default_format.ToLower.Contains("html") Then
            email_format_html.Checked = True
            email_format_text.Checked = False
        Else
            email_format_html.Checked = False
            email_format_text.Checked = True
        End If

        subins_email_replyaddress.Text = installInfo.subins_email_replyaddress.Trim
        subins_email_replyname.Text = installInfo.subins_email_replyname.Trim

        If installInfo.subins_evoview_id > 0 Then
            subins_evoview_id.Text = commonEvo.Get_Default_User_View(installInfo.subins_evoview_id)
        Else
            subins_evoview_id.Text = commonEvo.Get_Default_User_View(0)
        End If

        subins_nbr_rec_per_page.Text = installInfo.subins_nbr_rec_per_page.ToString
        subins_default_analysis_months.Text = installInfo.subins_default_analysis_months.ToString

        If installInfo.subins_background_image_id > 0 Then
            subins_background_image_id.Text = commonEvo.Get_Default_User_Background(installInfo.subins_background_image_id)
        Else
            subins_background_image_id.Text = commonEvo.Get_Default_User_Background(0)
        End If

        If installInfo.subins_default_amod_id > 0 Then
            subins_default_amod_id.Text = commonEvo.Get_Aircraft_Model_Info(installInfo.subins_default_amod_id, False, "").Replace("&nbsp;", " ") ' translate to model name
        Else
            subins_default_amod_id.Text = "None"
        End If

        Dim tmpString As String = ""

        'subins_default_airports.Text = installInfo.subins_default_airports.Trim ' translate to airport name list

        If String.IsNullOrEmpty(tmpString.Trim) Then
            subins_default_airports.Text = "No Airports"
        Else
            subins_default_airports.Text = tmpString.Replace("<em>", "").Replace("<\em>", "")
        End If

        tmpString = ""

        commonEvo.fillMakeModelDropDown(Nothing, Nothing, nMaxWidth, tmpString, installInfo.subins_default_models, False, False, True, False, False, True)

        If String.IsNullOrEmpty(tmpString.Trim) Then
            subins_default_models.Text = "No Models"
        Else
            subins_default_models.Text = tmpString.Replace("<em>", "").Replace("<\em>", "")
        End If

        Select Case (installInfo.subins_smstxt_active_flag.ToUpper)
            Case Constants.SMS_ACTIVATE_YES
                subins_smstxt_active_flag.Text = "ACTIVE"
            Case Constants.SMS_ACTIVATE_NO
                subins_smstxt_active_flag.Text = "INACTIVE"
            Case Constants.SMS_ACTIVATE_PENDING
                subins_smstxt_active_flag.Text = "PENDING"
            Case Constants.SMS_ACTIVATE_WAIT
                subins_smstxt_active_flag.Text = "WAITING"
            Case Constants.SMS_ACTIVATE_TEST
                subins_smstxt_active_flag.Text = "TESTING"
            Case Else
                subins_smstxt_active_flag.Text = "INACTIVE"
        End Select

        subins_cell_number.Text = installInfo.subins_cell_number.Trim
        subins_cell_service.Text = installInfo.subins_cell_service.Trim

        tmpString = ""

        commonEvo.fillMakeModelDropDown(Nothing, Nothing, nMaxWidth, tmpString, installInfo.subins_smstxt_models, False, False, True, False, False, True)

        If String.IsNullOrEmpty(tmpString.Trim) Then
            subins_smstxt_models.Text = "No Models"
        Else
            subins_smstxt_models.Text = tmpString.Replace("<em>", "").Replace("<\em>", "")
        End If

        Dim prefDataLayer As New preferencesDataLayer
        prefDataLayer.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

        tmpString = ""

        prefDataLayer.fillSMSEventsDropDown(Nothing, 0, tmpString, installInfo.subins_sms_events, True)


        If String.IsNullOrEmpty(tmpString.Trim) Then
            subins_sms_events.Text = "No Events"
        Else
            subins_sms_events.Text = tmpString.Replace("<em>", "").Replace("<\em>", "")
        End If

        subins_mobile_active_date.Text = FormatDateTime(installInfo.subins_mobile_active_date, DateFormat.ShortDate)

        Try

        Catch ex As Exception

            If Not IsNothing(masterPage) Then
                masterPage.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString + " : " + ex.Message.ToString.Trim)
            Else
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodInfo.GetCurrentMethod().ToString + "</b><br />" + ex.Message.ToString.Trim
            End If

        End Try

    End Sub

    Private Sub displaySubscription()

        Try

            Dim subscriptionInfo As New homebaseSubscriptionClass(inSubscriptionID, inComapnyID)

            subscriptionInfo.fillSubscriptionClass()

            sub_serv_english_desc.Text = subscriptionInfo.sub_serv_english_desc.ToUpper ' service code english translation

            sub_id.Text = inSubscriptionID.ToString.Trim

            If subscriptionInfo.sub_contract_amount > 0 Then
                sub_contract_amount.Text = FormatNumber(subscriptionInfo.sub_contract_amount, 0, False, False, True)
            End If

            If subscriptionInfo.sub_nbr_of_installs > 0 Then
                sub_nbr_of_installs.Text = subscriptionInfo.sub_nbr_of_installs.ToString.Trim
            End If

            If Not String.IsNullOrEmpty(subscriptionInfo.sub_end_date.Trim) Then
                If CDate(subscriptionInfo.sub_end_date) <= Today Then
                    sub_active.ForeColor = Drawing.Color.Red
                    sub_active.Text = "Inactive License"
                    saveLicense.Visible = False
                Else
                    sub_active.ForeColor = Drawing.Color.Green
                    sub_active.Text = "Active License"
                End If
            Else
                If String.IsNullOrEmpty(subscriptionInfo.sub_start_date.Trim) Then
                    sub_active.ForeColor = Drawing.Color.Red
                    sub_active.Text = "Inactive License"
                    saveLicense.Visible = False
                Else
                    sub_active.ForeColor = Drawing.Color.Green
                    sub_active.Text = "Active License"
                End If
            End If

            If Not String.IsNullOrEmpty(subscriptionInfo.sub_start_date.Trim) Then
                sub_start_date.Text = FormatDateTime(subscriptionInfo.sub_start_date.Trim, DateFormat.ShortDate)
            End If

            If Not String.IsNullOrEmpty(subscriptionInfo.sub_end_date.Trim) Then
                sub_end_date.Text = FormatDateTime(subscriptionInfo.sub_end_date.Trim, DateFormat.ShortDate)
            End If

            If Not String.IsNullOrEmpty(subscriptionInfo.sub_serv_code.Trim) Then
                sub_serv_code.SelectedValue = subscriptionInfo.sub_serv_code.ToLower
            End If

            If Not String.IsNullOrEmpty(subscriptionInfo.sub_frequency.Trim) Then
                sub_frequency.SelectedValue = subscriptionInfo.sub_frequency.ToLower
            End If

            sub_business_aircraft_flag.Checked = subscriptionInfo.sub_business_aircraft_flag

            If Not String.IsNullOrEmpty(subscriptionInfo.sub_busair_tier_level.Trim) Then
                sub_busair_tier_level.SelectedValue = subscriptionInfo.sub_busair_tier_level.Trim
            Else
                sub_busair_tier_level.SelectedValue = "3"
            End If

            sub_helicopters_flag.Checked = subscriptionInfo.sub_helicopters_flag

            sub_commerical_flag.Checked = subscriptionInfo.sub_commerical_flag

            sub_yacht_flag.Checked = subscriptionInfo.sub_yacht_flag

            sub_aerodex_flag.Checked = subscriptionInfo.sub_aerodex_flag

            If (subscriptionInfo.sub_aerodex_flag) Then

                sub_sale_price_flag.Visible = False
                sub_nbr_of_spi_installs.Visible = False

            Else

                sub_sale_price_flag.Checked = subscriptionInfo.sub_sale_price_flag

                If subscriptionInfo.sub_nbr_of_spi_installs > 0 Then
                    sub_nbr_of_spi_installs.Text = subscriptionInfo.sub_nbr_of_spi_installs.ToString.Trim
                End If

            End If

            If (subscriptionInfo.sub_serv_code.ToLower.Contains("api")) Then
                sub_history_flag.Checked = subscriptionInfo.sub_history_flag
            Else
                sub_history_flag.Visible = False
            End If


            If subscriptionInfo.sub_max_allowed_custom_export > 0 Then
                sub_max_allowed_custom_export.Text = FormatNumber(subscriptionInfo.sub_max_allowed_custom_export, 0, False, False, True)
            End If

            sub_share_by_comp_id_flag.Checked = subscriptionInfo.sub_share_by_comp_id_flag

            sub_share_by_parent_sub_id_flag.Checked = subscriptionInfo.sub_share_by_parent_sub_id_flag

            sub_abi_flag.Checked = subscriptionInfo.sub_abi_flag

            sub_marketing_flag.Checked = subscriptionInfo.sub_marketing_flag

            If subscriptionInfo.sub_nbr_days_expire > 0 Then
                sub_nbr_days_expire.Text = subscriptionInfo.sub_nbr_days_expire.ToString.Trim
            End If

            If subscriptionInfo.sub_cloud_notes_flag Then
                sub_notes.SelectedValue = "sub_cloud_notes_flag"
            End If

            If subscriptionInfo.sub_server_side_notes_flag Then
                sub_notes.SelectedValue = "sub_server_side_notes_flag"
            End If

            If ((Not subscriptionInfo.sub_cloud_notes_flag) And (Not subscriptionInfo.sub_server_side_notes_flag)) Then
                sub_notes.SelectedValue = "sub_notes_off"
            End If

            If Not String.IsNullOrEmpty(subscriptionInfo.sub_callback_status.Trim) Then
                sub_end_date.Text = subscriptionInfo.sub_callback_status
            End If

            Fill_Customer_Activities()

            Fill_Contract_Execution()

            Fill_Contract_List()

            Fill_Active_User_Block()

        Catch ex As Exception

            If Not IsNothing(masterPage) Then
                masterPage.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString + " : " + ex.Message.ToString.Trim)
            Else
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodInfo.GetCurrentMethod().ToString + "</b><br />" + ex.Message.ToString.Trim
            End If

        End Try

    End Sub

    Private Function saveSubscriptionChanges() As Boolean

        Try

            Dim subscriptionInfo As New homebaseSubscriptionClass(inSubscriptionID, inComapnyID)
            subscriptionInfo.fillSubscriptionClass()

            Dim subscriptionInfoChange As New homebaseSubscriptionClass(inSubscriptionID, inComapnyID)
            subscriptionInfoChange.fillSubscriptionClass()

            If Not String.IsNullOrEmpty(sub_nbr_of_installs.Text.Trim) Then
                If IsNumeric(sub_nbr_of_installs.Text) Then
                    subscriptionInfoChange.sub_nbr_of_installs = CInt(sub_nbr_of_installs.Text)
                End If
            End If

            If Not String.IsNullOrEmpty(sub_contract_amount.Text.Trim) Then
                If IsNumeric(sub_contract_amount.Text) Then
                    subscriptionInfoChange.sub_contract_amount = CDbl(sub_contract_amount.Text)
                End If
            End If

            subscriptionInfoChange.sub_start_date = sub_start_date.Text.Trim
            subscriptionInfoChange.sub_end_date = sub_end_date.Text.Trim

            subscriptionInfoChange.sub_marketing_flag = sub_marketing_flag.Checked

            If Not String.IsNullOrEmpty(sub_nbr_days_expire.Text.Trim) Then
                If IsNumeric(sub_nbr_days_expire.Text) Then
                    subscriptionInfoChange.sub_nbr_days_expire = CInt(sub_nbr_days_expire.Text)
                End If
            End If

            subscriptionInfoChange.sub_frequency = sub_frequency.SelectedValue.ToLower

            subscriptionInfoChange.sub_business_aircraft_flag = sub_business_aircraft_flag.Checked
            subscriptionInfoChange.sub_busair_tier_level = sub_busair_tier_level.SelectedValue

            subscriptionInfoChange.sub_helicopters_flag = sub_helicopters_flag.Checked
            subscriptionInfoChange.sub_commerical_flag = sub_commerical_flag.Checked
            subscriptionInfoChange.sub_aerodex_flag = sub_aerodex_flag.Checked

            subscriptionInfoChange.sub_yacht_flag = sub_yacht_flag.Checked
            subscriptionInfoChange.sub_history_flag = sub_history_flag.Checked
            subscriptionInfoChange.sub_sale_price_flag = sub_sale_price_flag.Checked

            If Not String.IsNullOrEmpty(sub_nbr_of_spi_installs.Text.Trim) Then
                If IsNumeric(sub_nbr_of_spi_installs.Text) Then
                    subscriptionInfoChange.sub_nbr_of_spi_installs = CInt(sub_nbr_of_spi_installs.Text)
                End If
            End If

            subscriptionInfoChange.sub_abi_flag = sub_abi_flag.Checked
            subscriptionInfoChange.sub_share_by_comp_id_flag = sub_share_by_comp_id_flag.Checked
            subscriptionInfoChange.sub_share_by_parent_sub_id_flag = sub_share_by_parent_sub_id_flag.Checked

            If Not String.IsNullOrEmpty(sub_max_allowed_custom_export.Text.Trim) Then
                If IsNumeric(sub_max_allowed_custom_export.Text) Then
                    subscriptionInfoChange.sub_max_allowed_custom_export = CLng(sub_max_allowed_custom_export.Text)
                End If
            End If

            If (subscriptionInfoChange <> subscriptionInfo) And Not bAddNewSubscription Then
                subscriptionInfo.updateSubscriptionClass()
            ElseIf bAddNewSubscription Then
                subscriptionInfo.insertSubscriptionClass()
            End If

            displaySubscription()

        Catch ex As Exception

            If Not IsNothing(masterPage) Then
                masterPage.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString + " : " + ex.Message.ToString.Trim)
            Else
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodInfo.GetCurrentMethod().ToString + "</b><br />" + ex.Message.ToString.Trim
            End If

        End Try

    End Function

    Public Function getServiceCodesDataTable() As DataTable

        Dim atemptable As New DataTable
        Dim subQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            subQuery.Append("SELECT DISTINCT serv_code, serv_name FROM [Service] WITH(NOLOCK) WHERE serv_active_flag = 'Y' ORDER BY serv_code ASC")

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

    Public Sub fillServiceList(ByRef maxWidth As Long, ByRef serviceCodes As ListBox)

        Dim results_table As New DataTable

        serviceCodes.Items.Clear()

        Try

            results_table = getServiceCodesDataTable()

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    For Each r As DataRow In results_table.Rows

                        If Not IsDBNull(r.Item("serv_code")) And Not String.IsNullOrEmpty(r.Item("serv_code").ToString.Trim) Then

                            serviceCodes.Items.Add(New ListItem(r.Item("serv_code").ToString.ToUpper + " (" + r.Item("serv_name").ToString + ")", r.Item("serv_code").ToString.ToLower))

                        End If

                    Next
                End If
            End If

            serviceCodes.Width = maxWidth

        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message

        Finally

        End Try

        results_table = Nothing

    End Sub

    Public Sub Fill_Customer_Activities()
        Dim user_table As New DataTable
        Dim htmlOut As New StringBuilder
        Dim temp_desc As String = ""

        Dim helperClass As New displayCompanyDetailsFunctions

        user_table = helperClass.Return_Customer_Activities_Summary(inComapnyID, 0, "N", inSubscriptionID)

        If Not IsNothing(user_table) Then

            htmlOut.Append("<table id=""customerActivitiesTable"" style=""padding: 4px; border-collapse: separate; border-spacing: 6px;"" border=""0"">")
            htmlOut.Append("<tr>")
            htmlOut.Append("<td align='left'><b>DATE (desc)</b></td>")
            htmlOut.Append("<td align='left'><b>TIME</b></td>")
            htmlOut.Append("<td align='left'><b>INIT</b></td>")
            htmlOut.Append("<td align='left'><b>CONTACT</b></td>")
            htmlOut.Append("<td align='left'><b>NOTE</b></td>")

            htmlOut.Append("</tr>")

            If (user_table.Rows.Count > 0) Then

                For Each q As DataRow In user_table.Rows

                    htmlOut.Append("<tr bgcolor=""white"">")

                    htmlOut.Append("<td style=""vertical-align: top; text-align: left; padding: 4px;"">")

                    If Not IsDBNull(q("cstact_added_date")) Then
                        If Not String.IsNullOrEmpty(q("cstact_added_date").ToString.Trim) Then
                            htmlOut.Append(FormatDateTime(q("cstact_added_date").ToString, DateFormat.ShortDate))
                        End If
                    End If

                    htmlOut.Append("</td><td style=""vertical-align: top; text-align: left; padding: 4px;"">")

                    If Not IsDBNull(q("cstact_added_time")) Then
                        If Not String.IsNullOrEmpty(q("cstact_added_time").ToString.Trim) Then
                            htmlOut.Append(FormatDateTime(q("cstact_added_time").ToString, DateFormat.LongTime).Replace(" ", "&nbsp;"))
                        End If
                    End If

                    htmlOut.Append("</td><td style=""vertical-align: top; text-align: left; padding: 4px;"">")

                    If Not IsDBNull(q("cstact_init")) Then
                        If Not String.IsNullOrEmpty(q("cstact_init").ToString.Trim) Then
                            htmlOut.Append(q("cstact_init").ToString.Trim)
                        End If
                    End If

                    htmlOut.Append("</td><td style=""vertical-align: top; text-align: left; padding: 4px;"">")

                    If Not IsDBNull(q("sub_contact_id")) Then
                        If Not String.IsNullOrEmpty(q("sub_contact_id").ToString.Trim) Then
                            If IsNumeric(q("sub_contact_id").ToString.Trim) Then

                                Dim contactName As New StringBuilder

                                If Not IsDBNull(q("contact_first_name")) Then
                                    If Not String.IsNullOrEmpty(q("contact_first_name").ToString.Trim) Then
                                        contactName.Append(q("contact_first_name").ToString.Trim)
                                    End If
                                End If

                                If Not IsDBNull(q("contact_last_name")) Then
                                    If Not String.IsNullOrEmpty(q("contact_last_name").ToString.Trim) Then
                                        contactName.Append("&nbsp;" + q("contact_last_name").ToString.Trim)
                                    End If
                                End If

                                htmlOut.Append("<a class=""underline"" onclick='javascript:openSmallWindowJS(""DisplayContactDetail.aspx?compid=" + inComapnyID.ToString + "&conid=" + q.Item("sub_contact_id").ToString + "&JournID=0"",""ContactDetailsWindow"");' title='Show Contact Details'>" + contactName.ToString.Trim + "</a>")
                            End If
                        End If
                    End If

                    htmlOut.Append("</td><td style=""vertical-align: top; text-align: left; padding: 4px;"">")

                    If Not IsDBNull(q("cstact_note")) Then
                        If Not String.IsNullOrEmpty(q("cstact_note").ToString.Trim) Then
                            htmlOut.Append(q("cstact_note").ToString.Trim)
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("</tr>")

                Next

                htmlOut.Append("</table>")

                customerActivities_Label.Text = htmlOut.ToString

            End If

        End If

    End Sub

    Public Sub Fill_Contract_Execution()
        Dim user_table As New DataTable
        Dim htmlOut As New StringBuilder
        Dim temp_desc As String = ""

        Dim helperClass As New displayCompanyDetailsFunctions

        user_table = helperClass.Return_Contract_Execution_Summary(inComapnyID, 0, "N", inSubscriptionID)

        If Not IsNothing(user_table) Then

            htmlOut.Append("<table id=""contractExecutionTable"" class=""formatTable blue"">")
            htmlOut.Append("<tr>")
            htmlOut.Append("<td align=""left""><b>DATE (desc)</b></td>")
            htmlOut.Append("<td align=""left""><b>FEE</b></td>")
            htmlOut.Append("<td align=""left""><b>NOTES</b></td>")
            htmlOut.Append("<td align=""left""><b>TYPE</b></td>")

            htmlOut.Append("</tr>")

            If (user_table.Rows.Count > 0) Then

                For Each q As DataRow In user_table.Rows

                    htmlOut.Append("<tr bgcolor=""white"">")

                    htmlOut.Append("<td style=""vertical-align: top; text-align: left; padding: 4px;"">")

                    If Not IsDBNull(q("cstexcform_exc_date")) Then
                        If Not String.IsNullOrEmpty(q("cstexcform_exc_date").ToString.Trim) Then
                            htmlOut.Append(FormatDateTime(q("cstexcform_exc_date").ToString, DateFormat.ShortDate))
                        End If
                    End If

                    htmlOut.Append("</td><td style=""vertical-align: top; text-align: left; padding: 4px;"">")

                    If Not IsDBNull(q("cstexcform_monthly_fee")) Then
                        If Not String.IsNullOrEmpty(q("cstexcform_monthly_fee").ToString.Trim) Then
                            htmlOut.Append(FormatNumber(q("cstexcform_monthly_fee").ToString.Trim, 2, TriState.False, TriState.False, True))
                        End If
                    End If

                    htmlOut.Append("</td><td style=""vertical-align: top; text-align: left; padding: 4px;"">")

                    If Not IsDBNull(q("cstexcform_notes")) Then
                        If Not String.IsNullOrEmpty(q("cstexcform_notes").ToString.Trim) Then
                            htmlOut.Append(q("cstexcform_notes").ToString.Replace(". ", ".<br /><br />").Trim)
                        End If
                    End If

                    htmlOut.Append("</td><td style=""vertical-align: top; text-align: left; padding: 4px;"">")

                    If Not IsDBNull(q("cstexcform_type")) Then
                        If Not String.IsNullOrEmpty(q("cstexcform_type").ToString.Trim) Then
                            htmlOut.Append(q("cstexcform_type").ToString.Trim)
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("</tr>")

                Next

                htmlOut.Append("</table>")

                contractExecution_Label.Text = htmlOut.ToString

            End If

        End If

    End Sub

    Public Sub Fill_Contract_List()
        Dim user_table As New DataTable
        Dim htmlOut As New StringBuilder
        Dim temp_desc As String = ""

        Dim helperClass As New displayCompanyDetailsFunctions

        user_table = helperClass.Return_Contract_List_Summary(inComapnyID, 0, "N", inSubscriptionID)

        If Not IsNothing(user_table) Then

            htmlOut.Append("<table id=""contractListTable"" style=""padding: 4px; border-collapse: separate; border-spacing: 6px;"" border=""0"">")
            htmlOut.Append("<tr>")
            htmlOut.Append("<td align=""left""><b>DOCID</b></td>")
            htmlOut.Append("<td align=""left""><b>DOC&nbsp;DATE</b></td>")
            htmlOut.Append("<td align=""left""><b>ENTRY&nbsp;DATE</b></td>")
            htmlOut.Append("<td align=""left""><b>TYPE</b></td>")
            htmlOut.Append("<td align=""left""><b>SUBJECT</b></td>")

            htmlOut.Append("</tr>")

            If (user_table.Rows.Count > 0) Then

                For Each q As DataRow In user_table.Rows

                    htmlOut.Append("<tr bgcolor=""white"">")

                    htmlOut.Append("<td style=""vertical-align: top; text-align: left; padding: 4px;"">")

                    If Not IsDBNull(q("DOCID")) Then
                        If Not String.IsNullOrEmpty(q("DOCID").ToString.Trim) Then
                            htmlOut.Append(q("DOCID").ToString.Trim)
                        End If
                    End If

                    htmlOut.Append("</td><td style=""vertical-align: top; text-align: left; padding: 4px;"">")

                    If Not IsDBNull(q("DOCDATE")) Then
                        If Not String.IsNullOrEmpty(q("DOCDATE").ToString.Trim) Then
                            htmlOut.Append(FormatDateTime(q("DOCDATE").ToString, DateFormat.ShortDate))
                        End If
                    End If

                    htmlOut.Append("</td><td style=""vertical-align: top; text-align: left; padding: 4px;"">")

                    If Not IsDBNull(q("ENTRYDATE")) Then
                        If Not String.IsNullOrEmpty(q("ENTRYDATE").ToString.Trim) Then
                            htmlOut.Append(FormatDateTime(q("ENTRYDATE").ToString, DateFormat.ShortDate))
                        End If
                    End If

                    htmlOut.Append("</td><td style=""vertical-align: top; text-align: left; padding: 4px;"">")

                    If Not IsDBNull(q("DOCTYPE")) Then
                        If Not String.IsNullOrEmpty(q("DOCTYPE").ToString.Trim) Then
                            htmlOut.Append(q("DOCTYPE").ToString.Trim)
                        End If
                    End If

                    htmlOut.Append("</td><td style=""vertical-align: top; text-align: left; padding: 4px;"">")

                    If Not IsDBNull(q("SUBJECT")) Then
                        If Not String.IsNullOrEmpty(q("SUBJECT").ToString.Trim) Then
                            htmlOut.Append(q("SUBJECT").ToString.Trim.Replace(". ", ".<br /><br />").Trim)
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("</tr>")

                Next

                htmlOut.Append("</table>")

                contractList_Label.Text = htmlOut.ToString

            End If

        End If

    End Sub

    Public Function Return_ActiveUser_Summary(ByVal compID As Long, ByVal JournalID As Long, ByVal is_rollup As String, ByVal Optional getActiveLogins As Boolean = True, Optional ByVal subID As Long = 0) As DataTable
        Dim sql As String = ""
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim atemptable As New DataTable
        Try

            sql += "SELECT DISTINCT contact_first_name AS FIRSTNAME, contact_last_name AS LASTNAME, contact_email_address AS EMAIL,"
            sql += " subins_platform_name AS NOTES, sublogin_password AS PWD, subins_last_login_date AS LASTLOGIN,"
            sql += " sublogin_contract_amount as AMOUNT, subins_platform_os as ENVIRONMENT, subins_last_login_date AS LASTLOGIN,"
            sql += " case when subins_admin_flag = 'Y' then 'YES' else 'NO' end as ADMIN, "
            sql += " case when sublogin_active_flag = 'Y' then 'ACTIVE' else 'INACTIVE' end AS STATUS, "
            sql += " case when sublogin_values_flag = 'Y' then 'YES' else 'NO' end AS JVALUES, "
            sql += " case when sublogin_mpm_flag = 'Y' then 'YES' else 'NO' end AS MPM, "
            sql += " contact_id AS CONTACTID, sub_id As SUBID, sub_start_date, sub_end_date, subins_login"

            sql += " FROM " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Subscription_Login WITH (NOLOCK)"
            sql += " INNER JOIN " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Subscription_Install WITH (NOLOCK) ON sublogin_sub_id = subins_sub_id and sublogin_login = subins_login"
            sql += " INNER JOIN " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Subscription WITH (NOLOCK) ON sublogin_sub_id = sub_id"
            sql += " INNER JOIN " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Contact WITH (NOLOCK) ON subins_contact_id = contact_id and contact_journ_id = 0"


            If is_rollup = "Y" Then
                sql += " WHERE sub_comp_id in (select distinct RelCompID from ReturnAllCompanyLocationsByCompId(" + compID.ToString + "))"
            Else
                sql += " WHERE sub_comp_id = " + compID.ToString
            End If

            sql += " AND (sub_frequency <> '') AND (sub_frequency IS NOT NULL) "

            If getActiveLogins Then
                sql += " AND sublogin_active_flag = 'Y'"
            End If

            If subID > 0 Then
                sql += " AND sub_id = " + subID.ToString
            End If

            sql += " ORDER BY STATUS, contact_last_name"

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, [GetType]().FullName, sql.ToString)

            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
            SqlConn.Open()
            SqlCommand.Connection = SqlConn


            SqlCommand.CommandText = sql
            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 60

            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
            End Try
        Catch ex As Exception
            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, [GetType]().FullName, ex.Message)
            Return Nothing
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

    Public Sub Fill_Active_User_Block()

        Dim user_table As New DataTable
        Dim htmlOut As New StringBuilder

        Dim helperClass As New displayCompanyDetailsFunctions

        user_table = Return_ActiveUser_Summary(inComapnyID, 0, "N", True, inSubscriptionID)

        If Not IsNothing(user_table) Then

            htmlOut.Append("<table id=""licences_DataTable"" cellpadding=""0"" cellspacing=""0"" border=""0"" width=""100%"">")
            htmlOut.Append("<thead><tr>")

            htmlOut.Append("<th width=""5""></th>")
            htmlOut.Append("<th width=""5"" class=""text_align_center"">EDIT</th>")
            htmlOut.Append("<th data-priority=""1"">NAME</th>")
            htmlOut.Append("<th>EMAIL</th>")
            htmlOut.Append("<th>NOTES</th>")
            htmlOut.Append("<th>PWD</th>")
            htmlOut.Append("<th>LASTLOGIN</th>")
            htmlOut.Append("<th>AMT</th>")
            htmlOut.Append("<th>ADMIN</th>")
            htmlOut.Append("<th>STATUS</th>")
            htmlOut.Append("<th>JVALUES</th>")
            htmlOut.Append("<th>MPM</th>")
            htmlOut.Append("<th>PLATFORM</th>")

            htmlOut.Append("</tr></thead><tbody>")

            If (user_table.Rows.Count > 0) Then

                For Each q As DataRow In user_table.Rows


                    htmlOut.Append("<tr>")

                    htmlOut.Append("<td align=""left"" valign=""top"" nowrap=""nowrap"" class=""text_align_center"">" + q.Item("CONTACTID").ToString.Trim + "</td>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" class=""text_align_center"">")

                    htmlOut.Append("<a onclick=""javascript:ShowLoadingMessage('DivLoadingMessage', 'Edit Licence', 'Loading ... Please Wait ...');return true;"" href=""homebaseSubscription.aspx?task=edit&fromTable=true&subid=" + q.Item("SUBID").ToString.Trim + "&compid=" + inComapnyID.ToString + "&contactid=" + q.Item("CONTACTID").ToString.Trim + "&login=" + q.Item("subins_login").ToString.Trim + """ title=""Edit Licence""><img src =""images/edit_icon.png"" alt=""Edit Licence"" title=""Edit Licence""></a>")


                    htmlOut.Append("</td><td style=""vertical-align: top; text-align: left; padding: 4px;""data-sort=""" + IIf(Not IsDBNull(q("LASTNAME")), q("LASTNAME").ToString, "") + """>")

                    If Not IsDBNull(q("FIRSTNAME")) Then
                        If Not String.IsNullOrEmpty(q("FIRSTNAME").ToString.Trim) Then
                            htmlOut.Append(q("FIRSTNAME").ToString.Replace(" ", "&nbsp;").Trim + " ")
                        End If
                    End If

                    If Not IsDBNull(q("LASTNAME")) Then
                        If Not String.IsNullOrEmpty(q("LASTNAME").ToString.Trim) Then
                            htmlOut.Append(q("LASTNAME").ToString.Replace(" ", "&nbsp;").Trim)
                        End If
                    End If

                    htmlOut.Append("</td><td style=""vertical-align: top; text-align: left; padding: 4px;"">")

                    If Not IsDBNull(q("EMAIL")) Then
                        If Not String.IsNullOrEmpty(q("EMAIL").ToString.Trim) Then
                            htmlOut.Append("<a class=""underline"" onclick='javascript:openSmallWindowJS(""DisplayContactDetail.aspx?compid=" + inComapnyID.ToString + "&conid=" + q.Item("CONTACTID").ToString + "&JournID=0"",""ContactDetailsWindow"");' title='Show Contact Details'>" + q("EMAIL").ToString.Trim + "</a>")
                        End If
                    End If

                    htmlOut.Append("</td><td style=""vertical-align: top; text-align: left; padding: 4px;"">")

                    If Not IsDBNull(q("NOTES")) Then
                        If Not String.IsNullOrEmpty(q("NOTES").ToString.Trim) Then
                            htmlOut.Append(q("NOTES").ToString)
                        End If
                    End If

                    htmlOut.Append("</td><td style=""vertical-align: top; text-align: left; padding: 4px;"">")

                    If Not IsDBNull(q("PWD")) Then
                        If Not String.IsNullOrEmpty(q("PWD").ToString.Trim) Then
                            htmlOut.Append(q("PWD").ToString.Trim)
                        End If
                    End If

                    htmlOut.Append("</td><td style=""vertical-align: top; text-align: left; padding: 4px;"">")

                    If Not IsDBNull(q("LASTLOGIN")) Then
                        If Not String.IsNullOrEmpty(q("LASTLOGIN").ToString.Trim) Then
                            htmlOut.Append(FormatDateTime(q("LASTLOGIN").ToString, DateFormat.ShortDate))
                        End If
                    End If
                    htmlOut.Append("</td><td style=""vertical-align: top; text-align: left; padding: 4px;"">")

                    If Not IsDBNull(q("AMOUNT")) Then
                        If Not String.IsNullOrEmpty(q("AMOUNT").ToString.Trim) Then
                            htmlOut.Append(q("AMOUNT").ToString)
                        End If
                    End If

                    htmlOut.Append("</td><td style=""vertical-align: top; text-align: left; padding: 4px;"">")

                    If Not IsDBNull(q("ADMIN")) Then
                        If Not String.IsNullOrEmpty(q("ADMIN").ToString.Trim) Then
                            htmlOut.Append(q("ADMIN").ToString.Trim)
                        End If
                    End If

                    htmlOut.Append("</td><td style=""vertical-align: top; text-align: left; padding: 4px;"">")

                    If Not IsDBNull(q("STATUS")) Then
                        If Not String.IsNullOrEmpty(q("STATUS").ToString.Trim) Then
                            htmlOut.Append(q("STATUS").ToString.Trim)
                        End If
                    End If

                    htmlOut.Append("</td><td style=""vertical-align: top; text-align: left; padding: 4px;"">")

                    If Not IsDBNull(q("JVALUES")) Then
                        If Not String.IsNullOrEmpty(q("JVALUES").ToString.Trim) Then
                            htmlOut.Append(q("JVALUES").ToString.Trim)
                        End If
                    End If

                    htmlOut.Append("</td><td style=""vertical-align: top; text-align: left; padding: 4px;"">")

                    If Not IsDBNull(q("MPM")) Then
                        If Not String.IsNullOrEmpty(q("MPM").ToString.Trim) Then
                            htmlOut.Append(q("MPM").ToString.Trim)
                        End If
                    End If

                    htmlOut.Append("</td><td style=""vertical-align: top; text-align: left; padding: 4px;"">")

                    If Not IsDBNull(q("ENVIRONMENT")) Then
                        If Not String.IsNullOrEmpty(q("ENVIRONMENT").ToString.Trim) Then
                            htmlOut.Append(q("ENVIRONMENT").ToString.Trim)
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("</tr>" + vbCrLf)

                Next

            End If

            htmlOut.Append("</tbody></table>")
            htmlOut.Append("<div id=""licences_Label"" class="""" style=""padding:2px;""><strong>" + user_table.Rows.Count.ToString + " Records</strong></div>")
            htmlOut.Append("<div id=""licences_InnerTable"" align=""left"" valign=""middle"" style=""max-height:610px; overflow: auto;""></div>")

            searchResultsTable_licences.Text = htmlOut.ToString

        End If

    End Sub

    Private Sub homebaseSubscription_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        Try
            Dim JavascriptOnLoad As String = ""


            If subscription_panel.Visible Then

                JavascriptOnLoad += vbCrLf + "CreateSearchTable(""licences_InnerTable"",""licences_DataTable"",""licences_jQueryTable"");"

            ElseIf subscription_login_install.Visible Then

                JavascriptOnLoad += vbCrLf + "resizeImages();"
            End If

            If Not Page.ClientScript.IsClientScriptBlockRegistered("onLoadCode") Then
                System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "onLoadCode", "$(document).ready(function() {" + JavascriptOnLoad.ToString + "});", True)
            End If

        Catch ex As Exception
            If Not IsNothing(masterPage) Then
                masterPage.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString + " (Page_PreRender): " + ex.Message.ToString)
            Else
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += " (Page_PreRender): " + ex.Message.ToString.Trim
            End If
        End Try

    End Sub
End Class