' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/staticFolderEditor.aspx.vb $
'$$Author: Mike $
'$$Date: 6/17/20 7:33p $
'$$Modtime: 6/17/20 7:23p $
'$$Revision: 8 $
'$$Workfile: staticFolderEditor.aspx.vb $
'
' ********************************************************************************

Partial Public Class staticFolderEditor
  Inherits System.Web.UI.Page

  Dim CRMViewActive As Boolean = False
  Const DISPLAY_COUNT As Integer = 500
  Private sTask As String = ""
  Public nFolderID As Integer = 0
  Public bRefreshPreferences As Boolean = False
  Public bFromHome As Boolean = False
  Dim bIsMobile As Boolean = False

  Public Shared masterPage As New Object

  'Private Sub staticFolderEditor_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
  '  If save_folder_edits.Visible Then
  '    System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "FolderRender", "$(document).ready(function(){showRemoveButton();});", True)
  '  End If
  'End Sub

  Private Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

    Try

      If Not IsNothing(HttpContext.Current.Session.Item("isMobile")) Then
        If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("isMobile").ToString.Trim) Then
          bIsMobile = CBool(HttpContext.Current.Session.Item("isMobile").ToString.ToLower.Trim)
        End If
      End If

      If bIsMobile Then
        Me.MasterPageFile = "~/EvoStyles/MobileTheme.master"
        masterPage = DirectCast(Page.Master, MobileTheme)
      Else
        Me.MasterPageFile = "~/EvoStyles/EmptyEvoTheme.master"
        masterPage = DirectCast(Page.Master, EmptyEvoTheme)
      End If

      If Not IsNothing(Session.Item("jetnetAppVersion")) Then
        If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CRM Then
          CRMViewActive = True
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

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    Dim sErrorString As String = ""
    Dim itemList As String = ""
    Dim searchList As String = ""

    Dim selectedFolderRows As String = ""
    Dim selectedSearchRows As String = ""

    Dim bSaveFolderEdits As Boolean = False
    Dim bQuickSearch As Boolean = False
    Dim bSaveQuickSearchResults As Boolean = False
    Dim bAirportFolderEdit As Boolean = False
    Dim bAircraftFolderEdit As Boolean = False
    Dim bDefaultFolder As Boolean = False
    Dim bClosePage As Boolean = False

    Try

      If Session.Item("crmUserLogon") <> True Then

        commonLogFunctions.forceLogError("ERROR", "Evolution Static Folder Editor")

        Response.Redirect("Default.aspx", False)

      Else

        masterPage.SetPageTitle("Folder List Manager")

        If Not IsPostBack Then

          If Not Session.Item("localPreferences").loadUserSession(sErrorString, CLng(Session.Item("localUser").crmSubSubID.ToString), Session.Item("localUser").crmUserLogin.ToString, CLng(Session.Item("localUser").crmSubSeqNo.ToString), CLng(Session.Item("localUser").crmUserContactID.ToString)) Then
            Response.Write("error in load staticFolderEditor : " + sErrorString)
          End If


        End If

        searchTable.Text = ""

        If Not IsNothing(Request.Item("airport")) Then
          If Not String.IsNullOrEmpty(Request.Item("airport").ToString.Trim) Then
            bAirportFolderEdit = CBool(Request.Item("airport").ToString.ToLower.Trim)
            quick_search_input.Attributes.Add("placeholder", "Airport Name, City, IATA, ICAO")
            quick_search_input.Attributes.Add("style", "padding:5px;height:15px;")
            quick_search_input.TextMode = TextBoxMode.MultiLine
            searchCodes.Visible = True
            searchCodes.Attributes.Add("onchange", "if ($('#" & searchCodes.ClientID & "').is(':checked')) {$('#" & quick_search_input.ClientID & "').attr('placeholder', 'Please List Airport Code(s) Separated By Commas.');$('#" & quick_search_input.ClientID & "').height('100')} else {$('#" & quick_search_input.ClientID & "').attr('placeholder', 'Airport Name, City, IATA, ICAO');$('#" & quick_search_input.ClientID & "').height('15')};")
          End If
        End If

        If Not IsNothing(Request.QueryString("aircraft")) Then
          If Not String.IsNullOrEmpty(Request.QueryString("aircraft").ToString.Trim) Then
            bAircraftFolderEdit = CBool(Request.QueryString("aircraft").ToString.ToLower.Trim)
            quick_search_input.Attributes.Add("placeholder", "Make, Model, SN, Reg")
            search_reg_nos.Visible = True
          End If
        End If

        If Not IsNothing(Request.Item("default")) Then
          If Not String.IsNullOrEmpty(Request.Item("default").ToString.Trim) Then
            bDefaultFolder = CBool(Request.Item("default").ToString.ToLower.Trim)
          End If
        End If

        If Not IsNothing(Request.Item("fromHome")) Then
          If Not String.IsNullOrEmpty(Request.Item("fromHome").ToString.Trim) Then
            bFromHome = CBool(Request.Item("fromHome").ToString.ToLower.Trim)
          End If
        End If

        If Not IsPostBack Then
          fill_location_box_inner()
        End If

        If Not IsNothing(Request.Item("folderID")) Then
          If Not String.IsNullOrEmpty(Request.Item("folderID").ToString.Trim) Then
            nFolderID = CInt(Request.Item("folderID").ToString.ToUpper.Trim)

            reset_page.Visible = True
            reset_page.PostBackUrl = "~/staticFolderEditor.aspx?folderID=" + nFolderID.ToString + IIf(bAircraftFolderEdit, "&aircraft=true", "") + IIf(bAirportFolderEdit, "&airport=true", "") + IIf(bDefaultFolder, "&default=true", "") + IIf(bFromHome, "&fromHome=true", "")

            save_folder_edits.PostBackUrl = "~/staticFolderEditor.aspx?task=saveEdits&folderID=" + nFolderID.ToString + IIf(bAircraftFolderEdit, "&aircraft=true", "") + IIf(bAirportFolderEdit, "&airport=true", "") + IIf(bDefaultFolder, "&default=true", "") + IIf(bFromHome, "&fromHome=true", "")
            quick_search_button.PostBackUrl = "~/staticFolderEditor.aspx?task=qSearch&folderID=" + nFolderID.ToString + IIf(bAircraftFolderEdit, "&aircraft=true", "") + IIf(bAirportFolderEdit, "&airport=true", "") + IIf(bDefaultFolder, "&default=true", "") + IIf(bFromHome, "&fromHome=true", "")
            save_quick_search_results_button.PostBackUrl = "~/staticFolderEditor.aspx?task=saveSearch&folderID=" + nFolderID.ToString + IIf(bAircraftFolderEdit, "&aircraft=true", "") + IIf(bAirportFolderEdit, "&airport=true", "") + IIf(bDefaultFolder, "&default=true", "") + IIf(bFromHome, "&fromHome=true", "")

            save_quick_search_results_button.Text = "<strong>Save Selections to " + lbl_static_folder_name.Text.Trim + "</strong>"

            If bAirportFolderEdit Then
              Session.Item("searchCriteria").SearchViewCriteriaAirportDropdown = 4
              Session.Item("searchCriteria").SearchViewCriteriaAirportFolderName = lbl_static_folder_name.Text.Trim
              Session.Item("searchCriteria").SearchViewCriteriaAirportDropdown2 = nFolderID
            End If
          Else
            reset_page.Visible = False
          End If
        Else
          reset_page.Visible = False
        End If

        If Not IsNothing(Request.Item("task")) Then
          If Not String.IsNullOrEmpty(Request.Item("task").ToString.Trim) Then
            sTask = Request.Item("task").ToString.ToUpper.Trim
          End If
        End If

        If sTask.ToLower.Contains("closepage") Then
          bClosePage = True
        End If

        If sTask.ToLower.Contains("saveedits") And nFolderID > 0 Then
          If Not String.IsNullOrEmpty(selected_folder_rows.Text.Trim) Then
            selectedFolderRows = selected_folder_rows.Text
            If bAirportFolderEdit Or bAircraftFolderEdit Then
              HttpContext.Current.Session.Item("currentFolderData") = selectedFolderRows
            End If
          End If
          reset_page.Visible = False
          bSaveFolderEdits = True
        End If

        If sTask.ToLower.Contains("qsearch") Then
          bQuickSearch = True
        End If

        If sTask.ToLower.Contains("savesearch") Then

          If Not String.IsNullOrEmpty(selected_quick_search_rows.Text.Trim) Then
            selectedSearchRows = selected_quick_search_rows.Text
          End If

          If Not String.IsNullOrEmpty(selected_folder_rows.Text.Trim) Then
            selectedFolderRows = selected_folder_rows.Text
            If bAirportFolderEdit Or bAircraftFolderEdit Then
              HttpContext.Current.Session.Item("currentFolderData") = selectedFolderRows
            End If
          End If

          bSaveQuickSearchResults = True
        End If

        If bSaveFolderEdits And nFolderID > 0 And Not String.IsNullOrEmpty(selectedFolderRows.Trim) Or bClosePage Then

          If Not bClosePage Then

            If bAirportFolderEdit Then
              updateStaticAirportFolderContents(nFolderID, selectedFolderRows, False)
            ElseIf bAircraftFolderEdit Then
              updateStaticAircraftFolderContents(nFolderID, selectedFolderRows, False)
            Else
              updateStaticFolderContents(nFolderID, selectedFolderRows, False)
            End If

          End If

          If (bAirportFolderEdit And bDefaultFolder) Then
            bRefreshPreferences = True
            Dim javascript As String = ""
            javascript = " window.onload = function() {closeAndRefreshParent()};"
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "ParseFormScript", javascript, True)
          Else
            Dim booleanString As String = ""
            Dim javascriptFunction As String = "ParseForm"
            booleanString = "false,false,true,false, false,"

            Dim javascript As String = "if (window.opener.location.pathname.toUpperCase().search('COMPANY_LISTING') == 1){" + javascriptFunction + "('" + nFolderID.ToString + "'," + booleanString + "'" + Replace(selectedFolderRows, "'", "\'") + "');} else {window.opener.location.reload(true);}"

            javascript = " window.onload = function() {" + javascript + ";self.close();};"
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "ParseFormScript", javascript, True)
          End If

        ElseIf bSaveQuickSearchResults And nFolderID > 0 And Not String.IsNullOrEmpty(selectedSearchRows.Trim) Then

          If bAirportFolderEdit Then
            updateStaticAirportFolderContents(nFolderID, selectedSearchRows, True)
          ElseIf bAircraftFolderEdit Then
            updateStaticAircraftFolderContents(nFolderID, selectedSearchRows, True)
          Else
            updateStaticFolderContents(nFolderID, selectedSearchRows, True)
          End If

          quick_search_input.Text = ""
          save_quick_search_results_button.Visible = False
          tabContainer.ActiveTabIndex = 0
          searchTable.Text = ""
          bQuickSearch = False

          Dim javascript As String = " window.onload = function() {window.opener.location.reload(true);}"
          System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "ReloadPage", javascript, True)

        ElseIf bSaveQuickSearchResults And nFolderID = 0 And Not String.IsNullOrEmpty(selectedSearchRows.Trim) Then

          If bAirportFolderEdit Then
            nFolderID = saveQuickSearchAirportContents(folderName.Text.Trim, selectedSearchRows, bDefaultFolder)
          ElseIf bAircraftFolderEdit Then
            nFolderID = saveQuickSearchAircraftContents(folderName.Text.Trim, selectedSearchRows, bDefaultFolder)
          Else
            nFolderID = saveQuickSearchContents(folderName.Text.Trim, selectedSearchRows, bDefaultFolder)
          End If

          quick_search_input.Text = ""
          save_quick_search_results_button.Visible = False
          tabContainer.ActiveTabIndex = 0
          searchTable.Text = ""
          bQuickSearch = False

          Dim javascript As String = " window.onload = function() {window.opener.location.reload(true);}"
          System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "ReloadPage", javascript, True)

        End If

        If nFolderID > 0 Then

          Dim results_table As New DataTable

          If bAirportFolderEdit Then
            results_table = returnStaticAirportFolderContents(nFolderID)
            display_airport_results_table(results_table, itemList, bIsMobile)
          ElseIf bAircraftFolderEdit Then
            results_table = returnStaticFolderContents(nFolderID)
            display_aircraft_results_table(results_table, itemList, bIsMobile)
          Else
            results_table = returnStaticFolderContents(nFolderID)
            display_company_results_table(results_table, itemList, bIsMobile)
          End If

          If Not String.IsNullOrEmpty(itemList.Trim) Then
            folderTable.Text = itemList

            If bAirportFolderEdit Then

              close_button_new.OnClientClick = ""
              close_button_new.PostBackUrl = "~/staticFolderEditor.aspx?task=closepage"

              System.Web.UI.ScriptManager.RegisterStartupScript(Me.tabPanel1, Me.tabPanel1.GetType(), "CreateAirportTable", "$(document).ready(function() { CreateTheDatatable('airportInnerTable','airportDataTable','airportjQueryTable'); });", True)

            ElseIf bAircraftFolderEdit Then
              System.Web.UI.ScriptManager.RegisterStartupScript(Me.tabPanel1, Me.tabPanel1.GetType(), "CreateAircraftTable", "$(document).ready(function() { CreateTheDatatable('aircraftInnerTable','aircraftDataTable','aircraftjQueryTable'); });", True)
            Else
              System.Web.UI.ScriptManager.RegisterStartupScript(Me.tabPanel1, Me.tabPanel1.GetType(), "CreateCompanyTable", "$(document).ready(function() { CreateTheDatatable('companyInnerTable','companyDataTable','companyjQueryTable'); });", True)
            End If

          Else
            folderTable.Text = "<table id=""folderResultsOuterTable"" cellpadding=""2"" cellspacing=""0"">"
            folderTable.Text += "<tr><td align=""left"" valign=""middle""><em>No Folder Results To Display</em></td></tr>"
            folderTable.Text += "</table>"

            lbl_static_folder_name.Text = IIf(String.IsNullOrEmpty(lbl_static_folder_name.Text.Trim), "No Folder Results", lbl_static_folder_name.Text.Trim)

            tabContainer.ActiveTabIndex = 1

          End If


          'Edited on 9/13/2017. This runs when a folder is opened. It sets one of these groups of 3 session variables that are used only on the utilization view.
          'All these do is mark your session as looking at this particular folder so that on refresh it will go ahead and select them. I have already checked
          'about it setting those outside of a view scenario
          If bAirportFolderEdit = True Then
            Session.Item("searchCriteria").SearchViewCriteriaAirportDropdown = 4
            Session("Last_Aport_ID") = 0
            Session.Item("searchCriteria").SearchViewCriteriaAirportDropdown2 = nFolderID
            Session.Item("searchCriteria").SearchViewCriteriaAirportFolderName = lbl_static_folder_name.Text.Trim
          ElseIf bAircraftFolderEdit = True Then
            Session.Item("searchCriteria").SearchViewCriteriaAircraftDropdown = 4
            Session.Item("searchCriteria").SearchViewCriteriaAircraftDropdown2 = nFolderID
            Session.Item("searchCriteria").SearchViewCriteriaAircraftFolderName = lbl_static_folder_name.Text.Trim
          Else
            Session.Item("searchCriteria").SearchViewCriteriaOperatorDropdown = 4
            Session.Item("searchCriteria").SearchViewCriteriaOperatorDropdown2 = nFolderID
            Session.Item("searchCriteria").SearchViewCriteriaOperatorFolderName = lbl_static_folder_name.Text.Trim
          End If

        End If

        If bQuickSearch Then

          Dim searchPhrase As String = quick_search_input.Text.Trim
          Dim results_table As New DataTable

          If bAirportFolderEdit Then
            results_table = returnQuickSearchAirportResults(searchPhrase)
            display_airport_qsearch_results_table(results_table, searchList, bIsMobile)
          ElseIf bAircraftFolderEdit Then
            results_table = returnQuickSearchAircraftResults(searchPhrase)
            display_aircraft_qsearch_results_table(results_table, searchList, bIsMobile)
          Else
            results_table = returnQuickSearchResults(searchPhrase)
            display_qsearch_results_table(results_table, searchList, bIsMobile)
          End If

          If nFolderID = 0 And String.IsNullOrEmpty(folderName.Text.Trim) Then
            folderName.Visible = True
          End If

          If Not String.IsNullOrEmpty(searchList.Trim) Then

            save_quick_search_results_button.Visible = True

            searchTable.Text = searchList

            If bAirportFolderEdit Then
              System.Web.UI.ScriptManager.RegisterStartupScript(Me.tabPanel1, Me.tabPanel1.GetType(), "CreateAirportQsearchTable", "$(document).ready(function() { CreateTheDatatable_Clean('airportQsearchInnerTable','airportQsearchDataTable','airportQsearchjQueryTable'); });", True)
            ElseIf bAircraftFolderEdit Then
              System.Web.UI.ScriptManager.RegisterStartupScript(Me.tabPanel1, Me.tabPanel1.GetType(), "CreateAircraftQsearchTable", "$(document).ready(function() { CreateTheDatatable_Clean('aircraftQsearchInnerTable','aircraftQsearchDataTable','aircraftQsearchjQueryTable'); });", True)
            Else
              System.Web.UI.ScriptManager.RegisterStartupScript(Me.tabPanel2, Me.tabPanel2.GetType(), "CreateQuickSearchTable", "$(document).ready(function() { CreateTheDatatable_Clean('qsearchInnerTable','qsearchDataTable','qsearchjQueryTable'); });", True)
            End If

          Else

            searchTable.Text = "<table id=""searchResultsOuterTable"" cellpadding=""2"" cellspacing=""0"">"
            searchTable.Text += "<tr><td align=""left"" valign=""middle""><em>No Results To Display</em></td></tr>"
            searchTable.Text += "</table>"

          End If

        End If

        If sTask.ToLower.Contains("newstaticfolder") Then

          If bAirportFolderEdit Then
            lbl_static_folder_name.Text = "New Static Airport Folder"
          ElseIf bAirportFolderEdit Then
            lbl_static_folder_name.Text = "New Static Aircraft Folder"
          Else
            lbl_static_folder_name.Text = "New Static Folder"
          End If

          tabContainer.ActiveTabIndex = 1

        End If

      End If

    Catch ex As Exception
      If Not IsNothing(masterPage) Then
        masterPage.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString + " : " + ex.Message.ToString.Trim)
      Else
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodInfo.GetCurrentMethod().ToString + "</b><br />" + ex.Message.ToString.Trim
      End If
    Finally

    End Try

  End Sub

  Public Sub fill_location_Box(ByVal sender As Object, ByVal e As System.EventArgs) Handles continent_or_region.SelectedIndexChanged
    Call fill_location_box_inner()
  End Sub

  Public Sub fill_location_box_inner()


    ' ADDED IN MSW - 9/7/18 
    Dim aclsData_Temp As New clsData_Manager_SQL
    Dim temp_location_table As New DataTable
    aclsData_Temp.JETNET_DB = HttpContext.Current.Session.Item("jetnetClientDatabase")
    aclsData_Temp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase")

    If Me.continent_or_region.SelectedValue = "R" Then
      temp_location_table = aclsData_Temp.Get_Jetnet_Regions()
    Else
      temp_location_table = aclsData_Temp.Get_Jetnet_Continents()
    End If

    quick_search_location_box.Items.Clear()
    quick_search_location_box.Items.Add("")
    If Not IsNothing(temp_location_table) Then
      If temp_location_table.Rows.Count > 0 Then
        For Each r As DataRow In temp_location_table.Rows
          If Me.continent_or_region.SelectedValue = "R" Then
            quick_search_location_box.Items.Add(r.Item("geographic_region_name"))
          Else
            quick_search_location_box.Items.Add(r.Item("continent_name"))
          End If
        Next
      End If
    End If


  End Sub

  Protected Sub display_company_results_table(ByRef folderTable As DataTable, ByRef out_htmlString As String, ByVal isMobileDisplay As Boolean)

    Dim htmlOut As New StringBuilder
    Dim toggleRowColor As Boolean = False

    Dim dataString As String = ""

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand

    Dim _dataTable As New DataTable
    Dim _recordSet As System.Data.SqlClient.SqlDataReader : _recordSet = Nothing

    Dim sQuery As New StringBuilder()

    Try

      out_htmlString = ""

      If Not IsNothing(folderTable) Then

        SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

        SqlConn.Open()

        SqlCommand.Connection = SqlConn
        SqlCommand.CommandType = System.Data.CommandType.Text
        SqlCommand.CommandTimeout = 90

        If folderTable.Rows.Count > 0 Then

          For Each r As DataRow In folderTable.Rows

            lbl_static_folder_name.Text = r.Item("cfolder_name").ToString.Replace(Constants.cSingleQuote, Constants.cEmptyString).ToUpper

            dataString = r.Item("cfolder_data").ToString

            HttpContext.Current.Session.Item("currentFolderData") = dataString

          Next

          Dim tmpArray(1) As String

          If Not String.IsNullOrEmpty(dataString.Trim) Then
            tmpArray = dataString.Split(Constants.cEq.Trim)
          End If

          If Not IsNothing(tmpArray(0)) And Not IsNothing(tmpArray(1)) Then

            If Not String.IsNullOrEmpty(tmpArray(0).Trim) And Not String.IsNullOrEmpty(tmpArray(1).Trim) Then

              If tmpArray(0).Trim.ToLower.Contains("therealsearchquery") Then
                Exit Sub
              End If

              sQuery.Append("SELECT DISTINCT * FROM Company WHERE " + tmpArray(0).Trim + " IN (")
              sQuery.Append(tmpArray(1).Trim)
              sQuery.Append(") AND comp_journ_id = 0 ORDER BY comp_name ASC")

              HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />display_company_results_table()<br />" + sQuery.ToString

              SqlCommand.CommandText = sQuery.ToString
              _recordSet = SqlCommand.ExecuteReader()

              Try
                _dataTable.Load(_recordSet)
              Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = _dataTable.GetErrors()
              End Try

              _recordSet.Close()
              _recordSet = Nothing

              If _dataTable.Rows.Count > 0 Then

                htmlOut.Append("<table id=""companyDataTable"" cellpadding=""0"" cellspacing=""0"" border=""0"" width=""100%"">")
                htmlOut.Append("<thead><tr>")
                htmlOut.Append("<th><span class=""help_cursor"" title=""Used to select and remove companies from the list"">SEL</span></th>")

                If isMobileDisplay Then
                  htmlOut.Append("<th></th>")
                  htmlOut.Append("<th></th>")
                Else
                  htmlOut.Append("<th></th>")
                  htmlOut.Append("<th data-priority=""1"">NAME</th>")
                  htmlOut.Append("<th>CITY</th>")
                  htmlOut.Append("<th>STATE</th>")
                  htmlOut.Append("<th>ZIP</th>")
                  htmlOut.Append("<th>COUNTRY</th>")
                  htmlOut.Append("<th data-priority=""2"">ADDRESS</th>")
                End If

                htmlOut.Append("</tr></thead><tbody>")

                For Each r As DataRow In _dataTable.Rows

                  htmlOut.Append("<tr>")

                  If isMobileDisplay Then

                    Dim Seperator As String = ""

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">&nbsp;</td>")
                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">" + r.Item("comp_id").ToString + "</td>")
                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

                    'If CRMViewActive Then
                    '  htmlOut.Append("<a class=""underline""  onclick=""javascript:window.opener.blur();window.opener.focus();window.opener.location.href='/details.aspx?type=1&comp_ID=" + r.Item("fts_comp_id").ToString + "&source=" & r.Item("source").ToString & "';"""",""CompanyDetails"");' title=""Display Company Details"">")
                    'Else
                    htmlOut.Append("<a class=""underline"" onclick='javascript:openSmallWindowJS(""DisplayCompanyDetail.aspx?compid=" + r.Item("comp_id").ToString + "&journid=0"",""CompanyDetails"");' title=""Display Company Details"">")
                    'End If

                    htmlOut.Append(Replace(r.Item("comp_name").ToString.Trim, Constants.cSingleSpace, Constants.cHTMLnbsp) + "</a><br />")

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

                    htmlOut.Append("</td>")

                  Else

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">&nbsp;</td>")
                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">" + r.Item("comp_id").ToString + "</td>")

                    'If CRMViewActive Then
                    '  htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap""><a class=""underline""  onclick=""javascript:window.opener.blur();window.opener.focus();window.opener.location.href='/details.aspx?type=1&comp_ID=" + r.Item("fts_comp_id").ToString + "&source=" & r.Item("source").ToString & "';"""",""CompanyDetails"");' title=""Display Company Details"">")
                    'Else
                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap""><a class=""underline"" onclick='javascript:openSmallWindowJS(""DisplayCompanyDetail.aspx?compid=" + r.Item("comp_id").ToString + "&journid=0"",""CompanyDetails"");' title=""Display Company Details"">")
                    'End If

                    htmlOut.Append(Replace(r.Item("comp_name").ToString.Trim, Constants.cSingleSpace, Constants.cHTMLnbsp) + "</a>")

                    htmlOut.Append("</td>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")
                    If Not (IsDBNull(r("comp_city"))) And Not String.IsNullOrEmpty(r.Item("comp_city").ToString.Trim) Then
                      htmlOut.Append(r.Item("comp_city").ToString.Trim)
                    End If
                    htmlOut.Append("</td>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")
                    If Not (IsDBNull(r("comp_state"))) And Not String.IsNullOrEmpty(r.Item("comp_state").ToString.Trim) Then
                      htmlOut.Append(r.Item("comp_state").ToString.Trim)
                    End If
                    htmlOut.Append("</td>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")
                    If Not (IsDBNull(r("comp_zip_code"))) And Not String.IsNullOrEmpty(r.Item("comp_zip_code").ToString.Trim) Then
                      htmlOut.Append(r.Item("comp_zip_code").ToString.Trim)
                    End If
                    htmlOut.Append("</td>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")
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

                  End If ' isMobileDisplay Then

                  htmlOut.Append("</tr>")

                Next

              End If ' _dataTable.Rows.Count > 0 Then

              htmlOut.Append("</tbody></table>")
              htmlOut.Append("<div id=""companyLabel"" class="""" style=""padding:2px;""><strong>" + _dataTable.Rows.Count.ToString + " Companies</strong></div>")
              htmlOut.Append("<div id=""companyInnerTable"" align=""left"" valign=""middle"" style=""max-height:470px; overflow: auto;""></div>")

            End If ' Not String.IsNullOrEmpty(tmpArray(0).Trim) And Not String.IsNullOrEmpty(tmpArray(1).Trim) Then

          End If ' Not IsNothing(tmpArray(0).Trim) And Not IsNothing(tmpArray(1).Trim) Then

        End If ' folderTable.Rows.Count > 0 Then

      End If ' Not IsNothing(folderTable) Then

      out_htmlString = htmlOut.ToString

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in display_company_results_table(ByRef folderTable As DataTable, ByRef out_htmlString As String, ByVal isMobileDisplay As Boolean) " + ex.Message

    Finally

      htmlOut = Nothing

    End Try

  End Sub

  Protected Sub display_qsearch_results_table(ByRef searchTable As DataTable, ByRef out_htmlString As String, ByVal isMobileDisplay As Boolean)

    Dim htmlOut As New StringBuilder
    Dim toggleRowColor As Boolean = False

    Dim compString As String = ""
    Dim arrCompanyID() As String = Nothing
    Dim nArrCount As Integer = 0

    Try

      out_htmlString = ""

      If Not IsNothing(searchTable) Then

        If searchTable.Rows.Count > 0 Then

          htmlOut.Append("<table id=""qsearchDataTable"" cellpadding=""0"" cellspacing=""0"" border=""0"" width=""100%"">")
          htmlOut.Append("<thead><tr>")
          htmlOut.Append("<th><span class=""help_cursor"" title=""Used to select and remove companies from the list"">SEL</span></th>")

          If isMobileDisplay = True Then
            htmlOut.Append("<th></th>")
            htmlOut.Append("<th></th>")
          Else
            htmlOut.Append("<th></th>")
            htmlOut.Append("<th data-priority=""1"">NAME</th>")
            htmlOut.Append("<th>CITY</th>")
            htmlOut.Append("<th>STATE</th>")
            htmlOut.Append("<th>ZIP</th>")
            htmlOut.Append("<th>COUNTRY</th>")
            htmlOut.Append("<th data-priority=""2"">ADDRESS</th>")
          End If

          htmlOut.Append("</tr></thead><tbody>")

          For Each r As DataRow In searchTable.Rows

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

              htmlOut.Append("<tr>")

              If isMobileDisplay Then

                Dim Seperator As String = ""

                htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">&nbsp;</td>")
                htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">" + r.Item("fts_comp_id").ToString + "</td>")
                htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

                'If CRMViewActive Then
                '  htmlOut.Append("<a class=""underline""  onclick=""javascript:window.opener.blur();window.opener.focus();window.opener.location.href='/details.aspx?type=1&comp_ID=" + r.Item("fts_comp_id").ToString + "&source=" & r.Item("source").ToString & "';"""",""CompanyDetails"");' title=""Display Company Details"">")
                'Else
                htmlOut.Append("<a class=""underline"" onclick='javascript:openSmallWindowJS(""DisplayCompanyDetail.aspx?compid=" + r.Item("fts_comp_id").ToString + "&journid=0"",""CompanyDetails"");' title=""Display Company Details"">")
                'End If

                htmlOut.Append(Replace(r.Item("comp_name").ToString.Trim, Constants.cSingleSpace, Constants.cHTMLnbsp) + "</a><br />")

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

                htmlOut.Append("</td>")

              Else

                htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">&nbsp;</td>")
                htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">" + r.Item("fts_comp_id").ToString + "</td>")

                'If CRMViewActive Then
                '  htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap""><a class=""underline""  onclick=""javascript:window.opener.blur();window.opener.focus();window.opener.location.href='/details.aspx?type=1&comp_ID=" + r.Item("fts_comp_id").ToString + "&source=" & r.Item("source").ToString & "';"""",""CompanyDetails"");' title=""Display Company Details"">")
                'Else
                htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap""><a class=""underline"" onclick='javascript:openSmallWindowJS(""DisplayCompanyDetail.aspx?compid=" + r.Item("fts_comp_id").ToString + "&journid=0"",""CompanyDetails"");' title=""Display Company Details"">")
                'End If

                htmlOut.Append(Replace(r.Item("comp_name").ToString.Trim, Constants.cSingleSpace, Constants.cHTMLnbsp) + "</a>")

                htmlOut.Append("</td>")

                htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")
                If Not (IsDBNull(r("comp_city"))) And Not String.IsNullOrEmpty(r.Item("comp_city").ToString.Trim) Then
                  htmlOut.Append(r.Item("comp_city").ToString.Trim)
                End If
                htmlOut.Append("</td>")

                htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")
                If Not (IsDBNull(r("comp_state"))) And Not String.IsNullOrEmpty(r.Item("comp_state").ToString.Trim) Then
                  htmlOut.Append(r.Item("comp_state").ToString.Trim)
                End If
                htmlOut.Append("</td>")

                htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")
                If Not (IsDBNull(r("comp_zip_code"))) And Not String.IsNullOrEmpty(r.Item("comp_zip_code").ToString.Trim) Then
                  htmlOut.Append(r.Item("comp_zip_code").ToString.Trim)
                End If
                htmlOut.Append("</td>")

                htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")
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

            End If

          Next

          htmlOut.Append("</tbody></table>")
          htmlOut.Append("<div id=""qsearchLabel"" class="""" style=""padding:2px;""><strong>" + arrCompanyID.Length.ToString + " Companies</strong></div>")
          htmlOut.Append("<div id=""qsearchInnerTable"" align=""left"" valign=""middle"" style=""max-height:470px; overflow: auto;""></div>")

        End If

      End If

      out_htmlString = htmlOut.ToString

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in display_company_results_table(ByRef fullTextSearchTable As DataTable, ByRef out_htmlString As String) " + ex.Message

    Finally

      htmlOut = Nothing

    End Try

  End Sub

  Protected Sub display_aircraft_results_table(ByRef folderTable As DataTable, ByRef out_htmlString As String, ByVal isMobileDisplay As Boolean)

    Dim htmlOut As New StringBuilder
    Dim toggleRowColor As Boolean = False

    Dim dataString As String = ""

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand

    Dim _dataTable As New DataTable
    Dim _recordSet As System.Data.SqlClient.SqlDataReader : _recordSet = Nothing

    Dim sQuery As New StringBuilder()
    Dim i As Integer = 0

    Try

      out_htmlString = ""

      If Not IsNothing(folderTable) Then

        SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

        SqlConn.Open()

        SqlCommand.Connection = SqlConn
        SqlCommand.CommandType = System.Data.CommandType.Text
        SqlCommand.CommandTimeout = 90

        If folderTable.Rows.Count > 0 Then

          For Each r As DataRow In folderTable.Rows

            lbl_static_folder_name.Text = r.Item("cfolder_name").ToString.Replace(Constants.cSingleQuote, Constants.cEmptyString).ToUpper

            dataString = r.Item("cfolder_data").ToString

            HttpContext.Current.Session.Item("currentFolderData") = dataString

          Next

          Dim tmpArray(1) As String

          If Not String.IsNullOrEmpty(dataString.Trim) Then
            tmpArray = dataString.Split(Constants.cEq.Trim)
          End If

          ' dont know why this is here static folders are saved in format
          ''ac_id=" + sNewContents.Replace(Constants.cMultiDelim, Constants.cCommaDelim).Trim + "'
          For i = 0 To UBound(tmpArray) - 1 ' dont do the last spot either 

            If Trim(tmpArray(i)) = "Equals!~!ac_id" And Trim(tmpArray(i + 1)) <> "ac_id" Then
              tmpArray(i) = "ac_id"
            End If

          Next

          If Not IsNothing(tmpArray(0)) And Not IsNothing(tmpArray(1)) Then

            If Not String.IsNullOrEmpty(tmpArray(0).Trim) And Not String.IsNullOrEmpty(tmpArray(1).Trim) Then

              If tmpArray(0).Trim.ToLower.Contains("therealsearchquery") Then
                Exit Sub
              End If

              sQuery.Append("SELECT DISTINCT * FROM Aircraft_Flat with (NOLOCK) WHERE " + tmpArray(0).Trim + " IN (")
              sQuery.Append(tmpArray(1).Trim)
              sQuery.Append(") AND ac_journ_id = 0 ORDER BY ac_ser_no_sort ASC")

              HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />display_aircraft_results_table()<br />" + sQuery.ToString

              SqlCommand.CommandText = sQuery.ToString
              _recordSet = SqlCommand.ExecuteReader()

              Try
                _dataTable.Load(_recordSet)
              Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = _dataTable.GetErrors()
              End Try

              _recordSet.Close()
              _recordSet = Nothing

              If _dataTable.Rows.Count > 0 Then

                htmlOut.Append("<table id=""aircraftDataTable"" cellpadding=""0"" cellspacing=""0"" border=""0"" width=""100%"">")
                htmlOut.Append("<thead><tr>")
                htmlOut.Append("<th><span class=""help_cursor"" title=""Used to select and remove aircraft from the list"">SEL</span></th>")

                If isMobileDisplay Then
                  htmlOut.Append("<th></th>")
                  htmlOut.Append("<th></th>")
                Else
                  htmlOut.Append("<th></th>")
                  htmlOut.Append("<th data-priority=""3"">MAKE</th>")
                  htmlOut.Append("<th data-priority=""4"">MODEL</th>")
                  htmlOut.Append("<th data-priority=""1"">SERIAL <br />NUMBER</th>")
                  htmlOut.Append("<th data-priority=""2"">REG <br />NUMBER</th>")
                  htmlOut.Append("<th data-priority=""5"">LOCATION</th>")
                  htmlOut.Append("<th data-priority=""6"">PREV <br />REG <br />NUMBER</th>")
                End If

                htmlOut.Append("</tr></thead><tbody>")

                Dim sSeparator As String = ""

                For Each r As DataRow In _dataTable.Rows

                  htmlOut.Append("<tr>")

                  If isMobileDisplay Then

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">&nbsp;</td>")
                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">" + r.Item("ac_id").ToString + "</td>")
                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""true""><label class=""distinct"">" + r.Item("amod_make_name").ToString.Trim + " " + r.Item("amod_model_name").ToString.Trim + " S/N: ")

                    If CRMViewActive Then
                      htmlOut.Append("<a class=""underline"" onclick=""javascript:window.opener.blur();window.opener.focus();window.opener.location.href='/details.aspx?type=3&ac_ID=" + r.Item("ac_id").ToString + "&source=" & r.Item("source").ToString & "';"""",""AircraftDetails"");' title=""Display Aircraft Details"">")
                    Else
                      htmlOut.Append("<a class=""underline"" onclick='javascript:openSmallWindowJS(""DisplayAircraftDetail.aspx?acid=" + r.Item("ac_id").ToString + "&jid=0"",""AircraftDetails"");' title=""Display Aircraft Details"">")
                    End If

                    htmlOut.Append(r.Item("ac_ser_no_full").ToString + "</a> </label>")


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

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">&nbsp;</td>")
                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">" + r.Item("ac_id").ToString + "</td>")
                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""true"">" + r.Item("amod_make_name").ToString.Trim + "</td>")
                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""true"">" + r.Item("amod_model_name").ToString.Trim + "</td>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap='nowrap' data-sort=""" + IIf(Not IsDBNull(r.Item("ac_ser_no_sort")), r.Item("ac_ser_no_sort").ToString, "") + """>")  ' SERIAL NUMBER

                    If CRMViewActive Then
                      htmlOut.Append("<a class=""underline"" onclick=""javascript:window.opener.blur();window.opener.focus();window.opener.location.href='/details.aspx?type=3&ac_ID=" + r.Item("ac_id").ToString + "&source=" + r.Item("source").ToString + "';"""",""AircraftDetails"");' title=""Display Aircraft Details"">")
                    Else
                      htmlOut.Append("<a class=""underline"" onclick='javascript:openSmallWindowJS(""DisplayAircraftDetail.aspx?acid=" + r.Item("ac_id").ToString + "&jid=0"",""AircraftDetails"");' title=""Display Aircraft Details"">")
                    End If

                    htmlOut.Append(r.Item("ac_ser_no_full").ToString + "</a></td>")

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


                  End If ' isMobileDisplay Then

                  htmlOut.Append("</tr>")

                Next

              End If ' _dataTable.Rows.Count > 0 Then

              htmlOut.Append("</tbody></table>")
              htmlOut.Append("<div id=""aircraftLabel"" class="""" style=""padding:2px;""><strong>" + _dataTable.Rows.Count.ToString + " Aircraft</strong></div>")
              htmlOut.Append("<div id=""aircraftInnerTable"" align=""left"" valign=""middle"" style=""max-height:470px; overflow: auto;""></div>")

            End If ' Not String.IsNullOrEmpty(tmpArray(0).Trim) And Not String.IsNullOrEmpty(tmpArray(1).Trim) Then

          End If ' Not IsNothing(tmpArray(0).Trim) And Not IsNothing(tmpArray(1).Trim) Then

        End If ' folderTable.Rows.Count > 0 Then

      End If ' Not IsNothing(folderTable) Then

      out_htmlString = htmlOut.ToString

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in display_aircraft_results_table(ByRef folderTable As DataTable, ByRef out_htmlString As String, ByVal isMobileDisplay As Boolean) " + ex.Message

    Finally

      htmlOut = Nothing

    End Try

  End Sub

  Protected Sub display_aircraft_qsearch_results_table(ByRef searchTable As DataTable, ByRef out_htmlString As String, ByVal isMobileDisplay As Boolean)

    Dim htmlOut As New StringBuilder
    Dim toggleRowColor As Boolean = False

    Dim acString As String = ""
    Dim arrAircraftID() As String = Nothing
    Dim nArrCount As Integer = 0

    Try

      out_htmlString = ""

      If Not IsNothing(searchTable) Then

        If searchTable.Rows.Count > 0 Then

          htmlOut.Append("<table id=""aircraftQsearchDataTable"" cellpadding=""0"" cellspacing=""0"" border=""0"" width=""100%"">")
          htmlOut.Append("<thead><tr>")
          htmlOut.Append("<th><span class=""help_cursor"" title=""Used to select and remove aircraft from the list"">SEL</span></th>")

          If isMobileDisplay = True Then
            htmlOut.Append("<th></th>")
            htmlOut.Append("<th></th>")
          Else
            htmlOut.Append("<th></th>")
            htmlOut.Append("<th data-priority=""3"">MAKE</th>")
            htmlOut.Append("<th data-priority=""4"">MODEL</th>")
            htmlOut.Append("<th data-priority=""1"">SERIAL <br />NUMBER</th>")
            htmlOut.Append("<th data-priority=""2"">REG <br />NUMBER</th>")
            htmlOut.Append("<th data-priority=""5"">LOCATION</th>")
            htmlOut.Append("<th data-priority=""6"">PREV <br />REG <br />NUMBER</th>")
          End If

          htmlOut.Append("</tr></thead><tbody>")

          Dim sSeparator As String = ""

          For Each r As DataRow In searchTable.Rows

            acString = r.Item("fts_ac_id").ToString

            If Not commonEvo.inMyArray(arrAircraftID, acString) Then

              If Not IsArray(arrAircraftID) And IsNothing(arrAircraftID) Then
                ReDim arrAircraftID(nArrCount)
              Else
                ReDim Preserve arrAircraftID(nArrCount)
              End If

              ' Add CompId To Array
              arrAircraftID(nArrCount) = acString
              nArrCount += 1

              htmlOut.Append("<tr>")

              If isMobileDisplay Then

                htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">&nbsp;</td>")
                htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">" + r.Item("fts_ac_id").ToString + "</td>")
                htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""true""><label class=""distinct"">" + r.Item("amod_make_name").ToString.Trim + " " + r.Item("amod_model_name").ToString.Trim + " S/N: ")

                If CRMViewActive Then
                  htmlOut.Append("<a class=""underline"" onclick=""javascript:window.opener.blur();window.opener.focus();window.opener.location.href='/details.aspx?type=3&ac_ID=" + r.Item("fts_ac_id").ToString + "&source=" & r.Item("source").ToString & "';"""",""AircraftDetails"");' title=""Display Aircraft Details"">")
                Else
                  htmlOut.Append("<a class=""underline"" onclick='javascript:openSmallWindowJS(""DisplayAircraftDetail.aspx?acid=" + r.Item("fts_ac_id").ToString + "&jid=0"",""AircraftDetails"");' title=""Display Aircraft Details"">")
                End If

                htmlOut.Append(r.Item("ac_ser_no_full").ToString + "</a> </label>")

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

                htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">&nbsp;</td>")
                htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">" + r.Item("fts_ac_id").ToString + "</td>")

                htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""true"">" + r.Item("amod_make_name").ToString.Trim + "</td>")
                htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""true"">" + r.Item("amod_model_name").ToString.Trim + "</td>")

                htmlOut.Append("<td align=""left"" valign=""middle"" nowrap='nowrap' data-sort=""" & IIf(Not IsDBNull(r("ac_ser_no_sort")), r("ac_ser_no_sort"), "") & """>")  ' SERIAL NUMBER

                If CRMViewActive Then
                  htmlOut.Append("<a class=""underline"" onclick=""javascript:window.opener.blur();window.opener.focus();window.opener.location.href='/details.aspx?type=3&ac_ID=" + r.Item("fts_ac_id").ToString + "&source=" & r.Item("source").ToString & "';"""",""AircraftDetails"");' title=""Display Aircraft Details"">")
                Else
                  htmlOut.Append("<a class=""underline"" onclick='javascript:openSmallWindowJS(""DisplayAircraftDetail.aspx?acid=" + r.Item("fts_ac_id").ToString + "&jid=0"",""AircraftDetails"");' title=""Display Aircraft Details"">")
                End If

                htmlOut.Append(r.Item("ac_ser_no_full").ToString + "</a></td>")

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

            End If

          Next

          htmlOut.Append("</tbody></table>")
          htmlOut.Append("<div id=""aircraftQsearchLabel"" class="""" style=""padding:2px;""><strong>" + arrAircraftID.Length.ToString + " Aircraft</strong></div>")
          htmlOut.Append("<div id=""aircraftQsearchInnerTable"" align=""left"" valign=""middle"" style=""max-height:470px; overflow: auto;""></div>")

        End If

      End If

      out_htmlString = htmlOut.ToString

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in display_aircraft_qsearch_results_table(ByRef searchTable As DataTable, ByRef out_htmlString As String, ByVal isMobileDisplay As Boolean) " + ex.Message

    Finally

      htmlOut = Nothing

    End Try

  End Sub

  Protected Sub display_airport_results_table(ByRef searchTable As DataTable, ByRef out_htmlString As String, ByVal isMobileDisplay As Boolean)

    Dim htmlOut As New StringBuilder
    Dim toggleRowColor As Boolean = False

    Dim dataString As String = ""

    Dim airportString As String = ""
    Dim arrAirportID() As String = Nothing
    Dim nArrCount As Integer = 0

    Try

      out_htmlString = ""

      If Not IsNothing(searchTable) Then

        Dim folderInfo As DataTable = returnStaticFolderContents(nFolderID)

        lbl_static_folder_name.Text = folderInfo.Rows(0).Item("cfolder_name").ToString.Replace(Constants.cSingleQuote, Constants.cEmptyString).ToUpper

        dataString = folderInfo.Rows(0).Item("cfolder_data").ToString

        HttpContext.Current.Session.Item("currentFolderData") = dataString

        folderInfo = Nothing

        If searchTable.Rows.Count > 0 Then


          htmlOut.Append("<table id=""airportDataTable"" cellpadding=""0"" cellspacing=""0"" border=""0"" width=""100%"">")
          htmlOut.Append("<thead><tr>")
          htmlOut.Append("<th><span class=""help_cursor"" title=""Used to select and remove Airports from the list"">SEL</span></th>")

          If isMobileDisplay = True Then
            htmlOut.Append("<th></th>")
            htmlOut.Append("<th></th>")
          Else
            htmlOut.Append("<th></th>")
            htmlOut.Append("<th data-priority=""1"">NAME</th>")
            htmlOut.Append("<th>CITY</th>")
            htmlOut.Append("<th>STATE</th>")
            htmlOut.Append("<th data-priority=""2"">COUNTRY</th>")
            htmlOut.Append("<th>IATA</th>")
            htmlOut.Append("<th>ICAO</th>")
          End If

          htmlOut.Append("</tr></thead><tbody>")

          For Each r As DataRow In searchTable.Rows

            airportString = r.Item("aport_id").ToString

            If Not commonEvo.inMyArray(arrAirportID, airportString) Then

              If Not IsArray(arrAirportID) And IsNothing(arrAirportID) Then
                ReDim arrAirportID(nArrCount)
              Else
                ReDim Preserve arrAirportID(nArrCount)
              End If

              ' Add CompId To Array
              arrAirportID(nArrCount) = airportString
              nArrCount += 1

              htmlOut.Append("<tr>")

              If isMobileDisplay Then

                Dim Seperator As String = ""

                htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">&nbsp;</td>")
                htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">" + r.Item("aport_id").ToString + "</td>")
                htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

                htmlOut.Append(Replace(r.Item("aport_name").ToString.Trim, Constants.cSingleSpace, Constants.cHTMLnbsp) + "<br />")

                If Not (IsDBNull(r("aport_city"))) And Not String.IsNullOrEmpty(r.Item("aport_city").ToString.Trim) Then
                  htmlOut.Append(r.Item("aport_city").ToString.Trim + ", ")
                  Seperator = "<br />"
                End If

                If Not (IsDBNull(r("aport_state"))) And Not String.IsNullOrEmpty(r.Item("aport_state").ToString.Trim) Then
                  htmlOut.Append(" " + r.Item("aport_state").ToString.Trim)
                End If

                If Not (IsDBNull(r("aport_country"))) And Not String.IsNullOrEmpty(r.Item("aport_country").ToString.Trim) Then
                  htmlOut.Append(" " + r.Item("aport_country").ToString.Trim)
                End If

                htmlOut.Append(Seperator)
                Seperator = ""


                If Not (IsDBNull(r("aport_iata_code"))) And Not String.IsNullOrEmpty(r.Item("aport_iata_code").ToString.Trim) Then
                  htmlOut.Append(r.Item("aport_iata_code").ToString.Trim + " ")
                End If

                If Not (IsDBNull(r("aport_icao_code"))) And Not String.IsNullOrEmpty(r.Item("aport_icao_code").ToString.Trim) Then
                  htmlOut.Append(r.Item("aport_icao_code").ToString.Trim)
                End If

                htmlOut.Append("</td>")

              Else

                htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">&nbsp;</td>")
                htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">" + r.Item("aport_id").ToString + "</td>")

                htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")
                htmlOut.Append(Replace(r.Item("aport_name").ToString.Trim, Constants.cSingleSpace, Constants.cHTMLnbsp))
                htmlOut.Append("</td>")

                htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")
                If Not (IsDBNull(r("aport_city"))) And Not String.IsNullOrEmpty(r.Item("aport_city").ToString.Trim) Then
                  htmlOut.Append(r.Item("aport_city").ToString.Trim)
                End If
                htmlOut.Append("</td>")

                htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")
                If Not (IsDBNull(r("aport_state"))) And Not String.IsNullOrEmpty(r.Item("aport_state").ToString.Trim) Then
                  htmlOut.Append(r.Item("aport_state").ToString.Trim)
                End If
                htmlOut.Append("</td>")

                htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")
                If Not (IsDBNull(r("aport_country"))) And Not String.IsNullOrEmpty(r.Item("aport_country").ToString.Trim) Then
                  htmlOut.Append(r.Item("aport_country").ToString.Trim)
                End If
                htmlOut.Append("</td>")

                htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")
                If Not (IsDBNull(r("aport_iata_code"))) And Not String.IsNullOrEmpty(r.Item("aport_iata_code").ToString.Trim) Then
                  htmlOut.Append(r.Item("aport_iata_code").ToString.Trim)
                End If
                htmlOut.Append("</td>")

                htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")
                If Not (IsDBNull(r("aport_icao_code"))) And Not String.IsNullOrEmpty(r.Item("aport_icao_code").ToString.Trim) Then
                  htmlOut.Append(r.Item("aport_icao_code").ToString.Trim)
                End If

                htmlOut.Append("</td>")

              End If

              htmlOut.Append("</tr>")

            End If

          Next

          htmlOut.Append("</tbody></table>")
          htmlOut.Append("<div id=""airportLabel"" class="""" style=""padding:2px;""><strong>" + arrAirportID.Length.ToString + " Airport(s)</strong></div>")
          htmlOut.Append("<div id=""airportInnerTable"" align=""left"" valign=""middle"" style=""max-height:470px; overflow: auto;""></div>")

        End If

      End If

      out_htmlString = htmlOut.ToString

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in display_airport_results_table(ByRef searchTable As DataTable, ByRef out_htmlString As String, ByVal isMobileDisplay As Boolean) " + ex.Message

    Finally

      htmlOut = Nothing

    End Try

  End Sub

  Protected Sub display_airport_qsearch_results_table(ByRef searchTable As DataTable, ByRef out_htmlString As String, ByVal isMobileDisplay As Boolean)

    Dim htmlOut As New StringBuilder
    Dim toggleRowColor As Boolean = False

    Dim airportString As String = ""
    Dim arrAirportID() As String = Nothing
    Dim nArrCount As Integer = 0

    Try

      out_htmlString = ""

      If Not IsNothing(searchTable) Then

        If searchTable.Rows.Count > 0 Then

          htmlOut.Append("<table id=""airportQsearchDataTable"" cellpadding=""0"" cellspacing=""0"" border=""0"" width=""100%"">")
          htmlOut.Append("<thead><tr>")
          htmlOut.Append("<th><span class=""help_cursor"" title=""Used to select and remove Airports from the list"">SEL</span></th>")

          If isMobileDisplay = True Then
            htmlOut.Append("<th></th>")
            htmlOut.Append("<th></th>")
          Else
            htmlOut.Append("<th></th>")
            htmlOut.Append("<th data-priority=""1"">NAME</th>")
            htmlOut.Append("<th>CITY</th>")
            htmlOut.Append("<th>STATE</th>")
            htmlOut.Append("<th data-priority=""2"">COUNTRY</th>")
            htmlOut.Append("<th>IATA</th>")
            htmlOut.Append("<th>ICAO</th>")
          End If

          htmlOut.Append("</tr></thead><tbody>")

          For Each r As DataRow In searchTable.Rows

            airportString = r.Item("aport_id").ToString

            If Not commonEvo.inMyArray(arrAirportID, airportString) Then

              If Not IsArray(arrAirportID) And IsNothing(arrAirportID) Then
                ReDim arrAirportID(nArrCount)
              Else
                ReDim Preserve arrAirportID(nArrCount)
              End If

              ' Add CompId To Array
              arrAirportID(nArrCount) = airportString
              nArrCount += 1

              htmlOut.Append("<tr>")

              If isMobileDisplay Then

                Dim Seperator As String = ""

                htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">&nbsp;</td>")
                htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">" + r.Item("aport_id").ToString + "</td>")
                htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")

                htmlOut.Append(Replace(r.Item("aport_name").ToString.Trim, Constants.cSingleSpace, Constants.cHTMLnbsp) + "<br />")

                If Not (IsDBNull(r("aport_city"))) And Not String.IsNullOrEmpty(r.Item("aport_city").ToString.Trim) Then
                  htmlOut.Append(r.Item("aport_city").ToString.Trim + ", ")
                  Seperator = "<br />"
                End If

                If Not (IsDBNull(r("aport_state"))) And Not String.IsNullOrEmpty(r.Item("aport_state").ToString.Trim) Then
                  htmlOut.Append(" " + r.Item("aport_state").ToString.Trim)
                End If

                If Not (IsDBNull(r("aport_country"))) And Not String.IsNullOrEmpty(r.Item("aport_country").ToString.Trim) Then
                  htmlOut.Append(" " + r.Item("aport_country").ToString.Trim)
                End If

                htmlOut.Append(Seperator)
                Seperator = ""


                If Not (IsDBNull(r("aport_iata_code"))) And Not String.IsNullOrEmpty(r.Item("aport_iata_code").ToString.Trim) Then
                  htmlOut.Append(r.Item("aport_iata_code").ToString.Trim + " ")
                End If

                If Not (IsDBNull(r("aport_icao_code"))) And Not String.IsNullOrEmpty(r.Item("aport_icao_code").ToString.Trim) Then
                  htmlOut.Append(r.Item("aport_icao_code").ToString.Trim)
                End If

                htmlOut.Append("</td>")

              Else

                htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">&nbsp;</td>")
                htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">" + r.Item("aport_id").ToString + "</td>")

                htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")
                htmlOut.Append(Replace(r.Item("aport_name").ToString.Trim, Constants.cSingleSpace, Constants.cHTMLnbsp))
                htmlOut.Append("</td>")

                htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")
                If Not (IsDBNull(r("aport_city"))) And Not String.IsNullOrEmpty(r.Item("aport_city").ToString.Trim) Then
                  htmlOut.Append(r.Item("aport_city").ToString.Trim)
                End If
                htmlOut.Append("</td>")

                htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")
                If Not (IsDBNull(r("aport_state"))) And Not String.IsNullOrEmpty(r.Item("aport_state").ToString.Trim) Then
                  htmlOut.Append(r.Item("aport_state").ToString.Trim)
                End If
                htmlOut.Append("</td>")

                htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")
                If Not (IsDBNull(r("aport_country"))) And Not String.IsNullOrEmpty(r.Item("aport_country").ToString.Trim) Then
                  htmlOut.Append(r.Item("aport_country").ToString.Trim)
                End If
                htmlOut.Append("</td>")

                htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")
                If Not (IsDBNull(r("aport_iata_code"))) And Not String.IsNullOrEmpty(r.Item("aport_iata_code").ToString.Trim) Then
                  htmlOut.Append(r.Item("aport_iata_code").ToString.Trim)
                End If
                htmlOut.Append("</td>")

                htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">")
                If Not (IsDBNull(r("aport_icao_code"))) And Not String.IsNullOrEmpty(r.Item("aport_icao_code").ToString.Trim) Then
                  htmlOut.Append(r.Item("aport_icao_code").ToString.Trim)
                End If

                htmlOut.Append("</td>")

              End If

              htmlOut.Append("</tr>")

            End If

          Next

          htmlOut.Append("</tbody></table>")
          htmlOut.Append("<div id=""airportQsearchLabel"" class="""" style=""padding:2px;""><strong>" + arrAirportID.Length.ToString + " Airport(s)</strong></div>")
          htmlOut.Append("<div id=""airportQsearchInnerTable"" align=""left"" valign=""middle"" style=""max-height:470px; overflow: auto;""></div>")

        End If

      End If

      out_htmlString = htmlOut.ToString

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in display_airport_qsearch_results_table(ByRef searchTable As DataTable, ByRef out_htmlString As String, ByVal isMobileDisplay As Boolean) " + ex.Message

    Finally

      htmlOut = Nothing

    End Try

  End Sub

  Protected Function returnQuickSearchResults(ByVal sSearchText As String) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim wordArray() As String = Nothing
    Dim sTmpString As String = ""

    Try

      Dim companyFilter As String = " " + commonEvo.MakeCompanyProductCodeClause(HttpContext.Current.Session.Item("localPreferences"), False)
      'Dim aircraftFilter As String = " " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False)
      'Dim modelFilter As String = Replace(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localSubscription"), False, True), "amod", "mdl.amod")

      sQuery.Append("SELECT fts_data_search")
      'sQuery.Append(", mdl.amod_make_name, mdl.amod_model_name, mdl.amod_manufacturer, mdl.amod_id as fts_amod_id, fts_ac_id as fts_ac_idFilter")
      'sQuery.Append(", ac_ser_no_full, ac_ser_no_sort, ac_reg_no, ac_aport_iata_code, ac_aport_icao_code, ac_aport_name, ac_prev_reg_no, ac_id as fts_ac_id")

      sQuery.Append(", comp_name, comp_city, comp_address1, comp_address2, comp_state, comp_zip_code, comp_country, comp_id as fts_comp_id")

      'sQuery.Append(", contact_sirname, contact_first_name, contact_middle_initial, contact_last_name, contact_suffix, contact_title, contact_email_address, fts_contact_id")

      sQuery.Append(" FROM Full_Text_Search WITH(NOLOCK)")
      sQuery.Append(" LEFT OUTER JOIN Company WITH(NOLOCK) ON comp_id = fts_comp_id AND comp_journ_id = 0 AND comp_active_flag = 'Y' AND comp_hide_flag = 'N' " + companyFilter)

      'sQuery.Append(" LEFT OUTER JOIN Contact WITH(NOLOCK) ON contact_id = fts_contact_id AND contact_journ_id = 0 AND contact_active_flag = 'Y' AND contact_hide_flag = 'N'")

      'sQuery.Append(" LEFT OUTER JOIN Aircraft_Flat WITH(NOLOCK) ON ac_id = fts_ac_id AND ac_journ_id = 0 " + aircraftFilter)

      'sQuery.Append(" LEFT OUTER JOIN Aircraft_Model AS mdl WITH(NOLOCK) ON mdl.amod_id = fts_amod_id " + modelFilter)

      sQuery.Append(" WHERE ( comp_id IS NOT NULL AND ")


      wordArray = sSearchText.Trim.Split(" ")

      For Each wd As String In wordArray

        If String.IsNullOrEmpty(sTmpString.Trim) Then
          sTmpString = "contains (Full_Text_Search.*, '""" + Replace(Replace(wd.Trim, "'", ""), "-", "") + "*""')"
        Else
          sTmpString += Constants.cAndClause + "contains (Full_Text_Search.*, '""" + Replace(Replace(wd.Trim, "'", ""), "-", "") + "*""')"
        End If

      Next

      If Not String.IsNullOrEmpty(sTmpString.Trim) Then
        sQuery.Append(sTmpString)
      End If

      sQuery.Append(") ORDER BY comp_name ASC")

      'sQuery.Append(", contact_last_name, amod_make_name, amod_model_name, ac_ser_no_full")

      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "staticFolderEditor.aspx.vb", sQuery.ToString)

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
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
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in returnQuickSearchResults load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      atemptable = Nothing

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in returnQuickSearchResults(ByVal sSearchText As String) As DataTable " + ex.Message

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

  Protected Function returnQuickSearchAircraftResults(ByVal sSearchText As String) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim wordArray() As String = Nothing
    Dim sTmpString As String = ""

    Try

      'Dim companyFilter As String = " " + commonEvo.MakeCompanyProductCodeClause(HttpContext.Current.Session.Item("localPreferences"), False)
      Dim aircraftFilter As String = " " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False)
      'Dim modelFilter As String = Replace(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localSubscription"), False, True), "amod", "mdl.amod")

      sQuery.Append("SELECT fts_data_search")
      'sQuery.Append(", mdl.amod_make_name, mdl.amod_model_name, mdl.amod_manufacturer, mdl.amod_id as fts_amod_id, fts_ac_id as fts_ac_idFilter")
      sQuery.Append(", amod_make_name, amod_model_name, ac_ser_no_full, ac_ser_no_sort, ac_reg_no, ac_aport_iata_code, ac_aport_icao_code, ac_aport_name, ac_prev_reg_no, ac_id as fts_ac_id")

      'sQuery.Append(", comp_name, comp_city, comp_address1, comp_address2, comp_state, comp_zip_code, comp_country, comp_id as fts_comp_id")

      'sQuery.Append(", contact_sirname, contact_first_name, contact_middle_initial, contact_last_name, contact_suffix, contact_title, contact_email_address, fts_contact_id")

      sQuery.Append(" FROM Full_Text_Search WITH(NOLOCK)")
      'sQuery.Append(" LEFT OUTER JOIN Company WITH(NOLOCK) ON comp_id = fts_comp_id AND comp_journ_id = 0 AND comp_active_flag = 'Y' AND comp_hide_flag = 'N' " + companyFilter)

      'sQuery.Append(" LEFT OUTER JOIN Contact WITH(NOLOCK) ON contact_id = fts_contact_id AND contact_journ_id = 0 AND contact_active_flag = 'Y' AND contact_hide_flag = 'N'")

      sQuery.Append(" LEFT OUTER JOIN Aircraft_Flat WITH(NOLOCK) ON ac_id = fts_ac_id AND ac_journ_id = 0 " + aircraftFilter)

      'sQuery.Append(" LEFT OUTER JOIN Aircraft_Model AS mdl WITH(NOLOCK) ON mdl.amod_id = fts_amod_id " + modelFilter)

      sQuery.Append(" WHERE ( ac_id IS NOT NULL AND ")

      If Me.search_reg_nos.Checked = True Then
        wordArray = sSearchText.Trim.Split(" ")

        For Each wd As String In wordArray

          If String.IsNullOrEmpty(sTmpString.Trim) Then
            sTmpString = "contains (Full_Text_Search.*, '""" + Replace(Replace(wd.Trim, "'", ""), "-", "") + "*""')"
          Else
            sTmpString += Constants.cOrClause + "contains (Full_Text_Search.*, '""" + Replace(Replace(wd.Trim, "'", ""), "-", "") + "*""')"
          End If

        Next
        sTmpString = (" ( " & sTmpString & ") ")
      Else
        wordArray = sSearchText.Trim.Split(" ")

        For Each wd As String In wordArray

          If String.IsNullOrEmpty(sTmpString.Trim) Then
            sTmpString = "contains (Full_Text_Search.*, '""" + Replace(Replace(wd.Trim, "'", ""), "-", "") + "*""')"
          Else
            sTmpString += Constants.cAndClause + "contains (Full_Text_Search.*, '""" + Replace(Replace(wd.Trim, "'", ""), "-", "") + "*""')"
          End If

        Next
      End If



      If Not String.IsNullOrEmpty(sTmpString.Trim) Then
        sQuery.Append(sTmpString)
      End If

      sQuery.Append(") ORDER BY ac_ser_no_sort ASC")

      'sQuery.Append(", contact_last_name, amod_make_name, amod_model_name, ac_ser_no_full")

      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "staticFolderEditor.aspx.vb", sQuery.ToString)

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
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
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in returnQuickSearchAircraftResults load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      atemptable = Nothing

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in returnQuickSearchAircraftResults(ByVal sSearchText As String) As DataTable " + ex.Message

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

  Private Function PrepareClause(ByVal searchText As String) As String
    Dim LoopArray As Array
    Dim inClause As String = ""

    searchText = searchText.Replace("'", "")
    searchText = searchText.Replace(vbCr, "").Replace(vbLf, ",")
    searchText = Replace(searchText, ";", ",")

    searchText = clsGeneral.clsGeneral.CleanUserData(searchText, Constants.cEmptyString, Constants.cCommaDelim, True)

    LoopArray = Split(searchText, ",")

    If UBound(LoopArray) = 0 Then
      inClause = "'" & searchText & "'"
    Else
      For x = 0 To UBound(LoopArray)
        If Trim(LoopArray(x)) <> "" Then
          If x > 0 Then
            inClause += ","
          End If
          inClause += "'" & Trim(LoopArray(x)) & "'"
        End If
      Next
    End If

    Return inClause
  End Function

  Protected Function returnQuickSearchAirportResults(ByVal sSearchText As String) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()


    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      If searchCodes.Checked = True Then
        sSearchText = PrepareClause(sSearchText)

        sQuery.Append("SELECT aport_name, aport_city, aport_state, aport_country, aport_iata_code, aport_icao_code, aport_id")
        sQuery.Append(" FROM Airport with (NOLOCK)")
        sQuery.Append(" LEFT OUTER JOIN State WITH (NOLOCK) ON (aport_state = state_code AND aport_country = state_country)")
        sQuery.Append(" WHERE aport_active_flag = 'Y'")
        sQuery.Append(" AND (aport_iata_code in (" + sSearchText.Trim + ")")
        sQuery.Append(" OR aport_icao_code in (" + sSearchText.Trim + "))")
        sQuery.Append(" ORDER BY aport_name ASC")

      Else


        sQuery.Append("SELECT aport_name, aport_city, aport_state, aport_country, aport_iata_code, aport_icao_code, aport_id")
        sQuery.Append(" FROM Airport with (NOLOCK)")
        sQuery.Append(" LEFT OUTER JOIN State WITH (NOLOCK) ON (aport_state = state_code AND aport_country = state_country)")

        sQuery.Append(" WHERE aport_active_flag = 'Y'")
        sQuery.Append(" AND (aport_name LIKE '%" + sSearchText.Trim + "%' OR aport_iata_code LIKE '%" + sSearchText.Trim + "%' OR state_name LIKE '%" + sSearchText.Trim + "%'")
        sQuery.Append(" OR aport_icao_code LIKE '%" + sSearchText.Trim + "%' OR aport_city LIKE '%" + sSearchText.Trim + "%' OR aport_country LIKE '%" + sSearchText.Trim + "%')")

        If Trim(quick_search_location_box.Text) <> "" Then
          If continent_or_region.SelectedValue = "R" Then
            Dim aclsData_Temp As New clsData_Manager_SQL
            Dim temp_location_table As New DataTable
            Dim state_string As String = ""
            Dim country_string As String = ""
            Dim temp_table As New DataTable
            Dim temp_table2 As New DataTable
            aclsData_Temp.JETNET_DB = HttpContext.Current.Session.Item("jetnetClientDatabase")
            aclsData_Temp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase")

            ' DisplayFunctions.GetRegionInfoFromCommonControl("quick_search_location_box", BuildSearchString, BaseCountriesString, BaseTimeZoneString, BaseContinentString, BaseRegionString, BaseStateName)

            temp_table = aclsData_Temp.Get_States_From_Jetnet_Region(quick_search_location_box.Text)

            If Not IsNothing(temp_table) Then
              If temp_table.Rows.Count > 0 Then
                For Each r As DataRow In temp_table.Rows
                  If Trim(state_string) <> "" Then
                    state_string &= ",'" & r.Item("state_name") & "'"
                  Else
                    state_string = "'" & r.Item("state_name") & "'"
                  End If
                Next
              End If
            End If

            temp_table2 = aclsData_Temp.Get_Countries_From_Jetnet_Region(quick_search_location_box.Text)
            If Not IsNothing(temp_table2) Then
              If temp_table2.Rows.Count > 0 Then
                For Each r As DataRow In temp_table2.Rows
                  If Trim(country_string) <> "" Then
                    country_string &= ",'" & r.Item("geographic_country_name") & "'"
                  Else
                    country_string = "'" & r.Item("geographic_country_name") & "'"
                  End If
                Next
              End If
            End If


            ' if there is no state, do what the other page does and just do region/countries
            If Trim(state_string) <> "" Then
              sQuery.Append(" and (")
              sQuery.Append(AdvancedQueryResults.BuildRegionWhereString("state_name", "aport_country", aclsData_Temp, state_string, country_string, "'" & quick_search_location_box.Text & "'"))
              sQuery.Append(" )")
            Else
              sQuery.Append(" AND aport_country in (select distinct geographic_country_name FROM geographic with (NOLOCK) where geographic_region_name in ('" & quick_search_location_box.Text & "')) ")
            End If

            'sQuery.Append(" AND aport_country in (select distinct geographic_country_name FROM geographic with (NOLOCK) where geographic_region_name in ('" & quick_search_location_box.Text & "')) ")
            'sQuery.Append(" and aport_state in (select distinct state_code FROM geographic with (NOLOCK) inner join State with (NOLOCK) on geographic_state_code = state_code where geographic_region_name in ('" & quick_search_location_box.Text & "')) ")

            'sQuery.Append(" and (state_name in  ")
            'sQuery.Append(" (select distinct state_code FROM geographic with (NOLOCK)  ")
            'sQuery.Append(" inner join State with (NOLOCK) on geographic_state_code = state_code  ")
            'sQuery.Append(" where geographic_region_name in ('" & quick_search_location_box.Text & "'))  ")
            'sQuery.Append(" or ")
            'sQuery.Append(" (select distinct top 1 state_code FROM geographic with (NOLOCK)  ")
            'sQuery.Append(" inner join State with (NOLOCK) on geographic_state_code = state_code  ")
            'sQuery.Append(" where geographic_region_name in ('" & quick_search_location_box.Text & "'))  is null ")

            'sQuery.Append(" or aport_state is null ")
            'sQuery.Append("  ) ")

          Else
            sQuery.Append(" AND aport_country in (select distinct country_name from Country WITH(NOLOCK) where country_continent_name in ('" & quick_search_location_box.Text & "')) ")
          End If
        End If

        sQuery.Append(" ORDER BY aport_name ASC")

      End If

      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "staticFolderEditor.aspx.vb", sQuery.ToString)

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
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
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in returnQuickSearchAirportResults load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      atemptable = Nothing

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in returnQuickSearchAirportResults(ByVal sSearchText As String) As DataTable " + ex.Message

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

  Protected Function returnStaticFolderContents(ByVal nFolderID As Integer) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT * FROM Client_Folder WITH(NOLOCK) WHERE (cfolder_method = 'S' AND cfolder_id = " + nFolderID.ToString + " )")

      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "staticFolderEditor.aspx.vb", sQuery.ToString)

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
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
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in returnStaticFolderContents load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      atemptable = Nothing

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in returnStaticFolderContents(ByVal nFolderID As Integer) As DataTable " + ex.Message

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

  Protected Function returnStaticAirportFolderContents(ByVal nFolderID As Integer) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT aport_name, aport_city, aport_state, aport_country, aport_iata_code, aport_icao_code, aport_id")
      sQuery.Append(" FROM Client_Folder_Index WITH(NOLOCK) INNER JOIN Airport WITH(NOLOCK) ON cfoldind_jetnet_aport_id = aport_id")
      sQuery.Append(" WHERE ( cfoldind_cfolder_id = " + nFolderID.ToString + " )")
      sQuery.Append(" ORDER BY aport_name")

      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "staticFolderEditor.aspx.vb", sQuery.ToString)

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
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
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in returnStaticAirportFolderContents load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      atemptable = Nothing

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in returnStaticAirportFolderContents(ByVal nFolderID As Integer) As DataTable " + ex.Message

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

  Protected Function updateStaticFolderContents(ByVal nFolderID As Integer, ByVal sNewContents As String, ByVal bAppendTo As Boolean) As Boolean

    Dim sQuery = New StringBuilder()
    Dim sIQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim bResult As Boolean = False
    Dim tmpArray() As String = Nothing

    Try

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      Try

        If Not bAppendTo Then

          ' first delete "previous" records
          sQuery.Append("DELETE FROM Client_Folder_Index WHERE ( cfoldind_cfolder_id = " + nFolderID.ToString + " )")

          SqlCommand.CommandText = sQuery.ToString
          SqlCommand.ExecuteNonQuery()

          sQuery = New StringBuilder()
          HttpContext.Current.Session.Item("currentFolderData") = ""

        End If

        If Not IsNothing(HttpContext.Current.Session.Item("currentFolderData")) Then
          If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("currentFolderData").ToString.Trim) Then
            tmpArray = HttpContext.Current.Session.Item("currentFolderData").ToString.Split("=")
          End If
        End If

        sQuery.Append("UPDATE Client_Folder SET")

        If Not IsNothing(tmpArray) Then
          If Not tmpArray(0).Trim.ToLower.Contains("comp_id") Then
            tmpArray(0) = "comp_id"
          End If
        Else
          ReDim tmpArray(1)
          tmpArray(0) = "comp_id"
          tmpArray(1) = ""
        End If

        If Not bAppendTo Or UBound(tmpArray) = 0 Then

          Dim newItems() As String = sNewContents.Trim.Split(Constants.cMultiDelim)

          For Each sID As String In newItems
            If Not String.IsNullOrEmpty(sID.Trim) Then

              sIQuery.Append("INSERT INTO Client_Folder_Index (cfoldind_cfolder_id, cfoldind_jetnet_comp_id) VALUES ( " + nFolderID.ToString + ", " + sID.Trim + ")")

              SqlCommand.CommandText = sIQuery.ToString
              SqlCommand.ExecuteNonQuery()

              sIQuery = New StringBuilder()

              If Not IsNothing(HttpContext.Current.Session.Item("currentFolderData")) Then

                If String.IsNullOrEmpty(HttpContext.Current.Session.Item("currentFolderData").ToString.Trim) Then
                  HttpContext.Current.Session.Item("currentFolderData") = sID.Trim
                Else
                  HttpContext.Current.Session.Item("currentFolderData") += Constants.cCommaDelim + sID.Trim
                End If

              Else
                HttpContext.Current.Session.Item("currentFolderData") = sID.Trim
              End If


            End If
          Next

          sQuery.Append(" cfolder_data = '" + tmpArray(0).Trim + "=" + sNewContents.Replace(Constants.cMultiDelim, Constants.cCommaDelim).Trim + "',")

        Else

          Dim noDuplicates() As String = tmpArray(1).Trim.Split(Constants.cCommaDelim)
          Dim newItems() As String = sNewContents.Trim.Split(Constants.cMultiDelim)

          For Each sID As String In newItems
            If Not commonEvo.inMyArray(noDuplicates, sID) And Not String.IsNullOrEmpty(sID.Trim) Then

              If String.IsNullOrEmpty(tmpArray(1).Trim) Then
                tmpArray(1) = sID.Trim
              Else
                tmpArray(1) += Constants.cCommaDelim + sID.Trim
              End If

              sIQuery.Append("INSERT INTO Client_Folder_Index (cfoldind_cfolder_id, cfoldind_jetnet_comp_id) VALUES ( " + nFolderID.ToString + ", " + sID.Trim + ")")

              SqlCommand.CommandText = sIQuery.ToString
              SqlCommand.ExecuteNonQuery()

              sIQuery = New StringBuilder()

              If Not IsNothing(HttpContext.Current.Session.Item("currentFolderData")) Then

                If String.IsNullOrEmpty(HttpContext.Current.Session.Item("currentFolderData").ToString.Trim) Then
                  HttpContext.Current.Session.Item("currentFolderData") = sID.Trim
                Else
                  HttpContext.Current.Session.Item("currentFolderData") += Constants.cCommaDelim + sID.Trim
                End If

              Else
                HttpContext.Current.Session.Item("currentFolderData") = sID.Trim
              End If

            End If
          Next

          sQuery.Append(" cfolder_data = '" + tmpArray(0).Trim + "=" + tmpArray(1).Trim + "',")

        End If

        sQuery.Append(" cfolder_update_date = '" + FormatDateTime(Now(), DateFormat.GeneralDate).ToString + "'")
        sQuery.Append(" WHERE (cfolder_method = 'S' AND cfolder_id = " + nFolderID.ToString + " )")

        clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "staticFolderEditor.aspx.vb", sQuery.ToString)

        SqlCommand.CommandText = sQuery.ToString
        SqlCommand.ExecuteNonQuery()

        bResult = True

      Catch SqlException
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in updateStaticFolderContents ExecuteNonQuery : " + SqlException.Message
      End Try

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in updateStaticFolderContents(ByVal nFolderID As Integer, ByVal sNewContents As String, ByVal bAppendTo As Boolean) As boolean " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return bResult

  End Function

  Protected Function updateStaticAircraftFolderContents(ByVal nFolderID As Integer, ByVal sNewContents As String, ByVal bAppendTo As Boolean) As Boolean

    Dim sQuery = New StringBuilder()
    Dim sIQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim bResult As Boolean = False
    Dim tmpArray() As String = Nothing

    Try

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      Try

        If Not bAppendTo Then

          ' first delete "previous" records
          sQuery.Append("DELETE FROM Client_Folder_Index WHERE ( cfoldind_cfolder_id = " + nFolderID.ToString + " )")

          SqlCommand.CommandText = sQuery.ToString
          SqlCommand.ExecuteNonQuery()

          sQuery = New StringBuilder()
          HttpContext.Current.Session.Item("currentFolderData") = ""

        End If

        If Not IsNothing(HttpContext.Current.Session.Item("currentFolderData")) Then
          If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("currentFolderData").ToString.Trim) Then
            tmpArray = HttpContext.Current.Session.Item("currentFolderData").ToString.Split("=")
          End If
        End If

        sQuery.Append("UPDATE Client_Folder SET")

        If Not IsNothing(tmpArray) Then
          If Not tmpArray(0).Trim.ToLower.Contains("ac_id") Then
            tmpArray(0) = "ac_id"
          End If
        Else
          ReDim tmpArray(1)
          tmpArray(0) = "ac_id"
          tmpArray(1) = ""
        End If

        If Not bAppendTo Or UBound(tmpArray) = 0 Then

          Dim newItems() As String = sNewContents.Trim.Split(Constants.cMultiDelim)

          For Each sID As String In newItems
            If Not String.IsNullOrEmpty(sID.Trim) Then

              sIQuery.Append("INSERT INTO Client_Folder_Index (cfoldind_cfolder_id, cfoldind_jetnet_ac_id) VALUES ( " + nFolderID.ToString + ", " + sID.Trim + ")")

              SqlCommand.CommandText = sIQuery.ToString
              SqlCommand.ExecuteNonQuery()

              sIQuery = New StringBuilder()

              If Not IsNothing(HttpContext.Current.Session.Item("currentFolderData")) Then

                If String.IsNullOrEmpty(HttpContext.Current.Session.Item("currentFolderData").ToString.Trim) Then
                  HttpContext.Current.Session.Item("currentFolderData") = sID.Trim
                Else
                  HttpContext.Current.Session.Item("currentFolderData") += Constants.cCommaDelim + sID.Trim
                End If

              Else
                HttpContext.Current.Session.Item("currentFolderData") = sID.Trim
              End If


            End If
          Next

          sQuery.Append(" cfolder_data = '" + tmpArray(0).Trim + "=" + sNewContents.Replace(Constants.cMultiDelim, Constants.cCommaDelim).Trim + "',")

        Else

          Dim noDuplicates() As String = tmpArray(1).Trim.Split(Constants.cCommaDelim)
          Dim newItems() As String = sNewContents.Trim.Split(Constants.cMultiDelim)

          For Each sID As String In newItems
            If Not commonEvo.inMyArray(noDuplicates, sID) And Not String.IsNullOrEmpty(sID.Trim) Then

              If String.IsNullOrEmpty(tmpArray(1).Trim) Then
                tmpArray(1) = sID.Trim
              Else
                tmpArray(1) += Constants.cCommaDelim + sID.Trim
              End If

              sIQuery.Append("INSERT INTO Client_Folder_Index (cfoldind_cfolder_id, cfoldind_jetnet_ac_id) VALUES ( " + nFolderID.ToString + ", " + sID.Trim + ")")

              SqlCommand.CommandText = sIQuery.ToString
              SqlCommand.ExecuteNonQuery()

              sIQuery = New StringBuilder()

              If Not IsNothing(HttpContext.Current.Session.Item("currentFolderData")) Then

                If String.IsNullOrEmpty(HttpContext.Current.Session.Item("currentFolderData").ToString.Trim) Then
                  HttpContext.Current.Session.Item("currentFolderData") = sID.Trim
                Else
                  HttpContext.Current.Session.Item("currentFolderData") += Constants.cCommaDelim + sID.Trim
                End If

              Else
                HttpContext.Current.Session.Item("currentFolderData") = sID.Trim
              End If

            End If
          Next

          sQuery.Append(" cfolder_data = '" + tmpArray(0).Trim + "=" + tmpArray(1).Trim + "',")

        End If

        sQuery.Append(" cfolder_update_date = '" + FormatDateTime(Now(), DateFormat.GeneralDate).ToString + "'")
        sQuery.Append(" WHERE (cfolder_method = 'S' AND cfolder_id = " + nFolderID.ToString + " )")

        clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "staticFolderEditor.aspx.vb", sQuery.ToString)

        SqlCommand.CommandText = sQuery.ToString
        SqlCommand.ExecuteNonQuery()

        bResult = True

      Catch SqlException
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in updateStaticAircraftFolderContents ExecuteNonQuery : " + SqlException.Message
      End Try

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in updateStaticAircraftFolderContents(ByVal nFolderID As Integer, ByVal sNewContents As String, ByVal bAppendTo As Boolean) As boolean " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return bResult

  End Function

  Protected Function updateStaticAirportFolderContents(ByVal nFolderID As Integer, ByVal sNewContents As String, ByVal bAppendTo As Boolean) As Boolean

    Dim sQuery = New StringBuilder()
    Dim sIQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim bResult As Boolean = False
    Dim tmpArray() As String = Nothing

    Try

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      Try

        If Not bAppendTo Then

          ' first delete "previous" records
          sQuery.Append("DELETE FROM Client_Folder_Index WHERE ( cfoldind_cfolder_id = " + nFolderID.ToString + " )")

          SqlCommand.CommandText = sQuery.ToString
          SqlCommand.ExecuteNonQuery()

          sQuery = New StringBuilder()
          HttpContext.Current.Session.Item("currentFolderData") = ""

        End If

        If Not IsNothing(HttpContext.Current.Session.Item("currentFolderData")) Then
          If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("currentFolderData").ToString.Trim) Then
            tmpArray = HttpContext.Current.Session.Item("currentFolderData").ToString.Split("=")
          End If
        End If

        sQuery.Append("UPDATE Client_Folder SET")

        If Not IsNothing(tmpArray) Then
          If Not tmpArray(0).Trim.ToLower.Contains("ac_aport_id") Then
            tmpArray(0) = "ac_aport_id"
          End If
        Else
          ReDim tmpArray(1)
          tmpArray(0) = "ac_aport_id"
          tmpArray(1) = ""
        End If

        If Not bAppendTo Or UBound(tmpArray) = 0 Then

          Dim newItems() As String = sNewContents.Trim.Split(Constants.cMultiDelim)

          For Each sID As String In newItems
            If Not String.IsNullOrEmpty(sID.Trim) Then

              sIQuery.Append("INSERT INTO Client_Folder_Index (cfoldind_cfolder_id, cfoldind_jetnet_aport_id) VALUES ( " + nFolderID.ToString + ", " + sID.Trim + ")")

              SqlCommand.CommandText = sIQuery.ToString
              SqlCommand.ExecuteNonQuery()

              sIQuery = New StringBuilder()

              If Not IsNothing(HttpContext.Current.Session.Item("currentFolderData")) Then

                If String.IsNullOrEmpty(HttpContext.Current.Session.Item("currentFolderData").ToString.Trim) Then
                  HttpContext.Current.Session.Item("currentFolderData") = sID.Trim
                Else
                  HttpContext.Current.Session.Item("currentFolderData") += Constants.cCommaDelim + sID.Trim
                End If

              Else
                HttpContext.Current.Session.Item("currentFolderData") = sID.Trim
              End If


            End If
          Next

          sQuery.Append(" cfolder_data = '" + tmpArray(0).Trim + "=" + sNewContents.Replace(Constants.cMultiDelim, Constants.cCommaDelim).Trim + "',")

        Else

          Dim noDuplicates() As String = tmpArray(1).Trim.Split(Constants.cCommaDelim)
          Dim newItems() As String = sNewContents.Trim.Split(Constants.cMultiDelim)

          For Each sID As String In newItems
            If Not commonEvo.inMyArray(noDuplicates, sID) And Not String.IsNullOrEmpty(sID.Trim) Then

              If String.IsNullOrEmpty(tmpArray(1).Trim) Then
                tmpArray(1) = sID.Trim
              Else
                tmpArray(1) += Constants.cCommaDelim + sID.Trim
              End If

              sIQuery.Append("INSERT INTO Client_Folder_Index (cfoldind_cfolder_id, cfoldind_jetnet_aport_id) VALUES ( " + nFolderID.ToString + ", " + sID.Trim + ")")

              SqlCommand.CommandText = sIQuery.ToString
              SqlCommand.ExecuteNonQuery()

              sIQuery = New StringBuilder()

              If Not IsNothing(HttpContext.Current.Session.Item("currentFolderData")) Then

                If String.IsNullOrEmpty(HttpContext.Current.Session.Item("currentFolderData").ToString.Trim) Then
                  HttpContext.Current.Session.Item("currentFolderData") = sID.Trim
                Else
                  HttpContext.Current.Session.Item("currentFolderData") += Constants.cCommaDelim + sID.Trim
                End If

              Else
                HttpContext.Current.Session.Item("currentFolderData") = sID.Trim
              End If


            End If
          Next

          sQuery.Append(" cfolder_data = '" + tmpArray(0).Trim + "=" + tmpArray(1).Trim + "',")

        End If

        sQuery.Append(" cfolder_update_date = '" + FormatDateTime(Now(), DateFormat.GeneralDate).ToString + "'")
        sQuery.Append(" WHERE (cfolder_method = 'S' AND cfolder_id = " + nFolderID.ToString + " )")

        clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "staticFolderEditor.aspx.vb", sQuery.ToString)

        SqlCommand.CommandText = sQuery.ToString
        SqlCommand.ExecuteNonQuery()

        bResult = True

      Catch SqlException
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in updateStaticAirportFolderContents ExecuteNonQuery : " + SqlException.Message
      End Try

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in updateStaticAirportFolderContents(ByVal nFolderID As Integer, ByVal sNewContents As String, ByVal bAppendTo As Boolean) As boolean " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return bResult

  End Function

  Protected Function saveQuickSearchContents(ByVal sNewName As String, ByVal sNewContents As String, ByVal bDefaultFolder As Boolean) As Integer

    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim nFolderID As Integer = 0

    Try

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      sQuery.Append("INSERT INTO Client_Folder (cfolder_cftype_id, cfolder_method, cfolder_name, cfolder_data, cfolder_entry_date, cfolder_default_flag,")
      sQuery.Append(" cfolder_sub_id, cfolder_login, cfolder_seq_no")
      sQuery.Append(" ) VALUES (")
      sQuery.Append(" 1, 'S', '" + sNewName.Trim.Replace(Constants.cSingleQuote, Constants.cEmptyString) + "', 'comp_id=" + sNewContents.Replace(Constants.cMultiDelim, Constants.cCommaDelim).Trim + "',")
      sQuery.Append(" '" + FormatDateTime(Now(), DateFormat.GeneralDate).ToString + "'," + IIf(bDefaultFolder, " 'Y',", " 'N',"))
      sQuery.Append(" " + Session.Item("localUser").crmSubSubID.ToString + ", '" + Session.Item("localUser").crmUserLogin.ToString.Trim + "', " + Session.Item("localUser").crmSubSeqNo.ToString + ")")

      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "staticFolderEditor.aspx.vb", sQuery.ToString)

      Try

        SqlCommand.CommandText = sQuery.ToString
        SqlCommand.ExecuteNonQuery()

        HttpContext.Current.Session.Item("currentFolderData") = ""

        sQuery = New StringBuilder()
        sQuery.Append("SELECT MAX(cfolder_id) AS MaxFolderID FROM Client_Folder WITH(NOLOCK) WHERE cfolder_name = '" + sNewName.Trim.Replace(Constants.cSingleQuote, Constants.cEmptyString) + "'")

        SqlCommand.CommandText = sQuery.ToString
        SqlReader = SqlCommand.ExecuteReader()

        If SqlReader.HasRows Then
          SqlReader.Read()
          If Not IsDBNull(SqlReader.Item("MaxFolderID")) Then
            If Not String.IsNullOrEmpty(SqlReader.Item("MaxFolderID").ToString.Trim) Then
              nFolderID = CInt(SqlReader.Item("MaxFolderID").ToString)
            End If
          End If
          SqlReader.Close()
        End If

        HttpContext.Current.Session.Item("currentFolderData") = sNewContents.Replace(Constants.cMultiDelim, Constants.cCommaDelim).Trim

        Dim tmpArray() = sNewContents.Split(Constants.cCommaDelim)

        For Each tStr As String In tmpArray

          sQuery.Append("INSERT INTO Client_Folder_Index (cfoldind_cfolder_id, cfoldind_jetnet_comp_id) VALUES ( " + nFolderID.ToString + ", " + tStr.Trim + ")")

          SqlCommand.CommandText = sQuery.ToString
          SqlCommand.ExecuteNonQuery()

          sQuery = New StringBuilder()

        Next

      Catch SqlException
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in saveQuickSearchContents ExecuteNonQuery : " + SqlException.Message
      End Try

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in saveQuickSearchContents(ByVal sNewName As String, ByVal sNewContents As String, ByVal bDefaultFolder As Boolean) As boolean " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return nFolderID

  End Function

  Protected Function saveQuickSearchAircraftContents(ByVal sNewName As String, ByVal sNewContents As String, ByVal bDefaultFolder As Boolean) As Integer

    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim nFolderID As Integer = 0

    Try

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      sQuery.Append("INSERT INTO Client_Folder (cfolder_cftype_id, cfolder_method, cfolder_name, cfolder_data, cfolder_entry_date, cfolder_default_flag,")
      sQuery.Append(" cfolder_sub_id, cfolder_login, cfolder_seq_no")
      sQuery.Append(" ) VALUES (")
      sQuery.Append(" 1, 'S', '" + sNewName.Trim.Replace(Constants.cSingleQuote, Constants.cEmptyString) + "', 'ac_id=" + sNewContents.Replace(Constants.cMultiDelim, Constants.cCommaDelim).Trim + "',")
      sQuery.Append(" '" + FormatDateTime(Now(), DateFormat.GeneralDate).ToString + "'," + IIf(bDefaultFolder, " 'Y',", " 'N',"))
      sQuery.Append(" " + Session.Item("localUser").crmSubSubID.ToString + ", '" + Session.Item("localUser").crmUserLogin.ToString.Trim + "', " + Session.Item("localUser").crmSubSeqNo.ToString + ")")

      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "staticFolderEditor.aspx.vb", sQuery.ToString)

      Try

        SqlCommand.CommandText = sQuery.ToString
        SqlCommand.ExecuteNonQuery()

        HttpContext.Current.Session.Item("currentFolderData") = ""

        sQuery = New StringBuilder()
        sQuery.Append("SELECT MAX(cfolder_id) AS MaxFolderID FROM Client_Folder WITH(NOLOCK) WHERE cfolder_name = '" + sNewName.Trim.Replace(Constants.cSingleQuote, Constants.cEmptyString) + "'")

        SqlCommand.CommandText = sQuery.ToString
        SqlReader = SqlCommand.ExecuteReader()

        If SqlReader.HasRows Then
          SqlReader.Read()
          If Not IsDBNull(SqlReader.Item("MaxFolderID")) Then
            If Not String.IsNullOrEmpty(SqlReader.Item("MaxFolderID").ToString.Trim) Then
              nFolderID = CInt(SqlReader.Item("MaxFolderID").ToString)
            End If
          End If
          SqlReader.Close()
        End If

        HttpContext.Current.Session.Item("currentFolderData") = sNewContents.Replace(Constants.cMultiDelim, Constants.cCommaDelim).Trim

        Dim tmpArray() = sNewContents.Split(Constants.cCommaDelim)

        For Each tStr As String In tmpArray

          sQuery.Append("INSERT INTO Client_Folder_Index (cfoldind_cfolder_id, cfoldind_jetnet_ac_id) VALUES ( " + nFolderID.ToString + ", " + tStr.Trim + ")")

          SqlCommand.CommandText = sQuery.ToString
          SqlCommand.ExecuteNonQuery()

          sQuery = New StringBuilder()

        Next

      Catch SqlException
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in saveQuickSearchAircraftContents ExecuteNonQuery : " + SqlException.Message
      End Try

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in saveQuickSearchAircraftContents(ByVal sNewName As String, ByVal sNewContents As String, ByVal bDefaultFolder As Boolean) As boolean " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return nFolderID

  End Function

  Protected Function saveQuickSearchAirportContents(ByVal sNewName As String, ByVal sNewContents As String, ByVal bDefaultFolder As Boolean) As Integer

    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim nFolderID As Integer = 0

    Try

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      sQuery.Append("INSERT INTO Client_Folder (cfolder_cftype_id, cfolder_method, cfolder_name, cfolder_data, cfolder_entry_date, cfolder_default_flag,")
      sQuery.Append(" cfolder_sub_id, cfolder_login, cfolder_seq_no")
      sQuery.Append(" ) VALUES (")
      sQuery.Append(" 17, 'S', '" + sNewName.Trim.Replace(Constants.cSingleQuote, Constants.cEmptyString) + "', 'ac_aport_id=" + sNewContents.Replace(Constants.cMultiDelim, Constants.cCommaDelim).Trim + "',")
      sQuery.Append(" '" + FormatDateTime(Now(), DateFormat.GeneralDate).ToString + "'," + IIf(bDefaultFolder, " 'Y',", " 'N',"))
      sQuery.Append(" " + Session.Item("localUser").crmSubSubID.ToString + ", '" + Session.Item("localUser").crmUserLogin.ToString.Trim + "', " + Session.Item("localUser").crmSubSeqNo.ToString + ")")

      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "staticFolderEditor.aspx.vb", sQuery.ToString)

      Try

        SqlCommand.CommandText = sQuery.ToString
        SqlCommand.ExecuteNonQuery()

        sQuery = New StringBuilder()
        sQuery.Append("SELECT MAX(cfolder_id) AS MaxFolderID FROM Client_Folder WITH(NOLOCK) WHERE cfolder_name = '" + sNewName.Trim.Replace(Constants.cSingleQuote, Constants.cEmptyString) + "'")

        SqlCommand.CommandText = sQuery.ToString
        SqlReader = SqlCommand.ExecuteReader()

        If SqlReader.HasRows Then
          SqlReader.Read()
          If Not IsDBNull(SqlReader.Item("MaxFolderID")) Then
            If Not String.IsNullOrEmpty(SqlReader.Item("MaxFolderID").ToString.Trim) Then
              nFolderID = CInt(SqlReader.Item("MaxFolderID").ToString)
            End If
          End If
          SqlReader.Close()
        End If

        sQuery = New StringBuilder()

        HttpContext.Current.Session.Item("currentFolderData") = sNewContents.Replace(Constants.cMultiDelim, Constants.cCommaDelim).Trim

        Dim tmpArray() = sNewContents.Split(Constants.cCommaDelim)

        For Each tStr As String In tmpArray

          sQuery.Append("INSERT INTO Client_Folder_Index (cfoldind_cfolder_id, cfoldind_jetnet_aport_id) VALUES ( " + nFolderID.ToString + ", " + tStr.Trim + ")")

          SqlCommand.CommandText = sQuery.ToString
          SqlCommand.ExecuteNonQuery()

          sQuery = New StringBuilder()

        Next

      Catch SqlException
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in saveQuickSearchAirportContents ExecuteNonQuery : " + SqlException.Message
      End Try

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in saveQuickSearchAirportContents(ByVal sNewName As String, ByVal sNewContents As String, ByVal bDefaultFolder As Boolean) As boolean " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return nFolderID

  End Function

End Class