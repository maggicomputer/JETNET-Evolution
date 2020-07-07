Partial Public Class EvoTheme
    Inherits System.Web.UI.MasterPage

    Public aclsData_Temp As clsData_Manager_SQL = Nothing
    Public error_string As String = ""
    Public bEnableChat As Boolean

    Dim TempTable As New DataTable
    Public script_version As String = ""

    Public Sub AddExtraButtons(ByVal ExtraButtonsString As String)
        WelcomeUser1.SetExtraButtons(ExtraButtonsString)
    End Sub

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Dim bHasMaster As Boolean = True
        'HttpContext.Current.Session.Item("localUser").crmUser_DebugText = ""

        If Not IsNothing(Request.Item("noMaster")) Then
            If Not String.IsNullOrEmpty(Request.Item("noMaster").ToString) Then
                bHasMaster = CBool(Request.Item("noMaster").ToString.Trim)
            End If
        End If

        aclsData_Temp = New clsData_Manager_SQL
        aclsData_Temp.JETNET_DB = Session.Item("jetnetClientDatabase")
        aclsData_Temp.client_DB = Session.Item("jetnetServerNotesDatabase")

        Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("")


        SetContainerClass("container MaxWidthRemove") 'set full width page
        'If HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.LOCAL Or HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.TEST Then
        homeLinkButton.InnerHtml = "<a href=""../home_tile.aspx"">Home</a>"
        'End If

        Dim SMgr As ScriptManager
        If ScriptManager.GetCurrent(Page) Is Nothing Then
            Throw New Exception("ScriptManager not found.")
        Else
            SMgr = ScriptManager.GetCurrent(Page)
        End If

        script_version = My.Settings.SCRIPT_VERSION.ToString

        Dim link As HtmlLink = New HtmlLink()
        link.Attributes.Add("type", "text/css")
        link.Attributes.Add("rel", "stylesheet")
        link.Attributes.Add("href", "/EvoStyles/stylesheets/additional_styles.css" + script_version)
        Page.Header.Controls.Add(link)

        Dim link1 As HtmlLink = New HtmlLink()
        link1.Attributes.Add("type", "text/css")
        link1.Attributes.Add("rel", "stylesheet")
        link1.Attributes.Add("href", "/EvoStyles/stylesheets/tableThemes.css" + script_version)
        Page.Header.Controls.Add(link1)

        Dim link2 As HtmlLink = New HtmlLink()
        link2.Attributes.Add("type", "text/css")
        link2.Attributes.Add("rel", "stylesheet")
        link2.Attributes.Add("href", "/EvoStyles/stylesheets/header_styles.css" + script_version)
        Page.Header.Controls.Add(link2)

        Dim SRef As ScriptReference = New ScriptReference()
        SRef.Path = "https://cdnjs.cloudflare.com/ajax/libs/jquery/3.4.1/jquery.min.js"
        SMgr.Scripts.Add(SRef)

        Dim SRef1 As ScriptReference = New ScriptReference()
        SRef1.Path = "https://code.jquery.com/ui/1.12.1/jquery-ui.js"
        SMgr.Scripts.Add(SRef1)

        Dim SRef2 As ScriptReference = New ScriptReference()
        SRef2.Path = "https://cdnjs.cloudflare.com/ajax/libs/jquery-migrate/3.1.0/jquery-migrate.min.js"
        SMgr.Scripts.Add(SRef2)

        Dim SRef3 As ScriptReference = New ScriptReference()
        SRef3.Path = "~/common/jquery.select-to-autocomplete.min.js"
        SMgr.Scripts.Add(SRef3)

        Dim SRef4 As ScriptReference = New ScriptReference()
        SRef4.Path = "~/common/common_functions.js" + script_version
        SMgr.Scripts.Add(SRef4)

        Dim SRef5 As ScriptReference = New ScriptReference()
        SRef5.Path = "~/common/jquery.number.min.js"
        SMgr.Scripts.Add(SRef5)

        Dim SRef6 As ScriptReference = New ScriptReference()
        SRef6.Path = "~/common/date.js"
        SMgr.Scripts.Add(SRef6)

        Dim SRef7 As ScriptReference = New ScriptReference()
        SRef7.Path = "~/common/daterangepicker.jQuery.js" + script_version
        SMgr.Scripts.Add(SRef7)

        Dim SRef8 As ScriptReference = New ScriptReference()
        SRef8.Path = "~/common/header_scripts.js" + script_version
        SMgr.Scripts.Add(SRef8)

        Dim SRef9 As ScriptReference = New ScriptReference()
        SRef9.Path = "~/common/chosen.jquery.min.js"
        SMgr.Scripts.Add(SRef9)

        Dim SRef10 As ScriptReference = New ScriptReference()
        SRef10.Path = "~/common/jQDateRangeSlider-min.js"
        SMgr.Scripts.Add(SRef10)

        Dim SRef11 As ScriptReference = New ScriptReference()
        SRef11.Path = "~/abiFiles/js/superfish.min.js"
        SMgr.Scripts.Add(SRef11)

        Dim SRef12 As ScriptReference = New ScriptReference()
        SRef12.Path = "https://www.gstatic.com/charts/loader.js"
        SMgr.Scripts.Add(SRef12)

        Dim SRef13 = New ScriptReference()
        SRef13.Path = "~/common/jquery.slicknav.min.js"
        SMgr.Scripts.Add(SRef13)

        If bHasMaster And (CBool(My.Settings.enableChat)) Then

            ChatManager.CheckAndInitChat(False, bEnableChat)

            notifyChatUserPanel.Visible = False

            If bEnableChat Or HttpContext.Current.Request.Path.ToString.Trim.ToLower.Contains("home.aspx") Then

                Dim SvcRef As ServiceReference = New ServiceReference()
                SvcRef.Path = "~/chat/Services/chatServices.svc"

                SMgr.Services.Add(SvcRef)

                notifyChatUserPanel.Visible = True

            End If
        End If

        If Session.Item("crmUserLogon") <> True Then
            Response.Redirect("Default.aspx", False)
        Else
            If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO Then
                googleAnalyticsScript.Visible = True
                ShowHelpfulHint()
                setUpHotJar() 'This enables the hotjar script on evo.

            End If
            If Session.Item("localUser").crmEvo = True Then
                'If Not Page.IsPostBack Then
                FillUpMenuItems()
                'End If
            End If
        End If
    End Sub
    Public Sub RemoveLine()
    End Sub


    Private Sub setUpHotJar()
        If HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.LOCAL Or HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.TEST Then
            hotjarScriptTestEvo.Visible = True 'This enables the hotjar script on evo.
            hotjarScriptLiveEvo.Visible = False
        Else
            hotjarScriptLiveEvo.Visible = True
            hotjarScriptTestEvo.Visible = False
        End If
    End Sub
    Public Sub ShowHelpfulHint()
        Dim pageName As String = ""
        Dim ShowView As Boolean = False
        Dim ViewIDStr As String = ""
        DisplayFunctions.ShowHelpfulHintPage(pageName, ShowView, ViewIDStr) 'Figures out the parameters based on what page you're on.
        If Not String.IsNullOrEmpty(pageName) Or ShowView = True Then
            DisplayFunctions.DisplayHelpfulHint(pageName, ShowView, ViewIDStr, hintText, aclsData_Temp, hintPopupHolder, hintTextUpdate) 'Actually displays the hint.
        End If
    End Sub

    Public Sub SetFormClass(ByVal className As String)
        aspnetForm.Attributes.Add("class", className)
    End Sub
    Public Sub SetContainerClass(ByVal ClassName As String)
        pageSizing.Attributes.Remove("class")
        pageSizing.Attributes.Add("class", "container MaxWidthRemove")
    End Sub
    'Filling up new menu items
    Private Sub FillUpMenuItems()
        'querying to display the view information for the menu item for Views
        If Not IsNothing(Session.Item("ViewDataTable")) Then
            TempTable = Session.Item("ViewDataTable")
        Else
            TempTable = aclsData_Temp.Display_View_Information(0)
            Session.Item("ViewDataTable") = TempTable
        End If
        Dim separateWindow As Boolean = True

        'Menu1.Items(1).ChildItems.Clear()
        If Not IsNothing(TempTable) Then
            If TempTable.Rows.Count > 0 Then
                For Each r As DataRow In TempTable.Rows
                    If IsNumeric(r("evoview_id")) Then
                        'If r("evoview_id") <> 27 Then
                        If CInt(r.Item("evoview_id").ToString) = 27 And HttpContext.Current.Session.Item("localPreferences").UserSPIViewFlag = False Then
                            'Do not add
                        ElseIf CInt(r.Item("evoview_id").ToString) = 24 And HttpContext.Current.Session.Item("localPreferences").AerodexStandard = True Then
                            ' do not add
                            ' ElseIf (CInt(r.Item("evoview_id").ToString) = 31) And Application.Item("crmClientSiteData").WebSiteType = eWebSiteTypes.LIVE Then
                            '   ' do not add
                        Else
                            'Dim TempMenu As New MenuItem
                            'TempMenu.Text = r("evoview_title").ToString
                            'TempMenu.Value = r("evoview_id").ToString
                            'Menu1.Items(1).ChildItems.Add(TempMenu)


                            If IsNothing(bulletedContainer.FindControl(Replace(r("evoview_Heading"), " ", ""))) Then
                                'Go ahead and add it to the container
                                Dim NewBulletedList As New BulletedList

                                If separateWindow Then
                                    NewBulletedList.DisplayMode = BulletedListDisplayMode.HyperLink
                                    NewBulletedList.Target = "_blank"
                                Else
                                    NewBulletedList.DisplayMode = BulletedListDisplayMode.LinkButton
                                End If


                                NewBulletedList.ID = Replace(r("evoview_heading"), " ", "")

                                If Session.Item("localSubscription").crmAerodexFlag = False Then
                                    NewBulletedList.Items.Add(New ListItem(r("evoview_heading"), "")) 'First list item with just header
                                ElseIf Session.Item("localSubscription").crmAerodexFlag = True Then
                                    NewBulletedList.Items.Add(New ListItem(Replace(UCase(r("evoview_heading")), "MARKET INSIGHT", "MODEL INSIGHT"), "")) 'First list item with just header
                                End If

                                Dim liAdd As New ListItem

                                liAdd.Text = r("evoview_title").ToString
                                buildLIItem(liAdd, r, separateWindow)

                                If separateWindow = False Then
                                    liAdd.Value = r("evoview_id").ToString
                                    AddHandler NewBulletedList.Click, AddressOf RedirectFromViewClick
                                End If

                                NewBulletedList.Items.Add(liAdd)

                                bulletedContainer.Controls.Add(NewBulletedList)

                            Else 'it's already in the container, so let's go ahead and just add the item there.
                                Dim TemporaryHold As BulletedList = CType(bulletedContainer.FindControl(Replace(r("evoview_Heading"), " ", "")), BulletedList)

                                If separateWindow Then
                                    TemporaryHold.DisplayMode = BulletedListDisplayMode.HyperLink
                                    TemporaryHold.Target = "_blank"
                                Else
                                    TemporaryHold.DisplayMode = BulletedListDisplayMode.LinkButton
                                End If


                                Dim liItem As New ListItem
                                liItem.Text = r("evoview_title").ToString
                                buildLIItem(liItem, r, separateWindow)

                                If separateWindow = False Then
                                    liItem.Value = r("evoview_id").ToString
                                    AddHandler TemporaryHold.Click, AddressOf RedirectFromViewClick

                                    ' changed for now -- MSW 
                                    If CInt(r.Item("evoview_id").ToString) = 24 Then
                                        liItem.Value = 28
                                    Else
                                        liItem.Value = r("evoview_id").ToString
                                    End If
                                End If


                                TemporaryHold.Items.Add(liItem)

                            End If

                        End If

                    End If
                Next

                '  If (HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.TEST Or HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.LOCAL) Then
                'if we are aerodex standard do now show this view - added MSW - 9/21/18
                If HttpContext.Current.Session("localSubscription").crmAerodexFlag = True And HttpContext.Current.Session.Item("localPreferences").AerodexStandard = True Then
                ElseIf Not IsNothing(bulletedContainer.FindControl("OPERATIONS")) Then
                    Dim TemporaryHold As BulletedList = CType(bulletedContainer.FindControl("OPERATIONS"), BulletedList)
                    Dim liItem As New ListItem


                    liItem.Text = "Route Analysis"

                    If separateWindow Then
                        TemporaryHold.DisplayMode = BulletedListDisplayMode.HyperLink
                        TemporaryHold.Target = "_blank"
                        liItem.Value = "/FAAFlightData.aspx?analysis=true"
                    Else
                        AddHandler TemporaryHold.Click, AddressOf RedirectFromViewClick
                        liItem.Attributes.Add("onclick", "javascript:load('/FAAFlightData.aspx?analysis=true','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');return false;")
                        TemporaryHold.DisplayMode = BulletedListDisplayMode.LinkButton
                        liItem.Value = "1000"
                    End If
                    TemporaryHold.Items.Add(liItem)
                End If

                ' If (HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.TEST Or HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.LOCAL) Then
                If Not IsNothing(bulletedContainer.FindControl("TOOLS")) Then
                    If clsGeneral.clsGeneral.isCrmDisplayMode = True Then
                        Dim TemporaryHold As BulletedList = CType(bulletedContainer.FindControl("TOOLS"), BulletedList)
                        Dim liAdd As New ListItem

                        liAdd.Text = "Prospect Management"
                        If separateWindow Then
                            TemporaryHold.DisplayMode = BulletedListDisplayMode.HyperLink
                            TemporaryHold.Target = "_blank"
                            liAdd.Value = "/view_template.aspx?ViewID=18&ViewName=Prospect Management&noMaster=false"
                        Else
                            AddHandler TemporaryHold.Click, AddressOf RedirectFromViewClick
                            If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Or Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
                                liAdd.Attributes.Add("onclick", "window.open('/view_template.aspx?ViewID=18&ViewName=Prospect Management&noMaster=false','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');return false;")
                            Else
                                liAdd.Attributes.Add("onclick", "javascript:load('/view_template.aspx?ViewID=18&ViewName=Prospect Management&noMaster=false','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');return false;")
                            End If
                            TemporaryHold.DisplayMode = BulletedListDisplayMode.LinkButton
                            liAdd.Value = "18"
                        End If


                        TemporaryHold.Items.Add(liAdd)

                    End If
                End If
            End If
            'End If
        Else
            LogError(aclsData_Temp.class_error)
        End If
    End Sub

    Public Sub buildLIItem(ByRef liItem As ListItem, ByVal r As DataRow, separateWindow As Boolean)
        If r("evoview_id") = 27 Then
            If separateWindow = True Then
                liItem.Value = "/view_template.aspx?ViewID=27&ViewName=Value"
            Else
                liItem.Attributes.Add("onclick", "javascript:load('/view_template.aspx?ViewID=27&ViewName=Value','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');return false;")
            End If

        ElseIf r("evoview_id") = 21 Or r("evoview_id") = 20 Or r("evoview_id") = 17 Or r("evoview_id") = 22 Or r("evoview_id") = 23 Then
            If separateWindow = True Then
                liItem.Value = "/Yacht_View_Template.aspx?ViewID=" & r("evoview_id").ToString & "&ViewName=" & r("evoview_title").ToString & "&noMaster=false"
            Else
                liItem.Attributes.Add("onclick", "javascript:load('/Yacht_View_Template.aspx?ViewID=" & r("evoview_id").ToString & "&ViewName=" & r("evoview_title").ToString & "&noMaster=false','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');return false;")
            End If
        ElseIf r("evoview_id") = 29 Then
            If separateWindow = True Then
                liItem.Value = "/aircraftFinder.aspx"
            Else
                liItem.Attributes.Add("onclick", "javascript:load('aircraftFinder.aspx','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');return false;")
            End If
        ElseIf r("evoview_id") = 30 Then
            liItem.Text = "Fleet Analyzer"
            If separateWindow = True Then
                liItem.Value = "/userPortfolio.aspx"
            Else
                liItem.Attributes.Add("onclick", "javascript:load('userPortfolio.aspx','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');return false;")
            End If

        ElseIf r("evoview_id") = 31 Then
            If separateWindow = True Then
                liItem.Value = "/resdiualMarketValue.aspx"
            Else
                liItem.Attributes.Add("onclick", "javascript:load('resdiualMarketValue.aspx','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');return false;")
            End If
        Else
            If separateWindow = True Then
                liItem.Value = "/view_template.aspx?ViewID=" & r("evoview_id").ToString & "&ViewName=" & r("evoview_title").ToString & "&noMaster=false"
            Else
                liItem.Attributes.Add("onclick", "javascript:load('/view_template.aspx?ViewID=" & r("evoview_id").ToString & "&ViewName=" & r("evoview_title").ToString & "&noMaster=false','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');return false;")
            End If
        End If
    End Sub

    Public Sub UpdateHelpLink(ByVal url As String)
        WelcomeUser1.ChangeHelpLink(url)
    End Sub
    Protected Sub Page_load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If Session.Item("crmUserLogon") <> True Then
            Response.Redirect("Default.aspx", False)
        Else
            If clsGeneral.clsGeneral.CheckForBotActivity(aclsData_Temp, Page.IsPostBack) Then
                Trace.Write("Start Page_load EvoTheme.Master.vb" + Now.ToString)



                If Not Page.IsPostBack Then

                    If Not IsNothing(Trim(Request("useFAAFlightData"))) Then
                        If Not String.IsNullOrEmpty(Trim(Request("useFAAFlightData"))) Then
                            Session.Item("useFAAFlightData") = Trim(Request("useFAAFlightData"))
                        End If
                    End If
                    Session.Item("localUser").crmUser_DebugText += "<br /><br />Master Page First Load: " & Now.ToString
                Else
                    Session.Item("localUser").crmUser_DebugText += "<br /><br />Master Page Post Back: " & Now.ToString
                End If

                'Clear this Automatic Refresh Time Integer: This clears every time the masterpage is hit in a refresh.
                Session.Item("AutomaticRefreshTime") = 0

                If Session.Item("localUser").crmEvo = True Then

                    If Not Page.IsPostBack Then
                        WelcomeUser1.CheckDataAge()


                        If Session.Item("localSubscription").crmAerodexFlag Then
                            'If Not IsNothing(Menu1.FindItem("Market Summary")) Then
                            '  Menu1.Items.Remove(Menu1.FindItem("Market Summary"))
                            'End If
                            marketLinkButton.Visible = False
                            wantedLinkButton.Visible = False
                            'If Not IsNothing(Menu1.FindItem("Wanted")) Then
                            '  Menu1.Items.Remove(Menu1.FindItem("Wanted"))
                            'End If
                        End If
                    End If



                    If Session.Item("localSubscription").crmYacht_Flag = True Then
                    Else
                        If Not Page.IsPostBack Then
                            'Menu1.Items(Menu1.Items.Count - 1).ChildItems.RemoveAt(12)
                            yachtGlossaryLinkButton.Visible = False
                        End If
                    End If

                End If

                'Configures background
                'Configure_Background()

                'If Session.Item("localSubscription").crmYacht_Flag = True Then
                '  If HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.LOCAL Or HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.TEST Then
                '  Else
                '    If Not Page.IsPostBack Then
                '      Menu1.Items(Menu1.Items.Count - 1).ChildItems.RemoveAt(12)
                '      Menu1.Items(Menu1.Items.Count - 1).ChildItems.RemoveAt(11)
                '    End If
                '  End If
                'Else ' there is no yacht flag, so get rid of either way
                '  If Not Page.IsPostBack Then
                '    Menu1.Items(Menu1.Items.Count - 1).ChildItems.RemoveAt(12)
                '  End If




                If Not Page.IsPostBack Then

                    If HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.LOCAL Or HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.TEST Then
                        If Session.Item("localUser").crmUserType = eUserTypes.ADMINISTRATOR And Session.Item("localUser").crmAllowExport_Flag = True Then
                            'Dim note_export As New MenuItem
                            'note_export.Value = "Note Export"
                            'note_export.Text = "Note Export"
                            'note_export.NavigateUrl = "javascript:load('help.aspx?t=1&export=Y','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');"

                            'Menu1.Items(Menu1.Items.Count - 1).ChildItems.Add(note_export)
                            noteExportLinkButton.Visible = True
                        End If
                    End If

                    If Session.Item("localUser").crmLocalUserEmailAddress.ToString.ToLower.Contains("@jetnet.com") Or Session.Item("localUser").crmLocalUserEmailAddress.ToString.ToLower.Contains("@mvintech.com") Then
                        'If HttpContext.Current.Session.Item("localPreferences").UserSPIViewFlag Then
                        '  Dim ValMenu As New MenuItem
                        '  ValMenu.Text = "Values"
                        '  ValMenu.NavigateUrl = "javascript:load('/view_template.aspx?ViewID=27&ViewName=Value','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');"
                        '  Menu1.Items(1).ChildItems.Add(ValMenu)
                        'End If
                        debugMenuLinkButton.Visible = True
                        'Dim debugItem As New MenuItem
                        'debugItem.Value = "Debug Menu"
                        'debugItem.Text = "Debug Menu"
                        'debugItem.NavigateUrl = "javascript:load('../Debug.aspx','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');"
                        'Menu1.Items(Menu1.Items.Count - 1).ChildItems.Add(debugItem)
                    End If

                    projectLinkButton.Visible = True


                    'Dim reportconverter As New MenuItem
                    'reportconverter.Value = "Project Conversion"
                    'reportconverter.Text = "Project Conversion"
                    'reportconverter.NavigateUrl = "javascript:load('../evoprojectconversion.aspx','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');"

                    'Menu1.Items(Menu1.Items.Count - 1).ChildItems.Add(reportconverter)
                    'End If
                End If

            Else 'Check for Bot Activity returned false, ship them to user verification

                Response.Redirect("UserVerification.aspx")
            End If

        End If

        Trace.Write("End Page_load EvoTheme.Master.vb" + Now.ToString)
        'Menu1.Visible = False
    End Sub
    Public Function SearchPanelVisible(ByVal visible As Boolean) As String
        WelcomeUser1.searchPanelSlideOut.Visible = visible
        Return WelcomeUser1.searchPanelSlideOut.ClientID
    End Function
    Public Sub SetDefaultButtion(ByVal buttonID As String)
        aspnetForm.DefaultButton = buttonID
    End Sub
    '' <summary>
    '' Configures background. With an ID of 0, it means they have a random picture background. This means
    '' they need to look up the background (if they haven't already). So we look it up, then store it and update the picture
    '' with the new background. Query isn't run on postback but update of picture is.
    '' </summary>
    '' <remarks></remarks>
    'Private Sub Configure_Background()
    '    Trace.Write("Start Configure_Background EvoTheme.Master.vb" + Now.ToString)

    '    If Not IsNothing(Session.Item("localUser").crmLocalUser_Background_ID) Then
    '        If Session.Item("localUser").crmLocalUser_Background_ID = 0 Then 'this means the background is random, this needs to be looked up once, and then set.
    '            If Not Page.IsPostBack Then
    '                TempTable = aclsData_Temp.GetBackgroundImages()
    '                If Not IsNothing(TempTable) Then
    '                    If TempTable.Rows.Count > 0 Then
    '                        Session.Item("localUser").crmLocalUser_Background_ID = TempTable.Rows(0).Item("evoback_id")
    '                    End If
    '                End If
    '            End If
    '        End If
    '    End If
    '    If Not IsNothing(Session.Item("localUser").crmLocalUser_Background_ID) Then
    '        If Session.Item("localUser").crmLocalUser_Background_ID <> 0 Then
    '            background_image.Text = "<img src=""images/background/" + Session.Item("localUser").crmLocalUser_Background_ID.ToString + ".jpg"" alt="""" class=""bg_image"" />"
    '        End If
    '    End If
    '    Trace.Write("End Configure_Background EvoTheme.Master.vb" + Now.ToString)

    'End Sub

    Public Sub Set_Active_Tab(ByVal active_tabIndex As Long)
        'Menu1.Items(active_tabIndex).Selected = True
        Select Case active_tabIndex
            Case 0
                homeLinkButton.Attributes.Add("class", "active")
            Case 1
                viewLinkButton.Attributes.Add("class", "active")
            Case 2
                acLinkButton.Attributes.Add("class", "active")
            Case 3
                historyLinkButton.Attributes.Add("class", "active")
            Case 4
                companyLinkButton.Attributes.Add("class", "active")
            Case 5
                perfLinkButton.Attributes.Add("class", "active")
            Case 6
                opLinkButton.Attributes.Add("class", "active")
            Case 7
                eventsLinkButton.Attributes.Add("class", "active")
            Case 8
                marketLinkButton.Attributes.Add("class", "active")
            Case 9
                wantedLinkButton.Attributes.Add("class", "active")
        End Select
    End Sub
    Public Sub SetStatusText(ByVal text As String, Optional ByRef aircraftSearch As Boolean = False)
        WelcomeUser1.SetStatusText(text, aircraftSearch)
    End Sub
    Public Function ReturnStatusText() As String
        Return WelcomeUser1.evo_message_text.Text
    End Function
    ''' <summary>
    ''' Just a temporary function to remove status notification
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub RemoveStatusNotification()
        Response.Write("this closed the notification")
    End Sub
    ''' <summary>
    ''' This is blank until the logging is put in
    ''' </summary>
    ''' <param name="ex"></param>
    ''' <remarks></remarks>
    Public Sub LogError(ByVal ex As String)
        'Error Logging Function
        Call commonLogFunctions.Log_User_Event_Data("UserError", Replace(Request.Url.AbsolutePath, "/", "") & " : " & Replace(ex, "'", "''"), Nothing, 0, 0, 0, 0, 0)
    End Sub
    ''' <summary>
    ''' Fills up Gray Menu Bar with a title text
    ''' </summary>
    ''' <param name="title_string">Acceptable title</param>
    ''' <remarks></remarks>
    Public Sub Gray_Title_Bar(ByVal title_string As String)
        gray_menu_title.Text = title_string
    End Sub
    ''' <summary>
    ''' Fills up Gray Menu Bar Buttons
    ''' </summary>
    ''' <param name="title_string"></param>
    ''' <remarks></remarks>
    Public Sub Gray_Title_Buttons(ByVal title_string As String)
        gray_menu_buttons.Text = title_string
    End Sub
    ''' <summary>
    ''' Needs true if the view is being viewed in the main template, false if a standalone.
    ''' </summary>
    ''' <param name="visibility"></param>
    ''' <remarks></remarks>
    Public Sub MenuBarVisibility(ByVal visibility As Boolean)
        ' WelcomeUser1.Visible = visibility
        ' Menu1.Visible = visibility
        replaceMenu.Visible = visibility
        gray_bar_container.Visible = visibility
        'WelcomeUser1.ToggleLogoutButton(visibility)
        WelcomeUser1.ToggleStandalone(visibility)
    End Sub
    Public Sub ToggleWelcomeHeader(ByVal visibility As Boolean)
        WelcomeUser1.Visible = visibility
        fixedBar.Visible = visibility
    End Sub
    Public Sub RemoveWhiteBackground(ByVal remove As Boolean)
        If remove = True Then
            container_white_background_div.Attributes("class") = "sixteen columns"
            container_border.Attributes("class") = ""
            content_clear.Attributes("class") = "display_none"
        End If
    End Sub
    Public Sub ChangeWhiteBackground(ClassName As String)
        container_white_background_div.Attributes("class") = ClassName
    End Sub
    Public Sub ToggleWelcomeMessage(ByVal visible As Boolean)
        WelcomeUser1.ToggleWelcomeMessage(visible)
    End Sub

    Private Sub RedirectFromViewClick(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.BulletedListEventArgs)
        Dim selectedLI As New ListItem
        Dim ViewID As Long = 0
        Dim ViewName As String = ""
        selectedLI = sender.Items(e.Index)

        ViewID = selectedLI.Value
        ViewName = selectedLI.Text

        If ViewID.ToString = "28" Or ViewID = "18" Then
            'Put in so View 28 never remembers it's aircraft information if you click on it from the menu.
            '4/6/2017
            HttpContext.Current.Session.Item("hasModelFilter") = False
            HttpContext.Current.Session.Item("hasHelicopterFilter") = False
            HttpContext.Current.Session.Item("hasBusinessFilter") = False
            HttpContext.Current.Session.Item("hasCommercialFilter") = False

            HttpContext.Current.Session.Item("viewAircraftModel") = ""
            HttpContext.Current.Session.Item("viewAircraftMake") = ""
            HttpContext.Current.Session.Item("viewAircraftType") = ""
        End If

        If ViewID = 21 Or ViewID = 20 Or ViewID = 17 Or ViewID = 22 Or ViewID = 23 Then
            Response.Redirect("Yacht_View_Template.aspx?ViewID=" + ViewID.ToString + "&ViewName=" + ViewName)
        ElseIf ViewID = 27 Then

        Else
            Response.Redirect("view_template.aspx?ViewID=" + ViewID.ToString + "&ViewName=" + ViewName)
        End If


    End Sub

    Private Sub hintUpdateButton_Click(sender As Object, e As EventArgs) Handles hintUpdateButton.Click
        'hintTextUpdate.Text = hintTextUpdate.Text
        Dim popupDateInsert As String = Format(Now(), "MM/dd/yyyy")
        Dim subID As Long = 0
        Dim login As String = ""
        Dim seqNo As Long = 0

        If Not IsNothing(HttpContext.Current.Session.Item("localUser").crmSubSubID) Then
            If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString.Trim) Then
                If IsNumeric(HttpContext.Current.Session.Item("localUser").crmSubSubID) Then
                    subID = CLng(HttpContext.Current.Session.Item("localUser").crmSubSubID)
                End If
            End If
        End If

        If Not IsNothing(HttpContext.Current.Session.Item("localUser").crmUserLogin) Then
            If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("localUser").crmUserLogin.Trim) Then
                login = HttpContext.Current.Session.Item("localUser").crmUserLogin.Trim
            End If
        End If

        If Not IsNothing(HttpContext.Current.Session.Item("localUser").crmSubSeqNo) Then
            If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("localUser").crmSubSeqNo.ToString.Trim) Then
                If IsNumeric(HttpContext.Current.Session.Item("localUser").crmSubSeqNo) Then
                    seqNo = CLng(HttpContext.Current.Session.Item("localUser").crmSubSeqNo)
                End If
            End If
        End If

        aclsData_Temp.InsertIntoSubscriptionNotifications(subID, login, seqNo, hintTextUpdate.Text, popupDateInsert, "R")

    End Sub

    Private Sub hintUpdateClickLink_Click(sender As Object, e As EventArgs) Handles hintUpdateClickLink.Click
        'hintTextUpdate.Text = hintTextUpdate.Text
        Dim popupDateInsert As String = Format(Now(), "MM/dd/yyyy")
        Dim subID As Long = 0
        Dim login As String = ""
        Dim seqNo As Long = 0

        If Not IsNothing(HttpContext.Current.Session.Item("localUser").crmSubSubID) Then
            If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString.Trim) Then
                If IsNumeric(HttpContext.Current.Session.Item("localUser").crmSubSubID) Then
                    subID = CLng(HttpContext.Current.Session.Item("localUser").crmSubSubID)
                End If
            End If
        End If

        If Not IsNothing(HttpContext.Current.Session.Item("localUser").crmUserLogin) Then
            If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("localUser").crmUserLogin.Trim) Then
                login = HttpContext.Current.Session.Item("localUser").crmUserLogin.Trim
            End If
        End If

        If Not IsNothing(HttpContext.Current.Session.Item("localUser").crmSubSeqNo) Then
            If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("localUser").crmSubSeqNo.ToString.Trim) Then
                If IsNumeric(HttpContext.Current.Session.Item("localUser").crmSubSeqNo) Then
                    seqNo = CLng(HttpContext.Current.Session.Item("localUser").crmSubSeqNo)
                End If
            End If
        End If

        aclsData_Temp.InsertIntoSubscriptionNotifications(subID, login, seqNo, hintTextUpdate.Text, popupDateInsert, "C")

    End Sub

End Class