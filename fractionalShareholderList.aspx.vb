
' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/fractionalShareholderList.aspx.vb $
'$$Author: Matt $
'$$Date: 11/21/19 10:56a $
'$$Modtime: 11/21/19 10:08a $
'$$Revision: 3 $
'$$Workfile: fractionalShareholderList.aspx.vb $
'
' ********************************************************************************

Partial Public Class fractionalShareholderList
  Inherits System.Web.UI.Page

  Private inAmodID As Long = 0
  Private inProgramID As Long = 0
  Private nExpireYear As Integer = 0
  Private sExpireFlag As String = ""
  Private sExpireTitle As String = ""
  Private sPageTitle As String = ""
  Private nAbsPage As Integer = 1
  Dim recStart As Integer = 0
  Dim recEnd As Integer = 0

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    If Session.Item("crmUserLogon") <> True Then
      Response.Redirect("Default.aspx", False)
    End If

    If Not IsNothing(Request.Item("AmodID")) Then
      If Not String.IsNullOrEmpty(Request.Item("AmodID").ToString.Trim) Then
        If IsNumeric(Request.Item("AmodID").ToString) Then
          inAmodID = CLng(Request.Item("AmodID").ToString)
        End If
      End If
    End If

    If Not IsNothing(Request.Item("ProgramID")) Then
      If Not String.IsNullOrEmpty(Request.Item("ProgramID").ToString.Trim) Then
        If IsNumeric(Request.Item("ProgramID").ToString) Then
          inProgramID = CLng(Request.Item("ProgramID").ToString)
        End If
      End If
    End If

    If Not IsNothing(Request.Item("expireYear")) Then
      If Not String.IsNullOrEmpty(Request.Item("expireFlag").ToString.Trim) Then
        If IsNumeric(Request.Item("expireYear").ToString) Then
          nExpireYear = CLng(Request.Item("expireYear").ToString)
        End If
      End If
    End If

    If Not IsNothing(Request("expireFlag")) Then
      If Not String.IsNullOrEmpty(Request.Item("expireFlag").ToString.Trim) Then
        sExpireFlag = Request.Item("expireFlag").ToString
      End If
    End If

    If Not IsNothing(Request("AbsPage")) Then
      If Not String.IsNullOrEmpty(Request.Item("AbsPage").ToString.Trim) Then
        nAbsPage = CInt(Request.Item("AbsPage").ToString)
      End If
    End If

    If Not IsNothing(Request("clearRS")) Then
      If Not String.IsNullOrEmpty(Request.Item("clearRS").ToString.Trim) Then
        If CBool(Request.Item("clearRS").ToString) Then
          HttpContext.Current.Session.Item("fractionalDataTable") = Nothing
        End If
      End If
    End If

    If nExpireYear > 0 And Not String.IsNullOrEmpty(sExpireFlag.Trim) Then
      If sExpireFlag.ToUpper.Contains("Y") Then
        sExpireTitle = " Expiring Shareholders"
      Else
        sExpireTitle = " Expired Shareholders"
      End If

      sPageTitle = nExpireYear.ToString + sExpireTitle
    Else
      sPageTitle = "Fractional Shareholder"
    End If

    Master.SetPageTitle(sPageTitle + " List") 'Page title that can be set to whatever is necessary. clearRS

    frac_label.Text = display_fractional_shareholders_list()

  End Sub

  Public Function display_fractional_shareholders_list() As String

    Dim bHadCity As Boolean = False
    Dim nFractionalCompID As Long = 0
    Dim sFractionalProgramName As String = ""

    Dim bUseExpire As Boolean = False
    Dim nNumPages As Integer = 1

    Dim holdCompID As Long = 0

    Dim nPageSize As Integer = HttpContext.Current.Session.Item("localUser").crmUserRecsPerPage

    If nExpireYear > 0 And Not String.IsNullOrEmpty(sExpireFlag.Trim) Then
      bUseExpire = True
    End If

    Dim nItemCount As Integer = 0

    Dim htmlOut As New StringBuilder
    Dim toggleRowColor As Boolean = False

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim results_table As New DataTable
    Dim sQuery = New StringBuilder()

    Dim fractionalReader As System.Data.SqlClient.SqlDataReader : fractionalReader = Nothing

    Try

      SqlConn.ConnectionString = Session.Item("jetnetClientDatabase").ToString.Trim

      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60

      htmlOut.Append("<table id='shareHolderOuterTable' width='750' cellspacing='0' cellpadding='2' class='module'>")
      htmlOut.Append("<tr><td valign='middle' align='center'>")


      If inProgramID > 0 Then

        sQuery.Append("SELECT DISTINCT prog_comp_id, prog_name")
        sQuery.Append(" FROM Aircraft_Programs WITH(NOLOCK) INNER JOIN program_reference WITH(NOLOCK) ON pgref_prog_id = prog_id")
        sQuery.Append(" WHERE prog_active_flag = 'Y'")
        sQuery.Append(" AND prog_id = " + inProgramID.ToString)

        SqlCommand.CommandText = sQuery.ToString
        fractionalReader = SqlCommand.ExecuteReader()

        Try
          results_table.Load(fractionalReader)
        Catch constrExc As System.Data.ConstraintException
          Dim rowsErr As System.Data.DataRow() = results_table.GetErrors()
        End Try

        fractionalReader.Close()
        fractionalReader.Dispose()

        If results_table.Rows.Count > 0 Then

          For Each Row As DataRow In results_table.Rows

            nFractionalCompID = CLng(Row.Item("prog_comp_id").ToString)
            sFractionalProgramName = Row.Item("prog_name").ToString.Trim

          Next

          sQuery = New StringBuilder

          sQuery.Append("SELECT DISTINCT comp_id, comp_journ_id, comp_name, comp_address1, comp_address2, comp_city,")
          sQuery.Append(" comp_state, comp_zip_code, comp_country, comp_email_address, comp_web_address, comp_fractowr_notes")
          sQuery.Append(" FROM Company WITH(NOLOCK) WHERE (comp_journ_id = 0 and comp_id = " + nFractionalCompID.ToString)
          sQuery.Append(" AND comp_active_flag = 'Y' AND comp_hide_flag = 'N')")
          sQuery.Append(" " + commonEvo.MakeCompanyProductCodeClause(HttpContext.Current.Session.Item("localPreferences"), False))

          SqlCommand.CommandText = sQuery.ToString
          fractionalReader = SqlCommand.ExecuteReader()

          ' clean up previous results
          results_table = New DataTable

          Try
            results_table.Load(fractionalReader)
          Catch constrExc As System.Data.ConstraintException
            Dim rowsErr As System.Data.DataRow() = results_table.GetErrors()
          End Try

          fractionalReader.Close()
          fractionalReader.Dispose()

          If results_table.Rows.Count > 0 Then

            For Each Row As DataRow In results_table.Rows

              htmlOut.Append("<table id='fractionalShareHoldersCompanyTable' width='60%' cellspacing='0' cellpadding='2' class='module'>")
              htmlOut.Append("<tr><td valign='middle' align='center'>FRACTIONAL PROGRAM COMPANY : <b>" + Row.Item("comp_name").ToString.Trim + "</b></td></tr>")
              htmlOut.Append("<tr><td valign='middle' align='center'><br />" + sFractionalProgramName + "</td></tr>")

              If Not (IsDBNull(Row.Item("comp_address1"))) Then
                htmlOut.Append("<tr><td valign='middle' align='center'>" + Row.Item("comp_address1").ToString.Trim + "</td></tr>")
              End If

              If Not (IsDBNull(Row.Item("comp_address2"))) Then
                htmlOut.Append("<tr><td valign='middle' align='center'>" + Row.Item("comp_address2").ToString.Trim + "</td></tr>")
              End If

              htmlOut.Append("<tr><td valign='middle' align='center'>")

              If Not (IsDBNull(Row.Item("comp_city"))) Then
                htmlOut.Append(Row.Item("comp_city").ToString.Trim)
                bHadCity = True
              End If

              If Not (IsDBNull(Row.Item("comp_state"))) Then

                If bHadCity Then htmlOut.Append(crmWebClient.Constants.cMultiDelim)

                htmlOut.Append(Row.Item("comp_state").trim)

              End If

              If Not (IsDBNull(Row.Item("comp_zip_code"))) Then
                htmlOut.Append("&nbsp;" + Row.Item("comp_zip_code").ToString.Trim)
              End If

              If Not (IsDBNull(Row.Item("comp_country"))) Then
                htmlOut.Append("<br />" + Row.Item("comp_country").ToString.Trim)
              End If

              htmlOut.Append("</td></tr>")

              If Not (IsDBNull(Row.Item("comp_email_address"))) Then
                If Not String.IsNullOrEmpty(Row.Item("comp_email_address").ToString.Trim) Then
                  htmlOut.Append("<tr><td valign='middle' align='center'><a href='mailto:" + Row.Item("comp_email_address").ToString + "'>" + Row.Item("comp_email_address").ToString + "</a></td></tr>")
                End If
              End If

              If Not (IsDBNull(Row.Item("comp_web_address"))) Then
                If Not String.IsNullOrEmpty(Row.Item("comp_web_address").ToString.Trim) Then
                  htmlOut.Append("<tr><td valign='middle' align='center'><a href='http://" + Row.Item("comp_web_address").ToString + "' target='_blank'>" + Row.Item("comp_web_address").ToString + "</a></td></tr>")
                End If
              End If

              htmlOut.Append("<tr><td valign='middle' align='center'>")
              htmlOut.Append(commonEvo.get_company_phone(CLng(Row.Item("comp_id").ToString), False))
              htmlOut.Append("</td></tr>")

              If Not IsDBNull(Row.Item("comp_fractowr_notes")) Then
                If Not String.IsNullOrEmpty(Row.Item("comp_fractowr_notes").ToString.Trim) Then
                  htmlOut.Append("<tr><td valign='top' align='center'><b>FRACTIONAL&nbsp;OWNER&nbsp;NOTES:</b>&nbsp;")
                  htmlOut.Append(Row.Item("comp_fractowr_notes").ToString.Trim)
                  htmlOut.Append("</td></tr>")
                End If
              End If

              htmlOut.Append("<tr><td valign='middle' align='center'><b>Business&nbsp;Type(s):</b>&nbsp;" + commonEvo.GetBusinessTypes(CLng(Row.Item("comp_id").ToString), CLng(Row.Item("comp_journ_id").ToString)) + "</td></tr>")
              htmlOut.Append("</table>")

            Next

          Else

            htmlOut.Append("<table id='fractionalShareHoldersCompanyTable' width='40%' ccellspacing='0' cellpadding='2' class='module'>")
            htmlOut.Append("<tr><td valign='middle' align='center' class='header' style='padding-left:3px;'>FRACTIONAL PROGRAM COMPANY</td></tr>")
            htmlOut.Append("<tr><td valign='middle' align='center'> No Company Record Found! </td></tr>")
            htmlOut.Append("</table>")

          End If

        Else
          htmlOut.Append("<table id='fractionalShareHoldersCompanyTable' width='40%' cellspacing='0' cellpadding='2' class='module'>")
          htmlOut.Append("<tr><td valign='middle' align='center' class='header' style='padding-left:3px;'>FRACTIONAL PROGRAM COMPANY</td></tr>")
          htmlOut.Append("<tr><td valign='middle' align='center'> No Fractional Program Found! </td></tr>")
          htmlOut.Append("</table>")
        End If

      Else
        htmlOut.Append("<table id='fractionalShareHoldersCompanyTable' width='40%' cellspacing='0' cellpadding='2' class='module'>")
        htmlOut.Append("<tr><td valign='middle' align='center' class='header' style='padding-left:3px;'>FRACTIONAL PROGRAM MODEL</td></tr>")
        htmlOut.Append("<tr><td valign='middle' align='center'><strong>" + commonEvo.Get_Aircraft_Model_Info(inAmodID, False, "") + "</strong></td></tr>")
        htmlOut.Append("</table>")
      End If

      If IsNothing(HttpContext.Current.Session.Item("fractionalDataTable")) Then

        sQuery = New StringBuilder

        If nExpireYear > 0 Then
          sQuery.Append("SELECT comp_id, comp_name, comp_name_alt_type, comp_name_alt, comp_city, comp_state, comp_zip_code,")
          sQuery.Append(" comp_email_address, comp_web_address, comp_address1, comp_address2, comp_country, comp_fractowr_notes, state_name,")
          sQuery.Append(" cref_fraction_expires_date, cref_owner_percent")
        Else
          sQuery.Append("SELECT comp_id, comp_name, comp_name_alt_type, comp_name_alt, comp_city, comp_state, comp_zip_code,")
          sQuery.Append(" comp_email_address, comp_web_address, comp_address1, comp_address2, comp_country, comp_fractowr_notes, state_name,")
          sQuery.Append(" sum(cref_owner_percent) as sumPercent")
        End If

        sQuery.Append(" FROM company WITH(NOLOCK) LEFT OUTER JOIN State WITH(NOLOCK) ON comp_state = state_code AND comp_country = state_country")
        sQuery.Append(" INNER JOIN aircraft_reference WITH(NOLOCK) ON comp_id = cref_comp_id AND comp_journ_id = cref_journ_id AND comp_journ_id = 0")
        sQuery.Append(" WHERE cref_contact_type = '97'")

        If nExpireYear > 0 Then

          sQuery.Append(" AND YEAR(cref_fraction_expires_date) = '" + nExpireYear.ToString + "'")

          If Now.Year = nExpireYear Then
            If sExpireFlag.ToUpper.Contains("Y") Then
              sQuery.Append(" AND cref_fraction_expires_date >= GETDATE()")
            Else
              sQuery.Append(" AND cref_fraction_expires_date <= GETDATE()")
            End If
          End If

        End If

        sQuery.Append(" AND comp_active_flag = 'Y' AND comp_hide_flag = 'N'")

        sQuery.Append(" AND (cref_ac_id IN (SELECT cref_ac_id FROM aircraft_reference WITH(NOLOCK) INNER JOIN program_reference WITH(NOLOCK) ON pgref_comp_id = cref_comp_id AND cref_journ_id = 0")
        sQuery.Append(" INNER JOIN aircraft WITH(NOLOCK) ON cref_ac_id = ac_id AND cref_journ_id = ac_journ_id")
        sQuery.Append(" INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id")
        sQuery.Append(" WHERE cref_contact_type = '17'")

        If inProgramID > 0 Then
          sQuery.Append(" AND pgref_prog_id = " + inProgramID.ToString)
        End If

        If inAmodID > 0 Then
          sQuery.Append(" AND ac_amod_id = " + inAmodID.ToString)
        End If

        sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))

        sQuery.Append("))")

        If nExpireYear > 0 Then
          sQuery.Append(" GROUP BY cref_fraction_expires_date, cref_owner_percent, comp_name, comp_city, comp_state, state_name, comp_zip_code, comp_country, comp_id, comp_name_alt_type, comp_name_alt, comp_email_address, comp_web_address, comp_address1, comp_address2, comp_fractowr_notes")
          sQuery.Append(" ORDER BY cref_fraction_expires_date asc, cref_owner_percent, comp_name, comp_city, comp_state, comp_zip_code, comp_country, comp_id")
        Else
          sQuery.Append(" GROUP BY comp_name, comp_city, comp_state, state_name, comp_zip_code, comp_country, comp_id, comp_name_alt_type, comp_name_alt, comp_email_address, comp_web_address, comp_address1, comp_address2, comp_fractowr_notes")
          sQuery.Append(" ORDER BY comp_name, comp_city, comp_state, comp_zip_code, comp_country, comp_id")
        End If

        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />display_fractional_shareholders_list()</b><br />" + sQuery.ToString

        HttpContext.Current.Session.Item("MasterShareholder") = sQuery.ToString

        SqlCommand.CommandText = sQuery.ToString
        fractionalReader = SqlCommand.ExecuteReader()

        ' clean up previous results
        results_table = New DataTable

        Try
          results_table.Load(fractionalReader)
        Catch constrExc As System.Data.ConstraintException
          Dim rowsErr As System.Data.DataRow() = results_table.GetErrors()
        End Try

        fractionalReader.Close()
        fractionalReader.Dispose()

        HttpContext.Current.Session.Item("fractionalDataTable") = results_table

      End If

      If Not IsNothing(HttpContext.Current.Session.Item("fractionalDataTable")) Then
        ' clean up previous results
        results_table = New DataTable
        results_table = CType(HttpContext.Current.Session.Item("fractionalDataTable"), DataTable)
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

        htmlOut.Append("<br /><table cellspacing='0' cellpadding='2' border='0' width='100%'>")
        htmlOut.Append("<tr>")

        If Not bUseExpire Then
          If nAbsPage > 1 Then
            htmlOut.Append("<td valign='middle' align='center' width='25%'>")
            htmlOut.Append("<input type=""button"" value=""< Previous Page"" onclick='javascript:document.location.href=""fractionalShareholderList.aspx?AmodID=" + inAmodID.ToString + "&ProgramID=" + inProgramID.ToString + "&AbsPage=" + (nAbsPage - 1).ToString + """;' title='Click to View The Previous " + nPageSize.ToString + " Results' /></td>" + vbCrLf)
          Else
            htmlOut.Append("<td valign='middle' align='center' width='25%'>")
            htmlOut.Append("<input type=""button"" disabled=""disabled"" value=""< Previous Page"" /></td>" + vbCrLf)
          End If
        Else
          If nAbsPage > 1 Then
            htmlOut.Append("<td valign='middle' align='center' width='25%'>")
            htmlOut.Append("<input type=""button"" value=""< Previous Page"" onclick='javascript:document.location.href=""fractionalShareholderList.aspx?AmodID=" + inAmodID.ToString + "&ProgramID=" + inProgramID.ToString + "&expireYear=" + nExpireYear.ToString + "&expireFlag=" + sExpireFlag + "&AbsPage=" + (nAbsPage - 1).ToString + """;' title='Click to View The Previous " + nPageSize.ToString + " Results' /></td>" + vbCrLf)
          Else
            htmlOut.Append("<td valign='middle' align='center' width='25%'>")
            htmlOut.Append("<input type=""button"" disabled=""disabled"" value=""< Previous Page"" /></td>" + vbCrLf)
          End If
        End If

        htmlOut.Append("<td valign='middle' align='center' nowrap='nowrap' width='25%'>")
        htmlOut.Append("<font color='#2E57B6'>&nbsp;&nbsp;<b>" + results_table.Rows.Count.ToString + "&nbsp;Companies&nbsp;Found&nbsp;&nbsp;Page&nbsp;" + nAbsPage.ToString + "&nbsp;of&nbsp;" + nNumPages.ToString + "</b></font>&nbsp;&nbsp;</td>" + vbCrLf)

        If Not bUseExpire Then
          If results_table.Rows.Count > (nAbsPage * nPageSize) Then
            htmlOut.Append("<td valign='middle' align='center' width='25%'>")
            htmlOut.Append("<input type=""button"" value=""Next Page >"" onclick='javascript:document.location.href=""fractionalShareholderList.aspx?AmodID=" + inAmodID.ToString + "&ProgramID=" + inProgramID.ToString + "&AbsPage=" + (nAbsPage + 1).ToString + """;' title='Click to View The Next " + nPageSize.ToString + " Results' /></td>" + vbCrLf)
          Else
            htmlOut.Append("<td valign='middle' align='center' width='25%'>")
            htmlOut.Append("<input type=""button"" disabled=""disabled"" value=""Next Page >"" /></td>" + vbCrLf)
          End If
        Else
          If results_table.Rows.Count > (nAbsPage * nPageSize) Then
            htmlOut.Append("<td valign='middle' align='center' width='25%'>")
            htmlOut.Append("<input type=""button"" value=""Next Page >"" onclick='javascript:document.location.href=""fractionalShareholderList.aspx?AmodID=" + inAmodID.ToString + "&ProgramID=" + inProgramID.ToString + "&expireYear=" + nExpireYear.ToString + "&expireFlag=" + sExpireFlag + "&AbsPage=" + (nAbsPage + 1).ToString + """;' title='Click to View The Next " + nPageSize.ToString + " Results' /></td>" + vbCrLf)
          Else
            htmlOut.Append("<td valign='middle' align='center' width='25%'>")
            htmlOut.Append("<input type=""button"" disabled=""disabled"" value=""Next Page >"" /></td>" + vbCrLf)
          End If
        End If

        htmlOut.Append("<td valign='middle' align='center' width='25%'>")

                'htmlOut.Append("<input type='button' value='Export/Report' onclick='javascript:load(""PDF_Creator.aspx?export_type=shareholder&frAmodID=" + inAmodID.ToString + "&frProgramID=" + inProgramID.ToString + "&expireYear=" + nExpireYear.ToString + "&expireFlag=" + sExpireFlag + ""","""",""ReportOptions"");' title='Click to View Reporting Options' /></td></tr>" + vbCrLf)

                htmlOut.Append("<a href=""PDF_Creator.aspx?export_type=shareholder&frAmodID=" + inAmodID.ToString + "&frProgramID=" + inProgramID.ToString + "&expireYear=" + nExpireYear.ToString + "&expireFlag=" + sExpireFlag + """ title='Click to View Reporting Options' target='_blank'/>Export/Report</a></td></tr>" + vbCrLf)


                htmlOut.Append("</table>" + vbCrLf)

        htmlOut.Append("<table id='fractionalShareHoldersInnerTable' width='100%' cellspacing='0' cellpadding='2' border='0'>" + vbCrLf)

        If nExpireYear > 0 Then
          htmlOut.Append("<tr><td valign='middle' align='right' colspan='4' style='padding-right:10px; height:24px;'>Goto&nbsp;Page&nbsp;")
          htmlOut.Append("<select name='txtGotoPage' id='txtGotoPageID' onchange='javascript:setAbsPage(""fractionalShareholderList.aspx?AmodID=" + inAmodID.ToString + "&ProgramID=" + inProgramID.ToString + "&expireYear=" + nExpireYear.ToString + "&expireFlag=" + sExpireFlag + "&AbsPage="");' title='Go to page'>")
        Else
          htmlOut.Append("<tr><td valign='middle' align='right' colspan='3' style='padding-right:10px; height:24px;'>Goto&nbsp;Page&nbsp;")
          htmlOut.Append("<select name='txtGotoPage' id='txtGotoPageID' onchange='javascript:setAbsPage(""fractionalShareholderList.aspx?AmodID=" + inAmodID.ToString + "&ProgramID=" + inProgramID.ToString + "&AbsPage="");' title='Go to page'>")
        End If

        For nLinkCount = 1 To nNumPages    'selected="selected" value="All"
          If nLinkCount <> nAbsPage Then
            htmlOut.Append("<option value=""" + nLinkCount.ToString + """>Page " + nLinkCount.ToString + "</option>" + vbCrLf)
          Else
            htmlOut.Append("<option value=""" + nLinkCount.ToString + """ selected=""selected"">Page " + nLinkCount.ToString + "</option>" + vbCrLf)
          End If
        Next

        htmlOut.Append("</select>")
        htmlOut.Append("</td></tr>")

        If nExpireYear > 0 Then
          htmlOut.Append("<tr><td colspan='4' valign='top'>&nbsp;</td></tr>" + vbCrLf)
          htmlOut.Append("<tr><td valign='middle' align='left' class='seperator' width='2%'>&nbsp;</td>")
          htmlOut.Append("<td valign='top' align='left' class='seperator' width='75%'>&nbsp;&nbsp;<strong>Company&nbsp;Information</strong></td>" + vbCrLf)
          htmlOut.Append("<td valign='top' align='center' class='seperator'>&nbsp;&nbsp;<strong>Fractional&nbsp;Share</strong></td>" + vbCrLf)

          If sExpireFlag.ToUpper.Contains("Y") Then
            htmlOut.Append("<td valign='top' align='center' class='seperator'>&nbsp;&nbsp;<strong>Expire&nbsp;Date</strong></td></tr>" + vbCrLf)
          Else
            htmlOut.Append("<td valign='top' align='center' class='seperator'>&nbsp;&nbsp;<strong>Expired&nbsp;Date</strong></td></tr>" + vbCrLf)
          End If
          htmlOut.Append("<tr><td colspan='4' valign='top'>")

        Else

          htmlOut.Append("<tr><td colspan='3' valign='top'>&nbsp;</td></tr>" + vbCrLf)
          htmlOut.Append("<tr><td valign='middle' align='left' class='seperator' width='2%'>&nbsp;</td>")
          htmlOut.Append("<td valign='top' align='left' class='seperator' width='80%'>&nbsp;&nbsp;<strong>Company&nbsp;Information</strong></td>" + vbCrLf)
          htmlOut.Append("<td valign='top' align='center' class='seperator'>&nbsp;&nbsp;<strong>%&nbsp;of&nbsp;Shares</strong></td></tr>" + vbCrLf)
          htmlOut.Append("<tr><td colspan='3' valign='top'>")

        End If

        htmlOut.Append("<table id='fractionalShareHoldersDataTable' width='100%' cellspacing='0' cellpadding='2' class='module'>" + vbCrLf)

        For Each r As DataRow In results_table.Rows

          If nItemCount >= recStart And nItemCount <= recEnd Then

            If Not toggleRowColor Then
              htmlOut.Append("<tr class='alt_row'>")
              toggleRowColor = True
            Else
              htmlOut.Append("<tr bgcolor='white'>")
              toggleRowColor = False
            End If

            If nExpireYear > 0 Then

              If holdCompID <> CLng(r.Item("comp_id").ToString) Then

                htmlOut.Append("<td valign='top' align='left' width='2%'><img src='images/ch_red.jpg' class='bullet' alt='" + nItemCount.ToString + "' title='" + nItemCount.ToString + "' /></td>")
                htmlOut.Append("<td align='left' valign='top' width='75%'>")
                htmlOut.Append(commonEvo.get_company_info_from_datarow(r, 0, True, True, "", ""))
                htmlOut.Append("</td>")

                holdCompID = CLng(r.Item("comp_id").ToString)

              Else

                htmlOut.Append("<td valign='top' align='left' width='2%'></td>")
                htmlOut.Append("<td align='left' valign='top' width='75%'></td>")

              End If

              htmlOut.Append("<td align='right' valign='middle' style='padding-right:30px;'>")
              If Not IsDBNull(r.Item("cref_owner_percent")) Then
                If Not String.IsNullOrEmpty(r.Item("cref_owner_percent").ToString.Trim) Then
                  htmlOut.Append(r.Item("cref_owner_percent").ToString + "%")
                End If
              End If
              htmlOut.Append("</td>")

              htmlOut.Append("<td align='right' valign='middle' style='padding-right:10px;'>")
              If Not IsDBNull(r.Item("cref_fraction_expires_date")) Then
                If Not String.IsNullOrEmpty(r.Item("cref_fraction_expires_date").ToString.Trim) Then
                  htmlOut.Append(FormatDateTime(r.Item("cref_fraction_expires_date").ToString, DateFormat.ShortDate))
                End If
              End If
              htmlOut.Append("</td></tr>")

            Else

              If holdCompID <> CLng(r.Item("comp_id").ToString) Then

                htmlOut.Append("<td valign='top' align='left' width='2%'><img src='images/ch_red.jpg' class='bullet' alt='" + nItemCount.ToString + "' title='" + nItemCount.ToString + "' /></td>")
                htmlOut.Append("<td align='left' valign='top' width='80%'>")

                htmlOut.Append(commonEvo.get_company_info_from_datarow(r, 0, True, True, "", "") + "</td>")

                htmlOut.Append("<td align='right' valign='middle' style='padding-right:50px;'>")
                If Not IsDBNull(r.Item("sumPercent")) Then
                  If Not String.IsNullOrEmpty(r.Item("sumPercent").ToString.Trim) Then
                    htmlOut.Append(r.Item("sumPercent").ToString + "%")
                  End If
                End If

                htmlOut.Append("</td></tr>")

                holdCompID = CLng(r.Item("comp_id").ToString)

              End If

            End If

          End If

          nItemCount += 1

        Next ' icount

        htmlOut.Append("</table></td></tr></table>")

        htmlOut.Append("<table cellspacing='0' cellpadding='2' border='0' width='100%'>")
        htmlOut.Append("<tr>")

        If Not bUseExpire Then
          If nAbsPage > 1 Then
            htmlOut.Append("<td valign='middle' align='center' width='25%'>")
            htmlOut.Append("<input type='button' value='< Previous Page' onclick='javascript:document.location.href=""fractionalShareholderList.aspx?AmodID=" + inAmodID.ToString + "&ProgramID=" + inProgramID.ToString + "&AbsPage=" + (nAbsPage - 1).ToString + """;' title='Click to View The Previous " + nPageSize.ToString + " Results' /></td>" + vbCrLf)
          Else
            htmlOut.Append("<td valign='middle' align='center' width='25%'>")
            htmlOut.Append("<input type='button' disabled='disabled' value='< Previous Page' /></td>" + vbCrLf)
          End If
        Else
          If nAbsPage > 1 Then
            htmlOut.Append("<td valign='middle' align='center' width='25%'>")
            htmlOut.Append("<input type='button' value='< Previous Page' onclick='javascript:document.location.href=""fractionalShareholderList.aspx?AmodID=" + inAmodID.ToString + "&ProgramID=" + inProgramID.ToString + "&expireYear=" + nExpireYear.ToString + "&expireFlag=" + sExpireFlag + "&AbsPage=" + (nAbsPage - 1).ToString + """;' title='Click to View The Previous " + nPageSize.ToString + " Results' /></td>" + vbCrLf)
          Else
            htmlOut.Append("<td valign='middle' align='center' width='25%'>")
            htmlOut.Append("<input type='button' disabled='disabled' value='< Previous Page' /></td>" + vbCrLf)
          End If
        End If

        htmlOut.Append("<td valign='middle' align='center' nowrap='nowrap' width='25%'>")
        htmlOut.Append("<font color='#2E57B6'>&nbsp;&nbsp;<b>" + results_table.Rows.Count.ToString + "&nbsp;Companies&nbsp;Found&nbsp;&nbsp;Page&nbsp;" + nAbsPage.ToString + "&nbsp;of&nbsp;" + nNumPages.ToString + "</b></font>&nbsp;&nbsp;</td>" + vbCrLf)

        If Not bUseExpire Then
          If results_table.Rows.Count > (nAbsPage * nPageSize) Then
            htmlOut.Append("<td valign='middle' align='center' width='25%'>")
            htmlOut.Append("<input type='button' value='Next Page >' onclick='javascript:document.location.href=""fractionalShareholderList.aspx?AmodID=" + inAmodID.ToString + "&ProgramID=" + inProgramID.ToString + "&AbsPage=" + (nAbsPage + 1).ToString + """;' title='Click to View The Next " + nPageSize.ToString + " Results' /></td>" + vbCrLf)
          Else
            htmlOut.Append("<td valign='middle' align='center' width='25%'>")
            htmlOut.Append("<input type='button' disabled='disabled' value='Next Page >' /></td>" + vbCrLf)
          End If
        Else
          If results_table.Rows.Count > (nAbsPage * nPageSize) Then
            htmlOut.Append("<td valign='middle' align='center' width='25%'>")
            htmlOut.Append("<input type='button' value='Next Page >' onclick='javascript:document.location.href=""fractionalShareholderList.aspx?AmodID=" + inAmodID.ToString + "&ProgramID=" + inProgramID.ToString + "&expireYear=" + nExpireYear.ToString + "&expireFlag=" + sExpireFlag + "&AbsPage=" + (nAbsPage + 1).ToString + """;' title='Click to View The Next " + nPageSize.ToString + " Results' /></td>" + vbCrLf)
          Else
            htmlOut.Append("<td valign='middle' align='center' width='25%'>")
            htmlOut.Append("<input type='button' disabled='disabled' value='Next Page >' /></td>" + vbCrLf)
          End If
        End If

        htmlOut.Append("<td valign='middle' align='center' width='25%'>")
        htmlOut.Append("<input type='button' value='Export/Report' onclick='javascript:load(""PDF_Creator.aspx?Area=shareholder&FrAmodID=" + inAmodID.ToString + "&FrProgramID=" + inProgramID.ToString + ""","""",""ReportOptions"");' title='Click to View Reporting Options' /></td></tr>" + vbCrLf)
        htmlOut.Append("</table>" + vbCrLf)

      Else

        htmlOut.Append("<table id='fractionalShareHoldersOuterTable' width='100%' cellspacing='0' cellpadding='2' border='1'>")
        htmlOut.Append("<tr><td valign='middle' align='center' class='header' colspan='3' style='padding-left:3px;'>FRACTIONAL SHAREHOLDERS</td></tr>")
        htmlOut.Append("<tr><td valign='middle' align='left' class='seperator' width='2%'>&nbsp;</td>")
        htmlOut.Append("<td valign='top' align='left' class='seperator' width='80%'><strong>Company&nbsp;Name</strong></td>")
        htmlOut.Append("<td valign='top' align='left' class='seperator'><strong>%&nbsp;of&nbsp;Shares</strong></td></tr>")
        htmlOut.Append("<tr><td colspan='3' class='rightside' valign='top'>")
        htmlOut.Append("<table id='fractionalShareHoldersDataTable' width='100%' cellspacing='0' cellpadding='4' border='1'>")
        htmlOut.Append("<tr><td valign='middle' align='center'>No Fractional Shareholders Found</td></tr>")
        htmlOut.Append("</table></td></tr></table>")

      End If ' not (localAdoRs.bof and localAdoRs.eof) 	

      htmlOut.Append("</td></tr></table>") ' end outer table

    Catch ex As Exception

      results_table = Nothing

    Finally

    End Try

    Return htmlOut.ToString

  End Function

End Class