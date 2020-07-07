Partial Public Class DisplayAircraftDetail

    Inherits System.Web.UI.Page


    Public aircraftID As Long = 0
    Public AircraftModel As Long = 0
    Public AircraftModel_JETNET As Long = 0
    Public ac_ser_nbr As String = ""
    Public journalID As Long = 0
    Public currentRecord As Long = 0
    Dim cstext1 As String = ""
    Dim cstext2 As String = ""
    Dim OtherID As Long = 0
    'This is either going to stay here, or become a session variable, but either way, this is gearing up to be
    'basically a toggle variable that will disable, change or remove some of the links to various items on this page 
    'depending on where they're coming from.
    Private CRMView As Boolean = False
    Private securityTokenLocal As String = ""
    Dim AclsData_Temp As New clsData_Manager_SQL
    Private bExtraJFWAFW As Boolean = False
    Private bFromJFWAFW As Boolean = False
    Private bFromView As Boolean = False
    Private bShowReminder As Boolean = False
    Private bShowNote As Boolean = False

    Private RunMap As Boolean = False
    Private Aircraft_Display_String As String = ""
    Private ViewAnalytics As Boolean = False
    Private JournalTable As New DataTable
    Private AircraftTable As New DataTable
    Private bHasNoBlankAcFieldsCookie As Boolean = False
    Private bShowBlankAcFields As Boolean = False
    Dim DisplayAnalyticsButton As Boolean = False
    Dim localDataLayer As New viewsDataLayer
    Dim transactionSource As String = "JETNET"
    Public AportLat As Double = 0
    Public AportLong As Double = 0
    Protected dsAircraftBrowse As DataTable = New DataTable
    Dim flight_data_temp As New flightDataFunctions
    Dim sort As String = ""
    Dim sortWay As String = ""
    Dim CRMSource As String = ""
    Dim temp_jetnet_ac_id As Long = 0
    Dim ValidatePermissions As Boolean = False
    Dim is_commercial_Ac As Boolean = False
    Private value_label As String = "eValue"
    Private value_color As String = "#078fd7"
    ' Dim temp_ac_mfr_year As Integer = 0
    Dim temp_ac_dlv_year As Integer = 0
    Dim est_aftt As String = ""
    Dim est_landings As String = ""
    Dim est_as_of_date As String = ""
    Dim est_aftt2 As String = ""
    Dim est_landings2 As String = ""
    Dim est_as_of_date2 As String = ""
    Dim est_afmp As String = ""


    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        ' if we arent logged in, and we havent passed homebase
        If Session.Item("crmUserLogon") <> True And Trim(Request("homebase")) <> "Y" Then
            Response.Redirect("Default.aspx", False)
        ElseIf HttpContext.Current.Session.Item("jetnetWebHostType") <> crmWebClient.eWebHostTypes.HOMEBASE And Trim(Request("homebase")) = "Y" Then
            ' if we arent on homebase.com, but have passed homebasee, then bad
            Response.Redirect("Default.aspx", False)
        Else

            If Not Page.ClientScript.IsClientScriptBlockRegistered("Toggle") Then
                Dim ToggleChangedScript As StringBuilder = New StringBuilder()

                ToggleChangedScript.Append(vbCrLf & " function ToggleButtons(class_name) {")
                ToggleChangedScript.Append(vbCrLf & " if (document.getElementById(""prev_button_slide"") != null) {")
                ToggleChangedScript.Append(vbCrLf & " document.getElementById(""prev_button_slide"").className = class_name;")
                ToggleChangedScript.Append(vbCrLf & " }")
                ToggleChangedScript.Append(vbCrLf & " if (document.getElementById(""next_button_slide"") != null) {")
                ToggleChangedScript.Append(vbCrLf & " document.getElementById(""next_button_slide"").className = class_name;")
                ToggleChangedScript.Append(vbCrLf & " }")
                ToggleChangedScript.Append(vbCrLf & " }")
                Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "Toggle", ToggleChangedScript.ToString, True)
            End If


            If Session.Item("isMobile") = True Then
                Dim reArrangeScript As String = ""
                reArrangeScript += "var companyContactAppend = jQuery('#" & aircraft_contacts_label.ClientID & "').detach();"
                reArrangeScript += "var slideshowAppend = jQuery('#" & aircraft_picture_slideshow.ClientID & "').detach();"
                reArrangeScript += "var ownershipAppend = jQuery('#" & ownership_update_panel.ClientID & "').detach();"
                reArrangeScript += "var analyticAppend = jQuery('#" & analytic_update_panel.ClientID & "').detach();"

                reArrangeScript += "analyticAppend.appendTo('#statusBeforeAppend');"
                reArrangeScript += "ownershipAppend.appendTo('#statusBeforeAppend');"
                reArrangeScript += "companyContactAppend.appendTo('#mobileAppend');"
                reArrangeScript += "slideshowAppend.appendTo('#mobileAppend');"
                mobileAdditionScript.Text = reArrangeScript.ToString
                mobileAdditionScript.Visible = True

            End If

            bShowBlankAcFields = commonEvo.getUserShowBlankACFields(Session.Item("ShowCondensedAcFormat"), bHasNoBlankAcFieldsCookie)

            'Sets Database information
            AclsData_Temp = New clsData_Manager_SQL
            flight_data_temp = New flightDataFunctions


            If Trim(Request("homebase")) = "Y" Then
                ValidatePermissions = True
                Dim useBackupSQL As Boolean = CBool(My.Settings.useBackupSQL_SRV.ToString)

                If Trim(Request("local")) = "Y" Then
                    If HttpContext.Current.Session.Item("jetnetWebHostType") = crmWebClient.eWebHostTypes.HOMEBASE Then
                        AclsData_Temp.JETNET_DB = My.Settings.LIVE_INHOUSE_MSSQL.ToString
                        Master.aclsData_Temp.JETNET_DB = My.Settings.LIVE_INHOUSE_MSSQL.ToString
                        Session.Item("jetnetClientDatabase") = My.Settings.LIVE_INHOUSE_MSSQL.ToString
                    End If
                ElseIf useBackupSQL Then
                    AclsData_Temp.JETNET_DB = "Data Source=www.jetnetsql2.com;Initial Catalog=jetnet_ra;Persist Security Info=False;User ID=homebase;Password=jd4kgcez"
                    Master.aclsData_Temp.JETNET_DB = "Data Source=www.jetnetsql2.com;Initial Catalog=jetnet_ra;Persist Security Info=False;User ID=homebase;Password=jd4kgcez"
                Else
                    AclsData_Temp.JETNET_DB = "Data Source=www.jetnetsql1.com;Initial Catalog=jetnet_ra;Persist Security Info=False;User ID=homebase;Password=jd4kgcez"
                    Master.aclsData_Temp.JETNET_DB = "Data Source=www.jetnetsql1.com;Initial Catalog=jetnet_ra;Persist Security Info=False;User ID=homebase;Password=jd4kgcez"
                End If

                If useBackupSQL Then
                    flight_data_temp.serverConnectStr = "Data Source=www.jetnetsql2.com;Initial Catalog=jetnet_ra;Persist Security Info=False;User ID=homebase;Password=jd4kgcez"
                    HttpContext.Current.Session.Item("jetnetClientDatabase") = "Data Source=www.jetnetsql2.com;Initial Catalog=jetnet_ra;Persist Security Info=False;User ID=homebase;Password=jd4kgcez"
                Else
                    flight_data_temp.serverConnectStr = "Data Source=www.jetnetsql1.com;Initial Catalog=jetnet_ra;Persist Security Info=False;User ID=homebase;Password=jd4kgcez"
                    HttpContext.Current.Session.Item("jetnetClientDatabase") = "Data Source=www.jetnetsql1.com;Initial Catalog=jetnet_ra;Persist Security Info=False;User ID=homebase;Password=jd4kgcez"
                End If

                ''Checking the FAA date.
                Dim FAATable As New DataTable
                FAATable = Master.aclsData_Temp.Get_FAA_Date()

                If Not IsNothing(FAATable) Then
                    If FAATable.Rows.Count > 0 Then
                        If Not IsDBNull(FAATable.Rows(0).Item("MaxDate")) Then
                            If Not String.IsNullOrEmpty(FAATable.Rows(0).Item("MaxDate").ToString.Trim) Then
                                HttpContext.Current.Session.Item("localSubscription").crmSubinst_FAA_data_date = FAATable.Rows(0).Item("MaxDate").ToString.Trim
                            End If
                        End If
                    End If

                    FAATable.Dispose()

                End If
            Else

                AclsData_Temp.JETNET_DB = Session.Item("jetnetClientDatabase")
                AclsData_Temp.client_DB = Session.Item("jetnetServerNotesDatabase")


                flight_data_temp.serverConnectStr = Session.Item("jetnetClientDatabase")
                flight_data_temp.clientConnectStr = Session.Item("jetnetServerNotesDatabase")

            End If


            'Sets Page Title

            Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("")

            'Checks whether or not this is the CRM Version of the page. 

            If clsGeneral.clsGeneral.isCrmDisplayMode() Then
                If HttpContext.Current.Session.Item("localUser").crmUser_Evo_MPM_Flag = True Then
                    Application.Item("crmClientDatabase") = HttpContext.Current.Session.Item("localPreferences").ServerNotesDatabaseConn
                End If

                CRMView = True
                ValidatePermissions = True

                'What we're going to do here is if the variable holding the IDs from the CRM is filled up,
                'We are changing the page to check to see whether you need a next/previous to the Details page.
                'This way - if they come from the details page, which the session variable set, the buttons should show up.
                If Not IsNothing(HttpContext.Current.Session("my_ids")) Then
                    If IsNothing(HttpContext.Current.Session("crmPagingParent")) Then
                        parent_check_page_name.Text = "DETAILS"
                    Else
                        If Not String.IsNullOrEmpty(HttpContext.Current.Session("crmPagingParent")) Then
                            parent_check_page_name.Text = HttpContext.Current.Session("crmPagingParent").ToString
                        Else
                            parent_check_page_name.Text = "DETAILS"
                        End If
                    End If
                End If


                'Toggle export menu off.
                'cssExportMenu.Attributes("class") = "display_none"
                ' turned it back on per request MSW 8/31/16

                'Toggles Notes off
                'Reminders.Visible = False
                'Notes.Visible = False
                'folders_container.Visible = False

                'view_analytics.Visible = False
                analyticContainer.Visible = False

                ' COMMENTED OUT MSW - 5/30/18
                '   foldersContainer.CssClass = "display_none"
                '   view_folders.Visible = False

                If Not IsNothing(Trim(HttpContext.Current.Request("source"))) Then
                    If Not String.IsNullOrEmpty(HttpContext.Current.Request("source")) Then
                        CRMSource = Trim(HttpContext.Current.Request("source"))
                    End If
                End If
                If Not IsNothing(Trim(HttpContext.Current.Request("tsource"))) Then
                    If Not String.IsNullOrEmpty(HttpContext.Current.Request("tsource")) Then
                        transactionSource = Trim(HttpContext.Current.Request("tsource"))
                    End If
                End If

            End If


            'Aircraft ID is filled here
            If Not IsNothing(Request.Item("acid")) Then
                If Not String.IsNullOrEmpty(Request.Item("acid").ToString) Then
                    aircraftID = CLng(Request.Item("acid").ToString.Trim)
                    newWindow.Text = "<a href=""DisplayEventsDetailListing.aspx?AircraftID=" + aircraftID.ToString + """ target=""_blank"" title=""Show Events In New Window""><strong>View All Events</strong></a>"
                End If
            End If
            'Fills Journal ID
            If Not IsNothing(Request.Item("jid")) Then
                If Not String.IsNullOrEmpty(Request.Item("jid").ToString) Then
                    journalID = CLng(Request.Item("jid").ToString.Trim)
                End If
            End If

            'Check for Event Sort -
            If Not IsNothing(Request.Item("sort")) Then
                If Not String.IsNullOrEmpty(Request.Item("sort").ToString) Then
                    sort = (Request.Item("sort").ToString.Trim)
                End If
            End If

            'Checking for sort order of events.
            If Not IsNothing(Request.Item("sortWay")) Then
                If Not String.IsNullOrEmpty(Request.Item("sortWay").ToString) Then
                    sortWay = (Request.Item("sortWay").ToString.Trim)
                End If
            End If

            'This is code in testing for the Notify Jetnet Widget on the Aircraft Screen.
            'Because it's in testing, it is only going to show initially on either LOCAL or TEST.
            'UPDATE: 2/25/16 - The protection on this was removed to now be available on live.
            ' If CRMView = False Then
            'If ((HttpContext.Current.Session.Item("jetnetWebSiteType") = crmWebClient.eWebSiteTypes.LOCAL) Or (HttpContext.Current.Session.Item("jetnetWebSiteType") = crmWebClient.eWebSiteTypes.TEST)) Then
            'End If

            If Trim(Request("homebase")) = "Y" Then
            Else
                Dim PermissionsAircraftTable As New DataTable
                PermissionsAircraftTable = commonEvo.GetAllAircraftInfo_dataTable(aircraftID, journalID, True)
                If Not IsNothing(PermissionsAircraftTable) Then
                    If PermissionsAircraftTable.Rows.Count > 0 Then
                        ValidatePermissions = True
                    End If
                End If
            End If
            'End If


            'If ((HttpContext.Current.Session.Item("jetnetWebSiteType") = crmWebClient.eWebSiteTypes.LOCAL) Or (HttpContext.Current.Session.Item("jetnetWebSiteType") = crmWebClient.eWebSiteTypes.TEST)) Then
            'If Session.Item("localUser").crmLocalUserEmailAddress.ToString.ToLower.Contains("@jetnet.com") Or Session.Item("localUser").crmLocalUserEmailAddress.ToString.ToLower.Contains("@mvintech.com") Then
            '  If Not Session.Item("localUser").crmLocalUserEmailAddress.ToString.ToLower.Contains("demo@jetnet.com") Then
            '    Dim AppraisalTable As New DataTable
            '    aircraft_appraisal_container.Visible = True
            '    aircraft_appraisal_tab.HeaderText = "<a href=" & DisplayFunctions.WriteAppraisalsLinks(aircraftID, 0, False, "", "") & " class='special'>APPRAISALS +</a>"
            '    appraisal_add_new.Text = "<p align='right'>+ " & DisplayFunctions.WriteAppraisalsLinks(aircraftID, 0, True, "", "Add New Appraisal") & "</p>"
            '    AppraisalTable = GetAircraftAppraisal(aircraftID, Session.Item("localUser").crmSubSubID)
            '    aircraft_appraisal_label.Text = DisplayFunctions.Display_Appraisals(AppraisalTable, AclsData_Temp, aircraftID)
            '  End If
            'End If
            'End If

            '----------------------------------------------------------------------------------------
            'NOTE: I want to put this in only on postback, but because it's being built dynamically,
            'and controls are being added with handlers, it faces the same problem as the aircraft listing page advanced search. 
            'Meaning that if not built on every init, they won't exist.
            'So this is a note to take a look at this whenever the aircraft listing page is 
            'worked through and use the same approach that was decided on there.
            '----------------------------------------------------------------------------------------
            'This Function Builds the Dynamic Table for Static Folders. This will allow them to add 
            'Aircraft to folders and this will only be built once. This is also built on page initialization because
            'It's adding dynamic controls to the page. These have to be put in at the very begining of the page lifecycle of the viewstate
            'will not be set.
            If Session.Item("isMobile") Then
                newWindow.Visible = False
                'aircrafthistory.Attributes.Remove("style")
                'aircraft_flight_activity.Height = Unit.Percentage(100D)
                'aircraft_flight_tab.CssClass = ""
                'events_container.Width = Unit.Percentage(100D)
                'events_container.Height = Unit.Percentage(100D)
                'events_tab.CssClass = ""
                mobileTellChanges.Visible = True
                mobileTellChanges.InnerHtml = "<a href=""javascript:void(0);"" onclick=""javascript:load('Notify.aspx?acID=" & aircraftID & "&jID=" & journalID & "','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');return false;"">Report Aircraft Changes</a>"
            Else
                Build_Dynamic_Folder_Table()
            End If
        End If
    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'variables
        Dim ResultsTable As New DataTable
        Dim DisplayNotes As Boolean = False
        Dim temp_link As String = ""
        Dim temp_est As Long = 0
        Dim asset_amod_id As Long = 0


        If (Session.Item("crmUserLogon") <> True And Trim(Request("homebase")) <> "Y") Or ValidatePermissions = False Then
            Response.Redirect("Default.aspx", False)
        ElseIf HttpContext.Current.Session.Item("jetnetWebHostType") <> crmWebClient.eWebHostTypes.HOMEBASE And Trim(Request("homebase")) = "Y" Then
            ' if we arent on homebase.com, but have passed homebasee, then bad
            Response.Redirect("Default.aspx", False)
        Else

            'Set Export Links:

            localDataLayer.clientConnectStr = Session.Item("localPreferences").UserDatabaseConn


            'Checks for Map Request String.
            If Not Page.IsPostBack Then
                If Not IsNothing(Request.Item("map")) Then
                    If Not String.IsNullOrEmpty(Request.Item("map").ToString) Then
                        RunMap = True
                        map_this_aircraft.CssClass = "blue_button float_left"
                        mapContainer.Visible = True
                    End If
                End If
            End If

            'Run Analytics automatically if hidden request is sent
            If Not Page.IsPostBack Then
                If Not IsNothing(Request.Item("analytics")) Then
                    If Not String.IsNullOrEmpty(Request.Item("analytics").ToString) Then

                        ' ADDED IN MSW - 3/5/2020
                        ' otherwise, dont let them cheat 
                        If (Session.Item("localUser").crmLocalUserEmailAddress.ToString.ToLower.Contains("@jetnet.com") And Not Session.Item("localUser").crmLocalUserEmailAddress.ToString.ToLower.Contains("demo@")) Or Session.Item("localUser").crmLocalUserEmailAddress.ToString.ToLower.Contains("@mvintech.com") Then
                            ViewAnalytics = True
                        ElseIf commonEvo.isDealerCompany(CLng(Session.Item("localUser").crmUserCompanyID.ToString), 0, aircraftID) = True Then
                            ' IN PASSING THE AC IN, IT TURNS IT INTO, IS IT CONNECTED TO THIS AC 
                            ViewAnalytics = True
                        End If

                    End If
                End If
            End If

            'Store Cookies
            clsGeneral.clsGeneral.Recent_Cookies("aircraft", aircraftID, IIf(CRMSource = "CLIENT", "CLIENT", "JETNET"))

            'Check if from Aircraft Listing
            If FromListing() Then
            End If

            If (Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO) Then
                'Toggle on Operator Link.
                view_operator_history.Visible = True
            End If



            If Trim(Request("evalues_update")) <> "" Then
                Call clsGeneral.clsGeneral.UpdateEvalues(Trim(Request("evalues_update")))
            End If

            If Not IsNothing(Session.Item("Aircraft_Master")) Then
                dsAircraftBrowse = CType(Session.Item("Aircraft_Master"), DataTable)
            End If


            If Not IsPostBack Then
                '        If Session.Item("SERVICE").ToString.ToUpper.Contains("JFW") Or Session.Item("SERVICE").ToString.ToUpper.Contains("AFW") Then

                '            'strACID = Session("ACID")
                '            'strJournId = Session("JOURNID")
                '            'strTechId = Session("TECHID")
                '            'strVersion = Session("VERSION")
                '            'strInsert = "INSERT INTO JFWAFW_Log (jfwafw_sub_id, jfwafw_ac_id, jfwafw_service, jfwafw_version, jfwafw_type, jfwafw_message) "
                '            'strInsert = strInsert & "VALUES (" & strTechId & ", " & strACID & ", '" & strService & "', '" & strVersion & "', 'JFWAFW ACDetails','Viewing Aircraft Details via JFW/AFW Application') "

                '            'adoRs = Application("objAdminConn").execute(strInsert, , adCmdText + adExecuteNoRecords)
                '            'adoRs = Nothing

                '            bFromJFWAFW = True

                '        End If ' Session.Item("SERVICE").ToString.ToUpper.Contains("JFW") Or Session.Item("SERVICE").ToString.ToUpper.Contains("AFW")

                '    Else

                '        'If String.IsNullOrEmpty(securityTokenLocal) Then
                '        '    If Not IsNothing(Session.Item("acDetailsDS")) And Me.currentRecord > 0 Then
                '        '        dsAircraftBrowse = Session.Item("acDetailsDS")
                '        '    Else
                '        '        If loadBrowseDataSet(dsAircraftBrowse) Then
                '        '            Session.Item("acDetailsDS") = dsAircraftBrowse
                '        '        End If
                '        '    End If
                '        'End If

                '    End If

                'Sets the History Background/Updates Browse Button for Non History Aircraft, Clears History Table.
                If journalID > 0 Then
                    recordsOf.Visible = False

                    view_aircraft_events.Visible = False

                    'cssExportMenu.Visible = False ' commented out msw 
                    history_background.CssClass = "history_bg"
                    outerDivAcDetailsID.Attributes.Add("class", "historyPage row valueSpec viewValueExport Simplistic aircraftSpec")
                    view_current_aircraft.Visible = True

                    view_current_aircraft.OnClientClick = "javascript:load('DisplayAircraftDetail.aspx?acid=" & aircraftID & "','','');return false;"
                Else
                    browseTable.Visible = True
                    history_background.CssClass = ""

                    'Right here is where we need to put a toggle in. There's a slightly different function used to display the next/previous CRM information.
                    If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CRM Then
                        If parent_check_page_name.Text = "VIEW_TEMPLATE" Then
                            Dim sortOrder As HttpCookie = Request.Cookies("viewSortOrder")
                            If Not IsNothing(sortOrder) Then
                                Dim arrayApart As Array = Split(sortOrder.Value, ",")
                                HttpContext.Current.Session("my_ids") = arrayApart
                            End If
                        End If
                        clsGeneral.clsGeneral.FindNextPreviousButtonsCRMACDetails(browseTableTitle, recordsOf, browse_label, currentRecLabel, totalRecLabel, aircraftID, PreviousACSwap, NextACSwap)
                    Else
                        UpdateBrowseButtons(dsAircraftBrowse)
                    End If


                End If

            End If


            'Add help button text here: 7/23/15
            ac_help_text.Text = clsGeneral.clsGeneral.CreateEvoHelpLink("Aircraft Details")
            'ac_help_text.Text = Replace(ac_help_text.Text, ">Help", "class=""gray_button float_left " & IIf(PreviousACSwap.Visible = False Or journalID > 0, "noBefore", "") & """><strong>Help</strong>")


            'Toggles Notes On/Off based on Flags
            If Not Session.Item("localUser").crmDemoUserFlag Then 'If this isn't a demo user.
                If Session.Item("localUser").crmEnableNotes Then 'If notes are enabled.
                    If journalID = 0 Then 'If this isn't a historical aircraft
                        If (Session.Item("localSubscription").crmServerSideNotes_Flag) Or (Session.Item("localSubscription").crmCloudNotes_Flag) Then 'If either Server Side Notes or Cloud Notes are on.
                            If (Not String.IsNullOrEmpty(Session.Item("jetnetServerNotesDatabase"))) Or (Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("localSubscription").crmCloudNotesDBName)) Then 'If either the server note db string, or the cloud note db name isn't empty
                                DisplayNotes = True
                            End If
                        End If
                    End If
                End If
            End If
            If Not Page.IsPostBack Then
                'generateDisplayAcDetails1()
                GenerateNewACDetails()
            End If

            If DisplayNotes = True And CRMView = False Then
                If Not Page.IsPostBack Then
                    Dim showViewAllLink As Boolean = False
                    Dim ShowCookieStatus As String = ""

                    'Reminders.Visible = True
                    'Notes.Visible = True
                    closeNotes.Visible = True
                    notesContainerItem.Visible = True
                    notesPanel.Visible = True
                    actionPanel.Visible = True
                    DisplayFunctions.DisplayLocalItems(AclsData_Temp, IIf(CRMSource = "CLIENT", jetnet_aircraft_id.Text, aircraftID), 0, 0, notes_label, action_label, False, True, False, True, 5, showViewAllLink, CRMView, "JETNET", Nothing, True)

                    notes_add_new.Text = "<p align='right'>" & DisplayFunctions.ViewAllNotesLink(0, aircraftID, IIf(CRMSource = "", "JETNET", CRMSource), "padding_left") & " + " & DisplayFunctions.WriteNotesRemindersLinks(0, IIf(CRMSource = "CLIENT", jetnet_aircraft_id.Text, aircraftID), 0, 0, True, "&n=1", "Add Note") & "</p>"
                    action_add_new.Text = "<p align='right'>+ " & DisplayFunctions.WriteNotesRemindersLinks(0, IIf(CRMSource = "CLIENT", jetnet_aircraft_id.Text, aircraftID), 0, 0, True, "", "Add Action") & "</p>"


                    If showViewAllLink = True Then
                        notes_view_all.Visible = True
                        notes_view_all.Text = "VIEW ALL"
                    End If

                    view_notes.Visible = True


                    GetNoteStatusCookies("NoteCookieStatus", ShowCookieStatus, HttpContext.Current.Session.Item("localUser").crmUserCompanyID.ToString & HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString & HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString)
                    SetUpTopMenuAddLinks()

                    'We're just going to toggle this off now.
                    If LCase(ShowCookieStatus) = "false" Then
                        Toggle_Tabs_Visibility(RunMap, False, False, False, False, False, False, False)
                    End If

                End If
            ElseIf CRMView = True Then 'HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CRM Then
                'Reminders.Visible = True
                'Notes.Visible = True
                notesContainerItem.Visible = True
                notesPanel.Visible = True
                actionPanel.Visible = True
                closeNotes.Visible = True
                view_notes.Visible = True
                Session.Item("localSubscription").crmServerSideNotes_Flag = True
                Session.Item("Listing") = 3
                Dim NotesLinkText As String = "javascript:load('edit_note.aspx?ac_ID=" & aircraftID & "&source=" & IIf(CRMSource <> "", CRMSource, "JETNET") & "&type=note&action=new&from=aircraftDetails','','scrollbars=yes,menubar=no,height=400,width=860,resizable=yes,toolbar=no,location=no,status=no');"
                Dim ActionsLinkText As String = "javascript:load('edit_note.aspx?ac_ID=" & aircraftID & "&source=" & IIf(CRMSource <> "", CRMSource, "JETNET") & "&type=action&action=new&from=aircraftDetails','','scrollbars=yes,menubar=no,height=400,width=860,resizable=yes,toolbar=no,location=no,status=no');"
                Dim ProspectsLinkText As String = "javascript:load('edit_note.aspx?ac_ID=" & aircraftID & "&source=" & IIf(CRMSource <> "", CRMSource, "JETNET") & "&type=prospect&action=new&from=aircraftDetails','','scrollbars=yes,menubar=no,height=400,width=860,resizable=yes,toolbar=no,location=no,status=no');"

                'notes_panel.HeaderText = "<a href=""javascript:void(0);"" onclick=""" & NotesLinkText & """ class='special'>NOTES +</a>"
                'action_panel.HeaderText = "<a href=""javascript:void(0);"" onclick=""" & ActionsLinkText & """ class='special'>ACTIONS +</a>"

                notes_add_new.Text = "<p align='right'>" & DisplayFunctions.ViewAllNotesLink(0, aircraftID, IIf(CRMSource = "", "JETNET", CRMSource), "padding_left") & " + <a href=""javascript:void(0);"" onclick=""" & NotesLinkText & """>Add Note</a></p>"
                action_add_new.Text = "<p align='right'>+ <a href=""javascript:void(0);"" onclick=""" & ActionsLinkText & """>Add Action</a></p>"
                SetUpTopMenuAddLinks()
                new_prospects_add.Text = "<p align='right'>+ <a href=""javascript:void(0);"" onclick=""" & ProspectsLinkText & """>Add Prospect</a></p>"
                prospects_update_panel.Visible = True

                DisplayFunctions.DisplayLocalItems(Master.aclsData_Temp, aircraftID, 0, 0, notes_label, action_label, False, True, False, True, 5, False, CRMView, CRMSource, prospects_label2, True)
                notes_update_panel.Update()
            Else
                view_notes.Visible = False
                notesPanel.Visible = False
                actionPanel.Visible = False
            End If



            If Not Page.IsPostBack Then
                'generateDisplayAcDetails1()
                'GenerateNewACDetails()
                ' generateDisplayAcDetails2() 

                'We need to add in 1 caveat:
                'If you're login has jetnet.com (or mvintech.com) - we want to see the analytics:
                'This needs to run after generateAcDetails1. That sets up the variable called DisplayAnalyticsButton. If the companyID of this
                'User matches the company ID of the broker, then we show the analytics button. This code down below overrides it (just for jetnet/mvintech)
                If (Session.Item("localUser").crmLocalUserEmailAddress.ToString.ToLower.Contains("@jetnet.com") And Not Session.Item("localUser").crmLocalUserEmailAddress.ToString.ToLower.Contains("demo@")) Or Session.Item("localUser").crmLocalUserEmailAddress.ToString.ToLower.Contains("@mvintech.com") Then
                    DisplayAnalyticsButton = True
                ElseIf commonEvo.isDealerCompany(CLng(Session.Item("localUser").crmUserCompanyID.ToString), 0, aircraftID) = True Then
                    ' IN PASSING THE AC IN, IT TURNS IT INTO, IS IT CONNECTED TO THIS AC 
                    DisplayAnalyticsButton = True
                End If

                Dim LoggableModel As String = aircraft_model.Text

                'Insert into Content Stat Table
                If CRMView = False Then
                    '  AclsData_Temp.Insert_Content_Stat(Now(), 0, aircraftID, AircraftModel, 0, 0, journalID, 0, Session.Item("localUser").crmSubSubID, Session.Item("localUser").crmUserLogin, Session.Item("localUser").crmSubSeqNo, Session.Item("localUser").crmUserContactID)
                    Call commonLogFunctions.Log_User_Event_Data("UserStatistics", "DisplayAircraftDetail: AC_ID = " & aircraftID, Nothing, 0, journalID, 0, 0, 0, aircraftID, AircraftModel)


                    'Setting up the value view button 
                    If HttpContext.Current.Session.Item("localPreferences").UserSPIViewFlag Then
                        'If ((HttpContext.Current.Session.Item("jetnetWebSiteType") = crmWebClient.eWebSiteTypes.LOCAL) Or (HttpContext.Current.Session.Item("jetnetWebSiteType") = crmWebClient.eWebSiteTypes.TEST)) Then
                        If journalID = 0 Then
                            Values_Drop.Visible = True
                        End If
                        'End If
                    End If
                Else
                    If Session.Item("isEVOLOGGING") = True Or HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO Then
                        'Logging here for CRM
                        If IsNumeric(jetnet_aircraft_id.Text) And IsNumeric(aircraft_model.Text) Then
                            If CLng(jetnet_aircraft_id.Text) > 0 Then


                                If CRMSource.ToUpper.Contains("CLIENT") Then
                                    LoggableModel = commonEvo.GetAircraftInfo(CLng(jetnet_aircraft_id.Text), True, False)
                                End If

                                If Not String.IsNullOrEmpty(LoggableModel.Trim) Then
                                    If Not IsNumeric(LoggableModel) Then
                                        LoggableModel = "0"
                                    End If
                                End If

                                Call commonLogFunctions.Log_User_Event_Data("UserStatistics", "DisplayAircraftDetail: AC_ID = " & jetnet_aircraft_id.Text & " " & IIf(CRMSource = "CLIENT", ". Viewing Client Record.", ""), Nothing, 0, journalID, 0, 0, 0, CLng(jetnet_aircraft_id.Text), CLng(LoggableModel))
                            End If
                        End If
                    End If
                End If

                ' weather or not crm view is true, we should show analytics if we should show it - MSW - 4/29/19
                If DisplayAnalyticsButton Then
                    ' view_analytics.Visible = True
                    'Me.li_start1.Visible = True
                    'Me.li_end1.Visible = True
                    'Me.analytics_link.Visible = True
                    viewAnalyticsToggle.Visible = True
                    'Me.analytics_link.Text = "<a href='FAAFlightData.aspx?acid=" & AircraftTable.Rows(0).Item("ac_id") & "&activetab=4' target='_blank'>Aircraft Utilization</a>"
                    Me.analytics_link.Text = "Analytics"
                Else
                    viewAnalyticsToggle.Visible = False
                    Me.analytics_link.Visible = False
                End If

                If CRMView = True Then
                    If journalID = 0 Then
                        viewProspectorToggle.Visible = True
                        ' viewProspectToggle.Visible = True
                        If jetnet_aircraft_id.Text > 0 Then
                            Dim tempModel As String = commonEvo.GetAircraftInfo(CLng(jetnet_aircraft_id.Text), True, False)
                            market_report_link.Text = "<a href=""/viewtopdf.aspx?viewID=998&ac_id=" & jetnet_aircraft_id.Text & "&journ_id=" & journalID & "&amod_id=" & tempModel & """ target=""_blank"">Market Report</a>"
                            prospectorLink.Text = "<a href=""/view_template.aspx?ViewID=18&ac_id=" & jetnet_aircraft_id.Text & "&amod_id=" & tempModel & "&noMaster=false"" target=""_blank"">Prospector</a>"
                        Else
                            market_report_link.Text = "<a href=""/viewtopdf.aspx?viewID=998&ac_id=" & jetnet_aircraft_id.Text & "&journ_id=" & journalID & "&amod_id=" & AircraftModel & """  target=""_blank"">Market Report</a>"
                            prospectorLink.Text = "<li><a href=""/view_template.aspx?ViewID=18&ac_id=" & jetnet_aircraft_id.Text & "&noMaster=false"" target=""_blank"">Prospector</a></li>"
                        End If
                    End If
                Else
                    market_report_link.Text = "<a href=""/viewtopdf.aspx?viewID=998&ac_id=" & aircraftID & "&journ_id=" & journalID & "&amod_id=" & AircraftModel & """ target=""_blank"">Market Report</a>"
                    If journalID = 0 Then
                        If Session.Item("localSubscription").crmAerodexFlag = False Then
                            viewProspectorToggle.Visible = True
                            prospectorLink.Text = "<a href=""/view_template.aspx?ViewID=18&ac_id=" & aircraftID & "&amod_id=" & AircraftModel & "&noMaster=false"" target=""_blank"">Prospector</a>"
                        End If
                    End If
                End If

                'Dispose Table
                ResultsTable.Dispose()

                'If analytics request variable has been passed, display info automatically
                If ViewAnalytics = True Then
                    DisplayAnalyticInformation()

                    If Not IsPostBack Then
                        If InStr(analytics_link.CssClass, "float_left 2") > 0 Then
                            Toggle_Tabs_Visibility(False, False, False, False, False, False, False, False)
                            Me.analytics_link.CssClass = "float_left 1 subMenuText"
                        Else
                            Me.analytics_link.CssClass = "float_left 2 subMenuText"
                            Toggle_Tabs_Visibility(False, True, False, False, False, False, False, False)
                            System.Web.UI.ScriptManager.RegisterStartupScript(Me.analytic_update_panel, Me.GetType(), "ToggleAnalytics", "drawVisualization();", True)

                            If IsNumeric(DOM.Text) Then
                                If DOM.Text > 0 Then
                                    System.Web.UI.ScriptManager.RegisterStartupScript(Me.analytic_update_panel, Me.GetType(), "ToggleAnalyticsBar", "drawBarVisualization();", True)
                                End If
                            End If
                            System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ToggleVis", "ToggleButtons('display_block');", True)

                        End If
                        analytic_update_panel.Update()
                    End If
                End If

            End If


            If clsGeneral.clsGeneral.isEValuesAvailable() = True And clsGeneral.clsGeneral.isShowingEvalues() = True Then
                'If ((HttpContext.Current.Session.Item("jetnetWebSiteType") = crmWebClient.eWebSiteTypes.LOCAL) Or (HttpContext.Current.Session.Item("jetnetWebSiteType") = crmWebClient.eWebSiteTypes.TEST)) Then
                'If HttpContext.Current.Session.Item("localPreferences").UserSPIViewFlag Then
                Dim temp_make_model As String = ""

                Dim found_eval As Boolean = False

                If AircraftModel_JETNET > 0 Then
                    Me.amod_id.Text = AircraftModel_JETNET
                Else
                    'this is a fix to put in for the re-post for the links to the valuation page 
                    If Trim(Me.amod_id.Text) <> "0" And Trim(Me.amod_id.Text) <> "" Then
                        AircraftModel_JETNET = Me.amod_id.Text
                    End If
                End If

                temp_make_model = Get_Model_Name(AircraftModel_JETNET)



                'If temp_jetnet_ac_id = 0 Then ' then we are on a jetnet record 
                '  temp_ac_mfr_year = Get_AC_MFR_YEAR(aircraftID)
                'Else ' we are on a client record
                '  temp_ac_mfr_year = Get_AC_MFR_YEAR(temp_jetnet_ac_id)  ' must use thise 
                'End If

                If temp_jetnet_ac_id = 0 Then ' then we are on a jetnet record 
                    temp_ac_dlv_year = Get_AC_DLV_YEAR(aircraftID)
                Else ' we are on a client record
                    temp_ac_dlv_year = Get_AC_DLV_YEAR(temp_jetnet_ac_id)  ' must use thise  
                End If


                AC_Estimates.Visible = False   ' removed to be put into page later 
                AC_Model_Estimates.Visible = True
                AC_Model_Time.Visible = True
                AC_Current_Market.Visible = True
                AC_Model_Residual_By_MFR.Visible = True
                AC_Model_AFTT.Visible = True
                View_Values.Visible = True
                ViewValuesViewLink.Visible = True
                eValues_Toggle.Visible = True

                AC_Model_Residual.Visible = False

                '  If (Session.Item("localUser").crmLocalUserEmailAddress.ToString.ToLower.Contains("@jetnet.com") And Not Session.Item("localUser").crmLocalUserEmailAddress.ToString.ToLower.Contains("demo@")) Or Session.Item("localUser").crmLocalUserEmailAddress.ToString.ToLower.Contains("@mvintech.com") Then
                AC_Assett_Summary.Visible = True
                Me.eValues_Update_Estimate.Visible = True
                If journalID = 0 Then
                    'eValues_update_estimate_button.Text = "<a href=""#"" onclick=""javascript:load('DisplayAircraftDetail.aspx?acid=" & aircraftID & "','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');return false;"">Update eValues Estimate</a>"
                    eValues_update_estimate_button.Text = "<a href='/DisplayAircraftDetail.aspx?acid=" & aircraftID & "&purpose=estimator'>Update eValues Estimate</a>"
                End If
                'End If

                Dim utilization_functions1 As New utilization_view_functions
                utilization_functions1.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
                utilization_functions1.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
                utilization_functions1.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
                utilization_functions1.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
                utilization_functions1.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim

                Dim current_month_table As New DataTable
                Dim searchCriteria As New viewSelectionCriteriaClass
                Dim comp_functions As New CompanyFunctions
                ' changed this to now pass AircraftModel_JETNET, from AircraftModel, so that it always passes jetnet amod id 
                'then the client one should be the aircraftID
                If temp_jetnet_ac_id > 0 And aircraftID > 0 Then
                    ViewACEstimatesLink.Text = "<a href=""/largeGraphDisplay.aspx?ac_id=" & temp_jetnet_ac_id & "&Client_AC_ID=" & aircraftID & "&source=CLIENT&ac_dlv_year=" & temp_ac_dlv_year & "&graph_type=ACESTIMATES&page_title=VALUE HISTORY/PROJECTIONS"" target=""_blank"">Aircraft Estimates</a>"
                    ViewACModelYearLink.Text = "<a href=""/largeGraphDisplay.aspx?amod_id=" & AircraftModel_JETNET & "&ac_id=" & temp_jetnet_ac_id & "&source=CLIENT&Client_AC_ID=" & aircraftID & "&ac_dlv_year=" & temp_ac_dlv_year & "&graph_type=DLVYEAR&page_title=VALUES BY DLV YEAR"" target=""_blank"">" & temp_make_model & " Estimates By Year</a>"
                    ViewACModelTimeLink.Text = "<a href=""/largeGraphDisplay.aspx?amod_id=" & AircraftModel_JETNET & "&ac_id=" & temp_jetnet_ac_id & "&source=CLIENT&Client_AC_ID=" & aircraftID & "&graph_type=ASKSOLD&ac_dlv_year=" & temp_ac_dlv_year & "&page_title=VALUES BY MONTH"" target=""_blank"">" & temp_make_model & " Estimates By Time</a>"
                    ViewACCurrentMarket.Text = "<a href=""/largeGraphDisplay.aspx?amod_id=" & AircraftModel_JETNET & "&ac_id=" & temp_jetnet_ac_id & "&source=CLIENT&Client_AC_ID=" & aircraftID & "&graph_type=CURRENTMARKET&ac_dlv_year=" & temp_ac_dlv_year & "&page_title=CURRENT MARKET"" target=""_blank"">" & temp_make_model & " Current Market</a>"
                    ViewACResidualByMFR.Text = "<a href=""/largeGraphDisplay.aspx?amod_id=" & AircraftModel_JETNET & "&ac_dlv_year=" & temp_ac_dlv_year & "&graph_type=RESIDUAL&page_title=RESIDUAL VALUES BY DLV YEAR"" target=""_blank"">" & temp_make_model & " Residual Estimates By DLV Year</a>"
                    ViewACResidual.Text = "<a href=""/largeGraphDisplay.aspx?amod_id=" & AircraftModel_JETNET & "&ac_dlv_year=" & temp_ac_dlv_year & "&ac_id=" & temp_jetnet_ac_id & "&source=CLIENT&Client_AC_ID=" & aircraftID & "&graph_type=RESIDUALAC&page_title=RESIDUAL VALUES"" target=""_blank"">Residual Estimates for MY AC</a>"
                    ViewACModelAFTT.Text = "<a href=""/largeGraphDisplay.aspx?amod_id=" & AircraftModel_JETNET & "&ac_id=" & temp_jetnet_ac_id & "&Client_AC_ID=" & aircraftID & "&ac_dlv_year=" & temp_ac_dlv_year & "&graph_type=AFTT&page_title=VALUES BY AFTT"" target=""_blank"">" & temp_make_model & " Estimates By AFTT</a>"
                    '  If (Session.Item("localUser").crmLocalUserEmailAddress.ToString.ToLower.Contains("@jetnet.com") And Not Session.Item("localUser").crmLocalUserEmailAddress.ToString.ToLower.Contains("demo@")) Or Session.Item("localUser").crmLocalUserEmailAddress.ToString.ToLower.Contains("@mvintech.com") Then
                    ViewACAssett.Text = "<a href=""/AssetInsight.aspx?acid=" & temp_jetnet_ac_id & "&customer=Y"" target=""_blank"">EVALUE SUMMARY</a>"
                    ' Else
                    '   ViewACAssett.Visible = False
                    ' End If
                    searchCriteria.ViewCriteriaAircraftID = temp_jetnet_ac_id
                Else
                    ViewACEstimatesLink.Text = "<a href=""/largeGraphDisplay.aspx?ac_id=" & aircraftID & "&Client_AC_ID=0&source=JETNET&ac_dlv_year=" & temp_ac_dlv_year & "&graph_type=ACESTIMATES&page_title=VALUE HISTORY/PROJECTIONS"" target=""_blank"">Aircraft Estimates</a>"
                    ViewACModelYearLink.Text = "<a href=""/largeGraphDisplay.aspx?amod_id=" & AircraftModel_JETNET & "&ac_id=" & aircraftID & "&Client_AC_ID=0&source=JETNET&ac_dlv_year=" & temp_ac_dlv_year & "&graph_type=DLVYEAR&page_title=VALUES BY DLV YEAR"" target=""_blank"">" & temp_make_model & " Estimates By Year</a>"
                    ViewACModelTimeLink.Text = "<a href=""/largeGraphDisplay.aspx?amod_id=" & AircraftModel_JETNET & "&ac_id=" & aircraftID & "&Client_AC_ID=0&source=JETNET&graph_type=ASKSOLD&ac_dlv_year=" & temp_ac_dlv_year & "&page_title=VALUES BY MONTH"" target=""_blank"">" & temp_make_model & " Estimates By Time</a>"
                    ViewACCurrentMarket.Text = "<a href=""/largeGraphDisplay.aspx?amod_id=" & AircraftModel_JETNET & "&ac_id=" & aircraftID & "&Client_AC_ID=0&source=JETNET&graph_type=CURRENTMARKET&ac_dlv_year=" & temp_ac_dlv_year & "&page_title=CURRENT MARKET"" target=""_blank"">" & temp_make_model & " Current Market</a>"
                    ViewACResidualByMFR.Text = "<a href=""/largeGraphDisplay.aspx?amod_id=" & AircraftModel_JETNET & "&ac_dlv_year=" & temp_ac_dlv_year & "&graph_type=RESIDUAL&page_title=RESIDUAL VALUES BY DLV YEAR"" target=""_blank"">" & temp_make_model & " Residual Estimates By DLV Year</a>"
                    ViewACResidual.Text = "<a href=""/largeGraphDisplay.aspx?amod_id=" & AircraftModel_JETNET & "&ac_dlv_year=" & temp_ac_dlv_year & "&ac_id=" & aircraftID & "&Client_AC_ID=0&graph_type=RESIDUALAC&page_title=RESIDUAL VALUES"" target=""_blank"">Residual Estimates for MY AC</a>"
                    ViewACModelAFTT.Text = "<a href=""/largeGraphDisplay.aspx?amod_id=" & AircraftModel_JETNET & "&ac_id=" & aircraftID & "&Client_AC_ID=0&ac_dlv_year=" & temp_ac_dlv_year & "&graph_type=AFTT&page_title=VALUES BY AFTT"" target=""_blank"">" & temp_make_model & " Estimates By AFTT</a>"
                    '   If (Session.Item("localUser").crmLocalUserEmailAddress.ToString.ToLower.Contains("@jetnet.com") And Not Session.Item("localUser").crmLocalUserEmailAddress.ToString.ToLower.Contains("demo@")) Or Session.Item("localUser").crmLocalUserEmailAddress.ToString.ToLower.Contains("@mvintech.com") Then
                    ViewACAssett.Text = "<a href=""/AssetInsight.aspx?acid=" & aircraftID & "&customer=Y"" target=""_blank"">EVALUE SUMMARY</a>"
                    '   Else
                    '   ViewACAssett.Visible = False
                    ' End If
                    ViewValuesViewLink.Text = "<a href=""/view_template.aspx?ViewID=27&ViewName=Value&amod_id=" & AircraftModel_JETNET & "&acid=" & aircraftID & """ target=""_blank"">Valuation</a>"
                    eValues_Toggle_Button.Text = "<a href=""/view_template.aspx?ViewID=27&ViewName=Value&amod_id=" & AircraftModel_JETNET & "&acid=" & aircraftID & """ target=""_blank"">Valuation</a>"
                    searchCriteria.ViewCriteriaAircraftID = aircraftID
                End If


                temp_link = HttpContext.Current.Request.Url.AbsoluteUri.ToString
                ' then replace whatever it was 
                If InStr(temp_link, "evalues_update") > 0 Then
                    temp_link = Replace(temp_link, "&evalues_update=N", "")
                    temp_link = Replace(temp_link, "&evalues_update=Y", "")
                End If

                If clsGeneral.clsGeneral.isShowingEvalues() = True Then
                    eValues_Toggle_Button.Text = "<a href='" & temp_link & "&evalues_update=N'>Toggle eValues Display Off</a>"
                Else
                    eValues_Toggle_Button.Text = "<a href='" & temp_link & "&evalues_update=Y'>Toggle eValues Display On</a>"
                End If




                If journalID = 0 Then

                    Call utilization_functions1.views_display_evalues_in_status_block(searchCriteria, journalID, Me.estimator_submit, Me.status_label.Text, found_eval, temp_ac_dlv_year, temp_make_model, AircraftModel_JETNET, temp_jetnet_ac_id, aircraftID)

                    If found_eval = True Then
                        AC_Model_Residual.Visible = True
                    Else
                        AC_Model_Residual.Visible = False
                    End If

                End If

                'End If
            ElseIf clsGeneral.clsGeneral.isEValuesAvailable() = True Then

                View_Values.Visible = True
                ViewValuesViewLink.Visible = True
                If temp_jetnet_ac_id > 0 And aircraftID > 0 Then
                Else
                    ViewValuesViewLink.Text = "<a href=""#"" onclick=""javascript:load('view_template.aspx?ViewID=27&ViewName=Value&amod_id=" & AircraftModel_JETNET & "&acid=" & aircraftID & "','','');return false;"">Valuation</a>"
                End If

                Dim temp_make_model As String = ""

                Dim found_eval As Boolean = False
                temp_make_model = Get_Model_Name(AircraftModel_JETNET)

                'If temp_jetnet_ac_id = 0 Then ' then we are on a jetnet record 
                '  temp_ac_mfr_year = Get_AC_MFR_YEAR(aircraftID)
                'Else ' we are on a client record
                '  temp_ac_mfr_year = Get_AC_MFR_YEAR(temp_jetnet_ac_id)  ' must use thise 
                'End If

                If temp_jetnet_ac_id = 0 Then ' then we are on a jetnet record 
                    temp_ac_dlv_year = Get_AC_DLV_YEAR(aircraftID)
                Else ' we are on a client record
                    temp_ac_dlv_year = Get_AC_DLV_YEAR(temp_jetnet_ac_id)  ' must use thise 
                End If

                eValues_Toggle.Visible = True
                temp_link = HttpContext.Current.Request.Url.AbsoluteUri.ToString
                ' then replace whatever it was 
                If InStr(temp_link, "evalues_update") > 0 Then
                    temp_link = Replace(temp_link, "&evalues_update=N", "")
                    temp_link = Replace(temp_link, "&evalues_update=Y", "")
                End If

                If clsGeneral.clsGeneral.isShowingEvalues() = True Then
                    eValues_Toggle_Button.Text = "<a href='" & temp_link & "&evalues_update=N'>Toggle eValues Display Off</a>"
                Else
                    eValues_Toggle_Button.Text = "<a href='" & temp_link & "&evalues_update=Y'>Toggle eValues Display On</a>"
                End If
            ElseIf HttpContext.Current.Session.Item("localPreferences").UserSPIViewFlag = True Then
                ' still need this link in here 
                View_Values.Visible = True
                ViewValuesViewLink.Visible = True
                If temp_jetnet_ac_id > 0 And aircraftID > 0 Then
                Else
                    ViewValuesViewLink.Text = "<a href=""#"" onclick=""javascript:load('view_template.aspx?ViewID=27&ViewName=Value&amod_id=" & AircraftModel_JETNET & "&acid=" & aircraftID & "','','');return false;"">Valuation</a>"
                End If

            End If

            ' if we are running from homebase and have homebase = Y then
            If HttpContext.Current.Session.Item("jetnetWebHostType") = crmWebClient.eWebHostTypes.HOMEBASE And Trim(Request("homebase")) = "Y" Then
                If Trim(Request("source")) = "CLIENT" Then
                    single_spec_link.Text = "<a href=""PDF_Creator.aspx?area=&r_id=39&page=single_page_spec&ac_id=" & temp_jetnet_ac_id & "&journ_id=" & journalID & "&amod_id=" & AircraftModel & "&homebase=Y"" target=""_blank"">Single Spec</a>"
                    full_spec_link.Text = "<a href=""PDF_Creator.aspx?area=&r_id=53&page=Spec_Sheet&ac_id=" & aircraftID & "&source=CLIENT&otherID=" & temp_jetnet_ac_id.ToString & "&journ_id=" & journalID & "&amod_id=" & AircraftModel & "&homebase=Y"" target=""_blank"">Full Spec</a>"
                    condensed_spec_link.Text = "<a href=""PDF_Creator.aspx?area=&r_id=40&page=Short_Spec_Sheet&ac_id=" & temp_jetnet_ac_id & "&journ_id=" & journalID & "&amod_id=" & AircraftModel & "&homebase=Y"" target=""_blank"">Condensed Spec</a>"
                Else
                    single_spec_link.Text = "<a href=""PDF_Creator.aspx?area=&r_id=39&page=single_page_spec&ac_id=" & aircraftID & "&journ_id=" & journalID & "&amod_id=" & AircraftModel & "&homebase=Y"" target=""_blank"">Single Spec</a>"
                    full_spec_link.Text = "<a href=""PDF_Creator.aspx?area=&r_id=53&page=Spec_Sheet&ac_id=" & aircraftID & "&journ_id=" & journalID & "&amod_id=" & AircraftModel & "&homebase=Y"" target=""_blank"">Full Spec</a>"
                    condensed_spec_link.Text = "<a href=""PDF_Creator.aspx?area=&r_id=40&page=Short_Spec_Sheet&ac_id=" & aircraftID & "&journ_id=" & journalID & "&amod_id=" & AircraftModel & "&homebase=Y"" target=""_blank"">Condensed Spec</a>"
                End If
            Else
                If Trim(Request("source")) = "CLIENT" Then
                    single_spec_link.Text = "<a href=""PDF_Creator.aspx?area=&r_id=39&page=single_page_spec&ac_id=" & temp_jetnet_ac_id & "&journ_id=" & journalID & "&amod_id=" & AircraftModel & """ target=""_blank"">Single Spec</a>"
                    full_spec_link.Text = "<a href=""PDF_Creator.aspx?area=&r_id=53&page=Spec_Sheet&ac_id=" & aircraftID & "&source=CLIENT&otherID=" & temp_jetnet_ac_id.ToString & "&journ_id=" & journalID & "&amod_id=" & AircraftModel & """ target=""_blank"">Full Spec</a>"
                    condensed_spec_link.Text = "<a href=""PDF_Creator.aspx?area=&r_id=40&page=Short_Spec_Sheet&ac_id=" & temp_jetnet_ac_id & "&journ_id=" & journalID & "&amod_id=" & AircraftModel & """ target=""_blank"">Condensed Spec</a>"
                Else
                    single_spec_link.Text = "<a href=""PDF_Creator.aspx?area=&r_id=39&page=single_page_spec&ac_id=" & aircraftID & "&journ_id=" & journalID & "&amod_id=" & AircraftModel & """ target=""_blank"">Single Spec</a>"
                    full_spec_link.Text = "<a href=""PDF_Creator.aspx?area=&r_id=53&page=Spec_Sheet&ac_id=" & aircraftID & "&journ_id=" & journalID & "&amod_id=" & AircraftModel & """ target=""_blank"">Full Spec</a>"
                    condensed_spec_link.Text = "<a href=""PDF_Creator.aspx?area=&r_id=40&page=Short_Spec_Sheet&ac_id=" & aircraftID & "&journ_id=" & journalID & "&amod_id=" & AircraftModel & """ target=""_blank"">Condensed Spec</a>"
                End If
            End If



            'Set Page Title:
            Master.SetPageTitle(aircraftPageTitle.Text)
            'Sets Focus on Information Label 
            'aircraft_information_label.Focus()


            'Added 10/14/19 to only run on homebase
            If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Then
                If Trim(HttpContext.Current.Session.Item("jetnetServerNotesDatabase")) = "" Then
                    HttpContext.Current.Session.Item("jetnetServerNotesDatabase") = HttpContext.Current.Session.Item("jetnetClientDatabase")
                End If
            End If

            ' If localDataLayer.Get_Subscription_Team(Session.Item("localUser").crmSubSubID, "Market Insight") = True Then
            'market_report_link.Text = "<a href='#' onclick=""javascript:load('viewtopdf.aspx?viewID=998&ac_id=" & aircraftID & "&journ_id=" & journalID & "&amod_id=" & AircraftModel & "','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');return false;"">Market Report</a>"
            'End If

            If Session.Item("isMobile") = False And journalID = 0 Then
                RunTellJetnetAboutChangesCode()
            End If

            If Session.Item("isMobile") = True Then
                '1.	Remove export menu completely as well as subitems
                cssExportMenu.Visible = False
                '2.	Remove folders option completely
                view_folders.Visible = False
                '3.	Remove help option completely
                ac_help_text.Visible = False

                view_aircraft_events.OnClientClick = "document.location='/DisplayAircraftDetail.aspx?acid=" & aircraftID.ToString & "#eventsView';"
                view_notes.OnClientClick = "document.location='/DisplayAircraftDetail.aspx?acid=" & aircraftID.ToString & "#notes';"
                map_this_aircraft.OnClientClick = "document.location='/DisplayAircraftDetail.aspx?acid=" & aircraftID.ToString & "#map';"
                ownership_link.OnClientClick = "document.location='/DisplayAircraftDetail.aspx?acid=" & aircraftID.ToString & "#ownership';"
                analytics_link.OnClientClick = "document.location='/DisplayAircraftDetail.aspx?acid=" & aircraftID.ToString & "#analytics';"

            End If
        End If



        If clsGeneral.clsGeneral.isEValuesAvailable() = True Then
            If Trim(Request.Item("purpose")) = "estimator" Then
                ProspectUpdate.Visible = False
                events_update_panel.Visible = False
                ownership_update_panel.Visible = False
                analytic_update_panel.Visible = False
                map_update_panel.Visible = False
                aircraft_picture_slideshow.Visible = False
                aircraft_contacts_label.Visible = False
                valuesUpdatePanel.Visible = False
                lease_tab_label.Visible = False
                aircraft_appraisal_container.Visible = False
                prospects_update_panel.Visible = False
                history_label.Visible = False
                flightContainer.Visible = False

                If Not IsPostBack Then
                    Me.estimator_aftt.Text = est_aftt
                    Me.estimator_landings.Text = est_landings
                    Me.estimator_as_of_date.Text = est_as_of_date

                    Dim utilization_functions2 As New utilization_view_functions
                    utilization_functions2.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
                    utilization_functions2.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
                    utilization_functions2.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
                    utilization_functions2.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
                    utilization_functions2.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim


                    ' if the value is based off of the average usage, then we should explain
                    If temp_jetnet_ac_id > 0 And aircraftID > 0 Then
                        temp_est = utilization_functions2.Get_Hours_Based_Usage(temp_jetnet_ac_id, est_as_of_date2, est_landings2)   ' so its jetnet not client
                    Else
                        temp_est = utilization_functions2.Get_Hours_Based_Usage(aircraftID, est_as_of_date2, est_landings2)
                    End If

                    ' then we used usage, so we need to put those values in the text boxes
                    If temp_est > 0 Then

                        If IsNumeric(temp_est) Then
                            Me.estimator_aftt.Text = FormatNumber(temp_est, 0)
                        Else
                            Me.estimator_aftt.Text = ""
                        End If


                        If IsNumeric(est_landings2) Then
                            Me.estimator_landings.Text = FormatNumber(est_landings2, 0)
                        Else
                            Me.estimator_landings.Text = ""
                        End If


                        Me.estimator_as_of_date.Text = ""     ' as of date is empty if we are using an estimated/evalue date 
                        'Me.estimator_as_of_date.Text = FormatDateTime(est_as_of_date2, DateFormat.ShortDate)
                    End If

                    If Me.estimator_verify.Checked = True Then

                    Else

                    End If

                    edit_eValues.Visible = True
                    Me.estimator_label1.Text = "Use the block below to submit updated data to JETNET resulting in a new eValue estimate for this aircraft. Note that updates must include the date data was valid as of."
                    Call Get_Airframe_Program_Names(AircraftModel_JETNET, Me.estimator_airframe_program)
                    Me.estimator_airframe_program.Text = est_afmp

                    asset_amod_id = Get_Asset_Model_Id(AircraftModel_JETNET)
                    Me.assett_click_label.Text = "Should you desire to perform a more detailed asset analysis on this aircraft thru Asset Insight, click "
                    Me.assett_click_label.Text += "<a href='https://www.assetinsight.com/referral?identifier=3af292c368e5b0c9b5e6d93752f657d1&model=" & asset_amod_id & "&serial =" & ac_ser_nbr & "&user-email=" & HttpContext.Current.Session.Item("localUser").crmLocalUserEmailAddress.ToString & "'>Here</a>"

                End If

            End If
        End If




        If HttpContext.Current.Session.Item("localPreferences").UserSPIViewFlag = True Then
            Me.Values_Drop.Visible = True
            Me.VALUES_UL.Visible = True
        Else
            Me.Values_Drop.Visible = False
            Me.VALUES_UL.Visible = False
        End If


    End Sub

    Private Sub SetUpTopMenuAddLinks()
        AddMenuItem.Visible = True
        Add_Note_Top.Visible = True
        Dim LinkDisplay() As String = Split(Replace(Replace(notes_add_new.Text, " +", ""), " New", ""), "View All</a> ")
        Add_Note_Top.InnerHtml = LinkDisplay(1)
        Add_Action_Top.Visible = True
        Add_Action_Top.InnerHtml = Replace(Replace(action_add_new.Text, "+ ", ""), " New", "")


        If CRMView = True Then
            Add_Prospect_Top.Visible = True
            Add_Prospect_Top.InnerHtml = Replace(Replace(new_prospects_add.Text, "+ ", ""), " New", "")

            If OtherID > 0 Then
                If CRMSource <> "CLIENT" Then
                    viewOther.Visible = True
                    viewOther.Text = "<li><a href='javascript:void();' " & DisplayFunctions.WriteDetailsLink(0, OtherID, 0, 0, False, "", "", "&source=CLIENT") & ">VIEW CLIENT</a></li><hr class=""remove_margin"" />"

                Else
                    edit_company_link.InnerHtml = CommonAircraftFunctions.CreateEditLink("", CRMSource, aircraftID, "", True, False, "", True)
                    edit_company_link.Visible = True
                    viewOther.Visible = True
                    viewOther.Text = "<li><a href='javascript:void();' " & DisplayFunctions.WriteDetailsLink(0, OtherID, 0, 0, False, "", "", "") & ">VIEW JETNET</a></li><hr class=""remove_margin"" />"
                End If
            ElseIf CRMSource <> "CLIENT" Then
                edit_company_link.InnerHtml = CommonAircraftFunctions.CreateEditLink("", CRMSource, aircraftID, "", False, True, "", True)
                edit_company_link.Visible = True
            End If
        End If



    End Sub

    Public Function Get_Model_Name(ByVal amod_id As Integer) As String
        Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
        Dim SqlCommand As New System.Data.SqlClient.SqlCommand
        Dim SqlConn As New System.Data.SqlClient.SqlConnection
        Dim SqlDataReader As System.Data.SqlClient.SqlDataReader : SqlDataReader = Nothing

        Dim tmpStr As String : tmpStr = ""

        Dim Query As String : Query = ""
        Query = "SELECT DISTINCT amod_make_name, amod_model_name FROM Aircraft_Model WITH(NOLOCK) WHERE amod_id = " + amod_id.ToString

        '    Query &= " " + commonEvo.GenerateProductCodeSelectionQuery(Session.Item("localSubscription"), False, True)
        Query = Query & clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(Session.Item("localSubscription"), False, False)

        Try

            'Select Case Application.Item("webHostObject").evoWebHostType
            'Case eWebSiteTypes.LOCAL
            'SqlConn.ConnectionString = My.Settings.DEFAULT_LOCAL_MSSQL
            '  Case Else
            SqlConn.ConnectionString = Session.Item("localPreferences").UserDatabaseConn
            'End Select

            SqlConn.Open()

            SqlCommand.Connection = SqlConn
            SqlCommand.CommandTimeout = 1000
            SqlCommand.CommandText = Query.ToString

            SqlDataReader = SqlCommand.ExecuteReader()

            If SqlDataReader.HasRows Then
                SqlDataReader.Read()
                If Not (IsDBNull(SqlDataReader("amod_make_name")) And Not IsDBNull(SqlDataReader("amod_model_name"))) Then
                    tmpStr = SqlDataReader("amod_make_name").ToString & " " & SqlDataReader("amod_model_name").ToString
                End If
            End If
            SqlDataReader.Close()
            SqlDataReader = Nothing
        Catch SqlException

            SqlConn.Dispose()
            SqlCommand.Dispose()

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in Get_Model_Name: " & SqlException.Message

        Finally

            SqlCommand.Dispose()
            SqlConn.Close()
            SqlConn.Dispose()

        End Try

        Return tmpStr

    End Function

    Public Function Get_Airframe_Program_Names(ByVal amod_id As Integer, ByRef dropdown_box As DropDownList) As String
        Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
        Dim SqlCommand As New System.Data.SqlClient.SqlCommand
        Dim SqlConn As New System.Data.SqlClient.SqlConnection
        Dim SqlDataReader As System.Data.SqlClient.SqlDataReader : SqlDataReader = Nothing

        Dim tmpStr As String : tmpStr = ""

        Dim Query As String : Query = ""

        Query = "select distinct amp_program_name "
        Query &= " from Aircraft_Flat with (NOLOCK)"
        Query &= " where ac_journ_id = 0  "
        Query &= " and amod_id in (" & amod_id & ")"
        Query &= " order by amp_program_name asc "

        Try

            'Select Case Application.Item("webHostObject").evoWebHostType
            'Case eWebSiteTypes.LOCAL
            'SqlConn.ConnectionString = My.Settings.DEFAULT_LOCAL_MSSQL
            '  Case Else
            SqlConn.ConnectionString = Session.Item("localPreferences").UserDatabaseConn
            'End Select

            SqlConn.Open()

            SqlCommand.Connection = SqlConn
            SqlCommand.CommandTimeout = 1000
            SqlCommand.CommandText = Query.ToString

            SqlDataReader = SqlCommand.ExecuteReader()

            dropdown_box.Items.Clear()
            dropdown_box.Items.Add("")
            If SqlDataReader.HasRows Then
                Do While SqlDataReader.Read
                    If Not (IsDBNull(SqlDataReader("amp_program_name")) And Not IsDBNull(SqlDataReader("amp_program_name"))) Then
                        dropdown_box.Items.Add(SqlDataReader("amp_program_name").ToString)
                    End If
                Loop
            End If
            SqlDataReader.Close()
            SqlDataReader = Nothing
        Catch SqlException

            SqlConn.Dispose()
            SqlCommand.Dispose()

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in Get_Model_Name: " & SqlException.Message

        Finally

            SqlCommand.Dispose()
            SqlConn.Close()
            SqlConn.Dispose()

        End Try

        Return tmpStr

    End Function

    Public Function Get_Asset_Model_Id(ByVal amod_id As Integer) As Long
        Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
        Dim SqlCommand As New System.Data.SqlClient.SqlCommand
        Dim SqlConn As New System.Data.SqlClient.SqlConnection
        Dim SqlDataReader As System.Data.SqlClient.SqlDataReader : SqlDataReader = Nothing

        Get_Asset_Model_Id = 0

        Dim Query As String : Query = ""

        Query = "select aimodel_asset_id from Asset_Insight_Model with (NOLOCK) where aimodel_jetnet_amod_id = " & amod_id & ""


        Try

            SqlConn.ConnectionString = Session.Item("localPreferences").UserDatabaseConn

            SqlConn.Open()

            SqlCommand.Connection = SqlConn
            SqlCommand.CommandTimeout = 1000
            SqlCommand.CommandText = Query.ToString

            SqlDataReader = SqlCommand.ExecuteReader()

            If SqlDataReader.HasRows Then
                Do While SqlDataReader.Read
                    If Not IsDBNull(SqlDataReader("aimodel_asset_id")) Then
                        Get_Asset_Model_Id = SqlDataReader("aimodel_asset_id")
                    End If
                Loop
            End If
            SqlDataReader.Close()
            SqlDataReader = Nothing
        Catch SqlException

            SqlConn.Dispose()
            SqlCommand.Dispose()

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in Get_Model_Name: " & SqlException.Message

        Finally

            SqlCommand.Dispose()
            SqlConn.Close()
            SqlConn.Dispose()

        End Try
    End Function
    Public Function Get_AC_MFR_YEAR(ByVal ac_id As Integer) As String
        Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
        Dim SqlCommand As New System.Data.SqlClient.SqlCommand
        Dim SqlConn As New System.Data.SqlClient.SqlConnection
        Dim SqlDataReader As System.Data.SqlClient.SqlDataReader : SqlDataReader = Nothing

        Dim tmpStr As String : tmpStr = "0"

        Dim Query As String : Query = ""
        Query = "SELECT DISTINCT ac_mfr_year FROM Aircraft WITH(NOLOCK) WHERE ac_journ_id = 0 and ac_id = " + ac_id.ToString

        Try

            'Select Case Application.Item("webHostObject").evoWebHostType
            'Case eWebSiteTypes.LOCAL
            'SqlConn.ConnectionString = My.Settings.DEFAULT_LOCAL_MSSQL
            '  Case Else
            SqlConn.ConnectionString = Session.Item("localPreferences").UserDatabaseConn
            'End Select

            SqlConn.Open()

            SqlCommand.Connection = SqlConn
            SqlCommand.CommandTimeout = 1000
            SqlCommand.CommandText = Query.ToString

            SqlDataReader = SqlCommand.ExecuteReader()

            If SqlDataReader.HasRows Then
                SqlDataReader.Read()
                If Not (IsDBNull(SqlDataReader("ac_mfr_year"))) Then
                    tmpStr = SqlDataReader("ac_mfr_year").ToString
                End If
            End If
            SqlDataReader.Close()
            SqlDataReader = Nothing
        Catch SqlException

            SqlConn.Dispose()
            SqlCommand.Dispose()

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in Get_AC_MFR_YEAR: " & SqlException.Message

        Finally

            SqlCommand.Dispose()
            SqlConn.Close()
            SqlConn.Dispose()

        End Try

        Return tmpStr

    End Function
    Public Function Get_AC_DLV_YEAR(ByVal ac_id As Integer) As String
        Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
        Dim SqlCommand As New System.Data.SqlClient.SqlCommand
        Dim SqlConn As New System.Data.SqlClient.SqlConnection
        Dim SqlDataReader As System.Data.SqlClient.SqlDataReader : SqlDataReader = Nothing

        Dim tmpStr As String : tmpStr = "0"

        Dim Query As String : Query = ""
        Query = "SELECT DISTINCT ac_year FROM Aircraft WITH(NOLOCK) WHERE ac_journ_id = 0 and ac_id = " + ac_id.ToString

        Try

            'Select Case Application.Item("webHostObject").evoWebHostType
            'Case eWebSiteTypes.LOCAL
            'SqlConn.ConnectionString = My.Settings.DEFAULT_LOCAL_MSSQL
            '  Case Else
            SqlConn.ConnectionString = Session.Item("localPreferences").UserDatabaseConn
            'End Select

            SqlConn.Open()

            SqlCommand.Connection = SqlConn
            SqlCommand.CommandTimeout = 1000
            SqlCommand.CommandText = Query.ToString

            SqlDataReader = SqlCommand.ExecuteReader()

            If SqlDataReader.HasRows Then
                SqlDataReader.Read()
                If Not (IsDBNull(SqlDataReader("ac_year"))) Then
                    tmpStr = SqlDataReader("ac_year").ToString
                End If
            End If
            SqlDataReader.Close()
            SqlDataReader = Nothing
        Catch SqlException

            SqlConn.Dispose()
            SqlCommand.Dispose()

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in Get_AC_MFR_YEAR: " & SqlException.Message

        Finally

            SqlCommand.Dispose()
            SqlConn.Close()
            SqlConn.Dispose()

        End Try

        Return tmpStr

    End Function
    Public Sub ViewProspects()
        If closeProspects.Visible = True Then
            Toggle_Tabs_Visibility(False, False, False, False, False, False, False, False)
        Else
            Toggle_Tabs_Visibility(False, False, False, False, False, False, True, False)
        End If
        ProspectUpdate.Update()
    End Sub
    Public Sub ViewAircraftOwnership(ByVal sender As Object, ByVal e As System.EventArgs)


        If InStr(ownership_link.CssClass, "float_left 2") > 0 Then
            Toggle_Tabs_Visibility(False, False, False, False, False, False, False, False)
            Me.ownership_link.CssClass = "float_left 1 subMenuText"
        Else

            Me.ownership_link.CssClass = "float_left 2 subMenuText"

            Toggle_Tabs_Visibility(False, False, False, False, False, True, False, False)
            System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ToggleVis", "ToggleButtons('display_block');", True)
        End If


    End Sub

    ''' <summary>
    ''' View analytics button click
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ViewAircraftAnalytics(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles view_analytics.Click
        If InStr(analytics_link.CssClass, "float_left 2") > 0 Then
            'view_analytics.CssClass = "gray_button float_left"
            'analytic_container.CssClass = "dark-theme"
            'view_analytics.Text = "View Analytics"
            'analytic_container.Visible = False
            'System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ToggleVis", "ToggleButtons('display_block');", True)
            Toggle_Tabs_Visibility(False, False, False, False, False, False, False, False)
            Me.analytics_link.CssClass = "float_left 1 subMenuText"
        Else
            Me.analytics_link.CssClass = "float_left 2 subMenuText"
            'Close Map/Update Map Panel
            'map_this_aircraft.CssClass = "gray_button float_left"
            'System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ToggleVis", "ToggleButtons('display_block');", True)
            'map_this_aircraft.Text = "Map Aircraft"
            'RunMap = False
            'ToggleMap()
            'map_update_panel.Update()

            ''Close Events/Update Event Panel
            'view_aircraft_events.Text = "View Events"
            'view_aircraft_events.CssClass = "gray_button float_left"
            'events_container.CssClass = "dark-theme"
            'events_container.Visible = False
            'events_update_panel.Update()

            ''display graph
            'DisplayAnalyticInformation()
            Toggle_Tabs_Visibility(False, True, False, False, False, False, False, False)


            'run needed javascript
            System.Web.UI.ScriptManager.RegisterStartupScript(Me.analytic_update_panel, Me.GetType(), "ToggleAnalytics", "drawVisualization();", True)

            If IsNumeric(DOM.Text) Then
                If DOM.Text > 0 Then
                    System.Web.UI.ScriptManager.RegisterStartupScript(Me.analytic_update_panel, Me.GetType(), "ToggleAnalyticsBar", "drawBarVisualization();", True)
                End If
            End If
            System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ToggleVis", "ToggleButtons('display_block');", True)


        End If
        'update analytic update panel
        analytic_update_panel.Update()
    End Sub
    ''' <summary>
    ''' Field to display Analytic Information and the grab.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DisplayAnalyticInformation()
        Dim ResultsTable As New DataTable
        Dim TotalRunning As Long = 0
        Dim total_since_listed As Integer = 0
        Dim has_stats As Boolean = False
        Dim count_since_date_listed As Integer = 0
        Dim bIsDealer As Boolean = False
        ' aircraft_picture_slideshow.Visible = False
        '  analytics_link.CssClass = "blue_button float_left"
        ' analytics_link.CssClass = "2"
        ' analytic_container.CssClass = "blue-theme"
        closeAnalytics.Visible = True
        'view_analytics.Text = "<strong>Close Analytics</strong>"
        analyticContainer.Visible = False

        Dim htmlOut As New StringBuilder
        Dim x As Integer = 0

        If Session.Item("localUser").crmLocalUserEmailAddress.ToString.ToLower.Contains("@jetnet.com") Or Session.Item("localUser").crmLocalUserEmailAddress.ToString.ToLower.Contains("@mvintech.com") Then
            bIsDealer = True
        Else
            bIsDealer = commonEvo.isDealerCompany(CLng(Session.Item("localUser").crmUserCompanyID.ToString), 0)
        End If


        ResultsTable = Master.aclsData_Temp.DisplayAnalyticInformationSummarizedByDate(CLng(Session.Item("localUser").crmUserCompanyID.ToString), aircraftID, 0, bIsDealer, has_stats, count_since_date_listed, DOM.Text)

        If count_since_date_listed > 0 Then
            clicks_label.Text = "Clicks per Month (Since Date Listed)"
        End If

        If Not IsNothing(ResultsTable) Then
            If ResultsTable.Rows.Count > 0 Then
                analyticContainer.Visible = True

                ' Check to see if the startup script is already registered.

                htmlOut.Append(" var data = new google.visualization.DataTable();" + vbCrLf)
                htmlOut.Append(" data.addColumn('string', 'Month');" + vbCrLf)
                htmlOut.Append(" data.addColumn('number', 'Evolution Clicks');" + vbCrLf)
                htmlOut.Append(" data.addColumn('number', 'Global Clicks');" + vbCrLf)

                htmlOut.Append(" data.addRows(" + ResultsTable.Rows.Count.ToString + ");" + vbCrLf)

                For Each r As DataRow In ResultsTable.Rows
                    htmlOut.Append(" data.setCell(" + x.ToString + ", 0, '" + r.Item("YTMONTH").ToString + "-" + r.Item("YTYEAR").ToString + "');" + vbCrLf)
                    htmlOut.Append(" data.setCell(" + x.ToString + ", 1, " + IIf(CLng(r.Item("tcount").ToString) > 0, FormatNumber(r.Item("tcount").ToString, 0, TriState.False, TriState.False, TriState.False).ToString, "0") + ");" + vbCrLf)
                    If has_stats = True Then
                        htmlOut.Append(" data.setCell(" + x.ToString + ", 2, " + IIf(CLng(r.Item("gcount").ToString) > 0, FormatNumber(r.Item("gcount").ToString, 0, TriState.False, TriState.False, TriState.False).ToString, "0") + ");" + vbCrLf)
                    End If
                    x += 1
                    TotalRunning += r("tcount")
                Next

                System.Web.UI.ScriptManager.RegisterStartupScript(Me.analytic_update_panel, Me.GetType(), "FillDataAnalytics", htmlOut.ToString, True)
                analytic_label.Text = Replace(crmWebClient.DisplayFunctions.CreateAnalyticsSummaryByDate(ResultsTable, Master, "", "100", False, True, has_stats), "data_aircraft_grid float_right fullWidthMobile", "formatTable blue analyticsTable")


            Else
                analytic_label.Text = "No analytic Data"
                'analytic_container.Visible = False
                'view_analytics.Visible = False
            End If

            'if the ac is for sale only, and has a DOM #
            'If status_tab_container.CssClass = "green-theme" Then
            If IsNumeric(DOM.Text) Then
                If DOM.Text > 0 Then
                    ResultsTable = New DataTable
                    ResultsTable = Master.aclsData_Temp.DisplayAnalyticInformationComparingModel(CLng(aircraft_model.Text), aircraftID, Session.Item("localUser").crmUserCompanyID, DOM.Text)
                    If Not IsNothing(ResultsTable) Then
                        If ResultsTable.Rows.Count > 0 Then

                            cstext2 = "data_bar = google.visualization.arrayToDataTable([" & vbNewLine
                            cstext2 += "[' ', 'My AC', 'Other AC']," & vbNewLine
                            For Each r As DataRow In ResultsTable.Rows
                                cstext2 += vbNewLine & "[' ',  " & count_since_date_listed & ", " & r("avgclick") & "],"
                            Next

                            cstext2 = cstext2.TrimEnd(",")

                            cstext2 += "]);"

                            System.Web.UI.ScriptManager.RegisterStartupScript(Me.analytic_update_panel, Me.GetType(), "FillBarAnalytics", cstext2, True)
                        End If
                    End If
                    System.Web.UI.ScriptManager.RegisterStartupScript(Me.analytic_update_panel, Me.GetType(), "ToggleAnalyticsBar", "drawBarVisualization();", True)
                Else
                    toggle_for_sale_analytics.Visible = False
                End If
            Else
                toggle_for_sale_analytics.Visible = False
            End If
            'Else
            '  toggle_for_sale_analytics.Visible = False
            'End If

            System.Web.UI.ScriptManager.RegisterStartupScript(Me.analytic_update_panel, Me.GetType(), "ToggleAnalytics", "drawVisualization();", True)

        Else
            'prep for error
            Master.LogError("DisplayAircraftDetail.aspx.vb - DisplayAnalyticInformationSummarizedByDate() - " & " " & Master.aclsData_Temp.class_error)
            'clear error for data layer class
            Master.aclsData_Temp.class_error = ""
        End If
        'End Display Analytic Information
        ResultsTable.Dispose()
    End Sub



    Public Sub GenerateNewACDetails()
        AircraftTable = New DataTable
        Dim ValueDescription As String = ""
        Dim IcaoCode As String = ""
        Dim IataCode As String = ""
        Dim airportTable As New DataTable
        Dim passCheckbox As New CheckBox
        '


        Dim JetnetForSaleCheck As New DataTable
        Dim JetnetNotForSale As Boolean = False
        Dim ClientNotForSale As Boolean = False
        Dim jetnetTransactionID As Long = 0
        Dim DisplayToggleSwitch As Boolean = False
        passCheckbox.Checked = True

        If Session.Item("localUser").crmEvo = True Then
            If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO Then
                ' If HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.LOCAL Or HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.TEST Then
                If UCase(Session.Item("localSubscription").crmFrequency) = "LIVE" Then
                    DisplayToggleSwitch = True
                End If

                'End If
            End If
        End If

        AircraftTable = CommonAircraftFunctions.BuildReusableTable(aircraftID, journalID, CRMSource, ValueDescription, Master.aclsData_Temp, CRMView, jetnetTransactionID, transactionSource)



        If AircraftTable.Rows.Count > 0 Then

            'Set the Page Title for history or regular aircraft.
            AircraftModel = IIf(Not IsDBNull(AircraftTable.Rows(0).Item("ac_amod_id")), AircraftTable.Rows(0).Item("ac_amod_id"), 0)

            If CRMView = True Then
                If CRMSource = "CLIENT" Then
                    AircraftModel_JETNET = IIf(Not IsDBNull(AircraftTable.Rows(0).Item("jetnet_amod_id")), AircraftTable.Rows(0).Item("jetnet_amod_id"), 0)
                Else
                    AircraftModel_JETNET = AircraftModel
                End If
            Else
                AircraftModel_JETNET = AircraftModel
            End If

            If Not IsDBNull(AircraftTable.Rows(0).Item("ac_ser_nbr")) Then
                ac_ser_nbr = AircraftTable.Rows(0).Item("ac_ser_nbr")
            End If

            aircraft_model.Text = IIf(Not IsDBNull(AircraftTable.Rows(0).Item("ac_amod_id")), AircraftTable.Rows(0).Item("ac_amod_id"), 0)
            jetnet_aircraft_id.Text = IIf(Not IsDBNull(AircraftTable.Rows(0).Item("ac_id")), AircraftTable.Rows(0).Item("ac_id"), 0)

            If CRMView = True Then
                If CRMSource <> "CLIENT" Then
                    Dim otherAircraft As New DataTable
                    otherAircraft = Master.aclsData_Temp.CHECKFORClient_Aircraft_JETNET_AC(aircraftID)
                    If Not IsNothing(otherAircraft) Then
                        If otherAircraft.Rows.Count > 0 Then
                            OtherID = otherAircraft.Rows(0).Item("cliaircraft_id")
                            If Not IsDBNull(otherAircraft.Rows(0).Item("cliaircraft_forsale_flag")) Then
                                ClientNotForSale = IIf(otherAircraft.Rows(0).Item("cliaircraft_forsale_flag") = "Y", False, True)
                            End If
                        End If
                    End If
                ElseIf CRMSource = "CLIENT" Then
                    OtherID = IIf(Not IsDBNull(AircraftTable.Rows(0).Item("ac_id")), AircraftTable.Rows(0).Item("ac_id"), 0)
                    temp_jetnet_ac_id = OtherID
                End If
            End If

            Dim HeaderString As String = ""

            identification_label.Text = CommonAircraftFunctions.Build_Identification_Block("blue", False, "", "100%", "100%", 0, AircraftTable, CRMSource, Me.journalID, Me.aircraftID, Master.aclsData_Temp, New CheckBox, passCheckbox, HttpContext.Current.Session.Item("localPreferences").AerodexFlag, OtherID, CRMView, True, DisplayToggleSwitch, Me, True, CommonAircraftFunctions.CreateHeaderLine(AircraftTable.Rows(0).Item("amod_make_name"), AircraftTable.Rows(0).Item("amod_model_name"), AircraftTable.Rows(0).Item("ac_ser_nbr"), HeaderString))

            'History
            'this basically says that if the journal ID isn't blank, but the above query returns zero, go ahead and query the same thing based on a journal ID of zero and
            'display that information.
            If journalID <> 0 Then
                If AircraftTable.Rows.Count = 0 Then
                    AircraftTable = New DataTable
                    AircraftTable = AclsData_Temp.GetJETNET_ACDetails_PresentAndHistorical(aircraftID, journalID)
                End If
            End If

            Dim HistoryLabelString As String = CommonAircraftFunctions.DisplayAircraftHistory_TopBlock(aircraftID, journalID, JournalTable, Me.Session, history__label, "", AclsData_Temp, CRMSource, AircraftTable.Rows(0).Item("ac_id"), CRMView, OtherID, transactionSource, jetnetTransactionID, HeaderString)
            If Not String.IsNullOrEmpty(HistoryLabelString) Then
                history__label.Text = HistoryLabelString
            End If

            'If journalID > 0 Then
            '    historyHeaderTitle.Text = CommonAircraftFunctions.CreateHeaderLine(AircraftTable.Rows(0).Item("amod_make_name"), AircraftTable.Rows(0).Item("amod_model_name"), AircraftTable.Rows(0).Item("ac_ser_nbr"), HeaderString)
            'Else
            '    headerTextTitle.Text = CommonAircraftFunctions.CreateHeaderLine(AircraftTable.Rows(0).Item("amod_make_name"), AircraftTable.Rows(0).Item("amod_model_name"), AircraftTable.Rows(0).Item("ac_ser_nbr"), "", AircraftModel_JETNET)
            'End If

            If Not IsDBNull(AircraftTable.Rows(0).Item("ac_aport_icao_code")) And Not String.IsNullOrEmpty(AircraftTable.Rows(0).Item("ac_aport_icao_code").ToString) Then
                IcaoCode = AircraftTable.Rows(0).Item("ac_aport_icao_code").ToString.Trim
            End If

            If Not IsDBNull(AircraftTable.Rows(0).Item("ac_aport_iata_code")) And Not String.IsNullOrEmpty(AircraftTable.Rows(0).Item("ac_aport_iata_code").ToString) Then
                IataCode = AircraftTable.Rows(0).Item("ac_aport_iata_code").ToString.Trim
            End If

            If Not IsDBNull(AircraftTable.Rows(0).Item("ac_product_commercial_flag")) And Not String.IsNullOrEmpty(AircraftTable.Rows(0).Item("ac_product_commercial_flag").ToString) Then
                If Trim(AircraftTable.Rows(0).Item("ac_product_commercial_flag").ToString.Trim) = "Y" Then
                    is_commercial_Ac = True
                Else
                    is_commercial_Ac = False
                End If


            End If

            'If runMap = True Then
            'This has been added to figure out the latitude/longitude of the aport.
            If IcaoCode <> "" Or IataCode <> "" Then
                airportTable = AclsData_Temp.AirportList(0, IcaoCode, IataCode)
                If Not IsNothing(airportTable) Then
                    If airportTable.Rows.Count > 0 Then
                        AportLat = IIf(Not IsDBNull(airportTable.Rows(0).Item("aport_latitude_decimal")), airportTable.Rows(0).Item("aport_latitude_decimal"), 0)
                        AportLong = IIf(Not IsDBNull(airportTable.Rows(0).Item("aport_longitude_decimal")), airportTable.Rows(0).Item("aport_longitude_decimal"), 0)
                    End If
                End If
            End If

            'Sets the lat/long inside a textbox to be used later on subsequent postbacks
            If AportLat = 0 And AportLong = 0 Then
                map_this_aircraft.Visible = False
                Latitude.Text = 0
                Longitude.Text = 0
            Else
                Latitude.Text = AportLat
                Longitude.Text = AportLong
            End If
            'Checks toggle map
            ToggleMap()
            map_update_panel.Update()


            If journalID = 0 Then
                If Not IsDBNull(AircraftTable.Rows(0).Item("amod_make_name")) And Not IsDBNull(AircraftTable.Rows(0).Item("amod_model_name")) Then
                    If Not IsDBNull(AircraftTable.Rows(0).Item("ac_ser_nbr")) And Not String.IsNullOrEmpty(AircraftTable.Rows(0).Item("ac_ser_nbr").ToString) Then
                        aircraftPageTitle.Text = (AircraftTable.Rows(0).Item("amod_make_name").ToString & " " & AircraftTable.Rows(0).Item("amod_model_name").ToString & " " & "S/N " + AircraftTable.Rows(0).Item("ac_ser_nbr").ToString & IIf(UCase(CRMSource) = "CLIENT", " CLIENT ", "") & " ")
                    Else
                        aircraftPageTitle.Text = (AircraftTable.Rows(0).Item("amod_make_name").ToString & " " & AircraftTable.Rows(0).Item("amod_model_name").ToString & IIf(UCase(CRMSource) = "CLIENT", " CLIENT ", "") & " ")
                    End If
                End If

            ElseIf JournalTable.Rows.Count > 0 Then
                flightContainer.Visible = False
                If Not IsDBNull(AircraftTable.Rows(0).Item("amod_make_name")) And Not IsDBNull(AircraftTable.Rows(0).Item("amod_model_name")) Then
                    If Not IsDBNull(AircraftTable.Rows(0).Item("ac_ser_nbr")) And Not String.IsNullOrEmpty(AircraftTable.Rows(0).Item("ac_ser_nbr").ToString) Then
                        aircraftPageTitle.Text = (AircraftTable.Rows(0).Item("amod_make_name").ToString & " " & AircraftTable.Rows(0).Item("amod_model_name").ToString & " " & "S/N " + AircraftTable.Rows(0).Item("ac_ser_nbr").ToString + IIf(journalID <> 0, " History " & IIf(JournalTable.Rows.Count > 0, "(" & JournalTable.Rows(0).Item("journ_date") & ")", "") & "", ""))
                    Else
                        aircraftPageTitle.Text = (AircraftTable.Rows(0).Item("amod_make_name").ToString & " " & AircraftTable.Rows(0).Item("amod_model_name").ToString & IIf(journalID <> 0, " History " & IIf(JournalTable.Rows.Count > 0, "(" & JournalTable.Rows(0).Item("journ_date") & ")", "") & "", " "))
                    End If
                End If

                If Not IsDBNull(JournalTable.Rows(0).Item("journ_subcategory_code")) Then
                    If Not String.IsNullOrEmpty(JournalTable.Rows(0).Item("journ_subcategory_code")) Then
                        If JournalTable.Rows(0).Item("journ_subcategory_code").ToString = "OM" Or JournalTable.Rows(0).Item("journ_subcategory_code").ToString = "MA" Or JournalTable.Rows(0).Item("journ_subcategory_code").ToString = "MS" Then
                            view_folders.Visible = False
                        End If
                    End If
                End If
            End If

            If CRMSource = "CLIENT" Then
                JetnetForSaleCheck = localDataLayer.Check_Jetnet_Off_Market_Aircraft(AircraftTable.Rows(0).Item("ac_id"))
                If Not IsNothing(JetnetForSaleCheck) Then
                    If JetnetForSaleCheck.Rows.Count > 0 Then
                        If JetnetForSaleCheck.Rows(0).Item(0) = 0 Then
                            JetnetNotForSale = True
                        End If
                    End If
                End If
            End If

            If CRMView = True And journalID = 0 Then
                If clsGeneral.clsGeneral.isEValuesAvailable() = True Then
                    'If CRMSource = "CLIENT" Then
                    '  temp_ac_mfr_year = Get_AC_MFR_YEAR(CLng(jetnet_aircraft_id.Text))
                    'Else
                    '  temp_ac_mfr_year = Get_AC_MFR_YEAR(aircraftID)
                    'End If

                    If CRMSource = "CLIENT" Then
                        temp_ac_dlv_year = Get_AC_DLV_YEAR(CLng(jetnet_aircraft_id.Text))
                    Else
                        temp_ac_dlv_year = Get_AC_DLV_YEAR(aircraftID)
                    End If
                End If

                values_label.Text = CommonAircraftFunctions.Build_ValuesBlock(localDataLayer, valuation_chart, jetnet_aircraft_id.Text, IIf(CRMSource = "CLIENT", aircraftID, OtherID), Page, valuesUpdatePanel, CRMSource, AircraftModel_JETNET, Page, temp_ac_dlv_year)

                If CRMSource = "CLIENT" Then
                    custom_label.Text = CommonAircraftFunctions.CreateCustomBlock("100%", True, CRMSource, aircraftID, "blue", AclsData_Temp, AircraftTable.Rows(0).Item("custom_1"), AircraftTable.Rows(0).Item("custom_2"), AircraftTable.Rows(0).Item("custom_3"), AircraftTable.Rows(0).Item("custom_4"), AircraftTable.Rows(0).Item("custom_5"), AircraftTable.Rows(0).Item("custom_6"), AircraftTable.Rows(0).Item("custom_7"), AircraftTable.Rows(0).Item("custom_8"), AircraftTable.Rows(0).Item("custom_9"), AircraftTable.Rows(0).Item("custom_10"))
                    If Not String.IsNullOrEmpty(custom_label.Text) Then
                        custom_label.Visible = True
                    End If
                End If
            ElseIf clsGeneral.clsGeneral.isEValuesAvailable() = True And journalID = 0 Then

                ' temp_ac_mfr_year = Get_AC_MFR_YEAR(aircraftID)

                temp_ac_dlv_year = Get_AC_DLV_YEAR(aircraftID)

                values_label.Text = CommonAircraftFunctions.Build_ValuesBlock(localDataLayer, valuation_chart, jetnet_aircraft_id.Text, IIf(CRMSource = "CLIENT", aircraftID, OtherID), Page, valuesUpdatePanel, CRMSource, AircraftModel_JETNET, Page, temp_ac_dlv_year)
            End If


            airframe_label.Text = CommonAircraftFunctions.Build_AirframeBlock("blue", False, "", "100%", "100%", AircraftTable, CRMSource, aircraftID, CRMView, IIf(journalID = 0, True, False), is_commercial_Ac, est_aftt, est_landings, est_as_of_date)



            status_label.Text = CommonAircraftFunctions.Build_Status_Block(Me.aircraftID, Me.journalID, JournalTable, AircraftTable, False, HttpContext.Current.Session.Item("localPreferences").AerodexFlag, 0, "100%", "100%", "blue", "", AclsData_Temp, New TextBox, passCheckbox, New CheckBox, New CheckBox, CRMSource, ValueDescription, DOM, JetnetNotForSale, ClientNotForSale, IIf(journalID = 0, True, False), transactionSource)


            engine_label.Text += CommonAircraftFunctions.Build_Engine_Block(aircraftID, AircraftTable, False, 0, "100%", "100%", "blue", "", CRMSource, IIf(journalID = 0, True, False), Me.Session, bShowBlankAcFields)

            If Not String.IsNullOrEmpty(engine_label.Text) Then
                engine_label.Visible = True
            End If

            apu__label.Text = CommonAircraftFunctions.Build_APU_Block(AircraftTable, False, 0, "100%", "100%", "blue", "", CRMSource, aircraftID, IIf(journalID = 0, True, False), AclsData_Temp)

            If Not String.IsNullOrEmpty(apu__label.Text) Then
                apu__label.Visible = True
            End If

            'cockpit_label.Text = CommonAircraftFunctions.Build_Details_Block("addl cockpit equipment", AircraftTable, "", "", CRMSource, False, 0, "100%", "100%", "blue", "", AclsData_Temp)
            equipment_label.Text = CommonAircraftFunctions.Build_Details_Block("Addl Cockpit Equipment','Equipment", AircraftTable, "", "", CRMSource, False, 0, "100%", "100%", "blue", "", AclsData_Temp, aircraftID, CRMView, IIf(journalID = 0, True, False), journalID, OtherID)
            If Not String.IsNullOrEmpty(equipment_label.Text) Then
                equipment_label.Visible = True
            End If


            maintenance_label.Text = CommonAircraftFunctions.Build_Details_Block("maintenance", AircraftTable, "", "", CRMSource, False, 0, "100%", "100%", "blue", "", AclsData_Temp, aircraftID, CRMView, IIf(journalID = 0, True, False), journalID, OtherID, est_afmp)
            If Not String.IsNullOrEmpty(maintenance_label.Text) Then
                maintenance_label.Visible = True
            End If


            interior_label.Text = CommonAircraftFunctions.Build_Details_Block("interior", AircraftTable, "", "", CRMSource, False, 0, "100%", "100%", "blue", "", AclsData_Temp, aircraftID, CRMView, IIf(journalID = 0, True, False), journalID, OtherID)
            If Not String.IsNullOrEmpty(interior_label.Text) Then
                interior_label.Visible = True
            End If


            exterior_label.Text = CommonAircraftFunctions.Build_Details_Block("exterior", AircraftTable, "", "", CRMSource, False, 0, "100%", "100%", "blue", "", AclsData_Temp, aircraftID, CRMView, IIf(journalID = 0, True, False), journalID, OtherID)
            If Not String.IsNullOrEmpty(exterior_label.Text) Then
                exterior_label.Visible = True
            End If
            avionics_label.Text = CommonAircraftFunctions.Build_Avionics_Block(aircraftID, journalID, 0, "", AircraftTable, AclsData_Temp, CRMSource, False, 0, "100%", "100%", "blue", IIf(journalID = 0, True, False), OtherID)
            If Not String.IsNullOrEmpty(avionics_label.Text) Then
                avionics_label.Visible = True
            End If

            features_label.Text = (CommonAircraftFunctions.DisplayKeyFeatures(AclsData_Temp, Me.Session, AircraftTable, CRMSource, "", bShowBlankAcFields, "blue", aircraftID, IIf(journalID = 0, True, False), journalID, OtherID))
            If Not String.IsNullOrEmpty(features_label.Text) Then
                features_label.Visible = True
            End If
            If (Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER) Then
                attributes_label.Text = (CommonAircraftFunctions.DisplayAttributes(AclsData_Temp, Me.Session, AircraftTable, CRMSource, "", bShowBlankAcFields, "blue", aircraftID, IIf(journalID = 0, True, False), journalID, OtherID))
                If Not String.IsNullOrEmpty(attributes_label.Text) Then
                    attributes_label.Visible = True
                End If
            End If

            propeller_tab_label.Text = (CommonAircraftFunctions.DisplayPropRotorInfo_Vertical(Me.Session, AircraftTable, propeller_tab_label, AclsData_Temp, bShowBlankAcFields))

            If Not String.IsNullOrEmpty(propeller_tab_label.Text) Then
                propeller_tab_label.Visible = True
            End If

            If Not IsNothing(JournalTable) Then


                If JournalTable.Rows.Count > 0 Then
                    If journalID = 0 Then
                        history_label.Text = (CommonAircraftFunctions.DisplayAircraftHistory_BottomBlock(JournalTable, Me.Application, Me.Session, Me.bExtraJFWAFW, Me.aircraftID, securityTokenLocal, CRMView, AclsData_Temp, AircraftTable, CRMSource, False))
                    End If
                End If
            End If


            If Not IsNothing(JournalTable) Then
                If JournalTable.Rows.Count > 0 Then
                    If journalID = 0 Then
                        prospects_label.Text = (CommonAircraftFunctions.DisplayAircraftHistory_BottomBlock(JournalTable, Me.Application, Me.Session, Me.bExtraJFWAFW, Me.aircraftID, securityTokenLocal, CRMView, AclsData_Temp, AircraftTable, CRMSource, False))
                    End If
                End If
            End If



            If bFromJFWAFW Then
                aircraft_picture_slideshow.Text = (CommonAircraftFunctions.GetAircraftPictures(aircraft_picture_slideshow, AclsData_Temp, Me.Session, AircraftTable, False, IIf(bFromView, "Y", "N"), currentRecord, slideshow_script, step_script, CRMView))
            Else
                aircraft_picture_slideshow.Text = (CommonAircraftFunctions.GetAircraftPictures(aircraft_picture_slideshow, AclsData_Temp, Me.Session, AircraftTable, True, IIf(bFromView, "Y", "N"), currentRecord, slideshow_script, step_script, CRMView))
            End If




            FlightDataBuild(AircraftTable)

            If bFromJFWAFW Then
                aircraft_contacts_label.Text = (CommonAircraftFunctions.GetCompanies_DisplayAircraftDetails(Me.Session, AircraftTable, False, False, Me.bExtraJFWAFW, AclsData_Temp, CRMSource, journalID, OtherID, aircraftID))
            Else
                aircraft_contacts_label.Text = (CommonAircraftFunctions.GetCompanies_DisplayAircraftDetails(Me.Session, AircraftTable, IIf(CRMView = True And HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CRM, False, True), False, False, AclsData_Temp, CRMSource, journalID, OtherID, aircraftID))
            End If


            If CommonAircraftFunctions.GetAircraft_Ownership(AclsData_Temp, AircraftTable.Rows(0).Item("ac_id")) <> "" Then
                Me.view_ac_insight.Visible = True
                Me.VALUES_UL.Visible = True
                'Me.li_start2.Visible = True
                'Me.li_end2.Visible = True
                'Me.ownership_link.Visible = True
                viewOwnershipToggle.Visible = False
                Me.ownership_link.Text = "Aircraft Ownership"
            End If

            If AircraftTable.Rows(0).Item("ac_lease_flag").ToString = "Y" Then
                'Build Lease Table to fill the tab container with.
                lease_tab_label.Text = (CommonAircraftFunctions.DisplayLeaseDetails(AircraftTable, AclsData_Temp))
            End If
            If lease_tab_label.Text = "" Then
                lease_tab_label.Visible = False
            End If

        End If

    End Sub
    Public Sub Build_Operator_History(ByVal AircraftTable As DataTable)
        Dim tabTitle As String = "OPERATOR HISTORY"
        Dim tmpFlightDataTable As New DataTable

        Try
            jetnet_aircraft_id.Text = jetnet_aircraft_id.Text

            tmpFlightDataTable = flight_data_temp.Get_Operator_History_Data(jetnet_aircraft_id.Text)

            operator_history_label.Text = flight_data_temp.Display_Operator_History_Function(tmpFlightDataTable, tabTitle)


        Catch ex As Exception

        End Try

    End Sub
    Private Sub FlightDataBuild(ByVal AircraftTable As DataTable)
        If Me.journalID = 0 Then
            Dim temp_reg As String = ""
            Dim tabTitle As String = "RECENT FLIGHT ACTIVITY"
            Dim tmpFlightDataTable As New DataTable
            If Not IsDBNull(AircraftTable.Rows(0).Item("ac_reg_nbr")) Then
                temp_reg = Trim(AircraftTable.Rows(0).Item("ac_reg_nbr"))
            Else
                temp_reg = ""
            End If


            If Trim(Session.Item("useFAAFlightData")) <> "" And Trim(Session.Item("useFAAFlightData")) <> "ARGUS" And HttpContext.Current.Session.Item("localPreferences").AerodexStandard = False Then

                '  If Not IsDBNull(AircraftTable.Rows(0).Item("ac_reg_nbr")) Then ' you need to check for "null" values

                ' if its on the blocked list, if not, check to see if there is any data 
                If flight_data_temp.IS_ON_BLOCKED_LIST(temp_reg) = True Then

                    tmpFlightDataTable = flight_data_temp.getFAAFlightData(temp_reg, AircraftTable.Rows(0).Item("ac_id"), Nothing, Nothing, True, Month(DateAdd(DateInterval.Month, -3, Now())) & "/" & Day(DateAdd(DateInterval.Month, -3, Now())) & "/" & Year(DateAdd(DateInterval.Month, -3, Now())), "", 0, 0, False, 0, True)

                    If Not IsNothing(tmpFlightDataTable) Then
                        ' aircraft_flight_activity.Visible = True
                        flightContainer.Visible = True
                        flightContainer.CssClass = ""
                        aircraft_flight_tab_label.Text = flight_data_temp.displayFAAFlightData(tmpFlightDataTable, tabTitle, "last_year", "", AircraftTable.Rows(0).Item("ac_id"), DateAdd(DateInterval.Day, -90, Date.Now.Date), True, temp_reg, True)
                        aircraft_flight_tab_label.Text &= "<table><tr><td align='left' colspan='5'><A href='FAAFlightData.aspx?acid=" & AircraftTable.Rows(0).Item("ac_id") & "' target='_blank'>View Flight Activity Map & Details</a></td></tr></table>"
                        If InStr(aircraft_flight_tab_label.Text, "No Flight Activity") > 0 Then
                        Else
                            Me.view_ac_insight.Visible = True
                            Me.VALUES_UL.Visible = True
                            'Me.li_start0.Visible = True
                            'Me.li_end0.Visible = True
                            viewUtilToggle.Visible = True
                            Me.util_link.Visible = True
                            Me.util_link.Text = "<a href='FAAFlightData.aspx?acid=" & AircraftTable.Rows(0).Item("ac_id") & "&activetab=4' target='_blank'  class=""subMenuText"">Flight Activity</a>"

                        End If
                    End If

                    ' if there is data in the clean table
                ElseIf flight_data_temp.checkForFAAFlightData(temp_reg, AircraftTable.Rows(0).Item("ac_id"), False, True) Or Trim(temp_reg) = "" Then

                    tmpFlightDataTable = flight_data_temp.getFAAFlightData(temp_reg, AircraftTable.Rows(0).Item("ac_id"), Nothing, Nothing, False, Month(DateAdd(DateInterval.Month, -3, Now())) & "/" & Day(DateAdd(DateInterval.Month, -3, Now())) & "/" & Year(DateAdd(DateInterval.Month, -3, Now())), "", 0, 0, False, 0, True)

                    If Not IsNothing(tmpFlightDataTable) Then
                        flightContainer.CssClass = ""
                        flightContainer.Visible = True
                        ' aircraft_flight_activity.Visible = True
                        aircraft_flight_tab_label.Text = flight_data_temp.displayFAAFlightData(tmpFlightDataTable, tabTitle, "90_days", "", AircraftTable.Rows(0).Item("ac_id"), DateAdd(DateInterval.Day, -90, Date.Now.Date), True, temp_reg, False)
                        If InStr(aircraft_flight_tab_label.Text, "No Flight Activity") > 0 Then
                        Else
                            Me.view_ac_insight.Visible = True
                            Me.VALUES_UL.Visible = True
                            'Me.li_start0.Visible = True
                            'Me.li_end0.Visible = True
                            viewUtilToggle.Visible = True
                            Me.util_link.Visible = True
                            Me.util_link.Text = "<a href=""FAAFlightData.aspx?acid=" & AircraftTable.Rows(0).Item("ac_id") & "&activetab=4"" target=""_blank"" class=""subMenuText"">Flight Activity</a>"
                        End If

                    End If


                    '    End If

                    'if there is no data in the clean table, and its not blocked, there shouldnt be any data
                    If flightContainer.Visible = False Then
                        aircraft_flight_tab_label.Text = ("<table cellpadding=""3"" cellspacing=""0"" width=""99%"" border='1'>")
                        aircraft_flight_tab_label.Text &= ("<tr><td valign=""middle"" align=""left""><strong>Date</strong></td>")
                        aircraft_flight_tab_label.Text &= ("<td valign=""middle"" align=""left""><strong>Origin</strong></td>")
                        aircraft_flight_tab_label.Text &= ("<td valign=""middle"" align=""left""><strong>Destination</strong></td>")
                        aircraft_flight_tab_label.Text &= ("<td valign=""middle"" align=""left""><strong>Distance</strong><em>(sm)</em></td>")
                        aircraft_flight_tab_label.Text &= ("<td valign=""middle"" align=""left"">")
                        aircraft_flight_tab_label.Text &= ("<strong>Flight&nbsp;Time</strong><em>(min)</em></td></tr>") '</a> 
                        aircraft_flight_tab_label.Text &= ("<tr><td valign=""middle"" align=""left"" colspan=""5"">No Recent Flight Activity</td></tr>")
                        aircraft_flight_tab_label.Text &= ("</table>")
                    End If

                End If

            ElseIf HttpContext.Current.Session.Item("localPreferences").AerodexStandard = False Then

                If Not IsDBNull(AircraftTable.Rows(0).Item("ac_reg_no_search")) Then ' you need to check for "null" values

                    If CommonAircraftFunctions.checkForBarred_AC(AircraftTable.Rows(0).Item("ac_reg_no_search").ToString.Trim, AclsData_Temp) Then
                        ' there is activity but the aircraft is on barr list
                        'aircraft_flight_activity.Visible = True

                        flightContainer.Visible = True
                        flightContainer.CssClass = ""
                        aircraft_flight_tab_label.Text = ("<table><tr><th valign='middle' align='left' colspan='5'>Detailed flight data for this aircraft (REG#" & AircraftTable.Rows(0).Item("ac_reg_no_search").ToString.Trim & ") is not available for public viewing based on the request of the owner/operator.</th></tr>")
                        aircraft_flight_tab_label.Text &= "</table>"
                    Else
                        If CommonAircraftFunctions.checkForFlightData(AircraftTable.Rows(0).Item("ac_reg_no_search").ToString.Trim, AclsData_Temp, False) Then
                            If CommonAircraftFunctions.checkForFlightData(AircraftTable.Rows(0).Item("ac_reg_no_search").ToString.Trim, AclsData_Temp, True) Then
                                'build table for flight data to fill tab container
                                ' aircraft_flight_activity.Visible = True
                                flightContainer.Visible = True
                                flightContainer.CssClass = ""
                                aircraft_flight_tab_label.Text = (CommonAircraftFunctions.DisplayFlightData(AircraftTable, CRMView, AclsData_Temp))
                            Else
                                ' there is activity but the aircraft is on barr list
                                ' aircraft_flight_activity.Visible = True
                                flightContainer.CssClass = ""
                                flightContainer.Visible = "true"
                                aircraft_flight_tab_label.Text = ("<table><tr><th valign='middle' align='left' colspan='5'>Detailed flight data for this aircraft (REG#" & AircraftTable.Rows(0).Item("ac_reg_no_search").ToString.Trim & ") is not available for public viewing based on the request of the owner/operator. </th></tr></table>")
                            End If
                        Else
                            ' there is no flight activity
                            flightContainer.CssClass = ""
                            flightContainer.Visible = True
                            'aircraft_flight_activity.Visible = True
                            aircraft_flight_tab_label.Text = ("<table><tr><th valign='middle' align='left' colspan='5'>No Flight Data Available for the last 90 days.</th></tr></table>")
                        End If
                    End If

                End If

            End If
        End If
    End Sub
    '''' <summary>
    '''' First block - generates aircraft details
    '''' </summary>
    '''' <remarks></remarks>
    'Public Sub generateDisplayAcDetails1()
    '  'Runs Display String for Aircraft Details Block. Displays Information Block and Status Tab Info
    '  'Aircraft_Display_String = (CommonAircraftFunctions.DisplayAircraftDetailsBlock(AclsData_Temp, Me.aircraftID, Me.journalID, Me.bExtraJFWAFW, True, AircraftTable, JournalTable, Me.Session, aircraft_status_label, status_tab, stats_tab, status_tab_container, usage_tab_container, history_information, history_information_label, history_information_panel, aircraft_stats, Notes, Reminders, company_tab_container, False, AportLat, AportLong, RunMap, DOM, CRMSource, DisplayAnalyticsButton, temp_jetnet_ac_id))
    '  ''Sets the lat/long inside a textbox to be used later on subsequent postbacks
    '  'If AportLat = 0 And AportLong = 0 Then
    '  '  map_this_aircraft.Visible = False
    '  '  Latitude.Text = 0
    '  '  Longitude.Text = 0
    '  'Else
    '  '  Latitude.Text = AportLat
    '  '  Longitude.Text = AportLong
    '  'End If
    '  ''Checks toggle map
    '  'ToggleMap()
    '  'map_update_panel.Update()

    '  ''Set the Page Title for history or regular aircraft.
    '  'If Not IsNothing(AircraftTable) Then
    '  '  If AircraftTable.Rows.Count > 0 Then
    '  '    AircraftModel = IIf(Not IsDBNull(AircraftTable.Rows(0).Item("ac_amod_id")), AircraftTable.Rows(0).Item("ac_amod_id"), 0)
    '  '    aircraft_model.Text = IIf(Not IsDBNull(AircraftTable.Rows(0).Item("ac_amod_id")), AircraftTable.Rows(0).Item("ac_amod_id"), 0)
    '  '    jetnet_aircraft_id.Text = IIf(Not IsDBNull(AircraftTable.Rows(0).Item("ac_id")), AircraftTable.Rows(0).Item("ac_id"), 0)
    '  '    If journalID = 0 Then
    '  '      If Not IsDBNull(AircraftTable.Rows(0).Item("amod_make_name")) And Not IsDBNull(AircraftTable.Rows(0).Item("amod_model_name")) Then
    '  '        If Not IsDBNull(AircraftTable.Rows(0).Item("ac_ser_nbr")) And Not String.IsNullOrEmpty(AircraftTable.Rows(0).Item("ac_ser_nbr").ToString) Then
    '  '          aircraftPageTitle.Text = (AircraftTable.Rows(0).Item("amod_make_name").ToString & " " & AircraftTable.Rows(0).Item("amod_model_name").ToString & " " & "S/N " + AircraftTable.Rows(0).Item("ac_ser_nbr").ToString & IIf(UCase(CRMSource) = "CLIENT", " CLIENT ", "") & " ")
    '  '        Else
    '  '          aircraftPageTitle.Text = (AircraftTable.Rows(0).Item("amod_make_name").ToString & " " & AircraftTable.Rows(0).Item("amod_model_name").ToString & IIf(UCase(CRMSource) = "CLIENT", " CLIENT ", "") & " ")
    '  '        End If
    '  '      End If

    '  '    ElseIf JournalTable.Rows.Count > 0 Then
    '  '      If Not IsDBNull(AircraftTable.Rows(0).Item("amod_make_name")) And Not IsDBNull(AircraftTable.Rows(0).Item("amod_model_name")) Then
    '  '        If Not IsDBNull(AircraftTable.Rows(0).Item("ac_ser_nbr")) And Not String.IsNullOrEmpty(AircraftTable.Rows(0).Item("ac_ser_nbr").ToString) Then
    '  '          aircraftPageTitle.Text = (AircraftTable.Rows(0).Item("amod_make_name").ToString & " " & AircraftTable.Rows(0).Item("amod_model_name").ToString & " " & "S/N " + AircraftTable.Rows(0).Item("ac_ser_nbr").ToString + IIf(journalID <> 0, " History " & IIf(JournalTable.Rows.Count > 0, "(" & JournalTable.Rows(0).Item("journ_date") & ")", "") & "", ""))
    '  '        Else
    '  '          aircraftPageTitle.Text = (AircraftTable.Rows(0).Item("amod_make_name").ToString & " " & AircraftTable.Rows(0).Item("amod_model_name").ToString & IIf(journalID <> 0, " History " & IIf(JournalTable.Rows.Count > 0, "(" & JournalTable.Rows(0).Item("journ_date") & ")", "") & "", " "))
    '  '        End If
    '  '      End If

    '  '      If Not IsDBNull(JournalTable.Rows(0).Item("journ_subcategory_code")) Then
    '  '        If Not String.IsNullOrEmpty(JournalTable.Rows(0).Item("journ_subcategory_code")) Then
    '  '          If JournalTable.Rows(0).Item("journ_subcategory_code").ToString = "OM" Or JournalTable.Rows(0).Item("journ_subcategory_code").ToString = "MA" Or JournalTable.Rows(0).Item("journ_subcategory_code").ToString = "MS" Then
    '  '            view_folders.Visible = False
    '  '          End If
    '  '        End If
    '  '      End If
    '  '    End If
    '  '  End If
    '  'End If

    'End Sub
    '''' <summary>
    '''' second block generates secondary ac details.
    '''' </summary>
    '''' <remarks></remarks>
    'Public Sub generateDisplayAcDetails2()
    '  aircraft_details_bottom.Text = ""

    '  Dim tmpFlightDataTable As DataTable = Nothing
    '  Dim tmpTripDataTable As DataTable = Nothing
    '  Dim tmpAircraftInfoTable As DataTable = Nothing
    '  Dim faa_temp_table As New DataTable
    '  Dim DamageCode As String = ""
    '  Dim temp_reg As String = ""

    '  Try
    '    'This is a container table that is going to put
    '    'the aircraft images on the right side
    '    'and the main aircraft information on the left hand side.
    '    If AircraftTable.Rows.Count > 0 Then

    '      'Picture display
    '      'If Session.Item("localUser").crmDontShowPics Then
    '      '    aircraft_picture_slideshow.Text = ("<br /><strong>Display Aircraft Pictures is turned off! Please, Check your preferences.</strong><br />")
    '      'Else
    '      'If bFromJFWAFW Then
    '      '  aircraft_picture_slideshow.Text = (CommonAircraftFunctions.GetAircraftPictures(AclsData_Temp, Me.Session, AircraftTable, False, IIf(bFromView, "Y", "N"), currentRecord, slideshow_script, step_script, CRMView))
    '      'Else
    '      '  aircraft_picture_slideshow.Text = (CommonAircraftFunctions.GetAircraftPictures(AclsData_Temp, Me.Session, AircraftTable, True, IIf(bFromView, "Y", "N"), currentRecord, slideshow_script, step_script, CRMView))
    '      'End If
    '      'End If

    '      'If CommonAircraftFunctions.GetAircraft_Ownership(AclsData_Temp, AircraftTable.Rows(0).Item("ac_id")) <> "" Then
    '      '  Me.view_ac_insight.Visible = True
    '      '  Me.li_start2.Visible = True
    '      '  Me.li_end2.Visible = True
    '      '  Me.ownership_link.Visible = True
    '      '  Me.ownership_link.Text = "Aircraft Ownership"
    '      'End If


    '      'update picture panel 
    '      'picture_update_panel.Update()

    '      'Fill the APU table to fill the tab container with.
    '      'apu_label.Text = (CommonAircraftFunctions.DisplayAPUDetails(Me.Session, AircraftTable, apu_tab, AclsData_Temp, bShowBlankAcFields))
    '      'Fill the Equipment table to fill the tab container with. 
    '      'equip_label.Text = (CommonAircraftFunctions.DisplayEquipmentDetails(AclsData_Temp, Me.Session, AircraftTable, CRMSource))

    '      'Build Key Feature Table to fill the tab container with.
    '      'features_label.Text = (CommonAircraftFunctions.DisplayKeyFeatures(AclsData_Temp, Me.Session, AircraftTable, CRMSource, DamageCode, bShowBlankAcFields, ""))


    '      'Fill the Maintenance table  to fill the tab container with.
    '      'maint_label.Text = (CommonAircraftFunctions.DisplayMaintenanceDetails(AclsData_Temp, Me.Session, AircraftTable, CRMSource, bShowBlankAcFields, DamageCode))
    '      'filling the ac information with the build string and then clearing it.
    '      ' aircraft_information_label.Text = (Aircraft_Display_String)
    '      Aircraft_Display_String = ""

    '      'Building Usage Table to fill up the Label in the Usage Tab Container


    '      ' If Trim(Session.Item("useFAAFlightData")) <> "" And Trim(Session.Item("useFAAFlightData")) <> "ARGUS" Then
    '      'If AircraftTable.Rows(0).Item("ac_journ_id").ToString = 0 Then
    '      '  faa_temp_table = flight_data_temp.getAllFAAFlightData(AircraftTable.Rows(0).Item("ac_reg_nbr").ToString, AircraftTable.Rows(0).Item("ac_id").ToString, AircraftTable.Rows(0).Item("ac_date_engine_times_as_of").ToString)
    '      '  aircraft_usage_label.Text = flight_data_temp.displayAirframeTimesData(faa_temp_table, AircraftTable.Rows(0).Item("ac_date_engine_times_as_of").ToString, AircraftTable.Rows(0).Item("ac_airframe_total_hours").ToString, AircraftTable.Rows(0).Item("ac_airframe_total_landings").ToString, True, AircraftTable.Rows(0).Item("ac_previously_owned_flag").ToString.ToUpper, IIf(Not IsDBNull(AircraftTable.Rows(0).Item("ac_date_purchased")), AircraftTable.Rows(0).Item("ac_date_purchased"), ""), IIf(Not IsDBNull(AircraftTable.Rows(0).Item("ac_year_dlv")), IIf(IsNumeric(AircraftTable.Rows(0).Item("ac_year_dlv")), AircraftTable.Rows(0).Item("ac_year_dlv"), 0), 0), bShowBlankAcFields)


    '      '  '  aircraft_usage_label.Text = "<table cellpadding='3' cellspacing='0' width='100%' border='1'><tr><td valign='middle' align='center' colspan='1'>&nbsp;</td><td valign='middle' align='center' colspan='1' class='header'><span class='label'>Current<br>Airframe&nbsp;Values</span></td>"
    '      '  '  aircraft_usage_label.Text &= "<td valign='middle' align='center' colspan='1' bgcolor='#F6CECE' class='display_none'><span class='label'>Flight Activity</span></td><td valign='middle' align='center' colspan='1' bgcolor='#F6CECE' class='display_none'><span class='label'>Estimated<br>Airframe&nbsp;Values</span></td></tr>"
    '      '  '  aircraft_usage_label.Text &= "<tr class='alt_row'><td valign='middle' align='left' class='header'><span class='label'>Data Valid As of</span></td><td valign='middle' align='right'>N/R&nbsp;</td><td valign='middle' align='right' nowrap='nowrap' bgcolor='#F6CECE' class='display_none'>Beginning - 10/31/2015&nbsp;</td><td valign='middle' align='right' bgcolor='#F6CECE' class='display_none'>10/31/2015&nbsp;</td></tr><tr><td valign='middle' align='left' class='header'><span class='label'>Airframe Total Time<br>(AFTT) (hrs):</span></td><td valign='middle' align='right'>N/R&nbsp;</td><td valign='middle' align='right' bgcolor='#F6CECE' class='display_none'>1,044&nbsp;</td><td valign='middle' align='right' bgcolor='#F6CECE' class='display_none'>1,044&nbsp;</td></tr><tr class='alt_row'><td valign='middle' align='left' class='header'><span class='label'>Landings/Cycles:</span></td><td valign='middle' align='right'>N/R&nbsp;</td><td valign='middle' align='right' bgcolor='#F6CECE' class='display_none'>411&nbsp;</td><td valign='middle' align='right' bgcolor='#F6CECE' class='display_none'>411&nbsp;</td></tr><tr><td valign='middle' align='left' class='header'><span class='label'>Nautical Miles:</span></td><td valign='middle' align='right'>&nbsp;</td><td valign='middle' align='right' bgcolor='#F6CECE' class='display_none'>438,082&nbsp;</td><td valign='middle' align='right' bgcolor='#F6CECE' class='display_none'>&nbsp;</td></tr><tr><td colspan='4' align='center' class='header'><span class='label'><font size='-8'>N/R = Not Reported</font></span></td></tr><tr class='display_none'><td colspan='4' align='center' class='header'><font size='-9'>Flight Activity AFTT: JETNET has added a total of 12 minutes (6 minutes for both departure and arrival) as an industry standard to each flight time to compensate for startup, taxi, and takeoff.</font></td></tr><tr class='display_none'><td colspan='4' align='center' class='header'><font size='-9'>Flight Activity Since Last Verified:  Note that the red columns above represent data received from the FAA and reported to clients for their interpretation and use.  JETNET is not responsible for any errors or omissions in the summarization or presentation of flight activity data..</font></td></tr></table>"

    '      '  '  aircraft_usage_label.Text &= "</table>"


    '      'Else
    '      '  aircraft_usage_label.Text = (CommonAircraftFunctions.DisplayUsageInfo(AircraftTable, usage_tab))
    '      'End If


    '      'If bFromJFWAFW Then
    '      '  aircraft_contacts_label.Text = (CommonAircraftFunctions.GetCompanies_DisplayAircraftDetails(Me.Session, AircraftTable, False, False, Me.bExtraJFWAFW, AclsData_Temp, CRMSource))
    '      'Else
    '      '  aircraft_contacts_label.Text = (CommonAircraftFunctions.GetCompanies_DisplayAircraftDetails(Me.Session, AircraftTable, IIf(CRMView = False, True, False), False, False, AclsData_Temp, CRMSource))
    '      'End If

    '      'Build Aircraft Engine Information and display at the bottom of the page.
    '      ' engine_tab_label.Text = (CommonAircraftFunctions.DisplayEngineInfo_Vertical(Me.Session, AircraftTable, engine_tab, bShowBlankAcFields))

    '      'Build Propeller Information and Display at the bottom of the page.
    '      'propeller_tab_label.Text = (CommonAircraftFunctions.DisplayPropRotorInfo_Vertical(Me.Session, AircraftTable, propeller_tab_container, AclsData_Temp, bShowBlankAcFields))

    '      'Build Interior Table to fill the tab container with.
    '      'interior_tab_label.Text = (CommonAircraftFunctions.DisplayInteriorDetails(AclsData_Temp, Me.Session, AircraftTable, interior_tab, CRMSource))

    '      'Build Exterior Table to fill the tab container with.
    '      '  exterior_label.Text = (CommonAircraftFunctions.DisplayExteriorDetails(AclsData_Temp, Me.Session, AircraftTable, exterior_tab_panel, CRMSource))

    '      'If AircraftTable.Rows(0).Item("ac_lease_flag").ToString = "Y" Then
    '      '  'Build Lease Table to fill the tab container with.
    '      '  lease_tab_label.Text = (CommonAircraftFunctions.DisplayLeaseDetails(AircraftTable, AclsData_Temp))
    '      'End If
    '      'If lease_tab_label.Text = "" Then
    '      '  lease_tab_container.Visible = False
    '      'End If

    '      'Build Avionics Table to fill the tab container with.
    '      ' avionics_tab_label.Text = (CommonAircraftFunctions.DisplayAvionicsDetails(AclsData_Temp, Me.Session, AircraftTable, CRMSource))

    '      'Build Cockpit Table to fill the tab container with.
    '      ' cockpit_tab_label.Text = (CommonAircraftFunctions.DisplayCockpitDetails(AclsData_Temp, Me.Session, AircraftTable, CRMSource))
    '      'aircraft_flight_activity.Visible = False
    '      'aircraft_history.Visible = False
    '      'If Me.journalID = 0 Then

    '      '  If Not IsDBNull(AircraftTable.Rows(0).Item("ac_reg_nbr")) Then
    '      '    temp_reg = Trim(AircraftTable.Rows(0).Item("ac_reg_nbr"))
    '      '  Else
    '      '    temp_reg = ""
    '      '  End If


    '      '  If Trim(Session.Item("useFAAFlightData")) <> "" And Trim(Session.Item("useFAAFlightData")) <> "ARGUS" And HttpContext.Current.Session.Item("localPreferences").AerodexStandard = False Then

    '      '    '  If Not IsDBNull(AircraftTable.Rows(0).Item("ac_reg_nbr")) Then ' you need to check for "null" values

    '      '    ' if its on the blocked list, if not, check to see if there is any data 
    '      '    If flight_data_temp.IS_ON_BLOCKED_LIST(temp_reg) = True Then

    '      '      tmpFlightDataTable = flight_data_temp.getFAAFlightData(temp_reg, AircraftTable.Rows(0).Item("ac_id"), Nothing, Nothing, True)

    '      '      If Not IsNothing(tmpFlightDataTable) Then
    '      '        aircraft_flight_activity.Visible = True
    '      '        aircraft_flight_tab_label.Text = flight_data_temp.displayFAAFlightData(tmpFlightDataTable, aircraft_flight_tab.HeaderText, "last_year", "", AircraftTable.Rows(0).Item("ac_id"), DateAdd(DateInterval.Day, -90, Date.Now.Date), True, temp_reg, True)
    '      '        aircraft_flight_tab_label.Text &= "<table><tr><td align='left' colspan='5'><A href='FAAFlightData.aspx?acid=" & AircraftTable.Rows(0).Item("ac_id") & "' target='_blank'>View Flight Activity Map & Details</a></td></tr></table>"
    '      '        If InStr(aircraft_flight_tab_label.Text, "No Flight Activity") > 0 Then
    '      '        Else
    '      '          Me.view_ac_insight.Visible = True
    '      '          Me.li_start0.Visible = True
    '      '          Me.li_end0.Visible = True
    '      '          Me.util_link.Visible = True
    '      '          Me.util_link.Text = "<a href='FAAFlightData.aspx?acid=" & AircraftTable.Rows(0).Item("ac_id") & "&activetab=4' target='_blank'  class=""subMenuText"">Flight Activity</a>"

    '      '        End If
    '      '      End If

    '      '      ' if there is data in the clean table
    '      '    ElseIf flight_data_temp.checkForFAAFlightData(temp_reg, AircraftTable.Rows(0).Item("ac_id"), False, True) Or Trim(temp_reg) = "" Then

    '      '      tmpFlightDataTable = flight_data_temp.getFAAFlightData(temp_reg, AircraftTable.Rows(0).Item("ac_id"), Nothing, Nothing)

    '      '      If Not IsNothing(tmpFlightDataTable) Then
    '      '        aircraft_flight_activity.Visible = True
    '      '        aircraft_flight_tab_label.Text = flight_data_temp.displayFAAFlightData(tmpFlightDataTable, aircraft_flight_tab.HeaderText, "last_year", "", AircraftTable.Rows(0).Item("ac_id"), DateAdd(DateInterval.Day, -90, Date.Now.Date), True, temp_reg, False)
    '      '        If InStr(aircraft_flight_tab_label.Text, "No Flight Activity") > 0 Then
    '      '        Else
    '      '          Me.view_ac_insight.Visible = True
    '      '          Me.li_start0.Visible = True
    '      '          Me.li_end0.Visible = True
    '      '          Me.util_link.Visible = True
    '      '          Me.util_link.Text = "<a href='#' onclick=""javascript:load('FAAFlightData.aspx?acid=" & AircraftTable.Rows(0).Item("ac_id") & "&activetab=4','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');return false;"" class=""subMenuText"">Flight Activity</a>"
    '      '        End If

    '      '      End If


    '      '      '    End If

    '      '      'if there is no data in the clean table, and its not blocked, there shouldnt be any data
    '      '      If aircraft_flight_activity.Visible = False Then
    '      '        aircraft_flight_tab_label.Text = ("<table cellpadding=""3"" cellspacing=""0"" width=""99%"" border='1'>")
    '      '        aircraft_flight_tab_label.Text &= ("<tr><td valign=""middle"" align=""left""><strong>Date</strong></td>")
    '      '        aircraft_flight_tab_label.Text &= ("<td valign=""middle"" align=""left""><strong>Origin</strong></td>")
    '      '        aircraft_flight_tab_label.Text &= ("<td valign=""middle"" align=""left""><strong>Destination</strong></td>")
    '      '        aircraft_flight_tab_label.Text &= ("<td valign=""middle"" align=""left""><strong>Distance</strong><em>(sm)</em></td>")
    '      '        aircraft_flight_tab_label.Text &= ("<td valign=""middle"" align=""left"">")
    '      '        aircraft_flight_tab_label.Text &= ("<strong>Flight&nbsp;Time</strong><em>(min)</em></td></tr>") '</a> 
    '      '        aircraft_flight_tab_label.Text &= ("<tr><td valign=""middle"" align=""left"" colspan=""5"">No " & DisplayFunctions.ConvertToTitleCase(aircraft_flight_tab.HeaderText) & "</td></tr>")
    '      '        aircraft_flight_tab_label.Text &= ("</table>")
    '      '      End If

    '      '    End If

    '      '  ElseIf HttpContext.Current.Session.Item("localPreferences").AerodexStandard = False Then

    '      '    If Not IsDBNull(AircraftTable.Rows(0).Item("ac_reg_no_search")) Then ' you need to check for "null" values

    '      '      If CommonAircraftFunctions.checkForBarred_AC(AircraftTable.Rows(0).Item("ac_reg_no_search").ToString.Trim, AclsData_Temp) Then
    '      '        ' there is activity but the aircraft is on barr list
    '      '        aircraft_flight_activity.Visible = True
    '      '        aircraft_flight_tab_label.Text = ("<table><tr><th valign='middle' align='left' colspan='5'>Detailed flight data for this aircraft (REG#" & AircraftTable.Rows(0).Item("ac_reg_no_search").ToString.Trim & ") is not available for public viewing based on the request of the owner/operator.</th></tr>")
    '      '        aircraft_flight_tab_label.Text &= "</table>"
    '      '      Else
    '      '        If CommonAircraftFunctions.checkForFlightData(AircraftTable.Rows(0).Item("ac_reg_no_search").ToString.Trim, AclsData_Temp, False) Then
    '      '          If CommonAircraftFunctions.checkForFlightData(AircraftTable.Rows(0).Item("ac_reg_no_search").ToString.Trim, AclsData_Temp, True) Then
    '      '            'build table for flight data to fill tab container
    '      '            aircraft_flight_activity.Visible = True
    '      '            aircraft_flight_tab_label.Text = (CommonAircraftFunctions.DisplayFlightData(AircraftTable, CRMView, AclsData_Temp))
    '      '          Else
    '      '            ' there is activity but the aircraft is on barr list
    '      '            aircraft_flight_activity.Visible = True
    '      '            aircraft_flight_tab_label.Text = ("<table><tr><th valign='middle' align='left' colspan='5'>Detailed flight data for this aircraft (REG#" & AircraftTable.Rows(0).Item("ac_reg_no_search").ToString.Trim & ") is not available for public viewing based on the request of the owner/operator. </th></tr></table>")
    '      '          End If
    '      '        Else
    '      '          ' there is no flight activity
    '      '          aircraft_flight_activity.Visible = True
    '      '          aircraft_flight_tab_label.Text = ("<table><tr><th valign='middle' align='left' colspan='5'>No Flight Data Available for the last 90 days.</th></tr></table>")
    '      '        End If
    '      '      End If

    '      '    End If

    '      '  End If


    '      'If Not IsNothing(JournalTable) Then
    '      '  If JournalTable.Rows.Count > 0 Then
    '      '    aircraft_history.Visible = True
    '      '    aircraft_history_tab_label.Text = (CommonAircraftFunctions.DisplayAircraftHistory_BottomBlock(JournalTable, Me.Application, Me.Session, Me.bExtraJFWAFW, Me.aircraftID, securityTokenLocal, CRMView, AclsData_Temp, AircraftTable, CRMSource))
    '      '  End If
    '      'End If

    '      JournalTable = Nothing

    '      'Else
    '      '  Notes.Visible = False
    '      '  Reminders.Visible = False
    '      'End If ' Me.journalID = 0 

    '    End If


    '  Catch ex As Exception
    '    '  Master.LogError("DisplayAircraftDetail.aspx.vb - GenerateDisplayACDetails2 - " & ex.Message)
    '  End Try


    'End Sub


    ''' <summary>
    ''' Function ran to tell if we're toggling map on or off so js doesn't error.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ToggleMap()
        AportLat = IIf(IsNumeric(Latitude.Text), Latitude.Text, AportLat)
        AportLong = IIf(IsNumeric(Longitude.Text), Longitude.Text, AportLong)
        If RunMap = True Then
            'map_container.CssClass = "blue-theme"
            mapContainer.CssClass = ""
            DisplayFunctions.BuildJavascriptMap(Me.map_update_panel, Me.GetType, False, "map_canvas", 0, False, False) ' builds javascript script for part below
            System.Web.UI.ScriptManager.RegisterStartupScript(Me.map_update_panel, Me.GetType(), "Draw Map", "DrawMap(" & AportLat & "," & AportLong & ",'');", True)
        Else
            'map_container.CssClass = "display_none"
            mapContainer.CssClass = "display_none"
        End If
    End Sub

    Public Sub ViewOperatorHistory(ByVal sender As Object, ByVal e As System.EventArgs)
        ' ADDED IN MSW 
        If (Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO) Then
            If InStr(operator_history_panel.CssClass, "display_none") = 0 Then
                Toggle_Tabs_Visibility(False, False, False, False, False, False, False, False)
            Else
                Build_Operator_History(AircraftTable)
                Toggle_Tabs_Visibility(False, False, False, False, False, False, False, True)

            End If
            operator_history_update_panel.Update()
        End If

    End Sub
    ''' <summary>
    ''' Button click for the ac map. Runs js, changes class on buttons, runs toggle map.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Sub ViewAircraftMap(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles map_this_aircraft.Click
        If InStr(mapContainer.CssClass, "display_none") = 0 Then
            Toggle_Tabs_Visibility(False, False, False, False, False, False, False, False)
        Else
            Toggle_Tabs_Visibility(True, False, False, False, False, False, False, False)
        End If
        map_update_panel.Update()
    End Sub

    ''' <summary>
    ''' This toggles the buttons by basically ensuring that when one is opened, the rest are closed
    ''' as long as the function is called in the button click correctly.
    ''' </summary>
    ''' <param name="MapVis"></param>
    ''' <param name="AnalyticsVis"></param>
    ''' <param name="EventsVis"></param>
    ''' <param name="FoldersVis"></param>
    ''' <param name="NotesVis"></param>
    ''' <remarks></remarks>
    ''' 

    Private Sub Toggle_Tabs_Visibility(ByVal MapVis As Boolean, ByVal AnalyticsVis As Boolean, ByVal EventsVis As Boolean, ByVal FoldersVis As Boolean, ByVal NotesVis As Boolean, ByVal OwnershipVis As Boolean, ByVal prospectVis As Boolean, ByVal OperatorHistory As Boolean)

        If MapVis = False Then
            closeMap.Visible = False
            'map_this_aircraft.CssClass = "gray_button float_left"
            System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ToggleVis", "ToggleButtons('display_block');", True)
            'map_this_aircraft.Text = "<strong>Map</strong>"
            RunMap = False
            ToggleMap()
            map_update_panel.Update()
        Else
            closeMap.Visible = True
            'map_this_aircraft.Text = "<strong>Close Map</strong>"
            'map_this_aircraft.CssClass = "blue_button float_left"
            System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ToggleVis", "ToggleButtons('display_none');", True)
            RunMap = True
            ' aircraft_picture_slideshow.Visible = False
            ToggleMap()
            map_update_panel.Update()
        End If

        If NotesVis Then
            ' notesContainerItem.Visible = True
            notesContainerItem.CssClass = ""
            closeNotes.Visible = True
            'view_notes.Text = "<strong>Close Notes/Actions</strong>"
            notesPanel.Visible = True
            actionPanel.Visible = True
            'view_notes.CssClass = "blue_button float_left"
            'Notes.CssClass = "blue-theme"
            'Notes.Visible = True
            'Reminders.CssClass = "blue-theme"
            'Reminders.Visible = True
            notes_update_panel.Update()
        Else
            notesContainerItem.CssClass = "display_none"
            'notesContainerItem.Visible = False
            notesPanel.Visible = False
            actionPanel.Visible = False
            closeNotes.Visible = False
            'view_notes.CssClass = "gray_button float_left"
            'view_notes.Text = "<strong>Notes/Actions</strong>"
            'Notes.CssClass = "dark-theme"
            'Notes.Visible = False
            'Reminders.CssClass = "dark-theme"
            'Reminders.Visible = False
            notes_update_panel.Update()
        End If


        If OwnershipVis = False Then
            Me.ownership_link.Text = "Aircraft Ownership"
            'ownership_tabcontainer.CssClass = "dark-theme"
            ownership_panel.CssClass = "display_none"
            ' view_analytics.CssClass = "gray_button float_left" 
            'view_analytics.Text = "<strong>Analytics</strong>"
            'ownership_tabcontainer.Visible = False
            ownership_update_panel.Visible = False
            ownership_update_panel.Update()
        Else
            Me.ownership_link.Text = "Close Aircraft Ownership"
            ownership_panel.CssClass = ""
            'ownership_tabcontainer.CssClass = "dark-theme"
            ownership_label.Text = CommonAircraftFunctions.GetAircraft_Ownership(AclsData_Temp, aircraftID)
            'ownership_tabcontainer.Visible = True
            ownership_update_panel.Visible = True
            ownership_update_panel.Update()
        End If

        If AnalyticsVis = False Then
            Me.analytics_link.Text = "Analytics"
            'close analytics
            closeAnalytics.Visible = False
            ' view_analytics.CssClass = "gray_button float_left"
            'analytic_container.CssClass = "dark-theme"
            'view_analytics.Text = "<strong>Analytics</strong>"
            'analytic_container.Visible = False
            analyticContainer.CssClass = "display_none"
            analytic_update_panel.Update()
        Else
            ' analytic_container.CssClass = "dark-theme"
            analyticContainer.CssClass = ""
            Me.analytics_link.Text = "Close Analytics"
            DisplayAnalyticInformation()
        End If

        If EventsVis = False Then
            'Close Events.
            closeEvents.Visible = False
            ' view_aircraft_events.CssClass = "gray_button float_left"
            ' events_container.CssClass = "dark-theme"
            eventContainer.CssClass = "display_none"
            'view_aircraft_events.Text = "<strong>Events</strong>"
            System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ToggleVis", "ToggleButtons('display_block');", True)
            aircraft_picture_slideshow.Visible = True
            events_update_panel.Update()
        Else
            eventContainer.CssClass = ""
            closeEvents.Visible = True
            'view_aircraft_events.Text = "<strong>Close Events</strong>"
            System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ToggleVis", "ToggleButtons('display_none');", True)
            aircraft_picture_slideshow.Visible = False
            'view_aircraft_events.CssClass = "blue_button float_left"
            'events_container.CssClass = "blue-theme"
            'events_container.Visible = True
        End If

        If FoldersVis = False Then
            closeFolders.Visible = False
            'view_folders.CssClass = "gray_button float_left"
            ' folders_container.CssClass = "dark-theme"
            foldersContainer.CssClass = "display_none"
            'view_folders.Text = "<strong>Folders</strong>"
            'folders_container.Visible = False
            aircraft_picture_slideshow.Visible = True
        Else

            'Set Folders since they're opened.
            closeFolders.Visible = True
            'view_folders.Text = "<strong>Close Folders</strong>"
            aircraft_picture_slideshow.Visible = False
            'view_folders.CssClass = "blue_button float_left"
            foldersContainer.CssClass = ""
            'folders_container.CssClass = "blue-theme"
            'folders_container.Visible = True
        End If

        If prospectVis = False Then
            closeProspects.Visible = False
            prospectsContainer.CssClass = "display_none"
        Else
            closeProspects.Visible = True
            prospectsContainer.CssClass = ""
        End If

        If OperatorHistory = False Then
            operator_history_panel.CssClass = "display_none"
        Else
            operator_history_panel.CssClass = ""
        End If

        If Not Page.ClientScript.IsClientScriptBlockRegistered("masonryPost") Then
            System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "masonryPost", "loadMasonry();", True)
        End If

    End Sub

    ''' <summary>
    ''' Function to update browse button. Tried to make this as simple as possible. Sent a datatable. Only care about previous/next ac. 
    ''' Creates text link.
    ''' </summary>
    ''' <param name="dsBrowse"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function UpdateBrowseButtons(ByRef dsBrowse As DataTable) As Boolean
        Dim FilterTable As New DataTable
        Dim nTotalRecordCount As Long = 0
        Dim ACIDNext As Long = 0
        Dim ACIDPrev As Long = 0
        If Me.aircraftID > 0 Then
            ' we must be browsing records
            ' find the ac record to display 
            If Not IsNothing(dsBrowse) Then
                If dsBrowse.Rows.Count > 0 Then
                    Me.currentRecord = 1
                    If dsBrowse.Rows.Count > 1 Then
                        For a As Integer = 0 To dsBrowse.Rows.Count - 1

                            If CLng(dsBrowse.Rows(a).Item("ac_id").ToString.Trim) = Me.aircraftID Then

                                If a + 1 = dsBrowse.Rows.Count Then
                                ElseIf a + 1 <= dsBrowse.Rows.Count Then
                                    ACIDNext = dsBrowse.Rows(a + 1).Item("ac_id").ToString.Trim
                                End If

                                If a >= 1 Then
                                    currentRecord = a + 1
                                    ACIDPrev = dsBrowse.Rows(a - 1).Item("ac_id").ToString.Trim
                                End If

                                Exit For
                            End If
                        Next
                    End If
                Else
                    ' browseTable.Visible = False
                    PreviousACSwap.Visible = False
                    browse_label.Visible = False
                    NextACSwap.Visible = False
                End If

            End If

        End If

        If ACIDPrev > 0 Then
            PreviousACSwap.Text = "<a href=""#"" id=""previousAC"" type=""button"" value=""&#8249;"" onclick=""document.body.style.cursor = 'wait';ToggleVis();window.location.href='DisplayAircraftDetail.aspx?acid=" & ACIDPrev & "';RemoveVis();"" tooltip = ""Click to View the Previous Aircraft"">&#8249;</a>"

            ' PreviousACSwap.Text = "<input id=""previousAC"" type=""button"" value="" < Previous Aircraft"" onclick=""document.body.style.cursor = 'wait';ToggleVis();window.location.href='DisplayAircraftDetail.aspx?acid=" & ACIDPrev & "';"" tooltip = ""Click to View the Previous Aircraft"" />"
            PreviousACSwap.Visible = True
        Else
            PreviousACSwap.Visible = False
        End If

        currentRecLabel.Text = Me.currentRecord.ToString
        totalRecLabel.Text = dsBrowse.Rows.Count 'nTotalRecordCount.ToString

        If dsBrowse.Rows.Count = 1 Then
            browse_label.Visible = False
            browseTableTitle.Text = ""
            recordsOf.Visible = False
        ElseIf dsBrowse.Rows.Count = 0 Then
            recordsOf.Visible = False
        End If
        If ACIDNext > 0 Then
            NextACSwap.Text = "<a href=""#"" id=""nextAC"" onclick=""document.body.style.cursor = 'wait';ToggleVis();window.location.href='DisplayAircraftDetail.aspx?acid=" & ACIDNext & "';RemoveVis();"" value=""&#8250"" tooltip = ""Click to View the Next Aircraft"">&#8250</a>"
            'NextACSwap.Text = "<input id=""nextAC"" onclick=""document.body.style.cursor = 'wait';ToggleVis();window.location.href='DisplayAircraftDetail.aspx?acid=" & ACIDNext & "';"" type=""button"" value=""Next Aircraft > "" tooltip = ""Click to View the Next Aircraft"" />"
            NextACSwap.Visible = True
        Else
            NextACSwap.Visible = False
        End If

        If PreviousACSwap.Visible = False Then
            'If there isn't, add class noBefore to intelDrop. Make ac_help_text cssclass "gray_button float_left"
            'intelDrop.Attributes.Add("class", "noBefore gray_button")
            'Values_Drop.Attributes.Add("class", "noBefore gray_button")
            'ac_help_text.CssClass = "gray_button float_left"
        End If
        Return True

    End Function


    ''' <summary>
    ''' View event button click.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Sub ViewAircraftEvents(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles view_aircraft_events.Click
        If InStr(eventContainer.CssClass, "display_none") = 0 Then
            Toggle_Tabs_Visibility(False, False, False, False, False, False, False, False)
        Else
            Toggle_Tabs_Visibility(False, False, True, False, False, False, False, False)
            BuildEventsTable(sort, sortWay)
        End If
        events_update_panel.Update()
    End Sub

    Public Sub BuildEventsTable(ByVal sort As String, ByVal sortWay As String)


        Dim EventsTable As New DataTable
        Dim css_string As String = ""
        Dim sortString As String = ""

        If sort = "desc" Then
            sortString = " apev_subject "
        Else
            sortString = " apev_action_date "
        End If

        If sortWay = "asc" Then
            sortString += " asc "
        Else
            sortString += " desc "
            sortWay = "desc"
        End If

        If CRMSource = "CLIENT" Then
            EventsTable = AclsData_Temp.AC_Listing_Market_Search("", "", "", HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag, jetnet_aircraft_id.Text, "", "", sortString)
        Else
            EventsTable = AclsData_Temp.AC_Listing_Market_Search("", "", "", HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag, aircraftID, "", "", sortString)

        End If

        'New sort Way defined for link after used in market search:
        If sortWay = "desc" Then
            sortWay = "asc"
        Else
            sortWay = "desc"
        End If


        If Not IsNothing(EventsTable) Then
            If EventsTable.Rows.Count > 0 Then
                Dim PageOutURL As String = "DisplayAircraftDetail.aspx?"

                If aircraftID > 0 Then
                    PageOutURL += "acid=" & aircraftID
                End If

                If journalID > 0 Then
                    PageOutURL += "&jid=" & journalID
                End If


                events_label.Text = "<table width='100%' cellspacing='3' cellpadding='3' class='formatTable blue eventTable' height='300' style='overflow:auto'>"
                events_label.Text += "<tr class='header_row'>"
                events_label.Text += "<td align='left' valign='top'><a href=""" & PageOutURL & "&sort=act&sortWay=" & sortWay & """ class=""no_text_underline""><b class='label'>ACTIVITY DATE/TIME</b></a></td>"
                events_label.Text += "<td align='left' valign='top' width='300'><a href=""" & PageOutURL & "&sort=desc&sortWay=" & sortWay


                events_label.Text += """ class=""no_text_underline""><b class='label'>DESCRIPTION</b></a></td></tr>"

                For Each r As DataRow In EventsTable.Rows
                    If css_string = "alt_row" Then
                        css_string = ""
                    Else
                        css_string = "alt_row"
                    End If
                    events_label.Text += "<tr class='" & css_string & "'><td align='left' valign='top'>" & r("apev_action_date") & "</td>"
                    events_label.Text += "<td align='left' valign='top'>" & r("apev_subject")
                    If Not IsDBNull(r("apev_description")) Then
                        If Not String.IsNullOrEmpty(r("apev_description")) Then
                            events_label.Text += "[" & r("apev_description") & "]"
                        End If
                    End If
                    events_label.Text += "</td></tr>"
                Next
                events_label.Text += "</table>"
            End If
        End If
    End Sub

    'Public Sub BuildEventsTable()
    '  Dim EventDisplayTable As New Table
    '  Dim EventsTable As New DataTable
    '  Dim tr As New TableRow
    '  Dim td_left As New TableCell
    '  Dim td_right As New TableCell
    '  Dim link_left As New LinkButton
    '  Dim link_right As New LinkButton


    '  Dim css_string As String = ""
    '  EventsTable = AclsData_Temp.AC_Listing_Market_Search("", "", "", HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag, aircraftID, "", "")

    '  If Not IsNothing(EventsTable) Then
    '    If EventsTable.Rows.Count > 0 Then
    '      EventDisplayTable.CellSpacing = "3"
    '      EventDisplayTable.CellPadding = "3"
    '      EventDisplayTable.CssClass = "data_aircraft_grid"

    '      tr.CssClass = "header_row"

    '      link_left.Text = "<b class=""title"">ACTIVITY DATE/TIME</b>"
    '      link_left.CommandName = "date"
    '      link_left.CommandArgument = "desc"
    '      link_left.ID = "link_left"

    '      td_left.Controls.Add(link_left)

    '      td_left.VerticalAlign = VerticalAlign.Top
    '      td_left.HorizontalAlign = HorizontalAlign.Left

    '      tr.Controls.Add(td_left)
    '      link_right.ID = "link_right"

    '      link_right.Text = "<b class=""title"">DESCRIPTION</b>"
    '      link_right.CommandName = "date"
    '      link_right.CommandArgument = "desc"
    '      AddHandler link_right.Click, AddressOf BuildEventsTable

    '      td_right.Controls.Add(link_right)

    '      td_right.VerticalAlign = VerticalAlign.Top
    '      td_right.HorizontalAlign = HorizontalAlign.Left
    '      td_right.Width = Unit.Pixel(350)

    '      tr.Controls.Add(td_right)

    '      EventDisplayTable.Controls.Add(tr)

    '      For Each r As DataRow In EventsTable.Rows
    '        tr = New TableRow
    '        td_left = New TableCell
    '        td_right = New TableCell

    '        If css_string = "alt_row" Then
    '          css_string = ""
    '        Else
    '          css_string = "alt_row"
    '        End If

    '        tr.CssClass = css_string
    '        td_left.VerticalAlign = VerticalAlign.Top
    '        td_left.HorizontalAlign = HorizontalAlign.Left
    '        td_left.Text = r("apev_action_date")

    '        tr.Controls.Add(td_left)

    '        td_right.VerticalAlign = VerticalAlign.Top
    '        td_right.HorizontalAlign = HorizontalAlign.Left
    '        td_right.Text = r("apev_subject")

    '        If Not IsDBNull(r("apev_description")) Then
    '          If Not String.IsNullOrEmpty(r("apev_description")) Then
    '            td_right.Text += "[" & r("apev_description") & "]"
    '          End If
    '        End If

    '        tr.Controls.Add(td_right)

    '        EventDisplayTable.Controls.Add(tr)

    '      Next

    '      events_label.Controls.Add(EventDisplayTable)
    '    End If
    '  End If


    '  '   picture_update_panel.Update()
    'End Sub


    ''' <summary>
    ''' Checks to see if parent page is the ac listing page. This determines whether browse table is shown or not.
    ''' Could possibly be refined to work better and more intelligently based on what criteria we're looking at.
    ''' For now all we care about is this simple check.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function FromListing() As Boolean
        If InStr(UCase(parent_page_name.Text), parent_check_page_name.Text) > 0 And journalID = 0 Then
            Return True
        Else
            Return False
        End If

    End Function

    ''' <summary>
    ''' Button click to view folders.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Sub ViewAircraftFolders(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles view_folders.Click
        If InStr(foldersContainer.CssClass, "display_none") = 0 Then
            Toggle_Tabs_Visibility(False, False, False, False, False, False, False, False)
        Else
            Toggle_Tabs_Visibility(False, False, False, True, False, False, False, False)
        End If
        folders_update_panel.Update()
    End Sub


    ''' <summary>
    ''' This function is running to build the dynamic folder list to allow adding to static folders.
    ''' It's built dynamically in page init
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Build_Dynamic_Folder_Table()
        'Dim FoldersTable As New DataTable
        Dim ContainerTable As New Table
        Dim TR As New TableRow
        Dim TDHold As New TableCell
        Dim SubmitButton As New LinkButton


        ContainerTable = DisplayFunctions.CreateStaticFoldersTable(aircraftID, 0, journalID, 0, 0, AclsData_Temp, 0)
        TDHold = DisplayFunctions.BuildTableCell(False, "", VerticalAlign.Top, HorizontalAlign.Left)

        SubmitButton.Text = "Save Folders"
        SubmitButton.ID = "SaveStaticFoldersButton"
        AddHandler SubmitButton.Click, AddressOf SaveStaticFolders

        TDHold.Controls.Add(SubmitButton)
        TR.Controls.Add(TDHold)

        ContainerTable.Controls.Add(TR)

        folders_label.Controls.Clear()
        ContainerTable.CssClass = "formatTable blue"
        folders_label.Controls.Add(ContainerTable)

        folders_update_panel.Update()
    End Sub
    ''' <summary>
    ''' This function allows saving of static folders.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SaveStaticFolders()
        folders_label = clsGeneral.clsGeneral.SaveStaticFolders(folders_label, AclsData_Temp, aircraftID, 0, 0, 0, journalID, 0)
        folders_update_panel.Update()
    End Sub
    ''' <summary>
    ''' button click for view notes. 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Sub ViewAircraftNotes(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles view_notes.Click
        If InStr(notesContainerItem.CssClass, "display_none") = 0 Then
            Toggle_Tabs_Visibility(False, False, False, False, False, False, False, False)
            SetNoteStatusCookies("NoteCookieStatus", "False", HttpContext.Current.Session.Item("localUser").crmUserCompanyID.ToString & HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString & HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString)
        Else
            Toggle_Tabs_Visibility(False, False, False, False, True, False, False, False)
            SetNoteStatusCookies("NoteCookieStatus", "True", HttpContext.Current.Session.Item("localUser").crmUserCompanyID.ToString & HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString & HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString)
        End If
    End Sub

    Private Sub GetNoteStatusCookies(ByVal NoteCookieName As String, ByRef NoteCookieValue As String, ByVal UserID As String)
        NoteCookieName = NoteCookieName & "-" & UserID
        Dim _NoteCookies As HttpCookie = HttpContext.Current.Request.Cookies(NoteCookieName)

        If _NoteCookies IsNot Nothing Then
            NoteCookieValue = _NoteCookies("VALUE")
        Else
            NoteCookieValue = "True"
        End If
    End Sub
    Private Sub SetNoteStatusCookies(ByVal NoteCookieName As String, ByVal NoteCookieValue As String, ByVal UserID As String)
        NoteCookieName = NoteCookieName & "-" & UserID
        Dim _NoteCookies As HttpCookie = HttpContext.Current.Request.Cookies(NoteCookieName)

        If _NoteCookies IsNot Nothing Then
            HttpContext.Current.Response.Cookies(NoteCookieName).Values("VALUE") = NoteCookieValue
            HttpContext.Current.Response.Cookies(NoteCookieName).Expires = DateTime.Now.AddDays(10)
        Else
            Dim aCookie As New HttpCookie(NoteCookieName)
            aCookie.Values("VALUE") = NoteCookieValue
            aCookie.Values("USER") = UserID
            aCookie.Expires = DateTime.Now.AddDays(10)
            HttpContext.Current.Response.Cookies.Add(aCookie)
        End If

    End Sub

    Private Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete


        '  aircraft_information_label.Focus()
        If sort <> "" And sortWay <> "" Then
            Toggle_Tabs_Visibility(False, False, True, False, False, False, False, False)
            BuildEventsTable(sort, sortWay)
        End If

        'If PreviousACSwap.Visible = False And view_ac_insight.Visible = False Then
        'ac_help_text.CssClass = "gray_button float_left noBefore"
        'ElseIf journalID > 0 Then
        'intelDrop.Attributes.Add("class", "noBefore gray_button")
        'Values_Drop.Attributes.Add("class", "noBefore gray_button")
        'ac_help_text.CssClass = "gray_button float_left"
        'End If

        Dim ChartJavascript As String = ""
        ChartJavascript = "function loadMasonry() {" & vbNewLine
        ChartJavascript += "var grid = document.querySelector('.grid');" & vbNewLine
        ChartJavascript += "var msnry = new Masonry(grid, {" & vbNewLine
        ChartJavascript += "itemSelector: '.grid-item'," & vbNewLine
        ChartJavascript += "columnWidth: '.grid-item'," & vbNewLine
        ChartJavascript += "gutter: 10," & vbNewLine
        ChartJavascript += "horizontalOrder: true," & vbNewLine
        ChartJavascript += "percentPosition: true" & vbNewLine
        ChartJavascript += "});" & vbNewLine
        ChartJavascript += "}" & vbNewLine



        ChartJavascript += "jQuery(window).resize(function() {" & vbNewLine
        ChartJavascript += "if(this.resizeTO) clearTimeout(this.resizeTO);" & vbNewLine
        ChartJavascript += "this.resizeTO = setTimeout(function() {" & vbNewLine
        ChartJavascript += "jQuery(this).trigger('resizeEnd');" & vbNewLine
        ChartJavascript += "}, 1000);" & vbNewLine
        ChartJavascript += "});" & vbNewLine

        '//redraw graph when window resize is completed  
        ChartJavascript += "jQuery(window).on('resizeEnd', function() {" & vbNewLine
        ChartJavascript += "jQuery("".resizeChart"").empty();" & vbNewLine
        ChartJavascript += "if (typeof drawVisualization === ""function"") {" & vbNewLine

        ChartJavascript += "  drawVisualization();" & vbNewLine
        ChartJavascript += " } " & vbNewLine
        ChartJavascript += "if (typeof drawBarVisualization === ""function"") {" & vbNewLine

        ChartJavascript += "  drawBarVisualization();" & vbNewLine
        ChartJavascript += " } " & vbNewLine
        ChartJavascript += "});" & vbNewLine


        'ChartJavaScript = "function drawModuleCharts() {" & ChartJavaScript & "};" & vbNewLine

        System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StartupScript", ChartJavascript, True)

    End Sub

    Private Sub RunTellJetnetAboutChangesCode()
        Dim DoNotShowEmailJetnetChanges As Boolean = False
        Dim ToggleCookie As HttpCookie = Request.Cookies("tellAboutChanges")

        If Not IsNothing(ToggleCookie) Then
            If ToggleCookie.Value = "true" Then
                DoNotShowEmailJetnetChanges = True
            End If
        End If



        If Not Page.ClientScript.IsClientScriptBlockRegistered("popups") Then
            Dim modalScript As StringBuilder = New StringBuilder()
            Dim modalPostbackScript As StringBuilder = New StringBuilder()

            DisplayFunctions.BuildJavascriptTellJetnetAboutChanges(modalPostbackScript, modalScript, IIf(CRMSource = "CLIENT", jetnet_aircraft_id.Text, aircraftID), journalID, 0, TellJetnetAboutChanges, TellJetnetAboutChangesForm, includeJqueryTheme, notifyIframe)
            System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "popupsPost", modalPostbackScript.ToString, True)
            System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "popups", " jQuery(document).ready(function() {" & modalScript.ToString & ";});", True)
        End If
        If DoNotShowEmailJetnetChanges = True Then
            TellJetnetAboutChanges.Visible = False
        End If

    End Sub

    ''This code should all be self contained in this block, as well as the actual widget block is named TellJetnetAboutChanges
    ''When coming into the page, these 2 panels are VISIBLE = FALSE. That means that certain actions have to occur (the code down below) 
    ''To have it display. Plus the JQUERY (in the block down below) also needs to run (for the popup).

    ''As a quick description of panels:
    ''TellJetnetAboutChanges contains the actual little block on the page that's sticky and ever present.
    ''TellJetnetAboutChangesForm contains the IFRAME that the popup displays. This is always invisible unless you actually click the sticky block, in 
    ''which case it actually displays. But the jquery should handle visible/invisible, so nothing further would be needed there.
    'Private Sub RunTellJetnetAboutChangesCode()
    '  TellJetnetAboutChanges.Visible = True
    '  TellJetnetAboutChangesForm.Visible = True

    '  'We need to set up the Jquery here:


    '  'Let's go ahead and set the iframe to pass the correct ID
    '  notifyIframe.Attributes.Add("src", "Notify.aspx?acID=" & aircraftID & "&jID=" & journalID)
    '  If Not Page.ClientScript.IsClientScriptBlockRegistered("popups") Then
    '    Dim modalScript As StringBuilder = New StringBuilder()
    '    Dim modalPostbackScript As StringBuilder = New StringBuilder()

    '    'modalPostbackScript.Append(" jQuery(function(){")
    '    modalPostbackScript.Append("Sys.Application.add_load(function() {")

    '    modalPostbackScript.Append("jQuery(""#notifyJetnetDialog"").dialog({")
    '    modalPostbackScript.Append("autoOpen: false,")
    '    modalPostbackScript.Append("show: {")
    '    modalPostbackScript.Append("effect: ""fade"",")
    '    modalPostbackScript.Append("duration: 500")
    '    modalPostbackScript.Append("},")
    '    modalPostbackScript.Append("modal: true,")
    '    modalPostbackScript.Append("dialogClass: ""welcomeUserPopup"",")
    '    modalPostbackScript.Append("minHeight: 470,")
    '    modalPostbackScript.Append("resizable: false,")
    '    modalPostbackScript.Append("maxHeight: 470,")
    '    modalPostbackScript.Append("maxWidth: 490,")
    '    modalPostbackScript.Append("minWidth: 490,")
    '    modalPostbackScript.Append("draggable: false,")
    '    modalPostbackScript.Append("close: function( event, ui ) {")
    '    'We need to add a silly little item here. We're just going to go ahead and tell jquery that when the little blue box popup view link is clicked, we 
    '    'should have it refresh the src on the iframe. So in case they submit information and for some reason try to submit information again on the same aircraft, it will refresh.
    '    modalPostbackScript.Append("jQuery('#" & notifyIframe.ClientID & "').attr('src','Notify.aspx?acID=" & aircraftID & "&jID=" & journalID & "');")
    '    modalPostbackScript.Append("},")
    '    modalPostbackScript.Append("closeText:""X""")
    '    modalPostbackScript.Append("});")

    '    modalPostbackScript.Append("jQuery(""#closeTellJetnetChanges"").click(function() {")
    '    modalPostbackScript.Append("jQuery(""#TellJetnetChangesContainer"").css('display','none');")
    '    modalPostbackScript.Append("});")

    '    modalPostbackScript.Append("jQuery(""#tellJetnetAboutChangesLink"").click(function() {")
    '    modalPostbackScript.Append("jQuery(""#notifyJetnetDialog"").dialog(""open"");")
    '    modalPostbackScript.Append("});")
    '    'Add before final closing, not needed
    '    modalScript.Append(Replace(modalPostbackScript.ToString, "Sys.Application.add_load(function() {", ""))


    '    modalPostbackScript.Append("});")
    '    System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "popupsPost", modalPostbackScript.ToString, True)
    '    ' Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "popupsPost", modalPostbackScript.ToString, True)
    '    System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "popups", " jQuery(document).ready(function() {" & modalScript.ToString & ";});", True)
    '  End If

    'End Sub

#Region "To Be Moved to DL"
    Public Function GetAircraftAppraisal(ByVal acID As Long, ByVal subscriptionID As Long) As DataTable
        Dim sqlQuery As String = ""
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim atemptable As New DataTable

        Try
            If Not String.IsNullOrEmpty(Session.Item("jetnetAdminDatabase").ToString) Then
                'Opening Connection
                SqlConn.ConnectionString = Session.Item("jetnetAdminDatabase").ToString
                SqlConn.Open()

                sqlQuery = "SELECT * FROM Aircraft_Appraisal WITH(NOLOCK) "
                sqlQuery += " WHERE "

                ''Aircraft ID
                sqlQuery += " acappr_ac_id  = @acID  "

                ''Subscription ID
                sqlQuery += " and "
                sqlQuery += " acappr_subid  = @subscriptionID  "
                sqlQuery += " ORDER BY acappr_date"

                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>GetAircraftAppraisal(ByVal acID As Long, ByVal subscriptionID As Long) As DataTable</b><br />" & sqlQuery

                Dim SqlCommand As New SqlClient.SqlCommand(sqlQuery, SqlConn)
                SqlCommand.Parameters.AddWithValue("acID", acID)
                SqlCommand.Parameters.AddWithValue("subscriptionID", subscriptionID)


                SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

                Try
                    atemptable.Load(SqlReader)
                Catch constrExc As System.Data.ConstraintException
                    Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                End Try

                SqlCommand.Dispose()
                SqlCommand = Nothing
            End If
            GetAircraftAppraisal = atemptable
        Catch ex As Exception
            GetAircraftAppraisal = Nothing
            ' Me.class_error = "Error in GetAircraftAppraisal(ByVal acID As Long, ByVal subscriptionID As Long) As DataTable: " & ex.Message
        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing


        End Try

    End Function
#End Region

    ''' <summary>
    ''' This is a function that is going to toggle the notes to either view all/view less.
    ''' It works by checking to see what the notes_view_all.text is (the button you're clicking).
    ''' If it's View All, it shows all the notes. It will either poll the database for all the notes - or it will just display what's in the label Notes_view_all.
    ''' If that has text in it, it will just turn it on, otherwise it queries.
    ''' If the text isn't View All, (meaning it's View Less), it does basically the same thing based on the label Notes_Label.
    ''' Added in on 2/15/16
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub notes_view_all_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles notes_view_all.Click
        If notes_view_all.Text = "VIEW ALL" Then
            notes_label.Visible = False
            notes_all_label.Visible = True
            notesContainerItem.Visible = True
            If notes_all_label.Text = "" Then 'We're actually going to run this now. If it's already been ran, we're not. We'll just toggle it back on.
                'No reason to requery if it's already been done.
                DisplayFunctions.DisplayLocalItems(AclsData_Temp, aircraftID, 0, 0, notes_all_label, action_label, False, True, False, False, 0, False)
            End If
            notes_view_all.Text = "VIEW LESS"
        Else
            notesContainerItem.Visible = True
            notes_label.Visible = True
            notes_all_label.Visible = False
            If notes_label.Text = "" Then 'See notes above for the notes_all_label.text = "". Same thing applies.
                DisplayFunctions.DisplayLocalItems(AclsData_Temp, aircraftID, 0, 0, notes_label, action_label, False, True, False, True, 5, False)
            End If
            notes_view_all.Text = "VIEW ALL"
        End If

        If Not Page.ClientScript.IsClientScriptBlockRegistered("ResetCursor") Then
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me.notes_update_panel, Me.GetType(), "ResetCursor", "document.body.style.cursor='default';", True)
        End If

        If Not Page.ClientScript.IsClientScriptBlockRegistered("masonryPost") Then
            System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "masonryPost", "loadMasonry();", True)
        End If


        notes_update_panel.Update()
    End Sub

    Private Sub estimator_submit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles estimator_submit.Click

        Dim objWebService As New com.jetnet.aiservice.www.AssetInsightService

        Dim nAirframeHrs As Long = 0
        Dim nAirframeCyls As Long = 0
        Dim nAirframeAsOf As String = ""
        Dim nAirframeProgram As String = ""
        Dim extra_info As String = ""
        Dim is_valid_date As Boolean = False
        Dim text_to_submit As String = ""
        Dim email_string_text As String = ""
        Dim has_error As Boolean = False

        Try

            Dim returnValue As Decimal = 0
            Dim returnValue2 As Boolean = False

            email_string_text = "EVALUE SUBMITTAL: "
            text_to_submit = "EVALUE SUBMITTAL: "

            If Not String.IsNullOrEmpty(estimator_aftt.Text.Trim) Then
                If IsNumeric(estimator_aftt.Text) Then
                    nAirframeHrs = CLng(estimator_aftt.Text)
                    text_to_submit &= "<aftt>" & nAirframeHrs & "</aftt>"
                    email_string_text &= "<br/>AFTT: " & nAirframeHrs
                End If
            End If

            If Not String.IsNullOrEmpty(estimator_landings.Text.Trim) Then
                If IsNumeric(estimator_landings.Text) Then
                    nAirframeCyls = CLng(estimator_landings.Text)
                    text_to_submit &= "<landings>" & nAirframeCyls & "</landings>"
                    email_string_text &= "<br/>Landings: " & nAirframeCyls
                End If
            End If

            If Not String.IsNullOrEmpty(estimator_as_of_date.Text.Trim) Then
                If IsDate(estimator_as_of_date.Text) Then
                    nAirframeAsOf = CDate(estimator_as_of_date.Text)
                    text_to_submit &= "<as of date>" & nAirframeAsOf & "</as of date>"
                    email_string_text &= "<br/>As of Date: " & nAirframeAsOf
                    is_valid_date = True
                End If
            End If

            If Not String.IsNullOrEmpty(estimator_airframe_program.SelectedValue.Trim) Then
                nAirframeProgram = estimator_airframe_program.SelectedValue.Trim

                If Trim(nAirframeProgram) = "" Or Trim(nAirframeProgram) = "Unknown" Then
                    nAirframeProgram = ""
                ElseIf Trim(nAirframeProgram) = "Confirmed not on any maintenance program" Then
                    nAirframeProgram = ""
                ElseIf Trim(nAirframeProgram) = "Confirmed to be on an AAIP" Then
                    nAirframeProgram = "AAIP"
                ElseIf Trim(nAirframeProgram) = "Confirmed to be on a maintenance program" Then
                    nAirframeProgram = "Other"
                ElseIf Trim(nAirframeProgram) = "Confirmed to be on a Factory maintenance program" Then
                    nAirframeProgram = "OEM Coverage"
                End If
                text_to_submit &= "<airframe program>" & nAirframeProgram & "</airframe program>"
                email_string_text &= "<br/>Airframe Program: " & nAirframeProgram
            Else
                nAirframeProgram = ""
            End If


            extra_info = Trim(estimator_extra_info.Text)

            text_to_submit &= "<other>" & extra_info & "</other>"
            email_string_text &= "<br/>Other Information: " & extra_info & ""


            text_to_submit = Replace(text_to_submit, "'", "''")

            estimator_result.Text = ""
            has_error = False
            If Session.Item("localUser").crmLocalUserEmailAddress.ToString.ToLower.Contains("@jetnet.com") Or Session.Item("localUser").crmLocalUserEmailAddress.ToString.ToLower.Contains("@mvintech.com") Then
                ' dont send emails
            Else
                Try

                    Call send_formatted_email(email_string_text)

                    Call commonLogFunctions.Log_User_Event_Data("Submitted Data", text_to_submit, Nothing, 0, journalID, 0, 0, 0, aircraftID, 0, 0, "1/1/1900")

                Catch ex As Exception
                    estimator_result.Text = "<font color='red'>Error In Processing eValue Request</font>"
                    has_error = True
                End Try
            End If

            If Me.estimator_submit.Width = 199 Then
                ' this means we do not have an evalue
                estimator_result.Text &= "<font color=""" + value_color + """>No Instant Estimate Available</font>"

                estimator_post_text.Text = "<font color=""" + value_color + """>Thank you for your submittal.<br/><br/>JETNET's research team will be reviewing and responding to your inputs as soon as possible..</font>"
                estimator_post_text.Visible = True
            ElseIf has_error = False Then
                ' if we have a valid date and we have verified with the checkbox
                If is_valid_date = True And Me.estimator_verify.Checked = True Then
                    objWebService.RunAnalysis(aircraftID, True, nAirframeHrs, True, nAirframeCyls, True, nAirframeProgram, True, True, returnValue, returnValue2)
                    estimator_result.Text &= "<font color=""" + value_color + """>Adjusted eValue : " + FormatCurrency(IIf(returnValue > 0, (returnValue / 1000), 0), 0, TriState.False, TriState.False, TriState.True) + "k</font>"

                    estimator_post_text.Text = "<font color=""" + value_color + """>Thank you for your submittal.<br/><br/>JETNET's research team will be reviewing and responding to your inputs as soon as possible.<br/><br/>The adjusted eValue is a estimate for your use only and does not reflect the Other Value Related Changes entered.</font>"
                    estimator_post_text.Visible = True
                ElseIf is_valid_date = False Then
                    estimator_result.Text &= "<font color='red'>Please Enter a Valid Date</font>"
                    estimator_post_text.Visible = False
                ElseIf Me.estimator_verify.Checked = False Then
                    estimator_result.Text &= "<font color='red'>Please Check the Verified Information Checkbox</font>"
                    estimator_post_text.Visible = False
                End If
            End If

        Catch ex As Exception
            estimator_post_text.Text = "<font color=""" + value_color + """>Thank you for your submittal.<br/>JETNET's research team will be reviewing and responding to your inputs as soon as possible.</font>"
            estimator_post_text.Visible = True
        End Try

    End Sub

    Public Sub send_formatted_email(ByVal email_string As String)

        ' if email text passes client validation, scrub input text for common script injection phrases (SQL and HTML)
        Dim tmpEmailString = commonEvo.scrubEmailString(email_string)
        Dim EmailString As New StringBuilder

        'Let's build the EMAIL
        EmailString.Append("<html><head>")
        EmailString.Append("<title>Evolution JETNET Customer Subscription Info</title>")
        EmailString.Append("</head><body>")
        EmailString.Append("<img src=" & clsData_Manager_SQL.get_site_name & "/images/JN_EvolutionMarketplace_Logo2.png""><br /><br />")
        EmailString.Append("<font face=""Arial"" size=""3"">" & FormatDateTime(Now, vbGeneralDate).ToString & "<br /><br />")
        EmailString.Append("JETNET LLC<br />Utica, NY  United States<br /><br />")
        EmailString.Append("Customer " & Session.Item("localUser").crmLocalUserFirstName.ToString.Trim & " " & Session.Item("localUser").crmLocalUserLastName.ToString.Trim & " has requested Information<br /><br />")
        EmailString.Append("<table border=""1"" cellspacing=""0"" cellpadding=""2"">")
        EmailString.Append("<tr><th align=""left"">User ID : </th><th align=""right"">" & Session.Item("localUser").crmUserLogin.ToString.Trim & "</th></tr>")
        EmailString.Append("<tr><th align=""left"">Subscription ID : </th><th align=""right"">" & Session.Item("localUser").crmSubSubID.ToString.Trim & "</th></tr>")
        EmailString.Append("<tr><th align=""left"">Install Seq No : </th><th align=""right"">" & Session.Item("localUser").crmSubSeqNo.ToString.Trim & "</th></tr>")
        EmailString.Append("<tr><th align=""left"">EMail Address : </th><th align=""right"">" & Session.Item("localUser").crmLocalUserName.ToString.Trim & "</th></tr>")
        EmailString.Append("<tr><th align=""left"">Company ID : </th><th align=""right"">" & Session.Item("localUser").crmUserCompanyID.ToString.Trim & "</th></tr>")
        EmailString.Append("<tr><th align=""left"">Contact ID : </th><th align=""right"">" & Session.Item("localUser").crmUserContactID.ToString.Trim & "</th></tr>")


        EmailString.Append("<tr><th align=""left"">Submitted Information : </th><th align=""right"">" & email_string.ToString.Trim & "</th></tr>")


        'Select Case (Session.Item("localPreferences").Tierlevel)
        '  Case eTierLevelTypes.JETS
        '    EmailString.Append("<tr><th align=""left"">Tier level : </th><th align=""right"">Jets</th></tr>")
        '  Case eTierLevelTypes.TURBOS
        '    EmailString.Append("<tr><th align=""left"">Tier level : </th><th align=""right"">Turbos</th></tr>")
        '  Case Else
        '    EmailString.Append("<tr><th align=""left"">Tier level : </th><th align=""right"">All</th></tr>")
        'End Select
        '' EmailString.Append("<tr><th align=""left"">Platform OS : </th><th align=""right"">" & subscription_platform.Text & "</th></tr>")
        'EmailString.Append("<tr><th align=""left"">Service Code : </th><th align=""right"">" & Session.Item("localPreferences").ServiceCode.trim & " : " & Session.Item("localPreferences").ServiceName.ToString & "</th></tr>")

        'EmailString.Append("<tr><th align=""left"">Aerodex : </th><th align=""right"">" & Session.Item("localPreferences").AerodexFlag.ToString & "</th></tr>")
        'EmailString.Append("<tr><th align=""left"">Business : </th><th align=""right"">" & Session.Item("localPreferences").UserBusinessFlag.ToString & "</th></tr>")
        'EmailString.Append("<tr><th align=""left"">Commericial : </th><th align=""right"">" & Session.Item("localPreferences").UserCommercialFlag.ToString & "</th></tr>")
        'EmailString.Append("<tr><th align=""left"">Helicopter : </th><th align=""right"">" & Session.Item("localPreferences").UserHelicopterFlag.ToString & "</th></tr>")
        'EmailString.Append("<tr><th align=""left"">SPI View : </th><th align=""right"">" & Session.Item("localPreferences").UserSPIViewFlag.ToString & "</th></tr>")
        'EmailString.Append("<tr><th align=""left"">STAR reports : </th><th align=""right"">" & Session.Item("localPreferences").UserStarRptFlag.ToString & "</th></tr>")
        'EmailString.Append("<tr><th align=""left"">Yacht : </th><th align=""right"">" & Session.Item("localPreferences").UserYachtFlag.ToString & "</th></tr>")

        'EmailString.Append("<tr><th align=""left"">Mobile web : </th><th align=""right"">" & Session.Item("localPreferences").MobleWebStatus.ToString & "</th></tr>")
        'EmailString.Append("<tr><th align=""left"">Mobile Number : </th><th align=""right"">" & Session.Item("localPreferences").SmsPhoneNumber.ToString & "</th></tr>")

        'If (CBool(My.Settings.enableChat)) Then

        '  If bEnableChat Then
        '    EmailString.Append("<tr><th align=""left"">Chat Enabled : </th><th align=""right"">" & Session.Item("localPreferences").ChatEnabled.ToString & "</th></tr>")
        '  End If

        'End If

        'Dim sProjectText As String = ""
        'Dim bHasDefaultProject = commonEvo.CheckForProject(sProjectText)

        'If Not String.IsNullOrEmpty(sProjectText) Then
        '  EmailString.Append("<tr><th align=""left"">Project : </th><th align=""right"">" & sProjectText & "</th></tr>")
        'Else
        '  EmailString.Append("<tr><th align=""left"">Project : </th><th align=""right"">No Default Project</th></tr>")
        'End If

        '  EmailString.Append("<tr><th align=""left"">Default Model : </th><th align=""right"">" & display_default_modelID.Text & "</th></tr>")
        '  EmailString.Append("<tr><th align=""left"">Default View : </th><th align=""right"">" & display_default_viewID.Text & "</th></tr>")

        'EmailString.Append("<tr><th align=""left"">Server Notes : </th><th align=""right"">" & Session.Item("localPreferences").HasServerNotes.ToString & "</th></tr>")
        'EmailString.Append("<tr><th align=""left"">Cloud Notes : </th><th align=""right"">" & Session.Item("localPreferences").HasCloudNotes.ToString & "</th></tr>")
        'EmailString.Append("<tr><th align=""left"">Marketing Account : </th><th align=""right"">" & Session.Item("localPreferences").MarketingFlag.ToString & "</th></tr>")
        'EmailString.Append("<tr><th align=""left"">Demo Account : </th><th align=""right"">" & Session.Item("localPreferences").DemoFlag.ToString & "</th></tr>")
        'EmailString.Append("<tr><th align=""left"">Default Email Format : </th><th align=""right"">" & Session.Item("localPreferences").UserEmailDefaultFormat.ToString.Trim & "</th></tr>")
        'EmailString.Append("<tr><th align=""left"">Default Reply Name : </th><th align=""right"">" & Session.Item("localPreferences").UserEmailReplyToName.ToString.Trim & "</th></tr>")
        'EmailString.Append("<tr><th align=""left"">Default Reply Email : </th><th align=""right"">" & Session.Item("localPreferences").UserEmailReplyToAddress.ToString.Trim & "</th></tr>")
        'EmailString.Append("<tr><th align=""left"">Show Listings on JETNET Global : </th><th align=""right"">" & Session.Item("localPreferences").ShowListingsOnGlobal.ToString & "</th></tr>")

        '   EmailString.Append("<tr><th align=""left"">SMS Text Msg Active : </th><th align=""right"">" & myservices_SMS_service_status.Text & "</th></tr>")

        '  Dim sModelsOut As String = ""
        '   commonEvo.fillMakeModelDropDown(Nothing, Nothing, 0, sModelsOut, sSMSSelectedModelID, False, False, True, False, False, True) ' display models
        '   EmailString.Append("<tr><th align=""left"">SMS Models : </th><th align=""right"">" & sModelsOut & "</th></tr>")

        '  EmailString.Append("<tr><th align=""left"">SMS Provider : </th><th align=""right"">" & Session.Item("localPreferences").SmsProviderName.ToString.Trim & "</th></tr>")

        '  Dim sEventsOut As String = ""
        '  localDataLayer.fillSMSEventsDropDown(Nothing, 0, sEventsOut, sSMSSelectedEvents, True)

        '   EmailString.Append("<tr><th align=""left"">SMS Events : </th><th align=""right"">" & sEventsOut & "</th></tr>")

        '   EmailString.Append("<tr><th align=""left"">User Email Request : </th><th align=""right"">" & tmpEmailString.ToLower.Trim & "</th></tr>")



        EmailString.Append("</table>")
        EmailString.Append("</body></html>")

        AclsData_Temp.InsertMailQueue(Session.Item("localUser").crmUserCompanyID, Session.Item("localUser").crmUserContactID, Session.Item("localUser").crmSubSubID, "lee@jetnet.com", EmailString.ToString)



    End Sub

    Private Sub contactUpdateButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles contactUpdateButton.Click

        AircraftTable = CommonAircraftFunctions.BuildReusableTable(aircraftID, journalID, CRMSource, "", Master.aclsData_Temp, CRMView, 0, transactionSource)

        If bFromJFWAFW Then
            aircraft_contacts_label.Text = (CommonAircraftFunctions.GetCompanies_DisplayAircraftDetails(Me.Session, AircraftTable, False, False, Me.bExtraJFWAFW, AclsData_Temp, CRMSource, journalID, 0, aircraftID, False, True))
        Else
            aircraft_contacts_label.Text = (CommonAircraftFunctions.GetCompanies_DisplayAircraftDetails(Me.Session, AircraftTable, IIf(CRMView = True And HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CRM, False, True), False, False, AclsData_Temp, CRMSource, journalID, 0, aircraftID, False, True))
        End If
        contactUpdatePanel.Update()

    End Sub


    Private Sub ContactUpdateCurrent_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ContactUpdateCurrent.Click
        AircraftTable = CommonAircraftFunctions.BuildReusableTable(aircraftID, journalID, CRMSource, "", Master.aclsData_Temp, CRMView, 0, transactionSource)

        If bFromJFWAFW Then
            aircraft_contacts_label.Text = (CommonAircraftFunctions.GetCompanies_DisplayAircraftDetails(Me.Session, AircraftTable, False, False, Me.bExtraJFWAFW, AclsData_Temp, CRMSource, journalID, 0, aircraftID, False, False))
        Else
            aircraft_contacts_label.Text = (CommonAircraftFunctions.GetCompanies_DisplayAircraftDetails(Me.Session, AircraftTable, IIf(CRMView = True And HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CRM, False, True), False, False, AclsData_Temp, CRMSource, journalID, 0, aircraftID, False, False))
        End If
        contactUpdatePanel.Update()

    End Sub

    Private Sub cancel_update_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cancel_update.Click

        Response.Redirect("DisplayAircraftDetail.aspx?acid=" & Trim(Request("acid")))

    End Sub

End Class