
' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/DisplayEventsDetailListing.aspx.vb $
'$$Author: Amanda $
'$$Date: 4/27/20 4:29p $
'$$Modtime: 4/23/20 4:12p $
'$$Revision: 4 $
'$$Workfile: DisplayEventsDetailListing.aspx.vb $
'
' ********************************************************************************

Partial Public Class DisplayEventsDetailListing
    Inherits System.Web.UI.Page

    Private inCompanyID As Long = 0
    Private inAircraftID As Long = 0

    Private sOrderByClause As String = ""

    Private sPageTitle As String = ""
    Private nAbsPage As Integer = 1

    Dim recStart As Integer = 0
    Dim recEnd As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session.Item("crmUserLogon") <> True Then
            Response.Redirect("Default.aspx", False)
        End If

        If Not IsNothing(Request("OrderBy")) Then
            If Not String.IsNullOrEmpty(Request.Item("OrderBy").ToString.Trim) Then
                sOrderByClause = Request.Item("OrderBy").ToString.Trim
                HttpContext.Current.Session.Item("eventsAcDataTable") = Nothing
                HttpContext.Current.Session.Item("eventsCompDataTable") = Nothing
            End If
        End If

        If Not IsNothing(Request.Item("AircraftID")) Then
            If Not String.IsNullOrEmpty(Request.Item("AircraftID").ToString.Trim) Then
                If IsNumeric(Request.Item("AircraftID").ToString) Then
                    inAircraftID = CLng(Request.Item("AircraftID").ToString)
                End If
            End If
        End If

        If Not IsNothing(Request.Item("CompanyID")) Then
            If Not String.IsNullOrEmpty(Request.Item("CompanyID").ToString.Trim) Then
                If IsNumeric(Request.Item("CompanyID").ToString) Then
                    inCompanyID = CLng(Request.Item("CompanyID").ToString)
                End If
            End If
        End If

        If Not IsNothing(Request("AbsPage")) Then
            If Not String.IsNullOrEmpty(Request.Item("AbsPage").ToString.Trim) Then
                If IsNumeric(Request.Item("AbsPage").ToString) Then
                    nAbsPage = CInt(Request.Item("AbsPage").ToString)
                End If
            End If
        End If

        If Not IsNothing(Request("clearRS")) Then
            If Not String.IsNullOrEmpty(Request.Item("clearRS").ToString.Trim) Then
                If CBool(Request.Item("clearRS").ToString) Then
                    HttpContext.Current.Session.Item("eventsAcDataTable") = Nothing
                    HttpContext.Current.Session.Item("eventsCompDataTable") = Nothing
                End If
            End If
        End If

        If inAircraftID > 0 Then
            sPageTitle = "Aircraft Events Detail"
            HttpContext.Current.Session.Item("eventsAcDataTable") = Nothing
        ElseIf inCompanyID > 0 Then
            sPageTitle = "Company Events Detail"
            HttpContext.Current.Session.Item("eventsCompDataTable") = Nothing
        End If

        Master.SetPageTitle(sPageTitle + " List") 'Page title that can be set to whatever is necessary. clearRS

        detailEventsList.Text = display_event_details_list()

    End Sub

    Public Function display_event_details_list() As String

        Dim nNumPages As Integer = 1
        Dim nPageSize As Integer = HttpContext.Current.Session.Item("localUser").crmUserRecsPerPage
        Dim nItemCount As Integer = 0

        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False

        Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
        Dim SqlConn As New System.Data.SqlClient.SqlConnection
        Dim SqlCommand As New System.Data.SqlClient.SqlCommand
        Dim results_table As New DataTable
        Dim sQuery = New StringBuilder()
        Dim sTmpQuery = New StringBuilder()

        Dim eventsDetailReader As System.Data.SqlClient.SqlDataReader : eventsDetailReader = Nothing

        Dim bSerialMatches As Boolean = False
        Dim sPreviousSerial As String = ""
        Dim fAc_ser_no_full As String = ""
        Dim fAc_id As Long = 0
        Dim fAmod_make_name As String = ""
        Dim fAmod_model_name As String = ""
        Dim fAc_reg_no As String = ""
        Dim fAmod_id As Integer = 0

        Dim sTmpSubject As String = ""
        Dim sTmpDescription As String = ""
        Dim nCompanyID As Long = 0
        Dim nContactID As Long = 0
        Dim nJournalID As Long = 0
        Dim nJournSeqNo As Long = 0

        Dim tmpCompanyName As String = ""
        Dim tmpContactName As String = ""

        Dim ContactTable As New DataTable

        Try

            SqlConn.ConnectionString = Session.Item("jetnetClientDatabase").ToString.Trim

            SqlConn.Open()

            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = System.Data.CommandType.Text
            SqlCommand.CommandTimeout = 60

            htmlOut.Append("<table id=""eventsOuterTable"" width=""1000"" cellspacing=""0"" cellpadding=""0"" class=""module"">")
            htmlOut.Append("<tr><td valign=""middle"" align=""center"">")

            If IsNothing(HttpContext.Current.Session.Item("eventsAcDataTable")) Or IsNothing(HttpContext.Current.Session.Item("eventsCompDataTable")) Then

                sQuery = New StringBuilder

                sQuery.Append("SELECT DISTINCT Priority_Events.*, Aircraft.*, amod_make_name, amod_airframe_type_code, amod_type_code, amod_id, amod_model_name")
                sQuery.Append(" FROM Priority_Events WITH(NOLOCK)")
                sQuery.Append(" INNER JOIN Priority_Events_category WITH(NOLOCK) ON priorevcat_category_code = priorev_category_code")

                sQuery.Append(" INNER JOIN Aircraft WITH(NOLOCK) ON (priorev_ac_id = ac_id AND ac_journ_id = 0)")
                sQuery.Append(" INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id")

                If inCompanyID > 0 Then
                    sTmpQuery.Append(" LEFT OUTER JOIN Company WITH(NOLOCK) ON (priorev_comp_id = comp_id AND priorev_journ_id = comp_journ_id)")
                    sTmpQuery.Append(" LEFT OUTER JOIN Aircraft_Reference WITH(NOLOCK) ON (priorev_ac_id = cref_ac_id AND priorev_journ_id = cref_journ_id)")
                    sTmpQuery.Append(" LEFT OUTER JOIN Business_Type_Reference WITH(NOLOCK) ON (priorev_comp_id = bustypref_id AND priorev_journ_id = bustypref_journ_id)")
                    sTmpQuery.Append(" LEFT OUTER JOIN Aircraft_Contact_Type WITH(NOLOCK) ON (cref_contact_type = actype_code)")
                    sQuery.Append(sTmpQuery.ToString)
                End If

                sQuery.Append(" WHERE priorev_hide_flag = 'N'")
                HttpContext.Current.Session.Item("MasterAircraftEventsWhere") = "priorev_hide_flag = 'N'"

                If HttpContext.Current.Session.Item("localPreferences").AerodexFlag Then ' NOTE : need to update clause if new market status types are added
                    sQuery.Append(" AND priorev_category_code NOT IN ('CA','EXOFF','EXON','MA','OM','OMNS','SALEP','SC','SPTOIM')")
                    sQuery.Append(" AND priorevcat_category <> 'Market Status'")
                    HttpContext.Current.Session.Item("MasterAircraftEventsWhere") += " AND priorev_category_code NOT IN ('CA','EXOFF','EXON','MA','OM','OMNS','SALEP','SC','SPTOIM')"
                    HttpContext.Current.Session.Item("MasterAircraftEventsWhere") += " AND priorevcat_category <> 'Market Status'"
                End If

                If inAircraftID > 0 Then
                    sQuery.Append(Constants.cAndClause + "priorev_ac_id = " + inAircraftID.ToString)
                    HttpContext.Current.Session.Item("MasterAircraftEventsWhere") += Constants.cAndClause + "priorev_ac_id = " + inAircraftID.ToString
                End If

                If inCompanyID > 0 Then
                    sQuery.Append(Constants.cAndClause + "priorev_comp_id = " + inCompanyID.ToString)
                    HttpContext.Current.Session.Item("MasterAircraftEventsWhere") += Constants.cAndClause + "priorev_comp_id = " + inCompanyID.ToString
                End If

                sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, True))
                HttpContext.Current.Session.Item("MasterAircraftEventsWhere") += " " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, True)

                Select Case (sOrderByClause)
                    Case "Make"
                        sQuery.Append(" ORDER BY amod_airframe_type_code, amod_type_code, amod_make_name, amod_id, amod_model_name, ac_ser_no_sort")
                        HttpContext.Current.Session.Item("MasterAircraftEventsWhere") += " ORDER BY amod_airframe_type_code, amod_type_code, amod_make_name, amod_id, amod_model_name, ac_ser_no_sort"
                    Case "Model"
                        sQuery.Append(" ORDER BY amod_model_name, amod_airframe_type_code, amod_type_code, amod_id, amod_make_name, ac_ser_no_sort")
                        HttpContext.Current.Session.Item("MasterAircraftEventsWhere") += " ORDER BY amod_model_name, amod_airframe_type_code, amod_type_code, amod_id, amod_make_name, ac_ser_no_sort"
                    Case "Year"
                        sQuery.Append(" ORDER BY ac_year, amod_make_name, amod_airframe_type_code, amod_type_code, amod_id, amod_model_name, ac_ser_no_sort")
                        HttpContext.Current.Session.Item("MasterAircraftEventsWhere") += " ORDER BY ac_year, amod_make_name, amod_airframe_type_code, amod_type_code, amod_id, amod_model_name, ac_ser_no_sort"
                    Case "Serial"
                        sQuery.Append(" ORDER BY ac_ser_no_sort, amod_make_name, amod_airframe_type_code, amod_type_code, amod_id, amod_model_name")
                        HttpContext.Current.Session.Item("MasterAircraftEventsWhere") += " ORDER BY ac_ser_no_sort, amod_make_name, amod_airframe_type_code, amod_type_code, amod_id, amod_model_name"
                    Case "RegNo"
                        sQuery.Append(" ORDER BY ac_reg_no, amod_make_name, amod_airframe_type_code, amod_type_code, amod_id, amod_model_name, ac_ser_no_sort")
                        HttpContext.Current.Session.Item("MasterAircraftEventsWhere") += " ORDER BY ac_reg_no, amod_make_name, amod_airframe_type_code, amod_type_code, amod_id, amod_model_name, ac_ser_no_sort"
                    Case "EventDesc"
                        sQuery.Append(" ORDER BY priorev_subject, priorev_description, amod_make_name, amod_airframe_type_code, amod_type_code, amod_id, amod_model_name, ac_ser_no_sort")
                        HttpContext.Current.Session.Item("MasterAircraftEventsWhere") += " ORDER BY priorev_subject, priorev_description, amod_make_name, amod_airframe_type_code, amod_type_code, amod_id, amod_model_name, ac_ser_no_sort"
                    Case Else
                        sQuery.Append(" ORDER BY priorev_entry_date DESC, amod_make_name, amod_airframe_type_code, amod_type_code, amod_id, amod_model_name, ac_ser_no_sort")
                        HttpContext.Current.Session.Item("MasterAircraftEventsWhere") += " ORDER BY priorev_entry_date DESC, amod_make_name, amod_airframe_type_code, amod_type_code, amod_id, amod_model_name, ac_ser_no_sort"
                End Select

                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />display_event_details_list()</b><br />" + sQuery.ToString

                HttpContext.Current.Session.Item("MasterEvents") = sQuery.ToString

                SqlCommand.CommandText = sQuery.ToString
                eventsDetailReader = SqlCommand.ExecuteReader()

                ' clean up previous results
                results_table = New DataTable

                Try
                    results_table.Load(eventsDetailReader)
                Catch constrExc As System.Data.ConstraintException
                    Dim rowsErr As System.Data.DataRow() = results_table.GetErrors()
                End Try

                eventsDetailReader.Close()
                eventsDetailReader.Dispose()

                If inAircraftID > 0 Then
                    HttpContext.Current.Session.Item("eventsAcDataTable") = results_table
                ElseIf inCompanyID > 0 Then
                    HttpContext.Current.Session.Item("eventsCompDataTable") = results_table
                End If

            End If

            If Not IsNothing(HttpContext.Current.Session.Item("eventsAcDataTable")) Or Not IsNothing(HttpContext.Current.Session.Item("eventsCompDataTable")) Then
                ' clean up previous results
                results_table = New DataTable
                If inAircraftID > 0 Then
                    results_table = CType(HttpContext.Current.Session.Item("eventsAcDataTable"), DataTable)
                ElseIf inCompanyID > 0 Then
                    results_table = CType(HttpContext.Current.Session.Item("eventsCompDataTable"), DataTable)
                End If
            End If

            If Not IsNothing(results_table) And results_table.Rows.Count > 0 Then

                ' figure out how many pages, then display from recStart to recEnd records
                nNumPages = commonEvo.DeterminePageSize(results_table.Rows.Count, nPageSize)

                If nAbsPage < 2 Then
                    recStart = 0
                    recEnd = ((nAbsPage * nPageSize) - 1)
                    If recEnd >= results_table.Rows.Count Then
                        recEnd = (results_table.Rows.Count - 1)
                    End If
                Else
                    recStart = ((nAbsPage - 1) * nPageSize)
                    recEnd = ((nAbsPage * nPageSize) - 1)
                    If recEnd >= results_table.Rows.Count Then
                        recEnd = (results_table.Rows.Count - 1)
                    End If
                End If

                htmlOut.Append("<table id=""eventsTopButtonsTable"" cellspacing=""0"" cellpadding=""2"" border=""0"" width=""100%"">")
                htmlOut.Append("<tr>")

                If nAbsPage > 1 Then
                    htmlOut.Append("<td valign=""middle"" align=""center"" width=""25%"">")
                    htmlOut.Append("<input type=""button"" value=""< Previous Page"" onclick='javascript:document.location.href=""DisplayEventsDetailListing.aspx?AircraftID=" + inAircraftID.ToString + "&CompanyID=" + inCompanyID.ToString + IIf(Not String.IsNullOrEmpty(sOrderByClause.Trim), "&OrderBy=" + sOrderByClause, "") + "&AbsPage=" + (nAbsPage - 1).ToString + """;' title='Click to View The Previous " + nPageSize.ToString + " Results' /></td>" + vbCrLf)
                Else
                    htmlOut.Append("<td valign=""middle"" align=""center"" width=""25%"">")
                    htmlOut.Append("<input type=""button"" disabled=""disabled"" value=""< Previous Page"" /></td>" + vbCrLf)
                End If

                htmlOut.Append("<td valign='middle' align='center' nowrap='nowrap' width='25%'>")
                htmlOut.Append("<font color='#2E57B6'>&nbsp;&nbsp;<b>" + results_table.Rows.Count.ToString + "&nbsp;Events&nbsp;Found&nbsp;&nbsp;Page&nbsp;" + nAbsPage.ToString + "&nbsp;of&nbsp;" + nNumPages.ToString + "</b></font>&nbsp;&nbsp;</td>" + vbCrLf)

                If results_table.Rows.Count > (nAbsPage * nPageSize) Then
                    htmlOut.Append("<td valign=""middle"" align=""center"" width=""25%"">")
                    htmlOut.Append("<input type=""button"" value=""Next Page >"" onclick='javascript:document.location.href=""DisplayEventsDetailListing.aspx?AircraftID=" + inAircraftID.ToString + "&CompanyID=" + inCompanyID.ToString + IIf(Not String.IsNullOrEmpty(sOrderByClause.Trim), "&OrderBy=" + sOrderByClause, "") + "&AbsPage=" + (nAbsPage + 1).ToString + """;' title='Click to View The Next " + nPageSize.ToString + " Results' /></td>" + vbCrLf)
                Else
                    htmlOut.Append("<td valign=""middle"" align=""center"" width=""25%"">")
                    htmlOut.Append("<input type=""button"" disabled=""disabled"" value=""Next Page >"" /></td>" + vbCrLf)
                End If

                htmlOut.Append("<td valign=""middle"" align=""center"" width=""25%"">")

                htmlOut.Append("<input type=""button"" value=""Export/Report"" onclick=""javascript:load('PDF_Creator.aspx?export_type=events&eventDetail=Y','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');"" title=""Click to View Reporting Options"" /></td></tr>" + vbCrLf)

                htmlOut.Append("</table>" + vbCrLf)

                htmlOut.Append("<table id=""eventsDetailInnerTable"" width=""100%"" cellspacing=""0"" cellpadding=""2"" border=""0"">" + vbCrLf)

                htmlOut.Append("<tr><td valign=""middle"" align=""right"" colspan=""8"" style=""padding-right:10px; height:24px;"">Goto&nbsp;Page&nbsp;")
                htmlOut.Append("<select name=""txtGotoPage"" id=""txtGotoPageID"" onchange='javascript:setAbsPage(""DisplayEventsDetailListing.aspx?AircraftID=" + inAircraftID.ToString + "&CompanyID=" + inCompanyID.ToString + IIf(Not String.IsNullOrEmpty(sOrderByClause.Trim), "&OrderBy=" + sOrderByClause, "") + "&AbsPage="");' title='Go to page'>")

                For nLinkCount = 1 To nNumPages    'selected="selected" value="All"
                    If nLinkCount <> nAbsPage Then
                        htmlOut.Append("<option value=""" + nLinkCount.ToString + """>Page " + nLinkCount.ToString + "</option>" + vbCrLf)
                    Else
                        htmlOut.Append("<option value=""" + nLinkCount.ToString + """ selected=""selected"">Page " + nLinkCount.ToString + "</option>" + vbCrLf)
                    End If
                Next

                htmlOut.Append("</select>")
                htmlOut.Append("</td></tr>")

                htmlOut.Append("<tr><td valign=""middle"" align=""center"" class=""header"" colspan=""8"" style=""padding-left:3px;"">&nbsp;</td></tr>" + vbCrLf)
                htmlOut.Append("<tr><td valign=""middle"" align=""left"" width=""2%"">&nbsp;</td><td valign=""middle"" align=""left""><strong>")
                htmlOut.Append("<a href=""DisplayEventsDetailListing.aspx?AircraftID=" + inAircraftID.ToString + "&CompanyID=" + inCompanyID.ToString + "&OrderBy=Make"" title=""Click to Sort By Make"">MAKE</a></strong></td>")
                htmlOut.Append("<td valign=""middle"" align=""left""><strong>")
                htmlOut.Append("<a href=""DisplayEventsDetailListing.aspx?AircraftID=" + inAircraftID.ToString + "&CompanyID=" + inCompanyID.ToString + "&OrderBy=Model"" title=""Click to Sort By Model"">MODEL</a></strong></td>")
                htmlOut.Append("<td valign=""middle"" align=""left""><strong>")
                htmlOut.Append("<a href=""DisplayEventsDetailListing.aspx?AircraftID=" + inAircraftID.ToString + "&CompanyID=" + inCompanyID.ToString + "&OrderBy=Year"" title=""Click to Sort By Year"">YEAR</a></strong></td>")
                htmlOut.Append("<td valign=""middle"" align=""left""><strong>")
                htmlOut.Append("<a href=""DisplayEventsDetailListing.aspx?AircraftID=" + inAircraftID.ToString + "&CompanyID=" + inCompanyID.ToString + "&OrderBy=Serial"" title=""Click to Sort By Serial Number"">SERIAL<br />NUMBER</a></strong></td>")
                htmlOut.Append("<td valign=""middle"" align=""left""><strong>")
                htmlOut.Append("<a href=""DisplayEventsDetailListing.aspx?AircraftID=" + inAircraftID.ToString + "&CompanyID=" + inCompanyID.ToString + "&OrderBy=RegNo"" title=""Click to Sort By Registration Number"">REG<br />NUMBER</a></strong></td>")
                htmlOut.Append("<td valign=""middle"" align=""right"" style=""padding-right:10px;""><strong>")
                htmlOut.Append("<a href=""DisplayEventsDetailListing.aspx?AircraftID=" + inAircraftID.ToString + "&CompanyID=" + inCompanyID.ToString + "&OrderBy=EventDate"" title=""Click to Sort By Date/Time"">ACTIVITY<br />DATE/TIME</a></strong></td>")
                htmlOut.Append("<td valign=""middle"" align=""center""><strong>")
                htmlOut.Append("<a href=""DisplayEventsDetailListing.aspx?AircraftID=" + inAircraftID.ToString + "&CompanyID=" + inCompanyID.ToString + "&OrderBy=EventDesc"" title=""Click to Sort By Description"">DESCRIPTION</a></strong></td></tr>")

                For Each r As DataRow In results_table.Rows

                    If nItemCount >= recStart And nItemCount <= recEnd Then

                        If Not toggleRowColor Then
                            htmlOut.Append("<tr class=""alt_row"">")
                            toggleRowColor = True
                        Else
                            htmlOut.Append("<tr bgcolor=""white"">")
                            toggleRowColor = False
                        End If

                        If Not IsDBNull(r.Item("ac_ser_no_full")) Then
                            If Not String.IsNullOrEmpty(r.Item("ac_ser_no_full").ToString.Trim) Then
                                fAc_ser_no_full = r.Item("ac_ser_no_full").ToString
                            End If
                        End If

                        If Not IsDBNull(r.Item("ac_id")) Then
                            If Not String.IsNullOrEmpty(r.Item("ac_id").ToString.Trim) Then
                                fAc_id = CLng(r.Item("ac_id").ToString)
                            End If
                        End If

                        If Not IsDBNull(r.Item("ac_reg_no")) Then
                            If Not String.IsNullOrEmpty(r.Item("ac_reg_no").ToString.Trim) Then
                                fAc_reg_no = r.Item("ac_reg_no").ToString
                            End If
                        End If

                        If Not IsDBNull(r.Item("amod_make_name")) Then
                            If Not String.IsNullOrEmpty(r.Item("amod_make_name").ToString.Trim) Then
                                fAmod_make_name = r.Item("amod_make_name").ToString
                            End If
                        End If

                        If Not IsDBNull(r.Item("amod_model_name")) Then
                            If Not String.IsNullOrEmpty(r.Item("amod_model_name").ToString.Trim) Then
                                fAmod_model_name = r.Item("amod_model_name").ToString
                            End If
                        End If

                        If Not IsDBNull(r.Item("amod_id")) Then
                            If Not String.IsNullOrEmpty(r.Item("amod_id").ToString.Trim) Then
                                fAmod_id = CLng(r.Item("amod_id").ToString)
                            End If
                        End If

                        If Not IsDBNull(r.Item("priorev_comp_id")) Then
                            If Not String.IsNullOrEmpty(r.Item("priorev_comp_id").ToString.Trim) Then
                                If IsNumeric(r.Item("priorev_comp_id").ToString) Then
                                    nCompanyID = CLng(r.Item("priorev_comp_id").ToString)
                                End If
                            End If
                        End If

                        If Not IsDBNull(r.Item("priorev_contact_id")) Then
                            If Not String.IsNullOrEmpty(r.Item("priorev_contact_id").ToString.Trim) Then
                                If IsNumeric(r.Item("priorev_contact_id").ToString) Then
                                    nContactID = CLng(r.Item("priorev_contact_id").ToString)
                                End If
                            End If
                        End If

                        If Not IsDBNull(r.Item("priorev_journ_id")) Then
                            If Not String.IsNullOrEmpty(r.Item("priorev_journ_id").ToString.Trim) Then
                                If IsNumeric(r.Item("priorev_journ_id").ToString) Then
                                    nJournalID = CLng(r.Item("priorev_journ_id").ToString)
                                End If
                            End If
                        End If

                        If Not IsDBNull(r.Item("priorev_journ_seq_no")) Then
                            If Not String.IsNullOrEmpty(r.Item("priorev_journ_seq_no").ToString.Trim) Then
                                If IsNumeric(r.Item("priorev_journ_seq_no").ToString) Then
                                    nJournSeqNo = CLng(r.Item("priorev_journ_seq_no").ToString)
                                End If
                            End If
                        End If

                        If sPreviousSerial = fAc_ser_no_full Then
                            bSerialMatches = True
                        Else
                            bSerialMatches = False
                        End If

                        sPreviousSerial = fAc_ser_no_full

                        If fAc_id = 0 Then

                            htmlOut.Append("<td colspan=""6"" valign=""middle"" align=""center""><font color=""DarkRed""><b>** Not Applicable **</b></font></td>")

                        Else

                            If bSerialMatches Then
                                htmlOut.Append("<td valign=""middle"" align=""left"" width=""2%""></td>")
                            Else
                                htmlOut.Append("<td valign=""middle"" align=""left"" width=""2%""><img src=""images/ch_red.jpg"" class=""bullet"" alt=""" + nItemCount.ToString + """ title=""" + nItemCount.ToString + """ /></td>")
                            End If

                            If bSerialMatches Then
                                htmlOut.Append("<td valign=""middle"" align=""left""></td>")
                            Else
                                htmlOut.Append("<td valign=""middle"" align=""left"">" + fAmod_make_name.Trim + "</td>")
                            End If

                            If bSerialMatches Then
                                htmlOut.Append("<td valign=""middle"" align=""left""></td>")
                            Else
                                htmlOut.Append("<td valign=""middle"" align=""left"">" & DisplayFunctions.WriteModelDetailsLink(fAmod_id, fAmod_model_name.Trim, True) & "</td>")
                            End If

                            If bSerialMatches Then
                                htmlOut.Append("<td valign=""middle"" align=""left""></td>")
                            Else
                                htmlOut.Append("<td valign=""middle"" align=""left"">")

                                If Not IsDBNull(r.Item("ac_year")) Then
                                    If Not String.IsNullOrEmpty(r.Item("ac_year").ToString.Trim) Then
                                        htmlOut.Append(r.Item("ac_year").ToString)
                                    End If
                                End If

                                htmlOut.Append("</td>")
                            End If

                            If bSerialMatches Then
                                htmlOut.Append("<td valign=""middle"" align=""left""></td>")
                            Else
                                htmlOut.Append("<td valign=""middle"" align=""left"">" & DisplayFunctions.WriteDetailsLink(fAc_id, 0, 0, 0, True, fAc_ser_no_full.Trim, "underline", "") & "</td>")
                            End If

                            If bSerialMatches Then
                                htmlOut.Append("<td valign=""middle"" align=""left""></td>")
                            Else
                                htmlOut.Append("<td valign=""middle"" align=""left"">" + fAc_reg_no.Trim + "</td>")
                            End If

                        End If

                        htmlOut.Append("<td align=""right"" valign=""middle"" nowrap=""nowrap"" style=""padding-right:10px;"">")
                        If Not IsDBNull(r.Item("priorev_entry_date")) Then
                            If Not String.IsNullOrEmpty(r.Item("priorev_entry_date").ToString.Trim) Then
                                htmlOut.Append(FormatDateTime(r.Item("priorev_entry_date").ToString, DateFormat.GeneralDate))
                            End If
                        End If
                        htmlOut.Append("</td>")

                        htmlOut.Append("<td valign=""middle"" align=""left"">")

                        If Not IsDBNull(r.Item("priorev_subject")) Then
                            If Not String.IsNullOrEmpty(r.Item("priorev_subject").ToString.Trim) Then
                                sTmpSubject = r.Item("priorev_subject").ToString.Trim
                            End If
                        End If

                        If nJournSeqNo > 0 Then
                            htmlOut.Append("<a class=""underline"" onclick='javascript:SubmitTransactionDocumentForm(""" + fAmod_make_name.Trim + """,""" + fAmod_model_name.Trim + """,""" + fAc_ser_no_full.Trim + """," + fAc_id.ToString + "," + nJournalID.ToString + "," + nJournSeqNo.ToString + ");' title=""Display Document Details"">" + sTmpSubject + "</a>")
                        Else
                            htmlOut.Append(sTmpSubject)
                        End If

                        sTmpSubject = ""
                        sTmpDescription = ""

                        If Not IsDBNull(r.Item("priorev_description")) Then
                            If Not String.IsNullOrEmpty(r.Item("priorev_description").ToString.Trim) Then
                                sTmpDescription = r.Item("priorev_description").ToString.Trim
                            End If
                        End If

                        If Not String.IsNullOrEmpty(sTmpDescription.Trim) Then

                            If nCompanyID > 0 Then
                                tmpCompanyName = commonEvo.get_company_name_fromID(nCompanyID, 0, False, False, "")

                                If Not String.IsNullOrEmpty(tmpCompanyName) Then

                                    If sTmpDescription.Contains(tmpCompanyName) Then
                                        htmlOut.Append(" [ " + sTmpDescription.Replace(tmpCompanyName, DisplayFunctions.WriteDetailsLink(0, nCompanyID, 0, nJournalID, True, tmpCompanyName, "underline", "")) + " ]")
                                    Else
                                        htmlOut.Append(" [ " + sTmpDescription.Trim + " " + DisplayFunctions.WriteDetailsLink(0, nCompanyID, 0, nJournalID, True, tmpCompanyName, "underline", "") + " ]")
                                    End If

                                Else
                                    htmlOut.Append(" [ " + sTmpDescription.Trim + " ]")
                                End If

                            ElseIf nContactID > 0 Then

                                ContactTable = commonEvo.get_contact_info_fromID_returnDatatable(0, nContactID, 0, True)
                                If Not IsNothing(ContactTable) And ContactTable.Rows.Count > 0 Then
                                    tmpContactName = IIf(Not IsDBNull(ContactTable.Rows(0).Item("contact_sirname")), IIf(Not String.IsNullOrEmpty(ContactTable.Rows(0).Item("contact_sirname").ToString.Trim), ContactTable.Rows(0).Item("contact_sirname").ToString.Trim + " ", ""), "")
                                    tmpContactName += IIf(Not IsDBNull(ContactTable.Rows(0).Item("contact_first_name")), ContactTable.Rows(0).Item("contact_first_name").ToString.Trim + " ", "")
                                    tmpContactName += IIf(Not IsDBNull(ContactTable.Rows(0).Item("contact_middle_initial")), IIf(Not String.IsNullOrEmpty(ContactTable.Rows(0).Item("contact_middle_initial").ToString.Trim), ContactTable.Rows(0).Item("contact_middle_initial").ToString.Trim + ".&nbsp;", ""), "")
                                    tmpContactName += IIf(Not IsDBNull(ContactTable.Rows(0).Item("contact_last_name")), ContactTable.Rows(0).Item("contact_last_name").ToString, "")
                                End If
                                ContactTable = Nothing

                                If Not String.IsNullOrEmpty(tmpContactName) Then
                                    If sTmpDescription.Contains(tmpContactName) Then
                                        htmlOut.Append(" [ " + sTmpDescription.Replace(tmpContactName, DisplayFunctions.WriteDetailsLink(0, nCompanyID, nContactID, nJournalID, True, tmpContactName.Trim, "underline", "")) + " ]")
                                    Else
                                        htmlOut.Append(" [ " + sTmpDescription.Trim + " " + DisplayFunctions.WriteDetailsLink(0, nCompanyID, nContactID, nJournalID, True, tmpContactName.Trim, "underline", "") + " ]")
                                    End If
                                Else
                                    htmlOut.Append(" [ " + sTmpDescription.Trim + " ]")
                                End If

                            Else
                                htmlOut.Append(" [ " + sTmpDescription.Trim + " ]")
                            End If

                            tmpCompanyName = ""
                            tmpContactName = ""

                        End If

                        htmlOut.Append("</td></tr>")

                    End If

                    nItemCount += 1

                Next ' icount

                htmlOut.Append("</table>")

                htmlOut.Append("<table id=""eventsBottomButtonsTable"" cellspacing=""0"" cellpadding=""2"" border=""0"" width=""100%"">")
                htmlOut.Append("<tr>")

                If nAbsPage > 1 Then
                    htmlOut.Append("<td valign=""middle"" align=""center"" width=""25%"">")
                    htmlOut.Append("<input type='button' value='< Previous Page' onclick='javascript:document.location.href=""DisplayEventsDetailListing.aspx?AircraftID=" + inAircraftID.ToString + "&CompanyID=" + IIf(Not String.IsNullOrEmpty(sOrderByClause.Trim), "&OrderBy=" + sOrderByClause, "") + inCompanyID.ToString + "&AbsPage=" + (nAbsPage - 1).ToString + """;' title='Click to View The Previous " + nPageSize.ToString + " Results' /></td>" + vbCrLf)
                Else
                    htmlOut.Append("<td valign=""middle"" align=""center"" width=""25%"">")
                    htmlOut.Append("<input type=""button"" disabled=""disabled"" value=""< Previous Page"" /></td>" + vbCrLf)
                End If

                htmlOut.Append("<td valign=""middle"" align=""center"" nowrap=""nowrap"" width=""25%"">")
                htmlOut.Append("<font color=""#2E57B6"">&nbsp;&nbsp;<b>" + results_table.Rows.Count.ToString + "&nbsp;Events&nbsp;Found&nbsp;&nbsp;Page&nbsp;" + nAbsPage.ToString + "&nbsp;of&nbsp;" + nNumPages.ToString + "</b></font>&nbsp;&nbsp;</td>" + vbCrLf)

                If results_table.Rows.Count > (nAbsPage * nPageSize) Then
                    htmlOut.Append("<td valign=""middle"" align=""center"" width=""25%"">")
                    htmlOut.Append("<input type='button' value='Next Page >' onclick='javascript:document.location.href=""DisplayEventsDetailListing.aspx?AircraftID=" + inAircraftID.ToString + "&CompanyID=" + IIf(Not String.IsNullOrEmpty(sOrderByClause.Trim), "&OrderBy=" + sOrderByClause, "") + inCompanyID.ToString + "&AbsPage=" + (nAbsPage + 1).ToString + """;' title='Click to View The Next " + nPageSize.ToString + " Results' /></td>" + vbCrLf)
                Else
                    htmlOut.Append("<td valign=""middle"" align=""center"" width=""25%"">")
                    htmlOut.Append("<input type=""button"" disabled=""disabled"" value=""Next Page >"" /></td>" + vbCrLf)
                End If

                htmlOut.Append("<td valign=""middle"" align=""center"" width=""25%"">")
                htmlOut.Append("<input type=""button"" value=""Export/Report"" onclick="""" title=""Click to View Reporting Options"" /></td></tr>" + vbCrLf)
                htmlOut.Append("</table>" + vbCrLf)

            Else

                htmlOut.Append("<table id=""eventsDetailOuterTable"" width=""100%"" cellspacing=""0"" cellpadding=""2"" border=""0"">")
                htmlOut.Append("<tr><td valign=""middle"" align=""center"" class=""header"" colspan=""7"" style=""padding-left:3px;""></td></tr>")
                htmlOut.Append("<td valign=""top"" align=""left"" class=""seperator""><strong>MAKE</strong></td>")
                htmlOut.Append("<td valign=""top"" align=""left"" class=""seperator""><strong>MODEL</strong></td>")
                htmlOut.Append("<td valign=""top"" align=""left"" class=""seperator""><strong>YEAR</strong></td>")
                htmlOut.Append("<td valign=""top"" align=""left"" class=""seperator""><strong>SERIAL<br />NUMBER</strong></td>")
                htmlOut.Append("<td valign=""top"" align=""left"" class=""seperator""><strong>REG<br />NUMBER</strong></td>")
                htmlOut.Append("<td valign=""top"" align=""left"" class=""seperator""><strong>ACTIVITY<br />DATE/TIME</strong></td>")
                htmlOut.Append("<td valign=""top"" align=""left"" class=""seperator""><strong>DESCRIPTION</strong></td></tr>")
                htmlOut.Append("<tr><td colspan=""7"" valign=""middle"" align=""center"">No Event Details Found</td></tr>")
                htmlOut.Append("</table>")

            End If

            htmlOut.Append("</td></tr></table>") ' end outer table

        Catch ex As Exception

            results_table = Nothing

        Finally

        End Try

        Return htmlOut.ToString

    End Function

End Class