Imports System.IO
Imports crmWebClient.clsGeneral
Partial Public Class aircraftCard
    Inherits System.Web.UI.UserControl
    Public aTempTable, aTempTable2 As New DataTable
    Public Event SetOtherID(ByVal id As Integer)
    Public Event SetUpDisplay()
    Public Event Synch_Date(ByVal Synch_Type As String, ByVal Sync_display As Label)
    'Public Event AddOtherIDToPage(ByVal c As ImageButton)
    Public Event Next_Prev_Btn(ByVal Command As String)
    Public Event ShareAircraftDataTable(ByVal t As clsClient_Aircraft, ByVal q As DataTable)
    Public Event ShareNotesDataTable(ByVal t As DataTable)
    Public Event ShareProspectDataTable(ByVal t As DataTable)
    Public Event ShareValueDataTable(ByVal t As DataTable)
    Public Event ShareActionDataTable(ByVal t As DataTable)
    Public Event ShareDocumentDataTable(ByVal t As DataTable)
    Dim error_string As String = ""
#Region "Page Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Me.Visible = True Then

            If Session.Item("crmUserLogon") = True Then
                Dim masterPage As main_site = DirectCast(Page.Master, main_site)
                Try
                    If Not IsNothing(Request.Item("ac_ID")) Then
                        If IsNumeric(Request.Item("ac_ID")) Then
                            Session.Item("ListingID") = Request.Item("ac_ID")
                        End If
                    End If

                    If Not IsNothing(Request.Item("source")) Then
                        If IsNumeric(Request.Item("source")) Then
                            Session.Item("ListingSource") = Request.Item("source")
                        End If
                    End If

                    set_next_prev(masterPage)
                    If Not Page.IsPostBack Then
                        RaiseEvent Synch_Date("Aircraft_Sync", synch_date_comp)
                        fill_AC_Info(masterPage.ListingSource, masterPage.ListingID, masterPage)
                        clsGeneral.clsGeneral.Recent_Cookies("aircraft", masterPage.ListingID, UCase(masterPage.ListingSource))
                    End If


                    'Check for Valuation:
                    'First clear the data table
                    aTempTable = New DataTable
                    'If the listing is CLIENT, we use the listing ID variable
                    If masterPage.ListingSource = "CLIENT" Then
                        aTempTable = masterPage.aclsData_Temp.Get_Open_Market_Valuation(masterPage.ListingID)
                    Else 'If the listing source is jetnet, we check to see if the other ID exists
                        'If it does, we use that one.
                        If masterPage.OtherID > 0 Then
                            aTempTable = masterPage.aclsData_Temp.Get_Open_Market_Valuation(masterPage.OtherID)
                        End If
                    End If
                    If Not IsNothing(aTempTable) Then
                        If aTempTable.Rows.Count > 0 Then
                            'This happens if a valuation record already exists. We send them off to the view.
                            If aTempTable.Rows.Count = 1 Then
                                masterPage.SetAircraftValuationLink = "<img src=""images/current_value.png"" alt="""" alt='Market Valuation' class='value_icon help_cursor values_icon_width' title='Launch Market Valuation' onclick=""javascript:load('view_template.aspx?ViewID=19&noteID=" & aTempTable.Rows(0).Item("lnote_id") & "&noMaster=false','','scrollbars=yes,menubar=no,height=700,width=1250,resizable=yes,toolbar=no,location=no,status=no');""/>"
                            Else
                                masterPage.SetAircraftValuationLink = "<img src=""images/current_value.png"" alt="""" alt='Market Valuation' class='value_icon help_cursor values_icon_width' title='Launch Market Valuation' onclick=""javascript:$find('THETABCONTAINERID').set_activeTabIndex($find('THETABID')._tabIndex);""/>"
                            End If
                        Else
                            'If the listing source is client, then we can go ahead and send them directly to the note.
                            If masterPage.ListingSource = "CLIENT" Then
                                masterPage.SetAircraftValuationLink = "<img src=""images/current_value.png"" alt="""" alt='Market Valuation' class='gold_icon help_cursor values_icon_width' title='Launch Market Valuation' onclick=""javascript:load('edit_note.aspx?action=new&amp;type=valuation&amp;cat_key=0&amp;refreshing=view&temporary=true','','scrollbars=yes,menubar=no,height=700,width=1250,resizable=yes,toolbar=no,location=no,status=no');""/>"
                            Else 'Otherwise if the listing source is JETNET and other ID is blank, then we need to go ahead and ask them to create a client aircraft record.
                                '    If masterPage.OtherID = 0 Then
                                '        masterPage.SetAircraftValuationLink = "<img src=""images/green_dollar.png"" alt="""" alt='Market Valuation' class='gold_icon help_cursor' title='Launch Market Valuation' onclick=""javascript:CreateValuationRecord('edit.aspx?action=edit&type=aircraft&ac_ID=" & masterPage.ListingID & "&source=JETNET');""/>"
                                '    Else 'Otherwise other ID exists and client record is there.
                                '        masterPage.SetAircraftValuationLink = "<img src=""images/green_dollar.png"" alt="""" alt='Market Valuation' class='gold_icon help_cursor' title='Launch Market Valuation' onclick=""javascript:load('edit_note.aspx?action=new&amp;type=valuation&amp;cat_key=0&amp;refreshing=view','','scrollbars=yes,menubar=no,height=700,width=1250,resizable=yes,toolbar=no,location=no,status=no');""/>"
                                '    End If
                            End If
                        End If
                    End If
                    ' HttpContext.Current.Session.Item("jetnetServerNotesDatabase") = CApplication.Item("crmClientDatabase")
                Catch ex As Exception
                    error_string = "AircraftCard.ascx.vb - Page Load() - " & ex.Message
                    masterPage.LogError(error_string)
                End Try
            End If
        End If
    End Sub
