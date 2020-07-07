' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/homebaseEditAircraftModel.aspx.vb $
'$$Author: Matt $
'$$Date: 4/20/20 1:00p $
'$$Modtime: 4/20/20 8:02a $
'$$Revision: 15 $
'$$Workfile: homebaseEditAircraftModel.aspx.vb $
'
' ********************************************************************************

Partial Public Class homebaseEditAircraftModel

  Inherits System.Web.UI.Page
  Private inAcID As Long = 0
  Private inAmodID As Long = 0
  Private sModelTask As String = ""
  Private bAddNewModel As Boolean = False
  Private bSaveModel As Boolean = False

  Public Shared masterPage As New Object

  Private Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

    If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO Then
      Me.MasterPageFile = "~/EvoStyles/EmptyEvoTheme.Master"
      masterPage = DirectCast(Page.Master, EmptyEvoTheme)
    ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Then
      Me.MasterPageFile = "~/EvoStyles/EmptyHomebaseTheme.Master"
      masterPage = DirectCast(Page.Master, EmptyHomebaseTheme)
    ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
      Me.MasterPageFile = "~/EvoStyles/EmptyCustomerAdminTheme.Master"
      masterPage = DirectCast(Page.Master, EmptyCustomerAdminTheme)
    End If

  End Sub

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    Dim sErrorString As String = ""

    ' get request variable
    If Session.Item("crmUserLogon") <> True Then
      Response.Redirect("Default.aspx", False)
    Else

      If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Then
        Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("Homebase - Edit Aircraft Model")
        masterPage.SetPageTitle("Homebase - Edit Aircraft Model")
      ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
        Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("Admin - Edit Aircraft Model")
        masterPage.SetPageTitle("Admin - Edit Aircraft Model")
      End If

      If Not Session.Item("localPreferences").loadUserSession(sErrorString, CLng(HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString),
                                                            HttpContext.Current.Session.Item("localUser").crmUserLogin.ToString,
                                                            CLng(HttpContext.Current.Session.Item("localUser").crmSubSeqNo.ToString),
                                                            CLng(HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString)) Then
        Response.Redirect("Default.aspx", True)
      End If

    End If

    If Not IsNothing(Request.Item("AircraftID")) Then
      If Not String.IsNullOrEmpty(Request.Item("AircraftID").Trim) Then
        If IsNumeric(Request.Item("AircraftID")) Then
          inAcID = CLng(Request.Item("AircraftID"))
        End If
      End If
    End If

    If Not IsNothing(modelList) Then
      If Not String.IsNullOrEmpty(modelList.SelectedValue.Trim) Then
        If IsNumeric(modelList.SelectedValue) Then
          inAmodID = CLng(modelList.SelectedValue)
        End If
      End If
    End If

    If Not IsNothing(Request.Item("task")) Then
      If Not String.IsNullOrEmpty(Request.Item("task").ToString.Trim) Then
        sModelTask = Request.Item("task").ToString.ToUpper.Trim

        If sModelTask.ToLower.Contains("add") Then
          bAddNewModel = True

          If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Then
            Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("Homebase - ADD Aircraft Model")
            masterPage.SetPageTitle("Homebase - ADD Aircraft Model")
          ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
            Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("Admin - ADD Aircraft Model")
            masterPage.SetPageTitle("Admin - ADD Aircraft Model")
          End If

        End If

        If sModelTask.ToLower.Contains("save") Then
          bSaveModel = True
        End If

      End If
    End If

    fillModelsDropdown()

    If Not bSaveModel And Not bAddNewModel Then

      modelIntel.Attributes.Remove("onclick")
      modelIntel.Attributes.Add("onclick", "openSmallWindowJS('DisplayModelDetail.aspx?id=" + inAmodID.ToString + "','Model Intelligence');return true;")

            model_attributes.Attributes.Remove("onclick")
            model_attributes.Attributes.Add("onclick", "openSmallWindowJS('home_model.aspx?modelID=" + inAmodID.ToString + "','Model Attributes');return true;")

            If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
                model_assett_insight_features.Attributes.Remove("onclick")
                model_assett_insight_features.Attributes.Add("onclick", "openSmallWindowJS('viewtopdf.aspx?viewID=111&amod_id=" + inAmodID.ToString + "','Model Attributes/Features PDF');return true;")
                model_assett_insight_features.Visible = True
            End If


            displayModel()
            End If

            ' enable save buttons on "homebase test" for now
            If Not Session.Item("localSubscription").crmFrequency.ToString.ToLower.Contains("test") Then

      saveModel0.Enabled = False
      saveModel1.Enabled = False
      saveModel2.Enabled = False

    End If

  End Sub

  Private Sub displayModel()

    Try

      ' display aircraft model info

      Dim modelInfo As New homebaseModelInfoClass(inAmodID)

      modelInfo.fillModelInfoClass()

      If Not String.IsNullOrEmpty(modelInfo.amod_make_name) Then
        amod_make_name.Text = modelInfo.amod_make_name.Trim
      End If

      If Not String.IsNullOrEmpty(modelInfo.amod_model_name) Then
        amod_model_name.Text = modelInfo.amod_model_name.Trim
      End If

      make_model_Label.Text = "ID :  " + inAmodID.ToString + " - " + amod_make_name.Text + " / " + amod_model_name.Text

      make_model_Label.ToolTip = "Model ID : " + inAmodID.ToString

      If Not String.IsNullOrEmpty(modelInfo.amod_manufacturer) Then
        amod_manufacturer.Text = modelInfo.amod_manufacturer.Trim
        amod_manufacturer.ToolTip = modelInfo.amod_manufacturer.Trim
      End If

      If Not String.IsNullOrEmpty(modelInfo.amod_manufacturer_comp_id) Then
        amod_manufacturer_comp_id.Text = modelInfo.amod_manufacturer_comp_id.ToString.Trim
      End If

      If Not String.IsNullOrEmpty(modelInfo.amod_make_abbrev) Then
        amod_make_abbrev.Text = modelInfo.amod_make_abbrev.Trim
      End If

      If Not String.IsNullOrEmpty(modelInfo.amod_model_abbrev) Then
        amod_model_abbrev.Text = modelInfo.amod_model_abbrev.Trim
      End If

      If Not String.IsNullOrEmpty(modelInfo.amod_manufacturer_common_name) Then
        amod_manufacturer_common_name.Text = modelInfo.amod_manufacturer_common_name.Trim
      End If

      model_mfr_Label.Text = amod_manufacturer.Text

      If Not String.IsNullOrEmpty(modelInfo.amod_id) Then
        amod_id.Text = modelInfo.amod_id.ToString.Trim
      End If

      If Not String.IsNullOrEmpty(modelInfo.amod_airframe_type_code) Then
        amod_airframe_type_code.SelectedValue = modelInfo.amod_airframe_type_code.ToUpper.Trim
        amod_airframe_type_code.ToolTip = modelInfo.amod_airframe_type_code.Trim
      End If

      If Not String.IsNullOrEmpty(modelInfo.amod_faa_model_id) Then
        amod_faa_model_id.Text = modelInfo.amod_faa_model_id.Trim
      End If

      If Not String.IsNullOrEmpty(modelInfo.amod_jniq_size) Then
        amod_jniq_size.SelectedValue = modelInfo.amod_jniq_size.ToUpper.Trim
        amod_jniq_size.ToolTip = modelInfo.amod_jniq_size.Trim
      End If

      If Not String.IsNullOrEmpty(modelInfo.amod_type_code) Then

        If (modelInfo.amod_airframe_type_code.ToUpper = "R") Then
          If (modelInfo.amod_type_code.ToUpper = "T") Then
            amod_type_code.SelectedValue = "T" ' set for turbine
          Else
            amod_type_code.SelectedValue = modelInfo.amod_type_code.ToUpper
          End If

        Else
          If (modelInfo.amod_type_code.ToUpper = "T") Then
            amod_type_code.SelectedValue = "TP" ' set for turbo prop
          Else
            amod_type_code.SelectedValue = modelInfo.amod_type_code.ToUpper
          End If

        End If

        amod_type_code.ToolTip = modelInfo.amod_type_code.Trim

      End If

      If Not String.IsNullOrEmpty(modelInfo.amod_class_code) Then
        amod_class_code.Text = modelInfo.amod_class_code.Trim
      End If

      If Not String.IsNullOrEmpty(modelInfo.amod_weight_class) Then
        amod_weight_class.SelectedValue = modelInfo.amod_weight_class.ToUpper
        amod_weight_class.ToolTip = modelInfo.amod_weight_class.Trim
      End If

      If Not String.IsNullOrEmpty(modelInfo.amod_start_year) Then
        amod_start_year.Text = modelInfo.amod_start_year.Trim.Trim
      End If

      If Not String.IsNullOrEmpty(modelInfo.amod_end_year) Then
        amod_end_year.Text = modelInfo.amod_end_year.Trim
      End If

      If Not String.IsNullOrEmpty(modelInfo.amod_start_price) Then
        amod_start_price.Text = FormatNumber(modelInfo.amod_start_price.Trim, 0, False, False, True)
      End If

      If Not String.IsNullOrEmpty(modelInfo.amod_end_price) Then
        amod_end_price.Text = FormatNumber(modelInfo.amod_end_price.Trim, 0, False, False, True)
      End If

      If Not String.IsNullOrEmpty(modelInfo.amod_description) Then
        amod_description.Text = modelInfo.amod_description.Trim
      End If

      amod_product_business_flag.Checked = modelInfo.amod_product_business_flag

      amod_product_commercial_flag.Checked = modelInfo.amod_product_commercial_flag

      amod_product_airbp_flag.Checked = modelInfo.amod_product_airbp_flag

      amod_product_helicopter_flag.Checked = modelInfo.amod_product_helicopter_flag


      amod_product_abi_flag.Checked = False
      amod_product_regional_flag.Checked = False

      amod_product_abi_flag.Enabled = False
      amod_product_regional_flag.Enabled = False

      If Not String.IsNullOrEmpty(modelInfo.amod_ser_no_prefix) Then
        amod_ser_no_prefix.Text = modelInfo.amod_ser_no_prefix.Trim
      End If

      If Not String.IsNullOrEmpty(modelInfo.amod_ser_no_start) Then
        amod_ser_no_start.Text = modelInfo.amod_ser_no_start.Trim
      End If

      If Not String.IsNullOrEmpty(modelInfo.amod_ser_no_end) Then
        amod_ser_no_end.Text = modelInfo.amod_ser_no_end.Trim
      End If

      If Not String.IsNullOrEmpty(modelInfo.amod_ser_no_suffix) Then
        amod_ser_no_suffix.Text = modelInfo.amod_ser_no_suffix.Trim
      End If

      amod_serno_hyphen_flag.Checked = modelInfo.amod_serno_hyphen_flag

      If Not String.IsNullOrEmpty(modelInfo.amod_body_config) Then
        amod_body_config.SelectedValue = modelInfo.amod_body_config.ToUpper.Trim
        amod_body_config.ToolTip = modelInfo.amod_body_config.Trim
      End If

      '' load tabs

      '' Performance
      '' FUSELAGE DIMENSIONS

      Dim modelPerf As New homebaseModelPerfSpecsClass(inAmodID)

      modelPerf.fillModelPerfSpecsClass()

      If modelPerf.amod_fuselage_length > 0 Then
        amod_fuselage_length.Text = FormatNumber(modelPerf.amod_fuselage_length.ToString.Trim, 1, False, False, True)
      End If

      If modelPerf.amod_fuselage_height > 0 Then
        amod_fuselage_height.Text = FormatNumber(modelPerf.amod_fuselage_height.ToString.Trim, 1, False, False, True)
      End If

      If modelInfo.amod_airframe_type_code.ToUpper.Contains("F") Then

        If modelPerf.amod_fuselage_wingspan > 0 Then
          amod_fuselage_wingspan.Visible = True
          amod_fuselage_width.Visible = False
          amod_fuselage_wingspan.Text = FormatNumber(modelPerf.amod_fuselage_wingspan.ToString.Trim, 1, False, False, True)
        End If

      Else

        If modelPerf.amod_fuselage_width > 0 Then
          amod_fuselage_wingspan.Visible = False
          amod_fuselage_width.Visible = True
          amod_fuselage_width.Text = FormatNumber(modelPerf.amod_fuselage_width.ToString.Trim, 1, False, False, True)
        End If

      End If


      '' TYPICAL CONFIGURATION
      If modelPerf.amod_number_of_crew > 0 Then
        amod_number_of_crew.Text = modelPerf.amod_number_of_crew.ToString.Trim
      End If

      If modelPerf.amod_number_of_passengers > 0 Then
        amod_number_of_passengers.Text = modelPerf.amod_number_of_passengers.ToString.Trim
      End If

      If modelInfo.amod_airframe_type_code.ToUpper.Contains("F") Then
        If modelPerf.amod_pressure > 0 Then
          amod_pressure.Text = FormatNumber(modelPerf.amod_pressure.ToString.Trim, 1, False, False, True)
        End If
      Else
        amod_pressure.Text = "N/A"
      End If


      '' WEIGHT
      If modelPerf.amod_max_ramp_weight > 0 Then
        amod_max_ramp_weight.Text = FormatNumber(modelPerf.amod_max_ramp_weight, 0, False, False, True)
      End If

      If modelPerf.amod_max_takeoff_weight > 0 Then
        amod_max_takeoff_weight.Text = FormatNumber(modelPerf.amod_max_takeoff_weight, 0, False, False, True)
      End If

      If modelInfo.amod_airframe_type_code.ToUpper.Contains("F") Then
        If modelPerf.amod_zero_fuel_weight > 0 Then
          amod_zero_fuel_weight.Text = FormatNumber(modelPerf.amod_zero_fuel_weight, 0, False, False, True)
        End If
      Else
        amod_zero_fuel_weight.Text = "N/A"
      End If

      If modelPerf.amod_weight_eow > 0 Then
        amod_weight_eow.Text = FormatNumber(modelPerf.amod_weight_eow, 0, False, False, True)
      End If

      If modelPerf.amod_basic_op_weight > 0 Then
        amod_basic_op_weight.Text = FormatNumber(modelPerf.amod_basic_op_weight, 0, False, False, True)
      End If

      If modelPerf.amod_max_landing_weight > 0 Then
        amod_max_landing_weight.Text = FormatNumber(modelPerf.amod_max_landing_weight, 0, False, False, True)
      End If

      ' IFR Certification
      If Not String.IsNullOrEmpty(modelPerf.amod_ifr_certification.Trim) Then
        amod_ifr_certification.Text = modelPerf.amod_ifr_certification.Trim
      End If

      ' CLIMB
      If modelPerf.amod_climb_normal_feet > 0 Then
        amod_climb_normal_feet.Text = FormatNumber(modelPerf.amod_climb_normal_feet, 0, False, False, True)
      End If

      If modelPerf.amod_climb_engout_feet > 0 Then
        amod_climb_engout_feet.Text = FormatNumber(modelPerf.amod_climb_engout_feet, 0, False, False, True)
      End If

      If modelPerf.amod_ceiling_feet > 0 Then
        amod_ceiling_feet.Text = FormatNumber(modelPerf.amod_ceiling_feet, 0, False, False, True)
      End If

      If modelInfo.amod_airframe_type_code.ToUpper.Contains("F") Then

        amod_climb_hoge.Text = "N/A"
        amod_climb_hige.Text = "N/A"

      Else

        If modelPerf.amod_climb_hoge > 0 Then
          amod_climb_hoge.Text = FormatNumber(modelPerf.amod_climb_hoge, 0, False, False, True)
        End If

        If modelPerf.amod_climb_hige > 0 Then
          amod_climb_hige.Text = FormatNumber(modelPerf.amod_climb_hige, 0, False, False, True)
        End If

      End If

      ' RANGE
      If modelPerf.amod_max_range_miles > 0 Then
        amod_max_range_miles.Text = FormatNumber(modelPerf.amod_max_range_miles, 0, False, False, True)
      End If

      If modelPerf.amod_range_tanks_full > 0 Then
        amod_range_tanks_full.Text = FormatNumber(modelPerf.amod_range_tanks_full, 0, False, False, True)
      End If

      If modelPerf.amod_range_seats_full > 0 Then
        amod_range_seats_full.Text = FormatNumber(modelPerf.amod_range_seats_full, 0, False, False, True)
      End If

      If modelPerf.amod_range_4_passenger > 0 Then
        amod_range_4_passenger.Text = FormatNumber(modelPerf.amod_range_4_passenger, 0, False, False, True)
      End If

      If modelPerf.amod_range_8_passenger > 0 Then
        amod_range_8_passenger.Text = FormatNumber(modelPerf.amod_range_8_passenger, 0, False, False, True)
      End If

      ' PROPELLERS
      If modelPerf.amod_number_of_props > 0 Then
        amod_number_of_props.Text = FormatNumber(modelPerf.amod_number_of_props, 0, False, False, True)
      End If

      If Not String.IsNullOrEmpty(modelPerf.amod_prop_model_name.Trim) Then
        amod_prop_model_name.Text = modelPerf.amod_prop_model_name.Trim
      End If

      If Not String.IsNullOrEmpty(modelPerf.amod_prop_mfr_name.Trim) Then
        amod_prop_mfr_name.Text = modelPerf.amod_prop_mfr_name.Trim
      End If

      If modelPerf.amod_prop_com_tbo_hrs > 0 Then
        amod_prop_com_tbo_hrs.Text = FormatNumber(modelPerf.amod_prop_com_tbo_hrs, 0, False, False, True)
      End If

      ' CONFIG NOTE
      If Not String.IsNullOrEmpty(modelPerf.amod_other_config_note.Trim) Then
        amod_other_config_note.Text = modelPerf.amod_other_config_note.Trim
      End If

      ' CABIN DIMENSIONS
      If modelPerf.amod_cabinsize_height_feet > 0 Then
        amod_cabinsize_height_feet.Text = FormatNumber(modelPerf.amod_cabinsize_height_feet, 0, False, False, True)
      End If

      If modelPerf.amod_cabinsize_height_inches > 0 Then
        amod_cabinsize_height_inches.Text = FormatNumber(modelPerf.amod_cabinsize_height_inches, 0, False, False, True)
      End If

      If modelPerf.amod_cabinsize_width_feet > 0 Then
        amod_cabinsize_width_feet.Text = FormatNumber(modelPerf.amod_cabinsize_width_feet, 0, False, False, True)
      End If

      If modelPerf.amod_cabinsize_width_inches > 0 Then
        amod_cabinsize_width_inches.Text = FormatNumber(modelPerf.amod_cabinsize_width_inches, 0, False, False, True)
      End If

      If modelPerf.amod_cabinsize_length_feet > 0 Then
        amod_cabinsize_length_feet.Text = FormatNumber(modelPerf.amod_cabinsize_length_feet, 0, False, False, True)
      End If

      If modelPerf.amod_cabinsize_length_inches > 0 Then
        amod_cabinsize_length_inches.Text = FormatNumber(modelPerf.amod_cabinsize_length_inches, 0, False, False, True)
      End If

      If modelPerf.amod_cabin_volume > 0 Then
        amod_cabin_volume.Text = FormatNumber(modelPerf.amod_cabin_volume, 0, False, False, True)
      End If

      If modelPerf.amod_baggage_volume > 0 Then
        amod_baggage_volume.Text = FormatNumber(modelPerf.amod_baggage_volume, 0, False, False, True)
      End If

      ' FUEL CAPACITY
      If modelPerf.amod_fuel_cap_std_weight > 0 Then
        amod_fuel_cap_std_weight.Text = FormatNumber(modelPerf.amod_fuel_cap_std_weight, 0, False, False, True)
      End If

      If modelPerf.amod_fuel_cap_std_gal > 0 Then
        amod_fuel_cap_std_gal.Text = FormatNumber(modelPerf.amod_fuel_cap_std_gal, 0, False, False, True)
      End If

      If modelPerf.amod_fuel_cap_opt_weight > 0 Then
        amod_fuel_cap_opt_weight.Text = FormatNumber(modelPerf.amod_fuel_cap_opt_weight, 0, False, False, True)
      End If

      If modelPerf.amod_fuel_cap_opt_gal > 0 Then
        amod_fuel_cap_opt_gal.Text = FormatNumber(modelPerf.amod_fuel_cap_opt_gal, 0, False, False, True)
      End If

      ' SPEED
      If modelInfo.amod_airframe_type_code.ToUpper.Contains("F") Then

        If modelPerf.amod_stall_vs > 0 Then
          amod_stall_vs.Text = FormatNumber(modelPerf.amod_stall_vs, 0, False, False, True)
        End If

        If modelPerf.amod_stall_vso > 0 Then
          amod_stall_vso.Text = FormatNumber(modelPerf.amod_stall_vso, 0, False, False, True)
        End If

      Else

        amod_stall_vs.Text = "N/A"
        amod_stall_vso.Text = "N/A"

      End If

      If modelPerf.amod_cruis_speed > 0 Then
        amod_cruis_speed.Text = FormatNumber(modelPerf.amod_cruis_speed, 0, False, False, True)
      End If

      If modelPerf.amod_max_speed > 0 Then
        amod_max_speed.Text = FormatNumber(modelPerf.amod_max_speed, 0, False, False, True)
      End If

      If modelInfo.amod_airframe_type_code.ToUpper.Contains("F") Then

        amod_vne_maxop_speed.Text = "N/A"

      Else

        If modelPerf.amod_vne_maxop_speed > 0 Then
          amod_vne_maxop_speed.Text = FormatNumber(modelPerf.amod_vne_maxop_speed, 0, False, False, True)
        End If

      End If


      If modelPerf.amod_v1_takeoff_speed > 0 Then
        amod_v1_takeoff_speed.Text = FormatNumber(modelPerf.amod_v1_takeoff_speed, 0, False, False, True)
      End If

      If modelPerf.amod_vfe_max_flap_extended_speed > 0 Then
        amod_vfe_max_flap_extended_speed.Text = FormatNumber(modelPerf.amod_vfe_max_flap_extended_speed, 0, False, False, True)
      End If

      If modelPerf.amod_vle_max_landing_gear_ext_speed > 0 Then
        amod_vle_max_landing_gear_ext_speed.Text = FormatNumber(modelPerf.amod_vle_max_landing_gear_ext_speed, 0, False, False, True)
      End If

      ' LANDING PERFORMANCE
      If modelInfo.amod_airframe_type_code.ToUpper.Contains("F") Then

        If modelPerf.amod_field_length > 0 Then
          amod_field_length.Text = FormatNumber(modelPerf.amod_field_length, 0, False, False, True)
        End If

      Else

        amod_field_length.Text = "N/A"

      End If


      ' TAKEOFF PERFORMANCE
      If modelPerf.amod_takeoff_ali > 0 Then
        amod_takeoff_ali.Text = FormatNumber(modelPerf.amod_takeoff_ali, 0, False, False, True)
      End If

      If modelPerf.amod_takeoff_500 > 0 Then
        amod_takeoff_500.Text = FormatNumber(modelPerf.amod_takeoff_500, 0, False, False, True)
      End If

      ' ENGINES
      If modelPerf.amod_number_of_engines > 0 Then
        amod_number_of_engines.Text = FormatNumber(modelPerf.amod_number_of_engines, 0, False, False, True)
      End If

      ' add engines to list box
      Dim engineString As String = commonEvo.GetEngines(modelPerf.amod_id, 0, True).Trim()

      ListBox_engines.Items.Clear()

      Dim EngineArray() As String = Split(engineString, ",")

      For Each eng As String In EngineArray
        ListBox_engines.Items.Add(New ListItem(eng, ""))
      Next

      If modelPerf.amod_engine_thrust_lbs > 0 Then
        amod_engine_thrust_lbs.Text = FormatNumber(modelPerf.amod_engine_thrust_lbs, 0, False, False, True)
      End If

      If modelPerf.amod_engine_shaft > 0 Then
        amod_engine_shaft.Text = FormatNumber(modelPerf.amod_engine_shaft, 0, False, False, True)
      End If

      If modelPerf.amod_engine_com_tbo_hrs > 0 Then
        amod_engine_com_tbo_hrs.Text = FormatNumber(modelPerf.amod_engine_com_tbo_hrs, 0, False, False, True)
      End If

      ' ROTORS
      If modelPerf.amod_main_rotor_1_blade_count > 0 Then
        amod_main_rotor_1_blade_count.Text = FormatNumber(modelPerf.amod_main_rotor_1_blade_count, 0, False, False, True)
      End If

      If modelPerf.amod_main_rotor_1_blade_diameter > 0 Then
        amod_main_rotor_1_blade_diameter.Text = FormatNumber(modelPerf.amod_main_rotor_1_blade_diameter, 0, False, False, True)
      End If

      If modelPerf.amod_main_rotor_2_blade_count > 0 Then
        amod_main_rotor_2_blade_count.Text = FormatNumber(modelPerf.amod_main_rotor_2_blade_count, 0, False, False, True)
      End If

      If modelPerf.amod_main_rotor_2_blade_diameter > 0 Then
        amod_main_rotor_2_blade_diameter.Text = FormatNumber(modelPerf.amod_main_rotor_2_blade_diameter, 0, False, False, True)
      End If

      If modelPerf.amod_tail_rotor_blade_count > 0 Then
        amod_tail_rotor_blade_count.Text = FormatNumber(modelPerf.amod_tail_rotor_blade_count, 0, False, False, True)
      End If

      If modelPerf.amod_tail_rotor_blade_diameter > 0 Then
        amod_tail_rotor_blade_diameter.Text = FormatNumber(modelPerf.amod_tail_rotor_blade_diameter, 0, False, False, True)
      End If

      If Not String.IsNullOrEmpty(modelPerf.amod_rotor_anti_torque_system.Trim) Then
        amod_rotor_anti_torque_system.Text = modelPerf.amod_rotor_anti_torque_system.Trim
      End If


      '' Operational Costs 
      Dim modelCost As New homebaseModelOpCostsClass(inAmodID)

      modelCost.fillModelOpCostsClass()

      '' DIRECT COSTS/HOUR
      '' FUEL

      If modelCost.amod_fuel_tot_cost > 0 Then
        amod_fuel_tot_cost.Text = FormatNumber(modelCost.amod_fuel_tot_cost, 2, False, False, True)
      End If

      If modelCost.amod_fuel_gal_cost > 0 Then
        amod_fuel_gal_cost.Text = FormatNumber(modelCost.amod_fuel_gal_cost, 2, False, False, True)
      End If

      If modelCost.amod_fuel_add_cost > 0 Then
        amod_fuel_add_cost.Text = FormatNumber(modelCost.amod_fuel_add_cost, 2, False, False, True)
      End If

      If modelCost.amod_fuel_burn_rate > 0 Then
        amod_fuel_burn_rate.Text = FormatNumber(modelCost.amod_fuel_burn_rate, 2, False, False, True)
      End If

      ' MAINTENANCE
      If modelCost.amod_maint_tot_cost > 0 Then
        amod_maint_tot_cost.Text = FormatNumber(modelCost.amod_maint_tot_cost, 2, False, False, True)
      End If

      If modelCost.amod_maint_lab_cost > 0 Then
        amod_maint_lab_cost.Text = FormatNumber(modelCost.amod_maint_lab_cost, 2, False, False, True)
      End If

      If modelCost.amod_maint_labor_cost_man_hours_multiplier > 0 Then
        amod_maint_labor_cost_man_hours_multiplier.Text = FormatNumber(modelCost.amod_maint_labor_cost_man_hours_multiplier, 2, True, False, True)
      End If

      If modelCost.amod_maint_parts_cost > 0 Then
        amod_maint_parts_cost.Text = FormatNumber(modelCost.amod_maint_parts_cost, 2, False, False, True)
      End If

      If modelCost.amod_maint_parts_cost_man_hours_multiplier > 0 Then
        amod_maint_parts_cost_man_hours_multiplier.Text = FormatNumber(modelCost.amod_maint_parts_cost_man_hours_multiplier, 2, True, False, True)
      End If

      If modelCost.amod_engine_ovh_cost > 0 Then
        amod_engine_ovh_cost.Text = FormatNumber(modelCost.amod_engine_ovh_cost, 2, False, False, True)
      End If

      If modelCost.amod_thrust_rev_ovh_cost > 0 Then
        amod_thrust_rev_ovh_cost.Text = FormatNumber(modelCost.amod_thrust_rev_ovh_cost, 2, False, False, True)
      End If

      ' MISC. FLIGHT EXP.
      If modelCost.amod_misc_flight_cost > 0 Then
        amod_misc_flight_cost.Text = FormatNumber(modelCost.amod_misc_flight_cost, 2, False, False, True)
      End If

      If modelCost.amod_land_park_cost > 0 Then
        amod_land_park_cost.Text = FormatNumber(modelCost.amod_land_park_cost, 2, False, False, True)
      End If

      If modelCost.amod_crew_exp_cost > 0 Then
        amod_crew_exp_cost.Text = FormatNumber(modelCost.amod_crew_exp_cost, 2, False, False, True)
      End If

      If modelCost.amod_supplies_cost > 0 Then
        amod_supplies_cost.Text = FormatNumber(modelCost.amod_supplies_cost, 2, False, False, True)
      End If

      ' TOTAL DIRECT COSTS
      If modelCost.amod_tot_hour_direct_cost > 0 Then
        amod_tot_hour_direct_cost.Text = FormatNumber(modelCost.amod_tot_hour_direct_cost, 2, False, False, True)
      End If

      If modelCost.amod_avg_block_speed > 0 Then
        amod_avg_block_speed.Text = FormatNumber(modelCost.amod_avg_block_speed, 2, False, False, True)
      End If

      If modelCost.amod_tot_stat_mile_cost > 0 Then
        amod_tot_stat_mile_cost.Text = FormatNumber(modelCost.amod_tot_stat_mile_cost, 2, False, False, True)
      End If

      ' ANNUAL FIXED COSTS
      ' CREW SALARIES
      If modelCost.amod_tot_crew_salary_cost > 0 Then
        amod_tot_crew_salary_cost.Text = FormatNumber(modelCost.amod_tot_crew_salary_cost, 2, False, False, True)
      End If

      If modelCost.amod_capt_salary_cost > 0 Then
        amod_capt_salary_cost.Text = FormatNumber(modelCost.amod_capt_salary_cost, 2, False, False, True)
      End If

      If modelCost.amod_cpilot_salary_cost > 0 Then
        amod_cpilot_salary_cost.Text = FormatNumber(modelCost.amod_cpilot_salary_cost, 2, False, False, True)
      End If

      If modelCost.amod_crew_benefit_cost > 0 Then
        amod_crew_benefit_cost.Text = FormatNumber(modelCost.amod_crew_benefit_cost, 2, False, False, True)
      End If

      If modelCost.amod_hangar_cost > 0 Then
        amod_hangar_cost.Text = FormatNumber(modelCost.amod_hangar_cost, 2, False, False, True)
      End If

      ' INSURANCE
      If modelCost.amod_insurance_cost > 0 Then
        amod_insurance_cost.Text = FormatNumber(modelCost.amod_insurance_cost, 2, False, False, True)
      End If

      If modelCost.amod_hull_insurance_cost > 0 Then
        amod_hull_insurance_cost.Text = FormatNumber(modelCost.amod_hull_insurance_cost, 2, False, False, True)
      End If

      If modelCost.amod_liability_insurance_cost > 0 Then
        amod_liability_insurance_cost.Text = FormatNumber(modelCost.amod_liability_insurance_cost, 2, False, False, True)
      End If

      ' MISC. OVERHEAD
      If modelCost.amod_tot_misc_ovh_cost > 0 Then
        amod_tot_misc_ovh_cost.Text = FormatNumber(modelCost.amod_tot_misc_ovh_cost, 2, False, False, True)
      End If

      If modelCost.amod_misc_train_cost > 0 Then
        amod_misc_train_cost.Text = FormatNumber(modelCost.amod_misc_train_cost, 2, False, False, True)
      End If

      If modelCost.amod_misc_modern_cost > 0 Then
        amod_misc_modern_cost.Text = FormatNumber(modelCost.amod_misc_modern_cost, 2, False, False, True)
      End If

      If modelCost.amod_misc_naveq_cost > 0 Then
        amod_misc_naveq_cost.Text = FormatNumber(modelCost.amod_misc_naveq_cost, 2, False, False, True)
      End If

      If modelCost.amod_deprec_cost > 0 Then
        amod_deprec_cost.Text = FormatNumber(modelCost.amod_deprec_cost, 2, False, False, True)
      End If

      ' TOTAL FIXED COSTS
      If modelCost.amod_tot_fixed_cost > 0 Then
        amod_tot_fixed_cost.Text = FormatNumber(modelCost.amod_tot_fixed_cost, 2, False, False, True)
      End If

      ' TOTAL VARIABLE COSTS
      If modelCost.amod_variable_costs > 0 Then
        amod_variable_costs.Text = FormatNumber(modelCost.amod_variable_costs, 2, False, False, True)
      End If

      ' ANNUAL BUDGET
      If modelCost.amod_number_of_seats > 0 Then
        amod_number_of_seats.Text = FormatNumber(modelCost.amod_number_of_seats, 0, False, False, True)
      End If

      If modelCost.amod_annual_miles > 0 Then
        amod_annual_miles.Text = FormatNumber(modelCost.amod_annual_miles, 0, False, False, True)
      End If

      If modelCost.amod_annual_hours > 0 Then
        amod_annual_hours.Text = FormatNumber(modelCost.amod_annual_hours, 0, False, False, True)
      End If

      ' TOTAL DIRECT COSTS
      If modelCost.amod_tot_direct_cost > 0 Then
        amod_tot_direct_cost.Text = FormatNumber(modelCost.amod_tot_direct_cost, 2, False, False, True)
      End If

      ' TOTAL FIXED COSTS
      If modelCost.amod_tot_fixed_cost > 0 Then
        amod_tot_fixed_cost2.Text = FormatNumber(modelCost.amod_tot_fixed_cost, 2, False, False, True)
      End If

      ' TOTAL FIXED + DIRECT COSTS
      If modelCost.amod_tot_df_annual_cost > 0 Then
        amod_tot_df_annual_cost.Text = FormatNumber(modelCost.amod_tot_df_annual_cost, 2, False, False, True)
      End If

      If modelCost.amod_tot_df_hour_cost > 0 Then
        amod_tot_df_hour_cost.Text = FormatNumber(modelCost.amod_tot_df_hour_cost, 2, False, False, True)
      End If

      If modelCost.amod_tot_df_statmile_cost > 0 Then
        amod_tot_df_statmile_cost.Text = FormatNumber(modelCost.amod_tot_df_statmile_cost, 2, False, False, True)
      End If

      If modelCost.amod_tot_df_seat_cost > 0 Then
        amod_tot_df_seat_cost.Text = FormatNumber(modelCost.amod_tot_df_seat_cost, 2, True, False, True)
      End If

      ' TOTAL COSTS no depreciation
      If modelCost.amod_tot_nd_annual_cost > 0 Then
        amod_tot_nd_annual_cost.Text = FormatNumber(modelCost.amod_tot_nd_annual_cost, 2, False, False, True)
      End If

      If modelCost.amod_tot_nd_hour_cost > 0 Then
        amod_tot_nd_hour_cost.Text = FormatNumber(modelCost.amod_tot_nd_hour_cost, 2, False, False, True)
      End If

      If modelCost.amod_tot_nd_statmile_cost > 0 Then
        amod_tot_nd_statmile_cost.Text = FormatNumber(modelCost.amod_tot_nd_statmile_cost, 2, False, False, True)
      End If

      If modelCost.amod_tot_nd_seat_cost > 0 Then
        amod_tot_nd_seat_cost.Text = FormatNumber(modelCost.amod_tot_nd_seat_cost, 2, True, False, True)
      End If

      ' fill attributes
      Dim attributeString = generateModelAttributesSummaryTable()

      If Not String.IsNullOrEmpty(attributeString.Trim) Then
        attributesLabel.Text = attributeString
      Else
        attributesLabel.Text = "No Attributes Found"
      End If

      ' fill features
      Dim featuresString = generateModelFeaturesSummaryTable()

      If Not String.IsNullOrEmpty(featuresString.Trim) Then
        featuresLabel.Text = featuresString
      Else
        featuresLabel.Text = "No Features Found"
      End If

    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />homebaseEditAircraftModel</b><br />" + ex.Message
    End Try

  End Sub

  Private Function saveModelChanges() As Boolean


    Dim modelInfo As New homebaseModelInfoClass(inAmodID)
    Dim modelPerf As New homebaseModelPerfSpecsClass(inAmodID)
    Dim modelCost As New homebaseModelOpCostsClass(inAmodID)

    ' fill in current model objects
    modelInfo.fillModelInfoClass()
    modelPerf.fillModelPerfSpecsClass()
    modelCost.fillModelOpCostsClass()

    Dim modelInfoChange As New homebaseModelInfoClass(inAmodID)
    Dim modelPerfChange As New homebaseModelPerfSpecsClass(inAmodID)
    Dim modelCostChange As New homebaseModelOpCostsClass(inAmodID)

    ' fill in change model info class so static items will not cause class not to be equal
    modelInfoChange.fillModelInfoClass()

    ' fill in change model performance class so static items will not cause class not to be equal
    modelPerfChange.fillModelPerfSpecsClass()

    ' fill in change model op costs class so static items will not cause class not to be equal
    modelCostChange.fillModelOpCostsClass()

    ' model info
    modelInfoChange.amod_make_name = amod_make_name.Text.Trim
    modelInfoChange.amod_model_name = amod_model_name.Text.Trim
    modelInfoChange.amod_manufacturer = amod_manufacturer.Text.Trim

    modelInfoChange.amod_make_abbrev = amod_make_abbrev.Text.Trim
    modelInfoChange.amod_model_abbrev = amod_model_abbrev.Text.Trim
    modelInfoChange.amod_manufacturer_common_name = amod_manufacturer_common_name.Text.Trim

    modelInfoChange.amod_airframe_type_code = amod_airframe_type_code.SelectedValue

    modelInfoChange.amod_jniq_size = amod_jniq_size.SelectedValue

    modelInfoChange.amod_type_code = IIf(amod_type_code.SelectedValue.ToUpper.Contains("TP"), "T", amod_type_code.SelectedValue)

    modelInfoChange.amod_class_code = amod_class_code.Text.Trim

    modelInfoChange.amod_weight_class = amod_weight_class.SelectedValue

    modelInfoChange.amod_start_year = amod_start_year.Text.Trim
    modelInfoChange.amod_end_year = amod_end_year.Text.Trim
    modelInfoChange.amod_start_price = amod_start_price.Text.Replace(",", "").Trim
    modelInfoChange.amod_end_price = amod_end_price.Text.Replace(",", "").Trim

    modelInfoChange.amod_description = amod_description.Text.Trim

    modelInfoChange.amod_product_business_flag = amod_product_business_flag.Checked
    modelInfoChange.amod_product_commercial_flag = amod_product_commercial_flag.Checked
    modelInfoChange.amod_product_airbp_flag = amod_product_airbp_flag.Checked
    modelInfoChange.amod_product_helicopter_flag = amod_product_helicopter_flag.Checked

    'modelInfoChange.amod_product_abi_flag = amod_product_abi_flag.Checked
    'modelInfoChange.amod_product_regional_flag = amod_product_regional_flag.Checked

    modelInfoChange.amod_ser_no_prefix = amod_ser_no_prefix.Text.Trim
    modelInfoChange.amod_ser_no_start = amod_ser_no_start.Text.Trim
    modelInfoChange.amod_ser_no_end = amod_ser_no_end.Text.Trim
    modelInfoChange.amod_ser_no_suffix = amod_ser_no_suffix.Text.Trim

    modelInfoChange.amod_serno_hyphen_flag = amod_serno_hyphen_flag.Checked

    modelInfoChange.amod_body_config = amod_body_config.SelectedValue

    ' performance
    '' FUSELAGE DIMENSIONS
    If Not String.IsNullOrEmpty(amod_fuselage_length.Text.Trim) Then
      If IsNumeric(amod_fuselage_length.Text) Then
        modelPerfChange.amod_fuselage_length = CDec(amod_fuselage_length.Text)
      End If
    End If

    If Not String.IsNullOrEmpty(amod_fuselage_height.Text.Trim) Then
      If IsNumeric(amod_fuselage_height.Text) Then
        modelPerfChange.amod_fuselage_height = CDec(amod_fuselage_height.Text)
      End If
    End If

    If modelInfo.amod_airframe_type_code.ToUpper.Contains("F") Then

      If Not String.IsNullOrEmpty(amod_fuselage_wingspan.Text.Trim) Then
        If IsNumeric(amod_fuselage_wingspan.Text) Then
          modelPerfChange.amod_fuselage_wingspan = CDec(amod_fuselage_wingspan.Text)
        End If
      End If

    Else

      If Not String.IsNullOrEmpty(amod_fuselage_width.Text.Trim) Then
        If IsNumeric(amod_fuselage_width.Text) Then
          modelPerfChange.amod_fuselage_width = CDec(amod_fuselage_width.Text)
        End If
      End If

    End If

    '' TYPICAL CONFIGURATION
    If Not String.IsNullOrEmpty(amod_number_of_crew.Text.Trim) Then
      If IsNumeric(amod_number_of_crew.Text) Then
        modelPerfChange.amod_number_of_crew = CDec(amod_number_of_crew.Text)
      End If
    End If

    If Not String.IsNullOrEmpty(amod_number_of_passengers.Text.Trim) Then
      If IsNumeric(amod_number_of_passengers.Text) Then
        modelPerfChange.amod_number_of_passengers = CDec(amod_number_of_passengers.Text)
      End If
    End If

    If modelInfo.amod_airframe_type_code.ToUpper.Contains("F") Then

      If Not String.IsNullOrEmpty(amod_pressure.Text.Trim) Then
        If IsNumeric(amod_pressure.Text) Then
          modelPerfChange.amod_pressure = CDec(amod_pressure.Text)
        End If
      End If

    End If

    If Not String.IsNullOrEmpty(amod_max_ramp_weight.Text.Trim) Then
      If IsNumeric(amod_max_ramp_weight.Text) Then
        modelPerfChange.amod_max_ramp_weight = CDec(amod_max_ramp_weight.Text)
      End If
    End If

    If Not String.IsNullOrEmpty(amod_max_takeoff_weight.Text.Trim) Then
      If IsNumeric(amod_max_takeoff_weight.Text) Then
        modelPerfChange.amod_max_takeoff_weight = CDec(amod_max_takeoff_weight.Text)
      End If
    End If

    If modelInfo.amod_airframe_type_code.ToUpper.Contains("F") Then

      If Not String.IsNullOrEmpty(amod_zero_fuel_weight.Text.Trim) Then
        If IsNumeric(amod_zero_fuel_weight.Text) Then
          modelPerfChange.amod_zero_fuel_weight = CDec(amod_zero_fuel_weight.Text)
        End If
      End If

    End If

    If Not String.IsNullOrEmpty(amod_weight_eow.Text.Trim) Then
      If IsNumeric(amod_weight_eow.Text) Then
        modelPerfChange.amod_weight_eow = CDec(amod_weight_eow.Text)
      End If
    End If

    If Not String.IsNullOrEmpty(amod_basic_op_weight.Text.Trim) Then
      If IsNumeric(amod_basic_op_weight.Text) Then
        modelPerfChange.amod_basic_op_weight = CDec(amod_basic_op_weight.Text)
      End If
    End If

    If Not String.IsNullOrEmpty(amod_max_landing_weight.Text.Trim) Then
      If IsNumeric(amod_max_landing_weight.Text) Then
        modelPerfChange.amod_max_landing_weight = CDec(amod_max_landing_weight.Text)
      End If
    End If

    ' IFR Certification
    If Not String.IsNullOrEmpty(amod_ifr_certification.Text.Trim) Then
      modelPerfChange.amod_ifr_certification = amod_ifr_certification.Text.Trim
    End If

    ' CLIMB
    If Not String.IsNullOrEmpty(amod_climb_normal_feet.Text.Trim) Then
      If IsNumeric(amod_climb_normal_feet.Text) Then
        modelPerfChange.amod_climb_normal_feet = CDec(amod_climb_normal_feet.Text)
      End If
    End If

    If Not String.IsNullOrEmpty(amod_climb_engout_feet.Text.Trim) Then
      If IsNumeric(amod_climb_engout_feet.Text) Then
        modelPerfChange.amod_climb_engout_feet = CDec(amod_climb_engout_feet.Text)
      End If
    End If

    If Not String.IsNullOrEmpty(amod_ceiling_feet.Text.Trim) Then
      If IsNumeric(amod_ceiling_feet.Text) Then
        modelPerfChange.amod_ceiling_feet = CDec(amod_ceiling_feet.Text)
      End If
    End If

    If modelInfo.amod_airframe_type_code.ToUpper.Contains("R") Then

      If Not String.IsNullOrEmpty(amod_climb_hoge.Text.Trim) Then
        If IsNumeric(amod_climb_hoge.Text) Then
          modelPerfChange.amod_climb_hoge = CDec(amod_climb_hoge.Text)
        End If
      End If

      If Not String.IsNullOrEmpty(amod_climb_hige.Text.Trim) Then
        If IsNumeric(amod_climb_hige.Text) Then
          modelPerfChange.amod_climb_hige = CDec(amod_climb_hige.Text)
        End If
      End If

    End If

    ' RANGE
    If Not String.IsNullOrEmpty(amod_max_range_miles.Text.Trim) Then
      If IsNumeric(amod_max_range_miles.Text) Then
        modelPerfChange.amod_max_range_miles = CDec(amod_max_range_miles.Text)
      End If
    End If

    If Not String.IsNullOrEmpty(amod_range_tanks_full.Text.Trim) Then
      If IsNumeric(amod_range_tanks_full.Text) Then
        modelPerfChange.amod_range_tanks_full = CDec(amod_range_tanks_full.Text)
      End If
    End If

    If Not String.IsNullOrEmpty(amod_range_seats_full.Text.Trim) Then
      If IsNumeric(amod_range_seats_full.Text) Then
        modelPerfChange.amod_range_seats_full = CDec(amod_range_seats_full.Text)
      End If
    End If

    If Not String.IsNullOrEmpty(amod_range_4_passenger.Text.Trim) Then
      If IsNumeric(amod_range_4_passenger.Text) Then
        modelPerfChange.amod_range_4_passenger = CDec(amod_range_4_passenger.Text)
      End If
    End If

    If Not String.IsNullOrEmpty(amod_range_8_passenger.Text.Trim) Then
      If IsNumeric(amod_range_8_passenger.Text) Then
        modelPerfChange.amod_range_8_passenger = CDec(amod_range_8_passenger.Text)
      End If
    End If

    ' PROPELLERS
    If Not String.IsNullOrEmpty(amod_number_of_props.Text.Trim) Then
      If IsNumeric(amod_number_of_props.Text) Then
        modelPerfChange.amod_number_of_props = CInt(amod_number_of_props.Text)
      End If
    End If

    If Not String.IsNullOrEmpty(amod_prop_model_name.Text.Trim) Then
      modelPerfChange.amod_prop_model_name = amod_prop_model_name.Text.Trim
    End If

    If Not String.IsNullOrEmpty(amod_prop_mfr_name.Text.Trim) Then
      modelPerfChange.amod_prop_mfr_name = amod_prop_mfr_name.Text.Trim
    End If

    If Not String.IsNullOrEmpty(amod_prop_com_tbo_hrs.Text.Trim) Then
      If IsNumeric(amod_prop_com_tbo_hrs.Text) Then
        modelPerfChange.amod_prop_com_tbo_hrs = CDec(amod_prop_com_tbo_hrs.Text)
      End If
    End If

    ' CONFIG NOTE
    If Not String.IsNullOrEmpty(amod_other_config_note.Text.Trim) Then
      modelPerfChange.amod_other_config_note = amod_other_config_note.Text.Trim
    End If

    ' CABIN DIMENSIONS
    If Not String.IsNullOrEmpty(amod_cabinsize_height_feet.Text.Trim) Then
      If IsNumeric(amod_cabinsize_height_feet.Text) Then
        modelPerfChange.amod_cabinsize_height_feet = CInt(amod_cabinsize_height_feet.Text)
      End If
    End If

    If Not String.IsNullOrEmpty(amod_cabinsize_height_inches.Text.Trim) Then
      If IsNumeric(amod_cabinsize_height_inches.Text) Then
        modelPerfChange.amod_cabinsize_height_inches = CInt(amod_cabinsize_height_inches.Text)
      End If
    End If

    If Not String.IsNullOrEmpty(amod_cabinsize_width_feet.Text.Trim) Then
      If IsNumeric(amod_cabinsize_width_feet.Text) Then
        modelPerfChange.amod_cabinsize_width_feet = CInt(amod_cabinsize_width_feet.Text)
      End If
    End If

    If Not String.IsNullOrEmpty(amod_cabinsize_width_inches.Text.Trim) Then
      If IsNumeric(amod_cabinsize_width_inches.Text) Then
        modelPerfChange.amod_cabinsize_width_inches = CInt(amod_cabinsize_width_inches.Text)
      End If
    End If

    If Not String.IsNullOrEmpty(amod_cabinsize_length_feet.Text.Trim) Then
      If IsNumeric(amod_cabinsize_length_feet.Text) Then
        modelPerfChange.amod_cabinsize_length_feet = CInt(amod_cabinsize_length_feet.Text)
      End If
    End If

    If Not String.IsNullOrEmpty(amod_cabinsize_length_inches.Text.Trim) Then
      If IsNumeric(amod_cabinsize_length_inches.Text) Then
        modelPerfChange.amod_cabinsize_length_inches = CInt(amod_cabinsize_length_inches.Text)
      End If
    End If

    If Not String.IsNullOrEmpty(amod_cabin_volume.Text.Trim) Then
      If IsNumeric(amod_cabin_volume.Text) Then
        modelPerfChange.amod_cabin_volume = CDec(amod_cabin_volume.Text)
      End If
    End If

    If Not String.IsNullOrEmpty(amod_baggage_volume.Text.Trim) Then
      If IsNumeric(amod_baggage_volume.Text) Then
        modelPerfChange.amod_baggage_volume = CDec(amod_baggage_volume.Text)
      End If
    End If

    ' FUEL CAPACITY
    If Not String.IsNullOrEmpty(amod_fuel_cap_std_weight.Text.Trim) Then
      If IsNumeric(amod_fuel_cap_std_weight.Text) Then
        modelPerfChange.amod_fuel_cap_std_weight = CDec(amod_fuel_cap_std_weight.Text)
      End If
    End If

    If Not String.IsNullOrEmpty(amod_fuel_cap_std_gal.Text.Trim) Then
      If IsNumeric(amod_fuel_cap_std_gal.Text) Then
        modelPerfChange.amod_fuel_cap_std_gal = CDec(amod_fuel_cap_std_gal.Text)
      End If
    End If

    If Not String.IsNullOrEmpty(amod_fuel_cap_opt_weight.Text.Trim) Then
      If IsNumeric(amod_fuel_cap_opt_weight.Text) Then
        modelPerfChange.amod_fuel_cap_opt_weight = CDec(amod_fuel_cap_opt_weight.Text)
      End If
    End If

    If Not String.IsNullOrEmpty(amod_fuel_cap_opt_gal.Text.Trim) Then
      If IsNumeric(amod_fuel_cap_opt_gal.Text) Then
        modelPerfChange.amod_fuel_cap_opt_gal = CDec(amod_fuel_cap_opt_gal.Text)
      End If
    End If

    ' SPEED
    If modelInfo.amod_airframe_type_code.ToUpper.Contains("F") Then

      If Not String.IsNullOrEmpty(amod_stall_vs.Text.Trim) Then
        If IsNumeric(amod_stall_vs.Text) Then
          modelPerfChange.amod_stall_vs = CDec(amod_stall_vs.Text)
        End If
      End If

      If Not String.IsNullOrEmpty(amod_stall_vso.Text.Trim) Then
        If IsNumeric(amod_stall_vso.Text) Then
          modelPerfChange.amod_stall_vso = CDec(amod_stall_vso.Text)
        End If
      End If

    End If

    If Not String.IsNullOrEmpty(amod_cruis_speed.Text.Trim) Then
      If IsNumeric(amod_cruis_speed.Text) Then
        modelPerfChange.amod_cruis_speed = CDec(amod_cruis_speed.Text)
      End If
    End If

    If Not String.IsNullOrEmpty(amod_max_speed.Text.Trim) Then
      If IsNumeric(amod_max_speed.Text) Then
        modelPerfChange.amod_max_speed = CDec(amod_max_speed.Text)
      End If
    End If

    If modelInfo.amod_airframe_type_code.ToUpper.Contains("R") Then

      If Not String.IsNullOrEmpty(amod_vne_maxop_speed.Text.Trim) Then
        If IsNumeric(amod_vne_maxop_speed.Text) Then
          modelPerfChange.amod_vne_maxop_speed = CDec(amod_vne_maxop_speed.Text)
        End If
      End If

    End If

    If Not String.IsNullOrEmpty(amod_v1_takeoff_speed.Text.Trim) Then
      If IsNumeric(amod_v1_takeoff_speed.Text) Then
        modelPerfChange.amod_v1_takeoff_speed = CDec(amod_v1_takeoff_speed.Text)
      End If
    End If

    If Not String.IsNullOrEmpty(amod_vfe_max_flap_extended_speed.Text.Trim) Then
      If IsNumeric(amod_vfe_max_flap_extended_speed.Text) Then
        modelPerfChange.amod_vfe_max_flap_extended_speed = CDec(amod_vfe_max_flap_extended_speed.Text)
      End If
    End If

    If Not String.IsNullOrEmpty(amod_vle_max_landing_gear_ext_speed.Text.Trim) Then
      If IsNumeric(amod_vle_max_landing_gear_ext_speed.Text) Then
        modelPerfChange.amod_vle_max_landing_gear_ext_speed = CDec(amod_vle_max_landing_gear_ext_speed.Text)
      End If
    End If

    ' LANDING PERFORMANCE
    If modelInfo.amod_airframe_type_code.ToUpper.Contains("F") Then

      If Not String.IsNullOrEmpty(amod_field_length.Text.Trim) Then
        If IsNumeric(amod_field_length.Text) Then
          modelPerfChange.amod_field_length = CDec(amod_field_length.Text)
        End If
      End If

    End If

    ' TAKEOFF PERFORMANCE
    If Not String.IsNullOrEmpty(amod_takeoff_ali.Text.Trim) Then
      If IsNumeric(amod_takeoff_ali.Text) Then
        modelPerfChange.amod_takeoff_ali = CDec(amod_takeoff_ali.Text)
      End If
    End If

    If Not String.IsNullOrEmpty(amod_takeoff_500.Text.Trim) Then
      If IsNumeric(amod_takeoff_500.Text) Then
        modelPerfChange.amod_takeoff_500 = CDec(amod_takeoff_500.Text)
      End If
    End If

    ' ENGINES
    If Not String.IsNullOrEmpty(amod_number_of_engines.Text.Trim) Then
      If IsNumeric(amod_number_of_engines.Text) Then
        modelPerfChange.amod_number_of_engines = CInt(amod_number_of_engines.Text)
      End If
    End If

    If Not String.IsNullOrEmpty(amod_engine_thrust_lbs.Text.Trim) Then
      If IsNumeric(amod_engine_thrust_lbs.Text) Then
        modelPerfChange.amod_engine_thrust_lbs = CDec(amod_engine_thrust_lbs.Text)
      End If
    End If

    If Not String.IsNullOrEmpty(amod_engine_shaft.Text.Trim) Then
      If IsNumeric(amod_engine_shaft.Text) Then
        modelPerfChange.amod_engine_shaft = CDec(amod_engine_shaft.Text)
      End If
    End If

    If Not String.IsNullOrEmpty(amod_engine_com_tbo_hrs.Text.Trim) Then
      If IsNumeric(amod_engine_com_tbo_hrs.Text) Then
        modelPerfChange.amod_engine_com_tbo_hrs = CDec(amod_engine_com_tbo_hrs.Text)
      End If
    End If

    ' ROTORS
    If Not String.IsNullOrEmpty(amod_main_rotor_1_blade_count.Text.Trim) Then
      If IsNumeric(amod_main_rotor_1_blade_count.Text) Then
        modelPerfChange.amod_main_rotor_1_blade_count = CInt(amod_main_rotor_1_blade_count.Text)
      End If
    End If

    If Not String.IsNullOrEmpty(amod_main_rotor_1_blade_diameter.Text.Trim) Then
      If IsNumeric(amod_main_rotor_1_blade_diameter.Text) Then
        modelPerfChange.amod_main_rotor_1_blade_diameter = CDec(amod_main_rotor_1_blade_diameter.Text)
      End If
    End If

    If Not String.IsNullOrEmpty(amod_main_rotor_2_blade_count.Text.Trim) Then
      If IsNumeric(amod_main_rotor_2_blade_count.Text) Then
        modelPerfChange.amod_main_rotor_2_blade_count = CInt(amod_main_rotor_2_blade_count.Text)
      End If
    End If

    If Not String.IsNullOrEmpty(amod_main_rotor_2_blade_diameter.Text.Trim) Then
      If IsNumeric(amod_main_rotor_2_blade_diameter.Text) Then
        modelPerfChange.amod_main_rotor_2_blade_diameter = CDec(amod_main_rotor_2_blade_diameter.Text)
      End If
    End If

    If Not String.IsNullOrEmpty(amod_tail_rotor_blade_count.Text.Trim) Then
      If IsNumeric(amod_tail_rotor_blade_count.Text) Then
        modelPerfChange.amod_tail_rotor_blade_count = CInt(amod_tail_rotor_blade_count.Text)
      End If
    End If

    If Not String.IsNullOrEmpty(amod_tail_rotor_blade_diameter.Text.Trim) Then
      If IsNumeric(amod_tail_rotor_blade_diameter.Text) Then
        modelPerfChange.amod_tail_rotor_blade_diameter = CDec(amod_tail_rotor_blade_diameter.Text)
      End If
    End If

    If Not String.IsNullOrEmpty(amod_rotor_anti_torque_system.Text.Trim) Then
      modelPerfChange.amod_rotor_anti_torque_system = amod_rotor_anti_torque_system.Text.Trim
    End If


    ' fill in change model op costs class from the "page controls"
    '' FUEL
    If Not String.IsNullOrEmpty(amod_fuel_gal_cost.Text.Trim) Then
      If IsNumeric(amod_fuel_gal_cost.Text) Then
        modelCostChange.amod_fuel_gal_cost = CDbl(amod_fuel_gal_cost.Text)
      End If
    End If

    If Not String.IsNullOrEmpty(amod_fuel_add_cost.Text.Trim) Then
      If IsNumeric(amod_fuel_add_cost.Text) Then
        modelCostChange.amod_fuel_add_cost = CDbl(amod_fuel_add_cost.Text)
      End If
    End If

    If Not String.IsNullOrEmpty(amod_fuel_burn_rate.Text.Trim) Then
      If IsNumeric(amod_fuel_burn_rate.Text) Then
        modelCostChange.amod_fuel_burn_rate = CDbl(amod_fuel_burn_rate.Text)
      End If
    End If

    '' MAINTENANCE
    If Not String.IsNullOrEmpty(amod_maint_lab_cost.Text.Trim) Then
      If IsNumeric(amod_maint_lab_cost.Text) Then
        modelCostChange.amod_maint_lab_cost = CDbl(amod_maint_lab_cost.Text)
      End If
    End If

    If Not String.IsNullOrEmpty(amod_maint_labor_cost_man_hours_multiplier.Text.Trim) Then
      If IsNumeric(amod_maint_labor_cost_man_hours_multiplier.Text) Then
        modelCostChange.amod_maint_labor_cost_man_hours_multiplier = CDbl(amod_maint_labor_cost_man_hours_multiplier.Text)
      End If
    End If

    If Not String.IsNullOrEmpty(amod_maint_parts_cost.Text.Trim) Then
      If IsNumeric(amod_maint_parts_cost.Text) Then
        modelCostChange.amod_maint_parts_cost = CDbl(amod_maint_parts_cost.Text)
      End If
    End If

    If Not String.IsNullOrEmpty(amod_maint_parts_cost_man_hours_multiplier.Text.Trim) Then
      If IsNumeric(amod_maint_parts_cost_man_hours_multiplier.Text) Then
        modelCostChange.amod_maint_parts_cost_man_hours_multiplier = CDbl(amod_maint_parts_cost_man_hours_multiplier.Text)
      End If
    End If

    If Not String.IsNullOrEmpty(amod_engine_ovh_cost.Text.Trim) Then
      If IsNumeric(amod_engine_ovh_cost.Text) Then
        modelCostChange.amod_engine_ovh_cost = CDbl(amod_engine_ovh_cost.Text)
      End If
    End If

    If Not String.IsNullOrEmpty(amod_thrust_rev_ovh_cost.Text.Trim) Then
      If IsNumeric(amod_thrust_rev_ovh_cost.Text) Then
        modelCostChange.amod_thrust_rev_ovh_cost = CDbl(amod_thrust_rev_ovh_cost.Text)
      End If
    End If

    '' MISC. FLIGHT EXP.
    If Not String.IsNullOrEmpty(amod_land_park_cost.Text.Trim) Then
      If IsNumeric(amod_land_park_cost.Text) Then
        modelCostChange.amod_land_park_cost = CDbl(amod_land_park_cost.Text)
      End If
    End If

    If Not String.IsNullOrEmpty(amod_crew_exp_cost.Text.Trim) Then
      If IsNumeric(amod_crew_exp_cost.Text) Then
        modelCostChange.amod_crew_exp_cost = CDbl(amod_crew_exp_cost.Text)
      End If
    End If

    If Not String.IsNullOrEmpty(amod_supplies_cost.Text.Trim) Then
      If IsNumeric(amod_supplies_cost.Text) Then
        modelCostChange.amod_supplies_cost = CDbl(amod_supplies_cost.Text)
      End If
    End If

    '' TOTAL DIRECT COSTS
    If Not String.IsNullOrEmpty(amod_avg_block_speed.Text.Trim) Then
      If IsNumeric(amod_avg_block_speed.Text) Then
        modelCostChange.amod_avg_block_speed = CDbl(amod_avg_block_speed.Text)
      End If
    End If

    '' ANNUAL FIXED COSTS
    '' CREW SALARIES
    If Not String.IsNullOrEmpty(amod_capt_salary_cost.Text.Trim) Then
      If IsNumeric(amod_capt_salary_cost.Text) Then
        modelCostChange.amod_capt_salary_cost = CDbl(amod_capt_salary_cost.Text)
      End If
    End If

    If Not String.IsNullOrEmpty(amod_cpilot_salary_cost.Text.Trim) Then
      If IsNumeric(amod_cpilot_salary_cost.Text) Then
        modelCostChange.amod_cpilot_salary_cost = CDbl(amod_cpilot_salary_cost.Text)
      End If
    End If

    If Not String.IsNullOrEmpty(amod_crew_benefit_cost.Text.Trim) Then
      If IsNumeric(amod_crew_benefit_cost.Text) Then
        modelCostChange.amod_crew_benefit_cost = CDbl(amod_crew_benefit_cost.Text)
      End If
    End If

    If Not String.IsNullOrEmpty(amod_hangar_cost.Text.Trim) Then
      If IsNumeric(amod_hangar_cost.Text) Then
        modelCostChange.amod_hangar_cost = CDbl(amod_hangar_cost.Text)
      End If
    End If

    '' INSURANCE
    If Not String.IsNullOrEmpty(amod_hull_insurance_cost.Text.Trim) Then
      If IsNumeric(amod_hull_insurance_cost.Text) Then
        modelCostChange.amod_hull_insurance_cost = CDbl(amod_hull_insurance_cost.Text)
      End If
    End If

    If Not String.IsNullOrEmpty(amod_liability_insurance_cost.Text.Trim) Then
      If IsNumeric(amod_liability_insurance_cost.Text) Then
        modelCostChange.amod_liability_insurance_cost = CDbl(amod_liability_insurance_cost.Text)
      End If
    End If


    '' MISC. OVERHEAD
    If Not String.IsNullOrEmpty(amod_misc_train_cost.Text.Trim) Then
      If IsNumeric(amod_misc_train_cost.Text) Then
        modelCostChange.amod_misc_train_cost = CDbl(amod_misc_train_cost.Text)
      End If
    End If

    If Not String.IsNullOrEmpty(amod_misc_modern_cost.Text.Trim) Then
      If IsNumeric(amod_misc_modern_cost.Text) Then
        modelCostChange.amod_misc_modern_cost = CDbl(amod_misc_modern_cost.Text)
      End If
    End If

    If Not String.IsNullOrEmpty(amod_misc_naveq_cost.Text.Trim) Then
      If IsNumeric(amod_misc_naveq_cost.Text) Then
        modelCostChange.amod_misc_naveq_cost = CDbl(amod_misc_naveq_cost.Text)
      End If
    End If

    If Not String.IsNullOrEmpty(amod_deprec_cost.Text.Trim) Then
      If IsNumeric(amod_deprec_cost.Text) Then
        modelCostChange.amod_deprec_cost = CDbl(amod_deprec_cost.Text)
      End If
    End If

    '' ANNUAL BUDGET
    If Not String.IsNullOrEmpty(amod_annual_miles.Text.Trim) Then
      If IsNumeric(amod_annual_miles.Text) Then
        modelCostChange.amod_annual_miles = CDbl(amod_annual_miles.Text)
      End If
    End If

    If Not String.IsNullOrEmpty(amod_annual_hours.Text.Trim) Then
      If IsNumeric(amod_annual_hours.Text) Then
        modelCostChange.amod_annual_hours = CDbl(amod_annual_hours.Text)
      End If
    End If

    If (modelInfoChange <> modelInfo) And Not bAddNewModel Then
      modelInfoChange.updateModelInfoClass()
    ElseIf bAddNewModel Then
      modelInfoChange.insertModelInfoClass()
    End If

    If (modelPerfChange <> modelPerf) And Not bAddNewModel Then
      modelPerfChange.updateModelPerfSpecsClass()
    ElseIf bAddNewModel Then
      modelPerfChange.insertModelPerfSpecsClass()
    End If

    If (modelCostChange <> modelCost) And Not bAddNewModel Then
      modelCostChange.updateModelOpCostsClass()
    ElseIf bAddNewModel Then
      modelCostChange.insertModelOpCostsClass()
    End If

    displayModel()

  End Function

  Private Sub saveModel_Click(sender As Object, e As EventArgs) Handles saveModel0.Click, saveModel1.Click, saveModel2.Click

    saveModelChanges()

  End Sub

  Private Sub fillModelsDropdown()

    'Filling up Model Dropdownlist if the count is 0
    If modelList.Items.Count = 0 Then
      Dim TempTable As New DataTable

      TempTable = commonEvo.Get_MakesModels_ByProductCode(False)

      Dim ModelTableView As New DataView
      Dim ModelTableFinal As New DataTable

      If Not IsNothing(TempTable) Then

        ModelTableView = TempTable.DefaultView
        ModelTableView.Sort = "atype_name, amod_make_name, amod_model_name"

        ModelTableFinal = ModelTableView.ToTable()

        modelList.Items.Insert(0, New ListItem("", ""))

        For Each r As DataRow In ModelTableFinal.Rows
          If Not IsDBNull(r("amod_model_name")) And Not IsDBNull(r("amod_make_name")) Then
            If Not IsDBNull(r("amod_id")) Then
              Dim NewItem As New ListItem(r("amod_make_name").ToString & " " & r("amod_model_name").ToString, r("amod_id"))
              NewItem.Attributes("OptionGroup") = r("atype_name")
              modelList.Items.Add(NewItem)
            End If
          End If
        Next

        TempTable.Dispose()

      End If

    End If

    'Let's select a default if needed:
    If inAmodID > 0 Then
      modelList.SelectedValue = inAmodID.ToString
    Else
      If Session.Item("localPreferences").DefaultModel > 0 Then
        modelList.SelectedValue = Session.Item("localPreferences").DefaultModel
      Else
        If Session.Item("localPreferences").UserBusinessFlag = True Then
          If Session.Item("localPreferences").Tierlevel = eTierLevelTypes.TURBOS Then
            modelList.SelectedValue = 207 '- king air b200 
          Else 'Jets or ALL
            modelList.SelectedValue = 272   ' challenger 300 - business jet
          End If
        ElseIf Session.Item("localPreferences").UserCommercialFlag = True Then
          modelList.SelectedValue = 698 ' boeng bbj -  commercial jet 
        ElseIf Session.Item("localPreferences").UserHelicopterFlag = True Then
          modelList.SelectedValue = 408 ' augusta westland aw139 - helicopter 
        End If
      End If

      If Not IsNothing(modelList) Then
        If Not String.IsNullOrEmpty(modelList.SelectedValue.Trim) Then
          inAmodID = CLng(modelList.SelectedValue)
        End If
      End If

    End If

  End Sub

  Public Function getModelAttributesDataTable() As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT acatt_name AS ATTRIBUTE, acatt_abbrev AS ABBREV,")
      sQuery.Append(" CASE WHEN attmod_value > 0 THEN attmod_value ELSE acatt_average_value END AS VALUE")
      sQuery.Append(" FROM " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Aircraft_Attribute WITH(NOLOCK)")
      sQuery.Append(" INNER JOIN " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Aircraft_Attribute_Model WITH(NOLOCK) ON acatt_id = attmod_att_id")
      sQuery.Append(" WHERE attmod_amod_id = @amodID")
      sQuery.Append(" GROUP BY acatt_name, acatt_abbrev, CASE WHEN attmod_value > 0 THEN attmod_value ELSE acatt_average_value END, attmod_seq_no, acatt_id")
      sQuery.Append(" ORDER BY attmod_seq_no")

      SqlCommand.Parameters.AddWithValue("@amodID", inAmodID.ToString)

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>getUserSummaryDatesDataTable() As DataTable</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 240

      SqlCommand.CommandText = sQuery.ToString

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getModelAttributesDataTable load datatable</b><br /> " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getModelAttributesDataTable() As DataTable</b><br />" + ex.Message

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

  Public Function generateModelAttributesSummaryTable() As String

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder

    Try

      results_table = getModelAttributesDataTable()

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          editAttributesBtn.Visible = True
          editAttributesBtn.Attributes.Remove("onclick")
          editAttributesBtn.Attributes.Add("onclick", "openSmallWindowJS('home_Model.aspx?modelID=" + inAmodID.ToString + "','Model Attributes');return true;")

          htmlOut.Append("<table border=""1"" cellpadding=""2"" cellspacing=""0"" width=""100%"">")

          ' second generate the header based off the column names in the datatable
          htmlOut.Append("<tr bgcolor=""#CCCCCC"">")
          For Each c As DataColumn In results_table.Columns

            htmlOut.Append("<td align=""left"" valign=""middle"">" + c.ColumnName.ToUpper.Replace("CCOUNT", "COUNT").Trim + "</td>")

          Next
          htmlOut.Append("</tr>")

          ' second display the report data based off the column names in the datatable
          For Each r As DataRow In results_table.Rows

            htmlOut.Append("<tr>")

            ' ramble through each "column name" and display data
            For Each c As DataColumn In results_table.Columns

              If Not IsDBNull(r.Item(c.ColumnName)) Then
                If Not String.IsNullOrEmpty(r.Item(c.ColumnName).ToString.Trim) Then
                  htmlOut.Append("<td align=""left"" valign=""middle"">" + r.Item(c.ColumnName).ToString.Trim + "</td>")
                Else
                  htmlOut.Append("<td align=""left"" valign=""middle"">0</td>")
                End If
              Else
                htmlOut.Append("<td align=""left"" valign=""middle"">0</td>")
              End If

            Next

            htmlOut.Append("</tr>")


          Next

          htmlOut.Append("</table>")

        End If

      End If

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error In generateModelAttributesSummaryTable() " + ex.Message

    Finally

    End Try

    'return resulting html string
    Return htmlOut.ToString
    htmlOut = Nothing
    results_table = Nothing

  End Function

  Public Function getModelFeaturesDataTable() As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("Select amfeat_feature_code As CODE, kfeat_name As NAME,")
      sQuery.Append(" Case When amfeat_standard_equip Is NULL Then 'UNKNOWN' WHEN amfeat_standard_equip = 'N' THEN 'OPTIONAL' ELSE 'STANDARD' END AS STANDARD,")
      sQuery.Append(" CASE WHEN kfeat_auto_generate_flag = 'N' THEN 'MANUAL' ELSE 'AUTOMATED' END AS MAINTAINED")

      sQuery.Append(" FROM " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Aircraft_Model_Key_Feature WITH(NOLOCK)")
      sQuery.Append(" INNER JOIN " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Key_Feature WITH(NOLOCK) ON amfeat_feature_code = Key_Feature.kfeat_code")

      sQuery.Append(" WHERE kfeat_inactive_date IS NULL AND amfeat_amod_id = @amodID")
      sQuery.Append(" ORDER BY amfeat_standard_equip, amfeat_seq_no ASC")

      SqlCommand.Parameters.AddWithValue("@amodID", inAmodID.ToString)

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>getUserSummaryDatesDataTable() As DataTable</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 240

      SqlCommand.CommandText = sQuery.ToString

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getUserSummaryDatesDataTable load datatable</b><br /> " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getUserSummaryDatesDataTable() As DataTable</b><br />" + ex.Message

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

  Public Function generateModelFeaturesSummaryTable() As String

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder

    Try

      results_table = getModelFeaturesDataTable()

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          htmlOut.Append("<table border=""1"" cellpadding=""2"" cellspacing=""0"" width=""100%"">")

          ' first add the report title
          'htmlOut.Append("<tr><td align=""left"" valign=""middle"" colspan=""" + results_table.Columns.Count.ToString + """><b>Features</b></td></tr>")

          ' second generate the header based off the column names in the datatable
          htmlOut.Append("<tr bgcolor=""#CCCCCC"">")
          For Each c As DataColumn In results_table.Columns
            htmlOut.Append("<td align=""left"" valign=""middle"">" + c.ColumnName.ToUpper.Replace("CCOUNT", "COUNT").Trim + "</td>")
          Next
          htmlOut.Append("</tr>")

          ' second display the report data based off the column names in the datatable
          For Each r As DataRow In results_table.Rows

            htmlOut.Append("<tr>")

            ' ramble through each "column name" and display data
            For Each c As DataColumn In results_table.Columns
              htmlOut.Append("<td align=""left"" valign=""middle"">" + r.Item(c.ColumnName).ToString.Trim + "</td>")
            Next

            htmlOut.Append("</tr>")

          Next

          htmlOut.Append("</table>")

        End If

      End If

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in generateUserSummaryTable() " + ex.Message

    Finally

    End Try

    'return resulting html string
    Return htmlOut.ToString
    htmlOut = Nothing
    results_table = Nothing

  End Function

End Class