Partial Public Class op_costs
  Inherits System.Web.UI.Page
  Dim type_string As String = ""
  Dim times_through_counter As Integer = 0

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Not Page.IsPostBack Then
      If Session.Item("localUser").crmEvo = True Then
        model_evo_swap.Visible = True
        model_crm_swap.Visible = False
        Dim masterPage As main_site = DirectCast(Page.Master, main_site)
        clsGeneral.clsGeneral.Getting_Type_Listbox_Set(New DataTable, model_type, Master, New DataTable, New DataTable, type)
      ElseIf Session.Item("localUser").crmEvo <> True Then 'If an EVO user
        clsGeneral.clsGeneral.populate_models(model_cbo, True, Me, Nothing, Master, True)
        model_crm_swap.Visible = True
        model_evo_swap.Visible = False
      End If
    End If

  End Sub
  Function on_model_selection_search() Handles model_search.Click
    on_model_selection_search = ""
    Dim i As Integer = 0
    Dim j As Integer = 0
    Dim amod_id As Integer = 0
    Dim make_model_string As String = ""
    Dim temp_item_string(4) As String
    Dim number_of_selected As Integer = 0
    Dim len_of_string As Integer = 0
    Dim row_height As Integer = 0
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


    Session.Item("fuelPriceBase") = Get_Fuel_Price()


    Me.Build_OperatingCosts_Label.Text = "<table cellpadding='3' height='200' cellspacing='0'><tr><td align='left' valign='top'><div class='tab_container_div3'>"


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

      Me.Build_OperatingCosts_Label.Text += "<table width='97%' cellpadding='3' cellspacing='2'><tr><td width='25%' align='left'>"
      Me.Build_OperatingCosts_Label.Text += Build_OperatingCosts(0, "", True, type_string)
      Me.Build_OperatingCosts_Label.Text += "</td><td width='20%' align='left'>"

      number_of_selected = 0
      For i = 0 To Me.make.Items.Count - 1
        If Me.make.Items.Item(i).Selected = True Then
          number_of_selected = number_of_selected + 1
          temp_item_string = Split(Me.make.Items.Item(i).Value, "] ")
          If number_of_selected > 1 Then
            type_string += ", "
          End If
          type_string += "'" & Right(temp_item_string(0), temp_item_string(0).Length - 1) & "'"
        End If
      Next


      If all_is_selected Then 'this part should only ever run during the evolution side 
        'number_of_selected = 0
        'For i = 0 To Me.make.Items.Count - 1
        '    If Me.make.Items.Item(i).Selected = True Then
        '        temp_item_string = Split(Me.make.Items.Item(i).Value, "] ")
        '        make_model_string = temp_item_string(1)

        '        ' THIS DOES THE SEARCH BY MAKE ---- so if you pick 2 makes with 9 models it will run throgh twice 
        '        Me.Build_OperatingCosts_Label.Text += Build_OperatingCosts(0, make_model_string, False, type_string)
        '    End If
        'Next

      Else


        number_of_selected = 0
        For i = 0 To model_c.Items.Count - 1
          If model_c.Items(i).Selected = True Or (model_c.SelectedValue = "All" And Session.Item("localUser").crmEvo <> True) Then 'This runs through and hits this function if the model is selected
            'or if All is selected and it's a crm version of the software

            If model_c.Items(i).Value <> "All" Then

              ''If number_of_selected = 8 Then
              ''    Me.Build_OperatingCosts_Label.Text += "</td></tr>"
              ''    Me.Build_OperatingCosts_Label.Text += "<tr bgcolor=white><Td colspan='10'>&nbsp;</td></tr>"
              ''    Me.Build_OperatingCosts_Label.Text += "<tr bgcolor=white><Td colspan='10'>&nbsp;</td></tr>"
              ''    Me.Build_OperatingCosts_Label.Text += "<tr><td width='25%'>"
              ''    Me.Build_OperatingCosts_Label.Text += Build_OperatingCosts(0, "", True, type_string)
              ''    Me.Build_OperatingCosts_Label.Text += "</td><td width='20%'>"
              ''    number_of_selected = 0
              If number_of_selected > 0 Then
                Me.Build_OperatingCosts_Label.Text += "</td><td width='20%' align='left'>"
              End If

              temp_item_string = Split(model_c.Items.Item(i).Value, "|")
              amod_id = temp_item_string(0)
              make_model_string = temp_item_string(1) & " " & temp_item_string(2)

              '' THIS DOES THE SEARCH BY MODEL ---- so if you pick 2 makes with 9 models it will run throgh 9 times 
              Me.Build_OperatingCosts_Label.Text += Build_OperatingCosts(amod_id, make_model_string, False, type_string)

              number_of_selected = number_of_selected + 1
            End If
          End If
        Next

      End If


      Me.Build_OperatingCosts_Label.Text += "</td></tr></table>"
    Else
      Me.Build_OperatingCosts_Label.Text += "<table><tr><td>"
      Me.Build_OperatingCosts_Label.Text += "No Models Selected"
      Me.Build_OperatingCosts_Label.Text += "</td></tr></table>"
    End If
    Me.Build_OperatingCosts_Label.Text += "</div></td></tr></table>"

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
  Private Sub default_models_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles default_models.CheckedChanged
    Master.default_models_check_changed(main_pnl)
  End Sub