#End Region
#Region "Fill Aircraft Card Information"
    'Private Sub BuildCustomDataTab(ByVal Aircraft_Data As clsClient_Aircraft, ByVal masterPage As main_site)
    '  Dim ClientPreferencesTable As New DataTable

    '  Dim DisplayStr As String = ""
    '  ClientPreferencesTable = masterPage.aclsData_Temp.Get_Client_Preferences()
    '  'First we need to get the client Preferences.
    '  If Not IsNothing(ClientPreferencesTable) Then
    '    If ClientPreferencesTable.Rows.Count > 0 Then
    '      'Custom Field #1
    '      If Not IsDBNull(ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_1_use")) Then
    '        If ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_1_use") = "Y" Then
    '          If Not IsDBNull(ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_1")) Then
    '            DisplayStr = "<span class=""li""><span class=""label"">" & ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_1").ToString & "</span>: "
    '            If Not String.IsNullOrEmpty(Aircraft_Data.cliaircraft_custom_1) Then
    '              DisplayStr += Aircraft_Data.cliaircraft_custom_1
    '            End If
    '            DisplayStr += "</span>"
    '          End If
    '        End If
    '      End If
    '      'Custom Field #2
    '      If Not IsDBNull(ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_2_use")) Then
    '        If ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_2_use") = "Y" Then
    '          If Not IsDBNull(ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_2")) Then
    '            DisplayStr += "<span class=""li""><span class=""label"">" & ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_2").ToString & "</span>: "
    '            If Not String.IsNullOrEmpty(Aircraft_Data.cliaircraft_custom_2) Then
    '              DisplayStr += Aircraft_Data.cliaircraft_custom_2
    '            End If
    '            DisplayStr += "</span>"
    '          End If
    '        End If
    '      End If

    '      'Custom Field #3
    '      If Not IsDBNull(ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_3_use")) Then
    '        If ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_3_use") = "Y" Then
    '          If Not IsDBNull(ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_3")) Then
    '            DisplayStr += "<span class=""li""><span class=""label"">" & ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_3").ToString & "</span>: "
    '            If Not String.IsNullOrEmpty(Aircraft_Data.cliaircraft_custom_3) Then
    '              DisplayStr += Aircraft_Data.cliaircraft_custom_3
    '            End If
    '            DisplayStr += "</span>"
    '          End If
    '        End If
    '      End If

    '      'Custom Field #4
    '      If Not IsDBNull(ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_4_use")) Then
    '        If ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_4_use") = "Y" Then
    '          If Not IsDBNull(ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_4")) Then
    '            DisplayStr += "<span class=""li""><span class=""label"">" & ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_4").ToString & "</span>: "
    '            If Not String.IsNullOrEmpty(Aircraft_Data.cliaircraft_custom_4) Then
    '              DisplayStr += Aircraft_Data.cliaircraft_custom_4
    '            End If
    '            DisplayStr += "</span>"
    '          End If
    '        End If
    '      End If

    '      'Custom Field #5
    '      If Not IsDBNull(ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_5_use")) Then
    '        If ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_5_use") = "Y" Then
    '          If Not IsDBNull(ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_5")) Then
    '            DisplayStr += "<span class=""li""><span class=""label"">" & ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_5").ToString & "</span>: "
    '            If Not String.IsNullOrEmpty(Aircraft_Data.cliaircraft_custom_5) Then
    '              DisplayStr += Aircraft_Data.cliaircraft_custom_5
    '            End If
    '            DisplayStr += "</span>"
    '          End If
    '        End If
    '      End If


    '      'Custom Field #6
    '      If Not IsDBNull(ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_6_use")) Then
    '        If ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_6_use") = "Y" Then
    '          If Not IsDBNull(ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_6")) Then
    '            DisplayStr += "<span class=""li""><span class=""label"">" & ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_6").ToString & "</span>: "
    '            If Not String.IsNullOrEmpty(Aircraft_Data.cliaircraft_custom_6) Then
    '              DisplayStr += Aircraft_Data.cliaircraft_custom_6
    '            End If
    '            DisplayStr += "</span>"
    '          End If
    '        End If
    '      End If


    '      'Custom Field #7
    '      If Not IsDBNull(ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_7_use")) Then
    '        If ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_7_use") = "Y" Then
    '          If Not IsDBNull(ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_7")) Then
    '            DisplayStr += "<span class=""li""><span class=""label"">" & ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_7").ToString & "</span>: "
    '            If Not String.IsNullOrEmpty(Aircraft_Data.cliaircraft_custom_7) Then
    '              DisplayStr += Aircraft_Data.cliaircraft_custom_7
    '            End If
    '            DisplayStr += "</span>"
    '          End If
    '        End If
    '      End If


    '      'Custom Field #8
    '      If Not IsDBNull(ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_8_use")) Then
    '        If ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_8_use") = "Y" Then
    '          If Not IsDBNull(ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_8")) Then
    '            DisplayStr += "<span class=""li""><span class=""label"">" & ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_8").ToString & "</span>: "
    '            If Not String.IsNullOrEmpty(Aircraft_Data.cliaircraft_custom_8) Then
    '              DisplayStr += " : " & Aircraft_Data.cliaircraft_custom_8
    '            End If
    '            DisplayStr += "</span>"
    '          End If
    '        End If
    '      End If

    '      'Custom Field #9
    '      If Not IsDBNull(ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_9_use")) Then
    '        If ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_9_use") = "Y" Then
    '          If Not IsDBNull(ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_9")) Then
    '            DisplayStr += "<span class=""li""><span class=""label"">" & ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_9").ToString & "</span>: "
    '            If Not String.IsNullOrEmpty(Aircraft_Data.cliaircraft_custom_9) Then
    '              DisplayStr += Aircraft_Data.cliaircraft_custom_9
    '            End If
    '            DisplayStr += "</span>"
    '          End If
    '        End If
    '      End If

    '      'Custom Field #10
    '      If Not IsDBNull(ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_10_use")) Then
    '        If ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_10_use") = "Y" Then
    '          If Not IsDBNull(ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_10")) Then
    '            DisplayStr += "<span class=""li""><span class=""label"">" & ClientPreferencesTable.Rows(0).Item("clipref_ac_custom_10").ToString & "</span>: "
    '            If Not String.IsNullOrEmpty(Aircraft_Data.cliaircraft_custom_10) Then
    '              DisplayStr += Aircraft_Data.cliaircraft_custom_10
    '            End If
    '            DisplayStr += "</span>"
    '          End If
    '        End If
    '      End If


    '    End If
    '  End If

    '  custom_data_information.Text = DisplayStr
    'End Sub
    Public Sub fill_AC_Info(ByVal source As String, ByVal idnum As Integer, ByVal masterPage As main_site)
        Dim Aircraft_Data As New clsClient_Aircraft
        Dim Aircraft_Model As String = ""
        'Dim Aircraft_Model_Data As New clsClient_Aircraft_Model
        Dim Price_Status As Integer = 0
        '--------Basic Aircraft Left Card Display for the AC Information------------------------------------------------
        Try
            If UCase(source) = "CLIENT" Then

                aTempTable = masterPage.aclsData_Temp.Get_Clients_Aircraft(idnum)
                If aTempTable.Rows.Count > 0 Then
                    Aircraft_Model = (aTempTable.Rows(0).Item("cliamod_make_name") & " " & aTempTable.Rows(0).Item("cliamod_model_name"))
                    Aircraft_Data = clsGeneral.clsGeneral.Create_Aircraft_Class(aTempTable, "cliaircraft")
                    Aircraft_Data.cliaircraft_id = idnum
                End If
                'Sharing Aircraft Data
                RaiseEvent ShareAircraftDataTable(Aircraft_Data, aTempTable)
                full_page.Text = ""
                aTempTable.Dispose()
                'Toggle the custom data tab on.
                custom_data_tab.Visible = True
                custom_data_information.Text = "<table width=""100%"" cellpadding=""0"" cellspacing=""0"">" & CommonAircraftFunctions.BuildCustomDataTab(masterPage.aclsData_Temp, Aircraft_Data.cliaircraft_custom_1, Aircraft_Data.cliaircraft_custom_2, Aircraft_Data.cliaircraft_custom_3, Aircraft_Data.cliaircraft_custom_4, Aircraft_Data.cliaircraft_custom_5, Aircraft_Data.cliaircraft_custom_6, Aircraft_Data.cliaircraft_custom_7, Aircraft_Data.cliaircraft_custom_8, Aircraft_Data.cliaircraft_custom_9, Aircraft_Data.cliaircraft_custom_10) & "</table>"

                ' full_page.Text = "<a href='#' onclick=""javascript:load('DisplayAircraftDetail.aspx?acid=" & CLng(Session("ListingID")) & "&jid=0&source=CLIENT','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');return false;"" ><img src='images/full_view.jpg' alt='Full Page View' border='0' /></a>"
            ElseIf UCase(source) = "JETNET" Then
                aTempTable = masterPage.aclsData_Temp.GetJETNET_Aircraft_Details_AC_ID(idnum, "")
                If aTempTable.Rows.Count > 0 Then
                    Aircraft_Data = clsGeneral.clsGeneral.Create_Aircraft_Class(aTempTable, "ac")
                    Aircraft_Model = (aTempTable.Rows(0).Item("amod_make_name") & " " & aTempTable.Rows(0).Item("amod_model_name"))
                    Aircraft_Data.cliaircraft_id = idnum
                End If
                'Sharing Aircraft Data
                RaiseEvent ShareAircraftDataTable(Aircraft_Data, aTempTable)
                aTempTable.Dispose()

                full_page.Text = "<a href='#' onclick=""javascript:load('DisplayAircraftDetail.aspx?acid=" & CLng(Session("ListingID")) & "&jid=0','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');return false;"" ><img src='images/full_view.jpg' alt='Full Page View' border='0' /></a>"

            End If

            If aTempTable.Rows.Count = 0 Then
                Response.Redirect("/listing_air.aspx?ac_not_exist=true", False)
                Context.ApplicationInstance.CompleteRequest()
            End If

            Dim aircraft_text As String = ""
            'comp_name.Text = Aircraft_Model & "<br />"
            info_tab.HeaderText = Aircraft_Model
            'This is for the OTHER ID only on the JETNET SIDE. 
            If Aircraft_Data.cliaircraft_id <> 0 Then 'protect it
                If masterPage.ListingSource = "JETNET" Then
                    aTempTable2 = masterPage.aclsData_Temp.CHECKFORClient_Aircraft_JETNET_AC(Aircraft_Data.cliaircraft_id)
                    If Not IsNothing(aTempTable2) Then
                        If aTempTable2.Rows.Count > 0 Then
                            'Raise event to set Other ID for Aircraft.This would be the client ID for a jetnet Aircraft if one exists. 
                            RaiseEvent SetOtherID(aTempTable2.Rows(0).Item("cliaircraft_id"))
                            masterPage.ShowJetnetClientOption = True
                        End If
                    Else
                        If masterPage.aclsData_Temp.class_error <> "" Then
                            error_string = masterPage.aclsData_Temp.class_error
                            masterPage.LogError("Aircraft_Card.ascx.vb - fill_AC_Info() - " & error_string)
                        End If
                        masterPage.display_error()
                    End If
                Else
                    'Raise event to set Other ID for Aircraft. This would be the jetnet ID for the client Aircraft if one exists. 
                    RaiseEvent SetOtherID(Aircraft_Data.cliaircraft_jetnet_ac_id)
                    masterPage.ShowJetnetClientOption = True
                End If
            End If
            'no longer used since we moved the pictures to their own tab
            'If Not IsDBNull(Aircraft_Data.cliaircraft_picture_exist_flag) Then
            '    If Aircraft_Data.cliaircraft_picture_exist_flag = "Y" Then
            '        ac_camera_photo.Visible = True
            '    End If
            'End If
            contact_info.EnableViewState = True
            contact_info.Text = clsGeneral.clsGeneral.Build_Aircraft_Display(Aircraft_Data, True, False, False)
            contact_right.EnableViewState = True
            aircraft_text = clsGeneral.clsGeneral.Build_Aircraft_Display(Aircraft_Data, False, True, False)

            aTempTable = New DataTable

            If Session.Item("localUser").crmEvo <> True Then
                If Aircraft_Data.cliaircraft_id <> 0 Then 'protect it
                    If masterPage.ListingSource = "JETNET" Then
                        aTempTable = masterPage.aclsData_Temp.Dual_NotesOnlyOne(0, Aircraft_Data.cliaircraft_id, "A", True, False)
                    Else
                        aTempTable = masterPage.aclsData_Temp.Dual_NotesOnlyOne(Aircraft_Data.cliaircraft_id, 0, "A", True, False)
                    End If
                End If
            End If

            'Figure out what category number Price/Status is: 
            Price_Status = masterPage.what_cat(0, "PRICE/STATUS", True)
            'Figure out if there's a Price/Status Note for the Aircraft. 
            If Not IsNothing(aTempTable) Then
                If aTempTable.Rows.Count > 0 Then
                    For Each t As DataRow In aTempTable.Rows
                        If t("lnote_notecat_key") = Price_Status Then
                            aircraft_text = aircraft_text & "<br />"
                            aircraft_text = aircraft_text & "<em>" & t("lnote_note") & "</em>"
                        End If
                    Next
                End If
            Else
                If masterPage.aclsData_Temp.class_error <> "" Then
                    error_string = masterPage.aclsData_Temp.class_error
                    masterPage.LogError("Aircraft_Card.ascx.vb - fill_AC_Info() - " & error_string)
                End If
                masterPage.display_error()
            End If
            RaiseEvent ShareNotesDataTable(aTempTable)
            aTempTable.Dispose()

            aTempTable = New DataTable
            If Session.Item("localUser").crmEvo <> True Then
                If Session.Item("localSubscription").crmDocumentsFlag = True Then
                    If Aircraft_Data.cliaircraft_id <> 0 Then 'protect it
                        If masterPage.ListingSource = "JETNET" Then
                            aTempTable = masterPage.aclsData_Temp.Dual_NotesOnlyOne(0, Aircraft_Data.cliaircraft_id, "F", True, False)
                        Else
                            aTempTable = masterPage.aclsData_Temp.Dual_NotesOnlyOne(Aircraft_Data.cliaircraft_id, 0, "F", True, False)
                        End If
                    End If
                End If
            End If
            RaiseEvent ShareDocumentDataTable(aTempTable)
            aTempTable.Dispose()

            ''Share Prospect Information
            aTempTable = New DataTable
            If Session.Item("localUser").crmEvo <> True Then
                If Aircraft_Data.cliaircraft_id <> 0 Then 'protect it
                    If masterPage.ListingSource = "JETNET" Then
                        aTempTable = masterPage.aclsData_Temp.ChangeProspectNotesByParameters(0, 0, 0, Aircraft_Data.cliaircraft_id, 0, Aircraft_Data.cliaircraft_cliamod_id, True, False, False)
                    Else
                        aTempTable = masterPage.aclsData_Temp.ChangeProspectNotesByParameters(0, 0, Aircraft_Data.cliaircraft_id, 0, Aircraft_Data.cliaircraft_cliamod_id, 0, True, False, False)
                    End If
                End If
            End If
            RaiseEvent ShareProspectDataTable(aTempTable)
            aTempTable.Dispose()

            'If Session.Item("jetnetWebSiteType") = eWebSiteTypes.TEST Or Session.Item("jetnetWebSiteType") = eWebSiteTypes.LOCAL Then
            'Share Value Information
            aTempTable = New DataTable
            If Session.Item("localUser").crmEvo <> True Then
                If Aircraft_Data.cliaircraft_id <> 0 Then 'protect it
                    If masterPage.ListingSource = "CLIENT" Then
                        aTempTable = masterPage.aclsData_Temp.GetListOfValuation(Aircraft_Data.cliaircraft_id, masterPage.OtherID, 0, 0)
                    ElseIf masterPage.ListingSource = "JETNET" And masterPage.OtherID > 0 Then
                        aTempTable = masterPage.aclsData_Temp.GetListOfValuation(masterPage.OtherID, Aircraft_Data.cliaircraft_id, 0, 0)
                    End If
                End If
            End If
            RaiseEvent ShareValueDataTable(aTempTable)
            aTempTable.Dispose()
            'End If

            'Jetnet LEASE information
            If source = "JETNET" Then
                aTempTable2 = masterPage.aclsData_Temp.GetAircraft_Lease_acID_ExpFlag(idnum, "N", 0)
                ' check the state of the DataTable
                If Not IsNothing(aTempTable2) Then
                    If aTempTable2.Rows.Count > 0 Then
                        For Each q As DataRow In aTempTable2.Rows
                            aircraft_text = aircraft_text & ": "
                            If Not IsDBNull(q("aclease_term")) Then
                                aircraft_text = aircraft_text & "Term " & q("aclease_term")
                            End If
                            If Not IsDBNull(q("aclease_date_expiration")) Then
                                If q("aclease_date_expiration") <> "12:00:00 AM" Then
                                    aircraft_text = aircraft_text & " Expires " & q("aclease_date_expiration")
                                End If
                            End If
                            If Not IsDBNull(q("aclease_note")) Then
                                If q("aclease_note") <> "" Then
                                    aircraft_text = aircraft_text & " - " & q("aclease_note")
                                End If
                            End If
                            aircraft_text = aircraft_text & "</span>"
                        Next
                    End If
                End If
            End If

            aircraft_text += "<br clear=""all"" /><a href='#' onclick=""javascript:load('FAAFlightData.aspx?acid=" & IIf(masterPage.ListingSource = "CLIENT", masterPage.OtherID, masterPage.ListingID) & "','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');""><img src='images/ac_active.png' alt='' class='float_left' border='0' /></a>"


            'Displaying contact right information
            contact_right.Text = aircraft_text

            If HttpContext.Current.Session.Item("localUser").crmEvo <> True Then 'If an EVO user'if not in evolution

                email_ac.Text = "<a href='#' onclick=""javascript:load('edit_note.aspx?action=new&type=email&ac_ID=" & masterPage.ListingID & "&source=" & masterPage.ListingSource & "&cat_key=0','','scrollbars=yes,menubar=no,height=610,width=890,resizable=yes,toolbar=no,location=no,status=no');""><img src='images/mail_compose.png' alt='Email Company' width='24' border='0' /></a>"

                create_client_company.OnClientClick = "javascript:load('edit.aspx?action=edit&type=aircraft&ac_ID=" & masterPage.ListingID & "&source=" & masterPage.ListingSource & "','','scrollbars=yes,menubar=no,height=900,width=1030,resizable=yes,toolbar=no,location=no,status=no');return false;"
                edit.OnClientClick = "javascript:load('edit.aspx?action=edit&type=aircraft&ac_ID=" & masterPage.ListingID & "&source=" & masterPage.ListingSource & "','','scrollbars=yes,menubar=no,height=900,width=1030,resizable=yes,toolbar=no,location=no,status=no');return false;"

            Else
                create_client_company.Visible = False
                email_ac.Visible = False
            End If
            If Aircraft_Data.cliaircraft_id <> 0 Then 'protect it

                aTempTable = New DataTable
                If Session.Item("localUser").crmEvo <> True Then
                    If masterPage.ListingSource = "JETNET" Then
                        aTempTable = masterPage.aclsData_Temp.Dual_NotesOnlyOne(masterPage.OtherID, Aircraft_Data.cliaircraft_id, "P", True, False)
                    Else
                        aTempTable = masterPage.aclsData_Temp.Dual_NotesOnlyOne(Aircraft_Data.cliaircraft_id, masterPage.OtherID, "P", True, False)
                    End If
                End If
            End If
            RaiseEvent ShareActionDataTable(aTempTable) 'share datatable
            aTempTable.Dispose()



            full_page.Text = "<a href='#' onclick=""javascript:load('DisplayAircraftDetail.aspx?acid=" & Aircraft_Data.cliaircraft_id & "&jid=0" & IIf(masterPage.ListingSource = "CLIENT", "&source=CLIENT", "") & "','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');return false;"" ><img src='images/full_view.jpg' alt='Full Page View' border='0' /></a>"

        Catch ex As Exception
            error_string = "AircraftCard.ascx.vb - Fill_AC_Info() " & ex.Message
            masterPage.LogError(error_string)
        End Try
        'End If 
    End Sub
