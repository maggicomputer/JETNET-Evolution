Partial Public Class Yacht_View_Master
  Inherits System.Web.UI.UserControl
  Dim View_ID As Integer = 0
  Dim View_Name As String = ""
  Dim MasterPage As New Object 'YachtTheme
  Dim PageSort As String = ""

  Private bClearView As Boolean = False
  Private bIsReport As Boolean = False

  Private bUseLoggedInUser As Boolean = True

  Private sYachtCategoryModelCtrlBaseName As String = "YachtView"

  Private localDatalayer As yachtViewDataLayer
  Private localCriteria As New yachtViewSelectionCriteria

  Dim sReportOutputFilename As String = ""
  Dim sReportFrom As String = ""

  Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

    load_yacht_view_session_variables()

  End Sub

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    'MasterPage = DirectCast(Page.Master, YachtTheme) ' Reference to the master page

    If Session.Item("localUser").crmEvo = True Then
      If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO Then
        MasterPage = DirectCast(Page.Master, EvoTheme) ' Reference to the master page
      ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT Then
        MasterPage = DirectCast(Page.Master, YachtTheme) ' Reference to the master page
      End If
    End If

    MasterPage.Set_Active_Tab(1)

    If Not IsNothing(Request.Item("ViewID")) Then
      If Not String.IsNullOrEmpty(Request.Item("ViewID").ToString.Trim) Then
        Session("View_ID") = Request.Item("ViewID").ToString
        If IsNumeric(Request.Item("ViewID").ToString) Then
          localCriteria.YachtViewID = CLng(Request.Item("ViewID").ToString)
        End If

      End If
    End If

    If Not IsNothing(Request.Item("ViewName")) Then
      If Not String.IsNullOrEmpty(Request.Item("ViewName").ToString.Trim) Then
        localCriteria.YachtViewName = Server.UrlEncode(Request.Item("ViewName").ToString.Trim)
      End If
    End If

    Dim sTmpHull As String = ""
    Dim sTmpCategory As String = ""
    Dim sTmpBrandModel As String = ""
    Dim activetab As Integer = 0
    Dim sTmpCompany As String = ""
    Dim charting_string As String = ""

    If Session.Item("crmUserLogon") <> True Then
      Response.Redirect("Default.aspx", False)

    Else

      localDatalayer = New yachtViewDataLayer
      localDatalayer.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

      Dim sErrorString As String = ""

      If Not Session.Item("localPreferences").loadUserSession(sErrorString, CLng(Session.Item("localUser").crmSubSubID.ToString), Session.Item("localUser").crmUserLogin.ToString, CLng(Session.Item("localUser").crmSubSeqNo.ToString), CLng(Session.Item("localUser").crmUserContactID.ToString)) Then
        Response.Write("error in load yacht view : " + sErrorString)
      End If

      localDatalayer.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
      localDatalayer.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
      localDatalayer.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
      localDatalayer.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim

      load_page_variables()

      Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title(View_Name)

      Select Case CInt(localCriteria.YachtViewCriteriaMotorType)
        Case Constants.VIEW_MOTORHULL
          sTmpHull = "Motor Hull - "
        Case Constants.VIEW_SAILHULL
          sTmpHull = "Sail Hull - "
      End Select

      Select Case localCriteria.YachtViewCriteriaYachtCategory
        Case Constants.YMOD_TYPE_GIGA
          sTmpCategory = "Giga - "
        Case Constants.YMOD_TYPE_MEGA
          sTmpCategory = "Mega - "
        Case Constants.YMOD_TYPE_SUPER
          sTmpCategory = "Super - "
        Case Constants.YMOD_TYPE_LUXURY
          sTmpCategory = "Luxury - "
      End Select

      'If localCriteria.ViewCriteriaCompanyID > 0 Then
      '  sTmpCompany = "<a href='DisplayCompanyDetail.aspx?compid=" + localCriteria.ViewCriteriaCompanyID.ToString + "' target='_new' class='underline'>" + commonEvo.get_company_name_fromID(localCriteria.ViewCriteriaCompanyID, 0, False, True, "") + "</a> - "
      'End If

      If localCriteria.YachtViewCriteriaYmodID > -1 Then
        sTmpBrandModel = "<a href='DisplayYachtDetail.aspx?yid=" + localCriteria.YachtViewCriteriaYmodID.ToString + "&jid=0' target='_new' class='underline'>" + commonEvo.Get_Yacht_Model_Info(localCriteria.YachtViewCriteriaYmodID, False) + "</a> - "
      ElseIf localCriteria.YachtViewCriteriaBrandID > -1 Then
        sTmpBrandModel = "<a href='DisplayYachtDetail.aspx?yid=" + localCriteria.YachtViewCriteriaBrandID.ToString + "&jid=0' target='_new' class='underline'>" + commonEvo.Get_Yacht_Model_Info(localCriteria.YachtViewCriteriaBrandID, True) + "</a> - "
      End If

      breadcrumbs1.Text = "<strong>" + sTmpHull + sTmpCategory + sTmpBrandModel + sTmpCompany + View_Name.Trim + "</strong>"

      atGlanceGo.PostBackUrl = "~/Yacht_View_Template.aspx?ViewID=" + localCriteria.YachtViewID.ToString + "&ViewName=" + localCriteria.YachtViewName.Trim
      atGlanceClear.PostBackUrl = "~/Yacht_View_Template.aspx?ViewID=" + localCriteria.YachtViewID.ToString + "&ViewName=" + localCriteria.YachtViewName.Trim + "&clear=true"

      Me.loaded_visibility1.Visible = True
      divLoading.Visible = False
      loaded_visibility1.CssClass = "display_block"

      ' clear these 2
      HttpContext.Current.Session.Item("MasterCompanyFrom") = ""
      HttpContext.Current.Session.Item("MasterCompanyWhere") = ""
      HttpContext.Current.Session.Item("MasterContactWhere") = ""

      If check_if_can_export() = True And (View_ID <> 0 And View_ID <> 16 And View_ID <> 22 And View_ID <> 25) Then ' not industry at a glance  
        Me.lower_actions_submenu_dropdown.Items.Clear()
        Me.lower_actions_submenu_dropdown.Items.Add(New ListItem("Custom Export", "javascript:SubMenuDrop(1,0, ""YACHT COMPANY"");"))
        If View_ID = 21 Then
          If Not IsPostBack Then
            If Trim(Request("activetab")) <> "" Then
              activetab = Trim(Request("activetab"))
            End If
          End If

          If activetab = 1 Or activetab = 100 Then
            Me.lower_actions_submenu_dropdown.Items.Add(New ListItem("Individuals Owning/Leasing Yachts/AC", "javascript:SubMenuDrop(1,0, ""YACHT COMPANY EXPORT"");"))
          End If
        End If
        ToggleVisibilityOfActionMenu(lower_actions_dropdown, lower_actions_submenu_dropdown)
      Else
        Me.lower_actions_dropdown.Visible = False
        Me.lower_actions_submenu_dropdown.Visible = False
      End If

      cellNotesSearchPnl.Visible = False

      If Me.lower_actions_submenu_dropdown.Items.Count > 1 Then
        Me.lower_actions_submenu_dropdown.Items.RemoveAt(1)
        ToggleVisibilityOfActionMenu(lower_actions_dropdown, lower_actions_submenu_dropdown)
      End If

      Select Case View_ID

        Case 0, 16, 22 'Industry at a Glance

          displayYachtView16()
          yacht_view16.Visible = True

        Case 17

          displayYachtView17()
          yacht_view17.Visible = True

        Case 20
          displayYachtView20_mfr()
          yacht_view18.Visible = True

        Case 21

          If IsNothing(Session("Last_Yacht_Crossover_Tab")) Then
            Session("Last_Yacht_Crossover_Tab") = 0
          End If

          If Not IsPostBack Then
            If Trim(Request("activetab")) <> "" Then
              activetab = Trim(Request("activetab"))
            End If

            If activetab = 1 Or activetab = 100 Then
              Me.lower_actions_submenu_dropdown.Items.Add(New ListItem("Individuals Owning/Leasing Yachts/AC - AC List", "javascript:SubMenuDrop(1,0, ""YACHT COMPANY EXPORT"");"))
              Me.lower_actions_submenu_dropdown.Items.Add(New ListItem("Individuals Owning/Leasing Yachts/AC - Yacht List", "javascript:SubMenuDrop(1,0, ""YACHT COMPANY YACHT EXPORT"");"))

              If Trim(Request("amod_id")) <> "" Then
                If CInt(Trim(Request("amod_id"))) > 0 Then
                  Me.lower_actions_submenu_dropdown.Items.Add(New ListItem("Individuals Owning " & Trim(Request("model_name")) & ", Not Yachts", "javascript:SubMenuDrop(1,0, ""YACHT COMPANY MODEL EXPORT"");"))
                End If
              End If
            ElseIf activetab = 3 Then
              Me.lower_actions_submenu_dropdown.Items.Add(New ListItem("Individuals Owning/Leasing Yachts, Not Aircraft", "javascript:SubMenuDrop(1,0, ""YACHT COMPANY YACHT EXPORT"");"))
            ElseIf activetab = 5 Then
              Me.lower_actions_submenu_dropdown.Items.Add(New ListItem("Individuals Owning/Leasing AC not Yachts - AC List", "javascript:SubMenuDrop(1,0, ""YACHT COMPANY NO YACHT EXPORT"");"))
            End If

            If Trim(Request("amod_id")) <> "" And Trim(Request("amod_id")) <> "0" Then
              If HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag Or HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag Or HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag Then
                Me.lower_actions_submenu_dropdown.Items.Add(New ListItem("View Aircraft Model Specs", "javascript:load('../DisplayModelDetail.aspx?id=" & Trim(Request("amod_id")) & "','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');"))
              End If
            End If
          End If



          'Remove the "New Search" link since we do not allow them to refine their search for this view.
          ControlImage1.CssClass = "display_none"


          Session("Yacht_Crossover_Select") = ""
          Session("Yacht_Crossover_Model_Select") = ""
          Session("Yacht_Crossover_Yacht_Select") = ""

          If (Me.view_21_tabcontain.ActiveTabIndex = 1 Or Me.view_21_tabcontain.ActiveTabIndex = 2) And IsPostBack Then
            'this is for re-loading the second tab
            HttpContext.Current.Session.Item("MasterCompanyWhere") = HttpContext.Current.Session.Item("MasterCompanyWhere_Tab1")
            view_21_1_list.Text = Session("YACHT_AC_Make_List")    ' make list
            view_21_1_list2.Text = Session("YACHT_AC_Model_List")   ' make model list
            view_21_1.Text = Session("YACHT_AC_Company_List") ' company list 
            view_21_tab1.HeaderText = Session("YACHT_AC_Company_List_HEADER")

            DisplayFunctions.load_google_chart(graph_panel, Session("YACHT_AC_Type"), "", "Model Count", "chart_div_tab1_all", 220, 220, "ARRAY", 1, charting_string, Me.Page, Me.graph_update_panel, False, False, False, False, False, True, False, False, False, True, Me.view_21_tabcontain.ActiveTabIndex)

            DisplayFunctions.load_google_chart(graph_panel, Session("YACHT_AC_Weight"), "", "", "chart_div_tab1_all_2", 220, 220, "ARRAY", 2, charting_string, Me.Page, Me.graph_update_panel, False, False, False, False, False, True, False, False, False, True, Me.view_21_tabcontain.ActiveTabIndex)

            '--------------------- BUILD CHART-----------------------
            Call load_google_chart_all(view_21_tab1, charting_string)
            '--------------------- BUILD CHART-----------------------
            Me.view_21_1.Visible = True
            Me.view_21_hide_left.Visible = True
            Me.view_21_tab100.Visible = False
            Me.tplist1.HeaderText = "Aircraft Makes Owned"
            Me.tplist2.HeaderText = "Aircraft Models Owned"
            Me.graph_title_label.Text = "<tr valign='top'><td width='50%' align='center'>Aircraft Types Owned </td><td align='center'>Jets Owned by Size</td></tr>"
          Else
            displayYachtView21_cross(activetab)

            If activetab = 1 Or activetab = 2 Then
              HttpContext.Current.Session.Item("MasterCompanyWhere_Tab1") = HttpContext.Current.Session.Item("MasterCompanyWhere")
              Session("YACHT_AC_Make_List") = view_21_1_list.Text   ' make list
              Session("YACHT_AC_Model_List") = view_21_1_list2.Text ' make model list 
              Session("YACHT_AC_Company_List") = view_21_1.Text ' company list 
              Session("YACHT_AC_Company_List_HEADER") = view_21_tab1.HeaderText
            End If
          End If

          yacht_view21.Visible = True

        Case 23
 
          If Not IsPostBack Then
            displayyachtview23_central()
            yacht_view23.Visible = True
          End If

        Case 25

          cellNotesSearchPnl.Visible = True

          ' certian items display 
          Dim bHasStandardCloudNotes As Boolean = HttpContext.Current.Session.Item("localPreferences").HasCloudNotes
          Dim bHasServerNotes As Boolean = HttpContext.Current.Session.Item("localPreferences").HasServerNotes

          Dim notes_functions As New notes_view_functions
          notes_functions.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
          notes_functions.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
          notes_functions.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
          notes_functions.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
          notes_functions.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim

          localCriteria.YachtViewCriteriaSubID = CLng(HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString)
          localCriteria.YachtViewCriteriaLogin = HttpContext.Current.Session.Item("localUser").crmUserLogin.ToString.Trim

          If bUseLoggedInUser Then

            localCriteria.YachtViewCriteriaNoteUserID = CLng(HttpContext.Current.Session.Item("localUser").crmUserContactID) ' automaticly fill in for cloud notes

            If bHasServerNotes Then 'If they're notes plus (server notes) users. 

              Dim tempTable As New DataTable
              Dim tmpPrefobj As New preferencesDataLayer

              tmpPrefobj.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

              tempTable = tmpPrefobj.ReturnUserDetailsAndImage(localCriteria.YachtViewCriteriaNoteUserID)

              If Not IsNothing(tempTable) Then
                If tempTable.Rows.Count > 0 Then

                  For Each r As DataRow In tempTable.Rows

                    If Not (IsDBNull(r.Item("contact_email_address"))) And Not String.IsNullOrEmpty(r.Item("contact_email_address").ToString) Then
                      localCriteria.YachtViewCriteriaNoteUserID = commonEvo.get_crm_client_info(r.Item("contact_email_address").ToString.Trim, "", "", "")
                    End If

                  Next

                End If

              End If

              tempTable = Nothing
              tmpPrefobj = Nothing

            End If

          End If


          ' fill in notesSearch_who          
          notes_functions.views_fill_notesUserDropdown(Nothing, 0, notesSearch_who, localCriteria)

          ' fill in notesSearch_display_cbo
          notes_functions = Nothing

          yacht_viewHeaderLabel.Text = "Notes Center"
          Build_Notes_tab(localCriteria, yacht_viewContentLabel.Text)

          yacht_view25.Visible = True

      End Select

      If Trim(HttpContext.Current.Session.Item("MasterCompanyFrom")) = "COMPANY" Then
        HttpContext.Current.Session.Item("MasterCompanyFrom") = " From Company "
        HttpContext.Current.Session.Item("MasterCompanyFrom") &= " LEFT OUTER JOIN Contact WITH(NOLOCK) ON (comp_id = contact_comp_id AND comp_journ_id = contact_journ_id and contact_hide_flag = 'N' and contact_active_flag = 'Y') "
      ElseIf Trim(HttpContext.Current.Session.Item("MasterCompanyFrom")) = "CONTACT" Then
        HttpContext.Current.Session.Item("MasterCompanyFrom") = " From Company "
        HttpContext.Current.Session.Item("MasterCompanyFrom") &= " LEFT OUTER JOIN Contact WITH(NOLOCK) ON (comp_id = contact_comp_id AND comp_journ_id = contact_journ_id and contact_hide_flag = 'N' and contact_active_flag = 'Y') "
      End If

      HttpContext.Current.Session.Item("MasterCompanyWhere") = " WHERE COMP_ID IN(" & Trim(HttpContext.Current.Session.Item("MasterCompanyWhere")) & ")"
      HttpContext.Current.Session.Item("MasterCompanyWhere") &= " and comp_journ_id = 0 "
      PanelCollapseEx1.Collapsed = True
      PanelCollapseEx1.ClientState = "True"

      viewCBMDropDowns.setIsView(True)

      If View_ID = 16 Or View_ID = 17 Or View_ID = 22 Or View_ID = 25 Then
        viewCBMDropDowns.setOverideMultiSelect(True)
      Else
        viewCBMDropDowns.setOverideMultiSelect(False)
      End If

      viewCBMDropDowns.setListSize(6)
      viewCBMDropDowns.setControlName(sYachtCategoryModelCtrlBaseName)

      End If

  End Sub

  Private Sub displayYachtView21_cross(ByVal active_tab As Integer)
    Dim HoldTable_years As New DataTable
    Dim HoldTable As New DataTable
    Dim string_for_all_brands As String = ""
    Dim string_for_all_sizes As String = ""
    Dim temp_year_chart As String = ""
    Dim this_company_name As String = ""
    Dim temp_view_name As String = ""
    Dim comp_link As String = ""
    Dim temp_comp As String = ""
    Dim toggleRowColor As Boolean = True
    Dim add_comma As Boolean = False
    Dim has_location As Boolean = False
    Dim last_contact_name As String = ""
    Dim table_count As Integer = 0
    Dim last_comp_id As Long = 0 
    Dim count_yacht_compaies As Integer = 0
    Dim type_of As String = ""
    Dim amod_id As Integer = 0

    Dim google_map_string As String = ""
    Dim charting_string As String = ""
    Dim company_id_string As String = ""
    Dim make_name As String = ""
    Dim contact_id_string As String = ""
    Dim related_contact_id_string As String = ""
    Dim temp_contact_list As Array
    Dim i As Integer = 0
    Dim yacht_size As String = ""


    Try
      Me.div_for_yacht_size.Visible = False

      view_21_0.Text = ""
      type_of = Trim(Request("type_of"))

      If Not IsPostBack Then
        If Trim(Request("amod_id")) <> "" Then
          amod_id = Trim(Request("amod_id"))
        End If
      End If

      If Not IsPostBack Then
        If Trim(Request("make_name")) <> "" Then
          make_name = Trim(Request("make_name"))
        End If
      End If

      If Not IsPostBack Then
        If Trim(Request("yacht_size")) <> "" Then
          yacht_size = Trim(Request("yacht_size"))
        End If
      End If 
      'view_21_0.Text &= "<ul>"


      'view_21_0.Text &= "<li>"
      'view_21_0.Text &= "<a href='Yacht_View_Template.aspx?ViewID=21&ViewName=Yacht/Aircraft Industry Crossovers&activetab=1'>"
      'view_21_0.Text &= "<b>Individuals Owning Yachts/AC</b>"
      'view_21_0.Text &= "</a>"

      'view_21_0.Text &= "<ul>"
      'view_21_0.Text &= "<li>"
      'view_21_0.Text &= "This view includes individuals who own both yachts and aircraft. "
      'view_21_0.Text &= "</li>"
      'view_21_0.Text &= "</ul>"

      'view_21_0.Text &= "</li>"

      'view_21_0.Text &= "<li>"
      'view_21_0.Text &= "<a href='Yacht_View_Template.aspx?ViewID=21&ViewName=Yacht/Aircraft Industry Crossovers&activetab=2'>"
      'view_21_0.Text &= "<b>Companies Owning Yachts/AC</b>"
      'view_21_0.Text &= "</a>"

      'view_21_0.Text &= "<ul>"
      'view_21_0.Text &= "<li>"
      ''view_21_0.Text &= "This view includes active companies owning both a yacht and aircraft. Parent company and affiliates are not considered."
      'view_21_0.Text &= "This view includes active companies owning both a yacht and aircraft.Note that this view also includes the company if their affiliates own the aircraft."
      'view_21_0.Text &= "</li>"
      'view_21_0.Text &= "</ul>"

      'view_21_0.Text &= "</li>"



      'view_21_0.Text &= "<li>"
      'view_21_0.Text &= "<a href='Yacht_View_Template.aspx?ViewID=21&ViewName=Yacht/Aircraft Industry Crossovers&activetab=3'>"
      'view_21_0.Text &= "<b>Companies Owning Yachts not AC</b>"
      'view_21_0.Text &= "</a>"

      'view_21_0.Text &= "<ul>"
      'view_21_0.Text &= "<li>"
      'view_21_0.Text &= "This view includes a list of companies owning yachts that do not own an aircraft. Parent company and affiliates are not considered."
      'view_21_0.Text &= "</li>"
      'view_21_0.Text &= "</ul>"

      'view_21_0.Text &= "</li>"


      'view_21_0.Text &= "<li>"
      'view_21_0.Text &= "<a href='Yacht_View_Template.aspx?ViewID=21&ViewName=Yacht/Aircraft Industry Crossovers&activetab=4'>"
      'view_21_0.Text &= "<b>Companies in Yacht/AC Business</b>"
      'view_21_0.Text &= "</a>"

      'view_21_0.Text &= "<ul>"
      'view_21_0.Text &= "<li>"
      'view_21_0.Text &= "This view includes a list of companies who do business in both the aviation and yacht industries but are generally not owners/end users of a yacht or aircraft."
      'view_21_0.Text &= "</li>"
      'view_21_0.Text &= "</ul>"

      'view_21_0.Text &= "</li>"


      'view_21_0.Text &= "<li>"
      'view_21_0.Text &= "<a href='Yacht_View_Template.aspx?ViewID=21&ViewName=Yacht/Aircraft Industry Crossovers&activetab=5'>"
      'view_21_0.Text &= "<b>Individuals Owning AC not Yachts</b>"
      'view_21_0.Text &= "</a>"

      'view_21_0.Text &= "<ul>"
      'view_21_0.Text &= "<li>"
      'view_21_0.Text &= "This view includes individuals that own or lease an aircraft, but do not own or lease a yacht. Note that the list includes owners of all types of aircraft including Jets, Turboprops, Pistons, & Helicopters."
      'view_21_0.Text &= "</li>"
      'view_21_0.Text &= "</ul>"

      'view_21_0.Text &= "</li>"


      'view_21_0.Text &= "</ul>"
      If active_tab > 0 Then
        Me.view_21_tabcontain.ActiveTabIndex = active_tab
      End If

      '  view_21_0.Text &= "<hr />"

      CreatePrototypeDatatables(HoldTable, 1) 'Can be completely deleted once style is all set.
      view_21_0.Text &= localDatalayer.Display_Crossover_Formatted_Table(HoldTable, "", "Yachts &amp; Aircraft", "/images/plane_icon.png", "/images/yacht_icon.png", "FIELD", "COUNT", "TAB", "TYPE_OF", False)

      HoldTable = New DataTable
      CreatePrototypeDatatables(HoldTable, 2) 'Can be completely deleted once style is all set.
      view_21_0.Text &= localDatalayer.Display_Crossover_Formatted_Table(HoldTable, "", "Aircraft Not Yachts", "/images/plane_icon.png", "/images/yacht_icon.png", "FIELD", "COUNT", "TAB", "TYPE_OF", True)

      HoldTable = New DataTable
      CreatePrototypeDatatables(HoldTable, 3) 'Can be completely deleted once style is all set.
      view_21_0.Text &= localDatalayer.Display_Crossover_Formatted_Table(HoldTable, "", "Yachts Not Aircraft", "/images/yacht_icon.png", "/images/plane_icon.png", "FIELD", "COUNT", "TAB", "TYPE_OF", True)

      view_21_tab0.HeaderText = "Crossover Summary"
      'view_21_tab1.HeaderText = "Individuals Owning Yachts/AC"
      ' view_21_tab2.HeaderText = "Companies Owning Yachts/AC"
      ' view_21_tab3.HeaderText = "Companies Owning Yachts not AC"
      ' view_21_tab4.HeaderText = "Companies in Yacht/AC Business"
      ' view_21_tab5.HeaderText = "Individuals Owning AC not Yachts" 

      'If (CInt(Session("Last_Yacht_Crossover_Tab")) <> CInt(Me.view_21_tabcontain.ActiveTabIndex)) Then

      If Me.view_21_tabcontain.ActiveTabIndex = 1 Or Me.view_21_tabcontain.ActiveTabIndex = 100 Or Me.view_21_tabcontain.ActiveTabIndex = 3 Then
        view_21_1.Text = ""
        view_21_3.Text = ""

 

        Session("Yacht_Crossover_Select") = ""
        Session("Yacht_Crossover_Select") &= " select ContactFullName, Contact.contact_title as Title, Contact.contact_email_address as EmailAddress, "
        Session("Yacht_Crossover_Select") &= " contact_phone_office as OfficePhone, contact_phone_mobile as MobilePhone, "
        Session("Yacht_Crossover_Select") &= " Company as CompanyName, comp_address1 as Address1, comp_address2 as Address2, "
        Session("Yacht_Crossover_Select") &= " comp_city as City, comp_state as State, comp_zip_code as ZipCode, comp_country as Country, "
        Session("Yacht_Crossover_Select") &= " comp_email_address as CompanyEmail,"
        Session("Yacht_Crossover_Select") &= " case amod_airframe_type_code when 'R' then 'Helicopter' else 'Aircraft' end as ACType,"
        Session("Yacht_Crossover_Select") &= " atype_name as ACType2, acwgtcls_name as WeightClass, "
        Session("Yacht_Crossover_Select") &= " amod_make_name as Make, amod_model_name as Model, ac_ser_no_full as SerNo,"
        Session("Yacht_Crossover_Select") &= " case ac_ownership_type when 'W' then 'Whole Owner' when 'S' then 'Shared Owner' else 'Fractional' end as Ownership, "
        Session("Yacht_Crossover_Select") &= " case when cref_owner_percent=0 then 100 when cref_owner_percent > 0 then cref_owner_percent else 100 end as PercentOwned"
        Session("Yacht_Crossover_Select") &= " from Yacht_Crossover_Table with (NOLOCK)"
        Session("Yacht_Crossover_Select") &= " inner join Contact with (NOLOCK) on Contact.contact_id = contactid and contact_journ_id = 0   " ' added since contacts for yacht related contacts are not going out into view aircraft company flat 
        Session("Yacht_Crossover_Select") &= " inner join View_Aircraft_Company_Flat with (NOLOCK) on ContactId = View_Aircraft_Company_Flat.contact_id and cref_journ_id = 0"
        Session("Yacht_Crossover_Select") &= " inner join Aircraft_Weight_Class with (NOLOCK) on acwgtcls_code = amod_weight_class and acwgtcls_airframe_type_code = amod_airframe_type_code and acwgtcls_maketype = amod_type_code "
        Session("Yacht_Crossover_Select") &= " and cref_contact_type in ('00','08','97')"
 
        Session("Yacht_Crossover_Select") &= " where Num_Aircraft > 0 " 


        If Me.view_21_tabcontain.ActiveTabIndex = 100 Then
          If amod_id > 0 Or Trim(make_name) <> "" Then
            Session("Yacht_Crossover_Select") &= " and comp_id in (XXXCOMP_IDXXX) "
          End If
        End If

        Session("Yacht_Crossover_Select") &= " order by ContactGroup"


        Session("Yacht_Crossover_Yacht_Select") = " select "
        If Me.view_21_tabcontain.ActiveTabIndex = 3 Or Me.view_21_tabcontain.ActiveTabIndex = 100 Then
          Session("Yacht_Crossover_Yacht_Select") &= " distinct "
        End If
        Session("Yacht_Crossover_Yacht_Select") &= "  ContactFullName, contact_title as Title, contact_email_address as EmailAddress,"
        Session("Yacht_Crossover_Yacht_Select") &= " Company as CompanyName, comp_address1 as Address1, comp_address2 as Address2,  comp_city as City, "
        Session("Yacht_Crossover_Yacht_Select") &= " comp_state as State, comp_zip_code as ZipCode, comp_country as Country,  comp_email_address as CompanyEmail,"
        Session("Yacht_Crossover_Yacht_Select") &= " ym_brand_name as BrandName,ym_model_name as ModelName, yt_yacht_name as YachtName, yt_hull_mfr_nbr as HULL#, yt_year_mfr as YearMFR "
        Session("Yacht_Crossover_Yacht_Select") &= " , case ym_motor_type when 'M' then 'Motor' else 'Sailing' end as YACHT_TYPE "
        Session("Yacht_Crossover_Yacht_Select") &= " , case ym_category_size when 'G' then 'Giga' when 'M' then 'Mega'  when 'S' then 'Super'  when 'L' then 'Luxury' else '' end as CATSIZE"
        Session("Yacht_Crossover_Yacht_Select") &= " , yt_length_overall_meters as LENGTH_METERS "
        Session("Yacht_Crossover_Yacht_Select") &= " , case yt_length_overall_meters when 0 then 0 else round((yt_length_overall_meters * 3.28084), 2) end as LENGTH_FEET "
        Session("Yacht_Crossover_Yacht_Select") &= " from Yacht_Crossover_Table with (NOLOCK)  "
        Session("Yacht_Crossover_Yacht_Select") &= " inner join View_Yacht_Company_Flat with (NOLOCK) on ContactId = contact_id and yt_journ_id = 0 "

        If Me.view_21_tabcontain.ActiveTabIndex = 3 Then
          Session("Yacht_Crossover_Yacht_Select") &= " where Num_Aircraft = 0 "
        Else
          Session("Yacht_Crossover_Yacht_Select") &= " where Num_Aircraft > 0 "
        End If

        If Me.view_21_tabcontain.ActiveTabIndex = 100 Then
          If amod_id > 0 Or Trim(make_name) <> "" Then
            Session("Yacht_Crossover_Yacht_Select") &= " and (contact_id in (XXXCONTACT_IDXXX) or contact_id in (XXXCONTACT_ID_LISTXXX)) "
          End If
        ElseIf Me.view_21_tabcontain.ActiveTabIndex = 3 Then
          Session("Yacht_Crossover_Yacht_Select") &= " and (contact_id in (XXXCONTACT_IDXXX)) "
        End If

        ' and CompId = yr_comp_id 
        Session("Yacht_Crossover_Yacht_Select") &= " and  yr_contact_type  in ('00','08','97') "

        If Me.view_21_tabcontain.ActiveTabIndex = 3 Or Me.view_21_tabcontain.ActiveTabIndex = 100 Then
          Session("Yacht_Crossover_Yacht_Select") &= "  order by ContactFullName, contact_title, contact_email_address, Company, comp_address1, comp_address2,  comp_city,  comp_state, comp_zip_code, comp_country,  comp_email_address, ym_brand_name,ym_model_name, yt_yacht_name, yt_hull_mfr_nbr, yt_year_mfr, YACHT_TYPE, CATSIZE, yt_length_overall_meters "
        Else
          Session("Yacht_Crossover_Yacht_Select") &= "  order by ContactGroup "
        End If




        Session("Yacht_Crossover_Model_Select") = ""
        If amod_id > 0 Then
          Session("Yacht_Crossover_Model_Select") = " select contact_first_name + ' ' + contact_last_name as  ContactFullName, contact_title as Title, contact_email_address as EmailAddress, "
          Session("Yacht_Crossover_Model_Select") &= " contact_phone_office as OfficePhone, contact_phone_mobile as MobilePhone, "
          Session("Yacht_Crossover_Model_Select") &= " Comp_name as CompanyName, comp_address1 as Address1, comp_address2 as Address2, "
          Session("Yacht_Crossover_Model_Select") &= " comp_city as City, comp_state as State, comp_zip_code as ZipCode, comp_country as Country, "
          Session("Yacht_Crossover_Model_Select") &= " comp_email_address as CompanyEmail,"
          Session("Yacht_Crossover_Model_Select") &= " case amod_airframe_type_code when 'R' then 'Helicopter' else 'Aircraft' end as ACType,"
          Session("Yacht_Crossover_Model_Select") &= " amod_make_name as Make, amod_model_name as Model, ac_ser_no_full as SerNo,"
          Session("Yacht_Crossover_Model_Select") &= " case ac_ownership_type when 'W' then 'Whole Owner' when 'S' then 'Shared Owner' else 'Fractional' end as Ownership, "
          Session("Yacht_Crossover_Model_Select") &= " case when cref_owner_percent=0 then 100 when cref_owner_percent > 0 then cref_owner_percent else 100 end as PercentOwned"

          Session("Yacht_Crossover_Model_Select") &= " from View_Aircraft_Company_Flat with (NOLOCK) "
          Session("Yacht_Crossover_Model_Select") &= " where  cref_contact_type in ('00','08','97') and amod_id = " & amod_id & " and ac_journ_id = 0  and contact_id > 0  "
          ' Session("Yacht_Crossover_Model_Select") &= " and not exists("
          ' Session("Yacht_Crossover_Model_Select") &= " select Yacht_Crossover_Table.compid from Yacht_Crossover_Table where Yacht_Crossover_Table.compid = View_Aircraft_Company_Flat.comp_id "
          ' Session("Yacht_Crossover_Model_Select") &= " )" 

          If Me.view_21_tabcontain.ActiveTabIndex = 100 Then
            If amod_id > 0 Or Trim(make_name) <> "" Then
              Session("Yacht_Crossover_Model_Select") &= " and comp_id not in (XXXCOMP_IDXXX) "
            End If
          End If

          Session("Yacht_Crossover_Model_Select") &= " order by ContactFullName asc "
        End If




        If Me.view_21_tabcontain.ActiveTabIndex = 1 Or Me.view_21_tabcontain.ActiveTabIndex = 100 Then
          table_count = localDatalayer.get_yacht_crossover_table_count(1, type_of, amod_id, make_name, yacht_size)
          HoldTable = localDatalayer.get_yacht_crossover_companies(True, type_of, amod_id, make_name, yacht_size)
        Else
          table_count = localDatalayer.get_yacht_crossover_table_count(11, type_of, 0, make_name, yacht_size)
          HoldTable = localDatalayer.get_yacht_crossover_companies(False, type_of, 0, make_name, yacht_size)
        End If

        If Not IsNothing(HoldTable) Then
          If Me.view_21_tabcontain.ActiveTabIndex = 1 Then
            temp_comp = "<div valign=""top"" style='height:550px; overflow: auto;'>"
          ElseIf Me.view_21_tabcontain.ActiveTabIndex = 100 Then
            temp_comp = "<div valign=""top"" style='height:280px; overflow: auto;'>"
          ElseIf Me.view_21_tabcontain.ActiveTabIndex = 3 And Trim(yacht_size) = "" Then
            temp_comp = "<div valign=""top"" style='height:720px; overflow: auto;'>"
          ElseIf Me.view_21_tabcontain.ActiveTabIndex = 3 And Trim(yacht_size) <> "" Then
            temp_comp = "<div valign=""top"" style='height:720px; overflow: auto;'>"
          Else
            temp_comp = "<div valign=""top"" style='height:620px; overflow: auto;'>"
          End If

          temp_comp += "<table width='100%' cellpadding='3' cellspacing='0' class='data_aircraft_grid'>"
          temp_comp += "<tr class='header_row'>"

          If Me.view_21_tabcontain.ActiveTabIndex = 1 Or Me.view_21_tabcontain.ActiveTabIndex = 100 Then
            temp_comp += "<td align='left' valign='top'><b>Individuals Owning/Leasing Yachts/AC (" & table_count & ")</b></td>"
          Else
            If Trim(type_of) = "" Then
              temp_comp += "<td align='left' valign='top'><b>Individuals Owning/Leasing Yachts, Not Aircraft</b></td>"
            Else
              temp_comp += "<td align='left' valign='top'><b>Individuals Owning/Leasing Yachts, Not Aircraft - Previously Owning Aircraft</b></td>"
            End If
          End If
          temp_comp += "</tr>"

          If amod_id > 0 Or Trim(make_name) <> "" Then
            If Trim(make_name) <> "" Then
              view_21_tab100.HeaderText = "Also Owning " & Trim(make_name) & " (" & table_count & ")"
            Else
              view_21_tab100.HeaderText = "Also Owning " & Trim(Request("model_name")) & " (" & table_count & ")"
            End If

            view_21_tab1.HeaderText = "Individuals Owning/Leasing Yachts/AC"
          Else
            If Me.view_21_tabcontain.ActiveTabIndex = 1 Or Me.view_21_tabcontain.ActiveTabIndex = 100 Then
              view_21_tab1.HeaderText = "Individuals Owning/Leasing Yachts/AC (" & table_count & ")"
            Else
              If Trim(type_of) = "" Then
                view_21_tab1.HeaderText = "Individuals Owning/Leasing Yachts, Not Aircraft (" & table_count & ")"
              Else
                view_21_tab1.HeaderText = "Individuals Owning/Leasing Yachts, Not Aircraft - Previously Owning Aircraft (" & table_count & ")"
              End If
            End If
          End If

          view_21_tab1.Visible = True
          'view_21_tab1.HeaderText = "Individuals with Yachts & Aircraft (" & HoldTable.Rows.Count & ") "

          If HoldTable.Rows.Count > 0 Then
            For Each Row As DataRow In HoldTable.Rows

              has_location = False


              HttpContext.Current.Session.Item("MasterCompanyFrom") = "COMPANY"
              If Trim(HttpContext.Current.Session.Item("MasterCompanyWhere")) <> "" Then
                HttpContext.Current.Session.Item("MasterCompanyWhere") &= ", " & Row.Item("CompId")
              Else
                HttpContext.Current.Session.Item("MasterCompanyWhere") &= Row.Item("CompId")
              End If


              If Me.view_21_tabcontain.ActiveTabIndex = 100 Then
                If amod_id > 0 Or Trim(make_name) <> "" Then
                  If Trim(company_id_string) <> "" Then
                    company_id_string &= ", " & Row.Item("CompId")
                  Else
                    company_id_string &= Row.Item("CompId")
                  End If
                End If
              End If

              'Else
              If Me.view_21_tabcontain.ActiveTabIndex = 3 Or Me.view_21_tabcontain.ActiveTabIndex = 100 Then
                If Trim(contact_id_string) <> "" Then
                  contact_id_string &= ", " & Row.Item("ContactId")
                Else
                  contact_id_string &= Row.Item("ContactId")
                End If
              End If


              If Trim(last_contact_name) = "" Or Trim(last_contact_name) <> Trim(Row.Item("ContactGroup")) Then
                If Not toggleRowColor Then
                  temp_comp += "<tr class='alt_row'>"
                  toggleRowColor = True
                Else
                  temp_comp += "<tr bgcolor='white'>"
                  toggleRowColor = False
                End If

                temp_comp += "<td>"
                temp_comp += DisplayFunctions.WriteDetailsLink(0, Row.Item("CompId"), Row.Item("ContactId"), 0, True, Row.Item("ContactFullName"), "", "")

                If Trim(type_of) = "" Then
                  If Row.Item("num_prevaircraft") > 0 And Row.Item("num_aircraft") = 0 Then

                    If Not IsDBNull(Row.Item("Num_Yachts")) Then
                      If Row.Item("Num_Yachts") > 1 Then
                        temp_comp += " - " & Row.Item("Num_Yachts") & " Yachts"
                      Else
                        temp_comp += " - " & Row.Item("Num_Yachts") & " Yacht"
                      End If
                    End If

                    temp_comp += " - (Previous Aircraft Owner on " & Row.Item("num_prevaircraft") & " Aircraft)"
                  ElseIf Row.Item("Num_Yachts") > 0 And Row.Item("num_aircraft") > 0 Then
                    If Not IsDBNull(Row.Item("num_aircraft")) Then 

                      If Not IsDBNull(Row.Item("Num_Yachts")) Then
                        If Row.Item("Num_Yachts") > 1 Then
                          temp_comp += " - (" & Row.Item("Num_Yachts") & " Yachts"
                        Else
                          temp_comp += " - (" & Row.Item("Num_Yachts") & " Yacht"
                        End If
                      End If

                      If Not IsDBNull(Row.Item("num_aircraft")) Then 
                        temp_comp += " / " & Row.Item("num_aircraft") & " Aircraft)"
                      End If
                    End If

                  End If
                End If


                temp_comp += "</td></tr>"
              End If

              If Not toggleRowColor Then
                temp_comp += "<tr class='alt_row'><td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&#149;&nbsp;"
                toggleRowColor = True
              Else
                temp_comp += "<tr bgcolor='white'><td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&#149;&nbsp;"
                toggleRowColor = False
              End If


              last_contact_name = Trim(Row.Item("ContactGroup"))

              temp_comp += "<b>" & Row.Item("Company") & "</b>"
              add_comma = True

              If Not IsDBNull(Row.Item("City")) Then
                If Trim(Row.Item("City")) <> "" Then
                  temp_comp += ", " & Row.Item("City")
                  add_comma = True
                End If
              End If

              If Not IsDBNull(Row.Item("State")) Then
                If Trim(Row.Item("State")) <> "" Then
                  If add_comma = True Then
                    temp_comp += ", "
                  End If
                  temp_comp += Row.Item("State")
                  add_comma = True
                End If
              End If

              If Not IsDBNull(Row.Item("Country")) Then
                If Trim(Row.Item("Country")) <> "" Then
                  If add_comma = True Then
                    temp_comp += ", "
                  End If
                  temp_comp += Row.Item("Country")
                End If
              End If

              temp_comp += ""

              If Not IsDBNull(Row.Item("ContactTitle")) Then
                If Trim(Row.Item("ContactTitle")) <> "" Then
                  If add_comma = True Then
                    temp_comp += ", "
                  End If
                  temp_comp += "<b><i>" & Row.Item("ContactTitle") & "</i></b>"
                End If
              End If


              temp_comp += "</td></tr>"


            Next

          End If
          temp_comp += "</table></div>"

          If amod_id > 0 Or Trim(make_name) <> "" Then
            view_21_tab100_label.Text = temp_comp
            view_21_tab100.Visible = True
          Else
            view_21_1.Text = temp_comp
          End If


        End If


        If Me.view_21_tabcontain.ActiveTabIndex = 1 Then
          view_21_tab1.Visible = True
          view_21_hide_left.Visible = True
          Me.end_row_new_row.Visible = False
          Me.tplist1.HeaderText = "Aircraft Makes Owned"
          Me.tplist2.HeaderText = "Aircraft Models Owned"
          Me.graph_title_label.Text = "<tr valign='top'><td width='50%' align='center'>Aircraft Types Owned </td><td align='center'>Jets Owned by Size</td></tr>"
        ElseIf Me.view_21_tabcontain.ActiveTabIndex = 3 Then
          view_21_tab1.Visible = True
          view_21_hide_left.Visible = True
          '  Me.tplist1.HeaderText = "Size of Yacht"
          ' Me.tplist2.HeaderText = "Brand of Yacht"
          Me.make_model_Tab_container.Visible = False
          Me.end_row_new_row.Visible = True
          Me.graph_title_label.Visible = True
          Me.graph_title_label2.Visible = True
          Me.graph_title_label.Text = "<tr valign='top'><td align='center'>Yacht Model Size</td></tr>"
          If Trim(yacht_size) <> "" Then
            graph_title_label.Visible = False
          End If
          Me.graph_title_label2.Text = "<tr valign='top'><td align='center'>Brand of Yacht</td></tr>"

          Call make_pie_charts_no_ac(view_21_tab1, view_21_1_list2, "chart_div_tab1_all", "chart_div_tab1_all_2", amod_id, view_21_1_list, make_name, type_of, div_for_yacht_size.Text, yacht_size)
          Me.div_for_yacht_size.Visible = True
        End If

        'XXXCONTACT_ID_LISTXXX

        If Me.view_21_tabcontain.ActiveTabIndex = 100 Then
          Session("Yacht_Crossover_Select") = Replace(Session("Yacht_Crossover_Select"), "XXXCOMP_IDXXX", company_id_string)
          Session("Yacht_Crossover_Model_Select") = Replace(Session("Yacht_Crossover_Model_Select"), "XXXCOMP_IDXXX", company_id_string)
          '  Session("Yacht_Crossover_Yacht_Select") = Replace(Session("Yacht_Crossover_Yacht_Select"), "XXXCOMP_IDXXX", company_id_string)

          temp_contact_list = Split(contact_id_string, ",")
  
          related_contact_id_string = ""
          For i = 0 To temp_contact_list.Length - 1
            related_contact_id_string &= MasterPage.aclsData_Temp.GET_CONTACT_IDS_RELATED(temp_contact_list(i))
          Next

          If Right(Trim(related_contact_id_string), 1) = "," Then
            related_contact_id_string = Left(Trim(related_contact_id_string), Len(Trim(related_contact_id_string)) - 1)
          End If

          Session("Yacht_Crossover_Yacht_Select") = Replace(Session("Yacht_Crossover_Yacht_Select"), "XXXCONTACT_IDXXX", contact_id_string)
          Session("Yacht_Crossover_Yacht_Select") = Replace(Session("Yacht_Crossover_Yacht_Select"), "XXXCONTACT_ID_LISTXXX", related_contact_id_string)

        ElseIf Me.view_21_tabcontain.ActiveTabIndex = 3 Then
          Session("Yacht_Crossover_Yacht_Select") = Replace(Session("Yacht_Crossover_Yacht_Select"), "XXXCONTACT_IDXXX", contact_id_string)
        End If



        '	Mr. Roman Abramovich (Abramovich, Roman, Russian Federation)
        'ContactFullName (Company, City, State, Country) 
        HoldTable.Dispose()

        If Me.view_21_tabcontain.ActiveTabIndex = 1 Or Me.view_21_tabcontain.ActiveTabIndex = 100 Then
          If amod_id > 0 Or Trim(make_name) <> "" Then
            Call make_pie_charts(view_21_tab100, view_21_100_list, "chart_div_tab100_all", "chart_div_tab100_all_2", amod_id, Nothing, make_name)
          Else
            Call make_pie_charts(view_21_tab1, view_21_1_list2, "chart_div_tab1_all", "chart_div_tab1_all_2", amod_id, view_21_1_list, make_name)
          End If
        End If


      ElseIf Me.view_21_tabcontain.ActiveTabIndex = 2 Or Me.view_21_tabcontain.ActiveTabIndex = 101 Then

        view_21_1.Text = ""
        HoldTable = localDatalayer.get_yacht_crossover_companies_section2_new(amod_id, make_name)

        If Not IsNothing(HoldTable) Then

          If Me.view_21_tabcontain.ActiveTabIndex = 2 Then
            temp_comp = "<div valign=""top"" style='height:550px; overflow: auto;'>"
          ElseIf Me.view_21_tabcontain.ActiveTabIndex = 101 Then
            temp_comp = "<div valign=""top"" style='height:280px; overflow: auto;'>"
          Else
            temp_comp = "<div valign=""top"" style='height:620px; overflow: auto;'>"
          End If

          temp_comp += "<table width='100%' cellpadding='3' cellspacing='0' class='data_aircraft_grid'>"
          temp_comp += "<tr class='header_row'>"
          temp_comp += "<td align='left' valign='top' width='50%'><b>Companies Owning/Leasing Yachts/AC </b></td>"
          ' temp_comp += "<td align='left' valign='top' width='10%'><b>#Yachts</b>&nbsp;</td>"
          'temp_comp += "<td align='left' valign='top' width='10%'><b>#AC</b>&nbsp;</td>"
          'temp_comp += "<td align='left' valign='top' width='30%'>&nbsp;</td>"
          temp_comp += "</tr>"

          'view_21_tab2.HeaderText = "Companies Owning Yachts & Aircraft (" & HoldTable.Rows.Count & ") "

          If HoldTable.Rows.Count > 0 Then
            For Each Row As DataRow In HoldTable.Rows


              HttpContext.Current.Session.Item("MasterCompanyFrom") = "COMPANY"
              If Trim(HttpContext.Current.Session.Item("MasterCompanyWhere")) <> "" Then
                HttpContext.Current.Session.Item("MasterCompanyWhere") &= ", " & Row.Item("comp_id")
              Else
                HttpContext.Current.Session.Item("MasterCompanyWhere") &= Row.Item("comp_id")
              End If

              If last_comp_id = Row.Item("comp_id") Then

              Else

                count_yacht_compaies = count_yacht_compaies + 1
                If Not toggleRowColor Then
                  temp_comp += "<tr class='alt_row'><td>"
                  toggleRowColor = True
                Else
                  temp_comp += "<tr bgcolor='white'><td>"
                  toggleRowColor = False
                End If

                temp_comp += DisplayFunctions.WriteDetailsLink(0, Row.Item("comp_id"), 0, 0, True, Row.Item("comp_name").ToString, "", "")


                add_comma = False
                has_location = False


                If Not IsDBNull(Row.Item("comp_city")) Then
                  If Trim(Row.Item("comp_city")) <> "" Then
                    has_location = True
                  End If
                End If
                If Not IsDBNull(Row.Item("comp_state")) Then
                  If Trim(Row.Item("comp_state")) <> "" Then
                    has_location = True
                  End If
                End If
                If Not IsDBNull(Row.Item("comp_country")) Then
                  If Trim(Row.Item("comp_country")) <> "" Then
                    has_location = True
                  End If
                End If

                If has_location = True Then
                  temp_comp += " ("

                  add_comma = False
                  If Not IsDBNull(Row.Item("comp_city")) Then
                    If Trim(Row.Item("comp_city")) <> "" Then
                      temp_comp += Row.Item("comp_city")
                      add_comma = True
                    End If
                  End If

                  If Not IsDBNull(Row.Item("comp_state")) Then
                    If Trim(Row.Item("comp_state")) <> "" Then
                      If add_comma = True Then
                        temp_comp += ","
                      End If
                      temp_comp += Row.Item("comp_state")
                      add_comma = True
                    End If
                  End If

                  If Not IsDBNull(Row.Item("comp_country")) Then
                    If Trim(Row.Item("comp_country")) <> "" Then
                      If add_comma = True Then
                        temp_comp += ","
                      End If
                      temp_comp += Row.Item("comp_country")
                    End If
                  End If

                  temp_comp += ") - "
                End If

                '  temp_comp += "</td>"

                ' temp_comp += "<td align='right'>"
                If Not IsDBNull(Row.Item("cross_yacht_count")) Then
                  If Trim(Row.Item("cross_yacht_count")) <> "" Then
                    temp_comp += Row.Item("cross_yacht_count").ToString

                    If Row.Item("cross_yacht_count") = 1 Then
                      temp_comp += "&nbsp;Yacht&nbsp;&nbsp;"
                    Else
                      temp_comp += "&nbsp;Yachts&nbsp;&nbsp;"
                    End If
                  Else
                    temp_comp += "0"
                  End If
                Else
                  temp_comp += "0"
                End If

                temp_comp += "</td>"

                ''temp_comp += "<td align='right'>"
                '' If Not IsDBNull(Row.Item("cross_aircraft_count")) Then
                ''If Trim(Row.Item("cross_aircraft_count")) <> "" Then
                ''  temp_comp += Row.Item("cross_aircraft_count").ToString
                ''Else
                ''  temp_comp += "0"
                ''End If
                ''Else
                ''temp_comp += "0"
                ''End If


                ' temp_comp += "&nbsp;&nbsp;&nbsp;</td>"

                ' temp_comp += "<td align='right'>&nbsp;&nbsp;&nbsp;</td>"
                temp_comp += "</tr>"
              End If


              If Not toggleRowColor Then
                temp_comp += "<tr class='alt_row'><td><b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&#149;&nbsp;"
                toggleRowColor = True
              Else
                temp_comp += "<tr bgcolor='white'><td><b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&#149;&nbsp;"
                toggleRowColor = False
              End If


              temp_comp += DisplayFunctions.WriteDetailsLink(0, Row.Item("cross_CompID"), 0, 0, True, Row.Item("cross_Company").ToString, "", "")


              add_comma = False
              has_location = False


              ', , , , ,
              '  cross_BFlag, cross_CFlag, cross_HFlag, cross_YFlag, cross_aircraft_count 

              If Not IsDBNull(Row.Item("cross_city")) Then
                If Trim(Row.Item("cross_city")) <> "" Then
                  has_location = True
                End If
              End If
              If Not IsDBNull(Row.Item("cross_State")) Then
                If Trim(Row.Item("cross_State")) <> "" Then
                  has_location = True
                End If
              End If
              If Not IsDBNull(Row.Item("cross_Country")) Then
                If Trim(Row.Item("cross_Country")) <> "" Then
                  has_location = True
                End If
              End If

              If has_location = True Then
                temp_comp += "</b> ("

                add_comma = False
                If Not IsDBNull(Row.Item("cross_city")) Then
                  If Trim(Row.Item("cross_city")) <> "" Then
                    temp_comp += Row.Item("cross_city")
                    add_comma = True
                  End If
                End If

                If Not IsDBNull(Row.Item("cross_State")) Then
                  If Trim(Row.Item("cross_State")) <> "" Then
                    If add_comma = True Then
                      temp_comp += ","
                    End If
                    temp_comp += Row.Item("cross_State")
                    add_comma = True
                  End If
                End If

                If Not IsDBNull(Row.Item("cross_Country")) Then
                  If Trim(Row.Item("cross_Country")) <> "" Then
                    If add_comma = True Then
                      temp_comp += ","
                    End If
                    temp_comp += Row.Item("cross_Country")
                  End If
                End If

                temp_comp += ") - "
              End If

              '  temp_comp += "</td>"

              '  temp_comp += "<td align='right'>"
              '  temp_comp += "&nbsp;</td>"

              '  temp_comp += "<td align='right'>"
              If Not IsDBNull(Row.Item("cross_aircraft_count")) Then
                If Trim(Row.Item("cross_aircraft_count")) <> "" Then
                  temp_comp += Row.Item("cross_aircraft_count").ToString
                Else
                  temp_comp += "0"
                End If
              Else
                temp_comp += "0"
              End If
              temp_comp += "&nbsp;Aircraft&nbsp;&nbsp;</td>"
              '  temp_comp += "<td align='right'>&nbsp;&nbsp;&nbsp;</td>"
              ' temp_comp += "</tr>"

              last_comp_id = Row.Item("comp_id")

              'AircraftRel
              'YachtRel
            Next
          End If
          temp_comp += "</table></div>"


          If amod_id > 0 Or Trim(make_name) <> "" Then
            If Trim(make_name) <> "" Then
              view_21_tab100.HeaderText = "Also Owning " & Trim(make_name) & " (" & count_yacht_compaies & ")"
            Else
              view_21_tab100.HeaderText = "Also Owning " & Trim(Request("model_name")) & " (" & count_yacht_compaies & ")"
            End If

            view_21_tab1.HeaderText = "Companies Owning/Leasing Yachts/AC (" & count_yacht_compaies & ")"
          Else
            view_21_tab1.HeaderText = "Companies Owning/Leasing Yachts/AC (" & count_yacht_compaies & ")"
          End If
          view_21_tab1.Visible = True

          If amod_id > 0 Or Trim(make_name) <> "" Then
            view_21_tab100_label.Text = temp_comp
            view_21_tab100.Visible = True
          Else
            view_21_1.Text = temp_comp
            view_21_1.Visible = True

            HoldTable.Dispose()

            view_21_tab1.Visible = True
            view_21_hide_left.Visible = True
            Me.end_row_new_row.Visible = False
            Me.tplist1.HeaderText = "Aircraft Makes Owned"
            Me.tplist2.HeaderText = "Aircraft Models Owned"
            Me.graph_title_label.Text = "<tr valign='top'><td width='50%' align='center'>Aircraft Types Owned </td><td align='center'>Jets Owned by Size</td></tr>"
          End If


          If amod_id > 0 Or Trim(make_name) <> "" Then
            Call make_pie_charts_company(view_21_tab100, view_21_100_list, "chart_div_tab100_all", "chart_div_tab100_all_2", amod_id, Nothing, make_name)
          Else
            Call make_pie_charts_company(view_21_tab1, view_21_1_list2, "chart_div_tab1_all", "chart_div_tab1_all_2", amod_id, view_21_1_list, make_name)
          End If

        End If

        HoldTable.Dispose()

      ElseIf Me.view_21_tabcontain.ActiveTabIndex = 4 Then

        view_21_4.Text = ""
        HoldTable = localDatalayer.get_yacht_central_agents_no_ac("")

        If Not IsNothing(HoldTable) Then
          temp_comp = "<div valign=""top"" style='height:710px; overflow: auto;'>"

          temp_comp += "<table width='100%' cellpadding='3' cellspacing='0' class='data_aircraft_grid'>"
          temp_comp += "<tr class='header_row'>"
          temp_comp += "<td align='left' valign='top'><b>Companies in Yacht/AC Business</b></td>"
          temp_comp += "</tr>"

          Me.view_21_tab1.HeaderText = "Companies in Yacht/AC Business (" & HoldTable.Rows.Count & ")"
          Me.view_21_tab1.Visible = True
          ' Me.view_21_tab1_bottom.HeaderText = "Yacht Companies (Not Owners) In Aviation (" & HoldTable.Rows.Count & ") "

          If HoldTable.Rows.Count > 0 Then
            For Each Row As DataRow In HoldTable.Rows

              HttpContext.Current.Session.Item("MasterCompanyFrom") = "COMPANY"
              If Trim(HttpContext.Current.Session.Item("MasterCompanyWhere")) <> "" Then
                HttpContext.Current.Session.Item("MasterCompanyWhere") &= ", " & Row.Item("comp_id")
              Else
                HttpContext.Current.Session.Item("MasterCompanyWhere") &= Row.Item("comp_id")
              End If

              has_location = False

              If Not toggleRowColor Then
                temp_comp += "<tr class='alt_row'>"
                toggleRowColor = True
              Else
                temp_comp += "<tr bgcolor='white'>"
                toggleRowColor = False
              End If
              temp_comp += "<td>"
              temp_comp += DisplayFunctions.WriteDetailsLink(0, Row.Item("comp_id"), 0, 0, True, Row.Item("comp_name").ToString, "", "")
              temp_comp += " ("

              add_comma = False
              If Not IsDBNull(Row.Item("comp_city")) Then
                If Trim(Row.Item("comp_city")) <> "" Then
                  temp_comp += "" & Row.Item("comp_city")
                  add_comma = True
                End If
              End If

              If Not IsDBNull(Row.Item("comp_state")) Then
                If Trim(Row.Item("comp_state")) <> "" Then
                  If add_comma = True Then
                    temp_comp += ","
                  End If
                  temp_comp += Row.Item("comp_state")
                  add_comma = True
                End If
              End If

              If Not IsDBNull(Row.Item("comp_country")) Then
                If Trim(Row.Item("comp_country")) <> "" Then
                  If add_comma = True Then
                    temp_comp += ","
                  End If
                  temp_comp += Row.Item("comp_country")
                End If
              End If

              temp_comp += ")"
              temp_comp += "</td>"

              temp_comp += "</tr>"

              view_21_1.Text &= temp_comp
              temp_comp = ""
            Next
          Else
            view_21_1.Text &= "<tr bgcolor='white'><td>No Companies Found</td></tr>"
          End If
          view_21_1.Text &= "</table></div>"


          view_21_1.Visible = True
        End If

        HoldTable.Dispose()



        Call make_pie_charts_dbi_both(view_21_tab1, view_21_1_list, "chart_div_tab1_all", "chart_div_tab1_all_2", amod_id, view_21_1_list2, make_name)
        Me.tplist2.Visible = False
        Me.view_21_1_list.Visible = True
        Me.tplist1.HeaderText = "Aircraft Business Types"
        Me.graph_title_label.Text = "<tr valign='top'><td width='50%' align='center'>Aircraft Business Types</td><td align='center'>&nbsp;</td></tr>"

      ElseIf Me.view_21_tabcontain.ActiveTabIndex = 5 Then

        HoldTable = localDatalayer.get_from_View_Private_Owners_of_Jets_With_No_Yachts("", type_of)

        ' Session("Yacht_Crossover_Yacht_Select") &= " and comp_id in (XXXCOMP_IDXXX) "

        view_21_1.Text = ""
        If Not IsNothing(HoldTable) Then
          temp_comp = "<div valign=""top"" style='height:630px; overflow: auto;'>"

          temp_comp += "<table width='100%' cellpadding='3' cellspacing='0' class='data_aircraft_grid'>"
          temp_comp += "<tr class='header_row'>"


          If Trim(type_of) = "" Then
            temp_comp += "<td align='left' valign='top'><b>Individuals Owning/Leasing AC not Yachts </b></td>"
          ElseIf Trim(type_of) = "J" Then
            temp_comp += "<td align='left' valign='top'><b>Individuals Owning/Leasing Jets not Yachts </b></td>"
          ElseIf Trim(type_of) = "T" Then
            temp_comp += "<td align='left' valign='top'><b>Individuals Owning/Leasing Turbo Props not Yachts)</b></td>"
          ElseIf Trim(type_of) = "H" Then
            temp_comp += "<td align='left' valign='top'><b>Individuals Owning/Leasing Helicopters not Yachts</b></td>"
          Else
            temp_comp += "<td align='left' valign='top'><b>Individuals Owning/Leasing AC not Yachts</b></td>"

          End If
          temp_comp += "</tr>"


          If Trim(type_of) = "" Then
            Me.view_21_tab1.HeaderText = "Individuals Owning/Leasing AC not Yachts (" & HoldTable.Rows.Count & ")"
          ElseIf Trim(type_of) = "J" Then
            Me.view_21_tab1.HeaderText = "Individuals Owning/Leasing Jets not Yachts (" & HoldTable.Rows.Count & ")"
          ElseIf Trim(type_of) = "T" Then
            Me.view_21_tab1.HeaderText = "Individuals Owning/Leasing Turbo Props not Yachts (" & HoldTable.Rows.Count & ")"
          ElseIf Trim(type_of) = "H" Then
            Me.view_21_tab1.HeaderText = "Individuals Owning/Leasing Helicopters not Yachts (" & HoldTable.Rows.Count & ")"
          Else
            Me.view_21_tab1.HeaderText = "Individuals Owning/Leasing AC not Yachts (" & HoldTable.Rows.Count & ")"
          End If


          Me.view_21_tab1.Visible = True
          ' Me.view_21_tab1_bottom.HeaderText = "Yacht Companies (Not Owners) In Aviation (" & HoldTable.Rows.Count & ") "

          If HoldTable.Rows.Count > 0 Then
            For Each Row As DataRow In HoldTable.Rows

              has_location = False

              If Not toggleRowColor Then
                temp_comp += "<tr class='alt_row'>"
                toggleRowColor = True
              Else
                temp_comp += "<tr bgcolor='white'>"
                toggleRowColor = False
              End If

              HttpContext.Current.Session.Item("MasterCompanyFrom") = "COMPANY"
              If Trim(HttpContext.Current.Session.Item("MasterCompanyWhere")) <> "" Then
                HttpContext.Current.Session.Item("MasterCompanyWhere") &= ", " & Row.Item("comp_id")
              Else
                HttpContext.Current.Session.Item("MasterCompanyWhere") &= Row.Item("comp_id")
              End If


              If Trim(HttpContext.Current.Session.Item("MasterContactWhere")) <> "" Then
                HttpContext.Current.Session.Item("MasterContactWhere") &= ", " & Row.Item("contact_id")
              Else
                HttpContext.Current.Session.Item("MasterContactWhere") &= Row.Item("contact_id")
              End If

              temp_comp += "<td>"
              temp_comp += DisplayFunctions.WriteDetailsLink(0, Row.Item("comp_id"), Row.Item("contact_id"), 0, True, Row.Item("comp_name").ToString, "", "")
              temp_comp += " ("

              add_comma = False
              If Not IsDBNull(Row.Item("comp_city")) Then
                If Trim(Row.Item("comp_city")) <> "" Then
                  temp_comp += "" & Row.Item("comp_city")
                  add_comma = True
                End If
              End If

              If Not IsDBNull(Row.Item("comp_state")) Then
                If Trim(Row.Item("comp_state")) <> "" Then
                  If add_comma = True Then
                    temp_comp += ","
                  End If
                  temp_comp += Row.Item("comp_state")
                  add_comma = True
                End If
              End If

              'If Not IsDBNull(Row.Item("comp_zip_code")) Then
              '  If Trim(Row.Item("comp_zip_code")) <> "" Then
              '    If add_comma = True Then
              '      temp_comp += ","
              '    End If
              '    temp_comp += Row.Item("comp_zip_code")
              '    add_comma = True
              '  End If
              'End If

              If Not IsDBNull(Row.Item("comp_country")) Then
                If Trim(Row.Item("comp_country")) <> "" Then
                  If add_comma = True Then
                    temp_comp += ","
                  End If
                  temp_comp += Row.Item("comp_country")
                End If
              End If

              temp_comp += ")"
              temp_comp += "</td>"

              temp_comp += "</tr>"
              view_21_1.Text &= temp_comp
              temp_comp = ""

            Next
          Else
            temp_comp += "<tr bgcolor='white'><td>No Companies Found</td></tr>"
          End If
          view_21_1.Text &= "</table></div>"

          view_21_1.Visible = True
        End If


        Call make_pie_charts_indv_own(view_21_tab1, view_21_1_list, "chart_div_tab1_all", "chart_div_tab1_all_2", amod_id, view_21_1_list2, make_name)

        Me.view_21_1_list.Visible = True
        Me.view_21_1_list2.Visible = True
        Me.tplist1.HeaderText = "Aircraft Owners By Make"
        Me.tplist2.HeaderText = "Aircraft Owners By Model"
        Me.graph_title_label.Text = "<tr valign='top'><td width='50%' align='center'>Whole vs. Fractional Ownership</td><td align='center'>&nbsp;</td></tr>"


      ElseIf Me.view_21_tabcontain.ActiveTabIndex = 6 Then

        view_21_1.Text = ""
        HoldTable = localDatalayer.get_Yacht_Owners_Not_Owning_AC(type_of)

        If Not IsNothing(HoldTable) Then
          temp_comp = "<div valign=""top"" style='height:620px; overflow: auto;'>"

          temp_comp += "<table width='100%' cellpadding='3' cellspacing='0' class='data_aircraft_grid'>"
          temp_comp += "<tr class='header_row'>"
          If Trim(type_of) = "" Then
            temp_comp += "<td align='left' valign='top'><b>Companies Owning/Leasing Yachts, Not Aircraft</b></td>"
          Else
            temp_comp += "<td align='left' valign='top'><b>Companies Owning/Leasing Yachts, Not Aircraft - Previously Owning Aircraft</b></td>"
          End If

          temp_comp += "</tr>"

          If Trim(type_of) = "" Then
            Me.view_21_tab1.HeaderText = "Companies Owning/Leasing Yachts, Not Aircraft (" & HoldTable.Rows.Count & ")"
          Else
            Me.view_21_tab1.HeaderText = "Companies Owning/Leasing Yachts, Not Aircraft - Previously Owning Aircraft (" & HoldTable.Rows.Count & ")"
          End If



          Me.view_21_tab1.Visible = True
          ' Me.view_21_tab1_bottom.HeaderText = "Yacht Companies (Not Owners) In Aviation (" & HoldTable.Rows.Count & ") "

          If HoldTable.Rows.Count > 0 Then
            For Each Row As DataRow In HoldTable.Rows

              HttpContext.Current.Session.Item("MasterCompanyFrom") = "COMPANY"
              If Trim(HttpContext.Current.Session.Item("MasterCompanyWhere")) <> "" Then
                HttpContext.Current.Session.Item("MasterCompanyWhere") &= ", " & Row.Item("comp_id")
              Else
                HttpContext.Current.Session.Item("MasterCompanyWhere") &= Row.Item("comp_id")
              End If

              If Not toggleRowColor Then
                temp_comp += "<tr class='alt_row'><td>"
                toggleRowColor = True
              Else
                temp_comp += "<tr bgcolor='white'><td>"
                toggleRowColor = False
              End If

              temp_comp += DisplayFunctions.WriteDetailsLink(0, Row.Item("comp_id"), 0, 0, True, Row.Item("comp_name").ToString, "", "")


              add_comma = False
              has_location = False


              If Not IsDBNull(Row.Item("comp_city")) Then
                If Trim(Row.Item("comp_city")) <> "" Then
                  has_location = True
                End If
              End If
              If Not IsDBNull(Row.Item("comp_state")) Then
                If Trim(Row.Item("comp_state")) <> "" Then
                  has_location = True
                End If
              End If
              If Not IsDBNull(Row.Item("comp_country")) Then
                If Trim(Row.Item("comp_country")) <> "" Then
                  has_location = True
                End If
              End If

              If has_location = True Then
                temp_comp += " ("

                add_comma = False
                If Not IsDBNull(Row.Item("comp_city")) Then
                  If Trim(Row.Item("comp_city")) <> "" Then
                    temp_comp += Row.Item("comp_city")
                    add_comma = True
                  End If
                End If

                If Not IsDBNull(Row.Item("comp_state")) Then
                  If Trim(Row.Item("comp_state")) <> "" Then
                    If add_comma = True Then
                      temp_comp += ","
                    End If
                    temp_comp += Row.Item("comp_state")
                    add_comma = True
                  End If
                End If

                If Not IsDBNull(Row.Item("comp_country")) Then
                  If Trim(Row.Item("comp_country")) <> "" Then
                    If add_comma = True Then
                      temp_comp += ","
                    End If
                    temp_comp += Row.Item("comp_country")
                  End If
                End If

                temp_comp += ")"
              End If
              temp_comp += " - "
              '  temp_comp += "</td>"

              ' temp_comp += "<td align='right'>"
              If Not IsDBNull(Row.Item("cross_yacht_count")) Then
                If Trim(Row.Item("cross_yacht_count")) <> "" Then
                  temp_comp += Row.Item("cross_yacht_count").ToString

                  If Row.Item("cross_yacht_count") = 1 Then
                    temp_comp += "&nbsp;Yacht&nbsp;&nbsp;"
                  Else
                    temp_comp += "&nbsp;Yachts&nbsp;&nbsp;"
                  End If
                Else
                  temp_comp += "0"
                End If
              Else
                temp_comp += "0"
              End If

              If Trim(type_of) = "" Then
                If Row.Item("cross_prevaircraft_count") > 0 Then
                  temp_comp += " (Previous Aircraft Owner)"
                End If
              End If

              temp_comp += "</td>"
              temp_comp += "</tr>"

              view_21_1.Text &= temp_comp
              temp_comp = ""
            Next
          Else
            temp_comp += "<tr bgcolor='white'><td>No Individuals Found</td></tr>"
          End If
          view_21_1.Text &= "</table></div>"

          view_21_1.Visible = True
        End If

        view_21_tab1.Visible = True
        view_21_hide_left.Visible = True
        Me.make_model_Tab_container.Visible = False
        Me.end_row_new_row.Visible = True
        Me.graph_title_label.Visible = True
        Me.graph_title_label2.Visible = True
        Me.graph_title_label.Text = "<tr valign='top'><td align='center'>Yacht Model Size</td></tr>"
        Me.graph_title_label2.Text = "<tr valign='top'><td align='center'>Brand of Yacht</td></tr>"

        Call make_pie_charts_comp_no_ac(view_21_tab1, view_21_1_list2, "chart_div_tab1_all", "chart_div_tab1_all_2", amod_id, view_21_1_list, make_name, type_of)


      ElseIf Me.view_21_tabcontain.ActiveTabIndex = 7 Then

        view_21_7.Text = ""
        HoldTable = localDatalayer.get_Individual_Owners_Without_Aircraft()

        If Not IsNothing(HoldTable) Then
          temp_comp = "<div valign=""top"" style='height:400px; overflow: auto;'>"

          temp_comp += "<table width='100%' cellpadding='3' cellspacing='0' class='data_aircraft_grid'>"
          temp_comp += "<tr class='header_row'>"
          temp_comp += "<td align='left' valign='top'><b>Recent Individuals Purchasing Yachts, Not Owning/Leasing Aircraft</b></td>"
          temp_comp += "</tr>"

          Me.view_21_tab7.HeaderText = "Recent Individuals Purchasing Yachts, Not Owning/Leasing Aircraft (" & HoldTable.Rows.Count & ")"
          Me.view_21_tab7.Visible = True
          ' Me.view_21_tab1_bottom.HeaderText = "Yacht Companies (Not Owners) In Aviation (" & HoldTable.Rows.Count & ") "

          If HoldTable.Rows.Count > 0 Then
            For Each Row As DataRow In HoldTable.Rows

              HttpContext.Current.Session.Item("MasterCompanyFrom") = "COMPANY"
              If Trim(HttpContext.Current.Session.Item("MasterCompanyWhere")) <> "" Then
                HttpContext.Current.Session.Item("MasterCompanyWhere") &= ", " & Row.Item("ype_comp_id")
              Else
                HttpContext.Current.Session.Item("MasterCompanyWhere") &= Row.Item("ype_comp_id")
              End If

              has_location = False

              If Not toggleRowColor Then
                temp_comp += "<tr class='alt_row'>"
                toggleRowColor = True
              Else
                temp_comp += "<tr bgcolor='white'>"
                toggleRowColor = False
              End If
              temp_comp += "<td>Recorded On "

              If Not IsDBNull(Row.Item("ype_entered_date")) Then
                If Trim(Row.Item("ype_entered_date")) <> "" Then
                  temp_comp += "" & FormatDateTime(Row.Item("ype_entered_date"), DateFormat.ShortDate) & " - "
                End If
              End If

              If Not IsDBNull(Row.Item("contact_first_name")) Then
                If Trim(Row.Item("contact_first_name")) <> "" Then
                  temp_comp += "" & Row.Item("contact_first_name")
                End If
              End If

              If Not IsDBNull(Row.Item("contact_last_name")) Then
                If Trim(Row.Item("contact_last_name")) <> "" Then
                  temp_comp += " " & Row.Item("contact_last_name")
                End If
              End If

              temp_comp += " - "

              temp_comp += DisplayFunctions.WriteDetailsLink(0, Row.Item("ype_comp_id"), 0, 0, True, Row.Item("comp_name").ToString, "", "")
              temp_comp += " ("

              add_comma = False
              If Not IsDBNull(Row.Item("comp_city")) Then
                If Trim(Row.Item("comp_city")) <> "" Then
                  temp_comp += "" & Row.Item("comp_city")
                  add_comma = True
                End If
              End If

              If Not IsDBNull(Row.Item("comp_state")) Then
                If Trim(Row.Item("comp_state")) <> "" Then
                  If add_comma = True Then
                    temp_comp += ","
                  End If
                  temp_comp += Row.Item("comp_state")
                  add_comma = True
                End If
              End If

              If Not IsDBNull(Row.Item("comp_country")) Then
                If Trim(Row.Item("comp_country")) <> "" Then
                  If add_comma = True Then
                    temp_comp += ","
                  End If
                  temp_comp += Row.Item("comp_country")
                End If
              End If

              temp_comp += ") - "
              temp_comp += "Purchased "
              temp_comp += "<a href='#' onclick=""javascript:load('DisplayYachtDetail.aspx?yid=" & Row.Item("yt_id") & "&jid=0','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');return false;"">"
              If Not IsDBNull(Row.Item("yt_yacht_name")) Then
                If Trim(Row.Item("yt_yacht_name")) <> "" Then
                  temp_comp += Row.Item("yt_yacht_name")
                End If
              End If

              temp_comp += "</a>"

              If Not IsDBNull(Row.Item("ym_brand_name")) Then
                If Trim(Row.Item("ym_brand_name")) <> "" Then
                  temp_comp += " " & Row.Item("ym_brand_name")
                End If
              End If

              If Not IsDBNull(Row.Item("ym_model_name")) Then
                If Trim(Row.Item("ym_model_name")) <> "" Then
                  temp_comp += " " & Row.Item("ym_model_name")
                End If
              End If

              temp_comp += " Yacht on "

              If Not IsDBNull(Row.Item("transdate")) Then
                If Trim(Row.Item("transdate")) <> "" Then
                  temp_comp += " " & Row.Item("transdate")
                End If
              End If

              temp_comp += "</td>"

              temp_comp += "</tr>"

              view_21_7.Text &= temp_comp
              temp_comp = ""
            Next
          Else
            view_21_7.Text &= "<tr bgcolor='white'><td>No Companies Found</td></tr>"
          End If
          view_21_7.Text &= "</table></div>"

          view_21_7.Visible = True
        End If

        HoldTable.Dispose()

      ElseIf Me.view_21_tabcontain.ActiveTabIndex = 8 Then

        view_21_8.Text = ""
        HoldTable = localDatalayer.get_Yacht_New_Company_Owners_Without_Aircraft()

        If Not IsNothing(HoldTable) Then
          temp_comp = "<div valign=""top"" style='height:400px; overflow: auto;'>"

          temp_comp += "<table width='100%' cellpadding='3' cellspacing='0' class='data_aircraft_grid'>"
          temp_comp += "<tr class='header_row'>"
          temp_comp += "<td align='left' valign='top'><b>Recent Companies Purchasing Yachts, Not Owning/Leasing Aircraft</b></td>"
          temp_comp += "</tr>"

          Me.view_21_tab8.HeaderText = "Recent Companies Purchasing Yachts, Not Owning/Leasing Aircraft (" & HoldTable.Rows.Count & ")"
          Me.view_21_tab8.Visible = True
          ' Me.view_21_tab1_bottom.HeaderText = "Yacht Companies (Not Owners) In Aviation (" & HoldTable.Rows.Count & ") "

          If HoldTable.Rows.Count > 0 Then
            For Each Row As DataRow In HoldTable.Rows

              HttpContext.Current.Session.Item("MasterCompanyFrom") = "COMPANY"
              If Trim(HttpContext.Current.Session.Item("MasterCompanyWhere")) <> "" Then
                HttpContext.Current.Session.Item("MasterCompanyWhere") &= ", " & Row.Item("ype_comp_id")
              Else
                HttpContext.Current.Session.Item("MasterCompanyWhere") &= Row.Item("ype_comp_id")
              End If

              has_location = False

              If Not toggleRowColor Then
                temp_comp += "<tr class='alt_row'>"
                toggleRowColor = True
              Else
                temp_comp += "<tr bgcolor='white'>"
                toggleRowColor = False
              End If
              temp_comp += "<td>Recorded On "

              If Not IsDBNull(Row.Item("ype_entered_date")) Then
                If Trim(Row.Item("ype_entered_date")) <> "" Then
                  temp_comp += "" & FormatDateTime(Row.Item("ype_entered_date"), DateFormat.ShortDate) & " - "
                End If
              End If

              temp_comp += DisplayFunctions.WriteDetailsLink(0, Row.Item("ype_comp_id"), 0, 0, True, Row.Item("comp_name").ToString, "", "")
              temp_comp += " ("

              add_comma = False
              If Not IsDBNull(Row.Item("comp_city")) Then
                If Trim(Row.Item("comp_city")) <> "" Then
                  temp_comp += "" & Row.Item("comp_city")
                  add_comma = True
                End If
              End If

              If Not IsDBNull(Row.Item("comp_state")) Then
                If Trim(Row.Item("comp_state")) <> "" Then
                  If add_comma = True Then
                    temp_comp += ","
                  End If
                  temp_comp += Row.Item("comp_state")
                  add_comma = True
                End If
              End If

              If Not IsDBNull(Row.Item("comp_country")) Then
                If Trim(Row.Item("comp_country")) <> "" Then
                  If add_comma = True Then
                    temp_comp += ","
                  End If
                  temp_comp += Row.Item("comp_country")
                End If
              End If

              temp_comp += ") - "
              temp_comp += "Purchased "
              temp_comp += "<a href='#' onclick=""javascript:load('DisplayYachtDetail.aspx?yid=" & Row.Item("yt_id") & "&jid=0','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');return false;"">"
              If Not IsDBNull(Row.Item("yt_yacht_name")) Then
                If Trim(Row.Item("yt_yacht_name")) <> "" Then
                  temp_comp += Row.Item("yt_yacht_name")
                End If
              End If

              temp_comp += "</a>"

              If Not IsDBNull(Row.Item("ym_brand_name")) Then
                If Trim(Row.Item("ym_brand_name")) <> "" Then
                  temp_comp += " " & Row.Item("ym_brand_name")
                End If
              End If

              If Not IsDBNull(Row.Item("ym_model_name")) Then
                If Trim(Row.Item("ym_model_name")) <> "" Then
                  temp_comp += " " & Row.Item("ym_model_name")
                End If
              End If

              temp_comp += " Yacht on "

              If Not IsDBNull(Row.Item("transdate")) Then
                If Trim(Row.Item("transdate")) <> "" Then
                  temp_comp += " " & Row.Item("transdate")
                End If
              End If

              temp_comp += "</td>"

              temp_comp += "</tr>"

              view_21_8.Text &= temp_comp
              temp_comp = ""
            Next
          Else
            view_21_8.Text &= "<tr bgcolor='white'><td>No Companies Found</td></tr>"
          End If
          view_21_8.Text &= "</table></div>"

          view_21_8.Visible = True
        End If

        HoldTable.Dispose()
      End If

        'End If

        Session("Last_Yacht_Crossover_Tab") = Me.view_21_tabcontain.ActiveTabIndex


        If Me.view_21_tabcontain.ActiveTabIndex = 1 Or Me.view_21_tabcontain.ActiveTabIndex = 2 Or Me.view_21_tabcontain.ActiveTabIndex = 4 Or Me.view_21_tabcontain.ActiveTabIndex = 5 Or Me.view_21_tabcontain.ActiveTabIndex = 3 Or Me.view_21_tabcontain.ActiveTabIndex = 6 Then
          If Me.view_21_tabcontain.ActiveTabIndex = 1 Or Me.view_21_tabcontain.ActiveTabIndex = 2 Or Me.view_21_tabcontain.ActiveTabIndex = 4 Or Me.view_21_tabcontain.ActiveTabIndex = 5 Or Me.view_21_tabcontain.ActiveTabIndex = 3 Or Me.view_21_tabcontain.ActiveTabIndex = 6 Then
            view_21_tab1.Visible = True
            view_21_hide_left.Visible = True
            'ElseIf Me.view_21_tabcontain.ActiveTabIndex = 2 Then
            '    Me.make_model_Tab_container.Visible = False ' for now 
          End If
        End If

        If Me.view_21_tabcontain.ActiveTabIndex = 0 Then
          Me.view_21_tab1.Visible = False
          Me.view_21_tab2.Visible = False
          Me.view_21_tab3.Visible = False
          Me.view_21_tab4.Visible = False
          Me.view_21_tab5.Visible = False
          Me.view_21_tab6.Visible = False
          Me.view_21_tab7.Visible = False
          Me.view_21_tab8.Visible = False
          Me.view_21_tab100.Visible = False
        ElseIf Me.view_21_tabcontain.ActiveTabIndex <> 100 And Me.view_21_tabcontain.ActiveTabIndex <> 101 Then
          'make the extra tab invisible
          Me.view_21_tab100.Visible = False
        End If

        HoldTable = Nothing



    Catch ex As Exception

    End Try
  End Sub
  Public Function convert_metric_to_us(ByVal metric As Double) As String
    convert_metric_to_us = ""

    Dim english As Double
    Dim feet As Integer
    Dim inches As Integer


    english = (metric * 3.28084)
    feet = Int(english)
    inches = (english - feet) * 12
    inches = FormatNumber(inches, 0)

    convert_metric_to_us = feet & "' " & inches & "' "
  End Function
  Public Sub make_pie_charts_dbi_both(ByRef tab_for_charts As AjaxControlToolkit.TabPanel, ByRef label_for_list As Label, ByRef graph_div1 As String, ByRef graph_div2 As String, ByVal amod_id As Integer, ByRef label_for_list_tab2 As Label, ByVal make_name As String)
    Dim HoldTable As New DataTable
    Dim google_map_string As String = ""
    Dim htmlOut As New StringBuilder
    Dim heli_count As Integer = 0
    Dim temp_string As String = ""
    Dim charting_string As String = ""
    Dim toggleRowColor As Boolean = True

    Try



      HoldTable = localDatalayer.get_summary_ac_business_types()

      If Not IsNothing(HoldTable) Then
        If HoldTable.Rows.Count > 0 Then 

          google_map_string = " ['AC Business Type', 'Total Count']"

          htmlOut.Append("<div valign=""top"" style='height:280px; overflow: auto;'>")
          htmlOut.Append("<table width='100%' cellpadding='3' cellspacing='0' class='data_aircraft_grid'>")
          htmlOut.Append("<tr class='header_row'>")
          htmlOut.Append("<td align='left' valign='top'><b>Aircraft Models Owned</b></td><td align='right'>Count&nbsp;</td>")
          htmlOut.Append("</tr>")

          For Each Row As DataRow In HoldTable.Rows

            If Not toggleRowColor Then
              htmlOut.Append("<tr class='alt_row'>")
              toggleRowColor = True
            Else
              htmlOut.Append("<tr bgcolor='white'>")
              toggleRowColor = False
            End If

            htmlOut.Append("<td align='left'>")
            htmlOut.Append(Row.Item("cbus_name"))
            htmlOut.Append("&nbsp;</td>")
            htmlOut.Append("<td align='right'>") 
            htmlOut.Append(Row.Item("tcount") & "&nbsp;</td>")
            htmlOut.Append("</tr>")

            google_map_string &= ", ['" & Replace(Row.Item("cbus_name"), "'", "") & "', " & Row.Item("tcount") & "]"


          Next

          htmlOut.Append("</table></div>")

          'charting_string is passed in and the string returned for each, must be named _all for it to ignore them on the way back
          DisplayFunctions.load_google_chart(graph_panel, google_map_string, "", "Total Count", graph_div1, 480, 380, "ARRAY", 1, charting_string, Me.Page, Me.graph_update_panel, False, False, False, False, False, False, False, False, False, True, 1)
          google_map_string = ""

          'charting_string is passed in and the string returned for each, must be named _all for it to ignore them on the way back
          '  DisplayFunctions.load_google_chart(graph_panel, google_map_string, "", "", graph_div2, 550, 275, "ARRAY", 2, charting_string, Me.Page, Me.graph_update_panel, False, False, False, False, False, False, False, False, False, True, Me.view_21_tabcontain.ActiveTabIndex)

          '--------------------- BUILD CHART-----------------------
          Call load_google_chart_all(tab_for_charts, charting_string)
          '--------------------- BUILD CHART-----------------------

          label_for_list.Text = htmlOut.ToString 
        End If

        label_for_list_tab2.Visible = False

      End If


    Catch ex As Exception

    End Try

  End Sub

  Public Sub make_pie_charts_indv_own(ByRef tab_for_charts As AjaxControlToolkit.TabPanel, ByRef label_for_list As Label, ByRef graph_div1 As String, ByRef graph_div2 As String, ByVal amod_id As Integer, ByRef label_for_list_tab2 As Label, ByVal make_name As String)
    Dim HoldTable As New DataTable
    Dim google_map_string As String = ""
    Dim htmlOut As New StringBuilder
    Dim heli_count As Integer = 0
    Dim temp_string As String = ""
    Dim charting_string As String = ""
    Dim toggleRowColor As Boolean = True
    Dim count_from_shared As Integer = 0

    Try

      google_map_string = " ['AC Ownership Type', 'Total Count']"

      HoldTable = localDatalayer.get_summary_whole_vs_fractional("'00','08','97'")
      If Not IsNothing(HoldTable) Then
        If HoldTable.Rows.Count > 0 Then 
          For Each Row As DataRow In HoldTable.Rows

            ' If Trim(Row.Item("tcount")) <> "" Then
            '   count_from_shared = cint(cint(count_from_shared) + cint(Row.Item("tcount"))
            ' Else
            google_map_string &= ", ['" & Trim(Row.Item("ac_own_type").ToString) & "', " & Row.Item("tcount") & "]"
            ' End If 

          Next

          ' If count_from_shared > 0 Then
          'google_map_string &= ", ['Fractional Ownership Program', " & count_from_shared & "]"
          'End If


        End If
      End If


      'charting_string is passed in and the string returned for each, must be named _all for it to ignore them on the way back
      DisplayFunctions.load_google_chart(graph_panel, google_map_string, "", "Total Count", graph_div1, 480, 280, "ARRAY", 1, charting_string, Me.Page, Me.graph_update_panel, False, False, False, False, False, False, False, False, False, True, 1)
      google_map_string = ""

      'charting_string is passed in and the string returned for each, must be named _all for it to ignore them on the way back
      '  DisplayFunctions.load_google_chart(graph_panel, google_map_string, "", "", graph_div2, 550, 275, "ARRAY", 2, charting_string, Me.Page, Me.graph_update_panel, False, False, False, False, False, False, False, False, False, True, Me.view_21_tabcontain.ActiveTabIndex)

      '--------------------- BUILD CHART-----------------------
      Call load_google_chart_all(tab_for_charts, charting_string)
      '--------------------- BUILD CHART-----------------------

      label_for_list.Text = htmlOut.ToString


      label_for_list_tab2.Visible = False



      '-------------------------------------------------------
      temp_string = ""
      HoldTable = localDatalayer.get_summary_ac_models_owned_by_ind("")

      If Not IsNothing(HoldTable) Then
        If HoldTable.Rows.Count > 0 Then

          htmlOut.Append("<div valign=""top"" style='height:280px; overflow: auto;'>")
          htmlOut.Append("<table width='100%' cellpadding='3' cellspacing='0' class='data_aircraft_grid'>")
          htmlOut.Append("<tr class='header_row'>")
          htmlOut.Append("<td align='left' valign='top'><b>Aircraft Owners by Model</b></td><td align='right'>Count&nbsp;</td>")
          htmlOut.Append("</tr>")

          For Each Row As DataRow In HoldTable.Rows

            If Not toggleRowColor Then
              htmlOut.Append("<tr class='alt_row'>")
              toggleRowColor = True
            Else
              htmlOut.Append("<tr bgcolor='white'>")
              toggleRowColor = False
            End If

            htmlOut.Append("<td align='left'>")
            ' htmlOut.Append("<a href='Yacht_View_Template.aspx?ViewID=21&ViewName=Yacht/Aircraft Industry Crossovers&activetab=100&amod_id=" & Row.Item("amod_id") & "&model_name=" & Row.Item("amod_make_name") & " " & Row.Item("amod_model_name") & "' onclick=""ChangeTheMouseCursorOnItemParentDocument('cursor_wait')"">")
            htmlOut.Append(Row.Item("amod_make_name") & " " & Row.Item("amod_model_name"))
            ' htmlOut.Append("</a>")
            htmlOut.Append("&nbsp;</td>")
            htmlOut.Append("<td align='right'>")
            ' htmlOut.Append("<a href='Yacht_View_Template.aspx?ViewID=21&ViewName=Yacht/Aircraft Industry Crossovers&activetab=100&amod_id=" & Row.Item("amod_id") & "&model_name=" & Row.Item("amod_make_name") & " " & Row.Item("amod_model_name") & "' onclick=""ChangeTheMouseCursorOnItemParentDocument('cursor_wait')"">")
            htmlOut.Append(Row.Item("acount") & "")
            ' htmlOut.Append("</a>")
            htmlOut.Append("&nbsp;</td>")
            htmlOut.Append("</tr>")

          Next

          htmlOut.Append("</table></div>") 
          label_for_list_tab2.Text = htmlOut.ToString

        End If

      End If

      htmlOut.Length = 0

      temp_string = ""
      HoldTable = localDatalayer.get_summary_ac_models_owned_by_ind("Make")

      If Not IsNothing(HoldTable) Then
        If HoldTable.Rows.Count > 0 Then

          htmlOut.Append("<div valign=""top"" style='height:280px; overflow: auto;'>")
          htmlOut.Append("<table width='100%' cellpadding='3' cellspacing='0' class='data_aircraft_grid'>")
          htmlOut.Append("<tr class='header_row'>")
          htmlOut.Append("<td align='left' valign='top'><b>Aircraft Owners by Make</b></td><td align='right'>Count&nbsp;</td>")
          htmlOut.Append("</tr>")

          For Each Row As DataRow In HoldTable.Rows

            If Not toggleRowColor Then
              htmlOut.Append("<tr class='alt_row'>")
              toggleRowColor = True
            Else
              htmlOut.Append("<tr bgcolor='white'>")
              toggleRowColor = False
            End If

            htmlOut.Append("<td align='left'>")
            '  htmlOut.Append("<a href='Yacht_View_Template.aspx?ViewID=21&ViewName=Yacht/Aircraft Industry Crossovers&activetab=100&amod_id=0&make_name=" & Row.Item("amod_make_name") & "&model_name=" & Row.Item("amod_make_name") & "' onclick=""ChangeTheMouseCursorOnItemParentDocument('cursor_wait')"">")
            htmlOut.Append(Row.Item("amod_make_name"))
            ' htmlOut.Append("</a>")
            htmlOut.Append("&nbsp;</td>")
            htmlOut.Append("<td align='right'>")
            'htmlOut.Append("<a href='Yacht_View_Template.aspx?ViewID=21&ViewName=Yacht/Aircraft Industry Crossovers&activetab=100&amod_id=0&make_name=" & Row.Item("amod_make_name") & "&model_name=" & Row.Item("amod_make_name") & "' onclick=""ChangeTheMouseCursorOnItemParentDocument('cursor_wait')"">")
            htmlOut.Append(Row.Item("acount") & "")
            ' htmlOut.Append("</a>")
            htmlOut.Append("&nbsp;</td>")
            htmlOut.Append("</tr>") 
          Next

          htmlOut.Append("</table></div>")
          label_for_list.Text = htmlOut.ToString

        End If

      End If



    Catch ex As Exception

    End Try

  End Sub



  Public Sub make_pie_charts_company(ByRef tab_for_charts As AjaxControlToolkit.TabPanel, ByRef label_for_list As Label, ByRef graph_div1 As String, ByRef graph_div2 As String, ByVal amod_id As Integer, ByRef label_for_list_tab2 As Label, ByVal make_name As String)
    Dim HoldTable As New DataTable
    Dim google_map_string As String = ""
    Dim htmlOut As New StringBuilder
    Dim heli_count As Integer = 0
    Dim temp_string As String = ""
    Dim charting_string As String = ""
    Dim toggleRowColor As Boolean = True

    Try


      If amod_id > 0 Or Trim(make_name) <> "" Then
        '--------------------------------------------------------------------------------------
        HoldTable = localDatalayer.get_summary_yacht_class_company(amod_id, make_name)

        If Not IsNothing(HoldTable) Then
          If HoldTable.Rows.Count > 0 Then
            'htmlOut.Append("<div valign=""top"" style='height:" & temp_height & "px; overflow: auto;'>")

            google_map_string = " ['Yacht Class', 'Total Count']"

            For Each Row As DataRow In HoldTable.Rows

              google_map_string &= ", ['" & Replace(Row.Item("YachtClass"), "'", "") & "', " & Row.Item("ycount") & "]"

            Next
          End If
        End If

        'charting_string is passed in and the string returned for each, must be named _all for it to ignore them on the way back
        DisplayFunctions.load_google_chart(graph_panel, google_map_string, "", "Total Count", graph_div1, 550, 275, "ARRAY", 1, charting_string, Me.Page, Me.graph_update_panel, False, False, False, False, False, False, False, False, False, True, 1)
        google_map_string = ""

        HoldTable = localDatalayer.get_summary_yacht_by_brand_company(amod_id, make_name)

        If Not IsNothing(HoldTable) Then
          If HoldTable.Rows.Count > 0 Then
            'htmlOut.Append("<div valign=""top"" style='height:" & temp_height & "px; overflow: auto;'>")

            google_map_string = " ['Yacht Brand', 'Total Count']"

            For Each Row As DataRow In HoldTable.Rows

              google_map_string &= ", ['" & Replace(Row.Item("ym_brand_name"), "'", "") & "', " & Row.Item("ycount") & "]"

            Next
          End If
        End If

        'charting_string is passed in and the string returned for each, must be named _all for it to ignore them on the way back
        DisplayFunctions.load_google_chart(graph_panel, google_map_string, "", "", graph_div2, 550, 275, "ARRAY", 2, charting_string, Me.Page, Me.graph_update_panel, False, False, False, False, False, False, False, False, False, True, 1)

        ''--------------------- BUILD CHART-----------------------
        Call load_google_chart_all(tab_for_charts, charting_string)
        ''--------------------- BUILD CHART-----------------------

        google_map_string = ""


        temp_string = ""
        HoldTable = localDatalayer.get_summary_yachts_owned_by_ac_model_company(amod_id, make_name)

        If Not IsNothing(HoldTable) Then
          If HoldTable.Rows.Count > 0 Then

            htmlOut.Append("<div valign=""top"" style='height:280px; overflow: auto;'>")
            htmlOut.Append("<table width='100%' cellpadding='3' cellspacing='0' class='data_aircraft_grid'>")
            htmlOut.Append("<tr class='header_row'>")
            htmlOut.Append("<td align='left' valign='top'><b>Yachts Owned (" & HoldTable.Rows.Count & ")</b></td>")
            htmlOut.Append("</tr>")

            For Each Row As DataRow In HoldTable.Rows

              If Not toggleRowColor Then
                htmlOut.Append("<tr class='alt_row'>")
                toggleRowColor = True
              Else
                htmlOut.Append("<tr bgcolor='white'>")
                toggleRowColor = False
              End If

              htmlOut.Append("<td align='left'>")
              htmlOut.Append(Row.Item("ym_brand_name") & " ")
              htmlOut.Append(Row.Item("ym_model_name") & " ")
              htmlOut.Append(DisplayFunctions.WriteYachtDetailsLink(Row.Item("yt_id"), True, "", "", "") & ">")
              htmlOut.Append(Row.Item("yt_yacht_name"))
              htmlOut.Append("</a>")
              htmlOut.Append(", Hull#: " & Row.Item("yt_hull_mfr_nbr"))
              htmlOut.Append(", Year: " & Row.Item("yt_year_mfr"))
              htmlOut.Append("</td></tr>")
            Next

            htmlOut.Append("</table></div>")
            label_for_list.Text = htmlOut.ToString

          End If
        End If


        '--------------------------------------------------------------------------------------
      Else
        '--------------------------------------------------------------------------------------
        HoldTable = localDatalayer.get_summary_by_ac_type_company()

        If Not IsNothing(HoldTable) Then
          If HoldTable.Rows.Count > 0 Then
            'htmlOut.Append("<div valign=""top"" style='height:" & temp_height & "px; overflow: auto;'>")

            google_map_string = " ['AC Type', 'Total Count']"

            For Each Row As DataRow In HoldTable.Rows

              If Trim(Row.Item("amod_airframe_type_code")) = "Helicopter" Then
                If heli_count > 0 Then
                  heli_count = heli_count + CInt(Row.Item("acount"))
                  google_map_string &= ", ['Helicopter', " & Row.Item("acount") & "]"
                Else
                  heli_count = CInt(Row.Item("acount"))
                End If
              Else
                google_map_string &= ", ['" & Row.Item("atype_name") & "', " & Row.Item("acount") & "]"
              End If

            Next
          End If
        End If

        Session("YACHT_AC_Type") = ""
        Session("YACHT_AC_Type") = google_map_string


        'charting_string is passed in and the string returned for each, must be named _all for it to ignore them on the way back
        DisplayFunctions.load_google_chart(graph_panel, google_map_string, "", "Model Count", graph_div1, 220, 220, "ARRAY", 1, charting_string, Me.Page, Me.graph_update_panel, False, False, False, False, False, True, False, False, False, True, 1)
        google_map_string = ""



        HoldTable = localDatalayer.get_summary_jets_by_weight_class_company()

        If Not IsNothing(HoldTable) Then
          If HoldTable.Rows.Count > 0 Then
            'htmlOut.Append("<div valign=""top"" style='height:" & temp_height & "px; overflow: auto;'>")

            google_map_string = " ['Weight Class', 'Total Count']"

            For Each Row As DataRow In HoldTable.Rows

              google_map_string &= ", ['" & Row.Item("acwgtcls_name") & "', " & Row.Item("acount") & "]"

            Next
          End If
        End If

        Session("YACHT_AC_Weight") = ""
        Session("YACHT_AC_Weight") = google_map_string

        DisplayFunctions.load_google_chart(graph_panel, google_map_string, "", "", graph_div2, 220, 220, "ARRAY", 2, charting_string, Me.Page, Me.graph_update_panel, False, False, False, False, False, True, False, False, False, True, 1)

        '--------------------- BUILD CHART-----------------------
        Call load_google_chart_all(tab_for_charts, charting_string)
        '--------------------- BUILD CHART-----------------------




        temp_string = ""
        HoldTable = localDatalayer.get_summary_ac_models_owned_company()

        If Not IsNothing(HoldTable) Then
          If HoldTable.Rows.Count > 0 Then

            htmlOut.Append("<div valign=""top"" style='height:280px; overflow: auto;'>")
            htmlOut.Append("<table width='100%' cellpadding='3' cellspacing='0' class='data_aircraft_grid'>")
            htmlOut.Append("<tr class='header_row'>")
            htmlOut.Append("<td align='left' valign='top'><b>Aircraft Models Owned</b></td><td align='right'>Count&nbsp;</td>")
            htmlOut.Append("</tr>")

            For Each Row As DataRow In HoldTable.Rows

              If Not toggleRowColor Then
                htmlOut.Append("<tr class='alt_row'>")
                toggleRowColor = True
              Else
                htmlOut.Append("<tr bgcolor='white'>")
                toggleRowColor = False
              End If

              htmlOut.Append("<td align='left'>")
              htmlOut.Append("<a href='Yacht_View_Template.aspx?ViewID=21&ViewName=Yacht/Aircraft Industry Crossovers&activetab=101&amod_id=" & Row.Item("amod_id") & "&model_name=" & Row.Item("amod_make_name") & " " & Row.Item("amod_model_name") & "' onclick=""ChangeTheMouseCursorOnItemParentDocument('cursor_wait')"">")
              htmlOut.Append(Row.Item("amod_make_name") & " " & Row.Item("amod_model_name"))
              htmlOut.Append("</a>&nbsp;</td>")
              htmlOut.Append("<td align='right'>")
              htmlOut.Append("<a href='Yacht_View_Template.aspx?ViewID=21&ViewName=Yacht/Aircraft Industry Crossovers&activetab=101&amod_id=" & Row.Item("amod_id") & "&model_name=" & Row.Item("amod_make_name") & " " & Row.Item("amod_model_name") & "' onclick=""ChangeTheMouseCursorOnItemParentDocument('cursor_wait')"">")
              htmlOut.Append(Row.Item("acount") & "</a>&nbsp;</td>")
              htmlOut.Append("</tr>")

            Next

            htmlOut.Append("</table></div>")
            label_for_list.Text = htmlOut.ToString

          End If

        End If

        htmlOut.Length = 0

        temp_string = ""
        HoldTable = localDatalayer.get_summary_ac_makes_owned_company()

        If Not IsNothing(HoldTable) Then
          If HoldTable.Rows.Count > 0 Then

            htmlOut.Append("<div valign=""top"" style='height:280px; overflow: auto;'>")
            htmlOut.Append("<table width='100%' cellpadding='3' cellspacing='0' class='data_aircraft_grid'>")
            htmlOut.Append("<tr class='header_row'>")
            htmlOut.Append("<td align='left' valign='top'><b>Aircraft Models Owned</b></td><td align='right'>Count&nbsp;</td>")
            htmlOut.Append("</tr>")

            For Each Row As DataRow In HoldTable.Rows

              If Not toggleRowColor Then
                htmlOut.Append("<tr class='alt_row'>")
                toggleRowColor = True
              Else
                htmlOut.Append("<tr bgcolor='white'>")
                toggleRowColor = False
              End If

              htmlOut.Append("<td align='left'>")
              htmlOut.Append("<a href='Yacht_View_Template.aspx?ViewID=21&ViewName=Yacht/Aircraft Industry Crossovers&activetab=101&amod_id=0&make_name=" & Row.Item("amod_make_name") & "&model_name=" & Row.Item("amod_make_name") & "' onclick=""ChangeTheMouseCursorOnItemParentDocument('cursor_wait')"">")
              htmlOut.Append(Row.Item("amod_make_name"))
              htmlOut.Append("</a>&nbsp;</td>")
              htmlOut.Append("<td align='right'>")
              htmlOut.Append("<a href='Yacht_View_Template.aspx?ViewID=21&ViewName=Yacht/Aircraft Industry Crossovers&activetab=101&amod_id=0&make_name=" & Row.Item("amod_make_name") & "&model_name=" & Row.Item("amod_make_name") & "' onclick=""ChangeTheMouseCursorOnItemParentDocument('cursor_wait')"">")
              htmlOut.Append(Row.Item("acount") & "</a>&nbsp;</td>")
              htmlOut.Append("</tr>")

            Next

            htmlOut.Append("</table></div>")
            label_for_list_tab2.Text = htmlOut.ToString

          End If

        End If



      End If

    Catch ex As Exception

    End Try

  End Sub

  Public Sub make_pie_charts(ByRef tab_for_charts As AjaxControlToolkit.TabPanel, ByRef label_for_list As Label, ByRef graph_div1 As String, ByRef graph_div2 As String, ByVal amod_id As Integer, ByRef label_for_list_tab2 As Label, ByVal make_name As String)
    Dim HoldTable As New DataTable
    Dim google_map_string As String = ""
    Dim htmlOut As New StringBuilder
    Dim heli_count As Integer = 0
    Dim temp_string As String = ""
    Dim charting_string As String = ""
    Dim toggleRowColor As Boolean = True

    Try


      If amod_id > 0 Or Trim(make_name) <> "" Then
        '--------------------------------------------------------------------------------------
        HoldTable = localDatalayer.get_summary_yacht_class(amod_id, make_name)

        If Not IsNothing(HoldTable) Then
          If HoldTable.Rows.Count > 0 Then
            'htmlOut.Append("<div valign=""top"" style='height:" & temp_height & "px; overflow: auto;'>")

            google_map_string = " ['Yacht Class', 'Total Count']"

            For Each Row As DataRow In HoldTable.Rows

              google_map_string &= ", ['" & Replace(Row.Item("YachtClass"), "'", "") & "', " & Row.Item("ycount") & "]"

            Next
          End If
        End If

        'charting_string is passed in and the string returned for each, must be named _all for it to ignore them on the way back
        DisplayFunctions.load_google_chart(graph_panel, google_map_string, "", "Total Count", graph_div1, 550, 275, "ARRAY", 1, charting_string, Me.Page, Me.graph_update_panel, False, False, False, False, False, False, False, False, False, True, Me.view_21_tabcontain.ActiveTabIndex)
        google_map_string = ""

        HoldTable = localDatalayer.get_summary_yacht_by_brand(amod_id, make_name)

        If Not IsNothing(HoldTable) Then
          If HoldTable.Rows.Count > 0 Then
            'htmlOut.Append("<div valign=""top"" style='height:" & temp_height & "px; overflow: auto;'>")

            google_map_string = " ['Yacht Class', 'Total Count']"

            For Each Row As DataRow In HoldTable.Rows

              google_map_string &= ", ['" & Replace(Row.Item("ym_brand_name"), "'", "") & "', " & Row.Item("ycount") & "]"

            Next
          End If
        End If

        'charting_string is passed in and the string returned for each, must be named _all for it to ignore them on the way back
        DisplayFunctions.load_google_chart(graph_panel, google_map_string, "", "", graph_div2, 550, 275, "ARRAY", 2, charting_string, Me.Page, Me.graph_update_panel, False, False, False, False, False, False, False, False, False, True, Me.view_21_tabcontain.ActiveTabIndex)

        '--------------------- BUILD CHART-----------------------
        Call load_google_chart_all(tab_for_charts, charting_string)
        '--------------------- BUILD CHART-----------------------

        google_map_string = ""


        temp_string = ""
        HoldTable = localDatalayer.get_summary_yachts_owned_by_ac_model(amod_id, make_name)

        If Not IsNothing(HoldTable) Then
          If HoldTable.Rows.Count > 0 Then

            htmlOut.Append("<div valign=""top"" style='height:280px; overflow: auto;'>")
            htmlOut.Append("<table width='100%' cellpadding='3' cellspacing='0' class='data_aircraft_grid'>")
            htmlOut.Append("<tr class='header_row'>")
            htmlOut.Append("<td align='left' valign='top'><b>Yachts Owned (" & HoldTable.Rows.Count & ")</b></td>")
            htmlOut.Append("</tr>")

            For Each Row As DataRow In HoldTable.Rows

              If Not toggleRowColor Then
                htmlOut.Append("<tr class='alt_row'>")
                toggleRowColor = True
              Else
                htmlOut.Append("<tr bgcolor='white'>")
                toggleRowColor = False
              End If

              htmlOut.Append("<td align='left'>")
              htmlOut.Append(Row.Item("ym_brand_name") & " ")
              htmlOut.Append(Row.Item("ym_model_name") & " ")
              htmlOut.Append(DisplayFunctions.WriteYachtDetailsLink(Row.Item("yt_id"), True, "", "", "") & ">")
              htmlOut.Append(Row.Item("yt_yacht_name"))
              htmlOut.Append("</a>")
              htmlOut.Append(", Hull#: " & Row.Item("yt_hull_mfr_nbr"))
              htmlOut.Append(", Year: " & Row.Item("yt_year_mfr"))
              htmlOut.Append("</td></tr>")
            Next

            htmlOut.Append("</table></div>")
            label_for_list.Text = htmlOut.ToString

          End If
        End If


        '--------------------------------------------------------------------------------------
      Else
        '--------------------------------------------------------------------------------------
        HoldTable = localDatalayer.get_summary_by_ac_type()

        If Not IsNothing(HoldTable) Then
          If HoldTable.Rows.Count > 0 Then
            'htmlOut.Append("<div valign=""top"" style='height:" & temp_height & "px; overflow: auto;'>")

            google_map_string = " ['AC Type', 'Total Count']"

            For Each Row As DataRow In HoldTable.Rows

              If Trim(Row.Item("amod_airframe_type_code")) = "Helicopter" Then
                If heli_count > 0 Then
                  heli_count = heli_count + CInt(Row.Item("acount"))
                  google_map_string &= ", ['Helicopter', " & Row.Item("acount") & "]"
                Else
                  heli_count = CInt(Row.Item("acount"))
                End If
              Else
                google_map_string &= ", ['" & Row.Item("atype_name") & "', " & Row.Item("acount") & "]"
              End If

            Next
          End If
        End If

        Session("YACHT_AC_Type") = ""
        Session("YACHT_AC_Type") = google_map_string


        'charting_string is passed in and the string returned for each, must be named _all for it to ignore them on the way back
        DisplayFunctions.load_google_chart(graph_panel, google_map_string, "", "Model Count", graph_div1, 220, 220, "ARRAY", 1, charting_string, Me.Page, Me.graph_update_panel, False, False, False, False, False, True, False, False, False, True, Me.view_21_tabcontain.ActiveTabIndex)
        google_map_string = ""



        HoldTable = localDatalayer.get_summary_jets_by_weight_class()

        If Not IsNothing(HoldTable) Then
          If HoldTable.Rows.Count > 0 Then
            'htmlOut.Append("<div valign=""top"" style='height:" & temp_height & "px; overflow: auto;'>")

            google_map_string = " ['Weight Class', 'Total Count']"

            For Each Row As DataRow In HoldTable.Rows

              google_map_string &= ", ['" & Row.Item("acwgtcls_name") & "', " & Row.Item("acount") & "]"

            Next
          End If
        End If

        Session("YACHT_AC_Weight") = ""
        Session("YACHT_AC_Weight") = google_map_string

        DisplayFunctions.load_google_chart(graph_panel, google_map_string, "", "", graph_div2, 220, 220, "ARRAY", 2, charting_string, Me.Page, Me.graph_update_panel, False, False, False, False, False, True, False, False, False, True, Me.view_21_tabcontain.ActiveTabIndex)

        '--------------------- BUILD CHART-----------------------
        Call load_google_chart_all(tab_for_charts, charting_string)
        '--------------------- BUILD CHART-----------------------




        temp_string = ""
        HoldTable = localDatalayer.get_summary_ac_models_owned()

        If Not IsNothing(HoldTable) Then
          If HoldTable.Rows.Count > 0 Then

            htmlOut.Append("<div valign=""top"" style='height:280px; overflow: auto;'>")
            htmlOut.Append("<table width='100%' cellpadding='3' cellspacing='0' class='data_aircraft_grid'>")
            htmlOut.Append("<tr class='header_row'>")
            htmlOut.Append("<td align='left' valign='top'><b>Aircraft Models Owned</b></td><td align='right'># of Aircraft&nbsp;</td>")
            htmlOut.Append("</tr>")

            For Each Row As DataRow In HoldTable.Rows

              If Not toggleRowColor Then
                htmlOut.Append("<tr class='alt_row'>")
                toggleRowColor = True
              Else
                htmlOut.Append("<tr bgcolor='white'>")
                toggleRowColor = False
              End If

              htmlOut.Append("<td align='left'>")
              htmlOut.Append("<a href='Yacht_View_Template.aspx?ViewID=21&ViewName=Yacht/Aircraft Industry Crossovers&activetab=100&amod_id=" & Row.Item("amod_id") & "&model_name=" & Row.Item("amod_make_name") & " " & Row.Item("amod_model_name") & "' onclick=""ChangeTheMouseCursorOnItemParentDocument('cursor_wait')"">")
              htmlOut.Append(Row.Item("amod_make_name") & " " & Row.Item("amod_model_name"))
              htmlOut.Append("</a>&nbsp;</td>")
              htmlOut.Append("<td align='right'>")
              htmlOut.Append("<a href='Yacht_View_Template.aspx?ViewID=21&ViewName=Yacht/Aircraft Industry Crossovers&activetab=100&amod_id=" & Row.Item("amod_id") & "&model_name=" & Row.Item("amod_make_name") & " " & Row.Item("amod_model_name") & "' onclick=""ChangeTheMouseCursorOnItemParentDocument('cursor_wait')"">")
              htmlOut.Append(Row.Item("acount") & "</a>&nbsp;</td>")
              htmlOut.Append("</tr>")

            Next

            htmlOut.Append("</table></div>")
            label_for_list.Text = htmlOut.ToString

          End If

        End If

        htmlOut.Length = 0

        temp_string = ""
        HoldTable = localDatalayer.get_summary_ac_makes_owned()

        If Not IsNothing(HoldTable) Then
          If HoldTable.Rows.Count > 0 Then

            htmlOut.Append("<div valign=""top"" style='height:280px; overflow: auto;'>")
            htmlOut.Append("<table width='100%' cellpadding='3' cellspacing='0' class='data_aircraft_grid'>")
            htmlOut.Append("<tr class='header_row'>")
            htmlOut.Append("<td align='left' valign='top'><b>Aircraft Models Owned</b></td><td align='right'>Count&nbsp;</td>")
            htmlOut.Append("</tr>")

            For Each Row As DataRow In HoldTable.Rows

              If Not toggleRowColor Then
                htmlOut.Append("<tr class='alt_row'>")
                toggleRowColor = True
              Else
                htmlOut.Append("<tr bgcolor='white'>")
                toggleRowColor = False
              End If

              htmlOut.Append("<td align='left'>")
              htmlOut.Append("<a href='Yacht_View_Template.aspx?ViewID=21&ViewName=Yacht/Aircraft Industry Crossovers&activetab=100&amod_id=0&make_name=" & Row.Item("amod_make_name") & "&model_name=" & Row.Item("amod_make_name") & "' onclick=""ChangeTheMouseCursorOnItemParentDocument('cursor_wait')"">")
              htmlOut.Append(Row.Item("amod_make_name"))
              htmlOut.Append("</a>&nbsp;</td>")
              htmlOut.Append("<td align='right'>")
              htmlOut.Append("<a href='Yacht_View_Template.aspx?ViewID=21&ViewName=Yacht/Aircraft Industry Crossovers&activetab=100&amod_id=0&make_name=" & Row.Item("amod_make_name") & "&model_name=" & Row.Item("amod_make_name") & "' onclick=""ChangeTheMouseCursorOnItemParentDocument('cursor_wait')"">")
              htmlOut.Append(Row.Item("acount") & "</a>&nbsp;</td>")
              htmlOut.Append("</tr>")

            Next

            htmlOut.Append("</table></div>")
            label_for_list_tab2.Text = htmlOut.ToString

          End If

        End If



      End If

    Catch ex As Exception

    End Try

  End Sub

  Public Sub make_pie_charts_no_ac(ByRef tab_for_charts As AjaxControlToolkit.TabPanel, ByRef label_for_list As Label, ByRef graph_div1 As String, ByRef graph_div2 As String, ByVal amod_id As Integer, ByRef label_for_list_tab2 As Label, ByVal make_name As String, ByVal type_string As String, ByRef div_for_yacht_size As String, ByVal yacht_size As String)
    Dim HoldTable As New DataTable
    Dim google_map_string As String = ""
    Dim htmlOut As New StringBuilder
    Dim heli_count As Integer = 0
    Dim temp_string As String = ""
    Dim charting_string As String = ""
    Dim toggleRowColor As Boolean = True

    Try
 


      HoldTable = localDatalayer.get_summary_yachts_by("ycs_description", type_string, yacht_size)

      If Not IsNothing(HoldTable) Then


        If Trim(yacht_size) <> "" Then
          div_for_yacht_size = "<div valign=""top"" style='height:50px; overflow: auto;'>"
        Else
          div_for_yacht_size = "<div valign=""top"" style='height:120px; overflow: auto;'>"
        End If



        div_for_yacht_size &= "<table width='100%' cellpadding='3' cellspacing='0' class='data_aircraft_grid'>"
        div_for_yacht_size &= "<tr class='header_row'>"
        div_for_yacht_size &= "<td align='left' valign='top'><b>Yacht Model Size</b></td>"
        div_for_yacht_size &= "<td align='right' valign='top'><b>Count</b></td>"
        div_for_yacht_size &= "</tr>"


        If HoldTable.Rows.Count > 0 Then
          'htmlOut.Append("<div valign=""top"" style='height:" & temp_height & "px; overflow: auto;'>")

          google_map_string = " ['Yacht Size', 'Total Count']"

          For Each Row As DataRow In HoldTable.Rows

            If Not toggleRowColor Then
              div_for_yacht_size &= "<tr class='alt_row'>"
              toggleRowColor = True
            Else
              div_for_yacht_size &= "<tr bgcolor='white'>"
              toggleRowColor = False
            End If

            div_for_yacht_size &= "<td align='left'>"
            div_for_yacht_size &= "<a href='Yacht_View_Template.aspx?ViewID=21&ViewName=Yacht/Aircraft Industry Crossovers&activetab=3&yacht_size=" & Left(Trim(Row.Item("ycs_description")), 1) & "' onclick=""ChangeTheMouseCursorOnItemParentDocument('cursor_wait')"">"
            div_for_yacht_size &= Replace(Row.Item("ycs_description"), "'", "")
            div_for_yacht_size &= "</a>&nbsp;"

            If Trim(yacht_size) <> "" Then
              div_for_yacht_size &= " - ("
              div_for_yacht_size &= "<a href='Yacht_View_Template.aspx?ViewID=21&ViewName=Yacht/Aircraft Industry Crossovers&activetab=3' onclick=""ChangeTheMouseCursorOnItemParentDocument('cursor_wait')"">"
              div_for_yacht_size &= "Clear</a>)&nbsp;"
            End If

            div_for_yacht_size &= "</td><td align='right'>"
            div_for_yacht_size &= "<a href='Yacht_View_Template.aspx?ViewID=21&ViewName=Yacht/Aircraft Industry Crossovers&activetab=3&yacht_size=" & Left(Trim(Row.Item("ycs_description")), 1) & "' onclick=""ChangeTheMouseCursorOnItemParentDocument('cursor_wait')"">"
            div_for_yacht_size &= Row.Item("acount")
            div_for_yacht_size &= "</a>&nbsp;</td></tr>"
            google_map_string &= ", ['" & Replace(Replace(Row.Item("ycs_description"), "'", ""), "&", "+") & "', " & Row.Item("acount") & "]"

          Next

          div_for_yacht_size += "</table></div>"
        End If
      End If

      '  Session("YACHT_AC_Weight") = ""
      '  Session("YACHT_AC_Weight") = google_map_string
      If Trim(yacht_size) = "" Then
        DisplayFunctions.load_google_chart(graph_panel, google_map_string, "", "Model Count", graph_div1, 430, 300, "ARRAY", 1, charting_string, Me.Page, Me.graph_update_panel, False, False, False, False, False, False, False, False, False, True, 1)
      End If

      HoldTable = localDatalayer.get_summary_yachts_by("ym_brand_name", type_string, yacht_size)

      If Not IsNothing(HoldTable) Then
        If HoldTable.Rows.Count > 0 Then
          'htmlOut.Append("<div valign=""top"" style='height:" & temp_height & "px; overflow: auto;'>")

          google_map_string = " ['Yacht Brand', 'Total Count']"

          For Each Row As DataRow In HoldTable.Rows

            google_map_string &= ", ['" & Replace(Row.Item("ym_brand_name"), "'", "") & "', " & Row.Item("acount") & "]"

          Next
        End If
      End If

      DisplayFunctions.load_google_chart(graph_panel, google_map_string, "", "", graph_div2, 430, 300, "ARRAY", 2, charting_string, Me.Page, Me.graph_update_panel, False, False, False, False, False, False, False, False, False, True, 1)

      '--------------------- BUILD CHART-----------------------
      Call load_google_chart_all(tab_for_charts, charting_string)
      '--------------------- BUILD CHART-----------------------



    Catch ex As Exception

    End Try

  End Sub
  Public Sub make_pie_charts_comp_no_ac(ByRef tab_for_charts As AjaxControlToolkit.TabPanel, ByRef label_for_list As Label, ByRef graph_div1 As String, ByRef graph_div2 As String, ByVal amod_id As Integer, ByRef label_for_list_tab2 As Label, ByVal make_name As String, ByVal type_string As String)
    Dim HoldTable As New DataTable
    Dim google_map_string As String = ""
    Dim htmlOut As New StringBuilder
    Dim heli_count As Integer = 0
    Dim temp_string As String = ""
    Dim charting_string As String = ""
    Dim toggleRowColor As Boolean = True

    Try


      HoldTable = localDatalayer.get_summary_yachts_comp_by("ycs_description", type_string)

      If Not IsNothing(HoldTable) Then
        If HoldTable.Rows.Count > 0 Then
          'htmlOut.Append("<div valign=""top"" style='height:" & temp_height & "px; overflow: auto;'>")

          google_map_string = " ['Yacht Size', 'Total Count']"

          For Each Row As DataRow In HoldTable.Rows

            google_map_string &= ", ['" & Replace(Replace(Row.Item("ycs_description"), "'", ""), "&", "+") & "', " & Row.Item("acount") & "]"

          Next
        End If
      End If

      '  Session("YACHT_AC_Weight") = ""
      '  Session("YACHT_AC_Weight") = google_map_string
      DisplayFunctions.load_google_chart(graph_panel, google_map_string, "", "Model Count", graph_div1, 430, 300, "ARRAY", 1, charting_string, Me.Page, Me.graph_update_panel, False, False, False, False, False, False, False, False, False, True, 1)


      HoldTable = localDatalayer.get_summary_yachts_comp_by("ym_brand_name", type_string)

      If Not IsNothing(HoldTable) Then
        If HoldTable.Rows.Count > 0 Then
          'htmlOut.Append("<div valign=""top"" style='height:" & temp_height & "px; overflow: auto;'>")

          google_map_string = " ['Yacht Brand', 'Total Count']"

          For Each Row As DataRow In HoldTable.Rows

            google_map_string &= ", ['" & Replace(Replace(Row.Item("ym_brand_name"), "'", ""), "&", "+") & "', " & Row.Item("acount") & "]"

          Next
        End If
      End If

      DisplayFunctions.load_google_chart(graph_panel, google_map_string, "", "", graph_div2, 430, 300, "ARRAY", 2, charting_string, Me.Page, Me.graph_update_panel, False, False, False, False, False, False, False, False, False, True, 1)

      '--------------------- BUILD CHART-----------------------
      Call load_google_chart_all(tab_for_charts, charting_string)
      '--------------------- BUILD CHART-----------------------



    Catch ex As Exception

    End Try

  End Sub

  Public Sub load_google_chart_all(ByVal tab_to_add_to As AjaxControlToolkit.TabPanel, ByVal string_from_charts As String)
    Dim GoogleChart1TabScript As StringBuilder = New StringBuilder()

    Dim temp_string As String = ""
    Dim label_script As New Label
    Dim chart_label As New Label

    temp_string = "<script type=""text/javascript"">"


    temp_string &= "drawCharts();"


    temp_string &= "function drawCharts() {"

    temp_string &= string_from_charts

    temp_string &= " } "

    'temp_string &= "alert(document.getElementById('" & div_name & "'));"
    temp_string &= "</script>"


    label_script.ID = "label_script"
    label_script.Text = temp_string


    'tab_to_add_to.Controls.AddAt(0, label_script)


    If Not Page.ClientScript.IsClientScriptBlockRegistered("GoogleChart1Tab") Then
      GoogleChart1TabScript.Append(temp_string)

      System.Web.UI.ScriptManager.RegisterStartupScript(Me.bottom_tab_update_panel, Me.GetType(), "GoogleChart1Tab", GoogleChart1TabScript.ToString, False)
    End If


  End Sub
  Private Sub CreatePrototypeDatatables(ByRef TempTable As DataTable, ByVal tableType As Integer)
    TempTable.Columns.Add("FIELD")
    TempTable.Columns.Add("COUNT")
    TempTable.Columns.Add("TAB")
    TempTable.Columns.Add("TYPE_OF")

    Dim newRow As DataRow = TempTable.NewRow()

    If tableType = 1 Then
      newRow("FIELD") = "Individuals Owning/Leasing Yachts/Aircraft"
      'newRow("COUNT") = localDatalayer.get_yacht_crossover_table_count(1)
      newRow("COUNT") = "0"
      newRow("TAB") = "1"
      newRow("TYPE_OF") = ""
      TempTable.Rows.Add(newRow)
      TempTable.AcceptChanges()

      newRow = TempTable.NewRow()
      newRow("FIELD") = "Companies Owning/Leasing Yacht/Aircraft"
      'newRow("COUNT") = localDatalayer.get_yacht_crossover_table_count(2)
      newRow("COUNT") = "0"
      newRow("TAB") = "2"
      newRow("TYPE_OF") = ""
      TempTable.Rows.Add(newRow)
      TempTable.AcceptChanges()

      newRow = TempTable.NewRow()
      newRow("FIELD") = "Companies in Yacht/Aircraft Business"
      ' newRow("COUNT") = localDatalayer.get_yacht_crossover_table_count(3)
      newRow("COUNT") = "0"
      newRow("TAB") = "4"
      newRow("TYPE_OF") = ""
      TempTable.Rows.Add(newRow)
      TempTable.AcceptChanges()
    ElseIf tableType = 2 Then 

      'Session.Item("localSubscription").crmJets_Flag As Boolean: False
      'Session.Item("localSubscription").crmExecutive_Flag As Boolean: False
      'Session.Item("localSubscription").crmTurboprops As Boolean: False

      If HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True Or HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True Or HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
        newRow("FIELD") = "Individuals Owning/Leasing Aircraft, Not Yachts"
        newRow("COUNT") = "1755"
        newRow("TAB") = "5"
        newRow("TYPE_OF") = ""
        TempTable.Rows.Add(newRow)
        TempTable.AcceptChanges()
      End If

      If HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True Or HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
        newRow = TempTable.NewRow()
        newRow("FIELD") = "Individuals Owning/Leasing Jets, Not Yachts"
        newRow("COUNT") = "225"
        newRow("TAB") = "5"
        newRow("TYPE_OF") = "J"
        TempTable.Rows.Add(newRow)
        TempTable.AcceptChanges()
      End If

      If HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True Or HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
        newRow = TempTable.NewRow()
        newRow("FIELD") = "Individuals Owning/Leasing Turbo Props, Not Yachts"
        newRow("COUNT") = "359"
        newRow("TAB") = "5"
        newRow("TYPE_OF") = "T"
        TempTable.Rows.Add(newRow)
        TempTable.AcceptChanges()
      End If

      If HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True Then
        newRow = TempTable.NewRow()
        newRow("FIELD") = "Individuals Owning/Leasing Helicopters, Not Yachts"
        newRow("COUNT") = "917"
        newRow("TAB") = "5"
        newRow("TYPE_OF") = "H"
        TempTable.Rows.Add(newRow)
        TempTable.AcceptChanges()
      End If

    Else
    newRow("FIELD") = "Individuals Owning/Leasing Yachts, Not Aircraft"
    newRow("COUNT") = "125"
    newRow("TAB") = "3"
    newRow("TYPE_OF") = ""
    TempTable.Rows.Add(newRow)
    TempTable.AcceptChanges()

    newRow = TempTable.NewRow()
    newRow("FIELD") = "&nbsp;&nbsp;&nbsp;Previously Owning Aircraft"
    newRow("COUNT") = "325"
    newRow("TAB") = "3"
    newRow("TYPE_OF") = "P"
    TempTable.Rows.Add(newRow)
    TempTable.AcceptChanges()

    newRow = TempTable.NewRow()
    newRow("FIELD") = "Companies Owning/Leasing Yachts, Not Aircraft"
    newRow("COUNT") = "325"
    newRow("TAB") = "6"
    newRow("TYPE_OF") = ""
    TempTable.Rows.Add(newRow)
    TempTable.AcceptChanges()

    newRow = TempTable.NewRow()
    newRow("FIELD") = "&nbsp;&nbsp;&nbsp;Previously Owning Aircraft"
    newRow("COUNT") = "325"
    newRow("TAB") = "6"
    newRow("TYPE_OF") = "P"
    TempTable.Rows.Add(newRow)
    TempTable.AcceptChanges()

    newRow = TempTable.NewRow()
    newRow("FIELD") = "Recent Individuals Purchasing Yachts, Not Owning/Leasing Aircraft"
    newRow("COUNT") = "0"
    newRow("TAB") = "7"
    newRow("TYPE_OF") = ""
    TempTable.Rows.Add(newRow)
    TempTable.AcceptChanges()

    newRow = TempTable.NewRow()
    newRow("FIELD") = "Recent Companies Purchasing Yachts, Not Owning/Leasing Aircraft"
    newRow("COUNT") = "0"
    newRow("TAB") = "8"
    newRow("TYPE_OF") = ""
    TempTable.Rows.Add(newRow)
    TempTable.AcceptChanges()

    End If
  End Sub
  Private Sub displayYachtView20_mfr()
    Dim HoldTable_years As New DataTable
    Dim HoldTable As New DataTable
    Dim string_for_all_brands As String = ""
    Dim string_for_all_sizes As String = ""
    Dim temp_year_chart As String = ""
    Dim this_company_name As String = ""
    Dim temp_view_name As String = ""
    Dim news_link As String = ""
    Dim temp_news As String = ""
    Dim title_link1 As String = ""
    Dim title_link2 As String = ""
    Dim order_by As String = ""

    Try

      mfr_comp_id.Text = Trim(Request("comp_id"))

      If Trim(mfr_comp_id.Text.ToString.Trim) = "" Then
        mfr_comp_id.Text = "0"
      End If

      temp_view_name = "Yacht_View_Template.aspx?ViewID=" & Trim(Request("ViewID")) & "&ViewName=" & Trim(Request("ViewName"))


      If mfr_comp_id.Text.ToString.Trim = "" Or mfr_comp_id.Text.ToString.Trim = "0" Then

        order_by = Trim(Request("order_by"))
        HoldTable = localDatalayer.get_yacht_all_mfr(order_by)

        If Trim(order_by) = "compnameasc" Then
          title_link1 = "<A href='Yacht_View_Template.aspx?ViewID=20&ViewName=Shipyard/Manufacturer&order_by=compnamedesc'>Shipyard/Manufacturer</a>"
        Else
          title_link1 = "<A href='Yacht_View_Template.aspx?ViewID=20&ViewName=Shipyard/Manufacturer&order_by=compnameasc'>Shipyard/Manufacturer</a>"
        End If

        If Trim(order_by) = "countdesc" Then
          title_link2 = "<A href='Yacht_View_Template.aspx?ViewID=20&ViewName=Shipyard/Manufacturer&order_by=countasc'># of Yachts</a>"
        Else
          title_link2 = "<A href='Yacht_View_Template.aspx?ViewID=20&ViewName=Shipyard/Manufacturer&order_by=countdesc'># of Yachts</a>"
        End If

        MFR_LIST_LABEL.Text = localDatalayer.display_two_column_view20(HoldTable, title_link1, "comp_name", title_link2, "tcount", True, "comp_id", temp_view_name, 650, False)
        MFR_LIST.Height = 650

        HoldTable.Dispose()

        LATEST_NEWS_MFR.Visible = False
      Else
        MFR_LIST_LABEL.CssClass = "display_block tab_container_div_small"
        MFR_LIST_LABEL.Text = localDatalayer.get_company_name_by_id(mfr_comp_id.Text.ToString.Trim, this_company_name)
        MFR_LIST_LABEL.Text = "" ' previous is currently called just to get company name 
        crmWebClient.CompanyFunctions.Fill_Information_Tab(MFR_LIST_PANEL, MFR_LIST_LABEL, MasterPage, mfr_comp_id.Text.ToString.Trim, 0, "", dummy_label, MFR_LIST, dummy_label, dummy_label, False)

        MFR_LIST_LABEL.Text += "<br><br><a href='" & temp_view_name & "'>Clear Company</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
        MFR_LIST_LABEL.Text += "<a href='#' onclick=" & """" & "javascript:load('DisplayCompanyDetail.aspx?compid=" & mfr_comp_id.Text.ToString.Trim & "','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');return false;" & """" & ">View Company Details</a>"

        MFR_LIST.Height = 260


        HoldTable = localDatalayer.get_latest_mfr_news(mfr_comp_id.Text.ToString.Trim)

        If Not IsNothing(HoldTable) Then
          temp_news = "<div valign=""top"" style='height:350px; overflow: auto;'>"
          temp_news += "<table cellpadding='5' cellspacing='0' width='100%'>"
          If HoldTable.Rows.Count > 0 Then
            For Each Row As DataRow In HoldTable.Rows
              If Not IsDBNull(Row.Item("ytnews_web_address")) Then
                news_link = Row.Item("ytnews_web_address")
                If InStr(news_link, "http://") = 0 And Trim(news_link) <> "" Then
                  news_link = "http://" & news_link
                End If
              End If
              temp_news += "<tr><td><span class='li'>" & Row.Item("ytnews_date") & "-<A href='" & news_link & "' target='_blank'>" & Row.Item("ytnews_title") & "</a>:<br> " & Left(Row.Item("ytnews_description"), 300) & " ... <i><u>More At <A href='" & news_link & "' target='_blank'>" & Row.Item("ytnewssrc_name") & "</a> </u></i></span></td></tr>"
            Next
          End If
          temp_news += "</table></div>"

          LATEST_NEWS_MFR_LABEL.Text = temp_news
          LATEST_NEWS_MFR.Visible = True
        End If

        HoldTable.Dispose()
      End If


      HoldTable_years = localDatalayer.get_yacht_all_mfr_years(CLng(mfr_comp_id.Text.ToString.Trim))

      ' if there is no company, then do years
      If mfr_comp_id.Text.ToString.Trim = "" Or mfr_comp_id.Text.ToString.Trim = "0" Then

        If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT Then
          YEAR_LIST_LABEL.Text = localDatalayer.display_two_column_view20(HoldTable_years, "Production Trends", "yt_year_mfr", "# of Yachts", "tcount", True, "yt_year_mfr", True, 350, True)
        Else
          YEAR_LIST_LABEL.Text = localDatalayer.display_two_column_view20(HoldTable_years, "Production Trends", "yt_year_mfr", "# of Yachts", "tcount", True, "yt_year_mfr", "", 350, False)
        End If

        YEAR_LIST_PANEL.HeaderText = "Production Trends"

        ' if there is a company, add name to label
        MFR_LIST_PANEL.HeaderText = "Shipyard/Manufacturer"

        YEAR_CHART_PANEL.HeaderText = "Production Trends Chart"
      Else
        'if there is a company, show yachts
        HoldTable = localDatalayer.get_yacht_for_mfr(CLng(mfr_comp_id.Text.ToString.Trim))
        YEAR_LIST_PANEL.HeaderText = "Yachts"

        YEAR_CHART_PANEL.HeaderText = "Production Trends Chart"
        MFR_LIST_PANEL.HeaderText = "Shipyard/Manufacturer"
        LATEST_NEWS_MFR_PANEL.HeaderText = "Latest "

        If mfr_comp_id.Text.ToString.Trim <> "" And mfr_comp_id.Text.ToString.Trim <> "0" Then
          YEAR_LIST_PANEL.HeaderText += ": " & this_company_name
          YEAR_CHART_PANEL.HeaderText += ": " & this_company_name
          MFR_LIST_PANEL.HeaderText += ": " & this_company_name
          LATEST_NEWS_MFR_PANEL.HeaderText += " News: " & this_company_name & ""
        End If

        YEAR_LIST_LABEL.Text = localDatalayer.display_two_column_view20_yacht(HoldTable, YEAR_LIST_PANEL.HeaderText, "yt_yacht_name", "Year", "yt_year_mfr", False, "yt_year_mfr", True, 340, "yt_id")
      End If




      '--------- EITHER WAY DO THE TOP YEAR CHART ------------------------------------------------------------------------------
      YEAR_MRF_CHART.Series.Clear()
      YEAR_MRF_CHART.Series.Add("YEAR").ChartType = UI.DataVisualization.Charting.SeriesChartType.Spline
      YEAR_MRF_CHART.Series("YEAR").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32
      YEAR_MRF_CHART.Series("YEAR").LabelForeColor = Drawing.Color.Blue
      YEAR_MRF_CHART.ChartAreas("ChartArea1").AxisY.Title = "Year MFR Count"
      YEAR_MRF_CHART.Series("YEAR").Color = Drawing.Color.Blue
      YEAR_MRF_CHART.Series("YEAR").BorderWidth = 1
      YEAR_MRF_CHART.Series("YEAR").MarkerSize = 5
      YEAR_MRF_CHART.Series("YEAR").MarkerStyle = UI.DataVisualization.Charting.MarkerStyle.Circle
      YEAR_MRF_CHART.Series("YEAR").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32
      ' if there is no company, then do 5 years
      If mfr_comp_id.Text.ToString.Trim = "" Or mfr_comp_id.Text.ToString.Trim = "0" Then
        YEAR_MRF_CHART.ChartAreas("ChartArea1").AxisX.Interval = 5
      Else ' do 2 years
        YEAR_MRF_CHART.ChartAreas("ChartArea1").AxisX.Interval = 2
      End If
      YEAR_MRF_CHART.Width = 450
      YEAR_MRF_CHART.Height = 250


      If Not IsNothing(HoldTable_years) Then
        If HoldTable_years.Rows.Count > 0 Then
          For Each Row As DataRow In HoldTable_years.Rows
            YEAR_MRF_CHART.Series("YEAR").Points.AddXY(Row.Item("yt_year_mfr").ToString(), CDbl(Row.Item("tcount")))
          Next
        End If
      End If

      YEAR_MRF_CHART.Titles.Clear()
      YEAR_MRF_CHART.Titles.Add("Yacht MFR Year")
      YEAR_MRF_CHART.ImageType = DataVisualization.Charting.ChartImageType.Jpeg
      YEAR_MRF_CHART.SaveImage(Server.MapPath("TempFiles") & "\" & mfr_comp_id.Text.ToString.Trim & "_YEARS.jpg", DataVisualization.Charting.ChartImageFormat.Jpeg)


      YEAR_CHART_LABEL.Text = ("<img src='TempFiles/" & mfr_comp_id.Text.ToString.Trim & "_YEARS.jpg'>")
      '--------------------------------------------------------------------------------------------------------------------------



      HoldTable_years.Dispose()
      HoldTable_years = Nothing

      HoldTable.Dispose()
      HoldTable = Nothing




    Catch ex As Exception

    End Try
  End Sub
  ''' <summary>
  ''' Sub that sets up the Industry At a Glance View (#16)
  ''' This is the schematic for this view:
  ''' VIEW:
  ''' (TL) FLEET SUMMARY | (TC) MOTOR YACHTS | (TR) SAILING YACHTS
  ''' (BL) YACHTS BY BRAND | (BC) YACHTS BY YEAR
  ''' Needed: Top Left, Top Center, Top Right, Bottom Left, Bottom Center
  ''' Not Needed: Bottom Right
  ''' </summary>
  ''' <remarks></remarks>
  Private Sub displayYachtView16()
    Dim HoldTable As New DataTable
    Dim LookupTable As New DataTable
    Dim string_for_all_brands As String = ""
    Dim string_for_all_sizes As String = ""

    Try

      ' SECTION ADDED IN MSW - 2/10/14
      '-------------------------------------------------------
      ' string_for_all_brands = "M|M##S|M"

      HoldTable = localDatalayer.get_yacht_all_brands()

      If Not IsNothing(HoldTable) Then
        If HoldTable.Rows.Count > 0 Then
          For Each Row As DataRow In HoldTable.Rows
            string_for_all_brands = string_for_all_brands & Row.Item("ycs_category_size").ToString & "|" & Row.Item("ycs_motor_type").ToString & "##"
            string_for_all_sizes = string_for_all_sizes & Row.Item("ycs_category_size").ToString & "##"
          Next
          string_for_all_brands = Left(string_for_all_brands, Len(string_for_all_brands) - 2)
          string_for_all_sizes = Left(string_for_all_sizes, Len(string_for_all_sizes) - 2)
        End If
      End If
      '-------------------------------------------------------

      'Data Information For First Query
      HoldTable = localDatalayer.get_yacht_fleet_summary(localCriteria)

      'We need a second table for yacht lifecycle. This table has all of the statuses so the function down below can create the correct link for the search.
      LookupTable = MasterPage.aclsData_Temp.ListOfYachtStagesStatusCombined()

      'Set Up Top Left Container 
      top_left_label.Text = localDatalayer.display_two_column_view16_lifecycle(LookupTable, HoldTable, True)
      top_left_panel.HeaderText = "FLEET SUMMARY"

      'Data Information for Second Query
      HoldTable = New DataTable

      'Set Up Top Center Container

      If localCriteria.YachtViewCriteriaMotorType <> crmWebClient.Constants.VIEW_SAILHULL Then

        HoldTable = localDatalayer.get_yachts_by_type(localCriteria, "M")
        top_center_panel.HeaderText = "MOTOR YACHTS"
        top_center_label.Text = localDatalayer.display_summary_by_yacht_motorType(HoldTable)

      Else

        top_center_panel.HeaderText = "MOTOR YACHTS"
        top_center_label.Text = "<table width='100%' cellpadding='3' cellspacing='0' class='data_aircraft_grid'>"
        top_center_label.Text += "<tr class='header_row'>"
        top_center_label.Text += "<td align='left' valign='top'><b>No yachts of this type to display</b></td>"
        top_center_label.Text += "</tr></table>"

      End If

      'Data Information for Third Query
      HoldTable = New DataTable

      'Set Up Top Center Container

      If localCriteria.YachtViewCriteriaMotorType <> crmWebClient.Constants.VIEW_MOTORHULL Then

        HoldTable = localDatalayer.get_yachts_by_type(localCriteria, "S")
        top_right_panel.HeaderText = "SAILING YACHTS"
        top_right_label.Text = localDatalayer.display_summary_by_yacht_motorType(HoldTable)

      Else

        top_right_panel.HeaderText = "SAILING YACHTS"
        top_right_label.Text = "<table width='100%' cellpadding='3' cellspacing='0' class='data_aircraft_grid'>"
        top_right_label.Text += "<tr class='header_row'>"
        top_right_label.Text += "<td align='left' valign='top'><b>No yachts of this type to display</b></td>"
        top_right_label.Text += "</tr></table>"

      End If

      'Data Information for Fourth Query
      HoldTable = New DataTable
      HoldTable = localDatalayer.get_yacht_brand_summary(localCriteria)

      'Set Up Bottom Left Container 
      bottom_left_label.Text = localDatalayer.display_two_column_view16_brand(HoldTable, "Brand", "ym_brand_name", "# of Yachts", "tcount", False, "ym_brand_name", string_for_all_brands, string_for_all_sizes)
      bottom_left_panel.HeaderText = "YACHTS BY BRAND"

      'Data Information for Fifth Query
      HoldTable = New DataTable
      HoldTable = localDatalayer.get_yacht_year_summary(localCriteria)

      'Set Up Bottom Right Container
      bottom_right_label.Text = localDatalayer.display_two_column_view16(HoldTable, "Year", "yt_year_mfr", "# of Yachts", "tcount", False, "yt_year_mfr", False, "operator_year_mfr", "Equals")
      bottom_right_panel.HeaderText = "YACHTS BY YEAR"

      SetUpViewDisplay("ThreeTwo")

      HoldTable.Dispose()
      HoldTable = Nothing

    Catch ex As Exception

    End Try

  End Sub

  ''' <summary>
  ''' This function sorts a table using a dataview. All you need to do is pass the datatable to be sorted and the
  ''' field you'd like it sorted by. It sends the table back, sorted.
  ''' </summary>
  ''' <param name="DataIn"></param>
  ''' <param name="dataSort"></param>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Private Function SortedTable(ByVal DataIn As DataTable, ByVal dataSort As String) As DataTable
    If Not String.IsNullOrEmpty(dataSort) Then
      Dim tableView As New DataView

      tableView = DataIn.DefaultView
      tableView.Sort = dataSort

      DataIn = tableView.ToTable()
    End If
    Return DataIn
  End Function

  Private Sub displayYachtView17()
    sort_dropdown.Visible = True
    sort_by_dropdown.Visible = True
    sortByText.Visible = True
    Dim HoldTable As New DataTable
    Dim tableView As New DataView
    Try

      'Data Information For First Query
      HoldTable = localDatalayer.get_yacht_naval_architects_info(localCriteria)

      'Added a sort.
      HoldTable = SortedTable(HoldTable, PageSort)

      navalArchitectsTabPanel.HeaderText = "NAVAL ARCHITECTS"
      If String.IsNullOrEmpty(localDatalayer.class_error) Then
        navalArchitectsLabel.Text = localDatalayer.display_two_column_view17(HoldTable, "Company Name", "comp_name", "# of Yachts", "tcount", True)
      Else
        navalArchitectsLabel.Text = localDatalayer.class_error
      End If

      'Data Information for Second Query
      HoldTable = New DataTable
      HoldTable = localDatalayer.get_yacht_interior_designers_info(localCriteria)

      'Added a sort.
      HoldTable = SortedTable(HoldTable, PageSort)

      interiorDesignersTabPanel.HeaderText = "INTERIOR DESIGNERS"
      If String.IsNullOrEmpty(localDatalayer.class_error) Then
        interiorDesignersLabel.Text = localDatalayer.display_two_column_view17(HoldTable, "Company Name", "comp_name", "# of Yachts", "tcount", True)
      Else
        interiorDesignersLabel.Text = localDatalayer.class_error
      End If

      'Data Information for Third Query
      HoldTable = New DataTable
      HoldTable = localDatalayer.get_yacht_exterior_designers_info(localCriteria)

      'Added a sort.
      HoldTable = SortedTable(HoldTable, PageSort)

      exteriorDesignersTabPanel.HeaderText = "EXTERIOR DESIGNERS"
      If String.IsNullOrEmpty(localDatalayer.class_error) Then
        exteriorDesignersLabel.Text = localDatalayer.display_two_column_view17(HoldTable, "Company Name", "comp_name", "# of Yachts", "tcount", True)
      Else
        exteriorDesignersLabel.Text = localDatalayer.class_error
      End If

      HoldTable.Dispose()
      HoldTable = Nothing

    Catch ex As Exception

    End Try

  End Sub

  Private Sub SetUpViewDisplay(ByVal TypeOfView As String)
    Select Case TypeOfView
      Case "ThreeTwo"

        bottom_left_container.Width = New Unit("48%")
        bottom_center_container.Visible = False
        bottom_right_container.Width = New Unit("48%")

    End Select
  End Sub

  ''' <summary>
  ''' Toggles visibility of action menu.
  ''' </summary>
  ''' <remarks></remarks>
  Private Sub ToggleVisibilityOfActionMenu(ByRef ParentBulletedList As BulletedList, ByRef SubMenuBulletedList As BulletedList)
    'actions dropdown
    ParentBulletedList.Attributes.Add("onmouseover", "javascript:ShowBar('" & SubMenuBulletedList.ClientID & "', true);")
    ParentBulletedList.Attributes.Add("onmouseout", "javascript:ShowBar('" & SubMenuBulletedList.ClientID & "', false);")

    SubMenuBulletedList.Attributes.Add("onmouseover", "javascript:ShowBar('" & SubMenuBulletedList.ClientID & "', true);")
    SubMenuBulletedList.Attributes.Add("onmouseout", "javascript:ShowBar('" & SubMenuBulletedList.ClientID & "', false);")

    ParentBulletedList.Visible = True

    If View_ID = 17 Then
      'Only initialize sort dropdown on view 17, architect view
      'Sort dropdown.
      sort_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & sort_submenu_dropdown.ClientID & "', true);")
      sort_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & sort_submenu_dropdown.ClientID & "', false);")

      sort_submenu_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & sort_submenu_dropdown.ClientID & "', true);")
      sort_submenu_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & sort_submenu_dropdown.ClientID & "', false);")

      'Sort By dropdown asc/desc:
      sort_by_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & sort_by_dropdown_submenu.ClientID & "', true);")
      sort_by_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & sort_by_dropdown_submenu.ClientID & "', false);")

      sort_by_dropdown_submenu.Attributes.Add("onmouseover", "javascript:ShowBar('" & sort_by_dropdown_submenu.ClientID & "', true);")
      sort_by_dropdown_submenu.Attributes.Add("onmouseout", "javascript:ShowBar('" & sort_by_dropdown_submenu.ClientID & "', false);")
    ElseIf View_ID = 23 Then
      central_agent_select_start.Attributes.Add("onmouseover", "javascript:ShowBar('" & central_agent_select.ClientID & "', true);")
      central_agent_select_start.Attributes.Add("onmouseout", "javascript:ShowBar('" & central_agent_select.ClientID & "', false);")

      central_agent_select.Attributes.Add("onmouseover", "javascript:ShowBar('" & central_agent_select.ClientID & "', true);")
      central_agent_select.Attributes.Add("onmouseout", "javascript:ShowBar('" & central_agent_select.ClientID & "', false);")
    End If

    'This is a small example of removing a list item:
    'SubMenuBulletedList.Items.RemoveAt(0)
    'actions_submenu_dropdown.Items.Clear() - This removes all

    'This is a small example of adding a list item:
    'SubMenuBulletedList.Items.Add(New ListItem("Item for view #" & View_ID, "javascript:alert('You clicked on this folder.');"))
  End Sub

  ''' <summary>
  ''' Click part of the dropdown list, switch the submenu bullet with the main bullet.
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  ''' <remarks></remarks>
  Public Sub submenu_dropdown_Click(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.BulletedListEventArgs)
    Try
      Dim selectedLI As New ListItem
      Dim SortBy As String = ""
      Dim SortField As String = ""
      selectedLI = sender.Items(e.Index) 'This is the item they clicked.
      'selectedLI.Text This is the text of the item they clicked

      selectedLI = sender.Items(e.Index)
      If sender.id.ToString = "sort_submenu_dropdown" Then
        'Get the sort by variables.
        SortBy = sort_by_dropdown.Items.Item(0).Text
        SortField = selectedLI.Text
        'combine them together to be the page sort.
        PageSort = SortField & " " & SortBy
        'clearing the bulleted list so the item appears selected.
        sort_dropdown.Items.Clear()
        sort_dropdown.Items.Add(New ListItem(SortField, ""))
        'setting the actual field name to be the page sort.
        sort_by_dropdown.Items.Clear()
        sort_by_dropdown.Items.Add(New ListItem(SortBy, ""))
        'setting the actual field name to be the page sort.
        SetPageSort(PageSort)
        'redisplaying the page, with newly sorted variable.
        displayYachtView17()
      ElseIf sender.id.ToString = "sort_by_dropdown_submenu" Then
        'Get the sort by variables.
        SortBy = selectedLI.Text
        SortField = sort_dropdown.Items.Item(0).Text
        'combine them together to be the page sort.
        PageSort = SortField & " " & SortBy
        'clearing the bulleted list so the item appears selected.
        sort_dropdown.Items.Clear()
        sort_dropdown.Items.Add(New ListItem(SortField, ""))
        'clearing the sort by list so item appears selected.
        sort_by_dropdown.Items.Clear()
        sort_by_dropdown.Items.Add(New ListItem(SortBy, ""))
        'setting the actual field name to be the page sort.
        SetPageSort(PageSort)
        'redisplaying the page, with newly sorted variable.
        displayYachtView17()
      End If
    Catch ex As Exception

    End Try
  End Sub

  ''' <summary>
  ''' A small function that takes the wordage of the selected item and changes it to an actual field name.
  ''' </summary>
  ''' <param name="selectedLI"></param>
  ''' <remarks></remarks>
  Public Sub SetPageSort(Optional ByVal selectedLI As String = "")
    Select Case selectedLI
      Case "Company ASC"
        PageSort = "comp_name asc"
      Case "Company DESC"
        PageSort = "comp_name desc"
      Case "# of Yachts ASC"
        PageSort = "tcount asc"
      Case Else
        PageSort = "tcount desc"
    End Select
  End Sub

  Public Sub load_page_variables()

    Dim sMotor_type As String = ""
    Dim sCategory As String = ""
    Dim sBrand As String = ""
    Dim sModel As String = ""

    ' load category/brand/model boxes
    If Not String.IsNullOrEmpty(Session.Item("viewYachtModel").ToString.Trim) Then

      Dim modelArray() As String = Nothing
      Dim tmpModelArray() As String = Nothing

      ' Check and see if user selected more than one model
      If Session.Item("viewYachtModel").ToString.Contains(Constants.cCommaDelim) Then

        modelArray = Session.Item("viewYachtModel").ToString.Split(Constants.cCommaDelim)

        If IsArray(modelArray) And Not IsNothing(modelArray) Then

          ReDim tmpModelArray(UBound(modelArray))

          For x As Integer = 0 To UBound(modelArray)
            ' translate index into actual ymod_id
            tmpModelArray(x) = commonEvo.ReturnYachtModelIDForItemIndex(CLng(modelArray(x)))
            If commonEvo.ReturnYachtModelDataFromIndex(CLng(modelArray(x)), sMotor_type, sCategory, sBrand, sModel) Then
              If String.IsNullOrEmpty(localCriteria.YachtViewCriteriaYachtModel.Trim) Then
                localCriteria.YachtViewCriteriaYachtModel = sModel
              Else
                localCriteria.YachtViewCriteriaYachtModel += Constants.cCommaDelim + sModel
              End If
            End If
          Next

          localCriteria.YachtViewCriteriaYmodIDArray = tmpModelArray

        End If

      Else

        ' translate index into actual ymod_id
        localCriteria.YachtViewCriteriaYmodID = commonEvo.ReturnYachtModelIDForItemIndex(CLng(Session.Item("viewYachtModel").ToString))

        If commonEvo.ReturnYachtModelDataFromIndex(CLng(Session.Item("viewYachtModel").ToString), sMotor_type, sCategory, sBrand, sModel) Then
          localCriteria.YachtViewCriteriaYachtModel = sModel
          localCriteria.YachtViewCriteriaYachtCategory = sCategory
        End If

      End If

    Else

      localCriteria.YachtViewCriteriaYmodIDArray = Nothing
      localCriteria.YachtViewCriteriaYmodID = -1
      localCriteria.YachtViewCriteriaYachtModel = ""
      Session.Item("viewYachtModel") = ""

    End If

    If Not String.IsNullOrEmpty(Session.Item("viewYachtBrand").ToString.Trim) Then

      Dim makeArray() As String = Nothing
      Dim tmpMakeArray() As String = Nothing

      ' Check and see if user selected more than one brand
      If Session.Item("viewYachtBrand").ToString.Contains(Constants.cCommaDelim) Then

        makeArray = Session.Item("viewYachtBrand").ToString.Split(Constants.cCommaDelim)

        If IsArray(makeArray) And Not IsNothing(makeArray) Then

          ReDim tmpMakeArray(UBound(makeArray))

          For x As Integer = 0 To UBound(makeArray)
            ' translate index into actual ymod_brand_id
            tmpMakeArray(x) = commonEvo.ReturnYachtModelIDForItemIndex(CLng(makeArray(x)))
            If commonEvo.ReturnYachtModelDataFromIndex(CLng(makeArray(x)), sMotor_type, sCategory, sBrand, sModel) Then
              If String.IsNullOrEmpty(localCriteria.YachtViewCriteriaYachtBrand.Trim) Then
                localCriteria.YachtViewCriteriaYachtBrand = sBrand
              Else
                localCriteria.YachtViewCriteriaYachtBrand += Constants.cCommaDelim + sBrand
              End If
            End If
          Next

          localCriteria.YachtViewCriteriaBrandIDArray = tmpMakeArray

        End If

      Else
        ' translate index into actual ymod_brand_id
        localCriteria.YachtViewCriteriaBrandID = commonEvo.ReturnYachtModelIDForItemIndex(CLng(Session.Item("viewYachtBrand")))

        If commonEvo.ReturnYachtModelDataFromIndex(CLng(Session.Item("viewYachtBrand").ToString), sMotor_type, sCategory, sBrand, sModel) Then
          localCriteria.YachtViewCriteriaYachtBrand = sBrand
          localCriteria.YachtViewCriteriaYachtCategory = sCategory

        End If

      End If

    Else

      localCriteria.YachtViewCriteriaBrandIDArray = Nothing
      localCriteria.YachtViewCriteriaBrandID = -1
      localCriteria.YachtViewCriteriaYachtBrand = ""
      localCriteria.YachtViewCriteriaMotorType = Constants.VIEW_ALLHULLTYPES
      localCriteria.YachtViewCriteriaYachtCategory = sCategory
      Session.Item("viewYachtBrand") = ""

    End If

    If Not String.IsNullOrEmpty(Session.Item("viewYachtCategory").ToString.Trim) And (View_ID = 16 Or View_ID = 22 Or View_ID = 17) Then

      ' translate index into actual amod_type_code
      localCriteria.YachtViewCriteriaCategoryID = commonEvo.ReturnYachtModelIDForItemIndex(CLng(Session.Item("viewYachtCategory").ToString))

      If commonEvo.ReturnYachtModelDataFromIndex(CLng(Session.Item("viewYachtCategory").ToString), sMotor_type, sCategory, sBrand, sModel) Then
        localCriteria.YachtViewCriteriaYachtCategory = sCategory

        Select Case (sMotor_type)
          Case Constants.YMOD_MOTOR_HULL
            localCriteria.YachtViewCriteriaMotorType = Constants.VIEW_MOTORHULL
          Case Constants.YMOD_SAIL_HULL
            localCriteria.YachtViewCriteriaMotorType = Constants.VIEW_SAILHULL
        End Select

      End If
    Else

      localCriteria.YachtViewCriteriaYachtMotor = ""
      localCriteria.YachtViewCriteriaMotorType = Constants.VIEW_ALLHULLTYPES
      localCriteria.YachtViewCriteriaCategoryID = -1
      localCriteria.YachtViewCriteriaYachtCategory = ""
      Session.Item("viewYachtCategory") = ""

    End If

    ' if amod_id has data that overides category/brand/model selections
    If Not IsNothing(Request.Item("ymod_id")) Then
      If Not String.IsNullOrEmpty(Request.Item("ymod_id").ToString) Then

        If CLng(Request.Item("ymod_id").ToString.Trim) = -1 Then

          localCriteria.YachtViewCriteriaYmodID = -1
          localCriteria.YachtViewCriteriaYachtModel = ""
          Session.Item("viewYachtModel") = ""

          localCriteria.YachtViewCriteriaBrandID = -1
          localCriteria.YachtViewCriteriaYachtBrand = ""
          Session.Item("viewYachtBrand") = ""

          localCriteria.YachtViewCriteriaYachtMotor = ""
          localCriteria.YachtViewCriteriaMotorType = Constants.VIEW_ALLHULLTYPES

          localCriteria.YachtViewCriteriaCategoryID = -1
          localCriteria.YachtViewCriteriaYachtCategory = ""
          Session.Item("viewYachtCategory") = ""

        Else

          localCriteria.YachtViewCriteriaYmodID = CLng(Request.Item("ymod_id").ToString.Trim)
          Session.Item("viewYachtModel") = commonEvo.FindYachtIndexForItemByModelID(localCriteria.YachtViewCriteriaYmodID)
          Session.Item("viewYachtBrand") = Session.Item("viewYachtModel")
          Session.Item("viewYachtCategory") = Session.Item("viewYachtModel")

          If commonEvo.ReturnYachtModelDataFromIndex(CLng(Session.Item("viewYachtModel").ToString), sMotor_type, sCategory, sBrand, sModel) Then
            localCriteria.YachtViewCriteriaYachtMotor = sMotor_type
            localCriteria.YachtViewCriteriaYachtCategory = sCategory
            localCriteria.YachtViewCriteriaYachtModel = sModel

          End If
        End If

      End If
    End If

    ' this sets a default yacht model to start off with *** commented out for now
    'If localCriteria.YachtViewCriteriaYmodID = -1 And localCriteria.YachtViewCriteriaBrandID = -1 Then

    '  localCriteria.YachtViewCriteriaYmodID = 32

    '  Session.Item("viewYachtModel") = commonEvo.FindYachtIndexForItemByModelID(localCriteria.YachtViewCriteriaYmodID)
    '  Session.Item("viewYachtBrand") = Session.Item("viewYachtModel")
    '  Session.Item("viewYachtCategory") = Session.Item("viewYachtModel")

    '  If commonEvo.ReturnYachtModelDataFromIndex(CLng(Session.Item("viewYachtModel").ToString), sMotor_type, sCategory, sBrand, sModel) Then
    '    localCriteria.YachtViewCriteriaYachtMotor = sMotor_type
    '    localCriteria.YachtViewCriteriaYachtCategory = sCategory
    '    localCriteria.YachtViewCriteriaYachtModel = sModel
    '  End If

    'Else

    If localCriteria.YachtViewCriteriaYmodID > -1 Then
      Session.Item("viewYachtModel") = commonEvo.FindYachtIndexForItemByModelID(localCriteria.YachtViewCriteriaYmodID)
      Session.Item("viewYachtBrand") = Session.Item("viewYachtModel")
      Session.Item("viewYachtCategory") = Session.Item("viewYachtModel")
      If commonEvo.ReturnYachtModelDataFromIndex(CLng(Session.Item("viewYachtModel").ToString), sMotor_type, sCategory, sBrand, sModel) Then
        localCriteria.YachtViewCriteriaYachtMotor = sMotor_type
        localCriteria.YachtViewCriteriaYachtCategory = sCategory
        localCriteria.YachtViewCriteriaYachtModel = sModel

        Select Case (sMotor_type)
          Case Constants.YMOD_MOTOR_HULL
            localCriteria.YachtViewCriteriaMotorType = Constants.VIEW_MOTORHULL
          Case Constants.YMOD_SAIL_HULL
            localCriteria.YachtViewCriteriaMotorType = Constants.VIEW_SAILHULL
        End Select

      End If
    ElseIf localCriteria.YachtViewCriteriaBrandID > -1 Then
      Session.Item("viewYachtBrand") = commonEvo.FindYachtIndexForItemByModelID(localCriteria.YachtViewCriteriaBrandID)
      Session.Item("viewYachtCategory") = Session.Item("viewYachtBrand").ToString
      If commonEvo.ReturnYachtModelDataFromIndex(CLng(Session.Item("viewYachtBrand").ToString), sMotor_type, sCategory, sBrand, sModel) Then
        localCriteria.YachtViewCriteriaYachtMotor = sMotor_type
        localCriteria.YachtViewCriteriaYachtCategory = sCategory
        localCriteria.YachtViewCriteriaYachtBrand = sBrand
        localCriteria.YachtViewCriteriaYmodID = -1

        Select Case (sMotor_type)
          Case Constants.YMOD_MOTOR_HULL
            localCriteria.YachtViewCriteriaMotorType = Constants.VIEW_MOTORHULL
          Case Constants.YMOD_SAIL_HULL
            localCriteria.YachtViewCriteriaMotorType = Constants.VIEW_SAILHULL
        End Select

      End If
    End If

    'End If

    commonEvo.fillYachtArray("")
    commonEvo.fillYachtCategoryLableArray("")

    Select Case View_ID
      Case 25 ' notes view

        If Not bIsReport Then

          If Not IsNothing(notesSearch_for_txt) Then
            If Not String.IsNullOrEmpty(notesSearch_for_txt.Text.Trim) Then
              localCriteria.YachtViewCriteriaNoteTextValue = notesSearch_for_txt.Text.Trim
            Else
              localCriteria.YachtViewCriteriaNoteTextValue = ""
            End If
          Else
            localCriteria.YachtViewCriteriaNoteTextValue = ""
          End If

          If Not IsNothing(notesSearch_who) And notesSearch_who.Items.Count > 0 Then

            If Not IsNothing(notesSearch_who.SelectedValue) And Not String.IsNullOrEmpty(notesSearch_who.SelectedValue.Trim) Then
              Dim tmpSelect As Long = 0

              If IsNumeric(notesSearch_who.SelectedValue) Then
                tmpSelect = CLng(notesSearch_who.SelectedValue)
                bUseLoggedInUser = False ' once selection is made use "selected item"
              End If

              If tmpSelect = -1 Then
                localCriteria.YachtViewCriteriaNoteUserID = 0
              Else
                localCriteria.YachtViewCriteriaNoteUserID = tmpSelect
              End If

            End If

          End If

          If Not IsNothing(notesSearch_yt_search_field.SelectedValue) And Not String.IsNullOrEmpty(notesSearch_yt_search_field.SelectedValue.Trim) Then
            localCriteria.YachtViewCriteriaNoteYTSearchField = CInt(notesSearch_yt_search_field.SelectedValue)
          Else
            localCriteria.YachtViewCriteriaNoteYTSearchField = 0
            notesSearch_yt_search_field.SelectedValue = ""
          End If

          If Not IsNothing(notesSearch_yt_search_field_text) Then
            If Not String.IsNullOrEmpty(notesSearch_yt_search_field_text.Text.Trim) Then
              localCriteria.YachtViewCriteriaNoteYTSearchTextValue = notesSearch_yt_search_field_text.Text.Trim
            Else
              localCriteria.YachtViewCriteriaNoteYTSearchTextValue = ""
            End If
          Else
            localCriteria.YachtViewCriteriaNoteYTSearchTextValue = ""
          End If

          If Not IsNothing(notesSearch_yt_search_field_operator.SelectedValue) And Not String.IsNullOrEmpty(notesSearch_yt_search_field_operator.SelectedValue.Trim) Then
            localCriteria.YachtViewCriteriaNoteYTSearchOperator = CInt(notesSearch_yt_search_field_operator.SelectedValue)
          Else
            localCriteria.YachtViewCriteriaNoteYTSearchOperator = 0
            notesSearch_yt_search_field_operator.SelectedValue = ""
          End If

          If Not String.IsNullOrEmpty(notesSearch_date.Text.Trim) Then
            If notesSearch_date.Text.Contains(Constants.cColonDelim) Then

              Dim datesArray As Array = Split(notesSearch_date.Text, Constants.cColonDelim)

              localCriteria.YachtViewCriteriaNoteStartDate = datesArray(0).ToString.Trim
              localCriteria.YachtViewCriteriaNoteEndDate = datesArray(1).ToString.Trim

            Else
              localCriteria.YachtViewCriteriaNoteStartDate = notesSearch_date.Text.Trim
            End If

          Else
            notesSearch_date.Text = ""
            localCriteria.YachtViewCriteriaNoteStartDate = ""
            localCriteria.YachtViewCriteriaNoteEndDate = ""
          End If

          If Not IsNothing(notesSearch_order_by.SelectedValue) And Not String.IsNullOrEmpty(notesSearch_order_by.SelectedValue.Trim) Then
            localCriteria.YachtViewCriteriaNoteOrderBy = notesSearch_order_by.SelectedValue.Trim
          Else
            localCriteria.YachtViewCriteriaNoteOrderBy = ""
            notesSearch_order_by.SelectedValue = "date"
          End If

          HttpContext.Current.Session.Item("yachtViewCriteria") = localCriteria ' save selections if user wants report

        Else
          localCriteria = HttpContext.Current.Session.Item("yachtViewCriteria") ' use saved selections for report
          localCriteria.YachtViewCriteriaIsReport = bIsReport ' set report flag on criteria
          bUseLoggedInUser = False ' and use the "user from the selection criteria"
        End If

      Case Else

    End Select

  End Sub

  Private Sub load_yacht_view_session_variables()

    ' because these values are needed on this page they need to match the control names in the control
    ' so the request header pickes up the right values
    If Not IsNothing(Request.Item("cbo" + sYachtCategoryModelCtrlBaseName + "Category")) Then
      If Not String.IsNullOrEmpty(Request.Item("cbo" + sYachtCategoryModelCtrlBaseName + "Category").ToString) Then
        If Not Request.Item("cbo" + sYachtCategoryModelCtrlBaseName + "Category").ToString.ToLower.Contains("all") Then
          Session.Item("viewYachtCategory") = Request.Item("cbo" + sYachtCategoryModelCtrlBaseName + "Category").ToString.Trim
        Else
          Session.Item("viewYachtModel") = ""
          Session.Item("viewYachtBrand") = ""
          Session.Item("viewYachtCategory") = ""
        End If
      End If
    End If

    If Not IsNothing(Request.Item("cbo" + sYachtCategoryModelCtrlBaseName + "Brand")) Then
      If Not String.IsNullOrEmpty(Request.Item("cbo" + sYachtCategoryModelCtrlBaseName + "Brand").ToString) Then
        If Not Request.Item("cbo" + sYachtCategoryModelCtrlBaseName + "Brand").ToString.ToLower.Contains("all") Then
          Session.Item("viewYachtBrand") = Request.Item("cbo" + sYachtCategoryModelCtrlBaseName + "Brand").ToString.Trim
        Else
          Session.Item("viewYachtModel") = ""
          Session.Item("viewYachtBrand") = ""
        End If
      End If
    End If

    If Not IsNothing(Request.Item("cbo" + sYachtCategoryModelCtrlBaseName + "Model")) Then
      If Not String.IsNullOrEmpty(Request.Item("cbo" + sYachtCategoryModelCtrlBaseName + "Model").ToString) Then
        If Not Request.Item("cbo" + sYachtCategoryModelCtrlBaseName + "Model").ToString.ToLower.Contains("all") Then
          Session.Item("viewYachtModel") = Request.Item("cbo" + sYachtCategoryModelCtrlBaseName + "Model").ToString.Trim
        Else
          Session.Item("viewYachtModel") = ""
        End If
      End If
    End If

    ' pick up the "company location values"
    If Not IsNothing(Request.Item("radViewContinentRegion")) Then
      If Not String.IsNullOrEmpty(Request.Item("radViewContinentRegion").ToString) And (Request.Item("radViewContinentRegion").ToString.ToLower <> Session.Item("viewRegionOrContinent").ToString.ToLower) Then
        Session.Item("viewRegionOrContinent") = Request.Item("radViewContinentRegion").ToString.ToLower
        Session.Item("viewRegion") = ""
        Session.Item("viewCountry") = ""
        Session.Item("viewState") = ""
        Session.Item("viewTimeZone") = ""
      End If
    End If

    If Not IsNothing(Request.Item("cboViewRegion")) Then
      If Not String.IsNullOrEmpty(Request.Item("cboViewRegion")) And Not Request.Item("cboViewRegion").ToString.ToLower.Contains("all") Then
        Session.Item("viewRegion") = Request.Item("cboViewRegion").ToString.Trim
      End If
    End If

    If Not String.IsNullOrEmpty(Session.Item("viewRegion").ToString.Trim) Then
      If Session.Item("viewRegion").ToString.ToLower.Contains("clear") Then
        Session.Item("viewRegion") = ""
        Session.Item("viewCountry") = ""
        Session.Item("viewState") = ""
        Session.Item("viewTimeZone") = ""
      End If
    End If

    If Not IsNothing(Request.Item("cboViewCountry")) Then
      If Not String.IsNullOrEmpty(Request.Item("cboViewCountry")) And Not Request.Item("cboViewCountry").ToString.ToLower.Contains("all") Then
        Session.Item("viewCountry") = Request.Item("cboViewCountry").ToString.Trim
      End If
    End If

    If Not String.IsNullOrEmpty(Session.Item("viewCountry").ToString.Trim) Then
      If Session.Item("viewCountry").ToString.ToLower.Contains("clear") Then
        Session.Item("viewCountry") = ""
        Session.Item("viewState") = ""
        Session.Item("viewTimeZone") = ""
      End If
    End If

    If Not IsNothing(Request.Item("cboViewState")) Then
      If Not String.IsNullOrEmpty(Request.Item("cboViewState")) And Not Request.Item("cboViewState").ToString.ToLower.Contains("all") Then
        Session.Item("viewState") = Request.Item("cboViewState").ToString.Trim
      End If
    End If

    If Not String.IsNullOrEmpty(Session.Item("viewState").ToString.Trim) Then
      If Session.Item("viewState").ToString.ToLower.Contains("clear") Then
        Session.Item("viewState") = ""
        Session.Item("viewTimeZone") = ""
      End If
    End If

    If Not IsNothing(Request.Item("cboViewTimeZone")) Then
      If Not String.IsNullOrEmpty(Request.Item("cboViewTimeZone")) And Not Request.Item("cboViewTimeZone").ToString.ToLower.Contains("all") Then
        Session.Item("viewTimeZone") = Request.Item("cboViewTimeZone").ToString.Trim
      End If
    End If

    If Not String.IsNullOrEmpty(Session.Item("viewTimeZone").ToString.Trim) Then
      If Session.Item("viewTimeZone").ToString.ToLower.Contains("clear") Then
        Session.Item("viewState") = ""
      End If
    End If

    If Not IsNothing(Request.Item("ViewName")) Then
      If Not String.IsNullOrEmpty(Request.Item("ViewName").ToString) Then
        View_Name = Request.Item("ViewName").ToString.Trim
      End If
    End If

    If Not IsNothing(Request.Item("ViewID")) Then
      If Not String.IsNullOrEmpty(Request.Item("ViewID").ToString) Then
        View_ID = CInt(Request.Item("ViewID").ToString.Trim)
      End If
    End If

    If Not IsNothing(Request.Item("clear")) Then
      If Not String.IsNullOrEmpty(Request.Item("clear").ToString) Then
        bClearView = CBool(Request.Item("clear").ToString.Trim)
      End If
    End If

    If Not IsNothing(Request.Item("bIsReport")) Then
      If Not String.IsNullOrEmpty(Request.Item("bIsReport").ToString) Then
        bIsReport = IIf(Request.Item("bIsReport").ToString.Trim.Contains("Y"), True, False)
      End If
    End If

    If bClearView Then

      HttpContext.Current.Session.Item("viewYachtModel") = ""
      HttpContext.Current.Session.Item("viewYachtBrand") = ""
      HttpContext.Current.Session.Item("viewYachtCategory") = ""

      HttpContext.Current.Session.Item("viewRegionOrContinent") = "Continent"
      HttpContext.Current.Session.Item("viewRegion") = ""
      HttpContext.Current.Session.Item("viewCountry") = ""
      HttpContext.Current.Session.Item("viewState") = ""
      HttpContext.Current.Session.Item("viewTimeZone") = ""

      localCriteria = Nothing
      localCriteria = New yachtViewSelectionCriteria

      HttpContext.Current.Session.Item("yachtViewCriteria") = Nothing

    End If

  End Sub

  Public Sub displayyachtview23_central()
    Dim HoldTable As New DataTable
    Dim temp_comp As String = ""
    Dim toggleRowColor As Boolean = True
    Dim add_comma As Boolean = False
    Dim has_location As Boolean = False
    Dim temp_country As String = ""
    Dim country_string As String = ""
    Dim big_comp_id_list As String = ""
    Dim main_comp_id As Long = 0
    Dim temp_count As Integer = 0
    Dim google_map_string As String = ""
    Dim charting_string As String = ""
    Dim total_count As Integer = 0
    Dim order_by As String = ""
    Dim comp_link As String = ""
    Dim temp_comp_sub As String = ""
    Dim temp_brand As String = ""
    Dim main_location_name As String = ""
    Dim additional_graph_title As String = ""
    Dim type_of As String = ""
    Dim end_link As String = ""

    ControlImage1.CssClass = "display_none"

    If Trim(Request("main_comp_id")) <> "" Then
      main_comp_id = CInt(Trim(Request("main_comp_id")))
      end_link &= "&main_comp_id=" & main_comp_id
    End If



    If Trim(Request("brand_name")) <> "" Then
      temp_brand = Trim(Request("brand_name"))
      end_link &= "&brand_name=" & temp_brand
      temp_brand = Replace(temp_brand, "~", "&")
      additional_graph_title &= " - " & temp_brand
    End If


    If Trim(Request("country")) <> "" Then
      temp_country = Trim(Request("country"))
      end_link &= "&country=" & temp_country
      country_string = " - " & temp_country
      additional_graph_title &= country_string
    End If


    If Trim(Request("order_by")) <> "" Then
      order_by = Trim(Request("order_by"))
      end_link &= "&order_by=" & order_by
    Else
      order_by = "count"
    End If


    If Trim(Request("type_of")) <> "" Then
      type_of = Trim(Request("type_of"))
    Else
      type_of = ""
    End If


    Me.central_agent_select_start.Visible = True
    Me.central_agent_select_start.Items.Clear()

    If Trim(type_of) = "" Then
      Me.central_agent_select_start.Items.Add(New ListItem("All Central Agents", "/Yacht_View_Template.aspx?ViewID=23&ViewName=Yacht Central Agents"))
    ElseIf Trim(type_of) = "FS" Then
      Me.central_agent_select_start.Items.Add(New ListItem("All Central Agents For Sale", "/Yacht_View_Template.aspx?ViewID=23&ViewName=Yacht Central Agents&type_of=FS"))
    ElseIf Trim(type_of) = "FC" Then
      Me.central_agent_select_start.Items.Add(New ListItem("All Central Agents For Charter", "/Yacht_View_Template.aspx?ViewID=23&ViewName=Yacht Central Agents&type_of=FC"))
    End If 

 
    Me.central_agent_select.Items.Clear()
    Me.central_agent_select.Items.Add(New ListItem("Show All Central Agents", "/Yacht_View_Template.aspx?ViewID=23&ViewName=Yacht Central Agents" & end_link))
    Me.central_agent_select.Items.Add(New ListItem("Show All Central Agents For Sale", "/Yacht_View_Template.aspx?ViewID=23&ViewName=Yacht Central Agents&type_of=FS" & end_link))
    Me.central_agent_select.Items.Add(New ListItem("Show All Central Agents For Charter", "/Yacht_View_Template.aspx?ViewID=23&ViewName=Yacht Central Agents&type_of=FC" & end_link))

    Me.sortByText.Visible = True
    Me.sortByText.Text = "View By:"


    Me.view_23_tab3.HeaderText = "YACHT SUMMARY BY SIZE" & country_string


    '---------------------------------------- CENTRAL AGENTS---------------------------------------------------------------------
    HoldTable = localDatalayer.get_yacht_all_central_agents(temp_country, order_by, main_comp_id, temp_brand, type_of)
 
    If Not IsNothing(HoldTable) Then

      If main_comp_id > 0 Or Trim(temp_country) <> "" Then
        temp_comp = "<div valign=""top"" style='height:200px; overflow: auto;'>"
      Else
        temp_comp = "<div valign=""top"" style='height:500px; overflow: auto;'>"
      End If


      temp_comp += "<table width='100%' cellpadding='3' cellspacing='0' class='data_aircraft_grid'>"
      temp_comp += "<tr class='header_row'>"
      If main_comp_id > 0 Then
        temp_comp += "<td align='left' valign='top'><b>Locations</b>"
        temp_comp += "&nbsp;&nbsp;&nbsp;(<a href='Yacht_View_Template.aspx?ViewID=23&brand_name=" & temp_brand & "&country=&ViewName=Yacht Central Agentss&type_of=" & type_of & "'>Clear Agent</a>)"
        temp_comp += "</a>" 
        temp_comp += "</td><td align='right'>Yachts&nbsp;</td>"
      Else
        temp_comp += "<td align='left' valign='top'><a href='Yacht_View_Template.aspx?ViewID=23&country=" & Trim(temp_country) & "&order_by=name&ViewName=Yacht Central Agentss&type_of=" & type_of & "'><b>Company</b></a></td><td align='right'><a href='Yacht_View_Template.aspx?ViewID=23&country=" & Trim(temp_country) & "&order_by=count&ViewName=Yacht Central Agentss&type_of=" & type_of & "'>Yachts</a>&nbsp;</td>"
      End If

      temp_comp += "</tr>"

      ' view_23_tab1.HeaderText = "Cental Agents (" & HoldTable.Rows.Count & ") "

      If HoldTable.Rows.Count > 0 Then
        For Each Row As DataRow In HoldTable.Rows

          temp_comp_sub = ""

          HttpContext.Current.Session.Item("MasterCompanyFrom") = "COMPANY"
          If Trim(HttpContext.Current.Session.Item("MasterCompanyWhere")) <> "" Then
            HttpContext.Current.Session.Item("MasterCompanyWhere") &= ", " & Row.Item("broker_main_comp_id")
          Else
            HttpContext.Current.Session.Item("MasterCompanyWhere") &= Row.Item("broker_main_comp_id")
          End If

          has_location = False

          'if we have picked a main location, show the main location first 
          If temp_count = 0 And main_comp_id > 0 Then

            '  If Not toggleRowColor Then
            '    temp_comp += "<tr class='alt_row'>"
            '    toggleRowColor = True
            '  Else
            '    temp_comp += "<tr bgcolor='white'>"
            '    toggleRowColor = False
            '  End If
            '  temp_comp += "<td>"
            '  temp_comp += DisplayFunctions.WriteDetailsLink(0, Row.Item("broker_main_comp_id"), 0, 0, True, Row.Item("comp_name").ToString, "", "")
            '  temp_comp += "</td>"

            '  temp_comp += "<td align='right'>"
            '  temp_comp += "<a href='Yacht_View_Template.aspx?ViewID=23&main_comp_id=" & Row.Item("broker_main_comp_id") & "&ViewName=Yacht Central Agents'>"
            '  temp_comp += Row.Item("tcount").ToString

            '  total_count = total_count + Row.Item("tcount")
            '  temp_comp += "</a>&nbsp;</td>"
            '  temp_comp += "</tr>"
            Me.view_23_tab1.HeaderText = Row.Item("comp_name").ToString
            main_location_name = Row.Item("comp_name").ToString
          ElseIf Trim(temp_country) <> "" Then ' then its now the yacht list
            Me.view_23_tab1.HeaderText = "CENTRAL AGENT LOCATIONS PER COUNTRY " & country_string
          ElseIf temp_count = 0 And main_comp_id = 0 Then
            Me.view_23_tab1.HeaderText = "CENTRAL AGENTS FOR YACHTS " & country_string
          End If

          'dont need to double show 
          If main_comp_id > 0 Or Trim(temp_country) <> "" Then
            'If (CInt(Row.Item("comp_id")) <> CInt(main_comp_id)) Then

            If Not toggleRowColor Then
              temp_comp_sub += "<tr class='alt_row'>"
              toggleRowColor = True
            Else
              temp_comp_sub += "<tr bgcolor='white'>"
              toggleRowColor = False
            End If

            temp_comp_sub += "<td>"
            comp_link = DisplayFunctions.WriteDetailsLink(0, Row.Item("comp_id"), 0, 0, True, Row.Item("lower_comp_name").ToString, "", "")
            'comp_link = Replace(comp_link, "href='#'", "href='#' onclick=""ChangeTheMouseCursorOnItemParentDocument('cursor_wait')""") 
            temp_comp_sub += comp_link
            temp_comp_sub += " ("

            add_comma = False
            If Not IsDBNull(Row.Item("comp_city")) Then
              If Trim(Row.Item("comp_city")) <> "" Then
                temp_comp_sub += "" & Row.Item("comp_city")
                add_comma = True
              End If
            End If

            If Not IsDBNull(Row.Item("comp_state")) Then
              If Trim(Row.Item("comp_state")) <> "" Then
                If add_comma = True Then
                  temp_comp_sub += ","
                End If
                temp_comp_sub += Row.Item("comp_state")
                add_comma = True
              End If
            End If

            If Not IsDBNull(Row.Item("comp_country")) Then
              If Trim(Row.Item("comp_country")) <> "" Then
                If add_comma = True Then
                  temp_comp_sub += ","
                End If
                temp_comp_sub += Row.Item("comp_country")
              End If
            End If

            temp_comp_sub += ")</td><td align='right'>" & Row.Item("tcount").ToString & "&nbsp;</td></tr>"
            total_count = total_count + Row.Item("tcount")

            'End If

          Else

            If Not toggleRowColor Then
              temp_comp_sub += "<tr class='alt_row'>"
              toggleRowColor = True
            Else
              temp_comp_sub += "<tr bgcolor='white'>"
              toggleRowColor = False
            End If
            temp_comp_sub += "<td><a href='Yacht_View_Template.aspx?ViewID=23&main_comp_id=" & Row.Item("broker_main_comp_id") & "&brand_name=" & temp_brand & "&ViewName=Yacht Central Agents&type_of=" & type_of & "' onclick=""ChangeTheMouseCursorOnItemParentDocument('cursor_wait')"">"
            temp_comp_sub += Row.Item("comp_name").ToString
            temp_comp_sub += "</a>&nbsp;("

            temp_comp_sub += Row.Item("sub_count").ToString

            If Row.Item("sub_count") > 1 Then
              temp_comp_sub += "&nbsp;Locations"
            Else
              temp_comp_sub += "&nbsp;Location"
            End If

            ' temp_comp += DisplayFunctions.WriteDetailsLink(0, Row.Item("broker_main_comp_id"), 0, 0, True, Row.Item("comp_name").ToString, "", "")
            temp_comp_sub += ")&nbsp;</td>"

            temp_comp_sub += "<td align='right'><a href='Yacht_View_Template.aspx?ViewID=23&main_comp_id=" & Row.Item("broker_main_comp_id") & "&brand_name=" & temp_brand & "&ViewName=Yacht Central Agents&type_of=" & type_of & "' onclick=""ChangeTheMouseCursorOnItemParentDocument('cursor_wait')"">"
            temp_comp_sub += Row.Item("tcount").ToString & "</a>&nbsp;</td></tr>"
          End If
 

          temp_comp = temp_comp & temp_comp_sub
          temp_comp_sub = ""

          temp_count = temp_count + 1
        Next




      Else
        temp_comp += "<tr bgcolor='white'><td>No Companies Found</td></tr>"
      End If
      temp_comp += "</table></div>"

      If Trim(temp_country) <> "" Then
        temp_comp = Replace(temp_comp, "200px", "350px")
        Me.view_23_central_bottom.Text = temp_comp
        Me.view_23_left_bottom.Visible = True
        Me.view_23_tab_bottom_left.HeaderText = "AGENT LOCATIONS IN " & country_string
      ElseIf Trim(temp_brand) <> "" And main_comp_id = 0 Then
        temp_comp = Replace(temp_comp, "500px", "400px")
        Me.view_23_central_bottom.Text = temp_comp
        Me.view_23_left_bottom.Visible = True
        Me.view_23_tab_bottom_left.HeaderText = "CENTRAL AGENTS WITH YACHTS FROM - " & Trim(temp_brand) & " "
      Else
        view_23_central.Text = temp_comp
        view_23_central.Visible = True
      End If

      If Trim(main_location_name) <> "" Then
        additional_graph_title &= " - " & main_location_name
      End If

    End If
    '---------------------------------------- CENTRAL AGENTS---------------------------------------------------------------------












    '---------------------------------------- COUNTRIES ---------------------------------------------------------------------

    HoldTable = localDatalayer.get_yacht_all_central_agents_by_section(temp_country, Trim(order_by), "comp_country", main_comp_id, temp_brand, "N", type_of)

    If Not IsNothing(HoldTable) Then
      If Trim(temp_country) <> "" Then
        temp_comp = "<div valign=""top"" style='height:90px; overflow: auto;'>"
      Else
        temp_comp = "<div valign=""top"" style='height:230px; overflow: auto;'>"
      End If

      temp_comp += "<table width='100%' cellpadding='3' cellspacing='0' class='data_aircraft_grid'><tr class='header_row'>"


      If Trim(temp_country) <> "" Then
        temp_comp += "<td align='left' valign='top'><b>Country</b>"
        temp_comp += "&nbsp;&nbsp;&nbsp;(<a href='Yacht_View_Template.aspx?ViewID=23&brand_name=" & temp_brand & "&country=&ViewName=Yacht Central Agents&type_of=" & type_of & "'>Clear Country</a>)"
        temp_comp += "</td><td align='right'>Agent Locations&nbsp;</td>"
      Else
        temp_comp += "<td align='left' valign='top'><a href='Yacht_View_Template.aspx?ViewID=23&country=" & Trim(temp_country) & "&brand_name=" & temp_brand & "&order_by=name&ViewName=Yacht Central Agents&type_of=" & type_of & "'><b>Country</b></a>"
        temp_comp += "</td><td align='right'><a href='Yacht_View_Template.aspx?ViewID=23&country=" & Trim(temp_country) & "&brand_name=" & temp_brand & "&order_by=count&ViewName=Yacht Central Agents&type_of=" & type_of & "'>Agent Locations</a>&nbsp;</td>"
      End If

      temp_comp += "</tr>"

      If Trim(temp_brand) <> "" Then
        Me.view_23_tab2.HeaderText = "CENTRAL AGENT LOCATIONS PER COUNTRY - " & Trim(temp_brand)
      Else
        Me.view_23_tab2.HeaderText = "CENTRAL AGENT LOCATIONS PER COUNTRY" & country_string
      End If
 

      ' view_21_tab1.HeaderText = "Countries (" & HoldTable.Rows.Count & ") "

      If HoldTable.Rows.Count > 0 Then

        google_map_string = " ['Country Name', 'Total Count']"
        For Each Row As DataRow In HoldTable.Rows

          has_location = False
          temp_comp_sub = ""

          If Not toggleRowColor Then
            temp_comp_sub += "<tr class='alt_row'>"
            toggleRowColor = True
          Else
            temp_comp_sub += "<tr bgcolor='white'>"
            toggleRowColor = False
          End If
          temp_comp_sub += "<td>"
          If Trim(temp_country) <> "" Then
            temp_comp_sub += "" & Row.Item("comp_country") & ""
          Else
            temp_comp_sub += "<a href='Yacht_View_Template.aspx?ViewID=23&country=" & Row.Item("comp_country") & "&brand_name=" & temp_brand & "&ViewName=Yacht Central Agents&type_of=" & type_of & "' onclick=""ChangeTheMouseCursorOnItemParentDocument('cursor_wait')"">" & Row.Item("comp_country") & "</a>"
          End If

          temp_comp_sub += "</td><td align='right'>"

          If Trim(temp_country) <> "" Then
            temp_comp_sub += Row.Item("tcount").ToString
          Else
            temp_comp_sub += "<a href='Yacht_View_Template.aspx?ViewID=23&country=" & Row.Item("comp_country") & "&brand_name=" & temp_brand & "&ViewName=Yacht Central Agents&type_of=" & type_of & "' onclick=""ChangeTheMouseCursorOnItemParentDocument('cursor_wait')"">" & Row.Item("tcount").ToString & "</a>"
          End If

          temp_comp_sub += "</td></tr>"

          google_map_string &= ", ['" & Replace(Replace(Row.Item("comp_country"), "'", ""), "&", "+") & "', " & Row.Item("tcount") & "]"

          temp_comp = temp_comp & temp_comp_sub
          temp_comp_sub = ""
        Next

        ' temp_comp = Replace(temp_comp, "X123", "&ViewName=Yacht Central Agents' onclick=""ChangeTheMouseCursorOnItemParentDocument('cursor_wait')"">")

      Else
        temp_comp += "<tr bgcolor='white'><td>No Companies Found</td></tr>"
      End If

      temp_comp += "</table></div>"

      If Trim(temp_country) <> "" Then
        Me.view_23_central.Text = temp_comp   ' put it to the top left 
        Me.view_23_tab2.Visible = False   ' maybe change to 
        Me.view_23_central.Visible = True
      ElseIf Trim(temp_brand) <> "" Then 
        Me.view_23_tab3.HeaderText = "CENTRAL AGENT LOCATIONS PER COUNTRY - " & Trim(temp_brand)
        temp_comp = Replace(temp_comp, "230px", "245px")
        view_23_central3.Text = temp_comp
        view_23_central3.Visible = True
        Me.view_23_right_bottom.Visible = True
      Else
        view_23_central2.Text = temp_comp
        view_23_central2.Visible = True
      End If
    End If

    If main_comp_id > 0 Or (Trim(temp_brand) <> "" And main_comp_id = 0 And Trim(temp_country) = "") Then
      Me.central_title_label1.Text = "<table><tr valign='top'><td width='100%' align='center'>AGENT LOCATIONS BY COUNTRY " & additional_graph_title & "</td><td align='center'>&nbsp;</td></tr></table>"

      DisplayFunctions.load_google_chart(graph_panel, google_map_string, "", "Model Count", "div_central_top_all", 480, 225, "ARRAY", 1, charting_string, Me.Page, Me.graph_update_panel, False, False, False, False, False, False, False, False, False, True, 1)
      Me.view_23_central2.Visible = False
      Me.view_23_right.Visible = False

      If (Trim(temp_brand) <> "" And main_comp_id = 0 And Trim(temp_country) = "") Then
        '--------------------- BUILD CHART-----------------------
        Call load_google_chart_all(view_21_tab1, charting_string)
        '--------------------- BUILD CHART-----------------------
      End If
    End If

    '---------------------------------------- COUNTRIES ---------------------------------------------------------------------







    '---------------------------------------- BRAND NAMES---------------------------------------------------------------------
    If (main_comp_id = 0 And Trim(temp_country) = "") Or Trim(temp_brand) <> "" Then
      HoldTable = localDatalayer.get_yacht_all_central_agents_by_section(temp_country, Trim(order_by), "ym_brand_name", main_comp_id, temp_brand, "N", type_of)

      If Not IsNothing(HoldTable) Then
        If Trim(temp_country) <> "" Then
          temp_comp = "<div valign=""top"" style='height:90px; overflow: auto;'>"
        ElseIf Trim(temp_brand) <> "" Then
          temp_comp = "<div valign=""top"" style='height:52px; overflow: auto;'>"
        Else
          temp_comp = "<div valign=""top"" style='height:230px; overflow: auto;'>"
        End If

        temp_comp += "<table width='100%' cellpadding='3' cellspacing='0' class='data_aircraft_grid'><tr class='header_row'>"

        If Trim(temp_brand) <> "" And Trim(country_string) <> "" Then
          Me.view_23_tab1.HeaderText = "YACHT SUMMARY - " & Trim(temp_brand) & " in " & country_string
        ElseIf Trim(temp_brand) <> "" And Trim(main_location_name) <> "" Then
          Me.view_23_tab1.HeaderText = "YACHT SUMMARY - " & main_location_name & " - " & Trim(temp_brand)
        ElseIf Trim(temp_brand) <> "" Then
          Me.view_23_tab1.HeaderText = "YACHT SUMMARY - " & Trim(temp_brand)
        Else
          Me.view_23_tab3.HeaderText = "YACHT SUMMARY BY BRAND NAME " & country_string
        End If


        If Trim(temp_brand) <> "" Then
          temp_comp += "<td align='left' valign='top'><b>Brand</b>"
          temp_comp += "&nbsp;&nbsp;&nbsp;(<a href='Yacht_View_Template.aspx?ViewID=23&country=" & Trim(temp_country) & "&main_comp_id=" & main_comp_id & "&ViewName=Yacht Central Agents&type_of=" & type_of & "'>Clear Brand</a>)"
          If Trim(main_location_name) <> "" Or Trim(country_string) <> "" Then
            temp_comp += "</td><td align='right'>Agent Locations&nbsp;</td>"
          Else
            temp_comp += "</td><td align='right'>Central Agents&nbsp;</td>"
          End If

        Else
          temp_comp += "<td align='left' valign='top'><a href='Yacht_View_Template.aspx?ViewID=23&country=" & Trim(temp_country) & "&main_comp_id=" & main_comp_id & "&order_by=name&ViewName=Yacht Central Agents&type_of=" & type_of & "'><b>Brand</b></a>"
          temp_comp += "</td><td align='right'><a href='Yacht_View_Template.aspx?ViewID=23&country=" & Trim(temp_country) & "&main_comp_id=" & main_comp_id & "&order_by=count&ViewName=Yacht Central Agents&type_of=" & type_of & "'>Central Agents</a>&nbsp;</td>"
        End If


        temp_comp += "</tr>"

        If HoldTable.Rows.Count > 0 Then

          google_map_string = " ['Country Name', 'Total Count']"
          For Each Row As DataRow In HoldTable.Rows

            has_location = False
            temp_comp_sub = ""

            If Not toggleRowColor Then
              temp_comp_sub += "<tr class='alt_row'>"
              toggleRowColor = True
            Else
              temp_comp_sub += "<tr bgcolor='white'>"
              toggleRowColor = False
            End If
            temp_comp_sub += "<td>"

            If Trim(temp_brand) <> "" Then
              temp_comp_sub += Row.Item("ym_brand_name")
            Else
              temp_comp_sub += "<a href='Yacht_View_Template.aspx?ViewID=23&brand_name=" & Replace(Row.Item("ym_brand_name"), "&", "~") & "&main_comp_id=" & main_comp_id & "&ViewName=Yacht Central Agents&type_of=" & type_of & "' onclick=""ChangeTheMouseCursorOnItemParentDocument('cursor_wait')"">" & Row.Item("ym_brand_name") & "</a>"
            End If


            temp_comp_sub += "</td><td align='right'>"

            If Trim(temp_brand) <> "" Then
              temp_comp_sub += Row.Item("tcount").ToString
            Else
              temp_comp_sub += "<a href='Yacht_View_Template.aspx?ViewID=23&brand_name=" & Replace(Row.Item("ym_brand_name"), "&", "~") & "&main_comp_id=" & main_comp_id & "&ViewName=Yacht Central Agents&type_of=" & type_of & "' onclick=""ChangeTheMouseCursorOnItemParentDocument('cursor_wait')"">" & Row.Item("tcount").ToString & "</a>"
            End If

            temp_comp_sub += "</td></tr>"

            ' google_map_string &= ", ['" & Replace(Replace(Row.Item("ym_brand_name"), "'", ""), "&", "+") & "', " & Row.Item("tcount") & "]"

            temp_comp = temp_comp & temp_comp_sub
            temp_comp_sub = ""
          Next

          ' temp_comp = Replace(temp_comp, "X123", "&ViewName=Yacht Central Agents' onclick=""ChangeTheMouseCursorOnItemParentDocument('cursor_wait')"">")

        Else
          temp_comp += "<tr bgcolor='white'><td>No Companies Found</td></tr>"
        End If

        temp_comp += "</table></div>"


        If Trim(temp_brand) <> "" Then
          If Trim(temp_country) <> "" Then
            Me.view_23_central.Text = temp_comp & Me.view_23_central.Text    ' put it to the top left 
            Me.view_23_central.Visible = True   ' maybe change to 
          ElseIf main_comp_id > 0 Then
            Me.view_23_central.Text = temp_comp & Me.view_23_central.Text  ' put it to the top left 
            Me.view_23_central.Visible = True   ' maybe change to 
          Else ' just brand click 
            Me.view_23_central.Text = temp_comp   ' put it to the top left 
            Me.view_23_central.Visible = True   ' maybe change to 
          End If
        Else
          Me.view_23_central3.Text = temp_comp   ' put it to the top left 
          Me.view_23_right_bottom.Visible = True   ' maybe change to  
          Me.view_23_tab3.Visible = True
        End If



      End If
    End If

    '---------------------------------------- BRAND NAMES---------------------------------------------------------------------







    '---------------------------------------- YACHT CATEGORY SIZE -----------------------------------------------------------
    If main_comp_id > 0 Or Trim(temp_country) <> "" Then
      HoldTable = localDatalayer.get_yacht_all_central_agents_by_section(temp_country, Trim(order_by), "ycs_description", main_comp_id, temp_brand, "N", type_of)

      If Not IsNothing(HoldTable) Then

        If Trim(temp_country) <> "" Then
          temp_comp = "<div valign=""top"" style='height:710px; overflow: auto;'>"
        Else
          temp_comp = "<div valign=""top"" style='height:400px; overflow: auto;'>"
        End If

        temp_comp += "<table width='100%' cellpadding='3' cellspacing='0' class='data_aircraft_grid'>"
        temp_comp += "<tr class='header_row'>"
        temp_comp += "<td align='left' valign='top'><b>YACHTS BY CATEGORY SIZE</b></td><td align='right'>Count&nbsp;</td>"
        temp_comp += "</tr>"


        If HoldTable.Rows.Count > 0 Then
          google_map_string = " ['Yacht Category Size', 'Total Count']"

          For Each Row As DataRow In HoldTable.Rows

            has_location = False

            If Not toggleRowColor Then
              temp_comp += "<tr class='alt_row'>"
              toggleRowColor = True
            Else
              temp_comp += "<tr bgcolor='white'>"
              toggleRowColor = False
            End If
            temp_comp += "<td>"
            If Not IsDBNull(Row.Item("ycs_description")) Then
              If Trim(Row.Item("ycs_description")) <> "" Then
                temp_comp += "" & Row.Item("ycs_description")
                add_comma = True
              End If
            End If

            temp_comp += "</td><td align='right'>" & Row.Item("tcount").ToString & "</td></tr>"

            google_map_string &= ", ['" & Replace(Replace(Row.Item("ycs_description"), "'", ""), " & ", " and ") & "', " & Row.Item("tcount") & "]"
          Next

        Else
          temp_comp += "<tr bgcolor='white'><td>No Companies Found</td></tr>"
        End If
        temp_comp += "</table></div>"

        view_23_central3.Text = temp_comp
        view_23_central3.Visible = True
        Me.view_23_tab3.Visible = True
      End If
    End If

    If main_comp_id > 0 Or Trim(temp_country) <> "" Then
      Me.central_title_label2.Text = "<table><tr valign='top'><td width='100%' align='center'>YACHTS BY CATEGORY SIZE " & additional_graph_title & "</td><td align='center'>&nbsp;</td></tr></table>"


      DisplayFunctions.load_google_chart(graph_panel, google_map_string, "", "", "div_central_bottom_all", 480, 225, "ARRAY", 2, charting_string, Me.Page, Me.graph_update_panel, False, False, False, False, False, False, False, False, False, True, 1)

      '----------------------- GET BRAND--------------------------------------------------
      HoldTable = localDatalayer.get_yacht_all_central_agents_by_section(temp_country, Trim(order_by), "ym_brand_name", main_comp_id, temp_brand, "Y", type_of)
      google_map_string = ""
      If Not IsNothing(HoldTable) Then

        If HoldTable.Rows.Count > 0 Then
          google_map_string = " ['Yachts By Brand', 'Total Count']"
          For Each Row As DataRow In HoldTable.Rows

            google_map_string &= ", ['" & Replace(Replace(Row.Item("ym_brand_name"), "'", ""), " & ", " and ") & "', " & Row.Item("tcount") & "]"
          Next
          temp_comp += "<tr bgcolor='white'><td>No Companies Found</td></tr>"
        End If
        temp_comp += "</table></div>"
      End If

      Me.central_title_label3.Text = "<table><tr valign='top'><td width='100%' align='center'>YACHTS BY BRAND " & additional_graph_title & "</td><td align='center'>&nbsp;</td></tr></table>"

      DisplayFunctions.load_google_chart(graph_panel, google_map_string, "", "", "div_central_third_all", 480, 225, "ARRAY", 2, charting_string, Me.Page, Me.graph_update_panel, False, False, False, False, False, False, False, False, False, True, 1)

      Me.view_23_right_bottom.Visible = False
      '--------------------- BUILD CHART-----------------------
      Call load_google_chart_all(view_21_tab1, charting_string)
      '--------------------- BUILD CHART-----------------------











      ''----------------------------------------- FIND YACHTS---------------------------------------------------------------
      If main_comp_id > 0 Then


        Me.view_23_tab_bottom_left.HeaderText = "LIST OF YACHTS " & country_string


        ' turn on the yachts tab
        Me.view_23_left_bottom.Visible = True
        HoldTable = localDatalayer.get_yacht_all_central_agents_by_section(temp_country, Trim(order_by), "ym_brand_name, ym_model_name, yt_yacht_name, yt_hull_mfr_nbr, yt_id", main_comp_id, temp_brand, "N", type_of)
        google_map_string = ""
        If Not IsNothing(HoldTable) Then
          If HoldTable.Rows.Count > 0 Then

            temp_comp = "<div valign=""top"" style='height:300px; overflow: auto;'>"

            temp_comp += "<table width='100%' cellpadding='3' cellspacing='0' class='data_aircraft_grid'>"
            temp_comp += "<tr class='header_row'>"
            temp_comp += "<td align='left' valign='top'><b>YACHTS</b></td>"
            temp_comp += "</tr>"


            For Each Row As DataRow In HoldTable.Rows

              temp_comp_sub = ""

              If Not toggleRowColor Then
                temp_comp_sub += "<tr class='alt_row'><td align='left'>"
                toggleRowColor = True
              Else
                temp_comp_sub += "<tr bgcolor='white'><td align='left'>"
                toggleRowColor = False
              End If

              If Not IsDBNull(Row.Item("ym_brand_name")) Then
                If Trim(Row.Item("ym_brand_name")) <> "" Then
                  temp_comp_sub += Row.Item("ym_brand_name")
                End If
              End If

              If Not IsDBNull(Row.Item("ym_model_name")) Then
                If Trim(Row.Item("ym_model_name")) <> "" Then
                  temp_comp_sub += " " & Row.Item("ym_model_name")
                End If
              End If

              temp_comp_sub += "&nbsp;<a href='#' onclick=""javascript:load('DisplayYachtDetail.aspx?yid=" & Row.Item("yt_id") & "&jid=0','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');return false;"">"
              If Not IsDBNull(Row.Item("yt_yacht_name")) Then
                If Trim(Row.Item("yt_yacht_name")) <> "" Then
                  temp_comp_sub += Row.Item("yt_yacht_name")
                End If
              End If

              temp_comp_sub += "</a>"

              If Not IsDBNull(Row.Item("yt_hull_mfr_nbr")) Then
                If Trim(Row.Item("yt_hull_mfr_nbr")) <> "" Then
                  temp_comp_sub += " Hull#: " & Row.Item("yt_hull_mfr_nbr")
                End If
              End If

              temp_comp_sub += "</td></tr>"
              temp_comp = temp_comp & temp_comp_sub
              temp_comp_sub = ""

            Next
          Else
            temp_comp += "<tr bgcolor='white'><td>No Yachts Found</td></tr>"
          End If
          temp_comp += "</table></div>"

          Me.view_23_central_bottom.Text = temp_comp
        End If
      End If

    End If
    ''----------------------------------------- FIND YACHTS---------------------------------------------------------------------



    'HoldTable = localDatalayer.get_yacht_all_central_agents_top_25(temp_country)


    'If Not IsNothing(HoldTable) Then

    '  If Trim(temp_country) <> "" Then
    '    temp_comp = "<div valign=""top"" style='height:710px; overflow: auto;'>"
    '  Else
    '    temp_comp = "<div valign=""top"" style='height:400px; overflow: auto;'>"
    '  End If

    '  temp_comp += "<table width='100%' cellpadding='3' cellspacing='0' class='data_aircraft_grid'>"
    '  temp_comp += "<tr class='header_row'>"
    '  temp_comp += "<td align='left' valign='top'><b>TOP 25 CENTRAL AGENTS FOR YACHTS</b></td><td align='right'>Count&nbsp;</td>"
    '  temp_comp += "</tr>"

    '  ' view_21_tab1.HeaderText = "TOP 25 CENTRAL AGENTS FOR YACHTS "

    '  If HoldTable.Rows.Count > 0 Then
    '    For Each Row As DataRow In HoldTable.Rows

    '      has_location = False

    '      If Not toggleRowColor Then
    '        temp_comp += "<tr class='alt_row'>"
    '        toggleRowColor = True
    '      Else
    '        temp_comp += "<tr bgcolor='white'>"
    '        toggleRowColor = False
    '      End If
    '      temp_comp += "<td>"
    '      temp_comp += DisplayFunctions.WriteDetailsLink(0, Row.Item("Comp_Id"), 0, 0, True, Row.Item("comp_name").ToString, "", "")
    '      temp_comp += " ("

    '      add_comma = False
    '      If Not IsDBNull(Row.Item("comp_city")) Then
    '        If Trim(Row.Item("comp_city")) <> "" Then
    '          temp_comp += "" & Row.Item("comp_city")
    '          add_comma = True
    '        End If
    '      End If

    '      If Not IsDBNull(Row.Item("comp_state")) Then
    '        If Trim(Row.Item("comp_state")) <> "" Then
    '          If add_comma = True Then
    '            temp_comp += ","
    '          End If
    '          temp_comp += Row.Item("comp_state")
    '          add_comma = True
    '        End If
    '      End If

    '      If Not IsDBNull(Row.Item("comp_country")) Then
    '        If Trim(Row.Item("comp_country")) <> "" Then
    '          If add_comma = True Then
    '            temp_comp += ","
    '          End If
    '          temp_comp += Row.Item("comp_country")
    '        End If
    '      End If

    '      temp_comp += ")"
    '      temp_comp += "</td>"

    '      temp_comp += "<td align='right'>"
    '      temp_comp += Row.Item("tcount").ToString
    '      temp_comp += "</td>"


    '      temp_comp += "</tr>"


    '    Next

    '  Else
    '    temp_comp += "<tr bgcolor='white'><td>No Companies Found</td></tr>"
    '  End If
    '  temp_comp += "</table></div>"

    '  view_23_central3.Text = temp_comp
    '   view_23_central3.Visible = True
    ' End If

  End Sub

  Public Function check_if_can_export() As Boolean
    check_if_can_export = False

    If Session.Item("localUser").crmAllowExport_Flag = False Then
      check_if_can_export = False
      '  ElseIf Session.Item("localUser").crmDemoUserFlag = True Then
      '      check_if_can_export = False
      '  ElseIf Session.Item("localSubscription").crmMarketingFlag = True Then
      '     check_if_can_export = False
    Else
      check_if_can_export = True
    End If

    If check_if_can_export = False Then
      ' Me.warning1.Text = "<p><font color='red'>You are currently on a Demo Account and are not able to use this capability.</font></p>"
      ' Me.export_now_btn.Visible = False
    End If

  End Function

  Private Sub Build_Notes_tab(ByRef searchCriteria As yachtViewSelectionCriteria, ByRef out_htmlString As String)

    Dim htmlOut As New StringBuilder

    Dim sHtmlNotesList As String = ""
    Dim sHtmlSummaryList As String = ""

    Dim notes_functions As New notes_view_functions

    Dim bAdminFlag As Boolean = HttpContext.Current.Session.Item("localPreferences").UserAdminFlag
    Dim bHasStandardCloudNotes As Boolean = HttpContext.Current.Session.Item("localPreferences").HasCloudNotes
    Dim bHasServerNotes As Boolean = HttpContext.Current.Session.Item("localPreferences").HasServerNotes

    Dim sNotesReportFileName As String = ""
    Dim sNotesReportString As String = ""
    Dim sReportTitle As String = ""

    Dim subscriptionInfo As String = Session.Item("localUser").crmSubSubID.ToString + "_" + Session.Item("localUser").crmUserLogin.ToString.Trim + "_" + Session.Item("localUser").crmSubSeqNo.ToString + "_"

    Dim reportDisplayFolder As String = HttpContext.Current.Application.Item("crmClientSiteData").ClientFullHostName + HttpContext.Current.Session.Item("MarketSummaryFolderVirtualPath")

    Try

      notes_functions.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      notes_functions.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
      notes_functions.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
      notes_functions.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
      notes_functions.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim

      htmlOut.Append("<table id=""notesViewOuterTable"" width=""100%"" height=""70%"" cellpadding=""4"" cellspacing=""0"" class=""module"">")
      htmlOut.Append("<tr><td align=""left"" valign=""top"">")

      If searchCriteria.YachtViewCriteriaIsReport Then

        If bHasServerNotes Then 'If they're plus + notes users. 
          sReportTitle = subscriptionInfo + "notesReport_cloudPlus"
          sReportFrom = "cloudPlus"
        ElseIf bHasStandardCloudNotes Then 'if they're standard cloud users     
          sReportTitle = subscriptionInfo + "notesReport_standardCloud"
          sReportFrom = "standardCloud"
        End If

        sNotesReportFileName = commonEvo.GenerateFileName(sReportTitle, ".xls", False)

        notes_functions.display_notes_view_listTable(True, bAdminFlag, Nothing, sNotesReportString, searchCriteria)

        If notes_functions.write_notesReport_string_to_file(sNotesReportString, sNotesReportFileName) Then
          sReportOutputFilename = reportDisplayFolder + "/" + sNotesReportFileName.Trim
        Else
          HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in sending string to file"
        End If

        searchCriteria.YachtViewCriteriaIsReport = False ' set report flag to "false" so "listing shows"

        notes_functions.display_notes_view_listTable(True, bAdminFlag, Nothing, sHtmlNotesList, searchCriteria)
        htmlOut.Append(sHtmlNotesList)

        searchCriteria.YachtViewCriteriaIsReport = True

      Else

        notes_functions.display_notes_view_listTable(True, bAdminFlag, Nothing, sHtmlNotesList, searchCriteria)
        htmlOut.Append(sHtmlNotesList)

      End If

      'If bAdminFlag Then

      '  htmlOut.Append("</td></tr><tr><td align=""left"" valign=""top"" width=""100%"" height=""30%"">")

      '  notes_functions.display_notes_view_summaryTable(searchCriteria, sHtmlSummaryList)
      '  htmlOut.Append(sHtmlSummaryList)

      'End If

      htmlOut.Append("</td></tr></table>") ' outer view table

    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "** Error in [Yacht_View_Master.ascx.vb :  [Build_Notes_tab] : " + ex.Message
    End Try

    out_htmlString = htmlOut.ToString()
    htmlOut = Nothing
    notes_functions = Nothing

  End Sub

  Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

    If (localCriteria.YachtViewID = 25) Then
      Dim notesScript As String = "$(document).ready(function(){$(""#" + notesSearch_date.ClientID.Trim + """).daterangepicker();});"

      If localCriteria.YachtViewCriteriaIsReport Then
        If Not String.IsNullOrEmpty(sReportOutputFilename.Trim) Then
          notesScript += vbCrLf + "openNotesWindowJS(""" + sReportOutputFilename.Trim + """,""" + sReportFrom.Trim + """);"
        End If
      End If

      If String.IsNullOrEmpty(notesSearch_date.Text.Trim) And Not String.IsNullOrEmpty(localCriteria.YachtViewCriteriaNoteStartDate.Trim) Then
        notesSearch_date.Text = localCriteria.YachtViewCriteriaNoteStartDate
      End If

      System.Web.UI.ScriptManager.RegisterStartupScript(Me.bottom_tab_update_panel, Me.bottom_tab_update_panel.GetType(), "notes_view_script", notesScript, True)

    End If

  End Sub

End Class