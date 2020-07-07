' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/homebasePerformance.aspx.vb $
'$$Author: Matt $
'$$Date: 10/01/19 2:11p $
'$$Modtime: 10/01/19 1:28p $
'$$Revision: 3 $
'$$Workfile: homebasePerformance.aspx.vb $
'
' ********************************************************************************

Partial Public Class homebasePerformance
  Inherits System.Web.UI.Page

  Private localDatalayer As viewsDataLayer
  Private localCriteria As New viewSelectionCriteriaClass
  Private SharedModelTable As DataTable = Nothing

  Private Amod_weight_class_name As String = ""
  Private Amod_manufacturer As String = ""
  Private Amod_description As String = ""
  Private Amod_start_end_years As String = ""
  Private Amod_ser_nbr_range As String = ""
  Private Amod_type_name As String = ""
  Private Amod_price_range As String = ""
  Private Amod_body_config As String = ""
    Private make_model_name As String = ""
    Private ac_id As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    localDatalayer = New viewsDataLayer
    localDatalayer.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
    localDatalayer.clientConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

    If String.IsNullOrEmpty(HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim) Then
      HttpContext.Current.Session.Item("jetnetClientDatabase") = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
    End If

    If Not IsNothing(Request.Item("AmodID")) Then
      If Not String.IsNullOrEmpty(Request.Item("AmodID").ToString.Trim) Then
        If IsNumeric(Request.Item("AmodID").ToString) Then

          localCriteria.ViewCriteriaAmodID = CLng(Request.Item("AmodID").ToString)
          localCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_NONE
          localCriteria.ViewCriteriaHasHelicopterFlag = True
          localCriteria.ViewCriteriaHasBusinessFlag = True
                    localCriteria.ViewCriteriaHasCommercialFlag = True

                    localCriteria.ViewCriteriaAircraftID = 0
                    If Not IsNothing(Request.Item("ac_id")) Then
                        If Not String.IsNullOrEmpty(Request.Item("ac_id").ToString.Trim) Then
                            ac_id = Request.Item("ac_id").ToString.Trim
                            localCriteria.ViewCriteriaAircraftID = ac_id
                        End If
                    End If



                    get_make_model_info(localCriteria)

                            Master.SetPageTitle(make_model_name.Trim + " - Homebase Performance - " + WeekdayName(Weekday(Today)).ToString + ", " + MonthName(Month(Today)).ToString + " " + Day(Today).ToString + ", " + Year(Today).ToString)

                            Build_Performance_Page(localCriteria)

                        End If
                    Else
        Response.Redirect("Default.aspx", True)
      End If
    Else
      Response.Redirect("Default.aspx", True)
    End If

  End Sub

  Private Sub Build_Performance_Page(ByRef searchCriteria As viewSelectionCriteriaClass)

    Dim htmlModelPerformance As String = ""
    Dim htmlOut As New StringBuilder
    Dim tempStr As String = ""

    Try

      tempStr += "<div style=""overflow: none; text-align: center; padding:25px;"">"
      tempStr += "<table id=""modelInfoTbl"" width=""100%"" cellspacing=""0"" cellpadding=""2"">"
      tempStr += "<tr><td><b>MAKE&nbsp;MODEL&nbsp;:&nbsp;</b></td><td colspan=""3"">" + make_model_name.Trim + "</td></tr>"

      tempStr += "<tr><td colspan=""4"" width=""100%"" height=""1"" bgcolor=""#67A0D9""></td></tr>"

      tempStr += "<tr><td><b>MANUFACTURER&nbsp;:&nbsp;</b></td><td>" + Amod_manufacturer.Trim + "</td>"
      tempStr += "<td><b>YEARS BUILT&nbsp;:&nbsp;</b></td><td>" + Amod_start_end_years.Trim + "</td></tr>"

      tempStr += "<tr><td><b>SER # RANGE&nbsp;:&nbsp;</b></td><td>" + Amod_ser_nbr_range.Trim + "</td>"
      tempStr += "<td><b>TYPE&nbsp;:&nbsp;</b></td><td>" + Amod_type_name.Trim + "</td></tr>"

      If String.IsNullOrEmpty(Amod_body_config.Trim) Then
        tempStr += "<tr><td><b>WEIGHT CLASS&nbsp;:&nbsp;</b></td><td>" + Amod_weight_class_name.Trim + "</td>"
        tempStr += "<td><b>General Market Price Range&nbsp;:&nbsp;</b></td><td>" + Amod_price_range.Trim + "</td></tr>"
      Else
        tempStr += "<tr><td><b>WEIGHT CLASS&nbsp;:&nbsp;</b></td><td>" + Amod_weight_class_name.Trim + "</td>"
        tempStr += "<td><b>BODY CONFIGURATION&nbsp;:&nbsp;</b></td><td>" + Amod_body_config.Trim + "</td></tr>"
        tempStr += "<tr><td colspan=""2"">&nbsp;</td><td><b>General Market Price Range&nbsp;:&nbsp;</b></td><td>" + Amod_price_range.Trim + "</td></tr>"
      End If

      tempStr += "<tr><td colspan=""4"" width=""100%"" height=""1"" bgcolor=""#67A0D9""></td></tr>"
      tempStr += "<tr><td colspan=""4"" align=""left"" valign=""top""><br />" + Amod_description.Trim + "</td></tr></table></div><br />"

      htmlOut.Append(tempStr.Trim + "<div style=""height:650px; overflow: auto; text-align: center; padding-left:25px;"">")

      htmlOut.Append("<table id=""outerPerformanceTbl"" width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"">")
      htmlOut.Append("<tr>")

      searchCriteria.ViewCriteriaUseMetricValues = False
      localDatalayer.views_display_performance_specs(False, "HTML", True, False, searchCriteria, htmlModelPerformance, SharedModelTable)
      htmlOut.Append(htmlModelPerformance)

      searchCriteria.ViewCriteriaUseMetricValues = False
            localDatalayer.views_display_performance_specs(False, "HTML", False, False, searchCriteria, htmlModelPerformance, SharedModelTable)
            htmlOut.Append(htmlModelPerformance)

      searchCriteria.ViewCriteriaUseMetricValues = True
      localDatalayer.views_display_performance_specs(False, "HTML", False, False, searchCriteria, htmlModelPerformance, SharedModelTable)
      htmlOut.Append(htmlModelPerformance)

      htmlOut.Append("</tr></table></div>")

      performance_listing_text.Text = htmlOut.ToString()

    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "** Error in [homebasePerformance.aspx.vb :[Build_Performance_Page] : " + ex.Message
    End Try

    htmlOut = Nothing

  End Sub

  Function get_make_model_info(ByRef searchCriteria As viewSelectionCriteriaClass) As Boolean

    Dim make_name As String = ""
    Dim model_name As String = ""
    Dim weight_class As String = ""
    Dim type_code As String = ""
    Dim airframe_type As String = ""

    Dim fAmod_start_year As String = ""
    Dim fAmod_end_year As String = ""
    Dim fAmod_ser_no_prefix As String = ""
    Dim fAmod_ser_no_start As String = ""
    Dim fAmod_ser_no_end As String = ""
    Dim fAmod_ser_no_suffix As String = ""
    Dim fAmod_start_price As Integer = 0
    Dim fAmod_end_price As Integer = 0

    'Dim results_table As DataTable

    Try

      CheckSharedModelTable()

      If Not IsNothing(SharedModelTable) Then

        If SharedModelTable.Rows.Count > 0 Then

          For Each r As DataRow In SharedModelTable.Rows

            If Not IsDBNull(r.Item("amod_make_name")) Then
              If Not String.IsNullOrEmpty(r.Item("amod_make_name").ToString) Then
                make_name = r.Item("amod_make_name").ToString.Trim
              End If
            End If

            If Not IsDBNull(r.Item("amod_model_name")) Then
              If Not String.IsNullOrEmpty(r.Item("amod_make_name").ToString) Then
                model_name = r.Item("amod_model_name").ToString.Trim
              End If
            End If

            make_model_name = make_name + " " + model_name

            If Not IsDBNull(r.Item("amod_manufacturer")) Then
              If Not String.IsNullOrEmpty(r.Item("amod_manufacturer").ToString) Then
                Amod_manufacturer = r.Item("amod_manufacturer").ToString.Trim
              End If
            End If

            If Not IsDBNull(r.Item("amod_type_code")) Then
              If Not String.IsNullOrEmpty(r.Item("amod_type_code").ToString) Then
                type_code = r.Item("amod_type_code").ToString.ToUpper.Trim
              End If
            End If

            If Not IsDBNull(r.Item("amod_airframe_type_code")) Then
              If Not String.IsNullOrEmpty(r.Item("amod_airframe_type_code").ToString) Then
                airframe_type = r.Item("amod_airframe_type_code").ToString.ToUpper.Trim
              End If
            End If

            Select Case type_code.Trim.ToUpper
              Case Constants.AMOD_TYPE_AIRLINER
                Amod_type_name = "Jet Airliner"
              Case Constants.AMOD_TYPE_JET
                Amod_type_name = "Business Jet"
              Case Constants.AMOD_TYPE_TURBO
                If airframe_type.Trim.ToUpper.Contains(Constants.AMOD_ROTARY_AIRFRAME) Then
                  Amod_type_name = "Turbine"
                Else
                  Amod_type_name = "Turboprop"
                End If
              Case Constants.AMOD_TYPE_PISTON
                Amod_type_name = "Piston"
            End Select

            If Not IsDBNull(r.Item("amod_weight_class")) Then
              If Not String.IsNullOrEmpty(r.Item("amod_weight_class").ToString) Then
                weight_class = r.Item("amod_weight_class").ToString.ToUpper.Trim
              End If
            End If

            Select Case weight_class
              Case "V"
                Amod_weight_class_name = "Very Light Jet"
              Case "L"
                Amod_weight_class_name = "Light"
              Case "M"
                Amod_weight_class_name = "Medium"
              Case "H"
                Amod_weight_class_name = "Heavy"
            End Select

            If Not IsDBNull(r.Item("amod_start_year")) Then
              If Not String.IsNullOrEmpty(r.Item("amod_start_year").ToString) Then
                fAmod_start_year = r.Item("amod_start_year").ToString.ToUpper.Trim
              End If
            End If

            If Not IsDBNull(r.Item("amod_end_year")) Then
              If Not String.IsNullOrEmpty(r.Item("amod_end_year").ToString) Then
                fAmod_end_year = r.Item("amod_end_year").ToString.ToUpper.Trim
              End If
            End If

            Amod_start_end_years = fAmod_start_year

            If Not String.IsNullOrEmpty(fAmod_end_year) Then
              Amod_start_end_years += " - " + fAmod_end_year + "&nbsp;"
            ElseIf Not String.IsNullOrEmpty(fAmod_start_year) Then
              Amod_start_end_years += " - Present&nbsp;"
            Else
              Amod_start_end_years += "&nbsp;"
            End If

            If Not IsDBNull(r.Item("amod_ser_no_prefix")) Then
              If Not String.IsNullOrEmpty(r.Item("amod_ser_no_prefix").ToString) Then
                fAmod_ser_no_prefix = r.Item("amod_ser_no_prefix").ToString.ToUpper.Trim
              End If
            End If

            If Not IsDBNull(r.Item("amod_ser_no_start")) Then
              If Not String.IsNullOrEmpty(r.Item("amod_ser_no_start").ToString) Then
                fAmod_ser_no_start = r.Item("amod_ser_no_start").ToString.ToUpper.Trim
              End If
            End If

            If Not IsDBNull(r.Item("amod_ser_no_end")) Then
              If Not String.IsNullOrEmpty(r.Item("amod_ser_no_end").ToString) Then
                fAmod_ser_no_end = r.Item("amod_ser_no_end").ToString.ToUpper.Trim
              End If
            End If

            If Not IsDBNull(r.Item("amod_ser_no_suffix")) Then
              If Not String.IsNullOrEmpty(r.Item("amod_ser_no_suffix").ToString) Then
                fAmod_ser_no_suffix = r.Item("amod_ser_no_suffix").ToString.ToUpper.Trim
              End If
            End If

            Amod_ser_nbr_range = fAmod_ser_no_prefix + fAmod_ser_no_start + fAmod_ser_no_suffix

            If Not String.IsNullOrEmpty(fAmod_ser_no_end) Then
              Amod_ser_nbr_range += " - " + fAmod_ser_no_prefix + fAmod_ser_no_end + fAmod_ser_no_suffix + "&nbsp;"
            ElseIf Not String.IsNullOrEmpty(fAmod_ser_no_start) Then
              Amod_ser_nbr_range += " &amp; Up&nbsp;"
            Else
              Amod_ser_nbr_range += "&nbsp;"
            End If

            If Not IsDBNull(r.Item("amod_start_price")) Then
              If Not String.IsNullOrEmpty(r.Item("amod_start_price").ToString) Then
                fAmod_start_price = CDbl(r.Item("amod_start_price").ToString)
              End If
            End If

            If Not IsDBNull(r.Item("amod_end_price")) Then
              If Not String.IsNullOrEmpty(r.Item("amod_end_price").ToString) Then
                fAmod_end_price = CDbl(r.Item("amod_end_price").ToString)
              End If
            End If

            If fAmod_start_price <> 0 Then
              Amod_price_range += "$" & FormatNumber(fAmod_start_price, 0, False, False, True)
            Else
              Amod_price_range += "&nbsp;"
            End If

            If fAmod_end_price <> 0 Then
              Amod_price_range += " - $" & FormatNumber(fAmod_end_price, 0, False, False, True) & "&nbsp;"
            Else
              Amod_price_range += "&nbsp;"
            End If

            If Not IsDBNull(r.Item("amod_description")) Then
              If Not String.IsNullOrEmpty(r.Item("amod_description").ToString) Then
                Amod_description = r.Item("amod_description").ToString.Trim
              End If
            End If

            If Not IsDBNull(r.Item("ambc_name")) Then
              If Not String.IsNullOrEmpty(r.Item("ambc_name").ToString) Then
                If Not r.Item("ambc_name").ToString.ToLower.Contains("unknown") And (r.Item("amod_product_helicopter_flag").ToString.ToUpper.Trim.Contains("Y") Or r.Item("amod_product_commercial_flag").ToString.ToUpper.Trim.Contains("Y")) Then
                  Amod_body_config = r.Item("ambc_name").ToString.Trim
                End If
              End If
            End If

          Next

        End If

      End If

    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "** Error in homebasePerformance.aspx.vb :  [get_make_model_info] : " + ex.Message
    End Try

  End Function

  Public Sub CheckSharedModelTable()
    'All that this does is check for the existence of the shared model table - basically so you don't use it if it doesn't exist.
    'It returns nothing but fills it up if it's not there.
    If Not IsNothing(SharedModelTable) Then
      If SharedModelTable.Rows.Count = 0 Then
        'We need to fill this up. If it has rows already, it already exists - so we're good to go.
        SharedModelTable = commonEvo.get_view_model_info(localCriteria, True)
      End If
    Else
      SharedModelTable = commonEvo.get_view_model_info(localCriteria, True)
    End If
  End Sub

End Class