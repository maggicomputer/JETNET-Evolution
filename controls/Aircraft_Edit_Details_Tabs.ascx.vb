Imports System.IO
Partial Public Class Aircraft_Edit_Details_Tabs
  Inherits System.Web.UI.UserControl
  Dim aclsData_Temp As New clsData_Manager_SQL
  Dim aTempTable, aTempTable2 As New DataTable 'Data Tables used
  Dim typed As String = ""
  Dim error_string As String = ""
  Dim AircraftID As Long = 0
#Region "Page Events"
  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Me.Visible Then

      If Not IsNothing(Request.Item("ac_ID")) Then
        If Not String.IsNullOrEmpty(Request.Item("ac_ID").ToString) Then
          AircraftID = CLng(Request.Item("ac_ID").ToString.Trim)
        End If
      End If

      If AircraftID = 0 Then
        AircraftID = Session.Item("ListingID")
      End If

      aclsData_Temp = New clsData_Manager_SQL
      aclsData_Temp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase") 'CApplication.Item("crmClientDatabase")
      aclsData_Temp.JETNET_DB = Session.Item("jetnetClientDatabase") 'CApplication.Item("crmJetnetDatabase")
      Try
        datagrid_details.Visible = True
        'If Not Page.IsPostBack Then
        '  typeDropdownPick.Items.Clear()
        'End If

        Select Case Trim(Request("typeofdetails"))
          Case "interior"
            typed = "Interior"
            interior.Visible = True
            Session.Item("ac_active_tab") = "7"

          Case "exterior"
            typed = "Exterior"
            exterior.Visible = True
            Session.Item("ac_active_tab") = "7"
          Case "cockpit"
            typed = "Addl Cockpit Equipment"
            done_with_Changes.Visible = True
            Session.Item("ac_active_tab") = "8"
          Case "equip"
            typed = "Equipment"
            done_with_Changes.Visible = True
            Session.Item("ac_active_tab") = "6"
            additional_details_edit.Visible = True
          Case "main"
            typed = "Maintenance"
            main.Visible = True
            Session.Item("ac_active_tab") = "5"
          Case Else
            Select Case Trim(Request("type"))
              Case "apu"
                APU.Visible = True
                datagrid_details.Visible = False
                add_new.Visible = False
                Session.Item("ac_active_tab") = "10"
              Case "usage"
                usage.Visible = True
                datagrid_details.Visible = False
                add_new.Visible = False
                Session.Item("ac_active_tab") = "5"
            End Select
        End Select
        If Not Page.IsPostBack Then

          If typed = "Equipment" Then
            typeDropdownPick.Items.Add(New ListItem("Equipment", "Equipment"))
            typeDropdownPick.Items.Add(New ListItem("Addl Cockpit Equipment", "Addl Cockpit Equipment"))
          Else
            typeDropdownPick.Items.Add(New ListItem(typed, typed))
          End If

          Dim atemptable3 As New DataTable
          atemptable3 = aclsData_Temp.lookupAirframeEngine_Mait(1, 0, 0, "Airframe", True)
          If Not IsNothing(atemptable3) Then
            If atemptable3.Rows.Count > 0 Then
              For Each r As DataRow In atemptable3.Rows
                If Not IsDBNull(r("amp_program_name")) And Not IsDBNull(r("amp_provider_name")) Then
                  If UCase(r("amp_program_name").ToString) = "UNKNOWN" Or UCase(r("amp_provider_name").ToString) = "UKNOWN" Then
                    airframe_maintenance_program.Items.Add(New ListItem(r("amp_program_name"), r("amp_id")))
                  Else
                    airframe_maintenance_program.Items.Add(New ListItem(r("amp_program_name") & " " & r("amp_provider_name"), r("amp_id")))
                  End If

                End If
              Next
            End If
          End If
        End If

          If Not Page.IsPostBack Then
            Dim atemptable3 As New DataTable
            atemptable3 = aclsData_Temp.lookupAirframeEngine_Mait(0, 1, 0, "Airframe", True)
            If Not IsNothing(atemptable3) Then
              If atemptable3.Rows.Count > 0 Then
                For Each r As DataRow In atemptable3.Rows
                  If Not IsDBNull(r("amtp_program_name")) And Not IsDBNull(r("amtp_provider_name")) Then
                    If UCase(r("amtp_program_name").ToString) = "UNKNOWN" Or UCase(r("amtp_provider_name").ToString) = "UKNOWN" Then
                      airframe_tracking_program.Items.Add(New ListItem(r("amtp_program_name"), r("amtp_id")))
                    Else
                      airframe_tracking_program.Items.Add(New ListItem(r("amtp_program_name") & " " & r("amtp_provider_name"), r("amtp_id")))
                    End If

                  End If

                Next
              End If
          End If

          'FIll up APU 
          atemptable3 = aclsData_Temp.APUMaintenanceName("")
          If Not IsNothing(atemptable3) Then
            If atemptable3.Rows.Count > 0 Then
              For Each r As DataRow In atemptable3.Rows
                If Not IsDBNull(r("emp_code")) And Not IsDBNull(r("emp_name")) Then
                  ac_apu_main_dropdown.Items.Add(New ListItem(r("emp_name"), r("emp_code")))
                End If
              Next
            End If
          End If


          End If

          If Not Page.IsPostBack Then
            If typed <> "" Then
              bind_data()
              aTempTable = aclsData_Temp.Get_Client_Aircraft_Data_Type(typed)
              If Not IsNothing(aTempTable) Then
                For Each r As DataRow In aTempTable.Rows
                  cliadet_data_name.Items.Add(New ListItem(r("cliadt_data_name"), r("cliadt_data_name")))
                Next
              Else
                If aclsData_Temp.class_error <> "" Then
                  error_string = "Aircraft_Edit_Details_Tabs.ascx.vb - Page_Load() - " & aclsData_Temp.class_error
                  clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
                End If
              End If
            End If

            aTempTable2 = aclsData_Temp.Get_Client_Aircraft_Engine(AircraftID)
            If Not IsNothing(aTempTable2) Then
              If aTempTable2.Rows.Count > 0 Then
                For Each q As DataRow In aTempTable2.Rows

                  If Not IsDBNull(q("cliacep_engine_overhaul_done_by_name")) Then
                    ac_maint_eoh_by_name.Text = q("cliacep_engine_overhaul_done_by_name")
                  End If

                  If Not IsDBNull(q("cliacep_engine_hot_inspection_done_by_name")) Then
                    ac_maint_hots_by_name.Text = q("cliacep_engine_hot_inspection_done_by_name")
                  End If
                Next
              End If
            Else
              If aclsData_Temp.class_error <> "" Then
                error_string = "Aircraft_Edit_Details_Tabs.ascx.vb - Page_Load() - " & aclsData_Temp.class_error
                clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
              End If
            End If

            aTempTable = aclsData_Temp.Get_Clients_Aircraft(AircraftID)
            ' check the state of the DataTable
            If Not IsNothing(aTempTable) Then
              If aTempTable.Rows.Count > 0 Then
                For Each R As DataRow In aTempTable.Rows
                  title_change.Text = CommonAircraftFunctions.CreateHeaderLine(R("cliamod_make_name"), R("cliamod_model_name"), R("cliaircraft_ser_nbr"), "")


                  'set up the first five interior
                  If Not IsDBNull(R("cliaircraft_interior_rating")) Then
                    ac_interior_rating.Text = R("cliaircraft_interior_rating")
                  End If
                  If Not IsDBNull(R("cliaircraft_interior_doneby_name")) Then
                    ac_interior_doneby_name.Text = R("cliaircraft_interior_doneby_name")
                  End If
                  If Not IsDBNull(R("cliaircraft_interior_month_year")) Then
                    If Len(Trim(R("cliaircraft_interior_month_year"))) > 4 Then
                      ac_interior_month.Text = Left(Trim(R("cliaircraft_interior_month_year")), 2)
                    End If
                    ac_interior_year.Text = Right(Trim(R("cliaircraft_interior_month_year")), 4)
                  End If
                  If Not IsDBNull(R("cliaircraft_passenger_count")) Then
                    ac_passenger_count.Text = R("cliaircraft_passenger_count")
                  End If
                  If Not IsDBNull(R("cliaircraft_interior_config_name")) Then
                    ac_interior_config_name.Text = R("cliaircraft_interior_config_name")
                  End If
                  If Not IsDBNull(R("cliaircraft_airframe_maintenance_program")) Then
                    airframe_maintenance_program.SelectedValue = R("cliaircraft_airframe_maintenance_program")
                  End If

                  If Not IsDBNull(R("cliaircraft_ac_maintained")) Then
                    ac_maintained.Text = R("cliaircraft_ac_maintained")
                  End If

                  If Not IsDBNull(R("cliaircraft_airframe_maintenance_tracking_program")) Then
                    airframe_tracking_program.SelectedValue = R("cliaircraft_airframe_maintenance_tracking_program")
                  End If

                  'set up the first three exterior
                  If Not IsDBNull(R("cliaircraft_exterior_rating")) Then
                    ac_exterior_rating.Text = R("cliaircraft_exterior_rating")
                  End If
                  If Not IsDBNull(R("cliaircraft_exterior_doneby_name")) Then
                    ac_exterior_doneby_name.Text = R("cliaircraft_exterior_doneby_name")
                  End If

                  If Not IsDBNull(R("cliaircraft_exterior_month_year")) Then
                    If Len(Trim(R("cliaircraft_exterior_month_year"))) > 4 Then
                      ac_exterior_month.Text = Left(Trim(R("cliaircraft_exterior_month_year")), 2)
                    End If
                    ac_exterior_year.Text = Right(Trim(R("cliaircraft_exterior_month_year")), 4)
                  End If

                  ' setup the APU info
                  If Not IsDBNull(R("cliaircraft_apu_model_name")) Then
                    ac_apu_model_name.Text = R("cliaircraft_apu_model_name")
                  End If
                  If Not IsDBNull(R("cliaircraft_apu_ser_nbr")) Then
                    ac_apu_ser_nbr.Text = R("cliaircraft_apu_ser_nbr")
                  End If
                  If Not IsDBNull(R("cliaircraft_apu_maintance_program")) Then
                  ac_apu_main_dropdown.SelectedValue = R("cliaircraft_apu_maintance_program")
                End If

                  If Not IsDBNull(R("cliaircraft_apu_ttsn_hours")) Then
                    ac_apu_ttsn_hours.Text = R("cliaircraft_apu_ttsn_hours")
                  End If
                  If Not IsDBNull(R("cliaircraft_apu_tsoh_hours")) Then
                    ac_apu_tsoh_hours.Text = R("cliaircraft_apu_tsoh_hours")
                  End If
                  If Not IsDBNull(R("cliaircraft_apu_tshi_hours")) Then
                    ac_apu_tshi_hours.Text = R("cliaircraft_apu_tshi_hours")
                  End If
                  If Not IsDBNull(R("cliaircraft_damage_history_notes")) Then
                    damage_history.Text = R("cliaircraft_damage_history_notes")
                  End If
                  If Not IsDBNull(R("cliaircraft_date_engine_times_as_of")) Then
                    ac_date_engine_times_as_of.Text = R("cliaircraft_date_engine_times_as_of")
                  End If
                  If Not IsDBNull(R("cliaircraft_airframe_total_hours")) Then
                    ac_airframe_total_hours.Text = R("cliaircraft_airframe_total_hours")
                  End If
                  If Not IsDBNull(R("cliaircraft_airframe_total_landings")) Then
                    ac_airframe_total_landings.Text = R("cliaircraft_airframe_total_landings")
                  End If
                Next
              End If
            Else
              If aclsData_Temp.class_error <> "" Then
                error_string = "Aircraft_Edit_Details_Tabs.ascx.vb - Page_Load() - " & aclsData_Temp.class_error
                clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
              End If
            End If
          End If
      Catch ex As Exception
        error_string = "Aircraft_Edit_Details_Tabs.ascx.vb - Page Load() - " & ex.Message
        clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
      End Try
    End If
  End Sub
