Imports System.IO
Partial Public Class Notes
    Inherits System.Web.UI.UserControl
    Dim aclsData_Temp As New Object
    Dim aTempTable, aTempTable2 As New DataTable 'Data Tables used
    Public Event Aircraft_Name_Changed(ByVal con As Control, ByVal FillModel As Boolean)
    Public Event company_name_changed(ByVal con As Control)
    Public Event contact_name_changed(ByVal con As Control)
    Public Event FillCompanyDrop(ByVal con As Control)
    Public Event ac_searchClick(ByVal con As Control)
    Public Event company_searchClick(ByVal con As Control)
    Public Event fill_drop(ByVal jetnet_ac As Integer, ByVal client_ac As Integer, ByVal jetnet_comp As Integer, ByVal client_comp As Integer, ByVal jetnet_contact As Integer, ByVal client_contact As Integer, ByVal jetnet_mod As Integer, ByVal client_mod As Integer, ByVal con As Control, ByVal FillModel As Boolean)
    Public Event edit_note(ByVal type As String, ByVal con As Control, ByVal idnum As Integer)
    Public Event remove_note_ev(ByVal idnum As Integer, ByVal con As Control, ByVal type As String)
    Dim error_string As String = ""
    Public TypeOfNote As String = ""

#Region "Page Events"

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Not IsNothing(Trim(Request("from"))) Then
            If Not String.IsNullOrEmpty(Trim(Request("from"))) Then
                If LCase(Trim(Request("from"))) = "aircraftdetails" Then
                    Session.Item("Listing") = 3
                ElseIf LCase(Trim(Request("from"))) = "companydetails" Then
                    Session.Item("Listing") = 1
                End If
            End If
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.Visible Then
            Try
                Dim valuationExists As Boolean = False
                aclsData_Temp = New clsData_Manager_SQL
                aclsData_Temp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase") 'CApplication.Item("crmClientDatabase")
                aclsData_Temp.JETNET_DB = Session.Item("jetnetClientDatabase") 'CApplication.Item("crmJetnetDatabase")

                aclsData_Temp.class_error = ""

                If Not String.IsNullOrEmpty(Trim(Request("rememberTab"))) Then
                    Session.Item("ViewActiveTab") = Trim(Request("rememberTab"))
                End If

                'We need to add a check in here.
                'When we click the $ to launch the market value tool from the aircraft details page - 
                'even if we believe that we do not have an available analysis (because we checked on page load) 
                'we need to recheck and not recreate if we have one when we click in case they had closed their 
                'analysis window and then clicked the $ without refreshing the page.

                If Trim(Request("action")) = "new" Then
                    If Trim(Request("type")) = "valuation" Then
                        If Trim(Request("refreshing")) = "view" Then
                            If Trim(Request("temporary")) = "true" Then
                                'This means that we should double check really quick to see if they already have an open valuation. 
                                'If they don't, carry on. If they do - redirect to the view.
                                If Session.Item("ListingSource") = "CLIENT" Then
                                    aTempTable = aclsData_Temp.Get_Open_Market_Valuation(Session.Item("ListingID"))
                                Else 'If the listing source is jetnet, we check to see if the other ID exists
                                    'If it does, we use that one.
                                    If Session.Item("OtherID") > 0 Then
                                        aTempTable = aclsData_Temp.Get_Open_Market_Valuation(Session.Item("OtherID"))
                                    End If
                                End If

                                If Not IsNothing(aTempTable) Then
                                    If aTempTable.Rows.Count > 0 Then
                                        valuationExists = True
                                        Response.Redirect("view_template.aspx?ViewID=19&noteID=" & aTempTable.Rows(0).Item("lnote_id") & "&noMaster=false", False)
                                        Context.ApplicationInstance.CompleteRequest()
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If



                'This is going to set a session variable that will get cleared on return to the company or aircraft tabs page.
                If Not IsNothing(Trim(Request("startCount"))) Then 'Making sure it's set before we save session.
                    If IsNumeric(Trim(Request("startCount"))) Then 'Making sure we're dealing with numbers only.
                        If Trim(Request("startCount")) > 0 Then 'Since 0 is where it default starts, we don't need to bother storing the variable if it's 0.
                            Session.Item("startCount") = Trim(Request("startCount"))
                        End If
                    End If
                End If

                Select Case Trim(Request("type"))
                    Case "prospect"
                        TypeOfNote = "Prospect"
                        CustomValidator1.Enabled = False
                        CustomValidator1.Visible = False
                        CalendarExtender2.OnClientDateSelectionChanged = ""
                        action_item_lbl.Visible = False
                        Me.category_cell.Visible = False
                        Me.category_cell2.Visible = False
                        prospect_status_row.Visible = True
                        mecbe1.Enabled = True
                        mecbe2.Enabled = True
                        ProspectAircraft.Visible = True
                        prospect_ac_required.Enabled = True
                        prospect_company_required.Enabled = True
                        prospect_company_required_2.Enabled = True
                        aircraft_model_prospect_swap.Visible = True
                        prospectOppRow.Visible = True
                        prospectOppRow2.Visible = True
                        prospectOppRow3.Visible = True
                        prospect_row.Visible = True
                        referral_row.Visible = True
                        prospect_source_row.Visible = True

                        If Trim(Request("action")) = "new" Then
                            add_note_automatically.Visible = True
                        End If


                        If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Or Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
                            If Not Page.IsPostBack Then
                                opp_status.Items.Add(New ListItem("Closed Deal", "C"))
                                opp_status.Items.FindByValue("A").Text = "Open"
                                opp_status.Items.FindByValue("I").Text = "Inactive"
                            End If
                        Else
                            If Not Page.IsPostBack Then
                                opp_status.Items.Add(New ListItem("Closed Deal", "C"))
                                opp_status.Items.FindByValue("A").Text = "Open Prospect"
                                opp_status.Items.FindByValue("I").Text = "Inactive Prospect"
                            End If
                        End If


                        opp_status.CssClass = "oppRadio tiny_text"
                        Dim ConfirmDialog As String = ""
                        ConfirmDialog += " if ($('#" & opp_status.ClientID & " input:checked').val() == 'C') { "
                        ConfirmDialog += "$('#" & TargetDateText.ClientID & "').html('Closing Date');"
                        ConfirmDialog += " $('#" & capt_per.ClientID & "').val('100');"
                        ConfirmDialog += "if (confirm(""Would you like to set today as the closing date?"")) {"

                        ConfirmDialog += "var date = new Date();"
                        ConfirmDialog += "var year = date.getFullYear();"
                        ConfirmDialog += "var day = date.getDate();"
                        ConfirmDialog += "var month = date.getMonth();"
                        ConfirmDialog += " var h = date.getHours();"
                        ConfirmDialog += " var hoursDis =date.getHours();"
                        ConfirmDialog += " var dd = ""AM"";"
                        ConfirmDialog += "if (h < 10) {"
                        ConfirmDialog += "hoursDis = '0' + hoursDis + ':00';"
                        ConfirmDialog += "} else {"
                        ConfirmDialog += "hoursDis =  hoursDis + ':00';} "
                        ConfirmDialog += "month = month + 1;"
                        ConfirmDialog += "month = month < 10? '0' + month: month;"
                        ConfirmDialog += "day = day < 10? '0' + day: day;"
                        ConfirmDialog += "var answer = month + '/' + day + '/' + year;"
                        ConfirmDialog += " $('#" & targetdate.ClientID & "').val(answer);"
                        ConfirmDialog += " $('#" & time.ClientID & "').val(hoursDis);"
                        ConfirmDialog += " $('#" & note_date.ClientID & "').val(answer);"

                        ConfirmDialog += "};"
                        ConfirmDialog += "} else if ($('#" & opp_status.ClientID & " input:checked').val() == 'I') {$('#" & capt_per.ClientID & "').val('0');$('#" & TargetDateText.ClientID & "').html('Target Closing Date');"
                        ConfirmDialog += "} else {$('#" & TargetDateText.ClientID & "').html('Target Closing Date');};"

                        opp_status.Attributes.Add("onchange", ConfirmDialog)
                        curprev.Items.FindByValue("P").Text = "Next Action Date/Time"
                        enteredByText.Text = "Assigned To:"
                        prospect_priority_row.Visible = True

                        If Not Page.IsPostBack Then
                            If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Or Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
                                ' dont run this, no need, and possibly no crm connection  
                                If Trim(Request("id")) <> "" Then
                                    aTempTable = aclsData_Temp.Notes_Search_For_Prospect_View_Homebase("", "", "", "B','O", 0, "", "", "", "", "", "", 0, 0, "", False, False, "", "", Trim(Request("id")))
                                    Call Fill_Customer_Activities_FromView(aTempTable.Rows(0).Item("comp_id"), 0)
                                End If
                            Else
                                clsGeneral.clsGeneral.FillPriorityDropdown(priorityID)

                                Try
                                    priorityID.SelectedValue = priorityID.Items.FindByText("None").Value
                                Catch
                                End Try
                            End If
                        End If

                    Case "valuation"
                        TypeOfNote = "Valuation"
                        prospect_status_row.Visible = True
                        prospect_status_text.Text = "Valuation Status:"
                        action_item_lbl.Visible = False
                        Me.category_cell.Visible = False
                        Me.category_cell2.Visible = False
                        ProspectAircraft.Visible = False
                        market_value_description_text.Visible = True
                        resize_function.Text = "<script type=""text/javascript"">function FitPic() {}</script>"
                        notesdate.Visible = False
                        If Trim(Request("action")) = "new" Then
                            If Trim(Request("temporary")) = "true" Then
                                If Not Page.IsPostBack Then
                                    If Not String.IsNullOrEmpty(Trim(Request("listing"))) Then
                                        If Trim(Request("listing")) = "true" Then
                                            Session.Item("Listing") = 3
                                        End If
                                    End If
                                    opp_status.Items.Add(New ListItem("Temporary", "T")) 'sets up the temporary status.
                                    opp_status.SelectedValue = "T" 'selects temporary status in preparation to autosave.
                                    RequiredFieldValidator3.Enabled = False 'This disables the required validator for the note text.
                                End If
                            End If
                            opp_status.Enabled = False
                            'add_note.ImageUrl = "~/images/edit.jpg"
                            opp_status.ToolTip = "Vaulation must be entered with a status of ""Open""."
                        Else
                            'add_note.ImageUrl = "~/images/edit.jpg"

                        End If

                    Case Else
                        TypeOfNote = "Note"
                        notesCell.ColumnSpan = 4
                        prospect_status_row.Visible = False
                        add_prospect_automatically.Visible = False
                        Dim PreviousNextText As String = ""
                        Dim NotesHold As New DataTable
                        Dim NotesTemp As New DataTable
                        Dim Notes_Search As New DataTable

                        If Not IsNothing(Trim(Request("nextNote"))) Or Not IsNothing(Trim(Request("previousNote"))) Then
                            If IsNumeric(Trim(Request("nextNote"))) Or IsNumeric(Trim(Request("previousNote"))) Then
                                If Trim(Request("nextNote")) <> "0" Or Trim(Request("previousNote")) <> "0" Then
                                    If Session.Item("Listing") = "1" Then
                                        If Session("ListingID") > 0 Then
                                            If Session.Item("ListingSource") = "CLIENT" Then 'If notes are a client Aircraft
                                                NotesHold = aclsData_Temp.Dual_NotesOnlyOne(Session("ListingID"), 0, "A", False, True) 'Datahook for client/note aircraft
                                            Else
                                                NotesHold = aclsData_Temp.Dual_NotesOnlyOne(0, Session("ListingID"), "A", False, True) 'Datahook for jetnet/note aircraft
                                            End If
                                            NotesHold = clsGeneral.clsGeneral.AddNextPreviousToNotesTable(NotesHold)
                                            Notes_Search = NotesHold.Clone
                                            NotesTemp = NotesHold
                                        End If
                                    ElseIf Session.Item("Listing") = "3" Then
                                        If Session("ListingID") > 0 Then
                                            If Session.Item("ListingSource") = "JETNET" Then
                                                NotesHold = aclsData_Temp.Dual_NotesOnlyOne(0, Session("ListingID"), "A", True, False)
                                            Else
                                                NotesHold = aclsData_Temp.Dual_NotesOnlyOne(Session("ListingID"), 0, "A", True, False)
                                            End If
                                            NotesHold = clsGeneral.clsGeneral.AddNextPreviousToNotesTable(NotesHold)
                                            Notes_Search = NotesHold.Clone
                                            NotesTemp = NotesHold
                                        End If
                                    End If
                                End If
                            End If
                        End If

                        If Not IsNothing(Trim(Request("nextNote"))) Then
                            If IsNumeric(Trim(Request("nextNote"))) Then
                                If Trim(Request("nextNote")) > 0 Then
                                    Dim TempNextHold As Integer = 0
                                    Dim TempPreviousHold As Integer = 0
                                    If NotesTemp.Rows.Count > 0 Then
                                        Dim afiltered_Client As DataRow() = NotesTemp.Select("lnote_id=" & Trim(Request("nextNote")), "")
                                        ' extract and import
                                        For Each atmpDataRow_Client In afiltered_Client
                                            Notes_Search.ImportRow(atmpDataRow_Client)
                                        Next
                                    End If

                                    If Not IsNothing(Notes_Search) Then
                                        If Notes_Search.Rows.Count > 0 Then
                                            If Notes_Search.Rows(0).Item("lnote_next_id") > 0 Then
                                                TempNextHold = Notes_Search.Rows(0).Item("lnote_next_id")
                                            End If
                                            If Notes_Search.Rows(0).Item("lnote_previous_id") > 0 Then
                                                TempPreviousHold = Notes_Search.Rows(0).Item("lnote_previous_id")
                                            End If
                                        End If
                                    End If

                                    PreviousNextText = "<a href=""edit_note.aspx?action=edit&type=note&id=" & Trim(Request("nextNote")) & IIf(TempNextHold > 0, "&nextNote=" & TempNextHold.ToString, "") & IIf(TempPreviousHold > 0, "&previousNote=" & TempPreviousHold.ToString, "") & """ class=""float_right"">Next Note ></a>"
                                End If
                            End If
                        End If


                        If Not IsNothing(Trim(Request("previousNote"))) Then
                            If IsNumeric(Trim(Request("previousNote"))) Then
                                If Trim(Request("previousNote")) > 0 Then
                                    NotesTemp = NotesHold
                                    Notes_Search = New DataTable
                                    Notes_Search = NotesHold.Clone
                                    Dim TempNextHold As Integer = 0
                                    Dim TempPreviousHold As Integer = 0
                                    Dim afiltered_Client As DataRow() = NotesTemp.Select("lnote_id=" & Trim(Request("previousNote")), "")
                                    ' extract and import
                                    For Each atmpDataRow_Client In afiltered_Client
                                        Notes_Search.ImportRow(atmpDataRow_Client)
                                    Next

                                    If Not IsNothing(Notes_Search) Then
                                        If Notes_Search.Rows.Count > 0 Then
                                            If Notes_Search.Rows(0).Item("lnote_next_id") > 0 Then
                                                TempNextHold = Notes_Search.Rows(0).Item("lnote_next_id")
                                            End If
                                            If Notes_Search.Rows(0).Item("lnote_previous_id") > 0 Then
                                                TempPreviousHold = Notes_Search.Rows(0).Item("lnote_previous_id")
                                            End If
                                        End If
                                    End If
                                    PreviousNextText += "<a href=""edit_note.aspx?action=edit&type=note&id=" & Trim(Request("previousNote")) & IIf(TempNextHold > 0, "&nextNote=" & TempNextHold.ToString, "") & IIf(TempPreviousHold > 0, "&previousNote=" & TempPreviousHold.ToString, "") & """ class=""float_left"">< Previous Note</a>"
                                End If
                            End If
                        End If

                        If PreviousNextText <> "" Then
                            PreviousNextText += "<br clear=""all"" />"
                        End If
                        next_previous_note_link.Text = PreviousNextText

                        If Trim(Request("type")) = "value_analysis" Then
                            aircraftForSaleBlock.Visible = False
                            Me.estimated_value_tr.Visible = True
                            Me.category_cell.Visible = False
                            Me.category_cell2.Visible = False
                            Me.time.Visible = False
                            Me.date_time_label.Text = "Estimate Date:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                            Me.curprev.Visible = False
                            Me.notes_edit.Height = 100
                            Me.action_item_lbl.Visible = False
                            If Not IsPostBack Then
                                Me.curprev.SelectedValue = "P"
                                current.Visible = False
                                note_date.Visible = True
                                note_date_image.Visible = True
                                note_date.Text = FormatDateTime(Now(), 2)
                                RequiredFieldValidator1.Enabled = True
                            End If
                            TypeOfNote = "value_analysis"
                            'LoadAcStatusChangeJS()

                            'clsGeneral.clsGeneral.WriteJqueryForAircraftEditBlocks(Page, ac_sale, ac_status_not_for_sale, ac_status_for_sale, CompareValidator1, date_listed_panel, date_listed, DOMlisted, DOMWord, est_label, cliaircraft_value_description_text, est_price, broker_price, broker_lbl, asking_price, asking_wordage, ask_lbl, New RadioButtonList)
                            'System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "runJQuery", "window.onload = function() {askingWordageChange();acSaleChanged(); };", True)

                            If HttpContext.Current.Session.Item("localUser").crmUserType <> eUserTypes.MyNotesOnly Then
                                aTempTable = aclsData_Temp.Get_AllClientUser_Active("Y")
                                If Not IsNothing(aTempTable) Then
                                    If aTempTable.Rows.Count > 0 Then
                                        For Each q As DataRow In aTempTable.Rows
                                            pertaining_to.Items.Add(New ListItem(q("cliuser_first_name") & " " & q("cliuser_last_name"), q("cliuser_id")))
                                        Next
                                    End If
                                Else
                                    If aclsData_Temp.class_error <> "" Then
                                        error_string = aclsData_Temp.class_error
                                        clsGeneral.clsGeneral.LogError("edit_note.ascx.vb -Fill_All_DropDowns() - " & error_string, aclsData_Temp)
                                    End If
                                End If
                            ElseIf HttpContext.Current.Session.Item("localUser").crmUserType = eUserTypes.MyNotesOnly Then
                                pertaining_to.Items.Add(New ListItem(Session.Item("localUser").crmLocalUserFirstName & " " & Session.Item("localUser").crmLocalUserLastName, HttpContext.Current.Session.Item("localUser").crmLocalUserID))
                            End If

                            If Not IsPostBack Then
                                If Trim(Request("clival_id")) <> "" Then
                                    aTempTable = aclsData_Temp.Get_Open_Market_Valuation(0, Trim(Request("clival_id")))
                                    If Not IsNothing(aTempTable) Then
                                        If aTempTable.Rows.Count > 0 Then
                                            If Not IsNothing(aTempTable) Then
                                                For Each r As DataRow In aTempTable.Rows

                                                    Dim aclsUpdate_Client_Transactions As New clsClient_Transactions


                                                    If Not IsDBNull(r("lnote_note")) Then
                                                        Me.notes_edit.Text = r("lnote_note")
                                                    End If
                                                    If Not IsDBNull(r("clival_type")) Then
                                                        Me.estval_type_of.SelectedValue = r("clival_type")

                                                        If Session.Item("localSubscription").crmAppraiser_Flag = True Then
                                                            If estval_type_of.SelectedValue = "F" Or estval_type_of.SelectedValue = "D" Then
                                                                Me.authorization_panel.Visible = True
                                                            Else
                                                                Me.authorization_panel.Visible = False
                                                            End If
                                                        End If
                                                    End If
                                                    If Not IsDBNull(r("asking_price")) Then
                                                        Me.estval_asking_price.Text = r("asking_price")
                                                        aclsUpdate_Client_Transactions.clitrans_asking_price = Me.estval_asking_price.Text
                                                    End If
                                                    If Not IsDBNull(r("take_price")) Then
                                                        Me.estval_take_price.Text = r("take_price")
                                                        aclsUpdate_Client_Transactions.clitrans_est_price = Me.estval_take_price.Text
                                                    End If
                                                    If Not IsDBNull(r("sold_price")) Then
                                                        Me.estval_estimated_value.Text = r("sold_price")
                                                        aclsUpdate_Client_Transactions.clitrans_sold_price = Me.estval_estimated_value.Text
                                                    End If
                                                    If Not IsDBNull(r("clival_aftt_hours")) Then
                                                        Me.estval_aftt.Text = r("clival_aftt_hours")
                                                        aclsUpdate_Client_Transactions.clitrans_airframe_total_hours = Me.estval_aftt.Text
                                                    End If
                                                    If Not IsDBNull(r("clival_total_landings")) Then
                                                        Me.estval_total_landings.Text = r("clival_total_landings")
                                                        aclsUpdate_Client_Transactions.clitrans_airframe_total_landings = Me.estval_total_landings.Text
                                                    End If
                                                    If Not IsDBNull(r("lnote_user_name")) Then
                                                        Me.pertaining_to.Text = r("lnote_user_name")
                                                    End If
                                                    If Not IsDBNull(r("clival_entry_date")) Then
                                                        Me.note_date.Text = r("clival_entry_date")
                                                        aclsUpdate_Client_Transactions.clitrans_date = Me.note_date.Text
                                                    End If

                                                    If Not IsDBNull(r("lnote_jetnet_ac_id")) Then
                                                        aclsUpdate_Client_Transactions.clitrans_jetnet_ac_id = r("lnote_jetnet_ac_id")
                                                    End If

                                                    acval_id.Text = aclsData_Temp.Find_Aircraft_Value_ID(aclsUpdate_Client_Transactions, 0)
                                                    If CInt(acval_id.Text) > 0 Then
                                                        Me.authorize_check.Checked = True
                                                    Else
                                                        Me.authorize_check.Checked = False
                                                    End If
                                                Next
                                            End If
                                        End If
                                    End If
                                End If
                            End If

                        ElseIf Session.Item("Listing") = "3" Then
                            aircraftForSaleBlock.Visible = True
                            asking_wordage.Attributes.Add("onChange", "askingWordageChange()")
                            ac_sale.Attributes.Add("onChange", "acSaleChanged()")
                            LoadAcStatusChangeJS()

                            clsGeneral.clsGeneral.WriteJqueryForAircraftEditBlocks(Page, ac_sale, ac_status_not_for_sale, ac_status_for_sale, CompareValidator1, date_listed_panel, date_listed, DOMlisted, DOMWord, est_label, cliaircraft_value_description_text, est_price, broker_price, broker_lbl, asking_price, asking_wordage, ask_lbl, New RadioButtonList)
                            'System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "runJQuery", "window.onload = function() {askingWordageChange();acSaleChanged(); };", True)
                            System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "PostbackRun", "Sys.Application.add_load(function() {askingWordageChange();acSaleChanged();});", True)
                        Else
                            aircraftForSaleBlock.Visible = False
                        End If

                End Select

                If Session.Item("isMobile") = True Then
                    resize_function.Text = "<script type=""text/javascript"">function FitPic() { };</script>"
                    notes_edit.Width = 320
                    contact_related.Width = 300
                    mobile_style.Visible = True
                    company_name.Width = 300
                    aircraft_name.Width = 300
                    'mobile_close.Text = "<img src=""images/cancel.gif"" alt=""Cancel"" border=""0""  onClick='history.go(-1)'/>"
                End If



                If Not Page.IsPostBack Then
                    clsGeneral.clsGeneral.Set_IDS(aclsData_Temp)
                End If

                Dim source As String = Session.Item("ListingSource")


                If Not Page.IsPostBack Then
                    Select Case Trim(Request("action"))
                        Case "edit" 'Edit Mode for Notes. 
                            upload_area.Visible = False
                            Dim idnum As Long = Trim(Request("id"))
                            If UCase(TypeOfNote) = "NOTE" Then
                                make_note_cookie(idnum)
                            End If
                            edit_table.Visible = True
                            removeNoteLB.Visible = True


                            If UCase(TypeOfNote) = "VALUATION" Then
                                DisplayCurrentValuationData(idnum)
                            End If

                            If TypeOfNote = "value_analysis" Then
                                'We already ran an applicable query up above to fill this table in
                                RequiredFieldValidator3.Enabled = False
                            Else

                                If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Or Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
                                    aTempTable = aclsData_Temp.Notes_Search_For_Prospect_View_Homebase("", "", "", "B','O", 0, "", "", "", "", "", "", 0, 0, "", False, False, "", "", idnum)
                                Else
                                    aTempTable = aclsData_Temp.Get_Local_Notes_Client_NoteID(idnum)
                                End If
                            End If


                            If Not IsNothing(aTempTable) Then
                                If aTempTable.Rows.Count > 0 Then

                                    If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Or Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
                                        Call guts_new_prospect()
                                    Else
                                        Call guts_normal_prospect()
                                    End If


                                End If
                            End If

                            If Session.Item("localUser").crmUserType = eUserTypes.RESEARCH Then
                                add_noteLB.Visible = False
                            End If


                        Case "new"

                            Dim jetnet_ac_id As Integer = 0
                            Dim client_ac_id As Integer = 0
                            Dim jetnet_comp_id As Integer = 0
                            Dim client_comp_id As Integer = 0
                            Dim jetnet_contact_id As Integer = 0
                            Dim client_contact_id As Integer = 0
                            Dim jetnet_mod_id As Integer = 0
                            Dim client_mod_id As Integer = 0


                            If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Or Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
                                Call MPM_Edit_Page_Load_Visibility(Trim(Request("comp_id")), "", 0)

                                If Not IsNothing(HttpContext.Current.Session.Item("homebaseUserClass").home_user_id) Then
                                    If Trim(HttpContext.Current.Session.Item("homebaseUserClass").home_user_id) <> "" Then
                                        Me.pertaining_to.SelectedValue = HttpContext.Current.Session.Item("homebaseUserClass").home_user_id
                                    End If
                                End If
                            Else

                            End If




                            Select Case Session.Item("Listing")
                                Case 1 'Company
                                    Select Case Session.Item("ListingSource")
                                        Case "JETNET"
                                            jetnet_comp_id = Session.Item("ListingID")
                                            jetnet_comp.Text = Session.Item("ListingID")
                                            If Session.Item("OtherID") <> 0 Then
                                                client_comp_id = Session.Item("OtherID")
                                            End If
                                        Case "CLIENT"
                                            client_comp_id = Session.Item("ListingID")
                                            If Session.Item("OtherID") <> 0 Then
                                                jetnet_comp_id = Session.Item("OtherID")
                                            End If
                                    End Select
                                    If Session.Item("ContactID") <> 0 Then
                                        Select Case Session.Item("ListingSource")
                                            Case "JETNET"
                                                jetnet_contact_id = Session.Item("ContactID")
                                                'We need to get the client contact ID if it exists for this!
                                                Dim atemptable = aclsData_Temp.GetContactInfo_JETNET_ID(Session.Item("ContactID"), "Y")
                                                If Not IsNothing(atemptable) Then
                                                    If atemptable.rows.count > 0 Then
                                                        client_contact_id = atemptable.rows(0).item("contact_id")
                                                    End If
                                                End If

                                            Case "CLIENT"
                                                client_contact_id = Session.Item("ContactID")
                                                Dim atemptable = aclsData_Temp.GetContacts_Details(Session.Item("ContactID"), "CLIENT")
                                                If Not IsNothing(atemptable) Then
                                                    If atemptable.rows.count > 0 Then
                                                        jetnet_contact_id = atemptable.rows(0).item("contact_jetnet_contact_id")
                                                    End If
                                                End If
                                        End Select
                                    End If
                                Case 3 'Aircraft
                                    Select Case Session.Item("ListingSource")
                                        Case "JETNET"
                                            jetnet_ac_id = Session.Item("ListingID")
                                            If Session.Item("OtherID") <> 0 Then
                                                client_ac_id = Session.Item("OtherID")
                                            End If
                                        Case "CLIENT"
                                            client_ac_id = Session.Item("ListingID")
                                            If Session.Item("OtherID") <> 0 Then
                                                jetnet_ac_id = Session.Item("OtherID")
                                            End If
                                    End Select
                            End Select


                            If LCase(Trim(Request("from"))) = "view" Then
                                'Adding a special check to see what happens.

                                If LCase(Trim(Request("from"))) = "view" Then
                                    If Not IsNothing(Trim(Request("Prospect_Client_Comp_ID"))) Then
                                        If IsNumeric(Trim(Request("Prospect_Client_Comp_ID"))) Then
                                            client_comp_id = Trim(Request("Prospect_Client_Comp_ID"))
                                        End If
                                    End If
                                    If Not IsNothing(Trim(Request("Prospect_Jetnet_Comp_ID"))) Then
                                        If IsNumeric(Trim(Request("Prospect_Jetnet_Comp_ID"))) Then
                                            jetnet_comp_id = Trim(Request("Prospect_Jetnet_Comp_ID"))
                                        End If
                                    End If
                                End If
                                If Not IsNothing(Trim(Request("source"))) Then
                                    If Not IsNothing(Trim(Request("ac_ID"))) Then
                                        If IsNumeric(Trim(Request("ac_ID"))) Then
                                            Select Case Trim(Request("source"))

                                                Case "JETNET"
                                                    jetnet_ac_id = Trim(Request("ac_ID"))
                                                    client_ac_id = 0
                                                Case Else
                                                    client_ac_id = Trim(Request("ac_ID"))
                                                    jetnet_ac_id = 0
                                            End Select

                                        End If
                                    End If
                                End If
                            End If

                            jetnet_ac.Text = jetnet_ac_id
                            client_ac.Text = client_ac_id
                            jetnet_comp.Text = jetnet_comp_id
                            client_comp.Text = client_comp_id
                            jetnet_contact.Text = jetnet_contact_id
                            client_contact.Text = client_contact_id

                            ToggleProspectAttachRadioButton()

                            RaiseEvent fill_drop(jetnet_ac_id, client_ac_id, jetnet_comp_id, client_comp_id, jetnet_contact_id, client_contact_id, jetnet_mod_id, client_mod_id, Me, IIf(UCase(attach_prospect_by.SelectedValue) = "MODEL", True, False))
                            If UCase(TypeOfNote) = "PROSPECT" Then
                                CheckForAutoInsertOppCategoryDefaultIsntThere()
                                notes_opp.SelectedValue = notes_opp.Items.FindByText("Not Specified").Value
                            End If


                            'Run this once just to see if we had a company/aircraft when we came into the page.
                            'This just makes sure that the prospect panel is available from the prospector view.
                            EvaluateAutomaticProspectAvailability()
                            If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Or Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
                                curprev.SelectedValue = "P"
                            Else
                                curprev.Items.Add(New ListItem("Current Date", "N"))
                            End If

                            If Trim(Request("type")) = "value_analysis" Then
                                RequiredFieldValidator3.Enabled = False
                            Else
                                curprev.SelectedValue = "N"
                                current.Visible = True
                                current.Text = Now()
                                note_date.Visible = False
                                note_date_image.Visible = False
                                time.Visible = False
                            End If



                            AC_Search_Vis.Visible = True
                            company_search_vis.Visible = True
                            company_related.Visible = False
                            removeNoteLB.Visible = False
                            Try
                                pertaining_to.SelectedValue = Session.Item("localUser").crmLocalUserID
                            Catch
                            End Try

                            Try
                                notes_cat.SelectedValue = Trim(Request("cat_key"))
                            Catch
                            End Try

                            ' ADDED MSW - 12/3/19
                            If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Or Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
                                add_note_automatically.Visible = False
                                add_noteLB.Visible = False
                                MPM_Prospect_edit.Visible = True
                                aircraft_model_prospect_swap.Visible = False
                            End If

                    End Select

                    If Session.Item("localUser").crmUserType = eUserTypes.RESEARCH Then
                        removeNoteLB.Visible = False
                    End If
                End If


                notes_edit.Focus()
                If Trim(Request("temporary")) = "true" Then
                    If valuationExists = False Then
                        Page.Validate()

                        add_note_Click()
                    End If
                End If



                If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Or Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
                    Call page_invisibles_mpm(0)
                End If


            Catch ex As Exception
                error_string = "Notes.ascx.vb - Page_Load() - " & ex.Message
                clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
            End Try
        End If
    End Sub
    Public Sub page_invisibles_mpm(ByVal comp_id As Long)
        '-----FILL IN THE ITEMS NEEDED --------------------------------
        aircraft_information_panel.Visible = False
        aircraft_model_prospect_swap.Visible = False
        visible_all.Visible = False
        add_noteLB.Visible = False
        MPM_Prospect_edit.Visible = True
        prospectOppRow.Visible = False
        CompareValidator1.Visible = False
        time.Visible = False

        current.Visible = False
        add_note_automatically.Visible = False
        add_note_automatically_checkbox.Visible = False
        start_table_row.Visible = True
        RequiredFieldValidator1.Visible = False
        curprev.Visible = False
        RequiredFieldValidator1.Visible = False
        CompareValidator1.Visible = False
        CustomValidator1.Visible = False

        ' the panel 
        notesdate.Visible = False
        next_action_panel.Visible = True
        next_action_date_label.Visible = True


        note_date.Visible = False
        note_date.Enabled = False

        If comp_id > 0 Then
            customer_activities_panel.Visible = True
        End If
    End Sub
    Public Sub MPM_Edit_Page_Load_Visibility(ByVal comp_id As Long, ByVal user_id As String, ByVal contact_id As Long)

        Call page_invisibles_mpm(comp_id)

        ' CALL THIS, MAINLY JUST FOR COMPANY ITEMS, OTHERSR WILL BE OVERWRITTEN
        RaiseEvent fill_drop(0, 0, comp_id, 0, contact_id, 0, 0, 0, Me, IIf(UCase(attach_prospect_by.SelectedValue) = "MODEL", True, False))


        Dim crmTestMaster As New main_site
        crmTestMaster.aclsData_Temp.JETNET_DB = Application.Item("crmJetnetDatabase")
        crmTestMaster.aclsData_Temp.client_DB = Application.Item("crmClientDatabase")

        pertaining_to.Items.Clear()

        clsGeneral.clsGeneral.FillCRMUser_Homebase(crmTestMaster, "Prospects", pertaining_to, False, user_id)

        '--- NEW VERSION - Notes_opp dropdown is service  
        Me.type_original_label.Text = "Service"
        crmTestMaster.aclsData_Temp.Fill_Service_Category(notes_opp, New DataTable, aclsData_Temp)
        notes_opp.Items.Add(New ListItem("ALL", "ALL"))


        source_dropdown.Items.Add(New ListItem("ALL", "ALL"))
        crmTestMaster.aclsData_Temp.Fill_Source_Drop(source_dropdown, New DataTable, aclsData_Temp)

        clsGeneral.clsGeneral.FillCRMUser_Homebase(crmTestMaster, "Prospects", referral_drop, False, "")

        ' NEW VERSION --- TYPE DROPDOWN IS PRIORTY  
        Me.action_label.Text = "Stage"
        crmTestMaster.aclsData_Temp.Fill_Type_Category(priorityID, aclsData_Temp)
        priorityID.SelectedValue = "ALL"
        '------------------------------------------------
    End Sub

    Public Sub Fill_Customer_Activities_FromView(ByVal CompanyID As Long, ByVal JournalID As Long)
        Dim user_table As New DataTable
        Dim htmlOut As New StringBuilder
        'certifications_label.Visible = True
        Dim temp_desc As String = ""
        Dim toggleAllActivities As Boolean = False

        Dim helperClass As New displayCompanyDetailsFunctions

        'If sTask.ToLower.Contains("showall") Then
        '    toggleAllActivities = True
        '    showTop50Activities.Visible = True
        '    showAllActivities.Visible = False
        'Else
        toggleAllActivities = False
        showTop50Activities.Visible = False
        showAllActivities.Visible = True
        '  End If 

        user_table = helperClass.Return_Customer_Actions_Summary(CompanyID, JournalID, "N", toggleAllActivities, customerActivitiesFilter.SelectedValue)

        If Not IsNothing(user_table) Then
            customerActivities_Label.Text = helperClass.DisplayCustomerActivitiesTable(user_table, CompanyID, 0)
        End If
    End Sub

    Public Sub guts_new_prospect()

        If Not IsDBNull(aTempTable.Rows(0).Item("cprospect_contact_id")) Then
            Call MPM_Edit_Page_Load_Visibility(aTempTable.Rows(0).Item("comp_id"), aTempTable.Rows(0).Item("cprospect_user_id"), aTempTable.Rows(0).Item("cprospect_contact_id"))
        Else
            Call MPM_Edit_Page_Load_Visibility(aTempTable.Rows(0).Item("comp_id"), aTempTable.Rows(0).Item("cprospect_user_id"), 0)
        End If


        Call Fill_Customer_Activities_FromView(aTempTable.Rows(0).Item("comp_id"), 0)


        ' notes_cat.SelectedValue = aTempTable.Rows(0).Item("lnote_notecat_key")
        ' notes_edit.Text = HttpUtility.HtmlDecode(aTempTable.Rows(0).Item("lnote_note"))
        ' jetnet_ac.Text = aTempTable.Rows(0).Item("lnote_jetnet_ac_id")
        ' client_ac.Text = aTempTable.Rows(0).Item("lnote_client_ac_id")
        jetnet_comp.Text = aTempTable.Rows(0).Item("comp_id")
        ' client_comp.Text = aTempTable.Rows(0).Item("lnote_client_comp_id")
        '  client_contact.Text = aTempTable.Rows(0).Item("lnote_client_contact_id")
        '  jetnet_contact.Text = aTempTable.Rows(0).Item("lnote_jetnet_contact_id")
        '  client_mod.Text = aTempTable.Rows(0).Item("lnote_client_amod_id")
        '  jetnet_mod.Text = aTempTable.Rows(0).Item("lnote_jetnet_amod_id") 

        ' added in msw - 6/15/20
        If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Or Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
            jetnet_contact.Text = aTempTable.Rows(0).Item("cprospect_contact_id")
        End If

        '  crmTestMaster.aclsData_Temp.Fill_Service_Category(crmProspectSearchCategory, New DataTable, aclsData_Temp)
        '  crmProspectActionTakenList.Items.Add(New ListItem("ALL", "ALL"))


        If LCase(TypeOfNote) = "prospect" Then

            'If Not IsDBNull(aTempTable.Rows(0).Item("lnote_clipri_ID")) Then
            '    Try
            '        priorityID.SelectedValue = aTempTable.Rows(0).Item("lnote_clipri_ID")
            '    Catch
            '    End Try
            'End If 

            If Not IsDBNull(aTempTable.Rows(0).Item("cprospect_id")) Then
                acval_id.Text = aTempTable.Rows(0).Item("cprospect_id")
            End If


            If Not IsDBNull(aTempTable.Rows(0).Item("cprospect_status")) Then
                If Trim(aTempTable.Rows(0).Item("cprospect_status")) = "Active" Then
                    opp_status.SelectedValue = "A"
                    target_label.Text = "Target Closing Date"
                ElseIf Trim(aTempTable.Rows(0).Item("cprospect_status")) = "Inactive" Then
                    opp_status.SelectedValue = "I"
                    target_label.Text = "Target Closing Date"
                ElseIf Trim(aTempTable.Rows(0).Item("cprospect_status")) = "Closed" Then
                    opp_status.SelectedValue = "C"
                    target_label.Text = "Date Closed"
                End If
            End If

            If Not IsDBNull(aTempTable.Rows(0).Item("cprospect_value")) Then
                opp_cash.Text = aTempTable.Rows(0).Item("cprospect_value")
            End If
            If Not IsDBNull(aTempTable.Rows(0).Item("cprospect_target_date")) Then
                targetdate.Text = aTempTable.Rows(0).Item("cprospect_target_date")
            End If

            If Not IsDBNull(aTempTable.Rows(0).Item("cprospect_start_date")) Then
                start_date.Text = aTempTable.Rows(0).Item("cprospect_start_date")
            End If

            next_action_date_label.Text = ""
            If Not IsDBNull(aTempTable.Rows(0).Item("cprospect_next_action_date")) Then
                'note_date.Text = aTempTable.Rows(0).Item("cprospect_next_action_date")
                next_action_date_label.Text = "Next Action Date/Time:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" & aTempTable.Rows(0).Item("cprospect_next_action_date")

                If Not IsDBNull(aTempTable.Rows(0).Item("cprospect_next_action")) Then ' the note 
                    next_action_date_label.Text &= "&nbsp;&nbsp;-&nbsp;&nbsp;" & aTempTable.Rows(0).Item("cprospect_next_action")
                End If
            End If

            If Not IsDBNull(aTempTable.Rows(0).Item("cprospect_percent_win")) Then
                capt_per.SelectedValue = aTempTable.Rows(0).Item("cprospect_percent_win")
            End If

            If Not IsDBNull(aTempTable.Rows(0).Item("cprospect_details")) Then
                'If InStr(HttpUtility.HtmlDecode(aTempTable.Rows(0).Item("cprospect_details")), " ::: ") Then
                '    Dim text As Array = Split(HttpUtility.HtmlDecode(aTempTable.Rows(0).Item("cprospect_details")), " ::: ")
                '    notes_edit.Text = text(1)
                '    notes_title.Text = text(0)
                'Else
                notes_edit.Text = HttpUtility.HtmlDecode(aTempTable.Rows(0).Item("cprospect_details"))
                '  End If
            End If



            '--- NEW VERSION - Notes_opp dropdown is service
            If Not IsDBNull(aTempTable.Rows(0).Item("cprospect_service")) Then
                notes_opp.Text = aTempTable.Rows(0).Item("cprospect_service")
            End If

            ' NEW VERSION --- TYPE DROPDOWN IS PRIORTY 
            If Not IsDBNull(aTempTable.Rows(0).Item("cprospect_type")) Then
                priorityID.SelectedValue = aTempTable.Rows(0).Item("cprospect_type")
            End If


            If Not IsDBNull(aTempTable.Rows(0).Item("cpsource_id")) Then
                source_dropdown.SelectedValue = aTempTable.Rows(0).Item("cpsource_id")
            End If

            If Not IsDBNull(aTempTable.Rows(0).Item("cprospect_referrer_user_id")) Then
                referral_drop.SelectedValue = aTempTable.Rows(0).Item("cprospect_referrer_user_id")
            End If



        End If




    End Sub
    Public Sub guts_normal_prospect()

        notes_cat.SelectedValue = aTempTable.Rows(0).Item("lnote_notecat_key")
        notes_edit.Text = HttpUtility.HtmlDecode(aTempTable.Rows(0).Item("lnote_note"))
        jetnet_ac.Text = aTempTable.Rows(0).Item("lnote_jetnet_ac_id")
        client_ac.Text = aTempTable.Rows(0).Item("lnote_client_ac_id")
        jetnet_comp.Text = aTempTable.Rows(0).Item("lnote_jetnet_comp_id")
        client_comp.Text = aTempTable.Rows(0).Item("lnote_client_comp_id")
        client_contact.Text = aTempTable.Rows(0).Item("lnote_client_contact_id")
        jetnet_contact.Text = aTempTable.Rows(0).Item("lnote_jetnet_contact_id")
        client_mod.Text = aTempTable.Rows(0).Item("lnote_client_amod_id")
        jetnet_mod.Text = aTempTable.Rows(0).Item("lnote_jetnet_amod_id")

        If LCase(TypeOfNote) = "prospect" Then

            If Not IsDBNull(aTempTable.Rows(0).Item("lnote_clipri_ID")) Then
                Try
                    priorityID.SelectedValue = aTempTable.Rows(0).Item("lnote_clipri_ID")
                Catch
                End Try
            End If


            If Not IsDBNull(aTempTable.Rows(0).Item("lnote_cash_value")) Then
                opp_cash.Text = aTempTable.Rows(0).Item("lnote_cash_value")
            End If
            If Not IsDBNull(aTempTable.Rows(0).Item("lnote_schedule_start_date")) Then
                targetdate.Text = aTempTable.Rows(0).Item("lnote_schedule_start_date")
            End If
            If Not IsDBNull(aTempTable.Rows(0).Item("lnote_capture_percentage")) Then
                capt_per.SelectedValue = aTempTable.Rows(0).Item("lnote_capture_percentage")
            End If
            If Not IsDBNull(aTempTable.Rows(0).Item("lnote_opportunity_status")) Then
                opp_status.SelectedValue = aTempTable.Rows(0).Item("lnote_opportunity_status")
            End If
            If Not IsDBNull(aTempTable.Rows(0).Item("lnote_note")) Then
                If InStr(HttpUtility.HtmlDecode(aTempTable.Rows(0).Item("lnote_note")), " ::: ") Then
                    Dim text As Array = Split(HttpUtility.HtmlDecode(aTempTable.Rows(0).Item("lnote_note")), " ::: ")
                    notes_edit.Text = text(1)
                    notes_title.Text = text(0)
                Else
                    notes_edit.Text = HttpUtility.HtmlDecode(aTempTable.Rows(0).Item("lnote_note"))
                End If
            End If
        End If

        If LCase(TypeOfNote) <> "valuation" Then
            'add_note.ImageUrl = "~/images/edit.jpg"
        End If

        ToggleProspectAttachRadioButton()

        RaiseEvent fill_drop(aTempTable.Rows(0).Item("lnote_jetnet_ac_id"), aTempTable.Rows(0).Item("lnote_client_ac_id"), aTempTable.Rows(0).Item("lnote_jetnet_comp_id"), aTempTable.Rows(0).Item("lnote_client_comp_id"), aTempTable.Rows(0).Item("lnote_jetnet_contact_id"), aTempTable.Rows(0).Item("lnote_client_contact_id"), aTempTable.Rows(0).Item("lnote_jetnet_amod_id"), aTempTable.Rows(0).Item("lnote_client_amod_id"), Me, IIf(UCase(attach_prospect_by.SelectedValue) = "MODEL", True, False))

        If aTempTable.Rows(0).Item("lnote_status") = "B" Then
            notes_opp.SelectedValue = aTempTable.Rows(0).Item("lnote_notecat_key")

        Else
            notes_cat.SelectedValue = aTempTable.Rows(0).Item("lnote_notecat_key")
        End If

        If Not IsDBNull(aTempTable.Rows(0).Item("lnote_user_id")) Then

            If aTempTable.Rows(0).Item("lnote_user_id") = 0 Then
                pertaining_to.SelectedValue = Session.Item("localUser").crmLocalUserID
            Else
                Try
                    pertaining_to.SelectedValue = aTempTable.Rows(0).Item("lnote_user_id")
                Catch ex As Exception
                    pertaining_to.SelectedValue = Session.Item("localUser").crmLocalUserID
                End Try
            End If

        Else
            pertaining_to.SelectedValue = Session.Item("localUser").crmLocalUserID
        End If


        Dim timed As String = ""
        Try
            Dim offset As Date = aTempTable.Rows(0).Item("lnote_entry_date")
            offset = DateAdd("h", Session("timezone_offset"), offset)
            timed = Format(offset, "HH:00")
            time.SelectedValue = CStr(timed)
        Catch ex As Exception
            error_string = "Notes.ascx.vb - Page_Load() - " & ex.Message
            clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
        End Try

        If aTempTable.Rows(0).Item("lnote_opportunity_status") = "O" Or aTempTable.Rows(0).Item("lnote_opportunity_status") = "A" Then
            opp_status.SelectedValue = "A" 'IIf(Not IsDBNull(aTempTable.Rows(0).Item("lnote_opportunity_status")), aTempTable.Rows(0).Item("lnote_opportunity_status"), "O")
        ElseIf aTempTable.Rows(0).Item("lnote_opportunity_status") = "C" Then
            TargetDateText.Text = "Closing Date"
            opp_status.SelectedValue = "C"
        Else
            TargetDateText.Text = "Closing Date"
            opp_status.SelectedValue = "I"
        End If



        If aTempTable.Rows(0).Item("lnote_status") <> "V" Then
            note_date.Text = FormatDateTime(aTempTable.Rows(0).Item("lnote_entry_date"), 2)
            curprev.SelectedValue = "P"
        Else
            note_date.Visible = False
            curprev.Visible = False
            time.Visible = False
            note_date_image.Visible = False
            current.Visible = True
            current.Text = aTempTable.Rows(0).Item("lnote_action_date")


            If Not IsDBNull(aTempTable.Rows(0).Item("lnote_opportunity_status")) Then

                If aTempTable.Rows(0).Item("lnote_opportunity_status") = "C" Then
                    opp_status.SelectedValue = "I"
                    add_noteLB.Enabled = False
                    add_noteLB.Visible = False
                    notesdate.Visible = True

                    aircraft_related.Enabled = False
                    aircraft_name.Enabled = False

                    company_name.Enabled = False
                    contact_name.Enabled = False
                    opp_status.Enabled = False
                    pertaining_to.Enabled = False
                Else
                    opp_status.SelectedValue = "A"
                End If
            End If



        End If

    End Sub
