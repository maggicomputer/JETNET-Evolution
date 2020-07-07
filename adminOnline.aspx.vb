' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/adminOnline.aspx.vb $
'$$Author: Matt $
'$$Date: 9/26/19 12:10p $
'$$Modtime: 9/26/19 12:04p $
'$$Revision: 18 $
'$$Workfile: adminOnline.aspx.vb $
'
' ********************************************************************************

Partial Public Class adminOnline
    Inherits System.Web.UI.Page

    Protected localDatalayer As New admin_center_dataLayer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim productCode As String = ""
        Dim type_to_show As String = ""
        Dim subFrequency As String = ""
        Dim orderByClause As String = ""
        Dim companyBusinessType As String = ""
        Dim subService As String = ""
        Dim companyID As Long = 0

        Dim bShowOverView As Boolean = False

        Dim bGetErrors As Boolean = False
        Dim bShowHostnames As Boolean = False

        Dim error_count As Integer = 50

        Dim htmlOut As New StringBuilder

        Dim sErrorString As String = ""

        If Session.Item("crmUserLogon") <> True Then

            Response.Redirect("Default.aspx", True)

        Else

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

            ' Pass the tab index of what you want highlighted on the bar.
            ' This will set page title.
            ' Switched this out with a more general function that would work for all applications.
            Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("Subscribers Online Now - Home")
            ' This allows you to toggle The Navigation Bar
            ' Master.MenuBarVisibility(False)
            ' This will remove the white background if you want it gone and want to see the background image instead of white box
            ' Master.RemoveWhiteBackground(True)
            ' This toggles the welcome message up top (myEvo Link)
            ' Master.ToggleWelcomeMessage(False)
            ' toggles welcome message + logo
            ' Master.ToggleWelcomeHeader(True)  

            If Not IsNothing(Request.Item("productCode")) Then
                If Not String.IsNullOrEmpty(Request.Item("productCode").Trim) Then
                    If Request.Item("productCode").Trim.ToUpper.Contains("B,C,H,Y") Then
                        productCode = ""
                    Else
                        productCode = Request.Item("productCode").Trim
                    End If
                End If
            End If

            If Not IsNothing(Request.Item("type_to_show")) Then
                If Not String.IsNullOrEmpty(Request.Item("type_to_show").Trim) Then
                    type_to_show = Request.Item("type_to_show").Trim
                End If
            End If

            If Not IsNothing(Request.Item("freq")) Then
                If Not String.IsNullOrEmpty(Request.Item("freq").Trim) Then
                    subFrequency = Request.Item("freq").Trim
                End If
            End If

            If Not IsNothing(Request.Item("order")) Then
                If Not String.IsNullOrEmpty(Request.Item("order").Trim) Then
                    orderByClause = Request.Item("order").Trim
                End If
            End If

            If Not IsNothing(Request.Item("cbus")) Then
                If Not String.IsNullOrEmpty(Request.Item("cbus").Trim) Then
                    companyBusinessType = Request.Item("cbus").Trim
                End If
            End If

            If Not IsNothing(Request.Item("service")) Then
                If Not String.IsNullOrEmpty(Request.Item("service").Trim) Then
                    subService = Request.Item("service").Trim
                End If
            End If

            If Not IsNothing(Request.Item("error_count")) Then
                If Not String.IsNullOrEmpty(Request.Item("error_count").Trim) Then
                    If IsNumeric(Request.Item("error_count").Trim) Then
                        error_count = CInt(Request.Item("error_count").Trim)
                    End If
                End If
            End If

            If Not IsNothing(Request.Item("errors")) Then
                If Not String.IsNullOrEmpty(Request.Item("errors").Trim) Then
                    If Request.Item("errors").Trim.ToUpper.Contains("Y") Then
                        bGetErrors = True
                    End If
                End If
            End If

            If Not IsNothing(Request.Item("showHostnames")) Then
                If Not String.IsNullOrEmpty(Request.Item("showHostnames").Trim) Then
                    If Request.Item("showHostnames").Trim.ToUpper.Contains("Y") Then
                        bShowHostnames = True
                    End If
                End If
            End If

            If Not IsNothing(Request.Item("id")) Then
                If Not String.IsNullOrEmpty(Request.Item("id").Trim) Then
                    companyID = CLng(Request.Item("id").Trim)
                End If
            End If

            If Not IsNothing(Request.Item("overView")) Then
                If Not String.IsNullOrEmpty(Request.Item("overView").Trim) Then
                    If Request.Item("overView").Trim.ToUpper.Contains("Y") Then
                        bShowOverView = True
                        If IsPostBack Then
                            Response.Redirect("adminOnline.aspx?overView=Y&order=" + orderByClause.Trim + "&service=" + subService.Trim + "&productCode=" + productCode.Trim + "&freq=" + subFrequency.Trim, True)
                        End If
                    End If
                End If
            End If

            If bShowOverView Or type_to_show.Trim.ToUpper.Contains("ALL") Then

                Master.Set_Active_Tab(2)
                btnRefresh.Visible = True
                Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("Evolution Subscriber Overview - Home")

                If String.IsNullOrEmpty(productCode.Trim) Then
                    htmlOut.Append("Business <input type=""checkbox"" name=""productCode"" value=""B"" checked=""checked"">&nbsp;&nbsp;")
                    htmlOut.Append("Commercial <input type=""checkbox"" name=""productCode"" value=""C"" checked=""checked"">&nbsp;&nbsp;")
                    htmlOut.Append("Helicopters <input type=""checkbox"" name=""productCode"" value=""H"" checked=""checked"">&nbsp;&nbsp;")
                    htmlOut.Append("Yachts <input type=""checkbox"" name=""productCode"" value=""Y"" checked=""checked"">&nbsp;&nbsp;")
                Else
                    If productCode.ToUpper.Trim.Contains("B") Then
                        htmlOut.Append("Business <input type=""checkbox"" name=""productCode"" value=""B"" checked=""checked"">&nbsp;&nbsp;")
                    Else
                        htmlOut.Append("Business <input type=""checkbox"" name=""productCode"" value=""B"">&nbsp;&nbsp;")
                    End If

                    If productCode.ToUpper.Trim.Contains("C") Then
                        htmlOut.Append("Commercial <input type=""checkbox"" name=""productCode"" value=""C"" checked=""checked"">&nbsp;&nbsp;")
                    Else
                        htmlOut.Append("Commercial <input type=""checkbox"" name=""productCode"" value=""C"">&nbsp;&nbsp;")
                    End If

                    If productCode.ToUpper.Trim.Contains("H") Then
                        htmlOut.Append("Helicopters <input type=""checkbox"" name=""productCode"" value=""H"" checked=""checked"">&nbsp;&nbsp;")
                    Else
                        htmlOut.Append("Helicopters <input type=""checkbox"" name=""productCode"" value=""H"">&nbsp;&nbsp;")
                    End If

                    If productCode.ToUpper.Trim.Contains("Y") Then
                        htmlOut.Append("Yachts <input type=""checkbox"" name=""productCode"" value=""Y"" checked=""checked"">&nbsp;&nbsp;")
                    Else
                        htmlOut.Append("Yachts <input type=""checkbox"" name=""productCode"" value=""Y"">&nbsp;&nbsp;")
                    End If
                End If

                htmlOut.Append("<select name=""service"" id=""serviceID"">")

                If String.IsNullOrEmpty(subService.Trim) Then
                    htmlOut.Append("<option value="""" selected=""selected"">All</option>")
                Else
                    htmlOut.Append("<option value="""">All</option>")
                End If

                If subService.ToUpper.Trim.Contains("M") Then
                    htmlOut.Append("<option value=""M"" selected=""selected"">Marketplace</option>")
                Else
                    htmlOut.Append("<option value=""M"">Marketplace</option>")
                End If

                If subService.ToUpper.Trim.Contains("A") Then
                    htmlOut.Append("<option value=""A"" selected=""selected"">Aerodex</option>")
                Else
                    htmlOut.Append("<option value=""A"">Aerodex</option>")
                End If

                htmlOut.Append("</select>")

                htmlOut.Append("<input type=""hidden"" value=""Y"" id=""overView"" name=""overView"">")
                htmlOut.Append("<input type=""hidden"" value=""" + subFrequency.Trim + """ id=""freqID"" name=""freq"">")
                htmlOut.Append("<input type=""hidden"" value=""" + orderByClause.Trim + """ id=""orderID"" name=""order""><br /><br />")

                htmlOut.Append(localDatalayer.displayAdminOnLineOverview("freq", subFrequency.Trim, companyBusinessType.Trim, productCode.Trim, subService.Trim, orderByClause.Trim))

                htmlOut.Append(localDatalayer.displayAdminOnLineOverview("types", subFrequency.Trim, companyBusinessType.Trim, productCode.Trim, subService.Trim, orderByClause.Trim))

            Else

                Master.Set_Active_Tab(1)
                Dim totalUsersOn As Integer = 0
                Dim totalAerodexOn As Integer = 0
                Dim totalLiveUsers As Integer = 0

                Dim totalWeeklyUsers As Integer = 0
                Dim totalMonthlyUsers As Integer = 0
                Dim totalBiWeeklyUsers As Integer = 0
                Dim totalBusinessUsers As Integer = 0
                Dim totalCommercialUsers As Integer = 0
                Dim totalHelicopterUsers As Integer = 0
                Dim totalYachtUsers As Integer = 0

                Dim growth_graph1 As String = ""
                Dim growth_graph2 As String = ""
                Dim total1 As Integer = 0
                Dim total_updown As Integer = 0
                Dim total_marketplace As Integer = 0
                Dim marketplace_updown As Integer = 0
                Dim total_aerodex As Integer = 0
                Dim aerodex_updown As Integer = 0
                Dim temp_tickers As String = ""
                Dim total_last As Integer = 0
                Dim marketplace_last As Integer = 0
                Dim aerodex_last As Integer = 0


                If Trim(Request("growth")) = "Y" Then
                    htmlOut.Append(localDatalayer.displayAdminOnLine(productCode.Trim, type_to_show.Trim, companyID, bShowHostnames, bGetErrors, error_count, totalUsersOn, totalAerodexOn, totalLiveUsers, totalWeeklyUsers, totalMonthlyUsers, totalBiWeeklyUsers, totalBusinessUsers, totalCommercialUsers, totalHelicopterUsers, totalYachtUsers, "Y", Trim(Request("growth")), growth_graph1, growth_graph2, ""))

                    Call localDatalayer.ticker_selects(total1, total_updown, total_marketplace, marketplace_updown, total_aerodex, aerodex_updown, total_last, marketplace_last, aerodex_last, False)

                    temp_tickers &= "<table width=""100%"" valign='top'><tr valign='top'><td valign='top'>"
                    temp_tickers &= "<div class=""Box""><table width=""95%""><tr><td><div class=""subHeader"">LICENSE GROWTH – THIS MONTH (" & MonthName(Month(Now())) & ")</div>"
                    temp_tickers &= (DisplayFunctions.make_ticker_box_growth("Total Licenses", total_updown, FormatNumber(total_last, 0) & " LAST", FormatNumber(total1, 0) & " NOW", False, False))

                    temp_tickers &= (DisplayFunctions.make_ticker_box_growth("MARKETPLACE Licenses", marketplace_updown, FormatNumber(marketplace_last, 0) & " LAST", FormatNumber(total_marketplace, 0) & " NOW", False, False))

                    temp_tickers &= (DisplayFunctions.make_ticker_box_growth("AERODEX Licenses", aerodex_updown, FormatNumber(aerodex_last, 0) & " LAST", FormatNumber(total_aerodex, 0) & " NOW", False, False))

                    temp_tickers &= "</td></tr></table></div>"

                    temp_tickers &= "</td><td valign='top'>"
                    Call localDatalayer.ticker_selects(total1, total_updown, total_marketplace, marketplace_updown, total_aerodex, aerodex_updown, total_last, marketplace_last, aerodex_last, True)

                    temp_tickers &= "<div class=""Box""><table width=""95%""><tr><td><div class=""subHeader"">LICENSE GROWTH – YEAR TO DATE (" & Year(Now()) & ")</div>"
                    temp_tickers &= (DisplayFunctions.make_ticker_box_growth("Total Licenses", total_updown, FormatNumber(total_last, 0) & " LAST", FormatNumber(total1, 0) & " NOW", False, False))

                    temp_tickers &= (DisplayFunctions.make_ticker_box_growth("MARKETPLACE Licenses", marketplace_updown, FormatNumber(marketplace_last, 0) & " LAST", FormatNumber(total_marketplace, 0) & " NOW", False, False))

                    temp_tickers &= (DisplayFunctions.make_ticker_box_growth("AERODEX Licenses", aerodex_updown, FormatNumber(aerodex_last, 0) & " LAST", FormatNumber(total_aerodex, 0) & " NOW", False, False))

                    temp_tickers &= "</td></tr></table></div>"
                    temp_tickers &= "</td></tr></table>"


                    temp_tickers = Replace(htmlOut.ToString, "YYYYY", temp_tickers & "<br/><br/>")
                    temp_tickers = Left(Trim(temp_tickers), InStr(Trim(temp_tickers), "ZZZZ") - 1)




                    htmlOut.Length = 0
                    htmlOut.Append(temp_tickers)

                    BuildGoogleCharts_Growth(growth_graph1, growth_graph2)

                    ' htmlOut.Append(DisplayFunctions.make_ticker_box("Days On Market (" & TimeSpan & " Months)", dom_up_down_value, dom_percent, avgdom_1 & " DAYS", False, False))
                Else
                    htmlOut.Append(localDatalayer.displayAdminOnLine(productCode.Trim, type_to_show.Trim, companyID, bShowHostnames, bGetErrors, error_count, totalUsersOn, totalAerodexOn, totalLiveUsers, totalWeeklyUsers, totalMonthlyUsers, totalBiWeeklyUsers, totalBusinessUsers, totalCommercialUsers, totalHelicopterUsers, totalYachtUsers, Trim(Request("complete")), Trim(Request("growth")), growth_graph1, growth_graph2, location_drop.Text))
                    BuildGoogleCharts(totalUsersOn, totalAerodexOn, totalLiveUsers, totalWeeklyUsers, totalMonthlyUsers, totalBiWeeklyUsers, totalBusinessUsers, totalCommercialUsers, totalHelicopterUsers, totalYachtUsers)
                    location_drop.Visible = True
                End If


            End If

            admin_online_display.Text = htmlOut.ToString

        End If

    End Sub
    Private Sub BuildGoogleCharts(totalUsersOn As Integer, totalAerodexOn As Integer, totalLiveUsers As Integer, totalWeeklyUsers As Integer, totalMonthlyUsers As Integer, totalBiWeeklyUsers As Integer, totalBusinessUsers As Integer, totalCommercialUsers As Integer, totalHelicopterUsers As Integer, totalYachtUsers As Integer)

        Dim jsStr As String = "function drawMarketCharts() {" & vbNewLine

        'Create Aerodex vs Marketplace chart.
        jsStr += " var Data = google.visualization.arrayToDataTable([" & vbNewLine
        jsStr += "['Program', '%']," & vbNewLine
        jsStr += "['MARKETPLACE',     " & (totalUsersOn - totalAerodexOn) / totalUsersOn & "]," & vbNewLine
        jsStr += " ['AERODEX',      " & totalAerodexOn / totalUsersOn & "]" & vbNewLine
        jsStr += "]); " & vbNewLine
        jsStr += "var options = {" & vbNewLine
        jsStr += " chartArea: {left:20,top:0,width:'100%',height:'90%'}"
        jsStr += "};" & vbNewLine
        jsStr += " var chart = new google.visualization.PieChart(document.getElementById('piechartMarket'));" & vbNewLine
        jsStr += " chart.draw(Data, options); " & vbNewLine

        'Create Frequency Chart
        jsStr += " var DataFreq = google.visualization.arrayToDataTable([" & vbNewLine
        jsStr += "['Frequency', '%']," & vbNewLine
        jsStr += "['LIVE',     " & totalLiveUsers / totalUsersOn & "]," & vbNewLine
        jsStr += " ['WEEKLY',     " & totalWeeklyUsers / totalUsersOn & "]," & vbNewLine
        If totalBiWeeklyUsers > 0 Then
            jsStr += " ['BIWEEKLY',    " & totalBiWeeklyUsers / totalUsersOn & "]," & vbNewLine
        End If
        jsStr += " ['MONTHLY',     " & totalMonthlyUsers / totalUsersOn & "]" & vbNewLine

        jsStr += "]); " & vbNewLine

        jsStr += " var chartFreq = new google.visualization.PieChart(document.getElementById('frequencychartMarket'));" & vbNewLine
        jsStr += " chartFreq.draw(DataFreq, options); " & vbNewLine


        'Create AC Types Chart
        jsStr += " var DataType = google.visualization.arrayToDataTable([" & vbNewLine
        jsStr += "['Type', '#', { role: 'annotation'}, { role: 'style' }]," & vbNewLine

        jsStr += "['BUSINESS',     " & totalBusinessUsers & ", 'BUSINESS', '#3366cc']," & vbNewLine
        jsStr += " ['HELICOPTER',    " & totalHelicopterUsers & ", 'HELICOPTER', '#dc3912']," & vbNewLine
        jsStr += " ['COMMERCIAL',     " & totalCommercialUsers & ", 'COMMERCIAL','#ff9900']," & vbNewLine

        jsStr += " ['YACHT',     " & totalYachtUsers & ", 'YACHT','#15970b']" & vbNewLine

        jsStr += "]); " & vbNewLine

        jsStr += "var optionsType = {" & vbNewLine
        jsStr += " legend: 'none'," & vbNewLine
        jsStr += " chartArea: {left:0,top:0,width:'100%',height:'90%'}"
        jsStr += "};" & vbNewLine

        jsStr += " var chartType = new google.visualization.BarChart(document.getElementById('acTypechartMarket'));" & vbNewLine
        jsStr += " chartType.draw(DataType, optionsType); " & vbNewLine

        jsStr += "};" & vbNewLine
        jsStr += " google.charts.setOnLoadCallback(drawMarketCharts);" & vbNewLine


        jsStr += "$(window).resize(function() {" & vbNewLine
        jsStr += "if(this.resizeTO) clearTimeout(this.resizeTO);" & vbNewLine
        jsStr += "this.resizeTO = setTimeout(function() {" & vbNewLine
        jsStr += "$(this).trigger('resizeEnd');" & vbNewLine
        jsStr += "}, 500);" & vbNewLine
        jsStr += "});" & vbNewLine

        '//redraw graph when window resize is completed  
        jsStr += "$(window).on('resizeEnd', function() {" & vbNewLine
        jsStr += "$('#frequencychartMarket').empty(); " & vbNewLine
        jsStr += "$('#piechartMarket').empty(); " & vbNewLine
        jsStr += "$('#acTypechartMarket').empty(); " & vbNewLine
        jsStr += "   drawMarketCharts();" & vbNewLine
        jsStr += "});" & vbNewLine


        System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "DrawChart", jsStr, True)
    End Sub

    Private Sub BuildGoogleCharts_Growth(ByVal growth1 As String, ByVal growth2 As String)

        Dim jsStr As String = "function drawMarketCharts() {" & vbNewLine

        'Create Aerodex vs Marketplace chart.
        jsStr += " var Data = google.visualization.arrayToDataTable([" & vbNewLine
        jsStr += "['Date', 'Total','Marketplace','Aerodex']," & vbNewLine
        jsStr += growth1
        jsStr += "]); " & vbNewLine
        jsStr += "var options = {" & vbNewLine
        jsStr += " chartArea: {top:8,width:'80%',height:'70%'}"
        jsStr += ", legend: 'none'," & vbNewLine
        jsStr += "};" & vbNewLine
        jsStr += " var chart = new google.visualization.LineChart(document.getElementById('piechartMarket'));" & vbNewLine
        jsStr += " chart.draw(Data, options); " & vbNewLine

        'Create Frequency Chart
        jsStr += " var DataFreq = google.visualization.arrayToDataTable([" & vbNewLine
        jsStr += "['Date', 'Values']," & vbNewLine
        jsStr += growth2
        jsStr += "]); " & vbNewLine

        jsStr += " var chartFreq = new google.visualization.LineChart(document.getElementById('frequencychartMarket'));" & vbNewLine
        jsStr += " chartFreq.draw(DataFreq, options); " & vbNewLine



        jsStr += "};" & vbNewLine
        jsStr += " google.charts.setOnLoadCallback(drawMarketCharts);" & vbNewLine


        jsStr += "$(window).resize(function() {" & vbNewLine
        jsStr += "if(this.resizeTO) clearTimeout(this.resizeTO);" & vbNewLine
        jsStr += "this.resizeTO = setTimeout(function() {" & vbNewLine
        jsStr += "$(this).trigger('resizeEnd');" & vbNewLine
        jsStr += "}, 500);" & vbNewLine
        jsStr += "});" & vbNewLine

        '//redraw graph when window resize is completed  
        jsStr += "$(window).on('resizeEnd', function() {" & vbNewLine
        jsStr += "$('#frequencychartMarket').empty(); " & vbNewLine
        jsStr += "$('#piechartMarket').empty(); " & vbNewLine
        jsStr += "   drawMarketCharts();" & vbNewLine
        jsStr += "});" & vbNewLine


        System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "DrawChart", jsStr, True)
    End Sub
End Class