' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/Entity Classes/ContactFunctions.vb $
'$$Author: Amanda $
'$$Date: 5/26/20 4:23p $
'$$Modtime: 5/26/20 4:23p $
'$$Revision: 12 $
'$$Workfile: ContactFunctions.vb $
'
' ********************************************************************************

Public Class ContactFunctions

    Public Shared Sub Display_Contact_Details(ByVal contactTable As DataTable, ByRef contacts_label As Label, ByVal CompanyID As Long, ByVal JournalID As Long, ByRef Master As Object, ByVal UseClass As Boolean, ByVal DisplayLink As Boolean, Optional ByVal displayName As Boolean = True, Optional ByRef ReturnContactName As String = "", Optional ByRef CRMView As Boolean = False, Optional ByRef CRMSOURCE As String = "JETNET", Optional ByVal fromContactDetails As Boolean = False, Optional ByVal OtherID As Long = 0, Optional ByVal OtherCompanyID As Long = 0)
        If Not IsNothing(contactTable) Then
            Dim PhoneTable As New DataTable
            Dim cssString As String = ""
            Dim x As Integer = 0

            Dim txtAlias As String = ""

            Const sEvoPreferencesText As String = " Evolution "
            Const sAeroPreferencesText As String = " Aerodex "
            Const sRotoPreferencesText As String = " Rotodex "
            Const sHeliPreferencesText As String = " Helidex "
            Const sYachtPreferencesText As String = " YachtSpot "
            Const sCRMPreferencesText As String = " CRM "
            Const sAdminPreferencesText As String = " Customer Center "
            Const sMyText As String = "My"

            Dim sPreferencesLinkTitle As String = ""
            Dim sContactName As String = ""
            Dim PictureCounter As Integer = 0
            If HttpContext.Current.Session.Item("jetnetWebHostType") = crmWebClient.eWebHostTypes.EVOLUTION Then

                If HttpContext.Current.Session.Item("localPreferences").AerodexFlag Then
                    If HttpContext.Current.Session.Item("localPreferences").isHeliOnlyProduct Then
                        sPreferencesLinkTitle = sMyText + sRotoPreferencesText
                    Else
                        sPreferencesLinkTitle = sMyText + sAeroPreferencesText
                    End If
                Else
                    If HttpContext.Current.Session.Item("localPreferences").isHeliOnlyProduct Then
                        sPreferencesLinkTitle = sMyText + sHeliPreferencesText
                    Else
                        sPreferencesLinkTitle = sMyText + sEvoPreferencesText
                    End If
                End If

            ElseIf HttpContext.Current.Session.Item("jetnetWebHostType") = crmWebClient.eWebHostTypes.YACHT Then
                sPreferencesLinkTitle = sMyText + sYachtPreferencesText

            ElseIf HttpContext.Current.Session.Item("jetnetWebHostType") = crmWebClient.eWebHostTypes.CRM Then
                sPreferencesLinkTitle = sMyText + sCRMPreferencesText

            ElseIf HttpContext.Current.Session.Item("jetnetWebHostType") = crmWebClient.eWebHostTypes.ADMIN Then
                sPreferencesLinkTitle = sMyText + sAdminPreferencesText
            End If





            If contactTable.Rows.Count > 0 Then
                contacts_label.Text = "<div " + IIf(UseClass = True, "class='Box'", "") + ">" & IIf(UseClass, "<div class=""subHeader padding_left"">CONTACTS" & IIf((HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER) Or (HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE), "/USERS", "") & IIf(fromContactDetails = False And CRMSOURCE = "CLIENT" And clsGeneral.clsGeneral.isCrmDisplayMode = True And (HttpContext.Current.Session.Item("jetnetAppVersion") <> Constants.ApplicationVariable.CUSTOMER_CENTER) And (HttpContext.Current.Session.Item("jetnetAppVersion") <> Constants.ApplicationVariable.HOMEBASE), "<span class=""float_right smallLink upperCase display_inline_block"">+<a href=""javascript:void(0);"" onclick=""load('/edit.aspx?type=contact&Listing=1&action=new&comp_ID=" & CompanyID.ToString & "&source=CLIENT&from=contactDetails', '', 'scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');"">ADD CONTACT</a></span>", "") & "</div>", "")
                ' this is my way of saying that it is called to display a single contact details 
                If contactTable.Rows.Count = 1 And DisplayLink = False Then
                Else
                    '  contacts_label.Text += "<td align='left' valign='top' width='33%'></td><td align='left' valign='top' width='33%'></td><td align='left' valign='top' width='33%'></td></tr>"
                End If
                If fromContactDetails = False Then
                    contacts_label.Text += "<div class=""row remove_margin"">"
                End If
                contacts_label.Text += "<table class=""formatTable blue " & IIf(fromContactDetails = False, "companyTable small", " mainContact") & """ cellpadding=""0"" cellspacing=""0"" " & IIf(fromContactDetails = False, " width=""99%"" ", " width=""100%""") & " align=""right"">"


                For Each r As DataRow In contactTable.Rows
                    Dim AccessedByDate As String = ""
                    Dim passwordbyUser As String = ""
                    If x Mod 2 = 0 Then
                        contacts_label.Text += "</tr><tr>"
                        'contacts_label.Text += "</div><div class=""row remove_margin"">"
                    End If

                    contacts_label.Text += "<td valign=""top"""

                    If (HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER) Or (HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE) Then
                        contacts_label.Text += " class=""adminContactBorder"

                        If Not IsDBNull(r("contact_hide_flag")) Then
                            If r("contact_hide_flag") = "Y" Then
                                contacts_label.Text += " hiddenContact"
                            End If
                        End If
                        contacts_label.Text += """"

                    End If
                    contacts_label.Text += ">"


                    contacts_label.Text += "<table width=""100%"" cellpadding=""0"" cellspacing=""0""><tr>"



                    If contactTable.Rows.Count > 1 Or fromContactDetails = False Then
                        contacts_label.Text += "<td width=""50"" valign=""top"">"
                        PictureCounter += 1
                        contacts_label.Text += "<table width=""50"" align=""left"" class=""imageContactPic"" cellpadding=""0"" cellspacing=""0""><tr><td align=""right"" valign=""middle"">"

                        'If Not IsDBNull(r("conpic_contact_id")) Then
                        '    contacts_label.Text += "&nbsp;&nbsp;" & IIf(DisplayLink = True, "<a " + DisplayFunctions.WriteDetailsLink(0, CompanyID, r("contact_id"), 0, False, "", "", "") + ">", "") + "<img src='/images/camera.png' width='12' title='" + r.Item("contact_first_name").ToString.Trim + "-" + r("conpic_contact_id").ToString.Trim + "-" + r("conpic_id").ToString + " Has a Photo' border='0' />" + IIf(DisplayLink = True, "</a>", "")
                        'End If
                        Dim imgDisplayFolder As String = ""


                        If HttpContext.Current.Session.Item("jetnetWebSiteType") = crmWebClient.eWebSiteTypes.LOCAL Then
                            imgDisplayFolder = "https://www.testjetnetevolution.com/" + HttpContext.Current.Session.Item("ContactPicturesFolderVirtualPath")
                        Else
                            imgDisplayFolder = HttpContext.Current.Application.Item("crmClientSiteData").ClientFullHostName + HttpContext.Current.Session.Item("ContactPicturesFolderVirtualPath")
                        End If

                        Dim TheFile As System.IO.FileInfo
                        Dim contactImageLink As String = ""
                        Dim contactImageFile As String = ""

                        If Not IsDBNull(r.Item("conpic_contact_id")) Then

                            contacts_label.Text += "<div id=""container-" & PictureCounter.ToString & """>"

                            contactImageLink = HttpContext.Current.Session.Item("ContactPicturesFolderVirtualPath") + "/" + HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString + ".jpg"

                            contactImageFile = HttpContext.Current.Server.MapPath(contactImageLink)

                            TheFile = New System.IO.FileInfo(contactImageFile)
                            If TheFile.Exists Then 'is the file actually there?

                                contacts_label.Text += "<img class=""pictureResize"" id=""" & PictureCounter.ToString & """ src=""" + imgDisplayFolder.Trim + "/" + r("conpic_contact_id").ToString + "." + r("conpic_image_type").ToString + """ alt=""" + r.Item("contact_first_name").ToString.Trim + """  title=""" + r.Item("contact_first_name").ToString.Trim + """ border=""0"" />"
                            Else
                                contacts_label.Text += "<img class=""pictureResize"" id=""" & PictureCounter.ToString & """ src=""" + imgDisplayFolder.Trim + "/" + r("conpic_contact_id").ToString + "-" + r("conpic_id").ToString + "." + r("conpic_image_type").ToString + """ onerror=""if (this.src != '/images/person-8x.png') {this.src='/images/person-8x.png';this.border='0';};"" alt=""" + r.Item("contact_first_name").ToString.Trim + """  title=""" + r.Item("contact_first_name").ToString.Trim + """ border=""0"" />"
                            End If
                            contacts_label.Text += "</div>"
                        Else
                            contacts_label.Text += "<img class=""circular--square"" src=""/images/person-8x.png"" alt=""" + r.Item("contact_first_name").ToString.Trim + """  title=""" + r.Item("contact_first_name").ToString.Trim + """ width=""80"" />"
                        End If


                        If (CBool(My.Settings.enableChat)) Then

                            Dim bEnableChat As Boolean = False
                            Dim bUserEnabledChat As Boolean = False
                            Dim nAliasID As Integer = 0

                            ChatManager.CheckAndInitChat(False, bEnableChat) ' checks to see if my chat is enabled

                            If bEnableChat Then

                                ' if my chat IS enabled (show online offline status of user)
                                If Not IsDBNull(r.Item("contact_email_address")) Then

                                    ' check and see if this user has "chat" enabled "before" checking on line status
                                    bUserEnabledChat = ChatManager.userEnabledChat(CompanyID, CLng(r.Item("contact_id").ToString), r.Item("contact_email_address").ToString.Trim, nAliasID)

                                    If bUserEnabledChat And nAliasID > 0 Then ' chat is enabled show online/offline status
                                        If ChatManager.isUserOnLine(r.Item("contact_email_address").ToString.ToLower.Trim, nAliasID) Then

                                            If CLng(HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString) <> CLng(r.Item("contact_id").ToString) Then
                                                contacts_label.Text += "&nbsp;&nbsp;<img src=""/images/user_male.png"" width=""16"" title=""" + sContactName.Trim + " is Online. Click to 'chat' with this user."" alt=""" + sContactName.Trim + " is Online. Click to 'chat' with this user."" onclick='fnStartNewChat(""" + r.Item("contact_email_address").ToString + """," + nAliasID.ToString + ",""" + sContactName.Trim + """);' style=""cursor: pointer;""/>"
                                            Else
                                                contacts_label.Text += "&nbsp;&nbsp;<img src=""/images/user_male.png"" width=""16"" title=""You are Online"" alt=""You are Online"" />"
                                            End If

                                        Else
                                            contacts_label.Text += "&nbsp;&nbsp;<img src=""/images/user_male_gray.png"" width=""16"" title=""" + sContactName.Trim + " is Offline"" alt=""" + sContactName.Trim + " is Offline"" style=""cursor: pointer;""/>"
                                        End If ' if user is on line

                                    End If ' if enable chat

                                End If ' Not IsDBNull(r.Item("contact_email_address")) Then

                            Else ' if my chat isn't enabled (show online offline status of user) but needs to turn the "chat" on for themselves

                                If Not IsDBNull(r.Item("contact_email_address")) Then
                                    ' check and see if this user has "chat" enabled "before" checking on line status
                                    bUserEnabledChat = ChatManager.userEnabledChat(CompanyID, CLng(r.Item("contact_id").ToString), r.Item("contact_email_address").ToString.Trim, nAliasID)

                                    If bUserEnabledChat And nAliasID > 0 Then ' chat is enabled show online/offline status

                                        If ChatManager.isUserOnLine(r.Item("contact_email_address").ToString.ToLower.Trim, nAliasID) Then

                                            If CLng(HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString) <> CLng(r.Item("contact_id").ToString) Then
                                                contacts_label.Text += "&nbsp;&nbsp;<img src=""/images/user_male.png"" width=""16"" title=""" + sContactName.Trim + " is Online"" alt=""" + sContactName.Trim + " is Online"" onclick='alert(""You must *enable* chat to use this feature via " + sPreferencesLinkTitle.Trim + """);' style=""cursor: pointer;""/>"
                                            Else
                                                contacts_label.Text += "&nbsp;&nbsp;<img src=""/images/user_male_gray.png"" width=""16"" title=""You must ""enable"" chat to use this feature via " + sPreferencesLinkTitle.Trim + """ alt=""You must *enable* chat to use this feature via " + sPreferencesLinkTitle.Trim + """ />"
                                            End If 'if contact isn't me

                                        Else
                                            contacts_label.Text += "&nbsp;&nbsp;<img src=""/images/user_male_gray.png"" width=""16"" title=""" + sContactName.Trim + " is Offline"" alt=""" + sContactName.Trim + " is Offline"" onclick='alert(""You must *enable* chat to use this feature via " + sPreferencesLinkTitle.Trim + """);' style=""cursor: pointer;""/>"
                                        End If ' if user is on line

                                    End If ' if enable chat

                                End If ' Not IsDBNull(r.Item("contact_email_address")) Then

                            End If ' my chat is enabled

                        End If ' if on local,jetnettest, yacht-spottest

                        contacts_label.Text += "</td></tr></table>"


                        contacts_label.Text += "</td>"

                    End If

                    contacts_label.Text += "<td valign=""top"">"

                    contacts_label.Text += IIf(UseClass = True, "<div class='header_row'>", "<span class='li_no_bullet'>")



                    contacts_label.Text += IIf(displayName, "<b class='company_title'>" + IIf(DisplayLink = True, "<a " & DisplayFunctions.WriteDetailsLink(0, CompanyID, CLng(r.Item("contact_id").ToString), 0, False, "", "", IIf(CRMSOURCE = "JETNET", "", "&source=CLIENT")) + " class=""noCase emphasisColor"">", ""), "")



                    sContactName = ""
                    sContactName = IIf(Not IsDBNull(r.Item("contact_sirname")), IIf(Not String.IsNullOrEmpty(r.Item("contact_sirname").ToString.Trim), r.Item("contact_sirname").ToString.Trim + " ", ""), "")
                    sContactName += IIf(Not IsDBNull(r.Item("contact_first_name")), r.Item("contact_first_name").ToString.Trim + " ", "")
                    sContactName += IIf(Not IsDBNull(r.Item("contact_middle_initial")), IIf(Not String.IsNullOrEmpty(r.Item("contact_middle_initial").ToString.Trim), r.Item("contact_middle_initial").ToString.Trim + ". ", ""), "")

                    sContactName += IIf(Not IsDBNull(r.Item("contact_last_name")), r.Item("contact_last_name").ToString.Trim, "")

                    'ReturnContactName = IIf(Not String.IsNullOrEmpty(sContactName), sContactName, "")

                    If fromContactDetails Then
                        ReturnContactName = sContactName

                        If CRMView = True Then 'And contactView = False Then
                            If OtherID > 0 Then
                                If CRMSOURCE <> "CLIENT" Then
                                    ReturnContactName += "<span><span class=""float_right""><a href=""/DisplayContactDetail.aspx?compid=" & OtherCompanyID & "&conid=" & OtherID & "&source=CLIENT"""">VIEW CLIENT</a></span></span>"
                                Else
                                    ReturnContactName += "<span><strong>/CLIENT RECORD</strong><span class=""float_right""><a href=""javascript:void(0);"" onclick=""javascript:load('/edit.aspx?type=contact&Listing=1&contact_ID=" & IIf(Not IsDBNull(r.Item("contact_id")), r.Item("contact_id").ToString.Trim, "") & "&comp_ID=" & IIf(Not IsDBNull(r.Item("contact_comp_id")), r.Item("contact_comp_id").ToString.Trim, "") & "&source=CLIENT&from=contactDetails','','scrollbars=yes,menubar=no,height=900,width=940,resizable=yes,toolbar=no,location=no,status=no');return false;"" class=""float_right padding_left""><img src=""images/edit_icon.png"" alt=""Edit Client"" /></a><span class=""float_right pipeDelimeter"">|</span><a href=""/DisplayContactDetail.aspx?compid=" & OtherCompanyID & "&conid=" & OtherID & """ class=""padding_right"">VIEW JETNET</a></span></span>"
                                End If
                                'ElseIf CRMSOURCE <> "CLIENT" Then
                                '  ReturnContactName += "<span><span class=""float_right""></span></span>"
                            ElseIf OtherID = 0 And CRMSOURCE = "CLIENT" Then
                                ReturnContactName += "<span><strong>/CLIENT RECORD</strong><span class=""float_right""><a href=""javascript:void(0);"" onclick=""javascript:load('/edit.aspx?type=contact&Listing=1&contact_ID=" & IIf(Not IsDBNull(r.Item("contact_id")), r.Item("contact_id").ToString.Trim, "") & "&comp_ID=" & IIf(Not IsDBNull(r.Item("contact_comp_id")), r.Item("contact_comp_id").ToString.Trim, "") & "&source=CLIENT&from=contactDetails','','scrollbars=yes,menubar=no,height=900,width=940,resizable=yes,toolbar=no,location=no,status=no');return false;"" class=""float_right padding_left""><img src=""images/edit_icon.png"" alt=""Edit Client"" /></a></span></span>"
                            ElseIf CRMSOURCE = "JETNET" And OtherCompanyID > 0 Then
                                ReturnContactName += "<a href=""javascript:void(0);"" onclick=""javascript:load('/edit.aspx?action=new&type=contact&createClient=true&Listing=1&contact_ID=" & IIf(Not IsDBNull(r.Item("contact_id")), r.Item("contact_id").ToString.Trim, "") & "&comp_ID=" & OtherCompanyID.ToString & "&source=JETNET&from=contactDetails','','scrollbars=yes,menubar=no,height=900,width=940,resizable=yes,toolbar=no,location=no,status=no');return false;"" class=""float_right padding_left""><img src=""images/edit_icon.png"" alt=""Create Client Contact"" /></a>"
                            End If
                        End If

                    End If
                    If displayName Then
                        If Not String.IsNullOrEmpty(Trim(sContactName)) Then
                            contacts_label.Text += IIf(Not IsDBNull(r.Item("contact_sirname")), IIf(Not String.IsNullOrEmpty(r.Item("contact_sirname").ToString.Trim), r.Item("contact_sirname").ToString.Trim + "&nbsp;", ""), "")
                            contacts_label.Text += IIf(Not IsDBNull(r.Item("contact_first_name")), r.Item("contact_first_name").ToString.Trim + "&nbsp;", "")
                            contacts_label.Text += IIf(Not IsDBNull(r.Item("contact_middle_initial")), IIf(Not String.IsNullOrEmpty(r.Item("contact_middle_initial").ToString.Trim), r.Item("contact_middle_initial").ToString.Trim + ".&nbsp;", ""), "")
                            contacts_label.Text += IIf(Not IsDBNull(r.Item("contact_last_name")), r.Item("contact_last_name").ToString.Trim, "")

                            If Not IsDBNull(r.Item("contact_suffix")) Then
                                If r.Item("contact_suffix").ToString.Trim <> "" Then
                                    contacts_label.Text += "&nbsp;" & r.Item("contact_suffix").ToString.Trim
                                End If
                            End If

                        Else
                            contacts_label.Text += IIf(Not IsDBNull(r.Item("contact_title")), IIf(Not String.IsNullOrEmpty(r.Item("contact_title").ToString.Trim), r.Item("contact_title").ToString.Trim + "&nbsp;", ""), "")
                        End If

                        contacts_label.Text += IIf(DisplayLink = True, "</a>", "") + "</b>"
                    End If



                    contacts_label.Text += IIf(UseClass = True, "</div>", "</span>")
                    contacts_label.Text += IIf(Not IsDBNull(r.Item("contact_title")), "<span class=""li_no_bullet"">" + r.Item("contact_title").ToString.Trim + "</span>", "")
                    If DisplayLink = False Then
                        contacts_label.Text += IIf(Not IsDBNull(r.Item("contact_email_address")), "<span class=""li_no_bullet"">" + r.Item("contact_email_address").ToString.Trim + "</span>", "")
                    Else
                        contacts_label.Text += IIf(Not IsDBNull(r.Item("contact_email_address")), "<span class=""li_no_bullet""><a href='mailto:" + r.Item("contact_email_address").ToString.Trim + "'>" + r.Item("contact_email_address").ToString.Trim + "</a></span>", "")
                    End If


                    If fromContactDetails Then
                        contacts_label.Text += "</td></tr><tr><td>"
                    End If
                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    'Phone Company Query
                    PhoneTable = Master.aclsData_Temp.GetPhoneNumbers(CompanyID, CLng(r.Item("contact_id").ToString), IIf(CRMView, CRMSOURCE, "JETNET"), JournalID)
                    If Not IsNothing(PhoneTable) Then
                        If PhoneTable.Rows.Count > 0 Then
                            For Each m As DataRow In PhoneTable.Rows
                                contacts_label.Text += "<span class=""li_no_bullet"">" + IIf(Not IsDBNull(m.Item("pnum_type")), m.Item("pnum_type").ToString.Trim, "") + " <span class=""make-tel-link"">" + IIf(Not IsDBNull(m.Item("pnum_number")), m.Item("pnum_number").ToString.Trim, "") + "</span></span>"
                            Next
                        End If
                    End If
                    PhoneTable.Dispose()

                    ''Icons for ADMIN
                    If (HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER) Or (HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE) Then
                        If Not IsDBNull(r.Item("ACTIVEUSER")) Then
                            If r.Item("ACTIVEUSER") = "YES" Then

                                If Not IsDBNull(r.Item("subins_last_login_date")) Then
                                    AccessedByDate = "Accessed on " & clsGeneral.clsGeneral.TwoPlaceYear(r.Item("subins_last_login_date")) & ", "
                                End If
                                If Not IsDBNull(r.Item("sublogin_password")) Then
                                    passwordbyUser = "PW: " & r("sublogin_password").ToString
                                End If
                                contacts_label.Text += "<span class=""li_no_bullet"">" + AccessedByDate
                                contacts_label.Text += passwordbyUser + "</span>"

                            End If
                        End If

                        contacts_label.Text += "<span class=""float_left"">"
                        If Not IsDBNull(r.Item("SALESFORCE")) Then
                            If r.Item("SALESFORCE") > 0 Then
                                contacts_label.Text += "<img src=""images/salesforce_36dp.png"" alt=""Salesforce"" title=""Salesforce User"" class=""salesforce"" />"
                            End If
                        End If


                        Dim UserLabel As String = ""
                        If Not IsDBNull(r.Item("ACTIVEUSER")) Then
                            If r.Item("ACTIVEUSER") = "YES" Then


                                Dim urlLink As String = "<a href=""javascript:void(0);"" onclick=""javascript:load('/adminSubErrors.aspx?email=" & HttpContext.Current.Server.UrlEncode(r.Item("contact_email_address")) & "','','scrollbars=yes,menubar=no,height=900,width=940,resizable=yes,toolbar=no,location=no,status=no');return false;"">"

                                UserLabel = urlLink & "<i class=""material-icons md-36 active float_right"" title=""Active"">account_circle</i></a>"
                                '

                                If Not IsDBNull(r.Item("subins_admin_flag")) Then
                                    If r.Item("subins_admin_flag") = "Y" Then
                                        UserLabel = urlLink & "<i class=""material-icons md-36 admin float_right"" title=""Admin User"">account_circle</i></a>"
                                    End If
                                End If

                                If Not IsDBNull(r.Item("VALUESUSER")) Then
                                    If r.Item("VALUESUSER") = "Y" Then
                                        UserLabel += "<img src=""images/current_value.png"" alt="" border=""0"" class=""help_cursor float_right"" title=""Values User"" width=""24""  />"
                                    End If
                                End If

                                contacts_label.Text += UserLabel

                            End If
                        End If

                        contacts_label.Text += "</span>"


                    End If


                    'If (HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER) Or (HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE) Then
                    '    contacts_label.Text += "<span class=""li_no_bullet"">" + AccessedByDate
                    '    contacts_label.Text += passwordbyUser + "</span>"
                    'End If

                    contacts_label.Text += "</td>"

                    contacts_label.Text += "</tr></table>"

                    contacts_label.Text += IIf(UseClass = True, "</td>", "") + "</div>"

                    If UseClass = True Then
                        If cssString = "" Then
                            cssString = "alt_row"
                        Else
                            cssString = ""
                        End If
                    End If
                    x += 1
                Next
                contacts_label.Text += "</tr></table>"
                contacts_label.Text += "</div>"
                If fromContactDetails = False Then
                    contacts_label.Text += "</div>"
                End If
            Else
                'contacts_label.Text = "<div " + IIf(UseClass = True, "class='Box'", "") + ">" & IIf(UseClass, "<div class=""subHeader padding_left"">CONTACTS</div>", "")
                contacts_label.Text = "<div " + IIf(UseClass = True, "class='Box'", "") + ">" & IIf(UseClass, "<div class=""subHeader padding_left"">CONTACTS" & IIf(fromContactDetails = False And CRMSOURCE = "CLIENT" And clsGeneral.clsGeneral.isCrmDisplayMode = True, "<span class=""float_right smallLink upperCase display_inline_block"">+<a href=""javascript:void(0);"" onclick=""load('/edit.aspx?type=contact&Listing=1&action=new&comp_ID=" & CompanyID.ToString & "&source=CLIENT&from=contactDetails', '', 'scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');"">ADD CONTACT</a></span>", "") & "</div>", "")
                contacts_label.Text += "<p align='center'>No Contacts Found.</p></div>"
                contacts_label.ForeColor = Drawing.Color.Red
                contacts_label.Font.Bold = True
            End If
        End If
    End Sub

    Public Shared Sub Display_Contact_Details_ChatBox(ByVal contactTable As DataTable, ByRef contacts_label As Label, ByVal CompanyID As Long, ByVal JournalID As Long)

        Dim aclsData_Temp As New clsData_Manager_SQL
        aclsData_Temp.JETNET_DB = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString

        If Not IsNothing(contactTable) Then
            Dim PhoneTable As New DataTable
            Dim cssString As String = ""
            Dim x As Integer = 0

            If contactTable.Rows.Count > 0 Then

                For Each r As DataRow In contactTable.Rows

                    contacts_label.Text += IIf(Not IsDBNull(r("contact_email_address")), "<span class=""li_no_bullet""><a href=""mailto:" + r("contact_email_address").ToString + """ class=""email"">" + r("contact_email_address").ToString + "</a></span>", "")

                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    'Phone Company Query
                    PhoneTable = aclsData_Temp.GetPhoneNumbers(CompanyID, r("contact_id"), "JETNET", JournalID)
                    If Not IsNothing(PhoneTable) Then
                        If PhoneTable.Rows.Count > 0 Then
                            For Each m As DataRow In PhoneTable.Rows
                                contacts_label.Text += "<span class=""li_no_bullet"">" + IIf(Not IsDBNull(m("pnum_type")), "<span class=""tel"">" + m("pnum_type") + "</span>", "") + " <span class=""make-tel-link"">" + IIf(Not IsDBNull(m("pnum_number")), m("pnum_number"), "") + "</span></span>"
                            Next
                        End If
                    End If
                    PhoneTable.Dispose()

                Next

            Else
                contacts_label.Text += "<p align=""center"">No Contacts Found.</p>"
                contacts_label.ForeColor = Drawing.Color.Red
                contacts_label.Font.Bold = True
            End If
        End If
    End Sub


    Public Shared Function DisplayContactNameTitle(ByVal contactFirstName As Object, ByRef contactLastName As Object, ByRef contactTitle As Object, ByRef ContactID As Object, ByRef CompanyID As Object, ByVal LinkNeeded As Boolean)
        Dim resultsString As String = ""

        If IsDBNull(contactFirstName) Then
            contactFirstName = ""
        End If

        If IsDBNull(contactLastName) Then
            contactLastName = ""
        End If

        If Not IsNumeric(ContactID) Then
            ContactID = 0
        End If

        If Not IsNumeric(CompanyID) Then
            CompanyID = 0
        End If


        If String.IsNullOrEmpty(contactFirstName) And String.IsNullOrEmpty(contactLastName) Then
            If Not IsDBNull(contactTitle) Then
                resultsString = contactTitle.ToString
            End If
        Else
            resultsString = contactFirstName.ToString
            If Not String.IsNullOrEmpty(contactLastName) Then
                resultsString += " " & contactLastName.ToString
            End If
        End If

        If LinkNeeded Then
            resultsString = DisplayFunctions.WriteDetailsLink(0, CompanyID, ContactID, 0, True, resultsString, "", "")
        End If

        Return resultsString
    End Function


    Public Shared Sub Display_Contact_Details_label(ByVal contactTable As DataTable, ByRef comp_contacts_string As String, ByVal CompanyID As Long, ByVal JournalID As Long, ByRef Master As Object, ByVal UseClass As Boolean, ByVal DisplayLink As Boolean, Optional ByVal displayName As Boolean = True, Optional ByRef ReturnContactName As String = "", Optional ByRef CRMView As Boolean = False, Optional ByRef CRMSOURCE As String = "JETNET", Optional ByVal fromContactDetails As Boolean = False, Optional ByVal OtherID As Long = 0, Optional ByVal OtherCompanyID As Long = 0)
        If Not IsNothing(contactTable) Then
            Dim PhoneTable As New DataTable
            Dim cssString As String = ""
            Dim x As Integer = 0

            Dim txtAlias As String = ""

            Const sEvoPreferencesText As String = " Evolution "
            Const sAeroPreferencesText As String = " Aerodex "
            Const sRotoPreferencesText As String = " Rotodex "
            Const sHeliPreferencesText As String = " Helidex "
            Const sYachtPreferencesText As String = " YachtSpot "
            Const sCRMPreferencesText As String = " CRM "
            Const sAdminPreferencesText As String = " Customer Center "
            Const sMyText As String = "My"

            Dim sPreferencesLinkTitle As String = ""
            Dim sContactName As String = ""

            If HttpContext.Current.Session.Item("jetnetWebHostType") = crmWebClient.eWebHostTypes.EVOLUTION Then

                If HttpContext.Current.Session.Item("localPreferences").AerodexFlag Then
                    If HttpContext.Current.Session.Item("localPreferences").isHeliOnlyProduct Then
                        sPreferencesLinkTitle = sMyText + sRotoPreferencesText
                    Else
                        sPreferencesLinkTitle = sMyText + sAeroPreferencesText
                    End If
                Else
                    If HttpContext.Current.Session.Item("localPreferences").isHeliOnlyProduct Then
                        sPreferencesLinkTitle = sMyText + sHeliPreferencesText
                    Else
                        sPreferencesLinkTitle = sMyText + sEvoPreferencesText
                    End If
                End If

            ElseIf HttpContext.Current.Session.Item("jetnetWebHostType") = crmWebClient.eWebHostTypes.YACHT Then
                sPreferencesLinkTitle = sMyText + sYachtPreferencesText

            ElseIf HttpContext.Current.Session.Item("jetnetWebHostType") = crmWebClient.eWebHostTypes.CRM Then
                sPreferencesLinkTitle = sMyText + sCRMPreferencesText

            ElseIf HttpContext.Current.Session.Item("jetnetWebHostType") = crmWebClient.eWebHostTypes.ADMIN Then
                sPreferencesLinkTitle = sMyText + sAdminPreferencesText
            End If


            If contactTable.Rows.Count > 0 Then
                '  comp_contacts_string = "<div " + IIf(UseClass = True, "class='Box'", "") + ">" & IIf(UseClass, "<div class=""subHeader padding_left"">CONTACTS</div>", "")
                ' this is my way of saying that it is called to display a single contact details 
                If contactTable.Rows.Count = 1 And DisplayLink = False Then
                Else
                    '  comp_contacts_string += "<td align='left' valign='top' width='33%'></td><td align='left' valign='top' width='33%'></td><td align='left' valign='top' width='33%'></td></tr>"
                End If
                '  If fromContactDetails = False Then
                '    comp_contacts_string += "<div class=""row remove_margin"">"
                '  End If
                comp_contacts_string += "<table cellpadding=""0"" cellspacing=""0"" " & IIf(fromContactDetails = False, " width=""99%"" ", " width=""100%""") & " align=""right"">"


                For Each r As DataRow In contactTable.Rows
                    If x Mod 2 = 0 Then
                        comp_contacts_string += "<tr>"
                        'comp_contacts_string += "</div><div class=""row remove_margin"">"
                    End If

                    comp_contacts_string += "<td valign=""top"">"
                    '  comp_contacts_string += IIf(UseClass = True, "<div class='header_row'>", "<span class='li_no_bullet'>") +
                    comp_contacts_string += "<a " & DisplayFunctions.WriteDetailsLink(0, CompanyID, CLng(r.Item("contact_id").ToString), 0, False, "", "", IIf(CRMSOURCE = "JETNET", "", "&source=CLIENT")) + ">"


                    sContactName = ""
                    sContactName = IIf(Not IsDBNull(r.Item("contact_sirname")), IIf(Not String.IsNullOrEmpty(r.Item("contact_sirname").ToString.Trim), r.Item("contact_sirname").ToString.Trim + " ", ""), "")
                    sContactName += IIf(Not IsDBNull(r.Item("contact_first_name")), r.Item("contact_first_name").ToString.Trim + " ", "")
                    sContactName += IIf(Not IsDBNull(r.Item("contact_middle_initial")), IIf(Not String.IsNullOrEmpty(r.Item("contact_middle_initial").ToString.Trim), r.Item("contact_middle_initial").ToString.Trim + ". ", ""), "")

                    sContactName += IIf(Not IsDBNull(r.Item("contact_last_name")), r.Item("contact_last_name").ToString.Trim, "")

                    'ReturnContactName = IIf(Not String.IsNullOrEmpty(sContactName), sContactName, "")

                    If fromContactDetails Then
                        ReturnContactName = sContactName

                        If CRMView = True Then 'And contactView = False Then
                            If OtherID > 0 Then
                                If CRMSOURCE <> "CLIENT" Then
                                    ReturnContactName += "<span><span class=""float_right""><a href=""/DisplayContactDetail.aspx?compid=" & OtherCompanyID & "&conid=" & OtherID & "&source=CLIENT"""">VIEW CLIENT</a></span></span>"
                                Else
                                    ReturnContactName += "<span><strong>/CLIENT RECORD</strong><span class=""float_right""><a href=""javascript:void(0);"" onclick=""javascript:load('/edit.aspx?type=contact&Listing=1&contact_ID=" & IIf(Not IsDBNull(r.Item("contact_id")), r.Item("contact_id").ToString.Trim, "") & "&comp_ID=" & IIf(Not IsDBNull(r.Item("contact_comp_id")), r.Item("contact_comp_id").ToString.Trim, "") & "&source=CLIENT&from=contactDetails','','scrollbars=yes,menubar=no,height=900,width=940,resizable=yes,toolbar=no,location=no,status=no');return false;"" class=""float_right padding_left""><img src=""images/edit_icon.png"" alt=""Edit Client"" /></a><span class=""float_right pipeDelimeter"">|</span><a href=""/DisplayContactDetail.aspx?compid=" & OtherCompanyID & "&conid=" & OtherID & """ class=""padding_right"">VIEW JETNET</a></span></span>"
                                End If
                                'ElseIf CRMSOURCE <> "CLIENT" Then
                                '  ReturnContactName += "<span><span class=""float_right""></span></span>"
                            ElseIf CRMSOURCE = "JETNET" And OtherCompanyID > 0 Then
                                ReturnContactName += "<a href=""javascript:void(0);"" onclick=""javascript:load('/edit.aspx?action=new&type=contact&createClient=true&Listing=1&contact_ID=" & IIf(Not IsDBNull(r.Item("contact_id")), r.Item("contact_id").ToString.Trim, "") & "&comp_ID=" & OtherCompanyID.ToString & "&source=JETNET&from=contactDetails','','scrollbars=yes,menubar=no,height=900,width=940,resizable=yes,toolbar=no,location=no,status=no');return false;"" class=""float_right padding_left""><img src=""images/edit_icon.png"" alt=""Create Client Contact"" /></a>"
                            End If
                        End If

                    End If
                    If displayName Then
                        If Not String.IsNullOrEmpty(Trim(sContactName)) Then
                            comp_contacts_string += IIf(Not IsDBNull(r.Item("contact_sirname")), IIf(Not String.IsNullOrEmpty(r.Item("contact_sirname").ToString.Trim), r.Item("contact_sirname").ToString.Trim + "&nbsp;", ""), "")
                            comp_contacts_string += IIf(Not IsDBNull(r.Item("contact_first_name")), r.Item("contact_first_name").ToString.Trim + "&nbsp;", "")
                            comp_contacts_string += IIf(Not IsDBNull(r.Item("contact_middle_initial")), IIf(Not String.IsNullOrEmpty(r.Item("contact_middle_initial").ToString.Trim), r.Item("contact_middle_initial").ToString.Trim + ".&nbsp;", ""), "")
                            comp_contacts_string += IIf(Not IsDBNull(r.Item("contact_last_name")), r.Item("contact_last_name").ToString.Trim, "")

                            If Not IsDBNull(r.Item("contact_suffix")) Then
                                If r.Item("contact_suffix").ToString.Trim <> "" Then
                                    comp_contacts_string += "&nbsp;" & r.Item("contact_suffix").ToString.Trim
                                End If
                            End If

                        Else
                            comp_contacts_string += IIf(Not IsDBNull(r.Item("contact_title")), IIf(Not String.IsNullOrEmpty(r.Item("contact_title").ToString.Trim), r.Item("contact_title").ToString.Trim + "&nbsp;", ""), "")
                        End If

                        comp_contacts_string += IIf(DisplayLink = True, "</a>", "") + "</b>"
                    End If

                    If contactTable.Rows.Count > 1 Then

                        comp_contacts_string += "<table align='right'><tr><td align='right'>"

                        If Not IsDBNull(r("conpic_contact_id")) Then
                            comp_contacts_string += "&nbsp;&nbsp;" & IIf(DisplayLink = True, "<a " + DisplayFunctions.WriteDetailsLink(0, CompanyID, r("contact_id"), 0, False, "", "", "") + ">", "") + "<img src='/images/camera.png' width='12' title='" + r.Item("contact_first_name").ToString.Trim + " Has a Photo' border='0' />" + IIf(DisplayLink = True, "</a>", "")
                        End If

                        If (CBool(My.Settings.enableChat)) Then

                            Dim bEnableChat As Boolean = False
                            Dim bUserEnabledChat As Boolean = False
                            Dim nAliasID As Integer = 0

                            ChatManager.CheckAndInitChat(False, bEnableChat) ' checks to see if my chat is enabled

                            If bEnableChat Then

                                ' if my chat IS enabled (show online offline status of user)
                                If Not IsDBNull(r.Item("contact_email_address")) Then

                                    ' check and see if this user has "chat" enabled "before" checking on line status
                                    bUserEnabledChat = ChatManager.userEnabledChat(CompanyID, CLng(r.Item("contact_id").ToString), r.Item("contact_email_address").ToString.Trim, nAliasID)

                                    If bUserEnabledChat And nAliasID > 0 Then ' chat is enabled show online/offline status
                                        If ChatManager.isUserOnLine(r.Item("contact_email_address").ToString.ToLower.Trim, nAliasID) Then

                                            If CLng(HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString) <> CLng(r.Item("contact_id").ToString) Then
                                                comp_contacts_string += "&nbsp;&nbsp;<img src=""/images/user_male.png"" width=""16"" title=""" + sContactName.Trim + " is Online. Click to 'chat' with this user."" alt=""" + sContactName.Trim + " is Online. Click to 'chat' with this user."" onclick='fnStartNewChat(""" + r.Item("contact_email_address").ToString + """," + nAliasID.ToString + ",""" + sContactName.Trim + """);' style=""cursor: pointer;""/>"
                                            Else
                                                comp_contacts_string += "&nbsp;&nbsp;<img src=""/images/user_male.png"" width=""16"" title=""You are Online"" alt=""You are Online"" />"
                                            End If

                                        Else
                                            comp_contacts_string += "&nbsp;&nbsp;<img src=""/images/user_male_gray.png"" width=""16"" title=""" + sContactName.Trim + " is Offline"" alt=""" + sContactName.Trim + " is Offline"" style=""cursor: pointer;""/>"
                                        End If ' if user is on line

                                    End If ' if enable chat

                                End If ' Not IsDBNull(r.Item("contact_email_address")) Then

                            Else ' if my chat isn't enabled (show online offline status of user) but needs to turn the "chat" on for themselves

                                If Not IsDBNull(r.Item("contact_email_address")) Then
                                    ' check and see if this user has "chat" enabled "before" checking on line status
                                    bUserEnabledChat = ChatManager.userEnabledChat(CompanyID, CLng(r.Item("contact_id").ToString), r.Item("contact_email_address").ToString.Trim, nAliasID)

                                    If bUserEnabledChat And nAliasID > 0 Then ' chat is enabled show online/offline status

                                        If ChatManager.isUserOnLine(r.Item("contact_email_address").ToString.ToLower.Trim, nAliasID) Then

                                            If CLng(HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString) <> CLng(r.Item("contact_id").ToString) Then
                                                comp_contacts_string += "&nbsp;&nbsp;<img src=""/images/user_male.png"" width=""16"" title=""" + sContactName.Trim + " is Online"" alt=""" + sContactName.Trim + " is Online"" onclick='alert(""You must *enable* chat to use this feature via " + sPreferencesLinkTitle.Trim + """);' style=""cursor: pointer;""/>"
                                            Else
                                                comp_contacts_string += "&nbsp;&nbsp;<img src=""/images/user_male_gray.png"" width=""16"" title=""You must ""enable"" chat to use this feature via " + sPreferencesLinkTitle.Trim + """ alt=""You must *enable* chat to use this feature via " + sPreferencesLinkTitle.Trim + """ />"
                                            End If 'if contact isn't me

                                        Else
                                            comp_contacts_string += "&nbsp;&nbsp;<img src=""/images/user_male_gray.png"" width=""16"" title=""" + sContactName.Trim + " is Offline"" alt=""" + sContactName.Trim + " is Offline"" onclick='alert(""You must *enable* chat to use this feature via " + sPreferencesLinkTitle.Trim + """);' style=""cursor: pointer;""/>"
                                        End If ' if user is on line

                                    End If ' if enable chat

                                End If ' Not IsDBNull(r.Item("contact_email_address")) Then

                            End If ' my chat is enabled

                        End If ' if on local,jetnettest, yacht-spottest

                        comp_contacts_string += "</td></tr></table>"

                    End If

                    ' comp_contacts_string += IIf(UseClass = True, "</div>", "</span>")
                    comp_contacts_string += IIf(Not IsDBNull(r.Item("contact_title")), "<span class=""li_no_bullet"">" + r.Item("contact_title").ToString.Trim + "</span>", "")
                    If DisplayLink = False Then
                        comp_contacts_string += IIf(Not IsDBNull(r.Item("contact_email_address")), "<span class=""li_no_bullet"">" + r.Item("contact_email_address").ToString.Trim + "</span>", "")
                    Else
                        comp_contacts_string += IIf(Not IsDBNull(r.Item("contact_email_address")), "<span class=""li_no_bullet""><a href='mailto:" + r.Item("contact_email_address").ToString.Trim + "'>" + r.Item("contact_email_address").ToString.Trim + "</a></span>", "")
                    End If


                    If fromContactDetails Then
                        comp_contacts_string += "</td></tr><tr><td>"
                    End If
                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    'Phone Company Query
                    'PhoneTable = Master.aclsData_Temp.GetPhoneNumbers(CompanyID, CLng(r.Item("contact_id").ToString), IIf(CRMView, CRMSOURCE, "JETNET"), JournalID)
                    'If Not IsNothing(PhoneTable) Then
                    '  If PhoneTable.Rows.Count > 0 Then
                    '    For Each m As DataRow In PhoneTable.Rows
                    '      comp_contacts_string += "<span class=""li_no_bullet"">" + IIf(Not IsDBNull(m.Item("pnum_type")), m.Item("pnum_type").ToString.Trim, "") + " <span class=""make-tel-link"">" + IIf(Not IsDBNull(m.Item("pnum_number")), m.Item("pnum_number").ToString.Trim, "") + "</span></span>"
                    '    Next
                    '  End If
                    'End If
                    'PhoneTable.Dispose()
                    '  comp_contacts_string += IIf(UseClass = True, "</td>", "") + "</div>"

                    If UseClass = True Then
                        If cssString = "" Then
                            cssString = "alt_row"
                        Else
                            cssString = ""
                        End If
                    End If
                    x += 1
                Next
                comp_contacts_string += "</tr></table>"
                ' comp_contacts_string += "</div>"
                'If fromContactDetails = False Then
                '  comp_contacts_string += "</div>"
                'End If
            Else
                '    comp_contacts_string = "<div " + IIf(UseClass = True, "class='Box'", "") + ">" & IIf(UseClass, "<div class=""subHeader padding_left"">CONTACTS</div>", "")

                comp_contacts_string += "<p align='center'>No Contacts Found.</p></div>"
                '  contacts_label.ForeColor = Drawing.Color.Red
                ' contacts_label.Font.Bold = True
            End If
        End If
    End Sub
End Class