#End Region
    Private Sub email_pertaining_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles email_pertaining.CheckedChanged
        If email_pertaining.Checked = True Then
            cc_row.Visible = True
        Else
            cc_row.Visible = False
        End If
    End Sub
    Private Sub CheckForAutoInsertOppCategoryDefaultIsntThere()
        If IsNothing(notes_opp.Items.FindByText("Not Specified")) Then
            If aclsData_Temp.Insert_Opportunity_Categories("Not Specified") = 1 Then
                'rebind the roll
                notes_opp.Items.Clear()
                clsGeneral.clsGeneral.Fill_Opportunity_Category(notes_opp, aTempTable, aclsData_Temp)
            Else
                If aclsData_Temp.class_error <> "" Then
                    error_string = aclsData_Temp.class_error
                    clsGeneral.clsGeneral.LogError("edit_note.ascx.vb - CheckForAutoInsertOppCategoryIfBlank() - " & error_string, aclsData_Temp)
                End If
            End If
        End If
    End Sub
    Private Sub ToggleProspectAttachRadioButton()
        If UCase(TypeOfNote) = "PROSPECT" Then
            If Not Page.IsPostBack Then
                If jetnet_ac.Text <> "0" Or client_ac.Text <> "0" Then
                    'default to aircraft.
                    attach_prospect_by.SelectedValue = "AIRCRAFT"
                    aircraft_information_panel.Visible = True
                    model_information_panel.Visible = False
                ElseIf jetnet_mod.Text <> "0" Or client_mod.Text <> "0" Then
                    'default to model.
                    attach_prospect_by.SelectedValue = "MODEL"
                    aircraft_information_panel.Visible = False
                    model_information_panel.Visible = True
                ElseIf Trim(Request("action")) = "edit" Then
                    'default to neither.
                    attach_prospect_by.SelectedValue = "NEITHER"
                    aircraft_information_panel.Visible = False
                    model_information_panel.Visible = False
                ElseIf LCase(Trim(Request("refreshing"))) = "prospect" Or LCase(Trim(Request("from"))) = "companydetails" Then
                    'default to neither.
                    attach_prospect_by.SelectedValue = "NEITHER"
                    aircraft_information_panel.Visible = False
                    model_information_panel.Visible = False
                Else
                    'default to aircraft
                    attach_prospect_by.SelectedValue = "AIRCRAFT"
                    aircraft_information_panel.Visible = True
                    model_information_panel.Visible = False
                End If
            End If
        End If
    End Sub