#End Region

  Public Function Build_OperatingCosts(ByVal amod_id As Integer, ByVal make_model_name As String, ByVal is_label_column As Boolean, ByVal product_code_from_make As String) As String
    Build_OperatingCosts = ""


    Dim DCRecCount, AFRecCount, ABRecCount, TCND, TCSM, TCST, ANCH, nCount, sTitle
    Dim bFirstOne As Boolean = True
    Dim fuelTotCost As Double = 0
    Dim fuelGalCost As Double = 0
    Dim fuelAddCost As Double = 0
    Dim fuelBurnRate As Double = 0
    Dim avgBlockSpeed As Double = 0
    Dim totalCostPer As Double = 0
    Dim annualMiles As Double = 0
    Dim dfstatmilecost As Double = 0
    Dim dfseatcost As Double = 0
    Dim totalDirCostHR As Double = 0
    Dim annualHrs As Double = 0
    Dim totalFixedDirect As Double = 0
    Dim totalDirCostYR As Double = 0
    Dim dfhourcost As Double = 0
    Dim tmpCDblValue As Double = 0
    Dim sCurrencyName = ""
    Dim sCurrencySymbol = ""
    Dim crewsalaries As Double = 0
    Dim hangercost As Double = 0
    Dim miscoverhead As Double = 0
    Dim depoverhead As Double = 0
    Dim totalfixedcost As Double = 0
    Dim insurancecost As Double = 0
    Dim totalmaintcost As Double = 0
    Dim miscflightcosts As Double = 0
    Dim overhaulcost As Double = 0
    Dim revoverhaulcost As Double = 0

    Dim sCurrencyDate = ""
    Dim nRememberSessionTimeout As Long = 0


    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim localAdoRs As System.Data.SqlClient.SqlDataReader : localAdoRs = Nothing

    Dim Query As String : Query = ""






    Try


      If amod_id = 0 And is_label_column Then
        Build_OperatingCosts = Build_OperatingCosts & "<table border='0' cellpadding='3' width='100%' cellspacing='0'  align='left'><tr valign='top' align='center' class='aircraft_list' valign='top'><td align=left colspan='3' nowrap>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<b>DIRECT COSTS PER HOUR</b></td></tr><tr class='alt_row'>"
        Build_OperatingCosts = Build_OperatingCosts & "<td align='left' width='50%'><u>Fuel</u></td>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
        Build_OperatingCosts = Build_OperatingCosts & "</tr><tr>"
        If Session.Item("useMetricValues") Then
          Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;&nbsp;&nbsp;&nbsp;Fuel Cost Per " & TranslateUSMetricUnitsLong("GAL") & "</td>" & vbCrLf
        Else
          Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;&nbsp;&nbsp;&nbsp;Fuel Cost Per Gallon</td>" & vbCrLf
        End If
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td></tr><tr class='alt_row'>"
        If Session.Item("useMetricValues") Then
          Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;&nbsp;&nbsp;&nbsp;Additive&nbsp;Cost&nbsp;Per&nbsp;" & TranslateUSMetricUnitsLong("GAL") & "</td>" & vbCrLf
        Else
          Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;&nbsp;&nbsp;&nbsp;Additive&nbsp;Cost&nbsp;Per&nbsp;Gallon</td>" & vbCrLf
        End If
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
        Build_OperatingCosts = Build_OperatingCosts & "</tr><tr>" & vbCrLf
        If Session.Item("useMetricValues") Then
          Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;&nbsp;&nbsp;&nbsp;Burn Rate (" & TranslateUSMetricUnitsLong("GAL") & "s Per Hour)</td>" & vbCrLf
        Else
          Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;&nbsp;&nbsp;&nbsp;Burn Rate (Gallons Per Hour)</td>" & vbCrLf
        End If
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
        Build_OperatingCosts = Build_OperatingCosts & "</tr><tr class='alt_row'>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<td><u>Maintenance</u></td>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
        Build_OperatingCosts = Build_OperatingCosts & "</tr><tr>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;&nbsp;&nbsp;&nbsp;Avg Labor Cost Per Flight Hour</td>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
        Build_OperatingCosts = Build_OperatingCosts & "</tr><tr class='alt_row'>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;&nbsp;&nbsp;&nbsp;Avg Parts Per Flight Hour Cost</td>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
        Build_OperatingCosts = Build_OperatingCosts & "</tr><tr>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<td>Engine Overhaul</td>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
        Build_OperatingCosts = Build_OperatingCosts & "</tr><tr class='alt_row'>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<td>Thrust Reverse Overhaul</td>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
        Build_OperatingCosts = Build_OperatingCosts & "</tr><tr>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<td><u>Miscellaneous&nbsp;Flight&nbsp;Expenses</u></td>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
        Build_OperatingCosts = Build_OperatingCosts & "</tr><tr class='alt_row'>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<td> &nbsp;&nbsp;&nbsp;&nbsp;Landing-Parking Fee</td>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
        Build_OperatingCosts = Build_OperatingCosts & "</tr><tr>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;&nbsp;&nbsp;&nbsp;Crew Expenses</td>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
        Build_OperatingCosts = Build_OperatingCosts & "</tr><tr class='alt_row'>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;&nbsp;&nbsp;&nbsp;Supplies-Catering</td>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
        Build_OperatingCosts = Build_OperatingCosts & "</tr><tr>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<td><b>Total Direct Costs</b></td>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
        Build_OperatingCosts = Build_OperatingCosts & "</tr><tr class='alt_row'>" & vbCrLf
        If Session.Item("useMetricValues") Then
          Build_OperatingCosts = Build_OperatingCosts & "<td><br>Block&nbsp;Speed&nbsp;" & TranslateUSMetricUnitsLong("SM") & "s&nbsp;Per&nbsp;Hour</td>" & vbCrLf
        Else
          Build_OperatingCosts = Build_OperatingCosts & "<td><br>Block&nbsp;Speed&nbsp;Statute&nbsp;Miles&nbsp;Per&nbsp;Hour</td>" & vbCrLf
        End If
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
        Build_OperatingCosts = Build_OperatingCosts & "</tr><tr>" & vbCrLf
        If Session.Item("useMetricValues") Then
          Build_OperatingCosts = Build_OperatingCosts & "<td>Total Cost Per " & TranslateUSMetricUnitsLong("SM") & "</td>" & vbCrLf
        Else
          Build_OperatingCosts = Build_OperatingCosts & "<td>Total Cost Per Statute Mile</td>" & vbCrLf
        End If
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
        Build_OperatingCosts = Build_OperatingCosts & "</tr><tr class='alt_row'>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<tr><td>&nbsp;</td></tr><tr class='aircraft_list' valign='top'><td align='left' colspan='3' nowrap><b>ANNUAL FIXED COSTS</b></td></tr><tr class='alt_row'>"
        Build_OperatingCosts = Build_OperatingCosts & "<td width='50%'><u>Crew Salaries</u></td>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
        Build_OperatingCosts = Build_OperatingCosts & "</tr><tr>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;&nbsp;&nbsp;&nbsp;Capt. Salary</td>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
        Build_OperatingCosts = Build_OperatingCosts & "</tr><tr class='alt_row'>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;&nbsp;&nbsp;&nbsp;Co-pilot Salary</td>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
        Build_OperatingCosts = Build_OperatingCosts & "</tr><tr>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;&nbsp;&nbsp;&nbsp;Benefits</td>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
        Build_OperatingCosts = Build_OperatingCosts & "</tr><tr class='alt_row'>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<td>Hangar Cost</td>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
        Build_OperatingCosts = Build_OperatingCosts & "</tr><tr>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<td><u>Insurance</u></td>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
        Build_OperatingCosts = Build_OperatingCosts & "</tr><tr class='alt_row'>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;&nbsp;&nbsp;&nbsp;Hull</td>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
        Build_OperatingCosts = Build_OperatingCosts & "</tr><tr>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;&nbsp;&nbsp;&nbsp;Legal Liability</td>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
        Build_OperatingCosts = Build_OperatingCosts & "</tr><tr class='alt_row'>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<td><u>Misc. Overhead</u></td>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
        Build_OperatingCosts = Build_OperatingCosts & "</tr><tr>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;&nbsp;&nbsp;&nbsp;Training</td>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
        Build_OperatingCosts = Build_OperatingCosts & "</tr><tr class='alt_row'>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<td> &nbsp;&nbsp;&nbsp;&nbsp;Modernization</td>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
        Build_OperatingCosts = Build_OperatingCosts & "</tr><tr>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;&nbsp;&nbsp;&nbsp;Nav. Equipment</td>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
        Build_OperatingCosts = Build_OperatingCosts & "</tr><tr class='alt_row'>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<td>Depreciation</td>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
        Build_OperatingCosts = Build_OperatingCosts & "</tr><tr>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<td><b>Total Fixed Costs</b></td>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
        Build_OperatingCosts = Build_OperatingCosts & "</tr><tr class='alt_row'>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<tr class='aircraft_list' valign='top'>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<td colspan='3' nowrap>&nbsp;&nbsp;<b>ANNUAL BUDGET</b></td></tr><tr class='alt_row'>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<td width='50%'>&nbsp;&nbsp;&nbsp;&nbsp;Number of Seats</td>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
        Build_OperatingCosts = Build_OperatingCosts & "</tr><tr>" & vbCrLf
        If Session.Item("useMetricValues") Then
          Build_OperatingCosts = Build_OperatingCosts & "<td width='50%'>&nbsp;&nbsp;&nbsp;&nbsp;" & TranslateUSMetricUnitsLong("M") & "s</td>" & vbCrLf
        Else
          Build_OperatingCosts = Build_OperatingCosts & "<td width='50%'>&nbsp;&nbsp;&nbsp;&nbsp;Miles</td>" & vbCrLf
        End If
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
        Build_OperatingCosts = Build_OperatingCosts & "</tr><tr class='alt_row'>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<td width='50%'>&nbsp;&nbsp;&nbsp;&nbsp;Hours</td>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
        Build_OperatingCosts = Build_OperatingCosts & "</tr><tr>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;&nbsp;<b>Total Direct Costs</b></td>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
        Build_OperatingCosts = Build_OperatingCosts & "</tr><tr class='alt_row'>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;&nbsp;<b>Total Fixed Costs</b></td>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
        Build_OperatingCosts = Build_OperatingCosts & "</tr><tr>" & vbCrLf

                Build_OperatingCosts = Build_OperatingCosts & "<td nowrap='nowrap'>&nbsp;&nbsp;<b><u>Total Cost (Fixed &amp; Direct w/Depreciation)</u></b></td>" & vbCrLf
                Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
        Build_OperatingCosts = Build_OperatingCosts & "</tr><tr class='alt_row'>" & vbCrLf

        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;&nbsp;&nbsp;&nbsp;Cost/Hour</td>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
        Build_OperatingCosts = Build_OperatingCosts & "</tr><tr>" & vbCrLf
        If Session.Item("useMetricValues") Then
          Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;&nbsp;&nbsp;&nbsp;Cost/" & TranslateUSMetricUnitsLong("SM") & "</td>" & vbCrLf
        Else
          Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;&nbsp;&nbsp;&nbsp;Cost/Statute Mile</td>" & vbCrLf
        End If
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
        Build_OperatingCosts = Build_OperatingCosts & "</tr><tr class='alt_row'>" & vbCrLf
        If Session.Item("useMetricValues") Then
          Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;&nbsp;&nbsp;&nbsp;Cost/Seat " & TranslateUSMetricUnitsLong("M") & "</td>" & vbCrLf
        Else
          Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;&nbsp;&nbsp;&nbsp;Cost/Seat Mile</td>" & vbCrLf
        End If
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
        Build_OperatingCosts = Build_OperatingCosts & "</tr><tr>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td><td>&nbsp;</td>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "</tr><tr class='alt_row'>" & vbCrLf

        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;&nbsp;<b><u>Total Cost (No Depreciation)</u></b></td>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
        Build_OperatingCosts = Build_OperatingCosts & "</tr><tr>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;&nbsp;&nbsp;&nbsp;Cost/Hour</td>" & vbCrLf
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
        Build_OperatingCosts = Build_OperatingCosts & "</tr><tr class='alt_row'>" & vbCrLf
        If Session.Item("useMetricValues") Then
          Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;&nbsp;&nbsp;&nbsp;Cost/" & TranslateUSMetricUnitsLong("SM") & "</td>" & vbCrLf
        Else
          Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;&nbsp;&nbsp;&nbsp;Cost/Statute Mile</td>" & vbCrLf
        End If
        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
        Build_OperatingCosts = Build_OperatingCosts & "</tr><tr>" & vbCrLf

        If Session.Item("useMetricValues") Then
          Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;&nbsp;&nbsp;&nbsp;Cost/Seat " & TranslateUSMetricUnitsLong("M") & "</td>" & vbCrLf
        Else
          Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;&nbsp;&nbsp;&nbsp;Cost/Seat Mile</td>" & vbCrLf
        End If


        Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
        Build_OperatingCosts = Build_OperatingCosts & "</tr>" & vbCrLf






        Build_OperatingCosts = Build_OperatingCosts & "</table>"
        '--------------------------------------- ENF OF SECTION FOR JUST LABELS----------------------------
        '--------------------------------------- ENF OF SECTION FOR JUST LABELS----------------------------
      Else



        SqlConn.ConnectionString = Application.Item("crmJetnetDatabase")

        SqlConn.Open()

        SqlCommand.Connection = SqlConn
        SqlCommand.CommandType = System.Data.CommandType.Text
        SqlCommand.CommandTimeout = 60


        If amod_id = 0 Then
          Query = "SELECT * FROM Aircraft_Model WITH(NOLOCK) "
          Query = Query & " WHERE amod_make_name = '" & make_model_name & "' "
          Query = Query & " and amod_type_code IN(" & product_code_from_make & ") "
        Else
          Query = "SELECT * FROM Aircraft_Model WITH(NOLOCK) "
          Query = Query & " WHERE amod_id = " + amod_id.ToString
          'Query = Query & " and amod_type_code IN(" & product_code_from_make & ") "
        End If

        Query = Query & clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(Session.Item("localSubscription"), False, False)
        SqlCommand.CommandText = Query
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>Function Build_OperatingCosts(ByVal amod_id As Integer, ByVal make_model_name As String, ByVal is_label_column As Boolean, ByVal product_code_from_make As String) As String</b><br />" & Query

        Dim l_objBuilder As New StringBuilder
        Dim nNumberOfEngines As Integer = 0
        Dim nRememberModelID As Long = 0
        Dim nMAXEngines As Long = 0
        Dim nRows As Long = 0
        localAdoRs = SqlCommand.ExecuteReader()

        If localAdoRs.HasRows Then
          ' EXTRA QUERY TAKEN OUT
          Do While localAdoRs.Read


            ' class='alt_row'
            ' if it loops through more then once then it will check, this should only happen when models are selected
            'If times_through_counter = 8 And amod_id = 0 Then
            '    Build_OperatingCosts = Build_OperatingCosts & "</td></tr>" 
            '    Build_OperatingCosts = Build_OperatingCosts & "<tr bgcolor='white'><Td colspan='10'>&nbsp;</td></tr>"
            '    Build_OperatingCosts = Build_OperatingCosts & "<tr bgcolor='white'><Td colspan='10'>&nbsp;</td></tr>"
            '    Build_OperatingCosts = Build_OperatingCosts & "<tr><td width='25%'>"
            '    Build_OperatingCosts = Build_OperatingCosts & Build_OperatingCosts(0, "", True, type_string)
            '    Build_OperatingCosts = Build_OperatingCosts & "</td><td width='20%'>"
            '    times_through_counter = 0
            If times_through_counter > 0 And amod_id = 0 Then
              Build_OperatingCosts = Build_OperatingCosts & "</td><td align='left'>"
            End If

            times_through_counter = times_through_counter + 1

            TCND = 0.0
            TCSM = 0.0
            TCST = 0.0
            ANCH = 0.0

            tmpCDblValue = 0.0

            ' bFirstOne = False
            nCount = 3

            DCRecCount = 0
            AFRecCount = 0
            ABRecCount = 0


            If CDbl(Session.Item("localfuelPrice")) > 0 Then
              Session.Item("fuelPriceBase") = Session.Item("localfuelPrice")
            ElseIf CDbl(Session.Item("homebasefuelPrice")) > 0 Then
              Session.Item("fuelPriceBase") = Session.Item("homebasefuelPrice")
            End If

            If LCase(Session.Item("useStandardOrMetric")) = LCase("standard") Then
              sTitle = "US Standard"
            Else
              sTitle = "Metric"
            End If

            sCurrencyName = ""
            sCurrencyDate = CStr(Now())

            ' MSW THIS WAS ADDED IN TO DUMMY IN US

            If CLng(Session.Item("defaultCurrency")) <> 9 Then ' 9 = us dollar

              Session.Item("currencyExchangeRate") = GetForeignExchangeRate(Session.Item("defaultCurrency"), sCurrencyName, sCurrencyDate)

              If Trim(sCurrencyDate) <> "" Then
                sTitle = sTitle & " <em>(" & sCurrencyName & ": " & Session.Item("currencyExchangeRate") & ") rate as of " & FormatDateTime(sCurrencyDate, vbShortDate) & "</em>]"
              End If

            Else
              Session.Item("currencyExchangeRate") = 0
              'sCurrencySymbol = commonEVO.cDollarSymbol
            End If
            'TEMP HOLD
            If InStr(1, LCase(sCurrencyName), "euro") > 0 Then
              'sCurrencySymbol = commonEVO.cEuroSymbol
            ElseIf InStr(1, LCase(sCurrencyName), "dollar") > 0 Then
              ' sCurrencySymbol = commonEVO.cDollarSymbol
            ElseIf InStr(1, LCase(sCurrencyName), "pound") > 0 Then
              ' sCurrencySymbol = commonEVO.cPoundSymbol
            Else
              'sCurrencySymbol = commonEVO.cEmptyString
            End If


            '   ------------------------------------------------------- THIS IS THE BEGGINING OF THE LEFT SIDE INFO -----------------------------------------------------------------
            ''''''''''''''''''''''''''''''''''''''''
            ' Starts the DIRECT COSTS PER HOUR 
            ''''''''''''''''''''''''''''''''''''''''



            'style='border:1px #000000 solid'
            Build_OperatingCosts = Build_OperatingCosts & "<table border='0' cellpadding='3' width='100%' cellspacing='0'  align='left'><tr valign='top' align='center' class='aircraft_list' valign='top'><td align=left colspan='3' nowrap>" & vbCrLf
            If amod_id = 0 Then
              Build_OperatingCosts = Build_OperatingCosts & "<b>" & make_model_name & " " & localAdoRs("amod_model_name") & "</b></td></tr><tr class='alt_row'>"
            Else
              Build_OperatingCosts = Build_OperatingCosts & "<b>" & make_model_name & "</b></td></tr><tr class='alt_row'>"
            End If


            Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"

            If Session.Item("useMetricValues") Then

              If Not IsDBNull(localAdoRs("amod_fuel_gal_cost")) And CDbl(Session.Item("fuelPriceBase")) = 0 Then
                If CDbl(localAdoRs("amod_fuel_gal_cost")) Then
                  fuelGalCost = ConvertUSToMetricValue("PPG", CDbl(localAdoRs("amod_fuel_gal_cost")))
                End If
              Else
                If CDbl(Session.Item("fuelPriceBase")) > 0 Then
                  fuelGalCost = ConvertUSToMetricValue("PPG", CDbl(Session.Item("fuelPriceBase")))
                End If
              End If

              If CDbl(Session.Item("currencyExchangeRate")) > 0 Then
                fuelGalCost = ConvertUSToForeignCurrency(Session.Item("currencyExchangeRate"), fuelGalCost)
              End If
              fuelGalCost = System.Math.Round(fuelGalCost, 2)

              fuelAddCost = ConvertUSToMetricValue("PPG", CDbl(localAdoRs("amod_fuel_add_cost")))
              If CDbl(Session.Item("currencyExchangeRate")) > 0 Then
                fuelAddCost = ConvertUSToForeignCurrency(Session.Item("currencyExchangeRate"), fuelAddCost)
              End If

              fuelAddCost = System.Math.Round(fuelAddCost, 2)


              fuelBurnRate = ConvertUSToMetricValue("GAL", CDbl(localAdoRs("amod_fuel_burn_rate")))

              fuelBurnRate = System.Math.Round(fuelBurnRate, 2)


              fuelTotCost = CDbl((fuelGalCost + fuelAddCost) * fuelBurnRate)


            Else

              If Not IsDBNull(localAdoRs("amod_fuel_gal_cost")) And CDbl(Session.Item("fuelPriceBase")) = 0 Then
                If CDbl(localAdoRs("amod_fuel_gal_cost")) Then
                  fuelGalCost = CDbl(localAdoRs("amod_fuel_gal_cost"))
                End If
              Else
                If CDbl(Session.Item("fuelPriceBase")) > 0 Then
                  fuelGalCost = CDbl(Session.Item("fuelPriceBase"))
                End If
              End If

              If CDbl(Session.Item("currencyExchangeRate")) > 0 Then
                ' exchange rate should always be set ? why always change 
                fuelGalCost = ConvertUSToForeignCurrency(Session.Item("currencyExchangeRate"), fuelGalCost)
              End If
              fuelGalCost = System.Math.Round(fuelGalCost, 2)



              If Not IsDBNull(localAdoRs("amod_fuel_add_cost")) Then
                fuelAddCost = CDbl(localAdoRs("amod_fuel_add_cost"))
              Else
                fuelAddCost = CDbl(0)
              End If

              If CDbl(Session.Item("currencyExchangeRate")) > 0 Then
                fuelAddCost = ConvertUSToForeignCurrency(Session.Item("currencyExchangeRate"), fuelAddCost)
              End If
              fuelAddCost = System.Math.Round(fuelAddCost, 2)

              If Not IsDBNull(localAdoRs("amod_fuel_burn_rate")) Then
                fuelBurnRate = CDbl(localAdoRs("amod_fuel_burn_rate"))
              Else
                fuelBurnRate = CDbl(0)
              End If

              fuelBurnRate = System.Math.Round(fuelBurnRate, 2)

              fuelTotCost = (fuelGalCost + fuelAddCost) * fuelBurnRate

            End If

            fuelTotCost = System.Math.Round(fuelTotCost, 2)



            tmpCDblValue = fuelTotCost
            'If CDbl(Session.item("currencyExchangeRate")) > 0 Then
            'tmpCDblValue = ConvertUSToForeignCurrency(Session.item("currencyExchangeRate"), tmpCDblValue)
            ' End If

            If Not IsDBNull(localAdoRs("amod_fuel_tot_cost")) Then
              Build_OperatingCosts = Build_OperatingCosts & "<td align='right'><font color='red'>" & sCurrencySymbol & FormatNumber(tmpCDblValue, 2, True, False, True) & "</font></td>" & vbCrLf
            Else
              Build_OperatingCosts = Build_OperatingCosts & "<td align='right'>&nbsp;</td>" & vbCrLf
            End If
            '   End If

            Build_OperatingCosts = Build_OperatingCosts & "</tr><tr>" & vbCrLf

            ' when changing from metric to US standard all lables have to change


            If Not IsDBNull(localAdoRs("amod_fuel_gal_cost")) And CDbl(Session.Item("fuelPriceBase")) = 0 Then
              If CDbl(localAdoRs("amod_fuel_gal_cost")) > 0 Then


                'If Session.item("useMetricValues") Then
                '  fuelGalCost = ConvertUSToMetricValue("PPG", CDbl(localAdoRs("amod_fuel_gal_cost")))
                'Else
                '  fuelGalCost = CDbl(localAdoRs("amod_fuel_gal_cost"))
                'End If



                If CDbl(Session.Item("currencyExchangeRate")) > 0 Then
                  fuelGalCost = ConvertUSToForeignCurrency(Session.Item("currencyExchangeRate"), fuelGalCost)
                  fuelGalCost = System.Math.Round(fuelGalCost, 2)
                End If


                Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td><td align='right'>" & sCurrencySymbol & FormatNumber(fuelGalCost, 2, True, False, True) & "</td>" & vbCrLf
                ' ok update the excell report with our item and value
                'Call UpdateExcelReport(rngDirectCosts, "FuelCost", fuelGalCost)
              Else
                Build_OperatingCosts = Build_OperatingCosts & "<td align='right'>&nbsp;</td>" & vbCrLf
                ' ok update the excell report with our item and value
                'Call UpdateExcelReport(rngDirectCosts, "FuelCost", 0)
              End If
            Else
              If CDbl(Session.Item("fuelPriceBase")) > 0 Then
                'If Session.item("useMetricValues") Then
                '  fuelGalCost = ConvertUSToMetricValue("PPG", CDbl(Session.Item("fuelPriceBase")))
                'Else
                '  fuelGalCost = CDbl(Session.Item("fuelPriceBase"))
                'End If
                ' If CDbl(Session.item("currencyExchangeRate")) > 0 Then
                'fuelGalCost = ConvertUSToForeignCurrency(Session.item("currencyExchangeRate"), fuelGalCost)
                ' End If
                Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td><td align='right'>" & sCurrencySymbol & FormatNumber(fuelGalCost, 2, True, False, True) & "</td>" & vbCrLf
                ' ok update the excell report with our item and value
                'Call UpdateExcelReport(rngDirectCosts, "FuelCost", fuelGalCost)
              Else
                Build_OperatingCosts = Build_OperatingCosts & "<td align='right'>&nbsp;</td>" & vbCrLf
                ' ok update the excell report with our item and value
                'Call UpdateExcelReport(rngDirectCosts, "FuelCost", 0)
              End If
            End If


            Build_OperatingCosts = Build_OperatingCosts & "</tr><tr class='alt_row'>" & vbCrLf

            Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"

            If Not IsDBNull(localAdoRs("amod_fuel_add_cost")) Then
              'If Session.item("useMetricValues") Then
              '  fuelAddCost = ConvertUSToMetricValue("PPG", CDbl(localAdoRs("amod_fuel_add_cost")))
              'Else
              '  fuelAddCost = CDbl(localAdoRs("amod_fuel_add_cost"))
              'End If
              '  If CDbl(Session.item("currencyExchangeRate")) > 0 Then
              'fuelAddCost = ConvertUSToForeignCurrency(Session.item("currencyExchangeRate"), fuelAddCost)
              'End If
              Build_OperatingCosts = Build_OperatingCosts & "<td align='right'>" & sCurrencySymbol & FormatNumber(fuelAddCost, 2, True, False, True) & "</td>" & vbCrLf
            Else
              Build_OperatingCosts = Build_OperatingCosts & "<td align='right'>&nbsp;</td>" & vbCrLf
            End If

            Build_OperatingCosts = Build_OperatingCosts & "</tr><tr>" & vbCrLf
            Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"

            If Not IsDBNull(localAdoRs("amod_fuel_burn_rate")) Then
              'If Session.item("useMetricValues") Then
              '  fuelBurnRate = ConvertUSToMetricValue("GAL", CDbl(localAdoRs("amod_fuel_burn_rate")))
              'Else
              '  fuelBurnRate = CDbl(localAdoRs("amod_fuel_burn_rate"))
              'End If
              Build_OperatingCosts = Build_OperatingCosts & "<td align='right'>" & FormatNumber(fuelBurnRate, 2, True, False, True) & "</td>" & vbCrLf
            Else
              Build_OperatingCosts = Build_OperatingCosts & "<td align='right'>&nbsp;</td>" & vbCrLf
            End If


            Build_OperatingCosts = Build_OperatingCosts & "</tr><tr class='alt_row'>" & vbCrLf

            Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"

            If Not IsDBNull(localAdoRs("amod_maint_tot_cost")) Then


              totalmaintcost = System.Math.Round(CDbl(localAdoRs("amod_maint_lab_cost")), 2) + System.Math.Round(CDbl(localAdoRs("amod_maint_parts_cost")), 2)

              totalmaintcost = System.Math.Round(totalmaintcost, 2)

              If CDbl(Session.Item("currencyExchangeRate")) > 0 Then
                totalmaintcost = ConvertUSToForeignCurrency(Session.Item("currencyExchangeRate"), totalmaintcost)
                totalmaintcost = System.Math.Round(totalmaintcost, 2)
              End If

              Build_OperatingCosts = Build_OperatingCosts & "<td align='right'><font color='red'>" & sCurrencySymbol & FormatNumber(totalmaintcost, 2, True, False, True) & "</font></td>" & vbCrLf
            Else
              Build_OperatingCosts = Build_OperatingCosts & "<td align='right'>&nbsp;</td>" & vbCrLf
            End If


            Build_OperatingCosts = Build_OperatingCosts & "</tr><tr>" & vbCrLf

            Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"

            If Not IsDBNull(localAdoRs("amod_maint_lab_cost")) Then
              tmpCDblValue = System.Math.Round(CDbl(localAdoRs("amod_maint_lab_cost")), 2)
              If CDbl(Session.Item("currencyExchangeRate")) > 0 Then
                tmpCDblValue = ConvertUSToForeignCurrency(Session.Item("currencyExchangeRate"), tmpCDblValue)
                tmpCDblValue = System.Math.Round(tmpCDblValue, 2)
              End If

              Build_OperatingCosts = Build_OperatingCosts & "<td align='right'>" & sCurrencySymbol & FormatNumber(tmpCDblValue, 2, True, False, True) & "</td>" & vbCrLf
            Else
              Build_OperatingCosts = Build_OperatingCosts & "<td align='right'>&nbsp;</td>" & vbCrLf
            End If

            Build_OperatingCosts = Build_OperatingCosts & "</tr><tr class='alt_row'>" & vbCrLf
            Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"

            If Not IsDBNull(localAdoRs("amod_maint_parts_cost")) Then
              tmpCDblValue = System.Math.Round(CDbl(localAdoRs("amod_maint_parts_cost")), 2)
              If CDbl(Session.Item("currencyExchangeRate")) > 0 Then
                tmpCDblValue = ConvertUSToForeignCurrency(Session.Item("currencyExchangeRate"), tmpCDblValue)
                tmpCDblValue = System.Math.Round(tmpCDblValue, 2)
              End If

              Build_OperatingCosts = Build_OperatingCosts & "<td align='right'>" & sCurrencySymbol & FormatNumber(tmpCDblValue, 2, True, False, True) & "</td>" & vbCrLf
            Else
              Build_OperatingCosts = Build_OperatingCosts & "<td align='right'>&nbsp;</td>" & vbCrLf
            End If


            Build_OperatingCosts = Build_OperatingCosts & "</tr><tr>" & vbCrLf

            Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
            If Not IsDBNull(localAdoRs("amod_engine_ovh_cost")) Then
              overhaulcost = System.Math.Round(CDbl(localAdoRs("amod_engine_ovh_cost")), 2)
              If CDbl(Session.Item("currencyExchangeRate")) > 0 Then
                overhaulcost = ConvertUSToForeignCurrency(Session.Item("currencyExchangeRate"), overhaulcost)
                overhaulcost = System.Math.Round(overhaulcost, 2)
              End If

              Build_OperatingCosts = Build_OperatingCosts & "<td align='right'>" & sCurrencySymbol & FormatNumber(overhaulcost, 2, True, False, True) & "</td>" & vbCrLf
            Else
              Build_OperatingCosts = Build_OperatingCosts & "<td align='right'>&nbsp;</td>" & vbCrLf
            End If


            Build_OperatingCosts = Build_OperatingCosts & "</tr><tr class='alt_row'>" & vbCrLf

            Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
            If Not IsDBNull(localAdoRs("amod_thrust_rev_ovh_cost")) Then
              revoverhaulcost = System.Math.Round(CDbl(localAdoRs("amod_thrust_rev_ovh_cost")), 2)
              If CDbl(Session.Item("currencyExchangeRate")) > 0 Then
                revoverhaulcost = ConvertUSToForeignCurrency(Session.Item("currencyExchangeRate"), revoverhaulcost)
                revoverhaulcost = System.Math.Round(revoverhaulcost, 2)
              End If
              Build_OperatingCosts = Build_OperatingCosts & "<td align='right'>" & sCurrencySymbol & FormatNumber(revoverhaulcost, 2, True, False, True) & "</td>" & vbCrLf
            Else
              Build_OperatingCosts = Build_OperatingCosts & "<td align='right'>&nbsp;</td>" & vbCrLf
            End If


            Build_OperatingCosts = Build_OperatingCosts & "</tr><tr>" & vbCrLf

            Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
            If Not IsDBNull(localAdoRs("amod_misc_flight_cost")) Then

              'tmpCDblValue = CDbl(localAdoRs("amod_misc_flight_cost"))
              miscflightcosts = System.Math.Round(CDbl(localAdoRs("amod_land_park_cost")), 2) + System.Math.Round(CDbl(localAdoRs("amod_crew_exp_cost")), 2) + System.Math.Round(CDbl(localAdoRs("amod_supplies_cost")), 2)
              miscflightcosts = System.Math.Round(miscflightcosts, 2)
              If CDbl(Session.Item("currencyExchangeRate")) > 0 Then
                miscflightcosts = ConvertUSToForeignCurrency(Session.Item("currencyExchangeRate"), miscflightcosts)
                miscflightcosts = System.Math.Round(miscflightcosts, 2)
              End If
              Build_OperatingCosts = Build_OperatingCosts & "<td align='right'><font color=red>" & sCurrencySymbol & FormatNumber(miscflightcosts, 2, True, False, True) & "</font></td>" & vbCrLf
            Else
              Build_OperatingCosts = Build_OperatingCosts & "<td align='right'>&nbsp;</td>" & vbCrLf
            End If


            Build_OperatingCosts = Build_OperatingCosts & "</tr><tr class='alt_row'>" & vbCrLf

            Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
            If Not IsDBNull(localAdoRs("amod_land_park_cost")) Then
              tmpCDblValue = System.Math.Round(CDbl(localAdoRs("amod_land_park_cost")), 2)
              If CDbl(Session.Item("currencyExchangeRate")) > 0 Then
                tmpCDblValue = ConvertUSToForeignCurrency(Session.Item("currencyExchangeRate"), tmpCDblValue)
                tmpCDblValue = System.Math.Round(tmpCDblValue, 2)
              End If
              Build_OperatingCosts = Build_OperatingCosts & "<td align='right'>" & sCurrencySymbol & FormatNumber(tmpCDblValue, 2, True, False, True) & "</td>" & vbCrLf
            Else
              Build_OperatingCosts = Build_OperatingCosts & "<td align='right'>&nbsp;</td>" & vbCrLf
            End If


            Build_OperatingCosts = Build_OperatingCosts & "</tr><tr>" & vbCrLf

            Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"

            If Not IsDBNull(localAdoRs("amod_crew_exp_cost")) Then
              tmpCDblValue = System.Math.Round(CDbl(localAdoRs("amod_crew_exp_cost")), 2)
              If CDbl(Session.Item("currencyExchangeRate")) > 0 Then
                tmpCDblValue = ConvertUSToForeignCurrency(Session.Item("currencyExchangeRate"), tmpCDblValue)
                tmpCDblValue = System.Math.Round(tmpCDblValue, 2)
              End If
              Build_OperatingCosts = Build_OperatingCosts & "<td align='right'>" & sCurrencySymbol & FormatNumber(tmpCDblValue, 2, True, False, True) & "</td>" & vbCrLf
            Else
              Build_OperatingCosts = Build_OperatingCosts & "<td align='right'>&nbsp;</td>" & vbCrLf
            End If


            Build_OperatingCosts = Build_OperatingCosts & "</tr><tr class='alt_row'>" & vbCrLf

            Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"

            If Not IsDBNull(localAdoRs("amod_supplies_cost")) Then
              tmpCDblValue = System.Math.Round(CDbl(localAdoRs("amod_supplies_cost")), 2)
              If CDbl(Session.Item("currencyExchangeRate")) > 0 Then
                tmpCDblValue = ConvertUSToForeignCurrency(Session.Item("currencyExchangeRate"), tmpCDblValue)
                tmpCDblValue = System.Math.Round(tmpCDblValue, 2)
              End If
              Build_OperatingCosts = Build_OperatingCosts & "<td align='right'>" & sCurrencySymbol & FormatNumber(tmpCDblValue, 2, True, False, True) & "</td>" & vbCrLf
            Else
              Build_OperatingCosts = Build_OperatingCosts & "<td align='right'>&nbsp;</td>" & vbCrLf

            End If


            Build_OperatingCosts = Build_OperatingCosts & "</tr><tr>" & vbCrLf
            Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
            ' THIS IS FOR TOTAL DIRECT COSTS
            If Not IsDBNull(localAdoRs("amod_tot_hour_direct_cost")) Then
              '  If Session.item("useMetricValues") Then
              ' totalDirCostHR = CDbl(fuelTotCost) + CDbl(amod_maint_tot_cost) + CDbl(amod_misc_flight_cost) + CDbl(amod_engine_ovh_cost) + CDbl(amod_thrust_rev_ovh_cost) 
              totalDirCostHR = System.Math.Round(CDbl(fuelTotCost), 2) + totalmaintcost + miscflightcosts + overhaulcost + revoverhaulcost

              'Else
              '  totalDirCostHR = CDbl(localAdoRs.Item("amod_tot_hour_direct_cost"))
              '  End If
              totalDirCostHR = System.Math.Round(totalDirCostHR, 2)
              tmpCDblValue = totalDirCostHR
              ' If CDbl(Session.item("currencyExchangeRate")) > 0 Then
              'tmpCDblValue = ConvertUSToForeignCurrency(Session.item("currencyExchangeRate"), tmpCDblValue)
              'End If
              Build_OperatingCosts = Build_OperatingCosts & "<td align='right'><font color='red'>" & sCurrencySymbol & FormatNumber(tmpCDblValue, 2, True, False, True) & "</font></td>" & vbCrLf
            Else
              Build_OperatingCosts = Build_OperatingCosts & "<td align='right'>&nbsp;</td>" & vbCrLf
            End If


            Build_OperatingCosts = Build_OperatingCosts & "</tr><tr class='alt_row'>" & vbCrLf


            Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"

            If Not IsDBNull(localAdoRs("amod_avg_block_speed")) Then
              If Session.Item("useMetricValues") Then
                avgBlockSpeed = System.Math.Round(ConvertUSToMetricValue("SM", CDbl(localAdoRs("amod_avg_block_speed"))), 0)
              Else
                avgBlockSpeed = System.Math.Round(CDbl(localAdoRs("amod_avg_block_speed")), 0)
              End If
              Build_OperatingCosts = Build_OperatingCosts & "<td align='right'><br>" & FormatNumber(avgBlockSpeed, 0, True, False, True) & "</td>" & vbCrLf
            Else
              Build_OperatingCosts = Build_OperatingCosts & "<td align='right'>&nbsp;</td>" & vbCrLf
            End If


            Build_OperatingCosts = Build_OperatingCosts & "</tr><tr>" & vbCrLf


            Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
            If totalDirCostHR > 0 And avgBlockSpeed > 0 Then
              If Session.Item("useMetricValues") Then
                totalCostPer = CDbl(CDbl(totalDirCostHR) / CDbl(avgBlockSpeed))
              Else
                totalCostPer = CDbl(CDbl(totalDirCostHR) / CDbl(avgBlockSpeed))
                'totalCostPer = CDbl(localAdoRs("amod_tot_stat_mile_cost"))
              End If

              totalCostPer = System.Math.Round(totalCostPer, 2)
              tmpCDblValue = totalCostPer
            Else
              totalCostPer = 0
              tmpCDblValue = 0
            End If
            ' If CDbl(Session.item("currencyExchangeRate")) > 0 Then
            'tmpCDblValue = ConvertUSToForeignCurrency(Session.item("currencyExchangeRate"), tmpCDblValue)
            '  End If

            If Not IsDBNull(localAdoRs("amod_tot_stat_mile_cost")) Then
              Build_OperatingCosts = Build_OperatingCosts & "<td align='right'><font color='red'>" & sCurrencySymbol & FormatNumber(tmpCDblValue, 2, True, False, True) & "</font></td>" & vbCrLf
            Else
              Build_OperatingCosts = Build_OperatingCosts & "<td align='right'>&nbsp;</td>" & vbCrLf
            End If


            Build_OperatingCosts = Build_OperatingCosts & "</tr>" & vbCrLf
            '    Build_OperatingCosts = Build_OperatingCosts & "</table>" & vbCrLf

            '  Build_OperatingCosts = Build_OperatingCosts & "</table><br>"




            ' Build_OperatingCosts = Build_OperatingCosts & "<tr><td>&nbsp;</td></tr>"
            ''''''''''''''''''''''''''''''''''''''''''
            ' End DIRECT COSTS PER HOUR 
            '''''''''''''''''''''''''''''''''''''''''
            ''''''''''''''''''''''''''''''''''''''''''
            ' End DIRECT COSTS PER HOUR 
            '''''''''''''''''''''''''''''''''''''''''
            ''''''''''''''''''''''''''''''''''''''''''
            ' End DIRECT COSTS PER HOUR 
            '''''''''''''''''''''''''''''''''''''''''

            ''''''''''''''''''''''''''''''''''''''''''
            ' START OF SECTION SECTION ON LEFT 
            '''''''''''''''''''''''''''''''''''''''''

            bFirstOne = False

            'Loop
            bFirstOne = True
            '''''''''''''''''''''''
            ' Start ANNUAL BUDGET 
            '''''''''''''''''''''''

            bFirstOne = True
            ' EXTRA QUERY TAKEN OUT RTW/MSW 8/23
            bFirstOne = True
            nCount = 3

            ' Build_OperatingCosts = Build_OperatingCosts & "<td valign=top align=left rowspan='3' class='Operating_Costs_TD_Bottom'>"


            bFirstOne = False


            '''''''''''''''''''''
            ' End Annual Budge
            '''''''''''''''''''''


            If amod_id = 0 Then

              Build_OperatingCosts = Build_OperatingCosts & "<tr><td>&nbsp;</td></tr><tr class='aircraft_list' valign='top'><td align='left' colspan='3' nowrap><b>" & make_model_name & " " & localAdoRs("amod_model_name") & "</b></td></tr><tr class='alt_row'>"
            Else
              Build_OperatingCosts = Build_OperatingCosts & "<tr><td>&nbsp;</td></tr><tr class='aircraft_list' valign='top'><td align='left' colspan='3' nowrap><b>" & make_model_name & "</b></td></tr><tr class='alt_row'>"

            End If


            ' EXTRA QUERY TAKEN OUT RTW/MSW 8/23
            bFirstOne = True
            nCount = 3


            Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
            If Not IsDBNull(localAdoRs("amod_tot_crew_salary_cost")) Then



              'Dim crewsalaries = 0

              crewsalaries = System.Math.Round(CDbl(localAdoRs("amod_capt_salary_cost")), 0) + System.Math.Round(CDbl(localAdoRs("amod_cpilot_salary_cost")), 0) + System.Math.Round(CDbl(localAdoRs("amod_crew_benefit_cost")), 0)
              If CDbl(Session.Item("currencyExchangeRate")) > 0 Then
                crewsalaries = ConvertUSToForeignCurrency(Session.Item("currencyExchangeRate"), crewsalaries)
                crewsalaries = System.Math.Round(crewsalaries, 0)
              End If

              crewsalaries = System.Math.Round(crewsalaries, 0)
              Build_OperatingCosts = Build_OperatingCosts & "<td align=right><font color=red>" & sCurrencySymbol & FormatNumber(crewsalaries, 0, True, False, True) & "</font></td>" & vbCrLf
            Else
              Build_OperatingCosts = Build_OperatingCosts & "<td align=right>&nbsp;</td>" & vbCrLf
            End If


            Build_OperatingCosts = Build_OperatingCosts & "</tr><tr>" & vbCrLf

            Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
            If Not IsDBNull(localAdoRs("amod_capt_salary_cost")) Then
              tmpCDblValue = System.Math.Round(CDbl(localAdoRs("amod_capt_salary_cost")), 0)
              If CDbl(Session.Item("currencyExchangeRate")) > 0 Then
                tmpCDblValue = ConvertUSToForeignCurrency(Session.Item("currencyExchangeRate"), tmpCDblValue)
                tmpCDblValue = System.Math.Round(tmpCDblValue, 0)
              End If
              Build_OperatingCosts = Build_OperatingCosts & "<td align=right>" & sCurrencySymbol & FormatNumber(tmpCDblValue, 0, True, False, True) & "</td>" & vbCrLf
            Else
              Build_OperatingCosts = Build_OperatingCosts & "<td align=right>&nbsp;</td>" & vbCrLf
            End If


            Build_OperatingCosts = Build_OperatingCosts & "</tr><tr class='alt_row'>" & vbCrLf


            Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
            If Not IsDBNull(localAdoRs("amod_cpilot_salary_cost")) Then
              tmpCDblValue = System.Math.Round(CDbl(localAdoRs("amod_cpilot_salary_cost")), 0)
              If CDbl(Session.Item("currencyExchangeRate")) > 0 Then
                tmpCDblValue = ConvertUSToForeignCurrency(Session.Item("currencyExchangeRate"), tmpCDblValue)
                tmpCDblValue = System.Math.Round(tmpCDblValue, 0)
              End If
              Build_OperatingCosts = Build_OperatingCosts & "<td align=right>" & sCurrencySymbol & FormatNumber(tmpCDblValue, 0, True, False, True) & "</td>" & vbCrLf
            Else
              Build_OperatingCosts = Build_OperatingCosts & "<td align=right>&nbsp;</td>" & vbCrLf
            End If

            Build_OperatingCosts = Build_OperatingCosts & "</tr><tr>" & vbCrLf

            Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
            If Not IsDBNull(localAdoRs("amod_crew_benefit_cost")) Then
              tmpCDblValue = System.Math.Round(CDbl(localAdoRs("amod_crew_benefit_cost")), 0)
              If CDbl(Session.Item("currencyExchangeRate")) > 0 Then
                tmpCDblValue = ConvertUSToForeignCurrency(Session.Item("currencyExchangeRate"), tmpCDblValue)
                tmpCDblValue = System.Math.Round(tmpCDblValue, 0)
              End If
              Build_OperatingCosts = Build_OperatingCosts & "<td align=right>" & sCurrencySymbol & FormatNumber(tmpCDblValue, 0, True, False, True) & "</td>" & vbCrLf
            Else
              Build_OperatingCosts = Build_OperatingCosts & "<td align=right>&nbsp;</td>" & vbCrLf
            End If

            Build_OperatingCosts = Build_OperatingCosts & "</tr><tr class='alt_row'>" & vbCrLf

            Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
            If Not IsDBNull(localAdoRs("amod_hangar_cost")) Then
              'Dim hangercost = 0

              hangercost = System.Math.Round(CDbl(localAdoRs("amod_hangar_cost")), 0)
              If CDbl(Session.Item("currencyExchangeRate")) > 0 Then
                hangercost = ConvertUSToForeignCurrency(Session.Item("currencyExchangeRate"), hangercost)
              End If
              hangercost = System.Math.Round(hangercost, 0)

              Build_OperatingCosts = Build_OperatingCosts & "<td align=right>" & sCurrencySymbol & FormatNumber(hangercost, 0, True, False, True) & "</td>" & vbCrLf
            Else
              Build_OperatingCosts = Build_OperatingCosts & "<td align=right>&nbsp;</td>" & vbCrLf
            End If

            Build_OperatingCosts = Build_OperatingCosts & "</tr><tr>" & vbCrLf



            Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
            If Not IsDBNull(localAdoRs("amod_insurance_cost")) Then
              insurancecost = System.Math.Round(CDbl(localAdoRs("amod_hull_insurance_cost")), 0) + System.Math.Round(CDbl(localAdoRs("amod_liability_insurance_cost")), 0)
              If CDbl(Session.Item("currencyExchangeRate")) > 0 Then
                insurancecost = ConvertUSToForeignCurrency(Session.Item("currencyExchangeRate"), insurancecost)
              End If
              insurancecost = System.Math.Round(insurancecost, 0)
              Build_OperatingCosts = Build_OperatingCosts & "<td align=right><font color=red>" & sCurrencySymbol & FormatNumber(insurancecost, 0, True, False, True) & "</font></td>" & vbCrLf
            Else
              Build_OperatingCosts = Build_OperatingCosts & "<td align=right>&nbsp;</td>" & vbCrLf
            End If

            Build_OperatingCosts = Build_OperatingCosts & "</tr><tr class='alt_row'>" & vbCrLf

            Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
            If Not IsDBNull(localAdoRs("amod_hull_insurance_cost")) Then
              tmpCDblValue = System.Math.Round(CDbl(localAdoRs("amod_hull_insurance_cost")), 0)
              If CDbl(Session.Item("currencyExchangeRate")) > 0 Then
                tmpCDblValue = ConvertUSToForeignCurrency(Session.Item("currencyExchangeRate"), tmpCDblValue)
                tmpCDblValue = System.Math.Round(tmpCDblValue, 0)
              End If
              Build_OperatingCosts = Build_OperatingCosts & "<td align=right>" & sCurrencySymbol & FormatNumber(tmpCDblValue, 0, True, False, True) & "</td>" & vbCrLf
            Else
              Build_OperatingCosts = Build_OperatingCosts & "<td align=right>&nbsp;</td>" & vbCrLf
            End If

            Build_OperatingCosts = Build_OperatingCosts & "</tr><tr>" & vbCrLf

            Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
            If Not IsDBNull(localAdoRs("amod_liability_insurance_cost")) Then
              tmpCDblValue = System.Math.Round(CDbl(localAdoRs("amod_liability_insurance_cost")), 0)
              If CDbl(Session.Item("currencyExchangeRate")) > 0 Then
                tmpCDblValue = ConvertUSToForeignCurrency(Session.Item("currencyExchangeRate"), tmpCDblValue)
              End If
              Build_OperatingCosts = Build_OperatingCosts & "<td align=right>" & sCurrencySymbol & FormatNumber(tmpCDblValue, 0, True, False, True) & "</td>" & vbCrLf
            Else
              Build_OperatingCosts = Build_OperatingCosts & "<td align=right>&nbsp;</td>" & vbCrLf
            End If

            Build_OperatingCosts = Build_OperatingCosts & "</tr><tr class='alt_row'>" & vbCrLf

            Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
            If Not IsDBNull(localAdoRs("amod_tot_misc_ovh_cost")) Then
              'Dim miscoverhead = 0
              miscoverhead = System.Math.Round(CDbl(localAdoRs("amod_misc_train_cost")), 0) + System.Math.Round(CDbl(localAdoRs("amod_misc_modern_cost")), 0) + System.Math.Round(CDbl(localAdoRs("amod_misc_naveq_cost")), 0)

              If CDbl(Session.Item("currencyExchangeRate")) > 0 Then
                miscoverhead = ConvertUSToForeignCurrency(Session.Item("currencyExchangeRate"), miscoverhead)
                miscoverhead = System.Math.Round(miscoverhead, 0)
              End If


              Build_OperatingCosts = Build_OperatingCosts & "<td align=right><font color=red>" & sCurrencySymbol & FormatNumber(miscoverhead, 0, True, False, True) & "</font></td>" & vbCrLf
            Else
              Build_OperatingCosts = Build_OperatingCosts & "<td align=right>&nbsp;</td>" & vbCrLf
            End If

            Build_OperatingCosts = Build_OperatingCosts & "</tr><tr>" & vbCrLf

            Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
            If Not IsDBNull(localAdoRs("amod_misc_train_cost")) Then
              tmpCDblValue = System.Math.Round(CDbl(localAdoRs("amod_misc_train_cost")), 0)
              If CDbl(Session.Item("currencyExchangeRate")) > 0 Then
                tmpCDblValue = ConvertUSToForeignCurrency(Session.Item("currencyExchangeRate"), tmpCDblValue)
                tmpCDblValue = System.Math.Round(tmpCDblValue, 0)
              End If
              Build_OperatingCosts = Build_OperatingCosts & "<td align=right>" & sCurrencySymbol & FormatNumber(tmpCDblValue, 0, True, False, True) & "</td>" & vbCrLf
            Else
              Build_OperatingCosts = Build_OperatingCosts & "<td align=right>&nbsp;</td>" & vbCrLf
            End If

            Build_OperatingCosts = Build_OperatingCosts & "</tr><tr class='alt_row'>" & vbCrLf




            Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
            If Not IsDBNull(localAdoRs("amod_misc_modern_cost")) Then
              tmpCDblValue = System.Math.Round(CDbl(localAdoRs("amod_misc_modern_cost")), 0)
              If CDbl(Session.Item("currencyExchangeRate")) > 0 Then
                tmpCDblValue = ConvertUSToForeignCurrency(Session.Item("currencyExchangeRate"), tmpCDblValue)
                tmpCDblValue = System.Math.Round(tmpCDblValue, 0)
              End If
              Build_OperatingCosts = Build_OperatingCosts & "<td align=right>" & sCurrencySymbol & FormatNumber(tmpCDblValue, 0, True, False, True) & "</td>" & vbCrLf
            Else
              Build_OperatingCosts = Build_OperatingCosts & "<td align=right>&nbsp;</td>" & vbCrLf
            End If

            Build_OperatingCosts = Build_OperatingCosts & "</tr><tr>" & vbCrLf



            Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
            If Not IsDBNull(localAdoRs("amod_misc_naveq_cost")) Then
              tmpCDblValue = System.Math.Round(CDbl(localAdoRs("amod_misc_naveq_cost")), 0)
              If CDbl(Session.Item("currencyExchangeRate")) > 0 Then
                tmpCDblValue = ConvertUSToForeignCurrency(Session.Item("currencyExchangeRate"), tmpCDblValue)
                tmpCDblValue = System.Math.Round(tmpCDblValue, 0)
              End If
              Build_OperatingCosts = Build_OperatingCosts & "<td align=right>" & sCurrencySymbol & FormatNumber(tmpCDblValue, 0, True, False, True) & "</td>" & vbCrLf
            Else
              Build_OperatingCosts = Build_OperatingCosts & "<td align=right>&nbsp;</td>" & vbCrLf
            End If

            Build_OperatingCosts = Build_OperatingCosts & "</tr><tr class='alt_row'>" & vbCrLf



            Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
            If Not IsDBNull(localAdoRs("amod_deprec_cost")) Then
              'Dim depoverhead = 0
              depoverhead = System.Math.Round(CDbl(localAdoRs("amod_deprec_cost")), 0)
              If CDbl(Session.Item("currencyExchangeRate")) > 0 Then
                depoverhead = ConvertUSToForeignCurrency(Session.Item("currencyExchangeRate"), depoverhead)
              End If
              depoverhead = System.Math.Round(depoverhead, 0)

              Build_OperatingCosts = Build_OperatingCosts & "<td align=right>" & sCurrencySymbol & FormatNumber(depoverhead, 0, True, False, True) & "</td>" & vbCrLf
            Else
              Build_OperatingCosts = Build_OperatingCosts & "<td align=right>&nbsp;</td>" & vbCrLf
            End If

            Build_OperatingCosts = Build_OperatingCosts & "</tr><tr>" & vbCrLf



            Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
            If Not IsDBNull(localAdoRs("amod_tot_fixed_cost")) Then

              totalfixedcost = crewsalaries + hangercost + miscoverhead + depoverhead + insurancecost

              totalfixedcost = System.Math.Round(totalfixedcost, 0)


              '   If CDbl(Session.item("currencyExchangeRate")) > 0 Then
              'tmpCDblValue = ConvertUSToForeignCurrency(Session.item("currencyExchangeRate"), tmpCDblValue)
              ' End If
              Build_OperatingCosts = Build_OperatingCosts & "<td align=right><font color=red>" & sCurrencySymbol & FormatNumber(totalfixedcost, 0, True, False, True) & "</font></td>" & vbCrLf
            Else
              Build_OperatingCosts = Build_OperatingCosts & "<td align=right>&nbsp;</td>" & vbCrLf
            End If



            Build_OperatingCosts = Build_OperatingCosts & "</tr>"
            '  Build_OperatingCosts = Build_OperatingCosts & "</td></tr></table>"
            '   ------------------------------------------------------- THIS IS THE END OF THE LEFT SIDE INFO -----------------------------------------------------------------
            '   ------------------------------------------------------- THIS IS THE END OF THE LEFT SIDE INFO -----------------------------------------------------------------
            '   ------------------------------------------------------- THIS IS THE END OF THE LEFT SIDE INFO -----------------------------------------------------------------

            Build_OperatingCosts = Build_OperatingCosts & "<tr class='aircraft_list' valign='top'>" & vbCrLf

            If amod_id = 0 Then
              Build_OperatingCosts = Build_OperatingCosts & "<td colspan='3' nowrap><b>" & make_model_name & " " & localAdoRs("amod_model_name") & "</b></td></tr><tr class='alt_row'>" & vbCrLf
            Else
              Build_OperatingCosts = Build_OperatingCosts & "<td colspan='3' nowrap><b>" & make_model_name & "</b></td></tr><tr class='alt_row'>" & vbCrLf
            End If




            Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
            If Not IsDBNull(localAdoRs("amod_number_of_seats")) Then
              Build_OperatingCosts = Build_OperatingCosts & "<td align=right>" & FormatNumber(System.Math.Round(CDbl(localAdoRs("amod_number_of_seats")), 0), 0, True, False, True) & "</td>" & vbCrLf
            Else
              Build_OperatingCosts = Build_OperatingCosts & "<td align=right>&nbsp;</td>" & vbCrLf
            End If

            Build_OperatingCosts = Build_OperatingCosts & "</tr><tr>" & vbCrLf




            Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
            If Not IsDBNull(localAdoRs("amod_annual_miles")) Then
              If Session.Item("useMetricValues") Then
                annualMiles = ConvertUSToMetricValue("M", CDbl(localAdoRs("amod_annual_miles")))
              Else
                annualMiles = CDbl(localAdoRs("amod_annual_miles"))
              End If
              annualMiles = System.Math.Round(annualMiles, 0)
              Build_OperatingCosts = Build_OperatingCosts & "<td align=right>" & FormatNumber(annualMiles, 0, True, False, True) & "</td>" & vbCrLf
            Else
              Response.Write("<td align=right>&nbsp;</td>" & vbCrLf)
            End If

            Build_OperatingCosts = Build_OperatingCosts & "</tr><tr class='alt_row'>" & vbCrLf


            Build_OperatingCosts = Build_OperatingCosts & "<td width='50%'>&nbsp;</td>"
            If Not IsDBNull(localAdoRs("amod_annual_hours")) And annualMiles > 0 And avgBlockSpeed > 0 Then
              'If Session.item("useMetricValues") Then
              ' annualHrs = Round(CDbl(annualMiles) / CDbl(avgBlockSpeed), 0)
              '  annualHrs = CDbl(localAdoRs("amod_annual_hours"))
              '   annualHrs = CDbl(annualMiles) / CDbl(avgBlockSpeed)
              ' Else
              annualHrs = CDbl(annualMiles) / CDbl(avgBlockSpeed)
              ' End If
              annualHrs = System.Math.Round(annualHrs, 0)

              Build_OperatingCosts = Build_OperatingCosts & "<td align='right'><font color='red'>" & FormatNumber(annualHrs, 0, True, False, True) & "</font></td>" & vbCrLf
            Else
              Build_OperatingCosts = Build_OperatingCosts & "<td align='right'>&nbsp;</td>" & vbCrLf
            End If

            Build_OperatingCosts = Build_OperatingCosts & "</tr>"
            Build_OperatingCosts = Build_OperatingCosts & "<tr>" & vbCrLf

            Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
            If Not IsDBNull(localAdoRs("amod_tot_direct_cost")) Then


              ' CALCULATING EITHER WAY FOR ACCURATE VALUES
              ' If Not Session.item("useMetricValues") Then
              'totalDirCostYR = annualHrs * totalDirCostHR
              ' Else
              totalDirCostYR = annualHrs * totalDirCostHR
              '  End If


              totalDirCostYR = System.Math.Round(totalDirCostYR, 0)
              tmpCDblValue = totalDirCostYR
              ' If CDbl(Session.item("currencyExchangeRate")) > 0 Then
              'tmpCDblValue = ConvertUSToForeignCurrency(Session.item("currencyExchangeRate"), tmpCDblValue)
              ' End If
              Build_OperatingCosts = Build_OperatingCosts & "<td align='right'><font color='red'>" & sCurrencySymbol & FormatNumber(tmpCDblValue, 0, True, False, True) & "</font></td>" & vbCrLf
            Else
              Build_OperatingCosts = Build_OperatingCosts & "<td align='right'>&nbsp;</td>" & vbCrLf
            End If

            Build_OperatingCosts = Build_OperatingCosts & "</tr><tr class='alt_row'>" & vbCrLf


            Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
            If Not IsDBNull(localAdoRs("amod_tot_fixed_cost")) Then
              'tmpCDblValue = System.Math.Round(CDbl(localAdoRs("amod_tot_fixed_cost")), 0)
              If CDbl(Session.Item("currencyExchangeRate")) > 0 Then
                tmpCDblValue = ConvertUSToForeignCurrency(Session.Item("currencyExchangeRate"), tmpCDblValue)
              End If
              Build_OperatingCosts = Build_OperatingCosts & "<td align='right'><font color='red'>" & sCurrencySymbol & FormatNumber(totalfixedcost, 0, True, False, True) & "</font></td>" & vbCrLf
            Else
              Build_OperatingCosts = Build_OperatingCosts & "<td align='right'>&nbsp;</td>" & vbCrLf
            End If

            Build_OperatingCosts = Build_OperatingCosts & "</tr><tr>" & vbCrLf



            Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
            If Not IsDBNull(localAdoRs("amod_tot_df_annual_cost")) Then
              'If Session.item("useMetricValues") Then
              ' totalFixedDirect = CDbl(CDbl(totalDirCostYR) + CDbl(localAdoRs("amod_tot_fixed_cost")))
              totalFixedDirect = totalDirCostYR + totalfixedcost

              'Else
              ' totalFixedDirect = CDbl(CDbl(totalDirCostYR) + CDbl(localAdoRs("amod_tot_fixed_cost")))
              'totalFixedDirect = CDbl(localAdoRs("amod_tot_df_annual_cost"))
              ' CALCULATING EITHER WAY FOR MORE ACCURATE
              ' End If
              tmpCDblValue = totalFixedDirect
              'If CDbl(Session.item("currencyExchangeRate")) > 0 Then
              'tmpCDblValue = ConvertUSToForeignCurrency(Session.item("currencyExchangeRate"), tmpCDblValue)
              ' End If
              Build_OperatingCosts = Build_OperatingCosts & "<td align='right'><font color='red'>" & sCurrencySymbol & FormatNumber(tmpCDblValue, 0, True, False, True) & "</font></td>" & vbCrLf
            Else
              Build_OperatingCosts = Build_OperatingCosts & "<td align=right>&nbsp;</td>" & vbCrLf
            End If

            Build_OperatingCosts = Build_OperatingCosts & "</tr><tr class='alt_row'>" & vbCrLf

            Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"


            If Not IsDBNull(localAdoRs("amod_tot_df_hour_cost")) And annualHrs > 0 And totalFixedDirect > 0 Then
              If Session.Item("useMetricValues") Then
                ' dfhourcost = dCDbl(CDbl(totalFixedDirect) / CDbl(annualHrs))
                dfhourcost = totalFixedDirect / annualHrs
              Else
                dfhourcost = totalFixedDirect / annualHrs
                '  dfhourcost = CDbl(localAdoRs("amod_tot_df_hour_cost"))
                ' CALCULATING EITHER WAY FOR MORE ACCURATE
              End If

              dfhourcost = System.Math.Round(dfhourcost, 0)
              tmpCDblValue = dfhourcost
              '  If CDbl(Session.item("currencyExchangeRate")) > 0 Then
              '      tmpCDblValue = ConvertUSToForeignCurrency(Session.item("currencyExchangeRate"), tmpCDblValue)
              'End If
              Build_OperatingCosts = Build_OperatingCosts & "<td align='right'><font color='red'>" & sCurrencySymbol & FormatNumber(tmpCDblValue, 0, True, False, True) & "</font></td>" & vbCrLf
            Else
              Build_OperatingCosts = Build_OperatingCosts & "<td align=right>&nbsp;</td>" & vbCrLf
            End If

            Build_OperatingCosts = Build_OperatingCosts & "</tr><tr>" & vbCrLf


            Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
            If Not IsDBNull(localAdoRs("amod_tot_df_statmile_cost")) And annualMiles > 0 And totalFixedDirect > 0 Then
              If Session.Item("useMetricValues") Then
                'dfstatmilecost = CDbl(CDbl(totalFixedDirect) / CDbl(annualMiles))
                dfstatmilecost = totalFixedDirect / annualMiles
              Else
                dfstatmilecost = totalFixedDirect / annualMiles
                ' dfstatmilecost = CDbl(localAdoRs("amod_tot_df_statmile_cost"))
              End If
              dfstatmilecost = System.Math.Round(dfstatmilecost, 2)
              tmpCDblValue = dfstatmilecost
              '  If CDbl(Session.item("currencyExchangeRate")) > 0 Then
              'tmpCDblValue = ConvertUSToForeignCurrency(Session.item("currencyExchangeRate"), tmpCDblValue)
              '    End If
              Build_OperatingCosts = Build_OperatingCosts & "<td align='right'><font color='red'>" & sCurrencySymbol & FormatNumber(tmpCDblValue, 2, True, False, True) & "</font></td>" & vbCrLf
            Else
              Build_OperatingCosts = Build_OperatingCosts & "<td align=right>&nbsp;</td>" & vbCrLf
            End If

            Build_OperatingCosts = Build_OperatingCosts & "  </tr><tr class='alt_row'>" & vbCrLf

            Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
            If Not IsDBNull(localAdoRs("amod_tot_df_seat_cost")) And dfstatmilecost > 0 And localAdoRs("amod_number_of_seats") > 0 Then
              '  If Session.item("useMetricValues") Then
              ' dfseatcost = CDbl(CDbl(dfstatmilecost) / CDbl(localAdoRs("amod_number_of_seats")))
              dfseatcost = dfstatmilecost / System.Math.Round(localAdoRs("amod_number_of_seats"), 0)
              ' Else
              '  dfseatcost = dfstatmilecost / CDbl(localAdoRs("amod_number_of_seats")))
              ' dfseatcost = CDbl(localAdoRs("amod_tot_df_seat_cost"))
              'End If
              dfseatcost = System.Math.Round(dfseatcost, 2)
              tmpCDblValue = dfseatcost
              '    If CDbl(Session.item("currencyExchangeRate")) > 0 Then
              'tmpCDblValue = ConvertUSToForeignCurrency(Session.item("currencyExchangeRate"), tmpCDblValue)
              '   End If
              Build_OperatingCosts = Build_OperatingCosts & "<td align='right'><font color='red'>" & sCurrencySymbol & FormatNumber(tmpCDblValue, 2, True, False, True) & "</font></td>" & vbCrLf
            Else
              Build_OperatingCosts = Build_OperatingCosts & "<td align=right>&nbsp;</td>" & vbCrLf
            End If


            Build_OperatingCosts = Build_OperatingCosts & "</tr><tr>" & vbCrLf
            Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>" & vbCrLf
            Build_OperatingCosts = Build_OperatingCosts & "</tr><tr class='alt_row'>" & vbCrLf
            Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"

            'If Session.item("useMetricValues") Then
            TCND = totalFixedDirect - depoverhead
            ' Else
            'TCND = CDbl(CDbl(localAdoRs("amod_tot_df_annual_cost")) - CDbl(localAdoRs("amod_deprec_cost")))
            'End If
            TCND = System.Math.Round(TCND, 0)
            tmpCDblValue = TCND
            '     If CDbl(Session.item("currencyExchangeRate")) > 0 Then
            'tmpCDblValue = ConvertUSToForeignCurrency(Session.item("currencyExchangeRate"), tmpCDblValue)
            '    End If

            If tmpCDblValue > 0 Then
              Build_OperatingCosts = Build_OperatingCosts & "<td align='right'><font color='red'>" & sCurrencySymbol & FormatNumber(tmpCDblValue, 0, True, False, True) & "</font></td>" & vbCrLf
            Else
              Build_OperatingCosts = Build_OperatingCosts & "<td align='right'><font color='red'>" & sCurrencySymbol & "0.00</font></td>" & vbCrLf
            End If

            Build_OperatingCosts = Build_OperatingCosts & "</tr><tr>" & vbCrLf

            Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"
            If Not IsDBNull(localAdoRs("amod_annual_hours")) Then
              If localAdoRs("amod_annual_hours") > 0 Then
                ' If Session.item("useMetricValues") Then


                ANCH = TCND / annualHrs
                ' THIS IS CALCULATING THE TOTAL 

                'Else
                '  ANCH = CDbl(CDbl(TCND) / CDbl(localAdoRs("amod_annual_hours")))

                ' End If
                ANCH = System.Math.Round(ANCH, 0)
                tmpCDblValue = ANCH
                '   If CDbl(Session.item("currencyExchangeRate")) > 0 Then
                'tmpCDblValue = ConvertUSToForeignCurrency(Session.item("currencyExchangeRate"), tmpCDblValue)
                '   End If

                Build_OperatingCosts = Build_OperatingCosts & "<td align='right'><font color='red'>" & sCurrencySymbol & FormatNumber(tmpCDblValue, 0, True, False, True) & "</font></td>" & vbCrLf
              Else
                Build_OperatingCosts = Build_OperatingCosts & "<td align='right'><font color='red'>" & sCurrencySymbol & "0.00</font></td>" & vbCrLf
              End If
            Else
              Build_OperatingCosts = Build_OperatingCosts & "<td align='right'><font color='red'>" & sCurrencySymbol & "0.00</font></td>" & vbCrLf
            End If

            Build_OperatingCosts = Build_OperatingCosts & "</tr><tr class='alt_row'>" & vbCrLf

            Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"

            If Not IsDBNull(localAdoRs("amod_annual_miles")) Then
              If localAdoRs("amod_annual_miles") > 0 Then
                ' If Session.item("useMetricValues") Then
                TCSM = TCND / annualMiles
                'Else
                '  TCSM = CDbl(CDbl(TCND) / CDbl(localAdoRs("amod_annual_miles")))
                ' End If
                TCSM = System.Math.Round(TCSM, 3)
                tmpCDblValue = TCSM
                ' If CDbl(Session.item("currencyExchangeRate")) > 0 Then
                'tmpCDblValue = ConvertUSToForeignCurrency(Session.item("currencyExchangeRate"), tmpCDblValue)
                '  End If

                Build_OperatingCosts = Build_OperatingCosts & "<td align='right'><font color='red'>" & sCurrencySymbol & FormatNumber(tmpCDblValue, 2, True, False, True) & "</font></td>" & vbCrLf
              Else
                Build_OperatingCosts = Build_OperatingCosts & "<td align='right'><font color='red'>" & sCurrencySymbol & "0.00</font></td>" & vbCrLf
              End If
            Else
              Build_OperatingCosts = Build_OperatingCosts & "<td align='right'><font color='red'>" & sCurrencySymbol & "0.00</font></td>" & vbCrLf
            End If

            Build_OperatingCosts = Build_OperatingCosts & "</tr><tr>" & vbCrLf


            Build_OperatingCosts = Build_OperatingCosts & "<td>&nbsp;</td>"

            If Not IsDBNull(localAdoRs("amod_number_of_seats")) Then
              If localAdoRs("amod_number_of_seats") > 0 Then
                TCST = TCSM / System.Math.Round(localAdoRs("amod_number_of_seats"), 0)
                TCST = System.Math.Round(TCST, 2)
                tmpCDblValue = TCST
                '  If CDbl(Session.item("currencyExchangeRate")) > 0 Then
                'tmpCDblValue = ConvertUSToForeignCurrency(Session.item("currencyExchangeRate"), tmpCDblValue)
                '  End If
                Build_OperatingCosts = Build_OperatingCosts & "<td align='right'><font color='red'>" & sCurrencySymbol & FormatNumber(tmpCDblValue, 2, True, False, True) & "</font></td>" & vbCrLf
              Else
                Build_OperatingCosts = Build_OperatingCosts & "<td align='right'><font color='red'>" & sCurrencySymbol & "0.00</font></td>" & vbCrLf
              End If
            Else
              Build_OperatingCosts = Build_OperatingCosts & "<td align='right'><font color='red'>" & sCurrencySymbol & "0.00</font></td>" & vbCrLf
            End If


            Build_OperatingCosts = Build_OperatingCosts & "  </tr>" & vbCrLf

            'close annual budget table
            Build_OperatingCosts = Build_OperatingCosts & "</table>" & vbCrLf ' </td>"



          Loop
          'End If
        Else
          Build_OperatingCosts = ""
          Exit Function
        End If
      End If
      '--------------------------------------- ENF OF SECTION FOR JUST DATA----------------------------
      '--------------------------------------- ENF OF SECTION FOR JUST DATA----------------------------
      '--------------------------------------- ENF OF SECTION FOR JUST DATA----------------------------
      '--------------------------------------- ENF OF SECTION FOR JUST DATA----------------------------
      '--------------------------------------- ENF OF SECTION FOR JUST DATA----------------------------






















      ' THIS ENDS THE INITIAL ROW CONTAINING ALL INFO AND THEN STARTS NEW ROW-------------------------------------------------------
      '  Build_OperatingCosts = Build_OperatingCosts & "</tr>"
      '  Build_OperatingCosts = Build_OperatingCosts & "<tr>"
      '-----------------------------------------------------------------------------------------------------------------------------

      ' THIS IS THE LAST COLUMN -------------------------------------------------------------------
      ' Build_OperatingCosts = Build_OperatingCosts & "<td colspan='5' class='Operating_Costs_TD_Bottom'>&nbsp;</td></tr></table>"
      'Build_OperatingCosts = Build_OperatingCosts & "<td colspan='5' class='Operating_Costs_TD_Top_1'>&nbsp;</td></tr></table>"
      ' THIS IS THE LAST COLUMN -------------------------------------------------------------------

      bFirstOne = False




    Catch ex As Exception
    Finally
      SqlConn.Close()
      SqlConn.Dispose()
    End Try
  End Function
  Public Function GetForeignExchangeRate(ByVal in_CurrencyID, ByRef out_CurrencyName, ByRef out_CurrencyDate) As Double

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim adoRs As System.Data.SqlClient.SqlDataReader : adoRs = Nothing
    Dim Query As String : Query = ""
    Try
      SqlConn.ConnectionString = Application.Item("crmJetnetDatabase")

      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60
      out_CurrencyName = ""
      out_CurrencyDate = ""
      GetForeignExchangeRate = CDbl(1)

      Query = "SELECT currency_exchange_rate, currency_name, currency_exchange_rate_date FROM Currency WITH(NOLOCK) WHERE currency_id = " & in_CurrencyID
      SqlCommand.CommandText = Query
      adoRs = SqlCommand.ExecuteReader()
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>Function GetForeignExchangeRate(ByVal in_CurrencyID, ByRef out_CurrencyName, ByRef out_CurrencyDate) As Double</b><br />" & Query

      If adoRs.HasRows Then
        adoRs.Read()
        If Not IsDBNull(adoRs.Item("currency_exchange_rate")) Then
          GetForeignExchangeRate = CDbl(adoRs.Item("currency_exchange_rate"))
        End If

        If Not IsDBNull(adoRs.Item("currency_exchange_rate")) Then
          If CLng(Trim(adoRs.Item("currency_exchange_rate"))) > 0 Then
            GetForeignExchangeRate = CDbl(adoRs.Item("currency_exchange_rate"))
          End If
        End If

        If Not IsDBNull(adoRs.Item("currency_name")) Then
          If Trim(Trim(adoRs.Item("currency_name"))) <> "" Then
            out_CurrencyName = Trim(adoRs.Item("currency_name"))
          End If
        End If

        If Not IsDBNull(adoRs.Item("currency_exchange_rate_date")) Then
          If Trim(Trim(adoRs.Item("currency_exchange_rate_date"))) <> "" Then
            out_CurrencyDate = Trim(adoRs.Item("currency_exchange_rate_date"))
          End If
        End If

        adoRs.Close()

      End If

      adoRs = Nothing
    Catch ex As Exception
    Finally
      SqlConn.Close()
      SqlConn.Dispose()
    End Try
    Return GetForeignExchangeRate
  End Function

  Public Function Get_Fuel_Price() As Double
    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim adoRs As System.Data.SqlClient.SqlDataReader : adoRs = Nothing
    Dim sQuery As String = ""
    Dim tempprice As Double = 0.0


    Try

      SqlConn.ConnectionString = Application.Item("crmJetnetDatabase")

      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text
      SqlCommand.CommandTimeout = 60


      sQuery = "SELECT evo_config_fuel_cost FROM Evolution_Configuration WITH(NOLOCK) WHERE evo_config_category = 'LIVE'"
      SqlCommand.CommandText = sQuery
      adoRs = SqlCommand.ExecuteReader()
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>Function Get_Fuel_Price() As Double</b><br />" & sQuery

      If adoRs.HasRows Then
        adoRs.Read()
        If Not IsDBNull(adoRs("evo_config_fuel_cost")) Then

          tempprice = CDbl(adoRs("evo_config_fuel_cost"))

        End If
      End If
      adoRs.Close()
      adoRs = Nothing
    Catch SqlException

      SqlConn.Dispose()
      SqlCommand.Dispose()

      sQuery = Nothing

    Finally

      SqlCommand.Dispose()
      SqlConn.Close()
      SqlConn.Dispose()

    End Try

    Return tempprice

  End Function


  Public Function ConvertUSToForeignCurrency(ByVal in_ExchangeRate, ByVal in_valToConvert)
    ConvertUSToForeignCurrency = CDbl(CDbl(in_ExchangeRate) * CDbl(in_valToConvert))
  End Function
  Public Function TranslateUSMetricUnitsLong(ByVal in_StrToTranslate)

    Select Case (UCase(in_StrToTranslate))

      Case "FT"
        TranslateUSMetricUnitsLong = "Meter"
      Case "NM"
        TranslateUSMetricUnitsLong = "Kilometer"
      Case "M"
        TranslateUSMetricUnitsLong = "Kilometer"
      Case "SM"
        TranslateUSMetricUnitsLong = "Kilometer"
      Case "KN"
        TranslateUSMetricUnitsLong = "Kilometers Per Hour"
      Case "FPM"
        TranslateUSMetricUnitsLong = "Meters Per Second"
      Case "PSI"
        TranslateUSMetricUnitsLong = "Milimeter of Mercury"
      Case "LB"
        TranslateUSMetricUnitsLong = "Kilogram"
      Case "GAL"
        TranslateUSMetricUnitsLong = "Liter"
      Case "HP"
        TranslateUSMetricUnitsLong = "Metric Horsepower"
      Case Else
        TranslateUSMetricUnitsLong = UCase(in_StrToTranslate)

    End Select

  End Function
  Public Function ConvertUSToMetricValue(ByVal in_convertWhat, ByVal in_valToConvert)

    Select Case (UCase(in_convertWhat))

      Case "FT"
        ConvertUSToMetricValue = CDbl(ConvertFeetToMeter(in_valToConvert))
      Case "NM"
        ConvertUSToMetricValue = CDbl(ConvertNauticalMileToKilometer(in_valToConvert))
      Case "M"
        ConvertUSToMetricValue = CDbl(ConvertMileToKilometer(in_valToConvert))
      Case "SM"
        ConvertUSToMetricValue = CDbl(ConvertStatuteMileToKilometer(in_valToConvert))
      Case "KN"
        ConvertUSToMetricValue = CDbl(ConvertKnotsToKPH(in_valToConvert))
      Case "FPM"
        ConvertUSToMetricValue = CDbl(ConvertFPMToMPS(in_valToConvert))
      Case "PSI"
        ConvertUSToMetricValue = CDbl(ConvertPSIToHG(in_valToConvert))
      Case "LBS"
        ConvertUSToMetricValue = CDbl(ConvertPoundToKilogram(in_valToConvert))
      Case "GAL"
        ConvertUSToMetricValue = CDbl(ConvertGallonToLiter(in_valToConvert))
      Case "PPG"
        ConvertUSToMetricValue = CDbl(ConvertCostGallonToCostLiter(in_valToConvert))
      Case Else
        ConvertUSToMetricValue = CDbl(in_valToConvert)

    End Select


  End Function
  Public Function ConvertCostGallonToCostLiter(ByVal dCostGallon)

    Dim dCostLiter

    dCostLiter = 0.0

    If CDbl(dCostGallon) > 0.0 Then
      dCostLiter = CDbl(dCostGallon) * 0.26417
    End If

    ConvertCostGallonToCostLiter = dCostLiter

  End Function ' ConvertCostGallonToCostLiter

  Public Function ConvertFeetToMeter(ByVal lFeet)

    Dim dMeter

    dMeter = 0.0
    If CDbl(lFeet) > 0.0 Then
      dMeter = CDbl(lFeet) * 0.3048
    End If
    ConvertFeetToMeter = dMeter

  End Function ' ConvertFeetToMeter

  Public Function ConvertMeterToFeet(ByVal lMeter)

    Dim dFeet

    dFeet = 0.0
    If CDbl(lMeter) > 0.0 Then
      dFeet = CDbl(lMeter) * 3.2808399
    End If
    ConvertMeterToFeet = dFeet

  End Function ' ConvertMeterToFeet

  Public Function ConvertNauticalMileToKilometer(ByVal lNMile)

    Dim dKilometer

    dKilometer = 0.0
    If CDbl(lNMile) > 0.0 Then
      dKilometer = CDbl(lNMile) * 1.852
    End If
    ConvertNauticalMileToKilometer = dKilometer

  End Function ' ConvertNauticalMileToKilometer

  Public Function ConvertKilometerToNauticalMile(ByVal lKilometer)

    Dim dNMile

    dNMile = 0.0
    If CDbl(lKilometer) > 0.0 Then
      dNMile = CDbl(lKilometer) * 0.53995
    End If
    ConvertKilometerToNauticalMile = dNMile

  End Function ' ConvertKilometerToNauticalMile

  Public Function ConvertKilometerToMile(ByVal lKilometer)

    Dim dMile

    dMile = 0.0
    If CDbl(lKilometer) > 0.0 Then
      dMile = CDbl(lKilometer) * 0.62137
    End If
    ConvertKilometerToMile = dMile

  End Function ' ConvertKilometerToMile

  Public Function ConvertKilometerToStatuteMile(ByVal lKilometer)

    Dim dSMile

    dSMile = 0.0
    If CDbl(lKilometer) > 0.0 Then
      dSMile = CDbl(lKilometer) * 0.62137
    End If
    ConvertKilometerToStatuteMile = dSMile

  End Function ' ConvertKilometerToStatuteMile

  Public Function ConvertStatuteMileToKilometer(ByVal lSMile)

    Dim dKilometer

    dKilometer = 0.0
    If CDbl(lSMile) > 0.0 Then
      dKilometer = CDbl(lSMile) * 1.609344
    End If
    ConvertStatuteMileToKilometer = dKilometer

  End Function ' ConvertStatuteMileToKilometer

  Public Function ConvertMileToKilometer(ByVal lMile)

    Dim dKilometer

    dKilometer = 0.0
    If CDbl(lMile) > 0.0 Then
      dKilometer = CDbl(lMile) * 1.609344
    End If
    ConvertMileToKilometer = dKilometer

  End Function ' ConvertMileToKilometer

  Public Function ConvertKnotsToKPH(ByVal lKnots) ' Knots To Kilometers Per Hour

    Dim dKPH

    dKPH = 0.0
    If CDbl(lKnots) > 0.0 Then
      dKPH = CDbl(lKnots) * 1.852
    End If
    ConvertKnotsToKPH = dKPH

  End Function ' ConvertKnotsToKPH 

  Public Function ConvertKPHToKnots(ByVal lKPH) ' Kilometers Per Hour To Knots

    Dim dKnots

    dKnots = 0.0
    If CDbl(lKPH) > 0.0 Then
      dKnots = CDbl(lKPH) * 0.53995
    End If
    ConvertKPHToKnots = dKnots

  End Function ' ConvertKPHToKnots 

  Public Function ConvertFPMToMPS(ByVal lFPM) ' Feet Per Minute to Meters Per Second

    Dim dMPS

    dMPS = 0.0
    If CDbl(lFPM) > 0.0 Then
      dMPS = ((CDbl(lFPM) * 0.3048) / 60)
    End If
    ConvertFPMToMPS = dMPS

  End Function ' ConvertFPMToMPS

  Public Function ConvertMPSToFPM(ByVal lMPS) ' Meters Per Second To Feet Per Minute

    Dim dFPM

    dFPM = 0.0
    If CDbl(lMPS) > 0.0 Then
      dFPM = ((CDbl(lMPS) * 3.281) * 60)
    End If
    ConvertMPSToFPM = dFPM

  End Function ' ConvertMPSToFPM

  Public Function ConvertPSIToHG(ByVal lPSI) ' Pounds Per Square Inch To Milimeter of Mercury (torr)

    Dim dHG

    dHG = 0.0
    If CDbl(lPSI) > 0.0 Then
      dHG = CDbl(lPSI) * 51.72
    End If
    ConvertPSIToHG = dHG

  End Function ' ConvertPSIToHG

  Public Function ConvertHGToPSI(ByVal lHG) ' Milimeter of Mercury (torr) To Pounds Per Square Inch

    Dim dPSI

    dPSI = 0.0
    If CDbl(lHG) > 0.0 Then
      dPSI = CDbl(lHG) * 0.01934
    End If
    ConvertHGToPSI = dPSI

  End Function ' ConvertHGToPSI

  Public Function ConvertPoundToKilogram(ByVal lPounds)

    Dim dKilo

    dKilo = 0.0
    If CDbl(lPounds) > 0.0 Then
      dKilo = CDbl(lPounds) * 0.4536
    End If
    ConvertPoundToKilogram = dKilo

  End Function ' ConvertPoundToKilogram

  Public Function ConvertKilogramToPound(ByVal lKilo)

    Dim dPound

    dPound = 0.0
    If CDbl(lKilo) > 0.0 Then
      dPound = CDbl(lKilo) * 2.205
    End If
    ConvertKilogramToPound = dPound

  End Function ' ConvertKilogramToPound

  Public Function ConvertGallonToLiter(ByVal lGallon)

    Dim dLiter

    dLiter = 0.0
    If CDbl(lGallon) > 0.0 Then
      dLiter = CDbl(lGallon) * 3.7854
    End If
    ConvertGallonToLiter = dLiter

  End Function ' ConvertGallonToLiter 

  Public Function ConvertLiterToGallon(ByVal lLiter)

    Dim dGallon

    dGallon = 0.0
    If CDbl(lLiter) > 0.0 Then
      dGallon = CDbl(lLiter) * 0.26417
    End If
    ConvertLiterToGallon = dGallon

  End Function ' ConvertLiterToGallon

  Public Function ConvertHPToMetricHP(ByVal lHorsepower)

    Dim dMetrichorsepower

    dMetrichorsepower = 0.0
    If CDbl(lHorsepower) > 0.0 Then
      dMetrichorsepower = CDbl(lHorsepower) * 1.000001
    End If
    ConvertHPToMetricHP = dMetrichorsepower

  End Function ' ConvertHPToMetricHP 

  Public Function ConvertMetricHPToHP(ByVal lMetrichorsepower)

    Dim dHorsepower

    dHorsepower = 0.0
    If CDbl(lMetrichorsepower) > 0.0 Then
      dHorsepower = CDbl(lMetrichorsepower) * 0.9999995
    End If
    ConvertMetricHPToHP = dHorsepower

  End Function ' ConvertMetricHPToHP

End Class