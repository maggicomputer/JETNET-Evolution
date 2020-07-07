' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/userPortfolio.aspx.vb $
'$$Author: Amanda $
'$$Date: 5/27/20 4:21p $
'$$Modtime: 5/27/20 4:18p $
'$$Revision: 36 $
'$$Workfile: userPortfolio.aspx.vb $
'
' ********************************************************************************

Partial Public Class userPortfolio

    Inherits System.Web.UI.Page


    Private localCriteria As New viewSelectionCriteriaClass
    Public Shared masterPage As New Object
    Public isDisplayEvalues As Boolean = False
    Dim companyID As Long = 0
    Dim use_insight_roll As String = "N"
    Dim use_insight_op As String = "N"
    Dim use_insight_own_op As String = "N"
    Dim use_insight_brokered As String = "N"
    Dim use_insight_managed As String = "N"
    Dim show_type As String = ""
    Dim searchCriteria As New viewSelectionCriteriaClass
    Dim comp_id_list As String = ""
    Dim util_functions As New utilization_functions
    Dim AllowExport As Boolean = True
    Dim ForceUncheckNotes As Boolean = False
    Private Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        Try

            If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
                Me.MasterPageFile = "~/EvoStyles/EmptyCustomerAdminTheme.master"
                masterPage = DirectCast(Page.Master, EmptyCustomerAdminTheme)
            ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO Then
                Me.MasterPageFile = "~/EvoStyles/EmptyEvoTheme.master"
                masterPage = DirectCast(Page.Master, EmptyEvoTheme)
                masterPage.RemoveLine()
            ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CRM Then
                Me.MasterPageFile = "~/EvoStyles/EmptyEvoTheme.master"
                masterPage = DirectCast(Page.Master, EmptyEvoTheme)
            End If

        Catch ex As Exception
            If Not IsNothing(masterPage) Then
                masterPage.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString & " (Page_PreInit): " & ex.Message.ToString)
            Else
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += " (Page_PreInit): " + ex.Message.ToString.Trim
            End If
        End Try

    End Sub




    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim ac_list As String = ""
        Dim ac_list_temp As String = ""
        Dim previousFolder As Long = 0
        Dim PageNeedUpdate As Boolean = False
        Dim results_table As New DataTable
        Dim comp_name As String = ""

        Try
            'Default
            Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("Fleet Analyzer")
            masterPage.SetPageTitle("Fleet Analyzer")

            If clsGeneral.clsGeneral.isEValuesAvailable() = True And clsGeneral.clsGeneral.isShowingEvalues() = True Then
                isDisplayEvalues = True
            End If
            If Not String.IsNullOrEmpty(user_portfolio_list.SelectedValue.Trim) Then
                localCriteria.ViewCriteriaFolderID = CLng(user_portfolio_list.SelectedValue)
                localCriteria.ViewCriteriaFolderName = user_portfolio_list.SelectedItem.Text
            End If


            masterPage.SetContainerClass("container MaxWidthRemove")

            Dim portfolio_functions As New userPortfolioDataLayer
            portfolio_functions.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
            portfolio_functions.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
            portfolio_functions.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
            portfolio_functions.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
            portfolio_functions.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim

            Dim util_functions As New utilization_functions
            util_functions.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
            util_functions.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
            util_functions.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
            util_functions.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
            util_functions.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim

            'added MSW - 2/23/20
            If HttpContext.Current.Session.Item("localPreferences").AerodexFlag = True Then
                portfolio_tabPanel1_Label1.Visible = False
            Else
                portfolio_tabPanel1_Label1.Visible = True
            End If




            If Not String.IsNullOrEmpty(localCriteria.ViewCriteriaFolderName.Trim) Then
                'breadcrumbs1.Text = "<strong>" + localCriteria.ViewCriteriaFolderName.Trim + " Portfolio</strong>"
                masterPage.SetPageTitle(localCriteria.ViewCriteriaFolderName.Trim + " Analysis")
                breadcrumbs1.Text = ""
                ' 
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me.searchUpdate, Me.GetType(), "UpdateText", "$('.logo_text_title').text('" & localCriteria.ViewCriteriaFolderName.Trim & " Analysis');", True)


            End If

            If Not Page.IsPostBack Then
                If Trim(Request("comp_id")) <> "" Then
                Else
                    Dim HideShared As String = ""
                    If Not IsNothing(Trim(Request("Shared"))) Then
                        If Trim(Request("Shared")) = "True" Then
                            HideShared = "N"
                            hideSharedCheck.Attributes.Add("checked", "True")
                        End If
                    End If
                    portfolio_functions.get_folder_list(localCriteria, 0, user_portfolio_list, HideShared)
                End If

                user_portfolio_list.Items.Add(New ListItem("Selected Aircraft", 1))
            End If

            PanelCollapseEx1.Collapsed = False
            PanelCollapseEx1.ClientState = "False"
            portfolio_view_results_div.Attributes.Remove("class")
            portfolio_tabContainer.CssClass = "display_none dark-theme"
            attention.Text = ""

            If user_portfolio_list.Items.Count = 1 Then
                startingText.InnerText = "You currently have no Portfolio's to select. The term Portfolio is just another name for an Aircraft Folder. Therefore, to create a portfolio, go to the aircraft search and create a folder (Portfolio) with the aircraft you desire to review and then return to this view."
            End If
            SetUpLinksInHeader()

            If Not IsNothing(Trim(Request("use_insight_roll"))) Then
                If Trim(Request("use_insight_roll")) = "Y" Then
                    use_insight_roll = "Y"
                End If
            End If

            If Trim(Request("show_type")) <> "" Then
                If Trim(Request("show_type")) = "operated" Then
                    use_insight_op = "Y"
                ElseIf Trim(Request("show_type")) = "own_operated" Then
                    use_insight_own_op = "Y"
                ElseIf Trim(Request("show_type")) = "brokered" Then
                    use_insight_brokered = "Y"
                ElseIf Trim(Request("show_type")) = "managed" Then
                    use_insight_managed = "Y"
                Else
                    use_insight_own_op = "Y"
                End If

                show_type = Trim(Request("show_type"))
            Else
                show_type = "own_operated"
                use_insight_own_op = "Y"
            End If


            ' if you are coming in for the first time 



            If Not IsNothing(Trim(Request("comp_id"))) Then
                If Not String.IsNullOrEmpty(Trim(Request("comp_id"))) Then
                    If IsNumeric(Trim(Request("comp_id"))) Then


                        companyIDText.Text = Trim(Request("comp_id"))

                        If use_insight_roll = "Y" Then
                            searchCriteria.ViewCriteriaAmodID = 0
                            searchCriteria.ViewCriteriaCompanyID = companyIDText.Text
                            searchCriteria.ViewCriteriaTimeSpan = 6
                            searchCriteria.ViewCriteriaCompanyID = companyIDText.Text
                            results_table = util_functions.util_get_opearators_rollup(searchCriteria, 0)

                            If Not IsNothing(results_table) Then
                                For Each r As DataRow In results_table.Rows
                                    If Not IsDBNull(r.Item("comp_id")) Then
                                        If Trim(comp_id_list) <> "" Then
                                            comp_id_list &= ", "
                                        End If

                                        comp_id_list &= r.Item("comp_id")
                                    End If
                                Next
                            End If
                        End If


                        ' ------------ IF SHOW TYPE IS BLANK --- TRY TO SEE IF THEY HAVE ANY -----------------------------
                        If Trim(Request("show_type")) = "" Then

                            If companyIDText.Text > 0 Then
                                localCriteria.ViewCriteriaFolderID = 11111
                            End If

                            ' set these back to blank for now 
                            show_type = ""
                            use_insight_own_op = "N"

                            ac_list_temp = portfolio_functions.returnStaticAircraftList(localCriteria.ViewCriteriaFolderID, companyIDText.Text, comp_id_list, "own_operated")

                            If Trim(ac_list_temp) = "" Then
                                ac_list_temp = portfolio_functions.returnStaticAircraftList(localCriteria.ViewCriteriaFolderID, companyIDText.Text, comp_id_list, "operated")
                                If Trim(ac_list_temp) = "" Then
                                    ac_list_temp = portfolio_functions.returnStaticAircraftList(localCriteria.ViewCriteriaFolderID, companyIDText.Text, comp_id_list, "brokered")

                                    If Trim(ac_list_temp) = "" Then
                                        ac_list_temp = portfolio_functions.returnStaticAircraftList(localCriteria.ViewCriteriaFolderID, companyIDText.Text, comp_id_list, "managed")

                                        If Trim(ac_list_temp) = "" Then
                                            show_type = "managed"
                                            use_insight_managed = "Y"
                                        Else
                                            show_type = "own_operated"
                                            use_insight_own_op = "Y"
                                            'default and they wont have anything
                                        End If
                                    Else
                                        show_type = "brokered"
                                        use_insight_brokered = "Y"
                                    End If
                                Else
                                    use_insight_op = "Y"
                                    show_type = "operated"
                                End If
                            Else
                                show_type = "own_operated"
                                use_insight_own_op = "Y"
                            End If
                        End If
                        ' ------------ IF SHOW TYPE IS BLANK --- TRY TO SEE IF THEY HAVE ANY -----------------------------








                        If Not Page.IsPostBack Then

                            System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType, "autoLoadReport", "$(document).ready(function () {SelectDropDownItem('11');});" + vbCrLf, True)
                            comp_name = commonEvo.get_company_name_fromID(companyIDText.Text, 0, True, False, "")






                            If use_insight_op = "Y" Then
                                If use_insight_roll = "Y" Then
                                    breadcrumbs1.Text = "<strong>" & comp_name & " Operated Aircraft Portfolio (All Locations)</strong>"
                                Else
                                    breadcrumbs1.Text = "<strong>" & comp_name & " Operated Aircraft Portfolio</strong>"
                                End If
                            End If

                            If use_insight_own_op = "Y" Then
                                If use_insight_roll = "Y" Then
                                    breadcrumbs1.Text = "<strong>" & comp_name & " Owned and Operated Aircraft Portfolio (All Locations)</strong>"
                                Else
                                    breadcrumbs1.Text = "<strong>" & comp_name & " Owned and Operated Aircraft Portfolio</strong>"
                                End If
                            End If

                            If use_insight_brokered = "Y" Then
                                If use_insight_roll = "Y" Then
                                    breadcrumbs1.Text = "<strong>" & comp_name & " Brokered Aircraft Portfolio (All Locations)</strong>"
                                Else
                                    breadcrumbs1.Text = "<strong>" & comp_name & " Brokered Aircraft Portfolio</strong>"
                                End If
                            End If

                            If use_insight_managed = "Y" Then
                                If use_insight_roll = "Y" Then
                                    breadcrumbs1.Text = "<strong>" & comp_name & " Managed Aircraft Portfolio (All Locations)</strong>"
                                Else
                                    breadcrumbs1.Text = "<strong>" & comp_name & " Managed Aircraft Portfolio</strong>"
                                End If
                            End If

                        End If
                    End If
                End If
            End If


            If Not IsNothing(Trim(Request("REPORT_ID"))) Then
                If Not String.IsNullOrEmpty(Trim(Request("REPORT_ID"))) Then
                    If IsNumeric(Trim(Request("REPORT_ID"))) Then
                        System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType, "autoLoadReport", "$(document).ready(function () {SelectDropDownItem('" & Request("REPORT_ID") & "');});" + vbCrLf, True)
                    End If
                End If
            End If

            ' MAKE SURE YOU DO NOT SHOW FOR AERODEX 
            If HttpContext.Current.Session.Item("localPreferences").AerodexFlag = True Then
                portfolio_tabPanel1.Visible = False
            Else
                portfolio_tabPanel1.Visible = True
            End If

            ' if aerodex eleite or not aerodex, then show flight activity
            If HttpContext.Current.Session.Item("localPreferences").AerodexElite = True Or HttpContext.Current.Session.Item("localPreferences").AerodexFlag = False Then
                portfolio_tabPanel4.Visible = True
            Else
                portfolio_tabPanel4.Visible = False
            End If


            If Not Page.IsPostBack Then
                Call commonLogFunctions.Log_User_Event_Data("UserDisplayView", "User Entered View " & Replace(commonEvo.Get_Default_User_View(30), "&nbsp;", " "), Nothing, 30, localCriteria.ViewCriteriaJournalID, 0, localCriteria.ViewCriteriaAircraftID, 0, localCriteria.ViewCriteriaAircraftID, localCriteria.ViewCriteriaAmodID)
            End If



            If IsPostBack Then

                'Important:
                If IsNumeric(companyIDText.Text) Then
                    If companyIDText.Text > 0 Then
                        companyID = companyIDText.Text
                    End If
                End If


                headerUpdate.Update()
                PanelCollapseEx1.Collapsed = True
                portfolio_view_results_div.Attributes.Remove("class")
                PanelCollapseEx1.ClientState = "True"
                portfolio_tabContainer.Visible = True
                If Not IsNothing(HttpContext.Current.Session.Item("portfolioFolder")) Then
                    If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("portfolioFolder").ToString.Trim) Then
                        If IsNumeric(HttpContext.Current.Session.Item("portfolioFolder").ToString.Trim) Then
                            previousFolder = CLng(HttpContext.Current.Session.Item("portfolioFolder").ToString.Trim)
                        End If
                    End If
                End If

                If Not IsNothing(HttpContext.Current.Session.Item("portfolioAircraft")) Then
                    If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("portfolioAircraft").ToString.Trim) Then
                        ac_list = HttpContext.Current.Session.Item("portfolioAircraft").ToString.Trim
                    End If
                End If





                If companyID > 0 Then
                    localCriteria.ViewCriteriaFolderID = 11111
                End If

                If (localCriteria.ViewCriteriaFolderID > 0) Then

                    If (previousFolder = 0) Or (previousFolder <> localCriteria.ViewCriteriaFolderID) Or (portfolio_tabContainer.ActiveTab.ID = "portfolio_tabPanel2") Or (portfolio_tabContainer.ActiveTab.ID = "portfolio_tabPanel9") Then

                        Select Case localCriteria.ViewCriteriaFolderID

                            Case 99999, 88888, 77777, 66666, 11111

                                If companyID > 0 Then

                                    If use_insight_roll = "Y" Then
                                        searchCriteria.ViewCriteriaAmodID = 0
                                        searchCriteria.ViewCriteriaCompanyID = companyID
                                        searchCriteria.ViewCriteriaTimeSpan = 6
                                        searchCriteria.ViewCriteriaCompanyID = companyID
                                        results_table = util_functions.util_get_opearators_rollup(searchCriteria, 0)

                                        If Not IsNothing(results_table) Then
                                            For Each r As DataRow In results_table.Rows
                                                If Not IsDBNull(r.Item("comp_id")) Then
                                                    If Trim(comp_id_list) <> "" Then
                                                        comp_id_list &= ", "
                                                    End If

                                                    comp_id_list &= r.Item("comp_id")
                                                End If
                                            Next
                                        End If

                                        ac_list = portfolio_functions.returnStaticAircraftList(localCriteria.ViewCriteriaFolderID, companyID, comp_id_list, show_type)
                                    Else
                                        ac_list = portfolio_functions.returnStaticAircraftList(localCriteria.ViewCriteriaFolderID, companyID, "", show_type)   ' CLng(HttpContext.Current.Session.Item("localUser").crmUserCompanyID.ToString)
                                    End If


                                    company_portfolio_links.Text = "<br><font size='+0'>"
                                    If use_insight_roll = "Y" Then
                                        company_portfolio_links.Text &= "<a href='userPortfolio.aspx?comp_id=" & companyID & "&use_insight_op=" & use_insight_op & "' onclick=""javascript:clearAllTextboxes();SelectDropDownItem('11111');"">Show Only My Location</a>"
                                    Else
                                        company_portfolio_links.Text &= "<a href='userPortfolio.aspx?comp_id=" & companyID & "&use_insight_roll=Y&use_insight_op=" & use_insight_op & "' onclick=""javascript:clearAllTextboxes();SelectDropDownItem('11111');"">Show All Locations for My Company</a>"
                                    End If

                                    company_portfolio_links.Text &= "<br/><br/>"

                                    ac_list_temp = portfolio_functions.returnStaticAircraftList(localCriteria.ViewCriteriaFolderID, companyID, comp_id_list, "operated")

                                    If Trim(ac_list_temp) <> "" Then
                                        If use_insight_op = "N" Then
                                            company_portfolio_links.Text &= "<a href='userPortfolio.aspx?comp_id=" & companyID & "&show_type=operated&use_insight_roll=" & use_insight_roll & "' onclick=""javascript:clearAllTextboxes();SelectDropDownItem('11111');"">Show Aircraft Operated</a>"
                                            company_portfolio_links.Text &= "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                                        End If
                                    End If

                                    ac_list_temp = portfolio_functions.returnStaticAircraftList(localCriteria.ViewCriteriaFolderID, companyID, comp_id_list, "own_operated")

                                    If Trim(ac_list_temp) <> "" Then
                                        If use_insight_own_op = "N" Then
                                            company_portfolio_links.Text &= "<a href='userPortfolio.aspx?comp_id=" & companyID & "&show_type=own_operated&use_insight_roll=" & use_insight_roll & "' onclick=""javascript:clearAllTextboxes();SelectDropDownItem('11111');"">Show Aircraft Owned and Operated</a>"
                                            company_portfolio_links.Text &= "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                                        End If
                                    End If

                                    ac_list_temp = portfolio_functions.returnStaticAircraftList(localCriteria.ViewCriteriaFolderID, companyID, comp_id_list, "brokered")

                                    If Trim(ac_list_temp) <> "" Then
                                        If use_insight_brokered = "N" Then
                                            company_portfolio_links.Text &= "<a href='userPortfolio.aspx?comp_id=" & companyID & "&show_type=brokered&use_insight_roll=" & use_insight_roll & "' onclick=""javascript:clearAllTextboxes();SelectDropDownItem('11111');"">Show Aircraft Brokered</a>"
                                            company_portfolio_links.Text &= "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                                        End If
                                    End If

                                    ac_list_temp = portfolio_functions.returnStaticAircraftList(localCriteria.ViewCriteriaFolderID, companyID, comp_id_list, "managed")

                                    If Trim(ac_list_temp) <> "" Then
                                        If use_insight_managed = "N" Then
                                            company_portfolio_links.Text &= "<a href='userPortfolio.aspx?comp_id=" & companyID & "&show_type=managed&use_insight_roll=" & use_insight_roll & "' onclick=""javascript:clearAllTextboxes();SelectDropDownItem('11111');"">Show Aircraft Managed</a>"
                                            company_portfolio_links.Text &= "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                                        End If
                                    End If

                                    company_portfolio_links.Text &= "</font>"

                                    company_portfolio_links.Visible = True
                                    user_portfolio_lbl.Visible = False
                                    user_portfolio_list.Visible = False

                                Else
                                    ac_list = portfolio_functions.returnStaticAircraftList(localCriteria.ViewCriteriaFolderID, CLng(HttpContext.Current.Session.Item("localUser").crmUserCompanyID.ToString))
                                End If

                            Case 1
                                ' ADDED IN MSW - 3/20/20 
                                ac_list = portfolio_functions.return_re_select_aircraft_list(ForceUncheckNotes)

                                ' last_search_label.Text = "<a href=""javascript:void(0);"" class=""display_block padded portfolioLinks"" onclick=""javascript:clearAllTextboxes();SelectDropDownItem('1');"">Aircraft Search Listing</a>"
                                ' last_search_label.Text = "test"

                                '   last_search_label.Visible = False
                                '   last_search_label.Visible = True


                            Case Else

                                ac_list = portfolio_functions.returnAircraftList(portfolio_functions.returnFolderContents(localCriteria.ViewCriteriaFolderID), ForceUncheckNotes)

                        End Select
                    End If



                    ' ADDED IN MSW - 3/31/20 - if the session is saved, then check the box or not, otherwise, yes show 
                    If Not IsPostBack Then
                        If Not IsNothing(HttpContext.Current.Session.Item("Show_Notes")) Then
                            If Trim(HttpContext.Current.Session.Item("Show_Notes")) <> "" Then
                                If Trim(HttpContext.Current.Session.Item("Show_Notes")) = "Y" Then
                                    Show_Notes.Checked = True
                                Else
                                    Show_Notes.Checked = False
                                End If
                            End If
                        Else
                            HttpContext.Current.Session.Item("Show_Notes") = "Y"
                            Show_Notes.Checked = True
                        End If
                    End If


                    'We need to force uncheck notes if the folder is too big
                    If ForceUncheckNotes Then
                        If Show_Notes.Checked Then
                            Show_Notes.Checked = False
                            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me.searchUpdate, Me.GetType(), "removeShowNotes", "alert('Notes has been automatically disabled due to the size of this folder. Please choose a smaller datasubset to view notes.');$(""#" & Show_Notes.ClientID & """). prop(""checked"", false);$(""#" & Show_Notes.ClientID & """). prop(""disabled"", true);", True)
                        End If
                    End If
                    If String.IsNullOrEmpty(ac_list.Trim) Then
                        'portfolio_tabContainer.Visible = False 
                        attention.Text = "<div class=""Box""><p align=""center"">This folder has no aircraft associated with it.</p></div>"
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me.searchUpdate, Me.GetType(), "removeTabNoAC", "$('#" & portfolio_view_results_div.ClientID & "').addClass('display_none');$('#" & attention.ClientID & "').html('<div class=""Box""><p align=""center"">This folder has no aircraft associated with it.</p></div>');CloseLoadingMessage(""DivLoadingMessage"");$('#" & portfolio_tabContainer.ClientID & "').removeClass('display_none');", True)
                    Else
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me.searchUpdate, Me.GetType(), "removeTabYesAC", "$('#" & portfolio_view_results_div.ClientID & "').removeClass('display_none');$('#" & attention.ClientID & "').html('');CloseLoadingMessage(""DivLoadingMessage"");$('#" & portfolio_tabContainer.ClientID & "').removeClass('display_none');", True)


                        Select Case portfolio_tabContainer.ActiveTab.ID
                            Case "portfolio_tabPanel0"
                                TurnOffTabPanels(0)
                                If ranTab0.Text = "false" Or IsPostBack Then
                                    Dim us_reg As Double = 0
                                    Dim th_stage As Double = 0
                                    PageNeedUpdate = True
                                    ranTab0.Text = "true"
                                    Dim summarize_field As String = ""

                                    If Not String.IsNullOrEmpty(ac_list.Trim) Then
                                        If fleet_dropdown.Items.Count > 1 Then
                                            If fleet_dropdown.SelectedValue = 0 Then
                                                summarize_field = ""
                                                '    Graph_Label_tab0.Text = "FLEET BY MFR YEAR"

                                            ElseIf fleet_dropdown.SelectedValue = 1 Then
                                                summarize_field = "amod_airframe_type_code"
                                                fleetTitle.Text = "FLEET BY AIRFRAME TYPE (F / R)"
                                            ElseIf fleet_dropdown.SelectedValue = 2 Then
                                                summarize_field = "atype_name"
                                                fleetTitle.Text = "FLEET BY MAKE TYPE NAME"
                                            ElseIf fleet_dropdown.SelectedValue = 3 Then
                                                summarize_field = "amod_make_name"
                                                fleetTitle.Text = "FLEET BY MAKE"
                                            ElseIf fleet_dropdown.SelectedValue = 4 Then
                                                summarize_field = "amod_model_name"
                                                fleetTitle.Text = "FLEET BY MODEL"
                                            ElseIf fleet_dropdown.SelectedValue = 5 Then
                                                summarize_field = "amjiqs_cat_desc"
                                                fleetTitle.Text = "FLEET BY SIZE CATEGORY"
                                            ElseIf fleet_dropdown.SelectedValue = 6 Then
                                                summarize_field = "ac_mfr_year"
                                                fleetTitle.Text = "FLEET BY MFR YEAR"
                                            ElseIf fleet_dropdown.SelectedValue = 7 Then
                                                summarize_field = "ac_year"
                                                fleetTitle.Text = "FLEET BY DLV YEAR"
                                            ElseIf fleet_dropdown.SelectedValue = 8 Then
                                                summarize_field = "acot_name"
                                                fleetTitle.Text = "FLEET BY OWNERSHIP"
                                            ElseIf fleet_dropdown.SelectedValue = 9 Then
                                                summarize_field = "ambc_name"
                                                fleetTitle.Text = "FLEET BY BODY CONFIGURATION"
                                            ElseIf fleet_dropdown.SelectedValue = 10 Then
                                                summarize_field = "acs_name"
                                                fleetTitle.Text = "FLEET BY LIFECYCLE STAGE"
                                            ElseIf fleet_dropdown.SelectedValue = 11 Then
                                                summarize_field = "acuse_name"
                                                fleetTitle.Text = "FLEET BY USAGE"
                                            ElseIf fleet_dropdown.SelectedValue = 12 Then
                                                summarize_field = "ac_passenger_count"
                                                fleetTitle.Text = "FLEET BY PASSENGERS"
                                            ElseIf fleet_dropdown.SelectedValue = 13 Then
                                                summarize_field = "ac_airframe_tot_hrs"
                                                fleetTitle.Text = "FLEET BY AFTT"
                                            End If


                                        Else
                                            fleet_dropdown.Items.Clear()
                                            fleet_dropdown.Items.Add(New ListItem("All", 0))
                                            fleet_dropdown.Items.Add(New ListItem("Airframe Type(F / R)", 1))
                                            fleet_dropdown.Items.Add(New ListItem("Make Type Name", 2))
                                            fleet_dropdown.Items.Add(New ListItem("Make", 3))
                                            fleet_dropdown.Items.Add(New ListItem("Model", 4))
                                            fleet_dropdown.Items.Add(New ListItem("Size Category", 5))
                                            fleet_dropdown.Items.Add(New ListItem("Year Manufactured", 6))
                                            fleet_dropdown.Items.Add(New ListItem("Year Delivered", 7))
                                            fleet_dropdown.Items.Add(New ListItem("Ownership", 8))
                                            fleet_dropdown.Items.Add(New ListItem("Body Configuration", 9))
                                            fleet_dropdown.Items.Add(New ListItem("Lifecycle Stage", 10))
                                            fleet_dropdown.Items.Add(New ListItem("Usage", 11))
                                            fleet_dropdown.Items.Add(New ListItem("Passengers", 12))
                                            fleet_dropdown.Items.Add(New ListItem("AFTT", 13))
                                        End If


                                        portfolio_functions.display_portfolio_aircraft_age_bar_chart(ac_list, 1, searchUpdate, summarize_field, fleet_dropdown.SelectedValue.ToString)
                                        ac_mfryear_bar_chart.Visible = True

                                        '  portfolio_functions.display_composition_results(ac_list, ac_composition_table.Text)
                                        ' ac_composition_table.Visible = True 

                                        portfolio_functions.display_value_composition_results(ac_list, ac_composition_table.Text, False, isDisplayEvalues, value_summary_box.Text, False, us_reg, th_stage, 6, True)


                                        If us_reg > 0 Or (th_stage - us_reg) > 0 Then
                                            portfolio_functions.display_portfolio_pie_chart_us_international(ac_list, 6, searchUpdate, us_reg, th_stage - us_reg)
                                        End If

                                        ac_composition_table.Visible = True
                                    End If
                                    portfolio_tab_0_graphs.Visible = True

                                    'If ranTab3.Text = "false" Then
                                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(searchUpdate, Me.GetType, "tab0array", portfolio_functions.display_tab0_results_table(AllowExport, ac_list, False, summarize_field, Show_Notes.Checked).ToString, True)
                                    'End If
                                    acSearchResultsTable_tabPanel0.Visible = True
                                    tab_0_graph_update_panel.Update()
                                    tab_0_update_panel.Update()
                                End If
                            Case "portfolio_tabPanel1"
                                TurnOffTabPanels(1)
                                If ranTab1.Text = "false" Then
                                    PageNeedUpdate = True
                                    Dim us_reg As Double = 0
                                    Dim th_stage As Double = 0
                                    ranTab1.Text = "true"
                                    value_summary_box.Text = ""

                                    If HttpContext.Current.Session.Item("localPreferences").UserSPIViewFlag Then
                                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(searchUpdate, Me.GetType, "tab1array", portfolio_functions.display_tab1_results_table(AllowExport, ac_list, False, isDisplayEvalues).ToString, True)

                                        acSearchResultsTable_tabPanel1.Visible = True
                                        portfolio_functions.display_value_composition_results(ac_list, value_composition_box.Text, False, isDisplayEvalues, value_summary_box.Text, True, us_reg, th_stage, 7)

                                    Else   ' if we are here, we need to hide the values columns
                                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(searchUpdate, Me.GetType, "tab1array", portfolio_functions.display_tab1_results_table(AllowExport, ac_list, False, isDisplayEvalues, Show_Notes.Checked).ToString, True)

                                        acSearchResultsTable_tabPanel1.Visible = True
                                        portfolio_functions.display_value_composition_results(ac_list, value_composition_box.Text, False, isDisplayEvalues, value_summary_box.Text, True, us_reg, th_stage, 7)

                                    End If

                                    If us_reg > 0 Or (th_stage - us_reg) > 0 Then
                                        portfolio_functions.display_portfolio_pie_chart_us_international(ac_list, 7, searchUpdate, us_reg, th_stage - us_reg)
                                    End If

                                    portfolio_tab_1_graphs.CssClass = ""
                                    portfolio_tab_1_graphs.Visible = True
                                    acSearchResultsTable_tabPanel1.Visible = True
                                    tab_1_update_panel.Update()
                                    tab_1_graph_update_panel.Update()
                                End If
                            Case "portfolio_tabPanel2"
                                'turn off other top panels.
                                TurnOffTabPanels(2)
                                If ranTab2.Text = "false" Or IsPostBack Then
                                    PageNeedUpdate = True
                                    ranTab2.Text = "true"
                                    portfolio_tab_2_graphs.CssClass = ""
                                    portfolio_tab_2_graphs.Visible = True



                                    Dim summarize_field As String = ""
                                    Dim string_for_graphs As String = ""
                                    Dim datatable_dropdown As New DataTable

                                    If equip_dropdown.Items.Count > 1 Then
                                        If equip_dropdown.SelectedValue = 0 Or equip_dropdown.SelectedValue = 12 Then
                                            summarize_field = ""
                                            afttTitle.InnerText = "AFTT SUMMARY"
                                        ElseIf equip_dropdown.SelectedValue = 1 Then
                                            summarize_field = "ac_engine_name"
                                            afttTitle.InnerText = "ENGINE MODEL NAME SUMMARY"
                                        ElseIf equip_dropdown.SelectedValue = 2 Then
                                            summarize_field = "emp_provider_name"
                                            afttTitle.InnerText = "ENGINE MAINTENANCE PROGRAM PROVIDER SUMMARY"
                                        ElseIf equip_dropdown.SelectedValue = 3 Then
                                            summarize_field = "emp_program_name"
                                            afttTitle.InnerText = "ENGINE MAINTENANCE PROGRAM NAME SUMMARY"
                                        ElseIf equip_dropdown.SelectedValue = 4 Then
                                            summarize_field = "emgp_provider_name"
                                            afttTitle.InnerText = "ENGINE MANAGEMENT PROGRAM PROVIDER SUMMARY"
                                        ElseIf equip_dropdown.SelectedValue = 5 Then
                                            summarize_field = "emgp_program_name"
                                            afttTitle.InnerText = "ENGINE MANAGEMENT PROGRAM NAME SUMMARY"
                                        ElseIf equip_dropdown.SelectedValue = 6 Then
                                            summarize_field = "amp_provider_name"
                                            afttTitle.InnerText = "AIRFRAME MAINTENANCE PROGRAM PROVIDER"
                                        ElseIf equip_dropdown.SelectedValue = 7 Then
                                            summarize_field = "amp_program_name"
                                            afttTitle.InnerText = "AIRFRAME MAINTENANCE PROGRAM NAME"
                                        ElseIf equip_dropdown.SelectedValue = 8 Then
                                            summarize_field = "amtp_provider_name"
                                            afttTitle.InnerText = "AIRFRAME MAINTENANCE TRACKING PROVIDER"
                                        ElseIf equip_dropdown.SelectedValue = 9 Then
                                            summarize_field = "amtp_program_name"
                                            afttTitle.InnerText = "AIRFRAME MAINTENANCE TRACKING NAME"
                                        ElseIf equip_dropdown.SelectedValue = 10 Then
                                            summarize_field = "ac_maintained"
                                            afttTitle.InnerText = "MAINTENANCE REGULATION"
                                        ElseIf equip_dropdown.SelectedValue = 11 Then
                                            summarize_field = "ac_apu_model_name"
                                            afttTitle.InnerText = "APU MODEL NAME"
                                        End If
                                    Else
                                        equip_dropdown.Items.Clear()
                                        equip_dropdown.Items.Add(New ListItem("All", 0))
                                        equip_dropdown.Items.Add(New ListItem("Engine Model Name", 1))
                                        equip_dropdown.Items.Add(New ListItem("Engine Maintenance Program Provider", 2))
                                        equip_dropdown.Items.Add(New ListItem("Engine Maintenance Program Name", 3))
                                        equip_dropdown.Items.Add(New ListItem("Engine Management Program Provider", 4))
                                        equip_dropdown.Items.Add(New ListItem("Engine Management Program Name", 5))
                                        equip_dropdown.Items.Add(New ListItem("Airframe Maintenance Program Provider", 6))
                                        equip_dropdown.Items.Add(New ListItem("Airframe Maintenance Program Name", 7))
                                        equip_dropdown.Items.Add(New ListItem("Airframe Maintenance Tracking Provider", 8))
                                        equip_dropdown.Items.Add(New ListItem("Airframe Maintenance Tracking Name", 9))
                                        equip_dropdown.Items.Add(New ListItem("Maintenance Regulation", 10))
                                        equip_dropdown.Items.Add(New ListItem("APU Model Name", 11))
                                        equip_dropdown.Items.Add(New ListItem("Airframe Total Time (AFTT)", 12))
                                    End If


                                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(searchUpdate, Me.GetType, "tab2array", portfolio_functions.display_tab2_results_table(AllowExport, ac_list, False, summarize_field, datatable_dropdown, Show_Notes.Checked).ToString, True)


                                    If equip_dropdown.SelectedValue = 0 Or equip_dropdown.SelectedValue = 12 Then
                                        acSearchResultsTable_tabPanel2.Visible = True
                                        acSearchResultsTable_tabPanel22.Visible = False
                                    Else
                                        acSearchResultsTable_tabPanel22.Visible = True
                                        acSearchResultsTable_tabPanel2.Visible = False
                                    End If


                                    acSearchResultsTable_tabPanel2.Visible = True

                                    If Trim(summarize_field) <> "" Then
                                        Dim us_reg As Double = 0
                                        Dim th_stage As Double = 0
                                        portfolio_functions.display_dropdown_bar_chart(datatable_dropdown, 2, searchUpdate, summarize_field)
                                        'But when the summaries are selected, have it change to use the same approach as other tabs where it shows the composition box to the left and the bar chart of what was selected to the right.

                                        engine_maintenance_program_container.Visible = False
                                        maintenance_program_summary_container.Visible = False
                                        maintenance_composition_container.Visible = True
                                        maintenance_graph_container.Attributes.Remove("class")
                                        maintenance_graph_container.Attributes.Add("class", "seven columns marginLeftHalf")
                                        portfolio_functions.display_value_composition_results(ac_list, maintenance_composition_table.Text, False, isDisplayEvalues, value_summary_box.Text, False, us_reg, th_stage, 14, True)
                                        If us_reg > 0 Or (th_stage - us_reg) > 0 Then
                                            portfolio_functions.display_portfolio_pie_chart_us_international(ac_list, 14, searchUpdate, us_reg, th_stage - us_reg)
                                        End If


                                    Else
                                        maintenance_graph_container.Attributes.Remove("class")
                                        maintenance_graph_container.Attributes.Add("class", "four columns")
                                        engine_maintenance_program_container.Visible = True
                                        maintenance_program_summary_container.Visible = True
                                        maintenance_composition_container.Visible = False
                                        portfolio_functions.display_portfolio_maint_bar_chart(ac_list, 2, searchUpdate)
                                    End If

                                    'maint_graph_1.Visible = True



                                    portfolio_functions.display_portfolio_pie_chart_maint_info_1(ac_list, 3, searchUpdate)
                                    ' maint_graph_2.Visible = True


                                    portfolio_functions.display_portfolio_pie_chart_maint_info_2(ac_list, 4, searchUpdate)
                                    ' maint_graph_3.Visible = True
                                    tab_2_update_panel.Update()
                                    tab_2_graph_update_panel.Update()
                                End If
                            Case "portfolio_tabPanel3"
                                TurnOffTabPanels(3)
                                If ranTab3.Text = "false" Then

                                    If features_dropdown.Items.Count = 2 Then

                                        Dim tempTable As New DataTable
                                        tempTable = portfolio_functions.GetFeaturesDropdown(ac_list, False, 0)
                                        If Not IsNothing(tempTable) Then
                                            If tempTable.Rows.Count > 0 Then 'acatt_name,acatt_id
                                                For Each r As DataRow In tempTable.Rows
                                                    features_dropdown.Items.Add(New ListItem(r("acatt_name"), r("acatt_id")))
                                                Next
                                            End If
                                        End If
                                    End If


                                    PageNeedUpdate = True
                                    Dim us_reg As Integer = 0
                                    Dim th_stage As Integer = 0
                                    Dim TotalAcCount As Integer = 0
                                    Dim MapString As String = ""
                                    portfolio_tab_3_graphs.Visible = True
                                    featuresChartPanel.Visible = True
                                    features_fleet_composition_panel.Visible = True

                                    ranTab3.Text = "true"
                                    Dim getTotalAircraftNumber As Long = 0

                                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(searchUpdate, Me.GetType, "tab3array", portfolio_functions.display_tab3_results_table(AllowExport, ac_list, False, features_dropdown.SelectedValue, Show_Notes.Checked, features_dropdown, features_dropdownButton).ToString, True)

                                    portfolio_functions.display_value_composition_results(ac_list, features_fleet_composition_label.Text, False, isDisplayEvalues, value_summary_box.Text, False, us_reg, th_stage, 21, True, TotalAcCount)


                                    If features_dropdown.SelectedValue > 0 Then
                                        Dim FeaturesDescription As String = portfolio_functions.GetFeaturesDescription(features_dropdown.SelectedValue)
                                        featuresGaugeSelectedPanel.Visible = True
                                        featuresChartPanel.Visible = False
                                        featureGaugeSelectedDescriptionPanel.Visible = False
                                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(searchUpdate, Me.GetType, "gaugeString", portfolio_functions.BuildSelectedFeatureGauge(localCriteria, ac_list, TotalAcCount, features_dropdown.SelectedValue, featuresGaugeSelectedLabel, featuresGaugeSelectedCompositionLabel) + vbCrLf, True)
                                        portfolio_functions.display_value_composition_results(ac_list, featuresGaugeSelectedCompositionLabel.Text, False, isDisplayEvalues, value_summary_box.Text, False, 0, 0, 25, True, 0, True, features_dropdown.SelectedValue)
                                        If Not String.IsNullOrEmpty(FeaturesDescription) Then
                                            featureGaugeSelectedDescriptionPanel.Visible = True
                                            featureGaugeSelectedDescription.Text = Trim(FeaturesDescription)
                                        End If
                                    Else
                                        featuresGaugeSelectedPanel.Visible = False
                                        featuresChartPanel.Visible = True
                                        MapString = portfolio_functions.BuildFeaturesChart(ac_list, featuresChartPanel.Visible, features_dropdown.SelectedValue, featuresGraphSubHeader.InnerText)
                                        Dim selectedString As String = ""
                                        Dim clickString As String = ""
                                        selectedString = "var value = data.getValue(selectedItem.row, 0);" + vbCrLf

                                        selectedString += "$('#" & features_dropdown.ClientID & " option')" + vbCrLf
                                        selectedString += ".filter(function() { return $.trim( $(this).text() ) == value; })" + vbCrLf
                                        selectedString += ".attr('selected','selected');" + vbCrLf
                                        selectedString += "$(""#" & features_dropdownButton.ClientID & """).click();" + vbCrLf

                                        portfolio_functions.display_portfolio_generic_bar_chart("", 23, searchUpdate, MapString, IIf(features_dropdown.SelectedValue = 0, "90%", "90%"), True, selectedString, clickString, IIf(features_dropdown.SelectedValue = -1, True, False), IIf(features_dropdown.SelectedValue = 0, "60%", "45%"))
                                    End If

                                    portfolio_tab_3_graphs.CssClass = ""

                                    acSearchResultsTable_tabPanel3.Visible = True

                                    If us_reg > 0 Or (th_stage - us_reg) > 0 Then
                                        portfolio_functions.display_portfolio_pie_chart_us_international(ac_list, 21, searchUpdate, us_reg, th_stage - us_reg)
                                    End If

                                    tab_3_update_panel.Update()
                                    tab_3_graph_update_panel.Update()
                                End If
                            Case "portfolio_tabPanel4"
                                If ranTab4.Text = "false" Then
                                    TurnOffTabPanels(4)
                                    ranTab4.Text = "true"
                                    PageNeedUpdate = True
                                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(searchUpdate, Me.GetType, "tab4array", portfolio_functions.display_tab4_results_table(AllowExport, ac_list, False, Show_Notes.Checked).ToString, True)

                                    Dim SummaryTable As New DataTable
                                    Dim last_year_count As Integer = 0
                                    util_functions.Aircraft_IDS_String = ac_list

                                    Dim sectionString As String = ""
                                    Dim charting_String As String = ""
                                    localCriteria.ViewCriteriaDocumentsEndDate = Now()
                                    localCriteria.ViewCriteriaDocumentsStartDate = CDate(DateAdd(DateInterval.Month, -12, Now()))
                                    SummaryTable = util_functions.get_flight_activity_overall_top_function(localCriteria, last_year_count, "")
                                    portfolio_functions.BuildUtilizationSummaryTable(SummaryTable, flight_activity_summary, localCriteria.ViewCriteriaFolderID, localCriteria.ViewCriteriaFolderName.Trim)


                                    Call util_functions.get_flight_profile_top_function(localCriteria, sectionString, "Month", Session.Item("localSubscription").crmSubinst_FAA_data_date, "")
                                    portfolio_tab_4_graphs.CssClass = ""
                                    portfolio_tab_4_graphs.Visible = True
                                    DisplayFunctions.load_google_chart(Nothing, sectionString, "", "Flights Per Month", "utilizationViewGraphall", 600, 255, "POINTS", 1, charting_String, Me.Page, searchUpdate, False, False, True, False, False, True, False, True, False, False, 0, "")
                                    portfolio_functions.PageResize(200, searchUpdate, "testFlight", "utilizationViewGraphall")
                                    charting_String = charting_String

                                    charting_String = Replace(charting_String, "'width':600,", "")
                                    charting_String = Replace(charting_String, "'height':255,", "")

                                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(searchUpdate, Me.GetType, "chartString", "function testFlight() { " & charting_String + vbCrLf & "};testFlight();", True)
                                    acSearchResultsTable_tabPanel4.Visible = True
                                    tab_4_update_panel.Update()
                                    tab_4_graph_update_panel.Update()
                                End If
                            Case "portfolio_tabPanel5"
                                If ranTab5.Text = "false" Then
                                    Dim us_reg As Integer = 0
                                    Dim th_stage As Integer = 0
                                    operator_fleet_comp.Visible = True
                                    TurnOffTabPanels(5)
                                    ranTab5.Text = "true"
                                    portfolio_tab_5_graphs.CssClass = ""
                                    PageNeedUpdate = True
                                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(searchUpdate, Me.GetType, "tab5array", portfolio_functions.display_tab5_results_table(AllowExport, ac_list, False, Show_Notes.Checked).ToString, True)
                                    portfolio_tab_5_graphs.Visible = True
                                    acSearchResultsTable_tabPanel5.Visible = True
                                    '  System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType, "chartStringOperatorCert", portfolio_functions.DrawOperatorCertifications(ac_list) + vbCrLf, True)
                                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(searchUpdate, Me.GetType, "chartStringOperatorType", portfolio_functions.DrawOperatorBusinessType(ac_list, searchUpdate) + vbCrLf & ";", True)
                                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(searchUpdate, Me.GetType, "chartStringOperatorContinent", portfolio_functions.DrawOwnerContinentPieChart(ac_list, True, False, searchUpdate) + vbCrLf, True)


                                    portfolio_functions.display_value_composition_results(ac_list, operator_fleet_comp.Text, False, isDisplayEvalues, value_summary_box.Text, False, us_reg, th_stage, 15, True)

                                    If us_reg > 0 Or (th_stage - us_reg) > 0 Then
                                        portfolio_functions.display_portfolio_pie_chart_us_international(ac_list, 15, searchUpdate, us_reg, th_stage - us_reg)
                                    End If

                                    tab_5_update_panel.Update()
                                    tab_5_graph_update_panel.Update()
                                End If
                            Case "portfolio_tabPanel6"
                                If ranTab6.Text = "false" Then
                                    Dim us_reg As Integer = 0
                                    Dim th_stage As Integer = 0
                                    TurnOffTabPanels(6)
                                    owners_fleet_composition.Visible = True
                                    ranTab6.Text = "true"
                                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(searchUpdate, Me.GetType, "tab6array", portfolio_functions.display_tab6_results_table(AllowExport, ac_list, False, Show_Notes.Checked).ToString, True)
                                    PageNeedUpdate = True
                                    portfolio_tab_6_graphs.Visible = True
                                    portfolio_tab_6_graphs.CssClass = ""
                                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(searchUpdate, Me.GetType, "chartStringOwnership", portfolio_functions.DrawOwnershipPieChart(ac_list, searchUpdate) + vbCrLf, True)
                                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(searchUpdate, Me.GetType, "chartStringContinent", portfolio_functions.DrawOwnerContinentPieChart(ac_list, False, True, searchUpdate) + vbCrLf, True)
                                    acSearchResultsTable_tabPanel6.Visible = True

                                    portfolio_functions.display_value_composition_results(ac_list, owners_fleet_composition.Text, False, isDisplayEvalues, value_summary_box.Text, False, us_reg, th_stage, 16, True)

                                    If us_reg > 0 Or (th_stage - us_reg) > 0 Then
                                        portfolio_functions.display_portfolio_pie_chart_us_international(ac_list, 16, searchUpdate, us_reg, th_stage - us_reg)
                                    End If

                                    tab_6_update_panel.Update()
                                    tab_6_graph_update_panel.Update()
                                End If
                            Case "portfolio_tabPanel7"
                                If ranTab7.Text = "false" Then
                                    ranTab7.Text = "true"
                                    TurnOffTabPanels(7)
                                    Dim chartingString As String = ""
                                    portfolio_tab_7_graphs.CssClass = ""
                                    portfolio_tab_7_graphs.Visible = True
                                    PageNeedUpdate = True
                                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(searchUpdate, Me.GetType, "tab7array", portfolio_functions.display_tab7_results_table(AllowExport, ac_list, False, chartingString, searchUpdate).ToString, True)

                                    acSearchResultsTable_tabPanel7.Visible = True
                                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(searchUpdate, Me.GetType, "chartStringModels", chartingString + vbCrLf, True)
                                    tab_7_update_panel.Update()
                                    tab_7_graph_update_panel.Update()
                                End If
                            Case "portfolio_tabPanel8"
                                If ranTab8.Text = "false" Then
                                    ranTab8.Text = "true"
                                    TurnOffTabPanels(8)
                                    Dim chartingString As String = ""
                                    portfolio_tab_8_graphs.CssClass = ""
                                    portfolio_tab_8_graphs.Visible = True
                                    PageNeedUpdate = True

                                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(searchUpdate, Me.GetType, "tab8array", portfolio_functions.display_tab8_results_table(AllowExport, ac_list, False, chartingString, companyID, show_type, comp_id_list, Show_Notes.Checked).ToString, True)

                                    Call CREATE_AC_DEALER(ac_list)

                                    acSearchResultsTable_tabPanel8.Visible = True
                                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(searchUpdate, Me.GetType, "chartStringHiistory", chartingString + vbCrLf, True)
                                    tab_8_update_panel.Update()
                                    tab_8_graph_update_panel.Update()
                                End If
                            Case "portfolio_tabPanel9"
                                If ranTab9.Text = "false" Or IsPostBack Then
                                    ranTab9.Text = "true"
                                    Dim us_reg As Double = 0
                                    Dim th_stage As Double = 0

                                    TurnOffTabPanels(9)

                                    'tab9_label.Text = "Here"

                                    Dim chartingString As String = ""
                                    portfolio_tab_9_graphs.CssClass = ""
                                    portfolio_tab_9_graphs.Visible = True '' NEED TO CHANGE BACK TO TRUE TO SEE GRAPHS 
                                    PageNeedUpdate = True

                                    Dim summarize_field As String = ""
                                    Dim summary_table As New DataTable

                                    If location_drop.Items.Count > 1 Then
                                        If location_drop.SelectedValue = 0 Then
                                            summarize_field = ""
                                        ElseIf location_drop.SelectedValue = 1 Then
                                            summarize_field = "ac_aport_iata_code"
                                        ElseIf location_drop.SelectedValue = 2 Then
                                            summarize_field = "ac_aport_icao_code"
                                        ElseIf location_drop.SelectedValue = 3 Then
                                            summarize_field = "ac_aport_faaid_code"
                                        ElseIf location_drop.SelectedValue = 4 Then
                                            summarize_field = "ac_aport_name"
                                        ElseIf location_drop.SelectedValue = 5 Then
                                            summarize_field = "ac_aport_city"
                                        ElseIf location_drop.SelectedValue = 6 Then
                                            summarize_field = "ac_aport_state_name"
                                        ElseIf location_drop.SelectedValue = 7 Then
                                            summarize_field = "ac_aport_country"
                                        ElseIf location_drop.SelectedValue = 8 Then
                                            summarize_field = "ac_country_continent_name"
                                        ElseIf location_drop.SelectedValue = 9 Then
                                            summarize_field = "ac_country_of_registration"
                                        End If
                                    Else
                                        location_drop.Items.Clear()
                                        location_drop.Items.Add(New ListItem("All", 0))
                                        location_drop.Items.Add(New ListItem("Base Airport IATA Code", 1))
                                        location_drop.Items.Add(New ListItem("Base Airport ICAO Code", 2))
                                        location_drop.Items.Add(New ListItem("Base FAA ID Code", 3))
                                        location_drop.Items.Add(New ListItem("Base Airport Name", 4))
                                        location_drop.Items.Add(New ListItem("Base Airport City", 5))
                                        location_drop.Items.Add(New ListItem("Base Airport State Name", 6))
                                        location_drop.Items.Add(New ListItem("Base Airport Country", 7))
                                        location_drop.Items.Add(New ListItem("Base Continent", 8))
                                        location_drop.Items.Add(New ListItem("Country of Registration", 9))
                                    End If



                                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(searchUpdate, Me.GetType, "tab9array", portfolio_functions.display_tab9_results_table(AllowExport, ac_list, False, chartingString, companyID, show_type, comp_id_list, summarize_field, summary_table, Show_Notes.Checked).ToString, True)

                                    If Trim(summarize_field) <> "" Then
                                        Call CREATE_AC_LOCATIONS(ac_list, summary_table, summarize_field, portfolio_functions)
                                    Else
                                        Call CREATE_AC_LOCATIONS(ac_list, Nothing, "", portfolio_functions)
                                    End If


                                    portfolio_functions.display_value_composition_results(ac_list, location_composition.Text, False, isDisplayEvalues, value_summary_box.Text, False, us_reg, th_stage, 9, True)


                                    If location_drop.SelectedValue = 0 Then
                                        acSearchResultsTable_tabPanel9.Visible = True
                                        acSearchResultsTable_tabPanel92.Visible = False
                                    Else
                                        acSearchResultsTable_tabPanel92.Visible = True
                                        acSearchResultsTable_tabPanel9.Visible = False
                                    End If

                                    If us_reg > 0 Or (th_stage - us_reg) > 0 Then
                                        portfolio_functions.display_portfolio_pie_chart_us_international(ac_list, 9, searchUpdate, us_reg, th_stage - us_reg)
                                    End If

                                    'System.Web.UI.ScriptManager.RegisterClientScriptBlock(searchUpdate, Me.GetType, "chartStringLocation", chartingString + vbCrLf, True)
                                    tab_9_update_panel.Update()
                                    tab_9_graph_update_panel.Update()
                                End If
                        End Select
                    End If
                End If

                If Not String.IsNullOrEmpty(ac_list.Trim) Then
                    HttpContext.Current.Session.Item("portfolioAircraft") = ac_list.Trim
                    HttpContext.Current.Session.Item("portfolioFolder") = localCriteria.ViewCriteriaFolderID
                    setUpLoadJavascript(PageNeedUpdate)
                End If

            Else
                HttpContext.Current.Session.Item("portfolioFolder") = 0
                HttpContext.Current.Session.Item("portfolioAircraft") = ""
                'buttons1.Text = "<a class=""underline cursor"" onclick=""javascript:window.close();return false;"" class=""close_button"">Close Window</a>"
            End If


        Catch ex As Exception
            If Not IsNothing(masterPage) Then
                masterPage.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString & " (Page_Load): " & ex.Message.ToString)
            Else
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += " (Page_Load): " + ex.Message.ToString.Trim
            End If
        End Try


    End Sub


    'Public Sub run_selected_portfolio_check()

    '    If Show_Notes.Checked = True Then
    '        HttpContext.Current.Session.Item("Show_Notes") = "Y"
    '    Else
    '        HttpContext.Current.Session.Item("Show_Notes") = "N"
    '    End If

    'End Sub
    Public Sub CREATE_AC_DEALER(ByVal ac_list As String)
        Dim ResultsString As String = ""
        Dim MapString As String = ""
        Dim temptable As New DataTable
        Dim returnString As String = ""

        Dim acdealer_view_function As New aircraft_dealer_functions

        acdealer_view_function.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
        acdealer_view_function.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
        acdealer_view_function.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
        acdealer_view_function.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
        acdealer_view_function.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim



        If Trim(comp_id_list) <> "" Or companyID > 0 Then

            If use_insight_roll = "Y" Then
                temptable = acdealer_view_function.ac_dealer_get_relationship_all_trans_main_comp_id(companyID, "", 0, searchCriteria, comp_id_list, "N", show_type, ac_list)
            Else
                temptable = acdealer_view_function.ac_dealer_get_relationship_all_trans_main_comp_id(companyID, "", 0, searchCriteria, "", "N", show_type, ac_list)
            End If


            If Not IsNothing(temptable) Then
                If temptable.Rows.Count > 0 Then

                    MapString = " ['Rel Type', 'Total Count']"
                    For Each r As DataRow In temptable.Rows
                        MapString += ", ['" & r("RELTYPE") & "', " & r("numtrans") & "]"
                    Next
                End If
            End If

            DisplayFunctions.load_google_chart(New AjaxControlToolkit.TabPanel, MapString, "", "", "chart_div_port_tab2_all", 605, 230, "ARRAY", 2, returnString, Me.Page, Me.tab_8_graph_update_panel, False, False, False, False, False, False, False, False, False, True)
        Else

            temptable = acdealer_view_function.ac_get_trans_by_year(companyID, "", 0, searchCriteria, "", "N", show_type, ac_list)

            If Not IsNothing(temptable) Then
                If temptable.Rows.Count > 0 Then

                    MapString = " ['Year', 'Total Count']"
                    For Each r As DataRow In temptable.Rows
                        MapString += ", ['" & r("Year_Of") & "', " & r("numtrans") & "]"
                    Next
                End If
            End If

            DisplayFunctions.load_google_chart(New AjaxControlToolkit.TabPanel, MapString, "", "", "chart_div_port_tab2_all", 605, 230, "ARRAY", 2, returnString, Me.Page, Me.tab_8_graph_update_panel, False, False, False, False, False, True, False, False, False, False)

            Me.left_graph_label.Text = "Transactions By Year"
        End If


        temptable.Clear()
        If use_insight_roll = "Y" Then
            temptable = acdealer_view_function.ac_dealer_get_relationship_all_trans_main_comp_id(companyID, "", 0, searchCriteria, comp_id_list, "Y", show_type, ac_list)
        Else
            temptable = acdealer_view_function.ac_dealer_get_relationship_all_trans_main_comp_id(companyID, "", 0, searchCriteria, "", "Y", show_type, ac_list)
        End If


        If Not IsNothing(temptable) Then
            If temptable.Rows.Count > 0 Then

                MapString = " ['Rel Type', 'Total Count']"
                For Each r As DataRow In temptable.Rows
                    MapString += ", ['" & r("jcat_subcategory_name") & "', " & r("numtrans") & "]"
                Next
            End If
        End If



        DisplayFunctions.load_google_chart(New AjaxControlToolkit.TabPanel, MapString, "", "", "chart_div_port_tab3_all", 405, 230, "ARRAY", 3, returnString, Me.Page, Me.tab_8_graph_update_panel, False, False, False, False, False, False, False, False, False, True)


        Call load_google_chart_all(New AjaxControlToolkit.TabPanel, returnString, tab_8_graph_update_panel)

    End Sub

    Public Sub CREATE_AC_LOCATIONS(ByVal ac_list As String, ByVal summary_table As DataTable, ByVal summary_field As String, portfolio_functions As userPortfolioDataLayer)
        Dim ResultsString As String = ""
        Dim MapString As String = ""
        Dim temptable As New DataTable
        Dim returnString As String = ""

        Dim acdealer_view_function As New aircraft_dealer_functions

        acdealer_view_function.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
        acdealer_view_function.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
        acdealer_view_function.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
        acdealer_view_function.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
        acdealer_view_function.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim

        temptable = acdealer_view_function.get_country_continent_totals_Graphs(ac_list, "ac_aport_country")


        If Not IsNothing(summary_table) Then
            Me.left_graph_label99.Text = location_drop.SelectedItem.ToString & " Summary"





            If Not IsNothing(summary_table) Then
                If summary_table.Rows.Count > 0 Then
                    Dim x As Integer = 0

                    MapString += "data.addColumn('string', '" & location_drop.SelectedItem.ToString & "');" + vbCrLf
                    MapString += "data.addColumn('number', 'Total Count');" + vbCrLf

                    MapString += "data.addRows(REPLACENUMBERHERE);" + vbCrLf

                    For Each r As DataRow In summary_table.Rows
                        If Not IsDBNull(r("Location")) Then
                            If Not String.IsNullOrEmpty(r("Location")) Then
                                If Not (r("Location").ToString.ToLower) = "unknown" Then
                                    If x < 50 Then
                                        MapString += "data.setCell(" + x.ToString + ", 0, '" + r("Location") + "');" + vbCrLf
                                        MapString += "data.setCell(" + x.ToString + ", 1, " + r("tcount").ToString + ");" + vbCrLf
                                        x += 1
                                    End If
                                End If
                            End If
                        End If
                    Next

                    MapString = Replace(MapString, "REPLACENUMBERHERE", x.ToString)

                End If
            End If

            portfolio_functions.display_portfolio_generic_bar_chart("", 13, Me.tab_9_graph_update_panel, MapString)
            ' DisplayFunctions.load_google_chart(New AjaxControlToolkit.TabPanel, MapString, "", "", "chart_div_port_tab99_all", 600, 330, "ARRAY", 3, returnString, Me.Page, Me.tab_9_graph_update_panel, False, False, False, False, True, True, False, False, False, False, 0, "", "", False)
        Else
            Dim x As Integer = 0
            If Not IsNothing(temptable) Then
                If temptable.Rows.Count > 0 Then

                    MapString += "data.addColumn('string', 'Country');" + vbCrLf
                    MapString += "data.addColumn('number', 'Total Count');" + vbCrLf

                    MapString += "data.addRows(REPLACENUMBERHERE);" + vbCrLf

                    For Each r As DataRow In temptable.Rows
                        If Not IsDBNull(r("ac_aport_country")) Then
                            If Not String.IsNullOrEmpty(r("ac_aport_country")) Then
                                If Not (r("ac_aport_country").ToString.ToLower) = "unknown" Then
                                    If x < 50 Then
                                        MapString += "data.setCell(" + x.ToString + ", 0, '" + r("ac_aport_country") + "');" + vbCrLf
                                        MapString += "data.setCell(" + x.ToString + ", 1, " + r("tcount").ToString + ");" + vbCrLf
                                        x += 1
                                    End If
                                End If
                            End If
                        End If
                    Next

                    MapString = Replace(MapString, "REPLACENUMBERHERE", x.ToString)
                End If
            End If

            portfolio_functions.display_portfolio_generic_bar_chart("", 13, Me.tab_9_graph_update_panel, MapString)
            'DisplayFunctions.load_google_chart(New AjaxControlToolkit.TabPanel, MapString, "", "", "chart_div_port_tab99_all", 600, 330, "ARRAY", 2, returnString, Me.Page, Me.tab_9_graph_update_panel, False, False, False, False, True, True, False, False, False, False, 0, "", "", False)

            'Me.left_graph_label99.Text = "Continent Summary"

            'temptable.Clear()
            'temptable = acdealer_view_function.get_country_continent_totals_Graphs(ac_list, "ac_country_continent_name")


            'If Not IsNothing(temptable) Then
            '    If temptable.Rows.Count > 0 Then

            '        MapString = " ['Continent', 'Total Count']"
            '        For Each r As DataRow In temptable.Rows
            '            MapString += ", ['" & r("ac_country_continent_name") & "', " & r("tcount") & "]"
            '        Next
            '    End If
            'End If

            'DisplayFunctions.load_google_chart(New AjaxControlToolkit.TabPanel, MapString, "", "", "chart_div_port_tab99_all", 490, 330, "ARRAY", 3, returnString, Me.Page, Me.tab_9_graph_update_panel, False, False, False, False, False, False, False, False, False, True)
        End If



        Call load_google_chart_all(New AjaxControlToolkit.TabPanel, returnString, tab_9_graph_update_panel)

    End Sub
    Public Sub load_google_chart_all(ByVal tab_to_add_to As AjaxControlToolkit.TabPanel, ByVal string_from_charts As String, updatePanel As UpdatePanel)
        Dim GoogleChart1TabScript As StringBuilder = New StringBuilder()

        Dim temp_string As String = ""
        Dim label_script As New Label
        Dim chart_label As New Label

        temp_string = "<script type=""text/javascript"">"




        temp_string &= "function drawChartsLocation() {"

        temp_string &= string_from_charts

        temp_string &= " } "
        temp_string &= "drawChartsLocation();"
        temp_string &= "</script>"


        label_script.ID = "label_script"
        label_script.Text = temp_string


        'tab_to_add_to.Controls.AddAt(0, label_script)


        If Not Page.ClientScript.IsClientScriptBlockRegistered("GoogleChartLocationTab") Then
            GoogleChart1TabScript.Append(temp_string)

            System.Web.UI.ScriptManager.RegisterStartupScript(updatePanel, Me.GetType(), "GoogleChartLocationTab", GoogleChart1TabScript.ToString, False)
        End If


    End Sub

    Sub setUpLoadJavascript(ByVal PageNeedUpdate As Boolean)
        Dim JavascriptOnLoad As String = ""
        Dim functionName As String = ""

        ' JavascriptOnLoad += vbCrLf + " var prm = Sys.WebForms.PageRequestManager.getInstance();"
        '
        If AllowExport = True Then
            'This means they are not over the limit. We need to verify they aren't a demo user. If they aren't, we're going to keep the allow export as true.
            'If they have no allow export, it doesn't really matter if they are demo or not, they still can't export.
            If Session.Item("localUser").crmDemoUserFlag Or Session.Item("localUser").crmAllowExport_Flag = False Then
                AllowExport = False
            End If
        End If

        If localCriteria.ViewCriteriaFolderID > 0 Then

            Select Case portfolio_tabContainer.ActiveTab.ID


                Case "portfolio_tabPanel0"
                    If fleet_dropdown.SelectedValue = 0 Then
                        functionName = "CreateSearchTableArray0(""tab0_InnerTable"",""tab0_DataTable"",""tab0_jQueryTable"", " & AllowExport.ToString.ToLower & ");"
                    Else
                        functionName = "CreateSearchTableArray0(""tab0_InnerTable"",""tab0_DataTable_Summary"",""tab0_jQueryTable"", " & AllowExport.ToString.ToLower & ");"
                    End If
                Case "portfolio_tabPanel1"
                    functionName = "CreateSearchTableArray0(""tab1_InnerTable"",""tab1_DataTable"",""tab1_jQueryTable"", " & AllowExport.ToString.ToLower & ");"
                Case "portfolio_tabPanel2"

                    If equip_dropdown.SelectedValue = 0 Or equip_dropdown.SelectedValue = 12 Then
                        functionName = "CreateSearchTableArray0(""tab2_InnerTable"",""tab2_DataTable"",""tab2_jQueryTable"", " & AllowExport.ToString.ToLower & ");"
                    Else
                        functionName = "CreateSearchTableArray0(""tab2_InnerTable"",""tab2_DataTable_Summary"",""tab2_jQueryTable"", " & AllowExport.ToString.ToLower & ");"
                    End If

                Case "portfolio_tabPanel3"
                    If features_dropdown.SelectedValue = -1 Then
                        functionName = "CreateSearchTableArray0(""tab3_InnerTable"",""tab3_DataTable_Summary"",""tab3_jQueryTable"", " & AllowExport.ToString.ToLower & ");"
                    Else
                        functionName = "CreateSearchTableArray0(""tab3_InnerTable"",""tab3_DataTable"",""tab3_jQueryTable"", " & AllowExport.ToString.ToLower & ");"
                    End If


                Case "portfolio_tabPanel4"
                    functionName = "CreateSearchTableArray0(""tab4_InnerTable"",""tab4_DataTable"",""tab4_jQueryTable"", " & AllowExport.ToString.ToLower & ");"
                Case "portfolio_tabPanel5"
                    functionName = "CreateSearchTableArray0(""tab5_InnerTable"",""tab5_DataTable"",""tab5_jQueryTable"", " & AllowExport.ToString.ToLower & ");"
                Case "portfolio_tabPanel6"
                    functionName = "CreateSearchTableArray0(""tab6_InnerTable"",""tab6_DataTable"",""tab6_jQueryTable"", " & AllowExport.ToString.ToLower & ");"
                Case "portfolio_tabPanel7"
                    functionName = "CreateSearchTableArray0(""tab7_InnerTable"",""tab7_DataTable"",""tab7_jQueryTable"", " & AllowExport.ToString.ToLower & ");"
                Case "portfolio_tabPanel8"

                    If Trim(comp_id_list) <> "" Or Trim(companyID) > 0 Then
                        functionName = "CreateSearchTableArray0(""tab8_InnerTable"",""tab8_DataTable"",""tab8_jQueryTable"", " & AllowExport.ToString.ToLower & ");"
                    Else
                        functionName = "CreateSearchTableArray0(""tab8_InnerTable"",""tab8_DataTable_folder"",""tab8_jQueryTable"", " & AllowExport.ToString.ToLower & ");"
                    End If
                Case "portfolio_tabPanel9"

                    If location_drop.SelectedValue = 0 Then
                        functionName = "CreateSearchTableArray0(""tab9_InnerTable"",""tab9_DataTable"",""tab9_jQueryTable"", " & AllowExport.ToString.ToLower & ");"
                    Else
                        functionName = "CreateSearchTableArray0(""tab9_InnerTable"",""tab9_DataTable_Summary"",""tab9_jQueryTable"", " & AllowExport.ToString.ToLower & ");"
                    End If

            End Select

        End If

        'If PageNeedUpdate = False Then
        '  JavascriptOnLoad += vbCrLf + "prm.remove_endRequest(function (s, e) {"
        '  JavascriptOnLoad += functionName
        '  JavascriptOnLoad += vbCrLf + "});"
        'End If
        'JavascriptOnLoad += vbCrLf + "prm.add_endRequest(function (s, e) {"
        JavascriptOnLoad += functionName
        'JavascriptOnLoad += vbCrLf + "});"



        'If PageNeedUpdate Then
        JavascriptOnLoad += vbCrLf + "CloseLoadingMessage(""DivLoadingMessage"");$('#" & portfolio_tabContainer.ClientID & "').removeClass('display_none');ChangeTheMouseCursorOnItemParentDocument('standalone_page');"
        'End If


        If Page.IsPostBack Then
            ' If Not Page.ClientScript.IsClientScriptBlockRegistered("onLoadCode") Then
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me.searchUpdate, Me.GetType(), "onLoadCode", JavascriptOnLoad.ToString, True)
            'End If
        End If


    End Sub
    Sub SetUpLinksInHeader()
        Dim strResult As String = "<div class=""row"">"
        Dim x As Integer = 0
        Dim firstPassThrough As Boolean = True
        For Each r As ListItem In user_portfolio_list.Items
            If Not String.IsNullOrEmpty(r.Text) Then
                If x = 4 Then
                    strResult += "</div>"
                    strResult += "<div class=""row"">"
                    x = 1
                    firstPassThrough = False
                Else
                    x += 1
                End If
                strResult += "<div class=""columns three""><a href=""javascript:void(0);"" class=""display_block padded portfolioLinks"" onclick=""javascript:clearAllTextboxes();SelectDropDownItem('" & r.Value.ToString & "');"">" & r.Text.ToString & "</a></div>"
            End If
        Next
        user_portfolio_lbl.Text = strResult
    End Sub
    Sub TurnOffTabPanels(ByVal excludeInteger As Integer)
        Dim JavascriptOnLoad As String = ""
        If excludeInteger <> 0 Then
            JavascriptOnLoad += vbCrLf + "$('#" & portfolio_tab_0_graphs.ClientID & "').hide();"
        Else
            JavascriptOnLoad += vbCrLf + "$('#" & portfolio_tab_0_graphs.ClientID & "').show();"
        End If
        If excludeInteger <> 1 Then
            JavascriptOnLoad += vbCrLf + "$('#" & portfolio_tab_1_graphs.ClientID & "').hide();"
        Else
            JavascriptOnLoad += vbCrLf + "$('#" & portfolio_tab_1_graphs.ClientID & "').show();"
        End If
        If excludeInteger <> 2 Then
            JavascriptOnLoad += vbCrLf + "$('#" & portfolio_tab_2_graphs.ClientID & "').hide();"
        Else
            JavascriptOnLoad += vbCrLf + "$('#" & portfolio_tab_2_graphs.ClientID & "').show();"
        End If
        If excludeInteger <> 3 Then
            JavascriptOnLoad += vbCrLf + "$('#" & portfolio_tab_3_graphs.ClientID & "').hide();"
        Else
            JavascriptOnLoad += vbCrLf + "$('#" & portfolio_tab_3_graphs.ClientID & "').show();"
        End If
        If excludeInteger <> 4 Then
            JavascriptOnLoad += vbCrLf + "$('#" & portfolio_tab_4_graphs.ClientID & "').hide();"
        Else
            JavascriptOnLoad += vbCrLf + "$('#" & portfolio_tab_4_graphs.ClientID & "').show();"
        End If
        If excludeInteger <> 5 Then
            JavascriptOnLoad += vbCrLf + "$('#" & portfolio_tab_5_graphs.ClientID & "').hide();"
        Else
            JavascriptOnLoad += vbCrLf + "$('#" & portfolio_tab_5_graphs.ClientID & "').show();"
        End If
        If excludeInteger <> 6 Then
            JavascriptOnLoad += vbCrLf + "$('#" & portfolio_tab_6_graphs.ClientID & "').hide();"
        Else
            JavascriptOnLoad += vbCrLf + "$('#" & portfolio_tab_6_graphs.ClientID & "').show();"
        End If
        If excludeInteger <> 7 Then
            JavascriptOnLoad += vbCrLf + "$('#" & portfolio_tab_7_graphs.ClientID & "').hide();"
        Else
            JavascriptOnLoad += vbCrLf + "$('#" & portfolio_tab_7_graphs.ClientID & "').show();"
        End If

        If excludeInteger <> 8 Then
            JavascriptOnLoad += vbCrLf + "$('#" & portfolio_tab_8_graphs.ClientID & "').hide();"
        Else
            JavascriptOnLoad += vbCrLf + "$('#" & portfolio_tab_8_graphs.ClientID & "').show();"
        End If

        If excludeInteger <> 9 Then
            JavascriptOnLoad += vbCrLf + "$('#" & portfolio_tab_9_graphs.ClientID & "').hide();"
        Else
            JavascriptOnLoad += vbCrLf + "$('#" & portfolio_tab_9_graphs.ClientID & "').show();"

        End If

        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me.searchUpdate, Me.GetType(), "tabsOff", JavascriptOnLoad.ToString & ";", True)
    End Sub
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

        Try


        Catch ex As Exception
            If Not IsNothing(masterPage) Then
                masterPage.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString & " (Page_PreRender): " & ex.Message.ToString)
            Else
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += " (Page_PreRender): " + ex.Message.ToString.Trim
            End If
        End Try

    End Sub


    'Private Sub atGlanceClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles atGlanceClear.Click
    '  localCriteria.ViewCriteriaFolderName = ""
    '  user_portfolio_list.SelectedValue = ""
    'End Sub


End Class