#Region "Deals with dropdown changing, visibility changing based on search type"
    Private Sub aircraft_related_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles aircraft_related.CheckedChanged
        Try
            If aircraft_related.Checked = True Then
                ac_search.Visible = False
            Else
                If ProspectAircraft.Checked = False Then
                    ac_search.Visible = True
                End If
            End If
        Catch ex As Exception
            error_string = "Notes.ascx.vb - aircraft_related_CheckedChanged() - " & ex.Message
            clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
        End Try
    End Sub
    Private Sub contact_related_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles contact_related.CheckedChanged
        Try
            If contact_related.Checked = True Then
                contact_search.Visible = False
            Else
                contact_search.Visible = True
            End If
        Catch ex As Exception
            error_string = "Notes.ascx.vb - contact_related_CheckedChanged() - " & ex.Message
            clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
        End Try
    End Sub
    Private Sub company_related_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles company_related.CheckedChanged
        Try
            If company_related.Checked = True Then
                company_search.Visible = False
            Else
                company_search.Visible = True
            End If
        Catch ex As Exception
            error_string = "Notes.ascx.vb - company_related_CheckedChanged() - " & ex.Message
            clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
        End Try
    End Sub
    Private Sub aircraft_name_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles aircraft_name.SelectedIndexChanged
        Try
            If ac_search.Visible = True Then
                ac_search.Visible = True
            End If
            RaiseEvent Aircraft_Name_Changed(Me, IIf(UCase(attach_prospect_by.SelectedValue) = "MODEL", True, False))

            EvaluateAutomaticProspectAvailability()
        Catch ex As Exception
            error_string = "Notes.ascx.vb - aircraft_name_SelectedIndexChanged() - " & ex.Message
            clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
        End Try
    End Sub
    Private Sub AC_Search_Vis_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AC_Search_Vis.Click
        Try
            If ac_search.Visible = False Then
                AC_Search_Vis.Text = "Toggle AC Search Off"
                ac_search.Visible = True
                aircraft_related.Checked = False
                ProspectAircraft.Checked = False
            Else
                AC_Search_Vis.Text = "Click for AC Search"
                ac_search.Visible = False
                AC_Search_Vis.Visible = True
            End If
        Catch ex As Exception
            error_string = "Notes.ascx.vb - AC_Search_Vis_Click() - " & ex.Message
            clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
        End Try
    End Sub
    Private Sub company_search_vis_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles company_search_vis.Click
        Try
            company_search.Visible = True
            company_search_vis.Visible = False
            ac_search.Visible = False
            ac_search_text.Visible = True
            AC_Search_Vis.Visible = True
        Catch ex As Exception
            error_string = "Notes.ascx.vb - company_search_vis_Click() - " & ex.Message
            clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
        End Try
    End Sub
    Private Sub contact_search_vis_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles contact_search_vis.Click
        Try
            contact_search.Visible = True
            contact_search_vis.Visible = False
        Catch ex As Exception
            error_string = "Notes.ascx.vb - contact_search_vis_Click() - " & ex.Message
            clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
        End Try
    End Sub
    Private Sub ac_search_button_Click() Handles ac_search_buttonLB.Click
        Try


            If Page.IsPostBack Then
                RaiseEvent ac_searchClick(Me)
            End If

            AC_Search_Vis.Text = "Toggle AC Search Off"
            ac_search.Visible = True
            aircraft_related.Visible = True
            ProspectAircraft.Visible = True
            company_search_vis.Visible = True

        Catch ex As Exception
            error_string = "Notes.ascx.vb - ac_search_button_Click() - " & ex.Message
            clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
        End Try
    End Sub
    Public Sub company_search_button_Click() Handles company_search_buttonLB.Click
        Try
            company_search.Visible = True
            company_search_vis.Visible = False

            RaiseEvent company_searchClick(Me)
        Catch ex As Exception
            error_string = "Notes.ascx.vb - company_search_button_Click() - " & ex.Message
            clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
        End Try
    End Sub
    Private Sub company_name_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles company_name.SelectedIndexChanged
        Try
            company_search.Visible = False
            company_search_vis.Visible = True
            RaiseEvent company_name_changed(Me)

            If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Or Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
            Else
                EvaluateAutomaticProspectAvailability()
            End If

        Catch ex As Exception
            error_string = "Notes.ascx.vb - company_name_SelectedIndexChanged() - " & ex.Message
            clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
        End Try
    End Sub
    Private Sub contact_name_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles contact_name.SelectedIndexChanged
        Try
            If company_search.Visible = True Then
                company_search.Visible = True
                company_search_vis.Visible = False
            End If

            RaiseEvent contact_name_changed(Me)
        Catch ex As Exception
            error_string = "Notes.ascx.vb - contact_name_SelectedIndexChanged() - " & ex.Message
            clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
        End Try
    End Sub
    Private Sub EvaluateAutomaticProspectAvailability()
        'Added a check after each post back (dropdown selections of ac and company cause postback)
        'To check and see if a valuation for this company/ac pair exists. If it does, don't toggle the 
        'panel to add one on.


        If Session.Item("localSubscription").crmAerodexFlag = False Then
            If UCase(TypeOfNote) = "NOTE" Or UCase(TypeOfNote) = "PROSPECT" Then
                If Trim(Request("action")) = "new" Then
                    add_noteLB.Visible = True
                    If Not String.IsNullOrEmpty(company_name.SelectedValue) And company_name.SelectedValue <> "|" Then
                        If aircraft_name.SelectedValue <> "0||0" Then
                            Dim ProspectAvailableCheck As New DataTable
                            'If Val(client_ac.Text) > 0 Then

                            ProspectAvailableCheck = aclsData_Temp.ReturnApplicableProspect(Val(jetnet_ac.Text), Val(client_ac.Text), Val(jetnet_comp.Text), Val(client_comp.Text))
                            If Not IsNothing(ProspectAvailableCheck) Then
                                If ProspectAvailableCheck.Rows.Count = 0 Then
                                    If UCase(TypeOfNote) = "NOTE" Then
                                        add_prospect_automatically.Visible = True
                                    Else
                                        add_noteLB.Visible = True
                                    End If
                                Else
                                    If UCase(TypeOfNote) = "NOTE" Then
                                        add_prospect_automatically_checkbox.Checked = False
                                    Else
                                        add_noteLB.Visible = False
                                    End If
                                End If
                            End If
                        Else
                            If UCase(TypeOfNote) = "NOTE" Then
                                add_prospect_automatically_checkbox.Checked = False
                            End If
                        End If
                    Else
                        If UCase(TypeOfNote) = "NOTE" Then
                            add_prospect_automatically_checkbox.Checked = False
                        End If
                    End If
                End If
            End If
        End If
    End Sub
