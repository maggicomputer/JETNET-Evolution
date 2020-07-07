' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/FAAFlightData.aspx.vb $
'$$Author: Amanda $
'$$Date: 6/17/20 12:45p $
'$$Modtime: 6/17/20 9:04a $

'$$Revision: 21 $
'$$Workfile: FAAFlightData.aspx.vb $
'
' ********************************************************************************

Partial Public Class FAAFlightData

    Inherits System.Web.UI.Page

    Private airframe_times_as_of As String = ""
    Private airframe_total_hours As String = ""
    Private airframe_total_landings As String = ""

    Dim AclsData_Temp As New clsData_Manager_SQL
    Dim flight_data_temp As New flightDataFunctions


    Private bHasNoBlankAcFieldsCookie As Boolean = False
    Private bShowBlankAcFields As Boolean = False
    Dim comp_functions As New CompanyFunctions
    Dim aport_id1 As Long = 0
    Dim aport_id2 As Long = 0
    Dim aport_name1 As String = ""
    Dim aport_name2 As String = ""
    Dim total_flights_count As Long = 0
    Dim product_code As String = ""
    Dim temp_code_string As String = ""
    Dim routeAnalysis As Boolean = False
    Dim temp_reg As String = ""
    Dim comp_id As Long = 0
    Dim last_aport_id As Long = 0
    Dim last_aport_lat As String = ""
    Dim last_aport_long As String = ""

    Dim Flight_Id1 As String = ""
    Dim Flight_Id2 As String = ""
    Dim Origin_Aport_id As Long = 0
    Dim Dest_Aport_id As Long = 0
    Dim last_distance As Integer = 0


    Private Sub FillUpOriginDestinationBoxes(ByVal aportID As Long, ByVal adjustBox As TextBox, ByVal adjustBoxID As TextBox, ByRef localDataLayer As viewsDataLayer)
        Dim AirportInfo As DataTable

        AirportInfo = localDataLayer.get_airports_by_IATA_or_ICAO_City_Name("", "", "", "", aportID)

        If Not IsNothing(AirportInfo) Then
            If AirportInfo.Rows.Count > 0 Then
                adjustBox.Text = AirportInfo.Rows(0).Item("aport_name")

                If Not IsDBNull(AirportInfo.Rows(0).Item("aport_city")) Then
                    adjustBox.Text += " (" & AirportInfo.Rows(0).Item("aport_city") & ") "
                End If
                If Not IsDBNull(AirportInfo.Rows(0).Item("aport_iata_code")) Then
                    adjustBox.Text += " - " & AirportInfo.Rows(0).Item("aport_iata_code")
                End If
                If Not IsDBNull(AirportInfo.Rows(0).Item("aport_icao_code")) Then
                    adjustBox.Text += " - " & AirportInfo.Rows(0).Item("aport_icao_code")
                End If
                adjustBoxID.Text = AirportInfo.Rows(0).Item("aport_id")

            End If
        End If
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim sErrorString As String = ""
        Dim sRegNumber As String = "" '"N895CC"
        Dim nAircraftID As Long = 0 '7177

        Dim tmpFlightDataTable As DataTable = Nothing
        Dim tmpTripDataTable As DataTable = Nothing
        Dim tmpAircraftInfoTable As DataTable = Nothing
        Dim tmpAircraftSummaryTable As DataTable = Nothing
        Dim tmpAircraftSummaryTable2 As DataTable = Nothing
        Dim tmpAircraftSummaryTable3 As DataTable = Nothing
        Dim purcahse_date As String = ""
        Dim start_date As String = ""
        Dim flightPathArray(,) As String = Nothing
        Dim localDataLayer As New viewsDataLayer

        Dim PreviouslyOwnedFlag As String = ""
        Dim temp_amod_id As Long = 0

        Dim temp_label_sting As String = ""
        Dim activetab As Integer = 0
        Dim show_one_way As Boolean = False
        Dim StartDateDisplay As String = ""
        Dim EndDateDisplay As String = ""
        Dim Export_Flights As Boolean = False




        Dim tmp_distance_table1 As DataTable = Nothing
        Dim tmp_distance_table2 As DataTable = Nothing

        Dim comp_name As String = ""
        Export_Flights = exportUtilization.Checked

        If Export_Flights Then
            main_tab_container.ActiveTabIndex = 0 'We're adding this here so that if they export the flights, it swaps them to the flight tab. This is just to keep the code more simple and avoid running queries to recreate the datatable shown.
        End If

        Try

            AclsData_Temp = New clsData_Manager_SQL
            AclsData_Temp.JETNET_DB = Session.Item("jetnetClientDatabase")
            AclsData_Temp.client_DB = Session.Item("jetnetServerNotesDatabase")

            flight_data_temp = New flightDataFunctions
            flight_data_temp.serverConnectStr = Session.Item("jetnetClientDatabase")
            flight_data_temp.clientConnectStr = Session.Item("jetnetServerNotesDatabase")

            localDataLayer.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim

            Master.RemoveSizes(True)
            Master.TurnOffPageHeader(True)
            Master.SetContainerClass("container MaxWidthRemove") 'set full width page

            If Session.Item("crmUserLogon") <> True Then

                Response.Redirect("Default.aspx", False)

            Else

                If Not Session.Item("localPreferences").loadUserSession(sErrorString, CLng(Session.Item("localUser").crmSubSubID.ToString), Session.Item("localUser").crmUserLogin.ToString, CLng(Session.Item("localUser").crmSubSeqNo.ToString), CLng(Session.Item("localUser").crmUserContactID.ToString)) Then
                    Response.Write("error in load flight data : " + sErrorString)
                End If

                Dim fSubins_platform_os As String = commonEvo.getBrowserCapabilities(Request.Browser)

                If Not IsNothing(Request.Item("regNumber")) Then
                    If Not String.IsNullOrEmpty(Request.Item("regNumber").ToString.Trim) Then
                        sRegNumber = Request("regNumber").ToString.Trim
                    End If
                End If

                If Not IsNothing(Request("analysis")) Then
                    If Trim(Request("analysis")) = "true" Then
                        routeAnalysis = True
                    End If
                End If
                ' added in MSW 
                If Not IsNothing(Request("comp_id")) Then
                    comp_id = Trim(Request("comp_id"))
                    comp_name = commonEvo.get_company_name_fromID(CLng(comp_id), 0, False, True, "").Trim()
                    company_name_label.Text = "<font class='" & HttpContext.Current.Session("FONT_CLASS_HEADER") & " mainHeading'><span><strong id='textToChangeOnTab'>" & comp_name & "</strong></span></font><br/>"
                    company_name_label.Visible = True
                End If


                If Not IsNothing(Request.Item("acid")) Then
                    If Not String.IsNullOrEmpty(Request.Item("acid").ToString.Trim) Then
                        nAircraftID = CLng(Request("acid").ToString.Trim)
                    End If
                End If

                If Not IsNothing(Request.Item("activetab")) Then
                    If Not String.IsNullOrEmpty(Request.Item("activetab").ToString.Trim) Then
                        activetab = CLng(Request("activetab").ToString.Trim)
                    End If
                End If

                '---------------- ADDING IN MSW - FOR THE REFUEL MAP FVIEW----------------------------------------
                If Not IsNothing(Request.Item("Flight_Id1")) Then
                    If Not String.IsNullOrEmpty(Request.Item("Flight_Id1").ToString.Trim) Then
                        Flight_Id1 = Request("Flight_Id1").ToString.Trim
                    End If
                End If

                If Not IsNothing(Request.Item("Flight_Id2")) Then
                    If Not String.IsNullOrEmpty(Request.Item("Flight_Id2").ToString.Trim) Then
                        Flight_Id2 = Request("Flight_Id2").ToString.Trim
                    End If
                End If

                If Not IsNothing(Request.Item("Origin_Aport_id")) Then
                    If Not String.IsNullOrEmpty(Request.Item("Origin_Aport_id").ToString.Trim) Then
                        Origin_Aport_id = Request("Origin_Aport_id")
                    End If
                End If

                If Not IsNothing(Request.Item("Dest_Aport_id")) Then
                    If Not String.IsNullOrEmpty(Request.Item("Dest_Aport_id").ToString.Trim) Then
                        Dest_Aport_id = Request("Dest_Aport_id")
                    End If
                End If


                If Trim(Flight_Id1) <> "" Then   ' if we r in the refuel tech stop comparison
                    Dim aCookie As New HttpCookie("Refuel_Flight_Check")
                    aCookie.Value = ""
                    aCookie.Expires = DateTime.Now.AddDays(365)
                    Response.Cookies.Add(aCookie)

                    If search_flight_checkbox.Visible = False Then   ' then its our first time in  
                        If Not IsNothing(Request.Cookies("Refuel_Flight_Check")) Then   ' load it if we have one 
                            If Trim(Request.Cookies("Refuel_Flight_Check").Value) <> "" Then
                                iata_icao_search.Text = Trim(Request.Cookies("Refuel_Flight_Check").Value)
                                HttpContext.Current.Response.Cookies("Refuel_Flight_Check").Value = iata_icao_search.Text
                                Me.search_flight_checkbox.Checked = True
                            End If
                        End If
                    ElseIf search_flight_checkbox.Checked = True Then   'if we have "searched" or checked, then save it 
                        If Trim(iata_icao_search.Text) <> "" Then
                            HttpContext.Current.Response.Cookies("Refuel_Flight_Check").Value = iata_icao_search.Text
                        Else
                            HttpContext.Current.Response.Cookies("Refuel_Flight_Check").Value = ""
                        End If
                    End If
                End If

                '--------------------------------------------------------


                If Not Page.IsPostBack Then
                    If Not IsNothing(Request.Item("aport_id1")) Then
                        If Not String.IsNullOrEmpty(Request.Item("aport_id1").ToString.Trim) Then
                            aport_id1 = CLng(Request("aport_id1").ToString.Trim)
                            route_id.Text = aport_id1
                        End If
                    End If

                    If Not IsNothing(Request.Item("aport_id2")) Then
                        If Not String.IsNullOrEmpty(Request.Item("aport_id2").ToString.Trim) Then
                            aport_id2 = CLng(Request("aport_id2").ToString.Trim)
                            destination_id.Text = aport_id2
                        End If
                    End If
                End If



                If Not String.IsNullOrEmpty(route_id.Text) Then
                    If IsNumeric(route_id.Text) Then
                        aport_id1 = route_id.Text
                        routeAnalysis = True
                    End If
                End If
                If Not String.IsNullOrEmpty(destination_id.Text) Then
                    If IsNumeric(destination_id.Text) Then
                        aport_id2 = destination_id.Text
                        routeAnalysis = True
                    End If
                End If



                dateSearchBoxes.Visible = True
                If routeAnalysis Or nAircraftID = 0 Then
                    ownerWidth.Visible = False
                    ownerLabelCell.Visible = False

                    'DropDownList_timeframe.Items.Remove(DropDownList_timeframe.Items.FindByValue("current"))
                    'DropDownList_timeframe.Items.Remove(DropDownList_timeframe.Items.FindByValue("all"))
                    'DropDownList_timeframe.Items.Remove(DropDownList_timeframe.Items.FindByValue("current"))
                    'DropDownList_timeframe_dup.Items.Remove(DropDownList_timeframe_dup.Items.FindByValue("all"))
                    If Page.IsPostBack Then
                        If Not Page.ClientScript.IsClientScriptBlockRegistered("autocomplete") Then
                            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me.flight_data_update, Me.GetType(), "autocomplete", "setUpAutoComplete();", True)
                        End If
                    End If

                End If


                aircraftInformationText.Visible = True
                If (routeAnalysis = True And Not Page.IsPostBack) Then


                    If aport_id1 > 0 And aport_id2 > 0 Then 'This means coming to the page with aport through query string.
                        FillUpOriginDestinationBoxes(aport_id1, route, route_id, localDataLayer)
                        FillUpOriginDestinationBoxes(aport_id2, destination, destination_id, localDataLayer)
                    End If

                    'dateSearchBoxes.Visible = False
                    flight_search_options.Visible = True 'False
                    'aircraftInformationText.Visible = False
                    route_analysis_panel.Visible = True
                    DropDownList_owner.Visible = False

                    owner_text.Visible = False
                    routes_search_panel.Visible = True
                    routes_search_panel.CssClass = "display_none"
                    endDateWidth.Width = "70"
                    search_date_range.CssClass = "display_none"
                    main_tab_container.CssClass += " display_none"
                ElseIf aport_id1 > 0 And aport_id2 > 0 And nAircraftID = 0 Then
                    main_tab_container.CssClass = "dark-theme"
                    'If Not IsPostBack Then
                    '  DropDownList_timeframe.Items.RemoveAt(2)
                    '  DropDownList_timeframe.Items.RemoveAt(2)

                    '  DropDownList_timeframe_dup.Items.RemoveAt(2)
                    '  DropDownList_timeframe_dup.Items.RemoveAt(2)
                    'End If
                    ownerLabelCell.Visible = False
                    ownerWidth.Visible = False
                    Me.owner_text.Visible = False
                    Me.DropDownList_owner.Visible = False
                    Me.origins_tab.Visible = False
                    Me.destinations_tab.Visible = False
                    Me.airframe_estimates_tab.Visible = False
                    Me.routes_tab.Visible = False
                    Me.pairs_tab.Visible = False
                    Me.aircraft_tab.Visible = True
                    Me.operators_tab.Visible = True
                    If routeAnalysis = False Then
                        Me.routes_search_panel.Visible = True
                    End If
                    Me.Flights_total_label.Visible = True


                    aport_name1 = flight_data_temp.GetAirportName(aport_id1)
                    aport_name2 = flight_data_temp.GetAirportName(aport_id2)

                    If Not IsPostBack Then
                        Me.route_selection.Items.Add(New ListItem("Origin " & aport_name1, 1))
                        Me.route_selection.Items.Add(New ListItem("Origin " & aport_name2, 2))

                        If Not IsNothing(Request.Item("orig_direction")) Then
                            If Not String.IsNullOrEmpty(Request.Item("orig_direction").ToString.Trim) Then
                                If Trim(Request("orig_direction")) = "1" Then
                                    Me.route_selection.SelectedValue = 1
                                ElseIf Trim(Request("orig_direction")) = "2" Then
                                    Me.route_selection.SelectedValue = 2
                                End If
                            End If
                        End If

                    End If



                    If routeAnalysis Then
                        Me.route_selection.Items.Add(New ListItem("Origin " & aport_name1, 1))
                        Me.route_selection.Items.Add(New ListItem("Origin " & aport_name2, 2))
                        If checkboxBoth.Checked Then
                            route_selection.SelectedValue = 0 'if checkbox is checked, show both ways
                        Else
                            route_selection.SelectedValue = 1 'otherwise first is origin
                        End If
                    End If

                    ' switch them, easier for selection 
                    If Me.route_selection.SelectedValue = 2 Then
                        nAircraftID = aport_id1
                        aport_id1 = aport_id2
                        aport_id2 = nAircraftID
                        nAircraftID = 0
                    End If

                    If Me.route_selection.SelectedValue = 1 Or Me.route_selection.SelectedValue = 2 Then
                        show_one_way = True
                    End If
                End If

                If Session.Item("isMobile") Then
                    activity_tab.HeaderText = "Util."
                    airframe_estimates_tab.HeaderText = "AIRF EST."
                    destinations_tab.HeaderText = "DEST."
                    origins_tab.HeaderText = "ORIG."
                    flight_activity_tab.HeaderText = "FLIGHT ACT."
                End If

                bShowBlankAcFields = commonEvo.getUserShowBlankACFields(Session.Item("ShowCondensedAcFormat"), bHasNoBlankAcFieldsCookie)
                Master.SetPageTitle("Flight Data")  ' sets the page title and page.text

                If route_selection.SelectedValue = 1 Then
                    Master.SetPageTitle("Route Analysis " & aport_name1 & " To " & aport_name2)
                ElseIf route_selection.SelectedValue = 2 Then
                    Master.SetPageTitle("Route Analysis " & aport_name2 & " To " & aport_name1)
                Else
                    Master.SetPageTitle("Route Analysis " & aport_name1 & " To/From " & aport_name2)
                End If
                'Me.route_selection.Items.Add(New ListItem("Origin " & aport_name1, 1))
                'Me.route_selection.Items.Add(New ListItem("Origin " & aport_name2, 2))


                Master.SetPageText("")
                ' use this mainly to get reg number
                If flight_data_temp.checkForFAAFlightData(sRegNumber, nAircraftID, True) = True Or Trim(sRegNumber) = "" Then






                    'Setting PreviouslyOwned Flag
                    If Not IsNothing(tmpAircraftInfoTable) Then
                        If tmpAircraftInfoTable.Rows.Count > 0 Then
                            If Not IsDBNull(tmpAircraftInfoTable.Rows(0).Item("ac_previously_owned_flag")) Then
                                PreviouslyOwnedFlag = tmpAircraftInfoTable.Rows(0).Item("ac_previously_owned_flag").ToString.ToUpper()
                            End If '
                        End If
                    End If

                    If Not IsNothing(tmpAircraftInfoTable) Then
                        If tmpAircraftInfoTable.Rows.Count > 0 Then
                            If Not IsDBNull(tmpAircraftInfoTable.Rows(0).Item("ac_amod_id")) Then
                                temp_amod_id = tmpAircraftInfoTable.Rows(0).Item("ac_amod_id").ToString.ToUpper()
                            End If '
                        End If
                    End If




                    If Trim(DropDownList_timeframe.SelectedValue) = "90_days" Then
                        start_date = FormatDateTime(DateAdd(DateInterval.Month, -3, Date.Now.Date), DateFormat.ShortDate)
                    ElseIf Trim(DropDownList_timeframe.SelectedValue) = "last_year" Then
                        start_date = FormatDateTime(DateAdd(DateInterval.Month, -12, Date.Now.Date), DateFormat.ShortDate)
                    ElseIf Trim(DropDownList_owner.SelectedValue) = "current" Or Trim(DropDownList_timeframe.SelectedValue) = "current" Then
                        start_date = purcahse_date
                    ElseIf Trim(DropDownList_timeframe.SelectedValue) = "avll" Then
                        start_date = ""
                    Else
                        start_date = FormatDateTime(DateAdd(DateInterval.Day, -90, Date.Now.Date), DateFormat.ShortDate)
                    End If

                    DropDownList_timeframe.Text = "date_search"
                    DropDownList_timeframe_dup.Text = "date_search"
                    Me.flight_aware_label.Visible = False
                    flightAwareCell.Visible = False
                    If Not IsPostBack Then
                        If Trim(activetab) > 0 Then
                            main_tab_container.ActiveTabIndex = activetab
                        End If

                        If Trim(Request("start_date")) <> "" Then
                            Me.faa_start_date.Text = Trim(Request("start_date"))
                        End If

                        If Trim(Request("end_date")) <> "" Then
                            Me.faa_end_date.Text = Trim(Request("end_date"))
                        End If

                    End If

                    If nAircraftID = 0 Then
                        SetUpStartEndDates(StartDateDisplay, EndDateDisplay, "")

                    End If

                    If nAircraftID > 0 Then
                        'live_flights_tab.Visible = True ' added MSW  
                        tmpAircraftInfoTable = commonEvo.GetAircraftInfo_dataTable(nAircraftID)
                        aircraftInformationText.Text = displayAircraftInfo(tmpAircraftInfoTable, purcahse_date, temp_amod_id) ' sets page text to somthing different than page title

                    ElseIf Me.route_selection.SelectedValue = 0 Then
                        If routeAnalysis = True Then
                            If Page.IsPostBack Then
                                toFromLabel.Text = "To/From<br />"
                                toFromLabel.Visible = True
                            End If
                            aircraftInformationText.Text = "<font class='" & HttpContext.Current.Session("FONT_CLASS_HEADER") & " mainHeading'><span><strong id='textToChangeOnTab'>" & main_tab_container.ActiveTab.HeaderText & "</strong></span></font>"
                        Else
                            aircraftInformationText.Text = "<font class='" & HttpContext.Current.Session("FONT_CLASS_HEADER") & " mainHeading'><strong>" & aport_name1 & "</strong> To/From " & aport_name2 & "<br /><span><strong id='textToChangeOnTab'>" & main_tab_container.ActiveTab.HeaderText & "</strong></span></font>"
                        End If

                    ElseIf Me.route_selection.SelectedValue = 1 Then
                        If routeAnalysis = True Then
                            If Page.IsPostBack Then
                                toFromLabel.Text = "To<br />"
                                toFromLabel.Visible = True
                            End If
                            aircraftInformationText.Text = "<font class='" & HttpContext.Current.Session("FONT_CLASS_HEADER") & " mainHeading'><span><strong id='textToChangeOnTab'>" & main_tab_container.ActiveTab.HeaderText & "</strong></span></font>"
                        Else
                            aircraftInformationText.Text = "<font class='" & HttpContext.Current.Session("FONT_CLASS_HEADER") & " mainHeading'><strong>" & aport_name1 & "</strong> To " & aport_name2 & "<br /><span><strong id='textToChangeOnTab'>" & main_tab_container.ActiveTab.HeaderText & "</strong></span></font>"
                        End If
                    ElseIf Me.route_selection.SelectedValue = 2 Then
                        If routeAnalysis = True Then
                            If Page.IsPostBack Then
                                toFromLabel.Text = "To<br />"
                                toFromLabel.Visible = True
                            End If
                            aircraftInformationText.Text = "<font class='" & HttpContext.Current.Session("FONT_CLASS_HEADER") & " mainHeading'><span><strong id='textToChangeOnTab'>" & main_tab_container.ActiveTab.HeaderText & "</strong></span></font>"
                        Else
                            aircraftInformationText.Text = "<font class='" & HttpContext.Current.Session("FONT_CLASS_HEADER") & " mainHeading'><strong>" & aport_name2 & "</strong> To " & aport_name1 & "<br /><span><strong id='textToChangeOnTab'>" & main_tab_container.ActiveTab.HeaderText & "</strong></span></font>"
                        End If
                    End If


                    ' MOVED HERE FROM ABOVE 
                    If Not IsNothing(Request.Item("pc")) Then
                        If Not String.IsNullOrEmpty(Request.Item("pc").ToString.Trim) Then
                            product_code = Request("pc").ToString.Trim


                            If Trim(product_code) = "B,C" Then
                                product_code = " and (ac_product_business_flag = 'Y' or  ac_product_commercial_flag = 'Y') "
                                temp_code_string = "Business and Commercial Aircraft"
                            ElseIf Trim(product_code) = "B,H" Then
                                product_code = " and (ac_product_business_flag = 'Y' or  ac_product_helicopter_flag = 'Y') "
                                temp_code_string = "Business Aircraft and Helicopters"
                            ElseIf Trim(product_code) = "C,H" Then
                                product_code = " and (ac_product_commercial_flag = 'Y' or  ac_product_helicopter_flag = 'Y') "
                                temp_code_string = "Helicopters and Commercial Aircraft"
                            ElseIf Trim(product_code) = "B" Then
                                product_code = " and ac_product_business_flag = 'Y'"
                                temp_code_string = "Business Aircraft"
                            ElseIf Trim(product_code) = "C" Then
                                product_code = " and ac_product_commercial_flag = 'Y'"
                                temp_code_string = "Commercial Aircraft"
                            ElseIf Trim(product_code) = "H" Then
                                product_code = " and ac_product_helicopter_flag = 'Y'"
                                temp_code_string = "Helicopters"
                            End If

                            aircraftInformationText.Text = Replace(aircraftInformationText.Text, "</table>", "<tr><td align='left'>" & temp_code_string & "</td></tr></table>")

                        End If
                    End If
                    ' aircraftInformationText.Text


                    AlignTheHeaderLabels()

                    If nAircraftID = 0 And aport_id1 > 0 And aport_id2 > 0 Then
                        Call commonLogFunctions.Log_User_Event_Data("UserStatistics", Replace("Route Analysis: " & aport_name1 & " to " & aport_name2, "'", ""), Nothing, 0, 0, 0, 0, 0, 0, 0)
                    ElseIf nAircraftID > 0 And Not Page.IsPostBack Then
                        Call commonLogFunctions.Log_User_Event_Data("UserStatistics", Replace("Route Analysis: AC_ID = " + nAircraftID.ToString + " RegNo = " + sRegNumber.Trim, "'", ""), Nothing, 0, 0, 0, 0, 0, nAircraftID.ToString, temp_amod_id)
                    End If


                    If flight_data_temp.IS_ON_BLOCKED_LIST(sRegNumber) Then

                        If Not IsPostBack And Trim(activetab) = 0 Then
                            Me.map_panel.Visible = False
                            Me.flight_data.Text = "Detailed flight data for this aircraft (REG#" & sRegNumber & ") is not available for public viewing based on the request of the owner/operator."
                            Me.origins_tab_data.Text = "Detailed flight data foevr this aircraft (REG#" & sRegNumber & ") is not available for public viewing based on the request of the owner/operator."
                            Me.destinations_tab_data.Text = "Detailed flight data for this aircraft (REG#" & sRegNumber & ") is not available for public viewing based on the request of the owner/operator."
                            main_tab_container.ActiveTabIndex = 3
                            airframe_estimates_tab_data.Text = flight_data_temp.displayAirframeTimesData(flight_data_temp.getAllFAAFlightData(sRegNumber, nAircraftID, airframe_times_as_of), airframe_times_as_of, airframe_total_hours, airframe_total_landings, False, PreviouslyOwnedFlag, purcahse_date, bShowBlankAcFields)


                        ElseIf main_tab_container.ActiveTabIndex = 4 Then

                            Call flight_data_temp.make_ac_flights_comparisons(nAircraftID, Me.chart_label.Text, DropDownList_owner, DropDownList_timeframe, temp_amod_id, Me.faa_start_date, Me.faa_end_date, start_date, sRegNumber, aport_id1, aport_id2, show_one_way, aport_name1, aport_name2, total_flights_count, Me.Page, Me.chart_panel, Me.map_panel, "")
                            'tmpAircraftSummaryTable = Nothing
                            'tmpAircraftSummaryTable2 = Nothing
                            'tmpAircraftSummaryTable = flight_data_temp.get_chart_data_activity_summary(nAircraftID, DropDownList_owner, DropDownList_timeframe, temp_amod_id, nAircraftID)
                            'tmpAircraftSummaryTable2 = flight_data_temp.get_chart_data_activity_summary(0, DropDownList_owner, DropDownList_timeframe, temp_amod_id, nAircraftID)

                            'Call make_google_summary_chart(tmpAircraftSummaryTable, nAircraftID, start_date, tmpAircraftSummaryTable2)

                            'Call make_chart_text(tmpAircraftSummaryTable, tmpAircraftSummaryTable2, start_date, sRegNumber, DropDownList_timeframe.SelectedValue)

                        End If

                    Else


                        If DropDownList_timeframe.Text = "date_search" Then
                            date_search_box.Visible = True

                            'If Not Page.IsPostBack Then
                            Dim jsString As String = " $(function() {"
                                jsString += "$(""#" & faa_start_date.ClientID & """).datepicker({"
                                jsString += " showOn: ""button"", "
                                jsString += " buttonImage: ""/images/final.jpg"","
                                jsString += " buttonImageOnly: true,"
                                jsString += " buttonText: ""Select date"""
                                jsString += " });"
                                jsString += "$(""#" & faa_end_date.ClientID & """).datepicker({"
                                jsString += " showOn: ""button"", "
                                jsString += " buttonImage: ""/images/final.jpg"","
                                jsString += " buttonImageOnly: true,"
                                jsString += " buttonText: ""Select date"""
                                jsString += " });"
                                jsString += " } );"
                                System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType, "dateString", jsString, True)
                            'End If


                            If Trim(Me.faa_start_date.Text) = "" And Trim(Me.faa_end_date.Text) = "" Then
                                Me.faa_start_date.Text = FormatDateTime(DateAdd(DateInterval.Month, -3, Date.Now()), DateFormat.ShortDate)
                                Me.faa_end_date.Text = FormatDateTime(Date.Now(), DateFormat.ShortDate)
                            End If
                        Else
                            date_search_box.Visible = False
                        End If

                        last_aport_id = 0
                        last_aport_lat = ""
                        last_aport_long = ""
                        Call flight_data_temp.checkForFAAFlightData_Last_Aport(sRegNumber, nAircraftID, last_aport_id, last_aport_lat, last_aport_long)



                        If nAircraftID > 0 Or (aport_id1 > 0 And aport_id2 > 0 And Page.IsPostBack) Then

                            If main_tab_container.ActiveTabIndex = 0 Or main_tab_container.ActiveTabIndex = 10 Then  ' FLIGHT MAP PAGE AS WELL

                                If flight_data_temp.checkForFAAFlightData(sRegNumber, nAircraftID) Or Trim(sRegNumber) = "" Then

                                    ' if it is date search, do the last search so that it shows map and everything still 
                                    If DropDownList_timeframe.Text = "date_search" Then
                                        If Trim(DropDownList_timeframe_dup.Text) = Trim(DropDownList_timeframe.Text) Then ' if they r equal then we have most likely done a search 
                                            tmpFlightDataTable = flight_data_temp.getFAAFlightData_View_Simple(sRegNumber, nAircraftID, DropDownList_owner, DropDownList_timeframe, False, Me.faa_start_date.Text, Me.faa_end_date.Text, aport_id1, aport_id2, show_one_way, 0, False, product_code, False, False, comp_id, Flight_Id1, Flight_Id2)
                                        Else ' if they are not equal, then dont use text as search 
                                            tmpFlightDataTable = flight_data_temp.getFAAFlightData_View_Simple(sRegNumber, nAircraftID, DropDownList_owner, DropDownList_timeframe_dup, False, "", "", aport_id1, aport_id2, show_one_way, 0, False, product_code, False, False, comp_id, Flight_Id1, Flight_Id2)
                                        End If
                                    Else
                                        tmpFlightDataTable = flight_data_temp.getFAAFlightData_View_Simple(sRegNumber, nAircraftID, DropDownList_owner, DropDownList_timeframe, False, "", "", aport_id1, aport_id2, show_one_way, 0, False, product_code, False, False, comp_id, Flight_Id1, Flight_Id2)
                                    End If


                                    If Not IsNothing(tmpFlightDataTable) Then

                                        If Trim(Flight_Id1) <> "" And Trim(Me.iata_icao_search.Text) <> "" And Me.search_flight_checkbox.Checked = True Then
                                            Call get_distance_flights_tables(tmp_distance_table1, tmp_distance_table2, Origin_Aport_id, Dest_Aport_id)
                                        End If

                                        getFlightPathsArray(tmpFlightDataTable, flightPathArray, tmp_distance_table1, tmp_distance_table2)


                                        If nAircraftID > 0 Or aport_id1 > 0 Or aport_id2 > 0 Then
                                            If DropDownList_timeframe.Text = "date_search" Then
                                                If Trim(DropDownList_timeframe_dup.Text) = Trim(DropDownList_timeframe.Text) Then ' if they r equal then we have most likely done a search
                                                    Dim ColumnList As String = ""
                                                    Dim arrayStr As String = ""

                                                    If HttpContext.Current.Session.Item("localUser").crmDemoUserFlag = True Then ' ADDED MSW - 3/27/20
                                                    Else
                                                        exportFlightsLink.Attributes.Remove("class")
                                                        exportFlightsLink.Attributes.Add("class", "float_right") 'removing the noBefore class for aircraft
                                                        exportFlightsLink.Attributes.Add("onClick", "$('#" & exportUtilization.ClientID & "').prop('checked', true);$('.searchResultsLoadingText').text('Exporting all flights to Excel. This may take a few minutes. Please wait...');$('#" & searchFlight.ClientID & "').click();")
                                                    End If

                                                    If nAircraftID > 0 Then
                                                        If viewMapLinkLabel.Text <> "" Then
                                                            exportFlightsLink.Attributes.Remove("class")
                                                            exportFlightsLink.Attributes.Add("class", "float_right") 'removing the noBefore class for aircraft
                                                        End If
                                                    End If

                                                    If Export_Flights = True Then
                                                        If tmpFlightDataTable.Rows.Count > HttpContext.Current.Session.Item("localPreferences").MaxAllowedCustomExport Then
                                                            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me.flight_data_update, Me.GetType(), "exportAndUncheck", "$('#" & exportUtilization.ClientID.ToString & "').prop('checked', false);$('.resultTextClass').hide();alert('The quantity of flights to export (" & tmpFlightDataTable.Rows.Count.ToString & ") is over the export limitation of your subscription (" & HttpContext.Current.Session.Item("localPreferences").MaxAllowedCustomExport.ToString & "). Contact customerservice@jetnet.com for more information');", True)
                                                        Else

                                                            Dim HeaderString As String = ""
                                                            If nAircraftID = 0 Then
                                                                HeaderString = "Route Analysis: " & aport_name1 & " to " & aport_name2 & " - " & faa_start_date.Text & "-" & faa_end_date.Text
                                                                Call commonLogFunctions.Log_User_Event_Data("UserStatistics", HeaderString, Nothing, 28, 0, 0, 0, 0, 0, 0)
                                                            Else
                                                                HeaderString = "Route Analysis: Aircraft REG# " & sRegNumber & " " & faa_start_date.Text & "-" & faa_end_date.Text
                                                                Call commonLogFunctions.Log_User_Event_Data("UserStatistics", HeaderString, Nothing, 28, 0, 0, 0, 0, nAircraftID, 0)
                                                            End If

                                                            Session("export_info") = DisplayFunctions.ConvertDataTableToNonDynamicTable(tmpFlightDataTable)
                                                            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me.flight_data_update, Me.GetType(), "export", "$('#" & exportUtilization.ClientID.ToString & "').prop('checked', false);window.open('export.aspx','_blank','width=400,height=400,toolbar=no,location=no, directories=no,status=no,menubar=no,scrollbars=no,resizable=no');$('.searchResultsLoadingText').text('Please wait ... ');", True)
                                                        End If
                                                    End If

                                                    arrayStr = (DisplayFunctions.ConvertDataTableToArrayCombinedFields(tmpFlightDataTable, ColumnList, New viewSelectionCriteriaClass, True, aport_id1, True))


                                                    If Page.IsPostBack Then
                                                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me.flight_data_update, Me.GetType(), "flightTableBuild", Replace(Replace(arrayStr, "view_template.aspx?", "FAAFlightData.aspx?aport_id1=" & aport_id1 & "&"), "aport_id=", "aport_id2=") + ";" + View_Master.BuildTable(True, 25, ColumnList).ToString, True)
                                                    ElseIf nAircraftID > 0 Then
                                                        System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "initializeFlight", arrayStr + ";" + View_Master.BuildTable(True, 25, ColumnList).ToString, True)
                                                    End If


                                                    'flight_data.Text = DisplayFunctions.ConvertDataTableToHTML(tmpFlightDataTable) 'flight_data_temp.displayFAAFlightData(tmpFlightDataTable, "", "DATE RANGE(" & Me.faa_start_date.Text & " - " & Me.faa_end_date.Text & ") ", purcahse_date, nAircraftID, start_date, False, "", False, "flightdatanoac", product_code)
                                                Else ' if they are not equal, then dont use text as search
                                                    flight_data.Text = flight_data_temp.displayFAAFlightData(tmpFlightDataTable, "", DropDownList_timeframe_dup.SelectedValue, purcahse_date, nAircraftID, start_date, False, "", False, "flightdatanoac", product_code, comp_id)
                                                End If
                                            Else
                                                flight_data.Text = flight_data_temp.displayFAAFlightData(tmpFlightDataTable, "", DropDownList_timeframe.SelectedValue, purcahse_date, nAircraftID, start_date, False, "", False, "flightdatanoac", product_code, comp_id)
                                            End If
                                        Else
                                            If DropDownList_timeframe.Text = "date_search" Then
                                                If Trim(DropDownList_timeframe_dup.Text) = Trim(DropDownList_timeframe.Text) Then ' if they r equal then we have most likely done a search 
                                                    flight_data.Text = flight_data_temp.displayFAAFlightData(tmpFlightDataTable, "", "DATE RANGE (" & Me.faa_start_date.Text & " - " & Me.faa_end_date.Text & ") ", purcahse_date, nAircraftID, start_date, False, "", False, "flightdata", product_code, comp_id)
                                                Else ' if they are not equal, then dont use text as search
                                                    flight_data.Text = flight_data_temp.displayFAAFlightData(tmpFlightDataTable, "", DropDownList_timeframe_dup.SelectedValue, purcahse_date, nAircraftID, start_date, False, "", False, "flightdata", product_code, comp_id)
                                                End If
                                            Else
                                                flight_data.Text = flight_data_temp.displayFAAFlightData(tmpFlightDataTable, "", DropDownList_timeframe.SelectedValue, purcahse_date, nAircraftID, start_date, False, "", False, "flightdata", product_code, comp_id)
                                            End If
                                        End If

                                        ' load the map with each flight path in the array
                                        If Trim(DropDownList_timeframe.SelectedValue) = "all" Then
                                            map_panel.Visible = False
                                        Else
                                            map_panel.Visible = True
                                            System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showFlightPath", buildMapFlightPaths(flightPathArray, last_aport_lat, last_aport_long), False)
                                        End If

                                        If Trim(Flight_Id1) <> "" Then
                                            iata_icao_search.Visible = True
                                            search_label.Visible = True
                                            search_flight_checkbox.Visible = True
                                        End If

                                    End If

                                Else ' just initialize a blank map
                                    System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "initializeMap", "initialize();", True)
                                End If

                            ElseIf main_tab_container.ActiveTabIndex = 1 Then

                                tmpTripDataTable = getOriginAndDestinationData(sRegNumber, nAircraftID, True, Me.faa_start_date.Text, Me.faa_end_date.Text, aport_id1, aport_id2, show_one_way, comp_id)
                                origins_tab_data.Text = displayOriginAndDestinationData(tmpTripDataTable, True, start_date)

                            ElseIf main_tab_container.ActiveTabIndex = 2 Then

                                tmpTripDataTable = Nothing
                                tmpTripDataTable = getOriginAndDestinationData(sRegNumber, nAircraftID, False, Me.faa_start_date.Text, Me.faa_end_date.Text, aport_id1, aport_id2, show_one_way, comp_id)
                                destinations_tab_data.Text = displayOriginAndDestinationData(tmpTripDataTable, False, start_date)


                            ElseIf main_tab_container.ActiveTabIndex = 3 Then

                                airframe_estimates_tab_data.Text = flight_data_temp.displayAirframeTimesData(flight_data_temp.getAllFAAFlightData(sRegNumber, nAircraftID, airframe_times_as_of, aport_id1, aport_id2, show_one_way), airframe_times_as_of, airframe_total_hours, airframe_total_landings, False, PreviouslyOwnedFlag, purcahse_date, bShowBlankAcFields, False, "flightdata", "", "", comp_id)

                            ElseIf main_tab_container.ActiveTabIndex = 4 Then


                                If aport_id1 > 0 And aport_id2 > 0 And nAircraftID = 0 Then
                                    tmpAircraftSummaryTable = Nothing
                                    tmpAircraftSummaryTable2 = Nothing
                                    '  tmpAircraftSummaryTable = flight_data_temp.get_chart_data_activity_summary(nAircraftID, DropDownList_owner, DropDownList_timeframe, temp_amod_id, nAircraftID, Me.faa_start_date.Text, Me.faa_end_date.Text, aport_id1, aport_id2)
                                    If show_one_way = True Then
                                        tmpAircraftSummaryTable2 = flight_data_temp.get_chart_data_activity_summary(0, DropDownList_owner, DropDownList_timeframe, temp_amod_id, nAircraftID, Me.faa_start_date.Text, Me.faa_end_date.Text, aport_id1, aport_id2, show_one_way, "", product_code, comp_id)
                                    Else
                                        tmpAircraftSummaryTable2 = flight_data_temp.get_chart_data_activity_summary(0, DropDownList_owner, DropDownList_timeframe, temp_amod_id, nAircraftID, Me.faa_start_date.Text, Me.faa_end_date.Text, aport_id1, aport_id2, False, "", product_code, comp_id)
                                        tmpAircraftSummaryTable = flight_data_temp.get_chart_data_activity_summary(0, DropDownList_owner, DropDownList_timeframe, temp_amod_id, nAircraftID, Me.faa_start_date.Text, Me.faa_end_date.Text, aport_id1, aport_id2, True, "orig1", product_code, comp_id)
                                        tmpAircraftSummaryTable3 = flight_data_temp.get_chart_data_activity_summary(0, DropDownList_owner, DropDownList_timeframe, temp_amod_id, nAircraftID, Me.faa_start_date.Text, Me.faa_end_date.Text, aport_id2, aport_id1, True, "orig2", product_code, comp_id)
                                    End If

                                    Dim displayOnlyFirstAirport As Boolean = False
                                    Dim DisplayOnlySecondAirport As Boolean = False
                                    If route_selection.SelectedValue = 1 Then
                                        displayOnlyFirstAirport = True
                                    ElseIf route_selection.SelectedValue = 2 Then
                                        DisplayOnlySecondAirport = True
                                    End If
                                    Call flight_data_temp.make_google_summary_chart(tmpAircraftSummaryTable, nAircraftID, start_date, tmpAircraftSummaryTable2, tmpAircraftSummaryTable3, aport_id1, aport_id2, aport_name1, aport_name2, total_flights_count, Me.Page, Me.chart_panel, Me.map_panel, Me.DropDownList_timeframe, "", True, displayOnlyFirstAirport, DisplayOnlySecondAirport)
                                    Me.chart_label.Text = "" '<div class=""Box""><table cellspacing='0' cellpadding='3' border='0' align='center'  class='formatTable blue'><thead><tr><td>&nbsp;</td></tr></thead><tbody>&nbsp;</tbody></table></div>"
                                    ' Call make_chart_text(tmpAircraftSummaryTable, tmpAircraftSummaryTable2, start_date, sRegNumber, DropDownList_timeframe.SelectedValue, "flightdata")




                                    Me.Flights_total_label.Text = "<font class='" & HttpContext.Current.Session("FONT_CLASS_HEADER") & " subHeader'>FLIGHTS PER MONTH</font>" 'TOTAL FLIGHTS: "

                                    'If Not IsNothing(tmpAircraftSummaryTable2) Then
                                    '  Me.Flights_total_label.Text &= "<strong>" & FormatNumber(total_flights_count, 0).ToString & "</strong>"
                                    'Else
                                    '  Me.Flights_total_label.Text &= "&nbsp;"
                                    'End If

                                    'Me.Flights_total_label.Text &= "<span class=""padding_left"">&nbsp;&nbsp;&nbsp;AVERAGE(FLIGHTS / MONTH): "


                                    'If Not IsNothing(tmpAircraftSummaryTable2) Then
                                    '  Me.Flights_total_label.Text &= "<strong>" & FormatNumber((total_flights_count / tmpAircraftSummaryTable2.Rows.Count), 0).ToString & "</strong>"
                                    'Else
                                    '  Me.Flights_total_label.Text &= "&nbsp;"
                                    'End If
                                    'Me.Flights_total_label.Text &= "</span></font>"


                                Else
                                    chart_label.Text = ""
                                    mapHeader.Text = "FLIGHTS PER MONTH"

                                    If Trim(DropDownList_timeframe.SelectedValue) = "last_year" Then
                                        flight_data_temp.make_ac_flights_comparisons(nAircraftID, Me.chart_label.Text, DropDownList_owner, DropDownList_timeframe, temp_amod_id, Me.faa_start_date, Me.faa_end_date, start_date, sRegNumber, aport_id1, aport_id2, show_one_way, aport_name1, aport_name2, total_flights_count, Me.Page, Me.chart_panel, Me.map_panel, "", True, comp_id)
                                    ElseIf Trim(DropDownList_owner.SelectedValue) = "current" Or Trim(DropDownList_timeframe.SelectedValue) = "current" Then
                                        flight_data_temp.make_ac_flights_comparisons(nAircraftID, Me.chart_label.Text, DropDownList_owner, DropDownList_timeframe, temp_amod_id, Me.faa_start_date, Me.faa_end_date, start_date, sRegNumber, aport_id1, aport_id2, show_one_way, aport_name1, aport_name2, total_flights_count, Me.Page, Me.chart_panel, Me.map_panel, "", True, comp_id, "current")
                                    ElseIf Trim(DropDownList_timeframe.SelectedValue) = "all" Then
                                        flight_data_temp.make_ac_flights_comparisons(nAircraftID, Me.chart_label.Text, DropDownList_owner, DropDownList_timeframe, temp_amod_id, Me.faa_start_date, Me.faa_end_date, start_date, sRegNumber, aport_id1, aport_id2, show_one_way, aport_name1, aport_name2, total_flights_count, Me.Page, Me.chart_panel, Me.map_panel, "", True, comp_id)
                                    ElseIf Trim(DropDownList_timeframe.SelectedValue) = "date_search" Then
                                        flight_data_temp.make_ac_flights_comparisons(nAircraftID, Me.chart_label.Text, DropDownList_owner, DropDownList_timeframe, temp_amod_id, Me.faa_start_date, Me.faa_end_date, start_date, sRegNumber, aport_id1, aport_id2, show_one_way, aport_name1, aport_name2, total_flights_count, Me.Page, Me.chart_panel, Me.map_panel, "", True, comp_id, "date_search")
                                    Else
                                        flight_data_temp.make_ac_flights_comparisons(nAircraftID, Me.chart_label.Text, DropDownList_owner, DropDownList_timeframe, temp_amod_id, Me.faa_start_date, Me.faa_end_date, start_date, sRegNumber, aport_id1, aport_id2, show_one_way, aport_name1, aport_name2, total_flights_count, Me.Page, Me.chart_panel, Me.map_panel, "", True, comp_id)
                                    End If

                                End If




                            ElseIf main_tab_container.ActiveTabIndex = 5 Then

                                tmpTripDataTable = getOriginAndDestinationROUTESData(sRegNumber, nAircraftID, DropDownList_owner, DropDownList_timeframe, Me.faa_start_date.Text, Me.faa_end_date.Text, aport_id1, aport_id2, show_one_way, comp_id)
                                routes_label.Text = displayRoutesData(tmpTripDataTable, True, start_date)

                            ElseIf main_tab_container.ActiveTabIndex = 6 Then

                                tmpTripDataTable = getOriginAndDestinationPAIRSData(sRegNumber, nAircraftID, DropDownList_owner, DropDownList_timeframe, Me.faa_start_date.Text, Me.faa_end_date.Text, aport_id1, aport_id2, show_one_way, comp_id)
                                city_pairs_label.Text = displayCITYPAIRSData(tmpTripDataTable, True, start_date)

                            ElseIf main_tab_container.ActiveTabIndex = 7 Then


                                If DropDownList_timeframe.Text = "date_search" Then
                                    If Trim(DropDownList_timeframe_dup.Text) = Trim(DropDownList_timeframe.Text) Then ' if they are equal then we have most likely done a search 
                                        tmpFlightDataTable = flight_data_temp.getFAAFlightData_View_Simple(sRegNumber, nAircraftID, DropDownList_owner, DropDownList_timeframe, False, Me.faa_start_date.Text, Me.faa_end_date.Text, aport_id1, aport_id2, show_one_way, 7, False, product_code, False, True, comp_id)
                                    Else ' if they are not equal, then dont use text as search 
                                        tmpFlightDataTable = flight_data_temp.getFAAFlightData_View_Simple(sRegNumber, nAircraftID, DropDownList_owner, DropDownList_timeframe_dup, False, "", "", aport_id1, aport_id2, show_one_way, 7, False, product_code, False, True, comp_id)
                                    End If
                                Else
                                    tmpFlightDataTable = flight_data_temp.getFAAFlightData_View_Simple(sRegNumber, nAircraftID, DropDownList_owner, DropDownList_timeframe, False, "", "", aport_id1, aport_id2, show_one_way, 7, False, product_code, False, True, comp_id)
                                End If

                                If Not IsNothing(tmpFlightDataTable) Then
                                    aircraft_tab_label.Text = make_ac_listing(tmpFlightDataTable)
                                End If

                            ElseIf main_tab_container.ActiveTabIndex = 8 Then   ' operators

                                If DropDownList_timeframe.Text = "date_search" Then
                                    If Trim(DropDownList_timeframe_dup.Text) = Trim(DropDownList_timeframe.Text) Then ' if they r equal then we have most likely done a search 
                                        tmpFlightDataTable = flight_data_temp.getFAAFlightData_View_Simple(sRegNumber, nAircraftID, DropDownList_owner, DropDownList_timeframe, False, Me.faa_start_date.Text, Me.faa_end_date.Text, aport_id1, aport_id2, show_one_way, 8, False, product_code, True, False, comp_id)
                                    Else ' if they are not equal, then dont use text as search 
                                        tmpFlightDataTable = flight_data_temp.getFAAFlightData_View_Simple(sRegNumber, nAircraftID, DropDownList_owner, DropDownList_timeframe_dup, False, "", "", aport_id1, aport_id2, show_one_way, 8, False, product_code, True, False, comp_id)
                                    End If
                                Else
                                    tmpFlightDataTable = flight_data_temp.getFAAFlightData_View_Simple(sRegNumber, nAircraftID, DropDownList_owner, DropDownList_timeframe, False, "", "", aport_id1, aport_id2, show_one_way, 8, False, product_code, True, False, comp_id)
                                End If

                                If Not IsNothing(tmpFlightDataTable) Then
                                    operators_tab_label.Text = make_operator_listing(tmpFlightDataTable)
                                End If

                            ElseIf main_tab_container.ActiveTabIndex = 9 Then   ' Live Flights
                                Me.dateSearchBoxes.Visible = False
                                Me.flight_aware_label.Visible = True
                                flightAwareCell.Visible = True
                                Me.live_flights_label.Text = "<iframe src='https://flightaware.com/live/flight/" & Trim(temp_reg) & "' width='100%' height='2000'></iframe>"
                            End If

                        End If  ' AC ID END IF 


                        DropDownList_timeframe_dup.Text = DropDownList_timeframe.Text

                    End If

                End If

            End If


            If Page.IsPostBack Then
                System.Web.UI.ScriptManager.RegisterStartupScript(Me.flight_data_update, Me.GetType(), "Toggle", BuildTable() & ";setUpLinkHover();", True)
            End If

            If Not Page.IsPostBack Then
                Dim jsStr As String = ""
                jsStr = "$(function() {" & vbNewLine

                jsStr += "setUpLinkHover();setUpAutoComplete();" & vbNewLine
                jsStr += "setTimeout(function(){"
                If routeAnalysis = True And aport_id1 > 0 And aport_id2 > 0 Then
                Else
                    jsStr += BuildTable()
                End If

                jsStr += "; }, 500);"

                If routeAnalysis = True And aport_id1 > 0 And aport_id2 > 0 Then 'already inside postback, only click if options prefilled.
                    jsStr += "$('#" & searchFlight.ClientID & "').click();"
                End If
                jsStr += "});" & vbNewLine


                System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StartupScr", jsStr, True)
            End If


        Catch ex As Exception

        End Try
    End Sub

    Public Sub get_distance_flights_tables(ByRef temp_table_1 As DataTable, ByRef temp_table_2 As DataTable, ByVal aport_id1 As Long, ByVal aport_id2 As Long)


        temp_table_1 = flight_data_temp.get_Distances_Flights(Me.iata_icao_search.Text, aport_id1, "origin")


        temp_table_2 = flight_data_temp.get_Distances_Flights(Me.iata_icao_search.Text, aport_id2, "destination")


    End Sub



    'Public Shared Function ConvertDataTableToHTML(ByVal dt As DataTable) As String
    '  Dim html As String = "<table class=""formatTable blue"">"
    '  html += "<thead>"
    '  html += "<tr>"

    '  html += "<th>Aircraft</th>"
    '  'amod_make_name as MAKE, amod_model_name as MODEL,  ac_mfr_year as 'MFR YEAR', SERNBR, REGNBR,
    '  html += "<th>Flight Date</th>"
    '  'ffd_date as 'FLIGHT DATE', 
    '  html += "<th>Flight Time</th>"
    '  'ffd_flight_time as 'FLIGHT TIME',
    '  html += "<th width=""100"">Dist.</th>"
    '  'ffd_distance as 'DISTANCE', 
    '  html += "<th>Fuel Burn</th>"
    '  'ESTFUELBURN, 
    '  html += "<th>Origin</th>"
    '  'ffd_origin_aport as 'ORIGIN CODE', origin_aport_name AS 'ORIGIN NAME', origin_aport_city AS 'ORIGIN CITY', origin_aport_state AS 'ORIGIN STATE', 
    '  'origin_aport_country as 'ORIGIN COUNTRY',  origin_aport_latitude AS 'ORIGIN LAT', origin_aport_longitude AS 'ORIGIN LONG',
    '  html += "<th>Destination</th>"
    '  'ffd_dest_aport AS 'DEST CODE',dest_aport_name AS 'DEST NAME',dest_aport_city AS 'DEST CITY',  dest_aport_state AS 'DEST STATE',  dest_aport_country AS 'DEST COUNTRY', 
    '  'dest_aport_latitude AS 'DEST LAT', dest_aport_longitude AS 'DEST LONG',  
    '  html += "<th>Operator</th>"
    '  ' comp_name AS 'OPERATOR', comp_address1 AS 'ADDRESS', 
    '  ' comp_city AS 'CITY', comp_state AS 'STATE', comp_country AS 'COUNTRY', comp_web_address AS 'WEB ADDRESS', comp_email_address AS 'EMAIL',
    '  'comp_off_phone AS 'OFFICE PHONE', 
    '  html += "<th>Contact</th>"
    '  'contact_first_name AS 'FIRST NAME', contact_last_name AS 'LAST NAME', contact_title AS 'TITLE', contact_email_address AS 'CONTACT EMAIL',
    '  'contact_off_phone AS 'CONTACT OFFICE PHONE',
    '  'contact_mob_phone AS 'CONTACT MOBILE PHONE'
    '  html += "</tr>"
    '  html += "</thead>"

    '  html += "<tbody>"
    '  For Each r As DataRow In dt.Rows
    '    html += "<tr>"
    '    'Aircraft
    '    html += "<td>"
    '    If Not IsDBNull(r("MFR YEAR")) Then
    '      html += r("MFR YEAR").ToString + " "
    '    End If
    '    If Not IsDBNull(r("MAKE")) Then
    '      html += r("MAKE").ToString
    '    End If
    '    If Not IsDBNull(r("MODEL")) Then
    '      html += " " + r("MODEL").ToString
    '    End If
    '    html += "<br />"
    '    If Not IsDBNull(r("SERNBR")) Then
    '      html += " S/N " + r("SERNBR").ToString
    '    End If
    '    If Not IsDBNull(r("REGNBR")) Then
    '      html += " Reg " + r("REGNBR").ToString
    '    End If
    '    html += "</td>"
    '    'FLIGHT DATE
    '    html += "<td>"
    '    If Not IsDBNull(r("FLIGHT DATE")) Then
    '      html += r("FLIGHT DATE").ToString
    '    End If
    '    html += "</td>"
    '    'Flight Time
    '    html += "<td>"
    '    If Not IsDBNull(r("FLIGHT TIME")) Then
    '      html += FormatNumber(r("FLIGHT TIME").ToString, 0, True, False, True)
    '    End If
    '    html += "</td>"
    '    'Distance
    '    html += "<td>"
    '    If Not IsDBNull(r("DISTANCE")) Then
    '      html += r("DISTANCE").ToString

    '    End If
    '    html += "</td>"
    '    'Fuel Burn
    '    html += "<td>"
    '    If Not IsDBNull(r("ESTFUELBURN")) Then
    '      html += FormatNumber(r("ESTFUELBURN").ToString, 0, True, False, True)
    '    End If
    '    'Origin
    '    html += "<td>"
    '    If Not IsDBNull(r("ORIGIN CODE")) Then
    '      html += r("ORIGIN CODE") & " - "
    '    End If
    '    If Not IsDBNull(r("ORIGIN NAME")) Then
    '      html += r("ORIGIN NAME")
    '    End If
    '    html += "<br />"
    '    If Not IsDBNull(r("ORIGIN CITY")) Then
    '      html += r("ORIGIN CITY") & " "
    '    End If
    '    If Not IsDBNull(r("ORIGIN STATE")) Then
    '      html += r("ORIGIN STATE") & ", "
    '    End If
    '    If Not IsDBNull(r("ORIGIN COUNTRY")) Then
    '      html += r("ORIGIN COUNTRY")
    '    End If
    '    html += "</td>"

    '    'Destination
    '    html += "<td>"
    '    If Not IsDBNull(r("DEST CODE")) Then
    '      html += r("DEST CODE") & " - "
    '    End If
    '    If Not IsDBNull(r("DEST NAME")) Then
    '      html += r("DEST NAME")
    '    End If
    '    html += "<br />"
    '    If Not IsDBNull(r("DEST CITY")) Then
    '      html += r("DEST CITY") & " "
    '    End If
    '    If Not IsDBNull(r("DEST STATE")) Then
    '      html += r("DEST STATE") & ", "
    '    End If
    '    If Not IsDBNull(r("DEST COUNTRY")) Then
    '      html += r("DEST COUNTRY")
    '    End If
    '    html += "</td>"

    '    'Operator
    '    html += "<td>"
    '    Dim seperator As String = ""
    '    If Not IsDBNull(r("OPERATOR")) Then
    '      html += " " & r("OPERATOR")
    '      seperator = "<br />"
    '    End If

    '    html += seperator
    '    seperator = ""
    '    If Not IsDBNull(r("ADDRESS")) Then
    '      html += " " & r("ADDRESS")
    '      Seperator = "<br />"
    '    End If

    '    html += Seperator
    '    Seperator = ""

    '    If Not IsDBNull(r("CITY")) Then
    '      html += r("CITY") & ", "
    '      Seperator = "<br />"
    '    End If

    '    If Not IsDBNull(r("STATE")) Then
    '      html += r("STATE") & " "
    '      Seperator = "<br />"
    '    End If

    '    If Not IsDBNull(r("COUNTRY")) Then
    '      html += r("COUNTRY")
    '      Seperator = "<br />"
    '    End If
    '    html += seperator
    '    seperator = ""
    '    If Not IsDBNull(r("WEB ADDRESS")) Then
    '      seperator = "<br />"
    '      html += "<a class=""tiny_text"" href="""
    '      If InStr("http://", r("WEB ADDRESS")) = 0 Then
    '        html += "http://" & r("WEB ADDRESS")
    '      Else
    '        html += r("WEB ADDRESS")
    '      End If
    '      html += """ target=""blank"">" & r("WEB ADDRESS") & "</a>"
    '    End If
    '    html += seperator
    '    seperator = ""
    '    If Not IsDBNull(r("EMAIL")) Then
    '      html += "<a class=""tiny_text"" href=""mailto:"
    '      html += r("EMAIL")
    '      html += """>" & r("EMAIL") & "</a>"
    '    End If
    '    html += "</td>"

    '    html += "<td>"
    '    If Not IsDBNull(r("TITLE")) Then
    '      html += r("TITLE")
    '      seperator = "<br />"
    '    End If
    '    html += seperator
    '    seperator = ""
    '    If Not IsDBNull(r("FIRST NAME")) Then
    '      html += r("FIRST NAME")
    '      seperator = " "
    '    End If
    '    html += seperator
    '    seperator = ""
    '    If Not IsDBNull(r("LAST NAME")) Then
    '      html += r("LAST NAME")
    '      seperator = "<br />"
    '    End If

    '    If Not IsDBNull(r("CONTACT EMAIL")) Then
    '      html += "<br /><a class=""tiny_text"" href=""mailto:"
    '      html += r("CONTACT EMAIL")
    '      html += """>" & r("CONTACT EMAIL") & "</a>"
    '      seperator = "<br />"
    '    End If
    '    html += seperator
    '    seperator = ""
    '    If Not IsDBNull(r("CONTACT OFFICE PHONE")) Then
    '      html += "OFFICE: " + r("CONTACT OFFICE PHONE")
    '      seperator = "<br />"
    '    End If
    '    html += seperator
    '    seperator = ""
    '    If Not IsDBNull(r("CONTACT MOBILE PHONE")) Then
    '      html += "MOBILE: " + r("CONTACT MOBILE PHONE")
    '      seperator = "<br />"
    '    End If


    '    html += "</td>"
    '    html += "</tr>"
    '  Next
    '  html += "</tbody>"
    '  html += "</table>"
    '  Return html
    'End Function

    Private Function BuildTable() As String
        Dim tableBuild As New StringBuilder
        Dim footerBuild As New StringBuilder
        Dim bottomNumber As Integer = 6
        Dim topNumber As Integer = 8

        If UCase(main_tab_container.ActiveTab.HeaderText) = "FLIGHTS" Then


            footerBuild.Append("""footerCallback"": function ( row, data, start, end, display ) {")
            footerBuild.Append("var api = this.api(), data;")

            '// Remove the formatting to get integer data for summation
            footerBuild.Append("var intVal = function ( i ) {")
            footerBuild.Append("return typeof i === 'string' ?")
            footerBuild.Append("i.replace(/[\$,]/g, '').replace(/<[^>]+>/ig,'')*1 :")
            footerBuild.Append("typeof i === 'number' ?")
            footerBuild.Append("i : 0;")
            footerBuild.Append("};")

            'Let's build the total string:

            '// Total over all pages
            Dim TotalSorting As New StringBuilder

            For x = bottomNumber To topNumber
                TotalSorting.Append("total = api")
                TotalSorting.Append(".column(" & x & ")")
                TotalSorting.Append(".data()")
                TotalSorting.Append(".reduce( function (a, b) {")
                TotalSorting.Append("return intVal(a) + intVal(b);")
                TotalSorting.Append("}, 0 );")

                '// Update footer
                TotalSorting.Append("if (Math.round(total) !== total) {")
                TotalSorting.Append("total = total.toFixed(2);")
                TotalSorting.Append("}")

                TotalSorting.Append("$( api.column(" & x & ").footer() ).html('<span>' + ")
                TotalSorting.Append("total.toLocaleString('en')")

                TotalSorting.Append("+ '</span>');")
            Next

            TotalSorting.Append("$( api.column(" & 0 & ").footer() ).html(")
            TotalSorting.Append("'Totals:'")
            TotalSorting.Append(");")

            footerBuild.Append(TotalSorting.ToString)
            footerBuild.Append("},")

        End If
        tableBuild.Append("var cw = $('.aircraftContainer').width() - 20;")
        tableBuild.Append("$("".resizeDiv"").width(cw);")

        tableBuild.Append("$(window).resize(function() {")
        tableBuild.Append("var cw = $('.aircraftContainer').width() - 20;")
        tableBuild.Append("$("".resizeDiv"").width(cw);")
        tableBuild.Append("});")
        tableBuild.Append("var hideFromExport = [12];")

        'tableBuild.Append("if ( $.fn.dataTable.isDataTable( '.ajax__tab_active:visible.formatTable' ) ) {")
        'tableBuild.Append(" $('.ajax__tab_active:visible.formatTable').DataTable().destroy();")
        'tableBuild.Append("}")

        tableBuild.Append("var table =$('.ajax__tab_active:visible.formatTable').DataTable({destroy:true,")
        tableBuild.Append("dom:        'Bfitrp',")
        tableBuild.Append("scrollY:        530, ")
        tableBuild.Append("scrollX:        cw, ")
        tableBuild.Append("scrollCollapse: true, ")
        tableBuild.Append("scroller:       true, ")
        tableBuild.Append(footerBuild)
        tableBuild.Append(BuildButtonString)
        'Remove Selected Button

        tableBuild.Append("});")

        'tableBuild.Append("if ( $.fn.dataTable.isDataTable( '.ajax__tab_panel:visible .formatTable' ) ) {")
        'tableBuild.Append(" $('.ajax__tab_panel:visible .formatTable').DataTable().destroy();")
        'tableBuild.Append("}")

        tableBuild.Append("$('.ajax__tab_panel:visible .formatTable').DataTable({destroy:true,")
        tableBuild.Append("dom:        'Bfitrp',")
        tableBuild.Append("scrollY:        530, ")
        tableBuild.Append("scrollX:        cw, ")
        tableBuild.Append("scrollCollapse: true, ")
        tableBuild.Append("scroller:       true, ")
        tableBuild.Append(footerBuild)
        tableBuild.Append(BuildButtonString)
        'Remove Selected Button:
        tableBuild.Append("});")
        tableBuild.Append("$($.fn.dataTable.tables(true)).DataTable().columns.adjust();")
        tableBuild.Append("$($.fn.dataTable.tables(true)).DataTable().scroller.measure();")

        Return tableBuild.ToString
    End Function
    Private Function BuildButtonString() As String
        Dim buttonsString As New StringBuilder
        Dim excelButton As String = ""
        Dim exportOptions As String = ""
        If Session.Item("isMobile") = False Then
            exportOptions = "columns: [function ( idx, data, node ) {"
            exportOptions += "var isVisible = table.column( idx ).visible();"
            exportOptions += "var isNotForExport = $.inArray( idx, hideFromExport ) !== -1;"
            exportOptions += "return isVisible && !isNotForExport ? true : false; "
            'ExportOptions += "}"
            exportOptions += "}, 'colvis']"


            buttonsString.Append("buttons: [ ")
            'CSV Button:
            buttonsString.Append("{")
            buttonsString.Append("extend:  'csv',")
            If HttpContext.Current.Session.Item("localUser").crmDemoUserFlag = True Then
                buttonsString.Append("enabled:  false,")
            End If

            buttonsString.Append("exportOptions: {")
            buttonsString.Append(exportOptions)
            buttonsString.Append("}")
            buttonsString.Append("}, ")
            'Excel Button
            CreateExcelButton(excelButton, "summaryTable")
            'buttonsString.Append("{extend: 'excel', ")
            'buttonsString.Append("exportOptions: {")
            'buttonsString.Append(exportOptions)
            'buttonsString.Append("}")
            'buttonsString.Append("},")
            buttonsString.Append(excelButton)
            'PDF Button

            buttonsString.Append(" {extend: 'pdf', orientation: 'landscape', pageSize: 'A2', ")

            If HttpContext.Current.Session.Item("localUser").crmDemoUserFlag = True Then
                buttonsString.Append("enabled:  false,")
            End If

            buttonsString.Append("exportOptions: {")
            buttonsString.Append(exportOptions)
            buttonsString.Append("}")
            buttonsString.Append("}, ")
            'Print Button
            'ButtonsString.Append(" {extend: 'print', ")
            'ButtonsString.Append("exportOptions: {")
            'ButtonsString.Append(exportOptions)
            'ButtonsString.Append("}")
            'ButtonsString.Append("}, ")
            'Column Visibility Button
            buttonsString.Append("{")
            buttonsString.Append("extend: 'colvis', text: 'Columns',")

            buttonsString.Append("collectionLayout:  'fixed two-column',")
            buttonsString.Append("postfixButtons: [ 'colvisRestore' ]")
            buttonsString.Append("}")
            buttonsString.Append("]")
        End If
        Return buttonsString.ToString
    End Function


    Public Shared Sub CreateExcelButton(ByRef ExcelButton As String, ByVal PanelName As String)
        Dim PlaceholderString As String = ""

        ExcelButton = "var panel = $("".ajax__tab_active:visible.formatTable"");"
        ExcelButton += "my_form = document.createElement('FORM');"
        ExcelButton += "my_form.name = 'myForm';"
        ExcelButton += "my_form.method = 'POST';"
        ExcelButton += "my_form.action = 'MacShell.aspx';"
        ExcelButton += "my_form.target = '_new';"
        ExcelButton += " my_tb = document.createElement('INPUT');"
        ExcelButton += "my_tb.type = 'HIDDEN';"
        ExcelButton += "my_tb.name = 'MacExport';"
        ExcelButton += "my_tb.value = true;"
        ExcelButton += "my_form.appendChild(my_tb);"

        ExcelButton += " my_tb = document.createElement('INPUT');"
        ExcelButton += "my_tb.type = 'HIDDEN';"
        ExcelButton += "my_tb.name = 'data';"
        ExcelButton += "my_tb.value = panel.innerHTML;"
        ExcelButton += "my_form.appendChild(my_tb);"
        ExcelButton += " document.body.appendChild(my_form);"
        ExcelButton += "  my_form.submit();"

        If Not IsNothing(HttpContext.Current.Session.Item("localUser").crmPlatformOS) Then
            If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("localUser").crmPlatformOS) Then
                If InStr(HttpContext.Current.Session.Item("localUser").crmPlatformOS, "mac") > 0 Then
                    PlaceholderString += ", { text:'Excel', "
                    If HttpContext.Current.Session.Item("localUser").crmDemoUserFlag = True Then
                        PlaceholderString += "enabled:  false,"
                    End If

                    PlaceholderString += " action: function( e, dt, node, config) {" & ExcelButton & "}},"
                Else
                    PlaceholderString += " {extend: 'excel', "

                    If HttpContext.Current.Session.Item("localUser").crmDemoUserFlag = True Then
                        PlaceholderString += "enabled:  false,"
                    End If

                    PlaceholderString += " exportOptions : {columns: ':visible'}}, "
                End If
            Else
                PlaceholderString += " {extend: 'excel', "

                If HttpContext.Current.Session.Item("localUser").crmDemoUserFlag = True Then
                    PlaceholderString += "enabled:  false,"
                End If

                PlaceholderString += " exportOptions : {columns: ':visible'}}, "
            End If
        Else
            PlaceholderString += " {extend: 'excel', "

            If HttpContext.Current.Session.Item("localUser").crmDemoUserFlag = True Then
                PlaceholderString += "enabled:  false,"
            End If

            PlaceholderString += " exportOptions : {columns: ':visible'}}, "
        End If
        ExcelButton = PlaceholderString
    End Sub
    Private Sub AlignTheHeaderLabels()
        'origins_tab_label.Text = Replace(aircraftInformationText.Text, "FLIGHT ACTIVITY", "ORIGINS")
        'destinations_tab_label.Text = Replace(aircraftInformationText.Text, "FLIGHT ACTIVITY", "DESTINATIONS")
        'estimates_tab_label.Text = Replace(aircraftInformationText.Text, "FLIGHT ACTIVITY", "AIFRAME ESTIMATES")
        'activity_tab_label.Text = Replace(aircraftInformationText.Text, "FLIGHT ACTIVITY", "UTILIZATION")
        'routes_tab_label.Text = Replace(aircraftInformationText.Text, "FLIGHT ACTIVITY", "ROUTES")
        'pairs_tab_label.Text = Replace(aircraftInformationText.Text, "FLIGHT ACTIVITY", "CITY PAIRS")
        'aircraft_tab_header_label.Text = Replace(aircraftInformationText.Text, "FLIGHT ACTIVITY", "AIRCRAFT SUMMARY")
        'operators_tab_header_label.Text = Replace(aircraftInformationText.Text, "FLIGHT ACTIVITY", "OPERATORS")
    End Sub
    Public Function make_ac_listing(ByVal final_table As DataTable) As String

        make_ac_listing = ""

        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False

        Try


            htmlOut.Append("<div class=""Box"">")
            htmlOut.Append("<table width=""100%"">")

            'htmlOut.Append("<tr><td valign=""middle"" align=""left"" colspan=""5"" " & IIf(HttpContext.Current.Session.Item("isMobile") = False, "nowrap='nowrap'", "class='mobileAlignLeft'") & "><font class='" & HttpContext.Current.Session("FONT_CLASS_HEADER") & " mainHeading'><strong>Aircraft</strong> Summary</font></td></tr>")
            htmlOut.Append("<table cellpadding='3' cellspacing=""0"" width=""100%"" class='formatTable blue'><thead>")

            ' htmlOut.Append("<tr><td colspan='10' align='right'><a title='Expand' onclick=""javascript:load('WebSource.aspx?viewType=dynamic&display=table&PageTitle=AIRCRAFT SUMMARY','','scrollbars=yes,menubar=no,height=700,width=1250,resizable=yes,toolbar=no,location=no,status=no');"" class=""cursor""><u>VIEW IN GRID</u></a></right></td></tr>")


            htmlOut.Append("<tr><th valign=""middle"" class=""left"">Aircraft</th>")
            htmlOut.Append("<th valign=""middle"" class=""left"">Ser#</th>")
            htmlOut.Append("<th valign=""middle"" class=""left"">Reg#</th>")

            htmlOut.Append("<th valign=""middle"" class=""right"" class=""mobileAlignBottom"">Nbr Flights</th>")

            ' htmlOut.Append("<th valign=""middle"" class='right'>Dist.<em>(nm)</em></th>")
            htmlOut.Append("<th valign=""middle"" class='right'>Total Flight Hours</th>") '</a>
            htmlOut.Append("<th valign=""middle"" class='right'>Est Fuel<br />Burn (GAL)</th>") '</a>

            'If aport_id1 > 0 And aport_id2 > 0 Then
            htmlOut.Append("<th valign=""middle"" class=""left"">Operator</th>")
            'End If
            htmlOut.Append("<th valign=""middle"" class=""left"">Address</th>")
            htmlOut.Append("<th valign=""middle"" class=""left"">City</th>")
            htmlOut.Append("<th valign=""middle"" class=""left"">State</th>")
            htmlOut.Append("<th valign=""middle"" class=""left"">Country</th>")
            htmlOut.Append("<th valign=""middle"" class=""left"">Email</th>")

            htmlOut.Append("<th valign=""middle"" class=""left"">Web Address</th>")
            htmlOut.Append("<th valign=""middle"" class=""left"">Office Phone</th>")



            htmlOut.Append("</tr></thead><tbody>")

            For Each r As DataRow In final_table.Rows

                If Not toggleRowColor Then
                    htmlOut.Append("<tr class=""alt_row"">")
                    toggleRowColor = True
                Else
                    htmlOut.Append("<tr bgcolor=""white"">")
                    toggleRowColor = False
                End If


                If Not IsDBNull(r("ac_id")) Then
                    If Not String.IsNullOrEmpty(r.Item("ac_id").ToString) Then '

                        htmlOut.Append("<td valign=""middle"" class=""left"">" & r.Item("amod_make_name").ToString & " " & r.Item("amod_model_name").ToString & "</td><td valign=""middle"" class=""left"">" & r.Item("ac_ser_no_full").ToString & "</td><td valign=""middle"" class=""left"">" & r.Item("ac_reg_no").ToString & "</td>")

                        '   total_distance_min = total_distance_min + FormatNumber(r.Item("flight_distance").ToString, 0, True, False, True)
                    Else
                        htmlOut.Append("<td colspan=""3"">&nbsp;</td>")
                    End If
                Else
                    htmlOut.Append("<td colspan=""3"">&nbsp;</td>")
                End If


                If Not IsDBNull(r("NbrFlights")) Then
                    If Not String.IsNullOrEmpty(r.Item("NbrFlights").ToString) Then '
                        htmlOut.Append("<td valign=""middle"" align=""right"">" + FormatNumber(r.Item("NbrFlights").ToString, 0, True, False, True) + "</td>")
                        '   total_distance_min = total_distance_min + FormatNumber(r.Item("flight_distance").ToString, 0, True, False, True)
                    Else
                        htmlOut.Append("<td>&nbsp;</td>")
                    End If
                Else
                    htmlOut.Append("<td>&nbsp;</td>")
                End If


                'If Not IsDBNull(r("flight_distance")) Then
                '  If Not String.IsNullOrEmpty(r.Item("flight_distance").ToString) Then '
                '    htmlOut.Append("<td valign=""middle"" align=""right"">" + FormatNumber(flightDataFunctions.ConvertStatuteMileToNauticalMile(r.Item("flight_distance").ToString), 0, True, False, True) + "</td>")
                '    '   total_distance_min = total_distance_min + FormatNumber(r.Item("flight_distance").ToString, 0, True, False, True)
                '  Else
                '    htmlOut.Append("<td>&nbsp;</td>")
                '  End If
                'Else
                '  htmlOut.Append("<td>&nbsp;</td>")
                'End If


                If Not IsDBNull(r("TotalFlightTimeHrs")) Then
                    If Not String.IsNullOrEmpty(r.Item("TotalFlightTimeHrs").ToString) Then
                        htmlOut.Append("<td valign=""middle"" align=""right"">" + FormatNumber(r.Item("TotalFlightTimeHrs").ToString, 0, True, False, True) + "</td>")
                        ' total_flight_time_min = total_flight_time_min + FormatNumber(r.Item("flight_time").ToString, 0, True, False, True)
                    Else
                        htmlOut.Append("<td>&nbsp;</td>")
                    End If
                Else
                    htmlOut.Append("<td>&nbsp;</td>")
                End If

                If Not IsDBNull(r("TotalFuelBurn")) Then
                    If Not String.IsNullOrEmpty(r.Item("TotalFuelBurn").ToString) Then
                        htmlOut.Append("<td valign=""middle"" align=""right"">" + FormatNumber(r.Item("TotalFuelBurn").ToString, 0, True, False, True) + "</td>")
                        ' total_flight_time_min = total_flight_time_min + FormatNumber(r.Item("flight_time").ToString, 0, True, False, True)
                    Else
                        htmlOut.Append("<td>&nbsp;</td>")
                    End If
                Else
                    htmlOut.Append("<td>&nbsp;</td>")
                End If

                'If aport_id1 > 0 And aport_id2 > 0 Then
                If Not IsDBNull(r.Item("OPERATOR")) And Not IsDBNull(r.Item("comp_id")) Then
                    If Not String.IsNullOrEmpty(r.Item("OPERATOR").ToString) Then
                        htmlOut.Append("<td valign=""middle"" class=""left"">" & DisplayFunctions.WriteDetailsLink(0, r.Item("comp_id"), 0, 0, True, r.Item("OPERATOR").ToString, "text_underline tiny_text", "") & "</td>")
                    Else
                        htmlOut.Append("<td>&nbsp;</td>")
                    End If
                Else
                    htmlOut.Append("<td>&nbsp;</td>")
                End If
                ' End If

                htmlOut.Append("<td>")
                If Not IsDBNull(r("ADDRESS")) Then
                    htmlOut.Append(r("ADDRESS"))
                End If
                htmlOut.Append("</td>")
                htmlOut.Append("<td>")
                If Not IsDBNull(r("CITY")) Then
                    htmlOut.Append(r("CITY"))
                End If
                htmlOut.Append("</td>")
                htmlOut.Append("<td>")
                If Not IsDBNull(r("STATE")) Then
                    htmlOut.Append(r("STATE"))
                End If
                htmlOut.Append("</td>")
                htmlOut.Append("<td>")
                If Not IsDBNull(r("COUNTRY")) Then
                    htmlOut.Append(Replace(r("COUNTRY"), "United States", "U.S."))
                End If
                htmlOut.Append("</td>")

                If Not IsDBNull(r("EMAIL")) Then
                    htmlOut.Append("<td data-sort=""" & r("EMAIL") & """ ><a class=""tiny_text text_underline"" href='mailto:" & r("EMAIL") & "'>" & r("EMAIL") & "</a></td>")
                Else
                    htmlOut.Append("<td></td>")
                End If

                htmlOut.Append("<td>")
                If Not IsDBNull(r("WEB ADDRESS")) Then
                    htmlOut.Append(r("WEB ADDRESS"))
                End If
                htmlOut.Append("</td>")

                htmlOut.Append("<td>")
                If Not IsDBNull(r("OFFICE PHONE")) Then
                    htmlOut.Append(r("OFFICE PHONE"))
                End If
                htmlOut.Append("</td>")

                htmlOut.Append("</tr>")
            Next


            htmlOut.Append("</table></tbody>")
            htmlOut.Append("</td></tr></table>")
            htmlOut.Append("</div>")


            make_ac_listing = htmlOut.ToString
        Catch ex As Exception

        End Try

    End Function
    Public Function make_operator_listing(ByVal final_table As DataTable) As String

        make_operator_listing = ""

        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False

        Try

            htmlOut.Append("<div class=""Box"">")


            ' htmlOut.Append("<tr><td valign=""middle"" align=""left"" colspan=""5"" " & IIf(HttpContext.Current.Session.Item("isMobile") = False, "nowrap='nowrap'", "class='mobileAlignLeft'") & "><font class='" & HttpContext.Current.Session("FONT_CLASS_HEADER") & " mainHeading'><strong>Operator</strong> Summary</font></td></tr>")
            htmlOut.Append("<table cellpadding='3' cellspacing=""0"" width=""100%"" class='formatTable blue'><thead>")

            ' htmlOut.Append("<tr><td colspan='10' align='right'><a title='Expand' onclick=""javascript:load('WebSource.aspx?viewType=dynamic&display=table&PageTitle=OPERATORS','','scrollbars=yes,menubar=no,height=700,width=1250,resizable=yes,toolbar=no,location=no,status=no');"" class=""cursor""><strong><u>VIEW IN GRID</u></strong></a></right></td></tr>")




            htmlOut.Append("<tr><th valign=""middle"" class=""left"">Operator</th>")
            htmlOut.Append("<th valign=""middle"" class=""right"">City</th>")
            htmlOut.Append("<th valign=""middle"" class=""right"">State</th>")
            htmlOut.Append("<th valign=""middle"" class=""right"">Country</th>")

            htmlOut.Append("<th valign=""middle"" class=""right"" class=""mobileAlignBottom"">Nbr Flights</th>")
            htmlOut.Append("<th valign=""middle"" class='right'>Total Flight Hours</th>")
            'htmlOut.Append("<th valign=""middle"" class='right'>Flight<br />Time<em>(min)</em></th>") '</a>
            htmlOut.Append("<th valign=""middle"" class='right'>Est Fuel Burn (GAL)</th>")
            htmlOut.Append("<th valign=""middle"">Email</th>")
            htmlOut.Append("<th valign=""middle"">Office Phone</th>")
            'htmlOut.Append("<th valign=""middle"">First Name</th>")
            'htmlOut.Append("<th valign=""middle"">Last Name</th>")
            'htmlOut.Append("<th valign=""middle"">Title</th>")
            'htmlOut.Append("<th valign=""middle"">Contact Email</th>")
            'htmlOut.Append("<th valign=""middle"">Contact Office Number</th>")
            'htmlOut.Append("<th valign=""middle"">Contact Mobile Number</th>")
            htmlOut.Append("<th valign=""middle"">Address</th></tr>")
            htmlOut.Append("</thead><tbody>")

            For Each r As DataRow In final_table.Rows

                If Not toggleRowColor Then
                    htmlOut.Append("<tr class=""alt_row"">")
                    toggleRowColor = True
                Else
                    htmlOut.Append("<tr bgcolor=""white"">")
                    toggleRowColor = False
                End If


                If Not IsDBNull(r("comp_name")) And Not IsDBNull(r.Item("comp_id")) Then
                    If Not String.IsNullOrEmpty(r.Item("comp_name").ToString) Then ' 

                        htmlOut.Append("<td valign=""middle"" class=""left"">" & DisplayFunctions.WriteDetailsLink(0, r.Item("comp_id"), 0, 0, True, r.Item("comp_name").ToString, "text_underline tiny_text", "") & "</td>")
                    Else
                        htmlOut.Append("<td>&nbsp;</td>")
                    End If
                Else
                    htmlOut.Append("<td>&nbsp;</td>")
                End If


                If Not IsDBNull(r("comp_city")) Then
                    htmlOut.Append("<td valign=""middle"" class=""left"">" & r("comp_city") & "</td>")
                Else
                    htmlOut.Append("<td>&nbsp;</td>")
                End If

                If Not IsDBNull(r("comp_state")) Then
                    htmlOut.Append("<td valign=""middle"" class=""left"">" & r("comp_state") & "</td>")
                Else
                    htmlOut.Append("<td>&nbsp;</td>")
                End If

                If Not IsDBNull(r("comp_country")) Then
                    htmlOut.Append("<td valign=""middle"" class=""left"">" & Replace(r("comp_country"), "United States", "U.S.") & "</td>")
                Else
                    htmlOut.Append("<td>&nbsp;</td>")
                End If



                If Not IsDBNull(r("NbrFlights")) Then
                    If Not String.IsNullOrEmpty(r.Item("NbrFlights").ToString) Then '
                        htmlOut.Append("<td valign=""middle"" align=""right"">" + FormatNumber(r.Item("NbrFlights").ToString, 0, True, False, True) + "</td>")
                        '   total_distance_min = total_distance_min + FormatNumber(r.Item("flight_distance").ToString, 0, True, False, True)
                    Else
                        htmlOut.Append("<td>&nbsp;</td>")
                    End If
                Else
                    htmlOut.Append("<td>&nbsp;</td>")
                End If



                If Not IsDBNull(r("TotalFlightTimeHrs")) Then
                    If Not String.IsNullOrEmpty(r.Item("TotalFlightTimeHrs").ToString) Then '
                        htmlOut.Append("<td valign=""middle"" align=""right"">" + FormatNumber(r.Item("TotalFlightTimeHrs").ToString, 0, True, False, True) + "</td>")
                        '   total_distance_min = total_distance_min + FormatNumber(r.Item("flight_distance").ToString, 0, True, False, True)
                    Else
                        htmlOut.Append("<td>&nbsp;</td>")
                    End If
                Else
                    htmlOut.Append("<td>&nbsp;</td>")
                End If



                If Not IsDBNull(r("TotalFuelBurn")) Then
                    If Not String.IsNullOrEmpty(r.Item("TotalFuelBurn").ToString) Then
                        htmlOut.Append("<td valign=""middle"" align=""right"">" + FormatNumber(r.Item("TotalFuelBurn").ToString, 0, True, False, True) + "</td>")
                        ' total_flight_time_min = total_flight_time_min + FormatNumber(r.Item("flight_time").ToString, 0, True, False, True)
                    Else
                        htmlOut.Append("<td>&nbsp;</td>")
                    End If
                Else
                    htmlOut.Append("<td>&nbsp;</td>")
                End If



                If Not IsDBNull(r("comp_email_address")) Then
                    htmlOut.Append("<td valign=""middle"" class=""left"" data-sort=""" & r("comp_email_address") & """><a href=""mailto:" & r("comp_email_address") & """ class=""tiny_text text_underline"">" & r("comp_email_address") & "</a></td>")
                Else
                    htmlOut.Append("<td data-sort="""">&nbsp;</td>")
                End If

                If Not IsDBNull(r("comp_off_phone")) Then
                    htmlOut.Append("<td valign=""middle"" class=""left"">" & r("comp_off_phone") & "</td>")
                Else
                    htmlOut.Append("<td>&nbsp;</td>")
                End If

                'If Not IsDBNull(r("contact_first_name")) Then
                '  htmlOut.Append("<td valign=""middle"" class=""left"">" & r("contact_first_name") & "</td>")
                'Else
                '  htmlOut.Append("<td>&nbsp;</td>")
                'End If

                'If Not IsDBNull(r("contact_last_name")) Then
                '  htmlOut.Append("<td valign=""middle"" class=""left"">" & r("contact_last_name") & "</td>")
                'Else
                '  htmlOut.Append("<td>&nbsp;</td>")
                'End If

                'If Not IsDBNull(r("contact_title")) Then
                '  htmlOut.Append("<td valign=""middle"" class=""left"">" & r("contact_title") & "</td>")
                'Else
                '  htmlOut.Append("<td>&nbsp;</td>")
                'End If

                'If Not IsDBNull(r("contact_email_address")) Then
                '  htmlOut.Append("<td valign=""middle"" class=""left"">" & r("contact_email_address") & "</td>")
                'Else
                '  htmlOut.Append("<td>&nbsp;</td>")
                'End If

                'If Not IsDBNull(r("contact_off_phone")) Then
                '  htmlOut.Append("<td valign=""middle"" class=""left"">" & r("contact_off_phone") & "</td>")
                'Else
                '  htmlOut.Append("<td>&nbsp;</td>")
                'End If

                'If Not IsDBNull(r("contact_mob_phone")) Then
                '  htmlOut.Append("<td valign=""middle"" class=""left"">" & r("contact_mob_phone") & "</td>")
                'Else
                '  htmlOut.Append("<td>&nbsp;</td>")
                'End If


                If Not IsDBNull(r("comp_address1")) Then
                    htmlOut.Append("<td valign=""middle"" class=""left"">" & r("comp_address1") & "</td>")
                Else
                    htmlOut.Append("<td>&nbsp;</td>")
                End If



                htmlOut.Append("</tr>")
            Next


            htmlOut.Append("</table>")
            htmlOut.Append("</div>")


            make_operator_listing = htmlOut.ToString
        Catch ex As Exception

        End Try

    End Function










    Private Sub getFlightPathsArray(ByRef dtFlightData As DataTable, ByRef arrflightPathArray(,) As String, ByVal temp_table_1 As DataTable, ByVal temp_table_2 As DataTable)

        Dim nCounter As Integer = 0

        Dim temp_count As Integer = 0
        below_graph_label.Text = ""
        below_graph_label2.Text = ""

        Try

            If Not IsNothing(temp_table_1) Then
                If temp_table_1.Rows.Count > 0 Then
                    temp_count = temp_table_1.Rows.Count
                End If
            End If

            If Not IsNothing(temp_table_2) Then
                If temp_table_2.Rows.Count > 0 Then
                    temp_count = temp_count + temp_table_2.Rows.Count
                End If
            End If

            If Not IsNothing(dtFlightData) Then
                If dtFlightData.Rows.Count > 0 Then
                    ReDim arrflightPathArray(dtFlightData.Rows.Count + temp_count - 1, 5)

                    Call add_to_flight_array(dtFlightData, nCounter, arrflightPathArray)
                End If
            End If

            If Not IsNothing(temp_table_1) Then
                If temp_table_1.Rows.Count > 0 Then
                    Call add_to_flight_array(temp_table_1, nCounter, arrflightPathArray)
                End If
            End If

            If Not IsNothing(temp_table_2) Then
                If temp_table_2.Rows.Count > 0 Then
                    Call add_to_flight_array(temp_table_2, nCounter, arrflightPathArray)
                End If
            End If




        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in displayFAAFlightData(ByVal dtFlightData As DataTable) As String) " + ex.Message

        Finally

        End Try

    End Sub

    Public Sub add_to_flight_array(ByVal dtFlightData As DataTable, ByRef nCounter As Integer, ByRef arrflightPathArray(,) As String)


        Const ORIGIN_AP = 0
        Const ORIGIN_AP_LAT = 1
        Const ORIGIN_AP_LONG = 2

        Const DEST_AP = 3
        Const DEST_AP_LAT = 4
        Const DEST_AP_LONG = 5
        Dim origin_name As String = ""
        Dim dest_name As String = ""
        Dim last_origin As String = ""
        Dim last_dest As String = ""
        Dim total_distance As Integer = 0
        Dim temp_label As String = ""




        If nCounter <= 1 Then  ' 1 here since it is not +'ed already below 
            temp_label &= "<table cellspacing='0' cellpadding='0' border='0'>"
            temp_label &= "<tr><td><font color='#FF0000'>Original Route</font></td><td align='left'>Distance</td></tr>"
        Else
            If InStr(below_graph_label2.Text, "Proposed") = 0 Then
                temp_label &= "<table cellspacing='0' cellpadding='0' border='0'>"
                temp_label &= "<tr><td><font color='#0000FF'>Proposed Route</font></td><td align='left'>Distance</td></tr>"
            End If
        End If





        For Each r As DataRow In dtFlightData.Rows

            If Not IsDBNull(r("ORIGIN NAME")) Then
                If Not String.IsNullOrEmpty(r.Item("ORIGIN NAME").ToString) Then
                    arrflightPathArray(nCounter, ORIGIN_AP) = r.Item("ORIGIN NAME").ToString
                Else
                    arrflightPathArray(nCounter, ORIGIN_AP) = "UNK"
                End If
            Else
                arrflightPathArray(nCounter, ORIGIN_AP) = "UNK"
            End If

            origin_name = arrflightPathArray(nCounter, ORIGIN_AP)

            If Not IsDBNull(r("ORIGIN LAT")) Then
                If Not String.IsNullOrEmpty(r.Item("ORIGIN LAT").ToString) Then
                    arrflightPathArray(nCounter, ORIGIN_AP_LAT) = r.Item("ORIGIN LAT").ToString
                Else
                    arrflightPathArray(nCounter, ORIGIN_AP_LAT) = ""
                End If
            Else
                arrflightPathArray(nCounter, ORIGIN_AP_LAT) = ""
            End If

            If Not IsDBNull(r("ORIGIN LONG")) Then
                If Not String.IsNullOrEmpty(r.Item("ORIGIN LONG").ToString) Then
                    arrflightPathArray(nCounter, ORIGIN_AP_LONG) = r.Item("ORIGIN LONG").ToString
                Else
                    arrflightPathArray(nCounter, ORIGIN_AP_LONG) = ""
                End If
            Else
                arrflightPathArray(nCounter, ORIGIN_AP_LONG) = ""
            End If


            If Not IsDBNull(r("DEST NAME")) Then
                If Not String.IsNullOrEmpty(r.Item("DEST NAME").ToString) Then
                    arrflightPathArray(nCounter, DEST_AP) = r.Item("DEST NAME").ToString
                Else
                    arrflightPathArray(nCounter, DEST_AP) = "UNK"
                End If
            Else
                arrflightPathArray(nCounter, DEST_AP) = "UNK"
            End If

            dest_name = arrflightPathArray(nCounter, DEST_AP)

            If Not IsDBNull(r("DEST LAT")) Then
                If Not String.IsNullOrEmpty(r.Item("DEST LAT").ToString) Then
                    arrflightPathArray(nCounter, DEST_AP_LAT) = r.Item("DEST LAT").ToString
                Else
                    arrflightPathArray(nCounter, DEST_AP_LAT) = ""
                End If
            Else
                arrflightPathArray(nCounter, DEST_AP_LAT) = ""
            End If

            If Not IsDBNull(r("DEST LONG")) Then
                If Not String.IsNullOrEmpty(r.Item("DEST LONG").ToString) Then
                    arrflightPathArray(nCounter, DEST_AP_LONG) = r.Item("DEST LONG").ToString
                Else
                    arrflightPathArray(nCounter, DEST_AP_LONG) = ""
                End If
            Else
                arrflightPathArray(nCounter, DEST_AP_LONG) = ""
            End If

            If Trim(Flight_Id1) <> "" Then

                dest_name = Replace(dest_name, "International", "")
                dest_name = Replace(dest_name, "Airport", "")

                origin_name = Replace(origin_name, "International", "")
                origin_name = Replace(origin_name, "Airport", "")

                If Not IsDBNull(r("dest_aport_icao_code")) Then
                    dest_name &= " - " & r("dest_aport_icao_code") & "/"
                End If

                If Not IsDBNull(r("dest_aport_iata_code")) Then
                    dest_name &= r("dest_aport_iata_code")
                End If

                If Not IsDBNull(r("origin_aport_icao_code")) Then
                    origin_name &= " - " & r("origin_aport_icao_code") & "/"
                End If

                If Not IsDBNull(r("origin_aport_iata_code")) Then
                    origin_name &= r("origin_aport_iata_code")
                End If



                If Not IsDBNull(r("DISTANCE")) And ((Trim(last_origin) = "" Or Trim(last_dest) = "") Or (Trim(last_dest) <> Trim(dest_name)) Or (Trim(last_origin) <> Trim(origin_name))) Then

                    If Trim(last_origin) <> "" Then
                        If Trim(last_origin) <> Trim(origin_name) And Trim(last_dest) <> Trim(dest_name) And Trim(last_origin) <> Trim(dest_name) And Trim(last_dest) <> Trim(origin_name) Then
                            temp_label &= "<tr><td align='left' colspan='1'>Total Route Distance:</td><td align='right'>" & FormatNumber(last_distance, 0) & "</td></tr>"
                            last_distance = 0
                        End If
                    End If

                    If nCounter = 0 Or nCounter = 2 Then
                        temp_label &= "<tr><td align='left'>Leg#1: " & origin_name & " -> " & dest_name & "&nbsp;&nbsp;</td><td align='right'>" & FormatNumber(r.Item("DISTANCE"), 0) & "</td></tr>"
                    Else
                        temp_label &= "<tr><td align='left'>Leg#2: " & origin_name & " -> " & dest_name & "&nbsp;&nbsp;</td><td align='right'>" & FormatNumber(r.Item("DISTANCE"), 0) & "</td></tr>"
                    End If



                    last_distance += r.Item("DISTANCE")
                End If


                last_origin = origin_name
                last_dest = dest_name
            End If


            nCounter += 1
        Next


        If last_distance > 0 And nCounter <> 3 Then  ' dont do it with "2nd" one which is first summary one 
            temp_label &= "<tr><td align='left' colspan='1'>Total Flight Distance:</td><td align='right'>" & FormatNumber(last_distance, 0) & "</td></tr>"
            last_distance = 0

            If Trim(Flight_Id1) <> "" Then
                temp_label &= "</table>"
            End If
        End If

        If Trim(Flight_Id1) <> "" And Trim(Flight_Id2) <> "" Then
            If nCounter <= 2 Then
                below_graph_label.Text &= temp_label
            Else
                below_graph_label2.Text &= temp_label
                below_graph_label2.Visible = True
            End If
        End If


    End Sub

    Private Function buildMapFlightPaths(ByRef arrFlightPathData(,) As String, Optional ByVal pos_lat As String = "", Optional ByVal post_long As String = "") As String

        Dim htmlOut As StringBuilder = New StringBuilder()
        Dim htmlOut2 As StringBuilder = New StringBuilder()
        Dim origin_lat As String = ""
        Dim origin_long As String = ""
        Dim dest_lat As String = ""
        Dim dest_long As String = ""

        Const ORIGIN_AP_LAT = 1
        Const ORIGIN_AP_LONG = 2

        Const DEST_AP_LAT = 4
        Const DEST_AP_LONG = 5

        Try

            htmlOut.Append("<script type=""text/javascript"" language=""javascript"">" + vbCrLf)

            htmlOut.Append("  var mapOptions = {" + vbCrLf)



            If Not IsNothing(arrFlightPathData) And IsArray(arrFlightPathData) Then

                If arrFlightPathData.Length > 0 Then

                    For I As Integer = 0 To UBound(arrFlightPathData)

                        If Not String.IsNullOrEmpty(arrFlightPathData(I, 1).ToString.Trim) Then
                            origin_lat = arrFlightPathData(I, ORIGIN_AP_LAT).ToString.Trim
                        End If

                        If Not String.IsNullOrEmpty(arrFlightPathData(I, 2).ToString.Trim) Then
                            origin_long = arrFlightPathData(I, ORIGIN_AP_LONG).ToString.Trim
                        End If

                        If Not String.IsNullOrEmpty(arrFlightPathData(I, 4).ToString.Trim) Then
                            dest_lat = arrFlightPathData(I, DEST_AP_LAT).ToString.Trim
                        End If

                        If Not String.IsNullOrEmpty(arrFlightPathData(I, 5).ToString.Trim) Then
                            dest_long = arrFlightPathData(I, DEST_AP_LONG).ToString.Trim
                        End If


                        If Trim(Flight_Id1) <> "" And Trim(Me.iata_icao_search.Text) <> "" Then

                            If I = 2 Or I = 3 Then
                                If Not String.IsNullOrEmpty(origin_lat) And Not String.IsNullOrEmpty(origin_long) And Not String.IsNullOrEmpty(dest_lat) And Not String.IsNullOrEmpty(dest_long) Then
                                    htmlOut2.Append("   AddGeodesicLine_Blue(map," + origin_lat + ", " + origin_long + ", " + dest_lat + ", " + dest_long + ");" + vbCrLf)
                                End If
                            Else
                                If Not String.IsNullOrEmpty(origin_lat) And Not String.IsNullOrEmpty(origin_long) And Not String.IsNullOrEmpty(dest_lat) And Not String.IsNullOrEmpty(dest_long) Then
                                    htmlOut2.Append("   AddGeodesicLine(map," + origin_lat + ", " + origin_long + ", " + dest_lat + ", " + dest_long + ");" + vbCrLf)
                                End If
                            End If
                        Else
                            If Not String.IsNullOrEmpty(origin_lat) And Not String.IsNullOrEmpty(origin_long) And Not String.IsNullOrEmpty(dest_lat) And Not String.IsNullOrEmpty(dest_long) Then
                                htmlOut2.Append("   AddGeodesicLine(map," + origin_lat + ", " + origin_long + ", " + dest_lat + ", " + dest_long + ");" + vbCrLf)
                            End If
                        End If




                    Next

                End If
            End If

            If Trim(pos_lat) <> "" And Trim(post_long) <> "" Then
                If Session.Item("isMobile") = True Then
                    htmlOut.Append("    zoom: 3," + vbCrLf)
                Else
                    htmlOut.Append("    zoom: 4," + vbCrLf)
                End If

                htmlOut.Append("    center: new google.maps.LatLng(" & pos_lat & ", " & post_long & ")," + vbCrLf)
            ElseIf aport_id1 > 0 And aport_id2 > 0 Then
                If Session.Item("isMobile") = True Then
                    htmlOut.Append("    zoom: 4," + vbCrLf)
                Else
                    htmlOut.Append("    zoom: 5," + vbCrLf)
                End If

                htmlOut.Append("    center: new google.maps.LatLng(" & origin_lat & ", " & origin_long & ")," + vbCrLf)
            Else
                If Session.Item("isMobile") = True Then
                    htmlOut.Append("    zoom: 2," + vbCrLf)
                Else
                    htmlOut.Append("    zoom: 4," + vbCrLf)
                End If

                htmlOut.Append("    center: new google.maps.LatLng(39.2323, -95.8887)," + vbCrLf)
            End If

            htmlOut.Append("    mapTypeId: google.maps.MapTypeId.ROADMAP" + vbCrLf)
            htmlOut.Append("  };" + vbCrLf)

            htmlOut.Append("  var mapDiv = document.getElementById(""map_canvas"");" + vbCrLf)
            htmlOut.Append("  var map = new google.maps.Map(mapDiv, mapOptions);" + vbCrLf)

            htmlOut.Append(htmlOut2.ToString)

            htmlOut.Append("</script>" + vbCrLf)

        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in buildMapFlightPaths(ByRef arrFlightPathData(,) As String) As String " + ex.Message

        Finally

        End Try

        'return resulting html string
        Return htmlOut.ToString

        htmlOut = Nothing

    End Function

    Private Function displayAircraftInfo(ByRef dtAircraftInfo As DataTable, ByRef purchase_date As String, ByRef temp_amod_id As Long) As String

        Dim htmlOut As StringBuilder = New StringBuilder()
        Dim sSeparator As String = ""
        Dim sAirport_name As String = ""
        Dim sAirport_country As String = ""
        Dim mfr_year As String = ""

        Dim sQuery As New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlException As SqlClient.SqlException = Nothing
        Dim temp_title As String = ""
        Dim latitude As String = ""
        Dim longitude As String = ""
        Dim times_thro As Integer = 1
        Dim startDateDisplay As String = ""
        Dim endDateDisplay As String = ""
        Try


            If Not IsNothing(dtAircraftInfo) Then

                If dtAircraftInfo.Rows.Count > 0 Then

                    ' htmlOut.Append("<table cellpadding=""2"" cellspacing=""0"" width=""90%"">")

                    For Each r As DataRow In dtAircraftInfo.Rows

                        If times_thro = 1 Then  ' only go thro once 
                            times_thro = 2

                            ' collect additional info for "airframe times tab"
                            If Not IsDBNull(r.Item("ac_times_as_of_date")) Then
                                If Not String.IsNullOrEmpty(r.Item("ac_times_as_of_date").ToString) Then
                                    airframe_times_as_of = r.Item("ac_times_as_of_date").ToString.Trim
                                End If
                            End If

                            If Not IsDBNull(r.Item("ac_amod_id")) Then
                                temp_amod_id = r.Item("ac_amod_id")
                            End If


                            If Not IsDBNull(r.Item("ac_airframe_tot_hrs")) Then
                                If Not String.IsNullOrEmpty(r.Item("ac_airframe_tot_hrs").ToString) Then
                                    airframe_total_hours = r.Item("ac_airframe_tot_hrs").ToString.Trim
                                End If
                            End If

                            If Not IsDBNull(r.Item("ac_airframe_tot_landings")) Then
                                If Not String.IsNullOrEmpty(r.Item("ac_airframe_tot_landings").ToString) Then
                                    airframe_total_landings = r.Item("ac_airframe_tot_landings").ToString.Trim
                                End If
                            End If

                            'htmlOut.Append("<tr><td width=""40%"" align='left'><table cellpadding=""0"" cellspacing=""0"" width=""90%"" align='left'>")
                            ' htmlOut.Append("<tr><td valign=""middle"" align=""left"" nowrap=""nowrap"">")


                            'htmlOut.Append("<span class=""li""><span class=""label"">AC:</span>&nbsp;")

                            If Not IsDBNull(r.Item("ac_mfr_year")) Then
                                If Not String.IsNullOrEmpty(r.Item("ac_mfr_year").ToString) Then
                                    'htmlOut.Append("" + r.Item("ac_mfr_year").ToString.Trim + " ")
                                    temp_title &= ("" + r.Item("ac_mfr_year").ToString.Trim + " ")
                                    mfr_year = r.Item("ac_mfr_year")
                                End If
                            End If
                            htmlOut.Append("<font class=""mainHeading"">")
                            If Not IsDBNull(r.Item("amod_make_name")) Then
                                If Not String.IsNullOrEmpty(r.Item("amod_make_name").ToString) Then
                                    'htmlOut.Append(r.Item("amod_make_name").ToString.Trim)
                                    temp_title &= (r.Item("amod_make_name").ToString.Trim)
                                    htmlOut.Append("<strong>" & r.Item("amod_make_name").ToString.Trim)
                                End If
                            End If

                            If Not IsDBNull(r.Item("amod_model_name")) Then
                                If Not String.IsNullOrEmpty(r.Item("amod_model_name").ToString) Then
                                    'htmlOut.Append("&nbsp;/&nbsp;" + r.Item("amod_model_name").ToString.Trim)
                                    temp_title &= (" / " + r.Item("amod_model_name").ToString.Trim)
                                    htmlOut.Append(" " & r.Item("amod_model_name").ToString.Trim & "</strong>")
                                End If
                            End If


                            If Not IsDBNull(r.Item("ac_ser_no_full")) Then
                                If Not String.IsNullOrEmpty(r.Item("ac_ser_no_full").ToString) Then
                                    'htmlOut.Append("&nbsp;S/N:&nbsp;" + r.Item("ac_ser_no_full").ToString.Trim + "")
                                    temp_title &= (" S/N: " + r.Item("ac_ser_no_full").ToString.Trim + "")
                                    htmlOut.Append(" SN #" + r.Item("ac_ser_no_full").ToString.Trim)
                                End If
                            End If

                            SetUpStartEndDates(startDateDisplay, endDateDisplay, r("ac_purchase_date"))


                            htmlOut.Append("<br /><span><strong  id='textToChangeOnTab'>" & main_tab_container.ActiveTab.HeaderText & "</strong></span></font>")
                            acDisplayData.Text = "<div class=""padding"">"
                            Master.SetPageTitle(temp_title & " - FAA Flight Activity")
                            Master.SetPageText("") '(temp_title)
                            ' htmlOut.Append("<div class=""five columns"">")
                            If Not IsDBNull(r.Item("ac_reg_no")) Then
                                If Not String.IsNullOrEmpty(r.Item("ac_reg_no").ToString) Then

                                    acDisplayData.Text += "Registration #:&nbsp;" + r.Item("ac_reg_no").ToString.Trim

                                    If Not IsDBNull(r.Item("ac_reg_no_expiration_date")) Then
                                        If Not String.IsNullOrEmpty(r.Item("ac_reg_no_expiration_date").ToString) Then
                                            acDisplayData.Text += ("&nbsp;(<em>Expires:&nbsp;" + FormatDateTime(r.Item("ac_reg_no_expiration_date"), DateFormat.ShortDate).Trim + "</em>)")
                                        End If
                                    End If

                                    acDisplayData.Text += (", ")

                                End If
                            End If

                            If Not IsDBNull(r.Item("ac_purchase_date")) Then
                                If Not String.IsNullOrEmpty(r.Item("ac_purchase_date").ToString) Then
                                    acDisplayData.Text += ("Purchased on&nbsp;" + FormatDateTime(r.Item("ac_purchase_date"), DateFormat.ShortDate).Trim + ".")
                                    purchase_date = FormatDateTime(r.Item("ac_purchase_date"), DateFormat.ShortDate).Trim
                                End If
                            End If


                            If Not IsDBNull(r.Item("ac_reg_no")) Then
                                If Not String.IsNullOrEmpty(r.Item("ac_reg_no").ToString) Then
                                    temp_reg = r.Item("ac_reg_no")
                                End If
                            End If

                            '   If HttpContext.Current.Session.Item("jetnetWebSiteType") = crmWebClient.eWebSiteTypes.TEST Or HttpContext.Current.Session.Item("jetnetWebSiteType") = crmWebClient.eWebSiteTypes.LOCAL Then
                            If Not IsDBNull(r.Item("ac_reg_no")) Then
                                If Not String.IsNullOrEmpty(r.Item("ac_reg_no").ToString) Then
                                    temp_reg = r.Item("ac_reg_no")
                                    acDisplayData.Text += "<div class=""containerElement"">View Live Flights for <a target='_blank' href=""https://flightaware.com/live/flight/" & Replace(r.Item("ac_reg_no"), "-", "") & """ id=""link"">" & r.Item("ac_reg_no") & "</a> on Flight Aware"
                                    acDisplayData.Text += "<div class=""hiddenBox""><img id=""imagePreview"" src=""/pictures/company/flight_aware.jpg"" /></div>"
                                    acDisplayData.Text += "</div><div class=""paddedBox""></div>"
                                End If
                            End If
                            'End If


                            'htmlOut.Append("</div><div class=""five columns"">")

                            'htmlOut.Append("</td></tr></table></td><td width=""150"">")
                            'htmlOut.Append("<table cellpadding=""0"" cellspacing=""0"" width=""150"">")

                            'htmlOut.Append("<tr><td valign=""top"" align=""left"" width=""150"">"
                            acDisplayData.Text += ("<br />Located at&nbsp;")

                            If Not IsDBNull(r.Item("ac_aport_iata_code")) Then
                                If Not String.IsNullOrEmpty(r.Item("ac_aport_iata_code").ToString) Then
                                    acDisplayData.Text += (r.Item("ac_aport_iata_code").ToString.Trim)
                                    sSeparator = "&nbsp;-&nbsp;"
                                End If
                            End If

                            'If Not IsDBNull(r.Item("aport_icao_code")) Then
                            '  If Not String.IsNullOrEmpty(r.Item("aport_icao_code").ToString) Then
                            '    htmlOut.Append(sSeparator + r.Item("aport_icao_code").ToString.Trim)
                            '    sSeparator = "&nbsp;-&nbsp;"
                            '  End If
                            'End If

                            ' If Not String.IsNullOrEmpty(sSeparator.Trim) Then
                            '  htmlOut.Append(",&nbsp;")
                            ' End If

                            If Not IsDBNull(r.Item("ac_aport_name")) Then
                                If Not String.IsNullOrEmpty(r.Item("ac_aport_name").ToString) Then
                                    acDisplayData.Text += (sSeparator & Replace(r.Item("ac_aport_name").ToString.Trim, " ", "&nbsp;"))
                                    sSeparator = ",&nbsp;"
                                    sAirport_name = r.Item("ac_aport_name").ToString.Trim
                                End If
                            End If

                            If Not IsDBNull(r.Item("ac_aport_city")) Then
                                If Not String.IsNullOrEmpty(r.Item("ac_aport_city").ToString) Then
                                    acDisplayData.Text += (sSeparator + Replace(r.Item("ac_aport_city").ToString.Trim, " ", "&nbsp;"))
                                    sSeparator = ",&nbsp;"
                                End If
                            End If

                            If Not IsDBNull(r.Item("ac_aport_state")) Then
                                If Not String.IsNullOrEmpty(r.Item("ac_aport_state").ToString) Then
                                    acDisplayData.Text += (sSeparator + Replace(r.Item("ac_aport_state").ToString.Trim, " ", "&nbsp;"))
                                    sSeparator = crmWebClient.Constants.cSingleSpace
                                End If
                            End If

                            If Not IsDBNull(r.Item("ac_aport_country")) Then
                                If Not String.IsNullOrEmpty(r.Item("ac_aport_country").ToString) Then
                                    acDisplayData.Text += (sSeparator + Replace(Replace(r.Item("ac_aport_country").ToString.Trim, " ", "&nbsp;"), "United&nbsp;States", "U.S.") + "")
                                    sAirport_country = r.Item("ac_aport_country").ToString.Trim
                                    'Else
                                    '  htmlOut.Append("</span>") '</td></tr>")
                                End If
                                'Else
                                '  htmlOut.Append("</span>") '</td></tr>")
                            End If

                            acDisplayData.Text += ("</div>")
                            ' go get latitude and longitude for base airport so we can display link to map
                            If Not String.IsNullOrEmpty(sAirport_name.Trim) And Not String.IsNullOrEmpty(sAirport_country.Trim) Then


                                'Try

                                'sQuery.Append("SELECT aport_latitude_decimal, aport_longitude_decimal FROM Airport WHERE")
                                'sQuery.Append(" lower(aport_name) = '" + sAirport_name.Trim + "'")
                                'sQuery.Append(" AND lower(aport_country) = '" + sAirport_country.Trim + "'")

                                'HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />checkForFAAFlightData(ByVal sRegNumber As String, ByVal sAircraftID As Long) As Boolean</b><br />" + sQuery.ToString

                                'SqlConn.ConnectionString = Session.Item("jetnetClientDatabase").ToString.Trim
                                'SqlConn.Open()

                                'SqlCommand.Connection = SqlConn
                                'SqlCommand.CommandType = CommandType.Text
                                'SqlCommand.CommandTimeout = 60

                                'SqlCommand.CommandText = sQuery.ToString
                                'SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

                                'If SqlReader.HasRows Then
                                '  SqlReader.Read()

                                If Not IsDBNull(r.Item("aport_latitude_decimal")) Then
                                    If Not String.IsNullOrEmpty(r.Item("aport_latitude_decimal").ToString.Trim) Then
                                        latitude = r.Item("aport_latitude_decimal").ToString.Trim
                                    End If
                                End If

                                If Not IsDBNull(r.Item("aport_longitude_decimal")) Then
                                    If Not String.IsNullOrEmpty(r.Item("aport_longitude_decimal").ToString.Trim) Then
                                        longitude = r.Item("aport_longitude_decimal").ToString.Trim
                                    End If
                                End If


                                Dim menuText As String = "<div class=""dropdownSettings-sub"">"
                                menuText += "<a href=""javascript:void(0);""><img src=""images/menu.svg"" alt=""Menu"" /></a>"
                                menuText += "<div class=""dropdown-content-sub"">"
                                menuText += "<div class=""row"">"
                                menuText += "<div class=""twelve columns""><ul style=""text-align:left;"">"
                                menuText += "<li><a href=""javascript:void(0);""  onClick=""javascript:load('AirportLocationMap.aspx?aportLat=" + latitude.Trim + "&aportLong=" + longitude.Trim + "&aportTitle=" + sAirport_name.Trim + "','AirportLocationMap','');"">View Airport Map</a></li></ul>"
                                menuText += "</div>"
                                menuText += "</div>"
                                menuText += "</div>"
                                menuText += "</div>"
                                viewMapLinkLabel.Text = menuText

                                ' End If

                                '          Catch SqlException

                                '  HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in checkForFAAFlightData SQL " + SqlException.Message

                                'Finally
                                '  SqlReader = Nothing

                                '  SqlConn.Dispose()
                                '  SqlConn = Nothing

                                '  SqlCommand.Dispose()
                                '  SqlCommand = Nothing
                                'End Try

                            End If


                            'htmlOut.Append("</table></td>")
                            ' htmlOut.Append("<td nowrap='nowrap'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>")
                            'htmlOut.Append("</tr>")



                            'a.Custom Dates (which would be the default) b.Current Owner (which would get flights from your 
                            'aircraft purchase from purchase date forward - so start date would be purchase date and end date would be today)
                            ' c.Lifetime (which would get all flights from January 1 of the manufacturer year up to and including today)
                            ' Note that if you select Current Owner or Lifetime we would lock the date selections above 

                            Dim jsString As String = ""
                            jsString += " $(""#" & DropDownList_owner.ClientID & """).change(function () {"
                            jsString += " switch ($( this ).val()) {"
                            jsString += " case ""current"":"
                            jsString += " $('#" & faa_start_date.ClientID & "').val('" & purchase_date & "');"
                            jsString += " $('#" & faa_end_date.ClientID & "').val('" & Now.ToShortDateString & "');"
                            jsString += " $('#" & faa_start_date.ClientID & "').attr(""disabled"", ""disabled""); "
                            jsString += " $('#" & faa_end_date.ClientID & "').attr(""disabled"", ""disabled""); "
                            jsString += " $('#" & faa_start_date.ClientID & "').addClass(""display_disable"");"
                            jsString += " $('#" & faa_end_date.ClientID & "').addClass(""display_disable"");"
                            jsString += " break;"
                            jsString += " case ""lifetime"":"
                            jsString += " $('#" & faa_start_date.ClientID & "').val('" & "01/01/" & mfr_year & "');"
                            jsString += " $('#" & faa_end_date.ClientID & "').val('" & Now.ToShortDateString & "');"
                            jsString += " $('#" & faa_start_date.ClientID & "').attr(""disabled"",""disabled""); "
                            jsString += " $('#" & faa_end_date.ClientID & "').attr(""disabled"", ""disabled""); "
                            jsString += " $('#" & faa_start_date.ClientID & "').addClass(""display_disable"");"
                            jsString += " $('#" & faa_end_date.ClientID & "').addClass(""display_disable"");"
                            jsString += " break;"
                            jsString += " default: " 'Custom dates.
                            jsString += " $('#" & faa_start_date.ClientID & "').removeAttr(""disabled"");"
                            jsString += " $('#" & faa_end_date.ClientID & "').removeAttr(""disabled""); "
                            jsString += " $('#" & faa_start_date.ClientID & "').removeClass(""display_disable"");"
                            jsString += " $('#" & faa_end_date.ClientID & "').removeClass(""display_disable"");"
                            jsString += " break;"
                            jsString += " }"
                            jsString += " })"
                            If Not Page.IsPostBack Then
                                System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType, "dateString", " $(function() {" & jsString & "});", True)
                            Else

                                System.Web.UI.ScriptManager.RegisterClientScriptBlock(flight_data_update, Me.GetType(), "postbackDate", jsString.ToString + ";$(""#" & DropDownList_owner.ClientID & """).change();", True)
                            End If


                        End If ' only go thro once 
                    Next


                Else
                    ' htmlOut.Append("<tr><td valign=""middle"" align=""left"" colspan=""5"">No Aircraft Info Available!</td></tr>")
                    htmlOut.Append("No Aircraft Info Available!")
                End If
            Else
                htmlOut.Append("No Aircraft Info Available!")
                ' htmlOut.Append("<tr><td valign=""middle"" align=""left"" colspan=""5"">No Aircraft Info Available!</td></tr>")
            End If

            'htmlOut.Append("</table>")

        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in displayFAAFlightData(ByVal dtFlightData As DataTable) As String) " + ex.Message

        Finally

        End Try

        'return resulting html string
        Return htmlOut.ToString

        htmlOut = Nothing

    End Function
    Private Sub SetUpStartEndDates(ByRef startDateDisplay As String, ByRef endDateDisplay As String, ByVal acPurchaseDate As Object)
        'Let's grab some date ranges for the title
        If Trim(DropDownList_timeframe.SelectedValue) = "90_days" Then
            startDateDisplay = FormatDateTime(DateAdd(DateInterval.Month, -3, Date.Now.Date), DateFormat.ShortDate)
            endDateDisplay = FormatDateTime(Date.Now(), DateFormat.ShortDate)
        ElseIf Trim(DropDownList_timeframe.SelectedValue) = "last_year" Then
            startDateDisplay = FormatDateTime(DateAdd(DateInterval.Month, -12, Date.Now.Date), DateFormat.ShortDate)
            endDateDisplay = FormatDateTime(Date.Now(), DateFormat.ShortDate)
        ElseIf Trim(DropDownList_owner.SelectedValue) = "current" Or Trim(DropDownList_timeframe.SelectedValue) = "current" Then
            If Not IsDBNull(acPurchaseDate) Then
                If Not String.IsNullOrEmpty(acPurchaseDate.ToString) Then
                    startDateDisplay = FormatDateTime(acPurchaseDate, DateFormat.ShortDate)
                End If
            End If
            endDateDisplay = FormatDateTime(Date.Now(), DateFormat.ShortDate)
        ElseIf Trim(DropDownList_timeframe.SelectedValue) = "all" Then
            startDateDisplay = ""
            endDateDisplay = ""
        ElseIf Trim(DropDownList_timeframe.SelectedValue) = "date_search" Then
            startDateDisplay = faa_start_date.Text
            endDateDisplay = faa_end_date.Text
        Else
            startDateDisplay = FormatDateTime(DateAdd(DateInterval.Day, -90, Date.Now.Date), DateFormat.ShortDate)
            endDateDisplay = FormatDateTime(Date.Now(), DateFormat.ShortDate)
        End If
    End Sub
    Private Function getOriginAndDestinationData(ByVal sRegNumber As String, ByVal sAircraftID As Long, ByVal bGetOrigin As Boolean, Optional ByVal start_date As String = "", Optional ByVal end_date As String = "", Optional ByVal aport_id1 As Long = 0, Optional ByVal aport_id2 As Long = 0, Optional ByVal show_one_way As Boolean = False, Optional ByVal comp_id As Long = 0) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            If bGetOrigin Then
                sQuery.Append("SELECT DISTINCT ffd_origin_aport_id, COUNT(*) AS trip_count,  aport_name, aport_city, aport_state, aport_icao_code, aport_iata_code ")
                sQuery.Append(" FROM Aircraft WITH (NOLOCK) INNER JOIN FAA_Flight_Data WITH (NOLOCK) ON")
                sQuery.Append(" ffd_ac_id = ac_id AND ffd_journ_id = ac_journ_id")
                sQuery.Append(" INNER JOIN Airport WITH (NOLOCK) ON aport_id = ffd_origin_aport_id ")
                'ffd_reg_no = '" + sRegNumber.Trim + "' AND 
                If sAircraftID = 0 Then ' aport_id1
                    sQuery.Append(" WHERE  ffd_journ_id = 0 ")
                    If show_one_way = True Then
                        sQuery.Append("  and  (ffd_origin_aport_id = " & aport_id1 & " and ffd_dest_aport_id = " & aport_id2 & ")  ")
                    Else
                        sQuery.Append("  and (   (ffd_origin_aport_id = " & aport_id1 & " and ffd_dest_aport_id = " & aport_id2 & ")  ")
                        sQuery.Append("  or   (ffd_origin_aport_id = " & aport_id2 & " and ffd_dest_aport_id = " & aport_id1 & ")  ) ")
                    End If
                Else
                    sQuery.Append(" WHERE (ffd_ac_id = " + sAircraftID.ToString.Trim + " and ffd_journ_id = 0)")
                End If

                If Trim(start_date) <> "" Or Trim(end_date) <> "" Then
                    If Trim(start_date) <> "" Then
                        sQuery.Append(" AND ffd_date >= '" & Trim(start_date) & "' ")
                    End If
                    If Trim(end_date) <> "" Then
                        sQuery.Append(" AND ffd_date <= '" & Trim(end_date) & "' ")
                    End If
                End If

                If DropDownList_owner.SelectedValue.Contains("current") Or DropDownList_timeframe.SelectedValue.Contains("current") Then
                    sQuery.Append(" AND (ffd_date >= ac_purchase_date)")
                End If

                If DropDownList_timeframe.SelectedValue.Contains("90_days") Then
                    'sQuery.Append(" AND (DATEDIFF(day,ffd_date, GETDATE()) <= 90)")
                    sQuery.Append("and ffd_date >= dateadd(d, -90, GETDATE()) ")
                End If

                If DropDownList_timeframe.SelectedValue.Contains("last_year") Then
                    sQuery.Append("  and ffd_date >= dateadd(yy, -1, GETDATE()) ")
                    ' sQuery.Append(" AND (DATEDIFF(year, ffd_date, GETDATE()) <= 1)")
                End If

                '   sQuery.Append(" AND (YEAR(ffd_date) = 2014)")

                sQuery.Append(" GROUP BY ffd_origin_aport_id, aport_name, aport_city, aport_state, aport_icao_code, aport_iata_code  ORDER BY trip_count DESC")
            Else
                sQuery.Append("SELECT DISTINCT ffd_dest_aport_id, COUNT(*) AS trip_count,  aport_name, aport_city, aport_state, aport_icao_code, aport_iata_code ")
                sQuery.Append(" FROM Aircraft WITH (NOLOCK) INNER JOIN FAA_Flight_Data WITH (NOLOCK) ON")
                sQuery.Append(" ffd_ac_id = ac_id AND ffd_journ_id = ac_journ_id")
                sQuery.Append(" INNER JOIN Airport WITH (NOLOCK) ON (aport_iata_code = ffd_dest_aport OR aport_icao_code = ffd_dest_aport)")
                'ffd_reg_no = '" + sRegNumber.Trim + "' AND 
                If sAircraftID = 0 Then ' aport_id1
                    sQuery.Append(" WHERE  ffd_journ_id = 0 ")
                    If show_one_way = True Then
                        sQuery.Append("  and  (ffd_origin_aport_id = " & aport_id1 & " and ffd_dest_aport_id = " & aport_id2 & ")  ")
                    Else
                        sQuery.Append("  and (   (ffd_origin_aport_id = " & aport_id1 & " and ffd_dest_aport_id = " & aport_id2 & ")  ")
                        sQuery.Append("  or   (ffd_origin_aport_id = " & aport_id2 & " and ffd_dest_aport_id = " & aport_id1 & ")  ) ")
                    End If
                Else
                    sQuery.Append(" WHERE (ffd_ac_id = " + sAircraftID.ToString.Trim + " and ffd_journ_id = 0)")
                End If

                If Trim(start_date) <> "" Or Trim(end_date) <> "" Then
                    If Trim(start_date) <> "" Then
                        sQuery.Append(" AND ffd_date >= '" & Trim(start_date) & "' ")
                    End If
                    If Trim(end_date) <> "" Then
                        sQuery.Append(" AND ffd_date <= '" & Trim(end_date) & "' ")
                    End If
                End If

                If DropDownList_owner.SelectedValue.Contains("current") Or DropDownList_timeframe.SelectedValue.Contains("current") Then
                    sQuery.Append(" AND (ffd_date >= ac_purchase_date)")
                End If

                If DropDownList_timeframe.SelectedValue.Contains("90_days") Then
                    'sQuery.Append(" AND (DATEDIFF(day,ffd_date, GETDATE()) <= 90)")
                    sQuery.Append("and ffd_date >= dateadd(d, -90, GETDATE()) ")
                End If

                If DropDownList_timeframe.SelectedValue.Contains("last_year") Then
                    sQuery.Append("  and ffd_date >= dateadd(yy, -1, GETDATE()) ")
                    ' sQuery.Append(" AND (DATEDIFF(year, ffd_date, GETDATE()) <= 1)")
                End If

                ' sQuery.Append(" AND (YEAR(ffd_date) = 2014)")

                sQuery.Append(" GROUP BY ffd_dest_aport_id, aport_name, aport_city, aport_state, aport_icao_code, aport_iata_code ORDER BY trip_count DESC")
            End If

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />getOriginAndDestinationData(ByVal sRegNumber As String, ByVal sAircraftID As Long, ByVal bGetOrigin As Boolean) As DataTable</b><br />" + sQuery.ToString

            SqlConn.ConnectionString = Session.Item("jetnetClientDatabase").ToString.Trim
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
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in getOriginAndDestinationData load datatable " + constrExc.Message
            End Try

        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in getOriginAndDestinationData(ByVal sRegNumber As String, ByVal sAircraftID As Long, ByVal bGetOrigin As Boolean) As DataTable " + ex.Message
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
    Private Function getOriginAndDestinationROUTESData(ByVal sRegNumber As String, ByVal sAircraftID As Long, ByRef DropDownList_owner As System.Web.UI.WebControls.DropDownList, ByRef DropDownList_timeframe As System.Web.UI.WebControls.DropDownList, Optional ByVal start_date As String = "", Optional ByVal end_date As String = "", Optional ByVal aport_id1 As Long = 0, Optional ByVal aport_id2 As Long = 0, Optional ByVal show_one_way As Boolean = False, Optional ByVal comp_id As Long = 0) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            HttpContext.Current.Session.Item("Selection_Listing_Fields") = ""
            HttpContext.Current.Session.Item("Selection_Listing_Table") = ""
            HttpContext.Current.Session.Item("Selection_Listing_Where") = " "
            HttpContext.Current.Session.Item("Selection_Listing_Group") = ""
            HttpContext.Current.Session.Item("Selection_Listing_Order") = ""




            sQuery.Append(" SELECT DISTINCT ffd_origin_aport_id, ffd_dest_aport_id, COUNT(*) AS trip_count  ")
            sQuery.Append(" ,a1.aport_name as Origin_Name, a1.aport_city as Origin_City, a1.aport_state as Origin_State, a1.aport_icao_code as Origin_icao, a1.aport_iata_code  as Origin_iata ")
            sQuery.Append(" ,a2.aport_name as Dest_Name, a2.aport_city as Dest_City, a2.aport_state as Dest_State, a2.aport_icao_code as Dest_icao, a2.aport_iata_code  as Dest_iata ")

            HttpContext.Current.Session.Item("Selection_Listing_Fields") = sQuery.ToString

            HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(sQuery.ToString, "SELECT ", "")
            HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "ffd_origin_aport_id,", "")
            HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "ffd_dest_aport_id,", "")
            HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "Origin_Name", "'Origin Name'")
            HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "Origin_City", "'Origin City'")
            HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "Origin_State", "'Origin State'")
            HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "Origin_icao", "'Origin Icao'")
            HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "Origin_iata", "'Origin Iata'")
            HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "Dest_Name", "'Dest Name'")
            HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "Dest_City", "'Dest City'")
            HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "Dest_State", "'Dest State'")
            HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "Dest_icao", "'Dest Icao'")
            HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "Dest_iata", "'Dest Iata'")





            HttpContext.Current.Session.Item("Selection_Listing_Table") = (" FROM Aircraft WITH (NOLOCK)  ")
            HttpContext.Current.Session.Item("Selection_Listing_Table") &= (" INNER JOIN FAA_Flight_Data WITH (NOLOCK) ON ffd_ac_id = ac_id AND ffd_journ_id = ac_journ_id  ")
            HttpContext.Current.Session.Item("Selection_Listing_Table") &= (" INNER JOIN Airport a1 WITH (NOLOCK) ON a1.aport_id = ffd_origin_aport_id   ")
            HttpContext.Current.Session.Item("Selection_Listing_Table") &= (" INNER JOIN Airport a2 WITH (NOLOCK) ON a2.aport_id = ffd_dest_aport_id   ")

            sQuery.Append(HttpContext.Current.Session.Item("Selection_Listing_Table"))


            HttpContext.Current.Session.Item("Selection_Listing_Where") = ""

            If sAircraftID = 0 Then ' aport_id1
                HttpContext.Current.Session.Item("Selection_Listing_Where") &= (" WHERE  ffd_journ_id = 0 ")
                If show_one_way = True Then
                    HttpContext.Current.Session.Item("Selection_Listing_Where") &= ("  and  (ffd_origin_aport_id = " & aport_id1 & " and ffd_dest_aport_id = " & aport_id2 & ")  ")
                Else
                    HttpContext.Current.Session.Item("Selection_Listing_Where") &= ("  and (   (ffd_origin_aport_id = " & aport_id1 & " and ffd_dest_aport_id = " & aport_id2 & ")  ")
                    HttpContext.Current.Session.Item("Selection_Listing_Where") &= ("  or   (ffd_origin_aport_id = " & aport_id2 & " and ffd_dest_aport_id = " & aport_id1 & ")  ) ")
                End If
            Else
                HttpContext.Current.Session.Item("Selection_Listing_Where") &= (" WHERE (ffd_ac_id = " & sAircraftID & " and ffd_journ_id = 0) ")
            End If

            If DropDownList_owner.SelectedValue.Contains("current") Or DropDownList_timeframe.SelectedValue.Contains("current") Then
                If Trim(sAircraftID) > 0 Then
                    HttpContext.Current.Session.Item("Selection_Listing_Where") &= ("  AND (ffd_date >= (select distinct ac_purchase_date from Aircraft where ac_journ_id = 0 and ac_id = ffd_ac_id)) ")
                Else
                    HttpContext.Current.Session.Item("Selection_Listing_Where") &= ("  AND (ffd_date >= (select distinct ac_purchase_date from Aircraft where ac_journ_id = 0 and ac_id = " & sAircraftID & ")) ")
                End If
            End If
            If DropDownList_timeframe.SelectedValue.Contains("90_days") Then
                HttpContext.Current.Session.Item("Selection_Listing_Where") &= ("and ffd_date >= dateadd(d, -90, GETDATE()) ")
            End If

            If DropDownList_timeframe.SelectedValue.Contains("last_year") Then
                HttpContext.Current.Session.Item("Selection_Listing_Where") &= ("  and ffd_date >= dateadd(yy, -1, GETDATE()) ")
            End If

            If Trim(start_date) <> "" Or Trim(end_date) <> "" Then
                If Trim(start_date) <> "" Then
                    HttpContext.Current.Session.Item("Selection_Listing_Where") &= (" AND ffd_date >= '" & Trim(start_date) & "' ")
                End If
                If Trim(end_date) <> "" Then
                    HttpContext.Current.Session.Item("Selection_Listing_Where") &= (" AND ffd_date <= '" & Trim(end_date) & "' ")
                End If
            End If

            sQuery.Append(HttpContext.Current.Session.Item("Selection_Listing_Where"))

            HttpContext.Current.Session.Item("Selection_Listing_Group") = ""
            HttpContext.Current.Session.Item("Selection_Listing_Group") = (" GROUP BY ffd_origin_aport_id, ffd_dest_aport_id, ")
            HttpContext.Current.Session.Item("Selection_Listing_Group") &= (" a1.aport_name, a1.aport_city, a1.aport_state, a1.aport_icao_code, a1.aport_iata_code   ")
            HttpContext.Current.Session.Item("Selection_Listing_Group") &= (" ,a2.aport_name, a2.aport_city, a2.aport_state, a2.aport_icao_code, a2.aport_iata_code   ")


            HttpContext.Current.Session.Item("Selection_Listing_Order") = (" ORDER BY trip_count DESC ")


            sQuery.Append(HttpContext.Current.Session.Item("Selection_Listing_Group"))
            sQuery.Append(HttpContext.Current.Session.Item("Selection_Listing_Order"))

            ' replace after 
            HttpContext.Current.Session.Item("Selection_Listing_Group") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Group"), "ffd_origin_aport_id,", "")
            HttpContext.Current.Session.Item("Selection_Listing_Group") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Group"), "ffd_dest_aport_id,", "")


            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />getOriginAndDestinationData(ByVal sRegNumber As String, ByVal sAircraftID As Long, ByVal bGetOrigin As Boolean) As DataTable</b><br />" + sQuery.ToString

            SqlConn.ConnectionString = Session.Item("jetnetClientDatabase").ToString.Trim
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
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in getOriginAndDestinationROUTESData load datatable " + constrExc.Message
            End Try

        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in getOriginAndDestinationROUTESData(ByVal sRegNumber As String, ByVal sAircraftID As Long, ByVal bGetOrigin As Boolean) As DataTable " + ex.Message
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
    Private Function getOriginAndDestinationPAIRSData(ByVal sRegNumber As String, ByVal sAircraftID As Long, ByRef DropDownList_owner As System.Web.UI.WebControls.DropDownList, ByRef DropDownList_timeframe As System.Web.UI.WebControls.DropDownList, Optional ByVal start_date As String = "", Optional ByVal end_date As String = "", Optional ByVal aport_id1 As Long = 0, Optional ByVal aport_id2 As Long = 0, Optional ByVal show_one_way As Boolean = False, Optional ByVal comp_id As Long = 0) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            HttpContext.Current.Session.Item("Selection_Listing_Fields") = ""
            HttpContext.Current.Session.Item("Selection_Listing_Fields") &= (" select distinct  ffd_origin_aport_id,  ffd_dest_aport_id, ")
            HttpContext.Current.Session.Item("Selection_Listing_Fields") &= (" (select COUNT(*)  ")
            HttpContext.Current.Session.Item("Selection_Listing_Fields") &= (" FROM Aircraft a2 WITH (NOLOCK)    ")
            HttpContext.Current.Session.Item("Selection_Listing_Fields") &= (" INNER JOIN FAA_Flight_Data f2 WITH (NOLOCK) ON f2.ffd_ac_id = a2.ac_id AND f2.ffd_journ_id = a2.ac_journ_id  ")
            HttpContext.Current.Session.Item("Selection_Listing_Fields") &= (" WHERE (f2.ffd_ac_id =  " & sAircraftID & " and f2.ffd_journ_id = 0)   ")

            If DropDownList_owner.SelectedValue.Contains("current") Or DropDownList_timeframe.SelectedValue.Contains("current") Then
                If Trim(sAircraftID) > 0 Then
                    HttpContext.Current.Session.Item("Selection_Listing_Fields") &= ("  AND (f2.ffd_date >= (select distinct ac_purchase_date from Aircraft where ac_journ_id = 0 and ac_id = f2.ffd_ac_id)) ")
                Else
                    HttpContext.Current.Session.Item("Selection_Listing_Fields") &= ("  AND (f2.ffd_date >= (select distinct ac_purchase_date from Aircraft where ac_journ_id = 0 and ac_id = " & sAircraftID & ")) ")
                End If
            End If
            If DropDownList_timeframe.SelectedValue.Contains("90_days") Then
                HttpContext.Current.Session.Item("Selection_Listing_Fields") &= ("and f2.ffd_date >= dateadd(d^ -90^ GETDATE()) ")
            End If

            If DropDownList_timeframe.SelectedValue.Contains("last_year") Then
                HttpContext.Current.Session.Item("Selection_Listing_Fields") &= ("  and f2.ffd_date >= dateadd(yy^ -1^ GETDATE()) ")
            End If

            If Trim(start_date) <> "" Or Trim(end_date) <> "" Then
                If Trim(start_date) <> "" Then
                    HttpContext.Current.Session.Item("Selection_Listing_Fields") &= (" AND ffd_date >= '" & Trim(start_date) & "' ")
                End If
                If Trim(end_date) <> "" Then
                    HttpContext.Current.Session.Item("Selection_Listing_Fields") &= (" AND ffd_date <= '" & Trim(end_date) & "' ")
                End If
            End If


            HttpContext.Current.Session.Item("Selection_Listing_Fields") &= (" and f2.ffd_dest_aport_id = FAA_Flight_Data.ffd_origin_aport_id ")
            HttpContext.Current.Session.Item("Selection_Listing_Fields") &= (" and f2.ffd_origin_aport_id = FAA_Flight_Data.ffd_dest_aport_id ")
            HttpContext.Current.Session.Item("Selection_Listing_Fields") &= (" ) as trips_back ")
            HttpContext.Current.Session.Item("Selection_Listing_Fields") &= (" , ")
            HttpContext.Current.Session.Item("Selection_Listing_Fields") &= (" COUNT(*) AS trip_out   ")

            HttpContext.Current.Session.Item("Selection_Listing_Fields") &= (" , ")
            HttpContext.Current.Session.Item("Selection_Listing_Fields") &= (" a1.aport_name as Origin_Name, a1.aport_city as Origin_City, ")
            HttpContext.Current.Session.Item("Selection_Listing_Fields") &= (" a1.aport_state as Origin_State, a1.aport_icao_code as Origin_icao, a1.aport_iata_code  as Origin_iata  ,a2.aport_name as Dest_Name,  ")
            HttpContext.Current.Session.Item("Selection_Listing_Fields") &= (" a2.aport_city as Dest_City, a2.aport_state as Dest_State, a2.aport_icao_code as Dest_icao, a2.aport_iata_code  as Dest_iata   ")

            sQuery.Append(Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "^", ","))

            HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "ffd_origin_aport_id,", "ffd_origin_aport_id as 'Origin Airport ID',")
            HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "ffd_dest_aport_id,", "ffd_dest_aport_id as 'Dest Airport ID',")


            HttpContext.Current.Session.Item("Selection_Listing_Table") = ""
            HttpContext.Current.Session.Item("Selection_Listing_Table") &= (" FROM Aircraft WITH (NOLOCK)    ")
            HttpContext.Current.Session.Item("Selection_Listing_Table") &= (" INNER JOIN FAA_Flight_Data WITH (NOLOCK) ON ffd_ac_id = ac_id AND ffd_journ_id = ac_journ_id   ")
            HttpContext.Current.Session.Item("Selection_Listing_Table") &= (" INNER JOIN Airport a1 WITH (NOLOCK) ON a1.aport_id = ffd_origin_aport_id ")
            HttpContext.Current.Session.Item("Selection_Listing_Table") &= (" INNER JOIN Airport a2 WITH (NOLOCK) ON a2.aport_id = ffd_dest_aport_id ")

            sQuery.Append(HttpContext.Current.Session.Item("Selection_Listing_Table"))



            HttpContext.Current.Session.Item("Selection_Listing_Where") = ""

            If sAircraftID = 0 Then ' aport_id1
                HttpContext.Current.Session.Item("Selection_Listing_Where") &= (" WHERE  ffd_journ_id = 0 ")
                If show_one_way = True Then
                    HttpContext.Current.Session.Item("Selection_Listing_Where") &= ("  and  (ffd_origin_aport_id = " & aport_id1 & " and ffd_dest_aport_id = " & aport_id2 & ")  ")
                Else
                    HttpContext.Current.Session.Item("Selection_Listing_Where") &= ("  and (   (ffd_origin_aport_id = " & aport_id1 & " and ffd_dest_aport_id = " & aport_id2 & ")  ")
                    HttpContext.Current.Session.Item("Selection_Listing_Where") &= ("  or   (ffd_origin_aport_id = " & aport_id2 & " and ffd_dest_aport_id = " & aport_id1 & ")  ) ")
                End If
            Else
                HttpContext.Current.Session.Item("Selection_Listing_Where") &= (" WHERE (ffd_ac_id = " & sAircraftID & " and ffd_journ_id = 0)  ")

                If DropDownList_owner.SelectedValue.Contains("current") Or DropDownList_timeframe.SelectedValue.Contains("current") Then
                    If Trim(sAircraftID) > 0 Then
                        HttpContext.Current.Session.Item("Selection_Listing_Where") &= ("  AND (ffd_date >= (select distinct ac_purchase_date from Aircraft where ac_journ_id = 0 and ac_id = ffd_ac_id)) ")
                    Else
                        HttpContext.Current.Session.Item("Selection_Listing_Where") &= ("  AND (ffd_date >= (select distinct ac_purchase_date from Aircraft where ac_journ_id = 0 and ac_id = " & sAircraftID & ")) ")
                    End If
                End If
            End If

            If DropDownList_timeframe.SelectedValue.Contains("90_days") Then
                HttpContext.Current.Session.Item("Selection_Listing_Where") &= ("and ffd_date >= dateadd(d, -90, GETDATE()) ")
            End If

            If DropDownList_timeframe.SelectedValue.Contains("last_year") Then
                HttpContext.Current.Session.Item("Selection_Listing_Where") &= ("  and ffd_date >= dateadd(yy, -1, GETDATE()) ")
            End If


            If Trim(start_date) <> "" Or Trim(end_date) <> "" Then
                If Trim(start_date) <> "" Then
                    HttpContext.Current.Session.Item("Selection_Listing_Where") &= (" AND ffd_date >= '" & Trim(start_date) & "' ")
                End If
                If Trim(end_date) <> "" Then
                    HttpContext.Current.Session.Item("Selection_Listing_Where") &= (" AND ffd_date <= '" & Trim(end_date) & "' ")
                End If
            End If

            sQuery.Append(HttpContext.Current.Session.Item("Selection_Listing_Where"))

            HttpContext.Current.Session.Item("Selection_Listing_Group") = ("  GROUP BY ffd_origin_aport_id, ffd_dest_aport_id, a1.aport_name, a1.aport_city, a1.aport_state, a1.aport_icao_code, a1.aport_iata_code,a2.aport_name, a2.aport_city, a2.aport_state, a2.aport_icao_code, a2.aport_iata_code     ")
            HttpContext.Current.Session.Item("Selection_Listing_Order") = (" ORDER BY trip_out DESC  ")


            sQuery.Append(HttpContext.Current.Session.Item("Selection_Listing_Group"))
            sQuery.Append(HttpContext.Current.Session.Item("Selection_Listing_Order"))


            '   HttpContext.Current.Session.Item("Selection_Listing_Group") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Group"), "ffd_origin_aport_id,", "")
            '  HttpContext.Current.Session.Item("Selection_Listing_Group") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Group"), "ffd_dest_aport_id,", "")


            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />getOriginAndDestinationData(ByVal sRegNumber As String, ByVal sAircraftID As Long, ByVal bGetOrigin As Boolean) As DataTable</b><br />" + sQuery.ToString

            SqlConn.ConnectionString = Session.Item("jetnetClientDatabase").ToString.Trim
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
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in getOriginAndDestinationPAIRSData load datatable " + constrExc.Message
            End Try

        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in getOriginAndDestinationPAIRSData(ByVal sRegNumber As String, ByVal sAircraftID As Long, ByVal bGetOrigin As Boolean) As DataTable " + ex.Message
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

    Private Function displayOriginAndDestinationData(ByRef dtOriginAndDestinationData As DataTable, ByVal bGetOrigin As Boolean, ByVal action_date As String) As String

        Dim htmlOut As StringBuilder = New StringBuilder()
        Dim toggleRowColor As Boolean = False
        Dim sSeparator As String = ""
        Dim temp_label_string As String = ""

        Try

            temp_label_string = DropDownList_timeframe.SelectedValue

            If temp_label_string = "date_search" Then
                temp_label_string = "date range"
                temp_label_string &= " (" & Me.faa_start_date.Text & " - " & Me.faa_end_date.Text & ") "
            End If

            Call flight_data_temp.get_title_for_time_period(temp_label_string, action_date, True, "")

            htmlOut.Append(comp_functions.NEW_build_style_page_full_spec(False, False, 998))

            htmlOut.Append("<div class=""Box"">")

            'htmlOut.Append("<table width=""100%""><tr><td>")

            'If bGetOrigin Then
            '  htmlOut.Append("<font class='" & Session("FONT_CLASS_HEADER") & " mainHeading'><strong>ORIGINS</strong></font>")
            'Else
            '  htmlOut.Append("<font class='" & Session("FONT_CLASS_HEADER") & " mainHeading'>DESTINATIONS</strong></font>")
            'End If

            'htmlOut.Append("</td></tr><tr><td>")

            htmlOut.Append("<table cellpadding='3' cellspacing=""0"" width=""100%"" class='formatTable blue'><thead>")

            If Not IsNothing(dtOriginAndDestinationData) Then

                If dtOriginAndDestinationData.Rows.Count > 0 Then

                    If bGetOrigin Then
                        htmlOut.Append("<tr><th valign=""middle"" align=""left"" " & IIf(Session.Item("isMobile") = False, "nowrap='nowrap'", "") & "><strong>ORIGIN AIRPORT</strong></th>")
                    Else
                        htmlOut.Append("<tr><th valign=""middle"" align=""left"" " & IIf(Session.Item("isMobile") = False, "nowrap='nowrap'", "") & "><strong>DESTINATION AIRPORT</strong></th>")
                    End If

                    htmlOut.Append("<th valign=""middle"" class=""right"" " & IIf(Session.Item("isMobile") = False, "nowrap='nowrap'", "") & "><strong>#Flights</strong></th></tr>")
                    htmlOut.Append("</thead><tbody>")


                    For Each r As DataRow In dtOriginAndDestinationData.Rows

                        If Not toggleRowColor Then
                            htmlOut.Append("<tr><td valign=""middle"" align=""left"">")
                            toggleRowColor = True
                        Else
                            htmlOut.Append("<tr><td valign=""middle"" align=""left"">")
                            toggleRowColor = False
                        End If
                        '   htmlOut.Append("<font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>")
                        If Not IsDBNull(r.Item("aport_name")) Then
                            If Not String.IsNullOrEmpty(r.Item("aport_name").ToString) Then
                                htmlOut.Append(Replace(r.Item("aport_name").ToString.Trim, " ", "&nbsp;") + " - ")
                            End If
                        End If

                        If Not IsDBNull(r.Item("aport_city")) Then
                            If Not String.IsNullOrEmpty(r.Item("aport_city").ToString) Then
                                htmlOut.Append("" + Replace(r.Item("aport_city").ToString.Trim, " ", "&nbsp;") + "")
                                sSeparator = ",&nbsp;"
                            End If
                        End If


                        If Not IsDBNull(r.Item("aport_state")) Then
                            If Not String.IsNullOrEmpty(r.Item("aport_state").ToString) Then
                                htmlOut.Append(sSeparator + "" + Replace(r.Item("aport_state").ToString.Trim, " ", "&nbsp;") + "")
                            End If
                        End If


                        htmlOut.Append("&nbsp;(")

                        If Not IsDBNull(r.Item("aport_icao_code")) Then
                            If Not String.IsNullOrEmpty(r.Item("aport_icao_code").ToString) Then
                                htmlOut.Append("" + Replace(r.Item("aport_icao_code").ToString.Trim, " ", "&nbsp;") + "")
                            End If
                        End If


                        If Not IsDBNull(r.Item("aport_iata_code")) Then
                            If Not String.IsNullOrEmpty(r.Item("aport_iata_code").ToString) Then
                                htmlOut.Append(sSeparator + "&nbsp;" + Replace(r.Item("aport_iata_code").ToString.Trim, " ", "&nbsp;") + "")
                            End If
                        End If

                        htmlOut.Append(")")

                        '    htmlOut.Append("</font>")
                        htmlOut.Append("</td>")

                        '<font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>
                        If Not IsDBNull(r("trip_count")) Then
                            If Not String.IsNullOrEmpty(r.Item("trip_count").ToString) Then '
                                htmlOut.Append("<td valign=""middle"" align=""right"" class='table_specs'>" + FormatNumber(r.Item("trip_count").ToString, 0, True, False, True) + "</td></tr>")
                            Else
                                htmlOut.Append("<td class='table_specs'>&nbsp;</td></tr>")
                            End If
                        Else
                            htmlOut.Append("<td class='table_specs'>&nbsp;</td></tr>")
                        End If

                    Next

                Else
                    If bGetOrigin Then
                        htmlOut.Append("<tr><td valign=""middle"" align=""center"" colspan=""3"" class='table_specs'>No top origins data available!</td></tr>")
                    Else
                        htmlOut.Append("<tr><td valign=""middle"" align=""center"" colspan=""3"" class='table_specs'>No top destinations data available!</td></tr>")
                    End If
                End If
            Else
                If bGetOrigin Then
                    htmlOut.Append("<tr><td valign=""middle"" align=""center"" colspan=""3"" class='table_specs'>No top origins data available!</td></tr>")
                Else
                    htmlOut.Append("<tr><td valign=""middle"" align=""center"" colspan=""3"" class='table_specs'>No top destinations data available!</td></tr>")
                End If
            End If

            htmlOut.Append("</body></table>")

            htmlOut.Append("</td></tr></table>")
            htmlOut.Append("</div>")

        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in displayOriginAndDestinationData(ByRef dtOriginAndDestinationData As DataTable, ByVal bGetOrigin As Boolean) As String " + ex.Message

        Finally

        End Try

        'return resulting html string
        Return htmlOut.ToString

        htmlOut = Nothing

    End Function
    Private Function displayCITYPAIRSData(ByRef dtOriginAndDestinationData As DataTable, ByVal bGetOrigin As Boolean, ByVal action_date As String) As String

        Dim htmlOut As StringBuilder = New StringBuilder()
        Dim toggleRowColor As Boolean = False
        Dim sSeparator As String = ""
        Dim temp_label_string As String = ""
        Dim city1_name(5000) As String
        Dim city2_name(5000) As String
        Dim city_total(5000) As Integer
        Dim count_city As Long
        Dim city_string As String = ""
        Dim city_string2 As String = ""
        Dim city_temp As String = ""
        Dim temp_count As Integer = 0
        Dim match_found As Boolean = False

        Try

            temp_label_string = DropDownList_timeframe.SelectedValue

            If temp_label_string = "date_search" Then
                temp_label_string = "date range"
                temp_label_string &= " (" & Me.faa_start_date.Text & " - " & Me.faa_end_date.Text & ") "
            End If

            Call flight_data_temp.get_title_for_time_period(temp_label_string, action_date, True, "")


            htmlOut.Append(comp_functions.NEW_build_style_page_full_spec(False, False, 998))

            htmlOut.Append("<div class=""Box"">")


            ' htmlOut.Append("<tr><td valign=""middle"" align=""center"" colspan=""2"" " & IIf(Session.Item("isMobile") = False, "nowrap='nowrap'", "") & "><font class='mainHeading'><strong>" & Replace(temp_label_string, "(", "</strong>(") & "</font></td></tr>")

            ' htmlOut.Append("<a title='Expand' onclick=""javascript:load('WebSource.aspx?viewType=dynamic&display=table&PageTitle=CITY PAIRS','','scrollbars=yes,menubar=no,height=700,width=1250,resizable=yes,toolbar=no,location=no,status=no');""  class=""cursor""><strong><u>VIEW IN GRID</u></strong></a>")
            htmlOut.Append("<table cellpadding='5' cellspacing=""0"" width=""100%"" class='formatTable blue'><thead>")


            If Not IsNothing(dtOriginAndDestinationData) Then

                If dtOriginAndDestinationData.Rows.Count > 0 Then

                    htmlOut.Append("<tr><th valign=""middle"" align=""left"" " & IIf(Session.Item("isMobile") = False, "nowrap='nowrap'", "") & "><strong>Airport 1 - Location</strong></th>")
                    htmlOut.Append("<th valign=""middle"" align=""left"" " & IIf(Session.Item("isMobile") = False, "nowrap='nowrap'", "") & "><strong>Airport 2 - Location</strong></th>")
                    htmlOut.Append("<th valign=""middle"" class=""right"" align=""right"" " & IIf(Session.Item("isMobile") = False, "nowrap='nowrap'", "") & "><strong>#Flights</strong></th></tr>")
                    htmlOut.Append("</thead><tbody>")

                    For Each r As DataRow In dtOriginAndDestinationData.Rows



                        city_string = ""
                        If Not IsDBNull(r.Item("Origin_name")) Then
                            If Not String.IsNullOrEmpty(r.Item("Origin_name").ToString) Then
                                city_string &= (Replace(Replace(r.Item("Origin_name").ToString.Trim, " ", "&nbsp;"), "Airport", "") + " - ")
                            End If
                        End If

                        If Not IsDBNull(r.Item("Origin_city")) Then
                            If Not String.IsNullOrEmpty(r.Item("Origin_city").ToString) Then
                                city_string &= ("<em>" + Replace(r.Item("Origin_city").ToString.Trim, " ", "&nbsp;") + "</em>")
                                sSeparator = ",&nbsp;"
                            End If
                        End If


                        If Not IsDBNull(r.Item("Origin_state")) Then
                            If Not String.IsNullOrEmpty(r.Item("Origin_state").ToString) Then
                                city_string &= (sSeparator + "<em>" + Replace(r.Item("Origin_state").ToString.Trim, " ", "&nbsp;") + "</em>")
                            End If
                        End If


                        'city_string &= ("&nbsp;(")

                        'If Not IsDBNull(r.Item("Origin_icao")) Then
                        '  If Not String.IsNullOrEmpty(r.Item("Origin_icao").ToString) Then
                        '    city_string &= ("<em>" + Replace(r.Item("Origin_icao").ToString.Trim, " ", "&nbsp;") + "</em>")
                        '  End If
                        'End If


                        'If Not IsDBNull(r.Item("Origin_iata")) Then
                        '  If Not String.IsNullOrEmpty(r.Item("Origin_iata").ToString) Then
                        '    city_string &= (sSeparator + "&nbsp;<em>" + Replace(r.Item("Origin_iata").ToString.Trim, " ", "&nbsp;") + "</em>")
                        '  End If
                        'End If

                        'city_string &= (")")


                        city_string2 = ""
                        If Not IsDBNull(r.Item("Dest_name")) Then
                            If Not String.IsNullOrEmpty(r.Item("Dest_name").ToString) Then
                                city_string2 &= (Replace(Replace(r.Item("Dest_name").ToString.Trim, " ", "&nbsp;"), "Airport", "") + " - ")
                            End If
                        End If

                        If Not IsDBNull(r.Item("Dest_city")) Then
                            If Not String.IsNullOrEmpty(r.Item("Dest_city").ToString) Then
                                city_string2 &= ("<em>" + Replace(r.Item("Dest_city").ToString.Trim, " ", "&nbsp;") + "</em>")
                                sSeparator = ",&nbsp;"
                            End If
                        End If


                        If Not IsDBNull(r.Item("Dest_state")) Then
                            If Not String.IsNullOrEmpty(r.Item("Dest_state").ToString) Then
                                city_string2 &= (sSeparator + "<em>" + Replace(r.Item("Dest_state").ToString.Trim, " ", "&nbsp;") + "</em>")
                            End If
                        End If


                        'city_string2 &= ("&nbsp;(")

                        'If Not IsDBNull(r.Item("Dest_icao")) Then
                        '  If Not String.IsNullOrEmpty(r.Item("Dest_icao").ToString) Then
                        '    city_string2 &= ("<em>" + Replace(r.Item("Dest_icao").ToString.Trim, " ", "&nbsp;") + "</em>")
                        '  End If
                        'End If


                        'If Not IsDBNull(r.Item("Dest_iata")) Then
                        '  If Not String.IsNullOrEmpty(r.Item("Dest_iata").ToString) Then
                        '    city_string2 &= (sSeparator + "&nbsp;<em>" + Replace(r.Item("Dest_iata").ToString.Trim, " ", "&nbsp;") + "</em>")
                        '  End If
                        'End If

                        'city_string2 &= (")")



                        If Trim(city_string2) < Trim(city_string) Then
                            city_temp = city_string
                            city_string = city_string2
                            city_string2 = city_temp
                        End If

                        temp_count = 0
                        If Not IsDBNull(r("trip_out")) Then
                            temp_count = r("trip_out")
                        End If

                        If Not IsDBNull(r("trips_back")) Then
                            temp_count = temp_count + r("trips_back")
                        End If

                        match_found = False
                        For i = 0 To count_city - 1
                            If Trim(city1_name(i)) = Trim(city_string) And Trim(city2_name(i)) = Trim(city_string2) Then
                                match_found = True
                            End If
                        Next

                        If match_found = False Then
                            city1_name(count_city) = city_string
                            city2_name(count_city) = city_string2
                            city_total(count_city) = temp_count
                            count_city = count_city + 1
                        End If

                    Next

                    ' this is to sort by count
                    For k = 0 To count_city - 1
                        For i = 0 To count_city - 1
                            If city_total(i) < city_total(i + 1) Then
                                temp_count = city_total(i)
                                city_total(i) = city_total(i + 1)
                                city_total(i + 1) = temp_count

                                city_temp = city1_name(i)
                                city1_name(i) = city1_name(i + 1)
                                city1_name(i + 1) = city_temp

                                city_temp = city2_name(i)
                                city2_name(i) = city2_name(i + 1)
                                city2_name(i + 1) = city_temp
                            End If
                        Next
                    Next



                    For i = 0 To count_city - 1
                        'If i > 19 Then ' only do top 20 
                        ' Else
                        If Not toggleRowColor Then
                            htmlOut.Append("<tr class=""alt_row"">")
                            toggleRowColor = True
                        Else
                            htmlOut.Append("<tr bgcolor=""white"">")
                            toggleRowColor = False
                        End If


                        htmlOut.Append("<td valign=""middle"" align=""left""><span>")
                        htmlOut.Append(city1_name(i))

                        htmlOut.Append("</span>")
                        htmlOut.Append("</td>")

                        htmlOut.Append("<td valign=""middle"" align=""left""><span>")

                        htmlOut.Append(city2_name(i))
                        htmlOut.Append("</span>")
                        htmlOut.Append("</td>")
                        htmlOut.Append("<td valign=""middle"" align=""right"">")
                        htmlOut.Append(city_total(i))

                        htmlOut.Append("</td></tr>")
                        ' End If
                    Next

                Else
                    If bGetOrigin Then
                        htmlOut.Append("<tr><td valign=""middle"" align=""center"" colspan=""3"">No top origins data available!</td></tr>")
                    Else
                        htmlOut.Append("<tr><td valign=""middle"" align=""center"" colspan=""3"">No top destinations data available!</td></tr>")
                    End If
                End If
            Else
                If bGetOrigin Then
                    htmlOut.Append("<tr><td valign=""middle"" align=""center"" colspan=""3"">No top origins data available!</td></tr>")
                Else
                    htmlOut.Append("<tr><td valign=""middle"" align=""center"" colspan=""3"">No top destinations data available!</td></tr>")
                End If
            End If

            htmlOut.Append("</tbody>")
            htmlOut.Append("</table>")
            htmlOut.Append("</div>")
        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in displayOriginAndDestinationData(ByRef dtOriginAndDestinationData As DataTable, ByVal bGetOrigin As Boolean) As String " + ex.Message

        Finally

        End Try

        'return resulting html string
        Return htmlOut.ToString

        htmlOut = Nothing

    End Function
    Private Function displayRoutesData(ByRef dtOriginAndDestinationData As DataTable, ByVal bGetOrigin As Boolean, ByVal action_date As String) As String

        Dim htmlOut As StringBuilder = New StringBuilder()
        Dim toggleRowColor As Boolean = False
        Dim sSeparator As String = ""
        Dim temp_label_string As String = ""

        Try

            temp_label_string = DropDownList_timeframe.SelectedValue

            If temp_label_string = "date_search" Then
                temp_label_string = "date range"
                temp_label_string &= " (" & Me.faa_start_date.Text & " - " & Me.faa_end_date.Text & ") "
            End If

            Call flight_data_temp.get_title_for_time_period(temp_label_string, action_date, True, "")

            htmlOut.Append(comp_functions.NEW_build_style_page_full_spec(False, False, 998))

            htmlOut.Append("<div class=""Box"">")



            'htmlOut.Append("<tr><td valign=""middle"" align=""center"" colspan=""2"" " & IIf(Session.Item("isMobile") = False, "nowrap='nowrap'", "") & "><span class=""mainHeading""><strong>ROUTES " & Replace(temp_label_string, "(", "</strong>(") & "</span></td></tr>")
            'htmlOut.Append("<a title='Expand' onclick=""javascript:load('WebSource.aspx?viewType=dynamic&display=table&PageTitle=ROUTES','','scrollbars=yes,menubar=no,height=700,width=1250,resizable=yes,toolbar=no,location=no,status=no');"" class=""cursor""><u>VIEW IN GRID</u></a></right>")


            htmlOut.Append("<table cellpadding='5' cellspacing=""0"" width=""100%"" class='formatTable blue'>")

            If Not IsNothing(dtOriginAndDestinationData) Then

                If dtOriginAndDestinationData.Rows.Count > 0 Then
                    htmlOut.Append("<thead>")
                    htmlOut.Append("<tr><th valign=""middle"" class=""left"" " & IIf(Session.Item("isMobile") = False, "nowrap='nowrap'", "") & "><strong>Origin Airport - Location</strong></th>")
                    htmlOut.Append("<th valign=""middle"" class=""left"" " & IIf(Session.Item("isMobile") = False, "nowrap='nowrap'", "") & "><strong>Destination Airport - Location</strong></th>")
                    htmlOut.Append("<th valign=""middle"" class=""right"" " & IIf(Session.Item("isMobile") = False, "nowrap='nowrap'", "") & "><strong>#Flights</strong></th></tr>")
                    htmlOut.Append("</thead><tbody>")

                    For Each r As DataRow In dtOriginAndDestinationData.Rows

                        'If Not toggleRowColor Then
                        '  htmlOut.Append("<tr class=""alt_row"">")
                        '  toggleRowColor = True
                        'Else
                        htmlOut.Append("<tr>")
                        '  toggleRowColor = False
                        '   End If


                        htmlOut.Append("<td valign=""middle"" align=""left"">")
                        If Not IsDBNull(r.Item("Origin_name")) Then
                            If Not String.IsNullOrEmpty(r.Item("Origin_name").ToString) Then
                                htmlOut.Append(Replace(Replace(r.Item("Origin_name").ToString.Trim, " ", " "), "Airport", "") + " - ")
                            End If
                        End If

                        If Not IsDBNull(r.Item("Origin_city")) Then
                            If Not String.IsNullOrEmpty(r.Item("Origin_city").ToString) Then
                                htmlOut.Append("<em>" + Replace(r.Item("Origin_city").ToString.Trim, " ", " ") + "</em>")
                                sSeparator = ",&nbsp;"
                            End If
                        End If


                        If Not IsDBNull(r.Item("Origin_state")) Then
                            If Not String.IsNullOrEmpty(r.Item("Origin_state").ToString) Then
                                htmlOut.Append(sSeparator + "<em>" + Replace(r.Item("Origin_state").ToString.Trim, " ", " ") + "</em>")
                            End If
                        End If


                        'htmlOut.Append("&nbsp;(")

                        'If Not IsDBNull(r.Item("Origin_icao")) Then
                        '  If Not String.IsNullOrEmpty(r.Item("Origin_icao").ToString) Then
                        '    htmlOut.Append("<em>" + Replace(r.Item("Origin_icao").ToString.Trim, " ", "&nbsp;") + "</em>")
                        '  End If
                        'End If


                        'If Not IsDBNull(r.Item("Origin_iata")) Then
                        '  If Not String.IsNullOrEmpty(r.Item("Origin_iata").ToString) Then
                        '    htmlOut.Append(sSeparator + "&nbsp;<em>" + Replace(r.Item("Origin_iata").ToString.Trim, " ", "&nbsp;") + "</em>")
                        '  End If
                        'End If

                        'htmlOut.Append(")")

                        'htmlOut.Append("</span>")
                        htmlOut.Append("</td>")

                        htmlOut.Append("<td valign=""middle"" align=""left"">")
                        If Not IsDBNull(r.Item("Dest_name")) Then
                            If Not String.IsNullOrEmpty(r.Item("Dest_name").ToString) Then
                                htmlOut.Append(Replace(Replace(r.Item("Dest_name").ToString.Trim, " ", " "), "Airport", "") + " - ")
                            End If
                        End If

                        If Not IsDBNull(r.Item("Dest_city")) Then
                            If Not String.IsNullOrEmpty(r.Item("Dest_city").ToString) Then
                                htmlOut.Append("<em>" + Replace(r.Item("Dest_city").ToString.Trim, " ", " ") + "</em>")
                                sSeparator = ",&nbsp;"
                            End If
                        End If


                        If Not IsDBNull(r.Item("Dest_state")) Then
                            If Not String.IsNullOrEmpty(r.Item("Dest_state").ToString) Then
                                htmlOut.Append(sSeparator + "<em>" + Replace(r.Item("Dest_state").ToString.Trim, " ", " ") + "</em>")
                            End If
                        End If


                        'htmlOut.Append("&nbsp;(")

                        'If Not IsDBNull(r.Item("Dest_icao")) Then
                        '  If Not String.IsNullOrEmpty(r.Item("Dest_icao").ToString) Then
                        '    htmlOut.Append("<em>" + Replace(r.Item("Dest_icao").ToString.Trim, " ", "&nbsp;") + "</em>")
                        '  End If
                        'End If


                        'If Not IsDBNull(r.Item("Dest_iata")) Then
                        '  If Not String.IsNullOrEmpty(r.Item("Dest_iata").ToString) Then
                        '    htmlOut.Append(sSeparator + "&nbsp;<em>" + Replace(r.Item("Dest_iata").ToString.Trim, " ", "&nbsp;") + "</em>")
                        '  End If
                        'End If

                        'htmlOut.Append(")")

                        ' htmlOut.Append("</span>")
                        htmlOut.Append("</td>")


                        htmlOut.Append("<td valign=""middle"" align=""right"">")

                        If Not IsDBNull(r("trip_count")) Then
                            If Not String.IsNullOrEmpty(r.Item("trip_count").ToString) Then '
                                htmlOut.Append("" + FormatNumber(r.Item("trip_count").ToString, 0, True, False, True) + "")
                            End If
                        End If
                        htmlOut.Append("</td>")

                        htmlOut.Append("</tr>")
                    Next

                Else
                    If bGetOrigin Then
                        htmlOut.Append("<tr><td valign=""middle"" align=""center"" colspan=""3"">No top origins data available!</td></tr>")
                    Else
                        htmlOut.Append("<tr><td valign=""middle"" align=""center"" colspan=""3"">No top destinations data available!</td></tr>")
                    End If
                End If
            Else
                If bGetOrigin Then
                    htmlOut.Append("<tr><td valign=""middle"" align=""center"" colspan=""3"">No top origins data available!</td></tr>")
                Else
                    htmlOut.Append("<tr><td valign=""middle"" align=""center"" colspan=""3"">No top destinations data available!</td></tr>")
                End If
            End If

            htmlOut.Append("</tbody></table>")


            htmlOut.Append("</div>")

        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in displayOriginAndDestinationData(ByRef dtOriginAndDestinationData As DataTable, ByVal bGetOrigin As Boolean) As String " + ex.Message

        Finally

        End Try

        'return resulting html string
        Return htmlOut.ToString

        htmlOut = Nothing

    End Function


    ' --  i took this out of code in front, wanted to keep around in case it was useful. 
    '<div id="ExpandableTitleBar" style="width: 98%; text-align: center; margin-left: 10px;
    '                          margin-bottom: 20px; margin-top: 20px;" visible="false" >
    '                          <asp:Panel ID="flight_search_title_panel" runat="server" Width="100%">
    '                              <table width="100%" cellpadding="3" cellspacing="0">
    '                                  <tr>
    '                                      <td align="left" valign="top">
    '                                          <span style="font-weight: bold; font-size: 16px; text-decoration: underline; cursor: pointer;">
    '                                              Flight Activity Search Options</span>
    '                                          <asp:Image ID="image_to_control" runat="server" ImageUrl="/Images/expand.jpg" />
    '                                      </td>
    '                                  </tr>
    '                              </table>
    '                          </asp:Panel>
    '                      </div>

    '                      <cc1:CollapsiblePanelExtender ID="PanelCollapse" runat="server" TargetControlID="flight_search_options"
    '                          ExpandControlID="flight_search_title_panel" Collapsed="true" ExpandedText=""
    '                          CollapsedText="" ImageControlID="image_to_control" ExpandedImage="/Images/root.jpg"
    '                          CollapsedImage="/Images/expand.jpg" SuppressPostBack="False" CollapseControlID="flight_search_title_panel">
    '                      </cc1:CollapsiblePanelExtender>
    '                      <div id='ExpandableContent' style='width: 98%; text-align: center; margin-left: 10px;
    '                          margin-bottom: 20px;'>   

    Private Sub FAAFlightData_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete

    End Sub


End Class