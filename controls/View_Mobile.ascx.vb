Partial Public Class View_Mobile
  Inherits System.Web.UI.UserControl
  Dim SharedModelTable As New DataTable
  Dim localCriteria As New viewSelectionCriteriaClass
  Public aclsData_Temp As New clsData_Manager_SQL
  Dim tmpViewObj As New viewsDataLayer
  Dim market_functions As New market_model_functions
  Dim localDataLayer As New viewsDataLayer



  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Me.Visible = True Then

      Try
        If Not Page.IsPostBack Then
          If Not IsNothing(Request.Item("tabStarReportID")) Then
            If Not String.IsNullOrEmpty(Request.Item("tabStarReportID").ToString) Then
              If CInt(Request.Item("tabStarReportID").ToString) > -1 Then
                localCriteria.ViewCriteriaStarReportID = CInt(Request.Item("tabStarReportID").ToString)
              Else
                localCriteria.ViewCriteriaStarReportID = -1
                localCriteria.ViewCriteriaStarReportDate = Now.ToString
              End If
            End If
          End If

          If Not IsNothing(Request.Item("amod_id")) Then 'It does exist.
            If Not String.IsNullOrEmpty(Request.Item("amod_id").ToString) Then 'It isn't empty.
              If IsNumeric(Request.Item("amod_id")) Then 'It is numeric.
                If CLng(Request.Item("amod_id")) <> -1 Then 'It doesn't equal -1
                  If CLng(Request.Item("amod_id")) > 0 Then 'Greater than zero.
                    localCriteria.ViewCriteriaAmodID = CLng(Request.Item("amod_id"))
                  End If
                End If
              End If
            End If
          End If
        End If

        tmpViewObj.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
        tmpViewObj.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim

        aclsData_Temp.JETNET_DB = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
        aclsData_Temp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase") 'CApplication.Item("crmClientDatabase")


        market_functions.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
        market_functions.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
        market_functions.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
        market_functions.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
        market_functions.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim


        localDataLayer.adminConnectStr = Application.Item("crmClientSiteData").AdminDatabaseConn
        localDataLayer.clientConnectStr = Session.Item("localPreferences").UserDatabaseConn
        localDataLayer.starConnectStr = Session.Item("localPreferences").STARDatabaseConn
        localDataLayer.serverConnectStr = Session.Item("localPreferences").ServerNotesDatabaseConn

        LoadModelDropDown()

        If Page.IsPostBack Then
          'Try dropdown:
          If Not String.IsNullOrEmpty(makeModelDynamic.SelectedValue) Then
            Dim ModelData As Array = Split(makeModelDynamic.SelectedValue, "|")
            If UBound(ModelData) = 3 Then
              localCriteria.ViewCriteriaAmodID = CLng(ModelData(3))
            End If
          End If
        End If

        localCriteria.ViewCriteriaTimeSpan = 6
        localCriteria.ViewID = 1
        localCriteria.ViewName = Server.UrlEncode("Model Market Summary")

        LoadInitJavascript()
        If localCriteria.ViewCriteriaAmodID <= 0 Then

          If Session.Item("localPreferences").DefaultModel > 0 Then
            localCriteria.ViewCriteriaAmodID = Session.Item("localPreferences").DefaultModel
          Else
            If Session.Item("localPreferences").UserBusinessFlag = True Then
              If Session.Item("localPreferences").Tierlevel = eTierLevelTypes.TURBOS Then
                localCriteria.ViewCriteriaAmodID = 207
              Else 'Jets or ALL
                localCriteria.ViewCriteriaAmodID = 272
              End If
            ElseIf Session.Item("localPreferences").UserCommercialFlag = True Then
              localCriteria.ViewCriteriaAmodID = 698
            ElseIf Session.Item("localPreferences").UserHelicopterFlag = True Then
              localCriteria.ViewCriteriaAmodID = 408
            End If
          End If
        End If

        If localCriteria.ViewCriteriaAmodID > 0 Then
          containerBox.Visible = True
          CheckSharedModelTable()
          BuildFleetTab()
          LoadDescription()
          LoadOperatingCosts()
          LoadPerformance()


          If Session.Item("localSubscription").crmAerodexFlag = False Then
            BuildTrends()
            BuildForSaleTab()
          End If

          BuildSalesTab()
          BuildStarTab()
        End If

      Catch ex As Exception
        Call commonLogFunctions.Log_User_Event_Data("UserError", Replace(Request.Url.AbsolutePath, "/", "") & " : " & System.Reflection.MethodInfo.GetCurrentMethod().ToString.Trim & " " & Replace(" Exception Thrown[" + ex.Message.Trim, "'", "''"), Nothing, 0, 0, 0, 0, 0)
      End Try
    End If
  End Sub

  Public Sub LoadModelDropDown()
    Try
      If Not Page.IsPostBack Then
        DisplayFunctions.SingleModelLookupAndFill(makeModelDynamic, DirectCast(Page.Master, MobileTheme))
      End If
    Catch ex As Exception
      Call commonLogFunctions.Log_User_Event_Data("UserError", Replace(Request.Url.AbsolutePath, "/", "") & " : " & System.Reflection.MethodInfo.GetCurrentMethod().ToString.Trim & " " & Replace(" Exception Thrown[" + ex.Message.Trim, "'", "''"), Nothing, 0, 0, 0, 0, 0)
    End Try
  End Sub
  Public Sub BuildStarTab()
    Try
      Dim ResultText As String = ""
      Dim viewReference As New View_Master
      starHeader.Text = "<a name=""star""></a><br /><h2>Star Reports</h2>"



      If localCriteria.ViewCriteriaStarReportID = 0 Or localCriteria.ViewCriteriaStarReportID = -1 Then
        viewReference.Build_star_links(localCriteria, ResultText)
        ResultText = Replace(Replace(ResultText, " width=""5%""", ""), "' title='", "#star' title='")
        starText.Text += ResultText

      ElseIf localCriteria.ViewCriteriaStarReportID > -1 Then
        starHeader.Text += "<a href=""javascript:void(0);"" onclick=""javascript:return PopUpPanel();"">Open Report</a>"
        PopUpPanelJavascript()
        viewReference.Build_star_report(localCriteria, ResultText)
        ResultText = Replace(Replace(ResultText, " width=""800""", "id=""starContent"" style=""display:none;"""), "'>HERE", "#star'>HERE")
        starText.Text += ResultText
      End If
    Catch ex As Exception
      Call commonLogFunctions.Log_User_Event_Data("UserError", Replace(Request.Url.AbsolutePath, "/", "") & " : " & System.Reflection.MethodInfo.GetCurrentMethod().ToString.Trim & " " & Replace(" Exception Thrown[" + ex.Message.Trim, "'", "''"), Nothing, 0, 0, 0, 0, 0)
    End Try
  End Sub
  Public Sub PopUpPanelJavascript()
    Try
      If Not Page.ClientScript.IsClientScriptBlockRegistered("popupLink") Then
        Dim popString As New StringBuilder

        popString.Append("function PopUpPanel() {")
        popString.Append("var panel = document.getElementById(""" & starReportHolder.ClientID & """);")
        popString.Append("var content = document.getElementById(""starContent"");")
        popString.Append("content.removeAttribute(""style"");")
        popString.Append("var printWindow = window.open('', '', 'menubar=yes, resizable=yes, titlebar=yes, height=760,width=800');")
        popString.Append("printWindow.document.write('<html><head><title>StarReport</title>');")
        popString.Append("printWindow.document.write('<link rel=""stylesheet"" href=""/EvoStyles/stylesheets/additional_styles.css"" /><link rel=""stylesheet"" href=""/EvoStyles/stylesheets/additional_mobile_styles.css"" /><link rel=""stylesheet"" href=""/common/aircraft_model.css"" /><style>body div {height:auto !important;width:auto !important;}</style></head><body >');")
        popString.Append("printWindow.document.write(panel.innerHTML);")
        popString.Append("printWindow.document.write('</body></html>');")
        popString.Append("printWindow.document.close();")
        popString.Append("content.setAttribute(""style"", ""display:none;"");")
        popString.Append("return false;")
        popString.Append("}")
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "popupLink", popString.ToString, True)
      End If
    Catch ex As Exception
      Call commonLogFunctions.Log_User_Event_Data("UserError", Replace(Request.Url.AbsolutePath, "/", "") & " : " & System.Reflection.MethodInfo.GetCurrentMethod().ToString.Trim & " " & Replace(" Exception Thrown[" + ex.Message.Trim, "'", "''"), Nothing, 0, 0, 0, 0, 0)
    End Try
  End Sub
  Public Sub BuildSalesTab()
    Try
      Dim ResultsTable As New DataTable
      ResultsTable = localDataLayer.get_retail_sales_info(localCriteria, "N", "Y", "", "", CInt(localCriteria.ViewCriteriaTimeSpan))
      If Not IsNothing(ResultsTable) Then
        If ResultsTable.Rows.Count > 0 Then
          retailText.Text = "<a name=""sales""></a><br /><h2>Retail Sales <span class=""tiny"">" & ResultsTable.Rows.Count.ToString & " Results</span></h2>"
          TransactionSearchDataList.DataSource = ResultsTable
          TransactionSearchDataList.DataBind()
        End If
      End If
    Catch ex As Exception
      Call commonLogFunctions.Log_User_Event_Data("UserError", Replace(Request.Url.AbsolutePath, "/", "") & " : " & System.Reflection.MethodInfo.GetCurrentMethod().ToString.Trim & " " & Replace(" Exception Thrown[" + ex.Message.Trim, "'", "''"), Nothing, 0, 0, 0, 0, 0)
    End Try
  End Sub
  Public Sub BuildForSaleTab()
    Try
      Dim ResultsTable As New DataTable
      ResultsTable = localDataLayer.get_model_forsale_info(localCriteria)
      If Not IsNothing(ResultsTable) Then
        If ResultsTable.Rows.Count > 0 Then
          forsale_text.Text = "<a name=""forsale""></a><br /><h2>For Sale <span class=""tiny"">" & ResultsTable.Rows.Count.ToString & " Results</span></h2>"
          AircraftSearchDataList.DataSource = ResultsTable
          AircraftSearchDataList.DataBind()
        End If
      End If
    Catch ex As Exception
      Call commonLogFunctions.Log_User_Event_Data("UserError", Replace(Request.Url.AbsolutePath, "/", "") & " : " & System.Reflection.MethodInfo.GetCurrentMethod().ToString.Trim & " " & Replace(" Exception Thrown[" + ex.Message.Trim, "'", "''"), Nothing, 0, 0, 0, 0, 0)
    End Try
  End Sub

  Public Sub BuildTrends()
    Dim htmlUpDown As String = ""
    Dim imgCnt As Integer = 0
    Dim sImageMapPath As String = ""
    Dim sImageSrc As String = ""
    Dim sImageName As String = ""
    Dim subscriptionInfo As String = Session.Item("localUser").crmSubSubID.ToString + "_" + Session.Item("localUser").crmUserLogin.ToString.Trim + "_" + Session.Item("localUser").crmSubSeqNo.ToString + "_"
    Dim displayFolder As String = HttpContext.Current.Application.Item("crmClientSiteData").ClientFullHostName + HttpContext.Current.Session.Item("MarketSummaryFolderVirtualPath")
    Try
      trends_text.Text = "<a name=""trends""></a><h2>Trends</h2>"
      If Not Session.Item("localPreferences").AerodexFlag Then
        market_functions.views_display_market_up_down_one_model(localCriteria, htmlUpDown)
        trends_text.Text += htmlUpDown
      End If

      If Not Session.Item("localPreferences").AerodexFlag Then

        imgCnt += 1
        sImageName = subscriptionInfo + commonEvo.GenerateFileName("image_" + imgCnt.ToString, ".jpg", False)
        sImageMapPath = Server.MapPath(Session.Item("MarketSummaryFolderVirtualPath")) + "\" + sImageName
        sImageSrc = displayFolder + "/" + sImageName

        FOR_SALE.Titles.Clear()
        FOR_SALE.Titles.Add("For Sale By Month (past " + localCriteria.ViewCriteriaTimeSpan.ToString + " months)")
        market_functions.views_display_for_sale_by_month_graph(localCriteria, Me.FOR_SALE)
        FOR_SALE.ImageType = DataVisualization.Charting.ChartImageType.Jpeg
        FOR_SALE.SaveImage(sImageMapPath, DataVisualization.Charting.ChartImageFormat.Jpeg)
        trends_text.Text += "<img src=""" + sImageSrc + """ width=""240"" height=""240"" style=""height:240px; width:240px;"">"
      End If


      imgCnt += 1
      sImageName = subscriptionInfo + commonEvo.GenerateFileName("image_" + imgCnt.ToString, ".jpg", False)
      sImageMapPath = Server.MapPath(Session.Item("MarketSummaryFolderVirtualPath")) + "\" + sImageName
      sImageSrc = displayFolder + "/" + sImageName

      AVG_PRICE_MONTH.Titles.Clear()
      AVG_PRICE_MONTH.Titles.Add("Avg Price By Month (past " + localCriteria.ViewCriteriaTimeSpan.ToString + " months)")
      market_functions.views_display_avg_price_by_month_graph(localCriteria, Me.AVG_PRICE_MONTH, "", False)
      AVG_PRICE_MONTH.ImageType = DataVisualization.Charting.ChartImageType.Jpeg
      AVG_PRICE_MONTH.SaveImage(sImageMapPath, DataVisualization.Charting.ChartImageFormat.Jpeg)
      trends_text.Text += "<img src=""" + sImageSrc + """ width=""240"" height=""240"" style=""height:240px; width:240px;"">"


      imgCnt += 1
      sImageName = subscriptionInfo + commonEvo.GenerateFileName("image_" + imgCnt.ToString, ".jpg", False)
      sImageMapPath = Server.MapPath(Session.Item("MarketSummaryFolderVirtualPath")) + "\" + sImageName
      sImageSrc = displayFolder + "/" + sImageName

      PER_MONTH.Titles.Clear()
      PER_MONTH.Titles.Add("Sold Per Month (past " + localCriteria.ViewCriteriaTimeSpan.ToString + " months)")
      market_functions.views_display_sold_per_month_graph(localCriteria, True, Me.PER_MONTH)
      PER_MONTH.ImageType = DataVisualization.Charting.ChartImageType.Jpeg
      PER_MONTH.SaveImage(sImageMapPath, DataVisualization.Charting.ChartImageFormat.Jpeg)
      trends_text.Text += "<img src=""" + sImageSrc + """ width=""240"" height=""240"" style=""height:240px; width:240px;"">"

      imgCnt += 1
      sImageName = subscriptionInfo + commonEvo.GenerateFileName("image_" + imgCnt.ToString, ".jpg", False)
      sImageMapPath = Server.MapPath(Session.Item("MarketSummaryFolderVirtualPath")) + "\" + sImageName
      sImageSrc = displayFolder + "/" + sImageName

      AVG_DAYS_ON.Titles.Clear()
      AVG_DAYS_ON.Titles.Add("Avg Days on Market (past " + localCriteria.ViewCriteriaTimeSpan.ToString + " months)")
      market_functions.views_display_average_days_on_market_graph(localCriteria, Me.AVG_DAYS_ON)
      AVG_DAYS_ON.ImageType = DataVisualization.Charting.ChartImageType.Jpeg
      AVG_DAYS_ON.SaveImage(sImageMapPath, DataVisualization.Charting.ChartImageFormat.Jpeg)
      trends_text.Text += "<img src=""" + sImageSrc + """ width=""240"" height=""240"" style=""height:240px; width:240px;"">"

    Catch ex As Exception
      Call commonLogFunctions.Log_User_Event_Data("UserError", Replace(Request.Url.AbsolutePath, "/", "") & " : " & System.Reflection.MethodInfo.GetCurrentMethod().ToString.Trim & " " & Replace(" Exception Thrown[" + ex.Message.Trim, "'", "''"), Nothing, 0, 0, 0, 0, 0)
    End Try
  End Sub
  Public Sub LoadInitJavascript()
    Try
      Dim dropdownString As New StringBuilder
      dropdownString.Append("$(""#metricToggle"").click(function() {")
      dropdownString.Append("$(""#standardTable"").hide();")
      dropdownString.Append("$(""#metricTable"").show();")
      dropdownString.Append("});")
      dropdownString.Append("$(""#imperialToggle"").click(function() {")
      dropdownString.Append("$(""#metricTable"").hide();")
      dropdownString.Append("$(""#standardTable"").show();")
      dropdownString.Append("});")

      dropdownString.Append("$(""#standardTableOp #metricToggle"").click(function() {")
      dropdownString.Append("$(""#standardTableOp"").hide();")
      dropdownString.Append("$(""#metricTableOp"").show();")
      dropdownString.Append("});")
      dropdownString.Append("$(""#metricTableOp #imperialToggle"").click(function() {")
      dropdownString.Append("$(""#metricTableOp"").hide();")
      dropdownString.Append("$(""#standardTableOp"").show();")
      dropdownString.Append("});")

      dropdownString.Append("function swapChosenDropdowns() {")
      dropdownString.Append("$("".chosen-select"").chosen(""destroy"");")
      dropdownString.Append("$("".chosen-select"").chosen({ no_results_text: ""No results found."", disable_search_threshold: 10 });")
      dropdownString.Append("}")


      If Not Page.ClientScript.IsClientScriptBlockRegistered("chosenDropdowns") Then
        System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "chosenDropdowns", dropdownString.ToString, True)
      End If

      dropdownString = New StringBuilder
      dropdownString.Append(";swapChosenDropdowns();")

      If Not Page.IsPostBack Then
        System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CreateDropdown", dropdownString.ToString, True)
      End If
    Catch ex As Exception
      Call commonLogFunctions.Log_User_Event_Data("UserError", Replace(Request.Url.AbsolutePath, "/", "") & " : " & System.Reflection.MethodInfo.GetCurrentMethod().ToString.Trim & " " & Replace(" Exception Thrown[" + ex.Message.Trim, "'", "''"), Nothing, 0, 0, 0, 0, 0)
    End Try
  End Sub
  Public Sub CheckSharedModelTable()
    Try
      'All that this does is check for the existence of the shared model table - basically so you don't use it if it doesn't exist.
      'It returns nothing but fills it up if it's not there.
      If Not IsNothing(SharedModelTable) Then
        If SharedModelTable.Rows.Count = 0 Then
          'We need to fill this up. If it has rows already, it already exists - so we're good to go.
          SharedModelTable = commonEvo.get_view_model_info(localCriteria, True)
        End If
      Else
        SharedModelTable = commonEvo.get_view_model_info(localCriteria, True)
      End If

      If Not Page.IsPostBack Then
        If Not IsNothing(SharedModelTable) Then
          If SharedModelTable.Rows.Count > 0 Then
            makeModelDynamic.SelectedValue = SharedModelTable.Rows(0).Item("amod_type_code") & "|" & SharedModelTable.Rows(0).Item("amod_airframe_type_code") & "|" & SharedModelTable.Rows(0).Item("amod_make_name") & "|" & localCriteria.ViewCriteriaAmodID
          End If
        End If
      End If
    Catch ex As Exception
      Call commonLogFunctions.Log_User_Event_Data("UserError", Replace(Request.Url.AbsolutePath, "/", "") & " : " & System.Reflection.MethodInfo.GetCurrentMethod().ToString.Trim & " " & Replace(" Exception Thrown[" + ex.Message.Trim, "'", "''"), Nothing, 0, 0, 0, 0, 0)
    End Try
  End Sub

  Public Sub BuildFleetTab()
    Try
      Dim FleetTabText As String = ""
      Dim MarketTabText As String = ""

      market_functions.views_display_fleet_market_summary(localCriteria, FleetTabText, MarketTabText)
      'FleetTabText = Replace(FleetTabText, "</tr>", "<br />")
      'FleetTabText = Regex.Replace(FleetTabText, "((<td[^>]*>)|(</td>))", "")
      'FleetTabText = Regex.Replace(FleetTabText, "((<table[^>]*>)|(</table>))", "")
      'FleetTabText = Regex.Replace(FleetTabText, "((<tr[^>]*>)|(</tr>))", "")

      fleet_text.Text = "<a name=""fleet""></a><h2>Fleet</h2><div class=""containerMarket"">"
      fleet_text.Text += FleetTabText & "</div>"

      ' MarketTabText = Replace(MarketTabText, "</tr>", "<br />")
      If Session.Item("localSubscription").crmAerodexFlag = False Then
        'MarketTabText = Regex.Replace(MarketTabText, "((<td[^>]*>)|(</td>))", "")
        'MarketTabText = Regex.Replace(MarketTabText, "((<table[^>]*>)|(</table>))", "")
        'MarketTabText = Regex.Replace(MarketTabText, "((<tr[^>]*>)|(</tr>))", "")
        MarketTabText = Replace(MarketTabText, "For Sale on Exclusive", "For Sale on Exc.")
        market_text.Text = "<a name=""market""></a><h2>Market Status</h2><div class=""containerMarket"">"
        market_text.Text += MarketTabText & "</div>"
        End If 
    Catch ex As Exception
      Call commonLogFunctions.Log_User_Event_Data("UserError", Replace(Request.Url.AbsolutePath, "/", "") & " : " & System.Reflection.MethodInfo.GetCurrentMethod().ToString.Trim & " " & Replace(" Exception Thrown[" + ex.Message.Trim, "'", "''"), Nothing, 0, 0, 0, 0, 0)
    End Try
  End Sub
  Public Sub LoadOperatingCosts()
    Try
      Dim ResultsText As String = ""
      localCriteria.ViewCriteriaUseMetricValues = False
      HttpContext.Current.Session.Item("localPreferences").DefaultCurrency = 9 'us dollar
      HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate = 0
      operating_text.Text = "<div class=""performance_container_content padding""><a name=""op""></a><h2>Operating Costs</h2>"
      operating_text.Text += "<table id=""standardTableOp"" cellspacing='0' cellpadding='0' class='data_aircraft_grid cell_right performanceTable'>"
      tmpViewObj.views_display_operating_costs(localCriteria, True, ResultsText, SharedModelTable)
      operating_text.Text += "<tr>"

      tmpViewObj.views_display_operating_costs(localCriteria, True, ResultsText)
      operating_text.Text += Replace(Replace(Replace(ResultsText, "&nbsp;", " "), "<br />", " "), "padding-left:5px;", "border:0px !important")
      tmpViewObj.views_display_operating_costs(localCriteria, False, ResultsText, SharedModelTable)
      operating_text.Text += Replace(Replace(Replace(ResultsText, "&nbsp;", " "), "<br />", " "), "padding-left:5px;", "border:0px !important")

      operating_text.Text += "</tr>"
      operating_text.Text += "</table>"

      operating_text.Text += "<table id=""metricTableOp"" style=""display:none"" cellspacing='0' cellpadding='0' class='data_aircraft_grid cell_right performanceTable'>"
      operating_text.Text += "<tr>"

      localCriteria.ViewCriteriaUseMetricValues = True
      HttpContext.Current.Session.Item("localPreferences").DefaultCurrency = 14 'euro
      HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate = 0

      operating_text.Text += "<tr>"

      tmpViewObj.views_display_operating_costs(localCriteria, True, ResultsText)
      operating_text.Text += Replace(Replace(Replace(ResultsText, "&nbsp;", " "), "<br />", " "), "padding-left:5px;", "border:0px !important")
      tmpViewObj.views_display_operating_costs(localCriteria, False, ResultsText, SharedModelTable)
      operating_text.Text += Replace(Replace(Replace(ResultsText, "&nbsp;", " "), "<br />", " "), "padding-left:5px;", "border:0px !important")

      operating_text.Text += "</tr>"
      operating_text.Text += "</table></div>"
    Catch ex As Exception
      Call commonLogFunctions.Log_User_Event_Data("UserError", Replace(Request.Url.AbsolutePath, "/", "") & " : " & System.Reflection.MethodInfo.GetCurrentMethod().ToString.Trim & " " & Replace(" Exception Thrown[" + ex.Message.Trim, "'", "''"), Nothing, 0, 0, 0, 0, 0)
    End Try
  End Sub
  Public Sub LoadPerformance()
    Try
      Dim ResultsText As String = ""

      performance_text.Text = "<a name=""perf""></a><h2>Performance Specs</h2>"

      performance_text.Text += "<table cellspacing='0' cellpadding='0' class='data_aircraft_grid cell_right performanceTable' id=""standardTable"">"
      performance_text.Text += "<tr>"


      localCriteria.ViewCriteriaUseMetricValues = False
      tmpViewObj.views_display_performance_specs(False, "Html", True, localCriteria.ViewCriteriaUseMetricValues, localCriteria, ResultsText)
      performance_text.Text += ResultsText

      tmpViewObj.views_display_performance_specs(False, "Html", False, localCriteria.ViewCriteriaUseMetricValues, localCriteria, ResultsText)
      performance_text.Text += ResultsText
      performance_text.Text += "</tr>"
      performance_text.Text += "</table>"


      'Second table toggle:
      performance_text.Text += "<table cellspacing='0' cellpadding='0' style=""display:none;"" class='data_aircraft_grid cell_right performanceTable' id=""metricTable"">"
      performance_text.Text += "<tr>"
      localCriteria.ViewCriteriaUseMetricValues = True 'opposite

      tmpViewObj.views_display_performance_specs(False, "Html", True, localCriteria.ViewCriteriaUseMetricValues, localCriteria, ResultsText)
      performance_text.Text += ResultsText

      tmpViewObj.views_display_performance_specs(False, "Html", False, localCriteria.ViewCriteriaUseMetricValues, localCriteria, ResultsText)
      performance_text.Text += ResultsText

      performance_text.Text += "</tr>"
      performance_text.Text += "</table>"
    Catch ex As Exception
      Call commonLogFunctions.Log_User_Event_Data("UserError", Replace(Request.Url.AbsolutePath, "/", "") & " : " & System.Reflection.MethodInfo.GetCurrentMethod().ToString.Trim & " " & Replace(" Exception Thrown[" + ex.Message.Trim, "'", "''"), Nothing, 0, 0, 0, 0, 0)
    End Try

  End Sub
  Public Sub LoadDescription()
    Try
      If Not IsNothing(SharedModelTable) Then
        If SharedModelTable.Rows.Count > 0 Then
          For Each r As DataRow In SharedModelTable.Rows

            Dim imgDisplayFolder As String = ""
            Dim DisplayPicture As Boolean = False
            Dim startYear As String = ""
            Dim endYear As String = ""
            Dim yearRange As String = ""
            Dim serStart As String = ""
            Dim serEnd As String = ""
            Dim serRange As String = ""
            Dim serPre As String = ""
            Dim serSuf As String = ""
            Dim startPrice As Double = 0
            Dim endPrice As Double = 0
            Dim priceRange As String = ""
            Dim typeName As String = ""
            Dim weightName As String = ""

            imgDisplayFolder = Session.Item("jetnetFullHostName") + Session.Item("ModelPicturesFolderVirtualPath")

            If SharedModelTable.Rows(0).Item("amod_picture_exists_flag").ToString.ToUpper = "Y" Then
              mainImage.ImageUrl = imgDisplayFolder.Trim + "/" + localCriteria.ViewCriteriaAmodID.ToString + ".jpg"
            End If

            If Not IsDBNull(r.Item("amod_make_name")) Then
              If Not String.IsNullOrEmpty(r.Item("amod_make_name").ToString) Then
                localCriteria.ViewCriteriaAircraftMake = r.Item("amod_make_name").ToString.Trim
              End If
            End If

            If Not IsDBNull(r.Item("amod_model_name")) Then
              If Not String.IsNullOrEmpty(r.Item("amod_make_name").ToString) Then
                localCriteria.ViewCriteriaAircraftModel = r.Item("amod_model_name").ToString.Trim
              End If
            End If


            If Not IsDBNull(r.Item("amod_type_code")) Then
              If Not String.IsNullOrEmpty(r.Item("amod_type_code").ToString) Then
                localCriteria.ViewCriteriaAircraftType = r.Item("amod_type_code").ToString.ToUpper.Trim
              End If
            End If

            If Not IsDBNull(r.Item("amod_airframe_type_code")) Then
              If Not String.IsNullOrEmpty(r.Item("amod_airframe_type_code").ToString) Then
                localCriteria.ViewCriteriaAirframeTypeStr = r.Item("amod_airframe_type_code").ToString.ToUpper.Trim
              End If
            End If

            Select Case localCriteria.ViewCriteriaAircraftType.Trim.ToUpper
              Case Constants.AMOD_TYPE_AIRLINER
                typeName = "Jet Airliner"
              Case Constants.AMOD_TYPE_JET
                typeName = "Business Jet"
              Case Constants.AMOD_TYPE_TURBO
                If localCriteria.ViewCriteriaAirframeTypeStr.Trim.ToUpper.Contains(Constants.AMOD_ROTARY_AIRFRAME) Then
                  typeName = "Turbine"
                Else
                  typeName = "Turboprop"
                End If
              Case Constants.AMOD_TYPE_PISTON
                typeName = "Piston"
            End Select

            If Not IsDBNull(r.Item("amod_weight_class")) Then
              If Not String.IsNullOrEmpty(r.Item("amod_weight_class").ToString) Then
                localCriteria.ViewCriteriaWeightClass = r.Item("amod_weight_class").ToString.ToUpper.Trim
              End If
            End If

            Select Case localCriteria.ViewCriteriaWeightClass
              Case "V"
                weightName = "Very Light Jet"
              Case "L"
                weightName = "Light"
              Case "M"
                weightName = "Medium"
              Case "H"
                weightName = "Heavy"
            End Select

            If Not IsDBNull(r.Item("amod_start_year")) Then
              If Not String.IsNullOrEmpty(r.Item("amod_start_year").ToString) Then
                startYear = r.Item("amod_start_year").ToString.ToUpper.Trim
              End If
            End If


            If Not IsDBNull(r.Item("amod_end_year")) Then
              If Not String.IsNullOrEmpty(r.Item("amod_end_year").ToString) Then
                endYear = r.Item("amod_end_year").ToString.ToUpper.Trim
              End If
            End If

            If Not String.IsNullOrEmpty(endYear) Then
              yearRange = startYear + " - " + endYear + "&nbsp;"
            ElseIf Not String.IsNullOrEmpty(startYear) Then
              yearRange = startYear + " - Present&nbsp;"
            End If

            If Not IsDBNull(r.Item("amod_ser_no_prefix")) Then
              If Not String.IsNullOrEmpty(r.Item("amod_ser_no_prefix").ToString) Then
                serPre = r.Item("amod_ser_no_prefix").ToString.ToUpper.Trim
              End If
            End If

            If Not IsDBNull(r.Item("amod_ser_no_start")) Then
              If Not String.IsNullOrEmpty(r.Item("amod_ser_no_start").ToString) Then
                serStart = r.Item("amod_ser_no_start").ToString.ToUpper.Trim
              End If
            End If

            If Not IsDBNull(r.Item("amod_ser_no_end")) Then
              If Not String.IsNullOrEmpty(r.Item("amod_ser_no_end").ToString) Then
                serEnd = r.Item("amod_ser_no_end").ToString.ToUpper.Trim
              End If
            End If

            If Not IsDBNull(r.Item("amod_ser_no_suffix")) Then
              If Not String.IsNullOrEmpty(r.Item("amod_ser_no_suffix").ToString) Then
                serSuf = r.Item("amod_ser_no_suffix").ToString.ToUpper.Trim
              End If
            End If

            serRange = serPre + serStart + serSuf

            If Not String.IsNullOrEmpty(serEnd) Then
              serRange += " - " + serPre + serEnd + serSuf + "&nbsp;"
            ElseIf Not String.IsNullOrEmpty(serStart) Then
              serRange += " &amp; Up&nbsp;"
            Else
              serRange += "&nbsp;"
            End If


            If Not IsDBNull(r.Item("amod_start_price")) Then
              If Not String.IsNullOrEmpty(r.Item("amod_start_price").ToString) Then
                startPrice = CDbl(r.Item("amod_start_price").ToString)
              End If
            End If

            If Not IsDBNull(r.Item("amod_end_price")) Then
              If Not String.IsNullOrEmpty(r.Item("amod_end_price").ToString) Then
                endPrice = CDbl(r.Item("amod_end_price").ToString)
              End If
            End If

            If startPrice <> 0 Then
              priceRange += "$" & FormatNumber((startPrice / 1000), 0, False, False, True) & "k"
            Else
              priceRange += "&nbsp;"
            End If

            If endPrice <> 0 Then
              priceRange += " - $ " & FormatNumber((endPrice / 1000), 0, False, False, True) & "k&nbsp;"
            Else
              priceRange += "&nbsp;"
            End If


            description_text.Text = "<a name=""desc""></a><h1>" & localCriteria.ViewCriteriaAircraftMake + " " + localCriteria.ViewCriteriaAircraftModel & "</h1>"

            If Not IsDBNull(r.Item("amod_manufacturer")) Then
              If Not String.IsNullOrEmpty(r.Item("amod_manufacturer").ToString) Then
                description_text.Text += "<span><strong>Manufacturer:</strong> " & r.Item("amod_manufacturer").ToString.Trim & "</span>"
              End If
            End If

            If Not String.IsNullOrEmpty(yearRange) Then
              description_text.Text += "<span><strong>Years Built:</strong> " & yearRange & "</span>"
            End If
            If Not String.IsNullOrEmpty(serRange) Then
              description_text.Text += "<span><strong>Ser # Range:</strong> " & serRange & "</span>"
            End If
            If Not String.IsNullOrEmpty(typeName) Then
              description_text.Text += "<span class=""display_inline_block float_left""><strong>Type:</strong> " & typeName & "</span>"
            End If

            If Not String.IsNullOrEmpty(weightName) Then
              description_text.Text += "<span class=""display_inline_block float_right padding_right""><strong>Weight Class:</strong> " & weightName & "</span>"
            End If
            If Not String.IsNullOrEmpty(priceRange) Then
              description_text.Text += "<span class=""div_clear""><strong>General Market Price Range:</strong> " & priceRange & "</span>"
            End If

            If Not IsDBNull(SharedModelTable.Rows(0).Item("amod_description")) Then
              If Not String.IsNullOrEmpty(SharedModelTable.Rows(0).Item("amod_description").ToString) Then
                description_text.Text += "<p>" & SharedModelTable.Rows(0).Item("amod_description").ToString.Trim & "</p>"
              End If
            End If

          Next


        End If
      End If
    Catch ex As Exception
      Call commonLogFunctions.Log_User_Event_Data("UserError", Replace(Request.Url.AbsolutePath, "/", "") & " : " & System.Reflection.MethodInfo.GetCurrentMethod().ToString.Trim & " " & Replace(" Exception Thrown[" + ex.Message.Trim, "'", "''"), Nothing, 0, 0, 0, 0, 0)
    End Try
  End Sub



End Class