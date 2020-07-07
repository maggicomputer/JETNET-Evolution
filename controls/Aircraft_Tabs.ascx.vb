Imports System.Web.UI
Partial Public Class Aircraft_Tabs
  Inherits System.Web.UI.UserControl
  Dim table As DataTable
  Public aTempTable, aTempTable2 As New DataTable 'Data Tables used
  Dim error_string As String = ""
  Public Event Notes(ByVal text As String, ByVal cat_name As String, ByVal main_id As Integer, ByVal cat_id As Integer, ByVal action As Boolean, ByVal label As Label, ByVal Notes_Data As DataTable)
  Dim Aircraft_Data As New clsClient_Aircraft
  Dim Aircraft_Table As New DataTable
  Dim Notes_Data As DataTable
  Dim Action_Data As DataTable
  Dim Prospect_Data As DataTable
  Dim Document_Data As DataTable
  Dim Value_Data As DataTable
  Dim ParPage As _details
  Dim localDatalayer As New viewsDataLayer


  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Me.Visible = True Then
      If Session.Item("crmUserLogon") = True Then
        ParPage = CType(Parent.Page, _details)
        Aircraft_Data = ParPage.Aircraft_Data
        Dim masterPage As main_site = DirectCast(Page.Master, main_site)
        Try
          masterPage.SetAircraftValuationLink = Replace(masterPage.SetAircraftValuationLink, "THETABCONTAINERID", tabs_container.ClientID.ToString)
          masterPage.SetAircraftValuationLink = Replace(masterPage.SetAircraftValuationLink, "THETABID", value_tab.ClientID.ToString)

          Session("export_info") = ""

          If Session.Item("localSubscription").crmDocumentsFlag = True Then
            opportunities_tab.Visible = True
          End If

          If Session.Item("localSubscription").crmAerodexFlag Then
            value_tab.Visible = False
          End If

          If Session.Item("localUser").crmEvo = True Then 'If an EVO user
            opportunities_tab.Visible = False
            action_tab.Visible = False
            notes_tab.Visible = False
          End If


          If Not IsNothing(ViewState("Notes_Data")) Then
            Notes_Data = DirectCast(ViewState("Notes_Data"), DataTable)
          End If
          If Not IsNothing(ViewState("Action_Data")) Then
            Action_Data = DirectCast(ViewState("Action_Data"), DataTable)
          End If
          If Not IsNothing(ViewState("Document_Data")) Then
            Document_Data = DirectCast(ViewState("Document_Data"), DataTable)
          End If
          If Not IsNothing(ViewState("Prospect_Data")) Then
            Prospect_Data = DirectCast(ViewState("Prospect_Data"), DataTable)
          End If
          If Not IsNothing(ViewState("Aircraft_Data")) Then
            Aircraft_Table = DirectCast(ViewState("Aircraft_Data"), DataTable)
            If masterPage.ListingSource = "CLIENT" Then
              Aircraft_Data = clsGeneral.clsGeneral.Create_Aircraft_Class(Aircraft_Table, "cliaircraft")
            ElseIf masterPage.ListingSource = "JETNET" Then
              Aircraft_Data = clsGeneral.clsGeneral.Create_Aircraft_Class(Aircraft_Table, "ac")
            End If

            'aTempTable.Dispose()
          End If

          If Not Page.IsPostBack Then
            trans_warning_text.Text = "<p align='center' class='red'>Please wait while the transaction information loads.</p>"
            avionics_warning_text.Text = "<p align='center'>Please wait while the avionics information loads.</p>"
            props_warning_text.Text = "<p align='center'>Please wait while the propeller information loads.</p>"
            apu_warning_text.Text = "<p align='center'>Please wait while the apu information loads.</p>"
            usuage_warning_text.Text = "<p align='center'>Please wait while the usuage information loads.</p>"
            int_warning_text.Text = "<p align='center'>Please wait while the interior/exterior information loads.</p>"
            maint_warning_text.Text = "<p align='center'>Please wait while the maintenance information loads.</p>"
            cockpit_warning_text.Text = "<p align='center'>Please wait while the cockpit information loads.</p>"
            equipment_warning_text.Text = "<p align='center'>Please wait while the equipment information loads.</p>"
            event_warning_text.Text = "<p align='center'>Please wait while the events loads.</p>"
            engine_warning_text.Text = "<p align='center'>Please wait while the engine information loads.</p>"
          End If

          Try 'This is only an attempt to set the active tab index. 
            If Not Page.IsPostBack Then 'meaning this only happens on a real page refresh and not on a tab post back
              tabs_container.ActiveTabIndex = Session.Item("ac_active_tab")
            End If
          Catch

          End Try

          Try
            If Not Page.IsPostBack Then
              Changed_Tab(False)

              If Aircraft_Table.Rows.Count > 0 Then
                If masterPage.ListingSource = "CLIENT" Then
                  changeProspectDropdown.Items.Add(New ListItem("Display Prospects for My Aircraft or " & Aircraft_Table.Rows(0).Item("cliamod_make_name") & " " & Aircraft_Table.Rows(0).Item("cliamod_model_name") & " (but not other Aircraft)", 2))
                  changeProspectDropdown.Items.Add(New ListItem("Display All " & Aircraft_Table.Rows(0).Item("cliamod_make_name") & " " & Aircraft_Table.Rows(0).Item("cliamod_model_name") & " Prospects", 3))

                Else
                  changeProspectDropdown.Items.Add(New ListItem("Display Prospects for My Aircraft or " & Aircraft_Table.Rows(0).Item("amod_make_name") & " " & Aircraft_Table.Rows(0).Item("amod_model_name") & " (but not other Aircraft)", 2))
                  changeProspectDropdown.Items.Add(New ListItem("Display All " & Aircraft_Table.Rows(0).Item("amod_make_name") & " " & Aircraft_Table.Rows(0).Item("amod_model_name") & " Prospects", 3))
                End If
              End If

            End If
          Catch ex As Exception
            error_string = "aircraft_tabs.ascx.vb - Page_Load - Error in Changed_Tab() " & ex.Message & " AC ID:" & masterPage.ListingID & " S:" & masterPage.ListingSource
            masterPage.LogError(error_string)
          End Try


          Try
            Fill_Notes_Data_On_Change() 'Doesn't matter how many times we do this, it is just a fill function, no db 
          Catch ex As Exception
            error_string = "aircraft_tabs.ascx.vb - Page_Load - Error in FillNotesOnDataChange() " & ex.Message & " AC ID:" & masterPage.ListingID & " S:" & masterPage.ListingSource
            masterPage.LogError(error_string)
          End Try


          If Not IsNumeric(Trim(Request("startCount"))) Then
            ' tabs_container.ActiveTabIndex = 0
          Else
            tabs_container.ActiveTabIndex = 11
          End If


          '---------------------------------------------End Database Connection Stuff---------------------------------------------
        Catch ex As Exception
          error_string = "aircraft_tabs.ascx.vb - Page_Load - " & ex.Message & " AC ID:" & masterPage.ListingID & " S:" & masterPage.ListingSource
          masterPage.LogError(error_string)
        End Try
      End If
    End If
  End Sub
  Public Sub Show_Jetnet_Tabs(ByVal show As CheckBox)
    feature_tab_time.Text = ""
    engine_tab_time.Text = ""
    trans_tab_time.Text = ""
    avionics_tab_time.Text = ""
    other_tab_time.Text = ""
    events_tab_time.Text = ""
    props_tab_time.Text = ""
    If show.Checked Then
      Changed_Tab(True)
    Else
      Changed_Tab(False)
    End If
  End Sub
  Public Sub Consume_Aircraft_Data(ByVal Aircraft_Table As clsClient_Aircraft, ByVal tempTable As DataTable)
    ViewState("Aircraft_Data") = tempTable
    Aircraft_Data = Aircraft_Table 'DirectCast(ViewState("Aircraft_Data"), clsClient_Aircraft)
  End Sub
  Public Sub Consume_Notes_Data(ByVal Notes_Table As DataTable)
    ViewState("Notes_Data") = Notes_Table
    Notes_Data = DirectCast(ViewState("Notes_Data"), DataTable)
  End Sub
  Public Sub Consume_Document_Data(ByVal Document_Table As DataTable)
    ViewState("Document_Data") = Document_Table
    Document_Data = DirectCast(ViewState("Document_Data"), DataTable)
  End Sub
  Public Sub Fill_Notes_Data_On_Change()

    Dim masterPage As main_site = DirectCast(Page.Master, main_site)
    Dim FEATURES_ID As Integer = 0
    Dim ENGINE_ID As Integer = 0
    Dim TRANSACTIONS_ID As Integer = 0
    Dim AVIONICS_ID As Integer = 0
    Dim INTERIOR_ID As Integer = 0
    Dim EXTERIOR_ID As Integer = 0
    Dim APU_ID As Integer = 0
    Dim MAINTENANCE_ID As Integer = 0
    Dim EQUIPMENT_ID As Integer = 0
    Dim COCKPIT_ID As Integer = 0
    Dim USAGE_ID As Integer = 0
    Dim PROPELLER_ID As Integer = 0
    Dim VALUE_ID As Integer = 0

    'All the notes categories are interchangeable. Learned that the hard way, so have to do a seperate lookup and store the ID. 
    Try
      If Session.Item("localUser").crmEvo <> True Then
        aTempTable = masterPage.aclsData_Temp.Get_Client_Note_Category
        If Not IsNothing(aTempTable) Then
          If aTempTable.Rows.Count > 0 Then
            For Each R As DataRow In aTempTable.Rows
              Select Case UCase(R("notecat_name"))
                Case "FEATURES"
                  FEATURES_ID = R("notecat_key")
                Case "ENGINE"
                  ENGINE_ID = R("notecat_key")
                Case "TRANSACTIONS"
                  TRANSACTIONS_ID = R("notecat_key")
                Case "AVIONICS"
                  AVIONICS_ID = R("notecat_key")
                Case "INTERIOR"
                  INTERIOR_ID = R("notecat_key")
                Case "EXTERIOR"
                  EXTERIOR_ID = R("notecat_key")
                Case "APU"
                  APU_ID = R("notecat_key")
                Case "MAINTENANCE"
                  MAINTENANCE_ID = R("notecat_key")
                Case "EQUIPMENT"
                  EQUIPMENT_ID = R("notecat_key")
                Case "COCKPIT"
                  COCKPIT_ID = R("notecat_key")
                Case "USAGE"
                  USAGE_ID = R("notecat_key")
                Case "PROPELLER"
                  PROPELLER_ID = R("notecat_key")
                Case "PRICE/STATUS"
                  VALUE_ID = R("notecat_key")
              End Select
            Next
          Else
          End If
        End If
      Else
      End If
    Catch ex As Exception
      error_string = "aircraft_Tabs.ascx.vb Tab Note Category - " & ex.Message
      masterPage.LogError(error_string)
    End Try

    aTempTable = Nothing
    'Fill NOTES for Features. 
    If Not IsNothing(Notes_Data) Then
      'Adding next/previous to notes data
      Notes_Data = clsGeneral.clsGeneral.AddNextPreviousToNotesTable(Notes_Data)

      RaiseEvent Notes("", "FEATURES", masterPage.ListingID, FEATURES_ID, False, features_label_notes, Notes_Data)
      'Fill NOTES For Engine
      RaiseEvent Notes("", "ENGINE", masterPage.ListingID, ENGINE_ID, False, engine_label_notes, Notes_Data)
      'Fill NOTES for Transaction
      RaiseEvent Notes("", "TRANSACTIONS", masterPage.ListingID, TRANSACTIONS_ID, False, trans_label_notes, Notes_Data)
      'Fill NOTES for Avionics 
      RaiseEvent Notes("", "AVIONICS", masterPage.ListingID, AVIONICS_ID, False, avionics_label_notes, Notes_Data)
      'Fill NOTES for Usage
      RaiseEvent Notes("", "USAGE", masterPage.ListingID, USAGE_ID, False, usage_label_notes, Notes_Data)
      'Fill NOTES for APU
      RaiseEvent Notes("", "APU", masterPage.ListingID, APU_ID, False, apu_label_notes, Notes_Data)
      'Fill NOTES for Interior
      RaiseEvent Notes("", "INTERIOR", masterPage.ListingID, INTERIOR_ID, False, interior_label_notes, Notes_Data)
      'Fill Notes for Exterior
      RaiseEvent Notes("", "EXTERIOR", masterPage.ListingID, EXTERIOR_ID, False, exterior_label_notes, Notes_Data)
      'Fill Notes for Maintenance
      RaiseEvent Notes("", "MAINTENANCE", masterPage.ListingID, MAINTENANCE_ID, False, maitenance_label_notes, Notes_Data)
      'Fill Notes for Equipment
      RaiseEvent Notes("", "EQUIPMENT", masterPage.ListingID, EQUIPMENT_ID, False, equipment_label_notes, Notes_Data)
      'Fill Notes for Cockpit
      RaiseEvent Notes("", "COCKPIT", masterPage.ListingID, COCKPIT_ID, False, cockpit_label_notes, Notes_Data)
      'Fill Notes for Propeller
      RaiseEvent Notes("", "PROPELLER", masterPage.ListingID, PROPELLER_ID, False, props_label_notes, Notes_Data)
      'Fill Notes for Value
      If VALUE_ID > 0 Then
        RaiseEvent Notes("", "PRICE/STATUS", masterPage.ListingID, VALUE_ID, False, value_label_notes, Notes_Data)
      End If

      'Fill ALL Notes

      Dim data As DataTable = Notes_Data.Clone
      Dim startCount As Integer = 0
      Dim endCount As Integer = 10
      If Not IsNothing(Trim(Request("startCount"))) Then
        If IsNumeric(Trim(Request("startCount"))) Then
          startCount = Trim(Request("startCount"))
          endCount = startCount + 10
        End If
      End If

      If startCount = 0 Then 'We're only checking on the session item existing if there's no request variable passed.
        If Not IsNothing(Trim(Session.Item("startCount"))) Then 'We check existence of session item.
          If IsNumeric(Trim(Session.Item("startCount"))) Then 'Check for numeric
            If Session.Item("startCount") > 0 Then 'Then make sure it's greater than 0.
              startCount = Session.Item("startCount") 'We set the start count to the session item that's set on the notes control.
              endCount = startCount + 10 'have to set an end count of + 10
              'Important note: On the company tab/aircraft tab pages, we will not clear this session variable.
              'That's because we need it on the details.aspx page and that loads after (load complete event). 
              'We will clear it on that.
            End If
          End If
        End If
      End If


      If Notes_Data.Rows.Count > 0 Then
        data = clsGeneral.clsGeneral.limit_rows(Notes_Data, startCount, endCount)
      End If

      RaiseEvent Notes("", "NOTES", masterPage.ListingID, 0, False, notes_list, data)
      'Fill ALL Actions
      RaiseEvent Notes("", "ACTION", masterPage.ListingID, 0, True, action_label, Action_Data)
      'Fill All Documents
      RaiseEvent Notes("", "DOCUMENTS", masterPage.ListingID, 0, False, document_label, Document_Data)

      'Fill Prospects
      'Using this as a test for right now. Will be moving this to set up like the other ones when SQL data manager is checked in.
      RaiseEvent Notes("", "PROSPECT", masterPage.ListingID, 0, False, prospect_label, Prospect_Data)


      ' value_tab.Visible = True
    End If

  End Sub
  Public Sub Consume_Action_Data(ByVal Action_Table As DataTable)
    ViewState("Action_Data") = Action_Table
    Action_Table = DirectCast(ViewState("Action_Data"), DataTable)
  End Sub
  Public Sub Consume_Prospect_Data(ByVal Prospect_Table As DataTable)
    ViewState("Prospect_Data") = Prospect_Table
    Prospect_Data = DirectCast(ViewState("Prospect_Data"), DataTable)
  End Sub
  Public Sub Consume_Value_Data(ByVal Value_Table As DataTable)
    ViewState("Value_Data") = Value_Table
    Value_Data = DirectCast(ViewState("Value_Data"), DataTable)
  End Sub
  Public Sub Changed_Tab(ByVal show_jetnet As Boolean) 'Handles tabs_container.ActiveTabChanged

    Dim masterPage As main_site = DirectCast(Page.Master, main_site)
    masterPage.CheckVisibilityForJetnetClient()
    Dim jetnet_id As Integer = 0
    Dim client_id As Integer = 0
    Dim jetnet_id_transaction As Integer = 0
    Dim client_id_transaction As Integer = 0
    Dim Flight_Table As New DataTable
    Select Case masterPage.ListingSource
      Case "JETNET"
        jetnet_id = masterPage.ListingID
        jetnet_id_transaction = masterPage.ListingID
        If show_jetnet = True Then
          client_id = masterPage.OtherID
        End If
        client_id_transaction = masterPage.OtherID
      Case "CLIENT"
        jetnet_id_transaction = masterPage.OtherID
        If show_jetnet = True Then
          jetnet_id = masterPage.OtherID
        End If
        client_id_transaction = masterPage.ListingID
        client_id = masterPage.ListingID
    End Select


    Session.Item("ac_active_tab") = tabs_container.ActiveTabIndex
    Select Case tabs_container.ActiveTab.ID
      Case "features_tab"
        If Not IsDate(feature_tab_time.Text) Then
          features_label.Text = clsGeneral.clsGeneral.Build_JETNET_Features_Tab(jetnet_id, masterPage.ListingSource, masterPage.ListingID, Nothing, masterPage)
          features_label_client.Text = clsGeneral.clsGeneral.Build_CLIENT_Features_Tab(client_id, masterPage.ListingSource, masterPage.ListingID, Nothing, masterPage)
          feature_tab_time.Text = Now()
        End If

      Case "value_tab"
        'Fill Value (for now)
        '  If aircraft_value_time.Text = "" Then 'This hasn't been ran yet.
        FillValueTable(masterPage)
        '  End If

      Case "engine_tab"
        engine_warning_text.Text = ""
        If Not IsDate(engine_tab_time.Text) Then

          If masterPage.ListingSource = "CLIENT" Then
            engine_label.Text = clsGeneral.clsGeneral.Build_JETNET_Engine_Tab(Nothing, jetnet_id, masterPage.ListingID, "JETNET", Nothing, masterPage)
          Else
            engine_label.Text = clsGeneral.clsGeneral.Build_JETNET_Engine_Tab(Aircraft_Table, jetnet_id, masterPage.ListingID, "JETNET", Nothing, masterPage)
          End If

          engine_label_client.Text = clsGeneral.clsGeneral.Build_CLIENT_Engine_Tab(client_id, masterPage.ListingID, "CLIENT", Nothing, masterPage)
          engine_tab_time.Text = Now()
        End If
      Case "transaction_tab"
        trans_warning_text.Text = ""
        If Not IsDate(trans_tab_time.Text) Then

          clsGeneral.clsGeneral.Build_Transaction_Tab(jetnet_id_transaction, client_id_transaction, masterPage.OtherID, masterPage.ListingID, masterPage.ListingSource, Nothing, masterPage, "both", trans_label, Nothing)
          trans_tab_time.Text = Now()
        End If
      Case "avionics_tab"
        avionics_warning_text.Text = ""
        If Not IsDate(avionics_tab_time.Text) Then

          avionics_label.Text = clsGeneral.clsGeneral.Build_JETNET_Avionics_Tab(jetnet_id, masterPage.ListingID, "JETNET", Nothing, masterPage)
          avionics_label_client.Text = clsGeneral.clsGeneral.Build_CLIENT_Avionics_Tab(client_id, masterPage.ListingID, "CLIENT", Nothing, masterPage)
          avionics_tab_time.Text = Now()
        End If
      Case "props_tab"
        props_warning_text.Text = ""
        If Not IsDate(props_tab_time.Text) Then

          Build_Propeller_Tab(jetnet_id, client_id, masterPage.ListingID, masterPage.ListingSource, masterPage)
          props_tab_time.Text = Now()
        End If
      Case "event_tab"
        event_warning_text.Text = ""
        If Not IsDate(events_tab_time.Text) Then

          If jetnet_id_transaction <> 0 Then
            event_label.Text = clsGeneral.clsGeneral.Build_Event_Tab(jetnet_id_transaction, masterPage.OtherID, masterPage.ListingID, masterPage.ListingSource, Nothing, masterPage)
            If event_label.Text = "" Then
              event_status.Text = "<p align='center'>There are no current events for this aircraft.</p>"
            End If
          Else
            event_status.Text = "<p align='center'>There are no current events for this aircraft.</p>"
          End If
          events_tab_time.Text = Now()
        End If
      Case "apu_tab", "usage_tab", "int_tab", "maint_tab", "cockpit_tab", "equipment_tab"
        apu_warning_text.Text = ""
        usuage_warning_text.Text = ""
        int_warning_text.Text = ""
        maint_warning_text.Text = ""
        cockpit_warning_text.Text = ""
        equipment_warning_text.Text = ""

        If Not IsDate(other_tab_time.Text) Then


          Dim Client_Aircraft_Data As New clsClient_Aircraft
          Dim Jetnet_Aircraft_Data As New clsClient_Aircraft
          '-----------------------------------------JETNET TAB DETAIL INFORMATION----------------------------------------------------------------------
          If jetnet_id <> 0 Then
            ' check the state of the DataTable
            If Not IsNothing(Aircraft_Data) Then
              If masterPage.ListingSource = "JETNET" Then 'This is fine and correct,
                Jetnet_Aircraft_Data = Aircraft_Data
              Else
                'NO WE HAVE TO POLL THE DATABASE FOR THIS AC INFO AGAIN.
                aTempTable = masterPage.aclsData_Temp.GetJETNET_Aircraft_Details_AC_ID(jetnet_id, "")
                Jetnet_Aircraft_Data = clsGeneral.clsGeneral.Create_Aircraft_Class(aTempTable, "ac")
                aTempTable.Dispose()
              End If
            End If
          End If
          If client_id <> 0 Then
            ' check the state of the DataTable
            If masterPage.ListingSource = "CLIENT" Then 'This is fine and correct, 
              Client_Aircraft_Data = Aircraft_Data
            Else
              'NO WE HAVE TO POLL THE DATABASE FOR THIS AC INFO AGAIN.
              aTempTable = masterPage.aclsData_Temp.Get_Clients_Aircraft(client_id)
              Client_Aircraft_Data = clsGeneral.clsGeneral.Create_Aircraft_Class(aTempTable, "cliaircraft")
              aTempTable.Dispose()
            End If
          End If

          If client_id <> 0 Then
            aTempTable2 = masterPage.aclsData_Temp.Get_Client_Aircraft_Details(client_id)
            apu_label_client.Text = clsGeneral.clsGeneral.Build_CLIENT_Equipment_Table_Tabs(client_id, "CLIENT", Client_Aircraft_Data, Nothing, masterPage, "apu", aTempTable2)
            usage_label_client.Text = clsGeneral.clsGeneral.Build_CLIENT_Equipment_Table_Tabs(client_id, "CLIENT", Client_Aircraft_Data, Nothing, masterPage, "usage", aTempTable2)
            interior_label_client.Text = clsGeneral.clsGeneral.Build_CLIENT_Equipment_Table_Tabs(client_id, "CLIENT", Client_Aircraft_Data, Nothing, masterPage, "interior", aTempTable2)
            exterior_label_client.Text = clsGeneral.clsGeneral.Build_CLIENT_Equipment_Table_Tabs(client_id, "CLIENT", Client_Aircraft_Data, Nothing, masterPage, "exterior", aTempTable2)
            maitenance_label_client.Text = clsGeneral.clsGeneral.Build_CLIENT_Equipment_Table_Tabs(client_id, "CLIENT", Client_Aircraft_Data, Nothing, masterPage, "maintenance", aTempTable2)

            ac_maint_right.Text = make_aircraft_maintenance(client_id, jetnet_id)

            cockpit_label_client.Text = clsGeneral.clsGeneral.Build_CLIENT_Equipment_Table_Tabs(client_id, "CLIENT", Client_Aircraft_Data, Nothing, masterPage, "cockpit", aTempTable2)
            equipment_label_client.Text = clsGeneral.clsGeneral.Build_CLIENT_Equipment_Table_Tabs(client_id, "CLIENT", Client_Aircraft_Data, Nothing, masterPage, "equipment", aTempTable2)
          End If

          If jetnet_id <> 0 Then

            aTempTable2 = masterPage.aclsData_Temp.GetJETNET_Aircraft_Details_Equipment_AC_ID(jetnet_id)

            ac_maint_left.Text = make_aircraft_maintenance(0, jetnet_id)


            apu_label.Text = clsGeneral.clsGeneral.Build_JETNET_Equipment_Table_Tabs(jetnet_id, "JETNET", Jetnet_Aircraft_Data, Nothing, masterPage, "apu", aTempTable2)
            usage_label.Text = clsGeneral.clsGeneral.Build_JETNET_Equipment_Table_Tabs(jetnet_id, "JETNET", Jetnet_Aircraft_Data, Nothing, masterPage, "usage", aTempTable2)
            interior_label.Text = clsGeneral.clsGeneral.Build_JETNET_Equipment_Table_Tabs(jetnet_id, "JETNET", Jetnet_Aircraft_Data, Nothing, masterPage, "interior", aTempTable2)
            exterior_label.Text = clsGeneral.clsGeneral.Build_JETNET_Equipment_Table_Tabs(jetnet_id, "JETNET", Jetnet_Aircraft_Data, Nothing, masterPage, "exterior", aTempTable2)
            maitenance_label.Text = clsGeneral.clsGeneral.Build_JETNET_Equipment_Table_Tabs(jetnet_id, "JETNET", Jetnet_Aircraft_Data, Nothing, masterPage, "maintenance", aTempTable2)
            cockpit_label.Text = clsGeneral.clsGeneral.Build_JETNET_Equipment_Table_Tabs(jetnet_id, "JETNET", Jetnet_Aircraft_Data, Nothing, masterPage, "cockpit", aTempTable2)
            equipment_label.Text = clsGeneral.clsGeneral.Build_JETNET_Equipment_Table_Tabs(jetnet_id, "JETNET", Jetnet_Aircraft_Data, Nothing, masterPage, "equipment", aTempTable2)
          End If
          other_tab_time.Text = Now()


        End If
    End Select



  End Sub
  Function make_aircraft_maintenance(ByVal Client_AircraftID As Long, ByVal jetnet_id As Long) As String
    make_aircraft_maintenance = ""

    Dim MaintenanceTable As New DataTable
    Dim temp_text As String = ""
    Dim aclsData_Manager_SQL As New clsData_Manager_SQL

    Try

      If jetnet_id = 0 Then
        jetnet_id = localDatalayer.Get_JETNET_AC_ID_FROM_CLIENT(Client_AircraftID, False)

        MaintenanceTable = clsGeneral.clsGeneral.Get_Maintenance_By_ID_Client(Client_AircraftID)
      Else
        aclsData_Manager_SQL.JETNET_DB = HttpContext.Current.Session.Item("jetnetClientDatabase")
        MaintenanceTable = aclsData_Manager_SQL.Get_Maintenance_By_ID(jetnet_id)
      End If


      'Sql = " cliacmaint_date_type as acmaint_date_type"

      temp_text = temp_text & "<table width='100%' cellpadding='3' cellspacing='0'>"
      temp_text = temp_text & "<tr><td align='left' valign='top'>"
      If Client_AircraftID > 0 Then
        temp_text = temp_text & "<a href='JavaScript:void();' onclick='return false;'><img src='images/client_info.jpg' class='float_right'  alt='EDIT CLIENT INFORMATION' border='0' onClick=""javascript:load('maintenance.aspx?acid=" & jetnet_id & "&cliacid=" & Client_AircraftID & "','','scrollbars=yes,menubar=no,height=500,width=1200,resizable=yes,toolbar=no,location=no,status=no');""/></a><br clear='all' />"
      Else
        temp_text = temp_text & "<img src='images/jetnet_info.jpg' alt='JETNET INFORMATION' /><br clear='all'/>"
      End If


     
      temp_text = temp_text & "</td></tr>"
      temp_text = temp_text & "<tr><td align='left' valign='top'>"

      If Not IsNothing(MaintenanceTable) Then
        If MaintenanceTable.Rows.Count > 0 Then
          For Each r As DataRow In MaintenanceTable.Rows

            If Client_AircraftID > 0 Then
              temp_text = temp_text & "<ul class='display_tab_client'>"
            Else
              temp_text = temp_text & "<ul class='display_tab'>"
            End If 

            temp_text = temp_text & "<li><b>" & r("acmaint_name") & "</b> - "


            If Not IsDBNull(r("acmaint_complied_date")) Then
              temp_text = temp_text & " CW Date: " & r("acmaint_complied_date")
            End If

            If Not IsDBNull(r("acmaint_complied_hrs")) Then
              If CInt(r("acmaint_complied_hrs")) > 0 Then
                temp_text = temp_text & " CW Hours: " & r("acmaint_complied_hrs")
              End If
            End If

            If Not IsDBNull(r("acmaint_due_date")) Then
              temp_text = temp_text & " Due Date: " & r("acmaint_due_date")
            End If

            If Not IsDBNull(r("acmaint_due_hrs")) Then
              If CInt(r("acmaint_due_hrs")) > 0 Then
                temp_text = temp_text & " Due Hours: " & r("acmaint_due_hrs")
              End If
            End If

            If Not IsDBNull(r("acmaint_notes")) Then
              If Trim(r("acmaint_notes")) <> "" Then
                temp_text = temp_text & " - " & r("acmaint_notes")
              End If
            End If


            temp_text = temp_text & "</li>"
            temp_text = temp_text & "</ul>"

          Next
        End If
      End If

      temp_text = temp_text & "</td></tr>"
      temp_text = temp_text & "</table>"

      make_aircraft_maintenance = temp_text

    Catch ex As Exception

    End Try
  End Function

  Sub Build_Propeller_Tab(ByVal jetnet_id As Integer, ByVal client_id As Integer, ByVal listingID As Integer, ByVal source As String, ByVal masterPage As crmWebClient.main_site)
    Dim propeller_text As String = ""
    Dim propeller_text_client As String = ""
    If jetnet_id <> 0 Then
      Try
        '---------------------------Propeller Information--------------------------------------------------------------
        ' get the propeller info
        propeller_text = propeller_text & "<table width='100%' cellpadding='3' cellspacing='0' class='engine'>"
        aTempTable2 = masterPage.aclsData_Temp.GetJETNET_Aircraft_Propeller(jetnet_id)
        If Not IsNothing(aTempTable2) Then
          If aTempTable2.Rows.Count > 0 Then

            For Each r As DataRow In aTempTable2.Rows
              propeller_text = propeller_text & "<tr><td align='left' valign='top'>&nbsp;</td>"
              propeller_text = propeller_text & "<td class='dark_blue' align='left' valign='top'><b>Serial #</b></td>"
              propeller_text = propeller_text & "<td class='dark_blue' align='left' valign='top'><b>TTSNEW Hrs</b> <span class='tiny'>(Total Time Since New)</span></td>"
              propeller_text = propeller_text & "<td class='dark_blue' align='left' valign='top'><b>SOH/SCOR Hrs</b> <span class='tiny'>(Since Overhaul)</span></td>"
              propeller_text = propeller_text & "<td class='dark_blue' align='left' valign='top'><b>Propeller Overhaul</b> <span class='tiny'>(MM/YYYY)</span></td></tr>"

              propeller_text = propeller_text & "<tr class='alt_row'><td align='left' valign='top'><b>Prop1:</b></td>"
              propeller_text = propeller_text & "<td align='left' valign='top'>" & r("ac_prop_1_ser_no") & "</td>"
              propeller_text = propeller_text & "<td align='left' valign='top'>" & r("ac_prop_1_snew_hrs") & "</td>"
              propeller_text = propeller_text & "<td align='left' valign='top'>" & r("ac_prop_1_soh_hrs") & "</td>"
              propeller_text = propeller_text & "<td align='left' valign='top'>" & r("ac_prop_1_soh_moyear") & "</td></tr>"

              propeller_text = propeller_text & "<tr><td align='left' valign='top'><b>Prop2:</b></td>"
              propeller_text = propeller_text & "<td align='left' valign='top'>" & r("ac_prop_2_ser_no") & "</td>"
              propeller_text = propeller_text & "<td align='left' valign='top'>" & r("ac_prop_2_snew_hrs") & "</td>"
              propeller_text = propeller_text & "<td align='left' valign='top'>" & r("ac_prop_2_soh_hrs") & "</td>"
              propeller_text = propeller_text & "<td align='left' valign='top'>" & r("ac_prop_2_soh_moyear") & "</td></tr>"

              propeller_text = propeller_text & "<tr class='alt_row'><td align='left' valign='top'><b>Prop3:</b></td>"
              propeller_text = propeller_text & "<td align='left' valign='top'>" & r("ac_prop_3_ser_no") & "</td>"
              propeller_text = propeller_text & "<td align='left' valign='top'>" & r("ac_prop_3_snew_hrs") & "</td>"
              propeller_text = propeller_text & "<td align='left' valign='top'>" & r("ac_prop_3_soh_hrs") & "</td>"
              propeller_text = propeller_text & "<td align='left' valign='top'>" & r("ac_prop_3_soh_moyear") & "</td></tr>"

            Next
            ' dump the datatable
            aTempTable2.Dispose()
            aTempTable2 = Nothing
          Else ' 0 rows
          End If
        Else
          If masterPage.aclsData_Temp.class_error <> "" Then
            error_string = masterPage.aclsData_Temp.class_error
            masterPage.LogError("details.aspx.vb - fill_AC_text() - " & error_string)
          End If
          masterPage.display_error()
        End If
        propeller_text = propeller_text & "</table>"


      Catch ex As Exception
        error_string = "details.aspx.vb - fill_AC_text() Propeller Tab - " & ex.Message
        masterPage.LogError(error_string)
      End Try
    End If
    aTempTable2 = Nothing
    props_label.Text = propeller_text
    Dim props_label_notes As New Label
    propeller_text = ""
    'props_label_notes = CType(FindControlRecursive(Aircraft_Tabs1, "props_label_notes"), Label)
    If client_id <> 0 Then
      Try

        '---------------------------Propeller Information--------------------------------------------------------------
        ' get the propeller info

        If source = "CLIENT" Then
          propeller_text_client = "<a href='JavaScript:void();' onclick='return false;'><img src='images/client_info.jpg' class='float_right'  alt='EDIT CLIENT INFORMATION' border='0' onClick=""javascript:load('edit.aspx?action=edit&type=propeller&ac_ID=" & masterPage.ListingID & "&source=" & masterPage.ListingSource & "','','scrollbars=yes,menubar=no,height=500,width=1200,resizable=yes,toolbar=no,location=no,status=no');""/></a><br clear='all' />"
        Else
          propeller_text_client = propeller_text_client & "<img src='images/non_client_info.jpg' class='float_right' alt='CLIENT INFORMATION' border='0' /><br clear='all'/>"
        End If
        propeller_text_client = propeller_text_client & "<table width='100%' cellpadding='3' cellspacing='0' class='engine_client'>"

        aTempTable2 = masterPage.aclsData_Temp.Get_Client_Aircraft_Propeller(client_id)
        If Not IsNothing(aTempTable2) Then
          If aTempTable2.Rows.Count > 0 Then
            For Each r As DataRow In aTempTable2.Rows
              propeller_text_client = propeller_text_client & "<tr><td align='left' valign='top'>&nbsp;</td>"
              propeller_text_client = propeller_text_client & "<td class='dark_red' align='left' valign='top'><b>Serial #</b></td>"
              propeller_text_client = propeller_text_client & "<td class='dark_red' align='left' valign='top'><b>TTSNEW Hrs</b> <span class='tiny'>(Total Time Since New)</span></td>"
              propeller_text_client = propeller_text_client & "<td class='dark_red' align='left' valign='top'><b>SOH/SCOR Hrs</b> <span class='tiny'>(Since Overhaul)</span></td>"
              propeller_text_client = propeller_text_client & "<td class='dark_red' align='left' valign='top'><b>Propeller Overhaul</b> <span class='tiny'>(MM/YYYY)</span></td></tr>"

              propeller_text_client = propeller_text_client & "<tr class='alt_row_client'><td align='left' valign='top'><b>Prop1:</b></td>"
              propeller_text_client = propeller_text_client & "<td align='left' valign='top'>" & r("cliacpr_prop_1_ser_nbr") & "</td>"
              propeller_text_client = propeller_text_client & "<td align='left' valign='top'>" & r("cliacpr_prop_1_ttsn_hours") & "</td>"
              propeller_text_client = propeller_text_client & "<td align='left' valign='top'>" & r("cliacpr_prop_1_tsoh_hours") & "</td>"
              propeller_text_client = propeller_text_client & "<td align='left' valign='top'>" & r("cliacpr_prop_1_month_year_oh") & "</td></tr>"

              propeller_text_client = propeller_text_client & "<tr><td align='left' valign='top'><b>Prop2:</b></td>"
              propeller_text_client = propeller_text_client & "<td align='left' valign='top'>" & r("cliacpr_prop_2_ser_nbr") & "</td>"
              propeller_text_client = propeller_text_client & "<td align='left' valign='top'>" & r("cliacpr_prop_2_ttsn_hours") & "</td>"
              propeller_text_client = propeller_text_client & "<td align='left' valign='top'>" & r("cliacpr_prop_2_tsoh_hours") & "</td>"
              propeller_text_client = propeller_text_client & "<td align='left' valign='top'>" & r("cliacpr_prop_2_month_year_oh") & "</td></tr>"

              propeller_text_client = propeller_text_client & "<tr class='alt_row_client'><td align='left' valign='top'><b>Prop3:</b></td>"
              propeller_text_client = propeller_text_client & "<td align='left' valign='top'>" & r("cliacpr_prop_3_ser_nbr") & "</td>"
              propeller_text_client = propeller_text_client & "<td align='left' valign='top'>" & r("cliacpr_prop_3_ttsn_hours") & "</td>"
              propeller_text_client = propeller_text_client & "<td align='left' valign='top'>" & r("cliacpr_prop_3_tsoh_hours") & "</td>"
              propeller_text_client = propeller_text_client & "<td align='left' valign='top'>" & r("cliacpr_prop_3_month_year_oh") & "</td></tr>"

            Next
            ' dump the datatable
            aTempTable2.Dispose()
            aTempTable2 = Nothing
          Else ' 0 rows
          End If
        Else
          If masterPage.aclsData_Temp.class_error <> "" Then
            error_string = masterPage.aclsData_Temp.class_error
            masterPage.LogError("details.aspx.vb - fill_AC_text() - " & error_string)
          End If
          masterPage.display_error()
        End If
        propeller_text_client = propeller_text_client & "</table>"
        props_label.Text = propeller_text_client
      Catch ex As Exception
        error_string = "details.aspx.vb - fill_AC_text() Propeller Tab - " & ex.Message
        masterPage.LogError(error_string)
      End Try
    End If
    aTempTable2 = Nothing
    props_label.Text = propeller_text_client
  End Sub

  Sub FillValueTable(ByVal masterPage As main_site)
    'If Session.Item("jetnetWebSiteType") = eWebSiteTypes.TEST Or Session.Item("jetnetWebSiteType") = eWebSiteTypes.LOCAL Then
    Try

      Dim returnString As String = ""
      Dim Aircraft_History_String As String = ""
      Dim AircraftID As Long = 0
      Dim JetnetAC_Id As Long = 0
      Dim localDataLayer As New viewsDataLayer
      Dim google_map_array_list As String = ""
      Dim exists_data As Boolean = False
      Dim OpenCount As Integer = 0

      If masterPage.ListingSource = "CLIENT" Then
        AircraftID = masterPage.ListingID
        JetnetAC_Id = masterPage.OtherID
      ElseIf masterPage.ListingSource = "JETNET" Then
        AircraftID = masterPage.OtherID
        JetnetAC_Id = masterPage.ListingID
      End If

      value_tab.Visible = True

      'HttpContext.Current.Session.Item("jetnetServerNotesDatabase") = CApplication.Item("crmClientDatabase")
      localDataLayer.clientConnectStr = Session.Item("jetnetClientDatabase") 'CApplication.Item("crmJetnetDatabase")

      ' valuation_chart.Titles.Clear()
      'valuation_chart.Titles.Add("My Aircraft Value History")
      localDataLayer.views_analytics_graph_1(AircraftID, Me.valuation_chart, Aircraft_History_String, JetnetAC_Id, google_map_array_list, "O", 0, exists_data)
      ' valuation_chart.ImageType = DataVisualization.Charting.ChartImageType.Jpeg
      If exists_data = True Then
        ' valuation_chart.SaveImage(Server.MapPath("TempFiles") + "\AC_" & AircraftID & "_Visualization_Chart_MONTHS.jpg", DataVisualization.Charting.ChartImageFormat.Jpeg)
        DisplayFunctions.load_google_chart(value_tab, google_map_array_list, "", "Aircraft Value ($k)", "chart_div_value_history", 430, 227, "POINTS", 1, "", Me.Page, Me.bottom_tab_update_panel, False, False, True)

        ' aircraft_value_history_label.Text = "<img src='TempFiles/AC_" & AircraftID & "_Visualization_Chart_MONTHS.jpg' width='300' />"
        aircraft_value_history_label.Text = Aircraft_History_String 
      Else
        aircraft_value_list_label.Text = "No Value History Available"
      End If


      If AircraftID > 0 Then


        If Not IsNothing(ViewState("Value_Data")) Then
          Value_Data = DirectCast(ViewState("Value_Data"), DataTable)
        End If

        If Not IsNothing(Value_Data) Then
          If Value_Data.Rows.Count > 0 Then
            returnString = "<table width='100%' cellpadding='5' cellspacing='0' class='data_aircraft_grid'>"
            returnString += "<tr class='header_row'>"
            returnString += "<td align='left' valign='top'><b>Date</b></td>"
            returnString += "<td align='left' valign='top'><b>Status</b></td>"
            returnString += "<td align='left' valign='top'><b>Asking</b></td>"
            returnString += "<td align='left' valign='top'><b>Take</b></td>"
            returnString += "<td align='left' valign='top'><b>ECV</b></td>"
            returnString += "<td align='left' valign='top'><b>Customer</b></td>"
            returnString += "<td align='left' valign='top'><b>PDF</b></td>"
            returnString += "</tr>"

            For Each r As DataRow In Value_Data.Rows
              Dim CompanyLocation As String = ""
              Dim Asking As Double = 0
              Dim Take As Double = 0
              Dim ECV As Double = 0

              returnString += "<tr>"
              'Date row
              returnString += "<td align='left' valign='top'>"
              If Not IsDBNull(r("lnote_action_date")) Then
                If IsDate(r("lnote_action_date")) Then
                  returnString += "<a href='#' onclick=""javascript:load('view_template.aspx?ViewID=19&noteID=" & r("lnote_id") & "&noMaster=false','scrollbars=yes,menubar=no,height=700,width=1150,resizable=yes,toolbar=no,location=no,status=no');"">" & FormatDateTime(r("lnote_action_date"), DateFormat.ShortDate) & "</a>"
                End If
              End If
              returnString += "</td>"
              'Status row
              returnString += "<td align='left' valign='top'>"
              If Not IsDBNull(r("lnote_opportunity_status")) Then
                returnString += IIf(r("lnote_opportunity_status").ToString = "O", "Open/Current", "Complete")
                Select Case r("lnote_opportunity_status")
                  Case "O"
                    OpenCount += 1
                    If Not IsDBNull(r("cliaircraft_asking_price")) Then
                      Asking = r("cliaircraft_asking_price")
                    End If
                    If Not IsDBNull(r("cliaircraft_est_price")) Then
                      Take = r("cliaircraft_est_price")
                    End If
                    If Not IsDBNull(r("cliaircraft_broker_price")) Then
                      ECV = r("cliaircraft_broker_price")
                    End If
                  Case Else

                    If Not IsDBNull(r("clival_asking_price")) Then
                      Asking = r("clival_asking_price")
                    End If
                    If Not IsDBNull(r("clival_est_price")) Then
                      Take = r("clival_est_price")
                    End If
                    If Not IsDBNull(r("clival_broker_price")) Then
                      ECV = r("clival_broker_price")
                    End If
                End Select
              End If
              returnString += "</td>"


              'Asking Row
              returnString += "<td align='left' valign='top'>"
              If Asking > 0 Then
                returnString += clsGeneral.clsGeneral.no_zero(Asking, "", True)
              End If
              returnString += "</td>"
              'Take Row
              returnString += "<td align='left' valign='top'>"
              If Take > 0 Then
                returnString += clsGeneral.clsGeneral.no_zero(Take, "", True)
              End If
              returnString += "</td>"
              'ECV row
              returnString += "<td align='left' valign='top'>"
              If ECV > 0 Then
                returnString += clsGeneral.clsGeneral.no_zero(ECV, "", True)
              End If
              returnString += "</td>"
              'Customer Row
              returnString += "<td align='left' valign='top'>"

              returnString += "<b><a href='details.aspx?comp_ID=" & r("clicomp_id") & "&source=CLIENT&type=1'>"


              If Not IsDBNull(r("clicomp_name")) Then
                returnString += r("clicomp_name")
              End If
              returnString += "</a></b><br />"

              If Not IsDBNull(r("clicomp_city")) Then
                CompanyLocation += r("clicomp_city")
              End If

              If Not IsDBNull(r("clicomp_state")) Then
                If CompanyLocation <> "" Then
                  CompanyLocation += ", "
                End If
                CompanyLocation += r("clicomp_state")
              End If

              If Not IsDBNull(r("clicomp_country")) Then
                If CompanyLocation <> "" Then
                  CompanyLocation += "<br />"
                End If
                CompanyLocation += r("clicomp_country")
              End If

              returnString += CompanyLocation

              returnString += "</td>"

              returnString += "<td align='left' valign='top'>"

              If Not IsDBNull(r("lnote_opportunity_status")) Then
                If r("lnote_opportunity_status") = "C" Then
                  Dim DocumentFile As String = ""

                  If Session.Item("jetnetWebSiteType") = eWebSiteTypes.LOCAL Then
                    DocumentFile = Server.MapPath("\Documents\") & r("lnote_id") & "_COMPARISON_VIEW_PDF.pdf"
                  Else
                    DocumentFile = "C:\inetpub\vhosts\jetnetcrm.com\private\documents\" & Replace(LCase(Application.Item("crmClientSiteData").crmClientHostName()), "www.", "") & "\" & r("lnote_id") & "_COMPARISON_VIEW_PDF.pdf"
                  End If

                  If System.IO.File.Exists(DocumentFile) Then
                    returnString += "<a href='edit_note.aspx?type=document_display&file=" & r("lnote_id") & "_COMPARISON_VIEW_PDF.pdf" & "&id=" & r("lnote_id") & "'' target='blank'><img src='images/pdf.jpg' alt='Click to view PDF' border='0' /></a>"
                  End If
                End If
              End If

              returnString += "</td>"
              returnString += "</tr>"
            Next
            returnString += "</table>"
          End If
          aircraft_value_list_label.Text = returnString
        End If
      End If
      aircraft_value_time.Text = Now()

      If OpenCount > 1 Then
        aircraftValueMessage.CssClass = "display_block"
      End If

    Catch ex As Exception
      error_string = "details.aspx.vb -  Aircraft Tabs (Fill Value Table) - " & ex.Message
      masterPage.LogError(error_string)
    End Try
    'End If
  End Sub


  Private Sub tabs_container_ActiveTabChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tabs_container.ActiveTabChanged
    Dim masterPage As main_site = DirectCast(Page.Master, main_site)
    masterPage.CheckVisibilityForJetnetClient()

    Changed_Tab(masterPage.ShowJetnetClient)
  End Sub



  Private Sub changeProspect_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles changeProspectDropdown.SelectedIndexChanged
    Dim masterPage As main_site = DirectCast(Page.Master, main_site)
    Dim TempTable As New DataTable
    TempTable = masterPage.aclsData_Temp.ChangeProspectNotesByParameters(0, 0, IIf(masterPage.ListingSource = "CLIENT", masterPage.ListingID, masterPage.OtherID), IIf(masterPage.ListingSource = "JETNET", masterPage.ListingID, masterPage.OtherID), IIf(masterPage.ListingSource = "CLIENT", Aircraft_Data.cliaircraft_cliamod_id, 0), IIf(masterPage.ListingSource = "JETNET", Aircraft_Data.cliaircraft_cliamod_id, 0), IIf(changeProspectDropdown.SelectedValue = 1, True, False), IIf(changeProspectDropdown.SelectedValue = 2, True, False), IIf(changeProspectDropdown.SelectedValue = 3, True, False))

    'Fill Prospects
    RaiseEvent Notes("", "PROSPECT", masterPage.ListingID, 0, False, prospect_label, TempTable)

  End Sub
End Class