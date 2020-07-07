Imports System.IO
Imports crmWebClient.clsGeneral
Partial Public Class Aircraft_Edit_Template
  Inherits System.Web.UI.UserControl
  Dim aclsData_Temp As New clsData_Manager_SQL
  Dim aTempTable, aTempTable2 As New DataTable 'Data Tables used
  Public Event loop_contacts(ByVal idnum_new As Integer, ByVal jetnet_id As Integer, ByVal contact_id As Integer, ByVal skip As Boolean, ByVal inactive As Boolean, ByVal new_client_ac As Integer, ByVal jetnet_ac_id As Integer)
  Public Event get_insert_ac(ByVal jetnet_id As Integer, ByVal idnum_new As Integer, ByVal get_previous_ac As Boolean, ByVal insert_ac As Boolean, ByVal new_client_ac As Integer, ByVal jetnet_ac_id As Integer)
  Dim error_string As String = ""
  Dim fromVIEW As Boolean = False
  Dim viewNOTEID As Long = 0



#Region "Page Load"

  Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
   
    'If (HttpContext.Current.Session.Item("jetnetWebSiteType") = crmWebClient.eWebSiteTypes.LOCAL) Then
    If Trim(Request("action")) = "edit" And Trim(Request("remove")) = "" And Trim(Request("addValueOnly")) = "" And Trim(Request("type")) = "aircraft" And Trim(Request("synch")) = "" Then
      Dim LargetabIndexChangedScript As StringBuilder = New StringBuilder()

      System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "runJQuery", "window.onload = function() {runJQuery();askingWordageChange();acSaleChanged(); };", True)
    End If
    'End If


    clsGeneral.clsGeneral.WriteJqueryForAircraftEditBlocks(Page, ac_sale, ac_status_not_for_sale, ac_status_for_sale, CompareValidator1, date_listed_panel, date_listed, DOMlisted, DOMWord, est_label, cliaircraft_value_description_text, est_price, broker_price, broker_lbl, asking_price, asking_wordage, ask_lbl, ac_exclusive)


    LoadAcStatusChangeJS()

    If Not Page.IsPostBack Then
      Dim strJv As String = ""
      strJv = " if ( $('#" & model_cbo.ClientID & " option:selected').text().toUpperCase() == 'NOT LISTED') { $('#" & model_listing.ClientID & "').css('display','block'); $('#" & model_cbo.ClientID & "').css('display','none'); } else {$('#" & model_listing.ClientID & "').css('display','none'); $('#" & model_cbo.ClientID & "').css('display','block'); } "
      model_cbo.Attributes.Add("onChange", strJv)

      notCustomACLink.Text = "<a href='#' onClick=""$('#" & model_cbo.ClientID & "').prop('disabled', false);$('#" & model_cbo.ClientID & "').css('display','block');$('#" & model_text.ClientID & "').css('display','true');$('#" & model_listing.ClientID & "').css('display','none');"">Not a Custom Aircraft?</a>"

      asking_wordage.Attributes.Add("onChange", "askingWordageChange()")
      ac_sale.Attributes.Add("onChange", "acSaleChanged()")
      lifecycle_list.Attributes.Add("onChange", "acSaleChanged()")
    End If

  End Sub


  Private Sub LoadAcStatusChangeJS()
    If Not Page.ClientScript.IsClientScriptBlockRegistered("acStatusLoad") Then
      Dim acStatusLoadScript As StringBuilder = New StringBuilder()
      acStatusLoadScript.Append(vbCrLf & "  function acStatusLoad() {")

      acStatusLoadScript.Append(vbCrLf & "if($('#" & ac_sale.ClientID & " input:checked').val() == 'Y') {")

      'toggle others as invisible
      acStatusLoadScript.Append(vbCrLf & "$('#" & ac_status_for_sale.ClientID & "').css('display','block');")
      acStatusLoadScript.Append(vbCrLf & "$('#" & ac_status_not_for_sale.ClientID & "').css('display','none');")
      acStatusLoadScript.Append(vbCrLf & "$('#" & ac_status_not_for_sale_withdrawn.ClientID & "').css('display','none');")

      'set values as empty
      acStatusLoadScript.Append(vbCrLf & "$('#" & ac_status_not_for_sale.ClientID & "').val('');")
      acStatusLoadScript.Append(vbCrLf & "$('#" & ac_status_not_for_sale_withdrawn.ClientID & "').val('');")


      acStatusLoadScript.Append(vbCrLf & "var new_val = $('#" & ac_status_hold.ClientID & "').val();")

      acStatusLoadScript.Append(vbCrLf & "if ($('#" & ac_status_for_sale.ClientID & " option[value=""'+new_val+'""]').length) {")
      acStatusLoadScript.Append(vbCrLf & "$('#" & ac_status_for_sale.ClientID & "').val($('#" & ac_status_hold.ClientID & "').val());")
      acStatusLoadScript.Append(vbCrLf & " } else {")
      acStatusLoadScript.Append(vbCrLf & "$('#" & ac_status_for_sale.ClientID & "').val('Other');")
      acStatusLoadScript.Append(vbCrLf & " } ")

      acStatusLoadScript.Append(vbCrLf & " } else if ($('#" & ac_sale.ClientID & " input:checked').val() == 'N' && $('#" & lifecycle_list.ClientID & "').val() == '4') {")

      'toggle others as invisible
      acStatusLoadScript.Append(vbCrLf & "$('#" & ac_status_for_sale.ClientID & "').css('display','none');")
      acStatusLoadScript.Append(vbCrLf & "$('#" & ac_status_not_for_sale.ClientID & "').css('display','none');")
      acStatusLoadScript.Append(vbCrLf & "$('#" & ac_status_not_for_sale_withdrawn.ClientID & "').css('display','block');")

      'set values as empty
      acStatusLoadScript.Append(vbCrLf & "$('#" & ac_status_for_sale.ClientID & "').val('');")
      acStatusLoadScript.Append(vbCrLf & "$('#" & ac_status_not_for_sale.ClientID & "').val('');")


      acStatusLoadScript.Append(vbCrLf & "var new_val = $('#" & ac_status_hold.ClientID & "').val();")

      acStatusLoadScript.Append(vbCrLf & "if ($('#" & ac_status_not_for_sale_withdrawn.ClientID & " option[value=""'+new_val+'""]').length) {")
      acStatusLoadScript.Append(vbCrLf & "$('#" & ac_status_not_for_sale_withdrawn.ClientID & "').val($('#" & ac_status_hold.ClientID & "').val());")
      acStatusLoadScript.Append(vbCrLf & " } else {")
      acStatusLoadScript.Append(vbCrLf & "$('#" & ac_status_not_for_sale_withdrawn.ClientID & "').val('Other');")
      acStatusLoadScript.Append(vbCrLf & " } ")


      acStatusLoadScript.Append(vbCrLf & " } else if ($('#" & ac_sale.ClientID & " input:checked').val() == 'N') {")

      acStatusLoadScript.Append(vbCrLf & "$('#" & ac_status_not_for_sale.ClientID & "').val('Not For Sale');")
      'toggle others as invisible
      acStatusLoadScript.Append(vbCrLf & "$('#" & ac_status_for_sale.ClientID & "').css('display','none');")
      acStatusLoadScript.Append(vbCrLf & "$('#" & ac_status_not_for_sale.ClientID & "').css('display','block');")
      acStatusLoadScript.Append(vbCrLf & "$('#" & ac_status_not_for_sale_withdrawn.ClientID & "').css('display','none');")

      'set values as empty
      acStatusLoadScript.Append(vbCrLf & "$('#" & ac_status_for_sale.ClientID & "').val('');")
      acStatusLoadScript.Append(vbCrLf & "$('#" & ac_status_not_for_sale_withdrawn.ClientID & "').val('');")

      acStatusLoadScript.Append(vbCrLf & "}")

      acStatusLoadScript.Append(vbCrLf & "}")

      Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "acStatusLoad()", acStatusLoadScript.ToString, True)
    End If

  End Sub

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Me.Visible Then
      Try

        Dim TransactionExists As Boolean = False
        Dim TransactionACExists As Boolean = False
        Dim TransactionSpecialUpdate As Boolean = False
        aclsData_Temp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase") 'CApplication.Item("crmClientDatabase")
        aclsData_Temp.JETNET_DB = Session.Item("jetnetClientDatabase") 'CApplication.Item("crmJetnetDatabase")


        If Not String.IsNullOrEmpty(Trim(Request("ac_ID"))) Then
          If IsNumeric(Trim(Request("ac_ID"))) Then
            Session.Item("ListingID") = Trim(Request("ac_ID"))
          End If
        End If
        If Not String.IsNullOrEmpty(Trim(Request("OtherID"))) Then
          If IsNumeric(Trim(Request("OtherID"))) Then
            Session.Item("OtherID") = Trim(Request("OtherID"))
          End If
        End If

        If Not String.IsNullOrEmpty(Trim(Request("source"))) Then
          Session.Item("ListingSource") = Trim(Request("source"))
        End If

        If Not String.IsNullOrEmpty(Trim(Request("activetab"))) Then
          Session.Item("ViewActiveTab") = Trim(Request("activetab"))
        End If

        If Trim(Request("autoCheckTransaction")) = "true" Then
          If IsNumeric(Trim(Request("ac_ID"))) Then
            Dim jetnet_trans_id As Long = 0
            Dim clitrans_id As Long = 0
            Dim ClientAC As Long = 0
            Session.Item("listingID") = Trim(Request("ac_ID"))
            Session.Item("ListingSource") = Trim(Request("source"))
            'We need to do several checks on this one. The first thing is - do we need to create a client record of
            'the aircraft? One could easily exist already.


            Select Case UCase(Session.Item("ListingSource"))
              Case "CLIENT"
                'Obviously this does not need an aircraft.
                TransactionACExists = True
                ClientAC = Session.Item("listingID")
              Case "JETNET"
                Dim ACTable As New DataTable
                'This might need a client aircraft. 

                ACTable = aclsData_Temp.Get_Client_Aircraft_JETNET_AC(Session.Item("listingID"))

                If Not IsNothing(ACTable) Then
                  If ACTable.Rows.Count > 0 Then
                    TransactionACExists = True
                    ClientAC = ACTable.Rows(0).Item("cliaircraft_id")
                  End If
                End If
            End Select

            If IsNumeric(Trim(Request("jtrans"))) Then
              jetnet_trans_id = Trim(Request("jtrans"))
            End If

            If IsNumeric(Trim(Request("clitrans"))) Then
              clitrans_id = Trim(Request("clitrans"))
            End If

            If jetnet_trans_id = 0 Then
              'Client only record, no creation needed, just forward to edit.
              TransactionExists = True
            Else
              Dim TransactionCheck As New DataTable
              'Go ahead and check to see if a client side already exists.
              TransactionCheck = aclsData_Temp.Get_Client_Client_Transactions(0, jetnet_trans_id)
              If Not IsNothing(TransactionCheck) Then
                If TransactionCheck.Rows.Count > 0 Then
                  clitrans_id = TransactionCheck.Rows(0).Item("clitrans_id")
                  TransactionExists = True
                Else
                  clitrans_id = 0
                End If
              Else
                clitrans_id = 0
              End If
            End If

            'From here we can figure out what we need to do.
            'if aircraft has a client and the transaction record already exists, 
            'then we just redirect to that transaction record.
            If TransactionACExists And TransactionExists Then
              Response.Redirect("edit.aspx?" & IIf(Trim(Request("from")) = "view", "from=view&", "") & "action=edit&acID=" & ClientAC & "&source=CLIENT&type=transaction&trans=" & jetnet_trans_id & "&cli_trans=" & clitrans_id, False)
              Context.ApplicationInstance.CompleteRequest()
            ElseIf TransactionACExists And TransactionExists = False Then
              'This means that the aircraft transaction doesn't exist on the client side, but the aircraft does.
              'We need to redirect to a transaction page.
              Response.Redirect("edit.aspx?" & IIf(Trim(Request("from")) = "view", "from=view&", "") & "action=edit&acID=" & ClientAC & "&source=CLIENT&type=transaction&trans=" & jetnet_trans_id, False)
              Context.ApplicationInstance.CompleteRequest()
            ElseIf TransactionACExists = False Then
              'This means we need to go ahead and create the transaction record.
              TransactionSpecialUpdate = True
            End If


          End If
        End If



        If Session.Item("isMobile") = True Then
          mobile_close.Text = "<a href='mobile_details.aspx?type=3&ac_ID=" & Session.Item("ListingID") & "'><img src=""images/cancel.gif"" alt=""Cancel"" border=""0""/></a>"
        End If

        If Not String.IsNullOrEmpty(Trim(Request("from"))) Then
          If Trim(Request("from")) = "view" Then
            If IsNumeric(Trim(Request("viewNOTEID"))) Then
              fromVIEW = True
              viewNOTEID = Trim(Request("viewNOTEID"))
            End If
          End If
        End If

        If Trim(Request("synch")) = "true" Then
          subpanel_folder.Visible = False
          aircraft_edit.Visible = False
          buttons.Visible = False
          synch.Visible = True

          Dim tempTableAC As New DataTable
          tempTableAC = aclsData_Temp.Get_Clients_Aircraft(Session.Item("ListingID"))
          If Not IsNothing(tempTableAC) Then
            If tempTableAC.Rows.Count > 0 Then
              sync_edit_text.Text = CommonAircraftFunctions.CreateHeaderLine(tempTableAC.Rows(0).Item("cliamod_make_name"), tempTableAC.Rows(0).Item("cliamod_model_name"), tempTableAC.Rows(0).Item("cliaircraft_ser_nbr"), "")
            End If
          End If


        ElseIf Trim(Request("addValueOnly")) = "true" Then
          Dim note_ID As Long = CLng(Trim(Request("viewNOTEID")))
          Dim acID As Long = CLng(Trim(Request("ac_id")))
          Dim jetnetAC As Long = CLng(Trim(Request("j_ac_id")))
          subpanel_folder.Visible = False
          aircraft_edit.Visible = False
          buttons.Visible = False

          If IsNumeric(aclsData_Temp.Insert_Client_Value_Comparable(note_ID, "F", acID, 0, "C", jetnetAC)) Then
            Dim URLstring As String = ""

            URLstring = "view_template.aspx?compare_ac_id=" & acID
            URLstring &= "&activetab=2"
            URLstring &= "&ViewID=19"
            URLstring &= "&noteID=" & note_ID

            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.location.href = '" & URLstring & "';", True)

          End If

        ElseIf Trim(Request("remove")) = "true" Then
          Remove_Aircraft()
        Else
          If Not Page.IsPostBack Then
            set_preferences()
          End If

          fill_make_type()
          If Trim(Request("action")) = "new" Then
            aircraft_edit_text.Text = "<h4 align='right'>Aircraft Add</h4>"
            deleteFunction.Visible = False
          Else
            If Not Session.Item("ListingID") Is Nothing Then
              If Not Page.IsPostBack Then
                fill_edit_data()


              End If
            End If
          End If

          If Session.Item("ListingSource") = "JETNET" Then
          ElseIf Trim(Request("action")) <> "new" Then
            add_folder_cbo.Visible = False
          End If
          If Session.Item("localSubscription").crmAerodexFlag = True Then
            aerodex_second.Visible = False
            aerodex_first.Visible = False
            for_sale_header.Visible = False
          Else
          End If

          If Trim(Request("noteCreationAC")) = "true" Then
            'If this is being sent from the note page, then go ahead and fill in the URL status parameters for the for sale block.
            FillUpForSaleParametersFromURL()

            'System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AutoGenLog", "$(document).ready(function() {runJQuery();GenerateLog();});", True)

            update_me()
          End If


          If Trim(Request("run_auto")) = "true" Or Trim(Request("auto_ac")) = "true" Or TransactionSpecialUpdate = True Then
            update_me()
          End If


        End If
      Catch ex As Exception
        error_string = "Aircraft_Edit_Template.ascx.vb - Page_Load() - " & ex.Message
        LogError(error_string)
      End Try
    End If
  End Sub
#End Region
#Region "Fill Model Dropdown/On Model Select Change"
  Function fill_make_type() As String
    fill_make_type = ""
    Try
      If Not Page.IsPostBack Then


        ac_make_type.Items.Add(New ListItem("PLEASE SELECT", ""))

        'ac_make_type.Items.Add(New ListItem("Exec Airliner", "E"))
        'ac_make_type.Items.Add(New ListItem("Jet", "J"))
        'ac_make_type.Items.Add(New ListItem("Piston", "P"))
        'ac_make_type.Items.Add(New ListItem("Turboprop", "T"))
        'ac_make_type.Items.Add(New ListItem("Turbine", "B"))
        'ac_make_type.Items.Add(New ListItem("Piston", "P"))

        aTempTable = aclsData_Temp.lookupMake_Types()
        If Not IsNothing(aTempTable) Then
          If aTempTable.Rows.Count > 0 Then
            For Each r In aTempTable.Rows
              ac_make_type.Items.Add(New ListItem(r("cliamt_name"), r("cliamt_type")))
            Next
          End If
        Else
          If aclsData_Temp.class_error <> "" Then
            error_string = aclsData_Temp.class_error
            LogError("Aircraft_Edit_Template.ascx.vb - fill_make_type()MTYPE - " & error_string)
          End If
        End If

        Airframe_type.Items.Add(New ListItem("PLEASE SELECT", ""))
        aTempTable = aclsData_Temp.Get_Client_Aircraft_Airframe_Type()
        '' check the state of the DataTable
        If Not IsNothing(aTempTable) Then
          If aTempTable.Rows.Count > 0 Then
            For Each r In aTempTable.Rows
              Airframe_type.Items.Add(New ListItem(r(1), r(0)))
            Next
          End If
        Else
          If aclsData_Temp.class_error <> "" Then
            error_string = aclsData_Temp.class_error
            LogError("Aircraft_Edit_Template.ascx.vb - fill_make_type()AFTYPE - " & error_string)
          End If
        End If
        aTempTable2 = aclsData_Temp.Get_Clients_Aircraft_Model()
        If Not IsNothing(aTempTable2) Then
          If aTempTable2.Rows.Count > 0 Then
            For Each q As DataRow In aTempTable2.Rows
              model_cbo.Items.Add(New ListItem(CStr(q("cliamod_make_name") & " " & q("cliamod_model_name")), q("cliamod_id")))
            Next
          End If
        Else
          If aclsData_Temp.class_error <> "" Then
            error_string = aclsData_Temp.class_error
            LogError("Aircraft_Edit_Propeller_Tabs.ascx.vb - fill_make_type() - " & error_string)
          End If
        End If
        model_cbo.SelectedValue = "NONE"
        model_cbo.Items.Add(New ListItem("", ""))
        model_cbo.Items.Add(New ListItem("NOT LISTED", "NOT LISTED"))
      End If
    Catch ex As Exception
      error_string = "Aircraft_Edit_Template.ascx.vb - fill_make_type() - " & ex.Message
      LogError(error_string)
    End Try
  End Function
  'Private Sub model_cbo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles model_cbo.SelectedIndexChanged
  '  Try
  '    If model_cbo.SelectedValue = "" Then

  '      model_listing.Visible = True
  '      model_cbo.Enabled = False
  '    End If
  '  Catch ex As Exception
  '    error_string = "Aircraft_Edit_Template.ascx.vb - fill_make_type() - " & ex.Message
  '    LogError(error_string)
  '  End Try
  'End Sub
