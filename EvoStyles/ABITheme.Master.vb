Partial Public Class ABI
  Inherits System.Web.UI.MasterPage
  Public AbiDataManager As New abi_functions
  Public aclsData_Temp As New clsData_Manager_SQL
  Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
    AbiDataManager.adminConnectStr = Session.Item("jetnetAdminDatabase")
    aclsData_Temp.JETNET_DB = Session.Item("jetnetAdminDatabase")
    HttpContext.Current.Session.Item("localUser").crmUser_DebugText = ""

    If Application.Item("crmClientSiteData").WebSiteType = eWebSiteTypes.LOCAL Then
      debugLink.Visible = True
    Else
      If Not String.IsNullOrEmpty(Trim(Request("debug"))) Then
        If Trim(Request("debug")) = "Y" Then
          debugLink.Visible = True
        Else
          debugLink.Visible = False
        End If
      End If
    End If

  End Sub

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    'SelectMenu()

    'FillMarketTrendsLineChart()

    FillScroller()

    FillFooter()

    FillAds()
  End Sub

  Private Sub FillAds()
    If marketTrends.Visible = True Then 'Only run if visible
      Dim AdTable As New DataTable
      Dim Distinct_Table_View As New DataView
      Dim Distinct_Table As New DataTable

      AdTable = AbiDataManager.GetAds()

      If Not IsNothing(AdTable) Then
        If AdTable.Rows.Count > 0 Then
          ''create the view to get the distinct values.
          Distinct_Table_View = AdTable.DefaultView

          ''actually get the distinct values.
          Distinct_Table = Distinct_Table_View.ToTable(True, "abicserv_id", "abicserv_web_address")

          If Not IsNothing(Distinct_Table) Then
            If Distinct_Table.Rows.Count > 0 Then
              For Each r As DataRow In Distinct_Table.Rows
                adBannerLiteral.Text += "<a href="""
                If Not IsDBNull(r("abicserv_web_address")) Then
                  If InStr(r("abicserv_web_address"), "http://") > 0 Then
                    adBannerLiteral.Text += r("abicserv_web_address")
                  Else
                    adBannerLiteral.Text += "http://" & r("abicserv_web_address")
                    adBannerLiteral.Text += """ target=""new"">"
                  End If
                End If

                If HttpContext.Current.Session.Item("jetnetWebSiteType") = crmWebClient.eWebSiteTypes.LOCAL Then
                  adBannerLiteral.Text += "<img src='http://www.jetnetGlobal.com/photos/tileads/" & r("abicserv_id") & ".jpg' />"
                Else
                  adBannerLiteral.Text += "<img src='" + HttpContext.Current.Session.Item("jetnetFullHostName").ToString + HttpContext.Current.Session.Item("ABIPhotosFolderVirtualPath") + "/tileads/" & r("abicserv_id") & ".jpg' />" '"http://www.jetnetGlobal.com/photos/tileads"
                End If

                If Not IsDBNull(r("abicserv_web_address")) Then
                  adBannerLiteral.Text += "</a>"
                End If
                adBannerLiteral.Text += "<br /><br />"
              Next
            End If
          End If
        End If
      End If
    End If
  End Sub
  Public Sub ToggleMarketTrendsColumn(ByVal toggle As Boolean)
    marketTrends.Visible = toggle

  End Sub
  Private Sub FillFooter()
    FillCachedFooterDataset() 'Fill cache if it isn't there.

    If Not IsNothing(Cache("FillCachedFooterDataset")) Then
      Dim FooterDisplay As String = ""
      Dim FooterData As New DataTable
      FooterData = Cache("FillCachedFooterDataset")

      If Not IsNothing(FooterData) Then
        If FooterData.Rows.Count > 0 Then
          For Each r As DataRow In FooterData.Rows
            If FooterDisplay <> "" Then
              FooterDisplay += " - "
            End If
            FooterDisplay += "<a href=""" & abi_functions.AircraftMakeForSaleURL(r("amod_make_name"), r("amod_airframe_type_code"), r("amod_type_code")) & """>" & r("amod_make_name") & "</a>"

          Next
        End If
      End If

      FooterDisplay = "<br /><p>" & FooterDisplay & "</p>"

      footerModelList.Text = FooterDisplay
    End If

  End Sub
  Public Sub Set_Page_Title(ByVal pageTitle As String)
    'Setting PageTitle First
    Page.Header.Title = pageTitle
  End Sub
  Public Sub Set_Meta_Information(ByVal PageDescription As String, ByVal PageKeywords As String)

    Dim MetaDescription As New HtmlMeta()

    If Not IsNothing(Page.Header.FindControl("metaDescriptionCtl")) Then
      MetaDescription = Page.Header.FindControl("metaDescriptionCtl")
    End If

    MetaDescription.Name = "description"
    MetaDescription.ID = "metaDescriptionCtl"
    MetaDescription.Content = PageDescription


    If String.IsNullOrEmpty(MetaDescription.Content) Then
      MetaDescription.Content = "Aircraft for sale, planes for sale, helicopters for sale, including: Cessna, Gulfstream, Challenger, Hawker, and Learjet aircraft by Aircraft Dealers & Brokers."
    End If

    'Finally add the pagedescription.
    Page.Header.Controls.Add(MetaDescription)

    'Next we can go ahead and do the Meta Keywords
    Dim MetaKeywords As New HtmlMeta()

    If Not IsNothing(Page.Header.FindControl("metaKeywordsCtl")) Then
      MetaKeywords = Page.Header.FindControl("metaKeywordsCtl")
    End If

    ' (HtmlMeta)Page.Header.FindControl(“cntrlMetaKeywords”);
    MetaKeywords.Name = "keywords"
    MetaKeywords.ID = "metaKeywordsCtl"
    MetaKeywords.Content = PageKeywords


    'Check for blank and set default
    If String.IsNullOrEmpty(MetaKeywords.Content) Then
      MetaKeywords.Content = "aircraft for sale, jets for sale, turbo props for sale, helicopters for sale, aircraft wanteds, business jets, used aircraft, used planes, aircraft sale, abi, aviation, aircraft, fbo, dealer, news, aviation links, aviation events, aviation products, plane, airplane, Cessna, gulfstream, hawker, learjet, lear jet, jetnet"
    End If

    'Finally add the pagedescription.
    Page.Header.Controls.Add(MetaKeywords)
  End Sub
  Sub FillCachedFooterDataset()
    'This function can be cached once a day.
    If IsNothing(HttpContext.Current.Cache("FillCachedFooterDataset")) Then
      HttpContext.Current.Cache.Insert("FillCachedFooterDataset", AbiDataManager.GetABIAircraftDistinctList("amod_make_name, amod_airframe_type_code, amod_type_code"), Nothing, DateTime.Now.AddDays(1), Cache.NoSlidingExpiration)
    End If
  End Sub

  Private Sub FillScroller()
    Dim ScrollerTable As New DataTable
    Dim DisplayText As String = ""
    Dim css As String = "red"
    Dim arrow As String = ""
    ScrollerTable = AbiDataManager.ScrollerStatsQuery()

    DisplayText = "<ul class=""bxslider"">"

    If Not IsNothing(ScrollerTable) Then
      If ScrollerTable.Rows.Count > 0 Then
        For Each r As DataRow In ScrollerTable.Rows
          Dim DisplayLargeNumber As Double = 0
          Dim DisplayMonth As Double = 0
          Dim DisplayYear As Double = 0

          'If the number for sale now is greater than in the past month then do an up arrow on green background – the color below looks pretty good.
          If r("currentforsale") > r("pastmonthforsale") Then
            css = "green"
            arrow = " &#8593; "
          ElseIf r("currentforsale") = r("pastmonthforsale") Then
            css = "orange"
            arrow = " &#8596; "
          Else
            css = "red"
            arrow = " &#8595; "
          End If

          DisplayLargeNumber = r("currentforsale") - r("pastmonthforsale")
          DisplayText += "<li><div class=""placerBox " & css & """><span class=""pull-left"">" & r("amod_make_name") & arrow & DisplayLargeNumber & "<br /><span class=""sliderDarkGrayText text_align_center"">AIRCRAFT FOR SALE</span></span>"

          If r("pastmonthforsale") > 0 Then
            DisplayMonth = FormatNumber(((r("currentforsale") - r("pastmonthforsale")) / r("pastmonthforsale")) * 100, 2)
          End If
          If r("pastyearforsale") > 0 Then
            DisplayYear = FormatNumber(((r("currentforsale") - r("pastyearforsale")) / r("pastyearforsale")) * 100, 2)
          End If

          DisplayText += "<span class=""pull-left sliderGrayText LeftBuffer"">" & DisplayMonth & "% Last Month<br />"
          DisplayText += "" & DisplayYear & "% Last Year</span></div></li>"

          If css = "red" Then
            css = "green"
          Else
            css = "red"
          End If
        Next
      End If
    End If

    DisplayText += "</ul>"
    scroller.Text = DisplayText

  End Sub
  'Private Sub SelectMenu()
  '  Dim callingFile As String = ""
  '  callingFile = Replace(Request.ServerVariables("SCRIPT_NAME"), ".aspx", "")
  '  callingFile = Replace(callingFile, "/abiFiles/", "")
  '  callingFile = UCase(callingFile)

  '  ResetMenu()

  '  Select Case callingFile
  '    Case "DEFAULTABI"
  '      homeLink.Attributes.Add("class", "current active")
  '    Case "ABIDEALER"
  '      dealersLink.Attributes.Add("class", "current active")
  '    Case "ABIWANTEDS"
  '      wantedLink.Attributes.Add("class", "current active")
  '    Case "ABIPRODUCTS"
  '      productLink.Attributes.Add("class", "current active")
  '    Case "ABINEWS"
  '      newsLink.Attributes.Add("class", "current active")
  '    Case "ABILINKS"
  '      linksLink.Attributes.Add("class", "current active")
  '    Case "ABIEVENTS"
  '      eventsLink.Attributes.Add("class", "current active")
  '    Case "ABIFORSALE"
  '      ACSaleLink.Attributes.Add("class", "current active")
  '  End Select
  'End Sub

  Private Sub ResetMenu()
    homeLink.Attributes.Remove("class")
    ACSaleLink.Attributes.Remove("class")
    wantedLink.Attributes.Remove("class")
    productLink.Attributes.Remove("class")
    newsLink.Attributes.Remove("class")
    linksLink.Attributes.Remove("class")
    dealersLink.Attributes.Remove("class")
    eventsLink.Attributes.Remove("class")

    'add submenu class
    ACSaleLink.Attributes.Add("class", "deeper dropdown parent")
    wantedLink.Attributes.Add("class", "deeper dropdown parent")
  End Sub


  Private Sub FillMarketTrendsLineChart()
    Dim MarketScript As StringBuilder = New StringBuilder()
    Dim marketTrends As New DataTable
    Dim count As Integer = 0
    marketTrends = GetMarketTrends()

    If Not Page.ClientScript.IsClientScriptBlockRegistered("Market") Then
      MarketScript.Append("<script type=""text/javascript"">")

      MarketScript.Append("function drawChart() {")
      MarketScript.Append("var data = google.visualization.arrayToDataTable([")
      MarketScript.Append("['Month', 'For Sale'],")

      For Each m As DataRow In marketTrends.Rows
        If count > 0 Then
          MarketScript.Append(",")
        End If
        MarketScript.Append("['" & m("mtrend_month") & "/" & m("mtrend_year") & "',  " & m("tcount") & "]")
        count += 1
      Next



      MarketScript.Append("]);")

      MarketScript.Append(" var options = {")
      MarketScript.Append("title:  'Worldwide Aircraft',")
      MarketScript.Append("curveType:  'function',")
      MarketScript.Append("legend: { position: 'bottom' }")
      MarketScript.Append("};")

      MarketScript.Append("var chart = new google.visualization.LineChart(document.getElementById('curve_chart'));")

      MarketScript.Append("chart.draw(data, options);")
      MarketScript.Append("}")
      MarketScript.Append("</script>")
      Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "Market", MarketScript.ToString, False)
    End If


  End Sub

  ''' <summary>
  ''' Dataquery for Market Trends Line Chart.
  ''' </summary>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function GetMarketTrends() As DataTable
    '-- TREND CHART FOR RIGHT HAND SIDE OF HOME PAGE - WORLDWIDE AIRCRAFT FOR SALE

    Dim sqlQuery As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim atemptable As New DataTable

    Try

      sqlQuery = "select distinct mtrend_year, mtrend_month, sum(mtrend_total_aircraft_for_sale) as tcount "
      sqlQuery += " from aircraft_model_trend "
      sqlQuery += " where (mtrend_year >= Year(getdate()) - 1)"
      sqlQuery += " and not ((mtrend_year = year(getdate())) and (mtrend_month = month(getdate())))"
      sqlQuery += " group by mtrend_year, mtrend_month"
      sqlQuery += " order by mtrend_year, mtrend_month"


      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>GetMarketTrends() As DataTable</b><br />" & sqlQuery

      SqlConn.ConnectionString = Session.Item("jetnetAdminDatabase")
      SqlConn.Open()
      SqlCommand.Connection = SqlConn


      SqlCommand.CommandText = sqlQuery
      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
      End Try

      GetMarketTrends = atemptable
    Catch ex As Exception
      GetMarketTrends = Nothing
      'Me.class_error = "Error in GetMarketTrends() As DataTable: " & ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

  End Function
End Class