
' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/Preferences.aspx.vb $
'$$Author: Amanda $
'$$Date: 6/18/20 4:20p $
'$$Modtime: 6/18/20 2:01p $
'$$Revision: 38 $
'$$Workfile: Preferences.aspx.vb $
'
' ********************************************************************************

Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Reflection

Partial Public Class Preferences
    Inherits System.Web.UI.Page
    Public aclsData_Temp As clsData_Manager_SQL

    Public bRefreshSession As Boolean = False
    Public bShowChatChangeDialog As Boolean = False

    Public sChatChangeGUID As String = ""
    Public sChatChangeUserAlias As String = ""
    Public bChatChangeEnable As Boolean = False
    Public bChatChangeSub As Boolean = False

    Private strUserEmailAddress As String = ""
    Private nDefaultModelID As Long = 0
    Private nDefaultBackgroundID As Long = 0
    Private sSMSSelectedModelID As String = ""
    Private sSMSActivationStatus As String = ""
    Private sSMSSelectedEvents As String = ""
    Private sDefaultModels As String = ""
    Private sDefaultAirports As String = ""
    Private bHasAcNotesOnListing As Boolean = False

    Private sEmailName As String = ""
    Private sEmailAddress As String = ""
    Private sEmailFormat As String = ""

    Private sMobileNumber As String = ""
    Private bEnableMobile As Boolean = False
    Private bEnableChat As Boolean = False
    Private bEnableGLOBALListings As Boolean = False

    Private bShareByCompany As Boolean = False
    Private bShareBySubscription As Boolean = False
    Private nParentSubID As Long = 0
    Private nSubCompID As Long = 0

    Private sSMSNumber As String = ""
    Private sSMSProviderID As Integer = 0
    Private sSMSProviderName As String = ""
    Private nPageSize As Long = 0

    Public bEnableSMS As Boolean = False

    Public bCanSaveProjects As Boolean = False
    Public bCanSaveDefaultEmail As Boolean = False
    Public bCanHaveSMS As Boolean = False
    Public bCanUseLocalNotes As Boolean = False

    Public bDemoUser As Boolean = False
    Public bMarketingUser As Boolean = False
    Public bHasServerNotes As Boolean = False
    Public bHasStandardCloudNotes As Boolean = False

    Public bEnableTelTags As Boolean = False
    Public bIsPhoneUnique As Boolean = False

    Public currentActiveTab As String = ""
    Public previousActiveTab As String = ""

    Private nMaxWidth As Long = 0

    Private nMPMPrefID As Long = 0

    Private localDatalayer As preferencesDataLayer

    Public Const sEvoPreferencesText As String = " Evolution "
    Public Const sAeroPreferencesText As String = " Aerodex "
    Public Const sRotoPreferencesText As String = " Rotodex "
    Public Const sHeliPreferencesText As String = " Helidex "
    Public Const sYachtPreferencesText As String = " YachtSpot "
    Public Const sCRMPreferencesText As String = " MPM "
    Public Const sAdminPreferencesText As String = " Customer Center "
    Public Const sHomebasePreferencesText As String = " Homebase "
    Public Const sAircraftLabel As String = " Aircraft "
    Public Const sYachtLabel As String = " Yachts "

    Public sPreferencesSiteTitle As String = ""

    Public nSupportTabIndex As Integer = 0
    Public nNotesTabIndex As Integer = 0
    Public nServicesTabIndex As Integer = 0

    Public bIsSiteChatEnabled As Boolean = False
    Public bUseValues As Boolean = False

    Protected PostBackStrDirty As String = ""
    Protected PostBackStrSave As String = ""