#End Region
#Region "Save/Insert/Update Aircraft Functions"
  Private Sub updateFunction_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles updateFunction.Click
    If Page.IsValid Then
      update_me()
    End If
  End Sub
  Private Sub update_me()
    aclsData_Temp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase") 'CApplication.Item("crmClientDatabase")
    aclsData_Temp.JETNET_DB = Session.Item("jetnetClientDatabase") 'CApplication.Item("crmJetnetDatabase")



    'If HttpContext.Current.Session.Item("localUser").crmUser_Evo_MPM_Flag = True Then
    '  If aclsData_Temp.client_DB = "" Then
    '    aclsData_Temp.client_DB = HttpContext.Current.Session.Item("localPreferences").ServerNotesDatabaseConn
    '  End If
    'End If


    Dim idnum As Integer
    If Not Session.Item("ListingID") Is Nothing Then
      If Trim(Request("action")) <> "new" Then
        idnum = Session.Item("ListingID")
      Else
        idnum = 0
      End If
    Else
      idnum = 0
    End If
    If (idnum = 0) Then
      save_aircraft("insert")
    Else
      Select Case Session.Item("ListingSource")
        Case "JETNET"
          'check to see if this exists!
          Dim exists As Boolean = False

          aTempTable2 = aclsData_Temp.Get_Client_Aircraft_JETNET_AC(idnum)

          If Not IsNothing(aTempTable2) Then
            If aTempTable2.Rows.Count > 0 Then
              exists = True
            Else
              exists = False
            End If
          Else
            If aclsData_Temp.class_error <> "" Then
              error_string = aclsData_Temp.class_error
              LogError("Aircraft_Edit_Propeller_Tabs.ascx.vb - Page_Load() - " & error_string)
            End If
            display_error()
          End If
          If exists = False Then
            save_aircraft("insert")
          Else
            'save_aircraft("update")
          End If
        Case "CLIENT"
          'Response.Write("update")
          save_aircraft("update")
      End Select
    End If

    'System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)
  End Sub
  Function save_aircraft(ByVal x As String) As String
    save_aircraft = ""
    Try
      If x = "insert" Then

        If Trim(Request("action")) <> "new" Then
          ' If Session.Item("ListingSource") = "JETNET" Then
          ac_insert_function(2) 'insert jetnet ac
        Else
          ac_insert_function(1) 'insert client ac
        End If

      ElseIf x = "update" Then
        Dim aclsUpdate_Client_Aircraft As New clsClient_Aircraft

        Dim model_id As Integer = 0
        If model_cbo.SelectedValue = "" Or model_cbo.SelectedValue = "NOT LISTED" Then

          Dim aclsInsert_Client_Aircraft_Model As New clsClient_Aircraft_Model
          aclsInsert_Client_Aircraft_Model.cliamod_airframe_type = Airframe_type.SelectedValue
          aclsInsert_Client_Aircraft_Model.cliamod_make_name = ac_make.Text
          aclsInsert_Client_Aircraft_Model.cliamod_make_type = ac_make_type.SelectedValue
          aclsInsert_Client_Aircraft_Model.cliamod_manufacturer_name = ac_manu_name.Text
          aclsInsert_Client_Aircraft_Model.cliamod_model_name = ac_model.Text
          aTempTable = aclsData_Temp.Get_Clients_Aircraft_Model_Make_Model(ac_make.Text, ac_model.Text)

          If Not IsNothing(aTempTable) Then
            If aTempTable.Rows.Count > 0 Then
              For Each t As DataRow In aTempTable.Rows
                model_id = t("cliamod_id")
              Next
            Else
              model_id = aclsData_Temp.Insert_Client_Aircraft_Model(aclsInsert_Client_Aircraft_Model) 'model doesn't exist - insert it
            End If
          Else
            If aclsData_Temp.class_error <> "" Then
              error_string = aclsData_Temp.class_error
              LogError("Aircraft_Edit_Template.ascx.vb - save_aircraft() - " & error_string)
            End If
            display_error()
          End If


          If model_id = 0 Then
            If aclsData_Temp.class_error <> "" Then
              error_string = aclsData_Temp.class_error
              LogError("Aircraft_Edit_Template.ascx.vb - save_aircraft() - " & error_string)
            End If
            display_error()
          End If
        Else
          model_id = model_cbo.SelectedValue
        End If

        aclsUpdate_Client_Aircraft.cliaircraft_value_description = cliaircraft_value_description_text.Text.ToString

        'Update the custom fields.
        aclsUpdate_Client_Aircraft.cliaircraft_custom_1 = ac_cat1.Text
        aclsUpdate_Client_Aircraft.cliaircraft_custom_2 = ac_cat2.Text
        aclsUpdate_Client_Aircraft.cliaircraft_custom_3 = ac_cat3.Text
        aclsUpdate_Client_Aircraft.cliaircraft_custom_4 = ac_cat4.Text
        aclsUpdate_Client_Aircraft.cliaircraft_custom_5 = ac_cat5.Text
        aclsUpdate_Client_Aircraft.cliaircraft_custom_6 = ac_cat6.Text
        aclsUpdate_Client_Aircraft.cliaircraft_custom_7 = ac_cat7.Text
        aclsUpdate_Client_Aircraft.cliaircraft_custom_8 = ac_cat8.Text
        aclsUpdate_Client_Aircraft.cliaircraft_custom_9 = ac_cat9.Text
        aclsUpdate_Client_Aircraft.cliaircraft_custom_10 = ac_cat10.Text


        aclsUpdate_Client_Aircraft.cliaircraft_action_date = Now()
        aclsUpdate_Client_Aircraft.cliaircraft_ser_nbr_sort = serial_sort.Text
        aclsUpdate_Client_Aircraft.cliaircraft_ser_nbr = serial.Text
        aclsUpdate_Client_Aircraft.cliaircraft_cliamod_id = model_id
        aclsUpdate_Client_Aircraft.cliaircraft_delivery = delivery.Text

        If Session.Item("localSubscription").crmAerodexFlag = True Then
        Else
          aclsUpdate_Client_Aircraft.cliaircraft_asking_wordage = asking_wordage.Text
          aclsUpdate_Client_Aircraft.cliaircraft_exclusive_flag = ac_exclusive.SelectedValue
          aclsUpdate_Client_Aircraft.cliaircraft_forsale_flag = ac_sale.SelectedValue


          aclsUpdate_Client_Aircraft.cliaircraft_lease_flag = ac_lease.SelectedValue

          If ac_sale.SelectedValue = "Y" Then
            If asking_price.Text <> "" Then
              aclsUpdate_Client_Aircraft.cliaircraft_asking_price = IIf(IsNumeric(clsGeneral.clsGeneral.FormatMKDollarValue(asking_price.Text)), clsGeneral.clsGeneral.FormatMKDollarValue(asking_price.Text), 0)
            Else
              aclsUpdate_Client_Aircraft.cliaircraft_asking_price = 0
            End If
            aclsUpdate_Client_Aircraft.cliaircraft_broker_price = IIf(IsNumeric(clsGeneral.clsGeneral.FormatMKDollarValue(broker_price.Text)), clsGeneral.clsGeneral.FormatMKDollarValue(broker_price.Text), 0)

            aclsUpdate_Client_Aircraft.cliaircraft_est_price = IIf(IsNumeric(clsGeneral.clsGeneral.FormatMKDollarValue(est_price.Text)), clsGeneral.clsGeneral.FormatMKDollarValue(est_price.Text), 0)
          Else
            aclsUpdate_Client_Aircraft.cliaircraft_asking_price = 0
            aclsUpdate_Client_Aircraft.cliaircraft_est_price = 0
            aclsUpdate_Client_Aircraft.cliaircraft_broker_price = 0
          End If


          If IsDate(date_listed.Text) Then
            aclsUpdate_Client_Aircraft.cliaircraft_date_listed = date_listed.Text
          End If
          If IsDate(date_purchased.Text) Then
            aclsUpdate_Client_Aircraft.cliaircraft_date_purchased = date_purchased.Text
          End If

        End If


        aclsUpdate_Client_Aircraft.cliaircraft_reg_nbr = reg.Text

        If ac_status_for_sale.SelectedValue <> "" Then
          aclsUpdate_Client_Aircraft.cliaircraft_status = ac_status_for_sale.SelectedValue
        ElseIf ac_status_not_for_sale.SelectedValue <> "" Then
          aclsUpdate_Client_Aircraft.cliaircraft_status = ac_status_not_for_sale.SelectedValue
        ElseIf ac_status_not_for_sale_withdrawn.SelectedValue <> "" Then
          aclsUpdate_Client_Aircraft.cliaircraft_status = ac_status_not_for_sale_withdrawn.SelectedValue
        End If


        aclsUpdate_Client_Aircraft.cliaircraft_user_id = IIf(Not IsNumeric(Session.Item("localUser").crmLocalUserID), 0, Session.Item("localUser").crmLocalUserID)
        aclsUpdate_Client_Aircraft.cliaircraft_year_mfr = year_manufactured.Text
        aclsUpdate_Client_Aircraft.cliaircraft_action_date = Now()
        aclsUpdate_Client_Aircraft.cliaircraft_lifecycle = IIf(Not IsNumeric(lifecycle_list.SelectedValue), 0, lifecycle_list.SelectedValue)
        aclsUpdate_Client_Aircraft.cliaircraft_ownership = ownership_list.SelectedValue
        aclsUpdate_Client_Aircraft.cliaircraft_jetnet_ac_id = IIf(Not IsNumeric(jetnet_ac.Text), 0, jetnet_ac.Text)
        aclsUpdate_Client_Aircraft.cliaircraft_aport_iata_code = iata_code.Text
        aclsUpdate_Client_Aircraft.cliaircraft_aport_icao_code = icao_code.Text
        aclsUpdate_Client_Aircraft.cliaircraft_aport_name = airport_name.Text
        aclsUpdate_Client_Aircraft.cliaircraft_aport_state = airport_state.Text
        aclsUpdate_Client_Aircraft.cliaircraft_aport_country = airport_country.Text
        aclsUpdate_Client_Aircraft.cliaircraft_country_of_registration = reg_country.Text
        aclsUpdate_Client_Aircraft.cliaircraft_aport_city = aiport_city.Text
        aclsUpdate_Client_Aircraft.cliaircraft_aport_private = airport_private.SelectedValue
        aclsUpdate_Client_Aircraft.cliaircraft_id = IIf(Not IsNumeric(Session.Item("ListingID")), 0, Session.Item("ListingID"))
        aclsUpdate_Client_Aircraft.cliaircraft_prev_reg_nbr = previous_registration.Text
        aclsUpdate_Client_Aircraft.cliaircraft_alt_ser_nbr = alternate_serial.Text
        aclsUpdate_Client_Aircraft.cliaircraft_year_dlv = year_dlv.Text
        aclsUpdate_Client_Aircraft.cliaircraft_new_flag = new_list.SelectedValue

        'Airframe Total Hours
        'Must be numeric, if it isn't - it gets set as null
        If IsNumeric(ac_airframe_total_hours.Text) Then
          aclsUpdate_Client_Aircraft.cliaircraft_airframe_total_hours = ac_airframe_total_hours.Text
        End If

        'Airframe total landings.
        'Must be numeric, if it isn't - it gets set as null
        If IsNumeric(ac_airframe_total_landings.Text) Then
          aclsUpdate_Client_Aircraft.cliaircraft_airframe_total_landings = ac_airframe_total_landings.Text
        End If

        'Aircraft date engines time of.
        'Must be a date, if it isn't, it doesn't get set and goes in as null.
        If Not String.IsNullOrEmpty(ac_date_engine_times_as_of.Text) Then
          If IsDate(ac_date_engine_times_as_of.Text) Then
            aclsUpdate_Client_Aircraft.cliaircraft_date_engine_times_as_of = ac_date_engine_times_as_of.Text
          End If
        End If


        aTempTable = aclsData_Temp.Get_Clients_Aircraft(Session.Item("ListingID"))
        If Not IsNothing(aTempTable) Then
          If aTempTable.Rows.Count > 0 Then
            For Each R As DataRow In aTempTable.Rows

              If Not IsDBNull(R("cliaircraft_airframe_maintenance_program")) Then
                aclsUpdate_Client_Aircraft.cliaircraft_airframe_maintenance_program = IIf(Not IsNumeric(R("cliaircraft_airframe_maintenance_program")), 0, R("cliaircraft_airframe_maintenance_program"))
              End If

              If Not IsDBNull(R("cliaircraft_airframe_maintenance_tracking_program")) Then
                aclsUpdate_Client_Aircraft.cliaircraft_airframe_maintenance_tracking_program = IIf(Not IsNumeric(R("cliaircraft_airframe_maintenance_tracking_program")), 0, R("cliaircraft_airframe_maintenance_tracking_program"))
              End If


              If Not IsDBNull(R("cliaircraft_ac_maintained")) Then
                aclsUpdate_Client_Aircraft.cliaircraft_ac_maintained = IIf(Not IsDBNull(R("cliaircraft_ac_maintained")), R("cliaircraft_ac_maintained"), "")
              End If

              If Not IsDBNull(R("cliaircraft_apu_model_name")) Then
                aclsUpdate_Client_Aircraft.cliaircraft_apu_model_name = R("cliaircraft_apu_model_name")
              End If
              If Not IsDBNull(R("cliaircraft_apu_ser_nbr")) Then
                aclsUpdate_Client_Aircraft.cliaircraft_apu_ser_nbr = R("cliaircraft_apu_ser_nbr")
              End If
              If Not IsDBNull(R("cliaircraft_apu_ttsn_hours")) Then
                aclsUpdate_Client_Aircraft.cliaircraft_apu_ttsn_hours = IIf(Not IsNumeric(R("cliaircraft_apu_ttsn_hours")), 0, R("cliaircraft_apu_ttsn_hours"))
              End If
              If Not IsDBNull(R("cliaircraft_apu_tsoh_hours")) Then
                aclsUpdate_Client_Aircraft.cliaircraft_apu_tsoh_hours = IIf(Not IsNumeric(R("cliaircraft_apu_tsoh_hours")), 0, R("cliaircraft_apu_tsoh_hours"))
              End If
              If Not IsDBNull(R("cliaircraft_apu_tshi_hours")) Then
                aclsUpdate_Client_Aircraft.cliaircraft_apu_tshi_hours = IIf(Not IsNumeric(R("cliaircraft_apu_tshi_hours")), 0, R("cliaircraft_apu_tshi_hours"))
              End If
              If Not IsDBNull(R("cliaircraft_apu_maintance_program")) Then
                aclsUpdate_Client_Aircraft.cliaircraft_apu_maintance_program = R("cliaircraft_apu_maintance_program")
              End If
              If Not IsDBNull(R("cliaircraft_damage_flag")) Then
                aclsUpdate_Client_Aircraft.cliaircraft_damage_flag = R("cliaircraft_damage_flag")
              End If
              If Not IsDBNull(R("cliaircraft_damage_history_notes")) Then
                aclsUpdate_Client_Aircraft.cliaircraft_damage_history_notes = R("cliaircraft_damage_history_notes")
              End If
              If Not IsDBNull(R("cliaircraft_interior_rating")) Then
                aclsUpdate_Client_Aircraft.cliaircraft_interior_rating = R("cliaircraft_interior_rating")
              End If

              If Not IsDBNull(R("cliaircraft_interior_month_year")) Then
                aclsUpdate_Client_Aircraft.cliaircraft_interior_month_year = R("cliaircraft_interior_month_year")
              End If
              If Not IsDBNull(R("cliaircraft_interior_doneby_name")) Then
                aclsUpdate_Client_Aircraft.cliaircraft_interior_doneby_name = R("cliaircraft_interior_doneby_name")
              End If
              If Not IsDBNull(R("cliaircraft_interior_config_name")) Then
                aclsUpdate_Client_Aircraft.cliaircraft_interior_config_name = R("cliaircraft_interior_config_name")
              End If
              If Not IsDBNull(R("cliaircraft_exterior_rating")) Then
                aclsUpdate_Client_Aircraft.cliaircraft_exterior_rating = R("cliaircraft_exterior_rating")
              End If
              If Not IsDBNull(R("cliaircraft_exterior_month_year")) Then
                aclsUpdate_Client_Aircraft.cliaircraft_exterior_month_year = R("cliaircraft_exterior_month_year")
              End If
              If Not IsDBNull(R("cliaircraft_exterior_doneby_name")) Then
                aclsUpdate_Client_Aircraft.cliaircraft_exterior_doneby_name = R("cliaircraft_exterior_doneby_name")
              End If
              If Not IsDBNull(R("cliaircraft_exterior_month_year")) Then
                aclsUpdate_Client_Aircraft.cliaircraft_exterior_month_year = R("cliaircraft_exterior_month_year")
              End If
              If Not IsDBNull(R("cliaircraft_passenger_count")) Then
                aclsUpdate_Client_Aircraft.cliaircraft_passenger_count = IIf(Not IsNumeric(R("cliaircraft_passenger_count")), 0, R("cliaircraft_passenger_count"))
              End If
              If Not IsDBNull(R("cliaircraft_confidential_notes")) Then
                aclsUpdate_Client_Aircraft.cliaircraft_confidential_notes = R("cliaircraft_confidential_notes")
              End If

            Next
          End If
        End If
        'Response.Write(aclsUpdate_Client_Aircraft.ClassInfo(aclsUpdate_Client_Aircraft))

        If aclsData_Temp.Update_Client_Aircraft(aclsUpdate_Client_Aircraft) = 1 Then

          If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO Then
            Call commonLogFunctions.Log_User_Event_Data("UserStatistics", "Aircraft_Edit_Template CRM Update: AC_ID = " & jetnet_ac.Text & " Client Record: " & Session.Item("ListingID"), Nothing, 0, 0, 0, 0, 0, CLng(jetnet_ac.Text), 0)
          End If


          SaveAircraftNoteIfNeeded(aclsUpdate_Client_Aircraft)

          If Session.Item("isMobile") = True Then
            Response.Redirect("Mobile_Details.aspx?type=" & Session.Item("Listing") & "&ac_id=" & Session.Item("ListingID") & "&source=" & Session.Item("ListingSource") & "&edited=ac", False)
          End If

          Dim url As String = "details.aspx"


          If fromVIEW = True Then
            If viewNOTEID <> 0 Then
              url = "view_template.aspx?ViewID=19&noteID=" & viewNOTEID & "&noMaster=false" & IIf(Not String.IsNullOrEmpty(Trim(Request("extra_amod"))), "&extra_amod=" & Trim(Request("extra_amod")), "")
            Else
              url = "view_template.aspx?ViewID=1&noMaster=false&amod_id=" & jetnet_amod_id.Text & IIf(Not String.IsNullOrEmpty(Trim(Request("extra_amod"))), "&extra_amod=" & Trim(Request("extra_amod")), "")
            End If

            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "if (window.opener.location.pathname.toUpperCase().search('VIEW_TEMPLATE') == 1){window.opener.location = '" & url & "';} else {window.opener.location.href = window.opener.location.href;}", True)
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)
          ElseIf Trim(Request("from")) = "aircraftDetails" Then
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.location = 'DisplayAircraftDetail.aspx?acID=" & Session.Item("ListingID").ToString & "&source=" & Session.Item("ListingSource") & "';", True)
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)
          Else

            If Trim(Request("noteCreationAC")) <> "true" Then
              System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.location = '" & url & "';", True)
            End If
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)


          End If




          'System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.location = '" & url & "';", True)
          'System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)
        Else
          If aclsData_Temp.class_error <> "" Then
            error_string = aclsData_Temp.class_error
            LogError("Aircraft_Edit_Template.ascx.vb - save_aircraft() - " & error_string)
          End If
          display_error()
        End If
      End If
    Catch ex As Exception
      error_string = "Aircraft_Edit_Template.ascx.vb - save_aircraft() - " & ex.Message
      LogError(error_string)
    End Try
  End Function

  Private Sub SaveAircraftNoteIfNeeded(ByRef AircraftClass As clsClient_Aircraft)

    'Right now we need to check and see if we need to log this change based on whether or not the flag is set in the cliuser table.
    If Session.Item("localUser").crmUser_Autolog_Flag Then
      If Not String.IsNullOrEmpty(logGenerated.Text) Then
        'Save note:
        Dim clsNote As New clsLocal_Notes
        Dim GeneralCategory As Integer = 25
        Dim CategoryTable As New DataTable
        Dim CategoryObject As New Object
        clsNote.lnote_action_date = Now()
        clsNote.lnote_client_ac_id = AircraftClass.cliaircraft_id
        clsNote.lnote_jetnet_ac_id = AircraftClass.cliaircraft_jetnet_ac_id
        clsNote.lnote_client_amod_id = AircraftClass.cliaircraft_cliamod_id
        clsNote.lnote_entry_date = Now()

        If InStr(logGenerated.Text, "undefined") > 0 And InStr(logGenerated.Text, "Asking Price") > 0 Then
          logGenerated.Text = Replace(logGenerated.Text, "undefined", "Make Offer")
          clsNote.lnote_note = Replace(logGenerated.Text, "*", "&#13;&#10;")
        Else
          clsNote.lnote_note = Replace(logGenerated.Text, "*", "&#13;&#10;")
        End If

        clsNote.lnote_user_id = Session.Item("localUser").crmLocalUserID
        clsNote.lnote_user_login = Session.Item("localUser").crmLocalUserID
        clsNote.lnote_user_name = Left(Session.Item("localUser").crmLocalUserFirstName & " " & Session.Item("localUser").crmLocalUserLastName, 15)


        CategoryTable = aclsData_Temp.Get_Client_Note_Document_Category("N")
        CategoryObject = clsGeneral.clsGeneral.Category_Belongs_To(0, True, "GENERAL", CategoryTable)

        If Not IsNothing(CategoryObject) Then
          If IsNumeric(CategoryObject) Then
            GeneralCategory = CategoryObject
          End If
        End If

        CategoryTable.Dispose()
        CategoryTable = New DataTable

        clsNote.lnote_notecat_key = GeneralCategory
        clsNote.lnote_clipri_ID = "1"
        clsNote.lnote_status = "A"

        aclsData_Temp.Insert_Note(clsNote)
      End If
    End If

  End Sub


  Private Sub ac_insert_function(ByVal source As Integer)
    Try
      Dim aclsInsert_Client_Aircraft As New clsClient_Aircraft
      ''This inserts the model
      Dim model_id As Integer = 0
      If model_cbo.SelectedValue = "" Or model_cbo.SelectedValue = "NOT LISTED" Then

        Dim aclsInsert_Client_Aircraft_Model As New clsClient_Aircraft_Model
        aclsInsert_Client_Aircraft_Model.cliamod_airframe_type = Airframe_type.SelectedValue
        aclsInsert_Client_Aircraft_Model.cliamod_make_name = ac_make.Text
        aclsInsert_Client_Aircraft_Model.cliamod_make_type = ac_make_type.SelectedValue
        aclsInsert_Client_Aircraft_Model.cliamod_manufacturer_name = ac_manu_name.Text
        aclsInsert_Client_Aircraft_Model.cliamod_model_name = ac_model.Text

        If jetnet_amod_id.Text = "" Then

        Else
          aclsInsert_Client_Aircraft_Model.cliamod_jetnet_amod_id = jetnet_amod_id.Text
        End If



        aTempTable = aclsData_Temp.Get_Clients_Aircraft_Model_Make_Model(ac_make.Text, ac_model.Text)

        If Not IsNothing(aTempTable) Then
          If aTempTable.Rows.Count > 0 Then
            For Each t As DataRow In aTempTable.Rows
              model_id = t("cliamod_id")
            Next
          Else
            model_id = aclsData_Temp.Insert_Client_Aircraft_Model(aclsInsert_Client_Aircraft_Model) 'model doesn't exist - insert it
          End If
        Else
          If aclsData_Temp.class_error <> "" Then
            error_string = aclsData_Temp.class_error
            LogError("Aircraft_Edit_Template.ascx.vb - save_aircraft() - " & error_string)
          End If
          display_error()
        End If

        If model_id = 0 Then
          display_error()
        End If
      Else
        model_id = model_cbo.SelectedValue
      End If

      Dim aError, enddate, enddated As String
      enddate = DateAdd(DateInterval.Minute, 30, Now())
      enddated = Year(enddate) & "-" & Month(enddate) & "-" & (Day(enddate)) & " " & FormatDateTime(enddate, 4) & ":" & Second(enddate)


      aclsInsert_Client_Aircraft.cliaircraft_action_date = enddated
      aclsInsert_Client_Aircraft.cliaircraft_ser_nbr = serial.Text

      If serial_sort.Text = "" Then
        aclsInsert_Client_Aircraft.cliaircraft_ser_nbr_sort = aclsInsert_Client_Aircraft.cliaircraft_ser_nbr.PadLeft(24, "0")
      Else
        aclsInsert_Client_Aircraft.cliaircraft_ser_nbr_sort = serial_sort.Text
      End If

      If Session.Item("localSubscription").crmAerodexFlag = True Then
      Else
        If asking_price.Text <> "" Then
          aclsInsert_Client_Aircraft.cliaircraft_asking_price = IIf(IsNumeric(clsGeneral.clsGeneral.FormatMKDollarValue(asking_price.Text)), clsGeneral.clsGeneral.FormatMKDollarValue(asking_price.Text), 0)
        Else
          aclsInsert_Client_Aircraft.cliaircraft_asking_price = 0
        End If
        aclsInsert_Client_Aircraft.cliaircraft_asking_wordage = asking_wordage.Text
        aclsInsert_Client_Aircraft.cliaircraft_exclusive_flag = ac_exclusive.SelectedValue
        aclsInsert_Client_Aircraft.cliaircraft_forsale_flag = ac_sale.SelectedValue
        aclsInsert_Client_Aircraft.cliaircraft_lease_flag = ac_lease.SelectedValue
        If IsDate(date_listed.Text) Then
          aclsInsert_Client_Aircraft.cliaircraft_date_listed = date_listed.Text
        End If
        If IsDate(date_purchased.Text) Then
          aclsInsert_Client_Aircraft.cliaircraft_date_purchased = date_purchased.Text
        End If
        If est_price.Text <> "" Then
          aclsInsert_Client_Aircraft.cliaircraft_est_price = IIf(IsNumeric(clsGeneral.clsGeneral.FormatMKDollarValue(est_price.Text)), clsGeneral.clsGeneral.FormatMKDollarValue(est_price.Text), 0)
        Else
          aclsInsert_Client_Aircraft.cliaircraft_est_price = 0
        End If
        If broker_price.Text <> "" Then
          aclsInsert_Client_Aircraft.cliaircraft_broker_price = IIf(IsNumeric(clsGeneral.clsGeneral.FormatMKDollarValue(broker_price.Text)), clsGeneral.clsGeneral.FormatMKDollarValue(broker_price.Text), 0)
        Else
          aclsInsert_Client_Aircraft.cliaircraft_broker_price = 0
        End If
      End If

      aclsInsert_Client_Aircraft.cliaircraft_value_description = cliaircraft_value_description_text.Text.ToString
      aclsInsert_Client_Aircraft.cliaircraft_cliamod_id = model_id
      aclsInsert_Client_Aircraft.cliaircraft_delivery = delivery.Text
      aclsInsert_Client_Aircraft.cliaircraft_reg_nbr = reg.Text

      If ac_status_for_sale.SelectedValue <> "" Then
        aclsInsert_Client_Aircraft.cliaircraft_status = ac_status_for_sale.SelectedValue
      ElseIf ac_status_not_for_sale.SelectedValue <> "" Then
        aclsInsert_Client_Aircraft.cliaircraft_status = ac_status_not_for_sale.SelectedValue
      ElseIf ac_status_not_for_sale_withdrawn.SelectedValue <> "" Then
        aclsInsert_Client_Aircraft.cliaircraft_status = ac_status_not_for_sale_withdrawn.SelectedValue
      End If

      aclsInsert_Client_Aircraft.cliaircraft_user_id = Session.Item("localUser").crmLocalUserID
      aclsInsert_Client_Aircraft.cliaircraft_year_mfr = year_manufactured.Text


      'Update the custom fields.
      aclsInsert_Client_Aircraft.cliaircraft_custom_1 = ac_cat1.Text
      aclsInsert_Client_Aircraft.cliaircraft_custom_2 = ac_cat2.Text
      aclsInsert_Client_Aircraft.cliaircraft_custom_3 = ac_cat3.Text
      aclsInsert_Client_Aircraft.cliaircraft_custom_4 = ac_cat4.Text
      aclsInsert_Client_Aircraft.cliaircraft_custom_5 = ac_cat5.Text
      aclsInsert_Client_Aircraft.cliaircraft_custom_6 = ac_cat6.Text
      aclsInsert_Client_Aircraft.cliaircraft_custom_7 = ac_cat7.Text
      aclsInsert_Client_Aircraft.cliaircraft_custom_8 = ac_cat8.Text
      aclsInsert_Client_Aircraft.cliaircraft_custom_9 = ac_cat9.Text
      aclsInsert_Client_Aircraft.cliaircraft_custom_10 = ac_cat10.Text


      If source = 2 Then
        aclsInsert_Client_Aircraft.cliaircraft_jetnet_ac_id = CLng(Session.Item("ListingID"))
      End If

      aclsInsert_Client_Aircraft.cliaircraft_action_date = Now()
      aclsInsert_Client_Aircraft.cliaircraft_lifecycle = lifecycle_list.SelectedValue
      aclsInsert_Client_Aircraft.cliaircraft_ownership = ownership_list.SelectedValue
      aclsInsert_Client_Aircraft.cliaircraft_year_dlv = year_dlv.Text
      aclsInsert_Client_Aircraft.cliaircraft_aport_iata_code = iata_code.Text
      aclsInsert_Client_Aircraft.cliaircraft_aport_icao_code = icao_code.Text
      aclsInsert_Client_Aircraft.cliaircraft_aport_name = airport_name.Text
      aclsInsert_Client_Aircraft.cliaircraft_aport_state = airport_state.Text
      aclsInsert_Client_Aircraft.cliaircraft_aport_country = airport_country.Text
      aclsInsert_Client_Aircraft.cliaircraft_country_of_registration = reg_country.Text
      aclsInsert_Client_Aircraft.cliaircraft_aport_city = aiport_city.Text
      aclsInsert_Client_Aircraft.cliaircraft_aport_private = airport_private.SelectedValue

      aclsInsert_Client_Aircraft.cliaircraft_prev_reg_nbr = previous_registration.Text
      aclsInsert_Client_Aircraft.cliaircraft_alt_ser_nbr = alternate_serial.Text
      aclsInsert_Client_Aircraft.cliaircraft_new_flag = new_list.SelectedValue


      'Airframe total hours - Must be Numeric. If it isn't, don't set and it will go in as null.
      If Not IsDBNull(ac_airframe_total_hours.Text) Then
        If IsNumeric(ac_airframe_total_hours.Text) Then
          aclsInsert_Client_Aircraft.cliaircraft_airframe_total_hours = ac_airframe_total_hours.Text
        End If
      End If

      'total landings - Must be Numeric. If it isn't, don't set and it will go in as null.
      If Not IsDBNull(ac_airframe_total_landings.Text) Then
        If IsNumeric(ac_airframe_total_landings.Text) Then
          aclsInsert_Client_Aircraft.cliaircraft_airframe_total_landings = ac_airframe_total_landings.Text
        End If
      End If

      'engines times - Must be a date. If it isn't, don't set and it will go in as null.
      If Not IsDBNull(ac_date_engine_times_as_of.Text) Then
        If IsDate(ac_date_engine_times_as_of.Text) Then
          aclsInsert_Client_Aircraft.cliaircraft_date_engine_times_as_of = ac_date_engine_times_as_of.Text
        End If
      End If


      If source = 2 Then
        aError = ""
        aTempTable = aclsData_Temp.GetJETNET_Aircraft_Details_AC_ID(Session.Item("ListingID"), aError)
        If Not IsNothing(aTempTable) Then
          If aTempTable.Rows.Count > 0 Then
            For Each R As DataRow In aTempTable.Rows

              If Not IsDBNull(R("ac_airframe_maintenance_program")) Then
                aclsInsert_Client_Aircraft.cliaircraft_airframe_maintenance_program = R("ac_airframe_maintenance_program")
              End If

              If Not IsDBNull(R("ac_airframe_maintenance_tracking_program")) Then
                aclsInsert_Client_Aircraft.cliaircraft_airframe_maintenance_tracking_program = R("ac_airframe_maintenance_tracking_program")
              End If


              If Not IsDBNull(R("ac_maintained")) Then
                aclsInsert_Client_Aircraft.cliaircraft_ac_maintained = IIf(Not IsDBNull(R("ac_maintained")), R("ac_maintained"), "")
              End If

              If Not IsDBNull(R("ac_apu_model_name")) Then
                aclsInsert_Client_Aircraft.cliaircraft_apu_model_name = R("ac_apu_model_name")
              End If
              If Not IsDBNull(R("ac_apu_ser_nbr")) Then
                aclsInsert_Client_Aircraft.cliaircraft_apu_ser_nbr = R("ac_apu_ser_nbr")
              End If
              If Not IsDBNull(R("ac_apu_ttsn_hours")) Then
                aclsInsert_Client_Aircraft.cliaircraft_apu_ttsn_hours = R("ac_apu_ttsn_hours")
              End If
              If Not IsDBNull(R("ac_apu_tsoh_hours")) Then
                aclsInsert_Client_Aircraft.cliaircraft_apu_tsoh_hours = R("ac_apu_tsoh_hours")
              End If
              If Not IsDBNull(R("ac_apu_tshi_hours")) Then
                aclsInsert_Client_Aircraft.cliaircraft_apu_tshi_hours = R("ac_apu_tshi_hours")
              End If
              If Not IsDBNull(R("ac_apu_maintance_program")) Then
                aclsInsert_Client_Aircraft.cliaircraft_apu_maintance_program = R("ac_apu_maintance_program")
              End If
              If Not IsDBNull(R("ac_damage_flag")) Then
                aclsInsert_Client_Aircraft.cliaircraft_damage_flag = R("ac_damage_flag")
              End If
              If Not IsDBNull(R("ac_damage_history_notes")) Then
                aclsInsert_Client_Aircraft.cliaircraft_damage_history_notes = R("ac_damage_history_notes")
              End If
              If Not IsDBNull(R("ac_interior_rating")) Then
                aclsInsert_Client_Aircraft.cliaircraft_interior_rating = R("ac_interior_rating")
              End If

              If Not IsDBNull(R("ac_interior_month_year")) Then
                aclsInsert_Client_Aircraft.cliaircraft_interior_month_year = R("ac_interior_month_year")
              End If
              If Not IsDBNull(R("ac_interior_doneby_name")) Then
                aclsInsert_Client_Aircraft.cliaircraft_interior_doneby_name = R("ac_interior_doneby_name")
              End If
              If Not IsDBNull(R("ac_interior_config_name")) Then
                aclsInsert_Client_Aircraft.cliaircraft_interior_config_name = R("ac_interior_config_name")
              End If
              If Not IsDBNull(R("ac_exterior_rating")) Then
                aclsInsert_Client_Aircraft.cliaircraft_exterior_rating = R("ac_exterior_rating")
              End If
              If Not IsDBNull(R("ac_exterior_month_year")) Then
                aclsInsert_Client_Aircraft.cliaircraft_exterior_month_year = R("ac_exterior_month_year")
              End If
              If Not IsDBNull(R("ac_exterior_doneby_name")) Then
                aclsInsert_Client_Aircraft.cliaircraft_exterior_doneby_name = R("ac_exterior_doneby_name")
              End If
              If Not IsDBNull(R("ac_exterior_month_year")) Then
                aclsInsert_Client_Aircraft.cliaircraft_exterior_month_year = R("ac_exterior_month_year")
              End If
              If Not IsDBNull(R("ac_passenger_count")) Then
                aclsInsert_Client_Aircraft.cliaircraft_passenger_count = R("ac_passenger_count")
              End If
              If Not IsDBNull(R("ac_confidential_notes")) Then
                aclsInsert_Client_Aircraft.cliaircraft_confidential_notes = R("ac_confidential_notes")
              End If

            Next
          End If
        Else
          If aclsData_Temp.class_error <> "" Then
            error_string = aclsData_Temp.class_error
            LogError("Aircraft_Edit_Template.ascx.vb - ac_insert_function() - " & error_string)
          End If
          display_error()
        End If
      End If



      'Response.Write(aclsInsert_Client_Aircraft.ClassInfo(aclsInsert_Client_Aircraft))
      Dim new_id As Integer = aclsData_Temp.Insert_Client_Aircraft(aclsInsert_Client_Aircraft)
      'Dim new_id As Integer = 109
      'Response.Write(new_id)
      If source = 2 Then

        SaveAircraftNoteIfNeeded(aclsInsert_Client_Aircraft)

        fill_avionics(new_id, Session.Item("ListingID"))
        fill_ac_details(new_id, "", Session.Item("ListingID"))
        fill_engine_details(new_id, 0, Session.Item("ListingID"))
        fill_features(new_id, Session.Item("ListingID"))
        fill_propeller(new_id)
        Fill_Companies(new_id)


        If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO Then
          Call commonLogFunctions.Log_User_Event_Data("UserStatistics", "Aircraft_Edit_Template CRM Insert: AC_ID = " & Session.Item("ListingID"), Nothing, 0, 0, 0, 0, 0, CLng(Session.Item("ListingID")), 0)
        End If

        If new_id <> 0 And Session.Item("listingID") <> 0 Then
          Dim response_id As Integer = aclsData_Temp.Update_Note_When_Jetnet_Made_Client(new_id, Session.Item("ListingID"))

          If response_id = 1 Then
            '  Dim x As Integer
          End If
        End If
      ElseIf source = 1 Then 'this means this is a client aircraft and we need to add a temporary owner.
        'Check to see if there's an owner not defined company:
        Dim OwnerID As Long = 0
        Dim checkTable As New DataTable
        checkTable = aclsData_Temp.FindNotDefinedOwnerCompany()

        If Not IsNothing(checkTable) Then
          If checkTable.Rows.Count > 0 Then
            OwnerID = checkTable.Rows(0).Item("comp_id")
          End If
        End If

        If OwnerID = 0 Then 'Owner not defined company needs to be inserted:
          OwnerID = InsertOwnerNotDefinedCompany()
        End If

        If OwnerID > 0 Then
          'add a reference to owner.
          Dim clsAircraftReference As New clsClient_Aircraft_Reference
          clsAircraftReference.cliacref_cliac_id = new_id
          clsAircraftReference.cliacref_comp_id = OwnerID
          clsAircraftReference.cliacref_contact_type = "00"
          clsAircraftReference.cliacref_jetnet_ac_id = 0
          If aclsData_Temp.Insert_Client_Aircraft_Reference(clsAircraftReference) = True Then
            'Owner has been inserted.
          End If
        End If
      End If
      update_text.Text = "Your Aircraft has been saved"

      'If add_folder_cbo.SelectedValue <> 0 Then
      '    AddToSubFolder(new_id)
      'End If

      ' ADDED MSW 4/10/17
      aclsData_Temp.Create_Aircraft_Maintenance(new_id)

      Session.Item("ListingID") = new_id
      Session.Item("ListingSource") = "CLIENT"
      Dim url As String = "details.aspx"
      If Session.Item("isMobile") = True Then
        Response.Redirect("Mobile_Details.aspx?type=" & Session.Item("Listing") & "&ac_id=" & Session.Item("ListingID") & "&source=" & Session.Item("ListingSource") & "&edited=ac", False)
      End If


      If Trim(Request("auto_ac")) = "true" And Trim(Request("trans")) <> "" Then
        Dim trans_url As String = "edit.aspx?action=edit&type=transaction&trans=" & Trim(Request("trans"))
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.location = '" & url & "';", True)
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Change_Window", "window.location.href = '" & trans_url & "';", True)
      ElseIf Trim(Request("auto_ac")) = "true" And Trim(Request("trans")) = "" And Trim(Request("from")) <> "view" Then
        Dim trans_url As String = "edit.aspx?action=edit&type=transaction&new=true"

        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.location = '" & url & "';", True)
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Change_Window", "window.location.href = '" & trans_url & "';", True)
      ElseIf Trim(Request("noteCreationAC")) = "true" Then
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)
      ElseIf Trim(Request("from")) = "aircraftDetails" Then
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.location = 'DisplayAircraftDetail.aspx?acID=" & new_id.ToString & "&source=CLIENT';", True)
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)
      ElseIf Trim(Request("auto_ac")) = "true" And Trim(Request("trans")) = "" And Trim(Request("from")) = "view" Then
        Dim URLstring As String = ""

        URLstring = "view_template.aspx?compare_ac_id=" & new_id
        URLstring &= "&activetab=" & Trim(Request("activetab"))
        URLstring &= "&ac_type=" & Trim(Request("ac_type"))
        URLstring &= "&created_client=Y"
        URLstring &= "&ViewID=19"
        URLstring &= "&ac_ID=" & Trim(Request("ac_ID"))
        URLstring &= "&noteID=" & Trim(Request("viewNOTEID"))

        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Change_Window", "window.location.href = '" & URLstring & "';", True)

      ElseIf Trim(Request("autoCheckTransaction")) = "true" And Trim(Request("jtrans")) <> "" Then
        Dim trans_url As String = "edit.aspx?" & IIf(Trim(Request("from")) = "view", "from=view&", "") & "action=edit&source=CLIENT&acID=" & new_id & "&type=transaction&trans=" & Trim(Request("jtrans"))

        If Trim(Request("from")) <> "view" Then
          url = "listing_transaction.aspx?redo_search=true"
          System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.location = '" & url & "';", True)
        End If

        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Change_Window", "window.location.href = '" & trans_url & "';", True)
      ElseIf Trim(Request("from")) = "view" And Trim(Request("autoCheckTransaction")) <> "true" Then
        'System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "if (window.opener.location.pathname.toUpperCase().search('VIEW_TEMPLATE') == 1){window.opener.location.href = window.opener.location.href;} else {window.opener.opener.location.href = window.opener.opener.location.href;window.opener.close();}", True)
        'System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)
        If viewNOTEID <> 0 Then
          url = "view_template.aspx?ViewID=19&noteID=" & viewNOTEID & "&noMaster=false" & IIf(Not String.IsNullOrEmpty(Trim(Request("extra_amod"))), "&extra_amod=" & Trim(Request("extra_amod")), "")
        Else
          url = "view_template.aspx?ViewID=1&noMaster=false&amod_id=" & jetnet_amod_id.Text & IIf(Not String.IsNullOrEmpty(Trim(Request("extra_amod"))), "&extra_amod=" & Trim(Request("extra_amod")), "")
        End If

        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "if (window.opener.location.pathname.toUpperCase().search('VIEW_TEMPLATE') == 1){window.opener.location = '" & url & "';} else {window.opener.location.href = window.opener.location.href;}", True)
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)

      ElseIf Trim(Request("redirect")) = "tovalue" Then
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.location.href = window.opener.location.href;", True)
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Change_Window", "window.location.href = 'edit_note.aspx?action=new&type=valuation&cat_key=0&refreshing=view';", True)
      Else
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.location = '" & url & "';", True)
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)
      End If
    Catch ex As Exception
      error_string = "Aircraft_Edit_Template.ascx.vb - ac_insert_function() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub
  Private Sub fill_propeller(ByVal id As Integer)
    Try
      Dim aclsClient_Aircraft_Propeller As New clsClient_Aircraft_Propeller
      aTempTable2 = aclsData_Temp.GetJETNET_Aircraft_Propeller(Session.Item("ListingID"))
      If Not IsNothing(aTempTable2) Then
        If aTempTable2.Rows.Count > 0 Then

          For Each r As DataRow In aTempTable2.Rows

            aclsClient_Aircraft_Propeller.cliacpr_cliac_id = id

            If Not IsDBNull(r("ac_prop_1_ser_no")) Then
              aclsClient_Aircraft_Propeller.cliacpr_prop_1_ser_nbr = r("ac_prop_1_ser_no")
            End If
            If Not IsDBNull(r("ac_prop_2_ser_no")) Then
              aclsClient_Aircraft_Propeller.cliacpr_prop_2_ser_nbr = r("ac_prop_2_ser_no")
            End If
            If Not IsDBNull(r("ac_prop_3_ser_no")) Then
              aclsClient_Aircraft_Propeller.cliacpr_prop_3_ser_nbr = r("ac_prop_3_ser_no")
            End If
            If Not IsDBNull(r("ac_prop_1_snew_hrs")) Then
              aclsClient_Aircraft_Propeller.cliacpr_prop_1_ttsn_hours = r("ac_prop_1_snew_hrs")
            End If
            If Not IsDBNull(r("ac_prop_2_snew_hrs")) Then
              aclsClient_Aircraft_Propeller.cliacpr_prop_2_ttsn_hours = r("ac_prop_2_snew_hrs")
            End If
            If Not IsDBNull(r("ac_prop_3_snew_hrs")) Then
              aclsClient_Aircraft_Propeller.cliacpr_prop_3_ttsn_hours = r("ac_prop_3_snew_hrs")
            End If

            If Not IsDBNull(r("ac_prop_1_soh_hrs")) Then
              aclsClient_Aircraft_Propeller.cliacpr_prop_1_tsoh_hours = r("ac_prop_1_soh_hrs")
            End If
            If Not IsDBNull(r("ac_prop_2_soh_hrs")) Then
              aclsClient_Aircraft_Propeller.cliacpr_prop_2_tsoh_hours = r("ac_prop_2_soh_hrs")
            End If
            If Not IsDBNull(r("ac_prop_3_soh_hrs")) Then
              aclsClient_Aircraft_Propeller.cliacpr_prop_3_tsoh_hours = r("ac_prop_3_soh_hrs")
            End If

            If Not IsDBNull(r("ac_prop_1_soh_moyear")) Then
              aclsClient_Aircraft_Propeller.cliacpr_prop_1_month_year_oh = r("ac_prop_1_soh_moyear")
            End If
            If Not IsDBNull(r("ac_prop_2_soh_moyear")) Then
              aclsClient_Aircraft_Propeller.cliacpr_prop_2_month_year_oh = r("ac_prop_2_soh_moyear")
            End If
            If Not IsDBNull(r("ac_prop_3_soh_moyear")) Then
              aclsClient_Aircraft_Propeller.cliacpr_prop_3_month_year_oh = r("ac_prop_3_soh_moyear")
            End If
          Next
          ' dump the datatable
          aTempTable2.Dispose()
          aTempTable2 = Nothing

          If aclsData_Temp.Insert_Client_Aircraft_Propeller(aclsClient_Aircraft_Propeller) = 1 Then

          Else
            If aclsData_Temp.class_error <> "" Then
              error_string = aclsData_Temp.class_error
              LogError("Aircraft_Edit_Template.ascx.vb - ac_insert_function() - " & error_string)
            End If
            display_error()
          End If

        End If
      Else
        If aclsData_Temp.class_error <> "" Then
          error_string = aclsData_Temp.class_error
          LogError("Aircraft_Edit_Template.ascx.vb - ac_insert_function() - " & error_string)
        End If
        display_error()
      End If


    Catch ex As Exception
      error_string = "Aircraft_Edit_Template.ascx.vb - fill_propeller() - " & ex.Message
      LogError(error_string)
    End Try

  End Sub
  Private Sub fill_features(ByVal id As Integer, ByVal jetnet_id As Integer)
    Try
      aTempTable2 = aclsData_Temp.GetJETNET_Aircraft_Details_Key_Features_AC_ID(jetnet_id, 0)

      If Not IsNothing(aTempTable2) Then
        For Each r As DataRow In aTempTable2.Rows

          If aclsData_Temp.Insert_Client_Aircraft_Key_Features(id, IIf(Not IsDBNull(r("kfeat_type")), r("kfeat_type"), ""), IIf(Not IsDBNull(r("afeat_flag")), r("afeat_flag"), ""), r("afeat_seq_nbr")) = 1 Then
          Else
            If aclsData_Temp.class_error <> "" Then
              error_string = aclsData_Temp.class_error
              LogError("Aircraft_Edit_Template.ascx.vb - ac_insert_function() - " & error_string)
            End If
            display_error()
          End If

        Next

      Else
        If aclsData_Temp.class_error <> "" Then
          error_string = aclsData_Temp.class_error
          LogError("Aircraft_Edit_Template.ascx.vb - ac_insert_function() - " & error_string)
        End If
        display_error()
      End If

    Catch ex As Exception
      error_string = "Aircraft_Edit_Template.ascx.vb - fill_propeller() - " & ex.Message
      LogError(error_string)
    End Try

  End Sub

  Public Function InsertOwnerNotDefinedCompany() As Long
    Dim clsCompany As New clsClient_Company
    Dim checkTable As New DataTable
    clsCompany.clicomp_name = "OWNER NOT DEFINED"
    clsCompany.clicomp_search_name = "OWNERNOTDEFINED"
    clsCompany.clicomp_user_id = Session.Item("localUser").crmLocalUserID
    clsCompany.clicomp_date_updated = Now()
    clsCompany.clicomp_status = "Y"

    If aclsData_Temp.Insert_Client_Company(clsCompany) = True Then
      checkTable = aclsData_Temp.FindNotDefinedOwnerCompany()
      If Not IsNothing(checkTable) Then
        If checkTable.Rows.Count > 0 Then
          clsCompany.clicomp_id = checkTable.Rows(0).Item("comp_id")
        End If
      End If
    End If

    Return clsCompany.clicomp_id
  End Function

  Private Sub fill_engine_details(ByVal id As Integer, ByVal update_id As Integer, ByVal jetnet_id As Integer)
    Try
      Dim aclsInsert_Client_Aircraft_Engine As New clsClient_Aircraft_Engine

      aTempTable2 = aclsData_Temp.GetJETNET_Aircraft_Engine(jetnet_id)
      If Not IsNothing(aTempTable2) Then
        If aTempTable2.Rows.Count > 0 Then
          For Each R As DataRow In aTempTable2.Rows

            aclsInsert_Client_Aircraft_Engine.cliacep_cliac_id = id
            If Not IsDBNull(R("ac_engine_name")) Then
              aclsInsert_Client_Aircraft_Engine.cliacep_engine_name = R("ac_engine_name")
            End If

            If Not IsDBNull(R("ac_engine_maintenance_prog_EMP")) Then
              aclsInsert_Client_Aircraft_Engine.cliacep_engine_maintenance_program = R("ac_engine_maintenance_prog_EMP")
            End If
            If Not IsDBNull(R("ac_engine_management_prog_EMGP")) Then
              aclsInsert_Client_Aircraft_Engine.cliacep_engine_management_program = R("ac_engine_management_prog_EMGP")
            End If

            If Not IsDBNull(R("ac_engine_tbo_oc_flag")) Then
              aclsInsert_Client_Aircraft_Engine.cliacep_engine_tbo_oc_flag = R("ac_engine_tbo_oc_flag")
            End If
            If Not IsDBNull(R("ac_engine_noise_rating")) Then
              Dim tes As String = R("ac_engine_noise_rating")
              aclsInsert_Client_Aircraft_Engine.cliacep_engine_noise_rating = CInt(R("ac_engine_noise_rating"))
            End If

            If Not IsDBNull(R("ac_model_config")) Then
              aclsInsert_Client_Aircraft_Engine.cliacep_engine_model_config = R("ac_model_config")
            End If
            If Not IsDBNull(R("ac_maint_eoh_by_name")) Then
              aclsInsert_Client_Aircraft_Engine.cliacep_engine_overhaul_done_by_name = R("ac_maint_eoh_by_name")
            End If
            If Not IsDBNull(R("ac_main_eoh_moyear")) Then
              aclsInsert_Client_Aircraft_Engine.cliacep_engine_overhaul_done_month_year = R("ac_main_eoh_moyear")
            End If
            If Not IsDBNull(R("ac_maint_hots_by_name")) Then
              aclsInsert_Client_Aircraft_Engine.cliacep_engine_hot_inspection_done_by_name = R("ac_maint_hots_by_name")
            End If
            If Not IsDBNull(R("ac_maint_hots_moyear")) Then
              aclsInsert_Client_Aircraft_Engine.cliacep_engine_hot_inspection_done_month_year = R("ac_maint_hots_moyear")
            End If

            If Not IsDBNull(R("ac_engine_1_ser_no")) Then
              aclsInsert_Client_Aircraft_Engine.cliacep_engine_1_ser_nbr = R("ac_engine_1_ser_no")
            End If
            If Not IsDBNull(R("ac_engine_2_ser_no")) Then
              aclsInsert_Client_Aircraft_Engine.cliacep_engine_2_ser_nbr = R("ac_engine_2_ser_no")
            End If
            If Not IsDBNull(R("ac_engine_3_ser_no")) Then
              aclsInsert_Client_Aircraft_Engine.cliacep_engine_3_ser_nbr = R("ac_engine_3_ser_no")
            End If
            If Not IsDBNull(R("ac_engine_4_ser_no")) Then
              aclsInsert_Client_Aircraft_Engine.cliacep_engine_4_ser_nbr = R("ac_engine_4_ser_no")
            End If
            If Not IsDBNull(R("ac_engine_1_tot_hrs")) Then
              aclsInsert_Client_Aircraft_Engine.cliacep_engine_1_ttsn_hours = R("ac_engine_1_tot_hrs")
            End If
            If Not IsDBNull(R("ac_engine_2_tot_hrs")) Then
              aclsInsert_Client_Aircraft_Engine.cliacep_engine_2_ttsn_hours = R("ac_engine_2_tot_hrs")
            End If
            If Not IsDBNull(R("ac_engine_3_tot_hrs")) Then
              aclsInsert_Client_Aircraft_Engine.cliacep_engine_3_ttsn_hours = R("ac_engine_3_tot_hrs")
            End If
            If Not IsDBNull(R("ac_engine_4_tot_hrs")) Then
              aclsInsert_Client_Aircraft_Engine.cliacep_engine_4_ttsn_hours = R("ac_engine_4_tot_hrs")
            End If
            If Not IsDBNull(R("ac_engine_1_soh_hrs")) Then
              aclsInsert_Client_Aircraft_Engine.cliacep_engine_1_tsoh_hours = R("ac_engine_1_soh_hrs")
            End If
            If Not IsDBNull(R("ac_engine_2_soh_hrs")) Then
              aclsInsert_Client_Aircraft_Engine.cliacep_engine_2_tsoh_hours = R("ac_engine_2_soh_hrs")
            End If
            If Not IsDBNull(R("ac_engine_3_soh_hrs")) Then
              aclsInsert_Client_Aircraft_Engine.cliacep_engine_3_tsoh_hours = R("ac_engine_3_soh_hrs")
            End If
            If Not IsDBNull(R("ac_engine_4_soh_hrs")) Then
              aclsInsert_Client_Aircraft_Engine.cliacep_engine_4_tsoh_hours = R("ac_engine_4_soh_hrs")
            End If
            If Not IsDBNull(R("ac_engine_1_shi_hrs")) Then
              aclsInsert_Client_Aircraft_Engine.cliacep_engine_1_tshi_hours = R("ac_engine_1_shi_hrs")
            End If
            If Not IsDBNull(R("ac_engine_2_shi_hrs")) Then
              aclsInsert_Client_Aircraft_Engine.cliacep_engine_2_tshi_hours = R("ac_engine_2_shi_hrs")
            End If
            If Not IsDBNull(R("ac_engine_3_shi_hrs")) Then
              aclsInsert_Client_Aircraft_Engine.cliacep_engine_3_tshi_hours = R("ac_engine_3_shi_hrs")
            End If
            If Not IsDBNull(R("ac_engine_4_shi_hrs")) Then
              aclsInsert_Client_Aircraft_Engine.cliacep_engine_4_tshi_hours = R("ac_engine_4_shi_hrs")
            End If
            If Not IsDBNull(R("ac_engine_1_tbo_hrs")) Then
              aclsInsert_Client_Aircraft_Engine.cliacep_engine_1_tbo_hours = R("ac_engine_1_tbo_hrs")
            End If
            If Not IsDBNull(R("ac_engine_2_tbo_hrs")) Then
              aclsInsert_Client_Aircraft_Engine.cliacep_engine_2_tbo_hours = R("ac_engine_2_tbo_hrs")
            End If
            If Not IsDBNull(R("ac_engine_3_tbo_hrs")) Then
              aclsInsert_Client_Aircraft_Engine.cliacep_engine_3_tbo_hours = R("ac_engine_3_tbo_hrs")
            End If
            If Not IsDBNull(R("ac_engine_4_tbo_hrs")) Then
              aclsInsert_Client_Aircraft_Engine.cliacep_engine_4_tbo_hours = R("ac_engine_4_tbo_hrs")
            End If
            If Not IsDBNull(R("ac_engine_1_snew_cycles")) Then
              aclsInsert_Client_Aircraft_Engine.cliacep_engine_1_tsn_cycle = R("ac_engine_1_snew_cycles")
            End If
            If Not IsDBNull(R("ac_engine_2_snew_cycles")) Then
              aclsInsert_Client_Aircraft_Engine.cliacep_engine_2_tsn_cycle = R("ac_engine_2_snew_cycles")
            End If
            If Not IsDBNull(R("ac_engine_3_snew_cycles")) Then
              aclsInsert_Client_Aircraft_Engine.cliacep_engine_3_tsn_cycle = R("ac_engine_3_snew_cycles")
            End If
            If Not IsDBNull(R("ac_engine_4_snew_cycles")) Then
              aclsInsert_Client_Aircraft_Engine.cliacep_engine_4_tsn_cycle = R("ac_engine_4_snew_cycles")
            End If
            If Not IsDBNull(R("ac_engine_1_soh_cycles")) Then
              aclsInsert_Client_Aircraft_Engine.cliacep_engine_1_tsoh_cycle = R("ac_engine_1_soh_cycles")
            End If
            If Not IsDBNull(R("ac_engine_2_soh_cycles")) Then
              aclsInsert_Client_Aircraft_Engine.cliacep_engine_2_tsoh_cycle = R("ac_engine_2_soh_cycles")
            End If
            If Not IsDBNull(R("ac_engine_3_soh_cycles")) Then
              aclsInsert_Client_Aircraft_Engine.cliacep_engine_3_tsoh_cycle = R("ac_engine_3_soh_cycles")
            End If
            If Not IsDBNull(R("ac_engine_4_soh_cycles")) Then
              aclsInsert_Client_Aircraft_Engine.cliacep_engine_4_tsoh_cycle = R("ac_engine_4_soh_cycles")
            End If
            If Not IsDBNull(R("ac_engine_1_shs_cycles")) Then
              aclsInsert_Client_Aircraft_Engine.cliacep_engine_1_tshi_cycle = R("ac_engine_1_shs_cycles")
            End If
            If Not IsDBNull(R("ac_engine_2_shs_cycles")) Then
              aclsInsert_Client_Aircraft_Engine.cliacep_engine_2_tshi_cycle = R("ac_engine_2_shs_cycles")
            End If
            If Not IsDBNull(R("ac_engine_3_shs_cycles")) Then
              aclsInsert_Client_Aircraft_Engine.cliacep_engine_3_tshi_cycle = R("ac_engine_3_shs_cycles")
            End If
            If Not IsDBNull(R("ac_engine_4_shs_cycles")) Then
              aclsInsert_Client_Aircraft_Engine.cliacep_engine_4_tshi_cycle = R("ac_engine_4_shs_cycles")
            End If

          Next
        End If
      Else
        If aclsData_Temp.class_error <> "" Then
          error_string = aclsData_Temp.class_error
          LogError("Aircraft_Edit_Template.ascx.vb - fill_engine_details() - " & error_string)
        End If
        display_error()
      End If

      'Response.Write(aclsInsert_Client_Aircraft_Engine.ClassInfo(aclsInsert_Client_Aircraft_Engine))

      If update_id <> 0 Then
        aclsData_Temp.Update_Client_Aircraft_Engine(aclsInsert_Client_Aircraft_Engine)
      Else
        aclsData_Temp.Insert_Client_Aircraft_Engine(aclsInsert_Client_Aircraft_Engine)
      End If
    Catch ex As Exception
      error_string = "Aircraft_Edit_Template.ascx.vb - fill_engine_details() - " & ex.Message
      LogError(error_string)
    End Try

  End Sub
  Private Sub fill_ac_details(ByVal id As Integer, ByVal type As String, ByVal jetnet_id As Integer)
    Try
      If type = "" Then
        aTempTable = aclsData_Temp.GetJETNET_Aircraft_Details_Equipment_AC_ID(jetnet_id)
      Else
        aTempTable = aclsData_Temp.GetJETNET_Aircraft_Details_Equipment_AC_ID_TYPE(jetnet_id, type, 0)
      End If

      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then
          For Each R As DataRow In aTempTable.Rows
            If aclsData_Temp.Insert_Client_Aircraft_Details(id, IIf(Not IsDBNull(R("adet_data_type")), R("adet_data_type"), ""), IIf(Not IsDBNull(R("adet_data_name")), R("adet_data_name"), ""), IIf(Not IsDBNull(R("adet_data_description")), R("adet_data_description"), ""), Now()) = 1 Then
            End If
          Next
        End If
      Else
        If aclsData_Temp.class_error <> "" Then
          error_string = aclsData_Temp.class_error
          LogError("Aircraft_Edit_Template.ascx.vb - fill_ac_details() - " & error_string)
        End If
        display_error()
      End If
    Catch ex As Exception
      error_string = "Aircraft_Edit_Template.ascx.vb - fill_ac_details() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub
  Private Sub fill_avionics(ByVal id As Integer, ByVal jetnet_id As Long)
    Try
      aTempTable = aclsData_Temp.GetJETNET_Aircraft_Avionics_AC_ID(jetnet_id, 0)
      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then
          For Each R As DataRow In aTempTable.Rows
            If aclsData_Temp.Insert_Client_Aircraft_Avionics(id, IIf(Not IsDBNull(R("av_name")), R("av_name"), ""), IIf(Not IsDBNull(R("av_description")), R("av_description"), "")) = 1 Then
            End If
          Next
        End If
      Else
        If aclsData_Temp.class_error <> "" Then
          error_string = aclsData_Temp.class_error
          LogError("Aircraft_Edit_Template.ascx.vb - fill_avionics() - " & error_string)
        End If
        display_error()
      End If
    Catch ex As Exception
      error_string = "Aircraft_Edit_Template.ascx.vb - fill_avionics() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub
  Private Sub Fill_Companies(ByVal new_id As Integer)
    Try
      Dim old_key As Integer = 0
      aTempTable = aclsData_Temp.GetAircraft_Listing_wContacts(Session.Item("ListingID"), "JETNET")
      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then
          For Each r As DataRow In aTempTable.Rows
            If old_key <> r("comp_id") Then
              'Response.Write(r("comp_id") & " " & r("comp_name") & " -Company attached to AC<br />")

              Fill_Company(r("comp_id"), new_id)
            End If
            old_key = r("comp_id")
          Next
        End If
      Else
        If aclsData_Temp.class_error <> "" Then
          error_string = aclsData_Temp.class_error
          LogError("Aircraft_Edit_Template.ascx.vb - Fill_Companies() - " & error_string)
        End If
        display_error()
      End If
    Catch ex As Exception
      error_string = "Aircraft_Edit_Template.ascx.vb - Fill_Companies() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub
  Private Sub Fill_Company(ByVal jetnet_id As Integer, ByVal new_id As Integer)
    Try
      aTempTable = aclsData_Temp.GetCompanyInfo_ID(jetnet_id, "JETNET", 0)
      If Not IsNothing(aTempTable) Then 'not nothing
        Dim aclsClient_Company As New clsClient_Company
        Dim comp_id As Integer = 0
        For Each r As DataRow In aTempTable.Rows
          aclsClient_Company.clicomp_user_id = Session.Item("localUser").crmLocalUserID
          aclsClient_Company.clicomp_name = r("comp_name")
          If Not IsDBNull(r("comp_alternate_name_type")) Then
            aclsClient_Company.clicomp_alternate_name_type = r("comp_alternate_name_type")
          End If
          If Not IsDBNull(r("comp_alternate_name")) Then
            aclsClient_Company.clicomp_alternate_name = r("comp_alternate_name")
          End If
          If Not IsDBNull(r("comp_address1")) Then
            aclsClient_Company.clicomp_address1 = r("comp_address1")
          End If
          If Not IsDBNull(r("comp_address2")) Then
            aclsClient_Company.clicomp_address2 = r("comp_address2")
          End If
          If Not IsDBNull(r("comp_city")) Then
            aclsClient_Company.clicomp_city = r("comp_city")
          End If
          If Not IsDBNull(r("comp_state")) Then
            aclsClient_Company.clicomp_state = r("comp_state")
          End If
          If Not IsDBNull(r("comp_zip_code")) Then
            aclsClient_Company.clicomp_zip_code = r("comp_zip_code")
          End If
          If Not IsDBNull(r("comp_country")) Then
            aclsClient_Company.clicomp_country = r("comp_country")
          End If
          If Not IsDBNull(r("comp_agency_type")) Then
            aclsClient_Company.clicomp_agency_type = r("comp_agency_type")
          End If
          If Not IsDBNull(r("comp_web_address")) Then
            aclsClient_Company.clicomp_web_address = r("comp_web_address")
          End If
          If Not IsDBNull(r("comp_email_address")) Then
            aclsClient_Company.clicomp_email_address = r("comp_email_address")
          End If
          aclsClient_Company.clicomp_date_updated = Now()
          aclsClient_Company.clicomp_jetnet_comp_id = r("comp_id")
          comp_id = r("comp_id")
        Next
        Dim insert_data As Boolean = True
        Dim idnum_new As Integer
        'inserting that info into the database. 
        Dim carry_on As Boolean = False
        aTempTable2 = aclsData_Temp.GetCompanyInfo_JETNET_ID(comp_id, "")
        If aTempTable2.Rows.Count = 0 Then 'This jetnet record isn't in a company record yet, so let's insert it.
          If aclsData_Temp.Insert_Client_Company(aclsClient_Company) = True Then
            carry_on = True
          End If
        Else
          'Doesn't need phone or contacts.
          insert_data = False
          carry_on = True
          'already exists don't add to database just swap ID
          comp_id = jetnet_id
        End If

        aTempTable = aclsData_Temp.GetCompanyInfo_JETNET_ID(comp_id, "")
        If Not IsNothing(aTempTable) Then 'not nothing
          For Each r As DataRow In aTempTable.Rows
            idnum_new = r("comp_id")
          Next
        Else
          If aclsData_Temp.class_error <> "" Then
            error_string = aclsData_Temp.class_error
            LogError("Aircraft_Edit_Template.ascx.vb - Fill_Company() - " & error_string)
          End If
          display_error()
        End If

        If carry_on = True And insert_data = True Then
          'This means that the company information got stored correctly.

          aTempTable = aclsData_Temp.GetCompanyInfo_JETNET_ID(comp_id, "")
          If Not IsNothing(aTempTable) Then 'not nothing
            For Each r As DataRow In aTempTable.Rows
              idnum_new = r("comp_id")
              aTempTable2 = aclsData_Temp.GetPhoneNumbers(comp_id, 0, "JETNET", 0)
              If Not IsNothing(aTempTable) Then
                If aTempTable.Rows.Count > 0 Then
                  For Each q As DataRow In aTempTable2.Rows
                    Dim aclsClient_Phone_Numbers As New clsClient_Phone_Numbers

                    aclsClient_Phone_Numbers.clipnum_type = q("pnum_type")
                    aclsClient_Phone_Numbers.clipnum_number = q("pnum_number")
                    aclsClient_Phone_Numbers.clipnum_comp_id = r("comp_id") 'This is the comp_id of the new company we just inserted.
                    aclsClient_Phone_Numbers.clipnum_contact_id = 0
                    If aclsData_Temp.Insert_Client_PhoneNumbers(aclsClient_Phone_Numbers) = True Then
                      ' Response.Write("insert contact phone Number<br />")
                    Else
                      If aclsData_Temp.class_error <> "" Then
                        error_string = aclsData_Temp.class_error
                        LogError("Aircraft_Edit_Template.ascx.vb - Fill_Company() - " & error_string)
                      End If
                      display_error()
                    End If
                  Next 'for each in get phone numbers
                End If



              Else
                If aclsData_Temp.class_error <> "" Then
                  error_string = aclsData_Temp.class_error
                  LogError("Aircraft_Edit_Template.ascx.vb - Fill_Company() - " & error_string)
                End If
                display_error()
              End If
            Next 'For each row in get company info
          End If


          RaiseEvent get_insert_ac(jetnet_id, idnum_new, True, True, new_id, CInt(Session.Item("ListingID")))
          'This is where I have to get all the other contacts from the jetnet company!!! Besides the one
          'That we have the id for!

          Dim status As Boolean = False
          '=========================
          ' If insert_data = True Then
          RaiseEvent loop_contacts(idnum_new, comp_id, jetnet_id, True, status, new_id, CInt(Session.Item("ListingID")))
          ' End If
        Else
          'client company 
          Dim client_company As Integer = aTempTable.Rows(0).Item("comp_id")
          'FInd all company references for this ac where comp_id = comp_id and AC id = ac id. 
          aTempTable2 = aclsData_Temp.Get_Aircraft_Reference_JETNET_ONLYcompID(comp_id)
          If Not IsNothing(aTempTable2) Then
            If aTempTable2.Rows.Count > 0 Then
              For Each q As DataRow In aTempTable2.Rows
                'loop through references - look up client contact relating to jetnet contact
                Dim client_contact As Integer = q("acref_contact_id")
                Dim atemptable3 As New DataTable
                If client_contact <> 0 Then 'have to protect against this
                  atemptable3 = aclsData_Temp.GetContacts_Details_JETNETID(client_contact)
                Else
                  atemptable3 = New DataTable
                End If
                If Not IsNothing(atemptable3) Then

                  Dim client_ac As Integer = 0

                  If Session.Item("ListingID") = q("acref_ac_id") Then
                    client_ac = new_id
                  End If

                  If client_ac <> 0 Then
                    If atemptable3.Rows.Count > 0 Then

                      Dim contact_id_new As Integer = atemptable3.Rows(0).Item("contact_id")
                      ' Response.Write(aTempTable.Rows(0).Item("contact_id") & "!!! client contact ID")
                      Dim aclsInsert_Client_Aircraft_Reference As New clsClient_Aircraft_Reference
                      aclsInsert_Client_Aircraft_Reference.cliacref_cliac_id = client_ac
                      aclsInsert_Client_Aircraft_Reference.cliacref_comp_id = client_company
                      aclsInsert_Client_Aircraft_Reference.cliacref_contact_type = q("acref_contact_type")
                      aclsInsert_Client_Aircraft_Reference.cliacref_contact_id = contact_id_new

                      aclsInsert_Client_Aircraft_Reference.cliacref_jetnet_ac_id = 0
                      aclsInsert_Client_Aircraft_Reference.cliacref_operator_flag = q("acref_operator_flag")
                      aclsInsert_Client_Aircraft_Reference.cliacref_owner_percentage = IIf(Not IsDBNull(q("acref_owner_percentage")), q("acref_owner_percentage"), "0")
                      aclsInsert_Client_Aircraft_Reference.cliacref_business_type = q("acref_business_type")
                      aclsInsert_Client_Aircraft_Reference.cliacref_date_fraction_expires = Now()
                      aclsInsert_Client_Aircraft_Reference.cliacref_date_fraction_purchased = Now()
                      'Response.Write(aclsInsert_Client_Aircraft_Reference.ClassInfo(aclsInsert_Client_Aircraft_Reference) & "<Hr />")
                      If aclsData_Temp.Insert_Client_Aircraft_Reference(aclsInsert_Client_Aircraft_Reference) = True Then
                      Else
                        If aclsData_Temp.class_error <> "" Then
                          error_string = aclsData_Temp.class_error
                          LogError("edit.aspx.vb - loop_contacts() - " & error_string)
                        End If
                        display_error()
                      End If

                    Else

                      Dim aclsInsert_Client_Aircraft_Reference As New clsClient_Aircraft_Reference
                      aclsInsert_Client_Aircraft_Reference.cliacref_cliac_id = client_ac
                      aclsInsert_Client_Aircraft_Reference.cliacref_comp_id = client_company
                      aclsInsert_Client_Aircraft_Reference.cliacref_contact_type = q("acref_contact_type")
                      aclsInsert_Client_Aircraft_Reference.cliacref_contact_id = 0
                      aclsInsert_Client_Aircraft_Reference.cliacref_jetnet_ac_id = 0
                      aclsInsert_Client_Aircraft_Reference.cliacref_operator_flag = q("acref_operator_flag")
                      aclsInsert_Client_Aircraft_Reference.cliacref_owner_percentage = IIf(Not IsDBNull(q("acref_owner_percentage")), q("acref_owner_percentage"), "0")
                      aclsInsert_Client_Aircraft_Reference.cliacref_business_type = q("acref_business_type")
                      aclsInsert_Client_Aircraft_Reference.cliacref_date_fraction_expires = Now()
                      aclsInsert_Client_Aircraft_Reference.cliacref_date_fraction_purchased = Now()
                      'Response.Write(aclsInsert_Client_Aircraft_Reference.ClassInfo(aclsInsert_Client_Aircraft_Reference) & "<Hr />")
                      If aclsData_Temp.Insert_Client_Aircraft_Reference(aclsInsert_Client_Aircraft_Reference) = True Then
                      Else
                        If aclsData_Temp.class_error <> "" Then
                          error_string = aclsData_Temp.class_error
                          LogError("edit.aspx.vb - loop_contacts() - " & error_string)
                        End If
                        display_error()
                      End If
                    End If
                  End If
                End If


              Next
            End If
          End If

        End If
      Else
        If aclsData_Temp.class_error <> "" Then
          error_string = aclsData_Temp.class_error
          LogError("Aircraft_Edit_Template.ascx.vb - Fill_Company() - " & error_string)
        End If
        display_error()
      End If
    Catch ex As Exception
      error_string = "Aircraft_Edit_Template.ascx.vb - Fill_Company() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub
  Private Sub fill_edit_data()
    Try
      Dim ID As Integer = CInt(Session.Item("ListingID"))
      Dim aError As String = ""
      'This fills up all of the text boxes based on what id, source, type. Parent is type. 1 = Company, 2 = Contact, 3 = Aircraft. 
      Dim atemptable3 As New DataTable
      If Session.Item("ListingSource") = "JETNET" Then
        deleteFunction.Visible = False
        aTempTable = aclsData_Temp.GetJETNET_Aircraft_Details_AC_ID(ID, aError)
        If Not IsNothing(aTempTable) Then
          If aTempTable.Rows.Count > 0 Then
            For Each R As DataRow In aTempTable.Rows

              Dim make As String = ""
              Dim model As String = ""
              Dim manu As String = ""
              Dim make_type As String = ""
              Dim airframe As String = ""

              aTempTable2 = aclsData_Temp.Get_JETNET_Aircraft_Model_amodID((R("ac_amod_id")))
              jetnet_amod_id.Text = R("ac_amod_id")
              model_cbo.SelectedValue = R("ac_amod_id")
              If Not IsNothing(aTempTable2) Then
                If aTempTable2.Rows.Count > 0 Then

                  For Each q As DataRow In aTempTable2.Rows
                    If q("source") = "JETNET" Then 'jetnet_amod_id
                      atemptable3 = aclsData_Temp.Get_Clients_Aircraft_Model_Make_Model(q("amod_make_name"), q("amod_model_name"))
                      make = CStr(IIf(Not IsDBNull(q("amod_make_name")), q("amod_make_name"), ""))
                      model = CStr(IIf(Not IsDBNull(q("amod_model_name")), q("amod_model_name"), ""))
                      model_cbo.SelectedValue = ""
                      manu = CStr(IIf(Not IsDBNull(q("amod_manufacturer_name")), q("amod_manufacturer_name"), ""))
                      make_type = CStr(IIf(Not IsDBNull(q("amod_make_type")), q("amod_make_type"), ""))
                      airframe = CStr(IIf(Not IsDBNull(q("amod_airframe_type")), q("amod_airframe_type"), ""))

                      model_listing.Style.Add("display", "none")
                      model_cbo.Items.FindByValue("").Text = make & " " & model
                    End If
                  Next

                End If
              Else
                If aclsData_Temp.class_error <> "" Then
                  error_string = aclsData_Temp.class_error
                  LogError("Aircraft_Edit_Template.ascx.vb - fill_edit_data() - " & error_string & " AC ID " & Session.Item("ListingID"))
                End If
                display_error()
              End If

              If Not IsNothing(atemptable3) Then
                If atemptable3.Rows.Count > 0 Then
                  For Each t As DataRow In atemptable3.Rows
                    model_cbo.SelectedValue = t("cliamod_id")
                    model_listing.Style.Add("display", "none")
                  Next
                Else

                  ac_make.Text = make
                  ac_model.Text = model
                  model_cbo.SelectedValue = ""
                  ac_manu_name.Text = manu
                  ac_make_type.SelectedValue = make_type
                  Airframe_type.SelectedValue = airframe
                  'model_listing.Visible = True
                  'model_text.Visible = False
                  'model_cbo.Enabled = False
                  'model_cbo.Visible = False
                  'custom_ac_hide.Visible = False

                End If
              Else
                If aclsData_Temp.class_error <> "" Then
                  error_string = aclsData_Temp.class_error
                  LogError("Aircraft_Edit_Template.ascx.vb - fill_edit_data() - " & error_string & " AC ID " & Session.Item("ListingID"))
                End If
                display_error()
              End If

              'ac_lifecycle()
              'ac_ownership()
              'ac_usage()

              aircraft_edit_text.Text = CommonAircraftFunctions.CreateHeaderLine(R("amod_make_name"), R("amod_model_name"), R("ac_ser_nbr"), "EDIT")

              serial_sort.Text = CStr(IIf(Not IsDBNull(R("ac_ser_nbr_sort")), R("ac_ser_nbr_sort"), ""))
              serial.Text = CStr(IIf(Not IsDBNull(R("ac_ser_nbr")), R("ac_ser_nbr"), ""))
              alternate_serial.Text = CStr(IIf(Not IsDBNull(R("ac_alt_ser_nbr")), R("ac_alt_ser_nbr"), ""))
              reg.Text = CStr(IIf(Not IsDBNull(R("ac_reg_nbr")), R("ac_reg_nbr"), ""))
              previous_registration.Text = CStr(IIf(Not IsDBNull(R("ac_prev_reg_nbr")), R("ac_prev_reg_nbr"), ""))
              reg_country.Text = CStr(IIf(Not IsDBNull(R("ac_country_of_registration")), R("ac_country_of_registration"), ""))
              new_list.SelectedValue = CStr(IIf(Not IsDBNull(R("ac_new_flag")), R("ac_new_flag"), ""))
              year_manufactured.Text = CStr(IIf(Not IsDBNull(R("ac_year_mfr")), R("ac_year_mfr"), ""))
              year_dlv.Text = CStr(IIf(Not IsDBNull(R("ac_year_dlv")), R("ac_year_dlv"), ""))

              Dim purchased As String = CStr(IIf(Not IsDBNull(R("ac_date_purchased")), R("ac_date_purchased"), ""))
              purchased = CStr(IIf(((purchased <> "12:00:00 AM")), purchased, ""))


              date_purchased.Text = purchased
              ac_sale.SelectedValue = CStr(IIf(Not IsDBNull(R("ac_forsale_flag")), R("ac_forsale_flag"), ""))

              Dim date_listing As String = CStr(IIf(Not IsDBNull(R("ac_date_listed")), R("ac_date_listed"), ""))
              date_listing = CStr(IIf(((date_listing <> "12:00:00 AM")), date_listing, ""))


              date_listed.Text = date_listing
              'ac_maintained.text = CStr(IIf(Not IsDBNull(R("ac_maintained")), R("ac_maintained"), ""))

              ac_status_hold.Text = CStr(IIf(Not IsDBNull(R("ac_status")), R("ac_status"), ""))
              lifecycle_list.SelectedValue = CStr(IIf(Not IsDBNull(R("ac_lifecycle")), R("ac_lifecycle"), ""))


              If ac_sale.SelectedValue = "Y" Then

                ac_status_not_for_sale_withdrawn.Attributes.Add("style", "display:none;")
                ac_status_not_for_sale.Attributes.Add("style", "display:none;")
                ac_status_for_sale.Attributes.Add("style", "display:block;")

                If ac_status_for_sale.Items.Contains(New ListItem(CStr(IIf(Not IsDBNull(R("ac_status")), R("ac_status"), "")), CStr(IIf(Not IsDBNull(R("ac_status")), R("ac_status"), "")))) Then
                Else
                  ac_status_for_sale.Items.Add(New ListItem(CStr(IIf(Not IsDBNull(R("ac_status")), R("ac_status"), "")), CStr(IIf(Not IsDBNull(R("ac_status")), R("ac_status"), ""))))
                End If
                ac_status_for_sale.SelectedValue = CStr(IIf(Not IsDBNull(R("ac_status")), R("ac_status"), ""))
              ElseIf ac_sale.SelectedValue = "N" And lifecycle_list.SelectedValue = "4" Then
                ac_status_not_for_sale_withdrawn.Attributes.Add("style", "display:block;")
                ac_status_not_for_sale.Attributes.Add("style", "display:none;")
                ac_status_for_sale.Attributes.Add("style", "display:none;")

                If ac_status_not_for_sale_withdrawn.Items.Contains(New ListItem(CStr(IIf(Not IsDBNull(R("ac_status")), R("ac_status"), "")), CStr(IIf(Not IsDBNull(R("ac_status")), R("ac_status"), "")))) Then
                Else
                  ac_status_not_for_sale_withdrawn.Items.Add(New ListItem(CStr(IIf(Not IsDBNull(R("ac_status")), R("ac_status"), "")), CStr(IIf(Not IsDBNull(R("ac_status")), R("ac_status"), ""))))
                End If 

               ac_status_not_for_sale_withdrawn.SelectedValue = CStr(IIf(Not IsDBNull(R("ac_status")), R("ac_status"), ""))
              ElseIf ac_sale.SelectedValue = "N" Then

                If ac_status_not_for_sale.Items.Contains(New ListItem(CStr(IIf(Not IsDBNull(R("ac_status")), R("ac_status"), "")), CStr(IIf(Not IsDBNull(R("ac_status")), R("ac_status"), "")))) Then
                Else
                  ac_status_not_for_sale.Items.Add(New ListItem(CStr(IIf(Not IsDBNull(R("ac_status")), R("ac_status"), "")), CStr(IIf(Not IsDBNull(R("ac_status")), R("ac_status"), ""))))
                End If
                 ac_status_not_for_sale.SelectedValue = CStr(IIf(Not IsDBNull(R("ac_status")), R("ac_status"), ""))

                ac_status_not_for_sale_withdrawn.Attributes.Add("style", "display:none;")
                ac_status_not_for_sale.Attributes.Add("style", "display:block;")
                ac_status_for_sale.Attributes.Add("style", "display:none;")

                End If

                DOMlisted.Text = DOM(Now(), R("ac_date_listed"))

                If Not IsDBNull(R("ac_asking_wordage")) Then
                  If UCase(R("ac_asking_wordage")) = "PRICE" Then
                    asking_price.Attributes.Add("style", "display:block;")
                    ask_lbl.Attributes.Add("style", "display:block;")
                    est_label.Attributes.Add("style", "display:block;")
                    est_price.Attributes.Add("style", "display:block;")
                    broker_price.Attributes.Add("style", "display:block;")
                    broker_lbl.Attributes.Add("style", "display:block;")
                    asking_wordage.Attributes.Add("style", "display:block;")
                    date_listed_panel.Attributes.Add("style", "display:block;")
                  Else
                    asking_price.Attributes.Add("style", "display:none;")
                    ask_lbl.Attributes.Add("style", "display:none;")
                  End If
                End If

                If Not IsDBNull(R("ac_forsale_flag")) Then
                  If R("ac_forsale_flag") = "Y" Then
                    CompareValidator1.Enabled = True
                    date_listed_panel.Attributes.Add("style", "display:block;")
                    fill_ac_status()
                    est_label.Attributes.Add("style", "display:block;")
                    est_price.Attributes.Add("style", "display:block;")
                    broker_price.Attributes.Add("style", "display:block;")
                    broker_lbl.Attributes.Add("style", "display:block;")
                  Else
                    CompareValidator1.Enabled = False
                    date_listed_panel.Attributes.Add("style", "display:none;")
                    fill_ac_status()
                    asking_price.Text = "0.00"
                    est_price.Text = "0.00"
                    broker_price.Text = "0.00"
                    exclusive_no.Selected = True
                    est_label.Attributes.Add("style", "display:none;")
                    est_price.Attributes.Add("style", "display:none;")
                    broker_price.Attributes.Add("style", "display:none;")
                    broker_lbl.Attributes.Add("style", "display:none;")
                  End If
                End If

                delivery.Text = CStr(IIf(Not IsDBNull(R("ac_delivery")), R("ac_delivery"), ""))
                ac_exclusive.SelectedValue = CStr(IIf(Not IsDBNull(R("ac_exclusive_flag")), R("ac_exclusive_flag"), ""))
                asking_wordage.Text = CStr(IIf(Not IsDBNull(R("ac_asking_wordage")), R("ac_asking_wordage"), ""))

                If Not IsDBNull(R("ac_asking_price")) Then
                  asking_price.Text = FormatNumber(R("ac_asking_price"), 2)
                End If

                ac_lease.SelectedValue = CStr(IIf(Not IsDBNull(R("ac_lease_flag")), R("ac_lease_flag"), ""))
                iata_code.Text = CStr(IIf(Not IsDBNull(R("ac_aport_iata_code")), R("ac_aport_iata_code"), ""))
                icao_code.Text = CStr(IIf(Not IsDBNull(R("ac_aport_icao_code")), R("ac_aport_icao_code"), ""))
                airport_name.Text = CStr(IIf(Not IsDBNull(R("ac_aport_name")), R("ac_aport_name"), ""))
                airport_state.Text = CStr(IIf(Not IsDBNull(R("ac_aport_state")), R("ac_aport_state"), ""))
                airport_country.Text = CStr(IIf(Not IsDBNull(R("ac_aport_country")), R("ac_aport_country"), ""))
                aiport_city.Text = CStr(IIf(Not IsDBNull(R("ac_aport_city")), R("ac_aport_city"), ""))
                airport_private.SelectedValue = CStr(IIf(Not IsDBNull(R("ac_aport_private")), R("ac_aport_private"), ""))

                ownership_list.SelectedValue = CStr(IIf(Not IsDBNull(R("ac_ownership")), R("ac_ownership"), ""))



                'Filling out newly added fields:
                If Not IsDBNull(R("ac_airframe_total_hours")) Then
                  ac_airframe_total_hours.Text = R("ac_airframe_total_hours")
                End If
                If Not IsDBNull(R("ac_airframe_total_landings")) Then
                  ac_airframe_total_landings.Text = R("ac_airframe_total_landings")
                End If
                If Not IsDBNull(R("ac_date_engine_times_as_of")) Then
                  ac_date_engine_times_as_of.Text = R("ac_date_engine_times_as_of")
                End If

            Next
          End If
        Else
          If aclsData_Temp.class_error <> "" Then
            error_string = aclsData_Temp.class_error
            LogError("Aircraft_Edit_Template.ascx.vb - fill_edit_data() - " & error_string & " AC ID " & Session.Item("ListingID"))
          End If
          display_error()
        End If

      Else
        aTempTable = aclsData_Temp.Get_Clients_Aircraft(ID)
        If Not IsNothing(aTempTable) Then
          If aTempTable.Rows.Count > 0 Then
            For Each R As DataRow In aTempTable.Rows

              aircraft_edit_text.Text = CommonAircraftFunctions.CreateHeaderLine(R("cliamod_make_name"), R("cliamod_model_name"), R("cliaircraft_ser_nbr"), " EDIT")
              model_cbo.SelectedValue = R("cliaircraft_cliamod_id")
              jetnet_amod_id.Text = R("cliamod_jetnet_amod_id")
              serial.Text = CStr(IIf(Not IsDBNull(R("cliaircraft_ser_nbr")), R("cliaircraft_ser_nbr"), ""))
              asking_wordage.SelectedValue = CStr(IIf(Not IsDBNull(R("cliaircraft_asking_wordage")), Trim(R("cliaircraft_asking_wordage")), ""))
              delivery.Text = CStr(IIf(Not IsDBNull(R("cliaircraft_delivery")), R("cliaircraft_delivery"), ""))
              ac_exclusive.SelectedValue = CStr(IIf(Not IsDBNull(R("cliaircraft_exclusive_flag")), R("cliaircraft_exclusive_flag"), ""))
              ac_sale.SelectedValue = CStr(IIf(Not IsDBNull(R("cliaircraft_forsale_flag")), R("cliaircraft_forsale_flag"), ""))


              If Not IsDBNull(R("cliaircraft_forsale_flag")) Then
                If R("cliaircraft_forsale_flag").ToString.ToUpper = "Y" Then
                  'Checking for OFF MARKET DUE TO SALE:
                  If Not IsDBNull(R("cliaircraft_jetnet_ac_id")) Then
                    If R("cliaircraft_jetnet_ac_id") > 0 Then
                      CheckOffMarketDueToSale(R("cliaircraft_jetnet_ac_id"), R("cliaircraft_id"))
                    End If
                  End If
                End If
              End If

              ac_lease.SelectedValue = CStr(IIf(Not IsDBNull(R("cliaircraft_lease_flag")), R("cliaircraft_lease_flag"), ""))
              reg.Text = CStr(IIf(Not IsDBNull(R("cliaircraft_reg_nbr")), R("cliaircraft_reg_nbr"), ""))


              ac_cat1.Text = CStr(IIf(Not IsDBNull(R("cliaircraft_custom_1")), R("cliaircraft_custom_1"), ""))
              ac_cat2.Text = CStr(IIf(Not IsDBNull(R("cliaircraft_custom_2")), R("cliaircraft_custom_2"), ""))
              ac_cat3.Text = CStr(IIf(Not IsDBNull(R("cliaircraft_custom_3")), R("cliaircraft_custom_3"), ""))
              ac_cat4.Text = CStr(IIf(Not IsDBNull(R("cliaircraft_custom_4")), R("cliaircraft_custom_4"), ""))
              ac_cat5.Text = CStr(IIf(Not IsDBNull(R("cliaircraft_custom_5")), R("cliaircraft_custom_5"), ""))
              ac_cat6.Text = CStr(IIf(Not IsDBNull(R("cliaircraft_custom_6")), R("cliaircraft_custom_6"), ""))
              ac_cat7.Text = CStr(IIf(Not IsDBNull(R("cliaircraft_custom_7")), R("cliaircraft_custom_7"), ""))
              ac_cat8.Text = CStr(IIf(Not IsDBNull(R("cliaircraft_custom_8")), R("cliaircraft_custom_8"), ""))
              ac_cat9.Text = CStr(IIf(Not IsDBNull(R("cliaircraft_custom_9")), R("cliaircraft_custom_9"), ""))
              ac_cat10.Text = CStr(IIf(Not IsDBNull(R("cliaircraft_custom_10")), R("cliaircraft_custom_10"), ""))

              If Not IsDBNull(R("cliaircraft_value_description")) Then
                cliaircraft_value_description_text.Text = R("cliaircraft_value_description").ToString
              End If

              If Not IsDBNull(R("cliaircraft_est_price")) Then
                est_price.Text = FormatNumber(R("cliaircraft_est_price"), 2)
              End If
              If Not IsDBNull(R("cliaircraft_broker_price")) Then
                broker_price.Text = FormatNumber(R("cliaircraft_broker_price"), 2)
              End If

              asking_price.Text = CStr(IIf(Not IsDBNull(R("cliaircraft_asking_price")), FormatNumber(R("cliaircraft_asking_price"), 2), ""))

              If Not IsDBNull(R("cliaircraft_date_purchased")) Then
                If CStr(R("cliaircraft_date_purchased")) <> "12:00:00 AM" Then
                  If Not R("cliaircraft_date_purchased") = "1/1/1900" Then
                    date_purchased.Text = R("cliaircraft_date_purchased")
                  End If
                End If
              End If
              If Not IsDBNull(R("cliaircraft_date_listed")) Then
                If Not R("cliaircraft_date_listed") = "1/1/1900" Then
                  date_listed.Text = R("cliaircraft_date_listed")
                End If
              End If
              ac_status_hold.Text = CStr(IIf(Not IsDBNull(R("cliaircraft_status")), R("cliaircraft_status"), ""))
              lifecycle_list.SelectedValue = CStr(IIf(Not IsDBNull(R("cliaircraft_lifecycle")), R("cliaircraft_lifecycle"), ""))

              If ac_sale.SelectedValue = "Y" Then
                ac_status_not_for_sale_withdrawn.Attributes.Add("style", "display:none;")
                ac_status_not_for_sale.Attributes.Add("style", "display:none;")
                ac_status_for_sale.Attributes.Add("style", "display:block;")

                If ac_status_for_sale.Items.Contains(New ListItem(CStr(IIf(Not IsDBNull(R("cliaircraft_status")), R("cliaircraft_status"), "")), CStr(IIf(Not IsDBNull(R("cliaircraft_status")), R("cliaircraft_status"), "")))) Then
                Else
                  ac_status_for_sale.Items.Add(New ListItem(CStr(IIf(Not IsDBNull(R("cliaircraft_status")), R("cliaircraft_status"), "")), CStr(IIf(Not IsDBNull(R("cliaircraft_status")), R("cliaircraft_status"), ""))))
                End If 
                ac_status_for_sale.SelectedValue = CStr(IIf(Not IsDBNull(R("cliaircraft_status")), R("cliaircraft_status"), ""))
              ElseIf ac_sale.SelectedValue = "N" And lifecycle_list.SelectedValue = "4" Then
                ac_status_not_for_sale_withdrawn.Attributes.Add("style", "display:block;")
                ac_status_not_for_sale.Attributes.Add("style", "display:none;")
                ac_status_for_sale.Attributes.Add("style", "display:none;")

                If ac_status_not_for_sale_withdrawn.Items.Contains(New ListItem(CStr(IIf(Not IsDBNull(R("cliaircraft_status")), R("cliaircraft_status"), "")), CStr(IIf(Not IsDBNull(R("cliaircraft_status")), R("cliaircraft_status"), "")))) Then
                Else
                  ac_status_not_for_sale_withdrawn.Items.Add(New ListItem(CStr(IIf(Not IsDBNull(R("cliaircraft_status")), R("cliaircraft_status"), "")), CStr(IIf(Not IsDBNull(R("cliaircraft_status")), R("cliaircraft_status"), ""))))
                End If
                
                ac_status_not_for_sale_withdrawn.SelectedValue = CStr(IIf(Not IsDBNull(R("cliaircraft_status")), R("cliaircraft_status"), ""))
              ElseIf ac_sale.SelectedValue = "N" Then
                ac_status_not_for_sale_withdrawn.Attributes.Add("style", "display:none;")
                ac_status_not_for_sale.Attributes.Add("style", "display:block;")
                ac_status_for_sale.Attributes.Add("style", "display:none;")

                If ac_status_not_for_sale.Items.Contains(New ListItem(CStr(IIf(Not IsDBNull(R("cliaircraft_status")), R("cliaircraft_status"), "")), CStr(IIf(Not IsDBNull(R("cliaircraft_status")), R("cliaircraft_status"), "")))) Then
                Else
                  ac_status_not_for_sale.Items.Add(New ListItem(CStr(IIf(Not IsDBNull(R("cliaircraft_status")), R("cliaircraft_status"), "")), CStr(IIf(Not IsDBNull(R("cliaircraft_status")), R("cliaircraft_status"), ""))))
                End If
              
                ac_status_not_for_sale.SelectedValue = CStr(IIf(Not IsDBNull(R("cliaircraft_status")), R("cliaircraft_status"), ""))
              End If

                serial_sort.Text = CStr(IIf(Not IsDBNull(R("cliaircraft_ser_nbr_sort")), R("cliaircraft_ser_nbr_sort"), ""))
                year_manufactured.Text = CStr(IIf(Not IsDBNull(R("cliaircraft_year_mfr")), R("cliaircraft_year_mfr"), ""))
                reg_country.Text = CStr(IIf(Not IsDBNull(R("cliaircraft_country_of_registration")), R("cliaircraft_country_of_registration"), ""))
                jetnet_ac.Text = CStr(IIf(Not IsDBNull(R("cliaircraft_jetnet_ac_id")), R("cliaircraft_jetnet_ac_id"), ""))
                alternate_serial.Text = CStr(IIf(Not IsDBNull(R("cliaircraft_alt_ser_nbr")), R("cliaircraft_alt_ser_nbr"), ""))
                previous_registration.Text = CStr(IIf(Not IsDBNull(R("cliaircraft_prev_reg_nbr")), R("cliaircraft_prev_reg_nbr"), ""))
                iata_code.Text = CStr(IIf(Not IsDBNull(R("cliaircraft_aport_iata_code")), R("cliaircraft_aport_iata_code"), ""))
                icao_code.Text = CStr(IIf(Not IsDBNull(R("cliaircraft_aport_icao_code")), R("cliaircraft_aport_icao_code"), ""))
                airport_name.Text = CStr(IIf(Not IsDBNull(R("cliaircraft_aport_name")), R("cliaircraft_aport_name"), ""))
                airport_state.Text = CStr(IIf(Not IsDBNull(R("cliaircraft_aport_state")), R("cliaircraft_aport_state"), ""))
                year_dlv.Text = CStr(IIf(Not IsDBNull(R("cliaircraft_year_dlv")), R("cliaircraft_year_dlv"), ""))
                new_list.SelectedValue = CStr(IIf(Not IsDBNull(R("cliaircraft_new_flag")), R("cliaircraft_new_flag"), ""))


                DOMlisted.Text = DOM(Now(), R("cliaircraft_date_listed"))
                If Not IsDBNull(R("cliaircraft_asking_wordage")) Then
                  If Trim(UCase(R("cliaircraft_asking_wordage"))) = "PRICE" Then
                    asking_price.Visible = True
                    ask_lbl.Attributes.Add("style", "display:block;")
                    est_label.Attributes.Add("style", "display:block;")
                    est_price.Attributes.Add("style", "display:block;")
                    broker_lbl.Attributes.Add("style", "display:block;")
                    broker_price.Attributes.Add("style", "display:block;")
                    asking_wordage.Attributes.Add("style", "display:block;")
                    date_listed_panel.Attributes.Add("style", "display:block;")
                  End If
                End If
                If R("cliaircraft_forsale_flag") = "Y" Then
                  CompareValidator1.Enabled = True
                  date_listed_panel.Attributes.Add("style", "display:block;")
                  fill_ac_status()
                  est_label.Attributes.Add("style", "display:block;")
                  est_price.Attributes.Add("style", "display:block;")
                  broker_lbl.Attributes.Add("style", "display:block;")
                  broker_price.Attributes.Add("style", "display:block;")
                Else
                  CompareValidator1.Enabled = False
                  date_listed_panel.Attributes.Add("style", "display:block;")
                  date_listed.Text = ""
                  asking_wordage.Items.Add(New ListItem("NONE", ""))
                  asking_wordage.SelectedValue = ""
                  fill_ac_status()
                  asking_price.Text = "0.00"
                  est_price.Text = "0.00"
                  broker_price.Text = "0.00"
                  exclusive_no.Selected = True
                End If
                If Not IsDBNull(R("cliaircraft_aport_country")) Then
                  airport_country.Text = R("cliaircraft_aport_country")
                End If
                If Not IsDBNull(R("cliaircraft_aport_city")) Then
                  aiport_city.Text = R("cliaircraft_aport_city")
                End If

                airport_private.SelectedValue = R("cliaircraft_aport_private")



                'Filling out newly added fields:
                If Not IsDBNull(R("cliaircraft_airframe_total_hours")) Then
                  ac_airframe_total_hours.Text = R("cliaircraft_airframe_total_hours")
                End If
                If Not IsDBNull(R("cliaircraft_airframe_total_landings")) Then
                  ac_airframe_total_landings.Text = R("cliaircraft_airframe_total_landings")
                End If
                If Not IsDBNull(R("cliaircraft_date_engine_times_as_of")) Then
                  ac_date_engine_times_as_of.Text = R("cliaircraft_date_engine_times_as_of")
                End If



                If Not IsDBNull(R("cliaircraft_user_id")) Then
                  aTempTable = aclsData_Temp.Get_Client_User(CInt(R("cliaircraft_user_id")))
                  If Not IsNothing(aTempTable) Then
                    If aTempTable.Rows.Count > 0 Then
                      For Each q As DataRow In aTempTable.Rows
                        update_text.Text = "Last Updated: " & R("cliaircraft_action_date") & "     By: " & q("cliuser_first_name") & " " & q("cliuser_last_name")
                      Next
                    End If
                  Else
                    If aclsData_Temp.class_error <> "" Then
                      error_string = aclsData_Temp.class_error
                      LogError("Aircraft_Edit_Template.ascx.vb - fill_edit_data() - " & error_string & " AC ID " & Session.Item("ListingID"))
                    End If
                    display_error()
                  End If
                End If
            Next
          End If
        Else
          If aclsData_Temp.class_error <> "" Then
            error_string = aclsData_Temp.class_error
            LogError("Aircraft_Edit_Template.ascx.vb - fill_edit_data() - " & error_string & " AC ID " & Session.Item("ListingID"))
          End If
          display_error()
        End If
      End If
    Catch ex As Exception
      error_string = "Aircraft_Edit_Template.ascx.vb - fill_edit_data() - " & ex.Message & " AC ID " & Session.Item("ListingID")
      LogError(error_string)
    End Try
  End Sub
 
