' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/homeTables.aspx.vb $
'$$Author: Matt $
'$$Date: 6/17/20 10:12a $
'$$Modtime: 6/17/20 10:03a $
'$$Revision: 45 $
'$$Workfile: homeTables.aspx.vb $
'
' ********************************************************************************

Partial Public Class homeTables
    Inherits System.Web.UI.Page

    Public Shared masterPage As New Object

    Private localDatalayer As viewsDataLayer
    Private localCriteria As New viewSelectionCriteriaClass
    Dim TempTable As New DataTable
    Dim type_of As String = ""
    Dim sub_type_of As String = ""
    Dim comp_id As Long = 0
    Dim user_id As String = ""
    Dim activityid As Long = 0
    Dim action As String = ""


    Private Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

        Try
            If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
                Me.MasterPageFile = "~/EvoStyles/EmptyCustomerAdminTheme.master"
                masterPage = DirectCast(Page.Master, EmptyCustomerAdminTheme)
            ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Then
                Me.MasterPageFile = "~/EvoStyles/EmptyHomebaseTheme.Master"
                masterPage = DirectCast(Page.Master, EmptyHomebaseTheme)
            ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO Then
                Me.MasterPageFile = "~/EvoStyles/EmptyEvoTheme.master"
                masterPage = DirectCast(Page.Master, EmptyEvoTheme)
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

        ' get request variable
        If Session.Item("crmUserLogon") <> True Then 'And Trim(Request("homebase")) <> "Y" Then
            Response.Redirect("Default.aspx", False)
        Else



            'Setting up page to display correct text/title.
            Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("Services")

            'If Not Session.Item("localPreferences").loadUserSession(sErrorString, CLng(HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString),
            '                                                      HttpContext.Current.Session.Item("localUser").crmUserLogin.ToString,
            '                                                      CLng(HttpContext.Current.Session.Item("localUser").crmSubSeqNo.ToString),
            '                                                      CLng(HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString)) Then
            '    Response.Redirect("Default.aspx", True)
            'End If




            If String.IsNullOrEmpty(HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim) Then
                HttpContext.Current.Session.Item("jetnetClientDatabase") = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
            End If

            If Trim(Request("type_of")) <> "" Then
                type_of = Trim(Request("type_of"))
            End If

            If Trim(Request("sub_type_of")) <> "" Then
                sub_type_of = Trim(Request("sub_type_of"))
            End If

            If Trim(Request("comp_id")) <> "" Then
                comp_id = Trim(Request("comp_id"))
            End If

            If Trim(Request("user_id")) <> "" Then
                user_id = Trim(Request("user_id"))
            End If


            If Trim(Request("note_text")) <> "" Then
                Me.note_text.Text = Trim(Request("note_text"))
            End If


            If Trim(Request("activityid")) <> "" Then
                activityid = Trim(Request("activityid"))
            End If

            If Trim(Request("action")) <> "" Then
                action = Trim(Request("action"))
            End If


            If Not IsPostBack Then


                localDatalayer = New viewsDataLayer
                localDatalayer.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
                localDatalayer.clientConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
                If Trim(type_of) = "Company" Then

                    'Fill Company label. same way as company details page.
                    crmWebClient.CompanyFunctions.Fill_Information_Tab(Nothing, information_label, masterPage, comp_id, 0, "", New Label, New AjaxControlToolkit.TabContainer, company_address, company_name, False, False, "JETNET", 0, 0)

                    masterPage.SetPageText(company_name.Text & " Services Used")

                    If Trim(sub_type_of) = "ServicesUsed" Then


                        If Trim(Request("edit")) = "delete" And Trim(Request("id")) <> "" Then
                            Call delete_button_click_inner()
                        End If

                        Call run_company_services_used()


                    ElseIf Trim(sub_type_of) = "Customer Execution" Then
                        Call run_company_customer_execution()
                        'www.homebase.com/homeTables.aspx?type_of=Company&sub_type_of=Customer Execution&comp_id=135887&excutionid=####&user_id=MVIT&homebase=Y&action=update
                    ElseIf Trim(sub_type_of.ToLower) = "journal" Or Trim(sub_type_of.ToLower) = "marketing" Then ' just display a simple display.
                        If action = "add" Then
                            SimpleAddDisplay()
                        Else
                            Run_Simple_Display(activityid, comp_id)
                        End If
                    ElseIf Trim(sub_type_of.ToLower) = "companynote" Then
                        RunCompanyDisplayNote()
                    ElseIf Trim(sub_type_of.ToLower) = "company documents" Then
                        'Document entry
                        RunCompanyDocumentDisplay(activityid, comp_id)
                    ElseIf Trim(sub_type_of.ToLower) = "customer activity" Then 'support
                        'Document entry
                        RunSupportEntry(activityid)
                    End If
                End If


            End If

        End If

    End Sub

    Private Sub RunCompanyDisplayNote()
        'This displays only the company note.
        journalInformationRow.Visible = False
        companyListingGrid.Visible = False
        companyMarketingNoteEdit.Visible = True

        Dim DisplayMarketingNote As New DataTable
        DisplayMarketingNote = getMarketingNote(comp_id)

        If Not IsNothing(DisplayMarketingNote) Then
            If DisplayMarketingNote.Rows.Count > 0 Then
                If Not IsDBNull(DisplayMarketingNote.Rows(0).Item("comp_marketing_notes")) Then
                    marketingNote.Text = DisplayMarketingNote.Rows(0).Item("comp_marketing_notes").ToString
                End If
            End If
        End If
    End Sub

    Public Function Edit_CompanyMarketingNotes(ByVal marketingNotes As String, companyID As Long) As Integer
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim sql As String = ""
        Edit_CompanyMarketingNotes = 0
        Try
            'make sure there's a session set.
            If HttpContext.Current.Session.Item("crmUserLogon") = True Then
                If companyID > 0 Then


                    sql = " UPDATE " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[HomeBase].jetnet_ra.dbo.", "") + "Company "
                    sql = sql & " SET comp_marketing_notes = '" & marketingNotes & "' WHERE comp_id = '" & companyID.ToString & "' and comp_journ_id = 0 "

                    SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
                    SqlConn.Open()
                    SqlCommand.Connection = SqlConn

                    SqlCommand.CommandText = sql
                    SqlCommand.ExecuteNonQuery()

                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Edit_CompanyMarketingNotes(ByVal marketingNotes As String, companyID As Long) As Integer</b><br />" & sql


                    Edit_CompanyMarketingNotes = 1
                    sql = ""
                End If
            End If

        Catch ex As Exception
            Edit_CompanyMarketingNotes = 0
            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in Edit_CompanyMarketingNotes(ByVal marketingNotes As String, companyID As Long) As Integer " & ex.Message
        Finally
            SqlCommand.Dispose()
            SqlConn.Close()
            SqlConn.Dispose()
        End Try
    End Function



    ''' <summary>
    ''' Once admin_center_datalayer is back in, this function and the one on displayCompanyDetails can be consolidated and moved there.
    ''' </summary>
    ''' <param name="inCompID"></param>
    ''' <returns></returns>
    Public Function getMarketingNote(ByVal inCompID As Long) As DataTable

        Dim atemptable As New DataTable
        Dim subQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            subQuery.Append("SELECT comp_marketing_notes FROM " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[HomeBase].jetnet_ra.dbo.", "") + "Company WITH(NOLOCK)")
            subQuery.Append(" WHERE comp_id = @comp_id and comp_journ_id = 0")

            SqlCommand.Parameters.Add("@comp_id", SqlDbType.Int).Value = inCompID.ToString.Trim


            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

            SqlConn.Open()
            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 90

            SqlCommand.CommandText = subQuery.ToString
            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)


            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, subQuery.ToString)

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

    Private Sub RunSupportEntry(activityID As Long)
        journalInformationRow.Visible = True
        companyListingGrid.Visible = False
        companyMarketingNoteEdit.Visible = False
        Dim htmlOut As New StringBuilder
        Dim DataOut As New DataTable



        DataOut = Get_Support_Entry_Query(activityID)
        If Not IsNothing(DataOut) Then
            If DataOut.Rows.Count > 0 Then
                htmlOut.Append("<div class=""Box""><div class=""subHeader padding_left emphasisColor"">Details</div>")
                htmlOut.Append("<table class=""formatTable blue"">")
                htmlOut.Append("<tr>")
                htmlOut.Append("<td width=""80""><strong>Date: </strong></td><td>")
                If Not IsNothing(DataOut.Rows(0).Item("cstact_added_date")) Then
                    htmlOut.Append(clsGeneral.clsGeneral.TwoPlaceYear(DataOut.Rows(0).Item("cstact_added_date")))
                End If

                htmlOut.Append("</td>")
                htmlOut.Append("<td  width=""80""><strong>User: </strong></td><td>")

                If Not IsDBNull(DataOut.Rows(0).Item("user_first_name")) Then
                    htmlOut.Append(DataOut.Rows(0).Item("user_first_name"))
                End If
                If Not IsDBNull(DataOut.Rows(0).Item("user_last_name")) Then
                    htmlOut.Append(" " & DataOut.Rows(0).Item("user_last_name"))
                End If
                htmlOut.Append("</td>")
                htmlOut.Append("</tr>")

                htmlOut.Append("<tr>")
                htmlOut.Append("<td><strong>Note: </strong></td><td colspan=""4"">")
                If Not IsNothing(DataOut.Rows(0).Item("cstact_note")) Then
                    If Not String.IsNullOrEmpty(DataOut.Rows(0).Item("cstact_note")) Then
                        htmlOut.Append(DataOut.Rows(0).Item("cstact_note"))
                    End If
                End If

                htmlOut.Append("</td>")


                htmlOut.Append("</tr>")

                htmlOut.Append("</table>")
            End If
        End If
        journalDisplayText.Text = htmlOut.ToString
    End Sub
    Private Sub RunCompanyDocumentDisplay(documentID As Long, companyID As Long)
        journalInformationRow.Visible = True
        companyListingGrid.Visible = False
        companyMarketingNoteEdit.Visible = False
        Dim htmlOut As New StringBuilder
        Dim DataOut As New DataTable


        DataOut = Get_Document_Display_Query(activityid, companyID)
        If Not IsNothing(DataOut) Then
            If DataOut.Rows.Count > 0 Then
                htmlOut.Append("<div class=""Box""><div class=""subHeader padding_left emphasisColor"">Details</div>")
                htmlOut.Append("<table class=""formatTable blue"">")
                htmlOut.Append("<tr>")
                htmlOut.Append("<td width=""80""><strong>Date: </strong></td><td>")
                If Not IsNothing(DataOut.Rows(0).Item("compdoc_doc_date")) Then
                    htmlOut.Append(clsGeneral.clsGeneral.TwoPlaceYear(DataOut.Rows(0).Item("compdoc_doc_date")))
                End If

                htmlOut.Append("</td>")
                htmlOut.Append("<td  width=""80""><strong>User: </strong></td><td>")

                If Not IsDBNull(DataOut.Rows(0).Item("user_first_name")) Then
                    htmlOut.Append(DataOut.Rows(0).Item("user_first_name"))
                End If
                If Not IsDBNull(DataOut.Rows(0).Item("user_last_name")) Then
                    htmlOut.Append(" " & DataOut.Rows(0).Item("user_last_name"))
                End If
                htmlOut.Append("</td>")
                htmlOut.Append("</tr>")

                htmlOut.Append("<tr>")
                htmlOut.Append("<td><strong>Subject: </strong></td><td>")
                If Not IsNothing(DataOut.Rows(0).Item("compdoc_subject")) Then
                    If Not String.IsNullOrEmpty(DataOut.Rows(0).Item("compdoc_subject")) Then
                        htmlOut.Append(DataOut.Rows(0).Item("compdoc_subject"))
                    End If
                End If

                htmlOut.Append("</td>")

                htmlOut.Append("<td><strong>Filename: </strong></td><td>")

                If Not IsDBNull(DataOut.Rows(0).Item("compdoc_filename")) Then
                    htmlOut.Append("<a href='http://jetnet4/contracts/" & DataOut.Rows(0).Item("compdoc_filename") & "' target='_blank'>View Document</a>")
                End If
                htmlOut.Append("</td>")
                htmlOut.Append("</tr>")


                If Not IsNothing(DataOut.Rows(0).Item("compdoc_description")) Then
                    If Not String.IsNullOrEmpty(DataOut.Rows(0).Item("compdoc_description")) Then
                        htmlOut.Append("<tr>")
                        htmlOut.Append("<td colspan=""4""><strong>Description:</strong><br /><p>")
                        htmlOut.Append(DataOut.Rows(0).Item("compdoc_description"))
                        htmlOut.Append("</p></td>")
                        htmlOut.Append("</tr>")
                    End If
                End If


                htmlOut.Append("</table>")
            End If
        End If
        journalDisplayText.Text = htmlOut.ToString
    End Sub
    Private Sub SimpleAddDisplay()
        journalEdit.Visible = True
        saveMarketingNote.Visible = False
        journalInformationRow.Visible = True
        companyListingGrid.Visible = False
        journContactRow.Visible = True
        addMarketingNote.Visible = True
        removeMarketingNote.Visible = False
        'Set up homebase users dropdown
        Dim crmTestMaster As New main_site
        crmTestMaster.aclsData_Temp.JETNET_DB = Application.Item("crmJetnetDatabase")
        crmTestMaster.aclsData_Temp.client_DB = Application.Item("crmClientDatabase")
        clsGeneral.clsGeneral.FillCRMUser_Homebase(crmTestMaster, "Prospects", journ_user, False, "")
        journ_user.Items.FindByValue("0").Text = "NONE SELECTED"


        Try
            journ_user.SelectedValue = Session.Item("homebaseUserClass").home_user_id
        Catch ex As Exception
            journ_user.SelectedValue = "0"
        End Try

        journ_date.Text = FormatDateTime(Now(), DateFormat.ShortDate)
        'Note type
        journ_note_type.Attributes.Add("onchange", "NoteSubjectReplace();")
        journ_note_type.Items.Add(New ListItem("NONE SELECTED", ""))
        Dim noteType As DataTable = Get_Journal_Note_Type()
        If Not IsNothing(noteType) Then
            If noteType.Rows.Count > 0 Then
                For Each r As DataRow In noteType.Rows
                    'subcategory_code, jcat_subcategory_name  
                    journ_note_type.Items.Add(New ListItem(r("jcat_subcategory_name"), r("jcat_subcategory_code")))
                Next
            End If
        End If

        Try
            journ_note_type.SelectedValue = "MN"
        Catch ex As Exception
            journ_note_type.SelectedValue = ""
        End Try
        Dim ContactTable As New DataTable
        Dim contactName As String = ""
        ContactTable = masterPage.aclsData_Temp.GetContacts(comp_id, "JETNET", "Y", 0)

        If Not IsNothing(ContactTable) Then
            If ContactTable.Rows.Count > 0 Then
                For Each r As DataRow In ContactTable.Rows
                    contactName = ""

                    If Not IsNothing(r("contact_first_name")) Then
                        contactName = r("contact_first_name")
                    End If
                    If Not IsNothing(r("contact_last_name")) Then
                        contactName += " " & r("contact_last_name")
                    End If

                    journ_contact.Items.Add(New ListItem(contactName, r("contact_id")))
                Next

            End If


        End If

    End Sub

    Private Sub Run_Simple_Display(activityID As Long, companyID As Long)
        journalInformationRow.Visible = True
        companyListingGrid.Visible = False
        journContactRow.Visible = True
        addMarketingNote.Visible = False
        Dim DataOut As New DataTable
        DataOut = Get_Simple_Display_Query(activityID, companyID)
        If Not IsNothing(DataOut) Then
            If DataOut.Rows.Count > 0 Then
                journalEdit.Visible = True
                removeMarketingNote.Visible = True
                If Not IsNothing(DataOut.Rows(0).Item("journ_date")) Then
                    journ_date.Text = FormatDateTime(DataOut.Rows(0).Item("journ_date"), vbShortDate)
                End If


                Dim crmTestMaster As New main_site
                crmTestMaster.aclsData_Temp.JETNET_DB = Application.Item("crmJetnetDatabase")
                crmTestMaster.aclsData_Temp.client_DB = Application.Item("crmClientDatabase")

                clsGeneral.clsGeneral.FillCRMUser_Homebase(crmTestMaster, "Prospects", journ_user, False, "")

                Try
                    journ_user.SelectedValue = DataOut.Rows(0).Item("journ_user_id")
                Catch ex As Exception
                    journ_user.SelectedValue = 0
                End Try


                journ_contact.SelectedValue = 0



                Dim ContactTable As New DataTable
                Dim contactName As String = ""
                ContactTable = masterPage.aclsData_Temp.GetContacts(companyID, "JETNET", "Y", 0)

                If Not IsNothing(ContactTable) Then
                    If ContactTable.Rows.Count > 0 Then
                        For Each r As DataRow In ContactTable.Rows
                            contactName = ""

                            If Not IsNothing(r("contact_first_name")) Then
                                contactName = r("contact_first_name")
                            End If
                            If Not IsNothing(r("contact_last_name")) Then
                                contactName += " " & r("contact_last_name")
                            End If

                            journ_contact.Items.Add(New ListItem(contactName, r("contact_id")))
                        Next

                        If Not IsDBNull(DataOut.Rows(0).Item("contact_id")) Then
                            Try
                                journ_contact.SelectedValue = DataOut.Rows(0).Item("contact_id")
                            Catch ex As Exception
                                journ_contact.SelectedValue = 0
                            End Try
                        End If
                    End If


                End If


                If Not IsDBNull(DataOut.Rows(0).Item("ac_id")) Then
                    journAircraftRow.Visible = True
                    Dim newListItem As New ListItem

                    If Not IsNothing(DataOut.Rows(0).Item("amod_make_name")) Then
                        newListItem.Text = DataOut.Rows(0).Item("amod_make_name")
                    End If
                    If Not IsNothing(DataOut.Rows(0).Item("amod_model_name")) Then
                        newListItem.Text += " " & DataOut.Rows(0).Item("amod_model_name")
                    End If

                    If Not IsNothing(DataOut.Rows(0).Item("ac_ser_no_full")) Then
                        newListItem.Text += " SN:  " & DataOut.Rows(0).Item("ac_ser_no_full")
                    End If
                    journ_ac.Items.Add(newListItem)
                End If

                'Note type
                'Get_Journal_Note_Type()
                journ_note_type.Items.Add(New ListItem("NONE SELECTED", ""))
                Dim noteType As DataTable = Get_Journal_Note_Type()
                If Not IsNothing(noteType) Then
                    If noteType.Rows.Count > 0 Then
                        For Each r As DataRow In noteType.Rows
                            'subcategory_code, jcat_subcategory_name  
                            journ_note_type.Items.Add(New ListItem(r("jcat_subcategory_name"), r("jcat_subcategory_code")))
                        Next
                    End If
                End If

                Try
                    journ_note_type.SelectedValue = DataOut.Rows(0).Item("journ_subcategory_code")
                Catch ex As Exception
                    journ_note_type.SelectedIndex = ""
                End Try

                If Not IsNothing(DataOut.Rows(0).Item("journ_subject")) Then
                    If Not String.IsNullOrEmpty(DataOut.Rows(0).Item("journ_subject")) Then
                        journ_subject.Text = DataOut.Rows(0).Item("journ_subject")
                    End If
                End If

                If Not IsNothing(DataOut.Rows(0).Item("journ_description")) Then
                    If Not String.IsNullOrEmpty(DataOut.Rows(0).Item("journ_description")) Then
                        journ_description.Text = Trim(Server.HtmlDecode(DataOut.Rows(0).Item("journ_description")))   ' added MSW 3/6/20

                        textRemaining.InnerText = (4000 - journ_description.Text.Length).ToString
                    End If
                End If
            End If
        End If
    End Sub
    Public Sub run_company_customer_execution()

        TempTable = get_company_execution_record(comp_id, activityid)

        If Not IsNothing(TempTable) Then
            If TempTable.Rows.Count > 0 Then
                If activityid > 0 Then
                    Call DisplayCompanyExecutionEdit(TempTable)
                    exec_add_button.Text = "Update"
                Else
                    listing_label.Text = display_execution_grid(TempTable)
                    exec_add_button.Text = "Add"
                End If
            End If
        End If

        'If Trim(Request("edit")) <> "" Then

        '    If Trim(Request("edit")) = "add" Then


        '    ElseIf Trim(Request("edit")) = "edit" And Trim(Request("id")) <> "" Then

        '    End If
        'End If 


    End Sub

    Public Function display_execution_grid(companyInfo As DataTable) As String
        Dim htmlOut As New StringBuilder
        htmlOut.Append("<div class=""subHeader"">Execution</div>")
        htmlOut.Append("<table cellpadding='0' cellspacing='0' border='0' class=""formatTable blue"" width=""100%"">")

        htmlOut.Append("<thead><tr><td><b>NAME</b></td><td><b>Monthly Price</b></td><td><b>NOTES</b></td><td width=""64"" align=""center""><b>EDIT</b></td><td width=""64"" align=""center""><b>DELETE</b></td></tr></thead>")

        htmlOut.Append("<tbody>")
        For Each r As DataRow In TempTable.Rows

            htmlOut.Append("<tr>")

            'Service Name Column
            htmlOut.Append("<td>")
            If Not IsDBNull(r.Item("comp_name")) Then
                If Not String.IsNullOrEmpty(r.Item("comp_name").ToString) Then
                    htmlOut.Append("<a href='homeTables.aspx?type_of= " & type_of & "&sub_type_of=" & sub_type_of & "&comp_id=" & comp_id & "&edit=edit&activityid=" & r.Item("cstexcform_id") & "&user_id=" & user_id & "&homebase=Y'>")
                    htmlOut.Append(r.Item("comp_name").ToString.Trim)
                    htmlOut.Append("</a>")
                End If
            End If
            htmlOut.Append("</td>")

            'cstexcform_monthly_fee
            htmlOut.Append("<td>")
            If Not IsDBNull(r.Item("cstexcform_monthly_fee")) Then
                If Not String.IsNullOrEmpty(r.Item("cstexcform_monthly_fee").ToString) Then
                    htmlOut.Append(r.Item("cstexcform_monthly_fee").ToString.Trim)
                End If
            End If
            htmlOut.Append("</td>")

            'Notes Column
            htmlOut.Append("<td>")
            If Not IsDBNull(r.Item("cstexcform_notes")) Then
                If Not String.IsNullOrEmpty(r.Item("cstexcform_notes").ToString) Then
                    htmlOut.Append(r.Item("cstexcform_notes").ToString.Trim)
                End If
            End If
            htmlOut.Append("</td>")


            'Edit Column
            htmlOut.Append("<td align=""center"">")
            htmlOut.Append("<a href='homeTables.aspx?type_of= " & type_of & "&sub_type_of=" & sub_type_of & "&comp_id=" & comp_id & "&edit=edit&activityid=" & r.Item("cstexcform_id") & "&user_id=" & user_id & "&homebase=Y'><img src=""/images/edit_icon.png"" width=""14px""></a></td>")


            'Delete Column
            htmlOut.Append("<td align=""center"">")
            htmlOut.Append("<a onclick = ""return confirm('Do you really want to remove this record?');""><i class=""fa fa-trash-o""></i></a>")
            htmlOut.Append("</td>")

            htmlOut.Append("</tr>")

        Next
        htmlOut.Append("</table>")

        htmlOut.Append("<a href='homeTables.aspx?type_of= " & type_of & "&sub_type_of=" & sub_type_of & "&comp_id=" & comp_id & "&edit=add&user_id=" & user_id & "&homebase=Y' class=""float_right"">+ ADD</a>")
        htmlOut.Append("<br clear=""all"" />")


        Return htmlOut.ToString
    End Function
    Public Sub run_company_services_used()
        Me.listing_label.Text = ""

        If Trim(Request("edit")) <> "" Then
            Me.droplabel1.Text = "Services Used:"


            Me.edit_panel.Visible = True
            Me.Textbox1.Text = ""
            Me.TextLabel1.Visible = False
            Me.Textbox1.Visible = False


            If Trim(Request("edit")) = "add" Then
                Call create_company_listing(comp_id, 0)

                Me.DateLabel1.Text = "Verified Date: "
                Me.Datebox1.Text = Date.Now().Date
                Me.DateLabel2.Text = "Service End Date: "
                Me.Datebox2.Text = ""

                ' NOTES
                Me.BottomLabel1.Text = "Notes: "
                Me.BottomText1.Text = ""
                Me.submit_button.Text = "Add"
                Me.delete_button.Visible = False


                Call Fill_Dropdown1(Me.Dynamic_Dropdown1, "")

            ElseIf Trim(Request("edit")) = "edit" And Trim(Request("id")) <> "" Then
                listing_label.Text = create_company_listing(comp_id, Trim(Request("id")))
                Me.submit_button.Text = "Update"
                Me.delete_button.Visible = True
            End If

        Else
            If comp_id > 0 Then
                Me.listing_panel.Visible = True
                listing_label.Text = create_company_listing(comp_id, 0)
            End If
        End If


    End Sub
    Public Sub delete_button_click_inner()
        Dim temp_text As String = ""


        If Trim(delete_button.Text) = "Delete" Then
            temp_text = delete_company_services_used()
            Me.edit_panel.Visible = False

            Call insert_research_note_services_used()
            Response.Redirect("homeTables.aspx?type_of= " & type_of & "&sub_type_of=" & sub_type_of & "&comp_id=" & comp_id & "&note_text=" & temp_text & "&user_id=" & user_id & "&homebase=Y")
        End If


    End Sub
    Sub delete_button_click(ByVal sender As Object, ByVal e As System.EventArgs) Handles delete_button.Click

        Call delete_button_click_inner()

    End Sub


    Sub exec_delete_button_click(ByVal sender As Object, ByVal e As System.EventArgs) Handles exec_delete_button.Click

        Dim temp_text As String = ""

        If Trim(exec_delete_button.Text) = "Delete" Then
            temp_text = delete_company_execution()
            Me.edit_panel.Visible = False
        End If

        '  Response.Redirect("homeTables.aspx?type_of= " & type_of & "&sub_type_of=" & sub_type_of & "&comp_id=" & comp_id & "&note_text=" & temp_text & "&user_id=" & user_id & "&homebase=Y")

    End Sub

    Sub exec_add_button_click(ByVal sender As Object, ByVal e As System.EventArgs) Handles exec_add_button.Click

        Dim temp_text As String = ""

        If Trim(exec_add_button.Text) = "Add" Then
            temp_text = Insert_into_company_execution()
        ElseIf Trim(exec_add_button.Text) = "Update" Then
            temp_text = Update_company_execution()
        End If

        '  Response.Redirect("homeTables.aspx?type_of= " & type_of & "&sub_type_of=" & sub_type_of & "&comp_id=" & comp_id & "&note_text=" & temp_text & "&user_id=" & user_id & "&homebase=Y")

    End Sub

    Sub cancel_button_click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cancel_button.Click

        Response.Redirect("homeTables.aspx?type_of= " & type_of & "&sub_type_of=" & sub_type_of & "&comp_id=" & comp_id & "&user_id=" & user_id & "&homebase=Y")

    End Sub

    Sub submit_button_click(ByVal sender As Object, ByVal e As System.EventArgs) Handles submit_button.Click

        Dim temp_text As String = ""

        If Trim(submit_button.Text) = "Add" Then
            temp_text = Insert_into_company_services_used()
        ElseIf Trim(submit_button.Text) = "Update" Then
            temp_text = Update_company_services_used()
        End If

        Me.edit_panel.Visible = False

        Response.Redirect("homeTables.aspx?type_of= " & type_of & "&sub_type_of=" & sub_type_of & "&comp_id=" & comp_id & "&note_text=" & temp_text & "&user_id=" & user_id & "&homebase=Y")

    End Sub

    Public Function DisplayCompanyGrid(companyInfo As DataTable) As String
        Dim htmlOut As New StringBuilder
        Dim temp_service As String = ""

        If Not IsNothing(TempTable) Then
            If TempTable.Rows.Count > 0 Then


                htmlOut.Append("<div class=""subHeader"">SERVICES</div>")
                htmlOut.Append("<table cellpadding='0' cellspacing='0' border='0' class=""formatTable blue"" width=""100%"">")

                htmlOut.Append("<thead><tr><td><b>NAME</b></td><td><b>END DATE</b></td><td><b>LAST VERIFIED</b></td><td><b>NOTES</b></td><td width=""64"" align=""center""><b>EDIT</b></td><td width=""64"" align=""center""><b>DELETE</b></td></tr></thead>")

                htmlOut.Append("<tbody>")
                For Each r As DataRow In TempTable.Rows

                    htmlOut.Append("<tr>")

                    'Service Name Column
                    temp_service = ""
                    htmlOut.Append("<td>")
                    If Not IsDBNull(r.Item("SERVICENAME")) Then
                        If Not String.IsNullOrEmpty(r.Item("SERVICENAME").ToString) Then
                            htmlOut.Append("<a href='homeTables.aspx?type_of= " & type_of & "&sub_type_of=" & sub_type_of & "&comp_id=" & comp_id & "&edit=edit&id=" & r.Item("csu_id") & "&user_id=" & user_id & "&homebase=Y'>")
                            htmlOut.Append(r.Item("SERVICENAME").ToString.Trim)
                            temp_service = r.Item("SERVICENAME").ToString.Trim
                            htmlOut.Append("</a>")
                        End If
                    End If
                    htmlOut.Append("</td>")

                    'End Date Column
                    htmlOut.Append("<td>")
                    If Not IsDBNull(r.Item("ENDDATE")) Then
                        If Not String.IsNullOrEmpty(r.Item("ENDDATE").ToString) Then
                            htmlOut.Append(FormatDateTime(r.Item("ENDDATE"), DateFormat.ShortDate))
                        End If
                    End If
                    htmlOut.Append("</td>")

                    'Last Verified Column
                    htmlOut.Append("<td>")
                    If Not IsDBNull(r.Item("LASTVERIFIED")) Then
                        If Not String.IsNullOrEmpty(r.Item("LASTVERIFIED").ToString) Then
                            htmlOut.Append(FormatDateTime(r.Item("LASTVERIFIED"), DateFormat.ShortDate))
                        End If
                    End If
                    htmlOut.Append("</td>")

                    'Notes Column
                    htmlOut.Append("<td>")
                    If Not IsDBNull(r.Item("NOTES")) Then
                        If Not String.IsNullOrEmpty(r.Item("NOTES").ToString) Then
                            htmlOut.Append(r.Item("NOTES").ToString.Trim)
                        End If
                    End If
                    htmlOut.Append("</td>")

                    'Edit Column
                    htmlOut.Append("<td align=""center"">")
                    htmlOut.Append("<a href='homeTables.aspx?type_of= " & type_of & "&sub_type_of=" & sub_type_of & "&comp_id=" & comp_id & "&edit=edit&id=" & r.Item("csu_id") & "&user_id=" & user_id & "&homebase=Y'><img src=""/images/edit_icon.png"" width=""14px""></a></td>")


                    'Delete Column
                    htmlOut.Append("<td align=""center"">")
                    htmlOut.Append("<a href='homeTables.aspx?type_of= " & type_of & "&sub_type_of=" & sub_type_of & "&comp_id=" & comp_id & "&service=" & temp_service & "&edit=delete&id=" & r.Item("csu_id") & "&user_id=" & user_id & "&homebase=Y' onclick = ""return confirm('Do you really want to remove this record?' );""><i class=""fa fa-trash-o""></i></a>")   ' 
                    htmlOut.Append("</td>")

                    htmlOut.Append("</tr>")

                Next
                htmlOut.Append("</table>")


            End If
        End If


        htmlOut.Append("<a href='homeTables.aspx?type_of= " & type_of & "&sub_type_of=" & sub_type_of & "&comp_id=" & comp_id & "&edit=add&user_id=" & user_id & "&homebase=Y' class=""float_right"">+ ADD</a>")
        htmlOut.Append("<br clear=""all"" />")


        Return htmlOut.ToString
    End Function

    Private Sub DisplayCompanyExecutionEdit(companyInfo As DataTable)
        Me.execution_panel.Visible = True

        Me.exec_label.Text = ""
        Me.exec_monthly_price.Text = ""
        Me.exec_notes.Text = ""
        Me.exec_id.Text = activityid

        Dim temp_action As String = ""
        Dim temp_sub_id As String = ""

        For Each r As DataRow In companyInfo.Rows

            'If Not IsDBNull(r.Item("comp_name")) Then
            '    If Not String.IsNullOrEmpty(r.Item("comp_name").ToString) Then
            '        Me.exec_label.Text = (r.Item("comp_name").ToString.Trim)
            '    End If
            'End If

            If Not IsDBNull(r.Item("sub_service_name")) Then
                If Not String.IsNullOrEmpty(r.Item("sub_service_name").ToString) Then
                    Me.exec_label.Text = (r.Item("sub_service_name").ToString.Trim)
                End If
            End If


            If Not IsDBNull(r.Item("cstexcform_monthly_fee")) Then
                If Not String.IsNullOrEmpty(r.Item("cstexcform_monthly_fee").ToString) Then
                    Me.exec_monthly_price.Text = FormatNumber((r.Item("cstexcform_monthly_fee").ToString.Trim), 2)
                End If
            End If

            If Not IsDBNull(r.Item("cstexcform_monthly_list_fee")) Then
                If Not String.IsNullOrEmpty(r.Item("cstexcform_monthly_list_fee").ToString) Then
                    Me.exec_list_fee.Text = FormatNumber((r.Item("cstexcform_monthly_list_fee").ToString.Trim), 2)
                End If
            End If

            If Not IsDBNull(r.Item("cstexcform_monthly_net_fee")) Then
                If Not String.IsNullOrEmpty(r.Item("cstexcform_monthly_net_fee").ToString) Then
                    Me.exec_monthly_net.Text = FormatNumber((r.Item("cstexcform_monthly_net_fee").ToString.Trim), 2)
                End If
            End If

            If Not IsDBNull(r.Item("cstexcform_notes")) Then
                If Not String.IsNullOrEmpty(r.Item("cstexcform_notes").ToString) Then
                    Me.exec_notes.Text = (r.Item("cstexcform_notes").ToString.Trim)
                End If
            End If

            Me.exec_new_customer.Checked = False
            If Not IsDBNull(r.Item("cstexcform_new_customer")) Then
                If Trim(r.Item("cstexcform_new_customer")) = "Y" Then
                    Me.exec_new_customer.Checked = True
                End If
            End If

            Me.exec_trial.Checked = False
            If Not IsDBNull(r.Item("cstexcform_trial")) Then
                If Trim(r.Item("cstexcform_trial")) = "Y" Then
                    Me.exec_trial.Checked = True
                End If
            End If

            Me.exec_new_contract.Checked = False
            If Not IsDBNull(r.Item("cstexcform_new_contract")) Then
                If Trim(r.Item("cstexcform_new_contract")) = "Y" Then
                    Me.exec_new_contract.Checked = True
                End If
            End If

            Me.exec_re_connected.Checked = False
            If Not IsDBNull(r.Item("cstexcform_re_connected")) Then
                If Trim(r.Item("cstexcform_re_connected")) = "Y" Then
                    Me.exec_re_connected.Checked = True
                End If
            End If

            Me.exec_addl_location.Checked = False
            If Not IsDBNull(r.Item("cstexcform_addl_location")) Then
                If Trim(r.Item("cstexcform_addl_location")) = "Y" Then
                    Me.exec_addl_location.Checked = True
                End If
            End If

            Me.exec_upgrade.Checked = False
            If Not IsDBNull(r.Item("cstexcform_upgrade")) Then
                If Trim(r.Item("cstexcform_upgrade")) = "Y" Then
                    Me.exec_upgrade.Checked = True
                End If
            End If

            Me.exec_downgrade.Checked = False
            If Not IsDBNull(r.Item("cstexcform_downgrade")) Then
                If Trim(r.Item("cstexcform_downgrade")) = "Y" Then
                    Me.exec_downgrade.Checked = True
                End If
            End If

            Me.exec_interrupted.Checked = False
            If Not IsDBNull(r.Item("cstexcform_interrupted")) Then
                If Trim(r.Item("cstexcform_interrupted")) = "Y" Then
                    Me.exec_interrupted.Checked = True
                End If
            End If

            Me.exec_cancellation.Checked = False
            If Not IsDBNull(r.Item("cstexcform_cancellation")) Then
                If Trim(r.Item("cstexcform_cancellation")) = "Y" Then
                    Me.exec_cancellation.Checked = True
                End If
            End If

            Me.exec_addl_license.Checked = False
            If Not IsDBNull(r.Item("cstexcform_addl_license")) Then
                If Trim(r.Item("cstexcform_addl_license")) = "Y" Then
                    Me.exec_addl_license.Checked = True
                End If
            End If

            If Not IsDBNull(r.Item("cstexcform_exc_seqnbr")) Then
                If Not String.IsNullOrEmpty(r.Item("cstexcform_exc_seqnbr").ToString) Then
                    Me.exec_seq.Text = (r.Item("cstexcform_exc_seqnbr").ToString.Trim)
                End If
            End If

            If Not IsDBNull(r.Item("cstexcform_techid_value")) Then
                If Not String.IsNullOrEmpty(r.Item("cstexcform_techid_value").ToString) Then
                    temp_sub_id = (r.Item("cstexcform_techid_value").ToString.Trim)
                End If
            End If

            If Not IsDBNull(r.Item("cstexcform_date_entered")) Then
                If Not String.IsNullOrEmpty(r.Item("cstexcform_date_entered").ToString) Then
                    Me.exec_entered_date.Text = (r.Item("cstexcform_date_entered").ToString.Trim)
                End If
            End If


            If Not IsDBNull(r.Item("cstexcform_service_change_date")) Then
                If Not String.IsNullOrEmpty(r.Item("cstexcform_service_change_date").ToString) Then
                    Me.exec_service_changed.Text = FormatDateTime((r.Item("cstexcform_service_change_date").ToString.Trim), vbShortDate)
                End If
            End If


            If Not IsDBNull(r.Item("cstexcform_exc_date")) Then
                If Not String.IsNullOrEmpty(r.Item("cstexcform_exc_date").ToString) Then
                    Me.exec_exc_date.Text = FormatDateTime((r.Item("cstexcform_exc_date").ToString.Trim), vbShortDate)
                End If
            End If


            'staff 
            'date 
            'id

            If Not IsDBNull(r.Item("cstexcform_action_name")) Then
                temp_action = r.Item("cstexcform_action_name")
            End If

        Next

        Call Fill_Dropdown1(Me.exec_action_drop, temp_action)
        Call Fill_Sub_ID(Me.exec_sub_drop, temp_sub_id)


    End Sub
    Private Sub DisplayCompanyEdit(companyInfo As DataTable)
        companyListingGrid.Visible = False
        Dim k As Integer = 0
        Dim temp_service As String = ""

        backButton.Text = "<a href='homeTables.aspx?type_of= " & type_of & "&sub_type_of=" & sub_type_of & "&comp_id=" & comp_id & "&user_id=" & user_id & "&homebase=Y'>Back</a>"
        For Each r As DataRow In companyInfo.Rows

            ''''''''This displays the edit form.
            DateLabel1.Text = "Verified Date: "
            Me.Datebox1.Text = ""
            If Not IsDBNull(r.Item("LASTVERIFIED")) Then
                If Not String.IsNullOrEmpty(r.Item("LASTVERIFIED").ToString) Then
                    Me.Datebox1.Text = (r.Item("LASTVERIFIED").ToString.Trim)
                End If
            End If

            Me.DateLabel2.Text = "Service End Date: "
            Me.Datebox2.Text = ""
            If Not IsDBNull(r.Item("ENDDATE")) Then
                If Not String.IsNullOrEmpty(r.Item("ENDDATE").ToString) Then
                    Me.Datebox2.Text = (r.Item("ENDDATE").ToString.Trim)
                End If
            End If

            ' NOTES
            Me.BottomLabel1.Text = "Notes: "
            Me.BottomText1.Text = ""
            If Not IsDBNull(r.Item("NOTES")) Then
                If Not String.IsNullOrEmpty(r.Item("NOTES").ToString) Then
                    Me.BottomText1.Text = r.Item("NOTES").ToString.Trim
                End If
            End If

            If Not IsDBNull(r.Item("SERVICENAME").ToString.Trim) Then
                temp_service = r.Item("SERVICENAME")
            End If

        Next

        Call Fill_Dropdown1(Me.Dynamic_Dropdown1, temp_service)


    End Sub
    Public Function create_company_execution_record(ByVal comp_id As Long, ByVal csu_id As Long) As String

        create_company_execution_record = ""
        Try

            TempTable = get_company_services_used(comp_id, csu_id)

            If Not IsNothing(TempTable) Then
                If TempTable.Rows.Count > 0 Then
                    If csu_id > 0 Then
                        DisplayCompanyEdit(TempTable)
                    Else 'Display Grid Listing
                        create_company_execution_record = DisplayCompanyGrid(TempTable)
                    End If

                End If
            End If

        Catch ex As Exception
            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, ex.Message)
        End Try

    End Function
    Public Function create_company_listing(ByVal comp_id As Long, ByVal csu_id As Long) As String

        'Fill Company label. same way as company details page.
        crmWebClient.CompanyFunctions.Fill_Information_Tab(Nothing, information_label, masterPage, comp_id, 0, "", New Label, New AjaxControlToolkit.TabContainer, company_address, company_name, False, False, "JETNET", 0, 0)

        create_company_listing = ""
        Try

            TempTable = get_company_services_used(comp_id, csu_id)


            If Not IsNothing(TempTable) Then
                If TempTable.Rows.Count > 0 Then
                    If csu_id > 0 Then
                        DisplayCompanyEdit(TempTable)
                    Else 'Display Grid Listing
                        create_company_listing = DisplayCompanyGrid(TempTable)
                    End If
                Else
                    create_company_listing = DisplayCompanyGrid(TempTable)
                End If
            Else
                create_company_listing = DisplayCompanyGrid(TempTable)
            End If

        Catch ex As Exception
            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, ex.Message)
        End Try

    End Function

    Function Fill_Dropdown1(ByRef dropdown1 As DropDownList, ByVal selected_value As String) As Boolean

        'Dim results_table As DataTable
        Dim field1 As String = ""
        Dim display_field As String = ""
        Dim id_field As String = ""

        Try

            If Trim(type_of) = "Company" And Trim(sub_type_of) = "ServicesUsed" Then
                TempTable = get_plain_dynamic_table_select("svud_desc", "svud_id", "services_used", "where svud_active_flag = 'Y' ", " order by svud_desc asc ")
            ElseIf Trim(type_of) = "Company" And Trim(sub_type_of) = "Customer Execution" Then
                TempTable = get_plain_dynamic_table_select("jcat_subcategory_name", "jcat_subcategory_name", "journal_Category", "where jcat_category_code = 'CS' and (jcat_subcategory_name like '%Service%' or jcat_subcategory_name like '%Contract%' or jcat_subcategory_name like '%License%') ", " order by jcat_subcategory_name ")
            End If


            If Not IsNothing(TempTable) Then

                If TempTable.Rows.Count > 0 Then

                    dropdown1.Items.Add(New ListItem("", 0))

                    For Each r As DataRow In TempTable.Rows

                        display_field = ""
                        If Not IsDBNull(r.Item("display_field")) Then
                            If Not String.IsNullOrEmpty(r.Item("display_field").ToString) Then
                                display_field = r.Item("display_field").ToString.Trim
                            End If
                        End If

                        id_field = ""
                        If Not IsDBNull(r.Item("id_field")) Then
                            If Not String.IsNullOrEmpty(r.Item("id_field").ToString) Then
                                id_field = r.Item("id_field").ToString.Trim
                            End If
                        End If

                        dropdown1.Items.Add(New ListItem(display_field, id_field))
                    Next

                End If

            End If


            If Trim(selected_value) <> "" Then
                For k = 0 To dropdown1.Items.Count - 1
                    dropdown1.SelectedIndex = k
                    If Trim(dropdown1.SelectedItem.Text) = selected_value.ToString.Trim Then
                        k = dropdown1.Items.Count
                    End If
                Next
            End If

        Catch ex As Exception
            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "** Error in homebaseOpCosts.aspx.vb :  [get_make_model_info] : " + ex.Message
        End Try

    End Function

    Function Fill_Sub_ID(ByRef dropdown1 As DropDownList, ByVal selected_value As String) As Boolean


        Dim field1 As String = ""
        Dim display_field As String = ""
        Dim id_field As String = ""

        Try

            TempTable = get_sub_id_company_list()


            If Not IsNothing(TempTable) Then

                If TempTable.Rows.Count > 0 Then

                    dropdown1.Items.Add(New ListItem("", 0))

                    For Each r As DataRow In TempTable.Rows

                        display_field = ""
                        If Not IsDBNull(r.Item("sub_id")) Then
                            If Not String.IsNullOrEmpty(r.Item("sub_id").ToString) Then
                                display_field = r.Item("sub_id").ToString.Trim
                            End If
                        End If

                        id_field = ""
                        If Not IsDBNull(r.Item("sub_id")) Then
                            If Not String.IsNullOrEmpty(r.Item("sub_id").ToString) Then
                                id_field = r.Item("sub_id").ToString.Trim
                            End If
                        End If

                        dropdown1.Items.Add(New ListItem(display_field, id_field))
                    Next

                End If

            End If


            If Trim(selected_value) <> "" Then
                For k = 0 To dropdown1.Items.Count - 1
                    dropdown1.SelectedIndex = k
                    If Trim(dropdown1.SelectedItem.Text) = selected_value.ToString.Trim Then
                        k = dropdown1.Items.Count
                    End If
                Next
            End If

        Catch ex As Exception
            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "** Error in homebaseOpCosts.aspx.vb :  [get_make_model_info] : " + ex.Message
        End Try

    End Function

    Public Shared Function get_plain_dynamic_table_select(ByVal display_field As String, ByVal id_field As String, ByVal table_name As String, ByVal where_clause As String, ByVal order_by As String) As DataTable
        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            sQuery.Append("select " & display_field & " as display_field, " & id_field & " as id_field  from " & table_name & "  with (NOLOCK) " & where_clause & " " & order_by & "  ")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_plain_dynamic_table_select(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
            SqlConn.Open()
            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 90

            SqlCommand.CommandText = sQuery.ToString
            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in get_plain_dynamic_table_select load datatable" + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in get_plain_dynamic_table_select(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable" + ex.Message

        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing

            SqlCommand.Dispose()
            SqlCommand = Nothing
        End Try

        Return atemptable

    End Function

    Public Shared Function Get_Simple_Display_Query(journID As Long, compID As Long) As DataTable
        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try
            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
            SqlConn.Open()

            sQuery.Append(" select journ_id, journ_date, journ_subject, journ_subcategory_code, journ_subcat_code_part1,journ_subcat_code_part2,journ_subcat_code_part3, journ_user_id, journ_description, contact_first_name, contact_last_name,contact_id, ")
            sQuery.Append(" ac_id, amod_make_name, amod_model_name, ac_ser_no_full, user_first_name, user_last_name   ")
            'Prefixes
            If (HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER) Then
                sQuery.Append(" from [Homebase].[jetnet_ra].[dbo].Journal with (NOLOCK) ")
                sQuery.Append(" left outer join [Homebase].[jetnet_ra].[dbo].Contact with (NOLOCK) on journ_contact_id = contact_id And contact_journ_id = 0 ")
                sQuery.Append(" left outer join [Homebase].[jetnet_ra].[dbo].Aircraft With (NOLOCK) On journ_ac_id = ac_id And ac_journ_id = 0")
                sQuery.Append(" left outer join [Homebase].[jetnet_ra].[dbo].Aircraft_Model With (NOLOCK) On amod_id = ac_amod_id ")
                sQuery.Append(" left outer join [Homebase].[jetnet_ra].[dbo].[User] with (NOLOCK) on journ_user_id = user_id ")
            Else
                'non prefixes
                sQuery.Append(" from Journal With (NOLOCK) ")
                sQuery.Append(" left outer join Contact With (NOLOCK) On journ_contact_id = contact_id And contact_journ_id = 0 ")
                sQuery.Append(" left outer join Aircraft With (NOLOCK) On journ_ac_id = ac_id And ac_journ_id = 0")
                sQuery.Append(" left outer join Aircraft_Model With (NOLOCK) On amod_id = ac_amod_id ")
                sQuery.Append(" left outer join User with (NOLOCK) on journ_user_id = user_id ")
            End If
            sQuery.Append(" where journ_comp_id = @compID ")
            sQuery.Append(" And journ_id=@journID ")




            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "homeTables.aspx.vb", sQuery.ToString)

            Dim SqlCommand As New SqlClient.SqlCommand(sQuery.ToString, SqlConn)


            SqlCommand.Parameters.AddWithValue("@compID", compID)

            SqlCommand.Parameters.AddWithValue("@journID", journID)

            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error In Get_Simple_Display_Query(journID As Long, compID As Long) As DataTable" + constrExc.Message
            End Try

            SqlCommand.Dispose()
            SqlCommand = Nothing

        Catch ex As Exception
            Return Nothing

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error In Get_Simple_Display_Query(journID As Long, compID As Long) As DataTable" + ex.Message

        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing

        End Try

        Return atemptable

    End Function




    Public Shared Function Get_Journal_Note_Type() As DataTable
        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try
            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
            SqlConn.Open()

            sQuery.Append(" select jcat_subcategory_code, jcat_subcategory_name from ")
            'Prefixes
            If (HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER) Then
                sQuery.Append(" [HomeBase].jetnet_ra.dbo.Journal_Category with (NOLOCK) ")
            Else
                'non prefixes
                sQuery.Append(" Journal_Category with (NOLOCK)  ")
            End If
            sQuery.Append(" where jcat_category_code in ('MR','CS')  ")

            sQuery.Append(" And (jcat_subcategory_name Like 'Marketing%' or jcat_subcategory_name like 'Accounting%' or jcat_subcategory_name like 'Customer%') ")




            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "homeTables.aspx.vb", sQuery.ToString)

            Dim SqlCommand As New SqlClient.SqlCommand(sQuery.ToString, SqlConn)

            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error In Get_Simple_Display_Query(journID As Long, compID As Long) As DataTable" + constrExc.Message
            End Try

            SqlCommand.Dispose()
            SqlCommand = Nothing

        Catch ex As Exception
            Return Nothing

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error In Get_Simple_Display_Query(journID As Long, compID As Long) As DataTable" + ex.Message

        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing

        End Try

        Return atemptable

    End Function
    Public Shared Function Get_Document_Display_Query(docID As Long, compID As Long) As DataTable
        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try
            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
            SqlConn.Open()


            sQuery.Append(" select company_documents.*,user_first_name, user_last_name   ")
            'Prefixes
            If (HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER) Then
                sQuery.Append(" from [Homebase].jetnet_ra.dbo.company_documents with (NOLOCK) ")
                sQuery.Append(" left outer join [Homebase].jetnet_ra.dbo.[User] on user_id = compdoc_user_id ")
            Else
                'non prefixes
                sQuery.Append(" from company_documents with (NOLOCK) ")
                sQuery.Append(" left outer join User on user_id = compdoc_user_id ")
            End If
            sQuery.Append(" where compdoc_id = @docID ")
            sQuery.Append(" and compdoc_comp_id = @compID ")




            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "homeTables.aspx.vb", sQuery.ToString)

            Dim SqlCommand As New SqlClient.SqlCommand(sQuery.ToString, SqlConn)

            SqlCommand.Parameters.AddWithValue("@docID", docID)
            SqlCommand.Parameters.AddWithValue("@compID", compID)



            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error In Get_Document_Display_Query(docID As Long, compID As Long) As DataTable" + constrExc.Message
            End Try

            SqlCommand.Dispose()
            SqlCommand = Nothing

        Catch ex As Exception
            Return Nothing

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error In Get_Simple_Display_Query(journID As Long, compID As Long) As DataTable" + ex.Message

        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing

        End Try

        Return atemptable

    End Function

    Public Shared Function Get_Support_Entry_Query(actID As Long) As DataTable
        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try
            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
            SqlConn.Open()

            sQuery.Append(" select customer_activity.*,user_first_name, user_last_name   ")
            'Prefixes
            If (HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER) Then
                sQuery.Append(" from [Homebase].customer.dbo.customer_activity with (NOLOCK) ")
                sQuery.Append(" left outer join [Homebase].jetnet_ra.dbo.[User] on user_id = cstact_init ")
            Else
                'non prefixes
                sQuery.Append(" from customer_activity with (NOLOCK)  ")
                sQuery.Append(" left outer join User on user_id = cstact_init ")
            End If
            sQuery.Append(" where cstact_id = @actID ")


            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "homeTables.aspx.vb", sQuery.ToString)

            Dim SqlCommand As New SqlClient.SqlCommand(sQuery.ToString, SqlConn)

            SqlCommand.Parameters.AddWithValue("@actID", actID)


            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error In Get_Support_Entry_Query(actID As Long) As DataTable" + constrExc.Message
            End Try

            SqlCommand.Dispose()
            SqlCommand = Nothing

        Catch ex As Exception
            Return Nothing

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error In Get_Support_Entry_Query(actID As Long) As DataTable" + ex.Message

        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing

        End Try

        Return atemptable

    End Function
    Public Function get_sub_id_company_list() As DataTable
        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            sQuery.Append("Select distinct sub_id from Subscription With (NOLOCK) where sub_comp_id = " & comp_id & " ")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_sub_id_company_list</b><br />" + sQuery.ToString

            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
            SqlConn.Open()
            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 90

            SqlCommand.CommandText = sQuery.ToString
            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error In get_plain_dynamic_table_select load datatable" + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error In get_plain_dynamic_table_select" + ex.Message

        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing

            SqlCommand.Dispose()
            SqlCommand = Nothing
        End Try

        Return atemptable

    End Function


    Public Function Insert_into_company_services_used() As String
        Insert_into_company_services_used = ""

        Dim insert_string_start As String = ""
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
            SqlConn.Open()
            SqlCommand.Connection = SqlConn
            'Change to: [Homebase].jetnet_ra.dbo.Company_Services_Used 

            If (HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER) Then
                insert_string_start = "INSERT INTO [Homebase].jetnet_ra.dbo.Company_Services_Used("
            Else
                insert_string_start = "INSERT INTO Company_Services_Used("
            End If

            insert_string_start &= " csu_comp_id, csu_user_id, csu_entered_date, csu_verified_date, csu_end_date, csu_notes, "
            insert_string_start &= " csu_journ_id, csu_svud_id  "
            insert_string_start &= ") VALUES ( "
            insert_string_start &= "" & comp_id & ", "
            insert_string_start &= "'" & Session.Item("homebaseUserClass").home_user_id & "', "
            insert_string_start &= "'" & Date.Now & "', "

            If Not String.IsNullOrEmpty(Me.Datebox1.Text) Then
                insert_string_start &= "'" & Me.Datebox1.Text & "', "
            Else
                insert_string_start &= "'" & Date.Now & "', "
            End If

            If Not String.IsNullOrEmpty(Me.Datebox2.Text) Then
                insert_string_start &= "'" & Me.Datebox2.Text & "', "
            Else
                insert_string_start &= "NULL, "
            End If

            insert_string_start &= "'" & Me.BottomText1.Text & "', "
            insert_string_start &= "0, "
            insert_string_start &= "" & Me.Dynamic_Dropdown1.SelectedItem.Value & " "
            insert_string_start &= ")"

            insert_string_start = insert_string_start

            SqlCommand.CommandText = insert_string_start
            SqlCommand.ExecuteNonQuery()
        Catch ex As Exception
        Finally
            SqlCommand.Dispose()
            SqlConn.Close()
            SqlConn.Dispose()
        End Try

        Insert_into_company_services_used = "Record Inserted"

    End Function

    Public Function Update_company_services_used() As String

        Update_company_services_used = ""

        Dim update_string_start As String = ""
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
            SqlConn.Open()
            SqlCommand.Connection = SqlConn
            'Update to [Homebase].jetnet_ra.dbo.Company_Services_Used 

            If (HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER) Then
                update_string_start = " Update [Homebase].jetnet_ra.dbo.Company_Services_Used set "
            Else
                update_string_start = " Update Company_Services_Used set "
            End If


            ' dont need to update csu_comp_id, csu_journ_id, csu_svud_id
            update_string_start &= " csu_user_id = '" & Session.Item("homebaseUserClass").home_user_id & "', "
            update_string_start &= " csu_entered_date = '" & Date.Now & "', "

            If Not String.IsNullOrEmpty(Me.Datebox1.Text) Then
                update_string_start &= " csu_verified_date = '" & Me.Datebox1.Text & "', "
            Else
                update_string_start &= " csu_verified_date = '" & Date.Now & "' , "
            End If

            If Not String.IsNullOrEmpty(Me.Datebox2.Text) Then
                update_string_start &= " csu_end_date = '" & Me.Datebox2.Text & "', "
            Else
                update_string_start &= " csu_end_date = NULL, "
            End If


            update_string_start &= " csu_notes = '" & Me.BottomText1.Text & "' "


            update_string_start &= " where csu_id = " & Trim(Request("id"))

            SqlCommand.CommandText = update_string_start
            SqlCommand.ExecuteNonQuery()
        Catch ex As Exception
        Finally
            SqlCommand.Dispose()
            SqlConn.Close()
            SqlConn.Dispose()
        End Try

        Update_company_services_used = "Record Updated"

    End Function

    Public Function delete_company_services_used() As String
        delete_company_services_used = ""


        Dim delete_string_start As String = ""
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            If Trim(Request("id")) <> "" And Trim(Request("id")) <> "0" Then
                SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
                SqlConn.Open()
                SqlCommand.Connection = SqlConn
                'Change to [Homebase].jetnet_ra.dbo.Company_Services_Used 

                If (HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER) Then
                    delete_string_start = " Delete from [Homebase].jetnet_ra.dbo.Company_Services_Used where csu_id = " & Trim(Request("id"))
                Else
                    delete_string_start = " Delete from Company_Services_Used where csu_id = " & Trim(Request("id"))
                End If

                delete_string_start = delete_string_start
                SqlCommand.CommandText = delete_string_start
                SqlCommand.ExecuteNonQuery()
            End If

        Catch ex As Exception
        Finally
            SqlCommand.Dispose()
            SqlConn.Close()
            SqlConn.Dispose()
        End Try


        delete_company_services_used = "Record Deleted"
    End Function


    Public Shared Function get_company_execution_record(ByVal comp_id As Long, ByVal exec_id As Long) As DataTable
        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try
            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
            SqlConn.Open()

            sQuery.Append(" select cstexcform_monthly_fee, comp_name, cstexcform_notes , cstexcform_id, cstexcform_action_name, ")
            sQuery.Append(" cstexcform_new_customer , cstexcform_trial, cstexcform_new_contract , cstexcform_re_connected, ")
            sQuery.Append(" cstexcform_addl_location , cstexcform_upgrade, cstexcform_downgrade, cstexcform_interrupted, cstexcform_cancellation, cstexcform_addl_license,  ")
            sQuery.Append(" cstexcform_monthly_list_fee, cstexcform_monthly_net_fee,  ")
            sQuery.Append(" cstexcform_date_entered, cstexcform_techid_value, cstexcform_exc_seqnbr , cstexcform_service_change_date, cstexcform_exc_date  ")
            sQuery.Append(" , sub_service_name, cstexcform_service ")
            sQuery.Append("  from [Homebase].[customer].[dbo].[Customer_Execution] with (NOLOCK) ")
            sQuery.Append(" inner join Subscription with (NOLOCK) on sub_id = cstexcform_techid_value ")
            sQuery.Append("  inner join company with (NOLOCK) on comp_id = sub_comp_id and comp_journ_id = 0  ")
            sQuery.Append(" where comp_id = @comp_id ")

            If exec_id > 0 Then
                sQuery.Append(" and cstexcform_id = @csu_id ")
            End If

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "homeTables.aspx.vb", sQuery.ToString)

            Dim SqlCommand As New SqlClient.SqlCommand(sQuery.ToString, SqlConn)


            SqlCommand.Parameters.AddWithValue("@comp_id", comp_id)

            If exec_id > 0 Then
                SqlCommand.Parameters.AddWithValue("@csu_id", exec_id)
            End If

            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in get_company_execution_record load datatable" + constrExc.Message
            End Try

            SqlCommand.Dispose()
            SqlCommand = Nothing

        Catch ex As Exception
            Return Nothing

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in get_company_execution_record(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable" + ex.Message

        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing

        End Try

        Return atemptable

    End Function

    Public Shared Function get_company_services_used(ByVal comp_id As Long, ByVal csu_id As Long) As DataTable
        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try
            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
            SqlConn.Open()

            sQuery.Append(" select svud_desc as SERVICENAME, csu_end_date as ENDDATE, csu_verified_date ")
            sQuery.Append(" as LASTVERIFIED, csu_notes as NOTES,  csu_id ")


            If (HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER) Then
                sQuery.Append(" from [Homebase].jetnet_ra.dbo.Company_Services_Used with (NOLOCK) inner join [Homebase].jetnet_ra.dbo.Services_Used with (NOLOCK) on svud_id = csu_svud_id  ")
            Else
                sQuery.Append(" from Company_Services_Used with (NOLOCK) inner join Services_Used with (NOLOCK) on svud_id = csu_svud_id  ")
            End If


            sQuery.Append(" where  csu_comp_id = @csu_comp_id ")

            If csu_id > 0 Then
                sQuery.Append(" and csu_id = @csu_id ")
            End If


            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "homeTables.aspx.vb", sQuery.ToString)

            Dim SqlCommand As New SqlClient.SqlCommand(sQuery.ToString, SqlConn)


            SqlCommand.Parameters.AddWithValue("@csu_comp_id", comp_id)

            If csu_id > 0 Then
                SqlCommand.Parameters.AddWithValue("@csu_id", csu_id)
            End If

            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in get_company_services_used load datatable" + constrExc.Message
            End Try

            SqlCommand.Dispose()
            SqlCommand = Nothing

        Catch ex As Exception
            Return Nothing

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in get_company_services_used(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable" + ex.Message

        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing

        End Try

        Return atemptable

    End Function

    Public Function Insert_into_company_execution() As String
        Insert_into_company_execution = ""

        Dim insert_string_start As String = ""
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand

        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
            SqlConn.Open()
            SqlCommand.Connection = SqlConn

            ' cstexcform_new_customer, cstexcform_trial, cstexcform_new_contract, cstexcform_re_connected,
            'cstexcform_addl_location, cstexcform_upgrade, cstexcform_downgrade, cstexcform_interrupted, cstexcform_cancellation, cstexcform_addl_license,
            ' cstexcform_monthly_list_fee, cstexcform_monthly_net_fee,
            ' cstexcform_date_entered, cstexcform_techid_value, cstexcform_exc_seqnbr, cstexcform_service_change_date 



            insert_string_start = "INSERT INTO [customer].[dbo].[Customer_Execution]("
            insert_string_start &= " cstexcform_monthly_fee, cstexcform_monthly_list_fee, cstexcform_monthly_net_fee, cstexcform_notes, cstexcform_action_name  "
            insert_string_start &= ", cstexcform_new_customer, cstexcform_trial, cstexcform_new_contract, cstexcform_re_connected "
            insert_string_start &= ", cstexcform_addl_location, cstexcform_upgrade, cstexcform_downgrade, cstexcform_interrupted, cstexcform_cancellation, cstexcform_addl_license "
            insert_string_start &= ",  cstexcform_monthly_list_fee, cstexcform_monthly_net_fee "
            insert_string_start &= ",  cstexcform_date_entered, cstexcform_techid_value, cstexcform_exc_seqnbr, cstexcform_service_change_date "

            insert_string_start &= ") VALUES ( "
            insert_string_start &= "" & Me.exec_monthly_price.Text & ", "
            insert_string_start &= "'" & Me.exec_list_fee.Text & "', "
            insert_string_start &= "'" & Me.exec_monthly_net.Text & "', "
            insert_string_start &= "'" & Me.exec_notes.Text & "', "
            insert_string_start &= "'" & Me.exec_action_drop.Text & "',  "

            If Me.exec_new_customer.Checked = True Then
                insert_string_start &= "'Y',  "   ' cstexcform_new_customer
            Else
                insert_string_start &= "'N',  "   ' cstexcform_new_customer
            End If

            If Me.exec_trial.Checked = True Then
                insert_string_start &= "'Y',  "   ' cstexcform_trial
            Else
                insert_string_start &= "'N',  "   ' cstexcform_trial
            End If


            If Me.exec_new_contract.Checked = True Then
                insert_string_start &= "'Y',  "   ' cstexcform_new_contract
            Else
                insert_string_start &= "'N',  "   ' cstexcform_new_contract
            End If


            If Me.exec_re_connected.Checked = True Then
                insert_string_start &= "'Y',  "   ' cstexcform_re_connected
            Else
                insert_string_start &= "'N',  "   ' cstexcform_re_connected
            End If


            If Me.exec_addl_location.Checked = True Then
                insert_string_start &= "'Y',  "   ' cstexcform_addl_location
            Else
                insert_string_start &= "'N',  "   ' cstexcform_addl_location
            End If


            If Me.exec_upgrade.Checked = True Then
                insert_string_start &= "'Y',  "   ' cstexcform_upgrade
            Else
                insert_string_start &= "'N',  "   ' cstexcform_upgrade
            End If


            If Me.exec_downgrade.Checked = True Then
                insert_string_start &= "'Y',  "   ' cstexcform_downgrade
            Else
                insert_string_start &= "'N',  "   ' cstexcform_downgrade
            End If


            If Me.exec_interrupted.Checked = True Then
                insert_string_start &= "'Y',  "   ' cstexcform_interrupted
            Else
                insert_string_start &= "'N',  "   ' cstexcform_interrupted
            End If

            If Me.exec_cancellation.Checked = True Then
                insert_string_start &= "'Y',  "   ' cstexcform_cancellation
            Else
                insert_string_start &= "'N',  "   ' cstexcform_cancellation
            End If

            If Me.exec_addl_license.Checked = True Then
                insert_string_start &= "'Y',  "   ' cstexcform_addl_license
            Else
                insert_string_start &= "'N',  "   ' cstexcform_addl_license
            End If



            insert_string_start &= "'" & Me.exec_list_fee.Text & "' , "   ' cstexcform_monthly_list_fee
            insert_string_start &= "'" & Me.exec_monthly_net.Text & "',  "   ' cstexcform_monthly_net_fee

            insert_string_start &= "'" & Me.exec_entered_date.Text & "',  "   ' cstexcform_date_entered
            insert_string_start &= "'" & Me.exec_sub_drop.Text & "',  "   ' cstexcform_techid_value
            insert_string_start &= "'" & Me.exec_seq.Text & "',  "   ' cstexcform_exc_seqnbr
            insert_string_start &= "'" & Me.exec_service_changed.Text & "'  "   ' cstexcform_service_change_date



            insert_string_start &= ")"

            insert_string_start = insert_string_start
            results_label.Text = ("<br/><br/><br>" & insert_string_start)

            SqlCommand.CommandText = insert_string_start
            'SqlCommand.ExecuteNonQuery()
        Catch ex As Exception
        Finally
            SqlCommand.Dispose()
            SqlConn.Close()
            SqlConn.Dispose()
        End Try

        Insert_into_company_execution = "Record Inserted"

    End Function
    Public Function Update_company_execution() As String

        Update_company_execution = ""

        Dim update_string_start As String = ""
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
            SqlConn.Open()
            SqlCommand.Connection = SqlConn

            update_string_start = " Update [customer].[dbo].[Customer_Execution] set "
            ' dont need to update csu_comp_id, csu_journ_id, csu_svud_id
            update_string_start &= " cstexcform_monthly_fee = '" & Me.exec_monthly_price.Text & "', "
            update_string_start &= " cstexcform_monthly_list_fee = '" & Me.exec_list_fee.Text & "', "
            update_string_start &= " cstexcform_monthly_net_fee = '" & Me.exec_monthly_net.Text & "', "
            update_string_start &= " cstexcform_notes = '" & Replace(Me.exec_notes.Text, "'", "''") & "', "
            update_string_start &= " cstexcform_action_name = '" & Replace(Me.exec_action_drop.Text, "'", "''") & "', "


            update_string_start &= " cstexcform_new_customer = "

            If Me.exec_new_customer.Checked = True Then
                update_string_start &= " 'Y',  "   ' cstexcform_new_customer
            Else
                update_string_start &= " 'N',  "   ' cstexcform_new_customer
            End If

            update_string_start &= " cstexcform_trial = "

            If Me.exec_trial.Checked = True Then
                update_string_start &= " 'Y',  "
            Else
                update_string_start &= " 'N',  "
            End If


            update_string_start &= " cstexcform_new_contract = "

            If Me.exec_new_contract.Checked = True Then
                update_string_start &= " 'Y',  "
            Else
                update_string_start &= " 'N',  "
            End If


            update_string_start &= " cstexcform_re_connected = "

            If Me.exec_re_connected.Checked = True Then
                update_string_start &= " 'Y',  "
            Else
                update_string_start &= " 'N',  "
            End If

            update_string_start &= " cstexcform_addl_location = "

            If Me.exec_addl_location.Checked = True Then
                update_string_start &= " 'Y',  "
            Else
                update_string_start &= " 'N',  "
            End If


            update_string_start &= " cstexcform_upgrade = "

            If Me.exec_upgrade.Checked = True Then
                update_string_start &= " 'Y',  "
            Else
                update_string_start &= " 'N',  "
            End If


            update_string_start &= " cstexcform_downgrade = "

            If Me.exec_downgrade.Checked = True Then
                update_string_start &= " 'Y',  "
            Else
                update_string_start &= " 'N',  "
            End If

            update_string_start &= " cstexcform_interrupted = "

            If Me.exec_interrupted.Checked = True Then
                update_string_start &= " 'Y',  "
            Else
                update_string_start &= " 'N',  "
            End If

            update_string_start &= " cstexcform_cancellation = "

            If Me.exec_cancellation.Checked = True Then
                update_string_start &= " 'Y',  "
            Else
                update_string_start &= " 'N',  "
            End If

            update_string_start &= " cstexcform_addl_license = "

            If Me.exec_addl_license.Checked = True Then
                update_string_start &= " 'Y',  "
            Else
                update_string_start &= " 'N',  "
            End If

            '  update_string_start &= " cstexcform_date_entered = '" & Me.exec_entered_date.Text & "', "
            update_string_start &= " cstexcform_techid_value = '" & Me.exec_sub_drop.Text & "', "
            update_string_start &= " cstexcform_exc_seqnbr = '" & Me.exec_seq.Text & "', "
            update_string_start &= " cstexcform_service_change_date = '" & Me.exec_service_changed.Text & "'  "



            update_string_start &= " where cstexcform_id = " & Trim(Request("activityid"))

            results_label.Text = ("<br/><br/><br>" & update_string_start)

            SqlCommand.CommandText = update_string_start
            '  SqlCommand.ExecuteNonQuery()
        Catch ex As Exception
        Finally
            SqlCommand.Dispose()
            SqlConn.Close()
            SqlConn.Dispose()
        End Try

        Update_company_execution = "Record Updated"

    End Function
    Public Function delete_company_execution() As String
        delete_company_execution = ""


        Dim delete_string_start As String = ""
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            If Trim(Request("id")) <> "" And Trim(Request("id")) <> "0" Then
                SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
                SqlConn.Open()
                SqlCommand.Connection = SqlConn

                delete_string_start = " Delete from [customer].[dbo].[Customer_Execution]  where cstexcform_id = " & Trim(Request("activityid"))


                results_label.Text = ("<br/><br/><br>" & delete_string_start)
                SqlCommand.CommandText = delete_string_start
                '  SqlCommand.ExecuteNonQuery()
            End If

        Catch ex As Exception
        Finally
            SqlCommand.Dispose()
            SqlConn.Close()
            SqlConn.Dispose()
        End Try


        delete_company_execution = "Record Deleted"
    End Function

    'Public Sub note_type_click()


    '    If Trim(journ_note_type.Text) <> "" Then
    '        journ_subject.Text &= journ_note_type.SelectedItem.Text
    '    Else
    '        journ_subject.Text = journ_note_type.SelectedItem.Text
    '    End If


    'End Sub




    Private Sub removeMarketingNote_Click(sender As Object, e As EventArgs) Handles removeMarketingNote.Click
        Dim Action As New journalClass
        If activityid > 0 And comp_id > 0 Then
            Try
                Action.journ_id = activityid
                Action.journ_comp_id = comp_id
                Action.deleteJournalRecord(True)

                attentionJournal.Text = "<p align=""center"">This item has been removed.</p>"


                Dim javastr As String = ""
                javastr = " if ((typeof (window.opener) != ""undefined"") && (window.opener != null)) { " & vbNewLine
                javastr += "  try { // call the fnRefreshPage on the parent window " & vbNewLine
                javastr += "   window.opener.fnRefreshPage(); " & vbNewLine
                javastr += "   self.close(); " & vbNewLine 'Only way the page is closed is if the opener refreshes. Otherwise, the page stays open and delivers item has been removed message.
                javastr += "  } " & vbNewLine
                javastr += "  catch (err) { // if that fails then we are going to let the page gracefully not do anything. " & vbNewLine
                javastr += "  } " & vbNewLine
                javastr += "}" & vbNewLine
                javastr += "else {" & vbNewLine
                javastr += " //let the page gracefully not do anything." & vbNewLine
                javastr += "}" & vbNewLine

                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window_remove", javastr, True)


            Catch ex As Exception
                attentionJournal.Text = "<p align=""center"">There was an issue removing this item.</p>"
            End Try
        Else
            attentionJournal.Text = "<p align=""center"">There was an issue removing this item.</p>"
        End If
    End Sub
    Public Sub insert_research_note_services_used()
        Dim Action As New journalClass
        If Trim(Request("id")) <> "" And Trim(Request("id")) <> "0" Then
            If comp_id > 0 Then
                Try
                    Action.journ_id = activityid
                    Action.journ_comp_id = comp_id

                    Action.journ_subcategory_code = "RN"
                    Action.journ_subcat_code_part1 = ""
                    Action.journ_subcat_code_part2 = ""
                    Action.journ_subcat_code_part3 = ""
                    Action.journ_contact_id = 0

                    If Not String.IsNullOrEmpty(journ_user.SelectedValue.Trim) Then
                        Action.journ_user_id = journ_user.SelectedValue.Trim
                    End If

                    Action.journ_subject = "Removed Services Used"
                    If Trim(Request("service")) <> "" Then
                        Action.journ_description = "Removed Services Used, Type: " & Trim(Request("service"))
                    Else
                        Action.journ_description = "Removed Services Used"
                    End If


                    Action.journ_status = "A"
                    Action.journ_entry_date = FormatDateTime(Date.Now(), DateFormat.ShortDate)
                    Action.journ_entry_time = Now.ToLongTimeString

                    Action.journ_date = Action.journ_entry_date + " " + Action.journ_entry_time

                    Action.journ_action_date = Now.ToString
                    Action.journ_account_id = Session.Item("homebaseUserClass").home_account_id

                    ' Action.updateJournalRecord(True)

                    Action.insertJournalRecord(True)

                    attentionJournal.Text = "<p align=""center"">This item has been updated.</p>"
                Catch ex As Exception
                    attentionJournal.Text = "<p align=""center"">There was an issue updating this item.</p>"
                End Try
            Else
                attentionJournal.Text = "<p align=""center"">There was an issue updating this item.</p>"
            End If
        End If
    End Sub


    Private Sub saveMarketingNote_Click(sender As Object, e As EventArgs) Handles saveMarketingNote.Click
        Dim Action As New journalClass
        If activityid > 0 And comp_id > 0 Then
            Try
                Action.journ_id = activityid
                Action.journ_comp_id = comp_id

                Action.journ_subcategory_code = journ_note_type.SelectedValue.Trim
                Action.journ_subcat_code_part1 = Left(Action.journ_subcategory_code, 2)
                Action.journ_subcat_code_part2 = Mid(Action.journ_subcategory_code, 3, 2)
                Action.journ_subcat_code_part3 = Mid(Action.journ_subcategory_code, 5, 2)

                If journ_contact.SelectedValue > 0 Then
                    Action.journ_contact_id = journ_contact.SelectedValue
                End If

                If Not String.IsNullOrEmpty(journ_user.SelectedValue.Trim) Then
                    Action.journ_user_id = journ_user.SelectedValue.Trim
                End If
                Action.journ_subject = journ_subject.Text
                Action.journ_description = Left(Server.HtmlDecode(journ_description.Text), 4000)
                journ_description.Text = Action.journ_description  ' was double replacing MSW  - 3/6/20
                Action.journ_status = "A"
                Action.journ_entry_date = FormatDateTime(journ_date.Text.Trim, DateFormat.ShortDate)
                Action.journ_entry_time = Now.ToLongTimeString

                Action.journ_date = Action.journ_entry_date + " " + Action.journ_entry_time

                Action.journ_action_date = Now.ToString
                Action.journ_account_id = Session.Item("homebaseUserClass").home_account_id



                Action.updateJournalRecord(True)

                attentionJournal.Text = "<p align=""center"">This item has been updated.</p>"

                Dim javastr As String = ""
                javastr = " if ((typeof (window.opener) != ""undefined"") && (window.opener != null)) { " & vbNewLine
                javastr += "  try { // call the fnRefreshPage on the parent window " & vbNewLine
                javastr += "   window.opener.fnRefreshPage(); " & vbNewLine
                javastr += "   self.close(); " & vbNewLine 'Only way the page is closed is if the opener refreshes. Otherwise, the page stays open and delivers item has been removed message.
                javastr += "  } " & vbNewLine
                javastr += "  catch (err) { // if that fails then we are going to let the page gracefully not do anything. " & vbNewLine
                javastr += "  } " & vbNewLine
                javastr += "}" & vbNewLine
                javastr += "else {" & vbNewLine
                javastr += " //let the page gracefully not do anything." & vbNewLine
                javastr += "}" & vbNewLine

                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window_update", javastr, True)

            Catch ex As Exception
                attentionJournal.Text = "<p align=""center"">There was an issue updating this item.</p>"
            End Try
        Else
            attentionJournal.Text = "<p align=""center"">There was an issue updating this item.</p>"
        End If
    End Sub

    Private Sub addMarketingNote_Click(sender As Object, e As EventArgs) Handles addMarketingNote.Click
        Dim Action As New journalClass
        Try
            Action.journ_comp_id = comp_id

            Action.journ_subcategory_code = journ_note_type.SelectedValue.Trim
            Action.journ_subcat_code_part1 = Left(Action.journ_subcategory_code, 2)
            Action.journ_subcat_code_part2 = Mid(Action.journ_subcategory_code, 3, 2)
            Action.journ_subcat_code_part3 = Mid(Action.journ_subcategory_code, 5, 2)

            If journ_contact.SelectedValue > 0 Then
                Action.journ_contact_id = journ_contact.SelectedValue
            End If

            If Not String.IsNullOrEmpty(journ_user.SelectedValue.Trim) Then
                Action.journ_user_id = journ_user.SelectedValue.Trim
            End If
            Action.journ_subject = Replace(journ_subject.Text, "'", "''")
            Action.journ_description = Left(Server.HtmlDecode(journ_description.Text), 4000)
            journ_description.Text = Action.journ_description   ' was double replacing
            Action.journ_status = "A"
            Action.journ_entry_date = FormatDateTime(journ_date.Text.Trim, DateFormat.ShortDate)
            Action.journ_entry_time = Now.ToLongTimeString


            Action.journ_date = Action.journ_entry_date + " " + Action.journ_entry_time

            Action.journ_action_date = Now.ToString
            Action.journ_account_id = Session.Item("homebaseUserClass").home_account_id

            attentionJournal.Text = "<p align=""center"">This item has been added.</p>"

            Action.insertJournalRecord(True)


            Dim javastr As String = ""
            javastr = " if ((typeof (window.opener) != ""undefined"") && (window.opener != null)) { " & vbNewLine
            javastr += "  try { // call the fnRefreshPage on the parent window " & vbNewLine
            javastr += "   window.opener.fnRefreshPage(); " & vbNewLine
            javastr += "   self.close(); " & vbNewLine 'Only way the page is closed is if the opener refreshes. Otherwise, the page stays open and delivers item has been removed message.
            javastr += "  } " & vbNewLine
            javastr += "  catch (err) { // if that fails then we are going to let the page gracefully not do anything. " & vbNewLine
            javastr += "  } " & vbNewLine
            javastr += "}" & vbNewLine
            javastr += "else {" & vbNewLine
            javastr += " //let the page gracefully not do anything." & vbNewLine
            javastr += "}" & vbNewLine

            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window_add", javastr, True)

        Catch ex As Exception
            attentionJournal.Text = "<p align=""center"">There was an issue updating this item.</p>"
        End Try
    End Sub

    Private Sub editMarketingSummaryNote_Click(sender As Object, e As EventArgs) Handles editMarketingSummaryNote.Click
        Edit_CompanyMarketingNotes(Replace(Server.HtmlDecode(marketingNote.Text), "'", "''"), comp_id)

        editMarketingSummaryNoteLabel.Visible = True
        editMarketingSummaryNoteLabel.Text = "<p align='center'>The summary has been edited.</p>"



        Dim javastr As String = ""
        javastr = " if ((typeof (window.opener) != ""undefined"") && (window.opener != null)) { " & vbNewLine
        javastr += "  try { // call the fnRefreshPage on the parent window " & vbNewLine
        javastr += "   window.opener.fnRefreshPage(); " & vbNewLine
        javastr += "   self.close(); " & vbNewLine 'Only way the page is closed is if the opener refreshes. Otherwise, the page stays open and delivers a Summary has been edited message.
        javastr += "  } " & vbNewLine
        javastr += "  catch (err) { // if that fails then we are going to let the page gracefully not do anything. " & vbNewLine
        javastr += "  } " & vbNewLine
        javastr += "}" & vbNewLine
        javastr += "else {" & vbNewLine
        javastr += " //let the page gracefully not do anything." & vbNewLine
        javastr += "}" & vbNewLine

        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", javastr, True)
    End Sub
End Class