#End Region

#Region "Datagrid Events"
  Public Sub MyDataGrid_Update(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)
    Try
      Dim id As TextBox = e.Item.FindControl("id")
      Dim typed As TextBox = e.Item.FindControl("type")
      Dim name_new As DropDownList = e.Item.FindControl("name_type")
      Dim description_new As TextBox = e.Item.FindControl("description")
      Dim type_new As TextBox = e.Item.FindControl("type_hidden")
      Dim ac_hidden As TextBox = e.Item.FindControl("ac_hidden")
      Dim t As String = ""
      If aclsData_Temp.Update_Client_Aircraft_Details(id.Text, ac_hidden.Text, type_new.Text, name_new.SelectedValue, description_new.Text, Now()) = 1 Then
        datagrid_details.EditItemIndex = -1
        bind_data()
        updated.Text = "<p align=""center"">Your information has been edited.</p>"
      End If
    Catch ex As Exception
      error_string = "Aircraft_Edit_Details_Tabs.ascx.vb - MyDataGrid_Update() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
  Public Sub MyDataGrid_Delete(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)
    Try
      Dim id As TextBox = e.Item.FindControl("id")

      Dim ac_hidden As TextBox = e.Item.FindControl("ac_hidden")
      Dim t As String = ""
      If aclsData_Temp.Delete_Client_Aircraft_Details(id.Text, ac_hidden.Text) = 1 Then
        bind_data()
        updated.Text = "<p align=""center"">Your information has been removed.</p>"
      End If
    Catch ex As Exception
      error_string = "Aircraft_Edit_Details_Tabs.ascx.vb - MyDataGrid_Delete() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
  Public Sub MyDataGrid_Cancel(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)
    Try
      bind_data()
      datagrid_details.EditItemIndex = -1
      updated.Text = ""
    Catch ex As Exception
      error_string = "Aircraft_Edit_Details_Tabs.ascx.vb - MyDataGrid_Cancel() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
  Sub MyDataGrid_Edit(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
    Try
      datagrid_details.EditItemIndex = CInt(E.Item.ItemIndex)
      bind_data()
      datagrid_details.DataBind()
      updated.Text = ""
    Catch ex As Exception
      error_string = "Aircraft_Edit_Details_Tabs.ascx.vb - MyDataGrid_Edit() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub

  Public Sub MyDataGrid_CancelAdd(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)
    Try
      bind_data()
      additional.EditItemIndex = -1
      updated.Text = ""
    Catch ex As Exception
      error_string = "Aircraft_Edit_Details_Tabs.ascx.vb - MyDataGrid_Cancel() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
  Sub MyDataGrid_EditAdd(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
    Try
      additional.EditItemIndex = CInt(E.Item.ItemIndex)
      bind_data()
      additional.DataBind()
      updated.Text = ""
    Catch ex As Exception
      error_string = "Aircraft_Edit_Details_Tabs.ascx.vb - MyDataGrid_Edit() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
  Public Sub bind_data()
    Dim displayData As String = typed

    Dim additionalData As New DataTable
    If displayData = "Equipment" Then
      additionalData = aclsData_Temp.Get_Client_Aircraft_Details(AircraftID, "Addl Cockpit Equipment")
      If Not IsNothing(additionalData) Then
        If additionalData.Rows.Count > 0 Then
          equipHeader.Visible = True
          topPage.CssClass = "display_none"
          addHeader.Visible = True
          additional.DataSource = additionalData
          additional.DataBind()
        End If
      Else
        If aclsData_Temp.class_error <> "" Then
          error_string = "Aircraft_Edit_Details_Tabs.ascx.vb - bind_data() 1 - " & aclsData_Temp.class_error
          clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
        End If
      End If
    End If


    aTempTable = aclsData_Temp.Get_Client_Aircraft_Details(AircraftID, displayData)


    If Not IsNothing(aTempTable) Then
      If aTempTable.Rows.Count > 0 Then
        datagrid_details.DataSource = aTempTable
        datagrid_details.DataBind()
      End If
    Else
      If aclsData_Temp.class_error <> "" Then
        error_string = "Aircraft_Edit_Details_Tabs.ascx.vb - bind_data() 2 - " & aclsData_Temp.class_error
        clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
      End If
    End If
  End Sub
  Private Sub datagrid_avionics_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles datagrid_details.ItemDataBound, additional.ItemDataBound
    Try
      Dim sel As TextBox = e.Item.FindControl("name_hidden")

      If Not IsNothing(e.Item.FindControl("name_type")) Then
        Dim ddl As DropDownList = e.Item.FindControl("name_type")
        aTempTable = aclsData_Temp.Get_Client_Aircraft_Data_Type(typed)

        For Each r As DataRow In aTempTable.Rows
          ddl.Items.Add(New ListItem(r("cliadt_data_name"), r("cliadt_data_name")))
        Next

        ddl.SelectedValue = sel.Text
      End If
      If Trim(Request("typeofdetails")) = "equip" Then
        If Not IsNothing(e.Item.FindControl("description")) Then
          Dim description_new As TextBox = e.Item.FindControl("description")
          description_new.TextMode = TextBoxMode.MultiLine
          description_new.Rows = 7
          description_new.Width = 200
        End If
      End If

    Catch ex As Exception
      error_string = "Aircraft_Edit_Details_Tabs.ascx.vb - datagrid_avionics_ItemDataBound() - " & ex.Message
      '  LogError(error_string)
    End Try

  End Sub
#End Region
#Region "Add New Row/Insert Click Events"
  Private Sub add_new_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles add_new.Click
    Try
      new_row.Visible = True
      add_new.Visible = False
      updated.Text = ""
    Catch ex As Exception
      error_string = "Aircraft_Edit_Details_Tabs.ascx.vb - add_new_Click() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
  Private Sub insert_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles insert.Click
    Try
      If aclsData_Temp.Insert_Client_Aircraft_Details(AircraftID, IIf(typeDropdownPick.SelectedValue = "", typed, typeDropdownPick.SelectedValue), cliadet_data_name.Text, cliadet_data_description.Text, Now()) = 1 Then
        bind_data()
        new_row.Visible = False
        add_new.Visible = True
        updated.Text = "<p align=""center"">Your information has been added.</p>"
      End If
    Catch ex As Exception
      error_string = "Aircraft_Edit_Details_Tabs.ascx.vb - add_new_Click() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
#End Region
#Region "Save Additional Information Events"
  Private Sub Save_It(ByVal id As Integer)
    Try
      Dim aclsUpdate_Client_Aircraft As New clsClient_Aircraft

      aclsUpdate_Client_Aircraft.cliaircraft_id = id

      aTempTable = aclsData_Temp.Get_Clients_Aircraft(id)
      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then
          For Each R As DataRow In aTempTable.Rows

            If Not IsDBNull(R("cliaircraft_action_date")) Then
              aclsUpdate_Client_Aircraft.cliaircraft_action_date = R("cliaircraft_action_date")
            End If
            If Not IsDBNull(R("cliaircraft_asking_price")) Then
              aclsUpdate_Client_Aircraft.cliaircraft_asking_price = R("cliaircraft_asking_price")
            End If
            If Not IsDBNull(R("cliaircraft_ser_nbr")) Then
              aclsUpdate_Client_Aircraft.cliaircraft_ser_nbr = R("cliaircraft_ser_nbr")
            End If
            If Not IsDBNull(R("cliaircraft_asking_wordage")) Then
              aclsUpdate_Client_Aircraft.cliaircraft_asking_wordage = R("cliaircraft_asking_wordage")
            End If
            If Not IsDBNull(R("cliaircraft_cliamod_id")) Then
              aclsUpdate_Client_Aircraft.cliaircraft_cliamod_id = R("cliaircraft_cliamod_id")
            End If
            If Not IsDBNull(R("cliaircraft_delivery")) Then
              aclsUpdate_Client_Aircraft.cliaircraft_delivery = R("cliaircraft_delivery")
            End If
            If Not IsDBNull(R("cliaircraft_exclusive_flag")) Then
              aclsUpdate_Client_Aircraft.cliaircraft_exclusive_flag = R("cliaircraft_exclusive_flag")
            End If
            If Not IsDBNull(R("cliaircraft_forsale_flag")) Then
              aclsUpdate_Client_Aircraft.cliaircraft_forsale_flag = R("cliaircraft_forsale_flag")
            End If
            If Not IsDBNull(R("cliaircraft_lease_flag")) Then
              aclsUpdate_Client_Aircraft.cliaircraft_lease_flag = R("cliaircraft_lease_flag")
            End If
            If Not IsDBNull(R("cliaircraft_reg_nbr")) Then
              aclsUpdate_Client_Aircraft.cliaircraft_reg_nbr = R("cliaircraft_reg_nbr")
            End If
            If Not IsDBNull(R("cliaircraft_status")) Then
              aclsUpdate_Client_Aircraft.cliaircraft_status = R("cliaircraft_status")
            End If

            aclsUpdate_Client_Aircraft.cliaircraft_user_id = Session.Item("localUser").crmLocalUserID

            If Not IsDBNull(R("cliaircraft_year_mfr")) Then
              aclsUpdate_Client_Aircraft.cliaircraft_year_mfr = R("cliaircraft_year_mfr")
            End If
            If Not IsDBNull(R("cliaircraft_jetnet_ac_id")) Then
              aclsUpdate_Client_Aircraft.cliaircraft_jetnet_ac_id = R("cliaircraft_jetnet_ac_id")
            End If

            aclsUpdate_Client_Aircraft.cliaircraft_action_date = Now()
            If Not IsDBNull(R("cliaircraft_lifecycle")) Then
              aclsUpdate_Client_Aircraft.cliaircraft_lifecycle = R("cliaircraft_lifecycle")
            End If
            If Not IsDBNull(R("cliaircraft_ownership")) Then
              aclsUpdate_Client_Aircraft.cliaircraft_ownership = R("cliaircraft_ownership")
            End If
            If Not IsDBNull(R("cliaircraft_usage")) Then
              aclsUpdate_Client_Aircraft.cliaircraft_usage = R("cliaircraft_usage")
            End If

            If Not IsDBNull(R("cliaircraft_aport_iata_code")) Then
              aclsUpdate_Client_Aircraft.cliaircraft_aport_iata_code = R("cliaircraft_aport_iata_code")
            End If
            If Not IsDBNull(R("cliaircraft_aport_icao_code")) Then
              aclsUpdate_Client_Aircraft.cliaircraft_aport_icao_code = R("cliaircraft_aport_icao_code")
            End If
            If Not IsDBNull(R("cliaircraft_aport_name")) Then
              aclsUpdate_Client_Aircraft.cliaircraft_aport_name = R("cliaircraft_aport_name")
            End If
            If Not IsDBNull(R("cliaircraft_aport_state")) Then
              aclsUpdate_Client_Aircraft.cliaircraft_aport_state = R("cliaircraft_aport_state")
            End If
            If Not IsDBNull(R("cliaircraft_aport_country")) Then
              aclsUpdate_Client_Aircraft.cliaircraft_aport_country = R("cliaircraft_aport_country")
            End If
            If Not IsDBNull(R("cliaircraft_aport_city")) Then
              aclsUpdate_Client_Aircraft.cliaircraft_aport_city = R("cliaircraft_aport_city")
            End If
            If Not IsDBNull(R("cliaircraft_aport_private")) Then
              aclsUpdate_Client_Aircraft.cliaircraft_aport_private = R("cliaircraft_aport_private")
            End If

            If Not IsDBNull(ac_airframe_total_hours.Text) Then
              If ac_airframe_total_hours.Text <> "" Then
                aclsUpdate_Client_Aircraft.cliaircraft_airframe_total_hours = ac_airframe_total_hours.Text
              End If
            End If

            If Not IsDBNull(ac_maintained.Text) Then
              If ac_maintained.Text <> "" Then
                aclsUpdate_Client_Aircraft.cliaircraft_ac_maintained = ac_maintained.Text
              End If
            End If

            If Not IsDBNull(ac_airframe_total_landings.Text) Then
              If ac_airframe_total_landings.Text <> "" Then
                aclsUpdate_Client_Aircraft.cliaircraft_airframe_total_landings = ac_airframe_total_landings.Text
              End If
            End If
            If Not IsDBNull(ac_date_engine_times_as_of.Text) Then
              If ac_date_engine_times_as_of.Text <> "" Then
                aclsUpdate_Client_Aircraft.cliaircraft_date_engine_times_as_of = ac_date_engine_times_as_of.Text
              End If
            End If
            If Not IsDBNull(R("cliaircraft_date_purchased")) Then
              aclsUpdate_Client_Aircraft.cliaircraft_date_purchased = R("cliaircraft_date_purchased")
            End If
            If Not IsDBNull(R("cliaircraft_date_listed")) Then
              aclsUpdate_Client_Aircraft.cliaircraft_date_listed = R("cliaircraft_date_listed")
            End If
            If Not IsDBNull(ac_date_engine_times_as_of.Text) Then
              If ac_date_engine_times_as_of.Text <> "" Then
                aclsUpdate_Client_Aircraft.cliaircraft_date_engine_times_as_of = ac_date_engine_times_as_of.Text
              End If
            End If
            If Not IsDBNull(ac_apu_model_name.Text) Then
              aclsUpdate_Client_Aircraft.cliaircraft_apu_model_name = ac_apu_model_name.Text
            End If

            If Not IsDBNull(ac_apu_main_dropdown.SelectedValue) Then
              aclsUpdate_Client_Aircraft.cliaircraft_apu_maintance_program = ac_apu_main_dropdown.SelectedValue
            End If

            If Not IsDBNull(ac_apu_ser_nbr.Text) Then
              If ac_apu_ser_nbr.Text <> "" Then
                aclsUpdate_Client_Aircraft.cliaircraft_apu_ser_nbr = ac_apu_ser_nbr.Text
              End If
            End If
            If Not IsDBNull(ac_apu_ttsn_hours.Text) Then
              If ac_apu_ttsn_hours.Text <> "" Then
                aclsUpdate_Client_Aircraft.cliaircraft_apu_ttsn_hours = ac_apu_ttsn_hours.Text
              End If
            End If
            If Not IsDBNull(ac_apu_tsoh_hours.Text) Then
              If ac_apu_tsoh_hours.Text <> "" Then
                aclsUpdate_Client_Aircraft.cliaircraft_apu_tsoh_hours = ac_apu_tsoh_hours.Text
              End If
            End If
            If Not IsDBNull(ac_apu_tshi_hours.Text) Then
              If ac_apu_tshi_hours.Text <> "" Then
                aclsUpdate_Client_Aircraft.cliaircraft_apu_tshi_hours = ac_apu_tshi_hours.Text
              End If
            End If
            If Not IsDBNull(airframe_maintenance_program.SelectedValue) Then
              If airframe_maintenance_program.SelectedValue <> "" Then
                aclsUpdate_Client_Aircraft.cliaircraft_airframe_maintenance_program = airframe_maintenance_program.SelectedValue
              End If
            End If
            If Not IsDBNull(airframe_tracking_program.SelectedValue) Then
              If airframe_tracking_program.SelectedValue <> "" Then
                aclsUpdate_Client_Aircraft.cliaircraft_airframe_maintenance_tracking_program = airframe_tracking_program.SelectedValue
              End If
            End If
            If Not IsDBNull(R("cliaircraft_damage_flag")) Then
              aclsUpdate_Client_Aircraft.cliaircraft_damage_flag = R("cliaircraft_damage_flag")
            End If
            If Not IsDBNull(R("cliaircraft_damage_history_notes")) Then
              If damage_history.Text <> "" Then
                aclsUpdate_Client_Aircraft.cliaircraft_damage_history_notes = damage_history.Text
              End If
            End If
            If Not IsDBNull(ac_interior_rating.Text) Then
              If ac_interior_rating.Text <> "" Then
                aclsUpdate_Client_Aircraft.cliaircraft_interior_rating = ac_interior_rating.Text
              End If
            End If

            If Not IsDBNull(ac_interior_month.Text) Or Not IsDBNull(ac_interior_year.Text) Then
              If ac_interior_month.Text <> "" Or ac_interior_year.Text <> "" Then
                aclsUpdate_Client_Aircraft.cliaircraft_interior_month_year = ac_interior_month.Text & ac_interior_year.Text
              End If
            End If
            If Not IsDBNull(ac_interior_doneby_name.Text) Then
              If ac_interior_doneby_name.Text <> "" Then
                aclsUpdate_Client_Aircraft.cliaircraft_interior_doneby_name = ac_interior_doneby_name.Text
              End If
            End If
            If Not IsDBNull(ac_interior_config_name.Text) Then
              If ac_interior_config_name.Text <> "" Then
                aclsUpdate_Client_Aircraft.cliaircraft_interior_config_name = ac_interior_config_name.Text
              End If
            End If
            If Not IsDBNull(ac_exterior_rating.Text) Then
              If ac_exterior_rating.Text <> "" Then
                aclsUpdate_Client_Aircraft.cliaircraft_exterior_rating = ac_exterior_rating.Text
              End If
            End If
 
            If Not IsDBNull(ac_exterior_month.Text) Or Not IsDBNull(ac_exterior_year.Text) Then
              If ac_exterior_month.Text <> "" Or ac_exterior_year.Text <> "" Then
                aclsUpdate_Client_Aircraft.cliaircraft_exterior_month_year = ac_exterior_month.Text & ac_exterior_year.Text
              End If
            End If

            If Not IsDBNull(ac_exterior_doneby_name.Text) Then
              If ac_exterior_doneby_name.Text <> "" Then
                aclsUpdate_Client_Aircraft.cliaircraft_exterior_doneby_name = ac_exterior_doneby_name.Text
              End If
            End If

            If Not IsDBNull(ac_passenger_count.Text) Then
              If ac_passenger_count.Text <> "" Then
                aclsUpdate_Client_Aircraft.cliaircraft_passenger_count = ac_passenger_count.Text
              End If
            End If

          Next

        End If
      Else
        If aclsData_Temp.class_error <> "" Then
          error_string = "Aircraft_Edit_Details_Tabs.ascx.vb - Save_It() - " & aclsData_Temp.class_error
          clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
        End If
      End If
      aclsData_Temp.Update_Client_Aircraft(aclsUpdate_Client_Aircraft)

      fill_engine_details()
      updated.Text = "Your Additional Information has Been Updated"
    Catch ex As Exception
      error_string = "Aircraft_Edit_Details_Tabs.ascx.vb - Save_It() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
  Private Sub usage_save_Click() Handles usageSave.Click, interiorSave.Click, exteriorSave.Click, maintenanceSave.Click, apuSave.Click
    Try
      Save_It(AircraftID)
      Dim url As String = "details.aspx?type=3&source=&" & Session.Item("ListingSource") & "&ac_ID=" & AircraftID

      System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.location.href = window.opener.location.href;", True)
      'System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.location = '" & url & "';", True)
      System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)
    Catch ex As Exception
      error_string = "Aircraft_Edit_Details_Tabs.ascx.vb - usage_save_Click() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
  Private Sub fill_engine_details()
    Try
      Dim aclsUpdate_Client_Aircraft_Engine As New clsClient_Aircraft_Engine

      aTempTable2 = aclsData_Temp.Get_Client_Aircraft_Engine(AircraftID)
      If Not IsNothing(aTempTable2) Then
        If aTempTable2.Rows.Count > 0 Then
          For Each R As DataRow In aTempTable2.Rows

            aclsUpdate_Client_Aircraft_Engine.cliacep_cliac_id = AircraftID
            If Not IsDBNull(R("cliacep_engine_name")) Then
              aclsUpdate_Client_Aircraft_Engine.cliacep_engine_name = R("cliacep_engine_name")
            End If

            If Not IsDBNull(R("cliacep_engine_maintenance_program")) Then
              aclsUpdate_Client_Aircraft_Engine.cliacep_engine_maintenance_program = R("cliacep_engine_maintenance_program")
            End If
            If Not IsDBNull(R("cliacep_engine_management_program")) Then
              aclsUpdate_Client_Aircraft_Engine.cliacep_engine_management_program = R("cliacep_engine_management_program")
            End If

            If Not IsDBNull(R("cliacep_engine_tbo_oc_flag")) Then
              aclsUpdate_Client_Aircraft_Engine.cliacep_engine_tbo_oc_flag = R("cliacep_engine_tbo_oc_flag")
            End If
            If Not IsDBNull(R("cliacep_engine_noise_rating")) Then
              aclsUpdate_Client_Aircraft_Engine.cliacep_engine_noise_rating = R("cliacep_engine_noise_rating")
            End If

            If Not IsDBNull(R("cliacep_engine_model_config")) Then
              aclsUpdate_Client_Aircraft_Engine.cliacep_engine_model_config = R("cliacep_engine_model_config")
            End If
            If Not IsDBNull(ac_maint_hots_by_name.Text) Then
              aclsUpdate_Client_Aircraft_Engine.cliacep_engine_overhaul_done_by_name = ac_maint_eoh_by_name.Text
            End If
            If Not IsDBNull(R("cliacep_engine_overhaul_done_month_year")) Then
              aclsUpdate_Client_Aircraft_Engine.cliacep_engine_overhaul_done_month_year = R("cliacep_engine_overhaul_done_month_year")
            End If
            If Not IsDBNull(ac_maint_hots_by_name.Text) Then
              aclsUpdate_Client_Aircraft_Engine.cliacep_engine_hot_inspection_done_by_name = ac_maint_hots_by_name.Text
            End If
            If Not IsDBNull(R("cliacep_engine_hot_inspection_done_month_year")) Then
              aclsUpdate_Client_Aircraft_Engine.cliacep_engine_hot_inspection_done_month_year = R("cliacep_engine_hot_inspection_done_month_year")
            End If

            If Not IsDBNull(R("cliacep_engine_1_ser_nbr")) Then
              aclsUpdate_Client_Aircraft_Engine.cliacep_engine_1_ser_nbr = R("cliacep_engine_1_ser_nbr")
            End If
            If Not IsDBNull(R("cliacep_engine_2_ser_nbr")) Then
              aclsUpdate_Client_Aircraft_Engine.cliacep_engine_2_ser_nbr = R("cliacep_engine_2_ser_nbr")
            End If
            If Not IsDBNull(R("cliacep_engine_3_ser_nbr")) Then
              aclsUpdate_Client_Aircraft_Engine.cliacep_engine_3_ser_nbr = R("cliacep_engine_3_ser_nbr")
            End If
            If Not IsDBNull(R("cliacep_engine_4_ser_nbr")) Then
              aclsUpdate_Client_Aircraft_Engine.cliacep_engine_4_ser_nbr = R("cliacep_engine_4_ser_nbr")
            End If
            If Not IsDBNull(R("cliacep_engine_1_ttsn_hours")) Then
              aclsUpdate_Client_Aircraft_Engine.cliacep_engine_1_ttsn_hours = R("cliacep_engine_1_ttsn_hours")
            End If
            If Not IsDBNull(R("cliacep_engine_2_ttsn_hours")) Then
              aclsUpdate_Client_Aircraft_Engine.cliacep_engine_2_ttsn_hours = R("cliacep_engine_2_ttsn_hours")
            End If
            If Not IsDBNull(R("cliacep_engine_3_ttsn_hours")) Then
              aclsUpdate_Client_Aircraft_Engine.cliacep_engine_3_ttsn_hours = R("cliacep_engine_3_ttsn_hours")
            End If
            If Not IsDBNull(R("cliacep_engine_4_ttsn_hours")) Then
              aclsUpdate_Client_Aircraft_Engine.cliacep_engine_4_ttsn_hours = R("cliacep_engine_4_ttsn_hours")
            End If
            If Not IsDBNull(R("cliacep_engine_1_tsoh_hours")) Then
              aclsUpdate_Client_Aircraft_Engine.cliacep_engine_1_tsoh_hours = R("cliacep_engine_1_tsoh_hours")
            End If
            If Not IsDBNull(R("cliacep_engine_2_tsoh_hours")) Then
              aclsUpdate_Client_Aircraft_Engine.cliacep_engine_2_tsoh_hours = R("cliacep_engine_2_tsoh_hours")
            End If
            If Not IsDBNull(R("cliacep_engine_3_tsoh_hours")) Then
              aclsUpdate_Client_Aircraft_Engine.cliacep_engine_3_tsoh_hours = R("cliacep_engine_3_tsoh_hours")
            End If
            If Not IsDBNull(R("cliacep_engine_4_tsoh_hours")) Then
              aclsUpdate_Client_Aircraft_Engine.cliacep_engine_4_tsoh_hours = R("cliacep_engine_4_tsoh_hours")
            End If
            If Not IsDBNull(R("cliacep_engine_1_tshi_hours")) Then
              aclsUpdate_Client_Aircraft_Engine.cliacep_engine_1_tshi_hours = R("cliacep_engine_1_tshi_hours")
            End If
            If Not IsDBNull(R("cliacep_engine_2_tshi_hours")) Then
              aclsUpdate_Client_Aircraft_Engine.cliacep_engine_2_tshi_hours = R("cliacep_engine_2_tshi_hours")
            End If
            If Not IsDBNull(R("cliacep_engine_3_tshi_hours")) Then
              aclsUpdate_Client_Aircraft_Engine.cliacep_engine_3_tshi_hours = R("cliacep_engine_3_tshi_hours")
            End If
            If Not IsDBNull(R("cliacep_engine_4_tshi_hours")) Then
              aclsUpdate_Client_Aircraft_Engine.cliacep_engine_4_tshi_hours = R("cliacep_engine_4_tshi_hours")
            End If
            If Not IsDBNull(R("cliacep_engine_1_tbo_hours")) Then
              aclsUpdate_Client_Aircraft_Engine.cliacep_engine_1_tbo_hours = R("cliacep_engine_1_tbo_hours")
            End If
            If Not IsDBNull(R("cliacep_engine_2_tbo_hours")) Then
              aclsUpdate_Client_Aircraft_Engine.cliacep_engine_2_tbo_hours = R("cliacep_engine_2_tbo_hours")
            End If
            If Not IsDBNull(R("cliacep_engine_3_tbo_hours")) Then
              aclsUpdate_Client_Aircraft_Engine.cliacep_engine_3_tbo_hours = R("cliacep_engine_3_tbo_hours")
            End If
            If Not IsDBNull(R("cliacep_engine_4_tbo_hours")) Then
              aclsUpdate_Client_Aircraft_Engine.cliacep_engine_4_tbo_hours = R("cliacep_engine_4_tbo_hours")
            End If
            If Not IsDBNull(R("cliacep_engine_1_tsn_cycle")) Then
              aclsUpdate_Client_Aircraft_Engine.cliacep_engine_1_tsn_cycle = R("cliacep_engine_1_tsn_cycle")
            End If
            If Not IsDBNull(R("cliacep_engine_2_tsn_cycle")) Then
              aclsUpdate_Client_Aircraft_Engine.cliacep_engine_2_tsn_cycle = R("cliacep_engine_2_tsn_cycle")
            End If
            If Not IsDBNull(R("cliacep_engine_3_tsn_cycle")) Then
              aclsUpdate_Client_Aircraft_Engine.cliacep_engine_3_tsn_cycle = R("cliacep_engine_3_tsn_cycle")
            End If
            If Not IsDBNull(R("cliacep_engine_4_tsn_cycle")) Then
              aclsUpdate_Client_Aircraft_Engine.cliacep_engine_4_tsn_cycle = R("cliacep_engine_4_tsn_cycle")
            End If
            If Not IsDBNull(R("cliacep_engine_1_tsoh_cycle")) Then
              aclsUpdate_Client_Aircraft_Engine.cliacep_engine_1_tsoh_cycle = R("cliacep_engine_1_tsoh_cycle")
            End If
            If Not IsDBNull(R("cliacep_engine_2_tsoh_cycle")) Then
              aclsUpdate_Client_Aircraft_Engine.cliacep_engine_2_tsoh_cycle = R("cliacep_engine_2_tsoh_cycle")
            End If
            If Not IsDBNull(R("cliacep_engine_3_tsoh_cycle")) Then
              aclsUpdate_Client_Aircraft_Engine.cliacep_engine_3_tsoh_cycle = R("cliacep_engine_3_tsoh_cycle")
            End If
            If Not IsDBNull(R("cliacep_engine_4_tsoh_cycle")) Then
              aclsUpdate_Client_Aircraft_Engine.cliacep_engine_4_tsoh_cycle = R("cliacep_engine_4_tsoh_cycle")
            End If
            If Not IsDBNull(R("cliacep_engine_1_tshi_cycle")) Then
              aclsUpdate_Client_Aircraft_Engine.cliacep_engine_1_tshi_cycle = R("cliacep_engine_1_tshi_cycle")
            End If
            If Not IsDBNull(R("cliacep_engine_2_tshi_cycle")) Then
              aclsUpdate_Client_Aircraft_Engine.cliacep_engine_2_tshi_cycle = R("cliacep_engine_2_tshi_cycle")
            End If
            If Not IsDBNull(R("cliacep_engine_3_tshi_cycle")) Then
              aclsUpdate_Client_Aircraft_Engine.cliacep_engine_3_tshi_cycle = R("cliacep_engine_3_tshi_cycle")
            End If
            If Not IsDBNull(R("cliacep_engine_4_tshi_cycle")) Then
              aclsUpdate_Client_Aircraft_Engine.cliacep_engine_4_tshi_cycle = R("cliacep_engine_4_tshi_cycle")
            End If

          Next
        End If
      Else
        If aclsData_Temp.class_error <> "" Then
          error_string = aclsData_Temp.class_error
          error_string = "Aircraft_Edit_Details_Tabs.ascx.vb - Save_It() - " & error_string
          clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
        End If
      End If

      aclsData_Temp.Update_Client_Aircraft_Engine(aclsUpdate_Client_Aircraft_Engine)
      'Dim url As String = "details.aspx?source=" & Session.Item("ListingSource") & "&type=3&ac_ID=" & AircraftID
      System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.location.href = window.opener.location.href;", True)

      'System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.location = '" & url & "';", True)
      System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)
    Catch ex As Exception
      error_string = "Aircraft_Edit_Details_Tabs.ascx.vb - fill_engine_details() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
#End Region

End Class