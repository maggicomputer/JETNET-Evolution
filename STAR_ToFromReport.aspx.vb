Partial Public Class STAR_ToFromReport
  Inherits System.Web.UI.Page

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    Dim searchCriteria As New viewSelectionCriteriaClass

    Dim marketLinkString As String = ""
    Dim tmpFunc As New crmLocalUserClass

    If Not IsNothing(Request.Item("starReport")) Then
      If Not String.IsNullOrEmpty(Request.Item("starReport").ToString.Trim) Then
        searchCriteria.ViewCriteriaStarReportID = CInt(Request.Item("starReport").ToString)
      End If
    End If

    If Not IsNothing(Request.Item("marketSelection")) Then
      If Not String.IsNullOrEmpty(Request.Item("marketSelection").ToString.Trim) Then
        marketLinkString = Replace(Request.Item("marketSelection").ToString, Constants.cSvrStringSeperator, Constants.cDymDataSeperator)
      End If
    End If

    ' parse market link string to "extract" selected aircraft
    parseMarketSelectionString(searchCriteria, marketLinkString)

    Select Case (searchCriteria.ViewCriteriaStarReportID)
      Case 5
        Master.SetPageTitle("STAR Report - Upgrade To Path")
      Case 12
        Master.SetPageTitle("STAR Report - Upgrade From Path")

    End Select

    Dim STAR_Functions As New star_view_functions

    Dim htmlOut As New StringBuilder
    Dim starHtmlOut As String = ""
    Dim aMaxDate As String = ""
    Dim results_table As New DataTable

    Try

      STAR_Functions.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      STAR_Functions.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
      STAR_Functions.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
      STAR_Functions.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
      STAR_Functions.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim

      results_table = STAR_Functions.get_max_star_report_date(searchCriteria, True)

      If Not IsNothing(results_table) Then
        If results_table.Rows.Count > 0 Then
          For Each r As DataRow In results_table.Rows

            If Not IsDBNull(r.Item("ac1_start_date")) Then
              If Not String.IsNullOrEmpty(r.Item("ac1_start_date").ToString.Trim) Then
                aMaxDate = FormatDateTime(r.Item("ac1_start_date").ToString, vbShortDate)
                searchCriteria.ViewCriteriaStarReportDate = CDate(r.Item("ac1_start_date").ToString)
              Else
                aMaxDate = "that timeframe"
              End If
            End If

          Next
        Else
          aMaxDate = "that timeframe"
        End If
      Else
        aMaxDate = "that timeframe"
      End If

      htmlOut.Append("<table width=""100%"" cellpadding=""4"" cellspacing=""0""><tr><td align=""left"" valign=""top"">")
      htmlOut.Append("<table width=""100%"" border='0' cellpadding='0' cellspacing='0'><tr><td align='center' valign='top'>") 'start  outer model star table

      htmlOut.Append("<table width=""100%"" cellpadding='2' cellspacing='0'>")
      htmlOut.Append("<tr><td align='center' valign='middle'><img src='images/Star_Report_img.jpg' alt='starReportID:" + searchCriteria.ViewCriteriaStarReportID.ToString + "' border='1'/></td></tr>")
      htmlOut.Append("<tr><td align='center' valign='middle'><i> Note that all <strong>STAR</strong> reports are compiled at the end of each month and therefore represent a snapshot of data</i></td></tr>")
      htmlOut.Append("<tr><td align='center' valign='middle'><i> as of " + aMaxDate.Trim + " resulting in data that may not be a direct match to live data summaries.</i></td></tr>")
      htmlOut.Append("</table>")

      htmlOut.Append("<br />")

      If searchCriteria.ViewCriteriaStarReportID = 5 Then

        STAR_Functions.views_display_star_aircraft_5(searchCriteria, starHtmlOut, True)
        htmlOut.Append(starHtmlOut + "<br />")

      Else

        STAR_Functions.views_display_star_aircraft_12(searchCriteria, starHtmlOut, True)
        htmlOut.Append(starHtmlOut + "<br />")

      End If

      htmlOut.Append("</td></tr></table>") 'end inner table
      htmlOut.Append("</td></tr></table>") 'end outer table

    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "error in [Build_star_report] : " + ex.Message
    End Try

    star_Report_html.Text = htmlOut.ToString
    htmlOut = Nothing

  End Sub

  Private Sub parseMarketSelectionString(ByRef localcriteria As viewSelectionCriteriaClass, ByVal sMarketSelection As String)

    Try

      localcriteria.ViewSelectionCriteriaDetailError = eObjDetailErrorCode.NULL
      localcriteria.ViewSelectionCriteriaStatusCode = eObjStatusCode.NULL

      Dim sepArry(2) As Char
      sepArry(0) = Constants.cSvrRecordSeperator.Substring(0, 1)
      sepArry(1) = Constants.cSvrRecordSeperator.Substring(1, 1)
      sepArry(2) = Constants.cSvrRecordSeperator.Substring(2, 1)

      Dim tmpArray() As String = sMarketSelection.Split(sepArry, StringSplitOptions.RemoveEmptyEntries)

      For Each mSel As String In tmpArray

        Dim tmpSplit() As String = mSel.Split(Constants.cEq.Trim)

        Select Case tmpSplit(0).ToLower

          Case "chkhelicopterfilterid"
            localcriteria.ViewCriteriaHasHelicopterFlag = CBool(tmpSplit(1))
          Case "chkbusinessfilterid"
            localcriteria.ViewCriteriaHasBusinessFlag = CBool(tmpSplit(1))
          Case "chkcommercialfilterid"
            localcriteria.ViewCriteriaHasCommercialFlag = CBool(tmpSplit(1))
          Case "cboaircrafttypeid"

            Dim tmpValue() = tmpSplit(1).Split(Constants.cSvrDataSeperator)

            localcriteria.ViewCriteriaAircraftType = "'" + tmpValue(0).ToUpper + "'"
            localcriteria.ViewCriteriaAirframeTypeStr = "'" + tmpValue(1).ToUpper + "'"

          Case "cboaircraftmakeid"

            Dim tmpMake() As String = Split(tmpSplit(1).ToUpper.Replace(Constants.cDymDataSeperator, Constants.cCommaDelim), Constants.cCommaDelim)
            Dim modelString As String = ""

            For Each sMake As String In tmpMake

              If String.IsNullOrEmpty(modelString.Trim) Then
                modelString = sMake.Substring(0, sMake.IndexOf(Constants.cSvrDataSeperator))
              Else
                modelString += Constants.cCommaDelim + sMake.Substring(0, sMake.IndexOf(Constants.cSvrDataSeperator))
              End If

            Next

            localcriteria.ViewCriteriaAircraftMake = "'" + modelString.Replace(Constants.cCommaDelim, Constants.cValueSeperator) + "'"

          Case "cboaircraftmodelid"

            localcriteria.ViewCriteriaAircraftModel = tmpSplit(1)


        End Select

      Next

    Catch ex As Exception
      localcriteria.ViewSelectionCriteriaDetailError = eObjDetailErrorCode.FUNCTION_EXCEPTION
      localcriteria.ViewSelectionCriteriaStatusCode = eObjStatusCode.FAILURE
    End Try

  End Sub


End Class