' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/Entity Classes/CompanyFunctions.vb $
'$$Author: Amanda $
'$$Date: 6/30/20 1:26p $
'$$Modtime: 6/30/20 11:09a $
'$$Revision: 18 $
'$$Workfile: CompanyFunctions.vb $
'
' ********************************************************************************
Public Class CompanyFunctions
    ''' <summary>
    ''' SUB Polls Database for Company Information
    ''' THIS ACCEPTS JOURNAL ID IN DATA HOOK NOW.
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub Fill_Information_Tab(ByRef information_tab As Object, ByRef information_label As Label, ByRef master As Object, ByVal CompanyID As Long, ByVal JournalID As Long, ByRef DoingBusinessAs As String, ByRef about_label As Label, ByRef about As AjaxControlToolkit.TabContainer, ByRef company_address As Label, ByVal company_name As Label, ByVal isNote As Boolean, Optional ByRef CrmVIEW As Boolean = False, Optional ByRef crmSource As String = "JETNET", Optional ByRef CrmJetnetID As Long = 0, Optional ByRef OtherID As Long = 0, Optional ByVal contactView As Boolean = False)
        Dim InfoTable As New DataTable
        Dim PhoneTable As New DataTable
        Dim Email As String = ""
        Dim Website As String = ""
        Dim LogoFlag As Boolean = False

        Dim MainLocationID As Long = 0
        Dim PerformExtraLookupMainLocation As Boolean = True
        Dim MainLocationDataTable As New DataTable
        information_label.Text = ""
        Dim rowspanCount As Integer = 1
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Main Company Query. 
        InfoTable = master.aclsData_Temp.GetCompanyInfo_ID(CompanyID, IIf(CrmVIEW = False, "JETNET", crmSource), JournalID)
        If Not IsNothing(InfoTable) Then
            If InfoTable.Rows.Count > 0 Then
                information_label.Text = "<div class=""row remove_margin"">"

                information_label.Text += "<div class=""Box"">"

                If Not IsDBNull(InfoTable.Rows(0).Item("comp_alternate_name")) Then
                    If Not String.IsNullOrEmpty(InfoTable.Rows(0).Item("comp_alternate_name")) Then
                        DoingBusinessAs = IIf(Not IsDBNull(InfoTable.Rows(0).Item("comp_alternate_name_type")), InfoTable.Rows(0).Item("comp_alternate_name_type").ToString, "") & ":" & IIf(Not IsDBNull(InfoTable.Rows(0).Item("comp_alternate_name")), InfoTable.Rows(0).Item("comp_alternate_name").ToString, "")
                    End If
                End If
                'Set the Tab Header
                If Not IsNothing(information_tab) Then
                    information_tab.HeaderText = IIf(Not IsDBNull(InfoTable.Rows(0).Item("comp_name")), InfoTable.Rows(0).Item("comp_name").ToString, "")
                Else
                    ' added MSw 
                    If contactView = True Then
                        information_label.Text += "<div class=""padding_left emphasisColor"">" & DisplayFunctions.WriteDetailsLink(0, CompanyID, 0, 0, True, IIf(Not IsDBNull(InfoTable.Rows(0).Item("comp_name")), InfoTable.Rows(0).Item("comp_name").ToString, ""), "subHeader companyContactSubheaderLink emphasisColor", IIf(crmSource = "CLIENT", "&source=CLIENT", ""))
                        ' information_label.Text += "<a " & DisplayFunctions.WriteDetailsLink(0, IIf(Not IsDBNull(InfoTable.Rows(0).Item("jetnet_comp_id")), InfoTable.Rows(0).Item("jetnet_comp_id").ToString, 0), 0, 0, False, "", "", "") & "><font size='-2'>(View Company Record)</font></a>"
                    Else
                        information_label.Text += "<div class=""subHeader padding_left emphasisColor"">" & IIf(Not IsDBNull(InfoTable.Rows(0).Item("comp_name")), InfoTable.Rows(0).Item("comp_name").ToString, "")
                    End If


                    If Not HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE And Not HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
                        If CrmVIEW = True Then 'And contactView = False Then
                            If OtherID > 0 Then
                                If crmSource <> "CLIENT" Then
                                    information_label.Text += "<span><span class=""float_right""><a " & IIf(contactView = False, "href=""/DisplayCompanyDetail.aspx?compid=" & OtherID & "&source=CLIENT""", "href='javascript:void();' " & DisplayFunctions.WriteDetailsLink(0, OtherID, 0, 0, False, "", "", "&source=CLIENT")) & ">VIEW CLIENT</a></span></span>"
                                Else
                                    information_label.Text += "<span><strong>/CLIENT RECORD</strong><span class=""float_right"">" & CreateCompanyEditLink(crmSource, CrmVIEW, CompanyID, True, False) & "<span class=""float_right pipeDelimeter"">|</span><a " & IIf(contactView = False, "href=""/DisplayCompanyDetail.aspx?compid=" & OtherID & """", "href='javascript:void();' " & DisplayFunctions.WriteDetailsLink(0, OtherID, 0, 0, False, "", "", "")) & " class=""padding_right"">VIEW JETNET</a></span></span>"
                                End If
                            ElseIf crmSource <> "CLIENT" Then
                                information_label.Text += "<span><span class=""float_right"">" & CreateCompanyEditLink(crmSource, CrmVIEW, CompanyID, False, True) & "</span></span>"
                            End If
                        End If
                    End If

                    information_label.Text += "</div>"
                End If


                company_name.Text = (IIf(Not IsDBNull(InfoTable.Rows(0).Item("comp_name")), InfoTable.Rows(0).Item("comp_name").ToString & "", " Company Details"))




                If crmSource = "CLIENT" Then
                    'PerformExtraLookupMainLocation = False
                    CrmJetnetID = IIf(Not IsDBNull(InfoTable.Rows(0).Item("jetnet_comp_id")), InfoTable.Rows(0).Item("jetnet_comp_id").ToString, 0)
                Else
                    If InfoTable.Rows(0).Item("comp_status").ToString = "N" And JournalID = 0 Then
                        company_name.Text &= " <font size='-1'>(No Longer Active)</font>"
                        If Not IsNothing(information_tab) Then
                            information_tab.HeaderText &= " <font size='-1'>(No Longer Active)</font>"
                        Else
                            information_label.Text = Replace(information_label.Text, "</div>", "<span class=""float_right"">(No Longer Active)</span></div>")
                        End If
                    End If
                End If



                information_label.Text += "<table width=""100%"" cellpadding=""0"" cellspacing=""0"" class=""formatTable blue"">"


                information_label.Text += "<tr><td align=""left"" valign=""top"">"
                If DoingBusinessAs <> "" Then
                    information_label.Text += "<span class='li_no_bullet'>" & DoingBusinessAs & "</span>"
                End If
                information_label.Text += IIf(Not IsDBNull(InfoTable.Rows(0).Item("comp_address1")), "<span class='li_no_bullet'>" & InfoTable.Rows(0).Item("comp_address1").ToString & "</span>", "")
                information_label.Text += IIf(Not IsDBNull(InfoTable.Rows(0).Item("comp_address2")), "<span class='li_no_bullet'>" & InfoTable.Rows(0).Item("comp_address2").ToString & "</span>", "")

                If isNote = False Then
                    company_address.Text = InfoTable.Rows(0).Item("comp_address1").ToString & " " & InfoTable.Rows(0).Item("comp_address2").ToString & " " & InfoTable.Rows(0).Item("comp_city").ToString & " " & InfoTable.Rows(0).Item("comp_state").ToString & " " & InfoTable.Rows(0).Item("comp_zip_code").ToString
                End If
                'information_label.Text += "</td></tr><tr><td align=""left"" valign=""top"">"
                information_label.Text += "<span class='li_no_bullet'>"
                information_label.Text += IIf(Not IsDBNull(InfoTable.Rows(0).Item("comp_city")), InfoTable.Rows(0).Item("comp_city").ToString & ", ", "")
                information_label.Text += IIf(Not IsDBNull(InfoTable.Rows(0).Item("comp_state")), InfoTable.Rows(0).Item("comp_state").ToString & " ", "")
                information_label.Text += IIf(Not IsDBNull(InfoTable.Rows(0).Item("comp_country")), InfoTable.Rows(0).Item("comp_country").ToString & " ", "")
                information_label.Text += IIf(Not IsDBNull(InfoTable.Rows(0).Item("comp_zip_code")), InfoTable.Rows(0).Item("comp_zip_code").ToString & " ", "")

                Website = IIf(Not IsDBNull(InfoTable.Rows(0).Item("comp_web_address")), InfoTable.Rows(0).Item("comp_web_address").ToString, "")
                Email = IIf(Not IsDBNull(InfoTable.Rows(0).Item("comp_email_address")), InfoTable.Rows(0).Item("comp_email_address").ToString, "")

                Website = Replace(Website, "http://", "")
                'Website = Replace(Website, "www.", "")
                If Not IsDBNull(InfoTable.Rows(0).Item("comp_logo_flag")) Then
                    If InfoTable.Rows(0).Item("comp_logo_flag") = "Y" Then
                        LogoFlag = True
                        PerformExtraLookupMainLocation = False
                    Else
                        LogoFlag = False
                    End If
                End If

                If Not IsDBNull(InfoTable.Rows(0).Item("clicomp_description")) Then
                    If InfoTable.Rows(0).Item("clicomp_description").ToString <> "" Then
                        If crmSource = "CLIENT" Then
                            about_label.Text = "<div class=""Box""><div class=""subHeader"">CLIENT SUMMARY</div><br />"
                            about_label.Text += InfoTable.Rows(0).Item("clicomp_description").ToString.Replace(vbCrLf, "<br />")
                        Else
                            about_label.Text = "<div class=""Box""><div class=""subHeader"">ABOUT</div><br />"
                            about_label.Text += InfoTable.Rows(0).Item("clicomp_description").ToString
                        End If


                        about_label.Text += "</div>"
                        'about.Visible = True
                        PerformExtraLookupMainLocation = False
                    End If
                End If

                If CrmVIEW Then
                    If crmSource = "CLIENT" Then 'need extra lookup
                        Dim otherData As New DataTable
                        otherData = master.aclsData_Temp.GetCompanyInfo_ID(OtherID, "JETNET", JournalID)
                        If Not IsNothing(otherData) Then
                            If otherData.Rows.Count > 0 Then
                                Website = Replace(Website, "http://", "")
                                'Website = Replace(Website, "www.", "")
                                If Not IsDBNull(otherData.Rows(0).Item("comp_logo_flag")) Then
                                    If otherData.Rows(0).Item("comp_logo_flag") = "Y" Then
                                        LogoFlag = True
                                        PerformExtraLookupMainLocation = False
                                    Else
                                        LogoFlag = False
                                    End If
                                End If

                                'If Not IsDBNull(otherData.Rows(0).Item("clicomp_description")) Then
                                '  If otherData.Rows(0).Item("clicomp_description").ToString <> "" Then
                                '    about_label.Text = "<div class=""Box""><div class=""subHeader"">ABOUT</div><br />"
                                '    about_label.Text += otherData.Rows(0).Item("clicomp_description").ToString
                                '    about_label.Text += "</div>"
                                '    'about.Visible = True
                                '    PerformExtraLookupMainLocation = False
                                '  End If
                                'End If
                            End If
                        End If
                    End If
                End If
                information_label.Text += "</span>"
                information_label.Text += "</td><td valign=""top"">LOGO</td></tr>"
                If Not String.IsNullOrEmpty(Website) Or Not String.IsNullOrEmpty(Email) Then
                    rowspanCount += 1
                    information_label.Text += " <tr><td align=""left"" valign=""top"">"
                    'Display Email Information.
                    If Not String.IsNullOrEmpty(Email) Then
                        If Email <> "" Then
                            information_label.Text += "<span class='li_no_bullet'>Email: <a href='mailto:" & Email & "'>" & Email & "</a></span>"
                        End If
                    End If
                    'Display Website Information.
                    If Not String.IsNullOrEmpty(Website) Then
                        If Website <> "" Then
                            information_label.Text += "<span class='li_no_bullet'><a href='http://www." & Replace(Website, "www.", "") & "' target='new'>" & Website & "</a></span>"
                        End If
                    End If
                    information_label.Text += "</td></tr>"
                End If
                ' information_label.Text += "</td></tr>" '</table>"
            End If
        End If

        If Not IsNothing(InfoTable) Then
            InfoTable.Dispose()
        End If

        If isNote = False Then
            '  information_label.Text += "</td>"


            ''''''Let's do an extra lookup to see if we have a main location logo.
            If PerformExtraLookupMainLocation = True Then
                'We now need to perform the extra main location lookup
                MainLocationDataTable = master.aclsdata_temp.GetCompanyMainLocationDescriptionLogo(IIf(CrmVIEW, IIf(crmSource = "CLIENT", OtherID, CompanyID), CompanyID))
                If Not IsNothing(MainLocationDataTable) Then
                    If MainLocationDataTable.Rows.Count > 0 Then
                        'This means there is a main location, let's check for a logo here.
                        If Not IsDBNull(MainLocationDataTable.Rows(0).Item("comp_logo_flag")) Then
                            LogoFlag = IIf(MainLocationDataTable.Rows(0).Item("comp_logo_flag") = "Y", True, False)
                            MainLocationID = MainLocationDataTable.Rows(0).Item("comp_id")
                        End If

                        If Not IsDBNull(MainLocationDataTable.Rows(0).Item("comp_customer_notes")) Then
                            If MainLocationDataTable.Rows(0).Item("comp_customer_notes").ToString <> "" Then
                                about_label.Text = "<div class=""Box""><div class=""subHeader"">ABOUT</div><br />"
                                about_label.Text += MainLocationDataTable.Rows(0).Item("comp_customer_notes").ToString
                                about_label.Text += "</div>"
                                'about.Visible = True
                            End If
                        End If

                    End If
                End If
            End If
        End If
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'information_label.Text += "</td></tr></table></div><br /><div class=""Box"">" '<tr><td align=""left"" valign=""top"">"
        'information_label.Text += "</div><div class=""columns four remove_margin"">"
        'information_label.Text += "<table width=""100%"" cellpadding=""0"" cellspacing=""0"" class=""formatTable blue"" >"

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' information_label.Text += "</td></tr>"
        'Phone Company Query
        If JournalID = 0 Then   ' only show current phone numbers 
            PhoneTable = master.aclsData_Temp.GetPhoneNumbers(CompanyID, 0, IIf(CrmVIEW = False, "JETNET", crmSource), JournalID)
            If Not IsNothing(PhoneTable) Then
                If PhoneTable.Rows.Count > 0 Then
                    rowspanCount += 1
                    information_label.Text += "<tr><td>"
                    For Each r As DataRow In PhoneTable.Rows
                        information_label.Text += "<span class='li_no_bullet'>" & IIf(Not IsDBNull(r("pnum_type")), r("pnum_type"), "") & " <span class=""make-tel-link"">" & IIf(Not IsDBNull(r("pnum_number")), r("pnum_number"), "") & "</span></span>"
                    Next
                    information_label.Text += "</td></tr>"
                End If
            End If


            If Not IsNothing(PhoneTable) Then
                PhoneTable.Dispose()
            End If
        End If


        'information_label.Text += "<tr class=""noBorder""><td align=""left"" valign=""top""><div class=""subHeader"">Phone Numbers</div>"
        If isNote = False Then
            ' information_label.Text += "<tr class=""noBorder""><td align='left' valign='top'>"
            If LogoFlag = True Then

                If HttpContext.Current.Session.Item("jetnetWebSiteType") = crmWebClient.eWebSiteTypes.LOCAL Then
                    information_label.Text = Replace(information_label.Text, ">LOGO", " " & IIf(rowspanCount > 1, "rowspan='" & rowspanCount.ToString & "'", "") & " class=""noBorderWhite""><img class=""imageCompany"" src=""http://www.jetnetevolution.com/pictures/company/" + IIf(MainLocationID = 0, IIf(crmSource = "CLIENT", OtherID.ToString, CompanyID.ToString), MainLocationID.ToString) + ".jpg"" width='250' class=""""  />")
                Else
                    information_label.Text = Replace(information_label.Text, ">LOGO", " " & IIf(rowspanCount > 1, "rowspan='" & rowspanCount.ToString & "'", "") & " class=""noBorderWhite""><img class=""imageCompany"" src=""" + HttpContext.Current.Session.Item("jetnetFullHostName").ToString + HttpContext.Current.Session.Item("CompanyPicturesFolderVirtualPath") + "/" + IIf(MainLocationID = 0, IIf(crmSource = "CLIENT", OtherID.ToString, CompanyID.ToString), MainLocationID.ToString) + ".jpg"" width='250' class=""""  />")
                End If
            Else
                information_label.Text = Replace(information_label.Text, ">LOGO", IIf(rowspanCount > 1, " rowspan='" & rowspanCount.ToString & "'", "") & "  class=""noBorderWhite"">")
            End If
            ' information_label.Text += "</td>"


            'PhoneTable.Dispose()
            'information_label.Text += "</tr>"
            'information_label.Text += "<tr>"


            'information_label.Text += "<td align='left' valign='top'>"
        Else
            information_label.Text = Replace(information_label.Text, ">LOGO", IIf(rowspanCount > 1, " rowspan='" & rowspanCount.ToString & "'", "") & " class=""noBorderWhite"">")
        End If

        information_label.Text += "</table></div></div>"

        ' Response.Write(information_label.Text)

    End Sub


    Public Shared Function CreateCompanyEditLink(ByVal crmSource As String, ByVal crmView As Boolean, ByVal companyID As Long, ByVal EditMainCompanyCLIENT As Boolean, ByVal CreateMainCompanyJETNET As Boolean, Optional ByVal UseText As Boolean = False) As String
        Dim returnString As String = ""
        If crmView Then
            If crmSource = "CLIENT" Then
                If companyID > 0 Then
                    If EditMainCompanyCLIENT = True Then
                        returnString = "<a href=""javascript:void(0);"" onclick=""javascript:load('/edit.aspx?action=edit&type=company&Listing=1&comp_ID=" & companyID.ToString & "&source=CLIENT&from=companyDetails','','scrollbars=yes,menubar=no,height=900,width=940,resizable=yes,toolbar=no,location=no,status=no');return false;"" " & IIf(UseText, "", "class=""float_right padding_left""") & ">" & IIf(UseText, "Edit Client", "<img src=""images/edit_icon.png"" alt=""Edit Client"" />") & "</a>"
                    End If
                End If
            ElseIf crmSource <> "CLIENT" And CreateMainCompanyJETNET = True Then
                returnString = "<a href=""javascript:void(0);"" onclick=""javascript:load('/edit.aspx?action=edit&type=company&Listing=1&comp_ID=" & companyID.ToString & "&source=JETNET&from=companyDetails','','scrollbars=yes,menubar=no,height=900,width=940,resizable=yes,toolbar=no,location=no,status=no');return false;"" title=""Create Client Record"">" & IIf(UseText, "Create Client", "<img src=""images/edit_icon.png"" alt=""Create Client"" />") & "</a>"
            End If
        End If
        Return returnString
    End Function


    Public Shared Sub Fill_Information_Tab_ChatBox(ByRef information_tab As Label, ByRef information_label As Label, ByVal CompanyID As Long, ByVal JournalID As Long, ByRef sDoingBusinessAs As String, ByRef company_address As Label)
        Dim InfoTable As New DataTable
        Dim PhoneTable As New DataTable
        Dim Email As String = ""
        Dim Website As String = ""
        Dim LogoFlag As Boolean = False
        Dim MainLocationID As Long = 0
        Dim PerformExtraLookupMainLocation As Boolean = True
        Dim MainLocationDataTable As New DataTable

        Dim aclsData_Temp As New clsData_Manager_SQL
        aclsData_Temp.JETNET_DB = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Main Company Query.
        InfoTable = aclsData_Temp.GetCompanyInfo_ID(CompanyID, "JETNET", JournalID)

        If Not IsNothing(InfoTable) Then
            If InfoTable.Rows.Count > 0 Then
                If Not IsDBNull(InfoTable.Rows(0).Item("comp_alternate_name")) Then
                    sDoingBusinessAs = IIf(Not IsDBNull(InfoTable.Rows(0).Item("comp_alternate_name_type")), InfoTable.Rows(0).Item("comp_alternate_name_type").ToString, "") & ":" & IIf(Not IsDBNull(InfoTable.Rows(0).Item("comp_alternate_name")), InfoTable.Rows(0).Item("comp_alternate_name").ToString, "")
                End If

                'Set the Tab Header
                information_tab.Text = "<div class=""subHeader padding_left emphasisColor"">" & IIf(Not IsDBNull(InfoTable.Rows(0).Item("comp_name")), InfoTable.Rows(0).Item("comp_name").ToString, "")

                If InfoTable.Rows(0).Item("comp_status").ToString = "N" And JournalID = 0 Then
                    information_tab.Text &= " <font size='-1'>(No Longer Active)</font>"
                End If

                information_tab.Text &= "</div>"

                'Fill the Label (static for now)
                information_label.Text = "<table width='100%' cellspacing='3' cellpadding='3' class=""formatTable blue"">"
                information_label.Text += "<tr>"
                information_label.Text += "<td align='left' valign='top' width='60%'>"
                If sDoingBusinessAs <> "" Then
                    information_label.Text += "<span class='li_no_bullet'>" + sDoingBusinessAs.Trim + "</span>"
                End If
                information_label.Text += IIf(Not IsDBNull(InfoTable.Rows(0).Item("comp_address1")), "<span class='li_no_bullet'>" + InfoTable.Rows(0).Item("comp_address1").ToString + "</span>", "")
                information_label.Text += IIf(Not IsDBNull(InfoTable.Rows(0).Item("comp_address2")), "<span class='li_no_bullet'>" + InfoTable.Rows(0).Item("comp_address2").ToString + "</span>", "")

                company_address.Text = InfoTable.Rows(0).Item("comp_address1").ToString & " " & InfoTable.Rows(0).Item("comp_address2").ToString & " " & InfoTable.Rows(0).Item("comp_city").ToString & " " & InfoTable.Rows(0).Item("comp_state").ToString & " " & InfoTable.Rows(0).Item("comp_zip_code").ToString

                information_label.Text += "<span class='li_no_bullet'>"
                information_label.Text += IIf(Not IsDBNull(InfoTable.Rows(0).Item("comp_city")), InfoTable.Rows(0).Item("comp_city").ToString & ", ", "")
                information_label.Text += IIf(Not IsDBNull(InfoTable.Rows(0).Item("comp_state")), InfoTable.Rows(0).Item("comp_state").ToString & " ", "")
                information_label.Text += IIf(Not IsDBNull(InfoTable.Rows(0).Item("comp_country")), InfoTable.Rows(0).Item("comp_country").ToString & " ", "")
                information_label.Text += IIf(Not IsDBNull(InfoTable.Rows(0).Item("comp_zip_code")), InfoTable.Rows(0).Item("comp_zip_code").ToString & " ", "")

                Website = IIf(Not IsDBNull(InfoTable.Rows(0).Item("comp_web_address")), InfoTable.Rows(0).Item("comp_web_address").ToString, "")
                Email = IIf(Not IsDBNull(InfoTable.Rows(0).Item("comp_email_address")), InfoTable.Rows(0).Item("comp_email_address").ToString, "")

                Website = Replace(Website, "http://", "")
                If Not IsDBNull(InfoTable.Rows(0).Item("comp_logo_flag")) Then
                    If InfoTable.Rows(0).Item("comp_logo_flag") = "Y" Then
                        LogoFlag = True
                        PerformExtraLookupMainLocation = False
                    Else
                        LogoFlag = False
                    End If
                End If

                If Not IsDBNull(InfoTable.Rows(0).Item("clicomp_description")) Then
                    If InfoTable.Rows(0).Item("clicomp_description").ToString <> "" Then
                        PerformExtraLookupMainLocation = False
                    End If
                End If
                information_label.Text += "</span>"

                'Display Email Information.
                If Email <> "" Then
                    information_label.Text += "<span class='li_no_bullet'>Email: <a href='mailto:" & Email & "'>" & Email & "</a></span>"
                End If
                'Display Website Information.
                If Website <> "" Then
                    information_label.Text += "<span class='li_no_bullet'><a href='http://www." & Replace(Website, "www.", "") & "' target='new'>" & Website & "</a></span>"
                End If
            End If
        End If

        InfoTable.Dispose()

        ''''''Let's do an extra lookup to see if we have a main location logo.
        If PerformExtraLookupMainLocation = True Then
            'We now need to perform the extra main location lookup
            MainLocationDataTable = aclsData_Temp.GetCompanyMainLocationDescriptionLogo(CompanyID)
            If Not IsNothing(MainLocationDataTable) Then
                If MainLocationDataTable.Rows.Count > 0 Then
                    'This means there is a main location, let's check for a logo here.
                    If Not IsDBNull(MainLocationDataTable.Rows(0).Item("comp_logo_flag")) Then
                        LogoFlag = IIf(MainLocationDataTable.Rows(0).Item("comp_logo_flag") = "Y", True, False)
                        MainLocationID = MainLocationDataTable.Rows(0).Item("comp_id")
                    End If

                End If
            End If

        End If
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        information_label.Text += "<td align='left' valign='top' width='40%'>"
        Dim imgDisplayFolder As String = HttpContext.Current.Session.Item("jetnetFullHostName") + HttpContext.Current.Session.Item("CompanyPicturesFolderVirtualPath")

        If LogoFlag = True Then

            If HttpContext.Current.Session.Item("jetnetWebSiteType") = crmWebClient.eWebSiteTypes.LOCAL Then
                information_label.Text += "<img src=""https://www.testjetnetevolution.com/pictures/company/" + IIf(MainLocationID = 0, CompanyID.ToString, MainLocationID.ToString) + ".jpg"" class="""" width=""175"" height=""60"" style=""width: 175px; height: 60px;"" border=""1""/><br /><br />"
            Else
                information_label.Text += "<img src=""" + imgDisplayFolder + "/" + IIf(MainLocationID = 0, CompanyID.ToString, MainLocationID.ToString) + ".jpg"" class="""" width=""175"" height=""60"" style=""width: 175px; height: 60px;"" border=""1""/><br /><br />"
            End If

        End If
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        'Phone Company Query
        PhoneTable = aclsData_Temp.GetPhoneNumbers(CompanyID, 0, "JETNET", JournalID)
        If Not IsNothing(PhoneTable) Then
            If PhoneTable.Rows.Count > 0 Then
                For Each r As DataRow In PhoneTable.Rows
                    information_label.Text += "<span class='li_no_bullet'>" & IIf(Not IsDBNull(r("pnum_type")), r("pnum_type"), "") & " <span class=""make-tel-link"">" & IIf(Not IsDBNull(r("pnum_number")), r("pnum_number"), "") & "</span></span>"
                Next
            End If
        End If
        PhoneTable.Dispose()

        information_label.Text += "</td>"
        information_label.Text += "</tr>"
        information_label.Text += "</table>"

    End Sub
    ''' <summary>
    ''' This is the helper query for the Evolution Yacht Listing Page Query
    ''' </summary>
    ''' <param name="yt_id">Yacht ID</param>
    ''' <returns></returns> 
    ''' <remarks></remarks>
    Public Shared Function EvolutionYachtListingRelatedCompanies(ByVal yt_id As Long, ByVal aerodex As Boolean, ByVal ApplicableRelationships As String) As DataTable
        Dim sql As String = ""
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim atemptable As New DataTable
        Dim count As Integer = 1
        atemptable.Columns.Add("act_name")
        atemptable.Columns.Add("yct_name")
        atemptable.Columns.Add("comp_name")
        atemptable.Columns.Add("comp_id")
        atemptable.Columns.Add("comp_address1")
        atemptable.Columns.Add("comp_address2")
        atemptable.Columns.Add("comp_city")
        atemptable.Columns.Add("comp_state")
        atemptable.Columns.Add("comp_zip_code")
        atemptable.Columns.Add("comp_country")
        atemptable.Columns.Add("comp_phone_office")
        atemptable.Columns.Add("comp_phone_fax")
        atemptable.Columns.Add("comp_email_address")
        atemptable.Columns.Add("comp_web_address")
        atemptable.Columns.Add("cref_contact_type")
        atemptable.Columns.Add("cref_transmit_seq_no")
        atemptable.Columns.Add("acref_comp_id")
        atemptable.Columns.Add("cref_owner_percent")
        atemptable.Columns.Add("actype_name")
        atemptable.Columns.Add("comp_name_alt")
        atemptable.Columns.Add("comp_name_alt_type")

        Try
            If yt_id <> 0 Then
                sql = ""
                sql = "select yct_name, comp_name, comp_id, comp_address1, comp_address2, comp_city, comp_state, comp_zip_code,"
                sql += " comp_country, comp_web_address, comp_email_address, yct_seq_no, yr_contact_type, comp_name_alt, comp_name_alt_type "
                sql += " from Yacht_Reference with (NOLOCK)"
                sql += " inner join Yacht_Contact_Type with (NOLOCK) on yr_contact_type=yct_code"
                sql += " inner join Company with (NOLOCK) on yr_comp_id = comp_id and yr_journ_id = comp_journ_id"
                sql += " where yr_yt_id = " & yt_id
                sql += " and yr_journ_id = 0"
                sql += " and yr_contact_type not in ('71') "
                If ApplicableRelationships <> "" Then
                    sql = sql & " and ( yr_contact_type in (" & ApplicableRelationships & "))"
                End If

                sql += " order by yct_seq_no"


                'HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>EvolutionYachtListingRelatedCompanies(ByVal ac_id As Long) As DataTable</b><br />" & sql


                SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
                SqlConn.Open()
                SqlCommand.Connection = SqlConn


                SqlCommand.CommandText = sql
                SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
                SqlCommand.CommandType = CommandType.Text
                SqlCommand.CommandTimeout = 60


                While SqlReader.Read()
                    Dim newCustomersRow As DataRow = atemptable.NewRow()
                    newCustomersRow("cref_transmit_seq_no") = SqlReader.Item("yct_seq_no")
                    newCustomersRow("cref_owner_percent") = "0"
                    newCustomersRow("cref_contact_type") = SqlReader.Item("yr_contact_type")
                    newCustomersRow("act_name") = SqlReader.Item("yct_name")
                    newCustomersRow("comp_name") = SqlReader.Item("comp_name")
                    newCustomersRow("comp_id") = SqlReader.Item("comp_id")
                    newCustomersRow("comp_address1") = SqlReader.Item("comp_address1")
                    newCustomersRow("comp_address2") = SqlReader.Item("comp_address2")
                    newCustomersRow("comp_city") = SqlReader.Item("comp_city")
                    newCustomersRow("comp_state") = SqlReader.Item("comp_state")
                    newCustomersRow("comp_zip_code") = SqlReader.Item("comp_zip_code")
                    newCustomersRow("comp_country") = SqlReader.Item("comp_country")

                    newCustomersRow("comp_phone_office") = ""
                    newCustomersRow("comp_phone_fax") = ""
                    newCustomersRow("comp_email_address") = SqlReader.Item("comp_email_address")
                    newCustomersRow("comp_web_address") = SqlReader.Item("comp_web_address")
                    newCustomersRow("acref_comp_id") = SqlReader.Item("comp_id")
                    newCustomersRow("actype_name") = SqlReader.Item("yct_name")
                    newCustomersRow("comp_name_alt") = SqlReader.Item("comp_name_alt")
                    newCustomersRow("comp_name_alt_type") = SqlReader.Item("comp_name_alt_type")

                    atemptable.Rows.Add(newCustomersRow)
                    atemptable.AcceptChanges()
                    count += 1
                End While

            End If

        Catch ex As Exception
            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "EvolutionYachtListingRelatedCompanies() (" + sql.Trim + "): " + ex.Message.ToString.Trim
        Finally
            SqlReader = Nothing
            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing

            SqlCommand.Dispose()
            SqlCommand = Nothing
        End Try

        Return atemptable
        atemptable = Nothing

    End Function
    ''' <summary>
    ''' This is the helper query for the Evolution Aircraft Listing Page Query
    ''' </summary>
    ''' <param name="ac_id">AC ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function MobileACLoadCompanies(ByVal ac_id As Long, ByVal aerodex As Boolean) As DataTable

        Dim sql As String = ""
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim atemptable As New DataTable
        Dim count As Integer = 1
        Dim sqlWhere As String = ""

        atemptable.Columns.Add("cref_transmit_seq_no")
        atemptable.Columns.Add("cref_contact_type")
        atemptable.Columns.Add("actype_name")
        atemptable.Columns.Add("comp_name")
        atemptable.Columns.Add("comp_id")
        atemptable.Columns.Add("cref_owner_percent")

        Try
            If ac_id <> 0 Then

                sql = ""
                sql = "SELECT top 1 cref_transmit_seq_no, cref_contact_type,	replace(replace(actype_name,'Exclusive Broker','Broker'),'Sales Company/Contact','Sales') as actype_name, comp_name,  comp_id, cref_owner_percent,"
                sql += " case when cref_contact_type IN ('99','93') then 2 when cref_contact_type IN ('38') then 3 "
                sql += " when cref_contact_type IN ('36') then 4 when cref_contact_type IN ('00') then 5 else 6 end as sortorder"
                sql += " FROM Aircraft_Company_Flat with (NOLOCK)  "
                sql += " WHERE  (cref_ac_id  = " & ac_id & " and cref_journ_id = 0 "

                If aerodex Then
                    sql += " and Cref_contact_type NOT IN ('93','98','99','71','44', '38') "
                Else
                    sql += " AND cref_contact_type NOT IN ('71','44') "
                End If
                sql += " )  "

                sql += commonEvo.MakeCompanyProductCodeClause(HttpContext.Current.Session.Item("localPreferences"), False)
                sql += " ORDER BY sortorder, cref_transmit_seq_no"

                SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
                SqlConn.Open()
                SqlCommand.Connection = SqlConn


                SqlCommand.CommandText = sql
                SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
                SqlCommand.CommandType = CommandType.Text
                SqlCommand.CommandTimeout = 60


                While SqlReader.Read()
                    Dim newCustomersRow As DataRow = atemptable.NewRow()
                    newCustomersRow("cref_transmit_seq_no") = SqlReader.Item("cref_transmit_seq_no")
                    newCustomersRow("cref_contact_type") = SqlReader.Item("cref_contact_type")
                    newCustomersRow("actype_name") = SqlReader.Item("actype_name")
                    newCustomersRow("comp_name") = SqlReader.Item("comp_name")
                    newCustomersRow("cref_owner_percent") = SqlReader.Item("cref_owner_percent")
                    newCustomersRow("comp_id") = SqlReader.Item("comp_id")

                    atemptable.Rows.Add(newCustomersRow)
                    atemptable.AcceptChanges()
                    count += 1
                End While

            End If
        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "MobileACLoadCompanies() (" + sql.Trim + "): " + ex.Message.ToString.Trim


        Finally
            SqlReader = Nothing
            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing

            SqlCommand.Dispose()
            SqlCommand = Nothing
        End Try

        Return atemptable
        atemptable = Nothing

    End Function



    ''' <summary>
    ''' This is the helper query for the Evolution Aircraft Listing Page Query
    ''' </summary>
    ''' <param name="ac_id">AC ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function EvolutionAircraftListingRelatedCompanies(ByVal ac_id As Long, ByVal aerodex As Boolean, ByVal ApplicableRelationships As String, ByVal AdvancedOperator As Boolean, ByVal CompanyCountriesString As String, ByVal CompanyTimeZoneString As String, ByVal CompanyContinentString As String, ByVal CompanyRegionString As String, ByVal CompanyStateName As String) As DataTable



        Dim sql As String = ""
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim atemptable As New DataTable
        Dim count As Integer = 1
        Dim sqlWhere As String = ""


        atemptable.Columns.Add("cref_transmit_seq_no")
        atemptable.Columns.Add("cref_contact_type")
        atemptable.Columns.Add("actype_name")
        atemptable.Columns.Add("comp_name")
        atemptable.Columns.Add("comp_address1")
        atemptable.Columns.Add("comp_address2")
        atemptable.Columns.Add("comp_city")
        atemptable.Columns.Add("comp_state")
        atemptable.Columns.Add("comp_country")
        atemptable.Columns.Add("comp_phone_office")
        atemptable.Columns.Add("comp_phone_fax")
        atemptable.Columns.Add("comp_email_address")
        atemptable.Columns.Add("comp_web_address")
        atemptable.Columns.Add("acref_comp_id")
        atemptable.Columns.Add("cref_owner_percent")
        atemptable.Columns.Add("comp_name_alt")
        atemptable.Columns.Add("comp_name_alt_type")

        Try
            If ac_id <> 0 Then

                sql = ""
                sql = "SELECT cref_transmit_seq_no, cref_contact_type, actype_name, comp_name, comp_name_alt, comp_name_alt_type, "
                sql += " comp_address1,comp_address2, comp_city, comp_state, comp_country, comp_phone_office,  "
                sql += "  comp_phone_fax, comp_email_address, comp_web_address, comp_id, cref_owner_percent "
                sql += " FROM Aircraft_Company_Flat with (NOLOCK) "
                sql += " WHERE "

                'Edited on 10/16/2014 
                'Added regional information to this query to filter the companies based on what was selected for the aircraft search.
                sqlWhere = " (cref_ac_id  = " & ac_id & " and cref_journ_id = 0 "

                If CompanyTimeZoneString <> "" Then
                    If sqlWhere <> "" Then
                        sqlWhere += " and "
                    End If

                    sqlWhere += " comp_timezone in (SELECT tzone_name FROM Timezone where tzone_id in (" & CompanyTimeZoneString & ")) "
                End If

                'Continent
                If CompanyContinentString <> "" Then
                    If sqlWhere <> "" Then
                        sqlWhere += " AND"
                    End If
                    sqlWhere += " country_continent_name in (" & CompanyContinentString & ") "
                End If

                ' check the state
                If CompanyStateName <> "" Then
                    If sqlWhere <> "" Then
                        sqlWhere += " AND "
                    End If
                    sqlWhere += " state_name IN (" & CompanyStateName & ")"
                End If


                ' check the country
                If CompanyCountriesString <> "" Then
                    If sqlWhere <> "" Then
                        sqlWhere += " AND "
                    End If
                    sqlWhere += " comp_country in (" & CompanyCountriesString & ") "
                End If

                If Not IsNothing(HttpContext.Current.Session.Item("Frac_Percent")) Then
                    If Trim(HttpContext.Current.Session.Item("Frac_Percent")) <> "" Then
                        If sqlWhere <> "" Then
                            sqlWhere &= " AND "
                        End If
                        sqlWhere &= HttpContext.Current.Session.Item("Frac_Percent")
                    End If
                End If


                'regions
                If CompanyRegionString <> "" Then
                    If sqlWhere <> "" Then
                        sqlWhere += " AND "
                    End If
                    sqlWhere += " comp_country in (select distinct geographic_country_name FROM geographic with (NOLOCK) where geographic_region_name in (" & CompanyRegionString & ")) "

                    If CompanyStateName <> "" Then
                        sqlWhere += " and state_name in (select distinct state_name FROM geographic with (NOLOCK) inner join State with (NOLOCK) on state_code=geographic_state_code and state_country=geographic_country_name where geographic_region_name in (" & CompanyRegionString & ")) "
                    End If
                End If

                '-- EXCLUDES RESEARCH ONLY AND CHIEF PILOT COMPANIES/REFERENCES
                'If you're searching for a relationship of Chief Pilot, then we go ahead and 
                'remove the exclusion on it. Otherwise - it's excluded just like normal.

                If InStr(ApplicableRelationships, "'44'") = 0 Then
                    If aerodex Then
                        sqlWhere += " and Cref_contact_type NOT IN ('93','98','99','71','44', '38','2X')) "
                    Else
                        sqlWhere += " AND cref_contact_type NOT IN ('71','44')) "
                    End If
                Else
                    If aerodex Then
                        sqlWhere += " and Cref_contact_type NOT IN ('93','98','99','71', '38','2X')) "
                    Else
                        sqlWhere += " AND cref_contact_type NOT IN ('71')) "
                    End If
                End If


                If ApplicableRelationships <> "" Then
                    sqlWhere += " and ( cref_contact_type in (" & ApplicableRelationships & ")"
                    If AdvancedOperator = False Then
                        sqlWhere += " ) "
                    End If
                End If

                If AdvancedOperator = True Then
                    If ApplicableRelationships <> "" Then
                        sqlWhere += " or "
                    Else
                        sqlWhere += " and "
                    End If

                    sqlWhere += " cref_operator_flag IN ('Y', 'O') " & " "

                    If ApplicableRelationships <> "" Then
                        sqlWhere += " ) "
                    End If
                End If

                '-- PART BELOW ADDED BASED ON PRODUCTS USER CAN SEE
                sqlWhere += " AND (comp_product_helicopter_flag = 'Y' OR comp_product_business_flag = 'Y' "
                sqlWhere += " OR comp_product_commercial_flag = 'Y') "

                sql += sqlWhere
                sql += " ORDER BY cref_transmit_seq_no, comp_name"

                'HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>EvolutionAircraftListingRelatedCompanies(ByVal ac_id As Long) As DataTable</b><br />" & sql


                SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
                SqlConn.Open()
                SqlCommand.Connection = SqlConn


                SqlCommand.CommandText = sql
                SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
                SqlCommand.CommandType = CommandType.Text
                SqlCommand.CommandTimeout = 60


                While SqlReader.Read()
                    Dim newCustomersRow As DataRow = atemptable.NewRow()
                    newCustomersRow("cref_transmit_seq_no") = SqlReader.Item("cref_transmit_seq_no")
                    newCustomersRow("cref_contact_type") = SqlReader.Item("cref_contact_type")
                    newCustomersRow("actype_name") = SqlReader.Item("actype_name")
                    newCustomersRow("comp_name") = SqlReader.Item("comp_name")
                    newCustomersRow("comp_address1") = SqlReader.Item("comp_address1")
                    newCustomersRow("comp_address2") = SqlReader.Item("comp_address2")
                    newCustomersRow("comp_city") = SqlReader.Item("comp_city")
                    newCustomersRow("comp_state") = SqlReader.Item("comp_state")
                    newCustomersRow("comp_country") = SqlReader.Item("comp_country")
                    newCustomersRow("comp_phone_office") = SqlReader.Item("comp_phone_office")
                    newCustomersRow("comp_phone_fax") = SqlReader.Item("comp_phone_fax")
                    newCustomersRow("comp_email_address") = SqlReader.Item("comp_email_address")
                    newCustomersRow("comp_web_address") = SqlReader.Item("comp_web_address")
                    newCustomersRow("acref_comp_id") = SqlReader.Item("comp_id")
                    newCustomersRow("cref_owner_percent") = SqlReader.Item("cref_owner_percent")
                    newCustomersRow("comp_name_alt") = SqlReader.Item("comp_name_alt")
                    newCustomersRow("comp_name_alt_type") = SqlReader.Item("comp_name_alt_type")

                    atemptable.Rows.Add(newCustomersRow)
                    atemptable.AcceptChanges()
                    count += 1
                End While

            End If
        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "EvolutionAircraftListingRelatedCompanies() (" + sql.Trim + "): " + ex.Message.ToString.Trim


        Finally
            SqlReader = Nothing
            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing

            SqlCommand.Dispose()
            SqlCommand = Nothing
        End Try

        Return atemptable
        atemptable = Nothing

    End Function



    ''' <summary>
    ''' Displays Evolution AC Companies for the Aircraft_Listing.aspx. This uses the company flat table and builds a string.
    ''' </summary>
    ''' <param name="aclsData_Temp"></param>
    ''' <param name="acID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks> 
    Public Shared Function FindEvolutionACCompanies(ByVal aclsData_Temp As clsData_Manager_SQL, ByVal acID As Long, Optional ByVal CRMAircraft As Boolean = False, Optional ByVal CRMView As Boolean = False) As String
        Dim tempTable As New DataTable
        Dim strAnswer As String = ""
        Dim oldCompID As Long = 0
        Dim oldCompName As String = ""
        Dim count As Integer = 0
        Dim strSeqNo As String = ""
        Dim strCompanyTypeName As String = ""
        Dim fAirRef_transmit_seq_no As Integer = 0
        Dim companyID As Integer = 0
        Dim fCompany_name As String = ""
        Dim fAirRef_contact_type As String = ""
        Dim fAirContactType_name As String = ""
        Dim fAirRef_owner_percent As Double = 0
        Dim CompanyTitle As String = ""
        Dim CompanyLocation As String = ""
        Dim AdvancedSearchRelationshipType As String = ""
        Dim AdvancedOperator As Boolean = False
        Dim DynamicSearchQuery As String = ""
        Dim li_class As String = "ac_company_bullet"
        'Dim FractionalDate As String = ""
        'Dim FractionalDateOperator As String = ""
        'datarow filtering variables.
        Dim afileterd As DataRow()
        Dim AdvancedSearchString As String = ""
        FindEvolutionACCompanies = ""
        Dim CompanyCountriesString As String = ""
        Dim CompanyTimeZoneString As String = ""
        Dim CompanyContinentString As String = ""
        Dim CompanyRegionString As String = ""
        Dim CompanyStateName As String = ""
        Dim CompanyAlternateName As String = ""
        Dim CompanyAlternateNameType As String = ""
        Dim CompanyCountriesArray As String()
        Dim CompanyContinentArray As String()
        Dim CompanyRegionArray As String()
        Dim CompanyStateNameArray As String()


        Try
            'There are two different contact types named for each different application.
            'The first is Yacht.
            If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT Then
                If Not IsNothing(HttpContext.Current.Session.Item("Advanced-yr_contact_type")) Then
                    If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("Advanced-yr_contact_type")) Then
                        AdvancedSearchString = HttpContext.Current.Session.Item("Advanced-yr_contact_type")
                    End If
                End If
            Else 'Second is aircraft

                If Not IsNothing(HttpContext.Current.Session.Item("companyRegionOrContinent")) And Not IsNothing(HttpContext.Current.Session.Item("companyRegion")) Then
                    If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("companyRegionOrContinent")) Then
                        If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("companyRegion")) Then
                            If Not HttpContext.Current.Session.Item("companyRegion").ToString.ToLower.Contains("all") Then
                                If HttpContext.Current.Session.Item("companyRegionOrContinent") = "continent" Then
                                    CompanyContinentArray = Split(HttpContext.Current.Session.Item("companyRegion"), ",")
                                    For CompanyContinentArrayCount = 0 To UBound(CompanyContinentArray)
                                        If CompanyContinentString <> "" Then
                                            CompanyContinentString += ","
                                        End If
                                        CompanyContinentString += "'" & Trim(CompanyContinentArray(CompanyContinentArrayCount)) & "'"
                                    Next
                                Else
                                    CompanyRegionArray = Split(HttpContext.Current.Session.Item("companyRegion"), ",")
                                    For CompanyRegionStringArrayCount = 0 To UBound(CompanyRegionArray)
                                        If CompanyRegionString <> "" Then
                                            CompanyRegionString += ","
                                        End If
                                        CompanyRegionString += "'" & Trim(CompanyRegionArray(CompanyRegionStringArrayCount)) & "'"
                                    Next
                                End If
                            End If
                        End If
                    End If
                End If

                If Not IsNothing(HttpContext.Current.Session.Item("companyTimeZone")) Then
                    If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("companyTimeZone")) Then
                        If Not HttpContext.Current.Session.Item("companyTimeZone").ToString.ToLower.Contains("all") Then
                            CompanyTimeZoneString = HttpContext.Current.Session.Item("companyTimeZone")
                        End If
                    End If
                End If

                If Not IsNothing(HttpContext.Current.Session.Item("companyCountry")) Then
                    If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("companyCountry")) Then
                        If Not HttpContext.Current.Session.Item("companyCountry").ToString.ToLower.Contains("all") Then
                            CompanyCountriesArray = Split(HttpContext.Current.Session.Item("companyCountry"), ",")
                            For CompanyCountriesArrayCount = 0 To UBound(CompanyCountriesArray)
                                If CompanyCountriesString <> "" Then
                                    CompanyCountriesString += ","
                                End If
                                CompanyCountriesString += "'" & Trim(CompanyCountriesArray(CompanyCountriesArrayCount)) & "'"
                            Next
                        End If
                    End If
                End If

                If Not IsNothing(HttpContext.Current.Session.Item("companyState")) Then
                    If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("companyState")) Then
                        If Not HttpContext.Current.Session.Item("companyState").ToString.ToLower.Contains("all") Then
                            CompanyStateNameArray = Split(HttpContext.Current.Session.Item("companyState"), ",")
                            For CompanyStateNameArrayCount = 0 To UBound(CompanyStateNameArray)
                                If CompanyStateName <> "" Then
                                    CompanyStateName += ","
                                End If
                                CompanyStateName += "'" & Trim(CompanyStateNameArray(CompanyStateNameArrayCount)) & "'"
                            Next
                        End If
                    End If
                End If


                If Not IsNothing(HttpContext.Current.Session.Item("Advanced-cref_contact_type")) Then
                    If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("Advanced-cref_contact_type")) Then
                        AdvancedSearchString = HttpContext.Current.Session.Item("Advanced-cref_contact_type")
                    End If
                End If
            End If
            'If the advanced search string isn't empty. Go ahead and fill it up.
            If AdvancedSearchString <> "" Then


                ' If InStr(AdvancedSearchString, "I") > 0 Then   ' commented out and changed- MSw - 11/13/19
                If InStr(AdvancedSearchString, "I") > 0 Then
                    AdvancedSearchString = AdvancedSearchString.Replace("I", "00','97','17','08','16")
                End If

                If InStr(AdvancedSearchString, "00,97,17,08,16") > 0 Then
                    AdvancedSearchString = AdvancedSearchString.Replace("00,97,17,08,16", "00','97','17','08','16")
                End If

                If InStr(AdvancedSearchString, "93,98,99,38,2X") > 0 Then
                    AdvancedSearchString = AdvancedSearchString.Replace("93,98,99,38,2X", "93','98','99','38','2X")
                End If






                AdvancedSearchRelationshipType = ""
                Dim MultipleSelection As Array
                'We split the answer.
                MultipleSelection = AdvancedSearchString.Split("##")
                For MultipleSelectionCount = 0 To UBound(MultipleSelection)
                    If Trim(MultipleSelection(MultipleSelectionCount)) <> "" Then
                        If Trim(MultipleSelection(MultipleSelectionCount)) <> "Y,O" Then ' MSW - 11/13/19
                            If AdvancedSearchRelationshipType <> "" Then
                                AdvancedSearchRelationshipType += ","
                            End If

                            AdvancedSearchRelationshipType += "'" & MultipleSelection(MultipleSelectionCount) & "'"
                        Else
                            AdvancedOperator = True
                        End If
                    End If
                Next
            End If

            If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT Then
                tempTable = EvolutionYachtListingRelatedCompanies(acID, HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag, AdvancedSearchRelationshipType)
            Else
                ''This is specifically checking for the fractional date expires
                'If Not IsNothing(HttpContext.Current.Session.Item("Advanced-COMPARE_cref_fraction_expires_date")) Then
                '    If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("Advanced-COMPARE_cref_fraction_expires_date")) Then
                '        If Not IsNothing(HttpContext.Current.Session.Item("Advanced-cref_fraction_expires_date")) Then
                '            If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("Advanced-cref_fraction_expires_date")) Then
                '                FractionalDate = HttpContext.Current.Session.Item("Advanced-cref_fraction_expires_date").ToString
                '                FractionalDateOperator = HttpContext.Current.Session.Item("Advanced-COMPARE_cref_fraction_expires_date").ToString
                '                If InStr(FractionalDate, "*") = 0 Then
                '                    DynamicSearchQuery += " cref_fraction_expires_date " & clsGeneral.clsGeneral.PrepQueryString(FractionalDateOperator, FractionalDate, "Date", False, "cref_fraction_expires_date", True)
                '                Else
                '                    DynamicSearchQuery += " " & clsGeneral.clsGeneral.PrepQueryString(FractionalDateOperator, FractionalDate, "Date", False, "cref_fraction_expires_date", True)
                '                End If
                '            End If
                '        End If
                '    End If
                'End If
                If CRMView = True And CRMAircraft = True Then
                    tempTable = aclsData_Temp.CRMAircraftListingRelatedCompanies(acID, HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag, AdvancedSearchRelationshipType, AdvancedOperator, CompanyCountriesString, CompanyTimeZoneString, CompanyContinentString, CompanyRegionString, CompanyStateName)
                Else
                    tempTable = EvolutionAircraftListingRelatedCompanies(acID, HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag, AdvancedSearchRelationshipType, AdvancedOperator, CompanyCountriesString, CompanyTimeZoneString, CompanyContinentString, CompanyRegionString, CompanyStateName)
                End If
            End If
            If Not IsNothing(tempTable) Then
                If tempTable.Rows.Count > 0 Then

                    Dim tmpCompanyArray() As String = CommonAircraftFunctions.SplitUserData(CommonAircraftFunctions.Get_ReferenceCompanyIDs(tempTable), Constants.cCommaDelim)
                    For x As Integer = 0 To UBound(tmpCompanyArray)
                        afileterd = tempTable.Select("acref_comp_id IN (" & CLng(tmpCompanyArray(x).ToString) & ")", "cref_transmit_seq_no")
                        Dim dalTable As DataTable = tempTable.Clone
                        CompanyTitle = ""
                        ' extract and import
                        dalTable.Clear()
                        dalTable.Rows.Clear()

                        For Each atmpDataRow As DataRow In afileterd
                            dalTable.ImportRow(atmpDataRow)
                        Next

                        'Going through filtered Table to display information.
                        If dalTable.Rows.Count > 0 Then

                            ' Clear These
                            companyID = 0
                            strSeqNo = ""
                            strCompanyTypeName = ""
                            CompanyLocation = ""
                            If Not IsDBNull(dalTable.Rows(0).Item("comp_name")) And Not String.IsNullOrEmpty(dalTable.Rows(0).Item("comp_name").ToString) Then
                                fCompany_name = dalTable.Rows(0).Item("comp_name").ToString.Trim
                            Else
                                fCompany_name = ""
                            End If
                            companyID = dalTable.Rows(0).Item("acref_comp_id")


                            CompanyTitle = IIf(Not String.IsNullOrEmpty(dalTable.Rows(0).Item("comp_name").ToString), dalTable.Rows(0).Item("comp_name") & vbNewLine, vbNewLine)
                            CompanyTitle += IIf(Not IsDBNull(dalTable.Rows(0).Item("comp_name_alt_type")), dalTable.Rows(0).Item("comp_name_alt_type").ToString & ": ", "") & IIf(Not IsDBNull(dalTable.Rows(0).Item("comp_name_alt")), dalTable.Rows(0).Item("comp_name_alt").ToString & vbNewLine, "")
                            CompanyTitle += IIf(Not String.IsNullOrEmpty(dalTable.Rows(0).Item("comp_address1").ToString), dalTable.Rows(0).Item("comp_address1") & " ", "")
                            CompanyTitle += IIf(Not String.IsNullOrEmpty(dalTable.Rows(0).Item("comp_address2").ToString), dalTable.Rows(0).Item("comp_address2") & vbNewLine, vbNewLine)
                            CompanyLocation += IIf(Not String.IsNullOrEmpty(dalTable.Rows(0).Item("comp_city").ToString), dalTable.Rows(0).Item("comp_city") & ", ", "")
                            CompanyLocation += IIf(Not String.IsNullOrEmpty(dalTable.Rows(0).Item("comp_state").ToString), dalTable.Rows(0).Item("comp_state") & " ", " ")
                            CompanyLocation += IIf(Not String.IsNullOrEmpty(dalTable.Rows(0).Item("comp_country").ToString), dalTable.Rows(0).Item("comp_country") & " ", " ")

                            CompanyLocation = Trim(Replace(CompanyLocation, "United States", "U.S."))
                            CompanyTitle += IIf(Not String.IsNullOrEmpty(dalTable.Rows(0).Item("comp_city").ToString), dalTable.Rows(0).Item("comp_city") & ", ", "")
                            CompanyTitle += IIf(Not String.IsNullOrEmpty(dalTable.Rows(0).Item("comp_state").ToString), dalTable.Rows(0).Item("comp_state") & " ", " ")

                            CompanyTitle += IIf(Not String.IsNullOrEmpty(dalTable.Rows(0).Item("comp_country").ToString), dalTable.Rows(0).Item("comp_country") & vbNewLine, "")
                            CompanyTitle += IIf(Not String.IsNullOrEmpty(dalTable.Rows(0).Item("comp_phone_office").ToString), vbNewLine & "Office: " & dalTable.Rows(0).Item("comp_phone_office"), "")
                            CompanyTitle += IIf(Not String.IsNullOrEmpty(dalTable.Rows(0).Item("comp_phone_fax").ToString), vbNewLine & "Fax: " & dalTable.Rows(0).Item("comp_phone_fax"), "")
                            CompanyTitle += IIf(Not String.IsNullOrEmpty(dalTable.Rows(0).Item("comp_email_address").ToString), vbNewLine & "Email: " & dalTable.Rows(0).Item("comp_email_address"), "")
                            CompanyTitle += IIf(Not String.IsNullOrEmpty(dalTable.Rows(0).Item("comp_web_address").ToString), vbNewLine & "Website: " & dalTable.Rows(0).Item("comp_web_address"), "")

                            CompanyTitle = Replace(CompanyTitle, "'", "&#39;")

                            'This goes through each matching row in the table
                            For y As Integer = 0 To dalTable.Rows.Count - 1
                                If Not IsDBNull(dalTable.Rows(y).Item("cref_transmit_seq_no")) And Not String.IsNullOrEmpty(dalTable.Rows(y).Item("cref_transmit_seq_no").ToString) Then
                                    fAirRef_transmit_seq_no = CInt(dalTable.Rows(y).Item("cref_transmit_seq_no").ToString)
                                Else
                                    fAirRef_transmit_seq_no = 0
                                End If
                                If Not IsDBNull(dalTable.Rows(y).Item("actype_name")) And Not String.IsNullOrEmpty(dalTable.Rows(y).Item("actype_name").ToString) Then
                                    fAirContactType_name = dalTable.Rows(y).Item("actype_name").ToString.Trim
                                Else
                                    fAirContactType_name = ""
                                End If

                                If Not IsDBNull(dalTable.Rows(y).Item("cref_contact_type")) And Not String.IsNullOrEmpty(dalTable.Rows(y).Item("cref_contact_type").ToString) Then
                                    fAirRef_contact_type = dalTable.Rows(y).Item("cref_contact_type").ToString.Trim
                                Else
                                    fAirRef_contact_type = ""
                                End If

                                If Not IsDBNull(dalTable.Rows(y).Item("cref_owner_percent")) And Not String.IsNullOrEmpty(dalTable.Rows(y).Item("cref_owner_percent").ToString) Then
                                    If dalTable.Rows(y).Item("cref_owner_percent") = 100 Then
                                        fAirRef_owner_percent = 0 'do not display percentage if 100
                                    Else
                                        fAirRef_owner_percent = CDbl(dalTable.Rows(y).Item("cref_owner_percent").ToString)
                                    End If
                                Else
                                    fAirRef_owner_percent = 0
                                End If

                                If String.IsNullOrEmpty(strSeqNo) Then
                                    strSeqNo = fAirRef_transmit_seq_no.ToString
                                Else
                                    strSeqNo &= Constants.cCommaDelim + " " + fAirRef_transmit_seq_no.ToString
                                End If


                                If Not fAirRef_contact_type.Contains("02") And Not fAirRef_contact_type.Contains("66") And Not fAirRef_contact_type.Contains("67") _
                                   And Not fAirRef_contact_type.Contains("68") And Not fAirRef_contact_type.Contains("44") Then

                                    If fAirRef_contact_type.Contains("97") Or fAirRef_contact_type.Contains("17") Or fAirRef_contact_type.Contains("08") Then

                                        strCompanyTypeName &= fAirContactType_name


                                        If fAirRef_owner_percent > 0 Then
                                            strCompanyTypeName &= " <span class='tiny_text'>[<em>" & fAirRef_owner_percent.ToString.Trim & "%</em>]</span>"
                                        End If

                                        strCompanyTypeName &= Constants.cCommaDelim + " "
                                    Else

                                        If fAirRef_contact_type.Contains("66") Or fAirRef_contact_type.Contains("67") Or fAirRef_contact_type.Contains("68") Then
                                            fAirContactType_name = "Additional Company/Contact"
                                        End If

                                        If Not strCompanyTypeName.Contains(fAirContactType_name) Then
                                            strCompanyTypeName &= fAirContactType_name + Constants.cCommaDelim + " "
                                        End If


                                    End If

                                Else

                                    If fAirRef_contact_type.Contains("66") Or fAirRef_contact_type.Contains("67") Or fAirRef_contact_type.Contains("68") Then
                                        fAirContactType_name = "Additional Company/Contact"
                                    End If

                                    If Not strCompanyTypeName.Contains(fAirContactType_name) Then
                                        strCompanyTypeName &= fAirContactType_name + Constants.cCommaDelim + " "
                                    End If

                                End If


                            Next ' y As Integer = 0 To dalTable.Rows.count - 1

                            If Not String.IsNullOrEmpty(strCompanyTypeName) Then
                                strCompanyTypeName = Left(strCompanyTypeName, Len(strCompanyTypeName) - 2)
                            Else
                                strCompanyTypeName = "Additional Company"
                            End If
                            strCompanyTypeName = strCompanyTypeName & ""

                            strCompanyTypeName = Replace(Trim(strCompanyTypeName), "Additional Company/Contact", "Additional Contact")
                            strCompanyTypeName = Replace(Trim(strCompanyTypeName), "Sales Company/Contact", "Sales Contact")
                            strCompanyTypeName = Replace(Trim(strCompanyTypeName), "Charter Company", "Charter")
                            strCompanyTypeName = Replace(Trim(strCompanyTypeName), "Aircraft Management Company", "Aircraft Management")


                            If (HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO And InStr(strCompanyTypeName, "Exclusive Broker") > 0) Or (HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT And InStr(strCompanyTypeName, "Central Agent") > 0) Then
                                strCompanyTypeName = "<span class='purple_background'><span class='label'>" & Replace(Trim(strCompanyTypeName), "Exclusive Broker", "Broker")
                            ElseIf InStr(strCompanyTypeName, "Lessee") > 0 Then
                                strCompanyTypeName = "<span class='orange_background'><span class='label'>" & strCompanyTypeName
                            Else
                                strCompanyTypeName = "<span><span class='label'>" & strCompanyTypeName
                            End If

                        End If

                        If CRMView And CRMAircraft Then
                            strAnswer += "<span class='" & li_class & "'  >" & strCompanyTypeName & ":</span> " & DisplayFunctions.WriteDetailsLink(0, companyID, 0, 0, True, fCompany_name, "", "&SOURCE=CLIENT") & " <span class='tiny'>" & CompanyLocation & "</span></span></span>"
                        Else
                            strAnswer += "<span class='" & li_class & "'  >" & strCompanyTypeName & ":</span> " & DisplayFunctions.WriteDetailsLink(0, companyID, 0, 0, True, fCompany_name, "", "") & " <span class='tiny'>" & CompanyLocation & "</span></span></span>"
                        End If


                    Next
                End If
            Else
                ' LogError(aclsData_Temp.class_error, aclsData_Temp)
            End If
        Catch ex As Exception
            'LogError(ex.Message, aclsData_Temp)
        End Try
        'tempTable = Nothing
        Return strAnswer
    End Function

    ''' <summary>
    ''' Used in several places on the home page, this sets up a uniform way to display aircraft information based on the link. Accepts a datatable with the company information
    ''' already included, builds the link string then goes ahead and sends it back.
    ''' </summary>
    ''' <param name="aTempTable"></param>
    ''' <param name="ShowLink"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Display_Company_Information_For_Link(ByVal aTempTable As DataTable, ByVal ShowLink As Boolean, ByVal rowCount As Integer) As String
        Dim link_text As String = ""
        'I extended this function to include a rowCount integer. This means you can pass a table with more than one row. Just pass the row# you want to read from.

        If Not IsNothing(aTempTable) Then
            If aTempTable.Rows.Count > 0 Then
                If aTempTable.Rows.Count >= rowCount Then
                    link_text = crmWebClient.DisplayFunctions.WriteDetailsLink(0, aTempTable.Rows(rowCount).Item("comp_id"), 0, 0, True, aTempTable.Rows(rowCount).Item("comp_name").ToString, "", "")
                    If Not IsDBNull(aTempTable.Rows(rowCount).Item("comp_city")) Then
                        If aTempTable.Rows(rowCount).Item("comp_city") <> "" Then
                            link_text = link_text & " " & aTempTable.Rows(rowCount).Item("comp_city").ToString
                        End If
                    End If
                    If Not IsDBNull(aTempTable.Rows(rowCount).Item("comp_state")) Then
                        If aTempTable.Rows(rowCount).Item("comp_state") <> "" Then
                            link_text = link_text & ", " & aTempTable.Rows(rowCount).Item("comp_state").ToString
                        End If
                    End If
                End If
            End If
        End If

        Return link_text
    End Function



    Public Function NEW_build_phone_info_full_spec(ByVal Sub_ID As Long, ByVal color As String, Optional ByVal font_size As String = "") As String

        Dim sQuery = New StringBuilder()
        Dim sOutString As StringBuilder = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            sQuery.Append("SELECT top 1 pnum_type, pnum_number_full FROM Subscription INNER JOIN Company ON Subscription.sub_comp_id = Company.comp_id AND Company.comp_journ_id = 0 INNER JOIN Phone_Numbers")
            sQuery.Append(" ON pnum_comp_id = Company.comp_id and pnum_journ_id = comp_journ_id")
            sQuery.Append(" INNER JOIN Phone_Type ON ptype_name = pnum_type ")
            sQuery.Append(" WHERE Subscription.sub_id = " + Sub_ID.ToString)
            sQuery.Append(" AND pnum_journ_id = 0")
            sQuery.Append(" AND pnum_hide_customer = 'N' AND pnum_contact_id = 0 ")
            sQuery.Append(" ORDER BY ptype_seq_no ASC")  ' QUERY EDITED TO DISPLAY TOLL FREE THEN OFFICE THEN FAX 

            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
            SqlConn.Open()
            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 90

            SqlCommand.CommandText = sQuery.ToString
            SqlReader = SqlCommand.ExecuteReader()

            If SqlReader.HasRows Then

                Do While SqlReader.Read
                    sOutString.Append("<tr><td>")
                    '<font class='" & Session("FONT_CLASS_TEXT") & "'>" + SqlReader.Item("pnum_type").ToString + ": </font>"
                    sOutString.Append("<font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>" + SqlReader.Item("pnum_number_full").ToString + "</font>")
                    sOutString.Append("</td></tr>")

                Loop

            End If

            SqlReader.Close()

        Catch ex As Exception
            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in build_phone_info_full_spec " + ex.Message
        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing

            SqlCommand.Dispose()
            SqlCommand = Nothing
        End Try

        Return sOutString.ToString

    End Function


    Public Function CompanyInformationHeaderWithPNG(ByVal nAircraftID As Long, ByRef Title As String, ByRef address_info As String, ByRef logo_link As String, ByRef invisible_label_parent_image As Label, ByVal logo_check As CheckBox, ByVal bWordReport As Boolean, Optional ByRef CompanyName As String = "", Optional ByRef From_Spot As String = "", Optional ByRef Show_Alt_Name As Boolean = False) As String
        CompanyInformationHeaderWithPNG = ""
        ' Dim company_name As String = ""

        Dim sQuery = New StringBuilder()
        Dim sOutString As StringBuilder = New StringBuilder()
        Dim comp_image_file As String = ""
        Dim temp_height As Integer = 0
        Dim temp_width As Integer = 0
        Dim zimage2 As System.Drawing.Image = Nothing
        Dim desired_width As Integer = 500
        Dim desired_height As Integer = 160
        Dim temp_percent1 As Double = 0.0
        Dim temp_percent2 As Double = 0.0
        Dim total_width As Integer = 0
        Dim width_size_total As Integer = 740
        Dim add_pic As String = "Y"
        Dim blow_up As Boolean = False
        Dim temp_calc As Double = 0.0
        Dim font_size_for_address As String = ""
        Dim nAircraftJournalID As Long = 0
        Dim compFileImageLink As String = ""

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim fix_width As String = "840"
        Dim curFile As String = ""
        Dim imgFolder As String = ""
        Try
            If clsGeneral.clsGeneral.isCrmDisplayMode() Then

            End If

            If Trim(From_Spot) = "PDF" Then
                imgFolder = HttpContext.Current.Session.Item("jetnetFullHostName").ToString + "/" + HttpContext.Current.Session.Item("CompanyPicturesFolderVirtualPath") + "/"
            ElseIf HttpContext.Current.Session.Item("jetnetWebSiteType") = crmWebClient.eWebSiteTypes.LOCAL Then
                imgFolder = "https://www.testjetnetevolution.com/" + HttpContext.Current.Session.Item("CompanyPicturesFolderVirtualPath") + "/"
            ElseIf HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CRM Then
                imgFolder = "http://www.jetnetevolution.com/" + HttpContext.Current.Session.Item("CompanyPicturesFolderVirtualPath") + "/"
            Else
                imgFolder = HttpContext.Current.Session.Item("jetnetFullHostName").ToString + "/" + HttpContext.Current.Session.Item("CompanyPicturesFolderVirtualPath") + "/"
            End If


            'SECTION 1 - COMPANY INFO ----------------------------------------------------------------------------------------------------------------------------------- 

            sQuery.Append("SELECT TOP 1 * FROM Company INNER JOIN Subscription ON comp_id = sub_comp_id AND comp_journ_id = 0")
            sQuery.Append(" WHERE sub_id = " + HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString)

            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
            SqlConn.Open()

            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 90

            SqlCommand.CommandText = sQuery.ToString
            SqlReader = SqlCommand.ExecuteReader()

            If SqlReader.HasRows Then

                SqlReader.Read()

                If Not IsDBNull(SqlReader.Item("comp_name")) Then
                    CompanyName = "" & SqlReader.Item("comp_name")
                End If

                address_info = ""

                If Show_Alt_Name = True Then
                    address_info = "<tr valign='top'><td align='left'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>"
                    If Not IsDBNull(SqlReader.Item("comp_name_alt")) Then
                        address_info &= "" & SqlReader.Item("comp_name_alt")
                    End If
                    address_info &= "</font></td></tr>"
                End If

                address_info &= "<tr valign='top'><td align='left'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>"
                If Not IsDBNull(SqlReader.Item("comp_address1")) Then
                    address_info &= "" & SqlReader.Item("comp_address1")
                End If
                address_info &= "</font></td></tr>"


                If Not IsDBNull(SqlReader.Item("comp_logo_flag")) And logo_check.Checked Then
                    If SqlReader.Item("comp_logo_flag").ToString.ToUpper.Contains("Y") And logo_check.Checked Then
                        comp_image_file = HttpContext.Current.Server.MapPath("pictures\company\") & SqlReader.Item("comp_id").ToString '& '".png"
                        compFileImageLink = SqlReader.Item("comp_id").ToString
                    ElseIf logo_check.Checked = True And invisible_label_parent_image.Text <> "" Then
                        comp_image_file = HttpContext.Current.Server.MapPath("pictures\company\") & invisible_label_parent_image.Text '& ".jpg"
                        compFileImageLink = invisible_label_parent_image.Text
                    End If
                ElseIf logo_check.Checked = True And invisible_label_parent_image.Text <> "" Then
                    comp_image_file = HttpContext.Current.Server.MapPath("pictures\company\") & invisible_label_parent_image.Text '& ".jpg"
                    compFileImageLink = invisible_label_parent_image.Text
                End If



                temp_width = 0
                temp_height = 0
                If Trim(comp_image_file) <> "" Then
                    Try
                        curFile = comp_image_file
                        If IO.File.Exists(curFile & ".png") Then
                            curFile = comp_image_file & ".png"
                            compFileImageLink += ".png"
                            zimage2 = System.Drawing.Image.FromFile(comp_image_file & ".png")
                        ElseIf IO.File.Exists(curFile & ".jpg") Then
                            zimage2 = System.Drawing.Image.FromFile(comp_image_file & ".jpg")
                            curFile = comp_image_file & ".jpg"
                            compFileImageLink += ".jpg"
                        Else
                            curFile = ""
                            compFileImageLink += ""
                        End If

                        If curFile <> "" Then

                            temp_width = zimage2.Width
                            temp_height = zimage2.Height

                            If bWordReport = True Then
                                desired_width = 275
                                desired_height = 68
                            Else
                                desired_width = 325
                                desired_height = 85
                            End If

                            If temp_width > desired_width Then
                                temp_percent1 = CDbl(CDbl(desired_width) / CDbl(temp_width))
                                temp_width = CDbl(CDbl(temp_width) * CDbl(temp_percent1))
                                temp_height = CDbl(CDbl(temp_height) * CDbl(temp_percent1))
                            End If

                            If temp_height > desired_height Then
                                temp_percent1 = CDbl(CDbl(desired_height) / CDbl(temp_height))
                                temp_width = CDbl(CDbl(temp_width) * CDbl(temp_percent1))
                                temp_height = CDbl(CDbl(temp_height) * CDbl(temp_percent1))
                            End If
                        End If
                    Catch ex As Exception

                    End Try
                End If


                If temp_width = 0 Then
                    temp_width = 140
                End If

                If curFile <> "" Then
                    If bWordReport Then
                        If Not IsDBNull(SqlReader.Item("comp_logo_flag")) And logo_check.Checked Then
                            If SqlReader.Item("comp_logo_flag").ToString.ToUpper.Contains("Y") And logo_check.Checked Then
                                compFileImageLink = imgFolder + compFileImageLink
                                logo_link = "<img src='" + compFileImageLink + "' width='" & temp_width & "' height='" & temp_height & "'>"
                            ElseIf logo_check.Checked = True And invisible_label_parent_image.Text <> "" Then
                                compFileImageLink = imgFolder + compFileImageLink
                                logo_link = "<img src='" + compFileImageLink + "' width='" & temp_width & "' height='" & temp_height & "'>"
                            End If
                        ElseIf logo_check.Checked = True And invisible_label_parent_image.Text <> "" Then
                            compFileImageLink = imgFolder + compFileImageLink
                            logo_link = "<img src='" + compFileImageLink + "' width='" & temp_width & "' height='" & temp_height & "'>"
                        End If
                    Else
                        If Not IsDBNull(SqlReader.Item("comp_logo_flag")) And logo_check.Checked Then
                            If SqlReader.Item("comp_logo_flag").ToString.ToUpper.Contains("Y") And logo_check.Checked Then
                                compFileImageLink = imgFolder + compFileImageLink
                                logo_link = "<img src='" + compFileImageLink + "' width='" & temp_width & "'>"
                            ElseIf logo_check.Checked = True And invisible_label_parent_image.Text <> "" Then
                                curFile = imgFolder + compFileImageLink
                                logo_link = "<img src='" + compFileImageLink + "' width='" & temp_width & "'>"
                            End If
                        ElseIf logo_check.Checked = True And invisible_label_parent_image.Text <> "" Then
                            compFileImageLink = imgFolder + compFileImageLink
                            logo_link = "<img src='" + compFileImageLink + "' width='" & temp_width & "'>"
                        End If
                    End If
                End If


                If Not IsDBNull(SqlReader.Item("comp_address2")) Then
                    If Trim(SqlReader.Item("comp_address2")) <> "" Then
                        address_info += "<tr><td><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>" + SqlReader.Item("comp_address2").ToString + "</font></td></tr>"
                    End If
                End If

                If Not IsDBNull(SqlReader.Item("comp_city")) Or Not IsDBNull(SqlReader.Item("comp_state")) Or Not IsDBNull(SqlReader.Item("comp_zip_code")) Then
                    address_info += "<tr><td><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>"
                End If

                If Not IsDBNull(SqlReader.Item("comp_city")) Then
                    address_info += "" + SqlReader.Item("comp_city").ToString + ""
                End If

                If Not IsDBNull(SqlReader.Item("comp_state")) Then
                    If Trim(SqlReader.Item("comp_state")) <> "" Then
                        address_info += ", " + SqlReader.Item("comp_state").ToString + ""
                    End If
                End If

                If Not IsDBNull(SqlReader.Item("comp_zip_code")) Then
                    address_info += " " + SqlReader.Item("comp_zip_code").ToString + ""
                End If

                If Not IsDBNull(SqlReader.Item("comp_city")) Or Not IsDBNull(SqlReader.Item("comp_state")) Or Not IsDBNull(SqlReader.Item("comp_zip_code")) Then
                    address_info += "</font></td></tr>"
                End If


                address_info += "<tr valign='top'><td>"
                address_info += "<table cellpadding='0' cellspacing='0' border='0'>"
                address_info += "<tr valign='top'><td align='left'>"
                address_info += "<table cellpadding='0' cellspacing='0' border='0'>"
                address_info += NEW_build_phone_info_full_spec(CLng(HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString), "black")
                address_info += "</table>"
                address_info += "</td><td><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>"
                If Not IsDBNull(SqlReader.Item("comp_web_address")) Then
                    address_info += "&nbsp;&nbsp;|&nbsp;&nbsp;"

                    If SqlReader.Item("comp_web_address").ToString.Trim.ToLower.Contains("www") Then
                        address_info += "<a href='http://" + SqlReader.Item("comp_web_address").ToString.Trim + "' target='new' class=""webAddress"">" + Replace(SqlReader.Item("comp_web_address").ToString.Trim, "www.", "") + "</a>"
                    Else
                        address_info += "<a href='" + SqlReader.Item("comp_web_address").ToString.Trim + "' target='new' class=""webAddress"">" + SqlReader.Item("comp_web_address").ToString.Trim + "</a>"
                    End If

                    address_info += ""

                Else
                    address_info += "&nbsp;"
                End If
                address_info += "</font></td></tr>"
                address_info += "</table>"
                address_info += "</td></tr>"
                address_info += ""

            End If

            SqlReader.Close()

        Catch ex As Exception
            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in build_full_spec_page_header " + ex.Message
        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing

            SqlCommand.Dispose()
            SqlCommand = Nothing
        End Try

    End Function




    Public Function NEW_Header_Comp_Info(ByVal nAircraftID As Long, ByRef Title As String, ByRef address_info As String, ByRef logo_link As String, ByRef invisible_label_parent_image As Label, ByVal logo_check As CheckBox, ByVal bWordReport As Boolean, Optional ByRef CompanyName As String = "", Optional ByRef From_Spot As String = "") As String
        NEW_Header_Comp_Info = ""
        ' Dim company_name As String = ""

        Dim sQuery = New StringBuilder()
        Dim sOutString As StringBuilder = New StringBuilder()
        Dim comp_image_file As String = ""
        Dim temp_height As Integer = 0
        Dim temp_width As Integer = 0
        Dim zimage2 As System.Drawing.Image
        ' Dim zimage3 As System.Drawing.Image
        Dim desired_width As Integer = 500
        Dim desired_height As Integer = 160
        Dim temp_percent1 As Double = 0.0
        Dim temp_percent2 As Double = 0.0
        Dim total_width As Integer = 0
        Dim width_size_total As Integer = 740
        Dim add_pic As String = "Y"
        Dim blow_up As Boolean = False
        Dim temp_calc As Double = 0.0
        Dim font_size_for_address As String = ""
        Dim nAircraftJournalID As Long = 0

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim fix_width As String = "840"

        Try


            'SECTION 1 - COMPANY INFO ----------------------------------------------------------------------------------------------------------------------------------- 

            sQuery.Append("SELECT TOP 1 * FROM Company INNER JOIN Subscription ON comp_id = sub_comp_id AND comp_journ_id = 0")
            sQuery.Append(" WHERE sub_id = " + HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString)

            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
            SqlConn.Open()

            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 90

            SqlCommand.CommandText = sQuery.ToString
            SqlReader = SqlCommand.ExecuteReader()

            If SqlReader.HasRows Then

                SqlReader.Read()

                If Not IsDBNull(SqlReader.Item("comp_name")) Then
                    CompanyName = "" & SqlReader.Item("comp_name")
                End If

                address_info = "<tr valign='top'><td align='left'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>"
                If Not IsDBNull(SqlReader.Item("comp_address1")) Then
                    address_info &= "" & SqlReader.Item("comp_address1")
                End If
                address_info &= "</font></td></tr>"


                If Not IsDBNull(SqlReader.Item("comp_logo_flag")) And logo_check.Checked Then
                    If SqlReader.Item("comp_logo_flag").ToString.ToUpper.Contains("Y") And logo_check.Checked Then
                        comp_image_file = HttpContext.Current.Server.MapPath("pictures\company\") & SqlReader.Item("comp_id").ToString & ".jpg"
                    ElseIf logo_check.Checked = True And invisible_label_parent_image.Text <> "" Then
                        comp_image_file = HttpContext.Current.Server.MapPath("pictures\company\") & invisible_label_parent_image.Text & ".jpg"
                    End If
                ElseIf logo_check.Checked = True And invisible_label_parent_image.Text <> "" Then
                    comp_image_file = HttpContext.Current.Server.MapPath("pictures\company\") & invisible_label_parent_image.Text & ".jpg"
                End If



                temp_width = 0
                temp_height = 0
                If Trim(comp_image_file) <> "" Then
                    Try
                        zimage2 = System.Drawing.Image.FromFile(comp_image_file)
                        temp_width = zimage2.Width
                        temp_height = zimage2.Height

                        If bWordReport = True Then
                            desired_width = 275
                            desired_height = 68
                        Else
                            desired_width = 325
                            desired_height = 85
                        End If



                        If temp_width > desired_width Then
                            temp_percent1 = CDbl(CDbl(desired_width) / CDbl(temp_width))
                            temp_width = CDbl(CDbl(temp_width) * CDbl(temp_percent1))
                            temp_height = CDbl(CDbl(temp_height) * CDbl(temp_percent1))
                        End If

                        If temp_height > desired_height Then
                            temp_percent1 = CDbl(CDbl(desired_height) / CDbl(temp_height))
                            temp_width = CDbl(CDbl(temp_width) * CDbl(temp_percent1))
                            temp_height = CDbl(CDbl(temp_height) * CDbl(temp_percent1))
                        End If
                    Catch ex As Exception

                    End Try
                End If


                If temp_width = 0 Then
                    temp_width = 140
                End If

                If bWordReport Then
                    If Not IsDBNull(SqlReader.Item("comp_logo_flag")) And logo_check.Checked Then
                        If SqlReader.Item("comp_logo_flag").ToString.ToUpper.Contains("Y") And logo_check.Checked Then
                            logo_link = HttpContext.Current.Session.Item("jetnetFullHostName").ToString.Trim + HttpContext.Current.Session.Item("CompanyPicturesFolderVirtualPath")
                            logo_link = "<img src='" + logo_link + "/" + SqlReader.Item("comp_id").ToString + ".jpg' width='" & temp_width & "' height='" & temp_height & "'>"
                        ElseIf logo_check.Checked = True And invisible_label_parent_image.Text <> "" Then
                            logo_link = HttpContext.Current.Session.Item("jetnetFullHostName").ToString.Trim + HttpContext.Current.Session.Item("CompanyPicturesFolderVirtualPath")
                            logo_link = "<img src='" + logo_link + "/" + invisible_label_parent_image.Text + ".jpg' width='" & temp_width & "' height='" & temp_height & "'>"
                        End If
                    ElseIf logo_check.Checked = True And invisible_label_parent_image.Text <> "" Then
                        logo_link = HttpContext.Current.Session.Item("jetnetFullHostName").ToString.Trim + HttpContext.Current.Session.Item("CompanyPicturesFolderVirtualPath")
                        logo_link = "<img src='" + logo_link + "/" + invisible_label_parent_image.Text + ".jpg' width='" & temp_width & "' height='" & temp_height & "'>"
                    End If
                Else
                    If Trim(From_Spot) = "PDF" Then

                        If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CRM Then
                            logo_link = HttpContext.Current.Session.Item("jetnetFullHostName").ToString.Trim + HttpContext.Current.Session.Item("CompanyPicturesFolderVirtualPath")
                        Else
                            logo_link = "http://www.jetnetevolution.com/" & HttpContext.Current.Session.Item("CompanyPicturesFolderVirtualPath")
                        End If

                        'if its from pdf - then hardcode to jetnetevolution.com
                        '  If InStr(HttpContext.Current.Session.Item("jetnetFullHostName"), "testjetnet") > 0 Or HttpContext.Current.Session.Item("jetnetWebSiteType") = crmWebClient.eWebSiteTypes.LOCAL Then
                        ' Else
                        '   logo_link = HttpContext.Current.Session.Item("jetnetFullHostName").ToString.Trim + HttpContext.Current.Session.Item("CompanyPicturesFolderVirtualPath")
                        '  End If

                        logo_link = "<img src='" + logo_link + "/" + SqlReader.Item("comp_id").ToString + ".jpg' width='" & temp_width & "'>"
                    ElseIf Not IsDBNull(SqlReader.Item("comp_logo_flag")) And logo_check.Checked Then
                        If SqlReader.Item("comp_logo_flag").ToString.ToUpper.Contains("Y") And logo_check.Checked Then
                            logo_link = HttpContext.Current.Session.Item("jetnetFullHostName").ToString.Trim + HttpContext.Current.Session.Item("CompanyPicturesFolderVirtualPath")
                            logo_link = "<img src='" + logo_link + "/" + SqlReader.Item("comp_id").ToString + ".jpg' width='" & temp_width & "'>"
                        ElseIf logo_check.Checked = True And invisible_label_parent_image.Text <> "" Then
                            logo_link = HttpContext.Current.Session.Item("jetnetFullHostName").ToString.Trim + HttpContext.Current.Session.Item("CompanyPicturesFolderVirtualPath")
                            logo_link = "<img src='" + logo_link + "/" + invisible_label_parent_image.Text + ".jpg' width='" & temp_width & "'>"
                        End If
                    ElseIf logo_check.Checked = True And invisible_label_parent_image.Text <> "" Then
                        logo_link = HttpContext.Current.Session.Item("jetnetFullHostName").ToString.Trim + HttpContext.Current.Session.Item("CompanyPicturesFolderVirtualPath")
                        logo_link = "<img src='" + logo_link + "/" + invisible_label_parent_image.Text + ".jpg' width='" & temp_width & "'>"
                    End If
                End If


                If Not IsDBNull(SqlReader.Item("comp_address2")) Then
                    If Trim(SqlReader.Item("comp_address2")) <> "" Then
                        address_info += "<tr><td><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>" + SqlReader.Item("comp_address2").ToString + "</font></td></tr>"
                    End If
                End If

                If Not IsDBNull(SqlReader.Item("comp_city")) Or Not IsDBNull(SqlReader.Item("comp_state")) Or Not IsDBNull(SqlReader.Item("comp_zip_code")) Then
                    address_info += "<tr><td><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>"
                End If

                If Not IsDBNull(SqlReader.Item("comp_city")) Then
                    address_info += "" + SqlReader.Item("comp_city").ToString + ""
                End If

                If Not IsDBNull(SqlReader.Item("comp_state")) Then
                    If Trim(SqlReader.Item("comp_state")) <> "" Then
                        address_info += ", " + SqlReader.Item("comp_state").ToString + ""
                    End If
                End If

                If Not IsDBNull(SqlReader.Item("comp_zip_code")) Then
                    address_info += " " + SqlReader.Item("comp_zip_code").ToString + ""
                End If

                If Not IsDBNull(SqlReader.Item("comp_city")) Or Not IsDBNull(SqlReader.Item("comp_state")) Or Not IsDBNull(SqlReader.Item("comp_zip_code")) Then
                    address_info += "</font></td></tr>"
                End If


                address_info += "<tr valign='top'><td>"
                address_info += "<table cellpadding='0' cellspacing='0' border='0'>"
                address_info += "<tr valign='top'><td align='left'>"
                address_info += "<table cellpadding='0' cellspacing='0' border='0'>"
                address_info += NEW_build_phone_info_full_spec(CLng(HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString), "black")
                address_info += "</table>"
                address_info += "</td><td><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>"
                If Not IsDBNull(SqlReader.Item("comp_web_address")) Then
                    address_info += " / "

                    If SqlReader.Item("comp_web_address").ToString.Trim.ToLower.Contains("www") Then
                        address_info += "<a href='http://" + SqlReader.Item("comp_web_address").ToString.Trim + "' target='new'>" + SqlReader.Item("comp_web_address").ToString.Trim + "</a>"
                    Else
                        address_info += "<a href='" + SqlReader.Item("comp_web_address").ToString.Trim + "' target='new'>" + SqlReader.Item("comp_web_address").ToString.Trim + "</a>"
                    End If

                    address_info += ""

                Else
                    address_info += "&nbsp;"
                End If
                address_info += "</font></td></tr>"
                address_info += "</table>"
                address_info += "</td></tr>"
                address_info += ""

            End If

            SqlReader.Close()

        Catch ex As Exception
            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in build_full_spec_page_header " + ex.Message
        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing

            SqlCommand.Dispose()
            SqlCommand = Nothing
        End Try

    End Function

    Public Function NEW_build_full_spec_page_header(ByVal nAircraftID As Long, ByVal Title As String, ByVal address_info As String, ByVal image_ref As String, ByVal amod_id As Long, ByVal NEW_COMP_ID As Long, ByVal bWordReport As Boolean, ByVal word_width As String, ByVal pdf_html_width As String, ByVal check_prepared_for As CheckBox, ByVal chkBlindReport As CheckBox, ByVal prepared_for As TextBox, Optional ByRef Company_Name As String = "", Optional ByVal showAircraftInfo As Boolean = True, Optional ByVal show_company_info As Boolean = True, Optional ByVal show_company_logo As Boolean = True) As String

        'Dim company_name As String = ""

        Dim sQuery = New StringBuilder()
        Dim sOutString As StringBuilder = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim fix_width As String = "840"
        Dim NEW_COMP_ID_name As String = ""

        Try

            sQuery.Append("SELECT TOP 1 Company.comp_name, Company.comp_web_address, Company.comp_email_address FROM Company")
            sQuery.Append(" INNER JOIN Subscription ON sub_comp_id = comp_id AND comp_journ_id = 0")
            sQuery.Append(" WHERE Subscription.sub_id = " + HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString)

            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
            SqlConn.Open()
            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 90

            SqlCommand.CommandText = sQuery.ToString
            SqlReader = SqlCommand.ExecuteReader()

            If SqlReader.HasRows Then
                SqlReader.Read()
                Company_Name = SqlReader.Item("comp_name").ToString.Trim
            End If

            SqlReader.Close()

            If NEW_COMP_ID > 0 Then
                sQuery.Length = 0
                sQuery.Append("SELECT TOP 1 Company.comp_name, Company.comp_web_address, Company.comp_email_address FROM Company")
                sQuery.Append(" WHERE comp_journ_id = 0 and comp_id = " & NEW_COMP_ID)
                SqlCommand.CommandText = sQuery.ToString
                SqlReader = SqlCommand.ExecuteReader()

                If SqlReader.HasRows Then
                    SqlReader.Read()
                    NEW_COMP_ID_name = SqlReader.Item("comp_name").ToString.Trim
                End If

                SqlReader.Close()
            End If


            If bWordReport = True Then
                sOutString.Append("<table width='" & word_width & "' align='center'>")
            Else
                sOutString.Append("<div class=""viewValueExport overlaySep""><table width='100%' align='center' cellpadding=""0"" cellspacing=""0"">")
                ' sOutString.Append("<div class=""viewValueExport overlaySep""><table width='" & pdf_html_width & "' align='center' cellpadding=""0"" cellspacing=""0"">")
            End If



            sOutString.Append("<tr valign='top'><td width='50%'>")

            sOutString.Append("<table cellpadding=""0"" cellspacing=""0"" " & IIf(bWordReport, " class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'", "class=""whiteText""") & ">")

            If show_company_info = True Then
                sOutString.Append("<tr valign='top'><td align='left'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'><font size='+2' color='black'>")
                sOutString.Append(Company_Name + "")
                sOutString.Append("</font></font></td></tr>")

                'The address stuff has all its own tr td items, but needs the outside from this new table
                If Not String.IsNullOrEmpty(address_info.Trim) Then
                    sOutString.Append(address_info.Trim)
                End If
            End If

            If Trim(HttpContext.Current.Request("viewID")) = "998" Then

            Else
                If IsNothing(check_prepared_for) Then
                    If HttpContext.Current.Request("viewID") <> "998" And showAircraftInfo = True Then
                        sOutString.Append("<tr><td nowrap='nowrap' align='right'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>&nbsp;</font></td></tr>")
                    End If
                ElseIf check_prepared_for.Checked = True Then
                    If prepared_for.Text <> "" Then
                        sOutString.Append("<tr><td nowrap='nowrap' align='left'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>Prepared For: </b>" & Trim(prepared_for.Text) & "</font></td></tr>")

                        If Trim(image_ref) <> "" Then
                            image_ref = Replace(image_ref, "150", "120")
                        End If
                    Else
                        If HttpContext.Current.Request("viewID") <> "998" And showAircraftInfo = True Then
                            sOutString.Append("<tr><td nowrap='nowrap' align='right'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>&nbsp;</font></td></tr>")
                        End If
                    End If
                Else
                    If HttpContext.Current.Request("viewID") <> "998" And showAircraftInfo = True Then
                        sOutString.Append("<tr><td nowrap='nowrap' align='right'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>&nbsp;</font></td></tr>")
                    End If
                End If

            End If

            sOutString.Append("</table>")

            sOutString.Append("</td><td width='50%' cellpadding='5' valign='top'>")


            sOutString.Append("<table align='right' valign='top' cellpadding=""0"" cellspacing=""0"">")


            sOutString.Append("<tr><td align='left' nowrap='nowrap'><font class='" & HttpContext.Current.Session("FONT_CLASS_HEADER_NOALIGN") & "' >")

            If CInt(nAircraftID) > 0 Then
                If showAircraftInfo Then
                    Dim acInfoArray() As String = Split(commonEvo.GetAircraftInfo(nAircraftID, False), Constants.cSvrDataSeperator)

                    If Not String.IsNullOrEmpty(acInfoArray(0).ToString) Then
                        sOutString.Append(acInfoArray(0).ToString & " ")
                    End If

                    If Not String.IsNullOrEmpty(acInfoArray(1).ToString) Then
                        sOutString.Append(acInfoArray(1).ToString)
                    End If

                    If Not chkBlindReport.Checked Then
                        If Not String.IsNullOrEmpty(acInfoArray(2).ToString) Then
                            sOutString.Append(" SN #" + acInfoArray(2).ToString)
                        End If
                    End If
                End If

            ElseIf NEW_COMP_ID > 0 Then
                sOutString.Append(NEW_COMP_ID_name & " ")
            End If

            sOutString.Append("</font>")
            sOutString.Append("</td></tr>")

            If Trim(Title) <> "" Then
                sOutString.Append("<tr><td align='right'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>" + Title.Trim + "</font></td></tr>")
            End If

            If show_company_logo = True Then
                If Not String.IsNullOrEmpty(image_ref.Trim) Then
                    sOutString.Append("<tr><td width='150' align='right'>" + image_ref.Trim + "</td></tr>")
                End If
            End If

            sOutString.Append("</table>")


            sOutString.Append("</td></tr></table>")


            If bWordReport = False Then
                sOutString.Append("</div>")
            End If

        Catch ex As Exception
            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in build_full_spec_page_header " + ex.Message
        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing

            SqlCommand.Dispose()
            SqlCommand = Nothing
        End Try

        Return (sOutString.ToString)

    End Function
    Public Function NEW_Insert_Page_Break_PDF(ByRef Counter_For_PDF As Long, ByVal selected_type As String) As String
        NEW_Insert_Page_Break_PDF = ""
        Try

            If Trim(selected_type) = "Word" Then
                NEW_Insert_Page_Break_PDF = "<br style='page-break-before: always'>"
            Else
                NEW_Insert_Page_Break_PDF &= "<table width='100%' align='center' class='break'><tr><td>&nbsp;</td></tr></table>"
            End If

            Counter_For_PDF = 0

        Catch ex As Exception
            ' Response.Write("Error " & ex.Message & " in Insert_Page_Break() As String")
            'clsGeneral.clsGeneral.LogError("Error " & ex.Message & " in Insert_Page_Break() As String", aclsData_Temp)
        End Try
    End Function


    Public Function NEW_build_style_page_full_spec(Optional ByVal is_word As Boolean = False, Optional ByVal is_100_percent As Boolean = False, Optional ByVal report_id As Long = 0, Optional ByVal temp_color As String = "") As String

        '  Dim sServerMapPath As String = ""
        '  Dim sSiteStyleSheet As String = "common\style.css"
        '  sServerMapPath = Server.MapPath(sSiteStyleSheet)
        ' Dim txtFile As New System.IO.StreamReader(sServerMapPath)
        Dim readStyle As New StringBuilder
        ' Dim formatStyle As String = ""
        Dim font_face As String = ""
        Dim font_face_light As String = ""
        Dim header_color As String = ""
        Dim font_color As String = ""
        Dim font_size As Integer = 14
        Dim font_family As String = ""
        Dim pad_bottom As Integer = 0
        Dim Emphasis_Color As String = ""
        Try

            readStyle.Append("<style type='text/css'>")
            readStyle.Append(".break { page-break-before: always; }" + vbCrLf)

            '  If Trim(HttpContext.Current.Request.Item("clouds")) = "Y" Or Trim(HttpContext.Current.Request.Item("clouds2")) = "Y" Or Trim(HttpContext.Current.Request.Item("clouds3")) = "Y" Then
            If is_100_percent = True Then
                readStyle.Append("body {margin: 0%;width: 100%;height: 100%;}")
            End If
            ' readStyle.Append("body {margin: 5%;width: 70%;}")
            ' readStyle.Append("body {margin: 2%;}")

            font_face = " face = 'sans-serif'; "
            If is_word = True Then
                font_size = 10
                pad_bottom = 5
            Else
                font_size = 16
                pad_bottom = 10
            End If


            'font_color = "#736F6E" 
            font_color = " #737373" '"black" 
            header_color = "#4f5050"
            Emphasis_Color = "#446191"


            ' font_face_light = " face = 'sans-serif-light'; "
            font_family = "font-family: 'Maax Standard','Avenir Next','Helvetica','sans-serif' !important;"

            HttpContext.Current.Session("FONT_CLASS_TEXT_SMALL") = "sub_text_small"
            HttpContext.Current.Session("FONT_CLASS_TEXT") = "sub_text"
            HttpContext.Current.Session("FONT_CLASS_TEXT_TITLE") = "bold_sub_text"
            HttpContext.Current.Session("FONT_CLASS_HEADER") = "sub_section_title_text"
            If Trim(temp_color) <> "" Then
                HttpContext.Current.Session("FONT_CLASS_HEADER") &= " " & Trim(temp_color)
            End If
            HttpContext.Current.Session("FONT_CLASS_HEADER_NOCOLOR") = "sub_section_title_text3"
            HttpContext.Current.Session("FONT_CLASS_HEADER_NOALIGN") = "sub_section_title_text2"
            HttpContext.Current.Session("FONT_CLASS_HEADER_BIG") = "section_title_text"
            HttpContext.Current.Session("ROW_CLASS_BOTTOM") = "underlineRow"



            If report_id = 997 Or report_id = 998 Then
                ' FONT STYLES------------------------------------
                readStyle.Append(".sub_text_small{" & font_face & font_family & " font-size: " & (font_size - 2) & "px; color: " & font_color & "; }" & vbCrLf)
                readStyle.Append(".sub_text{" & font_face & font_family & " font-size: " & font_size & "px; color: " & font_color & "; }" & vbCrLf)
                readStyle.Append(".bold_sub_text{" & font_face & font_family & " font-size: " & font_size & "px; color: " & font_color & "; font-weight: bold;}" & vbCrLf)
                readStyle.Append(".sub_section_title_text{text-indent:20px;" & font_face & font_family & " text-transform: uppercase; font-size: " & (font_size + 6) & "px; color: " & Emphasis_Color & "; font-weight: bold;text-align:left;display:block;width:100%;padding-bottom:" & pad_bottom & "px;padding-bottom:" & pad_bottom & "px;}" & vbCrLf)
                readStyle.Append(".sub_section_title_text3{text-indent:20px;" & font_face & font_family & " text-transform: uppercase; font-size: " & (font_size + 6) & "px;  color: " & font_color & "; text-align:right;display:block;width:100%;padding-bottom:" & pad_bottom & "px;padding-bottom:" & pad_bottom & "px;}" & vbCrLf)
                readStyle.Append(".sub_section_title_text2{" & font_face & font_family & " text-transform: uppercase; font-size: " & (font_size + 3) & "px; color: " & Emphasis_Color & "; font-weight: bold;display:block;width:100%;padding-bottom:" & pad_bottom & "px;padding-bottom:" & pad_bottom & "px;}" & vbCrLf)

                readStyle.Append(".section_title_text{" & font_face & font_family & " font-size: " & (font_size + 3) & "px; color: " & font_color & "; font-weight: bold;color: " & header_color & "}" & vbCrLf)
            Else
                ' FONT STYLES------------------------------------
                readStyle.Append(".sub_text_small{" & font_face & font_family & " font-size: " & (font_size - 2) & "px; color: " & font_color & "; }" & vbCrLf)
                readStyle.Append(".sub_text{" & font_face & font_family & " font-size: " & font_size & "px; color: " & font_color & "; }" & vbCrLf)
                readStyle.Append(".bold_sub_text{" & font_face & font_family & " font-size: " & font_size & "px; color: " & font_color & "; font-weight: bold;}" & vbCrLf)
                readStyle.Append(".sub_section_title_text{" & font_face & font_family & " text-transform: uppercase; font-size: " & (font_size + 3) & "px; color: " & Emphasis_Color & "; font-weight: bold;text-align:center;display:block;width:100%;padding-bottom:" & pad_bottom & "px;padding-bottom:" & pad_bottom & "px;}" & vbCrLf)
                readStyle.Append(".sub_section_title_text3{" & font_face & font_family & " text-transform: uppercase; font-size: " & (font_size + 3) & "px;  color: " & font_color & "; text-align:right;display:block;width:100%;padding-bottom:" & pad_bottom & "px;padding-bottom:" & pad_bottom & "px;}" & vbCrLf)
                readStyle.Append(".sub_section_title_text2{" & font_face & font_family & " text-transform: uppercase; font-size: " & (font_size + 3) & "px; color: " & Emphasis_Color & "; font-weight: bold;display:block;width:100%;padding-bottom:" & pad_bottom & "px;padding-bottom:" & pad_bottom & "px;}" & vbCrLf)
                readStyle.Append(".section_title_text{" & font_face & font_family & " font-size: " & (font_size + 2) & "px; color: " & font_color & "; font-weight: bold;}" & vbCrLf)
                readStyle.Append(".gray{" & font_face & font_family & " text-transform: uppercase; font-size: " & (font_size + 3) & "px; color: " & Emphasis_Color & "; font-weight: bold;text-align:center;display:block;width:100%;padding-bottom:" & pad_bottom & "px;padding-bottom:" & pad_bottom & "px;}" & vbCrLf)

            End If
            readStyle.Append(".valueSpec.Simplistic .formatTable th {" & font_face & font_family & " font-size: " & font_size & "px !important; color: " & header_color & "; }" & vbCrLf)

            readStyle.Append(".viewValueExport.Simplistic .formatTable.blue td, .Simplistic.viewValueExport .formatTable td{" & font_face & font_family & " font-size: " & font_size & "px !important; color: " & font_color & "; }" & vbCrLf)
            readStyle.Append(".valueSpec.Simplistic .sub_section_title_text2, .valueSpec.Simplistic .subHeader, .valueSpec.Simplistic .subHeaderBoldTable b {" & font_face & font_family & " font-size: " & font_size & "px !important; color: " & header_color & "; }" & vbCrLf)
            readStyle.Append(".valueSpec.Simplistic .formatTable.smallText, .valueSpec.Simplistic .formatTable.smallText td, .valueSpec.Simplistic .formatTable.smallText th, .valueSpec.Simplistic .formatTable.smallText td .sub_text {" & font_face & font_family & " font-size: " & font_size - 1 & "px !important; color: " & header_color & "; }" & vbCrLf)



            readStyle.Append(".underlineRow {border-bottom:1px solid #bdbdbd;}" & vbCrLf)
            readStyle.Append(".underlineRow td{border-bottom:1px solid #bdbdbd;padding-bottom:5px;padding-top:5px;}" & vbCrLf)



            ' ROW STYLES----------------------------
            ' Session("ROW_CLASS_BOTTOM") = "row_style_bottom"
            '  readStyle.Append(".row_style_bottom{border-bottom-style: solid;}" & vbCrLf)



            'If bWordReport Then
            '  readStyle.Append(".header_text{" & font_face & " font-size: 14pt; color: #736F6E; }" & vbCrLf)
            '  readStyle.Append(".small_header_text{" & font_face & "font-size: 9pt; color: #736F6E; font-weight: bold;} " & vbCrLf)
            '  readStyle.Append(".small_header_text2{" & font_face & "font-size: 9pt; color: #736F6E; font-weight: bold;} " & vbCrLf)
            '  readStyle.Append(".text_text{" & font_face & "font-size: 9pt; color: #736F6E; font-weight: lighter;}" & vbCrLf)
            '  readStyle.Append(".text_text2{" & font_face & "; font-size: 9pt; color: #736F6E; font-weight: lighter;}" & vbCrLf)
            '  readStyle.Append(".white_feat_text{" & font_face & "font-size: 10pt; color: white}" & vbCrLf)
            '  readStyle.Append(".white_feat_header_text{" & font_face & "font-size: 14pt; color: white;}" & vbCrLf)
            'Else
            '  readStyle.Append(".header_text{" & font_face & " font-size: 14pt; color: #736F6E; }" & vbCrLf)
            '  readStyle.Append(".small_header_text{" & font_face & "font-size: 9pt;   color: #736F6E; font-weight: bold;} " & vbCrLf)
            '  readStyle.Append(".small_header_text2{" & font_face & "font-size: 9pt;  color: #736F6E; font-weight: bold;} " & vbCrLf)
            '  readStyle.Append(".text_text{" & font_face & "font-size: 9pt; color: #736F6E; font-weight: lighter;}" & vbCrLf)
            '  readStyle.Append(".text_text2{" & font_face & "font-size: 9pt; color: #736F6E; font-weight: lighter;}" & vbCrLf)
            '  readStyle.Append(".white_feat_text{" & font_face & "font-size: 10pt; color: white}" & vbCrLf)
            '  readStyle.Append(".white_feat_header_text{" & font_face & "font-size: 14pt; color: white;}" & vbCrLf)
            'End If

            'readStyle.Append(".table_specs{font-size:12px;}" + vbCrLf)




            readStyle.Append("</style>" + vbCrLf)

        Catch ex As Exception
            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in build_template_header_full_spec()" + ex.Message
        End Try

        Return vbCrLf + readStyle.ToString + vbCrLf

    End Function

    Public Function create_value_with_label(ByVal title1 As String, ByVal value1 As String, ByVal make_rowcol As Boolean, ByVal use_space As Boolean, ByRef Counter_For_PDF As Integer, ByVal spacer_width As String)

        create_value_with_label = ""

        If make_rowcol = True Then

            Counter_For_PDF = Counter_For_PDF + 1

            create_value_with_label &= "<tr class='" & HttpContext.Current.Session("ROW_CLASS_BOTTOM") & "' valign='top'>"
            '  If use_space = True Then
            'create_value_with_label &= "<td width='2%'><font class='" & Session("FONT_CLASS_TEXT_TITLE") & "'>&nbsp;</font></td>"
            '  End If
            create_value_with_label &= "<td align='left' " & IIf(spacer_width = "", "nowrap", "  width='" & spacer_width & "'") & ">"
        End If


        If Trim(title1) <> "" Then
            create_value_with_label &= "<font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT_TITLE") & "'>"
            create_value_with_label &= title1 & ": </font>"
        End If


        If make_rowcol = True Then
            create_value_with_label &= "&nbsp;</td><td align='left'>"
        End If
        create_value_with_label &= "<font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>" & value1
        create_value_with_label &= "&nbsp;</font>"

        If make_rowcol = True Then
            create_value_with_label &= "</td></tr>"
        End If

    End Function


End Class
