' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/WebSource.aspx.vb $
'$$Author: Amanda $
'$$Date: 8/05/19 3:17p $
'$$Modtime: 8/02/19 12:26p $
'$$Revision: 3 $
'$$Workfile: WebSource.aspx.vb $
'
' ********************************************************************************

Partial Public Class WebSource

    Inherits System.Web.UI.Page
    Dim AmodID As Long = 0
    Dim viewID As Long = 0
    Dim noteID As Long = 0
    Dim viewType As String = ""
    Dim localDatalayer As New viewsDataLayer
    Dim CRMDataLayer As New crmViewDataLayer
    Dim CRMViewActive As Boolean = False
    Dim UseModelValueOnly As Boolean = False
    Dim aclsData_Temp As New clsData_Manager_SQL
    Dim exportID As Long = 0
    Dim extra_criteria As Boolean = False
    Dim internal As Boolean = False
    Dim retail As Boolean = False
    Dim AircraftID As Long = 0
    Dim YearsMonthsSettings As Long = 0
    Dim YearsOf As Long = 0
    Dim SalesWithin As Long = 0
    Dim YearOfCurrent As String = ""
    Dim AFTTCurrent As String = ""
    Dim UseUsed As Boolean = False
    Dim UseJetnet As Boolean = False
    Dim LastSaveDate As String = ""
    Dim JetnetACID As Long = 0
    Dim MakeName As String = ""
    Dim ModelName As String = ""
    Dim PageTitleText As String = ""
    Dim NoMaster As Boolean = False
    Dim afttStart As Long = 0
    Dim afttEnd As Long = 0
    Dim yearStart As Integer = 0
    Dim yearEnd As Integer = 0
    Dim variantList As String = ""

    Dim genericReport As Boolean = False
    Dim genericDatatable As New DataTable

    Dim urlRefresh As String = "view_template.aspx?"

    Private Sub WebSource_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

        If genericReport Then
            Dim JavascriptOnLoad As String = ""

            'JavascriptOnLoad += vbCrLf + ""

            JavascriptOnLoad += vbCrLf + "CreateGenericTable(""genericInnerTable"",""genericDataTable"",""genericjQueryTable"");"

            JavascriptOnLoad += vbCrLf + "CloseLoadingMessage(""DivLoadingMessage"");"

            If Not Page.ClientScript.IsClientScriptBlockRegistered("onLoadCode") Then
                System.Web.UI.ScriptManager.RegisterStartupScript(Me.form1, Me.form1.GetType(), "onLoadCode", "$(document).ready(function() {" + JavascriptOnLoad.ToString + "});", True)
            End If
        End If

    End Sub

    Private Sub displayGenericReport()

        Dim htmlOut As New StringBuilder
        Dim results_table As New DataTable

        Dim bIsFirstColumn As Boolean = False

        Dim arrColumnName(,) As String = Nothing

        Dim dbFieldID As String = ""
        Dim dbFieldName As String = ""
        Dim tableHdrName As String = ""
        Dim displayName As String = ""
        Dim showColumn As String = ""

        Dim nCounter As Long = 0

        Dim tmpColumnName As String = ""

        Try

            If Not IsNothing(HttpContext.Current.Session.Item("documentsDataTable")) Then
                genericDatatable = CType(HttpContext.Current.Session.Item("documentsDataTable"), DataTable)
            End If

            If genericDatatable.Rows.Count > 0 Then

                htmlOut.Append("<table id=""genericDataTable"" cellpadding=""0"" cellspacing=""0"" border=""0"" width=""100%"">")
                htmlOut.Append("<thead><tr>")

                htmlOut.Append("<th><span class=""help_cursor"" title=""Used to select and remove items from the list"">SEL</span></th>")
                htmlOut.Append("<th></th>")

                results_table = get_generic_report_headers()

                If Not IsNothing(results_table) Then

                    If results_table.Rows.Count > 0 Then

                        ReDim arrColumnName(results_table.Rows.Count - 1, 4)

                        For Each r As DataRow In results_table.Rows ' showColumn

                            If Not (IsDBNull(r("cef_id"))) Then
                                dbFieldID = r.Item("cef_id").ToString.Trim
                            End If

                            If Not (IsDBNull(r("cef_evo_field_name"))) Then
                                dbFieldName = r.Item("cef_evo_field_name").ToString.Trim
                            End If

                            If Not (IsDBNull(r("cef_header_field_name"))) Then
                                tableHdrName = r.Item("cef_header_field_name").ToString.Trim
                            End If

                            If Not (IsDBNull(r("cef_display"))) Then
                                displayName = r.Item("cef_display").ToString.Trim
                            End If

                            If Not (IsDBNull(r("cef_generic_field_display"))) Then
                                showColumn = r.Item("cef_generic_field_display").ToString.Trim
                            End If

                            arrColumnName(nCounter, 0) = dbFieldID
                            arrColumnName(nCounter, 1) = dbFieldName
                            arrColumnName(nCounter, 2) = tableHdrName
                            arrColumnName(nCounter, 3) = displayName
                            arrColumnName(nCounter, 4) = showColumn

                            nCounter += 1

                            dbFieldID = ""
                            dbFieldName = ""
                            tableHdrName = ""
                            displayName = ""
                            showColumn = ""

                        Next

                    End If

                End If

                For Each c As DataColumn In genericDatatable.Columns

                    tmpColumnName = c.ColumnName.ToUpper.Replace("CCOUNT", "COUNT")

                    For x As Integer = 0 To UBound(arrColumnName)

                        ' check the "array" to see if column name matches if it does match then 
                        If arrColumnName(x, 1).ToUpper = tmpColumnName.ToUpper Then

                            ' check to see if field is shown
                            If arrColumnName(x, 4).ToUpper.Contains("Y") Then

                                If Not bIsFirstColumn Then
                                    htmlOut.Append("<th data-priority=""1"">" + arrColumnName(x, 2) + "</th>")
                                    bIsFirstColumn = True
                                Else
                                    htmlOut.Append("<th>" + arrColumnName(x, 2) + "</th>")
                                End If

                            End If

                        End If

                    Next

                    tmpColumnName = ""

                Next

                htmlOut.Append("</tr></thead><tbody>")

                Dim nCount As Integer = 0

                For Each Row As DataRow In genericDatatable.Rows

                    nCount += 1

                    htmlOut.Append("<tr>")

                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">&nbsp;</td>")
                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">" + nCount.ToString + "</td>")

                    ' ramble through each "column name" and display data
                    For Each c As DataColumn In genericDatatable.Columns

                        tmpColumnName = c.ColumnName.ToUpper.Replace("CCOUNT", "COUNT")

                        For x As Integer = 0 To UBound(arrColumnName)

                            ' check the "array" to see if column name matches if it does match then 
                            If arrColumnName(x, 1).ToUpper = tmpColumnName.ToUpper Then

                                ' check to see if field is shown
                                If arrColumnName(x, 4).ToUpper.Contains("Y") Then

                                    If Not IsDBNull(Row.Item(arrColumnName(x, 1))) Then
                                        If Not String.IsNullOrEmpty(Row.Item(arrColumnName(x, 1)).ToString.Trim) Then

                                            Select Case Type.GetTypeCode(c.DataType)
                                                Case TypeCode.DateTime
                                                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">" + FormatDateTime(Row.Item(arrColumnName(x, 1)).ToString, DateFormat.GeneralDate).Trim + "</td>")
                                                Case TypeCode.Double
                                                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">" + FormatNumber(Row.Item(arrColumnName(x, 1)).ToString, 2, False, False, True).Trim + "</td>")
                                                Case TypeCode.Int32
                                                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">" + FormatNumber(Row.Item(arrColumnName(x, 1)).ToString, 0, False, False, False).Trim + "</td>")
                                                Case TypeCode.Int64
                                                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">" + FormatNumber(Row.Item(arrColumnName(x, 1)).ToString, 0, False, False, False).Trim + "</td>")
                                                Case Else
                                                    htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">" + Server.HtmlEncode(Row.Item(arrColumnName(x, 1)).ToString).Trim + "</td>")
                                            End Select

                                        Else
                                            htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap""></td>")
                                        End If
                                    Else
                                        htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap""></td>")
                                    End If

                                End If

                            End If

                        Next

                        tmpColumnName = ""

                    Next

                    htmlOut.Append("</tr>" + vbCrLf)

                Next

                htmlOut.Append("</tbody></table>")
                htmlOut.Append("<div id=""genericLabel"" class="""" style=""padding:2px;""><strong>" + genericDatatable.Rows.Count.ToString + " items</strong></div>")
                htmlOut.Append("<div id=""genericInnerTable"" align=""left"" valign=""middle"" style=""max-height:470px; overflow: auto;""></div>")

            End If

            genericReportTable.Text += htmlOut.ToString.Trim

        Catch ex As Exception
            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in displayGenericReport" + ex.Message
        Finally

        End Try

    End Sub

    Private Function get_generic_report_headers() As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            sQuery.Append("SELECT cef_id, cef_display, cef_evo_field_name, cef_header_field_name, cef_generic_field, cef_generic_field_display")
            sQuery.Append(" FROM Custom_Export_Fields WITH(NOLOCK)")
            sQuery.Append(" WHERE cef_generic_field = 'Y'")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_generic_report_headers() As DataTable</b><br />" + sQuery.ToString

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
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in get_generic_report_headers load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in get_generic_report_headers() As DataTable " + ex.Message

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


    Private Sub WebSource_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("")

        Dim SMgr As ScriptManager
        If ScriptManager.GetCurrent(Page) Is Nothing Then
            Throw New Exception("ScriptManager not found.")
        Else
            SMgr = ScriptManager.GetCurrent(Page)
        End If

        Dim SRef As ScriptReference = New ScriptReference()
        SRef.Path = "https://cdnjs.cloudflare.com/ajax/libs/jquery/3.4.1/jquery.min.js"
        SMgr.Scripts.Add(SRef)


        Dim SRef1 As ScriptReference = New ScriptReference()
        SRef1.Path = "https://code.jquery.com/ui/1.12.1/jquery-ui.js"
        SMgr.Scripts.Add(SRef1)

        SRef = New ScriptReference()
        SRef.Path = "https://cdnjs.cloudflare.com/ajax/libs/jquery-migrate/3.1.0/jquery-migrate.min.js"
        SMgr.Scripts.Add(SRef)


        '    Dim SRef2 As ScriptReference = New ScriptReference()
        'SRef2.Path = "~/common/jquery-ui-autocomplete.js"
        'SMgr.Scripts.Add(SRef2)

        Dim SRef3 As ScriptReference = New ScriptReference()
        SRef3.Path = "~/common/jquery.select-to-autocomplete.min.js"
        SMgr.Scripts.Add(SRef3)

        Dim SRef4 As ScriptReference = New ScriptReference()
        SRef4.Path = "~/common/common_functions.js?v=3"
        SMgr.Scripts.Add(SRef4)

        Dim SRef5 = New ScriptReference()
        SRef5.Path = "~/common/jquery.slicknav.min.js"
        SMgr.Scripts.Add(SRef5)

        Dim SRef6 = New ScriptReference()
        SRef6.Path = "~/common/header_scripts.js"
        SMgr.Scripts.Add(SRef6)

        Dim SRef7 = New ScriptReference()
        SRef7.Path = "~/common/chosen.jquery.min.js"
        SMgr.Scripts.Add(SRef7)


        If Not IsNothing(Request.Item("genericReport")) Then
            If Not String.IsNullOrEmpty(Request.Item("genericReport").ToString.Trim) Then
                genericReport = CBool(Request.Item("genericReport").ToString.Trim)
            End If
        End If

        If Not IsNothing(Trim(Request("viewID"))) Then
            If Not String.IsNullOrEmpty(Trim(Request("viewID"))) Then
                If IsNumeric(Trim(Request("viewID"))) Then
                    viewID = Trim(Request("viewID"))
                    urlRefresh += "viewID=" & viewID.ToString
                End If
            End If
        End If

        If Not IsNothing(Trim(Request("useModelOnly"))) Then
            If Not String.IsNullOrEmpty(Trim(Request("useModelOnly"))) Then
                Select Case Trim(Request("useModelOnly")).ToLower
                    Case "true"
                        UseModelValueOnly = True
                    Case Else
                        UseModelValueOnly = False
                End Select
            End If
        End If
        If Not IsNothing(Trim(Request("amodID"))) Then
            If Not String.IsNullOrEmpty(Trim(Request("amodID"))) Then
                If IsNumeric(Trim(Request("amodID"))) Then
                    AmodID = commonEvo.ReturnAmodIDForItemIndex(CLng(Trim(Request("amodID")).ToString)).ToString()
                    If viewID = 19 And UseModelValueOnly = False Then
                    Else
                        urlRefresh += "&amod_id=" & AmodID.ToString
                    End If
                End If
            End If
        End If

        If Not IsNothing(Trim(Request("noMaster"))) Then
            If Not String.IsNullOrEmpty(Trim(Request("noMaster"))) Then
                NoMaster = Trim(Request("noMaster"))
            End If
        End If
        If Not IsNothing(Trim(Request("viewType"))) Then
            If Not String.IsNullOrEmpty(Trim(Request("viewType"))) Then
                viewType = Trim(Request("viewType"))
            End If
        End If
        If Not IsNothing(Trim(Request("noteID"))) Then
            If Not String.IsNullOrEmpty(Trim(Request("noteID"))) Then
                If IsNumeric(Trim(Request("noteID"))) Then
                    noteID = Trim(Request("noteID"))
                    urlRefresh += "&noteID=" & noteID.ToString
                End If
            End If
        End If

        If Not IsNothing(Trim(Request("repID"))) Then
            If Not String.IsNullOrEmpty(Trim(Request("repID"))) Then
                Dim holdRep As String = Server.HtmlDecode(Trim(Request("repID")))
                Dim repID As Array = Split(holdRep, "      ")
                If UBound(repID) = 1 Then
                    exportID = repID(1)
                ElseIf UBound(repID) = 0 Then
                    If IsNumeric(repID(0)) Then
                        exportID = repID(0)
                        If exportID = 11111 Then
                            extra_criteria = True
                        End If
                    End If
                End If
            End If
        End If



        If Not IsNothing(Trim(Request("YearsMonthsSettings"))) Then
            If Not String.IsNullOrEmpty(Trim(Request("YearsMonthsSettings"))) Then
                If IsNumeric(Trim(Request("YearsMonthsSettings"))) Then
                    YearsMonthsSettings = Trim(Request("YearsMonthsSettings"))
                End If
            End If
        End If
        If Not IsNothing(Trim(Request("YearsOf"))) Then
            If Not String.IsNullOrEmpty(Trim(Request("YearsOf"))) Then
                If IsNumeric(Trim(Request("YearsOf"))) Then
                    YearsOf = Trim(Request("YearsOf"))
                End If
            End If
        End If
        If Not IsNothing(Trim(Request("SalesWithin"))) Then
            If Not String.IsNullOrEmpty(Trim(Request("SalesWithin"))) Then
                If IsNumeric(Trim(Request("SalesWithin"))) Then
                    SalesWithin = Trim(Request("SalesWithin"))
                End If
            End If
        End If

        If Not IsNothing(Trim(Request("useModelOnly"))) Then
            If Not String.IsNullOrEmpty(Trim(Request("useModelOnly"))) Then
                UseModelValueOnly = Trim(Request("useModelOnly"))
            End If
        End If
        'If Not IsNothing(Trim(Request("AFTTCurrent"))) Then
        '  If Not String.IsNullOrEmpty(Trim(Request("AFTTCurrent"))) Then
        '    AFTTCurrent = Trim(Request("AFTTCurrent"))
        '  End If
        'End If
        If Not IsNothing(Trim(Request("UseUsed"))) Then
            If Not String.IsNullOrEmpty(Trim(Request("UseUsed"))) Then
                UseUsed = Trim(Request("UseUsed"))
            End If
        End If
        If Not IsNothing(Trim(Request("UseJetnet"))) Then
            If Not String.IsNullOrEmpty(Trim(Request("UseJetnet"))) Then
                UseJetnet = Trim(Request("UseJetnet"))
            End If
        End If
        If Not IsNothing(Trim(Request("VAR"))) Then
            If Not String.IsNullOrEmpty(Replace(Trim(Request("VAR")), "undefined", "")) Then
                variantList = AmodID.ToString & "," & Trim(Request("VAR"))
            End If
        End If

        If Not IsNothing(Trim(Request("internal"))) Then
            If Not String.IsNullOrEmpty(Trim(Request("internal"))) Then
                internal = Trim(Request("internal"))
            End If
        End If

        If Not IsNothing(Trim(Request("retail"))) Then
            If Not String.IsNullOrEmpty(Trim(Request("retail"))) Then
                retail = Trim(Request("retail"))
            End If
        End If

        If Not IsNothing(Trim(Request("aftt_end"))) Then
            If IsNumeric(Trim(Request("aftt_end"))) Then
                afttEnd = Trim(Request("aftt_end"))
            End If
        End If

        If Not IsNothing(Trim(Request("aftt_start"))) Then
            If IsNumeric(Trim(Request("aftt_start"))) Then
                afttStart = Trim(Request("aftt_start"))
            End If
        End If

        If Not IsNothing(Trim(Request("year_start"))) Then
            If IsNumeric(Trim(Request("year_start"))) Then
                yearStart = Trim(Request("year_start"))
            End If
        End If

        If Not IsNothing(Trim(Request("year_end"))) Then
            If IsNumeric(Trim(Request("year_end"))) Then
                yearEnd = Trim(Request("year_end"))
            End If
        End If

        aclsData_Temp.JETNET_DB = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
        aclsData_Temp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase") 'CApplication.Item("crmClientDatabase") 'HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim

        localDatalayer = New viewsDataLayer
        localDatalayer.adminConnectStr = Application.Item("crmClientSiteData").AdminDatabaseConn
        localDatalayer.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
        localDatalayer.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
        localDatalayer.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
        localDatalayer.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim


        If noteID > 0 Then
            If UseModelValueOnly = False Then
                Dim NoteTable As New DataTable
                NoteTable = aclsData_Temp.Get_Local_Notes_Client_NoteID(noteID)

                If Not IsNothing(NoteTable) Then
                    If NoteTable.Rows.Count > 0 Then
                        If NoteTable.Rows(0).Item("lnote_client_ac_id") > 0 Then
                            AircraftID = NoteTable.Rows(0).Item("lnote_client_ac_id")
                        Else
                            AircraftID = NoteTable.Rows(0).Item("lnote_jetnet_ac_id")
                        End If
                        JetnetACID = NoteTable.Rows(0).Item("lnote_jetnet_ac_id")

                        If JetnetACID > 0 Then
                            localDatalayer.Get_AC_MAKE_MODEL(JetnetACID, MakeName, ModelName, AmodID, PageTitleText, "", YearOfCurrent, AFTTCurrent)
                        End If
                    End If
                End If
            End If
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim HTMLString As String = ""
        Dim SearchCriteria As New viewSelectionCriteriaClass

        If Not genericReport Then

            If AmodID > 0 And viewID > 0 Then
                SearchCriteria.ViewCriteriaAmodID = AmodID
                SearchCriteria.ViewID = viewID
                If clsGeneral.clsGeneral.isCrmDisplayMode() Then 'Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CRM Then
                    CRMViewActive = True
                    NoMaster = True

                    If variantList <> "" Then
                        SearchCriteria.ViewCriteriaAmodIDArray = Split(variantList, ",")
                    End If


                    If IsNumeric(afttEnd) Then
                        If afttEnd > 0 Then
                            SearchCriteria.ViewCriteriaAFTTEnd = afttEnd
                        End If
                    End If

                    If IsNumeric(afttStart) Then
                        If afttStart > 0 Then
                            SearchCriteria.ViewCriteriaAFTTStart = afttStart
                        End If
                    End If

                    If IsNumeric(yearEnd) Then
                        If yearEnd > 0 Then
                            SearchCriteria.ViewCriteriaYearEnd = yearEnd
                        End If
                    End If

                    If IsNumeric(yearStart) Then
                        If yearStart > 0 Then
                            SearchCriteria.ViewCriteriaYearStart = yearStart
                        End If
                    End If
                End If


                Select Case viewType
                    Case "sales"

                        Dim ClientIDsToExclude As String = ""
                        Dim ClientTable As New DataTable
                        Dim JetnetTable As New DataTable
                        Dim HoldTable As New DataTable
                        'We need to use a special view for this
                        typeOfView.Text = "Market Survey"
                        Select Case viewID
                            Case "19"
                                If UseModelValueOnly Then
                                    urlRefresh += "&activetab=0"
                                    Session.Item("ViewActiveTab") = 0
                                Else
                                    urlRefresh += "&activetab=1"
                                    Session.Item("ViewActiveTab") = 1
                                End If
                            Case Else

                                urlRefresh += "&activetab=1"
                        End Select

                        If CRMViewActive Then
                            If Trim(ClientIDsToExclude) = "" Then


                                'Get the client market summary data.
                                ClientTable = crmViewDataLayer.CLIENT_get_fleet_market_summary_info(SearchCriteria, False)
                                'Get the jetnet market summary data
                                JetnetTable = commonEvo.get_fleet_market_summary_info(SearchCriteria, False)
                                'Combine then and set the IDs to exclude to be used later on the forsale tab
                                ClientIDsToExclude = crmViewDataLayer.CombineTwoAircraftDatatables(ClientTable, JetnetTable, HoldTable, "", False)
                            End If
                        End If

                        crmViewDataLayer.Build_For_sale_tab(SearchCriteria, HTMLString, noteID, "", viewID, CRMViewActive, extra_criteria, "", "", localDatalayer, "", ClientIDsToExclude, AircraftID, True, 0, "", True, exportID, aclsData_Temp, 1, False, "", UseModelValueOnly)
                        CheckAndJSForDatatable()
                    Case "sold"
                        typeOfView.Text = "Sold Survey"
                        Select Case viewID
                            Case "19"
                                If UseModelValueOnly Then
                                    urlRefresh += "&activetab=5"
                                    Session.Item("ViewActiveTab") = 5
                                Else
                                    urlRefresh += "&activetab=5"
                                End If
                            Case Else
                                urlRefresh += "&activetab=2"
                        End Select

                        If CRMViewActive And viewID = 1 Then
                            SearchCriteria.ViewCriteriaTimeSpan = YearsMonthsSettings
                            crmViewDataLayer.Combined_views_display_recent_retail_sales(SearchCriteria, HTMLString, localDatalayer, CRMViewActive, True, Nothing, IIf(internal, "Y", "N"), IIf(retail, "Y", "N"), False, noteID, AircraftID, LastSaveDate, 1, 0, "", True, "", "", "", "", "", "", "", "", "", YearsMonthsSettings, "", YearsOf, SalesWithin, YearOfCurrent, AFTTCurrent, "", "", "", "", "", "", "", "", "", UseUsed, UseJetnet, "", "", False, "", "", UseModelValueOnly)
                        ElseIf CRMViewActive Then
                            crmViewDataLayer.Combined_views_display_recent_retail_sales(SearchCriteria, HTMLString, localDatalayer, CRMViewActive, True, Nothing, IIf(internal, "Y", "N"), IIf(retail, "Y", "N"), False, noteID, AircraftID, LastSaveDate, 1, 0, "", True, "", "", "", "", "", "", "", "", "", YearsMonthsSettings, "", YearsOf, SalesWithin, YearOfCurrent, AFTTCurrent, "", "", "", "", "", "", "", "", "", UseUsed, UseJetnet, "", "", False, "", "", UseModelValueOnly)
                        Else
                            SearchCriteria.ViewCriteriaTimeSpan = 6
                            localDatalayer.views_display_recent_retail_sales(SearchCriteria, HTMLString, IIf(internal, "Y", "N"), IIf(retail, "Y", "N"))
                        End If
                        RetailSalesJSCheck()
                    Case "value"
                        typeOfView.Text = "Value Estimates"
                        HTMLString = ValueEstimates()
                        ValueEstimateJavascript()
                        ValueEstimateJSCheck(False, False)
                End Select
            ElseIf Trim(Request("viewType")) = "dynamic" Then
                Dim ClientIDsToExclude As String = ""
                CRMViewActive = True
                NoMaster = True
                If Trim(Request("PageTitle")) <> "" Then
                    typeOfView.Text = Trim(Request("PageTitle"))
                Else
                    typeOfView.Text = ""
                End If
                'Flights has no link anymore.
                'If typeOfView.Text = "Flights" Then
                '  'toggleFlightsOn.Visible = True
                '  'DisplayFlightTableAndJS(HTMLString, SearchCriteria)
                'Else
                HTMLString = selection_listing.Build_Dynamic_Listing()
                CheckAndJSForDynamicDatatable()
                'End If
            End If

            tableBase.Text = Replace(HTMLString, "View Extra Criteria", "")
            tableBase.Visible = True

            If Trim(Request("PageTitle")) <> "" Then
                refreshClose.Attributes.Add("href", "javascript:close();")
            Else
                refreshClose.Attributes.Add("href", "javascript:close(); window.opener.location.href = ''; window.opener.location.href = '" + urlRefresh + "&ref=true" + IIf(NoMaster, "&noMaster=false&ViewName=", "") + "';")
            End If

            pnl_generic_report.Visible = False

        Else
            refreshClose.Attributes.Add("href", "javascript:close();")
            pnl_generic_report.Visible = True
            displayGenericReport()
        End If

    End Sub
    'Private Sub DisplayFlightTableAndJS(ByRef htmlString As String, ByVal searchCriteria As viewSelectionCriteriaClass)
    '  Dim resultsTable As New DataTable
    '  Dim JavascriptForTable As String = ""
    '  Dim ColumnList As String = ""
    '  HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "TOP 1000", "")
    '  resultsTable = aclsData_Temp.Run_Selection_Listing_Query()

    '  If Not IsNothing(resultsTable) Then
    '    htmlString = ""



    '    'So I don't want paging here, but I don't want to rebuild the same exact function the view master is already using. I'm just going to borrow it.
    '    JavascriptForTable = "window.onload = function() {" & DisplayFunctions.ConvertDataTableToArrayCombinedFields(resultsTable, ColumnList, searchCriteria, False, 0) & ";" & View_Master.BuildTable(False, 0, ColumnList) & ";};"

    '    System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "CreateDatatableOnLoad", JavascriptForTable, True)
    '  End If
    'End Sub
    Public Sub CreateDynamicTable(ByRef ScriptBu As String)
        Dim ExcelButton As String = ""
        Dim EveryRowScript As String = ""
        Dim ReloadScript As String = ""

        'If CRMViewActive And viewID = 19 Then
        '  EveryRowScript = "var data = dt.rows().column(1).data();"
        '  EveryRowScript += "var IDsToUse ='0|CLIENT, 0|JETNET';"
        '  EveryRowScript += "data.each(function (value, index) {"
        '  EveryRowScript += " if (IDsToUse.length > 0) {"
        '  EveryRowScript += " IDsToUse += ', '; "
        '  EveryRowScript += " }"
        '  EveryRowScript += "IDsToUse += ' ' + value;"
        '  EveryRowScript += "});"
        '  EveryRowScript += "$(""#" & fullSaleCurrentIDs.ClientID & """).val(IDsToUse);"
        '  ReloadScript = "ChangeTheMouseCursorOnItemParentDocument('cursor_wait');$(""#" & fullSaleCurrentIDs.ClientID & """).val('');"
        '  ReloadScript += "$(""#" & FullSaleRefresh.ClientID & """).click();"
        'End If

        ScriptBu += "function RedrawDatatablesOnSys() {"
        ScriptBu += "setTimeout(reRenderThem, 1800);"
        ScriptBu += "}"

        ScriptBu += "function reRenderThem() {"
        ScriptBu += "$($.fn.dataTable.tables(true)).DataTable().columns.adjust();"
        ScriptBu += "$($.fn.dataTable.tables(true)).DataTable().scroller.measure();"
        ScriptBu += "}"



        clsGeneral.clsGeneral.CreateExcelButton(ExcelButton, "MyJqueryTable")
        ScriptBu += "function CreateTheDatatable() { "

        'Adding this check to destroy a table if one already exists:
        ScriptBu += " if ($.fn.DataTable.isDataTable( '#MyJqueryTable' ) ) {"
        ScriptBu += "$('#forSaleInnerTable').empty();"
        ScriptBu += "};"

        ScriptBu += "if ( $(""#tableCopy"").length ) {"
        ScriptBu += "jQuery(""#tableCopy"").css('display','block');"
        ScriptBu += "var clone = jQuery(""#tableCopy"").clone(true);"
        ScriptBu += "jQuery(""#tableCopy"").css('display','none');"

        ScriptBu += "clone[0].setAttribute('id', 'MyJqueryTable');"
        ScriptBu += "clone.appendTo(""#forSaleInnerTable"");"


        ScriptBu += " var table = $('#MyJqueryTable').DataTable({"
        ScriptBu += " destroy: true,"

        ScriptBu += """initComplete"": function(settings, json) {"

        ScriptBu += "setTimeout(function(){"
        ScriptBu += "$('#MyJqueryTable').DataTable().columns.adjust();"
        ScriptBu += "$('#MyJqueryTable').DataTable().fixedColumns().relayout();"

        ScriptBu += "},1200)"
        ScriptBu += "},"


        If CRMViewActive Then
            ScriptBu += """infoCallback"": function( settings, start, end, max, total, pre ) {"
            ScriptBu += "return end + "" entries"";"
            ScriptBu += "},"
        End If


        ScriptBu += "scrollCollapse: true,"
        ScriptBu += " stateSave: true,"
        ScriptBu += "paging: false,"


        ScriptBu += "columnDefs: [ "
        'If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CRM Then
        '  If viewID = 19 And CRMViewActive Then
        '    ScriptBu += " {"
        '    ScriptBu += "targets: [ 1 , 2],"
        '    ScriptBu += "className: 'display_none'"
        '    ScriptBu += "},"
        '  ElseIf CRMViewActive Then
        '    ScriptBu += " {"
        '    ScriptBu += "targets: [1],"
        '    ScriptBu += "className: 'display_none'"
        '    ScriptBu += "},"
        '  End If


        '  ScriptBu += " {"
        '  If exportID = 0 Then
        '    If viewID = 19 Then
        '      ScriptBu += "targets: [3],"
        '    Else
        '      ScriptBu += "targets: [2],"
        '    End If

        '  Else
        '    ScriptBu += "targets: [2],"
        '  End If
        '  ScriptBu += "orderable: false"
        '  ScriptBu += "}, "
        'ElseIf viewID = 1 Then 'And CRMViewActive = False Then
        ' ScriptBu += " {"
        ' ScriptBu += "targets: [ 1 " & IIf(CRMViewActive, ",2", "") & " ],"
        '  ScriptBu += "className: 'display_none'"
        '  ScriptBu += "},"
        '   End If



        ScriptBu += " {"
        ScriptBu += "orderable: false,"
        ScriptBu += "className:  'select-checkbox',"
        ScriptBu += " width: '10px',"
        ScriptBu += "targets:   0"
        ScriptBu += " } ],"
        ScriptBu += "select: {"
        ScriptBu += "style:    'multi',"
        ScriptBu += "selector: 'td:first-child'"
        ScriptBu += "},"

        If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CRM Then
            If viewID = 19 Then
                ScriptBu += "order: [[ 6, 'asc' ]],"
            Else
                ScriptBu += "order: [[ 4, 'asc' ]],"
            End If
        Else
            ScriptBu += "order: [[ 1, 'asc' ]],"
        End If


        ScriptBu += "dom:        'Bfitrp',"
        ScriptBu += "buttons: [ "


        'CSV Button:
        ScriptBu += " {extend: 'csv', exportOptions : {columns: ':visible'}}, "
        'Excel Button
        ScriptBu += ExcelButton
        'PDF Button
        ScriptBu += " {extend: 'pdf', orientation: 'landscape', pageSize: 'A2', exportOptions : {columns: ':visible'}}, "
        'Column Visibility Button
        ScriptBu += "{"
        ScriptBu += "extend: 'colvis',"
        ScriptBu += " text: 'Columns',"
        ScriptBu += "collectionLayout:  'fixed two-column',"
        ScriptBu += "postfixButtons: [ 'colvisRestore' ]"
        ScriptBu += "},"

        'Remove Selected Button:
        ScriptBu += "{ text:'Remove Selected', "
        ScriptBu += " action: function( e, dt, node, config) { dt.rows( { selected: true } ).remove().draw( false );" & IIf(CRMViewActive = True And viewID = 19, EveryRowScript, "") & "}},"
        ScriptBu += "{ text:'Keep Selected', "
        ScriptBu += " action: function( e, dt, node, config) { dt.rows( { selected: false } ).remove().draw( false );dt.draw();dt.rows('.selected').deselect();" & IIf(CRMViewActive = True And viewID = 19, EveryRowScript, "") & "}},"

        ScriptBu += "{ text:'Reload Table', action: function( e, dt, node, config) { $('#forSaleInnerTable').empty();CreateTheDatatable();" & IIf(CRMViewActive = True And viewID = 19, ReloadScript, "") & "}}"
        ScriptBu += " "


        ScriptBu += "]"
        ScriptBu += "});"
        ScriptBu += "} "

        ScriptBu += "}; CreateTheDatatable();"


    End Sub
    Public Sub CreateSalesTable(ByRef ScriptBu As String)
        Dim ExcelButton As String = ""
        Dim EveryRowScript As String = ""
        Dim ReloadScript As String = ""

        If CRMViewActive And viewID = 19 Then
            EveryRowScript = "var data = dt.rows().column(1).data();"
            EveryRowScript += "var IDsToUse ='0|CLIENT, 0|JETNET';"
            EveryRowScript += "data.each(function (value, index) {"
            EveryRowScript += " if (IDsToUse.length > 0) {"
            EveryRowScript += " IDsToUse += ', '; "
            EveryRowScript += " }"
            EveryRowScript += "IDsToUse += ' ' + value;"
            EveryRowScript += "});"
            EveryRowScript += "$(""#" & fullSaleCurrentIDs.ClientID & """).val(IDsToUse);"
            ReloadScript = "ChangeTheMouseCursorOnItemParentDocument('cursor_wait');$(""#" & fullSaleCurrentIDs.ClientID & """).val('');"
            ReloadScript += "$(""#" & FullSaleRefresh.ClientID & """).click();"
        End If

        ScriptBu += "function RedrawDatatablesOnSys() {"
        ScriptBu += "setTimeout(reRenderThem, 1800);"
        ScriptBu += "}"

        ScriptBu += "function reRenderThem() {"
        ScriptBu += "$($.fn.dataTable.tables(true)).DataTable().columns.adjust();"
        ScriptBu += "$($.fn.dataTable.tables(true)).DataTable().scroller.measure();"
        ScriptBu += "}"



        clsGeneral.clsGeneral.CreateExcelButton(ExcelButton, "MyJqueryTable")
        ScriptBu += "function CreateTheDatatable() { "

        'Adding this check to destroy a table if one already exists:
        ScriptBu += " if ($.fn.DataTable.isDataTable( '#MyJqueryTable' ) ) {"
        ScriptBu += "$('#forSaleInnerTable').empty();"
        ScriptBu += "};"

        ScriptBu += "if ( $(""#tableCopy"").length ) {"
        ScriptBu += "jQuery(""#tableCopy"").css('display','block');"
        ScriptBu += "var clone = jQuery(""#tableCopy"").clone(true);"
        ScriptBu += "jQuery(""#tableCopy"").css('display','none');"

        ScriptBu += "clone[0].setAttribute('id', 'MyJqueryTable');"
        ScriptBu += "clone.appendTo(""#forSaleInnerTable"");"


        ScriptBu += " var table = $('#MyJqueryTable').DataTable({"
        ScriptBu += " destroy: true,"

        ScriptBu += """initComplete"": function(settings, json) {"

        ScriptBu += "setTimeout(function(){"
        ScriptBu += "$('#MyJqueryTable').DataTable().columns.adjust();"
        ScriptBu += "$('#MyJqueryTable').DataTable().fixedColumns().relayout();"

        ScriptBu += "},1200)"
        ScriptBu += "},"


        If CRMViewActive Then
            ScriptBu += """infoCallback"": function( settings, start, end, max, total, pre ) {"
            ScriptBu += "return end + "" entries"";"
            ScriptBu += "},"
        End If


        ScriptBu += "scrollCollapse: true,"
        ScriptBu += " stateSave: true,"
        ScriptBu += "paging: false,"


        ScriptBu += "columnDefs: [ "
        If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CRM Then
            If viewID = 19 And CRMViewActive Then
                ScriptBu += " {"
                ScriptBu += "targets: [ 1 , 2],"
                ScriptBu += "className: 'display_none'"
                ScriptBu += "},"
            ElseIf CRMViewActive Then
                ScriptBu += " {"
                ScriptBu += "targets: [1],"
                ScriptBu += "className: 'display_none'"
                ScriptBu += "},"
            End If


            ScriptBu += " {"
            If exportID = 0 Then
                If viewID = 19 Then
                    ScriptBu += "targets: [3],"
                Else
                    ScriptBu += "targets: [2],"
                End If

            Else
                ScriptBu += "targets: [2],"
            End If
            ScriptBu += "orderable: false"
            ScriptBu += "}, "
        ElseIf viewID = 1 Then 'And CRMViewActive = False Then
            ScriptBu += " {"
            ScriptBu += "targets: [ 1 " & IIf(CRMViewActive, ",2", "") & " ],"
            ScriptBu += "className: 'display_none'"
            ScriptBu += "},"
        End If



        ScriptBu += " {"
        ScriptBu += "orderable: false,"
        ScriptBu += "className:  'select-checkbox',"
        ScriptBu += " width: '10px',"
        ScriptBu += "targets:   0"
        ScriptBu += " } ],"
        ScriptBu += "select: {"
        ScriptBu += "style:    'multi',"
        ScriptBu += "selector: 'td:first-child'"
        ScriptBu += "},"

        If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CRM Then
            If viewID = 19 Then
                ScriptBu += "order: [[ 6, 'asc' ]],"
            Else
                ScriptBu += "order: [[ 4, 'asc' ]],"
            End If
        Else
            ScriptBu += "order: [[ 1, 'asc' ]],"
        End If


        ScriptBu += "dom:        'Bfitrp',"
        ScriptBu += "buttons: [ "


        'CSV Button:
        ScriptBu += " {extend: 'csv', exportOptions : {columns: ':visible'}}, "
        'Excel Button
        ScriptBu += ExcelButton
        'PDF Button
        ScriptBu += " {extend: 'pdf', orientation: 'landscape', pageSize: 'A2', exportOptions : {columns: ':visible'}}, "
        'Column Visibility Button
        ScriptBu += "{"
        ScriptBu += "extend: 'colvis',"
        ScriptBu += " text: 'Columns',"
        ScriptBu += "collectionLayout:  'fixed two-column',"
        ScriptBu += "postfixButtons: [ 'colvisRestore' ]"
        ScriptBu += "},"

        'Remove Selected Button:
        ScriptBu += "{ text:'Remove Selected', "
        ScriptBu += " action: function( e, dt, node, config) { dt.rows( { selected: true } ).remove().draw( false );" & IIf(CRMViewActive = True And viewID = 19, EveryRowScript, "") & "}},"
        ScriptBu += "{ text:'Keep Selected', "
        ScriptBu += " action: function( e, dt, node, config) { dt.rows( { selected: false } ).remove().draw( false );dt.draw();dt.rows('.selected').deselect();" & IIf(CRMViewActive = True And viewID = 19, EveryRowScript, "") & "}},"

        ScriptBu += "{ text:'Reload Table', action: function( e, dt, node, config) { $('#forSaleInnerTable').empty();CreateTheDatatable();" & IIf(CRMViewActive = True And viewID = 19, ReloadScript, "") & "}}"
        ScriptBu += " "


        ScriptBu += "]"
        ScriptBu += "});"
        ScriptBu += "} "

        ScriptBu += "}; CreateTheDatatable();"


    End Sub
    Private Sub CheckAndJSForDynamicDatatable()

        Dim JavascriptForTable As String = ""

        Call CreateDynamicTable(JavascriptForTable)

        JavascriptForTable = "window.onload = function() {" & JavascriptForTable & ";" & IIf(CRMViewActive, "ChangeTheMouseCursorOnItemParentDocument('cursor_default');", "") & "};"

        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "CreateDatatableOnLoad", JavascriptForTable, True)

    End Sub
    Private Sub CheckAndJSForDatatable()

        Dim JavascriptForTable As String = ""

        Call CreateSalesTable(JavascriptForTable)

        JavascriptForTable = "window.onload = function() {" & JavascriptForTable & ";" & IIf(CRMViewActive, "ChangeTheMouseCursorOnItemParentDocument('cursor_default');", "") & "};"

        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "CreateDatatableOnLoad", JavascriptForTable, True)

    End Sub

    Private Function ValueEstimates() As String
        Dim resultsTable As New DataTable
        Dim ExtraQuery As String = ""
        Dim ResultsString As String = ""
        If YearsMonthsSettings > 0 Then
            Dim TempDateHold As Date = DateAdd(DateInterval.Month, -YearsMonthsSettings, Now())
            ExtraQuery = " AND clival_entry_date >= '" & Year(TempDateHold) & "-" & Month(TempDateHold) & "-" & Day(TempDateHold) & "' "
        End If

        If Not String.IsNullOrEmpty(valueEstimateCurrentID.Text) Then
            ExtraQuery += " AND clival_id IN (" & clsGeneral.clsGeneral.StripChars(Trim(valueEstimateCurrentID.Text), False) & ") "
        End If

        resultsTable = localDatalayer.get_my_ac_value_history_comparables(AircraftID, "est_value", True, JetnetACID, noteID, "O", False, AmodID, ExtraQuery)

        ResultsString = BuildValueEstimatesTable(resultsTable)
        Return ResultsString
    End Function

    Public Function BuildValueEstimatesTable(ByVal ResultsTable As DataTable) As String
        Dim Temp_Label As String = ""
        Dim ResultsString As String = ""
        Dim dateSort As String = ""

        Dim date_changed As Boolean = False
        Dim temp_date As String = ""
        Dim last_date As String = ""
        Dim temp_desc As String = ""
        Dim color As String = ""
        Dim temp_num As Long = 0
        For Each r As DataRow In ResultsTable.Rows

            If Not IsDBNull(r("date_of")) Then
                temp_date = FormatDateTime(r("date_of"), DateFormat.ShortDate)
                dateSort = Format(r("date_of"), "yyyy/MM/dd")
            End If

            If (Trim(temp_date) <> Trim(last_date)) And Trim(last_date) <> "" Then
                date_changed = True
            Else
                date_changed = False
            End If

            If Not IsDBNull(r("description")) Then
                temp_desc = Trim(r("description"))

                If Trim(temp_desc) = "F" Then
                    temp_desc = "Full Appraisal"
                ElseIf Trim(temp_desc) = "D" Then
                    temp_desc = "Desktop Appraisal"
                ElseIf Trim(temp_desc) = "V" Then
                    temp_desc = "VREF"
                ElseIf Trim(temp_desc) = "B" Then
                    temp_desc = "Blue Book"
                ElseIf Trim(temp_desc) = "H" Then
                    temp_desc = "HeliValue$"
                End If

                If color = "alt_row" Then
                    color = ""
                Else
                    color = "alt_row"
                End If

                Temp_Label &= "<tr class='" & color & "'>"
                Temp_Label &= "<td align='left'>&nbsp;</td>"
                Temp_Label &= "<td align='left'>" & r("clival_id") & "</td>"
                Temp_Label &= "<td align='left'><font size='-2' style='font-family: Arial'  data-sort='" & dateSort & "'>" & temp_date & "</font></td>"
                Temp_Label &= "<td align='left'><font size='-2' style='font-family: Arial'>" & temp_desc & "</font></td>"

                If Not IsDBNull(r("ac_id")) Then
                    Dim TemporaryAircraftTable As New DataTable

                    TemporaryAircraftTable = aclsData_Temp.GetJETNET_AC_NAME(r("ac_id"), "")

                    If Not IsNothing(TemporaryAircraftTable) Then
                        If TemporaryAircraftTable.Rows.Count > 0 Then
                            'Make
                            Temp_Label &= "<td align='left'>"
                            If Not IsDBNull(TemporaryAircraftTable.Rows(0).Item("amod_make_name")) Then
                                Temp_Label &= TemporaryAircraftTable.Rows(0).Item("amod_make_name").ToString
                            End If
                            'temp_label &= "</td>"
                            'Model
                            'temp_label &= "<td align='left'>"
                            If Not IsDBNull(TemporaryAircraftTable.Rows(0).Item("amod_model_name")) Then
                                Temp_Label &= TemporaryAircraftTable.Rows(0).Item("amod_model_name").ToString
                            End If
                            Temp_Label &= "</td>"
                            'Ser
                            Temp_Label &= "<td align='left'>"
                            If Not IsDBNull(TemporaryAircraftTable.Rows(0).Item("ac_ser_nbr")) Then
                                Temp_Label &= TemporaryAircraftTable.Rows(0).Item("ac_ser_nbr").ToString
                            End If
                            Temp_Label &= "</td>"
                            'Reg
                            Temp_Label &= "<td align='left'>"
                            If Not IsDBNull(TemporaryAircraftTable.Rows(0).Item("ac_reg_nbr")) Then
                                Temp_Label &= TemporaryAircraftTable.Rows(0).Item("ac_reg_nbr").ToString
                            End If
                            Temp_Label &= "</td>"
                            'Year
                            Temp_Label &= "<td align='left'>"
                            If Not IsDBNull(TemporaryAircraftTable.Rows(0).Item("ac_year_mfr")) Then
                                Temp_Label &= TemporaryAircraftTable.Rows(0).Item("ac_year_mfr").ToString
                            End If
                            Temp_Label &= "</td>"
                        End If
                    End If
                    TemporaryAircraftTable.Dispose()
                Else
                    Temp_Label &= "<td align='left'><font size='-2' style='font-family: Arial'>&nbsp;</font></td>"
                End If


                If Not IsDBNull(r("asking_price")) Then
                    If CInt(r("asking_price")) > 0 Then
                        temp_num = r("asking_price")
                        temp_num = (temp_num / 1000)
                        Temp_Label &= "<td align='right'><font size='-2' style='font-family: Arial'>$" & FormatNumber(temp_num, 0) & "k</font></td>"
                    Else
                        Temp_Label &= "<td align='left'>&nbsp;</td>"
                    End If
                Else
                    Temp_Label &= "<td align='left'>&nbsp;</td>"
                End If


                If Not IsDBNull(r("take_price")) Then
                    If CInt(r("take_price")) > 0 Then
                        temp_num = r("take_price")
                        temp_num = (temp_num / 1000)
                        Temp_Label &= "<td align='right'><font size='-2' style='font-family: Arial'>$" & FormatNumber(temp_num, 0) & "k</font></td>"
                    Else
                        Temp_Label &= "<td align='left'>&nbsp;</td>"
                    End If
                Else
                    Temp_Label &= "<td align='left'>&nbsp;</td>"
                End If


                If Not IsDBNull(r("sold_price")) Then
                    If CInt(r("sold_price")) > 0 Then
                        temp_num = r("sold_price")
                        temp_num = (temp_num / 1000)
                        Temp_Label &= "<td align='right'><font size='-2' style='font-family: Arial'>$" & FormatNumber(temp_num, 0) & "k</font></td>"
                    Else
                        Temp_Label &= "<td align='left'>&nbsp;</td>"
                    End If
                Else
                    Temp_Label &= "<td align='left'>&nbsp;</td>"
                End If

                If Not IsDBNull(r("clival_aftt_hours")) Then
                    If r("clival_aftt_hours") > 0 Then
                        Temp_Label &= "<td align='right'><font size='-2' style='font-family: Arial'>" & FormatNumber(r("clival_aftt_hours"), 0) & "</font></td>"
                    Else
                        Temp_Label &= "<td align='left'>&nbsp;</td>"
                    End If
                Else
                    Temp_Label &= "<td align='left'>&nbsp;</td>"
                End If

                If Not IsDBNull(r("clival_total_landings")) Then
                    If r("clival_total_landings") > 0 Then
                        Temp_Label &= "<td align='right'><font size='-2' style='font-family: Arial'>" & FormatNumber(r("clival_total_landings"), 0) & "</font></td>"
                    Else
                        Temp_Label &= "<td align='left'>&nbsp;</td>"
                    End If
                Else
                    Temp_Label &= "<td align='left'>&nbsp;</td>"
                End If


                Temp_Label &= "</tr>"


                last_date = Trim(temp_date)
            End If

        Next


        ResultsString = "<span id=""ValueEstimateNewContents""><table id=""ValueEstimateCopy"" cellspacing='0' cellpadding='3' border='1' class='engine' width=""100%"">"
        ResultsString += "<thead><tr>"
        ResultsString += "<th>SEL</th>"
        ResultsString += "<th>ID</th>"
        ResultsString += "<th><font size='-2' style='font-family: Arial'><b>Date</b></font></th><th><font size='-2' style='font-family: Arial'><b>Type</b></font></th>"
        ResultsString += "<th><font size='-2' style='font-family: Arial'><b>Make/Model</b></font></th>"
        ResultsString += "<th><font size='-2' style='font-family: Arial'><b>Ser #</b></font></th>"
        ResultsString += "<th><font size='-2' style='font-family: Arial'><b>Reg #</b></font></th>"
        ResultsString += "<th><font size='-2' style='font-family: Arial'><b>Year MFR</b></font></th>"

        ResultsString += "<th><font size='-2' style='font-family: Arial'><b>Asking($k)</b></font></th><th><font size='-2' style='font-family: Arial'><b>Take($k)</b></font></th><th><font size='-2' style='font-family: Arial'><b>Est Value($k)</b></font></th>"
        ResultsString += "<th><font size='-2' style='font-family: Arial'><b>AFTT</b></font></th><th><font size='-2' style='font-family: Arial'><b>Total Landings</b></font></th>"
        ResultsString += "</thead></tr>"
        ResultsString += "<tbody"
        ResultsString += Temp_Label
        ResultsString += "</tbody>"
        ResultsString += "</table>"
        ResultsString += "</span><div id=""ValueEstimateInnerTable"" style=""width: 960px;""></div>"
        Return ResultsString

    End Function

    Public Sub RetailSalesJavascript()
        Dim ScriptBu As String = ""
        Dim ExcelButton As String = ""
        Dim EveryRowScript As String = ""
        Dim ReloadScript As String = ""

        If CRMViewActive And viewID = 19 Then
            EveryRowScript = "var data = dt.rows().column(1).data();"
            EveryRowScript += "var IDsToUse ='0|CLIENT, 0|JETNET';"
            EveryRowScript += "data.each(function (value, index) {"
            EveryRowScript += " if (IDsToUse.length > 0) {"
            EveryRowScript += " IDsToUse += ', '; "
            EveryRowScript += " }"
            EveryRowScript += "IDsToUse += ' ' + value;"
            EveryRowScript += "});"
            EveryRowScript += "$(""#" & SoldSurveyCurrentID.ClientID & """).val(IDsToUse);"

        End If

        clsGeneral.clsGeneral.CreateExcelButton(ExcelButton, "RetailsTable")

        ScriptBu += "function CreateRetailsDatatable() {"
        'Adding this check to destroy a table if one already exists:
        ScriptBu += " if ($.fn.DataTable.isDataTable( '#RetailsTable' ) ) {"
        ScriptBu += "$('#RetailInnerTable').empty();"
        ScriptBu += "};"

        ScriptBu += "if ( $(""#retailSalesCopy"").length ) {"
        ScriptBu += "jQuery(""#retailSalesCopy"").css('display','block');"
        ScriptBu += "var clone = jQuery(""#retailSalesCopy"").clone(true);"
        ScriptBu += "jQuery(""#retailSalesCopy"").css('display','none');"

        ScriptBu += "clone[0].setAttribute('id', 'RetailsTable');"
        ScriptBu += "clone.appendTo(""#RetailInnerTable"");"

        ScriptBu += "$('#RetailsTable').DataTable({"

        ScriptBu += " destroy: true,"

        ScriptBu += """initComplete"": function(settings, json) {"
        ScriptBu += "setTimeout(function(){"

        ScriptBu += "$('#RetailsTable').DataTable().columns.adjust();"
        ScriptBu += "$('#RetailsTable').DataTable().fixedColumns().relayout();"
        ScriptBu += "},1200)"
        ScriptBu += "},"


        If CRMViewActive Then
            ScriptBu += """infoCallback"": function( settings, start, end, max, total, pre ) {"
            ScriptBu += "return end + "" entries"";"
            ScriptBu += "},"
        End If



        ScriptBu += "scrollCollapse: true,"
        ScriptBu += " stateSave: true,"
        ScriptBu += "paging: false, "
        ScriptBu += "columnDefs: [ "

        If CRMViewActive = True And viewID = 19 Then
            ScriptBu += " {"
            ScriptBu += "targets: [ 1,2 ],"
            ScriptBu += "className: 'display_none'"
            ScriptBu += "},"
        ElseIf CRMViewActive Then
            ScriptBu += " {"
            ScriptBu += "targets: [1,2],"
            ScriptBu += "className: 'display_none'"
            ScriptBu += "},"
        ElseIf CRMViewActive = False And viewID = 1 Then
            ScriptBu += " {"
            ScriptBu += "targets: [1],"
            ScriptBu += "className: 'display_none'"
            ScriptBu += "},"
        End If

        ScriptBu += " {"
        ScriptBu += "targets: [2,0" & IIf(viewID = 19, ",3", "") & "],"
        ScriptBu += "orderable: false"
        ScriptBu += "}, "

        ScriptBu += " {"
        ScriptBu += "orderable: false,"
        ScriptBu += "className:  'select-checkbox',"
        ScriptBu += " width: '10px',"
        ScriptBu += "targets:   0"
        ScriptBu += " }"
        ScriptBu += "],"
        ScriptBu += "select: {"
        ScriptBu += "style:    'multi',"
        ScriptBu += "selector: 'td:first-child'"
        ScriptBu += "},"
        ScriptBu += "dom:        'Bfitrp',"


        If viewID = 1 Then
            ScriptBu += "order: [[ 1, 'asc' ]],"
        ElseIf viewID = 19 Then
            ScriptBu += "order: [[ 4, 'asc' ]],"
        Else
            ScriptBu += "order: [[ 3, 'asc' ]],"
        End If

        ScriptBu += "buttons: [ "


        ScriptBu += " {extend: 'csv', exportOptions : {columns: ':visible'}}, "
        'Excel Button
        ScriptBu += ExcelButton
        'PDF Button
        ScriptBu += " {extend: 'pdf', orientation: 'landscape', pageSize: 'A2', exportOptions : {columns: ':visible'}}, "


        'Column Visibility Button
        ScriptBu += "{"
        ScriptBu += "extend: 'colvis',"
        ScriptBu += " text: 'Columns',"
        ScriptBu += "collectionLayout:  'fixed two-column',"
        ScriptBu += "postfixButtons: [ 'colvisRestore' ]"
        ScriptBu += "},"
        'Remove Selected Button:
        ScriptBu += "{ text:'Remove Selected', "
        ScriptBu += " action: function( e, dt, node, config) { dt.rows( { selected: true } ).remove().draw( false ); " & IIf(CRMViewActive = True And viewID = 19, EveryRowScript, "") & "}},"

        ScriptBu += "{ text:'Keep Selected', "
        ScriptBu += " action: function( e, dt, node, config) { dt.rows( { selected: false } ).remove().draw( false );dt.draw();dt.rows('.selected').deselect();" & IIf(CRMViewActive = True And viewID = 19, EveryRowScript, "") & "}},"
        ScriptBu += "{ text:'Reload Table', action: function( e, dt, node, config) { $('#RetailInnerTable').empty();CreateRetailsDatatable();RedrawDatatablesOnSys();" & IIf(CRMViewActive = True And viewID = 19, ReloadScript, "") & "}}"


        ScriptBu += "] "

        ScriptBu += " });}"
        ScriptBu += ";}"
        ScriptBu += "$($.fn.dataTable.tables(true)).DataTable().columns.adjust();"
        ScriptBu += "$($.fn.dataTable.tables(true)).DataTable().scroller.measure();"

        System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CreateRetailsDatatableFunction", ScriptBu.ToString, True)
    End Sub

    Private Sub RetailSalesJSCheck()
        'If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CRM Then
        Dim scriptBuOnLoad As String = ""
        Dim ScriptBuPostback As String = ""

        RetailSalesJavascript()
        'First we need to check - is this table already initialized?
        'This is important because it doesn't need to be ran twice.

        scriptBuOnLoad = ScriptBuPostback & "CreateRetailsDatatable();"
        ScriptBuPostback += "Sys.Application.add_load(function() {CreateRetailsDatatable();ChangeTheMouseCursorOnItemParentDocument('cursor_default');});"


        scriptBuOnLoad = "window.onload = function() {" & scriptBuOnLoad & ";RedrawDatatablesOnSys();ChangeTheMouseCursorOnItemParentDocument('cursor_default');};"
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "CreateRDatatableOnLoad", scriptBuOnLoad, True)
    End Sub

    Public Sub ValueEstimateJavascript()
        Dim ScriptBu As String = ""
        Dim ExcelButton As String = ""
        Dim EveryRowScript As String = ""
        EveryRowScript = "var data = dt.rows().column(1).data();"
        EveryRowScript += "var IDsToUse ='';"
        EveryRowScript += "data.each(function (value, index) {"
        EveryRowScript += " if (IDsToUse.length > 0) {"
        EveryRowScript += " IDsToUse += ', '; "
        EveryRowScript += " }"
        EveryRowScript += "IDsToUse += ' ' + value;"
        EveryRowScript += "});"
        EveryRowScript += "$(""#" & valueEstimateCurrentID.ClientID & """).val(IDsToUse);"
        clsGeneral.clsGeneral.CreateExcelButton(ExcelButton, "ValueEstimateTable")

        ScriptBu += "function CreateValueEstimateDatatable() {"
        'ScriptBu += "$(""#" & valueEstimateCurrentID.ClientID & """).val('');"
        'Adding this check to destroy a table if one already exists:
        ScriptBu += " if ($.fn.DataTable.isDataTable( '#ValueEstimateTable' ) ) {"
        ScriptBu += "$('#ValueEstimateInnerTable').empty();"
        ScriptBu += "};"

        ScriptBu += "if ( $(""#ValueEstimateCopy"").length ) {"
        ScriptBu += "jQuery(""#ValueEstimateCopy"").css('display','block');"
        ScriptBu += "var clone = jQuery(""#ValueEstimateCopy"").clone(true);"
        ScriptBu += "jQuery(""#ValueEstimateCopy"").css('display','none');"

        ScriptBu += "clone[0].setAttribute('id', 'ValueEstimateTable');"
        ScriptBu += "clone.appendTo(""#ValueEstimateInnerTable"");"

        ScriptBu += "$('#ValueEstimateTable').DataTable({"

        ScriptBu += " destroy: true,"

        ScriptBu += """initComplete"": function(settings, json) {"
        ScriptBu += "setTimeout(function(){"
        ScriptBu += "$('#ValueEstimateTable').DataTable().columns.adjust();"
        ScriptBu += "$('#ValueEstimateTable').DataTable().fixedColumns().relayout();"
        ScriptBu += "},1200)"
        ScriptBu += "},"


        If CRMViewActive Then
            ScriptBu += """infoCallback"": function( settings, start, end, max, total, pre ) {"
            ScriptBu += "return end + "" entries"";"
            ScriptBu += "},"
        End If




        ScriptBu += "scrollCollapse: true,"
        ScriptBu += " stateSave: true,"
        ScriptBu += "paging: false, "
        ScriptBu += "columnDefs: [ "
        ScriptBu += " {"
        ScriptBu += "targets: [ 1 ],"
        ScriptBu += "visible: false"
        ScriptBu += "},"
        ScriptBu += " {"
        ScriptBu += "orderable: false,"
        ScriptBu += "className:  'select-checkbox',"
        ScriptBu += " width: '10px',"
        ScriptBu += "targets:   0"
        ScriptBu += " }],"
        ScriptBu += "select: {"
        ScriptBu += "style:    'multi',"
        ScriptBu += "selector: 'td:first-child'"
        ScriptBu += "},"
        ScriptBu += "dom:        'Bfitrp',"


        ScriptBu += "order: [[ 1, 'asc' ]],"


        ScriptBu += "buttons: [ "


        ScriptBu += " {extend: 'csv', exportOptions : {columns: ':visible'}}, "
        'Excel Button
        ScriptBu += ExcelButton
        'ScriptBu += " {extend: 'excel', exportOptions : {columns: ':visible'}}, "
        'PDF Button
        ScriptBu += " {extend: 'pdf', orientation: 'landscape', pageSize: 'A2', exportOptions : {columns: ':visible'}}, "

        'Print Button
        ScriptBu += " {extend: 'print', exportOptions : {columns: ':visible'}}, "
        'Column Visibility Button
        ScriptBu += "{"
        ScriptBu += "extend: 'colvis',"
        ScriptBu += "collectionLayout:  'fixed two-column',"
        ScriptBu += "postfixButtons: [ 'colvisRestore' ]"
        ScriptBu += "},"
        'Remove Selected Button:
        ScriptBu += "{ text:'Remove Selected', className: 'RemoveRowsValue', "
        ScriptBu += " action: function( e, dt, node, config) { dt.rows( { selected: true } ).remove().draw( false );"
        'ScriptBu += "var data = dt.rows().column(1).data();"
        'ScriptBu += "var IDsToUse ='';"
        'ScriptBu += "data.each(function (value, index) {"
        'ScriptBu += " if (IDsToUse.length > 0) {"
        'ScriptBu += " IDsToUse += ', '; "
        'ScriptBu += " }"
        'ScriptBu += "IDsToUse += ' ' + value;"
        'ScriptBu += "});"
        'ScriptBu += "$(""#" & valueEstimateCurrentID.ClientID & """).val(IDsToUse);"
        ScriptBu += EveryRowScript
        ' ScriptBu += "$(""#" & RefreshCurrentValueGraph.ClientID & """).click();"

        ScriptBu += "}},"
        ScriptBu += "{ text:'Keep Selected', action: function( e, dt, node, config) { dt.rows({ selected: false }).remove().draw(false);" & EveryRowScript & "dt.rows('.selected').deselect();}},"

        ScriptBu += "{ text:'Reload Table', className: 'RefreshTableValue', action: function( e, dt, node, config) {$(""#" & valueEstimateCurrentID.ClientID & """).val('');$(""#" & RefreshCurrentValueGraph.ClientID & """).click();ChangeTheMouseCursorOnItemParentDocument('cursor_wait');"
        '" $('#ValueEstimateInnerTable').empty();CreateValueEstimateDatatable();RedrawDatatablesOnSys();"
        ScriptBu += "}}"


        If (CRMViewActive) Then
            'Remove Selected Button:
            ScriptBu += ","
            ScriptBu += "{ text:'Refresh Graph', className: 'openButtonVis',"
            ScriptBu += " action: function( e, dt, node, config) {"
            ScriptBu += "$(""#" & RefreshCurrentValueGraph.ClientID & """).click();ChangeTheMouseCursorOnItemParentDocument('cursor_wait');}}"
        End If


        ScriptBu += "] "

        ScriptBu += " });}"
        ScriptBu += ";}"
        ScriptBu += "$($.fn.dataTable.tables(true)).DataTable().columns.adjust();"
        ScriptBu += "$($.fn.dataTable.tables(true)).DataTable().scroller.measure();"

        System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CreateValueEstimateDatatableFunction", ScriptBu.ToString, True)
    End Sub

    Private Sub ValueEstimateJSCheck(ByRef removeButton As Boolean, ByRef refreshTable As Boolean)
        'If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CRM Then
        Dim scriptBuOnLoad As String = ""
        Dim ScriptBuPostback As String = ""


        'First we need to check - is this table already initialized?
        'This is important because it doesn't need to be ran twice.

        scriptBuOnLoad = ScriptBuPostback & "CreateValueEstimateDatatable();"
        ScriptBuPostback += "Sys.Application.add_load(function() {CreateValueEstimateDatatable();"
        If removeButton = False Then 'do not show this
            ScriptBuPostback += "$("".RemoveRowsValue"").addClass('display_none');"
        End If
        If refreshTable = False Then 'do not show this
            ScriptBuPostback += "$("".RefreshTableValue"").addClass('display_none');"
        End If
        ScriptBuPostback += "ChangeTheMouseCursorOnItemParentDocument('cursor_default');});"


        scriptBuOnLoad = "window.onload = function() {" & scriptBuOnLoad & ";RedrawDatatablesOnSys();ChangeTheMouseCursorOnItemParentDocument('cursor_default');};"
        System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CreateValueEstimateDatatablePostback", ScriptBuPostback.ToString, True)
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "CreateValueEstimateDatatableOnLoad", scriptBuOnLoad, True)
    End Sub

End Class