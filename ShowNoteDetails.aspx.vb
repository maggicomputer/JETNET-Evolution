
' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/ShowNoteDetails.aspx.vb $
'$$Author: Amanda $
'$$Date: 6/26/19 9:02a $
'$$Modtime: 6/26/19 9:05a $
'$$Revision: 3 $
'$$Workfile: ShowNoteDetails.aspx.vb $
'
' ********************************************************************************

Partial Public Class ShowNoteDetails
    Inherits System.Web.UI.Page
    Dim CompanyID As Long = 0
    Dim AircraftID As Long = 0
    Dim JournalID As Long = 0
    Dim CrmSource As String = "JETNET"
    Dim CRMView As Boolean = False
    Dim DisplayCompany As Boolean = False
    Dim DisplayAircraft As Boolean = True
    Dim DisplayYacht As Boolean = False
    Dim YachtID As Long = 0
    Dim aclsData_Temp As New clsData_Manager_SQL
    Dim PageTitle As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session.Item("crmUserLogon") <> True Then

            Response.Redirect("Default.aspx", False)

        Else
            aclsData_Temp = New clsData_Manager_SQL
            aclsData_Temp.JETNET_DB = Session.Item("jetnetClientDatabase")
            aclsData_Temp.client_DB = Session.Item("jetnetServerNotesDatabase")


            'what Aircraft is it attached to?
            If Not IsNothing(Request.Item("acid")) Then
                If Not String.IsNullOrEmpty(Request.Item("acid").ToString) Then
                    If IsNumeric(Request.Item("acid").Trim) Then
                        AircraftID = Request.Item("acid").Trim
                    End If
                End If
            End If

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
                        End If
                    End If
                End If
            End If


            If clsGeneral.clsGeneral.isCrmDisplayMode() Then
                If Not IsNothing(Trim(HttpContext.Current.Request("source"))) Then
                    If Not String.IsNullOrEmpty(HttpContext.Current.Request("source")) Then
                        CrmSource = Trim(HttpContext.Current.Request("source"))
                    End If
                End If
                CRMView = True
            End If




            FillDropDownLists()
            SetUpLeftColumnInformation()
            DisplayNotesList()
            Master.SetPageTitle(PageTitle & " Detailed Notes")
        End If

    End Sub


    Private Sub FillDropDownLists()
        If Not Page.IsPostBack Then
            If CRMView Then
                Dim crmTestMaster As New main_site
                crmTestMaster.aclsData_Temp.JETNET_DB = Session.Item("jetnetClientDatabase")
                crmTestMaster.aclsData_Temp.client_DB = Session.Item("jetnetServerNotesDatabase")

                clsGeneral.clsGeneral.FillCRMUserOnEvol(crmTestMaster, "Notes", noteStaff, False)
                clsGeneral.clsGeneral.Fill_Note_Category(noteCategory, "N", Nothing, crmTestMaster)
                noteStaff.SelectedValue = "0"
            Else
                noteCategory.Items.Add(New ListItem("All", "0"))
                noteStaff.Items.Add(New ListItem("All", "0"))
            End If
        End If
    End Sub
    Private Sub DisplayNotesList()
        Dim HoldingTable As New DataTable

        If HttpContext.Current.Session.Item("localSubscription").crmServerSideNotes_Flag = True Then
            crmToggleOn.Visible = True
            If CRMView Then
                If DisplayAircraft Then
                    If AircraftID > 0 Then
                        HoldingTable = aclsData_Temp.Dual_NotesOnlyOne(IIf(CrmSource = "CLIENT", AircraftID, 0), IIf(CrmSource = "CLIENT", 0, AircraftID), "", DisplayAircraft, DisplayCompany)
                    ElseIf CompanyID > 0 Then
                        HoldingTable = aclsData_Temp.Dual_NotesOnlyOne(IIf(CrmSource = "CLIENT", CompanyID, 0), IIf(CrmSource = "CLIENT", 0, CompanyID), "", False, True)
                    End If
                Else
                    If CompanyID > 0 Then
                        HoldingTable = aclsData_Temp.Dual_NotesOnlyOne(IIf(CrmSource = "CLIENT", CompanyID, 0), IIf(CrmSource = "CLIENT", 0, CompanyID), "", DisplayAircraft, DisplayCompany)
                    End If
                End If
            Else
                If DisplayAircraft Then
                    If AircraftID > 0 Then
                        HoldingTable = aclsData_Temp.Dual_NotesOnlyOne(0, AircraftID, "", DisplayAircraft, DisplayCompany)
                    ElseIf CompanyID > 0 Then ' added IN MSW - was a missing spot, booleans like  DisplayAircraft  and DisplayCompany seem like they are not being set at all
                        HoldingTable = aclsData_Temp.Dual_NotesOnlyOne(0, CompanyID, "", False, True) ' neeed to use company id 
                    End If
                Else
                    If CompanyID > 0 Then
                        HoldingTable = aclsData_Temp.Dual_NotesOnlyOne(0, CompanyID, "", DisplayAircraft, DisplayCompany)
                    End If
                End If
            End If
        ElseIf HttpContext.Current.Session.Item("localSubscription").crmCloudNotes_Flag = True Then
            crmToggleOn.Visible = False
            If DisplayAircraft Then
                If AircraftID > 0 Then
                    HoldingTable = aclsData_Temp.CloudNotesDetailsNoteListingQuery(AircraftID, "", DisplayAircraft, DisplayCompany, DisplayYacht, False)
                End If
            ElseIf DisplayYacht Then
                If YachtID > 0 Then
                    HoldingTable = aclsData_Temp.CloudNotesDetailsNoteListingQuery(YachtID, "", DisplayAircraft, DisplayCompany, DisplayYacht, False)
                End If
            Else
                If CompanyID > 0 Then
                    HoldingTable = aclsData_Temp.CloudNotesDetailsNoteListingQuery(CompanyID, "", DisplayAircraft, DisplayCompany, DisplayYacht, False)
                End If
            End If
        End If

        'Clone the tables before filtering to get the schemas.
        If Not IsNothing(HoldingTable) Then
            Dim NotesTable As New DataTable
            NotesTable = HoldingTable.Clone


            Dim FilterView As New DataView
            Dim displayTable As New DataTable
            FilterView = HoldingTable.DefaultView
            FilterView.RowFilter = "lnote_status = 'A'" & IIf(noteStaff.SelectedValue > 0, " and lnote_user_id = " & noteStaff.SelectedValue.ToString, "") & IIf(noteCategory.SelectedValue > 0, " and lnote_notecat_key = " & noteCategory.SelectedValue.ToString, "")

            NotesTable = FilterView.ToTable()

            notesDataLiteral.Text = DisplayNotes(NotesTable)

            BuildTableJS()
        End If



    End Sub
    Private Sub BuildTableJS()
        Dim tableBuild As New StringBuilder
        tableBuild.Append("var hideFromExport = [];var table = $('.dataTableConvert').DataTable({destroy:true,")
        tableBuild.Append("dom:        'Bfitrp',")
        'tableBuild.Append("scrollY:        cw + 'px', ")
        'tableBuild.Append("scrollX:        '978px', ")

        tableBuild.Append("scrollCollapse: true, ")
        tableBuild.Append("scroller:       false")

        tableBuild.Append("});")

        System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StartupScr", "$(function() {" & tableBuild.ToString & "});", True)
    End Sub
    Private Function DisplayNotes(ByVal tempData As DataTable) As String
        Dim htmlOut As New StringBuilder
        If Not IsNothing(tempData) Then
            If CRMView Then
                htmlOut.Append("<p align='right'>+ " & DisplayFunctions.CRM_WriteNotesRemindersLinks(0, AircraftID, CompanyID, 0, True, "&action=new&type=note&notesViewAll=show&source=" & CrmSource & IIf(CompanyID > 0, "&Listing=1&from=companydetails", "&Listing=3&from=aircraftdetails"), "Add New Note") & "</p>")
            End If
            If tempData.Rows.Count > 0 Then

                htmlOut.Append("<table width=""100%"" border=""0"" cellpadding=""0"" class=""dataTableConvert"">")
                htmlOut.Append("<thead><tr><th></th><th>Entry Date</th><th>Action Date</th>")

                If HttpContext.Current.Session.Item("localSubscription").crmServerSideNotes_Flag = True Then
                    htmlOut.Append("<th>Category</th>")
                End If

                htmlOut.Append("<th>Staff</th><th>Note</th>")
                'If AircraftID = 0 The
                htmlOut.Append("<th>Aircraft</th>")

                'End If
                If CompanyID = 0 And HttpContext.Current.Session.Item("localSubscription").crmServerSideNotes_Flag = True Then
                    htmlOut.Append("<th>Company</th>")
                End If

                If HttpContext.Current.Session.Item("localSubscription").crmServerSideNotes_Flag = True Then
                    htmlOut.Append("<th>Contact</th>")
                End If


                htmlOut.Append("</thead>")
                htmlOut.Append("<tbody>")
                For Each r As DataRow In tempData.Rows
                    htmlOut.Append("<tr>")
                    Dim noteLink As String = ""

                    If CRMView Then
                        noteLink = DisplayFunctions.CRM_WriteNotesRemindersLinks(r("lnote_id"), r("lnote_jetnet_ac_id"), r("lnote_jetnet_comp_id"), 0, True, "&action=edit&type=note&cat_key=0&source=JETNET&notesViewAll=show" & IIf(CompanyID > 0, "&Listing=1&from=companyDetails", "&Listing=3&from=aircraftdetails"), "<img src=""/images/edit_icon.png"" alt=""Edit"" class=""cursor"" />")
                    Else
                        ' noteLink = DisplayFunctions.WriteNotesRemindersLinks(r("lnote_id"), IIf(r("lnote_jetnet_ac_id") > 0, r("lnote_jetnet_ac_id"), r("lnote_client_ac_id")), IIf(r("lnote_jetnet_comp_id") > 0, r("lnote_jetnet_comp_id"), r("lnote_client_comp_id")), 0, True, "&from=NoteView", "<img src=""/images/edit_icon.png"" alt=""Edit"" class=""cursor"" />")
                    End If
                    htmlOut.Append("<td valign=""top"" align=""left"" width=""18"">" & noteLink & "</td>")
                    htmlOut.Append("<td valign=""top"" align=""left"" width=""80"">")
                    If Not IsNothing(r("lnote_entry_date")) Then
                        htmlOut.Append(clsGeneral.clsGeneral.TwoPlaceYear(r("lnote_entry_date")))
                    End If
                    htmlOut.Append("</td>")
                    htmlOut.Append("<td valign=""top"" align=""left"" width=""80"">")
                    If Not IsNothing(r("lnote_action_date")) Then
                        htmlOut.Append(clsGeneral.clsGeneral.TwoPlaceYear(r("lnote_action_date")))
                    End If
                    htmlOut.Append("</td>")

                    If Not IsNothing(r("lnote_notecat_key")) Then
                        If HttpContext.Current.Session.Item("localSubscription").crmServerSideNotes_Flag = True Then
                            htmlOut.Append("<td valign=""top"" align=""left"" width=""100"">")
                            Dim CatTable As DataTable = ClientCategory(r("lnote_notecat_key"))

                            If Not IsNothing(CatTable) Then
                                If CatTable.Rows.Count > 0 Then
                                    htmlOut.Append(CatTable.Rows(0).Item("notecat_name"))
                                End If
                            End If
                            htmlOut.Append("</td>")
                        End If
                    End If

                    htmlOut.Append("<td valign=""top"" align=""left"">")
                    If HttpContext.Current.Session.Item("localSubscription").crmServerSideNotes_Flag = True Then
                        If Not IsDBNull(r("lnote_user_id")) Then
                            Dim UserTable As DataTable = aclsData_Temp.Get_Client_User(r("lnote_user_id"))

                            If Not IsNothing(UserTable) Then
                                If UserTable.Rows.Count > 0 Then
                                    htmlOut.Append(UserTable.Rows(0).Item("cliuser_first_name") & " " & UserTable.Rows(0).Item("cliuser_last_name") & " ")
                                End If
                            End If
                        Else
                            htmlOut.Append(r("lnote_user_id"))
                        End If
                    End If
                    htmlOut.Append("</td>")
                    htmlOut.Append("<td width=""250"" valign=""top"" align=""left"">")
                    If Not IsDBNull(r("lnote_note")) Then
                        htmlOut.Append(r("lnote_note"))
                    End If
                    htmlOut.Append("</td>")

                    If CRMView Then
                        ' If AircraftID = 0 Then
                        htmlOut.Append("<td valign=""top"" align=""left"">")
                        If r("lnote_jetnet_ac_id") > 0 Or r("lnote_client_ac_id") > 0 Then
                            htmlOut.Append(DisplayAircraftInfo(r("lnote_jetnet_ac_id"), r("lnote_client_ac_id")))
                        End If
                        htmlOut.Append("</td>")
                        'End If
                        If CompanyID = 0 Then
                            htmlOut.Append("<td valign=""top"" align=""left"">")
                            If r("lnote_jetnet_comp_id") > 0 Or r("lnote_client_comp_id") > 0 Then
                                htmlOut.Append(DisplayCompanyInfo(r("lnote_jetnet_comp_id"), r("lnote_client_comp_id"), False))
                            End If
                            htmlOut.Append("</td>")
                        End If

                        htmlOut.Append("<td valign=""top"" align=""left"">")
                        If r("lnote_jetnet_contact_id") > 0 Or r("lnote_client_contact_id") > 0 Then
                            htmlOut.Append(DisplayContactInfo(r("lnote_jetnet_contact_id"), r("lnote_client_contact_id")))
                        End If
                        htmlOut.Append("</td>")

                    Else
                        'If AircraftID = 0 Then
                        htmlOut.Append("<td valign=""top"" align=""left"">")
                        htmlOut.Append(DisplayAircraftInfo(r("lnote_jetnet_ac_id"), 0))
                        htmlOut.Append("</td>")
                        'End If
                        'If CompanyID = 0 Then
                        '  htmlOut.Append("<td valign=""top"" align=""left"">")
                        '  htmlOut.Append(DisplayCompanyInfo(r("lnote_jetnet_comp_id"), 0, False))
                        '  htmlOut.Append("</td>")
                        'End If

                        'htmlOut.Append("<td valign=""top"" align=""left"">")
                        'htmlOut.Append(DisplayContactInfo(r("lnote_jetnet_contact_id"), 0))
                        'htmlOut.Append("</td>")
                    End If

                    htmlOut.Append("</tr>")
                Next
                htmlOut.Append("</tbody>")
                htmlOut.Append("</table>")
            Else
                htmlOut.Append("<p align=""center"">There are no notes to display.</p>")
            End If
        End If
        Return htmlOut.ToString
    End Function

    Private Function DisplayCompanyInfo(ByVal jetnetID As Long, ByVal clientID As Long, ByVal displayHeading As Boolean)
        Dim infoTable As New DataTable
        Dim htmlOut As New StringBuilder
        If jetnetID > 0 Then
            infoTable = Master.aclsData_Temp.GetCompanyInfo_ID(jetnetID, "JETNET", JournalID)
        ElseIf clientID > 0 Then
            infoTable = Master.aclsData_Temp.GetCompanyInfo_ID(clientID, "CLIENT", JournalID)
        End If
        If Not IsNothing(infoTable) Then
            If infoTable.Rows.Count > 0 Then
                If Not IsDBNull(infoTable.Rows(0).Item("comp_name")) Then
                    If (displayHeading) Then
                        htmlOut.Append("<div class=""twelve columns"">")
                        htmlOut.Append("<h2 class=""mainHeading remove_margin""><strong>")
                        htmlOut.Append(infoTable.Rows(0).Item("comp_name") & "</strong></h2>")
                    Else
                        htmlOut.Append(infoTable.Rows(0).Item("comp_name") & "<br />")
                    End If

                End If
                htmlOut.Append(abi_functions.DisplayCompanyInformation(infoTable.Rows(0).Item("comp_id"), infoTable.Rows(0).Item("comp_address1"), infoTable.Rows(0).Item("comp_address2"), infoTable.Rows(0).Item("comp_city"), infoTable.Rows(0).Item("comp_state"), infoTable.Rows(0).Item("comp_zip_code"), infoTable.Rows(0).Item("comp_country"), ""))
                If displayHeading Then
                    htmlOut.Append("</div>")
                End If
            End If

        End If

        Return htmlOut.ToString

    End Function

    Private Function DisplayContactInfo(ByVal jetnetID As Long, ByVal clientID As Long)
        Dim ContactTable As New DataTable
        Dim htmlOut As New StringBuilder
        If jetnetID > 0 Then
            ContactTable = Master.aclsData_Temp.GetContacts_Details(jetnetID, CrmSource, False)
        Else
            ContactTable = Master.aclsData_Temp.GetContacts_Details(clientID, CrmSource, False)
        End If
        If Not IsNothing(ContactTable) Then
            If ContactTable.Rows.Count > 0 Then
                If Not IsDBNull(ContactTable.Rows(0).Item("contact_first_name")) Then
                    htmlOut.Append(ContactTable.Rows(0).Item("contact_first_name"))
                End If
                If Not IsDBNull(ContactTable.Rows(0).Item("contact_last_name")) Then
                    htmlOut.Append(" " & ContactTable.Rows(0).Item("contact_last_name"))
                End If
            End If
        End If

        Return htmlOut.ToString

    End Function
    Private Function DisplayAircraftInfo(ByVal jetnetID As Long, ByVal clientID As Long)
        Dim aircraftTable As New DataTable

        Dim htmlOut As New StringBuilder
        If jetnetID > 0 Then
            aircraftTable = CommonAircraftFunctions.BuildReusableTable(jetnetID, 0, "JETNET", "", aclsData_Temp, CRMView, 0, "JETNET")
        ElseIf clientID > 0 Then
            aircraftTable = CommonAircraftFunctions.BuildReusableTable(clientID, 0, "CLIENT", "", aclsData_Temp, CRMView, 0, "CLIENT")
        End If
        If Not IsNothing(aircraftTable) Then
            If aircraftTable.Rows.Count > 0 Then
                If Not IsDBNull(aircraftTable.Rows(0).Item("ac_year_mfr")) Then
                    If aircraftTable.Rows(0).Item("ac_year_mfr") <> "" Then
                        htmlOut.Append(aircraftTable.Rows(0).Item("ac_year_mfr") & " ")
                    End If
                End If

                htmlOut.Append(aircraftTable.Rows(0).Item("amod_make_name") & " " & aircraftTable.Rows(0).Item("amod_model_name") & " ")
                If Not IsDBNull(aircraftTable.Rows(0).Item("ac_ser_nbr")) Then
                    If aircraftTable.Rows(0).Item("ac_ser_nbr") <> "" Then
                        htmlOut.Append("Ser #: " & DisplayFunctions.WriteDetailsLink(jetnetID, 0, 0, 0, True, aircraftTable.Rows(0).Item("ac_ser_nbr"), "", ""))
                    End If
                End If
                htmlOut.Append("<br />")

                If Not IsDBNull(aircraftTable.Rows(0).Item("ac_reg_nbr")) Then
                    If aircraftTable.Rows(0).Item("ac_reg_nbr") <> "" Then
                        htmlOut.Append(" Reg #: " & aircraftTable.Rows(0).Item("ac_reg_nbr"))
                    End If
                End If
            End If
        End If

        Return htmlOut.ToString

    End Function

    Public Function ClientCategory(ByVal catKey As Long) As DataTable
        Dim sql As String = ""

        Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
        Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
        Dim MySqlReader As MySql.Data.MySqlClient.MySqlDataReader : MySqlReader = Nothing
        Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing
        Dim aTempTable As New DataTable
        Try
            sql = "SELECT * FROM note_category where notecat_key = " & catKey & " limit 1"

            MySqlConn.ConnectionString = Session.Item("jetnetServerNotesDatabase")
            MySqlConn.Open()
            MySqlCommand.Connection = MySqlConn
            MySqlCommand.CommandType = CommandType.Text
            MySqlCommand.CommandTimeout = 60

            MySqlCommand.CommandText = sql

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>ClientCategory(ByVal catKey As Long) As DataTable</b><br />" & sql


            MySqlReader = MySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                aTempTable.Load(MySqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = aTempTable.GetErrors()
            End Try
            Return aTempTable
        Catch ex As Exception
            ClientCategory = Nothing
        Finally
            MySqlReader.Close()
            MySqlReader = Nothing

            MySqlConn.Close()
            MySqlConn.Dispose()
            MySqlConn = Nothing

            MySqlCommand.Dispose()
            MySqlCommand = Nothing
        End Try

    End Function

    Private Sub SetUpLeftColumnInformation()
        Dim masterPage As EmptyEvoTheme = DirectCast(Page.Master, EmptyEvoTheme)
        If AircraftID <> 0 Then
            Dim AircraftTable As New DataTable
            Dim passCheckbox As New CheckBox
            passCheckbox.Checked = True
            AircraftTable = CommonAircraftFunctions.BuildReusableTable(AircraftID, JournalID, "", "", aclsData_Temp, True, 0, CrmSource)
            aircraft_information.Text = ShowAircraftInformation(AircraftTable)
            aircraft_information.Visible = True

        End If
        If CompanyID <> 0 Then
            If CrmSource = "CLIENT" Then
                company_information.Text = DisplayCompanyInfo(0, CompanyID, True)
            Else
                company_information.Text = DisplayCompanyInfo(CompanyID, 0, True)
            End If
            company_information.Visible = True
        End If
    End Sub



    Private Function ShowAircraftInformation(ByVal AircraftTable As DataTable) As String
        Dim htmlOut As New StringBuilder
        Dim sSeparator As String = ""

        If Not IsNothing(AircraftTable) Then
            If AircraftTable.Rows.Count > 0 Then
                For Each r As DataRow In AircraftTable.Rows
                    htmlOut.Append("<div class=""twelve columns"">")
                    htmlOut.Append("<h2 class=""mainHeading remove_margin"">")
                    If Not IsDBNull(r.Item("amod_make_name")) Then
                        If Not String.IsNullOrEmpty(r.Item("amod_make_name").ToString) Then
                            htmlOut.Append("<strong>" & r.Item("amod_make_name").ToString.Trim)
                            PageTitle = r.Item("amod_make_name").ToString.Trim
                        End If
                    End If

                    If Not IsDBNull(r.Item("amod_model_name")) Then
                        If Not String.IsNullOrEmpty(r.Item("amod_model_name").ToString) Then
                            htmlOut.Append(" " & r.Item("amod_model_name").ToString.Trim & "</strong>")
                            PageTitle += " " & r.Item("amod_model_name").ToString.Trim
                        End If
                    End If


                    If Not IsDBNull(r.Item("ac_ser_nbr")) Then
                        If Not String.IsNullOrEmpty(r.Item("ac_ser_nbr").ToString) Then
                            htmlOut.Append(" SN #" + r.Item("ac_ser_nbr").ToString.Trim)
                            PageTitle += " SN #" + r.Item("ac_ser_nbr").ToString.Trim
                        End If
                    End If

                    htmlOut.Append("</h2>")

                    If Not IsDBNull(r.Item("ac_reg_no")) Then
                        If Not String.IsNullOrEmpty(r.Item("ac_reg_no").ToString) Then

                            htmlOut.Append("Registration #:&nbsp;" + r.Item("ac_reg_no").ToString.Trim)

                            If Not IsDBNull(r.Item("ac_reg_no_expiration_date")) Then
                                If Not String.IsNullOrEmpty(r.Item("ac_reg_no_expiration_date").ToString) Then
                                    htmlOut.Append("&nbsp;(<em>Expires:&nbsp;" + FormatDateTime(r.Item("ac_reg_no_expiration_date"), DateFormat.ShortDate).Trim + "</em>)")
                                End If
                            End If
                            htmlOut.Append(", ")
                        End If
                    End If

                    If Not IsDBNull(r.Item("ac_purchase_date")) Then
                        If Not String.IsNullOrEmpty(r.Item("ac_purchase_date").ToString) Then
                            htmlOut.Append("Purchased on&nbsp;" + FormatDateTime(r.Item("ac_purchase_date"), DateFormat.ShortDate).Trim + ".")
                        End If
                    End If

                    htmlOut.Append("<br />Located at&nbsp;")

                    If Not IsDBNull(r.Item("ac_aport_iata_code")) Then
                        If Not String.IsNullOrEmpty(r.Item("ac_aport_iata_code").ToString) Then
                            htmlOut.Append(r.Item("ac_aport_iata_code").ToString.Trim)
                            sSeparator = "&nbsp;-&nbsp;"
                        End If
                    End If

                    If Not IsDBNull(r.Item("ac_aport_name")) Then
                        If Not String.IsNullOrEmpty(r.Item("ac_aport_name").ToString) Then
                            htmlOut.Append(sSeparator & Replace(r.Item("ac_aport_name").ToString.Trim, " ", "&nbsp;"))
                            sSeparator = ",&nbsp;"
                        End If
                    End If

                    If Not IsDBNull(r.Item("ac_aport_city")) Then
                        If Not String.IsNullOrEmpty(r.Item("ac_aport_city").ToString) Then
                            htmlOut.Append(sSeparator + Replace(r.Item("ac_aport_city").ToString.Trim, " ", "&nbsp;"))
                            sSeparator = ",&nbsp;"
                        End If
                    End If

                    If Not IsDBNull(r.Item("ac_aport_state")) Then
                        If Not String.IsNullOrEmpty(r.Item("ac_aport_state").ToString) Then
                            htmlOut.Append(sSeparator + Replace(r.Item("ac_aport_state").ToString.Trim, " ", "&nbsp;"))
                            sSeparator = crmWebClient.Constants.cSingleSpace
                        End If
                    End If

                    If Not IsDBNull(r.Item("ac_aport_country")) Then
                        If Not String.IsNullOrEmpty(r.Item("ac_aport_country").ToString) Then
                            htmlOut.Append(sSeparator + Replace(Replace(r.Item("ac_aport_country").ToString.Trim, " ", "&nbsp;"), "United&nbsp;States", "U.S.") + "")
                        End If
                    End If
                    htmlOut.Append("</div>")
                Next
            End If
        End If
        Return htmlOut.ToString
    End Function
End Class