' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/Entity Classes/charter_view_functions.vb $
'$$Author: Matt $
'$$Date: 3/26/20 2:08p $
'$$Modtime: 3/26/20 1:58p $
'$$Revision: 5 $
'$$Workfile: charter_view_functions.vb $
'
' ********************************************************************************

<System.Serializable()> Public Class charter_view_functions

  Private aError As String
  Private clientConnectString As String
  Private adminConnectString As String

  Private starConnectString As String
  Private cloudConnectString As String
  Private serverConnectString As String

  Sub New()

    aError = ""
    clientConnectString = ""
    adminConnectString = ""

    starConnectString = ""
    cloudConnectString = ""
    serverConnectString = ""

  End Sub

  Public Property class_error() As String
    Get
      class_error = aError
    End Get
    Set(ByVal value As String)
      aError = value
    End Set
  End Property

#Region "database_connection_strings"

  Public Property adminConnectStr() As String
    Get
      adminConnectStr = adminConnectString
    End Get
    Set(ByVal value As String)
      adminConnectString = value
    End Set
  End Property

  Public Property clientConnectStr() As String
    Get
      clientConnectStr = clientConnectString
    End Get
    Set(ByVal value As String)
      clientConnectString = value
    End Set
  End Property

  Public Property starConnectStr() As String
    Get
      starConnectStr = starConnectString
    End Get
    Set(ByVal value As String)
      starConnectString = value
    End Set
  End Property

  Public Property cloudConnectStr() As String
    Get
      cloudConnectStr = cloudConnectString
    End Get
    Set(ByVal value As String)
      cloudConnectString = value
    End Set
  End Property

  Public Property serverConnectStr() As String
    Get
      serverConnectStr = serverConnectString
    End Get
    Set(ByVal value As String)
      serverConnectString = value
    End Set
  End Property

#End Region