#Region "page_functions"

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        PostBackStrDirty = Page.ClientScript.GetPostBackEventReference(Me, "bIsPageDirty")
        PostBackStrSave = Page.ClientScript.GetPostBackEventReference(Me, "bSavePage")

        If Not IsPostBack Then

            If IsNothing(HttpContext.Current.Session.Item("currentDefaultAirportFolderID")) Then
                HttpContext.Current.Session.Item("currentDefaultAirportFolderID") = 0
            End If

        End If

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim selectedFolderRow As String = ""
        Dim selectedTemplateRow As String = ""

        Try

            If Session.Item("crmUserLogon") <> True Then
                Response.Redirect("Default.aspx", True)
            Else


                currentActiveTab = tab_container_ID.ActiveTab.ID

                bIsSiteChatEnabled = CBool(My.Settings.enableChat)


                Dim results_table As New DataTable
                Dim combined_table As New DataTable

                aclsData_Temp = New clsData_Manager_SQL
                aclsData_Temp.JETNET_DB = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
                aclsData_Temp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim

                localDatalayer = New preferencesDataLayer
                localDatalayer.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

                Dim fSubins_platform_os As String = commonEvo.getBrowserCapabilities(Request.Browser)

                If Not (fSubins_platform_os.Contains("win") Or fSubins_platform_os.Contains("mac") Or fSubins_platform_os.Contains("linux")) Then
                    bEnableTelTags = True
                End If

                Dim sErrorString As String = ""

                If Not Session.Item("localPreferences").loadUserSession(sErrorString, CLng(HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString),
                                                                        HttpContext.Current.Session.Item("localUser").crmUserLogin.ToString,
                                                                        CLng(HttpContext.Current.Session.Item("localUser").crmSubSeqNo.ToString),
                                                                        CLng(HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString)) Then
                    Response.Redirect("Default.aspx", True)
                End If

                If Not IsNothing(Session.Item("localPreferences").sessionGUID) Then
                    If String.IsNullOrEmpty(Session.Item("localPreferences").sessionGUID.ToString.Trim) Then
                        commonLogFunctions.Log_User_Event_Data("UserPreferences", "User has empty SESSION GUID SUB[" + HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString + "] LOGIN[" + HttpContext.Current.Session.Item("localUser").crmUserLogin.ToString.Trim + "] SEQ[" + HttpContext.Current.Session.Item("localUser").crmSubSeqNo.ToString + " CONTID[" + HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString + "]", Nothing, , , , CLng(HttpContext.Current.Session.Item("localUser").crmUserCompanyID.ToString), CLng(HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString))
                    End If
                Else
                    commonLogFunctions.Log_User_Event_Data("UserPreferences", "User has empty SESSION OBJECT SUB[" + HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString + "] LOGIN[" + HttpContext.Current.Session.Item("localUser").crmUserLogin.ToString.Trim + "] SEQ[" + HttpContext.Current.Session.Item("localUser").crmSubSeqNo.ToString + " CONTID[" + HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString + "]", Nothing, , , , CLng(HttpContext.Current.Session.Item("localUser").crmUserCompanyID.ToString), CLng(HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString))
                End If

                localDatalayer.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
                localDatalayer.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
                localDatalayer.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
                localDatalayer.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim

                If Session.Item("jetnetWebHostType") = crmWebClient.eWebHostTypes.EVOLUTION Then

                    If Session.Item("localPreferences").AerodexFlag Then
                        If Session.Item("localPreferences").isHeliOnlyProduct Then
                            Master.SetPageTitle("My" + sRotoPreferencesText + "Preferences")
                            sPreferencesSiteTitle = sRotoPreferencesText.Trim
                        Else
                            Master.SetPageTitle("My" + sAeroPreferencesText + "Preferences")
                            sPreferencesSiteTitle = sAeroPreferencesText.Trim
                        End If
                    Else
                        If Session.Item("localPreferences").isHeliOnlyProduct Then
                            Master.SetPageTitle("My" + sHeliPreferencesText + "Preferences")
                            sPreferencesSiteTitle = sHeliPreferencesText.Trim
                        Else
                            Master.SetPageTitle("My" + sEvoPreferencesText + "Preferences")
                            sPreferencesSiteTitle = sEvoPreferencesText.Trim
                        End If
                    End If

                ElseIf Session.Item("jetnetWebHostType") = crmWebClient.eWebHostTypes.YACHT Then
                    Master.SetPageTitle("My" + sYachtPreferencesText + "Preferences")
                    sPreferencesSiteTitle = sYachtPreferencesText.Trim

                ElseIf Session.Item("jetnetWebHostType") = crmWebClient.eWebHostTypes.CRM Then
                    Master.SetPageTitle("My" + sCRMPreferencesText + "Preferences")
                    sPreferencesSiteTitle = sCRMPreferencesText.Trim

                ElseIf Session.Item("jetnetWebHostType") = crmWebClient.eWebHostTypes.ADMIN Then
                    Master.SetPageTitle("My" + sAdminPreferencesText + "Preferences")
                    sPreferencesSiteTitle = sAdminPreferencesText.Trim
                ElseIf Session.Item("jetnetWebHostType") = crmWebClient.eWebHostTypes.HOMEBASE Then
                    Master.SetPageTitle("My" + sHomebasePreferencesText + "Preferences")
                    sPreferencesSiteTitle = sAdminPreferencesText.Trim
                End If

                ' set flags to control display ...
                bCanSaveProjects = Session.Item("localPreferences").SaveProjectsFlag
                bCanSaveDefaultEmail = False 'Session.Item("localPreferences").EmailRequestFlag
                bCanHaveSMS = Session.Item("localPreferences").EnableTextFlag

                bDemoUser = Session.Item("localPreferences").DemoFlag
                bMarketingUser = Session.Item("localPreferences").MarketingFlag

                bCanUseLocalNotes = Session.Item("localPreferences").EnableNotesFlag

                bHasStandardCloudNotes = Session.Item("localPreferences").HasCloudNotes
                bHasServerNotes = Session.Item("localPreferences").HasServerNotes

                bEnableMobile = Session.Item("localPreferences").MobleWebStatus

                bEnableChat = Session.Item("localPreferences").ChatEnabled

                bEnableGLOBALListings = Session.Item("localPreferences").ShowListingsOnGlobal

                bUseValues = clsGeneral.clsGeneral.isEValuesAvailable()

                'checkForDefaultCompRelationship()

                'removed
                display_defaults_ac_relationship_cell.Visible = False
                display_default_view_cell.Visible = False
                display_default_view_cell_1.Visible = False

                ' enable/disable ares not shown for various webhost types
                If Session.Item("jetnetWebHostType") = crmWebClient.eWebHostTypes.EVOLUTION Or
                   Session.Item("jetnetWebHostType") = crmWebClient.eWebHostTypes.HOMEBASE Or
                   Session.Item("jetnetWebHostType") = crmWebClient.eWebHostTypes.ADMIN Then

                    subscription_yacht.Visible = True
                    display_defaults_label.Text = "Default Model / Background"
                    tab_container_ID.Tabs(4).Visible = False ' default regions tab

                    If bCanUseLocalNotes Then
                        nSupportTabIndex = 6
                    Else
                        nSupportTabIndex = 5
                    End If

                    If bCanUseLocalNotes Then
                        nNotesTabIndex = 5
                    End If

                    nServicesTabIndex = 4

                    'This option needs to be removed for aerodex
                    If Session.Item("localPreferences").AerodexFlag Then
                        display_business_segment_ddl.Items.Remove(display_business_segment_ddl.Items.FindByText("Dealer/Broker"))
                    End If

                    If Session.Item("localUser").crmUserType = eUserTypes.ADMINISTRATOR And CBool(Session.Item("localUser").crmUser_Evo_MPM_Flag.ToString) Then
                        tab_container_ID.Tabs(11).Visible = True ' company prefs
                        tab_container_ID.Tabs(12).Visible = True ' features
                    Else
                        tab_container_ID.Tabs(11).Visible = False ' company prefs
                        tab_container_ID.Tabs(12).Visible = False ' features
                    End If


                    If bUseValues Then
                        display_values.Visible = True
                    Else
                        display_values.Visible = False
                    End If

                    If (Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER) Or (Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE) Then

                        my_modules.Visible = True
                        BuildModuleTab()
                        BuildHomeBaseUserBlock()
                        homebaseUserInformationPanel.Visible = True ' Display block about homebase user.
                    ElseIf Session.Item("jetnetWebHostType") = crmWebClient.eWebHostTypes.EVOLUTION Then
                        'If HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.LOCAL Or HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.TEST Then
                        my_modules.Visible = True
                        BuildEvolutionModuleTab()
                        'End If
                    End If

                ElseIf Session.Item("jetnetWebHostType") = crmWebClient.eWebHostTypes.YACHT Then

                    ' account tab
                    subscription_tier.Visible = False
                    subscription_aerodex.Visible = False
                    subscription_business.Visible = False
                    subscription_helicopter.Visible = False
                    subscription_commercial.Visible = False
                    subscription_spi_view.Visible = False
                    subscription_star.Visible = False
                    subscription_service_code.Visible = False

                    subscription_default_model.Visible = False
                    subscription_default_business_segment.Visible = False
                    display_defaults_label.Text = "Background"

                    ' display tab
                    display_defaults_model_view_bkground_text_cell.Visible = False

                    display_default_view_cell.Visible = False
                    display_default_view_cell_1.Visible = False

                    display_default_model_cell.Visible = False
                    display_default_model_cell_1.Visible = False

                    tab_container_ID.Tabs(2).Visible = False ' default models tab
                    tab_container_ID.Tabs(3).Visible = False ' default airports tab
                    tab_container_ID.Tabs(4).Visible = False ' default regions tab
                    tab_container_ID.Tabs(11).Visible = False ' company prefs
                    tab_container_ID.Tabs(12).Visible = False ' features

                    If bCanUseLocalNotes Then
                        nSupportTabIndex = 4
                    Else
                        nSupportTabIndex = 3
                    End If

                    If bCanUseLocalNotes Then
                        nNotesTabIndex = 3
                    End If

                    nServicesTabIndex = 2

                ElseIf Session.Item("jetnetWebHostType") = crmWebClient.eWebHostTypes.CRM Then
                ElseIf Session.Item("jetnetWebHostType") = crmWebClient.eWebHostTypes.ADMIN Then
                End If




                fillInSubscriptionInfo()

                If tab_container_ID.ActiveTabIndex = 0 Then

                    fillInCompanyInfo()

                    fillInContactInfo()

                    fillInCustomerRepInfo()

                    ' has to be set everytime page loads
                    notes_notes_options.Enabled = False

                    Dim bHasNoAutologonCookie As Boolean = False
                    Dim bAutologonFlag As Boolean = False

                    Dim bHasNoBlankAcFieldsCookie As Boolean = False
                    Dim bShowBlankAcFields As Boolean = False

                    bAutologonFlag = commonEvo.getUserAutoLogonCookies(Application.Item("crmClientSiteData").AutoLogonCookie, bHasNoAutologonCookie)
                    bShowBlankAcFields = commonEvo.getUserShowBlankACFields(Session.Item("ShowCondensedAcFormat"), bHasNoBlankAcFieldsCookie)
                    myservices_enable_global_list_ck.Checked = bEnableGLOBALListings

                    Dim bHasNoValuesCookie As Boolean = False

                    If bUseValues Then
                        display_valuesChk.Checked = commonEvo.getUserValuesCookie("evalues", bHasNoValuesCookie)
                        If bHasNoValuesCookie Then
                            HttpContext.Current.Response.Cookies("evalues").Value = "false"
                            HttpContext.Current.Response.Cookies("evalues").Expires = DateTime.Now.AddDays(300)
                        End If
                    End If


                    If Not IsPostBack Then

                        Dim ToggleCookie As HttpCookie = Request.Cookies("tellAboutChanges")

                        If Not IsNothing(ToggleCookie) Then
                            If ToggleCookie.Value = "true" Then
                                disableTellAbout.Checked = True
                            End If
                        End If

                        If bAutologonFlag Then
                            actinfo_auto_login_checkbox.Checked = True
                        End If

                        If bShowBlankAcFields Then
                            display_no_blank_fields_on_aircraft_ddl.SelectedValue = "EF"
                        Else
                            display_no_blank_fields_on_aircraft_ddl.SelectedValue = "CF"
                        End If

                        If bHasNoAutologonCookie Then
                            actinfo_auto_login_checkbox.Text = "Enable Auto Logon by checking '<em>Auto Logon to Jetnet</em>' on login page"
                            actinfo_auto_login_checkbox.Checked = False
                            actinfo_auto_login_checkbox.Enabled = False
                        End If


                        'Records per page.
                        Session.Item("localUser").crmUserRecsPerPage = Math.Round(Session.Item("localUser").crmUserRecsPerPage / 10.0) * 10

                        'Making sure 100 is max (for now at least)
                        If Session.Item("localUser").crmUserRecsPerPage > 100 Then
                            Session.Item("localUser").crmUserRecsPerPage = 100
                        End If

                        If Not String.IsNullOrEmpty(Session.Item("localPreferences").BusinessSegment.trim) Then
                            'We need to add a check in here, it's going to check to see if the user is Aerodex and if it is, it's going to
                            'Check and see if the business segment is DB. If it is, it's going to default it to FB instead. DB doesn't make much sense for 
                            'aerodex.
                            If Session.Item("localPreferences").AerodexFlag And Session.Item("localPreferences").BusinessSegment.trim = "DB" Then
                                display_business_segment_ddl.SelectedValue = "FB"
                            Else
                                display_business_segment_ddl.SelectedValue = Session.Item("localPreferences").BusinessSegment.trim
                            End If
                        End If

                        display_records_per_page_ddl.SelectedValue = Session.Item("localUser").crmUserRecsPerPage.ToString

                        default_analysis_months_ddl.SelectedValue = Session.Item("localPreferences").DefaultAnalysisMonths.ToString

                        'Background ID
                        choose_default_backgroundID.SelectedValue = Session.Item("localUser").crmLocalUser_Background_ID.ToString

                        If CLng(Session.Item("localUser").crmLocalUser_Background_ID) > 0 Then
                            subscription_default_background.Text = "Background:&nbsp;<em>" + choose_default_backgroundID.SelectedItem.Text + "</em>"
                            display_default_backgroundID.Text = choose_default_backgroundID.SelectedItem.Text
                        Else
                            subscription_default_background.Text = "Background:&nbsp;<em>Random</em>"
                            display_default_backgroundID.Text = "Random"
                        End If

                        If bCanUseLocalNotes Then

                            If bHasServerNotes Or bHasStandardCloudNotes Then
                                myservernotes_ac_notes_listing_ck.Enabled = True
                                myservernotes_ac_notes_listing_ck.Checked = bHasAcNotesOnListing
                            End If

                            If bHasServerNotes Then
                                ' notes_notes_options.SelectedValue = "Cloud Notes Plus"
                                notes_notes_options.SelectedIndex = 2
                            ElseIf bHasStandardCloudNotes Then
                                ' notes_notes_options.SelectedValue = "Standard Cloud Notes"
                                notes_notes_options.SelectedIndex = 1
                            Else
                                notes_notes_options.SelectedIndex = 0
                                'notes_notes_options.SelectedValue = ""
                                myservernotes_ac_notes_listing_ck.Visible = False
                                myservernotes_ac_notes_listing_img.Visible = False
                            End If

                            Dim textNotesLabel As String = IIf(Session.Item("jetnetWebHostType") <> crmWebClient.eWebHostTypes.YACHT, sAircraftLabel, sYachtLabel)

                            If Session.Item("localUser").crmUserType = eUserTypes.ADMINISTRATOR Then

                                If String.IsNullOrEmpty(notes_notes_options.SelectedValue.Trim) Then
                                    notes_admin_text.Text = "<p>JETNET provides subscribers with several different options for storing notes and action items within " + sPreferencesSiteTitle + " related products.</p>"

                                    '"<p>For more details about these options click"
                                    ' notes_admin_text.Text += "<a href=""#"" onclick=""javascript:load('help.aspx?id=349','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');"">here</a>.</p>"
                                    notes_admin_text.Text += "<p>Standard Cloud Notes is available to subscribers <strong>FREE</strong> of charge and allows all users under a given subscription to add, "
                                    notes_admin_text.Text += "update, delete, and view notes and action items relating to " + textNotesLabel.Trim + ". All notes and action items for" + textNotesLabel + "using this service will "
                                    notes_admin_text.Text += "be shared with other members of your designated subscription. </p>"
                                    notes_admin_text.Text += "<p class=""emphasis_text"">Contact JETNET Customer Service at <a href='mailto:customerservice@jetnet.com'>customerservice@jetnet.com</a> or 1-(800)-553-8638<br />to have your <b>cloud notes service turned on for this subscription</b>.</p>"
                                    notes_admin_text.Visible = True

                                ElseIf notes_notes_options.SelectedValue.ToLower = "standard" Then

                                    notes_admin_text.Text = "<p>Your subscription currently has Standard Cloud Notes allowing you to add, update, delete, and view notes and action items relating to " + textNotesLabel.Trim + ". If you would like the ability to also add, update, delete, and view notes and action items relating to companies you may want to consider upgrading to Cloud Notes Plus. </p>"
                                    'notes_admin_text.Text += "<p>For more details about Cloud Notes Plus click <a href=""#"" onclick=""javascript:load('help.aspx?id=349','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');"">here</a>. As subscription administrator, you may turn off standard cloud notes for your subscription by unchecking the box below and clicking Save."
                                    notes_admin_text.Text += "<p class=""emphasis_text"">For more details about Cloud Notes Plus contact JETNET Customer Service at<br /><a href='mailto:customerservice@jetnet.com'>customerservice@jetnet.com</a> or 1-(800)-553-8638.</p>"
                                    notes_admin_text.Visible = True

                                ElseIf notes_notes_options.SelectedValue.ToLower = "plus" Then

                                    notes_admin_text.Text = "<p>Your subscription currently has Standard Cloud Notes Plus option allowing you to add, update, delete, and view notes and action items relating to " + textNotesLabel + " and/or companies.</p>"
                                    notes_admin_text.Visible = True

                                End If

                            Else

                                If String.IsNullOrEmpty(notes_notes_options.SelectedValue.Trim) Then

                                    notes_admin_text.Text = "<p>JETNET provides subscribers with several different options for storing notes and action items within " + sPreferencesSiteTitle + " related products.</p>"

                                    '"<p>For more details about these options click"
                                    '  notes_admin_text.Text += "<a href=""#"" onclick=""javascript:load('help.aspx?id=349','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');"">here</a>.</p>"

                                    notes_admin_text.Text += "<p>Standard Cloud Notes is available to subscribers <strong>FREE</strong> of charge and allows all users under a given subscription to add, "
                                    notes_admin_text.Text += "update, delete, and view notes and action items relating to" + textNotesLabel.Trim + ". All notes and action items for" + textNotesLabel + "using this service will "
                                    notes_admin_text.Text += "be shared with other members of your designated subscription.</p>"
                                    notes_admin_text.Text += "<p class=""emphasis_text"">Contact JETNET Customer Service at <a href='mailto:customerservice@jetnet.com'>customerservice@jetnet.com</a> or 1-(800)-553-8638<br />to have your <b>cloud notes service turned on for this subscription</b>.</p>"
                                    notes_admin_text.Visible = True

                                ElseIf notes_notes_options.SelectedValue.ToLower = "standard" Then

                                    notes_admin_text.Text = "<p>Your subscription currently has Standard Cloud Notes allowing you to add, update, delete, and view notes and action items relating to" + textNotesLabel.Trim + ". If you would like the ability to also add, update, delete, and view notes and action items relating to companies you may want to consider upgrading to Cloud Notes Plus. </p>"
                                    ' notes_admin_text.Text += "<p>For more details about Cloud Notes Plus click <a href=""#"" onclick=""javascript:load('help.aspx?id=349','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');"">here</a>.</p>"
                                    notes_admin_text.Text += "<p class=""emphasis_text"">For more details about these options contact JETNET Customer Service at<br /><a href='mailto:customerservice@jetnet.com'>customerservice@jetnet.com</a> or 1-(800)-553-8638.</p>"
                                    notes_admin_text.Visible = True

                                ElseIf notes_notes_options.SelectedValue.ToLower = "plus" Then

                                    notes_admin_text.Text = "<p>Your subscription currently has Standard Cloud Notes Plus option allowing you to add, update, delete, and view notes and action items relating to " + textNotesLabel + " and/or companies.</p>"
                                    notes_admin_text.Visible = True

                                End If
                            End If

                        Else
                            tab_container_ID.Tabs(5).Visible = False ' notes tab
                        End If

                        If bEnableTelTags Then
                            subscription_mobile_number.Text = "Mobile #: <a href='tel:" + sMobileNumber.ToString + "'>" + sMobileNumber.ToString + "</a>"
                        Else
                            subscription_mobile_number.Text = "Mobile #: " + sMobileNumber.ToString
                        End If

                        If (CBool(My.Settings.enableChat)) Then

                            If bEnableChat Then
                                myservices_enable_chat_ck.Checked = bEnableChat
                                subscription_chat_enabled.Text = "Chat Enabled: <em>" + IIf(bEnableChat, "True", "False") + "</em>"
                            End If

                        End If

                        subscription_show_on_global.Text = "Show Listings on JETNET GLOBAL Enabled: <em>" + IIf(bEnableGLOBALListings, "True", "False") + "</em>"

                        If bCanSaveDefaultEmail Then
                            subscription_default_reply_email.Text = "Reply Email: <em>" + sEmailAddress.Trim + "</em>"
                            subscription_default_reply_name.Text = "Reply Name: <em>" + sEmailName.Trim + "</em>"
                            subscription_default_email_format.Text = "Email Format: <em>" + sEmailFormat.Trim + "</em>"

                            myservice_email_address_txt.Text = sEmailAddress.Trim
                            myservices_email_name_txt.Text = sEmailName.Trim

                            If sEmailFormat.Trim.ToUpper = "HTML" Then
                                myservices_email_format_html.Checked = True
                                myservices_email_format_text.Checked = False
                            Else
                                myservices_email_format_html.Checked = False
                                myservices_email_format_text.Checked = True
                            End If

                        End If

                        If bCanHaveSMS And Not CBool(Session.Item("localPreferences").AerodexFlag.ToString) Then

                            myservices_sms_phone_number.Text = sSMSNumber.Trim

                            bIsPhoneUnique = commonEvo.CheckUniquePhoneNumber(Me.Session, sSMSNumber)

                            subscription_SMS_provider.Text = "SMS Provider: <em>" + sSMSProviderName + "</em>"

                            localDatalayer.fillSMSProviderDropDown(myservices_SMS_providers, 0, sSMSProviderID.ToString)

                            ' fill listbox with models
                            commonEvo.fillMakeModelDropDown(Nothing, myservices_models_to_monitor, 0, "", sSMSSelectedModelID, False, False, True, False, False, False)

                            ' fill listbox with events
                            localDatalayer.fillSMSEventsDropDown(myservices_events_to_monitor, 0, "", sSMSSelectedEvents, False)

                            Select Case (sSMSActivationStatus)
                                Case Constants.SMS_ACTIVATE_YES
                                    bEnableSMS = True
                                    myservices_enable_SMS_ck.Checked = bEnableSMS
                                    myservices_terms_and_conditions_ck.Checked = True
                                    myservices_terms_and_conditions_ck.Enabled = False

                                Case Constants.SMS_ACTIVATE_NO
                                    bEnableSMS = False
                                    myservices_enable_SMS_ck.Checked = bEnableSMS

                                Case Constants.SMS_ACTIVATE_PENDING

                                    bEnableSMS = False

                                    myservices_enable_SMS_ck.Enabled = False
                                    myservices_terms_and_conditions_ck.Checked = True
                                    myservices_terms_and_conditions_ck.Enabled = False

                                Case Constants.SMS_ACTIVATE_WAIT

                                    bEnableSMS = False

                                    myservices_enable_SMS_ck.Enabled = False
                                    myservices_terms_and_conditions_ck.Checked = True
                                    myservices_terms_and_conditions_ck.Enabled = False

                                Case Else
                                    myservices_enable_SMS_ck.Checked = bEnableSMS
                                    sSMSActivationStatus = Constants.SMS_ACTIVATE_NO

                            End Select

                            subscription_SMS_txt_msg.Text = "SMS Text Msg Active: <em>" + IIf(bEnableSMS = True, "True", "False") + "</em>"

                        End If

                    End If ' If Not IsPostBack

                End If

                'If Not IsNothing(Request.Item("tab")) Then
                '  If Not String.IsNullOrEmpty(Request.Item("tab").ToString.Trim) Then
                '    currentActiveTab = Request.Item("tab").ToString.Trim
                '    tab_container_ID.ActiveTabIndex = 3
                '  End If
                'End If

                If Trim(Request("activetab")) <> "" Then
                    tab_container_ID.ActiveTabIndex = Trim(Request("activetab"))
                    currentActiveTab = tab_container_ID.ActiveTabIndex
                End If

                If Not Page.IsPostBack Then
                    If Not String.IsNullOrEmpty(Trim(Request("selected"))) Then
                        tab_container_ID.ActiveTab = FindTabIDOut(Trim(Request("selected")))
                        currentActiveTab = tab_container_ID.ActiveTabIndex
                    End If
                End If


                If Page.IsPostBack Then

                    Dim eventArg As String = Request("__EVENTARGUMENT")

                    If eventArg.Contains("bSavePage") Then

                        If Not IsNothing(Request.Item("previousTab")) Then
                            If Not String.IsNullOrEmpty(Request.Item("previousTab").ToString.Trim) Then
                                previousActiveTab = Request.Item("previousTab").ToString
                            End If
                        End If

                        tab_container_ID.ActiveTab.ID = previousActiveTab.ToLower

                        save_button2_Click(sender, e)

                        tab_container_ID.ActiveTab.ID = currentActiveTab.ToLower

                    ElseIf eventArg.Contains("bIsPageDirty") Then

                        Select Case tab_container_ID.ActiveTab.ID

                            Case "my_display"

                                commonEvo.fillMakeModelDropDown(choose_default_modelID, Nothing, nMaxWidth, "", nDefaultModelID.ToString, False, False, False, True, False, False) ' fill dropdownlist with models

                            Case "my_models"

                                ' fill in master model list 
                                commonEvo.fillMakeModelDropDown(Nothing, models_model_lb, nMaxWidth, "", sDefaultModels, False, False, True, False, False, False) ' fill list with models

                                ' this will take selected models and move to picked models  
                                clsGeneral.clsGeneral.AddBtn_Click(models_models_picked_lb, models_model_lb)

                            Case "my_airports"

                                localDatalayer.fillDefaultAirportsDropDown(default_airport_ddl, nMaxWidth, default_airport_ddl.SelectedValue)

                                If default_airport_ddl.SelectedIndex > 0 Then

                                    LinkButton1.Visible = False
                                    LinkButton2.Visible = True
                                    LinkButton3.Visible = True
                                    default_airport_label.Visible = False

                                    default_airport_ddl.Attributes.Add("onchange", "ShowChangeDefailtAirportButton(" + HttpContext.Current.Session.Item("currentDefaultAirportFolderID").ToString.Trim + ");")
                                    add_ChangeDefaultAirportFolder_Script(default_airport_ddl, LinkButton3, LinkButton2)

                                    default_airport_ddl.Visible = True

                                    LinkButton2.OnClientClick = "openSmallWindowJS(""staticFolderEditor.aspx?folderID=" + default_airport_ddl.SelectedValue.Trim + "&airport=true&default=true"",""StaticFolderEditor"");"

                                    ' if we have a default airpirt folder display contents 
                                    fillDefaultAirportFolderContents()

                                ElseIf (CLng(HttpContext.Current.Session.Item("currentDefaultAirportFolderID").ToString.Trim) > 0 Or default_airport_ddl.Items.Count > 1) And default_airport_ddl.SelectedIndex = 0 Then

                                    LinkButton1.Visible = True

                                    LinkButton2.Visible = False
                                    LinkButton3.Visible = False
                                    default_airport_label.Visible = False

                                    default_airport_ddl.Attributes.Remove("onchange")

                                    default_airport_ddl.Visible = True
                                    airportTable.Text = ""

                                Else
                                    LinkButton1.Visible = True
                                    default_airport_label.Text = "You currently have no default airports identified. To identify a default airport list please click on ""Add Default Airport Folder""."
                                End If

                            Case "my_users"

                                fillSubscriberUserList()

                            Case "my_folders"

                                If Session.Item("localUser").crmUserType <> eUserTypes.ADMINISTRATOR Then
                                    If DropDownList2.Items.Count > 1 Then
                                        DropDownList2.Items.RemoveAt(1)
                                    End If
                                End If

                                localDatalayer.fillFoldersDropDown(DropDownList1, nMaxWidth, DropDownList1.SelectedValue, DropDownList2.SelectedValue)

                                fillSubscriberFolderList()

                            Case "my_templates"


                                If Session.Item("localUser").crmUserType <> eUserTypes.ADMINISTRATOR Then
                                    If DropDownList4.Items.Count > 1 Then
                                        DropDownList4.Items.RemoveAt(1)
                                    End If
                                End If

                                localDatalayer.fillTemplatesDropDown(DropDownList3, nMaxWidth, DropDownList3.SelectedValue, DropDownList4.SelectedValue)

                                fillSubscriberTemplateList()

                        End Select

                    Else ' if neither eventArg.Contains("bIsPageDirty") or eventArg.Contains("bSavePage") Then

                        Select Case tab_container_ID.ActiveTab.ID
                            Case "my_airports"

                                localDatalayer.fillDefaultAirportsDropDown(default_airport_ddl, nMaxWidth, default_airport_ddl.SelectedValue)

                                If default_airport_ddl.SelectedIndex > 0 Then

                                    LinkButton1.Visible = False
                                    LinkButton2.Visible = True
                                    LinkButton3.Visible = True
                                    default_airport_label.Visible = False

                                    default_airport_ddl.Attributes.Add("onchange", "ShowChangeDefailtAirportButton(" + HttpContext.Current.Session.Item("currentDefaultAirportFolderID").ToString.Trim + ");")
                                    add_ChangeDefaultAirportFolder_Script(default_airport_ddl, LinkButton3, LinkButton2)
                                    default_airport_ddl.Visible = True

                                    LinkButton2.OnClientClick = "openSmallWindowJS(""staticFolderEditor.aspx?folderID=" + default_airport_ddl.SelectedValue.Trim + "&airport=true&default=true"",""StaticFolderEditor"");"

                                    ' if we have a default airpirt folder display contents
                                    fillDefaultAirportFolderContents()

                                ElseIf (CLng(HttpContext.Current.Session.Item("currentDefaultAirportFolderID").ToString.Trim) > 0 Or default_airport_ddl.Items.Count > 1) And default_airport_ddl.SelectedIndex = 0 Then

                                    LinkButton1.Visible = True
                                    LinkButton2.Visible = False
                                    LinkButton3.Visible = False
                                    default_airport_label.Visible = False

                                    default_airport_ddl.Attributes.Remove("onchange")
                                    default_airport_ddl.Visible = True
                                    airportTable.Text = ""

                                Else
                                    LinkButton1.Visible = True
                                    default_airport_label.Text = "You currently have no default airports identified. To identify a default airport list please click on Add Default Airport Folder."
                                End If

                            Case "my_folders"

                                If Session.Item("localUser").crmUserType <> eUserTypes.ADMINISTRATOR Then
                                    If DropDownList2.Items.Count > 1 Then
                                        DropDownList2.Items.RemoveAt(1)
                                    End If
                                End If

                                localDatalayer.fillFoldersDropDown(DropDownList1, nMaxWidth, DropDownList1.SelectedValue, DropDownList2.SelectedValue)

                                fillSubscriberFolderList()

                            Case "my_templates"

                                If Session.Item("localUser").crmUserType <> eUserTypes.ADMINISTRATOR Then
                                    If DropDownList4.Items.Count > 1 Then
                                        DropDownList4.Items.RemoveAt(1)
                                    End If
                                End If

                                localDatalayer.fillTemplatesDropDown(DropDownList3, nMaxWidth, DropDownList3.SelectedValue, DropDownList4.SelectedValue)

                                fillSubscriberTemplateList()

                        End Select

                    End If ' eventArg.Contains("bIsPageDirty") Then

                Else

                    If (Not IsNothing(Request.Item("activetab")) Or Not IsNothing(Request.Item("selected"))) And tab_container_ID.ActiveTab.ID = "my_airports" Then

                        localDatalayer.fillDefaultAirportsDropDown(default_airport_ddl, nMaxWidth, default_airport_ddl.SelectedValue)

                        If default_airport_ddl.SelectedIndex > 0 Then

                            LinkButton1.Visible = False
                            LinkButton2.Visible = True
                            LinkButton3.Visible = True

                            default_airport_label.Visible = False

                            default_airport_ddl.Attributes.Add("onchange", "ShowChangeDefailtAirportButton(" + HttpContext.Current.Session.Item("currentDefaultAirportFolderID").ToString.Trim + ");")
                            add_ChangeDefaultAirportFolder_Script(default_airport_ddl, LinkButton3, LinkButton2)
                            default_airport_ddl.Visible = True

                            LinkButton2.OnClientClick = "openSmallWindowJS(""staticFolderEditor.aspx?folderID=" + default_airport_ddl.SelectedValue.Trim + "&airport=true&default=true"",""StaticFolderEditor"");"

                            ' if we have a default airpirt folder display contents
                            fillDefaultAirportFolderContents()

                        ElseIf (CLng(HttpContext.Current.Session.Item("currentDefaultAirportFolderID").ToString.Trim) > 0 Or default_airport_ddl.Items.Count > 1) And default_airport_ddl.SelectedIndex = 0 Then

                            LinkButton1.Visible = True
                            LinkButton2.Visible = False
                            LinkButton3.Visible = False

                            default_airport_label.Visible = False

                            default_airport_ddl.Attributes.Remove("onchange")
                            default_airport_ddl.Visible = True
                            airportTable.Text = ""

                        Else
                            LinkButton1.Visible = True
                            default_airport_label.Text = "You currently have no default airports identified. To identify a default airport list please click on Add Default Airport Folder."
                        End If
                    ElseIf (Not IsNothing(Request.Item("activetab")) Or Not IsNothing(Request.Item("selected"))) And tab_container_ID.ActiveTab.ID = "my_users" Then

                        If Not IsNothing(Request.Item("returnstring")) Then
                            If Not String.IsNullOrEmpty(Request.Item("returnstring").ToString.Trim) Then

                                Dim data() As Byte = System.Convert.FromBase64String(Request.Item("returnstring").ToString)
                                Dim base64Decoded As String = System.Text.ASCIIEncoding.ASCII.GetString(data)

                                Dim passwordArray() As String = base64Decoded.Split(",")

                                my_users_email_response.Text = "<div style=""color:red""><strong>" + sendUserEmail(passwordArray(0), passwordArray(1), passwordArray(2), passwordArray(3), passwordArray(4)) + "</strong></div>"

                            End If

                        End If

                        fillSubscriberUserList()

                        Dim isreadonly As PropertyInfo = GetType(NameValueCollection).GetProperty("IsReadOnly", BindingFlags.Instance Or BindingFlags.NonPublic)

                        isreadonly.SetValue(Request.QueryString, False, Nothing)

                        Request.QueryString.Remove("activetab")
                        Request.QueryString.Remove("returnstring")

                        isreadonly.SetValue(Request.QueryString, True, Nothing)

                    End If

                End If ' Page.IsPostBack Then

                If Not (bDemoUser Or bMarketingUser) Then
                    newPasswordID.Attributes.Add("onblur", "validatePassword();")
                End If

                support_email_textbox.Attributes.Add("onblur", "validateEmailText();")
                add_validateEmailText_Script(support_email_textbox)

                'mydisplay_enabled_default_feature.Attributes.Add("onclick", "setdefaultCompType();")
                'add_SetDefaultCompType_Script(mydisplay_enabled_default_feature, mydisplay_default_relationships, mydisplay_selected_relationships)

                'mydisplay_default_relationships.Attributes.Add("onclick", "ToggleDefCompType();")
                'mydisplay_selected_relationships.Attributes.Add("onclick", "ToggleSelCompType();")
                'add_ToggleDefaultCompRadioButtons_Script(mydisplay_default_relationships, mydisplay_selected_relationships)

                If bCanSaveDefaultEmail Then

                    myservice_email_address_txt.Attributes.Add("onblur", "validateEmailAddress();")
                    add_ValidateEmailAddress_Script(myservice_email_address_txt)

                    myservices_email_format_html.Attributes.Add("onclick", "ToggleEmailFormatHtml();")
                    myservices_email_format_text.Attributes.Add("onclick", "ToggleEmailFormatText();")
                    add_ToggleDefaultEmailRadioButtons_Script(myservices_email_format_html, myservices_email_format_text) '

                End If

                If bCanHaveSMS And Not CBool(Session.Item("localPreferences").AerodexFlag.ToString) Then

                    myservices_sms_phone_number.Attributes.Add("onblur", "validateSMSPhoneNumber();")
                    add_validateSMSPhoneNumber_Script(myservices_sms_phone_number)

                    myservices_enable_SMS_ck.Attributes.Add("onclick", "enableSMS();")
                    add_EnableSMS_Script(myservices_enable_SMS_ck, myservices_sms_phone_number, myservices_SMS_providers, myservices_events_to_monitor, myservices_models_to_monitor, myservices_terms_and_conditions_ck)

                    add_MultiListEnsureItemVisible_Script(myservices_models_to_monitor)

                End If

                If (CBool(My.Settings.enableChat)) Then

                    myservices_enable_chat_ck.Text = "Enable " + sPreferencesSiteTitle + " Chat Service "
                    myservices_chat_img.AlternateText = "check this box to enable " + sPreferencesSiteTitle + " Chat Service and display my status as ONLINE"
                    myservices_chat_img.ToolTip = myservices_chat_img.AlternateText

                End If

                actinfo_auto_login_checkbox.Attributes.Add("onclick", "autologonAlert();")
                add_autologonAlert_Script(actinfo_auto_login_checkbox)

                add_ChangeActiveTab_Script(tab_container_ID)

                actinfo_password_mouseover_img.AlternateText = "Change Subscriber Password:" + vbCrLf + vbCrLf + "New password should be a minimum of 8 characters " + vbCrLf +
                                                             "and must contain *at least*" + vbCrLf + vbCrLf + "one number, one LOWER case and one UPPER case, and one SPECIAL character ( !@#$%^&*()_+=- )"
                actinfo_password_mouseover_img.ToolTip = actinfo_password_mouseover_img.AlternateText

                mydisplay_evalues_img.AlternateText = "Enable display of Values"
                mydisplay_evalues_img.ToolTip = mydisplay_evalues_img.AlternateText

                mydisplay_default_view_img.AlternateText = "Resets default home view for Subscriber homepage"
                mydisplay_default_view_img.ToolTip = mydisplay_default_view_img.AlternateText

                mydisplay_relationship_img.AlternateText = "Effects All Tabs"
                mydisplay_relationship_img.ToolTip = mydisplay_relationship_img.AlternateText

                myservices_SMS_img.AlternateText = "To enable this service and to start receiving text messages check this box" + vbCrLf + vbCrLf + " To stop receiving text messages and to disable this service, uncheck this box"
                myservices_SMS_img.ToolTip = myservices_SMS_img.AlternateText

                myservices_sms_phone_img.AlternateText = sPreferencesSiteTitle + " SMS Msg & Data rates may apply"
                myservices_sms_phone_img.ToolTip = myservices_sms_phone_img.AlternateText

                myservernotes_ac_notes_listing_img.AlternateText = "Enable Notes Indicator on Listings" + vbCrLf + vbCrLf + "Could increase search times for large Notes databases"
                myservernotes_ac_notes_listing_img.ToolTip = myservernotes_ac_notes_listing_img.AlternateText

                models_info_lbl.Text = "Use this form to identify the aircraft model(s) to use as defaults throughout " + sPreferencesSiteTitle + " as your primary aircraft market model(s)."

                regions_info_lbl.Text = "Use this form to identify the aircraft regions(s) of the world to use as defaults throughout " + sPreferencesSiteTitle + " as your primary aircraft market region(s)."

                chat_info_lbl.Text = "Enable " + sPreferencesSiteTitle + " Chat service and display my Online status."

                global_info_lbl.Text = "JETNET Global <strong><a href=""http://www.jetnetglobal.com"" title=""www.jetnetglobal.com"" target=""_blank"">www.jetnetglobal.com</a></strong>"
                global_info_lbl.Text += " is a free public website operated by JETNET and populated with aircraft for sale listings from reputable dealers and brokers. As a service to JETNET customers,"
                global_info_lbl.Text += " JETNET will automatically list brokered aircraft unless the service is shut off by customers by unchecking the box below."
                global_info_lbl.Text += "<p class=""emphasis_text"">This setting can only be changed by administrative users for your subscription.</p>"

                myservices_enable_global_list_ck.Text = "Include my aircraft listings on JETNET Global "
                myservices_global_img.AlternateText = "check this box to enable 'MY' aircraft listings on JETNET Global"
                myservices_global_img.ToolTip = myservices_global_img.AlternateText

                Dim TheFile As System.IO.FileInfo
                Dim contactImageLink As String = ""
                Dim contactImageFile As String = ""
                Dim contactNewImageLink As String = ""
                Dim contactNewImageFile As String = ""
                Dim contactPicID As Long = 0
                Dim imgDisplayFolder As String = HttpContext.Current.Session.Item("jetnetFullHostName") + HttpContext.Current.Session.Item("ContactPicturesFolderVirtualPath")
                contactPicID = localDatalayer.CheckForExistingUserImageRow(Session.Item("localUser").crmUserContactID)

                If contactPicID > 0 Then

                    Try
                        ' rename any "previous" contact image to match new paridigm
                        contactImageLink = Session.Item("ContactPicturesFolderVirtualPath") + "/" + Session.Item("localUser").crmUserContactID.ToString + ".jpg"
                        contactImageFile = HttpContext.Current.Server.MapPath(contactImageLink)

                        contactNewImageLink = Session.Item("ContactPicturesFolderVirtualPath") + "/" + Session.Item("localUser").crmUserContactID.ToString + "-" + contactPicID.ToString + ".jpg"
                        contactNewImageFile = HttpContext.Current.Server.MapPath(contactNewImageLink)

                        TheFile = New System.IO.FileInfo(contactImageFile)

                        If TheFile.Exists Then 'is the file actually there?
                            System.IO.File.Move(contactImageFile, contactNewImageFile) 'rename the file to append the contact picture ID to the name.
                        End If

                        actinfo_contact_image.ImageUrl = imgDisplayFolder.Trim + "/" + Session.Item("localUser").crmUserContactID.ToString + "-" + contactPicID.ToString + ".jpg"
                        actinfo_contact_image_large.Text = "<img src=""" + imgDisplayFolder.Trim + "/" + Session.Item("localUser").crmUserContactID.ToString + "-" + contactPicID.ToString + ".jpg"" alt=""" + actinfo_contact_name.Text.Trim + """  title=""" + actinfo_contact_name.Text.Trim + """ width=""225"" border=""1"" style=""width: 225px; "" />"
                        actinfo_contact_edit_image_button.Visible = True
                        actinfo_contact_edit_image_button_remove.Visible = True
                    Catch ex As Exception
                        actinfo_contact_edit_image_panel.Visible = True
                        actinfo_contact_edit_image_button_remove.Visible = False
                        actinfo_contact_image.ImageUrl = "images/contact.jpg"
                        actinfo_contact_image_large.Text = "<img src=""images/person.jpg"" alt=""" + actinfo_contact_name.Text.Trim + """  title=""" + actinfo_contact_name.Text.Trim + """ width=""225"" border=""1"" style=""width: 225px; "" />"
                        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br/><br/>Preferences File Error: " + ex.Message.ToString() + "<br/>"
                    End Try

                Else
                    actinfo_contact_edit_image_panel.Visible = True
                    actinfo_contact_edit_image_button_remove.Visible = False
                    actinfo_contact_image.ImageUrl = "images/contact.jpg"
                    actinfo_contact_image_large.Text = "<img src=""images/person.jpg"" alt=""" + actinfo_contact_name.Text.Trim + """  title=""" + actinfo_contact_name.Text.Trim + """ width=""225"" border=""1"" style=""width: 225px; "" />"
                End If

            End If




            If (Trim(Request("activetab")) = "2" And IsPostBack = False) Or (Trim(Request("selected")) = "models" And IsPostBack = False) Then
                ' fill in master model list 
                commonEvo.fillMakeModelDropDown(Nothing, models_model_lb, nMaxWidth, "", sDefaultModels, False, False, True, False, False, False) ' fill list with models

                ' this will take selected models and move to picked models  
                clsGeneral.clsGeneral.AddBtn_Click(models_models_picked_lb, models_model_lb)
            End If

        Catch ex As Exception
            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br/><br/>Preferences Page Load Error: " + ex.Message.ToString() + "<br/>"
        End Try

    End Sub
    Private Function FindTabIDOut(TabNameSent As String) As AjaxControlToolkit.TabPanel
        Dim ReturnPanel As New AjaxControlToolkit.TabPanel
        Select Case LCase(TabNameSent)
            Case "display"
                ReturnPanel = my_display
            Case "services"
                ReturnPanel = my_services
            Case "notes"
                ReturnPanel = my_notes
            Case "dashboard"
                ReturnPanel = my_modules
                Dim jsStr As String = ""

                jsStr = "$(""#sortableSave"").removeClass(""display_none"");" + vbCrLf
                jsStr += "$(""#" & save_button2.ClientID & """).addClass(""display_none"");" + vbCrLf
                jsStr += "$(""#" & save_button1.ClientID & """).attr(""style"", ""display: none!important;"");" + vbCrLf
                jsStr += "$(""#sortSaveTop"").removeAttr(""style"");" + vbCrLf

                System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType, "sortableScriptButtonToggle", jsStr + vbCrLf, True)
            Case "folders"
                ReturnPanel = my_folders
            Case "templates"
                ReturnPanel = my_templates
            Case "models"
                ReturnPanel = my_models
            Case "airports"
                ReturnPanel = my_airports
            Case "support"
                ReturnPanel = my_support
            Case Else 'Default to account
                ReturnPanel = my_account
        End Select

        'Let's add another check
        'If for some reason, they're playing around with the URL
        'They can't select a tab they can't see and throw an error
        If ReturnPanel.Visible = False Then
            ReturnPanel = my_account
        End If

        Return ReturnPanel
    End Function

    Private Sub BuildHomeBaseUserBlock()
        homebaseUser_UserID.Text = "User ID: " & Session.Item("homebaseUserClass").home_user_id
        homebaseUser_AccountID.Text = "Account ID: " & Session.Item("homebaseUserClass").home_account_id
        homebaseUser_userType.Text = "User Type: " & Session.Item("homebaseUserClass").home_user_type
    End Sub

    Private Sub BuildModuleTab()
        Dim adminDataLayer As New admin_center_dataLayer
        Dim moduleData As New DataTable
        adminDataLayer.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
        adminDataLayer.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
        adminDataLayer.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
        adminDataLayer.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
        adminDataLayer.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim
        adminDataLayer.crmMasterConnectStr = HttpContext.Current.Application.Item("crmMasterDatabase").ToString.Trim

        Dim oldArea As String = ""
        Dim chosenIDs As String = ""
        moduleData = New DataTable

        moduleData = adminDataLayer.DashboardModuleList(HttpContext.Current.Session.Item("localUser").crmSubSubID, HttpContext.Current.Session.Item("localUser").crmUserLogin, HttpContext.Current.Session.Item("localUser").crmSubSeqNo)

        sort2.Text = "<div class=""six columns""><h3 class=""moduleHeaderText"">Chosen Dashboard Modules</h3><ul id=""sortable2"" class=""connectedSortable"">"
        oldArea = ""
        If Not IsNothing(moduleData) Then
            If moduleData.Rows.Count > 0 Then

                For Each r As DataRow In moduleData.Rows
                    If chosenIDs <> "" Then
                        chosenIDs += ","
                    End If
                    chosenIDs += r("dashb_id").ToString
                    'If oldArea <> r("dashb_area") Then
                    '  sort2.Text += "<li class=""ui-state-default ui-state-disabled area"">" & r("dashb_area") & "</li>"
                    'End If


                    sort2.Text += "<li class=""ui-state-default indent"" id=""id_" & r("dashb_id").ToString & """>"
                    sort2.Text += r("dashb_display_title")

                    sort2.Text += "</li>"
                    oldArea = r("dashb_area")


                Next

            End If
        End If
        sort2.Text += "</ul>"
        sort2.Text += "</div>"

        '------------------------------------------------------------

        moduleData = adminDataLayer.DashboardSelectionList(chosenIDs)
        oldArea = ""
        ''Available Modules
        sort1.Text = "<div class=""six columns""><h3 class=""moduleHeaderText"">Available Dashboard Modules</h3><ul id=""sortable1"" class=""connectedSortable"">"

        If Not IsNothing(moduleData) Then
            If moduleData.Rows.Count > 0 Then

                For Each r As DataRow In moduleData.Rows

                    If oldArea <> r("dashb_area") Then
                        sort1.Text += "<li class=""ui-state-default ui-state-disabled area"">" & r("dashb_area") & "</li>"
                    End If


                    sort1.Text += "<li class=""ui-state-default indent"" id=""id_" & r("dashb_id").ToString & """>"
                    sort1.Text += r("dashb_display_title")

                    sort1.Text += "</li>"
                    oldArea = r("dashb_area")


                Next

            End If
        End If
        sort1.Text += "</ul>"
        sort1.Text += "</div>"

        Dim sortableSc As String = ""


        sortableSc = " $(function() {"
        sortableSc += "  $(""#sortable1, #sortable2"").sortable({"
        sortableSc += " connectWith: "".connectedSortable"","
        sortableSc += " items: ""li:Not(.ui-state-disabled)"""
        sortableSc += " }).disableSelection();"

        sortableSc += "$(""#sortable2"").sortable({" & vbNewLine
        sortableSc += "update: function (event, ui) {" & vbNewLine
        sortableSc += "var order = $(this).sortable('serialize');" & vbNewLine

        sortableSc += "}" & vbNewLine
        sortableSc += "}).disableSelection();" & vbNewLine

        sortableSc += "$('#sortableSave, #sortSaveTop').on('click', function () {ShowPreferencesMessage('DivPreferencesMessage','Saving Preferences','Saving Preferences ... Please Wait ...');" & vbNewLine
        sortableSc += "var a = $(""#sortable2"").sortable(""serialize"", {"
        sortableSc += "attribute: ""id"""
        sortableSc += "});"
        sortableSc += "$.ajax({"
        sortableSc += "data: a,"
        sortableSc += "type: 'GET',"
        sortableSc += "contentType: ""application/json; charset=utf-8"","
        sortableSc += "dataType: ""json"","
        sortableSc += "url: 'JSONresponse.aspx/DashBoardCreation?'"
        sortableSc += "}).done(function (data) {"
        sortableSc += "ClosePreferencesMessage(""DivPreferencesMessage"");"
        sortableSc += "});"
        sortableSc += "})"
        sortableSc += " });"

        System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType, "sortableScript", sortableSc + vbCrLf, True)


    End Sub

    Private Sub BuildEvolutionModuleTab()
        Dim adminDataLayer As New admin_center_dataLayer
        Dim moduleData As New DataTable
        adminDataLayer.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
        adminDataLayer.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
        adminDataLayer.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
        adminDataLayer.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
        adminDataLayer.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim
        adminDataLayer.crmMasterConnectStr = HttpContext.Current.Application.Item("crmMasterDatabase").ToString.Trim


        Dim oldArea As String = ""
        Dim chosenIDs As String = ""
        moduleData = New DataTable

        moduleData = adminDataLayer.DashboardModuleList(HttpContext.Current.Session.Item("localUser").crmSubSubID, HttpContext.Current.Session.Item("localUser").crmUserLogin, HttpContext.Current.Session.Item("localUser").crmSubSeqNo)

        sort2.Text = "<div class=""six columns""><h3 class=""moduleHeaderText"">Selected Homepage Tiles</h3><ul id=""sortable2"" class=""connectedSortable"">"
        oldArea = ""
        If Not IsNothing(moduleData) Then
            If moduleData.Rows.Count > 0 Then

                For Each r As DataRow In moduleData.Rows
                    If chosenIDs <> "" Then
                        chosenIDs += ","
                    End If
                    chosenIDs += r("dashb_id").ToString

                    sort2.Text += "<li class=""ui-state-default indent"" id=""id_" & r("dashb_id").ToString & """>"
                    sort2.Text += r("dashb_display_title")

                    sort2.Text += "</li>"
                    oldArea = r("dashb_area")


                Next

            End If
        End If
        sort2.Text += "</ul>"
        sort2.Text += "</div>"

        '------------------------------------------------------------

        moduleData = DisplayFunctions.EvolutionDashboardSelectionList(chosenIDs)
        oldArea = ""
        ''Available Modules
        sort1.Text = "<div class=""six columns""><h3 class=""moduleHeaderText"">Available Homepage Tiles</h3><ul id=""sortable1"" class=""connectedSortable"">"

        If Not IsNothing(moduleData) Then
            If moduleData.Rows.Count > 0 Then

                For Each r As DataRow In moduleData.Rows


                    sort1.Text += "<li class=""ui-state-default indent"" id=""id_" & r("dashb_id").ToString & """>"
                    sort1.Text += r("dashb_display_title")

                    sort1.Text += "</li>"
                    oldArea = r("dashb_area")


                Next

            End If
        End If
        sort1.Text += "</ul>"
        sort1.Text += "</div>"

        Dim sortableSc As String = ""


        sortableSc = " $(function() {"
        sortableSc += "  $(""#sortable1, #sortable2"").sortable({"
        sortableSc += " connectWith: "".connectedSortable"","
        sortableSc += " items: ""li:Not(.ui-state-disabled)"""
        sortableSc += " }).disableSelection();"

        sortableSc += "$(""#sortable2"").sortable({" & vbNewLine
        sortableSc += "update: function (event, ui) {" & vbNewLine
        sortableSc += "var order = $(this).sortable('serialize');" & vbNewLine

        sortableSc += "}" & vbNewLine
        sortableSc += "}).disableSelection();" & vbNewLine

        sortableSc += "$('#sortableSave, #sortSaveTop').on('click', function () {ShowPreferencesMessage('DivPreferencesMessage','Saving Preferences','Saving Preferences ... Please Wait ...');" & vbNewLine
        sortableSc += "var a = $(""#sortable2"").sortable(""serialize"", {"
        sortableSc += "attribute: ""id"""
        sortableSc += "});"
        sortableSc += "$.ajax({"
        sortableSc += "data: a,"
        sortableSc += "type: 'GET',"
        sortableSc += "contentType: ""application/json; charset=utf-8"","
        sortableSc += "dataType: ""json"","
        sortableSc += "url: 'JSONresponse.aspx/DashBoardCreation?'"
        sortableSc += "}).done(function (data) {"
        sortableSc += "ClosePreferencesMessage(""DivPreferencesMessage"");"
        sortableSc += "});"
        sortableSc += "})"
        sortableSc += " });"

        System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType, "sortableScript", sortableSc + vbCrLf, True)


    End Sub
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

        If IsPostBack Then

            If (CBool(My.Settings.enableChat)) Then

                If Not IsNothing(Session.Item("bHadOtherChatSubscription")) Then
                    If Not String.IsNullOrEmpty(Session.Item("bHadOtherChatSubscription").ToString) Then
                        If CBool(Session.Item("bHadOtherChatSubscription").ToString) Then
                            bShowChatChangeDialog = True

                            sChatChangeGUID = HttpContext.Current.Session.Item("localPreferences").SessionGUID.ToString.Trim
                            sChatChangeUserAlias = strUserEmailAddress.Trim
                            bChatChangeEnable = myservices_enable_chat_ck.Checked
                            bChatChangeSub = True

                            HttpContext.Current.Session.Item("bHadOtherChatSubscription") = Nothing

                        End If
                    End If
                End If

            End If

        End If

    End Sub

