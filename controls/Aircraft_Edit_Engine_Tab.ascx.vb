Imports System.IO
Partial Public Class Aircraft_Edit_Engine_Tab
  Inherits System.Web.UI.UserControl
  Public aclsData_Temp As New Object
  Public aTempTable, aTempTable2 As New DataTable 'Data Tables used 
  Dim error_string As String = ""
  Dim AircraftID As Long = 0
#Region "Page Events"

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Me.Visible Then
      Try
        '-------------------------------------------Database Connections--------------------------------------------------------------

        aclsData_Temp = New clsData_Manager_SQL
        aclsData_Temp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase") 'CApplication.Item("crmClientDatabase")
        aclsData_Temp.JETNET_DB = Session.Item("jetnetClientDatabase") 'CApplication.Item("crmJetnetDatabase")
        If Session.Item("crmUserLogon") <> True Then
          Response.Redirect("Default.aspx", False)
        End If
        Session("export_info") = ""

        '---------------------------------------------End Database Connection Stuff---------------------------------------------
      Catch ex As Exception
        error_string = "Aircraft_Edit_Details_Tabs.ascx.vb - PageLoad() - " & ex.Message
        clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
      End Try


      If Not IsNothing(Request.Item("ac_ID")) Then
        If Not String.IsNullOrEmpty(Request.Item("ac_ID").ToString) Then
          AircraftID = CLng(Request.Item("ac_ID").ToString.Trim)
        End If
      End If

      If AircraftID = 0 Then
        AircraftID = Session.Item("ListingID")
      End If


      If AircraftID > 0 Then 'You need an aircraft ID to use this page.

        engine_maintenance_program.Items.Add(New ListItem("NONE", 0))
        engine_management_program.Items.Add(New ListItem("NONE", 0))

        Dim atemptable3 As New DataTable
        If Not Page.IsPostBack Then
          atemptable3 = aclsData_Temp.lookupAirframeEngine_Mait(1, 0, 0, "Engine", True)
          If Not IsNothing(atemptable3) Then
            If atemptable3.Rows.Count > 0 Then
              For Each r As DataRow In atemptable3.Rows
                If Not IsDBNull(r("emp_program_name")) And Not IsDBNull(r("emp_provider_name")) Then
                  If UCase(r("emp_program_name").ToString) = "UNKNOWN" Or UCase(r("emp_provider_name").ToString) = "UKNOWN" Then
                    engine_maintenance_program.Items.Add(New ListItem(r("emp_program_name"), r("emp_id")))
                  Else
                    engine_maintenance_program.Items.Add(New ListItem(r("emp_program_name") & " " & r("emp_provider_name"), r("emp_id")))
                  End If

                End If

              Next
            End If
          End If


          atemptable3 = aclsData_Temp.lookupAirframeEngine_Mait(0, 0, 1, "Engine", True)
          If Not IsNothing(atemptable3) Then
            If atemptable3.Rows.Count > 0 Then
              For Each r As DataRow In atemptable3.Rows

                If Not IsDBNull(r("emgp_program_name")) And Not IsDBNull(r("emgp_provider_name")) Then
                  If UCase(r("emgp_program_name").ToString) = "UNKNOWN" Or UCase(r("emgp_provider_name").ToString) = "UKNOWN" Then
                    engine_management_program.Items.Add(New ListItem(r("emgp_program_name"), r("emgp_id")))
                  Else
                    engine_management_program.Items.Add(New ListItem(r("emgp_program_name") & " " & r("emgp_provider_name"), r("emgp_id")))
                  End If

                End If


              Next
            End If
          End If
        End If

        Try
          If Not Page.IsPostBack Then
            aTempTable2 = aclsData_Temp.Get_Client_Aircraft_Engine(AircraftID)
            If Not IsNothing(aTempTable2) Then
              If aTempTable2.Rows.Count > 0 Then
                For Each R As DataRow In aTempTable2.Rows

                  Dim aircraftTable As New DataTable
                  aircraftTable = CommonAircraftFunctions.BuildReusableTable(AircraftID, 0, "CLIENT", "", aclsData_Temp, True, 0, "CLIENT")

                  If Not IsNothing(aircraftTable) Then
                    If aircraftTable.Rows.Count > 0 Then
                      title_change.Text = CommonAircraftFunctions.CreateHeaderLine(aircraftTable.Rows(0).Item("amod_make_name"), aircraftTable.Rows(0).Item("amod_model_name"), aircraftTable.Rows(0).Item("ac_ser_nbr"), "")
                    End If
                  End If

                  If Not IsDBNull(R("cliacep_engine_name")) Then
                    engine_model.Text = R("cliacep_engine_name")
                  End If
                  If Not IsDBNull(R("cliacep_engine_maintenance_program")) Then
                    ' engine_maintenance_program.Items.Add(New ListItem(R("cliacep_engine_maintenance_program"), R("cliacep_engine_maintenance_program")))
                    engine_maintenance_program.SelectedValue = R("cliacep_engine_maintenance_program")
                  End If
                  If Not IsDBNull(R("cliacep_engine_management_program")) Then
                    'engine_management_program.Items.Add(New ListItem(R("cliacep_engine_management_program"), R("cliacep_engine_management_program")))
                    engine_management_program.SelectedValue = R("cliacep_engine_management_program")
                  End If
                  If Not IsDBNull(R("cliacep_engine_tbo_oc_flag")) Then
                    on_condition_tbo_rd.SelectedValue = R("cliacep_engine_tbo_oc_flag")
                  End If
                  If Not IsDBNull(R("cliacep_engine_noise_rating")) Then
                    noise_rating.Text = R("cliacep_engine_noise_rating")
                  End If
                  If Not IsDBNull(R("cliacep_engine_model_config")) Then
                    model_config.Text = R("cliacep_engine_model_config")
                  End If
                  If Not IsDBNull(R("cliacep_engine_overhaul_done_by_name")) Then
                    overhaul_done_by_name.Text = R("cliacep_engine_overhaul_done_by_name")
                  End If
                  If Not IsDBNull(R("cliacep_engine_overhaul_done_month_year")) Then
                    overhaul_done_month_year.Text = R("cliacep_engine_overhaul_done_month_year")
                  End If
                  If Not IsDBNull(R("cliacep_engine_hot_inspection_done_by_name")) Then
                    hot_inspection_done_by_name.Text = R("cliacep_engine_hot_inspection_done_by_name")
                  End If
                  If Not IsDBNull(R("cliacep_engine_hot_inspection_done_month_year")) Then
                    hot_inspection_done_month_year.Text = R("cliacep_engine_hot_inspection_done_month_year")
                  End If
                  If Not IsDBNull(R("cliacep_engine_1_ser_nbr")) Then
                    engine_1_ser.Text = R("cliacep_engine_1_ser_nbr")
                  End If
                  If Not IsDBNull(R("cliacep_engine_2_ser_nbr")) Then
                    engine_2_ser.Text = R("cliacep_engine_2_ser_nbr")
                  End If
                  If Not IsDBNull(R("cliacep_engine_3_ser_nbr")) Then
                    engine_3_ser.Text = R("cliacep_engine_3_ser_nbr")
                  End If
                  If Not IsDBNull(R("cliacep_engine_4_ser_nbr")) Then
                    engine_4_ser.Text = R("cliacep_engine_4_ser_nbr")
                  End If
                  If Not IsDBNull(R("cliacep_engine_1_ttsn_hours")) Then
                    engine_1_ttsnew.Text = R("cliacep_engine_1_ttsn_hours")
                  End If
                  If Not IsDBNull(R("cliacep_engine_2_ttsn_hours")) Then
                    engine_2_ttsnew.Text = R("cliacep_engine_2_ttsn_hours")
                  End If
                  If Not IsDBNull(R("cliacep_engine_3_ttsn_hours")) Then
                    engine_3_ttsnew.Text = R("cliacep_engine_3_ttsn_hours")
                  End If
                  If Not IsDBNull(R("cliacep_engine_4_ttsn_hours")) Then
                    engine_4_ttsnew.Text = R("cliacep_engine_4_ttsn_hours")
                  End If
                  If Not IsDBNull(R("cliacep_engine_1_tsoh_hours")) Then
                    engine_1_soh.Text = R("cliacep_engine_1_tsoh_hours")
                  End If
                  If Not IsDBNull(R("cliacep_engine_2_tsoh_hours")) Then
                    engine_2_soh.Text = R("cliacep_engine_2_tsoh_hours")
                  End If
                  If Not IsDBNull(R("cliacep_engine_3_tsoh_hours")) Then
                    engine_3_soh.Text = R("cliacep_engine_3_tsoh_hours")
                  End If
                  If Not IsDBNull(R("cliacep_engine_4_tsoh_hours")) Then
                    engine_4_soh.Text = R("cliacep_engine_4_tsoh_hours")
                  End If
                  If Not IsDBNull(R("cliacep_engine_1_tshi_hours")) Then
                    engine_1_shi.Text = R("cliacep_engine_1_tshi_hours")
                  End If
                  If Not IsDBNull(R("cliacep_engine_2_tshi_hours")) Then
                    engine_2_shi.Text = R("cliacep_engine_2_tshi_hours")
                  End If
                  If Not IsDBNull(R("cliacep_engine_3_tshi_hours")) Then
                    engine_3_shi.Text = R("cliacep_engine_3_tshi_hours")
                  End If
                  If Not IsDBNull(R("cliacep_engine_4_tshi_hours")) Then
                    engine_4_shi.Text = R("cliacep_engine_4_tshi_hours")
                  End If
                  If Not IsDBNull(R("cliacep_engine_1_tbo_hours")) Then
                    engine_1_tbo.Text = R("cliacep_engine_1_tbo_hours")
                  End If
                  If Not IsDBNull(R("cliacep_engine_2_tbo_hours")) Then
                    engine_2_tbo.Text = R("cliacep_engine_2_tbo_hours")
                  End If
                  If Not IsDBNull(R("cliacep_engine_3_tbo_hours")) Then
                    engine_3_tbo.Text = R("cliacep_engine_3_tbo_hours")
                  End If
                  If Not IsDBNull(R("cliacep_engine_4_tbo_hours")) Then
                    engine_4_tbo.Text = R("cliacep_engine_4_tbo_hours")
                  End If
                  If Not IsDBNull(R("cliacep_engine_1_tsn_cycle")) Then
                    engine_1_tot_snew_cycle.Text = R("cliacep_engine_1_tsn_cycle")
                  End If
                  If Not IsDBNull(R("cliacep_engine_2_tsn_cycle")) Then
                    engine_2_tot_snew_cycle.Text = R("cliacep_engine_2_tsn_cycle")
                  End If
                  If Not IsDBNull(R("cliacep_engine_3_tsn_cycle")) Then
                    engine_3_tot_snew_cycle.Text = R("cliacep_engine_3_tsn_cycle")
                  End If
                  If Not IsDBNull(R("cliacep_engine_4_tsn_cycle")) Then
                    engine_4_tot_snew_cycle.Text = R("cliacep_engine_4_tsn_cycle")
                  End If
                  If Not IsDBNull(R("cliacep_engine_1_tsoh_cycle")) Then
                    engine_1_tot_overhaul_cycles.Text = R("cliacep_engine_1_tsoh_cycle")
                  End If
                  If Not IsDBNull(R("cliacep_engine_2_tsoh_cycle")) Then
                    engine_2_tot_overhaul_cycles.Text = R("cliacep_engine_2_tsoh_cycle")
                  End If
                  If Not IsDBNull(R("cliacep_engine_3_tsoh_cycle")) Then
                    engine_3_tot_overhaul_cycles.Text = R("cliacep_engine_3_tsoh_cycle")
                  End If
                  If Not IsDBNull(R("cliacep_engine_4_tsoh_cycle")) Then
                    engine_4_tot_overhaul_cycles.Text = R("cliacep_engine_4_tsoh_cycle")
                  End If
                  If Not IsDBNull(R("cliacep_engine_1_tshi_cycle")) Then
                    engine_1_tot_cycle_shot.Text = R("cliacep_engine_1_tshi_cycle")
                  End If
                  If Not IsDBNull(R("cliacep_engine_2_tshi_cycle")) Then
                    engine_2_tot_cycle_shot.Text = R("cliacep_engine_2_tshi_cycle")
                  End If
                  If Not IsDBNull(R("cliacep_engine_3_tshi_cycle")) Then
                    engine_3_tot_cycle_shot.Text = R("cliacep_engine_3_tshi_cycle")
                  End If
                  If Not IsDBNull(R("cliacep_engine_4_tshi_cycle")) Then
                    engine_4_tot_cycle_shot.Text = R("cliacep_engine_4_tshi_cycle")
                  End If
                Next
              Else
                new_engine.Text = "true"
              End If
            End If
          End If

        Catch ex As Exception
          error_string = "Aircraft_Edit_Engine_Tabs.ascx.vb - Page_Load() - " & ex.Message
          clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
        End Try
      End If
    End If
  End Sub
