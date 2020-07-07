
' ********************************************************************************
' Copyright 2004-19. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/adminActions.aspx.vb $
'$$Author: Matt $
'$$Date: 5/28/20 3:41p $
'$$Modtime: 5/28/20 2:55p $
'$$Revision: 52 $
'$$Workfile: adminActions.aspx.vb $
'
' ********************************************************************************

Partial Public Class adminActions
  Inherits System.Web.UI.Page

  Private sTask As String = ""
  Private bAddAction As Boolean = False
  Private bEditAction As Boolean = False
  Private bInsertAction As Boolean = False
  Private bUpdateAction As Boolean = False
  Private bConfirmAction As Boolean = False
  Private bDeleteAction As Boolean = False
  Private bSearchActions As Boolean = False
  Private bFromTable As Boolean = False

  Dim actionItems As journalClass = New journalClass
  Dim actionEdit As journalClass = New journalClass
  Dim action As journalClass = New journalClass

  Dim nJournalID As Long = 0
  Dim nCompanyID As Long = 0
  Dim nContactID As Long = 0

  Public Const JOURN_DESCRIPTION_LEN = 4000

  Public Shared masterPage As New Object

  Private Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

    Try

      If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
        Me.MasterPageFile = "~/EvoStyles/CustomerAdminTheme.Master"
        masterPage = DirectCast(Page.Master, CustomerAdminTheme)
      ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Then
        Me.MasterPageFile = "~/EvoStyles/HomebaseTheme.Master"
        masterPage = DirectCast(Page.Master, HomebaseTheme)
      End If

    Catch ex As Exception
      If Not IsNothing(masterPage) Then
        masterPage.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString + " : " + ex.Message.ToString.Trim)
      Else
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodInfo.GetCurrentMethod().ToString + "</b><br />" + ex.Message.ToString.Trim
      End If
    End Try

  End Sub

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Dim sErrorString As String = ""

    Try

      ' get request variable
      If Session.Item("crmUserLogon") <> True Then
        Response.Redirect("Default.aspx", False)
      Else

        If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
          Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("User Actions Dashboard")
        ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Then
          Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("User Actions Dashboard")
        End If

        If Not Session.Item("localPreferences").loadUserSession(sErrorString, CLng(HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString),
                                                              HttpContext.Current.Session.Item("localUser").crmUserLogin.ToString,
                                                              CLng(HttpContext.Current.Session.Item("localUser").crmSubSeqNo.ToString),
                                                              CLng(HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString)) Then
          Response.Redirect("Default.aspx", True)
        End If

      End If

      If Not IsNothing(Request.Item("journalid")) Then
        If Not String.IsNullOrEmpty(Request.Item("journalid").ToString.Trim) Then
          If IsNumeric(Request.Item("journalid").ToString) Then
            nJournalID = CLng(Request.Item("journalid").ToString)
          End If
        End If
      End If

      If Not IsNothing(Request.Item("companyid")) Then
        If Not String.IsNullOrEmpty(Request.Item("companyid").ToString.Trim) Then
          If IsNumeric(Request.Item("companyid").ToString) Then
            nCompanyID = CLng(Request.Item("companyid").ToString)
          End If
        End If
      End If

      If Not IsNothing(Request.Item("contactid")) Then
        If Not String.IsNullOrEmpty(Request.Item("contactid").ToString.Trim) Then
          If IsNumeric(Request.Item("contactid").ToString) Then
            nContactID = CLng(Request.Item("contactid").ToString)
          End If
        End If
      End If

      If Not IsNothing(Request.Item("fromTable")) Then
        If Not String.IsNullOrEmpty(Request.Item("fromTable").ToString.Trim) Then
          bFromTable = CBool(Request.Item("fromTable").ToString)
        End If
      End If

      If Not IsNothing(Request.Item("task")) Then
        If Not String.IsNullOrEmpty(Request.Item("task").ToString.Trim) Then

          sTask = Request.Item("task").ToString.Trim

          If sTask.ToLower.Contains("results") Then
            bSearchActions = True
            Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("User Actions Results")
          End If

          If sTask.ToLower.Contains("edit") Then
            bEditAction = True
            saveBtn.PostBackUrl = "~/adminActions.aspx?task=update&fromTable=" + bFromTable.ToString.ToLower + "&journalid=" + nJournalID.ToString + "&companyid=" + nCompanyID.ToString + "&contactid=" + nContactID.ToString
            confirmBtn.PostBackUrl = "~/adminActions.aspx?task=confirm&fromTable=" + bFromTable.ToString.ToLower + "&journalid=" + nJournalID.ToString + "&companyid=&contactid="
            deleteBtn.PostBackUrl = "~/adminActions.aspx?task=delete&fromTable=" + bFromTable.ToString.ToLower + "&journalid=" + nJournalID.ToString + "&companyid=&contactid="
            Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("Edit Action")
          End If

          If sTask.ToLower.Contains("add") Then
            bAddAction = True
            saveBtn.PostBackUrl = "~/adminActions.aspx?task=insert&companyid=" + nCompanyID.ToString + "&contactid=" + nContactID.ToString
            confirmBtn.Visible = False
            deleteBtn.Visible = False
            Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("Add New Action")
          End If

          If sTask.ToLower.Contains("insert") Then
            bInsertAction = True
          End If

          If sTask.ToLower.Contains("update") Then
            bUpdateAction = True
          End If

          If sTask.ToLower.Contains("delete") Then
            bDeleteAction = True
          End If

          If sTask.ToLower.Contains("confirm") Then
            bConfirmAction = True
          End If

        End If

      End If

      If bSearchActions Then

        actions_tabPanel0.Visible = True
        actions_tabPanel1.Visible = False

        actionItems.journ_subcategory_code = "AIAI"

        If Not String.IsNullOrEmpty(action_start_date.Text.Trim) Then
          actionItems.journ_date = action_start_date.Text.Trim
        End If

        If Not String.IsNullOrEmpty(action_end_date.Text.Trim) Then
          actionItems.journ_end_date = action_end_date.Text.Trim
        End If

        If Not String.IsNullOrEmpty(action_users.SelectedValue.Trim) Then
          actionItems.journ_user_id = action_users.SelectedValue
        End If

        Session.Item("actionCriteria") = actionItems

        actionItems.fillJournalClass(True)

        fillUserList(215, action_users)

        display_journal_table(actSearchResultsTable_tabPanel0.Text)

      ElseIf bAddAction Then

        actions_tabPanel0.Visible = False
        actions_tabPanel1.Visible = True
        TableCell0.Visible = False

        fillUserList(215, ListBox1)

        fill_insert_tab()

      ElseIf bEditAction Then

        actionEdit = New journalClass

        actions_tabPanel0.Visible = False
        actions_tabPanel1.Visible = True
        Label1.Text = "Edit Action"
        TableCell0.Visible = False

        actionEdit.journ_id = nJournalID
        actionEdit.journ_comp_id = nCompanyID
        actionEdit.journ_contact_id = nContactID
                ' actionEdit.journ_subcategory_code = "AIAI"   - shouldnt need- should be AIAI or RALT 

                actionEdit.fillJournalClass(True)

        fillUserList(215, ListBox1)

        display_edit_tab()

      ElseIf bDeleteAction Then

        action.journ_id = nJournalID
        action.deleteJournalRecord(True)

        If bFromTable Then

          actions_tabPanel0.Visible = True
          actions_tabPanel1.Visible = False
          TableCell0.Visible = True

          actionItems.journ_subcategory_code = "AIAI"

          If Not String.IsNullOrEmpty(Session.Item("actionCriteria").journ_date.Trim) Then
            actionItems.journ_date = Session.Item("actionCriteria").journ_date.Trim
            action_start_date.Text = actionItems.journ_date
          End If

          If Not String.IsNullOrEmpty(Session.Item("actionCriteria").journ_end_date.Trim) Then
            actionItems.journ_end_date = Session.Item("actionCriteria").journ_end_date.Trim
            action_end_date.Text = actionItems.journ_end_date
          End If

          If Not String.IsNullOrEmpty(Session.Item("actionCriteria").journ_user_id.Trim) Then
            actionItems.journ_user_id = Session.Item("actionCriteria").journ_user_id.Trim
          End If

          actionItems.fillJournalClass(True)

          fillUserList(215, action_users)

          display_journal_table(actSearchResultsTable_tabPanel0.Text)

        End If

      ElseIf bConfirmAction Then

        action.journ_id = nJournalID
        action.journ_date = Now.ToString
        action.journ_action_date = Now.ToString
        action.journ_subcategory_code = "MN"
        action.journ_subcat_code_part1 = "MN"
        action.journ_subcat_code_part2 = ""
        action.journ_subcat_code_part3 = ""
        action.journ_subject = "MN - Marketing Representative Note"
        action.journ_status = "A"

        If TextBox4.Text.Length < JOURN_DESCRIPTION_LEN Then
          action.journ_description = TextBox4.Text.Trim
        Else
          action.journ_description = TextBox4.Text.Substring(0, JOURN_DESCRIPTION_LEN - 1)
        End If

        action.setJournalActionToMarketingNote(True)

        If bFromTable Then

          actions_tabPanel0.Visible = True
          actions_tabPanel1.Visible = False
          TableCell0.Visible = True

          actionItems.journ_subcategory_code = "AIAI"

          If Not String.IsNullOrEmpty(Session.Item("actionCriteria").journ_date.Trim) Then
            actionItems.journ_date = Session.Item("actionCriteria").journ_date.Trim
            action_start_date.Text = actionItems.journ_date
          End If

          If Not String.IsNullOrEmpty(Session.Item("actionCriteria").journ_end_date.Trim) Then
            actionItems.journ_end_date = Session.Item("actionCriteria").journ_end_date.Trim
            action_end_date.Text = actionItems.journ_end_date
          End If

          If Not String.IsNullOrEmpty(Session.Item("actionCriteria").journ_user_id.Trim) Then
            actionItems.journ_user_id = Session.Item("actionCriteria").journ_user_id.Trim
          End If

          actionItems.fillJournalClass(True)

          fillUserList(215, action_users)

          display_journal_table(actSearchResultsTable_tabPanel0.Text)

        End If

      ElseIf bInsertAction Then

        action.journ_comp_id = nCompanyID


                If Type_DropDown.SelectedIndex = 0 Then
                    action.journ_subcategory_code = "AIAI"
                    action.journ_subcat_code_part1 = "AI"
                    action.journ_subcat_code_part2 = "AI"
                    action.journ_subject = "Action Item"
                ElseIf Type_DropDown.SelectedIndex = 1 Then
                    action.journ_subcategory_code = "RALT"
                    action.journ_subcat_code_part1 = "RA"
                    action.journ_subcat_code_part2 = "LT"
                    action.journ_subject = "Research Action"
                End If




                If Not String.IsNullOrEmpty(ListBox3.SelectedValue.Trim) Then
          action.journ_contact_id = CLng(ListBox3.SelectedValue)
        End If

        If Not String.IsNullOrEmpty(ListBox1.SelectedValue.Trim) Then
          action.journ_user_id = ListBox1.SelectedValue.Trim
        End If

        action.journ_description = Left(TextBox4.Text.Trim, 4000)

        action.journ_entry_date = FormatDateTime(entry_date.Text.Trim, DateFormat.ShortDate)
        action.journ_entry_time = IIf(String.IsNullOrEmpty(entry_time.SelectedValue), Now.ToLongTimeString, entry_time.SelectedValue)

        action.journ_date = action.journ_entry_date + " " + action.journ_entry_time
        action.journ_action_date = Now.ToString

        action.insertJournalRecord(True)

      ElseIf bUpdateAction Then

        action.journ_id = nJournalID
                action.journ_comp_id = nCompanyID

                If Type_DropDown.SelectedIndex = 0 Then
                    action.journ_subcategory_code = "AIAI"
                    action.journ_subcat_code_part1 = "AI"
                    action.journ_subcat_code_part2 = "AI"
                    action.journ_subject = "Action Item"
                ElseIf Type_DropDown.SelectedIndex = 1 Then
                    action.journ_subcategory_code = "RALT"
                    action.journ_subcat_code_part1 = "RA"
                    action.journ_subcat_code_part2 = "LT"
                    action.journ_subject = "Research Action"
                End If



                If Not String.IsNullOrEmpty(ListBox3.SelectedValue.Trim) Then
                    action.journ_contact_id = CLng(ListBox3.SelectedValue)
                End If

                If Not String.IsNullOrEmpty(ListBox1.SelectedValue.Trim) Then
                    action.journ_user_id = ListBox1.SelectedValue.Trim
                End If

                action.journ_description = Left(TextBox4.Text.Trim, 4000)

        action.journ_entry_date = FormatDateTime(entry_date.Text.Trim, DateFormat.ShortDate)
        action.journ_entry_time = IIf(String.IsNullOrEmpty(entry_time.SelectedValue), Now.ToLongTimeString, entry_time.SelectedValue)

        action.journ_date = action.journ_entry_date + " " + action.journ_entry_time

        action.journ_action_date = Now.ToString

        action.updateJournalRecord(True)

        If bFromTable Then

          actions_tabPanel0.Visible = True
          actions_tabPanel1.Visible = False
          TableCell0.Visible = True

          actionItems.journ_subcategory_code = "AIAI"

          If Not String.IsNullOrEmpty(Session.Item("actionCriteria").journ_date.Trim) Then
            actionItems.journ_date = Session.Item("actionCriteria").journ_date.Trim
            action_start_date.Text = actionItems.journ_date
          End If

          If Not String.IsNullOrEmpty(Session.Item("actionCriteria").journ_end_date.Trim) Then
            actionItems.journ_end_date = Session.Item("actionCriteria").journ_end_date.Trim
            action_end_date.Text = actionItems.journ_end_date
          End If

          If Not String.IsNullOrEmpty(Session.Item("actionCriteria").journ_user_id.Trim) Then
            actionItems.journ_user_id = Session.Item("actionCriteria").journ_user_id.Trim
          End If

          actionItems.fillJournalClass(True)

          fillUserList(215, action_users)

          display_journal_table(actSearchResultsTable_tabPanel0.Text)

        End If

      Else

        If Not IsNothing(Session.Item("homebaseUserClass")) Then
          If Not String.IsNullOrEmpty(Session.Item("homebaseUserClass").home_user_id.ToString.Trim) Then

            actionItems.journ_subcategory_code = "AIAI"

            actionItems.journ_user_id = Session.Item("homebaseUserClass").home_user_id.ToString.ToLower

            Session.Item("actionCriteria") = actionItems

            actionItems.fillJournalClass(True)

            fillUserList(215, action_users)

            If Not String.IsNullOrEmpty(action_users.SelectedValue) Then

              actions_tabPanel0.Visible = True
              actions_tabPanel1.Visible = False
              display_journal_table(actSearchResultsTable_tabPanel0.Text)

            End If


          End If
        End If

      End If


    Catch ex As Exception
      If Not IsNothing(masterPage) Then
        masterPage.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString + " : " + ex.Message.ToString.Trim)
      Else
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodInfo.GetCurrentMethod().ToString + "</b><br />" + ex.Message.ToString.Trim
      End If
    End Try

  End Sub

  Public Function getUsersDataTable() As DataTable

    Dim atemptable As New DataTable
    Dim subQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      subQuery.Append("SELECT user_last_name, user_first_name, [user_id], user_type FROM [user] WITH(NOLOCK) WHERE user_email_address <> '' AND user_password <> 'inactive' ORDER BY user_last_name ASC")

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 90

      SqlCommand.CommandText = subQuery.ToString
      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()

        Return Nothing

      End Try

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message

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

  Public Sub fillUserList(ByRef maxWidth As Long, ByRef userList As ListBox)

    Dim results_table As New DataTable

    Try

      userList.Items.Clear()

      results_table = getUsersDataTable()

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          userList.Items.Add(New ListItem("", ""))

          For Each r As DataRow In results_table.Rows

            If Not IsDBNull(r.Item("user_last_name")) Then
              If Not String.IsNullOrEmpty(r.Item("user_last_name").ToString.Trim) Then

                userList.Items.Add(New ListItem(r.Item("user_first_name").ToString.Trim + " " + r.Item("user_last_name").ToString.Trim, r.Item("user_id").ToString.ToLower))

              End If
            End If

          Next
        End If
      End If

      If Not String.IsNullOrEmpty(actionItems.journ_user_id.Trim) Then
        userList.SelectedValue = actionItems.journ_user_id.Trim
      End If

      userList.Width = maxWidth

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message

    Finally

    End Try

    results_table = Nothing

  End Sub

  Private Sub adminActions_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
    Try

      Dim DatePickerOptions As String = ""
      Dim JavascriptOnLoad As String = ""

      DatePickerOptions = "{" + vbNewLine
      DatePickerOptions += " showOn: ""button"", " + vbNewLine
      DatePickerOptions += " buttonImage: ""/images/final.jpg""," + vbNewLine
      DatePickerOptions += " buttonImageOnly: true," + vbNewLine
      DatePickerOptions += " buttonText: ""Select date""" + vbNewLine
      DatePickerOptions += " }" + vbNewLine

      If sTask.ToLower.Contains("results") Then
        JavascriptOnLoad += vbCrLf + "$(""#" + action_start_date.ClientID.Trim + """).datepicker(" + DatePickerOptions + ");"
        JavascriptOnLoad += vbCrLf + "$(""#" + action_end_date.ClientID.Trim + """).datepicker(" + DatePickerOptions + ");"
        JavascriptOnLoad += vbCrLf + "CreateSearchTable(""tabPanel0_InnerTable"",""tabPanel0_DataTable"",""tabPanel0_jQueryTable"");"
        JavascriptOnLoad += vbCrLf + "CloseLoadingMessage(""DivLoadingMessage"");"
      ElseIf sTask.ToLower.Contains("edit") Or sTask.ToLower.Contains("add") Then
        JavascriptOnLoad += vbCrLf + "$(""#" + entry_date.ClientID.Trim + """).datepicker(" + DatePickerOptions + ");"
        If bFromTable Then
          JavascriptOnLoad += vbCrLf + "CloseLoadingMessage(""DivLoadingMessage"");"
        End If
      ElseIf sTask.ToLower.Contains("update") Or sTask.ToLower.Contains("delete") Or sTask.ToLower.Contains("confirm") Then
        If Not bFromTable Then
          'JavascriptOnLoad += vbCrLf + "alert('update');"
          JavascriptOnLoad += vbCrLf + "if ((typeof (parentWindow) != ""undefined"") && (parentWindow != null)) {"
          JavascriptOnLoad += vbCrLf + "  try { // call the fnRefreshPage on the parent window"
          JavascriptOnLoad += vbCrLf + "    parentWindow.fnRefreshPage();"
          'JavascriptOnLoad += vbCrLf + "    alert('update opener');"
          JavascriptOnLoad += vbCrLf + "  }"
          JavascriptOnLoad += vbCrLf + "  catch (err) { // if that fails then"
          'JavascriptOnLoad += vbCrLf + "  alert('update no opener');"
          JavascriptOnLoad += vbCrLf + "  }"
          JavascriptOnLoad += vbCrLf + "}"
          JavascriptOnLoad += vbCrLf + "window.close();"
        Else
          JavascriptOnLoad += vbCrLf + "$(""#" + action_start_date.ClientID.Trim + """).datepicker(" + DatePickerOptions + ");"
          JavascriptOnLoad += vbCrLf + "$(""#" + action_end_date.ClientID.Trim + """).datepicker(" + DatePickerOptions + ");"
          JavascriptOnLoad += vbCrLf + "CreateSearchTable(""tabPanel0_InnerTable"",""tabPanel0_DataTable"",""tabPanel0_jQueryTable"");"
        End If
      ElseIf sTask.ToLower.Contains("insert") Then
        'JavascriptOnLoad += vbCrLf + "alert('insert');"
        JavascriptOnLoad += vbCrLf + "if ((typeof (parentWindow) != ""undefined"") && (parentWindow != null)) {"
        JavascriptOnLoad += vbCrLf + "  try { // call the fnRefreshPage on the parent window"
        JavascriptOnLoad += vbCrLf + "    parentWindow.fnRefreshPage();"
        'JavascriptOnLoad += vbCrLf + "    alert('insert opener');"
        JavascriptOnLoad += vbCrLf + "  }"
        JavascriptOnLoad += vbCrLf + "  catch (err) { // if that fails then"
        'JavascriptOnLoad += vbCrLf + "  alert('insert no opener');"
        JavascriptOnLoad += vbCrLf + "  }"
        JavascriptOnLoad += vbCrLf + "}"
        JavascriptOnLoad += vbCrLf + "window.close();"

      Else
        JavascriptOnLoad += vbCrLf + "$(""#" + action_start_date.ClientID.Trim + """).datepicker(" + DatePickerOptions + ");"
        JavascriptOnLoad += vbCrLf + "$(""#" + action_end_date.ClientID.Trim + """).datepicker(" + DatePickerOptions + ");"

        If Not IsNothing(Session.Item("homebaseUserClass")) Then
          If Not String.IsNullOrEmpty(Session.Item("homebaseUserClass").home_user_id.ToString.Trim) Then
            If Not String.IsNullOrEmpty(action_users.SelectedValue.Trim) Then
              JavascriptOnLoad += vbCrLf + "CreateSearchTable(""tabPanel0_InnerTable"",""tabPanel0_DataTable"",""tabPanel0_jQueryTable"");"
            End If
          End If
        End If

      End If

      If Not Page.ClientScript.IsClientScriptBlockRegistered("onLoadCode") Then
        System.Web.UI.ScriptManager.RegisterStartupScript(Me.actions_tabContainer, Me.GetType(), "onLoadCode", "$(document).ready(function() {" + JavascriptOnLoad.ToString + "});", True)
      End If

    Catch ex As Exception
      If Not IsNothing(masterPage) Then
        masterPage.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString + " (Page_PreRender): " + ex.Message.ToString)
      Else
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += " (Page_PreRender): " + ex.Message.ToString.Trim
      End If
    End Try
  End Sub

  Public Sub display_journal_table(ByRef out_htmlString As String, Optional ByVal isMobileDisplay As Boolean = False)

    Dim htmlOut As New StringBuilder
    Dim toggleRowColor As Boolean = False

    Try

      out_htmlString = ""

      If Not IsNothing(actionItems.resultsTable) Then

        If actionItems.resultsTable.Rows.Count > 0 Then

          htmlOut.Append("<table id=""tabPanel0_DataTable"" cellpadding=""0"" cellspacing=""0"" border=""0"" width=""100%"">")
          htmlOut.Append("<thead><tr>")
          htmlOut.Append("<th><span class=""help_cursor"" title=""Used to select and remove item from the list"">SEL</span></th>")

          If isMobileDisplay Then
            htmlOut.Append("<th></th>")
            htmlOut.Append("<th></th>")
          Else
            htmlOut.Append("<th width=""10""></th>")
            htmlOut.Append("<th width=""10"" class=""text_align_center"">EDIT</th>")
            htmlOut.Append("<th data-priority=""1"" width=""89"">ACTION DATE</th>")
            htmlOut.Append("<th width=""150"">COMPANY</th>")
            htmlOut.Append("<th width=""150"">CONTACT</th>")
            htmlOut.Append("<th>DETAILS</th>")
            htmlOut.Append("<th width=""100"">ASSIGNED</th>")
          End If

          htmlOut.Append("</tr></thead><tbody>")

          Dim sSeparator As String = ""

          For Each r As DataRow In actionItems.resultsTable.Rows

            htmlOut.Append("<tr>")

            htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">&nbsp;</td>")
            htmlOut.Append("<td align=""left"" valign=""top"" nowrap=""nowrap"" class=""text_align_center"">" + r.Item("journ_id").ToString.Trim + "</td>")

            htmlOut.Append("<td align=""left"" valign=""middle"" class=""text_align_center"">")

            htmlOut.Append("<a onclick=""javascript:ShowLoadingMessage('DivLoadingMessage', 'Edit Action', 'Loading ... Please Wait ...');return true;"" href=""adminActions.aspx?task=edit&fromTable=true&journalid=" + r.Item("journ_id").ToString.Trim + "&companyid=" + r.Item("journ_comp_id").ToString.Trim + "&contactid=" + r.Item("journ_contact_id").ToString.Trim + """ title=""Edit Action Item""><img src =""images/edit_icon.png"" alt=""Edit Action Item"" title=""Edit Action Item""></a>")

            htmlOut.Append("</td>")

            htmlOut.Append("<td align=""left"" valign=""top"" nowrap=""nowrap"" data-sort=""" + IIf(Not IsDBNull(r.Item("journ_date")), r.Item("journ_date").ToString, "") + """>")

            If Not IsDBNull(r.Item("journ_date")) Then
              If Not String.IsNullOrEmpty(r.Item("journ_date").ToString.Trim) Then
                htmlOut.Append(r.Item("journ_date").ToString.Trim)
              End If
            End If

            htmlOut.Append("</td>")

            htmlOut.Append("<td align=""left"" valign=""top"">")

            If Not IsDBNull(r.Item("journ_comp_id")) Then
              If Not String.IsNullOrEmpty(r.Item("journ_comp_id").ToString.Trim) Then

                If IsNumeric(r.Item("journ_comp_id").ToString) Then
                  If CLng(r.Item("journ_comp_id").ToString) > 0 Then

                    htmlOut.Append("<a class=""underline distinct"" onclick='javascript:openSmallWindowJS(""DisplayCompanyDetail.aspx?compid=" + r.Item("journ_comp_id").ToString + "&journid=0"",""CompanyDetails"");' title=""Display Company Details"">")
                    htmlOut.Append(r.Item("comp_name").ToString.Trim)
                    htmlOut.Append("</a><br />")

                    Dim Seperator As String = ""
                    If Not (IsDBNull(r("comp_address1"))) And Not String.IsNullOrEmpty(r.Item("comp_address1").ToString.Trim) Then
                      htmlOut.Append(r.Item("comp_address1").ToString.Trim)
                      Seperator = "<br />"
                    End If

                    If Not (IsDBNull(r("comp_address2"))) And Not String.IsNullOrEmpty(r.Item("comp_address2").ToString.Trim) Then
                      htmlOut.Append(" " + r.Item("comp_address2").ToString.Trim)
                      Seperator = "<br />"
                    End If

                    htmlOut.Append(Seperator)
                    Seperator = ""

                    If Not (IsDBNull(r("comp_city"))) And Not String.IsNullOrEmpty(r.Item("comp_city").ToString.Trim) Then
                      htmlOut.Append(r.Item("comp_city").ToString.Trim + ", ")
                    End If

                    If Not (IsDBNull(r("comp_state"))) And Not String.IsNullOrEmpty(r.Item("comp_state").ToString.Trim) Then
                      htmlOut.Append(r.Item("comp_state").ToString.Trim + " ")
                    End If

                    If Not (IsDBNull(r("comp_zip_code"))) And Not String.IsNullOrEmpty(r.Item("comp_zip_code").ToString.Trim) Then
                      htmlOut.Append(r.Item("comp_zip_code").ToString.Trim + " ")
                    End If

                    If Not (IsDBNull(r("comp_country"))) And Not String.IsNullOrEmpty(r.Item("comp_country").ToString.Trim) Then
                      htmlOut.Append(r.Item("comp_country").ToString.Trim)
                    End If

                  End If

                End If

              End If

            End If

            htmlOut.Append("</td>")

            htmlOut.Append("<td align=""left"" valign=""top"">")

            If Not IsDBNull(r.Item("journ_contact_id")) Then
              If Not String.IsNullOrEmpty(r.Item("journ_contact_id").ToString.Trim) Then

                If IsNumeric(r.Item("journ_contact_id").ToString) Then
                  If CLng(r.Item("journ_contact_id").ToString) > 0 Then

                    htmlOut.Append("<a class=""underline distinct"" onclick='javascript:openSmallWindowJS(""DisplayContactDetail.aspx?compid=" + r.Item("journ_comp_id").ToString + "&jid=0&conid=" + r.Item("journ_contact_id").ToString + """,""ContactDetails"");' title=""Display Contact Details"">")

                    htmlOut.Append(r.Item("contact_sirname").ToString.Trim + Constants.cSingleSpace + r.Item("contact_first_name").ToString.Trim + Constants.cSingleSpace)

                    If Not String.IsNullOrEmpty(r.Item("contact_middle_initial").ToString) Then
                      htmlOut.Append(r.Item("contact_middle_initial").ToString.Trim + ". ")
                    End If

                    htmlOut.Append(r.Item("contact_last_name").ToString.Trim)

                    If Not String.IsNullOrEmpty(r.Item("contact_suffix").ToString) Then
                      htmlOut.Append(Constants.cSingleSpace + r.Item("contact_suffix").ToString.Trim)
                    End If

                    htmlOut.Append("</a>")

                    If Not (IsDBNull(r("contact_title"))) And Not String.IsNullOrEmpty(r.Item("contact_title").ToString.Trim) Then
                      htmlOut.Append("<br />" + r.Item("contact_title").ToString.Trim)
                    End If

                    If Not (IsDBNull(r("contact_email_address"))) And Not String.IsNullOrEmpty(r.Item("contact_email_address").ToString.Trim) Then
                      htmlOut.Append("<br /><a href=""mailto:" + r.Item("contact_email_address").ToString.Trim + """>" + r.Item("contact_email_address").ToString.Trim + "</a>")
                    End If

                  End If
                End If

              End If
            End If

            htmlOut.Append("</td>")

            htmlOut.Append("<td align=""left"" valign=""top"">")

            If Not IsDBNull(r.Item("journ_description")) Then
              If Not String.IsNullOrEmpty(r.Item("journ_description").ToString.Trim) Then
                If r.Item("journ_description").ToString.Length < 251 Then
                  htmlOut.Append(r.Item("journ_description").ToString.Trim)
                Else
                  htmlOut.Append(r.Item("journ_description").ToString.Substring(0, 251).Trim + " ...")
                End If
              End If
            End If

            htmlOut.Append("</td>")

            htmlOut.Append("<td align=""left"" valign=""top"" nowrap=""nowrap"">")

            If Not IsDBNull(r.Item("journ_user_id")) Then
              If Not String.IsNullOrEmpty(r.Item("journ_user_id").ToString.Trim) Then ' user_last_name, user_first_name

                htmlOut.Append(r.Item("user_first_name").ToString.Trim + Constants.cSingleSpace + r.Item("user_last_name").ToString.Trim)
                htmlOut.Append("(<em>" + r.Item("journ_user_id").ToString.Trim + "</em>)")

              End If
            End If

            htmlOut.Append("</td>")

            htmlOut.Append("</tr>" + vbCrLf)

          Next

        End If ' _dataTable.Rows.Count > 0 Then

        htmlOut.Append("</tbody></table>")
        htmlOut.Append("<div id=""tabPanel0_Label"" class="""" style=""padding:2px;""><strong>" + actionItems.resultsTable.Rows.Count.ToString + " Records</strong></div>")
        htmlOut.Append("<div id=""tabPanel0_InnerTable"" align=""left"" valign=""middle"" style=""max-height:610px; overflow: auto;""></div>")

      End If

      out_htmlString = htmlOut.ToString

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message

    Finally

      htmlOut = Nothing

    End Try

  End Sub

  Public Sub fill_insert_tab()

    Dim results_table As New DataTable

    Try

      entry_date.Text = Now.ToShortDateString
      entry_time.SelectedValue = Now.Hour.ToString + ":00:00 " + IIf(Now.Hour >= 12, "PM", "AM")

      If Not IsNothing(Session.Item("homebaseUserClass")) Then
        If Not String.IsNullOrEmpty(Session.Item("homebaseUserClass").home_user_id.ToString.Trim) Then
          ListBox1.SelectedValue = Session.Item("homebaseUserClass").home_user_id.ToString.ToLower
        End If
      End If

      If nCompanyID > 0 Then

        commonEvo.get_company_info_fromID(nCompanyID, nJournalID, False, False, "", "", True, results_table)

        If Not IsNothing(results_table) Then

          If results_table.Rows.Count > 0 Then

            For Each r As DataRow In results_table.Rows

              Dim htmlOut As New StringBuilder

              htmlOut.Append(r.Item("comp_name").ToString.Trim)

              Dim Seperator As String = ""
              If Not (IsDBNull(r("comp_address1"))) And Not String.IsNullOrEmpty(r.Item("comp_address1").ToString.Trim) Then
                htmlOut.Append(vbCrLf + r.Item("comp_address1").ToString.Trim)
              End If

              If Not (IsDBNull(r("comp_address2"))) And Not String.IsNullOrEmpty(r.Item("comp_address2").ToString.Trim) Then
                htmlOut.Append(" " + r.Item("comp_address2").ToString.Trim)
              End If

              If Not (IsDBNull(r("comp_city"))) And Not String.IsNullOrEmpty(r.Item("comp_city").ToString.Trim) Then
                htmlOut.Append(vbCrLf + r.Item("comp_city").ToString.Trim + ", ")
              End If

              If Not (IsDBNull(r("comp_state"))) And Not String.IsNullOrEmpty(r.Item("comp_state").ToString.Trim) Then
                htmlOut.Append(r.Item("comp_state").ToString.Trim + " ")
              End If

              If Not (IsDBNull(r("comp_zip_code"))) And Not String.IsNullOrEmpty(r.Item("comp_zip_code").ToString.Trim) Then
                htmlOut.Append(r.Item("comp_zip_code").ToString.Trim + " ")
              End If

              If Not (IsDBNull(r("comp_country"))) And Not String.IsNullOrEmpty(r.Item("comp_country").ToString.Trim) Then
                htmlOut.Append(vbCrLf + r.Item("comp_country").ToString.Trim)
              End If

              TextBox6.Text = htmlOut.ToString

              ' fill "contact" dropdown list 
              Dim helperClass As New displayCompanyDetailsFunctions
              Dim user_table As New DataTable
              ListBox3.Items.Clear()

              user_table = masterPage.aclsData_Temp.GetContactsAdmin(nCompanyID, "JETNET", 0)

              If Not IsNothing(user_table) Then

                If user_table.Rows.Count > 0 Then

                  ListBox3.Items.Add(New ListItem("", "0"))

                  For Each u As DataRow In user_table.Rows

                    If Not IsDBNull(u.Item("contact_last_name")) Then
                      If Not String.IsNullOrEmpty(u.Item("contact_last_name").ToString.Trim) Then

                        ListBox3.Items.Add(New ListItem(u.Item("contact_first_name").ToString.Trim + " " + u.Item("contact_last_name").ToString.Trim, u.Item("contact_id").ToString.ToLower))

                      End If
                    End If

                  Next
                End If
              End If


              ListBox3.Width = 215

            Next

          End If ' _dataTable.Rows.Count > 0 Then

        End If

      End If


    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message

    Finally

    End Try

  End Sub

    Public Sub display_edit_tab()

        Try

            If Not IsNothing(actionEdit.resultsTable) Then

                If actionEdit.resultsTable.Rows.Count > 0 Then

                    For Each r As DataRow In actionEdit.resultsTable.Rows

                        If Not IsDBNull(r.Item("journ_user_id")) Then
                            If Not String.IsNullOrEmpty(r.Item("journ_user_id").ToString.Trim) Then

                                ListBox1.SelectedValue = r.Item("journ_user_id").ToString.Trim

                            End If
                        End If

                        ' added MSW - 5/28/20 - if its RALT, otherwise default to action item 
                        If Trim(r.Item("journ_subcategory_code")) = "RALT" Then
                            Type_DropDown.SelectedIndex = 1
                        Else
                            Type_DropDown.SelectedIndex = 0 ' Action Item
                        End If


                entry_date.Text = FormatDateTime(r.Item("journ_date").ToString.Trim, DateFormat.ShortDate) 'This was changed 1/29/20 because of a message request for this form to display
                        'the journ_date in the entry date textbox, but to continue saving the way it was.

                        If Not IsDBNull(r.Item("journ_entry_time")) Then
                            If Not String.IsNullOrEmpty(r.Item("journ_entry_time").ToString.Trim) Then

                                entry_time.SelectedValue = CDate(r.Item("journ_entry_time").ToString).Hour.ToString + ":00:00 " + IIf(CDate(r.Item("journ_entry_time").ToString).Hour >= 12, "PM", "AM")

                            End If
                        End If

                        If Not IsDBNull(r.Item("journ_comp_id")) Then
                            If Not String.IsNullOrEmpty(r.Item("journ_comp_id").ToString.Trim) Then

                                If IsNumeric(r.Item("journ_comp_id").ToString) Then
                                    If CLng(r.Item("journ_comp_id").ToString) > 0 Then

                                        Dim htmlOut As New StringBuilder

                                        htmlOut.Append(r.Item("comp_name").ToString.Trim)

                                        Dim Seperator As String = ""
                                        If Not (IsDBNull(r("comp_address1"))) And Not String.IsNullOrEmpty(r.Item("comp_address1").ToString.Trim) Then
                                            htmlOut.Append(vbCrLf + r.Item("comp_address1").ToString.Trim)
                                        End If

                                        If Not (IsDBNull(r("comp_address2"))) And Not String.IsNullOrEmpty(r.Item("comp_address2").ToString.Trim) Then
                                            htmlOut.Append(" " + r.Item("comp_address2").ToString.Trim)
                                        End If

                                        If Not (IsDBNull(r("comp_city"))) And Not String.IsNullOrEmpty(r.Item("comp_city").ToString.Trim) Then
                                            htmlOut.Append(vbCrLf + r.Item("comp_city").ToString.Trim + ", ")
                                        End If

                                        If Not (IsDBNull(r("comp_state"))) And Not String.IsNullOrEmpty(r.Item("comp_state").ToString.Trim) Then
                                            htmlOut.Append(r.Item("comp_state").ToString.Trim + " ")
                                        End If

                                        If Not (IsDBNull(r("comp_zip_code"))) And Not String.IsNullOrEmpty(r.Item("comp_zip_code").ToString.Trim) Then
                                            htmlOut.Append(r.Item("comp_zip_code").ToString.Trim + " ")
                                        End If

                                        If Not (IsDBNull(r("comp_country"))) And Not String.IsNullOrEmpty(r.Item("comp_country").ToString.Trim) Then
                                            htmlOut.Append(vbCrLf + r.Item("comp_country").ToString.Trim)
                                        End If

                                        TextBox6.Text = htmlOut.ToString

                                        ' fill "contact" dropdown list 
                                        Dim helperClass As New displayCompanyDetailsFunctions
                                        Dim user_table As New DataTable
                                        ListBox3.Items.Clear()

                                        user_table = masterPage.aclsData_Temp.GetContactsAdmin(CLng(r.Item("journ_comp_id").ToString), "JETNET", 0)

                                        If Not IsNothing(user_table) Then

                                            If user_table.Rows.Count > 0 Then

                                                ListBox3.Items.Add(New ListItem("", "0"))

                                                For Each u As DataRow In user_table.Rows

                                                    If Not IsDBNull(u.Item("contact_last_name")) Then
                                                        If Not String.IsNullOrEmpty(u.Item("contact_last_name").ToString.Trim) Then

                                                            ListBox3.Items.Add(New ListItem(u.Item("contact_first_name").ToString.Trim + " " + u.Item("contact_last_name").ToString.Trim, u.Item("contact_id").ToString.ToLower))

                                                        End If
                                                    End If

                                                Next
                                            End If
                                        End If

                                        If Not IsDBNull(r.Item("journ_contact_id")) Then
                                            If Not String.IsNullOrEmpty(r.Item("journ_contact_id").ToString.Trim) Then
                                                ListBox3.SelectedValue = r.Item("journ_contact_id").ToString
                                            End If
                                        End If

                                        ListBox3.Width = 215

                                    End If
                                End If

                            End If
                        End If

                        TextBox4.Text = r.Item("journ_description").ToString.Trim
                        textRemaining.InnerText = (4000 - TextBox4.Text.Length).ToString
                    Next

                End If ' _dataTable.Rows.Count > 0 Then

            End If

        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message

        Finally

        End Try

    End Sub

End Class