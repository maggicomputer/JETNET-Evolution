Partial Public Class Yacht_Criteria_Bar
    Inherits System.Web.UI.UserControl
    Public Event AlterListing(ByVal typeID As Integer, ByVal recordAmount As Integer)
    Public Event Yacht_Search(ByVal model_string As String, ByVal ForSale_Flag As String, ByVal ForLease_Flag As String, ByVal OnExclusive_Flag As String, ByVal SerialNo_Start As String, ByVal SerialNo_End As String, ByVal RegistrationNo As String, ByVal registration_number_exact_match As String, ByVal do_not_search_prev_reg As String, ByVal LifeCycleStage_String As String, ByVal Status As String, ByVal Ownership_String As String, ByVal PreviouslyOwned_Flag As String, ByVal model_type_string As String, ByVal make_string As String, ByVal Brand As String, ByVal YearString As String, ByVal CategorySizeString As String, ByVal MotorSizeString As String, ByVal PageNumber As Integer, ByVal PageSort As String)
    Public Event PageData(ByVal next_val As Boolean, ByVal previous_val As Boolean, ByVal next_all_val As Boolean, ByVal previous_all_val As Boolean, ByVal goToPage As Boolean, ByVal PageInt As Integer)
    Public WithEvents type As Global.System.Web.UI.WebControls.ListBox
    Public WithEvents make As Global.System.Web.UI.WebControls.ListBox
    Public WithEvents model As Global.System.Web.UI.WebControls.ListBox
    Public WithEvents criteria_results As Global.System.Web.UI.WebControls.Label
    Public WithEvents record_count As Global.System.Web.UI.WebControls.Label
    Public WithEvents PanelCollapseEx As Global.AjaxControlToolkit.CollapsiblePanelExtender
    Dim TempTable As New DataTable
    Dim TypeDataTable As New DataTable
    Dim TypeDataHold As New DataTable
    Dim masterPage As New YachtTheme
    Dim PageNumber As Integer = 1
    Dim PageSort As String = ""
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session.Item("crmUserLogon") <> True Then
            Response.Redirect("Default.aspx", False)
        Else
            Dim TempRoundedPagingCount As Integer = 0
            Dim ModelsString As String = ""
            Dim BrandString As String = ""

            masterPage = DirectCast(Page.Master, YachtTheme)

            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            'toggle aerodex
            If Session.Item("localSubscription").crmAerodexFlag = True Then
                aerodex_toggle.Visible = False
            End If
            'Setting up the type listbox set
            If Not Page.IsPostBack Then

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                'TempRoundedPagingCount = Math.Round(Session.Item("localUser").crmUserRecsPerPage / 10.0) * 10
                ''Making sure 100 is max (for now at least)
                'If TempRoundedPagingCount > 100 Then
                '    TempRoundedPagingCount = 100
                'End If
                ''Temporarily restting the session variable for this session.
                'Session.Item("localUser").crmUserRecsPerPage = TempRoundedPagingCount
                ''Let's figure out the paging situation, what should it be defaulted to?
                'per_page_dropdown.Items.Clear()
                'per_page_dropdown.Items.Add(New ListItem(TempRoundedPagingCount, TempRoundedPagingCount))
                If Collapse_Panel.Visible = True Then ' This means that the search panel is visible.

                    'Let's fill out the Yacht Brands.
                    YachtFunctions.Display_Yacht_Brand_In_Listbox(masterPage, brand)

                    'let's fill out the category size
                    YachtFunctions.Display_Yacht_Category_In_Dropdown(masterPage, yacht_category)

                    'Fill yacht year
                    clsGeneral.clsGeneral.Year_Range_DropDownFill(yacht_year, 1960, 2015)

                    If Page.Request.Form("yt_lifecycle_id") <> "" Then
                        lifecycle.SelectedValue = Request.Form("yt_lifecycle_id")
                    End If

                    If Page.Request.Form("ym_category_size") <> "" Then
                        yacht_category.SelectedValue = Request.Form("ym_category_size")
                    End If

                    If Page.Request.Form("ym_brand_name") <> "" Then
                        brand.SelectedValue = Request.Form("ym_brand_name")
                    End If

                    If Page.Request.Form("yt_year_mfr") <> "" Then
                        yacht_year.SelectedValue = Request.Form("yt_year_mfr")
                    End If

                    If Page.Request.Form("for_sale") = "Y" Then
                        market.SelectedValue = "For Sale"
                    End If

                    If Page.Request.Form("complete_search") = "Y" Then
                        search_Click(search, EventArgs.Empty)
                    End If
                End If
            Else
                'If sort_by_dropdown.Visible = True Then
                '    SetPageSort(sort_dropdown.Items(0).Text)
                'End If
                'If go_to_dropdown.Visible = True Then
                '    SetPageNumber(CInt(go_to_dropdown.Items(0).Text))
                'End If
            End If

        End If
    End Sub

    ''' <summary>
    ''' This runs on the initial load of the page. It'll toggle off some of the paging elements and things we don't need displayed if we're first coming into the page.
    ''' </summary>
    ''' <param name="initial_page_load"></param>
    ''' <remarks></remarks>
    Public Sub Initial(ByVal initial_page_load As Boolean)
        If initial_page_load = True Then
            criteria_results.Visible = False
            sort_by_text.Visible = False
            sort_by_dropdown.Visible = False
            view_dropdown.Visible = False
            actions_dropdown.Visible = False
            paging.Visible = False
            PanelCollapseEx.Collapsed = False
            PanelCollapseEx.ClientState = False
        Else
            PanelCollapseEx.Collapsed = True
            PanelCollapseEx.ClientState = True
            criteria_results.Visible = True
            sort_by_text.Visible = True
            sort_by_dropdown.Visible = True
            view_dropdown.Visible = True
            actions_dropdown.Visible = True
            paging.Visible = True
        End If
    End Sub
    ''' <summary>
    ''' Toggles the bar whether it's the high bar or the low bar. This sets up the javascript for the bulleted lists as well.
    ''' </summary>
    ''' <param name="lower_bar"></param>
    ''' <remarks></remarks>
    Public Sub ToggleHigherLowerBar(ByVal lower_bar As Boolean)
        'setting the javascript of the menus
        'sort dropdown
        sort_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & sort_submenu_dropdown.ClientID & "', true);")
        sort_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & sort_submenu_dropdown.ClientID & "', false);")

        sort_submenu_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & sort_submenu_dropdown.ClientID & "', true);")
        sort_submenu_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & sort_submenu_dropdown.ClientID & "', false);")

        'page dropdown
        per_page_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & per_page_submenu_dropdown.ClientID & "', true);")
        per_page_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & per_page_submenu_dropdown.ClientID & "', false);")

        per_page_submenu_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & per_page_submenu_dropdown.ClientID & "', true);")
        per_page_submenu_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & per_page_submenu_dropdown.ClientID & "', false);")

        'go to dropdown
        go_to_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & go_to_submenu_dropdown.ClientID & "', true);")
        go_to_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & go_to_submenu_dropdown.ClientID & "', false);")

        go_to_submenu_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & go_to_submenu_dropdown.ClientID & "', true);")
        go_to_submenu_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & go_to_submenu_dropdown.ClientID & "', false);")


        'view dropdown
        view_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & view_submenu_dropdown.ClientID & "', true);")
        view_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & view_submenu_dropdown.ClientID & "', false);")

        view_submenu_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & view_submenu_dropdown.ClientID & "', true);")
        view_submenu_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & view_submenu_dropdown.ClientID & "', false);")

        'actions dropdown
        actions_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & actions_submenu_dropdown.ClientID & "', true);")
        actions_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & actions_submenu_dropdown.ClientID & "', false);")

        actions_submenu_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & actions_submenu_dropdown.ClientID & "', true);")
        actions_submenu_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & actions_submenu_dropdown.ClientID & "', false);")

        If lower_bar = True Then
            PanelCollapseEx.Enabled = False
            Collapse_Panel.Visible = False
            search_expand_text.Visible = False
            help_text.Visible = False
            sort_by_text.Visible = False
            sort_by_dropdown.Visible = False
            view_dropdown_.Visible = False

        Else
            per_page_dropdown_.Visible = False
            per_page_text.Visible = False
            go_to_dropdown_.Visible = False
            go_to_text.Visible = False
        End If
    End Sub
    Public Sub SetPageSort(Optional ByVal selectedLI As String = "")
        'Select Case selectedLI
        '    Case "List Date"
        '        PageSort = " ac_list_date"
        '    Case "AFTT"
        '        PageSort = " ac_airframe_tot_hrs" ', ac_engine_1_tot_hrs, ac_engine_2_tot_hrs, ac_engine_3_tot_hrs, ac_engine_4_tot_hrs"
        '    Case "Status"
        '        PageSort = " ac_status"
        '    Case Else
        '        PageSort = " amod_make_name, amod_model_name, ac_ser_no_sort"
        'End Select
    End Sub
    Public Sub SetPageNumber(Optional ByVal selectedLI As Integer = 0)
        PageNumber = selectedLI
    End Sub
    ''' <summary>
    ''' Small function to swap classes of listing view dropdown on ac listing page.
    ''' </summary>
    ''' <param name="showtype"></param>
    ''' <remarks></remarks>
    Sub SwitchGalleryListing(ByVal showtype As Integer)
        Select Case showtype
            Case 0
                view_dropdown.Items.Clear()
                view_dropdown.Items.Add(New ListItem("", ""))
                view_dropdown.CssClass = "ul_top listing_view_bullet"
                RaiseEvent AlterListing(0, 0)
                Session.Item("localUser").crmACListingView = eListingView.LISTING
            Case 1
                view_dropdown.Items.Clear()
                view_dropdown.Items.Add(New ListItem("", ""))
                view_dropdown.CssClass = "ul_top thumnail_view_bullet"
                RaiseEvent AlterListing(1, 0)
                Session.Item("localUser").crmACListingView = eListingView.GALLERY
        End Select

    End Sub

    ''' <summary>
    ''' Click part of the dropdown list, switch the submenu bullet with the main bullet
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Sub submenu_dropdown_Click(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.BulletedListEventArgs)
        Dim selectedLI As New ListItem
        selectedLI = sender.Items(e.Index)
        If sender.id.ToString = "sort_submenu_dropdown" Then
            sort_dropdown.Items.Clear()
            sort_dropdown.Items.Add(New ListItem(selectedLI.Text, ""))
            SetPageSort(selectedLI.Text)
            'search_Click(search, EventArgs.Empty)
        ElseIf sender.id.ToString = "view_submenu_dropdown" Then
            SwitchGalleryListing(e.Index)
            search_Click(search, EventArgs.Empty)
        ElseIf sender.id.ToString = "go_to_submenu_dropdown" Then
            go_to_dropdown.Items.Clear()
            go_to_dropdown.Items.Add(New ListItem(selectedLI.Text, ""))
            'PageNumber = CInt(selectedLI.Text)
            SetPageNumber(CInt(selectedLI.Text))
            RaiseEvent PageData(False, False, False, False, True, PageNumber)
            'search_Click(search, EventArgs.Empty)
        ElseIf sender.id.ToString = "per_page_submenu_dropdown" Then
            per_page_dropdown.Items.Clear()
            per_page_dropdown.Items.Add(New ListItem(selectedLI.Text & " ", selectedLI.Text))
            Session.Item("localUser").crmUserRecsPerPage = CInt(selectedLI.Value)
            RaiseEvent PageData(False, False, False, False, False, PageNumber)
            'ElseIf sender.id.ToString = "actions_submenu_dropdown" Then
            '    Select Case selectedLI.Text
            '        Case "Export/Report"
            '            ' Response.Write("test")
            '            Response.Redirect("evo_exporter.aspx")
            '    End Select
        End If
    End Sub
    Public Sub SetPagingButtons(ByVal back_page As Boolean, ByVal next_page As Boolean)
        previous_all.Visible = back_page
        previous.Visible = back_page

        next_all.Visible = next_page
        next_.Visible = next_page

    End Sub


    ''' <summary>
    ''' Runs and calls an event on search click. This calls an event which is then handled by the main.aspx page.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub search_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles search.Click
        Dim ForSale_Flag As String = ""
        Dim ForLease_Flag As String = ""
        Dim OnExclusive_Flag As String = ""
        Dim SerialNo_Start As String = ""
        Dim SerialNo_End As String = ""
        Dim RegistrationNo As String = ""
        Dim LifeCycleStage_String As String = ""
        Dim MotorType_String As String = ""
        Dim CategorySize_String As String = ""
        Dim Status As String = ""
        Dim Ownership_String As String = ""
        Dim PreviouslyOwned_Flag As String = ""
        PanelCollapseEx.Collapsed = True
        PanelCollapseEx.ClientState = True
        Dim model_type_string As String = ""
        Dim brand_string As String = ""
        Dim make_string As String = ""
        Dim model_string As String = ""
        Dim YearString As String = ""


        Session.Item("Yacht_Listing_Search") = Nothing

        'Life Cycle Building
        LifeCycleStage_String = clsGeneral.clsGeneral.ExtractSelectedStringFromListboxDropdown(lifecycle, True, 0, True)

        'Category Size Building
        CategorySize_String = clsGeneral.clsGeneral.ExtractSelectedStringFromListboxDropdown(yacht_category, False, 0, False)

        'Motor Building
        MotorType_String = clsGeneral.clsGeneral.ExtractSelectedStringFromListboxDropdown(yacht_category, False, 1, False)

        'Ownership Building
        Ownership_String = clsGeneral.clsGeneral.ExtractSelectedStringFromListboxDropdown(ownership, True, 0, True)


        'brand String Building
        model_type_string = clsGeneral.clsGeneral.ExtractSelectedStringFromListboxDropdown(brand, True, 0, False)
        brand_string = clsGeneral.clsGeneral.ExtractSelectedStringFromListboxDropdown(brand, False, 0, True)

        'Model String Building.
        model_string = clsGeneral.clsGeneral.ExtractSelectedStringFromListboxDropdown(model, True, 0, False)

        'year string building
        YearString = yacht_year.SelectedValue

        Select Case market.SelectedValue
            Case "For Sale"
                ForSale_Flag = "Y"
            Case "For Sale/Lease"
                ForLease_Flag = "Y"
                ForSale_Flag = "Y"
            Case "For Sale/Trade"
                ForSale_Flag = "Y"
            Case "For Sale on Exclusive"
                ForSale_Flag = "Y"
                OnExclusive_Flag = "Y"
            Case "For Sale Not on Exclusive"
                ForSale_Flag = "Y"
                OnExclusive_Flag = "N"
            Case "Not For Sale"
                ForSale_Flag = "N"
            Case "Lease"
                ForLease_Flag = "Y"
        End Select

        If Not String.IsNullOrEmpty(serial_number_from.Text) Then
            SerialNo_Start = serial_number_from.Text
        End If
        If Not String.IsNullOrEmpty(serial_number_to.Text) Then
            SerialNo_End = serial_number_to.Text
        End If
        If Not String.IsNullOrEmpty(registration_number.Text) Then
            RegistrationNo = registration_number.Text
        End If
        'Lease Flag
        If lease_status.SelectedValue <> "" Then
            ForLease_Flag = lease_status.SelectedValue
        End If
        'Previously Owned Flag
        If previously_owned.SelectedValue <> "" Then
            PreviouslyOwned_Flag = previously_owned.SelectedValue
        End If

        RaiseEvent Yacht_Search(model_string, ForSale_Flag, ForLease_Flag, OnExclusive_Flag, SerialNo_Start, SerialNo_End, RegistrationNo, registration_number_exact_match.Checked, do_not_search_prev_reg.Checked, LifeCycleStage_String, Status, Ownership_String, PreviouslyOwned_Flag, model_type_string, make_string, brand_string, YearString, CategorySize_String, MotorType_String, PageNumber, PageSort)

    End Sub

    Public Sub Fill_Page_To_To_Dropdown(ByVal pageNumber As Integer)
        go_to_submenu_dropdown.Items.Clear()
        For x = 1 To pageNumber
            go_to_submenu_dropdown.Items.Add(New ListItem(x, x))
        Next
    End Sub

    Private Sub next__Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles next_.Click, previous.Click, next_all.Click, previous_all.Click
        If sender.id.ToString = "next_" Then
            RaiseEvent PageData(True, False, False, False, False, 0)
        ElseIf sender.id.ToString = "previous" Then
            RaiseEvent PageData(False, True, False, False, False, 0)
        ElseIf sender.id.ToString = "next_all" Then
            RaiseEvent PageData(False, False, True, False, False, 0)
        ElseIf sender.id.ToString = "previous_all" Then
            RaiseEvent PageData(False, False, False, True, False, 0)
        End If
    End Sub





End Class