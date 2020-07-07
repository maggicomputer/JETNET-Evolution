' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/Entity Classes/operator_view_functions.vb $
'$$Author: Amanda $
'$$Date: 7/01/20 4:18p $
'$$Modtime: 7/01/20 8:24a $
'$$Revision: 8 $
'$$Workfile: operator_view_functions.vb $
'
' ********************************************************************************

<System.Serializable()> Public Class operator_view_functions
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

#Region "operator_view_functions"

    Public Function get_operator_companies(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

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


            If searchCriteria.ViewCriteriaAmodID > -1 Or (Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaEngineName.Trim) And Not searchCriteria.ViewCriteriaEngineName.ToLower.Trim.Contains("all")) Then
                HttpContext.Current.Session.Item("Selection_Listing_Fields") &= ("SELECT distinct comp_name , comp_country , comp_id , count(distinct ac_id) AS acCount ")
            Else
                HttpContext.Current.Session.Item("Selection_Listing_Fields") &= ("SELECT distinct top 250 comp_name , comp_country , comp_id , count(distinct ac_id) AS acCount ")
            End If
            HttpContext.Current.Session.Item("Selection_Listing_Fields") &= (", sum(case when ac_lease_flag = 'Y' then 1 else 0 end) as LeaseCount ")


            sQuery.Append(HttpContext.Current.Session.Item("Selection_Listing_Fields"))

            HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), " comp_name ", " comp_name as 'Company Name' ")
            HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), " comp_country ", " comp_country as 'Country' ")
            HttpContext.Current.Session.Item("Selection_Listing_Fields") = Replace(HttpContext.Current.Session.Item("Selection_Listing_Fields"), " comp_id ", " comp_id as 'Comp ID' ")


            HttpContext.Current.Session.Item("Selection_Listing_Table") &= (" FROM Aircraft_Summary WITH(NOLOCK) ")

            sQuery.Append(HttpContext.Current.Session.Item("Selection_Listing_Table"))


            HttpContext.Current.Session.Item("Selection_Listing_Where") &= ("  WHERE ac_lifecycle_stage = '3' AND cref_operator_flag IN ('Y', 'O')")

            If searchCriteria.ViewCriteriaAmodID > -1 Then
                HttpContext.Current.Session.Item("Selection_Listing_Where") &= (Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
            ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
                HttpContext.Current.Session.Item("Selection_Listing_Where") &= (Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
            ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftType.Trim) Then
                HttpContext.Current.Session.Item("Selection_Listing_Where") &= (Constants.cAndClause + "amod_type_code = '" + searchCriteria.ViewCriteriaAircraftType.Trim + "'")
            End If

            If searchCriteria.ViewCriteriaCompanyID > 0 Then
                HttpContext.Current.Session.Item("Selection_Listing_Where") &= (Constants.cAndClause + "comp_id = " + searchCriteria.ViewCriteriaCompanyID.ToString)
            End If

            If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaEngineName.Trim) And Not searchCriteria.ViewCriteriaEngineName.ToLower.Trim.Contains("all") Then
                HttpContext.Current.Session.Item("Selection_Listing_Where") &= (Constants.cAndClause + "ac_engine_name = '" + searchCriteria.ViewCriteriaEngineName.Trim + "'")
            End If

            If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaCountry.Trim) Then
                HttpContext.Current.Session.Item("Selection_Listing_Where") &= (Constants.cAndClause + "comp_country = '" + searchCriteria.ViewCriteriaCountry.ToString.Trim + "'")
            End If

            If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaCity.Trim) Then
                HttpContext.Current.Session.Item("Selection_Listing_Where") &= (Constants.cAndClause + "comp_city = '" + searchCriteria.ViewCriteriaCity.ToString.Trim + "'")
            End If

            Select Case CInt(searchCriteria.ViewCriteriaAirframeType)
                Case Constants.VIEW_EXECUTIVE
                    HttpContext.Current.Session.Item("Selection_Listing_Where") &= (Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'E'")
                Case Constants.VIEW_JETS
                    HttpContext.Current.Session.Item("Selection_Listing_Where") &= (Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'J'")
                Case Constants.VIEW_TURBOPROPS
                    HttpContext.Current.Session.Item("Selection_Listing_Where") &= (Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'T'")
                Case Constants.VIEW_PISTONS
                    HttpContext.Current.Session.Item("Selection_Listing_Where") &= (Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'P'")
                Case Constants.VIEW_HELICOPTERS
                    HttpContext.Current.Session.Item("Selection_Listing_Where") &= (Constants.cAndClause + "amod_airframe_type_code='R' AND amod_type_code in ('T','P')")
            End Select

            If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
                HttpContext.Current.Session.Item("Selection_Listing_Where") &= (" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))
            Else
                HttpContext.Current.Session.Item("Selection_Listing_Where") &= (" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, False))
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


            HttpContext.Current.Session.Item("Selection_Listing_Group") = (" GROUP BY comp_name, comp_country, comp_id")


            sQuery.Append(HttpContext.Current.Session.Item("Selection_Listing_Group"))

            HttpContext.Current.Session.Item("Selection_Listing_Order") = (" ORDER BY count(distinct ac_id) DESC, comp_name ASC")

            sQuery.Append(HttpContext.Current.Session.Item("Selection_Listing_Order"))



            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_operator_companies(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
                aError = "Error in get_operator_companies load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_operator_companies(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

    Public Sub views_display_operator_companies(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String)

        Dim strOut As New StringBuilder
        Dim htmlOut As New StringBuilder
        Dim strOut_temp As New StringBuilder
        Dim results_table As New DataTable
        Dim toggleRowColor As Boolean = False
        Dim aclsData_Temp As New clsData_Manager_SQL
        Dim sTmpTitle As String = ""
        Dim sTitle As String = ""
        Dim sTmpCompanyName As String = ""

        Dim sLinkString As String = ""

        Dim inservicetot As Integer = 0
        Dim leasedtot As Integer = 0
        Dim retiredtot As Integer = 0
        Dim retiredtot2 As Integer = 0
        Dim onordertot As Integer = 0
        Dim total As Integer = 0
        Dim totaltot As Integer = 0

        Dim nColspan As Long = 0
        Dim sTempCompanyHtml As String = ""
        Dim sTmpCompanyInfo As String = ""
        Dim MainLocationDataTable As New DataTable
        Dim logoID As Long = 0
        Dim imgDisplayFolder As String = ""
        Dim logo_image As String = ""
        Dim InfoTable As New DataTable

        If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_COMMERCIAL Then
            nColspan = 7
        ElseIf searchCriteria.ViewCriteriaCompanyID > 0 And searchCriteria.ViewCriteriaProductType <> Constants.PRODUCT_CODE_COMMERCIAL Then
            nColspan = 1
        Else
            nColspan = 3
        End If

        Try


            aclsData_Temp = New clsData_Manager_SQL
            aclsData_Temp.JETNET_DB = HttpContext.Current.Session.Item("jetnetClientDatabase")
            aclsData_Temp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase")


            results_table = get_operator_companies(searchCriteria)

            If searchCriteria.ViewCriteriaCompanyID > 0 Then
                sTmpCompanyName = commonEvo.get_company_name_fromID(searchCriteria.ViewCriteriaCompanyID, 0, False, True, "")
                sTmpCompanyInfo = commonEvo.get_company_info_fromID(searchCriteria.ViewCriteriaCompanyID, 0, True, True, "", "")

                MainLocationDataTable = aclsData_Temp.GetCompanyMainLocationDescriptionLogo(searchCriteria.ViewCriteriaCompanyID)
                If Not IsNothing(MainLocationDataTable) Then
                    If MainLocationDataTable.Rows.Count > 0 Then
                        'This means there is a main location, let's check for a logo here.
                        If Not IsDBNull(MainLocationDataTable.Rows(0).Item("comp_logo_flag")) Then
                            If MainLocationDataTable.Rows(0).Item("comp_logo_flag").ToString.ToUpper.Contains("Y") Then
                                logoID = CLng(MainLocationDataTable.Rows(0).Item("comp_id").ToString)
                            End If
                        End If
                    End If
                End If

                If logoID = 0 Then
                    InfoTable = aclsData_Temp.GetCompanyInfo_ID(searchCriteria.ViewCriteriaCompanyID, "JETNET", 0)
                    If Not IsNothing(InfoTable) Then
                        If InfoTable.Rows.Count > 0 Then
                            If Not IsDBNull(InfoTable.Rows(0).Item("comp_logo_flag")) Then
                                If Trim(InfoTable.Rows(0).Item("comp_logo_flag")) = "Y" Then
                                    logoID = searchCriteria.ViewCriteriaCompanyID
                                End If
                            End If
                        End If
                    End If
                End If

                aclsData_Temp = Nothing


                If HttpContext.Current.Session.Item("jetnetWebSiteType") = crmWebClient.eWebSiteTypes.LOCAL Then
                    imgDisplayFolder = "https://www.jetnetevolution.com/pictures/company"
                Else
                    imgDisplayFolder = HttpContext.Current.Session.Item("jetnetFullHostName").ToString.Trim + HttpContext.Current.Session.Item("CompanyPicturesFolderVirtualPath")
                End If

                If logoID = 0 Then
                    logo_image = ""
                Else
                    logo_image = "<img src='" + imgDisplayFolder + Constants.cSingleForwardSlash + logoID.ToString + ".jpg' class='float_right border' width='140' />"
                End If

                sTmpTitle += " : " + sTmpCompanyName.Trim
            End If

            If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaEngineName) And Not searchCriteria.ViewCriteriaEngineName.ToLower.Contains("all") Then

                sTitle = "OPERATOR SUMMARY" + sTmpTitle + searchCriteria.ViewCriteriaEngineName.Trim + " Engine"

            Else

                If searchCriteria.ViewCriteriaAmodID = -1 And searchCriteria.ViewCriteriaCompanyID = 0 Then
                    sTitle = "OPERATOR SUMMARY : TOP 250 OPERATORS"

                    Select Case (searchCriteria.ViewCriteriaProductType)

                        Case Constants.PRODUCT_CODE_BUSINESS
                            sTitle += " - Business "
                            sTmpTitle = " Business "
                        Case Constants.PRODUCT_CODE_COMMERCIAL
                            sTitle += " - Commercial "
                            sTmpTitle = " Commercial "
                        Case Constants.PRODUCT_CODE_HELICOPTERS
                            sTitle += " - Helicopter "
                            sTmpTitle = " Helicopter "
                    End Select

                Else

                    If searchCriteria.ViewCriteriaAmodID > -1 Then
                        sTmpTitle += " - " + commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaAmodID, False, "")
                    ElseIf searchCriteria.ViewCriteriaMakeAmodID > -1 Then
                        sTmpTitle += " - " + commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaMakeAmodID, True, "")
                    End If

                    If searchCriteria.ViewCriteriaCompanyID > 0 Then
                        sTitle = "OPERATOR" + sTmpTitle
                    Else
                        sTitle = "OPERATOR SUMMARY" + sTmpTitle
                    End If


                End If

            End If

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    If results_table.Rows.Count > 15 Then
                        htmlOut.Append("<div valign=""top"" style=""height:370px; overflow: auto;"">")
                    End If

                    If searchCriteria.ViewCriteriaCompanyID > 0 And searchCriteria.ViewCriteriaProductType <> Constants.PRODUCT_CODE_COMMERCIAL Then
                        htmlOut.Append("<table id='operatorCompaniesInnerTable' width='100%' cellpadding='2' cellspacing='0'>")
                    Else
                        htmlOut.Append("<table id='operatorCompaniesInnerTable' width='100%' cellpadding='2' cellspacing='0' class='module'>")
                    End If


                    htmlOut.Append("<tr><td align=""center"" valign=""middle"" class=""header"" colspan=""" + nColspan.ToString + """>" + sTitle + "")
                    If searchCriteria.ViewCriteriaCompanyID > 0 Then
                        htmlOut.Append("- <strong><a href='view_template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName + "&viewCompany=0&amod_id=" + searchCriteria.ViewCriteriaAmodID.ToString + "'><font color='white'>Clear</font></a></strong>")
                    End If
                    htmlOut.Append("</td></tr>")





                    If searchCriteria.ViewCriteriaCompanyID > 0 And searchCriteria.ViewCriteriaAmodID > -1 Then

                        sLinkString = "<tr><td valign='top' align='right' class='seperator' colspan=""" + nColspan.ToString + """><strong><a href='view_template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName + "&viewCompany=" + searchCriteria.ViewCriteriaCompanyID.ToString + "&amod_id=-1'>Clear Model</a></strong><br />"
                        sLinkString += "</td></tr>"

                    ElseIf searchCriteria.ViewCriteriaAmodID > -1 Then

                        sLinkString = "<tr><td valign='top' align='right' class='seperator' colspan=""" + nColspan.ToString + """><strong><a href='view_template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName + "&viewCompany=" + searchCriteria.ViewCriteriaCompanyID.ToString + "&amod_id=-1'>Clear Model</a></strong><br /><br /></td></tr>"

                    ElseIf searchCriteria.ViewCriteriaCompanyID > 0 Then

                        '  sLinkString = "<tr><td valign='top' align='right' class='seperator' colspan=""" + nColspan.ToString + """><strong><a href='view_template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName + "&viewCompany=0&amod_id=" + searchCriteria.ViewCriteriaAmodID.ToString + "'>Clear Company</a></strong><br /><br /></td></tr>"

                    End If

                    If Not String.IsNullOrEmpty(sLinkString.Trim) And searchCriteria.ViewID > 2 Then
                        htmlOut.Append(sLinkString)
                    End If

                    If searchCriteria.ViewCriteriaCompanyID > 0 And searchCriteria.ViewCriteriaProductType <> Constants.PRODUCT_CODE_COMMERCIAL Then
                    Else
                        htmlOut.Append("<tr>")
                    End If

                    If searchCriteria.ViewCriteriaCompanyID > 0 Then
                        If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_COMMERCIAL Then
                            htmlOut.Append("<td valign='middle' align='left' class='seperator' width='65%'>&nbsp;</td>")
                            htmlOut.Append("<td valign='middle' align='center' class='seperator'><strong>Order</td>")
                        Else
                            ' htmlOut.Append("<td valign='top' align='left' width='100%'>&nbsp;</td>")
                        End If
                    Else
                        If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_COMMERCIAL Then
                            htmlOut.Append("<td valign='middle' align='left' class='seperator' width='65%'><strong>Operator&nbsp;Name&nbsp;(<em>Country<em>)</strong></td>")
                            htmlOut.Append("<td valign='middle' align='center' class='seperator'><strong>Order</td>")
                        Else
                            htmlOut.Append("<td valign='middle' align='left' class='seperator' width='75%'><strong>Operator&nbsp;Name&nbsp;(<em>Country<em>)</strong>")
                            If HttpContext.Current.Session.Item("localUser").crmDemoUserFlag = True Then  ' ADDED IN MSW - 3/26/20
                            Else
                                htmlOut.Append("&nbsp;&nbsp;&nbsp;&nbsp;<a class=""text_underline cursor"" title='View/Export' href=""WebSource.aspx?viewType=dynamic&display=table&PageTitle=Owners"" target=""_blank"" >VIEW IN GRID</a>")
                            End If

                            htmlOut.Append("</td>")
                        End If
                    End If

                    If searchCriteria.ViewCriteriaCompanyID > 0 And searchCriteria.ViewCriteriaProductType <> Constants.PRODUCT_CODE_COMMERCIAL Then
                    Else
                        htmlOut.Append("<td valign='middle' align='center' class='seperator'><strong>Operation</strong></td>")
                        htmlOut.Append("<td valign='middle' align='center' class='seperator'><strong>Leased</strong></td>")
                    End If

                    If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_COMMERCIAL Then
                        htmlOut.Append("<td valign='middle' align='center' class='seperator'><strong>Stored</strong></td>")
                        htmlOut.Append("<td valign='middle' align='center' class='seperator'><strong>Retired</strong></td>")
                        htmlOut.Append("<td valign='middle' align='right' class='seperator' style='padding-right:3px;'><strong>Total</strong></td>")
                    End If

                    If searchCriteria.ViewCriteriaCompanyID > 0 And searchCriteria.ViewCriteriaProductType <> Constants.PRODUCT_CODE_COMMERCIAL Then
                    Else
                        htmlOut.Append("</tr>")
                    End If


                    For Each r As DataRow In results_table.Rows

                        inservicetot = 0
                        leasedtot = 0
                        retiredtot = 0
                        retiredtot2 = 0
                        onordertot = 0

                        If searchCriteria.ViewCriteriaCompanyID > 0 Then
                            strOut.Append("<tr bgcolor='white'>")
                        ElseIf Not toggleRowColor Then
                            strOut.Append("<tr class='alt_row'>")
                            toggleRowColor = True
                        Else
                            strOut.Append("<tr bgcolor='white'>")
                            toggleRowColor = False
                        End If

                        If searchCriteria.ViewCriteriaCompanyID > 0 Then
                            If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_COMMERCIAL Then
                                strOut.Append("<td valign='middle' align='left' width='65%' bgcolor='white'>")
                            Else
                                strOut.Append("<td valign='middle' align='left' width='100%' bgcolor='white'>")
                            End If
                        Else
                            If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_COMMERCIAL Then
                                strOut.Append("<td valign='middle' align='left' width='65%' class='border_bottom_right'>")
                            Else
                                strOut.Append("<td valign='middle' align='left' width='75%' class='border_bottom_right'>")
                            End If
                        End If


                        If searchCriteria.ViewCriteriaCompanyID > 0 Then
                            ' strOut.Append("<a class='underline' onclick='javascript:openSmallWindowJS(""DisplayCompanyDetail.aspx?compid=" + r.Item("comp_id").ToString + "&journid=0"",""CompanyDetails"");' title='Display Company Details'>")
                            'strOut.Append(Replace(r.Item("comp_name").ToString, Constants.cSingleSpace, Constants.cHTMLnbsp) + "</a> <em>(" + r.Item("comp_country").ToString.Trim + ")</em>")
                            If Trim(logo_image) <> "" Then
                                strOut.Append(logo_image.ToString)
                            End If
                            strOut.Append(sTmpCompanyInfo.ToString)

                        Else
                            If aclsData_Temp.is_aerodex_insight() = True Then
                                strOut.Append("<a class='underline' target=""_blank"" href=""DisplayCompanyDetail.aspx?compid=" + r.Item("comp_id").ToString + "&journid=0&use_insight_op=Y"" title='Display Company Details'>" & Replace(r.Item("comp_name").ToString, Constants.cSingleSpace, Constants.cHTMLnbsp) + "</a> <em>(" + r.Item("comp_country").ToString.Trim)
                            Else
                                strOut.Append("<a href='view_template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName + "&viewCompany=" + r.Item("comp_id").ToString + "' title='Show Aircraft Operations'>" + Replace(r.Item("comp_name").ToString, Constants.cSingleSpace, Constants.cHTMLnbsp) + "</a> <em>(" + r.Item("comp_country").ToString.Trim + ")</em>")
                            End If
                            strOut.Append("</a>")
                        End If

                        strOut.Append("</td>")

                        inservicetot = CInt(r.Item("acCount").ToString)

                        searchCriteria.ViewTempCompanyID = CLng(r.Item("comp_id").ToString)
                        searchCriteria.ViewTempAmodID = searchCriteria.ViewCriteriaAmodID

                        If Not IsDBNull(r.Item("LeaseCount")) Then
                            leasedtot = CLng(r.Item("LeaseCount").ToString)
                        Else
                            leasedtot = 0
                        End If

                        ' leasedtot = get_count_totals_ac_table(searchCriteria, "leased", True)

                        If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_COMMERCIAL Then
                            retiredtot2 = get_count_totals_ac_table(searchCriteria, "storage", True)
                            retiredtot = get_count_totals_ac_table(searchCriteria, "retired", True)
                            onordertot = get_count_totals_ac_table(searchCriteria, "order", True)
                            retiredtot = retiredtot - retiredtot2
                        End If

                        total = inservicetot + onordertot + retiredtot + retiredtot2

                        If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_COMMERCIAL Then
                            strOut.Append("<td valign='middle' align='right' class='border_bottom_right'>" + FormatNumber(onordertot, TriState.False, TriState.True, TriState.False, TriState.True).ToString + "&nbsp;</td>")
                        End If

                        If searchCriteria.ViewCriteriaCompanyID > 0 Then
                            strOut_temp.Append("<table><tr><td valign='middle' align='left'>Operation:</td><td valign='middle' align='left'>" + FormatNumber(inservicetot, TriState.False, TriState.True, TriState.False, TriState.True).ToString + "&nbsp;</td>")
                            strOut_temp.Append("<td valign='middle' align='left'>Leased:</td><td valign='middle' align='left'>" + FormatNumber(leasedtot, TriState.False, TriState.True, TriState.False, TriState.True).ToString + "&nbsp;</td>")
                            strOut_temp.Append("</td></tr></table>")
                        Else
                            strOut.Append("<td valign='middle' align='right' class='border_bottom_right'>" + FormatNumber(inservicetot, TriState.False, TriState.True, TriState.False, TriState.True).ToString + "&nbsp;</td>")
                            strOut.Append("<td valign='middle' align='right' class='border_bottom_right'>" + FormatNumber(leasedtot, TriState.False, TriState.True, TriState.False, TriState.True).ToString + "&nbsp;</td>")
                        End If


                        If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_COMMERCIAL Then
                            strOut.Append("<td valign='middle' align='right' class='border_bottom_right'>" + FormatNumber(retiredtot2, TriState.False, TriState.True, TriState.False, TriState.True).ToString + "&nbsp;</td>")
                            strOut.Append("<td valign='middle' align='right' class='border_bottom_right'>" + FormatNumber(retiredtot, TriState.False, TriState.True, TriState.False, TriState.True).ToString + "&nbsp;</td>")
                            strOut.Append("<td valign='middle' align='right' class='border_bottom_right'>" + FormatNumber(total, TriState.False, TriState.True, TriState.False, TriState.True).ToString + "&nbsp;</td>")
                        End If

                        strOut.Append("</tr>")

                        ' clear temp amod id on each loop
                        ' clear temp comp id on each loop
                        ' both to be (0) zero when not being used
                        searchCriteria.ViewTempCompanyID = 0
                        searchCriteria.ViewTempAmodID = 0

                    Next

                Else
                    strOut.Append("<tr><td valign=""top"" align=""left"" class=""border_bottom_right"" colspan=""" + nColspan.ToString + """><br/>No Data Available " + sTitle.Trim + "</td></tr>")
                End If

            Else
                strOut.Append("<tr><td valign=""top"" align=""left"" class=""border_bottom_right"" colspan=""" + nColspan.ToString + """><br/>No Data Available " + sTitle.Trim + "</td></tr>")
            End If

            strOut.Append("</table>")

            ' might be blank 
            strOut.Append(strOut_temp.ToString)

            If results_table.Rows.Count > 15 Then
                strOut.Append("</div>")
            End If

            If searchCriteria.ViewCriteriaCompanyID = 0 And ((String.IsNullOrEmpty(searchCriteria.ViewCriteriaEngineName.Trim) Or searchCriteria.ViewCriteriaEngineName.ToLower.Contains("all"))) Then

                inservicetot = get_count_totals_ac_table(searchCriteria, "operation", False)
                leasedtot = get_count_totals_ac_table(searchCriteria, "leased", False)

                If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_COMMERCIAL Then
                    onordertot = get_count_totals_ac_table(searchCriteria, "order", False)
                    retiredtot2 = get_count_totals_ac_table(searchCriteria, "storage", False)
                    retiredtot = get_count_totals_ac_table(searchCriteria, "retired", False)
                    retiredtot = retiredtot - retiredtot2
                End If

            Else

                Dim sExtraCompanyData As String = ""
                Dim sCompanyName As String = commonEvo.get_company_name_fromID(searchCriteria.ViewCriteriaCompanyID, 0, True, True, sExtraCompanyData)

                Dim aCompanyInfo = Split(sExtraCompanyData, ":")
                Dim sTmpCity As String = ""
                Dim sTmpCountry As String = ""

                For x As Integer = 0 To UBound(aCompanyInfo)

                    If x = 0 Then sTmpCity = aCompanyInfo(0).Trim
                    If x = 1 Then sTmpCountry = aCompanyInfo(1).Trim

                Next

                If Not String.IsNullOrEmpty(sCompanyName) Then
                    sTempCompanyHtml = "<br /><table width='100%' cellspacing='0' cellpadding='4' class='module'>"
                    sTempCompanyHtml += "<tr><td align='left' valign='middle' class='header'>OPERATOR DETAILS" + sTmpTitle.Trim + "</td></tr>"
                    sTempCompanyHtml += "<tr><td valign='middle' align='left' class='seperator'>Name : " + sCompanyName.Trim + "</td></tr>"
                    sTempCompanyHtml += "<tr class='alt_row'><td valign='middle' align='left' class='seperator'>City : " + sTmpCity + "</td></tr>"
                    sTempCompanyHtml += "<tr><td valign='middle' align='left' class='seperator'>Country : " + sTmpCountry + "</td></tr>"
                    sTempCompanyHtml += "</table>"
                End If

            End If

            totaltot = inservicetot + onordertot + retiredtot + retiredtot2

            htmlOut.Append(strOut.ToString())

            If String.IsNullOrEmpty(sTempCompanyHtml) Then

                Dim sOperatorSummaryHtml As String = ""
                views_display_operator_view_summary(searchCriteria, totaltot, inservicetot, onordertot, leasedtot, retiredtot, retiredtot2, False, sOperatorSummaryHtml)
                htmlOut.Append(sOperatorSummaryHtml)

            Else

                If searchCriteria.ViewCriteriaCompanyID > 0 And searchCriteria.ViewCriteriaAmodID > -1 Then

                    Dim sOperatorCertHtml As String = ""
                    display_operator_certification_images(searchCriteria, sOperatorCertHtml)
                    htmlOut.Append("<br />" + sOperatorCertHtml)

                    '  htmlOut.Append("<br /><a href='help/helpexamples/Operator-Certifications.pdf' target='_new' title='Click to view Operator Certification Descriptions PDF'>Operator Certification Decriptions</a>")

                    htmlOut.Append("<br />" + sTempCompanyHtml)

                End If

            End If

        Catch ex As Exception

            aError = "Error in views_display_operator_companies(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub

    Public Function get_operater_piechart_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable
        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            If searchCriteria.ViewCriteriaAmodID > -1 Then

                sQuery.Append("SELECT DISTINCT comp_country AS ac_aport_country, count(*) AS modelCount")
                sQuery.Append(" FROM aircraft_summary WITH(NOLOCK)")
                sQuery.Append(" WHERE ac_lifecycle_stage = 3")
                sQuery.Append(" and comp_country is not null ")

                sQuery.Append(Constants.cAndClause + "(cref_operator_flag IN ('Y', 'O'))")

                If searchCriteria.ViewCriteriaCompanyID > 0 Then
                    sQuery.Append(Constants.cAndClause + "comp_id  = " + searchCriteria.ViewCriteriaCompanyID.ToString)
                End If

                If searchCriteria.ViewCriteriaAmodID > -1 Then
                    sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
                ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
                    sQuery.Append(Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
                ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftType.Trim) Then
                    sQuery.Append(Constants.cAndClause + "amod_type_code = '" + searchCriteria.ViewCriteriaAircraftType.Trim + "'")
                End If

                Select Case CInt(searchCriteria.ViewCriteriaAirframeType)
                    Case Constants.VIEW_EXECUTIVE
                        sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'E'")
                    Case Constants.VIEW_JETS
                        sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'J'")
                    Case Constants.VIEW_TURBOPROPS
                        sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'T'")
                    Case Constants.VIEW_PISTONS
                        sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'P'")
                    Case Constants.VIEW_HELICOPTERS
                        sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='R' AND amod_type_code in ('T','P')")
                End Select

                If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
                    sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, True))
                Else
                    sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True))
                End If

                sQuery.Append(" GROUP BY comp_country ORDER BY modelCount DESC")

            Else

                sQuery.Append("SELECT Case ISNULL(ac_aport_country,'') When '' then 'unknown' ELSE ac_aport_country END AS ac_aport_country, COUNT(*) AS modelCount")
                sQuery.Append(" FROM Aircraft WITH(NOLOCK) INNER JOIN Company WITH(NOLOCK) INNER JOIN Aircraft_Reference WITH(NOLOCK) ON comp_id = cref_comp_id AND")
                sQuery.Append(" comp_journ_id = cref_journ_id ON ac_journ_id = cref_journ_id AND ac_id = cref_ac_id INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id")
                sQuery.Append(" WHERE ac_journ_id = 0 AND ac_lifecycle_stage = 3 AND comp_active_flag = 'Y'")

                sQuery.Append(Constants.cAndClause + "(cref_operator_flag IN ('Y', 'O'))")

                If searchCriteria.ViewCriteriaCompanyID > 0 Then
                    sQuery.Append(Constants.cAndClause + "cref_comp_id  = " + searchCriteria.ViewCriteriaCompanyID.ToString)
                End If

                If searchCriteria.ViewCriteriaAmodID > -1 Then
                    sQuery.Append(Constants.cAndClause + "ac_amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
                ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
                    sQuery.Append(Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
                ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftType.Trim) Then
                    sQuery.Append(Constants.cAndClause + "amod_type_code = '" + searchCriteria.ViewCriteriaAircraftType.Trim + "'")
                End If

                Select Case CInt(searchCriteria.ViewCriteriaAirframeType)
                    Case Constants.VIEW_EXECUTIVE
                        sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'E'")
                    Case Constants.VIEW_JETS
                        sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'J'")
                    Case Constants.VIEW_TURBOPROPS
                        sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'T'")
                    Case Constants.VIEW_PISTONS
                        sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'P'")
                    Case Constants.VIEW_HELICOPTERS
                        sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='R' AND amod_type_code in ('T','P')")
                End Select

                If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
                    sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, True))
                Else
                    sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True))
                End If

                sQuery.Append(" GROUP BY ac_aport_country ORDER BY modelCount DESC")

            End If

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_operater_piechart_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
                aError = "Error in get_operater_piechart_info load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_operater_piechart_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

    Public Sub views_display_operator_piechart(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByVal graphID As Integer, ByRef page1 As Page, ByRef temp_panel As System.Web.UI.UpdatePanel, Optional ByRef charting_string As String = "")

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder
        Dim temp_country As String = ""
        Dim GoogleChart1TabScript As StringBuilder = New StringBuilder()

        Dim temp_string As String = ""
        Dim label_script As New Label
        Dim chart_label As New Label
        Dim string_from_charts As String = ""
        Dim row_added As Boolean = False

        Dim x As Integer = 0

        Try

            results_table = get_operater_piechart_info(searchCriteria)

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    '  htmlOut.Append(vbCrLf + "<script type='text/javascript'>" + vbCrLf)
                    ' htmlOut.Append("google.load('visualization', '1', {'packages':['corechart']});" + vbCrLf)
                    ' htmlOut.Append("google.setOnLoadCallback(drawVisualization" + graphID.ToString + ");" + vbCrLf)




                    '  htmlOut.Append("DrawVisualization" + graphID.ToString + ";")
                    '   htmlOut.Append("function drawVisualization" + graphID.ToString + "() {" + vbCrLf)
                    ' htmlOut.Append("var data = new google.visualization.DataTable();" + vbCrLf)
                    ' htmlOut.Append("data.addColumn('string', 'Label');" + vbCrLf)
                    '  htmlOut.Append("data.addColumn('number', 'Value');" + vbCrLf)
                    'htmlOut.Append("data.addRows(" + results_table.Rows.Count.ToString + ");" + vbCrLf)
                    string_from_charts = string_from_charts & (" data1.addColumn('string', 'Country Name'); ")
                    string_from_charts = string_from_charts & (" data1.addColumn('number', 'Value'); ")
                    string_from_charts = string_from_charts & (" data1.addRows([")




                    For Each r As DataRow In results_table.Rows

                        If Not IsDBNull(r.Item("modelCount")) Then
                            If Not String.IsNullOrEmpty(r.Item("modelCount").ToString.Trim) Then

                                temp_country = r.Item("ac_aport_country").ToString.Trim
                                temp_country = Replace(temp_country, "'", "")


                                If row_added Then
                                    string_from_charts &= (",['" & temp_country & "'," & r.Item("modelCount").ToString & "]")
                                Else
                                    string_from_charts &= ("['" & temp_country & "'," & r.Item("modelCount").ToString & "]")
                                End If
                                row_added = True

                                'If CLng(r.Item("modelCount").ToString.Trim) > 0 Then
                                '  htmlOut.Append("data.setCell(" + x.ToString + ", 0, '" + r.Item("ac_aport_country").ToString.Trim + "');" + vbCrLf)
                                '  htmlOut.Append("data.setCell(" + x.ToString + ", 1, " + r.Item("modelCount").ToString + ");" + vbCrLf)
                                'Else
                                '  htmlOut.Append("data.setCell(" + x.ToString + ", 0, '" + r.Item("ac_aport_country").ToString.Trim + "');" + vbCrLf)
                                '  htmlOut.Append("data.setCell(" + x.ToString + ", 1, 0);" + vbCrLf)
                                'End If
                                x += 1
                            End If
                        End If

                    Next

                    string_from_charts &= ("]);")





                    'htmlOut.Append("var chart = new google.visualization.PieChart(document.getElementById('visualization" + graphID.ToString + "'));" + vbCrLf)

                    'If results_table.Rows.Count > 35 Then  ' 1/720 slice visibility threshold
                    '  htmlOut.Append("chart.draw(data, {chartArea:{width:'95%',height:'85%'}, sliceVisibilityThreshold:'0', pieResidueSliceLabel:'Other', legend:'left', legendFontSize:12 });" + vbCrLf)
                    'Else
                    '  htmlOut.Append("chart.draw(data, {chartArea:{width:'95%',height:'85%'}, legend:'left', legendFontSize:12 });" + vbCrLf)
                    'End If


                    '  htmlOut.Append("}" + vbCrLf)
                    ' htmlOut.Append("</script>" + vbCrLf)

                End If

            End If




            charting_string = string_from_charts



            temp_string = "<script type=""text/javascript"">google.charts.setOnLoadCallback(function() {drawChart1();function drawChart1() {var data1 = new google.visualization.DataTable(); "
            temp_string &= string_from_charts
            ' temp_string &= ";var options1 = {'title':'','width':500,curveType:  'function','height':250,legend: { position: 'right', textStyle:{fontSize:'11'}},colors: ['blue', 'red', 'green','blue', 'red', 'green'], 'chartArea': {top:5}, hAxis: { textStyle:{fontSize:'9'}},vAxis: { title: 'Aircraft Value ($k)'} , series: {    0: { lineWidth: 0, pointSize: 3  } ,  1: { lineWidth: 0, pointSize: 3  } ,  2: { lineWidth: 0, pointSize: 3  } ,  3: { lineWidth: 0, pointSize: 7,  pointShape: 'star', visibleInLegend: false  } ,  4: { lineWidth: 0, pointSize: 7,  pointShape: 'star', visibleInLegend: false  } ,  5: { lineWidth: 0, pointSize: 7,  pointShape: 'star', visibleInLegend: false  }  } };"
            temp_string &= "; var options1 = {'title':'', "
            If results_table.Rows.Count > 35 Then
                temp_string &= ("chartArea:{width:'95%',height:'85%'}, sliceVisibilityThreshold:'0', pieResidueSliceLabel:'Other', legend:'left', legendFontSize:12 ")
            Else
                temp_string &= ("chartArea:{width:'95%',height:'85%'}, legend:'left', legendFontSize:12 ")
            End If
            temp_string &= " }; "
            temp_string &= "var chart1 = new google.visualization.PieChart(document.getElementById('visualization" + graphID.ToString + "'));chart1.draw(data1, options1);}});"
            temp_string &= "</script>"

            If Trim(temp_string) = "" Then
                temp_string = "<script type=""text/javascript"">google.charts.setOnLoadCallback(function() {drawChart1();function drawChart1() {var data1 = new google.visualization.DataTable(); "

                temp_string = "data1.addColumn('string', 'Serial#');  "
                temp_string = "data1.addColumn('number', 'Asking'); "
                temp_string = " data1.addColumn('number', 'Take'); "
                temp_string = " data1.addColumn('number', 'Est/Sold Value'); "
                temp_string = " data1.addColumn('number', 'My AC Asking'); "
                temp_string = " data1.addColumn('number', 'My AC Take'); "
                temp_string = " data1.addColumn('number', 'My AC Est Value'); "
                temp_string = " data1.addRows(["
                temp_string = "['6/13/1996',  840, 800, 835, null, null, null],['6/2/2003',  1100, null, null, null, null, null],"
                temp_string = "['6/4/2010',  870, 850, 860, null, null, null],['9/4/2014', null, null, null,  795, null, null]"
                temp_string = "]);"
                temp_string = "var options1 = {"
                temp_string = "'title':'','width':500,curveType:  'function','height':250,"
                temp_string = "legend: { position: 'right', textStyle:{fontSize:'11'}},"
                temp_string = "colors: ['blue', 'red', 'green','blue', 'red', 'green'], 'chartArea': {top:5}, "
                temp_string = "hAxis: { textStyle:{fontSize:'9'}},"
                temp_string = "vAxis: { title: 'Aircraft Value ($k)'} "
                temp_string = ", series: {    0: { lineWidth: 0, pointSize: 3  } ,  1: { lineWidth: 0, pointSize: 3  } ,  2: { lineWidth: 0, pointSize: 3  } ,  3: { lineWidth: 0, pointSize: 7,  pointShape: 'star', visibleInLegend: false  } ,  4: { lineWidth: 0, pointSize: 7,  pointShape: 'star', visibleInLegend: false  } ,  5: { lineWidth: 0, pointSize: 7,  pointShape: 'star', visibleInLegend: false  }  }"
                temp_string = " };"
                temp_string = "var chart1 = new google.visualization.LineChart(document.getElementById('visualization" + graphID.ToString + "'));chart1.draw(data1, options1);}});"
                temp_string &= "</script>"""
            End If

            label_script.ID = "label_script"
            label_script.Text = temp_string


            If IsNothing(temp_panel) Then ' page1.ClientScript.IsClientScriptBlockRegistered("GoogleChart1Tab") Then
                GoogleChart1TabScript.Append(temp_string)
                System.Web.UI.ScriptManager.RegisterStartupScript(page1, page1.GetType(), "GoogleChart1TabStart", GoogleChart1TabScript.ToString, False)
            Else
                GoogleChart1TabScript.Append(temp_string)
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(temp_panel, page1.GetType(), "GoogleChart1Tab", GoogleChart1TabScript.ToString, False)

            End If



            If Not String.IsNullOrEmpty(temp_string) Then
                htmlOut.Append("<table width='100%' height='400' cellpadding='2' cellspacing='0' class='module'>")
                htmlOut.Append("<tr><td valign='middle' align='center' class='header'>OPERATOR COUNTRY SUMMARY</td></tr>")
                htmlOut.Append("<tr><td valign='top' align='left' class='border_bottom_right'><div id='visualization" + graphID.ToString + "' name='visualization" + graphID.ToString + "' style='text-align:center; width:100%; height:400px;'></div></td></tr></table>")
            Else
                htmlOut.Append("<table width='100%' height='400' cellpadding='2' cellspacing='0' class='module'>")
                htmlOut.Append("<tr><td valign='middle' align='center' class='header'>OPERATOR COUNTRY SUMMARY</td></tr>")
                htmlOut.Append("<tr><td valign='top' align='left' class='border_bottom_right'><div style='text-align:center; width:100%; height:400px;'>No Data to display</div></td></tr></table>")
            End If

        Catch ex As Exception

            aError = "Error in vieews_display_operator_piechart(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByVal graphID As Integer) " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub

    Public Function get_all_operator_models_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable
        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            If searchCriteria.ViewCriteriaAmodID = -1 And searchCriteria.ViewCriteriaCompanyID = 0 Then
                sQuery.Append("SELECT TOP 250 amod_make_name, amod_model_name, amod_id, count(distinct a.ac_id) AS account")
            Else
                sQuery.Append("SELECT amod_make_name, amod_model_name, amod_id, count(distinct a.ac_id) AS account")
            End If
            sQuery.Append(", sum(case when ac_lease_flag = 'Y' then 1 else 0 end) as LeaseCount ")
            sQuery.Append(" FROM Aircraft_Summary a WITH(NOLOCK) ")
            sQuery.Append(" WHERE ac_lifecycle_stage = 3 AND cref_operator_flag IN ('Y', 'O') ")

            If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaEngineName) And Not searchCriteria.ViewCriteriaEngineName.ToLower.Trim.Contains("all") Then
                sQuery.Append(Constants.cAndClause + "ac_engine_name = '" + searchCriteria.ViewCriteriaEngineName.Trim + "'")
            End If

            If searchCriteria.ViewCriteriaCompanyID > 0 Then
                sQuery.Append(Constants.cAndClause + "comp_id  = " + searchCriteria.ViewCriteriaCompanyID.ToString)
            End If

            If searchCriteria.ViewCriteriaAmodID > -1 Then
                sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
            ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
                sQuery.Append(Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
            ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftType.Trim) Then
                sQuery.Append(Constants.cAndClause + "amod_type_code = '" + searchCriteria.ViewCriteriaAircraftType.Trim + "'")
            End If

            Select Case CInt(searchCriteria.ViewCriteriaAirframeType)
                Case Constants.VIEW_EXECUTIVE
                    sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'E'")
                Case Constants.VIEW_JETS
                    sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'J'")
                Case Constants.VIEW_TURBOPROPS
                    sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'T'")
                Case Constants.VIEW_PISTONS
                    sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'P'")
                Case Constants.VIEW_HELICOPTERS
                    sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='R' AND amod_type_code in ('T','P')")
            End Select

            If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
                sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))
            Else
                sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, False))
            End If

            sQuery.Append(" GROUP BY amod_make_name, amod_model_name, amod_id")
            sQuery.Append(" ORDER BY count(distinct ac_id) desc, amod_make_name ASC")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_all_operator_models_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
                aError = "Error in get_all_operator_models_info load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_all_operator_models_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

    Public Sub views_display_operator_all_models(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByVal from_spot As String)

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder

        Dim toggleRowColor As Boolean = False

        Dim sTmpTitle As String = ""
        Dim sTitle As String = ""
        Dim nColspan As Integer = 0

        Dim inservicetot As Integer = 0
        Dim leasedtot As Integer = 0
        Dim retiredtot As Integer = 0
        Dim retiredtot2 As Integer = 0
        Dim onordertot As Integer = 0
        Dim total As Integer = 0
        Dim total_op As Long = 0
        Dim total_lease As Long = 0

        If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_COMMERCIAL Then
            nColspan = 7
        Else
            nColspan = 3
        End If

        Try

            results_table = get_all_operator_models_info(searchCriteria)

            If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaEngineName.Trim) And Not searchCriteria.ViewCriteriaEngineName.ToLower.Contains("all") Then

                If searchCriteria.ViewCriteriaCompanyID > 0 Then
                    If String.IsNullOrEmpty(sTmpTitle.Trim) Then
                        sTmpTitle = commonEvo.get_company_name_fromID(searchCriteria.ViewCriteriaCompanyID, 0, False, True, "")
                    Else
                        sTmpTitle += " - " + commonEvo.get_company_name_fromID(searchCriteria.ViewCriteriaCompanyID, 0, False, True, "")
                    End If
                End If

                If searchCriteria.ViewCriteriaAmodID > -1 Then
                    If String.IsNullOrEmpty(sTmpTitle.Trim) Then
                        sTmpTitle = commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaAmodID, False, "")
                    Else
                        sTmpTitle += " - " + commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaAmodID, False, "")
                    End If
                ElseIf searchCriteria.ViewCriteriaMakeAmodID > -1 Then
                    If String.IsNullOrEmpty(sTmpTitle.Trim) Then
                        sTmpTitle = commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaMakeAmodID, True, "")
                    Else
                        sTmpTitle += " / " + commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaMakeAmodID, True, "")
                    End If
                End If

                sTitle = "MODEL SUMMARY : " + sTmpTitle + searchCriteria.ViewCriteriaEngineName.Trim + " Engine"

            Else

                If searchCriteria.ViewCriteriaAmodID = -1 And searchCriteria.ViewCriteriaCompanyID = 0 And searchCriteria.ViewCriteriaMakeAmodID = -1 Then
                    sTitle = "MODEL SUMMARY : TOP 250 MODELS"
                ElseIf searchCriteria.ViewCriteriaMakeAmodID > -1 Then
                    sTitle = "MODEL SUMMARY : " + commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaMakeAmodID, True, "")
                ElseIf searchCriteria.ViewCriteriaAmodID > -1 Then
                    sTitle = "MODEL SUMMARY : " + commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaAmodID, False, "")
                ElseIf searchCriteria.ViewCriteriaCompanyID > 0 Then
                    sTitle = "MODEL SUMMARY :  " + commonEvo.get_company_name_fromID(searchCriteria.ViewCriteriaCompanyID, 0, False, True, "")
                End If

            End If

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then


                    If Trim(from_spot) = "Company" Then
                        htmlOut.Append("<table id='operatorModelsInnerTable' width='100%' cellpadding='2' cellspacing='0' class='data_aircraft_grid'>")
                        htmlOut.Append("<tr class='header_row'>")
                    Else
                        htmlOut.Append("<div valign=""top"" style='height:370px; overflow: auto;'>")
                        htmlOut.Append("<table id='operatorModelsInnerTable' width='100%' cellpadding='2' cellspacing='0' class='module'>")
                        htmlOut.Append("<tr><td align=""center"" valign=""middle"" class=""header"" colspan=""" + nColspan.ToString + """>" + sTitle + "</td></tr>")
                        htmlOut.Append("<tr>")
                    End If




                    htmlOut.Append("<td valign='middle' align='left' width='60%' class='seperator'><strong>Model</strong></td>")

                    If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_COMMERCIAL Then
                        htmlOut.Append("<td valign='middle' align='center' class='seperator'><strong>Order</strong></td>")
                    End If

                    htmlOut.Append("<td valign='middle' align='center' class='seperator'><strong>Operation</strong></td>")
                    htmlOut.Append("<td valign='middle' align='center' class='seperator'><strong>Leased</strong></td>")

                    If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_COMMERCIAL Then
                        htmlOut.Append("<td valign='middle' align='center' class='seperator'><strong>Stored</strong></td>")
                        htmlOut.Append("<td valign='middle' align='center' class='seperator'><strong>Retired</strong></td>")
                        htmlOut.Append("<td valign='middle' align='right' class='seperator' style='padding-right:3px;'><strong>Total</strong></td>")
                    End If

                    htmlOut.Append("</tr>")

                    For Each r As DataRow In results_table.Rows

                        inservicetot = 0
                        leasedtot = 0
                        retiredtot = 0
                        retiredtot2 = 0
                        onordertot = 0
                        total = 0

                        If Not toggleRowColor Then
                            htmlOut.Append("<tr class='alt_row'>")
                            toggleRowColor = True
                        Else
                            htmlOut.Append("<tr bgcolor='white'>")
                            toggleRowColor = False
                        End If

                        inservicetot = CInt(r.Item("account").ToString)

                        searchCriteria.ViewTempCompanyID = searchCriteria.ViewCriteriaCompanyID
                        searchCriteria.ViewTempAmodID = CLng(r.Item("amod_id").ToString)


                        If Not IsDBNull(r.Item("LeaseCount")) Then
                            leasedtot = CLng(r.Item("LeaseCount").ToString)
                        Else
                            leasedtot = 0
                        End If
                        ' leasedtot = get_count_totals_ac_table(searchCriteria, "leased", True)

                        If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_COMMERCIAL Then
                            retiredtot2 = get_count_totals_ac_table(searchCriteria, "storage", True)
                            retiredtot = get_count_totals_ac_table(searchCriteria, "retired", True)
                            onordertot = get_count_totals_ac_table(searchCriteria, "order", True)
                            retiredtot = retiredtot - retiredtot2
                        End If

                        total = inservicetot + onordertot + retiredtot + retiredtot2

                        htmlOut.Append("<td valign='top' align='left' class='border_bottom_right' width='60%'>")


                        If Trim(from_spot) = "Company" Then
                            htmlOut.Append("<a class=""underline cursor"" href='DisplayCompanyDetail.aspx?compid=" + searchCriteria.ViewCriteriaCompanyID.ToString + "&amod_id=" + r.Item("amod_id").ToString + "&use_insight=Y' title='Show operator details for this make/model' onclick=""ChangeTheMouseCursorOnItemParentDocument('cursor_wait')"">")
                        Else
                            If searchCriteria.ViewCriteriaCompanyID > 0 Then
                                htmlOut.Append("<a class=""underline cursor"" href='view_template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName + "&viewCompany=" + searchCriteria.ViewCriteriaCompanyID.ToString + "&amod_id=" + r.Item("amod_id").ToString + "' title='Show operator details for this make/model'>")
                            Else
                                htmlOut.Append("<a class=""underline cursor"" href='view_template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName + "&amod_id=" + r.Item("amod_id").ToString + "' title='Show operator details for this make/model'>")
                            End If
                        End If


                        htmlOut.Append(r.Item("amod_make_name").ToString.Trim + " " + r.Item("amod_model_name").ToString.Trim + "</a>")

                        htmlOut.Append("</td>")

                        If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_COMMERCIAL Then
                            htmlOut.Append("<td align='right' class='border_bottom_right'>" + FormatNumber(onordertot, TriState.False, TriState.True, TriState.False, TriState.True).ToString + "</td>")
                        End If

                        htmlOut.Append("<td align='right' class='border_bottom_right'>" + FormatNumber(inservicetot, TriState.False, TriState.True, TriState.False, TriState.True).ToString + "</td>")
                        htmlOut.Append("<td align='right' class='border_bottom_right'>" + FormatNumber(leasedtot, TriState.False, TriState.True, TriState.False, TriState.True).ToString + "</td>")

                        total_op = total_op + CLng(inservicetot)
                        total_lease = total_lease + CLng(leasedtot)

                        If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_COMMERCIAL Then
                            htmlOut.Append("<td align='right' class='border_bottom_right'>" + FormatNumber(retiredtot2, TriState.False, TriState.True, TriState.False, TriState.True).ToString + "</td>")
                            htmlOut.Append("<td align='right' class='border_bottom_right'>" + FormatNumber(retiredtot, TriState.False, TriState.True, TriState.False, TriState.True).ToString + "</td>")
                            htmlOut.Append("<td align='right' class='border_bottom_right'>" + FormatNumber(total, TriState.False, TriState.True, TriState.False, TriState.True).ToString + "</td>")
                        End If

                        htmlOut.Append("</tr>")

                    Next

                    If Trim(from_spot) = "Company" And searchCriteria.ViewCriteriaAmodID = -1 Then
                        If Not toggleRowColor Then
                            htmlOut.Append("<tr class='alt_row'>")
                            toggleRowColor = True
                        Else
                            htmlOut.Append("<tr bgcolor='white'>")
                            toggleRowColor = False
                        End If
                        htmlOut.Append("<td valign='top' align='right' class='border_bottom_right' width='60%'><strong>Totals</strong></td>")
                        If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_COMMERCIAL Then
                            htmlOut.Append("<td align='right' class='border_bottom_right'>&nbsp;</td>")
                        End If

                        htmlOut.Append("<td align='right' class='border_bottom_right'><strong>" & total_op & "</strong></td>")
                        htmlOut.Append("<td align='right' class='border_bottom_right'><strong>" & total_lease & "</strong></td>")

                        If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_COMMERCIAL Then
                            htmlOut.Append("<td align='right' class='border_bottom_right'>&nbsp;</td>")
                            htmlOut.Append("<td align='right' class='border_bottom_right'>&nbsp;</td>")
                            htmlOut.Append("<td align='right' class='border_bottom_right'>&nbsp;</td>")
                        End If

                        htmlOut.Append("</tr>")
                    End If

                    htmlOut.Append("</table>")

                    If Trim(from_spot) = "Company" Then
                    Else
                        htmlOut.Append("</div>")
                    End If


                    ' clear temp amod id on each loop
                    ' clear temp comp id on each loop
                    ' both to be (0) zero when not being used
                    searchCriteria.ViewTempCompanyID = 0
                    searchCriteria.ViewTempAmodID = 0

                Else
                    htmlOut.Append("<table id='operatorModelsInnerTable' width='100%' cellpadding='2' cellspacing='0' class='module'><tr><td valign=""top"" align=""left"" class=""border_bottom_right""><br/>No Operator Data Available</td></tr></table>")
                End If
            Else
                htmlOut.Append("<table id='operatorModelsInnerTable' width='100%' cellpadding='2' cellspacing='0' class='module'><tr><td valign=""top"" align=""left"" class=""border_bottom_right""><br/>No Operator Data Available</td></tr></table>")
            End If

        Catch ex As Exception

            aError = "Error in views_display_all_operator_models(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        out_htmlString = htmlOut.ToString()
        htmlOut = Nothing
        results_table = Nothing

    End Sub

    Public Function get_count_totals_ac_table(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal fieldToTotal As String, ByVal only_count_operator As Boolean) As Integer

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Dim current_total As Integer = 0

        Try

            sQuery.Append("SELECT count(distinct ac_id) as account FROM Aircraft_Summary")
            sQuery.Append(" WHERE")

            Select Case (fieldToTotal.ToLower.Trim)

                Case "leased"
                    sQuery.Append(" ac_lease_flag = 'Y'")
                    If only_count_operator Then
                        sQuery.Append(Constants.cAndClause + "cref_operator_flag IN ('Y', 'O')")
                    Else
                        sQuery.Append(Constants.cAndClause + "ac_lifecycle_stage IN ('3')")
                    End If
                Case "storage"
                    sQuery.Append(" (ac_lifecycle_stage IN ('4'))")
                    sQuery.Append(Constants.cAndClause + "((cref_operator_flag IN ('Y', 'O')) OR cref_contact_type IN ('42','56'))")
                    sQuery.Append(Constants.cAndClause + "(ac_status = 'Withdrawn from Use - Stored')")
                Case "retired"
                    sQuery.Append(" (ac_lifecycle_stage IN ('4'))")
                    sQuery.Append(Constants.cAndClause + "(cref_operator_flag IN ('Y', 'O') OR cref_contact_type IN ('42','56'))")
                Case "operation"
                    sQuery.Append(" ac_lifecycle_stage IN ('3')")
                    If only_count_operator Then
                        sQuery.Append(Constants.cAndClause + "cref_operator_flag IN ('Y', 'O')")
                    End If
                Case "order"
                    sQuery.Append(" (ac_lifecycle_stage IN ('1','2') AND (cref_contact_type in ('42') OR cref_operator_flag IN ('Y', 'O')))")
            End Select


            If searchCriteria.ViewTempAmodID > 0 Then ' use TempAmodID (for loop searches)
                sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewTempAmodID.ToString)
            ElseIf searchCriteria.ViewCriteriaAmodID > 0 Then
                sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
            ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
                sQuery.Append(Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
            ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftType.Trim) Then
                sQuery.Append(Constants.cAndClause + "amod_type_code = '" + searchCriteria.ViewCriteriaAircraftType.Trim + "'")
            End If

            Select Case CInt(searchCriteria.ViewCriteriaAirframeType)
                Case Constants.VIEW_EXECUTIVE
                    sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'E'")
                Case Constants.VIEW_JETS
                    sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'J'")
                Case Constants.VIEW_TURBOPROPS
                    sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'T'")
                Case Constants.VIEW_PISTONS
                    sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'P'")
                Case Constants.VIEW_HELICOPTERS
                    sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='R' AND amod_type_code in ('T','P')")
            End Select

            If searchCriteria.ViewTempCompanyID > 0 Then ' use TempCompanyID (for loop searches)
                sQuery.Append(Constants.cAndClause + "comp_id = " + searchCriteria.ViewTempCompanyID.ToString)
            End If

            If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
                sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))
            Else
                sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, False))
            End If

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_count_totals_ac_table(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal fieldToTotal As String, ByVal only_count_operator As Boolean) As Integer</b><br />" + sQuery.ToString

            SqlConn.ConnectionString = clientConnectString
            SqlConn.Open()
            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 60

            SqlCommand.CommandText = sQuery.ToString
            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                atemptable.Load(SqlReader)

                If Not IsNothing(atemptable) Then
                    If atemptable.Rows.Count > 0 Then
                        For Each r As DataRow In atemptable.Rows
                            If Not IsDBNull(r("account")) Then
                                current_total += CInt(r.Item("account").ToString)
                            End If
                        Next
                    End If
                End If

                atemptable = Nothing

            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                aError = "Error in get_count_totals_ac_table load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return 0

            aError = "Error in get_count_totals_ac_table(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal fieldToTotal As String, ByVal only_count_operator As Boolean) As Integer " + ex.Message

        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing

            SqlCommand.Dispose()
            SqlCommand = Nothing
        End Try

        Return current_total

    End Function

    Public Sub views_display_operator_view_summary(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal in_total As Double, ByVal in_Service As Double,
                                                 ByVal in_onOrder As Double, ByVal in_onLease As Double,
                                                 ByVal in_retired As Double, ByVal retiredtot2 As Double,
                                                 ByVal only_count_operator As Boolean, ByRef out_htmlString As String)

        Dim htmlOut As New StringBuilder
        Dim nColspan As Integer = 0
        Dim size_of As Integer = 185

        Try

            htmlOut.Append("<br /><table id='operatorModelsSummaryTable' width='100%' cellspacing='0' cellpadding='2' class='module'>")

            If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_COMMERCIAL Then
                nColspan = 7
            Else
                nColspan = 3
            End If

            If only_count_operator Then
                htmlOut.Append("<tr><td align='left' colspan='" + nColspan.ToString + "' valign='middle' class='header'>DETAILS : </td></tr>")
            Else
                ' htmlOut.Append("<tr><td align='left' colspan='" + nColspan.ToString + "' valign='middle' class='header'>TOTALS <em>( With or Without Operators )</em></td></tr>")
            End If

            htmlOut.Append("<tr>")

            If only_count_operator Then
                htmlOut.Append("<td width='205' class='border_bottom_right'><strong>Totals</strong></td>")
            Else
                If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_COMMERCIAL Then
                    htmlOut.Append("<td width='45%' class='rightside' align='right'><strong>TOTALS</strong>&nbsp;</td>")
                Else
                    htmlOut.Append("<td width='75%' class='rightside' align='right'><strong>TOTALS</strong>&nbsp;</td>")
                End If
            End If

            If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_COMMERCIAL Then
                htmlOut.Append("<td valign='top' width='40' align='center' class='border_bottom_right'>" + FormatNumber(in_onOrder, TriState.False, TriState.True, TriState.False, TriState.True).ToString + "</td>")
            End If

            htmlOut.Append("<td valign='top' width='40' align='center' class='border_bottom_right'>" + FormatNumber(in_Service, TriState.False, TriState.True, TriState.False, TriState.True).ToString + "</td>")

            If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_COMMERCIAL Then
                htmlOut.Append("<td valign='middle' width='35' align='center' class='border_bottom_right'>" + FormatNumber(in_onLease, TriState.False, TriState.True, TriState.False, TriState.True).ToString + "</td>")
                htmlOut.Append("<td valign='middle' width='40' align='center' class='border_bottom_right'>" + FormatNumber(retiredtot2, TriState.False, TriState.True, TriState.False, TriState.True).ToString + "</td>")
                htmlOut.Append("<td valign='middle' width='40' align='center' class='border_bottom_right'>" + FormatNumber(in_retired, TriState.False, TriState.True, TriState.False, TriState.True).ToString + "</td>")
                htmlOut.Append("<td valign='middle' width='35' align='center' class='border_bottom'>" + FormatNumber(in_total, TriState.False, TriState.True, TriState.False, TriState.True).ToString + "</td>")
            Else
                htmlOut.Append("<td valign='middle' width='35' align='center' class='border_bottom'>" + FormatNumber(in_onLease, TriState.False, TriState.True, TriState.False, TriState.True).ToString + "</td>")
            End If

            htmlOut.Append("</tr><tr>")

            If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_COMMERCIAL Then
                htmlOut.Append("<td width='45%' class='rightside'>&nbsp;</td>")
            Else
                htmlOut.Append("<td width='75%' class='rightside'>&nbsp;</td>")
            End If

            If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_COMMERCIAL Then
                htmlOut.Append("<td valign='middle' width='40' align='center' class='border_bottom_right'><strong>Order</strong></td>")
            End If

            htmlOut.Append("<td valign='middle' width='40' align='center' class='border_bottom_right'><strong>Operation</strong></td>")

            If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_COMMERCIAL Then
                htmlOut.Append("<td valign='middle' width='35' align='center' class='border_bottom_right'><strong>Leased</strong></td>")
                htmlOut.Append("<td valign='middle' width='40' align='center' class='border_bottom_right'><strong>Storage</strong></td>")
                htmlOut.Append("<td valign='middle' width='40' align='center' class='border_bottom_right'><strong>Retired</strong></td>")
                htmlOut.Append("<td valign='middle' width='35' align='center' bgcolor='#eeeeee' class='border_bottom'><strong>Total</strong></td>")
            Else
                htmlOut.Append("<td valign='middle' width='35' align='center' class='border_bottom'><strong>Leased</strong></td>")
            End If

            htmlOut.Append("</tr><tr>")

            htmlOut.Append("<tr><td colspan='" + nColspan.ToString + "' valign='middle' align='center'>")

            If searchCriteria.ViewCriteriaAmodID > -1 Then
                htmlOut.Append("<b>Click the Model Name above to view additional details regarding the model</b>")
            Else
                htmlOut.Append("<b>Click Operator Name to View Aircraft Operations</b>")
            End If

            htmlOut.Append("</td></tr></table>")
        Catch ex As Exception

            aError = "Error in views_display_operator_view_summary(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        out_htmlString = htmlOut.ToString()
        htmlOut = Nothing

    End Sub

    Public Function get_operator_aircraft_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable
        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            sQuery.Append("SELECT DISTINCT ac_id, ac_ser_no_full, ac_reg_no, ac_engine_name,")
            sQuery.Append(" amod_type_code, amod_make_name, amod_model_name, comp_name, comp_country, comp_id")
            sQuery.Append(" FROM Aircraft_Summary a WITH(NOLOCK) ")
            sQuery.Append(" WHERE ac_lifecycle_stage = 3 AND cref_operator_flag IN ('Y', 'O') ")

            If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaEngineName.Trim) And Not searchCriteria.ViewCriteriaEngineName.ToLower.Trim.Contains("all") Then
                sQuery.Append(Constants.cAndClause + "ac_engine_name = '" + searchCriteria.ViewCriteriaEngineName.Trim + "'")
            End If

            If searchCriteria.ViewCriteriaCompanyID > 0 Then
                sQuery.Append(Constants.cAndClause + "comp_id  = " + searchCriteria.ViewCriteriaCompanyID.ToString)
            End If

            If searchCriteria.ViewCriteriaAmodID > -1 Then
                sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
            ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
                sQuery.Append(Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
            ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftType.Trim) Then
                sQuery.Append(Constants.cAndClause + "amod_type_code = '" + searchCriteria.ViewCriteriaAircraftType.Trim + "'")
            End If

            Select Case CInt(searchCriteria.ViewCriteriaAirframeType)
                Case Constants.VIEW_EXECUTIVE
                    sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'E'")
                Case Constants.VIEW_JETS
                    sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'J'")
                Case Constants.VIEW_TURBOPROPS
                    sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'T'")
                Case Constants.VIEW_PISTONS
                    sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'P'")
                Case Constants.VIEW_HELICOPTERS
                    sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='R' AND amod_type_code in ('T','P')")
            End Select

            If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
                sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))
            Else
                sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, False))
            End If

            sQuery.Append(" ORDER BY amod_type_code, amod_make_name, amod_model_name, ac_id, ac_ser_no_full, ac_reg_no")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_operator_aircraft_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
                aError = "Error in get_operator_aircraft_info load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_operator_aircraft_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

    Public Sub views_display_operator_aircraft(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String)

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder

        Dim toggleRowColor As Boolean = False

        Dim sTmpTitle As String = ""
        Dim sTitle As String = ""

        Try

            results_table = get_operator_aircraft_info(searchCriteria)

            If searchCriteria.ViewCriteriaCompanyID > 0 Then
                If String.IsNullOrEmpty(sTmpTitle.Trim) Then
                    sTmpTitle = commonEvo.get_company_name_fromID(searchCriteria.ViewCriteriaCompanyID, 0, False, True, "")
                Else
                    sTmpTitle += " - " + commonEvo.get_company_name_fromID(searchCriteria.ViewCriteriaCompanyID, 0, False, True, "")
                End If
            End If

            If searchCriteria.ViewCriteriaAmodID > -1 Then
                If String.IsNullOrEmpty(sTmpTitle.Trim) Then
                    sTmpTitle = commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaAmodID, False, "")
                Else
                    sTmpTitle += " - " + commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaAmodID, False, "")
                End If
            ElseIf searchCriteria.ViewCriteriaMakeAmodID > -1 Then
                If String.IsNullOrEmpty(sTmpTitle.Trim) Then
                    sTmpTitle = commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaMakeAmodID, True, "")
                Else
                    sTmpTitle += " - " + commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaMakeAmodID, True, "")
                End If
            End If

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    sTitle = sTmpTitle + " : AIRCRAFT&nbsp;<em>(" + results_table.Rows.Count.ToString + ")</em>"

                    If results_table.Rows.Count > 15 Then
                        htmlOut.Append("<div valign=""top"" style='height:370px; overflow: auto;'>")
                    End If

                    htmlOut.Append("<table id='operatorAircraftInnerTable' width='100%' cellpadding='2' cellspacing='0' class='module'>")
                    htmlOut.Append("<tr><td align=""center"" valign=""middle"" class=""header"" colspan=""2"">" + sTitle + "</td></tr>")

                    For Each r As DataRow In results_table.Rows

                        If Not toggleRowColor Then
                            htmlOut.Append("<tr class='alt_row'>")
                            toggleRowColor = True
                        Else
                            htmlOut.Append("<tr bgcolor='white'>")
                            toggleRowColor = False
                        End If

                        htmlOut.Append("<td align='left' valign='top' class='seperator'><img src='images/ch_red.jpg' class='bullet' alt='acid : " + r.Item("ac_id").ToString + "' /></td>")
                        htmlOut.Append("<td align='left' valign='middle' class='seperator'> S/N: <a class='underline' href=""DisplayAircraftDetail.aspx?acid=" + r.Item("ac_id").ToString + "&jid=0"" target=""_blank"" title='Display Aircraft Details'>")
                        htmlOut.Append(r.Item("ac_ser_no_full").ToString + "</a>")

                        htmlOut.Append(", Reg# " + r.Item("ac_reg_no").ToString & " ")

                        If searchCriteria.ViewCriteriaAmodID > -1 Then
                        Else
                            htmlOut.Append(r.Item("amod_make_name").ToString + " / " + r.Item("amod_model_name").ToString + ", ")
                        End If

                        htmlOut.Append(r.Item("ac_engine_name").ToString + " Engine(s) ")


                        'If adors("ac_engine_noise_rating").value > 0 Then
                        '  htmlOut.Append("<em>" & adors("ac_engine_noise_rating").value & " rating</em>")
                        'End If

                        If searchCriteria.ViewCriteriaCompanyID = 0 Then
                            htmlOut.Append("<br /><a class='underline' href=""DisplayCompanyDetail.aspx?compid=" + r.Item("comp_id").ToString + "&journid=0"" target=""_blank"" title='Display Company Details'>")
                            htmlOut.Append(Replace(r.Item("comp_name").ToString, Constants.cSingleSpace, Constants.cHTMLnbsp) + "</a> <em>(" + r.Item("comp_country").ToString.Trim + ")</em>")
                        End If

                        htmlOut.Append("</td></tr>")

                    Next

                    htmlOut.Append("</table>")

                    If results_table.Rows.Count > 15 Then
                        htmlOut.Append("</div>")
                    End If

                Else
                    htmlOut.Append("<table id='operatorAircraftInnerTable' width='100%' cellpadding='2' cellspacing='0' class='module'><tr><td valign=""top"" align=""left"" class=""border_bottom_right""><br/>No Operator Data Available</td></tr></table>")
                End If
            Else
                htmlOut.Append("<table id='operatorAircraftInnerTable' width='100%' cellpadding='2' cellspacing='0' class='module'><tr><td valign=""top"" align=""left"" class=""border_bottom_right""><br/>No Operator Data Available</td></tr></table>")
            End If

        Catch ex As Exception

            aError = "Error in views_display_operator_aircraft(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        out_htmlString = htmlOut.ToString()
        htmlOut = Nothing
        results_table = Nothing

    End Sub

    Public Function get_operator_engine_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable
        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            sQuery.Append("SELECT DISTINCT ac_engine_name, ameng_mfr_name, count(*) as tcount")
            sQuery.Append(" FROM Aircraft WITH(NOLOCK)")
            sQuery.Append(" INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id")
            sQuery.Append(" LEFT OUTER JOIN Aircraft_Reference WITH(NOLOCK) ON ac_id = cref_ac_id and ac_journ_id = cref_journ_id and cref_operator_flag IN ('Y', 'O')")
            sQuery.Append(" LEFT OUTER JOIN company WITH(NOLOCK) ON comp_id = cref_comp_id and comp_journ_id = cref_journ_id")
            sQuery.Append(" INNER JOIN Aircraft_Model_Engine WITH(NOLOCK) ON ameng_engine_name = ac_engine_name")

            If searchCriteria.ViewCriteriaAmodID > -1 Then
                sQuery.Append(Constants.cAndClause + "ameng_amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
            End If

            sQuery.Append(" WHERE ac_journ_id = 0 AND ac_product_commercial_flag = 'Y'")
            sQuery.Append(Constants.cAndClause + "ac_lifecycle_stage = 3 AND ac_engine_name IS NOT NULL ")

            If searchCriteria.ViewCriteriaAmodID > -1 Then
                sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
            ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
                sQuery.Append(Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
            ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftType.Trim) Then
                sQuery.Append(Constants.cAndClause + "amod_type_code = '" + searchCriteria.ViewCriteriaAircraftType.Trim + "'")
            End If

            Select Case CInt(searchCriteria.ViewCriteriaAirframeType)
                Case Constants.VIEW_EXECUTIVE
                    sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'E'")
                Case Constants.VIEW_JETS
                    sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'J'")
                Case Constants.VIEW_TURBOPROPS
                    sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'T'")
                Case Constants.VIEW_PISTONS
                    sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'P'")
                Case Constants.VIEW_HELICOPTERS
                    sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='R' AND amod_type_code in ('T','P')")
            End Select

            If searchCriteria.ViewCriteriaCompanyID > 0 Then
                sQuery.Append(Constants.cAndClause + "comp_id  = " + searchCriteria.ViewCriteriaCompanyID.ToString)
            End If

            If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaEngineName.Trim) And Not searchCriteria.ViewCriteriaEngineName.ToLower.Trim.Contains("all") Then
                sQuery.Append(Constants.cAndClause + "ac_engine_name = '" + searchCriteria.ViewCriteriaEngineName.Trim + "'")
            End If

            If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
                sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))
            Else
                sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, False))
            End If

            sQuery.Append(" GROUP BY ac_engine_name, ameng_mfr_name")
            sQuery.Append(" ORDER BY count(*) desc")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_operator_engine_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
                aError = "Error in get_operator_engine_info load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_operator_engine_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

    Public Sub display_operator_engine_info(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String)

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder

        Dim toggleRowColor As Boolean = False

        Dim sTmpTitle As String = ""
        Dim sTitle As String = ""

        Try

            results_table = get_operator_engine_info(searchCriteria)

            If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaEngineName.Trim) And Not searchCriteria.ViewCriteriaEngineName.ToLower.Contains("all") Then
                sTmpTitle = searchCriteria.ViewCriteriaEngineName.Trim + " Engine"
            Else
                sTmpTitle = "ALL Engines"
            End If

            If searchCriteria.ViewCriteriaAmodID > -1 Then
                If String.IsNullOrEmpty(sTmpTitle.Trim) Then
                    sTmpTitle = commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaAmodID, False, "")
                Else
                    sTmpTitle += " - " + commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaAmodID, False, "")
                End If
            ElseIf searchCriteria.ViewCriteriaMakeAmodID > -1 Then
                If String.IsNullOrEmpty(sTmpTitle.Trim) Then
                    sTmpTitle = commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaMakeAmodID, True, "")
                Else
                    sTmpTitle += " - " + commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaMakeAmodID, True, "")
                End If
            End If

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    sTitle = sTmpTitle + "&nbsp;<em>(" + results_table.Rows.Count.ToString + ")</em>"

                    If results_table.Rows.Count > 15 Then
                        htmlOut.Append("<div valign=""top"" style='height:370px; overflow: auto;'>")
                    End If

                    htmlOut.Append("<table id='displayEngineInfoInnerTable' cellpadding='2' cellspacing='0' class='module'>")
                    htmlOut.Append("<tr><td align=""center"" valign=""middle"" class=""header"" colspan=""3"">ENGINE SUMMARY&nbsp;:&nbsp;" + sTitle + "</td></tr>")

                    If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaEngineName.Trim) Then
                        htmlOut.Append("<tr><td align=""right"" valign=""middle"" style=""padding-right:5px;"" colspan=""3"">")
                        htmlOut.Append("<a href='view_template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName + "&viewCompany=" + searchCriteria.ViewCriteriaCompanyID.ToString + "&operatorViewEngine='>Clear Engine</a>")
                        htmlOut.Append("</td></tr>")
                    End If

                    htmlOut.Append("<tr>")
                    htmlOut.Append("<td valign='middle' align='center' class='seperator'><strong>Engine Name</strong></td>")
                    htmlOut.Append("<td valign='middle' align='center' class='seperator'><strong>Manufacturer Name</strong></td>")
                    htmlOut.Append("<td valign='middle' align='right' class='seperator' style='padding-right:5px;'><strong>Aircraft Count</strong></td>")
                    htmlOut.Append("</tr>")

                    For Each r As DataRow In results_table.Rows

                        If Not toggleRowColor Then
                            htmlOut.Append("<tr class='alt_row'>")
                            toggleRowColor = True
                        Else
                            htmlOut.Append("<tr bgcolor='white'>")
                            toggleRowColor = False
                        End If

                        htmlOut.Append("<td valign='middle' align='left' class='border_bottom_right'>")
                        htmlOut.Append("<a href='view_template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName + "&operatorViewEngine=" + HttpContext.Current.Server.UrlEncode(r.Item("ac_engine_name").ToString) + "'>")
                        htmlOut.Append(r.Item("ac_engine_name").ToString.Trim + "</td>")
                        htmlOut.Append("<td valign='middle' align='left'class='border_bottom_right'>" + r.Item("ameng_mfr_name").ToString.Trim + "</td>")
                        htmlOut.Append("<td valign='middle' align='left' class='border_bottom_right'>" + r.Item("tcount").ToString + "</td>")
                        htmlOut.Append("</tr>")

                    Next

                    htmlOut.Append("</table>")

                    If results_table.Rows.Count > 15 Then
                        htmlOut.Append("</div>")
                    End If

                Else
                    htmlOut.Append("<table id='displayEngineInfoInnerTable' width='100%' cellpadding='2' cellspacing='0' class='module'><tr><td valign=""top"" align=""left"" class=""border_bottom_right""><br/>No Operator Data Available</td></tr></table>")
                End If
            Else
                htmlOut.Append("<table id='displayEngineInfoInnerTable' width='100%' cellpadding='2' cellspacing='0' class='module'><tr><td valign=""top"" align=""left"" class=""border_bottom_right""><br/>No Operator Data Available</td></tr></table>")
            End If

        Catch ex As Exception

            aError = "Error in display_operator_engine_info(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        out_htmlString = htmlOut.ToString()
        htmlOut = Nothing

    End Sub

    Public Function get_operator_certification_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable
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

            sQuery.Append(" AND ccerttype_logo_image IS NOT NULL AND ccerttype_logo_image <> ''")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_operator_certification_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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

    Public Sub display_operator_certification_images(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String)

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder

        Dim toggleRowColor As Boolean = False

        Try

            results_table = get_operator_certification_info(searchCriteria)

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    If searchCriteria.ViewCriteriaCompanyID > 0 Then

                        htmlOut.Append("<table id=""operatorCertificateDataTable"" width=""100%"" cellpadding=""4"" cellspacing=""0"" border=""0"">")
                        htmlOut.Append("<tr><td valign=""middle"" align=""center"" class=""header"">OPERATOR CERTIFICATION(S) - <a href=""help/helpexamples/Operator-Certifications.pdf"" target=""_blank"" title=""Operator Certification Descriptions"" style=""color='white'""><font color='white'>Certification Descriptions</font></a></td></tr>")
                        htmlOut.Append("<tr><td valign=""middle"" align=""left"">")

                        ' line up certification images on the row
                        For Each r As DataRow In results_table.Rows
                            htmlOut.Append("<img width=""50"" src=""images/" + r.Item("ccerttype_logo_image").ToString.Trim + """ alt=""" + r.Item("ccerttype_type").ToString.Trim + """ title=""" + r.Item("ccerttype_type").ToString.Trim + """ /> ")
                        Next

                        htmlOut.Append("</td></tr>")

                    Else

                        htmlOut.Append("<table id=""operatorCertificateDataTable"" width=""100%"" cellpadding=""4"" cellspacing=""0"" border=""0""><tr><td valign=""middle"" align=""center"">")

                        For Each r As DataRow In results_table.Rows
                            htmlOut.Append("<img width=""25"" src=""images/" + r.Item("ccerttype_logo_image").ToString.Trim + """ alt=""" + r.Item("ccerttype_type").ToString.Trim + """ title=""" + r.Item("ccerttype_type").ToString.Trim + """ /> ")
                        Next

                        htmlOut.Append("</td></tr>")

                    End If

                Else
                    htmlOut.Append("<table id=""operatorCertificateDataTable"" width=""100%"" cellpadding=""4"" cellspacing=""0"" border=""0"">")
                    htmlOut.Append("<tr><td valign=""middle"" align=""center"" class=""header"">OPERATOR CERTIFICATION(S)</td></tr>")
                    htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""border_bottom_right""><br/>No Certificate Information Available</td></tr>")
                End If
            Else
                htmlOut.Append("<table id=""operatorCertificateDataTable"" width=""100%"" cellpadding=""4"" cellspacing=""0"" border=""0"">")
                htmlOut.Append("<tr><td valign=""middle"" align=""center"" class=""header"">OPERATOR CERTIFICATION(S)</td></tr>")
                htmlOut.Append("<tr><td valign=""top"" align=""left"" class=""border_bottom_right""><br/>No Certificate Information Available</td></tr>")
            End If

            htmlOut.Append("</table>")

        Catch ex As Exception

            aError = "Error in display_operator_certification_images(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        out_htmlString = htmlOut.ToString()
        htmlOut = Nothing
        results_table = Nothing

    End Sub

#End Region

End Class