#End Region
#Region "Update Event"
  Private Sub update_Click() Handles updateButton.Click
    Try
      If AircraftID > 0 Then
        Dim aclsUpdate_Client_Aircraft_Engine As New clsClient_Aircraft_Engine

        'If new_engine.Text <> "true" Then
        aclsUpdate_Client_Aircraft_Engine.cliacep_cliac_id = AircraftID
        'End If
        If Not IsDBNull(engine_model.Text) Then
          aclsUpdate_Client_Aircraft_Engine.cliacep_engine_name = engine_model.Text
        End If
        If Not IsDBNull(engine_maintenance_program.SelectedValue) Then
          If engine_maintenance_program.SelectedValue <> "" Then
            aclsUpdate_Client_Aircraft_Engine.cliacep_engine_maintenance_program = engine_maintenance_program.SelectedValue
          End If
        End If
        If Not IsDBNull(engine_management_program.SelectedValue) Then
          If engine_management_program.SelectedValue <> "" Then
            aclsUpdate_Client_Aircraft_Engine.cliacep_engine_management_program = engine_management_program.SelectedValue
          End If
        End If
        If Not IsDBNull(on_condition_tbo_rd.SelectedValue) Then
          aclsUpdate_Client_Aircraft_Engine.cliacep_engine_tbo_oc_flag = on_condition_tbo_rd.SelectedValue
        End If
        If Not IsDBNull(noise_rating.Text) Then
          If noise_rating.Text <> "" Then
            aclsUpdate_Client_Aircraft_Engine.cliacep_engine_noise_rating = noise_rating.Text
          End If
        End If
        If Not IsDBNull(model_config.Text) Then
          aclsUpdate_Client_Aircraft_Engine.cliacep_engine_model_config = model_config.Text
        End If
        If Not IsDBNull(overhaul_done_by_name.Text) Then
          aclsUpdate_Client_Aircraft_Engine.cliacep_engine_overhaul_done_by_name = overhaul_done_by_name.Text
        End If
        If Not IsDBNull(overhaul_done_month_year.Text) Then
          aclsUpdate_Client_Aircraft_Engine.cliacep_engine_overhaul_done_month_year = overhaul_done_month_year.Text
        End If
        If Not IsDBNull(hot_inspection_done_by_name.Text) Then
          aclsUpdate_Client_Aircraft_Engine.cliacep_engine_hot_inspection_done_by_name = hot_inspection_done_by_name.Text
        End If
        If Not IsDBNull(hot_inspection_done_month_year.Text) Then
          aclsUpdate_Client_Aircraft_Engine.cliacep_engine_hot_inspection_done_month_year = hot_inspection_done_month_year.Text
        End If

        If Not IsDBNull(engine_1_ser.Text) Then
          aclsUpdate_Client_Aircraft_Engine.cliacep_engine_1_ser_nbr = engine_1_ser.Text
        End If
        If Not IsDBNull(engine_2_ser.Text) Then
          aclsUpdate_Client_Aircraft_Engine.cliacep_engine_2_ser_nbr = engine_2_ser.Text
        End If
        If Not IsDBNull(engine_3_ser.Text) Then
          aclsUpdate_Client_Aircraft_Engine.cliacep_engine_3_ser_nbr = engine_3_ser.Text
        End If
        If Not IsDBNull(engine_4_ser.Text) Then
          aclsUpdate_Client_Aircraft_Engine.cliacep_engine_4_ser_nbr = engine_4_ser.Text
        End If

        If Not IsDBNull(engine_1_ttsnew.Text) Then
          If engine_1_ttsnew.Text <> "" Then
            aclsUpdate_Client_Aircraft_Engine.cliacep_engine_1_ttsn_hours = engine_1_ttsnew.Text
          End If
        End If
        If Not IsDBNull(engine_2_ttsnew.Text) Then
          If engine_2_ttsnew.Text <> "" Then
            aclsUpdate_Client_Aircraft_Engine.cliacep_engine_2_ttsn_hours = engine_2_ttsnew.Text
          End If
        End If
        If Not IsDBNull(engine_3_ttsnew.Text) Then
          If engine_3_ttsnew.Text <> "" Then
            aclsUpdate_Client_Aircraft_Engine.cliacep_engine_3_ttsn_hours = engine_3_ttsnew.Text
          End If
        End If
        If Not IsDBNull(engine_4_ttsnew.Text) Then
          If engine_4_ttsnew.Text <> "" Then
            aclsUpdate_Client_Aircraft_Engine.cliacep_engine_4_ttsn_hours = engine_4_ttsnew.Text
          End If
        End If
        If Not IsDBNull(engine_1_soh.Text) Then
          If engine_1_soh.Text <> "" Then
            aclsUpdate_Client_Aircraft_Engine.cliacep_engine_1_tsoh_hours = engine_1_soh.Text
          End If
        End If
        If Not IsDBNull(engine_2_soh.Text) Then
          If engine_2_soh.Text <> "" Then
            aclsUpdate_Client_Aircraft_Engine.cliacep_engine_2_tsoh_hours = engine_2_soh.Text
          End If
        End If
        If Not IsDBNull(engine_3_soh.Text) Then
          If engine_3_soh.Text <> "" Then
            aclsUpdate_Client_Aircraft_Engine.cliacep_engine_3_tsoh_hours = engine_3_soh.Text
          End If
        End If
        If Not IsDBNull(engine_4_soh.Text) Then
          If engine_4_soh.Text <> "" Then
            aclsUpdate_Client_Aircraft_Engine.cliacep_engine_4_tsoh_hours = engine_4_soh.Text
          End If
        End If
        If Not IsDBNull(engine_1_shi.Text) Then
          If engine_1_shi.Text <> "" Then
            aclsUpdate_Client_Aircraft_Engine.cliacep_engine_1_tshi_hours = engine_1_shi.Text
          End If
        End If
        If Not IsDBNull(engine_2_shi.Text) Then
          If engine_2_shi.Text <> "" Then
            aclsUpdate_Client_Aircraft_Engine.cliacep_engine_2_tshi_hours = engine_2_shi.Text
          End If
        End If
        If Not IsDBNull(engine_3_shi.Text) Then
          If engine_3_shi.Text <> "" Then
            aclsUpdate_Client_Aircraft_Engine.cliacep_engine_3_tshi_hours = engine_3_shi.Text
          End If
        End If
        If Not IsDBNull(engine_4_shi.Text) Then
          If engine_4_shi.Text <> "" Then
            aclsUpdate_Client_Aircraft_Engine.cliacep_engine_4_tshi_hours = engine_4_shi.Text
          End If
        End If
        If Not IsDBNull(engine_4_shi.Text) Then
          If engine_4_shi.Text <> "" Then
            aclsUpdate_Client_Aircraft_Engine.cliacep_engine_4_tshi_hours = engine_4_shi.Text
          End If
        End If
        If Not IsDBNull(engine_1_tbo.Text) Then
          If engine_1_tbo.Text <> "" Then
            aclsUpdate_Client_Aircraft_Engine.cliacep_engine_1_tbo_hours = engine_1_tbo.Text
          End If
        End If
        If Not IsDBNull(engine_2_tbo.Text) Then
          If engine_2_tbo.Text <> "" Then
            aclsUpdate_Client_Aircraft_Engine.cliacep_engine_2_tbo_hours = engine_2_tbo.Text
          End If
        End If
        If Not IsDBNull(engine_3_tbo.Text) Then
          If engine_3_tbo.Text <> "" Then
            aclsUpdate_Client_Aircraft_Engine.cliacep_engine_3_tbo_hours = engine_3_tbo.Text
          End If
        End If
        If Not IsDBNull(engine_4_tbo.Text) Then
          If engine_4_tbo.Text <> "" Then
            aclsUpdate_Client_Aircraft_Engine.cliacep_engine_4_tbo_hours = engine_4_tbo.Text
          End If
        End If
        If Not IsDBNull(engine_1_tot_snew_cycle.Text) Then
          If engine_1_tot_snew_cycle.Text <> "" Then
            aclsUpdate_Client_Aircraft_Engine.cliacep_engine_1_tsn_cycle = engine_1_tot_snew_cycle.Text
          End If
        End If
        If Not IsDBNull(engine_2_tot_snew_cycle.Text) Then
          If engine_2_tot_snew_cycle.Text <> "" Then
            aclsUpdate_Client_Aircraft_Engine.cliacep_engine_2_tsn_cycle = engine_2_tot_snew_cycle.Text
          End If
        End If
        If Not IsDBNull(engine_3_tot_snew_cycle.Text) Then
          If engine_3_tot_snew_cycle.Text <> "" Then
            aclsUpdate_Client_Aircraft_Engine.cliacep_engine_3_tsn_cycle = engine_3_tot_snew_cycle.Text
          End If
        End If
        If Not IsDBNull(engine_4_tot_snew_cycle.Text) Then
          If engine_4_tot_snew_cycle.Text <> "" Then
            aclsUpdate_Client_Aircraft_Engine.cliacep_engine_4_tsn_cycle = engine_4_tot_snew_cycle.Text
          End If
        End If
        If Not IsDBNull(engine_1_tot_overhaul_cycles.Text) Then
          If engine_1_tot_overhaul_cycles.Text <> "" Then
            aclsUpdate_Client_Aircraft_Engine.cliacep_engine_1_tsoh_cycle = engine_1_tot_overhaul_cycles.Text
          End If
        End If
        If Not IsDBNull(engine_2_tot_overhaul_cycles.Text) Then
          If engine_2_tot_overhaul_cycles.Text <> "" Then
            aclsUpdate_Client_Aircraft_Engine.cliacep_engine_2_tsoh_cycle = engine_2_tot_overhaul_cycles.Text
          End If
        End If
        If Not IsDBNull(engine_3_tot_overhaul_cycles.Text) Then
          If engine_3_tot_overhaul_cycles.Text <> "" Then
            aclsUpdate_Client_Aircraft_Engine.cliacep_engine_3_tsoh_cycle = engine_3_tot_overhaul_cycles.Text
          End If
        End If
        If Not IsDBNull(engine_4_tot_overhaul_cycles.Text) Then
          If engine_4_tot_overhaul_cycles.Text <> "" Then
            aclsUpdate_Client_Aircraft_Engine.cliacep_engine_4_tsoh_cycle = engine_4_tot_overhaul_cycles.Text
          End If
        End If

        If Not IsDBNull(engine_1_tot_cycle_shot.Text) Then
          If engine_1_tot_cycle_shot.Text <> "" Then
            aclsUpdate_Client_Aircraft_Engine.cliacep_engine_1_tshi_cycle = engine_1_tot_cycle_shot.Text
          End If
        End If

        If Not IsDBNull(engine_2_tot_cycle_shot.Text) Then
          If engine_2_tot_cycle_shot.Text <> "" Then
            aclsUpdate_Client_Aircraft_Engine.cliacep_engine_2_tshi_cycle = engine_2_tot_cycle_shot.Text
          End If
        End If
        If Not IsDBNull(engine_3_tot_cycle_shot.Text) Then
          If engine_3_tot_cycle_shot.Text <> "" Then
            aclsUpdate_Client_Aircraft_Engine.cliacep_engine_3_tshi_cycle = engine_3_tot_cycle_shot.Text
          End If
        End If
        If Not IsDBNull(engine_4_tot_cycle_shot.Text) Then
          If engine_4_tot_cycle_shot.Text <> "" Then
            aclsUpdate_Client_Aircraft_Engine.cliacep_engine_4_tshi_cycle = engine_4_tot_cycle_shot.Text
          End If
        End If

        If new_engine.Text <> "true" Then
          aclsData_Temp.Update_Client_Aircraft_Engine(aclsUpdate_Client_Aircraft_Engine)
        Else
          aclsData_Temp.Insert_Client_Aircraft_Engine(aclsUpdate_Client_Aircraft_Engine)
        End If



        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.location.href = window.opener.location.href;", True)
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)

      End If
    Catch ex As Exception
      error_string = "Aircraft_Edit_Engine_Tabs.ascx.vb - update_click() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
#End Region

End Class