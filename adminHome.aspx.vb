' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/adminHome.aspx.vb $
'$$Author: Mike $
'$$Date: 6/15/20 10:33p $
'$$Modtime: 6/15/20 10:33p $
'$$Revision: 85 $
'$$Workfile: adminHome.aspx.vb $
'
' ********************************************************************************

Partial Public Class adminHome
  Inherits System.Web.UI.Page
  Private nMaxWidth As Long = 0
  Dim masterPage As New Object
  Public ModelID As Long = -1
  Protected localDatalayer As New admin_center_dataLayer
  Public type_of_selected As String = ""

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    If Session.Item("crmUserLogon") <> True Then

      Response.Redirect("Default.aspx", True)

    Else

      Dim sErrorString As String = ""

      If Not Session.Item("localPreferences").loadUserSession(sErrorString, CLng(HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString),
                                                        HttpContext.Current.Session.Item("localUser").crmUserLogin.ToString,
                                                        CLng(HttpContext.Current.Session.Item("localUser").crmSubSeqNo.ToString),
                                                        CLng(HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString)) Then
        Response.Redirect("Default.aspx", True)

      End If

      localDatalayer.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      localDatalayer.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
      localDatalayer.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
      localDatalayer.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
      localDatalayer.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim
      localDatalayer.crmMasterConnectStr = HttpContext.Current.Application.Item("crmMasterDatabase").ToString.Trim



      If Trim(Request.Item("type_of_selected")) <> "" Then
        type_of_selected = Trim(Request.Item("type_of_selected"))
      End If

      If Not IsPostBack Then
        commonEvo.fillMakeModelDropDown(ddlModelIntel, Nothing, nMaxWidth, "", -1, False, False, False, True, False, False) ' fill list with models

        BuildModuleList()
      End If

      Master.Set_Active_Tab(0)

      Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("Evolution Customer Center - Home")


    End If

  End Sub



  Private Sub btnModelIntel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnModelIntel.Click

    Dim htmlFunctionScript As String = ""

    If Not String.IsNullOrEmpty(ddlModelIntel.SelectedValue) Then
      If IsNumeric(ddlModelIntel.SelectedValue) And CLng(ddlModelIntel.SelectedValue) > 0 Then
        ModelID = CLng(ddlModelIntel.SelectedValue)

        htmlFunctionScript = vbCrLf + "<script type=""text/javascript"">" + vbCrLf
        htmlFunctionScript += "$(document).ready(function(){" + vbCrLf
        htmlFunctionScript += " openSmallWindowJS(""DisplayModelDetail.aspx?id=" + ModelID.ToString + """,""DisplayModelDetail"");" + vbCrLf
        htmlFunctionScript += "});" + vbCrLf
        htmlFunctionScript += "</script>" + vbCrLf

        System.Web.UI.ScriptManager.RegisterStartupScript(admin_home_panel, admin_home_panel.GetType(), "openModelWindow" + ModelID.ToString, htmlFunctionScript, False)

      End If
    End If

  End Sub


  Private Sub BuildModuleList()
    'Get a list of modules to display:
    'Loop through modules

    Dim moduleTable As New DataTable

    moduleTable = localDatalayer.DashboardModuleList(HttpContext.Current.Session.Item("localUser").crmSubSubID, HttpContext.Current.Session.Item("localUser").crmUserLogin, HttpContext.Current.Session.Item("localUser").crmSubSeqNo)


    DisplayModules(moduleTable)

  End Sub
  Private Sub DisplayModules(ModuleTable As DataTable)
    Dim ChartJavascript As String = ""
    Dim moduleCount As Integer = 1
    Dim oldArea As String = ""
    Dim chartType As String = ""
    Dim moduleWidth As String = "100%"
    Dim haveFooter As Boolean = False
    Dim footerCount As Integer = 0
    Dim sort As String = "desc"
    Dim sum_by As String = ""
    Dim modules_included As String = ""

    If Trim(Request("sum_by")) <> "" Then
      sum_by = Trim(Request("sum_by"))
    End If

    If Not IsNothing(ModuleTable) Then
      If ModuleTable.Rows.Count > 0 Then
        mainMenuRow.Visible = False
        For Each r As DataRow In ModuleTable.Rows

          haveFooter = False
          footerCount = 0

          Dim ModuleDataTable As New DataTable
          If Not IsDBNull(r("dashb_id")) Then
            If Not IsDBNull(r("dashb_width")) Then
              If IsNumeric(r("dashb_width")) Then
                moduleWidth = r("dashb_width") & "px"
              ElseIf InStr(r("dashb_width"), "px") > 0 Then
                moduleWidth = r("dashb_width")
              End If
            End If


            moduleLiteral.Text += "<div id=""div-" & r("dashb_id").ToString & """ class="" removeLeftMargin float_left"">"

            'Display Box Header
            moduleLiteral.Text += "<div class=""Box grid-item"" style=""width:auto !important;min-width:" & moduleWidth.ToString & " !important; max-width:98%; margin-bottom:10px;"">"
            If Not IsDBNull(r("dashb_display_title")) Then
              If Not String.IsNullOrEmpty(r("dashb_display_title")) Then
                moduleLiteral.Text += "<div class=""subHeader"">" & r("dashb_display_title").ToString & "</div>"
              End If
            End If

            If Not IsDBNull(r("dashb_display")) Then
              chartType = r("dashb_display")
            End If


            Select Case r("dashb_id")
              Case 1
                ModuleDataTable = localDatalayer.getModule1()
              Case 2
                ModuleDataTable = localDatalayer.getModule2()
              Case 3
                ModuleDataTable = localDatalayer.getModule3()
                chartType = "BAR CHART"
              Case 4
                moduleLiteral.Text += BuildModule4Datatable()
              Case 6
                ModuleDataTable = getContractActions()

                Dim distinct_table_view As New DataView
                Dim distinct_table As New DataTable
                distinct_table_view = ModuleDataTable.DefaultView
                distinct_table_view.Sort = "SERVICE"

                ''actually get the distinct values.
                distinct_table = distinct_table_view.ToTable(True, "SERVICE")

                moduleLiteral.Text += "Service:"
                moduleLiteral.Text += " <select id=""service_dropdown""><option value="""">All</option>"

                If Not IsNothing(distinct_table) Then
                  If distinct_table.Rows.Count > 0 Then
                    For Each q As DataRow In distinct_table.Rows
                      If Not IsDBNull(q(0)) Then
                        moduleLiteral.Text += "<option value=""" & q(0) & """>" & q(0) & "</option>"
                      End If
                    Next
                  End If
                End If


                Dim firstDayCurrentYear = New DateTime(Year(Now()), 1, 1)
                Dim timeSinceFirstDayCurrentYear = Now.Subtract(firstDayCurrentYear)
                Dim totalDaysSince As Integer = timeSinceFirstDayCurrentYear.Days ' + 1

                Dim firstDayCurrentMonth = New DateTime(Year(Now()), Month(Now()), 1)
                Dim timeSinceFirstDayCurrentMonth = Now.Subtract(firstDayCurrentMonth)
                Dim totalDaysSinceMonth As Integer = timeSinceFirstDayCurrentMonth.Days

                moduleLiteral.Text += "</select>&nbsp;&nbsp;"
                moduleLiteral.Text += "Timeframe: <select id=""timeframe_dropdown""><option value=""" & totalDaysSinceMonth.ToString & """>Current Month</option><option value=""P"">Previous Month</option><option value=""30"">Last 30 Days</option><option value=""90"">Last 90 Days</option><option value=""180"">Last 180 Days</option><option value=""" & totalDaysSince.ToString & """>Current Year</option><option value=""365"">Last 365 Days</option><option value=""PY"">Previous Year</option></select> &nbsp;&nbsp;Action: <select id=""action_dropdown""><option value="""">All</option>"

                distinct_table_view = New DataView
                distinct_table = New DataTable
                distinct_table_view = ModuleDataTable.DefaultView
                distinct_table_view.Sort = "ACTION"

                ''actually get the distinct values.
                distinct_table = distinct_table_view.ToTable(True, "ACTION")

                If Not IsNothing(distinct_table) Then
                  If distinct_table.Rows.Count > 0 Then
                    For Each q As DataRow In distinct_table.Rows
                      If Not IsDBNull(q(0)) Then
                        moduleLiteral.Text += "<option value=""" & q(0) & """>" & q(0) & "</option>"
                      End If
                    Next
                  End If
                End If
                moduleLiteral.Text += "</select>"

                moduleLiteral.Text += "&nbsp;&nbsp;Enterprise Group: <select id=""enterprise_dropdown""><option value="""">All</option>"


                distinct_table_view = New DataView
                distinct_table = New DataTable
                distinct_table_view = ModuleDataTable.DefaultView
                distinct_table_view.Sort = "ENTERPRISE GROUP"

                ''actually get the distinct values.
                distinct_table = distinct_table_view.ToTable(True, "ENTERPRISE GROUP")


                If Not IsNothing(distinct_table) Then
                  If distinct_table.Rows.Count > 0 Then
                    For Each q As DataRow In distinct_table.Rows
                      If Not IsDBNull(q(0)) Then
                        moduleLiteral.Text += "<option value=""" & q(0) & """>" & q(0) & "</option>"
                      End If
                    Next
                  End If
                End If


                moduleLiteral.Text += "</select>&nbsp;&nbsp;"

                haveFooter = True
                footerCount = 5

                buildModule6DataTableFilter(r("dashb_id"))
                moduleLiteral.Text += DisplayFunctions.ContractActionHTML(ModuleDataTable, r("dashb_id"))
              Case 7
                chartType = "LINE CHART"
                ModuleDataTable = localDatalayer.getModule7()

              Case 8
                chartType = "LINE CHART"
                ModuleDataTable = localDatalayer.getModule8()
              Case 9
                haveFooter = False
                footerCount = 0
                chartType = "TABLE"
                ModuleDataTable = getUpcomingContractActions()
                sort = "asc"
                moduleLiteral.Text += DisplayFunctions.ConvertUpcomingContractsHTML(ModuleDataTable, r("dashb_id"))
              Case 10
                chartType = "LINE CHART"
                ModuleDataTable = getCustomerNetValue(10)
              Case 11, 12
                chartType = "TICKER"
                Dim Total1, total_updown, total_marketplace, marketplace_updown, total_aerodex, aerodex_updown, total_last, marketplace_last, aerodex_last As Integer
                Call localDatalayer.ticker_selects(Total1, total_updown, total_marketplace, marketplace_updown, total_aerodex, aerodex_updown, total_last, marketplace_last, aerodex_last, IIf(r("dashb_id") = 12, True, False))

                moduleLiteral.Text += (DisplayFunctions.make_ticker_box_growth("Total", total_updown, FormatNumber(total_last, 0) & " LAST", FormatNumber(Total1, 0) & " NOW", False, False))
                moduleLiteral.Text += (DisplayFunctions.make_ticker_box_growth("MARKETPLACE", marketplace_updown, FormatNumber(marketplace_last, 0) & " LAST", FormatNumber(total_marketplace, 0) & " NOW", False, False))
                moduleLiteral.Text += (DisplayFunctions.make_ticker_box_growth("AERODEX", aerodex_updown, FormatNumber(aerodex_last, 0) & " LAST", FormatNumber(total_aerodex, 0) & " NOW", False, False))
                moduleLiteral.Text = Replace(moduleLiteral.Text, """row""", "")
              Case 13
                ModuleDataTable = getMyProspects("", "", "", "", "A")

                Dim distinct_table_view As New DataView
                Dim distinct_table As New DataTable
                distinct_table_view = ModuleDataTable.DefaultView
                distinct_table_view.Sort = "SERVICE"

                ''actually get the distinct values.
                distinct_table = distinct_table_view.ToTable(True, "SERVICE")

                moduleLiteral.Text += "Service:"
                moduleLiteral.Text += " <select id=""service_dropdown_13""><option value="""">All</option>"

                If Not IsNothing(distinct_table) Then
                  If distinct_table.Rows.Count > 0 Then
                    For Each q As DataRow In distinct_table.Rows
                      If Not IsDBNull(q(0)) Then
                        moduleLiteral.Text += "<option value=""" & q(0) & """>" & q(0) & "</option>"
                      End If
                    Next
                  End If
                End If


                'Dim firstDayCurrentYear = New DateTime(Year(Now()), 1, 1)
                'Dim timeSinceFirstDayCurrentYear = Now.Subtract(firstDayCurrentYear)
                'Dim totalDaysSince As Integer = timeSinceFirstDayCurrentYear.Days + 1

                'Dim firstDayCurrentMonth = New DateTime(Year(Now()), Month(Now()), 1)
                'Dim timeSinceFirstDayCurrentMonth = Now.Subtract(firstDayCurrentMonth)
                'Dim totalDaysSinceMonth As Integer = timeSinceFirstDayCurrentMonth.Days

                moduleLiteral.Text += "</select>&nbsp;&nbsp;"
                ' moduleLiteral.Text += "Timeframe: <select id=""timeframe_dropdown""><option value=""" & totalDaysSinceMonth.ToString & """>Current Month</option><option value=""P"">Previous Month</option><option value=""30"">Last 30 Days</option><option value=""90"">Last 90 Days</option><option value=""180"">Last 180 Days</option><option value=""" & totalDaysSince.ToString & """>Current Year</option><option value=""365"">Last 365 Days</option></select> "
                moduleLiteral.Text += "&nbsp;&nbsp;Stage: <select id=""action_dropdown_13""><option value="""">All</option>"

                distinct_table_view = New DataView
                distinct_table = New DataTable
                distinct_table_view = ModuleDataTable.DefaultView
                distinct_table_view.Sort = "TYPE"

                ''actually get the distinct values.
                distinct_table = distinct_table_view.ToTable(True, "TYPE")

                If Not IsNothing(distinct_table) Then
                  If distinct_table.Rows.Count > 0 Then
                    For Each q As DataRow In distinct_table.Rows
                      If Not IsDBNull(q(0)) Then
                        moduleLiteral.Text += "<option value=""" & q(0) & """>" & q(0) & "</option>"
                      End If
                    Next
                  End If
                End If
                moduleLiteral.Text += "</select>"

                'moduleLiteral.Text += "&nbsp;&nbsp;Enterprise Group: <select id=""enterprise_dropdown""><option value="""">All</option>"


                'distinct_table_view = New DataView
                'distinct_table = New DataTable
                'distinct_table_view = ModuleDataTable.DefaultView
                'distinct_table_view.Sort = "ENTERPRISE GROUP"

                'actually get the distinct values.
                'distinct_table = distinct_table_view.ToTable(True, "ENTERPRISE GROUP")


                'If Not IsNothing(distinct_table) Then
                '    If distinct_table.Rows.Count > 0 Then
                '        For Each q As DataRow In distinct_table.Rows
                '            If Not IsDBNull(q(0)) Then
                '                moduleLiteral.Text += "<option value=""" & q(0) & """>" & q(0) & "</option>"
                '            End If
                '        Next
                '    End If
                'End If


                'moduleLiteral.Text += "</select>&nbsp;&nbsp;"

                haveFooter = True
                footerCount = 9

                buildModule13DataTableFilter(r("dashb_id"))
                moduleLiteral.Text += DisplayFunctions.MyProspectsHTML(ModuleDataTable, r("dashb_id"))
              Case 14
                ' ACTION ITEMS
                Dim action_items_middle As String = ""
                ModuleDataTable = Create_Evo_Action_Items("")

              Case 15

                chartType = "COLUMN CHART"
                ModuleDataTable = getMyProspects("GraphType", "", sum_by, "")

              Case 16
                chartType = "COLUMN CHART"
                ModuleDataTable = getMyProspects("GraphType", "All", sum_by, type_of_selected)
                modules_included = "16"
              Case 17
                chartType = "COLUMN CHART"
                ModuleDataTable = getMyProspects("GraphService", "", sum_by, "")
              Case 18
                chartType = "COLUMN CHART"
                ModuleDataTable = getMyProspects("GraphService", "All", sum_by, "")
              Case 19
                chartType = "LINE CHART"
                ModuleDataTable = getCustomerNetValue(19)
              Case 21
                'My Closed Prospects by Year
                chartType = "COLUMN CHART"
                ModuleDataTable = localDatalayer.getModule21_closed_prospects("", sum_by)
              Case 22
                'My 2020 Sales/Support Activity Summarys
                chartType = "COLUMN CHART"
                ModuleDataTable = localDatalayer.getModule22_2020_sales_customer_support_activity("", sum_by)
              Case 23

                haveFooter = False
                footerCount = 0
                chartType = "TABLE"
                ModuleDataTable = getRecentErrors()
                sort = "desc"
                moduleLiteral.Text += ConvertModule23DataTable(ModuleDataTable, r("dashb_id"))

              Case 24
                '
                chartType = "LINE CHART"   ' really a column chart 
                ModuleDataTable = getCustomerNetValue(24)
              Case 25
                ModuleDataTable = get_My_Demos_Trials("", "", "")

                haveFooter = True
                footerCount = 0

                ' buildModule13DataTableFilter(r("dashb_id"))
                moduleLiteral.Text += DisplayFunctions.My_Demos_Trials_HTML(ModuleDataTable, r("dashb_id"))
              Case 38
                '2020 World-Wide Users by Day            
                chartType = "LINE CHART"
                ModuleDataTable = getModule38()
              Case 39
                '2020 EMEA Users by Day
                chartType = "LINE CHART"
                ModuleDataTable = getModule39()
              Case 42
                '2020 EMEA Users by Day
                chartType = "LINE CHART"
                ModuleDataTable = getModule42()

            End Select

            Select Case chartType
              Case "LINE CHART"

                moduleLiteral.Text += "<div class=""googleClear"" id=""linechart" & r("dashb_id") & """></div>"
                ChartJavascript += BuildLineChart(ModuleDataTable, r("dashb_id"))


              Case "PIE CHART"
                moduleLiteral.Text += "<div class=""googleClear"" id=""piechart" & r("dashb_id") & """></div>"
                ChartJavascript += BuildPieChart(ModuleDataTable, r("dashb_id"))

              Case "COMPLEX"
                                'ignore 
              Case "TICKER"
                                'ignore
              Case "TABLE"

                BuildJqueryDatatable(ModuleDataTable, r("dashb_id"), r("dashb_height"), haveFooter, footerCount, r("dashb_width"), sort)
              Case "BAR CHART"
                moduleLiteral.Text += "<div class=""googleClear"" id=""barchart" & r("dashb_id") & """></div>"
                ChartJavascript += BuildBarChart(ModuleDataTable, r("dashb_id"))
              Case "COLUMN CHART"



                If Not IsNothing(ModuleDataTable) Then
                  If ModuleDataTable.Rows.Count > 0 Then

                    If r("dashb_id") = 15 Or r("dashb_id") = 16 Or r("dashb_id") = 17 Or r("dashb_id") = 18 Or r("dashb_id") = 21 Then
                      If Trim(sum_by) = "price" Then
                        moduleLiteral.Text += "<A href='adminhome.aspx?sum_by=count'>Change to Count</a></br>"
                      Else
                        moduleLiteral.Text += "<A href='adminhome.aspx?sum_by=price'>Change to Value</a></br>"
                      End If
                    End If

                    moduleLiteral.Text += "<div class=""googleClear"" id=""columnchart" & r("dashb_id") & """></div>"
                    ChartJavascript += BuildColumnChart(ModuleDataTable, r("dashb_id"))
                  Else
                    moduleLiteral.Text += "No Prospects Found"
                  End If
                Else
                  moduleLiteral.Text += "No Prospects Found"
                End If


            End Select


            moduleLiteral.Text += "</div>"
            moduleLiteral.Text += "</div>"


            oldArea = r("dashb_area")
            moduleCount += 1
          End If
        Next

        moduleLiteral.Text = "<div class=""grid"">" & moduleLiteral.Text & "</div>"
        If ChartJavascript <> "" Then
          ChartJavascript = "function drawModuleCharts() {" & vbNewLine & ChartJavascript & vbNewLine & ";loadMasonry();};" & vbNewLine

          ChartJavascript += " google.charts.load('current', {packages: ['corechart']});" + vbNewLine
          ChartJavascript += " google.charts.setOnLoadCallback(drawModuleCharts);"

          ChartJavascript += "$(window).resize(function() {" & vbNewLine
          ChartJavascript += "if(this.resizeTO) clearTimeout(this.resizeTO);" & vbNewLine
          ChartJavascript += "this.resizeTO = setTimeout(function() {" & vbNewLine
          ChartJavascript += "$(this).trigger('resizeEnd');" & vbNewLine
          ChartJavascript += "}, 500);" & vbNewLine
          ChartJavascript += "});" & vbNewLine

          '//redraw graph when window resize is completed  
          ChartJavascript += "$(window).on('resizeEnd', function() {" & vbNewLine
          ChartJavascript += "$('.googleClear').empty(); " & vbNewLine
          ChartJavascript += "  drawModuleCharts();" & vbNewLine
          ChartJavascript += "});" & vbNewLine




          System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "DrawChart", ChartJavascript, True)
        End If







      End If
    End If
  End Sub
  Private GoogleColors() As String = {"#3366cc", "#dc3912", "#ff9900", "#15970b", "#3366cc", "#dc3912", "#ff9900", "#15970b", "#3366cc", "#dc3912", "#ff9900", "#15970b"}

  Sub buildModule6DataTableFilter(moduleID As Integer)
    Dim FilterJS As String = ""
    FilterJS = "  $.fn.dataTable.ext.search.push(" & vbNewLine
    FilterJS += "function (settings, data, dataIndex) {" & vbNewLine

    FilterJS += "if (settings.nTable.getAttribute('id') == 'table_" & moduleID.ToString & "') {"
    FilterJS += " //We need to set booleans for the filter return" & vbNewLine
    FilterJS += "var actionFilter = true;" & vbNewLine
    FilterJS += "var serviceNoFilter = true;" & vbNewLine
    FilterJS += "var saleDateFilter = true;" & vbNewLine
    FilterJS += "var enterpriseFilter = true;" & vbNewLine
    FilterJS += "var row = $.fn.dataTable.Api(settings).row(dataIndex).nodes();" & vbNewLine
    FilterJS += "var KeepRemove = ""remove"";" & vbNewLine
    FilterJS += "checkFilter = ($(row).hasClass('gone') ? false : true);" & vbNewLine

    FilterJS += " switch (KeepRemove) {" & vbNewLine
    FilterJS += " case ""remove"":" & vbNewLine
    FilterJS += "if ($(row).hasClass('remove')) {" & vbNewLine
    FilterJS += "$(row).removeClass('remove');" & vbNewLine
    FilterJS += "$(row).removeClass('keep');" & vbNewLine
    FilterJS += "$(row).addClass('gone');" & vbNewLine
    FilterJS += "checkFilter = false;" & vbNewLine
    FilterJS += "}" & vbNewLine
    FilterJS += "break;" & vbNewLine
    FilterJS += " default:" & vbNewLine
    FilterJS += "if ($(row).hasClass('keep')) {" & vbNewLine
    FilterJS += " $(row).removeClass('remove');" & vbNewLine
    FilterJS += "$(row).removeClass('keep');" & vbNewLine
    FilterJS += "$(row).removeClass('gone');" & vbNewLine
    FilterJS += "checkFilter = true;" & vbNewLine
    FilterJS += "} else {" & vbNewLine
    FilterJS += "$(row).removeClass('remove');" & vbNewLine
    FilterJS += "$(row).removeClass('keep');" & vbNewLine
    FilterJS += "$(row).addClass('gone');" & vbNewLine
    FilterJS += "checkFilter = false;" & vbNewLine
    FilterJS += "};" & vbNewLine
    FilterJS += "}" & vbNewLine



    FilterJS += " var value = $('#service_dropdown').val(); //checked_radio.val();" & vbNewLine
    FilterJS += "var serviceNo = data[1] || ''; // use data for the reg column" & vbNewLine


    FilterJS += "if (serviceNo.toUpperCase() == value.toUpperCase()) {" & vbNewLine
    FilterJS += " serviceNoFilter =true;" & vbNewLine
    FilterJS += "} else if (value == '') {" & vbNewLine
    FilterJS += "serviceNoFilter = true;" & vbNewLine
    FilterJS += "} else {" & vbNewLine
    FilterJS += "serviceNoFilter = false;" & vbNewLine
    FilterJS += "}" & vbNewLine




    FilterJS += " var enterPriseVal = $('#enterprise_dropdown').val(); //checked_radio.val();" & vbNewLine
    FilterJS += "var eData = data[7] || ''; // " & vbNewLine


    FilterJS += "if (eData.toUpperCase() == enterPriseVal.toUpperCase()) {" & vbNewLine
    FilterJS += " enterpriseFilter =true;" & vbNewLine
    FilterJS += "} else if (enterPriseVal == '') {" & vbNewLine
    FilterJS += "enterpriseFilter = true;" & vbNewLine
    FilterJS += "} else {" & vbNewLine
    FilterJS += "enterpriseFilter = false;" & vbNewLine
    FilterJS += "}" & vbNewLine

    '        ////////////////////////////////////////////////
    FilterJS += "var actvalue = $('#action_dropdown').val();" & vbNewLine
    FilterJS += "var actionNo = data[6] || ''; // use data for the reg column" & vbNewLine
    '       // console.log(actionNo.toUpperCase() + ' ' + actvalue.toUpperCase());

    FilterJS += "if (actionNo.toUpperCase() == actvalue.toUpperCase()) {" & vbNewLine
    FilterJS += "actionFilter = true;" & vbNewLine
    FilterJS += "} else if (actvalue == '') {" & vbNewLine
    FilterJS += "actionFilter = true;" & vbNewLine
    FilterJS += "} else {" & vbNewLine
    FilterJS += "actionFilter = false;" & vbNewLine
    FilterJS += "}" & vbNewLine


    '  ////////////////////////////////////////////////
    FilterJS += " var dateMin, dateMax, dateCol; "
    FilterJS += " dateCol = moment(data[0], ""MM/DD/YYYY"") || '';" & vbNewLine
    FilterJS += " if ($('#timeframe_dropdown').val() == 'PY') { "

    FilterJS += " var timeTest = moment().subtract(1, 'years');"
    FilterJS += " var timeTestYear = '01/01/' + timeTest.format('YYYY');"
    FilterJS += " var timeTestCur = moment();"
    FilterJS += " var timeTestCurYear = '01/01/' + timeTestCur.format('YYYY');"
    FilterJS += " timeTestCurYear = moment(timeTestCurYear).subtract(1,'days');"
    FilterJS += " timeTestCurYear = timeTestCurYear.format(""MM/DD/YYYY"");"
    FilterJS += " dateMin = new Date(timeTestYear); "
    FilterJS += " dateMax = new Date(timeTestCurYear); "


    FilterJS += " } else if  ($('#timeframe_dropdown').val() == 'P') { "

    FilterJS += " var timeTest = moment().subtract(1, 'months');"
    FilterJS += " var timeTestMonth = timeTest.format('M') + '/01/' + timeTest.format('YYYY');"
    FilterJS += " var timeTestCur = moment();"
    FilterJS += " var timeTestCurMonth = timeTestCur.format('M') + '/01/' + timeTestCur.format('YYYY');"
    FilterJS += " timeTestCurMonth = moment(timeTestCurMonth).subtract(1,'days');"
    FilterJS += " timeTestCurMonth = timeTestCurMonth.format(""MM/DD/YYYY"");"
    FilterJS += " dateMin = new Date(timeTestMonth); "
    FilterJS += " dateMax = new Date(timeTestCurMonth);"
    FilterJS += " } else { "
    FilterJS += " dateMin = new Date(moment().subtract($('#timeframe_dropdown').val(), 'days').format(""MM/DD/YYYY""));" & vbNewLine
    FilterJS += " dateMax = new Date(moment().format(""MM/DD/YYYY""));" & vbNewLine
    FilterJS += " } "



    'alert(timeTestMonth + ' ' + timeTestCurMonth);

    FilterJS += "if (dateCol.isBetween(dateMin, dateMax, 'days', '[]')) {" & vbNewLine
    FilterJS += "saleDateFilter = true;" & vbNewLine
    FilterJS += "} else {" & vbNewLine
    FilterJS += "saleDateFilter = false;" & vbNewLine
    FilterJS += "}" & vbNewLine

    FilterJS += "if (serviceNoFilter && saleDateFilter && actionFilter && enterpriseFilter)  {" & vbNewLine
    FilterJS += " return true;" & vbNewLine
    FilterJS += "} else {" & vbNewLine
    FilterJS += "return false;" & vbNewLine
    FilterJS += "}" & vbNewLine

    FilterJS += "} else { return true;  }" & vbNewLine
    FilterJS += "});                                   //Entire function end." & vbNewLine


    FilterJS += "$(""#service_dropdown"").change(function () {" & vbNewLine
    FilterJS += " $('#table_" & moduleID.ToString & "').DataTable().draw();" & vbNewLine
    FilterJS += "});" & vbNewLine
    FilterJS += "$(""#timeframe_dropdown"").change(function () {" & vbNewLine
    FilterJS += " $('#table_" & moduleID.ToString & "').DataTable().draw();" & vbNewLine
    FilterJS += "});" & vbNewLine

    FilterJS += "$(""#action_dropdown"").change(function () {" & vbNewLine
    FilterJS += "  $('#table_" & moduleID.ToString & "').DataTable().draw();" & vbNewLine
    FilterJS += "});" & vbNewLine

    FilterJS += "$(""#enterprise_dropdown"").change(function () {" & vbNewLine
    FilterJS += "  $('#table_" & moduleID.ToString & "').DataTable().draw();" & vbNewLine
    FilterJS += "});" & vbNewLine




    System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "FilterTable" & moduleID.ToString, FilterJS, True)
  End Sub


  Sub buildModule13DataTableFilter(moduleID As Integer)
    Dim FilterJS As String = ""
    FilterJS = "  $.fn.dataTable.ext.search.push(" & vbNewLine
    FilterJS += "function (settings, data, dataIndex) {" & vbNewLine

    'This line up here takes the ID and makes sure that you're only running this filter on the specified datatable. The extension would run for all of them
    'So if you had two datatables, one with this filter and that wasn't here, it would make the other datatable also try filtering. 
    FilterJS += "if (settings.nTable.getAttribute('id') == 'table_" & moduleID.ToString & "') {"
    FilterJS += " //We need to set booleans for the filter return" & vbNewLine
    'Setting up the 2 filter booleans that we set to true
    FilterJS += "var actionFilter = true;" & vbNewLine
    FilterJS += "var serviceNoFilter = true;" & vbNewLine
    FilterJS += "var row = $.fn.dataTable.Api(settings).row(dataIndex).nodes();" & vbNewLine

    'Ignore this part unless the datatable is going to have the option of checkbox removal - keep all, remove all.
    'I have chosen to keep this in because I am never really sure if a datatable will need those buttons and features.
    'If you're sure this one doesn't, you should be able to just remove it. 
    FilterJS += "var KeepRemove = ""remove"";" & vbNewLine
    FilterJS += "checkFilter = ($(row).hasClass('gone') ? false : true);" & vbNewLine
    FilterJS += " switch (KeepRemove) {" & vbNewLine
    FilterJS += " case ""remove"":" & vbNewLine
    FilterJS += "if ($(row).hasClass('remove')) {" & vbNewLine
    FilterJS += "$(row).removeClass('remove');" & vbNewLine
    FilterJS += "$(row).removeClass('keep');" & vbNewLine
    FilterJS += "$(row).addClass('gone');" & vbNewLine
    FilterJS += "checkFilter = false;" & vbNewLine
    FilterJS += "}" & vbNewLine
    FilterJS += "break;" & vbNewLine
    FilterJS += " default:" & vbNewLine
    FilterJS += "if ($(row).hasClass('keep')) {" & vbNewLine
    FilterJS += " $(row).removeClass('remove');" & vbNewLine
    FilterJS += "$(row).removeClass('keep');" & vbNewLine
    FilterJS += "$(row).removeClass('gone');" & vbNewLine
    FilterJS += "checkFilter = true;" & vbNewLine
    FilterJS += "} else {" & vbNewLine
    FilterJS += "$(row).removeClass('remove');" & vbNewLine
    FilterJS += "$(row).removeClass('keep');" & vbNewLine
    FilterJS += "$(row).addClass('gone');" & vbNewLine
    FilterJS += "checkFilter = false;" & vbNewLine
    FilterJS += "};" & vbNewLine
    FilterJS += "}" & vbNewLine

    '////////////////////////////////////////////////
    'This is the service dropdown. Value stores the dropdown value and serviceNo stores the data in the text field we're comparing it to. 
    FilterJS += " var value = $('#service_dropdown_13').val(); //checked_radio.val();" & vbNewLine
    FilterJS += "var serviceNo = data[2] || ''; // use data for the reg column" & vbNewLine

    'This is just a comparison to toggle the row on or off.
    FilterJS += "if (serviceNo.toUpperCase() == value.toUpperCase()) {" & vbNewLine
    FilterJS += " serviceNoFilter =true;" & vbNewLine
    FilterJS += "} else if (value == '') {" & vbNewLine
    FilterJS += "serviceNoFilter = true;" & vbNewLine
    FilterJS += "} else {" & vbNewLine
    FilterJS += "serviceNoFilter = false;" & vbNewLine
    FilterJS += "}" & vbNewLine

    '////////////////////////////////////////////////
    'Action dropdown, actvalue stores the dropdown value. actionNo stores the data for that column.
    FilterJS += "var actvalue = $('#action_dropdown_13').val();" & vbNewLine
    FilterJS += "var actionNo = data[3] || ''; " & vbNewLine


    'Simple comparison, making sure to make case the same.
    FilterJS += "if (actionNo.toUpperCase() == actvalue.toUpperCase()) {" & vbNewLine
    FilterJS += "actionFilter = true;" & vbNewLine
    FilterJS += "} else if (actvalue == '') {" & vbNewLine
    FilterJS += "actionFilter = true;" & vbNewLine
    FilterJS += "} else {" & vbNewLine
    FilterJS += "actionFilter = false;" & vbNewLine
    FilterJS += "}" & vbNewLine

    '////////////////////////////////////////////////
    'This return true if service filter and action filter are true, otherwise false.
    'This return tells whether or not the row is going to stick around or not.
    FilterJS += "if (serviceNoFilter &&  actionFilter )  {" & vbNewLine
    FilterJS += " return true;" & vbNewLine
    FilterJS += "} else {" & vbNewLine
    FilterJS += "return false;" & vbNewLine
    FilterJS += "}" & vbNewLine

    FilterJS += "} else { return true;  }" & vbNewLine
    FilterJS += "}); //Entire function end." & vbNewLine

    '////////////////////////////////////////////////
    'These following two change functions force the datatable to draw any time the dropdown is triggered. Redrawing forces the filtering. Kind of like how searching on the table would work, except without having to search
    'and using a dropdown instead.
    FilterJS += "$(""#service_dropdown_13"").change(function () {" & vbNewLine
    FilterJS += " $('#table_" & moduleID.ToString & "').DataTable().draw();" & vbNewLine
    FilterJS += "});" & vbNewLine


    FilterJS += "$(""#action_dropdown_13"").change(function () {" & vbNewLine
    FilterJS += "  $('#table_" & moduleID.ToString & "').DataTable().draw();" & vbNewLine
    FilterJS += "});" & vbNewLine




    'I added an ID to the function name parameter, that way they'd both work.
    System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "FilterTable" & moduleID.ToString, FilterJS, True)
  End Sub

  Private Sub BuildJqueryDatatable(moduleTable As DataTable, moduleNumber As Integer, moduleHeight As Object, haveFooter As Boolean, footerCount As Integer, width As Integer, sort As String)
    Dim scriptBu As String = ""
    Dim excelButton As String = ""
    If IsDBNull(moduleHeight) Then
      moduleHeight = 430
    End If

    scriptBu += "jQuery.fn.dataTable.Api.register( 'sum()', function () {"
    scriptBu += "return this.flatten().reduce(Function(a, b) {"
    scriptBu += "return (a*1) + (b*1); // cast values in-case they are strings"
    scriptBu += "});"
    scriptBu += "});"

    scriptBu = "function BuildModuleTable_" & moduleNumber.ToString & "() {"
    scriptBu += "var cw = $('#div-" & moduleNumber & "').width() - 10;"

    scriptBu += "$(window).resize(function() {"
    scriptBu += "var cw = $('#div-" & moduleNumber & "').width() - 10;"
    scriptBu += "});"



    scriptBu += "var dtTable = $('#table_" & moduleNumber.ToString & "').DataTable({"
    scriptBu += "destroy: true,"
    scriptBu += "fixedHeader: true, "
    scriptBu += "scrollX: cw,"
    scriptBu += "scrollY: " & moduleHeight & ","
    scriptBu += "order: [[ 0, '" & sort & "' ]],"
    scriptBu += """initComplete"": function(settings, json) {"
    scriptBu += "setTimeout(function(){"
    scriptBu += "$('#table_" & moduleNumber.ToString & "').DataTable().columns.adjust();"
    scriptBu += "$('#table_" & moduleNumber.ToString & "').DataTable().fixedColumns().relayout();"
    scriptBu += "},1200)"
    scriptBu += "},"
    If moduleNumber = 23 Then
      scriptBu += "autoWidth: false,"
    End If
    scriptBu += "scrollCollapse: true,"

    scriptBu += "buttons: [ "
    'Excel Button
    clsGeneral.clsGeneral.CreateExcelButton(excelButton, "table_" & moduleNumber.ToString)

    scriptBu += excelButton
    scriptBu += "], "


    scriptBu += " stateSave: true,"
    scriptBu += "paging: false, "

    scriptBu += "dom: 'Bfitrp'"

    If haveFooter Then
      scriptBu += ","

      scriptBu += """footerCallback"": function ( row, data, start, end, display ) {;"
      scriptBu += "var api = this.api(), data;"

      ''// Remove the formatting to get integer data for summation
      scriptBu += "var intVal = function ( i ) {"
      scriptBu += "return typeof i === 'string' ?"
      scriptBu += "i.replace(/[\$,]/g, '').replace(/<[^>]+>/ig,'')*1 :"
      scriptBu += "typeof i === 'number' ?"
      scriptBu += "i : 0;"
      scriptBu += "};"

      If footerCount > 0 Then
        scriptBu += "total = api.column(" & footerCount & ", {""filter"": ""applied""} ).data().reduce( function (a, b) {"
        scriptBu += "return intVal(a) + intVal(b);"
        scriptBu += "}, 0 );"

        '// Update footer
        'scriptBu += "if (Math.round(total) !== total) {"
        'scriptBu += "total = total.toFixed(2);"
        'scriptBu += "}"

        scriptBu += "$( api.column(" & footerCount & ").footer() ).html('<span>' + "
        scriptBu += "total.toLocaleString('en', {style:'currency', currency:'USD'})"

        scriptBu += "+ '</span>');"


        scriptBu += "$( api.column(" & footerCount - 1 & ").footer() ).html("
        scriptBu += "'Totals:'"
        scriptBu += ");"
      End If




      scriptBu += "}"

    End If

    scriptBu += " });"


    If haveFooter Then

      scriptBu += " dtTable.on( 'search.dt', function () {"

      scriptBu += "var api = $('#table_" & moduleNumber.ToString & "').DataTable();"
      scriptBu += "var intVal = function ( i ) {"
      scriptBu += "return typeof i === 'string' ?"
      scriptBu += "i.replace(/[\$,]/g, '').replace(/<[^>]+>/ig,'')*1 :"
      scriptBu += "typeof i === 'number' ?"
      scriptBu += "i : 0;"
      scriptBu += "};"
      scriptBu += "total = dtTable.column( 5, {""filter"": ""applied""} ).data().reduce( function (a, b) {return intVal(a) + intVal(b)}, 0 );"
      scriptBu += "if (Math.round(total) !== total) {"
      scriptBu += "total = total.toFixed(2);"
      scriptBu += "}"

      scriptBu += "$( api.column(5).footer() ).html('<span>' + "
      scriptBu += "total.toLocaleString('en')"

      scriptBu += "+ '</span>');"


      scriptBu += "$( api.column(4).footer() ).html("
      scriptBu += "'Totals:'"
      scriptBu += ");"
      scriptBu += " } );"
    End If

    scriptBu += "$($.fn.dataTable.tables(true)).DataTable().columns.adjust();"
    scriptBu += "$($.fn.dataTable.tables(true)).DataTable().scroller.measure();"
    scriptBu += "};BuildModuleTable_" & moduleNumber.ToString & "();"




    System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "BuildModuleTable_" & moduleNumber.ToString, scriptBu.ToString, True)
  End Sub


  Private Function BuildBarChart(ModuleTable As DataTable, moduleNumber As Integer) As String
    Dim JavaScriptString As String = ""
    Dim InnerScript As String = ""
    Dim colorCount As Integer = 0
    JavaScriptString += " var Data" & moduleNumber.ToString & " = google.visualization.arrayToDataTable([" & vbNewLine
    JavaScriptString += "['Program', '#', { role: 'annotation'}, { role: 'style' }]," & vbNewLine
    For Each r As DataColumn In ModuleTable.Columns

      If InnerScript <> "" Then
        InnerScript += ", "
      End If
      InnerScript += "['" & r.ColumnName & "',     " & (ModuleTable.Rows(0).Item(r.ColumnName).ToString).ToString & ", '" & r.ColumnName & "','" & GoogleColors(colorCount) & "']" & vbNewLine
      colorCount += 1
    Next
    JavaScriptString += InnerScript
    JavaScriptString += "]); " & vbNewLine
    JavaScriptString += "var options" & moduleNumber.ToString & " = {" & vbNewLine
    JavaScriptString += " legend: 'none'," & vbNewLine
    JavaScriptString += " chartArea: {left:0,top:0,width:'100%',height:'90%'}"

    ' added in MSW - 3/20/20  
    If moduleNumber = 18 Or moduleNumber = 24 Then
      JavaScriptString += " , hAxis: { "
      JavaScriptString += " slantedText: true, "
      JavaScriptString += " textStyle: { fontSize: 10 }, "
      JavaScriptString += " slantedTextAngle: 80 "
      JavaScriptString += " } "
    End If
    JavaScriptString += "};" & vbNewLine



    JavaScriptString += " var chart" & moduleNumber.ToString & " = new google.visualization.BarChart(document.getElementById('barchart" & moduleNumber.ToString & "'));" & vbNewLine
    JavaScriptString += " chart" & moduleNumber.ToString & ".draw(Data" & moduleNumber.ToString & ", options" & moduleNumber.ToString & "); " & vbNewLine

    Return JavaScriptString
  End Function


  Private Function BuildColumnChart(ModuleTable As DataTable, moduleNumber As Integer) As String
    Dim JavaScriptString As String = ""
    Dim InnerScript As String = ""
    Dim colorCount As Integer = 0
    Dim inner_count As Integer = 0
    Dim last_column_name As String = ""
    Dim i As Integer = 0
    Dim temp_service As String = ""

    JavaScriptString += " var Data" & moduleNumber.ToString & " = google.visualization.arrayToDataTable([" & vbNewLine

    If moduleNumber = 15 Or moduleNumber = 16 Then
      JavaScriptString += "['Type', '#', { role: 'style' }]," & vbNewLine
    Else
      JavaScriptString += "['Service', '#', { role: 'style' }]," & vbNewLine
    End If

    For Each k As DataRow In ModuleTable.Rows

      If moduleNumber = 15 Or moduleNumber = 16 Or moduleNumber = 22 Then
        If Not IsDBNull(k.Item("Type")) And Not IsDBNull(k.Item("Value")) Then
          If Trim(InnerScript) <> "" Then
            InnerScript += ", "
          End If

          If Trim(type_of_selected) <> "" And moduleNumber = 16 Then
            InnerScript += "['" & k.Item("cprospect_assigned_to").ToString & " - " & k.Item("Type").ToString & "',"
          Else
            InnerScript += "['" & (k.Item("Type").ToString).ToString & "',"
          End If


          InnerScript += "" & Replace(FormatNumber(k.Item("Value").ToString, 0), ",", "") & ""
          ' InnerScript += ", '" & k.Item("Type").ToString & "'"
          InnerScript += ",'" & GoogleColors(colorCount) & "']" & vbNewLine
        End If
      ElseIf moduleNumber = 21 Then
        If Not IsDBNull(k.Item("Year")) And Not IsDBNull(k.Item("Value")) Then
          If Trim(InnerScript) <> "" Then
            InnerScript += ", "
          End If

          InnerScript += "['" & (k.Item("Year").ToString).ToString & "',"
          InnerScript += "" & Replace(FormatNumber(k.Item("Value").ToString, 0), ",", "") & ""
          ' InnerScript += ", '" & k.Item("Type").ToString & "'"
          InnerScript += ",'" & GoogleColors(colorCount) & "']" & vbNewLine
        End If
      ElseIf moduleNumber = 17 Or moduleNumber = 18 Then
        If Not IsDBNull(k.Item("Service")) And Not IsDBNull(k.Item("Value")) Then
          If Trim(InnerScript) <> "" Then
            InnerScript += ", "
          End If
          temp_service = k.Item("Service").ToString
          temp_service = Replace(temp_service, "Marketplace Manager", "MPM")
          temp_service = Replace(temp_service, "Marketplace", "MP")
          temp_service = Replace(temp_service, "JETNET", "JN")
          temp_service = Replace(temp_service, "Aerodex", "Aero")
          temp_service = Replace(temp_service, "Yacht", "YT")
          temp_service = Replace(temp_service, "Standard", "Std")
          temp_service = Replace(temp_service, "Salesforce", "SF")
          temp_service = Replace(temp_service, "Yacht", "YT")
          temp_service = Replace(temp_service, "Yacht", "YT")

          InnerScript += "['" & temp_service & "',"
          InnerScript += "" & Replace(FormatNumber(k.Item("Value").ToString, 0), ",", "") & ""
          ' InnerScript += ", '" & k.Item("Service").ToString & "'"
          InnerScript += ",'" & GoogleColors(colorCount) & "']" & vbNewLine
        End If
      End If


      '        If inner_count = 0 Then
      '            inner_count = 1
      '            InnerScript += "['" & (ModuleTable.Rows(i).Item(r.ColumnName).ToString).ToString & "',"
      '            last_column_name = ModuleTable.Rows(i).Item(r.ColumnName).ToString
      '        ElseIf inner_count = 1 Then
      '            inner_count = 2
      '            InnerScript += "" & (ModuleTable.Rows(i).Item(r.ColumnName).ToString).ToString & ", '" & last_column_name & "','" & GoogleColors(colorCount) & "']" & vbNewLine
      '            last_column_name = ""
      '        End If

      'For Each r As DataColumn In ModuleTable.Columns    ' was originally done w columns, left in, in case isnt working 

      '    If inner_count = 2 Then
      '        InnerScript += ", "
      '        inner_count = 0
      '    End If

      '    '"['TYPE', Lead]," & vbCrLf  
      '    '"['VALUE', 200]" & vbCrLf

      '    If moduleNumber = 15 Or moduleNumber = 16 Then

      '        If inner_count = 0 Then
      '            inner_count = 1
      '            InnerScript += "['" & (ModuleTable.Rows(i).Item(r.ColumnName).ToString).ToString & "',"
      '            last_column_name = ModuleTable.Rows(i).Item(r.ColumnName).ToString
      '        ElseIf inner_count = 1 Then
      '            inner_count = 2
      '            InnerScript += "" & (ModuleTable.Rows(i).Item(r.ColumnName).ToString).ToString & ", '" & last_column_name & "','" & GoogleColors(colorCount) & "']" & vbNewLine
      '            last_column_name = ""
      '        End If

      '    ElseIf moduleNumber = 17 Or moduleNumber = 18 Then
      '        If inner_count = 0 Then
      '            inner_count = 1
      '            InnerScript += "['" & (ModuleTable.Rows(i).Item(r.ColumnName).ToString).ToString & "',"
      '            last_column_name = ModuleTable.Rows(i).Item(r.ColumnName).ToString
      '        ElseIf inner_count = 1 Then
      '            inner_count = 2
      '            InnerScript += "" & (ModuleTable.Rows(i).Item(r.ColumnName).ToString).ToString & ", '" & last_column_name & "','" & GoogleColors(colorCount) & "']" & vbNewLine
      '            last_column_name = ""
      '        End If
      '    End If


      '    ' InnerScript += "['" & r.ColumnName & "',     " & (ModuleTable.Rows(0).Item(r.ColumnName).ToString).ToString & ", '" & r.ColumnName & "','" & GoogleColors(colorCount) & "']" & vbNewLine
      colorCount += 1
      'Next 
      ' i += 1
    Next
    JavaScriptString += InnerScript
    JavaScriptString += "]); " & vbNewLine
    JavaScriptString += "var options" & moduleNumber.ToString & " = {"
    JavaScriptString += " legend: 'none',"

    If moduleNumber = 15 Or moduleNumber = 16 Or moduleNumber = 22 Then
      JavaScriptString += " chartArea: {left:45,top:6,width:'83%',height:'76%'}"
    ElseIf moduleNumber = 17 Or moduleNumber = 18 Then
      JavaScriptString += " chartArea: {left:45,top:6,width:'83%',height:'70%'}"
    Else
      JavaScriptString += " chartArea: {left:45,top:6,width:'84%',height:'85%'}"
    End If

    JavaScriptString += " , vAxis: { textStyle: { fontSize: 9 } } "

    If moduleNumber = 15 Or moduleNumber = 16 Or moduleNumber = 22 Then ' commented out MSW - 3/20/20
      ' JavaScriptString += " , hAxis: { "
      ' JavaScriptString += " slantedText: true, "
      ' JavaScriptString += " textStyle: { fontSize: 10 }, "
      ' JavaScriptString += " slantedTextAngle: 1 "
      ' JavaScriptString += " } "
    ElseIf moduleNumber = 17 Then
      JavaScriptString += " , hAxis: { "
      JavaScriptString += " slantedText: true, "
      JavaScriptString += " textStyle: { fontSize: 10 }, "
      JavaScriptString += " slantedTextAngle: 30 "
      JavaScriptString += " } "
    ElseIf moduleNumber = 18 Then
      JavaScriptString += " , hAxis: { "
      JavaScriptString += " slantedText: true, "
      JavaScriptString += " textStyle: { fontSize: 9 }, "
      JavaScriptString += " slantedTextAngle: 30 "
      JavaScriptString += " } "
    Else
      JavaScriptString += " , hAxis: { "
      JavaScriptString += " slantedText: true, "
      JavaScriptString += " textStyle: { fontSize: 10 }, "
      JavaScriptString += " slantedTextAngle: 80 "
      JavaScriptString += " } "
    End If


    JavaScriptString += "};" & vbNewLine
    JavaScriptString += " var chart" & moduleNumber.ToString & " = new google.visualization.ColumnChart(document.getElementById('columnchart" & moduleNumber.ToString & "'));" & vbNewLine

    If moduleNumber = 16 Then

      JavaScriptString += "   function redirect() {   "
      '  JavaScriptString += "   alert('The user selected ');  "

      JavaScriptString += "    var selectedItem = chart" & moduleNumber.ToString & ".getSelection()[0];  "

      JavaScriptString += "    if (selectedItem) {  "
      JavaScriptString += " var itemselected = Data16.getValue(selectedItem.row, 0); "
      ' JavaScriptString += " alert('The user selected ' + itemselected); "
      JavaScriptString += "   }   "

      ' JavaScriptString += "   window.location.href = ""adminHome.aspx?type_of_selected="" + itemselected;   "
      ' JavaScriptString += "   window.open(""adminHome.aspx?type_of_selected="" + itemselected);   "
      JavaScriptString += "   window.open(""view_template.aspx?ViewID=18&ViewName=Prospect+Management&noMaster=false&user_of_selected=All&type_of_selected="" + itemselected);   "


      JavaScriptString += "    }   " & vbNewLine

      JavaScriptString += "   google.visualization.events.addListener(chart16, 'select', redirect); " & vbNewLine
    End If

    JavaScriptString += " chart" & moduleNumber.ToString & ".draw(Data" & moduleNumber.ToString & ", options" & moduleNumber.ToString & "); " & vbNewLine

    Return JavaScriptString
  End Function

  Private Function BuildModule4Datatable() As String
    Dim ReturnString As String = ""
    Dim Module4Datatable As New DataTable
    Module4Datatable.Columns.Add("VALID_ESTIMATES")
    Module4Datatable.Columns.Add("ESTIMATES_TODAY")
    Module4Datatable.Columns.Add("AC_ON_PROBATION")
    Module4Datatable.Columns.Add("FAILED_ESTIMATES")

    '-- EVALUES BLOCK


    '-- COUNT OF VALID EVALUE ESTIMATES - AS TOTAL ESTIMATES
    Dim CountofEvalue As Long = localDatalayer.getEvaluesCount()

    ReturnString = "<div class=""row""><div class=""six columns""><strong>TOTAL ESTIMATES:</strong> " & FormatNumber(CountofEvalue, 0).ToString & "</div>"

    '-- COUNT OF EVALUE ESTIMATES FROM TODAY - AS ESTIMATES TODAY
    Dim CountOfEstimates As Long = getLatestEstimatesCount()
    ReturnString += "<div class=""six columns""><strong>ESTIMATES TODAY:</strong> " & FormatNumber(CountOfEstimates, 0).ToString & "</div></div>"

    '-- COUNT OF AIRCRAFT ON PROBATION - AS ON PROBATION
    Dim CountofProbabtion As Long = localDatalayer.getAircraftOnProbationCount()
    ReturnString += "<div class=""row""><div class=""six columns""><a href=""/adminSummary.aspx?rid=106""><strong>PROBATION COUNT:</strong></a> " & FormatNumber(CountofProbabtion, 0).ToString & "</div>"
    '-- COUNT OF ESTIMATE FAILURES TODAY - AS FAILED ESTIMATES
    Dim failedEstimates As Long = getFailedEstimates()
    ReturnString += "<div class=""six columns""><strong>FAILED ESTIMATES:</strong> " & FormatNumber(failedEstimates, 0).ToString & "</div></div><div class=""gaugeCanvas""><canvas id=""evalueCount""></canvas></div>"


    '-- GAUGE FOR ESTIMATE STATUS (% GOOD ESTIMATES)
    '-- IF PERCENT OF VALID ESTIMATES TODAY VS COUNT OF VALID And FAILED Is OVER 25% THEN CHANGE COLOR 
    '-- BLUE Is GOOD RED Is BAD

    'ESTIMATES TODAY /(ESTIMATES TODAU + FAILED ESTIMATES)
    generateGauge("evalueCount", IIf(CountOfEstimates > 0, ((CountOfEstimates / (CountOfEstimates + failedEstimates)) * 100), 0))
    Return ReturnString

  End Function

  Public Function getLatestEstimatesCount() As Long

    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim _dataTable As New DataTable
    Dim _recordSet As System.Data.SqlClient.SqlDataReader : _recordSet = Nothing

    Dim nReturnCount As Long = 0

    Try

      sQuery.Append("SELECT COUNT(*) AS LATESTESTIMATES from Aircraft_FMV WITH (NOLOCK) where afmv_latest_flag='Y' AND afmv_status='Y' and afmv_value > 0 and afmv_date > GETDATE()-1")

      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "admin_center_dataLayer.vb", sQuery.ToString)

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlCommand.CommandText = sQuery.ToString
      _recordSet = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        _dataTable.Load(_recordSet)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = _dataTable.GetErrors()
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in getLatestEstimatesCount load datatable " + constrExc.Message
      End Try

      _recordSet.Close()
      _recordSet = Nothing

      If _dataTable.Rows.Count > 0 Then

        For Each r As DataRow In _dataTable.Rows

          If Not IsDBNull(r.Item("LATESTESTIMATES")) Then
            If Not String.IsNullOrEmpty(r.Item("LATESTESTIMATES").ToString.Trim) Then
              If IsNumeric(r.Item("LATESTESTIMATES").ToString.Trim) Then
                nReturnCount = CLng(r.Item("LATESTESTIMATES").ToString)
              End If
            End If
          End If

        Next

      End If ' _dataTable.Rows.Count > 0 Then
    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in getLatestEstimatesCount() As Long " + ex.Message

    Finally

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return nReturnCount

  End Function


  Public Function getFailedEstimates() As Long

    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim _dataTable As New DataTable
    Dim _recordSet As System.Data.SqlClient.SqlDataReader : _recordSet = Nothing

    Dim nReturnCount As Long = 0

    Try

      sQuery.Append("select COUNT(*) as FAILEDESTIMATES from Aircraft_FMV WITH (NOLOCK) where afmv_value = 0 and afmv_date > GETDATE()-1")

      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlCommand.CommandText = sQuery.ToString
      _recordSet = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        _dataTable.Load(_recordSet)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = _dataTable.GetErrors()
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in " & Me.GetType().FullName & " load datatable " + constrExc.Message
      End Try

      _recordSet.Close()
      _recordSet = Nothing

      If _dataTable.Rows.Count > 0 Then

        For Each r As DataRow In _dataTable.Rows

          If Not IsDBNull(r.Item("FAILEDESTIMATES")) Then
            If Not String.IsNullOrEmpty(r.Item("FAILEDESTIMATES").ToString.Trim) Then
              If IsNumeric(r.Item("FAILEDESTIMATES").ToString.Trim) Then
                nReturnCount = CLng(r.Item("FAILEDESTIMATES").ToString)
              End If
            End If
          End If

        Next

      End If ' _dataTable.Rows.Count > 0 Then
    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in " & Me.GetType().FullName & " As Long " + ex.Message

    Finally

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return nReturnCount

  End Function
  Public Function getMyProspects(ByVal type_of As String, ByVal user_or_all As String, ByVal sum_by As String, ByVal type_of_selected As String, Optional ByVal only_active As String = "") As DataTable

    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim _dataTable As New DataTable
    Dim _recordSet As System.Data.SqlClient.SqlDataReader : _recordSet = Nothing


    Try

      'sQuery.Append("select journ_date as DATE, service_type as SERVICE, (replace(notetype,'Execution:','') + ':' + journ_description) as DETAILS, journ_user_id as USERS, comp_name as COMPANY, journ_comp_id  as COMPANY_ID, custvalue as VALUE, notesummary as ACTION, ID, SOURCE, CUSTOMERGROUP as 'ENTERPRISE GROUP' from [Homebase].jetnet_ra.dbo.View_Customer_Notes with (NOLOCK) where notegroup='Execution' and journ_date >= GETDATE() - 365 order by journ_Date desc")

      sQuery.Append("Select ")
      If Trim(type_of) = "GraphType" Then

        sQuery.Append(" case when cprospect_type = 'Contract' then 'Contracts Executed ' + cast(year(getdate()) as varchar(4)) else cprospect_type end  as TYPE, case when cprostype_sort = 6 then 100 else cprostype_sort end as cprostype_sort, ")


        If Trim(type_of_selected) <> "" Then
          sQuery.Append(" cprospect_assigned_to,  ")
        End If
        If Trim(sum_by) = "price" Then
          sQuery.Append(" sum((cprospect_value * ((case when cprospect_percent_win > 0 then cprospect_percent_win else 1 end) / 100))) as 'VALUE'  ")
        Else
          sQuery.Append(" count(distinct cprospect_id) as  'VALUE'  ")
        End If
      ElseIf Trim(type_of) = "GraphService" Then
        sQuery.Append(" cprospect_service as SERVICE, ")
        If Trim(sum_by) = "price" Then
          sQuery.Append(" sum((cprospect_value * ((case when cprospect_percent_win > 0 then cprospect_percent_win else 1 end) / 100))) as 'VALUE'  ")
        Else
          sQuery.Append(" count(distinct cprospect_id) as 'VALUE'  ")
        End If
      Else
        sQuery.Append(" comp_id, cprospect_id,")
        sQuery.Append(" comp_name as COMPANY, ")
        sQuery.Append(" cprospect_service as SERVICE, ")
        sQuery.Append(" cprospect_type as TYPE, ")
        sQuery.Append(" cprospect_details as DETAILS, ")
        sQuery.Append(" cprospect_target_date as TARGET, ")
        sQuery.Append(" Left(cprospect_next_action_date, 12) As NEXT, substring(cprospect_next_action_date,13,200) As cprospect_next_action, ")
        sQuery.Append(" cbus_name as BTYPE, ")
        sQuery.Append(" cprospect_value as VALUE, ")
        sQuery.Append(" cprospect_percent_win as 'PERCENT',  ")
        sQuery.Append(" cprospect_start_date as 'START'  ")
        sQuery.Append("  , JETNET, LEFT(LASTNOTE,12) as LASTNOTE,  substring(LASTNOTE,13,200) as LASTNOTE_TEXT   ")
      End If


      'sQuery.Append(" comp_id, comp_name, comp_address1, comp_city, comp_state, comp_country, comp_zip_code, cbus_name, ")
      'sQuery.Append(" cprospect_service, cprospect_type, cprospect_details, cprospect_id,  ")
      'sQuery.Append(" cprospect_assigned_to, cprospect_target_date, cprospect_next_action_date, ")
      'sQuery.Append(" cprospect_value, cprospect_percent_win, cprospect_user_id, cprospect_status, cprospect_contact_id ")

      If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE = True Then
        sQuery.Append(" From View_Company_Prospects with (NOLOCK) ")
      ElseIf HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER = True Then
        sQuery.Append(" From [Homebase].jetnet_ra.dbo.View_Company_Prospects with (NOLOCK) ")
      Else
        sQuery.Append(" From View_Company_Prospects with (NOLOCK) ")
      End If

      ' sQuery.Append(" inner Join company on comp_id = cprospect_comp_id And comp_journ_id = 0 ")
      '  sQuery.Append(" Left outer join Company_Business_Type with (NOLOCK) on cbus_type= comp_business_type ")

      If Trim(type_of) = "GraphType" Then


        If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE = True Then
          sQuery.Append(" inner Join Company_Prospect_Type with (NOLOCK) on cprospect_type = cprostype_name ")
        ElseIf HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER = True Then
          sQuery.Append(" inner Join [Homebase].jetnet_ra.dbo.Company_Prospect_Type with (NOLOCK) on cprospect_type = cprostype_name ")
        Else
          sQuery.Append(" inner Join Company_Prospect_Type with (NOLOCK) on cprospect_type = cprostype_name ")
        End If

      End If



      ' sQuery.Append(" where cprospect_status = 'Active' ")
      '
      '   sQuery.Append(" AND cprospect_type <> 'Do Not Market' ")

      If Trim(only_active) = "A" Then
        sQuery.Append("  where (cprospect_status = 'Active' AND cprospect_type <> 'Do Not Market' )  ")
      Else
        sQuery.Append("  where ((cprospect_status = 'Active' AND cprospect_type <> 'Do Not Market' )  ")
        sQuery.Append("  Or (cprospect_status = 'Closed' AND cprospect_type = 'Contract' and year(cprospect_target_date)=year(getdate()) )) ")
      End If


      If Trim(type_of_selected) <> "" Then
        sQuery.Append(" AND cprospect_type = '" & type_of_selected & "' ")
      End If

      If Trim(user_or_all) = "All" Then
      Else
        If Not IsNothing(HttpContext.Current.Session.Item("homebaseUserClass").home_user_id) Then
          If Trim(HttpContext.Current.Session.Item("homebaseUserClass").home_user_id) <> "" Then
            sQuery.Append("  AND cprospect_user_id = '" & HttpContext.Current.Session.Item("homebaseUserClass").home_user_id & "' ")
          End If
        End If
      End If

      If Trim(type_of) = "GraphType" Then

        sQuery.Append(" group by cprospect_type, cprostype_sort ")
        If Trim(type_of_selected) <> "" Then
          sQuery.Append(" , cprospect_assigned_to ")
        End If

        sQuery.Append(" order by cprostype_sort ")

      ElseIf Trim(type_of) = "GraphService" Then
        sQuery.Append(" group by cprospect_service ")
        sQuery.Append(" order by cprospect_service ")
      End If

      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

      '   If (HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER And HttpContext.Current.Application.Item("crmClientSiteData").WebSiteType <> eWebSiteTypes.LOCAL) Then
      '  SqlConn.ConnectionString = "Data Source=10.10.254.54;Initial Catalog=jetnet_ra;Persist Security Info=True;User ID=SVC-EVO-RO;Password=R-PL35$31;MultipleActiveResultSets=True;Asynchronous Processing=True"
      '   Else
      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      '   End If

      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlCommand.CommandText = sQuery.ToString
      _recordSet = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        _dataTable.Load(_recordSet)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = _dataTable.GetErrors()
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in " & Me.GetType().FullName & " load datatable " + constrExc.Message
      End Try

      _recordSet.Close()
      _recordSet = Nothing

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in " & Me.GetType().FullName & " As Long " + ex.Message

    Finally

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return _dataTable

  End Function

  Public Function get_My_Demos_Trials(ByVal type_of As String, ByVal user_or_all As String, ByVal sum_by As String) As DataTable

    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim _dataTable As New DataTable
    Dim _recordSet As System.Data.SqlClient.SqlDataReader : _recordSet = Nothing


    Try

      sQuery.Append("  Select Case when comp_name Is NULL then 'PLATFORM: ' + subins_platform_name else '<a href=''DisplayCompanyDetail.aspx?compid=' + cast(comp_id as varchar(10)) + ''' target=''_blank''>' + comp_name + '</a> (' + contact_first_name + ' ' + contact_last_name + ')' END as ASSIGNEDTO, ")
      sQuery.Append(" sub_service_name as SERVICE, sublogin_password As PASSWORD,  ")
      sQuery.Append("  subins_install_date as INSTALLED, subins_last_login_date As LASTLOGIN, EXPIREON, ")
      sQuery.Append(" STATUS, comptrial_user_id USERID, comp_id, contact_id, sub_id, sublogin_login, subins_seq_no ")


      If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE = True Then
        sQuery.Append(" From View_Company_Trials  with (NOLOCK) ")
      ElseIf HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER = True Then
        sQuery.Append(" From [Homebase].jetnet_ra.dbo.View_Company_Trials  with (NOLOCK) ")
      Else
        sQuery.Append(" From View_Company_Trials  with (NOLOCK) ")
      End If

      sQuery.Append(" where STATUS in ('Active','Expired') ")

      If Trim(user_or_all) = "All" Then
      Else
        If Not IsNothing(HttpContext.Current.Session.Item("homebaseUserClass").home_user_id) Then
          If Trim(HttpContext.Current.Session.Item("homebaseUserClass").home_user_id) <> "" Then
            sQuery.Append("  AND comptrial_user_id = '" & HttpContext.Current.Session.Item("homebaseUserClass").home_user_id & "' ")
          End If
        End If
      End If

      ' sQuery.Append("  AND comptrial_user_id = 'dj' ")
      sQuery.Append(" order by STATUS, comp_name, subins_platform_name ")


      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim


      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlCommand.CommandText = sQuery.ToString
      _recordSet = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        _dataTable.Load(_recordSet)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = _dataTable.GetErrors()
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in " & Me.GetType().FullName & " load datatable " + constrExc.Message
      End Try

      _recordSet.Close()
      _recordSet = Nothing

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in " & Me.GetType().FullName & " As Long " + ex.Message

    Finally

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return _dataTable

  End Function

  Private Function Create_Evo_Action_Items(ByRef ActionItemsLabel As String) As DataTable

    Create_Evo_Action_Items = Nothing
    Dim aclsData_Temp As New clsData_Manager_SQL

    aclsData_Temp.JETNET_DB = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString

    Dim NumberOfActionDays As Integer = 0

    'Select Case action_time.SelectedValue
    '    Case "5"
    '        NumberOfActionDays = 5
    '    Case "14"
    NumberOfActionDays = 14
    '    Case "30"
    '        NumberOfActionDays = 30
    '    Case Else
    '        NumberOfActionDays = 7
    'End Select 

    'If Session.Item("localSubscription").crmServerSideNotes_Flag = True Then

    '    Dim ExistsTable As New DataTable
    '    ExistsTable = aclsData_Temp.Get_Client_User_By_Email_Address(Session.Item("localUser").crmLocalUserEmailAddress)

    '    If Not IsNothing(ExistsTable) Then
    '        If ExistsTable.Rows.Count = 0 Then 'This means that the user needs to be inserted.
    '            'Please insert the user here.
    '            Session.Item("localUser").crmLocalUserID = aclsData_Temp.Insert_Client_User_Return(Session.Item("localUser").crmLocalUserFirstName, Session.Item("localUser").crmLocalUserLastName, Session.Item("localUser").crmLocalUserName, "", "N", Session.Item("localUser").crmLocalUserEmailAddress, Now(), 0, 0, New Nullable(Of System.DateTime))
    '        ElseIf ExistsTable.Rows.Count > 0 Then
    '            Session.Item("localUser").crmLocalUserID = ExistsTable.Rows(0).Item("cliuser_id")
    '        End If
    '        Create_Evo_Action_Items = aclsData_Temp.Get_Local_Notes_GetByUserIDStatusLessThanDate(Session.Item("localUser").crmLocalUserID, FormatDateTime(DateAdd(DateInterval.Day, NumberOfActionDays, Now()), DateFormat.ShortDate), "P")

    '        ' ActionItemsLabel = DisplayFunctions.Display_Notes_Or_Actions_HB_Admin(Create_Evo_Action_Items, aclsData_Temp, True, True, True, False, True, False, True)
    '    End If
    'ElseIf Session.Item("localSubscription").crmCloudNotes_Flag = True Then

    '    Create_Evo_Action_Items = aclsData_Temp.Get_CloudNotes_GetByUserIDStatusLessThanDate(Session.Item("localUser").crmSubSubID, Session.Item("localUser").crmUserLogin, FormatDateTime(DateAdd(DateInterval.Day, NumberOfActionDays, Now()), DateFormat.ShortDate), "P")
    '    ' ActionItemsLabel = DisplayFunctions.Display_Notes_Or_Actions_HB_Admin(Create_Evo_Action_Items, aclsData_Temp, True, True, True, True, True, False, True)
    'End If



    Create_Evo_Action_Items = getJournalDataTable(FormatDateTime(DateAdd(DateInterval.Day, NumberOfActionDays, Now()), DateFormat.ShortDate), "P")
    Call display_journal_table(ActionItemsLabel, Create_Evo_Action_Items)

    moduleLiteral.Text += ActionItemsLabel
  End Function
  Public Function getJournalDataTable(ByVal lnote_schedule_start_date As String, ByVal lnote_status As String) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sSeperator = ""

    Try

      sQuery.Append("SELECT * FROM " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Journal WITH(NOLOCK) ")
      sQuery.Append("LEFT OUTER JOIN " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Contact WITH(NOLOCK) ON contact_id = journ_contact_id And contact_journ_id = 0 And contact_active_flag = 'Y' ")    'AND contact_hide_flag = 'N' 
      sQuery.Append("LEFT OUTER JOIN " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "Company WITH(NOLOCK) ON comp_id = journ_comp_id AND comp_journ_id = 0 AND comp_active_flag = 'Y' AND comp_hide_flag = 'N' ")
      sQuery.Append("LEFT OUTER JOIN " + IIf(HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER, "[Homebase].jetnet_ra.dbo.", "") + "[user] WITH(NOLOCK) ON [user_id] = journ_user_id AND user_email_address <> '' AND user_password <> 'inactive' ")
      sQuery.Append("WHERE  journ_subcategory_code IN ('AIAI') ")

      ' If Trim(user_or_all) = "All" Then
      ' Else
      If Not IsNothing(HttpContext.Current.Session.Item("homebaseUserClass").home_user_id) Then
        If Trim(HttpContext.Current.Session.Item("homebaseUserClass").home_user_id) <> "" Then
          sQuery.Append("  AND journ_user_id = '" & HttpContext.Current.Session.Item("homebaseUserClass").home_user_id & "' ")
        End If
      End If
      ' End If

      'If lnote_user_login <> 0 Then
      '    sQuery.Append(" and (journ_user_id = " & lnote_user_login & ")  ")
      'End If

      'sQuery.Append(" AND (journ_date >= '" & lnote_schedule_start_date & "') ")
      sQuery.Append(" And journ_newac_flag = 'N' AND journ_internal_trans_flag = 'N' ")

      sQuery.Append(" ORDER BY journ_date ASC, journ_id ASC")

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 90

      SqlCommand.CommandText = sQuery.ToString
      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()

        Return Nothing

      End Try

    Catch ex As Exception

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

  Public Sub display_journal_table(ByRef out_htmlString As String, ByVal resultsTable As DataTable)

    Dim htmlOut As New StringBuilder
    Dim toggleRowColor As Boolean = False

    Try

      out_htmlString = ""

      If Not IsNothing(resultsTable) Then

        If resultsTable.Rows.Count > 0 Then

          htmlOut.Append("<table id='table_14' class=""formatTable blue datagrid small""  style=""width:100%"">")
          '  htmlOut.Append("<table id=""tabPanel0_DataTable"" cellpadding=""0"" cellspacing=""0"" border=""0"" width=""100%"">")
          htmlOut.Append("<thead><tr>")
          ' htmlOut.Append("<th><span class=""help_cursor"" title=""Used to select and remove item from the list"">SEL</span></th>")


          'htmlOut.Append("<th></th>")
          htmlOut.Append("<th>EDIT</th>")
          htmlOut.Append("<th data-priority=""1"">ACTION<br/>DATE</th>")
          htmlOut.Append("<th>COMPANY</th>")
          htmlOut.Append("<th>CONTACT</th>")
          htmlOut.Append("<th>DETAILS</th>")
          htmlOut.Append("<th>ASSIGNED</th>")

          htmlOut.Append("</tr></thead><tbody>")

          Dim sSeparator As String = ""
          Dim dateSort As String = ""

          For Each r As DataRow In resultsTable.Rows

            dateSort = ""

            htmlOut.Append("<tr>")

            htmlOut.Append("<td align=""left"" valign=""middle"">")

            htmlOut.Append("<a href=""javascript:void(0);"" onclick=""load('adminActions.aspx?task=edit&journalid=" + r.Item("journ_id").ToString.Trim + "&companyid=" & r.Item("journ_comp_id").ToString & "&contactid=" & r.Item("journ_contact_id").ToString & "','','scrollbars=yes,menubar=no,height=900,width=1350,resizable=yes,toolbar=no,location=no,status=no');return false;"" title=""Edit Action Item""><img src =""images/edit_icon.png"" alt=""Edit Action Item"" title=""Edit Action Item""></a>")

            htmlOut.Append("</td>")



            If Not IsDBNull(r.Item("journ_date")) Then
              If Not String.IsNullOrEmpty(r.Item("journ_date").ToString.Trim) Then
                dateSort = Format(r("journ_date"), "yyyy/MM/dd")
                htmlOut.Append("<td align=""left"" valign=""top"" nowrap=""nowrap""  data-sort='" & dateSort & "'>")


                htmlOut.Append(r.Item("journ_date").ToString.Trim)
              Else
                htmlOut.Append("<td align=""left"" valign=""top"" nowrap=""nowrap"">")
              End If
            Else
              htmlOut.Append("<td align=""left"" valign=""top"" nowrap=""nowrap"">")
            End If

            htmlOut.Append("</td>")

            htmlOut.Append("<td align=""left"" valign=""top"" style=""width: 145px;"">")

            If Not IsDBNull(r.Item("journ_comp_id")) Then
              If Not String.IsNullOrEmpty(r.Item("journ_comp_id").ToString.Trim) Then

                If IsNumeric(r.Item("journ_comp_id").ToString) Then
                  If CLng(r.Item("journ_comp_id").ToString) > 0 Then

                    'Request by Derek to be opened in a new tab 
                    ' htmlOut.Append("<a class=""underline distinct"" onclick='javascript:openSmallWindowJS(""DisplayCompanyDetail.aspx?compid=" + r.Item("journ_comp_id").ToString + "&journid=0"",""CompanyDetails"");' title=""Display Company Details"">")
                    htmlOut.Append("<a class=""underline distinct"" href='DisplayCompanyDetail.aspx?compid=" + r.Item("journ_comp_id").ToString + "&journid=0' target='_blank'>")

                    htmlOut.Append(Replace(r.Item("comp_name").ToString.Trim, Constants.cSingleSpace, Constants.cHTMLnbsp))
                    htmlOut.Append("</a><br />")

                    Dim Seperator As String = ""
                    If Not (IsDBNull(r("comp_address1"))) And Not String.IsNullOrEmpty(r.Item("comp_address1").ToString.Trim) Then
                      htmlOut.Append(r.Item("comp_address1").ToString.Trim)
                      Seperator = "<br />"
                    End If

                    If Not (IsDBNull(r("comp_address2"))) And Not String.IsNullOrEmpty(r.Item("comp_address2").ToString.Trim) Then
                      htmlOut.Append(" " + r.Item("comp_address2").ToString.Trim)
                      Seperator = "<br />"
                    End If

                    htmlOut.Append(Seperator)
                    Seperator = ""

                    If Not (IsDBNull(r("comp_city"))) And Not String.IsNullOrEmpty(r.Item("comp_city").ToString.Trim) Then
                      htmlOut.Append(r.Item("comp_city").ToString.Trim + ", ")
                    End If

                    If Not (IsDBNull(r("comp_state"))) And Not String.IsNullOrEmpty(r.Item("comp_state").ToString.Trim) Then
                      htmlOut.Append(r.Item("comp_state").ToString.Trim + " ")
                    End If

                    If Not (IsDBNull(r("comp_zip_code"))) And Not String.IsNullOrEmpty(r.Item("comp_zip_code").ToString.Trim) Then
                      htmlOut.Append(r.Item("comp_zip_code").ToString.Trim + " ")
                    End If

                    If Not (IsDBNull(r("comp_country"))) And Not String.IsNullOrEmpty(r.Item("comp_country").ToString.Trim) Then
                      htmlOut.Append(r.Item("comp_country").ToString.Trim)
                    End If

                  End If
                End If

              End If
            End If

            htmlOut.Append("</td>")

            htmlOut.Append("<td align=""left"" valign=""top"">")

            If Not IsDBNull(r.Item("journ_contact_id")) Then
              If Not String.IsNullOrEmpty(r.Item("journ_contact_id").ToString.Trim) Then

                If IsNumeric(r.Item("journ_contact_id").ToString) Then
                  If CLng(r.Item("journ_contact_id").ToString) > 0 Then

                    'requested to be changed - Derek - MSW - 6/3/2020 
                    'htmlOut.Append("<a class=""underline distinct"" onclick='javascript:openSmallWindowJS(""DisplayContactDetail.aspx?compid=" + r.Item("journ_comp_id").ToString + "&jid=0&conid=" + r.Item("journ_contact_id").ToString + """,""ContactDetails"");' title=""Display Contact Details"">")
                    htmlOut.Append("<a class=""underline distinct"" href='DisplayContactDetail.aspx?compid=" + r.Item("journ_comp_id").ToString + "&jid=0&conid=" + r.Item("journ_contact_id").ToString + "' target='_blank' title=""Display Contact Details"">")

                    htmlOut.Append(r.Item("contact_sirname").ToString.Trim + Constants.cSingleSpace + r.Item("contact_first_name").ToString.Trim + Constants.cSingleSpace)

                    If Not String.IsNullOrEmpty(r.Item("contact_middle_initial").ToString) Then
                      htmlOut.Append(r.Item("contact_middle_initial").ToString.Trim + ". ")
                    End If

                    htmlOut.Append(r.Item("contact_last_name").ToString.Trim)

                    If Not String.IsNullOrEmpty(r.Item("contact_suffix").ToString) Then
                      htmlOut.Append(Constants.cSingleSpace + r.Item("contact_suffix").ToString.Trim)
                    End If

                    htmlOut.Append("</a>")

                    If Not (IsDBNull(r("contact_title"))) And Not String.IsNullOrEmpty(r.Item("contact_title").ToString.Trim) Then
                      htmlOut.Append("<br />" + r.Item("contact_title").ToString.Trim)
                    End If

                    If Not (IsDBNull(r("contact_email_address"))) And Not String.IsNullOrEmpty(r.Item("contact_email_address").ToString.Trim) Then
                      htmlOut.Append("<br /><a href=""mailto:" + r.Item("contact_email_address").ToString.Trim + """>" + r.Item("contact_email_address").ToString.Trim + "</a>")
                    End If

                  End If
                End If

              End If
            End If

            htmlOut.Append("</td>")

            htmlOut.Append("<td align=""left"" valign=""top"" style=""width: 225px;"">")

            If Not IsDBNull(r.Item("journ_description")) Then
              If Not String.IsNullOrEmpty(r.Item("journ_description").ToString.Trim) Then
                If r.Item("journ_description").ToString.Length < 500 Then
                  htmlOut.Append(r.Item("journ_description").ToString.Trim)
                Else
                  htmlOut.Append(r.Item("journ_description").ToString.Substring(0, 500).Trim + " ...")
                End If
              End If
            End If

            htmlOut.Append("</td>")

            htmlOut.Append("<td align=""left"" valign=""top"" nowrap=""nowrap"">")

            If Not IsDBNull(r.Item("journ_user_id")) Then
              If Not String.IsNullOrEmpty(r.Item("journ_user_id").ToString.Trim) Then ' user_last_name, user_first_name

                htmlOut.Append(r.Item("user_last_name").ToString.Trim + Constants.cSingleSpace + r.Item("user_first_name").ToString.Trim)
                htmlOut.Append(" (<em>" + r.Item("journ_user_id").ToString.Trim + "</em>)")

              End If
            End If

            htmlOut.Append("</td>")

            htmlOut.Append("</tr>")

          Next

        End If ' _dataTable.Rows.Count > 0 Then

        htmlOut.Append("</tbody></table>")
        'htmlOut.Append("<div id=""tabPanel0_Label"" class="""" style=""padding:2px;""><strong>" + ActionItems.resultsTable.Rows.Count.ToString + " Records</strong></div>")
        'htmlOut.Append("<div id=""tabPanel0_InnerTable"" align=""left"" valign=""middle"" style=""max-height:610px; overflow: auto;""></div>")

      End If

      out_htmlString = htmlOut.ToString

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message

    Finally

      htmlOut = Nothing

    End Try

  End Sub


  Public Function getContractActions() As DataTable

    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim _dataTable As New DataTable
    Dim _recordSet As System.Data.SqlClient.SqlDataReader : _recordSet = Nothing


    Try
      'How many days since last year start?
      Dim firstDayLastYear = New DateTime(Year(Now()) - 1, 1, 1)
      Dim timeSinceFirstDayLastYear = Now.Subtract(firstDayLastYear)
      Dim totalDaysSince As Integer = timeSinceFirstDayLastYear.Days

      sQuery.Append("select journ_date as DATE, service_type as SERVICE, (replace(notetype,'Execution:','') + ':' + journ_description) as DETAILS, journ_user_id as USERS, comp_name as COMPANY, journ_comp_id  as COMPANY_ID, custvalue as VALUE, notesummary as ACTION, ID, SOURCE, CUSTOMERGROUP as 'ENTERPRISE GROUP' from [Homebase].jetnet_ra.dbo.View_Customer_Notes with (NOLOCK) where notegroup='Execution' and journ_date >= GETDATE() - " & totalDaysSince.ToString & " order by journ_Date desc")

      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

      '   If (HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER And HttpContext.Current.Application.Item("crmClientSiteData").WebSiteType <> eWebSiteTypes.LOCAL) Then
      '  SqlConn.ConnectionString = "Data Source=10.10.254.54;Initial Catalog=jetnet_ra;Persist Security Info=True;User ID=SVC-EVO-RO;Password=R-PL35$31;MultipleActiveResultSets=True;Asynchronous Processing=True"
      '   Else
      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      '   End If

      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlCommand.CommandText = sQuery.ToString
      _recordSet = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        _dataTable.Load(_recordSet)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = _dataTable.GetErrors()
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in " & Me.GetType().FullName & " load datatable " + constrExc.Message
      End Try

      _recordSet.Close()
      _recordSet = Nothing

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in " & Me.GetType().FullName & " As Long " + ex.Message

    Finally

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return _dataTable

  End Function
  Public Function getUpcomingContractActions() As DataTable

    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim _dataTable As New DataTable
    Dim _recordSet As System.Data.SqlClient.SqlDataReader : _recordSet = Nothing


    Try

      sQuery.Append("select convert(date, journ_date) as ENDDATE, journ_comp_id as COMPANY_ID, comp_name as COMPANY, notetype as ACTION, journ_description as DETAILS,  service_type as SERVICE ")

      sQuery.Append(",  (select top (1) CAST(n.journ_date AS varchar(12)) + ' (' + n.journ_user_id + ') ' + n.journ_description AS Expr1 ")
      sQuery.Append("  From [Homebase].jetnet_ra.dbo.View_Customer_Notes n with (NOLOCK) ")
      sQuery.Append("  Where n.notegroup In ('Marketing','Activity') AND (View_Customer_Notes.journ_comp_id = n.journ_comp_id) ")
      sQuery.Append("  order by n.journ_date desc) AS LASTNOTE  ")

      sQuery.Append(" From [Homebase].jetnet_ra.dbo.View_Customer_Notes with (NOLOCK) ")



      sQuery.Append(" Where notegroup = 'Accounting' order by journ_date ")




      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

      ' If (HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER And HttpContext.Current.Application.Item("crmClientSiteData").WebSiteType <> eWebSiteTypes.LOCAL) Then
      ' SqlConn.ConnectionString = "Data Source=10.10.254.54;Initial Catalog=jetnet_ra;Persist Security Info=True;User ID=SVC-EVO-RO;Password=R-PL35$31;MultipleActiveResultSets=True;Asynchronous Processing=True"
      ' Else
      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      '  End If

      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlCommand.CommandText = sQuery.ToString
      _recordSet = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        _dataTable.Load(_recordSet)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = _dataTable.GetErrors()
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in " & Me.GetType().FullName & " load datatable " + constrExc.Message
      End Try

      _recordSet.Close()
      _recordSet = Nothing

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in " & Me.GetType().FullName & " As Long " + ex.Message

    Finally

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return _dataTable

  End Function

  Public Function getRecentErrors() As DataTable

    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim _dataTable As New DataTable
    Dim _recordSet As System.Data.SqlClient.SqlDataReader : _recordSet = Nothing


    Try

      sQuery.Append("select top 1000 subislog_date as 'DATE', subislog_email_address as 'EMAIL', subislog_msg_type as 'TYPE', subislog_host_name as 'HOST', subislog_app_name as 'APP', subislog_message as 'MESSAGE', ")
      sQuery.Append("    Case when subislog_message <> 'User Failed Login' then comp_name when (subislog_message = 'User Failed Login' and (select top 1 View_JETNET_Customers.comp_name from View_JETNET_Customers where contact_email_address=subislog_email_address) IS NULL) then '** Not Customer'  else (select top 1 View_JETNET_Customers.comp_name from View_JETNET_Customers where contact_email_address=subislog_email_address) end as COMPANY ")

      '  comp_name as 'COMPANY'
      sQuery.Append(" , comp_id as 'COMPANY_ID', case when iploc_country is NULL then subislog_tcpip else (subislog_tcpip + ' (' + iploc_country + ' ' + iploc_region + ' ' + iploc_city + ')') end as 'IP', sub_service_name as 'SERVICE', sub_id")
      sQuery.Append(" from Subscription_Install_Log with (NOLOCK) ")
      sQuery.Append(" left outer join Subscription with (NOLOCK) on subislog_subid = sub_id ")
      sQuery.Append(" left outer join Company with (NOLOCK) on sub_comp_id = comp_id and comp_journ_id = 0 ")
      sQuery.Append(" Left outer join IP_location with (NOLOCK) on iploc_ip = subislog_tcpip ")
      sQuery.Append(" where subislog_date >= GETDATE() - 1 ")
      sQuery.Append("  and (subislog_msg_type in ('UserError', 'UserAbuse','UserLogoutForced') ")
      sQuery.Append(" or (subislog_msg_type = 'UserPreferences' and subislog_message like '%Password%')) ")
      sQuery.Append(" order by subislog_date desc ")


      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)


      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlCommand.CommandText = sQuery.ToString
      _recordSet = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        _dataTable.Load(_recordSet)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = _dataTable.GetErrors()
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in " & Me.GetType().FullName & " load datatable " + constrExc.Message
      End Try

      _recordSet.Close()
      _recordSet = Nothing

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in " & Me.GetType().FullName & " As Long " + ex.Message

    Finally

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return _dataTable

  End Function
  Public Shared Function ConvertModule23DataTable(ByVal dt As DataTable, tableID As Integer) As String

    Dim html As New StringBuilder


    Try
      html.Append("<table id='table_" & tableID.ToString & "' class=""formatTable blue datagrid small""  style=""width:100%"">")
      html.Append("<thead>")
      html.Append("<tr>")
      html.Append("<th width=""90"">DATE</th>")
      html.Append("<th width=""120"">EMAIL</th>")
      html.Append("<th width=""50"">TYPE</th>")
      html.Append("<th width=""300"">MESSAGE</th>")
      html.Append("<th width=""150"">COMPANY</th>")
      html.Append("<th width=""70"">IP</th>")
      html.Append("<th width=""150"">SERVICE</th>")
      html.Append("<th width=""40"">SUB ID</th>")
      html.Append("</tr>")
      html.Append("</thead>")
      html.Append("<tbody>")
      For Each r As DataRow In dt.Rows

        html.Append("<tr>")

        If Not IsDBNull(r("DATE")) Then
          html.Append("<td data-sort=""" & Format(r("DATE"), "yyyy-MM-dd HH:mm:ss") & """><span title=""")
          If Not IsDBNull(r("APP")) Then
            html.Append(r("APP").ToString)
          End If
          If Not IsDBNull(r("HOST")) Then
            If Not IsDBNull(r("APP")) Then
              html.Append(" - ")
            End If
            html.Append(r("HOST").ToString & ".")
          End If

          html.Append(""" class=""help_cursor""")
          html.Append("/>" & FormatDateTime(r("DATE"), DateFormat.GeneralDate))
          html.Append("</span></td>")
        Else
          html.Append("<td></td>")
        End If

        html.Append("<td align=""left"">")
        If Not IsDBNull(r("EMAIL")) Then
          html.Append("<a href=""javascript:void(0);"" onclick=""javascript:load('/adminSubErrors.aspx?email=" & HttpContext.Current.Server.UrlEncode(r("EMAIL")).ToString & "','','scrollbars=yes,menubar=no,height=700,width=1060,resizable=yes,toolbar=no,location=no,status=no');"" class=""text_underline help_cursor"" title=""Users Error Log"">" & r("EMAIL") & "</a>")
        End If

        html.Append("</td>")

        html.Append("<td align=""left"">")
        If Not IsDBNull(r("TYPE")) Then
          html.Append(r("TYPE"))
        End If

        html.Append("</td>")
        html.Append("<td align=""left"">")
        If Not IsDBNull(r("MESSAGE")) Then
          If r("MESSAGE").length > 255 Then
            html.Append("<span title=""" & HttpContext.Current.Server.HtmlEncode(r("MESSAGE")) & """ class=""help_cursor"" > " & Left(r("MESSAGE"), 255) & "..." & "</span>")
          Else
            html.Append("<span title=""" & HttpContext.Current.Server.HtmlEncode(r("MESSAGE")) & """  class=""help_cursor""> " & r("MESSAGE") & "</span>")
          End If

        End If

        html.Append("</td>")

        If Not IsDBNull(r("COMPANY")) Then
          If Not IsDBNull(r("COMPANY_ID")) Then
            html.Append("<td>" & DisplayFunctions.WriteDetailsLink(0, r("COMPANY_ID"), 0, 0, True, r("COMPANY"), "text_underline", "") & "</td>")
          Else
            html.Append("<td>" & r("COMPANY") & "</td>")
          End If
        Else
          html.Append("<td align=""left""></td>")
        End If

        html.Append("<td align=""left"">")
        If Not IsDBNull(r("IP")) Then
          html.Append(r("IP"))
        End If

        html.Append("</td>")

        html.Append("<td align=""left"">")
        If Not IsDBNull(r("SERVICE")) Then
          html.Append(r("SERVICE"))
        End If

        html.Append("</td>")
        html.Append("<td align=""left"">")
        If Not IsDBNull(r("SUB_ID")) Then
          html.Append("<a href=""javascript: Void(0);""  title=""Edit Subscription"" onclick=""javascript:load('/homebaseSubscription.aspx?compID=" & r("COMPANY_ID") & "&subID=" & r("SUB_ID") & "','','scrollbars=yes,menubar=no,height=700,width=1160,resizable=yes,toolbar=no,location=no,status=no');"" class=""text_underline help_cursor"">" & r("SUB_ID") & "</a>")
        End If

        html.Append("</td>")



        html.Append("</tr>")
      Next
      html.Append("</tbody>")

      html.Append("</table>")
    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in ConvertDataTabletoHTML " + ex.Message
    End Try
    Return html.ToString
  End Function

  Public Function getCustomerNetValue(ByVal module_number As Integer) As DataTable

    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim _dataTable As New DataTable
    Dim _recordSet As System.Data.SqlClient.SqlDataReader : _recordSet = Nothing


    Try

      sQuery.Append(" select distinct YEAR(A.journ_date) as cstat_year, MONTH(A.journ_date) as cstat_month, ")
      sQuery.Append(" (select SUM(C.custvalue) from [Homebase].jetnet_ra.dbo.View_Customer_Notes C with (NOLOCK)  ")

      If module_number = 24 Then
        sQuery.Append(" where YEAR(C.journ_date) = Year(A.journ_date) And Month(C.journ_Date) = Month(A.journ_date) ")
      Else
        sQuery.Append(" where YEAR(C.journ_date) = Year(A.journ_date) And Month(C.journ_Date) <= Month(A.journ_date) ")
      End If




      sQuery.Append(" And c.notegroup = 'Execution'  and C.service_type in ('Marketplace','Aerodex')) as 'TOTAL', ")
      sQuery.Append("(select SUM(C.custvalue) from [Homebase].jetnet_ra.dbo.View_Customer_Notes C With (NOLOCK) ")

      If module_number = 24 Then
        sQuery.Append(" where YEAR(C.journ_date) = YEAR(A.journ_date) and MONTH(C.journ_Date) = MONTH(A.journ_date) ")
      Else
        sQuery.Append(" where YEAR(C.journ_date) = YEAR(A.journ_date) and MONTH(C.journ_Date) <= MONTH(A.journ_date) ")
      End If


      sQuery.Append(" and c.notegroup = 'Execution' and C.service_type in ('Marketplace')) as 'MARKETPLACE', ")
      sQuery.Append(" (select  case when SUM(C.custvalue) is null then 0 else SUM(C.custvalue) end from [Homebase].jetnet_ra.dbo.View_Customer_Notes C with (NOLOCK)  ")

      If module_number = 24 Then
        sQuery.Append(" where YEAR(C.journ_date) = Year(A.journ_date) And Month(C.journ_Date) = Month(A.journ_date) ")
      Else
        sQuery.Append(" where YEAR(C.journ_date) = Year(A.journ_date) And Month(C.journ_Date) <= Month(A.journ_date) ")
      End If

      sQuery.Append(" and c.notegroup = 'Execution' and C.service_type in ('Aerodex')) as 'AERODEX' ")
      sQuery.Append(" from [Homebase].jetnet_ra.dbo.View_Customer_Notes A with (NOLOCK) ")

      sQuery.Append(" where A.notegroup = 'Execution' ")

      If module_number = 24 Then
        sQuery.Append(" and YEAR(a.journ_date) >= 2019 ")
      ElseIf module_number = 10 Then
        sQuery.Append(" and YEAR(a.journ_date)=2019 ")
      ElseIf module_number = 19 Then
        sQuery.Append(" and YEAR(a.journ_date)=2020 ")
      End If

      sQuery.Append(" And  a.journ_date <= getdate() ") ' adding really just for current year, but should be all 


      sQuery.Append(" and A.service_type in ('Marketplace','Aerodex') ")
      sQuery.Append(" group by YEAR(A.journ_date), MONTH(A.journ_date) ")
      sQuery.Append(" order by YEAR(A.journ_date), MONTH(A.journ_date) ")



      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

      '  If (HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER And HttpContext.Current.Application.Item("crmClientSiteData").WebSiteType <> eWebSiteTypes.LOCAL) Then
      '      SqlConn.ConnectionString = "Data Source=10.10.254.54;Initial Catalog=jetnet_ra;Persist Security Info=True;User ID=SVC-EVO-RO;Password=R-PL35$31;MultipleActiveResultSets=True;Asynchronous Processing=True"
      '  Else
      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      '  End If

      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlCommand.CommandText = sQuery.ToString
      _recordSet = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        _dataTable.Load(_recordSet)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = _dataTable.GetErrors()
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in " & Me.GetType().FullName & " load datatable " + constrExc.Message
      End Try

      _recordSet.Close()
      _recordSet = Nothing

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in " & Me.GetType().FullName & " As Long " + ex.Message

    Finally

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return _dataTable

  End Function


  Public Function generateGauge(ByVal canvasName As String, ByVal value As Double) As StringBuilder
    Dim htmlOut As New StringBuilder
    Dim jsScr As New StringBuilder

    jsScr.Append("function initGauge_" & canvasName & "() { ")
    jsScr.Append(" var gauge = new RadialGauge({ renderTo: '" & canvasName & "',")
    jsScr.Append(" width: 265, height: 265, units: false,")
    jsScr.Append(" fontTitleSize: ""34"",")
    jsScr.Append(" fontTitle:""Arial"",")
    jsScr.Append(" colorTitle:  '#4f5050',")
    jsScr.Append(" title: """ & FormatNumber(value, 2).ToString & "%"", ")
    jsScr.Append(" startAngle: 90, SweepAngle: 180, valueBox: false, ticksAngle: 180, exactTicks: true, ")
    jsScr.Append(" minValue: 0, ")
    jsScr.Append(" maxValue: 100,")
    jsScr.Append(" majorTicks: false, minorTicks: 0,strokeTicks: false,")
    jsScr.Append(" colorUnits: ""#000000"",")
    jsScr.Append(" fontUnitsSize: ""30"",")
    jsScr.Append(" highlights: false,animation: false,")
    jsScr.Append(" barWidth: 25,")
    jsScr.Append(" barProgress: true,")

    If value >= 50 Then
      jsScr.Append(" colorBarProgress:  '#008000',")
    Else
      jsScr.Append(" colorBarProgress:  '#dc3912',")
    End If

    jsScr.Append(" needle: false,")
    jsScr.Append(" colorBar:  '#eee',")
    jsScr.Append(" colorStrokeTicks: '#fff',")
    jsScr.Append(" numbersMargin: -18,")
    jsScr.Append(" colorPlate: ""rgba(0,0,0,0)"",") 'Make background transparent.
    jsScr.Append(" borderShadowWidth: 0,")
    jsScr.Append(" borders: false,")
    jsScr.Append(" value: " & value.ToString & ",")
    jsScr.Append("}).draw();")


    jsScr.Append(" };initGauge_" & canvasName & "();")

    System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType, "gaugeString" & canvasName, jsScr.ToString, True)


    Return htmlOut
  End Function

  Private Function BuildPieChart(ModuleTable As DataTable, moduleNumber As Integer) As String
    Dim JavaScriptString As String = ""
    Dim InnerScript As String = ""
    JavaScriptString += " var Data" & moduleNumber.ToString & " = google.visualization.arrayToDataTable([" & vbNewLine
    JavaScriptString += "['Program', '%']," & vbNewLine
    For Each r As DataColumn In ModuleTable.Columns
      If r.ColumnName <> "TOTLICENSES" Then
        If InnerScript <> "" Then
          InnerScript += ", "
        End If
        InnerScript += "['" & r.ColumnName & "',     " & (ModuleTable.Rows(0).Item(r.ColumnName).ToString) / ModuleTable.Rows(0).Item("TOTLICENSES").ToString & "]" & vbNewLine
      End If
    Next
    JavaScriptString += InnerScript
    JavaScriptString += "]); " & vbNewLine
    JavaScriptString += "var options" & moduleNumber.ToString & " = {" & vbNewLine
    JavaScriptString += " chartArea: {left:20,top:0,width:'100%',height:'90%'}"
    JavaScriptString += "};" & vbNewLine
    JavaScriptString += " var chart" & moduleNumber.ToString & " = new google.visualization.PieChart(document.getElementById('piechart" & moduleNumber.ToString & "'));" & vbNewLine
    JavaScriptString += " chart" & moduleNumber.ToString & ".draw(Data" & moduleNumber.ToString & ", options" & moduleNumber.ToString & "); " & vbNewLine

    Return JavaScriptString
  End Function


  Public Function getModule38() As DataTable

    Dim sQuery = New StringBuilder()
    Dim atemptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    'Dim firstDayCurrentYear = New DateTime(Year(Now()), 1, 1)
    'Dim timeSinceFirstDayCurrentYear = Now.Subtract(firstDayCurrentYear)
    'Dim totalDaysSince As Integer = timeSinceFirstDayCurrentYear.Days ' + 1

    Try

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      sQuery.Append("select convert(date,subislog_date) as cstat_day, count(distinct subislog_email_Address) as cstat_value from Subscription_Install_Log with (NOLOCK) where subislog_msg_type = 'UserLogin' and subislog_date >= '1/1/2020' group by convert(date,subislog_date)  order by convert(date,subislog_date) ")

      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

      SqlCommand.CommandText = sQuery.ToString
      SqlReader = SqlCommand.ExecuteReader()

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in " & Me.GetType().FullName & "</b><br /> " + constrExc.Message
      End Try


    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in " & Me.GetType().FullName & " " + ex.Message

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


  Public Function getModule42() As DataTable

    Dim sQuery = New StringBuilder()
    Dim atemptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    'Dim firstDayCurrentYear = New DateTime(Year(Now()), 1, 1)
    'Dim timeSinceFirstDayCurrentYear = Now.Subtract(firstDayCurrentYear)
    'Dim totalDaysSince As Integer = timeSinceFirstDayCurrentYear.Days ' + 1

    Try

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60



      sQuery.Append(" Select   ")
      sQuery.Append(" Convert(Char(8), ffd_origin_date, 112) As Year_Month_Day, ")
      sQuery.Append(" COUNT(ffd_unique_flight_id) As Total_Count ")
      sQuery.Append(" From FAA_Flight_Data WITH (NOLOCK) ")
      sQuery.Append(" Where (Year(ffd_origin_date) >= 2020) ")
      sQuery.Append(" And (CAST(ffd_origin_date AS DATE) < CAST(GETDATE() AS DATE)) ")
      sQuery.Append(" And (ffd_ac_id > 0) ")
      sQuery.Append(" And (ffd_data_source = 'FAA-LIVE') ")
      sQuery.Append(" And (ffd_callsign Is Not NULL And ffd_callsign <> '')   ")
      sQuery.Append(" Group BY CONVERT(CHAR(8), ffd_origin_date, 112) ")
      sQuery.Append(" ORDER BY Year_Month_Day ")

      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

      SqlCommand.CommandText = sQuery.ToString
      SqlReader = SqlCommand.ExecuteReader()

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in " & Me.GetType().FullName & "</b><br /> " + constrExc.Message
      End Try


    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in " & Me.GetType().FullName & " " + ex.Message

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

  Public Function getModule39() As DataTable

    Dim sQuery = New StringBuilder()
    Dim atemptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    'Dim firstDayCurrentYear = New DateTime(Year(Now()), 1, 1)
    'Dim timeSinceFirstDayCurrentYear = Now.Subtract(firstDayCurrentYear)
    'Dim totalDaysSince As Integer = timeSinceFirstDayCurrentYear.Days ' + 1

    Try

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60



      sQuery.Append(" select convert(date,subislog_date) as cstat_day, count(distinct subislog_email_Address) As cstat_value from Subscription_Install_Log With (NOLOCK) ")
      sQuery.Append(" inner Join subscription with (NOLOCK) on subislog_subid = sub_id")
      sQuery.Append(" where subislog_msg_type = 'UserLogin'")
      sQuery.Append(" And subislog_date >= '1/1/2020' ")
      sQuery.Append(" And sub_Comp_id in (")
      sQuery.Append(" select distinct comp_id from Company With (NOLOCK)")
      sQuery.Append(" where comp_journ_id = 0 ")
      sQuery.Append(" And comp_country in ('Albania', 'Algeria', 'Andorra', 'Angola', 'Armenia', 'Ascension Island', 'Austria', 'Azerbaijan', 'Bahrain', 'Belarus', 'Belgium', 'Benin', 'Bosnia and Herzegovina', 'Botswana', 'Bouvet Island', 'Bulgaria', 'Burkina Faso', 'Burundi', 'Cameroon', 'Canary Islands', 'Cape Verde', 'Central African Republic', 'Chad', 'Channel Islands', 'Comoros', 'Congo', 'Cote dIvoire', 'Croatia', 'Cyprus', 'Czech Republic', 'Dem. Republic of Congo', 'Denmark', 'Djibouti', 'Egypt', 'England', 'Equatorial Guinea', 'Eritrea', 'Estonia', 'Ethiopia', 'Faroe Islands', 'Finland', 'France Metropolitan', 'France', 'French Southern Territori', 'Gabon', 'Gambia', 'Georgia', 'Germany', 'Ghana', 'Gibraltar', 'Greece', 'Guernsey', 'Guinea', 'Guinea-Bissau', 'Hungary', 'Iceland', 'Iran', 'Iraq', 'Ireland', 'Isle of Man', 'Israel', 'Italy', 'Ivory Coast', 'Jersey', 'Jordan', 'Kenya', 'Kosovo', 'Kuwait', 'Latvia', 'Lebanon', 'Lesotho', 'Liberia', 'Libya', 'Liechtenstein', 'Lithuania', 'Luanda', 'Luxembourg', 'Macedonia', 'Madagascar', 'Malawi', 'Mali Republic', 'Malta', 'Mauritania', 'Mauritius', 'Mayotte Island', 'Moldova', 'Monaco', 'Montenegro', 'Morocco', 'Mozambique', 'Namibia', 'Netherlands', 'Niger', 'Nigeria', 'Northern Ireland', 'Norway', 'Oman', 'Palestine', 'Poland', 'Portugal', 'Qatar', 'Reunion Island', 'Romania', 'Russia', 'Russian Federation', 'Rwanda', 'Saint Helena', 'San Marino', 'Sao Tome and Principe', 'Saudi Arabia', 'Scotland', 'Senegal', 'Serbia and Montenegro', 'Serbia', 'Seychelles Islands', 'Sierra Leone', 'Slovak Republic', 'Slovenia', 'Somalia', 'South Africa', 'South Sudan', 'Spain', 'Sudan', 'Svalbard and Jan Mayen Is', 'Swaziland', 'Sweden', 'Switzerland', 'Syria', 'Tanzania', 'Togo', 'Tunisia', 'Turkey', 'Uganda', 'Ukraine', 'United Arab Emirates', 'United Kingdom', 'Vatican City State', 'Wales', 'West Germany', 'Western Sahara', 'Yemen', 'Yugoslavia', 'Zaire', 'Zambia', 'Zimbabwe') ")
      sQuery.Append(" ) group by convert(date,subislog_date)  order by convert(date,subislog_date)")

      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

      SqlCommand.CommandText = sQuery.ToString
      SqlReader = SqlCommand.ExecuteReader()

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in " & Me.GetType().FullName & "</b><br /> " + constrExc.Message
      End Try


    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in " & Me.GetType().FullName & " " + ex.Message

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

  Private Function BuildLineChart(ModuleDataTable As DataTable, moduleNumber As Integer) As String
    Dim JavaScriptString As String = ""
    Dim InnerScript As String = ""
    If Not IsNothing(ModuleDataTable) Then
      If ModuleDataTable.Rows.Count > 0 Then
        JavaScriptString += " var Data" & moduleNumber.ToString & " = google.visualization.arrayToDataTable([[" ' & vbNewLine

        For i As Integer = 0 To ModuleDataTable.Columns.Count - 1
          If Trim(InnerScript) <> "" Then
            InnerScript &= ", "
          End If
          If Not InStr(ModuleDataTable.Columns(i).ColumnName.ToUpper, "_YEAR") > 0 And Not InStr(ModuleDataTable.Columns(i).ColumnName.ToUpper, "_MONTH") > 0 And Not InStr(ModuleDataTable.Columns(i).ColumnName.ToUpper, "_DAY") > 0 Then
            InnerScript &= "'" & ModuleDataTable.Columns(i).ColumnName & "'"
          Else
            If InStr(ModuleDataTable.Columns(i).ColumnName.ToUpper, "_MONTH") Then
              InnerScript &= "'Date'"
            ElseIf InStr(ModuleDataTable.Columns(i).ColumnName.ToUpper, "_DAY") Then
              InnerScript &= "'Day'"
            End If
          End If
        Next
        InnerScript &= "], " 'Seperate first row.

        JavaScriptString += InnerScript
        InnerScript = ""


        For Each es As DataRow In ModuleDataTable.Rows
          If InnerScript <> "" Then
            InnerScript += ", "
          End If
          InnerScript += "["
          For j As Integer = 0 To ModuleDataTable.Columns.Count - 1
            If Not InStr(ModuleDataTable.Columns(j).ColumnName.ToUpper, "_YEAR") > 0 And Not InStr(ModuleDataTable.Columns(j).ColumnName.ToUpper, "_MONTH") > 0 And Not InStr(ModuleDataTable.Columns(j).ColumnName.ToUpper, "DAY") > 0 Then
              InnerScript += "" & es.Item(j) & ""
              If j <> (ModuleDataTable.Columns.Count - 1) Then
                InnerScript &= ","
              End If
            Else

              If moduleNumber = 42 Then
                InnerScript += "'" & es.Item("Year_Month_Day") & "'"

                If j <> (ModuleDataTable.Columns.Count - 1) Then
                  InnerScript &= ","
                End If
              ElseIf InStr(ModuleDataTable.Columns(j).ColumnName.ToUpper, "_MONTH") Then
                InnerScript += "'" & es.Item("cstat_month") & "-" & es.Item("cstat_year") & "'"

                If j <> (ModuleDataTable.Columns.Count - 1) Then
                  InnerScript &= ","
                End If
              ElseIf InStr(ModuleDataTable.Columns(j).ColumnName.ToUpper, "_DAY") Then
                InnerScript += "'" & es.Item("cstat_day") & "'"

                If j <> (ModuleDataTable.Columns.Count - 1) Then
                  InnerScript &= ","
                End If
              End If
            End If
          Next
          InnerScript += "]"


        Next

        JavaScriptString += InnerScript
        JavaScriptString += "]); " & vbNewLine
        JavaScriptString += "var options" & moduleNumber.ToString & " = {" & vbNewLine
        JavaScriptString += " legend: 'none'," & vbNewLine
        JavaScriptString += " chartArea: {left:40,top:10,width:'90%',height:'70%'}"


        ' added in MSW - 3/20/20  
        JavaScriptString += " , hAxis: { "
        JavaScriptString += " slantedText: true, "
        JavaScriptString += " textStyle: { fontSize: 10 }, "
        JavaScriptString += " slantedTextAngle: 60 "
        JavaScriptString += " } "


        JavaScriptString += "};" & vbNewLine

        If moduleNumber = 24 Then
          JavaScriptString += " var chart" & moduleNumber.ToString & " = new google.visualization.ColumnChart(document.getElementById('linechart" & moduleNumber.ToString & "'));" & vbNewLine
          JavaScriptString += " chart" & moduleNumber.ToString & ".draw(Data" & moduleNumber.ToString & ", options" & moduleNumber.ToString & "); " & vbNewLine
        Else
          JavaScriptString += " var chart" & moduleNumber.ToString & " = new google.visualization.LineChart(document.getElementById('linechart" & moduleNumber.ToString & "'));" & vbNewLine
          JavaScriptString += " chart" & moduleNumber.ToString & ".draw(Data" & moduleNumber.ToString & ", options" & moduleNumber.ToString & "); " & vbNewLine
        End If



      End If
    End If



    Return JavaScriptString
  End Function


End Class