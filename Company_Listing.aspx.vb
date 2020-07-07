Partial Public Class Company_Listing
    Inherits System.Web.UI.Page
    Dim aclsData_temp As New clsData_Manager_SQL

    Dim TempTable As New DataTable
    Dim TypeDataTable As New DataTable
    Dim TypeDataHold As New DataTable
    Public Shared masterPage As New Object
    Dim PageNumber As Integer = 1
    Dim PageSort As String = ""
    Dim Yacht As Boolean = False
    Public bUsernameExists As Boolean = False

    ''' <summary>
    ''' Company Listing Initialization Page.
    ''' Checks for redirection based on url strings/clears sessions.
    ''' Fills the listboxes, Fills the other subscription class. 
    ''' Minor error catching added
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Company_Listing_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Try

            If Session.Item("crmUserLogon") <> True Then
                Response.Redirect("Default.aspx", False)
            Else

                If Not Page.ClientScript.IsClientScriptBlockRegistered("masonryPostFolder") Then
                    Dim masonryStr As String = "function loadMasonry() { " & vbNewLine

                    masonryStr += " var grid = document.querySelector('.grid');" & vbNewLine
                    masonryStr += " var msnry = new Masonry(grid, {" & vbNewLine
                    masonryStr += " itemSelector: '.grid-item'," & vbNewLine
                    masonryStr += " columnWidth: '.grid-item'," & vbNewLine
                    masonryStr += " gutter: 10," & vbNewLine
                    masonryStr += " horizontalOrder: true," & vbNewLine
                    masonryStr += " percentPosition: true" & vbNewLine
                    masonryStr += " });" & vbNewLine
                    masonryStr += "setTimeout(function(){msnry.layout();},300);"
                    masonryStr += " };" & vbNewLine

                    System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "masonryPostFolder", masonryStr, True)
                End If


                If (Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER) Or (Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE) Then

                    goto_subscriberSearch.Visible = True
                    service_usedLabel.Visible = True
                    service_used.Visible = True
                    customer_targets_panel.Visible = True

                    company_custom_fields.Visible = False
                Else
                    If HttpContext.Current.Session.Item("localUser").crmUser_Evo_MPM_Flag Then
                        company_custom_fields.Visible = True
                        Call ToggleCustomFields()
                    End If
                End If

                If Not IsNothing(Request.Item("restart")) Then
                    If Not String.IsNullOrEmpty(Request.Item("restart").ToString) Then
                        If Request.Item("restart") = "1" Then
                            ResetPage()
                        End If
                    End If
                End If

                If Page.Request.Form("complete_search") = "Y" Then
                    clsGeneral.clsGeneral.ClearSavedSelection()
                ElseIf Page.Request.Form("project_search") = "Y" Then
                    If IsNumeric(Page.Request.Form("project_id")) Then
                        If Page.Request.Form("project_id") <> 0 Then
                            clsGeneral.clsGeneral.ClearSavedSelection()
                        End If
                    End If
                End If


                masterPage.UpdateHelpLink(clsGeneral.clsGeneral.CreateEvoHelpLink("Company List", True))

                If Not Page.IsPostBack Then

                    If (Session.Item("jetnetAppVersion") <> Constants.ApplicationVariable.CUSTOMER_CENTER) And (Session.Item("jetnetAppVersion") <> Constants.ApplicationVariable.HOMEBASE) Then
                        If clsGeneral.clsGeneral.isCrmDisplayMode Then
                            typeOfSearch.Visible = True
                            searchTypeDropdown.SelectedValue = "B"
                        End If
                    End If

                    'Add help button text here: 7/20/15
                    'company_help_text.Text = clsGeneral.clsGeneral.CreateEvoHelpLink("Company List")

                    If (Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER) Or (Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE) Then
                        'Fills the Company Listboxes (not in the control)
                        FillListBoxes(company_relationship, company_business, mobile_company_business,
                                      company_contact_title, comp_certifications,
                                      comp_member_accred, service_used, targets_services_used)
                    Else
                        'Fills the Company Listboxes (not in the control)
                        FillListBoxes(company_relationship, company_business, mobile_company_business,
                                      company_contact_title, comp_certifications,
                                      comp_member_accred, Nothing, Nothing)
                    End If


                    'This needs to be put in and loaded for now. Hopefully whenever the session variables are the same, this can go away.
                    If Not Session.Item("localPreferences").loadUserSession("", CLng(Session.Item("localUser").crmSubSubID.ToString), Session.Item("localUser").crmUserLogin.ToString, CLng(Session.Item("localUser").crmSubSeqNo.ToString), CLng(Session.Item("localUser").crmUserContactID.ToString)) Then
                        Response.Write("error in load preferences : ")
                    End If



                    ''Polling the folder data information

                    If Page.Request.Form("project_search") = "Y" Then

                        Dim folderID As Long = 0
                        Dim FoldersTableData As New DataTable
                        Dim cfolderData As String = ""
                        Dim FolderSource As String = "JETNET"

                        FolderInformation.Text = ""
                        FolderInformation.Visible = False
                        folderID = Page.Request.Form("project_id")

                        If Not String.IsNullOrEmpty(Page.Request.Form("cfolder_source")) Then
                            FolderSource = Page.Request.Form("cfolder_source")
                        End If

                        If folderID <> 0 Then

                            If FolderSource = "JETNET" Then
                                FoldersTableData = masterPage.aclsData_Temp.GetEvolutionFolderssBySubscription(folderID, Session.Item("localUser").crmUserLogin, Session.Item("localUser").crmSubSubID, Session.Item("localUser").crmSubSeqNo, "", 0, Nothing, "")
                                If Not IsNothing(FoldersTableData) Then
                                    If FoldersTableData.Rows.Count > 0 Then
                                        cfolderData = FoldersTableData.Rows(0).Item("cfolder_data").ToString
                                    End If
                                End If

                            Else
                                cfolderData = translateClientToJetnet(Page.Request.Form("cfolder_data"))
                                If cfolderData <> "" Then
                                    Dim UserTableCheck As DataTable
                                    UserTableCheck = masterPage.aclsData_temp.Get_Client_User_By_Email_Address(HttpContext.Current.Session.Item("localUser").crmLocalUserEmailAddress)
                                    If Not IsNothing(UserTableCheck) Then
                                        If UserTableCheck.Rows.Count > 0 Then
                                            FoldersTableData = masterPage.aclsdata_temp.Get_Client_Folders_ByID(UserTableCheck.Rows(0).Item("cliuser_id"), folderID)
                                        End If
                                    End If
                                End If

                            End If

                            If Not IsNothing(FoldersTableData) Then
                                If FoldersTableData.Rows.Count > 0 Then
                                    If FoldersTableData.Rows(0).Item("cfolder_method").ToString = "S" Then
                                        comp_folder_name.Text = FoldersTableData.Rows(0).Item("cfolder_name").ToString
                                    End If
                                    If cfolderData <> "" Then
                                        'Fills up the applicable folder Information pulled from the cfolder data field
                                        DisplayFunctions.FillUpFolderInformation(Table4, close_current_folder, cfolderData, FolderInformation, FoldersTableData, True, False, False, False, False, Company_Collapse_Panel, company_actions_submenu_dropdown, Nothing, StaticFolderNewSearchLabel, Company_Control_Panel, "", False, False, False, FolderSource)
                                    End If
                                End If
                            End If


                        Else
                            'Summary Search
                            'We need to build the cData from the request object because there is technically no created folder.
                            For Each name As String In Request.Form.AllKeys 'This will loop through all the keys.
                                If name <> "project_id" And name <> "project_search" Then
                                    Dim value As String = Request.Form(name)
                                    If cfolderData <> "" Then
                                        cfolderData += "!~!"
                                    End If
                                    cfolderData += name & "=" & value
                                End If
                            Next

                            If cfolderData <> "" Then
                                DisplayFunctions.RefillUpFolderInformation(False, cfolderData, Company_Collapse_Panel, Company_Control_Panel)
                            End If
                        End If


                    End If
                End If

            End If

        Catch ex As Exception

            Dim previousException As String = ex.Message.Trim

            Try

                If Not IsNothing(masterPage) Then
                    masterPage.LogError(Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " : " + ex.Message.Trim)
                Else
                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " : " + ex.Message.Trim
                End If

            Catch ex2 As Exception

                commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " : Previous Exception Thrown[" + previousException.Trim + "] : Exception Thrown[" + ex2.Message.Trim + "]")

            End Try
        End Try

    End Sub

    Private Sub ToggleCustomFields()

        Dim aTempTable As New DataTable
        ' Dim masterPage As main_site = DirectCast(Page.Master, main_site)
        Try

            If Not IsNothing(HttpContext.Current.Session.Item("jetnetServerNotesDatabase")) Then


                Dim currentHeight As Double = company_custom_fields.Height.Value

                '  aclsData_temp.client_DB = Application.Item("crmClientDatabase")
                aclsData_temp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase")

                aTempTable = aclsData_temp.Get_Client_Preferences()
                If Not IsNothing(aTempTable) Then
                    If aTempTable.Rows.Count > 0 Then
                        For Each r As DataRow In aTempTable.Rows
                            'If Not IsDBNull(r("clipref_ac_custom_1_use")) Then
                            '    If r("clipref_ac_custom_1_use") = "Y" Then
                            '        currentHeight += 30
                            '        custom_pref_name1.Visible = True
                            '        custom_pref_text1.Visible = True
                            '        custom_pref_name1.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_1")), r("clipref_ac_custom_1"), "")) & ":"
                            '    Else
                            '        custom_pref_name1.Visible = False
                            '        custom_pref_text1.Visible = False
                            '        custom_pref_name1.Text = ""
                            '    End If
                            'End If

                            'If Not IsDBNull(r("clipref_ac_custom_2_use")) Then
                            '    If r("clipref_ac_custom_2_use") = "Y" Then
                            '        custom_pref_name2.Visible = True
                            '        custom_pref_text2.Visible = True
                            '        custom_pref_name2.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_2")), r("clipref_ac_custom_2"), "")) & ":"
                            '    Else
                            '        custom_pref_name2.Visible = False
                            '        custom_pref_text2.Visible = False
                            '        custom_pref_name2.Text = ""
                            '    End If
                            'End If

                            'If Not IsDBNull(r("clipref_ac_custom_3_use")) Then
                            '    If r("clipref_ac_custom_3_use") = "Y" Then
                            '        currentHeight += 30
                            '        custom_pref_name3.Visible = True
                            '        custom_pref_text3.Visible = True
                            '        custom_pref_name3.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_3")), r("clipref_ac_custom_3"), "")) & ":"
                            '    Else
                            '        custom_pref_name3.Visible = False
                            '        custom_pref_text3.Visible = False
                            '        custom_pref_name3.Text = ""
                            '    End If
                            'End If

                            'If Not IsDBNull(r("clipref_ac_custom_4_use")) Then
                            '    If r("clipref_ac_custom_4_use") = "Y" Then
                            '        custom_pref_name4.Visible = True
                            '        custom_pref_text4.Visible = True
                            '        custom_pref_name4.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_4")), r("clipref_ac_custom_4"), "")) & ":"
                            '    Else
                            '        custom_pref_name4.Visible = False
                            '        custom_pref_text4.Visible = False
                            '        custom_pref_name4.Text = ""
                            '    End If
                            'End If


                            'If Not IsDBNull(r("clipref_ac_custom_5_use")) Then
                            '    If r("clipref_ac_custom_5_use") = "Y" Then
                            '        currentHeight += 30
                            '        custom_pref_name5.Visible = True
                            '        custom_pref_text5.Visible = True
                            '        custom_pref_name5.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_5")), r("clipref_ac_custom_5"), "")) & ":"
                            '    Else
                            '        custom_pref_name5.Visible = False
                            '        custom_pref_text5.Visible = False
                            '        custom_pref_name5.Text = ""
                            '    End If
                            'End If


                            'If Not IsDBNull(r("clipref_ac_custom_6_use")) Then
                            '    If r("clipref_ac_custom_6_use") = "Y" Then
                            '        custom_pref_name6.Visible = True
                            '        custom_pref_text6.Visible = True
                            '        custom_pref_name6.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_6")), r("clipref_ac_custom_6"), "")) & ":"
                            '    Else
                            '        custom_pref_name6.Visible = False
                            '        custom_pref_text6.Visible = False
                            '        custom_pref_name6.Text = ""
                            '    End If
                            'End If

                            'If Not IsDBNull(r("clipref_ac_custom_7_use")) Then
                            '    If r("clipref_ac_custom_7_use") = "Y" Then
                            '        currentHeight += 30
                            '        custom_pref_name7.Visible = True
                            '        custom_pref_text7.Visible = True
                            '        custom_pref_name7.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_7")), r("clipref_ac_custom_7"), "")) & ":"
                            '    Else
                            '        custom_pref_name7.Visible = False
                            '        custom_pref_text7.Visible = False
                            '        custom_pref_name7.Text = ""
                            '    End If
                            'End If


                            'If Not IsDBNull(r("clipref_ac_custom_8_use")) Then
                            '    If r("clipref_ac_custom_8_use") = "Y" Then
                            '        custom_pref_name8.Visible = True
                            '        custom_pref_text8.Visible = True
                            '        custom_pref_name8.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_8")), r("clipref_ac_custom_8"), "")) & ":"
                            '    Else
                            '        custom_pref_name8.Visible = False
                            '        custom_pref_text8.Visible = False
                            '        custom_pref_name8.Text = ""
                            '    End If
                            'End If

                            'If Not IsDBNull(r("clipref_ac_custom_9_use")) Then
                            '    If r("clipref_ac_custom_9_use") = "Y" Then
                            '        currentHeight += 30
                            '        custom_pref_name9.Visible = True
                            '        custom_pref_text9.Visible = True
                            '        custom_pref_name9.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_9")), r("clipref_ac_custom_9"), "")) & ":"
                            '    Else
                            '        custom_pref_name9.Visible = False
                            '        custom_pref_text9.Visible = False
                            '        custom_pref_name9.Text = ""
                            '    End If
                            'End If

                            'If Not IsDBNull(r("clipref_ac_custom_10_use")) Then
                            '    If r("clipref_ac_custom_10_use") = "Y" Then

                            '        custom_pref_name10.Visible = True
                            '        custom_pref_text10.Visible = True
                            '        custom_pref_name10.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_10")), r("clipref_ac_custom_10"), "")) & ":"
                            '    Else
                            '        custom_pref_name10.Visible = False
                            '        custom_pref_text10.Visible = False
                            '        custom_pref_name10.Text = ""
                            '    End If
                            'End If 

                            If Not IsDBNull(r("clipref_category1_use")) Then
                                If r("clipref_category1_use") = "Y" Then
                                    currentHeight += 30
                                    custom_pref_name1.Visible = True
                                    custom_pref_text1.Visible = True
                                    custom_pref_name1.Text = CStr(IIf(Not IsDBNull(r("clipref_category1_name")), r("clipref_category1_name"), "")) & ":"
                                Else
                                    custom_pref_name1.Visible = False
                                    custom_pref_text1.Visible = False
                                    custom_pref_name1.Text = ""
                                End If
                            End If

                            If Not IsDBNull(r("clipref_category2_use")) Then
                                If r("clipref_category2_use") = "Y" Then
                                    custom_pref_name2.Visible = True
                                    custom_pref_text2.Visible = True
                                    custom_pref_name2.Text = CStr(IIf(Not IsDBNull(r("clipref_category2_name")), r("clipref_category2_name"), "")) & ":"
                                Else
                                    custom_pref_name2.Visible = False
                                    custom_pref_text2.Visible = False
                                    custom_pref_name2.Text = ""
                                End If
                            End If

                            If Not IsDBNull(r("clipref_category3_use")) Then
                                If r("clipref_category3_use") = "Y" Then
                                    currentHeight += 30
                                    custom_pref_name3.Visible = True
                                    custom_pref_text3.Visible = True
                                    custom_pref_name3.Text = CStr(IIf(Not IsDBNull(r("clipref_category3_name")), r("clipref_category3_name"), "")) & ":"
                                Else
                                    custom_pref_name3.Visible = False
                                    custom_pref_text3.Visible = False
                                    custom_pref_name3.Text = ""
                                End If
                            End If

                            If Not IsDBNull(r("clipref_category4_use")) Then
                                If r("clipref_category4_use") = "Y" Then
                                    custom_pref_name4.Visible = True
                                    custom_pref_text4.Visible = True
                                    custom_pref_name4.Text = CStr(IIf(Not IsDBNull(r("clipref_category4_name")), r("clipref_category4_name"), "")) & ":"
                                Else
                                    custom_pref_name4.Visible = False
                                    custom_pref_text4.Visible = False
                                    custom_pref_name4.Text = ""
                                End If
                            End If


                            If Not IsDBNull(r("clipref_category5_use")) Then
                                If r("clipref_category5_use") = "Y" Then
                                    currentHeight += 30
                                    custom_pref_name5.Visible = True
                                    custom_pref_text5.Visible = True
                                    custom_pref_name5.Text = CStr(IIf(Not IsDBNull(r("clipref_category5_name")), r("clipref_category5_name"), "")) & ":"
                                Else
                                    custom_pref_name5.Visible = False
                                    custom_pref_text5.Visible = False
                                    custom_pref_name5.Text = ""
                                End If
                            End If


                            custom_pref_name6.Visible = False
                            custom_pref_text6.Visible = False
                            custom_pref_name6.Text = ""

                            custom_pref_name7.Visible = False
                            custom_pref_text7.Visible = False
                            custom_pref_name7.Text = ""

                            custom_pref_name8.Visible = False
                            custom_pref_text8.Visible = False
                            custom_pref_name8.Text = ""

                            custom_pref_name9.Visible = False
                            custom_pref_text9.Visible = False
                            custom_pref_name9.Text = ""

                            custom_pref_name10.Visible = False
                            custom_pref_text10.Visible = False
                            custom_pref_name10.Text = ""

                        Next

                    Else
                        If aclsData_temp.class_error <> "" Then
                            '  error_string = masterpage.aclsData_Temp.class_error
                            ' masterpage.LogError("AircraftSearch.ascx.vb - ToggleCustomFields() - " & error_string)
                        End If
                        ' masterPage.display_error()
                    End If

                    If company_custom_fields.Height.Value <> currentHeight Then
                        currentHeight += 10 'buffer for custom fields header.
                        company_custom_fields.Height = currentHeight
                    Else
                        advanced_search_categories.Visible = False 'toggle custom fields off
                    End If

                End If
            End If
        Catch ex As Exception
            ' error_string = "AircraftSearch.ascx.vb - ToggleCustomFields() " & ex.Message
            '  masterpage.LogError(error_string)
        End Try
    End Sub

    ''' <summary>
    ''' Sub to reset page.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ResetPage()
        clsGeneral.clsGeneral.ClearSavedSelection()
        Response.Redirect("Company_Listing.aspx")
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            Dim Results_Table As New DataTable
            If Session.Item("crmUserLogon") <> True Then
                Response.Redirect("Default.aspx", False)
            Else

                Dim FoldersTable As New DataTable
                aclsData_temp = New clsData_Manager_SQL
                aclsData_temp.JETNET_DB = Session.Item("jetnetClientDatabase") 'Application.Item("crmJetnetDatabase")

                'set the control  up
                'Is base is for the aircraft base
                'Is view is for the view
                'Otherwise (if both our false) it defaults to company listing
                'Show inactive countries is useful on the history search bar, in case a historical record uses a defunct country.
                viewCCSTDropDowns.setIsBase(False)
                viewCCSTDropDowns.setIsView(False)
                viewCCSTDropDowns.setListSize(10)
                viewCCSTDropDowns.setShowInactiveCountries(False)
                viewCCSTDropDowns.setFirstControl(True)

                company_search.OnClientClick = "javascript:FillStateHiddenValue(2); $('#" & divTabLoading.ClientID & "').css( ""display"", ""block"" ); ChangeTheMouseCursorOnItemParentDocument('cursor_wait');"

                If Session.Item("isMobile") = True Then

                    If Not Page.IsPostBack Then
                        companyNameCell.Attributes.Add("style", "padding-left:5px;")
                        company_name.Attributes.Add("style", "width:100%;float:right;")
                        businessTypeAnswerCell.Width = "50%"
                        businessTypeCell.Width = "50%"
                        Dim CountryTable As DataTable = aclsData_temp.Get_Jetnet_Country()

                        If Not IsNothing(CountryTable) Then
                            If CountryTable.Rows.Count > 0 Then
                                clsGeneral.clsGeneral.Populate_Dropdown(CountryTable, mobileCountryOptions, "clicountry_name", "clicountry_name", False)
                            End If
                        End If

                        Dim StateTable As DataTable = aclsData_temp.Get_Jetnet_State()

                        If Not IsNothing(StateTable) Then
                            If StateTable.Rows.Count > 0 Then
                                clsGeneral.clsGeneral.Populate_Dropdown(StateTable, mobileStateOptions, "client_state", "client_state", False)
                            End If
                        End If
                    End If

                    Dim dropdownString As New StringBuilder
                    dropdownString.Append("$('#mobileRadioButtonAnswer input').click(function () {")
                    dropdownString.Append("if ($(""#mobileRadioButtonAnswer input:radio:checked"").val() == ""Yes"") { ")
                    dropdownString.Append("$(""#mobileHide1"").removeClass(""display_none"");")
                    dropdownString.Append("$(""#mobileHide2"").removeClass(""display_none"");")
                    dropdownString.Append("} else {")
                    dropdownString.Append("$(""#" & mobile_company_relationship.ClientID & """).val('');")
                    dropdownString.Append("$(""#mobileHide1"").addClass(""display_none"");")
                    dropdownString.Append("$(""#mobileHide2"").addClass(""display_none"");")
                    dropdownString.Append("}")
                    dropdownString.Append("});")
                    If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaCompanyRelationshipsToAC) Then
                        If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaCompanyRelationshipsToAC) Then
                            dropdownString.Append("$(""#mobileHide1"").removeClass(""display_none"");")
                            dropdownString.Append("$(""#mobileHide2"").removeClass(""display_none"");")
                        End If
                    End If
                    If Not Page.ClientScript.IsClientScriptBlockRegistered("chosenDropdowns") Then
                        System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "chosenDropdowns", dropdownString.ToString, True)
                    End If

                End If

                'Set up bars to display correctly.
                If Not Page.IsPostBack Then

                    If Company_Criteria.Visible = True Then

                        If Session.Item("isMobile") Then
                            searchCell.ColumnSpan = 3
                            comp_city.Width = Unit.Percentage(100D)
                        End If




                        'Toggles checkboxes on or off.
                        If Session.Item("localSubscription").crmHelicopter_Flag <> True Then
                            comp_product_helicopter_flag.Visible = False
                            comp_product_helicopter_flag.Checked = False
                        End If

                        If Session.Item("localSubscription").crmBusiness_Flag <> True Then
                            comp_product_business_flag.Visible = False
                            comp_product_business_flag.Checked = False
                        End If
                        If Session.Item("localSubscription").crmCommercial_Flag <> True Then
                            comp_product_commercial_flag.Visible = False
                            comp_product_commercial_flag.Checked = False
                        End If

                        ' dissapear if only yacht
                        If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT Then ' if in yacht spot. 

                            ' if all 3 are false, then it looks like you can see all

                            ' if all 3 are false and bus or comm = true then show all of them
                            If Session.Item("localSubscription").crmBusiness_Flag = True Then
                                If Trim(Session.Item("localSubscription").crmTierlevel) = "ALL" Then
                                    ac_panel.Visible = True
                                    ac_fleet.Items.Add(New ListItem("Companies Owning Any Jet", "Show JET Owners"))
                                    ac_fleet.Items.Add(New ListItem("Companies Owning Any TurboProp", "Show TurboProp Owners"))
                                ElseIf Trim(Session.Item("localSubscription").crmTierlevel) = "J" Then
                                    ac_panel.Visible = True
                                    ac_fleet.Items.Add(New ListItem("Companies Owning Any Jet", "Show JET Owners"))
                                ElseIf Trim(Session.Item("localSubscription").crmTierlevel) = "T" Then
                                    ac_panel.Visible = True
                                    ac_fleet.Items.Add(New ListItem("Companies Owning Any TurboProp", "Show TurboProp Owners"))
                                End If
                            End If



                            If Session.Item("localSubscription").crmCommercial_Flag = True Then
                                ac_panel.Visible = True
                            End If

                            If Session.Item("localSubscription").crmHelicopter_Flag = True Then
                                ac_fleet.Items.Add(New ListItem("Companies Owning Any Helicopter", "Show Heli Owners"))
                                ac_panel.Visible = True
                            End If


                        ElseIf Session.Item("localSubscription").crmYacht_Flag <> True Then
                            comp_product_yacht_flag.Visible = False
                            comp_product_yacht_flag.Checked = False
                        ElseIf Session.Item("localSubscription").crmYacht_Flag = True Then
                            yacht_panel.Visible = True
                        End If


                        If ac_panel.Visible = True Then
                            ac_fleet.Items.Add(New ListItem("Companies Related to Any Aircraft", "Show Any Related"))
                        End If


                        DisplayFunctions.SetPagingItem(company_per_page_dropdown)
                    End If



                    'Fill Folders Table
                    folders_submenu_dropdown.Items.Clear()
                    DisplayFunctions.AddEditFolderListOptionToFolderDropdown(folders_submenu_dropdown, 1)
                    FoldersTable = masterPage.aclsData_Temp.GetEvolutionFolderssBySubscription(0, Session.Item("localUser").crmUserLogin, Session.Item("localUser").crmSubSubID, Session.Item("localUser").crmSubSeqNo, "", 1, Nothing, "")

                    If clsGeneral.clsGeneral.isCrmDisplayMode() And Not Page.IsPostBack Then
                        Dim UserTableCheck As DataTable
                        UserTableCheck = masterPage.aclsData_Temp.Get_Client_User_By_Email_Address(Session.Item("localUser").crmLocalUserEmailAddress)
                        If Not IsNothing(UserTableCheck) Then
                            If UserTableCheck.Rows.Count > 0 Then
                                FoldersTable.Merge(masterPage.aclsdata_temp.Get_Client_Folders_Complete(1, UserTableCheck.Rows(0).Item("cliuser_id")))
                                'Sort Together:
                                Dim SortView As New DataView
                                SortView = FoldersTable.DefaultView
                                SortView.Sort = "cfolder_name"
                                FoldersTable = SortView.ToTable()
                            End If
                        End If
                    End If

                    If Not IsNothing(FoldersTable) Then
                        If FoldersTable.Rows.Count > 0 Then
                            For Each r As DataRow In FoldersTable.Rows

                                If r("source") = "JETNET" Then
                                    If Not IsDBNull(r("cfolder_data")) Then
                                        Dim FolderDataString As Array
                                        'this was added to parse out the real search query now that we're saving it
                                        FolderDataString = Split(r("cfolder_data"), "THEREALSEARCHQUERY")

                                        If Replace(r("cfolder_data").ToString, "comp_id=", "") <> "" Then
                                            folders_submenu_dropdown.Items.Add(New ListItem(r("cfolder_name").ToString, "javascript:ParseForm('" & r("cfolder_id").ToString & "',false,false,true,false,false,'" & Replace(FolderDataString(0), "'", "\'") & "');"))
                                        Else
                                            folders_submenu_dropdown.Items.Add(New ListItem(r("cfolder_name").ToString, "javascript:alert('This folder contains no information.');"))
                                        End If
                                    End If
                                ElseIf r("source") = "CLIENT" Then
                                    'CLIENT FOLDER:
                                    Dim ClientFolderItem As New ListItem


                                    ClientFolderItem.Attributes.Add("class", "folderClientRow")

                                    If Not IsDBNull(r("cfolder_data")) Then
                                        ClientFolderItem.Text = r("cfolder_name").ToString
                                        ClientFolderItem.Value = "javascript:ParseCLIENTForm('" & r("cfolder_id").ToString & "',false,false,true,false,false,'" & r("cfolder_data").ToString & "');"
                                        folders_submenu_dropdown.Items.Add(ClientFolderItem)

                                    Else 'This is an index
                                        'We need to look up the index information.
                                        Dim FolderIndex As New DataTable
                                        Dim ClientFolderString As String = ""
                                        Dim FolderString As String = ""
                                        FolderIndex = masterPage.aclsData_Temp.Get_Client_Folder_Index(r("cfolder_id"))
                                        For Each q As DataRow In FolderIndex.Rows
                                            If q("cfoldind_jetnet_comp_id") > 0 Then
                                                If FolderString <> "" Then
                                                    FolderString += ","
                                                End If
                                                FolderString += q("cfoldind_jetnet_comp_id").ToString
                                            ElseIf q("cfoldind_client_comp_id") > 0 Then
                                                If ClientFolderString <> "" Then
                                                    ClientFolderString += ","
                                                End If
                                                ClientFolderString += q("cfoldind_client_comp_id").ToString
                                            End If
                                        Next


                                        If FolderString <> "" Or ClientFolderString <> "" Then
                                            ClientFolderItem.Text = r("cfolder_name").ToString
                                            ClientFolderItem.Value = "javascript:ParseCLIENTForm('" & r("cfolder_id").ToString & "',false,false,true,false,false,'" & IIf(FolderString <> "", "!~!comp_id=" & FolderString, "!~!comp_id=0") & IIf(ClientFolderString <> "", "!~!clicomp_id=" & ClientFolderString, "!~!clicomp_id=0") & "');"
                                            folders_submenu_dropdown.Items.Add(ClientFolderItem)
                                        Else
                                            ClientFolderItem.Value = "javascript:alert('This folder contains no information.');"
                                            folders_submenu_dropdown.Items.Add(ClientFolderItem)
                                        End If
                                    End If

                                End If

                            Next
                        End If
                    End If
                End If

                ToggleHigherLowerBar(False)

                If Request.Form("project_search") = "Y" Then
                    company_go_to_text.Visible = True
                    company_go_to_dropdown_.Visible = True
                ElseIf Not Page.IsPostBack And Page.Request.Form("complete_search") <> "Y" Then
                    Initial(True)
                Else

                    If (Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER) Or (Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE) Then
                        If goto_subscriberSearch.Checked Then
                            HttpContext.Current.Response.Redirect("adminSubscribers.aspx", True)
                        End If
                    End If

                    Initial(False)
                End If

                'Load Search Information:
                If Not Page.IsPostBack Then
                    If Page.Request.Form("project_search") = "Y" Then
                    Else
                        FillOutSearchParameters()
                    End If
                Else
                    If company_go_to_dropdown.Visible = True Then
                        SetPageNumber(CInt(company_go_to_dropdown.Items(0).Text))
                    End If
                End If

                Dim VariableStateName As String = ""


                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ''''''''''''''''Some neat functions that might help'''''''''''''''''''''''''
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' 
                'Pass the tab index of what you want highlighted on the bar.

                If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT Then
                    masterPage.Set_Active_Tab(3)
                Else
                    masterPage.Set_Active_Tab(4)
                End If

                'This will set page title.
                Me.Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("Company Search Results")

            End If

        Catch ex As Exception

            Dim previousException As String = ex.Message.Trim

            Try

                If Not IsNothing(masterPage) Then
                    masterPage.LogError(Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " : " + ex.Message.Trim)
                Else
                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " : " + ex.Message.Trim
                End If

            Catch ex2 As Exception

                commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " : Previous Exception Thrown[" + previousException.Trim + "] : Exception Thrown[" + ex2.Message.Trim + "]")

            End Try
        End Try

    End Sub

    Private Function translateClientToJetnet(ByVal dataIn As String) As String
        dataIn = Replace(dataIn, "search_for_txt=", "company_name=")

        dataIn = Replace(dataIn, "subset=JC", "searchTypeDropdown=JC")
        dataIn = Replace(dataIn, "subset=C", "searchTypeDropdown=C")
        dataIn = Replace(dataIn, "subset=J", "searchTypeDropdown=J")
        dataIn = Replace(dataIn, "country", "cboCompanyCountryID")
        dataIn = Replace(dataIn, "company_phone_number=", "company_phone=")
        dataIn = Replace(dataIn, "city_textbox=", "comp_city=")
        dataIn = Replace(dataIn, "state=", "cboCompanyStateID=")

        Return dataIn
    End Function

    Public Function Get_Certifications(ByVal category As String) As DataTable
        Dim sql As String = ""
        Dim aTempTable As New DataTable
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            sql = "SELECT ccerttype_id, ccerttype_type FROM Company_Certification_Type WITH(NOLOCK) "

            If Trim(category) <> "" Then
                If Trim(category) = "Certificate" Then
                    sql &= " where ccerttype_category = 'Certificate' "
                ElseIf Trim(category) = "Mem_Accred" Then
                    sql &= " where ccerttype_category in ('Accreditation', 'Membership')  "
                End If
            End If
            sql &= " ORDER BY ccerttype_type "


            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sql.ToString)

            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
            SqlConn.Open()
            SqlCommand.Connection = SqlConn


            SqlCommand.CommandText = sql
            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 60


            Try
                aTempTable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = aTempTable.GetErrors()
            End Try

            Return aTempTable
        Catch ex As Exception
            Get_Certifications = Nothing
            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in Get_Certifications() As DataTable: SQL VERSION " & ex.Message
        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing

            SqlCommand.Dispose()
            SqlCommand = Nothing
        End Try

    End Function

    Public Sub FillListBoxes(ByRef relationshipType As ListBox, ByRef BusinessType As ListBox, ByRef MobileBusinessType As DropDownList,
                             ByRef ContactTitleGroup As ListBox, ByRef Certifications As ListBox,
                             ByRef Mem_Accred As ListBox, ByRef serviceUsed As ListBox,
                             ByRef customerTargetsServiceUsed As ListBox)
        Try

            If Not Page.IsPostBack Then

                Dim TempTable As DataTable

                If Session.Item("isMobile") = True Then
                    If Not IsNothing(MobileBusinessType) Then
                        TempTable = New DataTable
                        TempTable = masterPage.aclsData_Temp.Get_Jetnet_Business_Type()
                        clsGeneral.clsGeneral.Populate_Dropdown(TempTable, MobileBusinessType, "cbus_name", "cbus_type", False)
                        MobileBusinessType.Items.Add(New ListItem("ALL", ""))
                    End If
                Else
                    If Not IsNothing(BusinessType) Then
                        TempTable = New DataTable
                        TempTable = masterPage.aclsData_Temp.Get_Jetnet_Business_Type()
                        clsGeneral.clsGeneral.Populate_Listbox(TempTable, BusinessType, "cbus_name", "cbus_type", False)
                    End If
                End If

                If Not IsNothing(ContactTitleGroup) Then
                    TempTable = New DataTable
                    TempTable = masterPage.aclsData_Temp.Get_Jetnet_Contact_Title_Group()
                    clsGeneral.clsGeneral.Populate_Listbox(TempTable, ContactTitleGroup, "ctitleg_group_name", "ctitleg_group_name", False)
                End If

                If Not IsNothing(Certifications) Then
                    TempTable = New DataTable
                    TempTable = Get_Certifications("Certificate")
                    clsGeneral.clsGeneral.Populate_Listbox(TempTable, Certifications, "ccerttype_type", "ccerttype_id", False)
                End If

                If Not IsNothing(Mem_Accred) Then
                    TempTable = New DataTable
                    TempTable = Get_Certifications("Mem_Accred")
                    clsGeneral.clsGeneral.Populate_Listbox(TempTable, Mem_Accred, "ccerttype_type", "ccerttype_id", False)
                End If

                If (Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER) Or (Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE) Then

                    If Not IsNothing(serviceUsed) And Not IsNothing(customerTargetsServiceUsed) Then

                        TempTable = New DataTable
                        TempTable = getServiceUsed()

                        clsGeneral.clsGeneral.Populate_Listbox(TempTable, serviceUsed, "svud_desc", "svud_id", False)
                        clsGeneral.clsGeneral.Populate_Listbox(TempTable, customerTargetsServiceUsed, "svud_desc", "svud_desc", False)

                    End If

                End If

                If Not Yacht Then

                    If Not IsNothing(relationshipType) Then

                        relationshipType.Items.Clear()

                        TempTable = New DataTable
                        TempTable = masterPage.aclsData_Temp.Get_Client_Aircraft_Contact_Type()
                        clsGeneral.clsGeneral.Populate_Listbox(TempTable, relationshipType, "cliact_name", "cliact_type", True, Session.Item("localPreferences").AerodexFlag)

                        relationshipType.Items.RemoveAt(0)

                        relationshipType.Items.Insert(0, New ListItem("All", ""))
                        relationshipType.Items.Insert(1, New ListItem("All Owners", "'00','97','17','08','16'"))
                        relationshipType.Items.Insert(2, New ListItem("All Operating Companies", "'Y'"))

                        If Not Session.Item("localPreferences").AerodexFlag Then
                            relationshipType.Items.Insert(3, New ListItem("All Dealers, Brokers, Reps", "'93','98','99'"))
                        End If

                        relationshipType.SelectedValue = ""

                        If Session.Item("isMobile") = True Then

                            Company_Image.ImageUrl = "/images/spacer.gif"
                            Company_Image.Width = Unit.Pixel(13)
                            company_sort_by_dropdown.Width = Unit.Empty
                            CompanyPanelEx.CollapsedImage = "/images/chevron.png"
                            CompanyPanelEx.ExpandedImage = "/images/spacer.gif"

                            If Not IsNothing(MobileBusinessType) Then

                                MobileBusinessType.Items.RemoveAt(0)
                                For i = 0 To MobileBusinessType.Items.Count - 1
                                    MobileBusinessType.Items.Add(New ListItem(relationshipType.Items(i).Text, relationshipType.Items(i).Value))
                                Next

                            End If

                        End If

                    End If

                Else

                    If Not IsNothing(relationshipType) Then

                        relationshipType.Items.Clear()

                        TempTable = New DataTable
                        TempTable = masterPage.aclsData_Temp.Get_Yacht_Contact_Type(False)
                        clsGeneral.clsGeneral.Populate_Listbox(TempTable, relationshipType, "yct_name", "yct_code", True)

                        relationshipType.Items.RemoveAt(0)
                        relationshipType.Items.Insert(0, New ListItem("All", ""))
                        relationshipType.Items.Insert(1, New ListItem("All Central Agents", "'99','C1','C2','C3','C4','C5','C6'"))
                        relationshipType.Items.Insert(2, New ListItem("All Designers", "'Y1','Y2','Y3','Y0','Y9'"))
                        relationshipType.Items.Insert(3, New ListItem("All Owners", "'00','08'"))
                        relationshipType.SelectedValue = ""

                    End If

                End If

                TempTable = Nothing

            End If

        Catch ex As Exception

            Dim previousException As String = ex.Message.Trim

            Try

                If Not IsNothing(masterPage) Then
                    masterPage.LogError(Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " : " + ex.Message.Trim)
                Else
                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " : " + ex.Message.Trim
                End If

            Catch ex2 As Exception

                commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " : Previous Exception Thrown[" + previousException.Trim + "] : Exception Thrown[" + ex2.Message.Trim + "]")

            End Try
        End Try

    End Sub

    Public Sub SearchCompany(ByVal CompanyName As String, ByVal CompanyBusinessType As String, ByVal CompanyCertifications As String,
                             ByVal CompanyRelationship As String, ByVal NotInRelationship As Boolean,
                             ByVal CompanyCity As String, ByVal CompanyPostal As String,
                             ByVal CompanyCountry As String, ByVal CompanyContinent As String,
                             ByVal RegionString As String, ByVal Timezone As String,
                             ByVal ContactTitle As String, ByVal ContactFirstName As String,
                             ByVal ContactLastName As String, ByVal DisplayContactInfo As Boolean,
                             ByVal CompanyAgency As String, ByVal CompanyEmail As String,
                             ByVal CompanyPhoneNumber As String, ByVal CompanyAddress As String,
                             ByVal compID As String, ByVal OperatorFlag As String,
                             ByVal pageSort As String, ByVal BusinessFlag As Boolean,
                             ByVal HelicopterFlag As Boolean, ByVal CommercialFlag As Boolean,
                             ByVal Fleet As String, ByVal FleetCondition As String,
                             ByVal FleetValue As String, ByVal bindFromSession As Boolean,
                             ByVal AircraftSalesOnly As Boolean, ByVal BuildSearchString As String,
                             ByVal ContactID As String, ByVal ContinentString As String,
                             ByVal StateName As String, ByVal YachtFleet As String, ByVal YachtFlag As Boolean,
                             ByVal DynamicQueryString As String, ByVal companySearchType As String,
                             ByVal clientIDs As String, ByVal company_id As String,
                             ByVal company_id_client As String, ByVal CompanyCertifications2 As String)
        Try
            Dim RecordsPerPage As Integer = 0
            Dim Paging_Table As New DataTable
            Dim Results_Table As New DataTable
            Dim temptable2 As New DataTable

            If Session.Item("localUser").crmUserRecsPerPage <> 0 Then
                RecordsPerPage = Session.Item("localUser").crmUserRecsPerPage
            End If
            company_attention.Text = ""

            If bindFromSession = True And Not IsNothing(Session.Item("Company_Master")) Then
                Results_Table = Session.Item("Company_Master")
            Else

                '------- CLIENT CUSTOM SEARCH ------------
                Dim query_where As String = ""
                If custom_pref_text1.Visible = True Then
                    If Not IsNothing(HttpContext.Current.Session.Item("jetnetServerNotesDatabase")) Then
                        aclsData_temp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase")

                        If Trim(custom_pref_text1.Text) <> "" Then
                            query_where += " and comp_id in (" & aclsData_temp.Get_Client_AC_IDS_With_Custom_Top(1, Trim(custom_pref_text1.Text)) & ") "
                        End If

                        If Trim(custom_pref_text2.Text) <> "" Then
                            query_where += " and comp_id in (" & aclsData_temp.Get_Client_AC_IDS_With_Custom_Top(2, Trim(custom_pref_text2.Text)) & ") "
                        End If

                        If Trim(custom_pref_text3.Text) <> "" Then
                            query_where += " and comp_id in (" & aclsData_temp.Get_Client_AC_IDS_With_Custom_Top(3, Trim(custom_pref_text3.Text)) & ") "
                        End If

                        If Trim(custom_pref_text4.Text) <> "" Then
                            query_where += " and comp_id in (" & aclsData_temp.Get_Client_AC_IDS_With_Custom_Top(4, Trim(custom_pref_text4.Text)) & ") "
                        End If

                        If Trim(custom_pref_text5.Text) <> "" Then
                            query_where += " and comp_id in (" & aclsData_temp.Get_Client_AC_IDS_With_Custom_Top(5, Trim(custom_pref_text5.Text)) & ") "
                        End If
                    End If
                End If
                '------- CLIENT CUSTOM SEARCH ------------



                If companySearchType <> "C" Then

                    Results_Table = EvolutionCompanyListingPageQuery(CompanyName, CompanyBusinessType, CompanyCertifications,
                                                                          CompanyRelationship, NotInRelationship,
                                                                          CompanyCity, CompanyPostal,
                                                                          CompanyCountry, CompanyContinent,
                                                                          RegionString, Timezone,
                                                                          ContactTitle, ContactFirstName,
                                                                          ContactLastName, DisplayContactInfo,
                                                                          CompanyAgency, CompanyEmail,
                                                                          CompanyPhoneNumber, CompanyAddress,
                                                                          compID, OperatorFlag,
                                                                          pageSort, BusinessFlag,
                                                                          HelicopterFlag, CommercialFlag,
                                                                          Fleet, FleetCondition, FleetValue,
                                                                          AircraftSalesOnly, ContactID,
                                                                          ContinentString, StateName,
                                                                          YachtFlag, YachtFleet, ac_fleet.SelectedValue,
                                                                          DynamicQueryString, company_id, query_where, CompanyCertifications2)
                    Session.Item("Company_Master") = Results_Table
                End If

                If (companySearchType = "B" Or companySearchType = "C") And Yacht = False Then
                    If (CompanyBusinessType <> "" Or CompanyCertifications <> "" Or AircraftSalesOnly = True Or (Fleet <> "" And FleetCondition <> "" And FleetValue <> "")) And (companySearchType = "B" Or companySearchType = "C") Then
                        'We need to get the company IDs here:
                        Dim TempHold As New DataTable
                        If companySearchType = "C" Then 'We need to run this for the IDs
                            TempHold = EvolutionCompanyListingPageQuery(CompanyName, CompanyBusinessType, CompanyCertifications,
                                                                                 CompanyRelationship, NotInRelationship,
                                                                                 CompanyCity, CompanyPostal,
                                                                                 CompanyCountry, CompanyContinent,
                                                                                 RegionString, Timezone,
                                                                                 ContactTitle, ContactFirstName,
                                                                                 ContactLastName, DisplayContactInfo,
                                                                                 CompanyAgency, CompanyEmail,
                                                                                 CompanyPhoneNumber, CompanyAddress,
                                                                                 compID, OperatorFlag,
                                                                                 pageSort, BusinessFlag,
                                                                                 HelicopterFlag, CommercialFlag,
                                                                                 Fleet, FleetCondition, FleetValue,
                                                                                 AircraftSalesOnly, ContactID,
                                                                                 ContinentString, StateName,
                                                                                 YachtFlag, YachtFleet, ac_fleet.SelectedValue,
                                                                                 DynamicQueryString, company_id, query_where, CompanyCertifications2)

                        Else
                            TempHold = Results_Table
                        End If
                        Dim IDsToExclude As String = ""
                        For Each drRow As DataRow In TempHold.Rows
                            If IDsToExclude <> "" Then
                                IDsToExclude += ", "
                            End If
                            IDsToExclude += drRow("comp_id").ToString
                        Next
                        IDsToExclude = IDsToExclude
                        CompanyBusinessType = IDsToExclude 'resetting this to be IDS
                        'If IDsToExclude = "" Then
                        '  IgnoreClientSearch = True
                        'End If
                    End If

                    If Trim(CompanyCertifications2) <> "" Then
                        CompanyCertifications2 = Find_AC_Certifications_List(CompanyCertifications2)
                    End If

                    If Trim(CompanyCertifications) <> "" Then
                        CompanyCertifications = Find_AC_Certifications_List(CompanyCertifications)
                    End If

                    temptable2 = masterPage.aclsData_Temp.EvolutionCLIENTCompanyListingPageQuery(CompanyName, CompanyBusinessType, CompanyCertifications,
                                                                         CompanyRelationship, NotInRelationship,
                                                                         CompanyCity, CompanyPostal,
                                                                         CompanyCountry, CompanyContinent,
                                                                         RegionString, Timezone,
                                                                         ContactTitle, ContactFirstName,
                                                                         ContactLastName, DisplayContactInfo,
                                                                         CompanyAgency, CompanyEmail,
                                                                         CompanyPhoneNumber, CompanyAddress,
                                                                         IIf(compID <> "0", compID, ""), OperatorFlag,
                                                                         pageSort, BusinessFlag,
                                                                         HelicopterFlag, CommercialFlag,
                                                                         Fleet, FleetCondition, FleetValue,
                                                                         AircraftSalesOnly, ContactID,
                                                                         ContinentString, StateName,
                                                                         YachtFlag, chkShowInactiveCompany.Checked,
                                                                         YachtFleet, ac_fleet.SelectedValue,
                                                                         DynamicQueryString, IIf(clientIDs <> "0", clientIDs, ""), company_id_client,
                                                                         query_where, CompanyCertifications2)

                    Dim temp_ac_id_list As String = ""
                    HttpContext.Current.Session.Item("CLIENT_AC_LIST") = ""
                    If Not IsNothing(temptable2) Then
                        For Each q As DataRow In temptable2.Rows
                            If Trim(temp_ac_id_list) <> "" Then
                                temp_ac_id_list &= ", "
                            End If
                            temp_ac_id_list &= q("comp_jetnet_comp_id").ToString
                        Next

                        HttpContext.Current.Session.Item("CLIENT_AC_LIST") = temp_ac_id_list
                    End If

                    Results_Table.Merge(temptable2)

                    Session.Item("Company_Master") = Results_Table

                End If
            End If

            If companySearchType = "B" Or companySearchType = "C" Then
                Dim FilterView As New DataView
                Dim displayTable As New DataTable
                FilterView = Results_Table.DefaultView

                If pageSort <> "" Then
                    FilterView.Sort = pageSort
                Else
                    FilterView.Sort = " comp_name"
                End If

                displayTable = FilterView.ToTable()
                Results_Table = displayTable
                Session.Item("Company_Master") = Results_Table
            End If

            HttpContext.Current.Session.Item("SearchString") = BuildSearchString

            Call commonLogFunctions.Log_User_Event_Data("UserSearch", "Company Search: " & clsGeneral.clsGeneral.StripChars(clsGeneral.clsGeneral.stripHTML(Replace(BuildSearchString, "<br />", " ")), False), Nothing, 0, 0, 0, 0, 0, 0, 0)

            masterPage.SetStatusText(HttpContext.Current.Session.Item("SearchString"))

            If Not IsNothing(Results_Table) Then
                Session.Item("localUser").crmLatestRecordCount = Results_Table.Rows.Count
                SetPageNumber(1)
                If Results_Table.Rows.Count > 0 Then


                    If Page.IsPostBack Then
                        If Not Page.ClientScript.IsClientScriptBlockRegistered("masonryPost") Then
                            System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "masonryPost", "loadMasonry();", True)
                        End If
                    Else
                        System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "masonryPostFolderCall", "loadMasonry();", True)
                    End If


                    company_next.CommandArgument = "1"
                    company_previous.CommandArgument = "0"
                    If Session.Item("isMobile") = True Then
                        Paging_Table = Results_Table
                    Else
                        Paging_Table = Results_Table.Clone
                        Dim afiltered_Client As DataRow() = Results_Table.Select("", pageSort) 'comp_count <= " & RecordsPerPage
                        'For Each atmpDataRow_Client In afiltered_Client

                        If RecordsPerPage - 1 > Results_Table.Rows.Count - 1 Then
                            For i = 0 To Results_Table.Rows.Count - 1
                                Paging_Table.ImportRow(afiltered_Client(i))
                            Next
                        Else
                            For i = 0 To RecordsPerPage - 1
                                Paging_Table.ImportRow(afiltered_Client(i))
                            Next
                        End If

                    End If

                    If ResultsSearchData.Visible = True Then

                        Results.CurrentPageIndex = 0
                        Results.PageSize = RecordsPerPage
                        Results.Visible = False

                        Results.DataSource = Results_Table
                        Results.DataBind()


                        ResultsSearchData.DataSource = Paging_Table
                        ResultsSearchData.DataBind()
                    End If


                    If ResultsSearchDataList.Visible = True Then

                        ResultsSearchDataList.DataSource = Paging_Table
                        ResultsSearchDataList.DataBind()
                    End If

                    company_criteria_results.Text = Results_Table.Rows.Count & " Results"
                    Company_Bottom_Paging.Visible = True

                    If Session.Item("isMobile") = True Then
                        SetPagingButtons(False, False)
                    Else
                        company_record_count.Text = "Showing 1 - " & IIf(Results_Table.Rows.Count <= RecordsPerPage, Results_Table.Rows.Count, RecordsPerPage)
                        bottom_record_count.Text = "Showing 1 - " & IIf(Results_Table.Rows.Count <= RecordsPerPage, Results_Table.Rows.Count, RecordsPerPage)


                        'This will fill up the dropdown bar with however many pages.
                        If Results_Table.Rows.Count > RecordsPerPage Then
                            Fill_Page_To_To_Dropdown(Math.Ceiling(Results_Table.Rows.Count / RecordsPerPage))
                            SetPagingButtons(False, True)
                        Else
                            SetPagingButtons(False, False)
                        End If
                    End If

                    CompanyPanelEx.Collapsed = True
                    Results_Table = Nothing
                Else
                    'added 7/01/2015
                    'This resets the page index to 0 if there are no companies returned.
                    Results.CurrentPageIndex = 0
                    Results.DataSource = New DataTable
                    Results.DataBind()
                    Company_Bottom_Paging.Visible = False
                    ResultsSearchDataList.DataSource = New DataTable
                    ResultsSearchDataList.DataBind()

                    ResultsSearchData.DataSource = New DataTable
                    ResultsSearchData.DataBind()

                    company_attention.Text = "<br /><p class='padding'><b>No Companies Found. Please refine your search and try again.</b></p><br /><br />"
                    bottom_record_count.Text = ""
                    company_criteria_results.Text = "0 Results"

                    company_record_count.Text = "Showing 0 Results"

                    SetPagingButtons(False, False)

                End If
            End If


            Results_Table = New DataTable
        Catch ex As Exception
            Company_Bottom_Paging.Visible = False
            'Some More Error Catching.
            masterPage.LogError("Company Search(): Query: " & HttpContext.Current.Session.Item("MasterCompany").ToString & " " & ex.Message.ToString)
            company_attention.Text = "<br /><p class='padding'>We're sorry, an error has occurred during your search.</b></p><br /><br />"
            If (InStr(UCase(Session.Item("localUser").crmLocalUserName), "JETNET.COM") > 0) Or (InStr(UCase(Session.Item("localUser").crmLocalUserName), "MVINTECH.COM") > 0) Then
                company_attention.Text += ex.Message.ToString
            End If
        End Try

    End Sub

    Public Function Find_AC_Certifications_List(ByVal cert_ids As String) As String
        Dim CertifactionsTable As New DataTable
        Dim count As Integer = 0
        Find_AC_Certifications_List = ""


        Try

            CertifactionsTable = masterPage.aclsData_Temp.Return_Certifications_AC(cert_ids)
            If Not IsNothing(CertifactionsTable) Then
                If (CertifactionsTable.Rows.Count > 0) Then

                    For Each q As DataRow In CertifactionsTable.Rows
                        If Trim(Find_AC_Certifications_List) <> "" Then
                            Find_AC_Certifications_List &= ", "
                        End If
                        Find_AC_Certifications_List &= q("ccert_comp_id").ToString
                        count = count + 1
                    Next
                End If
                'Response.Write(certifications_label.Text)
            Else
                If masterPage.aclsData_Temp.class_error <> "" Then
                    masterPage.LogError("CompanyTabs.ascx.vb -Fill_Certifications() - " & masterPage.aclsData_Temp.class_error)
                End If
            End If

        Catch ex As Exception

        End Try

    End Function

    ''' <summary>
    ''' Function to display contact information for link on gallery/listing.
    ''' Error reporting included
    ''' </summary>
    ''' <param name="companyID"></param>
    ''' <param name="contactID"></param>
    ''' <param name="sirname"></param>
    ''' <param name="firstName"></param>
    ''' <param name="lastName"></param>
    ''' <param name="title"></param>
    ''' <param name="gallery"></param>
    ''' <param name="middle"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DisplayContactInfoListing(ByVal companyID As Object, ByVal contactID As Object, ByVal sirname As Object, ByVal firstName As Object, ByVal lastName As Object, ByVal title As Object, ByVal gallery As Boolean, ByVal middle As Object)
        Dim returnString As String = ""
        Try
            If Not IsDBNull(contactID) Then
                If IsNumeric(contactID) Then
                    If gallery Then
                        returnString = ""
                    End If
                    returnString += crmWebClient.DisplayFunctions.WriteDetailsLink(0, CLng(companyID), CLng(contactID), 0, True, IIf(sirname.ToString <> "", sirname.ToString & " ", "") & firstName.ToString & " " & IIf(middle.ToString <> "", middle.ToString & ". ", "") & lastName.ToString, "text_underline", "")
                    If gallery Then
                        If title.ToString <> "" Then
                            returnString += " <em class='tiny_text'>" & title.ToString & "</em>"
                        End If
                    End If
                    If gallery Then
                        returnString += ""
                    End If
                End If
            End If
        Catch ex As Exception
            masterPage.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString & " : " & ex.Message.ToString)
        End Try

        Return returnString
    End Function

    ''' <summary>
    ''' Next/previous button clicks function.
    ''' </summary>
    ''' <param name="next_"></param>
    ''' <param name="prev_"></param>
    ''' <param name="next_all"></param>
    ''' <param name="prev_all"></param>
    ''' <param name="goToPage"></param>
    ''' <param name="pageNumber"></param>
    ''' <remarks></remarks>
    Public Sub MovePage(ByVal next_ As Boolean, ByVal prev_ As Boolean, ByVal next_all As Boolean, ByVal prev_all As Boolean, ByVal goToPage As Boolean, ByVal pageNumber As Integer)
        Try
            Dim holdTable As New DataTable
            Dim StartCount As Integer = 0
            Dim EndCount As Integer = 0
            Dim RecordsPerPage As Integer = 0
            If Session.Item("localUser").crmUserRecsPerPage <> 0 Then
                RecordsPerPage = Session.Item("localUser").crmUserRecsPerPage
            End If


            If Not IsNothing(Session.Item("Company_Master")) Then
                holdTable = Session.Item("Company_Master")
                Initial(False)
                MoveRepeater(StartCount, EndCount, Results, ResultsSearchData, ResultsSearchDataList, holdTable, next_, prev_, next_all, prev_all, goToPage, pageNumber)
                SetPagingButtons(IIf(StartCount = 1, False, True), IIf(holdTable.Rows.Count = EndCount, False, True))

                company_go_to_dropdown.Items.Clear()
                company_go_to_dropdown.Items.Add(New ListItem(pageNumber + 1, ""))
                company_record_count.Text = "Showing " & StartCount & " - " & IIf(holdTable.Rows.Count <= RecordsPerPage, holdTable.Rows.Count, IIf((RecordsPerPage + StartCount) <= holdTable.Rows.Count, RecordsPerPage + StartCount, holdTable.Rows.Count))
                bottom_record_count.Text = "Showing " & StartCount & " - " & IIf(holdTable.Rows.Count <= RecordsPerPage, holdTable.Rows.Count, IIf((RecordsPerPage + StartCount) <= holdTable.Rows.Count, RecordsPerPage + StartCount, holdTable.Rows.Count))
            End If

        Catch ex As Exception
            'Some More Error Catching.
            masterPage.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString & " : " & ex.Message.ToString)
        End Try

    End Sub

    Public Sub MoveRepeater(ByRef StartCount As Integer, ByRef EndCount As Integer, ByVal Dynamically_Configured_DataGrid As DataGrid, ByVal Dynamically_Configured_DataRepeater As Repeater, ByVal Dynamically_Configured_DataList As Object, ByVal HoldTable As DataTable, ByVal next_ As Boolean, ByVal prev_ As Boolean, ByVal next_all As Boolean, ByVal prev_all As Boolean, ByVal goToPage As Boolean, ByVal pageNumber As Integer)
        Dim RecordsPerPage As Integer = 0
        Dim CurrentPage As Integer = 0
        Dim CurrentRecord As Integer = 0
        ' Dim EndCount As Integer = 0
        'Dim StartCount As Integer = 0
        Dim Paging_Table As New DataTable
        Dim CountString As String = ""
        Dim TotalPageNumber As Integer = 0


        'Initial(False)
        If HttpContext.Current.Session.Item("localUser").crmUserRecsPerPage <> 0 Then
            RecordsPerPage = HttpContext.Current.Session.Item("localUser").crmUserRecsPerPage
        End If

        If Not IsNothing(HoldTable) Then
            TotalPageNumber = Math.Ceiling(HoldTable.Rows.Count / RecordsPerPage)
            'Dynamically_Configured_DataGrid.PageSize = RecordsPerPage

            'Dynamically_Configured_DataGrid.DataSource = HoldTable

            'If next_ Then
            '  Dynamically_Configured_DataGrid.CurrentPageIndex += 1
            'ElseIf prev_ Then
            '  Dynamically_Configured_DataGrid.CurrentPageIndex -= 1
            'ElseIf prev_all Then
            '  Dynamically_Configured_DataGrid.CurrentPageIndex = 0
            'ElseIf next_all Then
            '  Dynamically_Configured_DataGrid.CurrentPageIndex = TotalPageNumber - 1
            'Else
            '  Dynamically_Configured_DataGrid.CurrentPageIndex = pageNumber - 1
            'End If

            ''only bind if results is visible.
            'If Dynamically_Configured_DataGrid.Visible = True Then
            '  Try
            '    Dynamically_Configured_DataGrid.DataBind()
            '  Catch
            '    Dynamically_Configured_DataGrid.CurrentPageIndex = 0
            '    Dynamically_Configured_DataGrid.DataBind()
            '  End Try
            'End If


            CurrentPage = pageNumber 'company_next.CommandArgument ' - 1
            CurrentRecord = (RecordsPerPage * CurrentPage) - HoldTable.Rows.Count + HoldTable.Rows.Count
            If CurrentRecord = 0 Then
                StartCount = 1
            Else
                StartCount = CurrentRecord + 1
            End If

            If CurrentRecord + RecordsPerPage >= HoldTable.Rows.Count Then
                CountString = StartCount & "-" & HoldTable.Rows.Count
                EndCount = HoldTable.Rows.Count
            Else
                CountString = StartCount & "-" & CurrentRecord + pageNumber
                EndCount = CurrentRecord + RecordsPerPage
            End If

            Fill_Page_To_To_Dropdown(Math.Ceiling(HoldTable.Rows.Count / RecordsPerPage))


            Paging_Table = HoldTable.Clone
            Dim afiltered_Client As DataRow() = HoldTable.Select("", PageSort) '"comp_count >= " & StartCount & " and comp_count <= " & EndCount
            'For Each atmpDataRow_Client In afiltered_Client
            For i = (StartCount - 1) To EndCount - 1 'RecordsPerPage - 1
                Paging_Table.ImportRow(afiltered_Client(i))
            Next




            'only bind if results is visible.
            If Dynamically_Configured_DataRepeater.Visible = True Then
                Dynamically_Configured_DataRepeater.DataSource = Paging_Table
                Dynamically_Configured_DataRepeater.DataBind()
            End If



            If Not IsNothing(Dynamically_Configured_DataList) Then
                If Dynamically_Configured_DataList.Visible = True Then
                    Dynamically_Configured_DataList.DataSource = Paging_Table
                    Dynamically_Configured_DataList.DataBind()
                    If Not Page.ClientScript.IsClientScriptBlockRegistered("masonryPage") Then
                        System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "masonryPageMove", "loadMasonry();", True)
                    End If
                End If
            End If


        End If

        Dynamically_Configured_DataGrid.Dispose()
    End Sub

    ''' <summary>
    ''' Alter Listing is an event called from Criteria Bar which allows the Dropdown Menus on the Criteria bar to talk to this page.
    ''' Error catching is included
    ''' </summary>
    ''' <param name="TypeOfListing"></param>
    ''' <remarks></remarks>
    Public Sub AlterListing(ByVal TypeOfListing As Integer, ByVal RecordAmount As Integer)
        Try
            Dim Dynamically_Configured_Repeater As New Repeater
            Dim Dynamically_Configured_DataList As New DataList

            Dynamically_Configured_Repeater = ResultsSearchData
            Dynamically_Configured_DataList = ResultsSearchDataList


            Select Case TypeOfListing
                Case 0 'Listing Display
                    Dynamically_Configured_Repeater.Visible = True
                    Dynamically_Configured_DataList.Visible = False
                Case 1 'Image Display
                    Dynamically_Configured_Repeater.Visible = False
                    Dynamically_Configured_DataList.Visible = True
            End Select

            Dynamically_Configured_DataList.Dispose()
            Dynamically_Configured_Repeater.Dispose()
        Catch ex As Exception
            masterPage.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString & " : " & ex.Message.ToString)
        End Try
    End Sub

    ''' <summary>
    ''' This is going to deal with displaying the Company Logo. This only runs on the company listing gallery display.
    ''' Also includes error reporting
    ''' </summary>
    ''' <param name="companyLogoFlag"></param>
    ''' <param name="CompanyID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DisplayCompanyLogo(ByVal companyLogoFlag As Object, ByVal CompanyID As Long)
        Dim returnString As String = ""
        Try
            Dim MainLocationDataTable As New DataTable
            Dim logoID As Long = 0
            'Let's set the logo to make sure it's dealt with as a string.
            companyLogoFlag = companyLogoFlag.ToString
            'if the flag is going to be Y then the company ID is the logo ID.
            If companyLogoFlag = "Y" Then
                logoID = CompanyID
            Else
                'Look up the other locations logo. If there's a main location that has a logo, that ID is the logo ID
                MainLocationDataTable = masterPage.aclsData_Temp.GetCompanyMainLocationDescriptionLogo(CompanyID)
                If Not IsNothing(MainLocationDataTable) Then
                    If MainLocationDataTable.Rows.Count > 0 Then
                        'This means there is a main location, let's check for a logo here.
                        If Not IsDBNull(MainLocationDataTable.Rows(0).Item("comp_logo_flag")) Then
                            If MainLocationDataTable.Rows(0).Item("comp_logo_flag") = "Y" Then
                                logoID = MainLocationDataTable.Rows(0).Item("comp_id")
                            End If
                        End If
                    End If
                End If
            End If

            'if neither one of above is set, the logo ID is zero and there isn't a picture.
            If logoID = 0 Then
                returnString = "<span class=""company_picture_pad displayNoneMobile""><img src='images/comp_no_image.png' class='float_left' width='100%' /></span>"
            Else

                If HttpContext.Current.Session.Item("jetnetWebSiteType") = crmWebClient.eWebSiteTypes.LOCAL Then
                    returnString = "<span class=""company_picture_pad displayNoneMobile""><img src=""https://www.testjetnetevolution.com/pictures/company/" + logoID.ToString + ".jpg""  onerror=""if (this.src != 'images/comp_no_image.png') {this.src='images/comp_no_image.png'};"" class=""float_left"" width=""100%"" /></span>"
                Else
                    returnString = "<span class=""company_picture_pad displayNoneMobile""><img src=""" + HttpContext.Current.Session.Item("jetnetFullHostName").ToString + HttpContext.Current.Session.Item("CompanyPicturesFolderVirtualPath") + "/" + logoID.ToString + ".jpg""  onerror=""if (this.src != 'images/comp_no_image.png') {this.src='images/comp_no_image.png'};"" class=""float_left"" width=""100%"" /></span>"
                End If

            End If
        Catch ex As Exception
            masterPage.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString & " : " & ex.Message.ToString)
        End Try
        Return returnString

    End Function

    Public Function DisplayClientCompany(ByVal JetnetID As Object, ByVal dataList As Boolean, ByVal contactID As Object) As String
        Dim returnString As String = ""
        Dim contactIDQuery As Long = 0

        If clsGeneral.clsGeneral.isCrmDisplayMode() Then
            If Not IsDBNull(contactID) Then
                If IsNumeric(contactID) Then
                    contactIDQuery = contactID
                End If
            End If
            'If company_contact_info.Checked = False Then
            If IsNumeric(JetnetID) Then
                Dim clsDataTemp As New clsData_Manager_SQL
                Dim seperator As String = ""
                clsDataTemp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase")
                If Not String.IsNullOrEmpty(clsDataTemp.client_DB) Then
                    Dim TemporaryTable As New DataTable
                    TemporaryTable = clsDataTemp.GetCompanyInfo_JETNET_ID(JetnetID, "")
                    If Not IsNothing(TemporaryTable) Then
                        If TemporaryTable.Rows.Count > 0 Then
                            If dataList = True Then
                                returnString = "" & crmWebClient.DisplayFunctions.WriteDetailsLink(0, TemporaryTable.Rows(0).Item("comp_id"), 0, 0, True, TemporaryTable.Rows(0).Item("comp_name").ToString, "text_underline", "&SOURCE=CLIENT") & "/CLIENT"
                                'If TemporaryTable.Rows(0).Item("cliaircraft_forsale_flag") = "Y" Then
                                If Not IsDBNull(TemporaryTable.Rows(0).Item("comp_address1")) Then
                                    returnString += "<br />" & TemporaryTable.Rows(0).Item("comp_address1")
                                End If
                                If Not IsDBNull(TemporaryTable.Rows(0).Item("comp_address2")) Then
                                    returnString += " " & TemporaryTable.Rows(0).Item("comp_address2")
                                End If
                                returnString += "<br />"
                                If Not IsDBNull(TemporaryTable.Rows(0).Item("comp_city")) Then
                                    returnString += TemporaryTable.Rows(0).Item("comp_city").ToString & ", "
                                End If

                                If Not IsDBNull(TemporaryTable.Rows(0).Item("comp_state")) Then
                                    returnString += TemporaryTable.Rows(0).Item("comp_state").ToString & " "
                                End If

                                If Not IsDBNull(TemporaryTable.Rows(0).Item("comp_country")) Then
                                    returnString += Replace(TemporaryTable.Rows(0).Item("comp_country").ToString, "United States", "US") & " "
                                End If

                                If Not IsDBNull(TemporaryTable.Rows(0).Item("comp_zip_code")) Then
                                    returnString += TemporaryTable.Rows(0).Item("comp_zip_code").ToString & " "
                                End If
                                returnString += "<br />"
                                If Not IsDBNull(TemporaryTable.Rows(0).Item("comp_email_address")) Then
                                    If Not String.IsNullOrEmpty(TemporaryTable.Rows(0).Item("comp_email_address")) Then
                                        returnString += "<a href='mailto:" & TemporaryTable.Rows(0).Item("comp_email_address").ToString & "'>" & TemporaryTable.Rows(0).Item("comp_email_address").ToString & "</a><br />"
                                    End If
                                End If
                                If Not IsDBNull(TemporaryTable.Rows(0).Item("comp_web_address")) Then
                                    If Not String.IsNullOrEmpty(TemporaryTable.Rows(0).Item("comp_web_address")) Then
                                        returnString += "<a href='http://www." & Replace(Replace(TemporaryTable.Rows(0).Item("comp_web_address").ToString, "http://", ""), "www.", "") & "' target='new' title='http://www." & Replace(Replace(TemporaryTable.Rows(0).Item("comp_web_address").ToString, "http://", ""), "www.", "") & "'>" & IIf(HttpContext.Current.Session.Item("isMobile") = True, "<i class=""fa fa-globe"" aria-hidden=""true""></i>", TemporaryTable.Rows(0).Item("comp_web_address").ToString) & "</a><br />"
                                    End If
                                End If
                                If Not IsDBNull(TemporaryTable.Rows(0).Item("comp_office_phone")) Then
                                    returnString += "<span class='label'>Office: </span><a href=""tel:" & TemporaryTable.Rows(0).Item("comp_office_phone").ToString & """>" & TemporaryTable.Rows(0).Item("comp_office_phone") & "</a><br />"
                                End If
                                If Not IsDBNull(TemporaryTable.Rows(0).Item("comp_fax_phone")) Then
                                    returnString += "<span class='label'>Fax: </span>" & TemporaryTable.Rows(0).Item("comp_fax_phone").ToString & "<br />"
                                End If

                                If company_contact_info.Checked = True Then
                                    If contactIDQuery > 0 Then
                                        Dim contactTable As New DataTable
                                        contactTable = clsDataTemp.GetContactInfo_JETNET_ID(contactIDQuery, "Y")

                                        If Not IsNothing(contactTable) Then
                                            If contactTable.Rows.Count > 0 Then
                                                returnString += crmWebClient.DisplayFunctions.WriteDetailsLink(0, TemporaryTable.Rows(0).Item("comp_id"), contactTable.Rows(0).Item("contact_id"), 0, True, IIf(Not IsDBNull(contactTable.Rows(0).Item("contact_sirname")), contactTable.Rows(0).Item("contact_sirname") & " ", "") & contactTable.Rows(0).Item("contact_first_name").ToString & " " & IIf(Not IsDBNull(contactTable.Rows(0).Item("contact_middle_initial")), contactTable.Rows(0).Item("contact_middle_initial").ToString & ". ", "") & contactTable.Rows(0).Item("contact_last_name").ToString, "", "&source=CLIENT")
                                                If Not IsDBNull(contactTable.Rows(0).Item("contact_title")) Then
                                                    returnString += " <em class='tiny_text'>" & contactTable.Rows(0).Item("contact_title").ToString & "</em>"
                                                End If
                                            End If
                                        End If
                                    End If
                                End If

                                If returnString <> "" Then
                                    returnString = "<div class=""CLIENTCRMRow"">" & returnString & "</div>"
                                End If
                            Else
                                returnString += "<tr class=""CLIENTCRMRow"">"
                                returnString += "<td class=""mobile_display_off_cell"">"
                                returnString += crmWebClient.DisplayFunctions.WriteDetailsLink(0, TemporaryTable.Rows(0).Item("comp_id"), 0, 0, True, TemporaryTable.Rows(0).Item("comp_name").ToString, "", "&SOURCE=CLIENT") & "/CLIENT"
                                returnString += "</td>"
                                returnString += "<td class=""mobile_display_off_cell"">"
                                returnString += TemporaryTable.Rows(0).Item("comp_address1")
                                returnString += "</td>"
                                returnString += "<td class=""mobile_display_on_cell"">"
                                returnString += "<a " & crmWebClient.DisplayFunctions.WriteDetailsLink(0, TemporaryTable.Rows(0).Item("comp_id"), 0, 0, False, "", "", "") & " title=""" & IIf(Not IsDBNull(TemporaryTable.Rows(0).Item("comp_alternate_name_type")), TemporaryTable.Rows(0).Item("comp_alternate_name_type").ToString & ": ", "") & IIf(Not IsDBNull(TemporaryTable.Rows(0).Item("comp_alternate_name")), TemporaryTable.Rows(0).Item("comp_alternate_name").ToString, "") & """>" & TemporaryTable.Rows(0).Item("comp_name").ToString & "</a>"
                                returnString += "<br />"
                                returnString += "</td>"
                                returnString += "<td class=""mobile_display_off_cell"">"
                                If Not IsDBNull(TemporaryTable.Rows(0).Item("comp_city")) Then
                                    returnString += TemporaryTable.Rows(0).Item("comp_city")
                                End If
                                returnString += "</td>"
                                returnString += "<td class=""mobile_display_off_cell"">"
                                If Not IsDBNull(TemporaryTable.Rows(0).Item("comp_state")) Then
                                    returnString += TemporaryTable.Rows(0).Item("comp_state")
                                End If
                                returnString += "</td>"
                                returnString += "<td class=""mobile_display_off_cell"">"
                                If Not IsDBNull(TemporaryTable.Rows(0).Item("comp_country")) Then
                                    returnString += TemporaryTable.Rows(0).Item("comp_country")
                                End If
                                returnString += "</td>"
                                returnString += "<td class=""mobile_display_off_cell"">"
                                returnString += IIf(TemporaryTable.Rows(0).Item("comp_office_phone").ToString <> "", "<span class=""li_no_bullet"">" & TemporaryTable.Rows(0).Item("comp_office_phone").ToString & "</span>", "")
                                returnString += "</td>"

                                If company_contact_info.Checked = True Then
                                    returnString += "<td class=""mobile_display_off_cell"">"
                                    If contactIDQuery > 0 Then
                                        Dim contactTable As New DataTable
                                        contactTable = clsDataTemp.GetContactInfo_JETNET_ID(contactIDQuery, "Y")

                                        If Not IsNothing(contactTable) Then
                                            If contactTable.Rows.Count > 0 Then
                                                returnString += crmWebClient.DisplayFunctions.WriteDetailsLink(0, TemporaryTable.Rows(0).Item("comp_id"), contactTable.Rows(0).Item("contact_id"), 0, True, IIf(Not IsDBNull(contactTable.Rows(0).Item("contact_sirname")), contactTable.Rows(0).Item("contact_sirname") & " ", "") & contactTable.Rows(0).Item("contact_first_name").ToString & " " & IIf(Not IsDBNull(contactTable.Rows(0).Item("contact_middle_initial")), contactTable.Rows(0).Item("contact_middle_initial").ToString & ". ", "") & contactTable.Rows(0).Item("contact_last_name").ToString, "", "&source=CLIENT")
                                                If Not IsDBNull(contactTable.Rows(0).Item("contact_title")) Then
                                                    returnString += " <em class='tiny_text'>" & contactTable.Rows(0).Item("contact_title").ToString & "</em>"
                                                End If
                                            End If
                                        End If
                                    End If
                                    returnString += "</td>"
                                End If
                                returnString += IIf(HttpContext.Current.Session.Item("localSubscription").crmServerSideNotes_Flag = True, "<td>" & crmWebClient.DisplayFunctions.BuildNote(TemporaryTable.Rows(0).Item("comp_id"), masterPage.aclsData_Temp, "COMP") & "</td>", "")
                                returnString += "</tr>"

                            End If

                        End If
                        'End If
                    End If
                End If
            End If
        End If
        Return returnString
    End Function

    ''' <summary>
    ''' This pages the listing, either next, next all, previous or previous all. All includes error reporting
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Sub MoveNext(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) 'Private Sub next__Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles company_next.Click, company_previous.Click, company_next_all.Click, company_previous_all.Click
        Try
            If sender.commandname.ToString = "next" Then

                MovePage(True, False, False, False, False, company_next.CommandArgument)
                company_next.CommandArgument = company_next.CommandArgument + 1
                company_previous.CommandArgument = company_next.CommandArgument + 1
            ElseIf sender.commandname.ToString = "previous" Then
                MovePage(False, True, False, False, False, company_previous.CommandArgument)
                company_next.CommandArgument = company_next.CommandArgument - 1
                company_previous.CommandArgument = company_next.CommandArgument - 1
            ElseIf sender.commandname.ToString = "next_all" Then
                MovePage(False, False, True, False, False, company_next_all.CommandArgument - 1)
            ElseIf sender.commandname.ToString = "previous_all" Then
                MovePage(False, False, False, True, False, 0)
            End If
        Catch ex As Exception
            masterPage.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString & " : " & ex.Message.ToString)
        End Try
    End Sub


    ''' <summary>
    ''' This runs on the initial load of the page. It'll toggle off some of the paging elements and things we don't need displayed if we're first coming into the page.
    ''' Also includes error reporting
    ''' </summary>
    ''' <param name="initial_page_load"></param>
    ''' <remarks></remarks>
    Public Sub Initial(ByVal initial_page_load As Boolean)
        Try
            If initial_page_load = True Then
                If Company_Criteria.Visible = True Then

                    company_criteria_results.Visible = False
                    company_sort_by_text.Visible = False
                    company_sort_by_dropdown.Visible = False
                    company_actions_dropdown.Visible = False
                    company_paging.Visible = False

                    company_per_page_dropdown_.Visible = False
                    company_per_page_text.Visible = False
                    company_go_to_dropdown_.Visible = False
                    company_go_to_text.Visible = False

                    company_view_dropdown.Visible = False

                    CompanyPanelEx.Collapsed = False
                    CompanyPanelEx.ClientState = False
                End If
            Else
                If Company_Criteria.Visible = True Then
                    Company_Bottom_Paging.Visible = True
                    company_criteria_results.Visible = True
                    company_sort_by_text.Visible = True
                    company_sort_by_dropdown.Visible = True
                    company_actions_dropdown.Visible = True
                    company_paging.Visible = True
                    company_view_dropdown.Visible = True

                    company_per_page_dropdown_.Visible = True
                    company_per_page_text.Visible = True
                    company_go_to_dropdown_.Visible = True
                    company_go_to_text.Visible = True

                    CompanyPanelEx.Collapsed = True
                    CompanyPanelEx.ClientState = True
                End If
            End If


        Catch ex As Exception
            masterPage.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString & " : " & ex.Message.ToString)
        End Try
    End Sub

    ''' <summary>
    ''' This function deals with the company condition. Swapping the watermark on the textbox from nnnn to nnnn:nnnn depending
    ''' on operator selection.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub company_condition_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles company_condition.SelectedIndexChanged
        If company_condition.SelectedValue = "Between" Then
            TBWE2.WatermarkText = "nnnn:nnnn"
        Else
            TBWE2.WatermarkText = "nnnn"
        End If

        If company_condition.SelectedValue = "" Then
            company_fleet.SelectedValue = ""
        End If
    End Sub

    ''' <summary>
    ''' company search called from company listing
    ''' Also includes error reporting
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub company_search_Click(ByVal sender As Object, ByVal e As System.EventArgs, Optional ByVal LoadFromSession As Boolean = False) Handles company_search.Click
        Try
            Dim OperatorFlag As String = ""
            Dim CompanyName As String = ""
            Dim relationships As String = ""
            Dim businesstype As String = ""
            Dim certifications As String = ""
            Dim certifications2 As String = ""
            Dim CompanyFleet As String = ""
            Dim CompanyFleetCondition As String = ""
            Dim CompanyFleetValue As String = ""
            Dim BuildSearchString As String = ""
            Dim ContactID As String = ""
            Dim CompanyID As String = ""
            Dim contactTitle As String = ""
            Dim YachtFleet As String = ""
            Dim CompanyTypeSearch As String = ""
            Dim CompanyRegionString As String = ""
            Dim CompanyContinentString As String = ""
            Dim CompanyTimeZoneString As String = ""
            ' Dim CompanyStatesString As String = ""
            Dim CompanyCountriesString As String = ""
            Dim CompanyStateName As String = ""
            Dim DynamicQueryString As String = ""
            Dim clientIDs As String = ""
            Dim company_id As String = ""
            Dim company_id_client As String = ""
            Dim sServicesQuery As String = ""


            Dim NewSearchClass As New SearchSelectionCriteria
            If Not IsNothing(Session.Item("searchCriteria")) Then
                NewSearchClass = Session.Item("searchCriteria")
            End If

            If (Session.Item("jetnetAppVersion") <> Constants.ApplicationVariable.CUSTOMER_CENTER) And (Session.Item("jetnetAppVersion") <> Constants.ApplicationVariable.HOMEBASE) Then
                If clsGeneral.clsGeneral.isCrmDisplayMode Then
                    CompanyTypeSearch = searchTypeDropdown.SelectedValue
                End If
            End If


            'Contact ID, passed if we have a contact folder
            If contact_id.Text <> "" Then
                BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(comp_folder_name, "Folder")
                ContactID = clsGeneral.clsGeneral.StripChars(contact_id.Text, True)
            End If

            'Yacht Fleet.
            If yacht_fleet.SelectedValue <> "" Then
                YachtFleet = yacht_fleet.SelectedValue
                BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(YachtFleet, "Search Yacht Directory")
                NewSearchClass.SearchCriteriaCompanyYachtFleet = yacht_fleet.SelectedValue
            End If
            'Company Name
            'Clear Company Name
            NewSearchClass.SearchCriteriaCompanyName = ""
            If company_name.Text <> "" Then
                Dim TempCompHold As String = ""
                'company_name.Text = Replace(company_name.Text, ",", ";")
                TempCompHold = Replace(company_name.Text, ",", "_")
                TempCompHold = clsGeneral.clsGeneral.CleanUserData(TempCompHold, Constants.cEmptyString, Constants.cCommaDelim, True)
                TempCompHold = Replace(TempCompHold, ",", ";")
                TempCompHold = Replace(TempCompHold, ";", "*;")

                Dim TempNameHold As String = ""
                TempNameHold = clsGeneral.clsGeneral.FilterCompanyNameForCompanyAircraftSearch(TempCompHold)

                CompanyName = "( "
                If InStr(TempNameHold, "*") = 0 Then
                    CompanyName += "( comp_name_search " & clsGeneral.clsGeneral.PrepQueryString("Begins With", TempNameHold, "String", False, "comp_name_search", True)
                    CompanyName += " OR comp_altname_search " & clsGeneral.clsGeneral.PrepQueryString("Begins With", TempNameHold, "String", False, "comp_altname_search", True) & ")"
                    'CompanyName += ")"
                Else
                    CompanyName += " ( " & clsGeneral.clsGeneral.PrepQueryString("Begins With", TempNameHold, "String", False, "comp_name_search", True)
                    CompanyName += " OR " & clsGeneral.clsGeneral.PrepQueryString("Begins With", TempNameHold, "String", False, "comp_altname_search", True) & " ) "
                    'CompanyName += ")"

                End If
                CompanyName += ")"
                BuildSearchString += "Company Name" & " " & "Begins With" & " " & Replace(TempNameHold, ":", " and ") & "<br />"
                ' BuildSearchString += "Company Alternate Name" & " " & "Begins With" & " " & Replace(TempNameHold, ":", " and ") & "<br />"

                NewSearchClass.SearchCriteriaCompanyName = company_name.Text
                'Response.Write(CompanyName)
            End If

            'Company Fleet
            'Clear Fleet Answer
            NewSearchClass.SearchCriteriaCompanyFleetAnswer = ""
            If company_fleet.SelectedValue <> "" Then
                CompanyFleet = company_fleet.SelectedValue
                'Saving the fleet answer
                NewSearchClass.SearchCriteriaCompanyFleetAnswer = company_fleet.SelectedValue
            End If

            If Trim(company_id_text.Text) <> "" Then
                Dim TempCompHold2 As String = ""
                'company_name.Text = Replace(company_name.Text, ",", ";")
                TempCompHold2 = Replace(company_id_text.Text, ",", "_")
                TempCompHold2 = clsGeneral.clsGeneral.CleanUserData(TempCompHold2, Constants.cEmptyString, Constants.cCommaDelim, True)
                TempCompHold2 = Replace(TempCompHold2, ",", ";")
                TempCompHold2 = Replace(TempCompHold2, ";", "*;")

                TempCompHold2 = clsGeneral.clsGeneral.FilterCompanyNameForCompanyAircraftSearch(TempCompHold2)

                company_id = clsGeneral.clsGeneral.PrepQueryString("Equals", TempCompHold2, "Numeric", False, "comp_id", True)

                company_id_client = clsGeneral.clsGeneral.PrepQueryString("Equals", TempCompHold2, "Numeric", False, "clicomp_id", True)
            End If

            'Company Fleet Condition
            NewSearchClass.SearchCriteriaCompanyFleetOperator = ""
            If company_condition.SelectedValue <> "" Then
                CompanyFleetCondition = company_condition.SelectedValue
                'Saving the fleet operator
                NewSearchClass.SearchCriteriaCompanyFleetOperator = company_condition.SelectedValue
            End If

            'Company Value
            NewSearchClass.SearchCriteriaCompanyFleetValue = 0
            If company_fleet_value.Text <> "" Then
                CompanyFleetValue = company_fleet_value.Text
                'Saving the fleet value
                If IsNumeric(company_fleet_value.Text) Then
                    NewSearchClass.SearchCriteriaCompanyFleetValue = company_fleet_value.Text
                End If
            End If

            'Aircraft Sales Professional: 
            NewSearchClass.SearchCriteriaCompanyOnlyAircraftSalesProfessionals = False
            If company_aircraft_sales.Checked Then
                BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(company_aircraft_sales, "Aircraft Sales Professional Only")
                'Saving the aircraft sales professional
                NewSearchClass.SearchCriteriaCompanyOnlyAircraftSalesProfessionals = True
            End If

            'Display Contact Info:
            'Reset Search Criteria
            NewSearchClass.SearchCriteriaCompanyDisplayContactInfo = False
            If company_contact_info.Checked Then
                BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(company_contact_info, "Display Contact Info")
                'Saving the display contact info
                NewSearchClass.SearchCriteriaCompanyDisplayContactInfo = True
            End If

            'Services Used ( make this handle multi types )
            sServicesQuery = clsGeneral.clsGeneral.ExtractSelectedStringFromListboxDropdown(service_used, False, 0, True)
            If Not String.IsNullOrEmpty(sServicesQuery.Trim) Then
                BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(service_used, "Services Used")
                'Saving the Services Used
                NewSearchClass.SearchCriteriaServices = sServicesQuery
            End If

            'Only needed if we have a fleet value
            If CompanyFleet <> "" And CompanyFleetCondition <> "" And CompanyFleetValue <> "" Then
                BuildSearchString += "Fleet: " & CompanyFleet & " " & CompanyFleetCondition & " " & IIf(CompanyFleetCondition = "Between", Replace(CompanyFleetValue.ToString, ":", " and "), Replace(CompanyFleetValue.ToString, ":", "")) & "<br />"
            End If

            'Relationships
            NewSearchClass.SearchCriteriaCompanyRelationshipsToAC = ""

            If Session.Item("isMobile") = True Then
                If mobile_company_relationship.SelectedValue <> "" Then
                    If UCase(mobile_company_relationship.SelectedValue) <> "ALL" Then
                        If mobile_company_relationship.SelectedValue = "'Y'" Then
                            OperatorFlag = " (cref_operator_flag IN ('Y', 'O')) "
                            BuildSearchString += DisplayFunctions.BuildSearchTextDisplay("All Operators Included", "Operators")
                        Else
                            relationships += mobile_company_relationship.SelectedValue
                        End If
                        NewSearchClass.SearchCriteriaCompanyRelationshipsToAC += mobile_company_relationship.SelectedValue
                    End If
                End If
            Else
                For i = 0 To company_relationship.Items.Count - 1
                    If company_relationship.Items(i).Selected Then
                        If company_relationship.Items(i).Value <> "" Then 'Here we check to see if there is a value, meaning there's no selection
                            If UCase(company_relationship.Items(i).Value) <> "ALL" Then 'Checking to make sure ALL isn't checked, if it is, we don't need to search
                                If company_relationship.Items(i).Value = "'Y'" Then
                                    OperatorFlag = " (cref_operator_flag IN ('Y', 'O')) "
                                    If comp_not_in_selected.Checked = False Then
                                        BuildSearchString += DisplayFunctions.BuildSearchTextDisplay("All Operators Included", "Operators")
                                    Else
                                        BuildSearchString += DisplayFunctions.BuildSearchTextDisplay("Not Included", "Operators")
                                    End If
                                    ' NewSearchClass.SearchCriteriaCompanyRelationshipsToAC += company_relationship.Items(i).Value & "##"
                                Else
                                    relationships += company_relationship.Items(i).Value

                                    relationships += ","

                                    'Saving the relationships
                                    ' NewSearchClass.SearchCriteriaCompanyRelationshipsToAC += company_relationship.Items(i).Value & "##"
                                End If
                                NewSearchClass.SearchCriteriaCompanyRelationshipsToAC += company_relationship.Items(i).Value & "##"
                            End If
                        End If
                    End If
                Next
            End If

            'Reationships/Operator Flag, builds part of the search
            NewSearchClass.SearchCriteriaCompanyNotInSelectedRelationship = False
            If relationships <> "" Or OperatorFlag <> "" Then

                relationships = UCase(relationships.TrimEnd(","))

                If comp_not_in_selected.Checked = False Then
                    If relationships <> "" Then
                        If Yacht = False Then
                            relationships = " (cref_contact_type in (" & relationships & ") ) "
                        Else
                            relationships = " (yr_contact_type in (" & relationships & ") ) "
                        End If
                        BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(company_relationship, "Relationships")
                    End If
                Else
                    'Saving the not in selected relationships
                    'And going ahead and then running them.
                    NewSearchClass.SearchCriteriaCompanyNotInSelectedRelationship = True
                    If relationships <> "" Then
                        Dim TempHold As String = relationships

                        '(NOT EXISTS (SELECT NULL FROM Yacht_reference WITH (NOLOCK) WHERE yr_yt_id = yt_id
                        'AND yr_journ_id = yt_journ_id AND (yr_contact_type in ('Y2'))))


                        If Yacht = True Then
                            relationships = " (NOT EXISTS (SELECT NULL FROM " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Yacht_Reference WITH (NOLOCK) WHERE (yr_yt_id = yt_id) AND (yr_journ_id = yt_journ_id) AND (("

                            relationships += " yr_contact_type in (" & TempHold & ") "
                        Else

                            relationships = " (NOT EXISTS (SELECT NULL FROM " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Aircraft_Reference WITH (NOLOCK) WHERE (cref_comp_id = comp_id) AND (cref_journ_id = comp_journ_id) AND (("

                            relationships += " cref_contact_type in (" & TempHold & ") "
                        End If

                        If OperatorFlag <> "" Then
                            If relationships <> "" Then
                                relationships += " or "
                            End If
                        End If
                    Else 'No operator flag on the yacht side as of yet, so this doesn't need to be taken into account and had the table swapped depending yet
                        relationships = " (NOT EXISTS (SELECT NULL FROM " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Aircraft_Reference WITH (NOLOCK) WHERE (cref_comp_id = comp_id) AND (cref_journ_id = comp_journ_id) AND (("
                    End If
                    If OperatorFlag <> "" Then
                        relationships += OperatorFlag
                    End If

                    relationships += " )))) "

                    BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(company_relationship, "Not in these Relationships")
                End If


            End If


            'Business Checkbox
            Session.Item("chkBusinessFilter") = comp_product_business_flag.Checked
            Session.Item("hasBusinessFilter") = Session.Item("chkBusinessFilter")

            'Helicopter Checkbox
            Session.Item("chkHelicopterFilter") = comp_product_helicopter_flag.Checked
            Session.Item("hasHelicopterFilter") = Session.Item("chkHelicopterFilter")

            'Commercial Checkbox
            Session.Item("chkCommercialFilter") = comp_product_commercial_flag.Checked
            Session.Item("hasCommercialFilter") = Session.Item("chkCommercialFilter")

            'Yacht Checkbox
            NewSearchClass.SearchCriteriaYachtFlag = comp_product_yacht_flag.Checked

            If Session.Item("localSubscription").crmBusiness_Flag <> Session.Item("chkBusinessFilter") Then
                Session.Item("hasModelFilter") = True
            ElseIf Session.Item("localSubscription").crmHelicopter_Flag <> Session.Item("chkHelicopterFilter") Then
                Session.Item("hasModelFilter") = True
            ElseIf Session.Item("localSubscription").crmCommercial_Flag <> Session.Item("chkCommercialFilter") Then
                Session.Item("hasModelFilter") = True
            End If

            If Session.Item("localSubscription").crmYacht_Flag <> NewSearchClass.SearchCriteriaYachtFlag Then
                NewSearchClass.SearchCriteriaYachtHasFilterFlag = True
            End If

            'Business Type SearchCriteriaCompanyCertifications
            NewSearchClass.SearchCriteriaCompanyBusinessType = ""

            businesstype = clsGeneral.clsGeneral.ExtractSelectedStringFromListboxDropdown(IIf(Session.Item("isMobile"), mobile_company_business, company_business), True, 0, True)
            If businesstype <> "" Then
                BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(IIf(Session.Item("isMobile"), mobile_company_business, company_business), "Business Type")
                'Saving the Business Type
                NewSearchClass.SearchCriteriaCompanyBusinessType = businesstype
            End If

            'Certifications SearchCriteriaCompanyCertifications
            NewSearchClass.SearchCriteriaCompanyCertifications = ""
            certifications = clsGeneral.clsGeneral.ExtractSelectedStringFromListboxDropdown(comp_certifications, True, 0, True)
            If certifications <> "" Then
                BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(comp_certifications, "Certifications")
                'Saving the Business Type
                NewSearchClass.SearchCriteriaCompanyCertifications = certifications
            End If

            ' MSW - added a 2nd certification 1/29/20
            'Certifications SearchCriteriaCompanyCertifications
            NewSearchClass.SearchCriteriaCompanyCertifications = ""
            certifications2 = clsGeneral.clsGeneral.ExtractSelectedStringFromListboxDropdown(comp_member_accred, True, 0, True)

            If certifications2 <> "" Then
                BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(comp_member_accred, "Certifications")
                'Saving the Business Type
                '   NewSearchClass.SearchCriteriaCompanyCertifications2 = certifications2
            End If

            'Contact Title
            NewSearchClass.SearchCriteriaCompanyContactTitle = ""
            contactTitle = clsGeneral.clsGeneral.ExtractSelectedStringFromListboxDropdown(company_contact_title, True, 0, True)
            If contactTitle <> "" Then
                BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(company_contact_title, "Contact Title")
                'Saving the Contact Title
                NewSearchClass.SearchCriteriaCompanyContactTitle = contactTitle
            End If

            'Company Phone
            NewSearchClass.SearchCriteriaCompanyPhone = ""
            If company_phone.Text <> "" Then
                BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(company_phone, "Phone")
                'Saving the Company Phone
                NewSearchClass.SearchCriteriaCompanyPhone = Trim(clsGeneral.clsGeneral.StripChars(company_phone.Text, True))
            End If
            'Contact First
            NewSearchClass.SearchCriteriaCompanyContactFirstName = ""
            If company_contact_first.Text <> "" Then
                BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(company_contact_first, "First Name")
                'Saving the Contact First
                NewSearchClass.SearchCriteriaCompanyContactFirstName = company_contact_first.Text
            End If
            'Contact Last
            NewSearchClass.SearchCriteriaCompanyContactLastName = ""
            If company_contact_last.Text <> "" Then
                BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(company_contact_last, "Last Name")
                'Saving the Contact Last
                NewSearchClass.SearchCriteriaCompanyContactLastName = company_contact_last.Text
            End If
            'Company ID / company Folder
            If comp_id.Text <> "" Then
                CompanyID = clsGeneral.clsGeneral.StripChars(comp_id.Text, True)
                BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(comp_folder_name, "Folder")
            End If
            If clicomp_id.Text <> "" Then
                clientIDs = clsGeneral.clsGeneral.StripChars(clicomp_id.Text, True)
                BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(comp_folder_name, "Folder")
            End If

            'Company City in session
            NewSearchClass.SearchCriteriaCompanyCity = Trim(clsGeneral.clsGeneral.StripChars(comp_city.Text, True))
            If comp_city.Text <> "" Then
                BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(comp_city, "City")
            End If

            NewSearchClass.SearchCriteriaCompanyDisplayInactiveCompanies = False

            If chkShowInactiveCompany.Checked Then
                NewSearchClass.SearchCriteriaCompanyDisplayInactiveCompanies = True
                BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(chkShowInactiveCompany, "Display Inactive Companies")
            End If

            NewSearchClass.SearchCriteriaCompanyDisplayHiddenCompanies = False

            If chkShowHiddenCompany.Checked Then
                NewSearchClass.SearchCriteriaCompanyDisplayHiddenCompanies = True
                BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(chkShowHiddenCompany, "Display Hidden Companies")
            End If

            NewSearchClass.SearchCriteriaCompanyDisplayInactiveContacts = False

            If chkShowInactiveContact.Checked Then
                NewSearchClass.SearchCriteriaCompanyDisplayInactiveContacts = True
                BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(chkShowInactiveContact, "Display Inactive Contacts")
            End If

            NewSearchClass.SearchCriteriaCompanyDisplayHiddenContacts = False

            If chkShowHiddenContact.Checked Then
                NewSearchClass.SearchCriteriaCompanyDisplayHiddenContacts = True
                BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(chkShowHiddenContact, "Display Hidden Contacts")
            End If

            'Company Email in session
            NewSearchClass.SearchCriteriaCompanyEmail = Trim(clsGeneral.clsGeneral.StripChars(company_email_address.Text, True))
            If company_email_address.Text <> "" Then
                BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(company_email_address, "Email")
            End If

            'Company Postal Code
            NewSearchClass.SearchCriteriaCompanyPostalCode = Trim(clsGeneral.clsGeneral.StripChars(comp_zip_code.Text, True))
            If comp_zip_code.Text <> "" Then
                BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(comp_zip_code, "Zip Code")
            End If

            'This function grabs the all the region information from the locaton control
            If Session.Item("isMobile") = False Then
                DisplayFunctions.GetRegionInfoFromCommonControl("Company", BuildSearchString, CompanyCountriesString, CompanyTimeZoneString, CompanyContinentString, CompanyRegionString, CompanyStateName)
            Else
                'For mobile we only care about countries and states right now.
                If Not String.IsNullOrEmpty(mobileCountryOptions.SelectedValue) Then
                    CompanyCountriesString = "'" & Replace(mobileCountryOptions.SelectedValue, "'", "''") & "'"
                End If
                If Not String.IsNullOrEmpty(mobileStateOptions.SelectedValue) Then
                    CompanyStateName = "'" & Replace(mobileStateOptions.SelectedValue, "'", "''") & "'"
                End If
            End If

            'Company Country in session
            'NewSearchClass.SearchCriteriaCompanyCo= CompanyCountriesString

            'Company Address in session
            NewSearchClass.SearchCriteriaCompanyAddress = Trim(clsGeneral.clsGeneral.StripChars(company_address.Text, True))
            If company_address.Text <> "" Then
                BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(company_address, "Company Address")
            End If

            'Company Timezone in session
            NewSearchClass.SearchCriteriaCompanyTimezone = CompanyTimeZoneString

            'Company Continent in session
            If Session.Item("isMobile") = True Then
                NewSearchClass.SearchCriteriaCompanyContinent = mobileCountryOptions.SelectedValue
                NewSearchClass.SearchCriteriaCompanyStateName = mobileStateOptions.SelectedValue
            Else
                NewSearchClass.SearchCriteriaCompanyContinent = CompanyContinentString
            End If


            'Company Region in session
            NewSearchClass.SearchCriteriaCompanyRegion = CompanyRegionString


            'Display Contact Info in session 
            NewSearchClass.SearchCriteriaCompanyDisplayContactInfo = company_contact_info.Checked


            'aircraft sales professionals in session
            NewSearchClass.SearchCriteriaCompanyOnlyAircraftSalesProfessionals = company_aircraft_sales.Checked

            'Not In Selected Relationship
            NewSearchClass.SearchCriteriaCompanyNotInSelectedRelationship = comp_not_in_selected.Checked

            Dim CompanyWhereString As String = ""
            If CompanyRegionString <> "" Then
                If CompanyStateName <> "" Then
                    CompanyWhereString = AdvancedQueryResults.BuildRegionWhereString("state_name", "comp_country", masterPage.aclsData_Temp, CompanyStateName, CompanyCountriesString, CompanyRegionString)


                    If Not String.IsNullOrEmpty(DynamicQueryString.Trim) Then
                        DynamicQueryString += " and (" & CompanyWhereString & ")"
                    Else
                        DynamicQueryString += "(" & CompanyWhereString & ")"
                    End If
                    CompanyStateName = ""
                    CompanyCountriesString = ""
                    CompanyRegionString = ""
                End If
            End If

            If Not String.IsNullOrEmpty(NewSearchClass.SearchCriteriaServices.Trim) Then

                If Not String.IsNullOrEmpty(DynamicQueryString.Trim) Then
                    DynamicQueryString += Constants.cAndClause + "comp_id IN (select distinct csu_comp_id"
                    DynamicQueryString += " FROM Company_Services_Used with (NOLOCK)"
                    DynamicQueryString += " WHERE csu_svud_id IN (" + NewSearchClass.SearchCriteriaServices + "))"
                Else
                    DynamicQueryString += "comp_id IN (select distinct csu_comp_id"
                    DynamicQueryString += " FROM Company_Services_Used with (NOLOCK)"
                    DynamicQueryString += " WHERE csu_svud_id IN (" + NewSearchClass.SearchCriteriaServices + "))"
                End If


            End If

            If CompanyRegionString <> "" Then
                NewSearchClass.SearchCriteriaCompanyContinentOrRegion = "region"
            Else
                NewSearchClass.SearchCriteriaCompanyContinentOrRegion = "continent"
            End If

            Session.Item("searchCriteria") = NewSearchClass

            SearchCompany(CompanyName, businesstype, certifications,
              relationships, comp_not_in_selected.Checked,
              Trim(clsGeneral.clsGeneral.StripChars(comp_city.Text, True)),
              Trim(clsGeneral.clsGeneral.StripChars(comp_zip_code.Text, True)), CompanyCountriesString,
              CompanyContinentString, CompanyRegionString,
              CompanyTimeZoneString, contactTitle,
              Trim(clsGeneral.clsGeneral.StripChars(company_contact_first.Text, True)),
              Trim(clsGeneral.clsGeneral.StripChars(Replace(company_contact_last.Text, "'", "&apos;"), True)),
              company_contact_info.Checked, company_agency_type.SelectedValue,
              Trim(clsGeneral.clsGeneral.StripChars(company_email_address.Text, True)),
              Trim(clsGeneral.clsGeneral.StripChars(company_phone.Text, True)),
              Trim(clsGeneral.clsGeneral.StripChars(company_address.Text, True)),
              CompanyID, OperatorFlag, PageSort,
              comp_product_business_flag.Checked, comp_product_helicopter_flag.Checked, comp_product_commercial_flag.Checked,
              CompanyFleet, CompanyFleetCondition, CompanyFleetValue,
              LoadFromSession, company_aircraft_sales.Checked, BuildSearchString,
              ContactID, CompanyContinentString, CompanyStateName,
              YachtFleet, IIf(Session.Item("localSubscription").crmYacht_Flag, comp_product_yacht_flag.Checked, False), DynamicQueryString, CompanyTypeSearch, clientIDs, company_id, company_id_client, certifications2)
        Catch ex As Exception
            masterPage.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString & " : " & ex.Message.ToString)
        End Try
    End Sub

    ''' <summary>
    ''' Toggles the bar whether it's the high bar or the low bar. This sets up the javascript for the bulleted lists as well.
    ''' Also includes error reporting
    ''' </summary>
    ''' <param name="lower_bar"></param>
    ''' <remarks></remarks>
    Public Sub ToggleHigherLowerBar(ByVal lower_bar As Boolean)
        Try
            'setting the javascript of the menus
            If Company_Criteria.Visible = True Then
                If lower_bar = True Then
                    CompanyPanelEx.Collapsed = True
                    CompanyPanelEx.ClientState = True
                    company_search_expand_text.Visible = False
                    company_help_text.Visible = False
                    company_sort_by_text.Visible = False
                    company_sort_by_dropdown.Visible = False
                    FolderInformation.Visible = False
                Else
                    company_per_page_dropdown_.Visible = False
                    company_per_page_text.Visible = False
                    company_go_to_dropdown_.Visible = False
                    company_go_to_text.Visible = False
                End If

                'sort
                company_view_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & company_view_submenu_dropdown.ClientID & "', true);")
                company_view_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & company_view_submenu_dropdown.ClientID & "', false);")

                company_view_submenu_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & company_view_submenu_dropdown.ClientID & "', true);")
                company_view_submenu_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & company_view_submenu_dropdown.ClientID & "', false);")

                'folders:
                folders_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & folders_submenu_dropdown.ClientID & "', true);")
                folders_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & folders_submenu_dropdown.ClientID & "', false);")

                folders_submenu_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & folders_submenu_dropdown.ClientID & "', true);")
                folders_submenu_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & folders_submenu_dropdown.ClientID & "', false);")

                'sort dropdown
                company_sort_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & company_sort_submenu_dropdown.ClientID & "', true);")
                company_sort_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & company_sort_submenu_dropdown.ClientID & "', false);")

                company_sort_submenu_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & company_sort_submenu_dropdown.ClientID & "', true);")
                company_sort_submenu_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & company_sort_submenu_dropdown.ClientID & "', false);")

                'page dropdown
                company_per_page_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & company_per_page_submenu_dropdown.ClientID & "', true);")
                company_per_page_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & company_per_page_submenu_dropdown.ClientID & "', false);")

                company_per_page_submenu_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & company_per_page_submenu_dropdown.ClientID & "', true);")
                company_per_page_submenu_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & company_per_page_submenu_dropdown.ClientID & "', false);")

                'go to dropdown
                company_go_to_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & company_go_to_submenu_dropdown.ClientID & "', true);")
                company_go_to_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & company_go_to_submenu_dropdown.ClientID & "', false);")

                company_go_to_submenu_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & company_go_to_submenu_dropdown.ClientID & "', true);")
                company_go_to_submenu_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & company_go_to_submenu_dropdown.ClientID & "', false);")

                'actions dropdown
                company_actions_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & company_actions_submenu_dropdown.ClientID & "', true);")
                company_actions_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & company_actions_submenu_dropdown.ClientID & "', false);")

                company_actions_submenu_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & company_actions_submenu_dropdown.ClientID & "', true);")
                company_actions_submenu_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & company_actions_submenu_dropdown.ClientID & "', false);")


            End If

        Catch ex As Exception
            masterPage.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString & " : " & ex.Message.ToString)
        End Try
    End Sub

    ''' <summary>
    ''' Sets page sort, works only for ac page.
    ''' </summary>
    ''' <param name="selectedLI"></param>
    ''' <remarks></remarks>
    Public Sub SetPageSort(Optional ByVal selectedLI As String = "")
        Select Case selectedLI
            Case "Address"
                PageSort = " comp_address1"
            Case "City"
                PageSort = " comp_city"
            Case "State"
                PageSort = " comp_state"
            Case "Country"
                PageSort = " comp_country"
            Case "Last Change"
                PageSort = " comp_action_date"
            Case Else
                PageSort = " comp_name "
        End Select
    End Sub
    ''' <summary>
    ''' Sets dropdown page #
    ''' </summary>
    ''' <param name="selectedLI"></param>
    ''' <remarks></remarks>
    Public Sub SetPageNumber(Optional ByVal selectedLI As Integer = 0)
        PageNumber = selectedLI
    End Sub
    ''' <summary>
    ''' Fills out company search parameters with error reporting included
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FillOutSearchParameters()
        Try
            'Filling Back in the Search Criteria.
            'Company Listing.

            'company name
            If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaCompanyName) Then
                If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaCompanyName) Then
                    company_name.Text = Session.Item("searchCriteria").SearchCriteriaCompanyName.ToString
                End If
            End If


            If Session.Item("searchCriteria").SearchCriteriaCompanyDisplayInactiveCompanies Then
                chkShowInactiveCompany.Checked = True
            End If

            If Session.Item("searchCriteria").SearchCriteriaCompanyDisplayHiddenCompanies Then
                chkShowHiddenCompany.Checked = True
            End If

            If Session.Item("searchCriteria").SearchCriteriaCompanyDisplayInactiveContacts Then
                chkShowHiddenContact.Checked = True
            End If

            If Session.Item("searchCriteria").SearchCriteriaCompanyDisplayHiddenContacts Then
                chkShowInactiveContact.Checked = True
            End If


            If Session.Item("localSubscription").crmBusiness_Flag Then 'Check to see if user has business flags before doing anything.
                If Session.Item("hasModelFilter") Then
                    Session.Item("chkBusinessFilter") = Session.Item("hasBusinessFilter")
                ElseIf Not Session.Item("chkBusinessFilter") Then
                    Session.Item("chkBusinessFilter") = True
                End If

                'Business Checkbox
                If Not IsNothing(Session.Item("chkBusinessFilter")) Then
                    If Not String.IsNullOrEmpty(Session.Item("chkBusinessFilter")) Then
                        If UCase(Session.Item("chkBusinessFilter")) = "FALSE" Then
                            comp_product_business_flag.Checked = False
                        Else
                            comp_product_business_flag.Checked = True
                        End If
                    End If
                End If
            End If

            If Session.Item("localSubscription").crmHelicopter_Flag Then 'Check to see if user has helicopter flags before doing anything
                If Session.Item("hasModelFilter") Then
                    Session.Item("chkHelicopterFilter") = Session.Item("hasHelicopterFilter")
                ElseIf Not Session.Item("chkHelicopterFilter") Then
                    Session.Item("chkHelicopterFilter") = True
                End If

                'Helicopter Checkbox
                If Not IsNothing(Session.Item("chkHelicopterFilter")) Then
                    If Not String.IsNullOrEmpty(Session.Item("chkHelicopterFilter")) Then
                        If UCase(Session.Item("chkHelicopterFilter")) = "FALSE" Then
                            comp_product_helicopter_flag.Checked = False
                        Else
                            comp_product_helicopter_flag.Checked = True
                        End If
                    End If
                End If
            End If

            If Session.Item("localSubscription").crmCommercial_Flag Then 'Check to see if user has commercial flag before doing anything
                If Session.Item("hasModelFilter") Then
                    Session.Item("chkCommercialFilter") = Session.Item("hasCommercialFilter")
                ElseIf Not Session.Item("chkCommercialFilter") Then
                    Session.Item("chkCommercialFilter") = True
                End If

                'Commercial Checkbox
                If Not IsNothing(Session.Item("chkCommercialFilter")) Then
                    If Not String.IsNullOrEmpty(Session.Item("chkCommercialFilter")) Then
                        If UCase(Session.Item("chkCommercialFilter")) = "FALSE" Then
                            comp_product_commercial_flag.Checked = False
                        Else
                            comp_product_commercial_flag.Checked = True
                        End If
                    End If
                End If
            End If

            If Session.Item("localSubscription").crmYacht_Flag Then 'Check to see if user has yacht flag before doing anything
                'Yacht Checkbox:
                If Session.Item("searchCriteria").SearchCriteriaYachtHasFilterFlag Then
                    If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaYachtFlag) Then
                        If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaYachtFlag) Then
                            If UCase(Session.Item("searchCriteria").SearchCriteriaYachtFlag) = "FALSE" Then
                                comp_product_yacht_flag.Checked = False
                            End If
                        End If
                    End If

                End If

            End If


            'agency type
            If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaCompanyAgencyType) Then
                If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaCompanyAgencyType) Then
                    company_agency_type.SelectedValue = Session.Item("searchCriteria").SearchCriteriaCompanyAgencyType.ToString
                End If
            End If

            'Company Address:
            If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaCompanyAddress) Then
                If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaCompanyAddress) Then
                    company_address.Text = Session.Item("searchCriteria").SearchCriteriaCompanyAddress.ToString
                End If
            End If

            'Company City:
            If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaCompanyCity) Then
                If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaCompanyCity) Then
                    comp_city.Text = Session.Item("searchCriteria").SearchCriteriaCompanyCity.ToString
                End If
            End If

            'Company Postal Code:
            If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaCompanyPostalCode) Then
                If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaCompanyPostalCode) Then
                    comp_zip_code.Text = Session.Item("searchCriteria").SearchCriteriaCompanyPostalCode.ToString
                End If
            End If

            'Company Contact First Name:
            If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaCompanyContactFirstName) Then
                If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaCompanyContactFirstName) Then
                    company_contact_first.Text = Session.Item("searchCriteria").SearchCriteriaCompanyContactFirstName.ToString
                End If
            End If

            'Company Contact Last Name:
            If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaCompanyContactLastName) Then
                If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaCompanyContactLastName) Then
                    company_contact_last.Text = Session.Item("searchCriteria").SearchCriteriaCompanyContactLastName.ToString
                End If
            End If

            'Company Email:
            If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaCompanyEmail) Then
                If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaCompanyEmail) Then
                    company_email_address.Text = Session.Item("searchCriteria").SearchCriteriaCompanyEmail.ToString
                End If
            End If

            'Company Phone:
            If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaCompanyPhone) Then
                If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaCompanyPhone) Then
                    company_phone.Text = Session.Item("searchCriteria").SearchCriteriaCompanyPhone.ToString
                End If
            End If

            'Yacht Fleet Dropdown.
            If Session.Item("localSubscription").crmYacht_Flag = True Then
                If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaCompanyYachtFleet) Then
                    If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaCompanyYachtFleet) Then
                        yacht_fleet.SelectedValue = Session.Item("searchCriteria").SearchCriteriaCompanyYachtFleet
                    End If
                End If
            End If

            'Fleet Value:
            If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaCompanyFleetValue) Then
                If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaCompanyFleetValue) Then
                    If Session.Item("searchCriteria").SearchCriteriaCompanyFleetValue <> 0 Then
                        company_fleet_value.Text = Session.Item("searchCriteria").SearchCriteriaCompanyFleetValue.ToString
                    End If
                End If
            End If

            'Fleet Answer
            If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaCompanyFleetAnswer) Then
                If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaCompanyFleetAnswer) Then
                    company_fleet.SelectedValue = Session.Item("searchCriteria").SearchCriteriaCompanyFleetAnswer.ToString
                End If
            End If
            'Fleet Operator
            If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaCompanyFleetOperator) Then
                If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaCompanyFleetOperator) Then
                    company_condition.SelectedValue = Session.Item("searchCriteria").SearchCriteriaCompanyFleetOperator.ToString
                End If
            End If

            If Session.Item("isMobile") = True Then
                If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaCompanyContinent) Then
                    If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaCompanyContinent) Then
                        mobileCountryOptions.SelectedValue = Session.Item("searchCriteria").SearchCriteriaCompanyContinent.ToString
                    End If
                End If

                If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaCompanyStateName) Then
                    If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaCompanyStateName) Then
                        mobileStateOptions.SelectedValue = Session.Item("searchCriteria").SearchCriteriaCompanyStateName.ToString
                    End If
                End If
            End If

            'Display Contact Info
            If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaCompanyDisplayContactInfo) Then
                If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaCompanyDisplayContactInfo) Then
                    If UCase(Session.Item("searchCriteria").SearchCriteriaCompanyDisplayContactInfo.ToString) = "TRUE" Then
                        company_contact_info.Checked = Session.Item("searchCriteria").SearchCriteriaCompanyDisplayContactInfo
                    End If
                End If
            End If

            'aircraft sales professionals
            If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaCompanyOnlyAircraftSalesProfessionals) Then
                If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaCompanyOnlyAircraftSalesProfessionals) Then
                    If UCase(Session.Item("searchCriteria").SearchCriteriaCompanyOnlyAircraftSalesProfessionals.ToString) = "TRUE" Then
                        company_aircraft_sales.Checked = Session.Item("searchCriteria").SearchCriteriaCompanyOnlyAircraftSalesProfessionals
                    End If
                End If
            End If

            'Not In Selected Relationship
            If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaCompanyNotInSelectedRelationship) Then
                If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaCompanyNotInSelectedRelationship) Then
                    If UCase(Session.Item("searchCriteria").SearchCriteriaCompanyNotInSelectedRelationship.ToString) = "TRUE" Then
                        comp_not_in_selected.Checked = Session.Item("searchCriteria").SearchCriteriaCompanyNotInSelectedRelationship
                    End If
                End If
            End If

            'Company Services:
            If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaServices) Then
                If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaServices) Then
                    Dim ServicesSelection As Array
                    ServicesSelection = Split(Replace(Session.Item("searchCriteria").SearchCriteriaServices, Constants.cSingleQuote, Constants.cEmptyString), Constants.cCommaDelim)
                    service_used.SelectedIndex = -1 'This will remove any previously selected items in the listbox, such as the selection of all
                    'that the page defaults to.
                    For ServiceSelectionCount = 0 To UBound(ServicesSelection)
                        For ListBoxCount As Integer = 0 To service_used.Items.Count() - 1
                            If UCase(service_used.Items(ListBoxCount).Value) = UCase(ServicesSelection(ServiceSelectionCount)) Then
                                service_used.Items(ListBoxCount).Selected = True
                            End If
                        Next
                    Next
                End If
            End If

            'Company Relationship
            If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaCompanyRelationshipsToAC) Then
                If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaCompanyRelationshipsToAC) Then
                    If Session.Item("isMobile") = True Then
                        mobileYesNoDropDown.SelectedValue = "Yes"
                        mobile_company_relationship.SelectedValue = Session.Item("searchCriteria").SearchCriteriaCompanyRelationshipsToAC
                    Else
                        Dim RelationshipSelection As Array
                        RelationshipSelection = Split(Session.Item("searchCriteria").SearchCriteriaCompanyRelationshipsToAC, "##")
                        company_relationship.SelectedIndex = -1 'This will remove any previously selected items in the listbox, such as the selection of all
                        'that the page defaults to.
                        For RelationshipSelectionCount = 0 To UBound(RelationshipSelection)
                            For ListBoxCount As Integer = 0 To company_relationship.Items.Count() - 1
                                If UCase(RelationshipSelection(RelationshipSelectionCount)) <> "" Then
                                    If UCase(company_relationship.Items(ListBoxCount).Value) = UCase(RelationshipSelection(RelationshipSelectionCount)) Then
                                        company_relationship.Items(ListBoxCount).Selected = True
                                    End If
                                End If
                            Next
                        Next
                    End If
                End If
            End If

            'Company Business Type:
            If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaCompanyBusinessType) Then
                If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaCompanyBusinessType) Then
                    If Session.Item("isMobile") = True Then
                        mobile_company_business.SelectedValue = Replace(Session.Item("searchCriteria").SearchCriteriaCompanyBusinessType, "'", "")
                    Else
                        Dim BusinessSelection As Array
                        BusinessSelection = Split(Replace(Session.Item("searchCriteria").SearchCriteriaCompanyBusinessType, "'", ""), ",")
                        company_business.SelectedIndex = -1 'This will remove any previously selected items in the listbox, such as the selection of all
                        'that the page defaults to.
                        For BusinessSelectionCount = 0 To UBound(BusinessSelection)
                            For ListBoxCount As Integer = 0 To company_business.Items.Count() - 1
                                If UCase(company_business.Items(ListBoxCount).Value) = UCase(BusinessSelection(BusinessSelectionCount)) Then
                                    company_business.Items(ListBoxCount).Selected = True
                                End If
                            Next
                        Next
                    End If
                End If
            End If

            'Certifications:
            If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaCompanyCertifications) Then
                If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaCompanyCertifications) Then
                    Dim CertificationsSelection As Array
                    CertificationsSelection = Split(Replace(Session.Item("searchCriteria").SearchCriteriaCompanyCertifications, "'", ""), ",")
                    comp_certifications.SelectedIndex = -1 'This will remove any previously selected items in the listbox, such as the selection of all
                    'that the page defaults to.
                    For CertificationSelectionCount = 0 To UBound(CertificationsSelection)
                        For ListBoxCount As Integer = 0 To comp_certifications.Items.Count() - 1
                            If UCase(comp_certifications.Items(ListBoxCount).Value) = UCase(CertificationsSelection(CertificationSelectionCount)) Then
                                comp_certifications.Items(ListBoxCount).Selected = True
                            End If
                        Next
                    Next
                End If
            End If

            'Contact Title Group:
            If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaCompanyContactTitle) Then
                If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaCompanyContactTitle) Then
                    Dim TitleSelection As Array
                    TitleSelection = Split(Replace(Session.Item("searchCriteria").SearchCriteriaCompanyContactTitle, "'", ""), ",")
                    company_contact_title.SelectedIndex = -1 'This will remove any previously selected items in the listbox, such as the selection of all
                    'that the page defaults to.
                    For TitleSelectionCount = 0 To UBound(TitleSelection)
                        For ListBoxCount As Integer = 0 To company_contact_title.Items.Count() - 1
                            If UCase(company_contact_title.Items(ListBoxCount).Value) = UCase(TitleSelection(TitleSelectionCount)) Then
                                company_contact_title.Items(ListBoxCount).Selected = True
                            End If
                        Next
                    Next
                End If
            End If
        Catch ex As Exception
            masterPage.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString & " : " & ex.Message.ToString)
        End Try
    End Sub

    '''' <summary>
    '''' Click part of the dropdown list, switch the submenu bullet with the main bullet
    '''' Also includes error reporting
    '''' </summary>
    '''' <param name="sender"></param>
    '''' <param name="e"></param>
    '''' <remarks></remarks>
    Public Sub submenu_dropdown_Click(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.BulletedListEventArgs)
        Try
            Dim selectedLI As New ListItem
            selectedLI = sender.Items(e.Index)
            If sender.id.ToString = "company_sort_submenu_dropdown" Then
                company_sort_dropdown.Items.Clear()
                company_sort_dropdown.Items.Add(New ListItem(selectedLI.Text, ""))
                SetPageSort(selectedLI.Text)
                company_search_Click(company_search, EventArgs.Empty)

            ElseIf sender.id.ToString = "company_view_submenu_dropdown" Then
                SwitchGalleryListing(e.Index)

                company_search_Click(company_search, EventArgs.Empty, True)
                'End Select
            ElseIf sender.id.ToString = "company_go_to_submenu_dropdown" Then
                company_go_to_dropdown.Items.Clear()
                company_go_to_dropdown.Items.Add(New ListItem(selectedLI.Text, ""))

                SetPageNumber(CInt(selectedLI.Text))
                MovePage(False, False, False, False, True, PageNumber - 1)

            ElseIf sender.id.ToString = "company_per_page_submenu_dropdown" Then
                company_per_page_dropdown.Items.Clear()
                company_per_page_dropdown.Items.Add(New ListItem(selectedLI.Text & " ", selectedLI.Text))
                Session.Item("localUser").crmUserRecsPerPage = CInt(selectedLI.Value)
                MovePage(False, False, False, False, False, 0)

            End If
        Catch ex As Exception
            masterPage.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString & " : " & ex.Message.ToString)
        End Try
    End Sub

    ''' <summary>
    ''' Small function to swap classes of listing view dropdown on ac listing page.
    ''' Error reporting is included.
    ''' </summary>
    ''' <param name="showtype"></param>
    ''' <remarks></remarks>
    Sub SwitchGalleryListing(ByVal showtype As Integer)
        Try
            Select Case showtype
                Case 0
                    company_view_dropdown.Items.Clear()
                    company_view_dropdown.Items.Add(New ListItem("", ""))
                    company_view_dropdown.CssClass = "ul_top listing_view_bullet"
                    AlterListing(0, 0)
                    Session.Item("localUser").crmCompanyListingView = eListingView.LISTING
                Case 1
                    company_view_dropdown.Items.Clear()
                    company_view_dropdown.Items.Add(New ListItem("", ""))
                    company_view_dropdown.CssClass = "ul_top thumnail_view_bullet"
                    AlterListing(1, 0)
                    Session.Item("localUser").crmCompanyListingView = eListingView.GALLERY
            End Select
        Catch ex As Exception
            masterPage.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString & " : " & ex.Message.ToString)
        End Try
    End Sub

    ''' <summary>
    ''' Toggles visibility of next/prev
    ''' </summary>
    ''' <param name="back_page"></param>
    ''' <param name="next_page"></param>
    ''' <remarks></remarks>
    Public Sub SetPagingButtons(ByVal back_page As Boolean, ByVal next_page As Boolean)
        Dim backCSS As String = "display_none"
        Dim forCSS As String = "display_none"
        If back_page = True Then
            backCSS = ""
        End If
        If next_page = True Then
            forCSS = ""
        End If

        bottom_previous.CssClass = backCSS
        bottom_previous_all.CssClass = backCSS

        company_previous_all.CssClass = backCSS
        company_previous.CssClass = backCSS

        company_next_all.CssClass = forCSS
        company_next.CssClass = forCSS
        bottom_next_.CssClass = forCSS
        bottom_next_all.CssClass = forCSS

    End Sub


    Public Sub Fill_Page_To_To_Dropdown(ByVal pageNumber As Integer)
        If Company_Criteria.Visible = True Then
            company_go_to_submenu_dropdown.Items.Clear()
            For x = 1 To pageNumber
                company_go_to_submenu_dropdown.Items.Add(New ListItem(x, x))
            Next
            company_next_all.CommandArgument = pageNumber.ToString
            company_previous_all.CommandArgument = "0"
            bottom_next_all.CommandArgument = pageNumber.ToString
            bottom_previous_all.CommandArgument = "0"
        End If
    End Sub


    Private Sub reset_form_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles reset_form.Click
        ResetPage()
    End Sub
    ''' <summary>
    ''' Runs on page when page load is complete.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Company_Listing_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        Try
            If Not Page.IsPostBack Then
                If Company_Criteria.Visible = True Then

                    If Session.Item("isMobile") = False Then
                        SwitchGalleryListing(Session.Item("localUser").crmCompanyListingView)
                    Else
                        SwitchGalleryListing(1)
                    End If
                    'This needs to be done on load complete because otherwise the array of the models is not stored in session yet
                    'and the first time we complete a project or homepage search, it will not work (until the array is filled later on
                    'in page lifecycle)
                    If Page.Request.Form("complete_search") = "Y" Or Page.Request.Form("project_search") = "Y" Then
                        'if either of these variables is passed, then go ahead and complete this search.
                        company_search_Click(company_search, EventArgs.Empty)
                    End If
                End If
            End If
            Company_Criteria.Focus()

        Catch ex As Exception
            masterPage.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString & " : " & ex.Message.ToString)
        End Try
    End Sub


    Private Sub Company_Listing_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

        Yacht = False

        If Session.Item("localUser").crmEvo = True Then
            If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO Then
                'Changed 7/13/15 to reflect the new master page if you're in mobile site
                If Session.Item("isMobile") Then
                    Me.MasterPageFile = "~/EvoStyles/MobileTheme.master"
                    masterPage = DirectCast(Page.Master, MobileTheme)
                    ResultsSearchDataList.RepeatColumns = 1
                Else
                    Me.MasterPageFile = "~/EvoStyles/EvoTheme.master"
                    masterPage = DirectCast(Page.Master, EvoTheme)
                End If

                yacht_ac_cell.CssClass = "display_none"

                chkShowInactiveCompany.Visible = False
                chkShowHiddenCompany.Visible = False

                chkShowHiddenContact.Visible = False
                chkShowInactiveContact.Visible = False


            ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT Then
                Me.MasterPageFile = "~/EvoStyles/YachtTheme.master"
                masterPage = DirectCast(Page.Master, YachtTheme)
                company_fleet.SelectedValue = ""
                company_condition.SelectedValue = ""
                company_fleet_value.Text = ""
                product_spacer.Visible = True
                relationship_text.Text = "Relationships to Yacht:"
                fleet_cell.CssClass = "display_none"
                company_aircraft_sales.Checked = False
                company_aircraft_sales.CssClass = "display_none"
                comp_contacts_yacht_label.Visible = True
                comp_contacts_yacht_label.Text = "<b>Companies/Contacts</b><br>"
                comp_product_business_flag.Text = "Business Aircraft"
                comp_product_commercial_flag.Text = "Commercial  Aircraft"


                Yacht = True
                chkShowInactiveCompany.Visible = True
                chkShowHiddenCompany.Visible = False

                chkShowHiddenContact.Visible = False
                chkShowInactiveContact.Visible = False

                company_actions_submenu_dropdown.Items.Clear()
                company_actions_submenu_dropdown.CssClass = "ul_bottom yacht_action_dropdown"
                company_actions_submenu_dropdown.Items.Add(New ListItem("Save As - New Folder", "javascript:SubMenuDrop(3,0, 'COMPANY');"))
                company_actions_submenu_dropdown.Items.Add(New ListItem("Custom Export", "javascript:SubMenuDrop(1,0, 'YACHT COMPANY');"))
                company_actions_submenu_dropdown.Items.Add(New ListItem("YachtSpot Export/Report", "javascript:SubMenuDrop(5,0,'COMPANY');"))
            ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
                Me.MasterPageFile = "~/EvoStyles/CustomerAdminTheme.master"
                masterPage = DirectCast(Page.Master, CustomerAdminTheme)
                yacht_ac_cell.CssClass = "display_none"

                chkShowInactiveCompany.Visible = True
                chkShowHiddenCompany.Visible = True

                chkShowHiddenContact.Visible = True
                chkShowInactiveContact.Visible = True
            ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Then
                Me.MasterPageFile = "~/EvoStyles/HomebaseTheme.Master"
                masterPage = DirectCast(Page.Master, HomebaseTheme)

                yacht_ac_cell.CssClass = "display_none"

                chkShowInactiveCompany.Visible = True
                chkShowHiddenCompany.Visible = True

                chkShowHiddenContact.Visible = True
                chkShowInactiveContact.Visible = True

            End If
        End If

        masterPage.SetDefaultButtion(Me.company_search.UniqueID)
    End Sub

    Public Function EvolutionCompanyListingPageQuery(ByVal CompanyName As String, ByVal CompanyBusinessType As String, ByVal CompanyCertifications As String,
                                                     ByVal CompanyRelationship As String, ByVal NotInRelationship As Boolean,
                                                     ByVal CompanyCity As String, ByVal CompanyPostal As String,
                                                     ByVal CompanyCountry As String, ByVal CompanyContinent As String,
                                                     ByVal RegionString As String, ByVal Timezone As String,
                                                     ByVal ContactTitle As String, ByVal ContactFirstName As String,
                                                     ByVal ContactLastName As String, ByVal DisplayContactInfo As Boolean,
                                                     ByVal CompanyAgency As String, ByVal CompanyEmail As String,
                                                     ByVal CompanyPhoneNumber As String, ByVal CompanyAddress As String,
                                                     ByVal compID As String, ByVal OperatorFlag As String,
                                                     ByVal pageSort As String, ByVal BusinessFlag As Boolean,
                                                     ByVal HelicopterFlag As Boolean, ByVal CommercialFlag As Boolean,
                                                     ByVal Fleet As String, ByVal FleetCondition As String, ByVal FleetValue As String,
                                                     ByVal aircraftSalesOnly As Boolean, ByVal contactID As String,
                                                     ByVal ContinentString As String, ByVal StateName As String,
                                                     ByVal YachtFlag As Boolean, ByVal YachtFleet As String,
                                                     ByVal ac_fleet As String, ByVal DynamicQueryString As String,
                                                     ByVal comp_id As String, ByVal custom_serach As String,
                                                     ByVal CompanyCertifications2 As String) As DataTable
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim TempTable As New DataTable
        Dim query_sql As String = ""
        Dim query_select As String = ""
        Dim query_where As String = ""
        Dim queryFrom As String = ""
        Dim UseAlternate As Boolean = False
        Dim count As Long = 1
        Dim Yacht As Boolean = False 'Variable set whenever you're on the yacht application only. YachtFlag is set on Evo side when you have yacht checkbox checked

        If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT Then
            Yacht = True
        End If
        Dim comp_count As DataColumn = New DataColumn("comp_count", Type.GetType("System.Int64"))
        comp_count.AutoIncrement = True
        comp_count.AutoIncrementSeed = 1
        TempTable.Columns.Add(comp_count)

        TempTable.Columns.Add("comp_id")
        TempTable.Columns.Add("comp_name")
        TempTable.Columns.Add("comp_alternate_name")
        TempTable.Columns.Add("comp_alternate_name_type")
        TempTable.Columns.Add("comp_address1")
        TempTable.Columns.Add("comp_address2")
        TempTable.Columns.Add("comp_city")
        TempTable.Columns.Add("comp_state")
        TempTable.Columns.Add("comp_zip_code")
        TempTable.Columns.Add("comp_country")
        TempTable.Columns.Add("comp_agency_type")
        TempTable.Columns.Add("comp_web_address")
        TempTable.Columns.Add("comp_email_address")
        TempTable.Columns.Add("comp_jetnet_comp_id")
        TempTable.Columns.Add("comp_user_id")
        TempTable.Columns.Add("comp_action_date")
        TempTable.Columns.Add("comp_logo_flag")
        TempTable.Columns.Add("source")
        TempTable.Columns.Add("comp_product_helicopter_flag")
        TempTable.Columns.Add("comp_product_business_flag")
        TempTable.Columns.Add("comp_product_commercial_flag")
        TempTable.Columns.Add("comp_phone_office")
        TempTable.Columns.Add("comp_phone_fax")
        TempTable.Columns.Add("contact_first_name")
        TempTable.Columns.Add("contact_middle_initial")
        TempTable.Columns.Add("contact_suffix")
        TempTable.Columns.Add("contact_last_name")
        TempTable.Columns.Add("contact_title")
        TempTable.Columns.Add("contact_id")
        TempTable.Columns.Add("contact_sirname")

        TempTable.Columns.Add("PASTCUSTEND")
        TempTable.Columns.Add("PROSPECT")
        TempTable.Columns.Add("SERVICESUSED")

        Try
            HttpContext.Current.Session.Item("MasterCompany") = "" 'Whole Search
            HttpContext.Current.Session.Item("MasterCompanyWhere") = "" 'Where only
            HttpContext.Current.Session.Item("MasterCompanyFrom") = "" 'From Only

            query_select = "SELECT DISTINCT comp_id,  comp_name, comp_name_alt as comp_alternate_name, comp_name_alt_type as comp_alternate_name_type, "
            query_select += "comp_address1, comp_address2, comp_city, comp_state, comp_zip_code, comp_country, "
            query_select += " comp_agency_type, comp_web_address, comp_email_address, comp_id as "
            query_select += "comp_jetnet_comp_id, 0 as comp_user_id, comp_action_date,"
            query_select += "'JETNET' as source, comp_product_helicopter_flag,	"
            query_select += " comp_product_business_flag,	comp_product_commercial_flag, comp_logo_flag"

            If (Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER) Or (Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE) Then
                If chkSearchCustomerTargets.Checked Then
                    query_select += ", PASTCUSTEND, PROSPECT, SERVICESUSED"
                End If
            End If

            If DisplayContactInfo Then
                query_select += ", contact_suffix, contact_middle_initial, contact_first_name, contact_last_name, contact_title, contact_id, contact_sirname "
            Else
                query_select += ", '' as contact_suffix, '' as contact_middle_initial, '' as contact_first_name, '' as contact_last_name, '' as contact_title, 0 as contact_id, '' as contact_sirname "
            End If

            'If the company relationship is blank,there is no operator flag, phone number and aircraft sales only isn't checked OR if it's on the yacht side.
            If (CompanyRelationship = "" And OperatorFlag = "" Or NotInRelationship = True) Or Yacht = True Then

                If HttpContext.Current.Session.Item("isMobile") = False Then
                    query_select += ", (select top 1 pnum_number_full FROM " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Phone_Numbers WITH(NOLOCK) INNER JOIN " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Phone_Type WITH(NOLOCK) ON ptype_name = pnum_type WHERE comp_id = pnum_comp_id and pnum_contact_id = 0 and pnum_journ_id = 0 ORDER BY ptype_seq_no ASC) AS comp_phone_office,"
                    query_select += " (select top 1 pnum_number_full FROM " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Phone_Numbers WITH(NOLOCK) INNER JOIN " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Phone_Type WITH(NOLOCK) ON ptype_name = pnum_type WHERE pnum_type ='Fax' and comp_id = pnum_comp_id and pnum_contact_id = 0 and pnum_journ_id = 0 ORDER BY ptype_seq_no ASC) AS comp_phone_fax"
                Else
                    query_select += ", (select top 1 pnum_number_full FROM " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Phone_Numbers WITH(NOLOCK) INNER JOIN " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Phone_Type WITH(NOLOCK) ON ptype_name = pnum_type WHERE comp_id = pnum_comp_id and pnum_contact_id = 0 and pnum_journ_id = 0 ORDER BY ptype_seq_no ASC) AS comp_phone_office, '' as comp_phone_fax "
                End If

                queryFrom = "  FROM " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Company WITH(NOLOCK)"

                'If the yacht relationship is picked, we need this table joined.
                If (Yacht = True And CompanyRelationship <> "") Then
                    queryFrom += " INNER JOIN " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Yacht_Reference WITH(NOLOCK) on comp_id = yr_comp_id and comp_journ_id = yr_journ_id "
                    queryFrom += " INNER JOIN " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Yacht WITH(NOLOCK) on yr_yt_id = yt_id and yr_journ_id = yt_journ_id"
                End If

                queryFrom += " LEFT OUTER JOIN " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "State WITH(NOLOCK) on state_code = comp_state and state_country=comp_country"
                queryFrom += " LEFT OUTER JOIN " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Contact WITH(NOLOCK) ON (comp_id = contact_comp_id AND comp_journ_id = contact_journ_id)"

                ' added msw - from below where clause into inner join - 6/29/20 
                If (Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER) Or (Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE) Then

                    ' if we have not checked to include hidden, then onle join to non hiddne 
                    If chkShowHiddenCompany.Checked Then
                    ElseIf chkShowHiddenContact.Checked Then
                    Else
                        queryFrom += " AND contact_hide_flag = 'N'"
                    End If

                    If DisplayContactInfo = True Or ContactFirstName <> "" Or ContactLastName <> "" Or ContactTitle <> "" Then

                        If chkShowInactiveCompany.Checked Then
                            ' if we say include inactive companies, then also include inactive contacts 
                        ElseIf Not chkShowInactiveContact.Checked Then
                            queryFrom += " AND contact_active_flag = 'Y'"
                        End If

                        ' if we are showing company hidden, we should show contact hidden 
                        If chkShowHiddenCompany.Checked Then
                            queryFrom += " AND contact_hide_flag <> ''"
                        ElseIf chkShowHiddenContact.Checked Then
                            queryFrom += " AND contact_hide_flag <> ''"
                        Else
                            queryFrom += " AND contact_hide_flag = 'N'"
                        End If

                    Else
                        ' if we are showing company hidden, we should show contact hidden 
                        If chkShowHiddenCompany.Checked Then
                            queryFrom += " AND contact_hide_flag <> ''"
                        ElseIf chkShowHiddenContact.Checked Then
                            queryFrom += " AND contact_hide_flag <> ''"
                        Else
                            queryFrom += " AND contact_hide_flag = 'N'"
                        End If
                    End If


                Else
                    queryFrom += " AND contact_active_flag = 'Y' AND contact_hide_flag = 'N'"
                End If

                If ContinentString <> "" Then
                    queryFrom += " INNER JOIN " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Country WITH(NOLOCK) on comp_country = country_name "
                End If

                If chkSearchCustomerTargets.Checked Then
                    queryFrom += " INNER JOIN " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "View_Customer_Targets WITH(NOLOCK) on comp_id = ctarget_comp_id"
                End If

                query_where = " comp_journ_id = 0 "

                ' contact display moved from here - msw - 7/2/20

            Else

                UseAlternate = True

                If HttpContext.Current.Session.Item("isMobile") = False Then
                    query_select = query_select & ", comp_phone_office, comp_phone_fax "
                Else
                    query_select = query_select & ", (select top 1 pnum_number_full FROM " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Phone_Numbers WITH(NOLOCK) INNER JOIN " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Phone_Type WITH(NOLOCK) ON ptype_name = pnum_type WHERE comp_id = pnum_comp_id and pnum_contact_id = 0 and pnum_journ_id = 0 ORDER BY ptype_seq_no ASC) AS comp_phone_office, '' as comp_phone_fax "
                End If

                queryFrom = " FROM " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "View_Aircraft_Company_Flat "
                query_where = " cref_journ_id = 0 "

            End If

            If Yacht Then

                If Not chkShowInactiveCompany.Checked Then
                    query_where += " AND comp_active_flag='Y'"
                End If

            End If

            If (Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER) Or (Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE) Then

                If chkShowInactiveCompany.Checked Then
                    ' if we say include inactive companies, then also include inactive contacts 
                ElseIf Not chkShowInactiveCompany.Checked Then
                    query_where += " AND comp_active_flag = 'Y'"
                End If

                If chkShowHiddenCompany.Checked Then
                    query_where += " AND comp_hide_flag <> ''"
                Else
                    query_where += " AND comp_hide_flag = 'N'"
                End If

            Else

                query_where += " AND comp_active_flag = 'Y' AND comp_hide_flag = 'N'"

            End If

            If (Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER) Or (Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE) Then

                If chkSearchCustomerTargets.Checked Then

                    Select Case targets_previous_customer.SelectedValue
                        Case "No"
                            query_where += " AND PASTCUSTEND IS NULL"
                        Case "Yes"
                            query_where += " AND PASTCUSTEND IS NOT NULL"
                    End Select

                    Select Case targets_prospect.SelectedValue
                        Case "No"
                            query_where += " AND PROSPECT IS NULL"
                        Case "Yes"
                            query_where += " AND PROSPECT IS NOT NULL"
                    End Select

                    If Not String.IsNullOrEmpty(targets_services_used.SelectedValue.Trim) Then
                        query_where += " AND SERVICESUSED like '%" + targets_services_used.SelectedValue.Trim + "%'"
                    End If

                    For i = 0 To targets_customer_segments.Items.Count - 1
                        If targets_customer_segments.Items(i).Selected Then

                            Select Case targets_customer_segments.Items(i).Value.ToLower
                                Case "jet"
                                    query_where += " AND comp_id IN (SELECT distinct cref_comp_id FROM " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Aircraft_Reference WITH (NOLOCK)"
                                    query_where += " INNER JOIN " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Aircraft WITH (NOLOCK) ON cref_ac_id = ac_id AND cref_journ_id = ac_journ_id"
                                    query_where += " INNER JOIN " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Aircraft_Model WITH (NOLOCK) ON ac_amod_id = amod_id"
                                    query_where += " WHERE cref_journ_id = 0 AND cref_contact_type IN ('93', '99', '38') AND amod_type_code IN ('J', 'E') AND amod_airframe_type_code = 'F')"
                                Case "turbo"
                                    query_where += " AND comp_id IN (SELECT distinct cref_comp_id FROM " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Aircraft_Reference WITH (NOLOCK)"
                                    query_where += " INNER JOIN " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Aircraft WITH (NOLOCK) ON cref_ac_id = ac_id AND cref_journ_id = ac_journ_id"
                                    query_where += " INNER JOIN " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Aircraft_Model WITH (NOLOCK) ON ac_amod_id = amod_id"
                                    query_where += " WHERE cref_journ_id = 0 AND cref_contact_type IN ('93', '99', '38') AND amod_type_code IN ('P', 'T') AND amod_airframe_type_code = 'F')"
                                Case "heli"
                                    query_where += " AND comp_id IN (SELECT distinct cref_comp_id FROM " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Aircraft_Reference WITH (NOLOCK)"
                                    query_where += " INNER JOIN " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Aircraft WITH (NOLOCK) ON cref_ac_id = ac_id AND cref_journ_id = ac_journ_id"
                                    query_where += " INNER JOIN " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Aircraft_Model WITH (NOLOCK) ON ac_amod_id = amod_id"
                                    query_where += " WHERE cref_journ_id = 0 AND cref_contact_type IN ('93', '99', '38') AND amod_airframe_type_code = 'R')"
                            End Select

                        End If
                    Next
                End If

            End If

            If Not String.IsNullOrEmpty(compID.Trim) Then ' comma list of company IDs

                If query_where <> "" Then
                    query_where += " AND "
                End If

                query_where += " comp_id IN (" + compID.Replace("'", "").Trim + ")"

            End If

            If Not String.IsNullOrEmpty(comp_id.Trim) Then ' single company ID

                If query_where <> "" Then
                    query_where += " AND "
                End If

                If InStr(comp_id, "comp_id") > 0 Then
                    query_where += "  " & comp_id.Trim
                Else
                    query_where += " comp_id " & comp_id.Trim
                End If
            End If

            If Not String.IsNullOrEmpty(CompanyName.Trim) Then
                If query_where <> "" Then
                    query_where += " AND "
                End If
                query_where += CompanyName
            End If

            If CompanyRelationship <> "" Then
                If query_where <> "" Then
                    query_where += " AND "
                End If
                query_where += CompanyRelationship
            End If

            If Timezone <> "" Then
                If query_where <> "" Then
                    query_where += " AND "
                End If
                query_where += " comp_timezone in (SELECT tzone_name FROM " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Timezone where tzone_id in (" & Timezone & "))"
            End If

            If aircraftSalesOnly Then
                If query_where <> "" Then
                    query_where += " AND "
                End If
                query_where += " comp_acpros_flag = 'Y' "
            End If

            If Fleet <> "" And FleetCondition <> "" And FleetValue <> "" Then
                queryFrom += " INNER JOIN " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Company_Aircraft_Count WITH(NOLOCK) ON cac_comp_id = comp_id and cac_journ_id = "
                If UseAlternate = False Then
                    queryFrom += " comp_journ_id "
                Else
                    queryFrom += " cref_journ_id "
                End If
                query_where += " and ( "
                Select Case Fleet
                    Case "Owner"
                        query_where += " cac_fullsale_owner "
                    Case "Operator"
                        query_where += " cac_fullsale_operator "
                    Case "Co-Owner"
                        query_where += " cac_sharesale_owner "
                    Case Else
                        query_where += " cac_fractionsale_owner "
                End Select

                Select Case FleetCondition
                    Case "Less Than"
                        query_where += " < "
                        FleetValue = Replace(FleetValue.ToString, ":", "")
                    Case "Greater Than"
                        query_where += " > "
                        FleetValue = Replace(FleetValue.ToString, ":", "")
                    Case "Between"
                        If InStr(FleetValue, ":") > 0 Then
                            query_where += " BETWEEN "
                            FleetValue = Replace(FleetValue.ToString, ":", " and ")
                        Else
                            query_where += " = "
                        End If
                    Case Else
                        query_where += " = "
                        FleetValue = Replace(FleetValue.ToString, ":", "")
                End Select
                query_where += FleetValue.ToString

                'This is the added product type:
                'This will basically allow us to go ahead and better filter the results we get.
                'and ( cac_product_type in ('B','H','C') )
                query_where += " and cac_product_type in ("

                If BusinessFlag Then
                    query_where += " 'B' "
                    If HelicopterFlag Or CommercialFlag Then
                        query_where += ","
                    End If
                End If
                If HelicopterFlag Then
                    query_where += "'H'"
                    If CommercialFlag Then
                        query_where += ","
                    End If
                End If
                If CommercialFlag Then
                    query_where += "'C'"
                End If
                query_where += " )"
                query_where += " )"
            End If

            If CompanyPhoneNumber <> "" Then
                If query_where <> "" Then
                    query_where += " AND "
                End If
                CompanyPhoneNumber = clsGeneral.clsGeneral.CleanUserData(CompanyPhoneNumber, Constants.cEmptyString, Constants.cCommaDelim, True)
                CompanyPhoneNumber = Replace(CompanyPhoneNumber, "-", "")
                query_where += "( "

                ''search company phone office
                query_where += " comp_id in (select distinct pnum_comp_id from " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Phone_Numbers where pnum_comp_id=comp_id and pnum_journ_id = 0 and pnum_hide_customer='N' and "
                query_where += BuildPhoneQueryForCompanySearch(CompanyPhoneNumber, "pnum_number_full_search")
                query_where += ") "


                query_where += " )"
            End If

            If ContactTitle <> "" Then
                If query_where <> "" Then
                    query_where += " AND "
                End If
                query_where += " contact_title IN (SELECT ctitlegref_title_name FROM " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Contact_Title_Group_Reference WHERE(ctitlegref_group_name in (" & ContactTitle & ")))"
            End If


            If Trim(ac_fleet) <> "" Then
                Select Case UCase(ac_fleet)
                    Case "SHOW AC OWNERS"
                        If query_where <> "" Then
                            query_where += " AND "
                        End If
                        query_where += " (EXISTS (SELECT NULL FROM " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Aircraft_Reference WITH (NOLOCK) "
                        query_where += " WHERE (cref_comp_id = comp_id) "
                        query_where += " AND (cref_journ_id = 0)"
                        query_where += " AND (cref_contact_type in ('00','08') )  "
                        query_where += "  )) "
                    Case "SHOW HELI OWNERS"
                        If query_where <> "" Then
                            query_where += " AND "
                        End If
                        query_where += " (EXISTS (SELECT NULL FROM " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Aircraft_Reference WITH (NOLOCK) "
                        query_where += " INNER JOIN " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "AIRCRAFT with (NOLOCK) on ac_id = cref_ac_id and ac_journ_id = cref_journ_id and ac_product_helicopter_flag ='Y' "
                        query_where += " WHERE (cref_comp_id = comp_id) "
                        query_where += " AND (cref_journ_id = 0)"
                        query_where += " AND (cref_contact_type in ('00','08') )  "
                        query_where += "  )) "
                    Case "SHOW JET OWNERS"
                        If query_where <> "" Then
                            query_where += " AND "
                        End If
                        query_where += " (EXISTS (SELECT NULL FROM " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Aircraft_Reference WITH (NOLOCK) "
                        query_where += " INNER JOIN " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "AIRCRAFT_FLAT with (NOLOCK) on ac_id = cref_ac_id and ac_journ_id = cref_journ_id and ac_product_business_flag ='Y' "
                        query_where += " WHERE (cref_comp_id = comp_id) "
                        query_where += " AND (cref_journ_id = 0)"
                        query_where += " AND (cref_contact_type in ('00','08') ) and ac_product_business_flag='Y' and amod_type_code in ('J','E') "
                        query_where += "  )) "

                    Case "SHOW TURBOPROP OWNERS" ' Show TurboProp Owners
                        If query_where <> "" Then
                            query_where += " AND "
                        End If
                        query_where += " (EXISTS (SELECT NULL FROM " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Aircraft_Reference WITH (NOLOCK) "
                        query_where += " INNER JOIN " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "AIRCRAFT_FLAT with (NOLOCK) on ac_id = cref_ac_id and ac_journ_id = cref_journ_id and ac_product_business_flag ='Y' "
                        query_where += " WHERE (cref_comp_id = comp_id) "
                        query_where += " AND (cref_journ_id = 0)"
                        query_where += " AND (cref_contact_type in ('00','08') ) and ac_product_business_flag='Y' and amod_type_code in ('T') "
                        query_where += "  )) "

                    Case "SHOW ANY RELATED"
                        If query_where <> "" Then
                            query_where += " AND "
                        End If
                        query_where += " (EXISTS (SELECT NULL FROM " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Aircraft_Reference WITH (NOLOCK) "
                        query_where += " WHERE (cref_comp_id = comp_id) "
                        query_where += " AND (cref_journ_id = 0)"
                        query_where += "  )) "
                End Select
            End If

            If YachtFleet <> "" Then
                'This is a string, it's going to only have 4 options if anything is filled in (otherwise ignored).
                Select Case UCase(YachtFleet)
                    Case "SHOW YACHT OWNERS"
                        If query_where <> "" Then
                            query_where += " AND "
                        End If
                        query_where += " (EXISTS (SELECT NULL FROM " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Yacht_Reference WITH (NOLOCK) "
                        query_where += " WHERE (yr_comp_id = comp_id) "
                        query_where += " AND (yr_journ_id = 0)"
                        query_where += " AND (yr_contact_type in ('00','08') )  "
                        query_where += "  )) "
                    Case "SHOW COMPANIES RELATED TO YACHTS"
                        If query_where <> "" Then
                            query_where += " AND "
                        End If
                        query_where += " (EXISTS (SELECT NULL FROM " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Yacht_Reference WITH (NOLOCK) "
                        query_where += " WHERE (yr_comp_id = comp_id) "
                        query_where += " AND (yr_journ_id = 0)"
                        query_where += "  )) "
                    Case "SHOW COMPANIES NOT OWNING YACHTS"
                        If query_where <> "" Then
                            query_where += " AND "
                        End If
                        query_where += " (EXISTS (SELECT NULL FROM " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Yacht_Reference WITH (NOLOCK) "
                        query_where += " WHERE (yr_comp_id = comp_id) "
                        query_where += " AND (yr_journ_id = 0)"
                        query_where += " AND (yr_contact_type not in ('00','08') )  "
                        query_where += "  )) "
                    Case "SHOW COMPANIES NOT RELATED TO YACHTS"
                        If query_where <> "" Then
                            query_where += " AND "
                        End If
                        query_where += " (NOT EXISTS (SELECT NULL FROM " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Yacht_Reference WITH (NOLOCK) "
                        query_where += " WHERE (yr_comp_id = comp_id) "
                        query_where += " AND (yr_journ_id = 0)"
                        query_where += "  )) "
                End Select
            End If


            If Not String.IsNullOrEmpty(OperatorFlag.Trim) And NotInRelationship = False Then
                If query_where <> "" Then
                    query_where += " AND "
                End If
                query_where += OperatorFlag
            End If

            'Continent
            If ContinentString <> "" Then
                If query_where <> "" Then
                    query_where += " AND"
                End If
                query_where += " country_continent_name in (" & ContinentString & ")"
            End If

            If String.IsNullOrEmpty(RegionString.Trim) Then

                ' check the country
                If Not String.IsNullOrEmpty(CompanyCountry.Trim) Then

                    If query_where <> "" Then
                        query_where += " AND "
                    End If

                    query_where += " comp_country in (" + CompanyCountry + ")"
                End If

            End If

            'check the address:
            If CompanyAddress <> "" Then
                If query_where <> "" Then
                    query_where += " AND "
                End If

                query_where += "( comp_address1 LIKE '" & CompanyAddress & "%'"
                query_where += " or comp_address2 LIKE '" & CompanyAddress & "%' )"

            End If

            If ContactFirstName <> "" Then
                If query_where <> "" Then
                    query_where += " AND "
                End If

                If InStr(ContactFirstName, "*") = 0 Then
                    query_where += " contact_first_name " & clsGeneral.clsGeneral.PrepQueryString("Begins With", ContactFirstName, "String", False, "contact_first_name", True)
                Else
                    query_where += " " & clsGeneral.clsGeneral.PrepQueryString("Begins With", ContactFirstName, "String", False, "contact_first_name", True)
                End If
            End If

            'check contactID
            If Not String.IsNullOrEmpty(contactID.Trim) Then
                If query_where <> "" Then
                    query_where += " AND "
                End If
                query_where += " contact_id in (" & contactID & ")"
            End If

            If ContactLastName <> "" Then
                If query_where <> "" Then
                    query_where += " AND "
                End If

                If InStr(ContactLastName, "*") = 0 Then
                    query_where += " contact_last_name " & clsGeneral.clsGeneral.PrepQueryString("Begins With", ContactLastName, "String", False, "contact_last_name", False)
                Else
                    query_where += " " & clsGeneral.clsGeneral.PrepQueryString("Begins With", ContactLastName, "String", False, "contact_last_name", False)
                End If
            End If

            'search company email address
            If CompanyEmail <> "" Then
                If query_where <> "" Then
                    query_where += " AND "
                End If
                query_where += "( "
                CompanyEmail = clsGeneral.clsGeneral.CleanUserData(CompanyEmail, Constants.cEmptyString, Constants.cCommaDelim, True)

                If InStr(CompanyEmail, "*") = 0 Then
                    query_where += " comp_email_address " & clsGeneral.clsGeneral.PrepQueryString("Begins With", CompanyEmail, "String", False, "comp_email_address", True)
                Else
                    query_where += " " & clsGeneral.clsGeneral.PrepQueryString("Begins With", CompanyEmail, "String", False, "comp_email_address", True)
                End If

                'search contact email address
                If InStr(CompanyEmail, "*") = 0 Then
                    query_where += " or contact_email_address " & clsGeneral.clsGeneral.PrepQueryString("Begins With", CompanyEmail, "String", False, "contact_email_address", True)
                Else
                    query_where += " or " & clsGeneral.clsGeneral.PrepQueryString("Begins With", CompanyEmail, "String", False, "contact_email_address", True)
                End If

                query_where += " )"
            End If


            ' check the city
            If CompanyCity <> "" Then
                If query_where <> "" Then
                    query_where += " AND "
                End If
                Dim TemporaryOperator As String = "Begins With"
                CompanyCity = clsGeneral.clsGeneral.CleanUserData(CompanyCity, Constants.cEmptyString, Constants.cCommaDelim, True)

                If InStr(CompanyPostal, "*") = 0 Then
                    query_where += " " & "comp_city" & " " & clsGeneral.clsGeneral.PrepQueryString(TemporaryOperator, CompanyCity, "String", False, "comp_city", True)
                Else
                    query_where += " " & clsGeneral.clsGeneral.PrepQueryString(TemporaryOperator, CompanyCity, "String", False, "comp_city", True)
                End If
            End If


            ' check the zip
            If CompanyPostal <> "" Then
                If query_where <> "" Then
                    query_where += " AND "
                End If
                Dim TemporaryOperator As String = "="

                CompanyPostal = clsGeneral.clsGeneral.CleanUserData(CompanyPostal, Constants.cEmptyString, Constants.cCommaDelim, True)

                query_where += " " & clsGeneral.clsGeneral.ZipCodePrepQueryString(TemporaryOperator, CompanyPostal, "String", False, "comp_zip_code", True)

            End If

            'check agency type

            If Not String.IsNullOrEmpty(CompanyAgency.Trim) Then
                If query_where <> "" Then
                    query_where += " AND "
                End If
                query_where += " comp_agency_type = '" & CompanyAgency & "'"
            End If

            ' check the state
            If String.IsNullOrEmpty(RegionString.Trim) Then

                If Not String.IsNullOrEmpty(StateName.Trim) Then

                    If query_where <> "" Then
                        query_where += " AND "
                    End If

                    query_where += "( state_name IN (" & StateName & ")"

                    If clsGeneral.clsGeneral.isCrmDisplayMode() Then
                        query_where += " OR  ("
                        query_where += " comp_state IN (" & StateName & "))"
                    End If
                    query_where += " )"
                End If

            End If

            'regions
            If RegionString <> "" Then
                If query_where <> "" Then
                    query_where += " AND "
                End If

                If Not String.IsNullOrEmpty(CompanyCountry.Trim) Then
                    query_where += "comp_country in (" & CompanyCountry & ")"
                Else
                    query_where += "comp_country in (select distinct geographic_country_name FROM " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "geographic with (NOLOCK) where geographic_region_name in (" & RegionString & "))"
                End If

                If Not String.IsNullOrEmpty(StateName.Trim) Then
                    query_where += " AND state_name in (select distinct state_name FROM " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "geographic with (NOLOCK) inner join " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "State with (NOLOCK) on state_code=geographic_state_code and state_country=geographic_country_name where geographic_region_name in (" & RegionString & ")) "
                End If
            End If

            'cref_business_type
            If CompanyBusinessType <> "" Then
                If query_where <> "" Then
                    query_where += " AND "
                End If
                query_where += " (EXISTS (SELECT NULL FROM " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Business_Type_Reference WITH(NOLOCK) where (bustypref_comp_id = comp_id and bustypref_journ_id = 0) and bustypref_type IN (" & CompanyBusinessType & ") ))"
            End If

            'comp_certifications
            If CompanyCertifications <> "" Then
                If query_where <> "" Then
                    query_where += " AND "
                End If
                query_where += " (EXISTS (SELECT NULL FROM " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Company_Certification WITH(NOLOCK) where (ccert_comp_id = comp_id and ccert_journ_id = 0) and ccert_type_id IN (" & CompanyCertifications & ") ))"
            End If

            'comp_certifications
            If CompanyCertifications2 <> "" Then
                If query_where <> "" Then
                    query_where += " AND "
                End If
                query_where += " (EXISTS (SELECT NULL FROM " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Company_Certification WITH(NOLOCK) where (ccert_comp_id = comp_id and ccert_journ_id = 0) and ccert_type_id IN (" & CompanyCertifications2 & ") ))"
            End If

            If Trim(custom_serach) <> "" Then
                query_where += Trim(custom_serach)
            End If


            Dim HoldClsSubscription As New crmSubscriptionClass

            HoldClsSubscription.crmAerodexFlag = HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag
            HoldClsSubscription.crmBusiness_Flag = BusinessFlag
            HoldClsSubscription.crmCommercial_Flag = CommercialFlag
            HoldClsSubscription.crmHelicopter_Flag = HelicopterFlag

            HoldClsSubscription.crmYacht_Flag = YachtFlag

            HoldClsSubscription.crmJets_Flag = HttpContext.Current.Session.Item("localSubscription").crmJets_Flag
            HoldClsSubscription.crmTurboprops = HttpContext.Current.Session.Item("localSubscription").crmTurboprops
            HoldClsSubscription.crmExecutive_Flag = HttpContext.Current.Session.Item("localSubscription").crmExecutive_Flag

            query_where += " " & clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_YachtsIncluded(HoldClsSubscription, True, False)

            If DynamicQueryString <> "" Then
                If query_where <> "" Then
                    query_where += " and "
                End If
                query_where += DynamicQueryString
            End If


            'add the select and the from
            query_select = query_select + queryFrom
            'store the from
            HttpContext.Current.Session.Item("MasterCompanyFrom") = queryFrom

            'put all together
            query_sql = query_select + " WHERE " + query_where
            'add sort.
            If pageSort <> "" Then
                query_sql = query_sql + " order by " + pageSort
            Else
                query_sql = query_sql + " order by comp_name"
            End If

            HttpContext.Current.Session.Item("MasterCompanyWhere") = query_where


            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, query_sql.ToString)


            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase")

            SqlConn.Open()
            SqlCommand.Connection = SqlConn

            'This will be used on the Folder Maintenance Page to save the query:
            HttpContext.Current.Session.Item("MasterCompany") = query_sql

            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 120
            SqlCommand.CommandText = query_sql

            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                TempTable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = TempTable.GetErrors()
            End Try

            Return TempTable

        Catch ex As Exception
            EvolutionCompanyListingPageQuery = Nothing
            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message

        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing

            SqlCommand.Dispose()
            SqlCommand = Nothing

        End Try

    End Function

    Public Function BuildPhoneQueryForCompanySearch(ByVal CompanyPhoneNumber As String, ByVal CompanyFieldName As String) As String
        Dim Query_Where As String = ""
        If InStr(CompanyPhoneNumber, "*") = 0 Then
            Query_Where += " " & CompanyFieldName & " " & clsGeneral.clsGeneral.PrepQueryString("Begins With", CompanyPhoneNumber, "String", False, CompanyFieldName, True)
        Else
            Query_Where += " " & clsGeneral.clsGeneral.PrepQueryString("Begins With", CompanyPhoneNumber, "String", False, CompanyFieldName, True)
        End If

        Return Query_Where
    End Function

    Public Function getServiceUsed() As DataTable
        Dim sql As String = ""
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim atemptable As New DataTable
        Try

            sql += "SELECT svud_id, svud_desc FROM " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Services_Used WITH (NOLOCK)"
            sql += " WHERE svud_active_flag = 'Y' AND svud_id not in (25,40,11)"
            sql += " ORDER BY svud_desc ASC"

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, [GetType]().FullName, sql.ToString)

            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

            SqlConn.Open()
            SqlCommand.Connection = SqlConn


            SqlCommand.CommandText = sql
            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 60

            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
            End Try
        Catch ex As Exception
            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message
            Return Nothing
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


End Class