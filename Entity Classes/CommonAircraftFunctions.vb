' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/Entity Classes/CommonAircraftFunctions.vb $
'$$Author: Amanda $
'$$Date: 5/28/20 4:34p $
'$$Modtime: 5/28/20 11:41a $
'$$Revision: 17 $
'$$Workfile: CommonAircraftFunctions.vb $
'
' ********************************************************************************

Public Class CommonAircraftFunctions

    Public Shared Function DisplayAircraftDetailsBlock(ByVal aclsData_Temp As clsData_Manager_SQL, ByVal in_AircraftID As Long,
                                    ByVal in_JournalID As Long,
                                    ByVal isFromJFWAFW As Boolean,
                                    ByVal isDisplay As Boolean,
                                    ByRef out_AircraftRs As DataTable,
                                    ByRef out_HistoricalRs As DataTable,
                                    ByRef MySesState As HttpSessionState,
                                    ByRef aircraft_status_label As Label,
                                    ByRef status_tab_panel As Object,
                                    ByRef stats_tab As Object,
                                    ByRef status_tab_container As Object,
                                    ByRef usage_tab_container As Object,
                                    ByRef history_information As Object,
                                    ByRef history_information_label As Object,
                                    ByRef history_information_panel As Object,
                                    ByRef aircraft_stats As Object,
                                    ByRef notes_tab_container As Object,
                                    ByRef reminder_tab_container As Object,
                                    ByRef company_tab_container As Object,
                                    ByVal onNotesReminderScreen As Boolean, ByRef AportLat As Double, ByRef AportLong As Double,
                                    ByVal RunMap As Boolean, ByVal DOM As Object, ByRef CRMSource As String, Optional ByRef ToggleAnalytics As Boolean = False, Optional ByRef temp_jetnet_ac_id As Long = 0) As String


        Dim htmlOut As StringBuilder = New StringBuilder()
        Dim sQuery As String = ""
        Dim sTmpStr As String = ""
        Dim bHasHistorical As Boolean = False
        Dim ac_header As String = ""
        Dim separator As String = ""
        Dim nColSpan As Integer = 0
        Dim debugQuery2 As String = ""
        Dim left_hand_string As String = ""
        Dim right_hand_string As String = ""
        Dim year_mfr As String = ""
        Dim year_dlv As String = ""
        Dim NotesVar As String = ""
        Dim IataCode As String = ""
        Dim IcaoCode As String = ""
        Dim ValueDescription As String = ""
        Dim AirportTable As New DataTable
        Dim temp_date As String = ""
        Dim temp_date_year As String = ""
        Dim comp_id_list As String = ""
        Dim new_comp_id As Long = 0

        'set this to a new label just in case it's coming from the note page
        If IsNothing(aircraft_status_label) Then
            aircraft_status_label = New Label
        End If

        'let's reset the status label text
        aircraft_status_label.Text = ""

        If CRMSource = "CLIENT" Then
            out_AircraftRs = aclsData_Temp.Get_Client_Aircraft_as_Jetnet_Fields(in_AircraftID)

            'If Not IsNothing(out_AircraftRs) Then
            '  If (out_AircraftRs.Rows.Count) > 0 Then
            '    HttpContext.Current.Session.Item("OtherID") = out_AircraftRs.Rows(0).Item("ac_id")
            '  End If
            'End If
            If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_value_description")) Then
                ValueDescription = out_AircraftRs.Rows(0).Item("ac_value_description")
            End If

            If Not IsDBNull(out_AircraftRs.Rows(0).Item("jetnet_amod_id")) Then
                If out_AircraftRs.Rows(0).Item("jetnet_amod_id") > 0 Then
                    Dim ModelTable As New DataTable
                    ModelTable = aclsData_Temp.GetJetnetModelInfo(out_AircraftRs.Rows(0).Item("jetnet_amod_id"), True, "DisplayAircraftDetails/CommonACFunctions.vb")
                    out_AircraftRs = aclsData_Temp.AddEngineNumber(ModelTable, out_AircraftRs)

                    ModelTable.Dispose()
                    ModelTable = Nothing
                End If
            End If
        Else
            out_AircraftRs = aclsData_Temp.GetJETNET_ACDetails_PresentAndHistorical(in_AircraftID, in_JournalID)
        End If

        'this basically says that if the journal ID isn't blank, but the above query returns zero, go ahead and query the same thing based on a journal ID of zero and
        'display that information.
        If in_JournalID <> 0 Then
            If out_AircraftRs.Rows.Count = 0 Then
                out_AircraftRs = New DataTable
                out_AircraftRs = New DataTable
                out_AircraftRs = aclsData_Temp.GetJETNET_ACDetails_PresentAndHistorical(in_AircraftID, in_JournalID)
            End If
        End If

        ''''''''''''''''''''''''''''''''''''''''''''''
        ''''''''''''''''''''''''''''''''''''''''''''''
        ''''''''''''''''''''''''''''''''''''''''''''''
        If CRMSource = "CLIENT" Then
            temp_jetnet_ac_id = out_AircraftRs.Rows(0).Item("ac_id")
        End If

        sTmpStr = DisplayAircraftHistory_TopBlock(in_AircraftID, in_JournalID, out_HistoricalRs, MySesState, history_information_panel, debugQuery2, aclsData_Temp, CRMSource, out_AircraftRs.Rows(0).Item("ac_id"), False, 0, "JETNET", 0)


        If Not String.IsNullOrEmpty(sTmpStr) Then
            If Not IsNothing(history_information_label) Then
                history_information_label.text = sTmpStr
                history_information.visible = True
                aircraft_stats.cssClass = "dark-theme"
            End If
            bHasHistorical = True
        Else
            bHasHistorical = False
        End If

        '' Aircraft Status Block

        If out_AircraftRs.Rows.Count > 0 Then
            If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_year_mfr")) And Not String.IsNullOrEmpty(out_AircraftRs.Rows(0).Item("ac_year_mfr").ToString) Then
                ac_header += ("" + out_AircraftRs.Rows(0).Item("ac_year_mfr").ToString.Trim + "&nbsp;")
            Else
                ac_header += ("&nbsp;/")
            End If

            If Not IsDBNull(out_AircraftRs.Rows(0).Item("amod_make_name")) And Not String.IsNullOrEmpty(out_AircraftRs.Rows(0).Item("amod_make_name").ToString) Then
                ac_header += ("" + out_AircraftRs.Rows(0).Item("amod_make_name").ToString.Trim + " ")
            Else
                ac_header += ("&nbsp;")
            End If


            If isFromJFWAFW Then
                If Not IsDBNull(out_AircraftRs.Rows(0).Item("amod_model_name")) And Not String.IsNullOrEmpty(out_AircraftRs.Rows(0).Item("amod_model_name").ToString) Then
                    ac_header += ("" + out_AircraftRs.Rows(0).Item("amod_model_name").ToString.Trim + " ")
                Else
                    ac_header += ("&nbsp;")
                End If
            Else
                If Not IsDBNull(out_AircraftRs.Rows(0).Item("amod_model_name")) And Not String.IsNullOrEmpty(out_AircraftRs.Rows(0).Item("amod_model_name").ToString) Then
                    ac_header += out_AircraftRs.Rows(0).Item("amod_model_name").ToString.Trim & " " '+
                Else
                    ac_header += ("&nbsp;")
                End If
            End If

            If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_ser_nbr")) And Not String.IsNullOrEmpty(out_AircraftRs.Rows(0).Item("ac_ser_nbr").ToString) Then
                ac_header += ("S/N " + out_AircraftRs.Rows(0).Item("ac_ser_nbr").ToString + "")
            Else
                ac_header += ("&nbsp;")
            End If

            If Not IsNothing(stats_tab) Then
                stats_tab.HeaderText = ac_header
            End If

            htmlOut.Append("<div class=""twelve columns remove_margin"">")

            'second table row.
            right_hand_string = ""
            If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_alt_ser_no_full")) And Not String.IsNullOrEmpty(out_AircraftRs.Rows(0).Item("ac_alt_ser_no_full").ToString) Then
                right_hand_string += ("<span class='li'><span class='label'>Alt&nbsp;Serial&nbsp;#</span>")
                right_hand_string += ("" + out_AircraftRs.Rows(0).Item("ac_alt_ser_no_full").ToString + "</span>")
            End If

            left_hand_string = ""


            If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_year_mfr")) And Not String.IsNullOrEmpty(out_AircraftRs.Rows(0).Item("ac_year_mfr").ToString) Then
                left_hand_string += ("<span class='li'><span class='label'>Year&nbsp;Mfr&nbsp;")
                year_mfr = out_AircraftRs.Rows(0).Item("ac_year_mfr").ToString
            End If


            If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_year_dlv")) And Not String.IsNullOrEmpty(out_AircraftRs.Rows(0).Item("ac_year_dlv").ToString) Then
                If year_mfr <> "" Then
                    left_hand_string += "/"
                    year_dlv = "/"
                End If
                left_hand_string += ("Year&nbsp;Dlv:&nbsp;</span>")
                year_dlv += out_AircraftRs.Rows(0).Item("ac_year_dlv").ToString
            Else
                left_hand_string += (":</span>")
            End If

            If year_mfr <> "" Or year_dlv <> "" Then
                left_hand_string += year_mfr + year_dlv + "</span>"
            End If

            right_hand_string += ("<span class='li'><span class='label'>Airport:&nbsp;</span>")

            'separator = ""


            If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_aport_iata_code")) And Not String.IsNullOrEmpty(out_AircraftRs.Rows(0).Item("ac_aport_iata_code").ToString) Then
                right_hand_string += (out_AircraftRs.Rows(0).Item("ac_aport_iata_code").ToString.Trim)
                IataCode = out_AircraftRs.Rows(0).Item("ac_aport_iata_code").ToString.Trim
                separator = " - "
            End If

            If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_aport_icao_code")) And Not String.IsNullOrEmpty(out_AircraftRs.Rows(0).Item("ac_aport_icao_code").ToString) Then
                right_hand_string += (separator + out_AircraftRs.Rows(0).Item("ac_aport_icao_code").ToString.Trim)
                IcaoCode = out_AircraftRs.Rows(0).Item("ac_aport_icao_code").ToString.Trim
                separator = " - "
            End If

            If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_aport_faaid_code")) And Not String.IsNullOrEmpty(out_AircraftRs.Rows(0).Item("ac_aport_faaid_code").ToString) Then
                right_hand_string += (separator + out_AircraftRs.Rows(0).Item("ac_aport_faaid_code").ToString.Trim)
                separator = " - "
            End If


            'If runMap = True Then
            'This has been added to figure out the latitude/longitude of the aport.
            If IcaoCode <> "" Or IataCode <> "" Then
                AirportTable = aclsData_Temp.AirportList(0, IcaoCode, IataCode)
                If Not IsNothing(AirportTable) Then
                    If AirportTable.Rows.Count > 0 Then
                        AportLat = IIf(Not IsDBNull(AirportTable.Rows(0).Item("aport_latitude_decimal")), AirportTable.Rows(0).Item("aport_latitude_decimal"), 0)
                        AportLong = IIf(Not IsDBNull(AirportTable.Rows(0).Item("aport_longitude_decimal")), AirportTable.Rows(0).Item("aport_longitude_decimal"), 0)
                    End If
                End If
            End If
            'End If
            If separator <> "" Then
                If onNotesReminderScreen = False Then
                    right_hand_string += "<br />"
                Else
                    right_hand_string += (",&nbsp;")
                End If
            End If

            If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_aport_private")) And Not String.IsNullOrEmpty(out_AircraftRs.Rows(0).Item("ac_aport_private").ToString) Then
                If out_AircraftRs.Rows(0).Item("ac_aport_private").ToString.Trim = "Y" Then
                    right_hand_string += "Private: "
                End If
            End If

            If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_aport_name")) And Not String.IsNullOrEmpty(out_AircraftRs.Rows(0).Item("ac_aport_name").ToString) Then
                right_hand_string += (out_AircraftRs.Rows(0).Item("ac_aport_name").ToString.Trim)
            End If

            If Not String.IsNullOrEmpty(separator) Then
                If onNotesReminderScreen = False Then
                    right_hand_string += ("<br />")
                Else
                    right_hand_string += (",&nbsp;")
                End If
            End If

            If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_aport_city")) And Not String.IsNullOrEmpty(out_AircraftRs.Rows(0).Item("ac_aport_city").ToString) Then
                right_hand_string += (out_AircraftRs.Rows(0).Item("ac_aport_city").ToString.Trim)
                separator = ", "
            End If

            If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_aport_state")) And Not String.IsNullOrEmpty(out_AircraftRs.Rows(0).Item("ac_aport_state").ToString) Then
                right_hand_string += (separator + out_AircraftRs.Rows(0).Item("ac_aport_state").ToString.Trim)
                separator = Constants.cSingleSpace
            End If

            If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_aport_country")) And Not String.IsNullOrEmpty(out_AircraftRs.Rows(0).Item("ac_aport_country").ToString) Then
                If Trim(out_AircraftRs.Rows(0).Item("ac_aport_country").ToString.Trim) = "United States" Then
                    right_hand_string += (separator + "USA")
                Else
                    right_hand_string += (separator + out_AircraftRs.Rows(0).Item("ac_aport_country").ToString.Trim)
                End If


            End If

            right_hand_string += "</span>"

            If bHasHistorical Then

                If CLng(out_HistoricalRs.Rows(0).Item("journ_id").ToString) = 0 Then
                Else
                    'Historical Information Is There

                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    'Previous Registration Number
                    left_hand_string += "<span class='li'><span class='label'>Prev Reg#: </span>"

                    If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_prev_reg_nbr")) And Not String.IsNullOrEmpty(out_AircraftRs.Rows(0).Item("ac_prev_reg_nbr").ToString) Then
                        left_hand_string += ("" + out_AircraftRs.Rows(0).Item("ac_prev_reg_nbr").ToString.Trim + "")
                    End If

                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    'Expiration Date for Registration Number
                    ' left_hand_string += GetRegExpireDate(MySesState, CLng(out_AircraftRs.Rows(0).Item("ac_id").ToString), CLng(out_AircraftRs.Rows(0).Item("ac_journ_id").ToString))
                    left_hand_string += "</span>"
                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    'Date Purchased
                    If CLng(out_AircraftRs.Rows(0).Item("ac_journ_id").ToString) > 0 Then
                        right_hand_string += "<span class='li'><span class='label'>Date&nbsp;Seller&nbsp;Purchased:&nbsp;Aircraft </span>"
                    Else
                        right_hand_string += "<span class='li'><span class='label'>Purchased:&nbsp;</span>"
                    End If

                    If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_date_purchased")) And Not String.IsNullOrEmpty(out_AircraftRs.Rows(0).Item("ac_date_purchased").ToString) Then

                        right_hand_string += trim_out_year_start(FormatDateTime(out_AircraftRs.Rows(0).Item("ac_date_purchased").ToString, DateFormat.ShortDate))

                    End If
                    right_hand_string += "</span>"

                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    'Last Date Change
                    'Commented out per email on 1/7/2013
                    'right_hand_string += ("<span class='li'><span class='label'>Last&nbsp;Change:&nbsp;</span>")

                    'If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_upd_date")) And Not String.IsNullOrEmpty(out_AircraftRs.Rows(0).Item("ac_upd_date").ToString) Then
                    '    right_hand_string += ("" + FormatDateTime(out_AircraftRs.Rows(0).Item("ac_upd_date").ToString, DateFormat.ShortDate))
                    'End If
                    'right_hand_string += ("</span>")

                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    'We're going to append what's to the aircraft main table right now.
                    If onNotesReminderScreen = False Then
                        htmlOut.Append("<div class=""six columns remove_margin"">" & left_hand_string & "</div><div class=""six columns remove_margin"">" & right_hand_string & "</div>")

                    Else
                        htmlOut.Append("<div class=""six columns remove_margin"">" & left_hand_string & "" & right_hand_string & "</div>")

                    End If

                    'Aircraft For sale flag is Yes so the tab class is going to be Green, otherwise it's updated and not green.
                    If Not IsNothing(status_tab_container) And Not IsNothing(notes_tab_container) And Not IsNothing(company_tab_container) Then
                        If out_AircraftRs.Rows(0).Item("ac_forsale_flag").ToString.ToUpper = "Y" Then
                            status_tab_container.cssClass = "green-theme"
                            usage_tab_container.cssClass = "dark-theme"
                            notes_tab_container.cssclass = "blue-theme"
                        Else
                            status_tab_container.cssClass = "dark-theme"
                            usage_tab_container.cssclass = "dark-theme"
                        End If
                    End If

                    'aircraft_status_label.Text += "</div>"
                    aircraft_status_label.Text += "<div class=""twelve columns remove_margin"">"



                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    'let's reset the headertext of the status tab panel
                    If Not IsNothing(status_tab_panel) Then
                        status_tab_panel.HeaderText = "STATUS: "
                    End If
                    'Aircraft Status on Status Header Text
                    ' Don't show status for Aerodex Users
                    If MySesState.Item("localSubscription").crmAerodexFlag And CLng(out_AircraftRs.Rows(0).Item("ac_journ_id").ToString) = 0 Then

                    Else
                        If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_status")) And Not String.IsNullOrEmpty(out_AircraftRs.Rows(0).Item("ac_status").ToString) Then
                            If Not IsNothing(status_tab_panel) Then
                                status_tab_panel.HeaderText = "STATUS: " & UCase(out_AircraftRs.Rows(0).Item("ac_status").ToString.Trim + " ")
                            End If
                        End If
                    End If
                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    'Delivery on Status Header Text
                    'moving the delivery next to the status
                    If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_delivery")) And Not String.IsNullOrEmpty(out_AircraftRs.Rows(0).Item("ac_delivery").ToString) Then
                        If out_AircraftRs.Rows(0).Item("ac_delivery").ToString.ToLower.Contains("date") Then
                            If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_delivery_date")) And Not String.IsNullOrEmpty(out_AircraftRs.Rows(0).Item("ac_delivery_date").ToString) Then
                                If Not IsNothing(status_tab_panel) Then
                                    status_tab_panel.HeaderText += FormatDateTime(out_AircraftRs.Rows(0).Item("ac_delivery_date").ToString, DateFormat.ShortDate)
                                End If
                            End If
                        Else
                            If Not IsNothing(status_tab_panel) Then
                                status_tab_panel.HeaderText += UCase(out_AircraftRs.Rows(0).Item("ac_delivery").ToString)
                            End If
                        End If
                    End If

                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    'Lifecycle Stage
                    aircraft_status_label.Text += "<div class=""six columns remove_margin""><span class='li'>"
                    aircraft_status_label.Text += "" + GetLifeCycleStage(CLng(out_AircraftRs.Rows(0).Item("ac_lifecycle").ToString), aclsData_Temp) + "</span></div>"

                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    'Previously Owned
                    aircraft_status_label.Text += "<div class=""six columns remove_margin""><span class='li'> "

                    If out_AircraftRs.Rows(0).Item("ac_previously_owned_flag").ToString.ToUpper = "Y" Then
                        aircraft_status_label.Text += "Previously Owned</span>"
                    Else
                        aircraft_status_label.Text += "Not Previously Owned</span>"
                    End If

                    aircraft_status_label.Text += "</div>"
                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    'Ownership Type.
                    aircraft_status_label.Text += "<div class=""six columns remove_margin"">"
                    aircraft_status_label.Text += "<span class=""li"">" + GetOwnershipType(out_AircraftRs.Rows(0).Item("ac_ownership").ToString, aclsData_Temp) + "</span></div>"



                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    'For sale section, not shown for Aerodex
                    ' Skip this whole area for Aerodex users
                    If (Not HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag) Or (CLng(out_AircraftRs.Rows(0).Item("ac_journ_id").ToString)) > 0 Then

                        If out_AircraftRs.Rows(0).Item("ac_forsale_flag").ToString.ToUpper = "Y" Then 'Or CLng(out_AircraftRs.Rows(0).Item("ac_journ_id").ToString) > 0 Then

                            If out_AircraftRs.Rows(0).Item("ac_forsale_flag").ToString.ToUpper = "Y" Then 'Or CLng(out_AircraftRs.Rows(0).Item("ac_journ_id").ToString) > 0 Then
                                'Days on Market
                                If CLng(out_AircraftRs.Rows(0).Item("ac_journ_id").ToString) = 0 Then

                                    If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_date_listed")) And Not String.IsNullOrEmpty(out_AircraftRs.Rows(0).Item("ac_date_listed").ToString) Then
                                        aircraft_status_label.Text += "<div class=""six columns remove_margin""><span class='li'><span class='label'>Days on Market</span>&nbsp;:&nbsp;" + DateDiff("d", CDate(out_AircraftRs.Rows(0).Item("ac_date_listed").ToString.Trim), Today()).ToString + "</span></span></div>"
                                    Else
                                        aircraft_status_label.Text += "<div class=""six columns remove_margin""><span class='li'><span class='label'>Days on Market</span>&nbsp;:&nbsp;&lt;Unknown&gt;</span></span></div>"
                                    End If

                                Else

                                    If Not IsDBNull(out_HistoricalRs.Rows(0).Item("journ_date")) And Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_date_listed")) Then
                                        If Not String.IsNullOrEmpty(out_HistoricalRs.Rows(0).Item("journ_date").ToString) And Not String.IsNullOrEmpty(out_AircraftRs.Rows(0).Item("ac_date_listed").ToString) Then
                                            aircraft_status_label.Text += "<div class=""six columns remove_margin""><span class='li'><span class='label'>Days on Market</span>&nbsp;:&nbsp;" + DateDiff("d", CDate(out_AircraftRs.Rows(0).Item("ac_date_listed").ToString.Trim), CDate(out_HistoricalRs.Rows(0).Item("journ_date").ToString.Trim)).ToString + "</span></span></div>"
                                        Else
                                            aircraft_status_label.Text += "<div class=""six columns remove_margin""><span class='li'><span class='label'>Days on Market</span>&nbsp;:&nbsp;&lt;Unknown&gt;</span></span></div>"
                                        End If
                                    Else
                                        aircraft_status_label.Text += "<div class=""six columns remove_margin""><span class='li'><span class='label'>Days on Market</span>&nbsp;:&nbsp;&lt;Unknown&gt;</span></span></div>"
                                    End If

                                End If ' CLng(out_AircraftRs.Rows(0).Item("ac_journ_id").ToString) = 0

                            End If ' out_AircraftRs.Rows(0).Item("ac_forsale_flag").ToString.ToUpper = "Y" Or CLng(out_AircraftRs.Rows(0).Item("ac_journ_id").ToString) > 0 Then 

                            'aircraft_status_label.Text += "</tr><tr>"
                            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                            'AC asking price
                            If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_asking_wordage")) And Not String.IsNullOrEmpty(out_AircraftRs.Rows(0).Item("ac_asking_wordage").ToString) Then

                                If LCase(out_AircraftRs.Rows(0).Item("ac_asking_wordage")).ToString.Contains("price") Then
                                    'This means the asking contains the word price so a price needs to be displayed
                                    If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_foreign_currency_price")) And Not String.IsNullOrEmpty(out_AircraftRs.Rows(0).Item("ac_foreign_currency_price").ToString) Then
                                        aircraft_status_label.Text += "<div class=""six columns remove_margin""><span class='li'><span class='label'>Asking&nbsp;Amt&nbsp;</span> "
                                        ' If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_foreign_currency_price")) And Not String.IsNullOrEmpty(out_AircraftRs.Rows(0).Item("ac_foreign_currency_price").ToString) Then
                                        If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_foreign_currency_price")) And Not String.IsNullOrEmpty(out_AircraftRs.Rows(0).Item("ac_foreign_currency_price").ToString) Then
                                            aircraft_status_label.Text += "" + FormatNumber(out_AircraftRs.Rows(0).Item("ac_foreign_currency_price").ToString.Trim, 0, True, False, True) + ""
                                        Else
                                            aircraft_status_label.Text += "&nbsp;"
                                        End If
                                        aircraft_status_label.Text += out_AircraftRs.Rows(0).Item("ac_foreign_currency_name").ToString & "</span></div>"
                                        'End If
                                    Else
                                        aircraft_status_label.Text += "<div class=""six columns remove_margin""><span class='li'><span class='label'>Asking&nbsp;Amt&nbsp;</span> "
                                        If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_asking_price")) And Not String.IsNullOrEmpty(out_AircraftRs.Rows(0).Item("ac_asking_price").ToString) Then
                                            If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_asking_price")) And Not String.IsNullOrEmpty(out_AircraftRs.Rows(0).Item("ac_asking_price").ToString) Then
                                                aircraft_status_label.Text += "$" + FormatNumber(out_AircraftRs.Rows(0).Item("ac_asking_price").ToString.Trim, 0, True, False, True) + " (USD)</span></div>"
                                            Else
                                                aircraft_status_label.Text += "&nbsp;</span></div>"
                                            End If

                                        End If

                                    End If
                                Else
                                    'no price in the ac asking, so just display the wordage.
                                    aircraft_status_label.Text += "<div class=""six columns remove_margin""><span class='li'><span class='label'></span> "

                                    If Not String.IsNullOrEmpty(out_AircraftRs.Rows(0).Item("ac_asking_wordage").ToString) Then
                                        aircraft_status_label.Text += "" + out_AircraftRs.Rows(0).Item("ac_asking_wordage").ToString.Trim + "</span></div>"
                                    Else
                                        aircraft_status_label.Text += "&nbsp;</span></div>"
                                    End If

                                End If

                            Else
                                'this asking is just displayed
                                aircraft_status_label.Text += "<div class=""six columns remove_margin""><span class='li'><span class='label'>Asking</span> "
                                aircraft_status_label.Text += "&nbsp;</span></div>"

                            End If ' Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_asking")) Then
                            '''''''''''''''''''''''''''''''''''''''
                            'ac listed date.
                            aircraft_status_label.Text += "<div class=""twelve columns remove_margin""><span class='li'><span class='label'>Listed</span> "

                            If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_date_listed")) And Not String.IsNullOrEmpty(out_AircraftRs.Rows(0).Item("ac_date_listed").ToString) Then
                                aircraft_status_label.Text += "" + FormatDateTime(out_AircraftRs.Rows(0).Item("ac_date_listed").ToString.Trim, DateFormat.ShortDate) + "</span></div>"
                            Else
                                aircraft_status_label.Text += "&nbsp;</div>"
                            End If
                            '''''''''''''''''''''''''''''''''''''''


                        End If ' out_AircraftRs.Rows(0).Item("ac_forsale_flag").ToString.ToUpper = "Y" Or CLng(out_AircraftRs.Rows(0).Item("ac_journ_id").ToString) > 0 Then 

                    End If ' (Session.Item("localSubscription").crmAerodexFlag) Or (CLng(out_AircraftRs.Rows(0).Item("ac_journ_id").ToString) > 0)
                    '''''''''''''''''''''''''''''''''''''''
                    ' Don't show exclusive info to Aerodex users
                    If Not HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag Or CLng(out_AircraftRs.Rows(0).Item("ac_journ_id").ToString) > 0 Then

                        If out_AircraftRs.Rows(0).Item("ac_exclusive_flag").ToString.ToUpper = "Y" Then

                            If CLng(out_AircraftRs.Rows(0).Item("ac_journ_id").ToString) = 0 Then
                                aircraft_status_label.Text += "<div class=""remove_margin "
                                'AC lease flag, if it's Y then it's normal display
                                'But if it's not Y, then stretch the exclusive
                                If out_AircraftRs.Rows(0).Item("ac_lease_flag").ToString.ToUpper = "Y" Then
                                    aircraft_status_label.Text += "six columns "
                                Else
                                    aircraft_status_label.Text += "twelve columns "
                                End If

                                aircraft_status_label.Text += " purple_background"">"

                            Else
                                aircraft_status_label.Text += "<div class=""remove_margin twelve columns"">"
                            End If
                            'exclusive date
                            Dim d_exclusiveDate As String = GetExclusiveDate(CLng(out_AircraftRs.Rows(0).Item("ac_id").ToString), CLng(out_AircraftRs.Rows(0).Item("ac_journ_id").ToString), aclsData_Temp)

                            If Not String.IsNullOrEmpty(d_exclusiveDate) Then
                                aircraft_status_label.Text += "<span class='li'><span class='label'>Exclusive With</span><br /><em>" + GetExclusive(in_AircraftID, CLng(out_AircraftRs.Rows(0).Item("ac_journ_id").ToString), aclsData_Temp, CRMSource, ToggleAnalytics, comp_id_list) + "</em> as of " + d_exclusiveDate + "</span></div>"
                            Else
                                aircraft_status_label.Text += ("<span class='li'><span class='label'>Exclusive With</span><br /><em>" + GetExclusive(in_AircraftID, CLng(out_AircraftRs.Rows(0).Item("ac_journ_id").ToString), aclsData_Temp, CRMSource, ToggleAnalytics, comp_id_list) + "</em></span></div>")
                            End If



                            'exclusive expiration flag
                            If out_AircraftRs.Rows(0).Item("ac_exclusive_expiration_flag").ToString.ToUpper = "Y" Then
                                aircraft_status_label.Text += ("<div class=""remove_margin six columns""><span class='label'>Expiration&nbsp;Date</span>")

                                If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_exclusive_date")) And Not String.IsNullOrEmpty(out_AircraftRs.Rows(0).Item("ac_exclusive_date").ToString) Then
                                    aircraft_status_label.Text += ("" + FormatDateTime(out_AircraftRs.Rows(0).Item("ac_exclusive_date").ToString, DateFormat.ShortDate) + "</div>")
                                Else
                                    aircraft_status_label.Text += ("&nbsp;</div>")
                                End If

                            End If

                        End If ' out_AircraftRs.Rows(0).Item("ac_exclusive_flag").ToString.ToUpper = "Y" 

                    End If ' (Not MySesState.Item("localSubscription").evoAerodexFlag) Or CLng(out_AircraftRs.Rows(0).Item("ac_journ_id").ToString) > 0 Then


                    ' ADDED MSW - 8/4/16 - ADDING IN DISPLAYABLE SALE PRICSE
                    If HttpContext.Current.Session.Item("localSubscription").crmSalesPriceIndex_Flag = True And CLng(out_AircraftRs.Rows(0).Item("ac_journ_id").ToString) > 0 Then

                        If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_sale_price_display_flag")) And Trim(out_AircraftRs.Rows(0).Item("ac_sale_price_display_flag")) = "Y" Then

                            If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_sale_price")) Then
                                If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_sale_price")) Then
                                    If CDbl(out_AircraftRs.Rows(0).Item("ac_sale_price").ToString) > 0 Then
                                        aircraft_status_label.Text += "<div class=""six columns remove_margin""><span class='li'><span class='label'>Sale&nbsp;Price:&nbsp;</span> "
                                        aircraft_status_label.Text += (DisplayFunctions.TextToImage("$" & FormatNumber((out_AircraftRs.Rows(0).Item("ac_sale_price").ToString / 1000), 0) & "k", 12, "", "42", "Displayable Sale Price", "bottom", True))
                                        aircraft_status_label.Text += "&nbsp;</span></div>"
                                    End If
                                End If
                            End If

                            If out_AircraftRs.Rows(0).Item("ac_forsale_flag").ToString.ToUpper = "N" Then
                                If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_asking_price")) And Not String.IsNullOrEmpty(out_AircraftRs.Rows(0).Item("ac_asking_price").ToString) Then
                                    aircraft_status_label.Text += "<div class=""six columns remove_margin""><span class='li'><span class='label'>Asking&nbsp;Price:&nbsp;</span> "
                                    aircraft_status_label.Text += (DisplayFunctions.TextToImage("$" & FormatNumber((out_AircraftRs.Rows(0).Item("ac_asking_price").ToString / 1000), 0) & "k", 12, "", "42", "Displayable Sale Price", "bottom", True))
                                    aircraft_status_label.Text += "&nbsp;</span></div>"
                                End If
                            ElseIf LCase(out_AircraftRs.Rows(0).Item("ac_asking_wordage")).ToString.Contains("make offer") Then
                                If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_asking_price")) And Not String.IsNullOrEmpty(out_AircraftRs.Rows(0).Item("ac_asking_price").ToString) Then
                                    aircraft_status_label.Text += "<div class=""six columns remove_margin""><span class='li'><span class='label'>Asking&nbsp;Price:&nbsp;</span> "
                                    aircraft_status_label.Text += (DisplayFunctions.TextToImage("$" & FormatNumber((out_AircraftRs.Rows(0).Item("ac_asking_price").ToString / 1000), 0) & "k", 12, "", "42", "Displayable Sale Price", "bottom", True))
                                    aircraft_status_label.Text += "&nbsp;</span></div>"
                                End If
                            End If


                        End If

                    End If



                    If out_AircraftRs.Rows(0).Item("ac_lease_flag").ToString.ToUpper = "Y" Then
                        If CLng(out_AircraftRs.Rows(0).Item("ac_journ_id").ToString) = 0 Then
                            aircraft_status_label.Text += ("div class=""remove_margin six columns orange_background"">")
                        Else
                            aircraft_status_label.Text += ("<div class=""remove_margin six columns"">")
                        End If
                        aircraft_status_label.Text += ("<span class='li'>Currently&nbsp;On&nbsp;Lease</span></div>")
                    End If

                    ' aircraft_status_label.Text += ("</tr><tr>")
                    aircraft_status_label.Text += ("<div class=""remove_margin twelve columns""><span class='li'><span class='label'>Last&nbsp;Change: </span>")

                    If out_AircraftRs.Rows(0).Item("ac_exclusive_flag").ToString.ToUpper = "Y" And Not HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag Then
                        If out_AircraftRs.Rows(0).Item("ac_exclusive_expiration_flag").ToString.ToUpper = "Y" Then
                            If out_AircraftRs.Rows(0).Item("ac_lease_flag").ToString.ToUpper = "Y" Then
                                nColSpan = 4
                            Else
                                nColSpan = 5
                            End If
                        Else
                            If out_AircraftRs.Rows(0).Item("ac_lease_flag").ToString.ToUpper = "Y" Then
                                nColSpan = 5
                            Else
                                nColSpan = 6
                            End If
                        End If
                    Else
                        If out_AircraftRs.Rows(0).Item("ac_lease_flag").ToString.ToUpper = "Y" Then
                            nColSpan = 6
                        Else
                            nColSpan = 7
                        End If
                    End If

                    If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_upd_date")) And Not String.IsNullOrEmpty(out_AircraftRs.Rows(0).Item("ac_upd_date").ToString) Then
                        aircraft_status_label.Text += ("" + FormatDateTime(out_AircraftRs.Rows(0).Item("ac_upd_date").ToString, DateFormat.ShortDate) + "</span></div>")
                    Else
                        aircraft_status_label.Text += ("</span></div>")
                    End If

                    'seventh table row
                    'show confidental notes if there are any
                    If Not HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag Then
                        If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_confidential_notes")) Then
                            If Not String.IsNullOrEmpty(out_AircraftRs.Rows(0).Item("ac_confidential_notes")) Then
                                NotesVar = out_AircraftRs.Rows(0).Item("ac_confidential_notes").ToString.Trim
                                If NotesVar <> "" Then
                                    aircraft_status_label.Text += ("<div class=""remove_margin twelve columns remove_margin""><span class='li'>Notes : " + out_AircraftRs.Rows(0).Item("ac_confidential_notes").ToString.Trim + "</span></div>")
                                End If
                            End If
                        End If
                    End If

                    aircraft_status_label.Text += "</div>"
                    htmlOut.Append("</div>")
                End If ' CLng(out_HistoricalRs.Rows(0).Item("journ_id").ToString) = 0  

            Else
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                'Registration Information
                left_hand_string += ("<span class='li'><span class='label'>Reg #: </span>")
                If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_reg_nbr")) And Not String.IsNullOrEmpty(out_AircraftRs.Rows(0).Item("ac_reg_nbr").ToString) Then
                    left_hand_string += ("" + out_AircraftRs.Rows(0).Item("ac_reg_nbr").ToString.Trim + "")
                End If

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                'Registration Expiration
                left_hand_string += GetRegExpireDate(out_AircraftRs) + ""
                left_hand_string += "</span>"

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                'Previous Registration Information
                If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_prev_reg_nbr")) And Not String.IsNullOrEmpty(out_AircraftRs.Rows(0).Item("ac_prev_reg_nbr").ToString) Then
                    left_hand_string += ("<span class='li'><span class='label'>Prev Reg #: </span>")
                    left_hand_string += ("" + out_AircraftRs.Rows(0).Item("ac_prev_reg_nbr").ToString.Trim + "")
                    left_hand_string += "</span>"
                End If

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                'Purchase Date for Aircraft
                If CLng(out_AircraftRs.Rows(0).Item("ac_journ_id").ToString) > 0 Then
                    right_hand_string += ("<span class='li'><span class='label'>Date&nbsp;Seller&nbsp;Purchased&nbsp;Aircraft:&nbsp;</span>")
                Else
                    right_hand_string += ("<span class='li'><span class='label'>Purchased:&nbsp;</span>")
                End If

                If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_date_purchased")) And Not String.IsNullOrEmpty(out_AircraftRs.Rows(0).Item("ac_date_purchased").ToString) Then

                    right_hand_string += trim_out_year_start(FormatDateTime(out_AircraftRs.Rows(0).Item("ac_date_purchased").ToString, DateFormat.ShortDate))

                End If

                right_hand_string += ("</span>")

                'This is where we append the first table for the aircraft information main block.

                If onNotesReminderScreen = False Then
                    htmlOut.Append("<div class=""six columns remove_margin"">" & left_hand_string & "</div><div class=""six columns remove_margin"">" & right_hand_string & "</div>")
                Else
                    htmlOut.Append("<div class=""twelve columns remove_margin"">" & left_hand_string & "" & right_hand_string & "</div>")
                End If

                'and we clear the variables to get ready for tab container #2 the status tab container.
                left_hand_string = ""
                right_hand_string = ""

                'aircraft_status_label.Text += "</div>"
                aircraft_status_label.Text += "<div class=""twelve columns remove_margin"">"


                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                'let's reset the headertext of the status tab panel
                If Not IsNothing(status_tab_panel) Then
                    status_tab_panel.HeaderText = "STATUS: "
                End If
                'Aircraft Status
                If MySesState.Item("localSubscription").crmAerodexFlag = False Then
                    ' Don't show status for Aerodex Users
                    If MySesState.Item("localSubscription").crmAerodexFlag And CLng(out_AircraftRs.Rows(0).Item("ac_journ_id").ToString) = 0 Then
                    Else
                        If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_status")) And Not String.IsNullOrEmpty(out_AircraftRs.Rows(0).Item("ac_status").ToString) Then
                            If Not IsNothing(status_tab_panel) Then
                                status_tab_panel.HeaderText += UCase(out_AircraftRs.Rows(0).Item("ac_status").ToString.Trim + " ")
                            End If
                        End If
                    End If
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    'Aircraft Delivery / Delivery Date
                    If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_delivery")) And Not String.IsNullOrEmpty(out_AircraftRs.Rows(0).Item("ac_delivery").ToString) Then
                        If out_AircraftRs.Rows(0).Item("ac_delivery").ToString.ToLower.Contains("date") Then
                            If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_delivery_date")) And Not String.IsNullOrEmpty(out_AircraftRs.Rows(0).Item("ac_delivery_date").ToString) Then
                                'aircraft_status_label.Text += ("" + FormatDateTime(out_AircraftRs.Rows(0).Item("ac_delivery_date").ToString, DateFormat.ShortDate) + "</td>")
                                If Not IsNothing(status_tab_panel) Then
                                    status_tab_panel.HeaderText += UCase(("" + FormatDateTime(out_AircraftRs.Rows(0).Item("ac_delivery_date").ToString, DateFormat.ShortDate) + ""))
                                End If
                            End If
                        Else
                            ' aircraft_status_label.Text += ("" + out_AircraftRs.Rows(0).Item("ac_delivery").ToString.Trim + "</td>")
                            If Not IsNothing(status_tab_panel) Then
                                status_tab_panel.HeaderText += UCase(("" + out_AircraftRs.Rows(0).Item("ac_delivery").ToString.Trim + ""))
                            End If
                        End If
                    End If
                End If
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ' Skip this whole area for Aerodex users
                If Not MySesState.Item("localSubscription").crmAerodexFlag Or CLng(out_AircraftRs.Rows(0).Item("ac_journ_id").ToString) > 0 Then

                    If out_AircraftRs.Rows(0).Item("ac_forsale_flag").ToString.ToUpper = "Y" Or CLng(out_AircraftRs.Rows(0).Item("ac_journ_id").ToString) > 0 Then
                        'This is going to turn the tab green, but only if the aircraft is for sale.
                        If out_AircraftRs.Rows(0).Item("ac_forsale_flag").ToString.ToUpper = "Y" Then
                            left_hand_string += ("<span class='green_text'>")
                            If Not IsNothing(status_tab_container) Then
                                status_tab_container.cssClass = "green-theme"
                                '    If HttpContext.Current.Session.Item("localUser").crmEnableNotes = True Then
                                '        If Not IsNothing(reminder_tab_container) Then
                                '            reminder_tab_container.cssclass = "green-theme"
                                '        End If
                                '    ElseIf HttpContext.Current.Session.Item("localUser").crmEnableNotes = False Then
                                '        If Not IsNothing(company_tab_container) Then
                                '            usage_tab_container.cssclass = "green-theme"
                                '        End If
                                '    End If
                            End If
                        End If
                        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        'This is the Aircraft Asking Section.
                        If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_asking_wordage")) And Not String.IsNullOrEmpty(out_AircraftRs.Rows(0).Item("ac_asking_wordage").ToString) Then
                            'This only shows up if the word price is in asking (meaning there's an amount involved).
                            If out_AircraftRs.Rows(0).Item("ac_asking_wordage").ToString.ToLower.Contains("price") Then
                                'Asking Amount goes here
                                If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_foreign_currency_price")) And Not String.IsNullOrEmpty(out_AircraftRs.Rows(0).Item("ac_foreign_currency_price").ToString) Then
                                    left_hand_string += ("<span class='li'><span class='label'>Asking&nbsp;Amt:&nbsp;</span>")

                                    If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_foreign_currency_price")) And Not String.IsNullOrEmpty(out_AircraftRs.Rows(0).Item("ac_foreign_currency_price").ToString) Then
                                        left_hand_string += ("" + FormatNumber(out_AircraftRs.Rows(0).Item("ac_foreign_currency_price").ToString.Trim, 0, True, False, True))
                                    End If
                                    left_hand_string += (" (<em>" & out_AircraftRs.Rows(0).Item("ac_foreign_currency_name").ToString & "</em>)")

                                    left_hand_string += ("&nbsp;</span></li>")
                                Else
                                    left_hand_string += ("<span class='li'><span class='label'>Asking&nbsp;Amt:&nbsp;</span>")
                                    'Asking Price
                                    If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_asking_price")) And Not String.IsNullOrEmpty(out_AircraftRs.Rows(0).Item("ac_asking_price").ToString) Then
                                        If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_asking_price")) And Not String.IsNullOrEmpty(out_AircraftRs.Rows(0).Item("ac_asking_price").ToString) Then
                                            left_hand_string += ("$" + FormatNumber(out_AircraftRs.Rows(0).Item("ac_asking_price").ToString.Trim, 0, True, False, True) & " (USD)")
                                        End If
                                    End If
                                    left_hand_string += ("&nbsp;</span>")
                                End If

                            Else
                                'This is what happens if the word price is not involved in the asking wordage.
                                left_hand_string += ("<span class='li'><span class='label'>Asking:</span> ")
                                If Not String.IsNullOrEmpty(out_AircraftRs.Rows(0).Item("ac_asking_wordage").ToString) Then
                                    left_hand_string += ("" + out_AircraftRs.Rows(0).Item("ac_asking_wordage").ToString.Trim)
                                End If
                                left_hand_string += ("&nbsp;</span>")

                            End If
                        End If ' Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_asking")) Then
                        If CRMSource = "CLIENT" Then
                            '''''''''''''''''''''''''''''''''''''''
                            'ac take price.
                            If Not IsDBNull(out_AircraftRs.Rows(0).Item("take_price")) And Not String.IsNullOrEmpty(out_AircraftRs.Rows(0).Item("take_price").ToString) Then
                                If IsNumeric(out_AircraftRs.Rows(0).Item("take_price")) Then
                                    If out_AircraftRs.Rows(0).Item("take_price") > 0 Then
                                        left_hand_string += "<span class='li'><span class='label'>Take Price</span> " + ("$" + FormatNumber(out_AircraftRs.Rows(0).Item("take_price").ToString.Trim, 0, True, False, True)) + "</span>"
                                    End If
                                End If
                            End If
                            '''''''''''''''''''''''''''''''''''''''
                        End If
                        'This is the Date Listed Section.
                        left_hand_string += ("<span class='li'><span class=""label"">Listed:</span> ")

                        If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_date_listed")) And Not String.IsNullOrEmpty(out_AircraftRs.Rows(0).Item("ac_date_listed").ToString) Then
                            temp_date = FormatDateTime(out_AircraftRs.Rows(0).Item("ac_date_listed").ToString.Trim, DateFormat.ShortDate)

                            temp_date_year = trim_out_year_start(temp_date)

                            left_hand_string += ("" + temp_date_year)

                        End If

                        left_hand_string += ("&nbsp;")

                        'This is the Days on Market Section
                        If out_AircraftRs.Rows(0).Item("ac_forsale_flag").ToString.ToUpper = "Y" Or CLng(out_AircraftRs.Rows(0).Item("ac_journ_id").ToString) > 0 Then

                            If CLng(out_AircraftRs.Rows(0).Item("ac_journ_id").ToString) = 0 Then
                                If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_date_listed")) And Not String.IsNullOrEmpty(out_AircraftRs.Rows(0).Item("ac_date_listed").ToString) Then
                                    left_hand_string += ("(<span class=""label text_underline help_cursor"" alt=""Days on Market"" title=""Days on Market"">DOM</span>:&nbsp;" + DateDiff("d", CDate(out_AircraftRs.Rows(0).Item("ac_date_listed").ToString.Trim), Today()).ToString + ")")
                                    If Not IsNothing(DOM) Then
                                        DOM.text = DateDiff("d", CDate(out_AircraftRs.Rows(0).Item("ac_date_listed").ToString.Trim), Today()).ToString
                                    End If
                                Else
                                    left_hand_string += ("(<span class=""label text_underline help_cursor"" alt=""Days on Market"" title=""Days on Market"">DOM</span>:&nbsp;&lt;Unknown&gt;")
                                End If

                            Else
                                'This only happens if the days on market is unknown.
                                If Not IsDBNull(out_HistoricalRs.Rows(0).Item("journ_date")) And Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_date_listed")) Then
                                    If Not String.IsNullOrEmpty(out_HistoricalRs.Rows(0).Item("journ_date").ToString) And Not String.IsNullOrEmpty(out_AircraftRs.Rows(0).Item("ac_date_listed").ToString) Then
                                        left_hand_string += ("(<span class=""label text_underline help_cursor"" alt=""Days on Market"" title=""Days on Market"">DOM</span>:&nbsp;" + DateDiff("d", CDate(out_AircraftRs.Rows(0).Item("ac_date_listed").ToString.Trim), CDate(out_HistoricalRs.Rows(0).Item("journ_date").ToString.Trim)).ToString + "")
                                    Else
                                        left_hand_string += ("(<span class=""label text_underline help_cursor"" alt=""Days on Market"" title=""Days on Market"">DOM</span>:&nbsp;&lt;Unknown&gt;")
                                    End If
                                Else
                                    left_hand_string += ("(<span class=""label text_underline help_cursor"" alt=""Days on Market"" title=""Days on Market"">DOM</span>:&nbsp;&nbsp;&lt;Unknown&gt;")
                                End If

                            End If ' CLng(out_AircraftRs.Rows(0).Item("ac_journ_id").ToString) = 0

                        End If ' out_AircraftRs.Rows(0).Item("ac_forsale_flag").ToString.ToUpper = "Y" Or CLng(out_AircraftRs.Rows(0).Item("ac_journ_id").ToString) > 0


                        left_hand_string += ("&nbsp;</span>")



                    Else 'This makes the tab container css changes if the aircraft is not for sale. 
                        If Not IsNothing(status_tab_container) Then
                            status_tab_container.cssClass = "dark-theme"
                        End If

                        If Not IsNothing(notes_tab_container) Then
                            notes_tab_container.cssclass = "blue-theme"
                        End If
                    End If ' out_AircraftRs.Rows(0).Item("ac_forsale_flag").ToString.ToUpper = "Y" Or CLng(out_AircraftRs.Rows(0).Item("ac_journ_id").ToString) > 0 Then 
                Else 'This also makes the tab container changes if the user is aerodex.
                    If Not IsNothing(status_tab_container) Then
                        status_tab_container.cssClass = "dark-theme"
                    End If
                    If Not IsNothing(notes_tab_container) Then
                        notes_tab_container.cssclass = "blue-theme"
                    End If
                End If ' if (not session("localSubscription").evoAerodexFlag) or CLng(in_AircraftRs("ac_journ_id").ToString) > 0 then


                'AC exclusive information. This displays differently depending on if we're leased or not. If we aren't, it displays the exclusive
                'spanning over two columns.
                If Not HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag Or CLng(out_AircraftRs.Rows(0).Item("ac_journ_id").ToString) > 0 Then

                    If out_AircraftRs.Rows(0).Item("ac_exclusive_flag").ToString.ToUpper = "Y" Then
                        If right_hand_string = "" Then
                            ' left_hand_string = "<td align='left' colspan='2' valign='top'>"
                        Else
                            'left_hand_string = "<td align='left' valign='top'>"
                        End If
                        If CLng(out_AircraftRs.Rows(0).Item("ac_journ_id").ToString) = 0 Then
                            left_hand_string += ("<span class='li'><span class='purple_background'>")
                        Else
                            left_hand_string += ("<span class='li'>")
                        End If
                        'This displays the exclusive date.
                        Dim d_exclusiveDate As String = GetExclusiveDate(CLng(out_AircraftRs.Rows(0).Item("ac_id").ToString), CLng(out_AircraftRs.Rows(0).Item("ac_journ_id").ToString), aclsData_Temp)


                        d_exclusiveDate = trim_out_year_start(d_exclusiveDate)

                        If Not String.IsNullOrEmpty(d_exclusiveDate) Then
                            left_hand_string += ("<span class='label'>Exclusive With</span> <em>" + GetExclusive(in_AircraftID, CLng(out_AircraftRs.Rows(0).Item("ac_journ_id").ToString), aclsData_Temp, CRMSource, ToggleAnalytics) + " as of " + d_exclusiveDate + "</em></span> ")
                        Else
                            left_hand_string += ("<span class='label'>Exclusive With</span> <em>" + GetExclusive(in_AircraftID, CLng(out_AircraftRs.Rows(0).Item("ac_journ_id").ToString), aclsData_Temp, CRMSource, ToggleAnalytics) + "</em></span> ")
                        End If
                        'Exclusive expiration flag.
                        If out_AircraftRs.Rows(0).Item("ac_exclusive_expiration_flag").ToString.ToUpper = "Y" Then


                            If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_exclusive_date")) And Not String.IsNullOrEmpty(out_AircraftRs.Rows(0).Item("ac_exclusive_date").ToString) Then
                                left_hand_string += ("&nbsp;&nbsp;&nbsp;<em class='tiny'>(Expiration&nbsp;Date: ")
                                left_hand_string += ("" + FormatDateTime(out_AircraftRs.Rows(0).Item("ac_exclusive_date").ToString, DateFormat.ShortDate) + "")
                                left_hand_string += ")</em>"
                            End If

                        End If
                        left_hand_string += ("</span></span>")
                    End If ' out_AircraftRs.Rows(0).Item("ac_exclusive_flag").ToString.ToUpper = "Y" 

                End If ' (Not MySesState.Item("localSubscription").evoAerodexFlag) Or CLng(out_AircraftRs.Rows(0).Item("ac_journ_id").ToString) > 0 Then




                '    'This is saying whether or not the aircraft is previously owned.
                If out_AircraftRs.Rows(0).Item("ac_forsale_flag").ToString.ToUpper = "Y" Or CLng(out_AircraftRs.Rows(0).Item("ac_journ_id").ToString) > 0 Then
                    If out_AircraftRs.Rows(0).Item("ac_previously_owned_flag").ToString.ToUpper = "Y" Then
                        right_hand_string += ("<span class='li'>Previously&nbsp;Owned</span>")
                    Else
                        right_hand_string += ("<span class='li'>Not Previously&nbsp;Owned</span>")
                    End If
                    'This is the lifecycle and ownership type.
                    'I realize that it looks like the code below, but please note the fact that this one displays on the right hand side.
                    'The one below when the ac is not for sale displays on the left and the right.
                    right_hand_string += ("<span class='li'>" + GetLifeCycleStage(CLng(out_AircraftRs.Rows(0).Item("ac_lifecycle").ToString), aclsData_Temp) + "</span>")
                    right_hand_string += ("<span class='li'>" + GetOwnershipType(out_AircraftRs.Rows(0).Item("ac_ownership").ToString, aclsData_Temp) + "")

                    'Now we're going to see if the lease flag is Y, if it is, we're in good shape and we need two columns.
                    If MySesState.Item("localSubscription").crmAerodexFlag = False Then
                        If out_AircraftRs.Rows(0).Item("ac_lease_flag").ToString.ToUpper = "Y" Then
                            right_hand_string += ", "
                            If CLng(out_AircraftRs.Rows(0).Item("ac_journ_id").ToString) = 0 Then
                                right_hand_string += ("<span class='orange_background'>")
                            End If
                            right_hand_string += ("Leased</span>")
                            right_hand_string += ""
                        End If
                    End If
                    right_hand_string += ("</span>")
                Else
                    'The previously owned flag.
                    If out_AircraftRs.Rows(0).Item("ac_previously_owned_flag").ToString.ToUpper = "Y" Then
                        left_hand_string += ("<span class='li'>Previously&nbsp;Owned</span>")
                    Else
                        left_hand_string += ("<span class='li'>Not Previously&nbsp;Owned</span>")
                    End If
                    '        'The left/right lifecycle and ownership display. 
                    left_hand_string += ("<span class='li'>" + GetLifeCycleStage(CLng(out_AircraftRs.Rows(0).Item("ac_lifecycle").ToString), aclsData_Temp) + "</span>")
                    right_hand_string += ("<span class='li'>" + GetOwnershipType(out_AircraftRs.Rows(0).Item("ac_ownership").ToString, aclsData_Temp) + "")


                    'Now we're going to see if the lease flag is Y, if it is, we're in good shape and we need two columns.
                    If MySesState.Item("localSubscription").crmAerodexFlag = False Then
                        If out_AircraftRs.Rows(0).Item("ac_lease_flag").ToString.ToUpper = "Y" Then
                            right_hand_string += ", "
                            If CLng(out_AircraftRs.Rows(0).Item("ac_journ_id").ToString) = 0 Then
                                right_hand_string += ("<span class='orange_background'>")
                            End If
                            right_hand_string += ("Leased</span>")
                            right_hand_string += ""
                        End If
                    End If
                    right_hand_string += ("</span>")
                End If

                If MySesState.Item("localSubscription").crmAerodexFlag = True Then
                    'we need to move the right hand to the left hand side.
                    left_hand_string = right_hand_string
                    right_hand_string = ""
                End If



                'Let's update the control with what we currently have so we can do the exclusive special if the lease doesn't exist
                aircraft_status_label.Text += ("<div class=""six columns remove_margin"">" & left_hand_string & "</div><div class=""six columns remove_margin"">" & right_hand_string & "</div>")
                'We are first going to clear both lists. 
                right_hand_string = ""
                left_hand_string = ""



                'Adding the value description.
                'This is filled up top at the beginining of this function when you query for the information. It is only filled in for
                'Client Aircraft, so if you have a jetnet aircraft and are viewing it, this part should be skipped right over.
                If ValueDescription <> "" Then
                    left_hand_string += "<span class='li help_cursor' title='Value/Price Description'>" & ValueDescription & "</span>"
                End If


                'Finally we're going to add the exclusive and or lease information depending on
                'what was available.
                aircraft_status_label.Text += "<div class=""six columns remove_margin div_clear"">" & left_hand_string & "</div><div class=""six columns remove_margin"">" & right_hand_string & "</div>"

                If Not HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag Then
                    If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_confidential_notes")) Then
                        If Not String.IsNullOrEmpty(out_AircraftRs.Rows(0).Item("ac_confidential_notes")) Then
                            NotesVar = out_AircraftRs.Rows(0).Item("ac_confidential_notes").ToString.Trim
                            If NotesVar <> "" Then
                                aircraft_status_label.Text += ("<div class=""twelve columns remove_margin"">Notes : " + out_AircraftRs.Rows(0).Item("ac_confidential_notes").ToString.Trim + "</div>")
                            End If
                        End If
                    End If
                End If

                aircraft_status_label.Text += ("</div>")

                htmlOut.Append("</div>")

            End If ' (Not (in_JournalRs.eof AND in_JournalRs.bof)) AND (Not isNull(in_JournalRs("journ_id")))
        End If
        Return htmlOut.ToString.Trim

    End Function




#Region "PDF/Aircraft Details Blocks"
    'Public Shared Function Build_Features_Block(ByVal chkIncludeKeyFeatures As CheckBox, ByVal Counter_For_PDF As Integer, ByVal check_cover As CheckBox, ByVal TableColor As String, ByVal spacer_width As String, ByVal nAircraftID As Long, ByVal bWordReport As Boolean, ByVal word_width As String, ByVal pdf_html_width As String, ByVal features_text As TextBox) As String
    '  Dim comp_functions As New CompanyFunctions
    '  Dim temp_feat As String = ""
    '  Dim features_ac As String = ""
    '  Dim featureString As String = ""
    '  Dim featureCounter As Integer = 0
    '  If chkIncludeKeyFeatures.Checked = True Then
    '    temp_feat = Trim(features_text.Text)

    '    Dim featArray As ArrayList = New ArrayList()
    '    featArray.AddRange(Split(temp_feat, vbLf))

    '    For Each i As String In featArray
    '      If i <> "" Then
    '        If featureCounter = 2 And featArray.Count > 9 Then
    '          featureCounter = 0
    '          featureString += "</tr><tr>"
    '        ElseIf featArray.Count < 10 Then
    '          featureString += "</tr><tr>"
    '        End If
    '        featureCounter += 1
    '        featureString += "<td width=""50%"">" + i & "</td>"
    '      End If
    '    Next

    '    temp_feat = featureString

    '    If Trim(temp_feat) = "" And features_text.Visible = False And chkIncludeKeyFeatures.Checked = True Then
    '      'temp_feat = (build_full_spec_aircraft_features(nAircraftID, SqlCommand, SqlReader, 54))
    '    End If


    '    If Trim(temp_feat) <> "" Then
    '      If check_cover.Checked = True Then
    '        temp_feat = temp_feat
    '        If bWordReport = True Then
    '          features_ac &= ("<div class=""Box""><table width='" & word_width & "' align='center'>")
    '        Else
    '          features_ac &= ("<div class=""Box""><table width='" & pdf_html_width & "' align='center' class=""formatTable large " & TableColor & " subtextNoMargin"">")
    '        End If

    '        features_ac &= ("<tr class=""noBorder""><td valign='middle' align='center' colspan='2' class=""left""><font class='" & HttpContext.Current.Session("FONT_CLASS_HEADER") & " subHeader'>Features/Highlights</font></td></tr>")
    '        features_ac += temp_feat
    '        features_ac &= ("</table></div>")
    '      Else
    '        temp_feat = Replace(temp_feat, vbLf, "; ") '
    '        If Right(Trim(temp_feat), 1) = ";" Then
    '          temp_feat = Left(Trim(temp_feat), Len(Trim(temp_feat)) - 1)
    '        End If
    '        features_ac = comp_functions.create_value_with_label("Features/Highlights", temp_feat, True, True, Counter_For_PDF, spacer_width)
    '      End If
    '    End If
    '  End If
    '  Return features_ac
    'End Function

    Public Shared Function Build_Avionics_Block(ByVal nAircraftID As Long, ByVal nAircraftJournalID As Long, ByRef avionicsCount As Integer, ByVal spacer_width As String, ByVal out_AircraftRS As DataTable, ByVal aclsData_Temp As clsData_Manager_SQL, ByVal crmSource As String, ByVal bWordReport As Boolean, ByRef counter_For_PDf As Integer, ByVal word_width As String, ByVal pdf_html_width As String, ByVal tableColor As String, ByVal showEditLink As Boolean, ByVal otherID As Long) As String
        Dim Results_Table As New DataTable
        Dim sQuery = New StringBuilder()
        Dim outString As String = ""
        Dim last_detail_type As String = ""

        Try

            If crmSource = "CLIENT" And nAircraftJournalID = 0 Then
                Results_Table = aclsData_Temp.Get_Client_Aircraft_Avionics_As_Jetnet_Fields(out_AircraftRS.Rows(0).Item("CLIENT_ID"))
            ElseIf crmSource = "CLIENT" And nAircraftJournalID > 0 Then
                Results_Table = aclsData_Temp.GetJETNET_Aircraft_Avionics_AC_ID(otherID, out_AircraftRS.Rows(0).Item("ac_journ_id"))
            Else
                Results_Table = aclsData_Temp.GetJETNET_Aircraft_Avionics_AC_ID(out_AircraftRS.Rows(0).Item("ac_id"), out_AircraftRS.Rows(0).Item("ac_journ_id"))
            End If
            outString = "<div class=""Box"">"
            If bWordReport = True Then
                outString += ("<table width='" & word_width & "' align='center'>")
            Else
                outString += ("<table width='" & pdf_html_width & "' align='center' class=""formatTable large " & tableColor & " subtextNoMargin"">")
            End If

            outString += "<tr class=""noBorder""><td colspan='2'><font class='" & HttpContext.Current.Session("FONT_CLASS_HEADER") & " subHeader'>Avionics" & IIf(showEditLink, CreateEditLink("avionics", crmSource, nAircraftID, "height=400,width=450", False, False, ""), "") & "</font></td></tr>"


            If Results_Table.Rows.Count > 0 Then

                For Each r As DataRow In Results_Table.Rows
                    avionicsCount += 1

                    If Not IsDBNull(r("av_name")) Then



                        If last_detail_type.ToLower.Trim = r("av_name").ToString.ToLower.Trim Then
                            outString += "<font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>;  " + HttpContext.Current.Server.HtmlEncode(r("av_description").ToString.Trim) + "</font>"
                        Else
                            If Trim(last_detail_type) <> "" Then
                                outString += "</td></tr>"
                            End If
                            counter_For_PDf = counter_For_PDf + 1
                            outString += "<tr valign='top' class='" & HttpContext.Current.Session("ROW_CLASS_BOTTOM") & "'><td " & IIf(spacer_width = "", "nowrap", "width='" & spacer_width & "'") & ">"

                            outString += "<font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT_TITLE") & "'>" + r("av_name").ToString.Trim + ": </font>"

                            outString += "</td><td>"
                            outString += "<font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>" + HttpContext.Current.Server.HtmlEncode(r("av_description").ToString.Trim) + "</font>"
                        End If

                        last_detail_type = r("av_name").ToString.Trim

                    End If
                Next
            Else
                outString += "<tr><td colspan='2'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>No Details Reported.</font></td></tr>"
            End If

            outString += "</table></div>"

        Catch ex As Exception
            outString += "<div class=""Box""><table width='" & pdf_html_width & "' align='center' class=""formatTable large " & tableColor & " subtextNoMargin""><tr><td colspan='2' align=""left""><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>No Details Reported. " & ex.Message.ToString & "</font></td></tr></table></div>" + vbCrLf
        End Try

        Return outString

    End Function
    Public Shared Function Build_APU_Block(ByVal out_AircraftRS As DataTable, ByVal bWordReport As Boolean, ByRef counter_For_PDf As Integer, ByVal word_width As String, ByVal pdf_html_width As String, ByVal tableColor As String, ByVal spacer_width As String, ByVal crmSource As String, ByVal aircraftID As Long, ByVal showEditLink As Boolean, ByVal aclsData_Temp As clsData_Manager_SQL) As String
        Dim ac_apu_model_name As String = ""
        Dim ac_apu_tot_hrs As Integer
        Dim ac_apu_ser_no As String = ""
        Dim ac_apu_maintance_program As String = ""
        Dim showAPUTable As Boolean = False
        Dim comp_functions As New CompanyFunctions
        Dim outString As String = ""

        Try
            If bWordReport = True Then
                outString = ("<table width='" & word_width & "' align='center'>")
            Else
                outString = ("<table width='" & pdf_html_width & "' align='center' class=""formatTable large " & tableColor & " subtextNoMargin"">")
            End If



            counter_For_PDf = counter_For_PDf + 1
            If out_AircraftRS.Rows.Count > 0 Then

                If Not IsDBNull(out_AircraftRS.Rows(0).Item("ac_apu_model_name")) Then
                    ac_apu_model_name = out_AircraftRS.Rows(0).Item("ac_apu_model_name").ToString.Trim
                    outString += "<tr class=""noBorder""><td colspan='2'><span class='" & HttpContext.Current.Session("FONT_CLASS_HEADER") & " subHeader'>APU: <strong>" & out_AircraftRS.Rows(0).Item("ac_apu_model_name").ToString.Trim & "</strong>" & IIf(showEditLink, CreateEditLink("apu", crmSource, aircraftID, "height=400,width=450", False, False, ""), "") & "</span></td></tr>"
                Else
                    ac_apu_model_name = ""
                    outString += "<tr class=""noBorder""><td colspan='2'><span class='" & HttpContext.Current.Session("FONT_CLASS_HEADER") & " subHeader'>APU: " & CreateEditLink("apu", crmSource, aircraftID, "height=400,width=450", False, False, "") & "</span></td></tr>"
                End If

                If Not IsDBNull(out_AircraftRS.Rows(0).Item("ac_apu_tot_hrs")) Then
                    ac_apu_tot_hrs = CLng(out_AircraftRS.Rows(0).Item("ac_apu_tot_hrs").ToString)
                Else
                    ac_apu_tot_hrs = 0
                End If

                If Not IsDBNull(out_AircraftRS.Rows(0).Item("ac_apu_ser_no")) Then
                    ac_apu_ser_no = out_AircraftRS.Rows(0).Item("ac_apu_ser_no").ToString.Trim
                Else
                    ac_apu_ser_no = ""
                End If

                If Not IsDBNull(out_AircraftRS.Rows(0).Item("ac_apu_maintance_program")) Then
                    ac_apu_maintance_program = out_AircraftRS.Rows(0).Item("ac_apu_maintance_program").ToString.Trim

                    If crmSource = "CLIENT" Then
                        'We need one extra lookup:
                        Dim ApuTable As DataTable = aclsData_Temp.APUMaintenanceName(out_AircraftRS.Rows(0).Item("ac_apu_maintance_program").ToString.Trim)
                        If Not IsNothing(ApuTable) Then
                            If ApuTable.Rows.Count > 0 Then
                                ac_apu_maintance_program = ApuTable.Rows(0).Item("emp_name")
                            End If
                        End If
                    End If

                Else
                    ac_apu_maintance_program = ""
                End If



                If Not String.IsNullOrEmpty(ac_apu_model_name) Or ac_apu_tot_hrs > 0 Or Not String.IsNullOrEmpty(ac_apu_ser_no) Then


                    If Not String.IsNullOrEmpty(ac_apu_ser_no) Then
                        outString += comp_functions.create_value_with_label("Serial #", ac_apu_ser_no.Trim, True, True, counter_For_PDf, spacer_width)
                        showAPUTable = True
                    End If

                    If Not String.IsNullOrEmpty(ac_apu_model_name) Then
                        If ac_apu_tot_hrs > 0 Then
                            outString += comp_functions.create_value_with_label("Time Since New", FormatNumber(ac_apu_tot_hrs, 0, True, False, True) & " hrs", True, True, counter_For_PDf, spacer_width)
                            showAPUTable = True
                        End If
                    End If

                    If Not String.IsNullOrEmpty(ac_apu_model_name) Then
                        If Trim(ac_apu_maintance_program) <> "" Then
                            outString += comp_functions.create_value_with_label("Maintenance Plan", ac_apu_maintance_program, True, True, counter_For_PDf, spacer_width)
                            showAPUTable = True
                        End If
                    End If


                End If

                If Not IsDBNull(out_AircraftRS.Rows(0).Item("ac_apu_soh_hrs")) Then
                    outString += comp_functions.create_value_with_label("Since Overhaul (SOH)", FormatNumber(CDbl(out_AircraftRS.Rows(0).Item("ac_apu_soh_hrs").ToString), 0, True, False, True) & " hrs", True, True, counter_For_PDf, spacer_width)
                    showAPUTable = True
                End If


                If Not IsDBNull(out_AircraftRS.Rows(0).Item("ac_apu_shi_hrs")) Then
                    outString += comp_functions.create_value_with_label("Since Hot Inspection (SHI)", FormatNumber(CDbl(out_AircraftRS.Rows(0).Item("ac_apu_shi_hrs").ToString), 0, True, False, True) & " hrs", True, True, counter_For_PDf, spacer_width)
                    showAPUTable = True
                End If
            End If

            If showAPUTable = False Then
                outString += "<tr><td colspan=""2"" align=""left""><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>No Details Reported.</font></td></tr>" + vbCrLf
            End If

            outString += "</table>"


            outString = "<div class=""Box"">" & outString & "</div>"

        Catch ex As Exception
            outString += "<table width='" & pdf_html_width & "' align='center' class=""formatTable large " & tableColor & " subtextNoMargin""><tr><td colspan='2' align=""left""><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>No Details Reported. " & ex.Message.ToString & "</font></td></tr></table>" + vbCrLf
        End Try
        Return outString

    End Function

    Public Shared Function Build_Details_Block(ByVal detailsType As String, ByVal out_AircraftRS As DataTable, ByVal update_date As String, ByVal done_by As String, ByVal CRMSource As String, ByVal bWordReport As Boolean, ByRef counter_For_PDf As Integer, ByVal word_width As String, ByVal pdf_html_width As String, ByVal tableColor As String, ByVal spacer_width As String, ByVal aclsData_Temp As clsData_Manager_SQL, ByVal aircraftID As Long, ByVal crmView As Boolean, ByVal showEditLink As Boolean, ByVal journalID As Long, ByVal otherID As Long, Optional ByRef est_afmp As String = "")
        Dim ResultsTable As New DataTable
        Dim outString As String = ""
        Dim cert As String = ""

        Dim comp_functions As New CompanyFunctions
        If CRMSource = "CLIENT" And journalID = 0 Then
            ResultsTable = aclsData_Temp.Get_Client_Aircraft_Details_As_Jetnet_Fields(out_AircraftRS.Rows(0).Item("CLIENT_ID"), detailsType)
        ElseIf CRMSource = "CLIENT" And journalID > 0 Then
            ResultsTable = aclsData_Temp.GetJETNET_Aircraft_Details_Equipment_AC_ID_TYPE(otherID, detailsType, out_AircraftRS.Rows(0).Item("ac_journ_id"))
        Else
            ResultsTable = aclsData_Temp.GetJETNET_Aircraft_Details_Equipment_AC_ID_TYPE(out_AircraftRS.Rows(0).Item("ac_id"), detailsType, out_AircraftRS.Rows(0).Item("ac_journ_id"))
        End If

        outString = "<div class=""Box"">"

        If detailsType = "Addl Cockpit Equipment','Equipment" Then
            detailsType = "Additional Equipment"
        End If


        If bWordReport = True Then

            outString &= ("<table width='" & word_width & "' align='center'>")
        Else
            outString &= ("<table width='" & pdf_html_width & "' align='center' class=""formatTable " & tableColor & " subtextNoMargin large noBoldUpper"">")
        End If
        outString &= "<tr class=""noBorder""><td colspan='2' align='left'><font class='" & HttpContext.Current.Session("FONT_CLASS_HEADER") & " subHeader'>" & detailsType & "&nbsp;" & IIf(showEditLink, CreateEditLink("details", CRMSource, aircraftID, "height=400,width=450", False, False, detailsType), "") & "</font></td></tr>"

        Try


            If detailsType = "maintenance" Then
                If Not IsDBNull(out_AircraftRS.Rows(0).Item("ac_maintained")) Then
                    outString &= comp_functions.create_value_with_label("Maintained", out_AircraftRS.Rows(0).Item("ac_maintained").ToString.Trim, True, True, counter_For_PDf, spacer_width)
                End If

                If Not IsDBNull(out_AircraftRS.Rows(0).Item("amp_program_name")) Then
                    outString &= comp_functions.create_value_with_label("Airframe Maint Program", out_AircraftRS.Rows(0).Item("amp_program_name").ToString.Trim, True, True, counter_For_PDf, spacer_width)
                    est_afmp = out_AircraftRS.Rows(0).Item("amp_program_name").ToString.Trim
                End If

                If Not IsDBNull(out_AircraftRS.Rows(0).Item("amtp_program_name")) Then
                    outString &= comp_functions.create_value_with_label("Airframe Tracking Program", out_AircraftRS.Rows(0).Item("amtp_program_name").ToString.Trim, True, True, counter_For_PDf, spacer_width)
                End If

                If CRMSource <> "CLIENT" Or CRMSource = "CLIENT" And journalID > 0 Then
                    'Since there's no client table, for right now, we're only showing this for jetnet aircraft
                    If Not IsDBNull(out_AircraftRS.Rows(0).Item("certifications")) Then
                        cert = Left(Trim(out_AircraftRS.Rows(0).Item("certifications").ToString.Trim), Len(Trim(out_AircraftRS.Rows(0).Item("certifications").ToString.Trim)) - 1) ' get rid of last comma
                        outString &= comp_functions.create_value_with_label("Certification(s)", cert, True, True, counter_For_PDf, spacer_width)
                    End If
                End If


                'ac_maint_hots_moyear
                Dim acMaintMODisplay As String = ""
                If Not IsDBNull(out_AircraftRS.Rows(0).Item("ac_maint_hots_by_name")) Then
                    If Not String.IsNullOrEmpty(out_AircraftRS.Rows(0).Item("ac_maint_hots_by_name")) Then
                        acMaintMODisplay = "By&nbsp;" + out_AircraftRS.Rows(0).Item("ac_maint_hots_by_name").ToString.Trim
                    End If
                End If

                If Not IsDBNull(out_AircraftRS.Rows(0).Item("ac_maint_hots_moyear")) Then
                    If Len(Trim(out_AircraftRS.Rows(0).Item("ac_maint_hots_moyear"))) > 4 Then
                        acMaintMODisplay += "&nbsp;In&nbsp;" + out_AircraftRS.Rows(0).Item("ac_maint_hots_moyear").ToString.Substring(0, 2) + "/" + out_AircraftRS.Rows(0).Item("ac_maint_hots_moyear").ToString.Substring(2, 4) + ""
                    Else
                        acMaintMODisplay += "&nbsp;In&nbsp;" + out_AircraftRS.Rows(0).Item("ac_maint_hots_moyear").ToString
                    End If
                End If


                If Not String.IsNullOrEmpty(acMaintMODisplay) Then
                    outString &= comp_functions.create_value_with_label("Hot Inspection", acMaintMODisplay, True, True, counter_For_PDf, spacer_width)
                End If

                'ac_main_eoh_moyear
                Dim acMaintEOHDisplay As String = ""
                If Not IsDBNull(out_AircraftRS.Rows(0).Item("ac_maint_eoh_by_name")) Then
                    If Not String.IsNullOrEmpty(out_AircraftRS.Rows(0).Item("ac_maint_eoh_by_name")) Then
                        acMaintEOHDisplay = "By&nbsp;" + out_AircraftRS.Rows(0).Item("ac_maint_eoh_by_name").ToString.Trim
                    End If
                End If
                If Not IsDBNull(out_AircraftRS.Rows(0).Item("ac_main_eoh_moyear")) Then
                    If Not String.IsNullOrEmpty(out_AircraftRS.Rows(0).Item("ac_main_eoh_moyear")) Then
                        Dim lenT As Integer = Len(Trim(out_AircraftRS.Rows(0).Item("ac_main_eoh_moyear")))
                        If Len(Trim(out_AircraftRS.Rows(0).Item("ac_main_eoh_moyear"))) > 4 Then
                            acMaintEOHDisplay += "&nbsp;In&nbsp;" + Trim(out_AircraftRS.Rows(0).Item("ac_main_eoh_moyear")).ToString.Substring(0, 2) + "/" + Trim(out_AircraftRS.Rows(0).Item("ac_main_eoh_moyear")).ToString.Substring(2, 4) + ""
                        Else
                            acMaintEOHDisplay += "&nbsp;In&nbsp;" + Trim(out_AircraftRS.Rows(0).Item("ac_main_eoh_moyear").ToString)
                        End If
                    End If
                End If
                If Not String.IsNullOrEmpty(acMaintEOHDisplay) Then
                    outString &= comp_functions.create_value_with_label("Engine Overhaul", acMaintEOHDisplay, True, True, counter_For_PDf, spacer_width)
                End If
                'End If

                If Not IsDBNull(out_AircraftRS.Rows(0).Item("ac_damage_history_notes")) Then
                    outString &= comp_functions.create_value_with_label("Dam History Notes", out_AircraftRS.Rows(0).Item("ac_damage_history_notes").ToString.Trim, True, True, counter_For_PDf, spacer_width)
                End If

            End If


            If detailsType = "interior" Then
                Dim temp_moyear As String = ""
                Dim in_done_by_and_rating As String = ""
                Dim passengerCount As Integer = 0
                Dim configName As String = ""
                ' THIS IS FOR INTERIOR SECTION ------ 
                If Not IsDBNull(out_AircraftRS.Rows(0).Item("ac_interior_moyear")) Then
                    If out_AircraftRS.Rows(0).Item("ac_interior_moyear").ToString.Trim.Length > 0 Then
                        If out_AircraftRS.Rows(0).Item("ac_interior_moyear").ToString.Length > 4 Then
                            temp_moyear = out_AircraftRS.Rows(0).Item("ac_interior_moyear").ToString
                            If temp_moyear.ToString.Length = 5 Then
                                temp_moyear = Left(temp_moyear, 1) + "/" + Right(temp_moyear, 4)
                            Else
                                If temp_moyear.ToString.Length = 6 Then
                                    temp_moyear = Left(temp_moyear, 2) + "/" + Right(temp_moyear, 4)
                                End If
                            End If

                            If Left(Trim(temp_moyear), 1) = "/" Then
                                temp_moyear = Right(Trim(temp_moyear), Len(Trim(temp_moyear)) - 1)
                            End If
                        End If
                    End If
                End If



                If Not IsDBNull(out_AircraftRS.Rows(0).Item("ac_interior_doneby_name")) Then
                    If Trim(temp_moyear) <> "" Then
                        done_by += "<strong>BY " & out_AircraftRS.Rows(0).Item("ac_interior_doneby_name").ToString & " on " & Trim(temp_moyear) & "</strong>"
                    Else
                        done_by += " <strong>BY " & out_AircraftRS.Rows(0).Item("ac_exterior_doneby_name").ToString & "</strong>"
                    End If

                ElseIf Trim(temp_moyear) <> "" Then
                    done_by = "<strong>DONE ON " & Trim(temp_moyear) & "</strong>"
                End If

                If Not IsDBNull(out_AircraftRS.Rows(0).Item("ac_interior_rating")) Then
                    in_done_by_and_rating += comp_functions.create_value_with_label("Rating", out_AircraftRS.Rows(0).Item("ac_interior_rating").ToString, True, True, counter_For_PDf, spacer_width)
                End If
                If Not IsDBNull(out_AircraftRS.Rows(0).Item("ac_passenger_count")) Then
                    passengerCount = out_AircraftRS.Rows(0).Item("ac_passenger_count")
                End If

                If Not IsDBNull(out_AircraftRS.Rows(0).Item("ac_interior_config_name")) And Not String.IsNullOrEmpty(out_AircraftRS.Rows(0).Item("ac_interior_config_name").ToString) Then
                    configName = out_AircraftRS.Rows(0).Item("ac_interior_config_name")
                End If



                If passengerCount > 0 Or configName <> "" Then
                    If (configName <> "" And passengerCount > 0) Then
                        in_done_by_and_rating += comp_functions.create_value_with_label("Configuration/PAX", configName & "/" & passengerCount.ToString & " passengers", True, True, counter_For_PDf, spacer_width)
                    ElseIf configName <> "" Then
                        in_done_by_and_rating += comp_functions.create_value_with_label("Configuration", configName, True, True, counter_For_PDf, spacer_width)
                    ElseIf passengerCount > 0 Then
                        in_done_by_and_rating += comp_functions.create_value_with_label("PAX", passengerCount.ToString & " passengers", True, True, counter_For_PDf, spacer_width)
                    End If
                End If
                outString += in_done_by_and_rating

            End If

            If detailsType = "exterior" Then
                Dim temp_ex_moyear As String = ""
                If Not IsDBNull(out_AircraftRS.Rows(0).Item("ac_exterior_moyear")) Then
                    If out_AircraftRS.Rows(0).Item("ac_exterior_moyear").ToString.Trim.Length > 0 Then
                        temp_ex_moyear = out_AircraftRS.Rows(0).Item("ac_exterior_moyear").ToString
                        If temp_ex_moyear.Trim.Length = 5 Then
                            temp_ex_moyear = Left(temp_ex_moyear, 1) + "/" + Right(temp_ex_moyear, 4)
                        Else
                            If temp_ex_moyear.Trim.Length = 6 Then
                                temp_ex_moyear = Left(temp_ex_moyear, 2) + "/" + Right(temp_ex_moyear, 4)
                            End If
                        End If

                        If Left(Trim(temp_ex_moyear), 1) = "/" Then
                            temp_ex_moyear = Right(Trim(temp_ex_moyear), Len(Trim(temp_ex_moyear)) - 1)
                        End If
                    End If
                End If

                If Not IsDBNull(out_AircraftRS.Rows(0).Item("ac_exterior_doneby_name")) Then
                    If Trim(temp_ex_moyear) <> "" Then
                        done_by = " <strong>BY " & out_AircraftRS.Rows(0).Item("ac_exterior_doneby_name").ToString & " on " & Trim(temp_ex_moyear) & "</strong>"
                    Else
                        done_by = " <strong>BY " & out_AircraftRS.Rows(0).Item("ac_exterior_doneby_name").ToString & "</strong>"
                    End If
                ElseIf Trim(temp_ex_moyear) <> "" Then
                    done_by = "<strong>DONE ON " & Trim(temp_ex_moyear) & "</strong>"
                End If

                If Not IsDBNull(out_AircraftRS.Rows(0).Item("ac_exterior_rating")) Then
                    outString += comp_functions.create_value_with_label("Rating", out_AircraftRS.Rows(0).Item("ac_exterior_rating").ToString, True, True, counter_For_PDf, spacer_width)
                End If
            End If

            If Trim(done_by) <> "" Then
                outString = Replace(outString, detailsType & "&nbsp;</font>", detailsType & "&nbsp;<span>" & done_by & "</span></font>")
            ElseIf Trim(update_date) <> "" Then
                outString &= comp_functions.create_value_with_label("Last Done", update_date, True, True, counter_For_PDf, spacer_width)
            End If


            Dim last_detail_section As String = ""
            Dim last_detail_type As String = ""
            Dim resultsList As String = ""
            If ResultsTable.Rows.Count > 0 Then

                For Each r As DataRow In ResultsTable.Rows


                    If Not IsDBNull(r("adet_data_name")) Then

                        If (last_detail_type.ToLower.Trim = r("adet_data_name").ToString.ToLower.Trim) And (last_detail_section.ToLower.Trim = r("adet_data_type").ToString.ToLower.Trim) Then
                            outString += "<font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>;  " & Replace(HttpContext.Current.Server.HtmlEncode(r("adet_data_description").ToString.Trim), "&amp;", "&") & "</font>"
                        Else
                            If Trim(last_detail_type) <> "" Then
                                outString += "</td></tr>"
                            End If
                            counter_For_PDf = counter_For_PDf + 1
                            outString += "<tr valign='top' class='" & HttpContext.Current.Session("ROW_CLASS_BOTTOM") & "'><td  " & IIf(spacer_width = "", "nowrap", "  width='" & spacer_width & "'") & ">"

                            If Trim(detailsType) = "Additional Equipment" And r("adet_data_name").ToString.Trim = "General" And r("adet_data_type").ToString.Trim = "Addl Cockpit Equipment" Then
                                outString += "<font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT_TITLE") & "'>Cockpit: </font>"
                            ElseIf Trim(detailsType) = "Additional Equipment" And r("adet_data_name").ToString.Trim = "General" And r("adet_data_type").ToString.Trim = "Equipment" Then
                                outString += "<font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT_TITLE") & "'>Equipment: </font>"
                            ElseIf Trim(detailsType) = "Interior" And r("adet_data_name").ToString.Trim = "Refreshment Equipment" Then
                                outString += "<font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT_TITLE") & "'>Refreshment: </font>"
                            ElseIf Trim(detailsType) = "Interior" And r("adet_data_name").ToString.Trim = "Entertainment Equipment" Then
                                outString += "<font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT_TITLE") & "'>Entertainment: </font>"

                            Else
                                outString += "<font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT_TITLE") & "'>" & Replace(Replace(Replace(Replace(r("adet_data_name").ToString.Trim, "Inspection", "Notes"), "Equip ", ""), "Equipment", ""), "Woodwork", "Wood") & ": </font>"
                            End If

                            outString += "</td><td>"
                            resultsList = Replace(HttpContext.Current.Server.HtmlEncode(r("adet_data_description").ToString.Trim), "&amp;", "&")
                            resultsList = resultsList.TrimEnd(",")
                            outString += "<font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>" + resultsList + "</font>"
                        End If


                        last_detail_type = r("adet_data_name").ToString.Trim
                        If Not IsDBNull(r("adet_data_type")) Then
                            last_detail_section = r("adet_data_type").ToString.Trim
                        Else
                            last_detail_section = ""
                        End If

                    End If


                Next

                outString &= "</td></tr>"
            ElseIf (LCase(detailsType)) <> "maintenance" Then
                outString &= "<tr><td colspan='2' align=""left""><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>No Details Reported.&nbsp;</font></td></tr>"
            End If


            outString &= "</table>"

            If detailsType = "maintenance" Then
                Dim tempCount As Integer = 0
                Dim dateFormatted As String = ""
                Dim tempLabel As String = ""
                Dim tempDate As String = ""

                If CRMSource = "CLIENT" Then
                    ResultsTable = clsGeneral.clsGeneral.Get_Maintenance_By_ID_Client(aircraftID)
                Else
                    ResultsTable = aclsData_Temp.GetJETNET_Aircraft_Maintenance_BY_ACID(out_AircraftRS.Rows(0).Item("ac_id"), out_AircraftRS.Rows(0).Item("ac_journ_id"))
                End If

                If Not IsNothing(ResultsTable) Then

                    If ResultsTable.Rows.Count > 0 Then
                        outString &= "<hr />"
                        If bWordReport = True Then
                            outString += "<table width='" & word_width & "' align='center' cellpadding='3'>"
                        Else
                            outString += "<table width='" & pdf_html_width & "' align='center' cellpadding='0' cellspacing=""0"" class=""formatTable maintenanceTable " & tableColor & " subtextNoMargin "">"
                        End If

                        outString += "<tr><td style=""width:140px !important;""><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT_TITLE") & " subHeader'>Items</font></td>"
                        outString += "<td><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT_TITLE") & " subHeader'>C/W&nbsp;</font></td>"
                        outString += "<td><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT_TITLE") & " subHeader'>DUE&nbsp;</font></td>"
                        outString += "<td><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT_TITLE") & " subHeader'>NOTES&nbsp;</font></td>"
                        If crmView = True And showEditLink = True Then
                            If CRMSource = "CLIENT" Then
                                outString += "<td><a href=""javascript:void(0);"" onclick=""javascript:load('/maintenance.aspx?acid=" & out_AircraftRS.Rows(0).Item("ac_id") & "&cliacid=" & aircraftID & "','','scrollbars=yes,menubar=no,height=900,width=940,resizable=yes,toolbar=no,location=no,status=no');return false;"" class=""float_right padding_left""><img src=""images/edit_icon.png"" alt=""Edit Client"" /></a></td>"
                            End If
                        End If
                        outString += "</tr>"

                        For Each r As DataRow In ResultsTable.Rows

                            tempCount += 1

                            If Not IsDBNull(r("acmaint_date_type")) And Not String.IsNullOrEmpty(r("acmaint_date_type")) Then
                                Select Case UCase(r("acmaint_date_type"))
                                    Case "D"
                                        dateFormatted = "MM/dd/yy"
                                    Case "M"
                                        dateFormatted = "MM/yy"
                                    Case "Y"
                                        dateFormatted = "yyyy"
                                End Select
                            End If

                            tempLabel = ""
                            tempLabel &= ("<td align='left'>")
                            If Not IsDBNull(r("acmaint_complied_date")) And Not String.IsNullOrEmpty(r("acmaint_complied_date").ToString) Then
                                tempDate = Format(r("acmaint_complied_date"), dateFormatted)
                                tempLabel &= ("<font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>" & tempDate & "</font>")
                            End If

                            If Not IsDBNull(r("acmaint_complied_hrs")) And Not String.IsNullOrEmpty(r("acmaint_complied_hrs").ToString) Then
                                If IsNumeric(r("acmaint_complied_hrs")) Then
                                    If r("acmaint_complied_hrs") > 0 Then
                                        If Not String.IsNullOrEmpty(tempDate) Then
                                            tempLabel &= ", "
                                        End If
                                        tempLabel &= (" <font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>" + r("acmaint_complied_hrs").ToString.Trim & " hrs</font>")
                                    End If
                                End If
                            End If

                            tempLabel &= ("</td>")
                            tempLabel &= ("<td align='left'>")
                            tempDate = ""
                            If Not IsDBNull(r("acmaint_due_date")) And Not String.IsNullOrEmpty(r("acmaint_due_date").ToString) Then
                                tempDate = Format(r("acmaint_due_date"), dateFormatted)
                                tempLabel &= ("<font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>" & tempDate & "</font>")
                            End If

                            If Not IsDBNull(r("acmaint_due_hrs")) And Not String.IsNullOrEmpty(r("acmaint_due_hrs").ToString) Then
                                If IsNumeric(r("acmaint_due_hrs")) Then
                                    If r("acmaint_due_hrs") > 0 Then
                                        If Not String.IsNullOrEmpty(tempDate) Then
                                            tempLabel &= ", "
                                        End If
                                        tempLabel &= (" <font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>" + r("acmaint_due_hrs").ToString.Trim & " hrs</font>")
                                    End If
                                End If
                            End If
                            tempLabel &= ("</td>")

                            If Not IsDBNull(r("acmaint_notes")) And Not String.IsNullOrEmpty(r("acmaint_notes").ToString) Then
                                If InStr(Trim(r("acmaint_notes").ToString), "as reported") > 0 Then
                                    tempLabel &= ("<td align='left' " & IIf(crmView, IIf(CRMSource = "CLIENT", "colspan=""2""", ""), "") & "  width=""150""><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>" & Replace(r("acmaint_notes").ToString.Trim, "as reported", "Date(s) as reported/not actual.") & "</font></td>")
                                Else
                                    tempLabel &= ("<td align='left' " & IIf(crmView, IIf(CRMSource = "CLIENT", "colspan=""2""", ""), "") & "  width=""150""><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>" & r("acmaint_notes").ToString.Trim & "</font></td>")
                                End If
                            Else
                                tempLabel &= ("<td align='left' " & IIf(crmView, IIf(CRMSource = "CLIENT", "colspan=""2""", ""), "") & "><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>&nbsp;</font></td>")
                            End If



                            outString += "<tr class='" & HttpContext.Current.Session("ROW_CLASS_BOTTOM") & "' valign='top'>"
                            outString += "<td align='left' width='200'>"

                            tempLabel = "</td>" & tempLabel
                            outString += comp_functions.create_value_with_label(r("acmaint_name").ToString.Trim, tempLabel, False, False, counter_For_PDf, spacer_width)
                            outString += "</tr>"

                            counter_For_PDf = counter_For_PDf + 1
                        Next
                        outString += "</table>"

                    End If


                End If

                ResultsTable = New DataTable
            End If

            outString += "</div>"


        Catch ex As Exception
            outString += "<tr><td colspan='2' align=""left""><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>No Details Reported. " & ex.Message.ToString & "</font></td></tr>" + vbCrLf
        End Try

        Return outString

    End Function

    Public Shared Function CreateEditLink(ByVal typeDis As String, ByVal crmSource As String, ByVal aircraftID As Long, ByVal heightAndWidth As String, ByVal EditMainAircraftCLIENT As Boolean, ByVal CreateMainAircraftJETNET As Boolean, ByVal detailsType As String, Optional ByVal UseText As Boolean = False) As String
        Dim returnString As String = ""
        If crmSource = "CLIENT" Then
            If aircraftID > 0 Then
                If EditMainAircraftCLIENT = True Then
                    returnString = "<a href=""javascript:void(0);"" onclick=""javascript:load('/edit.aspx?action=edit&type=aircraft&ac_ID=" & aircraftID.ToString & "&source=CLIENT&from=aircraftDetails','','scrollbars=yes,menubar=no,height=900,width=940,resizable=yes,toolbar=no,location=no,status=no');return false;"" " & IIf(UseText, """>Edit Client", "class=""float_right padding_left""><img src=""images/edit_icon.png"" alt=""Edit Client"" />") & "</a>"
                Else
                    If detailsType = "" Then
                        returnString = "<span class=""float_right""><a href=""javascript:void(0);"" onclick=""javascript:load('/edit.aspx?action=edit&type=" & typeDis & "&Listing=3&ac_ID=" & aircraftID.ToString & "&source=CLIENT&from=aircraftDetails','','scrollbars=yes,menubar=no," & heightAndWidth & ",resizable=yes,toolbar=no,location=no,status=no');return false; class=""float_right padding_left""><img src=""images/edit_icon.png"" alt=""Edit Client"" /></a></span>"
                    Else
                        If detailsType = "maintenance" Then
                            detailsType = "main"
                        ElseIf detailsType = "Additional Equipment" Then
                            detailsType = "equip"
                        End If
                        returnString = "<span class=""float_right""><a href=""javascript:void(0);"" onclick=""javascript:load('/edit.aspx?action=edit&type=" & typeDis & "&Listing=3&typeofdetails=" & detailsType & "&ac_ID=" & aircraftID.ToString & "&source=CLIENT&from=aircraftDetails','','scrollbars=yes,menubar=no," & heightAndWidth & ",resizable=yes,toolbar=no,location=no,status=no');return false;"" class=""float_right padding_left""><img src=""images/edit_icon.png"" alt=""Edit Client"" /></a></span>"
                    End If
                End If
            End If
        ElseIf crmSource <> "CLIENT" And CreateMainAircraftJETNET = True Then
            returnString = "<a href=""javascript:void(0);"" onclick=""javascript:load('/edit.aspx?action=edit&type=aircraft&ac_ID=" & aircraftID.ToString & "&source=JETNET&from=aircraftDetails','','scrollbars=yes,menubar=no,height=900,width=940,resizable=yes,toolbar=no,location=no,status=no');return false;"" title=""Create Client Record"">" & IIf(UseText, "Create Client", "<img src=""images/edit_icon.png"" alt=""Create Client"" />") & "</a>"
        End If
        Return returnString
    End Function
    Public Shared Function Build_Engine_Block(ByVal aircraftID As Long, ByVal out_AircraftRS As DataTable, ByVal bWordReport As Boolean, ByRef counter_For_PDf As Integer, ByVal word_width As String, ByVal pdf_html_width As String, ByVal tableColor As String, ByVal spacer_width As String, ByVal CrmSOURCE As String, ByVal showEditLink As Boolean, ByRef MySesState As HttpSessionState, ByVal bShowBlankAcFields As Boolean, Optional ByVal from_spot As String = "")
        Dim htmlOutput As String = ""
        Dim comp_functions As New CompanyFunctions
        Dim sAirframeType As String = ""
        Dim sAircraftType As String = ""
        Dim nloopCount As Integer = 0
        Dim temp_start As String = ""
        Dim temp_ser_string As String = ""
        Dim temp_ttsn_string As String = ""
        Dim temp_since_over_string As String = ""
        Dim temp_since_hot_string As String = ""
        Dim temp_time_between_string As String = ""
        Dim temp_tot_cycles_string As String = ""
        Dim temp_tot_cycles_over_string As String = ""
        Dim temp_tot_cycles_since_hot_string As String = ""

        htmlOutput = "<div class=""Box removeTopPadding"">"
        htmlOutput += "<table width='" & pdf_html_width & "' align='center' class=""formatTable large " & tableColor & " subtextNoMargin"">"

        Try
            Dim number_of_engines As Integer = 0

            If Not IsDBNull(out_AircraftRS.Rows(0).Item("amod_number_of_engines")) Then
                number_of_engines = out_AircraftRS.Rows(0).Item("amod_number_of_engines")
            End If
            If Not IsDBNull(out_AircraftRS.Rows(0).Item("ac_engine_name")) Then
                htmlOutput += "<tr class=""noBorder""><td colspan='2'><span class='" & HttpContext.Current.Session("FONT_CLASS_HEADER") & "  subHeader'>Engine(s): <strong>" & out_AircraftRS.Rows(0).Item("ac_engine_name").ToString.Trim & "</strong>" & IIf(showEditLink, CreateEditLink("engine", CrmSOURCE, aircraftID, "height=380,width=1150", False, False, ""), "") & "</span></td></tr>"
            Else
                htmlOutput += "<tr class=""noBorder""><td colspan='2'><span class='" & HttpContext.Current.Session("FONT_CLASS_HEADER") & "  subHeader'>Engine(s)" & CreateEditLink("engine", CrmSOURCE, aircraftID, "height=380,width=1150", False, False, "") & "</span></td></tr>"
            End If
            If Not IsDBNull(out_AircraftRS.Rows(0).Item("ac_engine_tbo_oc_flag")) Then
                If out_AircraftRS.Rows(0).Item("ac_engine_tbo_oc_flag").ToString.Trim.ToUpper.Contains("Y") Then
                    htmlOutput += comp_functions.create_value_with_label("On&nbsp;Condition&nbsp;TBO", "Yes", True, True, counter_For_PDf, spacer_width)
                Else
                    htmlOutput += comp_functions.create_value_with_label("On&nbsp;Condition&nbsp;TBO", "No", True, True, counter_For_PDf, spacer_width)
                End If
            End If

            If Not IsDBNull(out_AircraftRS.Rows(0).Item("emp_provider_name")) Then
                htmlOutput += comp_functions.create_value_with_label("Maintenance&nbsp;Program", out_AircraftRS.Rows(0).Item("emp_provider_name").ToString.Trim + "&nbsp;-&nbsp;" + out_AircraftRS.Rows(0).Item("emp_program_name").ToString.Trim + "&nbsp;", True, True, counter_For_PDf, spacer_width)
            End If

            If Not IsDBNull(out_AircraftRS.Rows(0).Item("ac_engine_noise_rating")) Then
                If out_AircraftRS.Rows(0).Item("ac_engine_noise_rating") > 0 Then
                    htmlOutput += comp_functions.create_value_with_label("Noise Rating", out_AircraftRS.Rows(0).Item("ac_engine_noise_rating").ToString.Trim, True, True, counter_For_PDf, spacer_width)
                End If
            End If

            If Not IsDBNull(out_AircraftRS.Rows(0).Item("ac_model_config")) Then
                If Not String.IsNullOrEmpty(out_AircraftRS.Rows(0).Item("ac_model_config")) Then
                    htmlOutput += comp_functions.create_value_with_label("Model Configuration", out_AircraftRS.Rows(0).Item("ac_model_config").ToString.Trim, True, True, counter_For_PDf, spacer_width)
                End If
            End If

            If Not IsDBNull(out_AircraftRS.Rows(0).Item("amod_airframe_type_code")) Then
                sAirframeType = out_AircraftRS.Rows(0).Item("amod_airframe_type_code").ToString.Trim.ToUpper
            End If
            If Not IsDBNull(out_AircraftRS.Rows(0).Item("amod_type_code")) Then
                sAircraftType = out_AircraftRS.Rows(0).Item("amod_type_code").ToString.Trim.ToUpper
            End If


            If sAirframeType <> "R" Then
                nloopCount = 4
            Else
                nloopCount = 3
            End If


            If number_of_engines < 3 Then

                htmlOutput &= CommonAircraftFunctions.DisplayEngineInfo_Vertical(MySesState, out_AircraftRS, Nothing, bShowBlankAcFields, from_spot)
            Else


                For xLoop = 1 To nloopCount

                    counter_For_PDf = counter_For_PDf + 1

                    If (Not IsDBNull(out_AircraftRS.Rows(0).Item("ac_engine_" + xLoop.ToString + "_tot_hrs")) Or
                        Not IsDBNull(out_AircraftRS.Rows(0).Item("ac_engine_" + xLoop.ToString + "_soh_hrs")) Or
                        Not IsDBNull(out_AircraftRS.Rows(0).Item("ac_engine_" + xLoop.ToString + "_shi_hrs")) Or
                        Not IsDBNull(out_AircraftRS.Rows(0).Item("ac_engine_" + xLoop.ToString + "_tbo_hrs")) Or
                        Not IsDBNull(out_AircraftRS.Rows(0).Item("ac_engine_" + xLoop.ToString + "_snew_cycles")) Or
                        Not IsDBNull(out_AircraftRS.Rows(0).Item("ac_engine_" + xLoop.ToString + "_soh_cycles")) Or
                        Not IsDBNull(out_AircraftRS.Rows(0).Item("ac_engine_" + xLoop.ToString + "_shs_cycles"))) Then

                        If xLoop = 1 And sAirframeType <> "R" Then
                            temp_start = "Eng&nbsp;" + xLoop.ToString + "&nbsp;(L):&nbsp;"
                        ElseIf xLoop = 2 And sAirframeType <> "R" Then
                            temp_start = "Eng&nbsp;" + xLoop.ToString + "&nbsp;(R):&nbsp;"
                        ElseIf xLoop = 3 And sAirframeType <> "R" Then
                            temp_start = "Eng&nbsp;" + xLoop.ToString + "&nbsp;(L):&nbsp;"
                        ElseIf xLoop = 4 And sAirframeType <> "R" Then
                            temp_start = "Eng&nbsp;" + xLoop.ToString + "&nbsp;(R):&nbsp;"
                        Else
                            temp_start = "Eng&nbsp;" + xLoop.ToString + ":&nbsp;"
                        End If


                        If Trim(temp_ser_string) <> "" Then
                            temp_ser_string &= " / "
                        End If
                        If Trim(out_AircraftRS.Rows(0).Item("ac_engine_" + xLoop.ToString + "_ser_no").ToString) <> "" Then
                            temp_ser_string &= temp_start & "&nbsp;" + out_AircraftRS.Rows(0).Item("ac_engine_" + xLoop.ToString + "_ser_no").ToString + ""
                        End If

                        If Trim(temp_ttsn_string) <> "" Then
                            temp_ttsn_string &= " / "
                        End If
                        If Not IsDBNull(out_AircraftRS.Rows(0).Item("ac_engine_" + xLoop.ToString + "_tot_hrs")) Then
                            temp_ttsn_string &= temp_start & "" + FormatNumber(CDbl(out_AircraftRS.Rows(0).Item("ac_engine_" + xLoop.ToString + "_tot_hrs").ToString), 0, True, False, True) + " hrs"
                        End If

                        If Trim(temp_since_over_string) <> "" Then
                            temp_since_over_string &= " / "
                        End If
                        If Not IsDBNull(out_AircraftRS.Rows(0).Item("ac_engine_" + xLoop.ToString + "_soh_hrs")) Then
                            temp_since_over_string &= temp_start & "" + FormatNumber(CDbl(out_AircraftRS.Rows(0).Item("ac_engine_" + xLoop.ToString + "_soh_hrs").ToString), 0, True, False, True) + " hrs"
                        End If

                        If Trim(temp_since_hot_string) <> "" Then
                            temp_since_hot_string &= " / "
                        End If
                        If Not IsDBNull(out_AircraftRS.Rows(0).Item("ac_engine_" + xLoop.ToString + "_shi_hrs")) Then
                            temp_since_hot_string &= temp_start & "" + FormatNumber(CDbl(out_AircraftRS.Rows(0).Item("ac_engine_" + xLoop.ToString + "_shi_hrs").ToString), 0, True, False, True) + " hrs"
                        End If

                        If Trim(temp_time_between_string) <> "" Then
                            temp_time_between_string &= " / "
                        End If
                        If Not IsDBNull(out_AircraftRS.Rows(0).Item("ac_engine_" + xLoop.ToString + "_tbo_hrs")) Then
                            temp_time_between_string &= temp_start & "" + FormatNumber(CDbl(out_AircraftRS.Rows(0).Item("ac_engine_" + xLoop.ToString + "_tbo_hrs").ToString), 0, True, False, True) + " hrs"
                        End If

                        If Trim(temp_tot_cycles_string) <> "" Then
                            temp_tot_cycles_string &= " / "
                        End If
                        If Not IsDBNull(out_AircraftRS.Rows(0).Item("ac_engine_" + xLoop.ToString + "_snew_cycles")) Then
                            temp_tot_cycles_string &= temp_start & "" + FormatNumber(CDbl(out_AircraftRS.Rows(0).Item("ac_engine_" + xLoop.ToString + "_snew_cycles").ToString), 0, True, False, True) ' + " hrs"
                        End If

                        If Trim(temp_tot_cycles_over_string) <> "" Then
                            temp_tot_cycles_over_string &= " / "
                        End If
                        If Not IsDBNull(out_AircraftRS.Rows(0).Item("ac_engine_" + xLoop.ToString + "_soh_cycles")) Then
                            temp_tot_cycles_over_string &= temp_start & "" + FormatNumber(CDbl(out_AircraftRS.Rows(0).Item("ac_engine_" + xLoop.ToString + "_soh_cycles").ToString), 0, True, False, True) '+ " hrs"
                        End If

                        If Trim(temp_tot_cycles_since_hot_string) <> "" Then
                            temp_tot_cycles_since_hot_string &= " / "
                        End If
                        If Not IsDBNull(out_AircraftRS.Rows(0).Item("ac_engine_" + xLoop.ToString + "_shs_cycles")) Then
                            temp_tot_cycles_since_hot_string &= temp_start & "" + FormatNumber(CDbl(out_AircraftRS.Rows(0).Item("ac_engine_" + xLoop.ToString + "_shs_cycles").ToString), 0, True, False, True) '+ " hrs"
                        End If

                    End If

                Next ' xLoop

                If Trim(temp_ser_string) <> "" Then
                    htmlOutput &= comp_functions.create_value_with_label("Ser No", temp_ser_string, True, True, counter_For_PDf, spacer_width)
                End If

                If Trim(temp_ttsn_string) <> "" Then
                    htmlOutput &= comp_functions.create_value_with_label("Time Since New", temp_ttsn_string, True, True, counter_For_PDf, spacer_width)
                End If

                If Trim(temp_since_over_string) <> "" Then
                    htmlOutput &= comp_functions.create_value_with_label("Since Overhaul", temp_since_over_string, True, True, counter_For_PDf, spacer_width)
                End If

                If Trim(temp_since_hot_string) <> "" Then
                    htmlOutput &= comp_functions.create_value_with_label("Since Hot Inspection", temp_since_hot_string, True, True, counter_For_PDf, spacer_width)
                End If

                If Trim(temp_time_between_string) <> "" Then
                    htmlOutput &= comp_functions.create_value_with_label("Time Between Overhaul", temp_time_between_string, True, True, counter_For_PDf, spacer_width)
                End If

                If Trim(temp_tot_cycles_string) <> "" Then
                    htmlOutput &= comp_functions.create_value_with_label("Cycles Since New", temp_tot_cycles_string, True, True, counter_For_PDf, spacer_width)
                End If


                If Trim(temp_tot_cycles_over_string) <> "" Then
                    htmlOutput &= comp_functions.create_value_with_label("Cycles Since Overhaul", temp_tot_cycles_over_string, True, True, counter_For_PDf, spacer_width)
                End If

                If Trim(temp_tot_cycles_since_hot_string) <> "" Then
                    htmlOutput &= comp_functions.create_value_with_label("Cycles Since Hot", temp_tot_cycles_since_hot_string, True, True, counter_For_PDf, spacer_width)
                End If

            End If

        Catch ex As Exception
            htmlOutput += "<tr><td colspan='2' align=""left""><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>No Details Reported. " & ex.Message.ToString & "</font></td></tr>" + vbCrLf
        End Try

        htmlOutput += "</table>"
        htmlOutput += "</div>"
        Return htmlOutput
    End Function


    Public Shared Function Build_Status_Block(ByVal nAircraftID As Long, ByVal nAircraftJournalID_Full As Long, ByVal journalTable As DataTable, ByVal out_AircraftRS As DataTable, ByVal bWordReport As Boolean, ByVal bAerodexFlag As Boolean, ByRef counter_For_PDf As Integer, ByVal word_width As String, ByVal pdf_html_width As String, ByVal tableColor As String, ByVal spacer_width As String, ByRef aclsData_Temp As clsData_Manager_SQL, ByRef addToAsking As TextBox, ByRef chkShowAsking As System.Web.UI.WebControls.CheckBox, ByVal chkBlindReport As System.Web.UI.WebControls.CheckBox, ByVal chkEB As System.Web.UI.WebControls.CheckBox, ByVal crmSource As String, ByVal ValueDescription As String, ByRef DOM As TextBox, ByVal jetnetNotForSale As Boolean, ByVal ClientNotForSale As Boolean, ByVal showEditLink As Boolean, ByVal transactionSource As String, Optional ByVal DisplayEvaluesInfo As Boolean = False, Optional ByRef EvalButtonWidth As Button = Nothing, Optional ByRef EvalExists As Boolean = False, Optional ByVal ToggleExclusiveLeasingOff As Boolean = False)
        Dim htmlOutPut As New StringBuilder
        Dim comp_functions As New CompanyFunctions
        Dim asking_price As String = ""
        Dim ac_status As String = ""
        Dim ac_status_text As String = ""
        Dim deliveryAC As String = ""
        Dim asking_type As String = ""
        Dim list_date As String = ""
        Dim confidential As String = ""
        Dim exclusive_flag As String = ""
        Dim days_on_market As String = ""
        Dim tcomp As String = ""
        Dim foreign_asking_price As String = ""
        Dim isAircraftTransactionForSale As Boolean = False
        Dim comp_id_list As String = ""
        Dim new_comp_id As Long = 0
        Dim del_is_date As Boolean = False

        If out_AircraftRS.Rows(0).Item("ac_forsale_flag").ToString.ToUpper = "Y" Or (transactionSource = "CLIENT" And nAircraftJournalID_Full > 0 And Not IsDBNull(out_AircraftRS.Rows(0).Item("ac_asking"))) Then
            isAircraftTransactionForSale = True
        End If

        '  If Not bAerodexFlag Then
        counter_For_PDf = counter_For_PDf + 2

        If Not bAerodexFlag Then
            If chkShowAsking.Checked = True Then

                If Not IsDBNull(out_AircraftRS.Rows(0).Item("ac_foreign_currency_price")) Then
                    foreign_asking_price = comp_functions.create_value_with_label("Asking Amt (" + out_AircraftRS.Rows(0).Item("ac_foreign_currency_name").ToString & ")", FormatCurrency(CDbl(out_AircraftRS.Rows(0).Item("ac_foreign_currency_price").ToString), 0), True, True, counter_For_PDf, spacer_width)
                End If

                If Not IsDBNull(out_AircraftRS.Rows(0).Item("ac_status")) Then
                    ac_status = out_AircraftRS.Rows(0).Item("ac_status").ToString.ToUpper
                    If ac_status.ToUpper.Contains("FOR SALE/TRADE") And out_AircraftRS.Rows(0).Item("ac_asking").ToString.ToUpper.Contains("SALE/TRADE") Then
                        ac_status = ""
                    Else
                        ac_status = "<font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT_TITLE") & "'>Status: </font>"
                        ac_status_text = UCase(out_AircraftRS.Rows(0).Item("ac_status").ToString)
                    End If
                End If

                If isAircraftTransactionForSale = True Then
                    If Not IsDBNull(out_AircraftRS.Rows(0).Item("ac_delivery")) And Not String.IsNullOrEmpty(out_AircraftRS.Rows(0).Item("ac_delivery").ToString) Then
                        If out_AircraftRS.Rows(0).Item("ac_delivery").ToString.ToLower.Contains("date") Then
                            If Not IsDBNull(out_AircraftRS.Rows(0).Item("ac_delivery_date")) And Not String.IsNullOrEmpty(out_AircraftRS.Rows(0).Item("ac_delivery_date").ToString) Then
                                deliveryAC = " " & FormatDateTime(out_AircraftRS.Rows(0).Item("ac_delivery_date").ToString, DateFormat.ShortDate)
                                del_is_date = True
                            End If
                        Else
                            deliveryAC = " " & UCase(out_AircraftRS.Rows(0).Item("ac_delivery").ToString)
                        End If
                    End If

                    If Not IsDBNull(out_AircraftRS.Rows(0).Item("ac_asking")) Then

                        asking_type = ""

                        If (out_AircraftRS.Rows(0).Item("ac_asking").ToString.ToUpper.Contains("PRICE")) Then
                            asking_type += UCase(out_AircraftRS.Rows(0).Item("ac_asking").ToString)

                            If Not IsDBNull(out_AircraftRS.Rows(0).Item("ac_asking_price")) Then
                                asking_price = Trim(asking_price)
                                asking_price += ", ASKING: "
                                If Not String.IsNullOrEmpty(addToAsking.Text) And IsNumeric(addToAsking.Text) Then
                                    asking_price += FormatCurrency(((CDbl(out_AircraftRS.Rows(0).Item("ac_asking_price").ToString) + CDbl(addToAsking.Text)) / 1000), 0) & "k"
                                Else
                                    asking_price += FormatCurrency((CDbl(out_AircraftRS.Rows(0).Item("ac_asking_price").ToString) / 1000), 0) & "k"
                                End If
                            End If

                            asking_type = ""
                        ElseIf Not IsNothing(addToAsking) And Trim(addToAsking.Text) <> "" Then
                            If IsNumeric(addToAsking.Text) Then
                                asking_price += ", ASKING: "
                                asking_price += FormatCurrency((CDbl(addToAsking.Text) / 1000), 0) & "k"
                                asking_type = ""
                            Else
                                asking_type += UCase(out_AircraftRS.Rows(0).Item("ac_asking").ToString)
                            End If
                        Else
                            asking_type += UCase(out_AircraftRS.Rows(0).Item("ac_asking").ToString)
                        End If

                    Else

                        If Not IsDBNull(out_AircraftRS.Rows(0).Item("ac_asking_price")) Then
                            asking_price += Trim(asking_price)
                            asking_price += ", ASKING: "
                            If Not String.IsNullOrEmpty(addToAsking.Text) And IsNumeric(addToAsking.Text) Then
                                asking_price += FormatCurrency(((CDbl(out_AircraftRS.Rows(0).Item("ac_asking_price").ToString) + CDbl(addToAsking.Text)) / 1000), 0) & "k"
                            Else
                                asking_price += FormatCurrency((CDbl(out_AircraftRS.Rows(0).Item("ac_asking_price").ToString) / 1000), 0) & "k"
                            End If
                        End If

                    End If
                End If '1=1
            End If
        End If

        If bWordReport = True Then
            htmlOutPut.Append("<div class=""Box  removeTopPadding""><table width='" & word_width & "' align='center'>")
        Else
            htmlOutPut.Append("<div class=""Box removeTopPadding""><table width='" & pdf_html_width & "' align='center' class=""formatTable large " & tableColor & " subtextNoMargin"">")
        End If

        If Not bAerodexFlag Then

            If del_is_date = True Then
                If Trim(asking_type) = "" Then
                    htmlOutPut.Append("<tr class=""noBorder""><td colspan='2'><font class='" & HttpContext.Current.Session("FONT_CLASS_HEADER") & " subHeader'>STATUS  <strong class=""" & IIf(isAircraftTransactionForSale = True, "greenText bold", "") & """>" & ac_status_text & " " & asking_type & asking_price & ", Delivery: " & deliveryAC & "<span class=""float_right"">" & IIf(showEditLink, CreateEditLink("", crmSource, nAircraftID, "", True, False, ""), "") & "</span></strong></font></td></tr>")
                Else
                    htmlOutPut.Append("<tr class=""noBorder""><td colspan='2'><font class='" & HttpContext.Current.Session("FONT_CLASS_HEADER") & " subHeader'>STATUS  <strong class=""" & IIf(isAircraftTransactionForSale = True, "greenText bold", "") & """>" & ac_status_text & ", " & asking_type & asking_price & ", Delivery: " & deliveryAC & "<span class=""float_right"">" & IIf(showEditLink, CreateEditLink("", crmSource, nAircraftID, "", True, False, ""), "") & "</span></strong></font></td></tr>")
                End If
            Else
                If Trim(asking_type) = "" Then
                    htmlOutPut.Append("<tr class=""noBorder""><td colspan='2'><font class='" & HttpContext.Current.Session("FONT_CLASS_HEADER") & " subHeader'>STATUS  <strong class=""" & IIf(isAircraftTransactionForSale = True, "greenText bold", "") & """>" & ac_status_text & " " & asking_type & asking_price & " " & IIf(Not String.IsNullOrEmpty(deliveryAC), ", " & deliveryAC, "") & "<span class=""float_right"">" & IIf(showEditLink, CreateEditLink("", crmSource, nAircraftID, "", True, False, ""), "") & "</span></strong></font></td></tr>")
                Else
                    htmlOutPut.Append("<tr class=""noBorder""><td colspan='2'><font class='" & HttpContext.Current.Session("FONT_CLASS_HEADER") & " subHeader'>STATUS  <strong class=""" & IIf(isAircraftTransactionForSale = True, "greenText bold", "") & """>" & ac_status_text & ", " & asking_type & asking_price & " " & IIf(Not String.IsNullOrEmpty(deliveryAC), ", " & deliveryAC, "") & "<span class=""float_right"">" & IIf(showEditLink, CreateEditLink("", crmSource, nAircraftID, "", True, False, ""), "") & "</span></strong></font></td></tr>")
                End If
            End If


            If foreign_asking_price <> "" Then
                htmlOutPut.Append(foreign_asking_price)
            End If

            If isAircraftTransactionForSale = True Then
                tcomp = ""
                '''''' This is added for the AssetInsight Page in order to display the information of this block without the tabs below.
                Dim d_exclusiveDate As String = ""
                If ToggleExclusiveLeasingOff = False Then
                    tcomp = CommonAircraftFunctions.GetExclusive(CLng(out_AircraftRS.Rows(0).Item("ac_id").ToString), CLng(out_AircraftRS.Rows(0).Item("ac_journ_id").ToString), aclsData_Temp, "JETNET", False, comp_id_list)
                    If Trim(tcomp) <> "" Then
                        d_exclusiveDate = CommonAircraftFunctions.GetExclusiveDate(CLng(out_AircraftRS.Rows(0).Item("ac_id").ToString), CLng(out_AircraftRS.Rows(0).Item("ac_journ_id").ToString), aclsData_Temp)
                        If chkBlindReport.Checked Or chkEB.Checked Then
                        Else
                            If Not String.IsNullOrEmpty(d_exclusiveDate) Then
                                exclusive_flag = comp_functions.create_value_with_label("<span class=""purple_text"">On Exclusive</span>", "<span class=""purple_text"">" & CommonAircraftFunctions.GetExclusive(CLng(out_AircraftRS.Rows(0).Item("ac_id").ToString), CLng(out_AircraftRS.Rows(0).Item("ac_journ_id").ToString), aclsData_Temp, "JETNET", False) + "</em> as of " & d_exclusiveDate & "</span>", True, True, counter_For_PDf, spacer_width)
                            Else
                                exclusive_flag = comp_functions.create_value_with_label("<span class=""purple_text"">On Exclusive</span>", "<span class=""purple_text"">With " & tcomp & "</span>", True, True, counter_For_PDf, spacer_width)
                            End If
                        End If
                    End If


                    If Trim(tcomp) <> "" Then
                        If chkBlindReport.Checked Or chkEB.Checked Then
                        Else
                            Dim temp_Comp_name As String = ""
                            temp_Comp_name = Trim(CommonAircraftFunctions.GetExclusive(CLng(out_AircraftRS.Rows(0).Item("ac_id").ToString), CLng(out_AircraftRS.Rows(0).Item("ac_journ_id").ToString), aclsData_Temp, "JETNET", False, comp_id_list, new_comp_id))

                            If Trim(temp_Comp_name) <> "" Then
                                'd_exclusiveDate = GetExclusiveDate(CLng(out_AircraftRS.Rows(0).Item("ac_id").ToString), CLng(out_AircraftRS.Rows(0).Item("ac_journ_id").ToString), aclsData_Temp, new_comp_id)
                                ' cant get exclusive date of co-exclusive currently, note types do not go out to evo 
                                d_exclusiveDate = ""
                                exclusive_flag = exclusive_flag & comp_functions.create_value_with_label("<span class=""purple_text"">On Exclusive</span>", "<span class=""purple_text"">" & temp_Comp_name & "</em></span>", True, True, counter_For_PDf, spacer_width)

                            End If
                        End If
                    End If


                End If

                If Not IsDBNull(out_AircraftRS.Rows(0).Item("ac_list_date")) Then
                    days_on_market = ""
                    If nAircraftJournalID_Full > 0 Then
                        days_on_market = DateDiff("d", CDate(out_AircraftRS.Rows(0).Item("ac_list_date").ToString), journalTable.Rows(0).Item("journ_date").ToString).ToString
                    Else
                        days_on_market = DateDiff("d", CDate(out_AircraftRS.Rows(0).Item("ac_list_date").ToString), Now()).ToString
                        DOM.Text = DateDiff("d", CDate(out_AircraftRS.Rows(0).Item("ac_list_date").ToString.Trim), Today()).ToString
                    End If


                    If Trim(days_on_market) <> "" Then
                        list_date = comp_functions.create_value_with_label("Listed On", FormatDateTime(out_AircraftRS.Rows(0).Item("ac_list_date").ToString, DateFormat.ShortDate) & " Days on Market (" & days_on_market & ")", True, True, counter_For_PDf, spacer_width)
                    Else
                        list_date = comp_functions.create_value_with_label("Listed On", FormatDateTime(out_AircraftRS.Rows(0).Item("ac_list_date").ToString, DateFormat.ShortDate) & IIf(nAircraftJournalID_Full = 0, " Days on Market (" & days_on_market & ")", ""), True, True, counter_For_PDf, spacer_width)
                    End If

                End If

                If Not IsDBNull(out_AircraftRS.Rows(0).Item("take_price")) Then
                    If out_AircraftRS.Rows(0).Item("take_price") > 0 Then
                        htmlOutPut.Append(comp_functions.create_value_with_label("Take Price", FormatCurrency((CDbl(out_AircraftRS.Rows(0).Item("take_price").ToString) / 1000), 0) & "k", True, True, counter_For_PDf, spacer_width))
                    End If
                End If

                If Not IsDBNull(out_AircraftRS.Rows(0).Item("broker_price")) Then
                    If out_AircraftRS.Rows(0).Item("broker_price") > 0 Then
                        htmlOutPut.Append(comp_functions.create_value_with_label("Broker Price", FormatCurrency((CDbl(out_AircraftRS.Rows(0).Item("broker_price").ToString) / 1000), 0) & "k", True, True, counter_For_PDf, spacer_width))
                    End If
                End If
            End If

            If transactionSource <> "CLIENT" Then
                If HttpContext.Current.Session.Item("localSubscription").crmSalesPriceIndex_Flag = True And CLng(out_AircraftRS.Rows(0).Item("ac_journ_id").ToString) > 0 Then

                    If Not IsDBNull(out_AircraftRS.Rows(0).Item("ac_sale_price_display_flag")) And Trim(out_AircraftRS.Rows(0).Item("ac_sale_price_display_flag")) = "Y" Then

                        If Not IsDBNull(out_AircraftRS.Rows(0).Item("ac_sale_price")) Then
                            If Not IsDBNull(out_AircraftRS.Rows(0).Item("ac_sale_price")) Then
                                If CDbl(out_AircraftRS.Rows(0).Item("ac_sale_price").ToString) > 0 Then
                                    htmlOutPut.Append(comp_functions.create_value_with_label("Sale Price", DisplayFunctions.TextToImage("$" & FormatNumber((out_AircraftRS.Rows(0).Item("ac_sale_price").ToString / 1000), 0) & "k", 12, "", "42", "Displayable Sale Price", "bottom", True), True, True, counter_For_PDf, spacer_width))

                                End If
                            End If
                        End If
                    End If
                End If
            ElseIf transactionSource = "CLIENT" Then
                If Not IsDBNull(out_AircraftRS.Rows(0).Item("ac_sale_price")) Then
                    If Not IsDBNull(out_AircraftRS.Rows(0).Item("ac_sale_price")) Then
                        If CDbl(out_AircraftRS.Rows(0).Item("ac_sale_price").ToString) > 0 Then
                            htmlOutPut.Append(comp_functions.create_value_with_label("Sale Price", DisplayFunctions.TextToImage("$" & FormatNumber((out_AircraftRS.Rows(0).Item("ac_sale_price").ToString / 1000), 0) & "k", 12, "", "42", "Displayable Sale Price", "bottom", True), True, True, counter_For_PDf, spacer_width))

                        End If
                    End If
                End If
            End If

            htmlOutPut.Append(list_date)


            If Trim(confidential) <> "" Then
                htmlOutPut.Append(confidential)
                counter_For_PDf = counter_For_PDf + 1
            End If
        Else
            htmlOutPut.Append("<tr class=""noBorder""><td colspan='2'><font class='" & HttpContext.Current.Session("FONT_CLASS_HEADER") & " subHeader'>STATUS  <strong class="""">" & deliveryAC & "<span class=""float_right"">" & IIf(showEditLink, CreateEditLink("", crmSource, nAircraftID, "", True, False, ""), "") & "</span></strong></font></td></tr>")
        End If

        '''''' This is added for the AssetInsight Page in order to display the information of this block without the tabs below.
        If ToggleExclusiveLeasingOff = False Then
            If Trim(exclusive_flag) <> "" Then
                htmlOutPut.Append(exclusive_flag)
                counter_For_PDf = counter_For_PDf + 1
            End If

            If Not IsDBNull(out_AircraftRS.Rows(0).Item("ac_ownership_type")) Then
                htmlOutPut.Append(comp_functions.create_value_with_label("Ownership", CommonAircraftFunctions.GetOwnershipType(out_AircraftRS.Rows(0).Item("ac_ownership_type"), aclsData_Temp) & IIf(out_AircraftRS.Rows(0).Item("ac_previously_owned_flag").ToString.ToUpper = "Y", " / Previously Owned", " / Not Previously Owned"), True, False, counter_For_PDf, spacer_width))
            Else
                htmlOutPut.Append(comp_functions.create_value_with_label("Ownership", IIf(out_AircraftRS.Rows(0).Item("ac_previously_owned_flag").ToString.ToUpper = "Y", " / Previously Owned", " / Not Previously Owned"), True, False, counter_For_PDf, spacer_width))
            End If

            If Not IsDBNull(out_AircraftRS.Rows(0).Item("ac_lifecycle_stage")) Then
                If crmSource <> "CLIENT" Then
                    htmlOutPut.Append(comp_functions.create_value_with_label("Lifecycle/Use", CommonAircraftFunctions.GetLifeCycleStage(out_AircraftRS.Rows(0).Item("ac_lifecycle_stage"), aclsData_Temp) & IIf(Not IsDBNull(out_AircraftRS.Rows(0).Item("acuse_name")), " / " & out_AircraftRS.Rows(0).Item("acuse_name"), ""), True, False, counter_For_PDf, spacer_width))
                Else
                    htmlOutPut.Append(comp_functions.create_value_with_label("Lifecycle/Use", CommonAircraftFunctions.GetLifeCycleStage(out_AircraftRS.Rows(0).Item("ac_lifecycle_stage"), aclsData_Temp), True, False, counter_For_PDf, spacer_width))

                End If
            End If
        End If
        '''''''''''''''''''''





        If Not IsDBNull(out_AircraftRS.Rows(0).Item("ac_confidential_notes")) Then
            If Not String.IsNullOrEmpty(out_AircraftRS.Rows(0).Item("ac_confidential_notes").ToString.Trim) Then
                htmlOutPut.Append(comp_functions.create_value_with_label("Note", out_AircraftRS.Rows(0).Item("ac_confidential_notes").ToString.Trim, True, True, counter_For_PDf, spacer_width))
            End If
        End If

        If ValueDescription <> "" Then
            htmlOutPut.Append("<tr><td colspan=""2"" valign=""top"" align=""left"">" & ValueDescription & "</td></tr>")
        End If

        If Not IsDBNull(out_AircraftRS.Rows(0).Item("ac_forsale_flag")) Then
            If out_AircraftRS.Rows(0).Item("ac_forsale_flag") = "Y" Then 'The record you're looking at is for sale
                If crmSource = "CLIENT" Then
                    If jetnetNotForSale = True Then
                        htmlOutPut.Append("<tr><td colspan=""2"" valign=""top"" align=""left"">" & CreateASlideout("JETNET shows this aircraft as offmarket. To remove this aircraft from the market, click on the pencil icon to edit and remove the client aircraft.") & "</td></tr>")
                    End If
                ElseIf crmSource <> "CLIENT" Then
                    If ClientNotForSale = True Then
                        htmlOutPut.Append("<tr><td colspan=""2"" valign=""top"" align=""left"">" & CreateASlideout("Your client record shows this aircraft as Off Market. If this aircraft is really on market, then click on the pencil icon to edit and remove the client aircraft.") & "</td></tr>")
                    End If
                End If
            End If
        End If

        'This was added so that the evalues info could display in the status block.
        If DisplayEvaluesInfo = True Then
            Dim searchCriteria As New viewSelectionCriteriaClass
            Dim EvaluesText As String = "</table></div>" 'This should not be. I need to be able to check out utilization view functions so I can change the function to not use a replace anymore.
            Dim Utilization_Functions As New utilization_view_functions
            searchCriteria.ViewCriteriaAircraftID = nAircraftID

            Utilization_Functions.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
            Utilization_Functions.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
            Utilization_Functions.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
            Utilization_Functions.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
            Utilization_Functions.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim
            Call Utilization_Functions.views_display_evalues_in_status_block(searchCriteria, 0, New Button, EvaluesText, EvalExists, IIf(Not IsDBNull(out_AircraftRS.Rows(0).Item("ac_year")), out_AircraftRS.Rows(0).Item("ac_year"), ""), out_AircraftRS.Rows(0).Item("amod_make_name").ToString & " " & out_AircraftRS.Rows(0).Item("amod_model_name").ToString, out_AircraftRS.Rows(0).Item("jetnet_amod_id"), 0, nAircraftID)

            htmlOutPut.Append(EvaluesText)
        Else 'We only need the table ending if evalues is off for right now until the above comment is fixed.
            htmlOutPut.Append("</table>")
            htmlOutPut.Append("</div>")
        End If




        ' End If






        Return htmlOutPut.ToString
    End Function

    Public Shared Function Build_ValuesBlock(ByVal localDataLayer As viewsDataLayer, ByVal valChart As System.Web.UI.DataVisualization.Charting.Chart, ByVal jetnetAC_ID As Long, ByVal ClientID As Long, ByVal pageRef As System.Web.UI.Page, ByVal tempPanel As System.Web.UI.UpdatePanel, ByVal crmSource As String, Optional ByVal AircraftModel As Long = 0, Optional ByRef page1 As Page = Nothing, Optional ByVal ac_dlv_year As Integer = 0, Optional ByRef google_string As String = "", Optional ByVal from_spot As String = "") As String
        Dim exists_data As Boolean = False
        Dim google_map_array_list As String = ""
        Dim returnString As String = ""
        Dim graph1 As String = ""
        Dim graph2 As String = ""
        Dim temp_count As String = "0"
        Dim graph30_ticks_string As String = ""
        Dim has_info As Boolean = False
        Dim has_info2 As Boolean = False
        'If ClientID > 0 Then

        If clsGeneral.clsGeneral.isEValuesAvailable() = True And clsGeneral.clsGeneral.isShowingEvalues() = True Then

            Dim utilization_functions As New utilization_view_functions

            utilization_functions.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
            utilization_functions.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
            utilization_functions.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
            utilization_functions.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
            utilization_functions.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim

            Call utilization_functions.FillAssettInsightGraphs("ASKSOLD", 0, returnString, tempPanel, 1, jetnetAC_ID, ClientID, 400, 0, True, True, True, "", "A", "", "", "", "", "", "", "", "", "", graph1, "", False, True, has_info)

            Call utilization_functions.FillAssettInsightGraphs("RESIDUALAC", AircraftModel, "", tempPanel, 2, jetnetAC_ID, ClientID, 400, ac_dlv_year, True, True, True, "", "Y", "", temp_count, graph30_ticks_string, "", "", "", "", "", "", graph2, "", False, True, has_info2)

            If Trim(graph1) <> "" Or Trim(graph2) <> "" Then

                DisplayFunctions.load_google_chart(New AjaxControlToolkit.TabPanel, graph1, "", "Aircraft Value ($k)", "chart_div_value_history1_all", 465, 250, "POINTS", 1, google_map_array_list, pageRef, tempPanel, False, False, True, False, False, False, False, False, False, False, 0, "bottom", "", False)

                DisplayFunctions.load_google_chart(New AjaxControlToolkit.TabPanel, graph2, "", "Aircraft Value ($k)", "chart_div_value_history2_all", 465, 250, "POINTS", 2, google_map_array_list, pageRef, tempPanel, False, False, True, False, False, False, False, False, False, False, 0, "bottom", "", False, CInt(temp_count), graph30_ticks_string)

                google_string = google_map_array_list

                DisplayFunctions.load_google_chart_all(google_map_array_list, page1, tempPanel)
                ' ADDED PREVIOUS <br>" & returnString & " in at end of line 
                'If crmSource = "CLIENT" Then
                '  returnString = ("<div class=""Box""><table border = '0' width='100%' class='formatTable blue large valuesTable'><tr class='noBorder'><td colspan='5'><div class='subHeader'>VALUATION/HISTORY" & IIf(crmSource = "CLIENT", "<span class=""float_right""><a href=""#"" onclick=""javascript:window.open('edit_note.aspx?ac_ID=" & ClientID.ToString & "&action=new&amp;type=value_analysis&amp;cat_key=17&source=CLIENT&from=aircraftDetails','unloaded_me','scrollbars=yes,menubar=no,height=435,width=860,resizable=yes,toolbar=no,location=no,status=no');"">+ADD CLIENT ESTIMATE</a></span>", "") & "</div></td></tr><tr class='noBorder'><td colspan='5'><div id=""chart_div_value_history""></div></td></tr></table><br>" & returnString & "</div>")
                'Else
                '  returnString = ("<div class=""Box""><table border = '0' width='100%' class='formatTable blue large valuesTable'><tr class='noBorder'><td colspan='5'><div class='subHeader'>VALUATION/HISTORY<span class=""float_right""><a href=""#"" onclick=""javascript:window.open('edit_note.aspx?ac_ID=" & jetnetAC_ID.ToString & "&action=new&amp;type=value_analysis&amp;cat_key=17&source=JETNET&from=aircraftDetails','unloaded_me','scrollbars=yes,menubar=no,height=435,width=860,resizable=yes,toolbar=no,location=no,status=no');"">+ADD CLIENT ESTIMATE</a></span></div></td></tr><tr class='noBorder'><td colspan='5'><div id=""chart_div_value_history""></div></td></tr></table><br>" & returnString & "</div>")
                'End If
                If has_info = False And has_info2 = False Then
                    If crmSource = "CLIENT" Then
                        returnString = ("<div class=""Box""><table border = '0' width='100%' class='formatTable blue large valuesTable'><tr class='noBorder'><td colspan='5'><div class='subHeader'>VALUATION/HISTORY" & IIf(crmSource = "CLIENT", "<span class=""float_right"">&nbsp;</span><span class=""float_right""><a href=""#"" onclick=""javascript:window.open('edit_note.aspx?ac_ID=" & ClientID.ToString & "&action=new&amp;type=value_analysis&amp;cat_key=17&source=CLIENT&from=aircraftDetails','unloaded_me','scrollbars=yes,menubar=no,height=435,width=860,resizable=yes,toolbar=no,location=no,status=no');"">+CLIENT ESTIMATE</a></span>", "") & "</div></td></tr><tr class='noBorder'><td colspan='5' align='left'>No Value History Available</td></tr></table>") & "</div>"
                    Else
                        returnString = ("<div class=""Box""><table border = '0' width='100%' class='formatTable blue large valuesTable'><tr class='noBorder'><td colspan='5'><div class='subHeader'>VALUATION/HISTORY<span class=""float_right""><a href=""#"" onclick=""javascript:window.open('edit_note.aspx?ac_ID=" & jetnetAC_ID.ToString & "&action=new&amp;type=value_analysis&amp;cat_key=17&source=JETNET&from=aircraftDetails','unloaded_me','scrollbars=yes,menubar=no,height=435,width=860,resizable=yes,toolbar=no,location=no,status=no');"">+CLIENT ESTIMATE</a></span></div></td></tr><tr class='noBorder'><td colspan='5' align='left'>No Value History Available</td></tr></td></tr></table>") & "</div>"
                    End If
                Else
                    If crmSource = "CLIENT" Then
                        returnString = ("<div class=""Box"">" & Replace(returnString, " border='1' cellpadding='3' cellspacing='0' class='engine'>", " border = '0' width='100%' class='formatTable blue large valuesTable'><tr class='noBorder'><td colspan='5'><div class='subHeader'>VALUATION/HISTORY" & IIf(crmSource = "CLIENT", "<span class=""float_right"">&nbsp;</span><span class=""float_right""><a href=""#""  title='Expand Graph to Full Page' alt='Expand Graph to Full Page' onclick=""javascript:load('largeGraphDisplay.aspx?ac_id=" & jetnetAC_ID & "&Client_AC_ID=" & ClientID & "&source=CLIENT&ac_dlv_year=" & ac_dlv_year & "&graph_type=ACESTIMATES&page_title=VALUE HISTORY/PROJECTIONS','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');return false;"">EXPAND GRAPH</a></span><span class=""float_right"">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span><span class=""float_right""><a href=""#"" onclick=""javascript:window.open('edit_note.aspx?ac_ID=" & ClientID.ToString & "&action=new&amp;type=value_analysis&amp;cat_key=17&source=CLIENT&from=aircraftDetails','unloaded_me','scrollbars=yes,menubar=no,height=435,width=860,resizable=yes,toolbar=no,location=no,status=no');"">+CLIENT ESTIMATE</a></span>", "") & "</div></td></tr><tr class='noBorder'><td colspan='5'><div id=""chart_div_value_history1_all""></div></td></tr></table>") & "</div>")
                        If Trim(graph2) <> "" Then
                            returnString &= ("<div class=""Box""><table border='0' width='100%' class='formatTable blue large valuesTable'><tr class='noBorder'><td colspan='5'><div class='subHeader'>RESIDUAL VALUE</span><span class=""float_right"">&nbsp;</span><span class=""float_right""><a href=""#"" onclick=""javascript:load('largeGraphDisplay.aspx?amod_id=" & AircraftModel & "&ac_dlv_year=" & ac_dlv_year & "&ac_id=" & jetnetAC_ID & "&source=CLIENT&Client_AC_ID=" & ClientID & "&graph_type=RESIDUALAC&page_title=RESIDUAL VALUES','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');return false;"">EXPAND GRAPH</a></div><div id=""chart_div_value_history2_all""></div></td></tr></table></div>")
                        End If
                    Else

                        If Trim(from_spot) = "pdf" Then
                            returnString = ("<div class=""Box"">" & Replace(returnString, " border='1' cellpadding='3' cellspacing='0' class='engine'>", " border = '0' width='100%' class='formatTable blue large valuesTable'><tr class='noBorder'><td colspan='5'><div class='subHeader'>VALUATION/HISTORY</span></div></td></tr>") & "</div>")
                            If Trim(graph2) <> "" Then
                                returnString &= ("<div class=""Box""><table border='0' width='100%' class='formatTable blue large valuesTable'><tr class='noBorder'><td colspan='5'><div class='subHeader'>RESIDUAL VALUE</span></div></td></tr></table>")
                            End If
                        Else
                            returnString = ("<div class=""Box"">" & Replace(returnString, " border='1' cellpadding='3' cellspacing='0' class='engine'>", " border = '0' width='100%' class='formatTable blue large valuesTable'><tr class='noBorder'><td colspan='5'><div class='subHeader'>VALUATION/HISTORY<span class=""float_right"">&nbsp;</span><span class=""float_right""><a href=""#"" onclick=""javascript:window.open('edit_note.aspx?ac_ID=" & jetnetAC_ID.ToString & "&action=new&amp;type=value_analysis&amp;cat_key=17&source=JETNET&from=aircraftDetails','unloaded_me','scrollbars=yes,menubar=no,height=435,width=860,resizable=yes,toolbar=no,location=no,status=no');"">+CLIENT ESTIMATE</a></span><span class=""float_right"">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span><span class=""float_right""><a href=""#"" title='Expand Graph to Full Page' alt='Expand Graph to Full Page' onclick=""javascript:load('largeGraphDisplay.aspx?ac_id=" & jetnetAC_ID & "&Client_AC_ID=0&source=JETNET&ac_dlv_year=" & ac_dlv_year & "&graph_type=ACESTIMATES&page_title=VALUE HISTORY/PROJECTIONS','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');return false;"">EXPAND GRAPH</a></span></div></td></tr><tr class='noBorder'><td colspan='5'><div id=""chart_div_value_history1_all""></div></td></tr>") & "</div>")
                            If Trim(graph2) <> "" Then
                                returnString &= ("<div class=""Box""><table border='0' width='100%' class='formatTable blue large valuesTable'><tr class='noBorder'><td colspan='5'><div class='subHeader'>RESIDUAL VALUE</span><span class=""float_right"">&nbsp;</span><span class=""float_right""><a href=""#"" onclick=""javascript:load('largeGraphDisplay.aspx?amod_id=" & AircraftModel & "&ac_dlv_year=" & ac_dlv_year & "&ac_id=" & jetnetAC_ID & "&Client_AC_ID=0&graph_type=RESIDUALAC&page_title=RESIDUAL VALUES','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');return false;"">EXPAND GRAPH</a></div><div id=""chart_div_value_history2_all""></div></td></tr></table></div>")
                            End If
                        End If
                    End If
                End If


                returnString = Replace(returnString, "<td align='left'><font size='-2' style='font-family: Arial' nowrap='nowrap'><b>Take $</b>", "<td align='left' nowrap='nowrap'><font size='-2' style='font-family: Arial' nowrap='nowrap'><b>Take $</b>")
            Else
                If crmSource = "CLIENT" Then
                    returnString = "<div class=""Box""><table border = '0' width='100%' class='formatTable blue large valuesTable'><tr class='noBorder'><td colspan='5'><div class='subHeader'>VALUATION/HISTORY<span class=""float_right""><a href=""#"" onclick=""javascript:window.open('edit_note.aspx?ac_ID=" & ClientID.ToString & "&action=new&amp;type=value_analysis&amp;cat_key=17&source=CLIENT&from=aircraftDetails','unloaded_me','scrollbars=yes,menubar=no,height=435,width=860,resizable=yes,toolbar=no,location=no,status=no');"">+ADD CLIENT ESTIMATE</a></span></div></td></tr><tr class='noBorder'><td colspan='5'>No Value History Available</td></tr></table></div>"
                Else
                    returnString = "<div class=""Box""><table border = '0' width='100%' class='formatTable blue large valuesTable'><tr class='noBorder'><td colspan='5'><div class='subHeader'>VALUATION/HISTORY<span class=""float_right""><a href=""#"" onclick=""javascript:window.open('edit_note.aspx?ac_ID=" & jetnetAC_ID.ToString & "&action=new&amp;type=value_analysis&amp;cat_key=17&source=JETNET&from=aircraftDetails','unloaded_me','scrollbars=yes,menubar=no,height=435,width=860,resizable=yes,toolbar=no,location=no,status=no');"">+ADD CLIENT ESTIMATE</a></span></div></td></tr><tr class='noBorder'><td colspan='5'>No Value History Available</td></tr></table></div>"
                End If
            End If
        Else
            localDataLayer.views_analytics_graph_1(ClientID, valChart, returnString, jetnetAC_ID, google_map_array_list, "O", 0, exists_data)

            If exists_data = True Then
                DisplayFunctions.load_google_chart(New AjaxControlToolkit.TabPanel, google_map_array_list, "", "Aircraft Value ($k)", "chart_div_value_history", 530, 227, "POINTS", 1, "", pageRef, tempPanel, False, False, True, False, False, False, False, False, False, False, 0, "bottom", "", False)

                If crmSource = "CLIENT" Then
                    returnString = ("<div class=""Box"">" & Replace(returnString, " border='1' cellpadding='3' cellspacing='0' class='engine'>", " border = '0' width='100%' class='formatTable blue large valuesTable'><tr class='noBorder'><td colspan='5'><div class='subHeader'>VALUE HISTORY" & IIf(crmSource = "CLIENT", "<span class=""float_right""><a href=""#"" onclick=""javascript:window.open('edit_note.aspx?ac_ID=" & ClientID.ToString & "&action=new&amp;type=value_analysis&amp;cat_key=17&source=CLIENT&from=aircraftDetails','unloaded_me','scrollbars=yes,menubar=no,height=435,width=860,resizable=yes,toolbar=no,location=no,status=no');"">+ADD VALUE ESTIMATE</a></span>", "") & "</div></td></tr><tr class='noBorder'><td colspan='5'><div id=""chart_div_value_history""></div></td></tr></table>") & "</div>")
                Else
                    returnString = ("<div class=""Box"">" & Replace(returnString, " border='1' cellpadding='3' cellspacing='0' class='engine'>", " border = '0' width='100%' class='formatTable blue large valuesTable'><tr class='noBorder'><td colspan='5'><div class='subHeader'>VALUE HISTORY<span class=""float_right""><a href=""#"" onclick=""javascript:window.open('edit_note.aspx?ac_ID=" & jetnetAC_ID.ToString & "&action=new&amp;type=value_analysis&amp;cat_key=17&source=JETNET&from=aircraftDetails','unloaded_me','scrollbars=yes,menubar=no,height=435,width=860,resizable=yes,toolbar=no,location=no,status=no');"">+ADD VALUE ESTIMATE</a></span></div></td></tr><tr class='noBorder'><td colspan='5'><div id=""chart_div_value_history""></div></td></tr></table>") & "</div>")
                End If

                returnString = Replace(returnString, "<td align='left'><font size='-2' style='font-family: Arial' nowrap='nowrap'><b>Take $</b>", "<td align='left' nowrap='nowrap'><font size='-2' style='font-family: Arial' nowrap='nowrap'><b>Take $</b>")
            Else
                If crmSource = "CLIENT" Then
                    returnString = "<div class=""Box""><table border = '0' width='100%' class='formatTable blue large valuesTable'><tr class='noBorder'><td colspan='5'><div class='subHeader'>VALUE HISTORY<span class=""float_right""><a href=""#"" onclick=""javascript:window.open('edit_note.aspx?ac_ID=" & ClientID.ToString & "&action=new&amp;type=value_analysis&amp;cat_key=17&source=CLIENT&from=aircraftDetails','unloaded_me','scrollbars=yes,menubar=no,height=435,width=860,resizable=yes,toolbar=no,location=no,status=no');"">+ADD VALUE ESTIMATE</a></span></div></td></tr><tr class='noBorder'><td colspan='5'>No Value History Available</td></tr></table></div>"
                Else
                    returnString = "<div class=""Box""><table border = '0' width='100%' class='formatTable blue large valuesTable'><tr class='noBorder'><td colspan='5'><div class='subHeader'>VALUE HISTORY<span class=""float_right""><a href=""#"" onclick=""javascript:window.open('edit_note.aspx?ac_ID=" & jetnetAC_ID.ToString & "&action=new&amp;type=value_analysis&amp;cat_key=17&source=JETNET&from=aircraftDetails','unloaded_me','scrollbars=yes,menubar=no,height=435,width=860,resizable=yes,toolbar=no,location=no,status=no');"">+ADD VALUE ESTIMATE</a></span></div></td></tr><tr class='noBorder'><td colspan='5'>No Value History Available</td></tr></table></div>"
                End If
            End If
        End If


        'End If
        Return returnString
    End Function
    Public Shared Function CreateASlideout(ByVal TextStr As String) As String
        Dim returnString As String = ""
        returnString = "<div class=""searchCriteria slideoutToolTip""><p title='Information'>"

        returnString += TextStr

        returnString += "<br /></p></div>"
        Return returnString
    End Function
    Public Shared Function CreateCustomBlock(ByVal pdf_html_width As String, ByVal showEditLink As Boolean, ByVal crmSource As String, ByVal nAircraftID As Long, ByVal tableColor As String, ByVal aclsData_Temp As clsData_Manager_SQL, ByVal cliaircraft_custom_1 As Object, ByVal cliaircraft_custom_2 As Object, ByVal cliaircraft_custom_3 As Object, ByVal cliaircraft_custom_4 As Object, ByVal cliaircraft_custom_5 As Object, ByVal cliaircraft_custom_6 As Object, ByVal cliaircraft_custom_7 As Object, ByVal cliaircraft_custom_8 As Object, ByVal cliaircraft_custom_9 As Object, ByVal cliaircraft_custom_10 As Object)
        Dim returnString As String = ""
        returnString = "<div class=""Box removeTopPadding"">"
        returnString += "<table width='" & pdf_html_width & "' align='center' class=""formatTable smallerText " & tableColor & " subtextNoMargin"">"
        returnString += "<tr class=""noBorder"">"
        returnString += "<td valign=""middle"" align=""center""><font class='sub_section_title_text subHeader'>Custom Data " & IIf(showEditLink, CreateEditLink("", crmSource, nAircraftID, "", True, False, ""), "") & "</font></td></tr>"

        returnString += "<tr><td align=""left"" valign=""top"">"

        returnString += CommonAircraftFunctions.BuildCustomDataTab(aclsData_Temp, cliaircraft_custom_1, cliaircraft_custom_2, cliaircraft_custom_3, cliaircraft_custom_4, cliaircraft_custom_5, cliaircraft_custom_6, cliaircraft_custom_7, cliaircraft_custom_8, cliaircraft_custom_9, cliaircraft_custom_10)
        returnString += "</td></tr>"
        returnString += "</table></div>"
        Return returnString
    End Function
    Public Shared Function BuildCustomDataTab(ByVal aclsData_Temp As clsData_Manager_SQL, ByVal cliaircraft_custom_1 As Object, ByVal cliaircraft_custom_2 As Object, ByVal cliaircraft_custom_3 As Object, ByVal cliaircraft_custom_4 As Object, ByVal cliaircraft_custom_5 As Object, ByVal cliaircraft_custom_6 As Object, ByVal cliaircraft_custom_7 As Object, ByVal cliaircraft_custom_8 As Object, ByVal cliaircraft_custom_9 As Object, ByVal cliaircraft_custom_10 As Object) As String
        Dim ClientPreferencesTable As New DataTable

        Dim DisplayStr As String = ""
        Dim ShowDisplay As Boolean = False
        ClientPreferencesTable = aclsData_Temp.Get_Client_Preferences()
        'First we need to get the client Preferences.
        If Not IsNothing(ClientPreferencesTable) Then
            If ClientPreferencesTable.Rows.Count > 0 Then
                'Custom Field #1
                If Not IsDBNull(ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_1_use")) Then
                    If ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_1_use") = "Y" Then
                        If Not IsDBNull(ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_1")) Then
                            If Not String.IsNullOrEmpty(cliaircraft_custom_1) Then
                                DisplayStr = "<tr><td align=""left"" valign=""top""><span class=""li""><span class=""label"">" & ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_1").ToString & "</span>: "

                                DisplayStr += cliaircraft_custom_1
                                ShowDisplay = True

                                DisplayStr += "</span></td></tr>"
                            End If
                        End If
                    End If
                End If
                'Custom Field #2
                If Not IsDBNull(ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_2_use")) Then
                    If ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_2_use") = "Y" Then
                        If Not IsDBNull(ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_2")) Then
                            If Not String.IsNullOrEmpty(cliaircraft_custom_2) Then
                                DisplayStr += "<tr><td align=""left"" valign=""top""><span class=""li""><span class=""label"">" & ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_2").ToString & "</span>: "

                                DisplayStr += cliaircraft_custom_2
                                ShowDisplay = True

                                DisplayStr += "</span></td></tr>"
                            End If
                        End If
                    End If
                End If

                'Custom Field #3
                If Not IsDBNull(ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_3_use")) Then
                    If ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_3_use") = "Y" Then
                        If Not IsDBNull(ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_3")) Then
                            If Not String.IsNullOrEmpty(cliaircraft_custom_3) Then
                                DisplayStr += "<tr><td align=""left"" valign=""top""><span class=""li""><span class=""label"">" & ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_3").ToString & "</span>: "

                                DisplayStr += cliaircraft_custom_3
                                ShowDisplay = True

                                DisplayStr += "</span></td></tr>"
                            End If
                        End If
                    End If
                End If

                'Custom Field #4
                If Not IsDBNull(ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_4_use")) Then
                    If ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_4_use") = "Y" Then
                        If Not IsDBNull(ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_4")) Then
                            If Not String.IsNullOrEmpty(cliaircraft_custom_4) Then
                                DisplayStr += "<tr><td align=""left"" valign=""top""><span class=""li""><span class=""label"">" & ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_4").ToString & "</span>: "

                                DisplayStr += cliaircraft_custom_4
                                ShowDisplay = True

                                DisplayStr += "</span></td></tr>"
                            End If
                        End If
                    End If
                End If

                'Custom Field #5
                If Not IsDBNull(ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_5_use")) Then
                    If ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_5_use") = "Y" Then
                        If Not IsDBNull(ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_5")) Then
                            If Not String.IsNullOrEmpty(cliaircraft_custom_5) Then
                                DisplayStr += "<tr><td align=""left"" valign=""top""><span class=""li""><span class=""label"">" & ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_5").ToString & "</span>: "

                                DisplayStr += cliaircraft_custom_5
                                ShowDisplay = True

                                DisplayStr += "</span></td></tr>"
                            End If
                        End If
                    End If
                End If


                'Custom Field #6
                If Not IsDBNull(ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_6_use")) Then
                    If ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_6_use") = "Y" Then
                        If Not IsDBNull(ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_6")) Then
                            If Not String.IsNullOrEmpty(cliaircraft_custom_6) Then
                                DisplayStr += "<tr><td align=""left"" valign=""top""><span class=""li""><span class=""label"">" & ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_6").ToString & "</span>: "

                                DisplayStr += cliaircraft_custom_6
                                ShowDisplay = True

                                DisplayStr += "</span></td></tr>"
                            End If
                        End If
                    End If
                End If


                'Custom Field #7
                If Not IsDBNull(ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_7_use")) Then
                    If ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_7_use") = "Y" Then
                        If Not IsDBNull(ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_7")) Then
                            If Not String.IsNullOrEmpty(cliaircraft_custom_7) Then
                                DisplayStr += "<tr><td align=""left"" valign=""top""><span class=""li""><span class=""label"">" & ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_7").ToString & "</span>: "

                                DisplayStr += cliaircraft_custom_7
                                ShowDisplay = True

                                DisplayStr += "</span></td></tr>"
                            End If
                        End If
                    End If
                End If


                'Custom Field #8
                If Not IsDBNull(ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_8_use")) Then
                    If ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_8_use") = "Y" Then
                        If Not IsDBNull(ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_8")) Then
                            If Not String.IsNullOrEmpty(cliaircraft_custom_8) Then
                                DisplayStr += "<tr><td align=""left"" valign=""top""><span class=""li""><span class=""label"">" & ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_8").ToString & "</span>: "

                                DisplayStr += " : " & cliaircraft_custom_8
                                ShowDisplay = True

                                DisplayStr += "</span></td></tr>"
                            End If
                        End If
                    End If
                End If

                'Custom Field #9
                If Not IsDBNull(ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_9_use")) Then
                    If ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_9_use") = "Y" Then
                        If Not IsDBNull(ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_9")) Then
                            If Not String.IsNullOrEmpty(cliaircraft_custom_9) Then
                                DisplayStr += "<tr><td align=""left"" valign=""top""><span class=""li""><span class=""label"">" & ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_9").ToString & "</span>: "

                                DisplayStr += cliaircraft_custom_9
                                ShowDisplay = True

                                DisplayStr += "</span></td></tr>"
                            End If
                        End If
                    End If
                End If

                'Custom Field #10
                If Not IsDBNull(ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_10_use")) Then
                    If ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_10_use") = "Y" Then
                        If Not IsDBNull(ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_10")) Then
                            If Not String.IsNullOrEmpty(cliaircraft_custom_10) Then
                                DisplayStr += "<tr><td align=""left"" valign=""top""><span class=""li""><span class=""label"">" & ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_10").ToString & "</span>: "

                                DisplayStr += cliaircraft_custom_10
                                ShowDisplay = True

                                DisplayStr += "</span></td></tr>"
                            End If
                        End If
                    End If
                End If


            End If
        End If

        If ShowDisplay = False Then
            DisplayStr = "<tr><td>No custom details.</td></tr>"
        End If
        Return DisplayStr
        'custom_data_information.Text = DisplayStr
    End Function

    Public Shared Function Build_AirframeBlock(ByVal tableColor As String, ByRef bWordReport As Boolean, ByRef spacer_width As String, ByRef word_width As String, ByVal pdf_html_width As String, ByRef out_AircraftRs As DataTable, ByVal crmSource As String, ByVal aircraftID As Long, ByVal crmView As Boolean, ByVal showEditLink As Boolean, Optional ByVal is_commercial_Ac As Boolean = False, Optional ByRef est_aftt As String = "", Optional ByRef est_landings As String = "", Optional ByRef est_as_of_date As String = "") As String
        'Airframe String Append
        Dim AirframeStringAppend As String = ""


        Dim faa_temp_table As New DataTable
        Dim flight_data_temp As New flightDataFunctions
        flight_data_temp.serverConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase")
        flight_data_temp.clientConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase")



        If out_AircraftRs.Rows(0).Item("ac_journ_id").ToString = 0 Then

            faa_temp_table = flight_data_temp.getAllFAAFlightData(out_AircraftRs.Rows(0).Item("ac_reg_nbr").ToString, out_AircraftRs.Rows(0).Item("ac_id").ToString, out_AircraftRs.Rows(0).Item("ac_date_engine_times_as_of").ToString)

            AirframeStringAppend = "<div class=""Box removeTopPadding airframeTable"">"
            AirframeStringAppend += "<table width='" & pdf_html_width & "' align='center' class=""formatTable smallerText " & tableColor & " subtextNoMargin"">"
            AirframeStringAppend += "<tr class=""noBorder""><td colspan=""2"" class=""airframeTable"">" & Replace(flight_data_temp.displayAirframeTimesData(faa_temp_table, out_AircraftRs.Rows(0).Item("ac_times_as_of_date").ToString, out_AircraftRs.Rows(0).Item("ac_airframe_tot_hrs").ToString, out_AircraftRs.Rows(0).Item("ac_airframe_tot_landings").ToString, True, out_AircraftRs.Rows(0).Item("ac_previously_owned_flag").ToString.ToUpper, IIf(Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_purchase_date")), out_AircraftRs.Rows(0).Item("ac_purchase_date"), ""), IIf(Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_year")), IIf(IsNumeric(out_AircraftRs.Rows(0).Item("ac_year")), out_AircraftRs.Rows(0).Item("ac_year"), 0), 0), True, "", "", "", IIf(crmView And showEditLink, CommonAircraftFunctions.CreateEditLink("", crmSource, aircraftID, "", IIf(crmSource = "CLIENT", True, False), False, ""), ""), is_commercial_Ac, est_aftt, est_landings, est_as_of_date), "border=""1""", "border=""0""") & "</td></tr></table>"
            AirframeStringAppend += "</div>"
            AirframeStringAppend = Replace(Replace(Replace(AirframeStringAppend, "Current&nbsp;Values", "CURRENT<br />VALUES"), "Estimated&nbsp;Values", "EST. VALUES"), "Flight&nbsp;Activity", "FLIGHT ACTIVITY")

        Else

            AirframeStringAppend = "<div class=""Box removeTopPadding airframeTable"">"
            AirframeStringAppend += "<table width='" & pdf_html_width & "' align='center' class=""formatTable smallerText " & tableColor & " subtextNoMargin"">"
            AirframeStringAppend += "<tr class=""noBorder"">"
            AirframeStringAppend += "<td valign=""middle"" align=""center""><font class='sub_section_title_text subHeader'>Airframe</font></td></tr>"

            AirframeStringAppend += "<tr class=""noBorder""><td colspan=""2"" class=""airframeTable"">" & (CommonAircraftFunctions.DisplayUsageInfo(out_AircraftRs)) & "</td></tr></table>"
            AirframeStringAppend += "</div>"
        End If

        Return AirframeStringAppend
    End Function
    'Public Shared Function CreateHeaderLine(ByVal aircraftTable As DataTable) As String
    '  Dim serNoHeader As String = ""

    '  If aircraftTable.Rows.Count > 0 Then
    '    serNoHeader += "<h2 class='mainHeading padded_left'>"

    '    serNoHeader += "<strong>"
    '    If Not String.IsNullOrEmpty(aircraftTable.Rows(0).Item("amod_make_name")) Then
    '      serNoHeader += aircraftTable.Rows(0).Item("amod_make_name") & " "
    '    End If

    '    If Not String.IsNullOrEmpty(aircraftTable.Rows(0).Item("amod_model_name")) Then
    '      serNoHeader += aircraftTable.Rows(0).Item("amod_model_name")
    '    End If
    '    serNoHeader += "</strong> "

    '    If Not String.IsNullOrEmpty(aircraftTable.Rows(0).Item("ac_ser_nbr")) Then
    '      serNoHeader += " SN #" + aircraftTable.Rows(0).Item("ac_ser_nbr")
    '    End If

    '    serNoHeader += "</h2>"
    '  End If
    '  Return serNoHeader
    'End Function
    Public Shared Function CreateHeaderLine(ByVal amodMakeName As Object, ByVal amodModelName As Object, ByVal amodSerNo As Object, ByVal optionalTitle As String, Optional ByRef amod_id As Long = 0) As String
        Dim returnString As String = ""

        If amod_id > 0 Then
            returnString = "<a href=""#"" style='text-decoration: none' onclick=""javascript:load('view_template.aspx?ViewID=1&noMaster=false&amod_id=" & amod_id & "','','scrollbars=yes,menubar=no,height=700,width=1150,resizable=yes,toolbar=no,location=no,status=no');return false;"">"
        End If

        returnString += "<h2 class='mainHeading padded_left'>"
        returnString += "<strong>"
        If Not String.IsNullOrEmpty(amodMakeName) Then
            returnString += amodMakeName & " "
        End If
        If Not String.IsNullOrEmpty(amodModelName) Then
            returnString += amodModelName
        End If
        returnString += "</strong>"
        If amod_id > 0 Then
            returnString += "</a> "
        Else
            returnString += " "
        End If

        If Not String.IsNullOrEmpty(amodSerNo) Then
            returnString += " SN #" + amodSerNo
        End If
        If optionalTitle <> "" Then
            returnString += " " & optionalTitle
        End If
        returnString += "</h2>"
        Return returnString
    End Function
    'Public Shared Function CreateHeaderLineClient(ByVal aircraftTable As DataTable) As String
    '  Dim serNoHeader As String = ""

    '  If aircraftTable.Rows.Count > 0 Then
    '    serNoHeader += "<h2 class='mainHeading padded_left'>"

    '    serNoHeader += "<strong>"
    '    If Not String.IsNullOrEmpty(aircraftTable.Rows(0).Item("cliamod_make_name")) Then
    '      serNoHeader += aircraftTable.Rows(0).Item("cliamod_make_name") & " "
    '    End If

    '    If Not String.IsNullOrEmpty(aircraftTable.Rows(0).Item("cliamod_model_name")) Then
    '      serNoHeader += aircraftTable.Rows(0).Item("cliamod_model_name")
    '    End If
    '    serNoHeader += "</strong> "

    '    If Not String.IsNullOrEmpty(aircraftTable.Rows(0).Item("cliaircraft_ser_nbr")) Then
    '      serNoHeader += " SN #" + aircraftTable.Rows(0).Item("cliaircraft_ser_nbr")
    '    End If

    '    serNoHeader += "</h2>"
    '  End If
    '  Return serNoHeader
    'End Function
    Public Shared Function BuildReusableTable(ByVal in_AircraftID As Long, ByVal in_journalID As Long, ByVal crmSource As String, ByRef ValueDescription As String, ByVal aclsData_Temp As clsData_Manager_SQL, ByVal crmview As Boolean, ByRef jetnetTransactionID As Long, ByVal transactionSource As String) As DataTable
        Dim out_AircraftRs As New DataTable
        If (crmSource = "CLIENT" Or transactionSource = "CLIENT") And crmview = True Then
            If in_journalID = 0 Then
                out_AircraftRs = aclsData_Temp.Get_Client_Aircraft_as_Jetnet_Fields(in_AircraftID)
                If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_value_description")) Then
                    ValueDescription = out_AircraftRs.Rows(0).Item("ac_value_description")
                End If

                If Not IsDBNull(out_AircraftRs.Rows(0).Item("jetnet_amod_id")) Then
                    If out_AircraftRs.Rows(0).Item("jetnet_amod_id") > 0 Then
                        Dim ModelTable As New DataTable
                        ModelTable = aclsData_Temp.GetJetnetModelInfo(out_AircraftRs.Rows(0).Item("jetnet_amod_id"), True, "DisplayAircraftDetails/CommonACFunctions.vb")
                        out_AircraftRs = aclsData_Temp.AddEngineNumber(ModelTable, out_AircraftRs)

                        ModelTable.Dispose()
                        ModelTable = Nothing
                    End If
                End If
            ElseIf in_journalID > 0 Then
                Dim clientTable As New DataTable
                Dim jetnetTable As New DataTable

                If transactionSource = "CLIENT" Then 'need to combine it
                    clientTable = aclsData_Temp.Get_Client_Aircraft_Transaction_as_Jetnet_Fields(in_journalID, 0)
                    jetnetTable = aclsData_Temp.GetJETNET_ACDetails_PresentAndHistorical(clientTable.Rows(0).Item("ac_id"), clientTable.Rows(0).Item("ac_journ_id"))
                    out_AircraftRs = aclsData_Temp.CreateClientWithJetnet(clientTable, jetnetTable)
                Else
                    clientTable = aclsData_Temp.Get_Client_Aircraft_as_Jetnet_Fields(in_AircraftID)
                    out_AircraftRs = aclsData_Temp.GetJETNET_ACDetails_PresentAndHistorical(clientTable.Rows(0).Item("ac_id"), in_journalID)
                End If

            End If

        ElseIf crmview = True And transactionSource = "CLIENT" Then
            If transactionSource = "CLIENT" Then
                'Need the jetnet transaction ID
                Dim CheckTable As New DataTable
                CheckTable = aclsData_Temp.Get_Client_Client_Transactions(in_journalID, 0)
                If Not IsNothing(CheckTable) Then
                    If CheckTable.Rows.Count > 0 Then
                        jetnetTransactionID = CheckTable.Rows(0).Item("clitrans_jetnet_trans_id")
                    End If
                End If
            End If
            out_AircraftRs = aclsData_Temp.GetJETNET_ACDetails_PresentAndHistorical(in_AircraftID, IIf(transactionSource = "CLIENT", jetnetTransactionID, in_journalID))
        Else
            out_AircraftRs = aclsData_Temp.GetJETNET_ACDetails_PresentAndHistorical(in_AircraftID, in_journalID)
        End If
        Return out_AircraftRs
    End Function
    Public Shared Function Build_Identification_Block(ByVal tableColor As String, ByRef bWordReport As Boolean, ByRef spacer_width As String, ByRef word_width As String, ByVal pdf_html_width As String, ByRef Counter_For_PDF As Integer, ByRef out_AircraftRs As DataTable, ByVal CRMSource As String, ByVal in_JournalID As Long, ByVal in_AircraftID As Long, ByRef aclsData_Temp As clsData_Manager_SQL, ByRef chkBlindReport As System.Web.UI.WebControls.CheckBox, ByVal chkIncludeBaseLocation As CheckBox, ByVal bAerodexFlag As Boolean, ByVal otherID As Long, ByVal crmView As Boolean, Optional ByVal DisplayHeaderLine As Boolean = True, Optional ByVal showToggleSwitch As Boolean = False, Optional holder As Page = Nothing, Optional DisplayAircraftName As Boolean = False, Optional optionalHeaderString As String = "") As String
        Dim comp_functions As New CompanyFunctions

        Dim ValueDescription As String = ""
        Dim htmlOutput As New StringBuilder

        'values to append later:
        Dim confidential As String = ""
        Dim ac_year_display As String = ""
        Dim airfram_tot_time As String = ""
        Dim cycles As String = ""


        'If CRMSource = "CLIENT" Then
        '  out_AircraftRs = aclsData_Temp.Get_Client_Aircraft_as_Jetnet_Fields(in_AircraftID)
        '  If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_value_description")) Then
        '    ValueDescription = out_AircraftRs.Rows(0).Item("ac_value_description")
        '  End If

        '  If Not IsDBNull(out_AircraftRs.Rows(0).Item("jetnet_amod_id")) Then
        '    If out_AircraftRs.Rows(0).Item("jetnet_amod_id") > 0 Then
        '      Dim ModelTable As New DataTable
        '      ModelTable = aclsData_Temp.GetJetnetModelInfo(out_AircraftRs.Rows(0).Item("jetnet_amod_id"), True, "DisplayAircraftDetails/CommonACFunctions.vb")
        '      out_AircraftRs = aclsData_Temp.AddEngineNumber(ModelTable, out_AircraftRs)

        '      ModelTable.Dispose()
        '      ModelTable = Nothing
        '    End If
        '  End If
        'Else
        '  out_AircraftRs = aclsData_Temp.GetJETNET_ACDetails_PresentAndHistorical(in_AircraftID, in_JournalID)
        'End If


        If Not IsNothing(out_AircraftRs) Then
            If out_AircraftRs.Rows.Count > 0 Then
                Counter_For_PDF = Counter_For_PDF + 1

                htmlOutput.Append("<div class=""Box removeTopPadding specialHeadingTable"">")




                If bWordReport = True Then
                    htmlOutput.Append("<table width='" & word_width & "' align='center'>")
                Else
                    htmlOutput.Append("<table width='" & pdf_html_width & "' align='center' class=""formatTable large " & tableColor & " subtextNoMargin"">")
                End If



                If DisplayHeaderLine = True Then
                    htmlOutput.Append("<tr class=""noBorder""><td colspan='2' align=""left""><font class='" & HttpContext.Current.Session("FONT_CLASS_HEADER") & " subHeader'>")

                    If DisplayAircraftName Then
                        htmlOutput.Append("" & optionalHeaderString & "")
                    Else
                        htmlOutput.Append("Identification")
                    End If


                    If showToggleSwitch Then
                        Dim SwitchToggleChecked As Boolean = False
                        Dim alertTable As New DataTable
                        Dim folderIndexID As Long = 0
                        alertTable = clsGeneral.clsGeneral.CheckAircraftAlertsOn(IIf(CRMSource = "CLIENT", otherID, in_AircraftID), HttpContext.Current.Session.Item("localUser").crmSubSubID, HttpContext.Current.Session.Item("localUser").crmUserLogin)
                        If Not IsNothing(alertTable) Then
                            If alertTable.Rows.Count > 0 Then
                                SwitchToggleChecked = True
                            End If
                        End If
                        htmlOutput.Append("<label class=""switchToggle"" title=""Turn alerts on or off for this aircraft.  Emails will automatically be sent to your email address as JETNET records changes.""><span class=""toggleText"">Alerts</span>")
                        htmlOutput.Append("<input id=""aircraftAlertToggle"" type =""checkbox"" " & IIf(SwitchToggleChecked, "checked=""true""", "") & "/>")
                        htmlOutput.Append("<span class=""sliderToggle roundToggle""></span>")
                        htmlOutput.Append("</label>")


                        'We need to add some javascript for this button.

                        Dim jsStr As String = "jQuery(function() {"
                        jsStr += " jQuery('#aircraftAlertToggle').change(function() { " & vbNewLine
                        jsStr += "jQuery.ajax({"
                        jsStr += "data: this.checked,"
                        jsStr += "type: 'GET',"
                        jsStr += "contentType: ""application/json; charset=utf-8"","
                        jsStr += "dataType: ""json"","
                        jsStr += "url: 'JSONresponse.aspx/toggleAircraftAlert?acID=" & IIf(CRMSource = "CLIENT", otherID, in_AircraftID) & "&checked=' + this.checked + ''"
                        jsStr += "})"
                        jsStr += " });" & vbNewLine
                        jsStr += " });" & vbNewLine
                        System.Web.UI.ScriptManager.RegisterStartupScript(holder, holder.GetType, "toggleEventScr", jsStr + vbCrLf, True)
                    End If


                    If crmView And in_JournalID = 0 Then
                        If otherID > 0 Then
                            If CRMSource = "CLIENT" Then
                                htmlOutput.Append("<span><strong>/CLIENT RECORD</strong><span class=""float_right""><a href=""javascript:void();"" title=""SYNCHRONIZE WITH JETNET"" class=""no_text_underline"" onclick=""javascript:load('/edit.aspx?synch=true&type=aircraft&ac_ID=" & in_AircraftID.ToString & "&source=CLIENT&otherID=" & otherID.ToString & "','','scrollbars=yes,menubar=no,height=400,width=450,resizable=yes,toolbar=no,location=no,status=no');""><i class=""fa fa-refresh""></i></a>" & CreateEditLink("", CRMSource, in_AircraftID, "", True, False, "") & "<span class=""float_right pipeDelimeter"">|</span><a href=""/DisplayAircraftDetail.aspx?acid=" & otherID & """>VIEW JETNET</a></span></span>")
                            Else
                                htmlOutput.Append("<span><span class=""float_right""><a href=""/DisplayAircraftDetail.aspx?acid=" & otherID & "&source=CLIENT"">VIEW CLIENT</a></span></span>")
                            End If
                        Else
                            If CRMSource <> "CLIENT" Then
                                htmlOutput.Append("<span><span class=""float_right"">" & CreateEditLink("", CRMSource, in_AircraftID, "", False, True, "") & "</span></span>")
                            End If
                        End If
                    End If


                    htmlOutput.Append("</font></td></tr>")
                End If


                If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_mfr_year")) And Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_year")) Then
                    ac_year_display = out_AircraftRs.Rows(0).Item("ac_year")
                    htmlOutput.Append(comp_functions.create_value_with_label("Year Mfr/Dlv", out_AircraftRs.Rows(0).Item("ac_mfr_year").ToString & " / " + out_AircraftRs.Rows(0).Item("ac_year").ToString, True, True, Counter_For_PDF, spacer_width))
                ElseIf Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_mfr_year")) Then
                    htmlOutput.Append(comp_functions.create_value_with_label("Year Mfr", out_AircraftRs.Rows(0).Item("ac_mfr_year").ToString, True, True, Counter_For_PDF, spacer_width))
                ElseIf Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_year")) Then
                    ac_year_display = out_AircraftRs.Rows(0).Item("ac_year")
                    htmlOutput.Append(comp_functions.create_value_with_label("Year Dlv", out_AircraftRs.Rows(0).Item("ac_year").ToString, True, True, Counter_For_PDF, spacer_width))
                End If



                If Not chkBlindReport.Checked Then
                    ' reg number
                    If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_reg_no")) Then

                        If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_reg_no_expiration_date")) Then
                            htmlOutput.Append(comp_functions.create_value_with_label("Registration #", "<span Class=""mediumText emphasisColor"">" & out_AircraftRs.Rows(0).Item("ac_reg_no").ToString & "</span>, Expires:   " + clsGeneral.clsGeneral.TwoPlaceYear(out_AircraftRs.Rows(0).Item("ac_reg_no_expiration_date")) & IIf(Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_prev_reg_no")), ", Previous Reg #" & out_AircraftRs.Rows(0).Item("ac_prev_reg_no"), ""), True, True, Counter_For_PDF, spacer_width))
                        Else

                            htmlOutput.Append(comp_functions.create_value_with_label("Registration #", "<span class=""mediumText emphasisColor"">" & out_AircraftRs.Rows(0).Item("ac_reg_no").ToString & "</span>" & IIf(Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_prev_reg_no")), ", Previous Reg #" & out_AircraftRs.Rows(0).Item("ac_prev_reg_no"), ""), True, True, Counter_For_PDF, spacer_width))
                        End If
                    End If

                    If Not bAerodexFlag Then
                        ' airframe total time
                        If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_purchase_date")) Then
                            htmlOutput.Append(comp_functions.create_value_with_label("Purchase Date", clsGeneral.clsGeneral.TwoPlaceYear(out_AircraftRs.Rows(0).Item("ac_purchase_date")), True, True, Counter_For_PDF, spacer_width))
                        End If
                    End If
                Else
                    htmlOutput.Append(comp_functions.create_value_with_label("Aircraft ID", out_AircraftRs.Rows(0).Item("ac_id").ToString, True, True, Counter_For_PDF, spacer_width))
                End If


                If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_airframe_tot_hrs")) Then
                    airfram_tot_time = comp_functions.create_value_with_label("", FormatNumber(out_AircraftRs.Rows(0).Item("ac_airframe_tot_hrs"), 0), False, False, Counter_For_PDF, spacer_width)
                End If
                ' landing cycles
                If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_airframe_tot_landings")) Then
                    cycles = comp_functions.create_value_with_label("Landings/Cycles", FormatNumber(out_AircraftRs.Rows(0).Item("ac_airframe_tot_landings"), 0), True, True, Counter_For_PDF, spacer_width)
                End If

                If Not IsNothing(chkIncludeBaseLocation) Then
                    If chkIncludeBaseLocation.Checked Then
                        Dim airportInfo As String = ""

                        If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_aport_iata_code")) Then
                            airportInfo += out_AircraftRs.Rows(0).Item("ac_aport_iata_code").ToString.Trim
                        End If

                        If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_aport_icao_code")) Then
                            If airportInfo <> "" Then
                                airportInfo += "/"
                            End If
                            airportInfo += out_AircraftRs.Rows(0).Item("ac_aport_icao_code").ToString.Trim
                        End If

                        If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_aport_name")) Then
                            If airportInfo <> "" Then
                                airportInfo += "/"
                            End If
                            airportInfo += out_AircraftRs.Rows(0).Item("ac_aport_name").ToString.Trim
                        End If

                        If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_aport_city")) Then
                            If airportInfo <> "" Then
                                airportInfo += ", "
                            End If
                            airportInfo += out_AircraftRs.Rows(0).Item("ac_aport_city").ToString.Trim
                        End If

                        If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_aport_state")) Then
                            If airportInfo <> "" Then
                                airportInfo += ", "
                            End If
                            airportInfo += out_AircraftRs.Rows(0).Item("ac_aport_state").ToString.Trim
                        End If

                        If Not IsDBNull(out_AircraftRs.Rows(0).Item("ac_aport_country")) Then
                            If airportInfo <> "" Then
                                airportInfo += " - "
                            End If
                            airportInfo += Replace(out_AircraftRs.Rows(0).Item("ac_aport_country").ToString.Trim, "United States", "U.S")
                        End If

                        htmlOutput.Append(comp_functions.create_value_with_label("Location", airportInfo, True, True, Counter_For_PDF, spacer_width))
                    End If
                End If

                htmlOutput.Append("</table>")
                htmlOutput.Append("</div>")
            End If 'No rows
        End If 'Nothing table
        Return htmlOutput.ToString
    End Function
#End Region














    Public Shared Function trim_out_year_start(ByVal temp_date As String) As String

        Dim year_found As String = ""
        trim_out_year_start = temp_date

        If IsDate(temp_date) Then
            If Trim(temp_date) <> "" Then
                year_found = Right(Trim(temp_date), 4)
                If Left(Trim(year_found), 2) = "20" Or Left(Trim(year_found), 2) = "19" Then
                    year_found = Right(Trim(year_found), 2)

                    trim_out_year_start = Month(CDate(temp_date)) & "/" & Day(CDate(temp_date)) & "/" & year_found
                End If
            End If
        End If



    End Function
    Public Shared Function GetExclusive(ByVal in_AircraftID As Long,
                                       ByVal in_AircraftJournalID As Long, ByVal aclsData_Temp As clsData_Manager_SQL, ByRef CRMSource As String, ByRef ToggleAnalytics As Boolean, Optional ByRef comp_id_list As String = "", Optional ByRef new_comp_id As Long = 0) As String

        Dim RefTable As New DataTable
        Dim htmlOut As StringBuilder = New StringBuilder()
        Dim row_count As Integer = 0
        ToggleAnalytics = False 'Default this to false

        If CRMSource = "CLIENT" Then
            RefTable = aclsData_Temp.Get_Client_AC_RelationshipByType(in_AircraftID, "'93','98','99'", comp_id_list)
        Else
            RefTable = aclsData_Temp.Get_EvoAC_RelationshipByType(in_AircraftID, in_AircraftJournalID, "'93','98','99'", comp_id_list)
        End If

        If Not IsNothing(RefTable) Then

            If RefTable.Rows.Count > 0 Then
                If Not IsDBNull(RefTable.Rows(0).Item("comp_name")) Then
                    If Not String.IsNullOrEmpty(RefTable.Rows(0).Item("comp_name").ToString) Then
                        htmlOut.Append(RefTable.Rows(0).Item("comp_name").ToString)
                    End If
                End If

                For Each r As DataRow In RefTable.Rows 'This does something a little weird. It loops through the recordsets if there are more than 1 broker, then it goes ahead and looks at the Id to compare the company ID of the user with the returned IDs. If it's returned, we go ahead and show the analytics tab on the aircraft details page.
                    If Not IsDBNull(r("comp_id")) Then
                        If r("comp_id") > 0 Then

                            row_count = row_count + 1

                            If row_count = 1 Then ' so that we only add 1 company at a time 
                                new_comp_id = r("comp_id")

                                If Trim(comp_id_list) <> "" Then
                                    comp_id_list = comp_id_list & "," & r("comp_id")
                                Else
                                    comp_id_list = r("comp_id")
                                End If
                            End If


                            If HttpContext.Current.Session.Item("localUser").crmUserCompanyID = r("comp_id") Then
                                ToggleAnalytics = True 'not the case
                            End If
                        End If
                    End If
                Next
            End If
        End If

        RefTable.Dispose()
        'If String.IsNullOrEmpty(htmlOut.ToString) Then
        '  htmlOut.Append("&lt;Unknown&gt;")
        'End If

        Return htmlOut.ToString.Trim

    End Function
    Public Shared Function GetAircraft_Ownership(ByVal aclsData_temp As clsData_Manager_SQL, ByVal ac_id As Long)
        GetAircraft_Ownership = ""
        Dim temp_table As New DataTable
        Dim util_functions As New utilization_functions
        Dim htmlOut As New StringBuilder
        Dim htmlOut_start As New StringBuilder
        Dim cssClass As String = "alt_row"
        Dim font_text_title As String = ""
        Dim font_text_start As String = ""
        Dim font_text_end As String = ""
        Dim temp_dir As String = "right"
        Dim temp_count As Integer = 0
        Dim bgcolor As String = ""
        Dim temp_date As String = ""
        Dim temp_date2 As String = ""
        Dim util_link As String = ""
        Dim extra_words As String = ""

        Try

            'If Trim(from_spot) = "pdf" Then
            '  font_text_start = "<font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>"
            '  font_text_title = "<font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT_TITLE") & "'>"
            '  font_text_end = "</font>"
            '  temp_dir = "left"
            'Else
            font_text_start = ""
            font_text_title = ""
            font_text_end = ""
            '  End If


            util_functions.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim

            temp_table = util_functions.GetAircraft_Ownership_Function(ac_id)

            If Not IsNothing(temp_table) Then
                If temp_table.Rows.Count > 0 Then

                    htmlOut_start.Append("<div class='subHeader'>&nbsp;Aircraft Ownership</div><br /><table cellspacing='0' cellpadding='0' width='97%' align=""center"" class=""formatTable blue ownershipTable"">")
                    htmlOut_start.Append("<tr class='header_row'>")
                    htmlOut_start.Append("<th align='left'>" & font_text_title & "ACQUIRED&nbsp;" & font_text_end & "</th>")
                    htmlOut_start.Append("<th align='left'>" & font_text_title & "FROM" & font_text_end & "</th>")
                    htmlOut_start.Append("<th align='left'>" & font_text_title & "TRANSFERRED&nbsp;&nbsp;" & font_text_end & "</th>")
                    htmlOut_start.Append("<th align='left'>" & font_text_title & "TO" & font_text_end & "</th>")
                    htmlOut_start.Append("</tr>")
                    htmlOut.Append(htmlOut_start.ToString)

                    For Each r As DataRow In temp_table.Rows


                        htmlOut.Append("<tr class=""" & cssClass & """ valign='top'>")

                        extra_words = ""
                        If Not IsDBNull(r("journ_subcat_code_part1")) Then
                            If Trim(r("journ_subcat_code_part1")) = "FC" Then
                                extra_words = " (Foreclosed)"
                            ElseIf Trim(r("journ_subcat_code_part1")) = "SS" Then
                                If Not IsDBNull(r("cref_owner_percent")) Then
                                    extra_words = " (" & r("cref_owner_percent") & "%)"
                                End If

                            End If
                        End If

                        If Not IsDBNull(r("purchased_date")) Then
                            temp_date = r("purchased_date")
                            temp_date2 = Left(Trim(temp_date), Len(Trim(temp_date)) - 4)
                            temp_date = Right(Trim(temp_date), 4)
                            temp_date = Right(Trim(temp_date), 2)
                            htmlOut.Append("<td align='" & temp_dir & "' class=""text_align_center"">" & font_text_start & "")
                            '  If Trim(from_spot) = "pdf" Then
                            '    htmlOut.Append("" & temp_date2 & temp_date & "</td>")
                            '  Else

                            'htmlOut.Append("<a href='#' onclick=""javascript:load('DisplayAircraftDetail.aspx?acid=" & ac_id & IIf(r("journ_id") <> 0, "&jid=" & r("journ_id"), "") & "','','scrollbars=yes,menubar=no,height=900,width=1090,resizable=yes,toolbar=no,location=no,status=no');return false;"">")
                            'htmlOut.Append("" & temp_date2 & temp_date & "</a>")
                            htmlOut.Append(DisplayFunctions.WriteDetailsLink(ac_id, 0, 0, r("journ_id"), True, temp_date2 & temp_date, "", ""))
                            htmlOut.Append("</td>")
                            ' End If 

                        Else
                            htmlOut.Append("<td class=""text_align_left"">&nbsp;</td>")
                        End If

                        htmlOut.Append("<td align='" & temp_dir & "' class=""text_align_left"">" & font_text_start & "" & r("owner") & "" & font_text_end & "</td>")


                        If Not IsDBNull(r("Sold_on")) Then
                            temp_date = r("Sold_on")
                            temp_date2 = Left(Trim(temp_date), Len(Trim(temp_date)) - 4)
                            temp_date = Right(Trim(temp_date), 4)
                            temp_date = Right(Trim(temp_date), 2)
                            htmlOut.Append("<td align='" & temp_dir & "' class=""text_align_center"">" & font_text_start & "" & temp_date2 & temp_date & "" & font_text_end & "</td>")
                        Else
                            htmlOut.Append("<td class=""text_align_center"">&nbsp;</td>")
                        End If
                        htmlOut.Append("<td class=""text_align_left"">" & font_text_start & "" & r("sold_to") & "" & extra_words & font_text_end & "</td>")

                        htmlOut.Append("</tr>")

                        If cssClass = "" Then
                            cssClass = "alt_row"
                        Else
                            cssClass = ""
                        End If


                    Next

                    htmlOut.Append("</table>")

                    GetAircraft_Ownership = htmlOut.ToString
                End If
            End If

        Catch ex As Exception

        End Try
    End Function
    ''' <summary>
    ''' Setting up aircraft pictures.
    ''' </summary>
    ''' <param name="aclsData_temp">Datalayer class</param>
    ''' <param name="MySesState">Session State</param>
    ''' <param name="in_AircraftRs">Aircraft table reference</param>
    ''' <param name="bShowOtherLinks">Ability to toggle other links on/off</param>
    ''' <param name="isFromView"></param>
    ''' <param name="nCurrentRec">Current record of dataset</param>
    ''' <param name="slideshow_script">The literal that holds the initialization of the slideshow script. If we don't need it, toggle this off.</param>
    ''' <param name="step_script">The literal that holds the initialization of the step carousel script. If we don't need it, toggle off.</param>
    ''' <param name="crmView">Boolean that allows a CRMVIEW mode which will toggle features off (like links to all pics)</param>
    ''' <returns>Text string</returns>
    ''' <remarks></remarks>
    Public Shared Function GetAircraftPictures(ByRef aircraft_picture_slideshow As Label, ByVal aclsData_temp As clsData_Manager_SQL, ByVal MySesState As HttpSessionState,
                                           ByRef in_AircraftRs As DataTable,
                                           ByVal bShowOtherLinks As Boolean,
                                           ByVal isFromView As String,
                                           ByVal nCurrentRec As Long,
                                           ByRef slideshow_script As Literal,
                                           ByRef step_script As Literal,
                                           ByVal crmView As Boolean) As String

        Dim fAcpic_image_type As String = ""
        Dim fAcpic_id As String = ""
        Dim fAcpic_subject As String = ""
        Dim imgFolder As String = ""
        Dim theImgFile As String = ""
        Dim picture_counter As Integer = 0
        Dim javascript_slideshow_begining As String = ""
        Dim javascript_slideshow_ending As String = ""
        Dim PictureTable As New DataTable
        Dim sQuery As String = ""
        Dim htmlOut As StringBuilder = New StringBuilder()
        Dim view_all_pictures As String = ""
        Dim first_picture As String = ""


        Dim ac_image_file As String = ""
        Dim temp_height As Integer = 0
        Dim temp_width As Integer = 0
        Dim zimage2 As System.Drawing.Image
        ' Dim zimage3 As System.Drawing.Image
        Dim desired_width As Integer = 500
        Dim desired_height As Integer = 142
        Dim temp_percent1 As Double = 0.0
        Dim temp_percent2 As Double = 0.0
        Dim total_width As Integer = 0
        Dim width_size_total As Integer = 740
        Dim add_pic As String = "Y"
        Dim blow_up As Boolean = False
        Dim temp_calc As Double = 0.0



        slideshow_script.Visible = False
        step_script.Visible = False
        ' Try
        PictureTable = aclsData_temp.GetJETNET_AC_pictures(in_AircraftRs.Rows(0).Item("ac_id"), in_AircraftRs.Rows(0).Item("ac_journ_id"))

        htmlOut.Append("<div class=""Box""><table cellspacing='0' cellpadding='0' width='100%'>")
        htmlOut.Append("<tr><td align='left' valign='top'>")
        If Not IsNothing(PictureTable) Then
            If PictureTable.Rows.Count > 0 Then
                'view all pictures button is relevant
                If bShowOtherLinks Then
                    view_all_pictures = "<img src='images/view_all_pictures.png' onclick=""javascript:SubmitTransactionDocumentForm('" & in_AircraftRs.Rows(0).Item("amod_make_name").ToString & "','" & in_AircraftRs.Rows(0).Item("amod_model_name").ToString & "','" & in_AircraftRs.Rows(0).Item("ac_ser_nbr").ToString & "'," & in_AircraftRs.Rows(0).Item("ac_id").ToString & "," & in_AircraftRs.Rows(0).Item("ac_journ_id").ToString & ",'');""  alt='View all Pictures' class='spec_image' />"
                Else
                    view_all_pictures = "<img src='images/view_all_pictures.png' onclick=""javascript:SubmitTransactionDocumentForm('" & in_AircraftRs.Rows(0).Item("amod_make_name").ToString & "','" & in_AircraftRs.Rows(0).Item("amod_model_name").ToString & "','" & in_AircraftRs.Rows(0).Item("ac_ser_nbr").ToString & "'," & in_AircraftRs.Rows(0).Item("ac_id").ToString & "," & in_AircraftRs.Rows(0).Item("ac_journ_id").ToString & ",'');""  alt='View all Pictures' class='spec_image' />"
                End If

                If HttpContext.Current.Session.Item("isMobile") = False Then
                    slideshow_script.Visible = True
                    step_script.Visible = True
                End If


                If Not (IsDBNull(PictureTable.Rows(0).Item("acpic_image_type"))) Then
                    If Not String.IsNullOrEmpty(PictureTable.Rows(0).Item("acpic_image_type").ToString) Then
                        fAcpic_image_type = PictureTable.Rows(0).Item("acpic_image_type").ToString.ToLower.Trim
                    End If
                End If

                If Not (IsDBNull(PictureTable.Rows(0).Item("acpic_id"))) Then
                    If Not String.IsNullOrEmpty(PictureTable.Rows(0).Item("acpic_id").ToString) Then
                        fAcpic_id = PictureTable.Rows(0).Item("acpic_id").ToString.Trim
                    End If
                End If

                If Not (IsDBNull(PictureTable.Rows(0).Item("acpic_subject"))) Then
                    If Not String.IsNullOrEmpty(PictureTable.Rows(0).Item("acpic_subject").ToString) Then
                        fAcpic_subject = PictureTable.Rows(0).Item("acpic_subject").ToString.Trim
                    End If
                End If



                If fAcpic_image_type.Contains("jpg") Then
                    ' MJM - New Path Added 9/04 

                    If HttpContext.Current.Session.Item("jetnetWebSiteType") = crmWebClient.eWebSiteTypes.LOCAL Then
                        imgFolder = "https://www.testjetnetevolution.com/pictures/aircraft"
                    Else
                        imgFolder = HttpContext.Current.Session.Item("jetnetFullHostName").ToString + HttpContext.Current.Session.Item("AircraftPicturesFolderVirtualPath")
                    End If

                    theImgFile = imgFolder + "/" + in_AircraftRs.Rows(0).Item("ac_id").ToString + Constants.cHyphen + in_AircraftRs.Rows(0).Item("ac_journ_id").ToString + Constants.cHyphen + fAcpic_id + Constants.cDot + fAcpic_image_type

                    picture_counter = 1


                    javascript_slideshow_begining = ("<div id=""slider1"" class=""sliderwrapper"">")
                    javascript_slideshow_begining = javascript_slideshow_begining & ("<div id=""paginate-slider1"" class=""pagination""></div>")
                    javascript_slideshow_begining = javascript_slideshow_begining & ("<div class='contentdiv' align='center'>")

                    Try
                        ac_image_file = HttpContext.Current.Server.MapPath("pictures\aircraft\") & in_AircraftRs.Rows(0).Item("ac_id").ToString + Constants.cHyphen + in_AircraftRs.Rows(0).Item("ac_journ_id").ToString + Constants.cHyphen + fAcpic_id + Constants.cDot + fAcpic_image_type
                        zimage2 = System.Drawing.Image.FromFile(ac_image_file)
                        temp_width = zimage2.Width
                        temp_height = zimage2.Height
                        desired_width = 510
                        desired_height = 350

                        Call find_image_resize_to_fit(temp_width, temp_height, desired_width, desired_height, javascript_slideshow_begining, in_AircraftRs.Rows(0), imgFolder, fAcpic_id, fAcpic_image_type, fAcpic_subject, "Aircraft", 0, 0)

                        javascript_slideshow_begining = javascript_slideshow_begining & ("<div class='overlay_bottom_gray'>" + fAcpic_subject + "</div>")
                        javascript_slideshow_begining = javascript_slideshow_begining & ("</div>")


                        temp_width = zimage2.Width
                        temp_height = zimage2.Height
                        desired_height = 100
                        desired_width = 100

                        If (temp_height > desired_height) Then  '  And (temp_height > temp_width) 
                            temp_percent1 = CDbl(CDbl(desired_height) / CDbl(temp_height))
                            temp_width = CDbl(CDbl(temp_width) * CDbl(temp_percent1))
                            temp_height = CDbl(CDbl(temp_height) * CDbl(temp_percent1))
                        End If

                        If (temp_width > desired_width) Then
                            temp_percent1 = CDbl(CDbl(desired_width) / CDbl(temp_width))
                            temp_width = CDbl(CDbl(temp_width) * CDbl(temp_percent1))
                            temp_height = CDbl(CDbl(temp_height) * CDbl(temp_percent1))
                        End If

                        ' If (temp_height > desired_height) Then  '  And (temp_height > temp_width) 
                        'temp_percent1 = CDbl(CDbl(desired_height) / CDbl(temp_height))
                        '   temp_width = CDbl(CDbl(temp_width) * CDbl(temp_percent1))
                        '   temp_height = CDbl(CDbl(temp_height) * CDbl(temp_percent1))

                        If HttpContext.Current.Session.Item("isMobile") Then
                            first_picture = ("<div class='panel'><a target=""new"" href=""<a href=""/picture.aspx?url=" + in_AircraftRs.Rows(0).Item("ac_id").ToString + Constants.cHyphen + in_AircraftRs.Rows(0).Item("ac_journ_id").ToString + Constants.cHyphen + fAcpic_id + Constants.cDot + fAcpic_image_type + """ Title='" + fAcpic_subject + "' alt='" + fAcpic_subject + "' height='" & temp_height & "' width='" & temp_width & "' style='padding-bottom:3px;' /></a></div>")
                        Else
                            first_picture = ("<div class='panel'><a href=""javascript:featuredcontentslider.jumpTo('slider1', " & picture_counter & ")""><img border='0' src='" + imgFolder + "/" + in_AircraftRs.Rows(0).Item("ac_id").ToString + Constants.cHyphen + in_AircraftRs.Rows(0).Item("ac_journ_id").ToString + Constants.cHyphen + fAcpic_id + Constants.cDot + fAcpic_image_type + "' Title='" + fAcpic_subject + "' alt='" + fAcpic_subject + "' height='" & temp_height & "' width='" & temp_width & "' style='padding-bottom:3px;' /></a></div>")
                        End If
                        ' Else
                        '  first_picture = ("<div class='panel'><a href=""javascript:featuredcontentslider.jumpTo('slider1', " & picture_counter & ")""><img border='0' src='" + imgFolder + "/" + in_AircraftRs.Rows(0).Item("ac_id").ToString + Constants.cHyphen + in_AircraftRs.Rows(0).Item("ac_journ_id").ToString + Constants.cHyphen + fAcpic_id + Constants.cDot + fAcpic_image_type + "' Title='" + fAcpic_subject + "' alt='" + fAcpic_subject + "' height='100' width='100' style='padding-bottom:3px;' /></a></div>")
                        'End If



                    Catch ex As Exception
                        javascript_slideshow_begining = javascript_slideshow_begining & ("<img border='0' src='" + imgFolder + "/" + in_AircraftRs.Rows(0).Item("ac_id").ToString + Constants.cHyphen + in_AircraftRs.Rows(0).Item("ac_journ_id").ToString + Constants.cHyphen + fAcpic_id + Constants.cDot + fAcpic_image_type + "' Title='" + fAcpic_subject + "' alt='" + fAcpic_subject + "' height='350' />")
                        javascript_slideshow_begining = javascript_slideshow_begining & ("<div class='overlay_bottom_gray'>" + fAcpic_subject + "</div>")
                        javascript_slideshow_begining = javascript_slideshow_begining & ("</div>")

                        If HttpContext.Current.Session.Item("isMobile") Then
                            first_picture = ("<div class='panel'><a target=""new"" href=""/picture.aspx?url=" + in_AircraftRs.Rows(0).Item("ac_id").ToString + Constants.cHyphen + in_AircraftRs.Rows(0).Item("ac_journ_id").ToString + Constants.cHyphen + fAcpic_id + Constants.cDot + fAcpic_image_type + """><img border='0' src='" + imgFolder + "/" + in_AircraftRs.Rows(0).Item("ac_id").ToString + Constants.cHyphen + in_AircraftRs.Rows(0).Item("ac_journ_id").ToString + Constants.cHyphen + fAcpic_id + Constants.cDot + fAcpic_image_type + "' Title='" + fAcpic_subject + "' alt='" + fAcpic_subject + "' height='100' width='100' style='padding-bottom:3px;' /></a></div>")
                        Else
                            first_picture = ("<div class='panel'><a href=""javascript:featuredcontentslider.jumpTo('slider1', " & picture_counter & ")""><img border='0' src='" + imgFolder + "/" + in_AircraftRs.Rows(0).Item("ac_id").ToString + Constants.cHyphen + in_AircraftRs.Rows(0).Item("ac_journ_id").ToString + Constants.cHyphen + fAcpic_id + Constants.cDot + fAcpic_image_type + "' Title='" + fAcpic_subject + "' alt='" + fAcpic_subject + "' height='100' width='100' style='padding-bottom:3px;' /></a></div>")
                        End If


                    End Try





                End If
                javascript_slideshow_ending = ("</td>")

                javascript_slideshow_ending = javascript_slideshow_ending & ("</tr><tr>")

                If bShowOtherLinks And PictureTable.Rows.Count > 0 Then

                    javascript_slideshow_ending = javascript_slideshow_ending & ("<td align='left' valign='top'>") '&nbsp;Additional Images ...<br />")


                    If HttpContext.Current.Session.Item("isMobile") Then
                        javascript_slideshow_ending = javascript_slideshow_ending & ("<table cellpadding='1' cellspacing='0' width=""100%"">")
                        javascript_slideshow_ending = javascript_slideshow_ending & ("<tr><td align='left' valign='top'><div id=""thumbnails""><div class=""contentThumb"">")
                    Else
                        javascript_slideshow_ending = javascript_slideshow_ending & ("<table cellpadding='1' cellspacing='0' width=""100%"">")
                        javascript_slideshow_ending = javascript_slideshow_ending & ("<tr><td align='left' valign='top'><div id=""mygallery"" class=""stepcarousel""><div class=""belt"">")
                    End If

                    javascript_slideshow_ending = javascript_slideshow_ending & first_picture

                    For Each r As DataRow In PictureTable.Rows
                        If picture_counter = 1 Then
                        Else

                            fAcpic_image_type = ""
                            fAcpic_id = ""
                            fAcpic_subject = ""

                            If Not (IsDBNull(r("acpic_image_type"))) Then
                                If Not String.IsNullOrEmpty(r("acpic_image_type").ToString) Then
                                    fAcpic_image_type = r("acpic_image_type").ToString.ToLower.Trim
                                End If
                            End If

                            If Not (IsDBNull(r("acpic_id"))) Then
                                If Not String.IsNullOrEmpty(r("acpic_id").ToString) Then
                                    fAcpic_id = r("acpic_id").ToString.Trim
                                End If
                            End If

                            If Not (IsDBNull(r("acpic_subject"))) Then
                                If Not String.IsNullOrEmpty(r("acpic_subject").ToString) Then
                                    fAcpic_subject = r("acpic_subject").ToString.Trim
                                End If
                            End If

                            theImgFile = imgFolder + "/" + in_AircraftRs.Rows(0).Item("ac_id").ToString + Constants.cHyphen + in_AircraftRs.Rows(0).Item("ac_journ_id").ToString + Constants.cHyphen + fAcpic_id + Constants.cDot + fAcpic_image_type

                            javascript_slideshow_begining = javascript_slideshow_begining & ("<div class='contentdiv' align='center'>")

                            Try
                                ac_image_file = HttpContext.Current.Server.MapPath("pictures\aircraft\") & in_AircraftRs.Rows(0).Item("ac_id").ToString + Constants.cHyphen + in_AircraftRs.Rows(0).Item("ac_journ_id").ToString + Constants.cHyphen + fAcpic_id + Constants.cDot + fAcpic_image_type
                                zimage2 = System.Drawing.Image.FromFile(ac_image_file)
                                temp_width = zimage2.Width
                                temp_height = zimage2.Height
                                desired_width = 510
                                desired_height = 350



                                Call find_image_resize_to_fit(temp_width, temp_height, desired_width, desired_height, javascript_slideshow_begining, in_AircraftRs.Rows(0), imgFolder, fAcpic_id, fAcpic_image_type, fAcpic_subject, "Aircraft", 0, 0)


                                javascript_slideshow_begining = javascript_slideshow_begining & ("<div class='overlay_bottom_gray'>" + fAcpic_subject + "</div>")
                                javascript_slideshow_begining = javascript_slideshow_begining & ("</div>")

                                temp_width = zimage2.Width
                                temp_height = zimage2.Height
                                desired_height = 100
                                desired_width = 100

                                If (temp_height > desired_height) Then  '  And (temp_height > temp_width) 
                                    temp_percent1 = CDbl(CDbl(desired_height) / CDbl(temp_height))
                                    temp_width = CDbl(CDbl(temp_width) * CDbl(temp_percent1))
                                    temp_height = CDbl(CDbl(temp_height) * CDbl(temp_percent1))
                                End If

                                If (temp_width > desired_width) Then
                                    temp_percent1 = CDbl(CDbl(desired_width) / CDbl(temp_width))
                                    temp_width = CDbl(CDbl(temp_width) * CDbl(temp_percent1))
                                    temp_height = CDbl(CDbl(temp_height) * CDbl(temp_percent1))
                                End If


                                'If (temp_height > desired_height) And (temp_height > temp_width) Then
                                '  temp_percent1 = CDbl(CDbl(desired_height) / CDbl(temp_height))
                                '  temp_width = CDbl(CDbl(temp_width) * CDbl(temp_percent1))
                                '  temp_height = CDbl(CDbl(temp_height) * CDbl(temp_percent1))

                                If HttpContext.Current.Session.Item("isMobile") Then
                                    javascript_slideshow_ending = javascript_slideshow_ending & ("<div class='panel'><a target=""new"" href=""/picture.aspx?url=" + in_AircraftRs.Rows(0).Item("ac_id").ToString + Constants.cHyphen + in_AircraftRs.Rows(0).Item("ac_journ_id").ToString + Constants.cHyphen + fAcpic_id + Constants.cDot + fAcpic_image_type + """>")
                                    javascript_slideshow_ending = javascript_slideshow_ending & ("<img border='0' src='" + imgFolder + "/" + in_AircraftRs.Rows(0).Item("ac_id").ToString + Constants.cHyphen + in_AircraftRs.Rows(0).Item("ac_journ_id").ToString + Constants.cHyphen + fAcpic_id + Constants.cDot + fAcpic_image_type + "' Title='" + fAcpic_subject + "' alt='" + fAcpic_subject + "' height='" & temp_height & "' width='" & temp_width & "' style='padding-bottom:3px;' /></a></div>")
                                Else
                                    javascript_slideshow_ending = javascript_slideshow_ending & ("<div class='panel'><a href=""javascript:featuredcontentslider.jumpTo('slider1', " & picture_counter & ")"">")
                                    javascript_slideshow_ending = javascript_slideshow_ending & ("<img border='0' src='" + imgFolder + "/" + in_AircraftRs.Rows(0).Item("ac_id").ToString + Constants.cHyphen + in_AircraftRs.Rows(0).Item("ac_journ_id").ToString + Constants.cHyphen + fAcpic_id + Constants.cDot + fAcpic_image_type + "' Title='" + fAcpic_subject + "' alt='" + fAcpic_subject + "' height='" & temp_height & "' width='" & temp_width & "' style='padding-bottom:3px;' /></a></div>")
                                End If
                                'Else
                                '  javascript_slideshow_ending = javascript_slideshow_ending & ("<div class='panel'><a href=""javascript:featuredcontentslider.jumpTo('slider1', " & picture_counter & ")""><img border='0' src='" + imgFolder + "/" + in_AircraftRs.Rows(0).Item("ac_id").ToString + Constants.cHyphen + in_AircraftRs.Rows(0).Item("ac_journ_id").ToString + Constants.cHyphen + fAcpic_id + Constants.cDot + fAcpic_image_type + "' Title='" + fAcpic_subject + "' alt='" + fAcpic_subject + "' height='100' width='100' style='padding-bottom:3px;' /></a></div>")
                                'End If


                            Catch ex As Exception

                                javascript_slideshow_begining = javascript_slideshow_begining & ("<img border='0' src='" + imgFolder + "/" + in_AircraftRs.Rows(0).Item("ac_id").ToString + Constants.cHyphen + in_AircraftRs.Rows(0).Item("ac_journ_id").ToString + Constants.cHyphen + fAcpic_id + Constants.cDot + fAcpic_image_type + "' Title='" + fAcpic_subject + "' alt='" + fAcpic_subject + "'  height='350' />")
                                javascript_slideshow_begining = javascript_slideshow_begining & ("<div class='overlay_bottom_gray'>" + fAcpic_subject + "</div>")
                                javascript_slideshow_begining = javascript_slideshow_begining & ("</div>")

                                If HttpContext.Current.Session.Item("isMobile") Then
                                    javascript_slideshow_ending = javascript_slideshow_ending & ("<div class='panel'><a target=""new"" href=""/picture.aspx?url=" + in_AircraftRs.Rows(0).Item("ac_id").ToString + Constants.cHyphen + in_AircraftRs.Rows(0).Item("ac_journ_id").ToString + Constants.cHyphen + fAcpic_id + Constants.cDot + fAcpic_image_type + """><img border='0' src='" + imgFolder + "/" + in_AircraftRs.Rows(0).Item("ac_id").ToString + Constants.cHyphen + in_AircraftRs.Rows(0).Item("ac_journ_id").ToString + Constants.cHyphen + fAcpic_id + Constants.cDot + fAcpic_image_type + "' Title='" + fAcpic_subject + "' alt='" + fAcpic_subject + "' height='100' width='100' style='padding-bottom:3px;' /></a></div>")
                                Else
                                    javascript_slideshow_ending = javascript_slideshow_ending & ("<div class='panel'><a href=""javascript:featuredcontentslider.jumpTo('slider1', " & picture_counter & ")""><img border='0' src='" + imgFolder + "/" + in_AircraftRs.Rows(0).Item("ac_id").ToString + Constants.cHyphen + in_AircraftRs.Rows(0).Item("ac_journ_id").ToString + Constants.cHyphen + fAcpic_id + Constants.cDot + fAcpic_image_type + "' Title='" + fAcpic_subject + "' alt='" + fAcpic_subject + "' height='100' width='100' style='padding-bottom:3px;' /></a></div>")
                                End If

                            End Try





                        End If
                        picture_counter += 1
                    Next
                    javascript_slideshow_ending = javascript_slideshow_ending & "</div></div></td>"
                    javascript_slideshow_begining = javascript_slideshow_begining & ("</div>")
                    htmlOut.Append(javascript_slideshow_begining)
                    htmlOut.Append(javascript_slideshow_ending)
                    htmlOut.Append("</tr>")
                    htmlOut.Append("</table>")
                End If

            Else
                ' htmlOut.Append("nothing here")
                aircraft_picture_slideshow.CssClass = "display_none"
                aircraft_picture_slideshow.Visible = False
            End If

        End If

        PictureTable = New DataTable

        'If crmView = False Then
        If Not MySesState.Item("localSubscription").crmAerodexFlag And CLng(in_AircraftRs.Rows(0).Item("ac_journ_id").ToString) = 0 Then
            ' htmlOut.Append("<a href='#' title='Single Page Customer Spec Sheet'><img src='images/single_page_spec.jpg' alt='Single Page Spec Sheet' border='0' width='125' class='spec_image' /></a>")
            ' htmlOut.Append("<a href='#' title='Condensed Customer Spec Sheet'><img src='images/condensed_spec_sheet.jpg' alt='Condensed Customer Spec Sheet' width='125' border='0' class='spec_image' /></a>")
            ' htmlOut.Append("<a href='#' title='Full View Customer Spec Sheet'><img src='images/full_spec.jpg' alt='Full Spec Sheet' border='0' width='125' class='spec_image' /></a>")
            htmlOut.Append(view_all_pictures)
        End If
        'End If

        htmlOut.Append("</td></tr>")
        htmlOut.Append("</table></div>")

        Return htmlOut.ToString.Trim

    End Function

    Public Shared Function find_image_resize_to_fit(ByVal temp_width As Integer, ByVal temp_height As Integer, ByVal desired_width As Integer, ByVal desired_height As Integer, ByRef javascript_slideshow_begining As String, ByVal r As DataRow, ByVal imgFolder As String, ByVal fAcpic_id As String, ByVal fAcpic_image_type As String, ByVal fAcpic_subject As String, ByVal image_type As String, ByVal yacht_id As Long, ByVal journ_id As Long, Optional ByVal company_id As Long = 0)
        find_image_resize_to_fit = ""

        Dim temp_calc As Double = 0.0
        Dim temp_percent1 As Double = 0.0

        If temp_width > temp_height Then
            ' if the image is wider then the desired image width, then shirnk down to size.

            If (temp_width < desired_width) And (temp_height < desired_height) Then
                temp_calc = (temp_height / temp_width)
                If (temp_calc <= 0.7) Then  ' this is the ratio of the box, less than means just set width
                    'just force width, height will be fine
                    If Trim(image_type) = "Aircraft" Then
                        javascript_slideshow_begining = javascript_slideshow_begining & ("<img border='0' src='" + imgFolder + "/" + r("ac_id").ToString + Constants.cHyphen + r("ac_journ_id").ToString + Constants.cHyphen + fAcpic_id + Constants.cDot + fAcpic_image_type + "' Title='" + fAcpic_subject + "' alt='" + fAcpic_subject + "' width='" & desired_width & "' />")
                    ElseIf company_id > 0 Then
                        javascript_slideshow_begining = javascript_slideshow_begining & ("<img border='0' src='" + imgFolder + "/" & company_id.ToString + Constants.cDot + fAcpic_image_type + "' Title='" + fAcpic_subject + "' alt='" + fAcpic_subject + "' width='" & desired_width & "' />")
                    Else
                        javascript_slideshow_begining = javascript_slideshow_begining & ("<img border='0' src='" + imgFolder + "/" & yacht_id.ToString & Constants.cHyphen & journ_id & Constants.cHyphen + fAcpic_id + Constants.cDot + fAcpic_image_type + "' Title='" + fAcpic_subject + "' alt='" + fAcpic_subject + "' width='" & desired_width & "' />")
                    End If
                Else
                    temp_calc = (desired_height / temp_height)
                    temp_width = (temp_width * temp_calc)
                    If Trim(image_type) = "Aircraft" Then
                        javascript_slideshow_begining = javascript_slideshow_begining & ("<img border='0' src='" + imgFolder + "/" + r("ac_id").ToString + Constants.cHyphen + r("ac_journ_id").ToString + Constants.cHyphen + fAcpic_id + Constants.cDot + fAcpic_image_type + "' Title='" + fAcpic_subject + "' alt='" + fAcpic_subject + "' width='" & temp_width & "' />")
                    ElseIf company_id > 0 Then
                        javascript_slideshow_begining = javascript_slideshow_begining & ("<img border='0' src='" + imgFolder + "/" + company_id.ToString & Constants.cDot + fAcpic_image_type + "' Title='" + fAcpic_subject + "' alt='" + fAcpic_subject + "' width='" & temp_width & "' />")
                    Else
                        javascript_slideshow_begining = javascript_slideshow_begining & ("<img border='0' src='" + imgFolder + "/" + yacht_id.ToString & Constants.cHyphen & journ_id.ToString & Constants.cHyphen & fAcpic_id & Constants.cDot + fAcpic_image_type + "' Title='" + fAcpic_subject + "' alt='" + fAcpic_subject + "' width='" & temp_width & "' />")
                    End If

                End If
            Else
                If temp_width > desired_width Then
                    temp_percent1 = CDbl(CDbl(desired_width) / CDbl(temp_width))
                    temp_width = CDbl(CDbl(temp_width) * CDbl(temp_percent1))
                    temp_height = CDbl(CDbl(temp_height) * CDbl(temp_percent1))
                End If

                'assuming generally that a square is fine
                If temp_height > desired_height Then
                    temp_percent1 = CDbl(CDbl(desired_height) / CDbl(temp_height))
                    temp_width = CDbl(CDbl(temp_width) * CDbl(temp_percent1))
                    temp_height = CDbl(CDbl(temp_height) * CDbl(temp_percent1))
                End If

                If Trim(image_type) = "Aircraft" Then
                    javascript_slideshow_begining = javascript_slideshow_begining & ("<img border='0' src='" + imgFolder + "/" + r("ac_id").ToString + Constants.cHyphen + r("ac_journ_id").ToString + Constants.cHyphen + fAcpic_id + Constants.cDot + fAcpic_image_type + "' Title='" + fAcpic_subject + "' alt='" + fAcpic_subject + "' width='" & temp_width & "'  height='" & temp_height & "'/>")
                ElseIf company_id > 0 Then
                    javascript_slideshow_begining = javascript_slideshow_begining & ("<img border='0' src='" + imgFolder + "/" + company_id.ToString + Constants.cDot + fAcpic_image_type + "' Title='" + fAcpic_subject + "' alt='" + fAcpic_subject + "' width='" & temp_width & "'  height='" & temp_height & "'/>")
                Else
                    javascript_slideshow_begining = javascript_slideshow_begining & ("<img border='0' src='" + imgFolder + "/" + yacht_id.ToString + Constants.cHyphen + journ_id.ToString + Constants.cHyphen + fAcpic_id + Constants.cDot + fAcpic_image_type + "' Title='" + fAcpic_subject + "' alt='" + fAcpic_subject + "' width='" & temp_width & "'  height='" & temp_height & "'/>")
                End If

            End If


        ElseIf temp_height > temp_width Then

            If (temp_width < desired_width) And (temp_height < desired_height) Then
                temp_calc = (temp_width / temp_height)
                If (temp_calc <= 0.7) Then  ' this is the ratio of the box, less than means just set width
                    'just force width, height will be fine
                    If Trim(image_type) = "Aircraft" Then
                        javascript_slideshow_begining = javascript_slideshow_begining & ("<img border='0' src='" + imgFolder + "/" + r("ac_id").ToString + Constants.cHyphen + r("ac_journ_id").ToString + Constants.cHyphen + fAcpic_id + Constants.cDot + fAcpic_image_type + "' Title='" + fAcpic_subject + "' alt='" + fAcpic_subject + "' width='" & temp_width & "' />")
                    ElseIf company_id > 0 Then
                        javascript_slideshow_begining = javascript_slideshow_begining & ("<img border='0' src='" + imgFolder + "/" + company_id.ToString + Constants.cDot + fAcpic_image_type + "' Title='" + fAcpic_subject + "' alt='" + fAcpic_subject + "' width='" & temp_width & "' />")
                    Else
                        javascript_slideshow_begining = javascript_slideshow_begining & ("<img border='0' src='" + imgFolder + "/" + yacht_id.ToString + Constants.cHyphen + journ_id.ToString + Constants.cHyphen + fAcpic_id + Constants.cDot + fAcpic_image_type + "' Title='" + fAcpic_subject + "' alt='" + fAcpic_subject + "' width='" & temp_width & "' />")
                    End If
                Else
                    temp_calc = (desired_height / temp_height)
                    temp_width = (temp_width * temp_calc)
                    If Trim(image_type) = "Aircraft" Then
                        javascript_slideshow_begining = javascript_slideshow_begining & ("<img border='0' src='" + imgFolder + "/" + r("ac_id").ToString + Constants.cHyphen + r("ac_journ_id").ToString + Constants.cHyphen + fAcpic_id + Constants.cDot + fAcpic_image_type + "' Title='" + fAcpic_subject + "' alt='" + fAcpic_subject + "' width='" & temp_width & "' />")
                    ElseIf company_id > 0 Then
                        javascript_slideshow_begining = javascript_slideshow_begining & ("<img border='0' src='" + imgFolder + "/" + company_id.ToString + Constants.cDot + fAcpic_image_type + "' Title='" + fAcpic_subject + "' alt='" + fAcpic_subject + "' width='" & temp_width & "' />")
                    Else
                        javascript_slideshow_begining = javascript_slideshow_begining & ("<img border='0' src='" + imgFolder + "/" + yacht_id.ToString + Constants.cHyphen + journ_id.ToString + Constants.cHyphen + fAcpic_id + Constants.cDot + fAcpic_image_type + "' Title='" + fAcpic_subject + "' alt='" + fAcpic_subject + "' width='" & temp_width & "' />")
                    End If

                End If
            Else
                If temp_height > desired_height Then
                    temp_percent1 = CDbl(CDbl(desired_height) / CDbl(temp_height))
                    temp_width = CDbl(CDbl(temp_width) * CDbl(temp_percent1))
                    temp_height = CDbl(CDbl(temp_height) * CDbl(temp_percent1))
                End If

                ' if the image is wider then the desired image width, then shirnk down to size.
                If temp_width > desired_width Then
                    temp_percent1 = CDbl(CDbl(desired_width) / CDbl(temp_width))
                    temp_width = CDbl(CDbl(temp_width) * CDbl(temp_percent1))
                    temp_height = CDbl(CDbl(temp_height) * CDbl(temp_percent1))
                End If

                If Trim(image_type) = "Aircraft" Then
                    javascript_slideshow_begining = javascript_slideshow_begining & ("<img border='0' src='" + imgFolder + "/" + r("ac_id").ToString + Constants.cHyphen + r("ac_journ_id").ToString + Constants.cHyphen + fAcpic_id + Constants.cDot + fAcpic_image_type + "' Title='" + fAcpic_subject + "' alt='" + fAcpic_subject + "' width='" & temp_width & "'  height='" & temp_height & "'/>")
                ElseIf company_id > 0 Then
                    javascript_slideshow_begining = javascript_slideshow_begining & ("<img border='0' src='" + imgFolder + "/" + company_id.ToString + Constants.cDot + fAcpic_image_type + "' Title='" + fAcpic_subject + "' alt='" + fAcpic_subject + "' width='" & temp_width & "'  height='" & temp_height & "'/>")
                Else
                    javascript_slideshow_begining = javascript_slideshow_begining & ("<img border='0' src='" + imgFolder + "/" + yacht_id.ToString + Constants.cHyphen + journ_id.ToString + Constants.cHyphen + fAcpic_id + Constants.cDot + fAcpic_image_type + "' Title='" + fAcpic_subject + "' alt='" + fAcpic_subject + "' width='" & temp_width & "'  height='" & temp_height & "'/>")
                End If
            End If

        Else ' they are equal height and width

            'assuming generally that a square is fine
            If temp_height > desired_height Then
                temp_percent1 = CDbl(CDbl(temp_width) / CDbl(temp_height))
                temp_width = CDbl(CDbl(temp_width) * CDbl(temp_percent1))
                temp_height = CDbl(CDbl(temp_height) * CDbl(temp_percent1))
                If Trim(image_type) = "Aircraft" Then
                    javascript_slideshow_begining = javascript_slideshow_begining & ("<img border='0' src='" + imgFolder + "/" + r("ac_id").ToString + Constants.cHyphen + r("ac_journ_id").ToString + Constants.cHyphen + fAcpic_id + Constants.cDot + fAcpic_image_type + "' Title='" + fAcpic_subject + "' alt='" + fAcpic_subject + "' width='" & temp_width & "'  height='" & temp_height & "'/>")
                ElseIf company_id > 0 Then
                    javascript_slideshow_begining = javascript_slideshow_begining & ("<img border='0' src='" + imgFolder + "/" + company_id.ToString + Constants.cDot + fAcpic_image_type + "' Title='" + fAcpic_subject + "' alt='" + fAcpic_subject + "' width='" & temp_width & "'  height='" & temp_height & "'/>")
                Else
                    javascript_slideshow_begining = javascript_slideshow_begining & ("<img border='0' src='" + imgFolder + "/" + yacht_id.ToString + Constants.cHyphen + journ_id.ToString + Constants.cHyphen + fAcpic_id + Constants.cDot + fAcpic_image_type + "' Title='" + fAcpic_subject + "' alt='" + fAcpic_subject + "' width='" & temp_width & "'  height='" & temp_height & "'/>")
                End If

            Else
                If Trim(image_type) = "Aircraft" Then
                    javascript_slideshow_begining = javascript_slideshow_begining & ("<img border='0' src='" + imgFolder + "/" + r("ac_id").ToString + Constants.cHyphen + r("ac_journ_id").ToString + Constants.cHyphen + fAcpic_id + Constants.cDot + fAcpic_image_type + "' Title='" + fAcpic_subject + "' alt='" + fAcpic_subject + "' width='350' />")
                ElseIf company_id > 0 Then
                    javascript_slideshow_begining = javascript_slideshow_begining & ("<img border='0' src='" + imgFolder + "/" + company_id.ToString + Constants.cDot + fAcpic_image_type + "' Title='" + fAcpic_subject + "' alt='" + fAcpic_subject + "' width='350' />")
                Else
                    javascript_slideshow_begining = javascript_slideshow_begining & ("<img border='0' src='" + imgFolder + "/" + yacht_id.ToString + Constants.cHyphen + journ_id.ToString + Constants.cHyphen + fAcpic_id + Constants.cDot + fAcpic_image_type + "' Title='" + fAcpic_subject + "' alt='" + fAcpic_subject + "' width='350' />")
                End If

            End If

        End If
    End Function
    ''' <summary>
    ''' Very simple function that takes in a reference of a datatable and builds the tab container.
    ''' </summary>
    ''' <param name="MySesState">Session State object</param>
    ''' <param name="in_AircraftRs">Datatable reference (filled with the AC information).</param>
    ''' <param name="apu_tab">APU tab panel object allows setting the header text.</param>
    ''' <returns>Text</returns>
    ''' <remarks></remarks>
    Public Shared Function DisplayAPUDetails(ByRef MySesState As HttpSessionState,
                                          ByRef in_AircraftRs As DataTable,
                                          ByRef apu_tab As Object, ByVal aclsData_Temp As clsData_Manager_SQL, ByRef bShowBlankAcFields As Boolean) As String

        Dim htmlOut As StringBuilder = New StringBuilder()
        Dim DisplayField As String = ""


        If Not IsNothing(apu_tab) Then
            apu_tab.headerText = ""
        End If

        If Not IsNothing(apu_tab) Then
            If Not IsDBNull(in_AircraftRs.Rows(0).Item("ac_apu_model_name")) Then
                If Not String.IsNullOrEmpty(in_AircraftRs.Rows(0).Item("ac_apu_model_name")) Then
                    If apu_tab.headerText = "" Then
                        apu_tab.headerText = "APU: MODEL "
                    End If
                    apu_tab.headerText += " " + in_AircraftRs.Rows(0).Item("ac_apu_model_name").ToString
                End If
            End If
        End If

        If Not IsNothing(apu_tab) Then
            If apu_tab.headerText = "" Then
                apu_tab.headerText = "APU: "
            End If
        End If

        '''''''APU Maintenance Plan Field

        If Not IsDBNull(in_AircraftRs.Rows(0).Item("ac_apu_maintance_program")) And Not String.IsNullOrEmpty(in_AircraftRs.Rows(0).Item("ac_apu_maintance_program").ToString) Then
            DisplayField = in_AircraftRs.Rows(0).Item("ac_apu_maintance_program").ToString
        Else
            If bShowBlankAcFields = True Then
                DisplayField = "Unknown"
            End If
        End If

        If bShowBlankAcFields Or DisplayField <> "" Then
            DisplayField = "<tr><td valign='top' align='left' class='header'>Maintenance Plan:</td><td valign='top' align='left'>" & DisplayField & "</td></tr>"
        End If

        htmlOut.Append(DisplayField)
        DisplayField = ""

        'Serial # Field
        If Not IsDBNull(in_AircraftRs.Rows(0).Item("ac_apu_ser_nbr")) Then
            If Not String.IsNullOrEmpty(in_AircraftRs.Rows(0).Item("ac_apu_ser_nbr")) Then
                htmlOut.Append("<tr><td align='left' valign='top'  class='header' width=""150"">Serial Number</td>")
                htmlOut.Append("<td align='left' valign='top'>")
                htmlOut.Append(in_AircraftRs.Rows(0).Item("ac_apu_ser_nbr").ToString)
                htmlOut.Append("</td>")
                htmlOut.Append("</tr>")
            End If
        End If



        '''''''Total Time (Hours) Since New Field
        If Not IsDBNull(in_AircraftRs.Rows(0).Item("ac_apu_ttsn_hours")) And Not String.IsNullOrEmpty(in_AircraftRs.Rows(0).Item("ac_apu_ttsn_hours").ToString) Then
            DisplayField = FormatNumber(CDbl(in_AircraftRs.Rows(0).Item("ac_apu_ttsn_hours").ToString), 0, True, False, True) + ""
            If bShowBlankAcFields = False Then
                If DisplayField = "0" Then
                    DisplayField = ""
                End If
            End If
        End If

        If bShowBlankAcFields Or DisplayField <> "" Then
            DisplayField = "<tr><td valign='top' align='left' class='header' width=""160"">Total Time (Hours) Since New:</td><td valign='top' align='left'>" & DisplayField & "</td></tr>"
        End If

        htmlOut.Append(DisplayField)
        DisplayField = ""
        ''''''' End Total Time (Hours) Since New Field

        '''''''Since Overhaul (SOH) Hour Field
        If Not IsDBNull(in_AircraftRs.Rows(0).Item("ac_apu_tsoh_hours")) And Not String.IsNullOrEmpty(in_AircraftRs.Rows(0).Item("ac_apu_tsoh_hours").ToString) Then
            DisplayField = FormatNumber(CDbl(in_AircraftRs.Rows(0).Item("ac_apu_tsoh_hours").ToString), 0, True, False, True) + ""
            If bShowBlankAcFields = False Then
                If DisplayField = "0" Then
                    DisplayField = ""
                End If
            End If
        End If

        If bShowBlankAcFields Or DisplayField <> "" Then
            DisplayField = "<tr><td valign='top' align='left' class='header' width=""160"">Since Overhaul (SOH) Hours:</td><td valign='top' align='left'>" & DisplayField & "</td></tr>"
        End If

        htmlOut.Append(DisplayField)
        DisplayField = ""
        '''''''End Since Overhaul (SOH) Hour Field

        '''''''Since Hot Inspection (SHI) Hours
        If Not IsDBNull(in_AircraftRs.Rows(0).Item("ac_apu_tshi_hours")) And Not String.IsNullOrEmpty(in_AircraftRs.Rows(0).Item("ac_apu_tshi_hours").ToString) Then
            DisplayField = FormatNumber(CDbl(in_AircraftRs.Rows(0).Item("ac_apu_tshi_hours").ToString), 0, True, False, True) + ""
        End If

        If bShowBlankAcFields Or DisplayField <> "" Then
            DisplayField = "<tr><td valign='top' align='left' class='header' width=""160"">Since Hot Inspection (SHI) Hours:</td><td valign='top' align='left'>" & DisplayField & "</td></tr>"
        End If

        htmlOut.Append(DisplayField)
        DisplayField = ""
        '''''''End Since Hot Inspection (SHI) Hours

        DisplayField = htmlOut.ToString
        htmlOut = New StringBuilder

        If DisplayField = "" Then
            htmlOut.Append("<table width='100%' cellpadding='0' cellspacing='0'>")
            DisplayField = "<tr><td align=""left"" valign=""top""><span class=""li"">No APU Details</li></td></tr>"
        Else
            htmlOut.Append("<table width='100%' cellpadding='3' cellspacing='0' border='0' class='engine_tab'>")
        End If



        htmlOut.Append(DisplayField)
        htmlOut.Append("</table>")

        Return htmlOut.ToString.Trim

    End Function

    ''' <summary>
    ''' Displays the registration Date from an ac data table
    ''' </summary>
    ''' <param name="AircraftTable"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetRegExpireDate(ByRef AircraftTable As DataTable) As String
        Dim htmlOut As StringBuilder = New StringBuilder()


        If Not IsDBNull(AircraftTable.Rows(0).Item("ac_reg_no_expiration_date")) Then
            If Not String.IsNullOrEmpty(AircraftTable.Rows(0).Item("ac_reg_no_expiration_date").ToString) Then
                htmlOut.Append("&nbsp;<span class='tiny'>(<em>Expires: " + Format(AircraftTable.Rows(0).Item("ac_reg_no_expiration_date"), "MM/dd/yy") + "</em>)</span>")
            End If
        End If


        Return htmlOut.ToString.Trim

    End Function


    Public Shared Function DisplayLeaseDetails(ByRef in_AircraftRs As DataTable, ByVal aclsData_Temp As clsData_Manager_SQL) As String

        Dim LeaseData As New DataTable
        Dim htmlOut As StringBuilder = New StringBuilder()
        Dim sQuery As String = ""


        LeaseData = aclsData_Temp.GetAircraft_Lease_acID_ExpFlag(in_AircraftRs.Rows(0).Item("ac_id"), "N", in_AircraftRs.Rows(0).Item("ac_journ_id"))
        If Not IsNothing(LeaseData) Then

            If LeaseData.Rows.Count > 0 Then
                htmlOut.Append("<div class=""Box""><table class='formatTable blue' cellpadding='0' cellspacing='0' align=""center"" width='100%'>")
                htmlOut.Append("<tr class=""noBorder""><td align=""left"" align=""top"" colspan=""3""><div class=""subHeader"">LEASE DETAILS</div><br /></td></tr>")
                htmlOut.Append("<tr><th class='bottom'>Type</th><th class='bottom'>Term</th><th class='bottom'>Expiration Date</th><th class='bottom'>Expiration Confirmed</th></tr>")

                For Each r As DataRow In LeaseData.Rows

                    If Not IsDBNull(r("aclease_type")) And Not String.IsNullOrEmpty(r("aclease_type").ToString) Then
                        htmlOut.Append("<tr><td valign='middle' align='center'>" + r("aclease_type").ToString.Trim + "</td>")
                    Else
                        htmlOut.Append("<tr><td valign='middle' align='center'>&nbsp;</td>")
                    End If

                    If Not IsDBNull(r("aclease_term")) And Not String.IsNullOrEmpty(r("aclease_term").ToString) Then
                        htmlOut.Append("<td valign='middle' align='center'>" + r("aclease_term").ToString.Trim + "</td>")
                    Else
                        htmlOut.Append("<td valign='middle' align='center'>&nbsp;</td>")
                    End If

                    If Not IsDBNull(r("aclease_date_expiration")) And Not String.IsNullOrEmpty(r("aclease_date_expiration").ToString) Then
                        htmlOut.Append("<td valign='middle' align='center'>" + FormatDateTime(r("aclease_date_expiration").ToString, DateFormat.ShortDate) + "</td>")
                    Else
                        htmlOut.Append("<td valign='middle' align='center'>&nbsp;</td>")
                    End If

                    If Not IsDBNull(r("aclease_date_expiration_confirmed")) And Not String.IsNullOrEmpty(r("aclease_date_expiration_confirmed").ToString) Then
                        htmlOut.Append("<td valign='middle' align='center'>" + FormatDateTime(r("aclease_date_expiration_confirmed").ToString, DateFormat.ShortDate) + "</td></tr>")
                    Else
                        htmlOut.Append("<td valign='middle' align='center'>&nbsp;</td></tr>")
                    End If


                    If Not IsDBNull(r("aclease_note")) And Not String.IsNullOrEmpty(r("aclease_note").ToString) Then
                        htmlOut.Append("<tr><td colspan='4' valign='middle' align='left'><strong>Notes</strong>&nbsp;:&nbsp;")
                        htmlOut.Append(r("aclease_note").ToString + "</td></tr>")
                    Else
                    End If

                Next

                htmlOut.Append("</table></div>")
            End If
        End If

        LeaseData = Nothing

        Return htmlOut.ToString.Trim

    End Function
    Public Shared Function checkForFlightData(ByVal PassedRegNO As String, ByVal aclsData_Temp As clsData_Manager_SQL, Optional ByVal use_barr_list As Boolean = False) As Boolean
        Dim lData As New DataTable
        Dim bResult As Boolean = False

        lData = aclsData_Temp.EvoCheckFlightActivity(PassedRegNO, use_barr_list)
        If Not IsNothing(lData) Then
            If lData.Rows.Count > 0 Then
                bResult = True
            End If
        End If
        Return bResult

    End Function
    Public Shared Function checkForBarred_AC(ByVal PassedRegNO As String, ByVal aclsData_Temp As clsData_Manager_SQL) As Boolean
        Dim lData As New DataTable
        Dim bResult As Boolean = False

        lData = aclsData_Temp.EvoCheckifBarred(PassedRegNO)
        If Not IsNothing(lData) Then
            If lData.Rows.Count > 0 Then
                bResult = True
            End If
        End If
        Return bResult

    End Function
    Public Shared Function DisplayFlightData(ByRef in_AircraftRs As DataTable, ByVal crmView As Boolean, ByVal aclsData_Temp As clsData_Manager_SQL) As String

        Dim htmlOut As StringBuilder = New StringBuilder()
        Dim sQuery As String = ""
        Dim FlightTable As New DataTable
        Dim BGColor As String = "engine_1"

        Dim totalMiles As Long = 0
        Dim totalFlightTime As Long = 0

        Dim averageMiles As Long = 0
        Dim averageFlightTime As Long = 0

        Dim totalFlights As Long = 0
        Dim bPreviousFlag As Boolean = False

        htmlOut.Append("<table class='engine_tab' cellpadding='2' cellspacing='0' width='100%'>")
        htmlOut.Append("<tr class='small_links'><th valign='middle' align='center' colspan='5'><a href='http://www.traqpak.com' target='_new'>Powered by - ARG/US TRAQPak<a/>&nbsp;&nbsp;")
        If crmView = False Then
            htmlOut.Append("<a href=""#"" onclick=""javascript:load('http://v2.traqpak.com/JetNET/JetNET/TP_90Summary?key=" & crmWebClient.clsGeneral.clsGeneral.EncodeBase64(in_AircraftRs.Rows(0).Item("ac_reg_nbr").ToString.Trim) & "','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');return false;"">View Activity Map<a/>&nbsp;&nbsp;")
            'htmlOut.Append("<a href='argUS.aspx?regnumber=" + in_AircraftRs.Rows(0).Item("ac_reg_nbr").ToString.Trim + "' target='_new'>View Activity Map<a/>&nbsp;&nbsp;")
        End If
        htmlOut.Append("<a href='" & HttpContext.Current.Session.Item("JetnetDomainReference") & "/help/TRAQPak_faq.pdf' target='_new'>TRAQPak FAQs<a/>")

        htmlOut.Append("</th></tr>")

        htmlOut.Append("<tr><td class='header gray_head' valign='middle' align='center'><strong>Date</strong></td>")
        htmlOut.Append("<td class='header gray_head' valign='middle' align='center'><strong>Origin</strong></td>")
        htmlOut.Append("<td class='header gray_head' valign='middle' align='center'><strong>Destination</strong></td>")
        htmlOut.Append("<td class='header gray_head' valign='middle' align='center'><strong>Dist.</strong><em>(nm)</em></td>")
        htmlOut.Append("<td class='header gray_head' valign='middle' align='center' ><strong>Time</strong><em>(min)</em></td></tr>")
        FlightTable = aclsData_Temp.Aircraft_Flight_Results(in_AircraftRs.Rows(0).Item("ac_reg_no_search").ToString.Trim, DateAdd(DateInterval.Day, -90, Now()))

        If Not IsNothing(FlightTable) Then

            totalFlights = 0

            For Each r As DataRow In FlightTable.Rows

                If BGColor = "engine_1" Then
                    BGColor = "engine_5"
                Else
                    BGColor = "engine_1"
                End If

                If Not IsDBNull(r("aractivity_date_depart")) And Not IsDBNull(in_AircraftRs.Rows(0).Item("ac_date_purchased")) Then
                    If Not String.IsNullOrEmpty(r("aractivity_date_depart").ToString) And Not String.IsNullOrEmpty(in_AircraftRs.Rows(0).Item("ac_date_purchased").ToString) Then
                        If Not bPreviousFlag And (CDate(FormatDateTime(r("aractivity_date_depart").ToString, DateFormat.ShortDate)) <= CDate(FormatDateTime(in_AircraftRs.Rows(0).Item("ac_date_purchased").ToString, DateFormat.ShortDate))) Then
                            bPreviousFlag = True

                            htmlOut.Append("<tr><td valign='middle' align='middle' colspan='5'  class='" & BGColor & "'><strong>Flight Activity before this line is ACTIVITY PRIOR TO PURCHASE</strong></td></tr>")
                            If BGColor = "engine_1" Then
                                BGColor = "engine_5"
                            Else
                                BGColor = "engine_1"
                            End If

                            htmlOut.Append("<tr><td valign='middle' align='left' colspan='5'  class='" & BGColor & "'><hr /></td></tr>")
                            If BGColor = "engine_1" Then
                                BGColor = "engine_5"
                            Else
                                BGColor = "engine_1"
                            End If
                        End If
                    End If
                End If

                htmlOut.Append("<tr class='" & BGColor & "'>")

                If Not IsDBNull(r("aractivity_date_depart")) And Not String.IsNullOrEmpty(r("aractivity_date_depart").ToString) Then
                    htmlOut.Append("<td valign='middle' align='left' class='" & BGColor & "'>" + FormatDateTime(r("aractivity_date_depart").ToString, DateFormat.ShortDate) + "</td>")
                Else
                    htmlOut.Append("<td valign='middle' align='left' class='" & BGColor & "'>&nbsp;</td>")
                End If

                If Not IsDBNull(r("origin")) And Not String.IsNullOrEmpty(r("origin").ToString) Then
                    htmlOut.Append("<td valign='middle' align='left' class='" & BGColor & "'>" + r("origin").ToString.Trim + "</td>")
                Else
                    htmlOut.Append("<td valign='middle' align='left' class='" & BGColor & "'>&nbsp;</td>")
                End If

                If Not IsDBNull(r("destination")) And Not String.IsNullOrEmpty(r("destination").ToString) Then
                    htmlOut.Append("<td valign='middle' align='left' class='" & BGColor & "'>" + r("destination").ToString.Trim + "</td>")
                Else
                    htmlOut.Append("<td valign='middle' align='left' class='" & BGColor & "'>&nbsp;</td>")
                End If

                If Not IsDBNull(r("aractivity_distance")) Then
                    If CLng(r("aractivity_distance").ToString) > 0 Then
                        htmlOut.Append("<td valign='middle' align='right' class='" & BGColor & "'>" + FormatNumber(r("aractivity_distance").ToString, 0, True, False, True) + "</td>")
                        totalMiles += CLng(r("aractivity_distance").ToString)
                    Else
                        htmlOut.Append("<td valign='middle' align='right' class='" & BGColor & "'>0</td>")
                    End If
                Else
                    htmlOut.Append("<td valign='middle' align='right' class='" & BGColor & "'>&nbsp;</td>")
                End If

                If Not IsDBNull(r("aractivity_flight_time")) Then
                    If CLng(r("aractivity_flight_time").ToString) > 0 Then
                        htmlOut.Append("<td valign='middle' align='right' class='" & BGColor & "'>" + FormatNumber(r("aractivity_flight_time").ToString, 0, True, False, True) + "</td>")
                        totalFlightTime += CLng(r("aractivity_flight_time").ToString)
                    Else
                        htmlOut.Append("<td valign='middle' align='right' class='" & BGColor & "'>0</td>")
                    End If
                Else
                    htmlOut.Append("<td valign='middle' align='right' class='" & BGColor & "'>&nbsp;</td>")
                End If

                htmlOut.Append("</tr>")
                totalFlights += 1
            Next



            If totalFlights > 0 Then
                averageMiles = (totalMiles / totalFlights)
                averageFlightTime = (totalFlightTime / totalFlights)
            End If
            htmlOut.Append("</tr></table>")
            htmlOut.Append("<table class='engine_tab' cellpadding='2' cellspacing='0' width='100%'>")
            htmlOut.Append("<tr><td valign='top' align='left' rowspan='2' class='header answer'><strong>Total&nbsp;Flights</strong>&nbsp;:&nbsp;" + FormatNumber(totalFlights, 0, True, False, True) + "</td>")
            htmlOut.Append("<td valign='top' align='left' class='header answer'><strong>Total&nbsp;Miles</strong>&nbsp;:&nbsp;" + FormatNumber(totalMiles, 0, True, False, True) + "<em>(nm)</em></td>")
            htmlOut.Append("<td valign='top' align='left' class='header answer'><strong>Total&nbsp;Flight&nbsp;Time</strong>&nbsp;:&nbsp;" + FormatNumber(totalFlightTime, 0, True, False, True) + "<em>(min)</em></td></tr>")
            htmlOut.Append("<tr><td valign='top' align='left' class='header answer'><strong>Average&nbsp;Miles</strong>&nbsp;:&nbsp;" + FormatNumber(averageMiles, 0, True, False, True) + "<em>(nm)</em></td>")
            htmlOut.Append("<td valign='top' align='left' class='header answer'><strong>Average&nbsp;Flight&nbsp;Time</strong>&nbsp;:&nbsp;" + FormatNumber(averageFlightTime, 0, True, False, True) + "<em>(min)</em></td></tr>")

        Else
            htmlOut.Append("<tr><th valign='middle' align='left' colspan='5'>No Flight Data Available for the last 90 days.</th></tr>")
        End If

        htmlOut.Append("</table>")


        Return htmlOut.ToString.Trim

    End Function
    Public Shared Function GetLifeCycleStage(ByVal in_AircraftStage As Long, ByVal aclsData_Temp As clsData_Manager_SQL) As String
        Dim LifeCycleTable As New DataTable
        Dim htmlOut As StringBuilder = New StringBuilder()
        Dim sQuery As String = ""

        If in_AircraftStage <> 0 Then
            LifeCycleTable = aclsData_Temp.GetLifeCycleStage(in_AircraftStage)

            If Not IsNothing(LifeCycleTable) Then

                If LifeCycleTable.Rows.Count > 0 Then
                    If Not IsDBNull(LifeCycleTable.Rows(0).Item("acs_name")) Then
                        If Not String.IsNullOrEmpty(LifeCycleTable.Rows(0).Item("acs_name").ToString) Then
                            htmlOut.Append(LifeCycleTable.Rows(0).Item("acs_name").ToString)
                        End If
                    End If

                End If
            End If
        End If

        If String.IsNullOrEmpty(htmlOut.ToString) Then
            htmlOut.Append("&lt;Unknown&gt;")
        End If

        Return htmlOut.ToString.Trim

    End Function
    Public Shared Function HasAircraftEvents(ByVal in_AircraftID As Long, ByVal aclsData_Temp As clsData_Manager_SQL) As Boolean
        Dim lData As New DataTable
        Dim bResult As Boolean = False

        lData = aclsData_Temp.EvoACDetailsReturnEvents(in_AircraftID)
        If Not IsNothing(lData) Then
            If lData.Rows.Count > 0 Then
                bResult = True
            End If
        End If
        Return bResult
    End Function
    Public Shared Function BuildHistoryTable(ByVal jetnetTable As DataTable, ByVal clientTable As DataTable) As DataTable
        Dim returnTable As New DataTable
        Dim IDsToExclude As String = ""

        returnTable.Columns.Add("journ_id")
        returnTable.Columns.Add("journ_ac_id")
        returnTable.Columns.Add("journ_date", System.Type.GetType("System.DateTime"))
        returnTable.Columns.Add("source")
        returnTable.Columns.Add("client_jetnet_trans_id")
        returnTable.Columns.Add("journ_subject")
        returnTable.Columns.Add("jcat_subcategory_name")
        returnTable.Columns.Add("jcat_auto_subject_flag")
        returnTable.Columns.Add("journ_subcategory_code")
        returnTable.Columns.Add("journ_customer_note")

        returnTable.Columns.Add("adoc_doc_date")
        returnTable.Columns.Add("adoc_onbehalf_comp_id")
        returnTable.Columns.Add("adoc_onbehalf_text")
        returnTable.Columns.Add("adoc_infavor_comp_id")


        returnTable.Columns.Add("adoc_infavor_text")
        returnTable.Columns.Add("adoc_general_note")
        returnTable.Columns.Add("adoc_journ_seq_no")
        returnTable.Columns.Add("adoc_doc_type")
        returnTable.Columns.Add("adoc_journ_id")

        returnTable.Columns.Add("adoc_hide_flag")
        returnTable.Columns.Add("doctype_subdir_name")
        returnTable.Columns.Add("doctype_file_extension")
        returnTable.Columns.Add("Journal_Exists")

        For Each r As DataRow In clientTable.Rows
            Dim newCustomersRow As DataRow = returnTable.NewRow()

            If r("client_jetnet_trans_id") > 0 Then
                If IDsToExclude <> "" Then
                    IDsToExclude += ", "
                End If
                IDsToExclude += r("client_jetnet_trans_id").ToString
            End If

            newCustomersRow("journ_id") = r("journ_id")
            newCustomersRow("journ_ac_id") = r("journ_ac_id")
            newCustomersRow("journ_date") = r("journ_date")
            newCustomersRow("source") = r("source")
            newCustomersRow("client_jetnet_trans_id") = r("client_jetnet_trans_id")
            newCustomersRow("journ_subject") = r("journ_subject")
            newCustomersRow("jcat_subcategory_name") = r("jcat_subcategory_name")
            newCustomersRow("journ_subcategory_code") = r("journ_subcategory_code")
            newCustomersRow("jcat_auto_subject_flag") = r("jcat_auto_subject_flag")
            newCustomersRow("journ_customer_note") = r("journ_customer_note")
            newCustomersRow("Journal_Exists") = r("client_jetnet_trans_id")

            'Need to grab these from jetnet data and then go ahead and not show jetnet data for this row.


            Dim afiltered As DataRow() = jetnetTable.Select(" journ_id = " & r("client_jetnet_trans_id").ToString, "")
            If afiltered.Length > 0 Then
                For Each drJetnet In afiltered
                    newCustomersRow("adoc_doc_date") = drJetnet("adoc_doc_date")
                    newCustomersRow("adoc_onbehalf_comp_id") = drJetnet("adoc_onbehalf_comp_id")
                    newCustomersRow("adoc_onbehalf_text") = drJetnet("adoc_onbehalf_text")
                    newCustomersRow("adoc_infavor_comp_id") = drJetnet("adoc_infavor_comp_id")
                    newCustomersRow("adoc_infavor_text") = drJetnet("adoc_infavor_text")
                    newCustomersRow("adoc_general_note") = drJetnet("adoc_general_note")
                    newCustomersRow("adoc_journ_seq_no") = drJetnet("adoc_journ_seq_no")
                    newCustomersRow("adoc_doc_type") = drJetnet("adoc_doc_type")
                    newCustomersRow("adoc_journ_id") = drJetnet("adoc_journ_id")
                    newCustomersRow("adoc_hide_flag") = drJetnet("adoc_hide_flag")
                    newCustomersRow("doctype_subdir_name") = drJetnet("doctype_subdir_name")
                    newCustomersRow("doctype_file_extension") = drJetnet("doctype_file_extension")
                Next
            Else
                newCustomersRow("adoc_doc_date") = ""
                newCustomersRow("adoc_onbehalf_comp_id") = 0
                newCustomersRow("adoc_onbehalf_text") = ""
                newCustomersRow("adoc_infavor_comp_id") = 0
                newCustomersRow("adoc_infavor_text") = ""
                newCustomersRow("adoc_general_note") = ""
                newCustomersRow("adoc_journ_seq_no") = 0
                newCustomersRow("adoc_doc_type") = ""
                newCustomersRow("adoc_journ_id") = 0
                newCustomersRow("adoc_hide_flag") = ""
                newCustomersRow("doctype_subdir_name") = ""
                newCustomersRow("doctype_file_extension") = ""
            End If
            returnTable.Rows.Add(newCustomersRow)
            returnTable.AcceptChanges()

        Next


        If IDsToExclude <> "" Then
            Dim filteredJetnetTable As New DataTable
            filteredJetnetTable = jetnetTable.Clone


            Dim afiltered_Jetnet As DataRow() = jetnetTable.Select(" journ_id not in (" & IDsToExclude & ") ", "")
            For Each drJetnet In afiltered_Jetnet
                filteredJetnetTable.ImportRow(drJetnet)
            Next

            jetnetTable = filteredJetnetTable
        End If



        For Each r As DataRow In jetnetTable.Rows
            Dim newCustomersRow As DataRow = returnTable.NewRow()
            newCustomersRow("journ_id") = r("journ_id")
            newCustomersRow("journ_ac_id") = r("journ_ac_id")
            newCustomersRow("journ_date") = r("journ_date")
            newCustomersRow("source") = r("source")
            newCustomersRow("client_jetnet_trans_id") = r("client_jetnet_trans_id")
            newCustomersRow("journ_subject") = r("journ_subject")
            newCustomersRow("jcat_subcategory_name") = r("jcat_subcategory_name")
            newCustomersRow("jcat_auto_subject_flag") = r("jcat_auto_subject_flag")
            newCustomersRow("journ_subcategory_code") = r("journ_subcategory_code")
            newCustomersRow("journ_customer_note") = r("journ_customer_note")
            newCustomersRow("adoc_doc_date") = r("adoc_doc_date")
            newCustomersRow("journ_subject") = r("journ_subject")
            newCustomersRow("adoc_onbehalf_comp_id") = r("adoc_onbehalf_comp_id")
            newCustomersRow("adoc_onbehalf_text") = r("adoc_onbehalf_text")
            newCustomersRow("adoc_infavor_comp_id") = r("adoc_infavor_comp_id")
            newCustomersRow("adoc_infavor_text") = r("adoc_infavor_text")
            newCustomersRow("adoc_general_note") = r("adoc_general_note")
            newCustomersRow("adoc_journ_seq_no") = r("adoc_journ_seq_no")
            newCustomersRow("adoc_doc_type") = r("adoc_doc_type")
            newCustomersRow("adoc_journ_id") = r("adoc_journ_id")
            newCustomersRow("adoc_hide_flag") = r("adoc_hide_flag")
            newCustomersRow("doctype_subdir_name") = r("doctype_subdir_name")
            newCustomersRow("doctype_file_extension") = r("doctype_file_extension")
            newCustomersRow("Journal_Exists") = r("Journal_Exists")
            returnTable.Rows.Add(newCustomersRow)
            returnTable.AcceptChanges()
        Next

        Dim Filtered_DV As New DataView(returnTable)
        Filtered_DV.Sort = "journ_date DESC"
        returnTable = Filtered_DV.ToTable


        Return returnTable
    End Function
    Public Shared Function DisplayAircraftHistory_TopBlock(ByVal in_AircraftID As Long,
                                                       ByVal in_JournalID As Long,
                                                       ByRef out_HistoricalRs As DataTable,
                                                       ByRef MySesState As HttpSessionState,
                                                       ByRef history_information_panel As Object,
                                                       ByRef debugQuery As String, ByVal aclsData_Temp As clsData_Manager_SQL, ByRef crmSource As String, ByVal crmJetnetACID As Long, ByVal crmview As Boolean, ByVal ClientAircraftID As Long, ByVal transactionSource As String, ByVal jetnetTransactionID As Long, Optional ByRef historyHeader As String = "") As String

        ' History Status Block
        Dim htmlOut As StringBuilder = New StringBuilder()
        Dim sQuery As String = ""
        Dim excludeONOFFmarket As String = ""
        Dim column As New DataColumn
        Dim IDSToExclude As String = ""
        Dim foundClient As Boolean = False
        Dim clientTable As New DataTable
        If crmview = True And crmSource = "CLIENT" And crmJetnetACID > 0 Then
            clientTable = clsGeneral.clsGeneral.Get_Client_Transactions_as_JetnetFields(crmJetnetACID, MySesState.Item("localSubscription").crmAerodexFlag)
            out_HistoricalRs = aclsData_Temp.GetJETNET_Historical_Data(crmJetnetACID, in_JournalID, MySesState.Item("localSubscription").crmAerodexFlag)

            out_HistoricalRs = BuildHistoryTable(out_HistoricalRs, clientTable)
        ElseIf crmview = True And transactionSource = "CLIENT" Then
            clientTable = clsGeneral.clsGeneral.Get_Client_Transactions_as_JetnetFields(crmJetnetACID, MySesState.Item("localSubscription").crmAerodexFlag)
            out_HistoricalRs = aclsData_Temp.GetJETNET_Historical_Data(in_AircraftID, jetnetTransactionID, MySesState.Item("localSubscription").crmAerodexFlag)
            out_HistoricalRs = BuildHistoryTable(out_HistoricalRs, clientTable)
        Else
            out_HistoricalRs = aclsData_Temp.GetJETNET_Historical_Data(in_AircraftID, in_JournalID, MySesState.Item("localSubscription").crmAerodexFlag)
        End If

        If Not IsNothing(out_HistoricalRs) Then
            If in_JournalID > 0 Then
                For Each r As DataRow In out_HistoricalRs.Rows
                    'If CLng(r("journ_id").ToString) = in_JournalID Then
                    If ((crmview = False Or transactionSource = "JETNET") And CLng(r("journ_id").ToString) = in_JournalID) Or (transactionSource = "CLIENT" And crmview = True And CLng(r("journ_id").ToString) = in_JournalID) Then
                        htmlOut.Length = 0
                        foundClient = True
                        Dim OtherID As Long = 0
                        If crmview = True Then
                            If r("source") = "CLIENT" Then
                                OtherID = r("client_jetnet_trans_id")
                            Else
                                OtherID = r("journ_id")
                            End If
                        End If


                        htmlOut.Append("<div class=""Box""><table class='formatTable blue large lineHeight20' cellpadding='2' cellspacing='0' width='100%'>")
                        If Not IsNothing(history_information_panel) Then
                            ' htmlOut.Append("<tr class=""noBorder""><td align=""left"" valign=""top"" colspan=""3"">") '<div class=""subHeader"">
                            historyHeader = "<em class=""historyTag"">HISTORY AS OF " + clsGeneral.clsGeneral.TwoPlaceYear(r("journ_date")) & "</em>"
                            htmlOut.Append("<tr><td class='featuresHeader' valign=""top"">Date</td><td class='featuresHeader' valign=""top"">Description</td><td align=""left"" valign=""middle"">")

                            If crmview = True Then
                                If transactionSource = "CLIENT" Then
                                    If r("source") = "CLIENT" Then
                                        htmlOut.Append("<span class=""float_right""><a href=""javascript:void(0);"" title=""Edit Client Information"" onclick=""javascript:load('/edit.aspx?action=edit&type=transaction&cli_trans=" & in_JournalID.ToString & "&trans=" & r("client_jetnet_trans_id").ToString & "&acID=" & in_AircraftID.ToString & "&source=" & IIf(crmSource = "CLIENT", "CLIENT", "JETNET") & "&from=aircraftDetails','','scrollbars=yes,menubar=no,height=900,width=990,resizable=yes,toolbar=no,location=no,status=no');return false;"" class=""float_right padding_left""><img src=""images/edit_icon.png"" /></a></span><span class=""float_right pipeDelimeter"">|</span><a href=""/DisplayAircraftDetail.aspx?acid=" & crmJetnetACID.ToString & "&tsource=JETNET&jID=" & r("client_jetnet_trans_id").ToString & """>VIEW JETNET</a></span>")
                                    Else
                                        htmlOut.Append("<a href=""javascript:void(0);"" title=""Create Client Record"" onclick=""javascript:load('/edit.aspx?action=edit&type=transaction&trans=" & in_JournalID & "&acID=" & in_AircraftID.ToString & "&source=" & IIf(crmSource = "CLIENT", "CLIENT", "JETNET") & "&from=aircraftDetails','','scrollbars=yes,menubar=no,height=900,width=990,resizable=yes,toolbar=no,location=no,status=no');return false;"" class=""float_right padding_left""><img src=""images/edit_icon.png"" /></a>")
                                    End If
                                ElseIf crmview = True Then
                                    If transactionSource = "JETNET" Then
                                        OtherID = 0
                                        'We need to do a separate lookup.
                                        Dim CheckTable As New DataTable
                                        CheckTable = aclsData_Temp.Get_Client_Client_Transactions(0, r("journ_id"))
                                        If Not IsNothing(CheckTable) Then
                                            If CheckTable.Rows.Count > 0 Then
                                                OtherID = CheckTable.Rows(0).Item("clitrans_id")
                                                foundClient = True
                                            End If
                                        End If
                                        If OtherID > 0 Then
                                            htmlOut.Append("<span class=""float_right""><a href=""/DisplayAircraftDetail.aspx?acid=" & in_AircraftID.ToString & "&jID=" & OtherID.ToString & "&source=" & IIf(crmSource = "CLIENT", "CLIENT", "JETNET") & "&tsource=CLIENT"">VIEW CLIENT</a></span>")
                                        Else
                                            htmlOut.Append("<a href=""javascript:void(0);"" title=""Create Client Record"" onclick=""javascript:load('/edit.aspx?action=edit&type=transaction&trans=" & r("journ_id") & "&acID=" & in_AircraftID.ToString & "&source=" & IIf(crmSource = "CLIENT", "CLIENT", "JETNET") & "&from=aircraftDetails','','scrollbars=yes,menubar=no,height=900,width=990,resizable=yes,toolbar=no,location=no,status=no');return false;"" class=""float_right padding_left""><img src=""images/edit_icon.png"" /></a>")
                                        End If
                                    End If
                                End If
                            End If

                        End If

                        htmlOut.Append("</td></tr>")
                        If crmJetnetACID > 0 Or in_AircraftID > 0 Then
                            htmlOut.Append("<tr><td align=""left"" valign=""top"" colspan=""3""><span class=""historyTag"">The data below for this aircraft is as of the date of this transaction. To view current data for this aircraft click <a class=""historyTag underline"" " & DisplayFunctions.WriteDetailsLink(in_AircraftID, 0, 0, 0, False, "", "", IIf(crmview = True And crmSource = "CLIENT", "&source=CLIENT", "")) & ">here</a></td></tr>")
                        End If
                        htmlOut.Append("<tr><td align='left' valign='top'>" + clsGeneral.clsGeneral.TwoPlaceYear(r("journ_date").ToString) + "</td>")
                        ' htmlOut.Append("<td align='left' valign='top'>" + r("jcat_subcategory_name").ToString.Trim + "</td>")

                        If r("jcat_auto_subject_flag").ToString.ToUpper = "Y" Then
                            htmlOut.Append("<td align='left' valign='top' colspan=""2"">" + r("jcat_subcategory_name").ToString.Trim)

                            'If r("jcat_subcategory_name").ToString.ToLower.Contains("fractional sale") Then
                            '    htmlOut.Append(Constants.cSingleSpace + CommonAircraftFunctions.GetFractionPercent(MySesState, CLng(lDataReader.Item("journ_id").ToString), True))
                            'End If

                            htmlOut.Append(" - " + r("journ_subject").ToString.Trim + "</td></tr>")
                        Else
                            htmlOut.Append("<td align='left' valign='top' colspan=""2"">" + r("journ_subject").ToString.Trim + "</td></tr>")
                        End If

                        If Not IsDBNull(r("journ_customer_note")) And Not String.IsNullOrEmpty(r("journ_customer_note").ToString) Then
                            htmlOut.Append("<tr><td align='left' valign='top' colspan='4'>Notes:<br />" + r("journ_customer_note").ToString.Trim + "</td></tr>")
                        End If

                        htmlOut.Append("</table></div>")

                    End If ' lDataReader.Item("ac_journ_id").ToString > 0 
                Next
            End If
        End If

        Return htmlOut.ToString.Trim

    End Function
    Public Shared Function DisplayAircraftHistory_BottomBlock(ByRef in_HistoryRs As DataTable,
                                                            ByRef MyAppState As HttpApplicationState,
                                                            ByRef MySesState As HttpSessionState,
                                                            ByVal isFromJFWAFW As Boolean,
                                                            ByVal in_AircraftID As Long,
                                                            ByVal securityToken As String,
                                                            ByVal crmView As Boolean, ByVal aclsData_Temp As clsData_Manager_SQL, ByVal AircraftTable As DataTable, ByRef crmSource As String, ByVal viewAll As Boolean) As String

        Dim htmlOut As StringBuilder = New StringBuilder()
        Dim sTransDocHtml As String = ""
        Dim fJourn_date As String = ""
        Dim fJcat_subcategory_name As String = ""
        Dim hDocumentFile As String = ""
        Dim fJourn_id As Long = 0
        Dim fJourn_subject As String = ""
        Dim fJourn_customer_note As String = ""
        Dim fJcat_auto_subject_flag As String = "N"
        Dim BGColor As String = "engine_6"
        Dim cssClass As String = ""
        Dim temp_string As String = ""
        Dim last_temp_string As String = ""
        Dim sTransDocHtml_Total As String = ""
        Dim firstRow As Boolean = True

        htmlOut.Append("<div class=""Box""><table class='formatTable blue smallerText lineHeight20 historyTable' cellspacing='0' cellpadding='3' width='100%'>")


        If viewAll = False Then
            'Only grab last part
            htmlOut.Append("<tr class=""noBorder""><td colspan=""2""><font class='" & HttpContext.Current.Session("FONT_CLASS_HEADER") & "  subHeader'>HISTORY <b class=""noRowsShow"">(1 YEAR)</b> <span class=""float_right""><a href=""javascript:void(0);"" onClick='swapHistoryToggle()' id=""viewAllHistoryButton"">VIEW ALL HISTORY</a></span></font></td></tr>")
        Else
            htmlOut.Append("<tr class=""noBorder""><td colspan=""2""><font class='" & HttpContext.Current.Session("FONT_CLASS_HEADER") & "  subHeader'>HISTORY</font></td></tr>")
        End If

        If in_HistoryRs.Rows.Count > 0 Then
            For Each r As DataRow In in_HistoryRs.Rows
                cssClass = ""
                'We need to set a CSS class if the journal date is later than a year.. but only if viewAll = false.
                If viewAll = False Then
                    If Not IsDBNull(r("journ_date")) Then
                        Dim dateTimeCompare As Date = CDate(DateAdd(DateInterval.Year, -1, Now()))
                        Dim dateTimeAns As Date = CDate(r("journ_date") & " " & Hour(Now()) & ":" & Second(Now()))
                        Dim result As Integer = DateTime.Compare(dateTimeAns, dateTimeCompare)
                        If result < 0 Then
                            cssClass = "hideHistory"
                        End If
                    End If
                End If

                If Not IsDBNull(r("journ_date")) And Not String.IsNullOrEmpty(r("journ_date").ToString) Then
                    fJourn_date = clsGeneral.clsGeneral.TwoPlaceYear(r("journ_date")) 'Month(FormatDateTime(r("journ_date").ToString.Trim, DateFormat.ShortDate)) & "/" & Day(FormatDateTime(r("journ_date").ToString.Trim, DateFormat.ShortDate)) & "/" & Right(Year(FormatDateTime(r("journ_date").ToString.Trim, DateFormat.ShortDate)), 2)
                End If

                If Not IsDBNull(r("jcat_subcategory_name")) And Not String.IsNullOrEmpty(r("jcat_subcategory_name").ToString) Then
                    fJcat_subcategory_name = r("jcat_subcategory_name").ToString.Trim
                End If

                If Not IsDBNull(r("journ_id")) And Not String.IsNullOrEmpty(r("journ_id").ToString) Then
                    fJourn_id = CLng(r("journ_id").ToString)
                End If

                If Not IsDBNull(r("journ_subject")) And Not String.IsNullOrEmpty(r("journ_subject").ToString) Then
                    fJourn_subject = r("journ_subject").ToString.Trim
                End If

                If Not IsDBNull(r("journ_customer_note")) And Not String.IsNullOrEmpty(r("journ_customer_note").ToString) Then
                    fJourn_customer_note = r("journ_customer_note").ToString.Trim
                End If

                If Not IsDBNull(r("jcat_auto_subject_flag")) And Not String.IsNullOrEmpty(r("jcat_auto_subject_flag").ToString) Then
                    fJcat_auto_subject_flag = r("jcat_auto_subject_flag").ToString.ToUpper.Trim
                End If

                If isFromJFWAFW Then
                    temp_string &= ("<tr class=""" & cssClass & """><td valign='top' align='left' class='XXXCOLOR date_block'><span class='date_text'>" + fJourn_date + "</span></td>")
                    temp_string &= ("<td valign='top' align='left' class='XXXCOLOR'><span class='url_link'>")
                Else
                    temp_string &= ("<tr class=""" & cssClass & """><td valign='top' align='left' class='XXXCOLOR date_block'>")
                    temp_string &= ("<span>")

                    If crmView = True And HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CRM Then
                        temp_string &= ("<span ")
                    Else
                        If Not IsDBNull(r("journ_ac_id")) Then
                            If Not IsDBNull(r("Journal_Exists")) Then
                                temp_string &= ("<a " + DisplayFunctions.WriteDetailsLink(in_AircraftID, 0, 0, fJourn_id, False, "", "date_text", IIf(crmSource = "CLIENT", "&source=CLIENT&tsource=" & r("source").ToString, "")))
                            Else
                                temp_string &= ("<span ")
                            End If
                        Else
                            temp_string &= ("<span ")
                        End If
                    End If

                    temp_string &= (">" + fJourn_date)

                    If crmView = True Then
                        temp_string &= ("</span></span></td>")
                    Else
                        If Not IsDBNull(r("journ_ac_id")) Then
                            temp_string &= ("</a></span></td>")
                        Else
                            temp_string &= ("</span></span></td>")
                        End If
                    End If

                    temp_string &= ("<td valign='top' align='left' class='XXXCOLOR'><span class='url_link'>")

                    If crmView = True And HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CRM Then
                        temp_string &= ("<span")
                    Else
                        If Not IsDBNull(r("journ_ac_id")) Then
                            If Not IsDBNull(r("Journal_Exists")) Then
                                temp_string &= ("<a " & DisplayFunctions.WriteDetailsLink(in_AircraftID, 0, 0, fJourn_id, False, "", "", IIf(crmSource = "CLIENT", "&source=CLIENT&tsource=" & r("source").ToString, "")))
                            Else
                                temp_string &= ("<span")
                            End If
                        Else
                            temp_string &= ("<span")
                        End If
                    End If
                    temp_string &= (">")
                End If



                If fJcat_auto_subject_flag = "Y" Then

                    temp_string &= (Constants.cSingleSpace + fJcat_subcategory_name)

                    'If fJcat_subcategory_name.ToLower.Contains("fractional sale") Then
                    '    htmlOut.Append(Constants.cSingleSpace + CommonAircraftFunctions.GetFractionPercent(MySesState, fJourn_id, False))
                    'End If

                    temp_string &= (" - " + fJourn_subject)

                Else
                    temp_string &= (Constants.cSingleSpace + fJourn_subject)
                End If

                If isFromJFWAFW Then
                    '  htmlOut.Append("</td><td valign='top' align='left'  class='" & BGColor & "'>")
                Else
                    temp_string &= ("</a>")
                    ' htmlOut.Append("</a></td><td valign='top' align='left' class='" & BGColor & "'>")
                End If
                temp_string &= ("</span>")
                temp_string &= ("<br />")


                ' DISPLAY A LIST OF DOCUMENTS FOR THE TRANSACTION RECORD
                If isFromJFWAFW Then
                    If displayTransactionDocuments(IIf(crmView = False, in_AircraftID, AircraftTable.Rows(0).Item("ac_id")), fJourn_id, 0, False, False, isFromJFWAFW, False, MyAppState, MySesState, sTransDocHtml, hDocumentFile, fJourn_subject, fJourn_date, aclsData_Temp, crmView, AircraftTable, crmSource) Then
                        If Not String.IsNullOrEmpty(sTransDocHtml) Then
                            temp_string &= (sTransDocHtml)
                            ' Else
                            '     htmlOut.Append("<em>No&nbsp;Documents&nbsp;For&nbsp;Transaction</em>")
                        End If
                        ' Else
                        '     htmlOut.Append("<em>No&nbsp;Documents&nbsp;For&nbsp;Transaction</em>")
                    End If


                Else

                    displayTransactionDocuments_No_Query(r, IIf(crmView = False, in_AircraftID, AircraftTable.Rows(0).Item("ac_id")), fJourn_id, 0, True, False, False, False, MyAppState, MySesState, sTransDocHtml, hDocumentFile, fJourn_subject, fJourn_date, aclsData_Temp, crmView, AircraftTable, crmSource)

                    'If Not String.IsNullOrEmpty(sTransDocHtml) Then
                    '  sTransDocHtml_Total &= (sTransDocHtml)
                    'End If

                    'If displayTransactionDocuments(in_AircraftID, fJourn_id, 0, True, False, False, False, MyAppState, MySesState, sTransDocHtml, hDocumentFile, fJourn_subject, fJourn_date, aclsData_Temp, crmView, AircraftTable, crmSource) Then
                    '  If Not String.IsNullOrEmpty(sTransDocHtml) Then
                    '    htmlOut.Append(sTransDocHtml)
                    '    'Else
                    '    ' htmlOut.Append("<em>No&nbsp;Documents&nbsp;For&nbsp;Transaction<em>")
                    '  End If
                    '  ' Else
                    '  '    htmlOut.Append("<em>No&nbsp;Documents&nbsp;For&nbsp;Transaction</em>")
                    'End If 

                End If


                ' if it has changed, add in 
                If Trim(temp_string) <> Trim(last_temp_string) And Trim(last_temp_string) <> "" Then

                    If BGColor = "engine_6" Then
                        BGColor = "engine_5"
                    Else
                        BGColor = "engine_6"
                    End If

                    last_temp_string = Replace(last_temp_string, "XXXCOLOR", BGColor)

                    ' moved this from up above, so that the first row does not duplicate - MSW - 4/4/19
                    If firstRow = True And cssClass = "hideHistory" Then
                        'No displayable rows.
                        htmlOut.Append("<tr class=""noRowsShow""><td valign='top' align='left' colspan='2'>No applicable history for this date range.</td></tr>")
                    End If
                    firstRow = False

                    htmlOut.Append(last_temp_string)
                    htmlOut.Append(sTransDocHtml_Total)
                    htmlOut.Append("</td></tr>")
                    sTransDocHtml_Total = ""
                End If

                If Not String.IsNullOrEmpty(sTransDocHtml) Then
                    sTransDocHtml_Total &= (sTransDocHtml)
                End If

                last_temp_string = temp_string
                temp_string = ""

            Next

            If Trim(last_temp_string) <> "" Then

                If BGColor = "engine_6" Then
                    BGColor = "engine_5"
                Else
                    BGColor = "engine_6"
                End If

                htmlOut.Append(last_temp_string)
                htmlOut.Append(sTransDocHtml_Total)
                htmlOut.Append("</td></tr>")
            End If

        Else
            htmlOut.Append("<tr><td align='center'>No History Records Found</td></tr>")
        End If ' in_HistoryRs.HasRows Then

        htmlOut.Append("</table></div>")

        Return htmlOut.ToString.Trim

    End Function
    Public Shared Function displayTransactionDocuments_No_Query(ByVal r As DataRow, ByVal nAircraftID As Long,
                                               ByVal nAircraftJournalID As Long,
                                               ByVal nSequenceNo As Integer,
                                               ByVal isDisplay As Boolean,
                                               ByVal isDetails As Boolean,
                                               ByVal isJFWAFW As Boolean,
                                               ByVal isView As Boolean,
                                               ByRef MyAppState As HttpApplicationState,
                                               ByRef MySesState As HttpSessionState,
                                               ByRef out_html As String, ByRef hDocumentFile As String, ByRef journ_subject As String, ByRef fAdoc_doc_date As String,
                                               ByVal aclsData_Temp As clsData_Manager_SQL, ByVal crmView As Boolean,
                                               ByVal aircraftTable As DataTable, ByRef crmSource As String) As Boolean

        ' DISPLAY THE LIST OF AIRCRAFT DOCUMENTS ASSOCIATED WITH THE JOURNAL ENTRY
        Dim RefTable As New DataTable
        Dim sHtmlOut As StringBuilder = New StringBuilder()
        Dim HoldTable As New DataTable

        Dim fAdoc_general_note As String = ""
        Dim fAdoc_journ_seq_no As Integer = 0
        Dim fAdoc_doc_type As String = ""
        Dim fAdoc_journ_id As Long = 0
        Dim fAdoc_hide_flag As String = ""
        Dim fDoctype_subdir_name As String = ""
        Dim fDoctype_file_extension As String = ""
        Dim fAdoc_onbehalf_comp_id As Long = 0
        Dim fAdoc_onbehalf_text As String = ""
        Dim fAdoc_infavor_comp_id As Long = 0
        Dim fAdoc_infavor_text As String = ""
        Dim Make As String = ""
        Dim Model As String = ""
        Dim Ser As String = ""
        Dim temp_string As String = ""
        Dim add_open_paren As Boolean = False
        Dim lendor_string As String = ""

        If Not IsNothing(aircraftTable) Then
            If aircraftTable.Rows.Count > 0 Then
                Make = IIf(Not IsDBNull(aircraftTable.Rows(0).Item("amod_make_name")), aircraftTable.Rows(0).Item("amod_make_name"), "")
                Model = IIf(Not IsDBNull(aircraftTable.Rows(0).Item("amod_model_name")), aircraftTable.Rows(0).Item("amod_model_name"), "")
                Ser = IIf(Not IsDBNull(aircraftTable.Rows(0).Item("ac_ser_nbr")), aircraftTable.Rows(0).Item("ac_ser_nbr"), "")
            End If
        End If

        If Not IsDBNull(r("adoc_doc_type")) And Not String.IsNullOrEmpty(r("adoc_doc_type").ToString) Then
            fAdoc_doc_type = r("adoc_doc_type").ToString.Trim
        Else
            fAdoc_doc_type = ""
        End If

        If Trim(fAdoc_doc_type) <> "" Then

            If Not IsDBNull(r("adoc_general_note")) And Not String.IsNullOrEmpty(r("adoc_general_note").ToString) Then
                fAdoc_general_note = r("adoc_general_note").ToString.Trim
            Else
                fAdoc_general_note = ""
            End If

            If Not IsDBNull(r("journ_subject")) And Not String.IsNullOrEmpty(r("journ_subject").ToString) Then
                journ_subject = r("journ_subject").ToString.Trim
            Else
                journ_subject = ""
            End If


            If Not IsDBNull(r("adoc_doc_date")) And Not String.IsNullOrEmpty(r("adoc_doc_date").ToString) Then
                fAdoc_doc_date = FormatDateTime(r("adoc_doc_date").ToString.Trim, DateFormat.GeneralDate)
            Else
                fAdoc_doc_date = ""
            End If

            If Not IsDBNull(r("adoc_onbehalf_comp_id")) And Not String.IsNullOrEmpty(r("adoc_onbehalf_comp_id").ToString) Then
                fAdoc_onbehalf_comp_id = CLng(r("adoc_onbehalf_comp_id").ToString.Trim)
            Else
                fAdoc_onbehalf_comp_id = 0
            End If

            If Not IsDBNull(r("adoc_onbehalf_text")) And Not String.IsNullOrEmpty(r("adoc_onbehalf_text").ToString) Then
                fAdoc_onbehalf_text = r("adoc_onbehalf_text").ToString.Trim
            Else
                fAdoc_onbehalf_text = ""
            End If

            If Not IsDBNull(r("adoc_infavor_comp_id")) And Not String.IsNullOrEmpty(r("adoc_infavor_comp_id").ToString) Then
                fAdoc_infavor_comp_id = CLng(r("adoc_infavor_comp_id").ToString.Trim)
            Else
                fAdoc_infavor_comp_id = 0
            End If

            If Not IsDBNull(r("adoc_infavor_text")) And Not String.IsNullOrEmpty(r("adoc_infavor_text").ToString) Then
                fAdoc_infavor_text = r("adoc_infavor_text").ToString.Trim
            Else
                fAdoc_infavor_text = ""
            End If

            If Not IsDBNull(r("adoc_journ_seq_no")) And Not String.IsNullOrEmpty(r("adoc_journ_seq_no").ToString) Then
                fAdoc_journ_seq_no = CInt(r("adoc_journ_seq_no").ToString.Trim)
            Else
                fAdoc_journ_seq_no = 0
            End If

            If Not IsDBNull(r("adoc_journ_id")) And Not String.IsNullOrEmpty(r("adoc_journ_id").ToString) Then
                fAdoc_journ_id = CLng(r("adoc_journ_id").ToString.Trim)
            Else
                fAdoc_journ_id = 0
            End If

            If Not IsDBNull(r("adoc_hide_flag")) And Not String.IsNullOrEmpty(r("adoc_hide_flag").ToString) Then
                fAdoc_hide_flag = r("adoc_hide_flag").ToString.Trim
            Else
                fAdoc_hide_flag = ""
            End If

            If Not IsDBNull(r("doctype_subdir_name")) And Not String.IsNullOrEmpty(r("doctype_subdir_name").ToString) Then
                fDoctype_subdir_name = r("doctype_subdir_name").ToString.Trim
            Else
                fDoctype_subdir_name = ""
            End If

            If Not IsDBNull(r("doctype_file_extension")) And Not String.IsNullOrEmpty(r("doctype_file_extension").ToString) Then
                fDoctype_file_extension = r("doctype_file_extension").ToString.Trim
            Else
                fDoctype_file_extension = ""
            End If

            If fAdoc_hide_flag.ToUpper <> "Y" Then

                ' GET THE FILE NAME FOR THE ELECTRONIC DOCUMENT
                hDocumentFile = clsGeneral.clsGeneral.Get_Document_File_Name(nAircraftID, fAdoc_journ_id, fAdoc_journ_seq_no, fDoctype_subdir_name, fDoctype_file_extension, MyAppState, MySesState)

                If Not String.IsNullOrEmpty(hDocumentFile) Then

                    'If System.IO.File.Exists(HttpContext.Current.Server.MapPath(hDocumentFile)) Then
                    If isDisplay Then
                        sHtmlOut.Append("<a href='#' onclick=""javascript:SubmitTransactionDocumentForm('" & Make & "','" & Model & "','" & Ser & "'," & nAircraftID.ToString & "," & fAdoc_journ_id.ToString & "," & fAdoc_journ_seq_no.ToString & ");"">" & ReturnImageIcon(fDoctype_file_extension) & "</a>&nbsp;")
                        sHtmlOut.Append("<a href='#' onclick=""javascript:SubmitTransactionDocumentForm('" & Make & "','" & Model & "','" & Ser & "'," & nAircraftID.ToString & "," & fAdoc_journ_id.ToString & "," & fAdoc_journ_seq_no.ToString & ");"">" + fAdoc_doc_type.ToString + "</a>")
                    ElseIf isDetails Then
                        sHtmlOut.Append("<a href='#' onclick=""javascript:SubmitTransactionDocumentForm('" & Make & "','" & Model & "','" & Ser & "'," & nAircraftID.ToString & "," & fAdoc_journ_id.ToString & "," & fAdoc_journ_seq_no.ToString & ");"">" & ReturnImageIcon(fDoctype_file_extension) & "</a>&nbsp;")
                        sHtmlOut.Append(fAdoc_doc_type)
                    Else
                        If Not isJFWAFW Then
                            sHtmlOut.Append("<a href='#' onclick=""javascript:SubmitTransactionDocumentForm('" & Make & "','" & Model & "','" & Ser & "'," & nAircraftID.ToString & "," & fAdoc_journ_id.ToString & "," & fAdoc_journ_seq_no.ToString & ");"">" & ReturnImageIcon(fDoctype_file_extension) & "</a>&nbsp;")
                            sHtmlOut.Append(fAdoc_doc_type)
                        Else
                            sHtmlOut.Append("<img align='absmiddle' src='images/dark_blue_bullet.jpg' border='0' alt='" + fAdoc_doc_type + "'>&nbsp;")
                            sHtmlOut.Append(fAdoc_doc_type)
                        End If
                    End If

                    add_open_paren = False

                    If nAircraftJournalID > 0 Then
                        If Not String.IsNullOrEmpty(fAdoc_general_note) Then
                            add_open_paren = True
                        End If
                    End If

                    If fAdoc_infavor_comp_id > 0 Or Not String.IsNullOrEmpty(fAdoc_infavor_text) Then
                        If fAdoc_onbehalf_comp_id > 0 Or fAdoc_infavor_comp_id > 0 Or Not String.IsNullOrEmpty(fAdoc_infavor_text) Or Not String.IsNullOrEmpty(fAdoc_onbehalf_text) Then
                            add_open_paren = True
                        End If
                    End If

                    If add_open_paren = True Then
                        sHtmlOut.Append("&nbsp;(")
                    End If

                    If nAircraftJournalID > 0 Then
                        If Not String.IsNullOrEmpty(fAdoc_general_note) Then
                            sHtmlOut.Append("" + fAdoc_general_note.Trim + "")
                        End If
                    End If

                    If Trim(fAdoc_doc_date) <> "" Then
                        If IsDate(fAdoc_doc_date) = True Then
                            temp_string = Year(fAdoc_doc_date)
                            If Left(Trim(temp_string), 2) = "20" Then
                                temp_string = Right(Trim(temp_string), 2)
                                fAdoc_doc_date = Month(fAdoc_doc_date) & "/" & Day(fAdoc_doc_date) & "/" & temp_string
                            End If
                        End If
                    End If


                    ' we have an infavor company id or infavor text is not blank
                    If fAdoc_infavor_comp_id > 0 Or Not String.IsNullOrEmpty(fAdoc_infavor_text) Then

                        If Not String.IsNullOrEmpty(fAdoc_doc_date) Then

                            If Trim(fAdoc_doc_type) = "Lease Agreement" Then
                                lendor_string = "Lessor"
                            ElseIf Trim(fAdoc_doc_type) = "Lien Release" Then
                                lendor_string = "Released by"
                            Else
                                lendor_string = "Lender"
                            End If

                            sHtmlOut.Append("Filed on " + fAdoc_doc_date)

                            If fAdoc_onbehalf_comp_id > 0 And fAdoc_infavor_comp_id > 0 Then

                                sHtmlOut.Append(", Borrower: ")
                                HoldTable = aclsData_Temp.GetLimited_CompanyInfo_ID(fAdoc_onbehalf_comp_id, "JETNET", nAircraftJournalID)
                                If Not IsNothing(HoldTable) Then
                                    If HoldTable.Rows.Count > 0 Then
                                        sHtmlOut.Append(HoldTable.Rows(0).Item("comp_name"))
                                    End If
                                End If
                                HoldTable.Dispose()

                                sHtmlOut.Append(", " & lendor_string & ": ")
                                HoldTable = aclsData_Temp.GetLimited_CompanyInfo_ID(fAdoc_infavor_comp_id, "JETNET", nAircraftJournalID)
                                If Not IsNothing(HoldTable) Then
                                    If HoldTable.Rows.Count > 0 Then
                                        sHtmlOut.Append(HoldTable.Rows(0).Item("comp_name"))
                                    End If
                                End If
                                HoldTable.Dispose()

                            ElseIf fAdoc_infavor_comp_id > 0 Then


                                sHtmlOut.Append(", " & lendor_string & ": ")
                                HoldTable = aclsData_Temp.GetLimited_CompanyInfo_ID(fAdoc_infavor_comp_id, "JETNET", nAircraftJournalID)
                                If Not IsNothing(HoldTable) Then
                                    If HoldTable.Rows.Count > 0 Then
                                        sHtmlOut.Append(HoldTable.Rows(0).Item("comp_name"))
                                    End If
                                End If
                                HoldTable.Dispose()

                            ElseIf fAdoc_onbehalf_comp_id > 0 Then

                                sHtmlOut.Append(", Borrower: ")
                                HoldTable = aclsData_Temp.GetLimited_CompanyInfo_ID(fAdoc_onbehalf_comp_id, "JETNET", nAircraftJournalID)
                                If Not IsNothing(HoldTable) Then
                                    If HoldTable.Rows.Count > 0 Then
                                        sHtmlOut.Append(HoldTable.Rows(0).Item("comp_name"))
                                    End If
                                End If
                                HoldTable.Dispose()
                            ElseIf (fAdoc_onbehalf_comp_id = 0 And Not String.IsNullOrEmpty(fAdoc_onbehalf_text)) And (fAdoc_infavor_comp_id = 0 And Not String.IsNullOrEmpty(fAdoc_infavor_text)) Then

                                sHtmlOut.Append(", Borrower: " + fAdoc_onbehalf_text)

                                sHtmlOut.Append(", " & lendor_string & ": " + fAdoc_infavor_text)


                            ElseIf fAdoc_onbehalf_comp_id = 0 And Not String.IsNullOrEmpty(fAdoc_onbehalf_text) Then

                                sHtmlOut.Append(" Borrower: " + fAdoc_onbehalf_text)
                                sHtmlOut.Append(")")

                            ElseIf Not String.IsNullOrEmpty(fAdoc_onbehalf_text) Then

                                sHtmlOut.Append(", Borrower: " + fAdoc_onbehalf_text)

                            ElseIf fAdoc_infavor_comp_id = 0 And Not String.IsNullOrEmpty(fAdoc_infavor_text) Then

                                sHtmlOut.Append(", " & lendor_string & ": " + fAdoc_infavor_text)

                            ElseIf Not String.IsNullOrEmpty(fAdoc_infavor_text) Then

                                sHtmlOut.Append(", " & lendor_string & ": " + fAdoc_infavor_text)

                            End If
                        End If
                    End If


                    If add_open_paren = True Then
                        sHtmlOut.Append(")")
                    End If

                    sHtmlOut.Append("</span><br />")
                Else
                    sHtmlOut.Append(fAdoc_doc_type + "&nbsp;:&nbsp;Not&nbsp;On&nbsp;File---<br />")
                End If ' hDocumentFile <> "" 

            Else
                sHtmlOut.Append("&nbsp;")
            End If ' fAdoc_hide_flag <> "Y" 



            out_html = sHtmlOut.ToString
        Else
            out_html = ""
        End If

        Return True

        sHtmlOut = Nothing

    End Function

    Public Shared Function ReturnImageIcon(ByVal fileExtension As Object) 'move to display function class
        If Not IsDBNull(fileExtension) Then
            Select Case LCase(fileExtension.ToString)
                Case ".docx"
                    Return "<i class=""fa fa-file-word-o""></i>"
                Case ".pdf"
                    Return "<i class=""fa fa-file-pdf-o""></i>"
                Case ".xls"
                    Return "<i class=""fa fa-file-excel-o""></i>"
                Case ".ppt"
                    Return "<i class=""fa fa-file-powerpoint-o""></i>"
                Case "jpg", ".gif"
                    Return "<i class=""fa fa-file-image-o""></i>"
            End Select
        End If

        Return "<i class=""fa fa-file-pdf-o""></i>"
    End Function

    Public Shared Function displayTransactionDocuments(ByVal nAircraftID As Long,
                                               ByVal nAircraftJournalID As Long,
                                               ByVal nSequenceNo As Integer,
                                               ByVal isDisplay As Boolean,
                                               ByVal isDetails As Boolean,
                                               ByVal isJFWAFW As Boolean,
                                               ByVal isView As Boolean,
                                               ByRef MyAppState As HttpApplicationState,
                                               ByRef MySesState As HttpSessionState,
                                               ByRef out_html As String, ByRef hDocumentFile As String, ByRef journ_subject As String, ByRef fAdoc_doc_date As String,
                                               ByVal aclsData_Temp As clsData_Manager_SQL, ByVal crmView As Boolean,
                                               ByVal aircraftTable As DataTable, ByRef crmSource As String) As Boolean

        ' DISPLAY THE LIST OF AIRCRAFT DOCUMENTS ASSOCIATED WITH THE JOURNAL ENTRY
        Dim RefTable As New DataTable
        Dim sHtmlOut As StringBuilder = New StringBuilder()
        Dim HoldTable As New DataTable

        Dim fAdoc_general_note As String = ""
        Dim fAdoc_journ_seq_no As Integer = 0
        Dim fAdoc_doc_type As String = ""
        Dim fAdoc_journ_id As Long = 0
        Dim fAdoc_hide_flag As String = ""
        Dim fDoctype_subdir_name As String = ""
        Dim fDoctype_file_extension As String = ""
        Dim fAdoc_onbehalf_comp_id As Long = 0
        Dim fAdoc_onbehalf_text As String = ""
        Dim fAdoc_infavor_comp_id As Long = 0
        Dim fAdoc_infavor_text As String = ""
        Dim Make As String = ""
        Dim Model As String = ""
        Dim Ser As String = ""
        Dim temp_string As String = ""
        Dim add_open_paren As Boolean = False
        Dim lendor_string As String = ""

        If Not IsNothing(aircraftTable) Then
            If aircraftTable.Rows.Count > 0 Then
                Make = IIf(Not IsDBNull(aircraftTable.Rows(0).Item("amod_make_name")), aircraftTable.Rows(0).Item("amod_make_name"), "")
                Model = IIf(Not IsDBNull(aircraftTable.Rows(0).Item("amod_model_name")), aircraftTable.Rows(0).Item("amod_model_name"), "")
                Ser = IIf(Not IsDBNull(aircraftTable.Rows(0).Item("ac_ser_nbr")), aircraftTable.Rows(0).Item("ac_ser_nbr"), "")
            End If
        End If

        If crmSource = "CLIENT" Then
            RefTable = aclsData_Temp.Get_JETNET_TransactionDocuments(aircraftTable.Rows(0).Item("ac_id"), nAircraftJournalID, nSequenceNo)
        Else
            RefTable = aclsData_Temp.Get_JETNET_TransactionDocuments(nAircraftID, nAircraftJournalID, nSequenceNo)
        End If

        If Not IsNothing(RefTable) Then

            If Not isView Then

                For Each r As DataRow In RefTable.Rows

                    If Not IsDBNull(r("adoc_general_note")) And Not String.IsNullOrEmpty(r("adoc_general_note").ToString) Then
                        fAdoc_general_note = r("adoc_general_note").ToString.Trim
                    Else
                        fAdoc_general_note = ""
                    End If

                    If Not IsDBNull(r("journ_subject")) And Not String.IsNullOrEmpty(r("journ_subject").ToString) Then
                        journ_subject = r("journ_subject").ToString.Trim
                    Else
                        journ_subject = ""
                    End If


                    If Not IsDBNull(r("adoc_doc_date")) And Not String.IsNullOrEmpty(r("adoc_doc_date").ToString) Then
                        fAdoc_doc_date = FormatDateTime(r("adoc_doc_date").ToString.Trim, DateFormat.GeneralDate)
                    Else
                        fAdoc_doc_date = ""
                    End If

                    If Not IsDBNull(r("adoc_onbehalf_comp_id")) And Not String.IsNullOrEmpty(r("adoc_onbehalf_comp_id").ToString) Then
                        fAdoc_onbehalf_comp_id = CLng(r("adoc_onbehalf_comp_id").ToString.Trim)
                    Else
                        fAdoc_onbehalf_comp_id = 0
                    End If

                    If Not IsDBNull(r("adoc_onbehalf_text")) And Not String.IsNullOrEmpty(r("adoc_onbehalf_text").ToString) Then
                        fAdoc_onbehalf_text = r("adoc_onbehalf_text").ToString.Trim
                    Else
                        fAdoc_onbehalf_text = ""
                    End If

                    If Not IsDBNull(r("adoc_infavor_comp_id")) And Not String.IsNullOrEmpty(r("adoc_infavor_comp_id").ToString) Then
                        fAdoc_infavor_comp_id = CLng(r("adoc_infavor_comp_id").ToString.Trim)
                    Else
                        fAdoc_infavor_comp_id = 0
                    End If

                    If Not IsDBNull(r("adoc_infavor_text")) And Not String.IsNullOrEmpty(r("adoc_infavor_text").ToString) Then
                        fAdoc_infavor_text = r("adoc_infavor_text").ToString.Trim
                    Else
                        fAdoc_infavor_text = ""
                    End If

                    If Not IsDBNull(r("adoc_journ_seq_no")) And Not String.IsNullOrEmpty(r("adoc_journ_seq_no").ToString) Then
                        fAdoc_journ_seq_no = CInt(r("adoc_journ_seq_no").ToString.Trim)
                    Else
                        fAdoc_journ_seq_no = 0
                    End If

                    If Not IsDBNull(r("adoc_doc_type")) And Not String.IsNullOrEmpty(r("adoc_doc_type").ToString) Then
                        fAdoc_doc_type = r("adoc_doc_type").ToString.Trim
                    Else
                        fAdoc_doc_type = ""
                    End If

                    If Not IsDBNull(r("adoc_journ_id")) And Not String.IsNullOrEmpty(r("adoc_journ_id").ToString) Then
                        fAdoc_journ_id = CLng(r("adoc_journ_id").ToString.Trim)
                    Else
                        fAdoc_journ_id = 0
                    End If

                    If Not IsDBNull(r("adoc_hide_flag")) And Not String.IsNullOrEmpty(r("adoc_hide_flag").ToString) Then
                        fAdoc_hide_flag = r("adoc_hide_flag").ToString.Trim
                    Else
                        fAdoc_hide_flag = ""
                    End If

                    If Not IsDBNull(r("doctype_subdir_name")) And Not String.IsNullOrEmpty(r("doctype_subdir_name").ToString) Then
                        fDoctype_subdir_name = r("doctype_subdir_name").ToString.Trim
                    Else
                        fDoctype_subdir_name = ""
                    End If

                    If Not IsDBNull(r("doctype_file_extension")) And Not String.IsNullOrEmpty(r("doctype_file_extension").ToString) Then
                        fDoctype_file_extension = r("doctype_file_extension").ToString.Trim
                    Else
                        fDoctype_file_extension = ""
                    End If

                    If fAdoc_hide_flag.ToUpper <> "Y" Then

                        ' GET THE FILE NAME FOR THE ELECTRONIC DOCUMENT
                        hDocumentFile = clsGeneral.clsGeneral.Get_Document_File_Name(nAircraftID, fAdoc_journ_id, fAdoc_journ_seq_no, fDoctype_subdir_name, fDoctype_file_extension, MyAppState, MySesState)

                        If Not String.IsNullOrEmpty(hDocumentFile) Then

                            'If System.IO.File.Exists(HttpContext.Current.Server.MapPath(hDocumentFile)) Then
                            If isDisplay Then
                                sHtmlOut.Append("<a href='#' onclick=""javascript:SubmitTransactionDocumentForm('" & Make & "','" & Model & "','" & Ser & "'," & nAircraftID.ToString & "," & fAdoc_journ_id.ToString & "," & fAdoc_journ_seq_no.ToString & ");"">" & "<img align='absmiddle' src='images/DocumentSM.gif' border='0' alt='" + fAdoc_doc_type + "'></a>&nbsp;")
                                sHtmlOut.Append("<a href='#' onclick=""javascript:SubmitTransactionDocumentForm('" & Make & "','" & Model & "','" & Ser & "'," & nAircraftID.ToString & "," & fAdoc_journ_id.ToString & "," & fAdoc_journ_seq_no.ToString & ");"">" & "" + fAdoc_doc_type.ToString + "</a>")
                            ElseIf isDetails Then
                                sHtmlOut.Append("<a href='#' onclick=""javascript:SubmitTransactionDocumentForm('" & Make & "','" & Model & "','" & Ser & "'," & nAircraftID.ToString & "," & fAdoc_journ_id.ToString & "," & fAdoc_journ_seq_no.ToString & ");"">" & "<img align='absmiddle' src='images/DocumentSM.gif' border='0' alt='" + fAdoc_doc_type + "'></a>&nbsp;")
                                sHtmlOut.Append(fAdoc_doc_type)
                            Else
                                If Not isJFWAFW Then
                                    sHtmlOut.Append("<a href='#' onclick=""javascript:SubmitTransactionDocumentForm('" & Make & "','" & Model & "','" & Ser & "'," & nAircraftID.ToString & "," & fAdoc_journ_id.ToString & "," & fAdoc_journ_seq_no.ToString & ");"">" & "><img align='absmiddle' src='images/DocumentSM.gif' border='0' alt='" + fAdoc_doc_type + "'</a>&nbsp;")
                                    sHtmlOut.Append(fAdoc_doc_type)
                                Else
                                    sHtmlOut.Append("<img align='absmiddle' src='images/dark_blue_bullet.jpg' border='0' alt='" + fAdoc_doc_type + "'>&nbsp;")
                                    sHtmlOut.Append(fAdoc_doc_type)
                                End If
                            End If

                            add_open_paren = False

                            If nAircraftJournalID > 0 Then
                                If Not String.IsNullOrEmpty(fAdoc_general_note) Then
                                    add_open_paren = True
                                End If
                            End If

                            If fAdoc_infavor_comp_id > 0 Or Not String.IsNullOrEmpty(fAdoc_infavor_text) Then
                                If fAdoc_onbehalf_comp_id > 0 Or fAdoc_infavor_comp_id > 0 Or Not String.IsNullOrEmpty(fAdoc_infavor_text) Or Not String.IsNullOrEmpty(fAdoc_onbehalf_text) Then
                                    add_open_paren = True
                                End If
                            End If

                            If add_open_paren = True Then
                                sHtmlOut.Append("&nbsp;(")
                            End If

                            If nAircraftJournalID > 0 Then
                                If Not String.IsNullOrEmpty(fAdoc_general_note) Then
                                    sHtmlOut.Append("" + fAdoc_general_note.Trim + "")
                                End If
                            End If

                            If Trim(fAdoc_doc_date) <> "" Then
                                If IsDate(fAdoc_doc_date) = True Then
                                    temp_string = Year(fAdoc_doc_date)
                                    If Left(Trim(temp_string), 2) = "20" Then
                                        temp_string = Right(Trim(temp_string), 2)
                                        fAdoc_doc_date = Month(fAdoc_doc_date) & "/" & Day(fAdoc_doc_date) & "/" & temp_string
                                    End If
                                End If
                            End If


                            ' we have an infavor company id or infavor text is not blank
                            If fAdoc_infavor_comp_id > 0 Or Not String.IsNullOrEmpty(fAdoc_infavor_text) Then

                                If Not String.IsNullOrEmpty(fAdoc_doc_date) Then

                                    If Trim(fAdoc_doc_type) = "Lease Agreement" Then
                                        lendor_string = "Lessor"
                                    ElseIf Trim(fAdoc_doc_type) = "Lien Release" Then
                                        lendor_string = "Released by"
                                    Else
                                        lendor_string = "Lender"
                                    End If

                                    sHtmlOut.Append("Filed on " + fAdoc_doc_date)

                                    If fAdoc_onbehalf_comp_id > 0 And fAdoc_infavor_comp_id > 0 Then

                                        sHtmlOut.Append(", Borrower: ")
                                        HoldTable = aclsData_Temp.GetLimited_CompanyInfo_ID(fAdoc_onbehalf_comp_id, "JETNET", nAircraftJournalID)
                                        If Not IsNothing(HoldTable) Then
                                            If HoldTable.Rows.Count > 0 Then
                                                sHtmlOut.Append(HoldTable.Rows(0).Item("comp_name"))
                                            End If
                                        End If
                                        HoldTable.Dispose()

                                        sHtmlOut.Append(", " & lendor_string & ": ")
                                        HoldTable = aclsData_Temp.GetLimited_CompanyInfo_ID(fAdoc_infavor_comp_id, "JETNET", nAircraftJournalID)
                                        If Not IsNothing(HoldTable) Then
                                            If HoldTable.Rows.Count > 0 Then
                                                sHtmlOut.Append(HoldTable.Rows(0).Item("comp_name"))
                                            End If
                                        End If
                                        HoldTable.Dispose()

                                    ElseIf fAdoc_infavor_comp_id > 0 Then


                                        sHtmlOut.Append(", " & lendor_string & ": ")
                                        HoldTable = aclsData_Temp.GetLimited_CompanyInfo_ID(fAdoc_infavor_comp_id, "JETNET", nAircraftJournalID)
                                        If Not IsNothing(HoldTable) Then
                                            If HoldTable.Rows.Count > 0 Then
                                                sHtmlOut.Append(HoldTable.Rows(0).Item("comp_name"))
                                            End If
                                        End If
                                        HoldTable.Dispose()

                                    ElseIf fAdoc_onbehalf_comp_id > 0 Then

                                        sHtmlOut.Append(", Borrower: ")
                                        HoldTable = aclsData_Temp.GetLimited_CompanyInfo_ID(fAdoc_onbehalf_comp_id, "JETNET", nAircraftJournalID)
                                        If Not IsNothing(HoldTable) Then
                                            If HoldTable.Rows.Count > 0 Then
                                                sHtmlOut.Append(HoldTable.Rows(0).Item("comp_name"))
                                            End If
                                        End If
                                        HoldTable.Dispose()
                                    ElseIf (fAdoc_onbehalf_comp_id = 0 And Not String.IsNullOrEmpty(fAdoc_onbehalf_text)) And (fAdoc_infavor_comp_id = 0 And Not String.IsNullOrEmpty(fAdoc_infavor_text)) Then

                                        sHtmlOut.Append(", Borrower: " + fAdoc_onbehalf_text)

                                        sHtmlOut.Append(", " & lendor_string & ": " + fAdoc_infavor_text)


                                    ElseIf fAdoc_onbehalf_comp_id = 0 And Not String.IsNullOrEmpty(fAdoc_onbehalf_text) Then

                                        sHtmlOut.Append(" Borrower: " + fAdoc_onbehalf_text)
                                        sHtmlOut.Append(")")

                                    ElseIf Not String.IsNullOrEmpty(fAdoc_onbehalf_text) Then

                                        sHtmlOut.Append(", Borrower: " + fAdoc_onbehalf_text)

                                    ElseIf fAdoc_infavor_comp_id = 0 And Not String.IsNullOrEmpty(fAdoc_infavor_text) Then

                                        sHtmlOut.Append(", " & lendor_string & ": " + fAdoc_infavor_text)

                                    ElseIf Not String.IsNullOrEmpty(fAdoc_infavor_text) Then

                                        sHtmlOut.Append(", " & lendor_string & ": " + fAdoc_infavor_text)

                                    End If
                                End If
                            End If


                            If add_open_paren = True Then
                                sHtmlOut.Append(")")
                            End If

                            sHtmlOut.Append("</span><br />")
                        Else
                            sHtmlOut.Append(fAdoc_doc_type + "&nbsp;:&nbsp;Not&nbsp;On&nbsp;File---<br />")
                        End If ' hDocumentFile <> "" 

                    Else
                        sHtmlOut.Append("&nbsp;")
                    End If ' fAdoc_hide_flag <> "Y" 

                Next

            Else


                If Not IsDBNull(RefTable.Rows(0).Item("adoc_general_note")) And Not String.IsNullOrEmpty(RefTable.Rows(0).Item("adoc_general_note").ToString) Then
                    fAdoc_general_note = RefTable.Rows(0).Item("adoc_general_note").ToString.Trim
                Else
                    fAdoc_general_note = ""
                End If

                If Not IsDBNull(RefTable.Rows(0).Item("adoc_doc_date")) And Not String.IsNullOrEmpty(RefTable.Rows(0).Item("adoc_doc_date").ToString) Then
                    fAdoc_doc_date = RefTable.Rows(0).Item("adoc_doc_date").ToString.Trim
                Else
                    fAdoc_doc_date = ""
                End If

                If Not IsDBNull(RefTable.Rows(0).Item("adoc_onbehalf_comp_id")) And Not String.IsNullOrEmpty(RefTable.Rows(0).Item("adoc_onbehalf_comp_id").ToString) Then
                    fAdoc_onbehalf_comp_id = CLng(RefTable.Rows(0).Item("adoc_onbehalf_comp_id").ToString.Trim)
                Else
                    fAdoc_onbehalf_comp_id = 0
                End If

                If Not IsDBNull(RefTable.Rows(0).Item("adoc_onbehalf_text")) And Not String.IsNullOrEmpty(RefTable.Rows(0).Item("adoc_onbehalf_text").ToString) Then
                    fAdoc_onbehalf_text = RefTable.Rows(0).Item("adoc_onbehalf_text").ToString.Trim
                Else
                    fAdoc_onbehalf_text = ""
                End If

                If Not IsDBNull(RefTable.Rows(0).Item("adoc_infavor_comp_id")) And Not String.IsNullOrEmpty(RefTable.Rows(0).Item("adoc_infavor_comp_id").ToString) Then
                    fAdoc_infavor_comp_id = CLng(RefTable.Rows(0).Item("adoc_infavor_comp_id").ToString.Trim)
                Else
                    fAdoc_infavor_comp_id = 0
                End If

                If Not IsDBNull(RefTable.Rows(0).Item("adoc_infavor_text")) And Not String.IsNullOrEmpty(RefTable.Rows(0).Item("adoc_infavor_text").ToString) Then
                    fAdoc_infavor_text = RefTable.Rows(0).Item("adoc_infavor_text").ToString.Trim
                Else
                    fAdoc_infavor_text = ""
                End If

                If Not IsDBNull(RefTable.Rows(0).Item("adoc_journ_seq_no")) And Not String.IsNullOrEmpty(RefTable.Rows(0).Item("adoc_journ_seq_no").ToString) Then
                    fAdoc_journ_seq_no = CInt(RefTable.Rows(0).Item("adoc_journ_seq_no").ToString.Trim)
                Else
                    fAdoc_journ_seq_no = 0
                End If

                If Not IsDBNull(RefTable.Rows(0).Item("adoc_doc_type")) And Not String.IsNullOrEmpty(RefTable.Rows(0).Item("adoc_doc_type").ToString) Then
                    fAdoc_doc_type = RefTable.Rows(0).Item("adoc_doc_type").ToString.Trim
                Else
                    fAdoc_doc_type = ""
                End If

                If Not IsDBNull(RefTable.Rows(0).Item("adoc_journ_id")) And Not String.IsNullOrEmpty(RefTable.Rows(0).Item("adoc_journ_id").ToString) Then
                    fAdoc_journ_id = CLng(RefTable.Rows(0).Item("adoc_journ_id").ToString.Trim)
                Else
                    fAdoc_journ_id = 0
                End If

                If Not IsDBNull(RefTable.Rows(0).Item("adoc_hide_flag")) And Not String.IsNullOrEmpty(RefTable.Rows(0).Item("adoc_hide_flag").ToString) Then
                    fAdoc_hide_flag = RefTable.Rows(0).Item("adoc_hide_flag").ToString.Trim
                Else
                    fAdoc_hide_flag = ""
                End If

                If Not IsDBNull(RefTable.Rows(0).Item("doctype_subdir_name")) And Not String.IsNullOrEmpty(RefTable.Rows(0).Item("doctype_subdir_name").ToString) Then
                    fDoctype_subdir_name = RefTable.Rows(0).Item("doctype_subdir_name").ToString.Trim
                Else
                    fDoctype_subdir_name = ""
                End If

                If Not IsDBNull(RefTable.Rows(0).Item("doctype_file_extension")) And Not String.IsNullOrEmpty(RefTable.Rows(0).Item("doctype_file_extension").ToString) Then
                    fDoctype_file_extension = RefTable.Rows(0).Item("doctype_file_extension").ToString.Trim
                Else
                    fDoctype_file_extension = ""
                End If

                If fAdoc_hide_flag.ToUpper <> "Y" Then

                    ' GET THE FILE NAME FOR THE ELECTRONIC DOCUMENT
                    ' hDocumentFile = commonEVO.Get_Document_File_Name(nAircraftID, fAdoc_journ_id, fAdoc_journ_seq_no, fDoctype_subdir_name, fDoctype_file_extension, MyAppState, MySesState)

                    If Not String.IsNullOrEmpty(hDocumentFile) Then
                        If System.IO.File.Exists(HttpContext.Current.Server.MapPath(hDocumentFile)) Then
                            sHtmlOut.Append("<a href='" + IIf(crmView = False, hDocumentFile, "#") + "' target='_new'><img align='absmiddle' src='images/DocumentSM.gif' border='0' alt='" + fAdoc_doc_type + "'></a>")
                        Else
                            sHtmlOut.Append("<img align='absmiddle' src='images/DocumentSM.gif' border='0' alt=''>")
                        End If ' Document_Exists(hDocumentFile) 
                    Else
                        sHtmlOut.Append("<img align='absmiddle' src='images/DocumentSM.gif' border='0' alt=''>")
                    End If ' hDocumentFile <> "" 		  
                Else
                    sHtmlOut.Append("&nbsp;")
                End If ' fAdoc_hide_flag <> "Y" 

            End If ' isView

        End If


        out_html = sHtmlOut.ToString

        Return True

        sHtmlOut = Nothing

    End Function

    Public Shared Function GetOwnershipType(ByVal in_AircraftOwnerType As String, ByVal aclsData_Temp As clsData_Manager_SQL) As String
        Dim OwnershipType As New DataTable
        Dim htmlOut As StringBuilder = New StringBuilder()
        Dim sQuery As String = ""

        OwnershipType = aclsData_Temp.GetOwnershipType(in_AircraftOwnerType.ToUpper.Trim)

        If Not IsNothing(OwnershipType) Then

            If OwnershipType.Rows.Count > 0 Then
                If Not IsDBNull(OwnershipType.Rows(0).Item("acot_name")) And Not String.IsNullOrEmpty(OwnershipType.Rows(0).Item("acot_name").ToString) Then
                    htmlOut.Append(OwnershipType.Rows(0).Item("acot_name").ToString)
                End If

            End If
        End If

        If String.IsNullOrEmpty(htmlOut.ToString) Then
            htmlOut.Append("&lt;Unknown&gt;")
        End If

        Return htmlOut.ToString.Trim

    End Function
    ''' <summary>
    ''' This displays the equipment details on the Aircraft Details page.
    ''' </summary>
    ''' <param name="aclsData_Temp">Datalayer object</param>
    ''' <param name="MySesState">Session State</param>
    ''' <param name="in_AircraftRs">Receiving the aircraft datatable</param>
    ''' <returns>returns text</returns>
    ''' <remarks></remarks>
    Public Shared Function DisplayEquipmentDetails(ByVal aclsData_Temp As clsData_Manager_SQL, ByRef MySesState As HttpSessionState,
                                               ByRef in_AircraftRs As DataTable, ByRef crmSource As String) As String

        Dim htmlOut As StringBuilder = New StringBuilder()
        Dim Results_Table As New DataTable
        Dim rowCounter As Integer = 0

        If crmSource = "CLIENT" Then
            Results_Table = aclsData_Temp.Get_Client_Aircraft_Details_As_Jetnet_Fields(in_AircraftRs.Rows(0).Item("CLIENT_ID"), "equipment")
        Else
            Results_Table = aclsData_Temp.GetJETNET_Aircraft_Details_Equipment_AC_ID_TYPE(in_AircraftRs.Rows(0).Item("ac_id"), "equipment", in_AircraftRs.Rows(0).Item("ac_journ_id"))
        End If

        htmlOut.Append("<table cellpadding='0' cellspacing='0' width='100%'>")
        htmlOut.Append("<tr>")
        If Results_Table.Rows.Count > 0 Then

            For Each r As DataRow In Results_Table.Rows
                htmlOut.Append("</tr><tr>")
                'htmlOut.Append("<td valign='top' align='left' width='5'>&nbsp;&nbsp;</td>")
                htmlOut.Append("<td valign='top' align='left'>")


                If Not IsDBNull(r("adet_data_name")) And Not String.IsNullOrEmpty(r("adet_data_name").ToString) Then
                    htmlOut.Append("<span class='li'><span class='label'>" + r("adet_data_name").ToString.Trim + ":</span> ")

                    If Not IsDBNull(r("adet_data_description")) And Not String.IsNullOrEmpty(r("adet_data_description").ToString) Then
                        htmlOut.Append(r("adet_data_description").ToString.Trim)
                    Else
                        htmlOut.Append("")
                    End If

                    htmlOut.Append("</span>")
                    htmlOut.Append("</td>")
                Else
                    htmlOut.Append("")
                End If
                rowCounter += 1
            Next

        Else

            htmlOut.Append("<td valign='top' align='left'>")
            htmlOut.Append("<span class='li'>No Equipment Details</span>")
            htmlOut.Append("</td>")
        End If

        htmlOut.Append("</tr></table>")
        Results_Table = Nothing

        Return htmlOut.ToString.Trim

    End Function

    ''' <summary>
    ''' This displays the maintenance details on the AC details page. Accepts Datatable and returns a string
    ''' </summary>
    ''' <param name="aclsData_Temp">datalayer object</param>
    ''' <param name="MySesState">session state</param>
    ''' <param name="in_AircraftRs">datatable with AC data</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function DisplayMaintenanceDetails(ByVal aclsData_Temp As clsData_Manager_SQL, ByRef MySesState As HttpSessionState,
                                                     ByRef in_AircraftRs As DataTable, ByRef crmSource As String, ByRef bShowBlankAcFields As Boolean, ByRef DamageCode As String) As String

        'This has been added on 3/17/16.
        'If bShowBlankAcFields is true, we are going to show the following fields, regardless of if they're blank or not.
        'Maintained, Engine Overhaul By, Hot Inspection, Damage History Notes.

        Dim htmlOut As StringBuilder = New StringBuilder()
        Dim ResultsTable As New DataTable
        Dim rowCounter As Integer = 0
        Dim FieldDisplay As String = ""
        Dim temp_count As Integer = 0
        Dim temp_date As String = ""
        Dim cssClass As String = "alt_row"
        Dim dateFormatted As String = "MM/dd/yyyy"

        '''''''Maintained By Field:
        If bShowBlankAcFields Or (Not IsDBNull(in_AircraftRs.Rows(0).Item("ac_maintained")) And Not String.IsNullOrEmpty(in_AircraftRs.Rows(0).Item("ac_maintained").ToString)) Then 'We're going to show this label regardless:
            If Not IsDBNull(in_AircraftRs.Rows(0).Item("ac_maintained")) And Not String.IsNullOrEmpty(in_AircraftRs.Rows(0).Item("ac_maintained").ToString) Then
                FieldDisplay += in_AircraftRs.Rows(0).Item("ac_maintained").ToString.Trim + ""
            End If
        End If

        If FieldDisplay <> "" Or bShowBlankAcFields Then
            FieldDisplay = "<tr><td valign='top' align='left'><span class='li'><span class='label'>Maintained:</span> " & FieldDisplay & "</span></td></tr>"
        End If
        htmlOut.Append(FieldDisplay)
        FieldDisplay = ""
        '''''''End Maintained By Field
        '''''''Airframe Maintenance Program
        If Not IsDBNull(in_AircraftRs.Rows(0).Item("amp_program_name")) And Not String.IsNullOrEmpty(in_AircraftRs.Rows(0).Item("amp_program_name").ToString) Then
            FieldDisplay = in_AircraftRs.Rows(0).Item("amp_program_name")
            If bShowBlankAcFields = False Then
                If UCase(Trim(FieldDisplay)) = "UNKNOWN" Then
                    FieldDisplay = ""
                End If
            End If
        End If

        If FieldDisplay <> "" Or bShowBlankAcFields Then
            FieldDisplay = "<tr><td valign='top' align='left'><span class='li'><span class='label'>Airframe Maintenance Program:</span> " & FieldDisplay & "</span></td></tr>"
        End If
        htmlOut.Append(FieldDisplay)
        FieldDisplay = ""

        '''''''Aiframe Maintenance Tracking Program
        If Not IsDBNull(in_AircraftRs.Rows(0).Item("amtp_program_name")) And Not String.IsNullOrEmpty(in_AircraftRs.Rows(0).Item("amtp_program_name").ToString) Then
            FieldDisplay = in_AircraftRs.Rows(0).Item("amtp_program_name")
            If bShowBlankAcFields = False Then
                If UCase(Trim(FieldDisplay)) = "UNKNOWN" Then
                    FieldDisplay = ""
                End If
            End If
        End If

        If FieldDisplay <> "" Or bShowBlankAcFields Then
            FieldDisplay = "<tr><td valign='top' align='left'><span class='li'><span class='label'>Airframe Maintenance Tracking Program:</span> " & FieldDisplay & "</span></td></tr>"
        End If
        htmlOut.Append(FieldDisplay)
        FieldDisplay = ""

        '''''''''''Engine Overhaul By Field:
        If Not IsDBNull(in_AircraftRs.Rows(0).Item("ac_maint_eoh_by_name")) And Not String.IsNullOrEmpty(in_AircraftRs.Rows(0).Item("ac_maint_eoh_by_name").ToString) Then
            FieldDisplay += "By " & in_AircraftRs.Rows(0).Item("ac_maint_eoh_by_name").ToString.Trim

            If Not IsDBNull(in_AircraftRs.Rows(0).Item("ac_main_eoh_moyear")) And Not String.IsNullOrEmpty(in_AircraftRs.Rows(0).Item("ac_main_eoh_moyear").ToString) Then
                FieldDisplay += "&nbsp; On " + in_AircraftRs.Rows(0).Item("ac_main_eoh_moyear").ToString.Substring(0, 2) + "/" + in_AircraftRs.Rows(0).Item("ac_main_eoh_moyear").ToString.Substring(2, 4) + ""
            Else
                FieldDisplay += "&nbsp;"
            End If
        Else 'only add unknown if the date is there, otherwise it is not needed. 
            If Not IsDBNull(in_AircraftRs.Rows(0).Item("ac_main_eoh_moyear")) And Not String.IsNullOrEmpty(in_AircraftRs.Rows(0).Item("ac_main_eoh_moyear").ToString) Then
                If bShowBlankAcFields = True Then
                    FieldDisplay += "By Unknown"
                End If

                If Not IsDBNull(in_AircraftRs.Rows(0).Item("ac_main_eoh_moyear")) And Not String.IsNullOrEmpty(in_AircraftRs.Rows(0).Item("ac_main_eoh_moyear").ToString) Then
                    FieldDisplay += "&nbsp;On " + in_AircraftRs.Rows(0).Item("ac_main_eoh_moyear").ToString.Substring(0, 2) + "/" + in_AircraftRs.Rows(0).Item("ac_main_eoh_moyear").ToString.Substring(2, 4) + ""
                Else
                    FieldDisplay += "&nbsp;"
                End If
            End If
        End If

        If FieldDisplay <> "" Or bShowBlankAcFields Then
            FieldDisplay = "<tr><td valign='top' align='left'><span class='li'><span class='label'>Engine Overhaul:</span> " & FieldDisplay & "</span></td></tr>"
        End If
        htmlOut.Append(FieldDisplay)
        FieldDisplay = ""
        '''''''''''End Engine Overhaul By Field

        '''''''''''Hot Inspection By Field:
        If Not IsDBNull(in_AircraftRs.Rows(0).Item("ac_maint_hots_by_name")) And Not String.IsNullOrEmpty(in_AircraftRs.Rows(0).Item("ac_maint_hots_by_name").ToString) Then
            FieldDisplay = " By " + in_AircraftRs.Rows(0).Item("ac_maint_hots_by_name").ToString.Trim + ""
        End If

        If Not IsDBNull(in_AircraftRs.Rows(0).Item("ac_maint_hots_moyear")) And Not String.IsNullOrEmpty(in_AircraftRs.Rows(0).Item("ac_maint_hots_moyear").ToString) Then
            FieldDisplay += "&nbsp; On" + in_AircraftRs.Rows(0).Item("ac_maint_hots_moyear").ToString.Substring(0, 2) + "/" + in_AircraftRs.Rows(0).Item("ac_maint_hots_moyear").ToString.Substring(2, 4) + ""
        End If

        If FieldDisplay <> "" Or bShowBlankAcFields Then
            FieldDisplay = "<tr><td valign='top' align='left'><span class='li'><span class='label'>Hot Inspection:</span> " & FieldDisplay & "</span></td></tr>"
        End If
        htmlOut.Append(FieldDisplay)
        FieldDisplay = ""
        ''''''''''''End Hot Inspection Field

        ''''''''''''AC Damage History Note
        If Not IsDBNull(in_AircraftRs.Rows(0).Item("ac_damage_history_notes")) And Not String.IsNullOrEmpty(in_AircraftRs.Rows(0).Item("ac_damage_history_notes").ToString) Then
            FieldDisplay += "" + in_AircraftRs.Rows(0).Item("ac_damage_history_notes").ToString.Trim + ""
        End If

        If FieldDisplay <> "" Or bShowBlankAcFields Then
            FieldDisplay = "<tr><td valign='top' align='left'><span class='li'><span class='label'>Damage History:</span> " & FieldDisplay & "</span></td></tr>"
        ElseIf DamageCode <> "" Then
            Select Case DamageCode
                Case "Y"
                    FieldDisplay = "<tr><td valign='top' align='left'><span class='li'><span class='label'>Damage: YES</span></td></tr>"
                Case "A"
                    FieldDisplay = "<tr><td valign='top' align='left'><span class='li'><span class='label'>Damage: ACCIDENT</span></td></tr>"
                Case "I"
                    FieldDisplay = "<tr><td valign='top' align='left'><span class='li'><span class='label'>Damage: INCIDENT</span></td></tr>"
            End Select

        End If
        htmlOut.Append(FieldDisplay)
        FieldDisplay = ""
        ''''''''''''End AC Damage History Note



        If crmSource = "CLIENT" Then
            ResultsTable = aclsData_Temp.Get_Client_Aircraft_Details_As_Jetnet_Fields(in_AircraftRs.Rows(0).Item("CLIENT_ID"), "maintenance")
        Else
            ResultsTable = aclsData_Temp.GetJETNET_Aircraft_Details_Equipment_AC_ID_TYPE(in_AircraftRs.Rows(0).Item("ac_id"), "maintenance", in_AircraftRs.Rows(0).Item("ac_journ_id"))
        End If

        If Not IsNothing(ResultsTable) Then
            If ResultsTable.Rows.Count > 0 Then
                For Each r As DataRow In ResultsTable.Rows

                    'htmlOut.Append("<td valign='top' align='left' width='5'>&nbsp;&nbsp;</td>")
                    htmlOut.Append("<td valign='top' align='left'>")

                    If Not IsDBNull(r("adet_data_name")) And Not String.IsNullOrEmpty(r("adet_data_name").ToString) Then
                        htmlOut.Append("<span class='li'><span class='label'>" + Replace(r("adet_data_name").ToString.Trim, "Inspection", "Notes") + ": </span>")
                        If Not IsDBNull(r("adet_data_description")) And Not String.IsNullOrEmpty(r("adet_data_description").ToString) Then
                            htmlOut.Append("" + r("adet_data_description").ToString.Trim)
                        End If
                        htmlOut.Append("</span>")
                    End If

                    htmlOut.Append("</td>")
                    htmlOut.Append("</tr><tr>")
                    rowCounter += 1

                Next
                htmlOut.Append("</tr>")
            End If
        End If
        ResultsTable = New DataTable



        ' If (HttpContext.Current.Session.Item("jetnetWebSiteType") = crmWebClient.eWebSiteTypes.LOCAL Or HttpContext.Current.Session.Item("jetnetWebSiteType") = crmWebClient.eWebSiteTypes.TEST) Then
        '  If HttpContext.Current.Session.Item("localUser").crmLocalUserEmailAddress.ToString.ToLower.Contains("@jetnet.com") Or HttpContext.Current.Session.Item("localUser").crmLocalUserEmailAddress.ToString.ToLower.Contains("@mvintech.com") Then

        ResultsTable = aclsData_Temp.GetJETNET_Aircraft_Maintenance_BY_ACID(in_AircraftRs.Rows(0).Item("ac_id"), in_AircraftRs.Rows(0).Item("ac_journ_id"))

        htmlOut.Append("<tr><td align=""left"" valign=""top"" class=""padding"">")
        ' htmlOut.Append("<a href=""javascript:void(0);"" onclick=""javascript:load('maintenance.aspx?acID=" & in_AircraftRs.Rows(0).Item("ac_id") & "&jID=" & in_AircraftRs.Rows(0).Item("ac_journ_id") & "','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');"" class=""float_right"">Edit Maintenance</a>")

        If Not IsNothing(ResultsTable) Then

            If ResultsTable.Rows.Count > 0 Then

                htmlOut.Append("<table border='0' cellspacing='0' cellpadding='3' class='engine_tab maintTab' width=""100%"">")
                htmlOut.Append("<tr><td valign='top' align='left' class=""header_row""><b>Maintenance/Inspections</b></td>")
                htmlOut.Append("<td valign='top' align='center' class=""header_row maxwidth_date""><b class=""text_underline help_cursor"" title=""Complied With or Completed"" alt=""Complied with or Completed"">C/W</b></td>")
                htmlOut.Append("<td valign='top' align='center' class=""header_row""><b><span class=""text_underline help_cursor"" title=""Complied With or Completed"" alt=""Complied with or Completed"">C/W</span> Hrs</b></td>")
                htmlOut.Append("<td valign='top' align='center' class=""header_row maxwidth_date""><b>Due</b></td>")
                htmlOut.Append("<td valign='top' align='center' class=""header_row""><b>Due Hrs</b></td>")
                htmlOut.Append("<td valign='top' align='left' class=""header_row""><b>Notes</b></td>")
                htmlOut.Append("</tr>")

                Try

                    For Each r As DataRow In ResultsTable.Rows

                        temp_count = temp_count + 1

                        'htmlOut.Append("<td valign='top' align='left' width='5'>&nbsp;&nbsp;</td>")
                        htmlOut.Append("<tr class=""" & cssClass & """>")

                        htmlOut.Append("<td valign='top' align='left'>")
                        If Not IsDBNull(r("acmaint_name")) And Not String.IsNullOrEmpty(r("acmaint_name").ToString) Then
                            htmlOut.Append("" + r("acmaint_name").ToString.Trim + " ")
                        End If
                        htmlOut.Append("&nbsp;</td>")

                        If Not IsDBNull(r("acmaint_date_type")) And Not String.IsNullOrEmpty(r("acmaint_date_type")) Then
                            Select Case UCase(r("acmaint_date_type"))
                                Case "D"
                                    dateFormatted = "MM/dd/yy"
                                Case "M"
                                    dateFormatted = "MM/yy"
                                Case "Y"
                                    dateFormatted = "yyyy"
                            End Select
                        End If

                        htmlOut.Append("<td valign='top' align='center'>")
                        If Not IsDBNull(r("acmaint_complied_date")) And Not String.IsNullOrEmpty(r("acmaint_complied_date").ToString) Then
                            'temp_date = FormatDateTime(r("acmaint_complied_date").ToString.Trim, DateFormat.ShortDate)
                            ' temp_date = Month(temp_date) & "/" & Day(temp_date) & "/" & Right(Year(Trim(temp_date)), 2)
                            temp_date = Format(r("acmaint_complied_date"), dateFormatted)
                            htmlOut.Append(temp_date)
                        End If
                        htmlOut.Append("</td>")

                        htmlOut.Append("<td valign='top' align='center'>")
                        If Not IsDBNull(r("acmaint_complied_hrs")) And Not String.IsNullOrEmpty(r("acmaint_complied_hrs").ToString) Then
                            If IsNumeric(r("acmaint_complied_hrs")) Then
                                If r("acmaint_complied_hrs") > 0 Then
                                    htmlOut.Append("" + r("acmaint_complied_hrs").ToString.Trim)
                                End If
                            End If
                        End If
                        htmlOut.Append("</td>")

                        htmlOut.Append("<td valign='top' align='center'>")
                        If Not IsDBNull(r("acmaint_due_date")) And Not String.IsNullOrEmpty(r("acmaint_due_date").ToString) Then
                            'temp_date = FormatDateTime(r("acmaint_due_date").ToString.Trim, DateFormat.ShortDate)
                            'temp_date = Month(temp_date) & "/" & Day(temp_date) & "/" & Right(Year(Trim(temp_date)), 2)
                            temp_date = Format(r("acmaint_due_date"), dateFormatted)
                            htmlOut.Append("" & temp_date)
                        End If
                        htmlOut.Append("</td>")
                        htmlOut.Append("<td valign='top' align='center'>")
                        If Not IsDBNull(r("acmaint_due_hrs")) And Not String.IsNullOrEmpty(r("acmaint_due_hrs").ToString) Then
                            If IsNumeric(r("acmaint_due_hrs")) Then
                                If r("acmaint_due_hrs") > 0 Then
                                    htmlOut.Append("" + r("acmaint_due_hrs").ToString.Trim)
                                End If
                            End If
                        End If
                        htmlOut.Append("</td>")

                        htmlOut.Append("<td valign='top' align='left'>")
                        If Not IsDBNull(r("acmaint_notes")) And Not String.IsNullOrEmpty(r("acmaint_notes").ToString) Then
                            If InStr(Trim(r("acmaint_notes").ToString), "as reported") > 0 Then
                                htmlOut.Append("" + Replace(r("acmaint_notes").ToString.Trim, "as reported", "Date(s) as reported/not actual."))
                            Else
                                htmlOut.Append("" + r("acmaint_notes").ToString.Trim)
                            End If
                        End If
                        htmlOut.Append("</td>")

                        htmlOut.Append("</tr>")
                        rowCounter += 1

                        If cssClass = "" Then
                            cssClass = "alt_row"
                        Else
                            cssClass = ""
                        End If
                    Next

                Catch ex As Exception
                Finally
                    htmlOut.Append("</table>")
                End Try

            End If


        End If
        htmlOut.Append("</td></tr>")
        ResultsTable = New DataTable
        '   End If
        '  End If

        FieldDisplay = GetCertificationsInfo(in_AircraftRs.Rows(0).Item("ac_id"), in_AircraftRs.Rows(0).Item("ac_journ_id"), aclsData_Temp)
        If bShowBlankAcFields = False Then
            If UCase(Trim(FieldDisplay)) = "UNKNOWN" Then
                FieldDisplay = ""
            End If
        End If

        If FieldDisplay <> "" Or bShowBlankAcFields Then
            FieldDisplay = "<tr><td valign='top' align='left'><span class='li'><span class='label'>Certifications:</span> " & FieldDisplay & "</span></td></tr>"
        End If
        htmlOut.Append(FieldDisplay)
        FieldDisplay = ""

        FieldDisplay = htmlOut.ToString
        htmlOut = New StringBuilder

        If FieldDisplay = "" Then
            FieldDisplay = "<tr><td align=""left"" valign=""top""><span class=""li"">No Maintenance Details</span></td></tr>"
        End If


        htmlOut.Append("<table cellpadding='0' cellspacing='0' width='100%'>")
        htmlOut.Append(FieldDisplay)
        htmlOut.Append("</table>")

        Return htmlOut.ToString.Trim

    End Function

    Public Shared Function GetCertificationsInfo(ByVal acID As Long, ByVal journalID As Long, ByVal aclsData_Temp As clsData_Manager_SQL) As String
        Dim htmlOut As StringBuilder = New StringBuilder()
        Dim CertTable As New DataTable

        CertTable = aclsData_Temp.GetJETNET_AC_Certs(acID, journalID)

        If Not IsNothing(CertTable) Then
            If CertTable.Rows.Count > 0 Then
                For Each r As DataRow In CertTable.Rows
                    If Not String.IsNullOrEmpty(htmlOut.ToString) Then
                        htmlOut.Append(", ")
                    End If

                    If Not IsDBNull(r("accert_name")) Then
                        If Not String.IsNullOrEmpty(r("accert_name").ToString) Then
                            htmlOut.Append(r("accert_name").ToString)
                        End If
                    End If
                Next

            End If
        End If

        If String.IsNullOrEmpty(htmlOut.ToString) Then
            htmlOut.Append("Unknown")
        End If

        CertTable.Dispose()
        Return htmlOut.ToString.Trim

    End Function

    ''' <summary>
    ''' Display Usage Information. Accepts Datatable and returns string
    ''' </summary>
    ''' <param name="in_AircraftRs">datatable with AC data</param>
    ''' <remarks></remarks>
    Public Shared Function DisplayUsageInfo(ByRef in_AircraftRs As DataTable) As String
        Dim htmlOut As StringBuilder = New StringBuilder()

        htmlOut.Append("<table cellpadding='0' cellspacing='0' width='100%'>")
        'If Not IsNothing(usage_tab) Then
        '  usage_tab.headerText = "AIRFRAME"
        'End If
        htmlOut.Append("<tr>")
        'If Not IsNothing(usage_tab) Then
        '  If Not IsDBNull(in_AircraftRs.Rows(0).Item("ac_date_engine_times_as_of")) And Not String.IsNullOrEmpty(in_AircraftRs.Rows(0).Item("ac_date_engine_times_as_of").ToString) Then
        '    usage_tab.headerText = "AIRFRAME: TIMES AS OF " & FormatDateTime(in_AircraftRs.Rows(0).Item("ac_date_engine_times_as_of").ToString, DateFormat.ShortDate)
        '  End If
        'End If

        htmlOut.Append("<td valign='top' align='left' width='50%'>")
        htmlOut.Append("<span class='li'><span class='label'>Air Frame Total Time (AFTT)&nbsp;:</span>&nbsp;")
        If Not IsDBNull(in_AircraftRs.Rows(0).Item("ac_airframe_total_hours")) And Not String.IsNullOrEmpty(in_AircraftRs.Rows(0).Item("ac_airframe_total_hours").ToString) Then

            htmlOut.Append(FormatNumber(CDbl(in_AircraftRs.Rows(0).Item("ac_airframe_total_hours").ToString), 0, True, False, True) + "</span>")
        End If
        htmlOut.Append("</td>")

        htmlOut.Append("<td valign='top' align='left' width='50%'><span class='li'><span class='label'>Landings / Cycles&nbsp;:</span>&nbsp;")
        If Not IsDBNull(in_AircraftRs.Rows(0).Item("ac_airframe_total_landings")) And Not String.IsNullOrEmpty(in_AircraftRs.Rows(0).Item("ac_airframe_total_landings").ToString) Then
            htmlOut.Append(FormatNumber(CDbl(in_AircraftRs.Rows(0).Item("ac_airframe_total_landings").ToString), 0, True, False, True) + "</span>")
        End If
        htmlOut.Append("</td>")
        htmlOut.Append("</tr>")


        If Not IsDBNull(in_AircraftRs.Rows(0).Item("ac_est_airframe_hrs")) And Not String.IsNullOrEmpty(in_AircraftRs.Rows(0).Item("ac_est_airframe_hrs").ToString) Then
            htmlOut.Append("<tr>")
            htmlOut.Append("<td valign='top' align='left' width='100%' colspan='2'>")
            htmlOut.Append("<span class='li'><span class='label'>Estimated Airframe Total Time at time of Sale&nbsp;:</span>&nbsp;")
            htmlOut.Append(FormatNumber(CDbl(in_AircraftRs.Rows(0).Item("ac_est_airframe_hrs").ToString), 0, True, False, True) + "</span>")
            htmlOut.Append("</td>")
            htmlOut.Append("</tr>")
        End If

        htmlOut.Append("</table>")

        Return htmlOut.ToString.Trim

    End Function

    ''' <summary>
    ''' Displays a vertical representation of the engine details
    ''' </summary>
    ''' <param name="MySesState">session state</param>
    ''' <param name="in_AircraftRs">aircraft data filled table</param>
    ''' <param name="engine_tab">engine tab to set header text</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function DisplayEngineInfo_Vertical(ByRef MySesState As HttpSessionState, ByRef in_AircraftRs As DataTable, ByVal engine_tab As Object, Optional ByVal bShowBlankAcFields As Boolean = False, Optional ByVal from_spot As String = "") As String
        Dim engine As String = ""
        Dim sn As String = ""
        Dim ttsnew As String = ""
        Dim soh As String = ""
        Dim shi As String = ""
        Dim tbo As String = ""
        Dim tcsn As String = ""
        Dim tcso As String = ""
        Dim tcsh As String = ""
        Dim FieldDisplay As String = ""

        Dim htmlOut As StringBuilder = New StringBuilder()
        Dim nloopCount As Integer = 0
        'Row count check added 1/21/2016 to prevent erroring if no rows.
        If Not IsNothing(in_AircraftRs) Then
            If in_AircraftRs.Rows.Count > 0 Then

                'Engine Block, Do not display blank lines in table if optional bShowBlankAcFields is set to false. Defaults to false. Added 3/17/16.
                'Adding 7 variables here. Booleans. They'll all default to false, meaning do not display them.
                'As the variables are written to their respective strings, if a field is non null/blank, it's going to go ahead and turn the boolean to true.
                Dim TTSN_Display_Check As Boolean = False
                Dim SOH_Display_Check As Boolean = False
                Dim SHI_Display_Check As Boolean = False
                Dim TBO_Display_Check As Boolean = False
                Dim TCSN_Display_Check As Boolean = False

                Dim TCSO_Display_Check As Boolean = False
                Dim TCSH_Display_Check As Boolean = False

                Dim sAirframeType As String = in_AircraftRs.Rows(0).Item("amod_airframe_type").ToString.ToUpper.Trim
                Dim sAircraftType As String = in_AircraftRs.Rows(0).Item("amod_make_type").ToString.ToUpper.Trim
                Dim number_of_engines As Integer = 0

                If Not IsDBNull(in_AircraftRs.Rows(0).Item("amod_number_of_engines")) Then
                    number_of_engines = in_AircraftRs.Rows(0).Item("amod_number_of_engines")
                End If

                htmlOut.Append("<tr><td align=""left"" valign=""top"" colspan=""2""><table width='99%' cellpadding='3' cellspacing='0' border='0' align=""center"" class='formatTable blue'>")
                'htmlOut.Append("<tr>")



                'htmlOut.Append("<td valign='top' align='left' colspan='" & number_of_engines + 1 & "'><span class='li'><span class='label'>On&nbsp;Condition&nbsp;TBO&nbsp;:</span> ")

                'If in_AircraftRs.Rows(0).Item("ac_engine_tbo_oc_flag").ToString.ToUpper = "Y" Then
                '  htmlOut.Append("&nbsp;Yes")
                'Else
                '  htmlOut.Append("&nbsp;No")
                'End If
                'htmlOut.Append("</span>")

                'If Not IsDBNull(in_AircraftRs.Rows(0).Item("emp_provider_name")) Then
                '  If Not String.IsNullOrEmpty(in_AircraftRs.Rows(0).Item("emp_provider_name").ToString) Then
                '    FieldDisplay = in_AircraftRs.Rows(0).Item("emp_provider_name").ToString
                '    If bShowBlankAcFields = False Then
                '      If UCase(Trim(FieldDisplay)) = "UNKNOWN" Then
                '        FieldDisplay = ""
                '      End If
                '    End If
                '  End If
                'End If

                'If Not IsDBNull(in_AircraftRs.Rows(0).Item("emp_program_name")) Then
                '  If Not String.IsNullOrEmpty(in_AircraftRs.Rows(0).Item("emp_program_name").ToString) Then

                '    If bShowBlankAcFields = False Then
                '      If UCase(Trim(in_AircraftRs.Rows(0).Item("emp_program_name").ToString)) = "UNKNOWN" Then
                '        FieldDisplay = ""
                '      Else
                '        FieldDisplay += "&nbsp;-&nbsp;" + in_AircraftRs.Rows(0).Item("emp_program_name").ToString
                '      End If
                '    Else
                '      FieldDisplay += "&nbsp;-&nbsp;" + in_AircraftRs.Rows(0).Item("emp_program_name").ToString
                '    End If

                '  End If
                'End If

                'If FieldDisplay <> "" Then
                '  FieldDisplay = "<span class='li'><span class='label'>Maintenance&nbsp;Program:</span> " & FieldDisplay & "</span>"
                'End If

                'htmlOut.Append(FieldDisplay)
                'FieldDisplay = ""


                'If Not IsDBNull(in_AircraftRs.Rows(0).Item("emgp_program_name")) Then
                '  If Not String.IsNullOrEmpty(in_AircraftRs.Rows(0).Item("emgp_program_name").ToString) Then
                '    FieldDisplay = in_AircraftRs.Rows(0).Item("emgp_program_name").ToString
                '    If bShowBlankAcFields = False Then
                '      If UCase(Trim(FieldDisplay)) = "UNKNOWN" Then
                '        FieldDisplay = ""
                '      End If
                '    End If
                '  End If
                'End If

                'If FieldDisplay <> "" Then
                '  FieldDisplay = "<span class='li'><span class='label'>Management&nbsp;Program:</span> " & FieldDisplay & "</span>"
                'End If

                'htmlOut.Append(FieldDisplay)
                'FieldDisplay = ""


                'htmlOut.Append("</td>")

                If Trim(from_spot) = "pdf" Then
                    sn = ("<tr><td align='left' valign='top'  class='header' width='400'>Serial Number</td>")
                    ttsnew = ("<tr><td align='left' valign='top' class='header' width='400'>Total Time Since New (TTSNEW) Hrs</td>")
                    soh = ("<tr><td align='left' valign='top' class='header' width='400'>Since Overhaul (SOH/SCOR) Hrs</td>")
                    shi = ("<tr><td align='left' valign='top' class='header' width='400'>Since Hot Inspection (SHI/SMPI) Hrs</td>")
                    tbo = ("<tr><td align='left' valign='top' class='header' width='400'>Time Between Overhaul (TBO/TBCI) Hrs</td>")
                    tcsn = ("<tr><td align='left' valign='top' class='header' width='400'>Total Cycles Since New</td>")
                    tcso = ("<tr><td align='left' valign='top' class='header' width='400'>Total Cycles Since Overhaul</td>")
                    tcsh = ("<tr><td align='left' valign='top' class='header' width='400'>Total Cycles Since Hot</td>")
                Else
                    sn = ("<tr><td align='left' valign='top'  class='header' width='142'>Serial Number</td>")
                    ttsnew = ("<tr><td align='left' valign='top' class='header'>Total Time Since New (TTSNEW) Hrs</td>")
                    soh = ("<tr><td align='left' valign='top' class='header'>Since Overhaul (SOH/SCOR) Hrs</td>")
                    shi = ("<tr><td align='left' valign='top' class='header'>Since Hot Inspection (SHI/SMPI) Hrs</td>")
                    tbo = ("<tr><td align='left' valign='top' class='header'>Time Between Overhaul (TBO/TBCI) Hrs</td>")
                    tcsn = ("<tr><td align='left' valign='top' class='header'>Total Cycles Since New</td>")
                    tcso = ("<tr><td align='left' valign='top' class='header'>Total Cycles Since Overhaul</td>")
                    tcsh = ("<tr><td align='left' valign='top' class='header'>Total Cycles Since Hot</td>")
                End If


                If sAirframeType <> "R" Then
                    nloopCount = 4
                Else
                    nloopCount = 3
                End If

                nloopCount = number_of_engines
                engine = "<tr>"
                If Not IsNothing(engine_tab) Then
                    If Not IsDBNull(in_AircraftRs.Rows(0).Item("ac_engine_name")) And Not String.IsNullOrEmpty(in_AircraftRs.Rows(0).Item("ac_engine_name").ToString) Then
                        engine_tab.headertext = "ENGINE: MODEL " & in_AircraftRs.Rows(0).Item("ac_engine_name").ToString.Trim
                        engine += "<td valign='middle' align='left'>&nbsp;</td>"
                    Else
                        engine += "<td valign='middle' align='left'>&nbsp;</td>"
                    End If
                Else
                    engine += "<td valign='middle' align='left'><span class='label'><b class=""upperCase"">Model</b></span> " & in_AircraftRs.Rows(0).Item("ac_engine_name").ToString.Trim & "</td>"
                End If

                For xLoop As Integer = 1 To nloopCount

                    'If xLoop = 1 And Not sAirframeType.Contains("R") Then
                    '  engine += ("<td valign='top' align='center' class='engine_" & xLoop & " header'><b>Engine&nbsp;" + xLoop.ToString + "&nbsp;&nbsp;&nbsp;(Left)</b></td>")
                    'ElseIf xLoop = 2 And Not sAirframeType.Contains("R") Then
                    '  engine += ("<td valign='top' align='center' class='engine_" & xLoop & " header'><b>Engine&nbsp;" + xLoop.ToString + "&nbsp;(Right)</b></td>")
                    'ElseIf xLoop = 3 And Not sAirframeType.Contains("R") Then
                    '  engine += ("<td valign='top' align='center' class='engine_" & xLoop & " header'><b>Engine&nbsp;" + xLoop.ToString + "&nbsp;&nbsp;&nbsp;(Left)</b></td>")
                    'ElseIf xLoop = 4 And Not sAirframeType.Contains("R") Then
                    '  engine += ("<td valign='top' align='center' class='engine_" & xLoop & " header'><b>Engine&nbsp;" + xLoop.ToString + "&nbsp;(Right)</b></td>")
                    'Else
                    '  engine += ("<td valign='top' align='center' class='engine_" & xLoop & " header'><b>Engine&nbsp;" + xLoop.ToString + "</b></td>")
                    'End If

                    ' MSW - GOT RID OF RIGHTS AND LEFTS - n the engine tab in the column headers for the engines, remove the "(Left)" and "(Right)" entries. [12/28/2014]
                    If xLoop = 1 And Not sAirframeType.Contains("R") Then
                        engine += ("<td valign='top' align='right' class='engine_" & xLoop & " header'><b class=""upperCase"">Engine&nbsp;" + xLoop.ToString + "&nbsp;</b></td>")
                    ElseIf xLoop = 2 And Not sAirframeType.Contains("R") Then
                        engine += ("<td valign='top' align='right' class='engine_" & xLoop & " header'><b class=""upperCase"">Engine&nbsp;" + xLoop.ToString + "&nbsp;</b></td>")
                    ElseIf xLoop = 3 And Not sAirframeType.Contains("R") Then
                        engine += ("<td valign='top' align='right' class='engine_" & xLoop & " header'><b class=""upperCase"">Engine&nbsp;" + xLoop.ToString + "&nbsp;</b></td>")
                    ElseIf xLoop = 4 And Not sAirframeType.Contains("R") Then
                        engine += ("<td valign='top' align='right' class='engine_" & xLoop & " header'><b class=""upperCase"">Engine&nbsp;" + xLoop.ToString + "&nbsp;</b></td>")
                    Else
                        engine += ("<td valign='top' align='right' class='engine_" & xLoop & " header'><b class=""upperCase"">Engine&nbsp;" + xLoop.ToString + "</b></td>")
                    End If

                    sn += ("<td valign='top' align='right' width='90' class='engine_" & xLoop & "'>" + in_AircraftRs.Rows(0).Item("ac_engine_" + xLoop.ToString + "_ser_no").ToString + "</td>")

                    If Not IsDBNull(in_AircraftRs.Rows(0).Item("ac_engine_" + xLoop.ToString + "_tot_hrs")) And Not String.IsNullOrEmpty(in_AircraftRs.Rows(0).Item("ac_engine_" + xLoop.ToString + "_tot_hrs").ToString) Then
                        ttsnew += ("<td valign='top' align='right' width='90' class='engine_" & xLoop & "'>" + FormatNumber(CDbl(in_AircraftRs.Rows(0).Item("ac_engine_" + xLoop.ToString + "_tot_hrs").ToString), 0, True, False, True) + "</td>")
                        TTSN_Display_Check = True
                    Else
                        ttsnew += ("<td valign='top' align='right' width='90' class='engine_" & xLoop & "'>&nbsp;</td>")
                    End If

                    If Not IsDBNull(in_AircraftRs.Rows(0).Item("ac_engine_" + xLoop.ToString + "_soh_hrs")) And Not String.IsNullOrEmpty(in_AircraftRs.Rows(0).Item("ac_engine_" + xLoop.ToString + "_soh_hrs").ToString) Then
                        soh += ("<td valign='top' align='right' width='90' class='engine_" & xLoop & "'>" + FormatNumber(CDbl(in_AircraftRs.Rows(0).Item("ac_engine_" + xLoop.ToString + "_soh_hrs").ToString), 0, True, False, True) + "</td>")
                        SOH_Display_Check = True
                    Else
                        soh += ("<td valign='top' align='right' width='90' class='engine_" & xLoop & "'>&nbsp;</td>")
                    End If

                    If Not IsDBNull(in_AircraftRs.Rows(0).Item("ac_engine_" + xLoop.ToString + "_shi_hrs")) And Not String.IsNullOrEmpty(in_AircraftRs.Rows(0).Item("ac_engine_" + xLoop.ToString + "_shi_hrs").ToString) Then
                        shi += ("<td valign='top' align='right' width='90' class='engine_" & xLoop & "'>" + FormatNumber(CDbl(in_AircraftRs.Rows(0).Item("ac_engine_" + xLoop.ToString + "_shi_hrs").ToString), 0, True, False, True) + "</td>")
                        SHI_Display_Check = True
                    Else
                        shi += ("<td valign='top' align='right' width='90' class='engine_" & xLoop & "'>&nbsp;</td>")
                    End If

                    If Not IsDBNull(in_AircraftRs.Rows(0).Item("ac_engine_" + xLoop.ToString + "_tbo_hrs")) And Not String.IsNullOrEmpty(in_AircraftRs.Rows(0).Item("ac_engine_" + xLoop.ToString + "_tbo_hrs").ToString) Then
                        tbo += ("<td valign='top' align='right' width='90' class='engine_" & xLoop & "'>" + FormatNumber(CDbl(in_AircraftRs.Rows(0).Item("ac_engine_" + xLoop.ToString + "_tbo_hrs").ToString), 0, True, False, True) + "</td>")
                        TBO_Display_Check = True
                    Else
                        tbo += ("<td valign='top' align='right' width='90' class='engine_" & xLoop & "'>&nbsp;</td>")
                    End If

                    If Not IsDBNull(in_AircraftRs.Rows(0).Item("ac_engine_" + xLoop.ToString + "_snew_cycles")) And Not String.IsNullOrEmpty(in_AircraftRs.Rows(0).Item("ac_engine_" + xLoop.ToString + "_snew_cycles").ToString) Then
                        tcsn += ("<td valign='top' align='right' width='90' class='engine_" & xLoop & "'>" + FormatNumber(CDbl(in_AircraftRs.Rows(0).Item("ac_engine_" + xLoop.ToString + "_snew_cycles").ToString), 0, True, False, True) + "</td>")
                        TCSN_Display_Check = True
                    Else
                        tcsn += ("<td valign='middle' align='right' width='90' class='engine_" & xLoop & "'>&nbsp;</td>")
                    End If

                    If Not IsDBNull(in_AircraftRs.Rows(0).Item("ac_engine_" + xLoop.ToString + "_soh_cycles")) And Not String.IsNullOrEmpty(in_AircraftRs.Rows(0).Item("ac_engine_" + xLoop.ToString + "_soh_cycles").ToString) Then
                        tcso += ("<td valign='top' align='right' width='90' class='engine_" & xLoop & "'>" + FormatNumber(CDbl(in_AircraftRs.Rows(0).Item("ac_engine_" + xLoop.ToString + "_soh_cycles").ToString), 0, True, False, True) + "</td>")
                        TCSO_Display_Check = True
                    Else
                        tcso += ("<td valign='top' align='right' width='90' class='engine_" & xLoop & "'>&nbsp;</td>")
                    End If

                    If Not IsDBNull(in_AircraftRs.Rows(0).Item("ac_engine_" + xLoop.ToString + "_shs_cycles")) And Not String.IsNullOrEmpty(in_AircraftRs.Rows(0).Item("ac_engine_" + xLoop.ToString + "_shs_cycles").ToString) Then
                        tcsh += ("<td valign='top' align='right' width='90' class='engine_" & xLoop & "'>" + FormatNumber(CDbl(in_AircraftRs.Rows(0).Item("ac_engine_" + xLoop.ToString + "_shs_cycles").ToString), 0, True, False, True) + "</td>")
                        TCSH_Display_Check = True
                    Else
                        tcsh += ("<td valign='top' align='right' width='90' class='engine_" & xLoop & "'>&nbsp;</td>")
                    End If

                    'htmlOut.Append("</tr>")

                Next ' xLoop
                engine += "</tr>"
                htmlOut.Append(engine)
                sn += ("</tr>")
                htmlOut.Append(sn)

                If bShowBlankAcFields Or TTSN_Display_Check Then
                    ttsnew += ("</tr>")
                    htmlOut.Append(ttsnew)
                End If

                If bShowBlankAcFields Or SOH_Display_Check Then
                    soh += ("</tr>")
                    htmlOut.Append(soh)
                End If

                If bShowBlankAcFields Or SHI_Display_Check Then
                    shi += "</tr>"
                    htmlOut.Append(shi)
                End If

                If bShowBlankAcFields Or TBO_Display_Check Then
                    tbo += "</tr>"
                    htmlOut.Append(tbo)
                End If

                If bShowBlankAcFields Or TCSN_Display_Check Then
                    tcsn += "</tr>"
                    htmlOut.Append(tcsn)
                End If

                If bShowBlankAcFields Or TCSO_Display_Check Then
                    tcso += "</tr>"
                    htmlOut.Append(tcso)
                End If

                If bShowBlankAcFields Or TCSH_Display_Check Then
                    tcsh += "</tr>"
                    htmlOut.Append(tcsh)
                End If

                htmlOut.Append("</table></td></tr>")

            End If
        End If
        Return htmlOut.ToString.Trim

    End Function
    Public Shared Function DisplayPropRotorInfo_Vertical(ByRef MySesState As HttpSessionState, ByRef in_AircraftRs As DataTable, ByVal propeller_tab_container As Object, ByVal aclsData_Temp As clsData_Manager_SQL, ByRef bShowBlankAcFields As Boolean) As String
        Dim sn As String = ""
        Dim ttsn As String = ""
        Dim pr As String = ""
        Dim mypo As String = ""
        Dim prop As String = ""

        Dim ttsn_DisplayCheck As Boolean = False
        Dim pr_DisplayCheck As Boolean = False
        Dim mypo_DisplayCheck As Boolean = False
        Dim prop_DisplayCheck As Boolean = False
        Dim PlaceText As String = ""
        Dim htmlOut As StringBuilder = New StringBuilder()

        Dim sAirframeType As String = in_AircraftRs.Rows(0).Item("amod_airframe_type").ToString.ToUpper.Trim
        Dim sAircraftType As String = in_AircraftRs.Rows(0).Item("amod_make_type").ToString.ToUpper.Trim

        If in_AircraftRs.Rows(0).Item("ac_product_helicopter_flag").ToString.ToUpper.Trim = "Y" Or Trim(sAircraftType) = "T" Then
            If sAirframeType.Contains("F") Then

                If Not IsNothing(propeller_tab_container) Then
                    'propeller_tab_container.visible = True
                End If
                htmlOut.Append("<div class=""Box""><table  width='99%' cellpadding='3' align=""center"" cellspacing='0' border='0' class='formatTable blue propTable large'>")
                htmlOut.Append("<tr class=""noBorder""><td align=""left"" valign=""top""><div class=""subHeader"">PROPELLER</div></td></tr>")


                htmlOut.Append("<tr class=""noBorder"">")
                htmlOut.Append("<td align=""left"" valign=""top"">&nbsp;</td>")

                For xLoop As Integer = 1 To 4

                    If Not IsDBNull(in_AircraftRs.Rows(0).Item("ac_prop_" + xLoop.ToString + "_ser_no")) And Not String.IsNullOrEmpty(in_AircraftRs.Rows(0).Item("ac_prop_" + xLoop.ToString + "_ser_no").ToString) Then

                        htmlOut.Append("<td align=""right"" valign=""top"">Prop " & xLoop & "</td>")

                    Else
                        htmlOut.Append("<td align=""right"" valign=""top"">&nbsp;</td>")
                    End If
                Next

                htmlOut.Append("</tr>")

                'prop = "<tr><td valign='middle' align='right'></td>"


                For xLoop As Integer = 1 To 4

                    'prop += ("<td valign='middle' align='left' class='engine_" & xLoop & " header'><b>Prop<span class=""toggleSmallScreen"">eller</span>&nbsp;" + xLoop.ToString + "</b></td>")

                    If Not IsDBNull(in_AircraftRs.Rows(0).Item("ac_prop_" + xLoop.ToString + "_ser_no")) And Not String.IsNullOrEmpty(in_AircraftRs.Rows(0).Item("ac_prop_" + xLoop.ToString + "_ser_no").ToString) Then
                        ' If sn = "" Then
                        sn += "<td valign='middle' align='right' class='engine_" & xLoop & "' >"
                        'Else
                        '   sn += ", "
                        ' End If
                        sn += ("" + in_AircraftRs.Rows(0).Item("ac_prop_" + xLoop.ToString + "_ser_no").ToString)
                        'Else
                        '  sn += ("<td valign='middle' align='left' class='engine_" & xLoop & "'>&nbsp;</td>")

                        sn += "</td>"
                    End If

                    If Not IsDBNull(in_AircraftRs.Rows(0).Item("ac_prop_" + xLoop.ToString + "_snew_hrs")) And Not String.IsNullOrEmpty(in_AircraftRs.Rows(0).Item("ac_prop_" + xLoop.ToString + "_snew_hrs").ToString) Then
                        ' If ttsn = "" Then
                        ttsn += ("<td valign='middle' align='right' class='engine_" & xLoop & "'>")
                        'Else
                        '   ttsn += ", "
                        ' End If
                        ttsn += ("" + FormatNumber(CDbl(in_AircraftRs.Rows(0).Item("ac_prop_" + xLoop.ToString + "_snew_hrs").ToString), 0, True, False, True) & "")
                        ttsn_DisplayCheck = True
                        'Else
                        '  ttsn += ("<td valign='middle' align='right' class='engine_" & xLoop & "'>&nbsp;</td>")
                        ttsn += "</td>"
                    End If

                    If Not IsDBNull(in_AircraftRs.Rows(0).Item("ac_prop_" + xLoop.ToString + "_soh_hrs")) And Not String.IsNullOrEmpty(in_AircraftRs.Rows(0).Item("ac_prop_" + xLoop.ToString + "_soh_hrs").ToString) Then
                        '  If pr = "" Then
                        pr += "<td valign='middle' align='right' class='engine_" & xLoop & "'>"
                        'Else
                        '   pr += ", "
                        ' End If

                        pr += ("" + FormatNumber(CDbl(in_AircraftRs.Rows(0).Item("ac_prop_" + xLoop.ToString + "_soh_hrs").ToString), 0, True, False, True) & "")
                        pr_DisplayCheck = True
                        'Else
                        '  pr += ("<td valign='middle' align='right' class='engine_" & xLoop & "'>&nbsp;</td>")
                        pr += "</td>"
                    End If

                    If Not IsDBNull(in_AircraftRs.Rows(0).Item("ac_prop_" + xLoop.ToString + "_soh_moyear")) And Not String.IsNullOrEmpty(in_AircraftRs.Rows(0).Item("ac_prop_" + xLoop.ToString + "_soh_moyear").ToString) Then
                        '  If mypo = "" Then
                        mypo += "<td valign='middle' align='right' class='engine_" & xLoop & "'>"
                        'Else
                        '   mypo += ", "
                        '  End If

                        mypo += ("" + in_AircraftRs.Rows(0).Item("ac_prop_" + xLoop.ToString + "_soh_moyear").ToString.Substring(0, 2) + "/" + in_AircraftRs.Rows(0).Item("ac_prop_" + xLoop.ToString + "_soh_moyear").ToString.Substring(2, 4))
                        mypo_DisplayCheck = True
                        'Else
                        '  mypo += ("<td valign='middle' align='right' class='engine_" & xLoop & "'>&nbsp;</td>")
                        mypo += "</td>"
                    End If

                Next ' xLoop



                'For xLoop As Integer = 1 To 4

                '  'prop += ("<td valign='middle' align='left' class='engine_" & xLoop & " header'><b>Prop<span class=""toggleSmallScreen"">eller</span>&nbsp;" + xLoop.ToString + "</b></td>")

                '  If Not IsDBNull(in_AircraftRs.Rows(0).Item("ac_prop_" + xLoop.ToString + "_ser_no")) And Not String.IsNullOrEmpty(in_AircraftRs.Rows(0).Item("ac_prop_" + xLoop.ToString + "_ser_no").ToString) Then
                '    If sn = "" Then
                '      sn += "<td valign='middle' align='left' class='engine_" & xLoop & "' >"
                '    Else
                '      sn += ", "
                '    End If
                '    sn += ("Prop " + xLoop.ToString + ": " + in_AircraftRs.Rows(0).Item("ac_prop_" + xLoop.ToString + "_ser_no").ToString)
                '    'Else
                '    '  sn += ("<td valign='middle' align='left' class='engine_" & xLoop & "'>&nbsp;</td>")
                '  End If

                '  If Not IsDBNull(in_AircraftRs.Rows(0).Item("ac_prop_" + xLoop.ToString + "_snew_hrs")) And Not String.IsNullOrEmpty(in_AircraftRs.Rows(0).Item("ac_prop_" + xLoop.ToString + "_snew_hrs").ToString) Then
                '    If ttsn = "" Then
                '      ttsn += ("<td valign='middle' align='left' class='engine_" & xLoop & "'>")
                '    Else
                '      ttsn += ", "
                '    End If
                '    ttsn += ("Prop " + xLoop.ToString + ": " + FormatNumber(CDbl(in_AircraftRs.Rows(0).Item("ac_prop_" + xLoop.ToString + "_snew_hrs").ToString), 0, True, False, True) & " hrs")
                '    ttsn_DisplayCheck = True
                '    'Else
                '    '  ttsn += ("<td valign='middle' align='right' class='engine_" & xLoop & "'>&nbsp;</td>")
                '  End If

                '  If Not IsDBNull(in_AircraftRs.Rows(0).Item("ac_prop_" + xLoop.ToString + "_soh_hrs")) And Not String.IsNullOrEmpty(in_AircraftRs.Rows(0).Item("ac_prop_" + xLoop.ToString + "_soh_hrs").ToString) Then
                '    If pr = "" Then
                '      pr += "<td valign='middle' align='left' class='engine_" & xLoop & "'>"
                '    Else
                '      pr += ", "
                '    End If

                '    pr += ("Prop " + xLoop.ToString + ": " + FormatNumber(CDbl(in_AircraftRs.Rows(0).Item("ac_prop_" + xLoop.ToString + "_soh_hrs").ToString), 0, True, False, True) & " hrs")
                '    pr_DisplayCheck = True
                '    'Else
                '    '  pr += ("<td valign='middle' align='right' class='engine_" & xLoop & "'>&nbsp;</td>")
                '  End If

                '  If Not IsDBNull(in_AircraftRs.Rows(0).Item("ac_prop_" + xLoop.ToString + "_soh_moyear")) And Not String.IsNullOrEmpty(in_AircraftRs.Rows(0).Item("ac_prop_" + xLoop.ToString + "_soh_moyear").ToString) Then
                '    If mypo = "" Then
                '      mypo += "<td valign='middle' align='left' class='engine_" & xLoop & "'>"
                '    Else
                '      mypo += ", "
                '    End If

                '    mypo += ("Prop " + xLoop.ToString + ": " + in_AircraftRs.Rows(0).Item("ac_prop_" + xLoop.ToString + "_soh_moyear").ToString.Substring(0, 2) + "/" + in_AircraftRs.Rows(0).Item("ac_prop_" + xLoop.ToString + "_soh_moyear").ToString.Substring(2, 4))
                '    mypo_DisplayCheck = True
                '    'Else
                '    '  mypo += ("<td valign='middle' align='right' class='engine_" & xLoop & "'>&nbsp;</td>")
                '  End If

                'Next ' xLoop




                Dim noDetails As Boolean = True
                If sn <> "" Then
                    sn = ("<tr><td align='left' valign='top' width=""410"">Ser No:</td>") + sn
                    sn += "</td></tr>"
                    htmlOut.Append(sn)
                    noDetails = False
                End If
                If ttsn <> "" Then
                    ttsn = ("<tr><td align='left' valign='top' width=""410"">Time Since New Hrs</td>") + ttsn
                    If ttsn_DisplayCheck Or bShowBlankAcFields = True Then
                        ttsn += "</td></tr>"
                        htmlOut.Append(ttsn)
                        noDetails = False
                    End If
                End If
                If pr <> "" Then
                    pr = ("<tr><td align='left' valign='top' width=""410"">Time Since Overhaul Hrs</td>") + pr
                    If pr_DisplayCheck Or bShowBlankAcFields = True Then
                        pr += "</td></tr>"
                        htmlOut.Append(pr)
                        noDetails = False
                    End If

                End If
                If mypo <> "" Then
                    mypo = ("<tr><td align='left' valign='top' width=""410"">Month/Year of Overhaul</td>") + mypo
                    If mypo_DisplayCheck Or bShowBlankAcFields = True Then
                        mypo += "</td></tr>"
                        htmlOut.Append(mypo)
                        noDetails = False
                    End If
                End If

                ' prop += "</tr>"
                'htmlOut.Append(prop)


                If noDetails = True Then
                    htmlOut = New StringBuilder
                    htmlOut.Append("<div class=""Box""><table  width='98%' cellpadding='0' align=""center"" cellspacing='0' border='0' class='formatTable large blue subtextNoMargin propTable'>")
                    htmlOut.Append("<tr class=""noBorder""><td align=""left"" valign=""top""><div class=""subHeader"">PROPELLER</div></td></tr>")
                    htmlOut.Append("<tr><td align='left' valign='top'>No Details Reported.</td>")
                End If





                htmlOut.Append("</table></div>")

            ElseIf sAirframeType.Contains("R") Then
                If Not IsNothing(propeller_tab_container) Then
                    propeller_tab_container.visible = True
                End If

                Dim lData As New DataTable

                Dim sQuery As String = ""
                Dim bHasMainBlades As Boolean = False
                Dim bHasTailBlades As Boolean = False
                Dim sLabel As String = ""
                Dim xLoop As Integer = 1

                Dim ttsn_rDisplayCheck As Boolean = False
                Dim time_rDisplayCheck As Boolean = False
                Dim tsoh_rDisplayCheck As Boolean = False

                ' new query to grab Helicopter Gearbox/Rotorblade Info
                lData = aclsData_Temp.GetJETNET_GearboxRotorblade(in_AircraftRs.Rows(0).Item("ac_id"), in_AircraftRs.Rows(0).Item("ac_journ_id").ToString)

                If Not IsNothing(lData) Then
                    If lData.Rows.Count > 0 Then
                        htmlOut.Append("<div class=""Box""><table  width='98%' align='center' cellpadding='3' cellspacing='0' border='0' class='formatTable blue propTable large'>")
                        htmlOut.Append("<tr><td colspan=""5"" align=""left""><div class=""subHeader"">Gearbox/Rotorblade</div></td></tr>")
                        htmlOut.Append("<tr><td>&nbsp;</td>")
                        htmlOut.Append("<td align='left' valign='top' class='header'>Ser No</td>")
                        htmlOut.Append("<td align='left' valign='top' class='header ttsn_rDisplayCheck'>Time Since New</span></td>")
                        htmlOut.Append("<td align='left' valign='top' class='header  time_rDisplayCheck'>Time Remaining - Hrs</td>")
                        htmlOut.Append("<td align='left' valign='top' class='header  tsoh_rDisplayCheck'>Time Since Overhaul</td></tr>")


                        For Each r As DataRow In lData.Rows

                            bHasMainBlades = False
                            bHasTailBlades = False

                            If Not IsDBNull(r("heldt_category_type")) And Not String.IsNullOrEmpty(r("heldt_category_type").ToString) Then

                                Select Case r("heldt_category_type").ToString.ToUpper.Trim

                                    Case "INTERMEDIATE GEARBOX"
                                        bHasMainBlades = False
                                        bHasTailBlades = False
                                        sLabel = "Intermediate&nbsp;Gearbox"
                                        xLoop = 1
                                    Case "MAIN ROTOR #1 BLADES", "MAIN ROTOR #2 BLADES"
                                        ' Check for number of blades
                                        sLabel = "Main&nbsp;Blade&nbsp;"
                                        bHasMainBlades = True
                                        bHasTailBlades = False
                                        ' OK Get the Blade Number 1-10
                                        sLabel = sLabel & Right(r("heldt_subcat_type").ToString, 2)
                                        xLoop = 2
                                    Case "MAIN ROTOR HUB #1", "MAIN ROTOR HUB #2"
                                        bHasMainBlades = False
                                        bHasTailBlades = False
                                        sLabel = "Main&nbsp;Rotor&nbsp;Hub"
                                        xLoop = 3
                                    Case "MAIN TRANSMISSION #1", "MAIN TRANSMISSION #2"
                                        bHasMainBlades = False
                                        bHasTailBlades = False
                                        sLabel = "Main&nbsp;Transmission "
                                        xLoop = 6
                                    Case "TAIL ROTOR BLADES"
                                        ' Check for number of blades
                                        sLabel = "Tail&nbsp;Blade&nbsp;"
                                        bHasMainBlades = False
                                        bHasTailBlades = True
                                        ' OK Get the Blade Number 1-10
                                        sLabel = sLabel & Right(r("heldt_subcat_type").ToString, 2)
                                        xLoop = 4
                                    Case "TAIL ROTOR GEARBOX"
                                        bHasMainBlades = False
                                        bHasTailBlades = False
                                        sLabel = "Tail&nbsp;Rotor&nbsp;Gearbox"
                                        xLoop = 1
                                    Case "TAIL ROTOR HUB"
                                        bHasMainBlades = False
                                        bHasTailBlades = False
                                        sLabel = "Tail&nbsp;Rotor&nbsp;Hub"
                                        xLoop = 3
                                End Select

                            End If

                            htmlOut.Append("<tr><td valign='middle' align='header' class='engine_" & xLoop & "' >" + sLabel + "</td>")

                            If Not IsDBNull(r("heldt_ser_no_full")) And Not String.IsNullOrEmpty(r("heldt_ser_no_full").ToString) Then
                                htmlOut.Append("<td valign='middle' align='left' class='engine_" & xLoop & "' >" + r("heldt_ser_no_full").ToString.Trim + "</td>")
                            Else
                                htmlOut.Append("<td valign='middle' align='left' class='engine_" & xLoop & "' >&nbsp;</td>")
                            End If

                            If Not IsDBNull(r("heldt_ttsn")) And Not String.IsNullOrEmpty(r("heldt_ttsn").ToString) Then
                                ttsn_rDisplayCheck = True
                                htmlOut.Append("<td valign='middle' align='left' class='engine_" & xLoop & " ttsn_rDisplayCheck' >" + FormatNumber(CDbl(r("heldt_ttsn").ToString), 0, True, False, True) + "</td>")
                            Else
                                htmlOut.Append("<td valign='middle' align='left' class='engine_" & xLoop & " ttsn_rDisplayCheck' >&nbsp;</td>")
                            End If

                            If Not IsDBNull(r("heldt_remaining_hours")) And Not String.IsNullOrEmpty(r("heldt_remaining_hours").ToString) Then
                                time_rDisplayCheck = True
                                htmlOut.Append("<td valign='middle' align='left' class='engine_" & xLoop & " time_rDisplayCheck' >" + FormatNumber(CDbl(r("heldt_remaining_hours").ToString), 0, True, False, True) + "</td>")
                            Else
                                htmlOut.Append("<td valign='middle' align='left' class='engine_" & xLoop & " time_rDisplayCheck' >&nbsp;</td>")
                            End If

                            If Not IsDBNull(r("heldt_soh")) And Not String.IsNullOrEmpty(r("heldt_soh").ToString) And (Not bHasMainBlades Or Not bHasTailBlades) Then
                                htmlOut.Append("<td valign='middle' align='left' class='engine_" & xLoop & " tsoh_rDisplayCheck' >" + FormatNumber(CDbl(r("heldt_soh").ToString), 0, True, False, True) + "</td>")
                                tsoh_rDisplayCheck = True
                            Else
                                If bHasMainBlades Or bHasTailBlades Then
                                    htmlOut.Append("<td valign='middle' align='left' class='engine_" & xLoop & " tsoh_rDisplayCheck' >N/A</td>")
                                Else
                                    htmlOut.Append("<td valign='middle' align='left' class='engine_" & xLoop & " tsoh_rDisplayCheck' >&nbsp;</td>")
                                End If
                            End If

                            htmlOut.Append("</tr>")

                        Next

                        PlaceText = htmlOut.ToString

                        PlaceText = Replace(PlaceText, "ttsn_rDisplayCheck", IIf(ttsn_rDisplayCheck Or bShowBlankAcFields, "", "display_none"))

                        PlaceText = Replace(PlaceText, "time_rDisplayCheck", IIf(time_rDisplayCheck Or bShowBlankAcFields, "", "display_none"))
                        PlaceText = Replace(PlaceText, "tsoh_rDisplayCheck", IIf(tsoh_rDisplayCheck Or bShowBlankAcFields, "", "display_none"))

                        PlaceText += "</table></div>"

                        htmlOut = New StringBuilder
                        htmlOut.Append(PlaceText)

                        lData = Nothing
                    Else
                        htmlOut.Append("<div class=""Box""><table width='98%' align='center' cellpadding='3' cellspacing='0' border='0' class='formatTable large blue subtextNoMargin propTable'>")
                        htmlOut.Append("<tr class=""noBorder""><td align=""left"" valign=""top""><div class=""subHeader"">Gearbox/Rotorblade</div></td></tr><tr><td align=""left"" valign=""top"">No Details Reported</td></tr></table></div>")
                    End If
                End If


            End If
        End If
        Return htmlOut.ToString.Trim

    End Function
    ''' <summary>
    ''' Displays a string of the aircraft interior details.
    ''' </summary>
    ''' <param name="aclsData_Temp">the data layer class being passed.</param>
    ''' <param name="MySesState">session state reference.</param>
    ''' <param name="in_AircraftRs">aircraft table filled with data information.</param>
    ''' <param name="interior_tab">interior tab to set the tab header text.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function DisplayInteriorDetails(ByVal aclsData_Temp As clsData_Manager_SQL, ByVal MySesState As HttpSessionState,
                                              ByRef in_AircraftRs As DataTable,
                                              ByRef interior_tab As Object, ByRef crmSource As String) As String

        Dim htmlOut As StringBuilder = New StringBuilder()
        Dim sQuery As String = ""
        Dim ReturnTable As New DataTable
        Dim rowCounter As Integer = 0
        Dim on_count As Integer = 0 'needed to make sure the # of rows is even
        Dim no_details_count As Integer = 0 'running tally to see if, "no interior details" gets displayed 
        Dim month_hold As String = ""


        'setting the interior tab container header text to save some lines.
        If Not IsNothing(interior_tab) Then
            interior_tab.headerText = ""

            If Not IsDBNull(in_AircraftRs.Rows(0).Item("ac_interior_doneby_name")) And Not String.IsNullOrEmpty(in_AircraftRs.Rows(0).Item("ac_interior_doneby_name").ToString) Then
                interior_tab.headerText = " DONE BY " + in_AircraftRs.Rows(0).Item("ac_interior_doneby_name").ToString.Trim
            Else
                'This means unknown, only display if the moyear is there
                If Not IsDBNull(in_AircraftRs.Rows(0).Item("ac_interior_month_year")) And Not String.IsNullOrEmpty(in_AircraftRs.Rows(0).Item("ac_interior_month_year").ToString) Then
                    interior_tab.headerText = " DONE " 'BY UNKNOWN"
                End If
            End If
        End If

        If Not IsDBNull(in_AircraftRs.Rows(0).Item("ac_interior_month_year")) And Not String.IsNullOrEmpty(in_AircraftRs.Rows(0).Item("ac_interior_month_year").ToString) Then
            If Not IsNothing(interior_tab) Then
                If Len(in_AircraftRs.Rows(0).Item("ac_interior_month_year")) = 6 Then
                    month_hold = in_AircraftRs.Rows(0).Item("ac_interior_month_year").ToString.Substring(0, 2)
                    If Trim(month_hold) <> "" Then
                        month_hold = month_hold & "/"
                    End If
                    interior_tab.headerText += " ON " + month_hold + in_AircraftRs.Rows(0).Item("ac_interior_month_year").ToString.Substring(2, 4)
                Else
                    interior_tab.headerText += " ON " + in_AircraftRs.Rows(0).Item("ac_interior_month_year").ToString
                End If
            End If
        End If

        If interior_tab.headerText = "" Then
            interior_tab.headertext = "INTERIOR"
        Else
            interior_tab.headertext = "INTERIOR: " & interior_tab.headertext
        End If
        htmlOut.Append("<table cellpadding='0' cellspacing='0' width='100%'>")
        htmlOut.Append("<tr>")

        If crmSource = "CLIENT" Then
            ReturnTable = aclsData_Temp.Get_Client_Aircraft_Details_As_Jetnet_Fields(in_AircraftRs.Rows(0).Item("CLIENT_ID"), "interior")
        Else
            ReturnTable = aclsData_Temp.GetJETNET_Aircraft_Details_Equipment_AC_ID_TYPE(in_AircraftRs.Rows(0).Item("ac_id"), "interior", in_AircraftRs.Rows(0).Item("ac_journ_id"))
        End If

        If ReturnTable.Rows.Count > 0 Then

            For Each r As DataRow In ReturnTable.Rows
                ' If r("adet_journal_id") = in_AircraftRs.Rows(0).Item("journal_id") Then
                ' htmlOut.Append("<td valign='top' align='left' width='5'>&nbsp;&nbsp;</td>")
                htmlOut.Append("<td valign='top' align='left'>")

                If Not IsDBNull(r("adet_data_name")) And Not String.IsNullOrEmpty(r("adet_data_name").ToString) Then
                    htmlOut.Append("<span class='li'><span class='label'>" + r("adet_data_name").ToString.Trim + ":</span> ")

                    If Not IsDBNull(r("adet_data_description")) And Not String.IsNullOrEmpty(r("adet_data_description").ToString) Then
                        htmlOut.Append(r("adet_data_description").ToString.Trim)
                    End If
                    htmlOut.Append("</span>")
                End If

                htmlOut.Append("</td>")
                htmlOut.Append("</tr><tr>")
                no_details_count += 1
                rowCounter += 1
                ' End If
            Next

        End If
        ReturnTable = New DataTable

        htmlOut.Append("</tr>")
        htmlOut.Append("<tr>")

        If Not IsDBNull(in_AircraftRs.Rows(0).Item("ac_interior_rating")) And Not String.IsNullOrEmpty(in_AircraftRs.Rows(0).Item("ac_interior_rating").ToString) Then
            If (in_AircraftRs.Rows(0).Item("ac_interior_rating").ToString <> "0") Then
                ' htmlOut.Append("<td valign='top' align='left' width='5'>&nbsp;&nbsp;</td>")
                htmlOut.Append("<td align='left' valign='top'>")
                htmlOut.Append("<span class='li'><span class='label'>Rating:</span> " + in_AircraftRs.Rows(0).Item("ac_interior_rating").ToString.Trim + "</span>")
                htmlOut.Append("</td>")
                on_count = 1
                no_details_count += 1
            End If
        End If
        If on_count = 1 Then
            htmlOut.Append("</tr><tr>")
            on_count = 0
        End If
        If Not IsDBNull(in_AircraftRs.Rows(0).Item("ac_passenger_count")) And Not String.IsNullOrEmpty(in_AircraftRs.Rows(0).Item("ac_passenger_count").ToString) Then
            If (in_AircraftRs.Rows(0).Item("ac_passenger_count").ToString <> "0") Then
                'htmlOut.Append("<td valign='top' align='left' width='5'>&nbsp;&nbsp;</td>")
                htmlOut.Append("<td align='left' valign='top'>")
                htmlOut.Append("<span class='li'><span class='label'>Passengers:</span> " + in_AircraftRs.Rows(0).Item("ac_passenger_count").ToString.Trim + "</span>")
                htmlOut.Append("</td>")
                on_count += 1
                no_details_count += 1
            End If
        End If
        'Does row need to be closed?
        If on_count = 1 Then
            htmlOut.Append("</tr><tr>")
            on_count = 0
        End If
        If Not IsDBNull(in_AircraftRs.Rows(0).Item("ac_interior_config_name")) And Not String.IsNullOrEmpty(in_AircraftRs.Rows(0).Item("ac_interior_config_name").ToString) Then
            'htmlOut.Append("<td valign='top' align='left' width='5'>&nbsp;&nbsp;</td>")
            htmlOut.Append("<td align='left' valign='top'>")
            htmlOut.Append("<span class='li'><span class='label'>Configuration:</span> " + in_AircraftRs.Rows(0).Item("ac_interior_config_name").ToString.Trim + "</span>")
            htmlOut.Append("</td>")
            on_count += 1
            no_details_count += 1
        End If

        If on_count = 1 Then
            htmlOut.Append("</tr>")
            on_count = 0
        Else
            'htmlOut.Append("<td valign='top' align='left' width='5'>&nbsp;&nbsp;</td>")
            htmlOut.Append("<td align='left' valign='top'></td></tr>")
        End If
        If no_details_count = 0 Then
            htmlOut = New StringBuilder()
            htmlOut.Append("<table cellpadding='0' cellspacing='0' width='100%'>")
            'htmlOut.Append("<tr><td valign='top' align='left' width='5'>&nbsp;&nbsp;</td>")
            htmlOut.Append("<td valign='top' align='left'>")
            htmlOut.Append("<span class='li'>No Interior Details</span> ")
            htmlOut.Append("</td></tr>")

        End If
        htmlOut.Append("</table>")


        Return htmlOut.ToString.Trim

    End Function
    ''' <summary>
    ''' Displays a string of the aircraft exterior details.
    ''' </summary>
    ''' <param name="aclsData_Temp">the data layer class is being passed.</param>
    ''' <param name="MySesState">session state reference</param>
    ''' <param name="in_AircraftRs">aircraft table filled with data information.</param>
    ''' <param name="exterior_tab">exterior tab to set the tab header text.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function DisplayExteriorDetails(ByVal aclsData_Temp As clsData_Manager_SQL, ByRef MySesState As HttpSessionState,
                                                  ByRef in_AircraftRs As DataTable,
                                                  ByRef exterior_tab As Object, ByRef crmSource As String) As String

        Dim htmlOut As StringBuilder = New StringBuilder()
        Dim sQuery As String = ""
        Dim ReturnTable As New DataTable
        Dim rowCounter As Integer = 0
        Dim month_hold As String = ""

        If Not IsNothing(exterior_tab) Then
            exterior_tab.headerText = ""
            If Not IsDBNull(in_AircraftRs.Rows(0).Item("ac_exterior_doneby_name")) And Not String.IsNullOrEmpty(in_AircraftRs.Rows(0).Item("ac_exterior_doneby_name").ToString) Then
                exterior_tab.headerText = " DONE BY " + in_AircraftRs.Rows(0).Item("ac_exterior_doneby_name").ToString.Trim & " "
            Else 'only display if your moyear is there, otherwise doesn't make sense
                If Not IsDBNull(in_AircraftRs.Rows(0).Item("ac_exterior_month_year")) And Not String.IsNullOrEmpty(in_AircraftRs.Rows(0).Item("ac_exterior_month_year").ToString) Then
                    exterior_tab.headerText = " DONE " 'BY UNKNOWN "
                End If
            End If
        End If

        If Not IsDBNull(in_AircraftRs.Rows(0).Item("ac_exterior_month_year")) And Not String.IsNullOrEmpty(in_AircraftRs.Rows(0).Item("ac_exterior_month_year").ToString) Then
            If Not IsNothing(exterior_tab) Then
                If Len(in_AircraftRs.Rows(0).Item("ac_exterior_month_year")) = 6 Then
                    month_hold = in_AircraftRs.Rows(0).Item("ac_exterior_month_year").ToString.Substring(0, 2)
                    If Trim(month_hold) <> "" Then
                        month_hold = month_hold & "/"
                    End If
                    exterior_tab.headerText += "ON " & month_hold + in_AircraftRs.Rows(0).Item("ac_exterior_month_year").ToString.Substring(2, 4)
                Else
                    exterior_tab.headerText += "ON " & in_AircraftRs.Rows(0).Item("ac_exterior_month_year").ToString
                End If
            End If
        End If


        If exterior_tab.headertext = "" Then
            exterior_tab.headertext = "EXTERIOR"
        Else
            exterior_tab.headertext = "EXTERIOR: " & exterior_tab.headerText
        End If
        htmlOut.Append("<table cellpadding='0' cellspacing='0' width='100%'>")

        If crmSource = "CLIENT" Then
            ReturnTable = aclsData_Temp.Get_Client_Aircraft_Details_As_Jetnet_Fields(in_AircraftRs.Rows(0).Item("CLIENT_ID"), "exterior")
        Else
            ReturnTable = aclsData_Temp.GetJETNET_Aircraft_Details_Equipment_AC_ID_TYPE(in_AircraftRs.Rows(0).Item("ac_id"), "exterior", in_AircraftRs.Rows(0).Item("ac_journ_id"))
        End If

        htmlOut.Append("<tr>")

        If ReturnTable.Rows.Count > 0 Then

            For Each r As DataRow In ReturnTable.Rows

                'htmlOut.Append("<td valign='top' align='left' width='5'>&nbsp;&nbsp;</td>")
                htmlOut.Append("<td valign='top' align='left'>")

                If Not IsDBNull(r("adet_data_name")) And Not String.IsNullOrEmpty(r("adet_data_name").ToString) Then
                    htmlOut.Append("<span class='li'><span class='label'>" + r("adet_data_name").ToString.Trim + ":</span> ")

                    If Not IsDBNull(r("adet_data_description")) And Not String.IsNullOrEmpty(r("adet_data_description").ToString) Then
                        htmlOut.Append("" + r("adet_data_description").ToString.Trim)
                    End If
                    htmlOut.Append("</span>")
                End If

                htmlOut.Append("</td>")
                htmlOut.Append("</tr><tr>")
                rowCounter += 1
            Next

        End If

        If Not IsDBNull(in_AircraftRs.Rows(0).Item("ac_exterior_rating")) And Not String.IsNullOrEmpty(in_AircraftRs.Rows(0).Item("ac_exterior_rating").ToString) Then
            If (in_AircraftRs.Rows(0).Item("ac_exterior_rating").ToString <> "0") Then
                'htmlOut.Append("<tr><td valign='top' align='left' width='5'>&nbsp;&nbsp;</td>")
                htmlOut.Append("<td align='left' valign='top'><span class='li'><span class='label'>Rating:</span> " + in_AircraftRs.Rows(0).Item("ac_exterior_rating").ToString.Trim + "</span></td></tr>")
                rowCounter += 1
            End If
        End If

        'no rows
        If rowCounter = 0 Then
            'htmlOut.Append("<td valign='top' align='left' width='5'>&nbsp;&nbsp;</td>")
            htmlOut.Append("<td valign='top' align='left'>")
            htmlOut.Append("<span class='li'>No Exterior Details</span>")
            htmlOut.Append("</td>")
        End If
        htmlOut.Append("</table>")


        ReturnTable = New DataTable

        Return htmlOut.ToString.Trim

    End Function

    ''' <summary>
    ''' Displays the avionic details of the aircraft
    ''' NEEDS TO BE EDITED TO TAKE INTO CONSIDERATION JOURNAL ID WHEN DL IS CHECKED BACK IN
    ''' </summary>
    ''' <param name="aclsData_Temp">data layer object</param>
    ''' <param name="MySesState">session state</param>
    ''' <param name="in_AircraftRs">aircraft data table prefilled with information</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function DisplayAvionicsDetails(ByVal aclsData_Temp As clsData_Manager_SQL, ByVal MySesState As HttpSessionState,
                                               ByRef in_AircraftRs As DataTable, ByRef crmSource As String) As String

        Dim htmlOut As StringBuilder = New StringBuilder()
        Dim sQuery As String = ""
        Dim Results_Table As New DataTable
        Dim rowCounter As Integer = 0

        If crmSource = "CLIENT" Then
            Results_Table = aclsData_Temp.Get_Client_Aircraft_Avionics_As_Jetnet_Fields(in_AircraftRs.Rows(0).Item("CLIENT_ID"))
        Else
            Results_Table = aclsData_Temp.GetJETNET_Aircraft_Avionics_AC_ID(in_AircraftRs.Rows(0).Item("ac_id"), in_AircraftRs.Rows(0).Item("ac_journ_id"))
        End If

        htmlOut.Append("<table cellpadding='0' cellspacing='0' width='100%'>")
        htmlOut.Append("<tr>")

        If Results_Table.Rows.Count > 0 Then

            For Each r As DataRow In Results_Table.Rows

                If rowCounter = 2 Then
                    htmlOut.Append("</tr><tr>")
                    rowCounter = 0
                End If
                'htmlOut.Append("<td valign='top' align='left' width='5'>&nbsp;&nbsp;</td>")

                'this is important because if it's not here, the avionics tab won't grow properly if the row count is 1
                If Results_Table.Rows.Count = 1 Then
                    htmlOut.Append("<td valign='top' align='left'>")
                Else
                    htmlOut.Append("<td valign='top' align='left' width='50%'>")
                End If
                If Not IsDBNull(r("av_name")) And Not String.IsNullOrEmpty(r("av_name").ToString) Then
                    htmlOut.Append("<span class='li'><span class='label'>" + r("av_name").ToString.Trim & ":</span> ")

                    If Not IsDBNull(r("av_description")) And Not String.IsNullOrEmpty(r("av_description").ToString) Then
                        htmlOut.Append("" + r("av_description").ToString.Trim + "")
                    End If
                    htmlOut.Append("</span>")
                End If
                htmlOut.Append("</td>")

                rowCounter += 1
            Next

        Else
            htmlOut.Append("<td valign='top' align='left'>")
            htmlOut.Append("<span class='li'>No Avionics Details</span>")
            htmlOut.Append("</td>")
        End If

        htmlOut.Append("</tr></table>")
        Results_Table = Nothing

        Return htmlOut.ToString.Trim


    End Function

    ''' <summary>
    ''' Displays Cockpit information based on Aircraft Table sent.
    ''' NEEDS TO BE EDITED TO TAKE INTO CONSIDERATION JOURNAL ID WHEN DL IS CHECKED BACK IN
    ''' </summary>
    ''' <param name="aclsData_Temp">data layer object</param>
    ''' <param name="MySesState">session state</param>
    ''' <param name="in_AircraftRs">aircraft data table prefilled with information</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function DisplayCockpitDetails(ByVal aclsData_Temp As clsData_Manager_SQL, ByRef MySesState As HttpSessionState,
                                                 ByRef in_AircraftRs As DataTable, ByRef crmSource As String) As String

        Dim htmlOut As StringBuilder = New StringBuilder()
        Dim sQuery As String = ""
        Dim rowCounter As Integer = 0
        Dim ReturnTable As New DataTable


        htmlOut.Append("<table cellpadding='0' cellspacing='0' width='100%'>")
        If crmSource = "CLIENT" Then
            ReturnTable = aclsData_Temp.Get_Client_Aircraft_Details_As_Jetnet_Fields(in_AircraftRs.Rows(0).Item("CLIENT_ID"), "addl cockpit equipment")
        Else
            ReturnTable = aclsData_Temp.GetJETNET_Aircraft_Details_Equipment_AC_ID_TYPE(in_AircraftRs.Rows(0).Item("ac_id"), "addl cockpit equipment", in_AircraftRs.Rows(0).Item("ac_journ_id"))
        End If

        If ReturnTable.Rows.Count > 0 Then

            For Each r As DataRow In ReturnTable.Rows
                htmlOut.Append("<tr>")
                rowCounter = 0
                'htmlOut.Append("<td valign='top' align='left' width='5'>&nbsp;&nbsp;</td>")


                htmlOut.Append("<td valign='top' align='left'>")
                If Not IsDBNull(r("adet_data_name")) And Not String.IsNullOrEmpty(r("adet_data_name").ToString) Then
                    htmlOut.Append("<span class='li'><span class='label'>" + r("adet_data_name").ToString.Trim & ":</span> ")

                    If Not IsDBNull(r("adet_data_description")) And Not String.IsNullOrEmpty(r("adet_data_description").ToString) Then
                        htmlOut.Append(r("adet_data_description").ToString.Trim)
                    End If
                    htmlOut.Append("</span>")
                End If
                htmlOut.Append("</td></tr>")

                rowCounter += 1
            Next

        Else
            htmlOut.Append("<td valign='top' align='left'>")
            htmlOut.Append("<span class='li'>No Cockpit Details</span>")
            htmlOut.Append("</td>")
        End If

        htmlOut.Append("</tr></table>")


        Return htmlOut.ToString.Trim

    End Function

    '''' <summary>
    '''' NEEDS TO BE EDITED TO ADD JOURNAL ID + KFEAT_INACTIVE_DATE IS NULL?
    '''' Display AC Key Features
    '''' </summary>
    '''' <param name="MySesState"></param>
    '''' <param name="in_AircraftRs"></param>
    '''' <returns></returns>
    '''' <remarks></remarks>
    'Public Shared Function DisplayKeyFeatures(ByVal aclsData_Temp As clsData_Manager_SQL, ByRef MySesState As HttpSessionState, _
    '                                          ByRef in_AircraftRs As DataTable, ByRef crmSource As String, ByRef DamageCode As String) As String

    '  Dim ModelFeatures As New DataTable
    '  Dim htmlOut As StringBuilder = New StringBuilder()
    '  Dim sQuery As String = ""
    '  Dim rowCounter As Integer = 0
    '  Dim Results_Table As New DataTable
    '  Dim FilteredTable As New DataTable
    '  Dim StandardFeature As Boolean = False
    '  htmlOut.Append("<table cellpadding='0' cellspacing='0' width='100%'>")


    '  ModelFeatures = aclsData_Temp.GetJETNET_Model_Standard_Key_Features(in_AircraftRs.Rows(0).Item("ac_amod_id"))

    '  If crmSource = "CLIENT" Then
    '    Results_Table = clsGeneral.clsGeneral.Get_Client_Aircraft_Key_Features_As_Jetnet_Fields(in_AircraftRs.Rows(0).Item("CLIENT_ID"))
    '  Else
    '    Results_Table = aclsData_Temp.GetJETNET_Aircraft_Details_Key_Features_AC_ID(in_AircraftRs.Rows(0).Item("ac_id"), in_AircraftRs.Rows(0).Item("ac_journ_id"))
    '  End If

    '  If Results_Table.Rows.Count > 0 Then

    '    For Each r As DataRow In Results_Table.Rows
    '      If r("kfeat_code") <> "DAM" Then

    '        'First Thing is first, we need to run a test to see if this is a blue standard feature.
    '        If ModelFeatures.Rows.Count > 0 Then
    '          StandardFeature = False
    '          FilteredTable = New DataTable
    '          FilteredTable = ModelFeatures.Clone
    '          Dim afiltered_Client As DataRow() = ModelFeatures.Select("amfeat_feature_code = '" & r("kfeat_code") & "'", "")
    '          ' extract and import
    '          For Each atmpDataRow_Client In afiltered_Client
    '            FilteredTable.ImportRow(atmpDataRow_Client)
    '          Next
    '          If FilteredTable.Rows.Count > 0 Then
    '            StandardFeature = True
    '          End If
    '        End If




    '        If Not IsDBNull(r("kfeat_type")) And Not String.IsNullOrEmpty(r("kfeat_type").ToString) Then
    '          If rowCounter = 2 Then
    '            htmlOut.Append("</tr><tr>")
    '            rowCounter = 0
    '          End If
    '          'htmlOut.Append("<td align='left' valign='top' width='5'>&nbsp;&nbsp;</td>")

    '          'this is important because if it's not here, the cockpit tab won't grow properly if the row count is 1
    '          If Results_Table.Rows.Count = 1 Then
    '            htmlOut.Append("<td valign='top' align='left'" & IIf(StandardFeature = True, " class='blue_text' ", "") & ">")
    '          Else
    '            htmlOut.Append("<td valign='top' align='left' width='50%'" & IIf(StandardFeature = True, " class='blue_text' ", "") & ">")
    '          End If

    '          If Not IsDBNull(r("kff_name")) And Not String.IsNullOrEmpty(r("kff_name").ToString) Then
    '            Select Case (r("kff_name").ToString.ToUpper)
    '              Case "U"
    '                htmlOut.Append("<span class='li'>")
    '              Case "Y"
    '                htmlOut.Append("<span class='li'>")
    '              Case "N"
    '                htmlOut.Append("<span class='li'>")
    '              Case "A"
    '                htmlOut.Append("<span class='li'>")
    '              Case "I"
    '                htmlOut.Append("<span class='li'>")
    '              Case Else
    '                htmlOut.Append("<span class='black'>")
    '            End Select
    '          Else
    '            htmlOut.Append("<span class='black'>")
    '          End If

    '          If Not IsDBNull(r("kfeat_name")) And Not String.IsNullOrEmpty(r("kfeat_name").ToString) Then
    '            htmlOut.Append(r("kfeat_name").ToString.Trim)
    '          End If


    '          'htmlOut.Append(" <em>(")
    '          ''If StandardFeature = True Then
    '          ''    htmlOut.Append("<span class='blue_text'>")
    '          ''Else
    '          ''    htmlOut.Append("<span>")
    '          ''End If
    '          'htmlOut.Append(r("kfeat_type").ToString.Trim)
    '          'htmlOut.Append("</em>)")

    '        Else
    '          htmlOut.Append("<td valign='top' align='left'>&nbsp;")
    '        End If



    '        If Not IsDBNull(r("kff_name")) And Not String.IsNullOrEmpty(r("kff_name").ToString) Then
    '          Select Case (r("kff_name").ToString.ToUpper)
    '            Case "U"
    '              htmlOut.Append(":&nbsp;<em>UNK</em>")
    '            Case "Y"
    '              htmlOut.Append(":&nbsp;<span class='label'>YES</span>")
    '            Case "N"
    '              htmlOut.Append(":&nbsp;<em>NO</em>")
    '            Case "A"
    '              htmlOut.Append(":&nbsp;<em>ACCIDENT</em>")
    '            Case "I"
    '              htmlOut.Append(":&nbsp;<em>INCIDENT</em>")
    '            Case Else
    '              htmlOut.Append(":&nbsp;<em>UNK</em>")
    '          End Select
    '        Else
    '          htmlOut.Append(":&nbsp;<span class='label'>UNK</em>")
    '        End If





    '        htmlOut.Append("</span></td>")
    '        'htmlOut.Append("<td align='left' valign='top' width='5'>&nbsp;&nbsp;</td>")
    '        rowCounter += 1
    '      Else
    '        DamageCode = r("kff_name").ToString
    '      End If
    '    Next
    '    If ModelFeatures.Rows.Count > 0 Then
    '      'htmlOut.Append("<tr><td align='left' valign='top' width='5'>&nbsp;&nbsp;</td>")
    '      htmlOut.Append("<tr><td align='right' valign='top' colspan='2'><span class='blue_text emphasis_text'>BLUE - Standard Equipment*</span></td></tr>")
    '    End If
    '  Else
    '    htmlOut.Append("<tr><td valign='top' align='center' colspan='4'>No Feature Codes Found</td></tr>")
    '  End If

    '  htmlOut.Append("</table>")

    '  Return htmlOut.ToString.Trim

    'End Function

    'Public Shared Function DisplayKeyFeatures(ByVal aclsData_Temp As clsData_Manager_SQL, ByRef MySesState As HttpSessionState, _
    '                                        ByRef in_AircraftRs As DataTable, ByRef crmSource As String, ByRef DamageCode As String, ByRef bShowBlankAcFields As Boolean) As String

    '  Dim ModelFeatures As New DataTable
    '  Dim htmlOut As StringBuilder = New StringBuilder()
    '  Dim sQuery As String = ""
    '  Dim rowCounter As Integer = 0
    '  Dim Results_Table As New DataTable
    '  Dim FilteredTable As New DataTable
    '  Dim StandardFeature As Boolean = False

    '  Dim NonstandardFeaturesLeft As String = ""
    '  Dim NonstandardFeaturesRight As String = ""
    '  Dim NonstandardFeaturesCount As Integer = 0
    '  Dim StandardEquipmentLeft As String = ""
    '  Dim StandardEquipmentRight As String = ""
    '  Dim StandardEquipmentCount As Integer = 0
    '  Dim FeaturesNotOnAircraft As String = ""
    '  Dim FeaturesNotReported As String = ""

    '  htmlOut.Append("<table cellpadding='0' cellspacing='0' width='100%'>")


    '  ModelFeatures = aclsData_Temp.GetJETNET_Model_Standard_Key_Features(in_AircraftRs.Rows(0).Item("ac_amod_id"))

    '  If crmSource = "CLIENT" Then
    '    Results_Table = clsGeneral.clsGeneral.Get_Client_Aircraft_Key_Features_As_Jetnet_Fields(in_AircraftRs.Rows(0).Item("CLIENT_ID"))
    '  Else
    '    Results_Table = aclsData_Temp.GetJETNET_Aircraft_Details_Ordered_Features_AC_ID(in_AircraftRs.Rows(0).Item("ac_id"), in_AircraftRs.Rows(0).Item("ac_journ_id"))
    '  End If

    '  If Results_Table.Rows.Count > 0 Then

    '    For Each r As DataRow In Results_Table.Rows
    '      If r("kfeat_code") <> "DAM" Then

    '        'First Thing is first, we need to run a test to see if this is a blue standard feature.
    '        If ModelFeatures.Rows.Count > 0 Then
    '          StandardFeature = False
    '          FilteredTable = New DataTable
    '          FilteredTable = ModelFeatures.Clone
    '          Dim afiltered_Client As DataRow() = ModelFeatures.Select("amfeat_feature_code = '" & r("kfeat_code") & "'", "")
    '          ' extract and import
    '          For Each atmpDataRow_Client In afiltered_Client
    '            FilteredTable.ImportRow(atmpDataRow_Client)
    '          Next
    '          If FilteredTable.Rows.Count > 0 Then
    '            StandardFeature = True
    '          End If
    '        End If



    '        If Not IsDBNull(r("kff_name")) And Not String.IsNullOrEmpty(r("kff_name").ToString) Then
    '          If Not IsDBNull(r("kfeat_name")) And Not String.IsNullOrEmpty(r("kfeat_name").ToString) Then
    '            Select Case (r("kff_name").ToString.ToUpper)
    '              Case "U"
    '                If FeaturesNotReported <> "" Then
    '                  FeaturesNotReported += ", "
    '                End If
    '                FeaturesNotReported += r("kfeat_name").ToString.Trim
    '              Case "Y"
    '                If StandardFeature = True Then
    '                  If StandardEquipmentCount = 0 Then
    '                    StandardEquipmentLeft += "<span class=""li"">" & r("kfeat_name").ToString.Trim & "</span>"
    '                    StandardEquipmentCount += 1
    '                  Else
    '                    StandardEquipmentCount = 0
    '                    StandardEquipmentRight += "<span class=""li"">" & r("kfeat_name").ToString.Trim & "</span>"
    '                  End If
    '                Else
    '                  If NonstandardFeaturesCount = 0 Then
    '                    NonstandardFeaturesLeft += "<span class=""li"">" & r("kfeat_name").ToString.Trim & "</span>"
    '                    NonstandardFeaturesCount += 1
    '                  Else
    '                    NonstandardFeaturesCount = 0
    '                    NonstandardFeaturesRight += "<span class=""li"">" & r("kfeat_name").ToString.Trim & "</span>"
    '                  End If
    '                End If
    '              Case "N"
    '                If FeaturesNotOnAircraft <> "" Then
    '                  FeaturesNotOnAircraft += ", "
    '                End If
    '                FeaturesNotOnAircraft += r("kfeat_name").ToString.Trim
    '            End Select
    '          End If
    '        End If



    '        rowCounter += 1
    '      Else
    '        DamageCode = r("kff_name").ToString
    '      End If
    '    Next
    '    If Results_Table.Rows.Count > 1 Or (Results_Table.Rows.Count = 1 And DamageCode = "") Then
    '      htmlOut.Append("<tr><td align=""left"" valign=""top"" class=""featuresTable"">")
    '      If NonstandardFeaturesLeft <> "" Or NonstandardFeaturesRight <> "" Then
    '        htmlOut.Append("<div class=""row remove_margin""><div class=""row remove_margin""><strong class=""featuresHeader"">Equipped With</strong></div><div class=""four columns remove_margin"">" & NonstandardFeaturesLeft & "</div><div class=""four columns remove_margin"">" & NonstandardFeaturesRight & "</div><div class=""clearfix""></div></div>")
    '      End If
    '      If StandardEquipmentLeft <> "" Or StandardEquipmentRight <> "" Then
    '        htmlOut.Append("<div class=""alt_row""><div class=""row remove_margin" & IIf(NonstandardFeaturesLeft <> "", " border_top", "") & """><strong class=""featuresHeader"">Standard Equipment</strong></div><div class=""four columns remove_margin"">" & StandardEquipmentLeft & "</div><div class=""four columns remove_margin"">" & StandardEquipmentRight & "</div><div class=""clearfix""></div></div>")
    '      End If
    '      If FeaturesNotOnAircraft <> "" Then
    '        htmlOut.Append("<div class=""padding""><strong>NOT EQUIPPED WITH:</strong> " & FeaturesNotOnAircraft & "<div class=""clearfix""></div></div>")
    '      End If
    '      If bShowBlankAcFields Then
    '        If FeaturesNotReported <> "" Then
    '          htmlOut.Append("<div class=""padding""><strong>FEATURES (STATUS UNKNOWN):</strong> " & FeaturesNotReported & "<div class=""clearfix""></div></div>")
    '        End If
    '      Else 'if everything else is blank.
    '        If NonstandardFeaturesLeft = "" And NonstandardFeaturesRight = "" And StandardEquipmentLeft = "" And StandardEquipmentRight = "" And FeaturesNotOnAircraft = "" Then
    '          'acid=159137 is an example:
    '          htmlOut.Append("<span class='li'>No Features Details</span>")
    '        End If
    '      End If
    '      htmlOut.Append("</td></tr>")

    '    Else
    '      htmlOut.Append("<tr><td valign='top' align='left'><span class='li'>No Features Details</span></td></tr>")
    '    End If
    '  Else
    '    htmlOut.Append("<tr><td valign='top' align='left'><span class='li'>No Features Details</span></td></tr>")
    '  End If
    '  htmlOut.Append("</table>")

    '  Return htmlOut.ToString.Trim

    'End Function
    Public Shared Function DisplayKeyFeatures(ByVal aclsData_Temp As clsData_Manager_SQL, ByRef MySesState As HttpSessionState,
                                          ByRef in_AircraftRs As DataTable, ByRef crmSource As String, ByRef DamageCode As String, ByRef bShowBlankAcFields As Boolean, ByVal tableColor As String, ByVal aircraftID As Long, ByVal showEditLink As Boolean, ByVal journalID As Long, ByVal otherID As Long, Optional ByVal spacer_width As String = "") As String

        Dim ModelFeatures As New DataTable
        Dim htmlOut As StringBuilder = New StringBuilder()
        Dim sQuery As String = ""
        Dim rowCounter As Integer = 0
        Dim Results_Table As New DataTable
        Dim FilteredTable As New DataTable
        Dim StandardFeature As Boolean = False

        Dim NonstandardFeaturesLeft As String = ""
        Dim NonstandardFeaturesRight As String = ""
        Dim NonstandardFeaturesCount As Integer = 0
        Dim StandardEquipmentLeft As String = ""
        Dim StandardEquipmentRight As String = ""
        Dim StandardEquipmentCount As Integer = 0
        Dim FeaturesNotOnAircraft As String = ""
        Dim FeaturesNotReported As String = ""

        htmlOut.Append("<div class=""Box""><table cellpadding='0' cellspacing='0' width='100%' class=""formatTable " & tableColor & """>")
        htmlOut.Append("<tr class=""noBorder""><td colspan='2'><font class='" & HttpContext.Current.Session("FONT_CLASS_HEADER") & " subHeader'>Features" & IIf(showEditLink, CreateEditLink("features", crmSource, aircraftID, "height=400,width=450", False, False, ""), "") & "</font></td></tr>")

        ModelFeatures = aclsData_Temp.GetJETNET_Model_Standard_Key_Features(in_AircraftRs.Rows(0).Item("ac_amod_id"))

        If crmSource = "CLIENT" And journalID = 0 Then
            Results_Table = clsGeneral.clsGeneral.Get_Client_Aircraft_Key_Features_As_Jetnet_Fields(in_AircraftRs.Rows(0).Item("CLIENT_ID"))
        ElseIf crmSource = "CLIENT" And journalID > 0 Then
            Results_Table = aclsData_Temp.GetJETNET_Aircraft_Details_Ordered_Features_AC_ID(otherID, in_AircraftRs.Rows(0).Item("ac_journ_id"))
        Else
            Results_Table = aclsData_Temp.GetJETNET_Aircraft_Details_Ordered_Features_AC_ID(in_AircraftRs.Rows(0).Item("ac_id"), in_AircraftRs.Rows(0).Item("ac_journ_id"))
        End If

        If Results_Table.Rows.Count > 0 Then

            For Each r As DataRow In Results_Table.Rows
                If r("kfeat_code") <> "DAM" Then

                    'First Thing is first, we need to run a test to see if this is a blue standard feature.
                    If ModelFeatures.Rows.Count > 0 Then
                        StandardFeature = False
                        FilteredTable = New DataTable
                        FilteredTable = ModelFeatures.Clone
                        Dim afiltered_Client As DataRow() = ModelFeatures.Select("amfeat_feature_code = '" & r("kfeat_code") & "'", "")
                        ' extract and import
                        For Each atmpDataRow_Client In afiltered_Client
                            FilteredTable.ImportRow(atmpDataRow_Client)
                        Next
                        If FilteredTable.Rows.Count > 0 Then
                            StandardFeature = True
                        End If
                    End If



                    If Not IsDBNull(r("kff_name")) And Not String.IsNullOrEmpty(r("kff_name").ToString) Then
                        If Not IsDBNull(r("kfeat_name")) And Not String.IsNullOrEmpty(r("kfeat_name").ToString) Then
                            Select Case (r("kff_name").ToString.ToUpper)
                                Case "U"
                                    If FeaturesNotReported <> "" Then
                                        FeaturesNotReported += ", "
                                    End If
                                    FeaturesNotReported += r("kfeat_name").ToString.Trim
                                Case "Y"
                                    If StandardFeature = True Then
                                        If StandardEquipmentCount = 0 Then
                                            StandardEquipmentLeft += "<tr><td align=""left"" valign=""top"" " & IIf(spacer_width = "", " width=""50%""", "  width='" & spacer_width & "'") & ">" & r("kfeat_name").ToString.Trim & "</td>"
                                            StandardEquipmentCount += 1
                                        Else
                                            StandardEquipmentCount = 0
                                            StandardEquipmentLeft += "<td align=""left"" valign=""top"" " & IIf(spacer_width = "", " width=""50%""", "  width='" & spacer_width & "'") & ">" & r("kfeat_name").ToString.Trim & "</td></tr>"
                                        End If
                                    Else
                                        If NonstandardFeaturesCount = 0 Then
                                            NonstandardFeaturesLeft += "<tr><td align=""left"" valign=""top"" " & IIf(spacer_width = "", " width=""50%""", "  width='" & spacer_width & "'") & ">" & r("kfeat_name").ToString.Trim & "</td>"
                                            NonstandardFeaturesCount += 1
                                        Else
                                            NonstandardFeaturesCount = 0
                                            NonstandardFeaturesLeft += "<td align=""left"" valign=""top"" " & IIf(spacer_width = "", " width=""50%""", "  width='" & spacer_width & "'") & ">" & r("kfeat_name").ToString.Trim & "</td></tr>"
                                        End If
                                    End If
                                Case "N"
                                    If FeaturesNotOnAircraft <> "" Then
                                        FeaturesNotOnAircraft += ", "
                                    End If
                                    FeaturesNotOnAircraft += r("kfeat_name").ToString.Trim
                            End Select
                        End If
                    End If



                    rowCounter += 1
                Else
                    DamageCode = r("kff_name").ToString
                End If
            Next
            If Results_Table.Rows.Count > 1 Or (Results_Table.Rows.Count = 1 And DamageCode = "") Then
                ' htmlOut.Append("<tr><td align=""left"" valign=""top"" class=""featuresTable"">")
                If NonstandardFeaturesLeft <> "" Or NonstandardFeaturesRight <> "" Then
                    htmlOut.Append("<tr><td align=""left"" valign=""top""><strong class=""featuresHeader"">Equipped With</strong></td></tr><tr><td align=""left"" valign=""top""><table width=""100%"" cellpadding=""0"" cellspacing=""0"">" & NonstandardFeaturesLeft & NonstandardFeaturesRight & "</table></td></tr>")
                End If
                If StandardEquipmentLeft <> "" Or StandardEquipmentRight <> "" Then
                    htmlOut.Append("<tr><td align=""left"" valign=""top""><strong class=""featuresHeader"">Standard Equipment</strong></td></tr><tr><td align=""left"" valign=""top""><table width=""100%"" cellpadding=""0"" cellspacing=""0"">" & StandardEquipmentLeft & StandardEquipmentRight & "</table></td></tr>")
                End If
                If FeaturesNotOnAircraft <> "" Then
                    htmlOut.Append("<tr><td align=""left"" valign=""top""><strong class=""featuresHeader"">NOT EQUIPPED WITH:</strong></td></tr><tr><td align=""left"" valign=""top""><table width=""100%"" cellpadding=""0"" cellspacing=""0""><tr><td align=""left"" valign=""top"">" & FeaturesNotOnAircraft & "</td></tr></table></td></tr>")
                End If
                If bShowBlankAcFields Then
                    If FeaturesNotReported <> "" Then
                        htmlOut.Append("<tr><td align=""left"" valign=""top""><table width=""100%"" cellpadding=""0"" cellspacing=""0""><tr><td align=""left"" valign=""top""><strong>FEATURES (STATUS UNKNOWN):</strong></td></tr><tr><td align=""left"" valign=""top"">" & FeaturesNotReported & "</td></tr></table></td></tr>")
                    End If
                Else 'if everything else is blank.
                    If NonstandardFeaturesLeft = "" And NonstandardFeaturesRight = "" And StandardEquipmentLeft = "" And StandardEquipmentRight = "" And FeaturesNotOnAircraft = "" Then
                        'acid=159137 is an example:
                        htmlOut.Append("<tr><td align=""left"" valign=""top"">No Features Details</td></tr>")
                    End If
                End If
                'htmlOut.Append("</td></tr>")

            Else
                htmlOut.Append("<tr><td valign='top' align='left'>No Features Details</td></tr>")
            End If
        Else
            htmlOut.Append("<tr><td valign='top' align='left'>No Features Details</td></tr>")
        End If
        htmlOut.Append("</table></div>")

        Return htmlOut.ToString.Trim

    End Function
    Public Shared Function DisplayAttributes(ByVal aclsData_Temp As clsData_Manager_SQL, ByRef MySesState As HttpSessionState,
                                          ByRef in_AircraftRs As DataTable, ByRef crmSource As String, ByRef DamageCode As String, ByRef bShowBlankAcFields As Boolean, ByVal tableColor As String, ByVal aircraftID As Long, ByVal showEditLink As Boolean, ByVal journalID As Long, ByVal otherID As Long, Optional ByVal spacer_width As String = "") As String

        Dim ModelFeatures As New DataTable
        Dim htmlOut As StringBuilder = New StringBuilder()
        Dim sQuery As String = ""
        Dim rowCounter As Integer = 0
        Dim Results_Table As New DataTable
        Dim FilteredTable As New DataTable
        Dim StandardFeature As Boolean = False

        Dim last_feature As String = ""
        Dim col_count As Integer = 1
        Dim overall_count As Integer = 0

        htmlOut.Append("<div class=""Box""><table cellpadding='0' cellspacing='0' width='100%' class=""formatTable " & tableColor & """>")
        htmlOut.Append("<tr class=""noBorder""><td colspan='1'><font class='" & HttpContext.Current.Session("FONT_CLASS_HEADER") & " subHeader'>FEATURES/ATTRIBUTES" & IIf(showEditLink, CreateEditLink("features", crmSource, aircraftID, "height=400,width=450", False, False, ""), "") & "</font></td>")

        Results_Table = aclsData_Temp.GET_AC_ATTRIBUTES_STANDARD_AND_EQUIPPED(in_AircraftRs.Rows(0).Item("ac_id"))

        '  Results_Table = aclsData_Temp.GetJETNET_Aircraft_Details_Ordered_Features_AC_ID(in_AircraftRs.Rows(0).Item("ac_id"), in_AircraftRs.Rows(0).Item("ac_journ_id"))
        If Not IsNothing(Results_Table) Then
            If Results_Table.Rows.Count > 0 Then
                For Each r As DataRow In Results_Table.Rows

                    If overall_count = 0 Then
                        If Not IsDBNull(r.Item("ac_amod_id")) Then
                            htmlOut.Append("<td align='right'><a href='home_model.aspx?modelID=" & Trim(r.Item("ac_amod_id")) & "' target='_blank'>Edit Model Attributes</a></td></tr>")
                        Else
                            htmlOut.Append("<td>&nbsp;</td></tr>")
                        End If
                    End If
                    overall_count += 1


                    If (Trim(last_feature) <> Trim(r.Item("FEATURES"))) Or Trim(last_feature) = "" Then
                        If Trim(last_feature) <> "" Then
                            htmlOut.Append("</tr>")
                        End If
                        htmlOut.Append("<tr><td colspan='3'><b>" & Trim(r.Item("FEATURES")) & "</b></td></tr>")
                        htmlOut.Append("<tr>")
                        col_count = 1
                    Else
                        If col_count = 1 Then

                        ElseIf col_count = 3 Then ' if we r on 3, we need to reset the row , only 2 per ro
                            htmlOut.Append("</tr><tr>")
                            col_count = 1
                        End If
                    End If

                    If Not IsDBNull(r.Item("acatt_name")) Then
                        htmlOut.Append("<td align='left'>" & Trim(r.Item("acatt_name")) & "</td>")
                    End If

                    ' If Not IsDBNull(r.Item("acatt_name")) Then
                    '  htmlOut.Append("<td>" & Trim(r.Item("acatt_name")) & "</td>")
                    ' End If

                    last_feature = Trim(r.Item("FEATURES"))
                    col_count = col_count + 1
                Next

                htmlOut.Append("</tr>")
            Else
                If overall_count = 0 Then
                    htmlOut.Append("<td align='right'><a href='home_model.aspx?modelID=" & Trim(in_AircraftRs.Rows(0).Item("ac_amod_id")) & "' target='_blank'>Edit Model Attributes</a></td></tr>")

                    htmlOut.Append("<tr><td>No FEATURES/ATTRIBUTES</td></tr>")
                End If
            End If
        Else
            If overall_count = 0 Then
                htmlOut.Append("<td align='right'><a href='home_model.aspx?modelID=" & Trim(in_AircraftRs.Rows(0).Item("ac_amod_id")) & "' target='_blank'>Edit Model Attributes</a></td></tr>")

                htmlOut.Append("<tr><td>No FEATURES/ATTRIBUTES</td></tr>")
            End If
        End If

        'in_AircraftRs.Rows(0).Item("ac_id")

        htmlOut.Append("</table></div>")


        Results_Table.Clear()

        htmlOut.Append("<br/>")

        htmlOut.Append("<div class=""Box""><table cellpadding='0' cellspacing='0' width='100%' class=""formatTable " & tableColor & """>")
        htmlOut.Append("<tr class=""noBorder""><td colspan='2'><font class='" & HttpContext.Current.Session("FONT_CLASS_HEADER") & " subHeader'>ADDITIONAL ASSETS" & IIf(showEditLink, CreateEditLink("features", crmSource, aircraftID, "height=400,width=450", False, False, ""), "") & "</font></td></tr>")

        Results_Table = aclsData_Temp.GET_AC_ATTRIBUTES_ADDITIONAL_ASSETS(in_AircraftRs.Rows(0).Item("ac_id"))


        col_count = 1
        '  Results_Table = aclsData_Temp.GetJETNET_Aircraft_Details_Ordered_Features_AC_ID(in_AircraftRs.Rows(0).Item("ac_id"), in_AircraftRs.Rows(0).Item("ac_journ_id"))
        If Not IsNothing(Results_Table) Then
            If Results_Table.Rows.Count > 0 Then
                For Each r As DataRow In Results_Table.Rows

                    If col_count = 1 Then
                        htmlOut.Append("<tr>")
                    ElseIf col_count = 3 Then ' if we r on 3, we need to reset the row , only 2 per ro
                        htmlOut.Append("</tr><tr>")
                        col_count = 1
                    End If

                    If Not IsDBNull(r.Item("acatt_name")) Then
                        htmlOut.Append("<td align='left'>" & Trim(r.Item("acatt_name")) & "</td>")
                    End If

                    col_count = col_count + 1
                Next

                htmlOut.Append("</tr>")
            Else
                htmlOut.Append("<tr><td valign='top' align='left'>No ADDITIONAL ASSETS</td></tr>")
            End If
        End If

        htmlOut.Append("</table></div>")




        Return htmlOut.ToString.Trim

    End Function

    Public Shared Function TrimAndTitleString(ByVal strToTrim As String, ByVal characterNumber As Integer) As String
        Dim resultsString As String = ""
        If UCase(HttpContext.Current.Request.RawUrl.ToString()).Contains("/PDF_CREATOR.ASPX") Then
            resultsString = strToTrim
        Else

            If strToTrim.Length > characterNumber Then
                resultsString = "<span title=""" & strToTrim & """>" & Left(strToTrim, characterNumber) & "...</span>"
            Else
                resultsString = strToTrim
            End If

        End If

        Return resultsString
    End Function
    ''' <summary>
    ''' Shows the Company Details on the AC Details Page.
    ''' </summary>
    ''' <param name="MySesState"></param>
    ''' <param name="in_AircraftRs"></param>
    ''' <param name="isDisplay"></param>
    ''' <param name="showFullContact"></param>
    ''' <param name="isJFWAFW"></param>
    ''' <param name="aclsData_Temp"></param>
    ''' <returns></returns>
    ''' <remarks></remarks> 
    Public Shared Function GetCompanies_DisplayAircraftDetails(ByRef MySesState As HttpSessionState,
                                                           ByRef in_AircraftRs As DataTable,
                                                           ByVal isDisplay As Boolean,
                                                           ByVal showFullContact As Boolean,
                                                           ByVal isJFWAFW As Boolean, ByVal aclsData_Temp As clsData_Manager_SQL, ByRef crmSource As String, ByVal journalID As Long, ByVal otherID As Long, ByVal in_AircraftID As Long, Optional ByVal exclude_brokers As Boolean = False, Optional ByVal showHistoryWarning As Boolean = False) As String

        Dim htmlOut As StringBuilder = New StringBuilder()
        Dim sQuery As String = ""
        Dim class_color As String = "light_blue"
        ' create a DataSet to hold the aircraft contacts
        Dim dsAircraftCompanies As New DataTable
        Dim ShareRelationshipsTable As New DataTable
        Dim strSeqNo As String = ""
        Dim strCompanyTypeName As String = ""
        Dim strCompanyTypeNbr As String = ""
        Dim strCompanyIdFilter As String = ""

        Dim fCompany_name As String = ""
        Dim fAirRef_contact_type As String = ""
        Dim fAirContactType_name As String = ""
        Dim fAirRef_fraction_expires_date As String = ""

        Dim nContactCount As Long = 0

        Dim fAirRef_company_id As Long = 0
        Dim fAirRef_old_company_id As Long = 0
        Dim fAirRef_company_journ_id As Long = 0
        Dim fAirRef_contact_id As Long = 0
        Dim fAirRef_ac_id As Long = 0
        Dim fAirRef_id As Long = 0
        Dim fAirRef_transmit_seq_no As Integer = 0
        Dim fAirRef_owner_percent As Double = 0
        Dim tmpID As String = ""
        Dim contact_info As String = ""
        Dim temp_contact_info As String = ""
        Dim comp_info As String = ""
        Dim is_multi_contact As Boolean = False

        'datarow filtering variables.
        Dim afileterd As DataRow()

        htmlOut.Append("<div class=""Box""><table class='formatTable blue companyTable' cellspacing='0' cellpadding='0' border='0' width='100%'>")
        htmlOut.Append("<tr class=""noBorder""><td valign='top' align='left' colspan='2'><span class='" & HttpContext.Current.Session("FONT_CLASS_HEADER") & " subHeader'"">Company/Contacts ")
        If clsGeneral.clsGeneral.isCrmDisplayMode Then
            If in_AircraftID > 0 And journalID = 0 Then
                If crmSource = "CLIENT" Then
                    Dim pageLink As String = ""
                    pageLink = "/edit.aspx?from=aircraftDetails&action=reference&listing=3&"
                    pageLink += "itemID=" & in_AircraftID & "&source=" & crmSource
                    htmlOut.Append("<a href=""javascript:void(0)"" class=""float_right"" onclick=""javascript:load('" & pageLink & "','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');"">Add Reference</a>")
                End If
            End If
        End If

        htmlOut.Append("</span></td></tr>")
        If journalID > 0 And showHistoryWarning = False Then
            htmlOut.Append("<tr class=""noBorder""><td valign='top' align='left' colspan='2'><span class=""historyTag"">The data below is for the companies/contacts as of the date of this transaction. To view current data for these companies/contacts if available click  <a class=""historyTag underline viewCurrentCompanies"" href=""javascript:void(0);"">here</a>.</span></td></tr>")
        ElseIf showHistoryWarning = True Then
            htmlOut.Append("<tr class=""noBorder""><td valign='top' align='left' colspan='2'><span class=""historyTag"">Below is latest data for companies/contacts involved in this transaction. To view data for these companies/contacts at the time of sale click <a class=""historyTag underline viewTransCompanies"" href=""javascript:void(0);"">here</a>.</td></tr>")
            ' noting that for some companies and contacts there may be no active records … especially older ones
        End If

        htmlOut.Append("<tr><td valign='top' align='left' colspan='2'>")


        'This gets a list of References

        If crmSource = "CLIENT" And journalID = 0 Then
            dsAircraftCompanies = clsGeneral.clsGeneral.Get_Aircraft_Reference_Client_acID_As_JetnetFields(in_AircraftID)
        ElseIf showHistoryWarning = True Then
            'new query
            If crmSource = "CLIENT" And journalID > 0 Then
                dsAircraftCompanies = aclsData_Temp.Get_Aircraft_Reference_Jetnet_acID_CURRENT_COMPANY(otherID, in_AircraftRs.Rows(0).Item("ac_journ_id"))
            Else
                dsAircraftCompanies = aclsData_Temp.Get_Aircraft_Reference_Jetnet_acID_CURRENT_COMPANY(in_AircraftRs.Rows(0).Item("ac_id"), in_AircraftRs.Rows(0).Item("ac_journ_id"))
            End If

        ElseIf crmSource = "CLIENT" And journalID > 0 Then
            dsAircraftCompanies = aclsData_Temp.Get_Aircraft_Reference_Jetnet_acID(otherID, in_AircraftRs.Rows(0).Item("ac_journ_id"))
        Else
            dsAircraftCompanies = aclsData_Temp.Get_Aircraft_Reference_Jetnet_acID(in_AircraftRs.Rows(0).Item("ac_id"), in_AircraftRs.Rows(0).Item("ac_journ_id"))
        End If

        'Next we're going ahead and parsing through the reference answers
        If Not IsNothing(dsAircraftCompanies) Then
            If dsAircraftCompanies.Rows.Count > 0 Then
                'Split Data
                Dim tmpCompanyArray() As String = SplitUserData(Get_ReferenceCompanyIDs(dsAircraftCompanies), Constants.cCommaDelim)
                'For each company in the array
                For x As Integer = 0 To UBound(tmpCompanyArray)
                    fAirRef_company_id = CLng(tmpCompanyArray(x).ToString)
                    'Selecting a subset of company information where compid = company ID in array
                    afileterd = dsAircraftCompanies.Select("comp_id IN (" + fAirRef_company_id.ToString + ")", "acref_transmit_seq_no")
                    Dim dalTable As DataTable = dsAircraftCompanies.Clone

                    ' extract and import
                    dalTable.Clear()
                    dalTable.Rows.Clear()

                    For Each atmpDataRow As DataRow In afileterd
                        dalTable.ImportRow(atmpDataRow)
                    Next

                    'Going through filtered Table to display information.
                    If dalTable.Rows.Count > 0 Then
                        ' Clear These
                        strSeqNo = ""
                        strCompanyTypeNbr = ""
                        strCompanyTypeName = ""
                        If class_color = "light_blue" Then
                            class_color = "light_gray"
                        Else
                            class_color = "light_blue"
                        End If
                        If Not IsDBNull(dalTable.Rows(0).Item("comp_name")) And Not String.IsNullOrEmpty(dalTable.Rows(0).Item("comp_name").ToString) Then
                            fCompany_name = TrimAndTitleString(dalTable.Rows(0).Item("comp_name").ToString.Trim, 24)
                        Else
                            fCompany_name = ""
                        End If
                        'This goes through each matching row in the table
                        Dim share As String = ""
                        ShareRelationshipsTable = New DataTable
                        is_multi_contact = False

                        For y As Integer = 0 To dalTable.Rows.Count - 1


                            If Not IsDBNull(dalTable.Rows(y).Item("acref_transmit_seq_no")) And Not String.IsNullOrEmpty(dalTable.Rows(y).Item("acref_transmit_seq_no").ToString) Then
                                fAirRef_transmit_seq_no = CInt(dalTable.Rows(y).Item("acref_transmit_seq_no").ToString)
                            Else
                                fAirRef_transmit_seq_no = 0
                            End If

                            If Not IsDBNull(dalTable.Rows(y).Item("acref_contact_type")) And Not String.IsNullOrEmpty(dalTable.Rows(y).Item("acref_contact_type").ToString) Then
                                fAirRef_contact_type = dalTable.Rows(y).Item("acref_contact_type").ToString.Trim
                            Else
                                fAirRef_contact_type = ""
                            End If

                            ' exclude the exclusive broker, if its checked too 

                            If Not IsDBNull(dalTable.Rows(y).Item("acref_id")) And Not String.IsNullOrEmpty(dalTable.Rows(y).Item("acref_id").ToString) Then
                                fAirRef_id = CLng(dalTable.Rows(y).Item("acref_id").ToString)
                            Else
                                fAirRef_id = 0
                            End If

                            If Not IsDBNull(dalTable.Rows(y).Item("acref_contact_id")) And Not String.IsNullOrEmpty(dalTable.Rows(y).Item("acref_contact_id").ToString) Then
                                fAirRef_contact_id = CLng(dalTable.Rows(y).Item("acref_contact_id").ToString)
                            Else
                                fAirRef_contact_id = 0
                            End If

                            If Not IsDBNull(dalTable.Rows(y).Item("acref_ac_id")) And Not String.IsNullOrEmpty(dalTable.Rows(y).Item("acref_ac_id").ToString) Then
                                fAirRef_ac_id = CLng(dalTable.Rows(y).Item("acref_ac_id").ToString)
                            Else
                                fAirRef_ac_id = 0
                            End If

                            If Not IsDBNull(dalTable.Rows(y).Item("acref_owner_percentage")) And Not String.IsNullOrEmpty(dalTable.Rows(y).Item("acref_owner_percentage").ToString) Then
                                If dalTable.Rows(y).Item("acref_owner_percentage") = 100 Then
                                    fAirRef_owner_percent = 0 'do not display percentage if 100
                                Else
                                    fAirRef_owner_percent = CDbl(dalTable.Rows(y).Item("acref_owner_percentage").ToString)
                                End If

                            Else
                                fAirRef_owner_percent = 0
                            End If

                            If Not IsDBNull(dalTable.Rows(y).Item("acref_fraction_expires_date")) And Not String.IsNullOrEmpty(dalTable.Rows(y).Item("acref_fraction_expires_date").ToString) Then
                                fAirRef_fraction_expires_date = dalTable.Rows(y).Item("acref_fraction_expires_date").ToString.Trim
                            Else
                                fAirRef_fraction_expires_date = ""
                            End If

                            If Not IsDBNull(dalTable.Rows(y).Item("act_name")) And Not String.IsNullOrEmpty(dalTable.Rows(y).Item("act_name").ToString) Then
                                fAirContactType_name = dalTable.Rows(y).Item("act_name").ToString.Trim
                            Else
                                fAirContactType_name = ""
                            End If

                            If Not IsDBNull(dalTable.Rows(y).Item("comp_journ_id")) And Not String.IsNullOrEmpty(dalTable.Rows(y).Item("comp_journ_id").ToString) Then
                                fAirRef_company_journ_id = CLng(dalTable.Rows(y).Item("comp_journ_id").ToString)
                            Else
                                fAirRef_company_journ_id = 0
                            End If

                            If exclude_brokers = True And fAirRef_contact_type = "99" Then
                            Else
                                If String.IsNullOrEmpty(strSeqNo) Then
                                    strSeqNo = fAirRef_transmit_seq_no.ToString
                                Else
                                    strSeqNo &= Constants.cCommaDelim + " " + fAirRef_transmit_seq_no.ToString
                                End If

                                If String.IsNullOrEmpty(strCompanyTypeNbr) Then
                                    strCompanyTypeNbr &= Constants.cSingleQuote + fAirRef_contact_type.Trim + Constants.cSingleQuote
                                Else
                                    strCompanyTypeNbr &= Constants.cCommaDelim + " " + Constants.cSingleQuote + fAirRef_contact_type.Trim + Constants.cSingleQuote
                                End If

                                If Not fAirRef_contact_type.Contains("02") And Not fAirRef_contact_type.Contains("66") And Not fAirRef_contact_type.Contains("67") _
                                   And Not fAirRef_contact_type.Contains("68") And Not fAirRef_contact_type.Contains("44") Then

                                    If fAirRef_contact_type.Contains("97") Or fAirRef_contact_type.Contains("17") Then
                                        strCompanyTypeName &= "<span class='single_company'>"

                                        'run ShareRelationshipsCheck
                                        ShareRelationshipsTable = aclsData_Temp.ShareRelationshipExist(fAirRef_id, 0)

                                        If ShareRelationshipsTable.Rows.Count > 0 Then
                                            For Each m As DataRow In ShareRelationshipsTable.Rows
                                                share += "<span>" & m("actype_name").ToString & " - " & crmWebClient.DisplayFunctions.WriteDetailsLink(0, m("comp_id"), 0, 0, True, TrimAndTitleString(m("comp_name").ToString, 24), "", "") & ", " & IIf(Not String.IsNullOrEmpty(m("comp_city").ToString), m("comp_city").ToString & ", ", "") & IIf(Not String.IsNullOrEmpty(m("comp_state").ToString), m("comp_state").ToString & ", ", "") & IIf(Not String.IsNullOrEmpty(m("comp_country").ToString), Replace(m("comp_country").ToString, "United States", "US") & " ", "")
                                                If Not IsDBNull(m("contact_id")) Then
                                                    share += " (" & crmWebClient.DisplayFunctions.WriteDetailsLink(0, m("comp_id"), m("contact_id"), 0, True, m("contact_first_name").ToString & " " & m("contact_last_name").ToString, "", IIf(crmSource = "CLIENT", "&source=CLIENT", "")) & ") </span>"
                                                End If
                                            Next
                                            If isDisplay Then
                                                strCompanyTypeName &= fAirContactType_name ' + " [" + fAirRef_owner_percent.ToString + "%]</a>&nbsp;" + Constants.cCommaDelim + " "
                                            Else
                                                strCompanyTypeName &= fAirContactType_name '+ " [" + fAirRef_owner_percent + "%]&nbsp;" + Constants.cCommaDelim + " "
                                            End If
                                        Else
                                            strCompanyTypeName &= fAirContactType_name
                                        End If

                                        ShareRelationshipsTable.Dispose()

                                        If fAirRef_owner_percent > 0 Then
                                            strCompanyTypeName &= " <span class='tiny_text'>[<em>" + fAirRef_owner_percent.ToString.Trim + "%</em>"
                                            If strCompanyTypeName.Contains(fAirRef_fraction_expires_date) Then
                                                strCompanyTypeName &= "]"
                                                strCompanyTypeName &= "</span>"
                                            End If

                                        End If


                                        If y <> dalTable.Rows.Count - 1 Then
                                            strCompanyTypeName &= Constants.cCommaDelim + " "
                                        End If

                                        ' End If

                                    ElseIf fAirRef_contact_type.Contains("08") Then
                                        'If strCompanyTypeName <> "" Then
                                        '    strCompanyTypeName &= vbNewLine
                                        'End If
                                        strCompanyTypeName &= "<div class='single_company'>" & fAirContactType_name

                                        If fAirRef_owner_percent > 0 Then
                                            strCompanyTypeName &= " <span class='tiny_text'>[<em>" + fAirRef_owner_percent.ToString.Trim + "%</em>"
                                            If strCompanyTypeName.Contains(fAirRef_fraction_expires_date) Then
                                                strCompanyTypeName &= "]"
                                                strCompanyTypeName &= "</span>"
                                            End If
                                        End If

                                        If y <> dalTable.Rows.Count - 1 Then
                                            strCompanyTypeName &= Constants.cCommaDelim + " "
                                        End If

                                    Else
                                        'If strCompanyTypeName <> "" Then
                                        '    strCompanyTypeName &= "<br />"
                                        'End If
                                        If fAirRef_contact_type.Contains("66") Or fAirRef_contact_type.Contains("67") Or fAirRef_contact_type.Contains("68") Then
                                            fAirContactType_name = "Additional Company/Contact"
                                        End If

                                        If Not strCompanyTypeName.Contains(fAirContactType_name) Then
                                            strCompanyTypeName &= fAirContactType_name
                                            If dalTable.Rows.Count > 1 Then
                                                is_multi_contact = True
                                            End If
                                        End If

                                        If fAirRef_owner_percent > 0 Then
                                            strCompanyTypeName &= " <span class='tiny_text'>[<em>" + fAirRef_owner_percent.ToString.Trim + "%</em>"
                                            If strCompanyTypeName.Contains(fAirRef_fraction_expires_date) Then
                                                strCompanyTypeName &= "]"
                                                strCompanyTypeName &= "</span>"
                                            End If
                                        End If

                                        If y <> dalTable.Rows.Count - 1 Then
                                            strCompanyTypeName &= Constants.cCommaDelim + " "
                                        End If

                                    End If

                                Else

                                    If fAirRef_contact_type.Contains("66") Or fAirRef_contact_type.Contains("67") Or fAirRef_contact_type.Contains("68") Then
                                        fAirContactType_name = "Additional Company/Contact"
                                    End If

                                    If Not strCompanyTypeName.Contains(fAirContactType_name) Then
                                        strCompanyTypeName &= fAirContactType_name

                                        If fAirRef_owner_percent > 0 Then
                                            strCompanyTypeName &= " <span class='tiny_text'>[<em>" + fAirRef_owner_percent.ToString.Trim + "%</em>"
                                            If strCompanyTypeName.Contains(fAirRef_fraction_expires_date) Then
                                                strCompanyTypeName &= "]"
                                                strCompanyTypeName &= "</span>"
                                            End If
                                        End If


                                        If y <> dalTable.Rows.Count - 1 Then
                                            strCompanyTypeName &= Constants.cCommaDelim + " "
                                        End If
                                    End If

                                End If

                                If Not strCompanyTypeName.Contains(fAirRef_fraction_expires_date) Then
                                    If fAirRef_owner_percent = 0 Then
                                        strCompanyTypeName &= "<span class='tiny_text'>["
                                    End If
                                    strCompanyTypeName &= " <em>Expires: " + FormatDateTime(fAirRef_fraction_expires_date, DateFormat.ShortDate) + "</em>]</span> " ' + Constants.cCommaDelim + " "
                                End If
                                If strCompanyTypeName <> "" Then
                                    strCompanyTypeName &= "</span>"
                                End If


                                If fAirRef_company_journ_id > 0 Then     ' only show current phone numbers 
                                    comp_info = (GetCompanyInfo_DisplayAircraft_No_Query(dalTable, y, MySesState, fAirRef_company_id, fAirRef_company_journ_id, strCompanyTypeName, isDisplay, False, isJFWAFW, aclsData_Temp, crmSource))
                                Else
                                    comp_info = (GetCompanyInfo_DisplayAircraft_No_Query(dalTable, y, MySesState, fAirRef_company_id, fAirRef_company_journ_id, strCompanyTypeName, isDisplay, True, isJFWAFW, aclsData_Temp, crmSource))
                                End If




                                If Not tmpID.Contains(fAirRef_contact_id) Then

                                    contact_info &= ("</td><td valign='top' align='left' width=""50%"">")


                                    If is_multi_contact = True And fAirRef_contact_id > 0 Then
                                        contact_info &= "<b>" & fAirContactType_name & "</b>|<br/>"
                                    End If

                                    If fAirRef_contact_type.Contains("44") Then
                                        'If aclsData_Temp.IsChiefPilot(in_AircraftRs.Rows(0).Item("ac_id").ToString, CLng(fAirRef_company_journ_id), fAirRef_contact_id) Then
                                        ' contact_info &= (GetContactInfoCompany(MySesState, CLng(fAirRef_contact_id), CLng(fAirRef_company_journ_id), True, isDisplay, False, isJFWAFW, aclsData_Temp, crmSource))
                                        contact_info &= (GetContactInfoCompany_No_Query(dalTable, y, MySesState, CLng(fAirRef_contact_id), CLng(fAirRef_company_journ_id), True, isDisplay, False, isJFWAFW, aclsData_Temp, crmSource))
                                    Else
                                        contact_info &= (GetContactInfoCompany_No_Query(dalTable, y, MySesState, CLng(fAirRef_contact_id), CLng(fAirRef_company_journ_id), False, isDisplay, False, isJFWAFW, aclsData_Temp, crmSource))
                                    End If

                                    contact_info &= ("</td></tr>")

                                ElseIf CLng(fAirRef_contact_id) > 0 Then
                                    ' added in to display the second or third or so contact type 
                                    temp_contact_info = ""
                                    If fAirRef_contact_type.Contains("44") Then
                                        'If aclsData_Temp.IsChiefPilot(in_AircraftRs.Rows(0).Item("ac_id").ToString, CLng(fAirRef_company_journ_id), fAirRef_contact_id) Then
                                        ' contact_info &= (GetContactInfoCompany(MySesState, CLng(fAirRef_contact_id), CLng(fAirRef_company_journ_id), True, isDisplay, False, isJFWAFW, aclsData_Temp, crmSource))
                                        temp_contact_info = (GetContactInfoCompany_No_Query(dalTable, y, MySesState, CLng(fAirRef_contact_id), CLng(fAirRef_company_journ_id), True, isDisplay, False, isJFWAFW, aclsData_Temp, crmSource))
                                    Else
                                        temp_contact_info = (GetContactInfoCompany_No_Query(dalTable, y, MySesState, CLng(fAirRef_contact_id), CLng(fAirRef_company_journ_id), False, isDisplay, False, isJFWAFW, aclsData_Temp, crmSource))
                                    End If

                                    If is_multi_contact = True And fAirRef_contact_id > 0 Then
                                        If InStr(contact_info, "</b>|<br/>" & temp_contact_info) > 0 Then
                                            contact_info = Replace(contact_info, "</b>|<br/>" & temp_contact_info, ", " & Trim(fAirContactType_name) & "</b>|<br/>" & temp_contact_info)
                                        ElseIf InStr(temp_contact_info, "/Chief Pilot") > 0 Then
                                            temp_contact_info = Replace(temp_contact_info, "/Chief Pilot", "")
                                            contact_info = Replace(contact_info, "</b>|<br/>" & temp_contact_info, ", " & Trim(fAirContactType_name) & "</b>|<br/>" & temp_contact_info)
                                        End If
                                    End If

                                    ' this contact id is in our list but we already processed it
                                    contact_info &= ("</td><td valign='top' width=""50%"">&nbsp;</td></tr>")
                                ElseIf CLng(fAirRef_contact_id) = 0 Or String.IsNullOrEmpty(fAirRef_contact_id) Then
                                    ' this contact id is zero or blank add a filler table item
                                    contact_info &= ("</td><td valign='top' width=""50%"">&nbsp;</td></tr>")
                                End If

                                If String.IsNullOrEmpty(tmpID) Then
                                    tmpID = fAirRef_contact_id
                                Else
                                    tmpID = tmpID + Constants.cCommaDelim + fAirRef_contact_id.ToString
                                End If

                                nContactCount += 1

                            End If
                        Next ' y As Integer = 0 To dalTable.Rows.count - 1

                        ' make sure u replace any leftover | if its a single contact 
                        contact_info = Replace(contact_info, "|", "")

                        If exclude_brokers = True And fAirRef_contact_type = "99" Then
                        Else
                            If Not String.IsNullOrEmpty(strCompanyTypeName) Then
                                strCompanyTypeName = strCompanyTypeName.TrimEnd(strCompanyTypeName, ", ")
                            Else
                                strCompanyTypeName = "Additional Company"
                            End If
                            strCompanyTypeName = "<h4 class='company_title'>" & strCompanyTypeName
                            ' we have to span at least one row (even if zero contacts are found)
                            Dim pageLink As String = ""
                            If crmSource = "CLIENT" Then
                                If in_AircraftID > 0 And journalID = 0 Then
                                    pageLink = "<span class=""float_right""><a class=""tiny_text"" title=""Remove Client Reference"" href=""javascript:void(0)"" onclick=""javascript:load('/edit.aspx?from=aircraftDetails&action=reference&remove=true&listing=3&id=" & fAirRef_id & "','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');"">remove</a>"
                                End If
                            End If

                            strCompanyTypeName += pageLink & "</h4>"

                            If nContactCount = 0 Then
                                htmlOut.Append("<table width='100%' cellpadding='3' cellspacing='0' border='0' class=" & class_color & ">")
                                htmlOut.Append("<tr><td valign='top' align='left' colspan='2'>" & strCompanyTypeName & "</td></tr>")
                                htmlOut.Append("<tr><td valign='top' align='right' rowspan='1'  width='50%'>")
                            Else
                                htmlOut.Append("<table width='100%' cellpadding='3' cellspacing='0' border='0' class=" & class_color & ">")
                                htmlOut.Append("<tr><td valign='top' align='left' colspan='2'>" & strCompanyTypeName & "</td></tr>")
                                htmlOut.Append("<tr><td valign='top' align='left' rowspan='" + nContactCount.ToString + "'  width='50%'>")
                            End If
                        End If
                        strCompanyTypeName = ""

                        'If showFullContact Then
                        '  htmlOut.Append(GetCompanyInfo_DisplayAircraft(MySesState, fAirRef_company_id, CLng(in_AircraftRs.Rows(0).Item("ac_journ_id").ToString), strCompanyTypeName, isDisplay, True, isJFWAFW, aclsData_Temp, crmSource))
                        'Else 
                        '  htmlOut.Append(GetCompanyInfo_DisplayAircraft(MySesState, fAirRef_company_id, CLng(in_AircraftRs.Rows(0).Item("ac_journ_id").ToString), strCompanyTypeName, isDisplay, True, isJFWAFW, aclsData_Temp, crmSource))
                        'End If
                        htmlOut.Append(comp_info)
                        comp_info = ""
                        If crmSource = "CLIENT" Then
                            htmlOut.Append(Contact_ProcessingDisplayAircraft(MySesState, CLng(in_AircraftID), CLng(in_AircraftRs.Rows(0).Item("ac_journ_id").ToString), fAirRef_company_id, strCompanyTypeNbr, isDisplay, isJFWAFW, aclsData_Temp, crmSource, otherID))
                        Else
                            htmlOut.Append(contact_info)
                            contact_info = ""
                            tmpID = ""
                            ' htmlOut.Append(Contact_ProcessingDisplayAircraft(MySesState, CLng(in_AircraftRs.Rows(0).Item("ac_id").ToString), CLng(in_AircraftRs.Rows(0).Item("ac_journ_id").ToString), fAirRef_company_id, strCompanyTypeNbr, isDisplay, isJFWAFW, aclsData_Temp, crmSource))
                        End If

                        If share <> "" Then
                            htmlOut.Append("<tr><td valign='top' colspan='3'><span class='tiny_text'>" & share & "</span></td></tr>")
                        End If
                        htmlOut.Append("</table>")

                    End If ' daltable.Rows.Count > 0 Then

                Next ' For x As Integer = 0 To dsAircraftCompanies.Tables(0).Rows.Count - 1

            End If ' dsAircraftCompanies.Tables(0).Rows.Count > 0 Then

        Else
            htmlOut.Append("<tr><td valign='middle' align='center' colspan='2'> No Company or Contact Listed for Aircraft </td></tr>")
        End If '  Not IsNothing(dsAircraftCompanies) Then

        htmlOut.Append("</td></tr>")
        htmlOut.Append("</table></div>")


        Return htmlOut.ToString.Trim

    End Function


    Public Shared Function GetExclusiveDate(ByVal in_AircraftID As Long,
                                        ByVal in_AircraftJournalID As Long, ByVal aclsData_Temp As clsData_Manager_SQL, Optional ByVal exclusive_comp_id As Long = 0) As String
        Dim compTable As New DataTable
        Dim refTable As New DataTable
        Dim htmlOut As StringBuilder = New StringBuilder()
        Dim sQuery As String = ""
        Dim nGetExclusiveCompID As Long = 0


        If in_AircraftJournalID = 0 Then

            refTable = aclsData_Temp.GetExclusiveBrokerDateJournal(in_AircraftID, exclusive_comp_id)

            If Not IsNothing(refTable) Then

                If refTable.Rows.Count > 0 Then
                    If Not IsDBNull(refTable.Rows(0).Item("journ_date")) Then
                        If Not String.IsNullOrEmpty(refTable.Rows(0).Item("journ_date").ToString) Then
                            htmlOut.Append(FormatDateTime(refTable.Rows(0).Item("journ_date").ToString, DateFormat.ShortDate))
                        End If
                    End If
                End If

            End If
        Else

            compTable = aclsData_Temp.Get_EvoAC_RelationshipByType(in_AircraftID, in_AircraftJournalID, "'93','98','99'")

            If Not IsNothing(compTable) Then
                If compTable.Rows.Count > 0 Then

                    If Not IsDBNull(compTable.Rows(0).Item("comp_id")) Then
                        If Not String.IsNullOrEmpty(compTable.Rows(0).Item("comp_id").ToString) Then
                            nGetExclusiveCompID = CLng(compTable.Rows(0).Item("comp_id").ToString)
                        End If
                    End If
                End If
            End If

            If nGetExclusiveCompID > 0 Then

                refTable = aclsData_Temp.GetExclusiveBrokerDateJournal(in_AircraftID, nGetExclusiveCompID)

                If Not IsNothing(refTable) Then

                    If refTable.Rows.Count > 0 Then

                        If Not IsDBNull(refTable.Rows(0).Item("journ_date")) Then
                            If Not String.IsNullOrEmpty(refTable.Rows(0).Item("journ_date").ToString) Then
                                htmlOut.Append(FormatDateTime(refTable.Rows(0).Item("journ_date").ToString, DateFormat.ShortDate))
                            End If
                        End If

                    End If
                End If

            End If '  nGetExclusiveCompID > 0 Then

        End If

        Return htmlOut.ToString.Trim

    End Function

    Public Shared Function Contact_ProcessingDisplayAircraft(ByRef MySesState As HttpSessionState,
                                                         ByVal nAircraftID As Long,
                                                         ByVal nAircraftJournalID As Long,
                                                         ByVal nCompanyID As Long,
                                                         ByVal inStrCompanyTypeNbr As String,
                                                         ByVal isDisplay As Boolean,
                                                         ByVal isJFWAFW As Boolean, ByVal aclsData_Temp As clsData_Manager_SQL, ByRef crmSource As String, ByVal otherID As Long) As String

        Dim ReferenceTable As New DataTable
        Dim htmlOut As StringBuilder = New StringBuilder()
        Dim sQuery As String = ""
        Dim tmpID As String = ""


        If crmSource = "CLIENT" And nAircraftJournalID = 0 Then
            ReferenceTable = clsGeneral.clsGeneral.Get_ContactReferences_Client_ACDetails(nAircraftID, nCompanyID, inStrCompanyTypeNbr, HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag)
        ElseIf crmSource = "CLIENT" And nAircraftJournalID > 0 Then
            ReferenceTable = aclsData_Temp.Get_ContactReferences_Evo_AC_Details(otherID, nAircraftJournalID, nCompanyID, inStrCompanyTypeNbr, HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag)
        Else
            ReferenceTable = aclsData_Temp.Get_ContactReferences_Evo_AC_Details(nAircraftID, nAircraftJournalID, nCompanyID, inStrCompanyTypeNbr, HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag)
        End If

        If Not IsNothing(ReferenceTable) Then

            If ReferenceTable.Rows.Count > 0 Then
                For Each r As DataRow In ReferenceTable.Rows
                    If Not tmpID.Contains(r("cref_contact_id").ToString.Trim) Then

                        htmlOut.Append("</td><td valign='top' align='left'>")

                        If aclsData_Temp.IsChiefPilot(nAircraftID, nAircraftJournalID, r("cref_contact_id")) Then
                            htmlOut.Append(GetContactInfoCompany(MySesState, CLng(r("cref_contact_id").ToString), nAircraftJournalID, True, isDisplay, False, isJFWAFW, aclsData_Temp, crmSource))
                        Else
                            htmlOut.Append(GetContactInfoCompany(MySesState, CLng(r("cref_contact_id").ToString), nAircraftJournalID, False, isDisplay, False, isJFWAFW, aclsData_Temp, crmSource))
                        End If

                        htmlOut.Append("</td></tr>")

                    ElseIf CLng(r("cref_contact_id").ToString) > 0 Then
                        ' this contact id is in our list but we already processed it
                        htmlOut.Append("</td><td valign='top'>&nbsp;</td></tr>")
                    ElseIf CLng(r("cref_contact_id").ToString) = 0 Or String.IsNullOrEmpty(r("cref_contact_id").ToString) Then
                        ' this contact id is zero or blank add a filler table item
                        htmlOut.Append("</td><td valign='top'>&nbsp;</td></tr>")
                    End If

                    If String.IsNullOrEmpty(tmpID) Then
                        tmpID = r("cref_contact_id").ToString.Trim
                    Else
                        tmpID = tmpID + Constants.cCommaDelim + r("cref_contact_id").ToString.Trim
                    End If
                Next
            End If

        Else
            htmlOut.Append("</td><td valign='top'>&nbsp;</td></tr>")
        End If


        Return htmlOut.ToString.Trim

    End Function

    Public Shared Function GetContactInfoCompany_No_Query(ByVal ResultsTable As DataTable, ByVal row_num As Integer, ByRef MySesState As HttpSessionState,
                                             ByVal inContactID As Long,
                                             ByVal inJournalID As Long,
                                             ByVal isChiefPilot As Boolean,
                                             ByVal isDisplay As Boolean,
                                             ByVal isDetails As Boolean,
                                             ByVal isJFWAFW As Boolean, ByVal aclsData_Temp As clsData_Manager_SQL, ByRef crmSource As String) As String

        Dim htmlOut As StringBuilder = New StringBuilder()
        Dim sQuery As String = ""


        If isDisplay And Not (isDetails) Then
            If crmSource = "CLIENT" Then
                htmlOut.Append("<a " + DisplayFunctions.WriteDetailsLink(0, ResultsTable.Rows(row_num).Item("comp_id"), inContactID, inJournalID, False, "", "", "&SOURCE=CLIENT") & "><b>" & ResultsTable.Rows(row_num).Item("contact_sirname").ToString.Trim + " " + ResultsTable.Rows(row_num).Item("contact_first_name").ToString.Trim + Constants.cSingleSpace)
            Else
                htmlOut.Append("<a " + DisplayFunctions.WriteDetailsLink(0, ResultsTable.Rows(row_num).Item("contact_comp_id"), inContactID, inJournalID, False, "", "", "") & "><b>" & ResultsTable.Rows(row_num).Item("contact_sirname").ToString.Trim + " " + ResultsTable.Rows(row_num).Item("contact_first_name").ToString.Trim + Constants.cSingleSpace)
            End If
        Else
            htmlOut.Append(ResultsTable.Rows(row_num).Item("contact_sirname").ToString.Trim + Constants.cSingleSpace + ResultsTable.Rows(row_num).Item("contact_first_name").ToString.Trim + Constants.cSingleSpace)
        End If

        If Not IsDBNull(ResultsTable.Rows(row_num).Item("contact_middle_initial")) And Not String.IsNullOrEmpty(ResultsTable.Rows(row_num).Item("contact_suffix").ToString) Then
            htmlOut.Append(ResultsTable.Rows(row_num).Item("contact_middle_initial").ToString.Trim + ". ")
        End If

        If isDisplay And Not (isDetails) Then

            If Not (IsDBNull(ResultsTable.Rows(row_num).Item("contact_last_name"))) And Not String.IsNullOrEmpty(ResultsTable.Rows(row_num).Item("contact_last_name").ToString) Then
                htmlOut.Append(ResultsTable.Rows(row_num).Item("contact_last_name").ToString.Trim)
            End If

            If Not IsDBNull(ResultsTable.Rows(row_num).Item("contact_suffix")) And Not String.IsNullOrEmpty(ResultsTable.Rows(row_num).Item("contact_suffix").ToString) Then
                htmlOut.Append(Constants.cSingleSpace + ResultsTable.Rows(row_num).Item("contact_suffix").ToString.Trim)
            End If

            htmlOut.Append("</b></a><br />")

        Else

            If Not (IsDBNull(ResultsTable.Rows(row_num).Item("contact_last_name"))) And Not String.IsNullOrEmpty(ResultsTable.Rows(row_num).Item("contact_last_name").ToString) Then
                htmlOut.Append(ResultsTable.Rows(row_num).Item("contact_last_name").ToString.Trim)
            End If

            If Not (IsDBNull(ResultsTable.Rows(row_num).Item("contact_suffix"))) And Not String.IsNullOrEmpty(ResultsTable.Rows(row_num).Item("contact_suffix").ToString) Then
                htmlOut.Append(Constants.cSingleSpace + ResultsTable.Rows(row_num).Item("contact_suffix").ToString.Trim)
            End If

            htmlOut.Append("<br />")

        End If

        If Not (IsDBNull(ResultsTable.Rows(row_num).Item("contact_title"))) And Not String.IsNullOrEmpty(ResultsTable.Rows(row_num).Item("contact_title").ToString) Then

            If isChiefPilot And Not ResultsTable.Rows(row_num).Item("contact_title").ToString.ToLower.Contains(LCase("Chief Pilot")) Then
                htmlOut.Append(ResultsTable.Rows(row_num).Item("contact_title").ToString.Trim + "/Chief Pilot<br />")
            Else
                htmlOut.Append(ResultsTable.Rows(row_num).Item("contact_title").ToString.Trim + "<br />")
            End If

        End If

        If Not (IsDBNull(ResultsTable.Rows(row_num).Item("contact_email_address"))) And Not String.IsNullOrEmpty(ResultsTable.Rows(row_num).Item("contact_email_address").ToString) Then
            If isDisplay And Not (isDetails) Then
                htmlOut.Append("<a href='mailto:" + ResultsTable.Rows(row_num).Item("contact_email_address").ToString.Trim + "'  style='text-decoration: none'><font color='#25517d'>" + TrimAndTitleString(ResultsTable.Rows(row_num).Item("contact_email_address").ToString.Trim, 24) + "</font></a><br />")
            Else
                If isJFWAFW Then
                    htmlOut.Append("<a href='mailto:" + ResultsTable.Rows(row_num).Item("contact_email_address").ToString.Trim + "'>" + TrimAndTitleString(ResultsTable.Rows(row_num).Item("contact_email_address").ToString.Trim, 24) + "</a><br />")
                Else
                    htmlOut.Append(TrimAndTitleString(ResultsTable.Rows(row_num).Item("contact_email_address").ToString.Trim, 24) + "<br />")
                End If
            End If
        End If


        If Not (IsDBNull(ResultsTable.Rows(row_num).Item("contact_phone_office"))) Then
            If Trim(ResultsTable.Rows(row_num).Item("contact_phone_office")) <> "" Then
                htmlOut.Append("<span class=""make-tel-link"">Office : ")
                htmlOut.Append(ResultsTable.Rows(row_num).Item("contact_phone_office").ToString.Trim + "</span><br />")
            End If
        End If

        If Not (IsDBNull(ResultsTable.Rows(row_num).Item("contact_phone_mobile"))) Then
            If Trim(ResultsTable.Rows(row_num).Item("contact_phone_mobile")) <> "" Then
                htmlOut.Append("<span class=""make-tel-link"">Mobile : ")
                htmlOut.Append(ResultsTable.Rows(row_num).Item("contact_phone_mobile").ToString.Trim + "</span><br />")
            End If
        End If

        If Not (IsDBNull(ResultsTable.Rows(row_num).Item("contact_phone_fax"))) Then
            If Trim(ResultsTable.Rows(row_num).Item("contact_phone_fax")) <> "" Then
                htmlOut.Append("<span class=""make-tel-link"">Fax : ")
                htmlOut.Append(ResultsTable.Rows(row_num).Item("contact_phone_fax").ToString.Trim + "</span><br />")
            End If
        End If



        Return htmlOut.ToString.Trim

    End Function

    Public Shared Function GetContactInfoCompany(ByRef MySesState As HttpSessionState,
                                             ByVal inContactID As Long,
                                             ByVal inJournalID As Long,
                                             ByVal isChiefPilot As Boolean,
                                             ByVal isDisplay As Boolean,
                                             ByVal isDetails As Boolean,
                                             ByVal isJFWAFW As Boolean, ByVal aclsData_Temp As clsData_Manager_SQL, ByRef crmSource As String) As String
        Dim PhoneTable As New DataTable
        Dim ResultsTable As New DataTable
        Dim htmlOut As StringBuilder = New StringBuilder()
        Dim sQuery As String = ""


        If crmSource = "CLIENT" And inJournalID = 0 Then
            ResultsTable = clsGeneral.clsGeneral.Get_Aircraft_Contact_As_JetnetFields(inContactID)
        ElseIf crmSource = "CLIENT" And inJournalID > 0 Then
            ResultsTable = aclsData_Temp.ReturnContactInformationACDetails(inJournalID, inContactID)
        Else
            ResultsTable = aclsData_Temp.ReturnContactInformationACDetails(inJournalID, inContactID)
        End If

        If Not IsNothing(ResultsTable) Then

            If ResultsTable.Rows.Count Then

                If isDisplay And Not (isDetails) Then
                    If crmSource = "CLIENT" Then
                        htmlOut.Append("<a " + DisplayFunctions.WriteDetailsLink(0, ResultsTable.Rows(0).Item("contact_comp_id"), ResultsTable.Rows(0).Item("contact_id"), inJournalID, False, "", "", "&source=CLIENT") & ">" & ResultsTable.Rows(0).Item("contact_sirname").ToString.Trim + " " + ResultsTable.Rows(0).Item("contact_first_name").ToString.Trim + Constants.cSingleSpace)
                    Else
                        htmlOut.Append("<a " + DisplayFunctions.WriteDetailsLink(0, ResultsTable.Rows(0).Item("contact_comp_id"), inContactID, inJournalID, False, "", "", "") & ">" & ResultsTable.Rows(0).Item("contact_sirname").ToString.Trim + " " + ResultsTable.Rows(0).Item("contact_first_name").ToString.Trim + Constants.cSingleSpace)
                    End If
                Else
                    htmlOut.Append(ResultsTable.Rows(0).Item("contact_sirname").ToString.Trim + Constants.cSingleSpace + ResultsTable.Rows(0).Item("contact_first_name").ToString.Trim + Constants.cSingleSpace)
                End If

                If Not IsDBNull(ResultsTable.Rows(0).Item("contact_middle_initial")) And Not String.IsNullOrEmpty(ResultsTable.Rows(0).Item("contact_suffix").ToString) Then
                    htmlOut.Append(ResultsTable.Rows(0).Item("contact_middle_initial").ToString.Trim + ". ")
                End If

                If isDisplay And Not (isDetails) Then

                    If Not (IsDBNull(ResultsTable.Rows(0).Item("contact_last_name"))) And Not String.IsNullOrEmpty(ResultsTable.Rows(0).Item("contact_last_name").ToString) Then
                        htmlOut.Append(ResultsTable.Rows(0).Item("contact_last_name").ToString.Trim)
                    End If

                    If Not IsDBNull(ResultsTable.Rows(0).Item("contact_suffix")) And Not String.IsNullOrEmpty(ResultsTable.Rows(0).Item("contact_suffix").ToString) Then
                        htmlOut.Append(Constants.cSingleSpace + ResultsTable.Rows(0).Item("contact_suffix").ToString.Trim)
                    End If

                    htmlOut.Append("</a><br />")

                Else

                    If Not (IsDBNull(ResultsTable.Rows(0).Item("contact_last_name"))) And Not String.IsNullOrEmpty(ResultsTable.Rows(0).Item("contact_last_name").ToString) Then
                        htmlOut.Append(ResultsTable.Rows(0).Item("contact_last_name").ToString.Trim)
                    End If

                    If Not (IsDBNull(ResultsTable.Rows(0).Item("contact_suffix"))) And Not String.IsNullOrEmpty(ResultsTable.Rows(0).Item("contact_suffix").ToString) Then
                        htmlOut.Append(Constants.cSingleSpace + ResultsTable.Rows(0).Item("contact_suffix").ToString.Trim)
                    End If

                    htmlOut.Append("<br />")

                End If

                If Not (IsDBNull(ResultsTable.Rows(0).Item("contact_title"))) And Not String.IsNullOrEmpty(ResultsTable.Rows(0).Item("contact_title").ToString) Then

                    If isChiefPilot And Not ResultsTable.Rows(0).Item("contact_title").ToString.ToLower.Contains(LCase("Chief Pilot")) Then
                        htmlOut.Append(ResultsTable.Rows(0).Item("contact_title").ToString.Trim + "/Chief Pilot<br />")
                    Else
                        htmlOut.Append(ResultsTable.Rows(0).Item("contact_title").ToString.Trim + "<br />")
                    End If

                End If

                If Not (IsDBNull(ResultsTable.Rows(0).Item("contact_email_address"))) And Not String.IsNullOrEmpty(ResultsTable.Rows(0).Item("contact_email_address").ToString) Then
                    If isDisplay And Not (isDetails) Then
                        htmlOut.Append("<a href='mailto:" + ResultsTable.Rows(0).Item("contact_email_address").ToString.Trim + "'>" + ResultsTable.Rows(0).Item("contact_email_address").ToString.Trim + "</a><br />")
                    Else
                        If isJFWAFW Then
                            htmlOut.Append("<a href='mailto:" + ResultsTable.Rows(0).Item("contact_email_address").ToString.Trim + "'>" + ResultsTable.Rows(0).Item("contact_email_address").ToString.Trim + "</a><br />")
                        Else
                            htmlOut.Append(ResultsTable.Rows(0).Item("contact_email_address").ToString.Trim + "<br />")
                        End If
                    End If
                End If


                PhoneTable = aclsData_Temp.Get_All_JETNET_PhoneNbrs_compID(ResultsTable.Rows(0).Item("contact_comp_id"), ResultsTable.Rows(0).Item("contact_journ_id"), ResultsTable.Rows(0).Item("contact_id"))
                If Not IsNothing(PhoneTable) Then
                    If PhoneTable.Rows.Count > 0 Then
                        For Each r As DataRow In PhoneTable.Rows
                            If Not IsDBNull(r("pnum_type")) And Not String.IsNullOrEmpty(r("pnum_type").ToString) Then
                                htmlOut.Append(r("pnum_type").ToString.Trim + " : ")
                            End If

                            If Not IsDBNull(r("pnum_number_full")) And Not String.IsNullOrEmpty(r("pnum_number_full").ToString) Then
                                htmlOut.Append(r("pnum_number_full").ToString.Trim + "<br />")
                            End If
                        Next
                    End If
                End If
                PhoneTable = New DataTable

            Else
                htmlOut.Append("&nbsp;")
            End If
        End If

        Return htmlOut.ToString.Trim

    End Function

    Public Shared Function GetCompanyInfo_DisplayAircraft_No_Query(ByVal lData As DataTable, ByVal row_num As Integer, ByRef MySesState As HttpSessionState,
                                                        ByVal inCompanyID As Long,
                                                        ByVal inJournalID As Long,
                                                        ByVal inTypes As String,
                                                        ByVal isDisplay As Boolean,
                                                        ByVal b_showPhone As Boolean,
                                                        ByVal isJFWAFW As Boolean, ByVal aclsData_Temp As clsData_Manager_SQL, ByRef crmSource As String) As String

        Dim htmlOut As StringBuilder = New StringBuilder()
        Dim sQuery As String = ""
        Dim bHadCity As Boolean = False
        Dim PhoneTable As New DataTable


        If Not IsNothing(lData) Then
            If lData.Rows.Count > 0 Then

                ' If Not String.IsNullOrEmpty(inTypes) Then
                '   inTypes = inTypes.Replace(Constants.cCommaDelim, Constants.cSingleForwardSlash)
                '   htmlOut.Append(inTypes.Replace("Additional Contact1", "/Additional Company") + " - ")
                ' End If

                Dim sCompanyName As String = ""
                If Not HttpContext.Current.Session.Item("isMobile") = True Then
                    sCompanyName = TrimAndTitleString(lData.Rows(row_num).Item("comp_name").ToString, 24)
                Else
                    sCompanyName = TrimAndTitleString(lData.Rows(row_num).Item("comp_name").ToString.Replace(Constants.cSingleSpace, Constants.cHTMLnbsp), 24)
                End If

                If isDisplay Then
                    htmlOut.Append(DisplayFunctions.WriteDetailsLink(0, lData.Rows(row_num).Item("comp_id"), 0, inJournalID, True, sCompanyName, "underline", IIf(crmSource = "CLIENT", "&source=CLIENT", "")))
                    htmlOut.Append("<br />")
                Else
                    htmlOut.Append(sCompanyName + "<br />")
                End If

                If Not IsDBNull(lData.Rows(row_num).Item("comp_name_alt_type")) And Not IsDBNull(lData.Rows(row_num).Item("comp_name_alt")) Then
                    If Not String.IsNullOrEmpty(lData.Rows(row_num).Item("comp_name_alt_type").ToString) And Not String.IsNullOrEmpty(lData.Rows(row_num).Item("comp_name_alt").ToString) Then
                        htmlOut.Append(lData.Rows(row_num).Item("comp_name_alt_type").ToString.Trim + Constants.cSingleSpace + lData.Rows(row_num).Item("comp_name_alt").ToString.Trim + "<br />")
                    End If
                Else
                    If Not IsDBNull(lData.Rows(row_num).Item("comp_name_alt")) And Not String.IsNullOrEmpty(lData.Rows(row_num).Item("comp_name_alt").ToString) Then
                        htmlOut.Append(lData.Rows(row_num).Item("comp_name_alt").ToString.Trim + "<br />")
                    End If
                End If

                If Not IsDBNull(lData.Rows(row_num).Item("comp_address1")) And Not String.IsNullOrEmpty(lData.Rows(row_num).Item("comp_address1").ToString) Then
                    htmlOut.Append(lData.Rows(row_num).Item("comp_address1").ToString.Trim + "<br />")
                End If

                If Not IsDBNull(lData.Rows(row_num).Item("comp_address2")) And Not String.IsNullOrEmpty(lData.Rows(row_num).Item("comp_address2").ToString) Then
                    htmlOut.Append(lData.Rows(row_num).Item("comp_address2").ToString.Trim + "<br />")
                End If

                If Not IsDBNull(lData.Rows(row_num).Item("comp_city")) And Not String.IsNullOrEmpty(lData.Rows(row_num).Item("comp_city").ToString) Then
                    htmlOut.Append(lData.Rows(row_num).Item("comp_city").ToString.Trim)
                    bHadCity = True
                End If

                If Not IsDBNull(lData.Rows(row_num).Item("comp_state")) And Not String.IsNullOrEmpty(lData.Rows(row_num).Item("comp_state").ToString) Then
                    If bHadCity Then
                        htmlOut.Append(Constants.cMultiDelim + lData.Rows(row_num).Item("comp_state").ToString.Trim)
                    Else
                        htmlOut.Append(lData.Rows(row_num).Item("comp_state").ToString.Trim)
                    End If
                End If

                If Not IsDBNull(lData.Rows(row_num).Item("comp_zip_code")) And Not String.IsNullOrEmpty(lData.Rows(row_num).Item("comp_zip_code").ToString) Then
                    htmlOut.Append("&nbsp;" + lData.Rows(row_num).Item("comp_zip_code").ToString.Trim)
                End If

                If Not IsDBNull(lData.Rows(row_num).Item("comp_country")) And Not String.IsNullOrEmpty(lData.Rows(row_num).Item("comp_country").ToString) Then
                    htmlOut.Append("&nbsp;" + Replace(lData.Rows(row_num).Item("comp_country").ToString.Trim, "United States", "US"))
                End If

                htmlOut.Append("<br />")

                If Not IsDBNull(lData.Rows(row_num).Item("comp_email_address")) And Not String.IsNullOrEmpty(lData.Rows(row_num).Item("comp_email_address").ToString) Then
                    If isDisplay Then
                        htmlOut.Append("<a href='mailto:" + lData.Rows(row_num).Item("comp_email_address").ToString.Trim + "'   style='text-decoration: none'><font color='#25517d'>" + TrimAndTitleString(lData.Rows(row_num).Item("comp_email_address").ToString.Trim, 24) + "</font></a><br />")
                    Else
                        If isJFWAFW Then
                            htmlOut.Append("<a href='mailto:" + lData.Rows(row_num).Item("comp_email_address").ToString.Trim + "'   style='text-decoration: none'><font color='#25517d'>" + TrimAndTitleString(lData.Rows(row_num).Item("comp_email_address").ToString.Trim, 24) + "</font></a><br />")
                        Else
                            htmlOut.Append(TrimAndTitleString(lData.Rows(row_num).Item("comp_email_address").ToString.Trim, 24) + "<br />")
                        End If
                    End If
                End If

                If Not IsDBNull(lData.Rows(row_num).Item("comp_web_address")) And Not String.IsNullOrEmpty(lData.Rows(row_num).Item("comp_web_address").ToString) Then
                    If isDisplay Then
                        htmlOut.Append("<a href='http://" + lData.Rows(row_num).Item("comp_web_address").ToString.Trim + "' target=_new style='text-decoration: none'><font color='#25517d'>" + TrimAndTitleString(lData.Rows(row_num).Item("comp_web_address").ToString.Trim, 24) + "</font></a><br />")
                    Else
                        If isJFWAFW Then
                            htmlOut.Append("<a href='http://" + lData.Rows(row_num).Item("comp_web_address").ToString.Trim + "' target=_new style='text-decoration: none'><font color='#25517d'>" + TrimAndTitleString(lData.Rows(row_num).Item("comp_web_address").ToString.Trim, 24) + "</font></a><br />")
                        Else
                            htmlOut.Append(TrimAndTitleString(lData.Rows(row_num).Item("comp_web_address").ToString.Trim, 24) + "<br />")
                        End If
                    End If
                End If

                If b_showPhone Then

                    If crmSource = "CLIENT" Then
                        PhoneTable = clsGeneral.clsGeneral.Get_Client_Phone_As_JetnetFields(lData.Rows(row_num).Item("comp_id"), 0)

                        ' ADDED IN MSW - WAS NOT SHOWING PHONE NUMBERS ON CLIENT DATA EVEN THOUGH IT WAS FINDING THEM

                        Try
                            If Not IsNothing(PhoneTable) Then
                                If PhoneTable.Rows.Count > 0 Then
                                    For Each r As DataRow In PhoneTable.Rows

                                        If Not (IsDBNull(r.Item("pnum_number_full"))) Then
                                            If Trim(r.Item("pnum_type")) = "Office" Or Trim(r.Item("pnum_type")) = "Mobile" Or Trim(r.Item("pnum_type")) = "Fax" Then
                                                htmlOut.Append("<span class=""make-tel-link"">" & Trim(r.Item("pnum_type")) & " : ")
                                                htmlOut.Append(r.Item("pnum_number_full").ToString.Trim + "</span><br />")
                                            End If
                                        End If

                                    Next
                                End If
                            End If
                        Catch ex As Exception
                        Finally
                            PhoneTable = Nothing
                        End Try

                    Else

                        ' commented back in MSW - 3/29/19
                        PhoneTable = aclsData_Temp.Get_All_JETNET_PhoneNbrs_compID(lData.Rows(row_num).Item("comp_id"), lData.Rows(row_num).Item("comp_journ_id"), 0)

                        Try
                            If Not IsNothing(PhoneTable) Then
                                If PhoneTable.Rows.Count > 0 Then
                                    For Each r As DataRow In PhoneTable.Rows

                                        If Not (IsDBNull(r.Item("pnum_number_full"))) Then
                                            ' If Trim(r.Item("pnum_type")) = "Office" Or Trim(r.Item("pnum_type")) = "Mobile" Or Trim(r.Item("pnum_type")) = "Fax" Then
                                            htmlOut.Append("<span class=""make-tel-link"">" & Trim(r.Item("pnum_type")) & " : ")
                                            htmlOut.Append(r.Item("pnum_number_full").ToString.Trim + "</span><br />")
                                            'End If
                                        End If

                                    Next
                                End If
                            End If
                        Catch ex As Exception
                        Finally
                            PhoneTable = Nothing
                        End Try
                        'If Not (IsDBNull(lData.Rows(row_num).Item("comp_phone_office"))) Then
                        '  If Trim(lData.Rows(row_num).Item("comp_phone_office")) <> "" Then
                        '    htmlOut.Append("<span class=""make-tel-link"">Office : ")
                        '    htmlOut.Append(lData.Rows(row_num).Item("comp_phone_office").ToString.Trim + "</span><br />")
                        '  End If
                        'End If

                        'If Not (IsDBNull(lData.Rows(row_num).Item("comp_phone_mobile"))) Then
                        '  If Trim(lData.Rows(row_num).Item("comp_phone_mobile")) <> "" Then
                        '    htmlOut.Append("<span class=""make-tel-link"">Mobile : ")
                        '    htmlOut.Append(lData.Rows(row_num).Item("comp_phone_mobile").ToString.Trim + "</span><br />")
                        '  End If
                        'End If

                        'If Not (IsDBNull(lData.Rows(row_num).Item("comp_phone_fax"))) Then
                        '  If Trim(lData.Rows(row_num).Item("comp_phone_fax")) <> "" Then
                        '    htmlOut.Append("<span class=""make-tel-link"">Fax : ")
                        '    htmlOut.Append(lData.Rows(row_num).Item("comp_phone_fax").ToString.Trim + "</span><br />")
                        '  End If
                        'End If

                    End If

                    PhoneTable = New DataTable
                End If

                ' MSW - TO BE ADDED BACK IN 
                'If Not IsDBNull(lData.Rows(row_num).Item("comp_fractowr_notes")) And Not String.IsNullOrEmpty(lData.Rows(row_num).Item("comp_fractowr_notes").ToString) Then
                '  htmlOut.Append(lData.Rows(row_num).Item("comp_fractowr_notes").ToString.Trim + "<br />")
                'End If

            End If

        End If


        Return htmlOut.ToString.Trim

    End Function
    ''' <summary>
    ''' Imported pretty much as is from Evo. This displays the company information.
    ''' </summary>
    ''' <param name="MySesState"></param>
    ''' <param name="inCompanyID"></param>
    ''' <param name="inJournalID"></param>
    ''' <param name="inTypes"></param>
    ''' <param name="isDisplay"></param>
    ''' <param name="b_showPhone"></param>
    ''' <param name="isJFWAFW"></param>
    ''' <param name="aclsData_Temp"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetCompanyInfo_DisplayAircraft(ByRef MySesState As HttpSessionState,
                                                        ByVal inCompanyID As Long,
                                                        ByVal inJournalID As Long,
                                                        ByVal inTypes As String,
                                                        ByVal isDisplay As Boolean,
                                                        ByVal b_showPhone As Boolean,
                                                        ByVal isJFWAFW As Boolean, ByVal aclsData_Temp As clsData_Manager_SQL, ByRef crmSource As String) As String

        Dim lData As New DataTable
        Dim htmlOut As StringBuilder = New StringBuilder()
        Dim sQuery As String = ""
        Dim bHadCity As Boolean = False
        Dim PhoneTable As New DataTable

        If crmSource = "CLIENT" Then
            lData = clsGeneral.clsGeneral.Get_Aircraft_Company_As_JetnetFields(inCompanyID)
        Else
            lData = aclsData_Temp.Get_Company_Information_Evo_AC_Details_References(inCompanyID, inJournalID, False)
        End If



        If Not IsNothing(lData) Then
            If lData.Rows.Count > 0 Then

                If Not String.IsNullOrEmpty(inTypes) Then
                    inTypes = inTypes.Replace(Constants.cCommaDelim, Constants.cSingleForwardSlash)
                    htmlOut.Append(inTypes.Replace("Additional Contact1", "/Additional Company") + " - ")
                End If

                Dim sCompanyName As String = lData.Rows(0).Item("comp_name").ToString.Replace(Constants.cSingleSpace, Constants.cHTMLnbsp)

                If isDisplay Then
                    htmlOut.Append(DisplayFunctions.WriteDetailsLink(0, lData.Rows(0).Item("comp_id"), 0, inJournalID, True, sCompanyName, "underline", ""))
                    htmlOut.Append("<br />")
                Else
                    htmlOut.Append(sCompanyName + "<br />")
                End If

                If Not IsDBNull(lData.Rows(0).Item("comp_name_alt_type")) And Not IsDBNull(lData.Rows(0).Item("comp_name_alt")) Then
                    If Not String.IsNullOrEmpty(lData.Rows(0).Item("comp_name_alt_type").ToString) And Not String.IsNullOrEmpty(lData.Rows(0).Item("comp_name_alt").ToString) Then
                        htmlOut.Append(lData.Rows(0).Item("comp_name_alt_type").ToString.Trim + Constants.cSingleSpace + lData.Rows(0).Item("comp_name_alt").ToString.Trim + "<br />")
                    End If
                Else
                    If Not IsDBNull(lData.Rows(0).Item("comp_name_alt")) And Not String.IsNullOrEmpty(lData.Rows(0).Item("comp_name_alt").ToString) Then
                        htmlOut.Append(lData.Rows(0).Item("comp_name_alt").ToString.Trim + "<br />")
                    End If
                End If

                If Not IsDBNull(lData.Rows(0).Item("comp_address1")) And Not String.IsNullOrEmpty(lData.Rows(0).Item("comp_address1").ToString) Then
                    htmlOut.Append(lData.Rows(0).Item("comp_address1").ToString.Trim + "<br />")
                End If

                If Not IsDBNull(lData.Rows(0).Item("comp_address2")) And Not String.IsNullOrEmpty(lData.Rows(0).Item("comp_address2").ToString) Then
                    htmlOut.Append(lData.Rows(0).Item("comp_address2").ToString.Trim + "<br />")
                End If

                If Not IsDBNull(lData.Rows(0).Item("comp_city")) And Not String.IsNullOrEmpty(lData.Rows(0).Item("comp_city").ToString) Then
                    htmlOut.Append(lData.Rows(0).Item("comp_city").ToString.Trim)
                    bHadCity = True
                End If

                If Not IsDBNull(lData.Rows(0).Item("comp_state")) And Not String.IsNullOrEmpty(lData.Rows(0).Item("comp_state").ToString) Then
                    If bHadCity Then
                        htmlOut.Append(Constants.cMultiDelim + lData.Rows(0).Item("comp_state").ToString.Trim)
                    Else
                        htmlOut.Append(lData.Rows(0).Item("comp_state").ToString.Trim)
                    End If
                End If

                If Not IsDBNull(lData.Rows(0).Item("comp_zip_code")) And Not String.IsNullOrEmpty(lData.Rows(0).Item("comp_zip_code").ToString) Then
                    htmlOut.Append("&nbsp;" + lData.Rows(0).Item("comp_zip_code").ToString.Trim)
                End If

                If Not IsDBNull(lData.Rows(0).Item("comp_country")) And Not String.IsNullOrEmpty(lData.Rows(0).Item("comp_country").ToString) Then
                    htmlOut.Append("&nbsp;" + lData.Rows(0).Item("comp_country").ToString.Trim)
                End If

                htmlOut.Append("<br />")

                If Not IsDBNull(lData.Rows(0).Item("comp_email_address")) And Not String.IsNullOrEmpty(lData.Rows(0).Item("comp_email_address").ToString) Then
                    If isDisplay Then
                        htmlOut.Append("<a href='mailto:" + lData.Rows(0).Item("comp_email_address").ToString.Trim + "'>" + lData.Rows(0).Item("comp_email_address").ToString.Trim + "</a><br />")
                    Else
                        If isJFWAFW Then
                            htmlOut.Append("<a href='mailto:" + lData.Rows(0).Item("comp_email_address").ToString.Trim + "'>" + lData.Rows(0).Item("comp_email_address").ToString.Trim + "</a><br />")
                        Else
                            htmlOut.Append(lData.Rows(0).Item("comp_email_address").ToString.Trim + "<br />")
                        End If
                    End If
                End If

                If Not IsDBNull(lData.Rows(0).Item("comp_web_address")) And Not String.IsNullOrEmpty(lData.Rows(0).Item("comp_web_address").ToString) Then
                    If isDisplay Then
                        htmlOut.Append("<a href='http://" + lData.Rows(0).Item("comp_web_address").ToString.Trim + "' target=_new>" + lData.Rows(0).Item("comp_web_address").ToString.Trim + "</a><br />")
                    Else
                        If isJFWAFW Then
                            htmlOut.Append("<a href='http://" + lData.Rows(0).Item("comp_web_address").ToString.Trim + "' target=_new>" + lData.Rows(0).Item("comp_web_address").ToString.Trim + "</a><br />")
                        Else
                            htmlOut.Append(lData.Rows(0).Item("comp_web_address").ToString.Trim + "<br />")
                        End If
                    End If
                End If

                If b_showPhone Then
                    If crmSource = "CLIENT" Then
                        PhoneTable = clsGeneral.clsGeneral.Get_Client_Phone_As_JetnetFields(lData.Rows(0).Item("comp_id"), 0)
                    Else
                        PhoneTable = aclsData_Temp.Get_All_JETNET_PhoneNbrs_compID(lData.Rows(0).Item("comp_id"), lData.Rows(0).Item("comp_journ_id"), 0)
                    End If

                    If Not IsNothing(PhoneTable) Then
                        If PhoneTable.Rows.Count > 0 Then
                            For Each r As DataRow In PhoneTable.Rows
                                If Not IsDBNull(r("pnum_type")) And Not String.IsNullOrEmpty(r("pnum_type").ToString) Then
                                    htmlOut.Append(r("pnum_type").ToString.Trim + " : ")
                                End If

                                If Not IsDBNull(r("pnum_number_full")) And Not String.IsNullOrEmpty(r("pnum_number_full").ToString) Then
                                    htmlOut.Append(r("pnum_number_full").ToString.Trim + "<br />")
                                End If
                            Next
                        End If
                    End If
                    PhoneTable = New DataTable
                End If

                If Not IsDBNull(lData.Rows(0).Item("comp_fractowr_notes")) And Not String.IsNullOrEmpty(lData.Rows(0).Item("comp_fractowr_notes").ToString) Then
                    htmlOut.Append(lData.Rows(0).Item("comp_fractowr_notes").ToString.Trim + "<br />")
                End If

            End If

        End If


        Return htmlOut.ToString.Trim

    End Function

    ''' <summary>
    ''' This creates a split of the company 
    ''' </summary>
    ''' <param name="inputString"></param>
    ''' <param name="sDelimiter"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function SplitUserData(ByVal inputString As String, ByVal sDelimiter As String) As String()

        Dim tArray() As String = Nothing

        Try

            If Not String.IsNullOrEmpty(inputString) Then
                tArray = Split(inputString.Trim, sDelimiter)
            End If

            If ((tArray.Length > 0) And (Not String.IsNullOrEmpty(inputString))) Then
                Return tArray
            Else
                tArray(0) = inputString
                Return tArray
            End If

        Catch ex As Exception

            Return tArray

        End Try

    End Function
    ''' <summary>
    ''' Creates acref_comp_id array
    ''' </summary>
    ''' <param name="inDataset"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Get_ReferenceCompanyIDs(ByVal inDataset As DataTable) As String

        Dim oldCompanyID As String = ""
        Dim newCompanyID As String = ""
        Dim tString As String = ""

        Try

            If Not IsNothing(inDataset) Then

                If inDataset.Rows.Count > 0 Then
                    ' build the compID's
                    For x As Integer = 0 To inDataset.Rows.Count - 1

                        newCompanyID = inDataset.Rows(x).Item("acref_comp_id").ToString

                        If newCompanyID.Trim <> oldCompanyID.Trim Then

                            If String.IsNullOrEmpty(tString) Then
                                tString = tString + "'" & inDataset.Rows(x).Item("acref_comp_id").ToString & "'"
                            Else
                                If Not tString.Contains("'" & inDataset.Rows(x).Item("acref_comp_id").ToString & "'") Then
                                    tString = tString + Constants.cCommaDelim + "'" & inDataset.Rows(x).Item("acref_comp_id").ToString & "'"
                                End If
                            End If

                            oldCompanyID = inDataset.Rows(x).Item("acref_comp_id").ToString
                        End If

                    Next

                    tString = Replace(tString, "'", "")

                Else
                    Return tString = ""
                End If
            Else
                Return tString = ""
            End If

        Catch ex As Exception
            Return tString = ""
        End Try

        Return tString

    End Function









    ''' <summary>
    ''' Used in several places on the home page, this sets up a uniform way to display aircraft information based on the link. Accepts a datatable with the ac information
    ''' already included, builds the link string then goes ahead and sends it back.
    ''' </summary>
    ''' <param name="aTempTable"></param>
    ''' <param name="ShowLink"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Display_Aircraft_Information_For_Link(ByVal aTempTable As DataTable, ByVal ShowLink As Boolean, ByVal RowCount As Integer) As String
        Dim name_string As String = ""
        Dim link_text As String = ""
        If Not IsNothing(aTempTable) Then
            If aTempTable.Rows.Count >= RowCount Then
                'Get the name string ready then get the year displaying before the name.
                name_string = aTempTable.Rows(RowCount).Item("amod_make_name").ToString & " " & aTempTable.Rows(RowCount).Item("amod_model_name").ToString
                link_text = aTempTable.Rows(RowCount).Item("ac_year_mfr").ToString & " " & name_string & " - "

                'If optional parameter showlink = true then go ahead and make the serial # link out, otherwise just display serial #
                If ShowLink = True Then
                    link_text += crmWebClient.DisplayFunctions.WriteDetailsLink(aTempTable.Rows(RowCount).Item("ac_id"), 0, 0, 0, True, "S/N# " & aTempTable.Rows(RowCount).Item("ac_ser_nbr").ToString, "", "")
                Else
                    link_text += "S/N# " & aTempTable.Rows(RowCount).Item("ac_ser_nbr").ToString
                End If
                'If there's a registration number.
                If Not IsDBNull(aTempTable.Rows(RowCount).Item("ac_reg_nbr")) Then
                    If aTempTable.Rows(RowCount).Item("ac_reg_nbr").ToString <> "" Then
                        link_text = link_text & ", Reg# " & aTempTable.Rows(RowCount).Item("ac_reg_nbr").ToString
                    End If
                End If

            End If
        End If

        Return link_text
    End Function














    Public Shared Function Build_String_To_HTML(ByVal ViewToPDF As String, ByVal report_name As String) As Boolean
        Build_String_To_HTML = False
        Try
            Build_String_To_HTML = True
            ' create a file to dump the PDF report to
            ' create a streamwriter variable
            Dim swPDF As System.IO.StreamWriter
            ' create the html file

            'Temp Hold MSW0


            '    swPDF = IO.File.CreateText(HttpContext.Current.Server.MapPath(Session.Item("MarketSummaryFolderVirtualPath").ToString) + "\" + report_name)
            swPDF = IO.File.CreateText(HttpContext.Current.Server.MapPath("") + "\TempFiles\" + report_name)

            ' write to the file
            swPDF.WriteLine(ViewToPDF)
            'close the streamwriter
            swPDF.Close()
            ' call the webgrabber info
            'Response.Write("Page:<br>" & ViewToPDF)




        Catch ex As Exception
            '   aCommonEvo.DisplayAlert("Error in Build_String_To_HTML: " & ex.Message)
        End Try
    End Function



    Public Shared Function Draw_Black_Line() As String
        Return "</td></tr></table><table width='100%' height='1'><tr><td width='100%' height='1px' bgcolor='black'></td></tr></table>" ' This is Line for Black Spacer----------
    End Function

    Public Shared Function Insert_Page_Break() As String
        Insert_Page_Break = ""
        Try
            Insert_Page_Break = "<table width='100%' align='center' class='break'><tr><td>&nbsp;</td></tr></table>"
        Catch ex As Exception
            ' aCommonEvo.DisplayAlert("Error in Insert_Page_Break: " & ex.Message)
        End Try
    End Function

    Public Shared Function Build_HTML_Page(ByVal viewToPDF As String) As String
        Build_HTML_Page = ""
        Try

            viewToPDF = viewToPDF & "</body></html>"
            Build_HTML_Page = viewToPDF
        Catch ex As Exception
            ' aCommonEvo.DisplayAlert("Error in Build_HTML_Page: " & ex.Message)
        End Try
    End Function


End Class