#End Region

#Region "fill functions"

    Public Sub fillInCompanyInfo()
        Dim tempTable As New DataTable
        Dim strType As String = ""
        Dim strPhone As String = ""
        Dim strBusinessType As String = ""

        Try
            tempTable = aclsData_Temp.GetCompanyInfo_ID(Session.Item("localUser").crmUserCompanyID, "JETNET", 0)
            If Not IsNothing(tempTable) Then
                If tempTable.Rows.Count > 0 Then
                    If Not (IsDBNull(tempTable.Rows(0).Item("comp_name"))) And Not String.IsNullOrEmpty(tempTable.Rows(0).Item("comp_name").ToString) Then
                        actinfo_company_name.Text = tempTable.Rows(0).Item("comp_name").ToString.Trim
                    End If

                    If Not (IsDBNull(tempTable.Rows(0).Item("comp_address1"))) And Not String.IsNullOrEmpty(tempTable.Rows(0).Item("comp_address1").ToString) Then
                        actinfo_address1.Text = tempTable.Rows(0).Item("comp_address1").ToString.Trim
                    End If

                    If Not (IsDBNull(tempTable.Rows(0).Item("comp_address2"))) And Not String.IsNullOrEmpty(tempTable.Rows(0).Item("comp_address2").ToString) Then
                        actinfo_address2.Text = tempTable.Rows(0).Item("comp_address2").ToString.Trim
                    End If

                    If Not (IsDBNull(tempTable.Rows(0).Item("comp_city"))) And Not String.IsNullOrEmpty(tempTable.Rows(0).Item("comp_city").ToString) Then
                        actinfo_city.Text = tempTable.Rows(0).Item("comp_city").ToString.Trim
                    End If

                    If Not (IsDBNull(tempTable.Rows(0).Item("comp_state"))) And Not String.IsNullOrEmpty(tempTable.Rows(0).Item("comp_state").ToString) Then
                        actinfo_state.Text = ", " + tempTable.Rows(0).Item("comp_state").ToString.Trim
                    End If

                    If Not (IsDBNull(tempTable.Rows(0).Item("comp_zip_code"))) And Not String.IsNullOrEmpty(tempTable.Rows(0).Item("comp_zip_code").ToString) Then
                        actinfo_zipcode.Text = tempTable.Rows(0).Item("comp_zip_code").ToString.Trim
                    End If

                    If Not (IsDBNull(tempTable.Rows(0).Item("comp_country"))) And Not String.IsNullOrEmpty(tempTable.Rows(0).Item("comp_country").ToString) Then
                        actinfo_country.Text = tempTable.Rows(0).Item("comp_country").ToString.Trim
                    End If

                    If Not (IsDBNull(tempTable.Rows(0).Item("comp_email_address"))) And Not String.IsNullOrEmpty(tempTable.Rows(0).Item("comp_email_address").ToString) Then
                        actinfo_email.Text = "<a href='mailto:" + tempTable.Rows(0).Item("comp_email_address").ToString.Trim + "'>" + tempTable.Rows(0).Item("comp_email_address").ToString.Trim + "</a>"
                    End If

                    If Not (IsDBNull(tempTable.Rows(0).Item("comp_web_address"))) And Not String.IsNullOrEmpty(tempTable.Rows(0).Item("comp_web_address").ToString) Then
                        actinfo_website.Text = "<a href='http://" + tempTable.Rows(0).Item("comp_web_address").ToString.Trim + "' target='_new'>" + tempTable.Rows(0).Item("comp_web_address").ToString.Trim + "</a>"
                    End If

                End If
            Else
                Master.LogError("Error in Preferences.aspx.vb (FillInCompanyInfo) - " & aclsData_Temp.class_error)
            End If

            tempTable = New DataTable
            tempTable = aclsData_Temp.GetPreferencesPhoneNumbers(Session.Item("localUser").crmUserCompanyID, 0, False, "'Office', 'Hangar', 'Residence', 'Fax', 'Hangar Fax', 'Residential Fax', 'Toll Free'")
            If Not IsNothing(tempTable) Then
                If tempTable.Rows.Count > 0 Then
                    For Each r As DataRow In tempTable.Rows
                        'Resetting the type and phone.
                        strType = ""
                        strPhone = ""

                        If Not (IsDBNull(r("pnum_type"))) And Not String.IsNullOrEmpty(r("pnum_type").ToString) Then
                            strType = r("pnum_type").ToString.Trim
                        End If

                        If Not (IsDBNull(r("pnum_number"))) And Not String.IsNullOrEmpty(r("pnum_number").ToString) Then
                            strPhone = r("pnum_number").ToString.Trim
                        End If

                        Select Case strType.ToLower.Trim
                            Case "office", "hangar", "residence"
                                If bEnableTelTags Then
                                    actinfo_office.Text = "Office #: <a href='tel:" + strPhone + "'>" + strPhone + "</a>"
                                Else
                                    actinfo_office.Text = "Office #: " + strPhone
                                End If
                            Case "fax", "hangar fax", "residential fax"
                                If bEnableTelTags Then
                                    actinfo_fax.Text = "Fax #: <a href='tel:" + strPhone + "'>" + strPhone + "</a>"
                                Else
                                    actinfo_fax.Text = "Fax #: " + strPhone
                                End If
                            Case "toll free"
                                If bEnableTelTags Then
                                    actinfo_toll.Text = "Toll Free #: <a href='tel:" + strPhone + "'>" + strPhone + "</a>"
                                Else
                                    actinfo_toll.Text = "Toll Free #: " + strPhone
                                End If
                        End Select
                    Next
                End If
            Else
                Master.LogError("Error in Preferences.aspx.vb (FillInCompanyInfo) - " & aclsData_Temp.class_error)
            End If

            '' get and display business types.
            tempTable = aclsData_Temp.Return_Business_Type(Session.Item("localUser").crmUserCompanyID, 0)
            If Not IsNothing(tempTable) Then
                If tempTable.Rows.Count > 0 Then
                    For Each z As DataRow In tempTable.Rows
                        strBusinessType = strBusinessType & IIf(Not IsDBNull(z("cbus_name")), " " & z("cbus_name") & ",", "")
                    Next
                End If
            Else
                Master.LogError("Error in Preferences.aspx.vb (FillInCompanyInfo) - " & aclsData_Temp.class_error)
            End If
            tempTable = Nothing

            If strBusinessType <> "" Then
                strBusinessType = strBusinessType.TrimEnd(",")
            End If

            actinfo_business_type.Text = "Business Type(s):" & strBusinessType

        Catch ex As Exception
            Master.LogError("Error in Preferences.aspx.vb (FillInCompanyInfo) - " & ex.Message)
        End Try
    End Sub

    Public Sub fillInContactInfo()
        Dim tempTable As New DataTable
        Try
            tempTable = localDatalayer.ReturnUserDetailsAndImage(Session.Item("localUser").crmUserContactID) 'aclsData_Temp.GetContacts_Details(Session.Item("localUser").crmUserContactID, "JETNET", True)
            If Not IsNothing(tempTable) Then
                If tempTable.Rows.Count > 0 Then

                    actinfo_contact_name.Text = ""

                    If Not (IsDBNull(tempTable.Rows(0).Item("contact_sirname"))) And Not String.IsNullOrEmpty(tempTable.Rows(0).Item("contact_sirname").ToString) Then
                        actinfo_contact_name.Text = tempTable.Rows(0).Item("contact_sirname").ToString.Trim + "&nbsp;"
                    End If

                    If Not (IsDBNull(tempTable.Rows(0).Item("contact_first_name"))) And Not String.IsNullOrEmpty(tempTable.Rows(0).Item("contact_first_name").ToString) Then
                        actinfo_contact_name.Text += tempTable.Rows(0).Item("contact_first_name").ToString.Trim + "&nbsp;"
                    End If

                    If Not (IsDBNull(tempTable.Rows(0).Item("contact_middle_initial"))) And Not String.IsNullOrEmpty(tempTable.Rows(0).Item("contact_middle_initial").ToString) Then
                        actinfo_contact_name.Text += tempTable.Rows(0).Item("contact_middle_initial").ToString.Trim + ".&nbsp;"
                    End If

                    If Not (IsDBNull(tempTable.Rows(0).Item("contact_last_name"))) And Not String.IsNullOrEmpty(tempTable.Rows(0).Item("contact_last_name").ToString) Then
                        actinfo_contact_name.Text += tempTable.Rows(0).Item("contact_last_name").ToString.Trim + "&nbsp;"
                    End If

                    If Not (IsDBNull(tempTable.Rows(0).Item("contact_suffix"))) And Not String.IsNullOrEmpty(tempTable.Rows(0).Item("contact_suffix").ToString) Then
                        actinfo_contact_name.Text += tempTable.Rows(0).Item("contact_suffix").ToString.Trim
                    End If

                    If Not (IsDBNull(tempTable.Rows(0).Item("contact_title"))) And Not String.IsNullOrEmpty(tempTable.Rows(0).Item("contact_title").ToString) Then
                        actinfo_contact_title.Text = tempTable.Rows(0).Item("contact_title").ToString.Trim
                    Else
                        actinfo_contact_title.Text = "<em>No Title</em>"
                    End If

                    If Not (IsDBNull(tempTable.Rows(0).Item("contact_email_address"))) And Not String.IsNullOrEmpty(tempTable.Rows(0).Item("contact_email_address").ToString) Then
                        actinfo_contact_email.Text = "<a href='mailto:" + tempTable.Rows(0).Item("contact_email_address").ToString.Trim + "'>" + tempTable.Rows(0).Item("contact_email_address").ToString.Trim + "</a>"
                        strUserEmailAddress = tempTable.Rows(0).Item("contact_email_address").ToString.Trim
                    Else
                        actinfo_contact_email.Text = "<em>None</em>"
                    End If

                End If
            Else
                Master.LogError("Error in Preferences.aspx.vb (FillinContactInfo) - " & aclsData_Temp.class_error)
            End If


            tempTable = New DataTable
        Catch ex As Exception
            Master.LogError("Error in Preferences.aspx.vb (FillinContactInfo) - " & ex.Message)
        End Try


    End Sub

    Public Sub fillInCustomerRepInfo()
        Dim tempTable As New DataTable
        Dim repPictureName As String = ""
        Dim imgFolder As String = HttpContext.Current.Server.MapPath(Session.Item("AccountRepPicturesFolderVirtualPath"))
        Dim imgDisplayFolder As String = Session.Item("AccountRepPicturesFolderVirtualPath")
        Dim imgFileName As String = ""
        Try
            'Changed 11/04/15
            'Per Instruction: On the support tab of preferences page we need to change the way we get the account representative.
            'We should run the query down below first (GetRepresentativeFirstCheck) and if nothing is returned, 
            'then we run the second query (GetRepresentative)
            tempTable = aclsData_Temp.GetRepresentativeFirstCheck(Session.Item("localUser").crmUserCompanyID)

            If Not IsNothing(tempTable) Then
                If tempTable.Rows.Count = 0 Then
                    tempTable = aclsData_Temp.GetRepresentative(Session.Item("localUser").crmUserCompanyID)
                End If
            Else
                'Error logging if the datalayer returns nothing because it errored out.
                Master.LogError("Error in Preferences.aspx.vb (FillInCustomerRepInfo - First DataCheck) - " & aclsData_Temp.class_error)

                'Since the first query errored out, we still need to run the second query.
                tempTable = aclsData_Temp.GetRepresentative(Session.Item("localUser").crmUserCompanyID)
            End If

            If Not IsNothing(tempTable) Then
                If tempTable.Rows.Count > 0 Then

                    If Not (IsDBNull(tempTable.Rows(0).Item("user_id"))) And Not String.IsNullOrEmpty(tempTable.Rows(0).Item("user_id").ToString) Then
                        repPictureName = tempTable.Rows(0).Item("user_id").ToString.Trim
                    End If

                    If Not (IsDBNull(tempTable.Rows(0).Item("user_first_name"))) And Not String.IsNullOrEmpty(tempTable.Rows(0).Item("user_first_name").ToString) Then
                        support_rep_name.Text = tempTable.Rows(0).Item("user_first_name").ToString.Trim + "&nbsp;"
                    End If

                    If Not (IsDBNull(tempTable.Rows(0).Item("user_middle_initial"))) And Not String.IsNullOrEmpty(tempTable.Rows(0).Item("user_middle_initial").ToString) Then
                        support_rep_name.Text &= tempTable.Rows(0).Item("user_middle_initial").ToString.Trim + ".&nbsp;"
                    End If

                    If Not (IsDBNull(tempTable.Rows(0).Item("user_last_name"))) And Not String.IsNullOrEmpty(tempTable.Rows(0).Item("user_last_name").ToString) Then
                        support_rep_name.Text &= tempTable.Rows(0).Item("user_last_name").ToString.Trim
                    End If

                    If Not (IsDBNull(tempTable.Rows(0).Item("user_phone_no"))) And Not String.IsNullOrEmpty(tempTable.Rows(0).Item("user_phone_no").ToString) Then
                        If bEnableTelTags Then
                            support_rep_number.Text = "<a href='tel:" + tempTable.Rows(0).Item("user_phone_no").ToString.Trim + "'>" + tempTable.Rows(0).Item("user_phone_no").ToString.Trim + "</a>"
                        Else
                            support_rep_number.Text = tempTable.Rows(0).Item("user_phone_no").ToString.Trim
                        End If
                    End If

                    If Not (IsDBNull(tempTable.Rows(0).Item("user_phone_no_ext"))) And Not String.IsNullOrEmpty(tempTable.Rows(0).Item("user_phone_no_ext").ToString) Then
                        support_rep_number.Text &= " ext. " + tempTable.Rows(0).Item("user_phone_no_ext").ToString.Trim
                    End If

                    If Not (IsDBNull(tempTable.Rows(0).Item("user_email_address"))) And Not String.IsNullOrEmpty(tempTable.Rows(0).Item("user_email_address").ToString) Then
                        support_rep_email.Text = "<a href='mailto:" + tempTable.Rows(0).Item("user_email_address").ToString.Trim + "'>" + tempTable.Rows(0).Item("user_email_address").ToString.Trim + "</a>"
                    End If


                    ' get rep image
                    imgFileName = repPictureName.ToLower + ".jpg"
                    If System.IO.File.Exists(imgFolder.Trim + "\" + imgFileName.Trim) Then
                        support_rep_image.ImageUrl = imgDisplayFolder.Trim + "/" + imgFileName.Trim
                        support_rep_image.AlternateText = " Jetnet Representative " + support_rep_name.Text.Replace("&nbsp;", " ").Trim
                        support_rep_image.ToolTip = " Jetnet Representative " + support_rep_name.Text.Replace("&nbsp;", " ").Trim
                    Else
                        support_rep_image.ImageUrl = "images/person.jpg" ' if image is missing (for some unknown reason )
                        support_rep_image.AlternateText = " Jetnet Representative " + support_rep_name.Text.Replace("&nbsp;", " ").Trim
                        support_rep_image.ToolTip = " Jetnet Representative " + support_rep_name.Text.Replace("&nbsp;", " ").Trim
                    End If
                End If
            Else
                Master.LogError("Error in Preferences.aspx.vb (FillInCustomerRepInfo - Second Data Check) - " & aclsData_Temp.class_error)
            End If

        Catch ex As Exception
            Master.LogError("Error in Preferences.aspx.vb (FillInCustomerRepInfo) - " & ex.Message)
        End Try

    End Sub

    Public Sub fillInSubscriptionInfo()

        Dim results_table As New DataTable
        results_table = crmWebClient.clsSubscriptionClass.getSessionSubscriptionInfo()

        Dim sProjectText As String = ""

        Try

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    For Each r As DataRow In results_table.Rows

                        subscription_username.Text = "User Name: <em>" + r.Item("subins_login").ToString.Trim + "</em>"
                        subscription_email.Text = "Email: <em><a href='mailto:" + strUserEmailAddress.Trim + "'>" + strUserEmailAddress.Trim + "</a></em>"

                        subscription_subscription_id.Text = "Subscription ID: <em>" + r.Item("sub_id").ToString.Trim + "</em>"
                        subscription_subscription_id.Text = subscription_subscription_id.Text + " Seq No: <em>" + r.Item("subins_seq_no").ToString.Trim + "</em>"

                        subscription_platform.Text = "Platform OS: <em>" + r.Item("subins_platform_os").ToString.Trim + "</em>"

                        subscription_marketing_account.Text = "Marketing Account: <em>" + IIf(r.Item("sub_marketing_flag").ToString.ToUpper.Contains("Y"), "True", "False") + "</em>"
                        subscription_demo_account.Text = "Demo Account: <em>" + IIf(r.Item("sublogin_demo_flag").ToString.ToUpper.Contains("Y"), "True", "False") + "</em>"

                        subscription_max_export.Text = "<a class=""underline pointer"" href=""/help/documents/533.pdf"" title=""Click to view help document for this option"" target=""_new"">Max Records allowed for Exports and Reports</a>: <em>" + r.Item("sub_max_allowed_custom_export").ToString + "</em>"

                        If r.Item("sub_share_by_comp_id_flag").ToString.ToUpper.Contains("Y") Then
                            subscription_share.Text = "Sharing Folders/Templates: <em>With All Users From My Company/Location</em>"
                            bShareByCompany = IIf(r.Item("sub_share_by_comp_id_flag").ToString.ToUpper.Contains("Y"), True, False)
                        ElseIf r.Item("sub_share_by_parent_sub_id_flag").ToString.ToUpper.Contains("Y") Then
                            subscription_share.Text = "Sharing Folders/Templates: <em>With All Users Under My Parent Subscription</em>"
                            bShareBySubscription = IIf(r.Item("sub_share_by_parent_sub_id_flag").ToString.ToUpper.Contains("Y"), True, False)
                        End If

                        If Not (IsDBNull(r.Item("sub_parent_sub_id"))) Then
                            If CLng(r.Item("sub_parent_sub_id").ToString) > 0 Then
                                nParentSubID = CLng(r.Item("sub_parent_sub_id").ToString)
                            End If
                        End If

                        If Not (IsDBNull(r.Item("sub_comp_id"))) Then
                            If CLng(r.Item("sub_comp_id").ToString) > 0 Then
                                nSubCompID = CLng(r.Item("sub_comp_id").ToString)
                            End If
                        End If

                        subscription_privilege.Text = "Privileges: <em>" + IIf(r.Item("subins_admin_flag").ToString.ToUpper.Contains("Y"), "You are an Administrator", "You are a Standard User") + "</em>"

                        Select Case (r.Item("sub_busair_tier_level").ToString)
                            Case "1"
                                subscription_tier.Text = "Tier Level: <em>Jets</em>"
                            Case "2"
                                subscription_tier.Text = "Tier Level: <em>Turbos</em>"
                            Case Else
                                subscription_tier.Text = "Tier Level: <em>All</em>"
                        End Select

                        subscription_aerodex.Text = "Aerodex: <em>" + IIf(r.Item("sub_aerodex_flag").ToString.ToUpper.Contains("Y"), "True", "False") + "</em>"

                        subscription_business.Text = "Business: <em>" + IIf(r.Item("sub_business_aircraft_flag").ToString.ToUpper.Contains("Y"), "True", "False") + "</em>"
                        subscription_commercial.Text = "Commercial: <em>" + IIf(r.Item("sub_commerical_flag").ToString.ToUpper.Contains("Y"), "True", "False") + "</em>"
                        subscription_helicopter.Text = "Helicopter: <em>" + IIf(r.Item("sub_helicopters_flag").ToString.ToUpper.Contains("Y"), "True", "False") + "</em>"

                        subscription_star.Text = "STAR Reports: <em>" + IIf(r.Item("sub_starreports_flag").ToString.ToUpper.Contains("Y"), "True", "False") + "</em>"

                        subscription_spi_view.Text = "Values: <em>" + IIf(r.Item("sublogin_values_flag").ToString.ToUpper.Contains("Y"), "True", "False") + "</em>"
                        subscription_service_code.Text = "Service Code: [" + r.Item("sub_serv_code").ToString.ToUpper + "] <em>" + r.Item("serv_name").ToString + "</em>"

                        If Not (IsDBNull(r.Item("subins_default_amod_id"))) Then
                            If CLng(r.Item("subins_default_amod_id").ToString) > 0 Then
                                subscription_default_model.Text = "Model:&nbsp;<em>" + commonEvo.Get_Aircraft_Model_Info(CLng(r.Item("subins_default_amod_id").ToString), False, "") + "</em>"
                                display_default_modelID.Text = commonEvo.Get_Aircraft_Model_Info(CLng(r.Item("subins_default_amod_id").ToString), False, "").Trim
                                nDefaultModelID = CLng(r.Item("subins_default_amod_id").ToString)
                            Else
                                subscription_default_model.Text = "Model:&nbsp;<em>None</em>"
                                display_default_modelID.Text = "None"
                                nDefaultModelID = 0
                            End If
                        Else
                            subscription_default_model.Text = "Model:&nbsp;<em>None</em>"
                            display_default_modelID.Text = "None"
                            nDefaultModelID = 0
                        End If

                        If Not (IsDBNull(r.Item("subins_business_type_code"))) Then
                            If Not String.IsNullOrEmpty(r.Item("subins_business_type_code").ToString.Trim) Then

                                Dim sTmpStr As String = ""

                                Select Case r.Item("subins_business_type_code").ToString.Trim.ToUpper

                                    Case "DB"
                                        sTmpStr = "Dealer/Broker"
                                    Case "FB"
                                        sTmpStr = "Fixed Base Operator"
                                    Case "UI"
                                        sTmpStr = "Unidentified"

                                End Select
                                subscription_default_business_segment.Text = "User Perspective:&nbsp;<em>" + sTmpStr + "</em>"
                            End If
                        End If

                        'If Not (IsDBNull(r.Item("subins_evoview_id"))) Then
                        '  If CLng(r.Item("subins_evoview_id").ToString) > 0 Then
                        '    subscription_default_view.Text = "View:&nbsp;<em>" + commonEvo.Get_Default_User_View(CLng(r.Item("subins_evoview_id").ToString)) + "</em>"
                        '    display_default_viewID.Text = commonEvo.Get_Default_User_View(CLng(r.Item("subins_evoview_id").ToString)).Trim

                        '    If display_default_viewID.Text.Contains("No&nbsp;View&nbsp;Selected") Then
                        '      display_reset_default_viewID.Enabled = False
                        '    Else
                        '      display_reset_default_viewID.Enabled = True
                        '    End If
                        '  Else
                        '    subscription_default_view.Text = "View:&nbsp;<em>" + commonEvo.Get_Default_User_View(0) + "</em>"
                        '    display_default_viewID.Text = commonEvo.Get_Default_User_View(0).Trim
                        '    display_reset_default_viewID.Enabled = False
                        '  End If
                        'Else
                        '  subscription_default_view.Text = "View:&nbsp;<em>" + commonEvo.Get_Default_User_View(0) + "</em>"
                        '  display_default_viewID.Text = commonEvo.Get_Default_User_View(0).Trim
                        '  display_reset_default_viewID.Enabled = False
                        'End If

                        If IIf(r.Item("sub_yacht_flag").ToString.ToUpper.Contains("Y"), True, False) Then
                            subscription_yacht.Text = "Yacht: <em>" + IIf(r.Item("sub_yacht_flag").ToString.ToUpper.Contains("Y"), "True", "False") + "</em>"
                        Else
                            subscription_yacht.Text = ""
                        End If

                        If Not (IsDBNull(r.Item("subins_background_image_id"))) Then
                            If CLng(r.Item("subins_background_image_id").ToString) > 0 Then
                                subscription_default_background.Text = "Background:&nbsp;<em>" + commonEvo.Get_Default_User_Background(CLng(r.Item("subins_background_image_id").ToString)) + "</em>"
                                display_default_backgroundID.Text = commonEvo.Get_Default_User_Background(CLng(r.Item("subins_background_image_id").ToString)).Trim
                                nDefaultBackgroundID = CLng(r.Item("subins_background_image_id").ToString)
                            Else
                                subscription_default_background.Text = "Background:&nbsp;<em>" + commonEvo.Get_Default_User_Background(0) + "</em>"
                                display_default_backgroundID.Text = commonEvo.Get_Default_User_Background(0).Trim
                                nDefaultBackgroundID = 0
                            End If
                        Else
                            subscription_default_background.Text = "Background:&nbsp;<em>" + commonEvo.Get_Default_User_Background(0) + "</em>"
                            display_default_backgroundID.Text = commonEvo.Get_Default_User_Background(0).Trim
                            nDefaultBackgroundID = 0
                        End If

                        sMobileNumber = r.Item("subins_cell_number").ToString

                        If IIf(r.Item("subins_evo_mobile_flag").ToString.ToUpper.Contains("Y"), True, False) Then
                            bEnableMobile = True
                        Else
                            bEnableMobile = False
                        End If

                        If IIf(r.Item("subins_chat_flag").ToString.ToUpper.Contains("Y"), True, False) Then
                            bEnableChat = True
                        Else
                            bEnableChat = False
                        End If

                        If Session.Item("localUser").crmUserType = eUserTypes.ADMINISTRATOR Then
                            myservices_enable_global_list_ck.Enabled = True
                        Else
                            myservices_enable_global_list_ck.Enabled = False
                        End If

                        subscription_show_on_global.Text = "Show Listings on JETNET GLOBAL Enabled: <em>" + IIf(bEnableGLOBALListings, "True", "False") + "</em>"

                        If bCanSaveProjects Then

                            Dim bHasDefaultProject = commonEvo.CheckForProject(sProjectText)

                            If Not String.IsNullOrEmpty(sProjectText) Then
                                subscription_default_project.Text = "Project: <em>" + sProjectText + "</em>"
                            Else
                                subscription_default_project.Text = "Project: <em>No Default Project</em>"
                            End If

                        End If

                        If bCanSaveDefaultEmail Then

                            If Not (IsDBNull(r.Item("subins_email_replyname"))) Then
                                sEmailName = r.Item("subins_email_replyname").ToString.Trim
                            Else
                                sEmailName = ""
                            End If

                            If Not (IsDBNull(r.Item("subins_email_replyaddress"))) Then
                                sEmailAddress = r.Item("subins_email_replyaddress").ToString.Trim
                            Else
                                sEmailAddress = ""
                            End If

                            sEmailFormat = r.Item("subins_email_default_format").ToString.Trim.ToUpper
                        Else
                            sEmailAddress = ""
                            sEmailName = ""
                            sEmailFormat = "HTML"
                        End If

                        If Not (IsDBNull(r.Item("subins_default_models"))) Then
                            sDefaultModels = r.Item("subins_default_models").ToString.Trim
                        Else
                            sDefaultModels = ""
                        End If

                        If bCanHaveSMS And Not CBool(Session.Item("localPreferences").AerodexFlag.ToString) Then

                            sSMSNumber = sMobileNumber

                            If Not (IsDBNull(r.Item("subins_cell_service"))) Then
                                sSMSProviderName = r.Item("subins_cell_service").ToString.Trim
                            Else
                                sSMSProviderName = ""
                            End If

                            If Not (IsDBNull(r.Item("subins_cell_carrier_id"))) Then
                                sSMSProviderID = CInt(r.Item("subins_cell_carrier_id").ToString)
                            Else
                                sSMSProviderID = 0
                            End If

                            If Not (IsDBNull(r.Item("subins_sms_events"))) Then
                                sSMSSelectedEvents = r.Item("subins_sms_events").ToString.Trim
                            Else
                                sSMSSelectedEvents = ""
                            End If

                            Dim sEventsOut As String = ""
                            localDatalayer.fillSMSEventsDropDown(Nothing, 0, sEventsOut, sSMSSelectedEvents, True)
                            subscription_SMS_events.Text = "SMS Events:<br />" + sEventsOut

                            If Not (IsDBNull(r.Item("subins_smstxt_models"))) Then
                                sSMSSelectedModelID = r.Item("subins_smstxt_models").ToString.Trim
                            Else
                                sSMSSelectedModelID = ""
                            End If



                            If Not (IsDBNull(r.Item("subins_default_airports"))) Then
                                sDefaultAirports = r.Item("subins_default_airports").ToString.Trim
                            Else
                                sDefaultAirports = ""
                            End If

                            Dim sModelsOut As String = ""
                            commonEvo.fillMakeModelDropDown(Nothing, Nothing, 0, sModelsOut, sSMSSelectedModelID, False, False, True, False, False, True) ' display models
                            subscription_SMS_models.Text = "SMS Models:<br />" + sModelsOut

                            sSMSActivationStatus = r.Item("subins_smstxt_active_flag").ToString.Trim

                            Select Case (sSMSActivationStatus)
                                Case Constants.SMS_ACTIVATE_YES
                                    myservices_SMS_service_status.Text = "ACTIVE"
                                Case Constants.SMS_ACTIVATE_NO
                                    myservices_SMS_service_status.Text = "INACTIVE"
                                Case Constants.SMS_ACTIVATE_PENDING
                                    myservices_SMS_service_status.Text = "PENDING"
                                Case Constants.SMS_ACTIVATE_WAIT
                                    myservices_SMS_service_status.Text = "WAITING"
                                Case Constants.SMS_ACTIVATE_TEST
                                    myservices_SMS_service_status.Text = "TESTING"
                                Case Else
                                    myservices_SMS_service_status.Text = "INACTIVE"
                                    sSMSActivationStatus = Constants.SMS_ACTIVATE_NO
                            End Select

                            If Not String.IsNullOrEmpty(sSMSNumber) Then
                                myservices_sms_phone_number.Text = sSMSNumber
                            End If

                            If Not String.IsNullOrEmpty(sSMSSelectedModelID) Then
                                System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showSelectedItems", "MultiListEnsureItemVisible();", True)
                            End If

                        End If

                        If bCanUseLocalNotes Then

                            If bHasServerNotes Then
                                subscription_server_notes.Text = "Cloud Notes Plus: <em>" + IIf(bHasServerNotes = True, "True", "False") + "</em>"
                            ElseIf bHasStandardCloudNotes Then
                                subscription_server_notes.Text = "Standard Cloud Notes: <em>" + IIf(bHasStandardCloudNotes = True, "True", "False") + "</em>"
                            End If

                            If bHasServerNotes Then
                                notes_notes_options.SelectedValue = "Plus"
                            ElseIf bHasStandardCloudNotes Then
                                notes_notes_options.SelectedValue = "Standard"
                            Else
                                notes_notes_options.SelectedValue = ""
                            End If

                            If IIf(r.Item("subins_display_note_tag_on_aclist_flag").ToString.ToUpper.Contains("Y"), True, False) Then
                                bHasAcNotesOnListing = True
                            Else
                                bHasAcNotesOnListing = False
                            End If

                        End If

                        ' addtional subscription values
                        'Session.Item("localPreferences").DefaultCompType = r.Item("subins_aircraft_tab_relationship_to_ac_default").ToString.Trim

                        'If Not String.IsNullOrEmpty(Session.Item("localPreferences").DefaultCompType) Then
                        '  mydisplay_enabled_default_feature.Checked = True
                        'Else
                        '  If Not mydisplay_selected_relationships.Checked Then
                        '    mydisplay_enabled_default_feature.Checked = False
                        '  Else
                        '    mydisplay_enabled_default_feature.Checked = True  "User Perspective:&nbsp;<em>" + sTmpStr + "</em>"
                        '  End If
                        'End If

                        subscription_default_analysis_months.Text = "Default Analysis Timeframe (Months):&nbsp;<em>" + Session.Item("localPreferences").DefaultAnalysisMonths.ToString + "</em>"

                        If Session.Item("localUser").crmUserType = eUserTypes.ADMINISTRATOR Then
                            ' commented out MSW - 9/20/18 - 
                            'If bShareByCompany Or bShareBySubscription Then
                            my_users.Visible = True
                            'End If
                        End If

                    Next

                End If

            End If

            If Session.Item("localUser").crmUserType = eUserTypes.ADMINISTRATOR And CBool(Session.Item("localUser").crmUser_Evo_MPM_Flag.ToString) Then

                maximum_records_export.Text = "0" 'default to 0.
                If Not IsDBNull(Session.Item("localUser").crmMaxClientExport) Then
                    If IsNumeric(Session.Item("localUser").crmMaxClientExport) Then
                        If Session.Item("localUser").crmMaxClientExport <> 0 Then
                            maximum_records_export.Text = Session.Item("localUser").crmMaxClientExport
                        End If
                    End If
                End If

                results_table = New DataTable
                results_table = aclsData_Temp.Get_Client_Preferences()

                If Not IsNothing(results_table) Then

                    If results_table.Rows.Count > 0 Then

                        For Each r As DataRow In results_table.Rows

                            nMPMPrefID = CLng(IIf(Not IsDBNull(r("clipref_id")), r("clipref_id").ToString, "0"))

                            If IsPostBack Then
                                Exit For
                            End If

                            'Aircraft Preferences.
                            ac_category_1.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_1")), r("clipref_ac_custom_1"), ""))
                            ac_category_2.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_2")), r("clipref_ac_custom_2"), ""))
                            ac_category_3.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_3")), r("clipref_ac_custom_3"), ""))
                            ac_category_4.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_4")), r("clipref_ac_custom_4"), ""))
                            ac_category_5.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_5")), r("clipref_ac_custom_5"), ""))
                            ac_category_6.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_6")), r("clipref_ac_custom_6"), ""))
                            ac_category_7.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_7")), r("clipref_ac_custom_7"), ""))
                            ac_category_8.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_8")), r("clipref_ac_custom_8"), ""))
                            ac_category_9.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_9")), r("clipref_ac_custom_9"), ""))
                            ac_category_10.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_10")), r("clipref_ac_custom_10"), ""))

                            If Not IsDBNull(r("clipref_ac_custom_1_use")) Then
                                If r("clipref_ac_custom_1_use") = "Y" Then
                                    ac_category_1_use.Checked = True
                                Else
                                    ac_category_1_use.Checked = False
                                End If
                            End If
                            If Not IsDBNull(r("clipref_ac_custom_2_use")) Then
                                If r("clipref_ac_custom_2_use") = "Y" Then
                                    ac_category_2_use.Checked = True
                                Else
                                    ac_category_2_use.Checked = False
                                End If
                            End If
                            If Not IsDBNull(r("clipref_ac_custom_3_use")) Then
                                If r("clipref_ac_custom_3_use") = "Y" Then
                                    ac_category_3_use.Checked = True
                                Else
                                    ac_category_3_use.Checked = False
                                End If
                            End If
                            If Not IsDBNull(r("clipref_ac_custom_4_use")) Then
                                If r("clipref_ac_custom_4_use") = "Y" Then
                                    ac_category_4_use.Checked = True
                                Else
                                    ac_category_4_use.Checked = False
                                End If
                            End If
                            If Not IsDBNull(r("clipref_ac_custom_5_use")) Then
                                If r("clipref_ac_custom_5_use") = "Y" Then
                                    ac_category_5_use.Checked = True
                                Else
                                    ac_category_5_use.Checked = False
                                End If
                            End If
                            If Not IsDBNull(r("clipref_ac_custom_6_use")) Then
                                If r("clipref_ac_custom_6_use") = "Y" Then
                                    ac_category_6_use.Checked = True
                                Else
                                    ac_category_6_use.Checked = False
                                End If
                            End If
                            If Not IsDBNull(r("clipref_ac_custom_7_use")) Then
                                If r("clipref_ac_custom_7_use") = "Y" Then
                                    ac_category_7_use.Checked = True
                                Else
                                    ac_category_7_use.Checked = False
                                End If
                            End If
                            If Not IsDBNull(r("clipref_ac_custom_8_use")) Then
                                If r("clipref_ac_custom_8_use") = "Y" Then
                                    ac_category_8_use.Checked = True
                                Else
                                    ac_category_8_use.Checked = False
                                End If
                            End If
                            If Not IsDBNull(r("clipref_ac_custom_9_use")) Then
                                If r("clipref_ac_custom_9_use") = "Y" Then
                                    ac_category_9_use.Checked = True
                                Else
                                    ac_category_9_use.Checked = False
                                End If
                            End If
                            If Not IsDBNull(r("clipref_ac_custom_10_use")) Then
                                If r("clipref_ac_custom_10_use") = "Y" Then
                                    ac_category_10_use.Checked = True
                                Else
                                    ac_category_10_use.Checked = False
                                End If
                            End If


                            'Company Preferences
                            pref_1.Text = CStr(IIf(Not IsDBNull(r("clipref_category1_name")), r("clipref_category1_name"), ""))
                            pref_2.Text = CStr(IIf(Not IsDBNull(r("clipref_category2_name")), r("clipref_category2_name"), ""))
                            pref_3.Text = CStr(IIf(Not IsDBNull(r("clipref_category3_name")), r("clipref_category3_name"), ""))
                            pref_4.Text = CStr(IIf(Not IsDBNull(r("clipref_category4_name")), r("clipref_category4_name"), ""))
                            pref_5.Text = CStr(IIf(Not IsDBNull(r("clipref_category5_name")), r("clipref_category5_name"), ""))

                            If Not IsDBNull(r("clipref_category1_use")) Then
                                If r("clipref_category1_use") = "Y" Then
                                    pref_1_use.Checked = True
                                Else
                                    pref_1_use.Checked = False
                                End If
                            End If

                            If Not IsDBNull(r("clipref_category2_use")) Then
                                If r("clipref_category2_use") = "Y" Then
                                    pref_2_use.Checked = True
                                Else
                                    pref_2_use.Checked = False
                                End If
                            End If

                            If Not IsDBNull(r("clipref_category3_use")) Then
                                If r("clipref_category3_use") = "Y" Then
                                    pref_3_use.Checked = True
                                Else
                                    pref_3_use.Checked = False
                                End If
                            End If

                            If Not IsDBNull(r("clipref_category4_use")) Then
                                If r("clipref_category4_use") = "Y" Then
                                    pref_4_use.Checked = True
                                Else
                                    pref_4_use.Checked = False
                                End If
                            End If

                            If Not IsDBNull(r("clipref_category5_use")) Then
                                If r("clipref_category5_use") = "Y" Then
                                    pref_5_use.Checked = True
                                Else
                                    pref_5_use.Checked = False
                                End If
                            End If
                        Next

                    End If

                End If

                If Not IsPostBack Then

                    fill_feature_code()

                End If

            End If

        Catch ex As Exception
            Master.LogError("Error in Preferences.aspx.vb (FillInSubscriptionInfo) - " & ex.Message)
        End Try

    End Sub

    Private Sub fillSubscriberUserList()
        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False
        Dim results_table As New DataTable

        Try

            'Display User Licenses
            results_table = localDatalayer.DisplayTotalUserLicenses(bShareByCompany, bShareBySubscription, CLng(Session.Item("localUser").crmSubSubID.ToString), nParentSubID, nSubCompID)

            If Not IsNothing(results_table) Then
                If results_table.Rows.Count > 0 Then
                    myusers_total_user_license.Text = results_table.Rows(0).Item("totlicenses").ToString
                End If
            End If

            results_table = New DataTable

            results_table = localDatalayer.DisplayAdminUserList(bShareByCompany, bShareBySubscription, CLng(Session.Item("localUser").crmSubSubID.ToString), nParentSubID, nSubCompID)

            If Not IsNothing(results_table) Then
                If results_table.Rows.Count > 0 Then

                    myusers_total_user_assigned.Text = results_table.Rows.Count

                    htmlOut.Append("<table id=""subscribersTable"" width=""100%"" cellpadding=""2"" cellspacing=""2"">")
                    htmlOut.Append("<tr>")
                    htmlOut.Append("<td align=""left"" valign=""top""><strong>Name</strong></td>")
                    htmlOut.Append("<td align=""left"" valign=""top""><strong>Email</strong></td>")
                    htmlOut.Append("<td align=""left"" valign=""top""><strong>Password</strong></td>")
                    htmlOut.Append("<td align=""left"" valign=""top""><strong>Location</strong></td>")
                    htmlOut.Append("<td align=""left"" valign=""top""><strong>Notes</strong></td>")
                    htmlOut.Append("<td align=""right"" valign=""top""><strong>Contact ID</strong></td>")
                    htmlOut.Append("<td align=""right"" valign=""top""><strong>Company ID</strong></td>")
                    htmlOut.Append("<td align=""right"" valign=""top""><strong>Sub ID</strong></td>")
                    htmlOut.Append("</tr>")

                    'Set up to eventually work looping through a datatable 
                    For Each r As DataRow In results_table.Rows
                        If Not toggleRowColor Then
                            htmlOut.Append("<tr class=""alt_row"">")
                            toggleRowColor = True
                        Else
                            htmlOut.Append("<tr bgcolor=""white"">")
                            toggleRowColor = False
                        End If

                        'Placeholder for Name
                        htmlOut.Append("<td align=""left"" valign=""top"">" + IIf(Not IsDBNull(r("contact_sirname")), r("contact_sirname").ToString.Trim + " ", "") + (IIf(Not IsDBNull(r("contact_first_name")), r("contact_first_name").ToString.Trim + " ", "")) + r("contact_last_name").ToString.Trim + "</td>")

                        'Placeholder for Email
                        htmlOut.Append("<td align=""left"" valign=""top"">" + IIf(Not IsDBNull(r("contact_email_address")), "<a href=""mailto:" + r("contact_email_address").ToString.Trim + """>" + r("contact_email_address").ToString.Trim + "</a>", "") + "</td>")


                        'Placeholder for Password 
                        Dim returnstring As String = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(r("comp_id").ToString.Trim + "," + r("contact_id").ToString.Trim + "," + r("sub_id").ToString.Trim + "," + r("contact_email_address").ToString.Trim + "," + r("sublogin_password").ToString.Trim))

                        htmlOut.Append("<td align=""left"" valign=""top""><a href=""preferences.aspx?activetab=8&returnstring=" + returnstring + """ title=""Send User Password""> Send Password </a></td>")

                        'Placeholder for Location
                        htmlOut.Append("<td align=""left"" valign=""top"">" + IIf(Not IsDBNull(r("comp_city")), r("comp_city").ToString + ", ", "") + IIf(Not IsDBNull(r("comp_state")), " (" + r("comp_state") + ")", "") + "</td>")

                        'Placeholder for Notes
                        htmlOut.Append("<td align=""center"" valign=""top"">" + IIf(r("sub_server_side_notes_flag").ToString.ToUpper.Contains("Y"), "<img src=""images/server.png"" alt=""Server Notes Available"" title=""Server Notes Available"" />", "") + " " + IIf(r("sub_cloud_notes_flag").ToString.ToUpper.Contains("Y"), "<img src=""images/cloud.png"" alt=""Cloud Notes Available"" title=""Cloud Notes Available"" />", "") + "</td>")

                        'Placeholder for Contact ID
                        htmlOut.Append("<td align=""right"" valign=""top"">" + r("contact_id").ToString + "</td>")

                        'Placeholder for Company ID
                        htmlOut.Append("<td align=""right"" valign=""top"">" + r("comp_id").ToString + "</td>")

                        'Placeholder for Subscription ID
                        htmlOut.Append("<td align=""right"" valign=""top"">" + r("sub_id").ToString + "</td>")


                        'End Table Row
                        htmlOut.Append("</tr>")
                    Next

                    htmlOut.Append("</table>")

                End If
            End If

            my_users_list.Text = htmlOut.ToString

        Catch ex As Exception
            Master.LogError("Error in Preferences.aspx.vb (fillSubscriberUserList) - " & ex.Message)
        End Try

        htmlOut = Nothing
        results_table = Nothing

    End Sub

    Private Sub fillSubscriberFolderList()

        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False
        Dim results_table As New DataTable
        Dim folderList As String = ""
        Dim bIsAdmin As Boolean = False

        Try

            If DropDownList2.SelectedValue.ToLower.Contains("usf") Then
                bIsAdmin = True
            End If

            results_table = commonEvo.returnUserFolders(False, bIsAdmin, DropDownList1.SelectedValue)

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    localDatalayer.display_folder_results_table(results_table, folderList)

                    If Not String.IsNullOrEmpty(folderList.Trim) Then
                        folderTable.Text = folderList
                        System.Web.UI.ScriptManager.RegisterStartupScript(Me.my_folders, Me.my_folders.GetType(), "CreateFolderTable", "$(document).ready(function() { CreateTheDatatable('folderInnerTable','folderDataTable','folderjQueryTable'); });", True)
                    Else
                        folderTable.Text = "<table id=""folderResultsOuterTable"" cellpadding=""2"" cellspacing=""0"">"
                        folderTable.Text += "<tr><td align=""left"" valign=""middle""><em>No Folder Results To Display</em></td></tr>"
                        folderTable.Text += "</table>"
                    End If

                Else
                    folderTable.Text = "<table id=""folderResultsOuterTable"" cellpadding=""2"" cellspacing=""0"">"
                    folderTable.Text += "<tr><td align=""left"" valign=""middle""><em>No Folder Results To Display</em></td></tr>"
                    folderTable.Text += "</table>"
                End If

            Else
                folderTable.Text = "<table id=""folderResultsOuterTable"" cellpadding=""2"" cellspacing=""0"">"
                folderTable.Text += "<tr><td align=""left"" valign=""middle""><em>No Folder Results To Display</em></td></tr>"
                folderTable.Text += "</table>"
            End If

        Catch ex As Exception
            Master.LogError("Error in Preferences.aspx.vb (fillSubscriberFolderList) - " + ex.Message)
        End Try

        htmlOut = Nothing
        results_table = Nothing

    End Sub

    Private Sub fillSubscriberTemplateList()

        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False
        Dim results_table As New DataTable
        Dim templateList As String = ""
        Dim bIsAdmin As Boolean = False

        Try

            If DropDownList4.SelectedValue.ToLower.Contains("ust") Then
                bIsAdmin = True
            End If

            results_table = commonEvo.returnUserTemplates(False, bIsAdmin, DropDownList3.SelectedValue)

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    localDatalayer.display_template_results_table(results_table, templateList)

                    If Not String.IsNullOrEmpty(templateList.Trim) Then
                        templateTable.Text = templateList
                        System.Web.UI.ScriptManager.RegisterStartupScript(Me.my_templates, Me.my_templates.GetType(), "CreateTemplatesTable", "$(document).ready(function() { CreateTheDatatable('templateInnerTable','templateDataTable','templatejQueryTable'); });", True)
                    Else
                        templateTable.Text = "<table id=""templateResultsOuterTable"" cellpadding=""2"" cellspacing=""0"">"
                        templateTable.Text += "<tr><td align=""left"" valign=""middle""><em>No Template Results To Display</em></td></tr>"
                        templateTable.Text += "</table>"

                    End If

                Else
                    templateTable.Text = "<table id=""templateResultsOuterTable"" cellpadding=""2"" cellspacing=""0"">"
                    templateTable.Text += "<tr><td align=""left"" valign=""middle""><em>No Template Results To Display</em></td></tr>"
                    templateTable.Text += "</table>"

                End If

            Else
                templateTable.Text = "<table id=""templateResultsOuterTable"" cellpadding=""2"" cellspacing=""0"">"
                templateTable.Text += "<tr><td align=""left"" valign=""middle""><em>No Template Results To Display</em></td></tr>"
                templateTable.Text += "</table>"
            End If

        Catch ex As Exception
            Master.LogError("Error in Preferences.aspx.vb (fillSubscriberTemplateList) - " + ex.Message)
        End Try

        htmlOut = Nothing
        results_table = Nothing

    End Sub

    Private Sub fillDefaultAirportFolderContents()

        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False
        Dim results_table As New DataTable
        Dim airportList As String = ""
        Dim bIsAdmin As Boolean = False

        Try

            results_table = commonEvo.returnAirportFolderContents(CLng(default_airport_ddl.SelectedValue.Trim))

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    localDatalayer.display_airport_results_table(results_table, airportList)

                    If Not String.IsNullOrEmpty(airportList.Trim) Then
                        airportTable.Text = airportList
                        System.Web.UI.ScriptManager.RegisterStartupScript(Me.my_folders, Me.my_folders.GetType(), "CreateAirportTable", "$(document).ready(function() { CreateTheDatatable('airportInnerTable','airportDataTable','airportjQueryTable'); });", True)
                    Else
                        airportTable.Text = "<table id=""airportResultsOuterTable"" cellpadding=""2"" cellspacing=""0"">"
                        airportTable.Text += "<tr><td align=""left"" valign=""middle""><em>No airport results to Display</em></td></tr>"
                        airportTable.Text += "</table>"
                    End If

                Else
                    airportTable.Text = "<table id=""airportResultsOuterTable"" cellpadding=""2"" cellspacing=""0"">"
                    airportTable.Text += "<tr><td align=""left"" valign=""middle""><em>No airport results to Display</em></td></tr>"
                    airportTable.Text += "</table>"
                End If

            Else
                airportTable.Text = "<table id=""airportResultsOuterTable"" cellpadding=""2"" cellspacing=""0"">"
                airportTable.Text += "<tr><td align=""left"" valign=""middle""><em>No airport results to Display</em></td></tr>"
                airportTable.Text += "</table>"
            End If

        Catch ex As Exception
            Master.LogError("Error in Preferences.aspx.vb (fillDefaultAirportFolderContents) - " + ex.Message)
        End Try

        htmlOut = Nothing
        results_table = Nothing

    End Sub

    Private Function sendUserEmail(sub_comp_id As String, subins_contact_id As String, sublogon_subid As String, emailAddress As String, sublogon_password As String) As String

        Dim return_text As String = ""

        Dim HTML_Body As String = ""
        HTML_Body = "<html><head>"
        HTML_Body += "<title>Evolution JETNET, Jets and Turboprops, Helicopters, Commercial setup license info</title>"
        HTML_Body += "</head><body>"
        HTML_Body += "<img src=""https://www.jetnetevolution.com/images/evolutionlogo.gif""><br /><br />"
        HTML_Body += "<font face=""Arial"" size=""3"">" + FormatDateTime(Now, vbGeneralDate).Trim + "<br /><br />"
        HTML_Body += "JETNET LLC<br />Utica, NY  United States<br /><br />"
        HTML_Body += "Per your request, listed below is the license information for your access to the Evolution program.<br /><br /><b><a target=""_blank"" href=""https://www.jetnetevolution.com"">www.jetnetevolution.com</a></b><br /><br />"
        HTML_Body += "<table border=""1"" cellspacing=""0"" cellpadding=""5"">"
        HTML_Body += "<tr><th align=""left"">Subscription ID : </th>"
        HTML_Body += "<th align=""right"">" + sublogon_subid.Trim + "</th></tr>"
        HTML_Body += "<tr><th align=""left"">Email Address : </th>"
        HTML_Body += "<th align=""right"">" + emailAddress.Trim + "</th></tr>"
        HTML_Body += "<tr><th align=""left"">Password : </th>"
        HTML_Body += "<th align=""right"">" + sublogon_password.Trim + "</th></tr>"
        HTML_Body += "</table>"
        HTML_Body += "<br /><b>(Use only lower case letters on the Password)</b><br /><br />"
        HTML_Body += "Click the following link to view the Evolution user guide (PDF) <a target=""_blank"" title=""Evolution User Guide"" href=""https://www.jetnetevolution.com/help/evolution_user_guide.pdf"">User Guide</a><br /><br />"
        HTML_Body += "If the technical staff of JETNET can assist you in any way, please do not hesitate to call 800-553-8638, Ext 1, and we will be happy to assist you.<br /><br />Best regards<br /><br />"
        HTML_Body += "<span style=""font-size:10.5pt; font-family Arial; color:#616E7D"">"
        HTML_Body += "<em><b>Customer Technical Support</b></em><br />"
        HTML_Body += "<a href=""mailto:customerservice@jetnet.com?Subject=Customer Technical Support"">customerservice@jetnet.com</a><br />"
        HTML_Body += "<em><b>JETNET LLC</b></em><br />"
        HTML_Body += "<em>Worldwide leader in aviation market intelligence.</em><br />"
        HTML_Body += "101 First St. | Utica, NY 13501 USA |<br />"
        HTML_Body += "Main Office: 800.553.8638 >> N.Y. Office: 315.797.4420<br />"
        HTML_Body += "<span style=""font-size:9.0pt; color:#616E7D"">"
        HTML_Body += "<a target=""_blank"" href=""https://www.jetnet.com/"" title=""https://www.jetnet.com/"">website</a> |"
        HTML_Body += "<a target=""_blank"" href=""http://www.jetstreamblog.com/"" title=""http://www.jetstreamblog.com/"">blog</a> |"
        HTML_Body += "<a target=""_blank"" href=""http://www.twitter.com/jetnetllc"" title=""http://www.twitter.com/jetnetllc"">twitter</a> |"
        HTML_Body += "<a target=""_blank"" href=""http://www.jetnetglobal.com/"" title=""http://www.jetnetglobal.com/"">ABI</a>"
        HTML_Body += "</span></span>"
        HTML_Body += "</body></html>"

        aclsData_Temp.InsertMailQueue(sub_comp_id, subins_contact_id, sublogon_subid, emailAddress.Trim, HTML_Body)

        return_text = "Password has been sent to " + emailAddress.Trim + ". Please have user check email in a few minutes."



        Return return_text

    End Function

#End Region

#Region "client script functions"

    Public Sub add_ValidateEmailAddress_Script(ByVal tbSource As TextBox)

        'Register the script block
        Dim sScptStr As StringBuilder = New StringBuilder()

        If Not Page.ClientScript.IsClientScriptBlockRegistered("vea-tb-onblur") Then

            sScptStr.Append("<script type=""text/javascript"">")
            sScptStr.Append(vbCrLf & "  function validateEmailAddress() {")
            sScptStr.Append(vbCrLf & "    var txttext = document.getElementById(""" + tbSource.ClientID.ToString + """).value;")
            sScptStr.Append(vbCrLf & "    var regex = /^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i;")

            sScptStr.Append(vbCrLf & "    if (eval(regex.test(txttext)) == false && txttext != """") {")
            sScptStr.Append(vbCrLf & "      alert('Please enter a valid e-mail address i.e. ""youremail@email.com""');")
            sScptStr.Append(vbCrLf & "      document.getElementById(""" + tbSource.ClientID.ToString + """).focus();")
            sScptStr.Append(vbCrLf & "    }")
            sScptStr.Append(vbCrLf & "  }")
            sScptStr.Append(vbCrLf & "</script>")

            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "vea-tb-onblur", sScptStr.ToString, False)

        End If

        sScptStr = Nothing

    End Sub

    Public Sub add_validateSMSPhoneNumber_Script(ByVal tbSource As TextBox)

        'Register the script block
        Dim sScptStr As StringBuilder = New StringBuilder()

        If Not Page.ClientScript.IsClientScriptBlockRegistered("vsm-tb-onblur") Then

            sScptStr.Append("<script type=""text/javascript"">")
            sScptStr.Append(vbCrLf & "  function validateSMSPhoneNumber() {")
            sScptStr.Append(vbCrLf & "    var txttext = document.getElementById(""" + tbSource.ClientID.ToString + """).value;")
            sScptStr.Append(vbCrLf & "    var regex = /^[0-9\s\(\)\+\-\.]+$/;")

            sScptStr.Append(vbCrLf & "    if (eval(regex.test(txttext)) == false && txttext != """") {")
            sScptStr.Append(vbCrLf & "      alert('Please enter only [0-9] [ ( ) + - ] or space[ ] or period[.]');")
            sScptStr.Append(vbCrLf & "      document.getElementById(""" + tbSource.ClientID.ToString + """).focus();")
            sScptStr.Append(vbCrLf & "    }")
            sScptStr.Append(vbCrLf & "  }")
            sScptStr.Append(vbCrLf & "</script>")

            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "vsm-tb-onblur", sScptStr.ToString, False)

        End If

        sScptStr = Nothing

    End Sub

    Public Sub add_validateEmailText_Script(ByVal tbSource As TextBox)

        'Register the script block
        Dim sScptStr As StringBuilder = New StringBuilder()

        If Not Page.ClientScript.IsClientScriptBlockRegistered("vet-tb-onblur") Then

            sScptStr.Append("<script type=""text/javascript"">")
            sScptStr.Append(vbCrLf + "  function validateEmailText() {")
            sScptStr.Append(vbCrLf + "    var txttext = document.getElementById(""" + tbSource.ClientID.ToString + """).value;")
            sScptStr.Append(vbCrLf + "    if (txttext.length > 455 && txttext != """") {")
            sScptStr.Append(vbCrLf + "      alert(""You have entered more than 455 characters in your email message!\nPlease correct and re-submit""); ")
            'sScptStr.Append(vbCrLf + "      document.getElementById(""" + tbSource.ClientID.ToString + """).focus();")
            sScptStr.Append(vbCrLf + "    }")
            sScptStr.Append(vbCrLf + "    else {")  '
            sScptStr.Append(vbCrLf + "      var pattern = /^([a-zA-Z0-9,\s\-\'\.\@\;\?\(\)\/]{1,455})$/gi;")
            sScptStr.Append(vbCrLf + "      if (eval(pattern.test(txttext)) == false && txttext != """") {")
            sScptStr.Append(vbCrLf + "        var m;")
            sScptStr.Append(vbCrLf + "        var ms = """";")
            sScptStr.Append(vbCrLf + "        var pattern2 = /^( )|(\_)|(\*)|(\=)|(\:)|(\>)|(\<)|(\&)|(\+)|(%2a)|(%5f)|(%3d)|(%3a)|(%3f)|(%26)|(%2b)|(%3c)|(%3e)|(&amp;lt;)|(&amp;gt;)$/gmi;")
            sScptStr.Append(vbCrLf + "        while ((m = pattern2.exec(txttext)) !== null ) { ")
            sScptStr.Append(vbCrLf + "          if ((typeof (m[0]) != ""undefined"") && (m[0] != null)) { ")
            sScptStr.Append(vbCrLf + "            ms += m[0] + "" "";")
            sScptStr.Append(vbCrLf + "          }")
            sScptStr.Append(vbCrLf + "        }")
            sScptStr.Append(vbCrLf + "        alert(""There are invalid characters in your email message!\nThese characters are not allowed["" + ms + ""].\nPlease Correct and re-submit"");")
            '  sScptStr.Append(vbCrLf + "        document.getElementById(""" + tbSource.ClientID.ToString + """).focus();")
            sScptStr.Append(vbCrLf + "      }")
            sScptStr.Append(vbCrLf + "      else {")
            sScptStr.Append(vbCrLf + "        //alert(""email contains no invalid characters"");")
            sScptStr.Append(vbCrLf + "      }")
            sScptStr.Append(vbCrLf + "    }")
            sScptStr.Append(vbCrLf + "  }")
            sScptStr.Append(vbCrLf + "</script>")

            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "vet-tb-onblur", sScptStr.ToString, False)

        End If

        sScptStr = Nothing

    End Sub

    Public Sub add_autologonAlert_Script(ByVal cbSource As CheckBox)

        'Register the script block 
        Dim sScptStr As StringBuilder = New StringBuilder()

        If Not Page.ClientScript.IsClientScriptBlockRegistered("alc-ck-onclick") Then

            sScptStr.Append("<script type=""text/javascript"">")
            sScptStr.Append(vbCrLf & "  function autologonAlert() {")
            sScptStr.Append(vbCrLf & "    if (document.getElementById(""" + cbSource.ClientID.ToString + """).checked == true) {")
            sScptStr.Append(vbCrLf & "      alert(""This option should not be used from a public computer where your personal subscription could be compromised."");")
            sScptStr.Append(vbCrLf & "    }")
            sScptStr.Append(vbCrLf & "  }")
            sScptStr.Append(vbCrLf & "</script>")

            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "alc-ck-onclick", sScptStr.ToString, False)

        End If

        sScptStr = Nothing

    End Sub

    Public Sub add_SetDefaultCompType_Script(ByVal cbSource As CheckBox, ByVal rbSource1 As RadioButton, ByVal rbSource2 As RadioButton)

        'Register the script block
        Dim sScptStr As StringBuilder = New StringBuilder()

        If Not Page.ClientScript.IsClientScriptBlockRegistered("csd-cb-onclick") Then

            sScptStr.Append("<script type=""text/javascript"">")
            sScptStr.Append(vbCrLf & "  function setdefaultCompType() {")
            sScptStr.Append(vbCrLf & "    if (document.getElementById(""" + cbSource.ClientID.ToString + """).checked == true) {")
            sScptStr.Append(vbCrLf & "      document.getElementById(""" + rbSource1.ClientID.ToString + """).disabled = false;")
            sScptStr.Append(vbCrLf & "      document.getElementById(""" + rbSource2.ClientID.ToString + """).disabled = false;")
            sScptStr.Append(vbCrLf & "    }")
            sScptStr.Append(vbCrLf & "    else {")
            sScptStr.Append(vbCrLf & "      document.getElementById(""" + rbSource1.ClientID.ToString + """).disabled = true;")
            sScptStr.Append(vbCrLf & "      document.getElementById(""" + rbSource2.ClientID.ToString + """).disabled = true;")
            sScptStr.Append(vbCrLf & "      document.getElementById(""" + rbSource1.ClientID.ToString + """).checked = false;")
            sScptStr.Append(vbCrLf & "      document.getElementById(""" + rbSource2.ClientID.ToString + """).checked = false;")
            sScptStr.Append(vbCrLf & "    }")
            sScptStr.Append(vbCrLf & "  }")
            sScptStr.Append(vbCrLf & "</script>")

            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "csd-cb-onclick", sScptStr.ToString, False)

        End If

        sScptStr = Nothing

    End Sub

    Public Sub add_ToggleDefaultCompRadioButtons_Script(ByVal rbSource1 As RadioButton, ByVal rbSource2 As RadioButton)

        'Register the script block
        Dim sScptStr As StringBuilder = New StringBuilder()

        If Not Page.ClientScript.IsClientScriptBlockRegistered("cdc-rb-onclick") Then

            sScptStr.Append("<script type=""text/javascript"">")
            sScptStr.Append(vbCrLf & "  function ToggleDefCompType() {")
            sScptStr.Append(vbCrLf & "    if (document.getElementById(""" + rbSource1.ClientID.ToString + """).checked == true) {")
            sScptStr.Append(vbCrLf & "      document.getElementById(""" + rbSource2.ClientID.ToString + """).checked = false;")
            sScptStr.Append(vbCrLf & "    }")
            sScptStr.Append(vbCrLf & "  }")
            sScptStr.Append(vbCrLf & "  function ToggleSelCompType() {")
            sScptStr.Append(vbCrLf & "    if (document.getElementById(""" + rbSource2.ClientID.ToString + """).checked == true) {")
            sScptStr.Append(vbCrLf & "      document.getElementById(""" + rbSource1.ClientID.ToString + """).checked = false;")
            sScptStr.Append(vbCrLf & "    }")
            sScptStr.Append(vbCrLf & "  }")
            sScptStr.Append(vbCrLf & "</script>")

            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "cdc-rb-onclick", sScptStr.ToString, False)

        End If

        sScptStr = Nothing

    End Sub

    Public Sub add_ToggleDefaultEmailRadioButtons_Script(ByVal rbSource1 As RadioButton, ByVal rbSource2 As RadioButton)

        'Register the script block
        Dim sScptStr As StringBuilder = New StringBuilder()

        If Not Page.ClientScript.IsClientScriptBlockRegistered("cde-rb-onclick") Then

            sScptStr.Append("<script type=""text/javascript"">")
            sScptStr.Append(vbCrLf & "  function ToggleEmailFormatHtml() {")
            sScptStr.Append(vbCrLf & "    if (document.getElementById(""" + rbSource1.ClientID.ToString + """).checked == true) {")
            sScptStr.Append(vbCrLf & "      document.getElementById(""" + rbSource2.ClientID.ToString + """).checked = false;")
            sScptStr.Append(vbCrLf & "    }")
            sScptStr.Append(vbCrLf & "  }")
            sScptStr.Append(vbCrLf & "  function ToggleEmailFormatText() {")
            sScptStr.Append(vbCrLf & "    if (document.getElementById(""" + rbSource2.ClientID.ToString + """).checked == true) {")
            sScptStr.Append(vbCrLf & "      document.getElementById(""" + rbSource1.ClientID.ToString + """).checked = false;")
            sScptStr.Append(vbCrLf & "    }")
            sScptStr.Append(vbCrLf & "  }")
            sScptStr.Append(vbCrLf & "</script>")

            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "cde-rb-onclick", sScptStr.ToString, False)

        End If

        sScptStr = Nothing

    End Sub

    Public Sub add_MultiListEnsureItemVisible_Script(ByVal lbSource As ListBox)

        'Register the script block
        Dim sScptStr As StringBuilder = New StringBuilder()

        If Not Page.ClientScript.IsClientScriptBlockRegistered("eiv-lb-onclick") Then

            sScptStr.Append("<script type=""text/javascript"">")
            sScptStr.Append(vbCrLf & "  function MultiListEnsureItemVisible() {")
            sScptStr.Append(vbCrLf & "    var list = document.getElementById(""" + lbSource.ClientID.ToString + """);")
            sScptStr.Append(vbCrLf & "    var wasDisabled = false;")
            sScptStr.Append(vbCrLf & "    if (list.disabled) { // cant set selected items on disabled list")
            sScptStr.Append(vbCrLf & "      wasDisabled = true;")
            sScptStr.Append(vbCrLf & "      list.disabled = false;")
            sScptStr.Append(vbCrLf & "    }")
            sScptStr.Append(vbCrLf & "    if (!list || !list.multiple || list.length == 0) return;")
            sScptStr.Append(vbCrLf & "    var lastItem = list[list.length - 1];")
            sScptStr.Append(vbCrLf & "    if (lastItem.selected) {")
            sScptStr.Append(vbCrLf & "      lastItem.selected = true;")
            sScptStr.Append(vbCrLf & "      return;")
            sScptStr.Append(vbCrLf & "    }")
            sScptStr.Append(vbCrLf & "    else {")
            sScptStr.Append(vbCrLf & "      lastItem.selected = true;")
            sScptStr.Append(vbCrLf & "      lastItem.selected = false;")
            sScptStr.Append(vbCrLf & "    }")
            sScptStr.Append(vbCrLf & "    for (var i = 0; i < list.length; i++) {")
            sScptStr.Append(vbCrLf & "     if (list[i].selected) {")
            sScptStr.Append(vbCrLf & "       list[i].selected = true;")
            sScptStr.Append(vbCrLf & "         if (wasDisabled) {")
            sScptStr.Append(vbCrLf & "           list.disabled = true;")
            sScptStr.Append(vbCrLf & "         }")
            sScptStr.Append(vbCrLf & "       return;")
            sScptStr.Append(vbCrLf & "      }")
            sScptStr.Append(vbCrLf & "    }")
            sScptStr.Append(vbCrLf & "  }")
            sScptStr.Append(vbCrLf & "</script>")

            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "eiv-lb-onclick", sScptStr.ToString, False)

        End If

        sScptStr = Nothing

    End Sub

    Public Sub add_ChangeActiveTab_Script(ByVal tcSource As AjaxControlToolkit.TabContainer)

        'Register the script block
        Dim sScptStr As StringBuilder = New StringBuilder()

        If Not Page.ClientScript.IsClientScriptBlockRegistered("cht-tc-onclick") Then

            sScptStr.Append("<script type=""text/javascript"">")
            sScptStr.Append(vbCrLf + "  function changeTab(num) {")
            sScptStr.Append(vbCrLf + "    //alert(""nextTab: "" + num);")
            sScptStr.Append(vbCrLf + "    var container = $find(""" + tcSource.ClientID.ToString + """);")
            sScptStr.Append(vbCrLf + "    container.set_activeTabIndex(num);")
            sScptStr.Append(vbCrLf + "  }")
            sScptStr.Append(vbCrLf + "</script>")

            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "emp-tc-onclick", sScptStr.ToString, False)

        End If

        sScptStr = Nothing

    End Sub

    Public Sub add_EnableSMS_Script(ByVal cbSource As CheckBox, ByVal tbSource1 As TextBox, ByVal dlSource1 As DropDownList, ByVal lsSource1 As ListBox, ByVal lsSource2 As ListBox, ByVal cbSource2 As CheckBox)

        'Register the script block
        Dim sScptStr As StringBuilder = New StringBuilder()

        If Not Page.ClientScript.IsClientScriptBlockRegistered("esm-cb-onclick") Then

            sScptStr.Append("<script type=""text/javascript"">")
            sScptStr.Append(vbCrLf & "  function enableSMS() {")
            sScptStr.Append(vbCrLf & "    if (document.getElementById(""" + cbSource.ClientID.ToString + """).checked == true) {")
            sScptStr.Append(vbCrLf & "        document.getElementById(""" + tbSource1.ClientID.ToString + """).disabled = false;")
            sScptStr.Append(vbCrLf & "        document.getElementById(""" + dlSource1.ClientID.ToString + """).disabled = false;")
            sScptStr.Append(vbCrLf & "        document.getElementById(""" + lsSource1.ClientID.ToString + """).disabled = false;")
            sScptStr.Append(vbCrLf & "        document.getElementById(""" + lsSource2.ClientID.ToString + """).disabled = false;")
            sScptStr.Append(vbCrLf & "        document.getElementById(""" + cbSource2.ClientID.ToString + """).disabled = false;")
            sScptStr.Append(vbCrLf & "    }")
            sScptStr.Append(vbCrLf & "    else {")
            sScptStr.Append(vbCrLf & "      document.getElementById(""" + tbSource1.ClientID.ToString + """).disabled = true;")
            sScptStr.Append(vbCrLf & "      document.getElementById(""" + dlSource1.ClientID.ToString + """).disabled = true;")
            sScptStr.Append(vbCrLf & "      document.getElementById(""" + lsSource1.ClientID.ToString + """).disabled = true;")
            sScptStr.Append(vbCrLf & "      document.getElementById(""" + lsSource2.ClientID.ToString + """).disabled = true;")
            sScptStr.Append(vbCrLf & "      document.getElementById(""" + cbSource2.ClientID.ToString + """).disabled = true;")
            sScptStr.Append(vbCrLf & "    }")
            sScptStr.Append(vbCrLf & "  }")
            sScptStr.Append(vbCrLf & "</script>")

            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "esm-cb-onclick", sScptStr.ToString, False)

        End If

        sScptStr = Nothing

    End Sub

    Public Sub add_ChangeDefaultAirportFolder_Script(ByVal dlSource1 As DropDownList, ByVal btn1 As LinkButton, ByVal btn2 As LinkButton)

        'Register the script block
        Dim sScptStr As StringBuilder = New StringBuilder()

        If Not Page.ClientScript.IsClientScriptBlockRegistered("cdap-ddl-onchange") Then

            sScptStr.Append("<script type=""text/javascript"">")
            sScptStr.Append(vbCrLf + "  function ShowChangeDefailtAirportButton(inDefault) {")
            sScptStr.Append(vbCrLf + "    var list = document.getElementById(""" + dlSource1.ClientID.ToString + """);")
            sScptStr.Append(vbCrLf + "    for (var i = 0; i < list.length; i++) {")
            sScptStr.Append(vbCrLf + "      if (list[i].selected) {")
            sScptStr.Append(vbCrLf + "        if (list[i].value != inDefault && list[i].value != '0') {")
            sScptStr.Append(vbCrLf + "          $(""#" + btn1.ClientID.ToString + """).show();")
            sScptStr.Append(vbCrLf + "          $(""#" + btn2.ClientID.ToString + """).hide();")
            sScptStr.Append(vbCrLf + "          return;")
            sScptStr.Append(vbCrLf + "        } else {")
            sScptStr.Append(vbCrLf + "          $(""#" + btn1.ClientID.ToString + """).hide();")
            sScptStr.Append(vbCrLf + "          $(""#" + btn2.ClientID.ToString + """).show();")
            sScptStr.Append(vbCrLf + "          return;")
            sScptStr.Append(vbCrLf + "        }")
            sScptStr.Append(vbCrLf + "      }")
            sScptStr.Append(vbCrLf + "    }")
            sScptStr.Append(vbCrLf + "  }")
            sScptStr.Append(vbCrLf + "</script>")

            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "cdap-ddl-onchange", sScptStr.ToString, False)

        End If

        sScptStr = Nothing

    End Sub

#End Region

#Region "onClick_functions"

    Private Sub support_email_button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles support_email_button.Click

        ' if email text passes client validation, scrub input text for common script injection phrases (SQL and HTML)
        Dim tmpEmailString = commonEvo.scrubEmailString(support_email_textbox.Text)
        Dim EmailString As New StringBuilder


        If Len(Trim(support_email_textbox.Text)) > 455 Then
            alert_text.Text = "You have entered more than 455 characters in your email message! Please correct and re-submit."
            alert_text.Visible = True
        Else

            alert_text.Visible = False

            'Let's build the EMAIL
            EmailString.Append("<html><head>")
            EmailString.Append("<title>Evolution JETNET Customer Subscription Info</title>")
            EmailString.Append("</head><body>")
            EmailString.Append("<img src=""" & clsData_Manager_SQL.get_site_name & "/images/JN_EvolutionMarketplace_Logo2.png""><br /><br />")
            EmailString.Append("<font face=""Arial"" size=""3"">" + FormatDateTime(Now, vbGeneralDate).ToString + "<br /><br />")
            EmailString.Append("JETNET LLC<br />Utica, NY  United States<br /><br />")
            EmailString.Append("Customer " + actinfo_contact_name.Text.Replace("&nbsp;", " ").Trim + " has requested Information<br /><br />")
            EmailString.Append("<table border=""1"" cellspacing=""0"" cellpadding=""2"">")
            EmailString.Append("<tr><th align=""left"">User ID : </th><th align=""right"">" + Session.Item("localUser").crmUserLogin.ToString.Trim + "</th></tr>")
            EmailString.Append("<tr><th align=""left"">Subscription ID : </th><th align=""right"">" + Session.Item("localUser").crmSubSubID.ToString.Trim + "</th></tr>")
            EmailString.Append("<tr><th align=""left"">Install Seq No : </th><th align=""right"">" + Session.Item("localUser").crmSubSeqNo.ToString.Trim + "</th></tr>")
            EmailString.Append("<tr><th align=""left"">EMail Address : </th><th align=""right"">" + strUserEmailAddress.Trim + "</th></tr>")
            EmailString.Append("<tr><th align=""left"">Company ID : </th><th align=""right"">" + Session.Item("localUser").crmUserCompanyID.ToString.Trim + "</th></tr>")
            EmailString.Append("<tr><th align=""left"">Contact ID : </th><th align=""right"">" + Session.Item("localUser").crmUserContactID.ToString.Trim + "</th></tr>")
            Select Case (Session.Item("localPreferences").Tierlevel)
                Case eTierLevelTypes.JETS
                    EmailString.Append("<tr><th align=""left"">Tier level : </th><th align=""right"">Jets</th></tr>")
                Case eTierLevelTypes.TURBOS
                    EmailString.Append("<tr><th align=""left"">Tier level : </th><th align=""right"">Turbos</th></tr>")
                Case Else
                    EmailString.Append("<tr><th align=""left"">Tier level : </th><th align=""right"">All</th></tr>")
            End Select
            EmailString.Append("<tr><th align=""left"">Platform OS : </th><th align=""right"">" + subscription_platform.Text + "</th></tr>")
            EmailString.Append("<tr><th align=""left"">Service Code : </th><th align=""right"">" + Session.Item("localPreferences").ServiceCode.trim + " : " + Session.Item("localPreferences").ServiceName.ToString + "</th></tr>")

            EmailString.Append("<tr><th align=""left"">Aerodex : </th><th align=""right"">" + Session.Item("localPreferences").AerodexFlag.ToString + "</th></tr>")
            EmailString.Append("<tr><th align=""left"">Business : </th><th align=""right"">" + Session.Item("localPreferences").UserBusinessFlag.ToString + "</th></tr>")
            EmailString.Append("<tr><th align=""left"">Commericial : </th><th align=""right"">" + Session.Item("localPreferences").UserCommercialFlag.ToString + "</th></tr>")
            EmailString.Append("<tr><th align=""left"">Helicopter : </th><th align=""right"">" + Session.Item("localPreferences").UserHelicopterFlag.ToString + "</th></tr>")
            EmailString.Append("<tr><th align=""left"">SPI View : </th><th align=""right"">" + Session.Item("localPreferences").UserSPIViewFlag.ToString + "</th></tr>")
            EmailString.Append("<tr><th align=""left"">STAR reports : </th><th align=""right"">" + Session.Item("localPreferences").UserStarRptFlag.ToString + "</th></tr>")
            EmailString.Append("<tr><th align=""left"">Yacht : </th><th align=""right"">" + Session.Item("localPreferences").UserYachtFlag.ToString + "</th></tr>")

            EmailString.Append("<tr><th align=""left"">Mobile web : </th><th align=""right"">" + Session.Item("localPreferences").MobleWebStatus.ToString + "</th></tr>")
            EmailString.Append("<tr><th align=""left"">Mobile Number : </th><th align=""right"">" + Session.Item("localPreferences").SmsPhoneNumber.ToString + "</th></tr>")

            If (CBool(My.Settings.enableChat)) Then

                If bEnableChat Then
                    EmailString.Append("<tr><th align=""left"">Chat Enabled : </th><th align=""right"">" + Session.Item("localPreferences").ChatEnabled.ToString + "</th></tr>")
                End If

            End If

            Dim sProjectText As String = ""
            Dim bHasDefaultProject = commonEvo.CheckForProject(sProjectText)

            If Not String.IsNullOrEmpty(sProjectText) Then
                EmailString.Append("<tr><th align=""left"">Project : </th><th align=""right"">" + sProjectText + "</th></tr>")
            Else
                EmailString.Append("<tr><th align=""left"">Project : </th><th align=""right"">No Default Project</th></tr>")
            End If

            EmailString.Append("<tr><th align=""left"">Default Model : </th><th align=""right"">" + display_default_modelID.Text + "</th></tr>")
            EmailString.Append("<tr><th align=""left"">Default View : </th><th align=""right"">" + display_default_viewID.Text + "</th></tr>")

            EmailString.Append("<tr><th align=""left"">Server Notes : </th><th align=""right"">" + Session.Item("localPreferences").HasServerNotes.ToString + "</th></tr>")
            EmailString.Append("<tr><th align=""left"">Cloud Notes : </th><th align=""right"">" + Session.Item("localPreferences").HasCloudNotes.ToString + "</th></tr>")
            EmailString.Append("<tr><th align=""left"">Marketing Account : </th><th align=""right"">" + Session.Item("localPreferences").MarketingFlag.ToString + "</th></tr>")
            EmailString.Append("<tr><th align=""left"">Demo Account : </th><th align=""right"">" + Session.Item("localPreferences").DemoFlag.ToString + "</th></tr>")
            EmailString.Append("<tr><th align=""left"">Default Email Format : </th><th align=""right"">" + Session.Item("localPreferences").UserEmailDefaultFormat.ToString.Trim + "</th></tr>")
            EmailString.Append("<tr><th align=""left"">Default Reply Name : </th><th align=""right"">" + Session.Item("localPreferences").UserEmailReplyToName.ToString.Trim + "</th></tr>")
            EmailString.Append("<tr><th align=""left"">Default Reply Email : </th><th align=""right"">" + Session.Item("localPreferences").UserEmailReplyToAddress.ToString.Trim + "</th></tr>")
            EmailString.Append("<tr><th align=""left"">Show Listings on JETNET Global : </th><th align=""right"">" + Session.Item("localPreferences").ShowListingsOnGlobal.ToString + "</th></tr>")

            EmailString.Append("<tr><th align=""left"">SMS Text Msg Active : </th><th align=""right"">" + myservices_SMS_service_status.Text + "</th></tr>")

            Dim sModelsOut As String = ""
            commonEvo.fillMakeModelDropDown(Nothing, Nothing, 0, sModelsOut, sSMSSelectedModelID, False, False, True, False, False, True) ' display models
            EmailString.Append("<tr><th align=""left"">SMS Models : </th><th align=""right"">" + sModelsOut + "</th></tr>")

            EmailString.Append("<tr><th align=""left"">SMS Provider : </th><th align=""right"">" + Session.Item("localPreferences").SmsProviderName.ToString.Trim + "</th></tr>")

            Dim sEventsOut As String = ""
            localDatalayer.fillSMSEventsDropDown(Nothing, 0, sEventsOut, sSMSSelectedEvents, True)

            EmailString.Append("<tr><th align=""left"">SMS Events : </th><th align=""right"">" + sEventsOut + "</th></tr>")

            EmailString.Append("<tr><th align=""left"">User Email Request : </th><th align=""right"">" + tmpEmailString.ToLower.Trim + "</th></tr>")

            EmailString.Append("</table>")
            EmailString.Append("</body></html>")

            aclsData_Temp.InsertMailQueue(Session.Item("localUser").crmUserCompanyID, Session.Item("localUser").crmUserContactID, Session.Item("localUser").crmSubSubID, "customerservice@jetnet.com", EmailString.ToString)
            support_email_textbox.Text = ""
            System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CustomerServiceEmail", "alertEmailSent();", True)
        End If
    End Sub

    Private Sub send_password_change_email(ByVal in_EmailAddress As String)

        Dim EmailString As New StringBuilder

        'Let's build the EMAIL
        EmailString.Append("<html><head>")
        EmailString.Append("<title>Evolution JETNET, Jets and Turboprops, Helicopters, Commercial Setup License Information</title>")
        EmailString.Append("</head><body>")
        EmailString.Append("<img src=""" + clsData_Manager_SQL.get_site_name + "/images/JN_EvolutionMarketplace_Logo2.png""><br /><br />")
        EmailString.Append("<font face=""Arial"" size=""3"">" + FormatDateTime(Now, vbGeneralDate).ToString + "<br /><br />")
        EmailString.Append("JETNET LLC<br />Utica, NY  United States<br /><br />")
        EmailString.Append("Per your request, listed below is the license information for your access to the Evolution program.<br /><br /><b><a target=""_blank"" href=""" + Application.Item("crmClientSiteData").ClientFullHostName + """>" + Application.Item("crmClientSiteData").crmClientHostName + "</a></b><br /><br />")
        EmailString.Append("<table border=""1"" cellspacing=""0"" cellpadding=""5"">")
        EmailString.Append("<tr><th align=""left"">Subscription ID : </th>")
        EmailString.Append("<th align=""right"">" + Session.Item("localUser").crmSubSubID.ToString.Trim + "</th></tr>")
        EmailString.Append("<tr><th align=""left"">EMail Address : </th>")
        EmailString.Append("<th align=""right"">" + in_EmailAddress.Trim + "</th></tr>")
        EmailString.Append("</table>")
        EmailString.Append("<br /><b><font color=""red"">Per your request, your password to <a target=""_blank"" href=""" + Application.Item("crmClientSiteData").ClientFullHostName + """>" + Application.Item("crmClientSiteData").crmClientHostName + "</a> has been changed. If you did not make a change to your password please contact JETNET immediately at the customer support number below.</font></b><br /><br />")
        EmailString.Append("Click the following link to view the Evolution user guide (PDF) <a target=""_blank"" title=""Evolution User Guide"" href=""" + Application.Item("crmClientSiteData").ClientFullHostName + "/help/evolution_user_guide.pdf"">User Guide</a><br /><br />")
        EmailString.Append("If the technical staff of JETNET can assist you in any way, please do not hesitate to call 800-553-8638, Ext 1, and we will be happy to assist you.<br /><br />Best regards<br /><br />")
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

        aclsData_Temp.InsertMailQueue(Session.Item("localUser").crmUserCompanyID, Session.Item("localUser").crmUserContactID, Session.Item("localUser").crmSubSubID, strUserEmailAddress, EmailString.ToString, False, True)

    End Sub

    Protected Sub save_button2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles save_button2.Click, save_button1.Click

        Dim tempTable As DataTable
        Dim sSavedPassword As String = ""
        Dim newModelString As String = ""
        Dim newAirportString As String = ""

        Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
        Dim lDataReader As System.Data.SqlClient.SqlDataReader = Nothing
        Dim SqlCommand As New System.Data.SqlClient.SqlCommand
        Dim SqlConnection As New System.Data.SqlClient.SqlConnection

        Dim sQuery As StringBuilder = New StringBuilder()

        Select Case (tab_container_ID.ActiveTab.ID)

            Case "my_account"

                actinfo_auto_login_checkbox_CheckedChanged() 'run autologin checkbox update.

                ' check and see if user changed password > my_account panel
                If Not String.IsNullOrEmpty(oldPasswordID.Text) And Not String.IsNullOrEmpty(newPasswordID.Text) And Not String.IsNullOrEmpty(confirmPasswordID.Text) Then
                    Try

                        tempTable = localDatalayer.VerifyPassword(Session.Item("localPreferences").UserID.ToString, CLng(Session.Item("localUser").crmSubSubID.ToString), oldPasswordID.Text)

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

                        If String.Compare(oldPasswordID.Text.ToString.ToLower.Trim, sSavedPassword, True) <> 0 Then
                            System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "OldPasswordConfirmError", "pwdOldDontMatch();", True)
                        Else

                            If String.Compare(newPasswordID.Text.ToString.ToLower.Trim, confirmPasswordID.Text.ToString.ToLower.Trim, True) <> 0 Then
                                System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "NewPasswordConfirmError", "pwdConfirmDontMatch();", True)
                            Else

                                If localDatalayer.UpdatePassword(Session.Item("localPreferences").UserID.ToString, CLng(Session.Item("localUser").crmSubSubID.ToString), oldPasswordID.Text, newPasswordID.Text) Then
                                    System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "PasswordChangeSuccess", "pwdChangeSuccess();", True)
                                End If

                                ' send email of the password change
                                send_password_change_email(strUserEmailAddress)

                                ' if user has autologon selected then change users saved cookie of the password
                                If commonEvo.getUserAutoLogonCookies(Application.Item("crmClientSiteData").AutoLogonCookie, False) Then
                                    Response.Cookies.Item("crmUserPassword").Item(Session.Item("localUser").crmSubSubID.ToString) = Session.Item("localUser").EncodeBase64(newPasswordID.Text.ToString.ToLower.Trim)
                                    Response.Cookies.Item("crmUserPassword").Expires = DateTime.Now.AddDays(300)
                                End If

                                bRefreshSession = True

                            End If

                        End If

                    Catch ex As Exception

                        System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "PasswordChangeError", "pwdChangeError();", True)

                    End Try

                End If

            Case "my_display"

                ' if user picked a new default model update database with new one > my_display panel
                If Not String.IsNullOrEmpty(choose_default_modelID.SelectedValue.ToString) Then

                    If CLng(choose_default_modelID.SelectedValue.ToString) <> nDefaultModelID Then

                        localDatalayer.UpdateDefaultModel(CLng(choose_default_modelID.SelectedValue.ToString), Session.Item("localPreferences").SessionGUID)
                        nDefaultModelID = CLng(choose_default_modelID.SelectedValue.ToString)
                        bRefreshSession = True
                    End If
                Else
                    localDatalayer.UpdateDefaultModel(CLng(-1), Session.Item("localPreferences").SessionGUID)
                    bRefreshSession = True
                End If

                ' check if user changed default background update > my_display panel
                If Not String.IsNullOrEmpty(choose_default_backgroundID.SelectedValue.ToString) Then

                    If CLng(choose_default_backgroundID.SelectedValue.ToString) <> nDefaultBackgroundID Then

                        localDatalayer.UpdateDefaultBackground(CLng(choose_default_backgroundID.SelectedValue.ToString), Session.Item("localPreferences").SessionGUID)
                        bRefreshSession = True
                    End If

                End If

                ' check if user changed default relationship to aircraft > my_display panel
                'If mydisplay_enabled_default_feature.Checked And mydisplay_selected_relationships.Checked Then

                '  If mydisplay_selected_relationships.Checked Then
                '    localDatalayer.UpdateDefaultRelationship(Session.Item("localSubscription").evoCompType.ToString, Session.Item("localPreferences").SessionGUID, mydisplay_enabled_default_feature.Checked)
                '    bRefreshSession = True
                '  End If

                'Else

                '  localDatalayer.UpdateDefaultRelationship("", Session.Item("localPreferences").SessionGUID, mydisplay_enabled_default_feature.Checked)
                '  bRefreshSession = True
                'End If

                ' check if user reset default view > my_display panel
                If display_reset_default_viewID.Checked Then

                    localDatalayer.ResetDefaultView(Session.Item("localPreferences").SessionGUID)
                    bRefreshSession = True
                End If

                ' check if user changed default records per page update > my_display panel
                If Not String.IsNullOrEmpty(display_records_per_page_ddl.SelectedValue.ToString.Trim) Then

                    If IsNumeric(display_records_per_page_ddl.SelectedValue.ToString) Then

                        localDatalayer.UpdateRecordsPerPage(CLng(display_records_per_page_ddl.SelectedValue.ToString), Session.Item("localPreferences").SessionGUID)
                        bRefreshSession = True
                    End If

                End If

                ' check if user changed default business segment
                If Not String.IsNullOrEmpty(display_business_segment_ddl.SelectedValue.ToString.Trim) Then

                    localDatalayer.UpdateBusinessSegment(display_business_segment_ddl.SelectedValue.Trim, Session.Item("localPreferences").SessionGUID)
                    bRefreshSession = True

                End If

                ' check if user changed default show blank fields on ac
                If Not String.IsNullOrEmpty(display_no_blank_fields_on_aircraft_ddl.SelectedValue.ToString.Trim) Then

                    localDatalayer.UpdateShowBlankFields(display_no_blank_fields_on_aircraft_ddl.SelectedValue.Trim, Session.Item("localPreferences").SessionGUID)
                    bRefreshSession = True

                End If

                ' check if user changed Default Analysis Timeframe > my_display panel
                If Not String.IsNullOrEmpty(default_analysis_months_ddl.SelectedValue.ToString.Trim) Then

                    If IsNumeric(default_analysis_months_ddl.SelectedValue.ToString) Then

                        localDatalayer.UpdateAnalysisTimeframe(CLng(default_analysis_months_ddl.SelectedValue.ToString), Session.Item("localPreferences").SessionGUID)
                        bRefreshSession = True

                        subscription_default_analysis_months.Text = "Default Analysis Timeframe (Months):&nbsp;<em>" + default_analysis_months_ddl.SelectedValue.ToString + "</em>"

                    End If

                End If

                If bUseValues Then

                    If display_valuesChk.Checked Then
                        HttpContext.Current.Response.Cookies("evalues").Value = "true"
                        HttpContext.Current.Response.Cookies("evalues").Expires = DateTime.Now.AddDays(300)
                    Else
                        HttpContext.Current.Response.Cookies("evalues").Value = "false"
                        HttpContext.Current.Response.Cookies("evalues").Expires = DateTime.Now.AddDays(300)
                    End If

                End If


            Case "my_models"

                localDatalayer.UpdateDefaultModelMarket(models_models_picked_lb, Session.Item("localPreferences").SessionGUID, newModelString)
                bRefreshSession = True

            Case "my_airports"

            Case "my_region"

            Case "my_services"

                ' check if user checked mobile > my_services panel
                'If myservices_enable_mobile_ck.Checked Then

                '  localDatalayer.UpdateMobileFlag(HttpContext.Current.Session.Item("localPreferences").SessionGUID, myservices_enable_mobile_ck.Checked)
                '  bRefreshSession = True

                'Else

                '  localDatalayer.UpdateMobileFlag(HttpContext.Current.Session.Item("localPreferences").SessionGUID, myservices_enable_mobile_ck.Checked)
                '  bRefreshSession = True

                'End If

                ' check if user checked enable JETNET GLOBAL listings > my_services panel
                If myservices_enable_global_list_ck.Checked Then

                    localDatalayer.UpdateGLOBALListing(HttpContext.Current.Session.Item("localPreferences").SessionGUID, myservices_enable_global_list_ck.Checked)
                    bRefreshSession = True
                    subscription_show_on_global.Text = "Show Listings on JETNET GLOBAL Enabled: <em>True</em>"

                Else

                    localDatalayer.UpdateGLOBALListing(HttpContext.Current.Session.Item("localPreferences").SessionGUID, myservices_enable_global_list_ck.Checked)
                    bRefreshSession = True
                    subscription_show_on_global.Text = "Show Listings on JETNET GLOBAL Enabled: <em>False</em>"

                End If

                ' check if user checked enable chat > my_services panel
                If (CBool(My.Settings.enableChat)) Then

                    If myservices_enable_chat_ck.Checked Then

                        HttpContext.Current.Session.Item("bHadOtherChatSubscription") = ChatManager.checkForOtherChatSubscriptions(strUserEmailAddress, 0, "", 0)

                        If Not CBool(Session.Item("bHadOtherChatSubscription").ToString) Then

                            ChatManager.UpdateChatStatus(HttpContext.Current.Session.Item("localPreferences").SessionGUID, strUserEmailAddress, myservices_enable_chat_ck.Checked, False)

                            bRefreshSession = True

                        End If

                    Else

                        ChatManager.UpdateChatStatus(HttpContext.Current.Session.Item("localPreferences").SessionGUID, strUserEmailAddress, myservices_enable_chat_ck.Checked, False)

                        bRefreshSession = True

                    End If

                End If

                ' check if user changed email request > my_services panel
                If Not String.IsNullOrEmpty(myservices_email_name_txt.Text.Trim) And Not String.IsNullOrEmpty(myservice_email_address_txt.Text.Trim) Then

                    localDatalayer.UpdateEmailRequest(Session.Item("localPreferences").SessionGUID, myservices_email_name_txt.Text.Trim, myservice_email_address_txt.Text.Trim, myservices_email_format_html.Checked)
                    bRefreshSession = True

                End If

                Dim sSMSEvents As String = ""
                Dim sSMSModels As String = ""

                ' check if user checked SMS > my_services panel
                If myservices_enable_SMS_ck.Checked And Not String.IsNullOrEmpty(myservices_sms_phone_number.Text) Then

                    Dim bConfirmSMSInfo As Boolean = myservices_terms_and_conditions_ck.Checked

                    Dim sCarrierName As String = ""
                    Dim sSaveNumber As String = ""
                    Dim nCarrierID As Integer = 0

                    If sSMSActivationStatus = crmWebClient.Constants.SMS_ACTIVATE_YES Or sSMSActivationStatus = crmWebClient.Constants.SMS_ACTIVATE_NO Then

                        sSaveNumber = myservices_sms_phone_number.Text.Replace("(", "")
                        sSaveNumber = sSaveNumber.Replace(")", "")
                        sSaveNumber = sSaveNumber.Replace("-", "")
                        sSaveNumber = sSaveNumber.Replace(" ", "")

                        nCarrierID = CInt(myservices_SMS_providers.SelectedValue.ToString)

                        sCarrierName = commonEvo.ReturnSMSProviderName(nCarrierID).Replace("&nbsp;", "")

                        bIsPhoneUnique = commonEvo.CheckUniquePhoneNumber(Me.Session, sSMSNumber)

                        If bIsPhoneUnique Then
                            System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SMSPhoneNotUnique", "alertSMSPhoneNotUnique();", True)
                        End If

                        ' get selected values
                        For i As Integer = 0 To myservices_models_to_monitor.Items.Count - 1

                            If myservices_models_to_monitor.Items(i).Selected Then
                                If String.IsNullOrEmpty(sSMSModels) Then
                                    sSMSModels = myservices_models_to_monitor.Items(i).Value
                                Else
                                    sSMSModels &= "," + myservices_models_to_monitor.Items(i).Value
                                End If
                            End If

                        Next

                        ' get selected values
                        For i As Integer = 0 To myservices_events_to_monitor.Items.Count - 1

                            If myservices_events_to_monitor.Items(i).Selected Then
                                If String.IsNullOrEmpty(sSMSEvents) Then
                                    sSMSEvents = myservices_events_to_monitor.Items(i).Value
                                Else
                                    sSMSEvents &= "," + myservices_events_to_monitor.Items(i).Value
                                End If
                            End If

                        Next

                    End If ' if sSMSActivationStatus = commonEVO.SMS_ACTIVATE_YES or sSMSActivationStatus = commonEVO.SMS_ACTIVATE_NO 

                    If bConfirmSMSInfo And sSMSActivationStatus = crmWebClient.Constants.SMS_ACTIVATE_NO Then

                        localDatalayer.UpdateSMSActivation(Session.Item("localPreferences").SessionGUID, sSaveNumber, nCarrierID, sCarrierName, sSMSEvents, sSMSModels, bIsPhoneUnique, sSMSActivationStatus)

                        sSMSActivationStatus = crmWebClient.Constants.SMS_ACTIVATE_PENDING

                        bRefreshSession = True

                    ElseIf sSMSActivationStatus <> crmWebClient.Constants.SMS_ACTIVATE_NO Then ' user has confirmed inital activation 

                        localDatalayer.UpdateSMSActivation(Session.Item("localPreferences").SessionGUID, sSaveNumber, nCarrierID, sCarrierName, sSMSSelectedEvents, sSMSSelectedModelID, bIsPhoneUnique, sSMSActivationStatus)
                        bRefreshSession = True

                    ElseIf Not bConfirmSMSInfo Then

                        System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SMSServiceNotConfirmed", "alertSMSNotConfirmed();", True)

                    End If ' if session("SMSActivationStatus") <> SMS_ACTIVATE_PENDING AND session("SMSActivationStatus") <> SMS_ACTIVATE_WAIT then


                ElseIf sSMSActivationStatus = crmWebClient.Constants.SMS_ACTIVATE_YES Then

                    localDatalayer.UpdateSMSActivation(Session.Item("localPreferences").SessionGUID, "", 0, "", "", "", True, sSMSActivationStatus)
                    bRefreshSession = True

                End If

            Case "my_notes"

                localDatalayer.UpdateNotesIndicatorOnListing(Session.Item("localPreferences").SessionGUID, myservernotes_ac_notes_listing_ck.Checked)
                bRefreshSession = True

            Case "my_company"

                If nMPMPrefID = 0 Then
                    localDatalayer.InsertMPMClientPreferences(ac_category_1.Text.Trim, ac_category_2.Text.Trim, ac_category_3.Text.Trim, ac_category_4.Text.Trim, ac_category_5.Text.Trim, ac_category_6.Text.Trim, ac_category_7.Text.Trim, ac_category_8.Text.Trim, ac_category_9.Text.Trim, ac_category_10.Text.Trim,
                                                              IIf(ac_category_1_use.Checked, "Y", ""), IIf(ac_category_2_use.Checked, "Y", ""), IIf(ac_category_3_use.Checked, "Y", ""), IIf(ac_category_4_use.Checked, "Y", ""), IIf(ac_category_5_use.Checked, "Y", ""), IIf(ac_category_6_use.Checked, "Y", ""), IIf(ac_category_7_use.Checked, "Y", ""), IIf(ac_category_8_use.Checked, "Y", ""), IIf(ac_category_9_use.Checked, "Y", ""), IIf(ac_category_10_use.Checked, "Y", ""),
                                                              pref_1.Text.Trim, pref_2.Text.Trim, pref_3.Text.Trim, pref_4.Text.Trim, pref_5.Text.Trim,
                                                              IIf(pref_1_use.Checked, "Y", ""), IIf(pref_2_use.Checked, "Y", ""), IIf(pref_3_use.Checked, "Y", ""), IIf(pref_4_use.Checked, "Y", ""), IIf(pref_5_use.Checked, "Y", ""),
                                                              CLng(IIf(IsNumeric(maximum_records_export.Text.Trim), maximum_records_export.Text.Trim, "0")))

                Else
                    localDatalayer.UpdateMPMClientPreferences(ac_category_1.Text.Trim, ac_category_2.Text.Trim, ac_category_3.Text.Trim, ac_category_4.Text.Trim, ac_category_5.Text.Trim, ac_category_6.Text.Trim, ac_category_7.Text.Trim, ac_category_8.Text.Trim, ac_category_9.Text.Trim, ac_category_10.Text.Trim,
                                                              IIf(ac_category_1_use.Checked, "Y", ""), IIf(ac_category_2_use.Checked, "Y", ""), IIf(ac_category_3_use.Checked, "Y", ""), IIf(ac_category_4_use.Checked, "Y", ""), IIf(ac_category_5_use.Checked, "Y", ""), IIf(ac_category_6_use.Checked, "Y", ""), IIf(ac_category_7_use.Checked, "Y", ""), IIf(ac_category_8_use.Checked, "Y", ""), IIf(ac_category_9_use.Checked, "Y", ""), IIf(ac_category_10_use.Checked, "Y", ""),
                                                              pref_1.Text.Trim, pref_2.Text.Trim, pref_3.Text.Trim, pref_4.Text.Trim, pref_5.Text.Trim,
                                                              IIf(pref_1_use.Checked, "Y", ""), IIf(pref_2_use.Checked, "Y", ""), IIf(pref_3_use.Checked, "Y", ""), IIf(pref_4_use.Checked, "Y", ""), IIf(pref_5_use.Checked, "Y", ""),
                                                              CLng(IIf(IsNumeric(maximum_records_export.Text.Trim), maximum_records_export.Text.Trim, "0")), nMPMPrefID)
                End If

                ' clean up company preferences

                If localDatalayer.DeleteMPMCustomExportItem("Company") Then

                    If Not String.IsNullOrEmpty(pref_1.Text.Trim) And pref_1_use.Checked Then
                        localDatalayer.InsertMPMCustomExportItem(pref_1.Text.Trim, "Company", "1")
                    End If

                    If Not String.IsNullOrEmpty(pref_2.Text.Trim) And pref_2_use.Checked Then
                        localDatalayer.InsertMPMCustomExportItem(pref_2.Text.Trim, "Company", "2")
                    End If

                    If Not String.IsNullOrEmpty(pref_3.Text.Trim) And pref_3_use.Checked Then
                        localDatalayer.InsertMPMCustomExportItem(pref_3.Text.Trim, "Company", "3")
                    End If

                    If Not String.IsNullOrEmpty(pref_4.Text.Trim) And pref_4_use.Checked Then
                        localDatalayer.InsertMPMCustomExportItem(pref_4.Text.Trim, "Company", "4")
                    End If

                    If Not String.IsNullOrEmpty(pref_5.Text.Trim) And pref_5_use.Checked Then
                        localDatalayer.InsertMPMCustomExportItem(pref_5.Text.Trim, "Company", "5")
                    End If

                End If

                If localDatalayer.DeleteMPMCustomExportItem("Aircraft") Then

                    If Not String.IsNullOrEmpty(ac_category_1.Text.Trim) And ac_category_1_use.Checked Then
                        localDatalayer.InsertMPMCustomExportItem(ac_category_1.Text.Trim, "Aircraft", "1")
                    End If

                    If Not String.IsNullOrEmpty(ac_category_2.Text.Trim) And ac_category_2_use.Checked Then
                        localDatalayer.InsertMPMCustomExportItem(ac_category_2.Text.Trim, "Aircraft", "2")
                    End If

                    If Not String.IsNullOrEmpty(ac_category_3.Text.Trim) And ac_category_3_use.Checked Then
                        localDatalayer.InsertMPMCustomExportItem(ac_category_3.Text.Trim, "Aircraft", "3")
                    End If

                    If Not String.IsNullOrEmpty(ac_category_4.Text.Trim) And ac_category_4_use.Checked Then
                        localDatalayer.InsertMPMCustomExportItem(ac_category_4.Text.Trim, "Aircraft", "4")
                    End If

                    If Not String.IsNullOrEmpty(ac_category_5.Text.Trim) And ac_category_5_use.Checked Then
                        localDatalayer.InsertMPMCustomExportItem(ac_category_5.Text.Trim, "Aircraft", "5")
                    End If

                    If Not String.IsNullOrEmpty(ac_category_6.Text.Trim) And ac_category_6_use.Checked Then
                        localDatalayer.InsertMPMCustomExportItem(ac_category_6.Text.Trim, "Aircraft", "6")
                    End If

                    If Not String.IsNullOrEmpty(ac_category_7.Text.Trim) And ac_category_7_use.Checked Then
                        localDatalayer.InsertMPMCustomExportItem(ac_category_7.Text.Trim, "Aircraft", "7")
                    End If

                    If Not String.IsNullOrEmpty(ac_category_8.Text.Trim) And ac_category_8_use.Checked Then
                        localDatalayer.InsertMPMCustomExportItem(ac_category_8.Text.Trim, "Aircraft", "8")
                    End If

                    If Not String.IsNullOrEmpty(ac_category_9.Text.Trim) And ac_category_9_use.Checked Then
                        localDatalayer.InsertMPMCustomExportItem(ac_category_9.Text.Trim, "Aircraft", "9")
                    End If

                    If Not String.IsNullOrEmpty(ac_category_10.Text.Trim) And ac_category_10_use.Checked Then
                        localDatalayer.InsertMPMCustomExportItem(ac_category_10.Text.Trim, "Aircraft", "10")
                    End If

                End If

        End Select

        Select Case tab_container_ID.ActiveTab.ID

            Case "my_display"

                commonEvo.fillMakeModelDropDown(choose_default_modelID, Nothing, nMaxWidth, "", nDefaultModelID.ToString, False, False, False, True, False, False) ' fill dropdownlist with models

            Case "my_models"

                ' fill in master model list 
                commonEvo.fillMakeModelDropDown(Nothing, models_model_lb, nMaxWidth, "", newModelString, False, False, True, False, False, False) ' fill list with models

                ' this will take selected models and move to picked models
                clsGeneral.clsGeneral.AddBtn_Click(models_models_picked_lb, models_model_lb)

            Case "my_airports"

                Dim previousDefaultAirport As Long = 0

                If Not IsNothing(HttpContext.Current.Session.Item("currentDefaultAirportFolderID")) Then
                    If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("currentDefaultAirportFolderID").ToString.Trim) Then
                        previousDefaultAirport = CLng(HttpContext.Current.Session.Item("currentDefaultAirportFolderID").ToString)
                    End If
                End If

                localDatalayer.saveAsDefaultAirportFolder(previousDefaultAirport, CLng(default_airport_ddl.SelectedValue.Trim))

            Case "my_users"

                fillSubscriberUserList()

        End Select

        'checkForDefaultCompRelationship()

        ' this will reload session variables
        ' but inorder for the site to reflect the changes the main window needs to reload
        ' to pick up the changed values from the session

        tempTable = aclsData_Temp.MyEvolutionReloadSubscriptionQuery(Session.Item("localPreferences").sessionGUID)
        clsGeneral.clsGeneral.Reload_Evolution_Subscription(aclsData_Temp, tempTable)

        tempTable = Nothing

    End Sub

    Protected Sub actinfo_auto_login_checkbox_CheckedChanged() '(ByVal sender As Object, ByVal e As EventArgs) Handles actinfo_auto_login_checkbox.CheckedChanged

        ' check and see if user checked auto logon check box > my_account panel
        If actinfo_auto_login_checkbox.Checked = True Then
            Response.Cookies.Item(Application.Item("crmClientSiteData").AutoLogonCookie).Item(Session.Item("localUser").crmSubSubID.ToString) = True
            Response.Cookies.Item(Application.Item("crmClientSiteData").AutoLogonCookie).Expires = DateTime.Now.AddDays(300)
        Else
            Response.Cookies.Item(Application.Item("crmClientSiteData").AutoLogonCookie).Item(Session.Item("localUser").crmSubSubID.ToString) = False
        End If

        bRefreshSession = True

    End Sub

    Private Sub models_move_all_left_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles models_move_all_left.Click
        clsGeneral.clsGeneral.RemoveAllBtn_Click(sender, e, models_models_picked_lb, models_model_lb)
    End Sub

    Private Sub models_move_left_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles models_move_left.Click
        clsGeneral.clsGeneral.RemoveBtn_Click(sender, e, models_models_picked_lb, models_model_lb)
    End Sub

    Private Sub models_move_right_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles models_move_right.Click
        clsGeneral.clsGeneral.AddBtn_Click(models_models_picked_lb, models_model_lb)
    End Sub

    Private Sub models_move_all_right_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles models_move_all_right.Click
        clsGeneral.clsGeneral.AddAllBtn_Click(sender, e, models_models_picked_lb, models_model_lb)
    End Sub

    Private Sub LinkButton3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton3.Click

        Dim previousDefaultAirport As Long = 0

        If Not IsNothing(HttpContext.Current.Session.Item("currentDefaultAirportFolderID")) Then
            If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("currentDefaultAirportFolderID").ToString.Trim) Then
                previousDefaultAirport = CLng(HttpContext.Current.Session.Item("currentDefaultAirportFolderID").ToString)
            End If
        End If

        localDatalayer.saveAsDefaultAirportFolder(previousDefaultAirport, CLng(default_airport_ddl.SelectedValue.Trim))

    End Sub

