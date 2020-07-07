Partial Public Class DisplayContactDetail
    Inherits System.Web.UI.Page
    Dim CompanyID As Long = 0
    Dim JournalID As Long = 0
    Dim ContactID As Long = 0
    Dim DoingBusinessAs As String = ""
    Dim CRMView As Boolean = False
    Public sImageTitle As String = ""

    Public bIsAdd As Boolean = False
    Public bEnableChat As Boolean
    Public bDontShowList As Boolean
    Public strUserEmailAddress As String = ""
    Public nUserEmailAddressChatID As Integer = 0
    Dim CRMSource As String = "JETNET"
    Dim CRMJetnetContactID As Long = 0
    Dim CRMJetnetCompanyID As Long = 0
    Dim OtherCompanyID As Long = 0
    Dim OtherID As Long = 0
    Private sTask As String = ""
    Dim contact_first_name As String = ""
    Dim contact_last_name As String = ""
    Dim gbl_SubID As Long = 0

    Private Sub DisplayCompanyDetail_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Session.Item("crmUserLogon") <> True Then
            Response.Redirect("Default.aspx", False)
        Else

            If Not IsNothing(Request.Item("task")) Then
                If Not String.IsNullOrEmpty(Request.Item("task").ToString.Trim) Then
                    sTask = Request.Item("task").ToString.ToUpper.Trim
                End If
            End If

            'First thing is First, we need to determine the company that we're on.
            If Not IsNothing(Request.Item("compid")) Then
                If Not String.IsNullOrEmpty(Request.Item("compid").ToString) Then
                    CompanyID = CLng(Request.Item("compid").ToString.Trim)
                    Session("LAST_COMP") = CompanyID
                End If
            End If
            If Not IsNothing(Request.Item("jid")) Then
                If Not String.IsNullOrEmpty(Request.Item("jid").ToString) Then
                    JournalID = CLng(Request.Item("jid").ToString.Trim)
                End If
            End If
            If Not IsNothing(Request.Item("conid")) Then
                If Not String.IsNullOrEmpty(Request.Item("conid").ToString) Then
                    ContactID = CLng(Request.Item("conid").ToString.Trim)
                    Session("LAST_CONTACT") = ContactID
                End If
            End If


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

            System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StartupScript", ChartJavascript, True)


            If (Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER) Or (Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE) Then
                connect_trails.Visible = True
                If Not Page.IsPostBack Then
                    Fill_Trials_Summary_Tab()
                End If

                If IsPostBack Then
                    notes_update_panel.Update()
                End If

                'If Not IsPostBack Then
                '    trails_link_button_active.PostBackUrl = "/DisplayContactDetail.aspx?compid=" & CompanyID & "&conid=" & ContactID & "&task=inactive"
                'End If
            End If



            If clsGeneral.clsGeneral.isCrmDisplayMode() Then
                CRMView = True
                ' foldersContainer.Visible = False
                '  view_folders.Visible = False

                If Not IsNothing(Trim(HttpContext.Current.Request("source"))) Then
                    If Not String.IsNullOrEmpty(HttpContext.Current.Request("source")) Then
                        CRMSource = Trim(HttpContext.Current.Request("source"))
                        Master.aclsData_Temp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase") 'CApplication.Item("crmClientDatabase")
                    End If
                End If

                If CRMSource = "CLIENT" Then 'check for JETNET
                    Dim ClientCheck As DataTable = Master.aclsData_Temp.GetCompanyInfo_ID(CompanyID, CRMSource, 0)
                    If Not IsNothing(ClientCheck) Then 'not nothing
                        If ClientCheck.Rows.Count > 0 Then
                            OtherCompanyID = ClientCheck.Rows(0).Item("jetnet_comp_id")
                        End If
                    End If
                Else 'Check for Jetnet
                    Dim ClientCheck As DataTable = Master.aclsData_Temp.CheckforCompanyBy_JETNET_ID(CompanyID, "")
                    If Not IsNothing(ClientCheck) Then 'not nothing
                        If ClientCheck.Rows.Count > 0 Then
                            OtherCompanyID = ClientCheck.Rows(0).Item("comp_id")
                        End If
                    End If

                End If


            End If
            ac_results.Columns.Item(1).HeaderStyle.Wrap = False
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
            Build_Dynamic_Folder_Table()
        End If
    End Sub
    Private Sub SetUpTopMenuAddLinks()
        If CRMView = True Then
            Add_Note_Top.Visible = True
            AddMenuItem.Visible = True
            Add_Note_Top.InnerHtml = Replace(Replace(notes_add_new.Text, "+ ", ""), " New", "")
            Add_Action_Top.Visible = True
            Add_Action_Top.InnerHtml = Replace(Replace(action_add_new.Text, "+ ", ""), " New", "")


            If OtherCompanyID > 0 Then
                Add_Prospect_Top.Visible = True
                Add_Prospect_Top.InnerHtml = Replace(Replace(new_prospects_add.Text, "+ ", ""), " New", "")


                If CRMSource <> "CLIENT" Then
                    viewOther.Visible = True
                    viewOther.Text = "<li><a href='javascript:void();' " & DisplayFunctions.WriteDetailsLink(0, OtherCompanyID, 0, 0, False, "", "", "&source=CLIENT") & ">VIEW CLIENT COMPANY</a></li><hr class=""remove_margin"" />"
                Else
                    edit_company_link.InnerHtml = Replace(CompanyFunctions.CreateCompanyEditLink(CRMSource, CRMView, CompanyID, True, False, True), "Create Client", "CREATE CLIENT COMPANY")
                    edit_company_link.Visible = True
                    viewOther.Visible = True
                    viewOther.Text = "<li><a href='javascript:void();' " & DisplayFunctions.WriteDetailsLink(0, OtherCompanyID, 0, 0, False, "", "", "") & ">VIEW JETNET COMPANY</a></li><hr class=""remove_margin"" />"
                End If
            ElseIf CRMSource <> "CLIENT" Then
                edit_company_link.InnerHtml = Replace(CompanyFunctions.CreateCompanyEditLink(CRMSource, CRMView, CompanyID, False, True, True), "Create Client", "CREATE CLIENT COMPANY")
                edit_company_link.Visible = True
            End If
        End If



    End Sub
    Public Sub Create_Demo_Subscription() Handles save_update_demo.Click

        Try
            Dim strTemp1 As String = ""

            If (Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER) Then
                If HttpContext.Current.Session.Item("localUser").crmLocalUserEmailAddress.ToString.ToLower.Contains("@jetnet.com") Or HttpContext.Current.Session.Item("localUser").crmLocalUserEmailAddress.ToString.ToLower.Contains("@mvintech.com") Then
                    gbl_SubID = available_services.SelectedValue.ToString

                    Call Insert_Subscription_Login()


                    strTemp1 = "CompId:=[" & CStr(CompanyID) & "] "
                    strTemp1 = strTemp1 & "SubId:=[" & Trim(gbl_SubID & " ") & "]  "
                    strTemp1 = strTemp1 & "Login:=[" & Trim(txtLoginName.Text & " ") & "]  "

                    ' INSERT LOGIN EVENT
                    Call Insert_Into_EventLog("Subscription Login Added", strTemp1, 0, 0, CompanyID, False, 0, 419946)


                    Call Insert_Subscription_Install()

                    ' 03/12/2003 - By David D. Cruger
                    ' If someone adds a subscription login log an event entry
                    strTemp1 = "CompId:=[" & CStr(CompanyID) & "] "
                    strTemp1 = strTemp1 & "SubId:=[" & CStr(gbl_SubID) & "]  "
                    strTemp1 = strTemp1 & "Login:=[" & txtLoginName.Text & "]  "
                    strTemp1 = strTemp1 & "Install SeqNo:=[1]  "

                    Call Insert_Into_EventLog("Subscription Install Added", strTemp1, 0, 0, CompanyID, False, 0, 419946)

                    Call AddSubscriptionNote(CompanyID, gbl_SubID, "New Install Added For Login:=[" & txtLoginName.Text & "]  Platform:=[" & Fix_Quote(Trim(txt_Platform_Name.Text)) & "]", "")

                    create_results_label.Text = "Demo/Trial Created"
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub
    Public Sub Insert_Subscription_Login()

        Dim Query As String = ""
        Dim strTemp1 As String = ""



        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            Query = "INSERT INTO [Homebase].jetnet_ra.dbo.Subscription_Login ("
            Query = Query & "sublogin_sub_id, "
            Query = Query & "sublogin_login, "
            Query = Query & "sublogin_password, "
            Query = Query & "sublogin_contact_id, "
            Query = Query & "sublogin_entry_date, "
            Query = Query & "sublogin_active_flag, "

            ' 11/26/2007 - By David D. Cruger - Added
            Query = Query & "sublogin_nbr_of_installs, "
            Query = Query & "sublogin_contract_amount, "

            ' 05/02/2008 - By David D. Cruger; Added
            Query = Query & "sublogin_allow_export_flag, "
            Query = Query & "sublogin_allow_local_notes_flag, "
            Query = Query & "sublogin_allow_projects_flag, "
            Query = Query & "sublogin_allow_email_request_flag, "
            Query = Query & "sublogin_allow_event_request_flag, "

            ' 06/03/2009 - By David D. Cruger; Added
            Query = Query & "sublogin_allow_text_message_flag, "

            ' 01/19/2010 - By David D. Cruger; Added
            Query = Query & "sublogin_bypass_active_x_registry_flag, "

            ' 09/20/2019 - By David D. Cruger; Added
            Query = Query & "sublogin_values_flag, "
            Query = Query & "sublogin_mpm_flag, "

            Query = Query & "sublogin_demo_flag "  ' Last Field

            Query = Query & ") VALUES ("
            Query = Query & gbl_SubID & ", "
            Query = Query & "'" & Fix_Quote(Trim(txtLoginName.Text)) & "', "
            Query = Query & "'" & Fix_Quote(Trim(txt_sub_password.Text)) & "', "
            Query = Query & "419946 , "   ' DEFAULT CONTACT ID 
            Query = Query & "'" & Year(Date.Now()) & "-" & Month(Date.Now()) & "-" & Day(Date.Now()) & "', "


            Query = Query & "'Y', "   ' login active flag 

            ' 11/26/2007 - By David D. Cruger - Added
            strTemp1 = " 1"   ' number of installs
            strTemp1 = Replace(strTemp1, ",", "")
            If IsNumeric(strTemp1) = True Then
                If Val(strTemp1) >= 0 Then
                    Query = Query & strTemp1 & ", "
                Else
                    Query = Query & "0, "
                End If
            Else
                Query = Query & "0, "
            End If

            ' 11/26/2007 - By David D. Cruger - Added
            strTemp1 = "0" ' contract amount 
            strTemp1 = Replace(strTemp1, ",", "")
            If IsNumeric(strTemp1) = True Then
                If Val(strTemp1) >= 0 Then
                    Query = Query & strTemp1 & ", "
                Else
                    Query = Query & "0.00, "
                End If
            Else
                Query = Query & "0.00, "
            End If


            Query = Query & "'N', "    ' allow export

            Query = Query & "'Y', "     ' login local notes 

            Query = Query & "'Y', "       ' login projects

            Query = Query & "'N', "      ' login email request 


            Query = Query & "'N', "          'event request 


            Query = Query & "'N', "        ' text message


            Query = Query & "'N', "          ' active x 


            If Values_Checkbox.Checked = True Then
                Query = Query & "'Y', "
            Else
                Query = Query & "'N', "
            End If

            Query = Query & "'N', "        ' mpm login

            Query = Query & "'Y') "      ' login demo 


            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
            SqlConn.Open()
            SqlCommand.Connection = SqlConn

            SqlCommand.CommandText = Query

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Insert_Subscription_Login</b><br />" & Query

            SqlCommand.ExecuteNonQuery()


        Catch ex As Exception
            '   EvoSubscriptionInsertDefaultModel = False
            '   Me.class_error = "Error in EvoSubscriptionInsertDefaultModel(ByVal sub_id As Long, ByVal sim_login As String, ByVal sim_seq_no As Integer, ByVal sim_amod_id As Long) As Boolean SQL VERSION: " & ex.Message
        Finally
            SqlConn.Dispose()
            SqlConn.Close()

            SqlConn = Nothing

            SqlCommand.Dispose()
            SqlCommand = Nothing
        End Try
    End Sub
    Public Function Fix_Quote(ByVal text_to_replace As String)

        Fix_Quote = ""
        Fix_Quote = Replace(text_to_replace, "'", "''")

    End Function


    Public Sub Insert_Subscription_Install()

        Dim Query As String = ""
        Dim strTemp1 As String = ""



        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try


            Query = "INSERT INTO [Homebase].jetnet_ra.dbo.Subscription_Install ("
            Query = Query & "subins_sub_id, "
            Query = Query & "subins_login, "
            Query = Query & "subins_seq_no, "
            Query = Query & "subins_platform_name, "
            Query = Query & "subins_platform_os, "
            Query = Query & "subins_local_db_flag, "
            ' 03/17/2009 - By David D. Cruger Added Display Note Tab
            Query = Query & "subins_display_note_tag_on_aclist_flag, "
            Query = Query & "subins_local_db_file, "

            ' 12/03/2002 - By David D. Cruger; Added this field
            Query = Query & "subins_webpage_timeout, "

            ' 03/27/2003 - By David D. Cruger; Added this field
            Query = Query & "subins_activex_flag, "

            ' 11/15/2004 - By David D. Cruger; Added these two fields
            Query = Query & "subins_autocheck_tservice, "
            Query = Query & "subins_terminal_service, "

            ' 11/06/2012 - By David D. Cruger; Added
            Query = Query & "subins_background_image_id, "

            ' 11/08/2012 - By David D. Cruger; Added
            Query = Query & "subins_nbr_rec_per_page, "

            ' 08/14/2017 - By David D. Cruger; Added
            Query = Query & "subins_default_analysis_months, "

            ' 03/07/2005 - By David D. Cruger; Added Reply Name, EMail And Default Format
            Query = Query & "subins_email_replyname, "
            Query = Query & "subins_email_replyaddress, "
            Query = Query & "subins_email_default_format, "

            ' 11/26/2007 - By David D. Cruger - Added
            Query = Query & "subins_contract_amount, "

            ' 06/03/2009 - By David D. Cruger; Added
            Query = Query & "subins_cell_number, "
            Query = Query & "subins_cell_service, "
            Query = Query & "subins_cell_carrier_id, "
            Query = Query & "subins_smstxt_models, "

            ' 06/23/2009 - By David D. Cruger; Added
            Query = Query & "subins_smstxt_active_flag, "

            ' 02/16/2010 - By David D. Cruger; Added
            Query = Query & "subins_default_amod_id, "

            ' 02/19/2010 - By David D. Cruger; Added
            Query = Query & "subins_evo_mobile_flag, "

            ' 03/08/2011 - By David D. Cruger; Added
            Query = Query & "subins_sms_events, "

            ' 07/11/2011 - By David D. Cruger; Added
            Query = Query & "subins_contact_id, "

            ' 07/25/2013 - By David D. Cruger; Added
            Query = Query & "subins_admin_flag, "

            ' 08/22/2014 - By David D. Cruger; Added
            Query = Query & "subins_chat_flag, "

            ' 09/22/2015 - By David D. Cruger; Added
            Query = Query & "subins_business_type_code, "

            Query = Query & "subins_active_flag) "  ' Keep This the Last Field

            ' Start Of Values
            Query = Query & "VALUES ("
            Query = Query & gbl_SubID & ", "
            Query = Query & "'" & Fix_Quote(Trim(txtLoginName.Text)) & " ', "       '" & grd_Installations.Tag & "
            Query = Query & "1 , "      ' lSeqNo
            Query = Query & "'" & Fix_Quote(Trim(txt_Platform_Name.Text)) & " ', "      '
            Query = Query & "'', "      '" & Fix_Quote(Trim(txt_Platform_OS)) & "

            Query = Query & "'N', "    ' use local notes

            Query = Query & "'N', "      ' display note tag 

            'subins_local_db_file - subins_local_db_file
            '   Query = Query & "'" & Fix_Quote(Trim(txtInstallationPathToLocalDB)) & "', "
            Query = Query & " '', "

            ' 12/03/2002 - By David D. Cruger; Added this field - subins_webpage_timeout
            Query = Query & "'30', "

            ' 03/27/2003 - By David D. Cruger; Added this field - subins_activex_flag

            Query = Query & "'N', "      'active x 

            ' 11/15/2004 - By David D. Cruger; Added this field - subins_autocheck_tservice

            Query = Query & "'N', "      ' auto check 

            ' 11/15/2004 - By David D. Cruger; Added this field - subins_terminal_service 
            Query = Query & "'N', "      ' terminal service 

            ' 11/06/2012 - By David D. Cruger; Added 
            Query = Query & "0, "     ' sub image id 

            ' 11/08/2012 - By David D. Cruger; Added 
            Query = Query & "10, "      'records per page 

            ' 08/14/2017 - By David D. Cruger; Added 
            Query = Query & "6, "      'default analysis months 

            ' 03/07/2005 - By David D. Cruger; Added Reply Name 
            '  Query = Query & "'" & txtReplyName.Text & "', "
            Query = Query & "'', "

            ' 03/07/2005 - By David D. Cruger; Added Reply EMail  
            ' Query = Query & "'" & txtReplyEMail.Text & "', "
            Query = Query & "'', "

            ' 03/07/2005 - By David D. Cruger; Added  Default Format 
            Query = Query & "'TEXT', "      ' chkDefaultHTMLEMail.Value 

            ' 11/26/2007 - By David D. Cruger - Added 
            Query = Query & "0.00,"     'txt_SubInsContractAmount

            ' 06/03/2009 - By David D. Cruger; Added 
            Query = Query & "Null, "     'txtCellNumber


            ' 06/03/2009 - By David D. Cruger; Added 
            Query = Query & "Null, "     'cboCellCarrier ? 
            Query = Query & "0, "


            ' 06/03/2009 - By David D. Cruger; Added 
            Query = Query & "Null, "    'txtTextMsgModels
            '------------------------------------------------------
            ' 02/10/2010 - By David D. Cruger; Added
            ' SMS Text Messaging Active Flag 
            Query = Query & "'N', "      'txtSMSTextActiveFlag


            '------------------------------------------------------
            ' 02/16/2010 - By David D. Cruger
            ' Default Model Id's 
            Query = Query & "0, "      'txtSubDefaultAModId

            '------------------------------------------------------
            ' 02/19/2010 - By David D. Cruger; Added
            ' Evolution Mobile Flag 
            Query = Query & "'Y', "      'chkInstallEvoMobile

            '------------------------------------------------------
            ' 03/08/2011 - By David D. Cruger; Added
            ' SMS Text Message Events
            Query = Query & "'', "     'txtSMSEvents

            '------------------------------------------------------
            ' 07/11/2011 - By David D. Cruger; Added
            ' Contact Id


            Query = Query & " 419946 , "  ' Default  DEMO CONTACT ID 


            '------------------------------------------------------
            ' 07/25/2013 - By David D. Cruger; Added
            ' Admin Flag 
            Query = Query & "'N', "    ' chkInstallAdministrator

            '------------------------------------------------------
            ' 08/22/2014 - By David D. Cruger; Added
            ' Chat Flag 
            Query = Query & "'N', "     'chkInstallationChatFlag

            ' 09/22/2015 - By David D. Cruger; Added 
            Query = Query & "'DB', "       'cboSubBType

            '------------------------------------------------------
            ' Keep This The Last Field. No comma at the end.
            ' subins_active_flag
            '------------------------------------------------------ 
            Query = Query & "'Y')"     'chkInstallationActive

            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
            SqlConn.Open()
            SqlCommand.Connection = SqlConn

            SqlCommand.CommandText = Query

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Insert_Subscription_Install</b><br />" & Query

            SqlCommand.ExecuteNonQuery()

        Catch ex As Exception
            '   EvoSubscriptionInsertDefaultModel = False
            '   Me.class_error = "Error in EvoSubscriptionInsertDefaultModel(ByVal sub_id As Long, ByVal sim_login As String, ByVal sim_seq_no As Integer, ByVal sim_amod_id As Long) As Boolean SQL VERSION: " & ex.Message
        Finally
            SqlConn.Dispose()
            SqlConn.Close()

            SqlConn = Nothing

            SqlCommand.Dispose()
            SqlCommand = Nothing
        End Try

    End Sub

    Public Sub AddSubscriptionNote(ByVal lCompId As Long, ByVal lSubId As Long, ByVal strSubject As String, ByVal strNote As String)


        Dim strInsert1 As String = ""
        Dim strTemp1 As String = ""

        Dim strDateTime As String
        Dim strSubId As String
        Dim strDescription As String
        Dim strJournId As String
        Dim strCompId As String
        Dim strContactId As String
        Dim strUserId As String

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            strDateTime = DateTime.Now()

            strCompId = CStr(lCompId)
            strSubId = CStr(lSubId)
            strSubject = Trim(strSubject & " ")
            strNote = Trim(strNote & " ")

            strJournId = "0"
            strContactId = "419946"

            strUserId = ""
            If Not IsNothing(Session.Item("homebaseUserClass").home_user_id) Then
                strUserId = Trim(Session.Item("homebaseUserClass").home_user_id)
            End If


            strInsert1 = "INSERT INTO [Homebase].jetnet_ra.dbo.Journal ("
            strInsert1 = strInsert1 & "journ_subcategory_code, journ_subject, journ_description, "
            strInsert1 = strInsert1 & "journ_comp_id, journ_user_id, journ_entry_date, "
            strInsert1 = strInsert1 & "journ_entry_time, journ_action_date "
            strInsert1 = strInsert1 & ") VALUES ("
            strInsert1 = strInsert1 & "'SN',"
            strInsert1 = strInsert1 & "'" & strSubject & "',"
            strInsert1 = strInsert1 & "'" & strNote & "',"
            strInsert1 = strInsert1 & strCompId & ","
            strInsert1 = strInsert1 & "'" & strUserId & "',"
            strInsert1 = strInsert1 & "'" & Format(strDateTime, "mm/dd/yyyy") & "',"
            strInsert1 = strInsert1 & "'" & Format(strDateTime, "hh:mm:ss AM/PM") & "',"
            strInsert1 = strInsert1 & "GetDate()"
            strInsert1 = strInsert1 & ")"


            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
            SqlConn.Open()
            SqlCommand.Connection = SqlConn

            SqlCommand.CommandText = strInsert1

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Insert_Subscription_Install</b><br />" & strInsert1

            SqlCommand.ExecuteNonQuery()

        Catch ex As Exception
            '   EvoSubscriptionInsertDefaultModel = False
            '   Me.class_error = "Error in EvoSubscriptionInsertDefaultModel(ByVal sub_id As Long, ByVal sim_login As String, ByVal sim_seq_no As Integer, ByVal sim_amod_id As Long) As Boolean SQL VERSION: " & ex.Message
        Finally
            SqlConn.Dispose()
            SqlConn.Close()

            SqlConn = Nothing

            SqlCommand.Dispose()
            SqlCommand = Nothing
        End Try

    End Sub

    Public Sub Insert_Into_EventLog(ByVal inType As String, ByVal inText As String, ByVal inAC_ID As Long, ByVal inJourn_ID As Long, ByVal inComp_ID As Long, ByVal inUse_AutoLog As Boolean, ByVal inYacht_ID As Long, ByVal inContact_ID As Long)


        Dim Query As String = ""
        Dim strTemp1 As String = ""


        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            Query = "INSERT into [Homebase].jetnet_ra.dbo.EventLog ("
            Query = Query & "evtl_user_id, "
            Query = Query & "evtl_type, "
            Query = Query & "evtl_message, "
            Query = Query & "evtl_ac_id, "
            Query = Query & "evtl_journ_id, "
            Query = Query & "evtl_comp_id, "
            Query = Query & "evtl_yacht_id, "
            Query = Query & "evtl_contact_id"
            Query = Query & ") VALUES ("
            If Not IsNothing(Session.Item("homebaseUserClass").home_user_id) Then
                Query = Query & "'" & Trim(Session.Item("homebaseUserClass").home_user_id) & "', "
            Else
                Query = Query & "'', "
            End If


            Query = Query & "'" & inType & "', "
            Query = Query & "'" & Fix_Quote(inText) & "', "
            Query = Query & inAC_ID & ","
            Query = Query & inJourn_ID & ","
            Query = Query & inComp_ID & ","
            Query = Query & inYacht_ID & ","
            Query = Query & inContact_ID & ""
            Query = Query & ")"

            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
            SqlConn.Open()
            SqlCommand.Connection = SqlConn

            SqlCommand.CommandText = Query

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Insert_Subscription_Install</b><br />" & Query

            SqlCommand.ExecuteNonQuery()

        Catch ex As Exception
            '   EvoSubscriptionInsertDefaultModel = False
            '   Me.class_error = "Error in EvoSubscriptionInsertDefaultModel(ByVal sub_id As Long, ByVal sim_login As String, ByVal sim_seq_no As Integer, ByVal sim_amod_id As Long) As Boolean SQL VERSION: " & ex.Message
        Finally
            SqlConn.Dispose()
            SqlConn.Close()

            SqlConn = Nothing

            SqlCommand.Dispose()
            SqlCommand = Nothing
        End Try

    End Sub
    'Public Sub Generate_Random_Password(ByVal sender As Object, ByVal e As System.EventArgs)

    '    txt_sub_password.Text = "ABC123"

    'End Sub
    Public Sub Generate_Random_Password()

        Dim temp_random As New Random
        Dim abcs As String = "ZABCEYFGHIJLMNOPQSTUVXY"
        Dim abcs2 As String = "AMZNXJVHBGFYJVGFYTUWEFJOWEFWEHGOPQOWIEURHTYGJFLAKSJFGMHNBVHCBZVXVCHSDHFWHFWEHFWEFWEUFIWEGFWQWPOQWIERWUHFSNKSDGRIDLPAOQISUTHBZXYMQWERPTOYIALSKMZJKCNFJGHVNBBNTURIDLPAOQISUTHBZXYMQWERPTOYIALSKMZJKCNFJGHVNBBN"
        Dim symbols2 As String = "!@#$%*?#!#$"
        Dim random_number As String = ""
        Dim random_number2 As String = ""
        Dim random_string As String = ""
        Dim temp_string As String = ""
        Dim random_symbol As String = ""

        Try


            random_number = Left(Trim(temp_random.Next), 1)    ' gets random NUMBER  

            random_string = UCase(abcs.Substring(random_number, 1))  ' start at spot random, only do 1 capital letter  

            random_number = Left(Trim(temp_random.Next), 1)

            random_symbol = UCase(symbols2.Substring(random_number, 1))  ' get a random symbol

            random_string &= random_symbol  ' addsymbol

            ' get two random numbers, then go get a random pattern 
            random_number = Left(Trim(temp_random.Next), 1)

            random_number2 = Left(Trim(temp_random.Next), 1)

            random_number = random_number & random_number2 ' combine the two, could be up To 100 spots 

            temp_string = LCase(abcs2.Substring(random_number, 6))   ' get those 5 characters, starting at random spot 

            random_string &= temp_string  ' addsymbol 

            txt_sub_password.Text = random_string

        Catch ex As Exception

        End Try
    End Sub
    Public Sub CONNECT_TRIALS_REDIRECT()

        Dim temp_sub_id As Long = 0
        Dim temp_sub_login As String = ""
        Dim temp_seq_no As Long = 0

        '     html.Append("<td align=""left""><a href='DisplayContactDetail.aspx?compid=" & comp_id & "&contact_id=" & contact_id & "&trial_connect=Y&sub_id=" & r("sub_id") & "&sub_login=" & r("sublogin_login") & "&sub_seq=" & r("subins_seq_no") & "'>Connect Trial</a></td>")

        If Not IsNothing(Request.Item("compid")) Then
            If Not String.IsNullOrEmpty(Request.Item("compid").ToString) Then
                CompanyID = CLng(Request.Item("compid").ToString.Trim)
            End If
        End If

        If Not IsNothing(Request.Item("contact_id")) Then
            If Not String.IsNullOrEmpty(Request.Item("contact_id").ToString) Then
                ContactID = CLng(Request.Item("contact_id").ToString.Trim)
            End If
        End If

        If Not IsNothing(Request.Item("sub_id")) Then
            If Not String.IsNullOrEmpty(Request.Item("sub_id").ToString) Then
                temp_sub_id = CLng(Request.Item("sub_id").ToString.Trim)
            End If
        End If


        If Not IsNothing(Request.Item("sub_login")) Then
            If Not String.IsNullOrEmpty(Request.Item("sub_login").ToString) Then
                temp_sub_login = Request.Item("sub_login").ToString.Trim
            End If
        End If


        If Not IsNothing(Request.Item("sub_seq")) Then
            If Not String.IsNullOrEmpty(Request.Item("sub_seq").ToString) Then
                temp_seq_no = CLng(Request.Item("sub_seq").ToString.Trim)
            End If
        End If

        ' update STATEMENT 
        Call DisplayFunctions.Update_Subscription_Comp_Contact_ID(temp_sub_id, temp_sub_login, temp_seq_no, ContactID)

        Response.Redirect("DisplayContactDetail.aspx?compid=" & CompanyID & "&conid=" & ContactID & "&sub_id=" & temp_sub_id & "&sub_login=" & temp_sub_login & "&sub_seq=" & temp_seq_no & "")

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'What should this page do?
        'First we should verify that the user is logged in.
        'Even though we check on the masterpage, I don't think it hurts to add a check here:
        'Plus I'm stopping the page from running without a compID
        'No point in running if a companyID isn't there.
        Dim contact_id_list As String = ""

        ' Me.contact_information_tab.HeaderText = "Contact Information"
        'Me.company_information_tab.HeaderText = "Company Information"

        If Trim(Request("trial_connect")) = "Y" Then
            Call CONNECT_TRIALS_REDIRECT()
        End If


        If ContactID <> 0 Then
            If Session.Item("crmUserLogon") <> True Then
                Response.Redirect("Default.aspx", False)
            Else


                If Not Page.IsPostBack Then
                    Dim ContactTable As New DataTable
                    Dim PageTitle As String = "Contact Details"
                    If JournalID > 0 Then
                        history_background.CssClass = "history_bg"
                    End If
                    'Set Cookies
                    clsGeneral.clsGeneral.Recent_Cookies("contacts", ContactID, IIf(CRMSource = "CLIENT", "CLIENT", "JETNET"))


                    If CRMSource = "CLIENT" Then
                        ContactTable = Master.aclsData_Temp.GetContacts_Details(ContactID, CRMSource, False)
                        If Not IsNothing(ContactTable) Then
                            If ContactTable.Rows.Count > 0 Then
                                OtherID = ContactTable.Rows(0).Item("contact_jetnet_contact_id")
                            End If
                        End If
                    Else
                        ContactTable = Master.aclsData_Temp.ReturnContactInformationACDetails(JournalID, ContactID, IIf((Session.Item("jetnetAppVersion") <> Constants.ApplicationVariable.CUSTOMER_CENTER) And (Session.Item("jetnetAppVersion") <> Constants.ApplicationVariable.HOMEBASE), True, False))
                        Dim temporaryTable As DataTable = Master.aclsData_Temp.GetContactInfo_JETNET_ID(ContactID, "Y")
                        If Not IsNothing(temporaryTable) Then
                            If temporaryTable.Rows.Count > 0 Then
                                OtherID = temporaryTable.Rows(0).Item("contact_id")
                            End If
                        End If
                        temporaryTable = New DataTable
                    End If

                    'Fills Company Information Tab
                    crmWebClient.CompanyFunctions.Fill_Information_Tab(Nothing, company_information_label, Master, CompanyID, JournalID, DoingBusinessAs, New Label, New AjaxControlToolkit.TabContainer, company_address, New Label, False, CRMView, CRMSource, CRMJetnetCompanyID, OtherCompanyID, True)


                    If Session.Item("jetnetAppVersion") <> Constants.ApplicationVariable.CRM Then
                        Call commonLogFunctions.Log_User_Event_Data("UserStatistics", "DisplayContactDetails: Company" + CompanyID.ToString + ", Contact: " + ContactID.ToString & " " & IIf(CRMSource = "CLIENT", "Viewing Client Record.", ""), Nothing, 0, JournalID, 0, CompanyID, ContactID)
                    End If

                    If Not IsNothing(ContactTable) Then
                        If ContactTable.Rows.Count > 0 Then

                            If CRMView Then
                                If CRMSource = "CLIENT" Then
                                    CRMJetnetContactID = IIf(Not IsDBNull(ContactTable.Rows(0).Item("contact_jetnet_contact_id")), ContactTable.Rows(0).Item("contact_jetnet_contact_id"), 0)
                                End If
                            End If
                            PageTitle = IIf(Not IsDBNull(ContactTable.Rows(0).Item("contact_sirname")), IIf(Not String.IsNullOrEmpty(ContactTable.Rows(0).Item("contact_sirname").ToString.Trim), ContactTable.Rows(0).Item("contact_sirname").ToString.Trim + "&nbsp;", ""), "")

                            contact_first_name = IIf(Not IsDBNull(ContactTable.Rows(0).Item("contact_first_name")), ContactTable.Rows(0).Item("contact_first_name").ToString.Trim + "&nbsp;", "")
                            PageTitle += contact_first_name

                            PageTitle += IIf(Not IsDBNull(ContactTable.Rows(0).Item("contact_middle_initial")), IIf(Not String.IsNullOrEmpty(ContactTable.Rows(0).Item("contact_middle_initial").ToString.Trim), ContactTable.Rows(0).Item("contact_middle_initial").ToString.Trim + ".&nbsp;", ""), "")

                            contact_last_name += IIf(Not IsDBNull(ContactTable.Rows(0).Item("contact_last_name")), ContactTable.Rows(0).Item("contact_last_name").ToString.Trim, "")
                            PageTitle += contact_last_name

                            If (Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER) Then
                                If HttpContext.Current.Session.Item("localUser").crmLocalUserEmailAddress.ToString.ToLower.Contains("@jetnet.com") Or HttpContext.Current.Session.Item("localUser").crmLocalUserEmailAddress.ToString.ToLower.Contains("@mvintech.com") Then
                                    If Trim(contact_first_name) <> "" And Trim(contact_last_name) <> "" Then
                                        txtLoginName.Text = Left(Trim(contact_first_name), 1) & Right(Trim(contact_last_name), Len(Trim(contact_last_name)) - 1)
                                    End If
                                End If
                            End If



                            If String.IsNullOrEmpty(Trim(Replace(PageTitle, "&nbsp;", ""))) Then
                                PageTitle = IIf(Not IsDBNull(ContactTable.Rows(0).Item("contact_title")), ContactTable.Rows(0).Item("contact_title").ToString.Trim, "")
                            End If

                            sImageTitle = PageTitle.Replace("&nbsp;", Constants.cSingleSpace).Trim

                            ' PageTitle += ", " & Replace(Replace(Replace(company_information_tab.HeaderText, ",", ""), "</b>", ""), "<b>", "")

                            Dim imgDisplayFolder As String = HttpContext.Current.Application.Item("crmClientSiteData").ClientFullHostName + HttpContext.Current.Session.Item("ContactPicturesFolderVirtualPath")

                            Dim TheFile As System.IO.FileInfo
                            Dim contactImageLink As String = ""
                            Dim contactImageFile As String = ""

                            If Not IsDBNull(ContactTable.Rows(0).Item("conpic_contact_id")) Then

                                contactImageLink = Session.Item("ContactPicturesFolderVirtualPath") + "/" + Session.Item("localUser").crmUserContactID.ToString + ".jpg"
                                contactImageFile = HttpContext.Current.Server.MapPath(contactImageLink)

                                TheFile = New System.IO.FileInfo(contactImageFile)
                                If TheFile.Exists Then 'is the file actually there?
                                    contact_picture.Text = "<img src=""" + imgDisplayFolder.Trim + "/" + ContactTable.Rows(0).Item("conpic_contact_id").ToString + "." + ContactTable.Rows(0).Item("conpic_image_type").ToString + """ alt=""" + sImageTitle.Trim + """  title=""" + sImageTitle.Trim + """ border=""1"" width=""100%"" />"
                                Else
                                    contact_picture.Text = "<img src=""" + imgDisplayFolder.Trim + "/" + ContactTable.Rows(0).Item("conpic_contact_id").ToString + "-" + ContactTable.Rows(0).Item("conpic_id").ToString + "." + ContactTable.Rows(0).Item("conpic_image_type").ToString + """ alt=""" + sImageTitle.Trim + """  title=""" + sImageTitle.Trim + """ width=""100%"" border=""1"" />"
                                End If

                            End If

                            If (CBool(My.Settings.enableChat)) Then

                                ChatManager.CheckAndInitChat(False, bEnableChat) ' checks to see if my chat is enabled

                                If bEnableChat Then

                                    If Not (IsDBNull(ContactTable.Rows(0).Item("contact_email_address"))) Then
                                        If Not String.IsNullOrEmpty(ContactTable.Rows(0).Item("contact_email_address").ToString) Then

                                            strUserEmailAddress = ContactTable.Rows(0).Item("contact_email_address").ToString.Trim

                                            ' check and see if this user has "chat" enabled "before" checking on line status
                                            Dim bUserEnabledChat = ChatManager.userEnabledChat(CompanyID, CLng(ContactTable.Rows(0).Item("contact_id").ToString), strUserEmailAddress, nUserEmailAddressChatID)

                                            If bUserEnabledChat And nUserEmailAddressChatID > 0 Then ' chat is enabled show online/offline status

                                                If ChatManager.isUserOnLine(strUserEmailAddress.ToLower.Trim, nUserEmailAddressChatID) Then

                                                    If CLng(HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString) <> CLng(ContactTable.Rows(0).Item("contact_id").ToString) Then

                                                        contact_chat_img.Visible = True
                                                        contact_chat_img.AlternateText = "Click to 'start' a chat with " + sImageTitle.Trim
                                                        contact_chat_img.ToolTip = contact_chat_img.AlternateText

                                                        contact_chat_label.Visible = True
                                                        contact_chat_label.Text = sImageTitle.Trim + " is <b>ONLINE</b>. "

                                                        If Not ChatManager.IsUserOnCommunityList(HttpContext.Current, strUserEmailAddress, nUserEmailAddressChatID) Then
                                                            bIsAdd = True
                                                        End If

                                                    Else

                                                        bDontShowList = True

                                                        contact_chat_img_self.Visible = True
                                                        contact_chat_img_self.AlternateText = "You are online"
                                                        contact_chat_img_self.ToolTip = contact_chat_img.AlternateText

                                                        contact_chat_label.Visible = True
                                                        contact_chat_label.Text = "You are <b>ONLINE</b>."

                                                    End If

                                                Else ' user is not on line but show "add/remove" user from comunity list

                                                    contact_chat_img_offline.Visible = True
                                                    contact_chat_img_offline.AlternateText = sImageTitle.Trim + " is OFFLINE"
                                                    contact_chat_img_offline.ToolTip = contact_chat_img.AlternateText

                                                    contact_chat_label.Visible = True
                                                    contact_chat_label.Text = sImageTitle.Trim + " is <b>OFFLINE</b>"

                                                    If Not ChatManager.IsUserOnCommunityList(HttpContext.Current, strUserEmailAddress, nUserEmailAddressChatID) Then
                                                        bIsAdd = True
                                                    End If

                                                End If ' is user on line

                                            Else ' this user doesn't have chat enabled don't show anything

                                                bDontShowList = True

                                            End If

                                        End If
                                    End If

                                End If ' bEnableChat (my chat is enabled)

                            End If ' if on local,jetnettest, yacht-spottest

                        End If ' ContactTable.Rows.Count > 0

                    End If ' Not IsNothing(ContactTable) Then

                    Master.SetPageTitle(PageTitle)
                    PageTitle = ""

                    ContactFunctions.Display_Contact_Details(ContactTable, contact_information_label, CompanyID, JournalID, Master, False, True, False, contactNameText.Text, CRMView, CRMSource, True, OtherID, OtherCompanyID)
                    If Not IsNothing(ContactTable) Then
                        ContactTable.Dispose()
                    End If

                    If CRMView = True And CRMSource = "CLIENT" Then
                        contact_id_list = contact_id_list & "'" & CRMJetnetContactID & "'"
                    Else
                        contact_id_list = contact_id_list & "'" & ContactID & "'"
                    End If


                    DisplayContactOtherListings(contact_id_list)

                    Aircraft_Listings(contact_id_list)
                    If (Session.Item("jetnetAppVersion") <> Constants.ApplicationVariable.CUSTOMER_CENTER) And (Session.Item("jetnetAppVersion") <> Constants.ApplicationVariable.HOMEBASE) Then
                        If clsGeneral.clsGeneral.isCrmDisplayMode Then
                            If ((Not Session.Item("localUser").crmDemoUserFlag) And (Session.Item("localUser").crmEnableNotes) _
                            And Session.Item("localSubscription").crmServerSideNotes_Flag And JournalID = 0) And (Not String.IsNullOrEmpty(Session.Item("jetnetServerNotesDatabase"))) Then
                                Dim NotesLinkText As String = "javascript:load('edit_note.aspx?type=note&action=new&from=contactDetails&contact_ID=" & ContactID & "&comp_ID=" & CompanyID & "&source=" & IIf(CRMSource = "CLIENT", "CLIENT", "JETNET") & "','','scrollbars=yes,menubar=no,height=400,width=1260,resizable=yes,toolbar=no,location=no,status=no');"
                                Dim ActionsLinkText As String = "javascript:load('edit_note.aspx?from=contactDetails&contact_ID=" & ContactID & "&comp_ID=" & CompanyID & "&source=" & IIf(CRMSource = "CLIENT", "CLIENT", "JETNET") & "&type=action&action=new','','scrollbars=yes,menubar=no,height=400,width=1260,resizable=yes,toolbar=no,location=no,status=no');"

                                Dim ProspectsLinkText As String = "javascript:load('edit_note.aspx?action=new&type=prospect&cat_key=0&from=contactDetails&contact_ID=" & ContactID & "&comp_ID=" & CompanyID & "&source=" & IIf(CRMSource = "CLIENT", "CLIENT", "JETNET") & "','unloaded_me','scrollbars=yes,menubar=no,height=435,width=1260,resizable=yes,toolbar=no,location=no,status=no');"

                                notes_add_new.Text = "<p align='right'>+ <a href=""javascript:void(0);"" onclick=""" & NotesLinkText & """>Add New Note</a></p>"
                                action_add_new.Text = "<p align='right'>+ <a href=""javascript:void(0);"" onclick=""" & ActionsLinkText & """>Add New Action</a></p>"


                                new_prospects_add.Text = "<p align='right'>+ <a href=""javascript:void(0);"" onclick=""" & ProspectsLinkText & """>Add New Prospect</a></p>"
                                prospectsContainer.CssClass = "grid-item"
                                notesPanel.Visible = True
                                actionPanel.Visible = True
                                view_notes.Visible = True
                                closeNotes.Visible = True
                                view_notes_link.Visible = True
                                Session.Item("Listing") = 1
                                Session.Item("ListingSource") = "JETNET"
                                Session.Item("ListingID") = CompanyID
                                Session.Item("ContactID") = ContactID
                                If Trim(Request("source")) = "CLIENT" Then
                                    DisplayFunctions.DisplayLocalItems(Master.aclsData_Temp, 0, 0, 0, Nothing, Nothing, False, False, False, False, 0, False, True, "CLIENT", prospects_label, False, True, ContactID)
                                Else
                                    DisplayFunctions.DisplayLocalItems(Master.aclsData_Temp, 0, 0, 0, Nothing, Nothing, False, False, False, False, 0, False, True, "JETNET", prospects_label, False, True, ContactID)
                                End If

                                If Trim(Request("source")) = "CLIENT" Then
                                    DisplayFunctions.DisplayLocalItems(Master.aclsData_Temp, 0, 0, 0, notes_label, action_label, False, False, False, False, 5, False, clsGeneral.clsGeneral.isCrmDisplayMode, "CLIENT", Nothing, True, True, ContactID)
                                Else
                                    DisplayFunctions.DisplayLocalItems(Master.aclsData_Temp, 0, 0, 0, notes_label, action_label, False, False, False, False, 5, False, clsGeneral.clsGeneral.isCrmDisplayMode, "JETNET", Nothing, True, True, ContactID)
                                End If


                                SetUpTopMenuAddLinks()

                                'notes_add_new.Text = "<a href=" & DisplayFunctions.WriteNotesRemindersLinks(0, 0, CompanyID, 0, False, "&n=1", "Add New Note") & " class='special'>NOTES +</a>"

                                'action_add_new.Text = "<a href=" & DisplayFunctions.WriteNotesRemindersLinks(0, 0, CompanyID, 0, False, "", "Add New Action") & " class='special'>ACTIONS +</a>"

                                'If Trim(Request("source")) = "CLIENT" Then '
                                '  notes_add_new.Text = "<p align='right'>+ " & DisplayFunctions.WriteNotesRemindersLinks(0, 0, CompanyID, 0, True, "&n=1&source=CLIENT", "Add New Note") & "</p>"
                                '  action_add_new.Text = "<p align='right'>+ " & DisplayFunctions.WriteNotesRemindersLinks(0, 0, CompanyID, 0, True, "&source=CLIENT", "Add New Action") & "</p>"
                                'Else
                                '  notes_add_new.Text = "<p align='right'>+ " & DisplayFunctions.WriteNotesRemindersLinks(0, 0, CompanyID, 0, True, "&n=1", "Add New Note") & "</p>"
                                '  action_add_new.Text = "<p align='right'>+ " & DisplayFunctions.WriteNotesRemindersLinks(0, 0, CompanyID, 0, True, "", "Add New Action") & "</p>"
                                'End If
                                notes_update_panel.Update()
                            End If
                        End If
                    End If


                    If Session.Item("localSubscription").crmYacht_Flag Then
                        Fill_Yacht_Tab(contact_id_list)
                        If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT Then
                            Fill_Related_Transactions(contact_id_list)

                        End If
                        If Session.Item("localSubscription").crmBusiness_Flag = True Then
                            fill_related_AC_Trans(contact_id_list)
                        End If


                    End If

                End If
                If (Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER) Or (Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE) Then
                    FillAdminActionItems()
                    If Not Page.IsPostBack Then
                        Fill_Customer_Activities_FromView()
                    End If
                    DisplayUserAccounts(CompanyID, ContactID)
                    prospectsContainer.CssClass = "grid-item"
                    prospectsContainer.Visible = True
                    notesPanel.Visible = False
                    view_notes.Visible = False
                    Dim ProspectsLinkText As String = "javascript:load('edit_note.aspx?action=new&type=prospect&cat_key=0&from=companydetails&contact_ID=" & ContactID & "&comp_ID=" & CompanyID & "&source=" & IIf(CRMSource = "CLIENT", "CLIENT", "JETNET") & "','unloaded_me','scrollbars=yes,menubar=no,height=435,width=860,resizable=yes,toolbar=no,location=no,status=no');"
                    new_prospects_add.Text = "<p align='right'>+ <a href=""javascript:void(0);"" onclick=""" & ProspectsLinkText & """>Add New Prospect</a></p>"

                    Dim AclsData_Temp As New clsData_Manager_SQL
                    AclsData_Temp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase")
                    AclsData_Temp.JETNET_DB = Session.Item("jetnetClientDatabase")

                    Dim aTempTable As New DataTable
                    aTempTable = AclsData_Temp.Notes_Search_For_Prospect_View_Homebase("", "", "", "B','O", 0, "", "", "", "", "", "", 0, 0, "", False, False, "'Active'", "", 0, CompanyID, ContactID)


                    prospects_label.Text = DisplayFunctions.CRMDisplay_Notes_Or_Actions_MPM(aTempTable, AclsData_Temp, False, False, False, False, False, False, True, False, CRMView, CRMSource, True, False, False)

                    'ADDED MSW - TO CREATE BUTTONS FOR EDIT 
                    If (Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER) Then
                        If HttpContext.Current.Session.Item("localUser").crmLocalUserEmailAddress.ToString.ToLower.Contains("@jetnet.com") Or HttpContext.Current.Session.Item("localUser").crmLocalUserEmailAddress.ToString.ToLower.Contains("@mvintech.com") Then
                            demo_trial_block.Visible = True
                        End If
                    End If

                    SetUpTopMenuAddLinks()


                End If


            End If
        End If

    End Sub
    Public Sub Load_Available_Services()

        Dim aTempTable As New DataTable
        Dim helperClass As New displayCompanyDetailsFunctions

        Try

            aTempTable = helperClass.Get_Available_Services("") 'Time table fill up
            If Not IsNothing(aTempTable) Then
                If aTempTable.Rows.Count > 0 Then
                    For Each q As DataRow In aTempTable.Rows
                        available_services.Items.Add(New ListItem(q("sub_service_name") & " (" & q("sub_id") & ")", q("sub_id")))
                    Next
                End If
            End If

        Catch ex As Exception

        End Try

    End Sub





    Public Sub Fill_Customer_Activities_FromView()
        Dim user_table As New DataTable
        Dim htmlOut As New StringBuilder

        Dim temp_desc As String = ""
        Dim toggleAllActivities As Boolean = False
        customer_activities_panel.Visible = True
        Dim helperClass As New displayCompanyDetailsFunctions

        If sTask.ToLower.Contains("showall") Then
            toggleAllActivities = True
            showTop50Activities.Visible = True
            showAllActivities.Visible = False
        Else
            toggleAllActivities = False
            showTop50Activities.Visible = False
            showAllActivities.Visible = True
        End If


        user_table = helperClass.Return_Customer_Actions_Summary(CompanyID, JournalID, "N", toggleAllActivities, customerActivitiesFilter.SelectedValue, ContactID)



        If Not IsNothing(user_table) Then
            customerActivities_Label.Text = helperClass.DisplayCustomerActivitiesTable(user_table, CompanyID, ContactID)
        End If
    End Sub


    Public Function DisplayUserAccounts(companyID As Long, contactID As Long) As String
        Dim htmlOut As New StringBuilder
        Dim user_table As New DataTable
        user_table = Get_ContactUserAccounts(companyID, contactID)
        If Not IsNothing(user_table) Then

            htmlOut.Append("<table id='customerUserAccountsTable' width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" class=""formatTable blue small aircraftTable"">")
            htmlOut.Append("<tr class='header_row'>")
            htmlOut.Append("<td align='left'><b>SERVICES</b></td>")
            htmlOut.Append("<td align='left'><b>PW</b></td>")
            htmlOut.Append("<td align='left'><b>LAST ACCESS</b></td>")
            htmlOut.Append("<td align='left'><b>VALUES</b></td>")
            htmlOut.Append("<td align='left'><b>ADMIN?</b></td>")
            htmlOut.Append("<td align='left'><b>SUBID</b></td>")
            htmlOut.Append("</tr>")

            If (user_table.Rows.Count > 0) Then

                For Each q As DataRow In user_table.Rows

                    htmlOut.Append("<tr bgcolor=""white"">")

                    htmlOut.Append("<td align=""left"" valign=""top"">")

                    If Not IsDBNull(q("sub_service_name")) Then
                        If Not String.IsNullOrEmpty(q("sub_service_name").ToString.Trim) Then
                            htmlOut.Append(q("sub_service_name").ToString)
                        End If
                    End If

                    htmlOut.Append("</td>")
                    htmlOut.Append("<td align=""left"" valign=""top"">")

                    If Not IsDBNull(q("sublogin_password")) Then
                        If Not String.IsNullOrEmpty(q("sublogin_password").ToString.Trim) Then
                            htmlOut.Append(q("sublogin_password").ToString)
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("<td align=""left"" valign=""top"">")

                    If Not IsDBNull(q("subins_last_login_date")) Then
                        If Not String.IsNullOrEmpty(q("subins_last_login_date").ToString.Trim) Then

                            htmlOut.Append(Format(CDate(q("subins_last_login_date")), "MM/dd/yyyy HHtt"))
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("<td align=""left"" valign=""top"">")

                    If Not IsDBNull(q("sublogin_values_flag")) Then
                        If Not String.IsNullOrEmpty(q("sublogin_values_flag").ToString.Trim) Then
                            If q("sublogin_values_flag") = "Y" Then
                                htmlOut.Append("YES")
                            Else
                                htmlOut.Append("N")
                            End If
                        End If
                    End If

                    htmlOut.Append("</td>")


                    htmlOut.Append("<td align=""left"" valign=""top"">")

                    If Not IsDBNull(q("subins_admin_flag")) Then
                        If Not String.IsNullOrEmpty(q("subins_admin_flag").ToString.Trim) Then
                            If q("subins_admin_flag") = "Y" Then
                                htmlOut.Append("YES")
                            Else
                                htmlOut.Append("N")
                            End If
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("<td align=""left"" valign=""top"">")

                    If Not IsDBNull(q("sub_id")) Then
                        If Not String.IsNullOrEmpty(q("sub_id").ToString.Trim) Then
                            htmlOut.Append(q("sub_id").ToString)
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("</tr>")

                Next

            End If

            htmlOut.Append("</table>")
            user_account_label.Text = htmlOut.ToString

            user_account_panel.Visible = True
        Else

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "displayContactDetailsFunctions.vb", "DisplayCustomerActivitiesTable() Datatable is nothing on admin Activities items")

        End If
        Return htmlOut.ToString
    End Function
    Public Function Get_ContactUserAccounts(compID As Long, contactID As Long) As DataTable
        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try
            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
            SqlConn.Open()


            sQuery.Append("select sub_service_name,  sublogin_password, subins_last_login_date, sublogin_values_flag,subins_admin_flag, sub_id ")
            sQuery.Append(" from View_JETNET_Customers with (NOLOCK) ")
            sQuery.Append(" where comp_id = @compID and contact_id = @contactID")
            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "DisplayContactDetail.aspx.vb", sQuery.ToString)

            Dim SqlCommand As New SqlClient.SqlCommand(sQuery.ToString, SqlConn)


            SqlCommand.Parameters.AddWithValue("@compID", compID)
            SqlCommand.Parameters.AddWithValue("@contactID", contactID)
            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                clsGeneral.clsGeneral.Build_Debug_Text("Error in: " & System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "DisplayContactDetail.aspx.vb", constrExc.Message)
            End Try

            SqlCommand.Dispose()
            SqlCommand = Nothing

        Catch ex As Exception
            Return Nothing
            clsGeneral.clsGeneral.Build_Debug_Text("Error in: " & System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "DisplayContactDetail.aspx.vb", ex.Message)


        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing

        End Try

        Return atemptable

    End Function


    Private Sub FillAdminActionItems()
        Dim actionDataTable As New DataTable
        Dim helperClass As New displayCompanyDetailsFunctions
        actionDataTable = helperClass.Get_ActionItems_Query(CompanyID, ContactID)

        action_add_new.Visible = True
        closeNotes.Visible = False
        actionPanel.Visible = True
        action_label.Visible = True 'visible no matter
        If Not IsNothing(actionDataTable) Then
            action_add_new.Visible = True
            action_add_new.Text = "<a href='#' class=""float_right"" onclick=""javascript:load('/adminActions.aspx?task=add&journid=&companyid=" & CompanyID & "&contactid=" & ContactID.ToString & "','','scrollbars=yes,menubar=no,height=900,width=1350,resizable=yes,toolbar=no,location=no,status=no');return false;"">ADD ACTION ITEM</a>"

            Me.action_label.Text = helperClass.ReturnActionItemsDisplayTable(actionDataTable, CompanyID, ContactID)
        Else

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "displayContactDetailsFunctions.vb", "FillAdminActionItems() Datatable is nothing on admin action items")
        End If
    End Sub
    ''' <summary>
    ''' button click for view notes. 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Sub ViewCompanyNotes(ByVal sender As Object, ByVal e As System.EventArgs) ' Handles view_notes.Click
        If notesPanel.Visible = False Then
            ToggleButtons(False, False)
        Else
            ToggleButtons(False, True)
        End If
    End Sub
    Private Function FixAircraftTableRemoveDuplicates(ByVal AircraftTable As DataTable) As DataTable
        Dim oldACID As Long = 0
        Dim ACCounter As Integer = 0
        Dim dalTable As New DataTable
        Dim dataSet As DataSet = New DataSet("dataSet")


        dalTable = AircraftTable.Clone
        dataSet.EnforceConstraints = False
        dalTable.PrimaryKey = Nothing
        dalTable.Constraints.Clear()

        Dim afiltered_BOTH As DataRow() = AircraftTable.Select()

        For Each atmpDataRow_JETNET In afiltered_BOTH
            Dim newRow As DataRow = AircraftTable.NewRow()

            If oldACID = atmpDataRow_JETNET("ac_id") Then

            Else
                dalTable.ImportRow(atmpDataRow_JETNET)
            End If

            oldACID = atmpDataRow_JETNET("ac_id")
        Next



        'For Each r As DataRow In AircraftTable.Rows
        '  Dim newRow As DataRow = NewAircraftTable.NewRow()

        '  If oldACID = r("ac_id") Then 
        '    newRow("amod_model_name") = ""
        '    newRow("amod_make_name") = ""
        '    newRow("ac_id") = ""
        '    newRow("ac_ser_no_full") = ""
        '    newRow("ac_reg_no") = ""
        '    newRow("actype_name") = ""
        '    newRow("ac_asking_price") = ""
        '    newRow("ac_forsale_flag") = ""
        '    newRow("ac_asking_wordage") = ""
        '    newRow("ac_status") = ""
        '    newRow("ac_date_listed") = ""
        '    newRow("ac_delivery") = ""
        '  Else
        '    ACCounter += 1
        '    newRow("amod_model_name") = r("amod_model_name").ToString
        '    newRow("amod_make_name") = r("amod_make_name").ToString
        '    newRow("ac_id") = r("ac_id")
        '    newRow("ac_ser_no_full") = r("ac_ser_no_full").ToString
        '    newRow("ac_reg_no") = r("ac_reg_no").ToString
        '    newRow("actype_name") = r("actype_name") 
        '    newRow("ac_asking_price") = r("ac_asking_price").ToString   
        '    newRow("ac_forsale_flag") = r("ac_forsale_flag").ToString 
        '    newRow("ac_asking_wordage") = r("ac_asking_wordage").ToString 
        '    newRow("ac_status") = r("ac_status").ToString 
        '    newRow("ac_date_listed") = r("ac_date_listed").ToString  
        '    newRow("ac_delivery") = r("ac_delivery").ToString 
        '  End If



        '  newRow("comp_id") = r("comp_id").ToString
        '  newRow("comp_name") = r("comp_name").ToString
        '  newRow("comp_city") = r("comp_city").ToString
        '  newRow("comp_state") = r("comp_state").ToString


        '  NewAircraftTable.Rows.Add(newRow)
        '  NewAircraftTable.AcceptChanges()
        '  oldACID = r("ac_id")

        '  Next
        Return dalTable
    End Function

    Private Function FixRefTableForClient(ByVal AircraftTable As DataTable) As DataTable
        Dim dalTable As New DataTable
        Dim dataSet As DataSet = New DataSet("dataSet")
        Dim NewAircraftTable As DataTable = AircraftTable.Clone
        Dim SQLQuery As String = ""

        SQLQuery = "((contact_id = " & ContactID & " and source = 'CLIENT')"

        If CRMJetnetContactID > 0 Then
            SQLQuery += " or (clicontact_jetnet_contact_id = " & CRMJetnetContactID & " and source = 'JETNET')"
        End If
        SQLQuery += " )"

        dalTable = AircraftTable.Clone
        dataSet.EnforceConstraints = False
        dalTable.PrimaryKey = Nothing
        dalTable.Constraints.Clear()

        NewAircraftTable.Columns.Add("ac_reg_no")
        NewAircraftTable.Columns.Add("actype_name")

        Dim afiltered_BOTH As DataRow() = AircraftTable.Select(SQLQuery)


        For Each r As DataRow In afiltered_BOTH
            Dim newRow As DataRow = NewAircraftTable.NewRow()

            newRow("amod_make_name") = r("amod_make_name").ToString
            newRow("amod_make_type") = r("amod_make_Type").ToString
            newRow("ac_id") = r("ac_id")
            newRow("ac_amod_id") = r("ac_amod_id")
            newRow("ac_ser_nbr") = r("ac_ser_nbr").ToString
            newRow("ac_reg_nbr") = r("ac_reg_nbr").ToString
            newRow("ac_reg_no") = r("ac_reg_nbr").ToString
            newRow("ac_airframe_total_hours") = r("ac_airframe_total_hours")
            newRow("ac_year") = r("ac_year").ToString
            newRow("amod_model_name") = r("amod_model_name").ToString
            newRow("ac_status") = r("ac_status").ToString
            newRow("ac_year_mfr") = r("ac_year_mfr").ToString
            newRow("ac_forsale_flag") = r("ac_forsale_flag").ToString
            newRow("ac_exclusive_flag") = r("ac_exclusive_flag").ToString
            newRow("ac_asking_wordage") = r("ac_asking_wordage").ToString
            newRow("ac_asking_price") = r("ac_asking_price")
            newRow("ac_asking_wordage") = r("ac_asking_wordage").ToString
            newRow("ac_ser_no_full") = r("ac_ser_no_full").ToString
            newRow("ac_date_listed") = r("ac_date_listed")
            newRow("ac_product_business_flag") = r("ac_product_business_flag").ToString
            newRow("ac_product_helicopter_flag") = r("ac_product_helicopter_flag").ToString
            newRow("ac_product_commercial_flag") = r("ac_product_commercial_flag").ToString
            newRow("ac_delivery") = ""

            newRow("comp_name") = r("comp_name").ToString
            newRow("comp_country") = r("comp_country").ToString
            newRow("comp_state") = r("comp_state").ToString
            newRow("comp_city") = r("comp_city").ToString
            newRow("contact_title") = r("contact_title").ToString
            newRow("contact_sirname") = r("contact_sirname").ToString
            newRow("contact_first_name") = r("contact_first_name").ToString
            newRow("contact_middle_initial") = r("contact_middle_initial").ToString
            newRow("contact_last_name") = r("contact_last_name").ToString
            newRow("contact_suffix") = r("contact_suffix").ToString
            newRow("act_name") = r("act_name").ToString
            newRow("actype_name") = r("act_name").ToString
            newRow("cref_transmit_seq_no") = r("cref_transmit_seq_no")

            newRow("comp_id") = r("comp_id")
            newRow("contact_id") = r("contact_id")
            newRow("source") = r("source").ToString

            newRow("acref_owner_percentage") = r("acref_owner_percentage")


            NewAircraftTable.Rows.Add(newRow)
            NewAircraftTable.AcceptChanges()


        Next
        Return NewAircraftTable
    End Function
    Private Sub Aircraft_Listings(ByRef contact_id_list As String)
        Dim RefTable As New DataTable
        Dim AircraftTable As New DataTable
        Dim ACTable As New DataTable
        Dim bgcolor As String = "#FFFFFF"

        If CRMView = True And CRMSource = "CLIENT" Then
            '      RefTable = Master.aclsData_Temp.Client_Get_Related_Aircraft_Info(contact_id_list)
            'Client Contact will not have relevent other contact entries linked to other companies so only using contact ID.
            RefTable = Master.aclsData_Temp.Get_Client_JETNET_AC(CompanyID, "ac_id ASC", Session.Item("localSubscription").crmHelicopter_Flag, Session.Item("localSubscription").crmBusiness_Flag, Session.Item("localSubscription").crmCommercial_Flag, Session.Item("localSubscription").crmJets_Flag, Session.Item("localSubscription").crmExecutive_Flag, Session.Item("localSubscription").crmTurboprops)
            RefTable = FixRefTableForClient(RefTable)

        Else
            RefTable = Master.aclsData_Temp.Get_Related_Aircraft_Info(contact_id_list)
        End If



        If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT And Session.Item("localSubscription").crmBusiness_Flag = False Then
            If Not IsNothing(RefTable) Then
                If RefTable.Rows.Count > 0 Then
                    AircraftTable = FixAircraftTableRemoveDuplicates(RefTable)
                    ' Me.ac_information.Visible = True
                    'Me.ac_information_tab.Visible = True
                    aircraftPanel.Visible = True
                    AircraftTextHeader.Text = "RELATED YACHT LISTINGS"
                    Me.aircraftDataGrid_YachtSpot.Visible = True
                    Me.ac_results.Visible = False
                    aircraftDataGrid_YachtSpot.DataSource = AircraftTable
                    aircraftDataGrid_YachtSpot.DataBind()
                Else
                    'Me.ac_information.Visible = False
                    'Me.ac_information_tab.Visible = False
                    aircraftPanel.Visible = False
                    Me.ac_results.Visible = False
                End If
            Else
                aircraftPanel.Visible = False
                'Me.ac_information.Visible = False
                'Me.ac_information_tab.Visible = False
                Me.ac_results.Visible = False
            End If
        Else
            If Not IsNothing(RefTable) Then
                If RefTable.Rows.Count > 0 Then

                    If (Session.Item("localSubscription").crmAerodexFlag) Then
                        ac_results.Columns(2).Visible = False
                    End If

                    If CRMView = True Then
                        If CRMSource = "CLIENT" Then
                            ac_results.Columns(4).Visible = True
                            ac_results.Columns(3).Visible = False
                        End If
                    End If

                    'Me.ac_information.Visible = True
                    'Me.ac_information_tab.Visible = True
                    aircraftPanel.Visible = True
                    Me.ac_results.Visible = True
                    ac_results.DataSource = RefTable
                    ac_results.DataBind()
                Else
                    aircraftPanel.Visible = False
                    'Me.ac_information.Visible = False
                    'Me.ac_information_tab.Visible = False
                    Me.ac_results.Visible = False
                End If
            Else
                aircraftPanel.Visible = False
                'Me.ac_information.Visible = False
                'Me.ac_information_tab.Visible = False
                Me.ac_results.Visible = False
            End If
        End If




        RefTable.Dispose()
        RefTable = New DataTable
    End Sub
    Private Sub Fill_Yacht_Tab(ByRef contact_id_list As String)
        Dim YachtTable As New DataTable
        Dim journ_date_as_of As String = ""

        YachtTable = Master.aclsData_Temp.DisplayYachtForGivenCompanyByCompanyID(CompanyID, contact_id_list)

        If Not IsNothing(YachtTable) Then
            If YachtTable.Rows.Count > 0 Then
                yachtContainer.Visible = True
                YachtDataGrid.DataSource = YachtTable
                YachtDataGrid.DataBind()

                yachtHeader.InnerText = "Yachts <em class='tiny_text'>(" & YachtTable.Rows.Count & " relationship" & IIf(YachtTable.Rows.Count = 1, "", "s") & ")</em>"

            Else
                yacht_label.Text += "<p align='center'>No Yachts Found.</p>"
                yacht_label.ForeColor = Drawing.Color.Red
                yacht_label.Font.Bold = True
                yachtContainer.Visible = False
            End If
        Else
            If Master.aclsData_Temp.class_error <> "" Then
                Master.LogError("CompanyTabs.ascx.vb -Fill_Yacht_Tab() - " & Master.aclsData_Temp.class_error)
            End If
        End If
        YachtTable.Dispose()


        If JournalID > 0 Then
            historyContainer.Visible = True
            Me.history_information_label.Visible = True
            Me.history_information_label.Text = Master.aclsData_Temp.Get_Yacht_History(0, CompanyID, 0, JournalID, CRMView, journ_date_as_of)
            historyHeaderText.InnerText = "HISTORY INFORMATION AS OF: " & journ_date_as_of
            'Me.history_information_panel.Visible = True
            Me.history_information_label.Visible = False


            ' Me.contact_information_tab.HeaderText &= "Contact Information as of: " & journ_date_as_of
            '  Me.company_information_tab.HeaderText &= "Company Information as of: " & journ_date_as_of
        End If


    End Sub

    Private Sub fill_related_AC_Trans(ByVal contact_id_list As String)

        Dim acTable As New DataTable

        acTable = Master.aclsData_Temp.DisplayRelatedACTransactionsByContactID(contact_id_list)
        If Not IsNothing(acTable) Then
            If acTable.Rows.Count > 0 Then

                acTransContainer.Visible = True
                ac_trans_grid.DataSource = acTable
                ac_trans_grid.DataBind()

                ' ac_trans_panel.HeaderText = "Aircraft Transactions"

            Else
                ac_trans_label.Text += "<p align='center'>No Aircraft Transactions Found.</p>"
                ac_trans_label.ForeColor = Drawing.Color.Red
                ac_trans_label.Font.Bold = True
                acTransContainer.Visible = False
            End If
        Else
            If Master.aclsData_Temp.class_error <> "" Then
                Master.LogError("CompanyTabs.ascx.vb -fill_related_AC_Trans() - " & Master.aclsData_Temp.class_error)
            End If
        End If
        acTable.Dispose()
    End Sub


    Private Sub Fill_Related_Transactions(ByVal contact_id_list As String)
        Dim YachtTable As New DataTable


        YachtTable = Master.aclsData_Temp.DisplayRelatedYachtTransactionsByContactID(contact_id_list, 0, JournalID)
        If Not IsNothing(YachtTable) Then
            If YachtTable.Rows.Count > 0 Then

                yachtTransContainer.Visible = True
                yacht_trans_grid.DataSource = YachtTable
                yacht_trans_grid.DataBind()

                yachtTransText.InnerText = "Yacht Transactions"

            Else
                yacht_trans_label.Text += "<p align='center'>No Yacht Transactions Found.</p>"
                yacht_trans_label.ForeColor = Drawing.Color.Red
                yacht_trans_label.Font.Bold = True
                yachtTransContainer.Visible = False
            End If
        Else
            If Master.aclsData_Temp.class_error <> "" Then
                Master.LogError("CompanyTabs.ascx.vb -Fill_Yacht_Tab() - " & Master.aclsData_Temp.class_error)
            End If

        End If
        YachtTable.Dispose()
    End Sub
    Private Sub DisplayContactOtherListings(ByRef contact_id_list As String)
        Dim RefTable As New DataTable
        Dim CompanyTable As New DataTable
        Dim master_id As Long = 0
        Dim last_comp_id_string As String = ""
        Dim RunThroughRelated As Boolean = True
        contact_information_other_listing_label.Text = "<div class=""Box""><div class=""subHeader"">Other Listings:</div><br clear=""all"" /><span class='tiny_text'><em>This individual is also referenced on other companies including those listed below.</em></span> <br />"

        If CRMView = True And CRMSource = "CLIENT" Then
            If CRMJetnetContactID > 0 Then
                RefTable = Master.aclsData_Temp.Get_ContactReferenceRelatedCompanyTitle(CRMJetnetContactID, Session.Item("jetnetAppVersion"))
            End If
        Else
            RefTable = Master.aclsData_Temp.Get_ContactReferenceRelatedCompanyTitle(ContactID, Session.Item("jetnetAppVersion"))
        End If


        If Not IsNothing(RefTable) Then
            For Each r As DataRow In RefTable.Rows
                RunThroughRelated = True 'Reset this variable to true. If it's not supposed to run, the select statements will catch it down below.
                '' if the related company is the current company, then the other one is master 
                'If (r("cr_comp_rel_id") <> CompanyID And r("cr_comp_rel_id") <> master_id) Then
                '  CompanyTable = Master.aclsData_Temp.GetContactTitleAndCompanyInformation(r("cr_contact_rel_id"), JournalID)
                'ElseIf (r("cr_comp_id") <> CompanyID And r("cr_comp_id") <> master_id) Then
                '  CompanyTable = Master.aclsData_Temp.GetContactTitleAndCompanyInformation(r("cr_contact_id"), JournalID)
                'End If

                'If master_id = 0 Then
                '  If r("cr_comp_rel_id") = CompanyID Then
                '    master_id = r("cr_comp_id")
                '  ElseIf r("cr_comp_id") = CompanyID Then
                '    master_id = r("cr_comp_rel_id")
                '  End If
                'End If

                ' change - if the comp_id is not the current comp_id then do select

                Select Case CRMView
                    Case True
                        Select Case CRMSource
                            Case "CLIENT"
                                If r("comp_id") = CRMJetnetCompanyID Then
                                    RunThroughRelated = False
                                End If
                            Case Else
                                If r("comp_id") = CompanyID Then
                                    RunThroughRelated = False
                                End If
                        End Select
                    Case Else
                        If r("comp_id") = CompanyID Then
                            RunThroughRelated = False
                        End If
                End Select

                If RunThroughRelated = True Then
                    CompanyTable = Master.aclsData_Temp.GetContactTitleAndCompanyInformation(r("contact_id"), JournalID)

                    'if we havent displayed this company already
                    If (InStr(Trim(last_comp_id_string), Trim(" " & CompanyTable.Rows(0).Item("comp_id") & " ,")) = 0) Then
                        If Not IsNothing(CompanyTable) Then
                            If CompanyTable.Rows.Count > 0 Then
                                contact_information_other_listing_label.Visible = True
                                contact_information_other_listing_label.Text += "<span class='li'>" & DisplayFunctions.WriteDetailsLink(0, CompanyTable.Rows(0).Item("comp_id"), 0, 0, True, CompanyTable.Rows(0).Item("comp_name").ToString, "", "") & " " & CompanyTable.Rows(0).Item("comp_city") & " " & CompanyTable.Rows(0).Item("comp_state") & ", " & CompanyTable.Rows(0).Item("comp_country") & ", <span class='label'>" & CompanyTable.Rows(0).Item("contact_title").ToString & " </span></span>"
                                If Trim(contact_id_list) <> "" Then
                                    contact_id_list += ","
                                End If

                                If Not IsDBNull(CompanyTable.Rows(0).Item("contact_id")) Then
                                    contact_id_list += "'" & CompanyTable.Rows(0).Item("contact_id") & "'"
                                End If
                            End If
                        End If
                    End If


                    last_comp_id_string &= " " & CompanyTable.Rows(0).Item("comp_id") & " ,"


                    CompanyTable.Dispose()
                    CompanyTable = New DataTable

                End If

            Next

            RefTable.Dispose()

        End If

        contact_information_other_listing_label.Text += "</div>"
        RefTable = New DataTable
    End Sub

    Private Sub export_company_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles export_company.Click, export_contact.Click
        Dim CompanyInformation As New DataTable
        Dim CompanyPhoneInformation As New DataTable
        Dim ContactPhoneInformation As New DataTable
        Dim ContactTable As New DataTable

        If sender.id.ToString = "export_contact" Then
            ContactTable = Master.aclsData_Temp.ReturnContactInformationACDetails(JournalID, ContactID)
            CompanyInformation = Master.aclsData_Temp.GetCompanyInfo_ID(CompanyID, "JETNET", 0)
            ContactPhoneInformation = Master.aclsData_Temp.GetPhoneNumbers(CompanyID, ContactID, "JETNET", 0)
        Else
            CompanyInformation = Master.aclsData_Temp.GetCompanyInfo_ID(CompanyID, "JETNET", 0)
            CompanyPhoneInformation = Master.aclsData_Temp.GetPhoneNumbers(CompanyID, 0, "JETNET", 0)
        End If

        If clsGeneral.clsGeneral.Create_VCard(CompanyInformation, CompanyPhoneInformation, ContactPhoneInformation, ContactTable) = 1 Then
            Dim vCardPath As String = HttpContext.Current.Session.Item("MarketSummaryFolderVirtualPath").ToString + "\contact.vcf"
            Response.Redirect(vCardPath, False)
        Else
            System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Error", "javascript:alert('Error: No Information to Export.');", True)
        End If


        CompanyInformation.Dispose()
        CompanyPhoneInformation.Dispose()
        ContactPhoneInformation.Dispose()
        ContactTable.Dispose()
    End Sub


    Public Function display_comp_city(ByVal city, ByVal state) As String
        display_comp_city = ""
        If Not IsDBNull(city) And Not IsDBNull(state) Then
            If Trim(city) <> "" And Trim(state) <> "" Then
                display_comp_city = "(" & city & ", " & state & ")"
            ElseIf Trim(city) <> "" Then
                display_comp_city = "(" & city & ")"
            ElseIf Trim(state) <> "" Then
                display_comp_city = "(" & state & ")"
            End If
        ElseIf Not IsDBNull(city) Then
            If Trim(city) <> "" Then
                display_comp_city = "(" & city & ")"
            End If
        ElseIf Not IsDBNull(state) Then
            If Trim(state) <> "" Then
                display_comp_city = "(" & state & ")"
            End If
        End If



    End Function

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


        ContainerTable = DisplayFunctions.CreateStaticFoldersTable(0, 0, 0, 0, ContactID, Master.aclsData_Temp, 0)
        TDHold = DisplayFunctions.BuildTableCell(False, "", VerticalAlign.Top, HorizontalAlign.Left)
        ContainerTable.CssClass = "formatTable blue small"
        SubmitButton.Text = "Save Folders"
        SubmitButton.ID = "SaveStaticFoldersButton"
        AddHandler SubmitButton.Click, AddressOf SaveStaticFolders

        TDHold.Controls.Add(SubmitButton)
        TR.Controls.Add(TDHold)

        ContainerTable.Controls.Add(TR)

        folders_label.Controls.Clear()
        folders_label.Controls.Add(ContainerTable)


        folders_update_panel.Update()
    End Sub
    Public Sub Create_Demo_Trials(ByVal sender As Object, ByVal e As System.EventArgs) ' Handles view_folders.Click


        ' ADDED MSW - 3/12/20
        If (Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER) Then
            If HttpContext.Current.Session.Item("localUser").crmLocalUserEmailAddress.ToString.ToLower.Contains("@jetnet.com") Or HttpContext.Current.Session.Item("localUser").crmLocalUserEmailAddress.ToString.ToLower.Contains("@mvintech.com") Then
                create_subscription_panel.Visible = True

                If Trim(contact_first_name) <> "" And Trim(contact_last_name) <> "" Then
                    txtLoginName.Text = Left(Trim(contact_first_name), 1) & Right(Trim(contact_last_name), Len(Trim(contact_last_name)) - 1)
                End If

                Call Load_Available_Services()

                Call Generate_Random_Password()

                If available_services.Items.Count = 0 Then
                    create_subscription_panel.Visible = False
                End If
            End If
        End If

    End Sub

    Public Sub View_Connect_Trials(ByVal sender As Object, ByVal e As System.EventArgs) ' Handles view_folders.Click

        Me.edit_trial_panel.Visible = True

        Dim helperClass As New displayCompanyDetailsFunctions
        Dim trials_data As New DataTable
        trials_data = helperClass.get_My_Demos_Trials("", "", "")
        edit_trial_label.Text += DisplayFunctions.My_Demos_Trials_HTML(trials_data, 100, CompanyID, ContactID)

    End Sub
    Public Sub ViewContactFolders(ByVal sender As Object, ByVal e As System.EventArgs) ' Handles view_folders.Click
        If foldersContainer.Visible = True Then
            ToggleButtons(False, False)
        Else
            ToggleButtons(True, False)
        End If
        folders_update_panel.Update()
    End Sub


    Private Sub trials_link_button_all_Click(sender As Object, e As EventArgs) Handles trials_link_button_all.Click, trails_link_button_active.Click

        If sender.id = "trails_link_button_active" Then
            sTask = ""
        Else
            sTask = "inactive"
        End If

        Fill_Trials_Summary_Tab()

        If Not Page.ClientScript.IsClientScriptBlockRegistered("masonryPost") Then
            System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "masonryPost", "loadMasonry();", True)
        End If
    End Sub

    Private Sub showAllActivities_Click(sender As Object, e As EventArgs) Handles showAllActivities.Click, showTop50Activities.Click, customerActivitiesFilter.SelectedIndexChanged
        If sender.id = "showAllActivities" Then
            sTask = "showall"
        ElseIf sender.id = "showTop50Activities" Then
            sTask = ""
        Else
            If showAllActivities.Visible = False Then
                sTask = "showall" 'Just in case they use the dropdown, now it will remember if they're looking at all activities
            End If
        End If

        Fill_Customer_Activities_FromView()

        If Not Page.ClientScript.IsClientScriptBlockRegistered("masonryPost") Then
            System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "masonryPost", "loadMasonry();", True)
        End If

        customerActivitiesUpdate.Update()
    End Sub


    Public Sub Fill_Trials_Summary_Tab()
        Dim service_table As New DataTable
        Dim htmlOut As New StringBuilder
        Dim temp_total_contact As Long = 0
        trial_label.Visible = True

        Dim toggleAllSubs As Boolean = False

        Dim helperClass As New displayCompanyDetailsFunctions

        If sTask.ToLower.Contains("inactive") Then
            toggleAllSubs = True
            trails_link_button_active.Visible = True
            trials_link_button_all.Visible = False

            ' trails_link_button_active.PostBackUrl = "/DisplayContactDetail.aspx?compid=" & CompanyID & "&conid=" & ContactID & "&task=active"
        Else
            toggleAllSubs = False
            trials_link_button_all.Visible = True
            trails_link_button_active.Visible = False

            'trails_link_button_active.PostBackUrl = "/DisplayContactDetail.aspx?compid=" & CompanyID & "&conid=" & ContactID & "&task=inactive"
        End If

        ' added msw for company/yacht re-posting
        If CompanyID = 0 Then
            If Trim(Session("LAST_COMP")) <> "" Then
                If IsNumeric(Session("LAST_COMP")) Then
                    CompanyID = CDbl(Session("LAST_COMP"))
                End If
            End If
        End If

        If ContactID = 0 Then
            If Trim(Session("LAST_CONTACT")) <> "" Then
                If IsNumeric(Session("LAST_CONTACT")) Then
                    ContactID = CDbl(Session("LAST_CONTACT"))
                End If
            End If
        End If

        service_table = helperClass.Return_Trial_Summary(CompanyID, JournalID, "N", True, ContactID)
        ' if we have any at all, show the box 
        If Not IsNothing(service_table) Then
            If service_table.Rows.Count > 0 Then
                Trials_Container.Visible = True
            Else
                Trials_Container.Visible = False
            End If
        Else
            Trials_Container.Visible = False
        End If




        'no roll up currently 
        '  If use_insight_roll = True Then
        '  service_table = helperClass.Return_Trial_Summary(CompanyID, JournalID, "Y", toggleAllSubs)
        '  Else
        service_table.Clear()
        service_table = helperClass.Return_Trial_Summary(CompanyID, JournalID, "N", toggleAllSubs, ContactID)
        '  End If


        If Not IsNothing(service_table) Then

            htmlOut.Append("<table id='serviceTable' width=""100%"" cellpadding=""0"" cellspacing=""0"" class=""formatTable blue small aircraftTable"">")
            htmlOut.Append("<tr class='header_row'>")
            htmlOut.Append("<td align='left'><b>SERVICE</b></td>")
            htmlOut.Append("<td align='right'><b>NAME</b></td>")
            htmlOut.Append("<td align='right'><b>PASSWORD</b></td>")
            htmlOut.Append("<td align='right'><b>STATUS</b></td>")
            htmlOut.Append("<td align='right'><b>USERID</b></td>")
            htmlOut.Append("<td align='right'><b>INSTALL</b></td>")

            htmlOut.Append("</tr>")

            If (service_table.Rows.Count > 0) Then


                For Each q As DataRow In service_table.Rows

                    htmlOut.Append("<tr bgcolor=""white"" valign='top'>")

                    htmlOut.Append("<td align='left' colspan='6'>" & q("SERVICE").ToString & "</td>")

                    htmlOut.Append("</tr>")

                    htmlOut.Append("<tr><td align='right'>&nbsp;</td><td align='right'>")

                    If Not IsDBNull(q("NAME")) Then
                        If Not String.IsNullOrEmpty(q("NAME").ToString.Trim) Then
                            If Not IsDBNull(q("contact_email_address")) Then
                                htmlOut.Append("<a class=""underline"" onclick='javascript:openSmallWindowJS(""adminSubErrors.aspx?email=" & q("contact_email_address") & "&sub_id=" & q("sub_id") & "&login=" & q("sublogin_login") & """, ""Display Trial Actions"");' title='Display Trial Actions'>")
                            End If
                            htmlOut.Append(q("NAME").ToString)
                            If Not IsDBNull(q("contact_email_address")) Then
                                htmlOut.Append("</a>")
                            End If
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("<td align='right'>")

                    If Not IsDBNull(q("PASSWORD")) Then
                        If Not String.IsNullOrEmpty(q("PASSWORD").ToString.Trim) Then
                            htmlOut.Append(q("PASSWORD").ToString)
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("<td align='right'>")

                    If Not IsDBNull(q("STATUS")) Then
                        If Not String.IsNullOrEmpty(q("STATUS").ToString.Trim) Then
                            htmlOut.Append(q("STATUS").ToString)
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("<td align='right'>")

                    If Not IsDBNull(q("USERID")) Then
                        If Not String.IsNullOrEmpty(q("USERID").ToString.Trim) Then
                            htmlOut.Append(q("USERID").ToString)
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("<td align='right'>")

                    If Not IsDBNull(q("INSTALL")) Then
                        If Not String.IsNullOrEmpty(q("INSTALL").ToString.Trim) Then
                            htmlOut.Append(q("INSTALL").ToString)
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("</tr>")

                Next
            Else
                trial_label.Visible = False
            End If

            htmlOut.Append("</table>")


            Me.trial_label.Text = htmlOut.ToString
        Else
            '   If MasterPage.aclsData_Temp.class_error <> "" Then
            '   MasterPage.LogError("CompanyTabs.ascx.vb -Fill_Service_Summary_Tab() - " & MasterPage.aclsData_Temp.class_error)
            'End If
        End If
    End Sub
    ''' <summary>
    ''' Toggle Buttons
    ''' </summary>
    ''' <param name="FoldersVis"></param>
    ''' <remarks></remarks>
    Private Sub ToggleButtons(ByVal FoldersVis As Boolean, ByVal ActionNotesVis As Boolean)
        If FoldersVis Then
            closeFolders.Visible = True
            ' folders_tab.Visible = True
            foldersContainer.Visible = True
            'view_folders.CssClass = "blue_button float_left noBefore"
            'view_folders.Text = "<strong>Close Folders</strong>"
        Else
            closeFolders.Visible = False
            'folders_tab.Visible = False
            foldersContainer.Visible = False
            'view_folders.CssClass = "gray_button float_left noBefore"
            'view_folders.Text = "<strong>Folders</strong>"
            folders_update_panel.Update()
        End If

        If ActionNotesVis Then
            closeNotes.Visible = True
            notesPanel.Visible = True
            actionPanel.Visible = True
            'view_notes.CssClass = "blue_button float_left"
            notes_update_panel.Update()
        Else
            closeNotes.Visible = False
            notesPanel.Visible = False
            actionPanel.Visible = False
            'view_notes.CssClass = "gray_button float_left"
            notes_update_panel.Update()
        End If

        If Not Page.ClientScript.IsClientScriptBlockRegistered("masonryPost") Then
            System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "masonryPost", "loadMasonry();", True)
        End If
    End Sub

    ''' <summary>
    ''' This function allows saving of static folders.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SaveStaticFolders()
        folders_label = clsGeneral.clsGeneral.SaveStaticFolders(folders_label, Master.aclsData_Temp, 0, 0, 0, ContactID, 0, 0)
        folders_update_panel.Update()
    End Sub


End Class