#End Region
#Region "Save Note"


    Public Sub Add_Note_MPM()


        Try

            Dim jetnet_contact As TextBox = Name.FindControl("jetnet_contact")
            ' Dim client_contact As TextBox = Name.FindControl("client_contact")
            ' Dim notes_edit As TextBox = Name.FindControl("notes_edit")
            Dim jetnet_comp As TextBox = Name.FindControl("jetnet_comp")
            ' Dim client_comp As TextBox = Name.FindControl("client_comp")
            ' Dim jetnet_ac As TextBox = Name.FindControl("jetnet_ac")
            ' Dim client_ac As TextBox = Name.FindControl("client_ac")
            ' Dim jetnet_mod As TextBox = Name.FindControl("jetnet_mod")
            ' Dim client_mod As TextBox = Name.FindControl("client_mod")



            'Dim type_of_est_value_drop As DropDownList
            'Dim type_of_est_value As String = ""
            'If Not IsNothing(Name.FindControl("estval_type_of")) Then
            '    type_of_est_value_drop = Name.FindControl("estval_type_of")
            '    type_of_est_value = type_of_est_value_drop.SelectedValue
            'End If


            'Dim action_item_subject As TextBox = Name.FindControl("action_item_subject")


            'Dim notes_title As New TextBox
            'If Not IsNothing(Name.FindControl("notes_title")) Then
            '    notes_title = Name.FindControl("notes_title")
            'End If



            'Dim priority As New DropDownList
            'If Not IsNothing(Name.FindControl("priority")) Then
            '    priority = Name.FindControl("priority")
            'End If
            'If UCase(Type) = "PROSPECT" Then
            '    priority = Name.FindControl("priorityID")
            'End If





            Dim cprospect_comp_id As String = ""


            Dim cprospect_contact_id As String = ""

            If Trim(jetnet_comp.Text) <> "" Then
                If IsNumeric(jetnet_comp.Text) = True Then
                    cprospect_comp_id = Trim(jetnet_comp.Text)
                End If
            End If

            If Trim(jetnet_contact.Text) <> "" Then
                If IsNumeric(jetnet_contact.Text) = True Then
                    cprospect_contact_id = Trim(jetnet_contact.Text)
                End If
            End If


            Dim cprospect_next_action_date As String = ""
            If Not IsNothing(Name.FindControl("note_date")) Then
                Dim note_date As New TextBox
                note_date = Name.FindControl("note_date")
                cprospect_next_action_date = note_date.Text
            End If

            Dim cprospect_target_date As String = ""
            If Not IsNothing(Name.FindControl("targetdate")) Then
                Dim targetDateText As New TextBox
                targetDateText = Name.FindControl("targetdate")
                cprospect_target_date = targetDateText.Text
            End If



            Dim cprospect_start_date As String = ""
            If Not IsNothing(Name.FindControl("start_date")) Then
                Dim start_dateText As New TextBox
                start_dateText = Name.FindControl("start_date")
                cprospect_start_date = start_dateText.Text
            End If

            Dim cprospect_value As Integer = 0
            If Not IsNothing(Name.FindControl("opp_cash")) Then
                Dim opp_cash As TextBox = Name.FindControl("opp_cash")
                If IsNumeric(opp_cash.Text) Then
                    cprospect_value = opp_cash.Text
                End If
            Else
                cprospect_value = 0
            End If

            Dim cprospect_percent_win As Integer = 0
            If Not IsNothing(Name.FindControl("capt_per")) Then
                Dim capt_per As DropDownList = Name.FindControl("capt_per")
                cprospect_percent_win = capt_per.SelectedValue
            Else
                cprospect_percent_win = 0
            End If

            Dim cprospect_details As String = ""
            If Not IsNothing(Name.FindControl("notes_edit")) Then
                Dim store_doc As TextBox = Name.FindControl("notes_edit")
                cprospect_details = store_doc.Text
            Else
                cprospect_details = ""
            End If

            'If Not IsNothing(Name.FindControl("notes_title")) Then
            '    Dim store_doc2 As TextBox = Name.FindControl("notes_title")
            '    cprospect_details = store_doc2.Text & " ::: " & cprospect_details
            'Else
            '    cprospect_details = ""
            'End If


            Dim cprospect_type As String = ""
            If Not IsNothing(Name.FindControl("priorityID")) Then
                Dim store_doc3 As DropDownList = Name.FindControl("priorityID")
                cprospect_type = store_doc3.SelectedValue
            Else
                cprospect_type = ""
            End If



            Dim cprospect_status As String = ""
            If Not IsNothing(Name.FindControl("opp_status")) Then
                Dim store_doc4 As RadioButtonList = Name.FindControl("opp_status")
                cprospect_status = store_doc4.SelectedValue

                If Trim(cprospect_status) = "A" Then
                    cprospect_status = "Active"
                ElseIf Trim(cprospect_status) = "I" Then
                    cprospect_status = "Inactive"
                ElseIf Trim(cprospect_status) = "C" Then
                    cprospect_status = "Closed"
                End If
            Else
                cprospect_status = ""
            End If


            Dim cprospect_service As String = ""
            If Not IsNothing(Name.FindControl("notes_opp")) Then
                Dim store_doc5 As DropDownList = Name.FindControl("notes_opp")
                cprospect_service = store_doc5.Text
            Else
                cprospect_service = ""
            End If


            Dim cprospect_assigned_to As String = ""
            Dim cprospect_user_id As String = ""
            If Not IsNothing(Name.FindControl("pertaining_to")) Then
                Dim store_doc5 As DropDownList = Name.FindControl("pertaining_to")
                cprospect_assigned_to = store_doc5.SelectedItem.Text
                cprospect_user_id = store_doc5.SelectedValue
            Else
                cprospect_assigned_to = ""
                cprospect_user_id = ""
            End If


            Dim cprospect_id As Long = 0
            If Not IsNothing(Name.FindControl("acval_id")) Then
                Dim store_doc5 As Label = Name.FindControl("acval_id")
                If Trim(acval_id.Text) <> "" Then
                    cprospect_id = acval_id.Text
                Else
                    cprospect_id = 0
                End If
            Else
                cprospect_id = 0
            End If



            Dim cprospect_cpsource_id As String = ""
            If Not IsNothing(Name.FindControl("source_dropdown")) Then
                Dim store_doc6 As DropDownList = Name.FindControl("source_dropdown")
                cprospect_cpsource_id = store_doc6.SelectedValue
            Else
                cprospect_cpsource_id = ""
            End If


            Dim cprospect_referrer_user_id As String = ""
            If Not IsNothing(Name.FindControl("referral_drop")) Then
                Dim store_doc7 As DropDownList = Name.FindControl("referral_drop")
                cprospect_referrer_user_id = store_doc7.SelectedValue
            Else
                cprospect_referrer_user_id = ""
            End If



            cprospect_details = Replace(cprospect_details, "'", "''")
            ' cprospect_details = Replace(cprospect_details, "'", "''")

            Call aclsData_Temp.Insert_Update_Notes_Prospect_View_Homebase(cprospect_id, cprospect_service, cprospect_type, cprospect_details, cprospect_assigned_to, cprospect_target_date, cprospect_next_action_date, cprospect_value, cprospect_percent_win, cprospect_status, cprospect_user_id, cprospect_comp_id, cprospect_contact_id, cprospect_start_date, cprospect_cpsource_id, cprospect_referrer_user_id)



            'Either the button on the opener for the view is there and I'm clicking it, or it's not and I am refreshing the opening page. Either way, it doesn't really matter and will work. Clicking the button is nicer on the view, but isn't detrimental if it just refreshes the page.
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_WindowOpener", "if(window.opener.$('#ContentPlaceHolder1_View_Master1_crmProspectSearchButton').length){window.opener.$('#ContentPlaceHolder1_View_Master1_crmProspectSearchButton').trigger('click');} else {window.opener.location.href=window.opener.location.href;};", True)

            'close window.
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)


        Catch ex As Exception

        End Try

    End Sub

    Public Sub add_note_Click() 'Handles add_note.Click

        Try
            If (Page.IsValid) Then

                If UCase(TypeOfNote) = "NOTE" Then
                    If jetnet_ac.Text > 0 Or client_ac.Text > 0 Then
                        CheckForAircraftForSaleChangeCreationNeeded()
                    End If
                End If

                Select Case Trim(Request("action"))
                    Case "edit"
                        If Session.Item("localUser").crmUserType <> eUserTypes.RESEARCH Then
                            Dim idnum As Integer = Trim(Request("id"))
                            RaiseEvent edit_note(LCase(TypeOfNote), Me, idnum)

                            If Session.Item("isMobile") = True Then
                                Response.Redirect("Mobile_Details.aspx?type=" & Session.Item("Listing") & "&comp_id=" & Session.Item("ListingID") & "&source=" & Session.Item("ListingSource") & "&added=note", False)
                            End If
                        End If
                    Case "new"
                        RaiseEvent edit_note(LCase(TypeOfNote), Me, 0)

                        If Session.Item("isMobile") = True Then
                            Response.Redirect("Mobile_Details.aspx?type=" & Session.Item("Listing") & "&comp_id=" & Session.Item("ListingID") & "&source=" & Session.Item("ListingSource") & "&added=note", False)
                        End If
                End Select
            End If


        Catch ex As Exception
            error_string = "Notes.ascx.vb - add_note_Click() - " & ex.Message
            clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
        End Try
    End Sub
