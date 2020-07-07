' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/adminSubscribers.aspx.vb $
'$$Author: Matt $
'$$Date: 6/17/20 7:57a $
'$$Modtime: 6/16/20 10:36p $
'$$Revision: 18 $
'$$Workfile: adminSubscribers.aspx.vb $
'
' ********************************************************************************

Partial Public Class adminSubscribers
  Inherits System.Web.UI.Page

  Dim PageNumber As Integer = 1
  Dim PageSort As String = ""
  Dim bYachtFlag As Boolean = False

  Dim nToggleView As Integer = 0 ' start off with subscription view

  Protected localDatalayer As New admin_center_dataLayer

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    If Session.Item("crmUserLogon") <> True Then

      Response.Redirect("Default.aspx", True)

    Else

      Master.Set_Active_Tab(3)

      Dim sErrorString As String = ""

      If Not Session.Item("localPreferences").loadUserSession(sErrorString, CLng(HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString), _
                                                              HttpContext.Current.Session.Item("localUser").crmUserLogin.ToString, _
                                                              CLng(HttpContext.Current.Session.Item("localUser").crmSubSeqNo.ToString), _
                                                              CLng(HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString)) Then
        Response.Redirect("Default.aspx", True)
      End If

      ToggleHigherLowerBar(False)

      localDatalayer.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      localDatalayer.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
      localDatalayer.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
      localDatalayer.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
      localDatalayer.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim

      viewCCSTDropDowns.setIsBase(False)
      viewCCSTDropDowns.setIsView(False)
      viewCCSTDropDowns.setListSize(10)
      viewCCSTDropDowns.setShowInactiveCountries(False)
      viewCCSTDropDowns.setFirstControl(True)

      load_page_session_variables()

      If Not Page.IsPostBack Then

        If subscriber_Criteria.Visible = True Then

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

          If Session.Item("localSubscription").crmYacht_Flag <> True Then
            comp_product_yacht_flag.Visible = False
            comp_product_yacht_flag.Checked = False
          ElseIf Session.Item("localSubscription").crmYacht_Flag = True Then
            bYachtFlag = True
          End If

          FillListBoxes(company_relationship, company_business, company_contact_title, service_used, service_code_list)

          DisplayFunctions.SetPagingItem(Subscriber_per_page_dropdown)

          FillOutSearchParameters()
        End If

        Initial(True)
      Else

        If goto_companySearch.Checked Then
          HttpContext.Current.Response.Redirect("Company_Listing.aspx", True)
        End If

        Initial(False)
      End If

      Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("Subscriber Search - Home")

    End If

  End Sub

  Public Sub Initial(ByVal initial_page_load As Boolean)
    Try
      If initial_page_load Then
        If subscriber_Criteria.Visible Then

          Subscriber_criteria_results.Visible = False
          Subscriber_sort_by_text.Visible = False
          Subscriber_sort_by_dropdown.Visible = False
          Subscriber_actions_dropdown.Visible = False
          Subscriber_paging.Visible = False

          Subscriber_per_page_dropdown_.Visible = False
          Subscriber_per_page_text.Visible = False
          Subscriber_go_to_dropdown_.Visible = False
          Subscriber_go_to_text.Visible = False

          Subscriber_view_dropdown.Visible = False

          SubscriberPanelEx.Collapsed = False
          SubscriberPanelEx.ClientState = False
        End If
      Else
        If subscriber_Criteria.Visible Then
          Subscriber_criteria_results.Visible = True
          Subscriber_sort_by_text.Visible = True
          Subscriber_sort_by_dropdown.Visible = True
          Subscriber_actions_dropdown.Visible = True
          Subscriber_paging.Visible = True
          Subscriber_view_dropdown.Visible = True

          Subscriber_per_page_dropdown_.Visible = True
          Subscriber_per_page_text.Visible = True
          'company_go_to_dropdown_.Visible = True
          'company_go_to_text.Visible = True

          SubscriberPanelEx.Collapsed = True
          SubscriberPanelEx.ClientState = True

          SwitchGalleryListing(nToggleView)
        End If
      End If
    Catch ex As Exception
      Master.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString & " : " & ex.Message.ToString)
    End Try
  End Sub

  Public Sub ToggleHigherLowerBar(ByVal lower_bar As Boolean)
    Try
      'setting the javascript of the menus
      If subscriber_Criteria.Visible = True Then
        If lower_bar = True Then
          SubscriberPanelEx.Collapsed = True
          SubscriberPanelEx.ClientState = True
          Subscriber_search_expand_text.Visible = False
          Subscriber_help_text.Visible = False
          Subscriber_sort_by_text.Visible = False
          Subscriber_sort_by_dropdown.Visible = False
        Else
          Subscriber_per_page_dropdown_.Visible = False
          Subscriber_per_page_text.Visible = False
          Subscriber_go_to_dropdown_.Visible = False
          Subscriber_go_to_text.Visible = False
        End If

        'sort
        Subscriber_view_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & Subscriber_view_submenu_dropdown.ClientID & "', true);")
        Subscriber_view_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & Subscriber_view_submenu_dropdown.ClientID & "', false);")

        Subscriber_view_submenu_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & Subscriber_view_submenu_dropdown.ClientID & "', true);")
        Subscriber_view_submenu_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & Subscriber_view_submenu_dropdown.ClientID & "', false);")

        'sort dropdown
        Subscriber_sort_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & Subscriber_sort_submenu_dropdown.ClientID & "', true);")
        Subscriber_sort_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & Subscriber_sort_submenu_dropdown.ClientID & "', false);")

        Subscriber_sort_submenu_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & Subscriber_sort_submenu_dropdown.ClientID & "', true);")
        Subscriber_sort_submenu_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & Subscriber_sort_submenu_dropdown.ClientID & "', false);")

        'page dropdown
        Subscriber_per_page_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & Subscriber_per_page_submenu_dropdown.ClientID & "', true);")
        Subscriber_per_page_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & Subscriber_per_page_submenu_dropdown.ClientID & "', false);")

        Subscriber_per_page_submenu_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & Subscriber_per_page_submenu_dropdown.ClientID & "', true);")
        Subscriber_per_page_submenu_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & Subscriber_per_page_submenu_dropdown.ClientID & "', false);")

        'go to dropdown
        Subscriber_go_to_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & Subscriber_go_to_submenu_dropdown.ClientID & "', true);")
        Subscriber_go_to_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & Subscriber_go_to_submenu_dropdown.ClientID & "', false);")

        Subscriber_go_to_submenu_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & Subscriber_go_to_submenu_dropdown.ClientID & "', true);")
        Subscriber_go_to_submenu_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & Subscriber_go_to_submenu_dropdown.ClientID & "', false);")

        'actions dropdown
        Subscriber_actions_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & Subscriber_actions_submenu_dropdown.ClientID & "', true);")
        Subscriber_actions_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & Subscriber_actions_submenu_dropdown.ClientID & "', false);")

        Subscriber_actions_submenu_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & Subscriber_actions_submenu_dropdown.ClientID & "', true);")
        Subscriber_actions_submenu_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & Subscriber_actions_submenu_dropdown.ClientID & "', false);")


      End If

    Catch ex As Exception
      Master.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString & " : " & ex.Message.ToString)
    End Try
  End Sub

  Public Function DisplayContactInfoListing(ByVal companyID As Long, ByVal contactID As Long, ByVal sirname As String, ByVal firstName As String, ByVal middle As String, ByVal lastName As String, ByVal title As String, ByVal gallery As Boolean)
    Dim returnString As String = ""
    Try
      If Not IsNothing(contactID) Then
        If IsNumeric(contactID) Then
          If gallery Then
            returnString = "<span class=""li_no_bullet"">"
          End If
          returnString += crmWebClient.DisplayFunctions.WriteDetailsLink(0, companyID, contactID, 0, True, IIf(Not String.IsNullOrEmpty(sirname.Trim), sirname.Trim + " ", "") + firstName.Trim + " " + IIf(Not String.IsNullOrEmpty(middle.Trim), middle.Trim + ". ", "") + lastName.Trim, IIf(gallery, "small_to_medium_text blue_text", ""), "")
          If gallery Then
            If Not String.IsNullOrEmpty(title.Trim) Then
              returnString += " <em class='tiny_text'>" + title.Trim + "</em>"
            End If
          End If
          If gallery Then
            returnString += "</span>"
          End If
        End If
      End If
    Catch ex As Exception
      Master.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString & " : " & ex.Message.ToString)
    End Try

    Return returnString
  End Function

  Public Sub submenu_dropdown_Click(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.BulletedListEventArgs)
    Try
      Dim selectedLI As New ListItem
      selectedLI = sender.Items(e.Index)
      If sender.id.ToString = "Subscriber_sort_submenu_dropdown" Then
        Subscriber_sort_dropdown.Items.Clear()
        Subscriber_sort_dropdown.Items.Add(New ListItem(selectedLI.Text, ""))
        SetPageSort(selectedLI.Text)
        Subscriber_search_Click(Subscriber_search, EventArgs.Empty)

      ElseIf sender.id.ToString = "Subscriber_view_submenu_dropdown" Then

        SwitchGalleryListing(e.Index)
        Subscriber_search_Click(Subscriber_search, EventArgs.Empty, False)

      ElseIf sender.id.ToString = "Subscriber_go_to_submenu_dropdown" Then
        Subscriber_go_to_dropdown.Items.Clear()
        Subscriber_go_to_dropdown.Items.Add(New ListItem(selectedLI.Text, ""))

        SetPageNumber(CInt(selectedLI.Text))
        MovePage(False, False, False, False, True, PageNumber)

      ElseIf sender.id.ToString = "Subscriber_per_page_submenu_dropdown" Then
        Subscriber_per_page_dropdown.Items.Clear()
        Subscriber_per_page_dropdown.Items.Add(New ListItem(selectedLI.Text & " ", selectedLI.Text))
        Session.Item("localUser").crmUserRecsPerPage = CInt(selectedLI.Value)
        MovePage(False, False, False, False, False, PageNumber)
      End If
    Catch ex As Exception
      Master.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString & " : " & ex.Message.ToString)
    End Try
  End Sub

  Public Sub SwitchGalleryListing(ByVal showtype As Integer)
    Try
      Select Case showtype
        Case 0

          Subscriber_view_dropdown.Items.Clear()
          Subscriber_view_dropdown.Items.Add(New ListItem("", ""))
          Subscriber_view_dropdown.CssClass = "ul_top listing_view_bullet"
          Results_Subscription.Visible = True
          Results_Subscriber.Visible = False
          nToggleView = 0

        Case 1

          Subscriber_view_dropdown.Items.Clear()
          Subscriber_view_dropdown.Items.Add(New ListItem("", ""))
          Subscriber_view_dropdown.CssClass = "ul_top thumnail_view_bullet"

          Results_Subscription.Visible = False
          Results_Subscriber.Visible = True
          nToggleView = 1

      End Select

    Catch ex As Exception
      Master.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString & " : " & ex.Message.ToString)
    End Try
  End Sub

  Public Sub SetPagingButtons(ByVal back_page As Boolean, ByVal next_page As Boolean)

    Subscriber_previous_all.Visible = back_page
    Subscriber_previous.Visible = back_page

    Subscriber_next_all.Visible = next_page
    Subscriber_next.Visible = next_page

  End Sub

  Public Sub Fill_Page_To_To_Dropdown(ByVal pageNumber As Integer)
    If subscriber_Criteria.Visible = True Then
      Subscriber_go_to_submenu_dropdown.Items.Clear()
      For x = 1 To pageNumber
        Subscriber_go_to_submenu_dropdown.Items.Add(New ListItem(x, x))
      Next
    End If
  End Sub

  Public Sub SetPageSort(Optional ByVal selectedLI As String = "")
    Select Case selectedLI
      Case "Address"
        PageSort = " comp_address"
      Case "City"
        PageSort = " comp_city"
      Case "State"
        PageSort = " comp_state"
      Case "Country"
        PageSort = " comp_country"
      Case "Subscription"
        PageSort = " sub_id"
      Case Else
        PageSort = " comp_name "
    End Select
  End Sub

  Public Sub SetPageNumber(Optional ByVal selectedLI As Integer = 0)
    PageNumber = selectedLI
  End Sub

  Public Sub MovePage(ByVal next_ As Boolean, ByVal prev_ As Boolean, ByVal next_all As Boolean, ByVal prev_all As Boolean, ByVal goToPage As Boolean, ByVal pageNumber As Integer)
    Try
      Dim holdTable As New DataTable
      Dim StartCount As Integer = 0
      Dim EndCount As Integer = 0
      Dim RecordsPerPage As Integer = 0

      If Session.Item("localUser").crmUserRecsPerPage <> 0 Then
        RecordsPerPage = Session.Item("localUser").crmUserRecsPerPage
      End If

      If Not IsNothing(Session.Item("Subscription_Master")) Then
        holdTable = Session.Item("Subscription_Master")
        Initial(False)

        If Results_Subscription.Visible Then
          DisplayFunctions.MovePage(StartCount, EndCount, Results_Subscription, Nothing, holdTable, next_, prev_, next_all, prev_all, goToPage, pageNumber)
        ElseIf Results_Subscriber.Visible Then
          DisplayFunctions.MovePage(StartCount, EndCount, Results_Subscriber, Nothing, holdTable, next_, prev_, next_all, prev_all, goToPage, pageNumber)
        End If

        SetPagingButtons(IIf(StartCount = 1, False, True), IIf(holdTable.Rows.Count = EndCount, False, True))

        Subscriber_record_count.Text = "Showing " + StartCount.ToString + " - " + IIf(holdTable.Rows.Count <= RecordsPerPage, holdTable.Rows.Count.ToString, IIf((RecordsPerPage + StartCount) <= holdTable.Rows.Count, (RecordsPerPage + StartCount).ToString, holdTable.Rows.Count.ToString))

        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "updateSubscriberControls", postBackScript, True)

      End If
    Catch ex As Exception
      'Some More Error Catching.
      Master.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString & " : " & ex.Message.ToString)
    End Try

  End Sub

  Private Sub next__Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Subscriber_next.Click, Subscriber_previous.Click, Subscriber_next_all.Click, Subscriber_previous_all.Click
    Try
      If sender.commandname.ToString = "next" Then
        MovePage(True, False, False, False, False, 0)
      ElseIf sender.commandname.ToString = "previous" Then
        MovePage(False, True, False, False, False, 0)
      ElseIf sender.commandname.ToString = "next_all" Then
        MovePage(False, False, True, False, False, 0)
      ElseIf sender.commandname.ToString = "previous_all" Then
        MovePage(False, False, False, True, False, 0)
      End If
    Catch ex As Exception
      Master.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString & " : " & ex.Message.ToString)
    End Try
  End Sub

  Public Sub FillListBoxes(ByVal relationshipType As ListBox, ByVal BusinessType As ListBox, ByVal ContactTitleGroup As ListBox, ByVal serviceUsed As ListBox, ByVal serviceCode As ListBox)
    If Not Page.IsPostBack Then
      Try
        Dim TempTable As New DataTable

        relationshipType.Items.Clear()


        TempTable = New DataTable
        TempTable = Master.aclsData_temp.Get_Jetnet_Business_Type()
        clsGeneral.clsGeneral.Populate_Listbox(TempTable, BusinessType, "cbus_name", "cbus_type", False)

        TempTable = New DataTable
        TempTable = Master.aclsData_temp.Get_Jetnet_Contact_Title_Group()
        clsGeneral.clsGeneral.Populate_Listbox(TempTable, ContactTitleGroup, "ctitleg_group_name", "ctitleg_group_name", False)

        TempTable = New DataTable
        TempTable = getServiceUsed()
        clsGeneral.clsGeneral.Populate_Listbox(TempTable, serviceUsed, "svud_desc", "svud_id", False)

        TempTable = New DataTable
        TempTable = getServiceCodes()
        clsGeneral.clsGeneral.Populate_Listbox(TempTable, serviceCode, "service_text", "serv_code", False, False, True)

        TempTable = New DataTable


        If bYachtFlag = False Then
          TempTable = Master.aclsData_temp.Get_Client_Aircraft_Contact_Type()
          clsGeneral.clsGeneral.Populate_Listbox(TempTable, relationshipType, "cliact_name", "cliact_type", True)

          relationshipType.Items.RemoveAt(0)

          relationshipType.Items.Insert(0, New ListItem("All", ""))
          relationshipType.Items.Insert(1, New ListItem("All Owners", "'00','97','17','08','16'"))
          relationshipType.Items.Insert(2, New ListItem("All Operating Companies", "'Y'"))
          relationshipType.Items.Insert(3, New ListItem("All Dealers, Brokers, Reps", "'93','98','99'"))
          relationshipType.SelectedValue = ""
        Else

          TempTable = Master.aclsData_temp.Get_Yacht_Contact_Type(False)
          clsGeneral.clsGeneral.Populate_Listbox(TempTable, relationshipType, "yct_name", "yct_code", True)
          relationshipType.Items.RemoveAt(0)
          relationshipType.Items.Insert(0, New ListItem("All", ""))
          relationshipType.Items.Insert(1, New ListItem("All Central Agents", "'99','C1','C2','C3','C4','C5','C6'"))
          relationshipType.Items.Insert(2, New ListItem("All Designers", "'Y1','Y2','Y3','Y0','Y9'"))
          relationshipType.Items.Insert(3, New ListItem("All Owners", "'00','08'"))
          relationshipType.SelectedValue = ""
        End If

        TempTable.Dispose()
      Catch ex As Exception
        'Some More Error Catching.
        Master.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString & " : " & ex.Message.ToString)
      End Try
    End If
  End Sub

  Private Sub Subscriber_search_Click(ByVal sender As Object, ByVal e As System.EventArgs, Optional ByVal LoadFromSession As Boolean = False) Handles Subscriber_search.Click

    Dim nContactID As Long = 0
    Dim nCompanyID As Long = 0

    Dim localCriteria As New SearchSelectionCriteria

    Dim sCompanyNameQuery As String = ""
    Dim sOperatorFlagQuery As String = ""

    Dim sRelationshipsQuery As String = ""
    Dim sRelationshipsTmp As String = ""

    Dim sBusinesstypeQuery As String = ""
    Dim sContactTitleQuery As String = ""

    Dim sServicesQuery As String = ""
    Dim sServiceCodeQuery As String = ""

    Dim sSeperator As String = ""

    Try

      ' pick up product code selections
      localCriteria.SearchCriteriaHelicopterFlag = comp_product_helicopter_flag.Checked
      localCriteria.SearchCriteriaBusinessFlag = comp_product_business_flag.Checked
      localCriteria.SearchCriteriaCommercialFlag = comp_product_commercial_flag.Checked
      localCriteria.SearchCriteriaYachtFlag = comp_product_yacht_flag.Checked

      'Company Name

      If Not String.IsNullOrEmpty(company_name.Text) Then

        Dim TempCompHold As String = company_name.Text.Replace(",", "_")
        TempCompHold = clsGeneral.clsGeneral.CleanUserData(TempCompHold, Constants.cEmptyString, Constants.cCommaDelim, True)
        TempCompHold.Replace(",", ";")
        TempCompHold.Replace(";", "*;")

        Dim TempNameHold As String = clsGeneral.clsGeneral.FilterCompanyNameForCompanyAircraftSearch(TempCompHold)

                'sCompanyNameQuery = "("
                'If TempNameHold.Contains("*") Then
                '  sCompanyNameQuery += "comp_name_search " + clsGeneral.clsGeneral.PrepQueryString("Begins With", TempNameHold, "String", False, "comp_name_search", True)
                'Else
                '  sCompanyNameQuery += "comp_name_search " + clsGeneral.clsGeneral.PrepQueryString("Begins With", TempNameHold, "String", False, "comp_name_search", True)
                'End If
                '        sCompanyNameQuery += ")"

                sCompanyNameQuery = "( "
                If InStr(TempNameHold, "*") = 0 Then
                    sCompanyNameQuery += "( comp_name_search " & clsGeneral.clsGeneral.PrepQueryString("Begins With", TempNameHold, "String", False, "comp_name_search", True)
                    sCompanyNameQuery += " OR comp_altname_search " & clsGeneral.clsGeneral.PrepQueryString("Begins With", TempNameHold, "String", False, "comp_altname_search", True) & ")"
                    'CompanyName += ")"
                Else
                    sCompanyNameQuery += " ( " & clsGeneral.clsGeneral.PrepQueryString("Begins With", TempNameHold, "String", False, "comp_name_search", True)
                    sCompanyNameQuery += " OR " & clsGeneral.clsGeneral.PrepQueryString("Begins With", TempNameHold, "String", False, "comp_altname_search", True) & " ) "
                    'CompanyName += ")"

                End If
                sCompanyNameQuery += ")"


                localCriteria.SearchCriteriaDisplayString += "Company Name" + " " + "Begins With" + " " + Replace(TempNameHold, ":", " and ") + "<br />"

        localCriteria.SearchCriteriaCompanyName = company_name.Text
        localCriteria.SearchCriteriaCompanyNameQueryString = sCompanyNameQuery.Trim

      End If

      'Relationships

      For i As Integer = 0 To company_relationship.Items.Count - 1

        If company_relationship.Items(i).Selected Then

          If Not String.IsNullOrEmpty(company_relationship.Items(i).Value.Trim) Then 'Here we check to see if there is a value, meaning there's no selection

            If Not company_relationship.Items(i).Value.ToUpper.Contains("ALL") Then 'Checking to make sure ALL isn't checked, if it is, we don't need to search

              If company_relationship.Items(i).Value.ToUpper.Contains("'Y'") Then

                sOperatorFlagQuery = " (cref_operator_flag IN ('Y', 'O')) "

                If comp_not_in_selected.Checked = False Then
                  localCriteria.SearchCriteriaDisplayString += DisplayFunctions.BuildSearchTextDisplay("All Operators Included", "Operators")
                Else
                  localCriteria.SearchCriteriaDisplayString += DisplayFunctions.BuildSearchTextDisplay("Not Included", "Operators")
                End If

              Else

                If String.IsNullOrEmpty(sRelationshipsTmp.Trim) Then
                  sRelationshipsTmp = company_relationship.Items(i).Value.ToUpper
                Else
                  sRelationshipsTmp += company_relationship.Items(i).Value.ToUpper + ","
                End If

              End If


            End If
          End If
        End If
      Next

      'Saving the relationships
      localCriteria.SearchCriteriaCompanyRelationshipsToAC = sRelationshipsTmp

      'Reationships/Operator Flag, builds part of the search
      If Not String.IsNullOrEmpty(sRelationshipsTmp.Trim) Or Not String.IsNullOrEmpty(sOperatorFlagQuery.Trim) Then

        If Not comp_not_in_selected.Checked Then

          If Not String.IsNullOrEmpty(sRelationshipsTmp.Trim) Then

            If bYachtFlag Then
              sRelationshipsQuery = " (yr_contact_type in (" + sRelationshipsTmp.Trim + ") ) "
            Else
              sRelationshipsQuery = " (cref_contact_type in (" + sRelationshipsTmp.Trim + ") ) "
            End If

            localCriteria.SearchCriteriaDisplayString += DisplayFunctions.BuildSearchTextDisplay(company_relationship, "Relationships")
          End If

        Else

          'Saving the not in selected relationships
          'And going ahead and then running them.
          localCriteria.SearchCriteriaCompanyNotInSelectedRelationship = True

          If Not String.IsNullOrEmpty(sRelationshipsTmp.Trim) Then

            If bYachtFlag Then
              sRelationshipsQuery = " (NOT EXISTS (SELECT NULL FROM Yacht_Reference WITH (NOLOCK) WHERE (yr_yt_id = yt_id) AND (yr_journ_id = yt_journ_id) AND (("
              sRelationshipsQuery += " yr_contact_type IN (" + sRelationshipsTmp.Trim + ") "
            Else

              sRelationshipsQuery = " (NOT EXISTS (SELECT NULL FROM Aircraft_Reference WITH (NOLOCK) WHERE (cref_comp_id = comp_id)  AND (cref_journ_id = comp_journ_id) AND (("
              sRelationshipsQuery += " cref_contact_type IN (" + sRelationshipsTmp.Trim + ") "
            End If

            If Not String.IsNullOrEmpty(sOperatorFlagQuery.Trim) Then
              If Not String.IsNullOrEmpty(sRelationshipsTmp.Trim) Then
                sRelationshipsQuery += Constants.cOrClause
              End If
            End If
          Else 'No operator flag on the yacht side as of yet, so this doesn't need to be taken into account and had the table swapped depending yet
            sRelationshipsQuery = " (NOT EXISTS (SELECT NULL FROM Aircraft_Reference WITH (NOLOCK) WHERE (cref_comp_id = comp_id) AND (cref_journ_id = comp_journ_id) AND (("
          End If

          If Not String.IsNullOrEmpty(sOperatorFlagQuery.Trim) Then
            sRelationshipsQuery += sOperatorFlagQuery.Trim
          End If

          sRelationshipsQuery += " )))) "

          localCriteria.SearchCriteriaDisplayString += DisplayFunctions.BuildSearchTextDisplay(company_relationship, "Not in these Relationships")
        End If

      End If

      'Service Code( make this handle multi types )
      sServiceCodeQuery = clsGeneral.clsGeneral.ExtractSelectedStringFromListboxDropdown(service_code_list, True, 0, True)
      If Not String.IsNullOrEmpty(sServiceCodeQuery.Trim) Then
        localCriteria.SearchCriteriaDisplayString += DisplayFunctions.BuildSearchTextDisplay(service_code_list, "Service Code")
        'Saving the Service Code
        localCriteria.SearchCriteriaService_code = sServiceCodeQuery
      End If

      'Services Used ( make this handle multi types )
      sServicesQuery = clsGeneral.clsGeneral.ExtractSelectedStringFromListboxDropdown(service_used, False, 0, True)
      If Not String.IsNullOrEmpty(sServicesQuery.Trim) Then
        localCriteria.SearchCriteriaDisplayString += DisplayFunctions.BuildSearchTextDisplay(service_used, "Services Used")
        'Saving the Services Used
        localCriteria.SearchCriteriaServices = sServicesQuery
      End If

      'Business Type ( make this handle multi types )
      sBusinesstypeQuery = clsGeneral.clsGeneral.ExtractSelectedStringFromListboxDropdown(company_business, True, 0, True)
      If Not String.IsNullOrEmpty(sBusinesstypeQuery.Trim) Then
        localCriteria.SearchCriteriaDisplayString += DisplayFunctions.BuildSearchTextDisplay(company_business, "Business Type")
        'Saving the Business Type
        localCriteria.SearchCriteriaCompanyBusinessType = sBusinesstypeQuery
      End If

      'Company Email 
      If Not String.IsNullOrEmpty(company_email_address.Text.Trim) Then
        localCriteria.SearchCriteriaDisplayString += DisplayFunctions.BuildSearchTextDisplay(company_email_address, "Email")
        'Company Email in session
        localCriteria.SearchCriteriaCompanyEmail = clsGeneral.clsGeneral.StripChars(company_email_address.Text, True).Trim
      End If

      'Company Postal Code
      If Not String.IsNullOrEmpty(comp_zip_code.Text.Trim) Then
        localCriteria.SearchCriteriaDisplayString += DisplayFunctions.BuildSearchTextDisplay(comp_zip_code, "Zip Code")
        'Company Postal Codein session
        localCriteria.SearchCriteriaCompanyPostalCode = clsGeneral.clsGeneral.StripChars(comp_zip_code.Text, True).Trim
      End If

      'Company Address 
      If Not String.IsNullOrEmpty(company_address.Text.Trim) Then
        localCriteria.SearchCriteriaDisplayString += DisplayFunctions.BuildSearchTextDisplay(company_address, "Company Address")
        'Company Address in session
        localCriteria.SearchCriteriaCompanyAddress = clsGeneral.clsGeneral.StripChars(company_address.Text, True).Trim
      End If

      'Company City 
      If Not String.IsNullOrEmpty(comp_city.Text.Trim) Then
        localCriteria.SearchCriteriaDisplayString += DisplayFunctions.BuildSearchTextDisplay(comp_city, "City")
        'Company City in session
        localCriteria.SearchCriteriaCompanyCity = clsGeneral.clsGeneral.StripChars(comp_city.Text, True).Trim
      End If

      'Contact First
      If Not String.IsNullOrEmpty(company_contact_first.Text.Trim) Then
        localCriteria.SearchCriteriaDisplayString += DisplayFunctions.BuildSearchTextDisplay(company_contact_first, "First Name")
        'Saving the Contact First
        localCriteria.SearchCriteriaCompanyContactFirstName = clsGeneral.clsGeneral.StripChars(company_contact_first.Text, True).Trim
      End If

      'Contact Last
      If Not String.IsNullOrEmpty(company_contact_last.Text.Trim) Then
        localCriteria.SearchCriteriaDisplayString += DisplayFunctions.BuildSearchTextDisplay(company_contact_last, "Last Name")
        'Saving the Contact Last
        localCriteria.SearchCriteriaCompanyContactLastName = clsGeneral.clsGeneral.StripChars(company_contact_last.Text, True).Trim
      End If

      'contact Email in session
      If Not String.IsNullOrEmpty(company_contact_email_address.Text.Trim) Then
        localCriteria.SearchCriteriaDisplayString += DisplayFunctions.BuildSearchTextDisplay(company_contact_email_address, "Email")
        'Saving the contact Email
        localCriteria.SearchCriteriaCompanyContactEmail = clsGeneral.clsGeneral.StripChars(company_contact_email_address.Text, True).Trim
      End If

      'Contact Title
      sContactTitleQuery = clsGeneral.clsGeneral.ExtractSelectedStringFromListboxDropdown(company_contact_title, True, 0, True)
      If Not String.IsNullOrEmpty(sContactTitleQuery.Trim) Then
        localCriteria.SearchCriteriaDisplayString += DisplayFunctions.BuildSearchTextDisplay(company_contact_title, "Contact Title")
        'Saving the Contact Title
        localCriteria.SearchCriteriaCompanyContactTitle = sContactTitleQuery
      End If

      'Company ID 
      If Not String.IsNullOrEmpty(company_id.Text.Trim) Then
        localCriteria.SearchCriteriaCompanyID = CLng(company_id.Text)
      End If

      'Contact ID 
      If Not String.IsNullOrEmpty(company_contact_id.Text.Trim) Then
        localCriteria.SearchCriteriaCompanyContactID = CLng(company_contact_id.Text)
      End If

      If bYachtFlag Then
        If company_status_flag.Checked Then
          localCriteria.SearchCriteriaCompanyDisplayInactiveCompanies = True
          localCriteria.SearchCriteriaDisplayString += DisplayFunctions.BuildSearchTextDisplay(company_status_flag, "Display Inactive Companies")
        End If
      End If

      'Not In Selected Relationship
      localCriteria.SearchCriteriaCompanyNotInSelectedRelationship = comp_not_in_selected.Checked

      ' get the "selected" items from the "company location drop-downs"
      If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("companyRegionOrContinent").ToString.Trim) Then

        If HttpContext.Current.Session.Item("companyRegionOrContinent").ToString.Trim.ToLower.Contains("continent") Then
          localCriteria.SearchCriteriaUseContinent = True
          localCriteria.SearchCriteriaUseRegion = False
          HttpContext.Current.Session.Item("companyRegionOrContinent") = "Continent"
        Else
          localCriteria.SearchCriteriaUseContinent = False
          localCriteria.SearchCriteriaUseRegion = True
          HttpContext.Current.Session.Item("companyRegionOrContinent") = "Region"
        End If

      Else
        localCriteria.SearchCriteriaUseContinent = True
        localCriteria.SearchCriteriaUseRegion = False
        HttpContext.Current.Session.Item("companyRegionOrContinent") = "Continent"
      End If

      localCriteria.SearchCriteriaHasCompanyLocationInfo = False

      ' pick up the Continent/Region from the "company location drop-downs"
      If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("companyRegion").ToString.Trim) Then

        Dim continentArray() As String = Nothing

        ' Check and see if user selected more than one continent/region
        'If InStr(Session.Item("companyRegion").ToString, crmWebClient.Constants.cCommaDelim) Then
        '  continentArray = Session.Item("companyRegion").ToString.Split(crmWebClient.Constants.cCommaDelim)

        '  If IsArray(continentArray) And Not IsNothing(continentArray) Then

        '    localCriteria.ViewCriteriaContinentArray = continentArray

        '  End If

        'End If

        localCriteria.SearchCriteriaCompanyContinent = HttpContext.Current.Session.Item("companyRegion").ToString
        localCriteria.SearchCriteriaHasCompanyLocationInfo = True

      Else
        localCriteria.SearchCriteriaCompanyContinent = ""
        HttpContext.Current.Session.Item("companyRegion") = ""
      End If

      ' pick up the Country from the "company location drop-downs"
      If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("companyCountry").ToString.Trim) Then

        Dim countryArray() As String = Nothing

        ' Check and see if user selected more than one country
        'If InStr(Session.Item("companyCountry").ToString, crmWebClient.Constants.cCommaDelim) Then
        '  countryArray = Session.Item("companyCountry").ToString.Split(crmWebClient.Constants.cCommaDelim)

        '  If IsArray(countryArray) And Not IsNothing(countryArray) Then

        '    localCriteria.ViewCriteriaCountryArray = countryArray

        '  End If

        'End If

        localCriteria.SearchCriteriaCompanyRegion = HttpContext.Current.Session.Item("companyCountry").ToString
        localCriteria.SearchCriteriaHasCompanyLocationInfo = True

      Else
        localCriteria.SearchCriteriaCompanyRegion = ""
        HttpContext.Current.Session.Item("companyCountry") = ""
      End If

      ' pick up the state from the "company location drop-downs"
      If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("companyState").ToString.Trim) Then

        Dim stateArray() As String = Nothing

        ' Check and see if user selected more than one state
        'If InStr(Session.Item("companyState").ToString, crmWebClient.Constants.cCommaDelim) Then
        '  stateArray = Session.Item("companyState").ToString.Split(crmWebClient.Constants.cCommaDelim)

        '  If IsArray(stateArray) And Not IsNothing(stateArray) Then

        '    localCriteria.ViewCriteriaStateArray = stateArray

        '  End If

        'End If

        ' translate the "state name" to its state code for query
        localCriteria.SearchCriteriaCompanyStateProvince = HttpContext.Current.Session.Item("companyState").ToString
        localCriteria.SearchCriteriaHasCompanyLocationInfo = True

      Else
        localCriteria.SearchCriteriaCompanyStateProvince = ""
        HttpContext.Current.Session.Item("companyState") = ""
      End If

      ' pick up the timeZone from the "company location drop-downs"
      If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("companyTimeZone").ToString.Trim) Then

        Dim timeZoneArray() As String = Nothing

        ' Check and see if user selected more than one timezone
        'If InStr(Session.Item("companyTimeZone").ToString, crmWebClient.Constants.cCommaDelim) Then
        '  timeZoneArray = Session.Item("companyTimeZone").ToString.Split(crmWebClient.Constants.cCommaDelim)

        '  If IsArray(timeZoneArray) And Not IsNothing(timeZoneArray) Then

        '    localCriteria.ViewCriteriaTimeZoneArray = timeZoneArray

        '  End If

        'End If

        localCriteria.SearchCriteriaCompanyTimezone = HttpContext.Current.Session.Item("companyTimeZone").ToString
        localCriteria.SearchCriteriaHasCompanyLocationInfo = True

      Else
        localCriteria.SearchCriteriaCompanyTimezone = ""
        HttpContext.Current.Session.Item("localCriteriaTimeZone") = ""
      End If

      If Not String.IsNullOrEmpty(sub_id.Text.Trim) Then
        localCriteria.SearchCriteriaDisplayString += DisplayFunctions.BuildSearchTextDisplay(sub_id, "SubID")
        localCriteria.SearchCriteriaSub_id = CLng(sub_id.Text)
      End If

      If Not String.IsNullOrEmpty(last_login_date.Text.Trim) Then
        localCriteria.SearchCriteriaDisplayString += DisplayFunctions.BuildSearchTextDisplay(last_login_date, "LoginDate")
        localCriteria.SearchCriteriaLast_login_date = last_login_date.Text
      End If

      If Not String.IsNullOrEmpty(sub_end_date.Text.Trim) Then
        localCriteria.SearchCriteriaDisplayString += DisplayFunctions.BuildSearchTextDisplay(sub_end_date, "EndDate")
        localCriteria.SearchCriteriaEnd_date = sub_end_date.Text
      End If

      If chkAerodexFlag.Checked Then
        localCriteria.SearchCriteriaDisplayString += "Aerodex = True"
        localCriteria.SearchCriteriaAerodexFlag = chkAerodexFlag.Checked
      End If

      If chkDemoFlag.Checked Then
        localCriteria.SearchCriteriaDisplayString += "Demo = True"
        localCriteria.SearchCriteriaDemoFlag = chkDemoFlag.Checked
      End If

      If chkMarketingFlag.Checked Then
        localCriteria.SearchCriteriaDisplayString += "Marketing = True"
        localCriteria.SearchCriteriaMarketingFlag = chkMarketingFlag.Checked
      End If

      If chkCRMFlag.Checked Then
        localCriteria.SearchCriteriaDisplayString += "CRM = True"
        localCriteria.SearchCriteriaCRMFlag = chkCRMFlag.Checked
      End If

      If chkSPIFlag.Checked Then
        localCriteria.SearchCriteriaDisplayString += "SPI = True"
        localCriteria.SearchCriteriaSPIFlag = chkSPIFlag.Checked
      End If

      If chkMobileFlag.Checked Then
        localCriteria.SearchCriteriaDisplayString += "Mobile = True"
        localCriteria.SearchCriteriaMobileFlag = chkMobileFlag.Checked
      End If

      If chkCloudNotesFlag.Checked Then
        localCriteria.SearchCriteriaDisplayString += "CLOUD = True"
        localCriteria.SearchCriteriaCloudNotesFlag = chkCloudNotesFlag.Checked
      End If

      If chkNotesPlusFlag.Checked Then
        localCriteria.SearchCriteriaDisplayString += "CLOUD+ = True"
        localCriteria.SearchCriteriaNotesPlusFlag = chkNotesPlusFlag.Checked
      End If

      If chkAdminFlag.Checked Then
        localCriteria.SearchCriteriaDisplayString += "ADMIN = True"
        localCriteria.SearchCriteriaAdminFlag = chkAdminFlag.Checked
      End If

      If Not String.IsNullOrEmpty(parent_subscriptions.SelectedValue.Trim) Then
        localCriteria.SearchCriteriaDisplayString += "Only Parent Subscriptions"
        localCriteria.SearchCriteriaParentSub = True
      End If

      If Not String.IsNullOrEmpty(sRelationshipsQuery.Trim) Then
        localCriteria.SearchCriteriaQueryString += sSeperator + sRelationshipsQuery.Trim
        sSeperator = Constants.cAndClause
      End If

      Session.Item("searchCriteria") = localCriteria

      run_subscriber_search(LoadFromSession, CType(Session.Item("searchCriteria"), SearchSelectionCriteria))

      localCriteria = Nothing

    Catch ex As Exception
      Master.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString + " : " + ex.Message.ToString)
    End Try

  End Sub

  Private Sub run_subscriber_search(ByVal bindFromSession As Boolean, ByVal searchCriteria As SearchSelectionCriteria)
    Try
      Dim RecordsPerPage As Integer = 0
      Dim Paging_Table As New DataTable
      Dim Results_Table As New DataTable

      If Session.Item("localUser").crmUserRecsPerPage <> 0 Then
        RecordsPerPage = Session.Item("localUser").crmUserRecsPerPage
      End If

      company_attention.Text = ""

      If bindFromSession And Not IsNothing(Session.Item("Subscription_Master")) Then
        Results_Table = Session.Item("Subscription_Master")
      Else
        Results_Table = localDatalayer.getSubscriberSearchDataTable(searchCriteria, Results_Subscription.Visible, chkHistoricalSub.Checked)
        Session.Item("Subscription_Master") = Results_Table
      End If

      HttpContext.Current.Session.Item("SearchString") = searchCriteria.SearchCriteriaDisplayString
      Master.SetStatusText(HttpContext.Current.Session.Item("SearchString"))

      If Not IsNothing(Results_Table) Then

        Session.Item("localUser").crmLatestRecordCount = Results_Table.Rows.Count

        If Results_Table.Rows.Count > 0 Then

          If Results_Subscription.Visible Then

            Results_Subscription.PageSize = IIf(Results_Table.Rows.Count <= RecordsPerPage, Results_Table.Rows.Count, RecordsPerPage)
            Results_Subscription.DataSource = Results_Table
            Results_Subscription.DataBind()

          ElseIf Results_Subscriber.Visible = True Then

            Results_Subscriber.PageSize = IIf(Results_Table.Rows.Count <= RecordsPerPage, Results_Table.Rows.Count, RecordsPerPage)
            Results_Subscriber.DataSource = Results_Table
            Results_Subscriber.DataBind()

          End If

          Subscriber_criteria_results.Text = Results_Table.Rows.Count.ToString + " Results"

          Subscriber_record_count.Text = "Showing 1 - " + IIf(Results_Table.Rows.Count <= RecordsPerPage, Results_Table.Rows.Count.ToString, RecordsPerPage.ToString)

          'This will fill up the dropdown bar with however many pages.
          If Results_Table.Rows.Count > RecordsPerPage Then
            SetPagingButtons(False, True)
          Else
            SetPagingButtons(False, False)
          End If

          SubscriberPanelEx.Collapsed = True

          Results_Table = Nothing

        Else

          Results_Subscription.DataSource = New DataTable
          Results_Subscription.DataBind()

          company_attention.Text = "<br /><p class='padding'><b>No Companies Found. Please refine your search and try again.</b></p><br /><br />"

          Subscriber_criteria_results.Text = "0 Results"

          Subscriber_record_count.Text = "Showing 0 Results"

          SetPagingButtons(False, False)

        End If

        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "updateSubscriber", postBackScript, True)

      End If

      Results_Table = New DataTable

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in Private Sub run_subscriber_search(ByVal bindFromSession As Boolean, ByVal searchCriteria As SearchSelectionCriteria)</b><br /> " + ex.Message

      'Some More Error Catching.
      Master.LogError("Subscriber Search(): Query: " + HttpContext.Current.Session.Item("MasterSubscriber").ToString + " " + ex.Message.ToString)
      company_attention.Text = "<br /><p class='padding'>We're sorry, an error has occurred during your search.</b></p><br /><br />"
      If (InStr(UCase(Session.Item("localUser").crmLocalUserName), "JETNET.COM") > 0) Or (InStr(UCase(Session.Item("localUser").crmLocalUserName), "MVINTECH.COM") > 0) Then
        company_attention.Text += ex.Message.ToString
      End If

    End Try

  End Sub

  Private Sub FillOutSearchParameters()
    Try
      'Filling Back in the Search Criteria.

      'company name
      If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaCompanyName) Then
        If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaCompanyName) Then
          company_name.Text = Session.Item("searchCriteria").SearchCriteriaCompanyName.ToString
        End If
      End If

      If bYachtFlag Then
        If Session.Item("searchCriteria").SearchCriteriaCompanyDisplayInactiveCompanies = True Then
          company_status_flag.Checked = True
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

      'Company Email:
      If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaCompanyEmail) Then
        If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaCompanyEmail) Then
          company_email_address.Text = Session.Item("searchCriteria").SearchCriteriaCompanyEmail.ToString
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

      'Company ID:
      If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaCompanyID) Then
        If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaCompanyID) Then
          company_id.Text = Session.Item("searchCriteria").SearchCriteriaCompanyID.ToString
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

      'Company Contact Email:
      If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaCompanyContactEmail) Then
        If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaCompanyContactEmail) Then
          company_contact_email_address.Text = Session.Item("searchCriteria").SearchCriteriaCompanyContactEmail.ToString
        End If
      End If

      'Company Contact id:
      If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaCompanyContactID) Then
        If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaCompanyContactID) Then
          company_contact_id.Text = Session.Item("searchCriteria").SearchCriteriaCompanyContactID.ToString
        End If
      End If

      'Not In Selected Relationship
      If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaCompanyNotInSelectedRelationship) Then
        If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaCompanyNotInSelectedRelationship) Then
          If Session.Item("searchCriteria").SearchCriteriaCompanyNotInSelectedRelationship.ToString.ToUpper.Contains("TRUE") Then
            comp_not_in_selected.Checked = Session.Item("searchCriteria").SearchCriteriaCompanyNotInSelectedRelationship
          End If
        End If
      End If

      'Company Relationship
      If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaCompanyRelationshipsToAC) Then
        If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaCompanyRelationshipsToAC) Then
          Dim RelationshipSelection As Array
          RelationshipSelection = Split(Session.Item("searchCriteria").SearchCriteriaCompanyRelationshipsToAC, Constants.cCommaDelim)
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

      'Company Business Type:
      If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaCompanyBusinessType) Then
        If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaCompanyBusinessType) Then
          Dim BusinessSelection As Array
          BusinessSelection = Split(Replace(Session.Item("searchCriteria").SearchCriteriaCompanyBusinessType, Constants.cSingleQuote, Constants.cEmptyString), Constants.cCommaDelim)
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

      'Contact Title Group:
      If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaCompanyContactTitle) Then
        If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaCompanyContactTitle) Then
          Dim TitleSelection As Array
          TitleSelection = Split(Replace(Session.Item("searchCriteria").SearchCriteriaCompanyContactTitle, Constants.cSingleQuote, Constants.cEmptyString), Constants.cCommaDelim)
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

      'Service code:
      If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaService_code) Then
        If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaService_code) Then
          Dim serviceCodeSelection As Array
          serviceCodeSelection = Split(Replace(Session.Item("searchCriteria").SearchCriteriaService_code, Constants.cSingleQuote, Constants.cEmptyString), Constants.cCommaDelim)
          service_code_list.SelectedIndex = -1 'This will remove any previously selected items in the listbox, such as the selection of all
          'that the page defaults to.
          For svcCodeSelectionCount = 0 To UBound(serviceCodeSelection)
            For ListBoxCount As Integer = 0 To service_code_list.Items.Count() - 1
              If UCase(service_code_list.Items(ListBoxCount).Value) = UCase(serviceCodeSelection(svcCodeSelectionCount)) Then
                service_code_list.Items(ListBoxCount).Selected = True
              End If
            Next
          Next
        End If
      End If

      If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaParentSub) Then

        If Session.Item("searchCriteria").SearchCriteriaParentSub Then
          parent_subscriptions.SelectedValue = "parent"
        End If

      End If

    Catch ex As Exception
      Master.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString + " : " + ex.Message.ToString)
    End Try
  End Sub

  Public Sub ResetPage()
    clsGeneral.clsGeneral.ClearSavedSelection()
    Response.Redirect("adminSubscribers.aspx", True)
  End Sub

  Private Sub reset_form_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles reset_form.Click
    ResetPage()
  End Sub

  Public Sub load_page_session_variables()

    Try

      ' pick up the "company location values"
      If Not IsNothing(Request.Item("radContinentRegion")) Then
        If Not String.IsNullOrEmpty(Request.Item("radContinentRegion").ToString) And (Request.Item("radContinentRegion").ToString.ToLower <> Session.Item("companyRegionOrContinent").ToString.ToLower) Then
          HttpContext.Current.Session.Item("companyRegionOrContinent") = Request.Item("radContinentRegion").ToString.ToLower
          HttpContext.Current.Session.Item("companyRegion") = ""
          HttpContext.Current.Session.Item("companyCountry") = ""
          HttpContext.Current.Session.Item("companyState") = ""
          HttpContext.Current.Session.Item("companyTimeZone") = ""
        End If
      End If

      If Not IsNothing(Request.Item("cboCompanyRegion")) Then
        If Not String.IsNullOrEmpty(Request.Item("cboCompanyRegion")) And Not Request.Item("cboCompanyRegion").ToString.ToLower.Contains("all") Then
          HttpContext.Current.Session.Item("companyRegion") = Request.Item("cboCompanyRegion").ToString.Trim
        ElseIf Not String.IsNullOrEmpty(Request.Item("cboCompanyRegion")) And Request.Item("cboCompanyRegion").ToString.ToLower.Contains("all") Then
          HttpContext.Current.Session.Item("companyRegion") = ""
        End If
      End If

      If Not IsNothing(Request.Item("cboCompanyCountry")) Then
        If Not String.IsNullOrEmpty(Request.Item("cboCompanyCountry")) And Not Request.Item("cboCompanyCountry").ToString.ToLower.Contains("all") Then
          HttpContext.Current.Session.Item("companyCountry") = Request.Item("cboCompanyCountry").ToString.Trim
        ElseIf Not String.IsNullOrEmpty(Request.Item("cboCompanyCountry")) And Request.Item("cboCompanyCountry").ToString.ToLower.Contains("all") Then
          HttpContext.Current.Session.Item("companyCountry") = ""
        End If
      End If

      If Not IsNothing(Request.Item("cboCompanyState")) Then
        If Not String.IsNullOrEmpty(Request.Item("cboCompanyState")) And Not Request.Item("cboCompanyState").ToString.ToLower.Contains("all") Then
          HttpContext.Current.Session.Item("companyState") = Request.Item("cboCompanyState").ToString.Trim
        ElseIf Not String.IsNullOrEmpty(Request.Item("cboCompanyState")) And Request.Item("cboCompanyState").ToString.ToLower.Contains("all") Then
          HttpContext.Current.Session.Item("companyState") = ""
        End If
      End If

      If Not IsNothing(Request.Item("cboCompanyTimeZone")) Then
        If Not String.IsNullOrEmpty(Request.Item("cboCompanyTimeZone")) And Not Request.Item("cboCompanyTimeZone").ToString.ToLower.Contains("all") Then
          HttpContext.Current.Session.Item("companyTimeZone") = Request.Item("cboCompanyTimeZone").ToString.Trim
        ElseIf Not String.IsNullOrEmpty(Request.Item("cboCompanyTimeZone")) And Request.Item("cboCompanyTimeZone").ToString.ToLower.Contains("all") Then
          HttpContext.Current.Session.Item("companyTimeZone") = ""
        End If
      End If


    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "error in [load_page_session_variables] : " + ex.Message
    End Try

  End Sub

  Private Function postBackScript() As String
    Dim scriptOut As New StringBuilder

    scriptOut.Append(" $(document).ready(function(){" + vbCrLf)

    'scriptOut.Append("alert('(postback)');" + vbCrLf)

    scriptOut.Append("  if (" + HttpContext.Current.Session.Item("companyRegionOrContinent").ToString.Trim.ToLower.Contains("continent").ToString.ToLower + ") {" + vbCrLf)
    scriptOut.Append("    document.getElementById(""radContinentRegionID"").checked = true;" + vbCrLf)
    scriptOut.Append("    document.getElementById(""radContinentRegionID1"").checked = false;" + vbCrLf)
    scriptOut.Append("  } else {" + vbCrLf)
    scriptOut.Append("    document.getElementById(""radContinentRegionID"").checked = false;" + vbCrLf)
    scriptOut.Append("    document.getElementById(""radContinentRegionID1"").checked = true;" + vbCrLf)
    scriptOut.Append("  }" + vbCrLf)

    scriptOut.Append("  whichOneCompany = """ + HttpContext.Current.Session.Item("companyRegionOrContinent").ToString.ToLower + """;" + vbCrLf)

    'scriptOut.Append("alert('(postback)radContinentRegionID[ ' + document.getElementById(""radContinentRegionID"").checked + ' ]');" + vbCrLf)
    'scriptOut.Append("alert('(postback)radContinentRegionID1[ ' + document.getElementById(""radContinentRegionID1"").checked + ' ]');" + vbCrLf)
    'scriptOut.Append("alert('(postback)WhichOne[ ' + whichOneCompany + ' ]');" + vbCrLf)

    scriptOut.Append("  checkRadioButtons(false, false, '" + Session.Item("companyRegion").ToString.Trim + "', '', '', '" + Session.Item("companyCountry").ToString.Trim + "', '', '', '" + Session.Item("companyState").ToString.Trim + "', '', '', '" + Session.Item("companyTimeZone").ToString.Trim + "', '');" + vbCrLf)
    scriptOut.Append("});" + vbCrLf)

    Return scriptOut.ToString

    scriptOut = Nothing
  End Function

  Public Function setForcolor(ByVal comp_id As String, ByVal sub_id As String, ByVal parent_sub_id As String) As String

    If CLng(sub_id) = CLng(parent_sub_id) Then
      Return "<a Class= ""underline"" onclick='javascript:openSmallWindowJS(""homebaseSubscription.aspx?compID=" + comp_id.Trim + "&subID=" + sub_id.Trim + """,""SubscriptionWindow"");' title='Display Subscription Details'><font color=""red"">" + sub_id.Trim + "</font></a>"
    Else
      Return "<a class=""underline"" onclick='javascript:openSmallWindowJS(""homebaseSubscription.aspx?compID=" + comp_id.Trim + "&subID=" + sub_id.Trim + """,""SubscriptionWindow"");' title='Display Subscription Details'>" + sub_id.Trim + "</a>"
    End If

  End Function

  Public Function DisplayContactErrorListing(ByVal sEmailAddress As String)

    Return "<a class=""underline cursor"" onclick=""openSmallWindowJS('adminSubErrors.aspx?email=" + HttpContext.Current.Server.UrlEncode(sEmailAddress.Trim) + "','ErrorWindow');"" title=""Click to see errors for this user"">" + sEmailAddress.Trim + "</a>"

  End Function

  Public Function getServiceUsed() As DataTable
    Dim sql As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim atemptable As New DataTable
    Try

      sql += "SELECT svud_id, svud_desc FROM Services_Used WITH (NOLOCK)"
      sql += " WHERE svud_active_flag = 'Y'"
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

  Public Function getServiceCodes() As DataTable
    Dim sql As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim atemptable As New DataTable
    Try

      sql += "SELECT DISTINCT [Service].serv_code, (SELECT * FROM ReturnServiceFullName(sub_id)) AS service_text FROM [Service] WITH (NOLOCK)"
      sql += " INNER JOIN View_JETNET_Customers WITH (NOLOCK) ON sub_serv_code = [Service].serv_code"
      sql += " ORDER BY service_text"

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