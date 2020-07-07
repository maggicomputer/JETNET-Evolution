' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/largeGraphDisplay.aspx.vb $
'$$Author: Matt $
'$$Date: 8/12/19 9:15a $
'$$Modtime: 8/12/19 9:07a $
'$$Revision: 3 $
'$$Workfile: largeGraphDisplay.aspx.vb $
'
' ********************************************************************************

Partial Public Class largeGraphDisplay
  Inherits System.Web.UI.Page

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    Dim sErrorString As String = ""
    Dim ModelID As Long = 0
    Dim graph_type As String = ""
    Dim AircraftID As Long = 0
    Dim Client_AC_ID As Long = 0
    Dim localDatalayer As New viewsDataLayer
    Dim google_map_array_list As String = ""
    Dim utilization_functions As New utilization_view_functions
    Dim exists_data As Boolean = False
    Dim Aircraft_History_String As String = ""
    Dim htmlUtilizationGraphScript As String = ""
    Dim graph_string As String = ""
    Dim htmlOut As New StringBuilder
    Dim ac_value_datatable As New DataTable
    Dim MainHeading As String = ""
    Dim aclsData_Temp As New clsData_Manager_SQL
        Dim ac_dlv_year As Integer = 0
        Dim temp_Graph_text As String = ""

        If Session.Item("crmUserLogon") <> True Then

      Response.Redirect("Default.aspx", False)

    Else

      If Not Session.Item("localPreferences").loadUserSession(sErrorString, CLng(Session.Item("localUser").crmSubSubID.ToString), Session.Item("localUser").crmUserLogin.ToString, CLng(Session.Item("localUser").crmSubSeqNo.ToString), CLng(Session.Item("localUser").crmUserContactID.ToString)) Then
        Response.Write("error in load large graph : " + sErrorString)
      End If

      Dim fSubins_platform_os As String = commonEvo.getBrowserCapabilities(Request.Browser)


      AclsData_Temp.JETNET_DB = Session.Item("jetnetClientDatabase")
      AclsData_Temp.client_DB = Session.Item("jetnetServerNotesDatabase")

      If Trim(Request("amod_id")) <> "" Then
        ModelID = Trim(Request("amod_id"))
      Else
        ModelID = 0
      End If

      If Trim(Request("graph_type")) <> "" Then
        graph_type = Trim(Request("graph_type"))
      Else
        graph_type = "ASKSOLD"
      End If


      If Trim(Request("ac_id")) <> "" Then
        AircraftID = Trim(Request("ac_id"))
      Else
        AircraftID = 0
      End If

      If Trim(Request("Client_AC_ID")) <> "" Then
        Client_AC_ID = Trim(Request("Client_AC_ID"))
      Else
        Client_AC_ID = 0
      End If


      If Trim(Request("ac_dlv_year")) <> "" Then
        ac_dlv_year = Trim(Request("ac_dlv_year"))
      Else
        ac_dlv_year = 0
      End If


      If Trim(graph_type) = "DLVYEAR" Then
        Me.check_avg_asking.Visible = True
        Me.check_avg_sale.Visible = True
        Me.check_eValues.Visible = True
      End If

      If Trim(graph_type) = "CURRENTMARKET" Then
        Me.drop_order_by.Visible = True
        Me.drop_label.Visible = True
      End If



            'ADDED IN MSW 
            ' FillAssettInsightGraphs() 
            Try


                If Trim(Request("Residual")) = "Y" Then

                    If Trim(HttpContext.Current.Session.Item("Residual_Chart_Java")) <> "" Then

                        temp_Graph_text = Trim(HttpContext.Current.Session.Item("Residual_Chart_Java"))
                        temp_Graph_text = Replace(temp_Graph_text, "Visualization1", "Visualization7")
                        temp_Graph_text = Replace(temp_Graph_text, "visualization1", "visualization7")

                        temp_Graph_text = Replace(temp_Graph_text, "data1", "data7")
                        temp_Graph_text = Replace(temp_Graph_text, "textstyle:{fontsize:8}},", "textstyle:{fontsize:8}}")

                        htmlUtilizationGraphScript = vbCrLf + "<script type=""text/javascript"">" + vbCrLf
                        htmlUtilizationGraphScript += "$(document).ready(function(){" + vbCrLf
                        htmlUtilizationGraphScript += " drawVisualization7();" + vbCrLf
                        htmlUtilizationGraphScript += "});" + vbCrLf
                        htmlUtilizationGraphScript += temp_Graph_text
                        htmlUtilizationGraphScript += "</script>" + vbCrLf

                        System.Web.UI.ScriptManager.RegisterStartupScript(outer_update_panel, outer_update_panel.GetType(), "showUtilizationGraph7", htmlUtilizationGraphScript, False)

                        graph_label.Text = "<table id=""flightActivityTable"" width=""100%"" cellspacing=""0"" cellpadding=""4""><tr><td valign=""top"" align=""left""><div id='visualization7' style=""height:540px;""></div></td></tr></table>"

                    End If


                ElseIf clsGeneral.clsGeneral.isEValuesAvailable() = True Then

                    If Trim(graph_type) = "DLVYEAR" Or Trim(graph_type) = "CURRENTMARKET" Or Trim(graph_type) = "ASKSOLD" Then

                        '   If graph_type = "MFRYEAR" Then
                        '     MainHeading = "Prices By MFR Year"
                        '   Else
                        'MainHeading = "Asking vs. Sold Graph"
                        ' End If

                        MainHeading = Trim(Request("page_title"))

                        utilization_functions.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
                        utilization_functions.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
                        utilization_functions.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
                        utilization_functions.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
                        utilization_functions.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim


                        'MFRYEAR
                        'ASKSOLD
                        'ACESTIMATES

                        'AI Estimates for Aircraft
                        'AI Estimates for Model By Year
                        'AI Estimates for Model By Time

                        Call utilization_functions.FillAssettInsightGraphs(graph_type, ModelID, graph_label.Text, outer_update_panel, 7, 0, 0, 540, ac_dlv_year, check_avg_asking.Checked, check_avg_sale.Checked, check_eValues.Checked, drop_order_by.SelectedValue, "N", "", "", "", "", "", "", "", "", "", temp_Graph_text)

                    ElseIf Trim(graph_type) = "ACESTIMATES" Then

                        MainHeading = Trim(Request("page_title"))

                        utilization_functions.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
                        utilization_functions.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
                        utilization_functions.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
                        utilization_functions.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
                        utilization_functions.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim


                        'HttpContext.Current.Session.Item("jetnetServerNotesDatabase") = CApplication.Item("crmClientDatabase")
                        '   localDatalayer.clientConnectStr = Session.Item("jetnetClientDatabase") 'CApplication.Item("crmJetnetDatabase")

                        'valuation_chart.Titles.Clear()
                        'valuation_chart.Titles.Add("My Aircraft Value History")
                        '  localDatalayer.views_analytics_graph_1(AircraftID, Nothing, Aircraft_History_String, AircraftID, google_map_array_list, "O", 0, exists_data, "", True, 0, ac_value_datatable)

                        'MFRYEAR
                        'ASKSOLD
                        'ACESTIMATES

                        'AI Estimates for Aircraft
                        'AI Estimates for Model By Year
                        'AI Estimates for Model By Time


                        Call utilization_functions.FillAssettInsightGraphs("ASKSOLD", ModelID, graph_label.Text, outer_update_panel, 7, AircraftID, Client_AC_ID, 540, 0, True, True, True, "", "N", "", "", "", "", "", "", "", "", "", temp_Graph_text)




                        ' valuation_chart.ImageType = DataVisualization.Charting.ChartImageType.Jpeg
                        'If exists_data = True Then
                        '  ' valuation_chart.SaveImage(Server.MapPath("TempFiles") + "\AC_" & AircraftID & "_Visualization_Chart_MONTHS.jpg", DataVisualization.Charting.ChartImageFormat.Jpeg)
                        '  DisplayFunctions.load_google_chart(graph_panel, google_map_array_list, "", "Aircraft Value ($k)", "ac_all", 430, 227, "POINTS", 1, graph_string, Me.Page, Me.outer_update_panel, False, False, True)


                        '  htmlOut.Append("<table id=""modelUserInterestTable"" width=""100%"" cellspacing=""0"" cellpadding=""4"">")
                        '  htmlOut.Append("<tr><td valign=""top"" align=""left""><div id='ac_all' style=""height:295px;""></div></td></tr>")
                        '  htmlOut.Append("</table>" + vbCrLf)


                        '  If Not String.IsNullOrEmpty(google_map_array_list.Trim) Then
                        '    htmlUtilizationGraphScript = vbCrLf + "<script type=""text/javascript"">" + vbCrLf
                        '    htmlUtilizationGraphScript += "$(document).ready(function(){" + vbCrLf
                        '    htmlUtilizationGraphScript += " drawVisualization1();" + vbCrLf
                        '    htmlUtilizationGraphScript += "});" + vbCrLf
                        '    htmlUtilizationGraphScript += " function drawVisualization1() {  "
                        '    htmlUtilizationGraphScript += graph_string.Trim
                        '    htmlUtilizationGraphScript += "}</script>" + vbCrLf

                        '    System.Web.UI.ScriptManager.RegisterStartupScript(graph_tabs, graph_tabs.GetType(), "showUtilizationGraph1", htmlUtilizationGraphScript, False)
                        '  End If

                        '  graph_label.Text = htmlOut.ToString

                        '  ' aircraft_value_history_label.Text = "<img src='TempFiles/AC_" & AircraftID & "_Visualization_Chart_MONTHS.jpg' width='300' />"
                        '  ' graph_label.Text = Aircraft_History_String
                        'Else
                        '  '  graph_label.Text = "No Value History Available"
                        'End If
                    ElseIf Trim(graph_type) = "RESIDUAL" Then
                        MainHeading = Trim(Request("page_title"))

                        Call utilization_functions.FillAssettInsightGraphs("RESIDUAL", ModelID, graph_label.Text, outer_update_panel, 7, AircraftID, Client_AC_ID, 540, 0, True, True, True, "", "N", "", "", "", "", "", "", "", "", "", temp_Graph_text)
                    ElseIf Trim(graph_type) = "RESIDUALAC" Then
                        MainHeading = Trim(Request("page_title"))
                        Call utilization_functions.FillAssettInsightGraphs("RESIDUALAC", ModelID, graph_label.Text, outer_update_panel, 7, AircraftID, Client_AC_ID, 540, ac_dlv_year, True, True, True, "", "N", "", "", "", "", "", "", "", "", "", temp_Graph_text)
                    ElseIf Trim(graph_type) = "AFTT" Then
                        MainHeading = Trim(Request("page_title"))
                        Call utilization_functions.FillAssettInsightGraphs("AFTT", ModelID, graph_label.Text, outer_update_panel, 7, AircraftID, Client_AC_ID, 540, 0, True, True, True, "", "N", "", "", "", "", "", "", "", "", "", temp_Graph_text)
                    End If


                    If AircraftID > 0 Or Client_AC_ID > 0 And ModelID = 0 Then
                        Dim aircraftTable As New DataTable

                        aircraftTable = CommonAircraftFunctions.BuildReusableTable(IIf(AircraftID > 0, AircraftID, Client_AC_ID), 0, IIf(Client_AC_ID > 0, "CLIENT", "JETNET"), "", aclsData_Temp, True, 0, IIf(Client_AC_ID > 0, "CLIENT", "JETNET"))

                        If Not IsNothing(aircraftTable) Then
                            If aircraftTable.Rows.Count > 0 Then
                                MainHeading = CommonAircraftFunctions.CreateHeaderLine(aircraftTable.Rows(0).Item("amod_make_name"), aircraftTable.Rows(0).Item("amod_model_name"), aircraftTable.Rows(0).Item("ac_ser_nbr"), MainHeading)

                            End If
                        End If

                    ElseIf ModelID > 0 Then
                        Dim ModelTable As New DataTable
                        If Client_AC_ID > 0 Then
                            ModelTable = CommonAircraftFunctions.BuildReusableTable(Client_AC_ID, 0, "JETNET", "", aclsData_Temp, True, 0, "JETNET")
                        Else
                            ModelTable = aclsData_Temp.Get_JETNET_Aircraft_Model_amodID(ModelID)
                        End If

                        If Not IsNothing(ModelTable) Then
                            If ModelTable.Rows.Count > 0 Then
                                MainHeading = "<strong>" & CStr(IIf(Not IsDBNull(ModelTable.Rows(0).Item("amod_make_name")), ModelTable.Rows(0).Item("amod_make_name"), "")) & " " & CStr(IIf(Not IsDBNull(ModelTable.Rows(0).Item("amod_model_name")), ModelTable.Rows(0).Item("amod_model_name"), "")) & "</strong> " & MainHeading
                            End If
                        End If

                    End If
                    HeaderText.Text = "<h2 class=""mainHeading padded_left"">" & MainHeading & "</h2>"
                    If Trim(Request("page_title")) <> "" Then
                        'HeaderText.Text = '"<strong>" & Trim(Request("page_title")) & "</strong>"
                        Master.SetPageTitle(Trim(Request("page_title")))
                    Else
                        ' HeaderText.Text = '"<strong>" & Trim(Request("Graph Page")) & "</strong>"
                        Master.SetPageTitle("Graph Page")
                    End If

                    ' sets the page title
                End If


                If Trim(temp_Graph_text) <> "" Then
                    HttpContext.Current.Session.Item("Residual_Chart_Java_PDF") = temp_Graph_text

                    graph_label.Text &= "<br/><a href=""#"" onclick=""javascript:load('viewtopdf.aspx?ViewID=300&residual=Y&title=" & Trim(Request("page_title")) & "','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');return false;"">Create PDF</a>"
                    create_pdf.Text = "<a href=""#"" onclick=""javascript:load('viewtopdf.aspx?ViewID=300&residual=Y&title=" & Trim(Request("page_title")) & "','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');return false;"">Create PDF</a>"
                End If


            Catch ex As Exception

            End Try




    End If

  End Sub

End Class