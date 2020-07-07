
' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/MasterLists.aspx.vb $
'$$Author: Mike $
'$$Date: 6/16/20 11:55a $
'$$Modtime: 6/16/20 10:29a $
'$$Revision: 4 $
'$$Workfile: MasterLists.aspx.vb $
'
' ********************************************************************************

Partial Public Class MasterLists
  Inherits System.Web.UI.Page

  Dim localCriteria As New helpSelectionCriteriaClass
  Dim localDataLayer As New helpListsDataLayer

  Public Shared masterPage As New Object

  Private Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

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
      ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT Then
        Me.MasterPageFile = "~/EvoStyles/EmptyEvoTheme.master"
        masterPage = DirectCast(Page.Master, EmptyEvoTheme)
      ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CRM Then
        Me.MasterPageFile = "~/EvoStyles/EmptyEvoTheme.master"
        masterPage = DirectCast(Page.Master, EmptyEvoTheme)
      End If

    Catch ex As Exception
      If Not IsNothing(masterPage) Then
        masterPage.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString + " : " + ex.Message.ToString.Trim)
      Else
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodInfo.GetCurrentMethod().ToString + "</b><br />" + ex.Message.ToString.Trim
      End If
    End Try

  End Sub

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    Dim sErrorString As String = ""

    Try

      If Not Page.IsPostBack Then
        close_window_only.Text += ("<a class=""underline cursor"" onclick=""javascript:window.close();return false;"" class=""close_button"" style=""padding-right:15px;""><img src='images/x.svg' alt='Close' /></a>")
      End If

      If Session.Item("crmUserLogon") <> True Then
        Response.Redirect("Default.aspx", False)
      Else
        masterPage.SetPageTitle("Master Help Lists") 'Page title that can be set to whatever is necessary.
      End If

      If Not IsNothing(Request.Item("helplist")) Then
        If Not String.IsNullOrEmpty(Request.Item("helplist").ToString.Trim) Then
          localCriteria.HelpCriteriaShowList = Request.Item("helplist").ToString.Trim
        End If
      End If

      If Not IsNothing(Request.Item("helpAvType")) Then
        If Not String.IsNullOrEmpty(Request.Item("helpAvType").ToString.Trim) Then
          localCriteria.HelpCriteriaAvionicsType = Request.Item("helpAvType").ToString.Trim
        End If
      End If

      If Not IsNothing(Request.Item("helpAirframe")) Then
        If Not String.IsNullOrEmpty(Request.Item("helpAirframe").ToString.Trim) Then
          localCriteria.HelpCriteriaAirframeType = Request.Item("helpAirframe").ToString.Trim
        End If
      End If

      If Not IsNothing(Request.Item("helpAirtype")) Then
        If Not String.IsNullOrEmpty(Request.Item("helpAirtype").ToString.Trim) Then
          localCriteria.HelpCriteriaMakeType = Request.Item("helpAirtype").ToString.Trim
        End If
      End If

      If Not IsNothing(Request.Item("helpMake")) Then
        If Not String.IsNullOrEmpty(Request.Item("helpMake").ToString.Trim) Then
          localCriteria.HelpCriteriaMakeName = Request.Item("helpMake").ToString.Trim
        End If
      End If

      If Not IsNothing(Request.Item("helpModel")) Then
        If Not String.IsNullOrEmpty(Request.Item("helpModel").ToString.Trim) Then
          localCriteria.HelpCriteriaModelID = CLng(Request.Item("helpModel").ToString.Trim)
        End If
      End If

      localDataLayer.adminConnectStr = Session.Item("jetnetAdminDatabase").ToString.Trim

      If Not Session.Item("localPreferences").loadUserSession(sErrorString, CLng(Session.Item("localUser").crmSubSubID.ToString), Session.Item("localUser").crmUserLogin.ToString, CLng(Session.Item("localUser").crmSubSeqNo.ToString), CLng(Session.Item("localUser").crmUserContactID.ToString)) Then
        Response.Write("error in load preferences : " + sErrorString)
      End If

      Dim fSubins_platform_os As String = commonEvo.getBrowserCapabilities(Request.Browser)

      localDataLayer.clientConnectStr = Session.Item("jetnetClientDatabase").ToString.Trim
      localDataLayer.starConnectStr = Session.Item("jetnetStarDatabase").ToString.Trim
      localDataLayer.serverConnectStr = Session.Item("jetnetServerNotesDatabase").ToString.Trim
      localDataLayer.cloudConnectStr = Session.Item("jetnetCloudNotesDatabase").ToString.Trim

      If Not String.IsNullOrEmpty(localCriteria.HelpCriteriaShowList.Trim) Then

        ' load and display list selected
        Select Case localCriteria.HelpCriteriaShowList.ToLower

          Case "weightclass"
            displayList.HeaderText = "<strong>Aircraft Weight Class (MGTOW)</strong>"
            localDataLayer.help_display_weight_class_list(localCriteria, helpListContent.Text)

          Case "weightclassmodel"
            displayList.HeaderText = "<strong>Master List Aircraft Make/Models With Weight Class</strong>"
            localDataLayer.help_display_weight_class_list_model(localCriteria, helpListContent.Text)

          Case "lifecycle"
            displayList.HeaderText = "<strong>Aircraft Lifecycle</strong>"
            localDataLayer.help_display_aircraft_lifecycle(localCriteria, helpListContent.Text)

          Case "serial"
            displayList.HeaderText = "<strong>Master List Make/Model Serial Number Formats</strong>"
            localDataLayer.help_display_aircraft_serial_number_format(localCriteria, helpListContent.Text)

          Case "registration"
            displayList.HeaderText = "<strong>Registration Number Prefix Master List</strong>"
            localDataLayer.help_display_aircraft_registration_number_prefix(localCriteria, helpListContent.Text)

          Case "feature"
            displayList.HeaderText = "<strong>Feature Codes</strong>"
            localDataLayer.help_display_aircraft_features(localCriteria, helpListContent.Text)

          Case "featuremodel"
            displayList.HeaderText = "<strong>Key Features Master List By Make/Model</strong>&nbsp;<font color='blue'>Blue = Standard Equipment</font>"
            localDataLayer.help_display_aircraft_model_features(localCriteria, helpListContent.Text, Trim(Request("fcode")))

          Case "avionics"
            displayList.HeaderText = "<strong>Aircraft Avionics"

            If Not String.IsNullOrEmpty(localCriteria.HelpCriteriaAvionicsType.Trim) Then
              displayList.HeaderText += " " + crmWebClient.Constants.cHyphen + " " + localCriteria.HelpCriteriaAvionicsType.Trim
            End If

            displayList.HeaderText += "</strong>"

            localDataLayer.help_display_aircraft_avionics(localCriteria, helpListContent.Text)

          Case "engineprefix"
            displayList.HeaderText = "<strong>Master List Aircraft Make/Models - Engine Model Prefix</strong>"
            localDataLayer.help_display_aircraft_make_model_engine_prefix(localCriteria, helpListContent.Text)

          Case "emp"
            displayList.HeaderText = "<strong>Master List - Engine Maintenance Program Summary</strong>"
            localDataLayer.help_display_aircraft_emp(localCriteria, helpListContent.Text)

          Case "emgp"
            displayList.HeaderText = "<strong>Master List - Engine Management Program Summary</strong>"
            localDataLayer.help_display_aircraft_emgp(localCriteria, helpListContent.Text)

          Case "amp"
            displayList.HeaderText = "<strong>Master List - Airframe Maintenance Program Summary</strong>"
            localDataLayer.help_display_aircraft_amp(localCriteria, helpListContent.Text)

          Case "amtp"
            displayList.HeaderText = "<strong>Master List - Airframe Maintenance Tracking Program Summary</strong>"
            localDataLayer.help_display_aircraft_amtp(localCriteria, helpListContent.Text)

          Case "transactioncodes"
            displayList.HeaderText = "<strong>Transaction Codes Master List By Code</strong>"
            Dim tmpHtmlOut As String = ""

            localCriteria.HelpCriteriaTransactionSendToWeb = True
            localDataLayer.help_display_transaction_codes(localCriteria, tmpHtmlOut)
            helpListContent.Text = tmpHtmlOut

            If HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME").ToString.ToUpper.Contains("JETNET12") Then
              localCriteria.HelpCriteriaTransactionSendToWeb = False
              localDataLayer.help_display_transaction_codes(localCriteria, tmpHtmlOut)
              helpListContent.Text = "<br />" + tmpHtmlOut
            End If

          Case "contacttypes"
            displayList.HeaderText = "<strong>Aircraft Company Relation Contact Type Master List By Code (Includes Aircraft History)</strong>"
            localDataLayer.help_display_aircraft_contact_type(localCriteria, helpListContent.Text)

          Case "companybustype"
            displayList.HeaderText = "<strong>Company Business Type Master List By Code</strong>"
            localDataLayer.help_display_company_business_type(localCriteria, helpListContent.Text)

          Case "aircraftmodelbustypes"
            displayList.HeaderText = "<strong>Aircraft Model Business Types</strong>"
            localDataLayer.help_display_aircraft_model_types(localCriteria, helpListContent.Text)

          Case "bas"
            displayList.HeaderText = "<strong>Business Aircraft Sizes</strong>"
            localDataLayer.help_display_business_aircraft_sizes(localCriteria, helpListContent.Text)

          Case Else
            displayList.Visible = False
            tab_container_ID.ActiveTabIndex = 0

        End Select

        displayList.Visible = True
        tab_container_ID.ActiveTabIndex = 1

      Else

        displayList.Visible = False
        tab_container_ID.ActiveTabIndex = 0

      End If

      add_ChangeTopActiveTab_Script(tab_container_ID)

    Catch ex As Exception

      displayList.Visible = False
      tab_container_ID.ActiveTabIndex = 0

      If Not IsNothing(masterPage) Then
        masterPage.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString + " : " + ex.Message.ToString.Trim)
      Else
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodInfo.GetCurrentMethod().ToString + "</b><br />" + ex.Message.ToString.Trim
      End If

    Finally

    End Try


  End Sub

  Public Sub add_ChangeTopActiveTab_Script(ByVal tcSource As AjaxControlToolkit.TabContainer)

    'Register the script block
    Dim sScptStr As StringBuilder = New StringBuilder()

    If Not Page.ClientScript.IsClientScriptBlockRegistered("cht-top-onclick") Then

      sScptStr.Append("<script type=""text/javascript"">")
      sScptStr.Append(vbCrLf & "  function changeTopTab(num) {")
      sScptStr.Append(vbCrLf & "    var container = $find(""" + tcSource.ClientID.ToString + """);")
      sScptStr.Append(vbCrLf & "    container.set_activeTabIndex(num);")
      sScptStr.Append(vbCrLf & "  }")
      sScptStr.Append(vbCrLf & "</script>")

      Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "cht-top-onclick", sScptStr.ToString, False)

    End If

    sScptStr = Nothing

  End Sub

End Class