#End Region
#Region "Form Events"
  'Private Sub ac_sale_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ac_sale.SelectedIndexChanged


  '  'Try
  '  '  If ac_sale.SelectedValue = "Y" Then
  '  '    CompareValidator1.Enabled = True
  '  '    date_listed_panel.Visible = True
  '  '    fill_ac_status()
  '  '    est_label.Visible = True
  '  '    est_price.Visible = True
  '  '    broker_lbl.Visible = True
  '  '    broker_price.Visible = True
  '  '  Else
  '  '    CompareValidator1.Enabled = False
  '  '    date_listed_panel.Visible = False
  '  '    date_listed.Text = ""
  '  '    asking_wordage.Items.Add(New ListItem("NONE", ""))
  '  '    asking_wordage.SelectedValue = ""
  '  '    fill_ac_status()
  '  '    asking_price.Text = "0.00"
  '  '    est_price.Text = "0.00"
  '  '    broker_price.Text = "0.00"
  '  '    exclusive_no.Selected = True
  '  '  End If
  '  'Catch ex As Exception
  '  '  error_string = "Aircraft_Edit_Template.ascx.vb - ac_sale_SelectedIndexChanged() - " & ex.Message
  '  '  LogError(error_string)
  '  'End Try
  'End Sub
  'Private Sub asking_wordage_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles asking_wordage.SelectedIndexChanged


  '  'Try
  '  '  If asking_wordage.Text = "Price" Then
  '  '    asking_price.Visible = True
  '  '    ask_lbl.Visible = True
  '  '    est_label.Visible = True
  '  '    est_price.Visible = True
  '  '    broker_lbl.Visible = True
  '  '    broker_price.Visible = True
  '  '  Else
  '  '    est_label.Visible = True
  '  '    est_price.Visible = True
  '  '    broker_lbl.Visible = True
  '  '    broker_price.Visible = True
  '  '    asking_price.Visible = False
  '  '    ask_lbl.Visible = False
  '  '    asking_price.Text = "0.00"
  '  '  End If
  '  'Catch ex As Exception
  '  '  error_string = "Aircraft_Edit_Template.ascx.vb - asking_wordage_SelectedIndexChanged() - " & ex.Message
  '  '  LogError(error_string)
  '  'End Try
  'End Sub
  Private Sub fill_ac_status()
    Try
      'ac_status.Items.Clear()
      'ac_status.Items.Add(New ListItem("Other", "Other"))
      If ac_sale.SelectedValue = "Y" Then
        'ac_status.Items.Clear()
        'ac_status.Items.Add(New ListItem("Deal", "Deal"))
        'ac_status.Items.Add(New ListItem("For Sale", "For Sale"))
        'ac_status.Items.Add(New ListItem("For Sale/Best Deal", "For Sale/Best Deal"))
        'ac_status.Items.Add(New ListItem("For Sale/Lease", "For Sale/Lease"))
        'ac_status.Items.Add(New ListItem("For Sale/Off Market", "For Sale/Off Market"))

        'ac_status.Items.Add(New ListItem("For Sale/Possible", "For Sale/Possible"))
        'ac_status.Items.Add(New ListItem("For Sale/Trade", "For Sale/Trade"))
        'ac_status.Items.Add(New ListItem("For Sale/Share", "For Sale/Share"))

        'ac_status.Items.Add(New ListItem("Other", "Other"))
        'ac_status.Items.Add(New ListItem("Sale Pending", "Sale Pending"))
        'ac_status.Items.Add(New ListItem("Unconfirmed", "Unconfirmed"))
        ac_status_for_sale.Attributes.Add("style", "display:block;")
        ac_status_not_for_sale.Attributes.Add("style", "display:none;")
        ac_status_not_for_sale_withdrawn.Attributes.Add("style", "display:none;")

        Try
          ac_status_for_sale.SelectedValue = ac_status_hold.Text
        Catch ex As Exception
          ac_status_for_sale.SelectedValue = "Other"
        End Try

      ElseIf lifecycle_list.SelectedValue = "3" And ac_sale.SelectedValue = "N" Then
        'ac_status.Items.Clear()
        'ac_status.Items.Add(New ListItem("Not For Sale", "Not For Sale"))
        ac_status_for_sale.Attributes.Add("style", "display:none;")
        ac_status_not_for_sale.Attributes.Add("style", "display:block;")
        ac_status_not_for_sale_withdrawn.Attributes.Add("style", "display:none;")


      ElseIf lifecycle_list.SelectedValue = "4" And ac_sale.SelectedValue = "N" Then
        'ac_status.Items.Clear()
        'ac_status.Items.Add(New ListItem("Withdrawn from Use", "Withdrawn from Use"))
        'ac_status.Items.Add(New ListItem("Withdrawn from Use – Display", "Withdrawn from Use – Display"))
        'ac_status.Items.Add(New ListItem("Withdrawn from Use – Stored", "Withdrawn from Use – Stored"))
        'ac_status.Items.Add(New ListItem("Withdrawn from Use – Tech School", "Withdrawn from Use – Tech School"))
        'ac_status.Items.Add(New ListItem("Written Off", "Written Off"))
        'ac_status.Items.Add(New ListItem("Written Accident", "Written Accident"))
        'ac_status.Items.Add(New ListItem("Written Damage", "Written Damage"))
        'ac_status.Items.Add(New ListItem("Written Display", "Written Display"))
        'ac_status.Items.Add(New ListItem("Written - Parted Out", "Written - Parted Out"))
        'ac_status.Items.Add(New ListItem("Written - Fire", "Written - Fire"))
        'ac_status.Items.Add(New ListItem("Written - War Casualty", "Written - War Casualty"))
        'ac_status.Items.Add(New ListItem("Stolen", "Stolen"))
        'ac_status.Items.Add(New ListItem("Other", "Other"))
        ac_status_for_sale.Attributes.Add("style", "display:none;")
        ac_status_not_for_sale.Attributes.Add("style", "display:none;")
        ac_status_not_for_sale_withdrawn.Attributes.Add("style", "display:block;")

        Try
          ac_status_not_for_sale_withdrawn.SelectedValue = ac_status_hold.Text
        Catch ex As Exception
          ac_status_not_for_sale_withdrawn.SelectedValue = "Other"
        End Try
      ElseIf ac_sale.SelectedValue = "N" Then
        'ac_status.Items.Clear()
        'ac_status.Items.Add(New ListItem("Not For Sale", "Not For Sale"))

        ac_status_for_sale.Attributes.Add("style", "display:none;")
        ac_status_not_for_sale.Attributes.Add("style", "display:block;")
        ac_status_not_for_sale_withdrawn.Attributes.Add("style", "display:none;")
      End If
    Catch ex As Exception
      error_string = "Aircraft_Edit_Template.ascx.vb - fill_ac_status() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub
  Private Sub lifecycle_list_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lifecycle_list.SelectedIndexChanged
    fill_ac_status()
  End Sub
  'Private Sub custom_ac_hide_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles custom_ac_hide.Click
  '  Try
  '    model_listing.Visible = False
  '    model_text.Visible = True
  '    model_cbo.Visible = True
  '    model_cbo.Enabled = True
  '  Catch ex As Exception
  '    error_string = "Aircraft_Edit_Template.ascx.vb - custom_ac_hide_Click() - " & ex.Message
  '    LogError(error_string)
  '  End Try
  'End Sub
