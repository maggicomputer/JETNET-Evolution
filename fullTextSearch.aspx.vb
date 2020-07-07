
' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/fullTextSearch.aspx.vb $
'$$Author: Amanda $
'$$Date: 6/18/20 4:17p $
'$$Modtime: 6/18/20 2:20p $
'$$Revision: 8 $
'$$Workfile: fullTextSearch.aspx.vb $
'
' ********************************************************************************

Partial Public Class fullTextSearch

    Inherits System.Web.UI.Page
    Dim CRMViewActive As Boolean = False
    Dim SearchMPMData As Boolean = False
    Const DISPLAY_COUNT As Integer = 500
    Dim masterPage As New Object
    Dim aircraftFilter As String = ""
    Dim companyFilter As String = ""
    Dim modelFilter As String = ""
    Dim EditedSearchTerm As String = ""

    Private Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit



        If Session.Item("isMobile") = True Then
            Me.MasterPageFile = "~/EvoStyles/MobileTheme.master"
        Else
            Me.MasterPageFile = "~/EvoStyles/EmptyEvoTheme.master"
        End If
    End Sub
    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        If Session.Item("isMobile") Then
            masterPage = DirectCast(Page.Master, MobileTheme)
            masterPage.RemoveWhiteBackground(True)
        Else
            masterPage = DirectCast(Page.Master, EmptyEvoTheme)
        End If
        masterPage.SetContainerClass("container MaxWidthRemove") 'set full width page

        If Not IsNothing(Session.Item("jetnetAppVersion")) Then
            If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CRM Then
                CRMViewActive = True
                SearchMPMData = True
            End If

            If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO And Session.Item("localUser").crmUser_Evo_MPM_Flag = True Then
                SearchMPMData = True
                CRMViewActive = False
            End If

        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim sErrorString As String = ""

        If Session.Item("crmUserLogon") <> True Then

            ' commonLogFunctions.forceLogError("UserError", "Lost Session - Evolution Full Text Search")

            Response.Redirect("Default.aspx", False)

        Else



            If Not IsPostBack Then

                If Not Session.Item("localPreferences").loadUserSession(sErrorString, CLng(Session.Item("localUser").crmSubSubID.ToString), Session.Item("localUser").crmUserLogin.ToString, CLng(Session.Item("localUser").crmSubSeqNo.ToString), CLng(Session.Item("localUser").crmUserContactID.ToString)) Then
                    Response.Write("error in load FullTextSearch : " + sErrorString)
                End If

                Dim fSubins_platform_os As String = commonEvo.getBrowserCapabilities(Request.Browser)

                If Not IsNothing(Trim(Request("q"))) Then
                    If Not String.IsNullOrEmpty(Trim(Request("q"))) Then
                        full_text_search_input.Text = Trim(Request("q"))
                        full_text_search_button_Click(full_text_search_button, System.EventArgs.Empty)
                    End If
                End If
            End If

            ' added MSW - some crm sites are not working properly.
            If IsPostBack Then
                If Not IsNothing(Session.Item("jetnetAppVersion")) Then
                    If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CRM Then
                        CRMViewActive = True
                        SearchMPMData = True
                    End If
                End If
            End If


            masterPage.SetPageTitle("")  ' sets the page title

            'commonLogFunctions.forceLogError("INFORMATION", "Evolution Full Text Search")



        End If

    End Sub

    Protected Sub full_text_search_button_Click(ByVal sender As Object, ByVal e As EventArgs) Handles full_text_search_button.Click

        ' parse out text
        Dim results_table As New DataTable
        Dim DisplayAmount As Long = 4 ' start off saying 4 are going to show
        Dim aircraftList As String = ""
        Dim modelList As String = ""
        Dim companyList As String = ""
        Dim contactList As String = ""
        Dim yachtList As String = ""
        Dim ClientTable As New DataTable
        Dim temp_amod_id As Long = 0
        Dim is_for_sale As String = ""
        Dim jetnet_ac_ids As String = ""
        Dim jetnet_ac_ids_to_avoid As String = ""
        Dim pass_through_to_find_client As Boolean = False
        full_text_search_no_results.Text = ""
        modelTable.Text = ""
        aircraftTable.Text = ""
        companyTable.Text = ""
        contactTable.Text = ""
        yachtTable.Text = ""
        If Not String.IsNullOrEmpty(full_text_search_input.Text.Trim) Then
            EditedSearchTerm = strClean(full_text_search_input.Text.Trim)

            Call commonLogFunctions.Log_User_Event_Data("UserSearch", "Phrase: " & EditedSearchTerm, Nothing, 0, 0, 0, 0, 0, 0, , 0, "")

            If EditedSearchTerm.Length > 2 Then

                ''ADDED IN MSW - 3/20/18
                '' if we are on evo and have mpm then we will need to look up the mpm aircraft
                If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO And Session.Item("localUser").crmUser_Evo_MPM_Flag = True Then
                    If Trim(Request("amod_id")) <> "" Then
                        temp_amod_id = Trim(Request("amod_id"))
                        is_for_sale = Trim(Request("for_sale"))
                        ClientTable = grabClientAircrafForSaleByModel(temp_amod_id, is_for_sale, "")

                        If Not IsNothing(ClientTable) Then
                            If ClientTable.Rows.Count > 0 Then
                                For Each r As DataRow In ClientTable.Rows
                                    If Trim(jetnet_ac_ids) <> "" Then
                                        jetnet_ac_ids &= ", "
                                    End If
                                    jetnet_ac_ids &= r.Item("client_jetnet_ac_id").ToString
                                Next
                            End If
                        End If

                        'this will get you all of the client aircraft for that model, where its not whatever the other list should get 
                        ClientTable = grabClientAircrafForSaleByModel(temp_amod_id, "", jetnet_ac_ids)

                        If Not IsNothing(ClientTable) Then
                            If ClientTable.Rows.Count > 0 Then
                                For Each r As DataRow In ClientTable.Rows
                                    If Trim(jetnet_ac_ids_to_avoid) <> "" Then
                                        jetnet_ac_ids_to_avoid &= ", "
                                    End If
                                    jetnet_ac_ids_to_avoid &= r.Item("client_jetnet_ac_id").ToString
                                Next
                            End If
                        End If



                    End If
                End If



                If SearchMPMData Then
                    results_table = returnFullTextSearchResults(EditedSearchTerm, jetnet_ac_ids, jetnet_ac_ids_to_avoid)
                Else
                    results_table = returnFullTextSearchResults(EditedSearchTerm, jetnet_ac_ids, "")
                End If

                pass_through_to_find_client = False
                If Not IsNothing(results_table) And SearchMPMData = True Then   ' always finds something 
                    If results_table.Rows.Count = 0 Then
                        pass_through_to_find_client = True
                    End If
                End If



                If Not IsNothing(results_table) Or pass_through_to_find_client Then

                    If results_table.Rows.Count > 0 Or pass_through_to_find_client Then

                        display_aircraft_results_table(results_table, aircraftList, Session.Item("isMobile"))

                        display_model_results_table(results_table, modelList, Session.Item("isMobile"))

                        display_company_results_table(results_table, companyList, Session.Item("isMobile"))

                        display_contact_results_table(results_table, contactList, Session.Item("isMobile"))

                        If Session.Item("localSubscription").crmYacht_Flag Then
                            display_yacht_results_table(results_table, yachtList, Session.Item("isMobile"))
                        End If

                        If Not IsNothing(results_table) Then
                            full_text_search_results_row.Visible = True


                            If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT And String.IsNullOrEmpty(modelFilter) Then
                            Else
                                If Not String.IsNullOrEmpty(modelList.Trim) Then
                                    modelTable.Text = modelList
                                    System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CreateModelTable", "$(document).ready(function() { CreateTheDatatable('fullTextModelInnerTable','fullTextModelDataTable','fullTextModeljQueryTable'); });", True)
                                Else
                                    DisplayAmount = DisplayAmount - 1
                                    '  modelTable.Text = "<table id=""fullTextSearchOuterTable"" cellpadding=""2"" cellspacing=""0"">"
                                    '  modelTable.Text += "<tr><td align=""left"" valign=""middle""><em>No Model Results To Display</em></td></tr>"
                                    '  modelTable.Text += "</table>"
                                End If
                            End If
                            If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT And String.IsNullOrEmpty(aircraftFilter) Then
                            Else
                                If Not String.IsNullOrEmpty(aircraftList.Trim) Then
                                    aircraftTable.Text = aircraftList
                                    System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CreateAircraftTable", "$(document).ready(function() { CreateTheDatatable('fullTextAircraftInnerTable','fullTextAircraftDataTable','fullTextAircraftjQueryTable'); });", True)
                                Else
                                    DisplayAmount = DisplayAmount - 1
                                    '  aircraftTable.Text = "<table id=""fullTextSearchOuterTable"" cellpadding=""2"" cellspacing=""0"">"
                                    '  aircraftTable.Text += "<tr><td align=""left"" valign=""middle""><em>No Aircraft Results To Display</em></td></tr>"
                                    '  aircraftTable.Text += "</table>"
                                End If
                            End If
                            If Not String.IsNullOrEmpty(companyList.Trim) Then
                                companyTable.Text = companyList
                                System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CreateCompanyTable", "$(document).ready(function() { CreateTheDatatable('fullTextCompanyInnerTable','fullTextCompanyDataTable','fullTextCompanyjQueryTable'); });", True)
                            Else
                                DisplayAmount = DisplayAmount - 1
                                '  companyTable.Text = "<table id=""fullTextSearchOuterTable"" cellpadding=""2"" cellspacing=""0"">"
                                '  companyTable.Text += "<tr><td align=""left"" valign=""middle""><em>No Company Results To Display</em></td></tr>"
                                '  companyTable.Text += "</table>"
                            End If

                            If Not String.IsNullOrEmpty(contactList.Trim) Then
                                contactTable.Text = contactList
                                System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CreateContactTable", "$(document).ready(function() { CreateTheDatatable('fullTextContactInnerTable','fullTextContactDataTable','fullTextContactjQueryTable'); });", True)
                            Else
                                DisplayAmount = DisplayAmount - 1
                                '  contactTable.Text = "<table id=""fullTextSearchOuterTable"" cellpadding=""2"" cellspacing=""0"">"
                                '  contactTable.Text += "<tr><td align=""left"" valign=""middle""><em>No Contact Results To Display</em></td></tr>"
                                '  contactTable.Text += "</table>"
                            End If

                            If Not String.IsNullOrEmpty(yachtList.Trim) Then
                                yachtTable.Text = yachtList
                                System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CreateYachtTable", "$(document).ready(function() { CreateTheDatatable('fullTextYachtInnerTable','fullTextYachtDataTable','fullTextYachtjQueryTable'); });", True)
                            Else
                                DisplayAmount = DisplayAmount - 1
                                '  contactTable.Text = "<table id=""fullTextSearchOuterTable"" cellpadding=""2"" cellspacing=""0"">"
                                '  contactTable.Text += "<tr><td align=""left"" valign=""middle""><em>No Contact Results To Display</em></td></tr>"
                                '  contactTable.Text += "</table>"
                            End If


                            If Not IsNothing(Session.Item("isMobile")) Then
                                If Session.Item("isMobile") = False Then
                                    If DisplayAmount = 1 Then ' we can rework the floats
                                        modelColumn.Attributes.Remove("class")
                                        aircraftColumn.Attributes.Remove("class")
                                        contactColumn.Attributes.Remove("class")
                                        companyColumn.Attributes.Remove("class")

                                        modelColumn.Attributes.Add("class", "columns eight float_left")
                                        aircraftColumn.Attributes.Add("class", "columns eight float_left")
                                        contactColumn.Attributes.Add("class", "columns eight float_left")
                                        companyColumn.Attributes.Add("class", "columns eight float_left")
                                    ElseIf DisplayAmount = 3 Then
                                        contactColumn.Attributes.Remove("class")
                                        companyColumn.Attributes.Remove("class")


                                        contactColumn.Attributes.Add("class", "columns eight float_left")

                                        companyColumn.Attributes.Add("class", "columns eight float_left")
                                    End If
                                End If
                            End If
                        End If

                    Else

                        full_text_search_no_results.Visible = True
                        full_text_search_results_row.Visible = False

                        full_text_search_no_results.Text = "<table id=""fullTextSearchOuterTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"">"
                        full_text_search_no_results.Text += "<tr><td align=""left"" valign=""middle""><em>No Results To Display</em></td></tr>"
                        full_text_search_no_results.Text += "</table>"

                    End If

                Else

                    full_text_search_no_results.Visible = True
                    full_text_search_results_row.Visible = False

                    full_text_search_no_results.Text = "<table id=""fullTextSearchOuterTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"">"
                    full_text_search_no_results.Text += "<tr><td align=""left"" valign=""middle""><em>No Results To Display</em></td></tr>"
                    full_text_search_no_results.Text += "</table>"

                End If
            Else
                full_text_search_no_results.Visible = True
                full_text_search_results_row.Visible = False
                full_text_search_no_results.ForeColor = Drawing.Color.Red
                full_text_search_no_results.Font.Bold = True
                full_text_search_no_results.Text = "<table id=""fullTextSearchOuterTable"" width=""100%"" cellpadding=""2"" cellspacing=""0""><tr><td align=""left"" valign=""middle""><br /><p align=""center""><em>Please enter more than 2 characters in your search phrase.</em></p>"
                full_text_search_no_results.Text &= "<p align=""center""><em>Note that if your aircraft model or serial number is less than 3 characters then include the make of the aircraft in the search such as typing 'FALCON 7X' instead of just '7X'</em></p>"
                full_text_search_no_results.Text &= "</td></tr></table>"
            End If
        End If

        results_table = Nothing

    End Sub

    Protected Function returnFullTextSearchResults(ByVal sFullSearchText As String, ByVal jetnet_ac_ids As String, ByVal ids_to_ignore As String) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Dim wordArray() As String = Nothing
        Dim sTmpString As String = ""
        Dim not_for_sale_found As Integer = 0
        Dim for_sale_found As Integer = 0
        Dim tcount As Integer = 0
        Dim search_only_for_sale As Boolean = False
        Dim special_case As Boolean = False

        Try

            companyFilter = clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(HttpContext.Current.Session.Item("localSubscription"), True, False)

            If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT Then
                If HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = False And HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = False Then
                    companyFilter = " and comp_product_yacht_flag = 'Y'"
                End If
            End If

            aircraftFilter = clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(HttpContext.Current.Session.Item("localSubscription"), False, True)
            modelFilter = Replace(clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(HttpContext.Current.Session.Item("localSubscription"), False, False), "amod", "mdl.amod")




            sQuery.Append("SELECT fts_data_search, mdl.amod_make_name, mdl.amod_model_name, mdl.amod_manufacturer, mdl.amod_id as fts_amod_id, fts_ac_id as fts_ac_idFilter,")
            sQuery.Append(" Aircraft_Flat.ac_ser_no_full, Aircraft_Flat.ac_ser_no_sort, Aircraft_Flat.ac_reg_no, Aircraft_Flat.ac_aport_iata_code, Aircraft_Flat.ac_aport_icao_code, Aircraft_Flat.ac_aport_name, Aircraft_Flat.ac_prev_reg_no, Aircraft_Flat.ac_id as fts_ac_id,")
            sQuery.Append(" comp_name, comp_city, comp_address1, comp_address2, comp_state, comp_zip_code, comp_country, comp_id as fts_comp_id,")
            sQuery.Append(" contact_sirname, contact_first_name, contact_middle_initial, contact_last_name, contact_suffix, contact_title, contact_email_address, fts_contact_id")

            If Session.Item("localSubscription").crmYacht_Flag Then
                sQuery.Append(", yt_id as fts_yacht_id,yt_year_mfr, yt_yacht_name, yt_hull_mfr_nbr, ym_brand_name, ym_model_name ")
            End If
            sQuery.Append(" FROM Full_Text_Search WITH(NOLOCK)")

            If Session.Item("localSubscription").crmYacht_Flag Then
                sQuery.Append(" LEFT OUTER JOIN Yacht WITH(NOLOCK) ON yt_id = fts_yacht_id AND yt_journ_id = 0 ")
                sQuery.Append(" LEFT OUTER JOIN yacht_model WITH(NOLOCK) ON ym_model_id = yt_model_id ")
            End If

            sQuery.Append(" LEFT OUTER JOIN Company WITH(NOLOCK) ON comp_id = fts_comp_id AND comp_journ_id = 0 AND comp_active_flag = 'Y' AND comp_hide_flag = 'N' " & companyFilter)
            sQuery.Append(" LEFT OUTER JOIN Contact WITH(NOLOCK) ON contact_id = fts_contact_id AND contact_journ_id = 0 AND contact_active_flag = 'Y' AND contact_hide_flag = 'N'")
            sQuery.Append(" LEFT OUTER JOIN Aircraft_Flat WITH(NOLOCK) ON ac_id = fts_ac_id AND ac_journ_id = 0 " & aircraftFilter)
            sQuery.Append(" LEFT OUTER JOIN Aircraft_Model AS mdl WITH(NOLOCK) ON mdl.amod_id = fts_amod_id " & modelFilter)


            wordArray = sFullSearchText.Trim.Split(" ")

            ' ADDED MSW - 2/28/18
            for_sale_found = 0
            not_for_sale_found = 0
            tcount = 0
            For Each wd As String In wordArray
                tcount = tcount + 1

                If Trim(Replace(Replace(wd.Trim, "'", ""), "-", "")) = "not" Then
                    not_for_sale_found = tcount
                End If

                If Trim(Replace(Replace(wd.Trim, "'", ""), "-", "")) = "for" Then
                    for_sale_found = tcount
                End If

                ' if we have found a "for" and we have found the word "sale" in the next spot
                If for_sale_found > 0 And (for_sale_found = (tcount - 1)) Then
                    If Trim(Replace(Replace(wd.Trim, "'", ""), "-", "")) = "sale" Then
                        search_only_for_sale = True
                        special_case = True
                    End If
                End If
            Next

            ' inner join to a new table, to only get the for sale data, note you are still only selecting the other one, so results wont change  
            If not_for_sale_found > 0 And for_sale_found > 0 And ((not_for_sale_found + 1) = for_sale_found) And search_only_for_sale = True Then
                sQuery.Append(" inner join Aircraft_Flat a2 with (NOLOCK) on a2.ac_id = fts_ac_id AND a2.ac_journ_id = 0 AND a2.amod_customer_flag = 'Y'  AND (( a2.amod_product_business_flag = 'Y') OR ( a2.amod_product_commercial_flag = 'Y') OR (a2.amod_product_helicopter_flag = 'Y')) AND ( a2.ac_product_business_flag = 'Y'  OR  a2.ac_product_commercial_flag = 'Y'  OR  a2.ac_product_helicopter_flag = 'Y')")

                If Trim(jetnet_ac_ids) <> "" Then
                    sQuery.Append(" and (a2.ac_forsale_flag = 'N' or Aircraft_Flat.ac_id in (" & Trim(jetnet_ac_ids) & ")) ")
                Else
                    sQuery.Append(" and a2.ac_forsale_flag = 'N' ")
                End If

            ElseIf search_only_for_sale = True Then
                sQuery.Append(" inner join Aircraft_Flat a2 with (NOLOCK) on a2.ac_id = fts_ac_id AND a2.ac_journ_id = 0 AND a2.amod_customer_flag = 'Y'  AND (( a2.amod_product_business_flag = 'Y') OR ( a2.amod_product_commercial_flag = 'Y') OR (a2.amod_product_helicopter_flag = 'Y')) AND ( a2.ac_product_business_flag = 'Y'  OR  a2.ac_product_commercial_flag = 'Y'  OR  a2.ac_product_helicopter_flag = 'Y')")

                If Trim(jetnet_ac_ids) <> "" Then
                    sQuery.Append(" and (a2.ac_forsale_flag = 'Y' or Aircraft_Flat.ac_id in (" & Trim(jetnet_ac_ids) & ")) ")
                Else
                    sQuery.Append(" and a2.ac_forsale_flag = 'Y' ")
                End If

            End If

            sQuery.Append(" WHERE (")

            sQuery.Append(" ( ")

            tcount = 0
            For Each wd As String In wordArray
                tcount = tcount + 1
                If special_case = True Then
                    If search_only_for_sale = True And ((tcount = for_sale_found) Or (tcount = (for_sale_found + 1)) Or ((tcount = (for_sale_found - 1)) And not_for_sale_found > 0)) Then ' and we are in the "for" spot or the "sale" spot, which is one later 
                    Else
                        If String.IsNullOrEmpty(sTmpString.Trim) Then
                            sTmpString = "contains (Full_Text_Search.*, '""" + Replace(Replace(wd.Trim, "'", ""), "-", "") + "*""')"
                        Else
                            sTmpString += Constants.cAndClause + "contains (Full_Text_Search.*, '""" + Replace(Replace(wd.Trim, "'", ""), "-", "") + "*""')"
                        End If
                    End If

                Else
                    If String.IsNullOrEmpty(sTmpString.Trim) Then
                        sTmpString = "contains (Full_Text_Search.*, '""" + Replace(Replace(wd.Trim, "'", ""), "-", "") + "*""')"
                    Else
                        sTmpString += Constants.cAndClause + "contains (Full_Text_Search.*, '""" + Replace(Replace(wd.Trim, "'", ""), "-", "") + "*""')"
                    End If
                End If

            Next

            If Not String.IsNullOrEmpty(sTmpString.Trim) Then
                sQuery.Append(sTmpString)
            End If

            sQuery.Append(" ) ")


            If Trim(ids_to_ignore) <> "" Then
                sQuery.Append("  and Aircraft_Flat.ac_id not in (" & Trim(ids_to_ignore) & ")")
            End If

            sQuery.Append(") ORDER BY comp_name, contact_last_name, amod_make_name, amod_model_name, ac_ser_no_full")
            If Session.Item("localSubscription").crmYacht_Flag Then
                sQuery.Append(" ,yt_yacht_name")
            End If

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "fullTextSearch.aspx.vb", sQuery.ToString)

            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase") 'HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
            SqlConn.Open()
            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 60

            SqlCommand.CommandText = sQuery.ToString
            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in returnFullTextSearchResults load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            atemptable = Nothing

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in returnFullTextSearchResults(ByVal sFullSearchText As String) As DataTable " + ex.Message

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

    Protected Function grabClientAircraftByJetnetID(ByVal includeIDs As String, ByVal amod_id As Long, ByVal for_sale As String) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
        Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
        Dim MySqlReader As MySql.Data.MySqlClient.MySqlDataReader : MySqlReader = Nothing
        Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing
        Dim has_where As Boolean = False

        Try
            If includeIDs <> "" Then
                sQuery.Append("SELECT 'CLIENT' as source,0 as fts_ac_idFilter, cliaircraft_jetnet_ac_id as client_jetnet_ac_id, '' as fts_data_search, cliamod_make_name as amod_make_name, cliamod_model_name as amod_model_name,  '' as amod_manufacturer, cliamod_jetnet_amod_id as fts_amod_id,")
                sQuery.Append("cliaircraft_ser_nbr as ac_ser_no_full, cliaircraft_ser_nbr_sort as ac_ser_no_sort, cliaircraft_reg_nbr as ac_reg_no, cliaircraft_aport_iata_code as ac_aport_iata_code, cliaircraft_aport_icao_code as ac_aport_icao_code, cliaircraft_aport_name as ac_aport_name, cliaircraft_prev_reg_nbr as ac_prev_reg_no, cliaircraft_id as fts_ac_id,")
                sQuery.Append(" '' as comp_name, '' as comp_city, '' as comp_address1, '' as comp_address2, '' as comp_state, '' as comp_zip_code, '' as comp_country, 0 as fts_comp_id,")
                sQuery.Append(" '' as contact_sirname, '' as contact_first_name, '' as contact_middle_initial, '' as contact_last_name, '' as contact_suffix, '' as contact_title, '' as contact_email_address, 0 as fts_contact_id")
                sQuery.Append(" from client_aircraft INNER JOIN client_aircraft_model ON client_aircraft.cliaircraft_cliamod_id = client_aircraft_model.cliamod_id ")
                sQuery.Append(" WHERE ")

                If Trim(includeIDs) <> "" Then
                    sQuery.Append("cliaircraft_jetnet_ac_id in (" & includeIDs & ")")
                    has_where = True
                End If

                If amod_id > 0 Then
                    If has_where = True Then
                        sQuery.Append(" and ")
                    End If
                    sQuery.Append("cliamod_jetnet_amod_id in (" & includeIDs & ")")
                    has_where = True
                End If

                If Trim(for_sale) <> "" Then
                    If has_where = True Then
                        sQuery.Append(" and ")
                    End If
                    sQuery.Append("cliaircraft_forsale_flag = '" & for_sale & "' ")
                    has_where = True
                End If


                sQuery.Append(" ORDER BY cliaircraft_ser_nbr")

                clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "fullTextSearch.aspx.vb", sQuery.ToString)

                MySqlConn.ConnectionString = Session.Item("jetnetServerNotesDatabase")
                MySqlConn.Open()
                MySqlCommand.Connection = MySqlConn
                MySqlCommand.CommandType = CommandType.Text
                MySqlCommand.CommandTimeout = 60

                MySqlCommand.CommandText = sQuery.ToString
                MySqlReader = MySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

                Try
                    atemptable.Load(MySqlReader)
                Catch constrExc As System.Data.ConstraintException
                    Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in returnFullTextSearchResults grabClientAircraftByJetnetID " + constrExc.Message
                End Try
            End If
        Catch ex As Exception
            atemptable = Nothing

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in returnFullTextSearchResults(ByVal sFullSearchText As String) As DataTable " + ex.Message

        Finally
            MySqlReader = Nothing

            MySqlConn.Dispose()
            MySqlConn.Close()
            MySqlConn = Nothing

            MySqlCommand.Dispose()
            MySqlCommand = Nothing
        End Try

        Return atemptable

    End Function


    Protected Function grabClientAircrafForSaleByModel(ByVal amod_id As Long, ByVal for_sale As String, ByVal not_these_ac As String) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
        Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
        Dim MySqlReader As MySql.Data.MySqlClient.MySqlDataReader : MySqlReader = Nothing
        Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing
        Dim has_where As Boolean = False

        Try
            sQuery.Append("SELECT 'CLIENT' as source,0 as fts_ac_idFilter, cliaircraft_jetnet_ac_id as client_jetnet_ac_id, '' as fts_data_search, cliamod_make_name as amod_make_name, cliamod_model_name as amod_model_name,  '' as amod_manufacturer, cliamod_jetnet_amod_id as fts_amod_id,")
            sQuery.Append("cliaircraft_ser_nbr as ac_ser_no_full, cliaircraft_ser_nbr_sort as ac_ser_no_sort, cliaircraft_reg_nbr as ac_reg_no, cliaircraft_aport_iata_code as ac_aport_iata_code, cliaircraft_aport_icao_code as ac_aport_icao_code, cliaircraft_aport_name as ac_aport_name, cliaircraft_prev_reg_nbr as ac_prev_reg_no, cliaircraft_id as fts_ac_id,")
            sQuery.Append(" '' as comp_name, '' as comp_city, '' as comp_address1, '' as comp_address2, '' as comp_state, '' as comp_zip_code, '' as comp_country, 0 as fts_comp_id,")
            sQuery.Append(" '' as contact_sirname, '' as contact_first_name, '' as contact_middle_initial, '' as contact_last_name, '' as contact_suffix, '' as contact_title, '' as contact_email_address, 0 as fts_contact_id")
            sQuery.Append(" from client_aircraft INNER JOIN client_aircraft_model ON client_aircraft.cliaircraft_cliamod_id = client_aircraft_model.cliamod_id ")
            sQuery.Append(" WHERE ")

            If amod_id > 0 Then
                If has_where = True Then
                    sQuery.Append(" and ")
                End If
                sQuery.Append("cliamod_jetnet_amod_id in (" & amod_id & ")")
                has_where = True
            End If

            If Trim(for_sale) <> "" Then
                If has_where = True Then
                    sQuery.Append(" and ")
                End If
                sQuery.Append("cliaircraft_forsale_flag = '" & for_sale & "' ")
                has_where = True
            End If

            If Trim(not_these_ac) <> "" Then
                If has_where = True Then
                    sQuery.Append(" and ")
                End If
                sQuery.Append("cliaircraft_jetnet_ac_id not in(" & not_these_ac & ") ")
                has_where = True
            End If


            sQuery.Append(" ORDER BY cliaircraft_ser_nbr")

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "fullTextSearch.aspx.vb", sQuery.ToString)

            MySqlConn.ConnectionString = Session.Item("jetnetServerNotesDatabase")
            MySqlConn.Open()
            MySqlCommand.Connection = MySqlConn
            MySqlCommand.CommandType = CommandType.Text
            MySqlCommand.CommandTimeout = 60

            MySqlCommand.CommandText = sQuery.ToString
            MySqlReader = MySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                atemptable.Load(MySqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in grabClientAircrafForSaleByModel grabClientAircrafForSaleByModel " + constrExc.Message
            End Try
        Catch ex As Exception
            atemptable = Nothing

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in grabClientAircrafForSaleByModel(ByVal sFullSearchText As String) As DataTable " + ex.Message

        Finally
            MySqlReader = Nothing

            MySqlConn.Dispose()
            MySqlConn.Close()
            MySqlConn = Nothing

            MySqlCommand.Dispose()
            MySqlCommand = Nothing
        End Try

        Return atemptable

    End Function

    Protected Function grabClientCompanyByJetnetID(ByVal includeIDs As String) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
        Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
        Dim MySqlReader As MySql.Data.MySqlClient.MySqlDataReader : MySqlReader = Nothing
        Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing

        Try
            If includeIDs <> "" Then
                sQuery.Append("SELECT 'CLIENT' as source,0 as fts_ac_idFilter, clicomp_jetnet_comp_id as client_jetnet_comp_id, '' as fts_data_search, '' as amod_make_name, '' as amod_model_name,  '' as amod_manufacturer, 0 as fts_amod_id,")
                sQuery.Append("'' as ac_ser_no_full, '' as ac_ser_no_sort, '' as ac_reg_no, '' as ac_aport_iata_code, '' as ac_aport_icao_code, '' as ac_aport_name, '' as ac_prev_reg_no, 0 as fts_ac_id,")
                sQuery.Append(" clicomp_name as comp_name, clicomp_city as comp_city, clicomp_address1 as comp_address1, clicomp_address2 as comp_address2, clicomp_state as comp_state, clicomp_zip_code as comp_zip_code, clicomp_country as comp_country, clicomp_id as fts_comp_id,")
                sQuery.Append(" '' as contact_sirname, '' as contact_first_name, '' as contact_middle_initial, '' as contact_last_name, '' as contact_suffix, '' as contact_title, '' as contact_email_address, 0 as fts_contact_id")
                sQuery.Append(" from client_company ")
                sQuery.Append(" WHERE ")

                sQuery.Append("clicomp_jetnet_comp_id in (" & includeIDs & ")")
                sQuery.Append(" ORDER BY clicomp_name")

                clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "fullTextSearch.aspx.vb", sQuery.ToString)

                MySqlConn.ConnectionString = Session.Item("jetnetServerNotesDatabase")
                MySqlConn.Open()
                MySqlCommand.Connection = MySqlConn
                MySqlCommand.CommandType = CommandType.Text
                MySqlCommand.CommandTimeout = 60

                MySqlCommand.CommandText = sQuery.ToString
                MySqlReader = MySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

                Try
                    atemptable.Load(MySqlReader)
                Catch constrExc As System.Data.ConstraintException
                    Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in returnFullTextSearchResults grabClientAircraftByJetnetID " + constrExc.Message
                End Try
            End If
        Catch ex As Exception
            atemptable = Nothing

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in returnFullTextSearchResults(ByVal sFullSearchText As String) As DataTable " + ex.Message

        Finally
            MySqlReader = Nothing

            MySqlConn.Dispose()
            MySqlConn.Close()
            MySqlConn = Nothing

            MySqlCommand.Dispose()
            MySqlCommand = Nothing
        End Try

        Return atemptable

    End Function

    Protected Function grabClientContactByJetnetID(ByVal includeIDs As String, ByVal contactString As String) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
        Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
        Dim MySqlReader As MySql.Data.MySqlClient.MySqlDataReader : MySqlReader = Nothing
        Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing

        Try
            If includeIDs <> "" Or Trim(contactString) <> "" Then
                sQuery.Append("SELECT 'CLIENT' as source, 0 as fts_ac_idFilter, clicontact_jetnet_contact_id as client_jetnet_contact_id, '' as fts_data_search, '' as amod_make_name, '' as amod_model_name,  '' as amod_manufacturer, 0 as fts_amod_id,")
                sQuery.Append("'' as ac_ser_no_full, '' as ac_ser_no_sort, '' as ac_reg_no, '' as ac_aport_iata_code, '' as ac_aport_icao_code, '' as ac_aport_name, '' as ac_prev_reg_no, 0 as fts_ac_id,")
                sQuery.Append(" '' as comp_name, '' as comp_city, '' as comp_address1, '' as comp_address2, '' as comp_state, '' as comp_zip_code, '' as comp_country, clicontact_comp_id as fts_comp_id,")
                sQuery.Append(" clicontact_sirname as contact_sirname, clicontact_first_name as contact_first_name, clicontact_middle_initial as contact_middle_initial, clicontact_last_name as contact_last_name, clicontact_suffix as contact_suffix, clicontact_title as contact_title, clicontact_email_address as contact_email_address, clicontact_id as fts_contact_id")
                sQuery.Append(" from client_contact ")


                If Trim(includeIDs) <> "" Then
                    sQuery.Append(" WHERE (clicontact_jetnet_contact_id in (" & includeIDs & ") or (clicontact_first_name like '%" & contactString & "%' or clicontact_last_name like '%" & contactString & "%' or clicontact_email_address like '%" & contactString & "%')) ")
                Else
                    sQuery.Append(" WHERE (clicontact_first_name like '%" & contactString & "%' or clicontact_last_name like '%" & contactString & "%' or clicontact_email_address like '%" & contactString & "%')  ")
                End If



                sQuery.Append(" ORDER BY clicontact_first_name, clicontact_last_name")

                clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "fullTextSearch.aspx.vb", sQuery.ToString)

                MySqlConn.ConnectionString = Session.Item("jetnetServerNotesDatabase")
                MySqlConn.Open()
                MySqlCommand.Connection = MySqlConn
                MySqlCommand.CommandType = CommandType.Text
                MySqlCommand.CommandTimeout = 60

                MySqlCommand.CommandText = sQuery.ToString
                MySqlReader = MySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

                Try
                    atemptable.Load(MySqlReader)
                Catch constrExc As System.Data.ConstraintException
                    Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in returnFullTextSearchResults grabClientAircraftByJetnetID " + constrExc.Message
                End Try
            End If
        Catch ex As Exception
            atemptable = Nothing

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in returnFullTextSearchResults(ByVal sFullSearchText As String) As DataTable " + ex.Message

        Finally
            MySqlReader = Nothing

            MySqlConn.Dispose()
            MySqlConn.Close()
            MySqlConn = Nothing

            MySqlCommand.Dispose()
            MySqlCommand = Nothing
        End Try

        Return atemptable

    End Function

    Protected Function grabClientCompany(ByVal companyString As String) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
        Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
        Dim MySqlReader As MySql.Data.MySqlClient.MySqlDataReader : MySqlReader = Nothing
        Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing

        Try
            If Not String.IsNullOrEmpty(companyString.Trim) Then
                sQuery.Append("SELECT 'CLIENT' as source,0 as fts_ac_idFilter, clicomp_jetnet_comp_id as client_jetnet_comp_id, '' as fts_data_search, '' as amod_make_name, '' as amod_model_name,  '' as amod_manufacturer, 0 as fts_amod_id,")
                sQuery.Append("'' as ac_ser_no_full, '' as ac_ser_no_sort, '' as ac_reg_no, '' as ac_aport_iata_code, '' as ac_aport_icao_code, '' as ac_aport_name, '' as ac_prev_reg_no, 0 as fts_ac_id,")
                sQuery.Append(" clicomp_name as comp_name, clicomp_city as comp_city, clicomp_address1 as comp_address1, clicomp_address2 as comp_address2, clicomp_state as comp_state, clicomp_zip_code as comp_zip_code, clicomp_country as comp_country, clicomp_id as fts_comp_id,")
                sQuery.Append(" '' as contact_sirname, '' as contact_first_name, '' as contact_middle_initial, '' as contact_last_name, '' as contact_suffix, '' as contact_title, '' as contact_email_address, 0 as fts_contact_id")
                sQuery.Append(" from client_company ")
                sQuery.Append(" WHERE")

                sQuery.Append(" lower(clicomp_name) LIKE '%" + companyString.ToLower.Trim + "%'")
                sQuery.Append(" ORDER BY clicomp_name")

                clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "fullTextSearch.aspx.vb", sQuery.ToString)

                MySqlConn.ConnectionString = Session.Item("jetnetServerNotesDatabase")
                MySqlConn.Open()
                MySqlCommand.Connection = MySqlConn
                MySqlCommand.CommandType = CommandType.Text
                MySqlCommand.CommandTimeout = 60

                MySqlCommand.CommandText = sQuery.ToString
                MySqlReader = MySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

                Try
                    atemptable.Load(MySqlReader)
                Catch constrExc As System.Data.ConstraintException
                    Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in returnFullTextSearchResults grabClientCompany " + constrExc.Message
                End Try
            End If
        Catch ex As Exception
            atemptable = Nothing

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in grabClientCompany" + ex.Message

        Finally
            MySqlReader = Nothing

            MySqlConn.Dispose()
            MySqlConn.Close()
            MySqlConn = Nothing

            MySqlCommand.Dispose()
            MySqlCommand = Nothing
        End Try

        Return atemptable

    End Function

    Protected Function grabClientContact(ByVal contactString As String) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
        Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
        Dim MySqlReader As MySql.Data.MySqlClient.MySqlDataReader : MySqlReader = Nothing
        Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing

        Try
            If Not String.IsNullOrEmpty(contactString.Trim) Then
                sQuery.Append("SELECT 'CLIENT' as source, 0 as fts_ac_idFilter, clicontact_jetnet_contact_id as client_jetnet_contact_id, '' as fts_data_search, '' as amod_make_name, '' as amod_model_name,  '' as amod_manufacturer, 0 as fts_amod_id,")
                sQuery.Append("'' as ac_ser_no_full, '' as ac_ser_no_sort, '' as ac_reg_no, '' as ac_aport_iata_code, '' as ac_aport_icao_code, '' as ac_aport_name, '' as ac_prev_reg_no, 0 as fts_ac_id,")
                sQuery.Append(" '' as comp_name, '' as comp_city, '' as comp_address1, '' as comp_address2, '' as comp_state, '' as comp_zip_code, '' as comp_country, clicontact_comp_id as fts_comp_id,")
                sQuery.Append(" clicontact_sirname as contact_sirname, clicontact_first_name as contact_first_name, clicontact_middle_initial as contact_middle_initial, clicontact_last_name as contact_last_name, clicontact_suffix as contact_suffix, clicontact_title as contact_title, clicontact_email_address as contact_email_address, clicontact_id as fts_contact_id")
                sQuery.Append(" from client_contact ")
                sQuery.Append(" WHERE (clicontact_first_name like '%" & contactString & "%' or clicontact_last_name like '%" & contactString & "%' or clicontact_email_address like '%" & contactString & "%') ")
                sQuery.Append(" ORDER BY clicontact_first_name, clicontact_last_name")


                clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "fullTextSearch.aspx.vb", sQuery.ToString)

                MySqlConn.ConnectionString = Session.Item("jetnetServerNotesDatabase")
                MySqlConn.Open()
                MySqlCommand.Connection = MySqlConn
                MySqlCommand.CommandType = CommandType.Text
                MySqlCommand.CommandTimeout = 60

                MySqlCommand.CommandText = sQuery.ToString
                MySqlReader = MySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

                Try
                    atemptable.Load(MySqlReader)
                Catch constrExc As System.Data.ConstraintException
                    Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in returnFullTextSearchResults grabClientCompany " + constrExc.Message
                End Try
            End If
        Catch ex As Exception
            atemptable = Nothing

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in grabClientCompany" + ex.Message

        Finally
            MySqlReader = Nothing

            MySqlConn.Dispose()
            MySqlConn.Close()
            MySqlConn = Nothing

            MySqlCommand.Dispose()
            MySqlCommand = Nothing
        End Try

        Return atemptable

    End Function

    Protected Sub display_aircraft_results_table(ByRef fullTextSearchTable As DataTable, ByRef out_htmlString As String, ByVal isMobileDisplay As Boolean)

        Dim htmlOut As New StringBuilder
        Dim afiltered_Aircraft As DataRow() = Nothing
        Dim sSeparator As String = ""
        Dim nRecordCount As Integer = 0
        Dim is_only_show_client As Boolean = False
        Dim show_this_row As Boolean = False
        Dim client_count As Long = 0

        Try

            If Trim(Request("client_only")) = "Y" Then
                is_only_show_client = True
            End If

            out_htmlString = ""

            If Not IsNothing(fullTextSearchTable) Then

                afiltered_Aircraft = fullTextSearchTable.Select("fts_ac_id > 0 and ac_ser_no_full is not null", "amod_make_name, amod_model_name, ac_ser_no_sort")

                If SearchMPMData Then
                    Dim ClientTable As New DataTable
                    Dim JetnetTable As New DataTable
                    Dim ReturnTable As New DataTable
                    Dim IncludeID As String = ""

                    If afiltered_Aircraft.Length > 0 Then
                        For Each r As DataRow In afiltered_Aircraft
                            If IncludeID <> "" Then
                                IncludeID += ", "
                            End If
                            IncludeID += r.Item("fts_ac_id").ToString
                        Next
                    End If
                    'Let's go ahead and grab jetnet ac ID's and 
                    ClientTable = grabClientAircraftByJetnetID(IncludeID, 0, "")
                    JetnetTable = fullTextSearchTable.Clone

                    For Each drJetnet In afiltered_Aircraft
                        JetnetTable.ImportRow(drJetnet)
                    Next

                    CombineTwoDatatables("client_jetnet_ac_id", "fts_ac_id", ClientTable, JetnetTable, ReturnTable, "", False)
                    afiltered_Aircraft = ReturnTable.Select("fts_ac_id > 0", "amod_make_name, amod_model_name, ac_ser_no_sort")
                End If

                If afiltered_Aircraft.Length > 0 Then

                    htmlOut.Append("<table id=""fullTextAircraftDataTable"" cellpadding=""0"" cellspacing=""0"" border=""0"" width=""100%"">")
                    htmlOut.Append("<thead><tr>")

                    If isMobileDisplay = True Then
                        htmlOut.Append("<th></th>")
                    Else
                        htmlOut.Append("<th data-priority=""3"">MAKE</th>")
                        htmlOut.Append("<th data-priority=""4"">MODEL</th>")
                        htmlOut.Append("<th data-priority=""1"">SERIAL <br />NUMBER</th>")
                        htmlOut.Append("<th data-priority=""2"">REG <br />NUMBER</th>")
                        htmlOut.Append("<th data-priority=""5"">LOCATION</th>")
                        htmlOut.Append("<th data-priority=""6"">PREV <br />REG <br />NUMBER</th>")
                    End If

                    htmlOut.Append("</tr></thead><tbody>")

                    For Each r As DataRow In afiltered_Aircraft

                        show_this_row = False

                        If nRecordCount <= DISPLAY_COUNT Then

                            If is_only_show_client = True Then
                                If Trim(r("source")) = "CLIENT" Then
                                    show_this_row = True
                                    client_count = client_count + 1
                                Else
                                    show_this_row = False
                                End If
                            Else
                                show_this_row = True
                            End If

                            If show_this_row = True Then
                                htmlOut.Append("<tr ")

                                If SearchMPMData Then
                                    htmlOut.Append(" class=""" & r("source").ToString & "CRMRow""")
                                End If
                                htmlOut.Append(">")


                                If isMobileDisplay = True Then



                                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""true""><label class=""distinct"">" + r.Item("amod_make_name").ToString.Trim + " " + r.Item("amod_model_name").ToString.Trim + " S/N: ")

                                    If CRMViewActive Then  'if in the crm, keep this link 
                                        htmlOut.Append("<a class=""underline"" onclick=""javascript:window.opener.blur();window.opener.focus();window.opener.location.href='/details.aspx?type=3&ac_ID=" + r.Item("fts_ac_id").ToString + "&source=" & r.Item("source").ToString & "';"""",""AircraftDetails"");' title=""Display Aircraft Details"">")
                                        htmlOut.Append(r.Item("ac_ser_no_full").ToString + "</a>")
                                    Else
                                        htmlOut.Append(DisplayFunctions.WriteDetailsLink(r.Item("fts_ac_id"), 0, 0, 0, True, r.Item("ac_ser_no_full").ToString.Trim, "underline", ""))
                                    End If

                                    htmlOut.Append("</label>")



                                    If Not IsDBNull(r.Item("ac_reg_no")) Then
                                        If Not String.IsNullOrEmpty(r.Item("ac_reg_no")) Then
                                            htmlOut.Append("<br />Reg #: " + r.Item("ac_reg_no").ToString.Trim)
                                        End If
                                    End If

                                    If Not IsDBNull(r.Item("ac_prev_reg_no")) Then
                                        If Not String.IsNullOrEmpty(r.Item("ac_prev_reg_no")) Then
                                            If Not IsDBNull(r.Item("ac_reg_no")) Then
                                                If Not String.IsNullOrEmpty(r.Item("ac_reg_no")) Then
                                                    htmlOut.Append(" / ")
                                                Else
                                                    htmlOut.Append("<br />")
                                                End If
                                            Else
                                                htmlOut.Append("<br />")
                                            End If
                                            htmlOut.Append("Prev Reg #: " + r.Item("ac_prev_reg_no").ToString.Trim)
                                        End If
                                    End If

                                    htmlOut.Append("<br />")

                                    If Not String.IsNullOrEmpty(r.Item("ac_aport_iata_code").ToString.Trim) Then
                                        htmlOut.Append(r.Item("ac_aport_iata_code").ToString.Trim)
                                        sSeparator = " - "
                                    End If

                                    If Not String.IsNullOrEmpty(r.Item("ac_aport_icao_code").ToString.Trim) Then
                                        htmlOut.Append(sSeparator + r.Item("ac_aport_icao_code").ToString.Trim)
                                        sSeparator = " - "
                                    End If

                                    If Not String.IsNullOrEmpty(r.Item("ac_aport_name").ToString.Trim) Then
                                        htmlOut.Append(sSeparator + r.Item("ac_aport_name").ToString.Trim)
                                    End If



                                    htmlOut.Append("</td>")

                                Else

                                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""true"">" + r.Item("amod_make_name").ToString.Trim + "</td>")
                                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""true"">" + r.Item("amod_model_name").ToString.Trim + "</td>")

                                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap='nowrap' data-sort=""" & IIf(Not IsDBNull(r("ac_ser_no_sort")), r("ac_ser_no_sort"), "") & """>")  ' SERIAL NUMBER


                                    '&source=CLIENT
                                    If CRMViewActive Then
                                        htmlOut.Append("<a class=""underline"" onclick=""javascript:window.opener.blur();window.opener.focus();window.opener.location.href='/details.aspx?type=3&ac_ID=" + r.Item("fts_ac_id").ToString + "&source=" & r.Item("source").ToString & "';"""",""AircraftDetails"");' title=""Display Aircraft Details"">" & r.Item("ac_ser_no_full").ToString + "</a>")
                                    Else
                                        If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO And Session.Item("localUser").crmUser_Evo_MPM_Flag = True Then
                                            If Trim(r("source")) = "CLIENT" Then

                                                htmlOut.Append(DisplayFunctions.WriteDetailsLink(r.Item("fts_ac_id"), 0, 0, 0, True, r.Item("ac_ser_no_full").ToString.Trim, "underline", "&source=CLIENT"))

                                                ' htmlOut.Append("<a class=""underline"" onclick=""javascript:load('DisplayAircraftDetail.aspx?acid=" + r.Item("fts_ac_id").ToString + "&jid=0&source=CLIENT','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');"" title=""Display Aircraft Details"">")
                                            Else
                                                htmlOut.Append(DisplayFunctions.WriteDetailsLink(r.Item("fts_ac_id"), 0, 0, 0, True, r.Item("ac_ser_no_full").ToString.Trim, "underline", ""))
                                                'htmlOut.Append("<a class=""underline"" onclick=""javascript:load('DisplayAircraftDetail.aspx?acid=" + r.Item("fts_ac_id").ToString + "&jid=0','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');"" title=""Display Aircraft Details"">")
                                            End If
                                        Else
                                            htmlOut.Append(DisplayFunctions.WriteDetailsLink(r.Item("fts_ac_id"), 0, 0, 0, True, r.Item("ac_ser_no_full").ToString.Trim, "underline", ""))
                                            ' htmlOut.Append("<a class=""underline"" onclick=""javascript:load('DisplayAircraftDetail.aspx?acid=" + r.Item("fts_ac_id").ToString + "&jid=0','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');"" title=""Display Aircraft Details"">")
                                        End If
                                    End If

                                    htmlOut.Append("</td>")

                                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""true"">" + r.Item("ac_reg_no").ToString.Trim + "</td>")

                                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")
                                    If Not String.IsNullOrEmpty(r.Item("ac_aport_iata_code").ToString.Trim) Then
                                        htmlOut.Append(r.Item("ac_aport_iata_code").ToString.Trim)
                                        sSeparator = " - "
                                    End If

                                    If Not String.IsNullOrEmpty(r.Item("ac_aport_icao_code").ToString.Trim) Then
                                        htmlOut.Append(sSeparator + r.Item("ac_aport_icao_code").ToString.Trim)
                                        sSeparator = " - "
                                    End If

                                    If Not String.IsNullOrEmpty(r.Item("ac_aport_name").ToString.Trim) Then
                                        htmlOut.Append(sSeparator + r.Item("ac_aport_name").ToString.Trim)
                                    End If

                                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""true"">" + r.Item("ac_prev_reg_no").ToString.Trim + "</td>")

                                End If

                                htmlOut.Append("</tr>")

                                nRecordCount += 1
                            End If


                        Else

                            Exit For

                        End If


                    Next

                    htmlOut.Append("</tbody></table><div class=""Box"">")

                    If is_only_show_client = True Then
                        htmlOut.Append("<div id=""aircraftLabel"" class=""subHeader"">" & client_count & " Client Aircraft</div>")
                    Else
                        htmlOut.Append("<div id=""aircraftLabel"" class=""subHeader"">" + afiltered_Aircraft.Length.ToString + " Aircraft</div>")
                    End If

                    htmlOut.Append("<div id=""fullTextAircraftInnerTable"" align=""left"" valign=""middle"" style=""max-height:370px;overflow: auto;""></div></div>")

                End If

            End If

            out_htmlString = htmlOut.ToString

        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in display_aircraft_results_table(ByRef fullTextSearchTable As DataTable, ByRef out_htmlString As String) " + ex.Message

        Finally

            htmlOut = Nothing

        End Try

    End Sub

    Protected Sub display_company_results_table(ByRef fullTextSearchTable As DataTable, ByRef out_htmlString As String, ByVal isMobileDisplay As Boolean)

        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False
        Dim afiltered_Company As DataRow() = Nothing

        Dim compString As String = ""
        Dim arrCompanyID() As String = Nothing
        Dim nArrCount As Integer = 0
        Dim nRecordCount As Integer = 0

        Try

            out_htmlString = ""

            If Not IsNothing(fullTextSearchTable) Or SearchMPMData = True Then

                afiltered_Company = fullTextSearchTable.Select("fts_comp_id > 0 AND (fts_contact_id = 0 or fts_contact_id is NULL)", "comp_name, comp_country, comp_city, comp_state")

                If SearchMPMData Then
                    Dim ClientTable As New DataTable
                    Dim JetnetTable As New DataTable
                    Dim ReturnTable As New DataTable

                    Dim ClientCompanyTable As New DataTable

                    Dim IncludeID As String = ""

                    If afiltered_Company.Length > 0 Then
                        For Each r As DataRow In afiltered_Company
                            If IncludeID <> "" Then
                                IncludeID += ", "
                            End If
                            IncludeID += r.Item("fts_comp_id").ToString
                        Next
                    End If


                    'Let's go ahead and grab jetnet comp ID's and 
                    ClientTable = grabClientCompanyByJetnetID(IncludeID)
                    JetnetTable = fullTextSearchTable.Clone


                    For Each drJetnet In afiltered_Company
                        JetnetTable.ImportRow(drJetnet)
                    Next

                    CombineTwoDatatables("client_jetnet_comp_id", "fts_comp_id", ClientTable, JetnetTable, ReturnTable, "", False)
                    afiltered_Company = ReturnTable.Select("fts_comp_id > 0 AND fts_contact_id = 0", "comp_name, comp_country, comp_city, comp_state")

                    'ok now search the local client "company table" for this EditedSearchTerm
                    ClientCompanyTable = grabClientCompany(full_text_search_input.Text.Trim) ' switched to search by the original text 

                    CombineTwoDatatables("client_jetnet_comp_id", "fts_comp_id", ClientCompanyTable, JetnetTable, ReturnTable, "", False)
                    afiltered_Company = ReturnTable.Select("fts_comp_id > 0 AND fts_contact_id = 0", "comp_name, comp_country, comp_city, comp_state")

                End If

                If afiltered_Company.Length > 0 Then

                    htmlOut.Append("<table id=""fullTextCompanyDataTable"" cellpadding=""0"" cellspacing=""0"" border=""0"" width=""100%"">")
                    htmlOut.Append("<thead><tr>")

                    If isMobileDisplay = True Then
                        htmlOut.Append("<th></th>")
                    Else
                        htmlOut.Append("<th data-priority=""1"">NAME</th>")
                        htmlOut.Append("<th>CITY</th>")
                        htmlOut.Append("<th>STATE</th>")
                        htmlOut.Append("<th>ZIP</th>")
                        htmlOut.Append("<th>COUNTRY</th>")
                        htmlOut.Append("<th data-priority=""2"">ADDRESS</th>")
                    End If

                    htmlOut.Append("</tr></thead><tbody>")

                    For Each r As DataRow In afiltered_Company

                        If nRecordCount <= DISPLAY_COUNT Then

                            compString = r.Item("fts_comp_id").ToString

                            If Not commonEvo.inMyArray(arrCompanyID, compString) Then

                                If Not IsArray(arrCompanyID) And IsNothing(arrCompanyID) Then
                                    ReDim arrCompanyID(nArrCount)
                                Else
                                    ReDim Preserve arrCompanyID(nArrCount)
                                End If

                                ' Add CompId To Array
                                arrCompanyID(nArrCount) = compString
                                nArrCount += 1

                                htmlOut.Append("<tr ")

                                If SearchMPMData Then
                                    htmlOut.Append(" class=""" & r("source").ToString & "CRMRow""")
                                End If

                                htmlOut.Append(">")

                                If isMobileDisplay = True Then
                                    Dim Seperator As String = ""

                                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""true"">")

                                    If CRMViewActive Then
                                        htmlOut.Append("<a class=""underline distinct""  onclick=""javascript:window.opener.blur();window.opener.focus();window.opener.location.href='/details.aspx?type=1&comp_ID=" + r.Item("fts_comp_id").ToString + "&source=" & r.Item("source").ToString & "';"""",""CompanyDetails"");' title=""Display Company Details"">")
                                        htmlOut.Append(Replace(r.Item("comp_name").ToString.Trim, Constants.cSingleSpace, Constants.cHTMLnbsp) + "</a><br />")
                                    Else

                                        'Let's get rid of all that extra code and just do this.
                                        If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO And Session.Item("localUser").crmUser_Evo_MPM_Flag = True Then
                                            htmlOut.Append(DisplayFunctions.WriteDetailsLink(0, r.Item("fts_comp_id"), 0, 0, True, Replace(r.Item("comp_name").ToString.Trim, Constants.cSingleSpace, Constants.cHTMLnbsp), "underline distinct", IIf(r("source") = "CLIENT", "&source=CLIENT", "")))
                                        Else
                                            htmlOut.Append(DisplayFunctions.WriteDetailsLink(0, r.Item("fts_comp_id"), 0, 0, True, Replace(r.Item("comp_name").ToString.Trim, Constants.cSingleSpace, Constants.cHTMLnbsp), "underline distinct", ""))
                                        End If

                                        'If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO And Session.Item("localUser").crmUser_Evo_MPM_Flag = True Then
                                        '    If Trim(r("source")) = "CLIENT" Then
                                        '        htmlOut.Append("<a class=""underline distinct"" href=""/DisplayCompanyDetail.aspx?compid=" + r.Item("fts_comp_id").ToString + "&journid=0&source=CLIENT"" target=""_blank"" title=""Display Company Details"">")
                                        '    Else
                                        '        htmlOut.Append("<a class=""underline distinct"" href=""/DisplayCompanyDetail.aspx?compid=" + r.Item("fts_comp_id").ToString + "&journid=0"" target=""_blank"" title=""Display Company Details"">")
                                        '    End If
                                        'Else
                                        '    htmlOut.Append("<a class=""underline distinct"" href=""/DisplayCompanyDetail.aspx?compid=" + r.Item("fts_comp_id").ToString + "&journid=0"" target=""_blank"" title=""Display Company Details"">")
                                        'End If
                                    End If

                                    'htmlOut.Append(Replace(r.Item("comp_name").ToString.Trim, Constants.cSingleSpace, Constants.cHTMLnbsp) + "</a><br />")
                                    htmlOut.Append("<br />")
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
                                        htmlOut.Append(r.Item("comp_city").ToString.Trim & ", ")
                                    End If

                                    If Not (IsDBNull(r("comp_state"))) And Not String.IsNullOrEmpty(r.Item("comp_state").ToString.Trim) Then
                                        htmlOut.Append(r.Item("comp_state").ToString.Trim & " ")
                                    End If

                                    If Not (IsDBNull(r("comp_zip_code"))) And Not String.IsNullOrEmpty(r.Item("comp_zip_code").ToString.Trim) Then
                                        htmlOut.Append(r.Item("comp_zip_code").ToString.Trim & " ")
                                    End If

                                    If Not (IsDBNull(r("comp_country"))) And Not String.IsNullOrEmpty(r.Item("comp_country").ToString.Trim) Then
                                        htmlOut.Append(r.Item("comp_country").ToString.Trim)
                                    End If

                                    htmlOut.Append("</td>")
                                Else

                                    If CRMViewActive Then
                                        htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""true""><a class=""underline""  onclick=""javascript:window.opener.blur();window.opener.focus();window.opener.location.href='/details.aspx?type=1&comp_ID=" + r.Item("fts_comp_id").ToString + "&source=" & r.Item("source").ToString & "';"""",""CompanyDetails"");' title=""Display Company Details"">")
                                        htmlOut.Append(Replace(r.Item("comp_name").ToString.Trim, Constants.cSingleSpace, Constants.cHTMLnbsp) + "</a>")
                                    Else
                                        htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""true"">")
                                        'Let's get rid of all that extra code and just do this.
                                        If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO And Session.Item("localUser").crmUser_Evo_MPM_Flag = True Then
                                            htmlOut.Append(DisplayFunctions.WriteDetailsLink(0, r.Item("fts_comp_id"), 0, 0, True, Replace(r.Item("comp_name").ToString.Trim, Constants.cSingleSpace, Constants.cHTMLnbsp), "underline distinct", IIf(r("source") = "CLIENT", "&source=CLIENT", "")))
                                        Else
                                            htmlOut.Append(DisplayFunctions.WriteDetailsLink(0, r.Item("fts_comp_id"), 0, 0, True, Replace(r.Item("comp_name").ToString.Trim, Constants.cSingleSpace, Constants.cHTMLnbsp), "underline distinct", ""))
                                        End If

                                        'If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO And Session.Item("localUser").crmUser_Evo_MPM_Flag = True Then
                                        '    If Trim(r("source")) = "CLIENT" Then
                                        '        htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""true""><a class=""underline"" onclick=""javascript:load('DisplayCompanyDetail.aspx?compid=" + r.Item("fts_comp_id").ToString + "&journid=0&source=CLIENT','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');"" title=""Display Company Details"">")
                                        '    Else
                                        '        htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""true""><a class=""underline"" onclick=""javascript:load('DisplayCompanyDetail.aspx?compid=" + r.Item("fts_comp_id").ToString + "&journid=0','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');"" title=""Display Company Details"">")
                                        '    End If
                                        'Else
                                        '    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""true""><a class=""underline"" onclick=""javascript:load('DisplayCompanyDetail.aspx?compid=" + r.Item("fts_comp_id").ToString + "&journid=0','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');"" title=""Display Company Details"">")
                                        'End If

                                    End If

                                    'htmlOut.Append(Replace(r.Item("comp_name").ToString.Trim, Constants.cSingleSpace, Constants.cHTMLnbsp) + "</a>")

                                    htmlOut.Append("</td>")

                                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""true"">")
                                    If Not (IsDBNull(r("comp_city"))) And Not String.IsNullOrEmpty(r.Item("comp_city").ToString.Trim) Then
                                        htmlOut.Append(r.Item("comp_city").ToString.Trim)
                                    End If
                                    htmlOut.Append("</td>")

                                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""true"">")
                                    If Not (IsDBNull(r("comp_state"))) And Not String.IsNullOrEmpty(r.Item("comp_state").ToString.Trim) Then
                                        htmlOut.Append(r.Item("comp_state").ToString.Trim)
                                    End If
                                    htmlOut.Append("</td>")

                                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""true"">")
                                    If Not (IsDBNull(r("comp_zip_code"))) And Not String.IsNullOrEmpty(r.Item("comp_zip_code").ToString.Trim) Then
                                        htmlOut.Append(r.Item("comp_zip_code").ToString.Trim)
                                    End If
                                    htmlOut.Append("</td>")

                                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""true"">")
                                    If Not (IsDBNull(r("comp_country"))) And Not String.IsNullOrEmpty(r.Item("comp_country").ToString.Trim) Then
                                        htmlOut.Append(r.Item("comp_country").ToString.Trim)
                                    End If
                                    htmlOut.Append("</td>")

                                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")
                                    If Not (IsDBNull(r("comp_address1"))) And Not String.IsNullOrEmpty(r.Item("comp_address1").ToString.Trim) Then
                                        htmlOut.Append(r.Item("comp_address1").ToString.Trim)
                                    End If

                                    If Not (IsDBNull(r("comp_address2"))) And Not String.IsNullOrEmpty(r.Item("comp_address2").ToString.Trim) Then
                                        htmlOut.Append(" " + r.Item("comp_address2").ToString.Trim)
                                    End If

                                    htmlOut.Append("</td>")
                                End If

                                htmlOut.Append("</tr>")

                                nRecordCount += 1

                            End If

                        Else

                            Exit For

                        End If

                    Next

                    htmlOut.Append("</tbody></table><div class=""Box"">")
                    htmlOut.Append("<div id=""companyLabel"" class=""subHeader"">" + afiltered_Company.Length.ToString + " Companies</div>")
                    htmlOut.Append("<div id=""fullTextCompanyInnerTable"" align=""left"" valign=""middle"" style=""max-height:370px; overflow: auto;""></div></div>")

                End If

            End If

            out_htmlString = htmlOut.ToString

        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in display_company_results_table(ByRef fullTextSearchTable As DataTable, ByRef out_htmlString As String) " + ex.Message

        Finally

            htmlOut = Nothing

        End Try

    End Sub

    Protected Sub display_yacht_results_table(ByRef fullTextSearchTable As DataTable, ByRef out_htmlString As String, ByVal isMobileDisplay As Boolean)

        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False
        Dim afiltered_Yacht As DataRow() = Nothing
        Dim nRecordCount As Integer = 0

        Try

            out_htmlString = ""

            If Not IsNothing(fullTextSearchTable) Then

                afiltered_Yacht = fullTextSearchTable.Select("fts_yacht_id > 0", "yt_yacht_name")

                If afiltered_Yacht.Length > 0 Then

                    htmlOut.Append("<table id=""fullTextYachtDataTable"" cellpadding=""0"" cellspacing=""0"" border=""0"" width=""100%"">")
                    htmlOut.Append("<thead><tr>")

                    If isMobileDisplay Then
                        htmlOut.Append("<th></th>")
                    Else
                        htmlOut.Append("<th>BRAND/MODEL</th>")
                        htmlOut.Append("<th>NAME</th>")
                        htmlOut.Append("<th>HULL #</th>")
                        htmlOut.Append("<th>MFR</th>")
                    End If
                    htmlOut.Append("</tr></thead><tbody>")

                    For Each r As DataRow In afiltered_Yacht
                        Dim Seperator As String = ""
                        If nRecordCount <= DISPLAY_COUNT Then

                            htmlOut.Append("<tr> ")

                            If isMobileDisplay Then

                                htmlOut.Append("<td align=""left"" valign=""top"">")

                                If Not (IsDBNull(r("ym_brand_name"))) And Not String.IsNullOrEmpty(r.Item("ym_brand_name").ToString.Trim) Then
                                    htmlOut.Append(r.Item("ym_brand_name").ToString.Trim)
                                End If
                                htmlOut.Append("/")
                                If Not (IsDBNull(r("ym_model_name"))) And Not String.IsNullOrEmpty(r.Item("ym_model_name").ToString.Trim) Then
                                    htmlOut.Append(r.Item("ym_model_name").ToString.Trim)
                                End If
                                htmlOut.Append("</td>")

                            Else


                                htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

                                If Not (IsDBNull(r("ym_brand_name"))) And Not String.IsNullOrEmpty(r.Item("ym_brand_name").ToString.Trim) Then
                                    htmlOut.Append(r.Item("ym_brand_name").ToString.Trim)
                                End If
                                htmlOut.Append("/")
                                If Not (IsDBNull(r("ym_model_name"))) And Not String.IsNullOrEmpty(r.Item("ym_model_name").ToString.Trim) Then
                                    htmlOut.Append(r.Item("ym_model_name").ToString.Trim)
                                End If
                                htmlOut.Append("</td>")
                                htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap""><a class=""underline"" onclick=""javascript:load('/DisplayYachtDetail.aspx?jid=0&yid=" + r.Item("fts_yacht_id").ToString + "','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');"" title=""Display Yacht Details"">")

                                If Not String.IsNullOrEmpty(r.Item("yt_yacht_name").ToString) Then
                                    htmlOut.Append(Constants.cSingleSpace + r.Item("yt_yacht_name").ToString.Trim)
                                End If
                                htmlOut.Append("</a></td>")

                                htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

                                If Not (IsDBNull(r("yt_hull_mfr_nbr"))) And Not String.IsNullOrEmpty(r.Item("yt_hull_mfr_nbr").ToString.Trim) Then
                                    htmlOut.Append(r.Item("yt_hull_mfr_nbr").ToString.Trim)
                                End If

                                htmlOut.Append("</td>")

                                htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

                                If Not (IsDBNull(r("yt_year_mfr"))) And Not String.IsNullOrEmpty(r.Item("yt_year_mfr").ToString.Trim) Then
                                    htmlOut.Append(r.Item("yt_year_mfr").ToString.Trim)
                                End If

                                htmlOut.Append("</td>")
                            End If

                            htmlOut.Append("</tr>")

                            nRecordCount += 1

                        Else

                            Exit For

                        End If

                    Next

                    htmlOut.Append("</tbody></table><div class=""Box"">")
                    htmlOut.Append("<div id=""yachtLabel"" class=""subHeader"">" + afiltered_Yacht.Length.ToString + " Yachts</div>")
                    htmlOut.Append("<div id=""fullTextYachtInnerTable"" align=""left"" valign=""middle"" style=""max-height:370px; overflow: auto;""></div></div>")

                End If

            End If

            out_htmlString = htmlOut.ToString

        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in display_contact_yacht_table(ByRef fullTextSearchTable As DataTable, ByRef out_htmlString As String) " + ex.Message

        Finally

            htmlOut = Nothing

        End Try

    End Sub

    Protected Sub display_contact_results_table(ByRef fullTextSearchTable As DataTable, ByRef out_htmlString As String, ByVal isMobileDisplay As Boolean)

        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False
        Dim afiltered_Contact As DataRow() = Nothing
        Dim nRecordCount As Integer = 0
        Dim ClientContactTable As New DataTable

        Try

            out_htmlString = ""

            If Not IsNothing(fullTextSearchTable) Then

                afiltered_Contact = fullTextSearchTable.Select("fts_contact_id > 0 and contact_last_name is not NULL", "contact_last_name, contact_first_name, contact_title")

                If SearchMPMData Then
                    Dim ClientTable As New DataTable
                    Dim JetnetTable As New DataTable
                    Dim ReturnTable As New DataTable
                    Dim IncludeID As String = ""

                    If afiltered_Contact.Length > 0 Then
                        For Each r As DataRow In afiltered_Contact
                            If IncludeID <> "" Then
                                IncludeID += ", "
                            End If
                            IncludeID += r.Item("fts_contact_id").ToString
                        Next
                    End If
                    'Let's go ahead and grab jetnet ac ID's and 
                    ClientTable = grabClientContactByJetnetID(IncludeID, full_text_search_input.Text.Trim)
                    JetnetTable = fullTextSearchTable.Clone

                    For Each drJetnet In afiltered_Contact
                        JetnetTable.ImportRow(drJetnet)
                    Next

                    CombineTwoDatatables("client_jetnet_contact_id", "fts_contact_id", ClientTable, JetnetTable, ReturnTable, "", False)
                    afiltered_Contact = ReturnTable.Select("fts_contact_id > 0", "contact_last_name, contact_first_name, contact_title")

                End If

                If afiltered_Contact.Length > 0 Then

                    htmlOut.Append("<table id=""fullTextContactDataTable"" cellpadding=""0"" cellspacing=""0"" border=""0"" width=""100%"">")
                    htmlOut.Append("<thead><tr>")

                    If isMobileDisplay Then
                        htmlOut.Append("<th></th>")
                    Else
                        htmlOut.Append("<th>NAME</th>")
                        htmlOut.Append("<th>TITLE</th>")
                        htmlOut.Append("<th>EMAIL</th>")
                        htmlOut.Append("<th>COMPANY</th>")
                        htmlOut.Append("<th>CITY</th>")
                        htmlOut.Append("<th>STATE</th>")
                        htmlOut.Append("<th>ZIP</th>")
                        htmlOut.Append("<th>COUNTRY</th>")
                        htmlOut.Append("<th>ADDRESS</th>")
                    End If
                    htmlOut.Append("</tr></thead><tbody>")

                    For Each r As DataRow In afiltered_Contact
                        Dim Seperator As String = ""
                        If nRecordCount <= DISPLAY_COUNT Then

                            htmlOut.Append("<tr ")

                            If SearchMPMData Then
                                htmlOut.Append(" class=""" & r("source").ToString & "CRMRow""")
                            End If
                            htmlOut.Append(">")

                            If isMobileDisplay Then

                                htmlOut.Append("<td align=""left"" valign=""top"">")
                                If CRMViewActive Then
                                    htmlOut.Append("<a class=""underline distinct""  onclick=""javascript:window.opener.blur();window.opener.focus();window.opener.location.href='/details.aspx?type=1&contact_ID=" + r.Item("fts_contact_id").ToString + "&comp_ID=" + r.Item("fts_comp_id").ToString + "&source=" & r.Item("source").ToString & "';"""" title=""Display Contact Details"">")
                                Else
                                    If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO And Session.Item("localUser").crmUser_Evo_MPM_Flag = True Then
                                        If Trim(r("source")) = "CLIENT" Then
                                            htmlOut.Append("<a class=""underline distinct"" href=""/DisplayContactDetail.aspx?compid=" + r.Item("fts_comp_id").ToString + "&jid=0&conid=" + r.Item("fts_contact_id").ToString + "&source=CLIENT"" target=""_blank"" title=""Display Contact Details"">")
                                        Else
                                            htmlOut.Append("<a class=""underline distinct"" href=""/DisplayContactDetail.aspx?compid=" + r.Item("fts_comp_id").ToString + "&jid=0&conid=" + r.Item("fts_contact_id").ToString + """ target=""_blank"" title=""Display Contact Details"">")
                                        End If
                                    Else
                                        htmlOut.Append("<a class=""underline distinct"" href=""/DisplayContactDetail.aspx?compid=" + r.Item("fts_comp_id").ToString + "&jid=0&conid=" + r.Item("fts_contact_id").ToString + """ target=""_blank"" title=""Display Contact Details"">")
                                    End If
                                End If


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
                                    htmlOut.Append("<br />" & r.Item("contact_title").ToString.Trim)
                                End If

                                If Not (IsDBNull(r("contact_email_address"))) And Not String.IsNullOrEmpty(r.Item("contact_email_address").ToString.Trim) Then
                                    htmlOut.Append("<br /><a href=""mailto:" & r.Item("contact_email_address").ToString.Trim & """>" & r.Item("contact_email_address").ToString.Trim & "</a>")
                                End If


                                If Not (IsDBNull(r("comp_name"))) And Not String.IsNullOrEmpty(r.Item("comp_name").ToString.Trim) Then
                                    htmlOut.Append("<br />" & r.Item("comp_name").ToString.Trim & "")
                                End If

                                If Not (IsDBNull(r("comp_address1"))) And Not String.IsNullOrEmpty(r.Item("comp_address1").ToString.Trim) Then
                                    htmlOut.Append("<br />" & r.Item("comp_address1").ToString.Trim)
                                    Seperator = "<br />"
                                End If

                                If Not (IsDBNull(r("comp_address2"))) And Not String.IsNullOrEmpty(r.Item("comp_address2").ToString.Trim) Then
                                    htmlOut.Append(" " + r.Item("comp_address2").ToString.Trim)
                                    Seperator = "<br />"
                                End If

                                htmlOut.Append(Seperator)
                                Seperator = ""

                                If Not (IsDBNull(r("comp_city"))) And Not String.IsNullOrEmpty(r.Item("comp_city").ToString.Trim) Then
                                    htmlOut.Append(r.Item("comp_city").ToString.Trim & ", ")
                                End If

                                If Not (IsDBNull(r("comp_state"))) And Not String.IsNullOrEmpty(r.Item("comp_state").ToString.Trim) Then
                                    htmlOut.Append(r.Item("comp_state").ToString.Trim & " ")
                                End If

                                If Not (IsDBNull(r("comp_zip_code"))) And Not String.IsNullOrEmpty(r.Item("comp_zip_code").ToString.Trim) Then
                                    htmlOut.Append(r.Item("comp_zip_code").ToString.Trim & " ")
                                End If

                                If Not (IsDBNull(r("comp_country"))) And Not String.IsNullOrEmpty(r.Item("comp_country").ToString.Trim) Then
                                    htmlOut.Append(r.Item("comp_country").ToString.Trim)
                                End If

                                htmlOut.Append("</td>")

                            Else

                                If CRMViewActive Then
                                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap""><a class=""underline""  onclick=""javascript:window.opener.blur();window.opener.focus();window.opener.location.href='/details.aspx?type=1&contact_ID=" + r.Item("fts_contact_id").ToString + "&comp_ID=" + r.Item("fts_comp_id").ToString + "&source=" & r.Item("source").ToString & "';"""" title=""Display Contact Details"">")
                                Else
                                    If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO And Session.Item("localUser").crmUser_Evo_MPM_Flag = True Then
                                        If Trim(r("source")) = "CLIENT" Then
                                            htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap""><a class=""underline"" href=""/DisplayContactDetail.aspx?compid=" + r.Item("fts_comp_id").ToString + "&jid=0&conid=" + r.Item("fts_contact_id").ToString + "&source=CLIENT"" target=""_blank"" title=""Display Contact Details"">")
                                        Else
                                            htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap""><a class=""underline"" href=""/DisplayContactDetail.aspx?compid=" + r.Item("fts_comp_id").ToString + "&jid=0&conid=" + r.Item("fts_contact_id").ToString + """ target=""_blank"" title=""Display Contact Details"">")
                                        End If
                                    Else
                                        htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap""><a class=""underline"" href=""/DisplayContactDetail.aspx?compid=" + r.Item("fts_comp_id").ToString + "&jid=0&conid=" + r.Item("fts_contact_id").ToString + """ target=""_blank"" title=""Display Contact Details"">")
                                    End If

                                End If
                                htmlOut.Append(r.Item("contact_sirname").ToString.Trim + Constants.cSingleSpace + r.Item("contact_first_name").ToString.Trim + Constants.cSingleSpace)

                                If Not String.IsNullOrEmpty(r.Item("contact_middle_initial").ToString) Then
                                    htmlOut.Append(r.Item("contact_middle_initial").ToString.Trim + ". ")
                                End If

                                htmlOut.Append(r.Item("contact_last_name").ToString.Trim)

                                If Not String.IsNullOrEmpty(r.Item("contact_suffix").ToString) Then
                                    htmlOut.Append(Constants.cSingleSpace + r.Item("contact_suffix").ToString.Trim)
                                End If
                                htmlOut.Append("</a></td>")

                                htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")
                                If Not (IsDBNull(r("contact_title"))) And Not String.IsNullOrEmpty(r.Item("contact_title").ToString.Trim) Then
                                    htmlOut.Append(r.Item("contact_title").ToString.Trim)
                                End If
                                htmlOut.Append("</td>")

                                htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""true"">")
                                If Not (IsDBNull(r("contact_email_address"))) And Not String.IsNullOrEmpty(r.Item("contact_email_address").ToString.Trim) Then
                                    htmlOut.Append(r.Item("contact_email_address").ToString.Trim)
                                End If
                                htmlOut.Append("</td>")

                                htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""true"">")
                                If Not (IsDBNull(r("comp_name"))) And Not String.IsNullOrEmpty(r.Item("comp_name").ToString.Trim) Then
                                    htmlOut.Append(r.Item("comp_name").ToString.Trim)
                                End If
                                htmlOut.Append("</td>")

                                htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""true"">")
                                If Not (IsDBNull(r("comp_city"))) And Not String.IsNullOrEmpty(r.Item("comp_city").ToString.Trim) Then
                                    htmlOut.Append(r.Item("comp_city").ToString.Trim)
                                End If
                                htmlOut.Append("</td>")

                                htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""true"">")
                                If Not (IsDBNull(r("comp_state"))) And Not String.IsNullOrEmpty(r.Item("comp_state").ToString.Trim) Then
                                    htmlOut.Append(r.Item("comp_state").ToString.Trim)
                                End If
                                htmlOut.Append("</td>")

                                htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""true"">")
                                If Not (IsDBNull(r("comp_zip_code"))) And Not String.IsNullOrEmpty(r.Item("comp_zip_code").ToString.Trim) Then
                                    htmlOut.Append(r.Item("comp_zip_code").ToString.Trim)
                                End If
                                htmlOut.Append("</td>")

                                htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""true"">")
                                If Not (IsDBNull(r("comp_country"))) And Not String.IsNullOrEmpty(r.Item("comp_country").ToString.Trim) Then
                                    htmlOut.Append(r.Item("comp_country").ToString.Trim)
                                End If
                                htmlOut.Append("</td>")

                                htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")
                                If Not (IsDBNull(r("comp_address1"))) And Not String.IsNullOrEmpty(r.Item("comp_address1").ToString.Trim) Then
                                    htmlOut.Append(r.Item("comp_address1").ToString.Trim)
                                End If

                                If Not (IsDBNull(r("comp_address2"))) And Not String.IsNullOrEmpty(r.Item("comp_address2").ToString.Trim) Then
                                    htmlOut.Append(" " + r.Item("comp_address2").ToString.Trim)
                                End If

                                htmlOut.Append("</td>")
                            End If

                            htmlOut.Append("</tr>")

                            nRecordCount += 1

                        Else

                            Exit For

                        End If

                    Next

                    htmlOut.Append("</tbody></table><div class=""Box"">")
                    htmlOut.Append("<div id=""contactLabel"" class=""subHeader"">" + afiltered_Contact.Length.ToString + " Contacts</div>")
                    htmlOut.Append("<div id=""fullTextContactInnerTable"" align=""left"" valign=""middle"" style=""max-height:370px; overflow: auto;""></div></div>")

                End If

            End If

            out_htmlString = htmlOut.ToString

        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in display_contact_results_table(ByRef fullTextSearchTable As DataTable, ByRef out_htmlString As String) " + ex.Message

        Finally

            htmlOut = Nothing

        End Try

    End Sub

    Protected Sub display_model_results_table(ByRef fullTextSearchTable As DataTable, ByRef out_htmlString As String, ByVal isMobileDisplay As Boolean)

        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False
        Dim afiltered_Model As DataRow() = Nothing
        Dim nRecordCount As Integer = 0


        Try

            out_htmlString = ""

            If Not IsNothing(fullTextSearchTable) Then

                afiltered_Model = fullTextSearchTable.Select("fts_amod_id > 0 AND (fts_ac_id is NULL or fts_ac_id = 0) and (fts_ac_idFilter is NULL or fts_ac_idFilter = 0) and amod_make_name is not NULL ", "amod_make_name, amod_model_name")

                If afiltered_Model.Length > 0 Then

                    htmlOut.Append("<table id=""fullTextModelDataTable"" cellpadding=""0"" cellspacing=""0"" border=""0"" width=""100%"">")
                    htmlOut.Append("<thead><tr>")
                    If isMobileDisplay = False Then
                        htmlOut.Append("<th>MAKE</th>")
                        htmlOut.Append("<th>MODEL</th>")
                        htmlOut.Append("<th>MANUFACTURER</th>")
                        If Session.Item("localSubscription").crmAerodexFlag = False Then
                            htmlOut.Append("<th>FOR SALE</th>")
                        End If
                    Else
                        htmlOut.Append("<th></th>")
                    End If

                    htmlOut.Append("</tr></thead>")
                    htmlOut.Append("<tbody>")

                    For Each r As DataRow In afiltered_Model

                        If nRecordCount <= DISPLAY_COUNT Then

                            htmlOut.Append("<tr>")


                            If isMobileDisplay = True Then

                                htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""true"">")

                                If CRMViewActive Then
                                    'view_template.aspx?ViewID=1&ViewName=&amod_id=272
                                    htmlOut.Append("<a class=""underline distinct"" href=""/view_template.aspx?ViewID=1&ViewName=&amod_id=" + r.Item("fts_amod_id").ToString + """ target=""_blank"" title=""Display Model Details"">" + r.Item("amod_make_name").ToString.Trim + "</a> ")
                                    htmlOut.Append("<a class=""underline distinct"" href=""/view_template.aspx?ViewID=1&ViewName=&amod_id=" + r.Item("fts_amod_id").ToString + """ target=""_blank"" title=""Display Model Details"">" + r.Item("amod_model_name").ToString.Trim + "</a>")
                                Else
                                    htmlOut.Append("<a class=""underline distinct"" href=""/view_template.aspx?noMaster=false&ViewID=1&ViewName=Model%20Market%20Summary&amod_id=" + r.Item("fts_amod_id").ToString + """ target=""_blank"" title=""Display Model Details"">" + r.Item("amod_make_name").ToString.Trim + "</a> ")
                                    htmlOut.Append("<a class=""underline distinct"" href=""/view_template.aspx?noMaster=false&ViewID=1&ViewName=Model%20Market%20Summary&amod_id=" + r.Item("fts_amod_id").ToString + """ target=""_blank"" title=""Display Model Details"">" + r.Item("amod_model_name").ToString.Trim + "</a>")
                                End If

                                htmlOut.Append("<br />")
                                If Not (IsDBNull(r("amod_manufacturer"))) And Not String.IsNullOrEmpty(r.Item("amod_manufacturer").ToString.Trim) Then
                                    htmlOut.Append(r.Item("amod_manufacturer").ToString.Trim)
                                    htmlOut.Append("<br />")
                                End If


                                If Session.Item("localSubscription").crmAerodexFlag = False Then
                                    htmlOut.Append("<a class=""underline"" href=""/view_template.aspx?noMaster=false&ViewID=11&ViewName=" + Server.UrlEncode("Model Market List") + "&amod_id=" + r.Item("fts_amod_id").ToString + """ target=""_blank"" title=""Display FOR SALE Model "">")
                                    htmlOut.Append("View For Sale</a>")
                                End If

                                htmlOut.Append("</td>")
                            Else

                                'Model/Make
                                htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""true"">")
                                If CRMViewActive Then
                                    'view_template.aspx?ViewID=1&ViewName=&amod_id=272
                                    htmlOut.Append("<a class=""underline"" href=""/view_template.aspx?ViewID=1&ViewName=&amod_id=" + r.Item("fts_amod_id").ToString + """ target=""_blank"" title=""Display Model Details"">" + r.Item("amod_make_name").ToString.Trim + "</a> ")
                                    htmlOut.Append("</td>")
                                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""true"">")
                                    htmlOut.Append("<a class=""underline"" href=""/view_template.aspx?ViewID=1&ViewName=&amod_id=" + r.Item("fts_amod_id").ToString + """ target=""_blank"" title=""Display Model Details"">" + r.Item("amod_model_name").ToString.Trim + "</a>")
                                Else
                                    htmlOut.Append("<a class=""underline"" href=""/view_template.aspx?noMaster=false&ViewID=1&ViewName=Model%20Market%20Summary&amod_id=" + r.Item("fts_amod_id").ToString + """ target=""_blank"" title=""Display Model Details"">" + r.Item("amod_make_name").ToString.Trim + "</a> ")
                                    htmlOut.Append("</td>")
                                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""true"">")
                                    htmlOut.Append("<a class=""underline"" href=""/view_template.aspx?noMaster=false&ViewID=1&ViewName=Model%20Market%20Summary&amod_id=" + r.Item("fts_amod_id").ToString + """ target=""_blank"" title=""Display Model Details"">" + r.Item("amod_model_name").ToString.Trim + "</a>")
                                End If
                                htmlOut.Append("</td>")

                                'Manufacturer
                                htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""true"">")
                                If Not (IsDBNull(r("amod_manufacturer"))) And Not String.IsNullOrEmpty(r.Item("amod_manufacturer").ToString.Trim) Then
                                    htmlOut.Append(r.Item("amod_manufacturer").ToString.Trim)
                                End If
                                htmlOut.Append("</td>")
                                'For Sale
                                If Session.Item("localSubscription").crmAerodexFlag = False Then
                                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""true"">")
                                    htmlOut.Append("<a class=""underline"" href=""/view_template.aspx?noMaster=false&ViewID=11&ViewName=" + Server.UrlEncode("Model Market List") + "&amod_id=" + r.Item("fts_amod_id").ToString + """ target=""_blank"" title='Display FOR SALE Model'>")
                                    htmlOut.Append("View</a>")
                                    htmlOut.Append("</td>")
                                End If

                            End If
                            htmlOut.Append("</tr>")

                            nRecordCount += 1

                        Else

                            Exit For

                        End If

                    Next

                    htmlOut.Append("</tbody></table><div class=""Box"">")

                    htmlOut.Append("<div id=""modelLabel"" class=""subHeader"">" + afiltered_Model.Length.ToString + " Models</div>")

                    htmlOut.Append("<div id=""fullTextModelInnerTable"" align=""left"" valign=""middle"" style=""max-height:370px; overflow: auto;""></div></div>")

                End If

            End If

            out_htmlString = htmlOut.ToString

        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in display_model_results_table(ByRef fullTextSearchTable As DataTable, ByRef out_htmlString As String) " + ex.Message

        Finally

            htmlOut = Nothing

        End Try

    End Sub
    ''' <summary>
    ''' A generic function that will merge the jetnet/client table together, excluding either the client IDs in the table or the
    ''' optional parameter of IDs that you send it.
    ''' </summary>
    ''' <param name="ClientTable"></param>
    ''' <param name="JetnetTable"></param>
    ''' <param name="ReturnTable"></param>
    ''' <param name="FullClientIDsToExclude"></param>
    ''' <param name="UseFullClientIDs"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CombineTwoDatatables(ByVal uniqueFieldName As String, ByVal jetnetFieldString As String, ByRef ClientTable As DataTable, ByRef JetnetTable As DataTable, ByRef ReturnTable As DataTable, ByRef FullClientIDsToExclude As String, ByRef UseFullClientIDs As Boolean) As String
        Dim column As New DataColumn 'Column to Add Source to jetnet data.
        Dim column2 As New DataColumn 'Column to Add id to jetnet data. To match client side datatable 
        Dim IDsToExclude As String = ""

        'Going ahead to add the Source to the Jetnet Datatable, let's you know what type of data you're displaying. 
        Try

            If Not IsNothing(JetnetTable) Then
                If Not JetnetTable.Columns.Contains("source") Then
                    column.DataType = System.Type.GetType("System.String")
                    column.DefaultValue = "JETNET"
                    column.Unique = False
                    column.ColumnName = "source"
                    JetnetTable.Columns.Add(column)
                End If

                If JetnetTable.Columns.Contains(uniqueFieldName) Then
                    column2.DataType = System.Type.GetType("System.Int64")
                    column2.DefaultValue = 0
                    column2.Unique = False
                    column2.ColumnName = uniqueFieldName
                    JetnetTable.Columns.Add(column2)
                End If
            End If


            'First we need to loop through the client data to get a list for our not in statement on the jetnet side.
            If UseFullClientIDs Then
                IDsToExclude = FullClientIDsToExclude
            Else
                For Each drRow As DataRow In ClientTable.Rows
                    If IDsToExclude <> "" Then
                        IDsToExclude += ", "
                    End If
                    IDsToExclude += drRow(uniqueFieldName).ToString
                Next
                IDsToExclude = IDsToExclude
            End If
            'First we copy the Client data. This allows the return table to have
            'The Client Data In it.
            ReturnTable = JetnetTable.Clone

            For Each drClient As DataRow In ClientTable.Rows
                ReturnTable.ImportRow(drClient)
            Next

            If IDsToExclude <> "" Then
                Dim afiltered_Jetnet As DataRow() = JetnetTable.Select(jetnetFieldString & " not in (" & IDsToExclude & ") ", "")
                For Each drJetnet In afiltered_Jetnet
                    ReturnTable.ImportRow(drJetnet)
                Next
            Else
                'Nothing to exclude, so we go ahead and import the jetnet data as is.
                If Not IsNothing(JetnetTable) Then
                    For Each drRow As DataRow In JetnetTable.Rows
                        ReturnTable.ImportRow(drRow)
                    Next
                End If
            End If

        Catch ex As Exception

        End Try

        Return IDsToExclude

    End Function

    Public Shared Function strClean(ByVal keywordToClean As String)
        Dim DoNotSearchList As New ArrayList
        Dim ReturnString As String = ""
        Try
            keywordToClean = Replace(keywordToClean, "#", "")
            keywordToClean = Replace(keywordToClean, "&", "")

            DoNotSearchList.Add("of")
            DoNotSearchList.Add("the")

            Dim strOccurrence As String = ""
            Dim SplitSearchArray As String() = Split(keywordToClean, " ")
            For i As Integer = 0 To SplitSearchArray.Count - 1

                For Each strOccurrence In DoNotSearchList
                    If Not String.IsNullOrEmpty(SplitSearchArray(i)) Then
                        If UCase(SplitSearchArray(i).ToString()) = UCase(strOccurrence) Then
                            SplitSearchArray(i) = Replace(UCase(SplitSearchArray(i).ToString()), UCase(strOccurrence), "")
                        End If
                    End If
                Next

                If SplitSearchArray(i) <> "" Then
                    If ReturnString <> "" Then
                        ReturnString += " "
                    End If
                    ReturnString += SplitSearchArray(i)
                End If

            Next
        Catch ex As Exception

        Finally
            strClean = ReturnString
        End Try

    End Function

End Class