#End Region

#Region "contact_image_functions"

    Private Sub actinfo_image_upload_button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles actinfo_image_upload_button.Click

        Dim contactPicID As Long = 0

        Dim FileError As String = ""

        Dim contactImageLink As String = ""
        Dim contactImageFile As String = ""
        Dim TheFile As System.IO.FileInfo

        Dim objfilestream As IO.Stream = Nothing

        Dim imgOrigional As Drawing.Image = Nothing
        Dim imgResize As Drawing.Image = Nothing

        If (actinfo_contact_file_upload.HasFile) Then

            Try

                objfilestream = actinfo_contact_file_upload.PostedFile.InputStream
                imgOrigional = Drawing.Image.FromStream(objfilestream)

                'jpg check
                If imgOrigional.RawFormat.Equals(Drawing.Imaging.ImageFormat.Jpeg) Then

                    'Check that image does not exceed maximum dimension settings
                    If imgOrigional.Width > My.Settings.ResizeImageWidth Then
                        imgResize = resizeContactImage(imgOrigional, New Drawing.Size(600, 400))
                    End If

                    contactPicID = localDatalayer.CheckForExistingUserImageRow(Session.Item("localUser").crmUserContactID)

                    If contactPicID = 0 Then

                        If Not localDatalayer.InsertUserImage(Session.Item("localUser").crmUserContactID) Then
                            FileError = "There was a problem saving your image information to the database."
                        End If

                        contactPicID = localDatalayer.CheckForExistingUserImageRow(Session.Item("localUser").crmUserContactID)

                        contactImageLink = Session.Item("ContactPicturesFolderVirtualPath") + "/" + Session.Item("localUser").crmUserContactID.ToString + "-" + contactPicID.ToString + ".jpg"
                        contactImageFile = HttpContext.Current.Server.MapPath(contactImageLink)

                    Else

                        ' delete any "previous" contact image before saving new image
                        contactImageLink = Session.Item("ContactPicturesFolderVirtualPath") + "/" + Session.Item("localUser").crmUserContactID.ToString + "-" + contactPicID.ToString + ".jpg"
                        contactImageFile = HttpContext.Current.Server.MapPath(contactImageLink)

                        TheFile = New System.IO.FileInfo(contactImageFile)

                        If TheFile.Exists Then 'is the file actually there?
                            System.IO.File.Delete(contactImageFile) 'remove the file.
                        End If

                        If localDatalayer.RemoveUserImage(Session.Item("localUser").crmUserContactID, contactPicID) Then
                            If Not localDatalayer.InsertUserImage(Session.Item("localUser").crmUserContactID) Then
                                FileError = "There was a problem saving your image information to the database."
                            End If
                        End If

                        contactPicID = localDatalayer.CheckForExistingUserImageRow(Session.Item("localUser").crmUserContactID)

                        contactImageLink = Session.Item("ContactPicturesFolderVirtualPath") + "/" + Session.Item("localUser").crmUserContactID.ToString + "-" + contactPicID.ToString + ".jpg"
                        contactImageFile = HttpContext.Current.Server.MapPath(contactImageLink)


                    End If

                    'This means there were no errors updating/inserting to the db
                    If String.IsNullOrEmpty(FileError.Trim) Then

                        If Not IsNothing(imgResize) Then
                            imgResize.Save(contactImageFile, imgOrigional.RawFormat)
                        ElseIf Not IsNothing(imgOrigional) Then
                            imgOrigional.Save(contactImageFile, Drawing.Imaging.ImageFormat.Jpeg)
                        End If

                        actinfo_contact_image.ImageUrl = contactImageLink

                        Dim imgDisplayFolder As String = HttpContext.Current.Session.Item("jetnetFullHostName") + HttpContext.Current.Session.Item("ContactPicturesFolderVirtualPath")

                        If Not String.IsNullOrEmpty(Session.Item("localUser").crmUserContactID.ToString And contactPicID > 0) Then
                            actinfo_contact_image_large.Text = "<img src=""" + imgDisplayFolder.Trim + "/" + Session.Item("localUser").crmUserContactID.ToString + "-" + contactPicID.ToString + ".jpg"" alt=""" + actinfo_contact_name.Text.Trim + """  title=""" + actinfo_contact_name.Text.Trim + """ width=""225"" border=""1"" style=""width: 225px;"" />"
                        Else
                            actinfo_contact_image_large.Text = "<img src=""images/person.jpg"" alt=""" + actinfo_contact_name.Text.Trim + """  title=""" + actinfo_contact_name.Text.Trim + """ width=""225"" border=""1"" style=""width: 225px; "" />"
                        End If

                        FileError = "Your contact image has been uploaded."

                        actinfo_contact_edit_image_button.Text = "Change Image"
                        actinfo_contact_edit_image_panel.Visible = False
                        actinfo_contact_edit_image_button_remove.Visible = True
                        actinfo_contact_edit_image_button.Visible = True

                        bRefreshSession = True

                    End If

                Else
                    FileError = "Your picture must be a .jpg"
                End If

                objfilestream.Close()

            Catch ex As Exception
                FileError = "File Error: " + ex.Message.ToString()
            End Try
        Else
            FileError = "You have not specified a file."
        End If

        actinfo_contact_image_attention.Text = "<p align=""center"">" + FileError + "</p>"

    End Sub

    Private Sub actinfo_contact_edit_image_button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles actinfo_contact_edit_image_button.Click
        If actinfo_contact_edit_image_button.Text = "Change Image" Then
            actinfo_contact_edit_image_panel.Visible = True
            actinfo_contact_edit_image_button.Text = "Cancel Image Change"
            actinfo_contact_edit_image_button_remove.Visible = False
        Else
            actinfo_contact_edit_image_panel.Visible = False
            actinfo_contact_edit_image_button.Text = "Change Image"
            actinfo_contact_edit_image_button_remove.Visible = True
        End If
        actinfo_contact_image_attention.Text = ""
    End Sub

    Private Sub actinfo_contact_edit_image_button_remove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles actinfo_contact_edit_image_button_remove.Click

        Dim contactPicID As Long = 0
        Dim FileError As String = ""
        Dim FileName As String = ""
        Dim TheFile As System.IO.FileInfo

        actinfo_contact_image_attention.Text = ""

        'First we need to check and  see if there is actually an image row before we go ahead and remove.
        contactPicID = localDatalayer.CheckForExistingUserImageRow(Session.Item("localUser").crmUserContactID)

        If contactPicID > 0 Then
            'There was a row in the database for this person, go ahead and get rid of it:
            If localDatalayer.RemoveUserImage(Session.Item("localUser").crmUserContactID, contactPicID) Then
                FileError = "<p align=""center"">Your Image has been removed.</p>"
            Else
                FileError = "<p align=""center"">There was a problem updating the database.</p>"
            End If
        End If

        'Now we are off to check existence of the file and to remove the physical file
        FileName = HttpContext.Current.Server.MapPath(Session.Item("ContactPicturesFolderVirtualPath") + "/" + Session.Item("localUser").crmUserContactID.ToString + "-" + contactPicID.ToString + ".jpg")

        Try

            TheFile = New System.IO.FileInfo(FileName)

            If TheFile.Exists Then 'is the file actually there?

                System.IO.File.Delete(FileName) 'remove the file.

                FileError = "<p align=""center"">Your Image has been removed.</p>"

                actinfo_contact_image.ImageUrl = "images/contact.jpg"
                actinfo_contact_image_large.Text = "<img src=""images/person.jpg"" alt=""" + actinfo_contact_name.Text.Trim + """  title=""" + actinfo_contact_name.Text.Trim + """ width=""225"" border=""1"" style=""width: 225px;"" />"

                actinfo_contact_edit_image_button.Visible = False 'The edit image button doesn't need to show up, there's no image.
                actinfo_contact_edit_image_button_remove.Visible = False 'Same with the remove button.
                actinfo_contact_edit_image_panel.Visible = True 'however now the upload panel needs to be there.

            End If

        Catch ex As Exception
            FileError = "There was an error removing your file: " + ex.Message
        End Try

        actinfo_contact_image_attention.Text = FileError

    End Sub

    Private Function resizeContactImage(ByVal image As System.Drawing.Image, ByVal size As System.Drawing.Size, Optional ByVal preserveAspectRatio As Boolean = True) As System.Drawing.Image

        Dim newWidth As Integer
        Dim newHeight As Integer

        If preserveAspectRatio Then
            Dim originalWidth As Integer = image.Width
            Dim originalHeight As Integer = image.Height
            Dim percentWidth As Single = CSng(size.Width) / CSng(originalWidth)
            Dim percentHeight As Single = CSng(size.Height) / CSng(originalHeight)
            Dim percent As Single = If(percentHeight < percentWidth, percentHeight, percentWidth)
            newWidth = CInt(originalWidth * percent)
            newHeight = CInt(originalHeight * percent)
        Else
            newWidth = size.Width
            newHeight = size.Height
        End If

        Try

            Dim newImage As System.Drawing.Image = New Drawing.Bitmap(newWidth, newHeight)

            Using graphicsHandle As Graphics = Graphics.FromImage(newImage)
                graphicsHandle.InterpolationMode = InterpolationMode.HighQualityBicubic
                graphicsHandle.DrawImage(image, 0, 0, newWidth, newHeight)
            End Using

            Return newImage

        Catch ex As Exception

            actinfo_contact_image_attention.Text = "<p align=""center"">Resize File Error: " + ex.Message.ToString() + "</p>"

            Return Nothing

        End Try

    End Function

#End Region

#Region "unused_functions"

    Private Sub checkForDefaultCompRelationship()
        mydisplay_default_relationships_value.Text = ""
        mydisplay_selected_relationships_value.Text = ""

        If String.IsNullOrEmpty(Session.Item("localPreferences").DefaultCompType) Then
            mydisplay_default_relationships_value.Text = "None"
        Else
            ' change the values to real names
            Dim arrDefCompRelation = Session.Item("localPreferences").DefaultCompType.ToString.Split(Constants.cMultiDelim)

            ' need to cleanup duplicate relation types
            For xLoop As Integer = LBound(arrDefCompRelation) To UBound(arrDefCompRelation)
                For yLoop As Integer = xLoop + 1 To UBound(arrDefCompRelation)
                    If arrDefCompRelation(yLoop).ToString.Trim = arrDefCompRelation(xLoop).ToString.Trim Then arrDefCompRelation(yLoop) = ""
                Next
            Next

            For nLoop As Integer = LBound(arrDefCompRelation) To UBound(arrDefCompRelation)

                If arrDefCompRelation(nLoop).ToString.ToUpper = "ALL" Then
                    mydisplay_default_relationships_value.Text = "None"
                    Exit For
                End If

                If Not String.IsNullOrEmpty(arrDefCompRelation(nLoop)) Then
                    If String.IsNullOrEmpty(mydisplay_default_relationships_value.Text) Then
                        mydisplay_default_relationships_value.Text = commonEvo.GetReferenceType(arrDefCompRelation(nLoop).ToString.Trim)
                    Else
                        mydisplay_default_relationships_value.Text &= Constants.cSingleForwardSlash + commonEvo.GetReferenceType(arrDefCompRelation(nLoop).ToString.Trim)
                    End If
                End If
            Next

            arrDefCompRelation = Nothing

        End If

        If String.IsNullOrEmpty(Session.Item("localPreferences").CompType) Then
            mydisplay_selected_relationships_value.Text = "None"
        Else

            ' change the values to real names
            Dim arrCompRelation = Session.Item("localPreferences").CompType.ToString.Split(Constants.cMultiDelim)

            ' need to cleanup duplicate relation types
            For xLoop As Integer = LBound(arrCompRelation) To UBound(arrCompRelation)
                For yLoop As Integer = xLoop + 1 To UBound(arrCompRelation)
                    If arrCompRelation(yLoop).ToString.Trim = arrCompRelation(xLoop).ToString.Trim Then arrCompRelation(yLoop) = ""
                Next
            Next

            For nLoop As Integer = LBound(arrCompRelation) To UBound(arrCompRelation)

                If arrCompRelation(nLoop).ToString.ToUpper = "ALL" Then
                    mydisplay_selected_relationships_value.Text = "None"
                    Exit For
                End If

                If Not String.IsNullOrEmpty(arrCompRelation(nLoop).ToString) Then
                    If String.IsNullOrEmpty(mydisplay_selected_relationships_value.Text) Then
                        mydisplay_selected_relationships_value.Text = commonEvo.GetReferenceType(arrCompRelation(nLoop).ToString.Trim)
                    Else
                        mydisplay_selected_relationships_value.Text &= Constants.cSingleForwardSlash + commonEvo.GetReferenceType(arrCompRelation(nLoop).ToString.Trim)
                    End If
                End If
            Next

            arrCompRelation = Nothing

        End If

    End Sub

#End Region

#Region "mpm_custom_fields_functions"

    Private Sub edit_ac_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles edit_ac_1.Click, edit_ac_2.Click, edit_ac_3.Click, edit_ac_4.Click, edit_ac_5.Click, edit_ac_6.Click, edit_ac_7.Click, edit_ac_8.Click, edit_ac_9.Click, edit_ac_10.Click

        Dim c As Control = CType(sender, Control)

        Dim cName As String = c.ClientID

        If cName.ToUpper.Contains("EDIT_AC_1") Then

            Me.ac_category_1.Enabled = True
            Me.updateq_ac_1.Visible = True
            Me.deleteq_ac_1.Visible = False
            Me.edit_ac_1.Visible = False
            Me.cancel_ac_1.Visible = True

        ElseIf cName.ToUpper.Contains("EDIT_AC_2") Then

            Me.ac_category_2.Enabled = True
            Me.updateq_ac_2.Visible = True
            Me.deleteq_ac_2.Visible = False
            Me.edit_ac_2.Visible = False
            Me.cancel_ac_2.Visible = True

        ElseIf cName.ToUpper.Contains("EDIT_AC_3") Then

            Me.ac_category_3.Enabled = True
            Me.updateq_ac_3.Visible = True
            Me.deleteq_ac_3.Visible = False
            Me.edit_ac_3.Visible = False
            Me.cancel_ac_3.Visible = True

        ElseIf cName.ToUpper.Contains("EDIT_AC_4") Then

            Me.ac_category_4.Enabled = True
            Me.updateq_ac_4.Visible = True
            Me.deleteq_ac_4.Visible = False
            Me.edit_ac_4.Visible = False
            Me.cancel_ac_4.Visible = True

        ElseIf cName.ToUpper.Contains("EDIT_AC_5") Then

            Me.ac_category_5.Enabled = True
            Me.updateq_ac_5.Visible = True
            Me.deleteq_ac_5.Visible = False
            Me.edit_ac_5.Visible = False
            Me.cancel_ac_5.Visible = True

        ElseIf cName.ToUpper.Contains("EDIT_AC_6") Then

            Me.ac_category_6.Enabled = True
            Me.updateq_ac_6.Visible = True
            Me.deleteq_ac_6.Visible = False
            Me.edit_ac_6.Visible = False
            Me.cancel_ac_6.Visible = True

        ElseIf cName.ToUpper.Contains("EDIT_AC_7") Then

            Me.ac_category_7.Enabled = True
            Me.updateq_ac_7.Visible = True
            Me.deleteq_ac_7.Visible = False
            Me.edit_ac_7.Visible = False
            Me.cancel_ac_7.Visible = True

        ElseIf cName.ToUpper.Contains("EDIT_AC_8") Then

            Me.ac_category_8.Enabled = True
            Me.updateq_ac_8.Visible = True
            Me.deleteq_ac_8.Visible = False
            Me.edit_ac_8.Visible = False
            Me.cancel_ac_8.Visible = True

        ElseIf cName.ToUpper.Contains("EDIT_AC_9") Then

            Me.ac_category_9.Enabled = True
            Me.updateq_ac_9.Visible = True
            Me.deleteq_ac_9.Visible = False
            Me.edit_ac_9.Visible = False
            Me.cancel_ac_9.Visible = True

        ElseIf cName.ToUpper.Contains("EDIT_AC_10") Then

            Me.ac_category_10.Enabled = True
            Me.updateq_ac_10.Visible = True
            Me.deleteq_ac_10.Visible = False
            Me.edit_ac_10.Visible = False
            Me.cancel_ac_10.Visible = True

        End If

    End Sub

    Private Sub deleteq_ac_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles deleteq_ac_1.Click, deleteq_ac_2.Click, deleteq_ac_3.Click, deleteq_ac_4.Click, deleteq_ac_5.Click, deleteq_ac_6.Click, deleteq_ac_7.Click, deleteq_ac_8.Click, deleteq_ac_9.Click, deleteq_ac_10.Click

        Dim c As Control = CType(sender, Control)

        Dim cName As String = c.ClientID

        If cName.ToUpper.Contains("DELETEQ_AC_1") Then
            Me.yes_delete1.Visible = True
            Me.no_delete1.Visible = True
            Me.deleteq_ac_1.Visible = False
            Me.updateq_ac_1.Visible = False
            Me.edit_ac_1.Visible = False
            Me.deleteq_label1.Visible = True
        ElseIf cName.ToUpper.Contains("DELETEQ_AC_2") Then
            Me.yes_delete2.Visible = True
            Me.no_delete2.Visible = True
            Me.deleteq_ac_2.Visible = False
            Me.updateq_ac_2.Visible = False
            Me.edit_ac_2.Visible = False
            Me.deleteq_label2.Visible = True
        ElseIf cName.ToUpper.Contains("DELETEQ_AC_3") Then
            Me.yes_delete3.Visible = True
            Me.no_delete3.Visible = True
            Me.deleteq_ac_3.Visible = False
            Me.updateq_ac_3.Visible = False
            Me.edit_ac_3.Visible = False
            Me.deleteq_label3.Visible = True
        ElseIf cName.ToUpper.Contains("DELETEQ_AC_4") Then
            Me.yes_delete4.Visible = True
            Me.no_delete4.Visible = True
            Me.deleteq_ac_4.Visible = False
            Me.updateq_ac_4.Visible = False
            Me.edit_ac_4.Visible = False
            Me.deleteq_label4.Visible = True
        ElseIf cName.ToUpper.Contains("DELETEQ_AC_5") Then
            Me.yes_delete5.Visible = True
            Me.no_delete5.Visible = True
            Me.deleteq_ac_5.Visible = False
            Me.updateq_ac_5.Visible = False
            Me.edit_ac_5.Visible = False
            Me.deleteq_label5.Visible = True
        ElseIf cName.ToUpper.Contains("DELETEQ_AC_6") Then
            Me.yes_delete6.Visible = True
            Me.no_delete6.Visible = True
            Me.deleteq_ac_6.Visible = False
            Me.updateq_ac_6.Visible = False
            Me.edit_ac_6.Visible = False
            Me.deleteq_label6.Visible = True
        ElseIf cName.ToUpper.Contains("DELETEQ_AC_7") Then
            Me.yes_delete7.Visible = True
            Me.no_delete7.Visible = True
            Me.deleteq_ac_7.Visible = False
            Me.updateq_ac_7.Visible = False
            Me.edit_ac_7.Visible = False
            Me.deleteq_label7.Visible = True
        ElseIf cName.ToUpper.Contains("DELETEQ_AC_8") Then
            Me.yes_delete8.Visible = True
            Me.no_delete8.Visible = True
            Me.deleteq_ac_8.Visible = False
            Me.updateq_ac_8.Visible = False
            Me.edit_ac_8.Visible = False
            Me.deleteq_label8.Visible = True
        ElseIf cName.ToUpper.Contains("DELETEQ_AC_9") Then
            Me.yes_delete9.Visible = True
            Me.no_delete9.Visible = True
            Me.deleteq_ac_9.Visible = False
            Me.updateq_ac_9.Visible = False
            Me.edit_ac_9.Visible = False
            Me.deleteq_label9.Visible = True
        ElseIf cName.ToUpper.Contains("DELETEQ_AC_10") Then
            Me.yes_delete10.Visible = True
            Me.no_delete10.Visible = True
            Me.deleteq_ac_10.Visible = False
            Me.updateq_ac_10.Visible = False
            Me.edit_ac_10.Visible = False
            Me.deleteq_label10.Visible = True
        End If

    End Sub

    Private Sub updateq_ac_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles updateq_ac_1.Click, updateq_ac_2.Click, updateq_ac_3.Click, updateq_ac_4.Click, updateq_ac_5.Click, updateq_ac_6.Click, updateq_ac_7.Click, updateq_ac_8.Click, updateq_ac_9.Click, updateq_ac_10.Click
        Dim c As Control = CType(sender, Control)

        Dim cName As String = c.ClientID

        Dim cValue As String = ""
        Dim cUseField As String = ""
        Dim cField As String = ""

        Dim temp_id As Long = 0

        If cName.ToUpper.Contains("UPDATEQ_AC_1") Then

            cValue = ac_category_1.Text
            cField = "1"
            cUseField = IIf(ac_category_1_use.Checked, "Y", "")

            Me.yes_delete1.Visible = False
            Me.no_delete1.Visible = False
            Me.deleteq_ac_1.Visible = True
            Me.edit_ac_1.Visible = True
            Me.updateq_ac_1.Visible = False
            Me.cancel_ac_1.Visible = False
            Me.deleteq_label1.Visible = False
            Me.ac_category_1.Enabled = False

        ElseIf cName.ToUpper.Contains("UPDATEQ_AC_2") Then
            cValue = ac_category_2.Text
            cField = "2"
            cUseField = IIf(ac_category_2_use.Checked, "Y", "")

            Me.yes_delete2.Visible = False
            Me.no_delete2.Visible = False
            Me.deleteq_ac_2.Visible = True
            Me.edit_ac_2.Visible = True
            Me.updateq_ac_2.Visible = False
            Me.cancel_ac_2.Visible = False
            Me.deleteq_label2.Visible = False
            Me.ac_category_2.Enabled = False

        ElseIf cName.ToUpper.Contains("UPDATEQ_AC_3") Then
            cValue = ac_category_3.Text
            cField = "3"
            cUseField = IIf(ac_category_3_use.Checked, "Y", "")

            Me.yes_delete3.Visible = False
            Me.no_delete3.Visible = False
            Me.deleteq_ac_3.Visible = True
            Me.edit_ac_3.Visible = True
            Me.updateq_ac_3.Visible = False
            Me.cancel_ac_3.Visible = False
            Me.deleteq_label3.Visible = False
            Me.ac_category_3.Enabled = False

        ElseIf cName.ToUpper.Contains("UPDATEQ_AC_4") Then
            cValue = ac_category_4.Text
            cField = "4"
            cUseField = IIf(ac_category_4_use.Checked, "Y", "")

            Me.yes_delete4.Visible = False
            Me.no_delete4.Visible = False
            Me.deleteq_ac_4.Visible = True
            Me.edit_ac_4.Visible = True
            Me.updateq_ac_4.Visible = False
            Me.cancel_ac_4.Visible = False
            Me.deleteq_label4.Visible = False
            Me.ac_category_4.Enabled = False

        ElseIf cName.ToUpper.Contains("UPDATEQ_AC_5") Then
            cValue = ac_category_5.Text
            cField = "5"
            cUseField = IIf(ac_category_5_use.Checked, "Y", "")

            Me.yes_delete5.Visible = False
            Me.no_delete5.Visible = False
            Me.deleteq_ac_5.Visible = True
            Me.edit_ac_5.Visible = True
            Me.updateq_ac_5.Visible = False
            Me.cancel_ac_5.Visible = False
            Me.deleteq_label5.Visible = False
            Me.ac_category_5.Enabled = False

        ElseIf cName.ToUpper.Contains("UPDATEQ_AC_6") Then
            cValue = ac_category_6.Text
            cField = "6"
            cUseField = IIf(ac_category_6_use.Checked, "Y", "")

            Me.yes_delete6.Visible = False
            Me.no_delete6.Visible = False
            Me.deleteq_ac_6.Visible = True
            Me.edit_ac_6.Visible = True
            Me.updateq_ac_6.Visible = False
            Me.cancel_ac_6.Visible = False
            Me.deleteq_label6.Visible = False
            Me.ac_category_6.Enabled = False

        ElseIf cName.ToUpper.Contains("UPDATEQ_AC_7") Then
            cValue = ac_category_7.Text
            cField = "7"
            cUseField = IIf(ac_category_7_use.Checked, "Y", "")

            Me.yes_delete7.Visible = False
            Me.no_delete7.Visible = False
            Me.deleteq_ac_7.Visible = True
            Me.edit_ac_7.Visible = True
            Me.updateq_ac_7.Visible = False
            Me.cancel_ac_7.Visible = False
            Me.deleteq_label7.Visible = False
            Me.ac_category_7.Enabled = False

        ElseIf cName.ToUpper.Contains("UPDATEQ_AC_8") Then
            cValue = ac_category_8.Text
            cField = "8"
            cUseField = IIf(ac_category_8_use.Checked, "Y", "")

            Me.yes_delete8.Visible = False
            Me.no_delete8.Visible = False
            Me.deleteq_ac_8.Visible = True
            Me.edit_ac_8.Visible = True
            Me.updateq_ac_8.Visible = False
            Me.cancel_ac_8.Visible = False
            Me.deleteq_label8.Visible = False
            Me.ac_category_8.Enabled = False

        ElseIf cName.ToUpper.Contains("UPDATEQ_AC_9") Then
            cValue = ac_category_9.Text
            cField = "9"
            cUseField = IIf(ac_category_9_use.Checked, "Y", "")

            Me.yes_delete9.Visible = False
            Me.no_delete9.Visible = False
            Me.deleteq_ac_9.Visible = True
            Me.edit_ac_9.Visible = True
            Me.updateq_ac_9.Visible = False
            Me.cancel_ac_9.Visible = False
            Me.deleteq_label9.Visible = False
            Me.ac_category_9.Enabled = False

        ElseIf cName.ToUpper.Contains("UPDATEQ_AC_10") Then
            cValue = ac_category_10.Text
            cField = "10"
            cUseField = IIf(ac_category_10_use.Checked, "Y", "")

            Me.yes_delete10.Visible = False
            Me.no_delete10.Visible = False
            Me.deleteq_ac_10.Visible = True
            Me.edit_ac_10.Visible = True
            Me.updateq_ac_10.Visible = False
            Me.cancel_ac_10.Visible = False
            Me.deleteq_label10.Visible = False
            Me.ac_category_10.Enabled = False

        End If

        Call localDatalayer.UpdateMPMSingleClientPreference(cValue, cUseField, cField, nMPMPrefID)

        temp_id = localDatalayer.FindMPMCustomExportID("Aircraft", cField)

        If temp_id = 0 Then

            If cUseField.ToUpper.Contains("Y") Then
                Call localDatalayer.InsertMPMCustomExportItem(cValue, "Aircraft", cField)
            End If

        Else

            If cUseField.ToUpper.Contains("Y") Then
                Call localDatalayer.UpdateMPMSingleCustomExportItem(cValue, "Aircraft", cField, temp_id)
            Else
                'if its no longer in the custom export 
                Call localDatalayer.DeleteMPMCustomExportItem("Aircraft", cField)
                Call localDatalayer.DeleteMPMClientProjectReference(temp_id)
            End If

        End If

    End Sub

    Private Sub cancel_ac_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles cancel_ac_1.Click, cancel_ac_2.Click, cancel_ac_3.Click, cancel_ac_4.Click, cancel_ac_5.Click, cancel_ac_6.Click, cancel_ac_7.Click, cancel_ac_8.Click, cancel_ac_9.Click, cancel_ac_10.Click

        Dim c As Control = CType(sender, Control)

        Dim cName As String = c.ClientID

        If cName.ToUpper.Contains("CANCEL_AC_1") Then
            Me.yes_delete1.Visible = False
            Me.no_delete1.Visible = False
            Me.deleteq_ac_1.Visible = True
            Me.edit_ac_1.Visible = True
            Me.updateq_ac_1.Visible = False
            Me.cancel_ac_1.Visible = False
            Me.deleteq_label1.Visible = False
            Me.ac_category_1.Enabled = False
        ElseIf cName.ToUpper.Contains("CANCEL_AC_2") Then
            Me.yes_delete2.Visible = False
            Me.no_delete2.Visible = False
            Me.deleteq_ac_2.Visible = True
            Me.edit_ac_2.Visible = True
            Me.updateq_ac_2.Visible = False
            Me.cancel_ac_2.Visible = False
            Me.deleteq_label2.Visible = False
            Me.ac_category_2.Enabled = False
        ElseIf cName.ToUpper.Contains("CANCEL_AC_3") Then
            Me.yes_delete3.Visible = False
            Me.no_delete3.Visible = False
            Me.deleteq_ac_3.Visible = True
            Me.edit_ac_3.Visible = True
            Me.updateq_ac_3.Visible = False
            Me.cancel_ac_3.Visible = False
            Me.deleteq_label3.Visible = False
            Me.ac_category_4.Enabled = False
        ElseIf cName.ToUpper.Contains("CANCEL_AC_4") Then
            Me.yes_delete4.Visible = False
            Me.no_delete4.Visible = False
            Me.deleteq_ac_4.Visible = True
            Me.edit_ac_4.Visible = True
            Me.updateq_ac_4.Visible = False
            Me.cancel_ac_4.Visible = False
            Me.deleteq_label4.Visible = False
            Me.ac_category_4.Enabled = False
        ElseIf cName.ToUpper.Contains("CANCEL_AC_5") Then
            Me.yes_delete5.Visible = False
            Me.no_delete5.Visible = False
            Me.deleteq_ac_5.Visible = True
            Me.edit_ac_5.Visible = True
            Me.updateq_ac_5.Visible = False
            Me.cancel_ac_5.Visible = False
            Me.deleteq_label5.Visible = False
            Me.ac_category_5.Enabled = False
        ElseIf cName.ToUpper.Contains("CANCEL_AC_6") Then
            Me.yes_delete6.Visible = False
            Me.no_delete6.Visible = False
            Me.deleteq_ac_6.Visible = True
            Me.edit_ac_6.Visible = True
            Me.updateq_ac_6.Visible = False
            Me.cancel_ac_6.Visible = False
            Me.deleteq_label6.Visible = False
            Me.ac_category_6.Enabled = False
        ElseIf cName.ToUpper.Contains("CANCEL_AC_7") Then
            Me.yes_delete7.Visible = False
            Me.no_delete7.Visible = False
            Me.deleteq_ac_7.Visible = True
            Me.edit_ac_7.Visible = True
            Me.updateq_ac_7.Visible = False
            Me.cancel_ac_7.Visible = False
            Me.deleteq_label7.Visible = False
            Me.ac_category_7.Enabled = False
        ElseIf cName.ToUpper.Contains("CANCEL_AC_8") Then
            Me.yes_delete8.Visible = False
            Me.no_delete8.Visible = False
            Me.deleteq_ac_8.Visible = True
            Me.edit_ac_8.Visible = True
            Me.updateq_ac_8.Visible = False
            Me.cancel_ac_8.Visible = False
            Me.deleteq_label8.Visible = False
            Me.ac_category_8.Enabled = False
        ElseIf cName.ToUpper.Contains("CANCEL_AC_9") Then
            Me.yes_delete9.Visible = False
            Me.no_delete9.Visible = False
            Me.deleteq_ac_9.Visible = True
            Me.edit_ac_9.Visible = True
            Me.updateq_ac_9.Visible = False
            Me.cancel_ac_9.Visible = False
            Me.deleteq_label9.Visible = False
            Me.ac_category_9.Enabled = False
        ElseIf cName.ToUpper.Contains("CANCEL_AC_10") Then
            Me.yes_delete10.Visible = False
            Me.no_delete10.Visible = False
            Me.deleteq_ac_10.Visible = True
            Me.edit_ac_10.Visible = True
            Me.updateq_ac_10.Visible = False
            Me.cancel_ac_10.Visible = False
            Me.deleteq_label10.Visible = False
            Me.ac_category_10.Enabled = False
        End If

    End Sub

    Private Sub no_delete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles no_delete1.Click, no_delete2.Click, no_delete3.Click, no_delete4.Click, no_delete5.Click, no_delete6.Click, no_delete7.Click, no_delete8.Click, no_delete9.Click, no_delete10.Click

        Dim c As Control = CType(sender, Control)

        Dim cName As String = c.ClientID

        If cName.ToUpper.Contains("NO_DELETE1") Then
            Me.yes_delete1.Visible = False
            Me.no_delete1.Visible = False
            Me.deleteq_ac_1.Visible = True
            Me.edit_ac_1.Visible = True
            Me.updateq_ac_1.Visible = False
            Me.cancel_ac_1.Visible = False
            Me.deleteq_label1.Visible = False
            Me.ac_category_1.Enabled = False
        ElseIf cName.ToUpper.Contains("no_delete2") Then
            Me.yes_delete2.Visible = False
            Me.no_delete2.Visible = False
            Me.deleteq_ac_2.Visible = True
            Me.edit_ac_2.Visible = True
            Me.updateq_ac_2.Visible = False
            Me.cancel_ac_2.Visible = False
            Me.deleteq_label2.Visible = False
            Me.ac_category_2.Enabled = False
        ElseIf cName.ToUpper.Contains("NO_DELETE3") Then
            Me.yes_delete3.Visible = False
            Me.no_delete3.Visible = False
            Me.deleteq_ac_3.Visible = True
            Me.edit_ac_3.Visible = True
            Me.updateq_ac_3.Visible = False
            Me.cancel_ac_3.Visible = False
            Me.deleteq_label3.Visible = False
            Me.ac_category_4.Enabled = False
        ElseIf cName.ToUpper.Contains("NO_DELETE4") Then
            Me.yes_delete4.Visible = False
            Me.no_delete4.Visible = False
            Me.deleteq_ac_4.Visible = True
            Me.edit_ac_4.Visible = True
            Me.updateq_ac_4.Visible = False
            Me.cancel_ac_4.Visible = False
            Me.deleteq_label4.Visible = False
            Me.ac_category_4.Enabled = False
        ElseIf cName.ToUpper.Contains("NO_DELETE5") Then
            Me.yes_delete5.Visible = False
            Me.no_delete5.Visible = False
            Me.deleteq_ac_5.Visible = True
            Me.edit_ac_5.Visible = True
            Me.updateq_ac_5.Visible = False
            Me.cancel_ac_5.Visible = False
            Me.deleteq_label5.Visible = False
            Me.ac_category_5.Enabled = False
        ElseIf cName.ToUpper.Contains("NO_DELETE6") Then
            Me.yes_delete6.Visible = False
            Me.no_delete6.Visible = False
            Me.deleteq_ac_6.Visible = True
            Me.edit_ac_6.Visible = True
            Me.updateq_ac_6.Visible = False
            Me.cancel_ac_6.Visible = False
            Me.deleteq_label6.Visible = False
            Me.ac_category_6.Enabled = False
        ElseIf cName.ToUpper.Contains("NO_DELETE7") Then
            Me.yes_delete7.Visible = False
            Me.no_delete7.Visible = False
            Me.deleteq_ac_7.Visible = True
            Me.edit_ac_7.Visible = True
            Me.updateq_ac_7.Visible = False
            Me.cancel_ac_7.Visible = False
            Me.deleteq_label7.Visible = False
            Me.ac_category_7.Enabled = False
        ElseIf cName.ToUpper.Contains("NO_DELETE8") Then
            Me.yes_delete8.Visible = False
            Me.no_delete8.Visible = False
            Me.deleteq_ac_8.Visible = True
            Me.edit_ac_8.Visible = True
            Me.updateq_ac_8.Visible = False
            Me.cancel_ac_8.Visible = False
            Me.deleteq_label8.Visible = False
            Me.ac_category_8.Enabled = False
        ElseIf cName.ToUpper.Contains("NO_DELETE9") Then
            Me.yes_delete9.Visible = False
            Me.no_delete9.Visible = False
            Me.deleteq_ac_9.Visible = True
            Me.edit_ac_9.Visible = True
            Me.updateq_ac_9.Visible = False
            Me.cancel_ac_9.Visible = False
            Me.deleteq_label9.Visible = False
            Me.ac_category_9.Enabled = False
        ElseIf cName.ToUpper.Contains("NO_DELETE10") Then
            Me.yes_delete10.Visible = False
            Me.no_delete10.Visible = False
            Me.deleteq_ac_10.Visible = True
            Me.edit_ac_10.Visible = True
            Me.updateq_ac_10.Visible = False
            Me.cancel_ac_10.Visible = False
            Me.deleteq_label10.Visible = False
            Me.ac_category_10.Enabled = False
        End If

    End Sub

    Private Sub yes_delete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles yes_delete1.Click, yes_delete2.Click, yes_delete3.Click, yes_delete4.Click, yes_delete5.Click, yes_delete6.Click, yes_delete7.Click, yes_delete8.Click, yes_delete9.Click, yes_delete10.Click
        Dim c As Control = CType(sender, Control)

        Dim cName As String = c.ClientID

        Dim cField As String = ""

        If cName.ToUpper.Contains("YES_DELETE1") Then
            cField = "1"

            Me.yes_delete1.Visible = False
            Me.no_delete1.Visible = False
            Me.deleteq_ac_1.Visible = True
            Me.edit_ac_1.Visible = True
            Me.updateq_ac_1.Visible = False
            Me.cancel_ac_1.Visible = False
            Me.deleteq_label1.Visible = False
            Me.ac_category_1.Enabled = False
            Me.ac_category_1.Text = ""

        ElseIf cName.ToUpper.Contains("YES_DELETE2") Then
            cField = "2"

            Me.yes_delete2.Visible = False
            Me.no_delete2.Visible = False
            Me.deleteq_ac_2.Visible = True
            Me.edit_ac_2.Visible = True
            Me.updateq_ac_2.Visible = False
            Me.cancel_ac_2.Visible = False
            Me.deleteq_label2.Visible = False
            Me.ac_category_2.Enabled = False
            Me.ac_category_2.Text = ""

        ElseIf cName.ToUpper.Contains("YES_DELETE3") Then
            cField = "3"

            Me.yes_delete3.Visible = False
            Me.no_delete3.Visible = False
            Me.deleteq_ac_3.Visible = True
            Me.edit_ac_3.Visible = True
            Me.updateq_ac_3.Visible = False
            Me.cancel_ac_3.Visible = False
            Me.deleteq_label3.Visible = False
            Me.ac_category_3.Enabled = False
            Me.ac_category_3.Text = ""

        ElseIf cName.ToUpper.Contains("YES_DELETE4") Then
            cField = "4"

            Me.yes_delete4.Visible = False
            Me.no_delete4.Visible = False
            Me.deleteq_ac_4.Visible = True
            Me.edit_ac_4.Visible = True
            Me.updateq_ac_4.Visible = False
            Me.cancel_ac_4.Visible = False
            Me.deleteq_label4.Visible = False
            Me.ac_category_4.Enabled = False
            Me.ac_category_4.Text = ""

        ElseIf cName.ToUpper.Contains("YES_DELETE5") Then
            cField = "5"

            Me.yes_delete5.Visible = False
            Me.no_delete5.Visible = False
            Me.deleteq_ac_5.Visible = True
            Me.edit_ac_5.Visible = True
            Me.updateq_ac_5.Visible = False
            Me.cancel_ac_5.Visible = False
            Me.deleteq_label5.Visible = False
            Me.ac_category_5.Enabled = False
            Me.ac_category_5.Text = ""

        ElseIf cName.ToUpper.Contains("YES_DELETE6") Then
            cField = "6"

            Me.yes_delete6.Visible = False
            Me.no_delete6.Visible = False
            Me.deleteq_ac_6.Visible = True
            Me.edit_ac_6.Visible = True
            Me.updateq_ac_6.Visible = False
            Me.cancel_ac_6.Visible = False
            Me.deleteq_label6.Visible = False
            Me.ac_category_6.Enabled = False
            Me.ac_category_6.Text = ""

        ElseIf cName.ToUpper.Contains("YES_DELETE7") Then
            cField = "7"

            Me.yes_delete7.Visible = False
            Me.no_delete7.Visible = False
            Me.deleteq_ac_7.Visible = True
            Me.edit_ac_7.Visible = True
            Me.updateq_ac_7.Visible = False
            Me.cancel_ac_7.Visible = False
            Me.deleteq_label7.Visible = False
            Me.ac_category_7.Enabled = False
            Me.ac_category_7.Text = ""

        ElseIf cName.ToUpper.Contains("YES_DELETE8") Then
            cField = "8"

            Me.yes_delete8.Visible = False
            Me.no_delete8.Visible = False
            Me.deleteq_ac_8.Visible = True
            Me.edit_ac_8.Visible = True
            Me.updateq_ac_8.Visible = False
            Me.cancel_ac_8.Visible = False
            Me.deleteq_label8.Visible = False
            Me.ac_category_8.Enabled = False
            Me.ac_category_8.Text = ""

        ElseIf cName.ToUpper.Contains("YES_DELETE9") Then
            cField = "9"

            Me.yes_delete9.Visible = False
            Me.no_delete9.Visible = False
            Me.deleteq_ac_9.Visible = True
            Me.edit_ac_9.Visible = True
            Me.updateq_ac_9.Visible = False
            Me.cancel_ac_9.Visible = False
            Me.deleteq_label9.Visible = False
            Me.ac_category_9.Enabled = False
            Me.ac_category_9.Text = ""

        ElseIf cName.ToUpper.Contains("YES_DELETE10") Then
            cField = "10"

            Me.yes_delete10.Visible = False
            Me.no_delete10.Visible = False
            Me.deleteq_ac_10.Visible = True
            Me.edit_ac_10.Visible = True
            Me.updateq_ac_10.Visible = False
            Me.cancel_ac_10.Visible = False
            Me.deleteq_label10.Visible = False
            Me.ac_category_10.Enabled = False
            Me.ac_category_10.Text = ""

        End If

        Call localDatalayer.UpdateMPMSingleClientPreference("", "", cField, nMPMPrefID)

        'delete the custom export refereance items
        Dim temp_id As Long = localDatalayer.FindMPMCustomExportID("Aircraft", cField)
        Call localDatalayer.DeleteMPMCustomExportItem("Aircraft", cField)
        Call localDatalayer.DeleteMPMClientProjectReference(temp_id)

    End Sub

#End Region

#Region "Fill Feature Codes"

    Protected Sub fill_feature_code()

        Try

            Dim results_table As DataTable = New DataTable
            results_table = aclsData_Temp.Get_Client_Aircraft_Key_Features_List()

            If Not IsNothing(results_table) Then
                If results_table.Rows.Count > 0 Then
                    datagrid_feature_code.DataSource = results_table
                    datagrid_feature_code.DataBind()
                End If
            End If

        Catch ex As Exception
            Master.LogError("Error in Preferences.aspx.vb - fill_feature_code() - " + ex.Message)
        End Try

    End Sub

    Public Sub MyDataGrid_Delete(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)
        Try
            Dim id As TextBox = e.Item.FindControl("id_hidden")

            Dim type_hidden As TextBox = e.Item.FindControl("type_hidden")

            If aclsData_Temp.Delete_Client_Aircraft_Key_Features_List(id.Text, type_hidden.Text) = 1 Then
                main_attention.Text = "<p align=""center"">Your code has been removed.</p>"
                fill_feature_code()
            Else
                main_attention.Text = "<p align=""center"">ERROR ** Your code has NOT been removed.</p>"
            End If
        Catch ex As Exception
            Master.LogError("Error in Preferences.aspx.vb - MyDataGrid_Delete() - " + ex.Message)
        End Try
    End Sub

    Public Sub MyDataGrid_Cancel(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)
        Try
            datagrid_feature_code.EditItemIndex = -1
        Catch ex As Exception
            Master.LogError("Error in Preferences.aspx.vb - MyDataGrid_Cancel() - " + ex.Message)
        End Try
    End Sub

    Sub MyDataGrid_Edit(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
        Try
            datagrid_feature_code.EditItemIndex = CInt(E.Item.ItemIndex)
            main_attention.Text = ""
            fill_feature_code()
        Catch ex As Exception
            Master.LogError("Error in Preferences.aspx.vb - MyDataGrid_Edit() - " + ex.Message)
        End Try
    End Sub

    Private Sub insert_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles insert.Click

        Try
            If aclsData_Temp.Insert_Client_Aircraft_Key_Features_List(clickfeat_name.Text, clikfeat_type.Text, "") = 1 Then
                fill_feature_code()
                new_row.Visible = False
                add_new.Visible = True
                main_attention.Text = "<p align=""center"">Your code has been saved.</p>"
            Else
                main_attention.Text = "<p align=""center"">ERROR ** Your code has NOT been saved.</p>"
            End If
        Catch ex As Exception
            Master.LogError("Error in Preferences.aspx.vb - insert_Click() - " + ex.Message)
        End Try

    End Sub

    Private Sub add_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles add_new.Click
        Try

            new_row.Visible = True
            add_new.Visible = False
            main_attention.Text = ""
        Catch ex As Exception
            Master.LogError("Error in Preferences.aspx.vb - add_Click() - " & ex.Message)
        End Try

    End Sub

    Private Sub cancel_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles cancel.Click

        new_row.Visible = False
        add_new.Visible = True

    End Sub

#End Region


End Class