
' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/userScheduledJobs.aspx.vb $
'$$Author: Amanda $
'$$Date: 2/05/20 12:18p $
'$$Modtime: 2/05/20 12:19p $
'$$Revision: 23 $
'$$Workfile: userScheduledJobs.aspx.vb $
'
' ********************************************************************************

Public Class userScheduledJobs
    Inherits System.Web.UI.Page
    Public Shared masterPage As New Object


    Private Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

        Try
            'Swap the master page based on what application you're viewing the page from.
            If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
                Me.MasterPageFile = "~/EvoStyles/EmptyCustomerAdminTheme.master"
                masterPage = DirectCast(Page.Master, EmptyCustomerAdminTheme)
            ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO Then
                Me.MasterPageFile = "~/EvoStyles/EmptyEvoTheme.master"
                masterPage = DirectCast(Page.Master, EmptyEvoTheme)
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

    Public Function DisplayCheckbox(source As Object, acID As Object, runFlag As Object) As String
        Dim returnString As String = ""
        Dim checked As Boolean = False
        If Not IsDBNull(runFlag) Then
            If UCase(runFlag) = "Y" Then
                checked = True
            End If
        End If
        ' <asp:CheckBox runat="server" ID="onOff" Checked="true" visible='<%#IIf(DataBinder.Eval(Container.DataItem, "SOURCE") = "AIRCRAFT", "true", "false") %>' />
        If Not IsDBNull(source) Then
            If source = "AIRCRAFT" Then
                returnString = "<label class=""switchToggle"" title=""Turn alerts on or off for this aircraft.  Emails will automatically be sent to your email address as JETNET records changes."">"
                returnString += "<input id=""" & acID.ToString & """ type =""checkbox"" checked=""true"" data=""" & acID.ToString & """  />"
                returnString += "<span class=""sliderToggle roundToggle""></span>"
                returnString += "</label>"

                Dim jsStr As String = "jQuery(function() {"
                jsStr += " jQuery('#" & acID.ToString & "').change(function() {" & vbNewLine
                jsStr += "jQuery.ajax({"
                jsStr += "data: this.checked,"
                jsStr += "type: 'GET',"
                jsStr += "contentType: ""application/json; charset=utf-8"","
                jsStr += "dataType: ""json"","
                jsStr += "url: 'JSONresponse.aspx/toggleAircraftAlert?acID=' + this.id + '&checked=' + this.checked + ''"
                jsStr += "})"




                jsStr += " });" & vbNewLine
                jsStr += " });" & vbNewLine
                System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType, "toggleEventScr" & acID.ToString, jsStr + vbCrLf, True)
            ElseIf source = "FOLDER" Then
                returnString = "FOLDER ONOFF"
                returnString = "<label class=""switchToggle"" title=""" & IIf(checked, "Turn Off Automated Alert.", "Turn On Automated Alert.") & """>"
                returnString += "<input id=""folderOff-" & acID.ToString & """ type =""checkbox"" " & IIf(checked, "checked = ""true""", "") & " data=""" & acID.ToString & """  />"
                returnString += "<span Class=""sliderToggle roundToggle""></span>"
                returnString += "</label>"

                Dim jsStr As String = "jQuery(function() {"
                jsStr += " jQuery('#folderOff-" & acID.ToString & "').click(function() {" & vbNewLine
                jsStr += " if (confirm('" & IIf(checked, "Are you sure you want to turn off Automated Alert for this folder?", "Are you sure you want to turn on Automated Alert for this folder?") & "')) {" & vbNewLine
                jsStr += " return true;"
                jsStr += " } else { this.checked = !this.checked;return false; }" 'return not okay delete

                jsStr += " });" & vbNewLine


                jsStr += " jQuery('#folderOff-" & acID.ToString & "').change(function() {" & vbNewLine
                jsStr += "jQuery.ajax({"
                jsStr += "data: this.checked,"
                jsStr += "type: 'GET',"
                jsStr += "contentType: ""application/json; charset=utf-8"","
                jsStr += "dataType: ""json"","
                jsStr += "url: 'JSONresponse.aspx/toggleFolderAlert?folderID=' + this.id + '&checked=' + this.checked + ''"
                jsStr += "}).done(function (data) {"
                jsStr += "$('#" & refreshList.ClientID & "').click();"
                jsStr += "});"

                jsStr += " });" & vbNewLine



                jsStr += " });" & vbNewLine
                System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType, "toggleFolderSc" & acID.ToString, jsStr + vbCrLf, True)
            ElseIf source = "PROJECT" Then
                'returnString = "FOLDER ONOFF"
                'returnString = "<label class=""switchToggle"" title=""Turn Off Automated Alert."">"
                'returnString += "<input id=""projectOff-" & acID.ToString & """ type =""checkbox"" checked=""true"" data=""" & acID.ToString & """  />"
                'returnString += "<span class=""sliderToggle roundToggle""></span>"
                'returnString += "</label>"

                'Dim jsStr As String = "jQuery(function() {"
                'jsStr += " jQuery('#projectOff-" & acID.ToString & "').click(function() {" & vbNewLine
                'jsStr += " if (confirm('This is a legacy system event alert.  Turning this alert off will remove it from the system permanently. Click Yes to continue or Cancel.?')) {" & vbNewLine
                'jsStr += " return true;"
                'jsStr += " } else { this.checked = !this.checked;return false; }" 'return not okay delete

                'jsStr += " });" & vbNewLine


                'jsStr += " jQuery('#projectOff-" & acID.ToString & "').change(function() {" & vbNewLine
                'jsStr += "jQuery.ajax({"
                'jsStr += "data: this.checked,"
                'jsStr += "type: 'GET',"
                'jsStr += "contentType: ""application/json; charset=utf-8"","
                'jsStr += "dataType: ""json"","
                'jsStr += "url: 'JSONresponse.aspx/removeProjectAlert?folderID=' + this.id + '&checked=' + this.checked + ''"
                'jsStr += "})"
                'jsStr += " });" & vbNewLine
                'jsStr += " });" & vbNewLine
                'System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType, "toggleProjectFolderSc" & acID.ToString, jsStr + vbCrLf, True)
            End If
        End If
        Return returnString
    End Function
    Public Sub MyDataGrid_Delete(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)
        Try

            Dim id As TextBox = e.Item.FindControl("id_delete")

            If HttpContext.Current.Session.Item("crmUserLogon") = True Then

                Dim SubscriptionID As Long = HttpContext.Current.Session.Item("localUser").crmSubSubID
                Dim SubscriptionLogin As String = HttpContext.Current.Session.Item("localUser").crmUserLogin
                Dim SeqNo As Long = HttpContext.Current.Session.Item("localUser").crmSubSeqNo
                Dim aclsData_Temp As New clsData_Manager_SQL
                Dim folderID As String = id.Text



                folderID = folderID
                SubscriptionID = SubscriptionID
                SubscriptionLogin = SubscriptionLogin
                SeqNo = SeqNo

                clsGeneral.clsGeneral.RemoveProject(folderID, SubscriptionID, SubscriptionLogin, SeqNo)
                BindData()
                attentionWarning.Text = "<p>Your legacy system event alert has been removed.</p>"
                attentionWarning.Visible = True
            End If
        Catch ex As Exception
            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "userScheduledJobsa.spx.vb", ex.Message)
        End Try
    End Sub


    ''' <summary>
    ''' Data function to get user scheduled jobs. This returns a view.
    ''' </summary>
    ''' <param name="subID"></param>
    ''' <param name="userLogin"></param>
    ''' <param name="seqNO"></param>
    ''' 
    ''' <returns></returns>
    Public Function GetUserScheduledJobs(ByVal subID As Long, ByVal userLogin As String, ByVal seqNO As Long) As DataTable
        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try
            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
            SqlConn.Open()

            sQuery.Append("select * from View_Customer_Jobs with (NOLOCK) ")
            sQuery.Append("where sub_id = @subID and sublogin_login = @userLogin ")
            If show_unscheduled.Checked = False Then
                sQuery.Append("and cfolder_jetnet_run_flag='Y' ")
            End If

            sQuery.Append(" order by NEXTRUN asc")

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

            Dim SqlCommand As New SqlClient.SqlCommand(sQuery.ToString, SqlConn)


            SqlCommand.Parameters.AddWithValue("@subID", subID)
            SqlCommand.Parameters.AddWithValue("@userLogin", userLogin)

            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
            End Try

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

        Return atemptable

    End Function
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Setting up page to display correct text/title.
        masterPage.setPageText("Scheduled Event Alerts/Jobs")
        masterPage.setPageTitle("Scheduled Event Alerts/Jobs")


        Dim ContactTable As New DataTable
        BindData()
        'Fill Company label. same way as company details page.
        crmWebClient.CompanyFunctions.Fill_Information_Tab(Nothing, information_label, masterPage, Session.Item("localUser").crmUserCompanyID, 0, "", New Label, New AjaxControlToolkit.TabContainer, company_address, company_name, False, False, "JETNET", 0, 0)
        'Grab contact table
        ContactTable = masterPage.aclsData_Temp.ReturnContactInformationACDetails(0, Session.Item("localUser").crmUserContactID, False)
        'Fill Contact label as on the contact details page.
        ContactFunctions.Display_Contact_Details(ContactTable, contact_information_label, Session.Item("localUser").crmUserCompanyID, 0, Master, False, True, False, contactNameText.Text, False, "JETNET", True, 0, 0)

    End Sub
    Private Sub BindData()
        Dim JobsTable As New DataTable '964, "KWagman", 1) '
        JobsTable = GetUserScheduledJobs(HttpContext.Current.Session.Item("localUser").crmSubSubID, HttpContext.Current.Session.Item("localUser").crmUserLogin, HttpContext.Current.Session.Item("localUser").crmSubSeqNo)


        If Not IsNothing(JobsTable) Then
            If JobsTable.Rows.Count > 0 Then
                'Scheduled Jobs datagrid.
                jobsTableGrid.DataSource = JobsTable
                jobsTableGrid.DataBind()
                jobsTableGrid.Visible = True
                attentionSchedule.Visible = False 'Label for no records.
            Else
                jobsTableGrid.DataSource = New DataTable
                jobsTableGrid.DataBind()
                jobsTableGrid.Visible = False
                attentionSchedule.Visible = True 'Toggle off warning for no jobs.
            End If
        End If
    End Sub
    Public Function DisplayNameLink(name As Object, id As Object, source As Object, data As Object) As String
        Dim returnString As String = ""
        If Not IsDBNull(source) Then
            If Not IsDBNull(name) Then
                If source = "FOLDER" Then
                    Dim FolderDataString As Array = Nothing
                    If Not IsDBNull(data) And Not IsDBNull(id) Then
                        FolderDataString = Split(data, "THEREALSEARCHQUERY")

                        returnString = "<a href=""javascript:ParseForm('" & id.ToString & "', false,true,false,false,false, '" + IIf(Not IsNothing(FolderDataString), Replace(FolderDataString(0), "'", "\'"), "") + "');"" class=""text_underline"">" & name.ToString & "</a>"
                    Else
                        returnString = name.ToString
                    End If

                ElseIf source = "AIRCRAFT" Then
                    If Not IsDBNull(id) Then
                        returnString = DisplayFunctions.WriteDetailsLink(id, 0, 0, 0, True, name.ToString, "text_underline", "")
                    End If
                ElseIf source = "TEXTMESSAGE" Then
                    returnString = "<a href=""javascript:load('/Preferences.aspx?activetab=5','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');"">" & name.ToString & "</a>"
                Else
                    returnString = name.ToString
                End If
            End If
        End If
        Return returnString
    End Function
    ''' <summary>
    ''' Function to figure out the time display for the scheduled job. Does it run hours/minutes/days.
    ''' </summary>
    ''' <param name="days"></param>
    ''' <param name="hours"></param>
    ''' <param name="minutes"></param>
    ''' <returns></returns>
    Public Function FigureOutTimeDisplay(days As Object, hours As Object, minutes As Object, eventID As Object, source As Object) As String
        Dim returnString As String = ""
        Dim TotalMinutes As Long = 0
        Dim TotalDays As Integer = 0
        Dim TotalHours As Integer = 0
        Dim EventTiming As Long = 0

        If IsDBNull(hours) Then
            hours = 0
        End If
        If IsDBNull(minutes) Then
            minutes = 0
        End If
        If IsDBNull(days) Then
            days = 0
        End If

        'If Session.Item("searchCriteria").SearchCriteriaEventMonths <> 0 Then
        '    returnString += Session.Item("searchCriteria").SearchCriteriaEventMonths.ToString & " Month"
        '    If Session.Item("searchCriteria").SearchCriteriaEventMonths > 1 Then
        '        returnString += "s"
        '    End If
        '    EventTiming += 1
        '    TotalMinutes += Session.Item("searchCriteria").SearchCriteriaEventMonths * 43829
        'End If


        If days <> 0 Then
            If EventTiming > 0 Then
                returnString += ", "
            End If
            returnString += days.ToString & " Day"
            If days > 1 Then
                returnString += "s"
            End If
            EventTiming += 1
        End If


        If hours <> 0 Then
            If EventTiming > 0 Then
                returnString += ", "
            End If
            returnString += hours.ToString & " Hour"
            If hours > 1 Then
                returnString += "s"
            End If
            EventTiming += 1
        End If

        If minutes <> 0 Then
            If EventTiming >= 2 And EventTiming > 0 Then
                returnString += " and "
            ElseIf EventTiming > 0 Then
                returnString += " , "
            End If
            returnString += minutes.ToString & " Minute"

            If minutes > 1 Then
                returnString += "s"
            End If
            EventTiming += 1
        End If

        If Not IsDBNull(eventID) Then
            If source = "FOLDER" Then
                returnString = "<div class=""hiddenPopupContainer""><a id=""hiddenLink-" & eventID.ToString & """ class=""cursor"" title=""Modify Run Frequency."">" & returnString & "</a>"

                returnString += "<div id=""hiddenDiv-" & eventID.ToString & """ style=""display:none;"" class=""hiddenPopupDiv"">"
                returnString += "<div class=""Box""><div class=""subHeader"">RUN THIS EVENT EVERY:</div><br clear=""all"" />"
                returnString += "<select id=""month-" & eventID.ToString & """>" & BuildOption(0, 12, 0) & "</select> Month(s), <select id=""day-" & eventID.ToString & """>" & BuildOption(0, 30, days) & "</select> Day(s), <select id=""hour-" & eventID.ToString & """>" & BuildOption(0, 23, hours) & "</select> Hour(s), <select id=""minutes-" & eventID.ToString & """>" & BuildMinutes(minutes) & "</select> and Minute(s)"
                returnString += "<input type='submit' value='Save Schedule' id=""scheduleChange-" & eventID.ToString & """ />"
                returnString += "</div>"
                returnString += "</div></div>"

                Dim jsStr As String = ""
                jsStr += " $(""#hiddenLink-" & eventID.ToString & """).click(function(){"
                jsStr += " $(""#hiddenDiv-" & eventID.ToString & """).fadeToggle(""fast"");"
                jsStr += " });"
                jsStr += " $(""#scheduleChange-" & eventID.ToString & """).click(function(){"
                jsStr += " event.preventDefault();"
                jsStr += " var mVal = $(""#hiddenDiv-" & eventID.ToString & " #month-" & eventID.ToString & """).val();"
                jsStr += " var dVal = $(""#hiddenDiv-" & eventID.ToString & " #day-" & eventID.ToString & """).val();"
                jsStr += " var hVal = $(""#hiddenDiv-" & eventID.ToString & " #hour-" & eventID.ToString & """).val();"
                jsStr += " var miVal = $(""#hiddenDiv-" & eventID.ToString & " #minutes-" & eventID.ToString & """).val();"
                jsStr += " if (mVal == '0' && dVal == '0' && hVal == '0' && miVal == '0') {"
                jsStr += " alert('Please select a time range greater than 0.');"
                jsStr += " } else {"
                jsStr += " var urlEnd = 'mVal=' + mVal + '&dVal=' + dVal + '&hVal=' + hVal + '&miVal=' + miVal +'';"
                jsStr += "jQuery.ajax({"
                jsStr += "data: this.checked,"
                jsStr += "type: 'GET',"
                jsStr += "contentType: ""application/json; charset=utf-8"","
                jsStr += "dataType: ""json"","
                jsStr += "url: 'JSONresponse.aspx/SetSchedule?folderID=' + this.id + '&' + urlEnd + ''"
                jsStr += "}).done(function (data) {"
                jsStr += "$(""#hiddenDiv-" & eventID.ToString & """).fadeToggle(""fast"");$('#" & refreshList.ClientID & "').click();"
                jsStr += "});"
                jsStr += "}"

                jsStr += " });"
                System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType, "toggleSchedule" & eventID.ToString, jsStr + vbCrLf, True)

            End If
        End If

        Return returnString
    End Function

    Private Function BuildOption(lower As Integer, upper As Integer, selectedValue As Integer) As String
        Dim returnstring As String = ""
        For i = lower To upper
            returnstring += "<option value='" & i & "' " & IIf(i = selectedValue, "selected='true'", "") & ">" & i & "</option>"
        Next
        Return returnstring
    End Function

    Private Function BuildMinutes(selectedValue As Integer) As String
        Dim returnstring As String = ""

        Select Case selectedValue
            Case 15
                returnstring += "<option value='0'>0</option>"
                returnstring += "<option value='15' selected='true'>15</option>"
                returnstring += "<option value='30'>30</option>"
                returnstring += "<option value='45'>45</option>"
            Case 30
                returnstring += "<option value='0'>0</option>"
                returnstring += "<option value='15'>15</option>"
                returnstring += "<option value='30' selected='true'>30</option>"
                returnstring += "<option value='45'>45</option>"
            Case 45
                returnstring += "<option value='0'>0</option>"
                returnstring += "<option value='15'>15</option>"
                returnstring += "<option value='30'>30</option>"
                returnstring += "<option value='45' selected='true'>45</option>"
            Case Else
                returnstring += "<option value='0' selected='true'>0</option>"
                returnstring += "<option value='15'>15</option>"
                returnstring += "<option value='30'>30</option>"
                returnstring += "<option value='45'>45</option>"
        End Select


        Return returnstring
    End Function
End Class