#Region "charter_view_functions"

  Public Function get_charter_companies(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      HttpContext.Current.Session.Item("Selection_Listing_Fields") = ""
      HttpContext.Current.Session.Item("Selection_Listing_Table") = ""
      HttpContext.Current.Session.Item("Selection_Listing_Where") = ""
      HttpContext.Current.Session.Item("Selection_Listing_Group") = ""
      HttpContext.Current.Session.Item("Selection_Listing_Order") = ""

      If searchCriteria.ViewCriteriaCompanyID > 0 Then
        HttpContext.Current.Session.Item("Selection_Listing_Fields") = ("SELECT DISTINCT comp_id, comp_address1, comp_name, count(distinct ac_id) AS ac_count, count(distinct ccerttype_id) as certcount, comp_address2, comp_city, comp_state, comp_zip_code, comp_country")
      ElseIf searchCriteria.ViewCriteriaAmodID > -1 And searchCriteria.ViewCriteriaCompanyID = 0 Then
        HttpContext.Current.Session.Item("Selection_Listing_Fields") = ("SELECT DISTINCT comp_id, comp_address1, comp_name, count(distinct ac_id) AS ac_count, count(distinct ccerttype_id) as certcount, comp_address2, comp_city, comp_state, comp_zip_code, comp_country")
      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaCountry.Trim) Or Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaCity.Trim) Then
        HttpContext.Current.Session.Item("Selection_Listing_Fields") = ("SELECT DISTINCT TOP 250 comp_id, comp_address1, comp_name, count(distinct ac_id) AS ac_count, count(distinct ccerttype_id) as certcount, comp_address2, comp_city, comp_state, comp_zip_code, comp_country")
      Else
        HttpContext.Current.Session.Item("Selection_Listing_Fields") = ("SELECT DISTINCT TOP 50 comp_id, comp_address1, comp_name, count(distinct ac_id) AS ac_count, count(distinct ccerttype_id) as certcount, comp_address2, comp_city, comp_state, comp_zip_code, comp_country")
      End If


      sQuery.Append(HttpContext.Current.Session.Item("Selection_Listing_Fields"))


      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "comp_id", "comp_id as 'COMPID'")
      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "comp_name", "comp_name as 'COMPNAME'")
      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "comp_address1", "comp_address1 as 'ADDRESS'")
      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "comp_address2", "comp_address2 as 'ADDRESS2'")
      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "comp_city", "comp_city as 'CITY'")
      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "comp_state", "comp_state as 'STATE'")
      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "comp_zip_code", "comp_zip_code as 'ZIP'")
      HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), "comp_country", "comp_country as 'COUNTRY'")


      HttpContext.Current.Session.Item("Selection_Listing_Table") = (" FROM aircraft_summary WITH(NOLOCK)")
      HttpContext.Current.Session.Item("Selection_Listing_Table") &= (" LEFT OUTER JOIN company_certification ON ccert_comp_id = comp_id AND ccert_journ_id = 0 ")
      HttpContext.Current.Session.Item("Selection_Listing_Table") &= (" LEFT OUTER JOIN company_certification_type ON ccert_type_id = ccerttype_id AND ccerttype_charter_flag = 'Y' ")

      sQuery.Append(HttpContext.Current.Session.Item("Selection_Listing_Table"))


      HttpContext.Current.Session.Item("Selection_Listing_Where") &= (" WHERE ac_lifecycle_stage = 3 AND (cref_contact_type IN ('94','33') OR cref_business_type in ('CH'))")

      If searchCriteria.ViewCriteriaCompanyID > 0 Then
        HttpContext.Current.Session.Item("Selection_Listing_Where") &= (Constants.cAndClause + "comp_id = " + searchCriteria.ViewCriteriaCompanyID.ToString)
      End If

      If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaCountry.Trim) Then
        HttpContext.Current.Session.Item("Selection_Listing_Where") &= (Constants.cAndClause + "comp_country <> '' AND lower(comp_country) = '" + Replace(searchCriteria.ViewCriteriaCountry.Trim, Constants.cSingleQuote, Constants.cDoubleSingleQuote).ToLower + "'")
      End If

      If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaCity.Trim) Then
        HttpContext.Current.Session.Item("Selection_Listing_Where") &= (Constants.cAndClause + "comp_city <> '' AND lower(comp_city) = '" + Replace(searchCriteria.ViewCriteriaCity.Trim, Constants.cSingleQuote, Constants.cDoubleSingleQuote).ToLower + "'")
      End If

      If searchCriteria.ViewCriteriaAmodID > -1 Then
        HttpContext.Current.Session.Item("Selection_Listing_Where") &= (Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
        HttpContext.Current.Session.Item("Selection_Listing_Where") &= (Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
      End If

      Select Case CInt(searchCriteria.ViewCriteriaAirframeType)
        Case Constants.VIEW_EXECUTIVE
          HttpContext.Current.Session.Item("Selection_Listing_Where") &= (Constants.cAndClause + "(amod_customer_flag = 'Y' AND amod_airframe_type_code='F' AND amod_type_code = 'E')")
        Case Constants.VIEW_JETS
          HttpContext.Current.Session.Item("Selection_Listing_Where") &= (Constants.cAndClause + "(amod_customer_flag = 'Y' AND amod_airframe_type_code='F' AND amod_type_code = 'J')")
        Case Constants.VIEW_TURBOPROPS
          HttpContext.Current.Session.Item("Selection_Listing_Where") &= (Constants.cAndClause + "(amod_customer_flag = 'Y' AND amod_airframe_type_code='F' AND amod_type_code = 'T')")
        Case Constants.VIEW_PISTONS
          HttpContext.Current.Session.Item("Selection_Listing_Where") &= (Constants.cAndClause + "(amod_customer_flag = 'Y' AND amod_airframe_type_code='F' AND amod_type_code = 'P')")
        Case Constants.VIEW_HELICOPTERS
          HttpContext.Current.Session.Item("Selection_Listing_Where") &= (Constants.cAndClause + "(amod_customer_flag = 'Y' AND amod_airframe_type_code='R' AND amod_type_code in ('T','P'))")
      End Select

      If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAirframeTypeStr.Trim) Then
        HttpContext.Current.Session.Item("Selection_Listing_Where") &= (Constants.cAndClause + "amod_airframe_type_code = '" + searchCriteria.ViewCriteriaAirframeTypeStr.Trim + "'")
      End If

      If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
        HttpContext.Current.Session.Item("Selection_Listing_Where") &= (" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))
      Else
        HttpContext.Current.Session.Item("Selection_Listing_Where") &= (" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True))
      End If



      If searchCriteria.ViewCriteriaAFTTStart > 0 Then
        HttpContext.Current.Session.Item("Selection_Listing_Where") &= (Constants.cAndClause + " ((ac_airframe_tot_hrs >= " & searchCriteria.ViewCriteriaAFTTStart & ") or (ac_airframe_tot_hrs IS NULL))")
      End If

      If searchCriteria.ViewCriteriaAFTTEnd > 0 Then
        HttpContext.Current.Session.Item("Selection_Listing_Where") &= (Constants.cAndClause + " ((ac_airframe_tot_hrs <= " & searchCriteria.ViewCriteriaAFTTEnd & ") or (ac_airframe_tot_hrs IS NULL))")
      End If


      If searchCriteria.ViewCriteriaYearStart > 0 Then
        HttpContext.Current.Session.Item("Selection_Listing_Where") &= (Constants.cAndClause + " ac_mfr_year >= " & searchCriteria.ViewCriteriaYearStart)
      End If


      If searchCriteria.ViewCriteriaYearEnd > 0 Then
        HttpContext.Current.Session.Item("Selection_Listing_Where") &= (Constants.cAndClause + " ac_mfr_year <=  " & searchCriteria.ViewCriteriaYearEnd)
      End If

      sQuery.Append(HttpContext.Current.Session.Item("Selection_Listing_Where"))

      HttpContext.Current.Session.Item("Selection_Listing_Group") = (" GROUP BY comp_name, comp_id, comp_address1, comp_address2, comp_city, comp_state, comp_zip_code, comp_country")
      HttpContext.Current.Session.Item("Selection_Listing_Order") = (" ORDER BY ac_count DESC, comp_name asc")

      sQuery.Append(HttpContext.Current.Session.Item("Selection_Listing_Group"))
      sQuery.Append(HttpContext.Current.Session.Item("Selection_Listing_Order"))
 
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_charter_companies(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = clientConnectString
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
        aError = "Error in get_charter_companies load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_charter_companies(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

  Public Sub views_display_charter_companies(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String)

    Dim htmlOut As New StringBuilder
    Dim results_table As New DataTable
    Dim toggleRowColor As Boolean = False

    Dim sTitle As String = ""

    Dim sCharterCertHtml As String = ""

    Try

      results_table = get_charter_companies(searchCriteria)

      If searchCriteria.ViewCriteriaCompanyID > 0 Then
        sTitle = "OPERATOR&nbsp;INFO"
        sTitle += " : " + commonEvo.get_company_name_fromID(searchCriteria.ViewCriteriaCompanyID, 0, False, True, "")
      ElseIf searchCriteria.ViewCriteriaAmodID > -1 And searchCriteria.ViewCriteriaCompanyID = 0 Then

        sTitle = "TOP OPERATORS"

        If searchCriteria.ViewCriteriaAmodID > -1 Then
          sTitle += " of " + commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaAmodID, False, "")
        ElseIf searchCriteria.ViewCriteriaMakeAmodID > -1 Then
          sTitle += " of " + commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaMakeAmodID, True, "")
        End If

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaCity.Trim) Then
          sTitle += " in " + searchCriteria.ViewCriteriaCity.Trim
        ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaCountry.Trim) Then
          sTitle += " in " + searchCriteria.ViewCriteriaCountry.Trim
        End If

      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaCountry.Trim) Or Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaCity.Trim) Then

        sTitle = ""

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAirframeTypeStr.Trim) Then
          If searchCriteria.ViewCriteriaAirframeTypeStr.ToUpper.Contains("F") Then
            sTitle += "Fixed Airframe - "
          ElseIf searchCriteria.ViewCriteriaAirframeTypeStr.ToUpper.Contains("R") Then
            sTitle += "Rotary Airframe - "
          End If
        End If

        sTitle += "TOP OPERATORS"

        If searchCriteria.ViewCriteriaAmodID > -1 Then
          sTitle += " of " + commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaAmodID, False, "")
        ElseIf searchCriteria.ViewCriteriaMakeAmodID > -1 Then
          sTitle += " of " + commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaMakeAmodID, True, "")
        End If

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaCity.Trim) Then
          sTitle += " in " + searchCriteria.ViewCriteriaCity.Trim
        ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaCountry.Trim) Then
          sTitle += " in " + searchCriteria.ViewCriteriaCountry.Trim
        End If

      Else

        sTitle = ""

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAirframeTypeStr.Trim) Then
          If searchCriteria.ViewCriteriaAirframeTypeStr.ToUpper.Contains("F") Then
            sTitle += "Fixed Airframe - "
          ElseIf searchCriteria.ViewCriteriaAirframeTypeStr.ToUpper.Contains("R") Then
            sTitle += "Rotary Airframe - "
          End If
        End If

        sTitle += "TOP OPERATORS"

      End If

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 25 Then
          If searchCriteria.ViewCriteriaAmodID = -1 Then
            htmlOut.Append("<div valign=""top"" style=""height:1000px; overflow-y: auto;""><p>")
          Else
            htmlOut.Append("<div valign=""top"" style=""height:650px; overflow-y: auto;""><p>")
          End If
        End If

        htmlOut.Append("<table id=""charterCompaniesDataTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"" border=""1"" class=""module"">")

                If HttpContext.Current.Session.Item("localUser").crmDemoUserFlag = True Then  ' ADDED IN MSW - 3/26/20
                    htmlOut.Append("<tr><td align=""center"" valign=""middle"" class=""header""" + IIf(searchCriteria.ViewCriteriaCompanyID = 0, " colspan=""4""", " colspan=""2""") + ">" + sTitle + IIf(searchCriteria.ViewCriteriaCompanyID = 0, "<em>(" + results_table.Rows.Count.ToString + ") by number of aircraft</em>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;", "") + "</td></tr>")
                Else
                    htmlOut.Append("<tr><td align=""center"" valign=""middle"" class=""header""" + IIf(searchCriteria.ViewCriteriaCompanyID = 0, " colspan=""4""", " colspan=""2""") + ">" + sTitle + IIf(searchCriteria.ViewCriteriaCompanyID = 0, "<em>(" + results_table.Rows.Count.ToString + ") by number of aircraft</em>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a class=""text_underline cursor"" title='View/Export' ' onclick=""javascript:load('WebSource.aspx?viewType=dynamic&display=table&PageTitle=Top Operators','','scrollbars=yes,menubar=no,height=700,width=1250,resizable=yes,toolbar=no,location=no,status=no');"" ><font color='white'>VIEW IN GRID</font></a>", "") + "</td></tr>")
                End If


                If searchCriteria.ViewCriteriaCompanyID > 0 Then   ' got rid of the clear - so if u pick a operator in the MMS you can clear it 
                        htmlOut.Append("<tr><td align=""left"" valign=""middle"" class=""seperator""" + IIf(searchCriteria.ViewCriteriaCompanyID = 0, " colspan=""4""", " colspan=""2""") + ">")
                        htmlOut.Append("<a href=""View_Template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName.Trim + "&viewCompany=0&viewCity=" + HttpContext.Current.Server.UrlEncode(searchCriteria.ViewCriteriaCity.Trim) + "&viewCountry=" + HttpContext.Current.Server.UrlEncode(searchCriteria.ViewCriteriaCountry.Trim) + "&viewAirframe=" + searchCriteria.ViewCriteriaAirframeTypeStr.Trim + "&amod_id=" + searchCriteria.ViewCriteriaAmodID.ToString + """ title=""Clear Charter Operator"">Clear Operator</a>")
                        htmlOut.Append("</strong></td></tr>")
                    End If

                    If searchCriteria.ViewCriteriaCompanyID > 0 Then
                        htmlOut.Append("<tr><td align=""left"" valign=""middle"" class=""seperator"" style=""padding-left:3px;"" width=""80%""><strong>Name</strong></td>")
                        htmlOut.Append("<td align=""right"" valign=""middle"" class=""seperator"" style=""padding-right:5px;"" width=""20%""><strong>Aircraft</strong></td></tr>")
                    Else
                        htmlOut.Append("<tr><td align=""left"" valign=""middle"" class=""seperator"" style=""padding-left:3px;"" colspan=""2""><strong>Operator&nbsp;Name</strong></td>")
                        htmlOut.Append("<td align=""left"" valign=""middle"" class=""seperator""><strong>Cert</strong></td>")
                        htmlOut.Append("<td align=""right"" valign=""middle"" class=""seperator"" style=""padding-right:5px;""><strong>Count</strong></td></tr>")
                    End If

                    If results_table.Rows.Count > 0 Then

                        For Each r As DataRow In results_table.Rows

                            If searchCriteria.ViewCriteriaCompanyID = 0 Then

                                If CLng(r.Item("ac_count").ToString) > 0 Then

                                    If Not toggleRowColor Then
                                        htmlOut.Append("<tr class=""alt_row"">")
                                        toggleRowColor = True
                                    Else
                                        htmlOut.Append("<tr bgcolor=""white"">")
                                        toggleRowColor = False
                                    End If

                                    htmlOut.Append("<td align=""left"" valign=""middle"" class=""seperator"" width=""5%""><img src=""images/ch_red.jpg"" class=""bullet"" alt=""compid : " + r.Item("comp_id").ToString + """ />&nbsp;&nbsp;</td>")
                                    htmlOut.Append("<td align=""left"" valign=""middle"" class=""seperator"">")

                                    If searchCriteria.ViewCriteriaCompanyID > 0 Then
                                        htmlOut.Append("<a class=""underline cursor"" onclick=""javascript:load('DisplayCompanyDetail.aspx?compid=" + r.Item("comp_id").ToString + "&journid=0','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');"" title=""Display Company Details"">")
                                    Else
                                        htmlOut.Append("<a href=""View_Template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName.Trim + "&viewCompany=" + r.Item("comp_id").ToString + "" & IIf(searchCriteria.ViewID = 1, "&activetab=8", "") & "&viewCity=" + HttpContext.Current.Server.UrlEncode(searchCriteria.ViewCriteriaCity.Trim) + "&viewCountry=" + HttpContext.Current.Server.UrlEncode(searchCriteria.ViewCriteriaCountry.Trim) + "&viewAirframe=" + searchCriteria.ViewCriteriaAirframeTypeStr.Trim + "&amod_id=" + searchCriteria.ViewCriteriaAmodID.ToString + """ title=""Display Charter View by this Operator"">")
                                    End If

                                    htmlOut.Append(Replace(r.Item("comp_name").ToString, " ", "&nbsp;") + "</a><br />")

                                    If Not IsDBNull(r("comp_address1")) Then
                                        If Not String.IsNullOrEmpty(r.Item("comp_address1").ToString.Trim) Then
                                            htmlOut.Append(r.Item("comp_address1").ToString.Trim + "<br />")
                                        End If
                                    End If

                                    If Not IsDBNull(r("comp_address2")) Then
                                        If Not String.IsNullOrEmpty(r.Item("comp_address2").ToString.Trim) Then
                                            htmlOut.Append(r.Item("comp_address2").ToString.Trim + "<br />")
                                        End If
                                    End If

                                    htmlOut.Append(r.Item("comp_city").ToString.Trim)

                                    If Not IsDBNull(r("comp_state")) Then
                                        If Not String.IsNullOrEmpty(r.Item("comp_state").ToString.Trim) Then
                                            htmlOut.Append(" ," + r.Item("comp_state").ToString.Trim)
                                        End If
                                    End If

                                    htmlOut.Append(" " + r.Item("comp_zip_code").ToString.Trim)
                                    htmlOut.Append(" " + r.Item("comp_country").ToString.Trim)


                                    htmlOut.Append("</td>")

                                    htmlOut.Append("<td align=""left"" valign=""middle"" class=""seperator"">")

                                    If CLng(r.Item("certcount").ToString) > 0 Then

                                        searchCriteria.ViewTempCompanyID = CLng(r.Item("comp_id").ToString)

                                        display_charter_certification_images(searchCriteria, sCharterCertHtml)
                                        htmlOut.Append(sCharterCertHtml)

                                        searchCriteria.ViewTempCompanyID = 0

                                    End If

                                    htmlOut.Append("</td>")

                                    htmlOut.Append("<td align=""right"" valign=""middle"" class=""seperator"" width=""10%"">" + r.Item("ac_count").ToString + "</td></tr>")

                                End If

                            Else

                                htmlOut.Append("<tr bgcolor=""white""><td align=""left"" valign=""middle"" class=""seperator"" width=""80%"">")
                                htmlOut.Append(commonEvo.get_company_info_fromID(searchCriteria.ViewCriteriaCompanyID, 0, True, True, "", ""))
                                htmlOut.Append("</td><td align=""right"" valign=""middle"" class=""seperator"" width=""20%"">" + r.Item("ac_count").ToString)
                                htmlOut.Append("</td></tr>")

                            End If

                        Next

                    Else
                        htmlOut.Append("<tr><td align=""left"" valign=""middle"" class=""border_bottom_right""" + IIf(searchCriteria.ViewCriteriaCompanyID = 0, " colspan=""4""", " colspan=""2""") + "><br/>No data matches for your search criteria " + sTitle.Trim + "</td></tr>")
                    End If

                Else
                    htmlOut.Append("<tr><td align=""left"" valign=""middle"" class=""border_bottom_right""" + IIf(searchCriteria.ViewCriteriaCompanyID = 0, " colspan=""4""", " colspan=""2""") + "><br/>No data matches for your search criteria " + sTitle.Trim + "</td></tr>")
      End If

      htmlOut.Append("</table>")

      If results_table.Rows.Count > 15 Then
        htmlOut.Append("</p></div>")
      End If

    Catch ex As Exception

      aError = "Error in views_display_charter_companies(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

    Finally

    End Try

    'return resulting html string
    out_htmlString = htmlOut.ToString
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Function get_charter_airframeType(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append(" SELECT DISTINCT amod_airframe_type_code, count(distinct ac_id) AS ac_count")
      sQuery.Append(" FROM aircraft_summary WITH(NOLOCK)")

      sQuery.Append(" WHERE ac_lifecycle_stage = 3 AND (cref_contact_type IN ('94','33') OR cref_business_type in ('CH'))")

      If searchCriteria.ViewCriteriaCompanyID > 0 Then
        sQuery.Append(Constants.cAndClause + "comp_id = " + searchCriteria.ViewCriteriaCompanyID.ToString)
      End If

      If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaCountry.Trim) Then
        sQuery.Append(Constants.cAndClause + "comp_country <> '' AND lower(comp_country) = '" + Replace(searchCriteria.ViewCriteriaCountry.Trim, Constants.cSingleQuote, Constants.cDoubleSingleQuote).ToLower + "'")
      End If

      If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaCity.Trim) Then
        sQuery.Append(Constants.cAndClause + "comp_city <> '' AND lower(comp_city) = '" + Replace(searchCriteria.ViewCriteriaCity.Trim, Constants.cSingleQuote, Constants.cDoubleSingleQuote).ToLower + "'")
      End If

      If searchCriteria.ViewCriteriaAmodID > -1 Then
        sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
        sQuery.Append(Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
      End If

      Select Case CInt(searchCriteria.ViewCriteriaAirframeType)
        Case Constants.VIEW_EXECUTIVE
          sQuery.Append(Constants.cAndClause + "(amod_customer_flag = 'Y' AND amod_airframe_type_code='F' AND amod_type_code = 'E')")
        Case Constants.VIEW_JETS
          sQuery.Append(Constants.cAndClause + "(amod_customer_flag = 'Y' AND amod_airframe_type_code='F' AND amod_type_code = 'J')")
        Case Constants.VIEW_TURBOPROPS
          sQuery.Append(Constants.cAndClause + "(amod_customer_flag = 'Y' AND amod_airframe_type_code='F' AND amod_type_code = 'T')")
        Case Constants.VIEW_PISTONS
          sQuery.Append(Constants.cAndClause + "(amod_customer_flag = 'Y' AND amod_airframe_type_code='F' AND amod_type_code = 'P')")
        Case Constants.VIEW_HELICOPTERS
          sQuery.Append(Constants.cAndClause + "(amod_customer_flag = 'Y' AND amod_airframe_type_code='R' AND amod_type_code in ('T','P'))")
      End Select

      If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAirframeTypeStr.Trim) Then
        sQuery.Append(Constants.cAndClause + "amod_airframe_type_code = '" + searchCriteria.ViewCriteriaAirframeTypeStr.Trim + "'")
      End If

      If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
        sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))
      Else
        sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True))
      End If

      sQuery.Append(" GROUP BY amod_airframe_type_code")
      sQuery.Append(" ORDER BY ac_count, amod_airframe_type_code ASC")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_charter_airframeType(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = clientConnectString
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
        aError = "Error in get_charter_airframeType load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_charter_airframeType(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

  Public Sub views_display_charter_airframeType(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String)

    Dim total_Aircraft As Integer = 0

    Dim htmlOut As New StringBuilder
    Dim results_table As New DataTable
    Dim toggleRowColor As Boolean = False

    Dim sTmpTitle As String = ""
    Dim sTitle As String = ""

    Try

      results_table = get_charter_airframeType(searchCriteria)

      If Not IsNothing(results_table) Then
        If results_table.Rows.Count > 0 Then
          For Each r As DataRow In results_table.Rows
            total_Aircraft += CInt(r.Item("ac_count").ToString)
          Next
        End If
      End If

      sTitle = "AIRCRAFT"

      If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaCity.Trim) Then
        sTitle += " in " + searchCriteria.ViewCriteriaCity.Trim
      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaCountry.Trim) Then
        sTitle += " in " + searchCriteria.ViewCriteriaCountry.Trim
      End If

      htmlOut.Append("<table id=""charterAirframeDataTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"" border=""0"" class=""module"">")
      htmlOut.Append("<tr><td align=""center"" valign=""middle"" class=""header"" colspan=""2"">" + sTitle + "&nbsp;<em>(" + FormatNumber(total_Aircraft, 0) + ") by airframe</em></td></tr>")

      If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAirframeTypeStr.Trim) And searchCriteria.ViewID > 2 Then
        htmlOut.Append("<tr><td align=""left"" valign=""middle"" class=""seperator"" colspan=""2"">")
        htmlOut.Append("<a href=""View_Template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName.Trim + "&viewCompany=" + searchCriteria.ViewCriteriaCompanyID.ToString + "&viewCity=" + HttpContext.Current.Server.UrlEncode(searchCriteria.ViewCriteriaCity.Trim) + "&viewCountry=" + HttpContext.Current.Server.UrlEncode(searchCriteria.ViewCriteriaCountry.Trim) + "&viewAirframe=&amod_id=-1"" title=""Clears Charter Airframe and Model"">Clear Airframe</a>")
        htmlOut.Append("</strong></td></tr>")
      End If

      htmlOut.Append("<tr><td align=""left"" valign=""middle"" class=""seperator"" style=""padding-left:3px;"" width=""80%""><strong>Airframe</strong></td>")
      htmlOut.Append("<td align=""right"" valign=""middle"" class=""seperator"" style=""padding-right:5px;"" width=""20%""><strong>Count</strong></td></tr>")

      If Not IsNothing(results_table) Then
        If results_table.Rows.Count > 0 Then
          For Each r As DataRow In results_table.Rows

            If Not toggleRowColor Then
              htmlOut.Append("<tr class='alt_row'>")
              toggleRowColor = True
            Else
              htmlOut.Append("<tr bgcolor='white'>")
              toggleRowColor = False
            End If

            If Not IsDBNull(r.Item("amod_airframe_type_code")) Then
              If Not String.IsNullOrEmpty(r.Item("amod_airframe_type_code").ToString.Trim) Then

                If r.Item("amod_airframe_type_code").ToString.ToUpper.Contains("F") Then
                  htmlOut.Append("<td align=""left"" valign=""middle"" class=""seperator"" width=""80%"">")
                  htmlOut.Append("<a href=""View_Template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName.Trim + "&viewCompany=" + searchCriteria.ViewCriteriaCompanyID.ToString + "&viewCity=" + HttpContext.Current.Server.UrlEncode(searchCriteria.ViewCriteriaCity.Trim) + "&viewCountry=" + HttpContext.Current.Server.UrlEncode(searchCriteria.ViewCriteriaCountry.Trim) + "&viewAirframe=F&amod_id=-1"" title=""Display Charter View by Fixed Wing airframes"">Fixed Wing</a>")
                  htmlOut.Append("</td>")
                Else
                  htmlOut.Append("<td align=""left"" valign=""middle"" class=""seperator"" width=""80%"">")
                  htmlOut.Append("<a href=""View_Template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName.Trim + "&viewCompany=" + searchCriteria.ViewCriteriaCompanyID.ToString + "&viewCity=" + HttpContext.Current.Server.UrlEncode(searchCriteria.ViewCriteriaCity.Trim) + "&viewCountry=" + HttpContext.Current.Server.UrlEncode(searchCriteria.ViewCriteriaCountry.Trim) + "&viewAirframe=R&amod_id=-1"" title=""Display Charter View by Rotary airframes"">Rotary</a>")
                  htmlOut.Append("</td>")
                End If

                htmlOut.Append("<td align=""right"" valign=""middle"" class=""seperator"" width=""20%"" style=""padding-right:5px;"">" + FormatNumber(r.Item("ac_count").ToString, 0) + "</td></tr>")

              End If
            End If

          Next
        Else
          htmlOut.Append("<tr><td align=""left"" valign=""middle"" class=""border_bottom_right"" colspan=""2""><br/>No data matches for your search criteria</td></tr>")
        End If

      Else
        htmlOut.Append("<tr><td align=""left"" valign=""middle"" class=""border_bottom_right"" colspan=""2""><br/>No data matches for your search criteria</td></tr>")
      End If

      htmlOut.Append("</table>")

    Catch ex As Exception

      aError = "Error in views_display_charter_airframeType(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

    Finally

    End Try

    'return resulting html string
    out_htmlString = htmlOut.ToString
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Function get_charter_model_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      If searchCriteria.ViewCriteriaCompanyID > 0 Or Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaCountry.Trim) Or Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaCity.Trim) Then
        sQuery.Append("SELECT DISTINCT amod_id, amod_airframe_type_code, amod_type_code, amod_make_name, amod_model_name, count(distinct ac_id) AS ac_count")
      Else
        sQuery.Append("SELECT DISTINCT TOP 50 amod_id, amod_airframe_type_code, amod_type_code, amod_make_name, amod_model_name, count(distinct ac_id) AS ac_count")
      End If

      sQuery.Append(" FROM aircraft_summary WITH(NOLOCK)")
      sQuery.Append(" WHERE ac_lifecycle_stage = 3 AND (cref_contact_type IN ('94','33') OR cref_business_type in ('CH'))")

      If searchCriteria.ViewCriteriaCompanyID > 0 Then
        sQuery.Append(Constants.cAndClause + "comp_id = " + searchCriteria.ViewCriteriaCompanyID.ToString)
      End If

      If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaCountry.Trim) Then
        sQuery.Append(Constants.cAndClause + "comp_country <> '' AND lower(comp_country) = '" + Replace(searchCriteria.ViewCriteriaCountry.Trim, Constants.cSingleQuote, Constants.cDoubleSingleQuote).ToLower + "'")
      End If

      If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaCity.Trim) Then
        sQuery.Append(Constants.cAndClause + "comp_city <> '' AND lower(comp_city) = '" + Replace(searchCriteria.ViewCriteriaCity.Trim, Constants.cSingleQuote, Constants.cDoubleSingleQuote).ToLower + "'")
      End If

      If searchCriteria.ViewCriteriaAmodID > -1 Then
        sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
        sQuery.Append(Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
      End If

      Select Case CInt(searchCriteria.ViewCriteriaAirframeType)
        Case Constants.VIEW_EXECUTIVE
          sQuery.Append(Constants.cAndClause + "(amod_customer_flag = 'Y' AND amod_airframe_type_code='F' AND amod_type_code = 'E')")
        Case Constants.VIEW_JETS
          sQuery.Append(Constants.cAndClause + "(amod_customer_flag = 'Y' AND amod_airframe_type_code='F' AND amod_type_code = 'J')")
        Case Constants.VIEW_TURBOPROPS
          sQuery.Append(Constants.cAndClause + "(amod_customer_flag = 'Y' AND amod_airframe_type_code='F' AND amod_type_code = 'T')")
        Case Constants.VIEW_PISTONS
          sQuery.Append(Constants.cAndClause + "(amod_customer_flag = 'Y' AND amod_airframe_type_code='F' AND amod_type_code = 'P')")
        Case Constants.VIEW_HELICOPTERS
          sQuery.Append(Constants.cAndClause + "(amod_customer_flag = 'Y' AND amod_airframe_type_code='R' AND amod_type_code in ('T','P'))")
      End Select

      If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAirframeTypeStr.Trim) Then
        sQuery.Append(Constants.cAndClause + "amod_airframe_type_code = '" + searchCriteria.ViewCriteriaAirframeTypeStr.Trim + "'")
      End If

      If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
        sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))
      Else
        sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True))
      End If

      sQuery.Append(" GROUP BY amod_airframe_type_code, amod_type_code, amod_make_name, amod_model_name, amod_id")
      sQuery.Append(" ORDER BY ac_count DESC")


      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_charter_model_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = clientConnectString
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
        aError = "Error in get_charter_model_info load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_charter_model_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

  Public Sub views_display_charter_model(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String)

    Dim htmlOut As New StringBuilder
    Dim results_table As New DataTable
    Dim toggleRowColor As Boolean = False

    Dim sTitle As String = ""

    Try

      results_table = get_charter_model_info(searchCriteria)

      If searchCriteria.ViewCriteriaCompanyID > 0 Then
        sTitle = commonEvo.get_company_name_fromID(searchCriteria.ViewCriteriaCompanyID, 0, False, True, "")
        sTitle += " : MODELS"
      ElseIf searchCriteria.ViewCriteriaAmodID > -1 And searchCriteria.ViewCriteriaCompanyID = 0 Then

        sTitle = "TOP MODELS"

        If searchCriteria.ViewCriteriaAmodID > -1 Then
          sTitle += " of " + commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaAmodID, False, "")
        ElseIf searchCriteria.ViewCriteriaMakeAmodID > -1 Then
          sTitle += " of " + commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaMakeAmodID, True, "")
        End If

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaCity.Trim) Then
          sTitle += " in " + searchCriteria.ViewCriteriaCity.Trim
        ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaCountry.Trim) Then
          sTitle += " in " + searchCriteria.ViewCriteriaCountry.Trim
        End If

      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaCountry.Trim) Or Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaCity.Trim) Then

        sTitle = ""

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAirframeTypeStr.Trim) Then
          If searchCriteria.ViewCriteriaAirframeTypeStr.ToUpper.Contains("F") Then
            sTitle += "Fixed Airframe - "
          ElseIf searchCriteria.ViewCriteriaAirframeTypeStr.ToUpper.Contains("R") Then
            sTitle += "Rotary Airframe - "
          End If
        End If

        sTitle += "TOP MODELS"

        If searchCriteria.ViewCriteriaAmodID > -1 Then
          sTitle += " of " + commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaAmodID, False, "")
        ElseIf searchCriteria.ViewCriteriaMakeAmodID > -1 Then
          sTitle += " of " + commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaMakeAmodID, True, "")
        End If

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaCity.Trim) Then
          sTitle += " in " + searchCriteria.ViewCriteriaCity.Trim
        ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaCountry.Trim) Then
          sTitle += " in " + searchCriteria.ViewCriteriaCountry.Trim
        End If

      Else

        sTitle = ""

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAirframeTypeStr.Trim) Then
          If searchCriteria.ViewCriteriaAirframeTypeStr.ToUpper.Contains("F") Then
            sTitle += "Fixed Airframe - "
          ElseIf searchCriteria.ViewCriteriaAirframeTypeStr.ToUpper.Contains("R") Then
            sTitle += "Rotary Airframe - "
          End If
        End If

        sTitle += "TOP MODELS"

      End If

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 25 Then
          htmlOut.Append("<div valign=""top"" style=""height:1000px; overflow-y: auto;""><p>")
        End If

        htmlOut.Append("<table id=""charterModelDataTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"" border=""0"" class=""module"">")
        htmlOut.Append("<tr><td align=""center"" valign=""middle"" class=""header"" colspan=""2"">" + sTitle + "&nbsp;<em>(" + results_table.Rows.Count.ToString + ") by number of aircraft</em></td></tr>")

        If searchCriteria.ViewCriteriaAmodID > -1 And searchCriteria.ViewID > 2 Then
          htmlOut.Append("<tr><td align=""left"" valign=""middle"" class=""seperator"" colspan=""2"">")
          htmlOut.Append("<a href=""View_Template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName.Trim + "&viewCompany=" + searchCriteria.ViewCriteriaCompanyID.ToString + "&viewCity=" + HttpContext.Current.Server.UrlEncode(searchCriteria.ViewCriteriaCity.Trim) + "&viewCountry=" + HttpContext.Current.Server.UrlEncode(searchCriteria.ViewCriteriaCountry.Trim) + "&viewAirframe=&amod_id=-1"" title=""Clears Charter Model and Airframe"">Clear Model</a>")
          htmlOut.Append("</strong></td></tr>")
        End If

        htmlOut.Append("<tr><td align=""left"" valign=""middle"" class=""seperator"" style=""padding-left:3px;"" width=""80%""><strong>Model</strong></td>")
        htmlOut.Append("<td align=""left"" valign=""middle"" class=""seperator"" style=""padding-right:5px;"" width=""20%""><strong>Count</strong></td></tr>")

        If results_table.Rows.Count > 0 Then

          For Each r As DataRow In results_table.Rows


            If Not IsDBNull(r.Item("amod_id")) Then
              If Not String.IsNullOrEmpty(r.Item("amod_id").ToString.Trim) Then

                If Not toggleRowColor Then
                  htmlOut.Append("<tr class='alt_row'>")
                  toggleRowColor = True
                Else
                  htmlOut.Append("<tr bgcolor='white'>")
                  toggleRowColor = False
                End If

                htmlOut.Append("<td align=""left"" valign=""middle"" class=""seperator"" width=""80%"">")
                htmlOut.Append("<a href=""View_Template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName.Trim + "&viewCompany=" + searchCriteria.ViewCriteriaCompanyID.ToString + "&viewCity=" + HttpContext.Current.Server.UrlEncode(searchCriteria.ViewCriteriaCity.Trim) + "&viewCountry=" + HttpContext.Current.Server.UrlEncode(searchCriteria.ViewCriteriaCountry.Trim) + "&viewAirframe=" + searchCriteria.ViewCriteriaAirframeTypeStr.Trim + "&amod_id=" + r.Item("amod_id").ToString + """ title=""Display Charter View by This Model"">" + r.Item("amod_make_name").ToString + "&nbsp;/&nbsp;" + r.Item("amod_model_name").ToString + "</a>")
                htmlOut.Append("</td><td align=""right"" valign=""middle"" class=""seperator"" width=""20%"" style=""padding-right:5px;"">" + FormatNumber(r.Item("ac_count").ToString, 0) + "</td></tr>")

              End If
            End If

          Next

        Else
          htmlOut.Append("<tr><td align=""left"" valign=""middle"" class=""border_bottom_right"" colspan=""2""><br/>No data matches for your search criteria " + sTitle.Trim + "</td></tr>")
        End If

      Else
        htmlOut.Append("<tr><td align=""left"" valign=""middle"" class=""border_bottom_right"" colspan=""2""><br/>No data matches for your search criteria " + sTitle.Trim + "</td></tr>")
      End If

      htmlOut.Append("</table>")

      If results_table.Rows.Count > 25 Then
        htmlOut.Append("</p></div>")
      End If

    Catch ex As Exception

      aError = "Error in views_display_charter_model(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

    Finally

    End Try

    'return resulting html string
    out_htmlString = htmlOut.ToString
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Function get_charter_fleet_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT DISTINCT TOP 250 amod_airframe_type_code, amod_type_code, amod_make_name, amod_model_name, ac_ser_no_full, ac_reg_no, ac_mfr_year, ac_id")

      sQuery.Append(" FROM aircraft_summary WITH(NOLOCK)")
      sQuery.Append(" WHERE ac_lifecycle_stage = 3 AND (cref_contact_type IN ('94','33') OR cref_business_type in ('CH'))")

      If searchCriteria.ViewCriteriaCompanyID > 0 Then
        sQuery.Append(Constants.cAndClause + "comp_id = " + searchCriteria.ViewCriteriaCompanyID.ToString)
      End If

      If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaCountry.Trim) Then
        sQuery.Append(Constants.cAndClause + "comp_country <> '' AND lower(comp_country) = '" + Replace(searchCriteria.ViewCriteriaCountry.Trim, Constants.cSingleQuote, Constants.cDoubleSingleQuote).ToLower + "'")
      End If

      If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaCity.Trim) Then
        sQuery.Append(Constants.cAndClause + "comp_city <> '' AND lower(comp_city) = '" + Replace(searchCriteria.ViewCriteriaCity.Trim, Constants.cSingleQuote, Constants.cDoubleSingleQuote).ToLower + "'")
      End If

      If searchCriteria.ViewCriteriaAmodID > -1 Then
        sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
        sQuery.Append(Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
      End If

      If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAirframeTypeStr.Trim) Then
        sQuery.Append(Constants.cAndClause + "amod_airframe_type_code = '" + searchCriteria.ViewCriteriaAirframeTypeStr.Trim + "'")
      End If

      Select Case CInt(searchCriteria.ViewCriteriaAirframeType)
        Case Constants.VIEW_EXECUTIVE
          sQuery.Append(Constants.cAndClause + "(amod_customer_flag = 'Y' AND amod_airframe_type_code='F' AND amod_type_code = 'E')")
        Case Constants.VIEW_JETS
          sQuery.Append(Constants.cAndClause + "(amod_customer_flag = 'Y' AND amod_airframe_type_code='F' AND amod_type_code = 'J')")
        Case Constants.VIEW_TURBOPROPS
          sQuery.Append(Constants.cAndClause + "(amod_customer_flag = 'Y' AND amod_airframe_type_code='F' AND amod_type_code = 'T')")
        Case Constants.VIEW_PISTONS
          sQuery.Append(Constants.cAndClause + "(amod_customer_flag = 'Y' AND amod_airframe_type_code='F' AND amod_type_code = 'P')")
        Case Constants.VIEW_HELICOPTERS
          sQuery.Append(Constants.cAndClause + "(amod_customer_flag = 'Y' AND amod_airframe_type_code='R' AND amod_type_code in ('T','P'))")
      End Select



      If searchCriteria.ViewCriteriaAFTTStart > 0 Then
        sQuery.Append(Constants.cAndClause + " ((ac_airframe_tot_hrs >= " & searchCriteria.ViewCriteriaAFTTStart & ") or (ac_airframe_tot_hrs IS NULL))")
      End If

      If searchCriteria.ViewCriteriaAFTTEnd > 0 Then
        sQuery.Append(Constants.cAndClause + " ((ac_airframe_tot_hrs <= " & searchCriteria.ViewCriteriaAFTTEnd & ") or (ac_airframe_tot_hrs IS NULL))")
      End If


      If searchCriteria.ViewCriteriaYearStart > 0 Then
        sQuery.Append(Constants.cAndClause + " ac_mfr_year >= " & searchCriteria.ViewCriteriaYearStart)
      End If


      If searchCriteria.ViewCriteriaYearEnd > 0 Then
        sQuery.Append(Constants.cAndClause + " ac_mfr_year <=  " & searchCriteria.ViewCriteriaYearEnd)
      End If



      If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
        sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))
      Else
        sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True))
      End If

      sQuery.Append(" GROUP BY amod_airframe_type_code, amod_type_code, amod_make_name, amod_model_name, ac_ser_no_full, ac_reg_no, ac_mfr_year, ac_id")
      sQuery.Append(" ORDER BY amod_airframe_type_code, amod_type_code, amod_make_name, amod_model_name, ac_ser_no_full")


      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_charter_fleet_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = clientConnectString
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
        aError = "Error in get_charter_fleet_info load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_charter_fleet_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

  Public Sub views_display_charter_fleet(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String)

    Dim htmlOut As New StringBuilder
    Dim results_table As New DataTable
    Dim toggleRowColor As Boolean = False

    Dim sAssociatedCompanyHtml As String = ""

    Dim sTitle As String = ""

    Try

      results_table = get_charter_fleet_info(searchCriteria)

      If searchCriteria.ViewCriteriaCompanyID > 0 Then
        sTitle = commonEvo.get_company_name_fromID(searchCriteria.ViewCriteriaCompanyID, 0, False, True, "")
        sTitle += " : FLEET"
      ElseIf searchCriteria.ViewCriteriaAmodID > -1 And searchCriteria.ViewCriteriaCompanyID = 0 Then

        sTitle = "FLEET"

        If searchCriteria.ViewCriteriaAmodID > -1 Then
          sTitle += " of " + commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaAmodID, False, "")
        ElseIf searchCriteria.ViewCriteriaMakeAmodID > -1 Then
          sTitle += " of " + commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaMakeAmodID, True, "")
        End If

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaCity.Trim) Then
          sTitle += " in " + searchCriteria.ViewCriteriaCity.Trim
        ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaCountry.Trim) Then
          sTitle += " in " + searchCriteria.ViewCriteriaCountry.Trim
        End If

      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaCountry.Trim) Or Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaCity.Trim) Then

        sTitle = ""

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAirframeTypeStr.Trim) Then
          If searchCriteria.ViewCriteriaAirframeTypeStr.ToUpper.Contains("F") Then
            sTitle += "Fixed Airframe - "
          ElseIf searchCriteria.ViewCriteriaAirframeTypeStr.ToUpper.Contains("R") Then
            sTitle += "Rotary Airframe - "
          End If
        End If

        sTitle += "FLEET"

        If searchCriteria.ViewCriteriaAmodID > -1 Then
          sTitle += " of " + commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaAmodID, False, "")
        ElseIf searchCriteria.ViewCriteriaMakeAmodID > -1 Then
          sTitle += " of " + commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaMakeAmodID, True, "")
        End If

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaCity.Trim) Then
          sTitle += " in " + searchCriteria.ViewCriteriaCity.Trim
        ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaCountry.Trim) Then
          sTitle += " in " + searchCriteria.ViewCriteriaCountry.Trim
        End If

      Else

        sTitle = ""

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAirframeTypeStr.Trim) Then
          If searchCriteria.ViewCriteriaAirframeTypeStr.ToUpper.Contains("F") Then
            sTitle += "Fixed Airframe - "
          ElseIf searchCriteria.ViewCriteriaAirframeTypeStr.ToUpper.Contains("R") Then
            sTitle += "Rotary Airframe - "
          End If
        End If

        sTitle += "FLEET"

      End If

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 25 Then
          If searchCriteria.ViewCriteriaAmodID = -1 Or searchCriteria.ViewCriteriaCompanyID = 0 Then
            htmlOut.Append("<div valign=""top"" style=""height:1000px; overflow-y: auto;""><p>")
          Else
            htmlOut.Append("<div valign=""top"" style=""height:510px; overflow-y: auto;""><p>")
          End If
        End If

        htmlOut.Append("<table id=""charterFleetDataTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"" border=""0"" class=""module"">")
        htmlOut.Append("<tr><td align=""center"" valign=""middle"" class=""header"" colspan=""2"">" + sTitle + "&nbsp;<em>(" + results_table.Rows.Count.ToString + ")</em></td></tr>")

        If searchCriteria.ViewCriteriaAmodID > -1 And searchCriteria.ViewID > 2 Then
          htmlOut.Append("<tr><td align=""left"" valign=""middle"" class=""seperator"" colspan=""2"">")
          htmlOut.Append("<a href=""View_Template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName.Trim + "&viewCompany=" + searchCriteria.ViewCriteriaCompanyID.ToString + "&viewCity=" + HttpContext.Current.Server.UrlEncode(searchCriteria.ViewCriteriaCity.Trim) + "&viewCountry=" + HttpContext.Current.Server.UrlEncode(searchCriteria.ViewCriteriaCountry.Trim) + "&viewAirframe=&amod_id=-1"" title=""Clears Charter Model and Airframe"">Clear Model</a>")
          htmlOut.Append("</strong></td></tr>")
        End If

        If results_table.Rows.Count > 0 Then

          For Each r As DataRow In results_table.Rows

            If Not IsDBNull(r.Item("ac_id")) Then
              If Not String.IsNullOrEmpty(r.Item("ac_id").ToString.Trim) Then

                If Not toggleRowColor Then
                  htmlOut.Append("<tr class='alt_row'>")
                  toggleRowColor = True
                Else
                  htmlOut.Append("<tr bgcolor='white'>")
                  toggleRowColor = False
                End If

                htmlOut.Append("<td align=""left"" valign=""middle"" class=""seperator"" width=""5%""><img src=""images/ch_red.jpg"" class=""bullet"" alt=""acid : " + r.Item("ac_id").ToString + """ />&nbsp;&nbsp;</td>")

                If searchCriteria.ViewCriteriaAmodID = -1 Then
                  htmlOut.Append("<td align=""left"" valign=""middle"" class=""seperator"">" + r.Item("amod_make_name").ToString + "&nbsp;/&nbsp;" + r.Item("amod_model_name").ToString)
                                    htmlOut.Append(" Serial# <a class=""underline cursor"" onclick=""javascript:load('DisplayAircraftDetail.aspx?acid=" + r.Item("ac_id").ToString + "&jid=0','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');"" title='Display Aircraft Details'>")
                                    htmlOut.Append(r.Item("ac_ser_no_full").ToString + "</a>, Reg# " + r.Item("ac_reg_no").ToString)
                  htmlOut.Append("<br />Year MFR : " + r.Item("ac_mfr_year").ToString)
                Else
                  htmlOut.Append("<td align=""left"" valign=""middle"" class=""seperator"">")
                                    htmlOut.Append(" Serial# <a class=""underline cursor"" onclick=""javascript:load('DisplayAircraftDetail.aspx?acid=" + r.Item("ac_id").ToString + "&jid=0','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');"" title='Display Aircraft Details'>")
                                    htmlOut.Append(r.Item("ac_ser_no_full").ToString + "</a>, Reg# " + r.Item("ac_reg_no").ToString)
                  htmlOut.Append("<br />Year MFR : " + r.Item("ac_mfr_year").ToString)
                End If

                If searchCriteria.ViewCriteriaCompanyID = 0 Then
                  searchCriteria.ViewCriteriaAircraftID = CLng(r.Item("ac_id").ToString)
                  views_display_charter_company_associated_names(searchCriteria, sAssociatedCompanyHtml)
                  htmlOut.Append("<br />" + sAssociatedCompanyHtml.Trim)
                  searchCriteria.ViewCriteriaAircraftID = 0
                End If

                htmlOut.Append("</td></tr>")

              End If

            End If

          Next

        Else
          htmlOut.Append("<tr><td align=""left"" valign=""middle"" class=""border_bottom_right"" colspan=""2""><br/>No data matches for your search criteria " + sTitle.Trim + "</td></tr>")
        End If

      Else
        htmlOut.Append("<tr><td align=""left"" valign=""middle"" class=""border_bottom_right"" colspan=""2""><br/>No data matches for your search criteria " + sTitle.Trim + "</td></tr>")
      End If

      htmlOut.Append("</table>")

      If results_table.Rows.Count > 25 Then
        htmlOut.Append("</p></div>")
      End If

    Catch ex As Exception

      aError = "Error in views_display_charter_fleet(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

    Finally

    End Try

    'return resulting html string
    out_htmlString = htmlOut.ToString
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Function get_charter_certification_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable
    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT DISTINCT ccerttype_id, ccerttype_type, ccerttype_logo_image FROM company_certification WITH(NOLOCK)")
      sQuery.Append(" INNER JOIN company_certification_type WITH(NOLOCK) ON ccert_type_id = ccerttype_id")

      If searchCriteria.ViewTempCompanyID > 0 Then
        sQuery.Append(" WHERE ccert_journ_id = 0 AND ccert_comp_id = " + searchCriteria.ViewTempCompanyID.ToString)
      Else
        sQuery.Append(" WHERE ccert_journ_id = 0 AND ccert_comp_id = " + searchCriteria.ViewCriteriaCompanyID.ToString)
      End If

      sQuery.Append(" AND ccerttype_charter_flag = 'Y' AND ccerttype_logo_image IS NOT NULL AND ccerttype_logo_image <> ''")

      'HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_operator_certification_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = clientConnectString
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
        aError = "Error in get_operator_certification_info load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_operator_certification_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

  Public Sub display_charter_certification_images(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String)

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder

    Dim toggleRowColor As Boolean = False

    Try

      results_table = get_charter_certification_info(searchCriteria)

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          If searchCriteria.ViewCriteriaCompanyID > 0 Then

            htmlOut.Append("<table id=""charterCertificateDataTable"" width=""100%"" cellpadding=""4"" cellspacing=""0"" border=""0"">")
            htmlOut.Append("<tr><td valign='middle' align='center' class='header'>OPERATOR CERTIFICATION(S)</td></tr>")
            htmlOut.Append("<tr><td valign=""middle"" align=""left"">")

            ' line up certification images on the row
            For Each r As DataRow In results_table.Rows
              htmlOut.Append("<img width=""50"" src=""images/" + r.Item("ccerttype_logo_image").ToString.Trim + """ alt=""" + r.Item("ccerttype_type").ToString.Trim + """ title=""" + r.Item("ccerttype_type").ToString.Trim + """ />")
            Next

            htmlOut.Append("</td></tr>")

          Else

            htmlOut.Append("<table id=""charterCertificateDataTable"" width=""100%"" cellpadding=""4"" cellspacing=""0"" border=""0""><tr><td valign=""middle"" align=""center"">")

            For Each r As DataRow In results_table.Rows
              htmlOut.Append("<img width=""25"" src=""images/" + r.Item("ccerttype_logo_image").ToString.Trim + """ alt=""" + r.Item("ccerttype_type").ToString.Trim + """ title=""" + r.Item("ccerttype_type").ToString.Trim + """ />")
            Next

            htmlOut.Append("</td></tr>")

          End If

        Else
          htmlOut.Append("<table id=""charterCertificateDataTable"" width=""100%"" cellpadding=""4"" cellspacing=""0"" border=""0"">")
          htmlOut.Append("<tr><td valign='middle' align='center' class='header'>OPERATOR CERTIFICATION(S)</td></tr>")
          htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""border_bottom_right""><br/>No Certificate Information Available</td></tr>")
        End If
      Else
        htmlOut.Append("<table id=""charterCertificateDataTable"" width=""100%"" cellpadding=""4"" cellspacing=""0"" border=""0"">")
        htmlOut.Append("<tr><td valign='middle' align='center' class='header'>OPERATOR CERTIFICATION(S)</td></tr>")
        htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""border_bottom_right""><br/>No Certificate Information Available</td></tr>")
      End If

      htmlOut.Append("</table>")

    Catch ex As Exception

      aError = "Error in display_charter_certification_images(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

    Finally

    End Try

    out_htmlString = htmlOut.ToString()
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Function get_charter_model_about_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery As New StringBuilder

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT amod_id, amod_number_of_crew, amod_number_of_passengers, amod_field_length, amod_climb_normal_feet, amod_max_range_miles, amod_range_tanks_full, amod_range_seats_full,")
      sQuery.Append(" amod_cruis_speed, amod_number_of_engines, amod_annual_miles, amod_annual_hours, amod_tot_direct_cost, amod_tot_fixed_cost, amod_airframe_type_code,")
      sQuery.Append(" amod_cabinsize_length_feet, amod_cabinsize_height_feet, amod_cabinsize_width_feet, amod_cabinsize_length_inches, amod_cabinsize_height_inches, amod_cabinsize_width_inches, amod_takeoff_ali, amod_takeoff_500")
      sQuery.Append(" FROM aircraft_model WITH(NOLOCK) WHERE amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)

      If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
        sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))
      Else
        sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True))
      End If

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_charter_model_about_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = clientConnectString
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
        aError = "Error in get_charter_model_about_info load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_charter_model_about_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable" + ex.Message

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

  Public Sub views_display_about_charter_model(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByVal isHeliOnlyProduct As Boolean)

    Dim total_Aircraft As Integer = 0

    Dim htmlOut As New StringBuilder
    Dim results_table As New DataTable
    Dim toggleRowColor As Boolean = False

    Dim sTmpTitle As String = ""
    Dim sTitle As String = ""

    Dim nCrew As Integer = 0
    Dim nPassengers As Integer = 0

    Dim nCabinLength As String = "0'&nbsp;0"
    Dim nCabinWidth As String = "0'&nbsp;0"
    Dim nCabinHeight As String = "0'&nbsp;0"

    Dim nFieldlength As Integer = 0
    Dim nRange As Integer = 0

    Dim nAmodTakeoffAli As Integer = 0
    Dim nAmodTakeoff500 As Integer = 0

    Dim nRangeSeatsFull As Integer = 0
    Dim nRangeTanksFull As Integer = 0

    Dim sPieChartHtml As String = ""

    Try

      results_table = get_charter_model_about_info(searchCriteria)

      sTitle = "SPECIFICATIONS&nbsp;"

      If searchCriteria.ViewCriteriaAmodID > -1 Then
        sTitle += " for " + commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaAmodID, False, "")
      ElseIf searchCriteria.ViewCriteriaMakeAmodID > -1 Then
        sTitle += " for " + commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaMakeAmodID, True, "")
      End If

      htmlOut.Append("<table id=""charterAboutModelDataTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"" border=""0"" class=""module"">")
      htmlOut.Append("<tr><td align=""center"" valign=""middle"" class=""header"" colspan=""2"">" + sTitle + "</td></tr>")

      If Not IsNothing(results_table) Then
        If results_table.Rows.Count > 0 Then
          For Each r As DataRow In results_table.Rows

            If Not IsDBNull(r.Item("amod_id")) Then
              If Not String.IsNullOrEmpty(r.Item("amod_id").ToString.Trim) Then

                If Not IsDBNull(r.Item("amod_number_of_crew")) Then
                  If Not String.IsNullOrEmpty(r.Item("amod_number_of_crew").ToString.Trim) Then
                    nCrew = CInt(r.Item("amod_number_of_crew").ToString)
                  End If
                End If

                If Not IsDBNull(r.Item("amod_number_of_passengers")) Then
                  If Not String.IsNullOrEmpty(r.Item("amod_number_of_passengers").ToString.Trim) Then
                    nPassengers = CInt(r.Item("amod_number_of_passengers").ToString)
                  End If
                End If


                If Not IsDBNull(r.Item("amod_field_length")) Then
                  If Not String.IsNullOrEmpty(r.Item("amod_field_length").ToString.Trim) Then
                    nFieldlength = CInt(r.Item("amod_field_length").ToString)
                  End If
                End If

                If Not IsDBNull(r.Item("amod_max_range_miles")) Then
                  If Not String.IsNullOrEmpty(r.Item("amod_max_range_miles").ToString.Trim) Then
                    nRange = CInt(r.Item("amod_max_range_miles").ToString)
                  End If
                End If

                If Not IsDBNull(r.Item("amod_takeoff_ali")) Then
                  If Not String.IsNullOrEmpty(r.Item("amod_takeoff_ali").ToString.Trim) Then
                    nAmodTakeoffAli = CInt(r.Item("amod_takeoff_ali").ToString)
                  End If
                End If

                If Not IsDBNull(r.Item("amod_takeoff_500")) Then
                  If Not String.IsNullOrEmpty(r.Item("amod_takeoff_500").ToString.Trim) Then
                    nAmodTakeoff500 = CInt(r.Item("amod_takeoff_500").ToString)
                  End If
                End If

                If Not IsDBNull(r.Item("amod_range_seats_full")) Then
                  If Not String.IsNullOrEmpty(r.Item("amod_range_seats_full").ToString.Trim) Then
                    nRangeSeatsFull = CInt(r.Item("amod_range_seats_full").ToString)
                  End If
                End If

                If Not IsDBNull(r.Item("amod_range_tanks_full")) Then
                  If Not String.IsNullOrEmpty(r.Item("amod_range_tanks_full").ToString.Trim) Then
                    nRangeTanksFull = CInt(r.Item("amod_range_tanks_full").ToString)
                  End If
                End If

                If Not IsDBNull(r.Item("amod_cabinsize_length_feet")) Then
                  If Not String.IsNullOrEmpty(r.Item("amod_cabinsize_length_feet").ToString.Trim) Then
                    nCabinLength = r.Item("amod_cabinsize_length_feet").ToString + "'"

                    If Not IsDBNull(r.Item("amod_cabinsize_length_inches")) Then
                      If Not String.IsNullOrEmpty(r.Item("amod_cabinsize_length_inches").ToString.Trim) Then
                        nCabinLength += "&nbsp;" + r.Item("amod_cabinsize_length_inches").ToString
                      End If
                    End If

                  End If
                End If

                If Not IsDBNull(r.Item("amod_cabinsize_width_feet")) Then
                  If Not String.IsNullOrEmpty(r.Item("amod_cabinsize_width_feet").ToString.Trim) Then
                    nCabinWidth = r.Item("amod_cabinsize_width_feet").ToString + "'"

                    If Not IsDBNull(r.Item("amod_cabinsize_width_inches")) Then
                      If Not String.IsNullOrEmpty(r.Item("amod_cabinsize_width_inches").ToString.Trim) Then
                        nCabinWidth += "&nbsp;" + r.Item("amod_cabinsize_width_inches").ToString
                      End If
                    End If

                  End If
                End If

                If Not IsDBNull(r.Item("amod_cabinsize_height_feet")) Then
                  If Not String.IsNullOrEmpty(r.Item("amod_cabinsize_height_feet").ToString.Trim) Then
                    nCabinHeight = r.Item("amod_cabinsize_height_feet").ToString + "'"

                    If Not IsDBNull(r.Item("amod_cabinsize_height_inches")) Then
                      If Not String.IsNullOrEmpty(r.Item("amod_cabinsize_height_inches").ToString.Trim) Then
                        nCabinHeight += "&nbsp;" + r.Item("amod_cabinsize_height_inches").ToString
                      End If
                    End If

                  End If
                End If

                htmlOut.Append("<tr class='alt_row'><td valign='top' align='left' class='seperator'>Crew</td>")
                htmlOut.Append("<td valign='top' align='right' class='rightside'>" + FormatNumber(nCrew, 0, True, False, True) + "</td></tr>")
                htmlOut.Append("<tr bgcolor='white'><td valign='top' align='left' class='seperator'>Passengers</td>")
                htmlOut.Append("<td valign='top' align='right' class='rightside'>" + FormatNumber(nPassengers, 0, True, False, True) + "</td></tr>")
                htmlOut.Append("<tr class='alt_row'><td valign='top' align='left' class='seperator'>Cabin&nbsp;Dimensions&nbsp;(ft)<br /><em>(l x w x h)</em></td>")
                htmlOut.Append("<td valign='top' align='right' class='rightside'>")

                If Not String.IsNullOrEmpty(nCabinLength.Trim) Then
                  htmlOut.Append(nCabinLength + "(l)<br />")
                End If

                If Not String.IsNullOrEmpty(nCabinWidth.Trim) Then
                  htmlOut.Append(nCabinWidth + "(w)<br />")
                End If

                If Not String.IsNullOrEmpty(nCabinHeight.Trim) Then
                  htmlOut.Append(nCabinHeight + "(h)")
                End If

                htmlOut.Append("</td></tr>")

                If Not isHeliOnlyProduct And nFieldlength > 0 Then
                  htmlOut.Append("<tr><td valign='top' align='left' class='seperator'>Landing&nbsp;Field&nbsp;Length&nbsp;(ft)</td>")
                  htmlOut.Append("<td valign='top' align='right' class='rightside'>" + FormatNumber(nFieldlength, 0, True, False, True) + "</td></tr>")
                End If

                htmlOut.Append("<tr><td valign='top' align='left' class='seperator'>Takeoff&nbsp;Performance&nbsp;(ft)<br /><em>SL ISA BFL</em></td>")
                htmlOut.Append("<td valign='top' align='right' class='rightside'>" + FormatNumber(nAmodTakeoffAli, 0, True, False, True) + "</td></tr>")
                htmlOut.Append("<tr><td valign='top' align='left' class='seperator'>Takeoff&nbsp;Performance&nbsp;(ft)<br /><em>5000' +25C BLF</em></td>")
                htmlOut.Append("<td valign='top' align='right' class='rightside'>" + FormatNumber(nAmodTakeoff500, 0, True, False, True) + "</td></tr>")

                If Not isHeliOnlyProduct And nRange > 0 Then
                  htmlOut.Append("<tr><td valign='top' align='left' class='seperator'>Range&nbsp;(nm)</td>")
                  htmlOut.Append("<td valign='top' align='right' class='rightside'>" + FormatNumber(nRange, 0, True, False, True) + "</td></tr>")
                Else
                  htmlOut.Append("<tr><td valign='top' align='left' class='seperator'>Range&nbsp;Tanks&nbsp;Full&nbsp;(nm)</td>")
                  htmlOut.Append("<td valign='top' align='right' class='rightside'>" + FormatNumber(nRangeTanksFull, 0, True, False, True) + "</td></tr>")
                  htmlOut.Append("<tr><td valign='top' align='left' class='seperator'>Range&nbsp;Seats&nbsp;Full&nbsp;(nm)</td>")
                  htmlOut.Append("<td valign='top' align='right' class='rightside'>" + FormatNumber(nRangeSeatsFull, 0, True, False, True) + "</td></tr>")
                End If

                If searchCriteria.ViewCriteriaCompanyID > 0 Then
                  htmlOut.Append("<tr><td align=""left"" valign=""top"" colspan=""2"">")

                  views_display_charter_vs_noncharter_piechart(searchCriteria, sPieChartHtml, 1)
                  htmlOut.Append(sPieChartHtml.Trim)

                  htmlOut.Append("</td></tr>")
                End If

              End If
            End If

          Next
        Else
          htmlOut.Append("<tr><td align=""left"" valign=""middle"" class=""border_bottom_right"" colspan=""2""><br/>No data matches for your search criteria</td></tr>")
        End If

      Else
        htmlOut.Append("<tr><td align=""left"" valign=""middle"" class=""border_bottom_right"" colspan=""2""><br/>No data matches for your search criteria</td></tr>")
      End If

      htmlOut.Append("</table>")

    Catch ex As Exception

      aError = "Error in views_display_charter_model_about_info(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

    Finally

    End Try

    'return resulting html string
    out_htmlString = htmlOut.ToString
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Function get_charter_location_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      If String.IsNullOrEmpty(searchCriteria.ViewCriteriaCountry.Trim) Then

        sQuery.Append("SELECT DISTINCT comp_country, count(distinct comp_id) AS comp_count")
        sQuery.Append(" FROM aircraft_summary WITH(NOLOCK)")
        sQuery.Append(" WHERE ac_lifecycle_stage = 3 AND (cref_contact_type IN ('94','33') OR cref_business_type in ('CH'))")

        If searchCriteria.ViewCriteriaCompanyID > 0 Then
          sQuery.Append(Constants.cAndClause + "comp_id = " + searchCriteria.ViewCriteriaCompanyID.ToString)
        End If

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaCountry.Trim) Then
          sQuery.Append(Constants.cAndClause + "comp_country <> '' AND lower(comp_country) = '" + Replace(searchCriteria.ViewCriteriaCountry.Trim, Constants.cSingleQuote, Constants.cDoubleSingleQuote).ToLower + "'")
        End If

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaCity.Trim) Then
          sQuery.Append(Constants.cAndClause + "comp_city <> '' AND lower(comp_city) = '" + Replace(searchCriteria.ViewCriteriaCity.Trim, Constants.cSingleQuote, Constants.cDoubleSingleQuote).ToLower + "'")
        End If

        If searchCriteria.ViewCriteriaAmodID > -1 Then
          sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
        ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
          sQuery.Append(Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
        End If

        Select Case CInt(searchCriteria.ViewCriteriaAirframeType)
          Case Constants.VIEW_EXECUTIVE
            sQuery.Append(Constants.cAndClause + "(amod_customer_flag = 'Y' AND amod_airframe_type_code='F' AND amod_type_code = 'E')")
          Case Constants.VIEW_JETS
            sQuery.Append(Constants.cAndClause + "(amod_customer_flag = 'Y' AND amod_airframe_type_code='F' AND amod_type_code = 'J')")
          Case Constants.VIEW_TURBOPROPS
            sQuery.Append(Constants.cAndClause + "(amod_customer_flag = 'Y' AND amod_airframe_type_code='F' AND amod_type_code = 'T')")
          Case Constants.VIEW_PISTONS
            sQuery.Append(Constants.cAndClause + "(amod_customer_flag = 'Y' AND amod_airframe_type_code='F' AND amod_type_code = 'P')")
          Case Constants.VIEW_HELICOPTERS
            sQuery.Append(Constants.cAndClause + "(amod_customer_flag = 'Y' AND amod_airframe_type_code='R' AND amod_type_code in ('T','P'))")
        End Select



        If searchCriteria.ViewCriteriaAFTTStart > 0 Then
          sQuery.Append(Constants.cAndClause + " ((ac_airframe_tot_hrs >= " & searchCriteria.ViewCriteriaAFTTStart & ") or (ac_airframe_tot_hrs IS NULL))")
        End If

        If searchCriteria.ViewCriteriaAFTTEnd > 0 Then
          sQuery.Append(Constants.cAndClause + " ((ac_airframe_tot_hrs <= " & searchCriteria.ViewCriteriaAFTTEnd & ") or (ac_airframe_tot_hrs IS NULL))")
        End If


        If searchCriteria.ViewCriteriaYearStart > 0 Then
          sQuery.Append(Constants.cAndClause + " ac_mfr_year >= " & searchCriteria.ViewCriteriaYearStart)
        End If


        If searchCriteria.ViewCriteriaYearEnd > 0 Then
          sQuery.Append(Constants.cAndClause + " ac_mfr_year <=  " & searchCriteria.ViewCriteriaYearEnd)
        End If



        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAirframeTypeStr.Trim) Then
          sQuery.Append(Constants.cAndClause + "amod_airframe_type_code = '" + searchCriteria.ViewCriteriaAirframeTypeStr.Trim + "'")
        End If

        If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
          sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))
        Else
          sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True))
        End If

        sQuery.Append(" GROUP BY comp_country ORDER BY comp_country ASC")

      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaCountry.Trim) And searchCriteria.ViewID > 2 Then

        sQuery.Append("SELECT DISTINCT comp_city, comp_state, count(distinct comp_id) AS comp_count")
        sQuery.Append(" FROM aircraft_summary WITH(NOLOCK)")
        sQuery.Append(" WHERE ac_lifecycle_stage = 3 AND (cref_contact_type IN ('94','33') OR cref_business_type in ('CH'))")

        If searchCriteria.ViewCriteriaCompanyID > 0 Then
          sQuery.Append(Constants.cAndClause + "comp_id = " + searchCriteria.ViewCriteriaCompanyID.ToString)
        End If

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaCountry.Trim) Then
          sQuery.Append(Constants.cAndClause + "comp_country <> '' AND lower(comp_country) = '" + Replace(searchCriteria.ViewCriteriaCountry.Trim, Constants.cSingleQuote, Constants.cDoubleSingleQuote).ToLower + "'")
        End If

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaCity.Trim) Then
          sQuery.Append(Constants.cAndClause + "comp_city <> '' AND lower(comp_city) = '" + Replace(searchCriteria.ViewCriteriaCity.Trim, Constants.cSingleQuote, Constants.cDoubleSingleQuote).ToLower + "'")
        End If

        If searchCriteria.ViewCriteriaAmodID > -1 Then
          sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
        ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
          sQuery.Append(Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
        End If

        Select Case CInt(searchCriteria.ViewCriteriaAirframeType)
          Case Constants.VIEW_EXECUTIVE
            sQuery.Append(Constants.cAndClause + "(amod_customer_flag = 'Y' AND amod_airframe_type_code='F' AND amod_type_code = 'E')")
          Case Constants.VIEW_JETS
            sQuery.Append(Constants.cAndClause + "(amod_customer_flag = 'Y' AND amod_airframe_type_code='F' AND amod_type_code = 'J')")
          Case Constants.VIEW_TURBOPROPS
            sQuery.Append(Constants.cAndClause + "(amod_customer_flag = 'Y' AND amod_airframe_type_code='F' AND amod_type_code = 'T')")
          Case Constants.VIEW_PISTONS
            sQuery.Append(Constants.cAndClause + "(amod_customer_flag = 'Y' AND amod_airframe_type_code='F' AND amod_type_code = 'P')")
          Case Constants.VIEW_HELICOPTERS
            sQuery.Append(Constants.cAndClause + "(amod_customer_flag = 'Y' AND amod_airframe_type_code='R' AND amod_type_code in ('T','P'))")
        End Select

        If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
          sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))
        Else
          sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True))
        End If

        sQuery.Append(" GROUP BY comp_city, comp_state ORDER BY comp_city, comp_state ASC")

      End If

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_charter_location_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = clientConnectString
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
        aError = "Error in get_charter_location_info load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_charter_location_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

  Public Sub views_display_charter_location_table(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String)

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder
    Dim toggleRowColor As Boolean = False
    Dim sTitle As String = ""

    Try

      results_table = get_charter_location_info(searchCriteria)

      htmlOut.Append("<table id=""displayCharterLocationOuterTable"" width=""100%"" cellspacing=""0"" cellpadding=""0"" class=""module"">")

      If String.IsNullOrEmpty(searchCriteria.ViewCriteriaCountry.Trim) Then
        sTitle = "OPERATOR COUNTRIES"
      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaCountry.Trim) Then
        sTitle = "OPERATOR CITIES"
      End If

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          htmlOut.Append("<tr><td align=""center"" valign=""middle"" class=""header"" colspan=""2"">" + sTitle + "&nbsp;<em>(" + results_table.Rows.Count.ToString + ")</em></td></tr>")

          If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaCity.Trim) And searchCriteria.ViewID > 2 Then
            htmlOut.Append("<tr><td align=""left"" valign=""middle"" class=""seperator"" colspan=""2"">")
            htmlOut.Append("<a href=""View_Template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName.Trim + "&viewCompany=" + searchCriteria.ViewCriteriaCompanyID.ToString + "&viewCity=&viewCountry=" + HttpContext.Current.Server.UrlEncode(searchCriteria.ViewCriteriaCountry.Trim) + "&viewAirframe=" + searchCriteria.ViewCriteriaAirframeTypeStr.Trim + "&amod_id=" + searchCriteria.ViewCriteriaAmodID.ToString + """ title=""Clear Charter City"">Clear City</a>")
            htmlOut.Append("</strong></td></tr>")
          ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaCountry.Trim) And searchCriteria.ViewID > 2 Then
            htmlOut.Append("<tr><td align=""left"" valign=""middle"" class=""seperator"" colspan=""2"">")
            htmlOut.Append("<a href=""View_Template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName.Trim + "&viewCompany=" + searchCriteria.ViewCriteriaCompanyID.ToString + "&viewCity=" + HttpContext.Current.Server.UrlEncode(searchCriteria.ViewCriteriaCity.Trim) + "&viewCountry=&viewAirframe=" + searchCriteria.ViewCriteriaAirframeTypeStr.Trim + "&amod_id=" + searchCriteria.ViewCriteriaAmodID.ToString + """ title=""Clear Charter Country"">Clear Country</a>")
            htmlOut.Append("</strong></td></tr>")
          End If

          htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""seperator"" width=""80%"" style=""padding-left:5px;"" valign=""top""><strong>Location</strong></td>")
          htmlOut.Append("<td valign=""top"" align=""right"" class=""seperator"" style=""padding-right:5px;""><strong>Count</strong></td></tr>")

          htmlOut.Append("<tr><td class=""rightside"" colspan=""2"">")

          If results_table.Rows.Count > 12 Then
            If searchCriteria.ViewCriteriaAmodID = -1 Then
              htmlOut.Append("<div valign=""top"" style=""height:800px; overflow-y: auto;""><p>")
            Else
              htmlOut.Append("<div valign=""top"" style=""height:310px; overflow-y: auto;""><p>")
            End If
          End If

          htmlOut.Append("<table id=""displayCharterLocationDataTable"" width=""100%"" cellpadding=""4"" cellspacing=""0"">")

          Dim tmpValue As String = ""

          For Each r As DataRow In results_table.Rows

            If r.Table.Columns.Contains("comp_city") And Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaCountry.Trim) Then
              tmpValue = r.Item("comp_city").ToString.Trim
            ElseIf r.Table.Columns.Contains("comp_country") Then
              tmpValue = r.Item("comp_country").ToString.Trim
            End If

            If CLng(r.Item("comp_count").ToString) > 0 And Not String.IsNullOrEmpty(tmpValue.Trim) Then

              If Not toggleRowColor Then
                htmlOut.Append("<tr class=""alt_row"">")
                toggleRowColor = True
              Else
                htmlOut.Append("<tr bgcolor=""white"">")
                toggleRowColor = False
              End If

              If String.IsNullOrEmpty(searchCriteria.ViewCriteriaCountry.Trim) Then
                htmlOut.Append("<td align=""left"" valign=""top"" class=""seperator"" width=""80%"">")
                htmlOut.Append("<a href=""view_template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName.Trim + "&viewCompany=" + searchCriteria.ViewCriteriaCompanyID.ToString + "&viewCity=&viewCountry=" + HttpContext.Current.Server.UrlEncode(r.Item("comp_country").ToString.Trim) + "&viewAirframe=" + searchCriteria.ViewCriteriaAirframeTypeStr.Trim + "&amod_id=" + searchCriteria.ViewCriteriaAmodID.ToString + """>" + r.Item("comp_country").ToString.Trim + "</a></td>")
                htmlOut.Append("<td align=""right"" valign=""top"" class=""seperator"" style=""padding-right:5px;"">" + FormatNumber(r.Item("comp_count").ToString, 0) + "</td></tr>")
              ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaCountry.Trim) Then
                htmlOut.Append("<td align=""left"" valign=""top"" class=""seperator"" width=""80%"">")
                htmlOut.Append("<a href=""view_template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName.Trim + "&viewCompany=" + searchCriteria.ViewCriteriaCompanyID.ToString + "&viewCity=" + HttpContext.Current.Server.UrlEncode(r.Item("comp_city").ToString.Trim) + "&viewCountry=" + HttpContext.Current.Server.UrlEncode(searchCriteria.ViewCriteriaCountry.Trim) + "&viewAirframe=" + searchCriteria.ViewCriteriaAirframeTypeStr.Trim + "&amod_id=" + searchCriteria.ViewCriteriaAmodID.ToString + """>" + r.Item("comp_city").ToString.Trim + IIf(Not IsDBNull(r.Item("comp_state")), IIf(Not String.IsNullOrEmpty(r.Item("comp_state").ToString.Trim), ", " + r.Item("comp_state").ToString.Trim, ""), "") + "</a></td>")
                htmlOut.Append("<td align=""right"" valign=""top"" class=""seperator"" style=""padding-right:5px;"">" + FormatNumber(r.Item("comp_count").ToString, 0) + "</td></tr>")
              End If

            End If

          Next

          htmlOut.Append("</table>")

          If results_table.Rows.Count > 12 Then
            htmlOut.Append("</p></div>")
          End If

        Else
          htmlOut.Append("<tr><td align=""left"" valign=""middle"" class=""border_bottom_right"" colspan=""2"">No aircraft countries for your search criteria.</td></tr>")
        End If
      Else
        htmlOut.Append("<tr><td align=""left"" valign=""middle"" class=""border_bottom_right"" colspan=""2"">No aircraft countries for your search criteria.</td></tr>")
      End If

      htmlOut.Append("</table>")

    Catch ex As Exception

      aError = "Error in views_display_charter_location_table(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

    Finally

    End Try

    'return resulting html string
    out_htmlString = htmlOut.ToString
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Function get_charter_company_associated_names(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append(" SELECT DISTINCT comp_id, comp_name, comp_city, comp_state FROM aircraft_summary WITH(NOLOCK)")
      sQuery.Append(" WHERE ac_id = " + searchCriteria.ViewCriteriaAircraftID.ToString)
      sQuery.Append(" AND ac_lifecycle_stage = 3 AND (cref_contact_type IN ('94','33') OR cref_business_type in ('CH'))")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_charter_company_associated_names(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = clientConnectString
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
        aError = "Error in get_charter_company_associated_names load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_charter_company_associated_names(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

  Public Sub views_display_charter_company_associated_names(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String)

    Dim htmlOut As New StringBuilder
    Dim results_table As New DataTable

    Try

      results_table = get_charter_company_associated_names(searchCriteria)

      If Not IsNothing(results_table) Then
        If results_table.Rows.Count > 0 Then

          For Each r As DataRow In results_table.Rows

            If Not IsDBNull(r.Item("comp_id")) Then
              If Not String.IsNullOrEmpty(r.Item("comp_id").ToString.Trim) Then

                htmlOut.Append("<a href=""View_Template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "" & IIf(searchCriteria.ViewID = 1, "&activetab=8", "") & "&ViewName=" + searchCriteria.ViewName.Trim + "&viewCompany=" + r.Item("comp_id").ToString + "&viewCity=" + searchCriteria.ViewCriteriaCity.Trim + "&viewCountry=" + searchCriteria.ViewCriteriaCountry.Trim + "&viewAirframe=" + searchCriteria.ViewCriteriaAirframeTypeStr.Trim + "&amod_id=" + searchCriteria.ViewCriteriaAmodID.ToString + """>" + r.Item("comp_name").ToString.Trim + "</a> - <font size=""-2"">" + r.Item("comp_city").ToString.Trim + IIf(Not IsDBNull(r.Item("comp_state")), IIf(Not String.IsNullOrEmpty(r.Item("comp_state").ToString.Trim), ", " + r.Item("comp_state").ToString.Trim, ""), "") + "</font><br />")

              End If
            End If

          Next

        End If
      End If


    Catch ex As Exception

      aError = "Error in display_charter_company_associated_names(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

    Finally

    End Try

    'return resulting html string
    out_htmlString = htmlOut.ToString
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Function get_charter_vs_noncharter_piechart_info(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal bUseCharterQuery As Boolean) As DataTable
    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      If Not bUseCharterQuery Then

        ' get total fleet for model    
        sQuery.Append("SELECT DISTINCT ac_id, ac_ownership_type")
        sQuery.Append(" FROM Aircraft WITH(NOLOCK) INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id")

        If searchCriteria.ViewCriteriaCompanyID > 0 Then
          sQuery.Append(" INNER JOIN Aircraft_Reference WITH(NOLOCK) ON cref_ac_id = ac_id AND cref_journ_id = ac_journ_id")
        End If

        If searchCriteria.ViewCriteriaAmodID > -1 Then
          sQuery.Append(Constants.cWhereClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString + " AND ac_journ_id = 0")
        ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
          sQuery.Append(Constants.cWhereClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "' AND ac_journ_id = 0")
        End If

        If searchCriteria.ViewCriteriaAmodID > -1 And searchCriteria.ViewCriteriaCompanyID > 0 Then
          sQuery.Append(Constants.cAndClause + " ac_lifecycle_stage = 3 AND (cref_contact_type IN ('94','33') OR cref_business_type = 'CH')")
        End If

        Select Case CInt(searchCriteria.ViewCriteriaAirframeType)
          Case Constants.VIEW_EXECUTIVE
            sQuery.Append(Constants.cAndClause + "(amod_customer_flag = 'Y' AND amod_airframe_type_code='F' AND amod_type_code = 'E')")
          Case Constants.VIEW_JETS
            sQuery.Append(Constants.cAndClause + "(amod_customer_flag = 'Y' AND amod_airframe_type_code='F' AND amod_type_code = 'J')")
          Case Constants.VIEW_TURBOPROPS
            sQuery.Append(Constants.cAndClause + "(amod_customer_flag = 'Y' AND amod_airframe_type_code='F' AND amod_type_code = 'T')")
          Case Constants.VIEW_PISTONS
            sQuery.Append(Constants.cAndClause + "(amod_customer_flag = 'Y' AND amod_airframe_type_code='F' AND amod_type_code = 'P')")
          Case Constants.VIEW_HELICOPTERS
            sQuery.Append(Constants.cAndClause + "(amod_customer_flag = 'Y' AND amod_airframe_type_code='R' AND amod_type_code in ('T','P'))")
        End Select

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAirframeTypeStr.Trim) Then
          sQuery.Append(Constants.cAndClause + "amod_airframe_type_code = '" + searchCriteria.ViewCriteriaAirframeTypeStr.Trim + "'")
        End If

        If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
          sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, True))
        Else
          sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True))
        End If

      Else

        sQuery.Append("SELECT DISTINCT ac_id, ac_ownership_type")
        sQuery.Append(" FROM Aircraft WITH(NOLOCK) INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id")

        If searchCriteria.ViewCriteriaAmodID > -1 Then
          sQuery.Append(Constants.cWhereClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString + " AND ac_journ_id = 0")
        ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
          sQuery.Append(Constants.cWhereClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "' AND ac_journ_id = 0")
        End If

        sQuery.Append(" AND EXISTS (SELECT NULL FROM aircraft_reference WITH(NOLOCK)")
        sQuery.Append(" WHERE cref_ac_id = ac_id AND cref_journ_id = ac_journ_id")
        sQuery.Append(Constants.cAndClause + " ac_lifecycle_stage = 3 AND (cref_contact_type IN ('94','33') OR cref_business_type = 'CH')")

        If searchCriteria.ViewCriteriaCompanyID > 0 Then
          sQuery.Append(Constants.cAndClause + "cref_comp_id = " + searchCriteria.ViewCriteriaCompanyID.ToString)
        End If

        sQuery.Append(Constants.cSingleClose)

        Select Case CInt(searchCriteria.ViewCriteriaAirframeType)
          Case Constants.VIEW_EXECUTIVE
            sQuery.Append(Constants.cAndClause + "(amod_customer_flag = 'Y' AND amod_airframe_type_code='F' AND amod_type_code = 'E')")
          Case Constants.VIEW_JETS
            sQuery.Append(Constants.cAndClause + "(amod_customer_flag = 'Y' AND amod_airframe_type_code='F' AND amod_type_code = 'J')")
          Case Constants.VIEW_TURBOPROPS
            sQuery.Append(Constants.cAndClause + "(amod_customer_flag = 'Y' AND amod_airframe_type_code='F' AND amod_type_code = 'T')")
          Case Constants.VIEW_PISTONS
            sQuery.Append(Constants.cAndClause + "(amod_customer_flag = 'Y' AND amod_airframe_type_code='F' AND amod_type_code = 'P')")
          Case Constants.VIEW_HELICOPTERS
            sQuery.Append(Constants.cAndClause + "(amod_customer_flag = 'Y' AND amod_airframe_type_code='R' AND amod_type_code in ('T','P'))")
        End Select

        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAirframeTypeStr.Trim) Then
          sQuery.Append(Constants.cAndClause + "amod_airframe_type_code = '" + searchCriteria.ViewCriteriaAirframeTypeStr.Trim + "'")
        End If

        If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
          sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, True))
        Else
          sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True))
        End If

      End If

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_charter_vs_noncharter_piechart_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = clientConnectString
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
        aError = "Error in get_charter_vs_noncharter_piechart_info load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_charter_vs_noncharter_piechart_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

  Public Sub views_display_charter_vs_noncharter_piechart(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByVal graphID As Integer)

    Dim results_table_non_charter As New DataTable
    Dim results_table_charter As New DataTable
    Dim htmlOut As New StringBuilder
    Dim sTitle As String = ""

    Dim x As Integer = 0

    Dim totalInOpNonCharterCount As Double = 0.0
    Dim totalInOpCharterCount As Double = 0.0

    Dim oncharter_percent As Double = 0.0
    Dim notcharter_percent As Double = 0.0

    Dim total_non_and_charter As Double = 0.0

    Try

      results_table_non_charter = get_charter_vs_noncharter_piechart_info(searchCriteria, False)

      If Not IsNothing(results_table_non_charter) Then
        If results_table_non_charter.Rows.Count > 0 Then
          For Each r As DataRow In results_table_non_charter.Rows
            If r.Item("ac_ownership_type").ToString.ToUpper.Trim.Contains("S") Or r.Item("ac_ownership_type").ToString.ToUpper.Trim.Contains("F") Or r.Item("ac_ownership_type").ToString.ToUpper.Trim.Contains("W") Then
              totalInOpNonCharterCount += 1
            End If
          Next
        End If
      End If

      results_table_charter = get_charter_vs_noncharter_piechart_info(searchCriteria, True)
      If Not IsNothing(results_table_charter) Then
        If results_table_charter.Rows.Count > 0 Then
          For Each r As DataRow In results_table_charter.Rows
            If r.Item("ac_ownership_type").ToString.ToUpper.Trim.Contains("S") Or r.Item("ac_ownership_type").ToString.ToUpper.Trim.Contains("F") Or r.Item("ac_ownership_type").ToString.ToUpper.Trim.Contains("W") Then
              totalInOpCharterCount += 1
            End If
          Next
        End If
      End If

      If Not IsNothing(results_table_non_charter) And Not IsNothing(results_table_charter) Then

        If totalInOpNonCharterCount > 0 And totalInOpCharterCount > 0 Then

          htmlOut.Append(vbCrLf + "<script type=""text/javascript"">" + vbCrLf)
          htmlOut.Append("google.load('visualization', '1', {'packages':['corechart']});" + vbCrLf)
          htmlOut.Append("google.setOnLoadCallback(drawVisualization" + graphID.ToString + ");" + vbCrLf)
          htmlOut.Append("function drawVisualization" + graphID.ToString + "() {" + vbCrLf)
          htmlOut.Append("var data = new google.visualization.DataTable();" + vbCrLf)
          htmlOut.Append("data.addColumn('string', 'Charter');" + vbCrLf)
          htmlOut.Append("data.addColumn('number', 'Value');" + vbCrLf)
          htmlOut.Append("data.addRows(4);" + vbCrLf)

          oncharter_percent = (totalInOpCharterCount / totalInOpNonCharterCount) * CDbl(100)
          notcharter_percent = ((totalInOpNonCharterCount - totalInOpCharterCount) / totalInOpNonCharterCount) * CDbl(100)

          If searchCriteria.ViewCriteriaAmodID > -1 And searchCriteria.ViewCriteriaCompanyID > 0 Then  ' not then should be a acContactType_makemodel_name
            htmlOut.Append("data.setCell(0, 0, 'Remaining Percentage', 'Other (" + FormatNumber(notcharter_percent, 1, False, False, False) + "%)');" + vbCrLf)
            htmlOut.Append("data.setCell(0, 1, " + Math.Round(totalInOpNonCharterCount, 0).ToString + ");" + vbCrLf)
            htmlOut.Append("data.setCell(1, 0, 'Company Percentage','" + commonEvo.get_company_name_fromID(searchCriteria.ViewCriteriaCompanyID, 0, False, True, "") + " (" + FormatNumber(oncharter_percent, 1, False, False, False) + "%)');" + vbCrLf)
            htmlOut.Append("data.setCell(1, 1, " + Math.Round(totalInOpCharterCount, 0).ToString + ");" + vbCrLf)
          Else
            htmlOut.Append("data.setCell(0, 0, 'Charter Fleet','Charter (" + FormatNumber(oncharter_percent, 1, False, False, False) + "%)');" + vbCrLf)
            htmlOut.Append("data.setCell(0, 1, " + Math.Round(totalInOpCharterCount, 0).ToString + ");" + vbCrLf)
            htmlOut.Append("data.setCell(1, 0, 'Non Charter Fleet','Non Charter (" + FormatNumber(notcharter_percent, 1, False, False, False) + "%)');" + vbCrLf)
            htmlOut.Append("data.setCell(1, 1, " + Math.Round(totalInOpNonCharterCount, 0).ToString + ");" + vbCrLf)
          End If

          htmlOut.Append("var chart = new google.visualization.PieChart(document.getElementById(""visualization" + graphID.ToString + """));" + vbCrLf)

          htmlOut.Append("chart.draw(data, {chartArea:{width:'95%',height:'85%'}, legend:'top', legendFontSize:11});" + vbCrLf)

          htmlOut.Append("}" + vbCrLf)
          htmlOut.Append("</script>" + vbCrLf)

        End If

      End If

      If searchCriteria.ViewCriteriaAmodID > -1 And searchCriteria.ViewCriteriaCompanyID > 0 Then
        sTitle = "OPERATOR : "
        sTitle += commonEvo.get_company_name_fromID(searchCriteria.ViewCriteriaCompanyID, 0, False, True, "")

        If searchCriteria.ViewCriteriaAmodID > -1 Then
          sTitle += "<br /> MODEL : " + commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaAmodID, False, "")
        ElseIf searchCriteria.ViewCriteriaMakeAmodID > -1 Then
          sTitle += "<br />MODEL : " + commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaMakeAmodID, True, "")
        End If

      ElseIf searchCriteria.ViewCriteriaAmodID > -1 Then
        If searchCriteria.ViewCriteriaAmodID > -1 Then
          sTitle += "MODEL : " + commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaAmodID, False, "")
        ElseIf searchCriteria.ViewCriteriaMakeAmodID > -1 Then
          sTitle += "MODEL : " + commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaMakeAmodID, True, "")
        End If
      End If

      If Not String.IsNullOrEmpty(htmlOut.ToString.Trim) Then
        htmlOut.Append("<table width=""100%"" height=""200"" cellpadding=""2"" cellspacing=""0"" class=""module"">")
        If searchCriteria.ViewCriteriaAmodID > -1 And searchCriteria.ViewCriteriaCompanyID > 0 Then
          htmlOut.Append("<tr><td valign=""middle"" align=""center"" class=""header"">% OF CHARTER MARKET<br />" + sTitle + "</td></tr>")
        ElseIf searchCriteria.ViewCriteriaAmodID > -1 Then
          htmlOut.Append("<tr><td valign=""middle"" align=""center"" class=""header"">% CHARTER VS. NON CHARTER<br />" + sTitle + "</td></tr>")
        End If
        htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""border_bottom_right""><div id=""visualization" + graphID.ToString + """ style=""text-align:center; width:100%; height:200px;""></div></td></tr></table>")
      Else
        htmlOut.Append("<table width=""100%"" height=""200"" cellpadding=""2"" cellspacing=""0"" class=""module"">")
        If searchCriteria.ViewCriteriaAmodID > -1 And searchCriteria.ViewCriteriaCompanyID > 0 Then
          htmlOut.Append("<tr><td valign=""middle"" align=""center"" class=""header"">% OF CHARTER MARKET<br />" + sTitle + "</td></tr>")
        ElseIf searchCriteria.ViewCriteriaAmodID > -1 Then
          htmlOut.Append("<tr><td valign=""middle"" align=""center"" class=""header"">% CHARTER VS. NON CHARTER<br />" + sTitle + "</td></tr>")
        End If
        htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""border_bottom_right""><div style=""text-align:center; width:100%; height:200px;"">No Data to display</div></td></tr></table>")
      End If

    Catch ex As Exception

      aError = "Error in views_display_operator_piechart(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByVal graphID As Integer) " + ex.Message

    Finally

    End Try

    'return resulting html string
    out_htmlString = htmlOut.ToString
    htmlOut = Nothing

    results_table_non_charter = Nothing
    results_table_charter = Nothing

  End Sub

#End Region

End Class
