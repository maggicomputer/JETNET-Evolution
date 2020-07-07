
' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/DisplayCompanyDetailold.aspx.vb $
'$$Author: Mike $
'$$Date: 3/20/20 8:53a $
'$$Modtime: 3/20/20 8:52a $
'$$Revision: 1 $
'$$Workfile: DisplayCompanyDetailold.aspx.vb $
'
' ********************************************************************************

Partial Public Class DisplayCompanyDetailold
  Inherits System.Web.UI.Page
  Dim CompanyID As Long = 0
  Dim JournalID As Long = 0
  'Dim CompanyAddress As String = ""
  Dim DoingBusinessAs As String = ""
  Dim runMap As Boolean = False
  Dim CRMView As Boolean = False
  Public txtAlias As String = ""
  Dim DisplayMobile As Boolean = False 'This is just a holder variable until something is added to the asax.
  Dim CRMSource As String = "JETNET"
  Dim CRMJetnetID As Long = 0
  Dim ValidatePermissions As Boolean = False
  Dim use_insight_op As Boolean = False
  Dim use_insight_roll As Boolean = False
  Dim use_insight_own As Boolean = False
  Dim use_insight_manu As Boolean = False
  Dim use_insight_dealer As Boolean = False
  Dim use_insight_lease As Boolean = False
  Dim use_insight_finance As Boolean = False
  Dim operator_functions As New operator_view_functions
  Dim util_functions As New utilization_functions
  Dim searchCriteria As New viewSelectionCriteriaClass
  Dim acdealer_view_function As New aircraft_dealer_functions
  Dim AclsData_Temp As New clsData_Manager_SQL
  Dim manufacturer_functions As New manufacturer_view_functions
  Dim market_functions As New market_model_functions
  Private localDatalayer As viewsDataLayer
  Dim lease_functions As New lease_view_functions
  Dim financial_documents_functions As New financial_view_functions
  Dim amod_id As Long = -1
  Dim amod_make_name As String = ""
  Dim amod_model_name As String = ""
  Dim aport_id As Long = 0
  Dim OtherID As Long = 0
  Private sTask As String = ""
  Public Shared masterPage As New Object

  Private Sub DisplayCompanyDetail_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

    Try

      If Session.Item("crmUserLogon") <> True And Trim(Request("homebase")) <> "Y" Then
        Response.Redirect("Default.aspx", False)
      ElseIf HttpContext.Current.Session.Item("jetnetWebHostType") <> crmWebClient.eWebHostTypes.HOMEBASE And Trim(Request("homebase")) = "Y" Then
        ' if we arent on homebase.com, but have passed homebasee, then bad
        Response.Redirect("Default.aspx", False)
      Else

        masterPage.SetContainerClass("container MaxWidthRemove") 'set full width page
        Session.Item("COMPANY_HAS_YACHTS") = ""

        If Not IsNothing(Request.Item("task")) Then
          If Not String.IsNullOrEmpty(Request.Item("task").ToString.Trim) Then
            sTask = Request.Item("task").ToString.ToUpper.Trim

            If CompanyID = 0 Then
              If Trim(Session("LAST_COMP")) <> "" Then
                If IsNumeric(Session("LAST_COMP")) Then
                  CompanyID = CDbl(Session("LAST_COMP"))
                End If
              End If
            End If

          End If
        End If

        'First thing is First, we need to determine the company that we're on.
        If Not IsNothing(Request.Item("compid")) Then
          If Not String.IsNullOrEmpty(Request.Item("compid").ToString) Then
            CompanyID = CLng(Request.Item("compid").ToString.Trim)
            Session("LAST_COMP") = CompanyID
            newWindow.Text = "<a href='javascript:openSmallWindowJS(""DisplayEventsDetailListing.aspx?CompanyID=" + CompanyID.ToString + """,""EventsDetail"");' title=""Show Events In New Window""  class=""float_right"">New Window</a>"
          End If
        End If

        If Not IsNothing(Request("mobile")) Then
          If Trim(Request("mobile")) = "true" Then
            DisplayMobile = True
          End If
        End If

        ' added msw for company/yacht re-posting
        If CompanyID = 0 Then
          If Trim(Session("LAST_COMP")) <> "" And Trim(Request("order_by")) <> "" Then
            If IsNumeric(Session("LAST_COMP")) Then
              CompanyID = CDbl(Session("LAST_COMP"))
            End If
          End If
        End If

        If Not IsNothing(Request.Item("amod_id")) Then
          If Not String.IsNullOrEmpty(Request.Item("amod_id").ToString) Then
            amod_id = CLng(Request.Item("amod_id").ToString.Trim)
          Else
            amod_id = -1
          End If
        Else
          amod_id = -1
        End If

        ' added, if rolled up , show all / 100 ac 
        If Trim(Request("full_ac")) = "Y" Or Trim(Request("use_insight_roll")) = "Y" Then
          aircraftDataGrid.PageSize = 1000
        End If

        If Not IsNothing(Request.Item("jid")) Then
          If Not String.IsNullOrEmpty(Request.Item("jid").ToString) Then
            JournalID = CLng(Request.Item("jid").ToString.Trim)
          End If
        End If


        If Not Page.IsPostBack Then
          If Not IsNothing(Request.Item("map")) Then
            If Not String.IsNullOrEmpty(Request.Item("map").ToString) Then
              If Trim(Request.Item("map")) = "1" Then
                runMap = True
              End If
            End If
          End If
        End If




        If clsGeneral.clsGeneral.isCrmDisplayMode() Then
          CRMView = True
          new_company_link.Visible = True
          ' COMMENTED OUT MSW - 5/30/18
          foldersContainer.Visible = False
          'view_folders.Visible = False
          notesPanel.Visible = False
          actionPanel.Visible = False
          masterPage.aclsData_Temp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase") 'CApplication.Item("crmClientDatabase")
          If Not IsNothing(Trim(HttpContext.Current.Request("source"))) Then
            If Not String.IsNullOrEmpty(HttpContext.Current.Request("source")) Then
              CRMSource = Trim(HttpContext.Current.Request("source"))
            End If
          End If


          prospectsContainer.CssClass = ""

          If CRMSource = "CLIENT" Then 'check for JETNET
            Dim ClientCheck As DataTable = masterPage.aclsData_Temp.GetCompanyInfo_ID(CompanyID, CRMSource, 0)
            If Not IsNothing(ClientCheck) Then 'not nothing
              If ClientCheck.Rows.Count > 0 Then
                OtherID = ClientCheck.Rows(0).Item("jetnet_comp_id")
              End If
            End If
          Else 'Check for Jetnet
            Dim ClientCheck As DataTable = masterPage.aclsData_Temp.CheckforCompanyBy_JETNET_ID(CompanyID, "")
            If Not IsNothing(ClientCheck) Then 'not nothing
              If ClientCheck.Rows.Count > 0 Then
                OtherID = ClientCheck.Rows(0).Item("comp_id")
              End If
            End If

          End If
        Else
          RunTellJetnetAboutChangesCode()
          prospectsContainer.CssClass = "display_none"
        End If
        '----------------------------------------------------------------------------------------
        'NOTE: I want to put this in only on postback, but because it's being built dynamically,
        'and controls are being added with handlers, it faces the same problem as the aircraft listing page advanced search. 
        'Meaning that if not built on every init, they won't exist.
        'So this is a note to take a look at this whenever the aircraft listing page is 
        'worked through and use the same approach that was decided on there.
        '----------------------------------------------------------------------------------------
        'This Function Builds the Dynamic Table for Static Folders. This will allow them to add 
        'Aircraft to folders and this will only be built once. This is also built on page initialization because
        'It's adding dynamic controls to the page. These have to be put in at the very begining of the page lifecycle of the viewstate
        'will not be set.
        If Session.Item("isMobile") Then
        Else
          Build_Dynamic_Folder_Table()
        End If
        If (Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER) Or (Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE) Then
          new_company_link.Visible = False
          cssExportMenu2.Visible = True
        End If

      End If

    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /> error on DisplayCompanyDetail_Init : " + Now.ToString + " [ " + ex.Message.Trim + "]<br />"

    End Try

  End Sub

  Private Sub RunTellJetnetAboutChangesCode()

    Try

      If (Session.Item("jetnetAppVersion") <> Constants.ApplicationVariable.CUSTOMER_CENTER) And (Session.Item("jetnetAppVersion") <> Constants.ApplicationVariable.HOMEBASE) Then

        If Not Page.ClientScript.IsClientScriptBlockRegistered("popups") Then
          Dim modalScript As StringBuilder = New StringBuilder()
          Dim modalPostbackScript As StringBuilder = New StringBuilder()

          DisplayFunctions.BuildJavascriptTellJetnetAboutChanges(modalPostbackScript, modalScript, 0, JournalID, CompanyID, TellJetnetAboutChanges, TellJetnetAboutChangesForm, includeJqueryTheme, notifyIframe)
          System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "popupsPost", modalPostbackScript.ToString, True)
          System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "popups", " jQuery(document).ready(function() {" & modalScript.ToString & ";});", True)
        End If

      End If

    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /> error on RunTellJetnetAboutChangesCode : " + Now.ToString + " [ " + ex.Message.Trim + "]<br />"

    End Try

  End Sub

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    Try

      Dim trans_date As String = ""
      Dim useBackupSQL As Boolean = CBool(My.Settings.useBackupSQL_SRV.ToString)
      Dim temp_spot As Integer = 0
      Dim roll_link As String = ""

      Dim count_current_ownser As Long = 0
      Dim count_past_owner As Long = 0
      Dim count_operator As Long = 0
      Dim count_manu As Long = 0
      Dim count_dealer As Long = 0
      Dim count_locations As Long = 0
      Dim close_link As String = ""
      Dim count_lease As Long = 0
      Dim count_finance As Long = 0
      Dim current_insight As String = ""
      Dim out_HistoricalRs As New DataTable

      If Session.Item("crmUserLogon") <> True And Trim(Request("homebase")) <> "Y" Then
        Response.Redirect("Default.aspx", False)
      ElseIf HttpContext.Current.Session.Item("jetnetWebHostType") <> crmWebClient.eWebHostTypes.HOMEBASE And Trim(Request("homebase")) = "Y" Then
        ' if we arent on homebase.com, but have passed homebasee, then bad
        Response.Redirect("Default.aspx", False)
      Else


        'addes MSW, if its a client record, then run
        If Trim(CRMSource) = "CLIENT" Then
          crmWebClient.CompanyFunctions.Fill_Information_Tab(Nothing, information_label, masterPage, CompanyID, JournalID, DoingBusinessAs, about_label, New AjaxControlToolkit.TabContainer, company_address, company_name, False, True, CRMSource, CRMJetnetID, OtherID)
          ' Me.company_details_report_panel.Visible = False  ' panel and making it invisible when its a client record 
        Else
          CRMJetnetID = CompanyID
        End If



        'Response.BufferOutput = True
        'What should this page do?
        'First we should verify that the user is logged in.
        'Even though we check on the masterpage, I don't think it hurts to add a check here:
        'Plus I'm stopping the page from running without a compID
        'No point in running if a companyID isn't there.   

        If AclsData_Temp.is_aerodex_insight() = True Then


          acdealer_view_function.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
          acdealer_view_function.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
          acdealer_view_function.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
          acdealer_view_function.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
          acdealer_view_function.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim


          util_functions.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
          util_functions.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
          util_functions.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
          util_functions.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
          util_functions.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim




          If Not IsPostBack Then
            If Not IsNothing(Request("use_insight_op")) Then
              If Trim(Request("use_insight_op")) = "Y" Then
                use_insight_op = True
              End If
            End If

            If Not IsNothing(Request("use_insight_own")) Then
              If Trim(Request("use_insight_own")) = "Y" Then
                use_insight_own = True
              End If
            End If

            If Not IsNothing(Request("use_insight_roll")) Then
              If Trim(Request("use_insight_roll")) = "Y" Then
                use_insight_roll = True
                roll_link = "&use_insight_roll=Y"
              End If
            End If

            If Not IsNothing(Request("use_insight_manu")) Then
              If Trim(Request("use_insight_manu")) = "Y" Then
                use_insight_manu = True
                manufacturer_functions.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
                manufacturer_functions.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
                manufacturer_functions.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
                manufacturer_functions.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
                manufacturer_functions.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim

                market_functions.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
                market_functions.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
                market_functions.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
                market_functions.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
                market_functions.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim
              End If
            End If

            If Not IsNothing(Request("use_insight_dealer")) Then
              If Trim(Request("use_insight_dealer")) = "Y" Then
                use_insight_dealer = True
              End If
            End If

            If Not IsNothing(Request("use_insight_lease")) Then
              If Trim(Request("use_insight_lease")) = "Y" Then
                use_insight_lease = True


                lease_functions.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
                lease_functions.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
                lease_functions.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
                lease_functions.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
                lease_functions.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim

              End If
            End If

            If Not IsNothing(Request("use_insight_finance")) Then
              If Trim(Request("use_insight_finance")) = "Y" Then
                use_insight_finance = True
                financial_documents_functions.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
              End If
            End If
          End If






          searchCriteria.ViewCriteriaAmodID = amod_id
          searchCriteria.ViewCriteriaCompanyID = CRMJetnetID
          searchCriteria.ViewCriteriaTimeSpan = 6
          searchCriteria.ViewCriteriaCompanyID = CompanyID


          If use_insight_roll = True Then
            Call util_functions.get_company_profile_top_function(searchCriteria, "company", count_current_ownser, count_past_owner, count_operator, count_manu, count_dealer, count_locations, count_lease, count_finance, "Y")
          Else
            Call util_functions.get_company_profile_top_function(searchCriteria, "company", count_current_ownser, count_past_owner, count_operator, count_manu, count_dealer, count_locations, count_lease, count_finance, "N")
          End If




          If count_operator > 0 Then
            Me.operations_link.Visible = True
            Me.li_start0.Visible = True
            Me.li_end0.Visible = True
            view_company_insight.Visible = True
          Else
            Me.operations_link.Visible = False
            Me.li_start0.Visible = False
            Me.li_end0.Visible = False
          End If

          If count_current_ownser > 0 Or count_past_owner > 0 Then
            Me.ownership_link.Visible = True
            Me.li_start1.Visible = True
            Me.li_end1.Visible = True
            view_company_insight.Visible = True
          Else
            Me.ownership_link.Visible = False
            Me.li_start1.Visible = False
            Me.li_end1.Visible = False
          End If

          If count_locations > 1 Then
            Me.rollup_link.Visible = True
          Else
            Me.rollup_link.Visible = False
          End If

          If count_manu > 0 Then
            Me.manu_link.Visible = True
            Me.li_start2.Visible = True
            Me.li_end2.Visible = True
            view_company_insight.Visible = True
          Else
            Me.manu_link.Visible = False
            Me.li_start2.Visible = False
            Me.li_end2.Visible = False
          End If

          If count_dealer > 0 Then
            Me.dealer_link.Visible = True
            Me.li_start3.Visible = True
            Me.li_end3.Visible = True
            view_company_insight.Visible = True
          Else
            Me.dealer_link.Visible = False
            Me.li_start3.Visible = False
            Me.li_end3.Visible = False
          End If


          If count_lease > 0 Then
            Me.lease_link.Visible = True
            Me.li_start4.Visible = True
            Me.li_end4.Visible = True
            view_company_insight.Visible = True
          Else
            Me.lease_link.Visible = False
            Me.li_start4.Visible = False
            Me.li_end4.Visible = False
          End If

          If count_finance > 0 Then
            Me.financial_link.Visible = True
            Me.li_start5.Visible = True
            Me.li_end5.Visible = True
            view_company_insight.Visible = True
          Else
            Me.financial_link.Visible = False
            Me.li_start5.Visible = False
            Me.li_end5.Visible = False
          End If


          Me.li_start6.Visible = True
          Me.li_end6.Visible = True
          Me.portfolio_link.Visible = True

          If Not IsNothing(Request("use_insight_roll")) Then
            If Trim(Request("use_insight_roll")) = "Y" Then
              Me.portfolio_link.Text = "<a href='#' onclick=""javascript:load('userPortfolio.aspx?comp_id=" & CRMJetnetID & "&use_insight_roll=Y','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');return false;"">Portfolio Analysis</a>"
            Else
              Me.portfolio_link.Text = "<a href='#' onclick=""javascript:load('userPortfolio.aspx?comp_id=" & CRMJetnetID & "','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');return false;"">Portfolio Analysis</a>"
            End If
          Else
            Me.portfolio_link.Text = "<a href='#' onclick=""javascript:load('userPortfolio.aspx?comp_id=" & CRMJetnetID & "','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');return false;"">Portfolio Analysis</a>"
          End If




          If view_company_insight.Visible = True Then
            view_company_history.CssClass = "gray_button float_left" 'remove noBefore class on history link
          End If

          '  operations_link.Text = "<a href='#' onclick=""javascript:load('DisplayCompanyDetail.aspx?compid=" & CompanyID & "&amod_id=" & amod_id & "&use_insight_op=Y','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');return false;"">Aircraft Operations</a>"
          ' ownership_link.Text = "<a href='#' onclick=""javascript:load('DisplayCompanyDetail.aspx?compid=" & CompanyID & "&amod_id=" & amod_id & "&use_insight_op=Y','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');return false;"">Aircraft Ownership</a>"
          'rollup_link.Text = "<a href='#' onclick=""javascript:load('DisplayCompanyDetail.aspx?compid=" & CompanyID & "&amod_id=" & amod_id & "&use_insight_op=Y','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');return false;"">Rollup Locations</a>"

          If use_insight_op = True Then
            current_insight = "&use_insight_op=Y"
            operations_link.Text = "<a href='DisplayCompanyDetail.aspx?compid=" & CRMJetnetID & "&amod_id=" & amod_id & "&aport_id=" & aport_id & "" & roll_link & "' class=""subMenuText"">Close Aircraft Operations</a>"
          Else
            operations_link.Text = "<a href='DisplayCompanyDetail.aspx?compid=" & CRMJetnetID & "&amod_id=" & amod_id & "&aport_id=" & aport_id & "&use_insight_op=Y" & roll_link & "' class=""subMenuText"">Aircraft Operations</a>"
          End If

          If use_insight_own = True Then
            current_insight = "&use_insight_own=Y"
            ownership_link.Text = "<a href='DisplayCompanyDetail.aspx?compid=" & CRMJetnetID & "&amod_id=" & amod_id & "&aport_id=" & aport_id & "" & roll_link & "' class=""subMenuText"">Close Aircraft Ownership</a>"
          Else
            ownership_link.Text = "<a href='DisplayCompanyDetail.aspx?compid=" & CRMJetnetID & "&amod_id=" & amod_id & "&aport_id=" & aport_id & "&use_insight_own=Y" & roll_link & "' class=""subMenuText"">Aircraft Ownership</a>"
          End If

          If use_insight_manu = True Then
            current_insight = "&use_insight_manu=Y"
            Me.manu_link.Text = "<a href='DisplayCompanyDetail.aspx?compid=" & CRMJetnetID & "&amod_id=" & amod_id & "&aport_id=" & aport_id & "" & roll_link & "' class=""subMenuText"">Close Aircraft Manufactured</a>"
          Else
            Me.manu_link.Text = "<a href='DisplayCompanyDetail.aspx?compid=" & CRMJetnetID & "&amod_id=" & amod_id & "&aport_id=" & aport_id & "&use_insight_manu=Y" & roll_link & "' class=""subMenuText"">Aircraft Manufactured</a>"
          End If

          If Session.Item("localSubscription").crmAerodexFlag = True Then
            Me.dealer_link.Visible = False
            Me.li_start3.Visible = False
            Me.li_end3.Visible = False
          Else
            If use_insight_dealer = True Then
              current_insight = "&use_insight_dealer=Y"
              Me.dealer_link.Text = "<a href='DisplayCompanyDetail.aspx?compid=" & CRMJetnetID & "&amod_id=" & amod_id & "&aport_id=" & aport_id & "" & roll_link & "' class=""subMenuText"">Close Dealer Performance</a>"
            Else
              Me.dealer_link.Text = "<a href='DisplayCompanyDetail.aspx?compid=" & CRMJetnetID & "&amod_id=" & amod_id & "&aport_id=" & aport_id & "&use_insight_dealer=Y" & roll_link & "' class=""subMenuText"">Dealer Performance</a>"
            End If
          End If


          If use_insight_lease = True Then
            current_insight = "&use_insight_lease=Y"
            Me.lease_link.Text = "<a href='DisplayCompanyDetail.aspx?compid=" & CRMJetnetID & "&amod_id=" & amod_id & "&aport_id=" & aport_id & "" & roll_link & "' class=""subMenuText"">Close Aircraft Leasing</a>"
          Else
            Me.lease_link.Text = "<a href='DisplayCompanyDetail.aspx?compid=" & CRMJetnetID & "&amod_id=" & amod_id & "&aport_id=" & aport_id & "&use_insight_lease=Y" & roll_link & "' class=""subMenuText"">Aircraft Leasing</a>"
          End If


          If Session.Item("localSubscription").crmAerodexFlag = True Then
            Me.financial_link.Visible = False
            Me.li_start5.Visible = False
            Me.li_end5.Visible = False
          Else
            If use_insight_finance = True Then
              current_insight = "&use_insight_finance=Y"
              Me.financial_link.Text = "<a href='DisplayCompanyDetail.aspx?compid=" & CRMJetnetID & "&amod_id=" & amod_id & "&aport_id=" & aport_id & "" & roll_link & "' class=""subMenuText"">Close Financial Documents</a>"
            Else
              Me.financial_link.Text = "<a href='DisplayCompanyDetail.aspx?compid=" & CRMJetnetID & "&amod_id=" & amod_id & "&aport_id=" & aport_id & "&use_insight_finance=Y" & roll_link & "' class=""subMenuText"">Financial Documents</a>"
            End If
          End If


          If amod_id > 0 Then
            Me.clear_model.Visible = True
          Else
            Me.clear_model.Visible = False
          End If

          If use_insight_roll = True Then
            rollup_link.Text = "<a href='DisplayCompanyDetail.aspx?compid=" & CRMJetnetID & "&amod_id=" & amod_id & "&aport_id=" & aport_id & "&use_insight_roll=N" & current_insight & "'><font color='#25517d'>SHOW MY LOCATION ONLY</font></a>"
            Me.clear_model.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href='DisplayCompanyDetail.aspx?compid=" & CRMJetnetID & "&aport_id=" & aport_id & "&use_insight_roll=Y" & current_insight & "'><strong><font color='#25517d'>CLEAR AIRCRAFT MODEL</font></strong></a>"
          Else
            rollup_link.Text = "" & count_locations & " LOCATIONS - <a href='DisplayCompanyDetail.aspx?compid=" & CRMJetnetID & "&amod_id=" & amod_id & "&aport_id=" & aport_id & "&use_insight_roll=Y" & current_insight & "'><font color='#25517d'>SHOW ALL</font></a>"
            Me.clear_model.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href='DisplayCompanyDetail.aspx?compid=" & CRMJetnetID & "&aport_id=" & aport_id & "&use_insight_roll=N" & current_insight & "'><strong><font color='#25517d'>CLEAR AIRCRAFT MODEL</font></strong></a>"
          End If

          ' if we are rolled but no insight
          If use_insight_roll = True And Trim(current_insight) = "" Then
            operations_link.Text = "<a href='DisplayCompanyDetail.aspx?compid=" & CRMJetnetID & "&amod_id=" & amod_id & "&aport_id=" & aport_id & "&use_insight_op=Y&use_insight_roll=Y' class=""subMenuText"">Aircraft Operations</a>"
            ownership_link.Text = "<a href='DisplayCompanyDetail.aspx?compid=" & CRMJetnetID & "&amod_id=" & amod_id & "&aport_id=" & aport_id & "&use_insight_own=Y&use_insight_roll=Y' class=""subMenuText"">Aircraft Ownership</a>"
            rollup_link.Text = "<a href='DisplayCompanyDetail.aspx?compid=" & CRMJetnetID & "&aport_id=" & aport_id & "'><font color='#25517d'>SHOW MY LOCATION ONLY</font></a>"
            Me.dealer_link.Text = "<a href='DisplayCompanyDetail.aspx?compid=" & CRMJetnetID & "&amod_id=" & amod_id & "&aport_id=" & aport_id & "&use_insight_dealer=Y&use_insight_roll=Y' class=""subMenuText"">Dealer Performance</a>"
            Me.manu_link.Text = "<a href='DisplayCompanyDetail.aspx?compid=" & CRMJetnetID & "&amod_id=" & amod_id & "&aport_id=" & aport_id & "&use_insight_manu=Y&use_insight_roll=Y' class=""subMenuText"">Aircraft Manufactured</a>"

          ElseIf use_insight_roll = False And Trim(current_insight) = "" Then ' just normal company 
            operations_link.Text = "<a href='DisplayCompanyDetail.aspx?compid=" & CRMJetnetID & "&amod_id=" & amod_id & "&aport_id=" & aport_id & "&use_insight_op=Y' class=""subMenuText"">Aircraft Operations</a>"
            ownership_link.Text = "<a href='DisplayCompanyDetail.aspx?compid=" & CRMJetnetID & "&amod_id=" & amod_id & "&aport_id=" & aport_id & "&use_insight_own=Y' class=""subMenuText"" class=""subMenuText"">Aircraft Ownership</a>"
            rollup_link.Text = "" & count_locations & " LOCATIONS - <a href='DisplayCompanyDetail.aspx?compid=" & CRMJetnetID & "&amod_id=" & amod_id & "&aport_id=" & aport_id & "&use_insight_roll=Y'><font color='#25517d'>SHOW ALL</font></a>"
            Me.dealer_link.Text = "<a href='DisplayCompanyDetail.aspx?compid=" & CRMJetnetID & "&amod_id=" & amod_id & "&aport_id=" & aport_id & "&use_insight_dealer=Y' class=""subMenuText"">Dealer Performance</a>"
            Me.manu_link.Text = "<a href='DisplayCompanyDetail.aspx?compid=" & CRMJetnetID & "&amod_id=" & amod_id & "&aport_id=" & aport_id & "&use_insight_manu=Y' class=""subMenuText"">Aircraft Manufactured</a>"
          End If

          If use_insight_op = True Then
            Me.faa_data_link.Text &= "<Br/></br>UTILIZATION SUMMARIES BASED ON FLIGHT DATA&nbsp;<a href='/help/documents/589.pdf' target='_blank'><img src='images/info.png' width='15' alt='View UTILIZATION SUMMARIES BASED ON FLIGHT DATA' border='0'/></a>"
            Me.faa_data_link.Visible = True
          End If

        End If



        '  Me.rollup_link.Text = "<font color='#25517d'>" & Me.rollup_link.Text & "</font>"

        If use_insight_op = True Or use_insight_own = True Or use_insight_roll Or use_insight_dealer = True Or use_insight_manu = True Then


          If use_insight_roll = True Then
            util_functions.rollup_text = " and comp_id in (select distinct RelCompID from ReturnAllCompanyLocationsByCompId(" & CRMJetnetID & ")) "
          Else
            util_functions.rollup_text = ""
          End If

          If use_insight_own = True Then
            util_functions.use_owner = True
          Else
            util_functions.use_owner = False
          End If

          If use_insight_op = True Then
            util_functions.use_operator = True
          Else
            util_functions.use_operator = False
          End If

          If use_insight_dealer = True Then
            util_functions.use_insight_dealer = True
          Else
            util_functions.use_insight_dealer = False
          End If

          If use_insight_manu = True Then
            util_functions.use_insight_manu = True
          Else
            util_functions.use_insight_manu = False
          End If
        End If


        If use_insight_op = True And use_insight_roll = True Then
          relationshipHeaderText.Text = "OPERATING LOCATIONS (LAST YEAR)"
        ElseIf use_insight_own = True And use_insight_roll = True Then
          relationshipHeaderText.Text = "Ownership Locations"
        ElseIf use_insight_finance = True And use_insight_roll = True Then
          relationshipHeaderText.Text = "Related Financing Companies Financial Documents"
        Else
          'relationshipHeaderText.Text = "Company Locations"
          relationshipHeader.Visible = False
        End If

        If Trim(Request("jetnet_note")) = "Y" Then
          Dim productType As String = "aviation"
          Dim productName As String = "JETNET Evolution"
          If Not String.IsNullOrEmpty(Request("welcome")) Then
            If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO Then
              productType = "yacht"
              productName = "YachtSpot"
            End If
          End If

          masterPage.SetPageTitle("JETNET LLC")
          'Me.note_label.Text = "<p align='left'>JETNET LLC offers a companion service for the Aviation community named JETNET Evolution.  To view additional information regarding the aircraft listed for a given company or contact, email customerservice@jetnet.com or JETNET at 1-800-553-8638 for information regarding a JETNET Evolution subscription.</p>"

          Me.note_label.Text = "<p class=""clear_right padding"">"
          Me.note_label.Text += "JETNET LLC offers a companion service for the " & DisplayFunctions.ConvertToTitleCase(productType) & " community named <b>" & productName & "</b>.</p>"
          Me.note_label.Text += "<p class=""clear_right padding"">"
          Me.note_label.Text += "To view additional information regarding"

          If Not String.IsNullOrEmpty(Request("welcome")) Then
            'We're going to put in a log right here whenever we view this message (click the button to swap in yachtspot or evo). The button is in the header (the yacht/plane icon).
            If Not IsPostBack Then
              Call commonLogFunctions.Log_User_Event_Data("UserStatistics", "User Clicked " & productName & " Icon", Nothing, 0, JournalID, 0, CompanyID)
            End If
            Me.note_label.Text += " " & productType & " related services"
          Else
            Me.note_label.Text += " the aircraft listed for a given company or contact"
          End If

          Me.note_label.Text += ", email <a href=""mailto:customerservice@jetnet.com"">"
          Me.note_label.Text += " customerservice@jetnet.com</a> or JETNET at 1-800-553-8638 for information regarding"
          Me.note_label.Text += " a " & productType & " subscription."
          Me.note_label.Text += "</p>"

          Me.toggle_vis_note.Visible = True
          Me.toggle_vis.Visible = False
        Else
          If CompanyID <> 0 Then
            Dim ValidateCompanyInfo As New DataTable
            If Trim(Request("homebase")) = "Y" Then
              masterPage.aclsData_Temp.JETNET_DB = Session.Item("jetnetAdminDatabase")
              Session.Item("localSubscription").crmBusiness_Flag = True
              Session.Item("localSubscription").crmHelicopter_Flag = True
              Session.Item("localSubscription").crmCommercial_Flag = True
              Session.Item("localSubscription").crmJets_Flag = True
              Session.Item("localSubscription").crmExecutive_Flag = True
              Session.Item("localSubscription").crmTurboprops = True
              ValidatePermissions = True
              If Trim(Request("history")) = "Y" Then
                Get_Company_History()
              End If

            End If

            If CRMView = True And CRMSource = "CLIENT" Then
              ValidatePermissions = True
            Else
              ValidateCompanyInfo = CheckCompanyPermissions(CompanyID, JournalID)
              If Not IsNothing(ValidateCompanyInfo) Then
                If ValidateCompanyInfo.Rows.Count > 0 Then
                  ValidatePermissions = True
                End If
              End If
            End If

            If (Session.Item("crmUserLogon") <> True And Trim(Request("homebase")) <> "Y") Or ValidatePermissions = False Then
              Response.Redirect("Default.aspx", False)
            Else

              ' show "chat now icon budy" if chat enabled
              Dim bEnableChat = HttpContext.Current.Session.Item("localPreferences").ChatEnabled

              Dim isHomebase As Boolean = False

              If Not IsNothing(HttpContext.Current.Request("homebase")) Then
                If Not String.IsNullOrEmpty(HttpContext.Current.Request("homebase").ToString.Trim) Then
                  isHomebase = IIf(HttpContext.Current.Request("homebase").ToString.Trim.Contains("Y"), True, False)
                End If
              End If

              If bEnableChat And Not isHomebase Then

                Dim tempTable As New DataTable
                Dim tmpPrefobj As New preferencesDataLayer

                tmpPrefobj.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

                Try
                  tempTable = tmpPrefobj.ReturnUserDetailsAndImage(HttpContext.Current.Session.Item("localUser").crmUserContactID)

                  If Not IsNothing(tempTable) Then
                    If tempTable.Rows.Count > 0 Then

                      For Each r As DataRow In tempTable.Rows

                        If String.IsNullOrEmpty(txtAlias.Trim) Then
                          If Not (IsDBNull(r.Item("contact_email_address"))) And Not String.IsNullOrEmpty(r.Item("contact_email_address").ToString) Then
                            txtAlias = r.Item("contact_email_address").ToString.Trim
                          End If
                        End If

                      Next

                    End If

                  End If

                Catch ex As Exception
                End Try

                tempTable = Nothing
                tmpPrefobj = Nothing

              End If

              If (Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER) Or (Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE) Then
                Dim ShowSubscriptionSummary As Boolean = False

                Fill_Admin_Action_Items()
                actionPanel.Visible = True

                Fill_Service_Summary_Tab(ShowSubscriptionSummary)
                servicesContainer.Visible = True


                Fill_Trials_Summary_Tab()
                Trials_Container.Visible = True

                If ShowSubscriptionSummary = True Then
                  Fill_Subscription_Summary_Tab()
                  subscriptionSummaryContainer.Visible = True
                Else
                  subscriptionSummaryContainer.Visible = False
                End If

                'Fill_Active_User_Tab()
                'activeUserContainer.Visible = True

                Fill_Services_Used_Tab()
                services_used_panel.Visible = True

                'Fill_Customer_Activities_Tab()

                Fill_Customer_Activities_FromView()
                customer_activities_panel.Visible = True

                Fill_Research_Notes()
                researchNotesPanel.Visible = True

                'Fill_Contract_Execution_Tab()
                'contract_execution_panel.Visible = True

                'Fill_Contract_List_Tab()
                'contract_list_panel.Visible = True


                Dim tmpTable As DataTable = getMarketingNote(CompanyID)
                Dim tmpText As String = ""

                If Not IsNothing(tmpTable) Then
                  If tmpTable.Rows.Count > 0 Then

                    For Each r As DataRow In tmpTable.Rows

                      If Not (IsDBNull(r("comp_marketing_notes"))) Then
                        tmpText = r.Item("comp_marketing_notes").ToString.Trim
                      End If
                    Next

                  End If
                End If



                ' If Not String.IsNullOrEmpty(tmpText.Trim) Then
                marketing_label.Text = "<div class=""Box""><div class=""subHeader"">MARKETING SUMMARY<right><a href='javascript:void(0);' onclick='javascript:load(""homeTables.aspx?type_of=Company&sub_type_of=companyNote&comp_id=" & CompanyID.ToString & "&action=editCompanyNote"","""",""scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no"");return false;' class=""tiny_text"">Update</a></right></div>"
                marketing_label.Text += "<br />"
                If Not String.IsNullOrEmpty(tmpText.Trim) Then
                  marketing_label.Text += tmpText.Trim & "<hr class=""hrPadding boldHR""/>"
                End If


                marketing_label.Text += get_latest_marketing_notes_top(CompanyID)


                marketing_label.Text += "</div>"
                ' Else
                '     marketing_label.Visible = False
                ' End If

              End If

              If (Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER) Or (Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE) Then
                Dim SalesSubmissionRan As Boolean = False
                Me.submitted_label.Text = ""
                Fill_Sales_Price_Submissions_Tab(SalesSubmissionRan)

                If SalesSubmissionRan Then
                  Fill_Sales_Price_Submitted_On_Tab()
                End If
                dataProviderContainer.Visible = True
              End If

              If Not Page.IsPostBack Then
                If JournalID > 0 Then
                  Dim JournalTable As New DataTable
                  Dim debugQuery As String = ""
                  history_background.CssClass = "history_bg"
                  regular_toggle_buttons.Visible = False
                  'We need to perform a check to make sure this company is still active:
                  'comp_journ_id = 0 
                  'comp_active_flag = 'Y'
                  'comp_hide_flag = 'N'
                  Dim CheckTable As New DataTable
                  CheckTable = masterPage.aclsData_Temp.CheckIfCompanyIsActive(CompanyID, 0, "Y", "N")
                  If Not IsNothing(CheckTable) Then
                    If CheckTable.Rows.Count > 0 Then
                      history_toggle_buttons.Visible = True
                      history_toggle_buttons.Text = "<a " & DisplayFunctions.WriteDetailsLink(0, CompanyID, 0, 0, False, "", "gray_button large_button_width noBefore", "") & " class='gray_button large_button_width noBefore'>View Current Company</a>"
                    Else
                      history_toggle_buttons.Visible = False
                    End If
                  End If


                  'Display Company Block
                  history_information_label.Visible = True


                  history_information_label.Text = CommonAircraftFunctions.DisplayAircraftHistory_TopBlock(0, JournalID, JournalTable, Me.Session, history_information_label, debugQuery, masterPage.aclsData_Temp, CRMSource, 0, CRMView, 0, "JETNET", 0)


                Else
                  history_toggle_buttons.Visible = False

                End If

                'Add help button text here: 7/20/15
                company_help_button_label.Text = clsGeneral.clsGeneral.CreateEvoHelpLink("Company Details")
                company_help_button_label.Text = Replace(company_help_button_label.Text, ">Help", "class=""gray_button float_left"">Help")

                'Set Cookies
                clsGeneral.clsGeneral.Recent_Cookies("companies", CompanyID, IIf(CRMView, CRMSource, "JETNET"))
                If Not IsPostBack Then
                  If CRMView = False Then
                    'insert into content stat  
                    '  Master.aclsData_Temp.Insert_Content_Stat(Now(), 0, 0, 0, CompanyID, 0, JournalID, 0, Session.Item("localUser").crmSubSubID, Session.Item("localUser").crmUserLogin, Session.Item("localUser").crmSubSeqNo, Session.Item("localUser").crmUserContactID)
                    Call commonLogFunctions.Log_User_Event_Data("UserStatistics", "DisplayCompanyDetails: Company_ID = " + CompanyID.ToString, Nothing, 0, JournalID, 0, CompanyID)
                  Else
                    If Session.Item("isEVOLOGGING") = True Or HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO Then
                      If CRMJetnetID > 0 Then
                        Call commonLogFunctions.Log_User_Event_Data("UserStatistics", "DisplayCompanyDetails: Company_ID = " + CRMJetnetID.ToString & " " & IIf(CRMSource = "CLIENT", "Viewing Client Record.", ""), Nothing, 0, 0, 0, CRMJetnetID)
                      End If
                    End If
                  End If
                End If

                'Broken into sections, filling each tab individually:
                'Just as a note, I fill the relationship tab first, this 
                'allows me to get the DBA and store it into a public variable 
                'without having to do another query later on.

                'Fills Information Tab (and About Tab)
                crmWebClient.CompanyFunctions.Fill_Information_Tab(Nothing, information_label, masterPage, CompanyID, JournalID, DoingBusinessAs, about_label, New AjaxControlToolkit.TabContainer, company_address, company_name, False, CRMView, CRMSource, CRMJetnetID, OtherID)

                If use_insight_op = True Or use_insight_own = True Or use_insight_roll Or use_insight_manu = True Or use_insight_dealer = True Or use_insight_finance = True Then
                Else
                  If JournalID = 0 Then
                    Fill_Relationship_Tab()
                  Else
                    relationships_label.Visible = False
                  End If

                  Fill_Business_Type_Tab()


                  'If we're in Evolution, Yacht version, do not display the wanted and certifications.
                  '  If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT Then
                  If Session.Item("localSubscription").crmYacht_Flag = True Or Trim(Request("homebase")) = "Y" Then
                    Fill_Yacht_Tab(Trim(Request("order_by"))) 'Only fill if Yacht.  

                    If JournalID > 0 Then
                      Dim TemporaryHistoryString As String = ""
                      relationships_label.Visible = False
                      view_company_history.Visible = False

                      TemporaryHistoryString = masterPage.aclsData_Temp.Get_Yacht_History(0, CompanyID, 0, JournalID, CRMView, trans_date)
                      If TemporaryHistoryString <> "" Then
                        Me.history_information_label.Text = masterPage.aclsData_Temp.Get_Yacht_History(0, CompanyID, 0, JournalID, CRMView, trans_date)
                      End If
                    Else
                      Fill_Related_Transactions()
                      view_company_history.Visible = True
                    End If

                  End If
                End If

                'Do not display notes/reminders if Yacht
                actionPanel.Visible = False
                notesPanel.Visible = False
                view_notes.Visible = False

                If Session.Item("localSubscription").crmBusiness_Flag = True Then

                  If Not HttpContext.Current.Session.Item("localPreferences").isYachtOnlyProduct Then
                    certifications_label.Text = ""
                    certifications_label.Visible = False
                    Fill_Certifications_Tab("Certificate")
                    Fill_Certifications_Tab("Membership")
                    Fill_Certifications_Tab("Accreditation")
                  End If

                  Fill_Wanteds_Tab()
                  If JournalID = 0 Then
                    view_company_history.Visible = True
                    view_company_events.Visible = True
                  End If
                End If

                'Toggles Notes On/Off based on Flags
                If ((Not Session.Item("localUser").crmDemoUserFlag) And (Session.Item("localUser").crmEnableNotes) _
                                    And Session.Item("localSubscription").crmServerSideNotes_Flag And JournalID = 0) And (Not String.IsNullOrEmpty(Session.Item("jetnetServerNotesDatabase"))) Then
                  actionPanel.Visible = True
                  notesPanel.Visible = True
                  closeNotes.Visible = True

                  Dim ProspectsLinkText As String = "javascript:load('edit_note.aspx?action=new&type=prospect&cat_key=0&from=companydetails&comp_ID=" & CompanyID & "&source=" & IIf(CRMSource = "CLIENT", "CLIENT", "JETNET") & "','unloaded_me','scrollbars=yes,menubar=no,height=435,width=860,resizable=yes,toolbar=no,location=no,status=no');"

                  new_prospects_add.Text = "<p align='right'>+ <a href=""javascript:void(0);"" onclick=""" & ProspectsLinkText & """>Add New Prospect</a></p>"

                  Session.Item("Listing") = 1
                  Session.Item("ListingSource") = "JETNET"
                  Session.Item("ListingID") = CompanyID

                  'If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Or Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
                  '    notesPanel.Visible = False
                  '    view_notes.Visible = False

                  '    AclsData_Temp = New clsData_Manager_SQL
                  '    AclsData_Temp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase") 'CApplication.Item("crmClientDatabase")
                  '    AclsData_Temp.JETNET_DB = Session.Item("jetnetClientDatabase") 'CApplication.Item("crmJetnetDatabase")

                  '    Dim aTempTable As New DataTable
                  '    aTempTable = AclsData_Temp.Notes_Search_For_Prospect_View_Homebase("", "", "", "B','O", 0, "", "", "", "", "", "", 0, 0, "", False, False, "", "", 0, CompanyID)

                  '    prospects_label.Text = DisplayFunctions.CRMDisplay_Notes_Or_Actions_MPM(aTempTable, masterPage.aclsData_Temp, False, False, True, False, False, False, True, False, CRMView, CRMSource, True, False, False)
                  'Else

                  If Trim(Request("source")) = "CLIENT" Then
                    DisplayFunctions.DisplayLocalItems(masterPage.aclsData_Temp, 0, CompanyID, 0, notes_label, action_label, True, False, False, True, 5, False, True, "CLIENT", Nothing, True)
                  Else
                    DisplayFunctions.DisplayLocalItems(masterPage.aclsData_Temp, 0, CompanyID, 0, notes_label, action_label, True, False, False, True, 5, False, CRMView, "JETNET", Nothing, True)
                  End If


                  If Trim(Request("source")) = "CLIENT" Then
                    DisplayFunctions.DisplayLocalItems(masterPage.aclsData_Temp, 0, CompanyID, 0, Nothing, Nothing, True, False, False, False, 0, False, True, "CLIENT", prospects_label)
                  Else
                    DisplayFunctions.DisplayLocalItems(masterPage.aclsData_Temp, 0, CompanyID, 0, Nothing, Nothing, True, False, False, False, 0, False, True, "JETNET", prospects_label)
                  End If
                  'End If



                  notes_update_panel.Update()

                  notes_add_new.Text = "<a href=" & DisplayFunctions.WriteNotesRemindersLinks(0, 0, CompanyID, 0, False, "&n=1", "Add New Note") & " class='special'>NOTES +</a>"

                  action_add_new.Text = "<a href=" & DisplayFunctions.WriteNotesRemindersLinks(0, 0, CompanyID, 0, False, "", "Add New Action") & " class='special'>ACTIONS +</a>"

                  If Trim(Request("source")) = "CLIENT" Then '
                    notes_add_new.Text = "<p align='right'>" & DisplayFunctions.ViewAllNotesLink(CompanyID, 0, CRMSource, "padding_left") & " + " & DisplayFunctions.WriteNotesRemindersLinks(0, 0, CompanyID, 0, True, "&n=1&source=CLIENT", "Add New Note") & "</p>"
                    action_add_new.Text = "<p align='right'>+ " & DisplayFunctions.WriteNotesRemindersLinks(0, 0, CompanyID, 0, True, "&source=CLIENT", "Add New Action") & "</p>"
                  Else
                    notes_add_new.Text = "<p align='right'>" & DisplayFunctions.ViewAllNotesLink(CompanyID, 0, CRMSource, "padding_left") & " + " & DisplayFunctions.WriteNotesRemindersLinks(0, 0, CompanyID, 0, True, "&n=1", "Add New Note") & "</p>"
                    action_add_new.Text = "<p align='right'>+ " & DisplayFunctions.WriteNotesRemindersLinks(0, 0, CompanyID, 0, True, "", "Add New Action") & "</p>"
                  End If


                  view_notes.Visible = True
                ElseIf CRMView = True Then
                  Session.Item("localSubscription").crmServerSideNotes_Flag = True

                  'If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Or Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
                  '    AclsData_Temp = New clsData_Manager_SQL
                  '    AclsData_Temp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase") 'CApplication.Item("crmClientDatabase")
                  '    AclsData_Temp.JETNET_DB = Session.Item("jetnetClientDatabase") 'CApplication.Item("crmJetnetDatabase")
                  '    notesPanel.Visible = False
                  '    view_notes.Visible = False

                  '    Dim aTempTable As New DataTable
                  '    aTempTable = AclsData_Temp.Notes_Search_For_Prospect_View_Homebase("", "", "", "B','O", 0, "", "", "", "", "", "", 0, 0, "", False, False, "", "", 0, CompanyID)

                  '    prospects_label.Text = DisplayFunctions.CRMDisplay_Notes_Or_Actions_MPM(aTempTable, masterPage.aclsData_Temp, False, False, True, False, False, False, True, False, CRMView, CRMSource, True, False, False)
                  '    ProspectUpdate.Update()
                  'Else
                  DisplayFunctions.DisplayLocalItems(masterPage.aclsData_Temp, 0, CompanyID, 0, notes_label, action_label, True, False, False, True, 5, False, CRMView, CRMSource, Nothing, True)
                  notes_update_panel.Update()
                  'End If

                  Dim NotesLinkText As String = "javascript:load('edit_note.aspx?comp_ID=" & CompanyID & "&source=" & CRMSource & "&type=note&action=new','','scrollbars=yes,menubar=no,height=400,width=860,resizable=yes,toolbar=no,location=no,status=no');"
                  Dim ActionsLinkText As String = "javascript:load('edit_note.aspx?comp_ID=" & CompanyID & "&source=" & CRMSource & "&type=action&action=new','','scrollbars=yes,menubar=no,height=400,width=860,resizable=yes,toolbar=no,location=no,status=no');"
                  notes_add_new.Text = "" & DisplayFunctions.ViewAllNotesLink(CompanyID, 0, CRMSource, "padding_left") & " <a href=""javascript:void(0);"" onclick=""" & NotesLinkText & """ class='special'>NOTES +</a> "
                  action_add_new.Text = "<a href=""javascript:void(0);"" onclick=""" & ActionsLinkText & """ class='special'>ACTIONS +</a>"


                  notes_add_new.Text = "<p align='right'>+ <a href=""javascript:void(0);"" class=""float_right"" onclick=""" & NotesLinkText & """>Add New Note</a>" & DisplayFunctions.ViewAllNotesLink(CompanyID, 0, CRMSource, "padded_right float_left") & "</p>"
                  action_add_new.Text = "<p align='right'>+ <a href=""javascript:void(0);"" onclick=""" & ActionsLinkText & """>Add New Action</a></p>"

                  'Reminders.Visible = True
                  'Notes.Visible = True
                  actionPanel.Visible = True
                  notesPanel.Visible = True
                  closeNotes.Visible = True
                  view_notes.Visible = True
                  notes_update_panel.Update()
                Else

                  actionPanel.Visible = False

                  notesPanel.Visible = False
                  foldersContainer.Visible = False
                  'Reminders.Visible = False
                  'Notes.Visible = False
                  view_notes.Visible = False

                End If

                Fill_Contacts_Tab()

                Fill_Share_Relationship_Tab()

                Fill_Aircraft_Tab()

                If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT Then
                  If Session.Item("localSubscription").crmYacht_Flag Then
                    fill_news_tab()
                  Else
                    newsContainer.Visible = False
                  End If
                Else
                  newsContainer.Visible = False
                End If


                If (Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER) Or (Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE) Then
                  Fill_Admin_Action_Items()
                  prospectsContainer.CssClass = ""
                  prospectsContainer.Visible = True
                  notesPanel.Visible = False
                  view_notes.Visible = False
                  Dim ProspectsLinkText As String = "javascript:load('edit_note.aspx?action=new&type=prospect&cat_key=0&from=companydetails&comp_ID=" & CompanyID & "&source=" & IIf(CRMSource = "CLIENT", "CLIENT", "JETNET") & "','unloaded_me','scrollbars=yes,menubar=no,height=435,width=860,resizable=yes,toolbar=no,location=no,status=no');"
                  new_prospects_add.Text = "<p align='right'>+ <a href=""javascript:void(0);"" onclick=""" & ProspectsLinkText & """>Add New Prospect</a></p>"

                  AclsData_Temp = New clsData_Manager_SQL
                  AclsData_Temp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase") 'CApplication.Item("crmClientDatabase")
                  AclsData_Temp.JETNET_DB = Session.Item("jetnetClientDatabase") 'CApplication.Item("crmJetnetDatabase")


                  Dim aTempTable As New DataTable
                  aTempTable = AclsData_Temp.Notes_Search_For_Prospect_View_Homebase("", "", "", "B','O", 0, "", "", "", "", "", "", 0, 0, "", False, False, "'Active'", "", 0, CompanyID, 0)

                  prospects_label.Text = DisplayFunctions.CRMDisplay_Notes_Or_Actions_MPM(aTempTable, masterPage.aclsData_Temp, False, False, False, False, False, False, True, False, CRMView, CRMSource, True, False, False)

                End If



                If runMap = True Then
                  ViewCompanyMap(map_this_company, EventArgs.Empty)
                End If
              End If

            End If

            If InStr(LCase(company_name.Text), "no longer active") > 0 Then
              temp_spot = InStr(LCase(company_name.Text), "<font")
              If temp_spot > 0 Then
                masterPage.SetPageTitle(Left(company_name.Text, temp_spot - 1))
              Else
                masterPage.SetPageTitle(company_name.Text)
              End If
            Else
              masterPage.SetPageTitle(company_name.Text)
            End If


          End If
        End If


        If use_insight_op = True Or use_insight_own = True Or use_insight_roll Or use_insight_dealer = True Or use_insight_manu = True Or use_insight_lease = True Or use_insight_finance = True Then
          about_label.Visible = False
          contacts_label.Visible = False
          'Me.contacts.Visible = False
          business_label.Visible = False
          Me.yachtContainer.Visible = False
          Me.wanteds_label.Visible = False

          'if just roll up 
          If use_insight_roll = True And use_insight_own = False And use_insight_op = False And use_insight_dealer = False And use_insight_manu = False And use_insight_lease = False And use_insight_finance = False Then
            Fill_Relationship_Tab()
            newsContainer.Visible = False
            Me.aircraft_model_panel.Visible = False
            relationships_label.Visible = True
          Else
            newsContainer.Visible = True
            If use_insight_finance = True Then
              aircraft_model_label.Visible = True
              aircraft_model_panel.Visible = True
              modelHeader.Text = "Aircraft Models Financed (Last 6 Months)"
              relationships_label.Visible = True
              'relationships_tab.HeaderText = "Financial Documents By Month (Last 6 Months)"
              'summary.Visible = True
              summary_label.Visible = True
              'summary_tab.HeaderText = "Type of Financial Documents (Last 6 Months)"
              If use_insight_roll Then
                newsContainer.Visible = False ' changed
                all_news.Visible = False
                ' news_tab.HeaderText = "Related Financing Companies Financial Documents"
              Else
                newsContainer.Visible = False
              End If
              masterPage.SetPageText("Financial Documents - " & company_name.Text)
            ElseIf use_insight_own = True Then
              Me.aircraft_model_panel.Visible = True
              'Me.modelHeader.Text = "Aircraft Ownership History"
              Me.newsContainer.Visible = False
              Me.relationships_label.Text = ""
              relationships_label.Visible = True
              masterPage.SetPageText("Ownership - " & company_name.Text)
            ElseIf use_insight_op = True Then
              Me.aircraft_model_panel.Visible = True
              Me.all_news.Visible = False
              Me.relationships_label.Text = ""
              Me.relationships_label.Visible = False
              Me.newsContainer.Visible = True
              masterPage.SetPageText("Operations - " & company_name.Text)
            ElseIf use_insight_dealer = True Then

              Me.all_news.Visible = False
              Me.relationships_label.Text = ""
              relationshipHeader.CssClass = "rollupLink subHeader"
              relationshipHeader.Text = "Dealer Sales Per Year"
              relationships_label.Visible = True
              If searchCriteria.ViewCriteriaAmodID > 1 Then
                Me.aircraft_model_panel.Visible = False
              Else
                Me.aircraft_model_panel.Visible = True
                'modelHeader.Text = "Models Represented"
              End If

              Me.relationships_label.Visible = False
              Me.newsHeader.Text = "DEALER SALES ROLES SINCE " & Year(Now()) - 1
              newsHeader.CssClass = "rollupLink subHeader"
              newsContainer.Visible = True
              summary_label.Visible = True
              ' summary_tab.HeaderText = "DEALER SALES BY MODEL SINCE " & Year(Now()) - 1
              masterPage.SetPageText("Dealer Performance - " & company_name.Text)
            ElseIf use_insight_manu = True Then
              Me.relationships_label.Text = ""
              relationshipHeader.CssClass = "rollupLink subHeader"
              relationshipHeader.Text = "IN OPERATION AIRCRAFT BY MFR YEAR"
              relationships_label.Visible = True
              Me.Company_Relationship_Panel.Visible = True
              newsContainer.Visible = True
              Me.newsHeader.Text = "IN PRODUCTION AIRCRAFT BY MFR YEAR"
              Me.all_news.Visible = False

              If searchCriteria.ViewCriteriaAmodID > 1 Then
                Me.aircraft_model_panel.Visible = False
                relationshipHeaderText.Text = "MANUFACTURER SUMMARY: " & amod_make_name & " " & amod_model_name
              Else
                Me.aircraft_model_panel.Visible = True
                Me.modelHeader.Text = "Models Manufactured"
                relationshipHeaderText.Text = "MANUFACTURER SUMMARY"
              End If
              masterPage.SetPageText("Manufactured - " & company_name.Text)
            ElseIf use_insight_lease = True Then
              Me.relationships_label.Text = ""
              relationshipHeader.Text = "LEASES PER MONTH (Last 6 Months)"
              relationshipHeader.CssClass = "rollupLink subHeader"
              relationships_label.Visible = True

              summary_label.Visible = True
              newsContainer.Visible = False

              If use_insight_roll = True Then
                Me.Company_Relationship_Panel.Visible = True
              Else
                Me.Company_Relationship_Panel.Visible = False
              End If

              If searchCriteria.ViewCriteriaAmodID > 1 Then
                Me.aircraft_model_panel.Visible = False
                relationshipHeaderText.Text = "LESSOR SUMMARY: " & amod_make_name & " " & amod_model_name
              Else
                Me.aircraft_model_panel.Visible = True
                'Me.modelHeader.Text = "MODELS LEASED"
                relationshipHeaderText.Text = "LESSOR SUMMARY"
              End If
              masterPage.SetPageText("Leasing - " & company_name.Text)
            End If




          End If

          If use_insight_manu = True Then
            Me.Company_Relationship_Panel.Visible = True
          ElseIf use_insight_roll = True Then
            Me.Company_Relationship_Panel.Visible = True
          Else
            Me.Company_Relationship_Panel.Visible = False
          End If


          If Not IsNothing(Session.Item("localSubscription").crmSubinst_FAA_data_date) Then
            Session("Last_FAA_DATE") = Session.Item("localSubscription").crmSubinst_FAA_data_date
          End If
          Me.aircraftDataGrid.Columns(8).Visible = True
          Call Fill_Airport_Model_Tab()
        End If

        If Session.Item("isMobile") = True Then
          '1.	Remove export menu completely as well as subitems
          cssExportMenu.Visible = False
          '2.	Remove folders option completely
          view_folders.Visible = False
          '3.	Remove help option completely
          company_help_button_label.Visible = False
        End If

      End If

    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /> error on page load : " + Now.ToString + " [ " + ex.Message.Trim + "]<br />"
    End Try


  End Sub

  Private Sub Fill_Airport_Model_Tab()

    Dim this_section_string As String = ""
    Dim HoldTable As New DataTable
    Dim String_to_return As String = ""

    If use_insight_roll = True Then
      If use_insight_op = True Then
        Call util_functions.util_get_operators_top_function(searchCriteria, Me.Company_Relationship_Label.Text, aport_id, "", 0, "company")
      Else
        Call util_functions.util_get_operators_rollup_top_function(searchCriteria, Me.Company_Relationship_Label.Text, aport_id, "", 0, "company")
      End If
    End If





    If use_insight_own = True Then
      util_functions.get_company_ownership_top_function(searchCriteria, Me.aircraft_model_label.Text, "", "company", "")
      this_section_string = ""
      relationshipHeader.Text = "Purchase History"
      relationshipHeader.CssClass = "rollupLink subHeader"
      If use_insight_roll = True Then
        Call util_functions.get_company_purchase_history_top_function(searchCriteria, this_section_string, "Y", "company")
      Else
        Call util_functions.get_company_purchase_history_top_function(searchCriteria, this_section_string, "N", "company")
      End If

      DisplayFunctions.load_google_chart(New AjaxControlToolkit.TabPanel, this_section_string, "Company Purchase History", "", "chart_div_tab1_all", 0, 0, "POINTS", 1, String_to_return, Me.Page, Me.relationships_udpate_panel, False, False, True, False, False, False, False, True, True, False, 0, "bottom", "", True)
      Call load_google_chart_all(New AjaxControlToolkit.TabPanel, String_to_return)
    ElseIf use_insight_op = True Then
      util_functions.get_flight_activity_by_model_top_function(searchCriteria, Me.aircraft_model_label.Text, "", "company")
      this_section_string = ""
      relationshipHeader.Text = "Aircraft Utilization Summary"
      relationshipHeader.CssClass = "rollupLink subHeader"
      Call util_functions.get_flight_profile_top_function(searchCriteria, this_section_string, "Month", Session("Last_FAA_DATE"), "", "company")

      DisplayFunctions.load_google_chart(New AjaxControlToolkit.TabPanel, this_section_string, "Operator # Flts", "", "chart_div_tab1_all", 0, 0, "POINTS", 1, String_to_return, Me.Page, Me.relationships_udpate_panel, False, False, True, False, False, False, False, True, True, False, 0, "bottom", "", True)
      Call load_google_chart_all(New AjaxControlToolkit.TabPanel, String_to_return)
      ' Me.newsHeader.Text = "Airport Utilization (Last Year)"

      Call util_functions.util_get_operator_airports_top_function(searchCriteria, Me.news_label.Text, 0, "", 0, "", "company", False)
      'Me.news_label.Text = "<div class=""Box"">" & Me.news_label.Text & "</div>"
    ElseIf use_insight_dealer = True Then
      If searchCriteria.ViewCriteriaAmodID > 1 Then
      Else
        HoldTable = acdealer_view_function.ac_dealer_get_models_for_main_comp_id(CompanyID, "", searchCriteria.ViewCriteriaAmodID, searchCriteria, util_functions.rollup_text, "company")

        Call models_operated(HoldTable, Me.aircraft_model_label.Text, CompanyID, searchCriteria.ViewCriteriaAmodID, searchCriteria)
        'Me.aircraft_model_label.Text = "<div class=""tab_container_div"">" & Me.aircraft_model_label.Text & "</div>"
        HoldTable.Rows.Clear()
      End If

      HoldTable = acdealer_view_function.ac_dealer_get_sales_by_year_main_comp_id(CompanyID, "", searchCriteria.ViewCriteriaAmodID, searchCriteria, util_functions.rollup_text, "company")

      If Not IsNothing(HoldTable) Then
        If HoldTable.Rows.Count > 0 Then
          this_section_string = " ['Year', 'Dealer Sales Per Year']"
          For Each r As DataRow In HoldTable.Rows
            this_section_string &= ", ['" & r("TYEAR") & "', " & r("numtrans") & "]"
          Next
        End If
      End If
      HoldTable.Rows.Clear()
      '  DisplayFunctions.load_google_chart(viewTabPanel, google_map_string, "", "", "chart_div_top_all", 480, 230, "ARRAY", 1, charting_string, Me.Page, Me.bottom_tab_update_panel, False, True, True, False, False, True, False)
      DisplayFunctions.load_google_chart(New AjaxControlToolkit.TabPanel, this_section_string, "Dealer Sales Per Year", "Sales", "chart_div_tab1_all", 0, 0, "ARRAY", 1, String_to_return, Me.Page, Me.relationships_udpate_panel, False, True, True, False, False, False, False, False, True, False, 0, "bottom", "", True)

      Dim ac_dealer_view As Boolean = False
      If Trim(Request("ac_dealer_view")) = "Y" Then
        ac_dealer_view = True
      End If

      BuildRestOfDealerCharts(CompanyID, searchCriteria, String_to_return, util_functions.rollup_text, ac_dealer_view)

      Call load_google_chart_all(New AjaxControlToolkit.TabPanel, String_to_return)
    ElseIf use_insight_manu = True Then

      this_section_string = ""
      Call use_manu_functions(Me.Company_Relationship_Label.Text, Me.aircraft_model_label.Text)


    ElseIf use_insight_lease = True Then

      Call use_insight_lease_functions(Me.Company_Relationship_Label.Text, Me.aircraft_model_label.Text)

    ElseIf use_insight_finance = True Then

      Build_Model_Financial_Documents()
      Build_Types_Financial_Documents()
      Build_Financial_Documents_By_Month()

      If use_insight_roll Then
        Build_Related_Financing_Companies_Documents()
      End If


    End If




    'CheckAndJSForDatatable()

  End Sub

  Public Function WriteCompAdd(ByVal r As DataRow) As String
    Dim City As String = ""
    Dim State As String = ""
    Dim Country As String = ""
    Dim DisplayText As String = ""
    If Not IsDBNull(r("comp_name")) Then
      DisplayText += DisplayFunctions.WriteDetailsLink(0, r("comp_id"), 0, 0, True, r("comp_name"), "", "")
      DisplayText += "<br />"
    End If
    If Not IsDBNull(r("comp_city")) Then
      City = r("comp_city")
    End If
    If Not IsDBNull(r("comp_state")) Then
      If City <> "" Then
        City += ", "
      End If
      State += r("comp_state")
    End If
    If Not IsDBNull(r("comp_country")) Then
      If City = "" Then
        If State <> "" Then
          State += ", "
        End If
      Else
        Country = " "
      End If
      Country += r("comp_country")
    End If
    DisplayText += City & State & Country
    Return DisplayText
  End Function

  Public Sub BuildRestOfDealerCharts(ByVal companyID As Long, ByRef searchCriteria As viewSelectionCriteriaClass, ByRef returnString As String, ByVal company_string As String, Optional ByVal ac_dealer_view As Boolean = False)
    If companyID > 0 Then
      Dim TempTable As New DataTable
      Dim ResultsString As String = ""
      Dim MapString As String = ""
      Dim TotalCount As Long = 0
      Dim css As String = ""

      If use_insight_roll = True Then
        TempTable = acdealer_view_function.ac_dealer_get_relationship_sales_main_comp_id(companyID, "", 0, searchCriteria, company_string)
      Else
        TempTable = acdealer_view_function.ac_dealer_get_relationship_sales_main_comp_id(companyID, "", 0, searchCriteria, "")
      End If



      If Not IsNothing(TempTable) Then
        If TempTable.Rows.Count > 0 Then


          MapString = " ['Rel Type', 'Total Count']"
          For Each r As DataRow In TempTable.Rows
            MapString += ", ['" & r("RELTYPE") & "', " & r("numtrans") & "]"
          Next
        End If
      End If

      DisplayFunctions.load_google_chart(New AjaxControlToolkit.TabPanel, MapString, "", "", "chart_div_tab2_all", 0, 0, "ARRAY", 2, returnString, Me.Page, Me.news_tab_update_panel, False, False, False, False, False, False, False, False, True, True, 0, "", "", True)


      TempTable = New DataTable
      TempTable = acdealer_view_function.ac_dealer_sales_by_model(companyID, "", 0, searchCriteria, company_string, ac_dealer_view)


      If Not IsNothing(TempTable) Then

        ResultsString += "<div class=""Box""><div class=""subHeader"">DEALER SALES SINCE " & Year(DateAdd(DateInterval.Year, -1, Now())) & ""

        Dim temp_web As String = ""
        Try
          temp_web = Request.Url.AbsoluteUri
          temp_web = Replace(temp_web, "&ac_dealer_view=N", "")
          temp_web = Replace(temp_web, "&ac_dealer_view=Y", "")

          If ac_dealer_view = True Then
            ResultsString += " - <a href='" & temp_web & "&ac_dealer_view=N'>View by Model</a>"
          Else
            ResultsString += " - <a href='" & temp_web & "&ac_dealer_view=Y'>View by Aircraft</a>"
          End If
        Catch ex As Exception

        End Try
        ResultsString += "</div><br /><table width= ""100%"" cellpadding=""3"" cellspacing=""0"" Class=""formatTable blue"">"
        ResultsString += "<tr Class=""header_row"">"

        If ac_dealer_view = True Then
          ResultsString += "<td align=""left"" valign=""top"" nowrap='nowrap' width='250'>MAKE/MODEL/SERNO</td>"
          ResultsString += "<td align=""right"" valign=""top"" width='90'>TOTAL</td>"
        Else
          ResultsString += "<td align=""left"" valign=""top"" nowrap='nowrap' width='250'>MAKE/MODEL</td>"
          ResultsString += "<td align=""right"" valign=""top"" width='90'>TOTAL</td>"
        End If

        ResultsString += "</tr>"

        If TempTable.Rows.Count > 0 Then
          For Each r As DataRow In TempTable.Rows
            ResultsString += "<tr class=""" & css & """>"


            If ac_dealer_view = True Then
              ResultsString += "<td class=""text_align_left"">"
              ResultsString += Trim(r("amod_make_name")) & " " & r("amod_model_name")

              If Not IsDBNull(r("ac_ser_no_full")) Then
                ResultsString += " - " & r("ac_ser_no_full") & ""
              End If

              ResultsString += "</td>"

              If Not IsDBNull(r("TCOUNT")) Then
                ResultsString += "<td class=""text_align_right"" >" & r("TCOUNT") & "</td>"
                TotalCount += r("TCOUNT")
              Else
                ResultsString += "<td class=""text_align_right"">0</td>"
              End If
            Else
              ResultsString += "<td class=""text_align_left"">"
              ResultsString += Trim(r("amod_make_name")) & " " & r("amod_model_name")
              ResultsString += "</td>"
              If Not IsDBNull(r("TCOUNT")) Then
                ResultsString += "<td class=""text_align_right"" >" & r("TCOUNT") & "</td>"
                TotalCount += r("TCOUNT")
              Else
                ResultsString += "<td class=""text_align_right"">0</td>"
              End If
            End If

            ResultsString += "</tr>"
            If css <> "" Then
              css = ""
            Else
              css = "alt_row"
            End If
          Next
          ResultsString += "<tr class=""header_row""><td align=""left"" valign=""top"">TOTAL</td><td align=""right"" valign=""top"">" & TotalCount.ToString & "</td></tr>"
          ResultsString += "</table>"
          summary_label.Text = ResultsString
        End If
      End If

    End If


  End Sub

  Public Sub use_insight_lease_functions(ByRef company_text As String, ByRef model_text As String)

    Dim sHtmlTopLessors As String = ""
    Dim sHtmlModelsLeasedList As String = ""
    Dim sHtmlLeasesExpired As String = ""
    Dim sHtmlLeasesDueToExpire As String = ""
    Dim sHtmlMarketStatusBlock As String = ""
    Dim sHtmlMostRecentLeaseTrans As String = ""
    Dim sHtmlLeases As String = ""
    Dim HoldTable As New DataTable
    Dim this_section_string As String = ""
    Dim String_to_return As String = ""

    If use_insight_roll = True Then
      lease_functions.views_display_top_lessors(searchCriteria, sHtmlTopLessors, "company", "Y")
      company_text = sHtmlTopLessors
    End If



    If use_insight_roll = True Then
      lease_functions.views_display_models_leased_list(searchCriteria, sHtmlModelsLeasedList, "company", "Y")
    Else
      lease_functions.views_display_models_leased_list(searchCriteria, sHtmlModelsLeasedList, "company", "N")
    End If

    model_text = sHtmlModelsLeasedList



    If use_insight_roll = True Then
      HoldTable = lease_functions.get_leases_by_month(searchCriteria, "Y")
    Else
      HoldTable = lease_functions.get_leases_by_month(searchCriteria, "N")
    End If


    If Not IsNothing(HoldTable) Then
      If HoldTable.Rows.Count > 0 Then
        this_section_string = " ['Lease Month/Year', '# Aircraft']"
        For Each r As DataRow In HoldTable.Rows
          this_section_string &= ", ['" & r("tmonth") & "-" & r("tyear") & "', " & r("tCount") & "]"
        Next
      Else
        relationships_label.Visible = False ' there is no data to graph 
      End If
    Else
      relationships_label.Visible = False ' there is no data to graph 
    End If
    HoldTable.Rows.Clear()

    BuildLeaseSummaryTab(searchCriteria)

    DisplayFunctions.load_google_chart(New AjaxControlToolkit.TabPanel, this_section_string, "Leases Per Moth", "", "chart_div_tab1_all", 0, 0, "ARRAY", 1, String_to_return, Me.Page, Me.relationships_udpate_panel, False, True, True, False, False, False, False, False, True, False, 0, "bottom", "", True)
    Call load_google_chart_all(New AjaxControlToolkit.TabPanel, String_to_return)

    'lease_functions.views_display_leases_expired(searchCriteria, False, sHtmlLeasesExpired)

    'If sHtmlLeasesExpired.ToUpper.Trim.Contains("NO LEASES") Then
    '  lease_functions.views_display_leases_expired(searchCriteria, True, sHtmlLeasesExpired)  
    'End If

    'lease_functions.views_display_leases_due_to_expire(searchCriteria, False, sHtmlLeasesDueToExpire)

    'If sHtmlLeasesDueToExpire.ToUpper.Trim.Contains("NO LEASES") Then
    '  lease_functions.views_display_leases_due_to_expire(searchCriteria, True, sHtmlLeasesDueToExpire) 
    'End If


    'lease_functions.views_display_lease_market_status_block(searchCriteria, sHtmlMarketStatusBlock)

    'If searchCriteria.ViewCriteriaAmodID > -1 Or searchCriteria.ViewCriteriaCompanyID > 0 Then
    '  lease_functions.views_display_leased_aircraft(searchCriteria, sHtmlLeases) 
    'End If

    'lease_functions.views_display_most_recent_lease_trans(searchCriteria, sHtmlMostRecentLeaseTrans) 


  End Sub

  Private Sub BuildLeaseSummaryTab(ByRef searchCriteria As viewSelectionCriteriaClass)
    Dim LeaseTable As New DataTable
    Dim DisplayText As String = ""
    Dim totalCount As Long = 0
    Dim Css As String = ""
    LeaseTable = lease_functions.LeaseSummary(searchCriteria)

    If Not IsNothing(LeaseTable) Then
      If LeaseTable.Rows.Count > 0 Then
        DisplayText = "<div class=""Box""><div class=""subHeader"">Lease Summary</div><table width=""100%"" cellpadding=""3"" cellspacing=""0"" class=""formatTable blue"">"
        DisplayText += "<tr class=""header_row"">"
        DisplayText += "<td align=""left"" valign=""top""><b>Company</b></td>"
        DisplayText += "<td align=""right"" valign=""top""><b>#</b></td>"
        DisplayText += "</tr>"


        For Each r As DataRow In LeaseTable.Rows

          DisplayText += "<tr class=""" & Css & """>"
          DisplayText += "<td align=""left"" valign=""top"">"

          DisplayText += WriteCompAdd(r)
          DisplayText += "</td>"
          DisplayText += "<td align=""right"" valign=""top"">" & r("tcount").ToString & "</td>"
          If IsNumeric(r("tcount")) Then
            totalCount += r("tcount")
          End If

          DisplayText += "</tr>"
          If Css <> "" Then
            Css = ""
          Else
            Css = "alt_row"
          End If
        Next
        DisplayText += "<tr class=""" & Css & """><td align=""left"" valign=""top""><strong>Total</strong></td><td align=""right"" valign=""top""><strong>" & totalCount.ToString & "</strong></td></tr>"
        DisplayText += "</table></div>"
        summary_label.Text = DisplayText
      End If
    End If
  End Sub

  Public Sub use_manu_functions(ByRef company_text As String, ByRef model_text As String)
    Dim htmlOut As New StringBuilder
    Dim sHtmlManufacturerCompanies As String = ""
    Dim sHtmlManufacturerModels As String = ""
    Dim sHtmlManufacturerModelPieChart As String = ""
    Dim sHtmlManufacturerModelPic As String = ""
    Dim sHtmlManufacturerAircraftSummary As String = ""
    Dim sHtmlManufacturerAircraftBarChart As String = ""
    Dim sHtmlManufacturerAircraft As String = ""
    Dim HoldTable As New DataTable
    Dim this_section_string As String = ""
    Dim String_to_return As String = ""


    If searchCriteria.ViewCriteriaCompanyID > 0 Then

      If searchCriteria.ViewCriteriaAmodID = -1 Then

        ' first column left side
        If use_insight_roll = True Then
          manufacturer_functions.views_display_manufacturer_companies(searchCriteria, sHtmlManufacturerCompanies, False, "company", "Y")
        Else
          manufacturer_functions.views_display_manufacturer_companies(searchCriteria, sHtmlManufacturerCompanies, False, "company", "N")
        End If


        company_text = sHtmlManufacturerCompanies

        If use_insight_roll = True Then
          manufacturer_functions.views_display_manufacturer_aircraft_models(searchCriteria, sHtmlManufacturerModels, False, "company", "Y")
        Else
          manufacturer_functions.views_display_manufacturer_aircraft_models(searchCriteria, sHtmlManufacturerModels, False, "company", "N")
        End If

        model_text = sHtmlManufacturerModels


      Else

        ' first column left side
        If use_insight_roll = True Then
          manufacturer_functions.views_display_manufacturer_companies(searchCriteria, sHtmlManufacturerCompanies, False, "company", "Y")
        Else
          manufacturer_functions.views_display_manufacturer_companies(searchCriteria, sHtmlManufacturerCompanies, False, "company", "N")
        End If

        company_text = sHtmlManufacturerCompanies

        'market_functions.views_display_fleet_market_summary(searchCriteria, sHtmlManufacturerAircraftSummary, "")
        '  htmlOut.Append(sHtmlManufacturerAircraftSummary)

        '    manufacturer_functions.views_display_manufacturer_aircraft(searchCriteria, sHtmlManufacturerAircraft)
        '   htmlOut.Append(sHtmlManufacturerAircraft)

      End If

      If use_insight_roll = True Then
        HoldTable = manufacturer_functions.get_manufacturer_by_year(searchCriteria.ViewCriteriaCompanyID, searchCriteria.ViewCriteriaAmodID, "3", searchCriteria, "Y")
      Else
        HoldTable = manufacturer_functions.get_manufacturer_by_year(searchCriteria.ViewCriteriaCompanyID, searchCriteria.ViewCriteriaAmodID, "3", searchCriteria, "N")
      End If


      If Not IsNothing(HoldTable) Then
        If HoldTable.Rows.Count > 0 Then
          this_section_string = " ['Mfr Year', '# Aircraft']"
          For Each r As DataRow In HoldTable.Rows
            this_section_string &= ", ['" & r("ac_mfr_year") & "', " & r("tcount") & "]"
          Next
        End If
      End If
      HoldTable.Rows.Clear()

      DisplayFunctions.load_google_chart(New AjaxControlToolkit.TabPanel, this_section_string, "In Operation Aircraft By MFR Year", "", "chart_div_tab1_all", 0, 0, "ARRAY", 1, String_to_return, Me.Page, Me.relationships_udpate_panel, False, True, True, False, False, False, False, False, True, False, 0, "bottom", "", True)

      this_section_string = ""
      If use_insight_roll = True Then
        HoldTable = manufacturer_functions.get_manufacturer_by_year(searchCriteria.ViewCriteriaCompanyID, searchCriteria.ViewCriteriaAmodID, "1", searchCriteria, "Y")
      Else
        HoldTable = manufacturer_functions.get_manufacturer_by_year(searchCriteria.ViewCriteriaCompanyID, searchCriteria.ViewCriteriaAmodID, "1", searchCriteria, "N")
      End If


      If Not IsNothing(HoldTable) Then
        If HoldTable.Rows.Count > 0 Then
          this_section_string = " ['Mfr Year', '# Aircraft']"
          For Each r As DataRow In HoldTable.Rows
            this_section_string &= ", ['" & r("ac_mfr_year") & "', " & r("tcount") & "]"
          Next
        End If
      End If
      HoldTable.Rows.Clear()

      DisplayFunctions.load_google_chart(New AjaxControlToolkit.TabPanel, this_section_string, "In Production Aircraft By MFR Year", "", "chart_div_tab2_all", 0, 0, "ARRAY", 2, String_to_return, Me.Page, Me.news_tab_update_panel, False, True, True, False, False, False, False, False, True, False, 0, "bottom", "", True)

      Call load_google_chart_all(New AjaxControlToolkit.TabPanel, String_to_return)

    End If

    ' temp_string = htmlOut.ToString


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

      System.Web.UI.ScriptManager.RegisterStartupScript(Me.relationships_udpate_panel, Me.GetType(), "GoogleChart1Tab", GoogleChart1TabScript.ToString, False)
    End If


  End Sub

  Public Sub models_operated(ByVal holdtable As DataTable, ByRef string_return As String, ByVal main_comp_id As Long, ByVal amod_id As Long, ByVal localCriteria As viewSelectionCriteriaClass)
    Dim htmlOut_Make As New StringBuilder
    Dim toggleRowColor As Boolean = False



    If Not IsNothing(holdtable) Then
      If holdtable.Rows.Count > 0 Then
        htmlOut_Make.Append("<div class=""Box""><table id='modelForsaleViewOuterTable3' width=""100%"" cellpadding=""0"" cellspacing=""0""   class='formatTable blue'>")
        htmlOut_Make.Append("<tr class='header_row noBorder'>")

        htmlOut_Make.Append("<th class=""text_align_left"" width='323'><div class=""subHeader"">MODELS REPRESENTED</div><br />")
        htmlOut_Make.Append("</th>")
        If main_comp_id > 0 And amod_id > 0 Then
          'htmlOut_Make.Append("<th class=""text_align_center"" nowrap='nowrap'>&nbsp;</th>")
        Else
          htmlOut_Make.Append("<th class=""right"" nowrap='nowrap'>#AC</th>")
        End If

        htmlOut_Make.Append("</tr>")
        ' htmlOut_Make.Append("</thead>")

        ' ac_rank = 0
        For Each r As DataRow In holdtable.Rows

          If Not toggleRowColor Then
            htmlOut_Make.Append("<tr class=""alt_row"" valign='top'>")
            toggleRowColor = True
          Else
            htmlOut_Make.Append("<tr bgcolor=""white"" valign='top'>")
            toggleRowColor = False
          End If


          htmlOut_Make.Append("<td class=""text_align_left"" nowrap='nowrap' width='323'>")

          '  If amod_id = 0 Then
          '    htmlOut_Make.Append(temp_link & "&amod_id=" & Trim(r("amod_id")) & "&main_comp_id=" & main_comp_id & "&country_name=" & country_name & "&type_drop=" & localCriteria.ViewCriteriaAircraftType & "'>")
          '  End If


          'ac_id, ac_ser_no_full, amod_make_name, amod_model_name, amod_id
          If main_comp_id > 0 And amod_id > 0 Then

            If Not IsDBNull(r("amod_make_name")) And Not IsDBNull(r("amod_model_name")) Then
              htmlOut_Make.Append(Trim(r("amod_make_name")) & " " & Trim(r("amod_model_name")) & "")
            Else
              htmlOut_Make.Append("&nbsp;")
            End If

            If Not IsDBNull(r("ac_ser_no_full")) Then
              htmlOut_Make.Append(" Ser No: ")
              htmlOut_Make.Append(DisplayFunctions.WriteDetailsLink(r("ac_id"), 0, 0, 0, True, Trim(r("ac_ser_no_full")), "date_text", ""))
            End If

            If amod_id = 0 Then
              htmlOut_Make.Append("</a>")
            End If

            htmlOut_Make.Append("</td>")

            ' htmlOut_Make.Append("<td class=""text_align_right"" nowrap='nowrap'>&nbsp;</td>")

          Else

            ' ac_rank = ac_rank + 1
            ' htmlOut_Make.Append("<td class=""text_align_right"" nowrap='nowrap'>" & ac_rank & "</td>") 

            htmlOut_Make.Append("<a href='DisplayCompanyDetail.aspx?compid=" & CompanyID & "&amod_id=" & r("amod_id") & "&use_insight_dealer=Y&use_insight_roll=" & Trim(Request("use_insight_roll")) & "'>")

            If Not IsDBNull(r("amod_make_name")) And Not IsDBNull(r("amod_model_name")) Then
              htmlOut_Make.Append(Trim(r("amod_make_name")) & " " & Trim(r("amod_model_name")) & "")
            Else
              htmlOut_Make.Append("&nbsp;")
            End If

            ' If amod_id = 0 Then
            htmlOut_Make.Append("</a>")
            'End If

            htmlOut_Make.Append("</td>")

            If Not IsDBNull(r("num_ac")) Then
              htmlOut_Make.Append("<td class=""text_align_right"" nowrap='nowrap'>" & r("num_ac") & "</td>")
            Else
              htmlOut_Make.Append("<td class=""text_align_right"" nowrap='nowrap'>0</td>")
            End If

            'htmlOut.Append("<td class=""text_align_center"" nowrap='nowrap'>&nbsp;</td>") 
          End If
          htmlOut_Make.Append("</tr>")

        Next


        ' htmlOut_Make.Append("</table>")

        htmlOut_Make.Append("</table></div>")
      End If

    End If


    '  htmlOut_Make.Append("<div id=""forSaleInnerTable3"" style=""width: 445px;""></div>")
    ' htmlOut_Make.Append("</td></tr>")


    string_return = htmlOut_Make.ToString
  End Sub

  Private Sub CheckAndJSForDatatable()
    'If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CRM Then
    Dim scriptBuOnLoad As String = ""
    Dim ScriptBuPostback As String = ""


    'First we need to check - is this table already initialized?
    'This is important because it doesn't need to be ran twice.

    'scriptBuOnLoad = ScriptBuPostback & "CreateTheDatatable();" ';RedrawDatatablesOnSys();" 
    'ScriptBuPostback += "Sys.Application.add_load(function() {CreateTheDatatable();RedrawDatatablesOnSys();});"

    Call make_script_bu_load_postback("", scriptBuOnLoad, ScriptBuPostback)

    If Trim(Request("viewID")) = "26" Then
      Call make_script_bu_load_postback("2", scriptBuOnLoad, ScriptBuPostback)
      Call make_script_bu_load_postback("3", scriptBuOnLoad, ScriptBuPostback)
      Call make_script_bu_load_postback("4", scriptBuOnLoad, ScriptBuPostback)
    End If

    'Close postback script tag/added code to measure scrollbar/columns for table since tab swap will make the table need it.


    'Added to OnLoad variable on purposes before last close of tag, not needed because the window onLoad is a little different.


    scriptBuOnLoad = "window.onload = function() {" & scriptBuOnLoad & ";};"
    System.Web.UI.ScriptManager.RegisterStartupScript(folders_update_panel, Me.GetType(), "CreateDatatablePostback", ScriptBuPostback.ToString, True)
    System.Web.UI.ScriptManager.RegisterClientScriptBlock(folders_update_panel, Me.GetType, "CreateDatatableOnLoad", scriptBuOnLoad, True)
    'End If

  End Sub

  Public Sub make_script_bu_load_postback(ByVal num_spot As String, ByRef scriptBuOnLoad As String, ByRef ScriptBuPostback As String)

    scriptBuOnLoad = ScriptBuPostback & "CreateTheDatatable" & Trim(num_spot) & "();" ';RedrawDatatablesOnSys();" 
    ScriptBuPostback += "Sys.Application.add_load(function() {CreateTheDatatable" & Trim(num_spot) & "();RedrawDatatablesOnSys" & Trim(num_spot) & "();});"

  End Sub

  Private Sub Fill_Yacht_Tab(ByVal order_by As String)
    Dim YachtTable As New DataTable
    Dim last_yacht_name_type As String = ""

    If Trim(CRMSource) = "CLIENT" And CRMJetnetID = 0 Then
      ' then do not look up for all jetnet ids 0
    Else
      yachtContainer.Visible = True
      YachtTable = masterPage.aclsData_Temp.DisplayYachtForGivenCompanyByCompanyID(IIf(CRMSource <> "CLIENT", CompanyID, CRMJetnetID), "", JournalID, order_by)
      YachtTable = FixYachtTableRemoveDuplicates(YachtTable)

      If Not IsNothing(YachtTable) Then
        If YachtTable.Rows.Count > 0 Then

          Session.Item("COMPANY_HAS_YACHTS") = "Y"

          YachtDataGrid.DataSource = YachtTable
          YachtDataGrid.DataBind()

          yachtsHeader.Text = "Yachts <em class='tiny_text'>(" & YachtTable.Rows.Count & " relationship" & IIf(YachtTable.Rows.Count = 1, "", "s") & ")</em>"

        Else
          Session.Item("COMPANY_HAS_YACHTS") = "N"
          yacht_label.Text += "<p align='center'>No Yachts Found.</p>"
          yacht_label.ForeColor = Drawing.Color.Red
          yacht_label.Font.Bold = True
        End If
      Else
        Session.Item("COMPANY_HAS_YACHTS") = "N"
        If masterPage.aclsData_Temp.class_error <> "" Then
          masterPage.LogError("CompanyTabs.ascx.vb -Fill_Yacht_Tab() - " & masterPage.aclsData_Temp.class_error)
        End If
      End If

      YachtTable.Dispose()

    End If
  End Sub

  Public Sub Fill_Share_Relationship_Tab()
    Dim ShareTable As New DataTable
    ShareTable = masterPage.aclsData_Temp.Return_Share_Relationships(IIf(CRMSource <> "CLIENT", CompanyID, CRMJetnetID), JournalID)
    If Not IsNothing(ShareTable) Then
      If ShareTable.Rows.Count > 0 Then
        If JournalID = 0 Then
          view_share_relationships.Visible = True
        End If
      Else
        shareContainer.Visible = False
      End If
    End If
  End Sub

  Private Sub HelperShareRelationship(ByVal ShareTable As DataTable)
    share_label.Text = "<table width='100%' cellspacing='3' cellpadding='3' class='data_aircraft_grid'>"
    share_label.Text += "<tr class='header_row'>"
    share_label.Text += "<td align='left' valign='top'><b class='title'>Aircraft</b></td><td align='left' valign='top'>"
    share_label.Text += "<b class='title'>Status</b></td><td align='left' valign='top'><b class='title'>Fractional Owner</b></td>"
    share_label.Text += "<td align='left' valign='top'><b class='title'>Relationship</b></td></tr>"
    Dim x As Integer = 0
    For Each r As DataRow In ShareTable.Rows
      '   If x < 10 Then
      share_label.Text += "<tr>"
      share_label.Text += "<td align='left' valign='top'>" & r("ac_year").ToString & " " & r("amod_make_name").ToString
      share_label.Text += r("amod_model_name").ToString & "<br />Ser #: " & DisplayFunctions.WriteDetailsLink(r("ac_id").ToString, 0, 0, JournalID, True, r("ac_ser_no_full").ToString, "", "") & " Reg #:" & r("ac_reg_no").ToString & "</td>"
      share_label.Text += "<td align='left' valign='top'>"
      If r("ac_forsale_flag").ToString = "Y" Then
        share_label.Text += "<span class='light_green_background padding_text'>"
      Else
        share_label.Text += "<span>"
      End If
      share_label.Text += r("ac_status").ToString

      share_label.Text += "</span>"
      share_label.Text += "</td>"
      share_label.Text += "<td align='left' valign='top'>" & DisplayFunctions.WriteDetailsLink(0, CompanyID, 0, JournalID, True, r("comp_name").ToString, "", "") & " " & r("cref_owner_percent").ToString & "%<br />"
      share_label.Text += "<span class=""tiny"">" & r("comp_city").ToString & " " & r("comp_state").ToString & " " & r("comp_zip_code").ToString & " " & r("comp_country") & "</span></td>"
      share_label.Text += "<td align='left' valign='top'>" & r("actype_name").ToString & "</td></tr>"
      'End If
      '  x = x + 1
    Next

    share_label.Text += "</table>"
  End Sub

  Public Sub Fill_Business_Type_Tab()
    Dim BusinessType As New DataTable
    Dim DividedNumber As Integer = 0
    Dim Count As Integer = 0
    BusinessType = masterPage.aclsData_Temp.Return_Business_Type(IIf(CRMSource <> "CLIENT", CompanyID, CRMJetnetID), JournalID)
    business_label.Text = "<div class=""Box"">"


    If Not IsNothing(BusinessType) Then
      If BusinessType.Rows.Count > 0 Then
        business_label.Text += "<div class=""subHeader"">Business Type(s)</div><br /><table class=""formatTable blue"" cellpadding=""0"" cellspacing=""0"">"
        'If BusinessType.Rows.Count > 2 Then
        '  DividedNumber = Math.Round(BusinessType.Rows.Count / 2)
        'Else
        '  DividedNumber = 1
        'End If
        For Each z As DataRow In BusinessType.Rows
          If Count = 2 Then
            business_label.Text += "</tr>"
            business_label.Text += "<tr>"
            Count = 0
          End If
          business_label.Text += "<td>" & IIf(Not IsDBNull(z("cbus_name")), " " & z("cbus_name") & "", "") & "</td>"
          Count = Count + 1
        Next
        business_label.Text += "</tr></table>"
      End If
    Else
      If masterPage.aclsData_Temp.class_error <> "" Then
        masterPage.LogError("DisplayCompanyDetail.aspx.vb - Fill_Business_Type_Tab() - " & masterPage.aclsData_Temp.class_error)
      End If
    End If
    BusinessType.Dispose()


    business_label.Text += "</div>"
    'Response.Write(business_label.Text)
  End Sub

  Public Sub Fill_Certifications_Tab(ByVal cert_category As String)
    Dim CertifactionsTable As New DataTable
    Dim count As Integer = 0

    CertifactionsTable = masterPage.aclsData_Temp.Return_Certifications(IIf(CRMSource <> "CLIENT", CompanyID, CRMJetnetID), JournalID, cert_category)
    If Not IsNothing(CertifactionsTable) Then
      If (CertifactionsTable.Rows.Count > 0) Then

        certifications_label.Visible = True

        If Trim(cert_category) = "Certificate" Then
          certifications_label.Text += "<div class=""Box""><div class=""subHeader"">Operating Certification(s): "
        ElseIf Trim(cert_category) = "Membership" Then
          certifications_label.Text += "<div class=""Box""><div class=""subHeader"">Membership(s): "
        ElseIf Trim(cert_category) = "Accreditation" Then
          certifications_label.Text += "<div class=""Box""><div class=""subHeader"">Accreditation(s): "
        Else
          certifications_label.Text += "<div class=""Box""><div class=""subHeader"">Operating Certification(s): "
        End If


        certifications_label.Text += "<a href=""/help/documents/650.pdf"""
        certifications_label.Text += "target=""new"">"
        certifications_label.Text += "<img alt=""View Operator Certification Decriptions"" border=""0"" class=""float_right padding_left"""
        certifications_label.Text += "src=""images/info.png"" width=""15"" /></a></div><br />"

        For Each q As DataRow In CertifactionsTable.Rows
          'If count = 3 Then
          '  certifications_label.Text += "</div><div class=""padding row"">"
          '  count = 0
          'End If


          ' changed from height 98 to width 125 - MSW - 2/11/20
          certifications_label.Text += "<span class=""float_left padding text_align_center""><img src=""images/" & q("ccerttype_logo_image").ToString & """ alt=""" & q("ccerttype_type").ToString & """ width='100' /></span>"



          count = count + 1
        Next
        'certifications_label.Text += "</div>"
        certifications_label.Text += "<div class=""clearfix""></div></div>"
      End If
      'Response.Write(certifications_label.Text)
    Else
      If masterPage.aclsData_Temp.class_error <> "" Then
        masterPage.LogError("CompanyTabs.ascx.vb -Fill_Certifications() - " & masterPage.aclsData_Temp.class_error)
      End If
    End If
  End Sub

  Public Sub Fill_Sales_Price_Submitted_On_Tab()
    Dim service_table As New DataTable
    Dim htmlOut As New StringBuilder
    Dim count As Integer = 0
    Dim toggleRowColor As Boolean = False
    Dim temp_total_contact As Long = 0
    Dim temp_total_prices As Long = 0
    Dim temp_total_est As Long = 0


    If use_insight_roll = True Then
      service_table = GET_Submitted_Data(CompanyID, "Y", "Y")
    Else
      service_table = GET_Submitted_Data(CompanyID, "N", "Y")
    End If


    If Not IsNothing(service_table) Then

      htmlOut.Append("<table id='modelForsaleViewOuterTable' width=""100%"" cellpadding=""0"" cellspacing=""0""class='formatTable blue small'>")
      htmlOut.Append("<tr class='header_row'>")
      ' htmlOut.Append("<td align='left'><b>COMPANY&nbsp;</b></td>")
      htmlOut.Append("<td align='right'><b>MONTH/YEAR SUBMITTED&nbsp;</b></td>")
      htmlOut.Append("<td align='right'><b>SALE PRICES&nbsp;</b></td>")
      htmlOut.Append("<td align='right'><b>VALUE ESTIMATES&nbsp;</b></td>")
      htmlOut.Append("</tr>")

      If (service_table.Rows.Count > 0) Then

        For Each q As DataRow In service_table.Rows

          If Not toggleRowColor Then
            htmlOut.Append("<tr class=""alt_row"" valign='top'>")
            toggleRowColor = True
          Else
            htmlOut.Append("<tr bgcolor=""white"" valign='top'>")
            toggleRowColor = False
          End If

          '   htmlOut.Append("<td align='left'>" & q("COMPNAME").ToString & "&nbsp;</td>")
          htmlOut.Append("<td align='right'>" & q("CALMONTH").ToString & "/" & q("CALYEAR").ToString & "&nbsp;</td>")

          If Not IsDBNull(q("PRICES")) Then
            temp_total_prices = temp_total_prices + CDbl(q("PRICES"))
            htmlOut.Append("<td align='right'>" & q("PRICES").ToString & "&nbsp;</td>")
          Else
            htmlOut.Append("<td align='right'>0&nbsp;</td>")
          End If

          If Not IsDBNull(q("ESTIMATES")) Then
            temp_total_est = temp_total_est + CDbl(q("ESTIMATES"))
            htmlOut.Append("<td align='right'>" & q("ESTIMATES").ToString & "&nbsp;</td>")
          Else
            htmlOut.Append("<td align='right'>0&nbsp;</td>")
          End If

          htmlOut.Append("</tr>")

          count = count + 1
        Next
      Else
        certifications_label.Visible = False
      End If

      'If Not toggleRowColor Then
      '  htmlOut.Append("<tr class=""alt_row"" valign='top'>")
      '  toggleRowColor = True
      'Else
      '  htmlOut.Append("<tr bgcolor=""white"" valign='top'>")
      '  toggleRowColor = False
      'End If

      'htmlOut.Append("<td align='right'><b>Totals:&nbsp;</b></td>") 
      'htmlOut.Append("<td align='right'><b>" & temp_total_prices & ":&nbsp;</b></td>")
      'htmlOut.Append("<td align='right'><b>" & temp_total_est & "&nbsp;</b></td>")

      'htmlOut.Append("</tr>")


      htmlOut.Append("</table>")

      Me.submitted_label.Text &= htmlOut.ToString
    Else
      If masterPage.aclsData_Temp.class_error <> "" Then
        masterPage.LogError("CompanyTabs.ascx.vb -Fill_Sales_Price_Submitted_On_Tab() - " & masterPage.aclsData_Temp.class_error)
      End If
    End If
  End Sub


  Public Function get_latest_marketing_notes_top(ByVal comp_id As Long) As String
    get_latest_marketing_notes_top = ""
    Dim temp_table As New DataTable

    Try

      temp_table = get_latest_marketing_notes(comp_id)


      If Not IsNothing(temp_table) Then
        If temp_table.Rows.Count > 0 Then
          get_latest_marketing_notes_top &= "<ul class=""remove_margin remove_padding"">"
          For Each q As DataRow In temp_table.Rows

            If Not IsDBNull(q("compdoc_description")) Then
              get_latest_marketing_notes_top &= "<li>"
              If Not IsDBNull(q("compdoc_filename")) Then
                get_latest_marketing_notes_top &= "<a href='http://jetnet4/contracts/" & q("compdoc_filename") & "' target='_blank'>View</a> - "
              End If
              get_latest_marketing_notes_top &= q("compdoc_description")
              get_latest_marketing_notes_top &= "</li>"
            End If

          Next
          get_latest_marketing_notes_top &= "</ul>"
        End If
      End If

    Catch ex As Exception

    End Try

  End Function


  Public Sub Fill_Sales_Price_Submissions_Tab(ByRef SalesSubmissions As Boolean)
    Dim service_table As New DataTable
    Dim htmlOut As New StringBuilder
    Dim count As Integer = 0
    Dim toggleRowColor As Boolean = False
    Dim temp_total_contact As Long = 0
    Dim temp_total_prices As Long = 0
    Dim temp_total_est As Long = 0


    If use_insight_roll = True Then
      service_table = GET_Submitted_Data(CompanyID, "Y", "N")
    Else
      service_table = GET_Submitted_Data(CompanyID, "N", "N")
    End If

    If Not IsNothing(service_table) Then

      htmlOut.Append("<table id='modelForsaleViewOuterTable' width=""100%"" cellpadding=""0"" cellspacing=""0""class='formatTable blue small'>")
      htmlOut.Append("<tr class='header_row'>")
      ' htmlOut.Append("<td align='left'><b>COMPANY&nbsp;</b></td>")
      htmlOut.Append("<td align='right'><b>MONTH/YEAR SALE&nbsp;</b></td>")
      htmlOut.Append("<td align='right'><b>SALE PRICES&nbsp;</b></td>")
      htmlOut.Append("<td align='right'><b>VALUE ESTIMATES&nbsp;</b></td>")
      htmlOut.Append("</tr>")

      If (service_table.Rows.Count > 0) Then
        SalesSubmissions = True
        For Each q As DataRow In service_table.Rows

          If Not toggleRowColor Then
            htmlOut.Append("<tr class=""alt_row"" valign='top'>")
            toggleRowColor = True
          Else
            htmlOut.Append("<tr bgcolor=""white"" valign='top'>")
            toggleRowColor = False
          End If

          '  htmlOut.Append("<td align='left'>" & q("COMPNAME").ToString & "&nbsp;</td>")
          htmlOut.Append("<td align='right'>" & q("CALMONTH").ToString & "/" & q("CALYEAR").ToString & "&nbsp;</td>")

          If Not IsDBNull(q("PRICES")) Then
            temp_total_prices = temp_total_prices + CDbl(q("PRICES"))
            htmlOut.Append("<td align='right'>" & q("PRICES").ToString & "&nbsp;</td>")
          Else
            htmlOut.Append("<td align='right'>0&nbsp;</td>")
          End If

          If Not IsDBNull(q("ESTIMATES")) Then
            temp_total_est = temp_total_est + CDbl(q("ESTIMATES"))
            htmlOut.Append("<td align='right'>" & q("ESTIMATES").ToString & "&nbsp;</td>")
          Else
            htmlOut.Append("<td align='right'>0&nbsp;</td>")
          End If

          htmlOut.Append("</tr>")

          count = count + 1
        Next
      Else
        certifications_label.Visible = False
      End If

      If Not toggleRowColor Then
        htmlOut.Append("<tr class=""alt_row"" valign='top'>")
        toggleRowColor = True
      Else
        htmlOut.Append("<tr bgcolor=""white"" valign='top'>")
        toggleRowColor = False
      End If

      htmlOut.Append("<td align='right'><b>Totals:&nbsp;</b></td>")
      htmlOut.Append("<td align='right'><b>" & temp_total_prices & ":&nbsp;</b></td>")
      htmlOut.Append("<td align='right'><b>" & temp_total_est & "&nbsp;</b></td>")

      htmlOut.Append("</tr>")


      htmlOut.Append("</table>")

      Me.submitted_label.Text &= htmlOut.ToString
    Else
      If masterPage.aclsData_Temp.class_error <> "" Then
        masterPage.LogError("CompanyTabs.ascx.vb -Fill_Sales_Price_Submissions_Tab() - " & masterPage.aclsData_Temp.class_error)
      End If
    End If
  End Sub
  Public Sub Fill_Admin_Action_Items()
    Dim actionDataTable As New DataTable

    certifications_label.Visible = True
    Dim temp_desc As String = ""

    Dim helperClass As New displayCompanyDetailsFunctions
    actionDataTable = helperClass.Get_ActionItems_Query(CompanyID, 0)


    If (Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER) Or (Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE) Then
      action_add_new_Admin.Visible = True
      actionPanel_Admin_Top.Visible = True
      actionPanel_Admin.Visible = True
      action_label_Admin.Visible = True
      closeNotes_Admin.Visible = False

      actionPanel.Visible = False
    Else
      action_add_new.Visible = True
      closeNotes.Visible = False
      actionPanel.Visible = True
      action_label.Visible = True 'visible no matter
    End If

    If Not IsNothing(actionDataTable) Then
      action_add_new.Visible = True
      action_add_new.Text = "<a href='#' class=""float_right"" onclick=""javascript:load('/adminActions.aspx?task=add&journid=&companyid=" & CompanyID & "&contactid=','','scrollbars=yes,menubar=no,height=900,width=1350,resizable=yes,toolbar=no,location=no,status=no');return false;"">ADD NEW</a>"

      Me.action_label.Text = helperClass.ReturnActionItemsDisplayTable(actionDataTable, CompanyID, 0)


      Me.action_add_new_Admin.Text = Me.action_add_new.Text

      If (Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER) Or (Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE) Then
        Me.action_label_Admin.Text = Me.action_label.Text
        Me.action_add_new_Admin.Visible = True
      End If
    Else
      If masterPage.aclsData_Temp.class_error <> "" Then
        masterPage.LogError("DisplayCompanyDetails.aspx.vb -FillAdminActionItems() - " & masterPage.aclsData_Temp.class_error)
      End If
    End If
  End Sub


  Public Sub Fill_Trials_Summary_Tab()
    Dim service_table As New DataTable
    Dim htmlOut As New StringBuilder
    Dim temp_total_contact As Long = 0
    trial_label.Visible = True

    Dim toggleAllSubs As Boolean = False

    Dim helperClass As New displayCompanyDetailsFunctions

    If sTask.ToLower.Contains("inactive") Then
      toggleAllSubs = True
      trails_link_button_active.Visible = True
      trials_link_button_all.Visible = False
    Else
      toggleAllSubs = False
      trials_link_button_all.Visible = True
      trails_link_button_active.Visible = False
    End If

    'no roll up currently 
    '  If use_insight_roll = True Then
    '  service_table = helperClass.Return_Trial_Summary(CompanyID, JournalID, "Y", toggleAllSubs)
    '  Else
    service_table = helperClass.Return_Trial_Summary(CompanyID, JournalID, "N", toggleAllSubs)
    '  End If


    If Not IsNothing(service_table) Then

      htmlOut.Append("<table id='serviceTable' width=""100%"" cellpadding=""0"" cellspacing=""0"" class=""formatTable blue small aircraftTable"">")
      htmlOut.Append("<tr class='header_row'>")
      htmlOut.Append("<td align='left'><b>SERVICE</b></td>")
      htmlOut.Append("<td align='right'><b>NAME</b></td>")
      htmlOut.Append("<td align='right'><b>PASSWORD</b></td>")
      htmlOut.Append("<td align='right'><b>STATUS</b></td>")
      htmlOut.Append("<td align='right'><b>USERID</b></td>")
      htmlOut.Append("<td align='right'><b>INSTALL</b></td>")

      htmlOut.Append("</tr>")

      If (service_table.Rows.Count > 0) Then


        For Each q As DataRow In service_table.Rows

          htmlOut.Append("<tr bgcolor=""white"" valign='top'>")

          htmlOut.Append("<td align='left' colspan='6'>" & q("SERVICE").ToString & "</td>")

          htmlOut.Append("</tr>")

          htmlOut.Append("<tr><td align='right'>&nbsp;</td><td align='right'>")

          If Not IsDBNull(q("NAME")) Then
            If Not String.IsNullOrEmpty(q("NAME").ToString.Trim) Then
              If Not IsDBNull(q("contact_email_address")) Then
                htmlOut.Append("<a class=""underline"" onclick='javascript:openSmallWindowJS(""adminSubErrors.aspx?email=" & q("contact_email_address") & "&sub_id=" & q("sub_id") & "&login=" & q("sublogin_login") & """, ""Display Trial Actions"");' title='Display Trial Actions'>")
              End If
              htmlOut.Append(q("NAME").ToString)
              If Not IsDBNull(q("contact_email_address")) Then
                htmlOut.Append("</a>")
              End If
            End If
          End If

          htmlOut.Append("</td>")

          htmlOut.Append("<td align='right'>")

          If Not IsDBNull(q("PASSWORD")) Then
            If Not String.IsNullOrEmpty(q("PASSWORD").ToString.Trim) Then
              htmlOut.Append(q("PASSWORD").ToString)
            End If
          End If

          htmlOut.Append("</td>")

          htmlOut.Append("<td align='right'>")

          If Not IsDBNull(q("STATUS")) Then
            If Not String.IsNullOrEmpty(q("STATUS").ToString.Trim) Then
              htmlOut.Append(q("STATUS").ToString)
            End If
          End If

          htmlOut.Append("</td>")

          htmlOut.Append("<td align='right'>")

          If Not IsDBNull(q("USERID")) Then
            If Not String.IsNullOrEmpty(q("USERID").ToString.Trim) Then
              htmlOut.Append(q("USERID").ToString)
            End If
          End If

          htmlOut.Append("</td>")

          htmlOut.Append("<td align='right'>")

          If Not IsDBNull(q("INSTALL")) Then
            If Not String.IsNullOrEmpty(q("INSTALL").ToString.Trim) Then
              htmlOut.Append(q("INSTALL").ToString)
            End If
          End If

          htmlOut.Append("</td>")

          htmlOut.Append("</tr>")

        Next
      Else
        trial_label.Visible = False
      End If

      htmlOut.Append("</table>")


      Me.trial_label.Text = htmlOut.ToString
    Else
      If masterPage.aclsData_Temp.class_error <> "" Then
        masterPage.LogError("CompanyTabs.ascx.vb -Fill_Service_Summary_Tab() - " & masterPage.aclsData_Temp.class_error)
      End If
    End If
  End Sub



  Public Sub Fill_Service_Summary_Tab(ByRef ShowSubscriptionSummary As Boolean)
    Dim service_table As New DataTable
    Dim htmlOut As New StringBuilder
    Dim temp_total_contact As Long = 0
    services_label.Visible = True

    Dim toggleAllSubs As Boolean = False

    Dim helperClass As New displayCompanyDetailsFunctions

    If sTask.ToLower.Contains("inactive") Then
      toggleAllSubs = True
      activeServices.Visible = True
      inactiveServices.Visible = False
    Else
      toggleAllSubs = False
      inactiveServices.Visible = True
      activeServices.Visible = False
    End If

    If use_insight_roll = True Then
      service_table = helperClass.Return_Service_Summary(CompanyID, JournalID, "Y", toggleAllSubs)
    Else
      service_table = helperClass.Return_Service_Summary(CompanyID, JournalID, "N", toggleAllSubs)
    End If


    If Not IsNothing(service_table) Then

      htmlOut.Append("<table id='serviceTable' width=""100%"" cellpadding=""0"" cellspacing=""0"" class=""formatTable blue small aircraftTable"">")
      htmlOut.Append("<tr class='header_row'>")
      htmlOut.Append("<td align='left'><b>SERVICE</b></td>")
      htmlOut.Append("<td align='right'><b>AMT</b></td>")
      htmlOut.Append("<td align='right'><b>#LIC</b></td>")
      htmlOut.Append("<td align='right'><b>#SUB</b></td>")
      htmlOut.Append("<td align='right'><b>#LOC</b></td>")
      htmlOut.Append("<td align='right'><b>DATE(S)</b></td>")

      htmlOut.Append("</tr>")

      If (service_table.Rows.Count > 0) Then
        ShowSubscriptionSummary = True
        For Each q As DataRow In service_table.Rows

          If toggleAllSubs Then

            If Not IsDBNull(q("sub_end_date")) Then
              If CDate(q("sub_end_date").ToString) <= Today Then
                htmlOut.Append("<tr class=""alt_row"" valign='top'>")
              Else
                htmlOut.Append("<tr bgcolor=""white"" valign='top'>")
              End If
            Else
              If IsDBNull(q("sub_start_date")) Then
                htmlOut.Append("<tr class=""alt_row"" valign='top'>")
              Else
                htmlOut.Append("<tr bgcolor=""white"" valign='top'>")
              End If
            End If
          Else
            htmlOut.Append("<tr bgcolor=""white"" valign='top'>")
          End If

          htmlOut.Append("<td align='left'>" + q("sub_service_name").ToString.Replace(" (", "<br />(").Trim + "</td>")

          If Not IsDBNull(q("tsum")) Then
            temp_total_contact = CDbl(CDbl(q("tsum")) + CDbl(q("CONAMT")))
            htmlOut.Append("<td align='right'>" + FormatNumber(temp_total_contact.ToString, 0, False, False, True) + "</td>")
          Else
            htmlOut.Append("<td align='right'>" + FormatNumber(q("CONAMT").ToString, 0, False, False, True) + "</td>")
          End If

          htmlOut.Append("<td align='right'>" + q("LICENSES").ToString + "</td>")
          htmlOut.Append("<td align='right'>" + q("SUBSCRIPTIONS").ToString + "</td>")
          htmlOut.Append("<td align='right'>" + q("LOCATIONS").ToString + "</td>")

          htmlOut.Append("<td align='right'>")

          If Not IsDBNull(q("sub_start_date")) Then
            If Not String.IsNullOrEmpty(q("sub_start_date").ToString.Trim) Then
              htmlOut.Append(FormatDateTime(q("sub_start_date").ToString, DateFormat.ShortDate))
            End If
          End If

          If Not IsDBNull(q("sub_end_date")) Then
            If Not String.IsNullOrEmpty(q("sub_end_date").ToString.Trim) Then
              htmlOut.Append("&nbsp;-&nbsp;" + FormatDateTime(q("sub_end_date").ToString, DateFormat.ShortDate))
            End If
          End If

          htmlOut.Append("</td>")

          htmlOut.Append("</tr>")

        Next
      Else
        services_label.Visible = False
      End If

      htmlOut.Append("</table>")

      'We need to validate that there are no accounting errors on the company.
      service_table = New DataTable
      service_table = helperClass.Get_AccountingIssues(CompanyID)
      If Not IsNothing(service_table) Then
        If service_table.Rows.Count > 0 Then
          If Not IsNothing(service_table.Rows(0).Item("journ_description")) Then
            If Not IsDBNull(service_table.Rows(0).Item("journ_description")) Then
              If Not String.IsNullOrEmpty(service_table.Rows(0).Item("journ_description")) Then
                htmlOut.Append("<p class=""red_text padding_top text_align_center remove_margin"">")

                htmlOut.Append(service_table.Rows(0).Item("journ_description"))

                htmlOut.Append("<br />Entered by ")
                If Not IsNothing(service_table.Rows(0).Item("user_first_name")) Then
                  If Not IsDBNull(service_table.Rows(0).Item("user_first_name")) Then
                    htmlOut.Append(service_table.Rows(0).Item("user_first_name"))
                  End If
                End If
                If Not IsNothing(service_table.Rows(0).Item("user_last_name")) Then
                  If Not IsDBNull(service_table.Rows(0).Item("user_last_name")) Then
                    htmlOut.Append(" " & service_table.Rows(0).Item("user_last_name"))
                  End If
                End If

                If Not IsNothing(service_table.Rows(0).Item("journ_date")) Then
                  If Not IsDBNull(service_table.Rows(0).Item("journ_date")) Then
                    htmlOut.Append(" on " & clsGeneral.clsGeneral.TwoPlaceYear(service_table.Rows(0).Item("journ_date")))
                  End If
                End If

                htmlOut.Append("</p>")

                services_label.Visible = True
              End If
            End If
          End If
        End If
      End If

      'We need to check for a prospect type of DO NOT MARKET.
      service_table = New DataTable
      service_table = helperClass.Get_DONOTMARKET(CompanyID)
      If Not IsNothing(service_table) Then
        If service_table.Rows.Count > 0 Then
          services_label.Visible = True
          htmlOut.Append("<p class=""red_text padding_top text_align_center remove_margin"">DO NOT MARKET.</p>")
        End If
      End If

      Me.services_label.Text = htmlOut.ToString
    Else
      If masterPage.aclsData_Temp.class_error <> "" Then
        masterPage.LogError("CompanyTabs.ascx.vb -Fill_Service_Summary_Tab() - " & masterPage.aclsData_Temp.class_error)
      End If
    End If
  End Sub

  Public Sub Fill_Subscription_Summary_Tab()
    Dim service_table As New DataTable
    Dim htmlOut As New StringBuilder
    subscription_label.Visible = True
    Dim temp_desc As String = ""

    Dim toggleAllSubs As Boolean = False
    Dim helperClass As New displayCompanyDetailsFunctions

    If sTask.ToLower.Contains("inactive") Then
      toggleAllSubs = True
      activeSub.Visible = True
      inactiveSub.Visible = False
    Else
      toggleAllSubs = False
      activeSub.Visible = False
      inactiveSub.Visible = True
    End If

    If use_insight_roll = True Then
      service_table = helperClass.Return_Subscription_Summary(CompanyID, JournalID, "Y", toggleAllSubs)
    Else
      service_table = helperClass.Return_Subscription_Summary(CompanyID, JournalID, "N", toggleAllSubs)
    End If


    If Not IsNothing(service_table) Then

      htmlOut.Append("<table id='subscriptionTable' width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" class=""formatTable blue small aircraftTable"">")
      htmlOut.Append("<tr class='header_row'>")
      htmlOut.Append("<td align='left'><b>SERVICE</b></td>")
      htmlOut.Append("<td align='right'><b>SUB&nbsp;ID</b></td>")
      htmlOut.Append("<td align='right'><b>AMT</b></td>")
      htmlOut.Append("<td align='right'><b>#LIC/#USERS</b></td>")
      htmlOut.Append("<td align='right'><b>#VALUES/#USERS</b></td>")
      htmlOut.Append("<td align='right'><b>DATE(S)</b></td>")

      htmlOut.Append("</tr>")

      If (service_table.Rows.Count > 0) Then

        For Each q As DataRow In service_table.Rows
          Dim LicenseCount As Long = 0
          If Not q("SUBNAME").ToString.ToLower.Contains(temp_desc.ToLower.Trim) Or String.IsNullOrEmpty(temp_desc.Trim) Then

            If toggleAllSubs Then
              If Not IsDBNull(q("sub_end_date")) Then
                If CDate(q("sub_end_date").ToString) <= Today Then
                  htmlOut.Append("<tr class=""alt_row"" valign='top'>")
                Else
                  htmlOut.Append("<tr bgcolor=""white"" valign='top'>")
                End If
              Else
                If IsDBNull(q("sub_start_date")) Then
                  htmlOut.Append("<tr class=""alt_row"" valign='top'>")
                Else
                  htmlOut.Append("<tr bgcolor=""white"" valign='top'>")
                End If
              End If
            Else
              htmlOut.Append("<tr bgcolor=""white"" valign='top'>")
            End If

            htmlOut.Append("<td align='left' colspan='6'>" + q("SUBNAME").ToString.Trim + "</td>")
            htmlOut.Append("</tr>")

            If toggleAllSubs Then
              If Not IsDBNull(q("sub_end_date")) Then
                If CDate(q("sub_end_date").ToString) <= Today Then
                  htmlOut.Append("<tr class=""alt_row"" valign='top'>")
                Else
                  htmlOut.Append("<tr bgcolor=""white"" valign='top'>")
                End If
              Else
                If IsDBNull(q("sub_start_date")) Then
                  htmlOut.Append("<tr class=""alt_row"" valign='top'>")
                Else
                  htmlOut.Append("<tr bgcolor=""white"" valign='top'>")
                End If
              End If
            Else
              htmlOut.Append("<tr bgcolor=""white"" valign='top'>")
            End If

          Else

            If toggleAllSubs Then
              If Not IsDBNull(q("sub_end_date")) Then
                If CDate(q("sub_end_date").ToString) <= Today Then
                  htmlOut.Append("<tr class=""alt_row"" valign='top'>")
                Else
                  htmlOut.Append("<tr bgcolor=""white"" valign='top'>")
                End If
              Else
                If IsDBNull(q("sub_start_date")) Then
                  htmlOut.Append("<tr class=""alt_row"" valign='top'>")
                Else
                  htmlOut.Append("<tr bgcolor=""white"" valign='top'>")
                End If
              End If
            Else
              htmlOut.Append("<tr bgcolor=""white"" valign='top'>")
            End If
          End If

          htmlOut.Append("<td align='right'></td>")

          htmlOut.Append("<td align='right'><a class=""underline"" onclick='javascript:openSmallWindowJS(""homebaseSubscription.aspx?compID=" + CompanyID.ToString + "&subID=" + q("sub_id").ToString + """,""SubscriptionWindow"");' title='Display Subscription Details'>" + q("sub_id").ToString + "</a></td>")
          htmlOut.Append("<td align='right'>" + FormatNumber(q("CONAMT").ToString, 0, False, False, True) + "</td>")

          htmlOut.Append("<td align='right'>")

          If Not IsDBNull(q("LICENSES")) Then
            LicenseCount = CInt(q("LICENSES"))
            htmlOut.Append(q("LICENSES").ToString)
          Else
            htmlOut.Append("")
          End If

          If Not IsDBNull(q.Item("USERS")) Then
            If CInt(q.Item("USERS")) > 0 Then
              If LicenseCount = q.Item("USERS") Then
                htmlOut.Append("/" & q("USERS").ToString)
              Else
                htmlOut.Append("/<span class=""red_text"">" & q("USERS").ToString & "</span>")
              End If

            End If

          End If
          htmlOut.Append("</td>")

          htmlOut.Append("<td align='right'>")
          LicenseCount = 0
          If Not IsDBNull(q("VLICENSES")) Then
            LicenseCount = CInt(q("VLICENSES"))
            htmlOut.Append(q("VLICENSES").ToString)
          Else
            htmlOut.Append("")
          End If

          If Not IsDBNull(q.Item("VUSERS")) Then
            ' If CInt(q.Item("VUSERS")) > 0 Then
            If LicenseCount = q.Item("VUSERS") Then
              htmlOut.Append("/" & q("VUSERS").ToString)
            Else
              htmlOut.Append("/<span class=""red_text"">" & q("VUSERS").ToString & "</span>")
            End If

            ' End If

          End If
          htmlOut.Append("</td>")

          htmlOut.Append("<td align='right'>")

          If Not IsDBNull(q("sub_start_date")) Then
            If Not String.IsNullOrEmpty(q("sub_start_date").ToString.Trim) Then
              htmlOut.Append(FormatDateTime(q("sub_start_date").ToString, DateFormat.ShortDate))
            End If
          End If

          If Not IsDBNull(q("sub_end_date")) Then
            If Not String.IsNullOrEmpty(q("sub_end_date").ToString.Trim) Then
              htmlOut.Append("&nbsp;-&nbsp;" + FormatDateTime(q("sub_end_date").ToString, DateFormat.ShortDate))
            End If
          End If

          htmlOut.Append("</td>")

          htmlOut.Append("</tr>")

          '---------------- ADDED MSW - NEEDED TO DISPLAY SUB CONTACTS-
          If Not IsDBNull(q("tsum")) Then
            If CDbl(q("tsum")) > 0 Then

              If toggleAllSubs Then
                If Not IsDBNull(q("sub_end_date")) Then
                  If CDate(q("sub_end_date").ToString) <= Today Then
                    htmlOut.Append("<tr class=""alt_row"" valign='top'>")
                  Else
                    htmlOut.Append("<tr bgcolor=""white"" valign='top'>")
                  End If
                Else
                  If IsDBNull(q("sub_start_date")) Then
                    htmlOut.Append("<tr class=""alt_row"" valign='top'>")
                  Else
                    htmlOut.Append("<tr bgcolor=""white"" valign='top'>")
                  End If
                End If
              Else
                htmlOut.Append("<tr bgcolor=""white"" valign='top'>")
              End If

              htmlOut.Append("<td align='right'></td>")


              htmlOut.Append("<td align='right'>-</td>")
              htmlOut.Append("<td align='right'>" & q("tsum").ToString & "</td>")
              htmlOut.Append("<td align='right'>" & q("tcount").ToString & "</td>")

              htmlOut.Append("<td align='left'>User Adjustments</td>")

              htmlOut.Append("</tr>")
            End If
          End If
          '----------------------------------------------------------------

          temp_desc = q("SUBNAME").ToString.Trim

        Next
      Else
        subscription_label.Visible = False
      End If

      htmlOut.Append("</table>")

      Me.subscription_label.Text = htmlOut.ToString
    Else
      If masterPage.aclsData_Temp.class_error <> "" Then
        masterPage.LogError("CompanyTabs.ascx.vb -Fill_Service_Summary_Tab() - " & masterPage.aclsData_Temp.class_error)
      End If
    End If
  End Sub

  Public Sub Fill_Active_User_Tab()
    Dim user_table As New DataTable
    Dim htmlOut As New StringBuilder
    subscription_label.Visible = True
    Dim temp_desc As String = ""

    Dim toggleAllSubs As Boolean = False
    Dim helperClass As New displayCompanyDetailsFunctions

    If sTask.ToLower.Contains("inactive") Then
      toggleAllSubs = True
      activeSub.Visible = True
      inactiveSub.Visible = False
    Else
      toggleAllSubs = False
      activeSub.Visible = False
      inactiveSub.Visible = True
    End If

    If use_insight_roll = True Then
      user_table = helperClass.Return_ActiveUser_Summary(CompanyID, JournalID, "Y", toggleAllSubs)
    Else
      user_table = helperClass.Return_ActiveUser_Summary(CompanyID, JournalID, "N", toggleAllSubs)
    End If


    If Not IsNothing(user_table) Then

      htmlOut.Append("<table id='activeUserTable' width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" class=""formatTable blue small aircraftTable"">")
      htmlOut.Append("<tr class='header_row'>")
      htmlOut.Append("<td align='left'><b>FIRSTNAME</b></td>")
      htmlOut.Append("<td align='left'><b>LASTNAME</b></td>")
      htmlOut.Append("<td align='left'><b>EMAIL</b></td>")
      htmlOut.Append("<td align='left'><b>PASSWORD</b></td>")
      htmlOut.Append("<td align='left'><b>LASTLOGIN</b></td>")
      htmlOut.Append("<td align='left'><b>ADMIN</b></td>")
      htmlOut.Append("<td align='left'><b>SUB&nbsp;ID</b></td>")

      htmlOut.Append("</tr>")

      If (user_table.Rows.Count > 0) Then

        For Each q As DataRow In user_table.Rows

          If toggleAllSubs Then
            If Not IsDBNull(q("sub_end_date")) Then
              If CDate(q("sub_end_date").ToString) <= Today Then
                htmlOut.Append("<tr class=""alt_row"" valign='top'>")
              Else
                htmlOut.Append("<tr bgcolor=""white"" valign='top'>")
              End If
            Else
              If IsDBNull(q("sub_start_date")) Then
                htmlOut.Append("<tr class=""alt_row"" valign='top'>")
              Else
                htmlOut.Append("<tr bgcolor=""white"" valign='top'>")
              End If
            End If
          Else
            htmlOut.Append("<tr bgcolor=""white"" valign='top'>")
          End If

          htmlOut.Append("<td align='left'>")

          If Not IsDBNull(q("FIRSTNAME")) Then
            If Not String.IsNullOrEmpty(q("FIRSTNAME").ToString.Trim) Then
              htmlOut.Append(q("FIRSTNAME").ToString.Replace(" ", "&nbsp;").Trim)
            End If
          End If

          htmlOut.Append("</td><td align='left'>")

          If Not IsDBNull(q("LASTNAME")) Then
            If Not String.IsNullOrEmpty(q("LASTNAME").ToString.Trim) Then
              htmlOut.Append(q("LASTNAME").ToString.Replace(" ", "&nbsp;").Trim)
            End If
          End If

          htmlOut.Append("</td><td align='left'>")

          If Not IsDBNull(q("EMAIL")) Then
            If Not String.IsNullOrEmpty(q("EMAIL").ToString.Trim) Then
              htmlOut.Append("<a class=""underline"" onclick='javascript:openSmallWindowJS(""DisplayContactDetail.aspx?compid=" + CompanyID.ToString + "&conid=" + q.Item("CONTACTID").ToString + "&JournID=" + JournalID.ToString + """,""ContactDetailsWindow"");' title='Show Contact Details'>" + q("EMAIL").ToString.Trim + "</a>")
            End If
          End If

          htmlOut.Append("</td><td align='left'>")

          If Not IsDBNull(q("PWD")) Then
            If Not String.IsNullOrEmpty(q("PWD").ToString.Trim) Then
              htmlOut.Append(q("PWD").ToString.Trim)
            End If
          End If

          htmlOut.Append("</td><td align='left'>")

          If Not IsDBNull(q("LASTLOGIN")) Then
            If Not String.IsNullOrEmpty(q("LASTLOGIN").ToString.Trim) Then
              htmlOut.Append(FormatDateTime(q("LASTLOGIN").ToString, DateFormat.ShortDate))
            End If
          End If

          htmlOut.Append("</td><td align='left'>")

          If Not IsDBNull(q("ADMIN")) Then
            If Not String.IsNullOrEmpty(q("ADMIN").ToString.Trim) Then
              htmlOut.Append(q("ADMIN").ToString.Trim)
            End If
          End If

          htmlOut.Append("</td><td align='right'>")


          If Not IsDBNull(q("SUBID")) Then
            If Not String.IsNullOrEmpty(q("SUBID").ToString.Trim) Then
              htmlOut.Append("<a class=""underline"" onclick='javascript:openSmallWindowJS(""homebaseSubscription.aspx?compID=" + CompanyID.ToString + "&subID=" + q("SUBID").ToString.Trim + """,""SubscriptionWindow"");' title='Display Subscription Details'>" + q("SUBID").ToString.Trim + "</a>")
            End If
          End If

          htmlOut.Append("</td>")

          htmlOut.Append("</tr>")

        Next
      Else
        subscription_label.Visible = False
      End If

      htmlOut.Append("</table>")

      Me.activeUser_Label.Text = htmlOut.ToString
    Else
      If masterPage.aclsData_Temp.class_error <> "" Then
        masterPage.LogError("CompanyTabs.ascx.vb -Fill_Active_User_Tab() - " & masterPage.aclsData_Temp.class_error)
      End If
    End If
  End Sub

  Public Sub Fill_Services_Used_Tab()
    Dim user_table As New DataTable
    Dim htmlOut As New StringBuilder
    servicesUsed_Label.Visible = True

    Dim helperClass As New displayCompanyDetailsFunctions

    If use_insight_roll = True Then
      user_table = helperClass.Return_Services_Used_Summary(CompanyID, JournalID, "Y")
    Else
      user_table = helperClass.Return_Services_Used_Summary(CompanyID, JournalID, "N")
    End If

    add_services.Text = "<a href='#' onclick=""javascript:load('/homeTables.aspx?type_of=Company&sub_type_of=ServicesUsed&comp_id=" & CompanyID.ToString & "','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');return false;"">ADD/UPDATE LIST</a>"
    If Not IsNothing(user_table) Then

      htmlOut.Append("<table id='activeUserTable' width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" class=""formatTable blue small aircraftTable"">")


      If (user_table.Rows.Count > 0) Then

        For Each q As DataRow In user_table.Rows

          htmlOut.Append("<tr bgcolor=""white"" valign='top'>")

          htmlOut.Append("<td align='left'>")

          If Not IsDBNull(q("svud_desc")) Then
            If Not String.IsNullOrEmpty(q("svud_desc").ToString.Trim) Then
              htmlOut.Append(q("svud_desc").ToString.Replace(" ", "&nbsp;").Trim)
            End If
          End If

          If Not IsDBNull(q("csu_end_date")) Then
            If Not String.IsNullOrEmpty(q("csu_end_date").ToString.Trim) Then
              htmlOut.Append(" ending " & clsGeneral.clsGeneral.TwoPlaceYear(q("csu_end_date").ToString.Trim))
            End If
          End If

          If Not IsDBNull(q("csu_notes")) Then
            If Not String.IsNullOrEmpty(q("csu_notes").ToString.Trim) Then
              htmlOut.Append(" [" & q("csu_notes").Trim & "]")
            End If
          End If

          htmlOut.Append("</td>")

          htmlOut.Append("</tr>")

        Next
      Else
        servicesUsed_Label.Visible = False
      End If

      htmlOut.Append("</table>")

      Me.servicesUsed_Label.Text = htmlOut.ToString
    Else
      If masterPage.aclsData_Temp.class_error <> "" Then
        masterPage.LogError("CompanyTabs.ascx.vb -Fill_Services_Used_Tab() - " & masterPage.aclsData_Temp.class_error)
      End If
    End If
  End Sub

  Public Sub Fill_Customer_Activities_Tab()
    Dim user_table As New DataTable
    Dim htmlOut As New StringBuilder
    customerActivities_Label.Visible = True
    Dim temp_desc As String = ""

    Dim helperClass As New displayCompanyDetailsFunctions


    If use_insight_roll = True Then
      user_table = helperClass.Return_Customer_Activities_Summary(CompanyID, JournalID, "Y")
    Else
      user_table = helperClass.Return_Customer_Activities_Summary(CompanyID, JournalID, "N")
    End If


    If Not IsNothing(user_table) Then

      htmlOut.Append("<table id='customerActivitiesTable' width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" class=""formatTable blue small aircraftTable"">")
      htmlOut.Append("<tr class='header_row'>")
      htmlOut.Append("<td align='left'><b>DATE (desc)</b></td>")
      htmlOut.Append("<td align='left'><b>TIME</b></td>")
      htmlOut.Append("<td align='left'><b>INIT</b></td>")
      htmlOut.Append("<td align='left'><b>CONTACT</b></td>")
      htmlOut.Append("<td align='left'><b>NOTE</b></td>")

      htmlOut.Append("</tr>")

      If (user_table.Rows.Count > 0) Then

        For Each q As DataRow In user_table.Rows

          htmlOut.Append("<tr bgcolor=""white"">")

          htmlOut.Append("<td align=""left"" valign=""middle"">")

          If Not IsDBNull(q("cstact_added_date")) Then
            If Not String.IsNullOrEmpty(q("cstact_added_date").ToString.Trim) Then
              htmlOut.Append(FormatDateTime(q("cstact_added_date").ToString, DateFormat.ShortDate))
            End If
          End If

          htmlOut.Append("</td><td align=""left"" valign=""middle"">")

          If Not IsDBNull(q("cstact_added_time")) Then
            If Not String.IsNullOrEmpty(q("cstact_added_time").ToString.Trim) Then
              htmlOut.Append(FormatDateTime(q("cstact_added_time").ToString, DateFormat.LongTime).Replace(" ", "&nbsp;"))
            End If
          End If

          htmlOut.Append("</td><td align=""left"" valign=""middle"">")

          If Not IsDBNull(q("cstact_init")) Then
            If Not String.IsNullOrEmpty(q("cstact_init").ToString.Trim) Then
              htmlOut.Append(q("cstact_init").ToString.Trim)
            End If
          End If

          htmlOut.Append("</td><td align=""left"" valign=""middle"">")

          If Not IsDBNull(q("sub_contact_id")) Then
            If Not String.IsNullOrEmpty(q("sub_contact_id").ToString.Trim) Then
              If IsNumeric(q("sub_contact_id").ToString.Trim) Then

                Dim contactName As New StringBuilder

                If Not IsDBNull(q("contact_first_name")) Then
                  If Not String.IsNullOrEmpty(q("contact_first_name").ToString.Trim) Then
                    contactName.Append(q("contact_first_name").ToString.Trim)
                  End If
                End If

                If Not IsDBNull(q("contact_last_name")) Then
                  If Not String.IsNullOrEmpty(q("contact_last_name").ToString.Trim) Then
                    contactName.Append("&nbsp;" + q("contact_last_name").ToString.Trim)
                  End If
                End If

                htmlOut.Append("<a class=""underline"" onclick='javascript:openSmallWindowJS(""DisplayContactDetail.aspx?compid=" + CompanyID.ToString + "&conid=" + q.Item("sub_contact_id").ToString + "&JournID=" + JournalID.ToString + """,""ContactDetailsWindow"");' title='Show Contact Details'>" + contactName.ToString.Trim + "</a>")
              End If
            End If
          End If

          htmlOut.Append("</td><td align=""left"" valign=""middle"">")

          If Not IsDBNull(q("cstact_note")) Then
            If Not String.IsNullOrEmpty(q("cstact_note").ToString.Trim) Then
              htmlOut.Append(q("cstact_note").ToString.Trim)
            End If
          End If

          htmlOut.Append("</td>")

          htmlOut.Append("</tr>")

        Next
      Else
        customerActivities_Label.Visible = False
      End If

      htmlOut.Append("</table>")

      customerActivities_Label.Text = htmlOut.ToString
    Else
      If masterPage.aclsData_Temp.class_error <> "" Then
        masterPage.LogError("CompanyTabs.ascx.vb -Fill_Customer_Activities_Tab() - " & masterPage.aclsData_Temp.class_error)
      End If
    End If
  End Sub
  Public Sub Fill_Research_Notes()
    Dim user_table As New DataTable
    Dim htmlOut As New StringBuilder
    Dim temp_desc As String = ""

    Dim helperClass As New displayCompanyDetailsFunctions
    Dim toggleAllActivities As Boolean = False
    Dim NoteTypeTable As New DataTable

    If Not Page.IsPostBack Then
      If use_insight_roll = True Then
        NoteTypeTable = helperClass.Return_Distinct_NoteType(CompanyID, JournalID, "Y")
      Else
        NoteTypeTable = helperClass.Return_Distinct_NoteType(CompanyID, JournalID, "N")
      End If

      If Not IsNothing(NoteTypeTable) Then
        For Each r As DataRow In NoteTypeTable.Rows
          If Not IsDBNull(r(0)) Then
            researchNoteDropdown.Items.Add(New ListItem(r(0), UCase(r(0))))
          End If
        Next
      End If

      Try
        researchNoteDropdown.SelectedValue = "RESEARCH"
      Catch ex As Exception
        researchNoteDropdown.SelectedValue = ""
      End Try
    End If

    'I do not know if this will need to display more than it does for right now, so I am just commenting this part out to use as a skeleton in case it needs to be put in.

    'If sTask.ToLower.Contains("showall") Then
    '    toggleAllActivities = True
    '    showTop50Activities.Visible = True
    '    showAllActivities.Visible = False
    'Else
    '    toggleAllActivities = False
    '    showTop50Activities.Visible = False
    '    showAllActivities.Visible = True
    'End If


    If use_insight_roll = True Then
      user_table = helperClass.Return_Research_Notes(CompanyID, JournalID, "Y", toggleAllActivities, researchNoteDropdown.SelectedValue)
    Else
      user_table = helperClass.Return_Research_Notes(CompanyID, JournalID, "N", toggleAllActivities, researchNoteDropdown.SelectedValue)
    End If


    If Not IsNothing(user_table) Then

      htmlOut.Append("<table id='researchNotesTable' width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" class=""formatTable blue small aircraftTable"">")
      htmlOut.Append("<tr class='header_row'>")
      htmlOut.Append("<td align='left'><b>DATE</b></td>")
      htmlOut.Append("<td align='left'><b>DETAILS</b></td>")
      htmlOut.Append("<td align='left'><b>STAFF</b></td>")

      htmlOut.Append("</tr>")



      If (user_table.Rows.Count > 0) Then

        For Each q As DataRow In user_table.Rows

          htmlOut.Append("<tr bgcolor=""white"">")

          htmlOut.Append("<td align=""left"" valign=""top"">")

          If Not IsDBNull(q("DATE")) Then
            If Not String.IsNullOrEmpty(q("DATE").ToString.Trim) Then
              htmlOut.Append(FormatDateTime(q("DATE").ToString, DateFormat.ShortDate))
            End If
          End If

          htmlOut.Append("</td><td align=""left"" valign=""top"">")

          If Not IsDBNull(q("DETAILS")) Then
            If Not String.IsNullOrEmpty(q("DETAILS").ToString.Trim) Then

              Dim tmpDesc As String = ""
              Dim tmpID As String = ""

              If Not IsNothing(user_table.Columns.Item("source")) Then

                If Not IsDBNull(q("Source")) Then

                  If Not IsDBNull(q("ID")) Then

                    If Not String.IsNullOrEmpty(q("ID").ToString.Trim) Then
                      tmpID = q("ID").ToString.Trim
                    End If

                  End If

                  Select Case (q("Source").ToString.ToLower.Trim)
                    Case "company documents"
                      tmpDesc = q("DETAILS").ToString.Trim.Replace("DOCUMENT:", "<a class=""underline emphasisColor"" onclick='javascript:openSmallWindowJS(""homeTables.aspx?type_of=Company&sub_type_of=" + q("Source").ToString.Trim + "&comp_id=" + CompanyID.ToString + "&activityid=" + tmpID + "&user_id=MVIT&homebase=Y&action=update"",""CompanyDetailsWindow"");' title='Show " + q("Source").ToString.Trim + "'>DOCUMENT:</a>")
                    Case "customer activity"
                      tmpDesc = q("DETAILS").ToString.Trim.Replace("SUPPORT:", "<a class=""underline emphasisColor"" onclick='javascript:openSmallWindowJS(""homeTables.aspx?type_of=Company&sub_type_of=" + q("Source").ToString.Trim + "&comp_id=" + CompanyID.ToString + "&activityid=" + tmpID + "&user_id=MVIT&homebase=Y&action=update"",""CompanyDetailsWindow"");' title='Show " + q("Source").ToString.Trim + "'>SUPPORT:</a>")
                    Case "journal"
                      tmpDesc = q("DETAILS").ToString.Trim.Replace("LOG:", "<a class=""underline emphasisColor"" onclick='javascript:openSmallWindowJS(""homeTables.aspx?type_of=Company&sub_type_of=" + q("Source").ToString.Trim + "&comp_id=" + CompanyID.ToString + "&activityid=" + tmpID + "&user_id=MVIT&homebase=Y&action=update"",""CompanyDetailsWindow"");' title='Show " + q("Source").ToString.Trim + "'>LOG:</a>")
                      tmpDesc = q("DETAILS").ToString.Trim.Replace("RESEARCH:", "<a class=""underline emphasisColor"" onclick='javascript:openSmallWindowJS(""homeTables.aspx?type_of=Company&sub_type_of=" + q("Source").ToString.Trim + "&comp_id=" + CompanyID.ToString + "&activityid=" + tmpID + "&user_id=MVIT&homebase=Y&action=update"",""CompanyDetailsWindow"");' title='Show " + q("Source").ToString.Trim + "'>RESEARCH:</a>")

                    Case "customer execution"
                      tmpDesc = q("DETAILS").ToString.Trim.Replace("EXECUTION:", "<a class=""underline emphasisColor"" onclick='javascript:openSmallWindowJS(""homeTables.aspx?type_of=Company&sub_type_of=" + q("Source").ToString.Trim + "&comp_id=" + CompanyID.ToString + "&activityid=" + tmpID + "&user_id=MVIT&homebase=Y&action=update"",""CompanyDetailsWindow"");' title='Show " + q("Source").ToString.Trim + "'>EXECUTION:</a>")
                    Case "subscription"
                      tmpDesc = q("DETAILS").ToString.Trim.Replace("SERVICE END:", "<a class=""underline emphasisColor"" onclick='javascript:openSmallWindowJS(""homeTables.aspx?type_of=Company&sub_type_of=" + q("Source").ToString.Trim + "&comp_id=" + CompanyID.ToString + "&activityid=" + tmpID + "&user_id=MVIT&homebase=Y&action=update"",""CompanyDetailsWindow"");' title='Show " + q("Source").ToString.Trim + "'>SERVICE END:</a>")

                  End Select

                Else

                  tmpDesc = q("DETAILS").ToString.Trim

                End If

              Else

                tmpDesc = q("DETAILS").ToString.Trim

              End If



              htmlOut.Append(IIf(tmpDesc.Trim.Length < 125, tmpDesc.Trim, tmpDesc.Trim + "..."))

            End If
          End If

          htmlOut.Append("</td><td align=""left"" valign=""top"">")

          If Not IsDBNull(q("STAFF")) Then
            If Not String.IsNullOrEmpty(q("STAFF").ToString.Trim) Then
              htmlOut.Append(q("STAFF").ToString.Trim)
            End If
          End If

          htmlOut.Append("</td>")

          htmlOut.Append("</tr>")

        Next
      Else
        certifications_label.Visible = False
      End If

      htmlOut.Append("</table>")


      researchNotes.Text = htmlOut.ToString
    Else
      If masterPage.aclsData_Temp.class_error <> "" Then
        masterPage.LogError("CompanyTabs.ascx.vb -Fill_Research_Notes() - " & masterPage.aclsData_Temp.class_error)
      End If
    End If
  End Sub
  Public Sub Fill_Customer_Activities_FromView()
    Dim user_table As New DataTable
    Dim htmlOut As New StringBuilder
    Dim temp_desc As String = ""
    Dim toggleAllActivities As Boolean = False

    activitiesAddNew.OnClientClick = "javascript:load(""homeTables.aspx?type_of=Company&sub_type_of=marketing&table=Journal&comp_id=" + CompanyID.ToString + "&action=add"","""",""scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no"");return false;"

    Dim helperClass As New displayCompanyDetailsFunctions

    If sTask.ToLower.Contains("showall") Then
      toggleAllActivities = True
      showTop50Activities.Visible = True
      showAllActivities.Visible = False
    Else
      toggleAllActivities = False
      showTop50Activities.Visible = False
      showAllActivities.Visible = True
    End If

    If use_insight_roll = True Then
      user_table = helperClass.Return_Customer_Actions_Summary(CompanyID, JournalID, "Y", toggleAllActivities, customerActivitiesFilter.SelectedValue)
    Else
      user_table = helperClass.Return_Customer_Actions_Summary(CompanyID, JournalID, "N", toggleAllActivities, customerActivitiesFilter.SelectedValue)
    End If



    If Not IsNothing(user_table) Then
      customerActivities_Label.Text = helperClass.DisplayCustomerActivitiesTable(user_table, CompanyID, 0)
    End If
  End Sub


  Public Sub Fill_Contract_Execution_Tab()
    Dim user_table As New DataTable
    Dim htmlOut As New StringBuilder
    contractExecution_Label.Visible = True
    Dim temp_desc As String = ""

    Dim helperClass As New displayCompanyDetailsFunctions


    If use_insight_roll = True Then
      user_table = helperClass.Return_Contract_Execution_Summary(CompanyID, JournalID, "Y")
    Else
      user_table = helperClass.Return_Contract_Execution_Summary(CompanyID, JournalID, "N")
    End If


    If Not IsNothing(user_table) Then

      htmlOut.Append("<table id='contractExecutionTable' width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" class=""formatTable blue small aircraftTable"">")
      htmlOut.Append("<tr class='header_row'>")
      htmlOut.Append("<td align=""left""><b>DATE (desc)</b></td>")
      htmlOut.Append("<td align=""left""><b>FEE</b></td>")
      htmlOut.Append("<td align=""left""><b>NOTES</b></td>")
      htmlOut.Append("<td align=""left""><b>TYPE</b></td>")

      htmlOut.Append("</tr>")

      If (user_table.Rows.Count > 0) Then

        For Each q As DataRow In user_table.Rows

          htmlOut.Append("<tr bgcolor=""white"">")

          htmlOut.Append("<td align=""left"" valign=""middle"">")

          If Not IsDBNull(q("cstexcform_exc_date")) Then
            If Not String.IsNullOrEmpty(q("cstexcform_exc_date").ToString.Trim) Then
              htmlOut.Append(FormatDateTime(q("cstexcform_exc_date").ToString, DateFormat.ShortDate))
            End If
          End If

          htmlOut.Append("</td><td align=""left"" valign=""middle"">")

          If Not IsDBNull(q("cstexcform_monthly_fee")) Then
            If Not String.IsNullOrEmpty(q("cstexcform_monthly_fee").ToString.Trim) Then
              htmlOut.Append(FormatNumber(q("cstexcform_monthly_fee").ToString.Trim, 2, TriState.False, TriState.False, True))
            End If
          End If

          htmlOut.Append("</td><td align=""left"" valign=""middle"">")

          If Not IsDBNull(q("cstexcform_notes")) Then
            If Not String.IsNullOrEmpty(q("cstexcform_notes").ToString.Trim) Then
              htmlOut.Append(q("cstexcform_notes").ToString.Replace(". ", ".<br /><br />").Trim)
            End If
          End If

          htmlOut.Append("</td><td align=""left"" valign=""middle"">")

          If Not IsDBNull(q("cstexcform_type")) Then
            If Not String.IsNullOrEmpty(q("cstexcform_type").ToString.Trim) Then
              htmlOut.Append(q("cstexcform_type").ToString.Trim)
            End If
          End If

          htmlOut.Append("</td>")

          htmlOut.Append("</tr>")

        Next
      Else
        contractExecution_Label.Visible = False
      End If

      htmlOut.Append("</table>")

      contractExecution_Label.Text = htmlOut.ToString
    Else
      If masterPage.aclsData_Temp.class_error <> "" Then
        masterPage.LogError("CompanyTabs.ascx.vb -Fill_Contract_Execution_Tab() - " & masterPage.aclsData_Temp.class_error)
      End If
    End If
  End Sub

  Public Sub Fill_Contract_List_Tab()
    Dim user_table As New DataTable
    Dim htmlOut As New StringBuilder
    contractList_Label.Visible = True
    Dim temp_desc As String = ""

    Dim helperClass As New displayCompanyDetailsFunctions


    If use_insight_roll = True Then
      user_table = helperClass.Return_Contract_List_Summary(CompanyID, JournalID, "Y")
    Else
      user_table = helperClass.Return_Contract_List_Summary(CompanyID, JournalID, "N")
    End If


    If Not IsNothing(user_table) Then

      htmlOut.Append("<table id='contractListTable' width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" class=""formatTable blue small aircraftTable"">")
      htmlOut.Append("<tr class='header_row'>")
      htmlOut.Append("<td align=""left""><b>DOCID</b></td>")
      htmlOut.Append("<td align=""left""><b>DOC&nbsp;DATE</b></td>")
      htmlOut.Append("<td align=""left""><b>ENTRY&nbsp;DATE</b></td>")
      htmlOut.Append("<td align=""left""><b>TYPE</b></td>")
      htmlOut.Append("<td align=""left""><b>SUBJECT</b></td>")

      htmlOut.Append("</tr>")

      If (user_table.Rows.Count > 0) Then

        For Each q As DataRow In user_table.Rows

          htmlOut.Append("<tr bgcolor=""white"">")

          htmlOut.Append("<td align=""left"" valign=""middle"">")

          If Not IsDBNull(q("DOCID")) Then
            If Not String.IsNullOrEmpty(q("DOCID").ToString.Trim) Then
              htmlOut.Append(q("DOCID").ToString.Trim)
            End If
          End If

          htmlOut.Append("</td><td align=""left"" valign=""middle"">")

          If Not IsDBNull(q("DOCDATE")) Then
            If Not String.IsNullOrEmpty(q("DOCDATE").ToString.Trim) Then
              htmlOut.Append(FormatDateTime(q("DOCDATE").ToString, DateFormat.ShortDate))
            End If
          End If


          htmlOut.Append("</td><td align=""left"" valign=""middle"">")

          If Not IsDBNull(q("ENTRYDATE")) Then
            If Not String.IsNullOrEmpty(q("ENTRYDATE").ToString.Trim) Then
              htmlOut.Append(FormatDateTime(q("ENTRYDATE").ToString, DateFormat.ShortDate))
            End If
          End If

          htmlOut.Append("</td><td align=""left"" valign=""middle"">")

          If Not IsDBNull(q("DOCTYPE")) Then
            If Not String.IsNullOrEmpty(q("DOCTYPE").ToString.Trim) Then
              htmlOut.Append(q("DOCTYPE").ToString.Trim)
            End If
          End If

          htmlOut.Append("</td><td align=""left"" valign=""middle"">")

          If Not IsDBNull(q("SUBJECT")) Then
            If Not String.IsNullOrEmpty(q("SUBJECT").ToString.Trim) Then
              htmlOut.Append(q("SUBJECT").ToString.Trim.Replace(". ", ".<br /><br />").Trim)
            End If
          End If

          htmlOut.Append("</td>")

          htmlOut.Append("</tr>")

        Next
      Else
        contractList_Label.Visible = False
      End If

      htmlOut.Append("</table>")

      contractList_Label.Text = htmlOut.ToString
    Else
      If masterPage.aclsData_Temp.class_error <> "" Then
        masterPage.LogError("CompanyTabs.ascx.vb -Fill_Contract_List_Tab() - " & masterPage.aclsData_Temp.class_error)
      End If
    End If
  End Sub

  Public Sub Fill_Contacts_Tab()
    Dim ContactTable As New DataTable
    If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT Then
      ContactTable = masterPage.aclsData_Temp.GetContacts(CompanyID, IIf(CRMView = False, "JETNET", CRMSource), "Y", JournalID, True)
    Else
      If (Session.Item("jetnetAppVersion") <> Constants.ApplicationVariable.CUSTOMER_CENTER) And (Session.Item("jetnetAppVersion") <> Constants.ApplicationVariable.HOMEBASE) Then
        ContactTable = masterPage.aclsData_Temp.GetContacts(CompanyID, IIf(CRMView = False, "JETNET", CRMSource), "Y", JournalID)
      Else
        ContactTable = masterPage.aclsData_Temp.GetContactsAdmin(CompanyID, IIf(CRMView = False, "JETNET", CRMSource), JournalID)
      End If

    End If

    ContactFunctions.Display_Contact_Details(ContactTable, contacts_label, CompanyID, JournalID, masterPage, True, True, True, "", CRMView, CRMSource)
    ContactTable.Dispose()
  End Sub

  Public Sub Fill_Wanteds_Tab()
    Dim bgcolor As String = ""
    Dim WantedTable As New DataTable
    wanteds_label.Visible = True
    WantedTable = masterPage.aclsData_Temp.Return_Wanted(CompanyID, "JETNET", 0, "", "", "", "", "J", JournalID)
    If Not IsNothing(WantedTable) Then
      If WantedTable.Rows.Count > 0 Then
        wanteds_label.Text = "<div class=""Box""><div class=""subHeader"">WANTEDS</div><br /><table width='100%' cellspacing='3' cellpadding='3' class='formatTable blue small'>"
        wanteds_label.Text += "<tr class='header_row'>"
        wanteds_label.Text += "<td align='left' valign='top' width='70'><b class='title'>DATE LISTED</b></td>"
        wanteds_label.Text += "<td align='left' valign='top' width='150'><b class='title'>MAKE/MODEL</b></td>"
        wanteds_label.Text += "<td align='left' valign='top'><b class='title'>NOTES</b></td>"
        wanteds_label.Text += "</tr>"
        For Each r As DataRow In WantedTable.Rows
          'toggling css on/off
          If bgcolor = "" Then
            bgcolor = "alt_row"
          Else
            bgcolor = ""
          End If

          wanteds_label.Text += "<tr class='" & bgcolor & "'>"
          wanteds_label.Text += "<td align='left' valign='top'>"
          If Not IsDBNull(r("amwant_listed_date")) Then
            wanteds_label.Text += FormatDateTime(r("amwant_listed_date"), DateFormat.ShortDate)
          End If
          wanteds_label.Text += "</td>"

          wanteds_label.Text += "<td align='left' valign='top'>" & r("amod_make_name").ToString & " " & r("amod_model_name") & "</td>"
          wanteds_label.Text += "<td align='left' valign='top'>" & r("amwant_notes").ToString & "</td>"
          wanteds_label.Text += "</tr>"
        Next
        wanteds_label.Text += "</table></div>"
      Else
        wanteds_label.Visible = False
      End If
    Else
      wanteds_label.Visible = False
    End If
    ' Response.Write(wanteds_label.Text)
  End Sub

  Public Sub Fill_Relationship_Tab()
    Dim RelationshipTable As New DataTable
    Dim tempComp As New clsClient_Company
    Dim tempData As New DataTable
    Dim tempContact As New DataTable
    Dim bgcolor As String = "#ffffff"
    Dim related_images_text As String = ""

    RelationshipTable = CompanyDetailsRelationships(IIf(CRMSource <> "CLIENT", CompanyID, CRMJetnetID))
    If Not IsNothing(RelationshipTable) Then
      If RelationshipTable.Rows.Count > 0 Then
        'Setting up the relationship table because there are rows.

        relationships_label.Text = "<div class='Box'><div class=""subHeader"">COMPANY RELATIONSHIPS</div><br />"
        relationships_label.Text += "<table class=""formatTable blue small companyTable"" width=""100%"">"
        relationships_label.Text += "<tr class=""header_row""><td><b class='title'>Relationship</b></td><td>"
        relationships_label.Text += "<b class='title'>Company</b></td></tr>"

        For Each r As DataRow In RelationshipTable.Rows
          'Declaring the local variables within the loop
          Dim Contact_Class_Array As New ArrayList
          Dim company_one As Integer = 0
          Dim contact_one As Integer = 0
          Dim contact_display As String = ""
          'Dim company_two As Integer = 0
          'Dim contact_two As Integer = 0
          Dim LinkText As String = ""
          'making sure to set this to nothing
          tempContact = New DataTable

          'Setting up Company IDs
          company_one = IIf(Not IsDBNull(r("RelCompID")), r("RelCompID"), 0)
          contact_one = IIf(Not IsDBNull(r("RelContactID")), r("RelContactID"), 0)

          'company_two = IIf(Not IsDBNull(r("compref_comp_id")), r("compref_comp_id"), 0)
          'If company_one = CompanyID Then
          '  contact_one = IIf(Not IsDBNull(r("compref_contact_id")), r("compref_contact_id"), 0)
          'End If

          'toggling css on/off
          If bgcolor = "" Then
            bgcolor = "alt_row"
          Else
            bgcolor = ""
          End If

          'setting up the label.
          relationships_label.Text += "<tr>"


          'Get the contact information if we have it.
          If contact_one <> 0 Then
            tempContact = masterPage.aclsData_Temp.GetContacts_Details(contact_one, "JETNET")
            If Not IsNothing(tempContact) Then
              If tempContact.Rows.Count > 0 Then
                Contact_Class_Array = clsGeneral.clsGeneral.Create_Array_Contact_Class(tempContact)
                For Each Con As clsClient_Contact In Contact_Class_Array
                  contact_display = clsGeneral.clsGeneral.Show_Contact_Display(Con)
                Next
              End If
            Else
              If masterPage.aclsData_Temp.class_error <> "" Then
                masterPage.LogError("DisplayCompanyDetail - GetContacts_Details(" & contact_one & ", JETNET"") - " & masterPage.aclsData_Temp.class_error)
              End If
            End If
          End If

          related_images_text = "<div class=""clear_fix""></div>"

          'If company_one <> CompanyID Then
          LinkText = DisplayFunctions.WriteDetailsLink(0, company_one, 0, 0, False, "", "blue_text", "") & " "
          'Else
          '  LinkText = DisplayFunctions.WriteDetailsLink(0, company_two, 0, 0, False, "", "blue_text", "")
          'End If

          If Session.Item("localSubscription").crmBusiness_Flag = True Then
            If Not IsDBNull(r("ac_count")) Then
              If CInt(r("ac_count")) > 0 Then
                related_images_text += "<a " & LinkText & " title='Owns " & r("ac_count").ToString & " Aircraft' alt='Owns " & r("ac_count").ToString & " Aircraft' class=""ownsAircraftIcon""><img src='images/plane_icon.png' /></a>&nbsp;"
              End If
            End If
          End If

          If Session.Item("localSubscription").crmHelicopter_Flag = True Then
            If Not IsDBNull(r("heli_count")) Then
              If CInt(r("heli_count")) > 0 Then
                related_images_text += "<a " & LinkText & " title='Owns " & r("heli_count").ToString & " Helicopter(s)' alt='Owns " & r("heli_count").ToString & " Helicopter(s)' class=""ownsHelicopterIcon""><img src='images/helicopter_icon.png' /></a>&nbsp;"
              End If
            End If
          End If


          If Session.Item("localSubscription").crmYacht_Flag = True Then
            If Not IsDBNull(r("ytcount")) Then
              If CInt(r("ytcount")) > 0 Then
                related_images_text += "<a " & LinkText & " title='Owns " & r("ytcount").ToString & " Yacht(s)' alt='Owns " & r("ytcount").ToString & " Yacht(s)' class=""ownsYachtIcon""><img src='images/yacht_icon.png' /></a>&nbsp;"
              End If
            End If
          End If



          'Show the Company Information 
          'If company_one <> CompanyID Then
          tempData = masterPage.aclsData_Temp.GetCompanyInfo_ID(company_one, "JETNET", JournalID)
          If Not IsNothing(tempData) Then
            If tempData.Rows.Count > 0 Then
              tempComp = clsGeneral.clsGeneral.Create_Company_Class(tempData, "JETNET", Nothing)
              If Not IsDBNull(r("Relationship")) Then
                If UCase(r("Relationship").ToString) = "DOING BUSINESS AS" Then
                  DoingBusinessAs = "<a class=""emphasisColor"" class='blue_text' " & DisplayFunctions.WriteDetailsLink(0, company_one, 0, 0, True, tempComp.clicomp_name, "blue_text", "")
                End If
                relationships_label.Text += "<td valign=""top"" width=""130""><b class='title'>" & r("Relationship") & "</b>"
                relationships_label.Text += related_images_text
                relationships_label.Text += "</td>"
                relationships_label.Text += "<td valign=""top""><b class=""company_title"">" & DisplayFunctions.WriteDetailsLink(0, company_one, 0, 0, True, tempComp.clicomp_name, "emphasisColor", "") & "</b><br />" & clsGeneral.clsGeneral.Show_Company_Display(tempComp, False) & IIf(contact_display <> "", "<br /><br />" & contact_display, "") & "</td>"
              End If
            End If
          End If

          relationships_label.Text += "</tr>"
          Contact_Class_Array = Nothing
        Next
        relationships_label.Text += "</tr></table>"
        relationships_label.Text += "</table>"
        relationships_label.Text += "</div>"
      Else
        relationships_label.Visible = False
      End If

    Else
      relationships_label.Visible = False
    End If

    RelationshipTable = Nothing
    tempContact = Nothing
    tempComp = Nothing
    '  Response.Write(relationships_label.Text)
  End Sub

  Private Sub all_news_checked(ByVal sender As Object, ByVal e As System.EventArgs) Handles all_news.CheckedChanged
    fill_news_tab()
  End Sub

  Public Sub fill_news_tab()
    Dim temp_news As String = ""
    Dim news_link As String = ""
    newsContainer.Visible = False
    Dim Comp_News As New DataTable


    Comp_News = masterPage.aclsData_Temp.GetCompanyNews_Listing_compid(CompanyID, all_news.Checked, JournalID)

    If Not IsNothing(Comp_News) Then
      If Comp_News.Rows.Count > 0 Then
        temp_news = "<div class=""Box""><div class=""subHeader"">NEWS</div><table cellpadding='5' cellspacing='0' width='100%' class=""formatTable blue"">"


        If Comp_News.Rows.Count = 10 Then
          all_news.Visible = True
        Else
          all_news.Visible = False
        End If

        For Each r As DataRow In Comp_News.Rows
          If Not IsDBNull(r("ytnews_web_address")) Then
            news_link = r("ytnews_web_address")
            If InStr(news_link, "http://") = 0 And Trim(news_link) <> "" Then
              news_link = "http://" & news_link
            End If
          End If
          temp_news += "<tr><td><span class='li'>" & r("ytnews_date") & "-<A href='" & news_link & "' target='_blank'>" & r("ytnews_title") & "</a>: " & Left(r("ytnews_description"), 300) & "... <i><u>More At <A href='" & news_link & "' target='_blank'>" & r("ytnewssrc_name") & "</a> </u></i></span></td></tr>"
        Next

        temp_news += "</table></div>"

        newsContainer.Visible = True
        Me.news_label.Text = temp_news
      Else
        newsContainer.Visible = False
      End If
    Else
      newsContainer.Visible = False
    End If

  End Sub

  Public Sub Fill_Aircraft_Tab()
    Dim AircraftTable As New DataTable
    Dim bgcolor As String = ""
    Dim temp_title As String = ""

    If IsNothing(ViewState("CompanyAircraft")) Then
      If CRMSource <> "CLIENT" Then
        AircraftTable = masterPage.aclsData_Temp.GetAircraft_Listing_compid(CompanyID, Session.Item("localSubscription").crmHelicopter_Flag, Session.Item("localSubscription").crmBusiness_Flag, Session.Item("localSubscription").crmCommercial_Flag, Session.Item("localSubscription").crmJets_Flag, Session.Item("localSubscription").crmExecutive_Flag, Session.Item("localSubscription").crmTurboprops, JournalID, Session.Item("localSubscription").crmAerodexFlag, amod_id, use_insight_roll)
      Else
        'Dim ClientAircraftTable As New DataTable
        'Dim JetnetAircraftTable As New DataTable
        'JetnetAircraftTable = Master.aclsData_Temp.GetAircraft_Listing_compid(CRMJetnetID, Session.Item("localSubscription").crmHelicopter_Flag, Session.Item("localSubscription").crmBusiness_Flag, Session.Item("localSubscription").crmCommercial_Flag, Session.Item("localSubscription").crmJets_Flag, Session.Item("localSubscription").crmExecutive_Flag, Session.Item("localSubscription").crmTurboprops, JournalID, Session.Item("localSubscription").crmAerodexFlag)
        'ClientAircraftTable = Master.aclsData_Temp.Client_As_Jetnet_Fields_GetAircraft_Listing_compid(CompanyID, Session.Item("localSubscription").crmHelicopter_Flag, Session.Item("localSubscription").crmBusiness_Flag, Session.Item("localSubscription").crmCommercial_Flag, Session.Item("localSubscription").crmJets_Flag, Session.Item("localSubscription").crmExecutive_Flag, Session.Item("localSubscription").crmTurboprops, JournalID, Session.Item("localSubscription").crmAerodexFlag)

        'Master.aclsData_Temp.LoopAndCombineClientJetnet(ClientAircraftTable, JetnetAircraftTable, AircraftTable)

        AircraftTable = masterPage.aclsData_Temp.Get_Client_JETNET_AC(CompanyID, "ac_id ASC", Session.Item("localSubscription").crmHelicopter_Flag, Session.Item("localSubscription").crmBusiness_Flag, Session.Item("localSubscription").crmCommercial_Flag, Session.Item("localSubscription").crmJets_Flag, Session.Item("localSubscription").crmExecutive_Flag, Session.Item("localSubscription").crmTurboprops)

      End If
      AircraftTable = FixAircraftTableRemoveDuplicates(AircraftTable)
    Else
      AircraftTable = ViewState("CompanyAircraft")
    End If

    ViewState("CompanyAircraft") = AircraftTable


    ' if yacht and no ac flag
    If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT And (Session.Item("localSubscription").crmHelicopter_Flag = False Or Session.Item("localSubscription").crmBusiness_Flag = False Or Session.Item("localSubscription").crmCommercial_Flag = False) Then
      If Not IsNothing(AircraftTable) Then
        If AircraftTable.Rows.Count > 0 Then

          ' If (Session.Item("localSubscription").crmAerodexFlag) Then
          '   aircraftDataGrid_YachtSpot.Columns(5).Visible = False
          'aircraftDataGrid.Columns(4).Visible = False
          ' End If

          aircraftDataGrid_YachtSpot.DataSource = AircraftTable

          If AircraftTable.Rows.Count <= 50 Then
            aircraftDataGrid_YachtSpot.AllowPaging = False
            AircraftTextHeader.Text = "Aircraft (" & company_amount_of_ac.Text & ") <em class='tiny_text'>(" & AircraftTable.Rows.Count & " relationship" & IIf(AircraftTable.Rows.Count = 1, "", "s") & ")</em>"
          Else
            If Trim(Request("compid")) <> "" Then
              AircraftTextHeader.Text = "Aircraft (" & company_amount_of_ac.Text & ") <em class='tiny_text'>(1-" & aircraftDataGrid.PageSize & " of " & AircraftTable.Rows.Count & " relationship" & IIf(AircraftTable.Rows.Count = 1, "", "s") & ") <a href='DisplayCompanyDetail.aspx?compid=" & Trim(Request("compid")) & "&full_ac=Y'  class=""smallSubLink"">(View All)</a></em>"
            ElseIf Trim(Request("full_ac")) <> "" Then
              AircraftTextHeader.Text = "Aircraft (" & company_amount_of_ac.Text & ") <em class='tiny_text'>(" & AircraftTable.Rows.Count & " relationship" & IIf(AircraftTable.Rows.Count = 1, "", "s") & ")</em>"
            Else
              AircraftTextHeader.Text = "Aircraft (" & company_amount_of_ac.Text & ") <em class='tiny_text'>(1-" & aircraftDataGrid.PageSize & " of " & AircraftTable.Rows.Count & " relationship" & IIf(AircraftTable.Rows.Count = 1, "", "s") & ")</em>"
            End If


          End If

          aircraftDataGrid_YachtSpot.DataBind()
        Else
          aircraft_label.Text += "<p align='center'>No Aircraft Found.</p>"
          aircraft_label.ForeColor = Drawing.Color.Red
          aircraft_label.Font.Bold = True

          If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT And (Session.Item("localSubscription").crmHelicopter_Flag = False And Session.Item("localSubscription").crmBusiness_Flag = False And Session.Item("localSubscription").crmCommercial_Flag = False) Then
            aircraftPanel.Visible = False
          End If
        End If
      End If

      aircraftDataGrid_YachtSpot.Visible = True
      aircraftDataGrid.Visible = False
    Else

      If Not IsNothing(AircraftTable) Then
        If AircraftTable.Rows.Count > 0 Then


          If amod_id > -1 Then
            temp_title = amod_make_name & " " & amod_model_name & " "
          End If

          If (Session.Item("localSubscription").crmAerodexFlag) Then
            '  aircraftDataGrid.Columns(5).Visible = False
            aircraftDataGrid.Columns(5).Visible = False
          End If

          If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CRM Then
            aircraftDataGrid.Columns(0).Visible = True
          End If

          If DisplayMobile Then
            'This means it's displaying the mobile version of the grid.
            'Toggle off old ser/reg
            'aircraftDataGrid.Columns(2).Visible = False
            'aircraftDataGrid.Columns(3).Visible = False
            ''toggle off old contact and type columns
            'aircraftDataGrid.Columns(6).Visible = False
            'aircraftDataGrid.Columns(7).Visible = False

            ''toggle on new ser/reg combined column
            'aircraftDataGrid.Columns(4).Visible = True
            ''display contact/type combined column
            'aircraftDataGrid.Columns(8).Visible = True
          End If

          aircraftDataGrid.DataSource = AircraftTable

          If aircraftDataGrid.PageSize = 1000 Then ' then we said to show all 
            AircraftTextHeader.Text = "Aircraft (" & company_amount_of_ac.Text & ") <em class='tiny_text'>(" & AircraftTable.Rows.Count & " relationship" & IIf(AircraftTable.Rows.Count = 1, "", "s") & ")</em>"
          ElseIf AircraftTable.Rows.Count <= 50 Then
            aircraftDataGrid.AllowPaging = False
            AircraftTextHeader.Text = temp_title & "Aircraft (" & company_amount_of_ac.Text & ") <em class='tiny_text'>(" & AircraftTable.Rows.Count & " relationship" & IIf(AircraftTable.Rows.Count = 1, "", "s") & ")</em>"
          Else
            If Trim(Request("compid")) <> "" And Trim(Request("full_ac")) = "" Then
              AircraftTextHeader.Text = temp_title & "Aircraft (" & company_amount_of_ac.Text & ") <em class='tiny_text'>(1-" & aircraftDataGrid.PageSize & " of " & AircraftTable.Rows.Count & " relationship" & IIf(AircraftTable.Rows.Count = 1, "", "s") & ") <a href='DisplayCompanyDetail.aspx?compid=" & Trim(Request("compid")) & "&full_ac=Y'  class=""smallSubLink"">(View All)</a></em>"
            ElseIf Trim(Request("full_ac")) <> "" Then
              AircraftTextHeader.Text = temp_title & "Aircraft (" & company_amount_of_ac.Text & ") <em class='tiny_text'>(" & AircraftTable.Rows.Count & " relationship" & IIf(AircraftTable.Rows.Count = 1, "", "s") & ")</em>"
            Else
              AircraftTextHeader.Text = temp_title & "Aircraft (" & company_amount_of_ac.Text & ") <em class='tiny_text'>(1-" & aircraftDataGrid.PageSize & " of " & AircraftTable.Rows.Count & " relationship" & IIf(AircraftTable.Rows.Count = 1, "", "s") & ")</em>"
            End If
          End If
          If Trim(Request("full_ac")) = "Y" Then
            aircraftDataGrid.AllowPaging = False
          Else
            aircraft_label.Text = AircraftTextHeader.Text
          End If
          aircraftDataGrid.DataBind()
        Else
          aircraft_label.Text += "<p align='center'>No " & temp_title & " Aircraft Found.</p>"
          aircraft_label.ForeColor = Drawing.Color.Red
          aircraft_label.Font.Bold = True

          If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT And (Session.Item("localSubscription").crmHelicopter_Flag = False And Session.Item("localSubscription").crmBusiness_Flag = False And Session.Item("localSubscription").crmCommercial_Flag = False) Then
            aircraftPanel.Visible = False
          End If
        End If
      End If

      aircraftDataGrid_YachtSpot.Visible = False
      aircraftDataGrid.Visible = True
    End If



    AircraftTable.Dispose()
    aircraft_update_panel.Update()
  End Sub

  Private Function FixAircraftTableRemoveDuplicates(ByVal AircraftTable As DataTable) As DataTable
    Dim oldACID As Long = 0
    Dim ACCounter As Integer = 0
    Dim NewAircraftTable As New DataTable
    Dim cssClass As String = ""

    NewAircraftTable = AircraftTable.Clone
    NewAircraftTable.Columns.Add("cssClass")
    For Each r As DataRow In AircraftTable.Rows
      Dim newRow As DataRow = NewAircraftTable.NewRow()

      If oldACID = r("ac_id") Then
        newRow("cssClass") = cssClass
        newRow("amod_make_name") = ""
        newRow("amod_make_type") = ""
        newRow("ac_id") = 0
        newRow("ac_amod_id") = 0
        newRow("ac_ser_nbr") = ""
        newRow("ac_reg_nbr") = ""
        newRow("ac_airframe_total_hours") = 0
        newRow("ac_year") = ""
        newRow("amod_model_name") = ""
        newRow("ac_status") = ""
        newRow("ac_year_mfr") = ""
        newRow("ac_forsale_flag") = ""
        newRow("ac_exclusive_flag") = ""
        newRow("ac_asking_wordage") = ""
        newRow("ac_asking_price") = 0
        newRow("ac_asking_wordage") = ""
        newRow("ac_ser_no_full") = ""
        'newRow("ac_date_listed") = ""
        newRow("ac_product_business_flag") = ""
        newRow("ac_product_helicopter_flag") = ""
        newRow("ac_product_commercial_flag") = ""
        newRow("ac_delivery") = ""
        newRow("ac_engine_name") = ""
      Else
        If cssClass <> "" Then
          cssClass = ""
        Else
          cssClass = "alt_row"
        End If
        ACCounter += 1
        If amod_id > -1 Then
          amod_make_name = r("amod_make_name").ToString
          amod_model_name = r("amod_model_name").ToString
        End If
        newRow("cssClass") = cssClass
        newRow("amod_make_name") = r("amod_make_name").ToString
        newRow("amod_make_type") = r("amod_make_Type").ToString
        newRow("ac_id") = r("ac_id")
        newRow("ac_amod_id") = r("ac_amod_id")
        newRow("ac_ser_nbr") = r("ac_ser_nbr").ToString
        newRow("ac_reg_nbr") = r("ac_reg_nbr").ToString
        newRow("ac_airframe_total_hours") = r("ac_airframe_total_hours")
        newRow("ac_year") = r("ac_year").ToString
        newRow("amod_model_name") = r("amod_model_name").ToString
        newRow("ac_status") = r("ac_status").ToString
        newRow("ac_year_mfr") = r("ac_year_mfr").ToString
        newRow("ac_forsale_flag") = r("ac_forsale_flag").ToString
        newRow("ac_exclusive_flag") = r("ac_exclusive_flag").ToString
        newRow("ac_asking_wordage") = r("ac_asking_wordage").ToString
        newRow("ac_asking_price") = r("ac_asking_price")
        newRow("ac_asking_wordage") = r("ac_asking_wordage").ToString
        newRow("ac_ser_no_full") = r("ac_ser_no_full").ToString
        newRow("ac_date_listed") = r("ac_date_listed")
        newRow("ac_product_business_flag") = r("ac_product_business_flag").ToString
        newRow("ac_product_helicopter_flag") = r("ac_product_helicopter_flag").ToString
        newRow("ac_product_commercial_flag") = r("ac_product_commercial_flag").ToString
        newRow("ac_delivery") = ""
        newRow("ac_engine_name") = r("ac_engine_name").ToString
      End If

      newRow("comp_name") = r("comp_name").ToString
      newRow("comp_country") = r("comp_country").ToString
      newRow("comp_state") = r("comp_state").ToString
      newRow("comp_city") = r("comp_city").ToString
      newRow("contact_title") = r("contact_title").ToString
      newRow("contact_sirname") = r("contact_sirname").ToString
      newRow("contact_first_name") = r("contact_first_name").ToString
      newRow("contact_middle_initial") = r("contact_middle_initial").ToString
      newRow("contact_last_name") = r("contact_last_name").ToString
      newRow("contact_suffix") = r("contact_suffix").ToString
      newRow("act_name") = r("act_name").ToString

      newRow("cref_transmit_seq_no") = r("cref_transmit_seq_no")

      newRow("comp_id") = r("comp_id")
      newRow("contact_id") = r("contact_id")
      newRow("source") = r("source").ToString

      newRow("acref_owner_percentage") = r("acref_owner_percentage")

      NewAircraftTable.Rows.Add(newRow)
      NewAircraftTable.AcceptChanges()
      oldACID = r("ac_id")

    Next
    company_amount_of_ac.Text = ACCounter
    Return NewAircraftTable
  End Function

  Private Function FixYachtTableRemoveDuplicates(ByVal YachtTable As DataTable) As DataTable
    Dim oldYTID As Long = 0
    Dim YTCounter As Integer = 0
    Dim NewAircraftTable As New DataTable

    NewAircraftTable = YachtTable.Clone

    ' Sql = "select ym_brand_name, ym_model_name, yt_yacht_name, yct_name,  yt_year_mfr, yt_hull_mfr_nbr, yt_forsale_flag, "
    ' Sql += " yt_forsale_status, yt_asking_price, yt_central_agent_flag, yr_contact_type, yt_id"

    For Each r As DataRow In YachtTable.Rows
      Dim newRow As DataRow = NewAircraftTable.NewRow()

      If oldYTID = r("yt_id") Then
        newRow("ym_brand_name") = ""
        newRow("ym_model_name") = ""
        newRow("yt_yacht_name") = ""
        newRow("yct_name") = r("yct_name").ToString
        newRow("yt_year_mfr") = 1900
        newRow("yt_hull_mfr_nbr") = ""
        newRow("yt_forsale_flag") = ""
        newRow("yt_for_lease_flag") = ""
        newRow("yt_for_charter_flag") = ""
        newRow("yt_forsale_status") = ""
        newRow("yt_asking_price") = 0
        newRow("yt_central_agent_flag") = ""
        newRow("yr_contact_type") = ""
        newRow("yt_id") = 0
      Else
        YTCounter += 1
        newRow("ym_brand_name") = r("ym_brand_name").ToString
        newRow("ym_model_name") = r("ym_model_name").ToString
        newRow("yt_yacht_name") = r("yt_yacht_name").ToString
        newRow("yct_name") = r("yct_name").ToString
        newRow("yt_year_mfr") = r("yt_year_mfr").ToString
        newRow("yt_hull_mfr_nbr") = r("yt_hull_mfr_nbr").ToString
        newRow("yt_forsale_flag") = r("yt_forsale_flag").ToString
        newRow("yt_for_lease_flag") = r("yt_for_lease_flag").ToString
        newRow("yt_for_charter_flag") = r("yt_for_charter_flag").ToString
        newRow("yt_forsale_status") = r("yt_forsale_status").ToString
        newRow("yt_asking_price") = r("yt_asking_price").ToString
        newRow("yt_central_agent_flag") = r("yt_central_agent_flag").ToString
        newRow("yr_contact_type") = r("yr_contact_type").ToString
        newRow("yt_id") = r("yt_id").ToString
      End If


      NewAircraftTable.Rows.Add(newRow)
      NewAircraftTable.AcceptChanges()
      oldYTID = r("yt_id")
    Next

    company_amount_of_ac.Text = YTCounter
    Return NewAircraftTable
  End Function

  Public Sub ViewCompanyHistory(ByVal sender As Object, ByVal e As System.EventArgs) ' Handles view_company_history.Click

    Call Get_Company_History()

  End Sub

  Public Sub Get_Company_History()

    Dim HistoryTable As New DataTable
    Dim OldJournalID As Long = 0
    Dim css_string As String = ""
    If InStr(view_company_history.CssClass, "blue_button") > 0 Then
      ToggleButtons(False, False, False, False, False, False)
    Else
      ToggleButtons(False, False, False, False, True, False)

      If Not IsNothing(ViewState("HistoryEvents")) Then
        HistoryTable = ViewState("HistoryEvents")
      Else
        HistoryTable = masterPage.aclsData_Temp.GetJETNET_OnlyTransactionsByCompany(CompanyID)
      End If
      ViewState("HistoryEvents") = HistoryTable

      If Not IsNothing(HistoryTable) Then
        If HistoryTable.Rows.Count > 0 Then

          If Not IsNothing(HistoryTable) Then
            If HistoryTable.Rows.Count > 0 Then
              Dim FinalTable As New DataTable
              Dim DistinctTable As New DataTable
              Dim Distinct_Table_View As New DataView
              Dim SelectedTable As New DataTable
              FinalTable.Columns.Add("journ_id")
              FinalTable.Columns.Add("journ_date")
              FinalTable.Columns.Add("journ_subject")
              FinalTable.Columns.Add("amod_make_name")
              FinalTable.Columns.Add("amod_model_name")
              FinalTable.Columns.Add("ac_ser_no_full")
              FinalTable.Columns.Add("ac_id")
              FinalTable.Columns.Add("actype_name")
              FinalTable.Columns.Add("journ_customer_note")
              FinalTable.Columns.Add("cref_comp_id")


              Distinct_Table_View = HistoryTable.DefaultView

              DistinctTable = Distinct_Table_View.ToTable(True, "journ_id")


              For Each r As DataRow In DistinctTable.Rows
                Dim result As DataRow() = HistoryTable.Select("journ_id = '" & r("journ_id") & "'", "")
                Dim RelationshipHold As String = ""
                Dim newCustomersRow As DataRow = FinalTable.NewRow()
                For Each row As DataRow In result
                  newCustomersRow("journ_id") = row("journ_id")
                  newCustomersRow("journ_date") = row("journ_date")
                  newCustomersRow("journ_subject") = row("journ_subject")
                  newCustomersRow("amod_make_name") = row("amod_make_name")
                  newCustomersRow("amod_model_name") = row("amod_model_name")
                  newCustomersRow("ac_ser_no_full") = row("ac_ser_no_full")
                  newCustomersRow("ac_id") = row("ac_id")
                  RelationshipHold += row("actype_name").ToString & "<br />"
                  newCustomersRow("journ_customer_note") = row("journ_customer_note")
                  newCustomersRow("cref_comp_id") = row("cref_comp_id")
                Next
                newCustomersRow("actype_name") = RelationshipHold

                FinalTable.Rows.Add(newCustomersRow)
                FinalTable.AcceptChanges()
              Next
              If Not IsNothing(FinalTable) Then
                If FinalTable.Rows.Count > 0 Then
                  If FinalTable.Rows.Count <= 10 Then
                    historyDataGrid.AllowPaging = False
                  End If
                  historyDataGrid.DataSource = FinalTable
                  historyDataGrid.DataBind()
                End If
              End If
            End If
          End If
        End If
      End If



      If Session.Item("localSubscription").crmYacht_Flag = True Then
        Fill_Related_Transactions()

        If JournalID > 0 Then
          relationships_label.Visible = False
          view_company_history.Visible = False
        Else
          view_company_history.Visible = True
        End If

        yachtHistory.Visible = True
      End If





    End If
    history_update_panel.Update()
  End Sub

  Public Sub ViewCompanyMap(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles map_this_company.Click
    If InStr(map_this_company.CssClass, "blue_button") > 0 Then
      ToggleButtons(False, False, False, False, False, False)
    Else
      ToggleButtons(True, False, False, False, False, False)
      If company_address.Text <> "" Then
        DisplayFunctions.BuildJavascriptMap(Me.map_update_panel, Me.GetType, True, "map_canvas", 0, False, False) ' builds javascript script for part below
        System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Draw Map", "DrawMap('" & company_address.Text & "','');", True)
      End If
    End If
    map_update_panel.Update()
  End Sub

  Public Sub ViewCompanyEvents(ByVal sender As Object, ByVal e As System.EventArgs) ' Handles view_company_events.Click
    Dim eventsTable As New DataTable
    If InStr(view_company_events.CssClass, "blue_button") > 0 Then
      ToggleButtons(False, False, False, False, False, False)
    Else
      ToggleButtons(False, False, False, True, False, False)

      If Not IsNothing(ViewState("CompanyEvents")) Then
        eventsTable = ViewState("CompanyEvents")
      Else
        eventsTable = masterPage.aclsData_Temp.EvolutionCompanyEvents(CompanyID, Session.Item("localSubscription").crmAerodexFlag, True)
      End If

      ViewState("CompanyEvents") = eventsTable
      If Not IsNothing(eventsTable) Then
        If eventsTable.Rows.Count > 0 Then
          If eventsTable.Rows.Count <= 20 Then
            eventDataGrid.AllowPaging = False
          End If
          eventDataGrid.DataSource = eventsTable
          eventDataGrid.DataBind()
          eventDataGrid.Visible = True
        Else
          events_label.Visible = True
          events_label.Text += "<p align='center'>No Events Found.</p>"
          events_label.ForeColor = Drawing.Color.Red
          events_label.Font.Bold = True
        End If
      End If
    End If
    events_update_panel.Update()
  End Sub

  Private Sub eventDataGrid_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles eventDataGrid.PageIndexChanged
    If Not IsNothing(ViewState("CompanyEvents")) Then
      eventDataGrid.CurrentPageIndex = e.NewPageIndex
      eventDataGrid.DataSource = ViewState("CompanyEvents")
      eventDataGrid.DataBind()
    End If
    events_update_panel.Update()
  End Sub

  Private Sub historyDataGrid_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles historyDataGrid.PageIndexChanged
    If Not IsNothing(ViewState("HistoryEvents")) Then
      historyDataGrid.CurrentPageIndex = e.NewPageIndex
      historyDataGrid.DataSource = ViewState("HistoryEvents")
      historyDataGrid.DataBind()
    End If
    history_update_panel.Update()
  End Sub

  Private Sub aircraftDataGrid_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles aircraftDataGrid.ItemDataBound
    Dim str As String = e.Item.Cells(e.Item.Cells.Count - 1).Text
    e.Item.CssClass = str
  End Sub

  Private Sub aircraftDataGrid_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles aircraftDataGrid.PageIndexChanged
    Dim CurrentRecord As Long = 0
    Dim StartCount As Long = 0
    Dim EndCount As Long = 0
    Dim CountString As String = ""
    Dim HoldTable As New DataTable
    If Not IsNothing(ViewState("CompanyAircraft")) Then
      aircraftDataGrid.CurrentPageIndex = e.NewPageIndex
      aircraftDataGrid.DataSource = ViewState("CompanyAircraft")
      aircraftDataGrid.DataBind()
      HoldTable = ViewState("CompanyAircraft")
    End If

    CurrentRecord = (aircraftDataGrid.PageSize * aircraftDataGrid.CurrentPageIndex) - HoldTable.Rows.Count + HoldTable.Rows.Count
    If CurrentRecord = 0 Then
      StartCount = 1
    Else
      StartCount = CurrentRecord + 1
    End If

    If CurrentRecord + aircraftDataGrid.PageSize >= HoldTable.Rows.Count Then
      CountString = StartCount & "-" & HoldTable.Rows.Count
      EndCount = HoldTable.Rows.Count
    Else
      CountString = StartCount & "-" & CurrentRecord + aircraftDataGrid.PageSize
      EndCount = CurrentRecord + aircraftDataGrid.PageSize
    End If

    If Trim(Request("compid")) <> "" Then
      AircraftTextHeader.Text = " Aircraft (" & company_amount_of_ac.Text & ") <em class='tiny_text'>(" & CountString & " of " & HoldTable.Rows.Count & " relationships) <a href='DisplayCompanyDetail.aspx?compid=" & Trim(Request("compid")) & "&full_ac=Y' class=""smallSubLink"">(View All)</a></em>"
    Else
      AircraftTextHeader.Text = " Aircraft (" & company_amount_of_ac.Text & ") <em class='tiny_text'>(" & CountString & " of " & HoldTable.Rows.Count & " relationships)</em>"
    End If


    aircraft_update_panel.Update()
  End Sub

  Private Sub export_company_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles export_company.Click
    Dim CompanyInformation As New DataTable
    Dim PhoneInformation As New DataTable


    If Trim(Request.Item("source")) = "CLIENT" Then
      CompanyInformation = masterPage.aclsData_Temp.GetCompanyInfo_ID(CompanyID, "CLIENT", 0)
      PhoneInformation = masterPage.aclsData_Temp.GetPhoneNumbers(CompanyID, 0, "CLIENT", 0)
    Else
      CompanyInformation = masterPage.aclsData_Temp.GetCompanyInfo_ID(CompanyID, "JETNET", 0)
      PhoneInformation = masterPage.aclsData_Temp.GetPhoneNumbers(CompanyID, 0, "JETNET", 0)
    End If


    If clsGeneral.clsGeneral.Create_VCard(CompanyInformation, PhoneInformation, New DataTable, New DataTable) = 1 Then
      Dim vCardPath As String = HttpContext.Current.Session.Item("MarketSummaryFolderVirtualPath").ToString + "\contact.vcf"
      Response.Redirect(vCardPath, False)
    Else
      System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Error", "javascript:alert('Error: No Information to Export.');", True)
    End If

    CompanyInformation.Dispose()
    PhoneInformation.Dispose()

  End Sub

  Public Sub viewCompanyShare(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles view_share_relationships.Click
    Dim ShareTable As New DataTable
    If InStr(view_company_events.CssClass, "blue_button") > 0 Then
      ToggleButtons(False, False, False, False, False, False)
    Else
      ToggleButtons(False, True, False, False, False, False)
      If Not IsNothing(ViewState("CompanyShare")) Then
        ShareTable = ViewState("CompanyShare")
      Else
        ShareTable = masterPage.aclsData_Temp.Return_Share_Relationships(CompanyID, JournalID)
      End If

      If Not IsNothing(ShareTable) Then
        If ShareTable.Rows.Count > 0 Then
          HelperShareRelationship(ShareTable)
        End If
      End If
    End If
    share_update_panel.Update()
  End Sub

  Private Sub ToggleButtons(ByVal MapVis As Boolean, ByVal RelationshipsVis As Boolean, ByVal FoldersVis As Boolean, ByVal EventsVis As Boolean, ByVal HistoryVis As Boolean, ByVal viewNotes As Boolean)
    Try

      If EventsVis Then
        closeEvents.Visible = True
        'view_company_events.Text = "<strong>Close Events</strong>"
        view_company_events.CssClass = "blue_button float_left"
        'events_.CssClass = "blue-theme"
        eventContainer.Visible = True
      Else
        closeEvents.Visible = False
        view_company_events.CssClass = "gray_button float_left"
        'view_company_events.Text = "<strong>Events</strong>"
        ' events_.CssClass = "dark-theme"
        eventContainer.Visible = False
        events_label.Visible = False
        events_update_panel.Update()
      End If

      If viewNotes Then
        closeNotes.Visible = True
        'view_notes.Text = "<strong>Close Notes/Actions</strong>"
        view_notes.CssClass = "blue_button float_left"
        'Notes.CssClass = "blue-theme"
        notesPanel.Visible = True
        'Reminders.CssClass = "blue-theme"
        actionPanel.Visible = True
        notes_update_panel.Update()
      Else
        closeNotes.Visible = False
        view_notes.CssClass = "gray_button float_left"
        'view_notes.Text = "<strong>Notes/Actions</strong>"
        'Notes.CssClass = "dark-theme"
        notesPanel.Visible = False
        ' Reminders.CssClass = "blue-theme"
        actionPanel.Visible = False
        notes_update_panel.Update()
      End If
      If MapVis Then
        closeMap.Visible = True
        mapContainer.Visible = True
        map_this_company.CssClass = "blue_button float_left"
        'map_this_company.Text = "<strong>Close Map</strong>"
        ' map.CssClass = "blue-theme"
        mapContainer.Visible = True
        map_update_panel.Update()
      Else
        closeMap.Visible = False
        mapContainer.Visible = False
        'map_tab.Visible = False
        map_this_company.CssClass = "gray_button float_left"
        'map.CssClass = "dark-theme"
        'map_this_company.Text = "<strong>Map</strong>"
        map_update_panel.Update()
      End If


      If RelationshipsVis Then
        closeShare.Visible = True
        'view_share_relationships.Text = "<strong>Close Relationships"
        view_share_relationships.CssClass = "blue_button float_left</strong>"
        'share.CssClass = "blue-theme"
        shareContainer.Visible = True
      Else
        closeShare.Visible = False
        view_share_relationships.CssClass = "gray_button float_left"
        'view_share_relationships.Text = "<strong>Share Relationships</strong>"
        'share.CssClass = "dark-theme"
        shareContainer.Visible = False
        share_update_panel.Update()
      End If


      If HistoryVis Then
        closeHistory.Visible = True
        ' view_company_history.Text = "<strong>Close History</strong>"
        view_company_history.CssClass = "blue_button float_left noBefore"
        'history.CssClass = "blue-theme"
        historyContainer.Visible = True
        yachtHistory.Visible = True
      Else
        closeHistory.Visible = False
        historyContainer.Visible = False
        yachtHistory.Visible = False
        view_company_history.CssClass = "gray_button float_left noBefore"
        'view_company_history.Text = "<strong>History</strong>"
        ' history.CssClass = "dark-theme"
        history_update_panel.Update()
      End If

      If FoldersVis Then
        closeFolders.Visible = True
        foldersContainer.Visible = True
        view_folders.CssClass = "blue_button float_left"
        'view_folders.Text = "<strong>Close Folders</strong>"
      Else
        closeFolders.Visible = False
        foldersContainer.Visible = False
        view_folders.CssClass = "gray_button float_left"
        'view_folders.Text = "<strong>Folders</strong>"
        folders_update_panel.Update()
      End If
    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /> error on ToggleButtons : " + Now.ToString + " [ " + ex.Message.Trim + "]<br />"

    End Try
  End Sub

  Private Sub Build_Dynamic_Folder_Table()

    'Dim FoldersTable As New DataTable
    Dim ContainerTable As New Table
    Dim TR As New TableRow
    Dim TDHold As New TableCell
    Dim SubmitButton As New LinkButton

    Try
      Select Case CRMSource
        Case "CLIENT"
          ContainerTable = DisplayFunctions.CreateStaticFoldersTable(0, OtherID, JournalID, 0, 0, masterPage.aclsData_Temp, 0)
        Case "JETNET"
          ContainerTable = DisplayFunctions.CreateStaticFoldersTable(0, CompanyID, JournalID, 0, 0, masterPage.aclsData_Temp, 0)
      End Select

      TDHold = DisplayFunctions.BuildTableCell(False, "", VerticalAlign.Top, HorizontalAlign.Left)

      SubmitButton.Text = "Save Folders"
      SubmitButton.ID = "SaveStaticFoldersButton"
      AddHandler SubmitButton.Click, AddressOf SaveStaticFolders

      TDHold.Controls.Add(SubmitButton)
      TR.Controls.Add(TDHold)

      ContainerTable.Controls.Add(TR)

      folders_label.Controls.Clear()
      ContainerTable.CssClass = "formatTable blue small"
      folders_label.Controls.Add(ContainerTable)

      If clsGeneral.clsGeneral.isCrmDisplayMode() Then
        If CRMSource = "CLIENT" Then
          crm_folders_label.Controls.Add(clsGeneral.clsGeneral.Set_Folder_Editing("CLIENT", 0, CompanyID, 0, 0, 0, 0, 1, masterPage.aclsData_Temp))
        Else
          crm_folders_label.Controls.Add(clsGeneral.clsGeneral.Set_Folder_Editing("JETNET", CompanyID, 0, 0, 0, 0, 0, 1, masterPage.aclsData_Temp))
        End If

        TR = New TableRow
        TR.CssClass = "noBorder"
        TDHold = DisplayFunctions.BuildTableCell(False, "", VerticalAlign.Top, HorizontalAlign.Left)
        SubmitButton = New LinkButton
        SubmitButton.Text = "Save Client Folders"
        SubmitButton.ID = "ClientSaveStaticFoldersButton"
        AddHandler SubmitButton.Click, AddressOf SaveStaticFoldersButtonClient
        TDHold.Controls.Add(SubmitButton)
        TR.Controls.Add(TDHold)
        crm_folders_label.Controls.Add(TR)
      End If

      folders_update_panel.Update()

    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /> error on Build_Dynamic_Folder_Table : " + Now.ToString + " [ " + ex.Message.Trim + "]<br />"

    End Try

  End Sub

  Public Sub SaveStaticFoldersButtonClient()
    ' Response.Write("This is client save")
    Dim flist As CheckBoxList = crm_folders_label.FindControl("folder_ids")
    Dim personal_flist As CheckBoxList = crm_folders_label.FindControl("personal_folder_ids")
    Dim jetnet_ac_id As Integer = 0
    Dim jetnet_comp_id As Integer = 0
    Dim jetnet_contact_id As Integer = 0
    Dim client_ac_id As Integer = 0
    Dim client_comp_id As Integer = 0
    Dim client_contact_id As Integer = 0
    Dim cfolder_id As Integer = 0
    Dim fval As String = ""
    Dim errored As String = ""


    Select Case CRMSource
      Case "CLIENT"
        client_comp_id = CompanyID
      Case "JETNET"
        jetnet_comp_id = CompanyID
    End Select


    clsGeneral.clsGeneral.Save_Folder_Action(flist, jetnet_ac_id, jetnet_comp_id, jetnet_contact_id, client_ac_id, client_comp_id, client_contact_id, masterPage.aclsData_Temp)
    clsGeneral.clsGeneral.Save_Folder_Action(personal_flist, jetnet_ac_id, jetnet_comp_id, jetnet_contact_id, client_ac_id, client_comp_id, client_contact_id, masterPage.aclsData_Temp)


  End Sub

  Private Sub SaveStaticFolders()

    Select Case CRMSource
      Case "CLIENT"
        folders_label = clsGeneral.clsGeneral.SaveStaticFolders(folders_label, masterPage.aclsData_Temp, 0, CRMJetnetID, 0, 0, 0, 0)
      Case "JETNET"
        folders_label = clsGeneral.clsGeneral.SaveStaticFolders(folders_label, masterPage.aclsData_Temp, 0, CompanyID, 0, 0, 0, 0)
    End Select

    folders_update_panel.Update()
  End Sub

  Public Sub ViewCompanyFolders(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles view_folders.Click
    If InStr(view_folders.CssClass, "blue_button") > 0 Then
      ToggleButtons(False, False, False, False, False, False)
    Else
      ToggleButtons(False, False, True, False, False, False)
    End If
    folders_update_panel.Update()
  End Sub

  Public Sub ViewCompanyNotes(ByVal sender As Object, ByVal e As System.EventArgs) ' Handles view_notes.Click
    If InStr(view_notes.CssClass, "blue_button") > 0 Then
      ToggleButtons(False, False, False, False, False, False)
    Else
      ToggleButtons(False, False, False, False, False, True)
    End If
  End Sub

  Private Sub Fill_Related_Transactions()
    Dim YachtTable As New DataTable
    Dim trans_date As String = ""

    YachtTable = masterPage.aclsData_Temp.DisplayRelatedYachtTransactionsByContactID(0, CompanyID, JournalID)



    If JournalID > 0 Then
      ' Me.history_information.Visible = True
      Me.history_information_label.Visible = True
      Me.history_information_label.Text = masterPage.aclsData_Temp.Get_Yacht_History(0, CompanyID, 0, JournalID, CRMView, trans_date)
      'Me.history_information_panel.HeaderText = "HISTORY INFORMATION AS OF: " & trans_date
      Me.historyContainer.Visible = False
    End If

    If Not IsNothing(YachtTable) Then
      If YachtTable.Rows.Count > 0 Then

        yachtHistory.Visible = True
        yacht_trans_grid.DataSource = YachtTable
        yacht_trans_grid.DataBind()

        'yacht_trans_tab.HeaderText = "Yacht History"

      Else
        yacht_trans_label.Text += "<p align='center'>No Yacht Transactions Found.</p>"
        yacht_trans_label.ForeColor = Drawing.Color.Red
        yacht_trans_label.Font.Bold = True
        yachtHistory.Visible = False
      End If
    Else
      If masterPage.aclsData_Temp.class_error <> "" Then
        masterPage.LogError("CompanyTabs.ascx.vb -Fill_Yacht_Tab() - " & masterPage.aclsData_Temp.class_error)
      End If
    End If
    YachtTable.Dispose()

    If JournalID = 0 Then
      yachtHistory.Visible = False ' turn on thro button
    Else
      yachtHistory.Visible = True  ' turn on  
    End If

  End Sub

  Public Function get_latest_marketing_notes(ByVal compID As Long)
    Dim sql As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim atemptable As New DataTable
    Try


      sql &= " Select compdoc_doc_type_code, compdoc_subject,  "
      sql &= " Case when compdoc_doc_type_code IN('EVO','AVO') then compdoc_description else compdoc_subject + ': ' + compdoc_description end as compdoc_description, "
      sql &= " compdoc_doc_date, compdoc_filename, compdoc_expiration_date "


      If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE = True Then
        sql &= " From Company_Documents with (NOLOCK) "
      ElseIf HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER = True Then
        sql &= " From [Homebase].jetnet_ra.dbo.[Company_Documents] with (NOLOCK) "
      Else
        sql &= " From Company_Documents with (NOLOCK) "
      End If


      sql &= " Where (Not compdoc_description Is NULL And Not compdoc_description = '') "
      sql &= " And compdoc_comp_id = " & compID & " "
      sql &= " And (compdoc_expiration_date >= GETDATE() Or compdoc_expiration_date Is NULL Or compdoc_expiration_date ='12/30/1899') "
      sql &= " order by compdoc_doc_date desc "


      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>Get_Company_Relationships(ByVal compID As Integer)</b><br />" & sql


      SqlConn.ConnectionString = Session.Item("jetnetClientDatabase")
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

      Return atemptable
    Catch ex As Exception
      get_latest_marketing_notes = Nothing
      'Me.class_error = "Error in Get_Company_Relationships(ByVal comp_id As Integer): SQL VERSION " & ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

  End Function

  Public Function GET_Submitted_Data(ByVal compID As Long, ByVal is_rollup As String, ByVal is_submitted_on_summary As String)
    Dim sql As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim atemptable As New DataTable
    Try
      If Not String.IsNullOrEmpty(Session.Item("jetnetClientDatabase")) Then

        If Trim(is_submitted_on_summary) = "Y" Then
          sql &= " select DISTINCT comp_id, year(acval_entry_date) as CALYEAR , month(acval_entry_date) as CALMONTH, "
          sql &= " (select COUNT(distinct a2.acval_journ_id) "
          sql &= " from Aircraft_Value a2 with (NOLOCK) where Year(a2.acval_entry_date) = Year(av1.acval_entry_date) And Month(a2.acval_entry_date) = Month(av1.acval_entry_date) "
          sql &= " and a2.acval_type='SOLD' and a2.acval_comp_id = comp_id) as PRICES,"
          sql &= "  (select COUNT(distinct a2.acval_ac_id) "
          sql &= " from Aircraft_Value a2 with (NOLOCK) where Year(a2.acval_entry_date) = Year(av1.acval_entry_date) And Month(a2.acval_entry_date) = Month(av1.acval_entry_date) "
          sql &= " and a2.acval_type='ESTIMATED PRICE' and a2.acval_comp_id = comp_id) as ESTIMATES"
        Else
          sql &= " select DISTINCT comp_id, year(acval_date) as CALYEAR , month(acval_date) as CALMONTH, "
          sql &= " (select COUNT(distinct a2.acval_journ_id) "
          sql &= " from Aircraft_Value a2 with (NOLOCK) where Year(a2.acval_date) = Year(av1.acval_date) And Month(a2.acval_date) = Month(av1.acval_date) "
          sql &= " and a2.acval_type='SOLD' and a2.acval_comp_id = comp_id) as PRICES,"
          sql &= "  (select COUNT(distinct a2.acval_ac_id) "
          sql &= " from Aircraft_Value a2 with (NOLOCK) where Year(a2.acval_date) = Year(av1.acval_date) And Month(a2.acval_date) = Month(av1.acval_date) "
          sql &= " and a2.acval_type='ESTIMATED PRICE' and a2.acval_comp_id = comp_id) as ESTIMATES"
        End If





        sql &= " from Aircraft_Value av1 with (NOLOCK)"
        sql &= "  inner join Company with (NOLOCK) on acval_comp_id = comp_id and comp_journ_id = 0 "
        sql &= " inner join Journal with (NOLOCK) on journ_id = acval_journ_id and journ_ac_id = acval_ac_id "
        sql &= " inner join Aircraft  with (NOLOCK) on acval_journ_id = ac_journ_id and ac_id = acval_ac_id "
        sql &= "  inner join Aircraft_Model with (NOLOCK) on amod_id = ac_amod_id "
        sql &= " where acval_type in ('SOLD','ESTIMATED PRICE') "

        If is_rollup = "Y" Then
          sql &= " and comp_id in (select distinct RelCompID from ReturnAllCompanyLocationsByCompId(" & compID & "))"
        Else
          sql &= " and comp_id in (" & compID & ")  "
        End If

        If Trim(is_submitted_on_summary) = "Y" Then
          sql &= "  group by  comp_id, year(acval_entry_date), month(acval_entry_date)  "
          sql &= "  order by year(acval_entry_date) desc, month(acval_entry_date) desc"
        Else
          sql &= "  group by  comp_id, year(acval_date), month(acval_date)  "
          sql &= "  order by year(acval_date) desc, month(acval_date) desc"
        End If

        'If Trim(is_submitted_on_summary) = "Y" Then
        '  sql &= " select DISTINCT comp_name AS COMPNAME, comp_id AS COMPID, year(acval_entry_date) as CALYEAR , month(acval_entry_date) as CALMONTH ,"
        '  sql &= " count(distinct journ_id) as PRICES ,"
        '  sql &= "(select COUNT(distinct a2.acval_ac_id) from Aircraft_Value a2 with (NOLOCK) where year(a2.acval_entry_date)=YEAR(Aircraft_Value.acval_entry_date)  and month(a2.acval_entry_date)=month(Aircraft_Value.acval_entry_date) and a2.acval_type='ESTIMATED PRICE' "
        '  sql &= " and a2.acval_comp_id = comp_id) as ESTIMATES "
        'Else
        '  sql &= " select DISTINCT comp_name AS COMPNAME, comp_id AS COMPID, year(journ_date) as CALYEAR , month(journ_date) as CALMONTH, count(distinct journ_id) as PRICES ,"
        '  sql &= "   (select COUNT(distinct a2.acval_ac_id) from Aircraft_Value a2 with (NOLOCK) "
        '  sql &= "  where Year(a2.acval_date) = Year(journ_date) And Month(a2.acval_date) = Month(journ_date) "
        '  sql &= " and a2.acval_type='ESTIMATED PRICE' and a2.acval_comp_id = comp_id) as ESTIMATES "
        'End If



        'sql &= " from Aircraft_Value with (NOLOCK)"
        'sql &= "  inner join Company with (NOLOCK) on acval_comp_id = comp_id and comp_journ_id = 0 "
        'sql &= " inner join Journal with (NOLOCK) on journ_id = acval_journ_id and journ_ac_id = acval_ac_id "
        'sql &= " inner join Aircraft  with (NOLOCK) on acval_journ_id = ac_journ_id and ac_id = acval_ac_id "
        'sql &= "  inner join Aircraft_Model with (NOLOCK) on amod_id = ac_amod_id "
        'sql &= " where acval_sale_price > 0 and ac_sale_price > 0 and acval_type='SOLD' "


        'If is_rollup = "Y" Then 
        '  sql &= " and  comp_id in (select distinct RelCompID from ReturnAllCompanyLocationsByCompId(" & compID & "))" 
        'Else
        '  sql &= " and comp_id in (" & compID & ")  "
        'End If

        'If Trim(is_submitted_on_summary) = "Y" Then
        '  sql &= "  group by comp_name, comp_id, year(acval_entry_date), month(acval_entry_date)  "
        '  sql &= "  order by year(acval_entry_date) desc, month(acval_entry_date) desc"
        'Else
        '  sql &= "  group by comp_name, comp_id, year(journ_date), month(journ_date) "
        '  sql &= "  order by year(journ_date) desc, month(journ_date) desc "
        'End If



        HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>Get_Company_Relationships(ByVal compID As Integer)</b><br />" & sql


        SqlConn.ConnectionString = Session.Item("jetnetClientDatabase")
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
      End If

      Return atemptable
    Catch ex As Exception
      GET_Submitted_Data = Nothing
      'Me.class_error = "Error in Get_Company_Relationships(ByVal comp_id As Integer): SQL VERSION " & ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

  End Function

  Public Function CompanyDetailsRelationships(ByVal compID As Long)
    Dim sql As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim atemptable As New DataTable
    Try
      If Not String.IsNullOrEmpty(Session.Item("jetnetClientDatabase")) Then
        sql = "select * from ReturnCompanyRelationshipsByCompId(" & compID & ") order by Relsort, Relationship "

        HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>Get_Company_Relationships(ByVal compID As Integer)</b><br />" & sql


        SqlConn.ConnectionString = Session.Item("jetnetClientDatabase")
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
      End If

      Return atemptable
    Catch ex As Exception
      CompanyDetailsRelationships = Nothing
      'Me.class_error = "Error in Get_Company_Relationships(ByVal comp_id As Integer): SQL VERSION " & ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

  End Function

  Public Function CheckCompanyPermissions(ByVal compID As Long, ByVal jID As Long)
    Dim sql As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim atemptable As New DataTable
    Dim PermissionsClause As String = ""
    Try
      If Not String.IsNullOrEmpty(Session.Item("jetnetClientDatabase")) Then
        sql = "SELECT DISTINCT comp_id "
        sql += " FROM Company WITH(NOLOCK) WHERE (comp_id = " + compID.ToString + " AND comp_journ_id = " + jID.ToString + ")"


        sql += " " + clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_YachtsIncluded(HttpContext.Current.Session.Item("localSubscription"), True, False)

        If jID > 0 Then
          sql = Replace(sql, "AND comp_active_flag = 'Y'", "")
        End If


        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b> CheckCompanyPermissions(ByVal compID As Long, ByVal journalID As Long)</b><br />" & sql


        SqlConn.ConnectionString = Session.Item("jetnetClientDatabase")
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
      End If

      Return atemptable
    Catch ex As Exception
      CheckCompanyPermissions = Nothing
      'Me.class_error = "Error in Get_Company_Relationships(ByVal comp_id As Integer): SQL VERSION " & ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

  End Function

  Public Function showFractionalPercent(ByVal act_name As String, ByVal acref_owner_percentage As String) As String
    Dim htmlOutStr As String = ""

    If Not String.IsNullOrEmpty(act_name.Trim) Then

      If act_name.ToLower.Contains("fractional owner") Then

        If Not String.IsNullOrEmpty(acref_owner_percentage.Trim) Then
          If IsNumeric(acref_owner_percentage) Then
            If CDbl(acref_owner_percentage) > 0 And CDbl(acref_owner_percentage) < 100 Then
              htmlOutStr = act_name.Trim + "&nbsp;(" + FormatNumber(CDbl(acref_owner_percentage), 3, TriState.True, TriState.False, TriState.False) + "%)"
            End If
          End If
        End If

      Else
        htmlOutStr = act_name.Trim
      End If

    End If

    Return htmlOutStr

  End Function

#Region "New_Financial_Docs_Section"


  Public Sub Build_Financial_Documents_By_Month()
    Dim DisplayString As String = ""
    Dim InnerString As String = ""
    Dim DocTable As New DataTable
    Dim css As String = ""
    Dim MonthDisplay As String = ""
    DocTable = financial_documents_functions.GetDocsByMonth(searchCriteria, use_insight_roll)

    If Not IsNothing(DocTable) Then
      If DocTable.Rows.Count > 0 Then

        For Each r As DataRow In DocTable.Rows
          MonthDisplay = ""
          If InnerString <> "" Then
            InnerString += ","
          End If
          If Not IsDBNull(r("tmonth")) Then
            MonthDisplay = r("tmonth").ToString
          End If
          If Not IsDBNull(r("tyear")) Then
            If MonthDisplay <> "" Then
              MonthDisplay += "/"
            End If
            MonthDisplay += Right(r("tyear").ToString, 2)
          End If

          InnerString += "['" & MonthDisplay & "', " & r("tcount").ToString & "]"
        Next

        DisplayString = "var data = google.visualization.arrayToDataTable(["
        DisplayString += "['Month', 'Docs Per Month'],"
        DisplayString += InnerString
        DisplayString += "]);"

        DisplayString += "var options = {"
        DisplayString += "hAxis: {"
        DisplayString += "title:  'Month/Year',"
        DisplayString += " minValue: 0,"
        DisplayString += "},"
        DisplayString += "vAxis: {"
        DisplayString += "title:  '# of Docs'"
        DisplayString += "},"
        DisplayString += "chartArea: {'left':45,'top':20, 'width':'240', 'height':'120'},"
        DisplayString += "'width':320,"
        DisplayString += "'height':180,"
        DisplayString += "bars:   'vertical',"
        DisplayString += "legend: { position: ""none"" }"
        DisplayString += "};"
        DisplayString += "var material = new google.visualization.ColumnChart(document.getElementById('chart_div_tab1_all'));"
        DisplayString += "material.draw(data, options);"

      End If
    End If
    DocTable.Dispose()

    If Not Page.ClientScript.IsClientScriptBlockRegistered("DocByMonthChart") Then
      System.Web.UI.ScriptManager.RegisterStartupScript(news_tab_update_panel, Me.GetType(), "DocByMonthChart", DisplayString.ToString, True)
    End If

    'relationships.Height = Unit.Pixel(200)
  End Sub
  Public Sub Build_Related_Financing_Companies_Documents()
    Dim DisplayString As String = ""
    Dim DocTable As New DataTable
    Dim css As String = ""
    DocTable = financial_documents_functions.GetRelatedDocs(searchCriteria, use_insight_roll)

    If Not IsNothing(DocTable) Then
      If DocTable.Rows.Count > 0 Then
        DisplayString = "<div class=""Box""><table width=""100%"" cellpadding=""3"" cellspacing=""0"" class=""formatTable blue"">"
        DisplayString += "<tr class=""header_row""><td align=""left"" valign=""top""><b>Company</b></td><td align=""right"" valign=""top""><b># of Docs</b></td></tr>"

        For Each r As DataRow In DocTable.Rows
          DisplayString += "<tr class=""" & css & """>"
          DisplayString += "<td align=""left"" valign=""top"">"

          'Company
          DisplayString += WriteCompAdd(r)
          DisplayString += "</td>"

          DisplayString += "<td align=""right"" valign=""top"">"
          '# of documents.
          If Not IsDBNull(r("tcount")) Then
            DisplayString += r("tcount").ToString
          End If
          DisplayString += "</td>"
          DisplayString += "</tr>"
          If css <> "" Then
            css = ""
          Else
            css = "alt_row"
          End If
        Next

        DisplayString += "</table></div>"
      End If
    End If
    DocTable.Dispose()

    If use_insight_roll = True Then
      Company_Relationship_Label.Text = DisplayString
    Else
      news_label.Text = DisplayString
    End If

  End Sub
  Public Sub Build_Types_Financial_Documents()
    Dim DisplayString As String = ""
    Dim DocTable As New DataTable
    Dim css As String = ""
    DocTable = financial_documents_functions.GetTypeDocs(searchCriteria, use_insight_roll)

    If Not IsNothing(DocTable) Then
      If DocTable.Rows.Count > 0 Then
        DisplayString = "<div class=""Box""><div class=""subHeader"">Type of Financial Documents (Last 6 Months)</div><br /><table width=""100%"" cellpadding=""3"" cellspacing=""0"" class=""formatTable blue"">"
        DisplayString += "<tr class=""header_row""><td align=""left"" valign=""top""><b>Type</b></td><td align=""right"" valign=""top""><b># of Docs</b></td></tr>"

        For Each r As DataRow In DocTable.Rows
          DisplayString += "<tr class=""" & css & """>"
          DisplayString += "<td align=""left"" valign=""top"">"
          'Type
          If Not IsDBNull(r("adoc_doc_type")) Then
            DisplayString += r("adoc_doc_type").ToString
          End If
          DisplayString += "</td>"

          DisplayString += "<td align=""right"" valign=""top"">"
          '# of documents.
          If Not IsDBNull(r("tcount")) Then
            DisplayString += r("tcount").ToString
          End If
          DisplayString += "</td>"
          DisplayString += "</tr>"
          If css <> "" Then
            css = ""
          Else
            css = "alt_row"
          End If
        Next

        DisplayString += "</table></div>"
      End If
    End If
    DocTable.Dispose()
    summary_label.Text = DisplayString
  End Sub
  Public Sub Build_Model_Financial_Documents()

    Dim DisplayString As String = ""
    Dim ModTable As New DataTable
    Dim css As String = ""
    ModTable = financial_documents_functions.GetModelDocuments(searchCriteria, use_insight_roll)

    If Not IsNothing(ModTable) Then
      If ModTable.Rows.Count > 0 Then
        DisplayString = "<div class=""Box""><table width=""100%"" cellpadding=""3"" cellspacing=""0"" class=""formatTable blue"">"
        DisplayString += "<tr class=""header_row""><td align=""left"" valign=""top""><b>Make/Model</b></td><td align=""right"" valign=""top""><b># of Docs</b></td></tr>"

        For Each r As DataRow In ModTable.Rows
          DisplayString += "<tr class=""" & css & """>"
          DisplayString += "<td align=""left"" valign=""top"">"
          'Model / Make Link
          If Not IsDBNull(r("amod_make_name")) And Not IsDBNull(r("amod_make_name")) Then
            DisplayString += DisplayFunctions.WriteModelLink(r("amod_id"), r("amod_make_name") & " / " & r("amod_model_name"), True)
          End If
          DisplayString += "</td>"

          DisplayString += "<td align=""right"" valign=""top"">"
          '# of documents.
          If Not IsDBNull(r("tcount")) Then
            DisplayString += r("tcount").ToString
          End If
          DisplayString += "</td>"
          DisplayString += "</tr>"
          If css <> "" Then
            css = ""
          Else
            css = "alt_row"
          End If
        Next

        DisplayString += "</table></div>"
      End If
    End If
    ModTable.Dispose()
    aircraft_model_label.Text = DisplayString
  End Sub

  Private Sub DisplayCompanyDetail_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit

    Try
      If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
        Me.MasterPageFile = "~/EvoStyles/EmptyCustomerAdminTheme.master"
        masterPage = DirectCast(Page.Master, EmptyCustomerAdminTheme)
      ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Then
        Me.MasterPageFile = "~/EvoStyles/EmptyHomebaseTheme.Master"
        masterPage = DirectCast(Page.Master, EmptyHomebaseTheme)
      ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO Then
        Me.MasterPageFile = "~/EvoStyles/EmptyEvoTheme.master"
        masterPage = DirectCast(Page.Master, EmptyEvoTheme)
      ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CRM Then
        Me.MasterPageFile = "~/EvoStyles/EmptyEvoTheme.master"
        masterPage = DirectCast(Page.Master, EmptyEvoTheme)
      End If

    Catch ex As Exception
      If Not IsNothing(masterPage) Then
        masterPage.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString & " (DisplayCompanyDetail_PreInit): " & ex.Message.ToString)
      Else
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += " (DisplayCompanyDetail_PreInit): " + ex.Message.ToString.Trim
      End If
    End Try

  End Sub
#End Region

  Public Function getMarketingNote(ByVal inCompID As Long) As DataTable

    Dim atemptable As New DataTable
    Dim subQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      subQuery.Append("SELECT comp_marketing_notes FROM " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[HomeBase].jetnet_ra.dbo.", "") + "Company WITH(NOLOCK)")
      subQuery.Append(" WHERE comp_id = @comp_id and comp_journ_id = 0")

      SqlCommand.Parameters.Add("@comp_id", SqlDbType.Int).Value = inCompID.ToString.Trim

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 90

      SqlCommand.CommandText = subQuery.ToString
      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, subQuery.ToString)
      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()

        Return Nothing

      End Try

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message

      Return Nothing

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return atemptable

  End Function

  Private Sub DisplayCompanyDetail_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete

    Dim ChartJavaScript As String = ""

    'ChartJavaScript += " google.charts.setOnLoadCallback(drawModuleCharts);"

    ChartJavaScript += "$(window).resize(function() {" & vbNewLine
    ChartJavaScript += "if(this.resizeTO) clearTimeout(this.resizeTO);" & vbNewLine
    ChartJavaScript += "this.resizeTO = setTimeout(function() {" & vbNewLine
    ChartJavaScript += "$(this).trigger('resizeEnd');" & vbNewLine
    ChartJavaScript += "}, 200);" & vbNewLine
    ChartJavaScript += "});" & vbNewLine

    '//redraw graph when window resize is completed  
    ChartJavaScript += "$(window).on('resizeEnd', function() {" & vbNewLine
    ChartJavaScript += "$(""[id^='chart_div']"").empty(); " & vbNewLine
    ChartJavaScript += "if (typeof drawCharts === ""function"") { " & vbNewLine

    ChartJavaScript += "  drawCharts();" & vbNewLine
    ChartJavaScript += " } " & vbNewLine
    ChartJavaScript += "});" & vbNewLine


    'ChartJavaScript = "function drawModuleCharts() {" & ChartJavaScript & "};" & vbNewLine

    System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "RedrawChart", ChartJavaScript, True)
  End Sub
End Class