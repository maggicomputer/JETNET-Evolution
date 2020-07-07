
Imports System.IO
Partial Public Class WelcomeUser
    Inherits System.Web.UI.UserControl
    Dim aTempTable, aTempTable2 As New DataTable 'Data Tables used
    Dim error_string As String = ""
    Dim masterPage As New Object

    Public WithEvents evo_message_text As Global.System.Web.UI.WebControls.Label
    Public WithEvents searchPanelSlideOut As Global.System.Web.UI.WebControls.Label

#Region "Page Events"

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Try

            Dim sErrorString As String = ""

            If Not Session.Item("localPreferences").loadUserSession(sErrorString, CLng(Session.Item("localUser").crmSubSubID.ToString), Session.Item("localUser").crmUserLogin.ToString, CLng(Session.Item("localUser").crmSubSeqNo.ToString), CLng(Session.Item("localUser").crmUserContactID.ToString)) Then
                Response.Write("error in load preferences : " + sErrorString)
            End If

            If Session.Item("localUser").crmEvo = True Then

                Trace.Write("Start Load Preferences WelcomeUser.ascx" + Now.ToString)

                Trace.Write("End Load WelcomeUser.ascx" + Now.ToString)

                searchBoxText.Attributes.Add("title", "This feature is best used to search the following:" & vbNewLine & "Aircraft (Reg #, Ser # with or w/o Make/Model, MFR, Aircraft Airport Name/Codes)," & vbNewLine & "Aircraft Models (Brand/MFR, Model #)," & vbNewLine & "Company (Company Name, City, Company Phone #, Web Address)," & vbNewLine & "Contact (First/Last Name, Email, Phone #'s).")
                Dim titleTag As String = ""
                'titleTag = Replace(clsGeneral.clsGeneral.SettingWelcomeMessage(), ",<br /> ", "&#013;")

                titleTag = Replace(clsGeneral.clsGeneral.WelcomeMessageHover(), ",<br /> ", "&nbsp;")
                titleTag = Replace(titleTag, "<br /> ", "&nbsp;")

                'welcome_user.Text = "Welcome <a href='#' class=""white_text noBefore"" title=""" & titleTag & """ onclick=""javascript:window.open('Preferences.aspx','','scrollbars=yes,menubar=no,height=800,width=1150,resizable=yes,toolbar=no,location=no,status=no');"">" & HttpContext.Current.Session.Item("localUser").crmLocalUserFirstName & "</a>"

                '        Dim tmpDataAsOfDate As String = ""

                'If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("DataAsOfDate").ToString.Trim) Then
                '  tmpDataAsOfDate = "<span class=""white_text tiny_text""> (As Of " + FormatDateTime(HttpContext.Current.Session.Item("DataAsOfDate").ToString, DateFormat.ShortDate).Trim + ")</span>"
                'End If

                'Select Case HttpContext.Current.Session.Item("localPreferences").DatabaseType
                '  Case eDatabaseTypes.LIVE
                '    welcome_user.Text += " Live Updates"
                '  Case eDatabaseTypes.WEEKLY
                '    welcome_user.Text += " Weekly Updates" + tmpDataAsOfDate
                '  Case eDatabaseTypes.BIWEEKLY
                '    welcome_user.Text += " Biweekly Updates" + tmpDataAsOfDate
                '  Case eDatabaseTypes.MONTHLY
                '    welcome_user.Text += " Monthly Updates" + tmpDataAsOfDate
                '  Case Else
                '    welcome_user.Text += " Live Updates"

                'End Select


                logoTextClass.Attributes.Remove("class")

                If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT Then
                    auto_evolution_button.Text = "<img src=""/images/plane_icon.png"" border=""0"" alt=""Login to Evolution"" title=""Login to Evolution""/>"
                    logoTextClass.Attributes.Add("class", "three columns")
                    searchBoxText.Attributes.Add("style", "margin-right:0px !important")
                Else
                    logoTextClass.Attributes.Add("class", "threehalf columns")

                    EvoCRMDBError()
                End If

            Else
                Dim titleTag As String = ""

                welcomeContainer.Attributes.Remove("class")
                welcomeContainer.Attributes.Add("class", "sixteen columns headerHeight remove_margin")
                logoTextClass.Visible = False

                helpEvo.Attributes.Add("rel", "anylinkmenu_sub4")
                helpEvo.Attributes.Add("class", "menuanchorclass helpEvoButton")

                titleTag = Replace(clsGeneral.clsGeneral.SettingWelcomeMessage(), ",<br /> ", "&#013;")
                titleTag = Replace(titleTag, "<br /> ", "&#013;")
                welcome_user.Text = "Welcome <a href='#' class=""white_text noBefore"" title=""" & titleTag & """ onclick=""javascript:window.open('Preferences.aspx','','scrollbars=yes,menubar=no,height=800,width=1150,resizable=yes,toolbar=no,location=no,status=no');"">" & HttpContext.Current.Session.Item("localUser").crmLocalUserFirstName & " " & HttpContext.Current.Session.Item("localUser").crmLocalUserLastName & "</a>"

            End If

        Catch ex As Exception
            Dim previousException As String = ex.Message.Trim

            Try

                If Not IsNothing(masterPage) Then
                    masterPage.LogError(Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " : " + ex.Message.Trim)
                Else
                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " : " + ex.Message.Trim
                End If

            Catch ex2 As Exception

                commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " : Previous Exception Thrown[" + previousException.Trim + "] : Exception Thrown[" + ex2.Message.Trim + "]")

            End Try
        End Try

    End Sub


    Private Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            If Me.Visible Then

                If Session.Item("localUser").crmEvo = True Then
                    Select Case Session.Item("jetnetAppVersion")
                        Case Constants.ApplicationVariable.YACHT
                            If Session.Item("localSubscription").crmBusiness_Flag = True Or Session.Item("localSubscription").crmHelicopter_Flag = True Or Session.Item("localSubscription").crmCommercial_Flag = True Then
                            Else
                                SetUpPopupModal()
                            End If

                        Case Else

                            If Session.Item("isMobile") Then
                                searchPopup.Visible = True
                                If Session.Item("localSubscription").crmYacht_Flag = False Then
                                    SetUpPopupModal()
                                End If
                            End If
                    End Select
                End If

            End If

        Catch ex As Exception
            Dim previousException As String = ex.Message.Trim

            Try

                If Not IsNothing(masterPage) Then
                    masterPage.LogError(Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " : " + ex.Message.Trim)
                Else
                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " : " + ex.Message.Trim
                End If

            Catch ex2 As Exception

                commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " : Previous Exception Thrown[" + previousException.Trim + "] : Exception Thrown[" + ex2.Message.Trim + "]")

            End Try
        End Try

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Try

            If Me.Visible Then

                If Session.Item("localUser").crmEvo = True Then
                    If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO Then

                        If HttpContext.Current.Session.Item("localUser").crmUser_Evo_MPM_Flag = True Then
                            'Crm edit menu:
                            crmEvoModeScriptsToggle.Visible = True
                            crmEvoEditMenu.Visible = True
                        End If

                        Dim strPageAt As String = ""
                        strPageAt = UCase(Request.RawUrl.ToString())
                        searchBoxVisible.Visible = True

                        'Changed on 7/10/2015. This is going to check for the isMobile session variable
                        'If you're in the evolution app and swap your masterpage accordingly.
                        'Eventually the pageat part will be dropped, but some of the pages are still using evo mastertheme page
                        If Session.Item("isMobile") And (strPageAt.Contains("/HOME.ASPX") Or strPageAt.Contains("/VIEW_TEMPLATE.ASPX") Or strPageAt.Contains("/COMPANY_LISTING.ASPX") Or strPageAt.Contains("/OPERATING_LISTING.ASPX") Or strPageAt.Contains("/PERFORMANCE_LISTING.ASPX") Or strPageAt.Contains("/AIRPORT_LISTING.ASPX") Or strPageAt.Contains("/AIRCRAFT_LISTING.ASPX") Or strPageAt.Contains("/FULLTEXTSEARCH.ASPX")) Then
                            masterPage = DirectCast(Page.Master, MobileTheme)
                            isMobileVersion.Text = "true"
                            myPreferences_link.Visible = False
                            displayMobileMenuClass.Attributes.Add("class", "DetailsBrowseTable")
                            helpEvo.Visible = False
                            myPreferences_link.CssClass = "myCRM_login"
                            If Not strPageAt.Contains("/HOME.ASPX") Then
                                homeButton.Visible = True
                            Else
                                myPreferences_link.CssClass = "myCRM_login noBefore"
                            End If

                            If strPageAt.Contains("/FULLTEXTSEARCH.ASPX") Then
                                searchBoxVisible.Visible = False 'This needs to shut off on this page because it has a searchbar box of its own.
                            Else
                                searchBoxText.Attributes.Add("placeholder", "Search SN, Reg, Company, Airport")
                            End If
                        Else
                            masterPage = DirectCast(Page.Master, EvoTheme)
                        End If

                        If Not Page.IsPostBack Then
                            ' If HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.LOCAL Or HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.TEST Then
                            If Session.Item("isMobile") = False Then
                                'If strPageAt.Contains("/HOME.ASPX") Then
                                CheckForEvoAlerts()
                                'End If
                            End If
                            'End If
                        End If
                        If crmEvoEditMenu.Visible = False And EvoAlertMenu.Visible = False And Session.Item("isMobile") = False Then
                            myPreferences_link.CssClass = "myCRM_login noBefore" 'If edit menu for CRMEvo/Evo Alert Menu is not visible, this is the first link. Set nobefore class.
                        Else
                            myPreferences_link.CssClass = "myCRM_login"
                        End If

                        If EvoAlertMenu.Visible = True And crmEvoEditMenu.Visible = True Then
                            crmEvoEditMenu.CssClass = "myCRM_login" 'turn off noBefore Class since it's not the first link.
                        End If
                    ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT Then
                        searchBoxVisible.Visible = True
                        searchBoxVisible.Attributes.Add("style", "margin-top:-16px !important;")
                        masterPage = DirectCast(Page.Master, YachtTheme)
                    ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
                        masterPage = DirectCast(Page.Master, CustomerAdminTheme)
                    ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Then
                        masterPage = DirectCast(Page.Master, HomebaseTheme)
                    End If
                Else
                    'testing this process for now
                    If Trim(Request("viewID")) <> "" Or Trim(Request("ViewID")) <> "" Then
                        masterPage = DirectCast(Page.Master, EvoTheme)
                    Else
                        masterPage = DirectCast(Page.Master, main_site)
                    End If

                End If

                '---------------------------------------------This sets up the Time Zone stuff--------------------------------------------------------

                'belowWelcomeContainer.Attributes.Add("class", "headerHeightPadding")

                'logo.CssClass = "evolution_logo"
                'CRM_Logo_Text.Visible = False 'turn off large CRM Text
                If Session.Item("localUser").crmEvo = True Then



                    Select Case Session.Item("jetnetAppVersion")
                        Case Constants.ApplicationVariable.YACHT
                            logo.CssClass = "evolution_logo yachtMainLogo invert"
                            logo.ImageUrl = "~/images/JETNET_YachtSpot.png" 'swap logo
                            logo.Attributes.Add("style", "filter:invert(1);height:30px; padding-top:15px;")
                            'myPreferences_link.Text = "<i data-feather=""settings""></i>"
                            If Session.Item("localSubscription").crmBusiness_Flag = True Or Session.Item("localSubscription").crmHelicopter_Flag = True Or Session.Item("localSubscription").crmCommercial_Flag = True Then
                                'If HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.LOCAL Or HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.TEST Then
                                auto_evolution_button.Visible = True
                                'End If
                            Else
                                modalPopupsEvo.Visible = True
                                yachtSideOpener.Visible = True
                                'SetUpPopupModal()
                            End If

                        Case Else
                            logo.CssClass = "evolution_logo"
                            If Session.Item("localSubscription").crmAerodexFlag Then
                                logo.Attributes.Add("style", "margin-left:-50px;height:49px;padding-top:15px;")
                                If Session.Item("localSubscription").crmProductCode = "H" Then
                                    logo.ImageUrl = "~/images/JETNET_Aerodex.png" 'swap logo
                                Else

                                    If UCase(Session.Item("localSubscription").crmFrequency) = "LIVE" Then
                                        logo.ImageUrl = "~/images/JETNET_AerodexElite_FINAL.png" 'swap logo
                                        If HttpContext.Current.Session.Item("localUser").crmUser_Evo_MPM_Flag Then 'This means users have MPM and aerodex live
                                            logo.ImageUrl = "~/images/JETNET_MPMAerodexElite.png"
                                            logo.Attributes.Remove("style")
                                            logo.Attributes.Add("style", "margin-left:12px;height:49px;padding-top:15px;")
                                        End If
                                    Else
                                        logo.ImageUrl = "~/images/JETNET_Aerodex.png" 'swap logo
                                        If HttpContext.Current.Session.Item("localUser").crmUser_Evo_MPM_Flag Then 'This means users have MPM and aerodex that isn't live
                                            logo.ImageUrl = "~/images/JETNET_MPMAerodex.png"
                                            logo.Attributes.Remove("style")
                                            logo.Attributes.Add("style", "margin-left:12px;height:49px;padding-top:15px;")
                                        End If
                                    End If
                                End If

                            Else
                                If Session.Item("localSubscription").crmProductCode = "H" Then
                                    ' logo.ImageUrl = "~/images/JETNET_AerodexElite_FINAL.png" 'swap logo
                                    logo.ImageUrl = "~/images/JETNET_EvoMarketplace_White.png"
                                Else
                                    logo.ImageUrl = "~/images/JETNET_EvoMarketplace_White.png"
                                    If Session.Item("isMobile") = True Then
                                        logo.ImageUrl = "~/images/JETNET_EvoMarketplace_Mobile.png"
                                    End If


                                    If HttpContext.Current.Session.Item("localPreferences").UserSPIViewFlag Then
                                        If Session.Item("isMobile") = False Then
                                            logo.ImageUrl = "~/images/JETNET_MarketplaceValues.png"
                                            logo.CssClass = "evolution_ValuesLogo"
                                        End If
                                        If HttpContext.Current.Session.Item("localUser").crmUser_Evo_MPM_Flag Then 'This means users have MPM and values
                                            logo.ImageUrl = "~/images/JETNET_MPMandValues.png"
                                        End If
                                    Else
                                        If HttpContext.Current.Session.Item("localUser").crmUser_Evo_MPM_Flag Then 'This means users have MPM and not values
                                            logo.ImageUrl = "~/images/JETNET_MPMOnly.png"
                                        End If
                                    End If

                                End If
                            End If


                            ' myPreferences_link.Text = "<i data-feather=""settings""></i>" '<strong>My Evolution</strong>"

                            'Just swapping out the admin logo if we're on that side.
                            If (Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER) Then
                                logo.ImageUrl = "~/images/JETNET_EvoAdmin_Outlines.png" 'swap logo
                                logo.Attributes.Add("style", "margin-left:-50px;height:35px;")
                                'myPreferences_link.Text = "<i data-feather=""settings""></i>"
                                If Session.Item("isMobile") = False Then
                                    homePageLink.Attributes.Remove("href")
                                    homePageLink.Attributes.Add("href", "/adminHome.aspx")
                                End If
                            End If

                            'Just swapping out the homebase logo if we're on that side.
                            If (Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE) Then
                                logo.ImageUrl = "~/images/homebase.png" 'swap logo
                                logo.Attributes.Add("style", "margin-left:-50px;height:35px;")
                                'myPreferences_link.Text = "<i data-feather=""settings""></i>"
                            End If

                            If Session.Item("isMobile") = False Then
                                If Session.Item("localSubscription").crmYacht_Flag Then
                                    If Not (Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER) And Not (Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE) Then
                                        yacht_button.Visible = True
                                    End If
                                Else
                                    modalPopupsEvo.Visible = True
                                    evoSideOpener.Visible = True
                                    'SetUpPopupModal()
                                End If
                            End If



                    End Select

                    myPreferences_link.OnClientClick = "javascript:load('Preferences.aspx','Preferences','scrollbars=yes,menubar=no,height=800,width=1150,resizable=yes,toolbar=no,location=no,status=no');return false;"

                    login_to_evolution.Visible = False
                    Session("timezone") = 2
                    Session("timezone_offset") = 0

                    If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO Then
                        If Session.Item("isMobile") = False Then
                            homePageLink.Attributes.Remove("href")
                            homePageLink.Attributes.Add("href", "/home_tile.aspx")
                        End If

                        myPreferences_link.OnClientClick = "javascript:void(0);return false;"

                            break_toggle_evo.Visible = False
                            If Not Page.IsPostBack Then

                                'Figure_Out_Alerts() Removed 10/21/15 to be replaced with modal popup on login.
                                ' CheckDataAge()
                            End If
                        End If

                    Else
                    'CRM_Logo_Text.Visible = True 'turn on large CRM Text
                    break_toggle_evo.Visible = False
                    logo.ImageUrl = "~/images/JETNET_MarketplaceMan.png?v=1" 'swap logo
                    If Session.Item("localSubscription").crmSalesPriceIndex_Flag Then
                        logo.ImageUrl = "~/images/MPM_Values.png?v=1"
                    End If
                    login_to_evolution.Visible = True
                    searchBoxVisible.Visible = True
                    logo.Attributes.Add("style", "padding-top:5px;")
                    'Response.Write(Session.Item("localUser").crmUserType)
                End If

            End If
        Catch ex As Exception

            Dim previousException As String = ex.Message.Trim

            Try

                If Not IsNothing(masterPage) Then
                    masterPage.LogError(Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " : " + ex.Message.Trim)
                Else
                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " : " + ex.Message.Trim
                End If

            Catch ex2 As Exception

                commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " : Previous Exception Thrown[" + previousException.Trim + "] : Exception Thrown[" + ex2.Message.Trim + "]")

            End Try
        End Try

    End Sub

    Private Sub CheckForEvoAlerts()
        If UCase(Session.Item("localSubscription").crmFrequency) = "LIVE" Then
            'Dim JobCount As Integer = 0
            'JobCount = CountUserScheduledJobs(HttpContext.Current.Session.Item("localUser").crmSubSubID, HttpContext.Current.Session.Item("localUser").crmUserLogin, HttpContext.Current.Session.Item("localUser").crmSubSeqNo)

            'If JobCount > 0 Then
            EvoAlertMenu.Visible = True
            'Else
            '    EvoAlertMenu.Visible = False
            'End If
        End If
    End Sub

    Public Function CountUserScheduledJobs(ByVal subID As Long, ByVal userLogin As String, ByVal seqNO As Long) As Integer
        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Dim ReturnCount As Integer = 0
        Try
            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
            SqlConn.Open()

            sQuery.Append("select COUNT(*) from View_Customer_Jobs with (NOLOCK) ")
            sQuery.Append("where cfolder_jetnet_run_flag='Y' ")
            sQuery.Append("and sub_id = @subID and sublogin_login = @userLogin and subins_seq_no = @seqNo ")

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

            Dim SqlCommand As New SqlClient.SqlCommand(sQuery.ToString, SqlConn)


            SqlCommand.Parameters.AddWithValue("@subID", subID)
            SqlCommand.Parameters.AddWithValue("@userLogin", userLogin)
            SqlCommand.Parameters.AddWithValue("@seqNo", seqNO)

            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
            End Try

            If Not IsNothing(atemptable) Then
                If atemptable.Rows.Count > 0 Then
                    ReturnCount = atemptable.Rows(0).Item(0)
                End If
            End If
            SqlCommand.Dispose()
            SqlCommand = Nothing
        Catch ex As Exception
            Return Nothing

        Finally

            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing


        End Try

        Return ReturnCount

    End Function
    Public Sub SetStatusText(ByVal text As String, Optional ByRef aircraftSearch As Boolean = False)

        If Not Page.IsPostBack And aircraftSearch = True Then
            Dim jsStr As String = ""
            text = Replace(text, "'", "\'")
            text = Replace(text, vbNewLine, ", ")

            jsStr = "document.getElementById(""" & CRM_Logo_Text.ClientID & """).className = 'current_status';"
            jsStr = "document.getElementById(""" & CRM_Logo_Text.ClientID & """).innerHTML = '<div class=\'current_status_div\'>" & text & "</div>';"

            jsStr = "document.getElementById(""" & searchCriteriaToggle.ClientID & """).className += 'searchCriteria slideoutToolTip';"
            jsStr = "document.getElementById(""" & searchCriteriaToggle.ClientID & """).innerHTML = '<p title=\'Search Filters\' class=\'red\'><strong>Search Filters:</strong>" & text & "</p>';"

            If Not Page.ClientScript.IsStartupScriptRegistered("rerunSlider") Then
                System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "rerunSlider", jsStr.ToString, True)
            End If

        Else
            CRM_Logo_Text.Visible = True
            CRM_Logo_Text.CssClass = "current_status"
            CRM_Logo_Text.Text = "<div class='current_status_div'>" & text & "</div>"
            searchCriteriaToggle.CssClass = "searchCriteria slideoutToolTip"
            searchCriteriaToggle.Text = "<p title='Search Filters' class='red'><strong>Search Filters:</strong>" & text & "</p>"
            'searchCriteriaUpdate.Update()

        End If
    End Sub

#End Region
#Region "Logout"
    Private Sub logout_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles logoutButton.Click
        Try
            Dim url_string As String = "logout.aspx"

            If Session.Item("localUser").crmLocalUserID <> 0 Then
                clsGeneral.clsGeneral.LogUser(masterPage, "N")
            End If

            Response.Redirect(url_string, False)



        Catch ex As Exception
            error_string = "User_Edit_Template.ascx.vb - logout_Click() - " & ex.Message
            masterPage.LogError(error_string)
        End Try
    End Sub
    Public Sub EvoCRMDBError()

        If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO Then
            If HttpContext.Current.Session.Item("localUser").crmUser_CRM_Database_Not_Available = True Then
                'If Session.Item("localPreferences").HasServerNotes = False And Not String.IsNullOrEmpty(Session.Item("localPreferences").ServerNotesDatabaseConn) Then
                searchBoxVisible.CssClass = "searchBox margin_larger"
                welcome_user.Text += "<br /><i class=""fa fa-warning"" aria-hidden=""true""></i><span class='red_text smaller_text emphasis_text'>CRM Database is not currently available.</span>"
            End If
        End If
    End Sub
    Public Sub CheckDataAge()
        Trace.Write("Start CheckDataAge WelcomeUser.ascx.vb" + Now.ToString)

        'We're only performing this check for certain people who have jetnet.com in their username:

        If InStr(Session.Item("localUser").crmLocalUserName, "@jetnet.com") <> 0 Or InStr(Session.Item("localUser").crmLocalUserName, "@mvintech.com") <> 0 Then
            If Session.Item("localUser").crmEvo = True Then 'turned this check off for the CRM.
                'Dim aclsData_Temp As New clsData_Manager_SQL
                'aclsData_Temp.JETNET_DB = Application.Item("crmJetnetDatabase")
                Dim AgeTable As New DataTable
                If Session.Item("crmUserLogon") = True Then
                    AgeTable = masterPage.aclsData_Temp.Get_Data_Age()
                    If Not IsNothing(AgeTable) Then
                        If AgeTable.Rows.Count > 0 Then
                            If CLng(AgeTable.Rows(0).Item("viewage")) < -600 Then
                                searchBoxVisible.CssClass = "searchBox margin_larger"
                                welcome_user.Text += "<br /><span class='red_text smaller_text emphasis_text'>Flat Table Age: " & AgeTable.Rows(0).Item("viewage").ToString & "</span>"
                            End If
                        End If
                    End If
                End If
            End If
        End If
        Trace.Write("End CheckDataAge WelcomeUser.ascx.vb" + Now.ToString)

    End Sub
    'Commented out to turn on other version : modal popup on login.
    'Private Sub Figure_Out_Alerts()
    '  Dim AlertTable As New DataTable
    '  Trace.Write("Start Figure_Out_Alerts WelcomeUser.ascx.vb" + Now.ToString)

    '  'evo_message_text.Text = " 10/1/2013 - Welcome to my Message Text"
    '  If Session.Item("crmUserLogon") = True Then
    '    Dim popupDate As String = Format(DateAdd(DateInterval.Day, -90, Now()), "MM/dd/yyyy")
    '    AlertTable = masterPage.aclsData_Temp.Get_Jetnet_Notifications(HttpContext.Current.Session.Item("localUser").crmSubSubID, HttpContext.Current.Session.Item("localUser").crmUserLogin, HttpContext.Current.Session.Item("localUser").crmSubSeqNo, popupDate)
    '    If Not IsNothing(AlertTable) Then
    '      If AlertTable.Rows.Count > 0 Then
    '        evo_message_text.Text = "<marquee scrolldelay='170'><a href='#' title='Click to Read/Clear this Notice.' onclick=javascript:load('help.aspx?id=" & AlertTable.Rows(0).Item("evonot_id").ToString & "&clear=true','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');>" & AlertTable.Rows(0).Item("evonot_title").ToString & "</a></marquee>"
    '        Session.Item("localUser").crmSubscriberNotices = True
    '        break_toggle_evo.Visible = False
    '      Else
    '        evo_message_text.Text = ""
    '        Session.Item("localUser").crmSubscriberNotices = False
    '      End If
    '    Else
    '      evo_message_text.Text = ""
    '    End If
    '  End If
    '  Trace.Write("end Figure_Out_Alerts WelcomeUser.ascx.vb" + Now.ToString)

    'End Sub
    Public Sub ToggleWelcomeMessage(ByVal visible As Boolean)
        If visible = True Then
            welcome_message.Attributes("class") = "welcome_text"
        Else
            welcome_message.Attributes("class") = "display_none"
        End If
    End Sub

    Public Sub ToggleStandalone(ByVal visible As Boolean)
        If visible = True Then
            toggleStandaloneButtons.Attributes("class") = ""
            close_button.CssClass = "display_none"
            helpEvo.Attributes.Add("class", "helpEvoButton")
            close_button.Visible = False
        Else
            toggleStandaloneButtons.Attributes("class") = "display_none"
            helpEvo.Attributes.Add("class", "helpEvoButton noBefore")
            close_button.CssClass = "float_right closeButtonTopRight"
            close_button.Visible = True
            searchBoxVisible.CssClass += " noMenu "
            EvoAlertMenu.Visible = False 'turn this off if in standalone mode
        End If
    End Sub
    Public Sub ChangeHelpLink(ByVal url As String)
        helpEvo.Attributes.Add("href", url)
        crmEvoEditMenu.Visible = False
    End Sub
    'Public Sub ToggleLogoutButton(ByVal visible As Boolean)
    '    'logout.Visible = visible
    'End Sub

#End Region
    Public Sub SetExtraButtons(ByVal extraButtonsString As String)
        extraButtons.Text = extraButtonsString
        extraButtons.CssClass = "float_right specialActionsLink"
    End Sub

    Private Sub yacht_button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles yacht_button.Click, auto_evolution_button.Click
        Try

            Dim DomainString As String = "www.yacht-spotonline.com"

            If HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.LOCAL Then
                If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT Then
                    DomainString = "www.newevonet.com"
                Else
                    DomainString = "www.yachtsite.com"
                End If
            ElseIf HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.TEST Then
                If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT Then
                    DomainString = "www.jetnetevolutiontest.com"
                Else
                    DomainString = "www.yacht-spottest.com"
                End If
            ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT Then
                DomainString = "www.jetnetevolution.com"
            End If

            Dim url_string As String = "logout.aspx?url=http://" + DomainString.Trim + "/default.aspx"

            url_string += "&2=" & crmWebClient.clsGeneral.clsGeneral.EncodeBase64(Session.Item("localUser").crmLocalUserName)
            url_string += "&1=" & crmWebClient.clsGeneral.clsGeneral.EncodeBase64(Session.Item("localUser").crmLocalUserPswd)
            url_string += "&swap=true"


            If Session.Item("localUser").crmLocalUserID <> 0 Then
                clsGeneral.clsGeneral.LogUser(masterPage, "N")
            End If


            Response.Redirect(url_string, False)

        Catch ex As Exception
            error_string = "WelcomeUser.ascx.vb - yacht_button_Click() - " + ex.Message
            masterPage.LogError(error_string)
        End Try
    End Sub

    Private Sub SetUpPopupModal()


        If Not Page.ClientScript.IsClientScriptBlockRegistered("popups") Then
            Dim modalScript As StringBuilder = New StringBuilder()
            Dim modalPostbackScript As StringBuilder = New StringBuilder()

            modalPostbackScript.Append(" $(function(){")
            modalPostbackScript.Append("Sys.Application.add_load(function() {")

            modalPostbackScript.Append("jQuery(""#evoSidedialog"").dialog({")
            modalPostbackScript.Append("autoOpen: false,")
            modalPostbackScript.Append("show: {")
            modalPostbackScript.Append("effect: ""fade"",")
            modalPostbackScript.Append("duration: 500")
            modalPostbackScript.Append("},")
            modalPostbackScript.Append("modal: true,")
            modalPostbackScript.Append("dialogClass: ""welcomeUserPopup"",")
            modalPostbackScript.Append("minHeight: 130,")
            modalPostbackScript.Append("maxHeight: 130,")
            modalPostbackScript.Append("maxWidth: 750,")
            modalPostbackScript.Append("minWidth: 750,")
            modalPostbackScript.Append("draggable: false,")
            modalPostbackScript.Append("closeText:""X""")
            modalPostbackScript.Append("});")
            modalPostbackScript.Append("$(""#" & evoSideOpener.ClientID & """).click(function() {")
            modalPostbackScript.Append("jQuery(""#evoSidedialog"").dialog(""open"");")
            modalPostbackScript.Append("});")

            modalPostbackScript.Append("jQuery(""#yachtSidedialog"").dialog({")
            modalPostbackScript.Append("autoOpen: false,")
            modalPostbackScript.Append("show: {")
            modalPostbackScript.Append("effect: ""fade"",")
            modalPostbackScript.Append("duration: 500")
            modalPostbackScript.Append("},")
            modalPostbackScript.Append("modal: true,")
            modalPostbackScript.Append("dialogClass: ""welcomeUserPopup"",")
            modalPostbackScript.Append("minHeight: 130,")
            modalPostbackScript.Append("maxHeight: 130,")
            modalPostbackScript.Append("maxWidth: 750,")
            modalPostbackScript.Append("minWidth: 750,")
            modalPostbackScript.Append("draggable: false,")
            modalPostbackScript.Append("closeText:""X""")
            modalPostbackScript.Append("});")

            modalPostbackScript.Append("$(""#" & yachtSideOpener.ClientID & """).click(function() {")
            modalPostbackScript.Append("jQuery(""#yachtSidedialog"").dialog(""open"");")
            modalPostbackScript.Append("});")

            modalPostbackScript.Append("});")
            'Add before final closing, not needed
            modalScript.Append(Replace(modalPostbackScript.ToString, "Sys.Application.add_load(function() {", ""))


            modalPostbackScript.Append("});")
            System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "popupsPost", modalPostbackScript.ToString, True)
            ' Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "popupsPost", modalPostbackScript.ToString, True)
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "popups", " window.onload = function() {" & modalScript.ToString & "};", True)

        End If
    End Sub

    Private Sub evoSideOpener_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles evoSideOpener.Click
        Call commonLogFunctions.Log_User_Event_Data("UserStatistics", "User Clicked Yacht Icon", Nothing, 0, 0, 0, 0)
    End Sub


    Private Sub yachtSideOpener_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles yachtSideOpener.Click
        Call commonLogFunctions.Log_User_Event_Data("UserStatistics", "User Clicked Evolution Icon", Nothing, 0, 0, 0, 0)
    End Sub

    'Private Sub searchIcon_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles searchIcon.Click
    '  Dim SearchString As String = ""

    '  SearchString = "window.open('fullTextSearch.aspx?q=" & searchBoxText.Text & "','_blank','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');"

    '  System.Web.UI.ScriptManager.RegisterClientScriptBlock(searchUpdate, Me.GetType(), "searchScript", SearchString.ToString, True)

    'End Sub


End Class