#End Region
  Public Function DOM(ByVal x As Object, ByVal y As Object) As String
    DOM = ""
    Try

      Dim answer As String = ""
      If Not IsDBNull(x) And Not IsDBNull(y) Then
        answer = CStr(FormatDateTime(CDate(y.ToString), DateFormat.ShortDate))
        If answer = "1/1/1900" Then
          answer = ""
        End If

        If answer <> "" Then
          DOM = "" & DateDiff(DateInterval.Day, y, x) & " Days"
          DOMWord.Visible = True
        Else
          DOM = ""
        End If

      Else
        DOM = ""
      End If
    Catch ex As Exception
      error_string = "Aircraft_Edit_Template.ascx.vb - DOM() - " & ex.Message
      LogError(error_string)
    End Try
  End Function
  Private Sub AddToSubFolder(ByVal idnum As Integer)
    Try
      Dim source As String = "CLIENT"
      Dim ftype As Integer = Session.Item("Listing")
      Dim contact As Integer = 0
      Dim selectedvalue As String = add_folder_cbo.SelectedValue

      If Session.Item("Listing_ContactID") <> 0 Then
        contact = CInt(Session.Item("Listing_ContactID"))
        idnum = CInt(Session.Item("Listing_ContactID"))
      End If


      Dim errored As String = ""
      Select Case ftype
        Case 1
          If contact = 0 Then
            If source = "JETNET" Then
              If aclsData_Temp.Insert_Into_Client_Folder_Index(selectedvalue, 0, idnum, 0, 0, 0, 0, 0, errored) = 1 Then
              Else
                If aclsData_Temp.class_error <> "" Then
                  error_string = aclsData_Temp.class_error
                  LogError("Aircraft_Edit_Template.ascx.vb - AddToSubFolder() - " & error_string)
                End If
                display_error()
              End If
            ElseIf source = "CLIENT" Then
              If aclsData_Temp.Insert_Into_Client_Folder_Index(selectedvalue, 0, 0, 0, 0, idnum, 0, 0, errored) = 1 Then
              Else
                If aclsData_Temp.class_error <> "" Then
                  error_string = aclsData_Temp.class_error
                  LogError("Aircraft_Edit_Template.ascx.vb - AddToSubFolder() - " & error_string)
                End If
                display_error()
              End If
            End If
          Else
            If source = "JETNET" Then
              If aclsData_Temp.Insert_Into_Client_Folder_Index(selectedvalue, 0, 0, contact, 0, 0, 0, 0, errored) = 1 Then
              Else
                If aclsData_Temp.class_error <> "" Then
                  error_string = aclsData_Temp.class_error
                  LogError("Aircraft_Edit_Template.ascx.vb - AddToSubFolder() - " & error_string)
                End If
                display_error()
              End If
            ElseIf source = "CLIENT" Then
              If aclsData_Temp.Insert_Into_Client_Folder_Index(selectedvalue, 0, 0, 0, 0, 0, contact, 0, errored) = 1 Then
              Else
                If aclsData_Temp.class_error <> "" Then
                  error_string = aclsData_Temp.class_error
                  LogError("Aircraft_Edit_Template.ascx.vb - AddToSubFolder() - " & error_string)
                End If
                display_error()
              End If
            End If
          End If

        Case 3

          If source = "JETNET" Then
            If aclsData_Temp.Insert_Into_Client_Folder_Index(selectedvalue, idnum, 0, 0, 0, 0, 0, 0, errored) = 1 Then
            Else
              If aclsData_Temp.class_error <> "" Then
                error_string = aclsData_Temp.class_error
                LogError("Aircraft_Edit_Template.ascx.vb - AddToSubFolder() - " & error_string)
              End If
              display_error()
            End If
          ElseIf source = "CLIENT" Then
            If aclsData_Temp.Insert_Into_Client_Folder_Index(selectedvalue, 0, 0, 0, idnum, 0, 0, 0, errored) = 1 Then
            Else
              If aclsData_Temp.class_error <> "" Then
                error_string = aclsData_Temp.class_error
                LogError("Aircraft_Edit_Template.ascx.vb - AddToSubFolder() - " & error_string)
              End If
              display_error()
            End If
          End If
      End Select

    Catch ex As Exception
      error_string = "Aircraft_Edit_Template.ascx.vb - AddToSubFolder() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub
  Public Function display_error()
    '------------------------------Function that Creates a Javascript Error if the data manager class errors-----------
    display_error = ""
    If aclsData_Temp.class_error <> "" Then
      System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Error", "alert('" & Replace(aclsData_Temp.class_error, "'", " \'") & "');", True)
    End If
    aclsData_Temp.class_error = ""
  End Function
  Public Sub LogError(ByVal ex As String)
    aclsData_Temp.LogError(Application.Item("crmClientSiteData").crmClientHostName, ex, DateTime.Now.ToString())
  End Sub

  Sub Synch_General_Status_Location(ByVal client_id As Integer, ByVal jetnet_id As Integer)
    Try
      Dim enddate As String = ""
      Dim enddated As String = ""
      Dim aError As String = ""
      Dim eoh As String = ""
      Dim hot As String = ""
      Dim aclsUpdate_Client_Aircraft As New clsClient_Aircraft
      Dim aclsUpdate_Client_Aircraft_Engine As New clsClient_Aircraft_Engine

      enddate = DateAdd(DateInterval.Minute, 30, Now())
      enddated = Year(enddate) & "-" & Month(enddate) & "-" & (Day(enddate)) & " " & FormatDateTime(enddate, 4) & ":" & Second(enddate)


      aTempTable = aclsData_Temp.GetJETNET_Aircraft_Details_AC_ID(jetnet_id, "")
      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then
          aclsUpdate_Client_Aircraft.cliaircraft_id = client_id
          aclsUpdate_Client_Aircraft.cliaircraft_action_date = enddated
          aclsUpdate_Client_Aircraft.cliaircraft_ser_nbr = IIf(Not IsDBNull(aTempTable.Rows(0).Item("ac_ser_nbr")), aTempTable.Rows(0).Item("ac_ser_nbr"), "")
          aclsUpdate_Client_Aircraft.cliaircraft_asking_price = IIf(Not IsDBNull(aTempTable.Rows(0).Item("ac_asking_price")), aTempTable.Rows(0).Item("ac_asking_price"), 0)
          aclsUpdate_Client_Aircraft.cliaircraft_asking_wordage = IIf(Not IsDBNull(aTempTable.Rows(0).Item("ac_asking_wordage")), aTempTable.Rows(0).Item("ac_asking_wordage"), "")
          aclsUpdate_Client_Aircraft.cliaircraft_exclusive_flag = IIf(Not IsDBNull(aTempTable.Rows(0).Item("ac_exclusive_flag")), aTempTable.Rows(0).Item("ac_exclusive_flag"), "")
          aclsUpdate_Client_Aircraft.cliaircraft_forsale_flag = IIf(Not IsDBNull(aTempTable.Rows(0).Item("ac_forsale_flag")), aTempTable.Rows(0).Item("ac_forsale_flag"), "")
          aclsUpdate_Client_Aircraft.cliaircraft_lease_flag = IIf(Not IsDBNull(aTempTable.Rows(0).Item("ac_lease_flag")), aTempTable.Rows(0).Item("ac_lease_flag"), "")
          aclsUpdate_Client_Aircraft.cliaircraft_ser_nbr_sort = IIf(Not IsDBNull(aTempTable.Rows(0).Item("ac_ser_nbr_sort")), aTempTable.Rows(0).Item("ac_ser_nbr_sort"), "")
          If Not IsDBNull(aTempTable.Rows(0).Item("ac_date_listed")) Then
            aclsUpdate_Client_Aircraft.cliaircraft_date_listed = IIf(Not IsDBNull(aTempTable.Rows(0).Item("ac_date_listed")), aTempTable.Rows(0).Item("ac_date_listed"), "")
          End If
          If Not IsDBNull(aTempTable.Rows(0).Item("ac_date_purchased")) Then
            aclsUpdate_Client_Aircraft.cliaircraft_date_purchased = IIf(Not IsDBNull(aTempTable.Rows(0).Item("ac_date_purchased")), aTempTable.Rows(0).Item("ac_date_purchased"), "")
          End If


          aclsUpdate_Client_Aircraft.cliaircraft_est_price = 0
          aclsUpdate_Client_Aircraft.cliaircraft_broker_price = 0
          Dim modID As Integer = IIf(Not IsDBNull(aTempTable.Rows(0).Item("ac_amod_id")), aTempTable.Rows(0).Item("ac_amod_id"), 0)
          If modID <> 0 Then
            aTempTable2 = aclsData_Temp.Get_Clients_Aircraft_Model_ByJETNETAmod(modID)

            If Not IsNothing(aTempTable2) Then
              If aTempTable2.Rows.Count > 0 Then
                modID = aTempTable2.Rows(0).Item("cliamod_id")
              Else
                'have to create the model 
                Dim atemptable3 As New DataTable

                atemptable3 = aclsData_Temp.Get_JETNET_Aircraft_Model_amodID(modID)

                Dim aclsInsert_Client_Aircraft_Model As New clsClient_Aircraft_Model
                If Not IsNothing(atemptable3) Then
                  If atemptable3.Rows.Count > 0 Then

                    If Not IsDBNull(atemptable3.Rows(0).Item("amod_airframe_type")) Then
                      aclsInsert_Client_Aircraft_Model.cliamod_airframe_type = atemptable3.Rows(0).Item("amod_airframe_type")
                    End If

                    If Not IsDBNull(atemptable3.Rows(0).Item("amod_make_name")) Then
                      aclsInsert_Client_Aircraft_Model.cliamod_make_name = atemptable3.Rows(0).Item("amod_make_name")
                    End If

                    If Not IsDBNull(atemptable3.Rows(0).Item("amod_make_type")) Then
                      aclsInsert_Client_Aircraft_Model.cliamod_make_type = atemptable3.Rows(0).Item("amod_make_type")
                    End If

                    If Not IsDBNull(atemptable3.Rows(0).Item("amod_manufacturer_name")) Then
                      aclsInsert_Client_Aircraft_Model.cliamod_manufacturer_name = atemptable3.Rows(0).Item("amod_manufacturer_name")
                    End If

                    If Not IsDBNull(atemptable3.Rows(0).Item("amod_model_name")) Then
                      aclsInsert_Client_Aircraft_Model.cliamod_model_name = atemptable3.Rows(0).Item("amod_model_name")
                    End If

                  End If
                End If

                modID = aclsData_Temp.Insert_Client_Aircraft_Model(aclsInsert_Client_Aircraft_Model) 'model doesn't exist - insert it

              End If
            End If
          End If
          aclsUpdate_Client_Aircraft.cliaircraft_cliamod_id = modID
          aclsUpdate_Client_Aircraft.cliaircraft_delivery = IIf(Not IsDBNull(aTempTable.Rows(0).Item("ac_delivery")), aTempTable.Rows(0).Item("ac_delivery"), "")
          aclsUpdate_Client_Aircraft.cliaircraft_reg_nbr = IIf(Not IsDBNull(aTempTable.Rows(0).Item("ac_reg_nbr")), aTempTable.Rows(0).Item("ac_reg_nbr"), "")
          aclsUpdate_Client_Aircraft.cliaircraft_status = IIf(Not IsDBNull(aTempTable.Rows(0).Item("ac_status")), aTempTable.Rows(0).Item("ac_status"), "")
          aclsUpdate_Client_Aircraft.cliaircraft_user_id = Session.Item("localUser").crmLocalUserID
          aclsUpdate_Client_Aircraft.cliaircraft_year_mfr = IIf(Not IsDBNull(aTempTable.Rows(0).Item("ac_year_mfr")), aTempTable.Rows(0).Item("ac_year_mfr"), "")
          aclsUpdate_Client_Aircraft.cliaircraft_jetnet_ac_id = jetnet_id
          aclsUpdate_Client_Aircraft.cliaircraft_action_date = Now()
          aclsUpdate_Client_Aircraft.cliaircraft_lifecycle = IIf(Not IsDBNull(aTempTable.Rows(0).Item("ac_lifecycle")), aTempTable.Rows(0).Item("ac_lifecycle"), 0)
          aclsUpdate_Client_Aircraft.cliaircraft_ownership = IIf(Not IsDBNull(aTempTable.Rows(0).Item("ac_ownership")), aTempTable.Rows(0).Item("ac_ownership"), "")
          aclsUpdate_Client_Aircraft.cliaircraft_year_dlv = IIf(Not IsDBNull(aTempTable.Rows(0).Item("ac_year_dlv")), aTempTable.Rows(0).Item("ac_year_dlv"), "")
          aclsUpdate_Client_Aircraft.cliaircraft_aport_iata_code = IIf(Not IsDBNull(aTempTable.Rows(0).Item("ac_aport_iata_code")), aTempTable.Rows(0).Item("ac_aport_iata_code"), "")
          aclsUpdate_Client_Aircraft.cliaircraft_aport_icao_code = IIf(Not IsDBNull(aTempTable.Rows(0).Item("ac_aport_icao_code")), aTempTable.Rows(0).Item("ac_aport_icao_code"), "")
          aclsUpdate_Client_Aircraft.cliaircraft_aport_name = IIf(Not IsDBNull(aTempTable.Rows(0).Item("ac_aport_name")), aTempTable.Rows(0).Item("ac_aport_name"), "")
          aclsUpdate_Client_Aircraft.cliaircraft_aport_state = IIf(Not IsDBNull(aTempTable.Rows(0).Item("ac_aport_state")), aTempTable.Rows(0).Item("ac_aport_state"), "")
          aclsUpdate_Client_Aircraft.cliaircraft_aport_country = IIf(Not IsDBNull(aTempTable.Rows(0).Item("ac_aport_country")), aTempTable.Rows(0).Item("ac_aport_country"), "")
          aclsUpdate_Client_Aircraft.cliaircraft_country_of_registration = IIf(Not IsDBNull(aTempTable.Rows(0).Item("ac_country_of_registration")), aTempTable.Rows(0).Item("ac_country_of_registration"), "")
          aclsUpdate_Client_Aircraft.cliaircraft_aport_city = IIf(Not IsDBNull(aTempTable.Rows(0).Item("ac_aport_city")), aTempTable.Rows(0).Item("ac_aport_city"), "")
          aclsUpdate_Client_Aircraft.cliaircraft_aport_private = IIf(Not IsDBNull(aTempTable.Rows(0).Item("ac_aport_private")), aTempTable.Rows(0).Item("ac_aport_private"), "")
          aclsUpdate_Client_Aircraft.cliaircraft_prev_reg_nbr = IIf(Not IsDBNull(aTempTable.Rows(0).Item("ac_prev_reg_nbr")), aTempTable.Rows(0).Item("ac_prev_reg_nbr"), "")
          aclsUpdate_Client_Aircraft.cliaircraft_alt_ser_nbr = IIf(Not IsDBNull(aTempTable.Rows(0).Item("ac_alt_ser_nbr")), aTempTable.Rows(0).Item("ac_alt_ser_nbr"), "")
          aclsUpdate_Client_Aircraft.cliaircraft_new_flag = IIf(Not IsDBNull(aTempTable.Rows(0).Item("ac_new_flag")), aTempTable.Rows(0).Item("ac_new_flag"), "")

          aclsUpdate_Client_Aircraft.cliaircraft_ac_maintained = IIf(Not IsDBNull(aTempTable.Rows(0).Item("ac_maintained")), aTempTable.Rows(0).Item("ac_maintained"), "")


          aclsUpdate_Client_Aircraft.cliaircraft_airframe_maintenance_program = IIf(Not IsDBNull(aTempTable.Rows(0).Item("ac_airframe_maintenance_program")), aTempTable.Rows(0).Item("ac_airframe_maintenance_program"), 0)
          aclsUpdate_Client_Aircraft.cliaircraft_airframe_maintenance_tracking_program = IIf(Not IsDBNull(aTempTable.Rows(0).Item("ac_airframe_maintenance_tracking_program")), aTempTable.Rows(0).Item("ac_airframe_maintenance_tracking_program"), 0)

          aclsUpdate_Client_Aircraft.cliaircraft_damage_history_notes = IIf(Not IsDBNull(aTempTable.Rows(0).Item("ac_damage_history_notes")), aTempTable.Rows(0).Item("ac_damage_history_notes"), "")

        End If
      End If

      aTempTable = aclsData_Temp.Get_Clients_Aircraft(client_id)

      If Not IsNothing(aTempTable) Then

        'Details we're swapping out. If these aren't the ones we're changing, this is setting them to the values already in the client db.


        'usage
        If Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_date_engine_times_as_of")) Then
          aclsUpdate_Client_Aircraft.cliaircraft_date_engine_times_as_of = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_date_engine_times_as_of")), aTempTable.Rows(0).Item("cliaircraft_date_engine_times_as_of"), "")
        End If
        If Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_airframe_total_hours")) Then
          aclsUpdate_Client_Aircraft.cliaircraft_airframe_total_hours = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_airframe_total_hours")), aTempTable.Rows(0).Item("cliaircraft_airframe_total_hours"), "")
        End If
        aclsUpdate_Client_Aircraft.cliaircraft_airframe_total_landings = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_airframe_total_landings")), aTempTable.Rows(0).Item("cliaircraft_airframe_total_landings"), 0)

        'engine
        If Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_date_engine_times_as_of")) Then
          aclsUpdate_Client_Aircraft.cliaircraft_date_engine_times_as_of = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_date_engine_times_as_of")), aTempTable.Rows(0).Item("cliaircraft_date_engine_times_as_of"), "")
        End If

        'Apu
        aclsUpdate_Client_Aircraft.cliaircraft_apu_model_name = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_apu_model_name")), aTempTable.Rows(0).Item("cliaircraft_apu_model_name"), "")
        aclsUpdate_Client_Aircraft.cliaircraft_apu_ser_nbr = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_apu_ser_nbr")), aTempTable.Rows(0).Item("cliaircraft_apu_ser_nbr"), "")
        aclsUpdate_Client_Aircraft.cliaircraft_apu_ttsn_hours = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_apu_ttsn_hours")), aTempTable.Rows(0).Item("cliaircraft_apu_ttsn_hours"), 0)
        aclsUpdate_Client_Aircraft.cliaircraft_apu_tsoh_hours = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_apu_tsoh_hours")), aTempTable.Rows(0).Item("cliaircraft_apu_tsoh_hours"), 0)
        aclsUpdate_Client_Aircraft.cliaircraft_apu_tshi_hours = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_apu_tshi_hours")), aTempTable.Rows(0).Item("cliaircraft_apu_tshi_hours"), 0)
        aclsUpdate_Client_Aircraft.cliaircraft_apu_maintance_program = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_apu_maintance_program")), aTempTable.Rows(0).Item("cliaircraft_apu_maintance_program"), "")
        aclsUpdate_Client_Aircraft.cliaircraft_damage_flag = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_damage_flag")), aTempTable.Rows(0).Item("cliaircraft_damage_flag"), "")

        'Interior
        aclsUpdate_Client_Aircraft.cliaircraft_interior_rating = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_interior_rating")), aTempTable.Rows(0).Item("cliaircraft_interior_rating"), 0)
        aclsUpdate_Client_Aircraft.cliaircraft_interior_month_year = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_interior_month_year")), aTempTable.Rows(0).Item("cliaircraft_interior_month_year"), "")
        aclsUpdate_Client_Aircraft.cliaircraft_interior_doneby_name = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_interior_doneby_name")), aTempTable.Rows(0).Item("cliaircraft_interior_doneby_name"), "")
        aclsUpdate_Client_Aircraft.cliaircraft_interior_config_name = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_interior_config_name")), aTempTable.Rows(0).Item("cliaircraft_interior_config_name"), "")

        'Exterior
        aclsUpdate_Client_Aircraft.cliaircraft_exterior_rating = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_exterior_rating")), aTempTable.Rows(0).Item("cliaircraft_exterior_rating"), 0)
        aclsUpdate_Client_Aircraft.cliaircraft_exterior_month_year = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_exterior_month_year")), aTempTable.Rows(0).Item("cliaircraft_exterior_month_year"), "")
        aclsUpdate_Client_Aircraft.cliaircraft_exterior_doneby_name = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_exterior_doneby_name")), aTempTable.Rows(0).Item("cliaircraft_exterior_doneby_name"), "")
        aclsUpdate_Client_Aircraft.cliaircraft_exterior_month_year = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_exterior_month_year")), aTempTable.Rows(0).Item("cliaircraft_exterior_month_year"), "")
        aclsUpdate_Client_Aircraft.cliaircraft_passenger_count = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_passenger_count")), aTempTable.Rows(0).Item("cliaircraft_passenger_count"), 0)
        aclsUpdate_Client_Aircraft.cliaircraft_confidential_notes = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_confidential_notes")), aTempTable.Rows(0).Item("cliaircraft_confidential_notes"), "")


        aclsUpdate_Client_Aircraft.cliaircraft_airframe_maintenance_program = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_airframe_maintenance_program")), aTempTable.Rows(0).Item("cliaircraft_airframe_maintenance_program"), 0)
        aclsUpdate_Client_Aircraft.cliaircraft_airframe_maintenance_tracking_program = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_airframe_maintenance_tracking_program")), aTempTable.Rows(0).Item("cliaircraft_airframe_maintenance_tracking_program"), 0)

        aclsUpdate_Client_Aircraft.cliaircraft_damage_history_notes = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_damage_history_notes")), aTempTable.Rows(0).Item("cliaircraft_damage_history_notes"), "")

        If Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_date_engine_times_as_of")) Then
          aclsUpdate_Client_Aircraft.cliaircraft_date_engine_times_as_of = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_date_engine_times_as_of")), aTempTable.Rows(0).Item("cliaircraft_date_engine_times_as_of"), "")
        End If

        If (Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_airframe_total_hours"))) Then
          aclsUpdate_Client_Aircraft.cliaircraft_airframe_total_hours = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_airframe_total_hours")), aTempTable.Rows(0).Item("cliaircraft_airframe_total_hours"), 0)
        End If

        aclsUpdate_Client_Aircraft.cliaircraft_airframe_total_landings = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_airframe_total_landings")), aTempTable.Rows(0).Item("cliaircraft_airframe_total_landings"), 0)

        If Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_date_engine_times_as_of")) Then
          aclsUpdate_Client_Aircraft.cliaircraft_date_engine_times_as_of = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_date_engine_times_as_of")), aTempTable.Rows(0).Item("cliaircraft_date_engine_times_as_of"), "")
        End If

        'Value Description
        aclsUpdate_Client_Aircraft.cliaircraft_value_description = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_value_description")), aTempTable.Rows(0).Item("cliaircraft_value_description"), "")
        'est price
        aclsUpdate_Client_Aircraft.cliaircraft_est_price = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_est_price")), aTempTable.Rows(0).Item("cliaircraft_est_price"), 0)
        'broker price
        aclsUpdate_Client_Aircraft.cliaircraft_broker_price = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_broker_price")), aTempTable.Rows(0).Item("cliaircraft_broker_price"), 0)


        aclsUpdate_Client_Aircraft.cliaircraft_apu_model_name = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_apu_model_name")), aTempTable.Rows(0).Item("cliaircraft_apu_model_name"), "")
        aclsUpdate_Client_Aircraft.cliaircraft_apu_ser_nbr = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_apu_ser_nbr")), aTempTable.Rows(0).Item("cliaircraft_apu_ser_nbr"), "")
        aclsUpdate_Client_Aircraft.cliaircraft_apu_ttsn_hours = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_apu_ttsn_hours")), aTempTable.Rows(0).Item("cliaircraft_apu_ttsn_hours"), 0)
        aclsUpdate_Client_Aircraft.cliaircraft_apu_tsoh_hours = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_apu_tsoh_hours")), aTempTable.Rows(0).Item("cliaircraft_apu_tsoh_hours"), 0)
        aclsUpdate_Client_Aircraft.cliaircraft_apu_tshi_hours = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_apu_tshi_hours")), aTempTable.Rows(0).Item("cliaircraft_apu_tshi_hours"), 0)
        aclsUpdate_Client_Aircraft.cliaircraft_apu_maintance_program = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_apu_maintance_program")), aTempTable.Rows(0).Item("cliaircraft_apu_maintance_program"), "")
        aclsUpdate_Client_Aircraft.cliaircraft_damage_flag = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_damage_flag")), aTempTable.Rows(0).Item("cliaircraft_damage_flag"), "")


        aclsUpdate_Client_Aircraft.cliaircraft_interior_rating = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_interior_rating")), aTempTable.Rows(0).Item("cliaircraft_interior_rating"), 0)
        aclsUpdate_Client_Aircraft.cliaircraft_interior_month_year = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_interior_month_year")), aTempTable.Rows(0).Item("cliaircraft_interior_month_year"), "")
        aclsUpdate_Client_Aircraft.cliaircraft_interior_doneby_name = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_interior_doneby_name")), aTempTable.Rows(0).Item("cliaircraft_interior_doneby_name"), "")
        aclsUpdate_Client_Aircraft.cliaircraft_interior_config_name = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_interior_config_name")), aTempTable.Rows(0).Item("cliaircraft_interior_config_name"), "")


        aclsUpdate_Client_Aircraft.cliaircraft_exterior_rating = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_exterior_rating")), aTempTable.Rows(0).Item("cliaircraft_exterior_rating"), 0)
        aclsUpdate_Client_Aircraft.cliaircraft_exterior_month_year = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_exterior_month_year")), aTempTable.Rows(0).Item("cliaircraft_exterior_month_year"), "")
        aclsUpdate_Client_Aircraft.cliaircraft_exterior_doneby_name = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_exterior_doneby_name")), aTempTable.Rows(0).Item("cliaircraft_exterior_doneby_name"), "")
        aclsUpdate_Client_Aircraft.cliaircraft_exterior_month_year = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_exterior_month_year")), aTempTable.Rows(0).Item("cliaircraft_exterior_month_year"), "")
        aclsUpdate_Client_Aircraft.cliaircraft_passenger_count = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_passenger_count")), aTempTable.Rows(0).Item("cliaircraft_passenger_count"), 0)
        aclsUpdate_Client_Aircraft.cliaircraft_confidential_notes = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_confidential_notes")), aTempTable.Rows(0).Item("cliaircraft_confidential_notes"), "")

      End If

      If aclsData_Temp.Update_Client_Aircraft(aclsUpdate_Client_Aircraft) = 1 Then
        '
      End If

    Catch ex As Exception
      If aclsData_Temp.class_error <> "" Then
        error_string = aclsData_Temp.class_error
        LogError("Synch_General_Status_Location(ByVal client_id As Integer, ByVal jetnet_id As Integer) - " & error_string)
      Else
        error_string = "Synch_General_Status_Location(ByVal client_id As Integer, ByVal jetnet_id As Integer) - " & ex.Message
        LogError(error_string)
      End If
      display_error()
    End Try
  End Sub



  Sub CheckOffMarketDueToSale(ByRef Jetnet_AC_ID As Long, ByVal Client_AC_ID As Long)
    Dim JetnetViewData As New viewsDataLayer
    Dim JetnetForSaleCheck As New DataTable
    Dim NotForSaleJetnetSide As Boolean = False
    Dim ErrorString As String = ""
    Dim TransTable As New DataTable
    Dim ReturnString As String = ""
    Dim DisplayCount As Integer = 0
    JetnetViewData.clientConnectStr = Session.Item("jetnetClientDatabase") 'Application.Item("crmJetnetDatabase")

    'This is where we need to add a check for client off market aircraft. 
    JetnetForSaleCheck = JetnetViewData.Check_Jetnet_Off_Market_Aircraft(Jetnet_AC_ID)
    If Not IsNothing(JetnetForSaleCheck) Then
      If JetnetForSaleCheck.Rows.Count > 0 Then
        If JetnetForSaleCheck.Rows(0).Item(0) = 0 Then
          NotForSaleJetnetSide = True
        End If
      End If
    End If


    If NotForSaleJetnetSide Then
      TransTable = ReturnApplicableWholeSaleTransactions(Jetnet_AC_ID, ErrorString)

      If Not IsNothing(TransTable) Then
        If TransTable.Rows.Count > 0 Then

          ReturnString = "<table cellspacing=""0"" cellpadding=""4"" border=""0"" class=""data_aircraft_grid white_background_color"" id=""applicable_transactions"">"
          ReturnString += "<tr class=""header_row"">"
          ReturnString += "<td align=""left"" valign=""top"">&nbsp;</td>"
          ReturnString += "<td align=""left"" valign=""top""><b>Date</b></td>"
          ReturnString += "<td align=""left"" valign=""top""><b>Subject</b></td>"
          ReturnString += "</tr>"
          For Each r As DataRow In TransTable.Rows

            If DisplayCount < 5 Then
              Dim ClientTransLookup As New DataTable
              Dim DoNotShowTransaction As Boolean = False

              ClientTransLookup = aclsData_Temp.Get_Client_Client_Transactions(0, r("journ_id"))
              If Not IsNothing(ClientTransLookup) Then
                If ClientTransLookup.Rows.Count > 0 Then
                  'Do not count this
                  DoNotShowTransaction = True
                End If
              End If

              If DoNotShowTransaction = False Then
                OffMarketDueToSale.Visible = True
                DisplayCount += 1
                ReturnString += "<tr>"
                ReturnString += "<td align=""left"" valign=""top""><input type=""radio"" name=""radioSel"" id=""" & r.Item("journ_id") & """ onclick=""$('#" & changeIntoTransaction.ClientID & "').removeClass('display_none');"" /></td>"
                ReturnString += "<td align=""left"" valign=""top"">"
                'Date
                If Not IsDBNull(r("journ_date")) Then
                  ReturnString += FormatDateTime(r.Item("journ_date").ToString, DateFormat.GeneralDate)
                End If
                ReturnString += "</td>"

                ReturnString += "<td align=""left"" valign=""top"">"
                'Date
                If Not IsDBNull(r("journ_date")) Then
                  ReturnString += IIf(Not IsDBNull(r.Item("jcat_subcategory_name")), r.Item("jcat_subcategory_name") & " - ", "") & r.Item("journ_subject")
                  ReturnString += IIf(Not IsDBNull(r.Item("journ_customer_note")), IIf(Not String.IsNullOrEmpty(Trim(r.Item("journ_customer_note").ToString)), "&nbsp;&nbsp;(<a " & crmWebClient.DisplayFunctions.WriteDetailsLink(r.Item("ac_id"), 0, 0, r.Item("journ_id"), False, "", "help_cursor", "") & " title='" & r.Item("journ_customer_note") & "' alt='" & r.Item("journ_customer_note") & "'  class='help_cursor error_text no_text_underline'>Note</a>)", ""), "")
                End If
                ReturnString += "</td>"
                ReturnString += "</tr>"
              End If
            End If
          Next

          ReturnString += "</table>"
        End If
      End If

      applicableTransactions.Text = ReturnString
      changeIntoTransaction.OnClientClick = "javascript:var ran = false;$('#applicable_transactions').find('input').each(function(){if(this.checked){ ran = true;var openerW = window.opener.location.href;windowURL = '/edit.aspx?action=edit&type=transaction&trans=' + this.id + '&assumeID=" & Client_AC_ID & "" & IIf(Not String.IsNullOrEmpty(Trim(Request("from"))), "&from=" & Trim(Request("view")), "") & IIf(Not String.IsNullOrEmpty(Trim(Request("viewNOTEID"))), "&viewNOTEID=" & Trim(Request("viewNOTEID")), "") & IIf(Not String.IsNullOrEmpty(Trim(Request("viewNOTEID"))), "&activetab=" & Trim(Request("activetab")), "") & "&auto_trans=true&opener=' + openerW.replace(""&"",""&amp;"");"
      changeIntoTransaction.OnClientClick += " window.open(windowURL, ""_blank"", ""scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no"");}});if (ran == false) {alert('Please select a transaction first in order to move to a sold record.');$(""#" & changeIntoTransaction.ClientID & """).addClass(""display_none"");};return false;"
    End If



  End Sub


  Public Function ReturnApplicableWholeSaleTransactions(ByRef acID As Long, ByRef ErrorString As String) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim YearDateVariable As String = ""
    Dim start_date As String = ""

    Try

      sQuery.Append("select distinct ac_id, amod_airframe_type_code, amod_type_code,ac_est_airframe_hrs, ac_last_aerodex_event, ac_picture_id, ")
      sQuery.Append(" ac_aport_icao_code,ac_aport_iata_code,aport_latitude_decimal,aport_longitude_decimal, ac_list_date, amod_make_name, ")
      sQuery.Append("amod_model_name,amod_id, ac_mfr_year, ac_forsale_flag, ac_year, ac_ser_no_full,ac_ser_no_sort, ac_reg_no, ac_times_as_of_date, ")
      sQuery.Append(" ac_flights_id, ac_airframe_tot_hrs, ac_engine_1_tot_hrs, ac_engine_2_tot_hrs, ac_engine_3_tot_hrs, ac_engine_4_tot_hrs,  ")
      sQuery.Append(" ac_status, ac_asking, ac_asking_price, ac_delivery,ac_reg_no_search, ac_exclusive_flag, ac_lease_flag, ac_engine_1_soh_hrs, ")
      sQuery.Append(" ac_engine_2_soh_hrs,ac_engine_3_soh_hrs,ac_engine_4_soh_hrs, ac_last_event , ")
      sQuery.Append(" journ_date, journ_subject, journ_id, jcat_subcategory_name, journ_customer_note, ")
      sQuery.Append(" journ_subcat_code_part1 from View_Aircraft_History_Flat with (NOLOCK) where ")
      sQuery.Append(" journ_subcat_code_part1 not in ('OM','MA','MS') and  ( (journ_subcat_code_part1 IN ('WS'))) ")
      sQuery.Append(" and ac_id = " & acID.ToString & " AND amod_customer_flag = 'Y' and (journ_date >= '" & Month(DateAdd(DateInterval.Year, -1, Now())) & "/" & Day(DateAdd(DateInterval.Year, -1, Now())) & "/" & Year(DateAdd(DateInterval.Year, -1, Now())) & "') AND (journ_date <= '" & Month(Now()) & "/" & Day(Now()) & "/" & Year(Now()) & "') ")

      sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, True))

      sQuery.Append(" order by journ_date desc")

      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

      SqlConn.ConnectionString = Session.Item("jetnetClientDatabase") 'Application.Item("crmJetnetDatabase")
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
        ErrorString = "Error in get_retail_sales_info load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      ErrorString = "Error in get_retail_sales_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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



  Sub Synch_Other_Details_On_AC_Record(ByVal client_id As Integer, ByVal jetnet_id As Integer, ByVal type As String)
    Try
      Dim enddate As String = ""
      Dim enddated As String = ""
      Dim aError As String = ""
      Dim eoh As String = ""
      Dim hot As String = ""
      Dim aclsUpdate_Client_Aircraft As New clsClient_Aircraft
      Dim aclsUpdate_Client_Aircraft_Engine As New clsClient_Aircraft_Engine

      enddate = DateAdd(DateInterval.Minute, 30, Now())
      enddated = Year(enddate) & "-" & Month(enddate) & "-" & (Day(enddate)) & " " & FormatDateTime(enddate, 4) & ":" & Second(enddate)


      aTempTable = aclsData_Temp.Get_Clients_Aircraft(client_id)

      If Not IsNothing(aTempTable) Then
        aclsUpdate_Client_Aircraft.cliaircraft_id = client_id
        aclsUpdate_Client_Aircraft.cliaircraft_action_date = enddated
        aclsUpdate_Client_Aircraft.cliaircraft_ser_nbr = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_ser_nbr")), aTempTable.Rows(0).Item("cliaircraft_ser_nbr"), "")
        aclsUpdate_Client_Aircraft.cliaircraft_asking_price = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_asking_price")), aTempTable.Rows(0).Item("cliaircraft_asking_price"), 0)
        aclsUpdate_Client_Aircraft.cliaircraft_asking_wordage = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_asking_wordage")), aTempTable.Rows(0).Item("cliaircraft_asking_wordage"), "")
        aclsUpdate_Client_Aircraft.cliaircraft_exclusive_flag = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_exclusive_flag")), aTempTable.Rows(0).Item("cliaircraft_exclusive_flag"), "")
        aclsUpdate_Client_Aircraft.cliaircraft_forsale_flag = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_forsale_flag")), aTempTable.Rows(0).Item("cliaircraft_forsale_flag"), "")
        aclsUpdate_Client_Aircraft.cliaircraft_lease_flag = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_lease_flag")), aTempTable.Rows(0).Item("cliaircraft_lease_flag"), "")
        aclsUpdate_Client_Aircraft.cliaircraft_ser_nbr_sort = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_ser_nbr_sort")), aTempTable.Rows(0).Item("cliaircraft_ser_nbr_sort"), "")

        If Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_date_listed")) Then
          aclsUpdate_Client_Aircraft.cliaircraft_date_listed = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_date_listed")), aTempTable.Rows(0).Item("cliaircraft_date_listed"), "")
        End If

        If Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_date_purchased")) Then
          aclsUpdate_Client_Aircraft.cliaircraft_date_purchased = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_date_purchased")), aTempTable.Rows(0).Item("cliaircraft_date_purchased"), "")
        End If
        aclsUpdate_Client_Aircraft.cliaircraft_broker_price = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_broker_price")), aTempTable.Rows(0).Item("cliaircraft_broker_price"), 0)
        aclsUpdate_Client_Aircraft.cliaircraft_value_description = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_value_description")), aTempTable.Rows(0).Item("cliaircraft_value_description"), "")

        aclsUpdate_Client_Aircraft.cliaircraft_est_price = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_est_price")), aTempTable.Rows(0).Item("cliaircraft_est_price"), 0)
        aclsUpdate_Client_Aircraft.cliaircraft_cliamod_id = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_cliamod_id")), aTempTable.Rows(0).Item("cliaircraft_cliamod_id"), 0)
        aclsUpdate_Client_Aircraft.cliaircraft_delivery = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_delivery")), aTempTable.Rows(0).Item("cliaircraft_delivery"), "")
        aclsUpdate_Client_Aircraft.cliaircraft_reg_nbr = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_reg_nbr")), aTempTable.Rows(0).Item("cliaircraft_reg_nbr"), "")
        aclsUpdate_Client_Aircraft.cliaircraft_status = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_status")), aTempTable.Rows(0).Item("cliaircraft_status"), "")
        aclsUpdate_Client_Aircraft.cliaircraft_user_id = Session.Item("localUser").crmLocalUserID
        aclsUpdate_Client_Aircraft.cliaircraft_year_mfr = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_year_mfr")), aTempTable.Rows(0).Item("cliaircraft_year_mfr"), "")
        aclsUpdate_Client_Aircraft.cliaircraft_jetnet_ac_id = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_jetnet_ac_id")), aTempTable.Rows(0).Item("cliaircraft_jetnet_ac_id"), 0)
        aclsUpdate_Client_Aircraft.cliaircraft_action_date = Now()
        aclsUpdate_Client_Aircraft.cliaircraft_lifecycle = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_lifecycle")), aTempTable.Rows(0).Item("cliaircraft_lifecycle"), 0)
        aclsUpdate_Client_Aircraft.cliaircraft_ownership = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_ownership")), aTempTable.Rows(0).Item("cliaircraft_ownership"), "")
        aclsUpdate_Client_Aircraft.cliaircraft_year_dlv = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_year_dlv")), aTempTable.Rows(0).Item("cliaircraft_year_dlv"), "")
        aclsUpdate_Client_Aircraft.cliaircraft_aport_iata_code = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_aport_iata_code")), aTempTable.Rows(0).Item("cliaircraft_aport_iata_code"), "")
        aclsUpdate_Client_Aircraft.cliaircraft_aport_icao_code = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_aport_icao_code")), aTempTable.Rows(0).Item("cliaircraft_aport_icao_code"), "")
        aclsUpdate_Client_Aircraft.cliaircraft_aport_name = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_aport_name")), aTempTable.Rows(0).Item("cliaircraft_aport_name"), "")
        aclsUpdate_Client_Aircraft.cliaircraft_aport_state = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_aport_state")), aTempTable.Rows(0).Item("cliaircraft_aport_state"), "")
        aclsUpdate_Client_Aircraft.cliaircraft_aport_country = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_aport_country")), aTempTable.Rows(0).Item("cliaircraft_aport_country"), "")
        aclsUpdate_Client_Aircraft.cliaircraft_country_of_registration = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_country_of_registration")), aTempTable.Rows(0).Item("cliaircraft_country_of_registration"), "")
        aclsUpdate_Client_Aircraft.cliaircraft_aport_city = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_aport_city")), aTempTable.Rows(0).Item("cliaircraft_aport_city"), "")
        aclsUpdate_Client_Aircraft.cliaircraft_aport_private = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_aport_private")), aTempTable.Rows(0).Item("cliaircraft_aport_private"), "")
        aclsUpdate_Client_Aircraft.cliaircraft_prev_reg_nbr = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_prev_reg_nbr")), aTempTable.Rows(0).Item("cliaircraft_prev_reg_nbr"), "")
        aclsUpdate_Client_Aircraft.cliaircraft_alt_ser_nbr = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_alt_ser_nbr")), aTempTable.Rows(0).Item("cliaircraft_alt_ser_nbr"), "")
        aclsUpdate_Client_Aircraft.cliaircraft_new_flag = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_new_flag")), aTempTable.Rows(0).Item("cliaircraft_new_flag"), "")
        aclsUpdate_Client_Aircraft.cliaircraft_airframe_maintenance_program = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_airframe_maintenance_program")), aTempTable.Rows(0).Item("cliaircraft_airframe_maintenance_program"), 0)
        aclsUpdate_Client_Aircraft.cliaircraft_ac_maintained = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_ac_maintained")), aTempTable.Rows(0).Item("cliaircraft_ac_maintained"), "")


        aclsUpdate_Client_Aircraft.cliaircraft_damage_history_notes = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_damage_history_notes")), aTempTable.Rows(0).Item("cliaircraft_damage_history_notes"), "")

        'Details we're swapping out. If these aren't the ones we're changing, this is setting them to the values already in the client db.
        'usage
        If Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_airframe_total_hours")) Then
          aclsUpdate_Client_Aircraft.cliaircraft_airframe_total_hours = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_airframe_total_hours")), aTempTable.Rows(0).Item("cliaircraft_airframe_total_hours"), 0)
        End If
        aclsUpdate_Client_Aircraft.cliaircraft_airframe_total_landings = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_airframe_total_landings")), aTempTable.Rows(0).Item("cliaircraft_airframe_total_landings"), 0)

        'engine
        If Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_date_engine_times_as_of")) Then
          aclsUpdate_Client_Aircraft.cliaircraft_date_engine_times_as_of = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_date_engine_times_as_of")), aTempTable.Rows(0).Item("cliaircraft_date_engine_times_as_of"), "")
        End If
        'Apu
        aclsUpdate_Client_Aircraft.cliaircraft_apu_model_name = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_apu_model_name")), aTempTable.Rows(0).Item("cliaircraft_apu_model_name"), "")
        aclsUpdate_Client_Aircraft.cliaircraft_apu_ser_nbr = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_apu_ser_nbr")), aTempTable.Rows(0).Item("cliaircraft_apu_ser_nbr"), "")
        aclsUpdate_Client_Aircraft.cliaircraft_apu_ttsn_hours = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_apu_ttsn_hours")), aTempTable.Rows(0).Item("cliaircraft_apu_ttsn_hours"), 0)
        aclsUpdate_Client_Aircraft.cliaircraft_apu_tsoh_hours = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_apu_tsoh_hours")), aTempTable.Rows(0).Item("cliaircraft_apu_tsoh_hours"), 0)
        aclsUpdate_Client_Aircraft.cliaircraft_apu_tshi_hours = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_apu_tshi_hours")), aTempTable.Rows(0).Item("cliaircraft_apu_tshi_hours"), 0)
        aclsUpdate_Client_Aircraft.cliaircraft_apu_maintance_program = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_apu_maintance_program")), aTempTable.Rows(0).Item("cliaircraft_apu_maintance_program"), "")
        aclsUpdate_Client_Aircraft.cliaircraft_damage_flag = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_damage_flag")), aTempTable.Rows(0).Item("cliaircraft_damage_flag"), "")

        'Interior
        aclsUpdate_Client_Aircraft.cliaircraft_interior_rating = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_interior_rating")), aTempTable.Rows(0).Item("cliaircraft_interior_rating"), 0)
        aclsUpdate_Client_Aircraft.cliaircraft_interior_month_year = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_interior_month_year")), aTempTable.Rows(0).Item("cliaircraft_interior_month_year"), "")
        aclsUpdate_Client_Aircraft.cliaircraft_interior_doneby_name = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_interior_doneby_name")), aTempTable.Rows(0).Item("cliaircraft_interior_doneby_name"), "")
        aclsUpdate_Client_Aircraft.cliaircraft_interior_config_name = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_interior_config_name")), aTempTable.Rows(0).Item("cliaircraft_interior_config_name"), "")

        'Exterior
        aclsUpdate_Client_Aircraft.cliaircraft_exterior_rating = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_exterior_rating")), aTempTable.Rows(0).Item("cliaircraft_exterior_rating"), 0)
        aclsUpdate_Client_Aircraft.cliaircraft_exterior_month_year = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_exterior_month_year")), aTempTable.Rows(0).Item("cliaircraft_exterior_month_year"), "")
        aclsUpdate_Client_Aircraft.cliaircraft_exterior_doneby_name = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_exterior_doneby_name")), aTempTable.Rows(0).Item("cliaircraft_exterior_doneby_name"), "")
        aclsUpdate_Client_Aircraft.cliaircraft_exterior_month_year = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_exterior_month_year")), aTempTable.Rows(0).Item("cliaircraft_exterior_month_year"), "")
        aclsUpdate_Client_Aircraft.cliaircraft_passenger_count = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_passenger_count")), aTempTable.Rows(0).Item("cliaircraft_passenger_count"), 0)
        aclsUpdate_Client_Aircraft.cliaircraft_confidential_notes = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_confidential_notes")), aTempTable.Rows(0).Item("cliaircraft_confidential_notes"), "")

        aError = ""
        aTempTable = aclsData_Temp.GetJETNET_Aircraft_Details_AC_ID(jetnet_id, aError)
        If Not IsNothing(aTempTable) Then
          If aTempTable.Rows.Count > 0 Then

            Select Case type
              Case "Maintenance"
                aclsUpdate_Client_Aircraft.cliaircraft_ac_maintained = IIf(Not IsDBNull(aTempTable.Rows(0).Item("ac_maintained")), aTempTable.Rows(0).Item("ac_maintained"), "")

                aclsUpdate_Client_Aircraft.cliaircraft_airframe_maintenance_program = IIf(Not IsDBNull(aTempTable.Rows(0).Item("ac_airframe_maintenance_program")), aTempTable.Rows(0).Item("ac_airframe_maintenance_program"), 0)



                aclsUpdate_Client_Aircraft.cliaircraft_airframe_maintenance_tracking_program = IIf(Not IsDBNull(aTempTable.Rows(0).Item("ac_airframe_maintenance_tracking_program")), aTempTable.Rows(0).Item("ac_airframe_maintenance_tracking_program"), 0)

                aclsUpdate_Client_Aircraft.cliaircraft_damage_history_notes = IIf(Not IsDBNull(aTempTable.Rows(0).Item("ac_damage_history_notes")), aTempTable.Rows(0).Item("ac_damage_history_notes"), "")

                aTempTable2 = aclsData_Temp.GetJETNET_Aircraft_Engine(jetnet_id)
                If Not IsNothing(aTempTable2) Then
                  If aTempTable2.Rows.Count > 0 Then
                    'eoh = aTempTable2.Rows(0).Item("ac_maint_eoh_by_name")
                    'hot = aTempTable2.Rows(0).Item("ac_maint_hots_by_name")
                    eoh = IIf(Not IsDBNull(aTempTable2.Rows(0).Item("ac_maint_eoh_by_name")), aTempTable2.Rows(0).Item("ac_maint_eoh_by_name"), "")
                    hot = IIf(Not IsDBNull(aTempTable2.Rows(0).Item("ac_maint_hots_by_name")), aTempTable2.Rows(0).Item("ac_maint_hots_by_name"), "")
                  End If
                End If
                aTempTable2 = aclsData_Temp.Get_Client_Aircraft_Engine(client_id)
                If Not IsNothing(aTempTable2) Then
                  If aTempTable2.Rows.Count > 0 Then
                    For Each R As DataRow In aTempTable2.Rows

                      aclsUpdate_Client_Aircraft_Engine.cliacep_cliac_id = client_id
                      If Not IsDBNull(R("cliacep_engine_name")) Then
                        aclsUpdate_Client_Aircraft_Engine.cliacep_engine_name = IIf(Not IsDBNull(aTempTable2.Rows(0).Item("cliacep_engine_name")), aTempTable2.Rows(0).Item("cliacep_engine_name"), "")
                      End If

                      If Not IsDBNull(R("cliacep_engine_maintenance_program")) Then
                        aclsUpdate_Client_Aircraft_Engine.cliacep_engine_maintenance_program = IIf(Not IsDBNull(aTempTable2.Rows(0).Item("cliacep_engine_maintenance_program")), aTempTable2.Rows(0).Item("cliacep_engine_maintenance_program"), 0)
                      End If
                      If Not IsDBNull(R("cliacep_engine_management_program")) Then
                        aclsUpdate_Client_Aircraft_Engine.cliacep_engine_management_program = IIf(Not IsDBNull(aTempTable2.Rows(0).Item("cliacep_engine_management_program")), aTempTable2.Rows(0).Item("cliacep_engine_management_program"), 0)
                      End If

                      If Not IsDBNull(R("cliacep_engine_tbo_oc_flag")) Then
                        aclsUpdate_Client_Aircraft_Engine.cliacep_engine_tbo_oc_flag = IIf(Not IsDBNull(aTempTable2.Rows(0).Item("cliacep_engine_tbo_oc_flag")), aTempTable2.Rows(0).Item("cliacep_engine_tbo_oc_flag"), "")
                      End If
                      If Not IsDBNull(R("cliacep_engine_noise_rating")) Then
                        aclsUpdate_Client_Aircraft_Engine.cliacep_engine_noise_rating = IIf(Not IsDBNull(aTempTable2.Rows(0).Item("cliacep_engine_noise_rating")), aTempTable2.Rows(0).Item("cliacep_engine_noise_rating"), 0)
                      End If

                      If Not IsDBNull(R("cliacep_engine_model_config")) Then
                        aclsUpdate_Client_Aircraft_Engine.cliacep_engine_model_config = IIf(Not IsDBNull(aTempTable2.Rows(0).Item("cliacep_engine_model_config")), aTempTable2.Rows(0).Item("cliacep_engine_model_config"), "")
                      End If

                      aclsUpdate_Client_Aircraft_Engine.cliacep_engine_overhaul_done_by_name = eoh

                      If Not IsDBNull(R("cliacep_engine_overhaul_done_month_year")) Then
                        aclsUpdate_Client_Aircraft_Engine.cliacep_engine_overhaul_done_month_year = IIf(Not IsDBNull(aTempTable2.Rows(0).Item("cliacep_engine_overhaul_done_month_year")), aTempTable2.Rows(0).Item("cliacep_engine_overhaul_done_month_year"), "")
                      End If

                      aclsUpdate_Client_Aircraft_Engine.cliacep_engine_hot_inspection_done_by_name = hot


                      aclsUpdate_Client_Aircraft_Engine.cliacep_engine_hot_inspection_done_month_year = IIf(Not IsDBNull(aTempTable2.Rows(0).Item("cliacep_engine_hot_inspection_done_month_year")), aTempTable2.Rows(0).Item("cliacep_engine_hot_inspection_done_month_year"), "")


                      If Not IsDBNull(R("cliacep_engine_1_ser_nbr")) Then
                        aclsUpdate_Client_Aircraft_Engine.cliacep_engine_1_ser_nbr = IIf(Not IsDBNull(aTempTable2.Rows(0).Item("cliacep_engine_1_ser_nbr")), aTempTable2.Rows(0).Item("cliacep_engine_1_ser_nbr"), "")
                      End If
                      If Not IsDBNull(R("cliacep_engine_2_ser_nbr")) Then
                        aclsUpdate_Client_Aircraft_Engine.cliacep_engine_2_ser_nbr = IIf(Not IsDBNull(aTempTable2.Rows(0).Item("cliacep_engine_2_ser_nbr")), aTempTable2.Rows(0).Item("cliacep_engine_2_ser_nbr"), "")
                      End If
                      If Not IsDBNull(R("cliacep_engine_3_ser_nbr")) Then
                        aclsUpdate_Client_Aircraft_Engine.cliacep_engine_3_ser_nbr = IIf(Not IsDBNull(aTempTable2.Rows(0).Item("cliacep_engine_3_ser_nbr")), aTempTable2.Rows(0).Item("cliacep_engine_3_ser_nbr"), "")
                      End If
                      If Not IsDBNull(R("cliacep_engine_4_ser_nbr")) Then
                        aclsUpdate_Client_Aircraft_Engine.cliacep_engine_4_ser_nbr = IIf(Not IsDBNull(aTempTable2.Rows(0).Item("cliacep_engine_4_ser_nbr")), aTempTable2.Rows(0).Item("cliacep_engine_4_ser_nbr"), "")
                      End If
                      If Not IsDBNull(R("cliacep_engine_1_ttsn_hours")) Then
                        aclsUpdate_Client_Aircraft_Engine.cliacep_engine_1_ttsn_hours = IIf(Not IsDBNull(aTempTable2.Rows(0).Item("cliacep_engine_1_ttsn_hours")), aTempTable2.Rows(0).Item("cliacep_engine_1_ttsn_hours"), 0)
                      End If
                      If Not IsDBNull(R("cliacep_engine_2_ttsn_hours")) Then
                        aclsUpdate_Client_Aircraft_Engine.cliacep_engine_2_ttsn_hours = IIf(Not IsDBNull(aTempTable2.Rows(0).Item("cliacep_engine_2_ttsn_hours")), aTempTable2.Rows(0).Item("cliacep_engine_2_ttsn_hours"), 0)
                      End If
                      If Not IsDBNull(R("cliacep_engine_3_ttsn_hours")) Then
                        aclsUpdate_Client_Aircraft_Engine.cliacep_engine_3_ttsn_hours = IIf(Not IsDBNull(aTempTable2.Rows(0).Item("cliacep_engine_3_ttsn_hours")), aTempTable2.Rows(0).Item("cliacep_engine_3_ttsn_hours"), 0)
                      End If
                      If Not IsDBNull(R("cliacep_engine_4_ttsn_hours")) Then
                        aclsUpdate_Client_Aircraft_Engine.cliacep_engine_4_ttsn_hours = IIf(Not IsDBNull(aTempTable2.Rows(0).Item("cliacep_engine_4_ttsn_hours")), aTempTable2.Rows(0).Item("cliacep_engine_4_ttsn_hours"), 0)
                      End If
                      If Not IsDBNull(R("cliacep_engine_1_tsoh_hours")) Then
                        aclsUpdate_Client_Aircraft_Engine.cliacep_engine_1_tsoh_hours = IIf(Not IsDBNull(aTempTable2.Rows(0).Item("cliacep_engine_1_tsoh_hours")), aTempTable2.Rows(0).Item("cliacep_engine_1_tsoh_hours"), 0)
                      End If
                      If Not IsDBNull(R("cliacep_engine_2_tsoh_hours")) Then
                        aclsUpdate_Client_Aircraft_Engine.cliacep_engine_2_tsoh_hours = IIf(Not IsDBNull(aTempTable2.Rows(0).Item("cliacep_engine_2_tsoh_hours")), aTempTable2.Rows(0).Item("cliacep_engine_2_tsoh_hours"), 0)
                      End If
                      If Not IsDBNull(R("cliacep_engine_3_tsoh_hours")) Then
                        aclsUpdate_Client_Aircraft_Engine.cliacep_engine_3_tsoh_hours = IIf(Not IsDBNull(aTempTable2.Rows(0).Item("cliacep_engine_3_tsoh_hours")), aTempTable2.Rows(0).Item("cliacep_engine_3_tsoh_hours"), 0)
                      End If
                      If Not IsDBNull(R("cliacep_engine_4_tsoh_hours")) Then
                        aclsUpdate_Client_Aircraft_Engine.cliacep_engine_4_tsoh_hours = IIf(Not IsDBNull(aTempTable2.Rows(0).Item("cliacep_engine_4_tsoh_hours")), aTempTable2.Rows(0).Item("cliacep_engine_4_tsoh_hours"), 0)
                      End If
                      If Not IsDBNull(R("cliacep_engine_1_tshi_hours")) Then
                        aclsUpdate_Client_Aircraft_Engine.cliacep_engine_1_tshi_hours = IIf(Not IsDBNull(aTempTable2.Rows(0).Item("cliacep_engine_1_tshi_hours")), aTempTable2.Rows(0).Item("cliacep_engine_1_tshi_hours"), 0)
                      End If
                      If Not IsDBNull(R("cliacep_engine_2_tshi_hours")) Then
                        aclsUpdate_Client_Aircraft_Engine.cliacep_engine_2_tshi_hours = IIf(Not IsDBNull(aTempTable2.Rows(0).Item("cliacep_engine_2_tshi_hours")), aTempTable2.Rows(0).Item("cliacep_engine_2_tshi_hours"), 0)
                      End If
                      If Not IsDBNull(R("cliacep_engine_3_tshi_hours")) Then
                        aclsUpdate_Client_Aircraft_Engine.cliacep_engine_3_tshi_hours = IIf(Not IsDBNull(aTempTable2.Rows(0).Item("cliacep_engine_3_tshi_hours")), aTempTable2.Rows(0).Item("cliacep_engine_3_tshi_hours"), 0)
                      End If
                      If Not IsDBNull(R("cliacep_engine_4_tshi_hours")) Then
                        aclsUpdate_Client_Aircraft_Engine.cliacep_engine_4_tshi_hours = IIf(Not IsDBNull(aTempTable2.Rows(0).Item("cliacep_engine_4_tshi_hours")), aTempTable2.Rows(0).Item("cliacep_engine_4_tshi_hours"), 0)
                      End If
                      If Not IsDBNull(R("cliacep_engine_1_tbo_hours")) Then
                        aclsUpdate_Client_Aircraft_Engine.cliacep_engine_1_tbo_hours = IIf(Not IsDBNull(aTempTable2.Rows(0).Item("cliacep_engine_1_tbo_hours")), aTempTable2.Rows(0).Item("cliacep_engine_1_tbo_hours"), 0)
                      End If
                      If Not IsDBNull(R("cliacep_engine_2_tbo_hours")) Then
                        aclsUpdate_Client_Aircraft_Engine.cliacep_engine_2_tbo_hours = IIf(Not IsDBNull(aTempTable2.Rows(0).Item("cliacep_engine_2_tbo_hours")), aTempTable2.Rows(0).Item("cliacep_engine_2_tbo_hours"), 0)
                      End If
                      If Not IsDBNull(R("cliacep_engine_3_tbo_hours")) Then
                        aclsUpdate_Client_Aircraft_Engine.cliacep_engine_3_tbo_hours = IIf(Not IsDBNull(aTempTable2.Rows(0).Item("cliacep_engine_3_tbo_hours")), aTempTable2.Rows(0).Item("cliacep_engine_3_tbo_hours"), 0)
                      End If
                      If Not IsDBNull(R("cliacep_engine_4_tbo_hours")) Then
                        aclsUpdate_Client_Aircraft_Engine.cliacep_engine_4_tbo_hours = IIf(Not IsDBNull(aTempTable2.Rows(0).Item("cliacep_engine_4_tbo_hours")), aTempTable2.Rows(0).Item("cliacep_engine_4_tbo_hours"), 0)
                      End If
                      If Not IsDBNull(R("cliacep_engine_1_tsn_cycle")) Then
                        aclsUpdate_Client_Aircraft_Engine.cliacep_engine_1_tsn_cycle = IIf(Not IsDBNull(aTempTable2.Rows(0).Item("cliacep_engine_1_tsn_cycle")), aTempTable2.Rows(0).Item("cliacep_engine_1_tsn_cycle"), 0)
                      End If
                      If Not IsDBNull(R("cliacep_engine_2_tsn_cycle")) Then
                        aclsUpdate_Client_Aircraft_Engine.cliacep_engine_2_tsn_cycle = IIf(Not IsDBNull(aTempTable2.Rows(0).Item("cliacep_engine_2_tsn_cycle")), aTempTable2.Rows(0).Item("cliacep_engine_2_tsn_cycle"), 0)
                      End If
                      If Not IsDBNull(R("cliacep_engine_3_tsn_cycle")) Then
                        aclsUpdate_Client_Aircraft_Engine.cliacep_engine_3_tsn_cycle = IIf(Not IsDBNull(aTempTable2.Rows(0).Item("cliacep_engine_3_tsn_cycle")), aTempTable2.Rows(0).Item("cliacep_engine_3_tsn_cycle"), 0)
                      End If
                      If Not IsDBNull(R("cliacep_engine_4_tsn_cycle")) Then
                        aclsUpdate_Client_Aircraft_Engine.cliacep_engine_4_tsn_cycle = IIf(Not IsDBNull(aTempTable2.Rows(0).Item("cliacep_engine_4_tsn_cycle")), aTempTable2.Rows(0).Item("cliacep_engine_4_tsn_cycle"), 0)
                      End If
                      If Not IsDBNull(R("cliacep_engine_1_tsoh_cycle")) Then
                        aclsUpdate_Client_Aircraft_Engine.cliacep_engine_1_tsoh_cycle = IIf(Not IsDBNull(aTempTable2.Rows(0).Item("cliacep_engine_1_tsoh_cycle")), aTempTable2.Rows(0).Item("cliacep_engine_1_tsoh_cycle"), 0)
                      End If
                      If Not IsDBNull(R("cliacep_engine_2_tsoh_cycle")) Then
                        aclsUpdate_Client_Aircraft_Engine.cliacep_engine_2_tsoh_cycle = IIf(Not IsDBNull(aTempTable2.Rows(0).Item("cliacep_engine_2_tsoh_cycle")), aTempTable2.Rows(0).Item("cliacep_engine_2_tsoh_cycle"), 0)
                      End If
                      If Not IsDBNull(R("cliacep_engine_3_tsoh_cycle")) Then
                        aclsUpdate_Client_Aircraft_Engine.cliacep_engine_3_tsoh_cycle = IIf(Not IsDBNull(aTempTable2.Rows(0).Item("cliacep_engine_3_tsoh_cycle")), aTempTable2.Rows(0).Item("cliacep_engine_3_tsoh_cycle"), 0)
                      End If
                      If Not IsDBNull(R("cliacep_engine_4_tsoh_cycle")) Then
                        aclsUpdate_Client_Aircraft_Engine.cliacep_engine_4_tsoh_cycle = IIf(Not IsDBNull(aTempTable2.Rows(0).Item("cliacep_engine_4_tsoh_cycle")), aTempTable2.Rows(0).Item("cliacep_engine_4_tsoh_cycle"), 0)
                      End If
                      If Not IsDBNull(R("cliacep_engine_1_tshi_cycle")) Then
                        aclsUpdate_Client_Aircraft_Engine.cliacep_engine_1_tshi_cycle = IIf(Not IsDBNull(aTempTable2.Rows(0).Item("cliacep_engine_1_tshi_cycle")), aTempTable2.Rows(0).Item("cliacep_engine_1_tshi_cycle"), 0)
                      End If
                      If Not IsDBNull(R("cliacep_engine_2_tshi_cycle")) Then
                        aclsUpdate_Client_Aircraft_Engine.cliacep_engine_2_tshi_cycle = IIf(Not IsDBNull(aTempTable2.Rows(0).Item("cliacep_engine_2_tshi_cycle")), aTempTable2.Rows(0).Item("cliacep_engine_2_tshi_cycle"), 0)
                      End If
                      If Not IsDBNull(R("cliacep_engine_3_tshi_cycle")) Then
                        aclsUpdate_Client_Aircraft_Engine.cliacep_engine_3_tshi_cycle = IIf(Not IsDBNull(aTempTable2.Rows(0).Item("cliacep_engine_3_tshi_cycle")), aTempTable2.Rows(0).Item("cliacep_engine_3_tshi_cycle"), 0)
                      End If
                      If Not IsDBNull(R("cliacep_engine_4_tshi_cycle")) Then
                        aclsUpdate_Client_Aircraft_Engine.cliacep_engine_4_tshi_cycle = IIf(Not IsDBNull(aTempTable2.Rows(0).Item("cliacep_engine_4_tshi_cycle")), aTempTable2.Rows(0).Item("cliacep_engine_4_tshi_cycle"), 0)
                      End If

                    Next
                  End If
                Else
                  If aclsData_Temp.class_error <> "" Then
                    error_string = aclsData_Temp.class_error
                    LogError("Aircraft_Edit_Details_Tabs.ascx.vb - Save_It() - " & error_string)
                  End If
                End If

                aclsData_Temp.Update_Client_Aircraft_Engine(aclsUpdate_Client_Aircraft_Engine)


              Case "Usage"
                If Not IsDBNull(aTempTable.Rows(0).Item("ac_date_engine_times_as_of")) Then
                  aclsUpdate_Client_Aircraft.cliaircraft_date_engine_times_as_of = IIf(Not IsDBNull(aTempTable.Rows(0).Item("ac_date_engine_times_as_of")), aTempTable.Rows(0).Item("ac_date_engine_times_as_of"), "")
                End If
                If Not IsDBNull(aTempTable.Rows(0).Item("ac_airframe_total_hours")) Then
                  aclsUpdate_Client_Aircraft.cliaircraft_airframe_total_hours = IIf(Not IsDBNull(aTempTable.Rows(0).Item("ac_airframe_total_hours")), aTempTable.Rows(0).Item("ac_airframe_total_hours"), "")
                End If
                aclsUpdate_Client_Aircraft.cliaircraft_airframe_total_landings = IIf(Not IsDBNull(aTempTable.Rows(0).Item("ac_airframe_total_landings")), aTempTable.Rows(0).Item("ac_airframe_total_landings"), 0)

              Case "Engine"

                If Not IsDBNull(aTempTable.Rows(0).Item("ac_date_engine_times_as_of")) Then
                  aclsUpdate_Client_Aircraft.cliaircraft_date_engine_times_as_of = IIf(Not IsDBNull(aTempTable.Rows(0).Item("ac_date_engine_times_as_of")), aTempTable.Rows(0).Item("ac_date_engine_times_as_of"), "")
                End If
              Case "APU"
                aclsUpdate_Client_Aircraft.cliaircraft_apu_model_name = IIf(Not IsDBNull(aTempTable.Rows(0).Item("ac_apu_model_name")), aTempTable.Rows(0).Item("ac_apu_model_name"), "")
                aclsUpdate_Client_Aircraft.cliaircraft_apu_ser_nbr = IIf(Not IsDBNull(aTempTable.Rows(0).Item("ac_apu_ser_nbr")), aTempTable.Rows(0).Item("ac_apu_ser_nbr"), "")
                aclsUpdate_Client_Aircraft.cliaircraft_apu_ttsn_hours = IIf(Not IsDBNull(aTempTable.Rows(0).Item("ac_apu_ttsn_hours")), aTempTable.Rows(0).Item("ac_apu_ttsn_hours"), 0)
                aclsUpdate_Client_Aircraft.cliaircraft_apu_tsoh_hours = IIf(Not IsDBNull(aTempTable.Rows(0).Item("ac_apu_tsoh_hours")), aTempTable.Rows(0).Item("ac_apu_tsoh_hours"), 0)
                aclsUpdate_Client_Aircraft.cliaircraft_apu_tshi_hours = IIf(Not IsDBNull(aTempTable.Rows(0).Item("ac_apu_tshi_hours")), aTempTable.Rows(0).Item("ac_apu_tshi_hours"), 0)
                aclsUpdate_Client_Aircraft.cliaircraft_apu_maintance_program = IIf(Not IsDBNull(aTempTable.Rows(0).Item("ac_apu_maintance_program")), aTempTable.Rows(0).Item("ac_apu_maintance_program"), "")
                aclsUpdate_Client_Aircraft.cliaircraft_damage_flag = IIf(Not IsDBNull(aTempTable.Rows(0).Item("ac_damage_flag")), aTempTable.Rows(0).Item("ac_damage_flag"), "")



              Case "Interior"
                aclsUpdate_Client_Aircraft.cliaircraft_interior_rating = IIf(Not IsDBNull(aTempTable.Rows(0).Item("ac_interior_rating")), aTempTable.Rows(0).Item("ac_interior_rating"), 0)
                aclsUpdate_Client_Aircraft.cliaircraft_interior_month_year = IIf(Not IsDBNull(aTempTable.Rows(0).Item("ac_interior_month_year")), aTempTable.Rows(0).Item("ac_interior_month_year"), "")
                aclsUpdate_Client_Aircraft.cliaircraft_interior_doneby_name = IIf(Not IsDBNull(aTempTable.Rows(0).Item("ac_interior_doneby_name")), aTempTable.Rows(0).Item("ac_interior_doneby_name"), "")
                aclsUpdate_Client_Aircraft.cliaircraft_interior_config_name = IIf(Not IsDBNull(aTempTable.Rows(0).Item("ac_interior_config_name")), aTempTable.Rows(0).Item("ac_interior_config_name"), "")

              Case "Exterior"
                aclsUpdate_Client_Aircraft.cliaircraft_exterior_rating = IIf(Not IsDBNull(aTempTable.Rows(0).Item("ac_exterior_rating")), aTempTable.Rows(0).Item("ac_exterior_rating"), 0)
                aclsUpdate_Client_Aircraft.cliaircraft_exterior_month_year = IIf(Not IsDBNull(aTempTable.Rows(0).Item("ac_exterior_month_year")), aTempTable.Rows(0).Item("ac_exterior_month_year"), "")
                aclsUpdate_Client_Aircraft.cliaircraft_exterior_doneby_name = IIf(Not IsDBNull(aTempTable.Rows(0).Item("ac_exterior_doneby_name")), aTempTable.Rows(0).Item("ac_exterior_doneby_name"), "")
                aclsUpdate_Client_Aircraft.cliaircraft_exterior_month_year = IIf(Not IsDBNull(aTempTable.Rows(0).Item("ac_exterior_month_year")), aTempTable.Rows(0).Item("ac_exterior_month_year"), "")
                aclsUpdate_Client_Aircraft.cliaircraft_passenger_count = IIf(Not IsDBNull(aTempTable.Rows(0).Item("ac_passenger_count")), aTempTable.Rows(0).Item("ac_passenger_count"), 0)
                aclsUpdate_Client_Aircraft.cliaircraft_confidential_notes = IIf(Not IsDBNull(aTempTable.Rows(0).Item("ac_confidential_notes")), aTempTable.Rows(0).Item("ac_confidential_notes"), "")

            End Select

            If aclsData_Temp.Update_Client_Aircraft(aclsUpdate_Client_Aircraft) = 1 Then
              '
            End If
          End If
        End If
      End If
    Catch ex As Exception
      If aclsData_Temp.class_error <> "" Then
        error_string = aclsData_Temp.class_error
        LogError("Synch_Other_Details_On_AC_Record(ByVal client_id As Integer, ByVal jetnet_id As Integer, ByVal type As String) - " & error_string)
      Else
        error_string = "Synch_Other_Details_On_AC_Record(ByVal client_id As Integer, ByVal jetnet_id As Integer, ByVal type As String) - " & ex.Message
        LogError(error_string)
      End If
      display_error()
    End Try
  End Sub

  Private Sub synchronize_buttonFunction_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles synchronize_buttonFunction.Click
    Try
      Dim itemCount As Integer
      Dim updated_string As String = "" 'list of items updated
      itemCount = synch_list.Items.Count
      For i = 0 To (itemCount - 1)
        If synch_list.Items(i).Selected Then
          Select Case synch_list.Items(i).Value
            Case "General/Location/Status"
              Synch_General_Status_Location(Session.Item("ListingID"), Session.Item("OtherID"))
              updated_string = "General/Location/Status, "
            Case "Features"
              'delete
              aclsData_Temp.Delete_Client_Aircraft_Key_Features_ALL(Session.Item("ListingID"))
              'replace
              fill_features(Session.Item("ListingID"), Session.Item("OtherID"))
              updated_string = updated_string & "Features, "
            Case "Engine"
              fill_engine_details(Session.Item("ListingID"), Session.Item("ListingID"), Session.Item("OtherID"))
              updated_string = updated_string & "Engine, "
            Case "Avionics"
              aclsData_Temp.Delete_Client_Aircraft_Avionics(Session.Item("ListingID"))
              fill_avionics(Session.Item("ListingID"), Session.Item("OtherID"))
              updated_string = updated_string & "Avionics, "
            Case "Usage"
              Synch_Other_Details_On_AC_Record(Session.Item("ListingID"), Session.Item("OtherID"), "Usage")
              updated_string = updated_string & "Usage, "
            Case "Maintenance"
              aclsData_Temp.Delete_Client_Aircraft_Details_ALL(Session.Item("ListingID"), "Maintenance")
              fill_ac_details(Session.Item("ListingID"), "Maintenance", Session.Item("OtherID"))
              Synch_Other_Details_On_AC_Record(Session.Item("ListingID"), Session.Item("OtherID"), "Maintenance")
              updated_string = updated_string & "Maintenance, "
            Case "Equipment"
              aclsData_Temp.Delete_Client_Aircraft_Details_ALL(Session.Item("ListingID"), "Equipment")
              fill_ac_details(Session.Item("ListingID"), "Equipment", Session.Item("OtherID"))
              updated_string = updated_string & "Equipment, "
            Case "Interior/Exterior"
              aclsData_Temp.Delete_Client_Aircraft_Details_ALL(Session.Item("ListingID"), "Interior")
              fill_ac_details(Session.Item("ListingID"), "Interior", Session.Item("OtherID"))
              Synch_Other_Details_On_AC_Record(Session.Item("ListingID"), Session.Item("OtherID"), "Interior")
              aclsData_Temp.Delete_Client_Aircraft_Details_ALL(Session.Item("ListingID"), "Exterior")
              Synch_Other_Details_On_AC_Record(Session.Item("ListingID"), Session.Item("OtherID"), "Exterior")
              fill_ac_details(Session.Item("ListingID"), "Exterior", Session.Item("OtherID"))
              updated_string = updated_string & "Interior, Exterior, "
            Case "Cockpit"
              aclsData_Temp.Delete_Client_Aircraft_Details_ALL(Session.Item("ListingID"), "Addl Cockpit Equipment")
              fill_ac_details(Session.Item("ListingID"), "Addl Cockpit Equipment", Session.Item("OtherID"))
              updated_string = updated_string & "Cockpit, "
            Case "APU"
              Synch_Other_Details_On_AC_Record(Session.Item("ListingID"), Session.Item("OtherID"), "APU")
              updated_string = updated_string & "APU, "
            Case "Aircraft Relationships"
              Synch_Relationships()
              updated_string = updated_string & "Aircraft Relationships, "

          End Select
        End If
      Next i
      If updated_string <> "" Then
        updated_string = Trim(updated_string)
        updated_string = UCase(updated_string.TrimEnd(","))
      End If
      synch_note.Text = "<p class='alert_box'>Your record has been updated in the following areas: " & updated_string & "</p>"

      System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.location.reload();", True)
    Catch ex As Exception
      error_string = "Aircraft_Edit_Template.ascx.vb - synchronize_button click(Source: " & Session.Item(Session.Item("ListingSource")) & ". Source ID: " & Session.Item("ListingID") & ", " & Session.Item("OtherID") & ") - " & ex.Message
      LogError(error_string)
    End Try
  End Sub
  Private Sub Synch_Relationships()
    'Add new area checkbox for "Aircraft Relationships" - when this is picked go thru the following steps. 
    'Get list of jetnet aircraft relationships . Check each one to see if a corresponding company and contact exist in client records. 
    'o If not, then create them .... When creating them you can either copy the entire company with contacts - or just make 
    'a copy of the related company and contact whichever is easier. 
    'o if yes then you are all set to insert references . Then delete all existing client references and insert all others with 
    'their client company and contact ids 


    'Step 1: Add a new area Checkbox for Aircraft Relationships and when this is picked, go through the following:
    'Step 2: Get a list of jetnet Aircraft Relationships. 
    Dim contact_id As Integer = 0
    Dim company_id As Integer = 0
    Dim add_reference As Boolean = False
    Dim contact_type As String = ""
    Dim operator_flag As String = ""
    Dim owner_percentage As Double = 0

    Dim contact_priority As Integer = 0
    Dim date_fraction_expires As String = ""
    Dim date_fraction_purchased As String = ""
    Dim business_type As String = ""
    Dim jetnet_comp_id As Integer = 0

    'A-okay. Let's delete the references that are here first.
    If aclsData_Temp.Delete_Client_Aircraft_Reference_by_Client_AC(Session.Item("ListingID")) = True Then
      'references deleted
    Else
      If aclsData_Temp.class_error <> "" Then
        error_string = aclsData_Temp.class_error
        LogError("Aircraft_Edit_Template.ascx.vb - synchronize_button_click()- Aircraft Relationships - " & error_string)
      End If
      display_error()
    End If

    aTempTable = aclsData_Temp.Get_Aircraft_Relationship_By_AC(Session.Item("OtherID"))
    If Not IsNothing(aTempTable) Then
      If aTempTable.Rows.Count > 0 Then
        For Each r As DataRow In aTempTable.Rows
          'Check each one to see if there's a corresponding company that exists. 
          'Setting all the variables.
          company_id = IIf(Not IsDBNull(r("acref_comp_id")), r("acref_comp_id"), 0) 'Jetnet ID and needs to be changed. 
          contact_id = IIf(Not IsDBNull(r("acref_contact_id")), r("acref_contact_id"), 0) 'Jetnet ID and needs to be changed. 
          contact_type = IIf(Not IsDBNull(r("acref_contact_type")), r("acref_contact_type"), "")
          operator_flag = IIf(Not IsDBNull(r("acref_operator_flag")), r("acref_operator_flag"), "")
          owner_percentage = IIf(Not IsDBNull(r("acref_owner_percentage")), r("acref_owner_percentage"), 0)
          date_fraction_expires = IIf(Not IsDBNull(r("acref_date_fraction_expires")), r("acref_date_fraction_expires"), "")
          date_fraction_purchased = IIf(Not IsDBNull(r("acref_date_fraction_purchased")), r("acref_date_fraction_purchased"), "")
          business_type = IIf(Not IsDBNull(r("acref_business_type")), r("acref_business_type"), "")


          If company_id <> 0 Then
            aTempTable2 = aclsData_Temp.GetCompanyInfo_JETNET_ID(r("acref_comp_id"), "")
            If Not IsNothing(aTempTable2) Then
              If aTempTable2.Rows.Count > 0 Then
                company_id = IIf(Not IsDBNull(aTempTable2.Rows(0).Item("comp_id")), aTempTable2.Rows(0).Item("comp_id"), 0)
                add_reference = True

                If r("acref_contact_id") <> 0 Then 'Make sure there is a contact
                  Dim aDataTable As DataTable = aclsData_Temp.GetContacts_Details_JETNETID(r("acref_contact_id"))
                  If Not IsNothing(aDataTable) Then
                    If aDataTable.Rows.Count > 0 Then
                      contact_id = IIf(Not IsDBNull(aDataTable.Rows(0).Item("contact_id")), aDataTable.Rows(0).Item("contact_id"), 0)
                      add_reference = True
                    End If
                  End If
                Else
                  contact_id = 0
                  add_reference = True
                End If
              Else
                add_reference = False
                aTempTable = aclsData_Temp.GetCompanyInfo_ID(company_id, "JETNET", 0)
                If Not IsNothing(aTempTable) Then 'not nothing
                  'This jetnet record isn't in a company record yet, so let's insert it.
                  Dim aclsClient_Company As New clsClient_Company
                  For Each compInfo As DataRow In aTempTable.Rows
                    If Not IsDBNull(compInfo("comp_name")) Then
                      aclsClient_Company.clicomp_name = compInfo("comp_name")
                    End If
                    If Not IsDBNull(compInfo("comp_alternate_name_type")) Then
                      aclsClient_Company.clicomp_alternate_name_type = compInfo("comp_alternate_name_type")
                    End If

                    If Not IsDBNull(compInfo("comp_alternate_name")) Then
                      aclsClient_Company.clicomp_alternate_name = compInfo("comp_alternate_name")
                    End If

                    If Not IsDBNull(compInfo("comp_address1")) Then
                      aclsClient_Company.clicomp_address1 = compInfo("comp_address1")
                    End If
                    If Not IsDBNull(compInfo("comp_address2")) Then
                      aclsClient_Company.clicomp_address2 = compInfo("comp_address2")
                    End If
                    If Not IsDBNull(compInfo("comp_city")) Then
                      aclsClient_Company.clicomp_city = compInfo("comp_city")
                    End If
                    If Not IsDBNull(compInfo("comp_state")) Then
                      aclsClient_Company.clicomp_state = compInfo("comp_state")
                    End If
                    If Not IsDBNull(compInfo("comp_zip_code")) Then
                      aclsClient_Company.clicomp_zip_code = compInfo("comp_zip_code")
                    End If
                    If Not IsDBNull(compInfo("comp_country")) Then
                      aclsClient_Company.clicomp_country = compInfo("comp_country")
                    End If
                    If Not IsDBNull(compInfo("comp_agency_type")) Then
                      aclsClient_Company.clicomp_agency_type = compInfo("comp_agency_type")
                    End If
                    If Not IsDBNull(compInfo("comp_web_address")) Then
                      aclsClient_Company.clicomp_web_address = compInfo("comp_web_address")
                    End If
                    If Not IsDBNull(compInfo("comp_email_address")) Then
                      aclsClient_Company.clicomp_email_address = compInfo("comp_email_address")
                    End If

                    aclsClient_Company.clicomp_date_updated = Now()
                    aclsClient_Company.clicomp_jetnet_comp_id = compInfo("comp_id")
                    jetnet_comp_id = compInfo("comp_id")
                    'inserting that info into the database. 
                    If aclsData_Temp.Insert_Client_Company(aclsClient_Company) = True Then
                      aTempTable = aclsData_Temp.GetCompanyInfo_JETNET_ID(jetnet_comp_id, "")
                      If Not IsNothing(aTempTable) Then 'not nothing
                        For Each z As DataRow In aTempTable.Rows

                          aTempTable2 = aclsData_Temp.GetPhoneNumbers(jetnet_comp_id, 0, "JETNET", 0)
                          If Not IsNothing(aTempTable) Then
                            If aTempTable.Rows.Count > 0 Then
                              For Each q As DataRow In aTempTable2.Rows
                                Dim aclsClient_Phone_Numbers As New clsClient_Phone_Numbers
                                company_id = z("comp_id")
                                aclsClient_Phone_Numbers.clipnum_type = IIf(Not IsDBNull(q("pnum_type")), q("pnum_type"), "")
                                aclsClient_Phone_Numbers.clipnum_number = IIf(Not IsDBNull(q("pnum_number")), q("pnum_number"), "")
                                aclsClient_Phone_Numbers.clipnum_comp_id = z("comp_id") 'This is the comp_id of the new company we just inserted.
                                aclsClient_Phone_Numbers.clipnum_contact_id = 0
                                If aclsData_Temp.Insert_Client_PhoneNumbers(aclsClient_Phone_Numbers) = True Then
                                  ' Response.Write("insert contact phone Number<br />")
                                Else
                                  If aclsData_Temp.class_error <> "" Then
                                    error_string = aclsData_Temp.class_error
                                    LogError("Aircraft_Edit_Template.ascx.vb - synchronize_button_click()- Aircraft Relationships - " & error_string)
                                  End If
                                  display_error()
                                End If
                              Next 'for each in get phone numbers
                            End If
                          Else
                            If aclsData_Temp.class_error <> "" Then
                              error_string = aclsData_Temp.class_error
                              LogError("Aircraft_Edit_Template.ascx.vb - synchronize_button_click()- Aircraft Relationships - " & error_string)
                            End If
                            display_error()
                          End If
                        Next
                      End If
                    End If
                  Next
                End If


                RaiseEvent loop_contacts(company_id, jetnet_comp_id, contact_id, False, False, Session.Item("ListingID"), Session.Item("OtherID"))
                Dim aclsClient_Contact As New clsClient_Contact

                aTempTable = aclsData_Temp.GetContacts_Details(contact_id, "JETNET")
                If Not IsNothing(aTempTable) Then
                  If aTempTable.Rows.Count > 0 Then

                    For Each contactINFO As DataRow In aTempTable.Rows

                      aclsClient_Contact = New clsClient_Contact
                      'This is where I insert that last one.
                      aclsClient_Contact.clicontact_sirname = IIf(Not IsDBNull(contactINFO("contact_sirname")), CStr(contactINFO("contact_sirname")), "")
                      aclsClient_Contact.clicontact_first_name = IIf(Not IsDBNull(contactINFO("contact_first_name")), CStr(contactINFO("contact_first_name")), "")
                      aclsClient_Contact.clicontact_middle_initial = IIf(Not IsDBNull(contactINFO("contact_middle_initial")), CStr(contactINFO("contact_middle_initial")), "")
                      aclsClient_Contact.clicontact_last_name = IIf(Not IsDBNull(contactINFO("contact_last_name")), CStr(contactINFO("contact_last_name")), "")
                      aclsClient_Contact.clicontact_suffix = IIf(Not IsDBNull(contactINFO("contact_suffix")), CStr(contactINFO("contact_suffix")), "")
                      aclsClient_Contact.clicontact_title = IIf(Not IsDBNull(contactINFO("contact_title")), CStr(contactINFO("contact_title")), "")
                      aclsClient_Contact.clicontact_email_address = IIf(Not IsDBNull(contactINFO("contact_email_address")), CStr(contactINFO("contact_email_address")), "")
                      aclsClient_Contact.clicontact_date_updated = Now()
                      aclsClient_Contact.clicontact_status = "Y"
                      ' set to 0 since this is a Client record
                      aclsClient_Contact.clicontact_jetnet_contact_id = contact_id
                      aclsClient_Contact.clicontact_comp_id = company_id

                      'Now finally we insert the contact. 
                      If aclsData_Temp.Insert_Client_Contact(aclsClient_Contact) = True Then
                        '  Response.Write("Insert Client Contact Success")
                        'And closes the form and sends the user on their way. 
                        aTempTable2 = aclsData_Temp.GetContactInfo_JETNET_ID(contact_id, "Y")
                        If Not IsNothing(aTempTable2) Then 'not nothing
                          'Insert the new phone numbers
                          If aTempTable2.Rows.Count > 0 Then
                            contact_id = aTempTable2.Rows(0).Item("contact_id")
                          End If
                        Else
                          If aclsData_Temp.class_error <> "" Then
                            error_string = aclsData_Temp.class_error
                            LogError("Aircraft_Edit_Template.ascx.vb - synchronize_button_click()- Aircraft Relationships - " & error_string)
                          End If
                          display_error()
                        End If
                      End If

                      add_reference = True
                    Next
                  End If
                End If

              End If
            End If
            If add_reference = True Then
              '  Response.Write("We're okay to start adding the reference.")

              Dim aclsInsert_Client_Aircraft_Reference As New clsClient_Aircraft_Reference
              aclsInsert_Client_Aircraft_Reference.cliacref_comp_id = company_id
              aclsInsert_Client_Aircraft_Reference.cliacref_contact_type = contact_type 'relationship_con.SelectedValue
              aclsInsert_Client_Aircraft_Reference.cliacref_contact_id = contact_id
              aclsInsert_Client_Aircraft_Reference.cliacref_jetnet_ac_id = 0
              aclsInsert_Client_Aircraft_Reference.cliacref_cliac_id = Session.Item("ListingID")
              aclsInsert_Client_Aircraft_Reference.cliacref_operator_flag = operator_flag
              aclsInsert_Client_Aircraft_Reference.cliacref_owner_percentage = owner_percentage
              aclsInsert_Client_Aircraft_Reference.cliacref_contact_priority = 0
              If date_fraction_expires <> "" Then
                aclsInsert_Client_Aircraft_Reference.cliacref_date_fraction_expires = date_fraction_expires
              End If
              If date_fraction_purchased <> "" Then
                aclsInsert_Client_Aircraft_Reference.cliacref_date_fraction_purchased = date_fraction_purchased
              End If
              aclsInsert_Client_Aircraft_Reference.cliacref_business_type = business_type

              'Response.Write(aclsInsert_Client_Aircraft_Reference.ClassInfo(aclsInsert_Client_Aircraft_Reference))
              If aclsData_Temp.Insert_Client_Aircraft_Reference(aclsInsert_Client_Aircraft_Reference) = True Then
                ' Response.Write("added") 
              Else
                If aclsData_Temp.class_error <> "" Then
                  error_string = aclsData_Temp.class_error
                  LogError("Aircraft_Edit_Template.ascx.vb - synchronize_button_click()- Aircraft Relationships - " & error_string)
                End If
                display_error()
              End If
            End If
          End If
        Next
      End If
    Else
      'Error
      If aclsData_Temp.class_error <> "" Then
        error_string = aclsData_Temp.class_error
        LogError("Aircraft_Edit_Template.ascx.vb - synchronize_button_click()- Aircraft Relationships - " & error_string)
      End If
      display_error()
    End If
  End Sub
  Private Sub synch_list_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles synch_list.SelectedIndexChanged
    synchronize_buttonFunction.Visible = False
    Dim itemCount As Integer
    itemCount = synch_list.Items.Count
    For i = 0 To (itemCount - 1)
      If synch_list.Items(i).Selected Then
        synchronize_buttonFunction.Visible = True
      End If
    Next i
  End Sub

  Private Sub deleteFunction_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles deleteFunction.Click
    Remove_Aircraft()
  End Sub


  Private Sub Remove_Aircraft()
    Dim url As String = ""

    Session.Item("Listing") = 3
    Session.Item("FromTypeOfListing") = 3
    If Not IsNothing(Session.Item("ListingID")) Then
      If Not String.IsNullOrEmpty(Session.Item("ListingID").ToString) Then
        If IsNumeric(Session.Item("ListingID")) Then
          aclsData_Temp.Delete_Client_Aircraft(Session.Item("ListingID"))
        End If
      End If
    End If



    If fromVIEW = True Then
      If viewNOTEID <> 0 Then
        url = "view_template.aspx?ViewID=19&noteID=" & viewNOTEID & "&noMaster=false"
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.location = '" & url & "';", True)
      Else
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.location.reload();", True)
      End If
      System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)
    Else
      Session.Item("ListingID") = 0
      url = "listing_air.aspx?removed=true"

      If Not IsNothing(Trim(Request("from"))) Then
        If Not String.IsNullOrEmpty(Trim(Request("from"))) Then
          If LCase(Trim(Request("from"))) = "aircraftdetails" Then
            url = "/DisplayAircraftDetail.aspx?acid=" & jetnet_ac.Text
          End If
        End If
      End If


      System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.location = '" & url & "';", True)
      System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)
    End If

  End Sub



  Private Sub FillUpForSaleParametersFromURL()
    Dim logGeneration As String = ""

    'Broker Price
    If Not IsNothing(Trim(Request("brokp"))) Then
      If Not String.IsNullOrEmpty(Trim(Request("brokp"))) Then
        logGeneration += "Broker Price Changed from '" & Trim(Request("obrokp")) & "' to '" & Trim(Request("brokp")) & "'*"
        broker_price.Text = Trim(Request("brokp"))
      End If
    End If

    'Take price
    If Not IsNothing(Trim(Request("estp"))) Then
      If Not String.IsNullOrEmpty(Trim(Request("estp"))) Then
        logGeneration += "Take Price Changed from '" & Trim(Request("oestp")) & "' to '" & Trim(Request("estp")) & "'*"
        est_price.Text = Trim(Request("estp"))
      End If
    End If

    'Asking Price
    If Not IsNothing(Trim(Request("askp"))) Then
      If Not String.IsNullOrEmpty(Trim(Request("askp"))) Then
        logGeneration += "Asking Price Changed from '" & Trim(Request("oaskp")) & "' to '" & Trim(Request("askp")) & "'*"
        asking_price.Text = Trim(Request("askp"))
      End If
    End If

    'Asking Wordage
    If Not IsNothing(Trim(Request("askw"))) Then
      If Not String.IsNullOrEmpty(Trim(Request("askw"))) Then
        logGeneration += "Asking Wordage Changed from '" & Trim(Request("oaskw")) & "' to '" & Trim(Request("askw")) & "'*"
        asking_wordage.SelectedValue = Trim(Request("askw"))
      End If
    End If

    'Date Listed:
    If Not IsNothing(Trim(Request("datel"))) Then
      If Not String.IsNullOrEmpty(Trim(Request("datel"))) Then
        logGeneration += "Date Listed Changed from '" & Trim(Request("odatel")) & "' to '" & Trim(Request("datel")) & "'*"
        date_listed.Text = Trim(Request("datel"))

      End If
    End If

    'Value description:
    If Not IsNothing(Trim(Request("vdesc"))) Then
      If Not String.IsNullOrEmpty(Trim(Request("vdesc"))) Then
        logGeneration += "Value Description Changed from '" & Trim(Request("ovdesc")) & "' to '" & Trim(Request("vdesc")) & "'*"
        cliaircraft_value_description_text.Text = Trim(Request("vdesc"))
      End If
    End If

    'Ac For Sale
    If Not IsNothing(Trim(Request("forSale"))) Then
      If Not String.IsNullOrEmpty(Trim(Request("forSale"))) Then
        logGeneration += "For Sale Changed from '" & IIf(Trim(Request("forSale")) = "Y", "No", "Yes") & "' to '" & IIf(Trim(Request("forSale")) = "Y", "Yes", "No") & "'*"
        ac_sale.SelectedValue = Trim(Request("forSale"))
      End If
    End If

    'Ac Status

    If ac_sale.SelectedValue = "Y" Then
      If Not IsNothing(Trim(Request("status"))) Then
        If Not String.IsNullOrEmpty(Trim(Request("status"))) Then
          logGeneration += "Status Changed from '" & Trim(Request("ostatus")) & "' to '" & Trim(Request("status")) & "'*"
          ac_status_for_sale.SelectedValue = Trim(Request("status"))
          ac_status_hold.Text = Trim(Request("status"))
        End If
      End If
    ElseIf ac_sale.SelectedValue = "N" And lifecycle_list.SelectedValue = "4" Then
      If Not IsNothing(Trim(Request("status"))) Then
        If Not String.IsNullOrEmpty(Trim(Request("status"))) Then
          Try
            logGeneration += "Status Changed from '" & Trim(Request("ostatus")) & "' to '" & Trim(Request("status")) & "'*"
            ac_status_not_for_sale.SelectedValue = Trim(Request("status"))
            ac_status_not_for_sale_withdrawn.SelectedValue = Trim(Request("status"))
            ac_status_hold.Text = Trim(Request("status"))
          Catch

          End Try
        End If
      End If
    End If

    logGenerated.Text = logGeneration
  End Sub
  Public Sub set_preferences()
    Try


      If HttpContext.Current.Session.Item("localUser").crmUser_Evo_MPM_Flag = True Then
        If aclsData_Temp.client_DB = "" Then
          aclsData_Temp.client_DB = HttpContext.Current.Session.Item("localPreferences").ServerNotesDatabaseConn
        End If
      End If

      aTempTable = aclsData_Temp.Get_Client_Preferences()
      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then
          For Each r As DataRow In aTempTable.Rows
            If Not IsDBNull(r("clipref_ac_custom_1_use")) Then
              If r("clipref_ac_custom_1_use") = "Y" Then
                ac_cat1.Visible = True
                ac_cat1_text.Visible = True
                cat1row.Visible = True
                ac_cat1_text.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_1")), r("clipref_ac_custom_1"), ""))
                ac_cat1.Attributes.Add("alt", CStr(IIf(Not IsDBNull(r("clipref_ac_custom_1")), r("clipref_ac_custom_1"), "")))
              Else
                ac_cat1.Visible = False
                ac_cat1_text.Visible = False
                ac_cat1_text.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_1")), r("clipref_ac_custom_1"), ""))
              End If
            End If

            If Not IsDBNull(r("clipref_ac_custom_2_use")) Then
              If r("clipref_ac_custom_2_use") = "Y" Then
                ac_cat2.Visible = True
                ac_cat2_text.Visible = True
                cat2row.Visible = True
                ac_cat2_text.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_2")), r("clipref_ac_custom_2"), ""))
                ac_cat2.Attributes.Add("alt", CStr(IIf(Not IsDBNull(r("clipref_ac_custom_2")), r("clipref_ac_custom_2"), "")))
              Else
                ac_cat2.Visible = False
                ac_cat2_text.Visible = False
                ac_cat2_text.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_2")), r("clipref_ac_custom_2"), ""))
              End If
            End If

            If Not IsDBNull(r("clipref_ac_custom_3_use")) Then
              If r("clipref_ac_custom_3_use") = "Y" Then
                ac_cat3.Visible = True
                ac_cat3_text.Visible = True
                cat3row.Visible = True
                ac_cat3_text.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_3")), r("clipref_ac_custom_3"), ""))
                ac_cat3.Attributes.Add("alt", CStr(IIf(Not IsDBNull(r("clipref_ac_custom_3")), r("clipref_ac_custom_3"), "")))
              Else
                ac_cat3.Visible = False
                ac_cat3_text.Visible = False
                ac_cat3_text.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_3")), r("clipref_ac_custom_3"), ""))
              End If
            End If

            If Not IsDBNull(r("clipref_ac_custom_4_use")) Then
              If r("clipref_ac_custom_4_use") = "Y" Then
                ac_cat4.Visible = True
                cat4row.Visible = True
                ac_cat4_text.Visible = True
                ac_cat4_text.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_4")), r("clipref_ac_custom_4"), ""))
                ac_cat4.Attributes.Add("alt", CStr(IIf(Not IsDBNull(r("clipref_ac_custom_4")), r("clipref_ac_custom_4"), "")))
              Else
                ac_cat4.Visible = False
                ac_cat4_text.Visible = False
                ac_cat4_text.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_4")), r("clipref_ac_custom_4"), ""))
              End If
            End If


            If Not IsDBNull(r("clipref_ac_custom_5_use")) Then
              If r("clipref_ac_custom_5_use") = "Y" Then
                ac_cat5.Visible = True
                ac_cat5_text.Visible = True
                cat5row.Visible = True
                ac_cat5_text.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_5")), r("clipref_ac_custom_5"), ""))
                ac_cat5.Attributes.Add("alt", CStr(IIf(Not IsDBNull(r("clipref_ac_custom_5")), r("clipref_ac_custom_5"), "")))
              Else
                ac_cat5.Visible = False
                ac_cat5_text.Visible = False
                ac_cat5_text.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_5")), r("clipref_ac_custom_5"), ""))
              End If
            End If


            If Not IsDBNull(r("clipref_ac_custom_6_use")) Then
              If r("clipref_ac_custom_6_use") = "Y" Then
                ac_cat6.Visible = True
                cat6row.Visible = True
                ac_cat6_text.Visible = True
                ac_cat6_text.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_6")), r("clipref_ac_custom_6"), ""))
                ac_cat6.Attributes.Add("alt", CStr(IIf(Not IsDBNull(r("clipref_ac_custom_6")), r("clipref_ac_custom_6"), "")))
              Else
                ac_cat6.Visible = False
                ac_cat6_text.Visible = False
                ac_cat6_text.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_6")), r("clipref_ac_custom_6"), ""))
              End If
            End If

            If Not IsDBNull(r("clipref_ac_custom_7_use")) Then
              If r("clipref_ac_custom_7_use") = "Y" Then
                ac_cat7.Visible = True
                cat7row.Visible = True
                ac_cat7_text.Visible = True
                ac_cat7_text.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_7")), r("clipref_ac_custom_7"), ""))
                ac_cat7.Attributes.Add("alt", CStr(IIf(Not IsDBNull(r("clipref_ac_custom_7")), r("clipref_ac_custom_7"), "")))
              Else
                ac_cat7.Visible = False
                ac_cat7_text.Visible = False
                ac_cat7_text.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_7")), r("clipref_ac_custom_7"), ""))
              End If
            End If


            If Not IsDBNull(r("clipref_ac_custom_8_use")) Then
              If r("clipref_ac_custom_8_use") = "Y" Then
                ac_cat8.Visible = True
                cat8row.Visible = True
                ac_cat8_text.Visible = True
                ac_cat8_text.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_8")), r("clipref_ac_custom_8"), ""))
                ac_cat8.Attributes.Add("alt", CStr(IIf(Not IsDBNull(r("clipref_ac_custom_8")), r("clipref_ac_custom_8"), "")))
              Else
                ac_cat8.Visible = False
                ac_cat8_text.Visible = False
                ac_cat8_text.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_8")), r("clipref_ac_custom_8"), ""))
              End If
            End If

            If Not IsDBNull(r("clipref_ac_custom_9_use")) Then
              If r("clipref_ac_custom_9_use") = "Y" Then
                ac_cat9.Visible = True
                cat9row.Visible = True
                ac_cat9_text.Visible = True
                ac_cat9_text.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_9")), r("clipref_ac_custom_9"), ""))
                ac_cat9.Attributes.Add("alt", CStr(IIf(Not IsDBNull(r("clipref_ac_custom_9")), r("clipref_ac_custom_9"), "")))
              Else
                ac_cat9.Visible = False
                ac_cat9_text.Visible = False
                ac_cat9_text.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_9")), r("clipref_ac_custom_9"), ""))
              End If
            End If

            If Not IsDBNull(r("clipref_ac_custom_10_use")) Then
              If r("clipref_ac_custom_10_use") = "Y" Then
                ac_cat10.Visible = True
                cat10row.Visible = True
                ac_cat10_text.Visible = True
                ac_cat10_text.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_10")), r("clipref_ac_custom_10"), ""))
                ac_cat10.Attributes.Add("alt", CStr(IIf(Not IsDBNull(r("clipref_ac_custom_10")), r("clipref_ac_custom_10"), "")))
              Else
                ac_cat10.Visible = False
                ac_cat10_text.Visible = False
                ac_cat10_text.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_10")), r("clipref_ac_custom_10"), ""))
              End If
            End If
          Next
        End If
      Else
        If aclsData_Temp.class_error <> "" Then
          error_string = aclsData_Temp.class_error
          LogError("Company_Edit_Template.ascx.vb - Set_Preferences() - " & error_string)
        End If
        display_error()
      End If
    Catch ex As Exception
      error_string = "Company_Edit_Template.ascx.vb - Set_Preferences() " & ex.Message
      LogError(error_string)
    End Try
  End Sub



End Class