#End Region
#Region "Function that figures out the next/previous in the Aircraft Listing"
    Private Sub set_next_prev(ByVal masterpage As crmWebClient.main_site)
        Try
            Dim next_prev As String = ""
            Dim next_id As String = ""
            Dim prev_id As String = ""
            Dim next_type As String = ""
            Dim prev_type As String = ""
            Dim session_var_next() As String
            Dim session_var_prev() As String
            Dim CurrentRecord As Long = 0
            Dim AlreadyKnowOrderNumber As Long = 0
            Dim UseOrder As Boolean = False
            If Not IsNothing(Trim(Request("order"))) Then
                If IsNumeric(Trim(Request("order"))) Then
                    UseOrder = True
                    AlreadyKnowOrderNumber = Trim(Request("order"))
                End If
            End If

            If Not IsNothing(Session("my_ids")) Then
                HttpContext.Current.Session("crmPagingParent") = "DETAILS"

                If UseOrder Then
                    If UBound(Session("my_ids")) = AlreadyKnowOrderNumber Then
                    Else
                        session_var_next = Split((Session("my_ids")(AlreadyKnowOrderNumber + 1)), "|")
                        next_id = session_var_next(0)
                        next_type = session_var_next(1)
                    End If
                    If LBound(Session("my_ids")) = AlreadyKnowOrderNumber Then
                    Else
                        session_var_prev = Split((Session("my_ids")(AlreadyKnowOrderNumber - 1)), "|")
                        prev_id = session_var_prev(0)
                        prev_type = session_var_prev(1)
                    End If
                    CurrentRecord = AlreadyKnowOrderNumber
                Else

                    For i = LBound(Session("my_ids")) To UBound(Session("my_ids"))
                        Dim session_var() As String = Split((Session("my_ids")(i)), "|")
                        Dim compare_id As String = Trim(Request("contact_id"))
                        If compare_id = "" Then
                            compare_id = Session("ListingID")
                        End If
                        If session_var(0) = compare_id Then
                            CurrentRecord = i
                            'Try
                            If UBound(Session("my_ids")) = i Then
                                'No Next
                            Else
                                session_var_next = Split((Session("my_ids")(i + 1)), "|")
                                next_id = session_var_next(0)
                                next_type = session_var_next(1)
                            End If

                            If LBound(Session("my_ids")) = i Then
                                'Nothing previous
                            Else
                                session_var_prev = Split((Session("my_ids")(i - 1)), "|")
                                prev_id = session_var_prev(0)
                                prev_type = session_var_prev(1)
                            End If
                        End If
                    Next
                End If


                'Dim prev_btn As New ImageButton
                'Dim next_btn As New ImageButton

                Dim next_prev_str As String = "<table width='250' cellspacing='0' cellpadding='0' class=""float_right""><tr>"

                If prev_id <> "" Then
                    next_prev_str = next_prev_str & "<td align='left' valign='top' width='25'><a href='details.aspx?" & IIf(UseOrder, "order=" & AlreadyKnowOrderNumber - 1 & "&", "") & "source=" & prev_type & "&ac_ID=" & prev_id & "&type=3'><img src='images/previous.png' alt='Previous' border='0' /></a></td>"
                    'prev_btn.ImageUrl = "~/images/previous.png"
                    'prev_btn.AlternateText = "Previous"
                    'prev_btn.CommandName = prev_id & "|" & prev_type & "|"
                    'prev_btn.EnableViewState = True
                    'AddHandler prev_btn.Click, AddressOf show
                    'next_prev_text.Controls.Add(prev_btn)
                Else
                    next_prev_str = next_prev_str & "<td align='left' valign='top' width='25'><img src='images/spacer.gif' alt='' width='25' height='25' /></td>"
                End If

                If next_id <> "" Then
                    If next_prev_str <> "" Then
                        next_prev_str = next_prev_str & "  "
                    End If
                    next_prev_str = next_prev_str & "<td align='left' valign='top' width='25'><a href='details.aspx?" & IIf(UseOrder, "order=" & AlreadyKnowOrderNumber + 1 & "&", "") & "source=" & next_type & "&ac_ID=" & next_id & "&type=3'><img src='images/next.png' alt='Next'  border='0' /></a></td>"
                    'next_btn.ImageUrl = "~/images/next.png"
                    'next_btn.AlternateText = "Next"
                    'next_btn.CommandName = next_id & "|" & next_type & "|"
                    'next_btn.EnableViewState = True
                    'AddHandler next_btn.Click, AddressOf show
                    'next_prev_text.Controls.Add(next_btn)
                Else
                    next_prev_str = next_prev_str & "<td align='left' valign='top' width='25'><img src='images/spacer.gif' alt='' width='25' height='25' /></td>"
                End If

                If next_id <> "" Or prev_id <> "" Then
                    next_prev_str += "<td><span class=""tinyRecordCount"">" & CurrentRecord + 1 & " of " & UBound(Session("my_ids")) + 1 & "</span></td>"
                End If

                next_prev_str = next_prev_str & "</tr></table>"
                Dim lab As New Label
                lab.Text = next_prev_str
                next_prev_text.Controls.Add(lab)

            End If
        Catch ex As Exception
            error_string = "AircraftCard.ascx.vb - set_next_prev() " & ex.Message
            masterpage.LogError(error_string)
        End Try
    End Sub
#End Region

End Class