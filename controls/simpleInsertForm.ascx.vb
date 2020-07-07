Partial Public Class simpleInsertForm

    Inherits System.Web.UI.UserControl

    'Public cancleAndClose As Boolean = False
    'Public currentBrowseRec As Long = 0
    'Public fromView As String = ""
    Private AircraftID As Long = 0
    Private CompanyID As Long = 0
    Private YachtID As Long = 0

    Private journalID As Long = 0
    Private reminderID As Long = 0
    Public aclsData_Temp As New clsData_Manager_SQL
    Private JournalTable As New DataTable
    Private AircraftTable As New DataTable

    Private TypeOfNote As String = "Aircraft"
    Public bIsNote As Boolean = False
    Public bIsProspect As Boolean = False
    Public bIsUpdate As Boolean = False
    Dim NoteTitle As String = ""
    Dim crmSource As String = "JETNET"

    ''' <summary>
    ''' This function runs to fill in the left hand side.
    ''' Either with aircraft Information or with Company Info.
    ''' </summary>
    ''' <param name="masterPage"></param>
    ''' <remarks></remarks>
    Public Sub generateItemDetails(ByVal masterPage As EmptyEvoTheme)
        If AircraftID <> 0 Then
            'container_tab.Visible = True
            aircraft_information.Visible = True
            Dim passCheckbox As New CheckBox
            passCheckbox.Checked = True
            AircraftTable = CommonAircraftFunctions.BuildReusableTable(AircraftID, journalID, "", "", masterPage.aclsData_Temp, True, 0, crmSource)
            headerLabel.Text = CommonAircraftFunctions.CreateHeaderLine(AircraftTable.Rows(0).Item("amod_make_name"), AircraftTable.Rows(0).Item("amod_model_name"), AircraftTable.Rows(0).Item("ac_ser_nbr"), "")
            aircraft_information.Text += CommonAircraftFunctions.Build_Identification_Block("blue", False, "", "100%", "100%", 0, AircraftTable, "", Me.journalID, Me.AircraftID, masterPage.aclsData_Temp, New CheckBox, passCheckbox, HttpContext.Current.Session.Item("localPreferences").AerodexFlag, 0, False)
            ModelID.Text = AircraftTable.Rows(0).Item("ac_amod_id")
        End If
        If YachtID <> 0 Then
            'yacht_container_tab.Visible = True
            yacht_information.Visible = True
            yacht_information.Text = crmWebClient.DisplayFunctions.BuildYachtInformationTab(YachtID, yacht_features_tab, masterPage.aclsData_Temp, YachtModelID, Nothing, Nothing, "", "", "", Nothing, "", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, True) ', , masterPage, YachtID, 0, True, YachtModelID)
            yacht_information.Text = "<div class=""Box""><table width=""100%"" cellpadding=""0"" cellspacing=""0"" class=""formatTable blue""><tr><td align=""left"" valign=""top""><div class=""subHeader"">&nbsp;" & yacht_features_tab.HeaderText & "</div><br />" & yacht_information.Text & "</td></tr></table></div>"

        End If

        If CompanyID <> 0 Then
            company_information.Visible = True

            If Trim(Request("source")) = "CLIENT" Then
                crmWebClient.CompanyFunctions.Fill_Information_Tab(company_features_tab, company_information, masterPage, CompanyID, journalID, "", New Label, New AjaxControlToolkit.TabContainer, New Label, New Label, True, True, crmSource)
            Else
                crmWebClient.CompanyFunctions.Fill_Information_Tab(company_features_tab, company_information, masterPage, CompanyID, journalID, "", New Label, New AjaxControlToolkit.TabContainer, New Label, New Label, True)
            End If

            company_information.Text = "<div class=""Box""><table width=""100%"" cellpadding=""0"" cellspacing=""0"" class=""formatTable blue""><tr><td align=""left"" valign=""top""><div class=""subHeader"">&nbsp;" & company_features_tab.HeaderText & "</div><br />" & company_information.Text & "</td></tr></table></div>"
        End If
    End Sub


    Public Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim masterPage As EmptyEvoTheme = DirectCast(Page.Master, EmptyEvoTheme)


        Dim aTempTable As New DataTable
        aclsData_Temp = New clsData_Manager_SQL
        aclsData_Temp.JETNET_DB = Session.Item("jetnetClientDatabase")
        aclsData_Temp.client_DB = Session.Item("jetnetServerNotesDatabase")



        'what's the edit note ID?
        If Not IsNothing(Request.Item("lnoteID")) Then
            If Not String.IsNullOrEmpty(Request.Item("lnoteID").ToString) Then
                If IsNumeric(Request.Item("lnoteID").Trim) Then
                    If (Request.Item("lnoteID").Trim) <> 0 Then
                        reminderID = Request.Item("lnoteID").Trim
                        bIsUpdate = True
                        remove_note.Visible = True
                    End If
                End If
            End If
        End If

        'what Aircraft is it attached to?
        If Not IsNothing(Request.Item("acid")) Then
            If Not String.IsNullOrEmpty(Request.Item("acid").ToString) Then
                If IsNumeric(Request.Item("acid").Trim) Then
                    AircraftID = Request.Item("acid").Trim

                End If
            End If
        End If
        'What yt is it attached to?
        If Not IsNothing(Request.Item("ytid")) Then
            If Not String.IsNullOrEmpty(Request.Item("ytid").ToString) Then
                If IsNumeric(Request.Item("ytid").Trim) Then
                    YachtID = Request.Item("ytid").Trim
                    TypeOfNote = "Yacht"
                End If
            End If
        End If


        'what company is it attached to?
        If Not IsNothing(Request.Item("compid")) Then
            If Not String.IsNullOrEmpty(Request.Item("compid").ToString) Then
                If IsNumeric(Request.Item("compid").Trim) Then
                    'This is very important
                    'If the user is NOT a PLUS notes user
                    'They cannot save a company note.
                    'Meaning this can't be set
                    'Unless Server Side Notes Flag is true.
                    If Session.Item("localSubscription").crmServerSideNotes_Flag Then
                        CompanyID = Request.Item("compid").Trim
                        TypeOfNote = "Company"
                    End If
                End If
            End If
        End If


        If clsGeneral.clsGeneral.isCrmDisplayMode() Then
            If Not IsNothing(Trim(HttpContext.Current.Request("source"))) Then
                If Not String.IsNullOrEmpty(HttpContext.Current.Request("source")) Then
                    crmSource = Trim(HttpContext.Current.Request("source"))
                End If
            End If
        End If

        'is this a note?
        If Not IsNothing(Request.Item("n")) Then
            If Not String.IsNullOrEmpty(Request.Item("n").ToString) Then
                If IsNumeric(Request.Item("n").Trim) Then
                    bIsNote = True
                    current_or_previous_date.Visible = True
                    previous_date_text.CssClass = "display_none"
                    current_date_label.CssClass = ""
                End If
            End If
        End If

        'Second check. We're going to look for type b
        'is this a note?
        If Not IsNothing(Request.Item("b")) Then
            If Not String.IsNullOrEmpty(Request.Item("b").ToString) Then
                If IsNumeric(Request.Item("b").Trim) Then
                    bIsProspect = True
                    current_or_previous_date.Visible = True
                    previous_date_text.CssClass = "display_none"
                    current_date_label.CssClass = ""

                End If
            End If
        End If

        'This is going to set the page title and the header text.
        NoteTitle = IIf(bIsUpdate = True, "Edit ", "Insert ") & TypeOfNote

        If bIsNote Then
            NoteTitle += " Note"
            recordHeading.InnerText = UCase("Note Information")
        ElseIf bIsProspect Then
            NoteTitle += " Prospect"
            recordHeading.InnerText = UCase("Prospect Information")
        Else
            NoteTitle += " Action Item"
            recordHeading.InnerText = UCase("Action Item Information")

            If Not Page.IsPostBack Then
                Dim jsString As String = " $(function() {"
                jsString += "$(""#" & txtDateID.ClientID & """).datepicker({"
                jsString += " showOn: ""button"", "
                jsString += " buttonImage: ""/images/final.jpg"","
                jsString += " buttonImageOnly: true,"
                jsString += " minDate: 0,"
                jsString += " buttonText: ""Select date"""
                jsString += " });"
                jsString += " } );"
                System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType, "dateString", jsString, True)
            End If

        End If

        masterPage.SetPageTitle(NoteTitle)

        Try
            If Not Page.IsPostBack Then

                'This is going to fill up the username list ONLY IF THEY are Cloud Notes +
                If Session.Item("localSubscription").crmServerSideNotes_Flag Then

                    Dim tempTable As New DataTable
                    Dim tmpPrefobj As New preferencesDataLayer

                    tmpPrefobj.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

                    tempTable = tmpPrefobj.ReturnUserDetailsAndImage(HttpContext.Current.Session.Item("localUser").crmUserContactID)

                    If Not IsNothing(tempTable) Then
                        If tempTable.Rows.Count > 0 Then

                            For Each r As DataRow In tempTable.Rows

                                If Not (IsDBNull(r.Item("contact_email_address"))) And Not String.IsNullOrEmpty(r.Item("contact_email_address").ToString) Then
                                    commonEvo.get_crm_client_info(r.Item("contact_email_address").ToString.Trim, r.Item("contact_first_name").ToString.Trim, r.Item("contact_last_name").ToString.Trim, "")
                                End If

                            Next

                        End If

                    End If

                    tempTable = Nothing
                    tmpPrefobj = Nothing

                    'This function fills up the username list with applicable users.
                    If reminderID = 0 Then
                        aTempTable = aclsData_Temp.Get_Client_User_List(True)
                    Else
                        aTempTable = aclsData_Temp.Get_Client_User_List()
                    End If


                    If Not IsNothing(aTempTable) Then
                        If aTempTable.Rows.Count > 0 Then
                            For Each q As DataRow In aTempTable.Rows
                                'loads each user.
                                userNameList.Items.Add(New ListItem(q("cliuser_first_name").ToString & " " & q("cliuser_last_name").ToString, q("cliuser_id")))
                                'If the username matches the username of the active person, we select that user.
                                If UCase(q("cliuser_email_address").ToString) = UCase(Session.Item("localUser").crmLocalUserName) Then
                                    userNameList.SelectedValue = q("cliuser_id")
                                End If
                            Next
                        End If
                    End If
                ElseIf Session.Item("localSubscription").crmCloudNotes_Flag Then
                    userNameList.Items.Clear()
                    userNameList.Items.Add(New ListItem(Session.Item("localUser").crmLocalUserFirstName.ToString & " " & Session.Item("localUser").crmLocalUserLastName, Session.Item("localUser").crmUserContactID))
                End If



                'Fill up the left hand menu
                generateItemDetails(masterPage)
            End If

            ' insert or update
            entryDate.Text = Now()

            If bIsUpdate Then  ' get info (enable/disable controls)
                add_note_btn.Text = "Update"
                'If this is a note update
                If bIsNote Or bIsProspect Then
                    If Not Page.IsPostBack Then
                        current_or_previous_date.SelectedValue = "previous"
                        If Not Page.IsPostBack Then
                            Dim jsString As String = " $(function() {"
                            jsString += "$(""#" & txtDateID.ClientID & """).datepicker({"
                            jsString += " showOn: ""button"", "
                            jsString += " buttonImage: ""/images/final.jpg"","
                            jsString += " buttonImageOnly: true,"
                            jsString += " buttonText: ""Select date"""
                            jsString += " });"
                            jsString += " } );"
                            System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType, "dateString", jsString, True)
                        End If

                    End If
                    current_or_previous_date_SelectedIndexChanged(current_or_previous_date, System.EventArgs.Empty)

                    If String.IsNullOrEmpty(notes_edit.Text) Then
                        SavedNoteInformation(bIsNote, reminderID, Session.Item("localSubscription").crmServerSideNotes_Flag, Session.Item("localSubscription").crmCloudNotes_Flag)
                        userNameList.SelectedValue = Session.Item("nSelectedNoteCRMUserID")
                    End If

                    statUsed.Visible = False
                    statUsedLbl.Visible = False

                Else
                    'If this is an action item update.
                    If String.IsNullOrEmpty(notes_edit.Text) Then
                        SavedNoteInformation(bIsNote, reminderID, Session.Item("localSubscription").crmServerSideNotes_Flag, Session.Item("localSubscription").crmCloudNotes_Flag)
                        userNameList.SelectedValue = Session.Item("nSelectedReminderCRMUserID")
                    End If

                    statUsed.Visible = True
                    statUsedLbl.Visible = True

                End If

            Else ' clear form (enable/disable controls)
                'if this is an insert
                add_note_btn.Text = "Save"


                If String.IsNullOrEmpty(txtDateID.Text) Then
                    entryTime.SelectedValue = CInt(FormatDateTime(Now(), DateFormat.ShortTime).ToString.Substring(0, 2)).ToString
                End If

                If bIsNote Or bIsProspect Then
                    'If this is a note insert.
                    If String.IsNullOrEmpty(txtDateID.Text) Then
                        txtDateID.Text = Now.ToShortDateString
                    End If

                    statUsed.Visible = False
                    statUsedLbl.Visible = False

                Else
                    If String.IsNullOrEmpty(txtDateID.Text) Then
                        'if this is an action item insert.
                        txtDateID.Text = Now.ToShortDateString
                    End If

                    statUsed.Visible = False
                    statUsedLbl.Visible = False

                End If

            End If


            pertaining_to_lbl.Text = IIf(bIsNote, TypeOfNote & " Note by", TypeOfNote & " Action Item by")

            notes_edit.Focus()
        Catch ex As Exception

            itemErrorLblID.Text = "Error in " + IIf(bIsNote, TypeOfNote & " Note", TypeOfNote & " Action Item") + " Page_load " + ex.Message.Trim

        End Try

    End Sub

    ''' <summary>
    ''' This function fills in the Edit information, basically repopulating the form (based on note or reminder) with the stored information.
    ''' </summary>
    ''' <param name="bIsnote"></param>
    ''' <param name="nItemID"></param>
    ''' <remarks></remarks>
    Protected Sub SavedNoteInformation(ByVal bIsnote As Boolean, ByVal nItemID As Long, ByVal Plus As Boolean, ByVal Standard As Boolean)
        Dim masterPage As EmptyEvoTheme = DirectCast(Page.Master, EmptyEvoTheme)
        Try
            Dim TempNoteTable As New DataTable


            'Set up the Edit Information 
            If Plus Then 'If they're plus + notes users.
                TempNoteTable = aclsData_Temp.Get_Local_Notes_Client_NoteID(reminderID)
            ElseIf Standard Then 'if they're standard cloud users
                TempNoteTable = aclsData_Temp.Get_CloudNotes_ByID(0, "", reminderID)
            End If

            If Not IsNothing(TempNoteTable) Then
                If TempNoteTable.Rows.Count > 0 Then

                    'We need a check here. This checks to see if the user on the note matches the user's contact ID.
                    If HttpContext.Current.Session.Item("localUser").crmUserContactID <> TempNoteTable.Rows(0).Item("lnote_user_id") Then
                        'Let's only add this check for cloud notes.
                        If Session.Item("localSubscription").crmCloudNotes_Flag Then
                            'Let's set view mode only if they aren't administrators.
                            If Not HttpContext.Current.Session.Item("localUser").crmUserType = eUserTypes.ADMINISTRATOR Then
                                remove_note.Visible = False
                                add_note_btn.Visible = False
                                masterPage.SetPageTitle(Replace(NoteTitle, "Edit", "View"))
                            End If

                            userNameList.Items.Clear()
                            userNameList.Items.Add(New ListItem(TempNoteTable.Rows(0).Item("lnote_user_name"), TempNoteTable.Rows(0).Item("lnote_user_id")))

                        End If
                    End If

                    'get the selected user if selected
                    If Not (IsDBNull(TempNoteTable.Rows(0).Item("lnote_user_id"))) Then
                        Dim nlnote_user_id = CInt(TempNoteTable.Rows(0).Item("lnote_user_id").ToString)

                        If bIsnote Or bIsProspect Then
                            If CInt(Session.Item("nSelectedNoteCRMUserID")) = 0 And nlnote_user_id > 0 Then
                                Session.Item("nSelectedNoteCRMUserID") = nlnote_user_id
                                Session.Item("nSelectedReminderCRMUserID") = nlnote_user_id
                            ElseIf CInt(Session.Item("nSelectedNoteCRMUserID")) <> nlnote_user_id Then
                                Session.Item("nSelectedNoteCRMUserID") = nlnote_user_id
                            End If
                        Else
                            If CInt(Session.Item("nSelectedReminderCRMUserID")) = 0 And nlnote_user_id > 0 Then
                                Session.Item("nSelectedNoteCRMUserID") = nlnote_user_id
                                Session.Item("nSelectedReminderCRMUserID") = nlnote_user_id
                            ElseIf CInt(Session.Item("nSelectedReminderCRMUserID")) <> nlnote_user_id Then
                                Session.Item("nSelectedReminderCRMUserID") = nlnote_user_id
                            End If

                        End If

                    End If

                    If bIsnote Or bIsProspect Then

                        If Not (IsDBNull(TempNoteTable.Rows(0).Item("lnote_status"))) Then
                            If TempNoteTable.Rows(0).Item("lnote_status").ToString.ToUpper = "A" Or TempNoteTable.Rows(0).Item("lnote_status").ToString.ToUpper = "B" Then

                                If Not (IsDBNull(TempNoteTable.Rows(0).Item("lnote_note"))) Then
                                    notes_edit.Text = TempNoteTable.Rows(0).Item("lnote_note").ToString.Trim
                                    invis_note_text.Text = notes_edit.Text
                                End If

                                If Not (IsDBNull(TempNoteTable.Rows(0).Item("lnote_entry_date"))) Then
                                    txtDateID.Text = FormatDateTime(TempNoteTable.Rows(0).Item("lnote_entry_date").ToString, DateFormat.ShortDate)
                                    ' need to fix for proper time zone
                                    Dim offset As Date = CDate(TempNoteTable.Rows(0).Item("lnote_entry_date").ToString)

                                    If offset.ToShortTimeString.Contains("PM") Then
                                        DateAdd("h", 12, offset)
                                    End If

                                    entryTime.SelectedValue = CInt(FormatDateTime(offset, DateFormat.ShortTime).ToString.Substring(0, 2)).ToString

                                End If


                            End If

                        End If


                        statUsed.Visible = False

                    Else ' lnote_status must = P

                        If Not (IsDBNull(TempNoteTable.Rows(0).Item("lnote_status"))) Then
                            If TempNoteTable.Rows(0).Item("lnote_status").ToString.ToUpper = "P" Then

                                If Not (IsDBNull(TempNoteTable.Rows(0).Item("lnote_note"))) Then
                                    notes_edit.Text = TempNoteTable.Rows(0).Item("lnote_note").ToString.Trim
                                End If

                                If Not (IsDBNull(TempNoteTable.Rows(0).Item("lnote_action_date"))) Then
                                    entryDate.Text = FormatDateTime(TempNoteTable.Rows(0).Item("lnote_action_date").ToString, DateFormat.ShortDate)
                                End If

                                If Not (IsDBNull(TempNoteTable.Rows(0).Item("lnote_schedule_start_date"))) Then

                                    txtDateID.Text = FormatDateTime(TempNoteTable.Rows(0).Item("lnote_schedule_start_date").ToString, DateFormat.ShortDate)
                                    ' need to fix for proper time zone
                                    Dim offset As Date = CDate(TempNoteTable.Rows(0).Item("lnote_schedule_start_date").ToString)

                                    If offset.ToShortTimeString.Contains("PM") Then
                                        DateAdd("h", 12, offset)
                                    End If

                                    entryTime.SelectedValue = CInt(FormatDateTime(offset, DateFormat.ShortTime).ToString.Substring(0, 2)).ToString

                                End If

                            End If

                            statUsed.Visible = True

                        End If

                    End If

                End If
            End If

        Catch ex As Exception

            itemErrorLblID.Text = "Error in " + IIf(bIsnote, "Aircraft Note", "Aircraft Action Item") + " Lookup " + nItemID.ToString + " | " + ex.Message.Trim

        End Try

    End Sub

    ''' <summary>
    ''' This is the function that runs when the add button is clicked. 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub add_note_btn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles add_note_btn.Click
        Try
            Dim crm_ac_id As Long = 0
            Dim crm_comp_id As Long = 0
            Dim crm_ac_amod_id As Long = 0
            Dim LocalNote As New clsLocal_Notes
            Dim enterTime As String = ""

            'Figure out the entry time for previous date notes + action items.
            If CInt(entryTime.Text) > 12 Then
                enterTime = (CInt(entryTime.Text) - 12).ToString + ":00 PM"
            ElseIf CInt(entryTime.Text) = 12 Then
                enterTime = "12:00 PM"
            ElseIf CInt(entryTime.Text) = 0 Then
                enterTime = "12:00 AM"
            Else
                enterTime = entryTime.Text + ":00 AM"
            End If

            itemErrorLblID.Text = "" 'Clearing the note error field in case there's a new error (or isn't).
            action_to_note_warning.Text = "" 'Also clearing the notes to action item field, in case there's a new warning on this.

            'Some error catching to make sure the note isn't blank.
            If String.IsNullOrEmpty(notes_edit.Text) Then
                itemErrorLblID.Text = "<br /><p align='center'>Error " + IIf(bIsNote, TypeOfNote & " Note", TypeOfNote & " Action Item") + " can not be blank!</p>"
                notes_edit.Focus()
                Exit Sub
            End If

            'This should technically be taken care of 
            'in the javascript.
            'However even in the best of circumstances, client side validation could fail.
            'In order to double verify, we go to server side validation.
            'If the regular expression matches, we stop.
            If Regex.IsMatch(notes_edit.Text, "/<|>|\|\||&&/gi") Then 'This simply looks for <, >, || or &&. The gi means globally and case insensitive.
                itemErrorLblID.Text = "<br /><p align='center'>Error: Notes must not contain the following characters: <, >, ||, &&</p>"
                notes_edit.Focus()
                Exit Sub
            End If



            'Error catching to check date.
            If String.IsNullOrEmpty(txtDateID.Text) Then
                itemErrorLblID.Text = "<br /><p align='center'>Error " + IIf(bIsNote, TypeOfNote & " Note", TypeOfNote & " Action Item") + " date can not be blank!</p>"
                txtDateID.Focus()
                Exit Sub
            End If

            If bIsNote Or bIsProspect Then
                'if this is note insert, requires a date today or prior
                If (UCase(current_or_previous_date.SelectedValue) = "PREVIOUS" And CDate(txtDateID.Text & " " & FormatDateTime(enterTime, 3)) > CDate(Now())) Or (UCase(current_or_previous_date.SelectedValue) = "CURRENT" And CDate(entryDate.Text) > CDate(Now())) Then
                    action_to_note_warning.Text = "<p align='center' class='remove_margin padding'>" & TypeOfNote & " note requires a date today or prior.</p>"
                    txtDateID.Focus()
                    Exit Sub
                End If
            End If

            'if this is an update:
            If bIsUpdate Then
                If Not bIsNote Or bIsProspect Then
                    'if this is an action item that we're marking as complete, the date has to be today or prior.
                    If Me.statUsed.SelectedValue.ToString.ToUpper = "A" Then
                        If CDate(txtDateID.Text) > CDate(Now().ToShortDateString) Then
                            action_to_note_warning.Text = "<p align='center' class='remove_margin padding'>Completing an Action Item will store it as a Note and requires a date today or prior.</p>"
                            txtDateID.Focus()
                            Exit Sub
                        End If
                    End If
                End If
            Else
                If Not bIsNote Or bIsProspect Then
                    'if this is an action item, needs a date of today or after.
                    If CDate(txtDateID.Text) < CDate(Now().ToShortDateString) Then
                        action_to_note_warning.Text = "<p align='center' class='remove_margin padding'>" & TypeOfNote & " Action Item requires a date today or after.</p>"
                        txtDateID.Focus()
                        Exit Sub
                    End If
                End If

            End If

            'If the aircraft ID is set, we need to check for corresponding CRM Aircraft Information.
            'This only needs to be set if we're a server notes client (cloud notes +)
            If Session.Item("localSubscription").crmServerSideNotes_Flag Then
                If AircraftID > 0 Then
                    get_crm_client_aircraft_info(CLng(AircraftID), crm_ac_id, crm_ac_amod_id)
                End If
                'if the Company ID is set, we need to check for corresponding CRM Company Information.
                If Trim(Request("source")) = "CLIENT" Then
                    If CompanyID > 0 Then
                        crm_comp_id = CLng(CompanyID)
                        CompanyID = 0
                        get_crm_evo_client_company_info(CompanyID, crm_comp_id)
                    End If
                Else
                    If CompanyID > 0 Then
                        get_crm_client_company_info(CLng(CompanyID), crm_comp_id)
                    End If
                End If
            End If

            If bIsNote Or bIsProspect Then
                'if this is a note, set up entry date, status
                Dim enterDate As New Date
                If UCase(current_or_previous_date.SelectedValue) = "PREVIOUS" Then
                    enterDate = CDate(txtDateID.Text & " " & FormatDateTime(enterTime, 3))
                Else
                    enterDate = CDate(entryDate.Text)
                End If

                Dim ActionDate As String = Year(entryDate.Text).ToString + "-" + Month(entryDate.Text).ToString + "-" + Day(entryDate.Text).ToString + " " + FormatDateTime(entryDate.Text, 3).ToString
                Dim DateOfEntry As String = enterDate.Year().ToString + "-" + enterDate.Month().ToString + "-" + enterDate.Day().ToString + " " + FormatDateTime(enterDate, 3).ToString

                LocalNote.lnote_entry_date = DateOfEntry
                LocalNote.lnote_action_date = ActionDate

                If bIsNote Then
                    LocalNote.lnote_status = "A"
                Else
                    LocalNote.lnote_status = "B"
                End If
            Else
                'if this is an action item, setting up dates, entry date, scheduled date, status
                ' if Action Item status is "C" then set entry date to be selected date ...
                Dim enterDate As Date = Now()

                Dim scheduleDate As Date = CDate(txtDateID.Text + " " + enterTime)

                Dim mySQLenterDate As String = enterDate.Year().ToString + "-" + enterDate.Month().ToString + "-" + enterDate.Day().ToString + " " + FormatDateTime(enterDate, 3)
                Dim mySQLscheduleDate As String = scheduleDate.Year().ToString + "-" + scheduleDate.Month().ToString + "-" + scheduleDate.Day().ToString + " " + FormatDateTime(scheduleDate, 3)
                Dim mySQLnowDate As String = Now.Year().ToString + "-" + Now.Month().ToString + "-" + Now.Day().ToString + " " + FormatDateTime(Now, 3)

                If bIsUpdate = False Then
                    LocalNote.lnote_entry_date = mySQLenterDate
                Else
                    enterDate = CDate(entryDate.Text)
                End If

                LocalNote.lnote_action_date = mySQLnowDate
                LocalNote.lnote_schedule_start_date = mySQLscheduleDate
                LocalNote.lnote_status = statUsed.SelectedValue.ToString.ToUpper

            End If

            'fill up the rest of the notes class.
            LocalNote.lnote_clipri_ID = 1
            LocalNote.lnote_user_id = userNameList.SelectedValue
            LocalNote.lnote_user_login = userNameList.SelectedValue
            LocalNote.lnote_user_name = userNameList.SelectedItem.Text

            LocalNote.lnote_note = notes_edit.Text.Trim

            LocalNote.lnote_jetnet_ac_id = AircraftID
            LocalNote.lnote_jetnet_comp_id = CompanyID
            LocalNote.lnote_jetnet_yacht_id = YachtID
            If YachtID <> 0 Then
                LocalNote.lnote_jetnet_yacht_model_id = IIf(IsNumeric(YachtModelID.Text), YachtModelID.Text, 0)
            End If

            LocalNote.lnote_client_comp_id = crm_comp_id

            LocalNote.lnote_client_ac_id = crm_ac_id

            LocalNote.lnote_jetnet_amod_id = IIf(IsNumeric(ModelID.Text), ModelID.Text, 0)
            LocalNote.lnote_client_amod_id = crm_ac_amod_id
            LocalNote.lnote_notecat_key = 23

            'if the reminder ID isn't zero, go ahead and update
            Dim ReturnedValue As Boolean = False

            If Session.Item("localSubscription").crmServerSideNotes_Flag Then
                If reminderID <> 0 Then
                    LocalNote.lnote_id = reminderID
                    If Not IsNothing(aclsData_Temp.update_localNote(LocalNote)) Then
                        ReturnedValue = True
                    End If
                Else
                    'otherwise insert.
                    If Not IsNothing(aclsData_Temp.Insert_Note(LocalNote)) Then
                        ReturnedValue = True
                    End If
                End If
            ElseIf Session.Item("localSubscription").crmCloudNotes_Flag Then
                If reminderID <> 0 Then
                    LocalNote.lnote_id = reminderID
                    If Not IsNothing(aclsData_Temp.UpdateStandardCloudNoteForUser(LocalNote)) Then
                        ReturnedValue = True
                    End If
                Else
                    'otherwise insert.
                    If Not IsNothing(aclsData_Temp.Insert_StandardCloudNote(LocalNote)) Then
                        ReturnedValue = True
                    End If
                End If
            End If

            If ReturnedValue = True Then
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.location.reload(true);", True)
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)
            Else
                itemErrorLblID.Text = "<br /><p align='center'>There was an error " & IIf(reminderID <> 0, "editing", "inserting") & " your " & TypeOfNote & " " & IIf(bIsNote, "Note", "Action Item") & "</p>"
            End If
        Catch ex As Exception

            itemErrorLblID.Text = "Error in " + IIf(bIsNote, "" & TypeOfNote & " Note", "" & TypeOfNote & " Action Item") + " add/update | " + ex.Message.Trim

        End Try

    End Sub

    ''' <summary>
    ''' This function returns the CRM Aircraft Info based on a JetnetAC ID, it also returns the client MODEL ID, which is stored along with the ac ID.
    ''' </summary>
    ''' <param name="jetnet_ac_id"></param>
    ''' <param name="client_ac_id"></param>
    ''' <param name="client_ac_amod_id"></param>
    ''' <remarks></remarks>
    Protected Sub get_crm_client_aircraft_info(ByVal jetnet_ac_id As Long, ByRef client_ac_id As Long, ByRef client_ac_amod_id As Long)
        Try
            Dim TempACTable As New DataTable
            TempACTable = aclsData_Temp.Get_Client_Aircraft_JETNET_AC(jetnet_ac_id)

            If Not IsNothing(TempACTable) Then
                If TempACTable.Rows.Count > 0 Then
                    If Not (IsDBNull(TempACTable.Rows(0).Item("cliaircraft_id"))) Then
                        client_ac_id = TempACTable.Rows(0).Item("cliaircraft_id")
                    End If

                    If Not (IsDBNull(TempACTable.Rows(0).Item("cliaircraft_cliamod_id"))) Then
                        client_ac_amod_id = TempACTable.Rows(0).Item("cliaircraft_cliamod_id")
                    End If
                End If
            End If


        Catch ex As Exception

            itemErrorLblID.Text = "Error in " + IIf(bIsNote, "" & TypeOfNote & " Note", "" & TypeOfNote & " Action Item") + " ac Lookup " + jetnet_ac_id.ToString + " | " + ex.Message.Trim & " | comp lookup: " & CompanyID.ToString

        End Try

    End Sub

    ''' <summary>
    ''' This function returns the client company ID based on a jetnet ID. Much like the function above, if there's a corresponding client ID, it will be saved along with the jetnet ID.
    ''' </summary>
    ''' <param name="jetnet_comp_id"></param>
    ''' <param name="client_comp_id"></param>
    ''' <remarks></remarks>
    Protected Sub get_crm_client_company_info(ByVal jetnet_comp_id As Long, ByRef client_comp_id As Long)
        Try
            Dim TempACTable As New DataTable
            TempACTable = aclsData_Temp.CheckforCompanyBy_JETNET_ID(jetnet_comp_id, "")

            If Not IsNothing(TempACTable) Then
                If TempACTable.Rows.Count > 0 Then
                    If Not (IsDBNull(TempACTable.Rows(0).Item("comp_id"))) Then
                        client_comp_id = (TempACTable.Rows(0).Item("comp_id"))
                    End If
                End If
            End If


        Catch ex As Exception

            itemErrorLblID.Text = "Error in " + IIf(bIsNote, "" & TypeOfNote & " Note", "" & TypeOfNote & " Action Item") + " ac Lookup " + AircraftID.ToString + " | " + ex.Message.Trim & " | comp lookup: " & CompanyID.ToString

        End Try

    End Sub

    Protected Sub get_crm_evo_client_company_info(ByRef jetnet_comp_id As Long, ByVal client_comp_id As Long)
        Try
            Dim TempACTable As New DataTable
            TempACTable = aclsData_Temp.CheckforCompanyBy_CLIENT_ID(client_comp_id, "")

            If Not IsNothing(TempACTable) Then
                If TempACTable.Rows.Count > 0 Then
                    If Not (IsDBNull(TempACTable.Rows(0).Item("clicomp_jetnet_comp_id"))) Then
                        jetnet_comp_id = (TempACTable.Rows(0).Item("clicomp_jetnet_comp_id"))
                    End If
                End If
            End If


        Catch ex As Exception

            itemErrorLblID.Text = "Error in " + IIf(bIsNote, "" & TypeOfNote & " Note", "" & TypeOfNote & " Action Item") + " ac Lookup " + AircraftID.ToString + " | " + ex.Message.Trim & " | comp lookup: " & CompanyID.ToString

        End Try

    End Sub


    ''' <summary>
    ''' This function is for removing the note. It uses the Reminder ID that gets passed to the page, removes the note if it's not 0 and refreshes/closes the page.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param> 
    ''' <remarks></remarks>
    Private Sub remove_note_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles remove_note.Click
        Dim aclsLocal_Notes As New clsLocal_Notes
        Dim Refresh As Boolean = False

        aclsLocal_Notes.lnote_id = reminderID
        If reminderID <> 0 Then
            If Session.Item("localSubscription").crmServerSideNotes_Flag Then 'this is the notes + remove
                Refresh = aclsData_Temp.Delete_LocalNote(aclsLocal_Notes)

            ElseIf Session.Item("localSubscription").crmCloudNotes_Flag Then 'standard notes remove
                Refresh = aclsData_Temp.Delete_StandardCloudNote(aclsLocal_Notes)
            End If


            If Refresh Then
                Call commonLogFunctions.Log_User_Event_Data("UserNoteDelete", "User Removed " & IIf(bIsNote, "Note " & "", "Action Item") & " ID #" & reminderID.ToString & ", DELETED NOTE: " & Replace(invis_note_text.Text, "'", "") & "", Nothing, 0, 0, 0, 0)
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.location.reload(true);", True)
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)
            Else
                itemErrorLblID.Text = "<br /><p align='center'>There was an error removing your " & TypeOfNote & " " & IIf(bIsNote, "Note", "Action Item") & "</p>"
            End If
        End If

    End Sub

    Private Sub current_or_previous_date_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles current_or_previous_date.SelectedIndexChanged
        If UCase(current_or_previous_date.SelectedValue) = "PREVIOUS" Then
            Dim jsString As String = ""

            'This initializes the datepicker.

            jsString += "$(""#" & txtDateID.ClientID & """).datepicker({"
            jsString += " showOn: ""button"", "
            jsString += " buttonImage: ""/images/final.jpg"","
            jsString += " buttonImageOnly: true,"
            jsString += " maxDate: '0', "
            jsString += " buttonText: ""Select date"""
            jsString += " });"
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me.update_notes_date, Me.GetType, "dateStringPostback", jsString, True)

            current_date_label.CssClass = "display_none"
            previous_date_text.CssClass = "display_block"

        Else
            current_date_label.CssClass = "display_block"
            previous_date_text.CssClass = "display_none"
        End If
    End Sub


End Class