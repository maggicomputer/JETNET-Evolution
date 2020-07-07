Partial Public Class performance_specs
    Inherits System.Web.UI.Page
    Dim temp_op_cost_string As String = ""
    Dim string_from_op_costs_for_range As String = ""
    Dim airframe_model_type As String = "F"
    Dim number_of_engine_types As Integer = 0
    Dim times_through_counter As Integer = 0
    'on 5-3-2012 I made a change.
    'I changed the database connection strings to access the
    'database that they're supposed to be looking at. 
    'live, weekly, etc.
    'This is stored in Application.Item("crmJetnetDatabase") which is set in the master pages. 
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'It's very important that you only run this on not post back, 
        'Otherwise it'll repaint the selected items every time you make an action, plus rerun all of the 
        'model queries. 

        If Session.Item("crmUserLogon") = True Then
            If Not Page.IsPostBack Then
                If Session.Item("localUser").crmEvo = True Then
                    model_evo_swap.Visible = True
                    model_crm_swap.Visible = False
                    Dim masterPage As main_site = DirectCast(Page.Master, main_site)
                    clsGeneral.clsGeneral.Getting_Type_Listbox_Set(New DataTable, model_type, masterPage, New DataTable, New DataTable, type)
                ElseIf Session.Item("localUser").crmEvo <> True Then 'If an EVO user
                    clsGeneral.clsGeneral.populate_models(model_cbo, True, Me, Nothing, Master, True)
                    model_crm_swap.Visible = True
                    model_evo_swap.Visible = False
                End If
            End If
        End If
    End Sub

    Function performance_specs_search() Handles model_search.Click
        performance_specs_search = ""
        Dim i As Integer = 0
        Dim amod_id As Integer = 0
        Dim make_model_string As String = ""
        Dim temp_item_string(4) As String
        Dim number_of_selected As Integer = 0
        Dim row_height As Integer = 0
        Dim len_of_string As Integer = 0
        Dim max_num_engines As Integer = 0
        Dim all_is_selected As Boolean = False
        Dim type_string As String = ""

        'Added the crm model controls, added panels to swap and declared model as a new list box.
        'Depending on who you go in as - the model control is set to either the jetnet model or the crm model.
        'Also when I did this, I changed the reference from
        'model.item.items(i).property to model.items(i).property
        Dim model_c As New ListBox

        If Session.Item("localUser").crmEvo = True Then 'if an evo user.
            model_c = model
        ElseIf Session.Item("localUser").crmEvo <> True Then 'If not an EVO user and a CRM user
            model_c = model_cbo
        End If


        Me.performance_specs_label.Text = "<table cellpadding='0' height='200'><tr><td align='left' valign='top'><div class='tab_container_div3'>"


        If model_c.SelectedItem.Value <> "" Then
            'This all is selected is only applicable if you're 
            'an evo user. If you're coming in through the CRM, I 
            'basically just run through the list by model ID. Since
            'I don't have to look for Make/Model All's - I can just use the 
            'amod ID and ignore this part. 
            If Session.Item("localUser").crmEvo = True Then 'if an evo user.
                For i = 0 To model_c.Items.Count - 1
                    If model_c.Items(i).Selected = True Then
                        number_of_selected = number_of_selected + 1
                        If i = 0 Then
                            all_is_selected = True
                        End If
                    End If
                Next
            End If

            For i = 1 To model_c.Items.Count - 1
                If model_c.Items(i).Selected = True Then
                    'this was added purely for the export. It needs to keep a reminder of the models the user picked.
                    'This was because it's pretty impossible to send the selected models in the query because
                    'eventually they'll pick more than the allowable querystring length
                    If model_c.Items(i).Value <> "" Then
                        Session.Item("models_export") = Session.Item("models_export") & "'" & model_c.Items(i).Value & "',"
                    End If
                    number_of_selected = number_of_selected + 1
                    temp_item_string = Split(model_c.Items(i).Value, "|")
                    amod_id = temp_item_string(0)
                    max_num_engines = GetEnginesNumberForSpace(amod_id, 0)
                    If max_num_engines > number_of_engine_types Then
                        number_of_engine_types = max_num_engines
                    End If
                End If
            Next
            'once again, only added for the export 
            If Session.Item("models_export") <> "" Then
                Session.Item("models_export") = UCase(Session.Item("models_export").TrimEnd(","))
            End If


            If number_of_engine_types = 0 Then
                number_of_engine_types = 5
            End If

            'Going to have to ask about this
            number_of_selected = 0
            For i = 0 To Me.make.Items.Count - 1
                If Me.make.Items(i).Selected = True Then
                    number_of_selected = number_of_selected + 1
                    temp_item_string = Split(Me.make.Items(i).Value, "] ")
                    If number_of_selected > 1 Then
                        type_string += ", "
                    End If
                    type_string += "'" & Right(temp_item_string(0), temp_item_string(0).Length - 1) & "'"
                End If
            Next


            Me.performance_specs_label.Text += "<table width='100%' cellpadding='3' cellspacing='2'><tr valign='top'><td width='25%' valign='top' align='left'>"
            Me.performance_specs_label.Text += Build_PerformanceSpecifications(False, "", False, airframe_model_type, amod_id, make_model_string, True, type_string)
            Me.performance_specs_label.Text += "</td><td width='20%' valign='top'  align='left'>"

            If all_is_selected Then 'this part should only ever run during the evolution side 
                number_of_selected = 0
                For i = 0 To Me.make.Items.Count - 1
                    If Me.make.Items(i).Selected = True Then
                        temp_item_string = Split(Me.make.Items(i).Value, "] ")
                        make_model_string = temp_item_string(1)

                        ' THIS DOES THE SEARCH BY MAKE ---- so if you pick 2 makes with 9 models it will run throgh twice 
                        Me.performance_specs_label.Text += Build_PerformanceSpecifications(False, "", False, airframe_model_type, 0, make_model_string, False, type_string)
                    End If
                Next
            Else
                number_of_selected = 0
                For i = 0 To model_c.Items.Count - 1
                    If model_c.Items(i).Selected = True Or (model_c.SelectedValue = "All" And Session.Item("localUser").crmEvo <> True) Then 'This runs through and hits this function if the model is selected
                        'or if All is selected and it's a crm version of the software

                        If model_c.Items(i).Value <> "All" Then
                            'If number_of_selected = 8 Then
                            '    Me.performance_specs_label.Text += "</td></tr>"
                            '    Me.performance_specs_label.Text += "<tr bgcolor=white><Td colspan='10'>&nbsp;</td></tr>"
                            '    Me.performance_specs_label.Text += "<tr bgcolor=white><Td colspan='10'>&nbsp;</td></tr>"
                            '    Me.performance_specs_label.Text += "<tr><td width='25%'>"
                            '    Me.performance_specs_label.Text += Build_PerformanceSpecifications(False, "", False, airframe_model_type, amod_id, make_model_string, True, type_string)
                            '    Me.performance_specs_label.Text += "</td><td width='20%'>"
                            '    number_of_selected = 0
                            If number_of_selected > 0 Then
                                Me.performance_specs_label.Text += "</td><td width='20%' valign='top' align='left'>"
                            End If
                            temp_item_string = Split(model_c.Items.Item(i).Value, "|")
                            amod_id = temp_item_string(0)
                            make_model_string = temp_item_string(1) & " " & temp_item_string(2)
                            Me.performance_specs_label.Text += Build_PerformanceSpecifications(False, "", False, airframe_model_type, amod_id, make_model_string, False, type_string)

                            number_of_selected = number_of_selected + 1
                        End If
                    End If
                Next

            End If

            '   Session("export_info") = "."
            Master.fill_bar()
            Me.performance_specs_label.Text += "</td></tr></table>"
        Else
            Me.performance_specs_label.Text += "<table><tr><td>"
            Me.performance_specs_label.Text += "No Models Selected"
            Me.performance_specs_label.Text += "</td></tr></table>"
        End If
        Me.performance_specs_label.Text += "</div></td></tr></table>"


    End Function
  Public Function Build_PerformanceSpecifications(ByVal bisReport As Boolean, ByVal optFormat As String, ByVal bHasManyAirFrames As Boolean, ByVal sAirframeType As String, ByVal amod_id As Integer, ByVal make_model_name As String, ByVal bIsFirstTime As Boolean, ByVal product_code_from_make As String) As String
    Build_PerformanceSpecifications = ""

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim l_AdoRs As System.Data.SqlClient.SqlDataReader : l_AdoRs = Nothing
    Dim Query As String : Query = ""
    Dim l_objBuilder As New StringBuilder
    Dim nNumberOfEngines As Integer = 0
    Dim nRememberModelID As Long = 0
    Dim nMAXEngines As Long = 0
    Dim i As Integer = 0
    Dim iLoop2 As Integer = 0


    Try

      If bIsFirstTime Then
        l_objBuilder.Append("<table align='left' cellpadding='2' cellspacing='0' width='100%' valign='top'>" & vbCrLf)
        l_objBuilder.Append("<tr class='aircraft_list'><td nowrap><strong>Model Name</strong></td></tr><tr>" & vbCrLf)
        l_objBuilder.Append("<tr class='alt_row'><td nowrap><strong>Fuselage&nbsp;Dimensions</strong></td></tr>" & vbCrLf)
        l_objBuilder.Append("<tr>")

        If Session.Item("useMetricValues") Then
          l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Length (" & ConversionFunctions.TranslateUSMetricUnitsShort("FT") & "):</td></tr><tr class='alt_row'>" & vbCrLf)
          l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Height (" & ConversionFunctions.TranslateUSMetricUnitsShort("FT") & "):</td></tr><tr>" & vbCrLf)

          If sAirframeType = "F" And Not bHasManyAirFrames Then
            l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Wing Span (" & ConversionFunctions.TranslateUSMetricUnitsShort("FT") & "):</td></tr>" & vbCrLf)
          ElseIf sAirframeType = "R" And Not bHasManyAirFrames Then
            l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Width (" & ConversionFunctions.TranslateUSMetricUnitsShort("FT") & "):</td></tr>" & vbCrLf)
          ElseIf (sAirframeType = "F" Or sAirframeType = "R") And bHasManyAirFrames Then
            l_objBuilder.Append("<td class='Label' valign='middle' align='right'>[F] Wing Span (" & ConversionFunctions.TranslateUSMetricUnitsShort("FT") & ") / [R] Width (" & ConversionFunctions.TranslateUSMetricUnitsShort("FT") & "):</td></tr>" & vbCrLf)
          End If
        Else
          l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Length (ft):</td></tr><tr class='alt_row'>" & vbCrLf)
          l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Height (ft):</td></tr>" & vbCrLf)

          If sAirframeType = "F" And Not bHasManyAirFrames Then
            l_objBuilder.Append("<tr><td class='Label' valign='middle' align='right'>Wing Span (ft):</td></tr>" & vbCrLf)
          ElseIf sAirframeType = "R" And Not bHasManyAirFrames Then
            l_objBuilder.Append("<tr><td class='Label' valign='middle' align='right'>Width (ft):</td></tr>" & vbCrLf)
          ElseIf (sAirframeType = "F" Or sAirframeType = "R") And bHasManyAirFrames Then
            l_objBuilder.Append("<tr><td class='Label' valign='middle' align='right'>[F] Wing Span (ft) / [R] Width (ft):</td></tr>" & vbCrLf)
          End If
        End If

        l_objBuilder.Append("<tr class='alt_row'><td nowrap><strong>Cabin&nbsp;Dimensions</strong></td></tr><tr>" & vbCrLf)

        If Session.Item("useMetricValues") Then
          l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Length (" & ConversionFunctions.TranslateUSMetricUnitsShort("FT") & "):</td></tr><tr class='alt_row'>" & vbCrLf)
          l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Height (" & ConversionFunctions.TranslateUSMetricUnitsShort("FT") & "):</td></tr><tr>" & vbCrLf)
          l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Width (" & ConversionFunctions.TranslateUSMetricUnitsShort("FT") & "):</td></tr><tr>" & vbCrLf)
          l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Cabin&nbsp;Volume (" + ConversionFunctions.TranslateUSMetricUnitsShort("CBFT") + "):</td></tr><tr>" & vbCrLf)
          l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Baggage&nbsp;Volume (" + ConversionFunctions.TranslateUSMetricUnitsShort("CBFT") + "):</td></tr>" & vbCrLf)
        Else
          l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Length (ft)(inches):</td></tr><tr class='alt_row'>" & vbCrLf)
          l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Height (ft)(inches):</td></tr><tr>" & vbCrLf)
          l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Width (ft)(inches):</td></tr><tr>" & vbCrLf)
          l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Cabin&nbsp;Volume (cb ft):</td></tr><tr>" & vbCrLf)
          l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Baggage&nbsp;Volume (cb ft):</td></tr>" & vbCrLf)
        End If

        l_objBuilder.Append("<tr class='alt_row'><td><strong>Typical&nbsp;Configuration</strong></td></tr><tr>" & vbCrLf)

        l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Crew:</td></tr><tr class='alt_row'>" & vbCrLf)
        l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Passengers:</td></tr>" & vbCrLf)

        If Session.Item("useMetricValues") Then
          If sAirframeType = "F" And Not bHasManyAirFrames Then
            l_objBuilder.Append("<tr><td class='Label' valign='middle' align='right'>Pressurization&nbsp;(" & ConversionFunctions.TranslateUSMetricUnitsShort("PSI") & "):</td></tr>" & vbCrLf)
          ElseIf sAirframeType = "R" And Not bHasManyAirFrames Then
            ' do nothing
          ElseIf (sAirframeType = "F" Or sAirframeType = "R") And bHasManyAirFrames Then
            l_objBuilder.Append("<tr><td class='Label' valign='middle' align='right'>[F] Pressurization&nbsp;(" & ConversionFunctions.TranslateUSMetricUnitsShort("PSI") & "):</td></tr>" & vbCrLf)
          End If
        Else
          If sAirframeType = "F" And Not bHasManyAirFrames Then
            l_objBuilder.Append("<tr><td class='Label' valign='middle' align='right'>Pressurization&nbsp;(psi):</td></tr>" & vbCrLf)
          ElseIf sAirframeType = "R" And Not bHasManyAirFrames Then
            ' do nothing
          ElseIf (sAirframeType = "F" Or sAirframeType = "R") And bHasManyAirFrames Then
            l_objBuilder.Append("<tr><td class='Label' valign='middle' align='right'>[F] Pressurization&nbsp;(psi):</td></tr>" & vbCrLf)
          End If
        End If

        l_objBuilder.Append("<tr class='alt_row'><td><strong>Fuel Capacity</strong></td></tr><tr>")

        If Session.Item("useMetricValues") Then
          l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Standard&nbsp;(" & ConversionFunctions.TranslateUSMetricUnitsShort("lbs") & "):</td></tr><tr class='alt_row'>" & vbCrLf)
          l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Standard&nbsp;(" & ConversionFunctions.TranslateUSMetricUnitsShort("gal") & "):</td></tr><tr>" & vbCrLf)
          l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Optional&nbsp;(" & ConversionFunctions.TranslateUSMetricUnitsShort("lbs") & "):</td></tr><tr class='alt_row'>" & vbCrLf)
          l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Optional&nbsp;(" & ConversionFunctions.TranslateUSMetricUnitsShort("gal") & "):</td></tr>" & vbCrLf)
        Else
          l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Standard&nbsp;(lbs):</td></tr><tr class='alt_row'>" & vbCrLf)
          l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Standard&nbsp;(gal):</td></tr><tr>" & vbCrLf)
          l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Optional&nbsp;(lbs):</td></tr><tr class='alt_row'>" & vbCrLf)
          l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Optional&nbsp;(gal):</td></tr>" & vbCrLf)
        End If

        l_objBuilder.Append("<tr><td><strong>Weight</strong></td></tr><tr class='alt_row'>" & vbCrLf)

        If Session.Item("useMetricValues") Then
          l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Max.&nbsp;Ramp&nbsp;(" & ConversionFunctions.TranslateUSMetricUnitsShort("lbs") & "):</td></tr><tr>" & vbCrLf)
          l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Max.&nbsp;Takeoff&nbsp;(" & ConversionFunctions.TranslateUSMetricUnitsShort("lbs") & "):</td></tr><tr class='alt_row'>" & vbCrLf)

          If sAirframeType = "F" And Not bHasManyAirFrames Then
            l_objBuilder.Append("<td nowrap class='Label' valign='middle' align='right'>Zero&nbsp;Fuel&nbsp;(" & ConversionFunctions.TranslateUSMetricUnitsShort("lbs") & "):</td></tr><tr>" & vbCrLf)
          ElseIf sAirframeType = "R" And Not bHasManyAirFrames Then
            l_objBuilder.Append("<td nowrap class='Label' valign='middle' align='right'>Empty&nbsp;Operating&nbsp;Weight&nbsp;(EOW):</td></tr><tr>" & vbCrLf)
          ElseIf (sAirframeType = "F" Or sAirframeType = "R") And bHasManyAirFrames Then
            l_objBuilder.Append("<td nowrap class='Label' valign='middle' align='right'>[F] Zero&nbsp;Fuel&nbsp;(" & ConversionFunctions.TranslateUSMetricUnitsShort("lbs") & ") / [R] Empty&nbsp;Operating&nbsp;Weight&nbsp;(" & ConversionFunctions.TranslateUSMetricUnitsShort("lbs") & "):</td></tr><tr>" & vbCrLf)
          End If
          l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Basic&nbsp;Operating&nbsp;(" & ConversionFunctions.TranslateUSMetricUnitsShort("lbs") & "):</td></tr><tr class='alt_row'>" & vbCrLf)
          l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Max.&nbsp;Landing&nbsp;(" & ConversionFunctions.TranslateUSMetricUnitsShort("lbs") & "):</td></tr>" & vbCrLf)
        Else
          l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Max.&nbsp;Ramp&nbsp;(lbs):</td></tr><tr>" & vbCrLf)
          l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Max.&nbsp;Takeoff&nbsp;(lbs):</td></tr><tr class='alt_row'>" & vbCrLf)

          If sAirframeType = "F" And Not bHasManyAirFrames Then
            l_objBuilder.Append("<td nowrap class='Label' valign='middle' align='right'>Zero&nbsp;Fuel&nbsp;(lbs):</td></tr><tr>" & vbCrLf)
          ElseIf sAirframeType = "R" And Not bHasManyAirFrames Then
            l_objBuilder.Append("<td nowrap class='Label' valign='middle' align='right'>Empty&nbsp;Operating&nbsp;Weight&nbsp;(EOW):</td></tr><tr>" & vbCrLf)
          ElseIf (sAirframeType = "F" Or sAirframeType = "R") And bHasManyAirFrames Then
            l_objBuilder.Append("<td nowrap class='Label' valign='middle' align='right'>[F] Zero&nbsp;Fuel&nbsp;(lbs) / [R] Empty&nbsp;Operating&nbsp;Weight&nbsp;(lbs):</td></tr><tr>" & vbCrLf)
          End If
          l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Basic&nbsp;Operating&nbsp;(lbs):</td></tr><tr class='alt_row'>" & vbCrLf)
          l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Max.&nbsp;Landing&nbsp;(lbs):</td></tr>" & vbCrLf)
        End If

        If Session.Item("useMetricValues") Then
          l_objBuilder.Append("<tr><td><strong>Speed&nbsp;" & ConversionFunctions.TranslateUSMetricUnitsLong("KN") & "</strong></td></tr><tr class='alt_row'>" & vbCrLf)
        Else
          l_objBuilder.Append("<tr><td><strong>Speed&nbsp;Knots</strong></td></tr><tr class='alt_row'>" & vbCrLf)
        End If

        If sAirframeType = "F" And Not bHasManyAirFrames Then
          l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Vs&nbsp;Clean:</td></tr><tr>" & vbCrLf)
        ElseIf sAirframeType = "R" And Not bHasManyAirFrames Then
          ' do nothing
        ElseIf (sAirframeType = "F" Or sAirframeType = "R") And bHasManyAirFrames Then
          l_objBuilder.Append("<td class='Label' valign='middle' align='right'>[F] Vs&nbsp;Clean:</td></tr><tr>" & vbCrLf)
        End If

        If sAirframeType = "F" And Not bHasManyAirFrames Then
          l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Vso&nbsp;Landing:</td></tr><tr class='alt_row'>" & vbCrLf)
        ElseIf sAirframeType = "R" And Not bHasManyAirFrames Then
          ' do nothing
        ElseIf (sAirframeType = "F" Or sAirframeType = "R") And bHasManyAirFrames Then
          l_objBuilder.Append("<td class='Label' valign='middle' align='right'>[F] Vso&nbsp;Landing:</td></tr><tr class='alt_row'>" & vbCrLf)
        End If
        l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Normal&nbsp;Cruise&nbsp;TAS:</td></tr><tr>" & vbCrLf)
        l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Vmo&nbsp;(Max&nbsp;Op)&nbsp;IAS:</td></tr><tr>" & vbCrLf)

        If sAirframeType.ToUpper.Contains("F") And Not bHasManyAirFrames Then
          'do nothing
        ElseIf sAirframeType.ToUpper.Contains("R") And Not bHasManyAirFrames Then
          l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Vne&nbsp;:</td></tr><tr>" & vbCrLf)
        ElseIf bHasManyAirFrames Then
          l_objBuilder.Append("<td class='Label' valign='middle' align='right'>[R]&nbsp;Vne&nbsp;:</td></tr><tr>" & vbCrLf)
        End If

        l_objBuilder.Append("<td class='Label' valign='middle' align='right'>V1&nbsp;Takeoff&nbsp;:</td></tr><tr>" & vbCrLf)
        l_objBuilder.Append("<td class='Label' valign='middle' align='right'>VFE&nbsp;Max&nbsp;Flap&nbsp;Ext&nbsp;:</td></tr><tr>" & vbCrLf)
        l_objBuilder.Append("<td class='Label' valign='middle' align='right'>VLE&nbsp;Max&nbsp;Land Gear&nbsp;Ext&nbsp;:</td></tr>" & vbCrLf)

        l_objBuilder.Append("<tr class='alt_row'><td nowrap><strong>IFR Certification:<strong></td></tr><tr>" & vbCrLf)
        l_objBuilder.Append("<td class='Label' valign='middle' align='right'>(IFR):</td></tr>" & vbCrLf)

        l_objBuilder.Append("<tr class='alt_row'><td><strong>Climb</strong></td></tr><tr>")
        If Session.Item("useMetricValues") Then
          l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Normal&nbsp;(" & ConversionFunctions.TranslateUSMetricUnitsShort("FPM") & "):</td></tr><tr class='alt_row'>" & vbCrLf)
          l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Engine&nbsp;Out&nbsp;(" & ConversionFunctions.TranslateUSMetricUnitsShort("FPM") & "):</td></tr><tr>" & vbCrLf)
          l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Ceiling (" & ConversionFunctions.TranslateUSMetricUnitsShort("FT") & "):</td></tr>" & vbCrLf)
        Else
          l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Normal&nbsp;(fpm):</td></tr><tr class='alt_row'>" & vbCrLf)
          l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Engine&nbsp;Out&nbsp;(fpm):</td></tr><tr>" & vbCrLf)
          l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Ceiling (ft):</td></tr>" & vbCrLf)
        End If


        If sAirframeType = "F" And Not bHasManyAirFrames Then
          'do nothing
        ElseIf sAirframeType = "R" And Not bHasManyAirFrames Then
          l_objBuilder.Append("<tr class='alt_row'><td class='Label' valign='middle' align='right'>(HOGE)&nbsp;Out&nbsp;of&nbsp;Ground&nbsp;Effect:</td></tr>" & vbCrLf)
          l_objBuilder.Append("<tr><td class='Label' valign='middle' align='right'>(HIGE)&nbsp;In&nbsp;Ground&nbsp;Effect:</td></tr>" & vbCrLf)
        ElseIf (sAirframeType = "F" Or sAirframeType = "R") And bHasManyAirFrames Then
          l_objBuilder.Append("<tr class='alt_row'><td class='Label' valign='middle' align='right'>[R] (HOGE)&nbsp;Out&nbsp;of&nbsp;Ground&nbsp;Effect:</td></tr>" & vbCrLf)
          l_objBuilder.Append("<tr><td class='Label' valign='middle' align='right'>[R] (HIGE)&nbsp;In&nbsp;Ground&nbsp;Effect:</td></tr>" & vbCrLf)
        End If

        If sAirframeType = "F" And Not bHasManyAirFrames Then
          temp_op_cost_string += "<tr class='alt_row'><td><strong>Landing Performance</strong></td></tr><tr>"
          If Session.Item("useMetricValues") Then
            temp_op_cost_string += "<td class='Label' valign='middle' align='right'>FAA&nbsp;Field&nbsp;Length (" & ConversionFunctions.TranslateUSMetricUnitsShort("FT") & "):</td></tr>"
          Else
            temp_op_cost_string += "<td class='Label' valign='middle' align='right'>FAA&nbsp;Field&nbsp;Length (ft):</td></tr>"
          End If
        ElseIf sAirframeType = "R" And Not bHasManyAirFrames Then
          ' do nothing
        ElseIf (sAirframeType = "F" Or sAirframeType = "R") And bHasManyAirFrames Then
          temp_op_cost_string += "<tr class='alt_row'><td><strong>[F] Landing Performance<strong></td></tr><tr>"
          If Session.Item("useMetricValues") Then
            temp_op_cost_string += "<td class='Label' valign='middle' align='right'>[F] FAA&nbsp;Field&nbsp;Length (" & ConversionFunctions.TranslateUSMetricUnitsShort("FT") & "):</td></tr>"
          Else
            temp_op_cost_string += "<td class='Label' valign='middle' align='right'>[F] FAA&nbsp;Field&nbsp;Length (ft):</td></tr>"
          End If
        End If
        'amod_field_length, amod_max_range_miles
        temp_op_cost_string += "<tr class='alt_row'><td><strong>Takeoff Performance</strong></td></tr><tr>"
        temp_op_cost_string += "<td class='Label' valign='middle' align='right'>SL&nbsp;ISA&nbsp;BFL:</td></tr><tr class='alt_row'>"
        temp_op_cost_string += "<td class='Label' valign='middle' align='right'>5000'&nbsp;+20C&nbsp;BFL:</td></tr>"

        If Session.Item("useMetricValues") Then
          temp_op_cost_string += "<tr class='alt_row'><td nowrap><strong>Range (" & ConversionFunctions.TranslateUSMetricUnitsLong("NM") & ")</strong></td></tr>"
          temp_op_cost_string += "<tr><td class='Label' valign='middle' align='right'>Range&nbsp;(" & ConversionFunctions.TranslateUSMetricUnitsShort("NM") & "):</td></tr>"
          temp_op_cost_string += "<tr><td class='Label' valign='middle' align='right'>Tanks&nbsp;Full&nbsp;(" & ConversionFunctions.TranslateUSMetricUnitsShort("NM") & "):</td></tr>"
          temp_op_cost_string += "<tr><td class='Label' valign='middle' align='right'>Seats&nbsp;Full&nbsp;(" & ConversionFunctions.TranslateUSMetricUnitsShort("NM") & "):</td></tr>"
          temp_op_cost_string += "<tr><td class='Label' valign='middle' align='right'>Range&nbsp;(4&nbsp;PAX)&nbsp;(" & ConversionFunctions.TranslateUSMetricUnitsShort("NM") & "):</td></tr>"
          temp_op_cost_string += "<tr><td class='Label' valign='middle' align='right'>Range&nbsp;(8&nbsp;PAX)&nbsp;(" & ConversionFunctions.TranslateUSMetricUnitsShort("NM") & "):</td></tr>"
        Else
          temp_op_cost_string += "<tr class='alt_row'><td nowrap><strong>Range (Nautical Miles)</strong></td></tr>"
          temp_op_cost_string += "<tr><td class='Label' valign='middle' align='right'>Range&nbsp;(nm):</td></tr>"
          temp_op_cost_string += "<tr><td class='Label' valign='middle' align='right'>Tanks&nbsp;Full&nbsp;(nm):</td></tr>"
          temp_op_cost_string += "<tr><td class='Label' valign='middle' align='right'>Seats&nbsp;Full&nbsp;(nm):</td></tr>"
          temp_op_cost_string += "<tr><td class='Label' valign='middle' align='right'>Range&nbsp;(4&nbsp;PAX)&nbsp;(nm):</td></tr>"
          temp_op_cost_string += "<tr><td class='Label' valign='middle' align='right'>Range&nbsp;(8&nbsp;PAX)&nbsp;(nm):</td></tr>"
        End If

        l_objBuilder.Append(temp_op_cost_string)
        string_from_op_costs_for_range += temp_op_cost_string & "</table></td><td width='15%'><table width='100%'>"

        temp_op_cost_string = ""
        l_objBuilder.Append("<tr><td nowrap><strong>Engines</strong></td></tr><tr class='alt_row'>" & vbCrLf)
        l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Number&nbsp;of:</td></tr><tr valign='top'>" & vbCrLf)
        l_objBuilder.Append("<td class='Label' valign='top' align='right'>Model(s):" & vbCrLf)


        For iLoop2 = 1 To number_of_engine_types - 1
          l_objBuilder.Append("<br>&nbsp;" & vbCrLf)
        Next

        l_objBuilder.Append("</td></tr><tr class='alt_row'>" & vbCrLf)

        If Session.Item("useMetricValues") Then
          l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Thrust&nbsp;(" & ConversionFunctions.TranslateUSMetricUnitsShort("LBS") & "&nbsp;per&nbsp;Engine):</td></tr><tr>" & vbCrLf)
          l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Shaft&nbsp;(" & ConversionFunctions.TranslateUSMetricUnitsShort("HP") & "&nbsp;per&nbsp;Engine):</td></tr><tr>")
        Else
          l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Thrust&nbsp;(lbs&nbsp;per&nbsp;Engine):</td></tr><tr>" & vbCrLf)
          l_objBuilder.Append("<td class='Label' valign='middle' align='right'>Shaft&nbsp;(hp&nbsp;per&nbsp;Engine):</td></tr><tr>" & vbCrLf)
        End If

        l_objBuilder.Append("<td class='Label' valign='middle' align='right' class='alt_row'>Common&nbsp;TBO&nbsp;Hours:</td></tr>" & vbCrLf)
        ''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' End the break here for the other column
        ''''''''''''''''''''''''''''''''''''''''''''''''''''

        l_objBuilder.Append("</table>" & vbCrLf)

      Else


        SqlConn.ConnectionString = Application.Item("crmJetnetDatabase") 'My.Settings.DEFAULT_LIVE_MSSQL_DEBUG

        SqlConn.Open()

        SqlCommand.Connection = SqlConn
        SqlCommand.CommandType = System.Data.CommandType.Text
        SqlCommand.CommandTimeout = 60
        Query = "SELECT * FROM Aircraft_Model WITH(NOLOCK) WHERE amod_id = " + amod_id.ToString

        If amod_id = 0 Then
          Query = "SELECT * FROM Aircraft_Model WITH(NOLOCK) "
          Query = Query & " WHERE amod_make_name = '" & make_model_name & "' "
          Query = Query & " and amod_type_code IN(" & product_code_from_make & ") "
          'Else
          '    Query = "SELECT * FROM Aircraft_Model WITH(NOLOCK) "
          '    Query = Query & " WHERE amod_id = " + amod_id.ToString
          '    Query = Query & " and amod_type_code IN(" & product_code_from_make & ") "
        End If

        Query = Query & clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(Session.Item("localSubscription"), False, False)
        SqlCommand.CommandText = Query
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>Function Build_PerformanceSpecifications(ByVal bisReport As Boolean, ByVal optFormat As String, ByVal bHasManyAirFrames As Boolean, ByVal sAirframeType As String, ByVal amod_id As Integer, ByVal make_model_name As String, ByVal bIsFirstTime As Boolean, ByVal product_code_from_make As String) As String</b><br />" & Query


        l_AdoRs = SqlCommand.ExecuteReader()


        If l_AdoRs.HasRows Then
          Do While l_AdoRs.Read()

            If amod_id = 0 And times_through_counter > 0 Then
              l_objBuilder.Append("</td><td width='20%'>")
            End If

            times_through_counter = times_through_counter + 1


            Build_PerformanceSpecifications = ""
            If CInt(l_AdoRs.Item("amod_id")) <> CInt(nRememberModelID) Then
              nNumberOfEngines = l_AdoRs.Item("amod_number_of_engines")
            End If
            If nMAXEngines < nNumberOfEngines Then
              nMAXEngines = nNumberOfEngines
            End If

            If Trim(l_AdoRs.Item("amod_airframe_type_code")) <> "" Then
              sAirframeType = l_AdoRs.Item("amod_airframe_type_code")
            Else
              sAirframeType = "F"
            End If


            l_objBuilder.Append("<table align='left' cellpadding='2' cellspacing='0' width='100%' valign='top'>" & vbCrLf)

            '''''''''''''''''''''''''''''''''''''''''''''''''''
            ' Start left side
            '''''''''''''''''''''''''''''''''''''''''''''''''''
            l_objBuilder.Append("<tr class='aircraft_list'><td nowrap><b>" & l_AdoRs.Item("amod_make_name") & " " & l_AdoRs.Item("amod_model_name") & "</b></td></tr>" & vbCrLf)
            l_objBuilder.Append("<tr class='alt_row'><td nowrap>&nbsp;</td></tr>" & vbCrLf)
            l_objBuilder.Append("<tr>")
            If Session.Item("useMetricValues") Then
              l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(ConversionFunctions.ConvertUSToMetricValue("FT", CDbl(l_AdoRs.Item("amod_fuselage_length"))), 1, False, True, False) & "&nbsp;</td></tr><tr class='alt_row'>" & vbCrLf)
              l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(ConversionFunctions.ConvertUSToMetricValue("FT", CDbl(l_AdoRs.Item("amod_fuselage_height"))), 1, False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)

              If sAirframeType = "F" Then
                l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(ConversionFunctions.ConvertUSToMetricValue("FT", CDbl(l_AdoRs.Item("amod_fuselage_wingspan"))), 1, False, True, False) & "&nbsp;</td></tr>" & vbCrLf)
              ElseIf sAirframeType = "R" Then
                l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(ConversionFunctions.ConvertUSToMetricValue("FT", CDbl(l_AdoRs.Item("amod_fuselage_width"))), 1, False, True, False) & "&nbsp;</td></tr>" & vbCrLf)
              End If
            Else
              l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(CDbl(l_AdoRs.Item("amod_fuselage_length")), 1, False, True, False) & "&nbsp;</td></tr><tr class='alt_row'>" & vbCrLf)
              l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(CDbl(l_AdoRs.Item("amod_fuselage_height")), 1, False, True, False) & "&nbsp;</td></tr>" & vbCrLf)

              If sAirframeType = "F" Then
                l_objBuilder.Append("<tr><td valign='middle' align='right'>" & FormatNumber(CDbl(l_AdoRs.Item("amod_fuselage_wingspan")), 1, False, True, False) & "&nbsp;</td></tr>" & vbCrLf)
              ElseIf sAirframeType = "R" Then
                l_objBuilder.Append("<tr><td valign='middle' align='right'>" & FormatNumber(CDbl(l_AdoRs.Item("amod_fuselage_width")), 1, False, True, False) & "&nbsp;</td></tr>" & vbCrLf)
              End If
            End If

            l_objBuilder.Append("<tr class='alt_row'><td>&nbsp;</td></tr><tr>" & vbCrLf)

            ' THIS IS FOR CABIN DIMENSIONS
            If Session.Item("useMetricValues") Then

              If Not IsDBNull(l_AdoRs.Item("amod_cabinsize_length_feet")) Then
                If CDbl(l_AdoRs.Item("amod_cabinsize_length_feet")) > 0 Then
                  l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(ConversionFunctions.ConvertUSToMetricValue("FT", CDbl(l_AdoRs.Item("amod_cabinsize_length_feet"))), 1, False, True, False) & "&nbsp;</td></tr><tr class='alt_row'>" & vbCrLf)
                Else
                  l_objBuilder.Append("<td valign='middle' align='right'>0&nbsp;</td></tr><tr class='alt_row'>" & vbCrLf)
                End If
              Else
                l_objBuilder.Append("<td valign='middle' align='right'>0&nbsp;</td></tr><tr class='alt_row'>" & vbCrLf)
              End If

              If Not IsDBNull(l_AdoRs.Item("amod_cabinsize_height_feet")) Then
                If CDbl(l_AdoRs.Item("amod_cabinsize_height_feet")) > 0 Then
                  l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(ConversionFunctions.ConvertUSToMetricValue("FT", CDbl(l_AdoRs.Item("amod_cabinsize_height_feet"))), 1, False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
                Else
                  l_objBuilder.Append("<td valign='middle' align='right'>0&nbsp;</td></tr><tr>" & vbCrLf)
                End If
              Else
                l_objBuilder.Append("<td valign='middle' align='right'>0&nbsp;</td></tr><tr>" & vbCrLf)
              End If

              If Not IsDBNull(l_AdoRs.Item("amod_cabinsize_width_feet")) Then
                If CDbl(l_AdoRs.Item("amod_cabinsize_width_feet")) > 0 Then
                  l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(ConversionFunctions.ConvertUSToMetricValue("FT", CDbl(l_AdoRs.Item("amod_cabinsize_width_feet"))), 1, False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
                Else
                  l_objBuilder.Append("<td valign='middle' align='right'>0&nbsp;</td></tr>" & vbCrLf)
                End If
              Else
                l_objBuilder.Append("<td valign='middle' align='right'>0&nbsp;</td></tr>" & vbCrLf)
              End If

              If Not IsDBNull(l_AdoRs("amod_cabin_volume")) Then
                If CDbl(l_AdoRs.Item("amod_cabin_volume").ToString) > 0 Then
                  l_objBuilder.Append("<td valign='middle' align='right'>" + FormatNumber(ConversionFunctions.ConvertUSToMetricValue("CBFT", CDbl(l_AdoRs.Item("amod_cabin_volume").ToString)), 1, False, True, False) + "&nbsp;</td></tr><tr>")
                Else
                  l_objBuilder.Append("<td valign='middle' align='right'>0&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("CBFT") + ")</td></tr><tr>")
                End If
              Else
                l_objBuilder.Append("<td valign='middle' align='right'0&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("CBFT") + ")</td></tr><tr>")
              End If

              If Not IsDBNull(l_AdoRs("amod_baggage_volume")) Then
                If CDbl(l_AdoRs.Item("amod_baggage_volume").ToString) > 0 Then
                  l_objBuilder.Append("<td valign='middle' align='right'>" + FormatNumber(ConversionFunctions.ConvertUSToMetricValue("CBFT", CDbl(l_AdoRs.Item("amod_baggage_volume").ToString)), 1, False, True, False) + "&nbsp;</td></tr>")
                Else
                  l_objBuilder.Append("<td valign='middle' align='right'>0&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("CBFT") + ")</td></tr>")
                End If
              Else
                l_objBuilder.Append("<td valign='middle' align='right'>0&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("CBFT") + ")</td></tr>")
              End If

            Else

              If Not IsDBNull(l_AdoRs.Item("amod_cabinsize_length_feet")) Then
                If CDbl(l_AdoRs.Item("amod_cabinsize_length_feet")) > 0 Then
                  l_objBuilder.Append("<td valign='middle' align='right'>" & l_AdoRs.Item("amod_cabinsize_length_feet") & "&#34; " & l_AdoRs.Item("amod_cabinsize_length_inches") & "'&nbsp;</td></tr><tr class='alt_row'>" & vbCrLf)
                Else
                  l_objBuilder.Append("<td valign='middle' align='right'>0&nbsp;</td></tr><tr class='alt_row'>" & vbCrLf)
                End If
              Else
                l_objBuilder.Append("<td valign='middle' align='right'>0&nbsp;</td></tr><tr class='alt_row'>" & vbCrLf)
              End If

              If Not IsDBNull(l_AdoRs.Item("amod_cabinsize_height_feet")) Then
                If CDbl(l_AdoRs.Item("amod_cabinsize_height_feet")) > 0 Then
                  l_objBuilder.Append("<td valign='middle' align='right'>" & l_AdoRs.Item("amod_cabinsize_height_feet") & "&#34; " & l_AdoRs.Item("amod_cabinsize_height_inches") & "'&nbsp;</td></tr><tr>" & vbCrLf)
                Else
                  l_objBuilder.Append("<td valign='middle' align='right'>0&nbsp;</td></tr><tr>" & vbCrLf)
                End If
              Else
                l_objBuilder.Append("<td valign='middle' align='left'>0&nbsp;</td></tr><tr>" & vbCrLf)
              End If

              If Not IsDBNull(l_AdoRs.Item("amod_cabinsize_width_feet")) Then
                If CDbl(l_AdoRs.Item("amod_cabinsize_width_feet")) > 0 Then
                  l_objBuilder.Append("<td valign='middle' align='right'>" & l_AdoRs.Item("amod_cabinsize_width_feet") & "&#34; " & l_AdoRs.Item("amod_cabinsize_width_inches") & "'&nbsp;</td></tr>" & vbCrLf)
                Else
                  l_objBuilder.Append("<td valign='middle' align='right'>0&nbsp;</td></tr>" & vbCrLf)
                End If
              Else
                l_objBuilder.Append("<td valign='middle' align='right'>0&nbsp;</td></tr>" & vbCrLf)
              End If

              If Not IsDBNull(l_AdoRs("amod_cabin_volume")) Then
                If CDbl(l_AdoRs.Item("amod_cabin_volume").ToString) > 0 Then
                  l_objBuilder.Append("<td valign='middle' align='right'>" + FormatNumber(CDbl(l_AdoRs.Item("amod_cabin_volume").ToString), False, True, False) + "&nbsp;(cb)(ft)</td></tr><tr>")
                Else
                  l_objBuilder.Append("<td valign='middle' align='right'>0&nbsp;(cb)(ft)</td></tr><tr>")
                End If
              Else
                l_objBuilder.Append("<td valign='middle' align='right'0&nbsp;(cb)(ft)</td></tr><tr>")
              End If

              If Not IsDBNull(l_AdoRs("amod_baggage_volume")) Then
                If CDbl(l_AdoRs.Item("amod_baggage_volume").ToString) > 0 Then
                  l_objBuilder.Append("<td valign='middle' align='right'>" + FormatNumber(CDbl(l_AdoRs.Item("amod_baggage_volume").ToString), False, True, False) + "&nbsp;(cb)(ft)</td></tr>")
                Else
                  l_objBuilder.Append("<td valign='middle' align='right'>0&nbsp;(cb)(ft)</td></tr>")
                End If
              Else
                l_objBuilder.Append("<td valign='middle' align='right'>0&nbsp;(cb)(ft)</td></tr>")
              End If

            End If

            l_objBuilder.Append("<tr class='alt_row'><td>&nbsp;</td></tr><tr>" & vbCrLf)

            l_objBuilder.Append("<td valign='middle' align='right'>" & l_AdoRs.Item("amod_number_of_crew") & "&nbsp;</td></tr><tr class='alt_row'>" & vbCrLf)
            l_objBuilder.Append("<td valign='middle' align='right'>" & l_AdoRs.Item("amod_number_of_passengers") & "&nbsp;</td></tr>" & vbCrLf)


            ' THIS IS FOR PRESSURIZATION SECTION
            If sAirframeType = "F" Then
              If Session.Item("useMetricValues") Then
                l_objBuilder.Append("<tr><td valign='middle' align='right'>" & FormatNumber(ConversionFunctions.ConvertUSToMetricValue("PSI", CDbl(l_AdoRs.Item("amod_pressure"))), 1, False, True, False) & "&nbsp;</td></tr>" & vbCrLf)
              Else
                l_objBuilder.Append("<tr><td valign='middle' align='right'>" & CDbl(l_AdoRs.Item("amod_pressure")) & "&nbsp;</td></tr>" & vbCrLf)
              End If
            ElseIf sAirframeType = "R" And bHasManyAirFrames Then
              l_objBuilder.Append("<tr><td valign='middle' align='right'>&nbsp;</td></tr>" & vbCrLf)
            End If

            l_objBuilder.Append("<tr class='alt_row'><td>&nbsp;</td></tr><tr>" & vbCrLf)

            If Not IsDBNull(l_AdoRs.Item("amod_fuel_cap_std_weight")) Then
              If CDbl(l_AdoRs.Item("amod_fuel_cap_std_weight")) > 0 Then
                If Session.Item("useMetricValues") Then
                  l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(ConversionFunctions.ConvertUSToMetricValue("LBS", CDbl(l_AdoRs.Item("amod_fuel_cap_std_weight"))), False, True, False) & "&nbsp;</td></tr><tr class='alt_row'>" & vbCrLf)
                Else
                  l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(CDbl(l_AdoRs.Item("amod_fuel_cap_std_weight")), False, True, False) & "&nbsp;</td></tr><tr class='alt_row'>" & vbCrLf)
                End If
              Else
                l_objBuilder.Append("<td valign='middle' align='right'>0&nbsp;</td></tr><tr class='alt_row'>" & vbCrLf)
              End If
            Else
              l_objBuilder.Append("<td valign='middle' align='right'>0&nbsp;</td></tr><tr class='alt_row'>" & vbCrLf)
            End If

            If Not IsDBNull(l_AdoRs.Item("amod_fuel_cap_std_gal")) Then
              If CDbl(l_AdoRs.Item("amod_fuel_cap_std_gal")) > 0 Then
                If Session.Item("useMetricValues") Then
                  l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(ConversionFunctions.ConvertUSToMetricValue("GAL", CDbl(l_AdoRs.Item("amod_fuel_cap_std_gal"))), False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
                Else
                  l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(CDbl(l_AdoRs.Item("amod_fuel_cap_std_gal")), False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
                End If
              Else
                l_objBuilder.Append("<td valign='middle' align='right'>0&nbsp;</td></tr><tr>" & vbCrLf)
              End If
            Else
              l_objBuilder.Append("<td valign='middle' align='right'>0&nbsp;</td></tr><tr>" & vbCrLf)
            End If

            If Not IsDBNull(l_AdoRs.Item("amod_fuel_cap_opt_weight")) Then
              If CDbl(l_AdoRs.Item("amod_fuel_cap_opt_weight")) > 0 Then
                If Session.Item("useMetricValues") Then
                  l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(ConversionFunctions.ConvertUSToMetricValue("LBS", CDbl(l_AdoRs.Item("amod_fuel_cap_opt_weight"))), False, True, False) & "&nbsp;</td></tr><tr class='alt_row'>" & vbCrLf)
                Else
                  l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(CDbl(l_AdoRs.Item("amod_fuel_cap_opt_weight")), False, True, False) & "&nbsp;</td></tr><tr class='alt_row'>" & vbCrLf)
                End If
              Else
                l_objBuilder.Append("<td valign='middle' align='right'>0&nbsp;</td></tr><tr class='alt_row'>" & vbCrLf)
              End If
            Else
              l_objBuilder.Append("<td valign='middle' align='right'>0&nbsp;</td></tr><tr class='alt_row'>" & vbCrLf)
            End If

            If Not IsDBNull(l_AdoRs.Item("amod_fuel_cap_opt_gal")) Then
              If CDbl(l_AdoRs.Item("amod_fuel_cap_opt_gal")) > 0 Then
                If Session.Item("useMetricValues") Then
                  l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(ConversionFunctions.ConvertUSToMetricValue("GAL", CDbl(l_AdoRs.Item("amod_fuel_cap_opt_gal"))), False, True, False) & "&nbsp;</td></tr>" & vbCrLf)
                Else
                  l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(CDbl(l_AdoRs.Item("amod_fuel_cap_opt_gal")), False, True, False) & "&nbsp;</td></tr>" & vbCrLf)
                End If
              Else
                l_objBuilder.Append("<td valign='middle' align='right'>0&nbsp;</td></tr>" & vbCrLf)
              End If
            Else
              l_objBuilder.Append("<td valign='middle' align='right'>0&nbsp;</td></tr>" & vbCrLf)
            End If

            l_objBuilder.Append("<tr><td>&nbsp;</td></tr><tr class='alt_row'>" & vbCrLf)

            If Session.Item("useMetricValues") Then
              l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(ConversionFunctions.ConvertUSToMetricValue("LBS", CDbl(l_AdoRs.Item("amod_max_ramp_weight"))), False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
            Else
              l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(CDbl(l_AdoRs.Item("amod_max_ramp_weight")), False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
            End If

            If Session.Item("useMetricValues") Then
              l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(ConversionFunctions.ConvertUSToMetricValue("LBS", CDbl(l_AdoRs.Item("amod_max_takeoff_weight"))), False, True, False) & "&nbsp;</td></tr><tr class='alt_row'>" & vbCrLf)
            Else
              l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(CDbl(l_AdoRs.Item("amod_max_takeoff_weight")), False, True, False) & "&nbsp;</td></tr><tr class='alt_row'>" & vbCrLf)
            End If

            If sAirframeType = "F" Then
              If Session.Item("useMetricValues") Then
                l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(ConversionFunctions.ConvertUSToMetricValue("LBS", CDbl(l_AdoRs.Item("amod_zero_fuel_weight"))), False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
              Else
                l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(CDbl(l_AdoRs.Item("amod_zero_fuel_weight")), False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
              End If
            ElseIf sAirframeType = "R" Then
              If Session.Item("useMetricValues") Then
                l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(ConversionFunctions.ConvertUSToMetricValue("LBS", CDbl(l_AdoRs.Item("amod_weight_eow"))), False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
              Else
                l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(CDbl(l_AdoRs.Item("amod_weight_eow")), False, True, False) & "&nbsp;</td></tr><tr>")
              End If
            End If

            If Session.Item("useMetricValues") Then
              l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(ConversionFunctions.ConvertUSToMetricValue("LBS", CDbl(l_AdoRs.Item("amod_basic_op_weight"))), False, True, False) & "&nbsp;</td></tr><tr class='alt_row'>" & vbCrLf)
            Else
              l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(CDbl(l_AdoRs.Item("amod_basic_op_weight")), False, True, False) & "&nbsp;</td></tr><tr class='alt_row'>" & vbCrLf)
            End If

            If Session.Item("useMetricValues") Then
              l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(ConversionFunctions.ConvertUSToMetricValue("LBS", CDbl(l_AdoRs.Item("amod_max_landing_weight"))), False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
            Else
              l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(CDbl(l_AdoRs.Item("amod_max_landing_weight")), False, True, False) & "&nbsp;</td></tr>" & vbCrLf)
            End If

            l_objBuilder.Append("<tr><td>&nbsp;</td></tr><tr class='alt_row'>" & vbCrLf)

            If sAirframeType = "F" Then
              If Session.Item("useMetricValues") Then
                If Not IsDBNull(l_AdoRs.Item("amod_stall_vs")) Then
                  If CDbl(l_AdoRs.Item("amod_stall_vs")) > 0 Then
                    l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(ConversionFunctions.ConvertUSToMetricValue("KN", CDbl(l_AdoRs.Item("amod_stall_vs"))), False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
                  Else
                    l_objBuilder.Append("<td valign='middle' align='right'>0&nbsp;</td></tr><tr>" & vbCrLf)
                  End If
                Else
                  l_objBuilder.Append("<td valign='middle' align='right'>0&nbsp;</td></tr><tr>" & vbCrLf)
                End If
              Else
                If Not IsDBNull(l_AdoRs.Item("amod_stall_vs")) Then
                  If CDbl(l_AdoRs.Item("amod_stall_vs")) > 0 Then
                    l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(CDbl(l_AdoRs.Item("amod_stall_vs")), False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
                  Else
                    l_objBuilder.Append("<td valign='middle' align='right'>0&nbsp;</td></tr><tr>" & vbCrLf)
                  End If
                Else
                  l_objBuilder.Append("<td valign='middle' align='right'>0&nbsp;</td></tr><tr>" & vbCrLf)
                End If
              End If
            ElseIf sAirframeType = "R" And bHasManyAirFrames Then
              l_objBuilder.Append("<td valign='middle' align='right'>&nbsp;</td></tr><tr>" & vbCrLf)
            End If

            If sAirframeType = "F" Then
              If Session.Item("useMetricValues") Then
                If Not IsDBNull(l_AdoRs.Item("amod_stall_vso")) Then
                  If CDbl(l_AdoRs.Item("amod_stall_vso")) > 0 Then
                    l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(ConversionFunctions.ConvertUSToMetricValue("KN", CDbl(l_AdoRs.Item("amod_stall_vso"))), False, True, False) & "&nbsp;</td></tr><tr class='alt_row'>" & vbCrLf)
                  Else
                    l_objBuilder.Append("<td valign='middle' align='right'>0&nbsp;</td></tr><tr class='alt_row'>" & vbCrLf)
                  End If
                Else
                  l_objBuilder.Append("<td valign='middle' align='right'>0&nbsp;</td></tr><tr class='alt_row'>" & vbCrLf)
                End If
              Else
                If Not IsDBNull(l_AdoRs.Item("amod_stall_vso")) Then
                  If CDbl(l_AdoRs.Item("amod_stall_vso")) > 0 Then
                    l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(CDbl(l_AdoRs.Item("amod_stall_vso")), False, True, False) & "&nbsp;</td></tr><tr class='alt_row'>" & vbCrLf)
                  Else
                    l_objBuilder.Append("<td valign='middle' align='right'>0&nbsp;</td></tr><tr class='alt_row'>" & vbCrLf)
                  End If
                Else
                  l_objBuilder.Append("<td valign='middle' align='right'>0&nbsp;</td></tr><tr class='alt_row'>" & vbCrLf)
                End If
              End If
            ElseIf sAirframeType = "R" And bHasManyAirFrames Then
              l_objBuilder.Append("<td valign='middle' align='right'>&nbsp;</td></tr><tr class='alt_row'>" & vbCrLf)
            End If

            If Session.Item("useMetricValues") Then
              l_objBuilder.Append("<td valign='middle' align='right'>" + FormatNumber(ConversionFunctions.ConvertUSToMetricValue("KN", CDbl(l_AdoRs.Item("amod_cruis_speed").ToString)), False, True, False) + "&nbsp;</td></tr><tr>")
              l_objBuilder.Append("<td valign='middle' align='right'>" + FormatNumber(ConversionFunctions.ConvertUSToMetricValue("KN", CDbl(l_AdoRs.Item("amod_max_speed").ToString)), False, True, False) + "&nbsp;</td></tr><tr>")

              If sAirframeType.ToUpper.Contains("F") And Not bHasManyAirFrames Then
                'do nothing
              ElseIf sAirframeType.ToUpper.Contains("R") And Not bHasManyAirFrames Then
                If Not IsDBNull(l_AdoRs("amod_vne_maxop_speed")) Then
                  If CDbl(l_AdoRs.Item("amod_vne_maxop_speed").ToString) > 0 Then
                    l_objBuilder.Append("<td valign='middle' align='right'>" + FormatNumber(ConversionFunctions.ConvertUSToMetricValue("KN", CDbl(l_AdoRs.Item("amod_vne_maxop_speed").ToString)), False, True, False) + "&nbsp;</td></tr><tr>")
                  Else
                    l_objBuilder.Append("<td valign='middle' align='right'>0&nbsp;</td></tr><tr>")
                  End If
                Else
                  l_objBuilder.Append("<td valign='middle' align='right'>0&nbsp;</td></tr><tr>")
                End If
              ElseIf bHasManyAirFrames Then
                If Not IsDBNull(l_AdoRs("amod_vne_maxop_speed")) Then
                  If CDbl(l_AdoRs.Item("amod_vne_maxop_speed").ToString) > 0 Then
                    l_objBuilder.Append("<td valign='middle' align='right'>" + FormatNumber(ConversionFunctions.ConvertUSToMetricValue("KN", CDbl(l_AdoRs.Item("amod_vne_maxop_speed").ToString)), False, True, False) + "&nbsp;</td></tr><tr>")
                  Else
                    l_objBuilder.Append("<td valign='middle' align='right'>0&nbsp;</td></tr><tr>")
                  End If
                Else
                  l_objBuilder.Append("<td valign='middle' align='right'>0&nbsp;</td></tr><tr>")
                End If
              End If

              If Not IsDBNull(l_AdoRs("amod_v1_takeoff_speed")) Then
                If CDbl(l_AdoRs.Item("amod_v1_takeoff_speed").ToString) > 0 Then
                  l_objBuilder.Append("<td valign='middle' align='right'>" + FormatNumber(ConversionFunctions.ConvertUSToMetricValue("KN", CDbl(l_AdoRs.Item("amod_v1_takeoff_speed").ToString)), False, True, False) + "&nbsp;</td></tr><tr class='alt_row'>")
                Else
                  l_objBuilder.Append("<td valign='middle' align='right'>0&nbsp;</td></tr><tr class='alt_row'>")
                End If
              Else
                l_objBuilder.Append("<td valign='middle' align='right'>0&nbsp;</td></tr><tr class='alt_row'>")
              End If

              If Not IsDBNull(l_AdoRs("amod_vfe_max_flap_extended_speed")) Then
                If CDbl(l_AdoRs.Item("amod_vfe_max_flap_extended_speed").ToString) > 0 Then
                  l_objBuilder.Append("<td valign='middle' align='right'>" + FormatNumber(ConversionFunctions.ConvertUSToMetricValue("KN", CDbl(l_AdoRs.Item("amod_vfe_max_flap_extended_speed").ToString)), False, True, False) + "&nbsp;</td></tr><tr>")
                Else
                  l_objBuilder.Append("<td valign='middle' align='right'>0&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("KN") + ")</td></tr><tr>")
                End If
              Else
                l_objBuilder.Append("<td valign='middle' align='right'>0&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("KN") + ")</td></tr><tr>")
              End If

              If Not IsDBNull(l_AdoRs("amod_vle_max_landing_gear_ext_speed")) Then
                If CDbl(l_AdoRs.Item("amod_vle_max_landing_gear_ext_speed").ToString) > 0 Then
                  l_objBuilder.Append("<td valign='middle' align='right'>" + FormatNumber(ConversionFunctions.ConvertUSToMetricValue("KN", CDbl(l_AdoRs.Item("amod_vle_max_landing_gear_ext_speed").ToString)), False, True, False) + "&nbsp;</td></tr>")
                Else
                  l_objBuilder.Append("<td valign='middle' align='right'>0&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("KN") + ")</td></tr>")
                End If
              Else
                l_objBuilder.Append("<td valign='middle' align='right'>0&nbsp;(" + ConversionFunctions.TranslateUSMetricUnitsShort("KN") + ")</td></tr>")
              End If

            Else

              l_objBuilder.Append("<td valign='middle' align='right'>" + FormatNumber(CDbl(l_AdoRs.Item("amod_cruis_speed").ToString), False, True, False) + "&nbsp;</td></tr><tr>")
              l_objBuilder.Append("<td valign='middle' align='right'>" + FormatNumber(CDbl(l_AdoRs.Item("amod_max_speed").ToString), False, True, False) + "&nbsp;</td></tr><tr>")

              If sAirframeType.ToUpper.Contains("F") And Not bHasManyAirFrames Then
                'do nothing
              ElseIf sAirframeType.ToUpper.Contains("R") And Not bHasManyAirFrames Then
                If Not IsDBNull(l_AdoRs("amod_vne_maxop_speed")) Then
                  If CDbl(l_AdoRs.Item("amod_vne_maxop_speed").ToString) > 0 Then
                    l_objBuilder.Append("<td valign='middle' align='right'>" + FormatNumber(CDbl(l_AdoRs.Item("amod_vne_maxop_speed").ToString), False, True, False) + "&nbsp;</td></tr><tr>")
                  Else
                    l_objBuilder.Append("<td valign='middle' align='right'>0&nbsp;(kn)</td></tr><tr>")
                  End If
                Else
                  l_objBuilder.Append("<td valign='middle' align='right'>0&nbsp;(kn)</td></tr><tr>")
                End If
              ElseIf bHasManyAirFrames Then
                If Not IsDBNull(l_AdoRs("amod_vne_maxop_speed")) Then
                  If CDbl(l_AdoRs.Item("amod_vne_maxop_speed").ToString) > 0 Then
                    l_objBuilder.Append("<td valign='middle' align='right'>" + FormatNumber(CDbl(l_AdoRs.Item("amod_vne_maxop_speed").ToString), False, True, False) + "&nbsp;</td></tr><tr>")
                  Else
                    l_objBuilder.Append("<td valign='middle' align='right'>0&nbsp;(kn)</td></tr><tr>")
                  End If
                Else
                  l_objBuilder.Append("<td valign='middle' align='right'>0&nbsp;(kn)</td></tr><tr>")
                End If
              End If

              If Not IsDBNull(l_AdoRs("amod_v1_takeoff_speed")) Then
                If CDbl(l_AdoRs.Item("amod_v1_takeoff_speed").ToString) > 0 Then
                  l_objBuilder.Append("<td valign='middle' align='right'>" + FormatNumber(CDbl(l_AdoRs.Item("amod_v1_takeoff_speed").ToString), False, True, False) + "&nbsp;</td></tr><tr class='alt_row'>")
                Else
                  l_objBuilder.Append("<td valign='middle' align='right'>0&nbsp;(kn)</td></tr><tr class='alt_row'>")
                End If
              Else
                l_objBuilder.Append("<td valign='middle' align='right'>0&nbsp;(kn)</td></tr><tr class='alt_row'>")
              End If

              If Not IsDBNull(l_AdoRs("amod_vfe_max_flap_extended_speed")) Then
                If CDbl(l_AdoRs.Item("amod_vfe_max_flap_extended_speed").ToString) > 0 Then
                  l_objBuilder.Append("<td valign='middle' align='right'>" + FormatNumber(CDbl(l_AdoRs.Item("amod_vfe_max_flap_extended_speed").ToString), False, True, False) + "&nbsp;</td></tr><tr>")
                Else
                  l_objBuilder.Append("<td valign='middle' align='right'>0&nbsp;(kn)</td></tr><tr>")
                End If
              Else
                l_objBuilder.Append("<td valign='middle' align='right'>0&nbsp;(kn)</td></tr><tr>")
              End If

              If Not IsDBNull(l_AdoRs("amod_vle_max_landing_gear_ext_speed")) Then
                If CDbl(l_AdoRs.Item("amod_vle_max_landing_gear_ext_speed").ToString) > 0 Then
                  l_objBuilder.Append("<td valign='middle' align='right'>" + FormatNumber(CDbl(l_AdoRs.Item("amod_vle_max_landing_gear_ext_speed").ToString), False, True, False) + "&nbsp;</td></tr>")
                Else
                  l_objBuilder.Append("<td valign='middle' align='right'>0&nbsp;(kn)</td></tr>")
                End If
              Else
                l_objBuilder.Append("<td valign='middle' align='right'>0&nbsp;(kn)</td></tr>")
              End If

            End If

            l_objBuilder.Append("<tr class='alt_row'><td>&nbsp;</td></tr><tr>")

            If Not IsDBNull(l_AdoRs.Item("amod_ifr_certification")) Then
              If Not String.IsNullOrEmpty(l_AdoRs.Item("amod_ifr_certification").ToString.Trim) Then
                l_objBuilder.Append("<td valign='middle' align='right'>" + l_AdoRs.Item("amod_ifr_certification").ToString.Trim + "&nbsp;</td></tr>" & vbCrLf)
              Else
                l_objBuilder.Append("<td valign='middle' align='right'>-</td></tr>" & vbCrLf)
              End If
            Else
              l_objBuilder.Append("<td valign='middle' align='right'>-</td></tr>" & vbCrLf)
            End If

            l_objBuilder.Append("<tr class='alt_row'><td>&nbsp;</td></tr><tr>" & vbCrLf)

            If Session.Item("useMetricValues") Then
              If CDbl(l_AdoRs.Item("amod_climb_normal_feet")) > 0 Then
                l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(ConversionFunctions.ConvertUSToMetricValue("FPM", CDbl(l_AdoRs.Item("amod_climb_normal_feet"))), 1, False, True, False) & "&nbsp;</td></tr><tr class='alt_row'>" & vbCrLf)
              Else
                l_objBuilder.Append("<td valign='middle' align='right'>0&nbsp;</td></tr><tr class='alt_row'>" & vbCrLf)
              End If

              If CDbl(l_AdoRs.Item("amod_climb_engout_feet")) > 0 Then
                l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(ConversionFunctions.ConvertUSToMetricValue("FPM", CDbl(l_AdoRs.Item("amod_climb_engout_feet"))), 1, False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
              Else
                l_objBuilder.Append("<td valign='middle' align='right'>0&nbsp;</td></tr><tr>" & vbCrLf)
              End If

              If CDbl(l_AdoRs.Item("amod_ceiling_feet")) > 0 Then
                l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(ConversionFunctions.ConvertUSToMetricValue("FT", CDbl(l_AdoRs.Item("amod_ceiling_feet"))), False, True, False) & "&nbsp;</td></tr>" & vbCrLf)
              Else
                l_objBuilder.Append("<td valign='middle' align='right'>0&nbsp;</td></tr>" & vbCrLf)
              End If

            Else
              If CDbl(l_AdoRs.Item("amod_climb_normal_feet")) > 0 Then
                l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(CDbl(l_AdoRs.Item("amod_climb_normal_feet")), False, True, False) & "&nbsp;</td></tr><tr class='alt_row'>" & vbCrLf)
              Else
                l_objBuilder.Append("<td valign='middle' align='right'>0&nbsp;</td></tr><tr class='alt_row'>" & vbCrLf)
              End If

              If CDbl(l_AdoRs.Item("amod_climb_engout_feet")) > 0 Then
                l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(CDbl(l_AdoRs.Item("amod_climb_engout_feet")), False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
              Else
                l_objBuilder.Append("<td valign='middle' align='right'>0&nbsp;</td></tr><tr>" & vbCrLf)
              End If

              If CDbl(l_AdoRs.Item("amod_ceiling_feet")) > 0 Then
                l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(CDbl(l_AdoRs.Item("amod_ceiling_feet")), False, True, False) & "&nbsp;</td></tr>" & vbCrLf)
              Else
                l_objBuilder.Append("<td valign='middle' align='right'>0&nbsp;</td></tr>" & vbCrLf)
              End If
            End If

            If sAirframeType = "F" And bHasManyAirFrames Then
              l_objBuilder.Append("<tr class='alt_row'><td valign='middle' align='right'>&nbsp;</td></tr>" & vbCrLf)
              l_objBuilder.Append("<tr><td valign='middle' align='right'>&nbsp;</td></tr>" & vbCrLf)
            ElseIf sAirframeType = "R" Then

              If Not IsDBNull(l_AdoRs.Item("amod_climb_hoge")) Then
                If Session.Item("useMetricValues") Then
                  l_objBuilder.Append("<tr class='alt_row'><td valign='middle' align='right'>" & FormatNumber(ConversionFunctions.ConvertUSToMetricValue("FT", CDbl(l_AdoRs.Item("amod_climb_hoge"))), False, True, False) & "&nbsp;</td></tr>" & vbCrLf)
                Else
                  l_objBuilder.Append("<tr class='alt_row'><td valign='middle' align='right'>" & FormatNumber(CDbl(l_AdoRs.Item("amod_climb_hoge")), False, True, False) & "&nbsp;</td></tr>" & vbCrLf)
                End If
              Else
                l_objBuilder.Append("<tr class='alt_row'><td valign='middle' align='right'>&nbsp;</td></tr>")
              End If

              If Not IsDBNull(l_AdoRs.Item("amod_climb_hige")) Then
                If Session.Item("useMetricValues") Then
                  l_objBuilder.Append("<tr><td valign='middle' align='right'>" & FormatNumber(ConversionFunctions.ConvertUSToMetricValue("FT", CDbl(l_AdoRs.Item("amod_climb_hige"))), False, True, False) & "&nbsp;</td></tr>" & vbCrLf)
                Else
                  l_objBuilder.Append("<tr><td valign='middle' align='right'>" & FormatNumber(CDbl(l_AdoRs.Item("amod_climb_hige")), False, True, False) & "&nbsp;</td></tr>" & vbCrLf)
                End If
              Else
                l_objBuilder.Append("<tr><td valign='middle' align='right'>&nbsp;</td></tr>")
              End If

            End If

            If sAirframeType = "F" Then

              temp_op_cost_string += "<tr class='alt_row'><td>&nbsp;</td></tr>" & vbCrLf
              If Session.Item("useMetricValues") Then
                temp_op_cost_string += "<tr><td valign='middle' align='right'>" & FormatNumber(ConversionFunctions.ConvertUSToMetricValue("FT", CDbl(l_AdoRs.Item("amod_field_length"))), False, True, False) & "&nbsp;</td></tr>" & vbCrLf
              Else
                temp_op_cost_string += "<tr><td valign='middle' align='right'>" & FormatNumber(CLng(l_AdoRs.Item("amod_field_length")), False, True, False) & "&nbsp;</td></tr>" & vbCrLf
              End If
            ElseIf sAirframeType = "R" And bHasManyAirFrames Then

              temp_op_cost_string += "<tr class='alt_row'><th>&nbsp;</th></tr>"
              temp_op_cost_string += "<tr><td valign='middle' align='right'>&nbsp;</td></tr>" & vbCrLf
            End If

            temp_op_cost_string += "<tr class='alt_row'><td>&nbsp;</td></tr>" & vbCrLf

            temp_op_cost_string += "<tr><td valign='middle' align='right'>" & FormatNumber(CLng(l_AdoRs.Item("amod_takeoff_ali")), False, True, False) & "&nbsp;</td></tr>" & vbCrLf
            temp_op_cost_string += "<tr class='alt_row'><td valign='middle' align='right'>" & FormatNumber(CLng(l_AdoRs.Item("amod_takeoff_500")), False, True, False) & "&nbsp;</td></tr>" & vbCrLf

            If Session.Item("useMetricValues") Then

              If Not IsDBNull(l_AdoRs("amod_max_range_miles")) Then
                If CDbl(l_AdoRs.Item("amod_max_range_miles").ToString) > 0 Then
                  temp_op_cost_string += "<tr><td valign='middle' align='right'>" + FormatNumber(ConversionFunctions.ConvertUSToMetricValue("NM", CDbl(l_AdoRs.Item("amod_max_range_miles").ToString)), 1, False, True, False) + "&nbsp;</td></tr>"
                Else
                  temp_op_cost_string += "<tr><td valign='middle' align='right'>0&nbsp;</td></tr>"
                End If
              Else
                temp_op_cost_string += "<tr><td valign='middle' align='right'>0&nbsp;</td></tr>"
              End If

              If Not IsDBNull(l_AdoRs("amod_range_tanks_full")) Then
                If CDbl(l_AdoRs.Item("amod_range_tanks_full").ToString) > 0 Then
                  temp_op_cost_string += "<tr><td valign='middle' align='right'>" + FormatNumber(ConversionFunctions.ConvertUSToMetricValue("NM", CDbl(l_AdoRs.Item("amod_range_tanks_full").ToString)), 1, False, True, False) + "&nbsp;</td></tr>"
                Else
                  temp_op_cost_string += "<tr><td valign='middle' align='right'>0&nbsp;</td></tr>"
                End If
              Else
                temp_op_cost_string += "<tr><td valign='middle' align='right'>0&nbsp;</td></tr>"
              End If

              If Not IsDBNull(l_AdoRs("amod_range_seats_full")) Then
                If CDbl(l_AdoRs.Item("amod_range_seats_full").ToString) > 0 Then
                  temp_op_cost_string += "<tr><td valign='middle' align='right'>" + FormatNumber(ConversionFunctions.ConvertUSToMetricValue("NM", CDbl(l_AdoRs.Item("amod_range_seats_full").ToString)), 1, False, True, False) + "&nbsp;</td></tr>"
                Else
                  temp_op_cost_string += "<tr><td valign='middle' align='right'>0&nbsp;</td></tr>"
                End If
              Else
                temp_op_cost_string += "<tr><td valign='middle' align='right'>0&nbsp;</td></tr>"
              End If

              If Not IsDBNull(l_AdoRs("amod_range_4_passenger")) Then
                If CDbl(l_AdoRs.Item("amod_range_4_passenger").ToString) > 0 Then
                  temp_op_cost_string += "<tr><td valign='middle' align='right'>" + FormatNumber(ConversionFunctions.ConvertUSToMetricValue("NM", CDbl(l_AdoRs.Item("amod_range_4_passenger").ToString)), 1, False, True, False) + "&nbsp;</td></tr>"
                Else
                  temp_op_cost_string += "<tr><td valign='middle' align='right'>" + Constants.cHyphen + "&nbsp;</td></tr>"
                End If
              Else
                temp_op_cost_string += "<tr><td valign='middle' align='right'>" + Constants.cHyphen + "&nbsp;</td></tr>"
              End If

              If Not IsDBNull(l_AdoRs("amod_range_8_passenger")) Then
                If CDbl(l_AdoRs.Item("amod_range_8_passenger").ToString) > 0 Then
                  temp_op_cost_string += "<tr class='alt_row'><td valign='middle' align='right'>" + FormatNumber(ConversionFunctions.ConvertUSToMetricValue("NM", CDbl(l_AdoRs.Item("amod_range_8_passenger").ToString)), 1, False, True, False) + "&nbsp;</td></tr>"
                Else
                  temp_op_cost_string += "<tr class='alt_row'><td valign='middle' align='right'>" + Constants.cHyphen + "&nbsp;</td></tr>"
                End If
              Else
                temp_op_cost_string += "<tr class='alt_row'><td valign='middle' align='right'>" + Constants.cHyphen + "&nbsp;</td></tr>"
              End If

            Else

              If Not IsDBNull(l_AdoRs("amod_max_range_miles")) Then
                If CDbl(l_AdoRs.Item("amod_max_range_miles").ToString) > 0 Then
                  temp_op_cost_string += "<tr><td valign='middle' align='right'>" + FormatNumber(CDbl(l_AdoRs.Item("amod_max_range_miles").ToString), False, True, False) + "&nbsp;</td></tr>"
                Else
                  temp_op_cost_string += "<tr><td valign='middle' align='right'>0&nbsp;</td></tr>"
                End If
              Else
                temp_op_cost_string += "<tr><td valign='middle' align='right'>0&nbsp;</td></tr>"
              End If

              If Not IsDBNull(l_AdoRs("amod_range_tanks_full")) Then
                If CDbl(l_AdoRs.Item("amod_range_tanks_full").ToString) > 0 Then
                  temp_op_cost_string += "<tr><td valign='middle' align='right'>" + FormatNumber(CDbl(l_AdoRs.Item("amod_range_tanks_full").ToString), False, True, False) + "&nbsp;</td></tr>"
                Else
                  temp_op_cost_string += "<tr><td valign='middle' align='right'>0&nbsp;</td></tr>"
                End If
              Else
                temp_op_cost_string += "<tr><td valign='middle' align='right'>0&nbsp;</td></tr>"
              End If

              If Not IsDBNull(l_AdoRs("amod_range_seats_full")) Then
                If CDbl(l_AdoRs.Item("amod_range_seats_full").ToString) > 0 Then
                  temp_op_cost_string += "<tr><td valign='middle' align='right'>" + FormatNumber(CDbl(l_AdoRs.Item("amod_range_seats_full").ToString), False, True, False) + "&nbsp;</td></tr>"
                Else
                  temp_op_cost_string += "<tr><td valign='middle' align='right'>0&nbsp;</td></tr>"
                End If
              Else
                temp_op_cost_string += "<tr><td valign='middle' align='right'>0&nbsp;</td></tr>"
              End If

              If Not IsDBNull(l_AdoRs("amod_range_4_passenger")) Then
                If CDbl(l_AdoRs.Item("amod_range_4_passenger").ToString) > 0 Then
                  temp_op_cost_string += "<tr><td valign='middle' align='right'>" + FormatNumber(CDbl(l_AdoRs.Item("amod_range_4_passenger").ToString), False, True, False) + "&nbsp;</td></tr>"
                Else
                  temp_op_cost_string += "<tr><td valign='middle' align='right'>" + Constants.cHyphen + "&nbsp;</td></tr>"
                End If
              Else
                temp_op_cost_string += "<tr><td valign='middle' align='right'>" + Constants.cHyphen + "&nbsp;</td></tr>"
              End If

              If Not IsDBNull(l_AdoRs("amod_range_8_passenger")) Then
                If CDbl(l_AdoRs.Item("amod_range_8_passenger").ToString) > 0 Then
                  temp_op_cost_string += "<tr class='alt_row'><td valign='middle' align='right'>" + FormatNumber(CDbl(l_AdoRs.Item("amod_range_8_passenger").ToString), False, True, False) + "&nbsp;</td></tr>"
                Else
                  temp_op_cost_string += "<tr class='alt_row'><td valign='middle' align='right'>" + Constants.cHyphen + "&nbsp;</td></tr>"
                End If
              Else
                temp_op_cost_string += "<tr class='alt_row'><td valign='middle' align='right'>" + Constants.cHyphen + "&nbsp;</td></tr>"
              End If

            End If

            l_objBuilder.Append(temp_op_cost_string)
            string_from_op_costs_for_range += temp_op_cost_string

            temp_op_cost_string = ""

            l_objBuilder.Append("<tr><td>&nbsp;</td></tr><tr valign='top' class='alt_row'>" & vbCrLf)

            l_objBuilder.Append("<td valign='top' align='right'>" & FormatNumber(CLng(l_AdoRs.Item("amod_number_of_engines")), False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
            l_objBuilder.Append("<td valign='top' align='right' nowrap>" & GetEngines(l_AdoRs.Item("amod_id"), nMAXEngines))


            For i = CLng(GetEnginesNumberForSpace(l_AdoRs.Item("amod_id"), 0)) To number_of_engine_types - 1
              l_objBuilder.Append("<br>&nbsp;")
            Next

            l_objBuilder.Append("</td></tr><tr class='alt_row'>" & vbCrLf)


            If Session.Item("useMetricValues") Then
              l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(CDbl(ConversionFunctions.ConvertUSToMetricValue("LBS", l_AdoRs.Item("amod_engine_thrust_lbs"))), False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
              If Not IsDBNull(l_AdoRs.Item("amod_engine_shaft")) Then
                If CDbl(l_AdoRs.Item("amod_engine_shaft")) > 0 Then
                  l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(CDbl(ConversionFunctions.ConvertUSToMetricValue("HP", l_AdoRs.Item("amod_engine_shaft"))), False, True, False) & "&nbsp;</td></tr><tr class='alt_row'>" & vbCrLf)
                Else
                  l_objBuilder.Append("<td valign='middle' align='right'>0&nbsp;</td></tr><tr class='alt_row'>" & vbCrLf)
                End If
              Else
                l_objBuilder.Append("<td valign='middle' align='right'>0&nbsp;</td></tr><tr class='alt_row'>" & vbCrLf)
              End If
            Else
              l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(CDbl(l_AdoRs.Item("amod_engine_thrust_lbs")), False, True, False) & "&nbsp;</td></tr><tr>" & vbCrLf)
              If Not IsDBNull(l_AdoRs.Item("amod_engine_shaft")) Then
                If CDbl(l_AdoRs.Item("amod_engine_shaft")) > 0 Then
                  l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(CDbl(l_AdoRs.Item("amod_engine_shaft")), False, True, False) & "&nbsp;</td></tr><tr class='alt_row'>" & vbCrLf)
                Else
                  l_objBuilder.Append("<td valign='middle' align='right'>0&nbsp;</td></tr><tr class='alt_row'>" & vbCrLf)
                End If
              Else
                l_objBuilder.Append("<td valign='middle' align='right'>0&nbsp;</td></tr><tr class='alt_row'>" & vbCrLf)
              End If
            End If

            l_objBuilder.Append("<td valign='middle' align='right'>" & FormatNumber(CDbl(l_AdoRs.Item("amod_engine_com_tbo_hrs")), False, True, False) & "&nbsp;</td></tr>" & vbCrLf)
            '''''''''''''''''''''''''''''''''''''''''''''''''''
            ' End the left column
            '''''''''''''''''''''''''''''''''''''''''''''''''''

            l_objBuilder.Append("</table>" & vbCrLf)



          Loop
        End If

        l_AdoRs.Close()
        l_AdoRs = Nothing
      End If




      Build_PerformanceSpecifications = l_objBuilder.ToString()


    Catch ex As Exception
    Finally
      SqlConn.Close()
      SqlConn.Dispose()
    End Try
  End Function

  Public Function GetEngines(ByVal inModelID, ByVal nMAXEngines) As String
    Dim tmpString As String = ""
    Dim adoRs As System.Data.SqlClient.SqlDataReader : adoRs = Nothing
    Dim Query As String : Query = ""
    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Try


      SqlConn.ConnectionString = Application.Item("crmJetnetDatabase") 'My.Settings.DEFAULT_LIVE_MSSQL_DEBUG

      SqlConn.Open()

      Dim nLoop As Long = 0
      Dim nCurrentNumber As Integer = 0
      tmpString = ""
      Dim sSeparator As String = ""

      Query = "SELECT ameng_engine_name, ameng_seq_no"
      Query = Query & " FROM Aircraft_Model_Engine WITH(NOLOCK) WHERE ameng_amod_id = " & CStr(inModelID)
      Query = Query & " GROUP BY ameng_seq_no, ameng_engine_name ORDER BY ameng_seq_no, ameng_engine_name"

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlCommand.CommandText = Query

      adoRs = SqlCommand.ExecuteReader()

      If (adoRs.HasRows) Then
        Do While adoRs.Read
          tmpString = tmpString & sSeparator & Trim(adoRs("ameng_engine_name")) & "&nbsp;"
          sSeparator = "<br>"
          nCurrentNumber = nCurrentNumber + 1
          'adoRs.Read()
        Loop
        adoRs.Close()
      End If

      adoRs = Nothing
      ' SqlConn.Close()
      '    If nCurrentNumber <> nMAXEngines Then
      ' For nLoop = nCurrentNumber To nMAXEngines
      '  If nLoop < nMAXEngines Then
      '  tmpString = tmpString & "<br>&nbsp;"
      '  Else
      '   Exit For
      '  End If
      ' Next
      '  End If

    Catch ex As Exception
    Finally
      SqlConn.Close()
      SqlConn.Dispose()
    End Try
    Return tmpString
  End Function
    Public Function GetEnginesNumberForSpace(ByVal inModelID, ByVal nMAXEngines) As Integer
        Dim tmpString As String = ""
        Dim adoRs As System.Data.SqlClient.SqlDataReader : adoRs = Nothing
        Dim Query As String : Query = ""
        Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
        Dim SqlConn As New System.Data.SqlClient.SqlConnection
        Dim SqlCommand As New System.Data.SqlClient.SqlCommand
        Try

            ' THIS FUNCTION WAS CREATED TO RUN ON THE RIGHT COLUMN TO MAKE IT LINE UP WITH THE LEFT
            ' IT RUNS THE SAME QUERY AS GETENGINES FUNCTION BUT ONLY ADJUSTS VARIABLE NUMBER_OF_ENGINE_TYPES WHICH IS THEN LOOPED THROUGH
            ' MSW 8/30/10


            SqlConn.ConnectionString = Application.Item("crmJetnetDatabase") 'My.Settings.DEFAULT_LIVE_MSSQL_DEBUG

            SqlConn.Open()

            Dim nLoop As Long = 0
            Dim nCurrentNumber As Integer = 0
            tmpString = ""
            Dim sSeparator As String = ""
            Dim number_of_engine_types2 As Integer = 0

            Query = "SELECT ameng_engine_name, ameng_seq_no"
            Query = Query & " FROM Aircraft_Model_Engine WITH(NOLOCK) WHERE ameng_amod_id = " & CStr(inModelID)
            Query = Query & " GROUP BY ameng_seq_no, ameng_engine_name ORDER BY ameng_seq_no, ameng_engine_name"

            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = System.Data.CommandType.Text
            SqlCommand.CommandTimeout = 60

            SqlCommand.CommandText = Query

            adoRs = SqlCommand.ExecuteReader()

            If (adoRs.HasRows) Then
                Do While adoRs.Read
                    number_of_engine_types2 = number_of_engine_types2 + 1
                Loop
                adoRs.Close()
            End If

            adoRs = Nothing


            GetEnginesNumberForSpace = number_of_engine_types2
        Catch ex As Exception
        Finally
            SqlConn.Close()
            SqlConn.Dispose()
        End Try

    End Function


#Region "Control Events"
    Private Sub type_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles type.SelectedIndexChanged
        clsGeneral.clsGeneral.Type_Selected_Index_Changed(make, type, Page.IsPostBack)
    End Sub
    Private Sub make_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles make.SelectedIndexChanged
        clsGeneral.clsGeneral.Make_Selected_Index_Changed(model, make, type)
    End Sub
    Private Sub model_type_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles model_type.SelectedIndexChanged
        clsGeneral.clsGeneral.Model_Type_Selected_Index_Changed(type, model_type)
    End Sub
#End Region

    Private Sub default_models_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles default_models.CheckedChanged
        Master.default_models_check_changed(main_pnl)
    End Sub
End Class