#End Region

    Public Sub checkDate(ByVal sender As Object, ByVal args As ServerValidateEventArgs)

        If args.Value > Now() Then
            args.IsValid = False
            Exit Sub
        End If
        args.IsValid = True
    End Sub

    Public Sub checkDate_Future(ByVal sender As Object, ByVal args As ServerValidateEventArgs)

        If args.Value < Now() Then
            args.IsValid = False
            Exit Sub
        End If
        args.IsValid = True
    End Sub
    Private Sub make_note_cookie(ByVal idnum As Integer)
        Dim _noteCookies As HttpCookie = Request.Cookies("notes")
        If _noteCookies IsNot Nothing Then
            Dim stored_id As String = _noteCookies("ID")

            'Let's do one thing at a time. First we need to only store 5 companies. 
            'Also no duplicates.. 

            Dim id_array As Array = Split(stored_id, "|")
            'ubound needs to be less than 4 to have 5 companies stored.

            Dim exists As Integer = InStr(stored_id, CStr(idnum))

            If UBound(id_array) < 4 Then

                If exists = 0 Then
                    Response.Cookies("notes").Values("ID") = idnum & "|" & stored_id
                    Response.Cookies("notes").Values("USER") = Session.Item("localUser").crmLocalUserID
                    Response.Cookies("notes").Expires = DateTime.Now.AddDays(10)
                Else
                    Dim topnumber As Integer = UBound(id_array)


                    stored_id = ""

                    For i As Integer = 0 To topnumber
                        If id_array(i) <> CStr(idnum) Then
                            stored_id = stored_id & id_array(i) & "|"
                        End If
                    Next


                    If stored_id <> "" Then
                        stored_id = UCase(stored_id.TrimEnd("|"))
                    End If


                    Response.Cookies("notes").Values("ID") = idnum & "|" & stored_id
                    Response.Cookies("notes").Values("USER") = Session.Item("localUser").crmLocalUserID
                    Response.Cookies("notes").Expires = DateTime.Now.AddDays(10)
                End If

            Else
                'Store the ubound of the array.
                Dim topnumber As Integer = UBound(id_array)
                'rewrite the cookie with the last 5 in array.

                If exists = 0 Then
                    stored_id = id_array(topnumber - 4) & "|" & id_array(topnumber - 3) & "|" & id_array(topnumber - 2) & "|" & id_array(topnumber - 1)
                Else
                    stored_id = id_array(topnumber - 4) & "|" & id_array(topnumber - 3) & "|" & id_array(topnumber - 2) & "|" & id_array(topnumber - 1) & "|" & id_array(topnumber)
                End If


                id_array = Split(stored_id, "|")
                topnumber = UBound(id_array)
                stored_id = ""


                For i As Integer = 0 To topnumber
                    If id_array(i) <> CStr(idnum) Then
                        stored_id = stored_id & id_array(i) & "|"
                    End If
                Next

                If stored_id <> "" Then
                    stored_id = UCase(stored_id.TrimEnd("|"))
                End If

                Response.Cookies("notes").Values("ID") = idnum & "|" & stored_id
                Response.Cookies("notes").Values("USER") = Session.Item("localUser").crmLocalUserID
                Response.Cookies("notes").Expires = DateTime.Now.AddDays(10)
            End If

        Else
            Dim aCookie As New HttpCookie("notes")
            aCookie.Values("ID") = idnum
            aCookie.Values("USER") = Session.Item("localUser").crmLocalUserID
            aCookie.Expires = DateTime.Now.AddDays(10)
            Response.Cookies.Add(aCookie)
        End If


    End Sub
    Private Sub remove_note_Click() Handles removeNoteLB.Click

        If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Or Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
            Dim cprospect_id As Long = 0
            If Not IsNothing(Name.FindControl("acval_id")) Then
                Dim store_doc5 As Label = Name.FindControl("acval_id")
                If Trim(acval_id.Text) <> "" Then
                    cprospect_id = acval_id.Text
                Else
                    cprospect_id = 0
                End If
            Else
                cprospect_id = 0
            End If

            Call aclsData_Temp.Remove_Prospect_View_Homebase(cprospect_id)


            'Either the button on the opener for the view is there and I'm clicking it, or it's not and I am refreshing the opening page. Either way, it doesn't really matter and will work. Clicking the button is nicer on the view, but isn't detrimental if it just refreshes the page.
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_WindowOpener", "if(window.opener.$('#ContentPlaceHolder1_View_Master1_crmProspectSearchButton').length){window.opener.$('#ContentPlaceHolder1_View_Master1_crmProspectSearchButton').trigger('click');} else {window.opener.location.href=window.opener.location.href;};", True)

            'close window.
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)

        Else
            If Session.Item("localUser").crmUserType <> eUserTypes.RESEARCH Then
                Dim idnum As Integer = 0
                Try
                    idnum = Trim(Request("id"))
                Catch ex As Exception
                    error_string = "Notes.ascx.vb - remove_note_Click() - " & ex.Message
                    clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
                End Try
                RaiseEvent remove_note_ev(idnum, Me, LCase(TypeOfNote))
            End If
        End If

    End Sub
    Private Sub estval_type_of_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles estval_type_of.SelectedIndexChanged

        If Session.Item("localSubscription").crmAppraiser_Flag = True Then
            If estval_type_of.SelectedValue = "F" Or estval_type_of.SelectedValue = "D" Then
                Me.authorization_panel.Visible = True
            Else
                Me.authorization_panel.Visible = False
            End If
        End If

    End Sub
    Private Sub curprev_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles curprev.SelectedIndexChanged

        If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Or Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then

        Else
            If curprev.SelectedValue = "P" Then
                current.Visible = False
                note_date.Visible = True
                note_date_image.Visible = True
                time.Visible = True
                note_date.Text = FormatDateTime(Now(), 2)
                time.SelectedValue = Format(Now(), "HH:00")
                RequiredFieldValidator1.Enabled = True
            Else
                current.Visible = True
                current.Text = Now()
                note_date.Visible = False
                note_date_image.Visible = False
                time.Visible = False
                RequiredFieldValidator1.Enabled = False
            End If
        End If



    End Sub

    Private Sub follow_up_CheckedChanged() Handles follow_up.CheckedChanged
        If follow_up.Checked = True Then
            action_item_vis.Visible = True
            action_item_time.SelectedValue = Format(Now(), "HH:00")
        Else
            action_item_vis.Visible = False
        End If
    End Sub

    Private Sub ProspectAircraft_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ProspectAircraft.CheckedChanged
        If ProspectAircraft.Checked = True Then
            aircraft_name.Items.Clear()
            aircraft_name.Items.Add(New ListItem("Please select an Aircraft", "0||0"))
            Dim acTable As DataTable = aclsData_Temp.BuildACProspectList("")
            If Not IsNothing(acTable) Then
                For Each r As DataRow In acTable.Rows
                    Dim ACString As String = ""
                    ACString = r("amod_make_name") & " " & r("amod_model_name")
                    ACString += IIf(Not IsDBNull(r("ac_ser_nbr")), " Ser #:" & r("ac_ser_nbr") & " ", "")
                    ACString += IIf(Not IsDBNull(r("ac_reg_nbr")), "Reg #" & r("ac_reg_nbr"), "")

                    If r("lnote_client_ac_id") > 0 Then
                        aircraft_name.Items.Add(New ListItem(ACString, r("lnote_client_ac_id") & "|CLIENT|" & r("amod_id")))
                    Else
                        aircraft_name.Items.Add(New ListItem(ACString, r("lnote_jetnet_ac_id") & "|JETNET|" & r("amod_id")))
                    End If

                Next
            End If
        End If
    End Sub

    Private Sub DisplayCurrentValuationData(ByRef idnum As Long)
        DisplayComparableData(idnum, current_market_label, True, False)
        DisplayComparableData(idnum, current_sold_label, False, True)
        DisplayFieldsToCompare(idnum, field_label)
    End Sub


    Private Sub DisplayComparableData(ByRef idnum As Long, ByRef LabelDisplay As Label, ByRef Current As Boolean, ByRef Sold As Boolean)
        Dim CurrentMarket As New DataTable
        valuation_panel.Visible = True
        'Get current market comparables.
        If Current Then
            CurrentMarket = aclsData_Temp.Get_Client_Current_Market_Comparables(idnum)
        Else
            CurrentMarket = aclsData_Temp.Get_Client_Sold_Comparables(idnum)
        End If

        If Not IsNothing(CurrentMarket) Then
            If CurrentMarket.Rows.Count > 0 Then
                Dim cssStyle As String = "alt_row"

                If Current Then
                    LabelDisplay.Text = "<h2><u>Current Market Comparables</u></h2>"
                Else
                    LabelDisplay.Text = "<h2><u>Sold Comparables</u></h2>"
                End If
                LabelDisplay.Text += "<table class=""engine"" width='100%' border=""0"" cellpadding=""3"">"
                LabelDisplay.Text += "<tr>"
                LabelDisplay.Text += "<td align='left' valign='top' class=""dark_gray""><u>Comparing</u></td>"
                LabelDisplay.Text += "<td align='left' valign='top' class=""dark_gray""><u>Year Mfr</u></td>"
                LabelDisplay.Text += "<td align='left' valign='top' class=""dark_gray""><u>Reg #</u></td>"
                LabelDisplay.Text += "<td align='left' valign='top' class=""dark_gray""><u>Asking</u></td>"
                LabelDisplay.Text += "<td align='left' valign='top' class=""dark_gray""><u>Take</u></td>"
                If Current Then
                    LabelDisplay.Text += "<td align='left' valign='top' class=""dark_gray""><u>Broker</u></td>"
                Else
                    LabelDisplay.Text += "<td align='left' valign='top' class=""dark_gray""><u>Sold</u></td>"
                    LabelDisplay.Text += "<td align='left' valign='top' class=""dark_gray""><u>Sold Date</u></td>"
                End If

                LabelDisplay.Text += "</tr>"

                For Each r As DataRow In CurrentMarket.Rows
                    LabelDisplay.Text += "<tr class='" & cssStyle & "'>"
                    'Comparing Field.
                    LabelDisplay.Text += "<td align='left' valign='top'>"
                    If Not IsDBNull(r("Comparing")) Then
                        LabelDisplay.Text += r("Comparing")
                    End If
                    LabelDisplay.Text += "</td>"

                    'Year MFR Field.
                    LabelDisplay.Text += "<td align='left' valign='top'>"
                    If Not IsDBNull(r("Year Mfr")) Then
                        LabelDisplay.Text += r("Year Mfr")
                    End If
                    LabelDisplay.Text += "</td>"

                    'Reg # Field.
                    LabelDisplay.Text += "<td align='left' valign='top'>"
                    If Not IsDBNull(r("Reg #")) Then
                        LabelDisplay.Text += r("Reg #")
                    End If
                    LabelDisplay.Text += "</td>"

                    'Asking Field.
                    LabelDisplay.Text += "<td align='left' valign='top'>"
                    If Not IsDBNull(r("Asking")) Then
                        If Not IsDBNull(r("Asking")) Then
                            If r("Asking") > 0 Then
                                LabelDisplay.Text += clsGeneral.clsGeneral.no_zero(r("Asking"), "", True)
                            Else
                                LabelDisplay.Text += "-"
                            End If
                        End If
                    Else
                        LabelDisplay.Text += "-"
                    End If
                    LabelDisplay.Text += "</td>"

                    'Take Field.
                    LabelDisplay.Text += "<td align='left' valign='top'>"
                    If Not IsDBNull(r("Take")) Then
                        If r("Take") > 0 Then
                            LabelDisplay.Text += clsGeneral.clsGeneral.no_zero(r("Take"), "", True)
                        Else
                            LabelDisplay.Text += "-"
                        End If
                    Else
                        LabelDisplay.Text += "-"
                    End If

                    LabelDisplay.Text += "</td>"

                    If Current Then
                        'Broker Field.
                        LabelDisplay.Text += "<td align='left' valign='top'>"
                        If Not IsDBNull(r("Broker")) Then
                            If r("Broker") > 0 Then
                                LabelDisplay.Text += clsGeneral.clsGeneral.no_zero(r("Broker"), "", True)
                            Else
                                LabelDisplay.Text += "-"
                            End If
                        Else
                            LabelDisplay.Text += "-"
                        End If
                        LabelDisplay.Text += "</td>"
                        LabelDisplay.Text += "</tr>"
                    Else
                        'Sold Field.
                        LabelDisplay.Text += "<td align='left' valign='top'>"
                        If Not IsDBNull(r("Sold")) Then
                            If r("Sold") > 0 Then
                                LabelDisplay.Text += clsGeneral.clsGeneral.no_zero(r("Sold"), "", True)
                            Else
                                LabelDisplay.Text += "-"
                            End If
                        Else
                            LabelDisplay.Text += "-"
                        End If

                        LabelDisplay.Text += "</td>"

                        LabelDisplay.Text += "<td align='left' valign='top'>"
                        If Not IsDBNull(r("Sold Date")) Then
                            LabelDisplay.Text += r("Sold Date")
                        End If

                        LabelDisplay.Text += "</td>"
                        LabelDisplay.Text += "</tr>"
                    End If


                    'Value Field.
                    If Not IsDBNull(r("Value Desc.")) Then
                        LabelDisplay.Text += "<tr class='" & cssStyle & "'>"
                        LabelDisplay.Text += "<td align='left' valign='top' colspan='6'>Value Desc: "
                        LabelDisplay.Text += r("Value Desc.")
                        LabelDisplay.Text += "</td>"
                        LabelDisplay.Text += "</tr>"
                    End If


                    If cssStyle = "" Then
                        cssStyle = "alt_row"
                    Else
                        cssStyle = ""
                    End If
                Next


                LabelDisplay.Text += "</table>"
            End If
        End If
    End Sub
    Private Sub DisplayFieldsToCompare(ByRef idnum As Long, ByRef LabelDisplay As Label)
        Dim CurrentFields As New DataTable
        valuation_panel.Visible = True

        CurrentFields = aclsData_Temp.Get_Fields_To_Compare(idnum)

        If Not IsNothing(CurrentFields) Then
            If CurrentFields.Rows.Count > 0 Then
                Dim cssStyle As String = "alt_row"

                LabelDisplay.Text = "<h2><u>Fields to Compare</u></h2>"
                LabelDisplay.Text += "<table class=""engine"" width='100%' border=""0"" cellpadding=""3"">"
                LabelDisplay.Text += "<tr>"
                LabelDisplay.Text += "<td align='left' valign='top' class=""dark_gray""><u>Fields</u></td>"
                LabelDisplay.Text += "</tr>"

                For Each r As DataRow In CurrentFields.Rows
                    LabelDisplay.Text += "<tr class='" & cssStyle & "'>"
                    'Fieldname Field.
                    LabelDisplay.Text += "<td align='left' valign='top'>"
                    If Not IsDBNull(r("clivalfld_name")) Then
                        LabelDisplay.Text += r("clivalfld_name").ToString
                    End If
                    LabelDisplay.Text += "</td>"

                    LabelDisplay.Text += "</tr>"


                    If cssStyle = "" Then
                        cssStyle = "alt_row"
                    Else
                        cssStyle = ""
                    End If
                Next


                LabelDisplay.Text += "</table>"
            End If
        End If
    End Sub

    Private Sub attach_prospect_by_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles attach_prospect_by.SelectedIndexChanged
        If UCase(attach_prospect_by.SelectedValue) = "AIRCRAFT" Then

            aircraft_information_panel.Visible = True
            model_information_panel.Visible = False

            jetnet_mod.Text = "0"
            client_mod.Text = "0"
        ElseIf UCase(attach_prospect_by.SelectedValue) = "MODEL" Then
            model_information_panel.Visible = True
            jetnet_mod.Text = "0"
            client_mod.Text = "0"
            aircraft_information_panel.Visible = False
            Try
                aircraft_name.SelectedValue = "0||0"
            Catch
                aircraft_name.SelectedValue = "0"
            End Try

            aircraft_info.Text = ""
            jetnet_ac.Text = "0"
            client_ac.Text = "0"

            RaiseEvent fill_drop(jetnet_ac.Text, client_ac.Text, jetnet_comp.Text, client_comp.Text, jetnet_contact.Text, client_contact.Text, jetnet_mod.Text, client_mod.Text, Me, True)

        Else
            'neither was selected. Meaning no model/no aircraft. Toggle off the panels.
            model_information_panel.Visible = False

            aircraft_information_panel.Visible = False
            'Deselect the aircraft dropdown.
            Try
                aircraft_name.SelectedValue = "0||0"
            Catch
                aircraft_name.SelectedValue = "0"
            End Try
            'Deselect the model dropdown.
            Try
                model_name.SelectedValue = "0||0"
            Catch
            End Try
            'Clear the boxes.
            aircraft_info.Text = ""
            jetnet_mod.Text = "0"
            client_mod.Text = "0"
            jetnet_ac.Text = "0"
            client_ac.Text = "0"
        End If
    End Sub

    Private Sub model_name_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles model_name.SelectedIndexChanged
        Dim typed() As String
        If model_name.SelectedValue <> "0||0" Then
            typed = Split(model_name.SelectedValue, "|")
            jetnet_mod.Text = typed(0)
            client_mod.Text = typed(4)
        Else
            jetnet_mod.Text = "0"
            client_mod.Text = "0"
        End If
    End Sub

    Private Sub visible_all_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles visible_all.Click
        If notes_opp.Visible = False Then
            notes_opp.Visible = True
            cat_name.Visible = False
            cat_insert.Visible = False
            visible_all.Text = "Add Row"
            attention.Text = ""
            cat_name.Text = ""
        Else
            cat_insert.Visible = True
            notes_opp.Visible = False
            cat_name.Visible = True
            visible_all.Text = "Cancel"
        End If
    End Sub


    Private Sub cat_insert_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cat_insert.Click
        If cat_name.Text <> "" Then
            Dim DuplicateCheck As Boolean = False
            For Each item As ListItem In notes_opp.Items
                If Trim(item.Text) = Trim(cat_name.Text) Then
                    DuplicateCheck = True
                End If
            Next

            If DuplicateCheck = True Then
                attention.Text = "<p align='center'>Your category already exists.</p>"
            Else
                If aclsData_Temp.Insert_Opportunity_Categories(cat_name.Text) = 1 Then
                    attention.Text = "<p align='center'>Your category has been added.</p>"
                    cat_name.Visible = False
                    notes_opp.Visible = True
                    notes_opp.Items.Clear()
                    'rebind the roll
                    If Not IsNothing(Name.FindControl("notes_opp")) Then
                        notes_opp.Items.Clear()
                        clsGeneral.clsGeneral.Fill_Opportunity_Category(Name.FindControl("notes_opp"), aTempTable, aclsData_Temp)
                    End If
                Else
                    If aclsData_Temp.class_error <> "" Then
                        error_string = aclsData_Temp.class_error
                        clsGeneral.clsGeneral.LogError("edit_note.ascx.vb -Fill_All_DropDowns() - " & error_string, aclsData_Temp)
                    End If
                End If
            End If
        Else
            attention.Text = "<p align='center'>Your category name cannot be blank.</p>"
        End If
    End Sub


    Private Sub LoadAcStatusChangeJS()
        If Not Page.ClientScript.IsClientScriptBlockRegistered("acStatusLoad") Then
            Dim acStatusLoadScript As StringBuilder = New StringBuilder()
            acStatusLoadScript.Append(vbCrLf & "  function acStatusLoad() {")

            acStatusLoadScript.Append(vbCrLf & "if($('#" & ac_sale.ClientID & " input:checked').val() == 'Y') {")

            'toggle others as invisible
            acStatusLoadScript.Append(vbCrLf & "$('#" & ac_status_for_sale.ClientID & "').css('display','block');")
            acStatusLoadScript.Append(vbCrLf & "$('#" & ac_status_not_for_sale.ClientID & "').css('display','none');")

            'set values as empty
            acStatusLoadScript.Append(vbCrLf & "$('#" & ac_status_not_for_sale.ClientID & "').val('');")

            '  acStatusLoadScript.Append(vbCrLf & "var new_val = $('#" & ac_status_hold.ClientID & "').val();")

            'acStatusLoadScript.Append(vbCrLf & "if ($('#" & ac_status_for_sale.ClientID & " option[value=""'+new_val+'""]').length) {")
            'acStatusLoadScript.Append(vbCrLf & "$('#" & ac_status_for_sale.ClientID & "').val($('#" & ac_status_hold.ClientID & "').val());")
            'acStatusLoadScript.Append(vbCrLf & " } else {")
            'acStatusLoadScript.Append(vbCrLf & "$('#" & ac_status_for_sale.ClientID & "').val('Other');")
            'acStatusLoadScript.Append(vbCrLf & " } ")

            acStatusLoadScript.Append(vbCrLf & " } else if ($('#" & ac_sale.ClientID & " input:checked').val() == 'N') {")

            'acStatusLoadScript.Append(vbCrLf & "$('#" & ac_status_not_for_sale.ClientID & "').val('Not For Sale');")
            'toggle others as invisible
            acStatusLoadScript.Append(vbCrLf & "$('#" & ac_status_for_sale.ClientID & "').css('display','none');")
            acStatusLoadScript.Append(vbCrLf & "$('#" & ac_status_not_for_sale.ClientID & "').css('display','block');")

            'set values as empty
            acStatusLoadScript.Append(vbCrLf & "$('#" & ac_status_for_sale.ClientID & "').val('');")
            acStatusLoadScript.Append(vbCrLf & "}")
            acStatusLoadScript.Append(vbCrLf & "}")

            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "acStatusLoad()", acStatusLoadScript.ToString, True)
        End If

    End Sub



    Private Sub CheckForAircraftForSaleChangeCreationNeeded()
        Dim aTempTable As New DataTable
        Dim Aircraft_Data As New clsClient_Aircraft
        Dim URL_String_Parameters As String = ""
        Dim PageToEdit As String = ""

        'First we need to get the client or jetnet ID (client ID will take precendence).
        Dim jetnetID As Long = 0
        Dim clientID As Long = 0

        jetnetID = jetnet_ac.Text
        clientID = client_ac.Text

        'Then we will lookup the aircraft, client ID takes precendence if there is one.
        If clientID > 0 Then
            aTempTable = aclsData_Temp.Get_Clients_Aircraft(clientID)
            If Not IsNothing(aTempTable) Then
                If aTempTable.Rows.Count > 0 Then
                    Aircraft_Data = clsGeneral.clsGeneral.Create_Aircraft_Class(aTempTable, "cliaircraft")
                    Aircraft_Data.cliaircraft_id = clientID
                End If
            End If
        ElseIf jetnetID > 0 Then
            aTempTable = aclsData_Temp.GetJETNET_Aircraft_Details_AC_ID(jetnetID, "")
            If Not IsNothing(aTempTable) Then
                If aTempTable.Rows.Count > 0 Then
                    Aircraft_Data = clsGeneral.clsGeneral.Create_Aircraft_Class(aTempTable, "ac")
                    Aircraft_Data.cliaircraft_id = jetnetID
                End If
            End If
        End If

        'Then we will compare the fields to see if anything has changed. 
        Dim acChanged As Boolean = False

        'Ac For Sale
        If Aircraft_Data.cliaircraft_forsale_flag <> ac_sale.SelectedValue Then
            acChanged = True
            URL_String_Parameters = "forSale=" & ac_sale.SelectedValue
        End If

        'Ac Status
        If ac_status_for_sale.SelectedValue <> "" Then
            If Aircraft_Data.cliaircraft_status <> ac_status_for_sale.SelectedValue Then
                acChanged = True
                If URL_String_Parameters <> "" Then
                    URL_String_Parameters += "&"
                End If
                URL_String_Parameters += "status=" & ac_status_for_sale.SelectedValue
                URL_String_Parameters += "&ostatus=" & Aircraft_Data.cliaircraft_status
            End If
        Else
            If Aircraft_Data.cliaircraft_status <> ac_status_not_for_sale.SelectedValue Then
                acChanged = True
                If URL_String_Parameters <> "" Then
                    URL_String_Parameters += "&"
                End If
                URL_String_Parameters += "status=" & ac_status_not_for_sale.SelectedValue
                URL_String_Parameters += "&ostatus=" & Aircraft_Data.cliaircraft_status
            End If
        End If

        'Value description:
        If Aircraft_Data.cliaircraft_value_description <> cliaircraft_value_description_text.Text Then
            acChanged = True
            If URL_String_Parameters <> "" Then
                URL_String_Parameters += "&"
            End If
            URL_String_Parameters += "vdesc=" & cliaircraft_value_description_text.Text
            URL_String_Parameters += "&ovdesc=" & Aircraft_Data.cliaircraft_value_description
        End If

        'Date Listed: 
        If Not IsNothing(Aircraft_Data.cliaircraft_date_listed) Then
            If CStr(Aircraft_Data.cliaircraft_date_listed) <> CStr(date_listed.Text) Then
                acChanged = True
                If URL_String_Parameters <> "" Then
                    URL_String_Parameters += "&"
                End If
                URL_String_Parameters += "datel=" & date_listed.Text
                URL_String_Parameters += "&odatel=" & Aircraft_Data.cliaircraft_date_listed
            End If
        Else
            If date_listed.Text <> "" Then
                acChanged = True
                If URL_String_Parameters <> "" Then
                    URL_String_Parameters += "&"
                End If
                URL_String_Parameters += "datel=" & date_listed.Text
                URL_String_Parameters += "&odatel=" & Aircraft_Data.cliaircraft_date_listed
            End If
        End If


        'Asking Wordage
        If Aircraft_Data.cliaircraft_asking_wordage <> asking_wordage.SelectedValue Then
            acChanged = True
            If URL_String_Parameters <> "" Then
                URL_String_Parameters += "&"
            End If
            URL_String_Parameters += "askw=" & asking_wordage.SelectedValue
            URL_String_Parameters += "&oaskw=" & Aircraft_Data.cliaircraft_asking_wordage
        End If

        'Asking Price
        If Aircraft_Data.cliaircraft_asking_price <> asking_price.Text Then
            acChanged = True
            If URL_String_Parameters <> "" Then
                URL_String_Parameters += "&"
            End If
            URL_String_Parameters += "askp=" & asking_price.Text
            URL_String_Parameters += "&oaskp=" & Aircraft_Data.cliaircraft_asking_price
        End If

        'Take price
        If Aircraft_Data.cliaircraft_est_price <> est_price.Text Then
            acChanged = True
            If URL_String_Parameters <> "" Then
                URL_String_Parameters += "&"
            End If
            URL_String_Parameters += "estp=" & est_price.Text
            URL_String_Parameters += "&oestp=" & Aircraft_Data.cliaircraft_est_price
        End If

        'Broker Price
        If Aircraft_Data.cliaircraft_broker_price <> broker_price.Text Then
            acChanged = True
            If URL_String_Parameters <> "" Then
                URL_String_Parameters += "&"
            End If
            URL_String_Parameters += "brokp=" & broker_price.Text
            URL_String_Parameters += "&obrokp=" & Aircraft_Data.cliaircraft_broker_price
        End If

        'Then we will send the information off to the aircraft page to autocreate the aircraft. 
        If acChanged Then
            'Popup the new window:
            If clientID > 0 Then
                PageToEdit = "edit.aspx?noteCreationAC=true&action=edit&type=aircraft&ac_ID=" & clientID & "&source=CLIENT&" & URL_String_Parameters
            ElseIf jetnetID > 0 Then
                PageToEdit = "edit.aspx?noteCreationAC=true&action=edit&type=aircraft&ac_ID=" & jetnetID & "&source=JETNET&" & URL_String_Parameters
            End If

            add_noteLB.OnClientClick = "if(Page_ClientValidate(""Notes_Edit"")){window.open('" & PageToEdit & "','EditPage', 'scrollbars=yes,menubar=no,height=30,width=400,resizable=yes,toolbar=no,location=no,status=no')}"
        End If


        'Then we will close that page and continue on adding the note.
    End Sub

    Private Sub broker_price_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles broker_price.TextChanged, est_price.TextChanged, asking_price.TextChanged, ac_sale.SelectedIndexChanged, ac_status_for_sale.SelectedIndexChanged, ac_status_not_for_sale.SelectedIndexChanged, cliaircraft_value_description_text.TextChanged, date_listed.TextChanged, asking_wordage.SelectedIndexChanged
        CheckForAircraftForSaleChangeCreationNeeded()
    End Sub
End Class