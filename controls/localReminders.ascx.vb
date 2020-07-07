Partial Public Class localReminders

    '' ********************************************************************************
    '' Copyright 2004-11. JETNET,LLC. All rights reserved.
    ''
    ''$$Archive: /commonWebProject/controls/localReminders.ascx.vb $
    ''$$Author: Mike $
    ''$$Date: 6/19/19 8:46a $
    ''$$Modtime: 6/18/19 6:12p $
    ''$$Revision: 2 $
    ''$$Workfile: localReminders.ascx.vb $
    ''
    '' ********************************************************************************

    Inherits System.Web.UI.UserControl

    'Private reminderID As Long = 0
    'Public aclsData_Temp As New clsData_Manager_SQL

    ''  'Public postBackPage As String = ""

    ''  'Public reminderAircraftID As Long = 0
    ''  'Public currentBrowseRec As Long = 0
    ''  'Public fromView As String = ""
    ''  'Public extraURLParm As String = ""

    ''  'Public bUseEditInsertForm As Boolean = False

    ''  'Const _STARTCHARWIDTH As Double = 6.5
    ''  'Const _STARTMAXWIDTH As Double = (20 * _STARTCHARWIDTH)

    ''  'Public Event updateReminderDataSource As EventHandler ' event to signal grid update
    ''  'Public Event insertReminderDataSource As EventHandler ' event to signal grid insert

    ''  'Public Event openInsertControl As EventHandler ' event to signal open insert control
    ''  'Public Event openEditControl As EventHandler ' event to signal open edit control

    ''Protected Overridable Sub OnInsertReminderDataSource(ByVal e As EventArgs)
    ''      'RaiseEvent insertReminderDataSource(Me, e)
    ''End Sub

    ''Protected Overridable Sub OnUpdateReminderDataSource(ByVal e As EventArgs)
    ''      'RaiseEvent updateReminderDataSource(Me, e)
    ''End Sub

    ''Protected Overridable Sub OnOpenInsertControl(ByVal e As EventArgs)
    ''      'RaiseEvent openInsertControl(Me, e)
    ''End Sub

    ''Protected Overridable Sub OnOpenEditControl(ByVal e As EventArgs)
    ''      'RaiseEvent openEditControl(Me, e)
    ''End Sub

    ''Public Sub turnOffHideReminder(ByVal bToggle As Boolean)

    ''      'If bToggle Then
    ''      '  hideReminderBtnID.Visible = False
    ''      'Else
    ''      '  hideReminderBtnID.Visible = True
    ''      'End If

    ''End Sub

    ''Public Sub rebindDataSource()

    ''      'If Not IsNothing(reminderGridView) Then
    ''      '  reminderGridView.DataBind()
    ''      'End If

    ''End Sub

    'Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs)

    '    Try
    '        aclsData_Temp = New clsData_Manager_SQL
    '        aclsData_Temp.JETNET_DB = Application.Item("crmJetnetDatabase")
    '        aclsData_Temp.client_DB = Application.Item("crmJetnetServerNotes")

    '        If Not IsNothing(Request.Item("lnoteID")) Then
    '            If Not String.IsNullOrEmpty(Request.Item("lnoteID").ToString) Then
    '                If IsNumeric(Request.Item("lnoteID").Trim) Then
    '                    reminderID = Request.Item("lnoteID").Trim
    '                End If
    '            End If
    '        End If


    '        If Not IsPostBack Then
    '            BindGridView()


    '            '      '    If Not bUseEditInsertForm Then
    '            '      '      Session.Item("nSelectedReminderID") = 0
    '            '      '    End If

    '            '      '    Session.Item("lastReminderID") = reminderAircraftID

    '            '      '    ' check if this crm user is in user table ... if not add them ... return client user id
    '            '      '    ' use client user id to set selected item in username dropdown

    '            '      '    ' user was not found get first name and last name from EVO contact table ...
    '            '      '    Dim sQuery As String = "SELECT contact_first_name, contact_last_name, contact_email_address FROM Contact WITH(NOLOCK) WHERE contact_active_flag = 'Y' AND contact_id = '" + Session.Item("localSubscription").evoUserContactID.ToString + "'"

    '            '      '    Try
    '            '      '      SqlConnection.ConnectionString = Session.Item("localSubscription").evoUserDatabaseConn
    '            '      '      SqlConnection.Open()

    '            '      '      SqlCommand.Connection = SqlConnection
    '            '      '      SqlCommand.CommandTimeout = 1000
    '            '      '      SqlCommand.CommandText = sQuery.ToString

    '            '      '      lDataReader = SqlCommand.ExecuteReader()

    '            '      '      If lDataReader.HasRows Then

    '            '      '        Do While lDataReader.Read()
    '            '      '          If Not (IsDBNull(lDataReader("contact_first_name"))) Then
    '            '      '            sFirstName = lDataReader.Item("contact_first_name")
    '            '      '          End If

    '            '      '          If Not (IsDBNull(lDataReader("contact_last_name"))) Then
    '            '      '            sLastName = lDataReader.Item("contact_last_name")
    '            '      '          End If

    '            '      '          If Not (IsDBNull(lDataReader("contact_email_address"))) Then
    '            '      '            sEmailAddress = lDataReader.Item("contact_email_address")
    '            '      '          End If
    '            '      '        Loop ' lDataReader.HasRows

    '            '      '      End If

    '            '      '    Catch SqlException

    '            '      '      SqlConnection.Dispose()
    '            '      '      SqlCommand.Dispose()

    '            '      '    Finally

    '            '      '      SqlCommand.Dispose()
    '            '      '      SqlConnection.Close()
    '            '      '      SqlConnection.Dispose()

    '            '      '    End Try

    '            '      '    lDataReader = Nothing
    '            '      '    SqlCommand = Nothing

    '            '      '    Session.Item("nSelectedReminderCRMUserID") = commonEvo.get_crm_client_info(sEmailAddress, sFirstName, sLastName, reminderErrorLblID.Text)
    '            '      '    Session.Item("localSubscription").crmUserID = Session.Item("nSelectedReminderCRMUserID")

    '            '      '  Else

    '            '      '    If reminderAircraftID = 0 And CLng(Session.Item("lastReminderID").ToString) > 0 Then
    '            '      '      reminderAircraftID = Session.Item("lastReminderID")
    '            '      '    ElseIf CLng(Session.Item("lastReminderID").ToString) <> reminderAircraftID Then
    '            '      '      Session.Item("lastReminderID") = reminderAircraftID
    '            '      '    End If

    '        End If

    '        '      '  If Session.Item("nSelectedReminderCRMUserID") > 0 Then
    '        '      '    If Not IsNothing(reminderGridView.Controls(0).Controls(0).FindControl("userNameList")) Then
    '        '      '      CType(reminderGridView.Controls(0).Controls(0).FindControl("userNameList"), DropDownList).SelectedValue = Session.Item("nSelectedReminderCRMUserID")
    '        '      '    End If
    '        '      '  End If

    '        '      '  If Session.Item("nSelectedReminderCRMUserID") > 0 Then
    '        '      '    If Not IsNothing(reminderGridView.FooterRow) Then
    '        '      '      If Not IsNothing(reminderGridView.FooterRow.FindControl("userNameList")) Then
    '        '      '        CType(reminderGridView.FooterRow.FindControl("userNameList"), DropDownList).SelectedValue = Session.Item("nSelectedReminderCRMUserID")
    '        '      '      End If
    '        '      '    End If
    '        '      '  End If

    '        '      '  reminderErrorLblID.Text = "Use 'ADD ACTION' to save a new Action"

    '        '      '  If currentBrowseRec > 0 Then
    '        '      '    hideReminderBtnID.PostBackUrl = "~/" + postBackPage.Trim + "?currec=" + currentBrowseRec.ToString + "&acid=0&jid=0&ShowReminder=N&fromView=" + fromView + extraURLParm
    '        '      '  Else
    '        '      '    hideReminderBtnID.PostBackUrl = "~/" + postBackPage.Trim + "?currec=0&acid=" + reminderAircraftID.ToString + "&jid=0&ShowReminder=N&fromView=" + fromView + extraURLParm
    '        '      '  End If

    '        '      '  newReminder.Visible = True

    '    Catch ex As Exception

    '        reminderErrorLblID.Text = "Local Aircraft Action Item Page Load Error : " + ex.Message.ToString

    '    End Try

    'End Sub

    'Private Sub BindGridView()
    '    Dim TempNoteTable As New DataTable
    '    TempNoteTable = aclsData_Temp.Get_Local_Notes_Client_NoteID(reminderID)
    '    If Not IsNothing(TempNoteTable) Then
    '        If TempNoteTable.Rows.Count > 0 Then
    '            reminderGridView.DataSource = TempNoteTable
    '            reminderGridView.EditIndex = 0
    '            reminderGridView.DataBind()

    '        End If
    '    End If
    'End Sub

    ''Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

    ''      'If Not IsNothing(reminderGridView.FooterRow) Then
    ''      '  If reminderGridView.FooterRow.Visible Then
    ''      '    newReminder.Visible = False
    ''      '  End If
    ''      'Else
    ''      '  newReminder.Visible = False
    ''      'End If

    ''End Sub

    ''Private Sub ObjectDataSource2_Inserting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceMethodEventArgs) Handles ObjectDataSource2.Inserting

    ''      'Dim crm_ac_id As Integer = 0
    ''      'Dim crm_ac_amod_id As Integer = 0

    ''      'get_crm_client_aircraft_info(CLng(Session.Item("lastReminderID")), crm_ac_id, crm_ac_amod_id)

    ''      'e.InputParameters("lnote_jetnet_ac_id") = CInt(Session.Item("lastReminderID"))
    ''      'e.InputParameters("lnote_jetnet_comp_id") = 0
    ''      'e.InputParameters("lnote_client_ac_id") = crm_ac_id
    ''      'e.InputParameters("lnote_client_comp_id") = 0
    ''      'e.InputParameters("lnote_jetnet_contact_id") = 0
    ''      'e.InputParameters("lnote_client_contact_id") = 0
    ''      'e.InputParameters("lnote_note") = Session.Item("sNewReminderString").ToString.Trim
    ''      'e.InputParameters("lnote_entry_date") = Now().ToString
    ''      'e.InputParameters("lnote_action_date") = Now().ToString
    ''      'e.InputParameters("lnote_user_login") = Session.Item("nSelectedReminderUserID").ToString
    ''      'e.InputParameters("lnote_user_name") = IIf(String.IsNullOrEmpty(Session.Item("sSelectedReminderUserName")), Nothing, Session.Item("sSelectedReminderUserName"))
    ''      'e.InputParameters("lnote_notecat_key") = 23
    ''      'e.InputParameters("lnote_status") = Session.Item("sNewReminderStatus")
    ''      'e.InputParameters("lnote_schedule_start_date") = CDate(Session.Item("sNewReminderDate")).ToString
    ''      'e.InputParameters("lnote_schedule_end_date") = Now().ToString
    ''      'e.InputParameters("lnote_user_id") = Session.Item("nSelectedReminderUserID")
    ''      'e.InputParameters("lnote_clipri_ID") = 1
    ''      'e.InputParameters("lnote_document_flag") = "N"
    ''      'e.InputParameters("lnote_jetnet_amod_id") = CInt(commonEvo.GetAircraftInfo(CLng(Session.Item("lastReminderID")), True))
    ''      'e.InputParameters("lnote_client_amod_id") = crm_ac_amod_id

    ''      'Call OnInsertReminderDataSource(New EventArgs())

    ''End Sub

    ''Private Sub ObjectDataSource2_ObjectCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceEventArgs) Handles ObjectDataSource2.ObjectCreated
    ''      'Try

    ''      '  If e.ObjectInstance IsNot Nothing Then

    ''      '    Dim conn As MySql.Data.MySqlClient.MySqlConnection = New MySql.Data.MySqlClient.MySqlConnection

    ''      '    conn.ConnectionString = Session.Item("localSubscription").evoServerNotesDatabaseConn

    ''      '    e.ObjectInstance.GetType().GetProperty("Connection").SetValue(e.ObjectInstance, conn, Nothing)

    ''      '  End If

    ''      'Catch ex As Exception

    ''      'End Try

    ''End Sub

    ''Private Sub ObjectDataSource1_ObjectCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceEventArgs) Handles ObjectDataSource1.ObjectCreated

    ''      'Try

    ''      '  If e.ObjectInstance IsNot Nothing Then

    ''      '    Dim conn As MySql.Data.MySqlClient.MySqlConnection = New MySql.Data.MySqlClient.MySqlConnection

    ''      '    conn.ConnectionString = Session.Item("localSubscription").evoServerNotesDatabaseConn

    ''      '    e.ObjectInstance.GetType().GetProperty("Connection").SetValue(e.ObjectInstance, conn, Nothing)

    ''      '  End If

    ''      'Catch ex As Exception

    ''      'End Try

    ''End Sub

    ''Private Sub reminderGridView_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles reminderGridView.RowCancelingEdit

    ''      'If e.RowIndex > 0 Then
    ''      '  Dim RowinDex As TableCell = reminderGridView.Rows(e.RowIndex).Cells(0)
    ''      '  CType(RowinDex.FindControl("txtReminderDate"), TextBox).BackColor = Drawing.Color.White
    ''      'End If

    ''      'reminderGridView.EditIndex = -1
    ''      'reminderGridView.DataBind()
    ''      'reminderErrorLblID.Text = "Press 'ADD ACTION ITEM' to save a new Aircraft Action Item"

    ''End Sub

    ''Private Sub reminderGridView_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles reminderGridView.RowCommand

    ''      'Dim sNewReminder As String = ""
    ''      'Dim sUserName As String = ""
    ''      'Dim nUserNameID As Integer = 0

    ''      'Dim sReminderDate As String = ""

    ''      'If e.CommandName = "NoDataInsert" Then

    ''      '  If bUseEditInsertForm Then

    ''      '    ' raise event to open edit/insert control
    ''      '    OnOpenInsertControl(New EventArgs())
    ''      '    Exit Sub

    ''      '  Else

    ''      '    If Not IsNothing(reminderGridView.Controls(0).Controls(0).FindControl("NoDataReminder")) Then
    ''      '      sNewReminder = CType(reminderGridView.Controls(0).Controls(0).FindControl("NoDataReminder"), TextBox).Text.Trim
    ''      '    End If

    ''      '    If Not IsNothing(reminderGridView.Controls(0).Controls(0).FindControl("userNameList")) Then
    ''      '      sUserName = CType(reminderGridView.Controls(0).Controls(0).FindControl("userNameList"), DropDownList).SelectedItem.ToString.Trim
    ''      '      nUserNameID = CType(reminderGridView.Controls(0).Controls(0).FindControl("userNameList"), DropDownList).SelectedValue
    ''      '    End If

    ''      '    If Not IsNothing(reminderGridView.Controls(0).Controls(0).FindControl("txtReminderDate")) Then
    ''      '      sReminderDate = CType(reminderGridView.Controls(0).Controls(0).FindControl("txtReminderDate"), TextBox).Text.Trim
    ''      '    End If

    ''      '    If Not String.IsNullOrEmpty(sNewReminder) And nUserNameID > 0 And Not String.IsNullOrEmpty(sReminderDate) Then
    ''      '      Session.Item("sNewReminderString") = sNewReminder
    ''      '      Session.Item("sSelectedReminderUserName") = sUserName
    ''      '      Session.Item("nSelectedReminderUserID") = nUserNameID

    ''      '      Session.Item("sNewReminderDate") = sReminderDate
    ''      '      Session.Item("sNewReminderStatus") = "P"

    ''      '      If Session.Item("nSelectedReminderUserID") = 0 Then
    ''      '        Session.Item("sSelectedReminderUserName") = ""
    ''      '      End If

    ''      '      ObjectDataSource2.Insert()
    ''      '      reminderGridView.DataBind()

    ''      '      If reminderGridView.PageCount > 1 Then
    ''      '        reminderGridView.PageIndex = reminderGridView.PageCount - 1
    ''      '      End If

    ''      '      reminderErrorLblID.Text = "A New Action Item was Added"

    ''      '    ElseIf String.IsNullOrEmpty(sReminderDate) Then
    ''      '      Session.Item("sNewNoteString") = sNewReminder
    ''      '      Session.Item("nSelectedReminderUserID") = nUserNameID
    ''      '      reminderErrorLblID.Text = "Action Item Date cannot be blank"
    ''      '    ElseIf String.IsNullOrEmpty(sNewReminder) Then
    ''      '      Session.Item("sNewNoteDate") = sReminderDate
    ''      '      Session.Item("nSelectedReminderUserID") = nUserNameID
    ''      '      reminderErrorLblID.Text = "Action Item Text cannot be blank"
    ''      '    Else
    ''      '      reminderErrorLblID.Text = "Please Select User Name"
    ''      '    End If

    ''      '  End If

    ''      'ElseIf e.CommandName = "Insert" Then

    ''      '  If Not IsNothing(reminderGridView.FooterRow.FindControl("AddReminder")) Then
    ''      '    sNewReminder = CType(reminderGridView.FooterRow.FindControl("AddReminder"), TextBox).Text.Trim
    ''      '  End If

    ''      '  If Not IsNothing(reminderGridView.FooterRow.FindControl("userNameList")) Then
    ''      '    sUserName = CType(reminderGridView.FooterRow.FindControl("userNameList"), DropDownList).SelectedItem.ToString.Trim
    ''      '    nUserNameID = CType(reminderGridView.FooterRow.FindControl("userNameList"), DropDownList).SelectedValue
    ''      '  End If

    ''      '  If Not IsNothing(reminderGridView.FooterRow.FindControl("txtReminderDate")) Then
    ''      '    sReminderDate = CType(reminderGridView.FooterRow.FindControl("txtReminderDate"), TextBox).Text.Trim
    ''      '  End If

    ''      '  If Not String.IsNullOrEmpty(sNewReminder) And nUserNameID > 0 And Not String.IsNullOrEmpty(sReminderDate) Then
    ''      '    Session.Item("sNewReminderString") = sNewReminder
    ''      '    Session.Item("sSelectedReminderUserName") = sUserName
    ''      '    Session.Item("nSelectedReminderUserID") = nUserNameID

    ''      '    Session.Item("sNewReminderDate") = sReminderDate
    ''      '    Session.Item("sNewReminderStatus") = "P"

    ''      '    If Session.Item("nSelectedReminderUserID") = 0 Then
    ''      '      Session.Item("sSelectedReminderUserName") = ""
    ''      '    End If

    ''      '    ObjectDataSource2.Insert()
    ''      '    reminderGridView.DataBind()

    ''      '    If reminderGridView.PageCount > 1 Then
    ''      '      reminderGridView.PageIndex = reminderGridView.PageCount - 1
    ''      '    End If

    ''      '    reminderErrorLblID.Text = "A New Action Item was Added"

    ''      '  ElseIf String.IsNullOrEmpty(sReminderDate) Then
    ''      '    Session.Item("sNewNoteString") = sNewReminder
    ''      '    Session.Item("nSelectedReminderUserID") = nUserNameID
    ''      '    reminderErrorLblID.Text = "Action Item Date cannot be blank"
    ''      '  ElseIf String.IsNullOrEmpty(sNewReminder) Then
    ''      '    Session.Item("sNewNoteDate") = sReminderDate
    ''      '    Session.Item("nSelectedReminderUserID") = nUserNameID
    ''      '    reminderErrorLblID.Text = "Action Item Text cannot be blank"
    ''      '  Else
    ''      '    reminderErrorLblID.Text = "Please Select User Name"
    ''      '  End If

    ''      'End If

    ''End Sub

    ''Private Sub reminderGridView_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles reminderGridView.RowEditing

    ''      'Dim nReminderID As Integer = CInt(reminderGridView.DataKeys(e.NewEditIndex)("lnote_id").ToString())

    ''      'If bUseEditInsertForm Then
    ''      '  Session.Item("nSelectedReminderID") = nReminderID

    ''      '  ' raise event to open edit/insert control
    ''      '  e.Cancel = True
    ''      '  OnOpenEditControl(New EventArgs())

    ''      'Else
    ''      '  reminderGridView.EditIndex = e.NewEditIndex
    ''      '  reminderGridView.DataBind()
    ''      '  reminderErrorLblID.Text = "Editing Action Item : " + nReminderID.ToString
    ''      '  newReminder.Visible = False
    ''      'End If

    ''End Sub

    ''Private Sub reminderGridView_RowUpdated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdatedEventArgs) Handles reminderGridView.RowUpdated
    ''      'reminderGridView.DataBind()
    ''      'reminderErrorLblID.Text = "Action Item : " + Session.Item("nSelectedReminderID").ToString + " Was Updated"
    ''End Sub

    ''Protected Sub reminderGridView_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles reminderGridView.RowDataBound

    ''      'If e.Row.RowState = DataControlRowState.Alternate Or e.Row.RowState = DataControlRowState.Normal Then

    ''      '  If e.Row.RowType = DataControlRowType.DataRow Then ' if our row type is data row then fill in the row

    ''      '    'determine the value of the lnote_status field
    ''      '    Dim reminderStatus As String = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "lnote_status"))
    ''      '    Dim reminderText As String = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "lnote_note"))

    ''      '    Select Case (reminderStatus)
    ''      '      Case "P"
    ''      '        e.Row.Cells(5).Text = "Pending"
    ''      '      Case "A"
    ''      '        e.Row.Cells(5).Text = "Active"
    ''      '      Case "D"
    ''      '        e.Row.Cells(5).Text = "Dismissed"
    ''      '    End Select

    ''      '    e.Row.Cells(3).ToolTip = reminderText

    ''      '  End If

    ''      'End If

    ''End Sub

    ''Private Sub reminderGridView_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles reminderGridView.RowUpdating

    ''      'Dim RowinDex As TableCell = reminderGridView.Rows(e.RowIndex).Cells(0)

    ''      'Dim sNewReminder As String = CType(RowinDex.FindControl("EditReminder"), TextBox).Text
    ''      'Dim nReminderID As Integer = CInt(reminderGridView.DataKeys(e.RowIndex)("lnote_id").ToString())

    ''      'Dim sReminderEntryDate As String = CType(RowinDex.FindControl("txtEntryDate"), TextBox).Text

    ''      'Dim sReminderDate As String = CType(RowinDex.FindControl("txtReminderDate"), TextBox).Text
    ''      'Dim sReminderStatus = CType(RowinDex.FindControl("userReminderStatus"), DropDownList).SelectedValue

    ''      'If String.IsNullOrEmpty(sNewReminder) Or String.IsNullOrEmpty(sReminderDate) Then

    ''      '  e.Cancel = True

    ''      '  If (reminderGridView.PageIndex = reminderGridView.PageCount - 1 And reminderGridView.EditIndex = reminderGridView.Rows.Count - 1) Then
    ''      '    reminderGridView.DeleteRow(e.RowIndex)
    ''      '  End If

    ''      'ElseIf CDate(sReminderDate) > CDate(Now().ToShortDateString) Then
    ''      '  e.Cancel = True

    ''      '  CType(RowinDex.FindControl("txtReminderDate"), TextBox).Focus()
    ''      '  CType(RowinDex.FindControl("txtReminderDate"), TextBox).BackColor = Drawing.Color.Salmon

    ''      '  reminderErrorLblID.Text = "Completing an Aircraft Action Item will store it as an Aircraft Note and requires a date today or prior"

    ''      'Else

    ''      '  Session.Item("nSelectedReminderID") = nReminderID
    ''      '  Session.Item("sSelectedReminderEntryDate") = sReminderEntryDate
    ''      '  Session.Item("sNewReminderString") = sNewReminder
    ''      '  Session.Item("sNewReminderDate") = sReminderDate
    ''      '  Session.Item("sNewReminderStatus") = sReminderStatus

    ''      '  ObjectDataSource2.Update()

    ''      '  Call OnUpdateReminderDataSource(New EventArgs())

    ''      'End If

    ''End Sub

    ''Private Sub ObjectDataSource2_Updating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceMethodEventArgs) Handles ObjectDataSource2.Updating

    ''      'e.InputParameters.Item("lnote_note") = Session.Item("sNewReminderString").ToString.Trim
    ''      'e.InputParameters.Item("lnote_action_date") = Now().ToString

    ''      'e.InputParameters.Item("lnote_status") = Session.Item("sNewReminderStatus").ToString

    ''      'If Session.Item("sNewReminderStatus").ToString.ToUpper = "A" Then
    ''      '  e.InputParameters("lnote_entry_date") = CDate(Session.Item("sNewReminderDate").ToString).ToString
    ''      '  e.InputParameters.Item("lnote_schedule_start_date") = Now().ToString
    ''      'ElseIf Session.Item("sNewReminderStatus").ToString.ToUpper = "D" Then
    ''      '  e.InputParameters("lnote_entry_date") = CDate(Session.Item("sSelectedReminderEntryDate").ToString)
    ''      '  e.InputParameters.Item("lnote_schedule_start_date") = Now().ToString
    ''      'ElseIf Session.Item("sNewReminderStatus").ToString.ToUpper = "P" Then
    ''      '  e.InputParameters("lnote_entry_date") = CDate(Session.Item("sSelectedReminderEntryDate").ToString)
    ''      '  e.InputParameters.Item("lnote_schedule_start_date") = CDate(Session.Item("sNewReminderDate").ToString).ToString
    ''      'End If

    ''      'e.InputParameters.Item("Original_lnote_id") = Session.Item("nSelectedReminderID").ToString.Trim

    ''End Sub

    ''Protected Sub get_crm_client_aircraft_info(ByVal jetnet_ac_id As Long, ByRef client_ac_id As Integer, ByRef client_ac_amod_id As Integer)

    ''      '' look up client database info from CRM client_register_master table
    ''      'Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
    ''      'Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
    ''      'Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing
    ''      'Dim MySqlReader As MySql.Data.MySqlClient.MySqlDataReader : MySqlReader = Nothing

    ''      'Dim sQuery As String = "SELECT cliaircraft_id, cliaircraft_cliamod_id  FROM client_aircraft WHERE cliaircraft_jetnet_ac_id = " + jetnet_ac_id.ToString

    ''      'Try

    ''      '  Try

    ''      '    MySqlConn.ConnectionString = Session.Item("localSubscription").evoServerNotesDatabaseConn.ToString()
    ''      '    MySqlConn.Open()

    ''      '    MySqlCommand.Connection = MySqlConn
    ''      '    MySqlCommand.CommandType = CommandType.Text
    ''      '    MySqlCommand.CommandTimeout = 60
    ''      '    MySqlCommand.CommandText = sQuery

    ''      '    MySqlReader = MySqlCommand.ExecuteReader()

    ''      '    If MySqlReader.HasRows Then

    ''      '      MySqlReader.Read()

    ''      '      If Not (IsDBNull(MySqlReader("cliaircraft_id"))) Then
    ''      '        client_ac_id = MySqlReader.Item("cliaircraft_id")
    ''      '      End If

    ''      '      If Not (IsDBNull(MySqlReader("cliaircraft_cliamod_id"))) Then
    ''      '        client_ac_amod_id = MySqlReader.Item("cliaircraft_cliamod_id")
    ''      '      End If

    ''      '      MySqlReader.Close()
    ''      '      MySqlReader.Dispose()

    ''      '    End If 'MySqlReader.HasRows 

    ''      '  Catch MySqlException

    ''      '    MySqlConn.Dispose()
    ''      '    MySqlCommand.Dispose()

    ''      '  Finally

    ''      '    MySqlConn.Close()
    ''      '    MySqlCommand.Dispose()
    ''      '    MySqlConn.Dispose()

    ''      '  End Try

    ''      'Catch ex As Exception

    ''      '  reminderErrorLblID.Text = "Error in JETNET ac Lookup " + jetnet_ac_id.ToString + " | " + ex.Message.Trim

    ''      'End Try

    ''End Sub

    ''Protected Sub newReminder_Click(ByVal sender As Object, ByVal e As EventArgs) Handles newReminder.Click

    ''      'Session.Item("nSelectedReminderID") = 0

    ''      'If bUseEditInsertForm Then
    ''      '  ' raise event to open edit/insert control
    ''      '  OnOpenInsertControl(New EventArgs())
    ''      '  Exit Sub
    ''      'Else
    ''      '  If Not IsNothing(reminderGridView.FooterRow) Then
    ''      '    reminderGridView.FooterRow.Visible = True
    ''      '  End If
    ''      'End If

    ''      'reminderErrorLblID.Text = "Press 'ADD ACTION ITEM' to save a new Aircraft Action Item"

    ''End Sub

    ''Private Sub reminderGridView_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles reminderGridView.RowCreated

    ''      'If (e.Row.Cells.Count > 1) Then
    ''      '  e.Row.Cells(1).Visible = False
    ''      '  e.Row.Cells(2).Visible = False
    ''      '  e.Row.Cells(5).Visible = False
    ''      'End If


    ''End Sub

End Class